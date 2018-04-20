using CSLERP.DBData;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static iTextSharp.text.Font;
namespace CSLERP.PrintForms
{
    public class PrintSMRNReports
    {
        string str = "";
        string docID = "";
        private string getReportHeader(int ReportNo)
        {
            string header = "";
            if (ReportNo == 1)
            {
                header = "Priliminary Test Report";
                str = "PR";
                docID = "PSRPRILIMINARY";
            }
            else
            {
                header = "Final Test Report";
                str = "FR";
                docID = "PSRFINAL";
            }
            return header;
        }
        public string PrintReport(List<productservicereportdetail> PSRDetail, string detail)
        {
            string pathStr = "";
            string HeaderString = "";
           
            string ColHeader = ColHeader = "SI No.;Test Description;Value;Remark";
            string footer3 = "";
            string fileName = "";
            string[] smrndetail = detail.Split(';');
            string Title = "";
            string subDir = "";
            foreach (productservicereportdetail psrd in PSRDetail)
            {
                Title = getReportHeader(psrd.ReportType);
                HeaderString = "Product: " + smrndetail[0] + ";Customer: " + psrd.CustomerName + ";Job NO:" + psrd.SMRNNo + "/" + psrd.TemporaryNo + "/" + psrd.jobIDNo;
                fileName = str + psrd.SMRNNo + "-" + psrd.SMRNHeaderTempNo + "-" + psrd.jobIDNo;
                footer3 = "Test Engineer:" + psrd.CreatorName + ";Test Date:" + psrd.ReportDate + ";Service Manager Comment:" + smrndetail[1];
                subDir = psrd.TemporaryNo + "-" + psrd.TemporaryDate.ToString("yyyyMMddhhmmss");
                break;
            }
            string dicDir = Main.documentDirectory + "\\" + docID;
            int n = 1;
            string ColDetailString = "";
            var count = PSRDetail.Count();
            foreach (productservicereportdetail psrd in PSRDetail)
            {
                if(n == count)
                    ColDetailString = ColDetailString + n + "+" + psrd.TestDescriptionID + "+" + psrd.TestResult + "+" + psrd.TestRemarks ;
                else
                    ColDetailString = ColDetailString + n + "+" + psrd.TestDescriptionID + "+" + psrd.TestResult + "+" + psrd.TestRemarks + ";";
                n++;
            }
            try
            {
                string dir = dicDir + "\\" + subDir;
                if (!System.IO.Directory.Exists(dir))
                {
                    System.IO.Directory.CreateDirectory(dir);
                }
                string Fname = dir + "\\" + fileName;
                FileStream fs = new FileStream(Fname + ".pdf", FileMode.Create, FileAccess.Write);
                Rectangle rec = new Rectangle(PageSize.A4);
                iTextSharp.text.Document doc = new iTextSharp.text.Document(rec);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                MyEvent evnt = new MyEvent();
                writer.PageEvent = evnt;
                doc.Open();
                Font font1 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                Font font2 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                Font font3 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
                Paragraph paragraph = new Paragraph(new Phrase(Title, font2));
                paragraph.Alignment = Element.ALIGN_CENTER;
                string[] HeaderStr = HeaderString.Split(';');
                Paragraph pheader = new Paragraph();
                foreach(string str in HeaderStr)
                {
                    Phrase ph = new Phrase(str+"\n", font1);
                    pheader.Add(ph);
                }
                string[] ColHeaderStr = ColHeader.Split(';');
                
                PdfPTable table1 = new PdfPTable(4);
                table1.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table1.WidthPercentage = 100;
                float[] width = new float[] { 0.5f, 4f, 4f, 4f };
                table1.SetWidths(width);
                table1.SpacingBefore = 20f;
                for (int i = 0; i < ColHeaderStr.Length; i++)
                {
                    PdfPCell hcell = new PdfPCell(new Phrase(ColHeaderStr[i].Trim(), font2));
                    hcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table1.AddCell(hcell);
                }

                PdfPCell foot = new PdfPCell(new Phrase(""));
                foot.Colspan = 4;
                foot.BorderWidthTop = 0;
                foot.MinimumHeight = 0.5f;
                table1.AddCell(foot);

                table1.HeaderRows = 2;
                table1.FooterRows = 1;

                table1.SkipFirstHeader = false;
                table1.SkipLastFooter = true;

                string[] DetailStr = ColDetailString.Split(';');
                for (int i = 0; i < DetailStr.Length; i++)
                {
                    string[] str = DetailStr[i].Split('+');
                    for (int j = 0; j < str.Length; j++)
                    {
                        PdfPCell pcell;
                        pcell = new PdfPCell(new Phrase(str[j], font1));
                        pcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        table1.AddCell(pcell);
                    }

                }
                Paragraph pFooter = new Paragraph();
                string[] fillFooter = footer3.Split(';');
                Phrase EngName = new Phrase(fillFooter[0] +"\n", font1);
                Phrase TestDate = new Phrase(fillFooter[1] + "\n", font1);
                Phrase Remark = new Phrase(fillFooter[2] + "\n", font1);
                pFooter.Add(EngName);
                pFooter.Add(TestDate);
                pFooter.Add(Remark);
                doc.Add(paragraph);
                doc.Add(pheader);
                doc.Add(table1);
                doc.Add(pFooter);
                doc.Close();
                pathStr = dir + "\\" + fileName + ".pdf";
            }
            catch (Exception ie)
            {
            }
            return pathStr;
        }
        protected class MyEvent : PdfPageEventHelper
        {
            PdfTemplate total;
            Font font2 = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            public override void OnOpenDocument(PdfWriter writer, iTextSharp.text.Document document)
            {
                total = writer.DirectContent.CreateTemplate(40, 16);
            }
            public override void OnEndPage(PdfWriter writer, iTextSharp.text.Document document)
            {
                PdfPTable table = new PdfPTable(3);
                try
                {
                    table.SetWidths(new int[] { 20, 5, 20 });
                    table.DefaultCell.FixedHeight = 10;
                    table.DefaultCell.Border = 0;
                    table.DefaultCell.Border = Rectangle.NO_BORDER;
                    PdfPCell cell = new PdfPCell();
                    cell.Border = 0;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Phrase = new Phrase("");
                    table.AddCell(cell);


                    cell = new PdfPCell();
                    cell.Border = 0;
                    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    cell.Phrase = new Phrase(String.Format("Page " + document.PageNumber.ToString() + " of"), font2);
                    table.AddCell(cell);
                    Image img = Image.GetInstance(total);
                    string alt = img.Alt;
                    cell = new PdfPCell(Image.GetInstance(total));
                    cell.Border = 0;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                    table.TotalWidth = document.PageSize.Width
                            - document.LeftMargin - document.RightMargin;
                    table.WriteSelectedRows(0, -1, document.LeftMargin,
                            document.BottomMargin - 15, writer.DirectContent);

                }
                catch (DocumentException de)
                {
                    MessageBox.Show("Error found in Report Creation");
                }
            }
            public override void OnCloseDocument(PdfWriter writer, iTextSharp.text.Document document)
            {
                ColumnText.ShowTextAligned(total, Element.ALIGN_LEFT, new Phrase((writer.CurrentPageNumber - 1).ToString(), font2), 4, 4, 0);
            }
        }
    }
}
