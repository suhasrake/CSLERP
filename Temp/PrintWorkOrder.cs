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
    public class PrintPurchaseOrder
    {
        
        
        public void PrintPO(poheader poh, List<podetail> PODetail)
        {
            string HeaderString = "Dispatch To "+ poh.DeliveryAddress +
                            ";Voucher No:\n"+poh.PONo+";Dated:\n"+poh.PODate+ ";Supplier Ref./Order No.\n" + poh.PONo + ";Despatch Through\n***;Freight:\n"+poh.FreightTerms+";Delivery Date:\n"+poh.DeliveryPeriod+";Mode/Terms of Payment:\n"+poh.ModeOfPayment+
                            ";Supplier \nCellcomm Solution Limited\n#52/44, 8th Main, II Cross,\nMahalaxmi layout,\nBangalore-560 096(INDIA)\n\n;" +
                            "Tax And Duties:\n"+poh.TaxTerms+";Warrenty:\n1 Year;Price Basis:\nCSL-BANGALORE";
            string footer1 = "Amount Chargeable(In Words)\n\n";
            string ColHeader = "SI No.;Model No;Description of Goods;Quantity;Rate;Amount";
            string footer2 = "test";
            string footer3 = "for CELLCOMM SOLUTION LIMITED;Authorised Signatory";
            string termsAndCond = getTCString(poh.TermsAndCondition);
            double totQuant = 0.00;
            double totAmnt = 0.00;
            int n = 1;
            string ColDetailString = "";
            var count = PODetail.Count();
            foreach (podetail pod in PODetail)
            {
                if(n == count)
                    ColDetailString = ColDetailString + n + "+" + pod.StockItemID + "+" + pod.StockItemName + "+" + pod.Quantity + "+"
                                        + pod.Price + "+" + (pod.Quantity * pod.Price);
                else
                    ColDetailString = ColDetailString + n + "+" + pod.StockItemID + "+" + pod.StockItemName + "+" + pod.Quantity + "+"
                                        + pod.Price + "+" + (pod.Quantity * pod.Price) + ";";
                totQuant = totQuant + pod.Quantity;
                totAmnt = totAmnt + (pod.Quantity * pod.Price);
                n++;
            }
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Save As PDF";
                sfd.Filter = "Pdf files (*.Pdf)|*.pdf";
                sfd.InitialDirectory = @"C:\";
                sfd.FileName = poh.DocumentID + "-" + poh.PONo;
                sfd.ShowDialog();

                FileStream fs = new FileStream(sfd.FileName + ".pdf", FileMode.Create, FileAccess.Write);
                Rectangle rec = new Rectangle(PageSize.A4);
                iTextSharp.text.Document doc = new iTextSharp.text.Document(rec);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                MyEvent evnt = new MyEvent();
                writer.PageEvent = evnt;

                doc.Open();
                Font font1 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                Font font2 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                Font font3 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
                //String imageURL = @"D:\Smrutiranjan\PurchaseOrder\index.jpg";
                //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                String URL = @"..\\..\\Pictures\\Cellcomm2.JPG";
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(URL);
                img.Alignment = Element.ALIGN_LEFT;
                Paragraph paragraph = new Paragraph(new Phrase("PURCHASE ORDER", font2));
                paragraph.Alignment = Element.ALIGN_CENTER;

                PrintPurchaseOrder prog = new PrintPurchaseOrder();
                string[] HeaderStr = HeaderString.Split(';');

                PdfPTable table = new PdfPTable(3);

                table.SpacingBefore = 20f;
                table.WidthPercentage = 100;
                float[] HWidths = new float[] { 3f, 2f, 2f };
                table.SetWidths(HWidths);
                PdfPCell cell;
                for (int i = 0; i < HeaderStr.Length; i++)
                {
                    if (i == 0)
                    {
                        cell = new PdfPCell(new Phrase(HeaderStr[0].Trim(), font1));
                        cell.Rowspan = 4;
                        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }
                    else if ((i == 7) || ((i > 8) && (i <= 11)))
                    {
                        cell = new PdfPCell(new Phrase(HeaderStr[i].Trim(), font1));
                        cell.Colspan = 2;
                        table.AddCell(cell);
                    }
                    else if (i == 8)
                    {
                        cell = new PdfPCell(new Phrase(HeaderStr[i].Trim(), font1));
                        cell.Rowspan = 3;
                        table.AddCell(cell);
                    }
                    else
                        table.AddCell(new PdfPCell(new Phrase(HeaderStr[i].Trim(), font1)));
                }
                string[] ColHeaderStr = ColHeader.Split(';');
                
                PdfPTable table1 = new PdfPTable(6);
                table1.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table1.WidthPercentage = 100;
                float[] width = new float[] { 0.5f, 2.5f, 5f, 2f, 2f, 2f };
                table1.SetWidths(width);

                for (int i = 0; i < ColHeaderStr.Length; i++)
                {
                    PdfPCell hcell = new PdfPCell(new Phrase(ColHeaderStr[i].Trim(), font2));
                    hcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table1.AddCell(hcell);
                }
                //---
                PdfPCell foot = new PdfPCell(new Phrase(""));
                foot.Colspan = 6;
                foot.BorderWidthTop = 0;
                foot.MinimumHeight = 0.5f;
                table1.AddCell(foot);

                table1.HeaderRows = 2;
                table1.FooterRows = 1;

                table1.SkipFirstHeader = false;
                table1.SkipLastFooter = true;
                //--- 
                string[] DetailStr = ColDetailString.Split(';');
                float hg = 0f;
                for (int i = 0; i < DetailStr.Length; i++)
                {
                    hg = table1.GetRowHeight(i + 1);
                    string[] str = DetailStr[i].Split('+');
                    for (int j = 0; j < str.Length; j++)
                    {
                        PdfPCell pcell;
                        if (j == 1 || j == 3 || j == 5)
                        {
                            pcell = new PdfPCell(new Phrase(str[j], font2));
                        }
                        else
                            pcell = new PdfPCell(new Phrase(str[j], font1));
                        pcell.Border = 0;
                        if (i == (DetailStr.Length - 1))
                        {
                            pcell.MinimumHeight = 100;
                        }
                        else
                            pcell.MinimumHeight = 20;
                        pcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        pcell.BorderWidthLeft = 0.01f;
                        pcell.BorderWidthRight = 0.01f;
                        table1.AddCell(pcell);

                    }

                }
                //if()
                table1.AddCell("");
                table1.AddCell("");
                table1.AddCell(new Phrase("Total", font2));
                table1.AddCell(new Phrase(totQuant.ToString(),font2));
                table1.AddCell("");
                table1.AddCell(new Phrase(Math.Round(totAmnt,2).ToString(),font2));
                string total = footer1 + NumberToString.convert(totAmnt.ToString())+"\n\n P.S:";
                PdfPCell fcell1 = new PdfPCell(new Phrase((total), font3));
                fcell1.Colspan = 6;
                fcell1.MinimumHeight = 50;
                fcell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                fcell1.BorderWidthBottom = 0;
                table1.AddCell(fcell1);

                PdfPCell fcell2 = new PdfPCell(new Phrase(footer2, font1));
                fcell2.Colspan = 3;
                fcell2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                fcell2.BorderWidthTop = 0;
                table1.AddCell(fcell2);
                string[] ft = footer3.Split(';');

                PdfPCell fcell3 = new PdfPCell();
                Chunk ch1 = new Chunk(ft[0], font1);
                Chunk ch2 = new Chunk(ft[1], font1);
                Phrase phrase = new Phrase();
                phrase.Add(ch1);
                for (int i = 0; i < 3; i++)
                    phrase.Add(Chunk.NEWLINE);
                phrase.Add(ch2);

                Paragraph para = new Paragraph();
                para.Add(phrase);
                para.Alignment = Element.ALIGN_RIGHT;
                fcell3.AddElement(para);
                fcell3.Border = 0;
                fcell3.Colspan = 3;
                //fcell3.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                fcell3.BorderWidthTop = 0.5f;
                fcell3.BorderWidthRight = 0.5f;
                fcell3.BorderWidthBottom = 0.5f;
                fcell3.MinimumHeight = 50;
                table1.AddCell(fcell3);
                table1.KeepRowsTogether(table1.Rows.Count - 4, table1.Rows.Count);
                Chunk TCchunk = new Chunk("Terms And Conditoins:\n", font2);
                TCchunk.SetUnderline(0.2f, -2f);
                PdfPTable TCTab = new PdfPTable(2);
                TCTab.WidthPercentage = 100;
                PdfPCell TCCell = new PdfPCell();
                TCCell.Colspan = 2;
                TCCell.Border = 0;
                TCCell.AddElement(TCchunk);
                TCTab.AddCell(TCCell);
                try
                {
                    string[] ParaTC = termsAndCond.Split(';');
                    for (int i = 0; i < ParaTC.Length + 1; i++)
                    {
                        TCCell = new PdfPCell();
                        TCCell.Colspan = 2;
                        TCCell.Border = 0;
                        Paragraph header = new Paragraph();
                        Paragraph details = new Paragraph();
                        details.IndentationLeft = 12f;
                        details.IndentationRight = 12f;
                        string paraHeaderStr = (i + 1) + ". " + ParaTC[i].Substring(0, ParaTC[i].IndexOf('+')) + ":";
                        string paraFooterStr = ParaTC[i].Substring(ParaTC[i].IndexOf('+') + 1);
                        header.Add(new Phrase(paraHeaderStr, font2));
                        details.Add(new Phrase(paraFooterStr, font1));
                        TCCell.AddElement(header);
                        TCCell.AddElement(details);
                        TCTab.AddCell(TCCell);
                    }
                }
                catch (Exception ex)
                {
                }
                try
                {
                    TCTab.KeepRowsTogether(0, 3);
                }
                catch (Exception ex)
                {
                }

                //doc.Add(jpg);
                doc.Add(img);
                doc.Add(paragraph);
                doc.Add(table);
                doc.Add(table1);
                doc.Add(TCTab);
                doc.Close();
            }
            catch (Exception ie)
            {
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
                catch (DocumentException de)
                {
                    MessageBox.Show("Error found in Purchase Order details.");
                }
            }
            public override void OnCloseDocument(PdfWriter writer, iTextSharp.text.Document document)
            {
                ColumnText.ShowTextAligned(total, Element.ALIGN_LEFT, new Phrase((writer.CurrentPageNumber - 1).ToString(), font2), 4, 4, 0);
            }
        }
        protected string getTCString(string TC)
        {
            string TCString = "";
            string s;
            string[] str = TC.Trim().Split(new string[] { ";" }, StringSplitOptions.None);
            for(int i= 0; i<str.Length-1; i++)
            {

                try
                {
                    TCString = TCString + TermsAndConditionsDB.getTCDetails(Convert.ToInt32(str[i]));
                }
                catch (Exception ex)
                {
                }
            }
            return TCString;
        }
    }
}
