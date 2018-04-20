using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class leaveauthority
    {
        public int rowid { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Leavetype { get; set; }
        public int maxdays { get; set; }
        public string leaveid { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public DateTime sanctionedFrom { get; set; }
        public DateTime sanctionedTo { get; set; }
        public int status { get; set; }
        public int documentStatus { get; set; }
        public int leavepending { get; set; }
        public int leavestatus { get; set; }
        public string remarks { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; } // user id
        public string ForwardUser { get; set; } //user id
        public string ApproveUser { get; set; } //user id
        public string CreateUserName { get; set; } // user name
        public string ForwardUserName { get; set; } //user name
        public string ApproveUserName { get; set; } //user name
        public string ForwarderList { get; set; }
    }

    class LeaveAuthorityDB
    {
        ActivityLogDB alDB = new ActivityLogDB();


        public List<leaveauthority> getFilteredLeaveApproval(string userList, int opt)
        {
            leaveauthority lvpr;
            List<leaveauthority> LeaveApr = new List<leaveauthority>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query1 = "select b.EmployeeID,b.Name,c.Description, a.FromDate,a.ToDate,a.DocumentStatus,"+
                                 "a.Status,a.LeaveRequestStatus,a.Remarks,a.ForwardUser,a.ForwarderList,a.ApproveUser,a.CreateTime,c.LeaveID,a.RowID,a.SanctionedFromDate,a.SanctionedToDate" +
                                  " from LeaveRequest a, Employee b,LeaveType c, ERPUser d"+
                                  " where b.EmployeeID = d.EmployeeID  and a.LeaveID = c.LeaveID and a.UserID = d.UserID and a.Status = 1 and a.DocumentStatus between 2 and 99"+
                                    " and  a.LeaveRequestStatus in (1,2,4,5) ";

                string query3 = "select b.EmployeeID,b.Name,c.Description, a.FromDate,a.ToDate,a.DocumentStatus, "+
                                 "a.Status,a.LeaveRequestStatus,a.Remarks,a.ForwardUser,a.ForwarderList,a.ApproveUser,a.CreateTime,c.LeaveID,a.RowID,a.SanctionedFromDate,a.SanctionedToDate " +
                                 " from LeaveRequest a, Employee b,LeaveType c, ERPUser d where b.EmployeeID = d.EmployeeID "+
                                 " and a.LeaveID = c.LeaveID and a.UserID = d.UserID and a.Status <> 2 and "+
                                "   a.LeaveRequestStatus not in (4,8)";
                string query = "";
                switch (opt)
                {
                    case 1:
                        query = query1;
                        break;
                    case 3:
                        query = query3;
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
                    lvpr = new leaveauthority();
                    lvpr.EmployeeID = reader.GetString(0);
                    lvpr.EmployeeName = reader.GetString(1);
                    lvpr.Leavetype = reader.GetString(2);
                    lvpr.fromdate = reader.GetDateTime(3);
                    lvpr.todate = reader.GetDateTime(4);
                    lvpr.documentStatus = reader.GetInt32(5);
                    lvpr.status = reader.GetInt32(6);
                    lvpr.leavestatus = reader.GetInt32(7);
                    lvpr.remarks = reader.GetString(8);
                    lvpr.ForwardUser = reader.IsDBNull(9) ? "" : reader.GetString(9);
                    lvpr.ForwarderList = reader.IsDBNull(10) ? "" : reader.GetString(10);
                    lvpr.ApproveUser = reader.IsDBNull(11)?"": reader.GetString(11);
                    lvpr.CreateTime = reader.GetDateTime(12);
                    lvpr.leaveid = reader.GetString(13);
                    lvpr.rowid = reader.GetInt32(14);
                    lvpr.sanctionedFrom=reader.IsDBNull(15)?DateTime.Now: reader.GetDateTime(15);
                    lvpr.sanctionedTo= reader.IsDBNull(16) ? DateTime.Now : reader.GetDateTime(16);
                    LeaveApr.Add(lvpr);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return LeaveApr;
        }


        public List<leaveauthority> getLeaveLimit(string empid)
        {
            leaveauthority lvapp;
            List<leaveauthority> LVlist = new List<leaveauthority>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.LeaveCount,a.LeaveID from LeaveOB a where EmployeeID='"+empid+ "'and a.year=year(GetDate())";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvapp = new leaveauthority();
                    lvapp.maxdays = reader.GetInt32(0);
                    lvapp.leaveid = reader.GetString(1);
                    LVlist.Add(lvapp);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return LVlist;
        }

        public List<leaveauthority> getLeaveRemain(string empid,string leaveid)
        {
            leaveauthority lvapp;
            List<leaveauthority> LVlist = new List<leaveauthority>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select  DATEDIFF(DAY,a.SanctionedFromDate ,a.SanctionedToDate ) as totaldays  from LeaveRequest a, "+
                                "(select b.UserID  from ViewUserEmployeeList b where b.EmployeeID = '"+empid+"') c "+
                               " where a.UserID = c.UserID and a.LeaveID = '"+leaveid+ "' and a.Status = 1 and a.DocumentStatus = 99 and a.LeaveRequestStatus in (1,11,10,9,7) and year(a.SanctionedFromDate)=year(GetDate()) ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvapp = new leaveauthority();
                    lvapp.leavepending =reader.IsDBNull(0)? 0 : reader.GetInt32(0);
                    LVlist.Add(lvapp);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return LVlist;
        }

        public Boolean forwardleave(leaveauthority lv)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update LeaveRequest set DocumentStatus += 1 , forwardUser='" + lv.ForwardUser + "'" +
                     ", Remarks='" + lv.remarks + "'" +
                    ", ForwarderList='" + lv.ForwarderList + "'" +
                    " where RowID='"+lv.rowid+"'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("forward", "LeaveRequest", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }

        public Boolean ApproveLeaveRequest(leaveauthority lv)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update LeaveRequest set  Remarks='"+lv.remarks+"', "+
                                   " SanctionedFromDate = '"+lv.sanctionedFrom.ToString("yyyy-MM-dd")+"', SanctionedToDate ='"+lv.sanctionedTo.ToString("yyyy-MM-dd")+"',"+
                                    "ForwardUser='" + Login.userLoggedIn+"',ForwarderList='"+lv.ForwarderList+"', documentstatus=99, ApproveUser='" +Login.userLoggedIn+"', ApproveTime=GetDate() "+
                                   "where RowID='"+lv.rowid+"'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "LeaveRequest", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }

        public Boolean ApproveLeaveRequestCOmpoff(leaveauthority lv,int tp)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                 updateSQL = "update LeaveRequest set  Remarks='" + lv.remarks + "', " +
                                   " SanctionedFromDate = '" + lv.sanctionedFrom.ToString("yyyy-MM-dd") + "', SanctionedToDate ='" + lv.sanctionedTo.ToString("yyyy-MM-dd") + "'," +
                                    "ForwardUser='" + Login.userLoggedIn + "',ForwarderList='" + lv.ForwarderList + "', documentstatus=99, ApproveUser='" + Login.userLoggedIn + "', ApproveTime=GetDate() " +
                                   "where RowID='" + lv.rowid + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "LeaveRequest", "", updateSQL) +
                Main.QueryDelimiter;
                updateSQL = "update LeaveEarning set Status=2 where   RowID in" +
                           " (select top " + tp + " rowid from LeaveEarning where " +
                            " EmployeeID='" + lv.EmployeeID + "' and LeaveID='CO' and Status=1)";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "LeaveEarning", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }

        public Boolean ApproveLeaveRequestModified(leaveauthority lv)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update LeaveRequest set  Remarks='" + lv.remarks + "', SanctionedToDate='"+lv.sanctionedTo+"', " +
                                    "documentstatus=99,LeaveRequestStatus=9 ,ApproveUser='" + Login.userLoggedIn + "', " +
                                   "ApproveTime=GetDate() where RowID='"+lv.rowid+"'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "LeaveRequest", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }

        public Boolean ApproveLeaveRequestModify(leaveauthority lv)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update LeaveRequest set  Remarks='" + lv.remarks + "', " +
                                    "documentstatus=99,LeaveRequestStatus=6 ,ApproveUser='" + Login.userLoggedIn + "' " +
                                   ", ApproveTime=GetDate() where  RowID='" + lv.rowid + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "LeaveRequest", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }

        public Boolean CancelledLeaveRequestModified(leaveauthority lv)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update LeaveRequest set  Remarks='" + lv.remarks + "', " +
                                    "documentstatus=99,LeaveRequestStatus=10  " +
                                   "where  RowID= '"+lv.rowid+"' ";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "LeaveRequest", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean CancelledLeaveRequestModify(leaveauthority lv)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update LeaveRequest set  Remarks='" + lv.remarks + "', " +
                                    "documentstatus=99,LeaveRequestStatus=7  " +
                                   "where  RowID= '" + lv.rowid + "' ";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "LeaveRequest", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }

        public Boolean ApproveCancelRequest(leaveauthority lv)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update LeaveRequest set  Remarks='" + lv.remarks + "', " +
                                    "documentstatus=99,LeaveRequestStatus=5, Status=98 ,ApproveUser='" + Login.userLoggedIn + "' " +
                                   "where UserID in (select userid from ViewUserEmployeeList where EmployeeID = '" + lv.EmployeeID + "') and RowID='"+lv.rowid+"'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "LeaveRequest", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }

        public Boolean CancelCancelRequest(leaveauthority lv)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update LeaveRequest set  Remarks='" + lv.remarks + "', " +
                                    "LeaveRequestStatus=11 " +
                                   "where UserID in (select userid from ViewUserEmployeeList where EmployeeID = '" + lv.EmployeeID + "') and RowID='" + lv.rowid + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "LeaveRequest", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }


        public Boolean reverseLeave(leaveauthority lv)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update LeaveRequest set DocumentStatus= 1, Remarks='"+lv.remarks+"'"+
                      ",LeaveRequestStatus=3  where  RowID='"+lv.rowid+"'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "LeaveRequest", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }

        public Boolean RejectLeave(leaveauthority lv)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update LeaveRequest set " +
                    "Remarks='" + lv.remarks + "'" +
                      ", LeaveRequestStatus=99 " +
                      ", ApproveTime=GetDate() " +
                    " where  RowID='"+lv.rowid+"'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "LeaveRequest", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }

        public Boolean ApproveCheck(leaveauthority lv,double totaldays)
        {
            Boolean status = false;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.MaxSanctionLimit   from LeaveSanctionLimit a,EmployeeDesignation b where b.EmployeeID='"+ Login.empLoggedIn + "' "+
                                    "and a.Designation = b.Designation and a.LeaveID = '"+ lv.leaveid + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    if(reader.GetInt32(0)>=totaldays)
                    {
                        status = true;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        //public static void fillStockItemCombo(System.Windows.Forms.ComboBox cmb, string CategoryName)
        //{
        //    cmb.Items.Clear();
        //    try
        //    {
        //        StockItemDB stockitemdb = new StockItemDB();
        //        List<stockitem> StockItems = stockitemdb.getStockItems();
        //        foreach (stockitem si in StockItems)
        //        {
        //            //if (CategoryName.Length > 0 && CategoryName != si.Category)
        //            //{
        //            //    continue;
        //            //}
        //            if (si.status == 1)
        //            {
        //                cmb.Items.Add(si.StockItemID + "-" + si.Name);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
        //    }
        //}
        //public static void fillStockItemGridViewCombo(DataGridViewComboBoxCell cmb, string CategoryName)
        //{
        //    cmb.Items.Clear();
        //    try
        //    {
        //        StockItemDB stockitemdb = new StockItemDB();
        //        List<stockitem> StockItems = stockitemdb.getStockItems();
        //        foreach (stockitem si in StockItems)
        //        {
        //            //if (CategoryName.Length > 0 && CategoryName != si.Category)
        //            //{
        //            //    continue;
        //            //}
        //            if (si.status == 1)
        //            {
        //                cmb.Items.Add(si.StockItemID + "-" + si.Name);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
        //    }
        //}
        public static ListView getAccountCodeListView()
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
                AccountCodeDB ACDb = new AccountCodeDB();
                List<accountcode> acList = ACDb.getFilteredAccountDetails("", 6);
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Account code", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Account Name", -2, HorizontalAlignment.Left);
                foreach (accountcode ac in acList)
                {

                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(ac.AccountCode);
                    item1.SubItems.Add(ac.Name.ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lv;
        }

        //public static TreeView getStockItemTreeView()
        //{
        //    TreeView tv = new TreeView();
        //    try
        //    {
        //        tv.CheckBoxes = true;
        //        //tv.Location = new System.Drawing.Point(264, 90);
        //        //tv.Size = new System.Drawing.Size(332, 161);
        //        tv.LabelEdit = true;
        //        tv.ShowLines = true;
        //        tv.CheckBoxes = true;

        //        //tv.CheckBoxes = "leaf";
        //        // tv.sho
        //        tv.FullRowSelect = true;
        //        StockItemDB sidb = new StockItemDB();
        //        List<stockitem> stockitems = sidb.getFilteredStockItems("", 6);
        //        TreeNode tNode;
        //        string RootDesc = "";
        //        string SubRootDesc = "";
        //        string SubSubRootDesc = "";
        //        int j = 0;
        //        int k = 0;
        //        int l = 0;
        //        foreach (stockitem item in stockitems)
        //        {
        //            l++;
        //            int n = tv.Nodes.Count;
        //            if (item.Group1CodeDescription != RootDesc)
        //            {
        //                j = 0; k = 0;

        //                tNode = tv.Nodes.Add(item.Group1CodeDescription);
        //                tv.Nodes[n].Nodes.Add(item.Group2CodeDescription);
        //                tv.Nodes[n].Nodes[j].Nodes.Add(item.Group3CodeDescription);
        //                tv.Nodes[n].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID + "-" + item.Name);
        //                RootDesc = item.Group1CodeDescription;
        //                SubRootDesc = item.Group2CodeDescription;
        //                SubSubRootDesc = item.Group3CodeDescription;
        //            }
        //            else
        //            {
        //                if (item.Group2CodeDescription != SubRootDesc)
        //                {
        //                    j = j + 1; k = 0;
        //                    tv.Nodes[n - 1].Nodes.Add(item.Group2CodeDescription);
        //                    tv.Nodes[n - 1].Nodes[j].Nodes.Add(item.Group3CodeDescription);
        //                    tv.Nodes[n - 1].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID + "-" + item.Name);
        //                    SubRootDesc = item.Group2CodeDescription;
        //                    SubSubRootDesc = item.Group3CodeDescription;
        //                }
        //                else
        //                {
        //                    if (item.Group3CodeDescription != SubSubRootDesc)
        //                    {
        //                        k = k + 1;
        //                        tv.Nodes[n - 1].Nodes[j].Nodes.Add(item.Group3CodeDescription);
        //                        tv.Nodes[n - 1].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID + "-" + item.Name);
        //                        SubSubRootDesc = item.Group3CodeDescription;
        //                    }
        //                    else
        //                    {
        //                        tv.Nodes[n - 1].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID + "-" + item.Name);
        //                    }
        //                }
        //            }
        //        }
        //        // MessageBox.Show("Count:" + l);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error in TreeView");
        //    }
        //    return tv;
        //}
        //public static string getUnitForSelectedStockItem(string ItemID, string ItemName)
        //{
        //    stockitem sitem = new stockitem();

        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "select StockItemID, Name,Unit" +
        //            " from ViewStockItem where StockItemID = '" + ItemID + "' and Name = '" + ItemName + "'";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        try
        //        {
        //            if (reader.Read())
        //            {
        //                sitem.StockItemID = reader.GetString(0);
        //                sitem.Name = reader.GetString(1);
        //                sitem.Unit = reader.GetString(2);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error querying Stock Item Data");
        //    }
        //    return sitem.Unit;
        //}
    }
}
