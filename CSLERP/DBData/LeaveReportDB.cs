using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class leavereport
    {
        public int rowid { get; set; }
        public string userid { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string Leavetype { get; set; }
        public string officeid { get; set; } 
        public string officename { get; set; }
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

    class LeaveReportDB
    {
        ActivityLogDB alDB = new ActivityLogDB();


        public List<leavereport> getFilteredLeaveApproval(string officeid, int opt,string region)
        {
            leavereport lvpr;
            List<leavereport> LeaveApr = new List<leavereport>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query1 = "select a.EmployeeID,a.Name,a.OfficeID,a.OfficeName,b.PostingDate "+
                                "from ViewEmployeePosting a ,(select c.EmployeeID,MAX(c.PostingDate) as PostingDate from ViewEmployeePosting c group by c.EmployeeID) b "+
                                 "where a.EmployeeID = b.EmployeeID and b.PostingDate = a.PostingDate ";

                string query2 = "select s1.EmployeeID, s1.Name,s1.OfficeID,s1.OfficeName," + 
                                " s1.PostingDate from ViewEmployeePosting s1 inner join"+
                                "(select max(PostingDate) PostingDate,EmployeeID from ViewEmployeePosting "+
                            "group by EmployeeID) s2 on s1.EmployeeID = s2.EmployeeID and "+
                            "s1.PostingDate = s2.PostingDate where s1.OfficeID = '"+officeid+"' order by s1.EmployeeID";//part

                string query3 = "select s1.EmployeeID, s1.Name,s1.OfficeID,s1.OfficeName," + 
                                " s1.PostingDate from ViewEmployeePosting s1 inner join" +
                                "(select max(PostingDate) PostingDate,EmployeeID from ViewEmployeePosting " +
                            "group by EmployeeID) s2 on s1.EmployeeID = s2.EmployeeID and " +
                            "s1.PostingDate = s2.PostingDate,office b where b.RegionID = '"+region+"'and b.OfficeID=s1.OfficeID order by s1.EmployeeID";//region-part and office all

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
                    lvpr = new leavereport();
                    lvpr.EmployeeID = reader.GetString(0);
                    lvpr.EmployeeName = reader.GetString(1);
                    lvpr.officeid = reader.GetString(2);
                    lvpr.officename = reader.GetString(3); 
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


        public List<leavereport> getleavetype()
        {
            leavereport lvpr;
            List<leavereport> LeaveApr = new List<leavereport>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select distinct LeaveID from LeaveRequest where datepart(YEAR,SanctionedFromDate)=YEAR(GETDATE())";                 
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvpr = new leavereport();
                    lvpr.leaveid = reader.GetString(0);
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

        public List<leavereport> getLeaveRemain(string empid,string leaveid)
        {
            leavereport lvapp;
            List<leavereport> LVlist = new List<leavereport>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select  DATEDIFF(DAY,a.SanctionedFromDate ,a.SanctionedToDate ) as totaldays,d.EmployeeID,d.Name,d.OfficeID,d.OfficeName,a.LeaveID " +
                               "from LeaveRequest a, (select b.UserID,b.Name  from ViewUserEmployeeList b where b.EmployeeID = '"+empid+"') c, ViewEmployeeDetails d, ERPUser e " +
                               "where a.UserID = c.UserID and a.LeaveID in ('"+leaveid+"') and a.UserID=e.UserID and d.EmployeeID=e.EmployeeID and a.Status = 1 " +
                               " and a.DocumentStatus = 99 and a.LeaveRequestStatus in (1,11,10,9,7) and year(a.SanctionedFromDate)=year(GetDate()) ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvapp = new leavereport();
                    lvapp.leavepending =reader.IsDBNull(0)? '0' : reader.GetInt32(0);
                    lvapp.EmployeeID = reader.GetString(1);
                    lvapp.EmployeeName = reader.GetString(2);
                    lvapp.officeid = reader.GetString(3);
                    lvapp.officename = reader.GetString(4);
                    lvapp.leaveid = reader.GetString(5);
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

        public List<leaveapprove> getLeaveLimit()
        {
            leaveapprove lvapp;
            List<leaveapprove> LVlist = new List<leaveapprove>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "  select distinct a.LeaveID from LeaveOB a where a.Year=YEAR(GETDATE()) ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvapp = new leaveapprove();
                    lvapp.leaveid = reader.GetString(0);
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


        public List<leavereport> getLeaveRemain2(int Opt,string Region,string Officeid)
        {
            leavereport lvapp;
            List<leavereport> LVlist = new List<leavereport>();
            try
            {
                int inttemp = 0;
                DateTime dttemp = DateTime.Now;
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "";
                if (Opt == 2)
                {
                    query = "select  DATEDIFF(DAY,a.SanctionedFromDate ,a.SanctionedToDate ) as totaldays,d.EmployeeID,d.Name,a.LeaveID,d.OfficeName,d.OfficeID,a.SanctionedFromDate, a.SanctionedToDate ,a.Status,a.DocumentStatus from ViewEmployeeDetails d  FULL OUTER JOIN ERPUser e ON  e.EmployeeID = d.EmployeeID  FULL OUTER JOIN LeaveRequest a ON a.UserID = e.UserID where d.Status = 1 and   d.OfficeID = '" + Officeid + "'";
                }
                else if (Opt == 3)
                {
                    query = "select  DATEDIFF(DAY,a.SanctionedFromDate ,a.SanctionedToDate ) as totaldays,d.EmployeeID,d.Name,a.LeaveID,d.OfficeName,d.OfficeID,a.SanctionedFromDate, a.SanctionedToDate ,a.Status,a.DocumentStatus from ViewEmployeeDetails d  FULL OUTER JOIN ERPUser e ON  e.EmployeeID = d.EmployeeID  FULL OUTER JOIN LeaveRequest a ON a.UserID = e.UserID where d.Status = 1 and  d.RegionID = '" + Region + "'";

                }
                else
                {
                    query = "select  DATEDIFF(DAY,a.SanctionedFromDate ,a.SanctionedToDate ) as totaldays,d.EmployeeID,d.Name,a.LeaveID,d.OfficeName,d.OfficeID,a.SanctionedFromDate, a.SanctionedToDate ,a.Status,a.DocumentStatus from ViewEmployeeDetails d  FULL OUTER JOIN ERPUser e ON  e.EmployeeID = d.EmployeeID  FULL OUTER JOIN LeaveRequest a ON a.UserID = e.UserID where d.Status = 1";
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lvapp = new leavereport();
                    lvapp.leavepending = reader.IsDBNull(0) ? inttemp : reader.GetInt32(0);
                    lvapp.EmployeeID = reader.GetString(1);
                    lvapp.EmployeeName = reader.GetString(2);
                    lvapp.officeid = reader.IsDBNull(5) ? " " : reader.GetString(5);
                    lvapp.officename = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    lvapp.leaveid = reader.IsDBNull(3) ? " ": reader.GetString(3);
                    lvapp.sanctionedFrom = reader.IsDBNull(6) ? dttemp : reader.GetDateTime(6);
                    lvapp.sanctionedTo = reader.IsDBNull(7) ? dttemp : reader.GetDateTime(7);
                    lvapp.status = reader.IsDBNull(8) ? inttemp : reader.GetInt32(8);
                    lvapp.documentStatus = reader.IsDBNull(9) ? inttemp : reader.GetInt32(9);
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


        public static void fillRegionWiseOfficeCombo(System.Windows.Forms.ComboBox cmb, string region)
        {
            cmb.Items.Clear();
            try
            {
                OfficeDB officedb = new OfficeDB();
                List<office> Offices = officedb.getOfficesfromregion(region);
                foreach (office off in Offices)
                {
                    if (off.status == 1)
                    {
                        ////cmb.Items.Add(off.OfficeID + "-" + off.name);
                        Structures.ComboBoxItem cbitem =
                            new Structures.ComboBoxItem(off.name, off.OfficeID);
                        cmb.Items.Add(cbitem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
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
     
      

    }
}
