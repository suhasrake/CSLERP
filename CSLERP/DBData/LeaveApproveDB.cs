using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class leaveapprove
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
        public string username { get; set; }
        public int sanctiontype { get; set; }
        public int noofdays { get; set; }
        
    }

    class LeaveApproveDB
    {
        ActivityLogDB alDB = new ActivityLogDB();


        public List<leaveapprove> getFilteredLeaveApproval(string userList, int opt)
        {
            leaveapprove lvpr;
            List<leaveapprove> LeaveApr = new List<leaveapprove>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query1 = "select b.EmployeeID,b.Name,c.Description, a.FromDate,a.ToDate,a.DocumentStatus," +
                                 "a.Status,a.LeaveRequestStatus,a.Remarks,a.ForwardUser,a.ForwarderList,a.ApproveUser,a.CreateTime,c.LeaveID,a.RowID,a.SanctionedFromDate,a.SanctionedToDate,c.SanctionType" +
                                  " from LeaveRequest a, Employee b,LeaveType c, ERPUser d" +
                                  " where b.EmployeeID = d.EmployeeID  and a.LeaveID = c.LeaveID and a.UserID = d.UserID and a.Status = 1 and a.DocumentStatus between 2 and 99" +
                                    " and a.LeaveRequestStatus in (1,2,4,5,8) and a.ForwardUser = '" + Login.userLoggedIn + "'";

                string query2 = "select b.EmployeeID,b.Name,c.Description, a.FromDate,a.ToDate,a.DocumentStatus, " +
                                " a.Status,a.LeaveRequestStatus,a.Remarks,a.ForwardUser,a.ForwarderList,a.ApproveUser,a.CreateTime,c.LeaveID,a.RowID,a.SanctionedFromDate,a.SanctionedToDate,c.SanctionType " +
                                 " from LeaveRequest a, Employee b,LeaveType c, ERPUser d " +
                                 " where b.EmployeeID = d.EmployeeID  and a.LeaveID = c.LeaveID and a.UserID = d.UserID and a.Status = 1 and a.DocumentStatus between 2 and 99 " +
                                  " and a.LeaveRequestStatus in (1,2,4,5)  and ForwarderList like '%" + userList + "%'  and ForwardUser <> '" + Login.userLoggedIn + "'";

                string query3 = "select b.EmployeeID,b.Name,c.Description, a.FromDate,a.ToDate,a.DocumentStatus, " +
                                 "a.Status,a.LeaveRequestStatus,a.Remarks,a.ForwardUser,a.ForwarderList,a.ApproveUser,a.CreateTime,c.LeaveID,a.RowID,a.SanctionedFromDate,a.SanctionedToDate,c.SanctionType " +
                                 " from LeaveRequest a, Employee b,LeaveType c, ERPUser d where b.EmployeeID = d.EmployeeID " +
                                 " and a.LeaveID = c.LeaveID and a.UserID = d.UserID and a.Status <> 2 and " +
                                "   a.LeaveRequestStatus not in (4,8)  and " +
                                  "   ForwarderList like '%" + userList + "%'";

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
                    default:
                        query = "";
                        break;
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvpr = new leaveapprove();
                    lvpr.EmployeeID = reader.GetString(0);
                    lvpr.EmployeeName = reader.GetString(1);
                    lvpr.Leavetype = reader.GetString(2);
                    lvpr.fromdate = reader.GetDateTime(3);
                    lvpr.todate = reader.GetDateTime(4);
                    lvpr.documentStatus = reader.GetInt32(5);
                    lvpr.status = reader.GetInt32(6);
                    lvpr.leavestatus = reader.GetInt32(7);
                    lvpr.remarks = reader.GetString(8);
                    lvpr.ForwardUser = reader.GetString(9);
                    lvpr.ForwarderList = reader.GetString(10);
                    lvpr.ApproveUser = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    lvpr.CreateTime = reader.GetDateTime(12);
                    lvpr.leaveid = reader.GetString(13);
                    lvpr.rowid = reader.GetInt32(14);
                    lvpr.sanctionedFrom = reader.IsDBNull(15) ? DateTime.Now : reader.GetDateTime(15);
                    lvpr.sanctionedTo = reader.IsDBNull(16) ? DateTime.Now : reader.GetDateTime(16);
                    lvpr.sanctiontype = reader.GetInt32(17);
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


        public List<leaveapprove> getLeaveLimit(string empid)
        {
            leaveapprove lvapp;
            List<leaveapprove> LVlist = new List<leaveapprove>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.LeaveCount,a.LeaveID from ViewLeaveCount a where EmployeeID='" + empid + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvapp = new leaveapprove();
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




        public List<leaveapprove> getLeaveRemain(string empid, string leaveid)
        {
            leaveapprove lvapp;
            List<leaveapprove> LVlist = new List<leaveapprove>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select  DATEDIFF(DAY,a.SanctionedFromDate ,a.SanctionedToDate ) as totaldays  from LeaveRequest a, " +
                                "(select b.UserID  from ViewUserEmployeeList b where b.EmployeeID = '" + empid + "') c " +
                               " where a.UserID = c.UserID and a.LeaveID = '" + leaveid + "' and a.Status = 1 and a.DocumentStatus = 99 and a.LeaveRequestStatus in (1,11,10,9,7) and year(a.SanctionedFromDate)=year(GetDate()) ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvapp = new leaveapprove();
                    lvapp.leavepending = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
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

        public leaveapprove forwarderAuthority(string empid)
        {
            leaveapprove lvapp = new leaveapprove();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.UserID,e.Name from ERPUser a  ,Employee e where " +
                                "a.EmployeeID='"+ empid + "' and a.EmployeeID = e.EmployeeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvapp.ForwardUser = reader.GetString(0);
                    lvapp.username = reader.GetString(1);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lvapp;
        }

        public leaveapprove forwarder(string empid)
        {
            leaveapprove lvapp = new leaveapprove(); 
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.UserID,e.Name from ERPUser a,Employee e where "+
                                "a.EmployeeID in (select emp.ReportingOfficerID from ViewEmployeeDetails"+
                                " emp where emp.EmployeeID = '"+empid+"' ) and a.EmployeeID = e.EmployeeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvapp.ForwardUser = reader.GetString(0);
                    lvapp.username = reader.GetString(1);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lvapp;
        }

        public Boolean forwardleave(leaveapprove lv)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update LeaveRequest set DocumentStatus += 1 , forwardUser='" + lv.ForwardUser + "'" +
                     ", Remarks='" + lv.remarks + "'" +
                    ", ForwarderList='" + lv.ForwarderList + "'" +
                    " where RowID='" + lv.rowid + "'";
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

        public Boolean ApproveLeaveRequest(leaveapprove lv)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update LeaveRequest set  Remarks='" + lv.remarks + "', " +
                                   " SanctionedFromDate = '" + lv.sanctionedFrom.ToString("yyyy-MM-dd") + "', SanctionedToDate ='" + lv.sanctionedTo.ToString("yyyy-MM-dd") + "'," +
                                    "documentstatus=99, ApproveUser='" + Login.userLoggedIn + "', ApproveTime=GetDate() " +
                                   "where RowID='" + lv.rowid + "'";


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

        public Boolean ApproveLeaveRequestCOmpoff(leaveapprove lv,int tp)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                updateSQL = "update LeaveRequest set  Remarks='" + lv.remarks + "', " +
                   " SanctionedFromDate = '" + lv.sanctionedFrom.ToString("yyyy-MM-dd") + "', SanctionedToDate ='" + lv.sanctionedTo.ToString("yyyy-MM-dd") + "'," +
                    "documentstatus=99, ApproveUser='" + Login.userLoggedIn + "', ApproveTime=GetDate() " +
                   "where RowID='" + lv.rowid + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "LeaveRequest", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "update LeaveEarning set Status=2 where   RowID in" +
                                   " (select top "+tp+" rowid from LeaveEarning where " +
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

        public Boolean ApproveLeaveRequestModifiedCO(leaveapprove lv,double tp)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                updateSQL = "update LeaveRequest set  Remarks='" + lv.remarks + "', SanctionedToDate='" + lv.sanctionedTo.ToString("yyyy-MM-dd") + "', " +
                                   "documentstatus=99,LeaveRequestStatus=9 ,ApproveUser='" + Login.userLoggedIn + "', " +
                                  "ApproveTime=GetDate() where RowID='" + lv.rowid + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "LeaveRequest", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "update LeaveEarning set Status=2 where   RowID in" +
                                 " (select top "+tp+" rowid from LeaveEarning where " +
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

        public Boolean ApproveLeaveRequestModified(leaveapprove lv)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                updateSQL = "update LeaveRequest set  Remarks='" + lv.remarks + "', SanctionedToDate='" + lv.sanctionedTo.ToString("yyyy-MM-dd") + "', " +
                                   "documentstatus=99,LeaveRequestStatus=9 ,ApproveUser='" + Login.userLoggedIn + "', " +
                                  "ApproveTime=GetDate() where RowID='" + lv.rowid + "'";
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

        public Boolean ApproveLeaveRequestModify(leaveapprove lv)
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

        public Boolean CancelledLeaveRequestModified(leaveapprove lv)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update LeaveRequest set  Remarks='" + lv.remarks + "', " +
                                    "documentstatus=99,LeaveRequestStatus=10  " +
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

        public Boolean CancelledLeaveRequestModify(leaveapprove lv)
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

        public Boolean ApproveCancelRequest(leaveapprove lv)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update LeaveRequest set  Remarks='" + lv.remarks + "', " +
                                    "documentstatus=99,LeaveRequestStatus=5, Status=98 ,ApproveUser='" + Login.userLoggedIn + "' " +
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

        public Boolean ApproveCancelRequestCO(leaveapprove lv,int top)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                updateSQL = "update LeaveRequest set  Remarks='" + lv.remarks + "', " +
                                   "documentstatus=99,LeaveRequestStatus=5, Status=98 ,ApproveUser='" + Login.userLoggedIn + "' " +
                                  "where UserID in (select userid from ViewUserEmployeeList where EmployeeID = '" + lv.EmployeeID + "') and RowID='" + lv.rowid + "'";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "LeaveRequest", "", updateSQL) +
                Main.QueryDelimiter;
                updateSQL = "update LeaveEarning set Status=1 where   RowID in" +
                             "(select top "+top+" rowid from LeaveEarning where EmployeeID = '" + lv.EmployeeID + "'" +
                             " and LeaveID = 'CO' and Status = 2 order by date desc)";

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

        public int CheckCancel(string empid)
        {
            int val = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select count(*) from LeaveEarning where " +
                             "EmployeeID='" + empid + "' and Status=1 ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    val = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return val;
        }

        public Boolean CancelCancelRequest(leaveapprove lv)
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


        public Boolean reverseLeave(leaveapprove lv)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update LeaveRequest set DocumentStatus= 1, Remarks='" + lv.remarks + "'" +
                      ",LeaveRequestStatus=3  where  RowID='" + lv.rowid + "'";
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

        public Boolean RejectLeave(leaveapprove lv)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update LeaveRequest set " +
                    "Remarks='" + lv.remarks + "'" +
                      ", LeaveRequestStatus=99 " +
                      ", ApproveTime=GetDate() " +
                    " where  RowID='" + lv.rowid + "'";
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

        public Boolean ApproveCheck(leaveapprove lv, double totaldays)
        {
            Boolean status = false;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.MaxSanctionLimit   from LeaveSanctionLimit a,ViewEmployeeDetails b where b.EmployeeID='" + Login.empLoggedIn + "' " +
                                    "and a.Designation = b.Designation and a.LeaveID = '" + lv.leaveid + "'";
                
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader.GetInt32(0) >= totaldays)
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


        //changes for date
        public List<leaveapprove> getActionPending(DateTime frmdate, DateTime todate, int opt)
        {
            leaveapprove lvapp;
            List<leaveapprove> LVlist = new List<leaveapprove>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "";
                switch (opt)
                {
                    case 1:
                        query = "select b.EmployeeID,b.Name,a.LeaveID,CAST(  a.CreateTime as date)ApplicationDate,a.FromDate,a.ToDate, " +
                               "DATEDIFF(DAY, a.FromDate, a.ToDate) + 1 as noofdays, a.Status,a.DocumentStatus, a.LeaveRequestStatus, " +
                               "(select Name from ViewUserEmployeeList where UserID = a.ForwardUser) as Approver from LeaveRequest a, " +
                               "ViewUserEmployeeList b where a.UserID = b.UserID and " +
                               "((a.FromDate between '" + frmdate.ToString("yyyy-MM-dd") + "' and '" + todate.ToString("yyyy-MM-dd") + "') or " +
                               "(a.ToDate between '" + frmdate.ToString("yyyy-MM-dd") + "' and '" + todate.ToString("yyyy-MM-dd") + "')  or " +
                               "('" + frmdate.ToString("yyyy-MM-dd") + "' >= a.FromDate and '" + frmdate.ToString("yyyy-MM-dd") + "' <= a.ToDate) or " +
                               "('" + todate.ToString("yyyy-MM-dd") + "' >= a.FromDate and '" + todate.ToString("yyyy-MM-dd") + "' <= a.ToDate) ) and " +
                               "(a.DocumentStatus between 2 and 98) and a.LeaveRequestStatus <> 99";
                        break;
                    case 3:
                        query = "select b.EmployeeID,b.Name,a.LeaveID,CAST(  a.CreateTime as date)ApplicationDate,a.FromDate,a.ToDate," +
                            " DATEDIFF(DAY, a.FromDate, a.ToDate) + 1 as noofdays, a.Status,a.DocumentStatus, a.LeaveRequestStatus, " +
                            " (select Name from ViewUserEmployeeList where UserID = a.ForwardUser) as Approver from LeaveRequest a, " +
                            "ViewUserEmployeeList b where a.UserID = b.UserID and " +
                             "((a.FromDate between '" + frmdate.ToString("yyyy-MM-dd") + "' and '" + todate.ToString("yyyy-MM-dd") + "') or " +
                               "(a.ToDate between '" + frmdate.ToString("yyyy-MM-dd") + "' and '" + todate.ToString("yyyy-MM-dd") + "')  or " +
                               "('" + frmdate.ToString("yyyy-MM-dd") + "' >= a.FromDate and '" + frmdate.ToString("yyyy-MM-dd") + "' <= a.ToDate) or " +
                               "('" + todate.ToString("yyyy-MM-dd") + "' >= a.FromDate and '" + todate.ToString("yyyy-MM-dd") + "' <= a.ToDate) ) and " +
                            " a.Status=98";
                        break;
                    case 4:
                        query = "select b.EmployeeID,b.Name,a.LeaveID,CAST(  a.CreateTime as date)ApplicationDate,a.FromDate,a.ToDate," +
                            " DATEDIFF(DAY, a.FromDate, a.ToDate) + 1 as noofdays, a.Status,a.DocumentStatus, a.LeaveRequestStatus, " +
                            " (select Name from ViewUserEmployeeList where UserID = a.ForwardUser) as Approver from LeaveRequest a, " +
                            "ViewUserEmployeeList b where a.UserID = b.UserID and " +
                             "((a.FromDate between '" + frmdate.ToString("yyyy-MM-dd") + "' and '" + todate.ToString("yyyy-MM-dd") + "') or " +
                               "(a.ToDate between '" + frmdate.ToString("yyyy-MM-dd") + "' and '" + todate.ToString("yyyy-MM-dd") + "')  or " +
                               "('" + frmdate.ToString("yyyy-MM-dd") + "' >= a.FromDate and '" + frmdate.ToString("yyyy-MM-dd") + "' <= a.ToDate) or " +
                               "('" + todate.ToString("yyyy-MM-dd") + "' >= a.FromDate and '" + todate.ToString("yyyy-MM-dd") + "' <= a.ToDate) ) and " +
                            "a.LeaveRequestStatus=99";
                        break;
                    default:
                        break;
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvapp = new leaveapprove();
                    lvapp.EmployeeID = reader.GetString(0);
                    lvapp.EmployeeName = reader.GetString(1);
                    lvapp.leaveid = reader.GetString(2);
                    lvapp.CreateTime = reader.GetDateTime(3);
                    lvapp.fromdate = reader.GetDateTime(4);
                    lvapp.todate = reader.GetDateTime(5);
                    lvapp.noofdays = reader.GetInt32(6);
                    lvapp.status = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);
                    lvapp.documentStatus = reader.GetInt32(8);
                    lvapp.leavestatus = reader.GetInt32(9);
                    lvapp.ForwardUser = reader.GetString(10);
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

        public List<leaveapprove> getApprovedLeave(DateTime frmdate, DateTime todate)
        {
            leaveapprove lvapp;
            List<leaveapprove> LVlist = new List<leaveapprove>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select b.EmployeeID,b.Name,a.LeaveID,CAST(a.CreateTime as date)ApplicationDate, " +
                               "a.FromDate,a.ToDate,a.SanctionedFromDate, " +
                               "a.SanctionedToDate,DATEDIFF(DAY, a.SanctionedFromDate, a.SanctionedToDate) + 1 as noofdays, " +
                               "a.Status, a.DocumentStatus, a.LeaveRequestStatus,(select Name from ViewUserEmployeeList where UserID= a.ApproveUser) Approver " +
                               "from LeaveRequest a, ViewUserEmployeeList b " +
                               " where a.UserID = b.UserID and " +
                               "((a.SanctionedFromDate between '" + frmdate.ToString("yyyy-MM-dd") + "' and '" + todate.ToString("yyyy-MM-dd") + "') or " +
                                  "(a.SanctionedToDate between '" + frmdate.ToString("yyyy-MM-dd") + "' and '" + todate.ToString("yyyy-MM-dd") + "')  or " +
                                  "('" + frmdate.ToString("yyyy-MM-dd") + "' >= a.SanctionedFromDate and '" + frmdate.ToString("yyyy-MM-dd") + "' <= a.SanctionedToDate) or " +
                                  "('" + todate.ToString("yyyy-MM-dd") + "' >= a.SanctionedFromDate and '" + todate.ToString("yyyy-MM-dd") + "' <= a.SanctionedToDate) ) and " +
                               "a.Status = 1 and a.DocumentStatus = 99 ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvapp = new leaveapprove();
                    lvapp.EmployeeID = reader.GetString(0);
                    lvapp.EmployeeName = reader.GetString(1);
                    lvapp.leaveid = reader.GetString(2);
                    lvapp.CreateTime = reader.GetDateTime(3);
                    lvapp.fromdate = reader.GetDateTime(4);
                    lvapp.todate = reader.GetDateTime(5);
                    lvapp.sanctionedFrom = reader.GetDateTime(6);
                    lvapp.sanctionedTo = reader.GetDateTime(7);
                    lvapp.noofdays = reader.GetInt32(8);
                    lvapp.status = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                    lvapp.documentStatus = reader.GetInt32(10);
                    lvapp.leavestatus = reader.GetInt32(11);
                    lvapp.ApproveUser = reader.GetString(12);
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
    }
}
