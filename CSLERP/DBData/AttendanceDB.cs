using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class attendance
    {
        public int RowID { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string AttendenceStatus { get; set; }
        public string officeID { get; set; }
        public string officeName { get; set; }
        public DateTime StatusDate { get; set; }
        public DateTime Updatetime { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }

    class AttendanceDB
    {

        public List<attendance> getEmployeeList()
        {
            attendance lob;
            List<attendance> lobList = new List<attendance>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = " select distinct b.EmployeeID,b.Name " +
                               " from ViewEmployeeLocation b where b.EmployeeStatus='1' and " +
                               " b.OfficeID = (select top 1 OfficeID from EmployeePosting " +
                               " where EmployeeID = '" + Login.empLoggedIn + "' order by PostingDate desc)  ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lob = new attendance();
                    lob.EmployeeID = reader.GetString(0);
                    lob.EmployeeName = reader.GetString(1);
                    lobList.Add(lob);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Employee List Data");
            }
            return lobList;
        }

        public List<attendance> getEmployeeLeaveList(DateTime dt)
        {
            attendance lob;
            List<attendance> lobList = new List<attendance>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select b.EmployeeID  from LeaveRequest a, ViewUserEmployeeList b " +
                               " where a.UserID = b.UserID and " +
                               " ((a.SanctionedFromDate between '" + dt.ToString("yyyy-MM-dd") + "'  and '" + dt.ToString("yyyy-MM-dd") + "' or " +
                               " (a.SanctionedToDate between '" + dt.ToString("yyyy-MM-dd") + "'  and '" + dt.ToString("yyyy-MM-dd") + "'  or " +
                               " ('" + dt.ToString("yyyy-MM-dd") + "' >= a.SanctionedFromDate and '" + dt.ToString("yyyy-MM-dd") + "' <= a.SanctionedToDate) or " +
                               " ('" + dt.ToString("yyyy-MM-dd") + "' >= a.SanctionedFromDate and '" + dt.ToString("yyyy-MM-dd") + "' <= a.SanctionedToDate) ) and " +
                               " a.Status = 1 and a.DocumentStatus = 99 ))";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lob = new attendance();
                    lob.EmployeeID = reader.GetString(0);
                    lobList.Add(lob);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Leave List Data");
            }
            return lobList;
        }

        public List<attendance> getEmployeestatusList()
        {
            attendance lob;
            List<attendance> lobList = new List<attendance>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = " select b.EmployeeID , a.AttendanceStatus from Attendance a, " +
                              " (select distinct b.EmployeeID " +
                              " from ViewEmployeeLocation b where " +
                              " b.OfficeID = (select top 1 OfficeID from EmployeePosting " +
                              " where EmployeeID = '" + Login.empLoggedIn + "' order by PostingDate desc)) b " +
                              " where a.EmployeeID = b.EmployeeID and StatusDate = convert(date, GETDATE())   ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lob = new attendance();
                    lob.EmployeeID = reader.GetString(0);
                    lob.AttendenceStatus = reader.GetString(1);
                    lobList.Add(lob);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Status List Data");
            }
            return lobList;
        }




        public List<attendance> getEmployeeBiometricList(DateTime dt)
        {
            attendance lv;
            List<attendance> prsnt = new List<attendance>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.EmployeeID  from BiometricAttendance a where CONVERT(date,a.EDateTime)= '" + dt.ToString("yyyy-MM-dd") + "' ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lv = new attendance();
                    lv.EmployeeID = reader.GetString(0);
                    prsnt.Add(lv);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Employee Biometric List");
            }
            return prsnt;
        }

        public List<attendance> getMrEmployeeList(DateTime dt)
        {
            attendance lv;
            List<attendance> leave = new List<attendance>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select mr.EmployeeID from MovementRegister mr where mr.Status = 1 and "+
                    "mr.DocumentStatus in (99,3,4) and convert(date, mr.OutTime)= '"+dt.ToString("yyyy-MM-dd")+"'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lv = new attendance();
                    lv.EmployeeID = reader.GetString(0);
                    leave.Add(lv);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Employee MR List");
            }
            return leave;
        }

        public Boolean UpdateAttendance(List<attendance> attendanceupdate, List<attendance> attendanceinsert, List<attendance> emplst, DateTime dt)
        {
            Boolean status = true;
            string utString = "";
            string query = "";
            string updateSQL = "";
            try
            {
                ////List<attendance> emplst = getEmployeestatusList();
                foreach (attendance atdnce in attendanceupdate)
                {
                    attendance lst = emplst.FirstOrDefault(x => x.EmployeeID == atdnce.EmployeeID);
                    if (lst != null)
                    {
                        updateSQL = "update Attendance set AttendanceStatus='" + atdnce.AttendenceStatus + "', " +
                                     "UpdateTime=GETDATE() where EmployeeID='" + atdnce.EmployeeID + "' and" +
                                     " StatusDate='"+dt.ToString("yyyy-MM-dd")+"' ";
                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                         ActivityLogDB.PrepareActivityLogQquerString("Update", "Attendance", "", updateSQL) +
                         Main.QueryDelimiter;

                    }
                    else //leave will be updated in this list
                    {
                        query = "insert into Attendance (EmployeeID,StatusDate,AttendanceStatus,UpdateTime,CreateTime,CreateUser)" +
                     "values (" +
                     "'" + atdnce.EmployeeID + "'," +
                     " '"+dt.ToString("yyyy-MM-dd")+"' , "+ 
                       "'" + atdnce.AttendenceStatus + "'," +
                        "GETDATE()" + " ," +
                     "GETDATE()" + "," +
                     "'" + Login.userLoggedIn + "'" + ")";
                        utString = utString + query + Main.QueryDelimiter;
                        utString = utString +
                         ActivityLogDB.PrepareActivityLogQquerString("insert", "Attendance", "", query) +
                         Main.QueryDelimiter;
                    }
                }
                foreach (attendance atdncein in attendanceinsert)
                {
                    updateSQL = "insert into Attendance (EmployeeID,StatusDate,AttendanceStatus,UpdateTime,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + atdncein.EmployeeID + "'," +
                      " '" + dt.ToString("yyyy-MM-dd") + "' , " +
                      "'" + atdncein.AttendenceStatus + "'," +
                       "GETDATE()" + " ," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                     ActivityLogDB.PrepareActivityLogQquerString("insert", "Attendance", "", updateSQL) +
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
        //ReportDailyAttendence

        public List<catalogue> getCatalogues()
        {
            catalogue cat;
            List<catalogue> Catalogues = new List<catalogue>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select CatalogueValueID,Description from CatalogueValue where" +
                         " CatalogueID='AttendanceStatus' and Status=1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat = new catalogue();
                    cat.catalogueID = reader.GetString(0);
                    cat.description = reader.GetString(1);
                    Catalogues.Add(cat);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return Catalogues;
        }

        public List<attendance> getEmployeeoffceList(string officeID, string Attstat)
        {
            attendance lob;
            List<attendance> lobList = new List<attendance>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                if (officeID != "Total" && Attstat != "Total")
                {
                    query = "   select b.EmployeeID,c.AttendanceStatus,b.Name,b.OfficeName,b.OfficeID  from ViewEmployeeLocation b," +
                               " Employee a, Attendance c where a.EmployeeID = b.EmployeeID and a.EmployeeID = c.EmployeeID and " +
                               " b.OfficeID = '" + officeID + "' and a.Status = 1 and c.AttendanceStatus='" + Attstat + "' and c.StatusDate = CONVERT(date, GETDATE()) order by b.name";
                }
                else if (officeID != "Total" && Attstat == "Total")
                {
                    query = "   select b.EmployeeID,c.AttendanceStatus,b.Name,b.OfficeName,b.OfficeID  from ViewEmployeeLocation b," +
                              " Employee a, Attendance c where a.EmployeeID = b.EmployeeID and a.EmployeeID = c.EmployeeID and " +
                              " b.OfficeID = '" + officeID + "' and a.Status = 1  and c.StatusDate = CONVERT(date, GETDATE()) order by b.name";
                }
                else if (officeID == "Total" && Attstat != "Total")
                {
                    query = "   select b.EmployeeID,c.AttendanceStatus,b.Name,b.OfficeName,b.OfficeID  from ViewEmployeeLocation b," +
                              " Employee a, Attendance c where a.EmployeeID = b.EmployeeID and a.EmployeeID = c.EmployeeID " +
                              " and a.Status = 1 and c.AttendanceStatus='" + Attstat + "' and c.StatusDate = CONVERT(date, GETDATE()) order by b.name";
                }
                else if (officeID == "Total" && Attstat == "Total")
                {
                    query = "   select b.EmployeeID,c.AttendanceStatus,b.Name,b.OfficeName,b.OfficeID from ViewEmployeeLocation b," +
                              " Employee a, Attendance c where a.EmployeeID = b.EmployeeID and a.EmployeeID = c.EmployeeID and " +
                              " a.Status = 1 and c.StatusDate = CONVERT(date, GETDATE()) order by b.name";
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lob = new attendance();
                    lob.EmployeeID = reader.GetString(0);
                    lob.AttendenceStatus = reader.GetString(1);
                    lob.EmployeeName = reader.GetString(2);
                    lob.officeName = reader.GetString(3);
                    lob.officeID = reader.GetString(4);
                    lobList.Add(lob);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying EmployeeOffice List Data");
            }
            return lobList;
        }
        public List<attendance> getEmployeeAttendanceList(DateTime dt, string officeid)
        {
            attendance lob;
            List<attendance> lobList = new List<attendance>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                if (officeid == "All")
                {
                    query = " select b.EmployeeID,b.Name,b.OfficeName,c.AttendanceStatus,c.StatusDate  from ViewEmployeeLocation b," +
                           " Employee a, Attendance c where a.EmployeeID = b.EmployeeID and a.EmployeeID = c.EmployeeID " +
                            " and a.Status = 1  and Datepart(MONTH, c.StatusDate) = '" + dt.Month + "' " +
                           " and DATEPART(YEAR,c.StatusDate)='" + dt.Year + "' order by b.name, c.AttendanceStatus";
                }
                else
                {
                    query = " select b.EmployeeID,b.Name,b.OfficeName,c.AttendanceStatus,c.StatusDate  from ViewEmployeeLocation b," +
                           " Employee a, Attendance c where a.EmployeeID = b.EmployeeID and a.EmployeeID = c.EmployeeID and " +
                            " b.OfficeID = '" + officeid + "' and a.Status = 1  and Datepart(MONTH, c.StatusDate) = '" + dt.Month + "' " +
                           " and DATEPART(YEAR,c.StatusDate)='" + dt.Year + "' order by b.name, c.AttendanceStatus";
                }


                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lob = new attendance();
                    lob.EmployeeID = reader.GetString(0);
                    lob.EmployeeName = reader.GetString(1);
                    lob.officeName = reader.GetString(2);
                    lob.AttendenceStatus = reader.GetString(3);
                    lob.StatusDate = reader.GetDateTime(4);
                    lobList.Add(lob);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Employee Attendance List Data");
            }
            return lobList;
        }
        public List<attendance> getEmployeeListforoffice(string officeid)
        {
            attendance lob;
            List<attendance> lobList = new List<attendance>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select EmployeeID,Name from ViewEmployeeDetails where OfficeID='" + officeid + "' and status=1 order by Name ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lob = new attendance();
                    lob.EmployeeID = reader.GetString(0);
                    lob.EmployeeName = reader.GetString(1);
                    lobList.Add(lob);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Employee List");
            }
            return lobList;
        }



        public List<attendance> getEmployeestatusListforoffice(string officeid,DateTime dt)
        {
            attendance lob;
            List<attendance> lobList = new List<attendance>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = " select b.EmployeeID , a.AttendanceStatus from Attendance a, " +
                               " (select distinct b.EmployeeID from ViewEmployeeLocation b where " +
                               " b.OfficeID = '" + officeid + "') b " +
                               " where a.EmployeeID = b.EmployeeID and StatusDate = '"+dt.ToString("yyyy-MM-dd")+"'  ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lob = new attendance();
                    lob.EmployeeID = reader.GetString(0);
                    lob.AttendenceStatus = reader.GetString(1);
                    lobList.Add(lob);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Status List Data");
            }
            return lobList;
        }
    }
}
