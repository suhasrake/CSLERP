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
    public class PrintProductTestReportList
    {
        string s = "";
        Font font1 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
        Font font2 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        Font font3 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
        String URL = "Cellcomm2.JPG";
        string Title = "Product Test Report";
        static string HeaderString = "";
        private string getStatusCode(int status)
        {
            string str = "";
            if (status == 1)
                str = "Pass";
            else if (status == 0)
                str = "Fail";
            return str;
        }
        public Boolean PrintReport(Dictionary<Int32, DateTime> ReprotDict, string Dir, string fname, string prodInfo)
        {
            Boolean status = true;
            string pathStr = "";

            string ColHeader = ColHeader = "SI No.;Test Description;Expected Result;Result;Status";
            string fileName = fname;
            string footer3 = "";

            string subDir = Dir.Substring(0, Dir.IndexOf('-')) + "-" + Dir.Substring(Dir.IndexOf('-') + 1);
            string dicDir = Main.documentDirectory + "\\" + "PRODUCTTESTREPORT";
            string[] ProdInfoStr = prodInfo.Split(';');
            HeaderString = "Product Code: " + ProdInfoStr[0].Substring(0, ProdInfoStr[0].IndexOf('-')) +
                            ";Product Description: " + ProdInfoStr[0].Substring(ProdInfoStr[0].IndexOf('-')+1) +
                            ";ModelNo: " + ProdInfoStr[1].Substring(0, ProdInfoStr[1].IndexOf('-')) +
                             ";ModelName: " + ProdInfoStr[1].Substring(ProdInfoStr[1].IndexOf('-')+1);
            //ptrheader.CreateUser ----  // for testDeacriptionID
            //ptrheader.ForwardUser ----   // for Expected Result
            //ptrheader.ApproveUser ----   // for Actual Result
            //ptrheader.Status ----  // for TestStatus
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
               
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(URL);
                img.Alignment = Element.ALIGN_LEFT;
                Paragraph paragraph = new Paragraph(new Phrase(Title, font2));
                paragraph.Alignment = Element.ALIGN_CENTER;
                paragraph.SpacingAfter = 10f;
                int temp = 0;
                foreach (KeyValuePair<Int32, DateTime> report in ReprotDict)
                {
                    temp++;
                    doc.NewPage();
                    string prodSerNo = "";
                    string no = report.Key.ToString();
                    string val = report.Value.ToString();
                    string ColDetailString = "";
                    List<producttestreportheader> ptrhList = ProductTestReportHeaderDB.getReportListForPrint(report.Key, report.Value);
                    var count = ptrhList.Count();
                    int n = 1;
                    foreach (producttestreportheader ptrh1 in ptrhList)
                    {
                        if (n == count)
                            ColDetailString = ColDetailString + n + "+" + ptrh1.CreateUser + "+" + ptrh1.ForwardUser + "+" + ptrh1.ApproveUser + "+" + getStatusCode(ptrh1.Status);
                        else
                            ColDetailString = ColDetailString + n + "+" + ptrh1.CreateUser + "+" + ptrh1.ForwardUser + "+" + ptrh1.ApproveUser + "+" + getStatusCode(ptrh1.Status) + ";";
                        n++;
                    }
                    foreach (producttestreportheader ptrh in ptrhList)
                    {
                        prodSerNo = "Product Serial No: " + ptrh.ProductSerialNo;
                        footer3 = "Prepared By:" + ptrh.CreatorName + ";Approved By:" + ptrh.ApproverName;
                    }
                    //--
                    string[] HeaderStr = HeaderString.Split(';');

                    Paragraph pheader = new Paragraph();
                    ///pheader.SpacingBefore = 20f;
                    foreach (string str in HeaderStr)
                    {
                        Phrase ph = new Phrase();
                        ph.Add(new Chunk(str.Substring(0, str.IndexOf(':') + 1), font2));
                        ph.Add(new Chunk(str.Substring(str.IndexOf(':') + 1) + "\n", font1));
                        pheader.Add(ph);
                    }
                    Paragraph serial = new Paragraph();
                    Phrase phr = new Phrase();
                    phr.Add(new Chunk(prodSerNo.Substring(0, prodSerNo.IndexOf(':') + 1), font2));
                    phr.Add(new Chunk(prodSerNo.Substring(prodSerNo.IndexOf(':') + 1) + "\n", font1));
                    serial.Add(phr);
                    serial.SpacingBefore = 20f;
                    string[] ColHeaderStr = ColHeader.Split(';');
                    PdfPTable table1 = new PdfPTable(5);
                    table1.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table1.WidthPercentage = 100;
                    float[] width = new float[] { 3f, 3f, 3f, 3f, 3f };
                    table1.SetWidths(width);
                    table1.SpacingBefore = 10f;
                    for (int i = 0; i < ColHeaderStr.Length; i++)
                    {
                        PdfPCell hcell = new PdfPCell(new Phrase(ColHeaderStr[i].Trim(), font2));
                        hcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        table1.AddCell(hcell);
                        //string str = ColHeaderStr[i];
                    }

                    PdfPCell foot = new PdfPCell(new Phrase(""));
                    foot.Colspan = 5;
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
                            string s = str[j];
                            pcell = new PdfPCell(new Phrase(str[j], font1));
                            pcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            table1.AddCell(pcell);
                        }

                    }
                    Paragraph pFooter = new Paragraph();
                    string[] fillFooter = footer3.Split(';');
                    Phrase Creator = new Phrase(fillFooter[0] + "\n", font1);
                    Phrase Approver = new Phrase(fillFooter[1] + "\n", font1);
                    pFooter.Add(Creator);
                    pFooter.Add(Approver);
                    if (temp == 1)
                    {
                        doc.Add(img);
                        doc.Add(paragraph);
                        doc.Add(pheader);
                    }
                    doc.Add(serial);
                    doc.Add(table1);
                    doc.Add(pFooter);
                }
                doc.Close();
                pathStr = dir + "\\" + fileName + ".pdf";

                documentStorage ds = new documentStorage();
                DocumentStorageDB dsdb = new DocumentStorageDB();
                ds.DocumentID = "PRODUCTTESTREPORT";
                ds.Directory = subDir;
                ds.FileName = fileName + ".pdf";
                ds.Description = "ProductTestReport " + subDir;
                if (dsdb.validateDocumentDetails(ds))
                {
                    if (dsdb.iskDocumentDuplication(ds))
                    {
                        if (dsdb.UpdateDocumentDetails(ds))
                        {
                            MessageBox.Show("DocumetnStorage Updated");
                        }
                        else
                            MessageBox.Show("failed to update documentStorage");
                    }
                    else
                    {
                        if (dsdb.InsertDocumentDetails(ds))
                        {
                            MessageBox.Show("DocumetnStorage Updated");
                        }
                        else
                            MessageBox.Show("failed to update documentStorage");
                    }

                }
            }
            catch (Exception ie)
            {
                status = false;
            }
            return status;
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
            public override void OnStartPage(PdfWriter writer, iTextSharp.text.Document document)
            {
                PrintProductTestReportList list = new PrintProductTestReportList();
                if(document.PageNumber != 1)
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(list.URL);
                    img.Alignment = Element.ALIGN_LEFT;
                    Paragraph paragraph = new Paragraph(new Phrase(list.Title, list.font2));
                    paragraph.Alignment = Element.ALIGN_CENTER;
                    paragraph.SpacingAfter = 10f;

                    string[] HeaderStr = PrintProductTestReportList.HeaderString.Split(';');
                    Paragraph pheader = new Paragraph();
                    ///pheader.SpacingBefore = 20f;
                    foreach (string str in HeaderStr)
                    {
                        Phrase ph = new Phrase();
                        ph.Add(new Chunk(str.Substring(0, str.IndexOf(':') + 1), list.font2));
                        ph.Add(new Chunk(str.Substring(str.IndexOf(':') + 1) + "\n", list.font1));
                        pheader.Add(ph);
                    }

                    document.Add(img);
                    document.Add(paragraph);
                    document.Add(pheader);
                }
            } 
        }
    }
}
