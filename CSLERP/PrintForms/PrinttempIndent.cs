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
using System.Data;
using static iTextSharp.text.Font;
namespace CSLERP.PrintForms
{
    public class PrinttempIndent
    {


        public void PrintIndent(indentheader ioh, List<indentdetail> IODetails)
        {
            string[] Purchasesrce = ioh.PurchaseSource.Split(';');
            string ColHeader = "SI No.;Item Code;Item Name;Last\nPurcahse\nPrice;Quoted\nPrice;Expected\nPurchase\nPrice" +
                              ";Quotation\nNo;Quantity;Buffer\nQuantity;Warranty\nDays ";
            int n = 1;
            string ColDetailString = "";
            var count = IODetails.Count();

            foreach (indentdetail iod in IODetails)
            {
                if (n == count)
                {
                    ColDetailString = ColDetailString + n + Main.delimiter1 + iod.StockItemID + Main.delimiter1 + iod.StockItemName + Main.delimiter1 + iod.LastPurchasedPrice + Main.delimiter1
                                      + iod.QuotedPrice + Main.delimiter1 + iod.ExpectedPurchasePrice + Main.delimiter1 + iod.QuotationNo + Main.delimiter1 + iod.Quantity + Main.delimiter1 + iod.BufferQuantity + Main.delimiter1 + iod.WarrantyDays + "\n";
                }
                else
                {
                    ColDetailString = ColDetailString + n + Main.delimiter1 + iod.StockItemID + Main.delimiter1 + iod.StockItemName + Main.delimiter1 + iod.LastPurchasedPrice + Main.delimiter1
                                     + iod.QuotedPrice + Main.delimiter1 + iod.ExpectedPurchasePrice + Main.delimiter1 + iod.QuotationNo + Main.delimiter1 + iod.Quantity + Main.delimiter1 + iod.BufferQuantity + Main.delimiter1 + iod.WarrantyDays + "\n" + Main.delimiter2;
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
                Font font3 = FontFactory.GetFont("Arial", 8, Font.BOLD | Font.UNDERLINE);
                Font font4 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                string HeaderString = "Temporary No : " + ioh.TemporaryNo + 
                                      "    Temporary Date : " + String.Format("{0:dd MMMM, yyyy}", ioh.TemporaryDate) + Main.delimiter2 +
                                      "Target Date : " + String.Format("{0:dd MMMM, yyyy}", ioh.TargetDate) + Main.delimiter2 +
                                      "Purchase Source : " + getPurchaseSoursestring(ioh.PurchaseSource);

                Paragraph paragraph = new Paragraph(new Phrase("" + ioh.DocumentName + "", FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK)));
                paragraph.Alignment = Element.ALIGN_CENTER;
                paragraph.SpacingAfter = 10;

                PdfPTable tableMain = new PdfPTable(1);
                tableMain.WidthPercentage = 100;

                string[] header = HeaderString.Split(Main.delimiter2);
                foreach(string str in header)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(str,font1));
                    cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    cell.Border = 0;
                    tableMain.AddCell(cell);
                }

                if (ioh.DocumentID == "INDENT")
                {
                    System.Data.DataTable dt = IndentHeaderDB.getPODetailsInDatatable(ioh);
                    PdfPTable tablePODet = new PdfPTable(7);

                    tablePODet.SpacingBefore = 10;
                    tablePODet.WidthPercentage = 100;
                    float[] widthmain = new float[] { 2f, 2f, 2f, 2f, 2f, 2f, 5f };
                    tablePODet.SetWidths(widthmain);
                    tablePODet.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                    PdfPCell cellHEad = new PdfPCell(new Phrase("Internal Order Detail", font3));
                    cellHEad.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cellHEad.Border = 0;
                    cellHEad.Colspan = 7;
                    cellHEad.MinimumHeight = 20;
                    tablePODet.AddCell(cellHEad);

                    foreach (DataColumn c in dt.Columns)
                    {
                        tablePODet.AddCell(new Phrase(c.ColumnName, font2));
                    }

                    foreach (DataRow r in dt.Rows)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            tablePODet.AddCell(new Phrase(r[0].ToString(), font1));
                            tablePODet.AddCell(new Phrase(r[1].ToString(), font1));
                            tablePODet.AddCell(new Phrase(r[2].ToString(), font1));
                            tablePODet.AddCell(new Phrase(r[3].ToString(), font1));
                            tablePODet.AddCell(new Phrase(r[4].ToString(), font1));
                            tablePODet.AddCell(new Phrase(r[5].ToString(), font1));
                            tablePODet.AddCell(new Phrase(r[6].ToString(), font1));
                        }
                    }
                    PdfPCell cellPODet = new PdfPCell(tablePODet);
                    cellPODet.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cellPODet.Border = 0;
                    tableMain.AddCell(cellPODet);

                    //tableMain.AddCell(cellAdd);
                }
                Paragraph subHeadPara = new Paragraph(new Phrase("Indent Detail", font3));
                subHeadPara.Alignment = Element.ALIGN_CENTER;

                string[] ColHeaderStr = ColHeader.Split(';');

                PdfPTable table1 = new PdfPTable(10);
                table1.SpacingBefore = 10f;
                table1.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table1.WidthPercentage = 100;
                float[] width = new float[] { 1f, 2f, 4f, 1.2f, 1f, 1.2f, 1.3f, 1.2f, 1.2f, 1.2f };
                table1.SetWidths(width);

                for (int i = 0; i < ColHeaderStr.Length; i++)
                {
                    PdfPCell hcell = new PdfPCell(new Phrase(ColHeaderStr[i].Trim(), font2));
                    hcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table1.AddCell(hcell);
                }

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
                        PdfPCell pcell;
                        if (j > 2)
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
                        if (j == 0 || j > 2)
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

                doc.Add(paragraph);
                doc.Add(tableMain);
                doc.Add(subHeadPara);
                doc.Add(table1);
                doc.Close();
                MessageBox.Show("Document Saved");

                String wmurl = "";
                wmurl = "004.png";
                PrintWaterMark.PdfStampWithNewFile(wmurl, sfd.FileName);

            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error-" + ex.ToString());
                MessageBox.Show("Failed TO Save");
            }
        }
        private static string getPurchaseSoursestring(string PS)
        {
            string str = "";
            string[] PSArr = PS.Split(';');
            List<string> strList = new List<string>();
            foreach(string strt in PSArr)
            {
                if (strt.Trim().Length != 0)
                {
                    string psstr = strt.Trim().Substring(strt.Trim().IndexOf('-') + 1);
                    strList.Add(psstr);
                }
            }
            str = string.Join(",", strList.ToArray());
            return str;
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
