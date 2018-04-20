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
    public class PrinttempInvoiceOut
    {


        public void PrintIO(invoiceoutheader ioh, List<invoiceoutdetail> IODetails)
        {

            string[] pos = ioh.TrackingNos.Split(';');
            int b = 0;
            int[] a = (from s in pos where int.TryParse(s, out b) select b).ToArray();
            int min = a.Min();
            string[] dates = ioh.TrackingDates.Split(';');
            string ColHeader = "SI No.;Item Code;Item Name;Customer Description;Quantity;Details";
            int n = 1;
            string ColDetailString = "";
            var count = IODetails.Count();

            foreach (invoiceoutdetail iod in IODetails)
            {
                if (n == count)
                {
                    ColDetailString = ColDetailString + n + Main.delimiter1 + iod.StockItemID + Main.delimiter1 + iod.StockItemName + Main.delimiter1 + iod.CustomerItemDescription + Main.delimiter1
                                      + iod.Quantity + Main.delimiter1 + "MRN No    : " + iod.MRNNo + "\n" + "MRN Date : " + iod.MRNDate.ToString("dd-MM-yyyy") + "\n" + "Supplier     : " + iod.SupplierName + "\n";
                }
                else
                {
                    ColDetailString = ColDetailString + n + Main.delimiter1 + iod.StockItemID + Main.delimiter1 + iod.StockItemName + Main.delimiter1 + iod.CustomerItemDescription + Main.delimiter1
                                      + iod.Quantity + Main.delimiter1 + "MRN No    : " + iod.MRNNo + "\n" + "MRN Date : " + iod.MRNDate.ToString("dd-MM-yyyy") + "\n" + "Supplier     : " + iod.SupplierName + "\n" + Main.delimiter2;
                }
                n++;
            }
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Save As PDF";
                sfd.Filter = "Pdf files (*.Pdf)|*.pdf";
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                sfd.FileName = ioh.DocumentID + "-" + ioh.TemporaryNo;

                if (sfd.ShowDialog() == DialogResult.Cancel || sfd.FileName == "")
                {
                    return;
                }

                

                FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                Rectangle rec = new Rectangle(PageSize.A4);
                iTextSharp.text.Document doc = new iTextSharp.text.Document(rec);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                MyEvent evnt = new MyEvent();
                writer.PageEvent = evnt;


                doc.Open();
                Font font1 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                Font font2 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                Font font3 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
                Font font4 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


                PdfPTable tableMain = new PdfPTable(1);
                tableMain.WidthPercentage = 100;

                PdfPCell cellAdd = new PdfPCell();
                Paragraph ourAddr = new Paragraph("");
                DateTime dt =Convert.ToDateTime( dates[0]);
                ourAddr.Add(new Chunk("Tracking No     :" + a[0] + "           ", font2));
                ourAddr.Add(new Chunk("                    Tracking Date  :" + dt.ToString("dd-MM-yyyy") + "" + "\n", font2));
                ourAddr.Add(new Chunk("Customer        :" + ioh.ConsigneeName + "" + "\n", font2));
                ourAddr.Add(new Chunk("Temporary No: " + ioh.TemporaryNo + "               ", font2));
                ourAddr.Add(new Chunk("                 Temporary Date :" + ioh.TemporaryDate.ToString("dd-MM-yyyy") + "" + "\n", font2));
                ourAddr.Alignment = Element.ALIGN_LEFT;
                cellAdd.AddElement(ourAddr);
                cellAdd.Border = 0;
                tableMain.AddCell(cellAdd);

                string[] ColHeaderStr = ColHeader.Split(';');

                PdfPTable table1 = new PdfPTable(6);
                table1.SpacingBefore = 20f;
                table1.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table1.WidthPercentage = 100;
                float[] width = new float[] { 1f, 3f, 8f, 5f, 2f, 5f };
                table1.SetWidths(width);

                for (int i = 0; i < ColHeaderStr.Length; i++)
                {
                    PdfPCell hcell = new PdfPCell(new Phrase(ColHeaderStr[i].Trim(), font2));
                    hcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table1.AddCell(hcell);
                }
                ////---
                //PdfPCell foot = new PdfPCell(new Phrase(""));
                //foot.Colspan = 7;
                //foot.BorderWidthTop = 0;
                //foot.MinimumHeight = 0.5f;
                //table1.AddCell(foot);

                //table1.HeaderRows = 2;
                //table1.FooterRows = 1;

                //table1.SkipFirstHeader = false;
                //table1.SkipLastFooter = true;
                ////--- 
                int track = 0;

                string[] DetailStr = ColDetailString.Split(Main.delimiter2);
                float hg = 0f;
                for (int i = 0; i < DetailStr.Length; i++)
                {
                    track = 0;
                    hg = table1.GetRowHeight(i + 1);
                    string[] str = DetailStr[i].Split(Main.delimiter1);
                    for (int j = 0; j < str.Length; j++)
                    {
                        PdfPCell pcell ;
                        if (j == 4)
                        {
                            decimal p = 1;
                            if (Decimal.TryParse(str[j], out p))
                                pcell = new PdfPCell(new Phrase(String.Format("{0:0.00}", Convert.ToDecimal(str[j])), font1));
                            else
                                pcell = new PdfPCell(new Phrase(""));
                            pcell.Border = 0;
                        }
                        else
                        {
                            pcell = new PdfPCell(new Phrase(str[j], font1));
                            pcell.Border = 0;
                        }
                        pcell.MinimumHeight = 10;
                        if (j == 0 || j==4)
                            pcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        else
                            pcell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        pcell.BorderWidthLeft = 0.01f;
                        pcell.BorderWidthRight = 0.01f;
                        pcell.BorderWidthBottom = 0.5f;
                        table1.AddCell(pcell);

                    }
                }
                


                ///
                PdfPCell pcelNo = new PdfPCell(new Phrase("", font1));
                pcelNo.BorderWidthTop = 0;
                pcelNo.BorderWidthRight = 0;
                table1.AddCell(pcelNo);

                PdfPCell pcelMid = new PdfPCell(new Phrase("", font1));
                pcelMid.Colspan = 3;
                pcelMid.Border = 0;
                pcelMid.BorderWidthTop = 0;
                pcelMid.BorderWidthLeft = 0;
                pcelMid.BorderWidthRight = 0;
                table1.AddCell(pcelMid);

                doc.Add(tableMain);
                doc.Add(table1);
                doc.Close();
                MessageBox.Show("Document Saved");

            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error-" + ex.ToString());
                MessageBox.Show("Failed TO Save");
            }
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
                catch (Exception ex)
                {
                    MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error-" + ex.ToString());
                }
            }
            public override void OnCloseDocument(PdfWriter writer, iTextSharp.text.Document document)
            {
                ColumnText.ShowTextAligned(total, Element.ALIGN_LEFT, new Phrase((writer.CurrentPageNumber - 1).ToString(), font2), 4, 4, 0);
            }
        }
    }
}
