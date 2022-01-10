using MasavBL.Models;
using OfficeOpenXml;
using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MasavBL
{
    public class ExcelReport
    {
        public static GenerateReportRes ExportToExcel(List<Paying> usersList)
        {
            var list = DB.GetPayings();
            var res = new GenerateReportRes(string.Empty, false, "");
            try
            {
                var stream = new MemoryStream();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("משלמים");
                    workSheet.View.RightToLeft = true;
                    workSheet.Cells.LoadFromCollectionFiltered(usersList, true);
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    workSheet.Column(13).Style.Numberformat.Format = "dd-mm-yyyy";
                    workSheet.Column(14).Style.Numberformat.Format = "dd-mm-yyyy";

                    workSheet.Row(1).Style.Font.Bold = true;
                    workSheet.Row(1).Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    workSheet.Row(1).Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    workSheet.View.FreezePanes(2, 1);

                    package.Save();
                }
                stream.Position = 0;
                string excelName = $"רשימת משלמים -{DateTime.Now.ToString("yyyyMMddHHmm")}.xlsx";
                var filePath = Properties.Settings.Default.ExcelReportPath + @"\" + excelName;
                using (FileStream file = new FileStream(filePath, FileMode.Create, System.IO.FileAccess.Write))
                {
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    file.Write(bytes, 0, bytes.Length);
                    stream.Close();
                }
                res.FilePath = filePath;
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
                Console.WriteLine(ex.Message);
                throw;
            }
            return res;
        }

        public static GenerateReportRes ImportFromExcel(Stream stream, Customer customer, bool removeNotExsist)
        {
            var res = new GenerateReportRes(string.Empty, false, "");
            try
            {
                var exsistingPayings = DB.GetPayings(customer.Id);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.End.Row;//get row count
                    int row = 4;
                    try
                    {
                        for (; row <= rowCount; row++)
                        {
                            var identityNumber = worksheet.Cells[row, 1].Value?.ToString();
                            if (identityNumber == string.Empty || identityNumber == null)
                                break;
                            var exsist = exsistingPayings.FirstOrDefault(i => i.IdentityNumber == identityNumber);
                            var p = new Paying();
                            if (exsist != null)
                                p = exsist;
                            p.IdentityNumber = worksheet.Cells[row, 1].Value?.ToString();
                            p.Name = worksheet.Cells[row, 2].Value?.ToString();
                            p.CodeBankId = DB.GetBankIdByCode(worksheet.Cells[row, 3].Value?.ToString());
                            p.BankBranchNumber = worksheet.Cells[row, 4].Value?.ToString();
                            p.BankAccountNumber = worksheet.Cells[row, 5].Value?.ToString();
                            p.Amount = Int32.Parse(worksheet.Cells[row, 6].Value?.ToString());
                            if (exsist == null)//משלם חדש - הגדרות ברירת מחדל
                            {
                                p.CustomerId = customer.Id;
                                p.PaymentDate = (int)customer.PaymentDate1;
                                p.PaymentSum = -1;
                                p.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                                p.EndDate = DateTime.MaxValue;
                                p.CurrencyId = 1; //שקל
                                p.ActivityId = 1; //פעיל
                                p.IsNew = true;
                            }
                            exsistingPayings.RemoveAll(i => i.IdentityNumber == identityNumber);
                            var s = DB.UpdatePayings(p);
                            if (s == false)
                            {
                                
                                res.ErrorMessage = "קימת בעיה בשורה מספר " + row + " ";
                                if (p.CodeBankId == 0)
                                    res.ErrorMessage += " לא קיים קוד בנק כזה במסד הנתונים";
                                return res;
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        res.ErrorMessage = "קימת בעיה בשורה מספר "+ row + " ";
                        throw;
                    }
                    res.CountUpdatedRows = row -3;
                    if (removeNotExsist == true)
                    {
                        foreach (var item in exsistingPayings)
                        {
                            item.ActivityId = 3; //מחוק
                            var s = DB.UpdatePayings(item);
                        }
                    }
                }
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.ErrorMessage += ex.Message;
                Console.WriteLine(ex.Message);
                //throw;
            }
            return res;
        }

        public static GenerateReportRes ExportCostumersToExcel()
        {
            var list = DB.GetCustomers();
            var res = new GenerateReportRes(string.Empty, false, "");
            try
            {
                var stream = new MemoryStream();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(stream))
                {
                    var workSheet = package.Workbook.Worksheets.Add("לקוחות");
                    workSheet.View.RightToLeft = true;
                    workSheet.Cells.LoadFromCollectionFiltered(list, true);
                    workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
                    workSheet.Column(13).Style.Numberformat.Format = "dd-mm-yyyy";
                    workSheet.Column(14).Style.Numberformat.Format = "dd-mm-yyyy";

                    workSheet.Row(1).Style.Font.Bold = true;
                    workSheet.Row(1).Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    workSheet.Row(1).Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    workSheet.View.FreezePanes(2, 1);

                    package.Save();
                }
                stream.Position = 0;
                string excelName = $"רשימת לקוחות -{DateTime.Now.ToString("yyyyMMddHHmm")}.xlsx";
                var filePath = @"C:\" + excelName;
                using (FileStream file = new FileStream(@"C:\log\" + excelName, FileMode.Create, System.IO.FileAccess.Write))
                {
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    file.Write(bytes, 0, bytes.Length);
                    stream.Close();
                }
                res.FilePath = filePath;
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.ErrorMessage = ex.Message;
                Console.WriteLine(ex.Message);
                throw;
            }
            return res;
        }
    }
    public static class Extensions
    {
        public static ExcelRangeBase LoadFromCollectionFiltered<T>(this ExcelRangeBase @this,
                                       IEnumerable<T> collection, bool printHeader) where T : class
        {
            MemberInfo[] membersToInclude = typeof(T)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(p => !Attribute.IsDefined(p, typeof(EpplusIgnore)))
                .ToArray();

            return @this.LoadFromCollection<T>(collection, printHeader,
                OfficeOpenXml.Table.TableStyles.None,
                BindingFlags.Instance | BindingFlags.Public,
                membersToInclude);
        }
    }
}




