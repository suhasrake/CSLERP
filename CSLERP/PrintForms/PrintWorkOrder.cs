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
    public class PrintWorkOrder
    {


        public void PrintWO(workorderheader woh, List<workorderdetail> WODetail, string totalTaxDetail)
        {
            Dictionary<string, string> companyInfo = getCompanyInformation();

            customer custDetail = CustomerDB.getCustomerDetailForPO(woh.CustomerID);
            string[] companyBillingAdd = CompanyAddressDB.getCompTopBillingAdd(Login.companyID);
            string contractorAdd = "Contractor:\n" + custDetail.name + Main.delimiter1 + "\n" + woh.POAddress + "\n";
            if (custDetail.StateName.ToString().Length != 0)
                contractorAdd = contractorAdd + "Sate Name:" + custDetail.StateName;
            if (custDetail.StateCode.ToString().Length != 0)
                contractorAdd = contractorAdd + "\nState Code:" + custDetail.StateCode;
            if (custDetail.OfficeName.ToString().Length != 0)
                contractorAdd = contractorAdd + "\nGST:" + custDetail.OfficeName; // For GST Code
            string InvoiceTo = "Invoice To: \n" + companyBillingAdd[0] + Main.delimiter1 + "\n" + companyBillingAdd[1] + "\nGST:" + companyInfo["GST"] + "\nCIN:" + companyInfo["CIN"] + "\nPAN:" + companyInfo["PAN"];
            string declaration = "We, Cellcomm Solutions Limited (also referred to as “CSL”), are pleased to place" +
                " this Purchase Order (also referred to as 'PO') with the Contractor addressed below.In accepting this Purchase Order, the Contractor" +
                " undertakes to execute the work described herein as per the details and instructions shown below" +
                " (such details and instructions being the Specific Terms and Conditions agreed between the Parties) " +
                "and also subject to the General Terms & Conditions contained herein.";

            string HeaderString = contractorAdd +
                          Main.delimiter2 + "PO No : S-" + woh.WONo + " , Date : " + String.Format("{0:dd MMMM, yyyy}", Convert.ToDateTime(woh.WODate)) +
                          Main.delimiter2 + "Contractor Reference : " + woh.ContractorReference +
                          Main.delimiter2 + "Target Date : " + String.Format("{0:dd MMMM, yyyy}", Convert.ToDateTime(woh.TargetDate)) + Main.delimiter2 +

                          InvoiceTo +

                          Main.delimiter2 + "Payment Terms : " + PTDefinitionDB.getPaymentTermString(woh.PaymentTerms) +
                          Main.delimiter2 + woh.SpecialNote;


            string footer1 = "Amount in words\n\n";
            string ColHeader = "SI No.;Description of Work;Location;Quantity;Rate;Amount";
            string footer2 = "This Purchase Order, being computer generated, does not require physical signature.";
            string footer3 = "for Cellcomm Solutions Limited;Authorised Signatory";
            string termsAndCond = getTCString(woh.TermsAndCond,woh.DocumentID);
            double totQuant = 0.00;
            double totAmnt = 0.00;
            int n = 1;
            string ColDetailString = "";
            var count = WODetail.Count();

            //+ : main.delimeter1
            //; : main.delimiter2

            try
            {
                foreach (workorderdetail wod in WODetail)
                {
                    if (n == count)
                    {
                        ColDetailString = ColDetailString + n + Main.delimiter1 + wod.Description + Main.delimiter1 + wod.WorkLocation + Main.delimiter1 + wod.Quantity + Main.delimiter1
                                           + wod.Price + Main.delimiter1 + (wod.Quantity * wod.Price);
                    }
                    else
                    {
                        ColDetailString = ColDetailString + n + Main.delimiter1 + wod.Description + Main.delimiter1 + wod.WorkLocation + Main.delimiter1 + wod.Quantity + Main.delimiter1
                                           + wod.Price + Main.delimiter1 + (wod.Quantity * wod.Price) + Main.delimiter2;
                    }
                    totQuant = totQuant + wod.Quantity;
                    totAmnt = totAmnt + (wod.Quantity * wod.Price);
                    n++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to Save 1 : " + ex.ToString());
            }
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Save As PDF";
                sfd.Filter = "Pdf files (*.Pdf)|*.pdf";
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (woh.Status == 0 && woh.DocumentStatus < 99)
                {
                    sfd.FileName = woh.DocumentID + "-Temp-" + woh.TemporaryNo;
                }
                else
                {
                    sfd.FileName = woh.DocumentID + "-" + woh.WONo;
                }
                
                //sfd.ShowDialog();
                if (sfd.ShowDialog() == DialogResult.Cancel || sfd.FileName == "")
                {
                    return;
                }

                FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                Rectangle rec = new Rectangle(PageSize.A4);
                //rec.Bottom = 10;
                iTextSharp.text.Document doc = new iTextSharp.text.Document(rec);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                MyEvent evnt = new MyEvent();
                writer.PageEvent = evnt;

                doc.Open();
                Font font1 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                Font font2 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                Font font3 = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.ITALIC, BaseColor.BLACK);
                Font font4 = FontFactory.GetFont("Arial", 5, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                //String imageURL = @"D:\Smrutiranjan\PurchaseOrder\index.jpg";
                //iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageURL);
                String URL = "Cellcomm2.JPG";
                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(URL);
                img.Alignment = Element.ALIGN_LEFT;
                //--

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
                CompanyDetailDB compDB = new CompanyDetailDB();
                cmpnydetails det = compDB.getdetails().FirstOrDefault(comp => comp.companyID == 1);
                if (det != null)
                {
                    ourAddr.Add(new Chunk(det.companyname + "\n", font2));
                    ourAddr.Add(new Chunk(det.companyAddress.Replace("\r\n", "\n"), font4));
                    StringBuilder sb = new StringBuilder();
                    sb.Append("\nGST : " + companyInfo["GST"] + "\nState Code for GST : " + companyInfo["StateCode"] + "\nCIN : " + companyInfo["CIN"] + "\nPAN : " + companyInfo["PAN"]);
                    ourAddr.Add(new Chunk(sb.ToString(), font4));
                    ourAddr.Alignment = Element.ALIGN_RIGHT;
                    ourAddr.SetLeading(0.0f, 1.5f);
                }
                cellAdd.AddElement(ourAddr);
                cellAdd.Border = 0;
                tableHeader.AddCell(cellAdd);
                //----
                Paragraph paragraphDocumentName = new Paragraph(new Phrase("PURCHASE ORDER", font2));
                paragraphDocumentName.Alignment = Element.ALIGN_CENTER;
                Paragraph paragraphOpenCluse = new Paragraph(new Phrase(declaration, font1));
                paragraphOpenCluse.Alignment = Element.ALIGN_JUSTIFIED;
                paragraphDocumentName.SpacingAfter = 15;
                //PrintPurchaseOrder prog = new PrintPurchaseOrder();
                string[] HeaderStr = HeaderString.Split(Main.delimiter2);

                PdfPTable TableAddress = new PdfPTable(6);

                TableAddress.SpacingBefore = 20f;
                TableAddress.WidthPercentage = 100;
                float[] HWidths = new float[] { 0.5f, 4.5f, 2f, 2f, 2f, 3f };
                TableAddress.SetWidths(HWidths);
                PdfPCell cell;
                float wid = 0;
                for (int i = 0; i < HeaderStr.Length; i++)
                {
                    if (i == 0 || i == 4)
                    {
                        string[] format = HeaderStr[i].Split(Main.delimiter1);
                        Phrase phr = new Phrase();
                        phr.Add(new Chunk(format[0], font2));
                        phr.Add(new Chunk(format[1], font1));
                        cell = new PdfPCell(phr);
                        cell.Colspan = 3;
                        if (i == 0)
                            cell.Rowspan = 3;
                        else
                            cell.Rowspan = 2;
                        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        TableAddress.AddCell(cell);
                    }
                    else
                    {
                        cell = new PdfPCell(new Phrase(HeaderStr[i].Trim(), font1));
                        cell.Colspan = 3;
                        cell.MinimumHeight = wid;
                        TableAddress.AddCell(cell);
                    }
                }
                string[] ColHeaderStr = ColHeader.Split(';');

                PdfPTable TableItemDetails = new PdfPTable(6);
                TableItemDetails.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                TableItemDetails.WidthPercentage = 100;
                float[] width = new float[] { 0.5f, 4.5f, 2f, 2f, 2f, 3f };
                TableItemDetails.SetWidths(width);

                for (int i = 0; i < ColHeaderStr.Length; i++)
                {
                    PdfPCell hcell = new PdfPCell(new Phrase(ColHeaderStr[i].Trim(), font2));
                    hcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    TableItemDetails.AddCell(hcell);
                }
                //---
                PdfPCell foot = new PdfPCell(new Phrase(""));
                foot.Colspan = 6;
                foot.BorderWidthTop = 0;
                foot.MinimumHeight = 0.5f;
                TableItemDetails.AddCell(foot);

                TableItemDetails.HeaderRows = 2;
                TableItemDetails.FooterRows = 1;

                TableItemDetails.SkipFirstHeader = false;
                TableItemDetails.SkipLastFooter = true;
                //--- 
                int track = 0;
                decimal dc1 = 0;
                decimal dc2 = 0;

                string[] DetailStr = ColDetailString.Split(Main.delimiter2);
                float hg = 0f;
                for (int i = 0; i < DetailStr.Length; i++)
                {
                    
                    hg = TableItemDetails.GetRowHeight(i + 1);
                    string[] str = DetailStr[i].Split(Main.delimiter1);
                    for (int j = 0; j < str.Length; j++)
                    {
                        PdfPCell pcell;
                        if (j == 3 || j == 4 || j == 5)
                        {
                            decimal p = 1;
                            if (Decimal.TryParse(str[j], out p))
                                pcell = new PdfPCell(new Phrase(String.Format("{0:0.00}", Convert.ToDecimal(str[j])), font1));
                            else
                                pcell = new PdfPCell(new Phrase(""));
                            pcell.Border = 0;
                            if (j == 5)
                            {
                                if (str[0].Length == 0)
                                {
                                    pcell.BorderWidthBottom = 0.01f;
                                    track = 1;
                                    dc2 = Convert.ToDecimal(str[j]);
                                }
                                else
                                    dc1 = Convert.ToDecimal(str[j]);
                            }
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
                        TableItemDetails.AddCell(pcell);
                    }
                }
                double roundedAmt = Math.Round(totAmnt, 0);
                double diffAmount = roundedAmt - totAmnt;
                if (diffAmount != 0)
                {
                    TableItemDetails.AddCell("");
                    TableItemDetails.AddCell("");
                    PdfPCell cellTot = new PdfPCell(new Phrase("", font1));
                    cellTot.Colspan = 3;
                    cellTot.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    TableItemDetails.AddCell(cellTot);
                    TableItemDetails.AddCell(new Phrase(String.Format("{0:0.00}", totAmnt), font2));

                    TableItemDetails.AddCell("");
                    TableItemDetails.AddCell("");
                    PdfPCell cellRound = new PdfPCell(new Phrase("Rounding off", font1));
                    cellRound.Colspan = 3;
                    cellRound.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    TableItemDetails.AddCell(cellRound);
                    TableItemDetails.AddCell(new Phrase(String.Format("{0:0.00}", diffAmount), font1));
                }
                TableItemDetails.AddCell("");
                TableItemDetails.AddCell("");
                PdfPCell cellTotal = new PdfPCell(new Phrase("Total", font1));
                cellTotal.Colspan = 3;
                cellTotal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                TableItemDetails.AddCell(cellTotal);
                TableItemDetails.AddCell(new Phrase(String.Format("{0:0.00}", roundedAmt), font2));

                //-----
                string total = footer1 + NumberToString.convert(roundedAmt.ToString());
                PdfPCell fcell1 = new PdfPCell(new Phrase((total), font3));
                fcell1.Colspan = 6;
                fcell1.MinimumHeight = 50;
                fcell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                //fcell1.BorderWidthBottom = 0;
                TableItemDetails.AddCell(fcell1);

                PdfPTable tableSub = new PdfPTable(1);
                tableSub.DefaultCell.Border = 0;
                tableSub.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                tableSub.AddCell(new Phrase("Subject To Bangalore Jurisdiction", font2));
                PdfPCell celSub = new PdfPCell(tableSub);
                celSub.Border = 0;
                celSub.Colspan = 6;
                TableItemDetails.AddCell(celSub);
                TableItemDetails.KeepRowsTogether(TableItemDetails.Rows.Count - 5, TableItemDetails.Rows.Count);
                Paragraph footer = new Paragraph(new Phrase("Note : " + footer2, font3));
                footer.Alignment = Element.ALIGN_LEFT;
                //--------------------
                PdfPTable TCTab = new PdfPTable(2);
                if (woh.TermsAndCond.Trim().Length != 0)
                {
                    Chunk TCchunk = new Chunk("General Terms & Conditions\n", font2);
                    TCchunk.SetUnderline(0.2f, -2f);
                    TCTab = new PdfPTable(2);
                    TCTab.WidthPercentage = 100;
                    PdfPCell TCCell = new PdfPCell();
                    TCCell.Colspan = 2;
                    TCCell.Border = 0;
                    TCCell.AddElement(TCchunk);
                    TCTab.AddCell(TCCell);
                    try
                    {
                        string[] ParaTC = termsAndCond.Split(Main.delimiter2);
                        for (int i = 0; i < ParaTC.Length + 1; i++)
                        {
                            TCCell = new PdfPCell();
                            TCCell.Colspan = 2;
                            TCCell.Border = 0;
                            Paragraph header = new Paragraph();
                            Paragraph details = new Paragraph();
                            details.IndentationLeft = 12f;
                            details.IndentationRight = 12f;
                            details.Alignment = Element.ALIGN_JUSTIFIED;
                            string paraHeaderStr = (i + 1) + ". " + ParaTC[i].Substring(0, ParaTC[i].IndexOf(Main.delimiter1)) + ":";
                            string paraFooterStr = ParaTC[i].Substring(ParaTC[i].IndexOf(Main.delimiter1) + 1);
                            header.Add(new Phrase(paraHeaderStr, font2));
                            details.Add(new Phrase(paraFooterStr, font1));
                            TCCell.AddElement(header);
                            TCCell.AddElement(details);
                            TCTab.AddCell(TCCell);
                        }
                    }
                    catch (Exception ex)
                    {
                        ////MessageBox.Show("Failed to Save 2 : "+ex.ToString());
                    }
                    try
                    {
                        TCTab.KeepRowsTogether(0, 3);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to Save 3 : " + ex.ToString());
                    }
                }
                doc.Add(tableHeader);
                doc.Add(paragraphDocumentName);
                doc.Add(paragraphOpenCluse);
                doc.Add(TableAddress);
                doc.Add(TableItemDetails);
                doc.Add(footer);
                if (woh.TermsAndCond.Length != 0)
                    doc.Add(TCTab);
                doc.Close();
                
                if (woh.Status == 0 && woh.DocumentStatus < 99)
                {
                    String wmurl = "";
                    wmurl = "004.png";
                    PrintWaterMark.PdfStampWithNewFile(wmurl, sfd.FileName);
                }
                MessageBox.Show("Document Saved");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to Save 4 : " + ex.ToString());
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
                catch (DocumentException ex)
                {
                    MessageBox.Show("Failed to Save 5 : " + ex.ToString());
                }
            }
            public override void OnCloseDocument(PdfWriter writer, iTextSharp.text.Document document)
            {
                ColumnText.ShowTextAligned(total, Element.ALIGN_LEFT, new Phrase((writer.CurrentPageNumber - 1).ToString(), font2), 4, 4, 0);
            }
        }
        protected string getTCString(string TC, string documentID)
        {
            string TCString = "";
            string s;
            string[] str = TC.Trim().Split(new string[] { ";" }, StringSplitOptions.None);
            for (int i = 0; i < str.Length - 1; i++)
            {

                try
                {
                    TCString = TCString + TermsAndConditionsDB.getTCDetailsNew(Convert.ToInt32(str[i]), documentID);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to Save 6 : " + ex.ToString());
                }
            }
            return TCString;
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
    }
}
