using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Data;

namespace CSLERP.DBData
{
    public class invoiceoutheader
    {
        public int RowID { get; set; }
        public double FreightCharge { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }

        public int InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int TrackingNo { get; set; }
        public DateTime TrackingDate { get; set; }
        public string ConsigneeID { get; set; }
        public string ConsigneeName { get; set; }
        public string TermsOfPayment { get; set; }
        public string Description { get; set; }
        public string TransportationMode { get; set; }
        public string TransportationModeName { get; set; }
        public string TransportationType { get; set; }
        public string TransportationTypeName { get; set; }
        public string Transporter { get; set; }
        public string TransporterName { get; set; }
        public string CurrencyID { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryStateCode { get; set; }
        public int BankAcReference { get; set; }
        public decimal AmountReceived { get; set; }
        public decimal TDSReceived { get; set; }
        public string ProjectID { get; set; }
        public double INRConversionRate { get; set; }
        public string ADCode { get; set; }
        public string EntryPort { get; set; }
        public string ExitPort { get; set; }
        public string FinalDestinatoinCountryID { get; set; }
        public string FinalDestinatoinCountryName { get; set; }
        public string OriginCountryID { get; set; }
        public string OriginCountryName { get; set; }
        public string FinalDestinationPlace { get; set; }
        public string PreCarriageTransportationMode { get; set; }
        public string PreCarriageTransportationName { get; set; }
        public string PreCarrierReceiptPlace { get; set; }
        public string TermsOfDelivery { get; set; }
        public string Remarks { get; set; }
        public string CommentStatus { get; set; }
        public string Comments { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string CreatorName { get; set; }
        public string ForwarderName { get; set; }
        public string ApproverName { get; set; }
        public string ForwarderList { get; set; }
        public int status { get; set; }
        public int DocumentStatus { get; set; }
        public double ProductValue { get; set; }
        public double TaxAmount { get; set; }
        public double ProductValueINR { get; set; }
        public double TaxAmountINR { get; set; }
        public double InvoiceAmount { get; set; }
        public double INRAmount { get; set; }
        public string ReverseCharge { get; set; }
        public invoiceoutheader()
        {
            Comments = "";
        }

        public string TrackingNos { get; set; }
        public string TrackingDates { get; set; }

        //SJV Ref In INvoice
        public int SJVTNo { get; set; }
        public DateTime SJVTDate { get; set; }
        public int SJVNo { get; set; }
        public DateTime SJVDate { get; set; }

        public string SpecialNote { get; set; }
        public string OfficeID { get; set; }
    }
    public class invoiceoutdetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int PONo { get; set; }
        public DateTime PODate { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string Unit { get; set; }
        public string CustomerItemDescription { get; set; }
        // public string WorkDescription { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Tax { get; set; }
        public int WarrantyDays { get; set; }
        public int POItemReferenceNo { get; set; }
        public string TaxDetails { get; set; }
        public int MRNNo { get; set; }
        public DateTime MRNDate { get; set; }
        public string BatchNo { get; set; }
        public string SerielNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double PurchaseQuantity { get; set; }
        public double PurchasePrice { get; set; }
        public double PurchaseTax { get; set; }
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public int StockReferenceNo { get; set; }
        public string TaxCode { get; set; }
        public string HSNCode { get; set; }
    }
    class invoiceoutreceipts
    {
        public int RowID { get; set; }
        public string InvoiceDocumentID { get; set; }
        public string InvoiceDocumentName { get; set; }
        public string CustomerID { get; set; }
        public int InvoiceOutNo { get; set; }
        public DateTime InvoiceOutDate { get; set; }
        public int InvoiceOutTemporaryNo { get; set; }
        public DateTime InvoiceOutTemporaryDate { get; set; }
        public int RVTemporaryNo { get; set; }
        public DateTime RVTemporaryDate { get; set; }
        public int RVNo { get; set; }
        public DateTime RVDate { get; set; }
        public decimal Amount { get; set; }
        public decimal TDSAmount { get; set; }
        public string RVDocumentID { get; set; }
    }
    class InvoiceOutHeaderDB
    {
        public List<invoiceoutheader> getFilteredInvoiceOutHeader(string userList, int opt, string userCommentStatusString)
        {
            invoiceoutheader ioh;
            List<invoiceoutheader> IOHeaders = new List<invoiceoutheader>();
            try
            {
                //approved user comment status string
                string acStr = "";
                try
                {
                    acStr = userCommentStatusString.Substring(0, userCommentStatusString.Length - 2) + "1" + Main.delimiter2;
                }
                catch (Exception ex)
                {
                    acStr = "";
                }
                //-----
                string query1 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " InvoiceNo,InvoiceDate,TrackingNos,TrackingDates,Consignee,ConsigneeName,TermsOfPayment,Description,TransportationMode,TransportationModeName,TransportationType," +
                    " TransportationTypeName,Transporter,TransporterName,CurrencyID,INRConversionRate,ADCode,EntryPort, " +
                    " ExitPort,FinalDestinationCountryID,FinalDestinationCountryName,OriginCountryID,OriginCountryName," +
                    "FinalDestinationPlace,PreCarriageTransportationMode,PreCarriageTransportationName,PreCarrierReceiptPlace,TermsOfDelivery,Remarks," +
                    "CommentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,Status,DocumentStatus,ProductValue,TaxAmount,InvoiceAmount,INRAmount,ProjectID " +
                    ", ProductValueINR, TaxAmountINR,BankAcReference,FreightCharge,DeliveryAddress,DeliveryStateCode,SJVTNo,SJVTDate,SJVNo,SJVDate,SpecialNote,ReverseCharge from ViewInvoiceOutHeader" +
                    " where ((forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (commentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) and Status not in (7,98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query2 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " InvoiceNo,InvoiceDate,TrackingNos,TrackingDates,Consignee,ConsigneeName,TermsOfPayment,Description,TransportationMode,TransportationModeName,TransportationType," +
                    " TransportationTypeName,Transporter,TransporterName,CurrencyID,INRConversionRate,ADCode,EntryPort, " +
                    " ExitPort,FinalDestinationCountryID,FinalDestinationCountryName,OriginCountryID,OriginCountryName," +
                    "FinalDestinationPlace,PreCarriageTransportationMode,PreCarriageTransportationName,PreCarrierReceiptPlace,TermsOfDelivery,Remarks," +
                    "CommentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,Status,DocumentStatus,ProductValue,TaxAmount,InvoiceAmount,INRAmount,ProjectID " +
                    " , ProductValueINR, TaxAmountINR,BankAcReference,FreightCharge,DeliveryAddress,DeliveryStateCode,SJVTNo,SJVTDate,SJVNo,SJVDate,SpecialNote,ReverseCharge from ViewInvoiceOutHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) and Status not in (7,98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query3 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " InvoiceNo,InvoiceDate,TrackingNos,TrackingDates,Consignee,ConsigneeName,TermsOfPayment,Description,TransportationMode,TransportationModeName,TransportationType," +
                    " TransportationTypeName,Transporter,TransporterName,CurrencyID,INRConversionRate,ADCode,EntryPort, " +
                    " ExitPort,FinalDestinationCountryID,FinalDestinationCountryName,OriginCountryID,OriginCountryName," +
                    "FinalDestinationPlace,PreCarriageTransportationMode,PreCarriageTransportationName,PreCarrierReceiptPlace,TermsOfDelivery,Remarks," +
                    "CommentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,Status,DocumentStatus,ProductValue,TaxAmount,InvoiceAmount,INRAmount,ProjectID " +
                    " , ProductValueINR, TaxAmountINR,BankAcReference,FreightCharge,DeliveryAddress,DeliveryStateCode,SJVTNo,SJVTDate,SJVNo,SJVDate,SpecialNote,ReverseCharge from ViewInvoiceOutHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99  and Status = 1)  order by InvoiceDate desc,DocumentID asc,InvoiceNo desc";

                string query4 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                   " InvoiceNo,InvoiceDate,TrackingNos,TrackingDates,Consignee,ConsigneeName,TermsOfPayment,Description,TransportationMode,TransportationModeName,TransportationType," +
                   " TransportationTypeName,Transporter,TransporterName,CurrencyID,INRConversionRate,ADCode,EntryPort, " +
                   " ExitPort,FinalDestinationCountryID,FinalDestinationCountryName,OriginCountryID,OriginCountryName," +
                   "FinalDestinationPlace,PreCarriageTransportationMode,PreCarriageTransportationName,PreCarrierReceiptPlace,TermsOfDelivery,Remarks," +
                   "CommentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,Status,DocumentStatus,ProductValue,TaxAmount,InvoiceAmount,INRAmount,ProjectID " +
                   " , ProductValueINR, TaxAmountINR,BankAcReference,FreightCharge,DeliveryAddress,DeliveryStateCode,SJVTNo,SJVTDate,SJVNo,SJVDate,SpecialNote,ReverseCharge from ViewInvoiceOutHeader" +
                   " where ((createuser='" + Login.userLoggedIn + "'" +
                   " or ForwarderList like '%" + userList + "%'" +
                   " or commentStatus like '%" + acStr + "%'" +
                   " or approveUser='" + Login.userLoggedIn + "')" +
                   " and InvoiceNo > 0 and Status = 98)  order by InvoiceDate desc,DocumentID asc,InvoiceNo desc";

                string query6 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " InvoiceNo,InvoiceDate,TrackingNos,TrackingDates,Consignee,ConsigneeName,TermsOfPayment,Description,TransportationMode,TransportationModeName,TransportationType," +
                    " TransportationTypeName,Transporter,TransporterName,CurrencyID,INRConversionRate,ADCode,EntryPort, " +
                    " ExitPort,FinalDestinationCountryID,FinalDestinationCountryName,OriginCountryID,OriginCountryName," +
                    "FinalDestinationPlace,PreCarriageTransportationMode,PreCarriageTransportationName,PreCarrierReceiptPlace,TermsOfDelivery,Remarks," +
                    "CommentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,Status,DocumentStatus,ProductValue,TaxAmount,InvoiceAmount,INRAmount,ProjectID " +
                    " , ProductValueINR, TaxAmountINR,BankAcReference,FreightCharge,DeliveryAddress,DeliveryStateCode,SJVTNo,SJVTDate,SJVNo,SJVDate,SpecialNote,ReverseCharge from ViewInvoiceOutHeader" +
                    " where  DocumentStatus = 99  and Status = 1order by InvoiceDate desc,DocumentID asc,InvoiceNo desc";

                string query7 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                   " InvoiceNo,InvoiceDate,TrackingNos,TrackingDates,Consignee,ConsigneeName,TermsOfPayment,Description,TransportationMode,TransportationModeName,TransportationType," +
                   " TransportationTypeName,Transporter,TransporterName,CurrencyID,INRConversionRate,ADCode,EntryPort, " +
                   " ExitPort,FinalDestinationCountryID,FinalDestinationCountryName,OriginCountryID,OriginCountryName," +
                   "FinalDestinationPlace,PreCarriageTransportationMode,PreCarriageTransportationName,PreCarrierReceiptPlace,TermsOfDelivery,Remarks," +
                   "CommentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,Status,DocumentStatus,ProductValue,TaxAmount,InvoiceAmount,INRAmount,ProjectID " +
                   " , ProductValueINR, TaxAmountINR,BankAcReference,FreightCharge,DeliveryAddress,DeliveryStateCode,SJVTNo,SJVTDate,SJVNo,SJVDate,SpecialNote,ReverseCharge from ViewInvoiceOutHeader" +
                   " where InvoiceNo > 0 and Status = 98  order by InvoiceDate desc,DocumentID asc,InvoiceNo desc";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "";
                switch (opt)
                {
                    case 1:
                        query = query1;
                        break;
                    case 2:
                        query = query2;
                        break;
                    case 3:
                        query = query3;
                        break;
                    case 4:
                        query = query4;
                        break;
                    //case 5:
                    //    query = query5;
                    //    break;
                    case 6:
                        query = query6;
                        break;
                    case 7:
                        query = query7;
                        break;
                    default:
                        query = "";
                        break;
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ioh = new invoiceoutheader();
                    ioh.RowID = reader.GetInt32(0);
                    ioh.DocumentID = reader.GetString(1);
                    ioh.DocumentName = reader.GetString(2);
                    ioh.TemporaryNo = reader.GetInt32(3);
                    ioh.TemporaryDate = reader.GetDateTime(4);
                    ioh.InvoiceNo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        ioh.InvoiceDate = reader.GetDateTime(6);
                    }


                    //ioh.TrackingNo = reader.GetInt32(7);
                    //ioh.TrackingDate = reader.GetDateTime(8);
                    ioh.TrackingNos = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    ioh.TrackingDates = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    ioh.ConsigneeID = reader.GetString(9);
                    ioh.ConsigneeName = reader.GetString(10);
                    ioh.TermsOfPayment = reader.GetString(11);
                    ioh.Description = reader.IsDBNull(12) ? "" : reader.GetString(12);

                    ioh.TransportationMode = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    ioh.TransportationModeName = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    ioh.TransportationType = reader.IsDBNull(15) ? "" : reader.GetString(15);
                    ioh.TransportationTypeName = reader.IsDBNull(16) ? "" : reader.GetString(16);
                    ioh.Transporter = reader.IsDBNull(17) ? "" : reader.GetString(17);
                    ioh.TransporterName = reader.IsDBNull(18) ? "" : reader.GetString(18);

                    //popih.ValidityDate = reader.GetDateTime(12);
                    ioh.CurrencyID = reader.GetString(19);
                    // ioh.TaxCode = reader.GetString(20);
                    ioh.INRConversionRate = reader.GetDouble(20);
                    ioh.ADCode = reader.GetString(21);
                    ioh.EntryPort = reader.GetString(22);
                    ioh.ExitPort = reader.GetString(23);

                    ioh.FinalDestinatoinCountryID = reader.GetString(24);
                    if (!reader.IsDBNull(25))
                    {
                        ioh.FinalDestinatoinCountryName = reader.GetString(25);
                    }
                    else
                    {
                        ioh.FinalDestinatoinCountryName = "";
                    }

                    ioh.OriginCountryID = reader.GetString(26);
                    if (!reader.IsDBNull(27))
                    {
                        ioh.OriginCountryName = reader.GetString(27);
                    }
                    else
                    {
                        ioh.OriginCountryName = "";
                    }

                    ioh.FinalDestinationPlace = reader.GetString(28);
                    ioh.PreCarriageTransportationMode = reader.GetString(29);
                    if (!reader.IsDBNull(30))
                    {
                        ioh.PreCarriageTransportationName = reader.GetString(30);
                    }
                    else
                    {
                        ioh.PreCarriageTransportationName = "";
                    }

                    ioh.PreCarrierReceiptPlace = reader.GetString(31);
                    ioh.TermsOfDelivery = reader.GetString(32);
                    ioh.Remarks = reader.GetString(33);
                    if (!reader.IsDBNull(34))
                    {
                        ioh.CommentStatus = reader.GetString(34);
                    }
                    else
                    {
                        ioh.CommentStatus = "";
                    }
                    ioh.CreateUser = reader.GetString(35);
                    ioh.ForwardUser = reader.GetString(36);
                    ioh.ApproveUser = reader.GetString(37);
                    ioh.CreatorName = reader.GetString(38);
                    ioh.CreateTime = reader.GetDateTime(39);
                    ioh.ForwarderName = reader.GetString(40);
                    ioh.ApproverName = reader.GetString(41);
                    if (!reader.IsDBNull(42))
                    {
                        ioh.ForwarderList = reader.GetString(42);
                    }
                    else
                    {
                        ioh.ForwarderList = "";
                    }

                    // ioh.Remarks = reader.GetString(44);
                    ioh.status = reader.GetInt32(43);
                    ioh.DocumentStatus = reader.GetInt32(44);
                    ioh.ProductValue = reader.GetDouble(45);
                    ioh.TaxAmount = reader.GetDouble(46);
                    ioh.InvoiceAmount = reader.GetDouble(47);
                    ioh.INRAmount = reader.GetDouble(48);
                    ioh.ProjectID = reader.IsDBNull(49) ? "" : reader.GetString(49);
                    ioh.ProductValueINR = reader.GetDouble(50);
                    ioh.TaxAmountINR = reader.GetDouble(51);
                    ioh.BankAcReference = reader.IsDBNull(52) ? 0 : reader.GetInt32(52);
                    ioh.FreightCharge = reader.IsDBNull(53) ? 0 : reader.GetDouble(53);
                    ioh.DeliveryAddress = reader.IsDBNull(54) ? "" : reader.GetString(54);
                    ioh.DeliveryStateCode = reader.IsDBNull(55) ? "" : reader.GetString(55);

                    ioh.SJVTNo = reader.IsDBNull(56) ? 0 : reader.GetInt32(56);
                    ioh.SJVTDate = reader.IsDBNull(57) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(57);
                    ioh.SJVNo = reader.IsDBNull(58) ? 0 : reader.GetInt32(58);
                    ioh.SJVDate = reader.IsDBNull(59) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(59);

                    ioh.SpecialNote = reader.IsDBNull(60) ? "" : reader.GetString(60);
                    ioh.ReverseCharge = reader.IsDBNull(61) ? "" : reader.GetString(61);
                    IOHeaders.Add(ioh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice out Header Details");
            }
            return IOHeaders;
        }



        public static List<invoiceoutdetail> getInvoiceOutDetail(invoiceoutheader ioh)
        {
            invoiceoutdetail iod;
            List<invoiceoutdetail> IODetail = new List<invoiceoutdetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query1 = "select RowID,DocumentID,TemporaryNo, TemporaryDate,StockItemID,StockItemName,ModelNo,ModelName, " +
                   " ISNULL(CustomerItemDescription,' ')," +
                   " Quantity,Price,Tax,WarrantyDays,TaxDetails,POItemReferenceNo,MRNNo,MRNDate,BatchNo,ExpiryDate,SerialNO, " +
                   " PurchaseQuantity, PurchasePrice, PurchaseTax, SupplierId,SupplierName, StockReferenceNo,Unit,TaxCode,PONo,PODate,HSNCode " +
                   "from ViewInvoiceOutDetail  where " +
                   " DocumentID='" + ioh.DocumentID + "'" +
                   " and TemporaryNo=" + ioh.TemporaryNo +
                   " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                   " order by StockItemID";
                SqlCommand cmd = new SqlCommand(query1, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    iod = new invoiceoutdetail();
                    iod.RowID = reader.GetInt32(0);
                    iod.DocumentID = reader.GetString(1);
                    iod.TemporaryNo = reader.GetInt32(2);
                    iod.TemporaryDate = reader.GetDateTime(3).Date;
                    iod.StockItemID = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                    {
                        iod.StockItemName = reader.GetString(5);
                    }
                    else
                    {
                        iod.StockItemName = "";
                    }
                    iod.ModelNo = reader.IsDBNull(6) ? "NA" : reader.GetString(6);
                    iod.ModelName = reader.IsDBNull(7) ? "NA" : reader.GetString(7);
                    iod.CustomerItemDescription = reader.GetString(8);
                    iod.Quantity = reader.GetDouble(9);
                    iod.Price = reader.GetDouble(10);
                    iod.Tax = reader.GetDouble(11);
                    iod.WarrantyDays = reader.GetInt32(12);
                    iod.TaxDetails = reader.GetString(13);
                    iod.POItemReferenceNo = reader.GetInt32(14);

                    iod.MRNNo = reader.GetInt32(15);
                    iod.MRNDate = reader.GetDateTime(16);
                    iod.BatchNo = reader.GetString(17);
                    iod.SerielNo = reader.IsDBNull(19) ? "NA" : reader.GetString(19);
                    iod.ExpiryDate = reader.GetDateTime(18);
                    iod.PurchaseQuantity = reader.GetDouble(20);
                    iod.PurchasePrice = reader.GetDouble(21);
                    iod.PurchaseTax = reader.GetDouble(22);
                    iod.SupplierID = reader.GetString(23);
                    iod.SupplierName = reader.IsDBNull(24) ? "NA" : reader.GetString(24);
                    iod.StockReferenceNo = reader.GetInt32(25);
                    iod.Unit = reader.IsDBNull(26) ? "" : reader.GetString(26);
                    iod.TaxCode = reader.IsDBNull(27) ? "" : reader.GetString(27);
                    iod.PONo = reader.IsDBNull(28) ? 0 : reader.GetInt32(28);
                    iod.PODate = reader.IsDBNull(29) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(29);
                    iod.HSNCode = reader.IsDBNull(30) ? "" : reader.GetString(30);
                    IODetail.Add(iod);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying InvoiceOutHeader Details");
            }
            return IODetail;
        }
        public Boolean validateInvoiceOutHeader(invoiceoutheader ioh)
        {
            Boolean status = true;
            try
            {
                if (ioh.DocumentID.Trim().Length == 0 || ioh.DocumentID == null)
                {
                    return false;
                }

                if (ioh.ConsigneeID.Trim().Length == 0 || ioh.ConsigneeID == null)
                {
                    return false;
                }
                //if (ioh.TrackingNo == 0)
                //{
                //    return false;
                //}
                ////if (ioh.FreightCharge == 0)
                ////{
                ////    return false;
                ////}
                if (ioh.TrackingNos == null || ioh.TrackingNos.Trim().Length == 0)
                {
                    return false;
                }
                if (ioh.DeliveryAddress == null || ioh.DeliveryAddress.Trim().Length == 0)
                {
                    return false;
                }
                if (ioh.TrackingDates == null || ioh.TrackingDates.Trim().Length == 0)
                {
                    return false;
                }
                if (ioh.TermsOfPayment.Trim().Length == 0 || ioh.TermsOfPayment == null)
                {
                    return false;
                }
                if (ioh.ProjectID.Trim().Length == 0 || ioh.ProjectID == null)
                {
                    return false;
                }
                if ((ioh.DocumentID == "PRODUCTEXPORTINVOICEOUT") || (ioh.DocumentID == "PRODUCTINVOICEOUT"))
                {
                    if (ioh.TransportationMode.Trim().Length == 0 || ioh.TransportationType == null)
                    {
                        return false;
                    }
                    if (ioh.Transporter.Trim().Length == 0 || ioh.Transporter == null)
                    {
                        return false;
                    }
                    if (ioh.TransportationType.Trim().Length == 0 || ioh.ConsigneeID == null)
                    {
                        return false;
                    }
                }
                if (ioh.CurrencyID.Trim().Length == 0 || ioh.CurrencyID == null)
                {
                    return false;
                }
                //if (ioh.TaxCode.Trim().Length == 0 || ioh.TaxCode == null)
                //{
                //    return false;
                //}
                if ((ioh.DocumentID == "PRODUCTEXPORTINVOICEOUT") || (ioh.DocumentID == "SERVICEEXPORTINVOICEOUT"))
                {
                    if (ioh.ADCode.Trim().Length == 0 || ioh.ADCode == null)
                    {
                        return false;
                    }
                    if (ioh.EntryPort.Trim().Length == 0 || ioh.EntryPort == null)
                    {
                        return false;
                    }
                    if (ioh.ExitPort.Trim().Length == 0 || ioh.ExitPort == null)
                    {
                        return false;
                    }
                    if (ioh.FinalDestinatoinCountryID.Trim().Length == 0 || ioh.FinalDestinatoinCountryID == null)
                    {
                        return false;
                    }
                    if (ioh.PreCarriageTransportationMode.Trim().Length == 0 || ioh.PreCarriageTransportationMode == null)
                    {
                        return false;
                    }
                    if (ioh.PreCarrierReceiptPlace.Trim().Length == 0 || ioh.PreCarrierReceiptPlace == null)
                    {
                        return false;
                    }
                    if (ioh.TermsOfDelivery.Trim().Length == 0 || ioh.TermsOfDelivery == null)
                    {
                        return false;
                    }
                    if (ioh.OriginCountryID.Trim().Length == 0 || ioh.OriginCountryID == null)
                    {
                        return false;
                    }
                    if (ioh.FinalDestinationPlace.Trim().Length == 0 || ioh.FinalDestinationPlace == null)
                    {
                        return false;
                    }
                }

                if (ioh.INRConversionRate == 0)
                {
                    return false;
                }
                if (ioh.BankAcReference == 0)
                {
                    return false;
                }
                if (ioh.ProductValue == 0)
                {
                    return false;
                }
                //if (ioh.INRAmount == 0)
                //{
                //    return false;
                //}
                if (ioh.InvoiceAmount == 0)
                {
                    return false;
                }
                if (ioh.ProductValueINR == 0)
                {
                    return false;
                }
                if (ioh.INRAmount == 0)
                {
                    return false;
                }
                if (ioh.Remarks.Trim().Length == 0 || ioh.Remarks == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public Boolean forwardInvoiceHeader(invoiceoutheader ioh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InvoiceOutHeader set DocumentStatus=" + (ioh.DocumentStatus + 1) +
                    ", forwardUser='" + ioh.ForwardUser + "'" +
                    ", commentStatus='" + ioh.CommentStatus + "'" +
                    ", ForwarderList='" + ioh.ForwarderList + "'" +
                    " where DocumentID='" + ioh.DocumentID + "'" +
                    " and TemporaryNo=" + ioh.TemporaryNo +
                    " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InvoiceOutHeader", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        public Boolean reverseInvoiceOut(invoiceoutheader ioh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InvoiceOutHeader set DocumentStatus=" + ioh.DocumentStatus +
                    ", forwardUser='" + ioh.ForwardUser + "'" +
                    ", commentStatus='" + ioh.CommentStatus + "'" +
                    ", ForwarderList='" + ioh.ForwarderList + "'" +
                    " where DocumentID='" + ioh.DocumentID + "'" +
                    " and TemporaryNo=" + ioh.TemporaryNo +
                    " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InvoiceOutHeader", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        public Boolean ApproveInvoiceOut(invoiceoutheader ioh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InvoiceOutHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + ioh.CommentStatus + "'" +
                    ", InvoiceNo=" + ioh.InvoiceNo +
                    ", InvoiceDate=convert(date, getdate())" +
                    " where DocumentID='" + ioh.DocumentID + "'" +
                    " and TemporaryNo=" + ioh.TemporaryNo +
                    " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InvoiceOutHeader", "", updateSQL) +
                Main.QueryDelimiter;

                string narration = "Sales against Invoice No " + ioh.InvoiceNo + "," +
               "Dated " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy") + "," +
               "Party:" + ioh.ConsigneeName;

                int SJVNo = 0; //Journal No
                DateTime SJVDate = DateTime.Parse("1900-01-01"); //Journal Date
                //int SJVTempNo = 0; //Temporary No
                //DateTime SJVTempDate = DateTime.Parse("1900-01-01"); //Temporary Date

                if (ioh.SJVNo == 0 && ioh.SJVTNo > 0) // JV Available but not approved
                {
                    SJVNo = DocumentNumberDB.getNewNumber("SJV", 2);
                    SJVDate = UpdateTable.getSQLDateTime();
                }
                else //JV Available and approved // JV Not available
                {
                    SJVNo = ioh.SJVNo;
                    SJVDate = ioh.SJVDate;
                }

                //if (ioh.SJVTNo == 0)
                //{
                //    SJVTempNo = DocumentNumberDB.getNewNumber("SJV", 1);
                //    SJVTempDate = UpdateTable.getSQLDateTime();
                //}
                //else
                //{
                //    SJVTempNo = ioh.SJVNo;
                //    SJVTempDate = ioh.SJVDate;
                //}
                updateSQL = "update SJVHeader set DocumentStatus=99, status=1 ,InvReferenceNo = " + ioh.RowID +
                  ", ApproveUser='" + Login.userLoggedIn + "'" +
                  ", JournalNo=" + SJVNo +
                  ", JournalDate='" + SJVDate.ToString("yyyy-MM-dd") +
                   "', Narration='" + narration + "'" +
                  " where InvDocumentID='" + ioh.DocumentID + "'" +
                  " and InvTempNo=" + ioh.TemporaryNo +
                  " and InvTempDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "SJVHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "update InvoiceOutHeader set SJVNo='" + SJVNo + "'" +
                  ", SJVDate='" + SJVDate.ToString("yyyy-MM-dd") + "'" +
                 " where DocumentID='" + ioh.DocumentID + "'" +
                 " and TemporaryNo=" + ioh.TemporaryNo +
                 " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InvoiceOutHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "update InvoiceOutReceipts set " +
                   " InvoiceOutNo=" + ioh.InvoiceNo +
                   ", InvoiceOutDate=convert(date, getdate())" +
                   "  where InvoiceDocumentID='" + ioh.DocumentID + "'" +
                   " and InvoiceOutTemporaryNo=" + ioh.TemporaryNo +
                   " and InvoiceOutTemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InvoiceOutReceipts", "", updateSQL) +
                Main.QueryDelimiter;

                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        public static string getUserComments(string docid, int tempno, DateTime tempdate)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from InvoiceOutHeader where DocumentID='" + docid + "'" +
                        " and TemporaryNo=" + tempno +
                        " and TemporaryDate='" + tempdate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    cmtString = reader.GetString(0);
                }
                conn.Open();
            }
            catch (Exception ex)
            {
            }
            return cmtString;
        }
        public static ListView getInvoiceOutListView(string custID)
        {
            ListView lv = new ListView();
            try
            {

                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                InvoiceOutHeaderDB IODB = new InvoiceOutHeaderDB();
                List<invoiceoutheader> IOList = IODB.getInvoiceOutHeaderDetail(custID);
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Document ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Invoice No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Invoice Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ConsigneeID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Consignee", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Invoice Amount", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Remarks", -2, HorizontalAlignment.Left);
                lv.Columns[5].Width = 100;
                lv.Columns[4].Width = 0;
                lv.Columns[7].Width = 200;
                foreach (invoiceoutheader ioh in IOList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(ioh.DocumentID.ToString());
                    item1.SubItems.Add(ioh.InvoiceNo.ToString());
                    item1.SubItems.Add(ioh.InvoiceDate.ToString("yyyy-MM-dd"));
                    item1.SubItems.Add(ioh.ConsigneeID);
                    item1.SubItems.Add(ioh.ConsigneeName);
                    item1.SubItems.Add(ioh.InvoiceAmount.ToString());
                    item1.SubItems.Add(ioh.Remarks.ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public List<invoiceoutheader> getInvoiceOutHeaderDetail(string custID)
        {
            invoiceoutheader ioh;
            List<invoiceoutheader> IOHeaders = new List<invoiceoutheader>();
            try
            {
                string query = "select DocumentID, DocumentName," +
                    " InvoiceNo,InvoiceDate,Consignee,ConsigneeName,InvoiceAmount,Remarks from ViewInvoiceOutHeader" +
                    " where DocumentStatus = 99 and Status = 1 and Consignee = '" + custID + "' order by InvoiceDate desc";

                SqlConnection conn = new SqlConnection(Login.connString);

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ioh = new invoiceoutheader();
                    ioh.DocumentID = reader.GetString(0);
                    ioh.DocumentName = reader.GetString(1);
                    ioh.InvoiceNo = reader.GetInt32(2);
                    ioh.InvoiceDate = reader.GetDateTime(3);
                    ioh.ConsigneeID = reader.GetString(4);
                    ioh.ConsigneeName = reader.GetString(5);
                    ioh.InvoiceAmount = reader.GetDouble(6);
                    ioh.Remarks = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    IOHeaders.Add(ioh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying InvoiceOut Header Details");
            }
            return IOHeaders;
        }
        public static string getIOHDtlsForProjectTrans(string projectID)
        {
            string str = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select COUNT(*), SUM(InvoiceAmount) from InvoiceOutHeader where ProjectID = '" + projectID + "' and DocumentStatus = 99";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    double dd = reader.IsDBNull(1) ? 0 : reader.GetDouble(1);
                    str = reader.GetInt32(0) + "-" + dd;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return str;
        }
        public static List<invoiceoutheader> getRVINFOForProjectTrans(string projectID)
        {
            invoiceoutheader ioh;
            List<invoiceoutheader> IOHeaders = new List<invoiceoutheader>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select InvoiceNo,InvoiceDate,ConsigneeName,ProductValue,TaxAmount,InvoiceAmount,ProjectID from ViewInvoiceOutHeader where ProjectID = '" + projectID + "' and DocumentStatus = 99";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ioh = new invoiceoutheader();
                    ioh.InvoiceNo = reader.GetInt32(0);
                    ioh.InvoiceDate = reader.GetDateTime(1);
                    ioh.ConsigneeName = reader.GetString(2);
                    ioh.ProductValue = reader.GetDouble(3);
                    ioh.TaxAmount = reader.GetDouble(4);
                    ioh.InvoiceAmount = reader.GetDouble(5);
                    ioh.ProjectID = reader.GetString(6);
                    IOHeaders.Add(ioh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return IOHeaders;
        }
        public Boolean updateInvoiceOutHeaderAndDetail(invoiceoutheader ioh, invoiceoutheader previoh,
                                                List<invoiceoutdetail> iodList, List<invoiceoutreceipts> receiveList)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InvoiceOutHeader set " +
                   " TrackingNos='" + ioh.TrackingNos + // For List of POSElected
                    "',TrackingDates='" + ioh.TrackingDates + // For List of PODateSElected
                   "', Consignee='" + ioh.ConsigneeID +
                     "', TermsOfPayment='" + ioh.TermsOfPayment +
                   "', TransportationMode='" + ioh.TransportationMode +
                   "', TransportationType='" + ioh.TransportationType +
                   "', Transporter ='" + ioh.Transporter +
                   "', CurrencyID='" + ioh.CurrencyID +
                    "', ProjectID='" + ioh.ProjectID +
                       "', DeliveryAddress='" + ioh.DeliveryAddress +
                    "', DeliveryStateCode='" + ioh.DeliveryStateCode +
                   "', INRConversionRate='" + ioh.INRConversionRate +
                   "', ADCode='" + ioh.ADCode +
                     "', EntryPort='" + ioh.EntryPort +
                   "', ExitPort='" + ioh.ExitPort +
                     "', FinalDestinationCountryID='" + ioh.FinalDestinatoinCountryID +
                   "', OriginCountryID ='" + ioh.OriginCountryID +
                      "', FinalDestinationPlace='" + ioh.FinalDestinationPlace +
                   "', PreCarriageTransportationMode ='" + ioh.PreCarriageTransportationMode +
                     "', PreCarrierReceiptPlace='" + ioh.PreCarrierReceiptPlace +
                   "', TermsOfDelivery ='" + ioh.TermsOfDelivery +
                   "', Remarks='" + ioh.Remarks +
                     "', ReverseCharge='" + ioh.ReverseCharge +
                   "', Comments='" + ioh.Comments +
                   "', CommentStatus='" + ioh.CommentStatus +
                   "', ProductValue=" + ioh.ProductValue +
                    ", BankAcReference=" + ioh.BankAcReference +
                    ", FreightCharge=" + ioh.FreightCharge +
                   ", TaxAmount=" + ioh.TaxAmount +
                       ", ProductValueINR=" + ioh.ProductValueINR +
                   ", TaxAmountINR=" + ioh.TaxAmountINR +
                   ", InvoiceAmount=" + ioh.InvoiceAmount +
                   ", INRAmount=" + ioh.INRAmount +
                   ", ForwarderList='" + ioh.ForwarderList +
                    "', SpecialNote='" + ioh.SpecialNote +
                   "' where DocumentID='" + ioh.DocumentID + "'" +
                   " and TemporaryNo=" + ioh.TemporaryNo +
                   " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InvoiceOutHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from InvoiceOutDetail where DocumentID='" + ioh.DocumentID + "'" +
                     " and TemporaryNo=" + ioh.TemporaryNo +
                     " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InvoiceOutDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (invoiceoutdetail iod in iodList)
                {
                    updateSQL = "insert into InvoiceOutDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,PONo,PODate,StockItemID,ModelNo,CustomerItemDescription,TaxCode,Quantity,Price," +
                     " Tax,WarrantyDays,TaxDetails,POItemReferenceNo,MRNNo,MRNDate,BatchNo,ExpiryDate,SerialNO, " +
                   " PurchaseQuantity, PurchasePrice, PurchaseTax, SupplierId, StockReferenceNo) " +
                    "values ('" + ioh.DocumentID + "'," +
                    ioh.TemporaryNo + "," +
                    "'" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                      iod.PONo + "," +
                    "'" + iod.PODate.ToString("yyyy-MM-dd") + "'," +
                    "'" + iod.StockItemID + "'," +
                       "'" + iod.ModelNo + "'," +
                    "'" + iod.CustomerItemDescription + "'," +
                    "'" + iod.TaxCode + "'," +
                    iod.Quantity + "," +
                    iod.Price + "," +
                    iod.Tax + "," +
                    iod.WarrantyDays + "," +
                    "'" + iod.TaxDetails + "',"
                    + iod.POItemReferenceNo + "," +
                    iod.MRNNo + "," +
                    "'" + iod.MRNDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + iod.BatchNo + "'," +
                    "'" + iod.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                   "'" + iod.SerielNo + "'," +
                     iod.PurchaseQuantity + "," +
                    iod.PurchasePrice + "," +
                    iod.PurchaseTax + "," +
                    "'" + iod.SupplierID + "'," +
                      iod.StockReferenceNo + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InvoiceOutDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                }

                updateSQL = "Delete from InvoiceOutReceipts where InvoiceDocumentID = '" + ioh.DocumentID + "'" +
                   " and InvoiceOutTemporaryNo='" + ioh.TemporaryNo + "'" +
                   " and InvoiceOutTemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InvoiceOutReceipts", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (invoiceoutreceipts rec in receiveList)
                {
                    updateSQL = "insert into InvoiceOutReceipts " +
                    "(InvoiceDocumentID,CustomerID,InvoiceOutNo,InvoiceOutDate,InvoiceOutTemporaryNo,InvoiceOutTemporaryDate,RVDocumentID,RVTemporaryNo,RVTemporaryDate,RVNo,RVDate," +
                    "Amount) " +
                    "values ('" + ioh.DocumentID + "'," +
                     "'" + rec.CustomerID + "',0," +
                    "'1900-01-01'," +
                     +ioh.TemporaryNo + "," +
                    "'" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + rec.RVDocumentID + "',"
                        + rec.RVTemporaryNo + "," +
                    "'" + rec.RVTemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       +rec.RVNo + "," +
                    "'" + rec.RVDate.ToString("yyyy-MM-dd") + "'," +
                   rec.Amount + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InvoiceOutReceipts", "", updateSQL) +
                    Main.QueryDelimiter;
                }


                if (!UpdateTable.UT(utString))
                {
                    status = false;
                    MessageBox.Show("Transaction Exception Occured");
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        public Boolean InsertInvoiceOutHeaderAndDetail(invoiceoutheader ioh, List<invoiceoutdetail> iodList,
                                                            out int Tno, List<invoiceoutreceipts> receiveList)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            Tno = 0;
            try
            {
                ioh.TemporaryNo = DocumentNumberDB.getNumber(ioh.DocumentID, 1);
                if (ioh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                Tno = ioh.TemporaryNo;
                updateSQL = "update DocumentNumber set TempNo =" + ioh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + ioh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into InvoiceOutHeader " +
                    "(DocumentID,TemporaryNo,TemporaryDate,InvoiceNo,InvoiceDate,TrackingNos,TrackingDates,SpecialNote," +
                    "Consignee,TermsOfPayment,TransportationMode,TransportationType,ReverseCharge,DeliveryAddress,DeliveryStateCode,Transporter,CurrencyID,ProjectID,INRConversionRate,ADCode,EntryPort,ExitPort," +
                    "FinalDestinationCountryID,OriginCountryID,FinalDestinationPlace,PreCarriageTransportationMode,PreCarrierReceiptPlace,TermsOfDelivery,Remarks,Comments,CommentStatus," +
                    "BankAcReference,FreightCharge,ProductValue,TaxAmount,ProductValueINR,TaxAmountINR,InvoiceAmount,INRAmount,ForwarderList,Status,DocumentStatus,CreateTime,CreateUser)" +
                    " values (" +
                    "'" + ioh.DocumentID + "'," +
                    ioh.TemporaryNo + "," +
                    "'" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    ioh.InvoiceNo + "," +
                    "'" + ioh.InvoiceDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + ioh.TrackingNos + "'," +             // For List of POSElected
                    "'" + ioh.TrackingDates + "'," +     // For List of PODateSElected
                     "'" + ioh.SpecialNote + "'," +
                    "'" + ioh.ConsigneeID + "'," +
                    "'" + ioh.TermsOfPayment + "'," +
                    "'" + ioh.TransportationMode + "'," +
                    "'" + ioh.TransportationType + "'," +
                     "'" + ioh.ReverseCharge + "'," +
                     "'" + ioh.DeliveryAddress + "'," +
                    "'" + ioh.DeliveryStateCode + "'," +

                    "'" + ioh.Transporter + "'," +
                      "'" + ioh.CurrencyID + "'," +
                     "'" + ioh.ProjectID + "'," +
                    +ioh.INRConversionRate + "," +
                    "'" + ioh.ADCode + "'," +
                    "'" + ioh.EntryPort + "'," +
                    "'" + ioh.ExitPort + "'," +
                    "'" + ioh.FinalDestinatoinCountryID + "'," +

                       "'" + ioh.OriginCountryID + "'," +
                    "'" + ioh.FinalDestinationPlace + "'," +
                    "'" + ioh.PreCarriageTransportationMode + "'," +
                    "'" + ioh.PreCarrierReceiptPlace + "'," +
                      "'" + ioh.TermsOfDelivery + "'," +

                       "'" + ioh.Remarks + "'," +
                         "'" + ioh.Comments + "'," +
                    "'" + ioh.CommentStatus + "'," +
                    ioh.BankAcReference + "," +
                     ioh.FreightCharge + "," +
                    ioh.ProductValue + "," +
                    ioh.TaxAmount + "," +
                       ioh.ProductValueINR + "," +
                    ioh.TaxAmountINR + "," +
                    ioh.InvoiceAmount + "," +
                     ioh.INRAmount + "," +
                    "'" + ioh.ForwarderList + "'," +
                    ioh.status + "," +
                    ioh.DocumentStatus + "," +
                     "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "InvoiceOutHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from InvoiceOutDetail where DocumentID='" + ioh.DocumentID + "'" +
                   " and TemporaryNo=" + ioh.TemporaryNo +
                   " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InvoiceOutDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (invoiceoutdetail iod in iodList)
                {
                    updateSQL = "insert into InvoiceOutDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,PONo,PODate,StockItemID,ModelNo,CustomerItemDescription,TaxCode,Quantity,Price," +
                     " Tax,WarrantyDays,TaxDetails,POItemReferenceNo,MRNNo,MRNDate,BatchNo,ExpiryDate,SerialNO, " +
                   " PurchaseQuantity, PurchasePrice, PurchaseTax, SupplierId, StockReferenceNo) " +
                    "values ('" + ioh.DocumentID + "'," +
                    ioh.TemporaryNo + "," +
                    "'" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     iod.PONo + "," +
                    "'" + iod.PODate.ToString("yyyy-MM-dd") + "'," +
                    "'" + iod.StockItemID + "'," +
                       "'" + iod.ModelNo + "'," +
                    "'" + iod.CustomerItemDescription + "'," +
                    "'" + iod.TaxCode + "'," +
                    iod.Quantity + "," +
                    iod.Price + "," +
                    iod.Tax + "," +
                    iod.WarrantyDays + "," +
                    "'" + iod.TaxDetails + "',"
                    + iod.POItemReferenceNo + "," +
                    iod.MRNNo + "," +
                    "'" + iod.MRNDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + iod.BatchNo + "'," +
                    "'" + iod.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                   "'" + iod.SerielNo + "'," +
                     iod.PurchaseQuantity + "," +
                    iod.PurchasePrice + "," +
                    iod.PurchaseTax + "," +
                    "'" + iod.SupplierID + "'," +
                      iod.StockReferenceNo + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InvoiceOutDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                updateSQL = "Delete from InvoiceOutReceipts where InvoiceDocumentID = '" + ioh.DocumentID + "'" +
                  " and InvoiceOutTemporaryNo='" + ioh.TemporaryNo + "'" +
                  " and InvoiceOutTemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InvoiceOutReceipts", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (invoiceoutreceipts rec in receiveList)
                {
                    updateSQL = "insert into InvoiceOutReceipts " +
                    "(InvoiceDocumentID,CustomerID,InvoiceOutNo,InvoiceOutDate,InvoiceOutTemporaryNo,InvoiceOutTemporaryDate,RVDocumentID,RVTemporaryNo,RVTemporaryDate,RVNo,RVDate," +
                    "Amount) " +
                    "values ('" + ioh.DocumentID + "'," +
                     "'" + rec.CustomerID + "',0," +
                    "'1900-01-01'," +
                     +ioh.TemporaryNo + "," +
                    "'" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + rec.RVDocumentID + "',"
                        + rec.RVTemporaryNo + "," +
                    "'" + rec.RVTemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       +rec.RVNo + "," +
                    "'" + rec.RVDate.ToString("yyyy-MM-dd") + "'," +
                   rec.Amount + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InvoiceOutReceipts", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                //////return false;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                status = false;
                MessageBox.Show("Transaction Exception Occured");
            }
            return status;
        }
        public Boolean updateIOInStock(List<invoiceoutdetail> IODetails)
        {
            Boolean status = true;
            // string utString = "";
            try
            {
                foreach (invoiceoutdetail iod in IODetails)
                {
                    double quant = iod.Quantity;
                    int RefNo = iod.StockReferenceNo;
                    updateRefNoWiseIODetailInStock(quant, RefNo);
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        public void updateRefNoWiseIODetailInStock(double quantity, int stockrefno)
        {
            //Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update Stock set  " +
                    " PresentStock=" + "( (select PresentStock from Stock where RowID = " + stockrefno + ")-" + quantity + ")" +
                    ", IssueQuantity=" + "( (select IssueQuantity from Stock where RowID = " + stockrefno + ")+" + quantity + ")" +
                    " where RowID=" + stockrefno;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
                Main.QueryDelimiter;

                if (!UpdateTable.UT(utString))
                {
                    //status = false;
                    MessageBox.Show("updateRefNoWiseIODetailInStock() : failed to Update In Reference Number Wise Invoice out Detail in stock");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("updateRefNoWiseIODetailInStock() : failed to Update In Reference Number Wise Invoice Out Detail in stock");
                return;
            }
            //return status;
        }

        public static invoiceoutdetail getItemWiseTotalQuantForPerticularPOInInvoiceOUt(int poRefNo, string docIDstr)
        {
            invoiceoutdetail iod = new invoiceoutdetail();
            //List<mrnheader> MrnHeaders = new List<mrnheader>();
            try
            {
                string[] str = docIDstr.Split(';');
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select b.POItemReferenceNo,sum(b.Quantity) as TotQuant " +
                        " from  InvoiceOutDetail b,InvoiceOutHeader a " +
                        "where a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate and a.DocumentID=b.DocumentID and a.Status = 1 and a.DocumentStatus = 99" +
                        " and b.POItemReferenceNo = " + poRefNo +
                        " and b.DocumentID in ('" + str[0] + "','" + str[1] + "')" +
                        " group by b.POItemReferenceNo";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    iod.POItemReferenceNo = reader.GetInt32(0);
                    iod.Quantity = reader.GetDouble(1); // Total Quantity
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return iod;
        }

        //   Codes for ReportPOAnalysis
        public List<ReportPO> getIODetailForpartWise(int opt1, int opt2, DateTime fromDate, DateTime toDate)
        {
            // opt1 : PO Wise
            // opt2 : SeviceWise
            // opt3 : PartyWise
            // opt4 : RegionWIse
            ReportPO rpo;
            List<ReportPO> POList = new List<ReportPO>();
            try
            {
                string query = "";
                if (opt1 == 1 && opt2 == 1)
                {
                    query = "select a.DocumentID,a.Consignee,b.Name,SUM(a.ProductValueINR) from InvoiceOutHeader a, Customer b " +
                        " where a.Consignee = b.CustomerID and a.DocumentID not in ('SERVICERCINVOICEOUT') and a.InvoiceDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and a.InvoiceDate >= '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and a.DocumentStatus = 99 and a.Status = 1 " +
                        "group by a.DocumentID,a.Consignee,b.Name";
                }
                else if (opt1 == 1)
                {
                    query = "select a.DocumentID,a.Consignee,b.Name,SUM(a.ProductValueINR) from InvoiceOutHeader a, Customer b" +
                        " where a.Consignee = b.CustomerID and a.DocumentID in ( 'PRODUCTEXPORTINVOICEOUT','PRODUCTINVOICEOUT' )" +
                        " and a.InvoiceDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and a.InvoiceDate >= '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and a.DocumentStatus = 99 and a.Status = 1 " +
                        " group by a.DocumentID,a.Consignee,b.Name";
                }
                else
                {
                    query = "select a.DocumentID,a.Consignee,b.Name,SUM(a.ProductValueINR) from InvoiceOutHeader a, Customer b" +
                        " where a.Consignee = b.CustomerID and a.DocumentID in ( 'SERVICEINVOICEOUT','SERVICEEXPORTINVOICEOUT' )" +
                        " and a.InvoiceDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and a.InvoiceDate >= '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and a.DocumentStatus = 99 and a.Status = 1 " +
                        " group by a.DocumentID,a.Consignee,b.Name";
                }

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rpo = new ReportPO();
                    rpo.DocumentID = reader.GetString(0);
                    rpo.PartyID = reader.GetString(1);
                    rpo.Name = reader.GetString(2);
                    rpo.Value = reader.GetDouble(3);
                    if (rpo.DocumentID.Equals("PRODUCTEXPORTINVOICEOUT") || rpo.DocumentID.Equals("PRODUCTINVOICEOUT"))
                        rpo.DocumentType = "Product";
                    else if (rpo.DocumentID.Equals("SERVICEEXPORTINVOICEOUT") || rpo.DocumentID.Equals("SERVICEINVOICEOUT"))
                        rpo.DocumentType = "Service";
                    POList.Add(rpo);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying InvoiceOUt Header Details");
            }
            return POList;
        }
        public List<ReportPO> getIODetailForRegionWise(int opt1, int opt2, DateTime fromDate, DateTime toDate)
        {
            // opt1 : PO Wise
            // opt2 : SeviceWise
            // opt3 : PartyWise
            // opt4 : RegionWIse
            ReportPO rpo;
            List<ReportPO> POList = new List<ReportPO>();
            try
            {
                string query = "";
                if (opt1 == 1 && opt2 == 1)
                {
                    query = "select a.DocumentID,c.RegionID,d.Name, sum(a.ProductValueINR) " +
                        "from InvoiceOutHeader a,Customer b, Office c, Region d " +
                        "where a.Consignee = b.CustomerID and b.OfficeID = c.OfficeID and c.RegionID = d.RegionID and a.DocumentID not in ('SERVICERCINVOICEOUT') and" +
                         " a.InvoiceDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and a.InvoiceDate >= '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and a.DocumentStatus = 99 and a.Status = 1 " +
                        " group by a.DocumentID,c.RegionID, d.Name";
                }
                else if (opt1 == 1)
                {
                    query = "select a.DocumentID,c.RegionID,d.Name, sum(a.ProductValueINR) " +
                        "from InvoiceOutHeader a,Customer b, Office c, Region d " +
                        "where a.Consignee = b.CustomerID and b.OfficeID = c.OfficeID and c.RegionID = d.RegionID and a.DocumentID in ( 'PRODUCTEXPORTINVOICEOUT','PRODUCTINVOICEOUT' )" +
                         " and a.InvoiceDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and a.InvoiceDate >= '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and a.DocumentStatus = 99 and a.Status = 1 " +
                        " group by a.DocumentID,c.RegionID, d.Name";
                }
                else
                {
                    query = "select a.DocumentID,c.RegionID,d.Name, sum(a.ProductValueINR) " +
                        "from InvoiceOutHeader a,Customer b, Office c, Region d " +
                        "where a.Consignee = b.CustomerID and b.OfficeID = c.OfficeID and c.RegionID = d.RegionID and a.DocumentID in ( 'SERVICEINVOICEOUT','SERVICEEXPORTINVOICEOUT' )" +
                         " and a.InvoiceDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and a.InvoiceDate >= '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and a.DocumentStatus = 99 and a.Status = 1 " +
                        " group by a.DocumentID,c.RegionID, d.Name";
                }

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rpo = new ReportPO();
                    rpo.DocumentID = reader.GetString(0);
                    rpo.RegionID = reader.GetString(1);  // as region ID
                    rpo.Name = reader.GetString(2);  // as region name
                    rpo.Value = reader.GetDouble(3);
                    if (rpo.DocumentID.Equals("PRODUCTEXPORTINVOICEOUT") || rpo.DocumentID.Equals("PRODUCTINVOICEOUT"))
                        rpo.DocumentType = "Product";
                    else if (rpo.DocumentID.Equals("SERVICEEXPORTINVOICEOUT") || rpo.DocumentID.Equals("SERVICEINVOICEOUT"))
                        rpo.DocumentType = "Service";
                    POList.Add(rpo);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying InvoiceOUt Header Details");
            }
            return POList;
        }
        public List<ReportPO> getIODetailForDocumentWise(int opt1, int opt2, DateTime fromDate, DateTime toDate)
        {
            // opt1 : PO Wise
            // opt2 : SeviceWise
            // opt3 : PartyWise
            // opt4 : RegionWIse
            ReportPO rpo;
            List<ReportPO> POList = new List<ReportPO>();
            try
            {
                string query = "";
                if (opt1 == 1 && opt2 == 1)
                {
                    query = "select DocumentID,sum(ProductValueINR) from InvoiceOutHeader" +
                         " where DocumentID not in ('SERVICERCINVOICEOUT') and InvoiceDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and InvoiceDate >='" + fromDate.ToString("yyyy-MM-dd") + "'" + " and DocumentStatus = 99 and Status = 1 " +
                        " group by DocumentID ";
                }
                else if (opt1 == 1)
                {
                    query = "select DocumentID,sum(ProductValueINR) from InvoiceOutHeader" +
                        " where DocumentID in ( 'PRODUCTEXPORTINVOICEOUT','PRODUCTINVOICEOUT' )" +
                        " and InvoiceDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and InvoiceDate >= '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and DocumentStatus = 99 and Status = 1 " +
                        "group by DocumentID ";
                }
                else
                {
                    query = "select DocumentID,sum(ProductValueINR) from InvoiceOutHeader" +
                        " where DocumentID in ( 'SERVICEINVOICEOUT','SERVICEEXPORTINVOICEOUT' )" +
                        " and InvoiceDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and InvoiceDate >= '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and DocumentStatus = 99 and Status = 1 " +
                        "group by DocumentID ";
                }

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rpo = new ReportPO();
                    rpo.DocumentID = reader.GetString(0);
                    rpo.Value = reader.GetDouble(1);
                    if (rpo.DocumentID.Equals("PRODUCTEXPORTINVOICEOUT") || rpo.DocumentID.Equals("PRODUCTINVOICEOUT"))
                        rpo.DocumentType = "Product";
                    else if (rpo.DocumentID.Equals("SERVICEEXPORTINVOICEOUT") || rpo.DocumentID.Equals("SERVICEINVOICEOUT"))
                        rpo.DocumentType = "Service";
                    POList.Add(rpo);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying InvoiceOUt Header Details");
            }
            return POList;
        }
        public static double GetInvoiceOutQuantity(int trackingno, DateTime trackingdate,
            string documentID, string stockitemid, int poitemreferenceno)
        {
            double iqty = 0.0;
            try
            {
                string docString = "";
                string trackNos = "";
                string trackDates = "";
                if (documentID == "POPRODUCTINWARD")
                {
                    docString = "'PRODUCTINVOICEOUT', 'PRODUCTEXPORTINVOICEOUT'";
                }
                if (documentID == "POSERVICEINWARD")
                {
                    docString = "'SERVICEINVOICEOUT', 'SERVICEEXPORTINVOICEOUT'";
                }
                string query = "select Quantity,TrackingNos,TrackingDates from ViewInvoiceOutItemwiseQuantity " +
                    " where documentID in (" + docString + ")" +
                    " and StockItemID =  '" + stockitemid + "'" +
                    " and Status = 1 and DocumentStatus = 99" +
                    " and POItemReferenceNo =  " + poitemreferenceno +
                    " and TrackingNos like '%" + trackingno + ";%'" +
                    " and TrackingDates like '%" + trackingdate.ToString("yyyy-MM-dd") + ";%'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    trackNos = reader.GetString(1);
                    trackDates = reader.GetString(2);

                    string[] trackNosStr = trackNos.Split(';');
                    string[] trackDatesStr = trackDates.Split(';');

                    int indexofTrackNo = Array.IndexOf(trackNosStr, trackingno.ToString()); //get index of required track no in string array
                                                                                            //If required Track No is not found in trackNosStr array then index will be -1
                    if (indexofTrackNo != -1)
                    {
                        DateTime TrackDateFound = Convert.ToDateTime(trackDatesStr[indexofTrackNo]);  // get track date from trackDatesStr with track no index
                        DateTime TrackDateRequired = Convert.ToDateTime(trackingdate);

                        if (TrackDateFound == TrackDateRequired)
                        {
                            iqty = iqty + reader.GetDouble(0);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return iqty;
        }
        public static string getCustIDOfINvoiceOUT(invoiceoutheader ioh)
        {
            string custID = "";
            try
            {
                string query = "select Consignee" +
                     " from InvoiceOutHeader" +
                   " where DocumentID='" + ioh.DocumentID + "'" +
                  " and TemporaryNo=" + ioh.TemporaryNo +
                  " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    custID = reader.IsDBNull(0) ? "" : reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice Header CustoemrID");
            }
            return custID;
        }
        public static string getInvoiceNos(string docid, int trno, DateTime dt)
        {
            string invNos = "";
            try
            {
                string query = "select invoiceno, invoicedate from ViewInvoiceOutVsPOInSummary" +
                     " where trackingdocumentid='" + docid + "'" +
                     " and TrackingNo=" + trno +
                     " and TrackingDate='" + dt.ToString("yyyy-MM-dd") + "'";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    invNos = invNos + reader.GetInt32(0) + ":" + reader.GetDateTime(1).ToString("dd-MM-yyyy") + ",";
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice Header CustoemrID");
            }
            return invNos;
        }
        public List<invoiceoutheader> getInvoiceOutDetailList(string CustCode)
        {
            invoiceoutheader ioh;
            List<invoiceoutheader> InvoiceOutHeaderList = new List<invoiceoutheader>();
            try
            {
                string query = "select a.DocumentID,a.InvoiceNo,a.InvoiceDate," +
                    " a.Consignee, c.Name, a.InvoiceAmount,a.Remarks,isnull(b.AmountReceived,0),isnull(b.TDSReceived,0),a.TemporaryNo,a.TemporaryDate " +
                    " from InvoiceOutHeader a left outer join ViewInvoiceOutReceiptSummary as b  " +
                     " on a.InvoiceNo = b.InvoiceOutNo and a.InvoiceDate=b.InvoiceOutDate and a.DocumentID = b.InvoiceDocumentID" +
                    " left outer join Currency c on a.CurrencyID = c.CurrencyID where  a.DocumentStatus = 99  and a.Status = 1 and a.Consignee = '" + CustCode + "'" +
                    // " and a.InvoiceValueINR > isnull(b.amountpaid,0) order by DocumentDate desc";
                    " order by InvoiceDate desc";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        ioh = new invoiceoutheader();
                        ioh.DocumentID = reader.GetString(0);
                        ioh.InvoiceNo = reader.GetInt32(1);
                        ioh.InvoiceDate = reader.GetDateTime(2);
                        ioh.ConsigneeID = reader.GetString(3);
                        ioh.CurrencyID = reader.GetString(4); //Name of currency
                        ioh.InvoiceAmount = reader.GetDouble(5);
                        ioh.Remarks = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        ioh.AmountReceived = reader.GetDecimal(7);
                        ioh.TDSReceived = reader.GetDecimal(8);
                        ioh.TemporaryNo = reader.GetInt32(9);
                        ioh.TemporaryDate = reader.GetDateTime(10);
                        InvoiceOutHeaderList.Add(ioh);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice Header Details");
            }
            return InvoiceOutHeaderList;
        }
        public static DataGridView getGridViewForInvoiceOut(string CustCode)
        {

            DataGridView grdInv = new DataGridView();
            try
            {
                string[] strColArr = { "DocID", "InvNO","InvDate","ConsigneeID","Currency",
                    "InvoiceValue","AmountReceived","AmountToReceive","TDSReceived","TDSToReceive","Remarks","TempNo","TempDate"};
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                    ,new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn()
                };

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdInv.EnableHeadersVisualStyles = false;
                grdInv.AllowUserToAddRows = false;
                grdInv.AllowUserToDeleteRows = false;
                grdInv.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdInv.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdInv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdInv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdInv.ColumnHeadersHeight = 27;
                grdInv.RowHeadersVisible = false;
                grdInv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;
                grdInv.Columns.Add(colChk);
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str.Replace(" ", string.Empty);
                    colArr[index].HeaderText = str;
                    if (index == 7 || index == 9)
                        colArr[index].ReadOnly = false;
                    else
                        colArr[index].ReadOnly = true;
                    if (index == 2)
                        colArr[index].DefaultCellStyle.Format = "dd-MM-yyyy";
                    if (index == 0)
                        colArr[index].Width = 120;
                    else if (index == 10)
                        colArr[index].Width = 170;
                    else if (index == 1 || index == 2)
                        colArr[index].Width = 65;
                    else if (index == 4)
                        colArr[index].Width = 80;
                    else
                        colArr[index].Width = 110;
                    if (index == 11 || index == 12)
                        colArr[index].Visible = false;
                    else
                        colArr[index].Visible = true;
                    grdInv.Columns.Add(colArr[index]);
                }
                InvoiceOutHeaderDB iohDb = new InvoiceOutHeaderDB();
                List<invoiceoutheader> IOHList = iohDb.getInvoiceOutDetailList(CustCode); //All INvoice List
                foreach (invoiceoutheader ioh in IOHList)
                {
                    grdInv.Rows.Add();
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[0]].Value = ioh.DocumentID;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[1]].Value = ioh.InvoiceNo;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[2]].Value = ioh.InvoiceDate;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[3]].Value = ioh.ConsigneeID;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[4]].Value = ioh.CurrencyID;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[5]].Value = ioh.InvoiceAmount;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[6]].Value = ioh.AmountReceived;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[7]].Value = Convert.ToDecimal(0);
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[8]].Value = ioh.TDSReceived;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[9]].Value = Convert.ToDecimal(0);
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[10]].Value = ioh.Remarks;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[11]].Value = ioh.TemporaryNo;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[12]].Value = ioh.TemporaryDate;
                }
            }
            catch (Exception ex)
            {
            }

            return grdInv;
        }
        public static List<invoiceoutreceipts> getInvoiceOutReceiveDetails(string RVDocID, int RVTempNo, DateTime RVTempDate)
        {
            List<invoiceoutreceipts> receiveLIst = new List<invoiceoutreceipts>();
            try
            {
                invoiceoutreceipts receive = new invoiceoutreceipts();
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,InvoiceDocumentID,InvoiceOutNo,InvoiceOutDate,InvoiceOutTemporaryNo,InvoiceOutTemporaryDate, RVTemporaryNo," +
                    "RVTemporaryDate,RVNo,RVDate,Amount,CustomerID,RVDocumentID,TDSAmount from InvoiceOutReceipts  where RVTemporaryNo = " + RVTempNo +
                    " and RVTemporaryDate = '" + RVTempDate.ToString("yyyy-MM-dd") + "' and RVDocumentID = '" + RVDocID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    receive = new invoiceoutreceipts();
                    receive.RowID = reader.GetInt32(0);
                    receive.InvoiceDocumentID = reader.GetString(1);
                    receive.InvoiceOutNo = reader.GetInt32(2);
                    receive.InvoiceOutDate = reader.GetDateTime(3);
                    receive.InvoiceOutTemporaryNo = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                    receive.InvoiceOutTemporaryDate = reader.IsDBNull(5) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(5);
                    receive.RVTemporaryNo = reader.GetInt32(6);
                    receive.RVTemporaryDate = reader.GetDateTime(7);
                    receive.RVNo = reader.IsDBNull(8) ? 0 : reader.GetInt32(8);
                    receive.RVDate = reader.IsDBNull(9) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(9);
                    receive.Amount = reader.GetDecimal(10);
                    receive.CustomerID = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    receive.RVDocumentID = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    receive.TDSAmount = reader.IsDBNull(13) ? 0 : reader.GetDecimal(13);
                    receiveLIst.Add(receive);
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice out receipt details");
            }
            return receiveLIst;
        }

        //queryforproductservice:14-03-18

        public List<invoiceoutheader> getinvoiceforproduct()
        {
            invoiceoutheader ioh;
            List<invoiceoutheader> IOHeaders = new List<invoiceoutheader>();
            try
            {
                string query = "select a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate, a.InvoiceNo,a.InvoiceDate,a.TrackingNos,"+
                               " TrackingDates,a.TrackingNo,a.TrackingDate,b.OfficeID,c.Name OfficeName,Consignee, ConsigneeName, TermsOfPayment, Description,"+
                               " TransportationMode, TransportationModeName,TransportationType, TransportationTypeName, Transporter, TransporterName, a.CurrencyID,"+
                               " INRConversionRate,ADCode,EntryPort,  ExitPort,FinalDestinationCountryID,FinalDestinationCountryName,OriginCountryID,"+
                               " OriginCountryName,FinalDestinationPlace,PreCarriageTransportationMode,PreCarriageTransportationName, "+
                               " PreCarrierReceiptPlace,TermsOfDelivery,a.Remarks,a.CommentStatus,a.CreateUser,a.ForwardUser,a.ApproveUser, "+
                               " CreatorName,a.CreateTime,ForwarderName,ApproverName,a.ForwarderList,a.Status,a.DocumentStatus,a.ProductValue,"+
                               " a.TaxAmount,InvoiceAmount,INRAmount,a.ProjectID  , a.ProductValueINR, a.TaxAmountINR,BankAcReference,a.FreightCharge,"+
                               " a.DeliveryAddress,DeliveryStateCode,SJVTNo,SJVTDate,SJVNo,SJVDate from ViewInvoiceOutHeader as a"+
                               " left join POProductInwardHeader as b on a.TrackingNo = b.TrackingNo and a.TrackingDate = b.TrackingDate"+
                               " left join office as c on b.OfficeID = c.OfficeID"+
                               " where a.DocumentStatus = 99  and a.Status = 1 and a.DocumentID in ('PRODUCTINVOICEOUT','PRODUCTEXPORTINVOICEOUT')"+  
                               " and InvoiceDate > '" + UpdateTable.getSQLDateTime().AddDays(-10).ToString("yyyy-MM-dd") + "' order by InvoiceDate desc, DocumentID asc,InvoiceNo desc";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ioh = new invoiceoutheader();
                    ioh.RowID = reader.GetInt32(0);
                    ioh.DocumentID = reader.GetString(1);
                    ioh.DocumentName = reader.GetString(2);
                    ioh.TemporaryNo = reader.GetInt32(3);
                    ioh.TemporaryDate = reader.GetDateTime(4);
                    ioh.InvoiceNo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        ioh.InvoiceDate = reader.GetDateTime(6);
                    }


                    //ioh.TrackingNo = reader.GetInt32(7);
                    //ioh.TrackingDate = reader.GetDateTime(8);
                    ioh.TrackingNos = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    ioh.TrackingDates = reader.IsDBNull(8) ? "" : reader.GetString(8);


                    ioh.ConsigneeID = reader.GetString(9);
                    ioh.ConsigneeName = reader.GetString(10);
                    ioh.TermsOfPayment = reader.GetString(11);
                    ioh.Description = reader.IsDBNull(12) ? "" : reader.GetString(12);

                    ioh.TransportationMode = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    ioh.TransportationModeName = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    ioh.TransportationType = reader.IsDBNull(15) ? "" : reader.GetString(15);
                    ioh.TransportationTypeName = reader.IsDBNull(16) ? "" : reader.GetString(16);
                    ioh.Transporter = reader.IsDBNull(17) ? "" : reader.GetString(17);
                    ioh.TransporterName = reader.IsDBNull(18) ? "" : reader.GetString(18);

                    //popih.ValidityDate = reader.GetDateTime(12);
                    ioh.CurrencyID = reader.GetString(19);
                    // ioh.TaxCode = reader.GetString(20);
                    ioh.INRConversionRate = reader.GetDouble(20);
                    ioh.ADCode = reader.GetString(21);
                    ioh.EntryPort = reader.GetString(22);
                    ioh.ExitPort = reader.GetString(23);

                    ioh.FinalDestinatoinCountryID = reader.GetString(24);
                    if (!reader.IsDBNull(25))
                    {
                        ioh.FinalDestinatoinCountryName = reader.GetString(25);
                    }
                    else
                    {
                        ioh.FinalDestinatoinCountryName = "";
                    }

                    ioh.OriginCountryID = reader.GetString(26);
                    if (!reader.IsDBNull(27))
                    {
                        ioh.OriginCountryName = reader.GetString(27);
                    }
                    else
                    {
                        ioh.OriginCountryName = "";
                    }

                    ioh.FinalDestinationPlace = reader.GetString(28);
                    ioh.PreCarriageTransportationMode = reader.GetString(29);
                    if (!reader.IsDBNull(30))
                    {
                        ioh.PreCarriageTransportationName = reader.GetString(30);
                    }
                    else
                    {
                        ioh.PreCarriageTransportationName = "";
                    }

                    ioh.PreCarrierReceiptPlace = reader.GetString(31);
                    ioh.TermsOfDelivery = reader.GetString(32);
                    ioh.Remarks = reader.GetString(33);
                    if (!reader.IsDBNull(34))
                    {
                        ioh.CommentStatus = reader.GetString(34);
                    }
                    else
                    {
                        ioh.CommentStatus = "";
                    }
                    ioh.CreateUser = reader.GetString(35);
                    ioh.ForwardUser = reader.GetString(36);
                    ioh.ApproveUser = reader.GetString(37);
                    ioh.CreatorName = reader.GetString(38);
                    ioh.CreateTime = reader.GetDateTime(39);
                    ioh.ForwarderName = reader.GetString(40);
                    ioh.ApproverName = reader.GetString(41);
                    if (!reader.IsDBNull(42))
                    {
                        ioh.ForwarderList = reader.GetString(42);
                    }
                    else
                    {
                        ioh.ForwarderList = "";
                    }

                    // ioh.Remarks = reader.GetString(44);
                    ioh.status = reader.GetInt32(43);
                    ioh.DocumentStatus = reader.GetInt32(44);
                    ioh.ProductValue = reader.GetDouble(45);
                    ioh.TaxAmount = reader.GetDouble(46);
                    ioh.InvoiceAmount = reader.GetDouble(47);
                    ioh.INRAmount = reader.GetDouble(48);
                    ioh.ProjectID = reader.IsDBNull(49) ? "" : reader.GetString(49);
                    ioh.ProductValueINR = reader.GetDouble(50);
                    ioh.TaxAmountINR = reader.GetDouble(51);
                    ioh.BankAcReference = reader.IsDBNull(52) ? 0 : reader.GetInt32(52);
                    ioh.FreightCharge = reader.IsDBNull(53) ? 0 : reader.GetDouble(53);
                    ioh.DeliveryAddress = reader.IsDBNull(54) ? "" : reader.GetString(54);
                    ioh.DeliveryStateCode = reader.IsDBNull(55) ? "" : reader.GetString(55);

                    ioh.SJVTNo = reader.IsDBNull(56) ? 0 : reader.GetInt32(56);
                    ioh.SJVTDate = reader.IsDBNull(57) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(57);
                    ioh.SJVNo = reader.IsDBNull(58) ? 0 : reader.GetInt32(58);
                    ioh.SJVDate = reader.IsDBNull(59) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(59);
                    IOHeaders.Add(ioh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return IOHeaders;
        }
        public static List<ReceiptVoucherHeader> getRVlistOFCustomerForInvoiceOut(string custid)
        {
            List<ReceiptVoucherHeader> rvLIst = new List<ReceiptVoucherHeader>();
            try
            {
                ReceiptVoucherHeader rec = new ReceiptVoucherHeader();
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.DocumentID,a.VoucherNo, a.VoucherDate,a.TemporaryNo,a.TemporaryDate, a.VoucherAmount, isnull(b.TotAdjusted,0),c.Name from ReceiptVoucherHeader a left outer join " +
                    "(select RVDocumentID,RVTemporaryNo, RVTemporaryDate, SUM(Amount) TotAdjusted from InvoiceOutReceipts group by RVDocumentID,RVTemporaryNo, RVTemporaryDate) b" +
                    " on a.TemporaryNo = b.RVTemporaryNo and a.TemporaryDate = b.RVTemporaryDate and a.DocumentID = b.RVDocumentID " +
                    " left outer join Currency c on a.CurrencyID = c.CurrencyID where a.DocumentStatus = 99 and a.Status = 1 and a.SLCode = '" + custid + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rec = new ReceiptVoucherHeader();
                    rec.DocumentID = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    rec.VoucherNo = reader.GetInt32(1);
                    rec.VoucherDate = reader.GetDateTime(2);
                    rec.TemporaryNo = reader.GetInt32(3);
                    rec.TemporaryDate = reader.GetDateTime(4);
                    rec.VoucherAmount = reader.GetDecimal(5);
                    rec.TotalAdjusted = reader.GetDecimal(6);
                    rec.CurrencyID = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    rvLIst.Add(rec);
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Receipt voucher details");
            }
            return rvLIst;
        }
        public static DataGridView getGridViewOFRVForAdvAdjustment(string CustCode)
        {

            DataGridView grdInv = new DataGridView();
            try
            {
                string[] strColArr = { "VoucherID", "VoucherNO", "VoucherDate", "VoucherTempNo", "VoucherTempDate", "Currency", "VoucherAmt", "AmountAdjusted", "AmountToAdjust" };
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                };

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdInv.EnableHeadersVisualStyles = false;
                grdInv.AllowUserToAddRows = false;
                grdInv.AllowUserToDeleteRows = false;
                grdInv.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdInv.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdInv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdInv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdInv.ColumnHeadersHeight = 27;
                grdInv.RowHeadersVisible = false;
                //grdInv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;
                grdInv.Columns.Add(colChk);
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str.Replace(" ", string.Empty);
                    colArr[index].HeaderText = str;
                    if (index == 8)
                        colArr[index].ReadOnly = false;
                    else
                        colArr[index].ReadOnly = true;
                    if (index == 2 || index == 4)
                        colArr[index].DefaultCellStyle.Format = "dd-MM-yyyy";
                    if (index == 1)
                        colArr[index].Width = 80;
                    else
                        colArr[index].Width = 110;
                    if (index == 3 || index == 4)
                        colArr[index].Visible = false;
                    else
                        colArr[index].Visible = true;
                    grdInv.Columns.Add(colArr[index]);
                }
                List<ReceiptVoucherHeader> RVList = getRVlistOFCustomerForInvoiceOut(CustCode); //All voucher List

                foreach (ReceiptVoucherHeader rvh in RVList)
                {
                    grdInv.Rows.Add();
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[0]].Value = rvh.DocumentID;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[1]].Value = rvh.VoucherNo;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[2]].Value = rvh.VoucherDate;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[3]].Value = rvh.TemporaryNo;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[4]].Value = rvh.TemporaryDate;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[5]].Value = rvh.CurrencyID;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[6]].Value = rvh.VoucherAmount;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[7]].Value = rvh.TotalAdjusted;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[8]].Value = Convert.ToDecimal(0);
                }
            }
            catch (Exception ex)
            {
            }

            return grdInv;
        }
        public static List<invoiceoutreceipts> getInvoiceOutAdvPaymentDetails(int InvTempNo, DateTime InvTempDate, string INvDocID)
        {
            List<invoiceoutreceipts> recLIst = new List<invoiceoutreceipts>();
            try
            {
                invoiceoutreceipts rec = new invoiceoutreceipts();
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,InvoiceDocumentID,InvoiceOutNo,InvoiceOutDate,InvoiceOutTemporaryNo,InvoiceOutTemporaryDate, RVTemporaryNo," +
                    "RVTemporaryDate,RVNo,RVDate,Amount,CustomerID,RVDocumentID from InvoiceOutReceipts   where InvoiceDocumentID='" + INvDocID + "'" +
                    " and InvoiceOutTemporaryNo=" + InvTempNo +
                    " and InvoiceOutTemporaryDate='" + InvTempDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rec = new invoiceoutreceipts();
                    rec.RowID = reader.GetInt32(0);
                    rec.InvoiceDocumentID = reader.GetString(1);
                    rec.InvoiceOutNo = reader.GetInt32(2);
                    rec.InvoiceOutDate = reader.GetDateTime(3);
                    rec.InvoiceOutTemporaryNo = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                    rec.InvoiceOutTemporaryDate = reader.IsDBNull(5) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(5);
                    rec.RVTemporaryNo = reader.GetInt32(6);
                    rec.RVTemporaryDate = reader.GetDateTime(7);
                    rec.RVNo = reader.GetInt32(8);
                    rec.RVDate = reader.GetDateTime(9);
                    rec.Amount = reader.GetDecimal(10);
                    rec.CustomerID = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    rec.RVDocumentID = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    recLIst.Add(rec);
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice out receipt details");
            }
            return recLIst;
        }
        public List<invoiceoutheader> getFilteredInvoiceOutHeaderList(string DocID, int tempno, DateTime tempdate)
        {
            invoiceoutheader ioh;
            List<invoiceoutheader> IOHeaders = new List<invoiceoutheader>();
            try
            {
                //approved user comment status string
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate, InvoiceNo,InvoiceDate,TrackingNos,TrackingDates,Consignee," +
                               " ConsigneeName,TermsOfPayment,Description,TransportationMode,TransportationModeName,TransportationType, TransportationTypeName," +
                               " Transporter,TransporterName,CurrencyID,INRConversionRate,ADCode,EntryPort,  ExitPort,FinalDestinationCountryID," +
                               " FinalDestinationCountryName,OriginCountryID,OriginCountryName,FinalDestinationPlace,PreCarriageTransportationMode," +
                               " PreCarriageTransportationName,PreCarrierReceiptPlace,TermsOfDelivery,Remarks,CommentStatus,CreateUser,ForwardUser," +
                               " ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,Status,DocumentStatus,ProductValue,TaxAmount," +
                               " InvoiceAmount,INRAmount,ProjectID  , ProductValueINR, TaxAmountINR,BankAcReference,FreightCharge,DeliveryAddress," +
                               " DeliveryStateCode,SJVTNo,SJVTDate,SJVNo,SJVDate from ViewInvoiceOutHeader" +
                               " where DocumentID = '" + DocID + "' and TemporaryNo = " + tempno + " and TemporaryDate = '" + tempdate.ToString("yyyy-MM-dd") + "' and DocumentStatus = 99" +
                               " and Status = 1order by InvoiceDate desc, DocumentID asc,InvoiceNo desc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ioh = new invoiceoutheader();
                    ioh.RowID = reader.GetInt32(0);
                    ioh.DocumentID = reader.GetString(1);
                    ioh.DocumentName = reader.GetString(2);
                    ioh.TemporaryNo = reader.GetInt32(3);
                    ioh.TemporaryDate = reader.GetDateTime(4);
                    ioh.InvoiceNo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        ioh.InvoiceDate = reader.GetDateTime(6);
                    }


                    //ioh.TrackingNo = reader.GetInt32(7);
                    //ioh.TrackingDate = reader.GetDateTime(8);
                    ioh.TrackingNos = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    ioh.TrackingDates = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    ioh.ConsigneeID = reader.GetString(9);
                    ioh.ConsigneeName = reader.GetString(10);
                    ioh.TermsOfPayment = reader.GetString(11);
                    ioh.Description = reader.GetString(12);

                    ioh.TransportationMode = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    ioh.TransportationModeName = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    ioh.TransportationType = reader.IsDBNull(15) ? "" : reader.GetString(15);
                    ioh.TransportationTypeName = reader.IsDBNull(16) ? "" : reader.GetString(16);
                    ioh.Transporter = reader.IsDBNull(17) ? "" : reader.GetString(17);
                    ioh.TransporterName = reader.IsDBNull(18) ? "" : reader.GetString(18);

                    //popih.ValidityDate = reader.GetDateTime(12);
                    ioh.CurrencyID = reader.GetString(19);
                    // ioh.TaxCode = reader.GetString(20);
                    ioh.INRConversionRate = reader.GetDouble(20);
                    ioh.ADCode = reader.GetString(21);
                    ioh.EntryPort = reader.GetString(22);
                    ioh.ExitPort = reader.GetString(23);

                    ioh.FinalDestinatoinCountryID = reader.GetString(24);
                    if (!reader.IsDBNull(25))
                    {
                        ioh.FinalDestinatoinCountryName = reader.GetString(25);
                    }
                    else
                    {
                        ioh.FinalDestinatoinCountryName = "";
                    }

                    ioh.OriginCountryID = reader.GetString(26);
                    if (!reader.IsDBNull(27))
                    {
                        ioh.OriginCountryName = reader.GetString(27);
                    }
                    else
                    {
                        ioh.OriginCountryName = "";
                    }

                    ioh.FinalDestinationPlace = reader.GetString(28);
                    ioh.PreCarriageTransportationMode = reader.GetString(29);
                    if (!reader.IsDBNull(30))
                    {
                        ioh.PreCarriageTransportationName = reader.GetString(30);
                    }
                    else
                    {
                        ioh.PreCarriageTransportationName = "";
                    }

                    ioh.PreCarrierReceiptPlace = reader.GetString(31);
                    ioh.TermsOfDelivery = reader.GetString(32);
                    ioh.Remarks = reader.GetString(33);
                    if (!reader.IsDBNull(34))
                    {
                        ioh.CommentStatus = reader.GetString(34);
                    }
                    else
                    {
                        ioh.CommentStatus = "";
                    }
                    ioh.CreateUser = reader.GetString(35);
                    ioh.ForwardUser = reader.GetString(36);
                    ioh.ApproveUser = reader.GetString(37);
                    ioh.CreatorName = reader.GetString(38);
                    ioh.CreateTime = reader.GetDateTime(39);
                    ioh.ForwarderName = reader.GetString(40);
                    ioh.ApproverName = reader.GetString(41);
                    if (!reader.IsDBNull(42))
                    {
                        ioh.ForwarderList = reader.GetString(42);
                    }
                    else
                    {
                        ioh.ForwarderList = "";
                    }

                    // ioh.Remarks = reader.GetString(44);
                    ioh.status = reader.GetInt32(43);
                    ioh.DocumentStatus = reader.GetInt32(44);
                    ioh.ProductValue = reader.GetDouble(45);
                    ioh.TaxAmount = reader.GetDouble(46);
                    ioh.InvoiceAmount = reader.GetDouble(47);
                    ioh.INRAmount = reader.GetDouble(48);
                    ioh.ProjectID = reader.IsDBNull(49) ? "" : reader.GetString(49);
                    ioh.ProductValueINR = reader.GetDouble(50);
                    ioh.TaxAmountINR = reader.GetDouble(51);
                    ioh.BankAcReference = reader.IsDBNull(52) ? 0 : reader.GetInt32(52);
                    ioh.FreightCharge = reader.IsDBNull(53) ? 0 : reader.GetDouble(53);
                    ioh.DeliveryAddress = reader.IsDBNull(54) ? "" : reader.GetString(54);
                    ioh.DeliveryStateCode = reader.IsDBNull(55) ? "" : reader.GetString(55);
                    ioh.SJVTNo = reader.IsDBNull(56) ? 0 : reader.GetInt32(56);
                    ioh.SJVTDate = reader.IsDBNull(57) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(57);
                    ioh.SJVNo = reader.IsDBNull(58) ? 0 : reader.GetInt32(58);
                    ioh.SJVDate = reader.IsDBNull(59) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(59);
                    IOHeaders.Add(ioh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice Out header");
            }
            return IOHeaders;
        }

        public static Boolean isInvoiceOutReceiptPreparedForInvOut(invoiceoutheader ioh)
        {
            Boolean isAvail = false;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select count(*) from InvoiceOutReceipts where InvoiceDocumentID = '" + ioh.DocumentID + "'" +
                        " and InvoiceOutTemporaryNo=" + ioh.TemporaryNo +
                        " and InvoiceOutTemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader.GetInt32(0) > 0)
                    {
                        isAvail = true;
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return isAvail;
        }
        public static ListView getINvoiceOutDetailLVForDebitNote(string CustCode)
        {
            ListView lv = new ListView();
            try
            {
                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                lv.Columns.Add("Sel", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Doc ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Inv NO", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Inv Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Product Value", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Tax Amount", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Invoice Value", -2, HorizontalAlignment.Left);

                InvoiceOutHeaderDB iohDb = new InvoiceOutHeaderDB();

                List<invoiceoutheader> IHList = iohDb.getInvoiceOutListForDebitNote(CustCode);
                foreach (invoiceoutheader ioh in IHList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = false;
                    item.SubItems.Add(ioh.DocumentID.ToString());
                    item.SubItems.Add(ioh.InvoiceNo.ToString());
                    item.SubItems.Add(ioh.InvoiceDate.ToString("yyyy-MM-dd"));

                    item.SubItems.Add(ioh.TrackingNos);   //Tracking No
                    item.SubItems.Add(ioh.TrackingDates);  //Traccking Date
                    item.SubItems.Add(ioh.ProductValueINR.ToString());
                    item.SubItems.Add(ioh.TaxAmountINR.ToString());
                    item.SubItems.Add(ioh.INRAmount.ToString());
                    lv.Items.Add(item);
                }
            }
            catch (Exception ex)
            {

            }
            return lv;
        }

        public List<invoiceoutheader> getInvoiceOutListForDebitNote(string CustCode)
        {
            invoiceoutheader ioh;
            List<invoiceoutheader> InvoiceOutHeaderList = new List<invoiceoutheader>();
            try
            {
                string query = "select DocumentID,InvoiceNo,InvoiceDate,TrackingNos,TrackingDates,ProductValueINR,TaxAmountINR,INRAmount from InvoiceOutHeader" +
                    " where  DocumentStatus = 99  and Status = 1 and Consignee = '" + CustCode + "'" +
                    " order by InvoiceDate asc";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        ioh = new invoiceoutheader();
                        ioh.DocumentID = reader.GetString(0);
                        ioh.InvoiceNo = reader.GetInt32(1);
                        ioh.InvoiceDate = reader.GetDateTime(2);
                        ioh.TrackingNos = reader.GetString(3);
                        ioh.TrackingDates = reader.GetString(4);

                        ioh.ProductValueINR = reader.GetDouble(5);
                        ioh.TaxAmountINR = reader.GetDouble(6);
                        ioh.INRAmount = reader.GetDouble(7);

                        InvoiceOutHeaderList.Add(ioh);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice Header Details");
            }
            return InvoiceOutHeaderList;
        }
        public System.Data.DataTable taxDetails4PrintOld(List<invoiceoutdetail> IODetails, string documentID)
        {
            int HSNLength = 0;
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataTable dtTax = new System.Data.DataTable();
            //----------
            System.Data.DataTable dtRowID = new System.Data.DataTable();
            dtRowID.Columns.Add("RowID", typeof(Int32));
            //----------
            try
            {
                if (documentID == "PRODUCTINVOICEOUT")
                {
                    HSNLength = 4;
                }
                else if (documentID == "SERVICEINVOICEOUT")
                {
                    HSNLength = 6;
                }

                {
                    dt.Columns.Add("HSNCode", typeof(string));
                    dt.Columns.Add("TaxCode", typeof(string));
                    dt.Columns.Add("Amount", typeof(double));
                    dt.Columns.Add("TaxAmount", typeof(double));
                    dt.Columns.Add("TaxItem", typeof(string));
                    dt.Columns.Add("TaxItemPercentage", typeof(double));
                    dt.Columns.Add("TaxItemAmount", typeof(double));
                    dt.Columns.Add("RowID", typeof(Int32));
                }
                //fill hsn code wise tax details in dt
                foreach (invoiceoutdetail iod in IODetails)
                {
                    string tstr1 = iod.TaxDetails;
                    string[] lst1 = tstr1.Split('\n');
                    for (int j = 0; j < lst1.Length - 1; j++)
                    {
                        string[] lst2 = lst1[j].Split('-');
                        if (Convert.ToDouble(lst2[1]) > 0)
                        {
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1][0] = iod.HSNCode.Substring(0, HSNLength);
                            dt.Rows[dt.Rows.Count - 1][1] = iod.TaxCode;
                            dt.Rows[dt.Rows.Count - 1][2] = iod.Quantity * iod.Price;
                            dt.Rows[dt.Rows.Count - 1][3] = iod.Tax;
                            dt.Rows[dt.Rows.Count - 1][4] = lst2[0];
                            dt.Rows[dt.Rows.Count - 1][5] = iod.HSNCode; //need to replace with percentage
                            dt.Rows[dt.Rows.Count - 1][6] = lst2[1];
                            dt.Rows[dt.Rows.Count - 1][7] = iod.RowID;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
            string tstr = "dt=\n";
            ////for (int i = 0; i < (dt.Rows.Count); i++)
            ////{
            ////    tstr = tstr +
            ////        dt.Rows[i][0].ToString() + "," +
            ////        dt.Rows[i][1].ToString() + "," +
            ////        dt.Rows[i][2].ToString() + "," +
            ////        dt.Rows[i][3].ToString() + "," +
            ////        dt.Rows[i][4].ToString() + "," +
            ////        dt.Rows[i][5].ToString() + "," +
            ////        dt.Rows[i][6].ToString() + "," +
            ////        dt.Rows[i][7].ToString() + "\n";

            ////}
            ////MessageBox.Show(tstr);
            try
            {
                //fill tax rate for each tax item in dt
                TaxCodeWorkingDB tcwdb = new TaxCodeWorkingDB();
                List<taxcodeworking> tcwDetails = tcwdb.getTaxCodeDetails();
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    foreach (taxcodeworking tcwd in tcwDetails)
                    {
                        if (dt.Rows[i][1].ToString() == tcwd.TaxCode && dt.Rows[i][4].ToString() == tcwd.TaxItemName)
                        {
                            dt.Rows[i][5] = tcwd.OperatorValue;
                            break;
                        }
                    }

                }

                tstr = "dt=\n";
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    tstr = tstr +
                        dt.Rows[i][0].ToString() + "," +
                        dt.Rows[i][1].ToString() + "," +
                        dt.Rows[i][2].ToString() + "," +
                        dt.Rows[i][3].ToString() + "," +
                        dt.Rows[i][4].ToString() + "," +
                        dt.Rows[i][5].ToString() + "," +
                        dt.Rows[i][6].ToString() + "," +
                        dt.Rows[i][7].ToString() + "\n";

                }
                MessageBox.Show(tstr);

                //prepare HSN Code wise totals in a new table
                System.Data.DataTable dttotal = new System.Data.DataTable();
                dttotal = dt.Copy();
                dttotal.Clear();
                ////int frowid = 0; //intial value
                for (int i = 0; i < (dt.Rows.Count); i++)
                {

                    Boolean fount = false;
                    string tstr1 = dt.Rows[i][0].ToString(); //hsn code
                    string tstr2 = dt.Rows[i][4].ToString(); //taxitem code
                    string tstr3 = dt.Rows[i][5].ToString(); //tax %
                    for (int j = 0; j < (dttotal.Rows.Count); j++)
                    {
                        string tstr4 = dttotal.Rows[j][0].ToString();
                        string tstr5 = dttotal.Rows[j][4].ToString();
                        string tstr6 = dttotal.Rows[j][5].ToString();

                        if (tstr1 == tstr4 && tstr2 == tstr5 && tstr3 == tstr6)
                        {
                            ////int crowid = Convert.ToInt32(String.IsNullOrEmpty(dt.Rows[i][7].ToString()) ? "0" : dt.Rows[i][7].ToString());
                            ////if (frowid != crowid)
                            ////{
                            dttotal.Rows[j][2] = Convert.ToDouble(dttotal.Rows[j][2].ToString()) +
                            Convert.ToDouble(dt.Rows[i][2].ToString()); //item amount withot tax
                                                                        ////frowid = crowid;
                                                                        ////}
                            dttotal.Rows[j][6] = Convert.ToDouble(dttotal.Rows[j][6].ToString()) +
                                Convert.ToDouble(dt.Rows[i][6].ToString()); //tax total
                            fount = true;
                        }
                    }
                    if (!fount)
                    {
                        dttotal.ImportRow(dt.Rows[i]);
                    }
                }
                tstr = "dttotal=\n";
                for (int i = 0; i < (dttotal.Rows.Count); i++)
                {
                    tstr = tstr +
                        dttotal.Rows[i][0].ToString() + "," +
                        dttotal.Rows[i][1].ToString() + "," +
                        dttotal.Rows[i][2].ToString() + "," +
                        dttotal.Rows[i][3].ToString() + "," +
                        dttotal.Rows[i][4].ToString() + "," +
                        dttotal.Rows[i][5].ToString() + "," +
                        dttotal.Rows[i][6].ToString() + "," +
                        dttotal.Rows[i][7].ToString() + "\n";

                }
                MessageBox.Show(tstr);
                //create print table
                tstr = "";
                //find distinct tax item in dttotal
                DataTable dtDistinct = dttotal.AsEnumerable().GroupBy(row => row.Field<string>("TaxItem")).Select(group => group.First()).CopyToDataTable();
                for (int i = 0; i < (dtDistinct.Rows.Count); i++)
                {
                    tstr = tstr +
                        dtDistinct.Rows[i][0].ToString() + "," +
                        dtDistinct.Rows[i][1].ToString() + "," +
                        dtDistinct.Rows[i][2].ToString() + "," +
                        dtDistinct.Rows[i][3].ToString() + "," +
                        dtDistinct.Rows[i][4].ToString() + "," +
                        dtDistinct.Rows[i][5].ToString() + "," +
                        dtDistinct.Rows[i][6].ToString() + "\n";

                }
                MessageBox.Show(tstr);

                //create columns in dttax table. dynamically creating the columns for each tax item
                {
                    dtTax.Columns.Add("HSNCode", typeof(string));
                    dtTax.Columns.Add("Amount", typeof(double));
                    for (int i = 0; i < dtDistinct.Rows.Count && i < 3; i++)
                    {
                        dtTax.Columns.Add(dtDistinct.Rows[i][4].ToString(), typeof(string));
                        dtTax.Columns.Add(dtDistinct.Rows[i][4].ToString() + "Amount", typeof(double));
                    }
                    dtTax.Columns.Add("Total", typeof(double));
                }
                //add data in dttax table

                for (int i = 0; i < (dttotal.Rows.Count); i++)
                {

                    Boolean hsnFount = false;
                    string tstr1 = dttotal.Rows[i][0].ToString();
                    int j = 0;
                    for (j = 0; j < (dtTax.Rows.Count); j++)
                    {
                        string tstr2 = dtTax.Rows[j][0].ToString();
                        if (tstr1 == tstr2)
                        {
                            hsnFount = true;

                            //////{
                            //////    string t1 = String.IsNullOrEmpty(dtTax.Rows[j][1].ToString()) ? "0" : dtTax.Rows[j][1].ToString();
                            //////    string t2 = String.IsNullOrEmpty(dttotal.Rows[i][2].ToString()) ? "0" : dttotal.Rows[i][2].ToString();
                            //////    double d1 = Convert.ToDouble(t1) + Convert.ToDouble(t2);
                            //////    dtTax.Rows[j][1] = d1;

                            //////}
                            break;
                        }
                    }
                    if (!hsnFount)
                    {
                        dtTax.Rows.Add();
                        j = dtTax.Rows.Count - 1;
                        dtTax.Rows[j][0] = tstr1;
                        dtTax.Rows[j][1] = dttotal.Rows[i][2];
                    }
                    string tstr3 = dttotal.Rows[i][4].ToString();
                    string tstr4 = dttotal.Rows[i][4].ToString() + "Amount";
                    try
                    {
                        dtTax.Rows[j][tstr3] = dttotal.Rows[i][5];
                        dtTax.Rows[j][tstr4] = dttotal.Rows[i][6];
                        string t1 = String.IsNullOrEmpty(dtTax.Rows[j]["Total"].ToString()) ? "0" : dtTax.Rows[j]["Total"].ToString();
                        string t2 = String.IsNullOrEmpty(dttotal.Rows[i][6].ToString()) ? "0" : dttotal.Rows[i][6].ToString();
                        double d1 = Convert.ToDouble(t1) + Convert.ToDouble(t2);
                        dtTax.Rows[j]["Total"] = d1;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error creating HSN wise tax summary");
                    }
                }
                tstr = "";
                double td1 = 0, td2 = 0, td3 = 0;
                for (int i = 0; i < (dtTax.Rows.Count); i++)
                {
                    for (int j = 0; j < dtTax.Columns.Count; j++)
                    {
                        dtTax.Rows[i][j] = String.IsNullOrEmpty(dtTax.Rows[i][j].ToString()) ? "0" : dtTax.Rows[i][j].ToString();
                        tstr = tstr + dtTax.Rows[i][j].ToString() + ",";
                    }
                    tstr = tstr + "\n";
                    td1 = td1 + Convert.ToDouble(dtTax.Rows[i][1].ToString());
                    td2 = td2 + Convert.ToDouble(dtTax.Rows[i][dtTax.Columns.Count - 1].ToString());
                }

                MessageBox.Show(tstr);
                MessageBox.Show(td1.ToString());
                MessageBox.Show(td2.ToString());
            }

            catch (Exception ex)
            {
                MessageBox.Show("taxDetails4Print() : Error - " + ex.ToString());
            }
            return dtTax;
        }

        public System.Data.DataTable taxDetails4Print(List<invoiceoutdetail> IODetails, string documentID)
        {
            int HSNLength = 0;
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataTable dtflat = new System.Data.DataTable();
            System.Data.DataTable dtflattotal = new System.Data.DataTable();
            System.Data.DataTable dtTax = new System.Data.DataTable();
            //----------
            System.Data.DataTable dtRowID = new System.Data.DataTable();
            dtRowID.Columns.Add("RowID", typeof(Int32));
            //----------
            try
            {
                if (documentID == "PRODUCTINVOICEOUT")
                {
                    HSNLength = 4;
                }
                else if (documentID == "SERVICEINVOICEOUT")
                {
                    HSNLength = 6;
                }

                {
                    dt.Columns.Add("HSNCode", typeof(string));
                    dt.Columns.Add("TaxCode", typeof(string));
                    dt.Columns.Add("Amount", typeof(double));
                    dt.Columns.Add("TaxAmount", typeof(double));
                    dt.Columns.Add("TaxItem", typeof(string));
                    dt.Columns.Add("TaxItemPercentage", typeof(double));
                    dt.Columns.Add("TaxItemAmount", typeof(double));
                    dt.Columns.Add("RowID", typeof(Int32));
                }
                //fill hsn code wise tax details in dt
                foreach (invoiceoutdetail iod in IODetails)
                {
                    string tstr1 = iod.TaxDetails;
                    string[] lst1 = tstr1.Split('\n');
                    for (int j = 0; j < lst1.Length - 1; j++)
                    {
                        string[] lst2 = lst1[j].Split('-');
                        if (Convert.ToDouble(lst2[1]) > 0)
                        {
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1][0] = iod.HSNCode.Substring(0, HSNLength);
                            dt.Rows[dt.Rows.Count - 1][1] = iod.TaxCode;
                            dt.Rows[dt.Rows.Count - 1][2] = iod.Quantity * iod.Price;
                            dt.Rows[dt.Rows.Count - 1][3] = iod.Tax;
                            dt.Rows[dt.Rows.Count - 1][4] = lst2[0];
                            dt.Rows[dt.Rows.Count - 1][5] = iod.HSNCode; //need to replace with percentage
                            dt.Rows[dt.Rows.Count - 1][6] = lst2[1];
                            dt.Rows[dt.Rows.Count - 1][7] = iod.RowID;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
            string tstr = "dt=\n";
            ////for (int i = 0; i < (dt.Rows.Count); i++)
            ////{
            ////    tstr = tstr +
            ////        dt.Rows[i][0].ToString() + "," +
            ////        dt.Rows[i][1].ToString() + "," +
            ////        dt.Rows[i][2].ToString() + "," +
            ////        dt.Rows[i][3].ToString() + "," +
            ////        dt.Rows[i][4].ToString() + "," +
            ////        dt.Rows[i][5].ToString() + "," +
            ////        dt.Rows[i][6].ToString() + "," +
            ////        dt.Rows[i][7].ToString() + "\n";

            ////}
            ////MessageBox.Show(tstr);



            //fill tax rate for each tax item in dt
            try
            {
                TaxCodeWorkingDB tcwdb = new TaxCodeWorkingDB();
                List<taxcodeworking> tcwDetails = tcwdb.getTaxCodeDetails();
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    foreach (taxcodeworking tcwd in tcwDetails)
                    {
                        if (dt.Rows[i][1].ToString() == tcwd.TaxCode && dt.Rows[i][4].ToString() == tcwd.TaxItemName)
                        {
                            dt.Rows[i][5] = tcwd.OperatorValue;
                            break;
                        }
                    }

                }

                ////tstr = "dt=\n";
                ////for (int i = 0; i < (dt.Rows.Count); i++)
                ////{
                ////    tstr = tstr +
                ////        dt.Rows[i][0].ToString() + "," +
                ////        dt.Rows[i][1].ToString() + "," +
                ////        dt.Rows[i][2].ToString() + "," +
                ////        dt.Rows[i][3].ToString() + "," +
                ////        dt.Rows[i][4].ToString() + "," +
                ////        dt.Rows[i][5].ToString() + "," +
                ////        dt.Rows[i][6].ToString() + "," +
                ////        dt.Rows[i][7].ToString() + "\n";
                ////}
                ////MessageBox.Show(tstr);
            }
            catch (Exception ex)
            {
            }

            ////3/4/2018 create flat table 
            try
            {
                {
                    dtflat.Columns.Add("HSNCode", typeof(string));

                    dtflat.Columns.Add("Amount", typeof(double));
                    dtflat.Columns.Add("TaxAmount", typeof(double));

                    dtflat.Columns.Add("IGST", typeof(double));
                    dtflat.Columns.Add("IGSTAmount", typeof(double));

                    dtflat.Columns.Add("CGST", typeof(double));
                    dtflat.Columns.Add("CGSTAmount", typeof(double));

                    dtflat.Columns.Add("SGST", typeof(double));
                    dtflat.Columns.Add("SGSTAmount", typeof(double));

                    dtflat.Columns.Add("RowID", typeof(Int32));
                }
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    //check row id in dtflat

                    int dtflatid = -1;
                    for (int j = 0; j < (dtflat.Rows.Count); j++)
                    {
                        int dtrowid = Convert.ToInt32(String.IsNullOrEmpty(dt.Rows[i][7].ToString()) ? "0" : dt.Rows[i][7].ToString());
                        int dtflatrowid = Convert.ToInt32(String.IsNullOrEmpty(dtflat.Rows[j][9].ToString()) ? "0" : dtflat.Rows[j][9].ToString());
                        if (dtrowid == dtflatrowid)
                        {
                            dtflatid = j;
                            break;
                        }
                    }
                    if (dtflatid == -1)
                    {
                        dtflat.Rows.Add();
                        dtflatid = dtflat.Rows.Count - 1;
                        dtflat.Rows[dtflatid][0] = dt.Rows[i][0]; //HSN Code
                        dtflat.Rows[dtflatid][1] = dt.Rows[i][2]; //Amount
                        dtflat.Rows[dtflatid][2] = dt.Rows[i][3]; //Tax Amount
                        dtflat.Rows[dtflatid][3] = 0.0;
                        dtflat.Rows[dtflatid][4] = 0.0;
                        dtflat.Rows[dtflatid][5] = 0.0;
                        dtflat.Rows[dtflatid][6] = 0.0;
                        dtflat.Rows[dtflatid][7] = 0.0;
                        dtflat.Rows[dtflatid][8] = 0.0;
                        dtflat.Rows[dtflatid][9] = dt.Rows[i][7]; //Row ID
                    }
                    string tstr3 = dt.Rows[i][4].ToString();
                    string tstr4 = dt.Rows[i][4].ToString() + "Amount";

                    try
                    {
                        dtflat.Rows[dtflatid][tstr3] = dt.Rows[i][5];
                        dtflat.Rows[dtflatid][tstr4] = dt.Rows[i][6];
                    }
                    catch (Exception ex)
                    {
                    }
                }

                //-----------
                //////tstr = "dtflat=\n";
                //////for (int i = 0; i < (dtflat.Rows.Count); i++)
                //////{
                //////    tstr = tstr +
                //////        dtflat.Rows[i][0].ToString() + "," +
                //////        dtflat.Rows[i][1].ToString() + "," +
                //////        dtflat.Rows[i][2].ToString() + "," +
                //////        dtflat.Rows[i][3].ToString() + "," +
                //////        dtflat.Rows[i][4].ToString() + "," +
                //////        dtflat.Rows[i][5].ToString() + "," +
                //////        dtflat.Rows[i][6].ToString() + "," +
                //////        dtflat.Rows[i][7].ToString() + "," +
                //////        dtflat.Rows[i][8].ToString() + "," +
                //////        dtflat.Rows[i][9].ToString() + "\n";

                //////}
                //////MessageBox.Show(tstr);

                //-----------
                //----create total on HSN Code and tax rates
                dtflattotal = dtflat.Copy();
                dtflattotal.Clear();
                Boolean fount = false;
                for (int i = 0; i < (dtflat.Rows.Count); i++)
                {
                    //check row id in dtflat
                    fount = false;
                    tstr = "i=" + i.ToString() + dtflat.Rows[i][0] + "-" + dtflat.Rows[i][3] + "-" +
                        dtflat.Rows[i][5] + "-" + dtflat.Rows[i][7];
                    ////MessageBox.Show(tstr);
                    string i0 = dtflat.Rows[i][0].ToString();
                    string i3 = dtflat.Rows[i][3].ToString();
                    string i5 = dtflat.Rows[i][5].ToString();
                    string i7 = dtflat.Rows[i][7].ToString();
                    for (int j = 0; j < (dtflattotal.Rows.Count); j++)
                    {
                        tstr = "j=" + j.ToString() + dtflattotal.Rows[j][0] + "-" + dtflattotal.Rows[j][3] + "-" +
                        dtflattotal.Rows[j][5] + "-" + dtflattotal.Rows[j][7];
                        ////MessageBox.Show(tstr);
                        string j0 = dtflattotal.Rows[j][0].ToString();
                        string j3 = dtflattotal.Rows[j][3].ToString();
                        string j5 = dtflattotal.Rows[j][5].ToString();
                        string j7 = dtflattotal.Rows[j][7].ToString();
                        tstr = i0 + "-" + i3 + "-" + i5 + "-" + i7 + "\n" +
                            j0 + "-" + j3 + "-" + j5 + "-" + j7;
                        ////MessageBox.Show(tstr);

                        if (i0 == j0 && i3 == j3 && i5 == j5 && i7 == j7)
                        {

                            dtflattotal.Rows[j][1] = Convert.ToDouble(dtflattotal.Rows[j][1].ToString()) +
                                Convert.ToDouble(dtflat.Rows[i][1].ToString());
                            dtflattotal.Rows[j][2] = Convert.ToDouble(dtflattotal.Rows[j][2].ToString()) +
                               Convert.ToDouble(dtflat.Rows[i][2].ToString());
                            dtflattotal.Rows[j][4] = Convert.ToDouble(dtflattotal.Rows[j][4].ToString()) +
                               Convert.ToDouble(dtflat.Rows[i][4].ToString());
                            dtflattotal.Rows[j][6] = Convert.ToDouble(dtflattotal.Rows[j][6].ToString()) +
                               Convert.ToDouble(dtflat.Rows[i][6].ToString());
                            dtflattotal.Rows[j][8] = Convert.ToDouble(dtflattotal.Rows[j][8].ToString()) +
                               Convert.ToDouble(dtflat.Rows[i][8].ToString());
                            fount = true;
                            break;
                        }
                    }
                    if (!fount)
                    {
                        dtflattotal.ImportRow(dtflat.Rows[i]);
                    }
                }
                //////tstr = "dtflattotal=\n";
                //////for (int i = 0; i < (dtflattotal.Rows.Count); i++)
                //////{
                //////    tstr = tstr +
                //////        dtflattotal.Rows[i][0].ToString() + "," +
                //////        dtflattotal.Rows[i][1].ToString() + "," +
                //////        dtflattotal.Rows[i][2].ToString() + "," +
                //////        dtflattotal.Rows[i][3].ToString() + "," +
                //////        dtflattotal.Rows[i][4].ToString() + "," +
                //////        dtflattotal.Rows[i][5].ToString() + "," +
                //////        dtflattotal.Rows[i][6].ToString() + "," +
                //////        dtflattotal.Rows[i][7].ToString() + "," +
                //////        dtflattotal.Rows[i][8].ToString() + "," +
                //////        dtflattotal.Rows[i][9].ToString() + "\n";

                //////}
                //////MessageBox.Show(tstr);
                //------------------
                //create dttax from dtflattotal
                {
                    dtTax.Columns.Add("HSNCode", typeof(string));
                    dtTax.Columns.Add("Amount", typeof(double));

                    dtTax.Columns.Add("IGST", typeof(double));
                    dtTax.Columns.Add("IGSTAmount", typeof(double));
                    dtTax.Columns.Add("CGST", typeof(double));
                    dtTax.Columns.Add("CGSTAmount", typeof(double));
                    dtTax.Columns.Add("SGST", typeof(double));
                    dtTax.Columns.Add("SGSTAmount", typeof(double));

                    dtTax.Columns.Add("Total", typeof(double));
                }
                for (int i = 0; i < (dtflattotal.Rows.Count); i++)
                {
                    dtTax.Rows.Add();
                    dtTax.Rows[dtTax.Rows.Count - 1][0] = dtflattotal.Rows[i][0];
                    dtTax.Rows[dtTax.Rows.Count - 1][1] = dtflattotal.Rows[i][1];
                    dtTax.Rows[dtTax.Rows.Count - 1][2] = dtflattotal.Rows[i][3];
                    dtTax.Rows[dtTax.Rows.Count - 1][3] = dtflattotal.Rows[i][4];
                    dtTax.Rows[dtTax.Rows.Count - 1][4] = dtflattotal.Rows[i][5];
                    dtTax.Rows[dtTax.Rows.Count - 1][5] = dtflattotal.Rows[i][6];
                    dtTax.Rows[dtTax.Rows.Count - 1][6] = dtflattotal.Rows[i][7];
                    dtTax.Rows[dtTax.Rows.Count - 1][7] = dtflattotal.Rows[i][8];
                    dtTax.Rows[dtTax.Rows.Count - 1][8] = Convert.ToDouble(dtflattotal.Rows[i][4].ToString()) +
                        Convert.ToDouble(dtflattotal.Rows[i][6].ToString()) +
                        Convert.ToDouble(dtflattotal.Rows[i][8].ToString());

                }
            }
            catch (Exception ex)
            {
            }

            return dtTax;
        }
        public static invoiceoutheader getInvOutHeaderFromInvNoDate(string docID, int InNo, DateTime InvDate)
        {
            invoiceoutheader ioh = new invoiceoutheader();
            try
            {
                string query = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " InvoiceNo,InvoiceDate,TrackingNos,TrackingDates,Consignee,ConsigneeName,TermsOfPayment,Description,TransportationMode,TransportationModeName,TransportationType," +
                    " TransportationTypeName,Transporter,TransporterName,CurrencyID,INRConversionRate,ADCode,EntryPort, " +
                    " ExitPort,FinalDestinationCountryID,FinalDestinationCountryName,OriginCountryID,OriginCountryName," +
                    "FinalDestinationPlace,PreCarriageTransportationMode,PreCarriageTransportationName,PreCarrierReceiptPlace,TermsOfDelivery,Remarks," +
                    "CommentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,Status,DocumentStatus,ProductValue,TaxAmount,InvoiceAmount,INRAmount,ProjectID " +
                    " , ProductValueINR, TaxAmountINR,BankAcReference,FreightCharge,DeliveryAddress,DeliveryStateCode,SJVTNo,SJVTDate,SJVNo,SJVDate,SpecialNote,ReverseCharge from ViewInvoiceOutHeader" +
                    " where DocumentID = '" + docID + "' and InvoiceNo = " + InNo + " and InvoiceDate = '" + InvDate.ToString("yyyy-MM-dd") + "' and DocumentStatus = 99" +
                     " and Status = 1 ";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ioh.RowID = reader.GetInt32(0);
                    ioh.DocumentID = reader.GetString(1);
                    ioh.DocumentName = reader.GetString(2);
                    ioh.TemporaryNo = reader.GetInt32(3);
                    ioh.TemporaryDate = reader.GetDateTime(4);
                    ioh.InvoiceNo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        ioh.InvoiceDate = reader.GetDateTime(6);
                    }
                    ioh.TrackingNos = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    ioh.TrackingDates = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    ioh.ConsigneeID = reader.GetString(9);
                    ioh.ConsigneeName = reader.GetString(10);
                    ioh.TermsOfPayment = reader.GetString(11);
                    ioh.Description = reader.IsDBNull(12) ? "" : reader.GetString(12);

                    ioh.TransportationMode = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    ioh.TransportationModeName = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    ioh.TransportationType = reader.IsDBNull(15) ? "" : reader.GetString(15);
                    ioh.TransportationTypeName = reader.IsDBNull(16) ? "" : reader.GetString(16);
                    ioh.Transporter = reader.IsDBNull(17) ? "" : reader.GetString(17);
                    ioh.TransporterName = reader.IsDBNull(18) ? "" : reader.GetString(18);

                    //popih.ValidityDate = reader.GetDateTime(12);
                    ioh.CurrencyID = reader.GetString(19);
                    // ioh.TaxCode = reader.GetString(20);
                    ioh.INRConversionRate = reader.GetDouble(20);
                    ioh.ADCode = reader.GetString(21);
                    ioh.EntryPort = reader.GetString(22);
                    ioh.ExitPort = reader.GetString(23);

                    ioh.FinalDestinatoinCountryID = reader.GetString(24);
                    if (!reader.IsDBNull(25))
                    {
                        ioh.FinalDestinatoinCountryName = reader.GetString(25);
                    }
                    else
                    {
                        ioh.FinalDestinatoinCountryName = "";
                    }

                    ioh.OriginCountryID = reader.GetString(26);
                    if (!reader.IsDBNull(27))
                    {
                        ioh.OriginCountryName = reader.GetString(27);
                    }
                    else
                    {
                        ioh.OriginCountryName = "";
                    }

                    ioh.FinalDestinationPlace = reader.GetString(28);
                    ioh.PreCarriageTransportationMode = reader.GetString(29);
                    if (!reader.IsDBNull(30))
                    {
                        ioh.PreCarriageTransportationName = reader.GetString(30);
                    }
                    else
                    {
                        ioh.PreCarriageTransportationName = "";
                    }

                    ioh.PreCarrierReceiptPlace = reader.GetString(31);
                    ioh.TermsOfDelivery = reader.GetString(32);
                    ioh.Remarks = reader.GetString(33);
                    if (!reader.IsDBNull(34))
                    {
                        ioh.CommentStatus = reader.GetString(34);
                    }
                    else
                    {
                        ioh.CommentStatus = "";
                    }
                    ioh.CreateUser = reader.GetString(35);
                    ioh.ForwardUser = reader.GetString(36);
                    ioh.ApproveUser = reader.GetString(37);
                    ioh.CreatorName = reader.GetString(38);
                    ioh.CreateTime = reader.GetDateTime(39);
                    ioh.ForwarderName = reader.GetString(40);
                    ioh.ApproverName = reader.GetString(41);
                    if (!reader.IsDBNull(42))
                    {
                        ioh.ForwarderList = reader.GetString(42);
                    }
                    else
                    {
                        ioh.ForwarderList = "";
                    }
                    ioh.status = reader.GetInt32(43);
                    ioh.DocumentStatus = reader.GetInt32(44);
                    ioh.ProductValue = reader.GetDouble(45);
                    ioh.TaxAmount = reader.GetDouble(46);
                    ioh.InvoiceAmount = reader.GetDouble(47);
                    ioh.INRAmount = reader.GetDouble(48);
                    ioh.ProjectID = reader.IsDBNull(49) ? "" : reader.GetString(49);
                    ioh.ProductValueINR = reader.GetDouble(50);
                    ioh.TaxAmountINR = reader.GetDouble(51);
                    ioh.BankAcReference = reader.IsDBNull(52) ? 0 : reader.GetInt32(52);
                    ioh.FreightCharge = reader.IsDBNull(53) ? 0 : reader.GetDouble(53);
                    ioh.DeliveryAddress = reader.IsDBNull(54) ? "" : reader.GetString(54);
                    ioh.DeliveryStateCode = reader.IsDBNull(55) ? "" : reader.GetString(55);

                    ioh.SJVTNo = reader.IsDBNull(56) ? 0 : reader.GetInt32(56);
                    ioh.SJVTDate = reader.IsDBNull(57) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(57);
                    ioh.SJVNo = reader.IsDBNull(58) ? 0 : reader.GetInt32(58);
                    ioh.SJVDate = reader.IsDBNull(59) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(59);

                    ioh.SpecialNote = reader.IsDBNull(60) ? "" : reader.GetString(60);
                    ioh.ReverseCharge = reader.IsDBNull(61) ? "" : reader.GetString(61);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice out Header Details");
            }
            return ioh;
        }
    }
}

