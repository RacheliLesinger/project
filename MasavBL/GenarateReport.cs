using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MasavBL
{
    public class GenarateReport
    {
        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static async Task<GenerateReportRes> GenerateReport(string year, string month, int dayInMonth,
            int customerId, int payingClass, bool overrideFile, bool isOverride)
        {
            log.Info($"CreateMasavReport, customerId: {customerId} ");

            var chiyuvDate = new DateTime(Int32.Parse(year), Int32.Parse(month), dayInMonth);
            var fileName = DB.GetCustomerCode(customerId) + ".txt";
            string filePath = Properties.Settings.Default.ReportPath + "\\" + year + "\\" + month + "\\";
            Directory.CreateDirectory(filePath);
            FileInfo info = new FileInfo(filePath + fileName);
            if (info.Exists && overrideFile != true)
                return new GenerateReportRes(string.Empty, false, "File already Exsist");
            if (CreateMasavReport(filePath + fileName, dayInMonth, chiyuvDate,customerId, Int32.Parse(year), Int32.Parse(month), payingClass))
            {
                
                //יצירת רשומות בטבלת היסטורית תשלומים
                var phRes = await DB.AddPaymentHistory(dayInMonth, customerId,payingClass, Int32.Parse(year), Int32.Parse(month), isOverride);
                //יצירת הרשומה בטבלת היסטורית שידורים
                if (phRes.AmountSum != 0)
                {
                    bool bhRes = await DB.AddBrodcastHistory(chiyuvDate, phRes.AmountSum, customerId,payingClass, phRes.SumRecord, phRes.SumNewRecord);
                    if (bhRes)
                        return new GenerateReportRes(filePath + fileName, true, null);
                }
                else
                {
                    return new GenerateReportRes(string.Empty, true, "AmountSum is 0");
                }
            }
            return new GenerateReportRes(string.Empty, false, "CreateMasavReport fail");
        }

        public static string GetKOT(DateTime chiyuvDate,string customerCode, Models.Institution institution)
        {
            // כותרת
            var createdDate = DateTime.Now.ToString("yyMMdd");
            string mosad = customerCode; //Properties.Settings.Default.Mosad; // "09376013";
            string chiyuvDateStr = chiyuvDate.ToString("yyMMdd"); //תאריך חיוב 
            string mosadSholeach = institution.Code; //Properties.Settings.Default.MosadNum; // "00004"; //מוסד שולח    
            string mosadName = ConversionTable.ConvertFromHebrew(institution.Name); //Properties.Settings.Default.ShemMosad;// "D&TD OELQ - DXEAC OIXTLD"; // שם מוסד  

            string KOT = "K" + mosad + "00" + chiyuvDateStr + "0" + "001" + "0" + createdDate + mosadSholeach + "000000" + mosadName.PadLeft(30, ' ') + "KOT".PadLeft(59, ' ');
            if (KOT.Length == 128)
                return KOT;
            return null;

        }

        public static string GetEndLine(double amountSum, int sumRecord, DateTime chiyuvDate, string customerCode)
        {
            // רשומת סה"כ
            string mosad = customerCode; //Properties.Settings.Default.Mosad;
            string chiyuvDateStr = chiyuvDate.ToString("yyMMdd"); //תאריך חיוב

            string num7 = "".PadLeft(15, '0'); //בהוראות כתוב 15 אפסים ובדוגמא זה לא מופיע
            string num9 = "".PadLeft(7, '0');
            string sumReshumot = string.Format("{0:N2}", amountSum).Replace(".", "").Replace(",", ""); // סכום רשומות התנועה
            string countReshumot = string.Format("{0}", sumRecord).Replace(".", "").Replace(",", ""); //מספר רשומות התנועה


            //string KOT = "5" + mosad + "00" + chiyuvDateStr + "0" + "001" + num7 + sumReshumot.PadLeft(15, '0') + num9 +
            //               countReshumot.PadLeft(7, '0') + "".PadLeft(63, ' ');
            string KOT = "5" + mosad + "00" + chiyuvDateStr + "0" + "001"  +"".PadLeft(15, '0') + sumReshumot.PadLeft(15, '0') +
                           "".PadLeft(7, '0') + countReshumot.PadLeft(7, '0') +  "".PadLeft(63, ' ');
            if (KOT.Length == 128)
                return KOT;
            return null;

        }

        public static string GetReshuma(string KodBank, string MisparSnif, string MisparCheshbon, string payingIdentityNumber,
            string customerName, double amount,string customerCode)
        {
            // רשומת תנועה
            string mosad = customerCode; // Properties.Settings.Default.Mosad;
            string sugCheshbon = "0000"; // סוג חשבון
            string newCustomerName = ConversionTable.ConvertFromHebrew(customerName); //"DXETV ODK"; 
            if (newCustomerName.Length > 16)
                newCustomerName = newCustomerName.Substring(0, 16);
            string amountStr = string.Format("{0:N2}", amount).Replace(".", "").Replace(",","");
            string mosadIdentity = payingIdentityNumber; // מס מזהה ללקוח במוסד 
            if (mosadIdentity.Length > 9)
                mosadIdentity = mosadIdentity.Substring(0, 9);
            string tkufatChiyuv = "00000000"; // תקופת חיוב
            string sugTnua = "504"; // סוג תנועה - 504



            string res = "1" + mosad + "00" + "000000" + KodBank + MisparSnif.PadLeft(3,'0') + sugCheshbon 
                       + MisparCheshbon.PadLeft(9,'0') 
                       + "0" + payingIdentityNumber.PadLeft(9,'0')  +newCustomerName.PadLeft(16, ' ') 
                       + amountStr.PadLeft(13, '0') + mosadIdentity.PadLeft(20, '0') + tkufatChiyuv 
                       + "000" + sugTnua + "".PadLeft(18, '0') + "".PadLeft(2, ' ');
            if (res.Length == 128)
                return res;
            return null;

        }

        public static bool CreateMasavReport(string filePath, int dayInMonth, DateTime chiyuvDate, int customerId, int year, int month, int payingClass)
        {
            try
            {
                FileInfo info = new FileInfo(filePath);
                using (StreamWriter writer = info.CreateText())
                {
                    var customerCode = DB.GetCustomerCode(customerId);
                    var institution = DB.GetInstitutionByCustomerId(customerId);
                    writer.WriteLine(GetKOT(chiyuvDate, customerCode, institution));
                    var list = DB.GetPayingsToReport(dayInMonth, customerId, year, month, payingClass);
                    list.ForEach( i =>
                    {
                        if (i.CodeBank?.Code.Length == 1)
                            i.CodeBank.Code = "0" + i.CodeBank.Code;
                    });
                    list = list.OrderBy(i => i.CodeBank?.Code).ThenBy(i => i.BankBranchNumber).ThenBy(i => i.BankAccountNumber).ToList();

                    foreach (var item in list)
                    {
                        writer.WriteLine(GetReshuma(item.CodeBank?.Code, item.BankBranchNumber,
                               item.BankAccountNumber, item.IdentityNumber, item.Name,
                               Convert.ToDouble(item.Amount),customerCode));
                    }
                    var res = DB.GetSumResultOfPayment(dayInMonth, customerId);
                    writer.WriteLine(GetEndLine(res.AmountSum, res.SumRecord,chiyuvDate,customerCode));
                    //writer.WriteLine("".PadLeft(128, '9'));
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Error($"Exception in CreateMasavReport, customerId: {customerId} filePath: {filePath} , ex: {ex.Message}");
                return false;
            }

        }
    }

    public class GenerateReportRes
    {
        public GenerateReportRes(string filePath, bool success, string err)
        {
            this.FilePath = filePath;
            this.Success = success;
            this.ErrorMessage = err;
        }
        public string FilePath { get; set; }

        public int? CountUpdatedRows { get; set; }
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}
