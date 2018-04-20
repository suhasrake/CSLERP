using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class workplacecr
    {
        public int rowID { get; set; }
        public String EmployeeID { get; set; }
        public String EmployeeName { get; set; }
        public string ComplaintType { get; set; }
        public string ComplaintDescription { get; set; }
        public DateTime AcceptTime { get; set; }
        public DateTime CloseTime { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public int DocumentStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string Creator { get; set; }     
    }

    class WorkplaceCRDB
    {
        public List<workplacecr> getFilteredComplaintsForUser(int opt)
        {
            workplacecr wpcr;
            List<workplacecr> Complist = new List<workplacecr>();
            try
            {
                string query1 = "select a.ID, a.EmployeeID,b.Name, a.ComplaintType, a.ComplaintDescription,a.Remarks,a.AcceptTime, " +
                    " a.CloseTime, a.Status, a.DocumentStatus, a.CreateTime, a.CreateUser,c.Name" +
                    " from WorkplaceCR a, ViewUserEmployeeList b, ViewUserEmployeeList c" +
                   " where a.EmployeeID=b.EmployeeID and a.CreateUser = c.UserID and a.createuser='" + Login.userLoggedIn +
                   "' and a.Status = 1 and a.DocumentStatus in (1,3) order by CreateTime desc";
                string query2 = "select a.ID, a.EmployeeID,b.Name, a.ComplaintType, a.ComplaintDescription,a.Remarks,a.AcceptTime, " +
                    " a.CloseTime, a.Status, a.DocumentStatus, a.CreateTime, a.CreateUser,c.Name" +
                    " from WorkplaceCR a, ViewUserEmployeeList b, ViewUserEmployeeList c" +
                   " where a.EmployeeID=b.EmployeeID and a.CreateUser = c.UserID and a.createuser='" + Login.userLoggedIn +
                   "' and a.Status = 2 and a.DocumentStatus in (1,3) order by CreateTime desc";
                string query3 = "select a.ID, a.EmployeeID,b.Name, a.ComplaintType, a.ComplaintDescription,a.Remarks,a.AcceptTime, " +
                    " a.CloseTime, a.Status, a.DocumentStatus, a.CreateTime, a.CreateUser,c.Name" +
                    " from WorkplaceCR a, ViewUserEmployeeList b, ViewUserEmployeeList c" +
                   " where a.EmployeeID=b.EmployeeID and a.CreateUser = c.UserID and a.createuser='" + Login.userLoggedIn +
                  // "' and a.Status not in (1,2) and a.DocumentStatus not in(1,3) order by CreateTime desc" +
                  "' and a.Status <> 4 order by CreateTime desc";

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
                    default:
                        query = "";
                        break;
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    wpcr = new workplacecr();
                    wpcr.rowID = reader.GetInt32(0);
                    wpcr.EmployeeID = reader.GetString(1);
                    wpcr.EmployeeName = reader.GetString(2);
                    wpcr.ComplaintType = reader.GetString(3);
                    wpcr.ComplaintDescription = reader.GetString(4);
                    wpcr.Remarks = reader.GetString(5);
                    wpcr.AcceptTime = reader.IsDBNull(6) ? DateTime.MinValue : reader.GetDateTime(6);
                    wpcr.CloseTime = reader.IsDBNull(7) ? DateTime.MinValue : reader.GetDateTime(7);
                    wpcr.Status = reader.GetInt32(8);
                    wpcr.DocumentStatus = reader.GetInt32(9);
                    wpcr.CreateTime = reader.GetDateTime(10);
                    wpcr.CreateUser = reader.GetString(11);
                    wpcr.Creator = reader.GetString(12);
                    Complist.Add(wpcr);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Complaint  Data");
            }
            return Complist;

        }
        public List<workplacecr> getFilteredComplaintsForMO(int opt)
        {
            workplacecr wpcr;
            List<workplacecr> Complist = new List<workplacecr>();
            try
            {
                string query1 = "select a.ID, a.EmployeeID,b.Name, a.ComplaintType, a.ComplaintDescription,a.Remarks,a.AcceptTime, " +
                    " a.CloseTime, a.Status, a.DocumentStatus, a.CreateTime, a.CreateUser,c.Name" +
                    " from WorkplaceCR a, ViewUserEmployeeList b, ViewUserEmployeeList c" +
                   " where a.EmployeeID=b.EmployeeID and a.CreateUser = c.UserID and "  +
                   " a.Status = 1 and a.DocumentStatus in (1,3) order by CreateTime desc";
                string query2 = "select a.ID, a.EmployeeID,b.Name, a.ComplaintType, a.ComplaintDescription,a.Remarks,a.AcceptTime, " +
                    " a.CloseTime, a.Status, a.DocumentStatus, a.CreateTime, a.CreateUser,c.Name" +
                    " from WorkplaceCR a, ViewUserEmployeeList b, ViewUserEmployeeList c" +
                   " where a.EmployeeID=b.EmployeeID and a.CreateUser = c.UserID and " +
                   " a.Status = 2 and a.DocumentStatus in (1,3) order by CreateTime desc";
                string query3 = "select a.ID, a.EmployeeID,b.Name, a.ComplaintType, a.ComplaintDescription,a.Remarks,a.AcceptTime, " +
                    " a.CloseTime, a.Status, a.DocumentStatus, a.CreateTime, a.CreateUser,c.Name" +
                    " from WorkplaceCR a, ViewUserEmployeeList b, ViewUserEmployeeList c" +
                   " where a.EmployeeID=b.EmployeeID and a.CreateUser = c.UserID " +
                   " and a.Status <> 4 order by CreateTime desc";
                  // "' and a.Status not in (1,2) and a.DocumentStatus not in(1,3) order by CreateTime desc";

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
                    default:
                        query = "";
                        break;
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    wpcr = new workplacecr();
                    wpcr.rowID = reader.GetInt32(0);
                    wpcr.EmployeeID = reader.GetString(1);
                    wpcr.EmployeeName = reader.GetString(2);
                    wpcr.ComplaintType = reader.GetString(3);
                    wpcr.ComplaintDescription = reader.GetString(4);
                    wpcr.Remarks = reader.GetString(5);
                    wpcr.AcceptTime = reader.IsDBNull(6) ? DateTime.MinValue : reader.GetDateTime(6);
                    wpcr.CloseTime = reader.IsDBNull(7) ? DateTime.MinValue : reader.GetDateTime(7);
                    wpcr.Status = reader.GetInt32(8);
                    wpcr.DocumentStatus = reader.GetInt32(9);
                    wpcr.CreateTime = reader.GetDateTime(10);
                    wpcr.CreateUser = reader.GetString(11);
                    wpcr.Creator = reader.GetString(12);
                    Complist.Add(wpcr);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Complaint  Data");
            }
            return Complist;

        }
        public Boolean updateComplaintReg(workplacecr wp, workplacecr prevwp)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WorkplaceCR set "+ 
                    //"ExitTimePlanned ='" + mr.ExitTimePlanned.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                    // ", ReturnTimePlanned ='" + mr.ReturnTimePlanned.ToString("yyyy-MM-dd HH:mm:ss") +
                     "ComplaintType ='" + wp.ComplaintType +
                       "', ComplaintDescription ='" + wp.ComplaintDescription +
                       "', Remarks ='" + wp.Remarks +
                     "' where EmployeeID='" + prevwp.EmployeeID + "' and ID = " + prevwp.rowID ;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WorkplaceCR", "", updateSQL) +
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
        public Boolean insertComplaintReg(workplacecr wp)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into WorkplaceCR " +
                    " (EmployeeID,ComplaintType,ComplaintDescription,Remarks," +
                    "Status,DocumentStatus,CreateUser, CreateTime)" +
                    "values (" +
                    "'" + wp.EmployeeID + "'," +
                   // "'" + mr.ExitTimePlanned.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                   //"'" + mr.ReturnTimePlanned.ToString("yyyy-MM-dd HH:mm:ss") + "'," +
                    "'" + wp.ComplaintType + "'," +
                    "'" + wp.ComplaintDescription + "',"+
                     "'" + wp.Remarks + "'," +
                    wp.Status + "," +
                     wp.DocumentStatus + "," +
                    "'" + Login.userLoggedIn +"',"+
                    "GETDATE())";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "WorkplaceCR", "", updateSQL) +
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
        public Boolean validateComplaintReg(workplacecr wp)
        {
            Boolean status = true;
            try
            {
                if (wp.EmployeeID.Trim().Length == 0 || wp.EmployeeID == null)
                {
                    return false;
                }
                ////if (wp.Remarks.Trim().Length == 0 || wp.Remarks == null)
                ////{
                ////    return false;
                ////}
                if (wp.ComplaintType.Trim().Length == 0 || wp.ComplaintType == null)
                {
                    return false;
                }
                if (wp.ComplaintDescription.Trim().Length == 0 || wp.ComplaintDescription == null)
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
        public Boolean AcceptComplaint(workplacecr wp)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WorkplaceCR set Status=2 ,Remarks = '" + wp.Remarks+"'"+
                    ", AcceptTime = GETDATE() " + 
                   // ", Approver='" + Login.userLoggedIn + "'" +
                    " where employeeID='" + wp.EmployeeID  + "' and ID = " + wp.rowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "WorkplaceCR", "", updateSQL) +
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

        public Boolean CancelComplaintByUser(workplacecr wp)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WorkplaceCR set   Status = 4,Remarks = '" + wp.Remarks + "'" +
                     " where EmployeeID='" + wp.EmployeeID + "' and ID = " + wp.rowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WorkplaceCR", "", updateSQL) +
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

        public Boolean RejectComplaintRegByMO(workplacecr wp)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WorkplaceCR set   Status = 3,CloseTime = GETDATE(),DocumentStatus = 2,Remarks = '" + wp.Remarks + "'" +
                     " where EmployeeID='" + wp.EmployeeID + "' and ID = " + wp.rowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WorkplaceCR", "", updateSQL) +
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

        public Boolean RejectComplaintRegByUser(workplacecr wp)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WorkplaceCR set   Status = 1,DocumentStatus = 3,Remarks = '" + wp.Remarks + "'" +
                     ",AcceptTime ='" + wp.AcceptTime.ToString("yyyy-MM-dd HH:mm:ss") + "'" +
                      ", CloseTime ='" + wp.CloseTime.ToString("yyyy-MM-dd HH:mm:ss") +
                     "' where EmployeeID='" + wp.EmployeeID + "' and ID = " + wp.rowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WorkplaceCR", "", updateSQL) +
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
        public static Boolean IsAllowToRejectClosedComplaint(workplacecr wp) //within 2 days of rejection or completion
        {
            Boolean stat = true;
            int n = 0;
            DateTime closeTime = DateTime.MinValue;
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select CloseTime" +
                     " from WorkplaceCR " +
                 " where EmployeeID='" + wp.EmployeeID + "' and ID = " + wp.rowID;

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    closeTime = reader.GetDateTime(0);
                    n = 1;
                }
                conn.Close();
                if (n == 1)
                {
                    if ((UpdateTable.getSQLDateTime().Date - closeTime.Date).TotalDays > 2)
                    {
                        stat = false;
                    }
                    else
                    {
                        stat = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Complaint Details For Accept Status");
            }
            return stat;
        }
        public Boolean CloseComplaintByMO(workplacecr wp)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WorkplaceCR set Status=5 ,Remarks = '" + wp.Remarks + "'" +
                    ",DocumentStatus = 2 , CloseTime = GETDATE() " +
                    // ", Approver='" + Login.userLoggedIn + "'" +
                    " where employeeID='" + wp.EmployeeID + "' and ID = " + wp.rowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "WorkplaceCR", "", updateSQL) +
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
        public Boolean CloseComplaintByMM(workplacecr wp)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WorkplaceCR set Status=6 ,Remarks = '" + wp.Remarks+ "'" +
                    ", CloseTime = GETDATE() " +
                    " where employeeID='" + wp.EmployeeID + "' and ID = " + wp.rowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "WorkplaceCR", "", updateSQL) +
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

        public static String getEmployeeRoleForMOOrMM() //Employee Loggedin is MO or MM or none
        {
            string roles = "";
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select EmployeeRoles " +
                  "from EmployeeRoleMapping  where EmployeeID = '"+Login.empLoggedIn+"'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    roles = reader.GetString(0);
                }
                conn.Close();
                if (roles.Contains("MaintenanceOfficer"))
                {
                    roles = "MO";
                }
                else if (roles.Contains("MaintenanceManager"))
                {
                    roles = "MM";
                }
                else
                    roles = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Employee Role detail For Complaint");
            }
            return roles;
        }
        public List<workplacecr> getFilteredComplaintsForReport(int opt,DateTime from, DateTime to, string CType)
        {
            workplacecr wpcr;
            List<workplacecr> Complist = new List<workplacecr>();
            try
            {
                string query1 = "select a.EmployeeID,b.Name, a.ComplaintType, a.ComplaintDescription,a.Remarks,a.AcceptTime, " +
                    " a.CloseTime, a.Status, a.DocumentStatus, a.CreateTime" +
                    " from WorkplaceCR a, ViewUserEmployeeList b" +
                   " where a.EmployeeID=b.EmployeeID and"+
                   " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") +
                   "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") +
                   "' and a.ComplaintType = '" +CType+"' order by a.CreateTime desc";
                string query2 = "select a.EmployeeID,b.Name, a.ComplaintType, a.ComplaintDescription,a.Remarks,a.AcceptTime, " +
                    " a.CloseTime, a.Status, a.DocumentStatus, a.CreateTime" +
                    " from WorkplaceCR a, ViewUserEmployeeList b" +
                   " where a.EmployeeID=b.EmployeeID and" +
                   " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") +
                   "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") +
                   "' order by a.CreateTime desc";
                string query = "";
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
                    wpcr = new workplacecr();
                    wpcr.EmployeeID = reader.GetString(0);
                    wpcr.EmployeeName = reader.GetString(1);
                    wpcr.ComplaintType = reader.GetString(2);
                    wpcr.ComplaintDescription = reader.GetString(3);
                    wpcr.Remarks = reader.GetString(4);
                    wpcr.AcceptTime = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(5);
                    wpcr.CloseTime = reader.IsDBNull(6) ? DateTime.MinValue : reader.GetDateTime(6);
                    wpcr.Status = reader.GetInt32(7);
                    wpcr.DocumentStatus = reader.GetInt32(8);
                    wpcr.CreateTime = reader.GetDateTime(9);
                    Complist.Add(wpcr);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Complaint  Data");
            }
            return Complist;
        }
    }
}
