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
        public string POOfficeID { get; set; }
        public string POOfficeName { get; set; }
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
        public int ClosingStatus { get; set; }
        public int DocumentStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string CreatorName { get; set; }
        public string ForwarderName { get; set; }
        public string ApproverName { get; set; }
        public string OfficeID { get; set; }
        public string OfficeName { get; set; }
        public string CommentStatus { get; set; }
        public string Comments { get; set; }
        public string ForwarderList { get; set; }
        public string AmendmentDetails { get; set; }
        public double TotalValue { get; set; }
        public int invoiceno { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int DtlsID { get; set; }
        public double BlncAmount { get; set; }
        public string InvoiceDate2 { get; set; }
        public string invoiceno2 { get; set; }
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
        public string Location { get; set; }
        public string CustomerItemDescription { get; set; }
        public string WorkDescription { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Tax { get; set; }
        public string TaxCode { get; set; }
        public int WarrantyDays { get; set; }
        public string TaxDetails { get; set; }
        public string TrackingNo { get; set; }
        public string TrackingDate { get; set; }
    }
    class POPIHeaderDB
    {
        public List<popiheader> getFilteredPOPIHeader(string userList, int opt, string userCommentStatusString, string docrecvStr)
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
                //Query String
                string query = "";
                //THis is column String For retriving from table
                string columnsString = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " TrackingNo,TrackingDate,CustomerID,CustomerName,CustomerPONO,CustomerPODate,DeliveryDate,ValidityDate,PaymentTerms,ISNULL(PaymentMode,' ') as PaymentMode,ISNULL(FreightTerms,' ') as FreightTerms,ISNULL(FreightCharge,' ') as FreightCharge," +
                    " CurrencyID,BillingAddress,DeliveryAddress,ProductValue,TaxAmount,POValue,Remarks, " +
                    " status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,OfficeID,CommentStatus,ForwarderList,ReferenceNo,ProjectID " +
                    " ,ExchangeRate,ProductValueINR,TaxAmountINR,POValueINR,OfficeName,ClosingStatus,AmendmentDetails from ViewPOProductInwardHeader where ";
                //Doc Receiver list String
                string docRcvQry = "(" + docrecvStr + ")" + " and ";

                //Condition strings For query
                string condition = "";
                string cond1 = "  ((forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) and Status not in (98,7) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (commentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string cond2 = " ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) and Status not in (98,7) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string cond3 = " ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99 and status = 1)  order by TrackingDate desc,DocumentID asc,TrackingNo desc";
                //for choice/selection list
                string cond4 = " status = 1 and DocumentStatus = 99 order by TrackingDate desc,DocumentID asc,TrackingNo desc ";
                string cond6 = " status = 1 and DocumentStatus = 99 order by TrackingDate desc,DocumentID asc,TrackingNo desc";

                switch (opt)
                {
                    case 1:
                        condition = cond1;
                        break;
                    case 2:
                        condition = cond2;
                        break;
                    case 3:
                        condition = cond3;
                        break;
                    case 4:
                        condition = cond4;
                        break;
                    case 6:
                        condition = cond6;
                        break;
                    default:
                        condition = "";
                        break;
                }
                //Prepare main QueryString
                if (docrecvStr.Length != 0)
                    query = columnsString + docRcvQry + condition;
                else
                    query = columnsString + condition;

                SqlConnection conn = new SqlConnection(Login.connString);
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
                    popih.OfficeID = reader.IsDBNull(33) ? "" : reader.GetString(33);
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
                    popih.ProjectID = reader.IsDBNull(37) ? "" : reader.GetString(37);
                    popih.ExchangeRate = reader.GetDecimal(38);
                    popih.ProductValueINR = reader.GetDouble(39);
                    popih.TaxAmountINR = reader.GetDouble(40);
                    popih.POValueINR = reader.GetDouble(41);
                    popih.OfficeName = reader.IsDBNull(42) ? "" : reader.GetString(42);
                    popih.ClosingStatus = reader.IsDBNull(43) ? 0 : reader.GetInt32(43);
                    popih.AmendmentDetails = reader.IsDBNull(44) ? "" : reader.GetString(44);
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
                   "Quantity,Price,Tax,WarrantyDays,TaxDetails,TaxCode,Location " +
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
                    popid.StockItemName = reader.IsDBNull(5) ? "" : reader.GetString(5);
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
                    popid.Location = reader.IsDBNull(16) ? "" : reader.GetString(16);
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
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,Location,ModelNo,TaxCode,CustomerItemDescription,WorkDescription,Quantity,Price,Tax,WarrantyDays,TaxDetails) " +
                    "values ('" + popih.DocumentID + "'," +
                    popih.TemporaryNo + "," +
                    "'" + popih.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + popid.StockItemID + "'," +
                     "'" + popid.Location + "'," +
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
                if (popih.OfficeID.Trim().Length == 0 || popih.OfficeID == null)
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
        public Boolean ClosePOPI(popiheader popih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POProductInwardHeader set status=7 " +
                    " where DocumentID='" + popih.DocumentID + "'" +
                    " and TrackingNo=" + popih.TrackingNo +
                    " and TrackingDate='" + popih.TrackingDate.ToString("yyyy-MM-dd") + "'";
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
                List<popiheader> POPIHeaders = new List<popiheader>();
                List<popiheader> POPIHeadersTemp = new List<popiheader>();
                POPIHeadersTemp = popihdb.getFilteredPOPIHeader("", 4, "", "");
                POPIHeaders = POPIHeadersTemp.OrderBy(popi => popi.TrackingNo).ThenBy(popi => popi.TrackingDate).ToList();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Tr No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Tr Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Cust PO", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO Date", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Cust ID", -2, HorizontalAlignment.Center);
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
                        item1.SubItems.Add(popih.CustomerID);
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
        public static string getTemporaryNOFromPONo(int POTrackNo, DateTime POTrackdate, string poDociD)
        {
            string tempString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID, TemporaryNo, TemporaryDate from POProductInwardHeader" +
                    " where DocumentID = '" + poDociD + "' and TrackingNo = " + POTrackNo +
                    " and TrackingDate='" + POTrackdate.ToString("yyyy-MM-dd") + "'";
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
        public static ListView SelectPODetailForInvoiceOut(string podocID, string custId)
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
                List<popiheader> POPIHeaders = popihdb.getListOfPOPIDetailForListViewSelection(podocID, custId).OrderBy(pop => pop.TrackingNo).ToList();
                ///SortedSet<popiheader> popiList = popihdb.getListOfPOPIDetailForInvoiceOut(podocID, custId);

                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Tr No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Tr Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Cust PO", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO Date", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Cust Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Project ID", 100, HorizontalAlignment.Center);
                lv.Columns.Add("Delv Add", 100, HorizontalAlignment.Center);
                lv.Columns.Add("Payment Terms", -2, HorizontalAlignment.Center);
                lv.Columns[8].Width = 0;
                foreach (popiheader popih in POPIHeaders)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(popih.TrackingNo.ToString());
                    item1.SubItems.Add(popih.TrackingDate.ToString("yyyy-MM-dd"));//ToShortDateString());
                    item1.SubItems.Add(popih.CustomerPONO);
                    item1.SubItems.Add(popih.CustomerPODate.ToShortDateString());
                    item1.SubItems.Add(popih.CustomerName);
                    item1.SubItems.Add(popih.ProjectID.ToString());
                    item1.SubItems.Add(popih.DeliveryAddress.ToString());
                    item1.SubItems.Add(popih.PaymentTerms.ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static ListView getPONoWiseStockListView(int POTrackNo, DateTime POTrackDate, string podocid)
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
                lv.Columns.Add("TrackingNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("TrackingDate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItemID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("stockItemName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ModelNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ModelName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Location", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Cust.ItemDescription", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Quantity", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Price", -2, HorizontalAlignment.Left);
                lv.Columns.Add("WarrantyDays", -2, HorizontalAlignment.Left);
                lv.Columns.Add("TaxCode", -2, HorizontalAlignment.Left);
                POPIHeaderDB popihdb = new POPIHeaderDB();
                string tempString = POPIHeaderDB.getTemporaryNOFromPONo(POTrackNo, POTrackDate, podocid);
                string[] PODetArr = tempString.Split(';');
                popiheader popih = new popiheader();
                popih.DocumentID = PODetArr[0];
                popih.TemporaryNo = Convert.ToInt32(PODetArr[1]);
                popih.TemporaryDate = Convert.ToDateTime(PODetArr[2]);
                List<popidetail> POPIDetails = POPIHeaderDB.getPOPIDetail(popih);
                //List<popidetail> POPIDetails = POPIDetailsTemp.OrderByDescending(popid => popid.StockItemName).ToList() ;
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
                    item.SubItems.Add(popid.Location.ToString());
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
                string query = "select Quantity from POProductInwardDetail where RowID=" + RefNo;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader.GetDouble(0) < Qunt)
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
        public List<ReportPO> getPODetailForpartWise(int opt1, int opt2, DateTime fromDate, DateTime toDate)
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
                    query = "select a.DocumentID,a.CustomerID,b.Name,SUM(a.ProductValueINR) from POProductInwardHeader a, Customer b" +
                        " where a.CustomerID = b.CustomerID and DocumentID in ('POSERVICEINWARD','POPRODUCTINWARD') and a.TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and a.TrackingDate >= '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and a.DocumentStatus = 99 and a.Status = 1 " +
                        "group by a.DocumentID,a.CustomerID,b.Name";
                }
                else if (opt1 == 1)
                {
                    query = "select a.DocumentID,a.CustomerID,b.Name,SUM(a.ProductValueINR) from POProductInwardHeader a, Customer b" +
                        " where a.CustomerID = b.CustomerID and a.DocumentID = 'POPRODUCTINWARD'" +
                         " and a.TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and a.TrackingDate >= '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and a.DocumentStatus = 99 and a.Status = 1 " +
                        " group by a.DocumentID,a.CustomerID,b.Name";
                }
                else
                {
                    query = "select a.DocumentID,a.CustomerID,b.Name,SUM(a.ProductValueINR) from POProductInwardHeader a, Customer b" +
                        " where a.CustomerID = b.CustomerID and a.DocumentID = 'POSERVICEINWARD' and " +
                         " a.TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and a.TrackingDate >= '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and a.DocumentStatus = 99 and a.Status = 1 " +
                        "group by a.DocumentID,a.CustomerID,b.Name";
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
                    if (rpo.DocumentID.Equals("POPRODUCTINWARD"))
                        rpo.DocumentType = "Product";
                    else if (rpo.DocumentID.Equals("POSERVICEINWARD"))
                        rpo.DocumentType = "Service";
                    POList.Add(rpo);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO Product Inward Header Details");
            }
            return POList;
        }
        public List<ReportPO> getDetailForRegionWise(int opt1, int opt2, DateTime fromDate, DateTime toDate)
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
                        "from POProductInwardHeader a,Customer b, Office c, Region d " +
                        "where a.CustomerID = b.CustomerID and b.OfficeID = c.OfficeID and c.RegionID = d.RegionID and DocumentID in ('POSERVICEINWARD','POPRODUCTINWARD') and " +
                         " a.TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and a.TrackingDate >= '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and a.DocumentStatus = 99 and a.Status = 1 " +
                        " group by a.DocumentID,c.RegionID, d.Name";
                }
                else if (opt1 == 1)
                {
                    query = "select a.DocumentID,c.RegionID,d.Name, sum(a.ProductValueINR) " +
                        "from POProductInwardHeader a,Customer b, Office c, Region d " +
                        "where a.CustomerID = b.CustomerID and b.OfficeID = c.OfficeID and c.RegionID = d.RegionID and DocumentID = 'POPRODUCTINWARD'" +
                         " and a.TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and a.TrackingDate >= '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and a.DocumentStatus = 99 and a.Status = 1 " +
                        " group by a.DocumentID,c.RegionID, d.Name";
                }
                else
                {
                    query = "select a.DocumentID,c.RegionID,d.Name, sum(a.ProductValueINR) " +
                        "from POProductInwardHeader a,Customer b, Office c, Region d " +
                        "where a.CustomerID = b.CustomerID and b.OfficeID = c.OfficeID and c.RegionID = d.RegionID and DocumentID = 'POSERVICEINWARD'" +
                         " and a.TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and a.TrackingDate >= '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and a.DocumentStatus = 99 and a.Status = 1 " +
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
                    if (rpo.DocumentID.Equals("POPRODUCTINWARD"))
                        rpo.DocumentType = "Product";
                    else if (rpo.DocumentID.Equals("POSERVICEINWARD"))
                        rpo.DocumentType = "Service";
                    POList.Add(rpo);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO Product Inward Header Details");
            }
            return POList;
        }
        public List<ReportPO> getDetailForDocumentWise(int opt1, int opt2, DateTime fromDate, DateTime toDate)
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
                    query = "select DocumentID,sum(ProductValueINR) from POProductInwardHeader" +
                         " where TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and TrackingDate >='" + fromDate.ToString("yyyy-MM-dd") + "' and DocumentID in ('POSERVICEINWARD','POPRODUCTINWARD')" + " and DocumentStatus = 99 and Status = 1 " +
                        " group by DocumentID ";
                }
                else if (opt1 == 1)
                {
                    query = "select DocumentID,sum(ProductValueINR) from POProductInwardHeader" +
                        " where DocumentID = 'POPRODUCTINWARD' " +
                        " and TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and TrackingDate >= '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and DocumentStatus = 99 and Status = 1 " +
                        "group by DocumentID ";
                }
                else
                {
                    query = "select DocumentID,sum(ProductValueINR) from POProductInwardHeader" +
                        " where DocumentID = 'POSERVICEINWARD' " +
                        " and TrackingDate <= '" + toDate.ToString("yyyy-MM-dd") + "'" +
                        " and TrackingDate >= '" + fromDate.ToString("yyyy-MM-dd") + "'" + " and DocumentStatus = 99 and Status = 1 " +
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
                    if (rpo.DocumentID.Equals("POPRODUCTINWARD"))
                        rpo.DocumentType = "Product";
                    else if (rpo.DocumentID.Equals("POSERVICEINWARD"))
                        rpo.DocumentType = "Service";
                    POList.Add(rpo);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO Product Inward Header Details");
            }
            return POList;
        }

        public static string getPOPIDtlsForProjectTrans(string projectID)
        {
            string str = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select COUNT(*), SUM(ProductValueINR) from POProductInwardHeader where ProjectID = '" + projectID + "' and DocumentStatus = 99 and status = 1 ";
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
                    " where ProjectID = '" + projectID + "' and DocumentStatus = 99 and status = 1";
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
                      "', OfficeID='" + popih.OfficeID +
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
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,Location,ModelNo,TaxCode,CustomerItemDescription,WorkDescription,Quantity,Price,Tax,WarrantyDays,TaxDetails) " +
                    "values ('" + popih.DocumentID + "'," +
                    popih.TemporaryNo + "," +
                    "'" + popih.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + popid.StockItemID + "'," +
                      "'" + popid.Location + "'," +
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
                if (popih.DocumentID == "PAFPRODUCTINWARD")  //For Product PAF
                {
                    popih.TemporaryNo = DocumentNumberDB.getNumber("POPRODUCTINWARD", 1);
                    updateSQL = "update DocumentNumber set TempNo =" + popih.TemporaryNo +
                                " where FYID='" + Main.currentFY + "' and DocumentID='" + "POPRODUCTINWARD" + "'";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                       ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                       Main.QueryDelimiter;
                }
                else if (popih.DocumentID == "PAFSERVICEINWARD") //For Service PAF
                {
                    popih.TemporaryNo = DocumentNumberDB.getNumber("POSERVICEINWARD", 1);
                    updateSQL = "update DocumentNumber set TempNo =" + popih.TemporaryNo +
                                " where FYID='" + Main.currentFY + "' and DocumentID='" + "POSERVICEINWARD" + "'";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                       ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                       Main.QueryDelimiter;
                }
                else
                {
                    popih.TemporaryNo = DocumentNumberDB.getNumber(popih.DocumentID, 1);
                    updateSQL = "update DocumentNumber set TempNo =" + popih.TemporaryNo +
                   " where FYID='" + Main.currentFY + "' and DocumentID='" + popih.DocumentID + "'";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                       ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                       Main.QueryDelimiter;
                }

                if (popih.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                //updateSQL = "update DocumentNumber set TempNo =" + popih.TemporaryNo +
                //    " where FYID='" + Main.currentFY + "' and DocumentID='" + popih.DocumentID + "'";
                //utString = utString + updateSQL + Main.QueryDelimiter;
                //utString = utString +
                //   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                //   Main.QueryDelimiter;

                updateSQL = "insert into POProductInwardHeader " +
                     "(DocumentID,ReferenceNo,TemporaryNo,TemporaryDate,TrackingNo,TrackingDate,CustomerID,CustomerPONo,CustomerPODate," +
                     "DeliveryDate,ValidityDate,PaymentTerms,PaymentMode,FreightTerms,FreightCharge,CurrencyID,ExchangeRate,ProjectID,OfficeID,BillingAddress,DeliveryAddress,ProductValue,ProductValueINR,TaxAmount,TaxAmountINR," +
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
                      "'" + popih.OfficeID + "'," +
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
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,Location,ModelNo,TaxCode,CustomerItemDescription,WorkDescription,Quantity,Price,Tax,WarrantyDays,TaxDetails) " +
                    "values ('" + popih.DocumentID + "'," +
                    popih.TemporaryNo + "," +
                    "'" + popih.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + popid.StockItemID + "'," +
                     "'" + popid.Location + "'," +
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
        public static double getPOQuantityForInvoiceOut(int poRefNo)
        {
            double Qunt = 0;
            try
            {
                //string query1 = "select b.Quantity from POProductInwardHeader a, POProductInwardDetail b " +
                //    "where a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate " +
                //    "and b.TemporaryNo = (select TemporaryNo from POProductInwardHeader where documentid='" + docID + "'" + " and TrackingNo = " + iod.PONo + " and TrackingDate = '" + iod.PODate.ToString("yyyy-MM-dd") + "')" +
                //    " and b.TemporaryDate = (select TemporaryDate from POProductInwardHeader where documentid='" + docID + "'" + " and  TrackingNo = " + iod.PONo + " and TrackingDate = '" + iod.PODate.ToString("yyyy-MM-dd") + "')" +
                //    " and b.StockItemID = '" + iod.StockItemID + "' and b.ModelNo = '" + iod.ModelNo + "'";
                //string query2 = "select b.Quantity from POProductInwardHeader a, POProductInwardDetail b " +
                //   "where a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate " +
                //    "and b.TemporaryNo = (select TemporaryNo from POProductInwardHeader where documentid='" + docID + "'" + " and TrackingNo = " + iod.PONo + " and TrackingDate = '" + iod.PODate.ToString("yyyy-MM-dd") + "')" +
                //    " and b.TemporaryDate = (select TemporaryDate from POProductInwardHeader where documentid='" + docID + "'" + " and  TrackingNo = " + iod.PONo + " and TrackingDate = '" + iod.PODate.ToString("yyyy-MM-dd") + "')" +
                //   " and b.StockItemID = '" + iod.StockItemID + "'";
                //switch()
                string query = "select Quantity from POProductInwardDetail " +
                   "where RowID = " + poRefNo;
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

        public static string getCustomerPOAndDateForInvoiceOut(int TrackNo, DateTime TrackDate, string docID)
        {
            string poNo = "";
            DateTime PODate = DateTime.Parse("1900-01-01");
            string billingAdd = "";
            string paymentTermCode = "";
            try
            {
                string query = "select CustomerPONo,CustomerPODate,BillingAddress,PaymentTerms from POProductInwardHeader  " +
                    "where documentID =  '" + docID + "'" +
                    " and TrackingNo =  " + TrackNo +
                    " and TrackingDate = '" + TrackDate.ToString("yyyy-MM-dd") + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    poNo = reader.GetString(0);
                    PODate = reader.GetDateTime(1);
                    billingAdd = reader.GetString(2);
                    paymentTermCode = reader.GetString(3);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return poNo.ToString() + Main.delimiter1 + PODate + Main.delimiter1 + billingAdd + Main.delimiter1 + paymentTermCode;
        }


        public static string getCustomerPOAndDateForInvoiceOutOld(int TrackNo, DateTime TrackDate, string docID)
        {
            string poNo = "";
            DateTime PODate = DateTime.Parse("1900-01-01");
            string billingAdd = "";
            string paymentTermCode = "";
            try
            {
                string query = "select CustomerPONo,CustomerPODate,BillingAddress,PaymentTerms from POProductInwardHeader  " +
                    "where documentID =  '" + docID + "'" +
                    " and TrackingNo =  " + TrackNo +
                    " and TrackingDate = '" + TrackDate.ToString("yyyy-MM-dd") + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    poNo = reader.GetString(0);
                    PODate = reader.GetDateTime(1);
                    billingAdd = reader.GetString(2);
                    paymentTermCode = reader.GetString(3);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return poNo.ToString() + Main.delimiter1 + PODate.ToString("dd-MM-yyyy") + Main.delimiter1 + billingAdd + Main.delimiter1 + paymentTermCode;
        }

        public List<popiheader> ListPopiFilters(string QueryData)
        {
            popiheader popih;
            int ABC = 0;
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
                    popih.ForwardUser = reader.IsDBNull(28) ? "" : reader.GetString(28);
                    popih.ApproveUser = reader.IsDBNull(29) ? "" : reader.GetString(29);
                    popih.CreatorName = reader.IsDBNull(30) ? "" : reader.GetString(30);
                    popih.ForwarderName = reader.IsDBNull(31) ? "" : reader.GetString(31);
                    popih.ApproverName = reader.IsDBNull(32) ? "" : reader.GetString(32);
                    popih.OfficeID = reader.IsDBNull(33) ? "" : reader.GetString(33);
                    //CreatorName,ForwarderName,ApproverName,OfficeID
                    if (!reader.IsDBNull(34))
                    {
                        popih.CommentStatus = reader.GetString(34);
                    }
                    else
                    {
                        popih.CommentStatus = "";
                    }
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
                    popih.ProjectID = reader.IsDBNull(37) ? "" : reader.GetString(37);
                    popih.ExchangeRate = reader.GetDecimal(38);
                    popih.ProductValueINR = reader.GetDouble(39);
                    popih.TaxAmountINR = reader.GetDouble(40);
                    popih.POValueINR = reader.GetDouble(41);
                    ////////popih.DtlsID = reader.IsDBNull(42) ? 0 : reader.GetInt32(42);
                    ////////ABC = reader.IsDBNull(47) ? 99 : reader.GetInt32(47);
                    ////////if (ABC != 99)
                    ////////{
                    ////////    popih.BlncAmount = 0;
                    ////////    popih.invoiceno = reader.IsDBNull(44) ? 0 : reader.GetInt32(44);
                    ////////    popih.InvoiceDate = reader.IsDBNull(45) ? DateTime.MinValue : reader.GetDateTime(45);
                    ////////    popih.status = reader.IsDBNull(46) ? 1 : reader.GetInt32(46);
                    ////////    popih.DocumentStatus = reader.IsDBNull(47) ? 99 : reader.GetInt32(47);
                    ////////    POPIHeadersReport.Add(popih);
                    ////////}
                    ////////else
                    ////////{
                    ////////    popih.BlncAmount = reader.IsDBNull(43) ? 0 : reader.GetDouble(43);
                    ////////    popih.invoiceno = reader.IsDBNull(44) ? 0 : reader.GetInt32(44);
                    ////////    popih.InvoiceDate = reader.IsDBNull(45) ? DateTime.MinValue : reader.GetDateTime(45);
                    ////////    popih.status = reader.IsDBNull(46) ? 1 : reader.GetInt32(46);
                    ////////    popih.DocumentStatus = reader.IsDBNull(47) ? 99 : reader.GetInt32(47);
                    ////////}
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
        public List<popiheader> ListPopiFilters1(string QueryData)
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
                    popih.ForwardUser = reader.IsDBNull(28) ? "" : reader.GetString(28);
                    popih.ApproveUser = reader.IsDBNull(29) ? "" : reader.GetString(29);
                    popih.CreatorName = reader.IsDBNull(30) ? "" : reader.GetString(30);
                    popih.ForwarderName = reader.IsDBNull(31) ? "" : reader.GetString(31);
                    popih.ApproverName = reader.IsDBNull(32) ? "" : reader.GetString(32);
                    popih.OfficeID = reader.IsDBNull(33) ? "" : reader.GetString(33);
                    //CreatorName,ForwarderName,ApproverName,OfficeID
                    if (!reader.IsDBNull(34))
                    {
                        popih.CommentStatus = reader.GetString(34);
                    }
                    else
                    {
                        popih.CommentStatus = "";
                    }
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
                    popih.ProjectID = reader.IsDBNull(37) ? "" : reader.GetString(37);
                    popih.ExchangeRate = reader.GetDecimal(38);
                    popih.ProductValueINR = reader.GetDouble(39);
                    popih.TaxAmountINR = reader.GetDouble(40);
                    popih.POValueINR = reader.GetDouble(41);


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
        public List<invoiceoutheader> ListPopiqty(string trno, DateTime trdate, string documentID)
        {
            invoiceoutheader popih;
            List<invoiceoutheader> POPIHeadersReport = new List<invoiceoutheader>();
            try
            {
                string docString = "";
                if (documentID == "POPRODUCTINWARD")
                {
                    docString = "'PRODUCTINVOICEOUT', 'PRODUCTEXPORTINVOICEOUT'";
                }
                if (documentID == "POSERVICEINWARD")
                {
                    docString = "'SERVICEINVOICEOUT', 'SERVICEEXPORTINVOICEOUT'";
                }
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select b.ProductValueINR,b.TrackingNos from " +
                              "ViewInvoiceOutHeader b where b.DocumentID in (" +
                              docString +
                              ") and " +
                              "b.TrackingNos like ('%" + trno + ";%') and b.TrackingDates like ('%" + trdate.Date.ToString("yyyy-MM-dd") + ";%') ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popih = new invoiceoutheader();
                    popih.ProductValueINR = reader.IsDBNull(0) ? 0 : reader.GetDouble(0);
                    popih.TrackingNos = reader.GetString(1);
                    POPIHeadersReport.Add(popih);
                }

            }
            catch (Exception ex)
            {

            }
            return POPIHeadersReport;
        }

        public List<invoiceoutheader> ListInvOutValue(string trno, DateTime trdate, string documentID)
        {
            invoiceoutheader popih;
            List<invoiceoutheader> POPIHeadersReport = new List<invoiceoutheader>();
            try
            {
                string docString = "";
                if (documentID == "POPRODUCTINWARD")
                {
                    docString = "'PRODUCTINVOICEOUT', 'PRODUCTEXPORTINVOICEOUT'";
                }
                if (documentID == "POSERVICEINWARD")
                {
                    docString = "'SERVICEINVOICEOUT', 'SERVICEEXPORTINVOICEOUT'";
                }
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select b.ProductValueINR,b.TrackingNos,b.TrackingDates from " +
                              "ViewInvoiceOutHeader b where b.DocumentID in (" +
                              docString +
                              ") and status=1 and documentstatus=99";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popih = new invoiceoutheader();
                    popih.ProductValueINR = reader.IsDBNull(0) ? 0 : reader.GetDouble(0);
                    popih.TrackingNos = reader.GetString(1);
                    popih.TrackingDates = reader.GetString(2);
                    POPIHeadersReport.Add(popih);
                }

            }
            catch (Exception ex)
            {

            }
            return POPIHeadersReport;
        }

        //string query1 = "select b.ProductValueINR from " +
        //              "ViewInvoiceOutHeader b where b.DocumentID in ('PRODUCTINVOICEOUT', 'PRODUCTEXPORTINVOICEOUT') and " +
        //               "b.TrackingNos = '" + popih.TrackingNo + ";' and b.TrackingDates = '" + popih.TemporaryDate.Date.ToString("yyyy-MM-dd") + ";' ";
        //SqlCommand cmd1 = new SqlCommand(query1, conn);
        //reader = cmd1.ExecuteNonQuery();
        //while (reader1.Read())
        //{
        //    popih.TotalValue = reader.IsDBNull(0) ? 0 : reader.GetDouble(0);
        //}
        public static List<invoiceoutheader> getPOPitrack(popiheader popih)
        {
            invoiceoutheader popid;
            List<invoiceoutheader> POPIHdr = new List<invoiceoutheader>();
            try
            {
                string docString = "";
                if (popih.DocumentID == "POPRODUCTINWARD")
                {
                    docString = "'PRODUCTINVOICEOUT', 'PRODUCTEXPORTINVOICEOUT'";
                }
                if (popih.DocumentID == "POSERVICEINWARD")
                {
                    docString = "'SERVICEINVOICEOUT', 'SERVICEEXPORTINVOICEOUT'";
                }
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.TrackingDates,a.TrackingNos,a.TemporaryNo,a.TemporaryDate,InvoiceNo," +
                    "InvoiceDate  from ViewInvoiceOutHeader a" +
                 " where a.DocumentID in (" +
                 docString +
                 ") " +
             "and a.TrackingNos like  ('%" + popih.TrackingNo + ";%') and a.TrackingDates like ('%" + popih.TrackingDate.ToString("yyyy-MM-dd") + ";%') ";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popid = new invoiceoutheader();
                    popid.TrackingNos = reader.GetString(1);
                    popid.TrackingDates = reader.GetString(0);
                    popid.TemporaryNo = reader.GetInt32(2);
                    popid.TemporaryDate = reader.GetDateTime(3);
                    popid.InvoiceNo = reader.GetInt32(4);
                    popid.InvoiceDate = reader.GetDateTime(5);
                    POPIHdr.Add(popid);

                }
            }
            catch (Exception ex)
            {

            }
            return POPIHdr;
        }


        public static List<popidetail> getPOPOutDetailqty(popiheader popih)
        {
            popidetail popid;
            List<popidetail> POPIDetail = new List<popidetail>();
            try
            {
                string docString = "";
                if (popih.DocumentID == "POPRODUCTINWARD")
                {
                    docString = "'PRODUCTINVOICEOUT', 'PRODUCTEXPORTINVOICEOUT'";
                }
                if (popih.DocumentID == "POSERVICEINWARD")
                {
                    docString = "'SERVICEINVOICEOUT', 'SERVICEEXPORTINVOICEOUT'";
                }
                //string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.RowID,a.DocumentID,a.TemporaryNo, a.TemporaryDate,b.InvoiceNo,b.InvoiceDate,a.StockItemID,a.StockItemName, " +
                               "a.ModelNo, ISNULL(a.ModelName ,'') as ModelName, ISNULL(a.CustomerItemDescription,' ') as CustomerItemDescription, " +
                               "a.Quantity,a.Price,a.Tax,a.WarrantyDays,a.TaxDetails,a.TaxCode from ViewInvoiceOutDetail a,ViewInvoiceOutHeader b " +
                               "where a.TemporaryNo=b.TemporaryNo and b.InvoiceNo = '" + popih.invoiceno + "' " +
                               "and a.DocumentID in (" +
                               docString +
                               ") " +
                               "and b.InvoiceDate = ('" + popih.InvoiceDate.Date.ToString("yyyy-MM-dd") + "') and a.DocumentID=b.DocumentID order by a.TemporaryNo";

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
                  "Quantity,Price " +
                  " from ViewPOInwardItemwiseQuantity " +
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
                    popid.StockItemID = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    popid.StockItemName = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    popid.ModelNo = reader.IsDBNull(6) ? "NA" : reader.GetString(6);
                    popid.ModelName = reader.IsDBNull(7) ? "NA" : reader.GetString(7);

                    popid.CustomerItemDescription = reader.GetString(8);
                    popid.WorkDescription = reader.GetString(9);
                    popid.Quantity = reader.GetDouble(10);
                    popid.Price = reader.GetDouble(11);
                    ////popid.Tax = reader.GetDouble(12);
                    ////popid.WarrantyDays = reader.GetInt32(13);
                    ////popid.TaxDetails = reader.GetString(14);
                    ////popid.TaxCode = reader.IsDBNull(15) ? "" : reader.GetString(15);
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
        public List<invoiceoutheader> ListInvOutValue(DateTime frmdt, DateTime todt)
        {
            invoiceoutheader popih;
            List<invoiceoutheader> POPIHeadersReport = new List<invoiceoutheader>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = " select TrackingDocumentID,TrackingDate,TrackingNo,AmountINR from " +
                              " ViewPOInVsInvValue where TrackingDate >='" + frmdt.ToString("yyyy-MM-dd") + "'" +
                              " and TrackingDate <='" + todt.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popih = new invoiceoutheader();
                    popih.DocumentID = reader.GetString(0);
                    popih.TrackingDates = reader.GetDateTime(1).ToString("yyyy-MM-dd");
                    popih.TrackingNos = reader.GetInt32(2).ToString();
                    popih.ProductValueINR = reader.IsDBNull(3) ? 0 : reader.GetDouble(3);
                    POPIHeadersReport.Add(popih);
                }

            }
            catch (Exception ex)
            {

            }
            return POPIHeadersReport;
        }

        public DataGridView getGridViewForGivenListOfItems(string docID)
        {

            DataGridView grdPOPI = new DataGridView();
            try
            {
                string[] strColArr = { "TrackingNo", "TrackingDate","CustPONo","CustPODate", "ReferenceNo",
                    "CustID","CustName","ProdValue","TaxAmount","ProjectID","OfficeName","OfficeID"};
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                };

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdPOPI.EnableHeadersVisualStyles = false;
                grdPOPI.AllowUserToAddRows = false;
                grdPOPI.AllowUserToDeleteRows = false;
                grdPOPI.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdPOPI.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdPOPI.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdPOPI.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdPOPI.ColumnHeadersHeight = 27;
                grdPOPI.RowHeadersVisible = false;
                grdPOPI.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;
                grdPOPI.Columns.Add(colChk);
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str;
                    colArr[index].HeaderText = str;
                    colArr[index].ReadOnly = true;
                    if (index == 1 || index == 3)
                        colArr[index].DefaultCellStyle.Format = "dd-MM-yyyy";
                    if (index == 6)
                        colArr[index].Width = 350;
                    else if (index == 2)
                        colArr[index].Width = 100;
                    else
                        colArr[index].Width = 80;
                    if (index == 5 || index == 11)
                        colArr[index].Visible = false;
                    else
                        colArr[index].Visible = true;
                    grdPOPI.Columns.Add(colArr[index]);
                }

                POPIHeaderDB popihdb = new POPIHeaderDB();
                List<popiheader> POPIHeaders = new List<popiheader>();
                List<popiheader> POPIHeadersTemp = new List<popiheader>();
                //POPIHeaders = popihdb.getFilteredPOPIHeader("", 4, "", "").Where(popi => popi.DocumentID == docID)
                //                                    .OrderByDescending(popi => popi.TrackingNo).ThenBy(popi => popi.TrackingDate).ToList();
                POPIHeaders = popihdb.getListOfPOPIDetailForGridviewSelection(docID)
                                                   .OrderBy(popi => popi.TrackingNo).ToList();
                //POPIHeaders = POPIHeadersTemp.OrderBy(popi => popi.TrackingNo).ThenBy(popi => popi.TrackingDate).ToList();
                foreach (popiheader pop in POPIHeaders)
                {
                    grdPOPI.Rows.Add();
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[0]].Value = pop.TrackingNo;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[1]].Value = pop.TrackingDate;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[2]].Value = pop.CustomerPONO;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[3]].Value = pop.CustomerPODate;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[4]].Value = pop.ReferenceNo;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[5]].Value = pop.CustomerID;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[6]].Value = pop.CustomerName;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[7]].Value = pop.ProductValue;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[8]].Value = pop.TaxAmount;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[9]].Value = pop.ProjectID;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[10]].Value = pop.OfficeName;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[11]].Value = pop.OfficeID;
                }
            }
            catch (Exception ex)
            {
            }

            return grdPOPI;
        }


        //get OfficeID, Customer and ProjectID from POPI service
        public static string getPOPIServiceDetailForIndentService(int TrackNo, DateTime TrackDate, string docID)
        {
            string OfficeName = "";
            string CustomerID = "";
            string ProjectID = "";
            string OfficeID = "";
            try
            {
                string query = "select OfficeName,CustomerName,ProjectID,OfficeID from ViewPOProductInwardHeader  " +
                    "where documentID =  '" + docID + "'" +
                    " and TrackingNo =  " + TrackNo +
                    " and TrackingDate = '" + TrackDate.ToString("yyyy-MM-dd") + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    OfficeName = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    CustomerID = reader.GetString(1);
                    ProjectID = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    OfficeID = reader.IsDBNull(3) ? "" : reader.GetString(3);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return OfficeName + Main.delimiter1 + CustomerID + Main.delimiter1 + ProjectID + Main.delimiter1 + OfficeID;
        }

        //get POInwardDetail For PAFConversion
        public List<popiheader> getFilteredPAFTypeProductInward(string Docid, string custID)
        {
            popiheader popih = new popiheader();
            List<popiheader> POPIHeaders = new List<popiheader>();
            string str = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID,TemporaryNo,TemporaryDate,TrackingNo, TrackingDate,CustomerID,CustomerName,ReferenceNo,CustomerPONO,CustomerPODate,POValueINR from ViewPOProductInwardHeader " +
                    " where DocumentID = '" + Docid + "' and DocumentStatus = 99 and Status = 1 and PreviousDocumentType = 0 and CustomerID = '" + custID +
                    "' order by TrackingDate desc,DocumentID asc,TrackingNo desc ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popih = new popiheader();
                    popih.DocumentID = reader.GetString(0);
                    popih.TemporaryNo = reader.GetInt32(1);
                    popih.TemporaryDate = reader.GetDateTime(2);
                    popih.TrackingNo = reader.GetInt32(3);
                    popih.TrackingDate = reader.GetDateTime(4);
                    popih.CustomerID = reader.GetString(5);
                    popih.CustomerName = reader.GetString(6);
                    popih.ReferenceNo = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    popih.CustomerPONO = reader.GetString(8);
                    popih.CustomerPODate = reader.GetDateTime(9);
                    popih.POValueINR = reader.GetDouble(10);
                    POPIHeaders.Add(popih);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Retriving PAF Inward List");
            }
            return POPIHeaders;
        }
        public Boolean updatePAFPOInwardCLosing(popiheader poih, List<ioheader> IOList, string newRefTracStr)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POProductInwardHeader set Status=7" +
                    " where DocumentID='" + poih.DocumentID + "'" +
                    " and TrackingNo=" + poih.TrackingNo +
                    " and TrackingDate='" + poih.TrackingDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POProductInwardHeader", "", updateSQL) +
                Main.QueryDelimiter;

                foreach (ioheader ioh in IOList)
                {
                    updateSQL = "update InternalOrderHeader " +
                    " set ReferenceTrackingNos = '" + newRefTracStr + "'" +
                    " where DocumentID = '" + ioh.DocumentID + "'" +
                    " and TemporaryNo = " + ioh.TemporaryNo +
                    " and TemporaryDate = '" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";

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
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        public Boolean updatePAFPOInwardCLosingNew(popiheader poih, string DocID)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POProductInwardHeader set DocumentID = '" + DocID + "' , PreviousDocumentType = 1 " +
                    "where DocumentID = '" + poih.DocumentID + "'" +
                    " and TemporaryNo=" + poih.TemporaryNo +
                    " and TemporaryDate='" + poih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POProductInwardHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "update POProductInwardDetail set DocumentID = '" + DocID + "'" +
                    " where DocumentID = '" + poih.DocumentID + "'" +
                    " and TemporaryNo=" + poih.TemporaryNo +
                    " and TemporaryDate='" + poih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POProductInwardDetail", "", updateSQL) +
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
        //For Validating CustomerPONo And Date Duplication
        public static Boolean checkCustomerPONoAndDatePresence(popiheader poh, int opt)
        {
            Boolean status = false;
            string query = "";
            try
            {
                string query1 = "select count(*) from POProductInwardHeader  " +
                  "where documentID =  '" + poh.DocumentID + "'" +
                  " and CustomerPONo = '" + poh.CustomerPONO + "'" +
                  " and CustomerPODate = '" + poh.CustomerPODate.ToString("yyyy-MM-dd") + "'";
                string query2 = "select count(*) from POProductInwardHeader  " +
                   "where documentID =  '" + poh.DocumentID + "'" +
                   " and TemporaryNo <> " + poh.TemporaryNo +
                   " and TemporaryDate <> '" + poh.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                    " and CustomerPONo = '" + poh.CustomerPONO + "'" +
                  " and CustomerPODate = '" + poh.CustomerPODate.ToString("yyyy-MM-dd") + "'";
                switch (opt)
                {
                    case 1:
                        query = query1;
                        break;
                    case 2:
                        query = query2;
                        break;
                    default:
                        query = "";
                        break;
                }

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader.GetInt32(0) != 0)
                        status = true;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        //Get tracking No and Date from tempNo and date
        public static string getTrackingNoAndDate(int TempNo, DateTime TempDate, string DocID)
        {
            string tempString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID, TrackingNo, TrackingDate from POProductInwardHeader" +
                    " where DocumentID = '" + DocID + "' and TemporaryNo = " + TempNo +
                    " and TemporaryDate='" + TempDate.ToString("yyyy-MM-dd") + "'";
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
        //Check Invoice Prepared For a Perticular Tracking
        public static string checkForInvoiceIssuesForPOPI(int tno, DateTime tdt, string docID)
        {
            string result = "";   // Not issued
            try
            {
                POPIHeaderDB popihdb = new POPIHeaderDB();
                string trackString = POPIHeaderDB.getTrackingNoAndDate(tno, tdt, docID);
                string[] PODetArr = trackString.Split(';');

                string invoiceDocIDStr = "";
                string TrackNoStr = PODetArr[1] + ";";
                string TrackDateStr = Convert.ToDateTime(PODetArr[2]).ToString("yyyy-MM-dd") + ";";

                if (PODetArr[0] == "POPRODUCTINWARD")
                {
                    invoiceDocIDStr = "PRODUCTINVOICEOUT','PRODUCTEXPORTINVOICEOUT";
                }
                else if (PODetArr[0] == "POSERVICEINWARD")
                {
                    invoiceDocIDStr = "SERVICEINVOICEOUT','SERVICEEXPORTINVOICEOUT";
                }

                SqlConnection conn = new SqlConnection(Login.connString);

                List<invoiceoutheader> invoiceList = new List<invoiceoutheader>();
                invoiceoutheader ioh = new invoiceoutheader();

                string query = "select DocumentID,TemporaryNo,TemporaryDate,TrackingNos,TrackingDates from InvoiceOutHeader where DocumentID in ('" + invoiceDocIDStr + "') and " +
                        " TrackingNos like '%" + TrackNoStr + "%' and TrackingDates like '%" + TrackDateStr + "%'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ioh = new invoiceoutheader();
                    ioh.DocumentID = reader.GetString(0);
                    ioh.TemporaryNo = reader.GetInt32(1);
                    ioh.TemporaryDate = reader.GetDateTime(2);
                    ioh.TrackingNos = reader.GetString(3);
                    ioh.TrackingDates = reader.GetString(4);
                    invoiceList.Add(ioh);
                }
                if (invoiceList.Count != 0)
                {
                    foreach (invoiceoutheader iohead in invoiceList)
                    {
                        string[] trackNosStr = iohead.TrackingNos.Split(';');
                        string[] trackDatesStr = iohead.TrackingDates.Split(';');

                        int indexofTrackNo = Array.IndexOf(trackNosStr, PODetArr[1]); //get index of required track no in string array
                        //If required Track No is not found in trackNosStr array then index will be -1
                        if (indexofTrackNo != -1)
                        {
                            DateTime TrackDateFound = Convert.ToDateTime(trackDatesStr[indexofTrackNo]);  // get track date from trackDatesStr with track no index
                            DateTime TrackDateRequired = Convert.ToDateTime(PODetArr[2]);

                            if (TrackDateFound == TrackDateRequired)
                            {
                                result = result + iohead.TemporaryNo + ";" + iohead.TemporaryDate + Main.delimiter1;
                            }
                        }
                        string str = PODetArr[2];
                    }
                }
                conn.Close();
                //result = "error";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error quering Invoice Out Header Details");
                result = "error";
            }
            return result;
        }
        public static List<popidetail> getPOPIDetailForIO(popiheader popih)
        {
            popidetail popid;
            List<popidetail> POPIDetail = new List<popidetail>();
            try
            {
                //string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID,StockItemID,StockItemName," +
                   " CustomerItemDescription,Quantity,WarrantyDays,ApproveUserName,Price,CustomerName " +
                   "from ViewPOProductInwardDetail " +
                   " where DocumentID='" + popih.DocumentID + "'" +
                   " and TrackingNo=" + popih.TrackingNo +
                   " and TrackingDate='" + popih.TrackingDate.ToString("yyyy-MM-dd") + "'" +
                   " order by StockItemID";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popid = new popidetail();
                    popid.DocumentID = reader.GetString(0);
                    popid.StockItemID = reader.GetString(1);
                    popid.StockItemName = reader.GetString(2);
                    popid.CustomerItemDescription = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    popid.Quantity = reader.GetDouble(4);
                    popid.WarrantyDays = reader.GetInt32(5);
                    popid.WorkDescription = reader.IsDBNull(6) ? "" : reader.GetString(6); ///For ApproveUSername
                    popid.Price = reader.GetDouble(7);
                    popid.TaxDetails = reader.IsDBNull(8) ? "" : reader.GetString(8); ///For CustomerName
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
        public static ListView getPONoWiseStockListViewForIndentService(int POTrackNo, DateTime POTrackDate, string podocid)
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
                lv.Columns.Add("TrackingNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("TrackingDate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItemID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("stockItemName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Cust.ItemDescription", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Location", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Quantity", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Price", -2, HorizontalAlignment.Left);
                lv.Columns.Add("WarrantyDays", -2, HorizontalAlignment.Left);
                lv.Columns.Add("TaxCode", -2, HorizontalAlignment.Left);
                POPIHeaderDB popihdb = new POPIHeaderDB();
                string tempString = POPIHeaderDB.getTemporaryNOFromPONo(POTrackNo, POTrackDate, podocid);
                string[] PODetArr = tempString.Split(';');
                popiheader popih = new popiheader();
                popih.DocumentID = PODetArr[0];
                popih.TemporaryNo = Convert.ToInt32(PODetArr[1]);
                popih.TemporaryDate = Convert.ToDateTime(PODetArr[2]);
                List<popidetail> POPIDetails = POPIHeaderDB.getPOPIDetail(popih);
                //List<popidetail> POPIDetails = POPIDetailsTemp.OrderByDescending(popid => popid.StockItemName).ToList() ;
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
                    item.SubItems.Add(popid.CustomerItemDescription.ToString());
                    item.SubItems.Add(popid.Location.ToString());
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
        public static double getPOItemWisePrice(int RefNo)
        {
            double price = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select Price from POProductInwardDetail where RowID=" + RefNo;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    price = reader.GetDouble(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO item price");
            }
            return price;
        }

        //CLosing POINward
        public static Boolean RequestTOClosePOHeader(popiheader popih, string comments)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POProductInwardHeader set ClosingStatus=1 " +
                     ", Comments='" + comments +
                    "' where DocumentID='" + popih.DocumentID + "'" +
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
        public static Boolean ClosePOHeader(popiheader popih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POProductInwardHeader set ClosingStatus=2 , Status = 7 " +
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
        public static Boolean RejectClosingRequest(popiheader popih, string comments)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POProductInwardHeader set ClosingStatus= 0 " +
                     ", Comments= '" + comments +
                    "' where DocumentID='" + popih.DocumentID + "'" +
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

        //Query Optimization
        //For ListView Selection
        public List<popiheader> getListOfPOPIDetailForListViewSelection(string docId, string custID)
        {
            popiheader popih;
            List<popiheader> POPIHeaders = new List<popiheader>();
            try
            {
                string query = "select a.RowID, a.DocumentID,a.TemporaryNo, a.TemporaryDate," +
                    " a.TrackingNo,a.TrackingDate,a.CustomerID,b.Name,a.CustomerPONO,a.CustomerPODate," +
                    " a.status,a.DocumentStatus,a.OfficeID,a.ProjectID,a.DeliveryAddress,a.PaymentTerms " +
                    " from POProductInwardHeader a, Customer b where a.CustomerID = b.CustomerID and a.DocumentID = '" + docId + "'" +
                    " and a.CustomerID = '" + custID + "'" +
                    " and a.status = 1 and a.DocumentStatus = 99";

                SqlConnection conn = new SqlConnection(Login.connString);
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
                    popih.TrackingDate = reader.GetDateTime(5);
                    popih.CustomerID = reader.GetString(6);
                    popih.CustomerName = reader.GetString(7);
                    popih.CustomerPONO = reader.GetString(8);
                    popih.CustomerPODate = reader.GetDateTime(9);
                    popih.status = reader.GetInt32(10);
                    popih.DocumentStatus = reader.GetInt32(11);
                    popih.OfficeID = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    popih.ProjectID = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    popih.DeliveryAddress = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    popih.PaymentTerms = reader.IsDBNull(15) ? "" : reader.GetString(15);
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
        //for GridvIew selection
        public List<popiheader> getListOfPOPIDetailForGridviewSelection(string docId)
        {
            popiheader popih;
            List<popiheader> POPIHeaders = new List<popiheader>();
            try
            {
                string query = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " TrackingNo,TrackingDate,CustomerID,CustomerName,CustomerPONO,CustomerPODate," +
                    " status,DocumentStatus,OfficeID,ReferenceNo,ProjectID,ProductValue,TaxAmount,OfficeName " +
                    " from ViewPOProductInwardHeader where  DocumentID = '" + docId + "'" +
                    " and status = 1 and DocumentStatus = 99";

                SqlConnection conn = new SqlConnection(Login.connString);
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
                    popih.TrackingDate = reader.GetDateTime(6);
                    popih.CustomerID = reader.GetString(7);
                    popih.CustomerName = reader.GetString(8);
                    popih.CustomerPONO = reader.GetString(9);
                    popih.CustomerPODate = reader.GetDateTime(10);
                    popih.status = reader.GetInt32(11);
                    popih.DocumentStatus = reader.GetInt32(12);
                    popih.OfficeID = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    popih.ReferenceNo = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    popih.ProjectID = reader.IsDBNull(15) ? "" : reader.GetString(15);
                    popih.ProductValue = reader.GetDouble(16);
                    popih.TaxAmount = reader.GetDouble(17);
                    popih.OfficeName = reader.IsDBNull(18) ? "" : reader.GetString(18);
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

        //CLosing Product PAF with updating internal order reference
        public static Boolean CLosingPAFPOProductInward(popiheader poih, List<ioheader> IOList, string docID,
            string custPONo, DateTime custPODate, string refNo)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POProductInwardHeader set PreviousDocumentType = 1, DocumentID = '" + docID + "'," +
                    " CustomerPONo='" + custPONo + "'," +
                    " CustomerPODate='" + custPODate.ToString("yyyy-MM-dd") + "'," +
                    " ReferenceNo='" + refNo + "'" +
                    " where DocumentID='" + poih.DocumentID + "'" +
                    " and TrackingNo=" + poih.TrackingNo +
                    " and TrackingDate='" + poih.TrackingDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POProductInwardHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "update POProductInwardDetail set DocumentID = '" + docID + "'" +
                   " where DocumentID = '" + poih.DocumentID + "'" +
                   " and TemporaryNo=" + poih.TemporaryNo +
                   " and TemporaryDate='" + poih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POProductInwardDetail", "", updateSQL) +
                Main.QueryDelimiter;

                foreach (ioheader ioh in IOList)
                {
                    updateSQL = "update InternalOrderHeader " +
                    " set ReferenceTrackingNos = '" + ioh.ReferenceTrackingNos + "'" +
                    " where DocumentID = '" + ioh.DocumentID + "'" +
                    " and TemporaryNo = " + ioh.TemporaryNo +
                    " and TemporaryDate = '" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";

                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InternalOrderHeader", "", updateSQL) +
                    Main.QueryDelimiter;
                }
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
        //CLosing Service PAF with updating Iindent Header reference
        public static Boolean CLosingPAFPOServiceInward(popiheader poih, List<indentserviceheader> inhList,
            string docID, string custPONo, DateTime custPODate, string refNo)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POProductInwardHeader set PreviousDocumentType = 1, DocumentID = '" + docID + "'," +
                    " CustomerPONo='" + custPONo + "'," +
                    " CustomerPODate='" + custPODate.ToString("yyyy-MM-dd") + "'," +
                    " ReferenceNo='" + refNo + "'" +
                    " where DocumentID='" + poih.DocumentID + "'" +
                    " and TrackingNo=" + poih.TrackingNo +
                    " and TrackingDate='" + poih.TrackingDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POProductInwardHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "update POProductInwardDetail set DocumentID = '" + docID + "'" +
                   " where DocumentID = '" + poih.DocumentID + "'" +
                   " and TemporaryNo=" + poih.TemporaryNo +
                   " and TemporaryDate='" + poih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POProductInwardDetail", "", updateSQL) +
                Main.QueryDelimiter;

                foreach (indentserviceheader inh in inhList)
                {
                    updateSQL = "update WORequestHeader " +
                    " set ReferenceInternalOrder = '" + inh.ReferenceInternalOrder + "'" +
                    " where DocumentID = '" + inh.DocumentID + "'" +
                    " and TemporaryNo = " + inh.TemporaryNo +
                    " and TemporaryDate = '" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'";

                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "WORequestHeader", "", updateSQL) +
                    Main.QueryDelimiter;
                }
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
        public static string getPOLocaationForInvoiceOut(int poRefNo)
        {
            string loc = "";
            try
            {
                string query = "select Location from POProductInwardDetail " +
                   "where RowID = " + poRefNo;
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    loc = reader.IsDBNull(0) ? "" : reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return loc;
        }
        public Boolean updatePOPIAmandment(List<popidetail> POPIDetails, popiheader popih)
        {
            //return false;
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                foreach (popidetail popid in POPIDetails)
                {
                    if (popid.RowID == 0)
                    {
                        //new record
                        updateSQL = "insert into POProductInwardDetail " +
                            "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,Location,ModelNo,TaxCode,CustomerItemDescription,WorkDescription,Quantity,Price,Tax,WarrantyDays,TaxDetails) " +
                             "values ('" + popid.DocumentID + "'," +
                            popid.TemporaryNo + "," +
                            "'" + popid.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                            "'" + popid.StockItemID + "'," +
                                "'" + popid.Location + "'," +
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
                    else
                    {
                        //update record
                        if (popid.Quantity > 0)
                        {
                            updateSQL = "update POProductInwardDetail " +
                                " set StockItemID = '" + popid.StockItemID + "'" +
                                ",Location = '" + popid.Location + "'" +
                                ",ModelNo = '" + popid.ModelNo + "'" +
                                ",TaxCode = '" + popid.TaxCode + "'" +
                                ",CustomerItemDescription = '" + popid.CustomerItemDescription + "'" +
                                ",WorkDescription = '" + popid.WorkDescription + "'" +
                                ",Quantity = " + popid.Quantity +
                                ",Price = " + popid.Price +
                                 ",Tax = " + popid.Tax +
                                ",WarrantyDays = " + popid.WarrantyDays +
                                ",TaxDetails = '" + popid.TaxDetails + "'" +
                                " where RowID = " + popid.RowID;
                            utString = utString + updateSQL + Main.QueryDelimiter;
                            utString = utString +
                            ActivityLogDB.PrepareActivityLogQquerString("update", "POProductInwardDetail", "", updateSQL) +
                            Main.QueryDelimiter;
                        }
                        if (popid.Quantity == 0)
                        {
                            //delete the record
                            updateSQL = "delete POProductInwardDetail " +
                                " where RowID = " + popid.RowID;
                            utString = utString + updateSQL + Main.QueryDelimiter;
                            utString = utString +
                            ActivityLogDB.PrepareActivityLogQquerString("update", "POProductInwardDetail", "", updateSQL) +
                            Main.QueryDelimiter;
                        }
                    }
                }
                string updateSQLHeader = "update POProductInwardHeader set CustomerID='" + popih.CustomerID +
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
                      "', OfficeID='" + popih.OfficeID +
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
                    "', AmendmentDetails='" + popih.AmendmentDetails +
                   "', ReferenceNo='" + popih.ReferenceNo +
                   "' where DocumentID='" + popih.DocumentID + "'" +
                   " and TemporaryNo=" + popih.TemporaryNo +
                   " and TemporaryDate='" + popih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQLHeader + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POProductInwardHeader", "", updateSQL) +
                Main.QueryDelimiter;

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
        public List<popiheader> getPOPIHeader(int TempNo, DateTime TempDate, string DocID)
        {
            popiheader popih;
            List<popiheader> POPIHeaders = new List<popiheader>();
            try
            {
                string query = "select RowID,DocumentID,TemporaryNo, TemporaryDate,TrackingNo,TrackingDate,ReferenceNo, " +
                    " CustomerID , CustomerPONo,CustomerPODate, DeliveryDate,ProjectID,OfficeID,ProductValueINR,TaxAmountINR, " +
                    " POValueINR,ValidityDate,PaymentTerms,PaymentMode,FreightTerms,FreightCharge,CurrencyID from POProductInwardHeader" +
                    " where DocumentID = '" + DocID + "' and TemporaryNo = " + TempNo +
                    " and TemporaryDate='" + TempDate.ToString("yyyy-MM-dd") + "'" +
                    " and status = 1 and DocumentStatus = 99";

                SqlConnection conn = new SqlConnection(Login.connString);
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
                    popih.TrackingDate = reader.GetDateTime(5);
                    popih.ReferenceNo = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    popih.CustomerID = reader.GetString(7);
                    popih.CustomerPONO = reader.GetString(8);
                    popih.CustomerPODate = reader.GetDateTime(9);

                    popih.DeliveryDate = reader.GetDateTime(10);
                    popih.OfficeID = reader.IsDBNull(12) ? "" : reader.GetString(12);

                    popih.ProjectID = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    popih.ProductValueINR = reader.GetDouble(13);
                    popih.TaxAmountINR = reader.GetDouble(14);
                    popih.POValueINR = reader.GetDouble(15);
                    popih.ValidityDate = reader.GetDateTime(16);
                    popih.PaymentTerms = reader.GetString(17);
                    popih.PaymentMode = reader.GetString(18);
                    popih.FreightTerms = reader.GetString(19);
                    popih.FreightCharge = reader.GetDouble(20);
                    popih.CurrencyID = reader.GetString(21);
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
        public DataGridView getPONoWiseStockGridViewForIndentService(string fullRefTrackStr)
        {

            DataGridView grdPOPI = new DataGridView();
            try
            {

                string[] strColArr = { "RefNo", "TempNo","TempDate","TrackingNo", "TrackingDate",
                    "StockItemID","stockItemName","Cust.ItemDescription","Location","Quantity","Price","WarrantyDays","TaxCode"};
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                };

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdPOPI.EnableHeadersVisualStyles = false;
                grdPOPI.AllowUserToAddRows = false;
                grdPOPI.AllowUserToDeleteRows = false;
                grdPOPI.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdPOPI.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdPOPI.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdPOPI.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdPOPI.ColumnHeadersHeight = 27;
                grdPOPI.RowHeadersVisible = false;
                grdPOPI.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;
                grdPOPI.Columns.Add(colChk);
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str;
                    colArr[index].HeaderText = str;
                    colArr[index].ReadOnly = true;
                    if (index == 2 || index == 4)
                        colArr[index].DefaultCellStyle.Format = "dd-MM-yyyy";
                    if (index == 6)
                        colArr[index].Width = 150;
                    else if (index == 7)
                        colArr[index].Width = 250;
                    else if (index == 11)
                        colArr[index].Width = 100;
                    else
                        colArr[index].Width = 80;
                    if (index == 0 || index == 1 || index == 2)
                        colArr[index].Visible = false;
                    else
                        colArr[index].Visible = true;
                    grdPOPI.Columns.Add(colArr[index]);
                }
                List<popidetail> ListPODetail = new List<popidetail>();
                string[] mainStr = fullRefTrackStr.Trim().Split(Main.delimiter1);
                foreach (string str in mainStr)
                {
                    if (str.Length != 0)
                    {
                        string[] strRef = str.Trim().Split(';');
                        string DocIDStr = strRef[0]; //DocID
                        int trackNo1 = Convert.ToInt32(strRef[1].Substring(0, strRef[1].IndexOf('('))); //TrackNo
                        int findex = strRef[1].IndexOf('(');
                        int sindex = strRef[1].IndexOf(')');
                        string tstr = strRef[1].Substring(findex + 1, (sindex - findex) - 1);
                        DateTime trackDate1 = Convert.ToDateTime(tstr); //TrackDate

                        List<popidetail> ListPODetailTemp = getPodetailForIndent(trackNo1, trackDate1, DocIDStr);
                        //ListPODetail.AddRange(ListPODetailTemp);
                        foreach (popidetail popid in ListPODetailTemp)
                        {
                            grdPOPI.Rows.Add();
                            grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[0]].Value = popid.RowID;
                            grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[1]].Value = popid.TemporaryNo;
                            grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[2]].Value = popid.TemporaryDate;
                            grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[3]].Value = trackNo1;
                            grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[4]].Value = trackDate1;
                            grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[5]].Value = popid.StockItemID;
                            grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[6]].Value = popid.StockItemName;
                            grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[7]].Value = popid.CustomerItemDescription;
                            grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[8]].Value = popid.Location;
                            grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[9]].Value = popid.Quantity;
                            grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[10]].Value = popid.Price;
                            grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[11]].Value = popid.WarrantyDays;
                            grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[12]].Value = popid.TaxCode;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }

            return grdPOPI;
        }
        public static List<popidetail> getPodetailForIndent(int POTrackNo, DateTime POTrackDate, string podocid)
        {
            List<popidetail> ListPODetail = new List<popidetail>();
            try
            {
                POPIHeaderDB popihdb = new POPIHeaderDB();
                string tempString = POPIHeaderDB.getTemporaryNOFromPONo(POTrackNo, POTrackDate, podocid);
                string[] PODetArr = tempString.Split(';');
                popiheader popih = new popiheader();
                popih.DocumentID = PODetArr[0];
                popih.TemporaryNo = Convert.ToInt32(PODetArr[1]);
                popih.TemporaryDate = Convert.ToDateTime(PODetArr[2]);
                ListPODetail = POPIHeaderDB.getPOPIDetail(popih);
            }
            catch (Exception ex)
            {
            }
            return ListPODetail;
        }
    }
}
