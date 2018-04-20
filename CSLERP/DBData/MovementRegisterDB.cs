using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class movementregister
    {
        public int rowID { get; set; }
        public String EmployeeID { get; set; }
        public String EmployeeName { get; set; }
        public DateTime ReturnTimePlanned { get; set; }
        public DateTime ExitTimePlanned { get; set; }
        public string Purpose { get; set; }
        public string ModeOfTravel { get; set; }
        public DateTime ApproveTime { get; set; }
        public DateTime OutTime { get; set; }
        public DateTime InTime { get; set; }
        public string Comments { get; set; }
        public int Status { get; set; }
        public int DocumentStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string Creator { get; set; }
        public string ApproveUser { get; set; }
        public string Approver { get; set; }
      
    }

    class MovementRegisterDB
    {
        public List<movementregister> getFilteredmovementregister(int opt)
        {
            movementregister mr;
            List<movementregister> MRlist = new List<movementregister>();
            try
            {
                string query1 = "select a.RowID, a.EmployeeID,b.Name, a.ExitTimePlanned, a.ReturnTimePlanned,a.Purpose, " +
                    " a.ModeOfTravel, a.OutTime, a.InTime, a.Comments, a.Status, a.DocumentStatus,a.Approver, c.Name, a.CreateTime," +
                    " a.ApproveTime, a.CreateUser,d.Name" +
                    " from MovementRegister a, ViewUserEmployeeList b, ViewUserEmployeeList c,ViewUserEmployeeList d" +
                   " where a.EmployeeID=b.EmployeeID and a.Approver = c.UserID and a.CreateUser = d.UserID and (a.createuser='" + Login.userLoggedIn +
                   "' and a.Status = 1 and a.DocumentStatus =1) order by CreateTime desc";
                string query2 = "select a.RowID, a.EmployeeID,b.Name, a.ExitTimePlanned, a.ReturnTimePlanned,a.Purpose, " +
                    " a.ModeOfTravel, a.OutTime, a.InTime, a.Comments, a.Status, a.DocumentStatus,a.Approver, c.Name, a.CreateTime," +
                    " a.ApproveTime, a.CreateUser,d.Name" +
                    " from MovementRegister a, ViewUserEmployeeList b, ViewUserEmployeeList c,ViewUserEmployeeList d" +
                   " where a.EmployeeID=b.EmployeeID and a.Approver = c.UserID and a.CreateUser = d.UserID and (a.createuser='" + Login.userLoggedIn +
                   "' and a.DocumentStatus in (2,3,99,98,10,5,6)) order by CreateTime desc";
                string query3 = "select a.RowID, a.EmployeeID,b.Name, a.ExitTimePlanned, a.ReturnTimePlanned,a.Purpose, " +
                    " a.ModeOfTravel, a.OutTime, a.InTime, a.Comments, a.Status, a.DocumentStatus,a.Approver, c.Name, a.CreateTime," +
                    " a.ApproveTime, a.CreateUser,d.Name" +
                    " from MovementRegister a, ViewUserEmployeeList b, ViewUserEmployeeList c,ViewUserEmployeeList d" +
                   " where a.EmployeeID=b.EmployeeID and a.Approver = c.UserID and a.CreateUser = d.UserID and (a.Approver='" + Login.userLoggedIn +
                   "' and a.Status = 1 and a.DocumentStatus in (1,5) and a.Status <> 98) order by CreateTime desc";
                string query4 = "select a.RowID, a.EmployeeID,b.Name, a.ExitTimePlanned, a.ReturnTimePlanned,a.Purpose, " +
                   " a.ModeOfTravel, a.OutTime, a.InTime, a.Comments, a.Status, a.DocumentStatus,a.Approver, c.Name, a.CreateTime," +
                   " a.ApproveTime, a.CreateUser,d.Name" +
                   " from MovementRegister a, ViewUserEmployeeList b, ViewUserEmployeeList c,ViewUserEmployeeList d" +
                  " where a.EmployeeID=b.EmployeeID and a.Approver = c.UserID and a.CreateUser = d.UserID and a.Approver='" + Login.userLoggedIn +
                  "' and a.Status = 1 and a.DocumentStatus in (99,2,3,10,4) order by CreateTime desc";
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
                    default:
                        query = "";
                        break;
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mr = new movementregister();
                    mr.rowID = reader.GetInt32(0);
                    mr.EmployeeID = reader.GetString(1);
                    mr.EmployeeName = reader.GetString(2);
                    mr.ExitTimePlanned = reader.GetDateTime(3);
                    mr.ReturnTimePlanned = reader.GetDateTime(4);
                    mr.Purpose = reader.GetString(5);
                    mr.ModeOfTravel = reader.GetString(6);
                    mr.OutTime = reader.IsDBNull(7) ? DateTime.MinValue : reader.GetDateTime(7);
                    mr.InTime = reader.IsDBNull(8) ? DateTime.MinValue : reader.GetDateTime(8);
                    mr.Comments = reader.IsDBNull(9) ? "" : reader.GetString(9);
                    mr.Status = reader.GetInt32(10);
                    mr.DocumentStatus = reader.GetInt32(11);
                    mr.ApproveUser = reader.GetString(12);
                    mr.Approver = reader.GetString(13);
                    mr.CreateTime = reader.GetDateTime(14);
                    mr.ApproveTime = reader.IsDBNull(15) ? DateTime.MinValue : reader.GetDateTime(15);
                    mr.CreateUser = reader.GetString(16);
                    mr.Creator = reader.GetString(17);
                    MRlist.Add(mr);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying MovementReg  Data");
            }
            return MRlist;

        }
        public Boolean updateMovementReg(movementregister mr, movementregister prevmr)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update MovementRegister set   ExitTimePlanned ='" + mr.ExitTimePlanned.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                     ", ReturnTimePlanned ='" + mr.ReturnTimePlanned.ToString("yyyy-MM-dd HH:mm:ss") +
                     "', ModeOfTravel ='" + mr.ModeOfTravel +
                     "', Comments ='" +
                     mr.Comments.Replace("'", "''") + 
                     "', Approver='" + mr.ApproveUser +
                     "', Purpose='" + mr.Purpose +
                     "' where EmployeeID='" + prevmr.EmployeeID + "' and RowID = " + prevmr.rowID ;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "MovementRegister", "", updateSQL) +
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
        public Boolean insertMovementReg(movementregister mr)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into MovementRegister " +
                    " (EmployeeID,ExitTimePlanned,ReturnTimePlanned,Purpose,ModeOfTravel,Comments,"+
                    "Approver,Status,DocumentStatus,CreateUser, CreateTime)" +
                    "values (" +
                    "'" + mr.EmployeeID + "'," +
                    "'" + mr.ExitTimePlanned.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                   "'" + mr.ReturnTimePlanned.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    "'" + mr.Purpose + "'," +
                    "'" + mr.ModeOfTravel + "',"+
                     "'" + mr.Comments.Replace("'","''") + "'," +
                    "'" + mr.ApproveUser + "'," +
                    mr.Status + "," +
                     mr.DocumentStatus + "," +
                    "'" + Login.userLoggedIn +"',"+
                    "GETDATE())";
                //"'" + pheader.ForwardUser + "'," +
                //"'" + pheader.ApproveUser + "'," +
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "MovementRegister", "", updateSQL) +
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
        public Boolean validateMovementReg(movementregister mr)
        {
            Boolean status = true;
            try
            {
                if (mr.EmployeeID.Trim().Length == 0 || mr.EmployeeID == null)
                {
                    return false;
                }

                if (mr.ExitTimePlanned == null)
                {
                    return false;
                }
                if (mr.ReturnTimePlanned == null)
                {
                    return false;
                }
                //DateTime a = UpdateTable.getSQLDateTime();
                //DateTime dt = mr.ExitTimePlanned;
                //int secs = (mr.ExitTimePlanned - UpdateTable.getSQLDateTime()).Seconds;
                //double sec = mr.ExitTimePlanned.Subtract(UpdateTable.getSQLDateTime()).TotalSeconds;
                DateTime tdt = UpdateTable.getSQLDateTime();
                Double val1 = mr.ExitTimePlanned.Subtract(tdt).TotalSeconds;
                Double val2 = mr.ReturnTimePlanned.Subtract(mr.ExitTimePlanned).TotalSeconds;
                if ((val1 < -300 ) || (val2 < 600) )
                {
                    return false;
                }
                if (mr.Purpose.Trim().Length == 0 || mr.Purpose == null)
                {
                    return false;
                }
                if (mr.ModeOfTravel.Trim().Length == 0 || mr.ModeOfTravel == null)
                {
                    return false;
                }
                if (mr.ApproveUser.Trim().Length == 0 || mr.ApproveUser == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Validation Failed.");
                return false;
            }
            
            return status;
        }
        public static Boolean checkMovementStatus(movementregister mr)
        {
            Boolean stat = true;
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID, EmployeeID, ExitTimePlanned, ReturnTimePlanned,Purpose " +
                     " from MovementRegister " +
                    " where createuser='" + Login.userLoggedIn +
                    "' and Status = 1 and DocumentStatus in (1,2,3,5)";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    stat = false;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Movement Details For Status");
            }
            return stat;
        }
        public Boolean ApproveMR(movementregister mr)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update MovementRegister set DocumentStatus=2, Comments='" + mr.Comments + "'" +
                    ", ApproveTime = GETDATE() " + 
                    ", Approver='" + Login.userLoggedIn + "'" +
                    " where employeeID='" + mr.EmployeeID  + "' and RowID = " + mr.rowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "MovementRegister", "", updateSQL) +
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

        public Boolean CancelMovementReg(movementregister prevmr, int opt)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                string updateSQL1 = "update MovementRegister set   Status = 98,Comments = '" + prevmr.Comments + "'" +
                     " where EmployeeID='" + prevmr.EmployeeID + "' and RowID = " + prevmr.rowID;
                string updateSQL2 = "update MovementRegister set   DocumentStatus = 98,"+
                    "Comments = '" + prevmr.Comments + "'" +
                     " where EmployeeID='" + prevmr.EmployeeID + "' and RowID = " + prevmr.rowID;
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
                ActivityLogDB.PrepareActivityLogQquerString("update", "MovementRegister", "", updateSQL) +
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

        public Boolean RejectMovementReg(movementregister prevmr)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update MovementRegister set   DocumentStatus = 10, Comments = '" + prevmr.Comments + "'" +
                     " where EmployeeID='" + prevmr.EmployeeID + "' and RowID = " + prevmr.rowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "MovementRegister", "", updateSQL) +
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
        public static string valuetostring(int Docstat,int stat)
        {
            string status = "";
            if (Docstat == 1 && stat==1)
            {
                status = "Created";
            }
            else if (Docstat == 1 && stat==98)
            {
                status = "User Cancelled";
            }
            else if (Docstat == 2)
            {
                status = "Approved";
            }
            else if (Docstat == 3)
            {
                status = "Out";
            }
            else if (Docstat == 10)
            {
                status = "Rejected";
            }
            else if (Docstat == 99)
            {
                status = "In";
            }
            else if (Docstat == 98)
            {
                status = "Approver Cancelled";
            }
            else if (Docstat == 4)
            {
                status = "Auto-In";
            }
            else if (Docstat == 5)
            {
                status = "Request for Cancel";
            }
            else if (Docstat == 6)
            {
                status = "Cancel Request Approved";
            }
            return status;
        }
        public static int stringtovalue(string status)
        {
            int stat = 0;
            if (status.Equals("Created"))
            {
                stat = 1;
            }
            else if (status.Equals("Approved"))
            {
                stat = 2;
            }
            else if (status.Equals("Out"))
            {
                stat = 3;
            }
            else if (status.Equals("Rejected"))
            {
                stat = 10;
            }
            else if (status.Equals("In"))
            {
                stat = 99;
            }
            else if (status.Equals("Approver Cancelled"))
            {
                stat = 98;
            }
            else if(status.Equals("Request for Cancel"))
            {
                stat = 5;
            }
            else if (status.Equals("Cancel Request Approved"))
            {
                stat = 6;
            }
            return stat;
        }

        //Movement Register List For DashBoard
        public List<movementregister> getMovementRegForDashboard()
        {
            movementregister mr;
            List<movementregister> MRlist = new List<movementregister>();
            try
            {
                string query = "select a.RowID, a.EmployeeID,b.Name, a.ExitTimePlanned, a.ReturnTimePlanned,a.Purpose, " +
                    " a.Status, a.DocumentStatus,a.Approver, c.Name, a.CreateTime" +
                    " from MovementRegister a, ViewUserEmployeeList b, ViewUserEmployeeList c" +
                   " where a.EmployeeID=b.EmployeeID and a.Approver = c.UserID"+
                   " and ((a.createuser='" + Login.userLoggedIn + "' and a.DocumentStatus = 2 and a.Status = 1)"+
                   " or (a.Approver ='" + Login.userLoggedIn + "' and a.DocumentStatus = 1 and a.Status = 1)) order by a.CreateTime desc";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mr = new movementregister();
                    mr.rowID = reader.GetInt32(0);
                    mr.EmployeeID = reader.GetString(1);
                    mr.EmployeeName = reader.GetString(2);
                    mr.ExitTimePlanned = reader.GetDateTime(3);
                    mr.ReturnTimePlanned = reader.GetDateTime(4);
                    mr.Purpose = reader.GetString(5);
                    mr.Status = reader.GetInt32(6);
                    mr.DocumentStatus = reader.GetInt32(7);
                    mr.ApproveUser = reader.GetString(8);
                    mr.Approver = reader.GetString(9);
                    mr.CreateTime = reader.GetDateTime(10);
                    MRlist.Add(mr);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying MovemetnReg  Data");
            }
            return MRlist;

        }

        ////for movement  cancellation

        public Boolean CancelReqforApprovedMR(movementregister mr)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update MovementRegister set DocumentStatus=5, Comments='" + mr.Comments + "'" +
                    " where employeeID='" + mr.EmployeeID + "' and RowID = " + mr.rowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("Cancel", "MovementRegister", "", updateSQL) +
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

        public Boolean ApproveReqforCancelledMR(movementregister mr)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update MovementRegister set DocumentStatus=6, Status=98, Comments='"+mr.Comments+"'"+
                    " where employeeID='" + mr.EmployeeID + "' and RowID = " + mr.rowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("Accept", "MovementRegister", "", updateSQL) +
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

        public Boolean RejReqforCancelledMR(movementregister mr)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update MovementRegister set DocumentStatus=2, Comments='" + mr.Comments + "'" +
                    " where employeeID='" + mr.EmployeeID + "' and RowID = " + mr.rowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("Cancel", "MovementRegister", "", updateSQL) +
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
