using CSLERP.DBData;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class PrintInvoiceOut
    {
        public void PrintIO(invoiceoutheader ioh, List<invoiceoutdetail> IODetails, string taxStr, string podocID)
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
                string othRef = "";
                for (int i = 0; i < pos.Length - 1; i++)
                {
                    string custPOStr = POPIHeaderDB.getCustomerPOAndDateForInvoiceOut(Convert.ToInt32(pos[i]), Convert.ToDateTime(dates[i]), podocID);
                    string[] custPO = custPOStr.Split(Main.delimiter1);
                    ////if (Convert.ToInt32(pos[i]) == min)
                        ////billingAdd = custPO[2];
                    poStr = poStr + custPO[0] + ", Date: " + String.Format("{0:dd MMMM, yyyy}", Convert.ToDateTime(custPO[1])) + "\n";
                    othRef = othRef + pos[i] + ",";
                }
                companybank cb = CompanyBankDB.getCompBankDetailForIOPrint(ioh.BankAcReference);
                customer custDetail = CustomerDB.getCustomerDetailForPO(ioh.ConsigneeID);
                
                string[] companyBillingAdd = CompanyAddressDB.getCompTopBillingAdd(Login.companyID);
                string ConsgAdd = "Consignee:\n" + custDetail.name + Main.delimiter2 + "\n" + ioh.DeliveryAddress + "\n";
                string buyer = "Buyer:\n" + custDetail.name + Main.delimiter2 + "\n" + custDetail.BillingAddress + "\n";
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

                string HeaderString = buyer +

                                Main.delimiter1 + "Invoice No : " + ioh.InvoiceNo + " , Date: " + String.Format("{0:dd MMMM, yyyy}", ioh.InvoiceDate) +
                                Main.delimiter1 + "Buyer's Reference : " + poStr.Trim() +
                                Main.delimiter1 + "Mode of Dispatch : " + ioh.TransportationModeName +
                                Main.delimiter1 + "Terms of Payment : " + ioh.TermsOfPayment + Main.delimiter1 + //Description : Name of terms of payment

                                ConsgAdd +

                                Main.delimiter1 + "Supplier's Reference : " + othRef.Trim().Substring(0, othRef.Length - 1) +
                                Main.delimiter1 + "Reverse Charge : " + ioh.ReverseCharge +
                                Main.delimiter1 + ioh.SpecialNote;

                string footer1 = "Amount in words\n\n";
                string ColHeader = "";
                if (ioh.DocumentID == "PRODUCTINVOICEOUT" || ioh.DocumentID == "PRODUCTEXPORTINVOICEOUT")
                    ColHeader = "SI No.;Description of Goods;HSN;Quantity;Unit;Unit Rate;Amount";
                else
                    ColHeader = "SI No.;Description of Goods;SAC;Quantity;Unit;Unit Rate;Amount";
                CompanyDetailDB compDB = new CompanyDetailDB();
                cmpnydetails det = compDB.getdetails().FirstOrDefault(comp => comp.companyID == 1);
                string compName = "";
                if (det != null)
                {
                    compName = det.companyname;
                }
                string footer2 = "\n\nAccount Name : " + compName + "\nAccount No : " + cb.AccountCode + "\nAccount Type : " + cb.AccountType + "\nBank : " + cb.BankName + "\nBranch : " + cb.BranchName + "\nIFSC : " + cb.CreateUser;
                //"\nSWIFT Code : " + cb.CompanyID + 
                string footer3 = "For Cellcomm Solutions Limited;Authorised Signatory";
                double totQuant = 0.00;
                double totAmnt = 0.00;
                int n = 1;
                string ColDetailString = "";
                var count = IODetails.Count();

                foreach (invoiceoutdetail iod in IODetails)
                {
                    if (ioh.DocumentID == "PRODUCTINVOICEOUT")
                    {
                        iod.HSNCode = iod.HSNCode.Substring(0, 4);
                    }
                    else if (ioh.DocumentID == "SERVICEINVOICEOUT")
                    {
                        iod.HSNCode = iod.HSNCode.Substring(0, 6);
                    }
                    if (n == count)
                    {
                        //if (ioh.DocumentID == "PRODUCTINVOICEOUT")
                        ColDetailString = ColDetailString + n + Main.delimiter1 + iod.CustomerItemDescription + Main.delimiter1 + iod.HSNCode + Main.delimiter1 + iod.Quantity + Main.delimiter1
                                          + iod.Unit + Main.delimiter1 + iod.Price + Main.delimiter1 + (iod.Quantity * iod.Price);
                        //else
                        //    ColDetailString = ColDetailString + n + Main.delimiter1 + iod.CustomerItemDescription + Main.delimiter1 + iod.HSNCode.Substring(0, 4) + Main.delimiter1 + iod.Quantity + Main.delimiter1
                        //                   + iod.Price + Main.delimiter1 + (iod.Quantity * iod.Price);
                    }
                    else
                    {
                        //if (ioh.DocumentID == "PRODUCTINVOICEOUT")
                            ColDetailString = ColDetailString + n + Main.delimiter1 + iod.CustomerItemDescription + Main.delimiter1 + iod.HSNCode + Main.delimiter1 + iod.Quantity + Main.delimiter1
                                          + iod.Unit + Main.delimiter1 + iod.Price + Main.delimiter1 + (iod.Quantity * iod.Price) + Main.delimiter2;
                        //else
                        //    ColDetailString = ColDetailString + n + Main.delimiter1 + iod.CustomerItemDescription + Main.delimiter1 + iod.HSNCode.Substring(0, 4) + Main.delimiter1 + iod.Quantity + Main.delimiter1
                        //                   + iod.Price + Main.delimiter1 + (iod.Quantity * iod.Price) + Main.delimiter2;
                    }
                    totQuant = totQuant + iod.Quantity;
                    totAmnt = totAmnt + (iod.Quantity * iod.Price);
                    n++;
                }

                try
                {
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Title = "Save As PDF";
                    sfd.Filter = "Pdf files (*.Pdf)|*.pdf";
                    sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    if (ioh.status == 0 && ioh.DocumentStatus < 99)
                    {
                        sfd.FileName = ioh.DocumentID + "-Temp-" + ioh.TemporaryNo;
                    }
                    else
                    {
                        sfd.FileName = ioh.DocumentID + "-" + ioh.InvoiceNo;
                    }

                    if (sfd.ShowDialog() == DialogResult.Cancel || sfd.FileName == "")
                    {
                        return;
                    }
                    FileStream fs = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write);
                    Rectangle rec = new Rectangle(PageSize.A4);
                    rec.Bottom = 10;
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

                    Paragraph ParagraphDocumentName = new Paragraph(new Phrase("Tax Invoice", font2));
                    ParagraphDocumentName.Alignment = Element.ALIGN_CENTER;

                    PrintPurchaseOrder prog = new PrintPurchaseOrder();
                    string[] HeaderStr = HeaderString.Split(Main.delimiter1);

                    PdfPTable TableAddress = new PdfPTable(7);

                    TableAddress.SpacingBefore = 20f;
                    TableAddress.WidthPercentage = 100;
                    float[] HWidths = new float[] { 1f, 8f, 1.5f, 2f, 1.5f, 2f, 3f };
                    TableAddress.SetWidths(HWidths);
                    PdfPCell cell;
                    int[] arr = { 3, 7, 9, 10 };
                    float wid = 0;
                    for (int i = 0; i < HeaderStr.Length; i++)
                    {
                        if (i == 0 || i == 5)
                        {
                            string[] format = HeaderStr[i].Split(Main.delimiter2);
                            Phrase phr = new Phrase();
                            phr.Add(new Chunk(format[0], font2));
                            phr.Add(new Chunk(format[1], font1));
                            cell = new PdfPCell(phr);
                            if (i == 0)
                                cell.Rowspan = 4;
                            else
                                cell.Rowspan = 3;
                            cell.Colspan = 2;
                            cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
                            TableAddress.AddCell(cell);

                        }
                        else
                        {
                            cell = new PdfPCell(new Phrase(HeaderStr[i].Trim(), font1));
                            cell.Colspan = 5;
                            cell.MinimumHeight = wid;
                            TableAddress.AddCell(cell);
                        }
                    }
                    string[] ColHeaderStr = ColHeader.Split(';');

                    PdfPTable TableItemDetail = new PdfPTable(7);
                    TableItemDetail.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    TableItemDetail.WidthPercentage = 100;
                    float[] widthProd = new float[] { 1f, 8f, 1.5f, 2f, 1.5f, 2f, 3f };
                    float[] widthServ = new float[] { 1f, 8f, 1.5f, 2f, 0f, 2f, 3f };
                    if (ioh.DocumentID == "PRODUCTINVOICEOUT" || ioh.DocumentID == "PRODUCTEXPORTINVOICEOUT")
                        TableItemDetail.SetWidths(widthProd);
                    else
                        TableItemDetail.SetWidths(widthServ);
                    

                    //Table Row No : 1 (Header Column)
                    for (int i = 0; i < ColHeaderStr.Length; i++)
                    {
                        if (i == (ColHeaderStr.Length-1) || i == (ColHeaderStr.Length-2))
                        {
                            PdfPCell hcell = new PdfPCell(new Phrase(ColHeaderStr[i].Trim() + "\n(" + ioh.CurrencyID + ")", font2));
                            hcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            TableItemDetail.AddCell(hcell);
                        }
                        else
                        {
                            PdfPCell hcell = new PdfPCell(new Phrase(ColHeaderStr[i].Trim(), font2));
                            hcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            TableItemDetail.AddCell(hcell);
                        }
                    }
                    //---
                    //Table Row No : 2 (Footer Column)
                    PdfPCell foot = new PdfPCell(new Phrase(""));
                    foot.Colspan = 7;
                    foot.BorderWidthTop = 0;
                    foot.MinimumHeight = 0.5f;
                    TableItemDetail.AddCell(foot);

                    TableItemDetail.HeaderRows = 2;
                    TableItemDetail.FooterRows = 1;

                    TableItemDetail.SkipFirstHeader = false;
                    TableItemDetail.SkipLastFooter = true;
                    //--- 
                    int track = 0;
                    decimal dc1 = 0;
                    decimal dc2 = 0;

                    //Table Row No : 3 (Header Column)
                    string[] DetailStr = ColDetailString.Split(Main.delimiter2);
                    float hg = 0f;
                    for (int i = 0; i < DetailStr.Length; i++)
                    {
                        track = 0;
                        hg = TableItemDetail.GetRowHeight(i + 1);
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

                            pcell.MinimumHeight = 10;
                            if (j == 1)
                                pcell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                            else
                                pcell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            pcell.BorderWidthLeft = 0.01f;
                            pcell.BorderWidthRight = 0.01f;

                            TableItemDetail.AddCell(pcell);

                        }
                    }
                    PdfPCell Temp = new PdfPCell(new Phrase(""));
                    Temp.Border = 0;
                    Temp.BorderWidthLeft = 0.01f;
                    Temp.BorderWidthRight = 0.01f;

                    int dd = 0;
                    if (ioh.TaxAmount != 0)
                    {
                        ////Table Row No : 4 (Total Amount)
                        PdfPCell Temp1 = new PdfPCell(new Phrase(""));
                        Temp1.Border = 0;
                        Temp1.BorderWidthTop = 0.01f;
                        Temp1.BorderWidthLeft = 0.01f;
                        Temp1.BorderWidthRight = 0.01f;
                        TableItemDetail.AddCell(Temp1); //blank cell
                        TableItemDetail.AddCell(Temp1);
                        PdfPCell cellCom = new PdfPCell(new Phrase("", font1));
                        cellCom.Colspan = 4;
                        cellCom.Border = 0;
                        cellCom.BorderWidthLeft = 0.01f;
                        cellCom.BorderWidthRight = 0.01f;
                        cellCom.BorderWidthTop = 0.01f;
                        cellCom.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        TableItemDetail.AddCell(cellCom);

                        PdfPCell cellTot = new PdfPCell(new Phrase(String.Format("{0:0.00}", Convert.ToDecimal(totAmnt)), font2));
                        cellTot.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        TableItemDetail.AddCell(cellTot);
                        dd++;

                        string[] tax = taxStr.Split(Main.delimiter2);
                        for (int i = 0; i < tax.Length - 1; i++)
                        {
                            TableItemDetail.AddCell(Temp); //blank cell
                            TableItemDetail.AddCell(Temp);

                            string[] subtax = tax[i].Split(Main.delimiter1);
                            PdfPCell cellinrtax = new PdfPCell(new Phrase(subtax[0], font1));
                            cellinrtax.Colspan = 4;
                            cellinrtax.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                            TableItemDetail.AddCell(cellinrtax);

                            PdfPCell pcell2;
                            pcell2 = new PdfPCell(new Phrase(String.Format("{0:0.00}", Convert.ToDecimal(subtax[1])), font1));
                            pcell2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            TableItemDetail.AddCell(pcell2);
                            dd++;
                        }
                        double taxAmRnd = Math.Round(ioh.TaxAmount, 0);
                        if (taxAmRnd != ioh.TaxAmount)
                        {
                            TableItemDetail.AddCell(Temp); //blank cell
                            TableItemDetail.AddCell(Temp);
                            PdfPCell cellTotTax = new PdfPCell(new Phrase("", font1));
                            cellTotTax.Colspan = 4;
                            cellTotTax.Border = 0;
                            cellTotTax.BorderWidthLeft = 0.01f;
                            cellTotTax.BorderWidthRight = 0.01f;
                            //cellCom.BorderWidthTop = 0.01f;
                            cellTotTax.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            TableItemDetail.AddCell(cellTotTax);

                            PdfPCell cellTotTaxValue = new PdfPCell(new Phrase(String.Format("{0:0.00}", Convert.ToDecimal(totAmnt + ioh.TaxAmount)), font2));
                            cellTotTaxValue.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            TableItemDetail.AddCell(cellTotTaxValue);
                            dd++;
                        }
                    }



                    double roundedAmt = Math.Round(ioh.InvoiceAmount, 0);
                    double diffAmount = roundedAmt - ioh.InvoiceAmount;

                    if (diffAmount != 0)
                    {
                        TableItemDetail.AddCell("");
                        TableItemDetail.AddCell("");
                        PdfPCell cellRound = new PdfPCell(new Phrase("Rounding off", font1));
                        cellRound.Colspan = 4;
                        cellRound.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                        TableItemDetail.AddCell(cellRound);
                        TableItemDetail.AddCell(new Phrase(String.Format("{0:0.00}", diffAmount), font1));
                        //table1.AddCell("");
                        dd++;
                    }

                    TableItemDetail.AddCell("");
                    TableItemDetail.AddCell("");
                    PdfPCell cellRoundTot = new PdfPCell(new Phrase("Total", font1));
                    cellRoundTot.Colspan = 4;
                    cellRoundTot.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    TableItemDetail.AddCell(cellRoundTot);

                    PdfPCell roundTot = new PdfPCell(new Phrase(String.Format("{0:0.00}", roundedAmt), font2));
                    roundTot.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    TableItemDetail.AddCell(roundTot);

                    //table1.AddCell("");
                    string total = footer1 + NumberToString.convert(roundedAmt.ToString()).Replace("INR", ioh.CurrencyID) + "\n\n";
                    PdfPCell fcell1 = new PdfPCell(new Phrase((total), font1));
                    fcell1.Border = 0;
                    fcell1.Colspan = 6;
                    fcell1.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    fcell1.BorderWidthLeft = 0.5f;
                    fcell1.BorderWidthBottom = 0.5f;
                    fcell1.BorderWidthRight = 0;
                    fcell1.BorderWidthTop = 0;
                    TableItemDetail.AddCell(fcell1);

                    PdfPCell fcell4 = new PdfPCell(new Phrase("E. & O.E", font1));
                    fcell4.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
                    fcell4.Border = 0;
                    fcell4.BorderWidthBottom = 0.5f;
                    fcell4.BorderWidthRight = 0.5f;
                    fcell4.BorderWidthLeft = 0;
                    fcell4.BorderWidthTop = 0;
                    TableItemDetail.AddCell(fcell4);
                    TableItemDetail.KeepRowsTogether(TableItemDetail.Rows.Count - (dd + 4), TableItemDetail.Rows.Count);
                    //int HSNMappCount = TaxDataTable.Rows.Count + 6;
                    //int itemCount = DetailStr.Length;
                    //int tableCount = table1.Rows.Count;

                    //=================================================
                    //DataTable TaxDataTable = new DataTable("TaxDetails");
                    //DataColumn[] colArr = { new DataColumn("HSN Code"), new DataColumn("Taxable Value"), new DataColumn("CGST %"),
                    //    new DataColumn("CGST Amt"), new DataColumn("SGST %"), new DataColumn("SGST Amt"), new DataColumn("IGST %"),
                    //    new DataColumn("IGST Amt"), new DataColumn("Total Amt") };
                    //TaxDataTable.Columns.AddRange(colArr);

                    //DataRow myDataRow = TaxDataTable.NewRow();
                    //myDataRow[colArr[0]] = "1111";
                    //myDataRow[colArr[1]] = "1000";
                    //myDataRow[colArr[2]] = "9";
                    //myDataRow[colArr[3]] = "200";
                    //myDataRow[colArr[4]] = "9";
                    //myDataRow[colArr[5]] = "200";
                    //myDataRow[colArr[6]] = "14";
                    //myDataRow[colArr[7]] = "600";
                    //myDataRow[colArr[8]] = "2000";
                    //TaxDataTable.Rows.Add(myDataRow);
                    //DataRow myDataRow1 = TaxDataTable.NewRow();
                    //myDataRow1[colArr[0]] = "2222";
                    //myDataRow1[colArr[1]] = "2000";
                    //myDataRow1[colArr[2]] = "9";
                    //myDataRow1[colArr[3]] = "300";
                    //myDataRow1[colArr[4]] = "9";
                    //myDataRow1[colArr[5]] = "400";
                    //myDataRow1[colArr[6]] = "14";
                    //myDataRow1[colArr[7]] = "1300";
                    //myDataRow1[colArr[8]] = "4000";
                    //TaxDataTable.Rows.Add(myDataRow1);
                    //==============================================
                    InvoiceOutHeaderDB iohDB = new InvoiceOutHeaderDB();
                    DataTable TaxDataTable = iohDB.taxDetails4Print(IODetails,ioh.DocumentID);


                    PdfPTable TabTaxDetail = new PdfPTable(TaxDataTable.Columns.Count);
                    TabTaxDetail.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    TabTaxDetail.WidthPercentage = 100;
                    //TabTaxDetail.SpacingBefore = 10;
                    PdfPCell cellTax = new PdfPCell();
                    //Adding columns in table
                    List<string> colListStr = new List<string>();
                    List<string> colListSubStr = new List<string>();
                    double amtTot = 0;
                    double TaxTot1 = 0;
                    double TaxTot2 = 0;
                    double TaxTot3 = 0;
                    double TaxTot = 0;
                    Dictionary<int, string> dictTot = new Dictionary<int, string>();
                    for (int p = 0; p < TaxDataTable.Columns.Count; p++)
                    {
                        if (p != 0 && p != 1 && p != TaxDataTable.Columns.Count - 1)
                        {
                            TaxTot1 = 0;
                            string substr = TaxDataTable.Columns[p].ColumnName;
                            colListStr.Add(substr.Trim()); //CGST
                            colListSubStr.Add(TaxDataTable.Columns[p].ColumnName); //CGST% in subList
                            colListSubStr.Add(TaxDataTable.Columns[p + 1].ColumnName); //CGST amount in subList
                            p++;
                            if (p %2 != 0)
                            {
                                TaxTot1 = TaxDataTable.AsEnumerable().Sum(c => c.Field<double>(TaxDataTable.Columns[p].ColumnName));
                                dictTot.Add(p, String.Format("{0:0.00}", Convert.ToDecimal(TaxTot1)));
                            }
                        }
                        else
                        {
                            colListStr.Add(TaxDataTable.Columns[p].ColumnName);
                            if (p == 1)
                            {
                                amtTot = TaxDataTable.AsEnumerable().Sum(c => c.Field<double>(TaxDataTable.Columns[p].ColumnName));
                                dictTot.Add(p, String.Format("{0:0.00}", Convert.ToDecimal(amtTot)));
                            }
                            if (p == TaxDataTable.Columns.Count - 1)
                            {
                                TaxTot = TaxDataTable.AsEnumerable().Sum(c => c.Field<double>(TaxDataTable.Columns[p].ColumnName));
                                dictTot.Add(p, String.Format("{0:0.00}", Convert.ToDecimal(TaxTot)));
                            }
                        }
                    }
                    foreach (string str in colListStr)
                    {
                        int index = colListStr.FindIndex(x => x == str);
                        if(index == 0 || index == 1 || index == colListStr.Count-1)
                        {
                            cellTax = new PdfPCell();
                            cellTax.Rowspan = 2;
                            Paragraph p = new Paragraph(str, font2);
                            p.Alignment = Element.ALIGN_CENTER;
                            cellTax.AddElement(p);
                            TabTaxDetail.AddCell(cellTax);
                        }
                        else
                        {
                            cellTax = new PdfPCell();
                            cellTax.Colspan = 2;
                            Paragraph p = new Paragraph(str, font2);
                            p.Alignment = Element.ALIGN_CENTER;
                            cellTax.AddElement(p);
                            TabTaxDetail.AddCell(cellTax);
                        }
                    }
                    foreach (string str in colListSubStr)
                    {
                        cellTax = new PdfPCell();
                        Paragraph p = new Paragraph(str, font2);
                        p.Alignment = Element.ALIGN_CENTER;
                        cellTax.AddElement(p);
                        TabTaxDetail.AddCell(cellTax);
                    }
                    int t = 0;
                    foreach (DataRow row in TaxDataTable.Rows)
                    {
                        int l = 0;
                        foreach (DataColumn col in TaxDataTable.Columns)
                        {
                            cellTax = new PdfPCell();
                            if(l == 1 || l%2 != 0 || l == TaxDataTable.Columns.Count-1)
                            {
                                Paragraph p = new Paragraph(String.Format("{0:0.00}", Convert.ToDecimal(row[col].ToString())), font1);
                                p.Alignment = Element.ALIGN_CENTER;
                                cellTax.AddElement(p);
                                TabTaxDetail.AddCell(cellTax);
                            }
                            else
                            {
                                Paragraph p = new Paragraph(row[col].ToString(), font1);
                                p.Alignment = Element.ALIGN_CENTER;
                                cellTax.AddElement(p);
                                TabTaxDetail.AddCell(cellTax);
                            }
                            t++;
                            l++;
                        }
                    }
                    for (int k = 0; k < TaxDataTable.Columns.Count; k++)
                    {
                        if (k == 0)
                        {
                            cellTax = new PdfPCell();
                            Paragraph p = new Paragraph("Total", font2);
                            p.Alignment = Element.ALIGN_CENTER;
                            cellTax.AddElement(p);
                            TabTaxDetail.AddCell(cellTax);
                        }
                        else if(dictTot.ContainsKey(k))
                        {
                            cellTax = new PdfPCell();
                            Paragraph p = new Paragraph(dictTot[k], font2);
                            p.Alignment = Element.ALIGN_CENTER;
                            cellTax.AddElement(p);
                            TabTaxDetail.AddCell(cellTax);
                        }
                        else
                        {
                            cellTax = new PdfPCell();
                            Paragraph p = new Paragraph("", font1);
                            p.Alignment = Element.ALIGN_CENTER;
                            cellTax.AddElement(p);
                            TabTaxDetail.AddCell(cellTax);
                        }
                    }
                    //TabTaxDetail.KeepRowsTogether(TabTaxDetail.Rows.Count - 10, TabTaxDetail.Rows.Count);
                    TabTaxDetail.KeepTogether = true;

                    //Bank dtails and authorised Signature
                    PdfPTable tableFooter = new PdfPTable(2);
                    tableFooter.WidthPercentage = 100;
                    Phrase phrs = new Phrase();
                    phrs.Add(new Chunk("\nBank Details for Payment", font2));

                    phrs.Add(new Chunk(footer2, font1));

                    PdfPCell fcell2 = new PdfPCell(phrs);
                    fcell2.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                    fcell2.BorderWidthRight = 0;
                    tableFooter.AddCell(fcell2);

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
                    fcell3.BorderWidthRight = 0.5f;
                    fcell3.BorderWidthBottom = 0.5f;
                    fcell3.MinimumHeight = 50;
                    tableFooter.AddCell(fcell3);

                    PdfPTable tableSub = new PdfPTable(1);
                    tableSub.DefaultCell.Border = 0;
                    tableSub.DefaultCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                    tableSub.AddCell(new Phrase("Subject To Bangalore Jurisdiction", font2));

                    PdfPCell celSub = new PdfPCell(tableSub);
                    celSub.Border = 0;
                    celSub.Colspan = 2;
                    tableFooter.AddCell(celSub);

                    //=======
                   

                    doc.Add(tableHeader);
                    doc.Add(ParagraphDocumentName);
                    doc.Add(TableAddress);
                    doc.Add(TableItemDetail);
                    doc.Add(TabTaxDetail);
                    doc.Add(tableFooter);


                    doc.Close();
                    
                    if(ioh.status == 0 && ioh.DocumentStatus < 99)
                    {
                        String wmurl = "";
                        wmurl = "004.png";
                        PrintWaterMark.PdfStampWithNewFile(wmurl, sfd.FileName);
                    }
                    if (ioh.status == 98)
                    {
                        String wmurl = "";
                        wmurl = "003.png";
                        PrintWaterMark.PdfStampWithNewFile(wmurl, sfd.FileName);
                    }
                    MessageBox.Show("Document Saved");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error-" + ex.ToString());
                    MessageBox.Show("Failed TO Save");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception : " + ex.ToString());
                MessageBox.Show("Failed TO Save");
            }
        }
        protected class MyEvent : PdfPageEventHelper
        {

            PdfTemplate total;
            Font font2 = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            public override void OnOpenDocument(PdfWriter writer, iTextSharp.text.Document document)
            {
                total = writer.DirectContent.CreateTemplate(10, 16);
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
                    table.DefaultCell.FixedHeight = 5;
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
                            document.BottomMargin - 4, writer.DirectContent);

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
    //public static class Extensions
    //{
    //    //Creating an Extension method to compare two string using contains with case insensitive
    //    public static bool CaseInsensitiveContains(this string text, string value,
    //        StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
    //    {
    //        return text.IndexOf(value, stringComparison) >= 0;
    //    }
    //}
    public static class MyUtility
    {
        public static string Tab
        {
            get { return "\t"; }
        }
    }
}
