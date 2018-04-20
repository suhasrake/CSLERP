using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    public class pogeneralheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int PONo { get; set; }
        public DateTime PODate { get; set; }
        public string Reference { get; set; }
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

        public string DeliveryAddress { get; set; }
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
    }
    public class pogeneraldetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string ServiceItemID { get; set; }
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
    class PurchaseOrderGeneralDB
    {
        public List<pogeneralheader> getFilteredpogeneralheaders(string userList, int opt, string userCommentStatusString, string docrecvStr)
        {
            pogeneralheader poh;
            List<pogeneralheader> POHeaders = new List<pogeneralheader>();
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
                string columnsString = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " PONo,PODate,Reference,ProjectID,OfficeID,CustomerID,CustomerName,CurrencyID,CurrencyName,StartDate,TargetDate,PaymentTerms,PaymentMode," +
                    " DeliveryAddress,ProductValue,TaxAmount,TotalAmount,TermsAndCondition,Remarks, " +
                    " Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,CommentStatus,ForwarderList " +
                    ",ExchangeRate,ProductValueINR,TaxAmountINR,TotalAmountINR,SpecialNote  from ViewPOGeneral where ";
                //Doc Receiver list String
                string docRcvQry = "(" + docrecvStr + ")" + " and ";

                //Condition strings For query
                string condition = "";
                string cond1 = "  ((forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (commentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string cond2 = " ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string cond3 = " ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99 and Status = 1)  order by PODate desc,DocumentID asc,PONo desc";
                string cond6 = " status = 1 and DocumentStatus = 99  order by PODate desc,DocumentID asc,PONo desc";

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
                    poh = new pogeneralheader();
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
                    poh.Reference = reader.GetString(7);
                    poh.ProjectID = reader.GetString(8);
                    poh.OfficeID = reader.GetString(9);
                    poh.CustomerID = reader.GetString(10);
                    poh.CustomerName = reader.GetString(11);
                    poh.CurrencyID = reader.GetString(12);
                    poh.CurrencyName = reader.GetString(13);
                    poh.StartDate = reader.GetDateTime(14);
                    poh.TargetDate = reader.GetDateTime(15);
                    poh.PaymentTerms = reader.GetString(16);
                    poh.PaymentMode = reader.GetString(17);
                    //woh.TaxCode = reader.GetString(20);
                    poh.DeliveryAddress = reader.GetString(18);
                    poh.ServiceValue = reader.GetDouble(19);
                    poh.TaxAmount = reader.GetDouble(20);
                    poh.TotalAmount = reader.GetDouble(21);
                    poh.TermsAndCond = reader.IsDBNull(22) ? " " : reader.GetString(22);
                    poh.Remarks = reader.GetString(23);
                    poh.Status = reader.GetInt32(24);
                    poh.DocumentStatus = reader.GetInt32(25);
                    poh.CreateTime = reader.GetDateTime(26);
                    poh.CreateUser = reader.GetString(27);
                    poh.ForwardUser = reader.GetString(28);
                    poh.ApproveUser = reader.GetString(29);
                    poh.CreatorName = reader.GetString(30);
                    poh.ForwarderName = reader.GetString(31);
                    poh.ApproverName = reader.GetString(32);
                    if (!reader.IsDBNull(33))
                    {
                        poh.CommentStatus = reader.GetString(33);
                    }
                    else
                    {
                        poh.CommentStatus = "";
                    }
                    if (!reader.IsDBNull(34))
                    {
                        poh.ForwarderList = reader.GetString(34);
                    }
                    else
                    {
                        poh.ForwarderList = "";
                    }
                    poh.ExchangeRate = reader.GetDecimal(35);
                    poh.ServiceValueINR = reader.GetDouble(36);
                    poh.TaxAmountINR = reader.GetDouble(37);
                    poh.TotalAmountINR = reader.GetDouble(38);
                    poh.SpecialNote = reader.IsDBNull(39) ? "" : reader.GetString(39);
                    POHeaders.Add(poh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Purchase Order General Header Details");
            }
            return POHeaders;
        }

        public static List<pogeneraldetail> getpogeneraldetails(pogeneralheader poh)
        {
            pogeneraldetail wod;
            List<pogeneraldetail> WODetail = new List<pogeneraldetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select a.RowID,a.DocumentID,a.TemporaryNo, a.TemporaryDate,a.ServiceItemID,b.Name as Description,a.WorkDescription, " +
                   " a.Quantity,a.Price,a.Tax,a.WarrantyDays,a.TaxDetails,a.TaxCode " +
                   " from POGeneralDetail a ,ServiceItem b " +
                   " where a.ServiceItemID = b.ServiceItemID and a.DocumentID='" + poh.DocumentID + "'" +
                   " and a.TemporaryNo=" + poh.TemporaryNo +
                   " and a.TemporaryDate='" + poh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    wod = new pogeneraldetail();
                    wod.RowID = reader.GetInt32(0);
                    wod.DocumentID = reader.GetString(1);
                    wod.TemporaryNo = reader.GetInt32(2);
                    wod.TemporaryDate = reader.GetDateTime(3).Date;
                    wod.ServiceItemID = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    wod.Description = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    wod.WorkDescription = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    wod.Quantity = reader.GetDouble(7);
                    wod.Price = reader.GetDouble(8);
                    wod.Tax = reader.GetDouble(9);
                    wod.WarrantyDays = reader.GetInt32(10);
                    wod.TaxDetails = reader.GetString(11);
                    wod.TaxCode = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    WODetail.Add(wod);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Purchase Order Genera Details");
            }
            return WODetail;
        }

        public Boolean validatePORequestrHeader(pogeneralheader woh)
        {
            Boolean status = true;
            try
            {
                if (woh.DocumentID.Trim().Length == 0 || woh.DocumentID == null)
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
                if (woh.Reference.Trim().Length == 0 || woh.Reference == null)
                {
                    return false;
                }
                if (woh.StartDate == null)
                {
                    return false;
                }
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
                if (woh.DeliveryAddress.Trim().Length == 0 || woh.DeliveryAddress == null)
                {
                    return false;
                }
                //////if (woh.TermsAndCond.Trim().Length == 0 || woh.TermsAndCond == null)
                //////{
                //////    return false;
                //////}
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
        public Boolean forwardPurchaseOrderGen(pogeneralheader poh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POGeneralHeader set DocumentStatus=" + (poh.DocumentStatus + 1) +
                     ", forwardUser='" + poh.ForwardUser + "'" +
                    ", commentStatus='" + poh.CommentStatus + "'" +
                    ", ForwarderList='" + poh.ForwarderList + "'" +
                    " where DocumentID='" + poh.DocumentID + "'" +
                    " and TemporaryNo=" + poh.TemporaryNo +
                    " and TemporaryDate='" + poh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POGeneralHeader", "", updateSQL) +
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
        public Boolean reversePO(pogeneralheader poh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POGeneralHeader set DocumentStatus=" + poh.DocumentStatus +
                    ", forwardUser='" + poh.ForwardUser + "'" +
                    ", commentStatus='" + poh.CommentStatus + "'" +
                    ", ForwarderList='" + poh.ForwarderList + "'" +
                    " where DocumentID='" + poh.DocumentID + "'" +
                    " and TemporaryNo=" + poh.TemporaryNo +
                    " and TemporaryDate='" + poh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POGeneralHeader", "", updateSQL) +
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
        public Boolean ApprovePurchaseOrderGen(pogeneralheader poh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POGeneralHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + poh.CommentStatus + "'" +
                    ", PONo=" + poh.PONo +
                    ", PODate=convert(date, getdate())" +
                    " where DocumentID='" + poh.DocumentID + "'" +
                    " and TemporaryNo=" + poh.TemporaryNo +
                    " and TemporaryDate='" + poh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POGeneralHeader", "", updateSQL) +
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
                string query = "select comments from POGeneralHeader where DocumentID='" + docid + "'" +
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
        //public static Boolean checkAvailabilityOfWo(int WOReqNo, DateTime WOReqDate)
        //{
        //    Boolean status = true;
        //    int count = 0;
        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "select count(*) from WOHeader where WORequestNo=" + WOReqNo +
        //                " and WORequestDate='" + WOReqDate.ToString("yyyy-MM-dd") + "'";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            count = reader.GetInt32(0);
        //        }
        //        conn.Close();
        //        if (count != 0)
        //            status = false;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return status;
        //}
        //public List<pogeneralheader> getpogeneralheadersList()
        //{
        //    pogeneralheader woh;
        //    List<pogeneralheader> WOHeaders = new List<pogeneralheader>();
        //    try
        //    {
        //        string query = "select DocumentID,DocumentName,TemporaryNo,TemporaryDate," +
        //            " WONo,WODate,WORequestNo,WORequestDate,ReferenceInternalOrder,ProjectID,OfficeID,CustomerID,CustomerName from ViewWorkOrder where " +
        //            " status = 1 and DocumentStatus = 99 and WorkOrderStatus = 6 order by WORequestNo desc";
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            woh = new pogeneralheader();
        //            woh.DocumentID = reader.GetString(0);
        //            woh.DocumentName = reader.GetString(1);
        //            woh.TemporaryNo = reader.GetInt32(2);
        //            woh.TemporaryDate = reader.GetDateTime(3);
        //            woh.WONo = reader.GetInt32(4);
        //            if (!reader.IsDBNull(5))
        //            {
        //                woh.WODate = reader.GetDateTime(5);
        //            }
        //            woh.WORequestNo = reader.GetInt32(6);
        //            if (!reader.IsDBNull(7))
        //            {
        //                woh.WORequestDate = reader.GetDateTime(7);
        //            }
        //            if (!reader.IsDBNull(8))
        //            {
        //                woh.ReferenceInternalOrder = reader.GetString(8);
        //            }
        //            woh.ProjectID = reader.GetString(9);
        //            woh.OfficeID = reader.GetString(10);
        //            woh.CustomerID = reader.GetString(11);
        //            woh.CustomerName = reader.GetString(12);
        //            WOHeaders.Add(woh);
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error querying Work Order Header Details");
        //    }
        //    return WOHeaders;
        //}
        //public static ListView getWOHeaderListView()
        //{
        //    ListView lv = new ListView();
        //    try
        //    {

        //        lv.View = View.Details;
        //        lv.LabelEdit = true;
        //        lv.AllowColumnReorder = true;
        //        lv.CheckBoxes = true;
        //        lv.FullRowSelect = true;
        //        lv.GridLines = true;
        //        lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
        //        WorkOrderDB wodb = new WorkOrderDB();
        //        List<pogeneralheader> WOHeaders = wodb.getpogeneralheadersList();
        //        ////int index = 0;
        //        lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
        //        lv.Columns.Add("WO No", -2, HorizontalAlignment.Left);
        //        lv.Columns.Add("WO Date", -2, HorizontalAlignment.Left);
        //        lv.Columns.Add("Customer Name", -2, HorizontalAlignment.Center);

        //        foreach (pogeneralheader woh in WOHeaders)
        //        {
        //            ListViewItem item1 = new ListViewItem();
        //            item1.Checked = false;
        //            item1.SubItems.Add(woh.WONo.ToString());
        //            item1.SubItems.Add(woh.WODate.ToShortDateString());
        //            item1.SubItems.Add(woh.CustomerName);
        //            lv.Items.Add(item1);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return lv;
        //}
        //public static string getWODtlsForProjectTrans(string projectID)
        //{
        //    string str = "";
        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "select COUNT(*), SUM(TotalAmount) from WOHeader where ProjectID = '" + projectID + "' and DocumentStatus = 99";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            double dd = reader.IsDBNull(1) ? 0 : reader.GetDouble(1);
        //            str = reader.GetInt32(0) + "-" + dd;
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return str;
        //}
        //public static List<pogeneralheader> getRVINFOForProjectTrans(string projectID)
        //{
        //    pogeneralheader woh;
        //    List<pogeneralheader> WOHeaders = new List<pogeneralheader>();
        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "select WONo,WODate,CustomerName,ServiceValue,TaxAmount,TotalAmount,ProjectID from ViewWorkOrder where ProjectID = '" + projectID + "' and DocumentStatus = 99";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            woh = new pogeneralheader();
        //            woh.WONo = reader.GetInt32(0);
        //            woh.WODate = reader.GetDateTime(1);
        //            woh.CustomerName = reader.GetString(2);
        //            woh.ServiceValue = reader.GetDouble(3);
        //            woh.TaxAmount = reader.GetDouble(4);
        //            woh.TotalAmount = reader.GetDouble(5);
        //            woh.ProjectID = reader.GetString(6);
        //            WOHeaders.Add(woh);
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return WOHeaders;
        //}
        //public static pogeneralheader getTempNoAndDateOfWO(pogeneralheader wohTemp)
        //{
        //    pogeneralheader woh = new pogeneralheader(); ;
        //    List<pogeneralheader> WOHeaders = new List<pogeneralheader>();
        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "select DocumentID,TemporaryNo,TemporaryDate from WOHeader where" +
        //            " DocumentID = '" + wohTemp.DocumentID + "'" +
        //            " and WONo = " + wohTemp.WONo +
        //            " and WODate = '" + wohTemp.WODate.ToString("yyyy-MM-dd") + "'";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            woh.DocumentID = reader.GetString(0);
        //            woh.TemporaryNo = reader.GetInt32(1);
        //            woh.TemporaryDate = reader.GetDateTime(2);
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error in quering TempNo And Date of WO.");
        //    }
        //    return woh;
        //}
        public Boolean updatePOHeaderAndDetail(pogeneralheader poh, pogeneralheader prevpoh, List<pogeneraldetail> PODetail)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POGeneralHeader set TemporaryNo = " + poh.TemporaryNo +
                    ", TemporaryDate='" + poh.TemporaryDate.ToString("yyyy-MM-dd") +
                    "', Reference='" + poh.Reference +
                    "', ProjectID='" + poh.ProjectID +
                    "', OfficeID='" + poh.OfficeID +
                    "', CustomerID='" + poh.CustomerID +
                    "', CurrencyID='" + poh.CurrencyID +
                      "',ExchangeRate=" + poh.ExchangeRate + "," +
                     " StartDate='" + poh.StartDate.ToString("yyyy-MM-dd") +
                     "', TargetDate='" + poh.TargetDate.ToString("yyyy-MM-dd") +
                    "', PaymentTerms='" + poh.PaymentTerms +
                    "', PaymentMode='" + poh.PaymentMode +
                    "', DeliveryAddress='" + poh.DeliveryAddress +
                    "', ProductValue=" + poh.ServiceValue +
                    ",TaxAmount=" + poh.TaxAmount + "," +
                    "TotalAmount= " + poh.TotalAmount +
                      ", ProductValueINR=" + poh.ServiceValueINR +
                    ",TaxAmountINR=" + poh.TaxAmountINR + "," +
                    "TotalAmountINR= " + poh.TotalAmountINR +
                    ", TermsAndCondition ='" + poh.TermsAndCond +
                    "', Remarks ='" + poh.Remarks +
                    "', CommentStatus='" + poh.CommentStatus +
                    "', Comments='" + poh.Comments +
                     "', SpecialNote='" + poh.SpecialNote +
                    "', ForwarderList='" + poh.ForwarderList + "'" +
                   " where DocumentID='" + prevpoh.DocumentID + "'" +
                   " and TemporaryNo=" + prevpoh.TemporaryNo +
                   " and TemporaryDate='" + prevpoh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POGeneralHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from POGeneralDetail where DocumentID='" + prevpoh.DocumentID + "'" +
                    " and TemporaryNo=" + prevpoh.TemporaryNo +
                    " and TemporaryDate='" + prevpoh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "POGeneralDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (pogeneraldetail pod in PODetail)
                {
                    updateSQL = "insert into POGeneralDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,ServiceItemID,TaxCode,WorkDescription,Quantity,Price,Tax,WarrantyDays,TaxDetails) " +
                    "values ('" + pod.DocumentID + "'," +
                    pod.TemporaryNo + "," +
                    "'" + pod.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       "'" + pod.ServiceItemID + "'," +
                       "'" + pod.TaxCode + "'," +
                    "'" + pod.WorkDescription + "'," +
                    pod.Quantity + "," +
                    pod.Price + " ," +
                    pod.Tax + "," +
                    pod.WarrantyDays + "," +
                    "'" + pod.TaxDetails + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "POGeneralDetail", "", updateSQL) +
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
        public Boolean InsertPOHeaderAndDetail(pogeneralheader poh, List<pogeneraldetail> PODetail)
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

                updateSQL = "insert into POGeneralHeader " +
                     "(DocumentID,TemporaryNo,TemporaryDate,PONo,PODate,Reference,ProjectID,OfficeID,CustomerID,CurrencyID,ExchangeRate," +
                     "StartDate,TargetDate,PaymentTerms,PaymentMode,DeliveryAddress,ProductValue,TaxAmount,TotalAmount,ProductValueINR,TaxAmountINR,TotalAmountINR,TermsAndCondition," +
                     "Remarks,Status,DocumentStatus,CreateTime,CreateUser, CommentStatus,SpecialNote,Comments,ForwarderList)" +
                     " values (" +
                     "'" + poh.DocumentID + "'," +
                     poh.TemporaryNo + "," +
                     "'" + poh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       poh.PONo + "," +
                     "'" + poh.PODate.ToString("yyyy-MM-dd") + "'," +
                     "'" + poh.Reference + "'," +
                     "'" + poh.ProjectID + "'," +
                     "'" + poh.OfficeID + "'," +
                     "'" + poh.CustomerID + "'," +
                     "'" + poh.CurrencyID + "'," +
                     poh.ExchangeRate + "," +
                     "'" + poh.StartDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + poh.TargetDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + poh.PaymentTerms + "'," +
                     "'" + poh.PaymentMode + "'," +
                     "'" + poh.DeliveryAddress + "'," +
                     poh.ServiceValue + "," +
                     poh.TaxAmount + "," +
                     poh.TotalAmount + "," +
                        poh.ServiceValueINR + "," +
                     poh.TaxAmountINR + "," +
                     poh.TotalAmountINR + "," +
                    "'" + poh.TermsAndCond + "'," +
                     "'" + poh.Remarks + "'," +
                     poh.Status + "," +
                     poh.DocumentStatus + "," +
                      "GETDATE()" + "," +
                     "'" + Login.userLoggedIn + "'," +
                     "'" + poh.CommentStatus + "'," +
                      "'" + poh.SpecialNote + "'," +
                     "'" + poh.Comments + "'," +
                     "'" + poh.ForwarderList + "')";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "POGeneralHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from POGeneralDetail where DocumentID='" + poh.DocumentID + "'" +
                     " and TemporaryNo=" + poh.TemporaryNo +
                     " and TemporaryDate='" + poh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "POGeneralDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (pogeneraldetail pod in PODetail)
                {
                    updateSQL = "insert into POGeneralDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,ServiceItemID,TaxCode,WorkDescription,Quantity,Price,Tax,WarrantyDays,TaxDetails) " +
                    "values ('" + poh.DocumentID + "'," +
                    poh.TemporaryNo + "," +
                    "'" + poh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       "'" + pod.ServiceItemID + "'," +
                         "'" + pod.TaxCode + "'," +
                    "'" + pod.WorkDescription + "'," +
                    pod.Quantity + "," +
                    pod.Price + " ," +
                    pod.Tax + "," +
                    pod.WarrantyDays + "," +
                    "'" + pod.TaxDetails + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "POGeneralDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                //return false;
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

        public static ListView getPOHeaderListView()
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
                PurchaseOrderGeneralDB podb = new PurchaseOrderGeneralDB();
                List<pogeneralheader> POHeaders = podb.getPOHeadersList();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Cust ID", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Cust Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("InvoiceCount", -2, HorizontalAlignment.Center);
                lv.Columns[5].Width = 0;
                foreach (pogeneralheader po in POHeaders)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(po.PONo.ToString());
                    item1.SubItems.Add(po.PODate.ToShortDateString());
                    item1.SubItems.Add(po.CustomerID);
                    item1.SubItems.Add(po.CustomerName);
                    item1.SubItems.Add(po.WorkOrderStatus.ToString());
                    if (po.WorkOrderStatus != 0)
                        item1.BackColor = System.Drawing.Color.Tan;
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }

        public List<pogeneralheader> getPOHeadersList()
        {
            pogeneralheader pog;
            List<pogeneralheader> POGList = new List<pogeneralheader>();
            try
            {

                string query = "select a.PONo,a.PODate,a.CustomerID,b.Name, c.NoOfFound from POGeneralHeader a left outer join " +
                                " Customer b on a.CustomerID = b.CustomerID left outer join " +
                                " (select MRNNo,MRNDate,COUNT(*) NoOfFound from InvoiceInHeader where DocumentID = 'POGENERALINVOICEIN' group by MRNNo,MRNDate) c "+
                               " on a.PONo = c.MRNNo and a.PODate = c.MRNDate  where a.status = 1 and a.DocumentStatus = 99" +
                               " order by PONo asc";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pog = new pogeneralheader();
                    pog.PONo = reader.GetInt32(0);
                    pog.PODate = reader.GetDateTime(1);
                    pog.CustomerID = reader.GetString(2);
                    pog.CustomerName = reader.GetString(3);
                    pog.WorkOrderStatus = reader.IsDBNull(4) ? 0 : reader.GetInt32(4); //For invoice count
                    POGList.Add(pog);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Purchase Order Header Details");
            }
            return POGList;
        }


        public static ListView getPODetailListView(pogeneralheader po)
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
                pogeneralheader poh = PurchaseOrderGeneralDB.getTempNoAndDateOfPO(po);
                List<pogeneraldetail> PODetList = PurchaseOrderGeneralDB.getpogeneraldetails(poh);
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
                foreach (pogeneraldetail wod in PODetList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(wod.RowID.ToString());
                    item1.SubItems.Add(wod.ServiceItemID);
                    item1.SubItems.Add(wod.Description);
                    item1.SubItems.Add(wod.TaxCode);
                    item1.SubItems.Add(wod.WorkDescription);
                    item1.SubItems.Add("");
                    item1.SubItems.Add(wod.Quantity.ToString());
                    item1.SubItems.Add(wod.Price.ToString());
                    item1.SubItems.Add(PurchaseOrderGeneralDB.getItemWiseTotalQuantOFPOIssuedInvoiceIn(wod.RowID).ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }

        public static pogeneralheader getTempNoAndDateOfPO(pogeneralheader pohmain)
        {
            pogeneralheader woh = new pogeneralheader(); ;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID,TemporaryNo,TemporaryDate from POGeneralHeader where" +
                    " DocumentID = '" + pohmain.DocumentID + "'" +
                    " and PONo = " + pohmain.PONo +
                    " and PODate = '" + pohmain.PODate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    woh.DocumentID = reader.GetString(0);
                    woh.TemporaryNo = reader.GetInt32(1);
                    woh.TemporaryDate = reader.GetDateTime(2);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in quering TempNo And Date of PO.");
            }
            return woh;
        }
        public static double getItemWiseTotalQuantOFPOIssuedInvoiceIn(int refNo)
        {
            double TotQuant = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select SUM(a.Quantity) from InvoiceInDetail a , InvoiceInHeader b where a.DocumentID = b.DocumentID and " +
                    " a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate and a.DocumentID = 'POGENERALINVOICEIN' and a.ReferenceNo =" + refNo +
                    " and b.Status = 1 and b.DocumentStatus = 99";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    TotQuant = reader.IsDBNull(0) ? 0 : reader.GetDouble(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return TotQuant;
        }
        //get all invoice prepared again one POGeneral
        public static List<invoiceinheader> getInvoiceListAgainstOnePOGen(int poNo, DateTime podate)
        {
            invoiceinheader inh;
            List<invoiceinheader> InvoiceInHeaderList = new List<invoiceinheader>();
            try
            {
                string query = "select a.DocumentID,a.TemporaryNo, a.TemporaryDate,a.DocumentNo,a.DocumentDate,a.MRNNo, " +
                     "a.MRNDate, b.StockItemID, c.Name,b.Quantity,b.ReferenceNo from InvoiceInHeader a, InvoiceInDetail b, ServiceItem c" +
                    " where a.DocumentID = b.DocumentID and a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate " +
                    "  and b.StockItemID = c.ServiceItemID and a.DocumentID = 'POGENERALINVOICEIN'  and a.MRNNo = " + poNo +
                    " and a.MRNDate = '" + podate.ToString("yyyy-MM-dd") + "'" +
                    " and a.status = 1 and a.DocumentStatus = 99 order by a.DocumentNo desc";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        inh = new invoiceinheader();
                        inh.DocumentID = reader.GetString(0);
                        inh.TemporaryNo = reader.GetInt32(1);
                        inh.TemporaryDate = reader.GetDateTime(2);
                        inh.DocumentNo = reader.GetInt32(3);
                        inh.DocumentDate = reader.GetDateTime(4);
                        inh.MRNNo = reader.GetInt32(5);
                        inh.MRNDate = reader.GetDateTime(6);

                        inh.CreateUser = reader.GetString(7); //For StockItemID
                        inh.CreatorName = reader.GetString(8); //For StockItemName
                        inh.InvoiceValue = reader.GetDouble(9); //For WO Quant
                        inh.RowID = reader.GetInt32(10); //For WO RowId (Ref NO)
                        InvoiceInHeaderList.Add(inh);
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
            return InvoiceInHeaderList;
        }
        public static double getRefNoWiseQuantINPOGen(int refNo)
        {
            double Quant = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select Quantity from POGeneralDetail where RowID =" + refNo;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Quant = reader.GetDouble(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return Quant;
        }

        public List<pogeneralheader> getFilteredpogeneralheadersList(string Docid, int tempno, DateTime tempdate)
        {
            pogeneralheader poh;
            List<pogeneralheader> POHeaders = new List<pogeneralheader>();
            try
            {

                string query = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate, PONo,PODate,Reference," +
                               "ProjectID,OfficeID,CustomerID,CustomerName,CurrencyID,CurrencyName,StartDate,TargetDate,PaymentTerms," +
                               "PaymentMode, DeliveryAddress,ProductValue,TaxAmount,TotalAmount,TermsAndCondition,Remarks,Status,DocumentStatus," +
                               "CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,CommentStatus,ForwarderList " +
                               ",ExchangeRate,ProductValueINR,TaxAmountINR,TotalAmountINR,SpecialNote from ViewPOGeneral " +
                               "where DocumentID = '" + Docid + "' and TemporaryNo = " + tempno + " and TemporaryDate = '" + tempdate.ToString("yyyy-MM-dd") + "' and status = 1 and " +
                               "DocumentStatus = 99  order by PODate desc, DocumentID asc,PONo desc";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    poh = new pogeneralheader();
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
                    poh.Reference = reader.GetString(7);
                    poh.ProjectID = reader.GetString(8);
                    poh.OfficeID = reader.GetString(9);
                    poh.CustomerID = reader.GetString(10);
                    poh.CustomerName = reader.GetString(11);
                    poh.CurrencyID = reader.GetString(12);
                    poh.CurrencyName = reader.GetString(13);
                    poh.StartDate = reader.GetDateTime(14);
                    poh.TargetDate = reader.GetDateTime(15);
                    poh.PaymentTerms = reader.GetString(16);
                    poh.PaymentMode = reader.GetString(17);
                    //woh.TaxCode = reader.GetString(20);
                    poh.DeliveryAddress = reader.GetString(18);
                    poh.ServiceValue = reader.GetDouble(19);
                    poh.TaxAmount = reader.GetDouble(20);
                    poh.TotalAmount = reader.GetDouble(21);
                    poh.TermsAndCond = reader.IsDBNull(22) ? " " : reader.GetString(22);
                    poh.Remarks = reader.GetString(23);
                    poh.Status = reader.GetInt32(24);
                    poh.DocumentStatus = reader.GetInt32(25);
                    poh.CreateTime = reader.GetDateTime(26);
                    poh.CreateUser = reader.GetString(27);
                    poh.ForwardUser = reader.GetString(28);
                    poh.ApproveUser = reader.GetString(29);
                    poh.CreatorName = reader.GetString(30);
                    poh.ForwarderName = reader.GetString(31);
                    poh.ApproverName = reader.GetString(32);
                    if (!reader.IsDBNull(33))
                    {
                        poh.CommentStatus = reader.GetString(33);
                    }
                    else
                    {
                        poh.CommentStatus = "";
                    }
                    if (!reader.IsDBNull(34))
                    {
                        poh.ForwarderList = reader.GetString(34);
                    }
                    else
                    {
                        poh.ForwarderList = "";
                    }
                    poh.ExchangeRate = reader.GetDecimal(35);
                    poh.ServiceValueINR = reader.GetDouble(36);
                    poh.TaxAmountINR = reader.GetDouble(37);
                    poh.TotalAmountINR = reader.GetDouble(38);
                    poh.SpecialNote = reader.IsDBNull(39) ? "" : reader.GetString(39);
                    POHeaders.Add(poh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Purchase Order General Header Details");
            }
            return POHeaders;
        }
        public static pogeneraldetail getRefNoWisePriceINPOGen(int refNo)
        {
            pogeneraldetail det = new pogeneraldetail();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select Quantity,Price from POGeneralDetail where RowID =" + refNo;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    det.Quantity = reader.GetDouble(0);
                    det.Price = reader.GetDouble(1);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return det;
        }
        public static Boolean isInvoicePreparedForPOGeneral(int PONo, DateTime PODate)
        {
            Boolean isAvail = false;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select count(*) from InvoiceInHeader where DocumentID = 'POGENERALINVOICEIN'" +
                        " and MRNNo=" + PONo +
                        " and MRNDate='" + PODate.ToString("yyyy-MM-dd") + "'";
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
        public static pogeneralheader getPONOAndDateOFPOGen(pogeneralheader pohTemp)
        {
            pogeneralheader poh = new pogeneralheader(); ;
            //List<pogeneralheader> WOHeaders = new List<pogeneralheader>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID,PONo,PODate from POGeneralHeader where" +
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
        public Boolean ClosePOCheck(pogeneralheader POG)
        {
            Boolean status = false;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,DocumentID,PONo,PODate " +
                   "from POGeneralHeader  where PONo='" + POG.PONo + "' and PODate='" + POG.PODate.ToString("yyyy-MM-dd") + "' and Status = 7 ";

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

        public Boolean ClosePO(pogeneralheader poh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update POGeneralHeader set status=7 " +
                    " where PONo=" + poh.PONo +
                    " and PODate='" + poh.PODate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POGeneralHeader", "", updateSQL) +
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
