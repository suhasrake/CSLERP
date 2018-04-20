using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class dashboardalarm
    {
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public int ActivityType { get; set; }
        public string Details { get; set; }
        public int AckStatus { get; set; }
        public string FromUser { get; set; }
        public string FromUserName { get; set; }
        public string ToUser { get; set; }
        public string ToUserName { get; set; }
    }

    class DashboardDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<dashboardalarm> getDashboardAlarms(string uid)
        {
            dashboardalarm dba;
            List<dashboardalarm> dbas = new List<dashboardalarm>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select top 50 DocumentID, DocumentName,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate," +
                    "ActivityType,Details,AckStatus,FromUser,FromUserName from ViewDashboardAlarm " +
                    "where ToUser='" + uid + "' order by Temporarydate desc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dba = new dashboardalarm();
                    dba.DocumentID = reader.GetString(0);
                    dba.DocumentName = reader.GetString(1);
                    dba.TemporaryNo = reader.GetInt32(2);
                    dba.TemporaryDate = reader.GetDateTime(3);
                    dba.DocumentNo = reader.GetInt32(4);
                    dba.DocumentDate = reader.GetDateTime(5);
                    dba.ActivityType = reader.GetInt32(6);
                    dba.Details = reader.GetString(7);
                    dba.AckStatus = reader.GetInt32(8);
                    dba.FromUser = reader.GetString(9);
                    dba.FromUserName = reader.GetString(10);
                    dbas.Add(dba);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            return dbas;

        }
 
        public Boolean updateDashboardAlarm(dashboardalarm dba, string userId)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update DashboardAlarm set AckStatus=" + dba.AckStatus + 
                    " where DocumentID='" + dba.DocumentID + "'" +
                    " and TemporaryNo=" + dba.TemporaryNo +
                      " and ToUser='" + userId + "'" +
                    " and TemporaryDate='" + dba.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "DashboardAlarm", "", updateSQL) +
                    Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean DeleteDashboardAlarm(dashboardalarm dba, string userId)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "delete DashboardAlarm " + 
                    " where DocumentID='" + dba.DocumentID + "'" +
                    " and TemporaryNo=" + dba.TemporaryNo +
                     " and ToUser='" + userId + "'"+
                    " and TemporaryDate='" + dba.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "DashboardAlarm", "", updateSQL) +
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
        public Boolean insertDashboardAlarm(dashboardalarm dba)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into DashboardAlarm (DocumentID,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate,"+
                    "ActivityType,Details,FromUser,ToUser)" +
                    "values (" +
                    "'" + dba.DocumentID + "'," +
                    dba.TemporaryNo+","+
                    "'" + dba.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    dba.DocumentNo + "," +
                    "'" + dba.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                    dba.ActivityType + "," +
                    "'" + dba.Details + "'," +
                    "'" + Login.userLoggedIn + "'," +
                    "'" + dba.ToUser + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "DashboardAlarm", "", updateSQL) +
                    Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
    }
}

