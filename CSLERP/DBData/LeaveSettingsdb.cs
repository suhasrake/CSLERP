using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CSLERP.DBData
{
    public class Leave
    {
        public string leaveID { get; set; }
        public string description { get; set; }
        public int MaxAccrual { get; set; }
        public int SanctionType { get; set; }
        public string designation { get; set; }
        public string officeID { get; set; }
        public string officeName { get; set; }
        public int MaxDays { get; set; }
        public string Gender { get; set; }
        public int rowid { get; set; }
        public int ahead { get; set; }
        public int Delay { get; set; }
        public int CarryForward { get; set; }

    }

    public class HolidayList
    {
        public int RowID { get; set; }
        public string Type { get; set; }
        public DateTime date { get; set; }
        public string officeID { get; set; }
        public string Description { get; set; }
        public string Weekoffs { get; set; }
    }

    class LeaveSettingsdb
    {
        public List<Leave> getLeaveTypeList()
        {
            Leave Alist;
            List<Leave> AccList = new List<Leave>();
            try
            {
                string query = "select LeaveID,Description,MaxAccrual,SanctionType,Gender,RowID, " +
                              " DaysAhead,DaysDelay,Carryforward from LeaveType  ";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Alist = new Leave();
                    Alist.leaveID = reader.GetString(0);
                    Alist.description = reader.GetString(1);
                    Alist.MaxAccrual = reader.GetInt32(2);
                    Alist.SanctionType = reader.GetInt32(3);
                    Alist.Gender = reader.GetString(4);
                    Alist.rowid = reader.GetInt32(5);
                    Alist.ahead = reader.GetInt32(6);
                    Alist.Delay = reader.GetInt32(7);
                    Alist.CarryForward = reader.GetInt32(8);
                    AccList.Add(Alist);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return AccList;
        }
        public List<Leave> getSanctionLimitList()
        {
            Leave Alist;
            List<Leave> AccList = new List<Leave>();
            try
            {
                string query = "select LeaveID,Designation,MaxSanctionLimit,RowID from LeaveSanctionlimit ";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Alist = new Leave();
                    Alist.leaveID = reader.GetString(0);
                    Alist.designation = reader.GetString(1);
                    Alist.MaxAccrual = reader.GetInt32(2);
                    Alist.rowid = reader.GetInt32(3);
                    AccList.Add(Alist);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return AccList;
        }
        public List<Leave> getleaveofficemappingList()
        {
            Leave Alist;
            List<Leave> AccList = new List<Leave>();
            try
            {
                string query = "select a.LeaveID,a.OfficeID,a.MaxDays,a.RowID,b.Name from LeaveOfficeMapping a, Office b where a.OfficeID=b.officeID";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Alist = new Leave();
                    Alist.leaveID = reader.GetString(0);
                    Alist.officeID = reader.GetString(1);
                    Alist.MaxDays = reader.GetInt32(2);
                    Alist.rowid = reader.GetInt32(3);
                    Alist.officeName = reader.GetString(4);
                    AccList.Add(Alist);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return AccList;
        }

        public Boolean UpdateLeaveType(Leave leavesett)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Update LeaveType set Description='" + leavesett.description + "',SanctionType='" + leavesett.SanctionType + "'," +
                    " MaxAccrual='" + leavesett.MaxAccrual + "',Gender='" + leavesett.Gender + "',DaysAhead='" + leavesett.ahead + "' , " +
                    " DaysDelay ='" + leavesett.Delay + "',Carryforward='" + leavesett.CarryForward + "' where LeaveID='" + leavesett.leaveID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "LeaveType", "", updateSQL) +
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

        public Boolean UpdateOfficeHW(HolidayList Ofw)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Update HolidayList set Description='" + Ofw.Description + "',officeIDs='" + Ofw.officeID + "'," +
                    " Date='" + Ofw.date.ToString("yyyy-MM-dd") + "' where RowID='" + Ofw.RowID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "HolidayList", "", updateSQL) +
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

        public Boolean UpdateOfficeWD(HolidayList OWD)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Update OfficeWorkingDay set Description='" + OWD.Description + "',officeID='" + OWD.officeID + "'" +
                    " where RowID='" + OWD.RowID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "OfficeWorkingDay", "", updateSQL) +
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


        public Boolean UpdateOfficeWO(HolidayList Ofw)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Update OfficeWeekOff set WeekOffs='" + Ofw.Weekoffs + "'" +
                                   " where OfficeID='" + Ofw.officeID + "' and  RowID='" + Ofw.RowID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "OfficeWeekOff", "", updateSQL) +
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

        public Boolean UpdateLeaveSanctionLimit(Leave leavesett)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Update LeaveSanctionLimit set MaxSanctionLimit='" + leavesett.MaxAccrual + "',Designation='" + leavesett.designation + "' where LeaveID='" + leavesett.leaveID + "' and RowID='" + leavesett.rowid + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "LeaveSanctionLimit", "", updateSQL) +
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

        public Boolean UpdateLeaveOfficeMapping(Leave leavesett)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Update LeaveOfficeMapping set MaxDays='" + leavesett.MaxDays + "',OfficeID='" + leavesett.officeID + "' where LeaveID='" + leavesett.leaveID + "'and RowID='" + leavesett.rowid + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "LeaveSanctionLimit", "", updateSQL) +
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

        public Boolean InsertLeaveType(Leave leave)
        {
            Boolean status = true;
            string utString = "";
            try
            {

                string updateSQL = "insert into LeaveType (LeaveID,Description,MaxAccrual,Gender,SanctionType,DaysAhead,DaysDelay,Carryforward)" +
                    "values (" +
                    "'" + leave.leaveID + "'," +
                     "'" + leave.description + "','" +
                    leave.MaxAccrual + "','" +
                     leave.Gender + "','" +
                     leave.SanctionType + "'," +
                    " '" + leave.ahead + "'," +
                     " '" + leave.Delay + "'," +
                     " '" + leave.CarryForward + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("insert", "LeaveType", "", updateSQL) +
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

        public Boolean InsertLeaveSanctionLimit(Leave leave)
        {
            Boolean status = true;
            string utString = "";
            try
            {

                string updateSQL = "insert into LeaveSanctionLimit (LeaveID,Designation,MaxSanctionLimit)" +
                    "values (" +
                    "'" + leave.leaveID + "'," +
                     "'" + leave.designation + "','" +
                    leave.MaxAccrual + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("insert", "LeaveSanctionLimit", "", updateSQL) +
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


        public Boolean InsertLeaveOfficeHW(HolidayList ofh)
        {
            Boolean status = true;
            string utString = "";
            try
            {

                string updateSQL = "insert into HolidayList (Type,Date,officeIDs,Description)" +
                    "values (" +
                    "'" + ofh.Type + "'," +
                     "'" + ofh.date.ToString("yyyy-MM-dd") + "','" +
                       ofh.officeID + "','" +
                    ofh.Description + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("insert", "HolidayList", "", updateSQL) +
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

        public Boolean InsertLeaveOfficeWD(HolidayList ofh)
        {
            Boolean status = true;
            string utString = "";
            try
            {

                string updateSQL = "insert into OfficeWorkingDay (Type,Date,officeID,Description)" +
                    "values (" +
                    "'" + ofh.Type + "'," +
                     "'" + ofh.date.ToString("yyyy-MM-dd") + "','" +
                       ofh.officeID + "','" +
                    ofh.Description + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("insert", "OfficeWorkingDay", "", updateSQL) +
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

        public Boolean InsertLeaveOfficeWO(HolidayList ofh)
        {
            Boolean status = true;
            string utString = "";
            try
            {

                string updateSQL = "insert into OfficeWeekOff (officeID,Weekoffs)" +
                    "values (" +
                    "'" + ofh.officeID + "','" +
                       ofh.Weekoffs + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("insert", "OfficeWeekOff", "", updateSQL) +
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

        public Boolean validateOfficeWO(HolidayList ofhw)
        {
            int count = 0;
            Boolean status = true;
            try
            {
                string query = "select WeekOffs from OfficeWeekOff where OfficeID='" + ofhw.officeID + "'";
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
        public Boolean InsertLeaveofficeMapping(Leave leave)
        {
            Boolean status = true;
            string utString = "";
            try
            {

                string updateSQL = "insert into LeaveOfficeMapping (LeaveID,OfficeID,MaxDays)" +
                    "values (" +
                    "'" + leave.leaveID + "'," +
                     "'" + leave.officeID + "','" +
                    leave.MaxDays + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("insert", "LeaveOfficeMapping", "", updateSQL) +
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
        public Boolean ValidateLeaveType(Leave leave)
        {
            Regex r = new Regex("[a-zA-Z]");
            Boolean stat = true;
            try
            {
                if (leave.leaveID == "" || leave.leaveID.Trim().Length == 0)
                {
                    stat = false;
                }
                if (leave.description == "" || leave.description.Trim().Length == 0)
                {
                    stat = false;
                }
                if (leave.MaxAccrual <= 0 || r.IsMatch(leave.ahead.ToString()))
                {
                    stat = false;
                }
                if (leave.SanctionType == 0)
                {
                    stat = false;
                }
                if (leave.Gender == "" || leave.Gender.Trim().Length == 0)
                {
                    stat = false;
                }
                if (leave.ahead <= 0 || r.IsMatch(leave.ahead.ToString()))
                {
                    stat = false;
                }
            }
            catch (Exception ex)
            {

            }
            return stat;
        }

        public Boolean ValidateOfficeHW(HolidayList Ofc)
        {
            //Regex r = new Regex("[a-zA-Z]");
            Boolean stat = true;
            try
            {
                if (Ofc.Type == "" || Ofc.Type.Trim().Length == 0)
                {
                    stat = false;
                }
                if (Ofc.Description == "" || Ofc.Description.Trim().Length == 0)
                {
                    stat = false;
                }

                if (Ofc.officeID == "" || Ofc.officeID.Trim().Length == 0)
                {
                    stat = false;
                }
            }
            catch (Exception ex)
            {

            }
            return stat;
        }

        public Boolean ValidateOfficeWO(HolidayList Ofc)
        {
            //Regex r = new Regex("[a-zA-Z]");
            Boolean stat = true;
            try
            {
                if (Ofc.officeID == "" || Ofc.officeID.Trim().Length == 0)
                {
                    stat = false;
                }
            }
            catch (Exception ex)
            {

            }
            return stat;
        }



        public Boolean validateSanctionLimitList(Leave leave)
        {
            int count = 0;
            Boolean status = true;
            try
            {
                string query = "select LeaveID,Designation from LeaveSanctionlimit where LeaveID='" + leave.leaveID + "' and Designation='" + leave.designation + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    status = false;
                }
                // count =(int) cmd.ExecuteScalar();
                //if(count > 0)
                // {
                //     status = false;
                // }
                conn.Close();
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public Boolean validateLeaveType(Leave leave)
        {
            int count = 0;
            Boolean status = true;
            try
            {
                string query = "select LeaveID from LeaveType where LeaveID='" + leave.leaveID + "'";
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

        public Boolean validateOfficeHw(HolidayList ofhw)
        {
            int count = 0;
            Boolean status = true;
            try
            {
                string query = "select Description from HolidayList where Type='" + ofhw.Type + "' and Date='" + ofhw.date.ToString("yyyy-MM-dd") + "'";
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

        public Boolean validateOfficeWD(HolidayList ofhw)
        {
            int count = 0;
            Boolean status = true;
            try
            {
                string query = "select Description from OfficeWorkingDay where Type='" + ofhw.Type + "' and Date='" + ofhw.date.ToString("yyyy-MM-dd") + "'";
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

        public List<string> validateOfficeWDforHW(HolidayList ofhw)
        {
            string offc = "";
            List<string> offlist = new List<string>();
            try
            {
                string[] office = ofhw.officeID.Split(Main.delimiter1);
                foreach (string ofce in office)
                {
                    if (ofce != "")
                    {
                        string query = "select Description from HolidayList where  Date='" + ofhw.date.ToString("yyyy-MM-dd") + "' and   officeIDs like '%" + ofce + "%'";
                        SqlConnection conn = new SqlConnection(Login.connString);
                        SqlCommand cmd = new SqlCommand(query, conn);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (!reader.Read())
                        {
                            offc = ofce;
                        }
                        conn.Close();
                        if (offc != "")
                        {
                            offlist.Add(offc);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("validateOfficeWDforHW Error!!!");
            }
            return offlist;
        }

        //public Boolean validateOfficeWDforHW(officehw ofhw)
        //{
        //    int count = 0;
        //    Boolean status = false;
        //    try
        //    {
        //        string query = "select Description from OfficeHW where  Date='" + ofhw.date.ToString("yyyy-MM-dd") + "'";
        //        SqlConnection conn = new SqlConnection(Login.connString);
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
        //        status = false;
        //    }
        //    return status;
        //}

        public List<string> validateOfficeWDforWO(HolidayList ofhw)
        {
            string offc="";
            List<string> offlist = new List<string>();
            try
            {
                string[] office = ofhw.officeID.Split(Main.delimiter1);
                foreach(string ofce in office)
                {
                    if (ofce != "")
                    {
                        string query = "select Weekoffs from OfficeWeekOff where  OfficeID='" + ofce + "'";
                        SqlConnection conn = new SqlConnection(Login.connString);
                        SqlCommand cmd = new SqlCommand(query, conn);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            List<DateTime> allweekenddate = GetAllweekends(UpdateTable.getSQLDateTime().Year, reader.GetString(0));
                            if (!allweekenddate.Contains(ofhw.date.Date))
                            {
                                offc = ofce;
                            }
                        }
                        else
                        {
                            offc = ofce;
                        }
                        conn.Close();
                        if (offc != "")
                        {
                            offlist.Add(offc);
                        }
                    }             
                }              
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("validateOfficeWDforWO Error!!!");
            }
            return offlist;
        }

        List<DateTime> GetAllweekends(int year, string AllDays)
        {
            List<DateTime> Dates = new List<DateTime>();
            DateTime Date = new DateTime(year, 1, 1);
            string[] days = AllDays.Split(Main.delimiter1); 
            //string[] dayOfWeekArr = string.Format()
            if (days.Length > 0 )
            {
                foreach (string day in days)
                {
                    if(day!="")
                    {
                        DayOfWeek val = 0;
                        switch (day)
                        {
                            case "Sunday":
                                val = DayOfWeek.Sunday;
                                break;
                            case "Monday":
                                val = DayOfWeek.Monday;
                                break;
                            case "Tueday":
                                val = DayOfWeek.Tuesday;
                                break;
                            case "Wednesday":
                                val = DayOfWeek.Wednesday;
                                break;
                            case "Thursday":
                                val = DayOfWeek.Thursday;
                                break;
                            case "Friday":
                                val = DayOfWeek.Friday;
                                break;
                            case "Saturday":
                                val = DayOfWeek.Saturday;
                                break;
                        }
                        while (Date.Year == year)
                        {
                            if (Date.DayOfWeek == val)
                                Dates.Add(Date);
                            Date = Date.AddDays(1);
                        }
                    }
                    Date = new DateTime(year, 1, 1);
                }

            }
            return Dates;
        }


        public Boolean Validatemapping(Leave leave)
        {
            int count = 0;
            Boolean status = true;
            try
            {
                string query = "select LeaveID,OfficeID from LeaveOfficeMapping where LeaveID='" + leave.leaveID + "' and OfficeID='" + leave.officeID + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    status = false;
                }
                // count =(int) cmd.ExecuteScalar();
                //if(count > 0)
                // {
                //     status = false;
                // }
                conn.Close();
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public Boolean ValidateLeaveSanctionLimit(Leave leave)
        {
            Boolean stat = true;
            try
            {
                if (leave.leaveID == "" || leave.leaveID.Trim().Length == 0)
                {
                    stat = false;
                }
                if (leave.designation == "" || leave.designation.Trim().Length == 0)
                {
                    stat = false;
                }
                if (leave.MaxAccrual <= 0)
                {
                    stat = false;
                }

            }
            catch (Exception ex)
            {

            }
            return stat;
        }
        public Boolean ValidateLeaveOfficeMapping(Leave leave)
        {
            Boolean stat = true;
            try
            {
                if (leave.leaveID == "" || leave.leaveID.Trim().Length == 0)
                {
                    stat = false;
                }
                if (leave.officeID == "" || leave.officeID.Trim().Length == 0)
                {
                    stat = false;
                }
                if (leave.MaxDays <= 0)
                {
                    stat = false;
                }
            }
            catch (Exception ex)
            {
                stat = false;
            }
            return stat;
        }

        public static List<Leave> fillleavecombo()
        {
            Leave Alist;
            List<Leave> AccList = new List<Leave>();
            try
            {
                string query = "select LeaveID,Description from LeaveType  ";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Alist = new Leave();
                    Alist.leaveID = reader.GetString(0);
                    Alist.description = reader.GetString(1);
                    AccList.Add(Alist);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return AccList;
        }

        public static void fillLeaveComboNew(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                List<Leave> leaves = fillleavecombo();
                foreach (Leave le in leaves)
                {
                    ////cmb.Items.Add(off.OfficeID + "-" + off.name);
                    Structures.ComboBoxItem cbitem =
                        new Structures.ComboBoxItem(le.description, le.leaveID);
                    cmb.Items.Add(cbitem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        //16-04-2018
        public static ListView getOfficeListView()
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
                OfficeDB OffDB = new OfficeDB();
                List<office> OffList = OffDB.getOffices();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("OfficeID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Office Name", -2, HorizontalAlignment.Left);
                foreach (office offc in OffList)
                {
                    if (offc.status == 1)
                    {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        item1.SubItems.Add(offc.OfficeID.ToString());
                        item1.SubItems.Add(offc.name.ToString());
                        lv.Items.Add(item1);
                    }
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }

        public List<HolidayList> getOfficeHW()
        {
            HolidayList hwlist;
            List<HolidayList> HOList = new List<HolidayList>();
            try
            {
                string query = "select RowID,Type,Date,officeIDs,Description from HolidayList where datepart(yyyy,Date)='" + UpdateTable.getSQLDateTime().Year + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    hwlist = new HolidayList();
                    hwlist.RowID = reader.GetInt32(0);
                    hwlist.Type = reader.GetString(1);
                    hwlist.date = reader.GetDateTime(2);
                    hwlist.officeID = reader.GetString(3);
                    hwlist.Description = reader.GetString(4);
                    HOList.Add(hwlist);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return HOList;
        }

        public List<HolidayList> getOfficeWO()
        {
            HolidayList WOlist;
            List<HolidayList> OfficeWOList = new List<HolidayList>();
            try
            {
                string query = "select RowID,OfficeID,Weekoffs from OfficeWeekOff ";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    WOlist = new HolidayList();
                    WOlist.RowID = reader.GetInt32(0);
                    WOlist.officeID = reader.GetString(1);
                    WOlist.Weekoffs = reader.GetString(2);
                    OfficeWOList.Add(WOlist);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return OfficeWOList;
        }

        public List<HolidayList> getOfficeWD()
        {
            HolidayList Wdlist;
            List<HolidayList> OfficeWDList = new List<HolidayList>();
            try
            {
                string query = "select RowID,Type,Date,officeID,Description from OfficeWorkingDay  where datepart(yyyy,Date)='"+UpdateTable.getSQLDateTime().Year+"' ";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Wdlist = new HolidayList();
                    Wdlist.RowID = reader.GetInt32(0);
                    Wdlist.Type = reader.GetString(1);
                    Wdlist.date = reader.GetDateTime(2);
                    Wdlist.officeID = reader.GetString(3);
                    Wdlist.Description = reader.GetString(4);
                    OfficeWDList.Add(Wdlist);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return OfficeWDList;
        }

    }
}
