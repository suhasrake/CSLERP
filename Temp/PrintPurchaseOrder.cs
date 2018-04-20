using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CSLERP
{
    class PrintPurchaseOrder
    {
        string HeaderStr = "Dispatch To\nCellcomm Solution Limited\n#52/44, 8th Main, II Cross,\nMahalaxmi layout,\nBangalore-560 096(INDIA)\n\n;" +
                            "Voucher No:\nCSL/PO/INT/63161;Dated:\n26-OCT-2016;Supplier Ref./Order No.\nCSL/PO/INT/63161;Despatch Through\n***;Freight:\nThrough our Forwarder;Delivery Date:\n7 Weeks;Mode/Terms of Payment:\n10% Advance and Balance on 60th day from the date of shipment;" +
                            "Supplier \nZTT INTERNATIONAL lIMITED\nNo. 5, Zhongtian Road\nNantong Economic And Technical Development Zone\nJiangsu Provience, P. R. China 226009;" +
                            "Tax And Duties:\nExclusive;Warrenty:\n1 Year;Price Basis:\nCSL-BANGALORE";
        string ColHeaderStr = "SI No.;Model No;Description of Goods;Quantity;Rate;Amount";
        string ColDetailStr1 = "1\n\r2\n\r3\n\r4\n\r1\n\r2\n\r3\n\r4\n\r1\n\r2\n\r3\n\r4\n\r1\n\r2\n\r3\n\r4\n\r1\n\r2\n\r3\n\r4\n\r;" +
            "CSL-7/8\" RF\n\rCSL-7/8\" FR\n\rCSL-1/2\" RF\n\rCSL-1/2\" FR;" + "7/8\" RF CABLE\n\r7/8\" FR CABLE\n\r1/2\" RF CABLE\n\r1/2\" FR CABLE" +
            "For example, in a table that represents companies, each row would represent a single company;" +
            "5000 MTRS.\n\r3000 MTRS.\n\r15000 MTRS.\n\r40000 MTRS.;" + "$2.080\n\r$2.200\n\r$0.940\n\r$0.910;" + "$10400.000\n\r$6600.000\n\r$14100.000\n\r$36400.000";
        string ColDetailStr = "1+CSL-7/8\" RF+7/8\" RF CABLE" +
            "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+5000 MTRS.+$2.080+$10400.000;2+CSL-7/8\" FR+7/8\" FR CABLE" +
            "7/8\" FR CABLE7/8\" FR CABLE7/8\" FR CABLE7/8\" FR CABLE7/8\" FR CABLE7/8\" FR CABLE7/8\" FR CABLE7/8\" FR CABLE7/8\" FR CABLE" +
            "+15000 MTRS+$2.200+$6600.000;3+CSL-1/2\" FR+1/2\" FR CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+15000 MTRS+$0.940+$14100.000;4+CSL-1/2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000" +
            ";3+CSL-1/2\" FR+1/2\" FR CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+15000 MTRS+$0.940+$14100.000;4+CSL-1/2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000;" +
            "5 + CSL - 1 / 2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "1 / 2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000;" +
            "6 + CSL - 1 / 2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "1 / 2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000;" +
            "7 + CSL - 1 / 2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "1 / 2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000;" +
            "8 + CSL - 1 / 2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "1 / 2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000;" +
            "9 + CSL - 1 / 2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "1 / 2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000;" +
            "10 + CSL - 1 / 2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "1 / 2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000;" +
            "11 + CSL - 1 / 2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "1 / 2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000;" +
            "12 + CSL - 1 / 2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "1 / 2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000;" +
            "13 + CSL - 1 / 2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "1 / 2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000l;" +
            "14 + CSL - 1 / 2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "1 / 2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000;" +
            "15 + CSL - 1 / 2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "1 / 2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000;" +
            "16 + CSL - 1 / 2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "1 / 2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000;" +
            "17 + CSL - 1 / 2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "1 / 2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000;" +
            "18 + CSL - 1 / 2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "1 / 2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000;" +
            "19 + CSL - 1 / 2\" RF+1/2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "1 / 2\" RF CABLE" + "7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE7/8\" RF CABLE" +
            "+45000 MTRS+$0.910+$36400.000";


        string footer1 = "Amount Chargeable(In Words)\n*********\n\n\n P.S:";
        string footer2 = "test";
        string footer3 = "for CELLCOMM SOLUTION LIMITED;Authorised Signatory";
        static void PrintPO()
        {
            try
            {
                FileStream fs = new FileStream("Purchase_Order.pdf", FileMode.Create, FileAccess.Write);
                Rectangle rec = new Rectangle(PageSize.A4);
                Document doc = new Document(rec);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                MyEvent evnt = new MyEvent();
                writer.PageEvent = evnt;

                doc.Open();
                Font font1 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                Font font2 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                String imageURL = @"D:\Smrutiranjan\PurchaseOrder\index.jpg";
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                jpg.Alignment = Element.ALIGN_LEFT;
                Paragraph paragraph = new Paragraph(new Phrase("PURCHASE ORDER", font2));
                paragraph.Alignment = Element.ALIGN_CENTER;

                Program prog = new Program();
                string[] HeaderStr = prog.HeaderStr.Split(';');

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
                string[] ColHeaderStr = prog.ColHeaderStr.Split(';');
                string[] DetailStr = prog.ColDetailStr.Split(';');
                PdfPTable table1 = new PdfPTable(6);
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
                            pcell.MinimumHeight = 100;
                        else
                            pcell.MinimumHeight = 20;
                        pcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        pcell.BorderWidthLeft = 0.01f;
                        pcell.BorderWidthRight = 0.01f;
                        table1.AddCell(pcell);

                    }

                }
                table1.AddCell("");
                table1.AddCell("");
                table1.AddCell(new Phrase("Total", font1));
                table1.AddCell("****");
                table1.AddCell("");
                table1.AddCell("****");
                PdfPCell fcell1 = new PdfPCell(new Phrase(prog.footer1, font1));
                fcell1.Colspan = 6;
                fcell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                fcell1.BorderWidthBottom = 0;
                table1.AddCell(fcell1);

                PdfPCell fcell2 = new PdfPCell(new Phrase(prog.footer2, font1));
                fcell2.Colspan = 3;
                fcell2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                fcell2.BorderWidthTop = 0;
                table1.AddCell(fcell2);
                string[] ft = prog.footer3.Split(';');

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

                doc.Add(jpg);
                doc.Add(paragraph);
                doc.Add(table);
                doc.Add(table1);

                doc.Close();



            }
            catch (Exception ie)
            {
                Console.WriteLine("{0}" + ie);
            }
            Console.ReadKey();
        }
        protected class MyEvent : PdfPageEventHelper
        {

            PdfTemplate total;
            Font font2 = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);

            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                total = writer.DirectContent.CreateTemplate(40, 16);
            }
            public override void OnEndPage(PdfWriter writer, Document document)
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

                }
            }
            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                ColumnText.ShowTextAligned(total, Element.ALIGN_LEFT, new Phrase((writer.CurrentPageNumber - 1).ToString(), font2), 4, 4, 0);
            }
        }
    }
}
