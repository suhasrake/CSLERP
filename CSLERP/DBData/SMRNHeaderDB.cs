using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    public class smrnheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TrackingNo { get; set; }
        public DateTime TrackingDate { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int SMRNNo { get; set; }
        public DateTime SMRNDate { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int Status { get; set; }
        public string CreateUser { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreateTime { get; set; }
        public int DocumentStatus { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string ForwarderName { get; set; }
        public string ApproverName { get; set; }
        public string CommentStatus { get; set; }
        public string Comments { get; set; }
        public string ForwarderList { get; set; }
        public int PreCheckStatus { get; set; }
    }
    public class smrndetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string SerialNo { get; set; }
        public string ItemDetails { get; set; }
        public int WarrantyStatus { get; set; }
        public int ServiceApprovalStatus { get; set; }
        public int InspectionStatus { get; set; }
        public string InspectionRemarks { get; set; }
        public int ServiceStatus { get; set; }
        public string ServiceRemarks { get; set; }
        public int JobIDNo { get; set; }
        public string ProductServiceStatus { get; set; }
    }
    class SMRNHeaderDB
    {
        public List<smrnheader> getFilteredSMRNHeader(string userList, int opt, string userCommentStatusString)
        {
            smrnheader smrnh;
            List<smrnheader> SMRNHeaders = new List<smrnheader>();
            string acStr = "";
            try
            {
                acStr = userCommentStatusString.Substring(0, userCommentStatusString.Length - 2) + "1" + Main.delimiter2;
            }
            catch (Exception ex)
            {
                acStr = "";
            }
            try
            {
                string query1 = "select RowID, DocumentID, DocumentName," +
                    " TemporaryNo, TemporaryDate, DocumentNo,DocumentDate,SMRNNo,SMRNDate,CustomerID,CustomerName," +
                    " CreateUser,CreateTime,ForwardUser, ApproveUser,CreatorName, ForwarderName, ApproverName, " +
                    "CommentStatus, ForwarderList , Status, DocumentStatus, PreCheckstatus, TrackingNo, TrackingDate " +
                    " from ViewSMRNHeader" +
                     " where ((ForwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (Createuser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (CommentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";
                string query2 = "select RowID, DocumentID, DocumentName," +
                    " TemporaryNo, TemporaryDate, DocumentNo,DocumentDate,SMRNNo,SMRNDate,CustomerID,CustomerName," +
                    " CreateUser,CreateTime,ForwardUser, ApproveUser,CreatorName, ForwarderName, ApproverName, " +
                    "CommentStatus, ForwarderList , Status, DocumentStatus, PreCheckstatus, TrackingNo, TrackingDate  " +
                    " from ViewSMRNHeader" +
                    " where ((Createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (CommentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";
                string query3 = "select RowID, DocumentID, DocumentName," +
                    " TemporaryNo, TemporaryDate, DocumentNo,DocumentDate,SMRNNo,SMRNDate,CustomerID,CustomerName," +
                    " CreateUser,CreateTime,ForwardUser, ApproveUser,CreatorName, ForwarderName, ApproverName, " +
                    "CommentStatus, ForwarderList , Status, DocumentStatus, PreCheckstatus, TrackingNo, TrackingDate  " +
                    " from ViewSMRNHeader" +
                    " where ((CreateUser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or CommentStatus like '%" + acStr + "%'" +
                    " or ApproveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99)  order by DocumentDate desc,DocumentID asc,DocumentNo desc";
                string query6 = "select RowID, DocumentID, DocumentName," +
                    " TemporaryNo, TemporaryDate, DocumentNo,DocumentDate,SMRNNo,SMRNDate,CustomerID,CustomerName," +
                    " CreateUser,CreateTime,ForwardUser, ApproveUser,CreatorName, ForwarderName, ApproverName, " +
                    "CommentStatus, ForwarderList , Status, DocumentStatus, PreCheckstatus, TrackingNo, TrackingDate  " +
                    " from ViewSMRNHeader" +
                    " where  DocumentStatus = 99  order by DocumentDate desc,DocumentID asc,DocumentNo desc";
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
                    smrnh = new smrnheader();
                    smrnh.RowID = reader.GetInt32(0);
                    smrnh.DocumentID = reader.GetString(1);
                    smrnh.DocumentName = reader.GetString(2);
                    smrnh.TemporaryNo = reader.GetInt32(3);
                    if (!reader.IsDBNull(4))
                    {
                        smrnh.TemporaryDate = reader.GetDateTime(4);
                    }
                    smrnh.DocumentNo = reader.GetInt32(5);
                    smrnh.DocumentDate = reader.GetDateTime(6);
                    smrnh.SMRNNo = reader.GetInt32(7);
                    smrnh.SMRNDate = reader.GetDateTime(8);
                    smrnh.CustomerID = reader.GetString(9);
                    smrnh.CustomerName = reader.GetString(10);
                    smrnh.CreateUser = reader.GetString(11);
                    smrnh.CreateTime = reader.GetDateTime(12);
                    smrnh.ForwardUser = reader.GetString(13);
                    smrnh.ApproveUser = reader.GetString(14);
                    smrnh.CreatorName = reader.GetString(15);
                    smrnh.ForwarderName = reader.GetString(16);
                    smrnh.ApproverName = reader.GetString(17);
                    smrnh.CommentStatus = reader.GetString(18);
                    smrnh.ForwarderList = reader.GetString(19);
                    smrnh.Status = reader.GetInt32(20);
                    smrnh.DocumentStatus = reader.GetInt32(21);
                    smrnh.PreCheckStatus = reader.GetInt32(22);
                    smrnh.TrackingNo = reader.GetInt32(23);
                    if (!reader.IsDBNull(24))
                    {
                        smrnh.TrackingDate = reader.GetDateTime(24);
                    }

                    SMRNHeaders.Add(smrnh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Quotation Inward Details");
            }
            return SMRNHeaders;
        }




        public static List<smrndetail> getSMRNDetail(smrnheader smrnh)
        {
            smrndetail smrnd;
            List<smrndetail> SMRNDetail = new List<smrndetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,DocumentName,TemporaryNo,TemporaryDate,StockItemID,StockItemName,ModelNo,ModelName, SerialNo, " +
                   "ItemDetails,WarrantyStatus,ServiceApprovalstatus,InspectionStatus,InspectionRemarks,ServiceStatus,ServiceRemarks,JobIDNo, ProductServiceStatus " +
                   "from ViewSMRNDetail where " +
                    " DocumentID='" + smrnh.DocumentID + "'" +
                    " and TemporaryNo=" + smrnh.TemporaryNo +
                    " and TemporaryDate='" + smrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                " order by StockItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    smrnd = new smrndetail();
                    smrnd.RowID = reader.GetInt32(0);
                    smrnd.DocumentID = reader.GetString(1);
                    smrnd.DocumentName = reader.GetString(2);
                    smrnd.TemporaryNo = reader.GetInt32(3);
                    smrnd.TemporaryDate = reader.GetDateTime(4).Date;
                    smrnd.StockItemID = reader.GetString(5);
                    smrnd.StockItemName = reader.GetString(6);
                    smrnd.ModelNo = reader.IsDBNull(7) ? "NA" : reader.GetString(7);
                    smrnd.ModelName = reader.IsDBNull(8) ? "NA" : reader.GetString(8);
                    smrnd.SerialNo = reader.GetString(9);
                    smrnd.ItemDetails = reader.GetString(10);
                    smrnd.WarrantyStatus = reader.GetInt32(11);
                    smrnd.ServiceApprovalStatus = reader.GetInt32(12);
                    smrnd.InspectionStatus = reader.GetInt32(13);
                    smrnd.InspectionRemarks = reader.GetString(14);
                    smrnd.ServiceStatus = reader.GetInt32(15);
                    smrnd.ServiceRemarks = reader.GetString(16);
                    smrnd.JobIDNo = reader.GetInt32(17);
                    smrnd.ProductServiceStatus = reader.GetString(18);
                    SMRNDetail.Add(smrnd);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying SMRN Details");
            }
            return SMRNDetail;
        }
        //--
        //public Boolean updateSMRNHeader(smrnheader smrnh)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "update SMRNHeader set SMRNNo=" + smrnh.SMRNNo +
        //             ",SMRNDate='" + smrnh.SMRNDate.ToString("yyyy-MM-dd") +
        //              "', CommentStatus='" + smrnh.CommentStatus +
        //              "', Comments='" + smrnh.Comments +
        //            "', ForwarderList='" + smrnh.ForwarderList + "'" +
        //           " where DocumentID='" + smrnh.DocumentID + "'" +
        //            " and TemporaryNo=" + smrnh.TemporaryNo +
        //            " and TemporaryDate='" + smrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("update", "SMRNHeader", "", updateSQL) +
        //        Main.QueryDelimiter;
        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        status = false;
        //    }
        //    return status;
        //}

        //public Boolean UpdateSMRNDetail(List<smrndetail> SMRNDetail, smrnheader smrnh)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "Delete from SMRNDetail where DocumentID='" + smrnh.DocumentID + "'" +
        //            " and TemporaryNo=" + smrnh.TemporaryNo +
        //            " and TemporaryDate='" + smrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //            ActivityLogDB.PrepareActivityLogQquerString("delete", "SMRNDetail", "", updateSQL) +
        //            Main.QueryDelimiter;
        //        foreach (smrndetail smrnd in SMRNDetail)
        //        {
        //            updateSQL = "insert into SMRNDetail " +
        //            "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,SerialNo,ItemDetails,WarrantyStatus,ServiceApprovalStatus,InspectionStatus" +
        //            ",InspectionRemarks, ServiceStatus, ServiceRemarks, JobIDNo, ProductServiceStatus) " +
        //            "values ('" + smrnd.DocumentID + "'," +
        //            smrnd.TemporaryNo + "," +
        //            "'" + smrnd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
        //            "'" + smrnd.StockItemID + "'," +
        //            "'" + smrnd.ModelNo + "'," +
        //            "'" + smrnd.SerialNo + "'," +
        //            "'" + smrnd.ItemDetails + "'," +
        //            smrnd.WarrantyStatus + "," +
        //            smrnd.ServiceApprovalStatus + "," +
        //            smrnd.InspectionStatus + "," +
        //            "'" + smrnd.InspectionRemarks + "'," +
        //            smrnd.ServiceStatus + "," +
        //            "'" + smrnd.ServiceRemarks + "'," + smrnd.JobIDNo + ",'" + smrnd.ProductServiceStatus + "')";
        //            utString = utString + updateSQL + Main.QueryDelimiter;
        //            utString = utString +
        //            ActivityLogDB.PrepareActivityLogQquerString("insert", "SMRNDetail", "", updateSQL) +
        //            Main.QueryDelimiter;
        //        }
        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = false;
        //    }
        //    return status;
        //}

        //public Boolean insertSMRNHeader(smrnheader smrnh)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "insert into SMRNHeader " +
        //            "(DocumentID,TemporaryNo, TemporaryDate,DocumentNo,DocumentDate," +
        //            "SMRNNo,SMRNDate,Status,DocumentStatus,CreateUser,CreateTime," +
        //            "Comments,ForwarderList,CommentStatus)" +

        //            " values (" +
        //            "'" + smrnh.DocumentID + "'," +
        //            smrnh.TemporaryNo + "," +
        //            "'" + smrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
        //            smrnh.DocumentNo + "," +
        //            "'" + smrnh.DocumentDate.ToString("yyyy-MM-dd") + "'," +
        //            +smrnh.SMRNNo + "," +
        //            "'" + smrnh.SMRNDate.ToString("yyyy-MM-dd") + "'," +
        //            smrnh.Status + "," +
        //            smrnh.DocumentStatus + "," +
        //            "'" + Login.userLoggedIn + "'," +
        //            "GETDATE()" +
        //            ",'" + smrnh.Comments + "'," +
        //            "'" + smrnh.ForwarderList + "'," +
        //            "'" + smrnh.CommentStatus + "')";



        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("insert", "SMRNHeader", "", updateSQL) +
        //        Main.QueryDelimiter;
        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        status = false;
        //    }
        //    return status;
        //}
        //public Boolean deleteSMRNHeader(smrnheader smrnh)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "delete SMRNHeader where DocumentID='" + smrnh.DocumentID + "'" +
        //            " where DocumentID='" + smrnh.DocumentID + "'" +
        //            " and TemporaryNo=" + smrnh.TemporaryNo +
        //            " and TemporaryDate='" + smrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("delete", "SMRNHeader", "", updateSQL) +
        //        Main.QueryDelimiter;
        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        status = false;
        //    }
        //    return status;
        //}

        public Boolean validateSMRNHeader(smrnheader smrnh)
        {
            Boolean status = true;
            try
            {

                if (smrnh.SMRNNo == 0)
                {
                    return false;
                }
                if (smrnh.SMRNDate == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public static List<smrnheader> getSMRNHeader(int opt)
        {
            smrnheader smrnh;
            List<smrnheader> SMRNheader = new List<smrnheader>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query1 = "select TemporaryNo,TemporaryDate,SMRNNo,SMRNDate, CustomerID " +
                   "from ViewSMRNHeader where PreCheckStatus = 1 and DocumentStatus <> 99 and Status <> 1";
                string query2 = "select Distinct(a.TemporaryNo),a.TemporaryDate,a.SMRNNo,a.SMRNDate, c.CustomerID from SMRNHeader a, SMRNDetail b,SMRN c " +
                   "where a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate and a.SMRNNo = c.SMRNNo " +
                   "and a.SMRNDate = c.SMRNDate and a.DocumentStatus = 99 and a.Status = 1 and b.ServiceApprovalStatus = 1" +
                   " and b.ServiceStatus <> 1";
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
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    smrnh = new smrnheader();
                    smrnh.TemporaryNo = reader.GetInt32(0);
                    smrnh.TemporaryDate = reader.GetDateTime(1).Date;
                    smrnh.SMRNNo = reader.GetInt32(2);
                    smrnh.SMRNDate = reader.GetDateTime(3).Date;
                    smrnh.CustomerID = reader.GetString(4);
                    SMRNheader.Add(smrnh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying SMRN Details");
            }
            return SMRNheader;
        }
        public static ListView getSMRNHeaderNoListView(int opt)
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

                List<smrnheader> SMRNHList = getSMRNHeader(opt);
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SMRNHTempNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SMRNHTempDate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SRMNNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SMRNdate", -2, HorizontalAlignment.Center);
                lv.Columns.Add("CustomerID", -2, HorizontalAlignment.Center);
                foreach (smrnheader smrnh in SMRNHList)
                {
                    {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        item1.SubItems.Add(smrnh.TemporaryNo.ToString());
                        item1.SubItems.Add(smrnh.TemporaryDate.ToShortDateString());
                        item1.SubItems.Add(smrnh.SMRNNo.ToString());
                        item1.SubItems.Add(smrnh.SMRNDate.ToShortDateString());
                        item1.SubItems.Add(smrnh.CustomerID.ToString());
                        lv.Items.Add(item1);
                    }
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static List<smrndetail> getSMRNDetailForReporting(smrnheader smrnh, int opt)
        {
            smrndetail smrnd;
            List<smrndetail> SMRNDetail = new List<smrndetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query1 = "Select StockItemID,StockItemName, SerialNo, " +
                   "ItemDetails,InspectionStatus,InspectionRemarks,ServiceStatus,ServiceRemarks,JobIDNo " +
                   "from ViewSMRNDetail where " +
                     " InspectionStatus = 0 and ServiceStatus = 0 and" +
                    " DocumentID='" + smrnh.DocumentID + "'" +
                    " and TemporaryNo=" + smrnh.TemporaryNo +
                    " and TemporaryDate='" + smrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                " order by StockItemID";
                string query2 = "select StockItemID,StockItemName, SerialNo, " +
                   "ItemDetails,InspectionStatus,InspectionRemarks,ServiceStatus,ServiceRemarks,JobIDNo " +
                   "from ViewSMRNDetail where " +
                    " InspectionStatus = 1 and ServiceStatus = 0 and ServiceApprovalStatus = 1 and" +
                    " DocumentID='" + smrnh.DocumentID + "'" +
                    " and TemporaryNo=" + smrnh.TemporaryNo +
                    " and TemporaryDate='" + smrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                " order by StockItemID";
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
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    smrnd = new smrndetail();
                    smrnd.StockItemID = reader.GetString(0);
                    smrnd.StockItemName = reader.GetString(1);
                    smrnd.SerialNo = reader.GetString(2);
                    smrnd.ItemDetails = reader.GetString(3);
                    smrnd.InspectionStatus = reader.GetInt32(4);
                    smrnd.InspectionRemarks = reader.GetString(5);
                    smrnd.ServiceStatus = reader.GetInt32(6);
                    smrnd.ServiceRemarks = reader.GetString(7);
                    smrnd.JobIDNo = reader.GetInt32(8);
                    SMRNDetail.Add(smrnd);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying SMRN Details");
            }
            return SMRNDetail;
        }
        public static ListView getSMRNJOBIDNoListView(int TempNo, DateTime TempDate, int ReportType)
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
                smrnheader smrnh = new smrnheader();
                smrnh.TemporaryNo = TempNo;
                smrnh.TemporaryDate = TempDate;
                smrnh.DocumentID = "SMRNHEADER";
                List<smrndetail> SMRNDetailList = getSMRNDetailForReporting(smrnh, ReportType);
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("JobIDNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItemID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItemName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SerielNo", -2, HorizontalAlignment.Center);
                lv.Columns.Add("ItemDetail", -2, HorizontalAlignment.Center);
                foreach (smrndetail smrnd in SMRNDetailList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(smrnd.JobIDNo.ToString());
                    item1.SubItems.Add(smrnd.StockItemID);
                    item1.SubItems.Add(smrnd.StockItemName.ToString());
                    item1.SubItems.Add(smrnd.SerialNo.ToString());
                    item1.SubItems.Add(smrnd.ItemDetails.ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static string getUserComments(string docid, int dno, DateTime ddate)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from SMRNHeader where DocumentID='" + docid + "'" +
                       " and TemporaryNo=" + dno +
                    " and TemporaryDate='" + ddate.ToString("yyyy-MM-dd") + "'";
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
        public Boolean forwardSMRN(smrnheader smrnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update SMRNHeader set DocumentStatus=" + (smrnh.DocumentStatus + 1) +
                    ", ForwardUser='" + smrnh.ForwardUser + "'" +
                    ", CommentStatus='" + smrnh.CommentStatus + "'" +
                    ", ForwarderList='" + smrnh.ForwarderList + "'" +
                   " where DocumentID='" + smrnh.DocumentID + "'" +
                    " and TemporaryNo=" + smrnh.TemporaryNo +
                    " and TemporaryDate='" + smrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "SMRNHeader", "", updateSQL) +
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
        public Boolean finalizeSMRN(smrnheader smrnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update SMRNHeader set PreCheckStatus= " + smrnh.PreCheckStatus +
                   " where DocumentID='" + smrnh.DocumentID + "'" +
                    " and TemporaryNo=" + smrnh.TemporaryNo +
                    " and TemporaryDate='" + smrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "SMRNHeader", "", updateSQL) +
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

        public Boolean reverseSMRNH(smrnheader smrnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update SMRNHeader set DocumentStatus=" + smrnh.DocumentStatus +
                    ", forwardUser='" + smrnh.ForwardUser + "'" +
                    ", commentStatus='" + smrnh.CommentStatus + "'" +
                    ", ForwarderList='" + smrnh.ForwarderList + "'" +
                    " where DocumentID='" + smrnh.DocumentID + "'" +
                    " and TemporaryNo=" + smrnh.TemporaryNo +
                    " and TemporaryDate='" + smrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "SMRNHeader", "", updateSQL) +
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

        public Boolean ApproveSMRNH(smrnheader smrnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update SMRNHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + smrnh.CommentStatus + "'" +
                     ", TrackingNo=" + smrnh.TrackingNo +
                    ", TrackingDate ='" + smrnh.TrackingDate.ToString("yyyy - MM - dd") + "'" +
                    ", DocumentNo=" + smrnh.DocumentNo +
                    ", DocumentDate = convert(date, getdate())" +
                     " where DocumentID='" + smrnh.DocumentID + "'" +
                    " and TemporaryNo=" + smrnh.TemporaryNo +
                    " and TemporaryDate='" + smrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "SMRNHeader", "", updateSQL) +
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
        public static List<productservicereportdetail> getPSRDetailForReport(smrnheader smrnh, int opt, int jobNo)
        {
            productservicereportdetail prsd;
            string docID = "";
            if (opt == 1)
                docID = "PSRPRELIMINARY";
            else
                docID = "PSRFINAL";
            List<productservicereportdetail> PSRDetails = new List<productservicereportdetail>();
            try
            {
                string query = "select RowID, DocumentID, DocumentName," +
                    " TemporaryNo, TemporaryDate,ReportType,ReportNo,ReportDate,SMRNHeaderTempNo," +
                    "SMRNHeaderTempDate, SMRNHeaderNo,SMRNHeaderDate,CustomerID,Name, SMRNNo,SMRNDate,JobIDNo," +
                    " CreateUser,CreatorName,TestDescriptionID,TestResult,TestRemarks " +
                    " from ViewProductServiceReportDetail" +
                    " where SMRNHeaderTempNo = " + smrnh.TemporaryNo +
                    " and SMRNHeaderTempDate = '" + smrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                    " and SMRNNo = " + smrnh.SMRNNo +
                    " and SMRNDate = '" + smrnh.SMRNDate.ToString("yyyy-MM-dd") + "'" +
                    " and JobIDNo = " + jobNo +
                    " and DocumentID = '" + docID + "'";

                SqlConnection conn = new SqlConnection(Login.connString);

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prsd = new productservicereportdetail();
                    prsd.RowID = reader.GetInt32(0);
                    prsd.DocumentID = reader.GetString(1);
                    prsd.DocumentName = reader.GetString(2);
                    prsd.TemporaryNo = reader.GetInt32(3);
                    if (!reader.IsDBNull(4))
                    {
                        prsd.TemporaryDate = reader.GetDateTime(4);
                    }
                    prsd.ReportType = reader.GetInt32(5);
                    prsd.ReportNo = reader.GetInt32(6);
                    prsd.ReportDate = reader.GetDateTime(7);
                    prsd.SMRNHeaderTempNo = reader.GetInt32(8);
                    prsd.SMRNHeaderTempDate = reader.GetDateTime(9);
                    prsd.SMRNHeaderNo = reader.GetInt32(10);
                    prsd.SMRNHeaderDate = reader.GetDateTime(11);

                    prsd.CustomerID = reader.GetString(12);
                    prsd.CustomerName = reader.GetString(13);
                    prsd.SMRNNo = reader.GetInt32(14);
                    prsd.SMRNDate = reader.GetDateTime(15);
                    prsd.jobIDNo = reader.GetInt32(16);
                    prsd.CreateUser = reader.GetString(17);
                    prsd.CreatorName = reader.GetString(18);
                    prsd.TestDescriptionID = reader.GetString(19);
                    prsd.TestResult = reader.GetString(20);
                    prsd.TestRemarks = reader.GetString(21);
                    PSRDetails.Add(prsd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Quotation Inward Details");
            }
            return PSRDetails;
        }
        public static List<popiheader> getTrackingNoForserviceLsit()
        {
            popiheader popih;
            List<popiheader> POPIList = new List<popiheader>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select TrackingNo,TrackingDate,CustomerID,CustomerName,CustomerPONO,CustomerPODate " +
                    "from ViewPOProductInwardHeader where Status = 1 and DocumentStatus = 99 and TrackingNo in (select TrackingNo from SMRNHeader)";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popih = new popiheader();
                    popih.TrackingNo = reader.GetInt32(0);
                    popih.TrackingDate = reader.GetDateTime(1);
                    popih.CustomerID = reader.GetString(2);
                    popih.CustomerName = reader.GetString(3);
                    popih.CustomerPONO = reader.GetString(4);
                    popih.CustomerPODate = reader.GetDateTime(5);
                    POPIList.Add(popih);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying SMRN Details");
            }
            return POPIList;
        }
        public static ListView getTempNoForServiceListView()
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

                List<popiheader> POPIHList = getTrackingNoForserviceLsit();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Tracking No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Tracking Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Customer ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Customer Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Cust PO No", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Cust PO Date", -2, HorizontalAlignment.Center);
                foreach (popiheader popih in POPIHList)
                {
                    {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        item1.SubItems.Add(popih.TrackingNo.ToString());
                        item1.SubItems.Add(popih.TrackingDate.ToShortDateString());
                        item1.SubItems.Add(popih.CustomerID.ToString());
                        item1.SubItems.Add(popih.CustomerName);
                        item1.SubItems.Add(popih.CustomerPONO);
                        item1.SubItems.Add(popih.CustomerPODate.ToShortDateString());
                        lv.Items.Add(item1);
                    }
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static ListView getLVServiceForStockIssue()
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
                SMRNHeaderDB smrndb = new SMRNHeaderDB();
                List<smrnheader> SMRNHList = smrndb.getFilteredSMRNHeader("", 6, "");
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Document No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Document Date", -2, HorizontalAlignment.Center);
                lv.Columns.Add("CustomerId", -2, HorizontalAlignment.Left);
                lv.Columns.Add("CustomerName", -2, HorizontalAlignment.Center);
                foreach (smrnheader smrnh in SMRNHList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(smrnh.DocumentNo.ToString());
                    item1.SubItems.Add(smrnh.DocumentDate.ToShortDateString());
                    item1.SubItems.Add(smrnh.CustomerID);
                    item1.SubItems.Add(smrnh.CustomerName);
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public Boolean updateSMRNHeaderAndDetail( smrnheader smrnh, smrnheader prevsmrnh, List<smrndetail> SMRNDetail)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update SMRNHeader set SMRNNo=" + smrnh.SMRNNo +
                    ",SMRNDate='" + smrnh.SMRNDate.ToString("yyyy-MM-dd") +
                     "', CommentStatus='" + smrnh.CommentStatus +
                     "', Comments='" + smrnh.Comments +
                   "', ForwarderList='" + smrnh.ForwarderList + "'" +
                  " where DocumentID='" + prevsmrnh.DocumentID + "'" +
                   " and TemporaryNo=" + prevsmrnh.TemporaryNo +
                   " and TemporaryDate='" + prevsmrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "SMRNHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from SMRNDetail where DocumentID='" + prevsmrnh.DocumentID + "'" +
                    " and TemporaryNo=" + prevsmrnh.TemporaryNo +
                    " and TemporaryDate='" + prevsmrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "SMRNDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (smrndetail smrnd in SMRNDetail)
                {
                    updateSQL = "insert into SMRNDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,SerialNo,ItemDetails,WarrantyStatus,ServiceApprovalStatus,InspectionStatus" +
                    ",InspectionRemarks, ServiceStatus, ServiceRemarks, JobIDNo, ProductServiceStatus) " +
                    "values ('" + smrnd.DocumentID + "'," +
                    smrnd.TemporaryNo + "," +
                    "'" + smrnd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + smrnd.StockItemID + "'," +
                    "'" + smrnd.ModelNo + "'," +
                    "'" + smrnd.SerialNo + "'," +
                    "'" + smrnd.ItemDetails + "'," +
                    smrnd.WarrantyStatus + "," +
                    smrnd.ServiceApprovalStatus + "," +
                    smrnd.InspectionStatus + "," +
                    "'" + smrnd.InspectionRemarks + "'," +
                    smrnd.ServiceStatus + "," +
                    "'" + smrnd.ServiceRemarks + "'," + smrnd.JobIDNo + ",'" + smrnd.ProductServiceStatus + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "SMRNDetail", "", updateSQL) +
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
        public Boolean InsertSMRNHeaderAndDetail(smrnheader smrnh, List<smrndetail> SMRNDetail)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                smrnh.TemporaryNo = DocumentNumberDB.getNumber(smrnh.DocumentID, 1);
                if (smrnh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + smrnh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + smrnh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into SMRNHeader " +
                     "(DocumentID,TemporaryNo, TemporaryDate,DocumentNo,DocumentDate," +
                     "SMRNNo,SMRNDate,Status,DocumentStatus,CreateUser,CreateTime," +
                     "Comments,ForwarderList,CommentStatus)" +

                     " values (" +
                     "'" + smrnh.DocumentID + "'," +
                     smrnh.TemporaryNo + "," +
                     "'" + smrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     smrnh.DocumentNo + "," +
                     "'" + smrnh.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                     +smrnh.SMRNNo + "," +
                     "'" + smrnh.SMRNDate.ToString("yyyy-MM-dd") + "'," +
                     smrnh.Status + "," +
                     smrnh.DocumentStatus + "," +
                     "'" + Login.userLoggedIn + "'," +
                     "GETDATE()" +
                     ",'" + smrnh.Comments + "'," +
                     "'" + smrnh.ForwarderList + "'," +
                     "'" + smrnh.CommentStatus + "')";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "SMRNHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from SMRNDetail where DocumentID='" + smrnh.DocumentID + "'" +
                   " and TemporaryNo=" + smrnh.TemporaryNo +
                   " and TemporaryDate='" + smrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "SMRNDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (smrndetail smrnd in SMRNDetail)
                {
                    updateSQL = "insert into SMRNDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,SerialNo,ItemDetails,WarrantyStatus,ServiceApprovalStatus,InspectionStatus" +
                    ",InspectionRemarks, ServiceStatus, ServiceRemarks, JobIDNo, ProductServiceStatus) " +
                    "values ('" + smrnd.DocumentID + "'," +
                    smrnh.TemporaryNo + "," +
                    "'" + smrnd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + smrnd.StockItemID + "'," +
                    "'" + smrnd.ModelNo + "'," +
                    "'" + smrnd.SerialNo + "'," +
                    "'" + smrnd.ItemDetails + "'," +
                    smrnd.WarrantyStatus + "," +
                    smrnd.ServiceApprovalStatus + "," +
                    smrnd.InspectionStatus + "," +
                    "'" + smrnd.InspectionRemarks + "'," +
                    smrnd.ServiceStatus + "," +
                    "'" + smrnd.ServiceRemarks + "'," + smrnd.JobIDNo + ",'" + smrnd.ProductServiceStatus + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "SMRNDetail", "", updateSQL) +
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
