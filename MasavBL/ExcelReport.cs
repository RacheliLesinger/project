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

        public static GenerateReportRes ImportFromExcel(Stream stream)
        {
            var res = new GenerateReportRes(string.Empty, false, "");
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                    int rowCount = worksheet.Dimension.End.Row;     //get row count
                    for (int row = 1; row <= rowCount; row++)
                    {
                        var p = new Paying();
                        p.Name = worksheet.Cells[row, 1].Value?.ToString();
                        p.IdentityNumber = worksheet.Cells[row, 2].Value?.ToString();
                        p.PaymentDate = (int)worksheet.Cells[row, 3].Value;
                        p.PaymentSum = (int)worksheet.Cells[row, 4].Value;
                        p.Amount = (int)worksheet.Cells[row, 5].Value;
                        p.CodeBankId = (int)worksheet.Cells[row, 6].Value;
                        p.BankBranchNumber = worksheet.Cells[row, 7].Value?.ToString();
                        p.BankAccountNumber = worksheet.Cells[row, 8].Value?.ToString();
                        p.StartDate =Convert.ToDateTime(worksheet.Cells[row, 9].Value?.ToString());
                        p.EndDate = Convert.ToDateTime(worksheet.Cells[row, 10].Value?.ToString());

                       var s = DB.UpdatePayings(p);
                    }
                }

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




