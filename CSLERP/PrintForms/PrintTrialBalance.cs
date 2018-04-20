using CSLERP.DBData;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public class PrintTrialBalance
    {

        string fileDir = "";
        public byte[] PrintTRBalance(string Header, List<ledger> ledgerList, string balance)
        {
            MemoryStream ms = new MemoryStream();
            string fileName = "";
            string payMode = "";
            string[] str = Header.Split(';');
            string HeaderSring = "Financial Year: " + str[0] + ";Trial Balance Date: " + str[1];
            string ColHeader = "SI No.;Account No;Account Date; Debit Balance; Credit Balance";
            //string footer3 = "Receiver's Signature;Authorised Signatory";
            int n = 1;
            string ColDetailString = "";
            var count = ledgerList.Count();
            foreach (ledger ldg in ledgerList)
            {
                if(n == count)
                    ColDetailString = ColDetailString + n + "+" + ldg.AccountCode + "+"+ ldg.AccountName + "+" +
                        ldg.DebitAmnt + "+" + ldg.CreditAmnt;
                else
                    ColDetailString = ColDetailString + n + "+" + ldg.AccountCode + "+" + ldg.AccountName + "+" +
                        ldg.DebitAmnt + "+" + ldg.CreditAmnt + ";";
                //totQuant = totQuant + pod.Quantity;
                //totAmnt = totAmnt + (pod.Quantity * pod.Price);
                n++;
            }
            try
            {
                //SaveFileDialog sfd = new SaveFileDialog();
                //sfd.Title = "Save As PDF";
                //sfd.Filter = "Pdf files (*.Pdf)|*.pdf";
                //sfd.InitialDirectory = @"C:\";
                //sfd.FileName = "ledger";
                //sfd.ShowDialog();

                //FileStream fs = new FileStream(sfd.FileName + ".pdf", FileMode.Create, FileAccess.Write);

                //fileName = sfd.FileName + ".pdf";
                Rectangle rec = new Rectangle(PageSize.A4);
                iTextSharp.text.Document doc = new iTextSharp.text.Document(rec);
                PdfWriter writer = PdfWriter.GetInstance(doc, ms);
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
                Paragraph paragraph1 = new Paragraph(new Phrase("CELLCOMM SOLUTIONS Ltd.", font2));
                paragraph1.Alignment = Element.ALIGN_CENTER;
                Paragraph paragraph2 = new Paragraph(new Phrase("Trial Balance", font2));
                paragraph2.Alignment = Element.ALIGN_CENTER;

                //PrintPurchaseOrder prog = new PrintPurchaseOrder();
                string[] HeaderStr = HeaderSring.Split(';');

                Paragraph para = new Paragraph();
                para.Add(new Chunk(HeaderStr[0] + "\n",font1));
                para.Add(new Chunk(HeaderStr[1] + "\n", font1));
                string[] ColHeaderStr = ColHeader.Split(';');
                
                PdfPTable table1 = new PdfPTable(5);
                table1.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table1.WidthPercentage = 100;
                table1.SpacingBefore = 20;
                float[] width = new float[] { 0.5f, 2f, 2f, 3f, 3f };
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
                string[] DetailStr = ColDetailString.Split(';');
                float hg = 0f;
                for (int i = 0; i < DetailStr.Length; i++)
                {
                    string st = DetailStr[i];
                    hg = table1.GetRowHeight(i + 1);
                    string[] detail = DetailStr[i].Split('+');
                    for (int j = 0; j < detail.Length; j++)
                    {
                        PdfPCell pcell;
                        pcell = new PdfPCell(new Phrase(detail[j], font1));
                        pcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        table1.AddCell(pcell);
                        string stt = detail[j];
                    }

                }
                if (table1.Rows.Count > 10)
                    table1.KeepRowsTogether(table1.Rows.Count - 4, table1.Rows.Count);
                PdfPTable table2 = new PdfPTable(5);
                table2.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table2.WidthPercentage = 100;
                table2.SetWidths(width);
                //table2.SpacingBefore = 20;
                PdfPCell cel = new PdfPCell(new Phrase(""));
                cel.Colspan = 2;
                table2.AddCell(cel);
                //table2.AddCell(new Phrase("Balance",font2));
                //string[] bal = balance.Split(';');
                //if (bal[0].Contains("Credit"))
                //{
                //    string str4 = bal[0].Substring(bal[0].IndexOf(':') + 1);
                //    table2.AddCell(new Phrase(""));
                //    table2.AddCell(new Phrase(bal[0].Substring(bal[0].IndexOf(':') + 1), font2));
                //}
                //else if (bal[0].Contains("Debit"))
                //{
                //    string str4 = bal[0].Substring(bal[0].IndexOf(':') + 1);
                //    table2.AddCell(new Phrase(bal[0].Substring(bal[0].IndexOf(':') + 1), font2));
                //    table2.AddCell(new Phrase(""));
                //}
                table2.AddCell(new Phrase("Total", font2));
                table2.AddCell(new Phrase(balance, font2));
                table2.AddCell(new Phrase(balance, font2));
                doc.Add(img);
                doc.Add(paragraph1);
                doc.Add(paragraph2);
                doc.Add(para);
                doc.Add(table1);
                doc.Add(table2);
                doc.Close();
            }
            catch (Exception ie)
            {
                MessageBox.Show("error");
            }
            //return fileName;
            byte[] pdfByte = ms.ToArray();
            string path = Path.GetTempPath();
            fileDir = path + "Test.pdf";
            File.Delete(fileDir);
            using (FileStream fs = File.Create(path+"Test.pdf"))
            {
                fs.Write(pdfByte, 0, (int)pdfByte.Length);
            }
            File.SetAttributes(fileDir,FileAttributes.Hidden);
            System.Diagnostics.Process process = System.Diagnostics.Process.Start(path + "Test.pdf");
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(myProcess_Exited);
            process.WaitForExit();
            //if (process.HasExited)
            //{
            //    MessageBox.Show("Exited");
            //}
            //System.Diagnostics.Process.Start(path + "Test.pdf");
            return ms.ToArray();
        }
        private void myProcess_Exited(object sender, System.EventArgs e)
        {
            if (File.Exists(fileDir))
            {
                try
                {
                    File.Delete(fileDir);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
                }
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
    }
}
