using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace CSLERP.DBData
{
    class popiheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string ReferenceNo { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int TrackingNo { get; set; }
        public DateTime TrackingDate { get; set; }
        public string CustomerID { get; set; }
        public string ProjectID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPONO { get; set; }
        public DateTime CustomerPODate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ValidityDate { get; set; }
        public string PaymentTerms { get; set; }
        public string PaymentMode { get; set; }
        public string FreightTerms { get; set; }
        public double FreightCharge { get; set; }
        public string CurrencyID { get; set; }
        public decimal ExchangeRate { get; set; }
        
        public string BillingAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public double ProductValue { get; set; }
        public double TaxAmount { get; set; }
        public double POValue { get; set; }
        public double ProductValueINR { get; set; }
        public double TaxAmountINR { get; set; }
        public double POValueINR { get; set; }
        public string Remarks { get; set; }
        public int status { get; set; }
        public int DocumentStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string CreatorName { get; set; }
        public string ForwarderName { get; set; }
        public string ApproverName { get; set; }
        public string OfficeID { get; set; }
        public string CommentStatus { get; set; }
        public string Comments { get; set; }
        public string ForwarderList { get; set; }
        public double TotalValue { get; set; }

        public popiheader()
        {
            ReferenceNo = "";
            Comments = "";
        }
    }
    class popidetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public int invoiceno { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string CustomerItemDescription { get; set; }
        public string WorkDescription { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Tax { get; set; }
        public string TaxCode { get; set; }
        public int WarrantyDays { get; set; }
        public string TaxDetails { get; set; }
    }
    class POPIHeaderDB
    {
        public List<popiheader> getFilteredPOPIHeader(string userList, int opt, string userCommentStatusString)
        {
            popiheader popih;
            List<popiheader> POPIHeaders = new List<popiheader>();
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
                    " TrackingNo,TrackingDate,CustomerID,CustomerName,CustomerPONO,CustomerPODate,DeliveryDate,ValidityDate,PaymentTerms,ISNULL(PaymentMode,' ') as PaymentMode,ISNULL(FreightTerms,' ') as FreightTerms,ISNULL(FreightCharge,' ') as FreightCharge," +
                    " CurrencyID,BillingAddress,DeliveryAddress,ProductValue,TaxAmount,POValue,Remarks, " +
                    " status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,OfficeID,CommentStatus,ForwarderList,ReferenceNo,ProjectID " +
                    " ,ExchangeRate,ProductValueINR,TaxAmountINR,POValueINR from ViewPOProductInwardHeader" +
                    " where ((forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (commentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query2 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " TrackingNo,TrackingDate,CustomerID,CustomerName,CustomerPONO,CustomerPODate,DeliveryDate,ValidityDate,PaymentTerms,ISNULL(PaymentMode,' ') as PaymentMode,ISNULL(FreightTerms,' ') as FreightTerms,ISNULL(FreightCharge,' ') as FreightCharge," +
                    " CurrencyID,BillingAddress,DeliveryAddress,ProductValue,TaxAmount,POValue,Remarks, " +
                    " status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,OfficeID,CommentStatus,ForwarderList,ReferenceNo,ProjectID " +
                    " ,ExchangeRate,ProductValueINR,TaxAmountINR,POValueINR from ViewPOProductInwardHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query3 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " TrackingNo,TrackingDate,CustomerID,CustomerName,CustomerPONO,CustomerPODate,DeliveryDate,ValidityDate,PaymentTerms,ISNULL(PaymentMode,' ') as PaymentMode,ISNULL(FreightTerms,' ') as FreightTerms,ISNULL(FreightCharge,' ') as FreightCharge," +
                    " CurrencyID,BillingAddress,DeliveryAddress,ProductValue,TaxAmount,POValue,Remarks, " +
                    " status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,OfficeID,CommentStatus,ForwarderList,ReferenceNo,ProjectID " +
                    " ,ExchangeRate,ProductValueINR,TaxAmountINR,POValueINR from ViewPOProductInwardHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99)  order by TrackingDate desc,DocumentID asc,TrackingNo desc";

                //for choice/selection list
                string query4 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " TrackingNo,TrackingDate,CustomerID,CustomerName,CustomerPONO,CustomerPODate,DeliveryDate,ValidityDate,PaymentTerms,ISNULL(PaymentMode,' ') as PaymentMode,ISNULL(FreightTerms,' ') as FreightTerms,ISNULL(FreightCharge,' ') as FreightCharge," +
                    " CurrencyID,BillingAddress,DeliveryAddress,ProductValue,TaxAmount,POValue,Remarks, " +
                    " status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,OfficeID,CommentStatus,ForwarderList,ReferenceNo,ProjectID " +
                   " ,ExchangeRate,ProductValueINR,TaxAmountINR,POValueINR from ViewPOProductInwardHeader" +
                   " where getdate() - trackingdate < 90 and TrackingNo > 0 and DocumentStatus = 99  ";

                string query5 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " TrackingNo,TrackingDate,CustomerID,CustomerName,CustomerPONO,CustomerPODate,DeliveryDate,ValidityDate,PaymentTerms,ISNULL(PaymentMode,' ') as PaymentMode,ISNULL(FreightTerms,' ') as FreightTerms,ISNULL(FreightCharge,' ') as FreightCharge," +
                    " CurrencyID,BillingAddress,DeliveryAddress,ProductValue,TaxAmount,POValue,Remarks, " +
                    " status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,OfficeID,CommentStatus,ForwarderList,ReferenceNo,ProjectID " +
                   " ,ExchangeRate,ProductValueINR,TaxAmountINR,POValueINR from ViewPOProductInwardHeader" +
                   " where  DocumentStatus = 99 ";
                //////and OfficeID in (" + userList + ")";

                string query6 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " TrackingNo,TrackingDate,CustomerID,CustomerName,CustomerPONO,CustomerPODate,DeliveryDate,ValidityDate,PaymentTerms,ISNULL(PaymentMode,' ') as PaymentMode,ISNULL(FreightTerms,' ') as FreightTerms,ISNULL(FreightCharge,' ') as FreightCharge," +
                    " CurrencyID,BillingAddress,DeliveryAddress,ProductValue,TaxAmount,POValue,Remarks, " +
                    " status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,OfficeID,CommentStatus,ForwarderList,ReferenceNo,ProjectID " +
                    " ,ExchangeRate,ProductValueINR,TaxAmountINR,POValueINR from ViewPOProductInwardHeader" +
                    " where  DocumentStatus = 99  order by TrackingDate desc,DocumentID asc,TrackingNo desc";

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
                    case 5:
                        query = query5;
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
                    popih = new popiheader();
                    popih.RowID = reader.GetInt32(0);
                    popih.DocumentID = reader.GetString(1);
                    popih.DocumentName = reader.GetString(2);
                    popih.TemporaryNo = reader.GetInt32(3);
                    popih.TemporaryDate = reader.GetDateTime(4);
                    popih.TrackingNo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        popih.TrackingDate = reader.GetDateTime(6);
                    }

                    popih.CustomerID = reader.GetString(7);
                    popih.CustomerName = reader.GetString(8);
                    popih.CustomerPONO = reader.GetString(9);
                    popih.CustomerPODate = reader.GetDateTime(10);
                    popih.DeliveryDate = reader.GetDateTime(11);
                    if (!reader.IsDBNull(12))
                    {
                        popih.ValidityDate = reader.GetDateTime(12);
                    }
                    //popih.ValidityDate = reader.GetDateTime(12);
                    popih.PaymentTerms = reader.GetString(13);
                    popih.PaymentMode = reader.GetString(14);
                    popih.FreightTerms = reader.GetString(15);
                    popih.FreightCharge = reader.GetDouble(16);
                    popih.CurrencyID = reader.GetString(17);
                    
                    popih.BillingAddress = reader.GetString(18);
                    popih.DeliveryAddress = reader.GetString(19);
                    popih.ProductValue = reader.GetDouble(20);
                    popih.TaxAmount = reader.GetDouble(21);
                    popih.POValue = reader.GetDouble(22);
                    popih.Remarks = reader.GetString(23);
                    popih.status = reader.GetInt32(24);
                    popih.DocumentStatus = reader.GetInt32(25);
                    popih.CreateTime = reader.GetDateTime(26);
                    popih.CreateUser = reader.GetString(27);
                    popih.ForwardUser = reader.GetString(28);
                    popih.ApproveUser = reader.GetString(29);
                    popih.CreatorName = reader.GetString(30);
                    popih.ForwarderName = reader.GetString(31);
                    popih.ApproverName = reader.GetString(32);
                    popih.OfficeID = reader.GetString(33);
                    if (!reader.IsDBNull(34))
                    {
                        popih.CommentStatus = reader.GetString(34);
                    }
                    else
                    {
                        popih.CommentStatus = "";
                    }
                    ////if (!reader.IsDBNull(35))
                    ////{
                    ////    popih.Comments = reader.GetString(35);
                    ////}
                    ////else
                    ////{
                    ////    popih.Comments = "";
                    ////}
                    if (!reader.IsDBNull(35))
                    {
                        popih.ForwarderList = reader.GetString(35);
                    }
                    else
                    {
                        popih.ForwarderList = "";
                    }
                    if (!reader.IsDBNull(36))
                    {
                        popih.ReferenceNo = reader.GetString(36);
                    }
                    else
                    {
                        popih.ReferenceNo = "";
                    }
                    popih.ProjectID = reader.IsDBNull(37)?"":reader.GetString(37);
                    popih.ExchangeRate = reader.GetDecimal(38);
                    popih.ProductValueINR = reader.GetDouble(39);
                    popih.TaxAmountINR = reader.GetDouble(40);
                    popih.POValueINR = reader.GetDouble(41);
                    POPIHeaders.Add(popih);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO Product Inward Header Details");
            }
            return POPIHeaders;
        }



        public static List<popidetail> getPOPIDetail(popiheader popih)
        {
            popidetail popid;
            List<popidetail> POPIDetail = new List<popidetail>();
            try
            {
                //string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,DocumentID,TemporaryNo, TemporaryDate,StockItemID,StockItemName,ModelNo, ISNULL(ModelName ,'') as ModelName," +
                   " ISNULL(CustomerItemDescription,' ') as CustomerItemDescription,WorkDescription, " +
                   "Quantity,Price,Tax,WarrantyDays,TaxDetails,TaxCode " +
                   "from ViewPOProductInwardDetail " +
                   " where DocumentID='" + popih.DocumentID + "'" +
                   " and TemporaryNo=" + popih.TemporaryNo +
                   " and TemporaryDate='" + popih.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                   " order by StockItemID";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popid = new popidetail();
                    popid.RowID = reader.GetInt32(0);
                    popid.DocumentID = reader.GetString(1);
                    popid.TemporaryNo = reader.GetInt32(2);
                    popid.TemporaryDate = reader.GetDateTime(3).Date;
                    popid.StockItemID = reader.GetString(4);
                    popid.StockItemName = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                    {
                        popid.ModelNo = reader.GetString(6);
                    }
                    else
                    {
                        popid.ModelNo = "NA";
                    }
                    if (reader.GetString(7).Length != 0)
                    {
                        popid.ModelName = reader.GetString(7);
                    }
                    else
                    {
                        popid.ModelName = "NA";
                    }
                   
                    popid.CustomerItemDescription = reader.GetString(8);
                    popid.WorkDescription = reader.GetString(9);
                    popid.Quantity = reader.GetDouble(10);
                    popid.Price = reader.GetDouble(11);
                    popid.Tax = reader.GetDouble(12);
                    popid.WarrantyDays = reader.GetInt32(13);
                    popid.TaxDetails = reader.GetString(14);
                    popid.TaxCode = reader.IsDBNull(15)?"":reader.GetString(15);
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

        public Boolean updatePOPIDetail(List<popidetail> POPIDetails, popiheader popih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Delete from POProductInwardDetail where DocumentID='" + popih.DocumentID + "'" +
                    " and TemporaryNo=" + popih.TemporaryNo +
                    " and TemporaryDate='" + popih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "POProductInwardDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (popidetail popid in POPIDetails)
                {
                    updateSQL = "insert into POProductInwardDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,TaxCode,CustomerItemDescription,WorkDescription,Quantity,Price,Tax,WarrantyDays,TaxDetails) " +
                    "values ('" + popih.DocumentID + "'," +
                    popih.TemporaryNo + "," +
                    "'" + popih.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + popid.StockItemID + "'," +
                    "'" + popid.ModelNo + "'," +
                     "'" + popid.TaxCode + "'," +
                    "'" + popid.CustomerItemDescription + "'," +
                    "'" + popid.WorkDescription + "'," +
                    popid.Quantity + "," +
                    popid.Price + "," +
                    popid.Tax + "," +
                    popid.WarrantyDays + "," +
                    "'" + popid.TaxDetails + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "POProductInwardDetail", "", updateSQL) +
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
            }
            return status;
        }

        public Boolean validatePOPIHeader(popiheader popih)
        {
            Boolean status = true;
            try
            {
                if (popih.DocumentID.Trim().Length == 0 || popih.DocumentID == null)
                {
                    return false;
                }

                if (popih.CustomerID.Trim().Length == 0 || popih.CustomerID == null)
                {
                    return false;
                }
                if (popih.CustomerPONO.Trim().Length == 0 || popih.CustomerPONO == null)
                {
                    return false;
                }
                if (popih.CustomerPODate == null)
                {
                    return false;
                }
                if (popih.DeliveryDate == null)
                {
                    return false;
                }
                if (popih.ValidityDate < popih.DeliveryDate || popih.ValidityDate == null)
                {
                    return false;
                }
                if (popih.FreightTerms == "Extra")
                {
                    if (popih.FreightCharge == 0)
                    {
                        return false;
                    }
                }
                if (popih.DeliveryDate < popih.CustomerPODate)
                {
                    return false;
                }
                if (popih.PaymentTerms == null || popih.PaymentTerms.Trim().Length == 0)
                {
                    return false;
                }
                if (popih.PaymentMode == null || popih.PaymentMode.Trim().Length == 0)
                {
                    return false;
                }
                ////if (popih.ProjectID.Trim().Length == 0 || popih.ProjectID == null)
                ////{
                ////    return false;
                ////}
                if (popih.CurrencyID.Trim().Length == 0 || popih.CurrencyID == null)
                {
                    return false;
                }
                if (popih.BillingAddress.Trim().Length == 0 || popih.BillingAddress == null)
                {
                    return false;
                }
                if (popih.DeliveryAddress.Trim().Length == 0 || popih.DeliveryAddress == null)
                {
                    return false;
                }
                if (popih.DeliveryAddress.Trim().Length == 0 || popih.DeliveryAddress == null)
                {
                    return false;
                }
                if (popih.ProductValue == 0)
                {
                    return false;
                }
                if (popih.ProductValueINR == 0)
                {
                    return false;
                }
                if (popih.POValue == 0)
                {
                    return false;
                }
                if (popih.POValueINR == 0)
                {
                    return false;
                }
                if (popih.ExchangeRate == 0)
                {
                    return false;
                }
                if (popih.Remarks.Trim().Length == 0 || popih.Remarks == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public Boolean forwardPOPI(popiheader poih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POProductInwardHeader set DocumentStatus=" + (poih.DocumentStatus + 1) +
                    ", forwardUser='" + poih.ForwardUser + "'" +
                    ", commentStatus='" + poih.CommentStatus + "'" +
                    ", ForwarderList='" + poih.ForwarderList + "'" +
                    " where DocumentID='" + poih.DocumentID + "'" +
                    " and TemporaryNo=" + poih.TemporaryNo +
                    " and TemporaryDate='" + poih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
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

        public Boolean reversePOPI(popiheader poih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POProductInwardHeader set DocumentStatus=" + poih.DocumentStatus +
                    ", forwardUser='" + poih.ForwardUser + "'" +
                    ", commentStatus='" + poih.CommentStatus + "'" +
                    ", ForwarderList='" + poih.ForwarderList + "'" +
                    " where DocumentID='" + poih.DocumentID + "'" +
                    " and TemporaryNo=" + poih.TemporaryNo +
                    " and TemporaryDate='" + poih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
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

        public Boolean ApprovePOPI(popiheader popih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POProductInwardHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + popih.CommentStatus + "'" +
                    ", TrackingNo=" + popih.TrackingNo +
                    ", TrackingDate=convert(date, getdate())" +
                    " where DocumentID='" + popih.DocumentID + "'" +
                    " and TemporaryNo=" + popih.TemporaryNo +
                    " and TemporaryDate='" + popih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
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
        public static ListView TrackingSelectionView(string docID)
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
                POPIHeaderDB popihdb = new POPIHeaderDB();
                List<popiheader> POPIHeaders = popihdb.getFilteredPOPIHeader("", 4, "");
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Tr No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Tr Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Cust PO", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO Date", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Cust Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Prod Value", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Tax Amt", -2, HorizontalAlignment.Center);

                foreach (popiheader popih in POPIHeaders)
                {
                    if (popih.DocumentID.Equals(docID))
                    {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        item1.SubItems.Add(popih.TrackingNo.ToString()); 
                        item1.SubItems.Add(popih.TrackingDate.ToShortDateString());//ToString("dd-MM-yyyy"));
                        item1.SubItems.Add(popih.CustomerPONO);
                        item1.SubItems.Add(popih.CustomerPODate.ToShortDateString());//.ToString("dd-MM-yyyy"));
                        item1.SubItems.Add(popih.CustomerName);
                        item1.SubItems.Add(popih.ProductValue.ToString());
                        item1.SubItems.Add(popih.TaxAmount.ToString());
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
        public static string getUserComments(string docid, int tempno, DateTime tempdate)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from POProductInwardHeader where DocumentID='" + docid + "'" +
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
        public static string getTemporaryNOFromPONo(int POTrackNo, DateTime POTrackdate,string DociD)
        {
            string tempString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID, TemporaryNo, TemporaryDate from POProductInwardHeader where TrackingNo='" + POTrackNo + "'" +
                        " and TrackingDate='" + POTrackdate.ToString("yyyy-MM-dd") + "' and DocumentID = '" + DociD + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    tempString = reader.GetString(0);
                    tempString = reader.GetInt32(1).ToString();
                    tempString = reader.GetDateTime(2).ToString();
                    tempString = reader.GetString(0) + ";" + reader.GetInt32(1).ToString() + ";" + reader.GetDateTime(2).ToString();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return tempString;
        }
        public static ListView SelectPODetailForInvoiceOut(string docID, string custId)
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
                string DocID = "";
                switch (docID)
                {
                    case "PRODUCTEXPORTINVOICEOUT":
                        DocID = "POPRODUCTINWARD";
                        break;
                    case "SERVICEEXPORTINVOICEOUT":
                        DocID = "POSERVICEINWARD";
                        break;
                    case "PRODUCTINVOICEOUT":
                        DocID = "POPRODUCTINWARD";
                        break;
                    case "SERVICEINVOICEOUT":
                        DocID = "POSERVICEINWARD";
                        break;
                    default:
                        DocID = "";
                        break;
                }
                POPIHeaderDB popihdb = new POPIHeaderDB();
                List<popiheader> POPIHeaders = popihdb.getFilteredPOPIHeader("", 4, "");
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Tr No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Tr Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Cust PO", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO Date", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Cust Name", -2, HorizontalAlignment.Center);
                //lv.Columns.Add("Prod Value", -2, HorizontalAlignment.Center);
                //lv.Columns.Add("Tax Amt", -2, HorizontalAlignment.Center);

                foreach (popiheader popih in POPIHeaders)
                {
                    if (popih.DocumentID.Equals(DocID) && popih.CustomerID.Equals(custId))
                    {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        item1.SubItems.Add(popih.TrackingNo.ToString());
                        item1.SubItems.Add(popih.TrackingDate.ToString("yyyy-MM-dd"));//ToShortDateString());
                        item1.SubItems.Add(popih.CustomerPONO);
                        item1.SubItems.Add(popih.CustomerPODate.ToShortDateString());
                        item1.SubItems.Add(popih.CustomerName);
                        //item1.SubItems.Add(popih.ProductValue.ToString());
                        //item1.SubItems.Add(popih.TaxAmount.ToString());
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
        public static ListView getPONoWiseStockListView(int POTrackNo, DateTime POTrackDate,string docID)
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
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("RefNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("TempNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("TempDate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PONo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PODate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItemID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("stockItemName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ModelNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ModelName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Cust.ItemDescription", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Quantity", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Price", -2, HorizontalAlignment.Left);
                lv.Columns.Add("WarrentyDays", -2, HorizontalAlignment.Left);
                lv.Columns.Add("TaxCode", -2, HorizontalAlignment.Left);
                string docid = "";
                switch (docID)
                {
                    case "PRODUCTEXPORTINVOICEOUT":
                        docid = "POPRODUCTINWARD";
                        break;
                    case "SERVICEEXPORTINVOICEOUT":
                        docid = "POSERVICEINWARD";
                        break;
                    case "PRODUCTINVOICEOUT":
                        docid = "POPRODUCTINWARD";
                        break;
                    case "SERVICEINVOICEOUT":
                        docid = "POSERVICEINWARD";
                        break;
                    default:
                        docid = "";
                        break;
                }
                POPIHeaderDB popihdb = new POPIHeaderDB();
                string tempString = POPIHeaderDB.getTemporaryNOFromPONo(POTrackNo, POTrackDate,docid);
                string[] PODetArr = tempString.Split(';');
                popiheader popih = new popiheader();
                popih.DocumentID = PODetArr[0];
                popih.TemporaryNo = Convert.ToInt32(PODetArr[1]);
                popih.TemporaryDate = Convert.ToDateTime(PODetArr[2]);
                List<popidetail> POPIDetails = POPIHeaderDB.getPOPIDetail(popih);
                foreach (popidetail popid in POPIDetails)
                {
                    //if (popid.DocumentID == docid)
                    //{
                        ListViewItem item = new ListViewItem();
                        item.Checked = false;
                        item.SubItems.Add(popid.RowID.ToString());
                        item.SubItems.Add(popid.TemporaryNo.ToString());
                        item.SubItems.Add(popid.TemporaryDate.ToShortDateString());
                        item.SubItems.Add(POTrackNo.ToString());
                        item.SubItems.Add(POTrackDate.ToShortDateString());
                        item.SubItems.Add(popid.StockItemID.ToString());
                        item.SubItems.Add(popid.StockItemName.ToString());
                        item.SubItems.Add(popid.ModelNo.ToString());
                        item.SubItems.Add(popid.ModelName.ToString());
                        item.SubItems.Add(popid.CustomerItemDescription.ToString());
                        item.SubItems.Add(popid.Quantity.ToString());
                        item.SubItems.Add(popid.Price.ToString());
                        item.SubItems.Add(popid.WarrantyDays.ToString());
                        item.SubItems.Add(popid.TaxCode.ToString());

                        lv.Items.Add(item);
                   // }
                }

            }
            catch (Exception ex)
            {

            }
            return lv;
        }
        public static Boolean checkAvailableQuantityForInvoiceOut(int RefNo, double Qunt)
        {
            Boolean status = true;
           // int AvlQuant = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select Quantity from POProductInwardDetail where RowID=" + RefNo ;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                   if(reader.GetDouble(0) < Qunt)
                    {
                        return false;
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        //   Codes for ReportPOAnalysis
        public List<popiheader> getDetailForpartWise(int opt1, int opt2, DateTime fromDate, DateTime toDate)
        {
            // opt1 : PO Wise
            // opt2 : SeviceWise
            // opt3 : PartyWise
            // opt4 : RegionWIse
            popiheader popih;
            List<popiheader> POPIHeaders = new List<popiheader>();
            try
            {
                string query = "";
                if (opt1 == 1 && opt2 == 1)
                {
                    query = "select DocumentID,CustomerID,SUM(ProductValue) from POProductInwardHeader" +
                        " where TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and TrackingDate > '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and DocumentStatus = 99 " +
                        "group by DocumentID,CustomerID";
                }
                else if (opt1 == 1)
                {
                    query = "select DocumentID,CustomerID,SUM(ProductValue) from POProductInwardHeader" +
                        " where DocumentID = 'POPRODUCTINWARD'" +
                         " and TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and TrackingDate > '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and DocumentStatus = 99 " +
                        " group by DocumentID,CustomerID ";
                }
                else
                {
                    query = "select DocumentID,CustomerID,SUM(ProductValue) from POProductInwardHeader" +
                        " where DocumentID = 'POSERVICEINWARD' and " +
                         " TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and TrackingDate > '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and DocumentStatus = 99 " +
                        "group by DocumentID, CustomerID ";
                }

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popih = new popiheader();
                    popih.DocumentID = reader.GetString(0);
                    popih.CustomerID = reader.GetString(1);
                    popih.POValue = reader.GetDouble(2);
                    POPIHeaders.Add(popih);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO Product Inward Header Details");
            }
            return POPIHeaders;
        }
        public List<popiheader> getDetailForRegionWise(int opt1, int opt2, DateTime fromDate, DateTime toDate)
        {
            // opt1 : PO Wise
            // opt2 : SeviceWise
            // opt3 : PartyWise
            // opt4 : RegionWIse
            popiheader popih;
            List<popiheader> POPIHeaders = new List<popiheader>();
            try
            {
                string query = "";
                if (opt1 == 1 && opt2 == 1)
                {
                    query = "select a.DocumentID,c.RegionID,d.Name, sum(a.ProductValue) " +
                        "from POProductInwardHeader a,Customer b, Office c, Region d "+
                        "where a.CustomerID = b.CustomerID and b.OfficeID = c.OfficeID and c.RegionID = d.RegionID and" +
                         " a.TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and a.TrackingDate > '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and a.DocumentStatus = 99 " +
                        " group by a.DocumentID,c.RegionID, d.Name";
                }
                else if (opt1 == 1)
                {
                    query = "select a.DocumentID,c.RegionID,d.Name, sum(a.ProductValue) " +
                        "from POProductInwardHeader a,Customer b, Office c, Region d " +
                        "where a.CustomerID = b.CustomerID and b.OfficeID = c.OfficeID and c.RegionID = d.RegionID and DocumentID = 'POPRODUCTINWARD'" +
                         " and a.TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and a.TrackingDate > '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and a.DocumentStatus = 99 " +
                        " group by a.DocumentID,c.RegionID, d.Name";
                }
                else
                {
                    query = "select a.DocumentID,c.RegionID,d.Name, sum(a.ProductValue) " +
                        "from POProductInwardHeader a,Customer b, Office c, Region d " +
                        "where a.CustomerID = b.CustomerID and b.OfficeID = c.OfficeID and c.RegionID = d.RegionID and DocumentID = 'POSERVICEINWARD'" +
                         " and a.TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and a.TrackingDate > '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and a.DocumentStatus = 99 " +
                        " group by a.DocumentID,c.RegionID, d.Name";
                }

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popih = new popiheader();
                    popih.DocumentID = reader.GetString(0);
                    popih.OfficeID = reader.GetString(1);  // as region ID
                    popih.Remarks = reader.GetString(2);  // as region name
                    popih.POValue = reader.GetDouble(3);
                    POPIHeaders.Add(popih);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO Product Inward Header Details");
            }
            return POPIHeaders;
        }
        public List<popiheader> getDetailForDocumentWise(int opt1, int opt2, DateTime fromDate, DateTime toDate)
        {
            // opt1 : PO Wise
            // opt2 : SeviceWise
            // opt3 : PartyWise
            // opt4 : RegionWIse
            popiheader popih;
            List<popiheader> POPIHeaders = new List<popiheader>();
            try
            {
                string query = "";
                if (opt1 == 1 && opt2 == 1)
                {
                    query = "select DocumentID,sum(ProductValue) from POProductInwardHeader" +
                         " where TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and TrackingDate > '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and DocumentStatus = 99 " +
                        " group by DocumentID ";
                }
                else if (opt1 == 1)
                {
                    query = "select DocumentID,sum(ProductValue) from POProductInwardHeader" +
                        " where DocumentID = 'POPRODUCTINWARD' " +
                        " and TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and TrackingDate > '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and DocumentStatus = 99 " +
                        "group by DocumentID ";
                }
                else
                {
                    query = "select DocumentID,sum(ProductValue) from POProductInwardHeader" +
                        " where DocumentID = 'POSERVICEINWARD' " +
                        " and TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and TrackingDate > '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and DocumentStatus = 99 " +
                        "group by DocumentID ";
                }

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popih = new popiheader();
                    popih.DocumentID = reader.GetString(0);
                    popih.POValue = reader.GetDouble(1);
                    POPIHeaders.Add(popih);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO Product Inward Header Details");
            }
            return POPIHeaders;
        }

        public static string getPOPIDtlsForProjectTrans(string projectID)
        {
            string str = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select COUNT(*), SUM(POValue) from POProductInwardHeader where ProjectID = '" + projectID + "' and DocumentStatus = 99";
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
        public static List<popiheader> getPOPIINFOForProjectTrans(string projectID)
        {
            popiheader popih = new popiheader();
            List<popiheader> POPIHeaders = new List<popiheader>();
            string str = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select TrackingNo, TrackingDate,CustomerName,ProductValue,TaxAmount,POValue,CustomerPONO,CustomerPODate from ViewPOProductInwardHeader " +
                    " where ProjectID = '" + projectID + "' and DocumentStatus = 99";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popih = new popiheader();
                    popih.TrackingNo = reader.GetInt32(0);
                    popih.TrackingDate = reader.GetDateTime(1);
                    popih.CustomerName = reader.GetString(2);
                    popih.ProductValue = reader.GetDouble(3);
                    popih.TaxAmount = reader.GetDouble(4);
                    popih.POValue = reader.GetDouble(5);
                    popih.CustomerPONO = reader.GetString(6);
                    popih.CustomerPODate = reader.GetDateTime(7);
                    POPIHeaders.Add(popih);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return POPIHeaders;
        }

        public Boolean updatePOPIHeaderAndDetail(popiheader popih, popiheader prevpopi, List<popidetail> POPIDetails)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POProductInwardHeader set CustomerID='" + popih.CustomerID +
                   "',CustomerPONO='" + popih.CustomerPONO +
                    "',CustomerPODate='" + popih.CustomerPODate.ToString("yyyy-MM-dd") +
                   "', DeliveryDate='" + popih.DeliveryDate.ToString("yyyy-MM-dd") +
                     "', ValidityDate='" + popih.ValidityDate.ToString("yyyy-MM-dd") +
                   "', PaymentTerms='" + popih.PaymentTerms +
                   "', PaymentMode='" + popih.PaymentMode +
                   "', FreightTerms ='" + popih.FreightTerms +
                   "', FreightCharge=" + popih.FreightCharge +
                   ", CurrencyID='" + popih.CurrencyID +
                     "', ExchangeRate=" + popih.ExchangeRate +
                    ", ProjectID='" + popih.ProjectID +
                   "', BillingAddress='" + popih.BillingAddress +
                   "', DeliveryAddress='" + popih.DeliveryAddress +
                   "', ProductValue=" + popih.ProductValue +
                   ", TaxAmount=" + popih.TaxAmount +
                   ", POValue=" + popih.POValue +
                      ", ProductValueINR=" + popih.ProductValueINR +
                   ", TaxAmountINR=" + popih.TaxAmountINR +
                   ", POValueINR=" + popih.POValueINR +
                   ", Remarks='" + popih.Remarks +
                   "', CommentStatus='" + popih.CommentStatus +
                   "', Comments='" + popih.Comments +
                   "', ForwarderList='" + popih.ForwarderList +
                   "', ReferenceNo='" + popih.ReferenceNo +
                   "' where DocumentID='" + prevpopi.DocumentID + "'" +
                   " and TemporaryNo=" + prevpopi.TemporaryNo +
                   " and TemporaryDate='" + prevpopi.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POProductInwardHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from POProductInwardDetail where DocumentID='" + prevpopi.DocumentID + "'" +
                    " and TemporaryNo=" + prevpopi.TemporaryNo +
                    " and TemporaryDate='" + prevpopi.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "POProductInwardDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (popidetail popid in POPIDetails)
                {
                    updateSQL = "insert into POProductInwardDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,TaxCode,CustomerItemDescription,WorkDescription,Quantity,Price,Tax,WarrantyDays,TaxDetails) " +
                    "values ('" + popih.DocumentID + "'," +
                    popih.TemporaryNo + "," +
                    "'" + popih.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + popid.StockItemID + "'," +
                    "'" + popid.ModelNo + "'," +
                    "'" + popid.TaxCode + "'," +
                    "'" + popid.CustomerItemDescription + "'," +
                    "'" + popid.WorkDescription + "'," +
                    popid.Quantity + "," +
                    popid.Price + "," +
                    popid.Tax + "," +
                    popid.WarrantyDays + "," +
                    "'" + popid.TaxDetails + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "POProductInwardDetail", "", updateSQL) +
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
        public Boolean InsertPOPIHeaderAndDetail(popiheader popih, List<popidetail> POPIDetails)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                popih.TemporaryNo = DocumentNumberDB.getNumber(popih.DocumentID, 1);
                if (popih.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + popih.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + popih.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into POProductInwardHeader " +
                     "(DocumentID,ReferenceNo,TemporaryNo,TemporaryDate,TrackingNo,TrackingDate,CustomerID,CustomerPONo,CustomerPODate," +
                     "DeliveryDate,ValidityDate,PaymentTerms,PaymentMode,FreightTerms,FreightCharge,CurrencyID,ExchangeRate,ProjectID,BillingAddress,DeliveryAddress,ProductValue,ProductValueINR,TaxAmount,TaxAmountINR," +
                     "POValue,POValueINR,Remarks,CommentStatus,Comments,ForwarderList,Status,DocumentStatus,CreateTime,CreateUser)" +
                     " values (" +
                     "'" + popih.DocumentID + "'," +
                     "'" + popih.ReferenceNo + "'," +
                     popih.TemporaryNo + "," +
                     "'" + popih.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     popih.TrackingNo + "," +
                     "'" + popih.TrackingDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + popih.CustomerID + "'," +
                     "'" + popih.CustomerPONO + "'," +
                     "'" + popih.CustomerPODate.ToString("yyyy-MM-dd") + "'," +
                     "'" + popih.DeliveryDate.ToString("yyyy-MM-dd") + "'," +
                      "'" + popih.ValidityDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + popih.PaymentTerms + "'," +
                     "'" + popih.PaymentMode + "'," +
                     "'" + popih.FreightTerms + "'," +
                     popih.FreightCharge + "," +
                     "'" + popih.CurrencyID + "'," +
                      popih.ExchangeRate + "," +
                     "'" + popih.ProjectID + "'," +
                     "'" + popih.BillingAddress + "'," +
                     "'" + popih.DeliveryAddress + "'," +
                     popih.ProductValue + "," +
                     popih.ProductValueINR + "," +
                     popih.TaxAmount + "," +
                     popih.TaxAmountINR + "," +
                     popih.POValue + "," +
                     popih.POValueINR + "," +
                     "'" + popih.Remarks + "'," +
                     "'" + popih.CommentStatus + "'," +
                     "'" + popih.Comments + "'," +
                     "'" + popih.ForwarderList + "'," +
                     popih.status + "," +
                     popih.DocumentStatus + "," +
                      "GETDATE()" + "," +
                     "'" + Login.userLoggedIn + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "POProductInwardHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from POProductInwardDetail where DocumentID='" + popih.DocumentID + "'" +
                    " and TemporaryNo=" + popih.TemporaryNo +
                    " and TemporaryDate='" + popih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "POProductInwardDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (popidetail popid in POPIDetails)
                {
                    updateSQL = "insert into POProductInwardDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,TaxCode,CustomerItemDescription,WorkDescription,Quantity,Price,Tax,WarrantyDays,TaxDetails) " +
                    "values ('" + popih.DocumentID + "'," +
                    popih.TemporaryNo + "," +
                    "'" + popih.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + popid.StockItemID + "'," +
                    "'" + popid.ModelNo + "'," +
                    "'" + popid.TaxCode + "'," +
                    "'" + popid.CustomerItemDescription + "'," +
                    "'" + popid.WorkDescription + "'," +
                    popid.Quantity + "," +
                    popid.Price + "," +
                    popid.Tax + "," +
                    popid.WarrantyDays + "," +
                    "'" + popid.TaxDetails + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "POProductInwardDetail", "", updateSQL) +
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
        public static double getPOQuantityForInvoiceOut(int TrackNo, DateTime TrackDate, string StockId, string ModelNo,string docID)
        {
            double Qunt = 0;
            try
            {
                string query = "select b.Quantity from POProductInwardHeader a, POProductInwardDetail b " +
                    "where a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate " +
                    "and b.TemporaryNo = (select TemporaryNo from POProductInwardHeader where documentid='"+docID+"'"+" and TrackingNo = " + TrackNo + " and TrackingDate = '" + TrackDate.ToString("yyyy-MM-dd") + "')" +
                    " and b.TemporaryDate = (select TemporaryDate from POProductInwardHeader where documentid='" + docID + "'" + " and  TrackingNo = " + TrackNo + " and TrackingDate = '" + TrackDate.ToString("yyyy-MM-dd") + "')" +
                    " and b.StockItemID = '" + StockId + "' and b.ModelNo = '" + ModelNo + "'";
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
        public static string getCustomerPOAndDateForInvoiceOut(int TrackNo, DateTime TrackDate,string docID)
        {
            string poNo = "";
            DateTime PODate = DateTime.Parse("1900-01-01");
            try
            {
                string query = "select CustomerPONo,CustomerPODate from POProductInwardHeader  " +
                    "where documentID =  '" + docID + "'"+
                    " and TrackingNo =  " + TrackNo + 
                    " and TrackingDate = '"+ TrackDate.ToString("yyyy-MM-dd") +"'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    poNo = reader.GetString(0);
                    PODate = reader.GetDateTime(1);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return poNo.ToString() + ";" + PODate.ToString("yyyy-MM-dd");
        }

        /*-------------POPI Report--------------------------------*/

        public List<popiheader> ListPopiFilters(string QueryData)
        {
            popiheader popih;
            List<popiheader> POPIHeadersReport = new List<popiheader>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = QueryData;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popih = new popiheader();
                    popih.RowID = reader.GetInt32(0);
                    popih.DocumentID = reader.GetString(1);
                    popih.TemporaryNo = reader.GetInt32(2);
                    popih.TemporaryDate = reader.GetDateTime(3);
                    popih.TrackingNo = reader.GetInt32(4);
                    if (!reader.IsDBNull(5))
                    {
                        popih.TrackingDate = reader.GetDateTime(5);
                    }
                    popih.CustomerID = reader.GetString(6);
                    popih.CustomerPONO = reader.GetString(7);
                    popih.CustomerPODate = reader.GetDateTime(8);
                    popih.DeliveryDate = reader.GetDateTime(9);
                    if (!reader.IsDBNull(10))
                    {
                        popih.ValidityDate = reader.GetDateTime(10);
                    }
                    popih.PaymentTerms = reader.GetString(11);
                    popih.PaymentMode = reader.GetString(12);
                    popih.FreightTerms = reader.GetString(13);
                    popih.FreightCharge = reader.GetDouble(14);
                    popih.CurrencyID = reader.GetString(15);
                    popih.BillingAddress = reader.GetString(16);
                    popih.DeliveryAddress = reader.GetString(17);
                    popih.ProductValue = reader.GetDouble(18);
                    popih.TaxAmount = reader.GetDouble(19);
                    popih.POValue = reader.GetDouble(20);
                    popih.Remarks = reader.GetString(21);
                    popih.status = reader.GetInt32(22);
                    popih.DocumentStatus = reader.GetInt32(23);
                    popih.CreateTime = reader.GetDateTime(24);
                    popih.CreateUser = reader.GetString(25);
                    popih.ForwardUser = reader.IsDBNull(26) ? "" : reader.GetString(26);
                    popih.ApproveUser = reader.IsDBNull(27) ? "" : reader.GetString(27);
                    if (!reader.IsDBNull(28))
                    {
                        popih.CommentStatus = reader.GetString(28);
                    }
                    else
                    {
                        popih.CommentStatus = "";
                    }
                    if (!reader.IsDBNull(29))
                    {
                        popih.ForwarderList = reader.GetString(29);
                    }
                    else
                    {
                        popih.ForwarderList = "";
                    }
                    if (!reader.IsDBNull(30))
                    {
                        popih.ReferenceNo = reader.GetString(30);
                    }
                    else
                    {
                        popih.ReferenceNo = "";
                    }
                    popih.ProjectID = reader.IsDBNull(31) ? "" : reader.GetString(31);
                    popih.ExchangeRate = reader.GetDecimal(32);
                    popih.ProductValueINR = reader.GetDouble(33);
                    popih.TaxAmountINR = reader.GetDouble(34);
                    popih.POValueINR = reader.GetDouble(35);
                    popih.TotalValue = reader.IsDBNull(36) ? 0 : reader.GetDouble(36);
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

        public static List<popidetail> getPOPOutDetailqty(popiheader popih)
        {
            popidetail popid;
            List<popidetail> POPIDetail = new List<popidetail>();
            try
            {
                //string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.RowID,a.DocumentID,a.TemporaryNo, a.TemporaryDate,b.InvoiceNo,b.InvoiceDate,a.StockItemID,a.StockItemName, " +
"a.ModelNo, ISNULL(a.ModelName ,'') as ModelName, ISNULL(a.CustomerItemDescription,' ') as CustomerItemDescription, " +
 "a.Quantity,a.Price,a.Tax,a.WarrantyDays,a.TaxDetails,a.TaxCode from ViewInvoiceOutDetail a,ViewInvoiceOutHeader b " +
 "where a.TemporaryNo=b.TemporaryNo and a.TemporaryNo='" + popih.TemporaryNo + "' and a.DocumentID in ('PRODUCTINVOICEOUT','PRODUCTEXPORTINVOICEOUT') " +
   "and a.TemporaryDate='" + popih.TemporaryDate.ToString() + "' and a.DocumentID=b.DocumentID order by a.TemporaryNo";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popid = new popidetail();
                    popid.RowID = reader.GetInt32(0);
                    popid.DocumentID = reader.GetString(1);
                    popid.TemporaryNo = reader.GetInt32(2);
                    popid.TemporaryDate = reader.GetDateTime(3).Date;
                    popid.invoiceno = reader.GetInt32(4);
                    popid.InvoiceDate = reader.GetDateTime(5).Date;
                    popid.StockItemID = reader.GetString(6);
                    popid.StockItemName = reader.GetString(7);
                    if (!reader.IsDBNull(8))
                    {
                        popid.ModelNo = reader.GetString(8);
                    }
                    else
                    {
                        popid.ModelNo = "NA";
                    }
                    if (reader.GetString(9).Length != 0)
                    {
                        popid.ModelName = reader.GetString(9);
                    }
                    else
                    {
                        popid.ModelName = "NA";
                    }
                    popid.CustomerItemDescription = reader.GetString(10);
                    popid.Quantity = reader.GetDouble(11);
                    popid.Price = reader.GetDouble(12);
                    popid.Tax = reader.GetDouble(13);
                    popid.WarrantyDays = reader.GetInt32(14);
                    popid.TaxDetails = reader.GetString(15);
                    popid.TaxCode = reader.IsDBNull(16) ? "" : reader.GetString(16);
                    POPIDetail.Add(popid);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying PO Product Out Details");
            }
            return POPIDetail;
        }

        public static List<popidetail> getPOPIDetailqty(popiheader popih)
        {
            popidetail popid;
            List<popidetail> POPIDetail = new List<popidetail>();
            try
            {
                //string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,DocumentID,TemporaryNo, TemporaryDate,StockItemID,StockItemName,ModelNo, ISNULL(ModelName ,'') as ModelName," +
                   " ISNULL(CustomerItemDescription,' ') as CustomerItemDescription,WorkDescription, " +
                   "Quantity,Price,Tax,WarrantyDays,TaxDetails,TaxCode " +
                   "from ViewPOProductInwardDetail " +
                   " where TemporaryNo='" + popih.TemporaryNo + "'" +
                    " and DocumentID='" + popih.DocumentID + "'" +
                   " and TemporaryDate='" + popih.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                   " order by StockItemID";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popid = new popidetail();
                    popid.RowID = reader.GetInt32(0);
                    popid.DocumentID = reader.GetString(1);
                    popid.TemporaryNo = reader.GetInt32(2);
                    popid.TemporaryDate = reader.GetDateTime(3).Date;
                    popid.StockItemID = reader.GetString(4);
                    popid.StockItemName = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                    {
                        popid.ModelNo = reader.GetString(6);
                    }
                    else
                    {
                        popid.ModelNo = "NA";
                    }
                    if (reader.GetString(7).Length != 0)
                    {
                        popid.ModelName = reader.GetString(7);
                    }
                    else
                    {
                        popid.ModelName = "NA";
                    }

                    popid.CustomerItemDescription = reader.GetString(8);
                    popid.WorkDescription = reader.GetString(9);
                    popid.Quantity = reader.GetDouble(10);
                    popid.Price = reader.GetDouble(11);
                    popid.Tax = reader.GetDouble(12);
                    popid.WarrantyDays = reader.GetInt32(13);
                    popid.TaxDetails = reader.GetString(14);
                    popid.TaxCode = reader.IsDBNull(15) ? "" : reader.GetString(15);
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
    }
}
