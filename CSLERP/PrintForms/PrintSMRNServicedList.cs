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
    public class PrintSMRNServicedList
    {
        public string PrintServicedList(List<smrndetail> smrnList, smrnservicedlist list)
        {
            string pathStr = "";
            string HeaderString = "List No:" + list.ListNo + ";List Date:" + list.ListDate + ";Customer:" + list.CustomerID +
                ";CustomerPO No:" + list.CustomerPONo + ";CustomerPO Date:" + list.CustomerPODate;

            string ColHeader = ColHeader = "SI No.;Items;itemDetails;SerielNo;ServiceStatus";
            string fileName = list.TrackingNo + "-" + list.ListNo;
            string Title = "List Of Serviced Items";
            string subDir = list.ListNo + "-" + list.ListDate.ToString("yyyyMMddhhmmss");
            string dicDir = Main.documentDirectory + "\\" + list.DocumentID;
            int n = 1;
            string ColDetailString = "";
            var count = smrnList.Count();
            foreach (smrndetail smrnd in smrnList)
            {
                if (n == count)
                    ColDetailString = ColDetailString + n + "+" + smrnd.StockItemID + "+" + smrnd.ItemDetails + "+" + smrnd.SerialNo + "+" + smrnd.ProductServiceStatus;
                else
                    ColDetailString = ColDetailString + n + "+" + smrnd.StockItemID + "+" + smrnd.ItemDetails + "+" + smrnd.SerialNo + "+" + smrnd.ProductServiceStatus + ";";
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
                foreach (string str in HeaderStr)
                {
                    Phrase ph = new Phrase();
                    ph.Add(new Chunk(str.Substring(0, str.IndexOf(':') + 1), font2));
                    ph.Add(new Chunk(str.Substring(str.IndexOf(':') + 1) + "\n", font1));
                    pheader.Add(ph);
                }
                string[] ColHeaderStr = ColHeader.Split(';');

                PdfPTable table1 = new PdfPTable(5);
                table1.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table1.WidthPercentage = 100;
                float[] width = new float[] { 3f, 3f, 3f, 3f, 3f };
                table1.SetWidths(width);
                table1.SpacingBefore = 20f;
                for (int i = 0; i < ColHeaderStr.Length; i++)
                {
                    PdfPCell hcell = new PdfPCell(new Phrase(ColHeaderStr[i].Trim(), font2));
                    hcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table1.AddCell(hcell);
                    string str = ColHeaderStr[i];
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
                doc.Add(paragraph);
                doc.Add(pheader);
                doc.Add(table1);
                doc.Close();
                pathStr = dir + "\\" + fileName + ".pdf";

                documentStorage ds = new documentStorage();
                DocumentStorageDB dsdb = new DocumentStorageDB();
                ds.DocumentID = list.DocumentID;
                ds.Directory = subDir;
                ds.FileName = fileName + ".pdf";
                ds.Description = "SMRNServiceList" + list.ListNo;
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
