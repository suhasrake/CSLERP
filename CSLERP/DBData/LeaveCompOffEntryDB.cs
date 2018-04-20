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
    class leavecompoff
    {
        public int RowID { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveID { get; set; }
        public string LeaveDescription { get; set; }
        public DateTime cdate { get; set; }
        public int LeaveStatus { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }

    class LeaveCompOffEntryDB
    {
        public  List<leavecompoff> getLeaveOBDetails(int empid)
        {
            leavecompoff lob;
            List<leavecompoff> lobList = new List<leavecompoff>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = " select a.RowID,a.LeaveID,a.Date "+
                               ",a.Status from LeaveEarning a" +
                               "  where a.EmployeeID='"+empid+ "' and a.Status=1 order by Createtime desc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lob = new leavecompoff();
                    lob.RowID = reader.GetInt32(0);
                    lob.LeaveID = reader.GetString(1);
                    lob.cdate = reader.GetDateTime(2);
                    //lob.LeaveStatus = reader.GetInt32(3);
                    lob.Status = reader.GetInt32(3);
                    lobList.Add(lob);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying leavecompoff Data");
            
            }
            return lobList;
            
        }
       


        public  Boolean insertLeaveCompOff(leavecompoff lco)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                    updateSQL = "insert into LeaveEarning (EmployeeID,LeaveID,Date,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + lco.EmployeeID + "'," +
                     "'CO','" +
                     lco.cdate + "',"+ 1 +"," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                     ActivityLogDB.PrepareActivityLogQquerString("insert", "LeaveEarning", "", updateSQL) +
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



        public Boolean DeleteLeaveCompOff(int rowid)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                updateSQL = "update LeaveEarning set Status=4 where RowID='" + rowid + "' ";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("insert", "LeaveEarning", "", updateSQL) +
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


        public  int getMaxAccuralForLeaveType()
        {
            int str = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select MaxAccrual from LeaveType where LeaveID ='CO'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    str = reader.GetInt32(0);
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
                string query = "select Gender from Employee where EmployeeID = '" + EmpID +"'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    str = reader.IsDBNull(0)?"":reader.GetString(0);
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
    }
}
