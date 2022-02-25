using Codaxy.WkHtmlToPdf;
using log4net;
using MasavBL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MasavBL
{
    public class PdfReport
    {

        private static readonly ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //public static async Task<GenerateReportRes> GenerateReceiptsReport(DateTime fromDate, DateTime toDate, int customerId, bool overrideFile)
        //{
        //    var date = DateTime.Now;

        //    var fileName = DB.GetCustomerName(customerId) + " " + date.ToString("yyMMdd") + ".txt";
        //    string filePath = Properties.Settings.Default.ReportPath;
        //    // "\\" + year + "\\" + month + "\\";
        //    Directory.CreateDirectory(filePath);
        //    FileInfo info = new FileInfo(filePath + fileName);
        //    if (info.Exists && overrideFile != true)
        //        return new GenerateReportRes(string.Empty, false, "File already Exsist");

        //    var list = DB.GetPaymentHistory(customerId, fromDate, toDate);
        //    var customer = DB.GetCustomerName(customerId);

        //    var res = CreatePdfFile(list, customer, fromDate, toDate);
        //    if (!res)
        //        return new GenerateReportRes(string.Empty, false, "GenerateReceiptsReport fail");
        //    return new GenerateReportRes(filePath + fileName, true, string.Empty);
        //}

        //public static bool CreatePdfFile(List<PaymentHistory> list, string customer, DateTime fromDate, DateTime toDate)
        //{
        //    try
        //    {
        //        int yPoint = 0;
        //        PdfSharp.Pdf.PdfDocument pdf = new PdfSharp.Pdf.PdfDocument();
        //        PdfSharp.Pdf.PdfPage pdfPage = pdf.AddPage();
        //        XGraphics graph = XGraphics.FromPdfPage(pdfPage);
        //        XFont font = new XFont("Verdana", 15, XFontStyle.Regular);
        //        graph.DrawString($" דוח קבלות עבור {customer} מתאריך {fromDate} עד תאריך {toDate}", font, XBrushes.Black,
        //                    new XRect(0, 0, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);
        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            yPoint = yPoint + 40;
        //            var row = string.Format($"{list[0].CustomerId } PaymentAmount: {list[0].PaymentAmount}");
        //            graph.DrawString(row, font, XBrushes.Black,
        //                        new XRect(40, yPoint, pdfPage.Width.Point, pdfPage.Height.Point), XStringFormats.TopRight);

        //        }
        //        string pdfFilename = "dbtopdf.pdf";
        //        pdf.Save(pdfFilename);
        //        Process.Start(pdfFilename);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}


        public static async Task<GenerateReportRes> GenerateBroadcastReport(int year, int month,int dayInMonth,
            int customerId,int payingClass, bool overrideFile)
        {
            var date = DateTime.Now;
            var pClass = payingClass == 0 ? "" : " " + payingClass;
            var fileName = customerId + "_" + date.ToString("yyMMdd") + ".pdf";
           
            string filePath = Properties.Settings.Default.PdfReportPath + "\\" + year + "\\" + month + "\\";

            //string filePath = Properties.Settings.Default.PdfReportPath;
            Directory.CreateDirectory(filePath);
            FileInfo info = new FileInfo(filePath + fileName);
            if (info.Exists && overrideFile != true)
                return new GenerateReportRes(string.Empty, false, "File already Exsist");

            var list = DB.GetPayingsToReport(dayInMonth, customerId, year,month,payingClass);
            var customer = DB.GetCustomerName(customerId);

            var dt = new DateTime(year, month, dayInMonth);
            var repPath = CreatePdf(list,customer, filePath + "\\" + fileName, dt);
            var newFileName = filePath + "\\" + customer + pClass + " " + date.ToString("yyMMdd") + ".pdf";
            if (File.Exists(newFileName))
            {
                File.Delete(newFileName);
            }
            File.Move(repPath, newFileName);
            return new GenerateReportRes(newFileName, true, string.Empty);
        }

        public static string CreatePdf(List<Paying> list, string customer ,string filePath, DateTime reportDate)
        {
            foreach (var item in list)
                item.IsNew = DB.IsNewPaying(item);

            var htmlData = File.ReadAllText(Environment.CurrentDirectory + "..\\..\\..\\..\\MasavBL\\‏‏HTMLbroadcastReportNew.html");
            var htmlPath = Path.Combine(Properties.Settings.Default.PdfReportPath, "ReplayMessage.html");
            htmlData = htmlData.Replace("customer_name", customer);
            htmlData = htmlData.Replace("madad", "     ");
            htmlData = htmlData.Replace("dolar", "     ");
            htmlData = htmlData.Replace("report_date", reportDate.ToShortDateString());
            htmlData = htmlData.Replace(@"<tr><td>addRow</td></tr>", InsertRows(list));
            htmlData = htmlData.Replace("sum_record", InsertSumRecord(list));
            htmlData = htmlData.Replace("sum_amount", list.Sum(i => i.Amount).Value.ToString());
            File.WriteAllText(htmlPath, htmlData, Encoding.UTF8);
       
            PdfConvert.ConvertHtmlToPdf(new Codaxy.WkHtmlToPdf.PdfDocument
            {
                Url = htmlPath,
                //HeaderRight = "מעלה אדומים משופר",
                HeaderLeft = "[date] ",
                FooterCenter = " [page] ", //- [topage]",
                HeaderFontSize = "8"

            }, new PdfOutput
            {
                OutputFilePath = filePath
            });
            return filePath;
        }

        private static string InsertSumRecord(List<Paying> list)
        {
            var res = @"סה""כ רשומות = " + list.Count();
            var sumNew = 0;
            var sumAmount = 0;
            foreach (var item in list)
            {
                if (item.IsNew)
                  sumNew++;
                sumAmount += (int)item.Amount;
            }
            res += @"&emsp;&emsp; סה""כ חיובים חדשים = " + sumNew;
            res += @"&emsp;&emsp; סה""כ לדוח  ";
            return res;
        }

        private static string InsertRows(List<Paying> list)
        {
            var res = string.Empty;
            foreach(var item in list)
            {
                res += InsertRow(item);
            }
            return res;
        }

        private static string InsertRow(Paying item)
        {
           var row = @"<tr>
             <td> identity </td>
             <td> new </td>
             <td> paying_name </td>
             <td> &nbsp; </td>
             <td> bank </td>
             <td> branch_num </td>
             <td> account </td>
             <td> sum </td>
             <td> madad </td>
             <td> amount </td>
             </tr> ";
            row = row.Replace("identity", item.IdentityNumber); 
            row = row.Replace("new", item.IsNew == true ? "ח":"");
            row = row.Replace("paying_name", item.Name);
            row = row.Replace("bank", item.CodeBank?.Name);
            row = row.Replace("branch_num", item.BankBranchNumber);
            row = row.Replace("account", item.BankAccountNumber);
            row = row.Replace("sum", item.Amount.ToString());
            row = row.Replace("madad", "");
            row = row.Replace("amount", item.Amount.ToString());
            return row;
        }


        //Not in use

        //private static bool CreatePdfFileNew(List<Paying> list, string customer)
        //{
        //    var htmlData = File.ReadAllText(Environment.CurrentDirectory + "..\\..\\..\\..\\MasavBL\\HTMLbroadcastReport.html");
        //    htmlData = htmlData.Replace("customer_name", customer);
        //    htmlData = htmlData.Replace("amount", "בדיקה");
        //    htmlData = htmlData.Replace("madad", "מדד א");
        //    htmlData = htmlData.Replace("report_date", DateTime.Now.ToShortDateString());

        //    // step 1
        //    Document document = new Document();
        //    // step 2
        //    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream("MySamplePDFNew.pdf", FileMode.Create));
        //    // step 3
        //    document.Open();
        //    // step 4
        //    // Styles
        //    ICSSResolver cssResolver = new StyleAttrCSSResolver();
        //    XMLWorkerFontProvider fontProvider = new XMLWorkerFontProvider(XMLWorkerFontProvider.DONTLOOKFORFONTS);
        //    var fontPath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\tahoma.ttf";
        //    fontProvider.Register(fontPath);
        //    CssAppliers cssAppliers = new CssAppliersImpl(fontProvider);
        //    HtmlPipelineContext htmlContext = new HtmlPipelineContext(cssAppliers);
        //    htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());

        //    // Pipelines
        //    PdfWriterPipeline pdf = new PdfWriterPipeline(document, writer);
        //    HtmlPipeline html = new HtmlPipeline(htmlContext, pdf);
        //    CssResolverPipeline css = new CssResolverPipeline(cssResolver, html);

        //    // XML Worker
        //    XMLWorker worker = new XMLWorker(css, true);
        //    XMLParser p = new XMLParser(worker);
        //    p.Parse(new FileStream(Environment.CurrentDirectory + "..\\..\\..\\..\\MasavBL\\HTMLbroadcastReport1.html",FileMode.Open), false); ;
        //    // step 5
        //    document.Close();
        //    return true;
        //}

        //private static bool CreatePdfFile(List<Paying> list, string customer)
        //{
        //    try
        //    {

        //        var htmlData = File.ReadAllText(Environment.CurrentDirectory + "..\\..\\..\\..\\MasavBL\\HTMLbroadcastReport.html");
        //        htmlData = htmlData.Replace("customer_name", customer);
        //        htmlData = htmlData.Replace("amount", "בדיקה");
        //        htmlData = htmlData.Replace("madad", "מדד א");
        //        htmlData = htmlData.Replace("report_date", DateTime.Now.ToShortDateString());

        //        StyleSheet styles = new StyleSheet();
        //        styles.LoadTagStyle("h3", "size", "5");
        //        styles.LoadTagStyle("td", "size", ".6");
        //        FontFactory.Register("c:\\windows\\fonts\\arial.ttf", "Garamond");   // just give a path of arial.ttf 
        //        styles.LoadTagStyle("body", "face", "Garamond");
        //        styles.LoadTagStyle("body", "encoding", "Identity-H");
        //        styles.LoadTagStyle("body", "size", "12pt");
        //        styles.LoadTagStyle("body", "text-align", "rtl");


        //        using (var document = new Document())
        //        {
        //            PdfWriter.GetInstance(document, new FileStream("MySamplePDF6.pdf", FileMode.Create));
        //            document.SetPageSize(PageSize.A4);
        //            document.Open();

        //            //Use a table so that we can set the text direction
        //            PdfPTable table = new PdfPTable(1);
        //            //Ensure that wrapping is on, otherwise Right to Left text will not display
        //            table.DefaultCell.NoWrap = false;
        //            table.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

        //            var fontPath1 = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\tahoma.ttf";
        //            var baseFont = BaseFont.CreateFont(fontPath1, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //            var tahomaFont = new Font(baseFont, 10, Font.NORMAL, BaseColor.BLACK);

        //            //ColumnText ct = new ColumnText(pdfWriter.DirectContent);
        //            //ct.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
        //            //ct.SetSimpleColumn(100, 100, 500, 800, 24, Element.ALIGN_RIGHT);

        //            ///////////////////////////////////////////
        //            //You need to use container elements which support RunDirection, such as ColumnText or PdfPCell and then set their element.RunDirection = PdfWriter.RUN_DIRECTION_RTL

        //          List<IElement> list1 = HTMLWorker.ParseToList(new StringReader(htmlData), styles);
        //            document.Open();

        //            //Use a table so that we can set the text direction
        //            PdfPTable table1 = new PdfPTable(1);
        //            //Ensure that wrapping is on, otherwise Right to Left text will not display
        //            table1.DefaultCell.NoWrap = false;
        //            table1.RunDirection = PdfWriter.RUN_DIRECTION_RTL;

        //            //Loop through each element, don't bother wrapping in P tags
        //            foreach (var element in list1)
        //            {
        //                foreach (var item in element.Chunks)
        //                {
        //                    item.Font = tahomaFont;
        //                }
        //                if (element is PdfPTable)
        //                {
        //                    table1 = (PdfPTable)element;
        //                    table1.DefaultCell.NoWrap = false;
        //                    table1.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
        //                    foreach (PdfPRow row in table1.Rows)
        //                    {
        //                        foreach (PdfPCell cell in row.GetCells())
        //                        {
                                    
        //                            cell.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
        //                            cell.NoWrap = false;
        //                        }
        //                    }
        //                    //Add the table to the document
        //                    document.Add(table1);

        //                }
        //            }
                   
        //            document.Close();
        //           // pdfWriter.Close();







        //            List<IElement> sr = HTMLWorker.ParseToList(new StringReader(htmlData), styles);

        //            foreach (IElement element in sr)
        //            {


        //                foreach (var item in element.Chunks)
        //                {
        //                    item.Font = tahomaFont;
        //                    var p = new Paragraph(item);
        //                    // = Element.ALIGN_RIGHT;
        //                    document.Add(p);
        //                }



        //                document.Add(element);
        //            }
        //            document.Close();
        //        }

        //        /////////////////////////////////
        //        //Register a single font
        //        //FontFactory.Register(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "tahoma.ttf"), "Tahoma Sans MS");

        //        var fontPath = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\tahoma.ttf";
        //        var fontImp = new XMLWorkerFontProvider(fontPath); 
        //        fontImp.DefaultEncoding = "Identity-H";
        //        fontImp.DefaultEmbedding = true;


        //        using (var doc = new Document())
        //            {
        //                doc.SetPageSize(PageSize.A4.Rotate());

        //            using (var writer = PdfWriter.GetInstance(doc, new FileStream("Test2.pdf", FileMode.Create)))
        //            {
        //                doc.Open();

        //                //Get a stream of our HTML
        //                using (var msHTML = new MemoryStream(Encoding.UTF8.GetBytes(htmlData)))
        //                {
        //                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHTML, null, Encoding.UTF8, fontImp);
        //                }

        //                doc.Close();
        //            }
        //            //var res = PdfSharpConvert(DrawRightToLeft(htmlData));

        //            Document document = new Document();
        //            BaseFont STF_Helvetica_Turkish = iTextSharp.text.pdf.BaseFont.CreateFont("Helvetica", "Cp1255", BaseFont.NOT_EMBEDDED);
        //            Font fontNormal = new iTextSharp.text.Font(STF_Helvetica_Turkish, 12, Font.NORMAL);

        //            StyleSheet styles1 = new StyleSheet();
        //            styles.LoadTagStyle("h3", "size", "5");
        //            styles.LoadTagStyle("td", "size", ".6");
        //            FontFactory.Register("c:\\windows\\fonts\\arial.ttf", "Garamond");   // just give a path of arial.ttf 
        //            styles.LoadTagStyle("body", "face", "Garamond");
        //            styles.LoadTagStyle("body", "encoding", "Identity-H");
        //            styles.LoadTagStyle("body", "size", "12pt");

        //            PdfWriter.GetInstance(document, new FileStream("MySamplePDF.pdf", FileMode.Create));
        //            var font1 = FontFactory.GetFont(BaseFont.HELVETICA, "Cp1255", BaseFont.NOT_EMBEDDED, 24, Font.BOLD, BaseColor.BLACK);
        //            document.Open();

        //            using (var pdfDoc = new Document(PageSize.A4))
        //            {
        //                var pdfWriter = PdfWriter.GetInstance(pdfDoc, new FileStream("Test.pdf", FileMode.Create));
        //                pdfDoc.Open();

        //                var fontPath1 = Environment.GetEnvironmentVariable("SystemRoot") + "\\fonts\\tahoma.ttf";
        //                var baseFont = BaseFont.CreateFont(fontPath1, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        //                var tahomaFont = new Font(baseFont, 10, Font.NORMAL, BaseColor.BLACK);

        //                ColumnText ct = new ColumnText(pdfWriter.DirectContent);
        //                ct.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
        //                ct.SetSimpleColumn(100, 100, 500, 800, 24, Element.ALIGN_RIGHT);

        //                var chunk = new Chunk(htmlData, tahomaFont);

        //                ct.AddElement(chunk);
        //                ct.Go();
        //            }

        //            var hw = new HTMLWorker(document, null,null);
        //            hw.Parse(new StringReader(htmlData));
        //            document.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //    return true;
        //}

        //public static Byte[] PdfSharpConvert(String html)
        //{
        //    Byte[] res = null;
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        var pdf = PdfGenerator.GeneratePdf(html, PdfSharp.PageSize.A4);
        //        //pdf.Save(ms);
        //        //res = ms.ToArray();
        //        string pdfFilename = "dbtopdf.pdf";
        //        pdf.Save(pdfFilename);
        //        Process.Start(pdfFilename);
        //    }
        //    return res;
        //}

        //private static string DrawRightToLeft( string text)
        //{
        //    List<string> words = text.Split(' ').ToList();
        //    List<string> sentences = new List<string>();

        //    while (words.Any())
        //    {
        //        string s = words[0];
        //        sentences.Add(s.RightToLeft());
        //        words.RemoveAt(0);
        //    }
        //    return string.Join(" ", sentences);
        //}

    }

    public static class StringRTL
    {
        public static string ReverseString(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str.Reverse())
            {
                sb.Append(c);
            }

            return sb.ToString();
        }

        public static string RightToLeft(this string str)
        {
            List<string> output = str.Split(' ').Select(s => s.Any(c => c >= 1424 && c <= 1535) ? s.ReverseString() : s).ToList();
            output.Reverse();
            return string.Join(" ", output.ToArray());
        }

    }
}
