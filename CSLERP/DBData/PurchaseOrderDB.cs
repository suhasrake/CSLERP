using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    public class poheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int PONo { get; set; }
        public DateTime PODate { get; set; }
        public string ReferenceIndent { get; set; }
        public string ReferenceQuotation { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CurrencyID { get; set; }
        public int DeliveryPeriod { get; set; }
        public int validityPeriod { get; set; }
        public string TaxTerms { get; set; }
        public string CountryID { get; set; }
        public string ModeOfPayment { get; set; }
        public string TransportationMode { get; set; }
        public string PaymentTerms { get; set; }
        public decimal ExchangeRate { get; set; }
        public string FreightTerms { get; set; }
        public double FreightCharge { get; set; }
        public string DeliveryAddress { get; set; }
        public double ProductValue { get; set; }
        public double ProductValueINR { get; set; }
        public double TaxAmount { get; set; }
        public double TaxAmountINR { get; set; }
        public double POValue { get; set; }
        public double POValueINR { get; set; }
        public string Remarks { get; set; }
        public string TermsAndCondition { get; set; }
        public int Status { get; set; }
        public int DocumentStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string CreatorName { get; set; }
        public string ForwarderName { get; set; }
        public string ApproverName { get; set; }
        public string CommentStatus { get; set; }
        public string Comments { get; set; }
        public string ForwarderList { get; set; }
        public string SpecialNote { get; set; }

        public string PartialShipment { get; set; }
        public string Transhipment { get; set; }
        public string PackingSpec { get; set; }
        public string PriceBasis { get; set; }
        public string DeliveryAt { get; set; }
    }
    public class podetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Tax { get; set; }
        public int WarrantyDays { get; set; }
        public string TaxDetails { get; set; }
        public string TaxCode { get; set; }
        public double BilledQuantity { get; set;}
        public int MRNNo { get; set; }
        public string MRNDate { get; set; }
    }
    class MRNHeader2
    {
        public string PONos { get; set; }
        public string PODates { get; set; }
        public double MRNValue { get; set; }
        public int MRNNo { get; set; }
        public DateTime MRNDate { get; set; }
    }
    class PurchaseOrderDB
    {
        public List<poheader> getFilteredPurchaseOrderHeader(string userList, int opt, string userCommentStatusString)
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
            poheader poh;
            List<poheader> POHeader = new List<poheader>();
            try
            {
                string query1 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " PONo,PODate,ReferenceIndent,ReferenceQuotation,CustomerID,CustomerName,CurrencyID,DeliveryPeriod,ValidityPeriod,TaxTerms,ModeOfPayment,PaymentTerms," +
                    " FreightTerms,FreightCharge,DeliveryAddress,ProductValue,TaxAmount,POValue,Remarks, " +
                    " TermsAndCondition,Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,CommentStatus,ForwarderList " +
                    " ,ExchangeRate, ProductValueINR, POValueINR, TaxAmountINR,TransportationMode,SpecialNote,PartialShipment,Transhipment,PackingSpec,PriceBasis,DeliveryAt,CountryID from ViewPOHeader" +
                    " where ((forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (commentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) and Status not in (7,98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query2 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " PONo,PODate,ReferenceIndent,ReferenceQuotation,CustomerID,CustomerName,CurrencyID,DeliveryPeriod,ValidityPeriod,TaxTerms,ModeOfPayment,PaymentTerms," +
                    " FreightTerms,FreightCharge,DeliveryAddress,ProductValue,TaxAmount,POValue,Remarks, " +
                    " TermsAndCondition,Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,CommentStatus,ForwarderList " +
                   " ,ExchangeRate, ProductValueINR, POValueINR, TaxAmountINR,TransportationMode,SpecialNote,PartialShipment,Transhipment,PackingSpec,PriceBasis,DeliveryAt,CountryID from ViewPOHeader" +
                   " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98))  and Status not in (7,98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query3 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " PONo,PODate,ReferenceIndent,ReferenceQuotation,CustomerID,CustomerName,CurrencyID,DeliveryPeriod,ValidityPeriod,TaxTerms,ModeOfPayment,PaymentTerms," +
                    " FreightTerms,FreightCharge,DeliveryAddress,ProductValue,TaxAmount,POValue,Remarks, " +
                    " TermsAndCondition,Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName ,CommentStatus,ForwarderList" +
                    " ,ExchangeRate, ProductValueINR, POValueINR, TaxAmountINR,TransportationMode,SpecialNote,PartialShipment,Transhipment,PackingSpec,PriceBasis,DeliveryAt,CountryID from ViewPOHeader" +
                   " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99 and Status = 1)  order by PODate desc,DocumentID asc,PONo desc";
                string query6 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                   " PONo,PODate,ReferenceIndent,ReferenceQuotation,CustomerID,CustomerName,CurrencyID,DeliveryPeriod,ValidityPeriod,TaxTerms,ModeOfPayment,PaymentTerms," +
                   " FreightTerms,FreightCharge,DeliveryAddress,ProductValue,TaxAmount,POValue,Remarks, " +
                   " TermsAndCondition,Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName ,CommentStatus,ForwarderList" +
                   " ,ExchangeRate, ProductValueINR, POValueINR, TaxAmountINR,TransportationMode,SpecialNote,PartialShipment,Transhipment,PackingSpec,PriceBasis,DeliveryAt,CountryID from ViewPOHeader" +
                   " where  DocumentStatus = 99 and Status = 1 order by PODate desc,DocumentID asc,PONo desc";

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
                    case 6:
                        query = query6;
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
                    poh = new poheader();
                    poh.RowID = reader.GetInt32(0);
                    poh.DocumentID = reader.GetString(1);
                    poh.DocumentName = reader.GetString(2);
                    poh.TemporaryNo = reader.GetInt32(3);
                    poh.TemporaryDate = reader.GetDateTime(4);
                    poh.PONo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        poh.PODate = reader.GetDateTime(6);
                    }
                    poh.ReferenceIndent = reader.GetString(7);
                    poh.ReferenceQuotation = reader.GetString(8);
                    poh.CustomerID = reader.GetString(9);
                    poh.CustomerName = reader.GetString(10);
                    poh.CurrencyID = reader.GetString(11);
                    poh.DeliveryPeriod = reader.GetInt32(12);
                    poh.validityPeriod = reader.GetInt32(13);
                    poh.TaxTerms = reader.GetString(14);
                    poh.ModeOfPayment = reader.GetString(15);
                    poh.PaymentTerms = reader.GetString(16);
                    //poh.CreditPeriod = reader.GetInt32(17);
                    poh.FreightTerms = reader.GetString(17);
                    poh.FreightCharge = reader.GetDouble(18);
                    poh.DeliveryAddress = reader.GetString(19);
                    poh.ProductValue = reader.GetDouble(20);
                    poh.TaxAmount = reader.GetDouble(21);
                    poh.POValue = reader.GetDouble(22);
                    poh.Remarks = reader.GetString(23);
                    poh.TermsAndCondition = reader.GetString(24);
                    poh.Status = reader.GetInt32(25);
                    poh.DocumentStatus = reader.GetInt32(26);
                    poh.CreateTime = reader.GetDateTime(27);
                    poh.CreateUser = reader.GetString(28);
                    poh.ForwardUser = reader.GetString(29);
                    poh.ApproveUser = reader.GetString(30);
                    poh.CreatorName = reader.GetString(31);
                    poh.ForwarderName = reader.GetString(32);
                    poh.ApproverName = reader.GetString(33);
                    if (!reader.IsDBNull(34))
                    {
                        poh.CommentStatus = reader.GetString(34);
                    }
                    else
                    {
                        poh.CommentStatus = "";
                    }
                    if (!reader.IsDBNull(35))
                    {
                        poh.ForwarderList = reader.GetString(35);
                    }
                    else
                    {
                        poh.ForwarderList = "";
                    }
                    poh.ExchangeRate = reader.GetDecimal(36);
                    poh.ProductValueINR = reader.GetDouble(37);
                    poh.POValueINR = reader.GetDouble(38);
                    poh.TaxAmountINR = reader.GetDouble(39);
                    poh.TransportationMode = reader.IsDBNull(40) ? "" : reader.GetString(40);
                    poh.SpecialNote = reader.IsDBNull(41) ? "" : reader.GetString(41);
                    poh.PartialShipment = reader.IsDBNull(42) ? "" : reader.GetString(42);
                    poh.Transhipment = reader.IsDBNull(43) ? "" : reader.GetString(43);
                    poh.PackingSpec = reader.IsDBNull(44) ? "" : reader.GetString(44);
                    poh.PriceBasis = reader.IsDBNull(45) ? "" : reader.GetString(45);
                    poh.DeliveryAt = reader.IsDBNull(46) ? "" : reader.GetString(46);
                    poh.CountryID = reader.IsDBNull(47) ? "" : reader.GetString(47);
                    POHeader.Add(poh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Work Order Header Details");
            }
            return POHeader;
        }

        public static List<podetail> getPurchaseOrderDetails(poheader poh)
        {
            podetail pod;
            List<podetail> PODetail = new List<podetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,TemporaryNo,TemporaryDate,StockItemID,StockItemName,Modelno, ModelName,Description, " +
                   "Quantity,Price,Tax,WarrantyDays,TaxDetails,Unit,TaxCode " +
                   "from ViewPODetail " +
                   "where DocumentID='" + poh.DocumentID + "'" +
                   " and TemporaryNo=" + poh.TemporaryNo +
                   " and TemporaryDate='" + poh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pod = new podetail();
                    pod.RowID = reader.GetInt32(0);
                    pod.DocumentID = reader.GetString(1);
                    pod.TemporaryNo = reader.GetInt32(2);
                    pod.TemporaryDate = reader.GetDateTime(3).Date;
                    pod.StockItemID = reader.GetString(4);
                    pod.StockItemName = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                    {
                        pod.ModelNo = reader.GetString(6);
                    }
                    else
                    {
                        pod.ModelNo = "NA";
                    }
                    if (!reader.IsDBNull(7))
                    {
                        pod.ModelName = reader.GetString(7);
                    }
                    else
                    {
                        pod.ModelName = "NA";
                    }


                    pod.Description = reader.GetString(8);
                    pod.Quantity = reader.GetDouble(9);
                    pod.Price = reader.GetDouble(10);
                    pod.Tax = reader.GetDouble(11);
                    pod.WarrantyDays = reader.GetInt32(12);
                    pod.TaxDetails = reader.GetString(13);
                    pod.Unit = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    pod.TaxCode = reader.IsDBNull(15) ? "" : reader.GetString(15);
                    PODetail.Add(pod);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Work Order Details");
            }
            return PODetail;
        }

        public Boolean validatePurchaseOrderHeader(poheader poh)
        {
            Boolean status = true;
            try
            {
                if (poh.CustomerID.Trim().Length == 0 || poh.CustomerID == null)
                {
                    return false;
                }
                if (poh.TaxTerms.Trim().Length == 0 || poh.TaxTerms == null)
                {
                    return false;
                }
                if (poh.CurrencyID.Trim().Length == 0 || poh.CurrencyID == null)
                {
                    return false;
                }

                if (poh.DeliveryPeriod == 0)
                {
                    return false;
                }
                if (poh.validityPeriod == 0 || poh.validityPeriod < poh.DeliveryPeriod)
                {
                    return false;
                }
                if (poh.ExchangeRate == 0)
                {
                    return false;
                }
                if (poh.ProductValue == 0)
                {
                    return false;
                }
                if (poh.POValue == 0)
                {
                    return false;
                }
                if (poh.ProductValueINR == 0)
                {
                    return false;
                }
                if (poh.POValueINR == 0)
                {
                    return false;
                }
                if (poh.FreightTerms.Trim().Length == 0 || poh.FreightTerms == null)
                {
                    return false;
                }
                if (poh.ModeOfPayment.Trim().Length == 0 || poh.ModeOfPayment == null)
                {
                    return false;
                }
                if (poh.TransportationMode.Trim().Length == 0 || poh.TransportationMode == null)
                {
                    return false;
                }
                if (poh.DeliveryAddress.Trim().Length == 0 || poh.DeliveryAddress == null)
                {
                    return false;
                }
                if (poh.Remarks.Trim().Length == 0 || poh.Remarks == null)
                {
                    return false;
                }
                if (poh.TermsAndCondition.Trim().Length == 0 || poh.TermsAndCondition == null)
                {
                    return false;
                }
                if (poh.PaymentTerms.Trim().Length == 0 || poh.PaymentTerms == null)
                {
                    return false;
                }
                //if (poh.PaymentTerms == "Credit")
                //{
                //    if (poh.CreditPeriod == 0)
                //    {
                //        return false;
                //    }
                //}
                if (poh.FreightTerms == "Extra")
                {
                    if (poh.FreightCharge == 0)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public Boolean ForwardPurchaseOrder(poheader poh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POHeader set DocumentStatus=" + (poh.DocumentStatus + 1) +
                    ", forwardUser='" + poh.ForwardUser + "'" +
                    ", commentStatus='" + poh.CommentStatus + "'" +
                    ", ForwarderList='" + poh.ForwarderList + "'" +
                    " where DocumentID='" + poh.DocumentID + "'" +
                    " and TemporaryNo=" + poh.TemporaryNo +
                    " and TemporaryDate='" + poh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POHeader", "", updateSQL) +
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
        public Boolean reversePO(poheader poh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POHeader set DocumentStatus=" + poh.DocumentStatus +
                    ", forwardUser='" + poh.ForwardUser + "'" +
                    ", commentStatus='" + poh.CommentStatus + "'" +
                    ", ForwarderList='" + poh.ForwarderList + "'" +
                    " where DocumentID='" + poh.DocumentID + "'" +
                    " and TemporaryNo=" + poh.TemporaryNo +
                    " and TemporaryDate='" + poh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POHeader", "", updateSQL) +
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
        public Boolean ApprovePurchaseOrder(poheader poh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POHeader set DocumentStatus=99, status=1 " +
                   ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + poh.CommentStatus + "'" +
                    ", PONo=" + poh.PONo +
                    ", PODate=convert(date, getdate())" +
                    " where DocumentID='" + poh.DocumentID + "'" +
                    " and TemporaryNo=" + poh.TemporaryNo +
                    " and TemporaryDate='" + poh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POHeader", "", updateSQL) +
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
                string query = "select comments from POHeader where DocumentID='" + docid + "'" +
                        " and TemporaryNo=" + tempno +
                        " and TemporaryDate='" + tempdate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    cmtString = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return cmtString;
        }
        public static string getTempNo(int PONo, DateTime PODate)
        {
            poheader poh = new poheader();
            try
            {
                string query = "select TemporaryNo, TemporaryDate from POHeader " +
                    "where PONo=" + PONo +
                    "and PODate='" + PODate.ToString("yyyy-MM-dd") + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    poh.TemporaryNo = reader.GetInt32(0);
                    poh.TemporaryDate = reader.GetDateTime(1);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return poh.TemporaryNo + ";" + poh.TemporaryDate;
        }
        public static ListView PODetailForMRN(string CustID)
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
                PurchaseOrderDB podb = new PurchaseOrderDB();
                List<poheader> POHeaders = podb.getFilteredPurchaseOrderHeader("", 6, "").OrderBy(poh => poh.CreateTime).ToList();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Temp No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Temp Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Cust PO", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO Date", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Cust Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Prod Value", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Tax Amt", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Currency ID", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Exchange Rate", -2, HorizontalAlignment.Center);
                //lv.Columns.Add("Tax Code", -2, HorizontalAlignment.Center);
                foreach (poheader poh in POHeaders)
                {
                    if (poh.CustomerID.Equals(CustID))
                    {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        item1.SubItems.Add(poh.TemporaryNo.ToString());
                        item1.SubItems.Add(poh.TemporaryDate.ToShortDateString());
                        item1.SubItems.Add(poh.PONo.ToString());
                        item1.SubItems.Add(poh.PODate.ToString("yyyy-MM-dd"));//ToShortDateString());
                        item1.SubItems.Add(poh.CustomerName);
                        item1.SubItems.Add(poh.ProductValue.ToString());
                        item1.SubItems.Add(poh.TaxAmount.ToString());
                        item1.SubItems.Add(poh.CurrencyID.ToString());
                        item1.SubItems.Add(poh.ExchangeRate.ToString());
                        //item1.SubItems.Add(poh.TaxCode.ToString());
                        lv.Items.Add(item1);
                        ////index++;
                    }

                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static List<podetail> getPODetailForMRNDetail(int TempNo, DateTime TempDate)
        {
            podetail pod;
            List<podetail> PODetail = new List<podetail>();
            try
            {
                //SqlConnection conn = new SqlConnection(Login.connString);
                string query1 = "select TemporaryNo, TemporaryDate,StockItemID,StockItemName,ModelNo, ModelName, " +
                  "Quantity,Price,Tax,WarrantyDays,TaxDetails,Unit,TaxCode,RowID " +
                   "from ViewPODetail " +
                   " where TemporaryNo=" + TempNo +
                   " and TemporaryDate='" + TempDate.ToString("yyyy-MM-dd") + "'" +
                   " order by StockItemID";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query1, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pod = new podetail();
                    pod.TemporaryNo = reader.GetInt32(0);
                    pod.TemporaryDate = reader.GetDateTime(1).Date;
                    pod.StockItemID = reader.GetString(2);
                    pod.StockItemName = reader.GetString(3);
                    pod.ModelNo = reader.IsDBNull(4) ? "NA" : reader.GetString(4);
                    pod.ModelName = reader.IsDBNull(5) ? "NA" : reader.GetString(5);
                    pod.Quantity = reader.GetDouble(6);
                    pod.Price = reader.GetDouble(7);
                    pod.Tax = reader.GetDouble(8);
                    pod.WarrantyDays = reader.GetInt32(9);
                    pod.TaxDetails = reader.GetString(10);
                    pod.Unit = reader.GetString(11);
                    pod.TaxCode = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    pod.RowID = reader.GetInt32(13);
                    PODetail.Add(pod);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Purchase Order Details");
            }
            return PODetail;
        }

        public static string fillMRStockItemGridViewComboWithValue(DataGridViewComboBoxCell cmb, List<podetail> PODetail, int RowNo)
        {
            int count = 0;
            string firstValue = "";
            cmb.Items.Clear();

            foreach (podetail pod in PODetail)
            {
                count++;
                cmb.Items.Add(pod.StockItemID + "-" + pod.StockItemName);
                if (RowNo == count)
                {
                    firstValue = pod.StockItemID + "-" + pod.StockItemName;
                }
            }
            return firstValue;
        }
        public static void fillMRStockItemGridViewCombo(DataGridViewComboBoxCell cmb, string PONo, DateTime CustomerPODate)
        {
            cmb.Items.Clear();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.TemporaryNo, a.TemporaryDate,a.StockItemID,b.Name " +
                   "from PODetail a, StockItem b  where a.StockItemID= b.StockItemID " +
                   " and a.TemporaryNo=(select TemporaryNo from POHeader where PONo = '" + PONo + "'" + "and PODate='" + CustomerPODate.ToString("yyyy-MM-dd") + "')" +
                   " and a.TemporaryDate=(select TemporaryDate from POHeader where PONo = '" + PONo + "'" + "and PODate='" + CustomerPODate.ToString("yyyy-MM-dd") + "')" +
                   " order by a. StockItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cmb.Items.Add(reader.GetString(2) + "-" + reader.GetString(3));
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in filling Stock Item in gridview");
            }
        }
        public Boolean updatePOHeaderAndDetail(poheader poh, poheader prevpoh, List<podetail> PODetail)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POHeader set TemporaryNo = " + poh.TemporaryNo +
                     ", TemporaryDate='" + poh.TemporaryDate.ToString("yyyy-MM-dd") +
                     //"', PONo=" + poh.PONo +
                     //", PODate='" + poh.PODate.ToString("yyyy-MM-dd") +
                     "', ReferenceIndent='" + poh.ReferenceIndent +
                     "', ReferenceQuotation='" + poh.ReferenceQuotation +
                     "', CustomerID='" + poh.CustomerID +
                     "', CurrencyID='" + poh.CurrencyID +
                      "',ExchangeRate=" + poh.ExchangeRate +
                     ", DeliveryPeriod =" + poh.DeliveryPeriod +
                      ", ValidityPeriod =" + poh.validityPeriod +
                     ", TaxTerms='" + poh.TaxTerms +
                     "', ModeOfPayment='" + poh.ModeOfPayment +
                       "', TransportationMode='" + poh.TransportationMode +
                     "', PaymentTerms='" + poh.PaymentTerms +
                     "', FreightTerms='" + poh.FreightTerms +
                     "', FreightCharge=" + poh.FreightCharge +
                     ", DeliveryAddress ='" + poh.DeliveryAddress +
                     "',ProductValue=" + poh.ProductValue + "," +
                      "ProductValueINR=" + poh.ProductValueINR + "," +
                     "TaxAmount=" + poh.TaxAmount + "," +
                      "TaxAmountINR=" + poh.TaxAmountINR + "," +
                     "POValue= " + poh.POValue +
                      ",POValueINR=" + poh.POValueINR + "," +
                     " Remarks ='" + poh.Remarks +
                     "', TermsAndCondition ='" + poh.TermsAndCondition + "'"+
                     //"', Status =" + poh.Status +
                     ", CommentStatus='" + poh.CommentStatus +
                     "', Comments='" + poh.Comments +
                     "', ForwarderList='" + poh.ForwarderList + "'" +
                      ", SpecialNote='" + poh.SpecialNote + "'" +
                      ", Transhipment='" + poh.Transhipment + "'" +
                      ", PartialShipment='" + poh.PartialShipment + "'" +
                      ", PackingSpec='" + poh.PackingSpec + "'" +
                       ", PriceBasis='" + poh.PriceBasis + "'" +
                      ", DeliveryAt='" + poh.DeliveryAt + "'" +
                    " where DocumentID='" + prevpoh.DocumentID + "'" +
                    " and TemporaryNo=" + prevpoh.TemporaryNo +
                    " and TemporaryDate='" + prevpoh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from PODetail where DocumentID='" + prevpoh.DocumentID + "'" +
                    " and TemporaryNo=" + prevpoh.TemporaryNo +
                    " and TemporaryDate='" + prevpoh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "PODetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (podetail pod in PODetail)
                {
                    updateSQL = "insert into PODetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,Description,TaxCode,Quantity,Price,Tax,WarrantyDays,TaxDetails) " +
                    "values ('" + pod.DocumentID + "'," +
                    pod.TemporaryNo + "," +
                    "'" + pod.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + pod.StockItemID + "'," +
                       "'" + pod.ModelNo + "'," +
                    "'" + pod.Description + "'," +
                     "'" + pod.TaxCode + "'," +
                    pod.Quantity + "," +
                    pod.Price + " ," +
                    pod.Tax + "," +
                    pod.WarrantyDays + "," +
                    "'" + pod.TaxDetails + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "PODetail", "", updateSQL) +
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
        public Boolean InsertPOHeaderAndDetail(poheader poh, List<podetail> PODetail)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                poh.TemporaryNo = DocumentNumberDB.getNumber(poh.DocumentID, 1);
                if (poh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + poh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + poh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into POHeader " +
                    "(DocumentID,TemporaryNo,TemporaryDate,PONo,PODate,ReferenceIndent,ReferenceQuotation,CustomerID,CurrencyID,ExchangeRate," +
                    "DeliveryPeriod,ValidityPeriod,TaxTerms,ModeOfPayment,TransportationMode,PaymentTerms," +
                    "FreightTerms,FreightCharge,DeliveryAddress,ProductValue,ProductValueINR,TaxAmount,TaxAmountINR,POValue,POValueINR,Remarks," +
                    "PartialShipment,Transhipment,PackingSpec,PriceBasis,DeliveryAt,TermsAndCondition,Status,DocumentStatus,CreateTime,CreateUser,CommentStatus,SpecialNote,Comments,ForwarderList)" +
                    " values (" +
                    "'" + poh.DocumentID + "'," +
                    poh.TemporaryNo + "," +
                    "'" + poh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    poh.PONo + "," +
                    "'" + poh.PODate.ToString("yyyy-MM-dd") + "'," +
                    "'" + poh.ReferenceIndent + "'," +
                    "'" + poh.ReferenceQuotation + "'," +
                    "'" + poh.CustomerID + "'," +
                    "'" + poh.CurrencyID + "'," +
                     poh.ExchangeRate + "," +
                    poh.DeliveryPeriod + "," +
                    poh.validityPeriod + "," +
                    "'" + poh.TaxTerms + "'," +
                    "'" + poh.ModeOfPayment + "'," +
                      "'" + poh.TransportationMode + "'," +
                    "'" + poh.PaymentTerms + "'," +
                    //poh.CreditPeriod + "," +
                    "'" + poh.FreightTerms + "'," +
                    poh.FreightCharge + "," +
                    "'" + poh.DeliveryAddress + "'," +
                    poh.ProductValue + "," +
                     poh.ProductValueINR + "," +
                    poh.TaxAmount + "," +
                     poh.TaxAmountINR + "," +
                    poh.POValue + "," +
                    poh.POValueINR + "," +
                    "'" + poh.Remarks + "'," +
                     "'" + poh.PartialShipment + "'," +
                      "'" + poh.Transhipment + "'," +
                       "'" + poh.PackingSpec + "'," +
                        "'" + poh.PriceBasis + "'," +
                       "'" + poh.DeliveryAt + "'," +
                    "'" + poh.TermsAndCondition + "'," +
                    poh.Status + "," +
                    poh.DocumentStatus + "," +
                     "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" +
                    ",'" + poh.CommentStatus + "'," +
                     "'" + poh.SpecialNote + "'," +
                    "'" + poh.Comments + "'," +
                    "'" + poh.ForwarderList + "')";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "POHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from PODetail where DocumentID='" + poh.DocumentID + "'" +
                   " and TemporaryNo=" + poh.TemporaryNo +
                   " and TemporaryDate='" + poh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "PODetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (podetail pod in PODetail)
                {
                    updateSQL = "insert into PODetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,Description,TaxCode,Quantity,Price,Tax,WarrantyDays,TaxDetails) " +
                    "values ('" + pod.DocumentID + "'," +
                    poh.TemporaryNo + "," +
                    "'" + pod.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + pod.StockItemID + "'," +
                       "'" + pod.ModelNo + "'," +
                    "'" + pod.Description + "'," +
                      "'" + pod.TaxCode + "'," +
                    pod.Quantity + "," +
                    pod.Price + " ," +
                    pod.Tax + "," +
                    pod.WarrantyDays + "," +
                    "'" + pod.TaxDetails + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "PODetail", "", updateSQL) +
                    Main.QueryDelimiter;
                }
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
        public static List<podetail> getPODetailForInvoice(int PONo, DateTime poDate)
        {
            List<podetail> polist = new List<podetail>();
            podetail pod = new podetail();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.StockItemID,a.StockItemName,a.ModelNo,a.ModelName,a.Quantity,a.Price " +
                   "from ViewPODetail a " +
                   " where a.TemporaryNo = (select TemporaryNo from POHeader where PONo =  '" + PONo + "' and PODate = '"+poDate.ToString("yyyy-MM-dd") + "')" +
                   " and a.TemporaryDate = (select TemporaryDate from POHeader where  PONo =  '" + PONo + "' and PODate = '" + poDate.ToString("yyyy-MM-dd") + "')" +
                   "";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pod = new podetail();
                    pod.StockItemID = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    pod.StockItemName = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    pod.ModelNo = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    pod.ModelName = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    pod.Quantity = reader.GetDouble(4);
                    pod.Price = reader.GetDouble(5);
                    polist.Add(pod);
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in retriving PO Detail");
            }
            return polist;
        }
        public static ListView getPODetailForInvoiceIN(int poNo, DateTime poDate)
        {
            ListView lv = new ListView();
            try
            {

                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                ///lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                List<podetail> PODetails = PurchaseOrderDB.getPODetailForInvoice(poNo, poDate);
                ////int index = 0;
                lv.Columns.Add("PO No", -2, HorizontalAlignment.Center);
                lv.Columns.Add("PO Date", -2, HorizontalAlignment.Center);
                lv.Columns.Add("StockItem ID", -2, HorizontalAlignment.Center);
                lv.Columns.Add("StockItem Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Model No", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Model Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Quantity", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Price", -2, HorizontalAlignment.Center);
                lv.Columns[5].Width = 150;
                lv.Columns[3].Width = 280;
                foreach (podetail pod in PODetails)
                {
                    ListViewItem item1 = new ListViewItem(poNo.ToString());
                    item1.SubItems.Add(poDate.ToShortDateString());
                    item1.SubItems.Add(pod.StockItemID.ToString());
                    item1.SubItems.Add(pod.StockItemName.ToString());
                    item1.SubItems.Add(pod.ModelNo.ToString());
                    item1.SubItems.Add(pod.ModelName.ToString());
                    item1.SubItems.Add(pod.Quantity.ToString());
                    item1.SubItems.Add(pod.Price.ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }

        public static double getPOQuantityFor(int PORefNo)
        {
            double Qunt = 0;
            try
            {
                //string query1 = "select b.Quantity from POHeader a, PODetail b " +
                //    "where a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate " +
                //    "and b.TemporaryNo = (select TemporaryNo from POHeader where PONo = " + PONo + " and PODate = '" + PODate.ToString("yyyy-MM-dd") + "')" +
                //    " and b.TemporaryDate = (select TemporaryDate from POHeader where PONo = " + PONo + " and PODate = '" + PODate.ToString("yyyy-MM-dd") + "')" +
                //    " and b.StockItemID = '" + StockId + "' and b.ModelNo = '" + ModelNo + "'";
                string query = "select Quantity from PODetail " +
                    "where RowID = " + PORefNo;
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Qunt = reader.GetDouble(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return Qunt;
        }

        public static podetail getPODetailsFromRowID(int RowID)
        {
            podetail pod = new podetail();
            try
            {
                string query = "select RowID,TemporaryNo, TemporaryDate,StockItemID,StockItemName,Modelno, ModelName,Description," +
                    "Quantity,Price,Tax from ViewPODetail " +
                    "where RowID=" + RowID;
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    pod.RowID = reader.GetInt32(0);
                    pod.TemporaryNo = reader.GetInt32(1);
                    pod.TemporaryDate = reader.GetDateTime(2).Date;
                    pod.StockItemID = reader.GetString(3);
                    pod.StockItemName = reader.GetString(4);
                    pod.ModelNo = reader.IsDBNull(5)?"NA":reader.GetString(5);
                    pod.ModelName = reader.IsDBNull(6) ? "NA" : reader.GetString(6);
                    pod.Description = reader.GetString(7);
                    pod.Quantity = reader.GetDouble(8);
                    pod.Price = reader.GetDouble(9);
                    pod.Tax = reader.GetDouble(10);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return pod;
        }
        public Boolean ClosePOPI2(poheader popih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POHeader set status=7 " +
                    " where PONo=" + popih.PONo +
                    " and PODate='" + popih.PODate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POProductInwardHeader", "", updateSQL) +
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
        public Boolean ClosePOPICheck(poheader popih)
        {
            Boolean status = false;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,DocumentID,PONo,PODate " +
                   "from POHeader  where PONo='" + popih.PONo + "' and PODate='" + popih.PODate.ToString("yyyy-MM-dd") + "' and Status = 7 ";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    status = true;
                }
                conn.Close();
            }

            catch (Exception Ex)
            {
                status = false;
            }
            return status;
        }
        public List<MRNHeader2> getMRNHeaderdata()
        {
            MRNHeader2 popid;
            List<MRNHeader2> POPIDetail = new List<MRNHeader2>();
            try
            {
                //string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select PONos,PODates,ProductValueINR,MRNNo,MRNDate from ViewMRNHeader  where Status =1 and DocumentStatus = 99  and QCStatus = 99 ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popid = new MRNHeader2();
                    popid.PONos = reader.GetString(0);
                    popid.PODates = reader.GetString(1);
                    popid.MRNValue = reader.GetDouble(2);
                    popid.MRNNo = reader.GetInt32(3);
                    popid.MRNDate = reader.GetDateTime(4);
                    POPIDetail.Add(popid);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying PO Product Inward Details");
            }
            return POPIDetail;
        }
        public List<poheader> ListPopiFilters2(string QueryData)
        {
            poheader popih;
            List<poheader> POPIHeadersReport = new List<poheader>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = QueryData;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popih = new poheader();
                    popih.DocumentID = reader.GetString(0);
                    popih.TemporaryNo = reader.GetInt32(1);
                    popih.TemporaryDate = reader.GetDateTime(2);
                    popih.PONo = reader.GetInt32(3);
                    popih.PODate = reader.GetDateTime(4);
                    popih.CustomerName = reader.GetString(5);
                    popih.ProductValueINR = reader.GetDouble(6);
                    POPIHeadersReport.Add(popih);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO Product Inward Header Report");
            }
            return POPIHeadersReport;
        }

        public static List<podetail> getPOPIDetailqty2(poheader popih)
        {
            podetail popid;
            List<podetail> POPIDetail = new List<podetail>();
            try
            {
                //string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);

                string query = "select P.RowID ,P.StockItemName,p.Description,p.Quantity,p.price,M.QuantityAccepted,Mh.MRNNo,Mh.MRNDate  from ViewPODetail P LEFT OUTER JOIN ViewMRNDetail M ON P.RowID = M.POItemReferenceNo LEFT Outer JOIN ViewMRNHeader Mh ON M.TemporaryNo = Mh.TemporaryNo and M.TemporaryDate = Mh.TemporaryDate where P.TemporaryNo = " + popih.TemporaryNo + " and p.TemporaryDate = '" + popih.TemporaryDate.ToString("yyyy-MM-dd") + "' order by P.StockItemName";



                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popid = new podetail();
                    popid.RowID = reader.GetInt32(0);
                    popid.StockItemName = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    popid.Description = reader.GetString(2);
                    popid.Quantity = reader.GetDouble(3);
                    popid.Price = reader.GetDouble(4);
                    popid.BilledQuantity = reader.IsDBNull(5) ? 0 : reader.GetDouble(5);
                    popid.MRNNo = reader.IsDBNull(6) ? 0 : reader.GetInt32(6);
                    popid.MRNDate = reader.IsDBNull(7) ? "" : reader.GetDateTime(7).ToString("dd-MM-yyyy");
                    POPIDetail.Add(popid);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO Product Inward Details");
            }
            return POPIDetail;
        }
        public List<poheader> getFilteredPurchaseOrderHeaderlist(string docid, int tempno, DateTime tempdate)
        {
            poheader poh;
            List<poheader> POHeader = new List<poheader>();
            try
            {
                string query = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                   " PONo,PODate,ReferenceIndent,ReferenceQuotation,CustomerID,CustomerName,CurrencyID,DeliveryPeriod,ValidityPeriod,TaxTerms,ModeOfPayment,PaymentTerms," +
                   " FreightTerms,FreightCharge,DeliveryAddress,ProductValue,TaxAmount,POValue,Remarks, " +
                   " TermsAndCondition,Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName ,CommentStatus,ForwarderList" +
                   " ,ExchangeRate, ProductValueINR, POValueINR, TaxAmountINR,TransportationMode,SpecialNote from ViewPOHeader" +
                   " where DocumentID='" + docid + "' and TemporaryNo=" + tempno + " and TemporaryDate='" + tempdate.ToString("yyyy-MM-dd") + "' " +
                   " and DocumentStatus = 99 and Status = 1 order by PODate desc,DocumentID asc,PONo desc";

                SqlConnection conn = new SqlConnection(Login.connString);

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    poh = new poheader();
                    poh.RowID = reader.GetInt32(0);
                    poh.DocumentID = reader.GetString(1);
                    poh.DocumentName = reader.GetString(2);
                    poh.TemporaryNo = reader.GetInt32(3);
                    poh.TemporaryDate = reader.GetDateTime(4);
                    poh.PONo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        poh.PODate = reader.GetDateTime(6);
                    }
                    poh.ReferenceIndent = reader.GetString(7);
                    poh.ReferenceQuotation = reader.GetString(8);
                    poh.CustomerID = reader.GetString(9);
                    poh.CustomerName = reader.GetString(10);
                    poh.CurrencyID = reader.GetString(11);
                    poh.DeliveryPeriod = reader.GetInt32(12);
                    poh.validityPeriod = reader.GetInt32(13);
                    poh.TaxTerms = reader.GetString(14);
                    poh.ModeOfPayment = reader.GetString(15);
                    poh.PaymentTerms = reader.GetString(16);
                    //poh.CreditPeriod = reader.GetInt32(17);
                    poh.FreightTerms = reader.GetString(17);
                    poh.FreightCharge = reader.GetDouble(18);
                    poh.DeliveryAddress = reader.GetString(19);
                    poh.ProductValue = reader.GetDouble(20);
                    poh.TaxAmount = reader.GetDouble(21);
                    poh.POValue = reader.GetDouble(22);
                    poh.Remarks = reader.GetString(23);
                    poh.TermsAndCondition = reader.GetString(24);
                    poh.Status = reader.GetInt32(25);
                    poh.DocumentStatus = reader.GetInt32(26);
                    poh.CreateTime = reader.GetDateTime(27);
                    poh.CreateUser = reader.GetString(28);
                    poh.ForwardUser = reader.GetString(29);
                    poh.ApproveUser = reader.GetString(30);
                    poh.CreatorName = reader.GetString(31);
                    poh.ForwarderName = reader.GetString(32);
                    poh.ApproverName = reader.GetString(33);
                    if (!reader.IsDBNull(34))
                    {
                        poh.CommentStatus = reader.GetString(34);
                    }
                    else
                    {
                        poh.CommentStatus = "";
                    }
                    if (!reader.IsDBNull(35))
                    {
                        poh.ForwarderList = reader.GetString(35);
                    }
                    else
                    {
                        poh.ForwarderList = "";
                    }
                    poh.ExchangeRate = reader.GetDecimal(36);
                    poh.ProductValueINR = reader.GetDouble(37);
                    poh.POValueINR = reader.GetDouble(38);
                    poh.TaxAmountINR = reader.GetDouble(39);
                    poh.TransportationMode = reader.IsDBNull(40) ? "" : reader.GetString(40);
                    poh.SpecialNote = reader.IsDBNull(41) ? "" : reader.GetString(41);
                    POHeader.Add(poh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Work Order Header List Details");
            }
            return POHeader;
        }

        public static podetail getPOQuantAndPRiceFromRowID(int RowID)
        {
            podetail pod = new podetail();
            try
            {
                string query = "select RowID,Quantity, Price from PODetail where RowID = (Select POItemReferenceNo from MRNDetail where RowID = " + RowID + ")";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    pod.RowID = reader.GetInt32(0);
                    pod.Quantity = reader.GetDouble(1);
                    pod.Price = reader.GetDouble(2);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return pod;
        }
        public static Boolean isMRNPreparedForPO(int PONo, DateTime PODate)
        {
            Boolean isAvail = false;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select PONos,PODates from MRNHeader where " +
                        " PONos like '%;" + PONo + ";%'" +
                        " and PODates like '%;" + PODate.ToString("yyyy-MM-dd") + ";%' and Status != '98'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string ponos = reader.GetString(0);
                    string podates = reader.GetString(1);
                    string[] subPoNo = ponos.Split(';');
                    string[] subPodate = podates.Split(';');
                    if(Array.IndexOf(subPoNo,PONo) == Array.IndexOf(subPodate, PODate))
                    {
                        isAvail = true;
                        break;
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                //isAvail = true;
            }
            return isAvail;
        }
        public static poheader getPONOAndDateOFPOOut(poheader pohTemp)
        {
            poheader poh = new poheader(); ;
            //List<pogeneralheader> WOHeaders = new List<pogeneralheader>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID,PONo,PODate from POHeader where" +
                    " DocumentID = '" + pohTemp.DocumentID + "'" +
                    " and TemporaryNo = " + pohTemp.TemporaryNo +
                    " and TemporaryDate = '" + pohTemp.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    poh.DocumentID = reader.GetString(0);
                    poh.PONo = reader.GetInt32(1);
                    poh.PODate = reader.GetDateTime(2);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in quering TempNo And Date of PO.");
            }
            return poh;
        }
    }
}
