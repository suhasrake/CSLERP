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
    public class PrintExportInvoiceOutOld
    {


        public void PrintExportIO(invoiceoutheader ioh, List<invoiceoutdetail> IODetails, string taxStr, string podocID)
        {
            try
            {
                Dictionary<string, string> companyInfo = getCompanyInformation();
                string[] pos = ioh.TrackingNos.Split(';');
                int b = 0;
                int[] a = (from s in pos where int.TryParse(s, out b) select b).ToArray();
                int min = a.Min();
                string[] dates = ioh.TrackingDates.Split(';');
                string poStr = "";
                string billingAdd = "";
                //StringBuilder sb = new StringBuilder();
                for (int i = 0; i < pos.Length - 1; i++)
                {
                    string custPOStr = POPIHeaderDB.getCustomerPOAndDateForInvoiceOut(Convert.ToInt32(pos[i]), Convert.ToDateTime(dates[i]), podocID);
                    string[] custPO = custPOStr.Split(Main.delimiter1);
                    if (Convert.ToInt32(pos[i]) == min)
                        billingAdd = custPO[2];
                    poStr = poStr + custPO[0] + " dated " + custPO[1] + "\n";
                }
                companybank cb = CompanyBankDB.getCompBankDetailForIOPrint(ioh.BankAcReference);
                CompanyDataDB datadb = new CompanyDataDB();
                string ieccode = "";
                cmpnydata cdata = datadb.getData("1").FirstOrDefault(c => c.DataID == "ImportExportCode");
                if (cdata != null)
                {
                    ieccode = cdata.DataValue;
                }

                customer custDetail = CustomerDB.getCustomerDetailForPO(ioh.ConsigneeID);
                string[] companyBillingAdd = CompanyAddressDB.getCompTopBillingAdd(Login.companyID);
                string ConsgAdd = "Consignee:\n" + custDetail.name + Main.delimiter2 + "\n" + ioh.DeliveryAddress + "\n";
                string buyer = "Buyer:\n" + custDetail.name + Main.delimiter2 + "\n" + billingAdd + "\n";
                if (custDetail.StateName.ToString().Length != 0)
                {
                    ConsgAdd = ConsgAdd + "Sate Name:" + custDetail.StateName;
                    buyer = buyer + "Sate Name:" + custDetail.StateName;
                    if (custDetail.StateCode.ToString().Length != 0)
                    {
                        ConsgAdd = ConsgAdd + " ,\nState Code:" + custDetail.StateCode;
                        buyer = buyer + " ,\nState Code:" + custDetail.StateCode;
                    }
                }
                else
                {
                    if (custDetail.StateCode.ToString().Length != 0)
                    {
                        ConsgAdd = ConsgAdd + "\nState Code:" + custDetail.StateCode;
                        buyer = buyer + "\nState Code:" + custDetail.StateCode;
                    }
                }
                //string buyer = 
                if (custDetail.OfficeName.ToString().Length != 0)
                {
                    ConsgAdd = ConsgAdd + "\nGST:" + custDetail.OfficeName; // For GST Code
                    buyer = buyer + "\nGST:" + custDetail.OfficeName; // For GST Code
                }
                if (CustomerDB.getCustomerPANForInvoicePrint(ioh.ConsigneeID).Length != 0)
                {
                    ConsgAdd = ConsgAdd + "\nPAN:" + custDetail.OfficeName; // For PAN Code
                    buyer = buyer + "\nPAN:" + custDetail.OfficeName; // For PAN Code
                }

                string HeaderString = companyBillingAdd[0] + Main.delimiter2 + "\n" + companyBillingAdd[1] + "\nGST:" + companyInfo["GST"] + "\nCIN:" + companyInfo["CIN"] + "\nPAN:" + companyInfo["PAN"] +

                                    Main.delimiter1 + "Invoice No : " + ioh.InvoiceNo + ", Date : " + ioh.InvoiceDate.ToString("dd-MM-yyyy") +
                                    Main.delimiter1 + "Reference : " + poStr +
                                    Main.delimiter1 + "IEC Code : " + ieccode +
                                    Main.delimiter1 + "AD Code : " + ioh.ADCode + Main.delimiter1 +

                                    ConsgAdd +

                                    Main.delimiter1 + "Country of origin of goods : " + ioh.OriginCountryName +
                                    Main.delimiter1 + "Country of final destination : " + ioh.FinalDestinatoinCountryName +
                                    Main.delimiter1 + "Pre-Carriage by : " + ioh.PreCarriageTransportationName +
                                    Main.delimiter1 + "Place of Receipt by pre-carrier : " + ioh.PreCarrierReceiptPlace +
                                    Main.delimiter1 + "Port of Loading : " + ioh.ExitPort + Main.delimiter1 +

                                    buyer +

                                    Main.delimiter1 + "Port of Discharge : " + ioh.EntryPort +
                                    Main.delimiter1 + "Final Destination : " + ioh.FinalDestinationPlace +
                                    Main.delimiter1 + "Terms of Delivery : " + ioh.TermsOfDelivery +
                                    Main.delimiter1 + ioh.SpecialNote;


                string footer1 = "Amount In Words\n\n";
                string ColHeader = "SI No.;Description of Goods;HSN;Quantity;Unit;Unit Rate;Amount";
                string footer2 = "\n\nBank : " + cb.BankName + "\nBranch : " + cb.BranchName + "\nAC Type : " + cb.AccountType + "\nAC No : " +
                    cb.AccountCode + "\nSWIFT Code : " + cb.CompanyID + "\nIFSC Code : " + cb.CreateUser;
                string footer3 = "For Cellcomm Solutions Limited;Authorised Signatory";
                //string termsAndCond = getTCString(poh.TermsAndCondition);
                double totQuant = 0.00;
                double totAmnt = 0.00;
                int n = 1;
                string ColDetailString = "";
                var count = IODetails.Count();
                ////string HSNCode = "";

                foreach (invoiceoutdetail iod in IODetails)
                {
                    ////if ((ioh.DocumentID == "SERVICEINVOICEOUT") || (ioh.DocumentID == "SERVICEEXPORTINVOICEOUT"))
                    ////{
                    ////    HSNCode = ServiceHSNMappingDB.getHSNCode(iod.StockItemID);
                    ////}
                    ////else
                    ////{
                    ////    HSNCode = ProductHSNMappingDB.getHSNCode(iod.StockItemID, iod.ModelNo);
                    ////}
                    //+ : main.delimiter1
                    if (n == count)
                    {
                        //ColDetailString = ColDetailString + n + "+" + iod.CustomerItemDescription + "+" + HSNCode + "+" + iod.Quantity + "+"
                        //                  + iod.Unit + "+" + iod.Price + "+" + (iod.Quantity * iod.Price);
                        ColDetailString = ColDetailString + n + Main.delimiter1 + iod.CustomerItemDescription + Main.delimiter1 + iod.HSNCode + Main.delimiter1 + iod.Quantity + Main.delimiter1
                                          + iod.Unit + Main.delimiter1 + iod.Price + Main.delimiter1 + (iod.Quantity * iod.Price);
                        if (iod.Tax != 0)
                        {
                            //ColDetailString = ColDetailString + Main.delimiter2 + "" + "+" + iod.TaxCode + "+" + "" + "+"
                            //              + "" + "+" + "" + "+" + "" + "+" + iod.Tax;
                            ColDetailString = ColDetailString + Main.delimiter2 + "" + Main.delimiter1 + iod.TaxCode + Main.delimiter1 + "" + Main.delimiter1
                                          + "" + Main.delimiter1 + "" + Main.delimiter1 + "" + Main.delimiter1 + iod.Tax;
                        }
                    }
                    else
                    {
                        ColDetailString = ColDetailString + n + Main.delimiter1 + iod.CustomerItemDescription + Main.delimiter1 + iod.HSNCode + Main.delimiter1 + iod.Quantity + Main.delimiter1
                                          + iod.Unit + Main.delimiter1 + iod.Price + Main.delimiter1 + (iod.Quantity * iod.Price) + Main.delimiter2;
                        if (iod.Tax != 0)
                        {
                            //ColDetailString = ColDetailString + "" + "+" + iod.TaxCode + "+" + "" + "+"
                            //              + "" + "+" + "" + "+" + "" + "+" + iod.Tax + Main.delimiter2;
                            ColDetailString = ColDetailString + "" + Main.delimiter1 + iod.TaxCode + Main.delimiter1 + "" + Main.delimiter1
                                         + "" + Main.delimiter1 + "" + Main.delimiter1 + "" + Main.delimiter1 + iod.Tax + Main.delimiter2;
                        }
                    }
                    totQuant = totQuant + iod.Quantity;
                    totAmnt = totAmnt + (iod.Quantity * iod.Price);
                    n++;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Save As PDF";
                sfd.Filter = "Pdf files (*.Pdf)|*.pdf";
                sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                sfd.FileName = ioh.DocumentID + "-" + ioh.InvoiceNo;

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
                Font font4 = FontFactory.GetFont("Arial", 5, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
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
                Paragraph ourAddr = new Paragraph();
                CompanyDetailDB compDB = new CompanyDetailDB();
                cmpnydetails det = compDB.getdetails().FirstOrDefault(comp => comp.companyID == 1);
                if (det != null)
                {
                    //string addr = det.companyname + "\n" + det.companyAddress;
                    ourAddr.Add(new Chunk(det.companyname + "\n", font2));
                    ourAddr.Add(new Chunk(det.companyAddress + "\n", font4));
                    ourAddr.SetLeading(0.0f, 1.5f);
                    //ourAddr = new Paragraph(new Phrase(addr, font2));
                    ourAddr.Alignment = Element.ALIGN_RIGHT;
                }
                cellAdd.AddElement(ourAddr);
                cellAdd.Border = 0;
                tableMain.AddCell(cellAdd);

                Paragraph paragraph = new Paragraph(new Phrase("Invoice", font2));
                paragraph.Alignment = Element.ALIGN_CENTER;

                PrintPurchaseOrder prog = new PrintPurchaseOrder();
                string[] HeaderStr = HeaderString.Split(Main.delimiter1);

                PdfPTable table = new PdfPTable(7);

                table.SpacingBefore = 20f;
                table.WidthPercentage = 100;
                float[] HWidths = new float[] { 0.5f, 8f, 1.5f, 2f, 1.5f, 2f, 3f };
                table.SetWidths(HWidths);
                PdfPCell cell;
                int[] arr = { 3, 7, 9, 10 };
                float wid = 0;
                for (int i = 0; i < HeaderStr.Length; i++)
                {
                    if (i == 0 || i == 5 || i == 11)
                    {
                        string[] format = HeaderStr[i].Split(Main.delimiter2);
                        Phrase phr = new Phrase();
                        phr.Add(new Chunk(format[0], font2));
                        phr.Add(new Chunk(format[1], font1));
                        cell = new PdfPCell(phr);
                        if (i == 5)
                            cell.Rowspan = 5;
                        else
                            cell.Rowspan = 4;
                        cell.Colspan = 2;
                        cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);

                    }
                    //else if (arr.Contains(i))
                    //{
                    //    cell = new PdfPCell(new Phrase(HeaderStr[i].Trim(), font1));
                    //    cell.Colspan = 5;
                    //    cell.MinimumHeight = wid;
                    //    table.AddCell(cell);
                    //}
                    else
                    {
                        cell = new PdfPCell(new Phrase(HeaderStr[i].Trim(), font1));
                        cell.Colspan = 5;
                        //if (i % 2 != 0)
                        //    cell.Colspan = 3;
                        //else
                        //    cell.Colspan = 2;
                        table.AddCell(cell);
                    }
                }
                string[] ColHeaderStr = ColHeader.Split(';');

                PdfPTable table1 = new PdfPTable(7);
                table1.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table1.WidthPercentage = 100;
                float[] width = new float[] { 0.5f, 8f, 1.5f, 2f, 1.5f, 2f, 3f };
                table1.SetWidths(width);

                for (int i = 0; i < ColHeaderStr.Length; i++)
                {
                    if (i == 5 || i == 6)
                    {
                        PdfPCell hcell = new PdfPCell(new Phrase(ColHeaderStr[i].Trim() + "\n(" + ioh.CurrencyID + ")", font2));
                        hcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        table1.AddCell(hcell);
                    }
                    else
                    {
                        PdfPCell hcell = new PdfPCell(new Phrase(ColHeaderStr[i].Trim(), font2));
                        hcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        table1.AddCell(hcell);
                    }
                }
                //---
                PdfPCell foot = new PdfPCell(new Phrase(""));
                foot.Colspan = 7;
                foot.BorderWidthTop = 0;
                foot.MinimumHeight = 0.5f;
                table1.AddCell(foot);

                table1.HeaderRows = 2;
                table1.FooterRows = 1;

                table1.SkipFirstHeader = false;
                table1.SkipLastFooter = true;
                //--- 
                int track = 0;
                decimal dc1 = 0;
                decimal dc2 = 0;

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
                        if (j == 3 || j == 5 || j == 6)
                        {
                            decimal p = 1;
                            if (Decimal.TryParse(str[j], out p))
                                pcell = new PdfPCell(new Phrase(String.Format("{0:0.00}", Convert.ToDecimal(str[j])), font1));
                            else
                                pcell = new PdfPCell(new Phrase(""));
                            pcell.Border = 0;
                            if (j == 6)
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
                            if (j == 2)
                            {
                                if (str[j].Trim().Length == 0)
                                    pcell = new PdfPCell(new Phrase("", font1));
                                else
                                    pcell = new PdfPCell(new Phrase(str[j], font1));
                            }
                            else if (j == 4)
                            {
                                int m = 1;
                                if (Int32.TryParse(str[j], out m) == true)
                                {
                                    if (Convert.ToInt32(str[j]) == 0)
                                        pcell = new PdfPCell(new Phrase("", font1));
                                    else
                                        pcell = new PdfPCell(new Phrase(str[j], font1));
                                }
                                else
                                    pcell = new PdfPCell(new Phrase(str[j], font1));
                            }
                            else
                                pcell = new PdfPCell(new Phrase(str[j], font1));
                            pcell.Border = 0;
                        }
                        //pcell.Border = 0;
                        //if (i == DetailStr.Length - 1)
                        //{
                        //    pcell.MinimumHeight = 50;
                        //}
                        //else
                        pcell.MinimumHeight = 10;
                        //pcell.MinimumHeight = 20;
                        if (j == 1)
                            pcell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                        else
                            pcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        pcell.BorderWidthLeft = 0.01f;
                        pcell.BorderWidthRight = 0.01f;
                        table1.AddCell(pcell);

                    }
                    if (track == 1)
                    {
                        for (int j = 0; j < 7; j++)
                        {
                            PdfPCell pcell1;

                            if (j == 6)
                            {
                                pcell1 = new PdfPCell(new Phrase(String.Format("{0:0.00}", Convert.ToDecimal(dc1 + dc2)), font1));
                                pcell1.Border = 0;
                                pcell1.BorderWidthBottom = 0.01f;
                            }
                            else
                            {
                                pcell1 = new PdfPCell(new Phrase(""));
                                pcell1.Border = 0;
                            }
                            pcell1.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            pcell1.BorderWidthLeft = 0.01f;
                            pcell1.BorderWidthRight = 0.01f;
                            table1.AddCell(pcell1);
                        }
                    }
                }

                double roundedAmt = Math.Round(ioh.InvoiceAmount, 0);
                double diffAmount = roundedAmt - ioh.InvoiceAmount;

                if (diffAmount != 0)
                {
                    table1.AddCell("");
                    table1.AddCell("");
                    PdfPCell cellRound = new PdfPCell(new Phrase("Round off Adj.", font1));
                    cellRound.Colspan = 4;
                    cellRound.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    table1.AddCell(cellRound);
                    table1.AddCell(new Phrase(String.Format("{0:0.00}", diffAmount), font1));
                    //table1.AddCell("");
                }


                table1.AddCell("");
                table1.AddCell("");
                PdfPCell cellRoundTot = new PdfPCell(new Phrase("Total", font1));
                cellRoundTot.Colspan = 4;
                cellRoundTot.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table1.AddCell(cellRoundTot);

                PdfPCell roundTot = new PdfPCell(new Phrase(String.Format("{0:0.00}", roundedAmt), font1));
                roundTot.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                table1.AddCell(roundTot);

                string total = footer1 + NumberToString.convertFC(roundedAmt.ToString(), ioh.CurrencyID).Replace("INR", ioh.CurrencyID) + "\n\n";
                PdfPCell fcell1 = new PdfPCell(new Phrase((total), font1));
                fcell1.Colspan = 6;
                //fcell1.MinimumHeight = 30;
                fcell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                fcell1.BorderWidthBottom = 0;
                fcell1.BorderWidthRight = 0;
                fcell1.BorderWidthTop = 0;
                table1.AddCell(fcell1);

                PdfPCell fcell4 = new PdfPCell(new Phrase("E. & O.E", font1));
                //fcell4.MinimumHeight = 50;
                fcell4.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                fcell4.BorderWidthBottom = 0;
                //fcell4.BorderWidthRight = 0;
                fcell4.BorderWidthLeft = 0;
                fcell4.BorderWidthTop = 0;
                table1.AddCell(fcell4);

                //if (ioh.SpecialNote.Trim().Length != 0)
                //{
                //    PdfPCell SplNote = new PdfPCell(new Phrase(("Note:\n" + ioh.SpecialNote.Trim() + "\n"), font1));
                //    SplNote.Colspan = 5;
                //    SplNote.MinimumHeight = 50;
                //    SplNote.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                //    SplNote.BorderWidthBottom = 0;
                //    SplNote.BorderWidthRight = 0;
                //    SplNote.BorderWidthTop = 0;
                //    table1.AddCell(SplNote);

                //    PdfPCell temp1 = new PdfPCell(new Phrase(""));
                //    temp1.Colspan = 2;
                //    temp1.MinimumHeight = 50;
                //    temp1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                //    temp1.BorderWidthBottom = 0;
                //    temp1.BorderWidthLeft = 0;
                //    temp1.BorderWidthTop = 0;
                //    table1.AddCell(temp1);
                //    //table1.AddCell("");
                //}


                PdfPCell pcelNo = new PdfPCell(new Phrase("", font1));
                pcelNo.BorderWidthTop = 0;
                pcelNo.BorderWidthRight = 0;
                table1.AddCell(pcelNo);

                Phrase phrs = new Phrase();
                phrs.Add(new Chunk("Bank Details", font2));

                phrs.Add(new Chunk(footer2, font1));
                //PdfPCell fcell2 = new PdfPCell(new Phrase(footer2, font1));

                PdfPCell fcell2 = new PdfPCell(phrs);
                fcell2.HorizontalAlignment = PdfPCell.ALIGN_MIDDLE;
                //fcell2.BorderWidthTop = 0;
                //fcell2.BorderWidthRight = 0;
                table1.AddCell(fcell2);

                PdfPCell pcelMid = new PdfPCell(new Phrase("", font1));
                pcelMid.Colspan = 3;
                //pcelMid.Border = 0;
                pcelMid.BorderWidthTop = 0;
                pcelMid.BorderWidthLeft = 0;
                pcelMid.BorderWidthRight = 0;
                table1.AddCell(pcelMid);

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
                fcell3.BorderWidthTop = 0f;
                fcell3.BorderWidthLeft = 0f;
                fcell3.BorderWidthRight = 0.5f;
                fcell3.BorderWidthBottom = 0.5f;
                fcell3.MinimumHeight = 50;
                table1.AddCell(fcell3);
                table1.KeepRowsTogether(table1.Rows.Count - 4, table1.Rows.Count);
                //-------------------------
                double dd = 0;
                PdfPTable taxTab = new PdfPTable(3);
                taxTab.WidthPercentage = 100;

                float[] twidth = new float[] { 3f, 3f, 10f };
                taxTab.SetWidths(twidth);
                if (ioh.TaxAmount != 0)
                {
                    PdfPCell pcell;
                    pcell = new PdfPCell(new Phrase("Tax Details", font2));
                    taxTab.AddCell(pcell);
                    PdfPCell pcellc = new PdfPCell(new Phrase("Amount(" + ioh.CurrencyID + ")", font2));
                    taxTab.AddCell(pcellc);
                    PdfPCell pcelllst = new PdfPCell(new Phrase("", font1));
                    pcelllst.Border = 0;
                    taxTab.AddCell(pcelllst);

                    string[] tax = taxStr.Split(Main.delimiter2);
                    for (int i = 0; i < tax.Length - 1; i++)
                    {

                        string[] subtax = tax[i].Split(Main.delimiter1);
                        PdfPCell pcell1;
                        pcell1 = new PdfPCell(new Phrase(subtax[0], font1));
                        PdfPCell pcell2;
                        pcell2 = new PdfPCell(new Phrase(String.Format("{0:0.00}", Convert.ToDecimal(subtax[1])), font1));
                        PdfPCell pcell3 = new PdfPCell(new Phrase("", font1));
                        pcell3.Border = 0;
                        taxTab.AddCell(pcell1);
                        taxTab.AddCell(pcell2);
                        taxTab.AddCell(pcell3);
                    }
                    taxTab.AddCell(new Phrase("Total Tax Amount", font2));
                    taxTab.AddCell(new Phrase(String.Format("{0:0.00}", Convert.ToDecimal(ioh.TaxAmount)), font2));
                    PdfPCell pcellt = new PdfPCell(new Phrase("", font1));
                    pcellt.Border = 0;
                    taxTab.AddCell(pcellt);
                    taxTab.KeepTogether = true;
                    taxTab.SpacingAfter = 2f;
                    taxTab.SpacingBefore = 3f;
                }
                //PdfPTable TCTab = new PdfPTable(2); ;
                //if (poh.TermsAndCondition.Trim().Length != 0)
                //{
                //    Chunk TCchunk = new Chunk("Terms And Conditions:\n", font2);
                //    TCchunk.SetUnderline(0.2f, -2f);
                //    TCTab = new PdfPTable(2);
                //    TCTab.WidthPercentage = 100;
                //    PdfPCell TCCell = new PdfPCell();
                //    TCCell.Colspan = 2;
                //    TCCell.Border = 0;
                //    TCCell.AddElement(TCchunk);
                //    TCTab.AddCell(TCCell);
                //    try
                //    {
                //        string[] ParaTC = termsAndCond.Split(Main.delimiter2);
                //        for (int i = 0; i < ParaTC.Length - 1; i++)
                //        {
                //            TCCell = new PdfPCell();
                //            TCCell.Colspan = 2;
                //            TCCell.Border = 0;
                //            Paragraph header = new Paragraph();
                //            Paragraph details = new Paragraph();
                //            details.IndentationLeft = 12f;
                //            details.IndentationRight = 12f;
                //            string paraHeaderStr = (i + 1) + ". " + ParaTC[i].Substring(0, ParaTC[i].IndexOf(Main.delimiter1)) + ":";
                //            string paraFooterStr = ParaTC[i].Substring(ParaTC[i].IndexOf(Main.delimiter1) + 1);
                //            header.Add(new Phrase(paraHeaderStr, font2));
                //            details.Add(new Phrase(paraFooterStr, font1));
                //            TCCell.AddElement(header);
                //            TCCell.AddElement(details);
                //            TCTab.AddCell(TCCell);
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error-" + ex.ToString());
                //    }
                //    try
                //    {
                //        if (TCTab.Rows.Count >= 3)
                //        {
                //            TCTab.KeepRowsTogether(0, 3);
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error-" + ex.ToString());
                //    }
                //}
                //doc.Add(jpg);
                doc.Add(tableMain);
                doc.Add(paragraph);
                doc.Add(table);
                doc.Add(table1);
                if (ioh.TaxAmount != 0)
                    doc.Add(taxTab);
                //if(poh.TermsAndCondition.Trim().Length != 0)
                //    doc.Add(TCTab);
                doc.Close();
                MessageBox.Show("Document Saved");
                //-----watermark
                String wmurl = "";
                if (ioh.status == 98)
                {
                    //cancelled
                    wmurl = "003.png";
                }
                else
                {
                    //approved
                    wmurl = "001.png";
                }

                ////String wmurl = "napproved-1.jpg";
                iTextSharp.text.Image wmimg = iTextSharp.text.Image.GetInstance(URL);
                PrintWaterMark.PdfStampWithNewFile(wmurl, sfd.FileName);
                //-----

            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error-" + ex.ToString());
                MessageBox.Show("Failed TO Save");
            }
        }
        //private string getReferenceQuotNo(string quot)
        //{
        //    string str = "";
        //    try
        //    {
        //        if (quot.Length != 0)
        //        {
        //            string[] QuotItem = quot.Split(';');
        //            for (int i = 0; i < QuotItem.Length - 1; i++)
        //            {
        //                //str = str + get.Substring(0, get.IndexOf('('));
        //                if (str.Length == 0)
        //                    str = str + QuotItem[i].Substring(0, QuotItem[i].IndexOf('('));
        //                else
        //                    str = str + "," + QuotItem[i].Substring(0, QuotItem[i].IndexOf('('));
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("getReferenceQuotNo() exception");
        //    }
        //    return str;
        //}
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
        ////protected string getTCString(string TC)
        ////{
        ////    string TCString = "";
        ////    string s;
        ////    string[] str = TC.Trim().Split(new string[] { ";" }, StringSplitOptions.None);
        ////    for (int i = 0; i < str.Length - 1; i++)
        ////    {

        ////        try
        ////        {
        ////            TCString = TCString + TermsAndConditionsDB.getTCDetailsNew(Convert.ToInt32(str[i]));
        ////        }
        ////        catch (Exception ex)
        ////        {
        ////            MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error-" + ex.ToString());
        ////        }
        ////    }
        ////    return TCString;
        ////}
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("getCompanyInformation() exception");
            }
            return dict;
        }
    }
    //public static class Extensions
    //{
    //    //Creating an Extension method to compare two string using contains with case insensitive
    //    public static bool CaseInsensitiveContains(this string text, string value,
    //        StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
    //    {
    //        return text.IndexOf(value, stringComparison) >= 0;
    //    }
    //}
    //public static class MyUtility
    //{
    //    public static string Tab
    //    {
    //        get { return "\t"; }
    //    }
    //}
}
