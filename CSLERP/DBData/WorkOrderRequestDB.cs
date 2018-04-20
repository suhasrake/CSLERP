using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class woheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int WORequestNo { get; set; }
        public DateTime WORequestDate { get; set; }
        public string ReferenceInternalOrder { get; set; }
        public string ProjectID { get; set; }
        public string OfficeID { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CurrencyID { get; set; }
        public decimal ExchangeRate { get; set; }
        public string CurrencyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TargetDate { get; set; }
        public String PaymentTerms { get; set; }
        public String PaymentMode { get; set; }

        public string POAddress { get; set; }
        public double ServiceValue { get; set; }
        public double TaxAmount { get; set; }
        public double TotalAmount { get; set; }
        public double ServiceValueINR { get; set; }
        public double TaxAmountINR { get; set; }
        public double TotalAmountINR { get; set; }
        public string TermsAndCond { get; set; }
        public string Remarks { get; set; }
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

    }
    class wodetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string StockItemID { get; set; }
        public string Description { get; set; }
        public string WorkDescription { get; set; }
        public string WorkLocation { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Tax { get; set; }
        public int WarrantyDays { get; set; }
        public string TaxDetails { get; set; }
        public string TaxCode { get; set; }
    }
    class WorkOrderRequestDB
    {
        public List<woheader> getFilteredWorkOrderHeaders(string userList, int opt, string userCommentStatusString)
        {
            woheader woh;
            List<woheader> POPIHeaders = new List<woheader>();
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
                    " WORequestNo,WORequestDate,ReferenceInternalOrder,ProjectID,OfficeID,CustomerID,CustomerName,CurrencyID,CurrencyName,StartDate,TargetDate,PaymentTerms,PaymentMode," +
                    " POAddress,ServiceValue,TaxAmount,TotalAmount,TermsAndCondition,Remarks, " +
                    " Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,CommentStatus,ForwarderList " +
                    " ,ExchangeRate,ServiceValueINR,TaxAmountINR,TotalAmountINR from ViewWORequestHeader" +
                   " where ((forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (commentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query2 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " WORequestNo,WORequestDate,ReferenceInternalOrder,ProjectID,OfficeID,CustomerID,CustomerName,CurrencyID,CurrencyName,StartDate,TargetDate,PaymentTerms,PaymentMode," +
                    " POAddress,ServiceValue,TaxAmount,TotalAmount,TermsAndCondition,Remarks, " +
                    " Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName ,CommentStatus,ForwarderList " +
                     " ,ExchangeRate,ServiceValueINR,TaxAmountINR,TotalAmountINR from ViewWORequestHeader" +
                   " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query3 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " WORequestNo,WORequestDate,ReferenceInternalOrder,ProjectID,OfficeID,CustomerID,CustomerName,CurrencyID,CurrencyName,StartDate,TargetDate,PaymentTerms,PaymentMode," +
                    " POAddress,ServiceValue,TaxAmount,TotalAmount,TermsAndCondition,Remarks, " +
                    " Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,CommentStatus,ForwarderList  " +
                    " ,ExchangeRate,ServiceValueINR,TaxAmountINR,TotalAmountINR from ViewWORequestHeader" +
                   " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99)  order by WORequestDate desc,DocumentID asc,WORequestNo desc";
                string query6 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                   " WORequestNo,WORequestDate,ReferenceInternalOrder,ProjectID,OfficeID,CustomerID,CustomerName,CurrencyID,CurrencyName,StartDate,TargetDate,PaymentTerms,PaymentMode," +
                   " POAddress,ServiceValue,TaxAmount,TotalAmount,TermsAndCondition,Remarks, " +
                   " Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,CommentStatus,ForwarderList  " +
                   " ,ExchangeRate,ServiceValueINR,TaxAmountINR,TotalAmountINR from ViewWORequestHeader" +
                   " where  DocumentStatus = 99  order by WORequestDate desc,DocumentID asc,WORequestNo desc";
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
                    woh = new woheader();
                    woh.RowID = reader.GetInt32(0);
                    woh.DocumentID = reader.GetString(1);
                    woh.DocumentName = reader.GetString(2);
                    woh.TemporaryNo = reader.GetInt32(3);
                    woh.TemporaryDate = reader.GetDateTime(4);
                    woh.WORequestNo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        woh.WORequestDate = reader.GetDateTime(6);
                    }
                    if (!reader.IsDBNull(6))
                    {
                        woh.ReferenceInternalOrder = reader.GetString(7);
                    }
                    woh.ProjectID = reader.GetString(8);
                    woh.OfficeID = reader.GetString(9);
                    woh.CustomerID = reader.GetString(10);
                    woh.CustomerName = reader.GetString(11);
                    woh.CurrencyID = reader.GetString(12);
                    woh.CurrencyName = reader.GetString(13);
                    woh.StartDate = reader.GetDateTime(14);
                    woh.TargetDate = reader.GetDateTime(15);
                    woh.PaymentTerms = reader.GetString(16);
                    woh.PaymentMode = reader.GetString(17);
                    //woh.TaxCode = reader.GetString(18);
                    woh.POAddress = reader.GetString(18);
                    woh.ServiceValue = reader.GetDouble(19);
                    woh.TaxAmount = reader.GetDouble(20);
                    woh.TotalAmount = reader.GetDouble(21);
                    woh.TermsAndCond = reader.IsDBNull(22) ? " " : reader.GetString(22);
                    woh.Remarks = reader.GetString(23);
                    woh.Status = reader.GetInt32(24);
                    woh.DocumentStatus = reader.GetInt32(25);
                    woh.CreateTime = reader.GetDateTime(26);
                    woh.CreateUser = reader.GetString(27);
                    woh.ForwardUser = reader.GetString(28);
                    woh.ApproveUser = reader.GetString(29);
                    woh.CreatorName = reader.GetString(30);
                    woh.ForwarderName = reader.GetString(31);
                    woh.ApproverName = reader.GetString(32);
                    if (!reader.IsDBNull(33))
                    {
                        woh.CommentStatus = reader.GetString(33);
                    }
                    else
                    {
                        woh.CommentStatus = "";
                    }
                    if (!reader.IsDBNull(34))
                    {
                        woh.ForwarderList = reader.GetString(34);
                    }
                    else
                    {
                        woh.ForwarderList = "";
                    }
                    woh.ExchangeRate = reader.GetDecimal(35);
                    woh.ServiceValueINR = reader.GetDouble(36);
                    woh.TaxAmountINR = reader.GetDouble(37);
                    woh.TotalAmountINR = reader.GetDouble(38);
                    POPIHeaders.Add(woh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Work Order Header Details");
            }
            return POPIHeaders;
        }

        public static List<wodetail> getWorkOrderDetails(woheader woh)
        {
            wodetail wod;
            List<wodetail> WODetail = new List<wodetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select a.RowID,a.DocumentID,a.TemporaryNo, a.TemporaryDate,a.StockItemID,b.Description as Description,a.WorkDescription,a.WorkLocation, " +
                   "a.Quantity,a.Price,a.Tax,a.WarrantyDays,a.TaxDetails,a.TaxCode " +
                   "from WORequestDetail a , CatalogueValue b " +
                   "where a.StockItemID = b.CatalogueValueID and a.DocumentID='" + woh.DocumentID + "'" +
                   " and a.TemporaryNo=" + woh.TemporaryNo +
                   " and a.TemporaryDate='" + woh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    wod = new wodetail();
                    wod.RowID = reader.GetInt32(0);
                    wod.DocumentID = reader.GetString(1);
                    wod.TemporaryNo = reader.GetInt32(2);
                    wod.TemporaryDate = reader.GetDateTime(3).Date;
                    wod.StockItemID = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    wod.Description = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    wod.WorkDescription = reader.GetString(6);
                    wod.WorkLocation = reader.GetString(7);
                    wod.Quantity = reader.GetDouble(8);
                    wod.Price = reader.GetDouble(9);
                    wod.Tax = reader.GetDouble(10);
                    wod.WarrantyDays = reader.GetInt32(11);
                    wod.TaxDetails = reader.GetString(12);
                    wod.TaxCode = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    WODetail.Add(wod);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Work Order Details");
            }
            return WODetail;
        }
        public Boolean validateWORequestrHeader(woheader woh)
        {
            Boolean status = true;
            try
            {
                if (woh.DocumentID.Trim().Length == 0 || woh.DocumentID == null)
                {
                    return false;
                }
                if (woh.DocumentID != "INDENTSERVICE")
                {
                    if (woh.ProjectID.Trim().Length == 0 || woh.ProjectID == null)
                    {
                        return false;
                    }
                    if (woh.OfficeID.Trim().Length == 0 || woh.OfficeID == null)
                    {
                        return false;
                    }
                }
                if (woh.DocumentID == "INDENTSERVICE")
                {
                    if (woh.ReferenceInternalOrder.Trim().Length == 0 || woh.ReferenceInternalOrder == null)
                    {
                        return false;
                    }
                }
                if (woh.CustomerID.Trim().Length == 0 || woh.CustomerID == null)
                {
                    return false;
                }
                if (woh.CurrencyID.Trim().Length == 0 || woh.CurrencyID == null)
                {
                    return false;
                }

                //if (woh.TaxCode.Trim().Length == 0 || woh.TaxCode == null)
                //{
                //    return false;
                //}
                if (woh.StartDate == null)
                {
                    return false;
                }
                //if (woh.ReferenceInternalOrder == null)
                //{
                //    return false;
                //}
                if (woh.TargetDate < DateTime.Now.Date || woh.TargetDate < woh.StartDate || woh.TargetDate == null)
                {
                    return false;
                }
                if (woh.PaymentTerms == null)
                {
                    return false;
                }
                if (woh.PaymentMode == null)
                {
                    return false;
                }
                if (woh.POAddress.Trim().Length == 0 || woh.POAddress == null)
                {
                    return false;
                }
                if (woh.TermsAndCond.Trim().Length == 0 || woh.TermsAndCond == null)
                {
                    return false;
                }
                if (woh.ServiceValue == 0)
                {
                    return false;
                }
                if (woh.TotalAmount == 0)
                {
                    return false;
                }
                if (woh.ExchangeRate == 0)
                {
                    return false;
                }
                if (woh.Remarks.Trim().Length == 0 || woh.Remarks == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public Boolean forwardWorkOrder(woheader woh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WORequestHeader set DocumentStatus=" + (woh.DocumentStatus + 1) +
                     ", forwardUser='" + woh.ForwardUser + "'" +
                    ", commentStatus='" + woh.CommentStatus + "'" +
                    ", ForwarderList='" + woh.ForwarderList + "'" +
                    " where DocumentID='" + woh.DocumentID + "'" +
                    " and TemporaryNo=" + woh.TemporaryNo +
                    " and TemporaryDate='" + woh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WORequestHeader", "", updateSQL) +
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
        public Boolean reverseWO(woheader woh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WORequestHeader set DocumentStatus=" + woh.DocumentStatus +
                    ", forwardUser='" + woh.ForwardUser + "'" +
                    ", commentStatus='" + woh.CommentStatus + "'" +
                    ", ForwarderList='" + woh.ForwarderList + "'" +
                    " where DocumentID='" + woh.DocumentID + "'" +
                    " and TemporaryNo=" + woh.TemporaryNo +
                    " and TemporaryDate='" + woh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WORequestHeader", "", updateSQL) +
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
        public Boolean ApproveWorkOrder(woheader woh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WORequestHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + woh.CommentStatus + "'" +
                    ", WORequestNo=" + woh.WORequestNo +
                    ", WORequestDate=convert(date, getdate())" +
                    " where DocumentID='" + woh.DocumentID + "'" +
                    " and TemporaryNo=" + woh.TemporaryNo +
                    " and TemporaryDate='" + woh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WORequestHeader", "", updateSQL) +
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
        public static string GetWORequestNumbers(string IODetails)
        {
            //trackingdetails=2(dd-MM-yyyy)
            string WORequestDetails = "";

            try
            {
                string query = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " WORequestNo,WORequestDate,ReferenceInternalOrder,ProjectID,OfficeID,CustomerID,CustomerName,CurrencyID,CurrencyName,StartDate,TargetDate,PaymentTerms,PaymentMode," +
                    " BillingAddress,ServiceValue,TaxAmount,TotalAmount,TermsAndCondition,Remarks, " +
                    " Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName " +
                    " from ViewWORequestHeader" +
                   " where DocumentStatus = 99 and Status=1  and ReferenceInternalOrder like '%" + IODetails + "%'";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    WORequestDetails = WORequestDetails + reader.GetInt32(5) + " : " + reader.GetDateTime(6).ToString("dd-MM-yyyy") + " ; ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetWORequestNumbers() : Erroer");
            }
            return WORequestDetails;
        }
        public static string getUserComments(string docid, int tempno, DateTime tempdate)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from WORequestHeader where DocumentID='" + docid + "'" +
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
        public static List<woheader> getApprovedWorkOrderRequest()
        {
            woheader woh;
            List<woheader> WOHeaders = new List<woheader>();
            try
            {
                string query = "select WORequestNo,WORequestDate,CustomerID,CustomerName" +
                      " from ViewWORequestHeader" +
                   " where  DocumentStatus = 99 and status  = 1 and DocumentID = 'WORKORDERREQUEST'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    woh = new woheader();
                    woh.WORequestNo = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                    {
                        woh.WORequestDate = reader.GetDateTime(1);
                    }
                    woh.CustomerID = reader.GetString(2);
                    woh.CustomerName = reader.GetString(3);
                    WOHeaders.Add(woh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Work Order Header Details");
            }
            return WOHeaders;
        }
        public static ListView WorkOrderRequestListView()
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
                List<woheader> WOHeaderList = getApprovedWorkOrderRequest();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("WO Req No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("WO Req Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Cust ID", -2, HorizontalAlignment.Left);
                foreach (woheader woh in WOHeaderList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(woh.WORequestNo.ToString());
                    item1.SubItems.Add(woh.WORequestDate.ToShortDateString());
                    item1.SubItems.Add(woh.CustomerID + "-" + woh.CustomerName);
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }

        public Boolean updateWOHeaderAndDetail(woheader woh, woheader prevwoh, List<wodetail> WODetail)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WORequestHeader set TemporaryNo = " + woh.TemporaryNo +
                   ", TemporaryDate='" + woh.TemporaryDate.ToString("yyyy-MM-dd") +
                   "', WORequestNo=" + woh.WORequestNo +
                   ", WORequestDate='" + woh.WORequestDate.ToString("yyyy-MM-dd") +
                   "', ReferenceInternalOrder='" + woh.ReferenceInternalOrder +
                   "', ProjectID='" + woh.ProjectID +
                   "', OfficeID='" + woh.OfficeID +
                   "', CustomerID='" + woh.CustomerID +
                   "', CurrencyID='" + woh.CurrencyID +
                   "', ExchangeRate=" + woh.ExchangeRate +
                    ", StartDate='" + woh.StartDate.ToString("yyyy-MM-dd") +
                    "', TargetDate='" + woh.TargetDate.ToString("yyyy-MM-dd") +
                   "', PaymentTerms='" + woh.PaymentTerms +
                   "', PaymentMode='" + woh.PaymentMode +
                   "', POAddress='" + woh.POAddress +
                   "', ServiceValue=" + woh.ServiceValue +
                   ",TaxAmount=" + woh.TaxAmount + "," +
                   "TotalAmount= " + woh.TotalAmount +
                    ", ServiceValueINR=" + woh.ServiceValueINR +
                   ",TaxAmountINR=" + woh.TaxAmountINR + "," +
                   "TotalAmountINR= " + woh.TotalAmountINR +
                   ", TermsAndCondition ='" + woh.TermsAndCond +
                   "', Remarks ='" + woh.Remarks +
                   "', Status =" + woh.Status +
                   ", CommentStatus='" + woh.CommentStatus +
                   "', Comments='" + woh.Comments +
                   "', ForwarderList='" + woh.ForwarderList + "'" +
                  " where DocumentID='" + prevwoh.DocumentID + "'" +
                  " and TemporaryNo=" + prevwoh.TemporaryNo +
                  " and TemporaryDate='" + prevwoh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WORequestHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from WORequestDetail where DocumentID='" + prevwoh.DocumentID + "'" +
                     " and TemporaryNo=" + prevwoh.TemporaryNo +
                     " and TemporaryDate='" + prevwoh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "WORequestDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (wodetail wod in WODetail)
                {
                    updateSQL = "insert into WORequestDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,WorkDescription,TaxCode,WorkLocation,Quantity,Price,Tax,WarrantyDays,TaxDetails) " +
                    "values ('" + wod.DocumentID + "'," +
                    wod.TemporaryNo + "," +
                    "'" + wod.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       "'" + wod.StockItemID + "'," +
                    "'" + wod.WorkDescription + "'," +
                     "'" + wod.TaxCode + "'," +
                    "'" + wod.WorkLocation + "'," +
                    wod.Quantity + "," +
                    wod.Price + " ," +
                    wod.Tax + "," +
                    wod.WarrantyDays + "," +
                    "'" + wod.TaxDetails + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "WORequestDetail", "", updateSQL) +
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
        public Boolean InsertWOHeaderAndDetail(woheader woh, List<wodetail> WODetail)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                woh.TemporaryNo = DocumentNumberDB.getNumber(woh.DocumentID, 1);
                if (woh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + woh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + woh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into WORequestHeader " +
                    "(DocumentID,TemporaryNo,TemporaryDate,WORequestNo,WORequestDate,ReferenceInternalOrder,ProjectID,OfficeID,CustomerID,CurrencyID," +
                    "ExchangeRate,StartDate,TargetDate,PaymentTerms,PaymentMode,POAddress,ServiceValue,TaxAmount,TotalAmount,ServiceValueINR,TaxAmountINR,TotalAmountINR,TermsAndCondition," +
                    "Remarks,Status,DocumentStatus,CreateTime,CreateUser, CommentStatus,Comments,ForwarderList)" +
                    " values (" +
                    "'" + woh.DocumentID + "'," +
                    woh.TemporaryNo + "," +
                    "'" + woh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    woh.WORequestNo + "," +
                    "'" + woh.WORequestDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + woh.ReferenceInternalOrder + "'," +
                    "'" + woh.ProjectID + "'," +
                    "'" + woh.OfficeID + "'," +
                    "'" + woh.CustomerID + "'," +
                    "'" + woh.CurrencyID + "'," +
                    woh.ExchangeRate + "," +
                    "'" + woh.StartDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + woh.TargetDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + woh.PaymentTerms + "'," +
                    "'" + woh.PaymentMode + "'," +
                    "'" + woh.POAddress + "'," +
                    woh.ServiceValue + "," +
                    woh.TaxAmount + "," +
                    woh.TotalAmount + "," +
                     woh.ServiceValueINR + "," +
                    woh.TaxAmountINR + "," +
                    woh.TotalAmountINR + "," +
                      "'" + woh.TermsAndCond + "'," +
                    "'" + woh.Remarks + "'," +
                    woh.Status + "," +
                    woh.DocumentStatus + "," +
                     "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'," +
                    "'" + woh.CommentStatus + "'," +
                    "'" + woh.Comments + "'," +
                    "'" + woh.ForwarderList + "')";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "WORequestHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from WORequestDetail where DocumentID='" + woh.DocumentID + "'" +
                    " and TemporaryNo=" + woh.TemporaryNo +
                    " and TemporaryDate='" + woh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "WORequestDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (wodetail wod in WODetail)
                {
                    updateSQL = "insert into WORequestDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,WorkDescription,TaxCode,WorkLocation,Quantity,Price,Tax,WarrantyDays,TaxDetails) " +
                    "values ('" + wod.DocumentID + "'," +
                    woh.TemporaryNo + "," +
                    "'" + wod.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       "'" + wod.StockItemID + "'," +
                    "'" + wod.WorkDescription + "'," +
                        "'" + wod.TaxCode + "'," +
                    "'" + wod.WorkLocation + "'," +
                    wod.Quantity + "," +
                    wod.Price + " ," +
                    wod.Tax + "," +
                    wod.WarrantyDays + "," +
                    "'" + wod.TaxDetails + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "WORequestDetail", "", updateSQL) +
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
    }
}
