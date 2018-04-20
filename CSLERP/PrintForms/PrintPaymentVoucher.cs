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
    public class PrintPaymentVoucher
    {


        public string PrintVoucher(paymentvoucher pvh, List<paymentvoucherdetail> pvDetails)
        {
            string fileName = "";
            try
            {
                
                string payMode = "";
                if (pvh.BookType.Equals("BANKBOOK"))
                    payMode = "Bank";
                else
                    payMode = "Cash";
                string HeaderString = "No : " + pvh.VoucherNo + Main.delimiter1 + "Date : " + pvh.VoucherDate.ToString("dd-MM-yyyy") +
                                 Main.delimiter1 + "Payment Mode" + Main.delimiter1 + payMode + Main.delimiter1 + "Paied To" + Main.delimiter1 +
                                 pvh.SLName + Main.delimiter1 + "Amount("+pvh.CurrencyID+")" + Main.delimiter1 + pvh.VoucherAmount +
                                 Main.delimiter1 + "Amount In Words" + Main.delimiter1 +
                                 NumberToString.convert(pvh.VoucherAmount.ToString()).Trim().Replace("INR", pvh.CurrencyID) +
                                 Main.delimiter1 + "Narration" + Main.delimiter1 + pvh.Narration;
                string ColHeader = "SI No.;Bill No;Date;Amount("+pvh.CurrencyID+");Account Name";
                string footer3 = "Receiver's Signature;Authorised Signatory";
                int n = 1;
                string ColDetailString = "";
                var count = pvDetails.Count();
                List<string> billNos = new List<string>();
                List<string> billDates = new List<string>();
                string[] billdetail = pvh.BillDetails.Split(Main.delimiter2); // 0: doctype, 1: billno, 2 : billdate
                foreach (string str in billdetail)
                {
                    if (str.Trim().Length != 0)
                    {
                        string[] subStr = str.Split(Main.delimiter1);
                        billNos.Add(subStr[1]);
                        billDates.Add(Convert.ToDateTime(subStr[2]).ToString("dd-MM-yyyy"));
                    }
                }
                foreach (paymentvoucherdetail pvd in pvDetails)
                {
                    if (pvd.AmountCredit != 0)
                    {
                        if (pvh.BillDetails.Trim().Length != 0)
                        {
                            //ColDetailString = ColDetailString + n + Main.delimiter1 + billdetail[1] + Main.delimiter1 +
                            //  Convert.ToDateTime(billdetail[2].Replace(Main.delimiter2.ToString(), "")).ToString("dd-MM-yyyy") +
                            //  Main.delimiter1 + pvd.AmountCreditINR + Main.delimiter1
                            //                      + pvd.AccountName + Main.delimiter2;
                            ColDetailString = ColDetailString + n + Main.delimiter1 + String.Join(",",billNos.ToArray()) + Main.delimiter1 +
                             string.Join(",",billDates.ToArray()) +
                             Main.delimiter1 + pvd.AmountCredit + Main.delimiter1
                                                 + pvd.AccountName + Main.delimiter2;
                        }
                        else
                        {
                            ColDetailString = ColDetailString + n + Main.delimiter1 + "" + Main.delimiter1 +
                              "" +
                              Main.delimiter1 + pvd.AmountCredit + Main.delimiter1
                                                  + pvd.AccountName + Main.delimiter2;
                        }
                        n++;
                    }
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Save As PDF";
                sfd.Filter = "Pdf files (*.Pdf)|*.pdf";
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                sfd.FileName = pvh.DocumentID + "-" + pvh.VoucherNo;
                if (sfd.ShowDialog() == DialogResult.Cancel || sfd.FileName == "")
                {
                    return "";
                }
                FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                fileName = sfd.FileName;
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
                String URL = "Cellcomm2.JPG";
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(URL);
                img.Alignment = Element.ALIGN_LEFT;

                PdfPTable tableMain = new PdfPTable(2);

                tableMain.WidthPercentage = 100;
                PdfPCell cellImg = new PdfPCell();
                Paragraph pp = new Paragraph();
                pp.Add(new Chunk(img, 0, 0));
                cellImg.AddElement(pp);
                cellImg.Border = 0;
                tableMain.AddCell(cellImg);

                PdfPCell cellAdd = new PdfPCell();
                Paragraph ourAddr = new Paragraph("");
                CompanyDetailDB compDB = new CompanyDetailDB();
                cmpnydetails det = compDB.getdetails().FirstOrDefault(comp => comp.companyID == 1);
                if (det != null)
                {
                    string addr = det.companyname + "\n" + det.companyAddress;
                    ourAddr = new Paragraph(new Phrase(addr, font2));
                    ourAddr.Alignment = Element.ALIGN_RIGHT;
                }
                cellAdd.AddElement(ourAddr);
                cellAdd.Border = 0;
                tableMain.AddCell(cellAdd);


                Paragraph paragraph2 = new Paragraph(new Phrase("Payment Voucher", font2));
                paragraph2.Alignment = Element.ALIGN_CENTER;

                //PrintPurchaseOrder prog = new PrintPurchaseOrder();
                string[] HeaderStr = HeaderString.Split(Main.delimiter1);

                PdfPTable table = new PdfPTable(4);

                table.SpacingBefore = 20f;
                table.WidthPercentage = 100;
                float[] HWidths = new float[] { 2f, 1f, 1f, 2f };
                table.SetWidths(HWidths);
                PdfPCell cell = null;
                for (int i = 0; i < HeaderStr.Length; i++)
                {
                    if ((i % 2) != 0)
                    {
                        cell = new PdfPCell(new Phrase(HeaderStr[i].Trim(), font1));
                        cell.Colspan = 3;
                        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }
                    else
                    {
                        table.AddCell(new PdfPCell(new Phrase(HeaderStr[i].Trim(), font1)));
                    }
                }
                Paragraph paragraph3 = new Paragraph(new Phrase("Bill Details", font2));
                paragraph3.Alignment = Element.ALIGN_CENTER;
                paragraph3.SpacingBefore = 10;
                paragraph3.SpacingAfter = 10;
                string[] ColHeaderStr = ColHeader.Split(';');

                PdfPTable table1 = new PdfPTable(5);
                table1.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table1.WidthPercentage = 100;
                float[] width = new float[] { 0.5f, 2f, 3f, 3f, 6f };
                table1.SetWidths(width);

                for (int i = 0; i < ColHeaderStr.Length; i++)
                {
                    PdfPCell hcell = new PdfPCell(new Phrase(ColHeaderStr[i].Trim(), font2));
                    hcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table1.AddCell(hcell);
                }
                //---
                PdfPCell foot = new PdfPCell(new Phrase(""));
                foot.Colspan = 5;
                foot.BorderWidthTop = 0;
                foot.MinimumHeight = 0.5f;
                table1.AddCell(foot);

                table1.HeaderRows = 2;
                table1.FooterRows = 1;

                table1.SkipFirstHeader = false;
                table1.SkipLastFooter = true;
                //--- 
                string[] DetailStr = ColDetailString.Split(Main.delimiter2);
                float hg = 0f;
                for (int i = 0; i < DetailStr.Length; i++)
                {
                    if (DetailStr[i].Length != 0)
                    {
                        hg = table1.GetRowHeight(i + 1);
                        string[] str = DetailStr[i].Split(Main.delimiter1);
                        for (int j = 0; j < str.Length; j++)
                        {
                            PdfPCell pcell;
                            //if (j == 1 || j == 3 || j == 4)
                            //{
                            //    pcell = new PdfPCell(new Phrase(str[j], font2));
                            //}
                            //else
                            pcell = new PdfPCell(new Phrase(str[j], font1));
                            pcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            table1.AddCell(pcell);
                        }
                    }

                }
                string[] ft = footer3.Split(';');

                PdfPTable tableFooter = new PdfPTable(3);
                tableFooter.SpacingBefore = 50;
                tableFooter.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tableFooter.WidthPercentage = 100;
                PdfPCell fcell1 = new PdfPCell(new Phrase(ft[0], font2));
                fcell1.Border = 0;
                fcell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                PdfPCell fcell2 = new PdfPCell(new Phrase(ft[1], font2));
                fcell2.Border = 0;
                fcell2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;

                PdfPCell fcell3 = new PdfPCell();
                fcell3.Border = 0;

                tableFooter.AddCell(fcell1);
                tableFooter.AddCell(fcell3);
                tableFooter.AddCell(fcell2);
                if (table1.Rows.Count > 10)
                    table1.KeepRowsTogether(table1.Rows.Count - 4, table1.Rows.Count);
                doc.Add(tableMain);
                //doc.Add(img);
                doc.Add(paragraph2);
                doc.Add(table);
                doc.Add(paragraph3);
                doc.Add(table1);
                doc.Add(tableFooter);
                doc.Close();
               
                //-----watermark
                String wmurl = "";
                if (pvh.status==98)
                {
                    //cancelled
                    wmurl = "003.png";
                }
                else
                {
                    wmurl = "001.png";
                }
                 
                ////String wmurl = "napproved-1.jpg";
                iTextSharp.text.Image wmimg = iTextSharp.text.Image.GetInstance(URL);
                PrintWaterMark.PdfStampWithNewFile(wmurl, sfd.FileName);
                //-----
                MessageBox.Show("Document Saved");
            }
            catch (Exception ie)
            {
                MessageBox.Show("Failed to save.\n" + ie.Message);
            }
            return fileName;
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
    }
}
