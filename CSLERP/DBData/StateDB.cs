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
    class state
    {
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }

    class StateDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public  List<state> getStateList()
        {
            state stat;
            List<state> stateList = new List<state>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select StateCode, StateName,Status,CreateTime,CreateUser "+
                    "from State order by StateCode";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stat = new state();
                    stat.StateCode = reader.GetString(0);
                    stat.StateName = reader.GetString(1);
                    stat.Status = reader.GetInt32(2);
                    stat.CreateTime = reader.GetDateTime(3);
                    stat.CreateUser = reader.GetString(4);
                    
                    stateList.Add(stat);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying State Data");
            
            }
            return stateList;
            
        }
 
        public Boolean updateState(state stat )
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update State set StateName='"+ stat.StateName + 
                    "',Status="+ stat.Status+
                    " where StateCode='" + stat.StateCode + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("update", "State", "", updateSQL) +
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
        public Boolean insertState(state stat)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into State (StateCode,StateName,Status,CreateTime,CreateUser)" +
                    " values (" +
                    "'" + stat.StateCode + "'," +
                     "'" + stat.StateName + "'," +
                    stat.Status+","+
                    "GETDATE()"+","+
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("insert", "State", "", updateSQL) +
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
        public Boolean validateState(state stat)
        {
            Boolean status = true;
            try
            {
                if (stat.StateCode.Trim().Length == 0 || stat.StateCode == null)
                {
                    return false;
                }
                if (stat.StateName.Trim().Length == 0 || stat.StateName == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {

            }
            return status;
        }
        public static void fillStateComboNew(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                StateDB sdb = new StateDB();
                List<state> stList = sdb.getStateList();
                foreach (state stat in stList)
                {
                    if (stat.Status == 1)
                    {
                        Structures.ComboBoxItem cbitem =
                            new Structures.ComboBoxItem(stat.StateName, stat.StateCode);
                        cmb.Items.Add(cbitem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
       
    }
}
