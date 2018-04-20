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
    public class PrintGatePass
    {


        public void PrintGatePassDetail(gatepassheader gph, List<gatepassdetail> GPDetails)
        {
            Dictionary<string, string> companyInfo = getCompanyInformation();
            string ColHeader = "SI No.;Description;Quantity;Value(Approx.)";
            int n = 1;
            string ColDetailString = "";
            var count = GPDetails.Count();

            foreach (gatepassdetail gpd in GPDetails)
            {
                if (n == count)
                {
                    ColDetailString = ColDetailString + n + Main.delimiter1 + gpd.StockItemName + Main.delimiter1 + gpd.Quantity +
                        Main.delimiter1 + gpd.Value;
                }
                else
                {
                    ColDetailString = ColDetailString + n + Main.delimiter1 + gpd.StockItemName + Main.delimiter1 + gpd.Quantity +
                        Main.delimiter1 + gpd.Value + Main.delimiter2;
                }
                n++;
            }
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Save As PDF";
                sfd.Filter = "Pdf files (*.Pdf)|*.pdf";
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                sfd.FileName = gph.DocumentID + "-" + gph.GatePassNo;

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
                string toData = "";
                CompanyDetailDB comDb = new CompanyDetailDB();
                cmpnydetails com = comDb.getdetails().FirstOrDefault(c => c.companyID == Login.companyID);
                if (gph.CustomerID == null || gph.CustomerID.Trim().Length == 0)
                {
                    
                    if(com != null)
                    {
                        toData = com.companyname + "\n" + gph.ToOfficeName;
                    }
                }
                else
                {
                    customer custDetail = CustomerDB.getCustomerDetailForPO(gph.CustomerID);
                    toData = custDetail.name + "\n" + custDetail.BillingAddress;
                }
                string HeaderString = "To : \n" + toData + Main.delimiter2 + "Gate Pass No : " + gph.GatePassNo + Main.delimiter2 +
                                      "Gate Pass Date : " + String.Format("{0:dd MMMM, yyyy}", gph.GatePassDate) + Main.delimiter2 +
                                      gph.SpecialNotes;

                String URL = "Cellcomm2.JPG";
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(URL);
                img.Alignment = Element.ALIGN_LEFT;

                PdfPTable tableHeader = new PdfPTable(2);

                tableHeader.WidthPercentage = 100;
                PdfPCell cellImg = new PdfPCell();
                Paragraph pp = new Paragraph();
                pp.Add(new Chunk(img, 0, 0));
                cellImg.AddElement(pp);
                cellImg.Border = 0;
                tableHeader.AddCell(cellImg);

                PdfPCell cellAdd = new PdfPCell();
                Paragraph ourAddr = new Paragraph("");

                if (com != null)
                {
                    ourAddr.Add(new Chunk(com.companyname + "\n", font2));
                    ourAddr.Add(new Chunk(com.companyAddress.Replace("\r\n", "\n"), font4));
                    StringBuilder sb = new StringBuilder();
                    sb.Append("\nGST : " + companyInfo["GST"] + "\nState Code for GST : " + companyInfo["StateCode"] + "\nCIN : " + companyInfo["CIN"] + "\nPAN : " + companyInfo["PAN"]);
                    ourAddr.Add(new Chunk(sb.ToString(), font4));
                    ourAddr.Alignment = Element.ALIGN_RIGHT;
                    ourAddr.SetLeading(0.0f, 1.5f);
                }
                cellAdd.AddElement(ourAddr);
                cellAdd.Border = 0;
                tableHeader.AddCell(cellAdd);

                Paragraph paragraph = new Paragraph(new Phrase("OUT GATE PASS", font3));
                paragraph.Alignment = Element.ALIGN_CENTER;
                paragraph.SpacingAfter = 15;

                PdfPTable tableMain = new PdfPTable(4);
                tableMain.WidthPercentage = 100;
                float[] widthMain = new float[] { 1f, 5f, 2f, 2f };
                tableMain.SetWidths(widthMain);

                string[] header = HeaderString.Split(Main.delimiter2);
                foreach(string str in header)
                {
                    int index = Array.IndexOf(header, str);
                    if(index == 0)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(str, font1));
                        cell.Rowspan = 3;
                        cell.Colspan = 2;
                        cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        //cell.Border = 0;
                        tableMain.AddCell(cell);
                    }
                    else
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(str, font1));
                        cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        cell.Colspan = 2;
                        ///cell.Border = 0;
                        tableMain.AddCell(cell);
                    }
                   
                }

                string[] ColHeaderStr = ColHeader.Split(';');

                PdfPTable table1 = new PdfPTable(4);
                //table1.SpacingBefore = 10f;
                table1.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table1.WidthPercentage = 100;
                float[] width = new float[] { 1f, 5f, 2f, 2f };
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
                        if (j == 2)
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
                        pcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        pcell.BorderWidthLeft = 0.01f;
                        pcell.BorderWidthRight = 0.01f;
                        pcell.BorderWidthBottom = 0.5f;
                        table1.AddCell(pcell);

                    }
                }


                ///
                PdfPCell pcelBlank = new PdfPCell(new Phrase("", font1));
                pcelBlank.Border = 0;
                pcelBlank.Colspan = 4;
                pcelBlank.MinimumHeight = 40;
                table1.AddCell(pcelBlank);

                PdfPCell pcelNo = new PdfPCell(new Phrase("", font1));
                pcelNo.Border = 0;
                table1.AddCell(pcelNo);

                PdfPCell pcellAuth = new PdfPCell();
                pcellAuth = new PdfPCell(new Phrase("Approved By : ", font1));
                pcellAuth.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                pcellAuth.Colspan = 3;
                pcellAuth.Border = 0;
                table1.AddCell(pcellAuth);

                table1.HeaderRows = 1; //For continuing column header in next page 
                table1.KeepRowsTogether(table1.Rows.Count - 4, table1.Rows.Count);

                doc.Add(tableHeader);
                doc.Add(paragraph);
                doc.Add(tableMain);
                doc.Add(table1);
                doc.Close();
                MessageBox.Show("Document Saved");

                //String wmurl = "";
                //wmurl = "004.png";
                //PrintWaterMark.PdfStampWithNewFile(wmurl, sfd.FileName);

            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error-" + ex.ToString());
                MessageBox.Show("Failed TO Save");
            }
        }
        private Dictionary<string, string> getCompanyInformation()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            CompanyDataDB dbrecord = new CompanyDataDB();
            try
            {
                List<cmpnydata> data = dbrecord.getData(Login.companyID.ToString());

                //string[] idArr = { "GSTNO", "CIN", "PAN" };
                foreach (cmpnydata cd in data)
                {
                    if (cd.DataID.Equals("GSTNO"))
                    {
                        dict.Add("GST", cd.DataValue);
                    }
                    else if (cd.DataID.Equals("CIN"))
                    {
                        dict.Add("CIN", cd.DataValue);
                    }
                    else if (cd.DataID.Equals("PAN"))
                    {
                        dict.Add("PAN", cd.DataValue);
                    }
                    else if (cd.DataID.Equals("StateCode"))
                    {
                        dict.Add("StateCode", cd.DataValue);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("getCompanyInformation() exception");
            }
            return dict;
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
                PdfPTable regAdd = new PdfPTable(1);
                try
                {
                    CompanyAddressDB compDb = new CompanyAddressDB();
                    companyaddress com = compDb.getCompAddList().FirstOrDefault(c => c.AddressType == 3);
                    string RegAdd = "";
                    if (com != null)
                    {
                        RegAdd = "Registered Address : " + com.Address;
                    }
                    PdfPCell cell23 = new PdfPCell(new Phrase(RegAdd.Replace("\n", " "), FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    cell23.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    cell23.Border = 0;
                    regAdd.AddCell(cell23);
                    regAdd.TotalWidth = document.PageSize.Width
                           - document.LeftMargin - document.RightMargin;
                    regAdd.WriteSelectedRows(0, -1, document.LeftMargin,
                            document.BottomMargin + 6, writer.DirectContent);

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
