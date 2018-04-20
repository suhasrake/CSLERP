using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    public class producttestreportheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int ReportNo { get; set; }
        public DateTime ReportDate { get; set; }
        public int ProductionPalnNo { get; set; }
        public DateTime ProductionPlanDate { get; set; }
        public string ProductSerialNo { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public int Status { get; set; }
        public int DocumentStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string CreatorName { get; set; }
        public string ForwarderName { get; set; }
        public string ApproverName { get; set; }
        public string ForwarderList { get; set; }
    }
    public class producttestreportdetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int SLNo { get; set; }
        public string TestDescriptionID { get; set; }
        public string TestDescription { get; set; }
        public string ExpectedResult { get; set; }
        public string ActualResult { get; set; }
        public int TestStatus { get; set; }
    }
    class ProductTestReportHeaderDB
    {
        public List<producttestreportheader> getFilteredProductTestReportHeader(string userList, int opt, string planDetail)
        {
            string planNo = planDetail.Substring(0, planDetail.IndexOf('-'));
            DateTime planDate = Convert.ToDateTime(planDetail.Substring(planDetail.IndexOf('-') + 1));
            producttestreportheader ptrheader;
            List<producttestreportheader> PTRHeader = new List<producttestreportheader>();
            try
            {
                string query1 = "select distinct DocumentID,TemporaryNo, TemporaryDate,ReportNo,ReportDate," +
                    " ProductionPlanNo,ProductionPlanDate,ProductSerialNo," +
                    " Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,ForwarderList,StockItemID,StockItemName " +
                    " from ViewProductTestReport" +
                    " where ((Forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98 and ProductionPlanNo = " +planNo+" and ProductionPlanDate = '"+ planDate.ToString("yyyy-MM-dd") + "') " +
                    " or (CreateUser='" + Login.userLoggedIn + "' and DocumentStatus=1 and ProductionPlanNo = " + planNo + " and ProductionPlanDate = '" + planDate.ToString("yyyy-MM-dd") + "'))"+
                    " order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query2 = "select distinct DocumentID,TemporaryNo, TemporaryDate,ReportNo,ReportDate," +
                    " ProductionPlanNo,ProductionPlanDate,ProductSerialNo," +
                    " Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,ForwarderList,StockItemID,StockItemName " +
                    " from ViewProductTestReport" +
                   " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 and ProductionPlanNo = " + planNo + " and ProductionPlanDate = '" + planDate.ToString("yyyy-MM-dd") + "') " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "' and ProductionPlanNo = " + planNo + " and ProductionPlanDate = '" + planDate.ToString("yyyy-MM-dd") + "'))" +
                    " order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query3 = "select distinct DocumentID,TemporaryNo, TemporaryDate,ReportNo,ReportDate," +
                    " ProductionPlanNo,ProductionPlanDate,ProductSerialNo," +
                    " Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,ForwarderList,StockItemID,StockItemName " +
                    " from ViewProductTestReport" +
                   " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99 and ProductionPlanNo = " + planNo + " and ProductionPlanDate = '" + planDate.ToString("yyyy-MM-dd") + "') " +
                    " order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";
                string query6 = "select distinct DocumentID,TemporaryNo, TemporaryDate,ReportNo,ReportDate," +
                    " ProductionPlanNo,ProductionPlanDate,ProductSerialNo," +
                    " Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,ForwarderList,StockItemID,StockItemName " +
                    " from ViewProductTestReport" +
                   " where  DocumentStatus = 99 and ProductionPlanNo = " + planNo + " and ProductionPlanDate = '" + planDate.ToString("yyyy-MM-dd") + "'" +
                   " order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

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
                    ptrheader = new producttestreportheader();
                    ptrheader.DocumentID = reader.GetString(0);
                    ptrheader.TemporaryNo = reader.GetInt32(1);
                    ptrheader.TemporaryDate = reader.GetDateTime(2);
                    ptrheader.ReportNo = reader.GetInt32(3);
                    if (!reader.IsDBNull(4))
                    {
                        ptrheader.ReportDate = reader.GetDateTime(4);
                    }
                    ptrheader.ProductionPalnNo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        ptrheader.ProductionPlanDate = reader.GetDateTime(6);
                    }
                    ptrheader.ProductSerialNo = reader.GetString(7);
                    ptrheader.Status = reader.GetInt32(8);
                    ptrheader.DocumentStatus = reader.GetInt32(9);
                    ptrheader.CreateTime = reader.GetDateTime(10);
                    ptrheader.CreateUser = reader.GetString(11);
                    ptrheader.ForwardUser = reader.GetString(12);
                    ptrheader.ApproveUser = reader.GetString(13);
                    ptrheader.CreatorName = reader.GetString(14);
                    ptrheader.ForwarderName = reader.GetString(15);
                    ptrheader.ApproverName = reader.GetString(16);
                    if (!reader.IsDBNull(17))
                    {
                        ptrheader.ForwarderList = reader.GetString(17);
                    }
                    else
                    {
                        ptrheader.ForwarderList = "";
                    }
                    ptrheader.StockItemID = reader.GetString(18);
                    ptrheader.StockItemName = reader.GetString(19);
                    PTRHeader.Add(ptrheader);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Product Test Report Header Details");
            }
            return PTRHeader;
        }

        public static List<producttestreportdetail> getProductTestReportDetails(producttestreportheader ptrh)
        {
            producttestreportdetail ptrd;
            List<producttestreportdetail> PTRDetail = new List<producttestreportdetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,TemporaryNo, TemporaryDate,SLNo,TestDescriptionID,TestDescription,ExpectedResult,ActualResult,TestStatus " +
                   "from ViewProductTestReport " +
                   "where DocumentID='" + ptrh.DocumentID + "'" +
                   " and TemporaryNo=" + ptrh.TemporaryNo +
                   " and TemporaryDate='" + ptrh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ptrd = new producttestreportdetail();
                    ptrd.RowID = reader.GetInt32(0);
                    ptrd.DocumentID = reader.GetString(1);
                    ptrd.TemporaryNo = reader.GetInt32(2);
                    ptrd.TemporaryDate = reader.GetDateTime(3).Date;
                    ptrd.SLNo = reader.GetInt32(4);
                    ptrd.TestDescriptionID = reader.GetString(5);
                    ptrd.TestDescription = reader.GetString(6);
                    ptrd.ExpectedResult = reader.GetString(7);
                    ptrd.ActualResult = reader.GetString(8);
                    ptrd.TestStatus = reader.GetInt32(9);
                    PTRDetail.Add(ptrd);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Product Test Report Details");
            }
            return PTRDetail;
        }

        public Boolean validateProductTestReportHeader(producttestreportheader ptrh)
        {
            Boolean status = true;
            try
            {
                if (ptrh.ProductionPalnNo == 0)
                {
                    return false;
                }
                if (ptrh.ProductionPlanDate == null)
                {
                    return false;
                }
                if (ptrh.ProductSerialNo.Trim().Length == 0 || ptrh.ProductSerialNo == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public Boolean ForwardProductTestReportHeader(producttestreportheader ptrh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductTestReportHeader set DocumentStatus=" + (ptrh.DocumentStatus + 1) +
                    ", ForwardUser='" + ptrh.ForwardUser + "'" +
                    ", ForwarderList='" + ptrh.ForwarderList + "'" +
                    " where DocumentID='" + ptrh.DocumentID + "'" +
                    " and TemporaryNo=" + ptrh.TemporaryNo +
                    " and TemporaryDate='" + ptrh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductTestReportHeader", "", updateSQL) +
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
        public Boolean reverseProductTestReportHeader(producttestreportheader ptrh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductTestReportHeader set DocumentStatus=" + ptrh.DocumentStatus +
                    ", ForwardUser='" + ptrh.ForwardUser + "'" +
                    ", ForwarderList='" + ptrh.ForwarderList + "'" +
                    " where DocumentID='" + ptrh.DocumentID + "'" +
                    " and TemporaryNo=" + ptrh.TemporaryNo +
                    " and TemporaryDate='" + ptrh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductTestReportHeader", "", updateSQL) +
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
        public Boolean ApproveProductTestReportHeader(producttestreportheader ptrh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductTestReportHeader set DocumentStatus=99, status=1 " +
                   ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", ReportNo=" + ptrh.ReportNo +
                    ", ReportDate=convert(date, getdate())" +
                    " where DocumentID='" + ptrh.DocumentID + "'" +
                    " and TemporaryNo=" + ptrh.TemporaryNo +
                    " and TemporaryDate='" + ptrh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductTestReportHeader", "", updateSQL) +
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
        public List<producttestreportheader> getTestReports(int planNo, DateTime planDate)
        {
            producttestreportheader ptrh;
            List<producttestreportheader> PTRDetail = new List<producttestreportheader>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select distinct ReportNo, ReportDate, ProductSerialNo, StockItemID, StockItemName,ModelNo,ModelName " +
                   "from ViewProductTestReport " +
                   " where ProductionPlanNo=" + planNo +
                   " and ProductionPlanDate='" + planDate.ToString("yyyy-MM-dd") + "'" +
                   " and Status  = 1 and DocumentStatus  = 99";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ptrh = new producttestreportheader();
                    ptrh.ReportNo = reader.GetInt32(0);
                    ptrh.ReportDate = reader.GetDateTime(1);
                    ptrh.ProductSerialNo = reader.GetString(2);
                    ptrh.StockItemID = reader.GetString(3);
                    ptrh.StockItemName = reader.GetString(4);
                    ptrh.CreateUser = reader.GetString(5);   /// for ModelNo
                    ptrh.CreatorName = reader.GetString(6);  /// for mOdelname
                    PTRDetail.Add(ptrh);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Product Test Report Details");
            }
            return PTRDetail;
        }
        public static ListView getProdTestReportListView(int planNo, DateTime planDate)
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
                ProductTestReportHeaderDB ptrDB = new ProductTestReportHeaderDB();
                List<producttestreportheader> PTRDetail = ptrDB.getTestReports(planNo, planDate);
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Report No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Report Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Product Serial No", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Product", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Product Description", -2, HorizontalAlignment.Center);
                lv.Columns.Add("ModelNo", -2, HorizontalAlignment.Center);
                lv.Columns.Add("ModelName", -2, HorizontalAlignment.Center);
                foreach (producttestreportheader ptrh in PTRDetail)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(ptrh.ReportNo.ToString());
                    item1.SubItems.Add(ptrh.ReportDate.ToShortDateString());
                    item1.SubItems.Add(ptrh.ProductSerialNo);
                    item1.SubItems.Add(ptrh.StockItemID);
                    item1.SubItems.Add(ptrh.StockItemName);
                    item1.SubItems.Add(ptrh.CreateUser);   //// For MOdelNO
                    item1.SubItems.Add(ptrh.CreatorName);  /// For ModelName
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static List<producttestreportheader> getReportListForPrint(int ReportNo, DateTime ReportDate)
        {
            producttestreportheader ptrheader;
            List<producttestreportheader> PTRHeader = new List<producttestreportheader>();
            try
            {
                string query = "select TemporaryNo, TemporaryDate,ReportNo,ReportDate," +
                    " ProductionPlanNo,ProductionPlanDate,ProductSerialNo,StockItemID,StockItemName," +
                    " CreatorName,ApproverName,TestDescription,ExpectedResult,ActualResult,TestStatus " +
                    " from ViewProductTestReport" +
                   " where ReportNo ="+ ReportNo +
                   " and ReportDate= '" + ReportDate.ToString("yyyy-MM-dd") + "'"+
                   " and DocumentStatus = 99";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ptrheader = new producttestreportheader();
                    ptrheader.TemporaryNo = reader.GetInt32(0);
                    ptrheader.TemporaryDate = reader.GetDateTime(1);
                    ptrheader.ReportNo = reader.GetInt32(2);
                    ptrheader.ReportDate = reader.GetDateTime(3);
                    ptrheader.ProductionPalnNo = reader.GetInt32(4);
                    ptrheader.ProductionPlanDate = reader.GetDateTime(5);
                    ptrheader.ProductSerialNo = reader.GetString(6);
                    ptrheader.StockItemID = reader.GetString(7);
                    ptrheader.StockItemName = reader.GetString(8);
                    ptrheader.CreatorName = reader.GetString(9);
                    ptrheader.ApproverName = reader.GetString(10);
                    ptrheader.CreateUser = reader.GetString(11);  // for testDeacription
                    ptrheader.ForwardUser = reader.GetString(12);  // for Expected Result
                    ptrheader.ApproveUser = reader.GetString(13);  // for Actual Result
                    ptrheader.Status = reader.GetInt32(14);   // for TestStatus
                    PTRHeader.Add(ptrheader);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Product Test Report Header Details");
            }
            return PTRHeader;
        }
        //public static string fillMRStockItemGridViewComboWithValue(DataGridViewComboBoxCell cmb, List<podetail> PODetail, int RowNo)
        //{
        //    int count = 0;
        //    string firstValue = "";
        //    cmb.Items.Clear();

        //    foreach (podetail pod in PODetail)
        //    {
        //        count++;
        //        cmb.Items.Add(pod.StockItemID + "-" + pod.StockItemName);
        //        if (RowNo == count)
        //        {
        //            firstValue = pod.StockItemID + "-" + pod.StockItemName;
        //        }
        //    }
        //    return firstValue;
        //}
        public static Boolean checkForApprove(int plnNo, double quantity)
        {
            Boolean status = true;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select count(*) as noCount " +
                   "from ProductTestReportHeader " +
                   "where ProductionPlanNo = " + plnNo;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader.GetInt32(0) >= quantity)
                    {
                        MessageBox.Show("Not allow to select more test report of selected plan no");
                        status = false;
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                status = false;
                MessageBox.Show("Error in filling Stock Item in gridview");
            }
            return status;
        }
        public Boolean updatePTRHeaderAndDetail(producttestreportheader ptrh , producttestreportheader prevptrh,List<producttestreportdetail> PTRDetail )
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductTestReportHeader set TemporaryNo = " + ptrh.TemporaryNo +
                   ", TemporaryDate='" + ptrh.TemporaryDate.ToString("yyyy-MM-dd") +
                   "', ReportNo=" + ptrh.ReportNo +
                   ", ReportDate='" + ptrh.ReportDate.ToString("yyyy-MM-dd") +
                   "', ProductionPlanNo='" + ptrh.ProductionPalnNo +
                   "', ProductionPlanDate='" + ptrh.ProductionPlanDate.ToString("yyyy-MM-dd") +
                   "', ProductSerialNo='" + ptrh.ProductSerialNo +
                   "', ForwarderList='" + ptrh.ForwarderList + "'" +
                  " where DocumentID='" + prevptrh.DocumentID + "'" +
                  " and TemporaryNo=" + prevptrh.TemporaryNo +
                  " and TemporaryDate='" + prevptrh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductTestReportHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from ProductTestReportDetail where DocumentID='" + prevptrh.DocumentID + "'" +
                    " and TemporaryNo=" + prevptrh.TemporaryNo +
                    " and TemporaryDate='" + prevptrh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "ProductTestReportDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (producttestreportdetail ptrd in PTRDetail)
                {
                    updateSQL = "insert into ProductTestReportDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,SLNo,TestDescriptionID,ExpectedResult,ActualResult,TestStatus) " +
                    "values ('" + ptrd.DocumentID + "'," +
                    ptrd.TemporaryNo + "," +
                    "'" + ptrd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    ptrd.SLNo + "," +
                   "'" + ptrd.TestDescriptionID + "' ," +
                    "'" + ptrd.ExpectedResult + "' ," +
                    "'" + ptrd.ActualResult + "'," + ptrd.TestStatus + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductTestReportDetail", "", updateSQL) +
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
        public Boolean InsertPTRHeaderAndDetail(producttestreportheader ptrh, List<producttestreportdetail> PTRDetail)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                ptrh.TemporaryNo = DocumentNumberDB.getNumber(ptrh.DocumentID, 1);
                if (ptrh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + ptrh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + ptrh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into ProductTestReportHeader " +
                     "(DocumentID,TemporaryNo,TemporaryDate,ReportNo,ReportDate,ProductionPlanNo,ProductionPlanDate,ProductSerialNo,DocumentStatus," +
                    "CreateTime,CreateUser,ForwarderList)" +
                     " values (" +
                     "'" + ptrh.DocumentID + "'," +
                     ptrh.TemporaryNo + "," +
                     "'" + ptrh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     ptrh.ReportNo + "," +
                     "'" + ptrh.ReportDate.ToString("yyyy-MM-dd") + "'," +
                    ptrh.ProductionPalnNo + "," +
                     "'" + ptrh.ProductionPlanDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + ptrh.ProductSerialNo + "'," +
                     ptrh.DocumentStatus + "," +
                      "GETDATE()" + "," +
                     "'" + Login.userLoggedIn + "'," +
                     "'" + ptrh.ForwarderList + "')";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductTestReportHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from ProductTestReportDetail where DocumentID='" + ptrh.DocumentID + "'" +
                    " and TemporaryNo=" + ptrh.TemporaryNo +
                    " and TemporaryDate='" + ptrh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "ProductTestReportDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (producttestreportdetail ptrd in PTRDetail)
                {
                    updateSQL = "insert into ProductTestReportDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,SLNo,TestDescriptionID,ExpectedResult,ActualResult,TestStatus) " +
                    "values ('" + ptrd.DocumentID + "'," +
                    ptrh.TemporaryNo + "," +
                    "'" + ptrd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    ptrd.SLNo + "," +
                   "'" + ptrd.TestDescriptionID + "' ," +
                    "'" + ptrd.ExpectedResult + "' ," +
                    "'" + ptrd.ActualResult + "'," + ptrd.TestStatus + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductTestReportDetail", "", updateSQL) +
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
