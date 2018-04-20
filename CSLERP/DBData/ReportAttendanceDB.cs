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
    class reportattendence
    {
        public int RowID { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public DateTime Etime { get; set; }
        public DateTime Updatetime { get; set; }
        public string DataValue { get; set; }
        public string DataID { get; set; }
        public int year { get; set; }
        public int LeaveCount { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public DateTime outime { get; set; }
        public DateTime Intime { get; set; }
        public DateTime PlannedoutTime { get; set; }
        public DateTime PlannedIntime { get; set; }
    }

    class ReportAttendanceDB
    {
        public List<reportattendence> getEmployeeList()
        {
            reportattendence lob;
            List<reportattendence> lobList = new List<reportattendence>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select EmployeeID,Name from ViewEmployeeDetails where OfficeID='NVP' and status=1 order by Name ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lob = new reportattendence();
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

        //public Boolean getEmployeePresentList(string date, string empid)
        //{
        //    bool status = false;
        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "select distinct a.EmployeeID  from BiometricAttendance a where cast(a.Edatetime as date)='" + date + "' and a.EmployeeID='" + empid + "' ";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            status = true;
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error querying Employee Present List");
        //    }
        //    return status;
        //}

        public List<reportattendence> getEmployeePresentListnew(string mon, string yr)
        {
            reportattendence lv;
            List<reportattendence> prsnt = new List<reportattendence>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.EmployeeID,a.Edatetime  from BiometricAttendance a where datepart(yy,a.Edatetime)='" + yr + "' and datepart(MM,a.Edatetime)='" + mon + "' ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lv = new reportattendence();
                    lv.EmployeeID = reader.GetString(0);
                    lv.Etime = reader.GetDateTime(1);
                    prsnt.Add(lv);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Employee Present List");
            }
            return prsnt;
        }


        public List<reportattendence> getEmployeeLeaveList(string mon, string yr)
        {
            reportattendence lv;
            List<reportattendence> leave = new List<reportattendence>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select e.EmployeeID,Lr.SanctionedFromDate,Lr.SanctionedToDate from LeaveRequest Lr ,ERPUser e  " +
                                "where Lr.Status = 1 and Lr.DocumentStatus = 99 and Lr.LeaveRequestStatus in (1,9,11,4,6) and " +
                                " lr.UserID = e.UserID and datepart(yy, Lr.SanctionedToDate)= '" + yr + "' and " +
                                " (DATEPART(MM, Lr.SanctionedFromDate) = '" + mon + "' or DATEPART(MM, Lr.SanctionedToDate) = '" + mon + "')";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lv = new reportattendence();
                    lv.EmployeeID = reader.GetString(0);
                    lv.fromdate = reader.GetDateTime(1);
                    lv.todate = reader.GetDateTime(2);
                    leave.Add(lv);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Employee Leave List");
            }
            return leave;
        }

        public List<reportattendence> getMrList(string mon, string yr)
        {
            reportattendence lv;
            List<reportattendence> leave = new List<reportattendence>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select mr.EmployeeID,mr.OutTime,mr.InTime,mr.ExitTimePlanned,mr.ReturnTimePlanned from MovementRegister mr where mr.Status = 1 " +
                               " and mr.DocumentStatus in (99,3,4) and datepart(yy, mr.OutTime)= '" + yr + "' and datepart(MM, mr.OutTime) = '" + mon + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lv = new reportattendence();
                    lv.EmployeeID = reader.GetString(0);
                    lv.outime = reader.GetDateTime(1);
                    lv.Intime = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2);
                    lv.PlannedoutTime = reader.GetDateTime(3);
                    lv.PlannedIntime = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4);
                    leave.Add(lv);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Employee Leave List");
            }
            return leave;
        }

        //public Boolean getEmployeeEarlyList(string date, string empid)
        //{
        //    bool status = false;
        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = " select distinct e.EmployeeID,a.DataValue,CAST(b.Edatetime as time)time " +
        //                       "from BiometricAttendance e, (select a.DataValue from CompanyData a " +
        //                       "where a.DataID = 'OfficeEndTime') a, (select top 1 c.Edatetime, c.EmployeeID " +
        //                       "from BiometricAttendance c where cast(c.Edatetime as date) = '" + date + "' and " +
        //                       "c.EmployeeID = '" + empid + "' order by cast(c.Edatetime as time) Desc)b where " +
        //                       " a.DataValue > CAST(b.Edatetime as time) and  e.EmployeeID = b.EmployeeID ";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            status = true;
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error querying Employee Early List");
        //    }
        //    return status;
        //}


        //public Boolean getEmployeeLateList(string date, string empid)
        //{
        //    bool status = false;
        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "select distinct e.EmployeeID,a.DataValue,CAST(b.Edatetime as time)time " +
        //                   "from BiometricAttendance e, (select a.DataValue from CompanyData a " +
        //                   "where a.DataID = 'OfficeStartTime') a, (select top 1 c.Edatetime, c.EmployeeID from " +
        //                   "BiometricAttendance c where cast(c.Edatetime as date) = '" + date + "' and " +
        //                    "c.EmployeeID = '" + empid + "' order by cast(c.Edatetime as time) Asc)b where " +
        //                    "a.DataValue < CAST(b.Edatetime as time) and  e.EmployeeID = b.EmployeeID ";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            status = true;
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error querying Employee Late List");
        //    }
        //    return status;
        //}

        public reportattendence getEmployeeLatenEarlyList(string date, string empid)
        {
            reportattendence lobList = new reportattendence();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = " select distinct d.EmployeeID,a.Edatetime,b.Edatetime from BiometricAttendance d, " +
                               " (select top 1 c.Edatetime, c.EmployeeID from BiometricAttendance c  where " +
                               " cast(c.Edatetime as date) = '" + date + "' and c.EmployeeID = '" + empid + "'  order by cast(c.Edatetime as time) Asc) a, " +
                               " (select top 1 c.Edatetime,c.EmployeeID from BiometricAttendance c  where " +
                               " cast(c.Edatetime as date) = '" + date + "' and c.EmployeeID = '" + empid + "'  order by cast(c.Edatetime as time) desc ) b " +
                                " where d.EmployeeID = a.EmployeeID and d.EmployeeID = b.EmployeeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {

                    lobList.EmployeeID = reader.GetString(0);
                    lobList.EntryTime = reader.GetDateTime(1);
                    lobList.ExitTime = reader.GetDateTime(2);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Employee Late and Early List");
            }
            return lobList;
        }

        public static DateTime getUpdatedateTime()
        {
            DateTime dt = DateTime.MinValue;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select  Max(UploadTime) from BiometricAttendance";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    dt = reader.GetDateTime(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Updatetime");
            }
            return dt;
        }

        public List<reportattendence> getcompanydata()
        {
            reportattendence lst;
            List<reportattendence> lobList = new List<reportattendence>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = " select a.DataID,a.DataValue  from CompanyData a where a.DataID in ('OfficeOpenTime','OfficeCloseTime')";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lst = new reportattendence();
                    lst.DataID = reader.GetString(0);
                    lst.DataValue = reader.GetString(1);
                    lobList.Add(lst);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying company Data List");
            }
            return lobList;
        }
    }
}
