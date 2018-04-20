using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace CSLERP.DBData
{
    class movementhistory
    {
        public int rowid { get; set; }
        public DateTime date { get; set; }
        public string empid { get; set; }
        public string empname { get; set; }
        public Nullable <DateTime> exittime { get; set; }
        public string purpose { get; set; }
        public Nullable<DateTime> returntime { get; set; }
        public Nullable<DateTime> actexittime { get; set; }
        public Nullable<DateTime> actreturntime { get; set; }
        public int documentstatus { get; set; }
        public int status { get; set; }
    }
    class MovementHistoryDB
    {
        string strQry = "select a.RowID,a.CreateTime,a.EmployeeID,b.Name,a.Purpose,a.ExitTimePlanned,a.ReturnTimePlanned, outtime,intime,a.DocumentStatus,a.Status " +
                    " from MovementRegister a, Employee b where a.EmployeeID=b.EmployeeID and ";
        public List<movementhistory> getFilteredMovementHistory(DateTime from, DateTime to, int opt, Dictionary<String,String> dict)
        {
            movementhistory mh;
            List<movementhistory> movementHistory = new List<movementhistory>();
            try
            {
                //No Filter
                string query1 = strQry +
                      "CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") + "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") +
                      "' and a.DocumentStatus in (1,2,3,4,10,98,99,6) and a.Status in (1,98) order by a.CreateTime desc";
                //Only EmpID 
                string query2 = strQry+
                     " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") + "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") + "'" +
                     " and a.EmployeeID='" + dict["EmpId"] + "' and a.DocumentStatus in (1,2,3,4,10,98,99,6)  and a.Status in (1,98) order by a.CreateTime desc";
                //only purpose
                string query3 = strQry +
                    " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") + "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") + "'" +
                    " and a.Purpose='" + dict["Purpose"] + "' and a.DocumentStatus in (1,2,3,4,10,98,99,6)  and a.Status in (1,98) order by a.CreateTime desc";
                //only Doc Status
                string query4 = "";

                if (dict["DocStat"].Contains( Convert.ToString(6)))
                {
                    query4 = strQry +
                     " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") + "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") + "'" +
                    " and a.DocumentStatus in ( " + dict["DocStat"] + ") and a.Status in (1, 98) order by a.CreateTime desc";
                } 
                else
                {
                    query4 = strQry +
                     " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") + "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") + "'" +
                    " and a.DocumentStatus = " + dict["DocStat"] + " and a.Status = 1 order by a.CreateTime desc";
                }
                
                //EmpID & Purpose
                string query5 = strQry +
                   " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") + "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") + "'" +
                    " and a.EmployeeID='" + dict["EmpId"] + "'" +
                  " and a.Purpose='" + dict["Purpose"] + "' and a.Status = 1 order by a.CreateTime desc";
                //EmpID & Doc Stat
                string query6 = "";

                if(dict["DocStat"].Contains(Convert.ToString(6)))
                {
                    query6 = strQry +
                  " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") + "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") + "'" +
                   " and a.EmployeeID='" + dict["EmpId"] + "'" +
                 " and a.DocumentStatus in (" + dict["DocStat"] + ") and a.Status in (1, 98) order by a.CreateTime desc";
                }
                else
                {
                    query6 = strQry +
                  " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") + "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") + "'" +
                   " and a.EmployeeID='" + dict["EmpId"] + "'" +
                 " and a.DocumentStatus = " + dict["DocStat"] + " and a.Status = 1 order by a.CreateTime desc";
                }
               
                //Purpose & Doc Stat
                string query7 = "";

                if (dict["DocStat"].Contains(Convert.ToString(6)))
                {
                    query7 = strQry +
                 " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") + "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") + "'" +
                    " and a.Purpose='" + dict["Purpose"] + "'" +
                 " and a.DocumentStatus in (" + dict["DocStat"] + ") and a.Status in (1, 98) order by a.CreateTime desc";
                }
                else
                {
                    query7 = strQry +
                   " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") + "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") + "'" +
                    " and a.Purpose='" + dict["Purpose"] + "'" +
                  " and a.DocumentStatus = " + dict["DocStat"] + " and a.Status = 1 order by a.CreateTime desc";
                }
                //EmpID & Purpose & Doc Stat
                string query8 = "";

                if (dict["DocStat"].Contains(Convert.ToString(6)))
                {
                    query8 = strQry +
                  " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") + "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") + "'" +
                   " and a.EmployeeID='" + dict["EmpId"] + "'" +
                   " and a.Purpose='" + dict["Purpose"] + "'" +
                 " and a.DocumentStatus in (" + dict["DocStat"] + ") and a.Status in (1, 98) order by a.CreateTime desc";
                }
                else
                {
                    query8 = strQry +
                   " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") + "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") + "'" +
                   " and a.EmployeeID='" + dict["EmpId"] + "'" +
                   " and a.Purpose='" + dict["Purpose"] + "'" +
                  " and a.DocumentStatus = " + dict["DocStat"] + " and a.Status = 1 order by a.CreateTime desc";
                }
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "";
                switch (opt)
                {
                    case 1:
                        query = query1; // No Filter
                        break;
                    case 2:
                        query = query2; // EmpID
                        break;
                    case 3:
                        query = query3; //Purpose
                        break;
                    case 4:
                        query = query4; //Document Status
                        break;
                    case 5:
                        query = query5; //EmpID & Purpose
                        break;
                    case 6:
                        query = query6; //EmpID & Doc Stat
                        break;
                    case 7:
                        query = query7; // Purpose & DocStat
                        break;
                    case 8:
                        query = query8; // EmpID & Purpose & DocStat
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
                    mh = new movementhistory();
                    mh.rowid = reader.GetInt32(0);
                    mh.date = reader.GetDateTime(1);
                    mh.empid = reader.GetString(2);
                    mh.empname = reader.GetString(3);
                    mh.purpose = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                    {
                        mh.exittime = reader.GetDateTime(5);
                    }else
                    {
                        mh.exittime = null;
                    }

                    if (!reader.IsDBNull(6))
                    {
                        mh.returntime = reader.GetDateTime(6);
                    }
                    else
                    {
                        mh.returntime = null;
                    }
                    if (!reader.IsDBNull(7))
                    {
                        mh.actexittime = reader.GetDateTime(7);
                    }
                    else
                    {
                        mh.actexittime = null;
                    }
                    if (!reader.IsDBNull(8))
                    {
                        mh.actreturntime = reader.GetDateTime(8);
                    }
                    else
                    {
                        mh.actreturntime = null;
                    }
                    mh.documentstatus = reader.GetInt32(9);
                    mh.status = reader.GetInt32(10);
                    ////////mh.exittime = !reader.IsDBNull(5) ? reader.GetDateTime(5);
                    ////////mh.exittime = null;
                    ////////mh.returntime = reader.GetDateTime(6);
                    ////////mh.actexittime = reader.GetDateTime(7);
                    ////////mh.actreturntime = reader.GetDateTime(8);
                    movementHistory.Add(mh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return movementHistory;
        }



        public List<movementhistory> getMovementHistoryForAll(DateTime from, DateTime to,String empId, string purpose)
        {
            movementhistory mh;
            List<movementhistory> movementHistory = new List<movementhistory>();
            try
            {
                //EmpId & Purpose
                string query1 = strQry +
                 " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") + "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") + "'" +
                  " and a.EmployeeID='" + empId + "' and a.Purpose = '" + purpose + "'"+
                " and a.DocumentStatus in (1,2,3,4,99,98,10,6) and a.Status in (1,98) order by a.CreateTime desc";
                //EmpID only
                string query2 = strQry +
                 " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") + "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") + "'" +
                  " and a.EmployeeID='" + empId + "'"+
                " and a.DocumentStatus in (1,2,3,4,99,98,10,6) and a.Status in (1,98) order by a.CreateTime desc";
                //Purpose Only
                string query3 = strQry +
                 " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") + "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") + "'" +
                  " and a.Purpose = '" + purpose + "'" +
                " and a.DocumentStatus in (1,2,3,4,99,98,10,6) and a.Status in (1,98) order by a.CreateTime desc";
                //Only all
                string query4 = strQry +
                     " CAST(a.CreateTime as DATE) <= '" + to.ToString("yyyy-MM-dd") + "' and CAST(a.CreateTime as DATE)>='" + from.ToString("yyyy-MM-dd") + "'" +
                     " and a.DocumentStatus in (1,2,3,4,99,98,10,6) and a.Status in (1,98) order by a.CreateTime desc";

                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "";
                if (empId.Length != 0 && purpose.Length != 0)
                    query = query1;
                else if(empId.Length != 0)
                    query = query2;
                else if(purpose.Length != 0)
                    query = query3;
                else
                    query = query4;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mh = new movementhistory();
                    mh.rowid = reader.GetInt32(0);
                    mh.date = reader.GetDateTime(1);
                    mh.empid = reader.GetString(2);
                    mh.empname = reader.GetString(3);
                    mh.purpose = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                    {
                        mh.exittime = reader.GetDateTime(5);
                    }
                    else
                    {
                        mh.exittime = null;
                    }

                    if (!reader.IsDBNull(6))
                    {
                        mh.returntime = reader.GetDateTime(6);
                    }
                    else
                    {
                        mh.returntime = null;
                    }
                    if (!reader.IsDBNull(7))
                    {
                        mh.actexittime = reader.GetDateTime(7);
                    }
                    else
                    {
                        mh.actexittime = null;
                    }
                    if (!reader.IsDBNull(8))
                    {
                        mh.actreturntime = reader.GetDateTime(8);
                    }
                    else
                    {
                        mh.actreturntime = null;
                    }
                    mh.documentstatus = reader.GetInt32(9);
                    mh.status = reader.GetInt32(10);
                    movementHistory.Add(mh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return movementHistory;
        }
    }
}

