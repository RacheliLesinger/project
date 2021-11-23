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

        public static async Task<GenerateReportRes> GenerateReport(string year, string month, int dayInMonth, int customerId, bool overrideFile)
        {
            var date = DateTime.Now;
            var chiyuvDate = new DateTime(Int32.Parse(year), Int32.Parse(month), dayInMonth);
            var fileName = DB.GetCustomerName(customerId) + " " + date.ToString("yyMMdd") + ".txt";
            string filePath = Properties.Settings.Default.ReportPath +
               /* System.Environment.CurrentDirectory +*/ "\\" + year + "\\" + month + "\\";
            Directory.CreateDirectory(filePath);
            FileInfo info = new FileInfo(filePath + fileName);
            if (info.Exists && overrideFile != true)
                return new GenerateReportRes(string.Empty, false, "File already Exsist");
            if (CreateMasavReport(filePath + fileName, dayInMonth, chiyuvDate,customerId))
            {
                //יצירת רשומות בטבלת היסטורית תשלומים
                var phRes = await DB.AddPaymentHistory(dayInMonth, customerId);
                //יצירת הרשומה בטבלת היסטורית שידורים
                if (phRes.AmountSum != 0)
                {
                    bool bhRes = await DB.AddBrodcastHistory(phRes.AmountSum, customerId, phRes.SumRecord, phRes.SumNewRecord);
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

        public static string GetKOT(DateTime chiyuvDate)
        {
            // כותרת
            var createdDate = DateTime.Now.ToString("yyMMdd");
            string mosad = Properties.Settings.Default.Mosad; // "09376013";
            string chiyuvDateStr = chiyuvDate.ToString("yyMMdd"); //תאריך חיוב 
            string mosadSholeach = Properties.Settings.Default.MosadNum; // "00004"; //מוסד שולח    
            string mosadName = Properties.Settings.Default.ShemMosad;// "D&TD OELQ - DXEAC OIXTLD"; // שם מוסד  

            string KOT = "K" + mosad + "00" + chiyuvDateStr + "0" + "001" + "0" + createdDate + mosadSholeach + "000000" + mosadName.PadLeft(30, ' ') + "KOT".PadLeft(59, ' ');
            if (KOT.Length == 128)
                return KOT;
            return null;

        }

        public static string GetEndLine(int amountSum, int sumRecord, DateTime chiyuvDate)
        {
            // רשומת סה"כ
            string mosad = Properties.Settings.Default.Mosad;
            string chiyuvDateStr = chiyuvDate.ToString("yyMMdd"); //תאריך חיוב

            string num7 = "".PadLeft(15, '0'); //בהוראות כתוב 15 אפסים ובדוגמא זה לא מופיע
            string num9 = "".PadLeft(7, '0');
            string sumReshumot = string.Format("{0:N2}", amountSum).Replace(".", "").Replace(",", ""); // סכום רשומות התנועה
            string countReshumot = string.Format("{0}", sumRecord).Replace(".", "").Replace(",", ""); //מספר רשומות התנועה


            //string KOT = "5" + mosad + "00" + chiyuvDateStr + "0" + "001" + num7 + sumReshumot.PadLeft(15, '0') + num9 +
            //               countReshumot.PadLeft(7, '0') + "".PadLeft(63, ' ');
            string KOT = "5" + mosad + "00" + chiyuvDateStr + "0" + "001" + sumReshumot.PadLeft(15, '0') + "".PadLeft(15, '0') +
                           countReshumot.PadLeft(7, '0') + "".PadLeft(7, '0') + "".PadLeft(63, ' ');
            if (KOT.Length == 128)
                return KOT;
            return null;

        }

        public static string GetReshuma(string KodBank, string MisparSnif, string MisparCheshbon, string payingIdentityNumber,
            string customerName, double amount)
        {
            // רשומת תנועה
            string mosad = Properties.Settings.Default.Mosad;
            string sugCheshbon = "0000"; // סוג חשבון
            string newCustomerName = ConversionTable.ConvertFromHebrew(customerName); //"DXETV ODK"; 

            string amountStr = string.Format("{0:N2}", amount).Replace(".", "").Replace(",","");
            string mosadIdentity = payingIdentityNumber; // מס מזהה ללקוח במוסד 
            string tkufatChiyuv = "00000000"; // תקופת חיוב
            string sugTnua = "006"; // סוג תנועה - 504



            string res = "1" + mosad + "00" + "000000" + KodBank + MisparSnif.PadLeft(3,'0') + sugCheshbon 
                       + MisparCheshbon.PadLeft(9,'0') 
                       + "0" + payingIdentityNumber.PadLeft(9,'0')  +newCustomerName.PadLeft(16, ' ') 
                       + amountStr.PadLeft(13, '0') + mosadIdentity.PadLeft(20, '0') + tkufatChiyuv 
                       + "000" + sugTnua + "".PadLeft(18, '0') + "".PadLeft(2, ' ');
            if (res.Length == 128)
                return res;
            return null;

        }

        public static bool CreateMasavReport(string filePath, int dayInMonth, DateTime chiyuvDate, int customerId)
        {
            try
            {
                FileInfo info = new FileInfo(filePath);
                using (StreamWriter writer = info.CreateText())
                {
                    writer.WriteLine(GetKOT(chiyuvDate));
                    foreach (var item in DB.GetPayingsToReport(dayInMonth, customerId))
                    {
                        writer.WriteLine(GetReshuma(item.CodeBank?.Code, item.BankBranchNumber,
                               item.BankAccountNumber, item.IdentityNumber, item.Name,
                               Convert.ToDouble(item.Amount)));
                    }
                    var res = DB.GetSumResultOfPayment(dayInMonth, customerId);
                    writer.WriteLine(GetEndLine(res.AmountSum, res.SumRecord,chiyuvDate));
                    writer.WriteLine("".PadLeft(128, '9'));
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
        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}
