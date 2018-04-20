using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    //public strin "Data Source = PRAKASHPC; Initial Catalog = newERP; Integrated Security = True"
    class leaveob
    {
        public int RowID { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveID { get; set; }
        public string LeaveDescription { get; set; }
        public int maxdays { get; set; }
        public string officeid { get; set; }
        public int year { get; set; }
        public int LeaveCount { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }

    class LeaveOBDB
    {
        public List<leaveob> getLeaveOBDetails(string empID, int year)
        {
            leaveob lob;
            List<leaveob> lobList = new List<leaveob>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.RowID,a.LeaveID,b.Description,a.Year,a.LeaveCount,a.CreateUser,a.CreateTime" +
                    " from LeaveOB a , LeaveType b where a.LeaveID = b.LeaveID and a.EmployeeID = '" + empID + "' and a.Year = " + year;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lob = new leaveob();
                    lob.RowID = reader.GetInt32(0);
                    lob.LeaveID = reader.GetString(1);
                    lob.LeaveDescription = reader.GetString(2);
                    lob.year = reader.GetInt32(3);
                    lob.LeaveCount = reader.GetInt32(4);
                    lob.CreateUser = reader.GetString(5);
                    lob.CreateTime = reader.GetDateTime(6);
                    lobList.Add(lob);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying LEAVE OB Data");

            }
            return lobList;

        }
        public Boolean insertLeaveOBDetail(List<leaveob> lobList, string empID, int year)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                updateSQL = "Delete from LeaveOB where EmployeeID='" + empID + "'" +
                     " and Year=" + year;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "LeaveOB", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (leaveob lob in lobList)
                {
                    if (lob.LeaveCount > 0 || lob.LeaveID == "LOP")
                    {
                        updateSQL = "insert into LeaveOB (EmployeeID,LeaveID,Year,LeaveCount,CreateTime,CreateUser)" +
                        "values (" +
                        "'" + empID + "'," +
                        "'" + lob.LeaveID + "'," +
                        +year + "," +
                        "'" + lob.LeaveCount + "'," +
                        "GETDATE()" + "," +
                        "'" + Login.userLoggedIn + "'" + ")";
                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                         ActivityLogDB.PrepareActivityLogQquerString("insert", "LeaveOB", "", updateSQL) +
                         Main.QueryDelimiter;
                    }

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
        public static string getGenderForLeaveType(string leaveID)
        {
            string str = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select Gender from LeaveType where LeaveID = '" + leaveID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    str = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return str;
        }
        public static string getGenderForEmployee(string EmpID)
        {
            string str = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select Gender from Employee where EmployeeID = '" + EmpID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    str = reader.IsDBNull(0) ? "" : reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return str;
        }
        public static List<employeeposting> getEmployeePostingListForLeaveOB()
        {
            employeeposting emppostingrec;
            List<employeeposting> EmployeePosting = new List<employeeposting>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "Select distinct cast(a.EmployeeID as integer), a.Name, a.OfficeID, a.OfficeName from ViewEmployeeDetails a" +
                    " where a.status = 1  order by cast(a.EmployeeID as integer)";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    emppostingrec = new employeeposting();
                    emppostingrec.empID = reader.GetInt32(0);
                    emppostingrec.empName = reader.GetString(1);
                    emppostingrec.officeID = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    emppostingrec.officeName = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    EmployeePosting.Add(emppostingrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");

            }
            return EmployeePosting;
        }

        ///
        public static List<leaveob> getLeaveLimit()
        {
            leaveob lvapp;
            List<leaveob> LVlist = new List<leaveob>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.EmployeeID,a.LeaveCount,a.LeaveID from ViewLeaveCount a";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvapp = new leaveob();
                    lvapp.EmployeeID = reader.GetString(0);
                    lvapp.maxdays = reader.GetInt32(1);
                    lvapp.LeaveID = reader.GetString(2);
                    LVlist.Add(lvapp);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                
            }
            return LVlist;
        }

     

        public  List<leaveob> getOfficeMax(string officeid)
        {
            leaveob lvapp;
            List<leaveob> LVlist = new List<leaveob>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select LeaveID, OfficeID, MaxDays from LeaveOfficeMapping where OfficeID='"+officeid+"'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvapp = new leaveob();
                    lvapp.maxdays = reader.GetInt32(2);
                    lvapp.officeid = reader.GetString(1);
                    lvapp.LeaveID = reader.GetString(0);
                    LVlist.Add(lvapp);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                
            }
            return LVlist;
        }
        //select LeaveID, OfficeID, MaxDays from LeaveOfficeMapping


        public List<leaveapprove> getLeaveRemainYearWise(string empid, string leaveid,int year)
        {
            leaveapprove lvapp;
            List<leaveapprove> LVlist = new List<leaveapprove>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select  DATEDIFF(DAY,a.SanctionedFromDate ,a.SanctionedToDate ) as totaldays  from LeaveRequest a, " +
                                "(select b.UserID  from ViewUserEmployeeList b where b.EmployeeID = '" + empid + "') c " +
                               " where a.UserID = c.UserID and a.LeaveID = '" + leaveid + "' and a.Status = 1 and a.DocumentStatus = 99 and a.LeaveRequestStatus in (1,11,10,9,7) and year(a.SanctionedFromDate) <'"+year+"' ";
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



        public static Boolean checkfryear(int year)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string query = "select distinct LeaveID,LeaveCount from leaveob where Year='"+year+"'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    status = false;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }


        public static List<string>  getyear()
        {
            List<string> val = new List<string>();
            try
            {                
                string query = "select distinct year from LeaveOB ";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int a = reader.GetInt32(0);
                    val.Add(a.ToString());
                }
                conn.Close();               
            }
            catch (Exception ex)
            {
                MessageBox.Show("getyear():error query execution");
            }
            return val;
        }




        public static Boolean Delfryear(int year)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "delete from LeaveOB where Year='" + year + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "LeaveOB", "", updateSQL) +
                    Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }


        public Boolean insertLeaveyearOBDetail(List<leaveob> lobList, int year)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
               
                foreach (leaveob lob in lobList)
                    {


                    if (lob.LeaveCount > 0 || lob.LeaveID == "LOP")
                        {
                            updateSQL = "insert into LeaveOB (EmployeeID,LeaveID,Year,LeaveCount,CreateTime,CreateUser)" +
                            "values (" +
                            "'" + lob.EmployeeID + "'," +
                            "'" + lob.LeaveID + "','" +
                            +year + "'," +
                            "'" + lob.LeaveCount + "'," +
                            "GETDATE()" + "," +
                            "'" + Login.userLoggedIn + "'" + ")";
                            utString = utString + updateSQL + Main.QueryDelimiter;
                            utString = utString +
                             ActivityLogDB.PrepareActivityLogQquerString("insert", "LeaveOB", "", updateSQL) +
                             Main.QueryDelimiter;
                        }

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


        public List<leaveob> getLeaveOBDetailsnew(string empID, int year)
        {
            leaveob lob;
            List<leaveob> lobList = new List<leaveob>();
            try
            {

                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.RowID,a.LeaveID,b.Description,a.Year,a.LeaveCount,a.CreateUser,a.CreateTime" +
                    " from LeaveOB a , LeaveType b where a.LeaveID = b.LeaveID and a.EmployeeID = '" + empID + "' and a.Year <= '" + year+"'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lob = new leaveob();
                    lob.RowID = reader.GetInt32(0);
                    lob.LeaveID = reader.GetString(1);
                    lob.LeaveDescription = reader.GetString(2);
                    lob.year = reader.GetInt32(3);
                    lob.LeaveCount = reader.GetInt32(4);
                    lob.CreateUser = reader.GetString(5);
                    lob.CreateTime = reader.GetDateTime(6);
                    lobList.Add(lob);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying LEAVE OB Data");

            }
            return lobList;

        }

    }
}
