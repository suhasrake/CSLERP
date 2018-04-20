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
    class productionplanheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int ProductionPlanNo { get; set; }
        public DateTime ProductionPlanDate { get; set; }
        //public int InternalOrderNo { get; set; }
        //public DateTime InternalOrderDate { get; set; }
        public string Reference { get; set; }
        //Newly Added
        public string InternalOrderNos { get; set; }
        public string InternalOrderDates { get; set; }

        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public double Quantity { get; set; }
        public DateTime PlannedStartTime { get; set; }
        public DateTime PlannedEndTime { get; set; }
        public DateTime ActualStartTime { get; set; }
        public DateTime ActualEndTime { get; set; }
        public string FloorManager { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public int DocumentStatus { get; set; }
        public int ProductionStatus { get; set; }
        public string ProductionStatusString { get; set; }
        public string Comments { get; set; }
        public string CommentStatus { get; set; }
        public string CreateUser { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreateTime { get; set; }
        public string ForwarderName { get; set; }
        public string ApproverName { get; set; }
        public string ForwarderList { get; set; }
        public int RawMaterialUsage { get; set; }
        public int HaltStatus { get; set; }
        public int CancelStatus { get; set; }
        public int ClosureStatus { get; set; }
        public productionplanheader()
        {
            //Reference = "";
            Comments = "";
        }
    }
    class productionplandetail
    {
        public int RowID { get; set; }
        public int SlNo { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int ProductionPlanNo { get; set; }
        public DateTime ProductionPlanDate { get; set; }
        public DateTime PlannedStartTime { get; set; }
        public DateTime PlannedEndTime { get; set; }
        public int Status { get; set; }
        public int DocumentStatus { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public double Quantity { get; set; }
        public string ProcessID { get; set; }
        public string ProcessDescription { get; set; }
        public string TeamMembers { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime ActualStartTime { get; set; }
        public DateTime ActualEndTime { get; set; }
        public int ProcessStatus { get; set; }
       
        public String Remarks { get; set; }

        public productionplandetail()
        {
            StartTime = DateTime.Today;
            EndTime = DateTime.Today;
            ActualStartTime = DateTime.Today;
            ActualEndTime = DateTime.Today;
        }

    }
    class ProductionPlanHeaderDB
    {
        public List<productionplanheader> getFilteredOngoingProductionPlan()
        {
            productionplanheader pph;
            List<productionplanheader> ProductionPlanHeaderList = new List<productionplanheader>();
            try
            {
                //-----
                string query = "select H.ProductionPlanNo,H.ProductionPlanDate,H.PlannedStartTime,H.PlannedEndTime,H.ActualStartTime,H.ActualEndTime,H.FloorManager ,d.TeamMembers from ProductionPlanHeader H, ProductionPlanDetail D where h.DocumentID = d.DocumentID and h.TemporaryNo = d.TemporaryNo and h.TemporaryDate = d.TemporaryDate and H.Status=1 and H.DocumentStatus=99 and h.ProductionStatus=1";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        pph = new productionplanheader();

                        pph.ProductionPlanNo = reader.GetInt32(0);
                        pph.ProductionPlanDate = reader.GetDateTime(1);
                        pph.PlannedStartTime = reader.GetDateTime(2);
                        pph.PlannedEndTime = reader.GetDateTime(3);
                        pph.ActualStartTime = reader.GetDateTime(4);
                        pph.ActualEndTime = reader.GetDateTime(5);
                        pph.FloorManager = reader.GetString(6);
                        pph.ForwarderList = reader.GetString(7);
                        ProductionPlanHeaderList.Add(pph);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Production Header Details");
            }
            return ProductionPlanHeaderList;
        }
        public List<productionplanheader> getFilteredProductionPlanHeader(string userList, int opt, string userCommentStatusString)
        {
            productionplanheader pph;
            List<productionplanheader> ProductionPlanHeaderList = new List<productionplanheader>();
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
                string query1 = "select RowID,DocumentID,DocumentName,TemporaryNo,TemporaryDate,ProductionPlanNo,ProductionPlanDate," +
                    " InternalOrderNos,InternalOrderDates,Reference,StockItemID,StockItemName,ModelNo,ModelName,Quantity,PlannedStartTime,PlannedEndTime,ActualStartTime,ActualEndTime,FloorManager,Remarks," +
                    " Status,DocumentStatus,ProductionStatus,CommentStatus,CreateUser,ForwardUser ," +
                    " ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,RawMaterialUsage,ProductionStatusString " +
                    " from ViewProductionPlanHeader" +
                    " where ((ForwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (CreateUser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (CommentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) and Status not in (7,98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query2 = "select RowID,DocumentID,DocumentName,TemporaryNo,TemporaryDate,ProductionPlanNo,ProductionPlanDate," +
                    " InternalOrderNos,InternalOrderDates,Reference,StockItemID,StockItemName,ModelNo,ModelName,Quantity,PlannedStartTime,PlannedEndTime,ActualStartTime,ActualEndTime,FloorManager,Remarks," +
                    " Status,DocumentStatus,ProductionStatus,CommentStatus,CreateUser,ForwardUser ," +
                    " ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,RawMaterialUsage,ProductionStatusString  " +
                    " from ViewProductionPlanHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) and Status not in (7,98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query3 = "select RowID,DocumentID,DocumentName,TemporaryNo,TemporaryDate,ProductionPlanNo,ProductionPlanDate," +
                    " InternalOrderNos,InternalOrderDates,Reference,StockItemID,StockItemName,ModelNo,ModelName,Quantity,PlannedStartTime,PlannedEndTime,ActualStartTime,ActualEndTime,FloorManager,Remarks," +
                    " Status,DocumentStatus,ProductionStatus,CommentStatus,CreateUser,ForwardUser ," +
                    " ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,RawMaterialUsage,ProductionStatusString " +
                    " from ViewProductionPlanHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99) and Status = 1 order by ProductionPlanDate desc,DocumentID asc,ProductionPlanNo desc";

                string query6 = "select RowID,DocumentID,DocumentName,TemporaryNo,TemporaryDate,ProductionPlanNo,ProductionPlanDate," +
                    " InternalOrderNos,InternalOrderDates,Reference,StockItemID,StockItemName,ModelNo,ModelName,Quantity,PlannedStartTime,PlannedEndTime,ActualStartTime,ActualEndTime,FloorManager,Remarks," +
                    " Status,DocumentStatus,ProductionStatus,CommentStatus,CreateUser,ForwardUser ," +
                    " ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,RawMaterialUsage,ProductionStatusString " +
                    " from ViewProductionPlanHeader" +
                    " where  DocumentStatus = 99 and Status = 1 order by ProductionPlanDate desc,DocumentID asc,ProductionPlanNo desc";

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
                    try
                    {
                        pph = new productionplanheader();
                        pph.RowID = reader.GetInt32(0);
                        pph.DocumentID = reader.GetString(1);
                        pph.DocumentName = reader.GetString(2);
                        pph.TemporaryNo = reader.GetInt32(3);
                        pph.TemporaryDate = reader.GetDateTime(4);
                        pph.ProductionPlanNo = reader.GetInt32(5);
                        pph.ProductionPlanDate = reader.GetDateTime(6);
                        pph.InternalOrderNos = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        pph.InternalOrderDates = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        pph.Reference = reader.GetString(9);
                        pph.StockItemID = reader.GetString(10);
                        pph.StockItemName = reader.GetString(11);
                        pph.ModelNo = reader.IsDBNull(12)?"NA":reader.GetString(12);
                        pph.ModelName = reader.IsDBNull(13) ? "NA" : reader.GetString(13);
                        pph.Quantity = reader.GetDouble(14);
                        pph.PlannedStartTime = reader.GetDateTime(15);
                        pph.PlannedEndTime = reader.GetDateTime(16);
                        pph.ActualStartTime = reader.GetDateTime(17);
                        pph.ActualEndTime = reader.GetDateTime(18);
                        pph.FloorManager = reader.GetString(19);
                        pph.Remarks = reader.GetString(20);
                        pph.Status = reader.GetInt32(21);
                        pph.DocumentStatus = reader.GetInt32(22);
                        pph.ProductionStatus = reader.GetInt32(23);
                        //pph.Comments = reader.GetString(22);
                        if (!reader.IsDBNull(24))
                        {
                            pph.CommentStatus = reader.GetString(24);
                        }
                        else
                        {
                            pph.CommentStatus = "";
                        }
                        pph.CreateUser = reader.GetString(25);
                        pph.ForwardUser = reader.GetString(26);
                        pph.ApproveUser = reader.GetString(27);
                        pph.CreatorName = reader.GetString(28);
                        pph.CreateTime = reader.GetDateTime(29);
                        pph.ForwarderName = reader.GetString(30);
                        pph.ApproverName = reader.GetString(31);

                        if (!reader.IsDBNull(32))
                        {
                            pph.ForwarderList = reader.GetString(32);
                        }
                        else
                        {
                            pph.ForwarderList = "";
                        }
                        pph.RawMaterialUsage = reader.GetInt32(33);
                        pph.ProductionStatusString = reader.IsDBNull(34)?"":reader.GetString(34);
                        ProductionPlanHeaderList.Add(pph);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Production Header Details");
            }
            return ProductionPlanHeaderList;
        }

        internal List<productionplandetail> getProductionPlanDetail(string userString, int option, string userCommentStatusString)
        {
            throw new NotImplementedException();
        }

        public static List<productionplandetail> getProductionPlanDetail(productionplanheader pph)
        {
            productionplandetail ppd;
            List<productionplandetail> ProductionPlanDetailList = new List<productionplandetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,SlNo,DocumentID,DocumentName,TemporaryNo,TemporaryDate,ProductionPlanNo,ProductionPlanDate,PlannedStartTime,PlannedEndTime,Status,DocumentStatus,StockItemID,StockItemname,Quantity,ProcessID,ProcessDescription,TeamMembers,StartTime,EndTime,ActualStartTime,ActualEndTime,ProcessStatus, " +
                   "Remarks " +
                  "from ViewProductionPlanDetail " +
                  " where DocumentID='" + pph.DocumentID + "'" +
                  " and TemporaryNo=" + pph.TemporaryNo +
                  " and TemporaryDate='" + pph.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                  " order by StartTime,EndTime";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ppd = new productionplandetail();
                    ppd.RowID = reader.GetInt32(0);
                    ppd.SlNo = reader.GetInt32(1);
                    ppd.DocumentID = reader.GetString(2);
                    ppd.DocumentName = reader.GetString(3);
                    ppd.TemporaryNo = reader.GetInt32(4);
                    ppd.TemporaryDate = reader.GetDateTime(5).Date;
                    ppd.ProductionPlanNo = reader.GetInt32(6);
                    ppd.ProductionPlanDate = reader.GetDateTime(7).Date;
                    ppd.PlannedStartTime = reader.GetDateTime(8);
                    ppd.PlannedEndTime = reader.GetDateTime(9);
                    ppd.Status = reader.GetInt32(10);
                    ppd.DocumentStatus = reader.GetInt32(11);
                    ppd.StockItemID = reader.GetString(12);
                    ppd.StockItemName = reader.GetString(13);
                    ppd.Quantity = reader.GetDouble(14);
                    ppd.ProcessID = reader.GetString(15);
                    ppd.ProcessDescription = reader.GetString(16);
                    ppd.TeamMembers = reader.GetString(17);
                    ppd.StartTime = reader.GetDateTime(18);
                    ppd.EndTime = reader.GetDateTime(19);
                    ppd.ActualStartTime = reader.GetDateTime(20);
                    ppd.ActualEndTime = reader.GetDateTime(21);
                    ppd.ProcessStatus = reader.GetInt32(22);
                    ppd.Remarks = reader.GetString(23);
                    ProductionPlanDetailList.Add(ppd);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Production Details");
            }
            return ProductionPlanDetailList;
        }
        public Boolean validateProductionPlanHeader(productionplanheader pph)
        {
            Boolean status = true;
            try
            {
                if (pph.DocumentID.Trim().Length == 0 || pph.DocumentID == null)
                {
                    return false;
                }
                if (pph.InternalOrderNos.Trim().Length == 0 || pph.InternalOrderNos == null)
                {
                    return false;
                }
                if (pph.InternalOrderDates.Trim().Length == 0 || pph.InternalOrderDates == null)
                {
                    return false;
                }
                //if (pph.InternalOrderDate == null || pph.InternalOrderDate > DateTime.Now)
                //{
                //    return false;
                //}
                //if (pph.Reference.Trim().Length == 0 || pph.Reference == null)
                //{
                //    return false;
                //}
                if (pph.StockItemID.Trim().Length == 0 || pph.StockItemID == null)
                {
                    return false;
                }
                if (pph.ModelNo.Trim().Length == 0 || pph.ModelNo == null)
                {
                    return false;
                }
                if (pph.ModelName.Trim().Length == 0 || pph.ModelName == null)
                {
                    return false;
                }
                if (pph.Quantity == 0)
                {
                    return false;
                }
                if (pph.PlannedStartTime == null || pph.PlannedStartTime < DateTime.Now)
                {
                    return false;
                }
                if (pph.PlannedEndTime == null || pph.PlannedEndTime < DateTime.Now)
                {
                    return false;
                }
                if (pph.FloorManager.Trim().Length == 0 || pph.FloorManager == null)
                {
                    return false;
                }
                if (pph.Remarks.Trim().Length == 0 || pph.Remarks == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public Boolean forwardProductionPlanHeader(productionplanheader pph)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductionPlanHeader set DocumentStatus=" + (pph.DocumentStatus + 1) +
                    ", forwardUser='" + pph.ForwardUser + "'" +
                    ", commentStatus='" + pph.CommentStatus + "'" +
                    ", ForwarderList='" + pph.ForwarderList + "'" +
                    //", ProductionStatus= " + pph.ProductionStatus +
                    " where DocumentID='" + pph.DocumentID + "'" +
                    " and TemporaryNo=" + pph.TemporaryNo +
                    " and TemporaryDate='" + pph.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductionPlanHeader", "", updateSQL) +
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

        public Boolean reverseProductionPlanHeader(productionplanheader pph)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductionPlanHeader set DocumentStatus=" + pph.DocumentStatus +
                    //",ProductionStatus=" + pph.ProductionStatus +
                    ", forwardUser='" + pph.ForwardUser + "'" +
                    ", commentStatus='" + pph.CommentStatus + "'" +
                    ", ForwarderList='" + pph.ForwarderList + "'" +
                    " where DocumentID='" + pph.DocumentID + "'" +
                    " and TemporaryNo=" + pph.TemporaryNo +
                    " and TemporaryDate='" + pph.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductionPlanHeader", "", updateSQL) +
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

        public Boolean ApproveProductionPlanHeader(productionplanheader pph)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductionPlanHeader set DocumentStatus=99, status=1, ProductionStatus=1" +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + pph.CommentStatus + "'" +
                    ", ProductionPlanNo=" + pph.ProductionPlanNo +
                    ", ProductionPlanDate=convert(date, getdate())" +
                    " where DocumentID='" + pph.DocumentID + "'" +
                    " and TemporaryNo=" + pph.TemporaryNo +
                    " and TemporaryDate='" + pph.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductionPlanHeader", "", updateSQL) +
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

        public static List<productionplandetail> getProductionProcessDetail()
        {
            productionplandetail ppd;
            List<productionplandetail> ProductionProcessDetailList = new List<productionplandetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select distinct ProductionPlanNo,ProductionPlanDate,PlannedStartTime,StockItemname, " +
                   "Quantity,TemporaryNo,TemporaryDate " +
                  "from ViewProductionPlandetail " +
                  " where ProcessStatus=0 and DocumentStatus=99 and status=1 and ProductionStatus = 1 order by ProductionPlanDate desc,ProductionPlanNo desc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ppd = new productionplandetail();
                    ppd.ProductionPlanNo = reader.GetInt32(0);
                    ppd.ProductionPlanDate = reader.GetDateTime(1).Date;
                    ppd.PlannedStartTime = reader.GetDateTime(2);
                    ppd.StockItemName = reader.GetString(3);
                    ppd.Quantity = reader.GetDouble(4);
                    ppd.TemporaryNo = reader.GetInt32(5);
                    ppd.TemporaryDate = reader.GetDateTime(6).Date;
                    ProductionProcessDetailList.Add(ppd);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Production Details");
            }
            return ProductionProcessDetailList;
        }


        public static ListView ProductionProcessStatus()
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
                ////lv.Sorting = System.Windows.Forms.SortOrder.Ascending;

                List<productionplandetail> processlist = ProductionPlanHeaderDB.getProductionProcessDetail();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PlanNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PlanDate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PlandStartTime", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ProductName", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Quantity", -2, HorizontalAlignment.Center);
                lv.Columns.Add("TemporaryNo", -2, HorizontalAlignment.Center);
                lv.Columns.Add("TemporaryDate", -2, HorizontalAlignment.Center);
                // lv.Columns[3].Width = 0;
                foreach (productionplandetail process in processlist)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(process.ProductionPlanNo.ToString());
                    item1.SubItems.Add(process.ProductionPlanDate.ToString());
                    item1.SubItems.Add(process.PlannedStartTime.ToString());
                    item1.SubItems.Add(process.StockItemName);
                    item1.SubItems.Add(process.Quantity.ToString());
                    item1.SubItems.Add(process.TemporaryNo.ToString());
                    item1.SubItems.Add(process.TemporaryDate.ToString());
                    lv.Items.Add(item1);

                }
            }
            catch (Exception)
            {

            }
            return lv;
        }


        public static List<productionplandetail> getProductionProcessRunning(int no,DateTime planDate)
        {
            productionplandetail ppd;
            List<productionplandetail> ProductionProcessRunning = new List<productionplandetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select SlNo,StockItemID,StockItemName,ProcessID, " +
                   "ProcessDescription,StartTime,EndTime,ActualStartTime, ActualEndTime " +
                  " from ViewProductionPlandetail " +
                  " where ProcessStatus=0 and ProductionPlanNo=" + no +
                  " and ProductionPlanDate = '"+ planDate.ToString("yyyy-MM-dd") + "'" +
                  " order by SlNo asc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ppd = new productionplandetail();
                    ppd.SlNo = reader.GetInt32(0);
                    ppd.StockItemID = reader.GetString(1);
                    ppd.StockItemName = reader.GetString(2);
                    ppd.ProcessID = reader.GetString(3);
                    ppd.ProcessDescription = reader.GetString(4);
                    ppd.StartTime = reader.GetDateTime(5);
                    ppd.EndTime = reader.GetDateTime(6);
                    ppd.ActualStartTime = reader.GetDateTime(7);
                    ppd.ActualEndTime = reader.GetDateTime(8);
                    ProductionProcessRunning.Add(ppd);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Production Details");
            }
            return ProductionProcessRunning;
        }


        public static ListView ProductionProcessRunning(int planno, DateTime planDate)
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
                //lv.Sorting = System.Windows.Forms.SortOrder.Ascending;

                List<productionplandetail> processlist = ProductionPlanHeaderDB.getProductionProcessRunning(planno, planDate);
                ////int index = 0;
                lv.Columns.Add("Sel", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SlNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItem ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItemName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Process ID", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Process Desc", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Start Time", -2, HorizontalAlignment.Center);
                lv.Columns.Add("End Time", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Actual Start Time", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Actual End Time", -2, HorizontalAlignment.Center);

                //lv.Columns.Add("TemporaryNo", -2, HorizontalAlignment.Center);
                //lv.Columns.Add("TemporaryDate", -2, HorizontalAlignment.Center);
                lv.Columns[2].Width = 0;
                lv.Columns[3].Width = 0;
                //lv.Columns[8].Width = 0;
                //lv.Columns[9].Width = 0;
                foreach (productionplandetail process in processlist)
                {

                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(process.SlNo.ToString());
                    item1.SubItems.Add(process.StockItemID.ToString());
                    item1.SubItems.Add(process.StockItemName.ToString());
                    item1.SubItems.Add(process.ProcessID.ToString());
                    item1.SubItems.Add(process.ProcessDescription.ToString());
                    item1.SubItems.Add(process.StartTime.ToString());
                    item1.SubItems.Add(process.EndTime.ToString());
                    item1.SubItems.Add(process.ActualStartTime.ToString());
                    item1.SubItems.Add(process.ActualEndTime.ToString());
                    // item1.SubItems.Add(process.TemporaryDate.ToString());
                    lv.Items.Add(item1);

                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static string getProductionProcessRemarks(string PlanNo, DateTime PlanDate)
        {
            string rem = "";
            List<productionplanheader> ProductionProcessRemarksList = new List<productionplanheader>();
            try
            {
                string query = "";

                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select Remarks " +
                  "from ViewProductionPlanHeader " +
                  " where ProductionPlanNo= " + PlanNo +
                   " and ProductionPlanDate='" + PlanDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    rem = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Production Details remark");
            }
            return rem;
        }
        public static string getUserComments(string docid, int tempno, DateTime tempdate)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from ProductionPlanHeader where DocumentID='" + docid + "'" +
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
        public static string getUserCommentsForProcess(string docid, int tempno, DateTime tempdate,int slno )
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select Remarks from ProductionPlanDetail where "+
                        " TemporaryNo=" + tempno +
                        " and TemporaryDate='" + tempdate.ToString("yyyy-MM-dd") + "'" +
                        " and SlNo="+slno;
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
        public static Boolean updateProductionProcessStatus(productionplandetail ppd, int opt, Boolean isUpdateProdStat)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                
                string updateSQL1 = "update ProductionPlanDetail set ProcessStatus = 1,Remarks = '" + ppd.Remarks + "'," +
                    " ActualStartTime= '" + ppd.ActualStartTime.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                    " where TemporaryNo=" + ppd.TemporaryNo +
                    " and TemporaryDate='" + ppd.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                    " and SLNo=" + ppd.SlNo;
                string updateSQL2 = "update ProductionPlanDetail set ProcessStatus = 99,Remarks = '" + ppd.Remarks + "'," +
                   " ActualEndTime= '" + ppd.ActualEndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                   " where TemporaryNo=" + ppd.TemporaryNo +
                   " and TemporaryDate='" + ppd.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                   " and SLNo=" + ppd.SlNo;

                switch (opt)
                {
                    case 1:
                        updateSQL = updateSQL1;
                        break;
                    case 2:
                        updateSQL = updateSQL2;
                        break;
                    default:
                        updateSQL = "";
                        break;
                }
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductionPlanDetail", "", updateSQL) +
                Main.QueryDelimiter;


                if (isUpdateProdStat)
                {
                    updateSQL = "update ProductionPlanHeader set ProductionStatus = 2 " +
                     " where TemporaryNo=" + ppd.TemporaryNo +
                   " and TemporaryDate='" + ppd.TemporaryDate.ToString("yyyy-MM-dd") + "' and DocumentID = 'PRODUCTIONPLAN'";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "ProductionPlanHeader", "", updateSQL) +
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
        public static Boolean updateProductionProcessRemarks(productionplandetail ppd, string remarks)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductionPlanDetail" +
                    " set Remarks= '" + remarks + "'" +
                    " where TemporaryNo=" + ppd.TemporaryNo +
                    " and TemporaryDate='" + ppd.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                    " and SLNo=" + ppd.SlNo;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductionPlanDetail", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: "+ ex.ToString());
                status = false;
            }
            return status;
        }
        public static ListView getPlanNoListView()
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
                ////lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                ProductionPlanHeaderDB planDB = new ProductionPlanHeaderDB();
                List<productionplanheader> planList = planDB.getUnClosedProductionPlanForStockIssue();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PlanNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PlanDate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ProductName", -2, HorizontalAlignment.Center);
                lv.Columns.Add("ModelNo", -2, HorizontalAlignment.Center);
                lv.Columns.Add("ModelName", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Quantity", -2, HorizontalAlignment.Center);
                // lv.Columns[3].Width = 0;
                foreach (productionplanheader pph in planList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(pph.ProductionPlanNo.ToString());
                    item1.SubItems.Add(pph.ProductionPlanDate.ToString());
                    item1.SubItems.Add(pph.StockItemID +"-"+pph.StockItemName);
                    item1.SubItems.Add(pph.ModelNo.ToString());
                    item1.SubItems.Add(pph.ModelName.ToString());
                    item1.SubItems.Add(pph.Quantity.ToString());
                    lv.Items.Add(item1);

                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static ListView getPlanNoListViewForRawMaterialUsage()
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
                ////lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                ProductionPlanHeaderDB planDB = new ProductionPlanHeaderDB();
                List<productionplanheader> planList = planDB.getFilteredProductionPlanHeader("", 6, "");
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PlanNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PlanDate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ProductName", -2, HorizontalAlignment.Center);
                lv.Columns.Add("ModelNo", -2, HorizontalAlignment.Center);
                lv.Columns.Add("ModelName", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Quantity", -2, HorizontalAlignment.Center);
                // lv.Columns[3].Width = 0;
                foreach (productionplanheader pph in planList)
                {
                    if (pph.ProductionStatus == 1 && pph.RawMaterialUsage != 1)
                    {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        item1.SubItems.Add(pph.ProductionPlanNo.ToString());
                        item1.SubItems.Add(pph.ProductionPlanDate.ToString());
                        item1.SubItems.Add(pph.StockItemID + "-" + pph.StockItemName);
                        item1.SubItems.Add(pph.ModelNo.ToString());
                        item1.SubItems.Add(pph.ModelName.ToString());
                        item1.SubItems.Add(pph.Quantity.ToString());
                        lv.Items.Add(item1);
                    }
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static ListView getPlanNoListViewForStatusChange()
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
                ////lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                ProductionPlanHeaderDB planDB = new ProductionPlanHeaderDB();
                List<productionplanheader> planList = planDB.getFilteredProductionPlanHeader("", 6, "");
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PlanNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PlanDate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ProductName", -2, HorizontalAlignment.Center);
                lv.Columns.Add("ModelNo", -2, HorizontalAlignment.Center);
                lv.Columns.Add("ModelName", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Quantity", -2, HorizontalAlignment.Center);
                lv.Columns.Add("ProductionStatus", -2, HorizontalAlignment.Center);
                lv.Columns.Add("PlannedStartTime", -2, HorizontalAlignment.Center);
                lv.Columns.Add("PlannedEndTime", -2, HorizontalAlignment.Center);
                // lv.Columns[3].Width = 0;
                foreach (productionplanheader pph in planList)
                {
                    if ((pph.ProductionStatus == 1 || pph.ProductionStatus == 2) && pph.RawMaterialUsage == 1)
                    {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        item1.SubItems.Add(pph.ProductionPlanNo.ToString());
                        item1.SubItems.Add(pph.ProductionPlanDate.ToString());
                        item1.SubItems.Add(pph.StockItemID + "-" + pph.StockItemName);
                        item1.SubItems.Add(pph.ModelNo.ToString());
                        item1.SubItems.Add(pph.ModelName.ToString());
                        item1.SubItems.Add(pph.Quantity.ToString());
                        item1.SubItems.Add(pph.ProductionStatus.ToString());
                        item1.SubItems.Add(pph.PlannedStartTime.ToString());
                        item1.SubItems.Add(pph.PlannedEndTime.ToString());
                        lv.Items.Add(item1);
                    }
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public Boolean updateProductionStatus(int stat, productionplanheader pph,string store)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
               string updateSQL1 = "update ProductionPlanHeader set ProductionStatus=" + stat +
                      ", ActualStartTime = '" + pph.ActualStartTime.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                     ", ActualEndTime = '" + pph.ActualEndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                    " where ProductionPlanNo='" + pph.ProductionPlanNo + "'" +
                    " and ProductionPlanDate='" + pph.ProductionPlanDate.ToString("yyyy-MM-dd") + "'";
                string updateSQL2 = "update ProductionPlanHeader set ProductionStatus=" + stat + 
                    ", ActualStartTime = '"+ pph.ActualStartTime.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                     ", ActualEndTime = '" + pph.ActualEndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                   " RawMaterialUsage = 0 where ProductionPlanNo='" + pph.ProductionPlanNo + "'" +
                   " and ProductionPlanDate='" + pph.ProductionPlanDate.ToString("yyyy-MM-dd") + "'";
                switch (stat)
                {
                    case 5:
                        updateSQL = updateSQL2;
                        break;
                    case 2:
                        updateSQL = updateSQL1;
                        break;
                    default:
                        updateSQL = updateSQL1;
                        break;
                }
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductionPlanHeader", "", updateSQL) +
                Main.QueryDelimiter;

                if(stat == 99)
                {
                    updateSQL = "insert into Stock " +
                   "(FYID,StockItemID,ModelNo,InwardDocumentID,InwardDocumentNo,InwardDocumentDate,InwardQuantity,PresentStock,MRNNo,MRNDate," +
                   "BatchNo,SerialNo,ExpiryDate,PurchaseQuantity,PurchasePrice,PurchaseTax,SupplierID,StoreLocation," +
                   "CreateTime,CreateUser,IssueQuantity)" +
                   " values (" +
                   "'" + Main.currentFY + "'," +
                   "'" + pph.StockItemID + "'," +
                     "'" + pph.ModelNo + "'," +
                   "'" + pph.DocumentID + "'," +
                   pph.ProductionPlanNo + "," +
                   "'" + pph.ProductionPlanDate.ToString("yyyy-MM-dd") + "'," +
                   pph.Quantity + "," +
                   pph.Quantity + "," +
                    "0" + "," +
                   "'" + DateTime.Parse("1900-01-01").ToString("yyyy-MM-dd") + "'," +
                   "'" + "" + "'," +
                  "'" + "" + "'," +
                   "'" + DateTime.Parse("1900-01-01").ToString("yyyy-MM-dd") + "'," +
                   "0" + "," +
                   "0" + "," +
                   "0" + "," +
                   "'" + "" + "'," +
                   "'" + store + "'," +
                   "GETDATE()" + "," +
                   "'" + Login.userLoggedIn + "',0)";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "Stock", "", updateSQL) +
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


        public Boolean updatePPlanHeaderAndDetail( productionplanheader pph, productionplanheader prevpph, List<productionplandetail> ProductionDetails)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductionPlanHeader set ProductionPlanNo='" + pph.ProductionPlanNo +
                    "',ProductionPlanDate='" + pph.ProductionPlanDate.ToString("yyyy-MM-dd") +
                     "',InternalOrderNos='" + pph.InternalOrderNos +
                     "',InternalOrderDates='" + pph.InternalOrderDates +
                    "', Reference='" + pph.Reference +
                    "', StockItemID='" + pph.StockItemID +
                        "', ModelNo='" + pph.ModelNo +
                    "', Quantity='" + pph.Quantity +
                     "',PlannedStartTime='" + pph.PlannedStartTime.ToString("yyyy-MM-dd HH:mm:ss") +
                      "',PlannedEndTime='" + pph.PlannedEndTime.ToString("yyyy-MM-dd HH:mm:ss") +
                       "',ActualStartTime='" + pph.ActualStartTime.ToString("yyyy-MM-dd") +
                        "',ActualEndTime='" + pph.ActualEndTime.ToString("yyyy-MM-dd") +
                         "', FloorManager='" + pph.FloorManager +
                          "', Remarks='" + pph.Remarks +
                           "',ProductionStatus='" + pph.ProductionStatus +
                    "', Comments='" + pph.Comments +
                         "', CommentStatus='" + pph.CommentStatus +
                           "', ForwarderList='" + pph.ForwarderList + "'" +
                        " where DocumentID='" + prevpph.DocumentID + "'" +
                    " and TemporaryNo=" + prevpph.TemporaryNo +
                    " and TemporaryDate='" + prevpph.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductionPlanHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from ProductionPlanDetail where DocumentID='" + prevpph.DocumentID + "'" +
                   " and TemporaryNo=" + prevpph.TemporaryNo +
                   " and TemporaryDate='" + prevpph.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "ProductionPlanDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (productionplandetail ppd in ProductionDetails)
                {
                    updateSQL = "insert into ProductionPlanDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,SlNo,ProcessID,TeamMembers,StartTime,EndTime,ActualStartTime,ActualEndTime,ProcessStatus,Remarks) " +
                    "values ('" + ppd.DocumentID + "'," +
                    ppd.TemporaryNo + "," +
                    "'" + ppd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    ppd.SlNo + "," +
                    "'" + ppd.ProcessID + "'," +
                     "'" + ppd.TeamMembers + "'," +
                      "'" + ppd.StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                      "'" + ppd.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                      "'" + ppd.ActualStartTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    "'" + ppd.ActualEndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                   ppd.ProcessStatus + "," +
                    "'" + ppd.Remarks + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductionPlanDetail", "", updateSQL) +
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
        public Boolean InsertPPlanHeaderAndDetail(productionplanheader pph, List<productionplandetail> ProductionDetails)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                pph.TemporaryNo = DocumentNumberDB.getNumber(pph.DocumentID, 1);
                if (pph.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + pph.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + pph.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into ProductionPlanHeader " +
                    "(DocumentID,TemporaryNo,TemporaryDate,ProductionPlanNo,ProductionPlanDate,InternalOrderNos,InternalOrderDates,Reference,StockItemID,ModelNo,Quantity," +
                    "PlannedStartTime,PlannedEndTime,ActualStartTime,ActualEndTime,FloorManager,Remarks,Status,DocumentStatus,ProductionStatus,CommentStatus,CreateUser,CreateTime," +
                    "ForwarderList)" +
                    " values (" +
                    "'" + pph.DocumentID + "'," +
                    pph.TemporaryNo + "," +
                    "'" + pph.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                   pph.ProductionPlanNo + "," +
                    "'" + pph.ProductionPlanDate.ToString("yyyy-MM-dd") + "'," +
                    "'"+pph.InternalOrderNos + "'," +
                    "'" + pph.InternalOrderDates + "'," +
                    "'" + pph.Reference + "'," +
                    "'" + pph.StockItemID + "'," +
                       "'" + pph.ModelNo + "'," +
                     pph.Quantity + "," +
                    "'" + pph.PlannedStartTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    "'" + pph.PlannedEndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    "'" + pph.ActualStartTime.ToString("yyyy-MM-dd") + "'," +
                    "'" + pph.ActualEndTime.ToString("yyyy-MM-dd") + "'," +
                    "'" + pph.FloorManager + "'," +
                      "'" + pph.Remarks + "'," +
                        pph.Status + "," +
                          pph.DocumentStatus + "," +
                            pph.ProductionStatus + "," +
                     //  "'" + pph.Comments + "'," +
                     "'" + pph.CommentStatus + "'," +
                      "'" + Login.userLoggedIn + "'," +
                      "GETDATE()" + "," +
                    "'" + pph.ForwarderList + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductionPlanHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from ProductionPlanDetail where DocumentID='" + pph.DocumentID + "'" +
                    " and TemporaryNo=" + pph.TemporaryNo +
                    " and TemporaryDate='" + pph.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "ProductionPlanDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (productionplandetail ppd in ProductionDetails)
                {
                    updateSQL = "insert into ProductionPlanDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,SlNo,ProcessID,TeamMembers,StartTime,EndTime,ActualStartTime,ActualEndTime,ProcessStatus,Remarks) " +
                    "values ('" + ppd.DocumentID + "'," +
                    pph.TemporaryNo + "," +
                    "'" + ppd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    ppd.SlNo + "," +
                    "'" + ppd.ProcessID + "'," +
                     "'" + ppd.TeamMembers + "'," +
                      "'" + ppd.StartTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                      "'" + ppd.EndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                      "'" + ppd.ActualStartTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    "'" + ppd.ActualEndTime.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                   ppd.ProcessStatus + "," +
                    "'" + ppd.Remarks + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductionPlanDetail", "", updateSQL) +
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

        public List<productionplanheader> getProductionPlanForRawMaterialMainGrid()
        {
            productionplanheader pph;
            List<productionplanheader> ProductionPlanHeaderList = new List<productionplanheader>();
            try
            {
                string query = "select TemporaryNo,TemporaryDate,ProductionPlanNo,ProductionPlanDate," +
                    " InternalOrderNos,InternalOrderDates,Reference,StockItemID,StockItemName,ModelNo,ModelName,Quantity" +
                    " from ViewProductionPlanHeader" +
                    " where  DocumentStatus = 99 and status = 1 and RawMaterialUsage <> 1 order by ProductionPlanDate desc,ProductionPlanNo desc";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        pph = new productionplanheader();
                        pph.TemporaryNo = reader.GetInt32(0);
                        pph.TemporaryDate = reader.GetDateTime(1);
                        pph.ProductionPlanNo = reader.GetInt32(2);
                        pph.ProductionPlanDate = reader.GetDateTime(3);
                        pph.InternalOrderNos = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        pph.InternalOrderDates = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        pph.Reference = reader.GetString(6);
                        pph.StockItemID = reader.GetString(7);
                        pph.StockItemName = reader.GetString(8);
                        pph.ModelNo = reader.IsDBNull(9) ? "NA" : reader.GetString(9);
                        pph.ModelName = reader.IsDBNull(10) ? "NA" : reader.GetString(10);
                        pph.Quantity = reader.GetDouble(11);
                        ProductionPlanHeaderList.Add(pph);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Production Header Details");
            }
            return ProductionPlanHeaderList;
        }
        public static Boolean FinalizeRawmaterialusageForPlan(int planNo,DateTime planDate, List<stockissuedetail> SIDetails)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductionPlanHeader set RawMaterialUsage= 1 where ProductionPlanNo=" + planNo +
                    " and ProductionPlanDate='" + planDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "ProductionPlanHeader", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (stockissuedetail sid in SIDetails)
                {
                    updateSQL = "update Stock set  " +
                    " PresentStock=" + "( (select PresentStock from Stock where RowID = " + sid.StockReferenceNo + ")+" + sid.ReturnedQuantity + ")" +
                    " where RowID=" + sid.StockReferenceNo;
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
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

        public productionplanheader getProductionPlanHeaderDetail(int planNo,DateTime planDate)
        {
            productionplanheader pph = new productionplanheader();
            try
            {
                string query = "select RowID,DocumentID,DocumentName,TemporaryNo,TemporaryDate,ProductionPlanNo,ProductionPlanDate," +
                   " InternalOrderNos,InternalOrderDates,Reference,StockItemID,StockItemName,ModelNo,ModelName,Quantity" +
                   " from ViewProductionPlanHeader" +
                   " where  ProductionPlanNo = " + planNo + " and ProductionPlanDate = '" + planDate.ToString("yyyy-MM-dd") + "'";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    try
                    {
                        pph.RowID = reader.GetInt32(0);
                        pph.DocumentID = reader.GetString(1);
                        pph.DocumentName = reader.GetString(2);
                        pph.TemporaryNo = reader.GetInt32(3);
                        pph.TemporaryDate = reader.GetDateTime(4);
                        pph.ProductionPlanNo = reader.GetInt32(5);
                        pph.ProductionPlanDate = reader.GetDateTime(6);
                        pph.InternalOrderNos = reader.IsDBNull(7) ? "NA" : reader.GetString(7);
                        pph.InternalOrderDates = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        pph.Reference = reader.GetString(9);
                        pph.StockItemID = reader.GetString(10);
                        pph.StockItemName = reader.GetString(11);
                        pph.ModelNo = reader.IsDBNull(12) ? "NA" : reader.GetString(12);
                        pph.ModelName = reader.IsDBNull(13) ? "NA" : reader.GetString(13);
                        pph.Quantity = reader.GetDouble(14);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Production Header Details");
            }
            return pph;
        }

        //Date: 25-10-2017
        //Filtering All Production plan that Not Closed
        public List<productionplanheader> getUnClosedProductionPlanForStockIssue()
        {
            productionplanheader pph;
            List<productionplanheader> ProductionPlanHeaderList = new List<productionplanheader>();
            try
            {
                string query = "select TemporaryNo,TemporaryDate,ProductionPlanNo,ProductionPlanDate," +
                    " StockItemID,StockItemName,ModelNo,ModelName,Quantity" +
                    " from ViewProductionPlanHeader" +
                    " where Status = 1 and DocumentStatus = 99 and ProductionStatus in (1,2,5) and RawMaterialUsage = 0 " +
                    "order by ProductionPlanDate desc,ProductionPlanNo desc";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        pph = new productionplanheader();
                        pph.TemporaryNo = reader.GetInt32(0);
                        pph.TemporaryDate = reader.GetDateTime(1);
                        pph.ProductionPlanNo = reader.GetInt32(2);
                        pph.ProductionPlanDate = reader.GetDateTime(3);
                        pph.StockItemID = reader.GetString(4);
                        pph.StockItemName = reader.GetString(5);
                        pph.ModelNo = reader.IsDBNull(6) ? "NA" : reader.GetString(6);
                        pph.ModelName = reader.IsDBNull(7) ? "NA" : reader.GetString(7);
                        pph.Quantity = reader.GetDouble(8);
                        ProductionPlanHeaderList.Add(pph);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
           catch(Exception ex)
            {

            }
            return ProductionPlanHeaderList;
        }
        //Date: 25-10-2017
        //check atleast one process should be there if RawMaterial is not issued againist perticular Production Plan
        public static Boolean checkProductionProcessStatusAgainstRawmaterialIssue(productionplandetail ppd, string TempNo, DateTime TempDate)
        {
            Boolean status = false;
            int count = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select count(*) from ProductionPlanDetail where DocumentID='" + ppd.DocumentID + "'" +
                        " and TemporaryNo=" + TempNo +
                        " and TemporaryDate='" + TempDate.ToString("yyyy-MM-dd") + "' and ProcessStatus in (0,1)";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    count = reader.GetInt32(0);
                }
                conn.Close();
                if(count == 1)
                {
                    int str = ppd.ProcessStatus;
                    int RawmatStat = ProductionPlanHeaderDB.checkRawMaterialStatusOfAProdPlan(TempNo, TempDate, ppd.DocumentID);
                    if(RawmatStat == 1)
                    {
                        status = true;
                    }
                    else if(str == 0)
                    {
                        status = true;
                    }
                    else
                    {
                        status = false;
                    }
                }
                else
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public static int checkRawMaterialStatusOfAProdPlan(string TempNo, DateTime TempDate,string DocID)
        {
            int rawMatStat = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RawMaterialUsage from ProductionPlanHeader where DocumentID='" + DocID + "'" +
                        " and TemporaryNo=" + TempNo +
                        " and TemporaryDate='" + TempDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    rawMatStat = reader.IsDBNull(0)?0:reader.GetInt32(0);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return rawMatStat;
        }
        public static Boolean checkproductionClosedStatus(int planNo, DateTime planDate)
        {
            int rawMatStat = 0;
            int tempNo = 0;
            Boolean status = false;
            DateTime tempDate = DateTime.Parse("1900-01-01"); 
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                //Get TemporaryNo And TemporaryDate
                string query = "select TemporaryNo,TemporaryDate from ProductionPlanHeader where DocumentID='PRODUCTIONPLAN'" +
                        " and ProductionPlanNo=" + planNo +
                        " and ProductionPlanDate='" + planDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    tempNo = reader.GetInt32(0);
                    tempDate = reader.GetDateTime(1);
                }
                conn.Close();
                //get PlanNo and Plan Date
                string query1 = "select count(*) from ProductionPlanDetail where DocumentID='PRODUCTIONPLAN'" +
                       " and TemporaryNo=" + tempNo +
                       " and TemporaryDate='" + tempDate.ToString("yyyy-MM-dd") + "' and ProcessStatus in (0,1)";
                SqlCommand cmd1 = new SqlCommand(query1, conn);
                conn.Open();
                SqlDataReader reader1 = cmd1.ExecuteReader();
               
                if (reader1.Read())
                {
                    if (reader1.GetInt32(0) == 0)
                        status = true;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        public static List<productionplanheader> getProductionDetailsForPerticularIO(int ioNo, DateTime ioDate)
        {
            productionplanheader pph;
            List<productionplanheader> ProductionPlanHeaderList = new List<productionplanheader>();
            try
            {
                string query = "select ProductionPlanNo,ProductionPlanDate," +
                    " StockItemID,StockItemName,ModelNo,ModelName,Quantity,PlannedStartTime,PlannedEndTime,ActualStartTime,ActualEndTime,ProductionStatus" +
                    " ,InternalOrderNos,InternalOrderDates,ProductionStatusString from ViewProductionPlanHeader" +
                    " where  DocumentStatus = 99 and Status = 1 and InternalOrderNos like '%;" + ioNo +
                    ";%' and InternalOrderDates like '%;"+ ioDate.ToString("yyyy-MM-dd") + ";%' order by ProductionPlanDate desc,ProductionPlanNo desc";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        string inos = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        string idates = reader.IsDBNull(13) ? "" : reader.GetString(13);
                        string[] inoArr = inos.Split(';');
                        string[] idateArr = idates.Split(';');
                        int i = Array.IndexOf(inoArr, ioNo.ToString());
                        if(i!= -1)
                        {
                            if(idateArr[i] == ioDate.ToString("yyyy-MM-dd"))
                            {
                                pph = new productionplanheader();
                                pph.ProductionPlanNo = reader.GetInt32(0);
                                pph.ProductionPlanDate = reader.GetDateTime(1);
                                pph.StockItemID = reader.GetString(2);
                                pph.StockItemName = reader.GetString(3);
                                pph.ModelNo = reader.IsDBNull(4) ? "NA" : reader.GetString(4);
                                pph.ModelName = reader.IsDBNull(5) ? "NA" : reader.GetString(5);
                                pph.Quantity = reader.GetDouble(6);
                                pph.PlannedStartTime = reader.GetDateTime(7);
                                pph.PlannedEndTime = reader.GetDateTime(8);
                                pph.ActualStartTime = reader.GetDateTime(9);
                                pph.ActualEndTime = reader.GetDateTime(10);
                                pph.ProductionStatus = reader.GetInt32(11);
                                pph.ProductionStatusString = reader.IsDBNull(14) ? "" : reader.GetString(14);
                                ProductionPlanHeaderList.Add(pph);
                            }
                        }
                       
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in retriving ProductionPlan Dettail");
            }
            return ProductionPlanHeaderList;
        }
        //Production Plan details List In GridViewNew
        public DataGridView getGridViewOfPlanDetail(int ioNo, DateTime ioDate)
        {
            DataGridView grdIODetail = new DataGridView();
            try
            {
                string[] strColArr = { "PlanNo", "PlanDate", "StockItemName", "Quantity", "PlannedStartTime", "PlannedEndTime", "ActualStartTime","ActualEndTime","ProdStatus" };
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                    ,new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                };

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdIODetail.EnableHeadersVisualStyles = false;
                grdIODetail.AllowUserToAddRows = false;
                grdIODetail.AllowUserToDeleteRows = false;
                grdIODetail.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdIODetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdIODetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdIODetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdIODetail.ColumnHeadersHeight = 27;
                grdIODetail.RowHeadersVisible = false;
                grdIODetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str;
                    colArr[index].HeaderText = str;
                    colArr[index].ReadOnly = true;
                    if (index > 3)
                        colArr[index].Width = 120;
                    else if(index == 0 || index == 1 || index == 3)
                        colArr[index].Width = 80;
                    else if (index == 2 )
                        colArr[index].Width = 200;
                    else
                        colArr[index].Width = 100;
                    if (index == 1)
                        colArr[index].DefaultCellStyle.Format = "dd-MM-yyyy";
                    if (index == 4 || index == 5 || index == 6 || index == 7)
                        colArr[index].DefaultCellStyle.Format = "dd-MM-yyyy HH:mm:ss";
                    //if (index == 2)
                    //    colArr[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    grdIODetail.Columns.Add(colArr[index]);
                }

                List<productionplanheader> planList = ProductionPlanHeaderDB.getProductionDetailsForPerticularIO(ioNo, ioDate);
                foreach (productionplanheader pph in planList)
                {
                    grdIODetail.Rows.Add();
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells[strColArr[0]].Value = pph.ProductionPlanNo;
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells[strColArr[1]].Value = pph.ProductionPlanDate;
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells[strColArr[2]].Value = pph.StockItemName;
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells[strColArr[3]].Value = pph.Quantity;
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells[strColArr[4]].Value = pph.PlannedStartTime;
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells[strColArr[5]].Value = pph.PlannedEndTime;
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells[strColArr[6]].Value = pph.ActualStartTime;
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells[strColArr[7]].Value = pph.ActualEndTime;
                    //grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells[strColArr[8]].Value = getProdStringStatus(pph.ProductionStatus);
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells[strColArr[8]].Value = pph.ProductionStatusString;
                }
            }
            catch (Exception ex)
            {
            }

            return grdIODetail;
        }
        private string getProdStringStatus(int stat)
        {
            string str = "";
            if (stat == 99)
                str = "Completed";
            else if (stat == 1)
                str = "Started/Opened";
            else
                str = "Not Started";
            return str;
        }

        public static double[] getNoOfProdPlanPreparedPerIO(int ioNo, DateTime ioDate)
        {
            double[] str = new double[2];
            try
            {
                string query = "select Quantity,InternalOrderNos,InternalOrderDates from ProductionPlanHeader " +
                    " where  DocumentStatus = 99 and Status = 1 and InternalOrderNos like '%;" + ioNo +
                    ";%' and InternalOrderDates like '%;" + ioDate.ToString("yyyy-MM-dd") + ";%'";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string inos = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    string idates = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    string[] inoArr = inos.Split(';');
                    string[] idateArr = idates.Split(';');
                    int i = Array.IndexOf(inoArr, ioNo.ToString());
                    if (i != -1)
                    {
                        if (idateArr[i] == ioDate.ToString("yyyy-MM-dd"))
                        {
                            str[0] = 1;
                            str[1] = str[1] + reader.GetDouble(0);
                        }
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in retriving ProductionPlan Dettail");
            }
            return str;
        }
        public static DateTime[] getActualStartAndEndTimeForAPlan(int planNO, DateTime PlanDate)
        {
            DateTime[] dtArr = new DateTime[2];
            int tempNo = 0;
            DateTime tempDate = DateTime.Parse("1900-01-01");
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                //Get TemporaryNo And TemporaryDate
                string query = "select TemporaryNo,TemporaryDate from ProductionPlanHeader where DocumentID='PRODUCTIONPLAN'" +
                        " and ProductionPlanNo=" + planNO +
                        " and ProductionPlanDate='" + PlanDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    tempNo = reader.GetInt32(0);
                    tempDate = reader.GetDateTime(1);
                }
                conn.Close();
                string query1 = "select MIN(ActualStartTime) as ActualStartTime, MAX(ActualEndTime) as ActualEndTime from ProductionPlanDetail" +
                    " where TemporaryNo = " + tempNo +
                    " and TemporaryDate = '" + tempDate.ToString("yyyy-MM-dd") + "'";

                SqlCommand cmd1 = new SqlCommand(query1, conn);
                conn.Open();
                SqlDataReader reader1 = cmd1.ExecuteReader();
                while (reader1.Read())
                {
                    dtArr[0] = reader1.GetDateTime(0);
                    dtArr[1] = reader1.GetDateTime(1);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in retriving ProductionPlan Dettail");
            }
            return dtArr;
        }
        public static string getCustomerListFromReference(string poref)
        {
            string custList = "";
            try
            {
                string[] PoRefMain = poref.Trim().Split(Main.delimiter2); //splits ref wrt internal orders (PAFPRODUCTINWARD;437(2018-01-03)ÞPAFPRODUCTINWARD;427(2017-12-03)Þ,
                                                                          //POPRODUCTINWARD; 431(2017 - 12 - 26)Þ,
                                                                          //POPRODUCTINWARD; 401(2017 - 12 - 22)Þ,""
                foreach (string strMain in PoRefMain)
                {
                    if (strMain.Trim().Length != 0)
                    {
                        if (!strMain.Contains("Projection"))
                        {
                            string[] PoRefSub = strMain.Trim().Split(Main.delimiter1);  // splits ref wrt PO (PAFPRODUCTINWARD;437(2018-01-03),PAFPRODUCTINWARD;427(2017-12-03),"")
                            foreach (string strSub in PoRefSub)
                            {
                                if (strSub.Trim().Length != 0)
                                {
                                    string[] poStr = strSub.Trim().Split(';');
                                    string docid = poStr[0];
                                    int pono = Convert.ToInt32(poStr[1].Substring(0, poStr[1].IndexOf('(')));
                                    string str = poStr[1].Substring(poStr[1].IndexOf('(') + 1).Replace(")", string.Empty);
                                    DateTime podate = Convert.ToDateTime(poStr[1].Substring(poStr[1].IndexOf('(') + 1).Replace(")", string.Empty));
                                    string custname = ProductionPlanHeaderDB.getCUstomerListFromPO(pono, podate, docid);
                                    custList = custList + Environment.NewLine + custname;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return custList;
        }
        private static string getCUstomerListFromPO(int trackNo, DateTime trackDate, string docid)
        {
            string custName = "" ;
            try
            {
                string query = "select a.CustomerID,b.Name from POProductInwardHeader a , Customer b where a.CustomerID = b.CustomerID"+
                    " and  a.DocumentID = '"+ docid + "' and a.TrackingNo = " + trackNo + 
                    " and a.TrackingDate = '" + trackDate.ToString("yyyy-MM-dd") + "'";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    custName = reader.GetString(0)+ ":" + reader.GetString(1);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in retriving ProductionPlan Dettail");
            }
            return custName;
        }

        //For Production plan Header listing in Production process status(only Ongoing plans)
        public List<productionplanheader> getOnGoingProductionPlansForProcessStatus(int opt)
        {
            productionplanheader pph;
            string query = "";
            List<productionplanheader> ProductionPlanHeaderList = new List<productionplanheader>();
            try
            {
                string query1 = "select RowID,DocumentID,DocumentName,TemporaryNo,TemporaryDate,ProductionPlanNo,ProductionPlanDate," +
                    " InternalOrderNos,InternalOrderDates,Reference,StockItemID,StockItemName,ModelNo,ModelName,Quantity,PlannedStartTime,PlannedEndTime,ActualStartTime,ActualEndTime,FloorManager,Remarks," +
                    " Status,DocumentStatus,ProductionStatus,ProductionStatusString" +
                    " from ViewProductionPlanHeader" +
                    " where  DocumentStatus = 99 and Status = 1 and ProductionStatus in (1,2,5)";

                string query2 = "select RowID,DocumentID,DocumentName,TemporaryNo,TemporaryDate,ProductionPlanNo,ProductionPlanDate," +
                   " InternalOrderNos,InternalOrderDates,Reference,StockItemID,StockItemName,ModelNo,ModelName,Quantity,PlannedStartTime,PlannedEndTime,ActualStartTime,ActualEndTime,FloorManager,Remarks," +
                   " Status,DocumentStatus,ProductionStatus,ProductionStatusString" +
                   " from ViewProductionPlanHeader" +
                   " where  DocumentStatus = 99 and Status = 1 and ProductionStatus in (1,2,4,5)";

                // 1: Production Process(will show approved,started ), 2: ProductionStatus change (will show approved, started, halted, resumed)
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
                while (reader.Read())
                {
                    try
                    {
                        pph = new productionplanheader();
                        pph.RowID = reader.GetInt32(0);
                        pph.DocumentID = reader.GetString(1);
                        pph.DocumentName = reader.GetString(2);
                        pph.TemporaryNo = reader.GetInt32(3);
                        pph.TemporaryDate = reader.GetDateTime(4);
                        pph.ProductionPlanNo = reader.GetInt32(5);
                        pph.ProductionPlanDate = reader.GetDateTime(6);
                        pph.InternalOrderNos = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        pph.InternalOrderDates = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        pph.Reference = reader.GetString(9);
                        pph.StockItemID = reader.GetString(10);
                        pph.StockItemName = reader.GetString(11);
                        pph.ModelNo = reader.IsDBNull(12) ? "NA" : reader.GetString(12);
                        pph.ModelName = reader.IsDBNull(13) ? "NA" : reader.GetString(13);
                        pph.Quantity = reader.GetDouble(14);
                        pph.PlannedStartTime = reader.GetDateTime(15);
                        pph.PlannedEndTime = reader.GetDateTime(16);
                        pph.ActualStartTime = reader.GetDateTime(17);
                        pph.ActualEndTime = reader.GetDateTime(18);
                        pph.FloorManager = reader.GetString(19);
                        pph.Remarks = reader.GetString(20);
                        pph.Status = reader.GetInt32(21);
                        pph.DocumentStatus = reader.GetInt32(22);
                        pph.ProductionStatus = reader.GetInt32(23);
                        pph.ProductionStatusString = reader.IsDBNull(24) ? "" : reader.GetString(24);
                        ProductionPlanHeaderList.Add(pph);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Production Header Details");
            }
            return ProductionPlanHeaderList;
        }

    }
}
