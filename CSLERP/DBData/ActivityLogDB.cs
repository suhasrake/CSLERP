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
    class activitylog
    {
        public string UserID { get; set; }
        public DateTime ActivityDate { get; set; }
        public string ActivityID { get; set; }
        public string TableName { get; set; }
        public string Activity { get; set; }
        public string DBString { get; set; }

    }

    class ActivityLogDB
    {
        ////////public Boolean insertActivityLog(string aid, string tname, string activity, string dbstring)
        ////////{
        ////////    Boolean status = true;
        ////////    try
        ////////    {
        ////////        dbstring = prepareDBString(dbstring);
        ////////        string updateSQL = "insert into ActivityLog (userID,ActivityDate,ActivityID,TableName,Activity,DBString)" +
        ////////            "values (" +
        ////////            "'" + Login.userLoggedIn + "'," +
        ////////            "GETDATE()," +
        ////////            "'" + aid + "'," +
        ////////            "'" + tname + "'," +
        ////////            "'" + activity + "','" +
        ////////            dbstring + "')";
        ////////        SqlConnection conn = new SqlConnection(Login.connString);
        ////////        SqlCommand cmd = new SqlCommand(updateSQL, conn);
        ////////        conn.Open();
        ////////        cmd.ExecuteNonQuery();
        ////////    }
        ////////    catch (Exception)
        ////////    {
        ////////        status = false;
        ////////    }
        ////////    return status;
        ////////}

        public static string PrepareActivityLogQquerString(string aid, string tname, string activity, string dbstring)
        {
            string updateSQL = "";
            try
            {
                dbstring = prepareDBString(dbstring);
                 updateSQL = "insert into ActivityLog (userID,ActivityDate,ActivityID,TableName,Activity,DBString)" +
                    "values (" +
                    "'" + Login.userLoggedIn + "'," +
                    "GETDATE()," +
                    "'" + aid + "'," +
                    "'" + tname + "'," +
                    "'" + activity + "','" +
                    dbstring + "')";
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                updateSQL = "";
            }
            return updateSQL;
        }

        private static string prepareDBString(string dbstring)
        {
            string newStrig = "";
            int count = 0;
            try
            {
                string[] strArr = dbstring.Trim().Split('\'');
                for (count = 0; count <= strArr.Length - 1; count++)
                {
                    if (strArr[count].Trim().Length>0)
                    {
                        newStrig = newStrig + strArr[count] + "''";
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return newStrig;
        }
        public static Boolean updateActivity(string activity)
        {
            Boolean status = true;
            try
            {
                string utString =
                           ActivityLogDB.PrepareActivityLogQquerString("", "", activity, "") +
                           Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return status;
        }
    }
}
