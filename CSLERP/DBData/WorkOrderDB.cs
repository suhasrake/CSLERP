using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    public class workorderheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int WONo { get; set; }
        public DateTime WODate { get; set; }
        public int WORequestNo { get; set; }
        public DateTime WORequestDate { get; set; }
        public string ReferenceInternalOrder { get; set; }
        public string ProjectID { get; set; }
        public string OfficeID { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public decimal ExchangeRate { get; set; }
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
        public int WorkOrderStatus { get; set; }
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
        public string ContractorReference { get; set; }
        public int InvoiceCount { get; set; }
    }
    public class workorderdetail
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
    class WorkOrderDB
    {
        public List<workorderheader> getFilteredWorkOrderHeaders(string userList, int opt, string userCommentStatusString, string docrecvStr)
        {
            workorderheader woh;
            List<workorderheader> WOHeaders = new List<workorderheader>();
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

                //Query String
                string query = "";
                //THis is column String For retriving from table
                string columnsString = "select a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate," +
                    " a.WONo,a.WODate,a.WORequestNo,a.WORequestDate,a.ReferenceInternalOrder,a.ProjectID,a.OfficeID,a.CustomerID,a.CustomerName,a.CurrencyID,a.CurrencyName,a.StartDate,a.TargetDate,a.PaymentTerms,a.PaymentMode," +
                    " a.POAddress,a.ServiceValue,a.TaxAmount,a.TotalAmount,a.TermsAndCondition,a.Remarks, " +
                    " a.Status,a.DocumentStatus,a.WorkOrderStatus,a.CreateTime,a.CreateUser,a.ForwardUser,a.ApproveUser,a.CreatorName,a.ForwarderName,a.ApproverName,a.CommentStatus,a.ForwarderList " +
                    ",a.ExchangeRate,a.ServiceValueINR,a.TaxAmountINR,a.TotalAmountINR,a.SpecialNote,b.NoFound,a.ContractorReference  from ViewWorkOrder a left outer join " +
                    " (select DocumentID,MRNNo,MRNDate,COUNT(*) as NoFound from InvoiceInHeader where Status = 1 and DocumentStatus = 99 group by DocumentID,MRNNo,MRNDate) b on " +
                    " a.WONo = b.MRNNo and a.WODate = b.MRNDate and b.DocumentID = 'WOINVOICEIN' where";
                //Doc Receiver list String
                string docRcvQry = "(" + docrecvStr + ")" + " and ";

                //Condition strings For query
                string condition = "";
                string cond1 = "  ((a.forwarduser='" + Login.userLoggedIn + "' and a.DocumentStatus between 2 and 98) " +
                    " or (a.createuser='" + Login.userLoggedIn + "' and a.DocumentStatus=1)" +
                    " or (a.commentStatus like '%" + userCommentStatusString + "%' and a.DocumentStatus between 1 and 98)) order by a.TemporaryDate desc,a.DocumentID asc,a.TemporaryNo desc";

                string cond2 = " ((a.createuser='" + Login.userLoggedIn + "'  and a.DocumentStatus between 2 and 98 ) " +
                    " or (a.ForwarderList like '%" + userList + "%' and a.DocumentStatus between 2 and 98 and a.ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (a.commentStatus like '%" + acStr + "%' and a.DocumentStatus between 1 and 98)) order by a.TemporaryDate desc,a.DocumentID asc,a.TemporaryNo desc";

                string cond3 = " ((a.createuser='" + Login.userLoggedIn + "'" +
                    " or a.ForwarderList like '%" + userList + "%'" +
                    " or a.commentStatus like '%" + acStr + "%'" +
                    " or a.approveUser='" + Login.userLoggedIn + "')" +
                    " and a.DocumentStatus = 99 and a.Status = 1)  order by a.WORequestDate desc,a.DocumentID asc,a.WORequestNo desc";
                string cond6 = " a.status = 1 and a.DocumentStatus = 99  order by a.WORequestDate desc,a.DocumentID asc,a.WORequestNo desc";

                SqlConnection conn = new SqlConnection(Login.connString);

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

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    woh = new workorderheader();
                    woh.RowID = reader.GetInt32(0);
                    woh.DocumentID = reader.GetString(1);
                    woh.DocumentName = reader.GetString(2);
                    woh.TemporaryNo = reader.GetInt32(3);
                    woh.TemporaryDate = reader.GetDateTime(4);
                    woh.WONo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        woh.WODate = reader.GetDateTime(6);
                    }
                    woh.WORequestNo = reader.GetInt32(7);
                    if (!reader.IsDBNull(8))
                    {
                        woh.WORequestDate = reader.GetDateTime(8);
                    }
                    if (!reader.IsDBNull(9))
                    {
                        woh.ReferenceInternalOrder = reader.GetString(9);
                    }
                    woh.ProjectID = reader.GetString(10);
                    woh.OfficeID = reader.GetString(11);
                    woh.CustomerID = reader.GetString(12);
                    woh.CustomerName = reader.GetString(13);
                    woh.CurrencyID = reader.GetString(14);
                    woh.CurrencyName = reader.GetString(15);
                    woh.StartDate = reader.GetDateTime(16);
                    woh.TargetDate = reader.GetDateTime(17);
                    woh.PaymentTerms = reader.GetString(18);
                    woh.PaymentMode = reader.GetString(19);
                    //woh.TaxCode = reader.GetString(20);
                    woh.POAddress = reader.GetString(20);
                    woh.ServiceValue = reader.GetDouble(21);
                    woh.TaxAmount = reader.GetDouble(22);
                    woh.TotalAmount = reader.GetDouble(23);
                    woh.TermsAndCond = reader.IsDBNull(24) ? "" : reader.GetString(24);
                    woh.Remarks = reader.GetString(25);
                    woh.Status = reader.GetInt32(26);
                    woh.DocumentStatus = reader.GetInt32(27);
                    woh.WorkOrderStatus = reader.GetInt32(28);
                    woh.CreateTime = reader.GetDateTime(29);
                    woh.CreateUser = reader.GetString(30);
                    woh.ForwardUser = reader.GetString(31);
                    woh.ApproveUser = reader.GetString(32);
                    woh.CreatorName = reader.GetString(33);
                    woh.ForwarderName = reader.GetString(34);
                    woh.ApproverName = reader.GetString(35);
                    if (!reader.IsDBNull(36))
                    {
                        woh.CommentStatus = reader.GetString(36);
                    }
                    else
                    {
                        woh.CommentStatus = "";
                    }
                    if (!reader.IsDBNull(37))
                    {
                        woh.ForwarderList = reader.GetString(37);
                    }
                    else
                    {
                        woh.ForwarderList = "";
                    }
                    woh.ExchangeRate = reader.GetDecimal(38);
                    woh.ServiceValueINR = reader.GetDouble(39);
                    woh.TaxAmountINR = reader.GetDouble(40);
                    woh.TotalAmountINR = reader.GetDouble(41);
                    woh.SpecialNote = reader.IsDBNull(42) ? "" : reader.GetString(42);
                    woh.InvoiceCount = reader.IsDBNull(43)? 0 : reader.GetInt32(43);
                    woh.ContractorReference = reader.IsDBNull(44) ? "" : reader.GetString(44);
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

        public static List<workorderdetail> getWorkOrderDetails(workorderheader woh)
        {
            workorderdetail wod;
            List<workorderdetail> WODetail = new List<workorderdetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select a.RowID,a.DocumentID,a.TemporaryNo, a.TemporaryDate,a.StockItemID,b.Name as Description,a.WorkDescription,a.WorkLocation, " +
                   " a.Quantity,a.Price,a.Tax,a.WarrantyDays,a.TaxDetails,a.TaxCode " +
                   " from WODetail a ,ServiceItem b " +
                   " where a.StockItemID = b.ServiceItemID and a.DocumentID='" + woh.DocumentID + "'" +
                   " and a.TemporaryNo=" + woh.TemporaryNo +
                   " and a.TemporaryDate='" + woh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    wod = new workorderdetail();
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

        public Boolean validateWORequestrHeader(workorderheader woh)
        {
            Boolean status = true;
            try
            {
                if (woh.DocumentID.Trim().Length == 0 || woh.DocumentID == null)
                {
                    return false;
                }

                if (woh.WORequestNo == 0)
                {
                    return false;
                }
                if (woh.WORequestDate == null)
                {
                    return false;
                }
                if (woh.CustomerID.Trim().Length == 0 || woh.CustomerID == null)
                {
                    return false;
                }
                if (woh.ProjectID.Trim().Length == 0 || woh.ProjectID == null)
                {
                    return false;
                }
                if (woh.OfficeID.Trim().Length == 0 || woh.OfficeID == null)
                {
                    return false;
                }
                if (woh.CurrencyID.Trim().Length == 0 || woh.CurrencyID == null)
                {
                    return false;
                }
                if (woh.ReferenceInternalOrder.Trim().Length == 0 || woh.ReferenceInternalOrder == null)
                {
                    return false;
                }
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
                if (woh.TotalAmountINR == 0)
                {
                    return false;
                }
                if (woh.ServiceValueINR == 0)
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
        public Boolean forwardWorkOrder(workorderheader woh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WOHeader set DocumentStatus=" + (woh.DocumentStatus + 1) +
                     ", forwardUser='" + woh.ForwardUser + "'" +
                    ", commentStatus='" + woh.CommentStatus + "'" +
                    ", ForwarderList='" + woh.ForwarderList + "'" +
                    " where DocumentID='" + woh.DocumentID + "'" +
                    " and TemporaryNo=" + woh.TemporaryNo +
                    " and TemporaryDate='" + woh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WOHeader", "", updateSQL) +
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
        public Boolean reverseWO(workorderheader woh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WOHeader set DocumentStatus=" + woh.DocumentStatus +
                    ", forwardUser='" + woh.ForwardUser + "'" +
                    ", commentStatus='" + woh.CommentStatus + "'" +
                    ", ForwarderList='" + woh.ForwarderList + "'" +
                    " where DocumentID='" + woh.DocumentID + "'" +
                    " and TemporaryNo=" + woh.TemporaryNo +
                    " and TemporaryDate='" + woh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WOHeader", "", updateSQL) +
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
        public Boolean ApproveWorkOrder(workorderheader woh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WOHeader set DocumentStatus=99, status=1, WorkOrderStatus = 1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + woh.CommentStatus + "'" +
                    ", WONo=" + woh.WONo +
                    ", WODate=convert(date, getdate())" +
                    " where DocumentID='" + woh.DocumentID + "'" +
                    " and TemporaryNo=" + woh.TemporaryNo +
                    " and TemporaryDate='" + woh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WOHeader", "", updateSQL) +
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
                string query = "select comments from WOHeader where DocumentID='" + docid + "'" +
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
        public static Boolean checkAvailabilityOfWo(int WOReqNo, DateTime WOReqDate)
        {
            Boolean status = true;
            int count = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select count(*) from WOHeader where WORequestNo=" + WOReqNo +
                        " and WORequestDate='" + WOReqDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    count = reader.GetInt32(0);
                }
                conn.Close();
                if (count != 0)
                    status = false;
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public List<workorderheader> getWorkOrderHeadersList()
        {
            workorderheader woh;
            List<workorderheader> WOHeaders = new List<workorderheader>();
            try
            {
                string query = "select a.WONo,a.WODate,a.CustomerID,b.Name,c.NoOfFound from WOHeader a left outer join" +
                    " Customer b on a.CustomerID = b.CustomerID left outer join " +
                    " (select MRNNo,MRNDate,COUNT(*) NoOfFound from InvoiceInHeader where DocumentID = 'WOINVOICEIN' group by MRNNo,MRNDate) c on "+
                    " a.WONo = c.MRNNo and a.WODate = c.MRNDate where  a.status = 1 and a.DocumentStatus = 99 and a.WorkOrderStatus > 0 order by WONo asc";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    woh = new workorderheader();
                    woh.WONo = reader.GetInt32(0);
                    woh.WODate = reader.GetDateTime(1);
                    woh.CustomerID = reader.GetString(2);
                    woh.CustomerName = reader.GetString(3);
                    woh.InvoiceCount = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
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
        public static ListView getWOHeaderListView()
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
                WorkOrderDB wodb = new WorkOrderDB();
                List<workorderheader> WOHeaders = wodb.getWorkOrderHeadersList();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("WO No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("WO Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Cust ID", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Cust Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("InvCount", -2, HorizontalAlignment.Center);
                lv.Columns[5].Width = 0;
                foreach (workorderheader woh in WOHeaders)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(woh.WONo.ToString());
                    item1.SubItems.Add(woh.WODate.ToShortDateString());
                    item1.SubItems.Add(woh.CustomerID);
                    item1.SubItems.Add(woh.CustomerName);
                    if (woh.InvoiceCount != 0)
                        item1.BackColor = System.Drawing.Color.Tan;
                    item1.SubItems.Add(woh.InvoiceCount.ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static string getWODtlsForProjectTrans(string projectID)
        {
            string str = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select COUNT(*), SUM(ServiceValueINR) from WOHeader where ProjectID = '" + projectID + "' and DocumentStatus = 99";
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
        public static List<workorderheader> getRVINFOForProjectTrans(string projectID)
        {
            workorderheader woh;
            List<workorderheader> WOHeaders = new List<workorderheader>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select WONo,WODate,CustomerName,ServiceValue,TaxAmount,TotalAmount,ProjectID from ViewWorkOrder where ProjectID = '" + projectID + "' and DocumentStatus = 99";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    woh = new workorderheader();
                    woh.WONo = reader.GetInt32(0);
                    woh.WODate = reader.GetDateTime(1);
                    woh.CustomerName = reader.GetString(2);
                    woh.ServiceValue = reader.GetDouble(3);
                    woh.TaxAmount = reader.GetDouble(4);
                    woh.TotalAmount = reader.GetDouble(5);
                    woh.ProjectID = reader.GetString(6);
                    WOHeaders.Add(woh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return WOHeaders;
        }
        public static workorderheader getTempNoAndDateOfWO(workorderheader wohTemp)
        {
            workorderheader woh = new workorderheader(); ;
            List<workorderheader> WOHeaders = new List<workorderheader>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID,TemporaryNo,TemporaryDate from WOHeader where" +
                    " DocumentID = '" + wohTemp.DocumentID + "'" +
                    " and WONo = " + wohTemp.WONo +
                    " and WODate = '" + wohTemp.WODate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    woh.DocumentID = reader.GetString(0);
                    woh.TemporaryNo = reader.GetInt32(1);
                    woh.TemporaryDate = reader.GetDateTime(2);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in quering TempNo And Date of WO.");
            }
            return woh;
        }
        public Boolean updateWOHeaderAndDetail(workorderheader woh, workorderheader prevwoh, List<workorderdetail> WODetail)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WOHeader set TemporaryNo = " + woh.TemporaryNo +
                    ", TemporaryDate='" + woh.TemporaryDate.ToString("yyyy-MM-dd") +
                    "', WORequestNo=" + woh.WORequestNo +
                    ", WORequestDate='" + woh.WORequestDate.ToString("yyyy-MM-dd") +
                    "', ReferenceInternalOrder='" + woh.ReferenceInternalOrder +
                    "', ProjectID='" + woh.ProjectID +
                    "', OfficeID='" + woh.OfficeID +
                    "', CustomerID='" + woh.CustomerID +
                    "', CurrencyID='" + woh.CurrencyID +
                      "',ExchangeRate=" + woh.ExchangeRate + "," +
                     " StartDate='" + woh.StartDate.ToString("yyyy-MM-dd") +
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
                       "', ContractorReference ='" + woh.ContractorReference +
                    "', CommentStatus='" + woh.CommentStatus +
                    "', Comments='" + woh.Comments +
                     "', SpecialNote='" + woh.SpecialNote +
                    "', ForwarderList='" + woh.ForwarderList + "'" +
                   " where DocumentID='" + prevwoh.DocumentID + "'" +
                   " and TemporaryNo=" + prevwoh.TemporaryNo +
                   " and TemporaryDate='" + prevwoh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WOHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from WODetail where DocumentID='" + prevwoh.DocumentID + "'" +
                    " and TemporaryNo=" + prevwoh.TemporaryNo +
                    " and TemporaryDate='" + prevwoh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "WODetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (workorderdetail wod in WODetail)
                {
                    updateSQL = "insert into WODetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,TaxCode,WorkDescription,WorkLocation,Quantity,Price,Tax,WarrantyDays,TaxDetails) " +
                    "values ('" + wod.DocumentID + "'," +
                    wod.TemporaryNo + "," +
                    "'" + wod.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       "'" + wod.StockItemID + "'," +
                       "'" + wod.TaxCode + "'," +
                    "'" + wod.WorkDescription + "'," +
                    "'" + wod.WorkLocation + "'," +
                    wod.Quantity + "," +
                    wod.Price + " ," +
                    wod.Tax + "," +
                    wod.WarrantyDays + "," +
                    "'" + wod.TaxDetails + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "WODetail", "", updateSQL) +
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
        public Boolean InsertWOHeaderAndDetail(workorderheader woh, List<workorderdetail> WODetail)
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

                updateSQL = "insert into WOHeader " +
                     "(DocumentID,TemporaryNo,TemporaryDate,WONo,WODate,WORequestNo,WORequestDate,ReferenceInternalOrder,ProjectID,OfficeID,CustomerID,CurrencyID,ExchangeRate," +
                     "StartDate,TargetDate,PaymentTerms,PaymentMode,POAddress,ServiceValue,TaxAmount,TotalAmount,ServiceValueINR,TaxAmountINR,TotalAmountINR,TermsAndCondition," +
                     "Remarks,Status,DocumentStatus,CreateTime,CreateUser, CommentStatus,Comments,ContractorReference,SpecialNote,ForwarderList)" +
                     " values (" +
                     "'" + woh.DocumentID + "'," +
                     woh.TemporaryNo + "," +
                     "'" + woh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       woh.WONo + "," +
                     "'" + woh.WODate.ToString("yyyy-MM-dd") + "'," +
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
                     "'" + woh.ContractorReference + "'," +
                     "'" + woh.SpecialNote + "'," +
                     "'" + woh.ForwarderList + "')";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "WOHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from WODetail where DocumentID='" + woh.DocumentID + "'" +
                     " and TemporaryNo=" + woh.TemporaryNo +
                     " and TemporaryDate='" + woh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "WODetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (workorderdetail wod in WODetail)
                {
                    updateSQL = "insert into WODetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,TaxCode,WorkDescription,WorkLocation,Quantity,Price,Tax,WarrantyDays,TaxDetails) " +
                    "values ('" + wod.DocumentID + "'," +
                    woh.TemporaryNo + "," +
                    "'" + wod.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       "'" + wod.StockItemID + "'," +
                         "'" + wod.TaxCode + "'," +
                    "'" + wod.WorkDescription + "'," +
                    "'" + wod.WorkLocation + "'," +
                    wod.Quantity + "," +
                    wod.Price + " ," +
                    wod.Tax + "," +
                    wod.WarrantyDays + "," +
                    "'" + wod.TaxDetails + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "WODetail", "", updateSQL) +
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
        public static ListView getWODetailListView(workorderheader woh)
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
                workorderheader wohMain = WorkOrderDB.getTempNoAndDateOfWO(woh);
                List<workorderdetail> WODetList = WorkOrderDB.getWorkOrderDetails(wohMain);
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("RefNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Item ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Item Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Tax Code", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Work Desc", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Work Loc", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Quantity", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Price", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Billed Quantity", -2, HorizontalAlignment.Left);
                foreach (workorderdetail wod in WODetList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(wod.RowID.ToString());
                    item1.SubItems.Add(wod.StockItemID);
                    item1.SubItems.Add(wod.Description);
                    item1.SubItems.Add(wod.TaxCode);
                    item1.SubItems.Add(wod.WorkDescription);
                    item1.SubItems.Add(wod.WorkLocation);
                    item1.SubItems.Add(wod.Quantity.ToString());
                    item1.SubItems.Add(wod.Price.ToString());
                    item1.SubItems.Add(InvoiceInHeaderDB.getItemWiseTotalQuantOFWOIssuedInvoiceIn(wod.RowID).ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static workorderdetail getRefNoWiseWODetail(int refNo)
        {
            workorderdetail wod = new workorderdetail();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select Quantity,Price from WODetail where RowID =" + refNo;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    wod.Quantity = reader.GetDouble(0);
                    wod.Price = reader.GetDouble(1);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return wod;
        }

        public static Boolean isInvoicePreparedForWO(int WONo, DateTime WODate)
        {
            Boolean isAvail = false;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select count(*) from InvoiceInHeader where DocumentID = 'WOINVOICEIN'" +
                        " and MRNNo=" + WONo +
                        " and MRNDate='" + WODate.ToString("yyyy-MM-dd") + "'";
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
        public static workorderheader getWONOAndDateOFWO(workorderheader wohTemp)
        {
            workorderheader woh = new workorderheader(); ;
            List<workorderheader> WOHeaders = new List<workorderheader>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID,WONo,WODate from WOHeader where" +
                    " DocumentID = '" + wohTemp.DocumentID + "'" +
                    " and TemporaryNo = " + wohTemp.TemporaryNo +
                    " and TemporaryDate = '" + wohTemp.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    woh.DocumentID = reader.GetString(0);
                    woh.WONo = reader.GetInt32(1);
                    woh.WODate = reader.GetDateTime(2);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in quering TempNo And Date of WO.");
            }
            return woh;
        }
        public List<workorderheader> getFilteredWorkOrderHeadersList(string DocID, int TempNo, DateTime tempdate)
        {
            workorderheader woh;
            List<workorderheader> WOHeaders = new List<workorderheader>();
            try
            {
                //THis is column String For retriving from table
                string query = "select a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate," +
                    " a.WONo,a.WODate,a.WORequestNo,a.WORequestDate,a.ReferenceInternalOrder,a.ProjectID,a.OfficeID,a.CustomerID,a.CustomerName,a.CurrencyID,a.CurrencyName,a.StartDate,a.TargetDate,a.PaymentTerms,a.PaymentMode," +
                    " a.POAddress,a.ServiceValue,a.TaxAmount,a.TotalAmount,a.TermsAndCondition,a.Remarks, " +
                    " a.Status,a.DocumentStatus,a.WorkOrderStatus,a.CreateTime,a.CreateUser,a.ForwardUser,a.ApproveUser,a.CreatorName,a.ForwarderName,a.ApproverName,a.CommentStatus,a.ForwarderList " +
                    ",a.ExchangeRate,a.ServiceValueINR,a.TaxAmountINR,a.TotalAmountINR,a.SpecialNote from ViewWorkOrder a  " +
                    "  where a.DocumentID='" + DocID + "' and a.TemporaryNo='" + TempNo + "' and a.TemporaryDate='" + tempdate.ToString("yyyy-MM-dd") + "' and " +
                    " a.status = 1 and a.DocumentStatus = 99  order by a.WORequestDate desc,a.DocumentID asc,a.WORequestNo desc";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    woh = new workorderheader();
                    woh.RowID = reader.GetInt32(0);
                    woh.DocumentID = reader.GetString(1);
                    woh.DocumentName = reader.GetString(2);
                    woh.TemporaryNo = reader.GetInt32(3);
                    woh.TemporaryDate = reader.GetDateTime(4);
                    woh.WONo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        woh.WODate = reader.GetDateTime(6);
                    }
                    woh.WORequestNo = reader.GetInt32(7);
                    if (!reader.IsDBNull(8))
                    {
                        woh.WORequestDate = reader.GetDateTime(8);
                    }
                    if (!reader.IsDBNull(9))
                    {
                        woh.ReferenceInternalOrder = reader.GetString(9);
                    }
                    woh.ProjectID = reader.GetString(10);
                    woh.OfficeID = reader.GetString(11);
                    woh.CustomerID = reader.GetString(12);
                    woh.CustomerName = reader.GetString(13);
                    woh.CurrencyID = reader.GetString(14);
                    woh.CurrencyName = reader.GetString(15);
                    woh.StartDate = reader.GetDateTime(16);
                    woh.TargetDate = reader.GetDateTime(17);
                    woh.PaymentTerms = reader.GetString(18);
                    woh.PaymentMode = reader.GetString(19);
                    //woh.TaxCode = reader.GetString(20);
                    woh.POAddress = reader.GetString(20);
                    woh.ServiceValue = reader.GetDouble(21);
                    woh.TaxAmount = reader.GetDouble(22);
                    woh.TotalAmount = reader.GetDouble(23);
                    woh.TermsAndCond = reader.IsDBNull(24) ? "" : reader.GetString(24);
                    woh.Remarks = reader.GetString(25);
                    woh.Status = reader.GetInt32(26);
                    woh.DocumentStatus = reader.GetInt32(27);
                    woh.WorkOrderStatus = reader.GetInt32(28);
                    woh.CreateTime = reader.GetDateTime(29);
                    woh.CreateUser = reader.GetString(30);
                    woh.ForwardUser = reader.GetString(31);
                    woh.ApproveUser = reader.GetString(32);
                    woh.CreatorName = reader.GetString(33);
                    woh.ForwarderName = reader.GetString(34);
                    woh.ApproverName = reader.GetString(35);
                    if (!reader.IsDBNull(36))
                    {
                        woh.CommentStatus = reader.GetString(36);
                    }
                    else
                    {
                        woh.CommentStatus = "";
                    }
                    if (!reader.IsDBNull(37))
                    {
                        woh.ForwarderList = reader.GetString(37);
                    }
                    else
                    {
                        woh.ForwarderList = "";
                    }
                    woh.ExchangeRate = reader.GetDecimal(38);
                    woh.ServiceValueINR = reader.GetDouble(39);
                    woh.TaxAmountINR = reader.GetDouble(40);
                    woh.TotalAmountINR = reader.GetDouble(41);
                    woh.SpecialNote = reader.IsDBNull(42) ? "" : reader.GetString(42);
                    //woh.InvoiceCount = reader.IsDBNull(43) ? 0 : reader.GetInt32(43);
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
        public Boolean CloseWOCheck(workorderheader WOh)
        {
            Boolean status = false;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,DocumentID,WONo,WODate " +
                   "from POHeader  where WONo='" + WOh.WONo + "' and WODate='" + WOh.WODate.ToString("yyyy-MM-dd") + "' and Status = 7 ";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    status = true;
                }
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return status;
        }

        public Boolean CloseWO(workorderheader woh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WOHeader set status=7 " +
                    " where WONo=" + woh.WONo +
                    " and WODate='" + woh.WODate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WOHeader", "", updateSQL) +
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
    }
}
