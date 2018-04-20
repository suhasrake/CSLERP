using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{

    class accountgroup
    {
        public int RowID { get; set; }
        public int GroupLevel { get; set; }
        public string GroupCode { get; set; }
        public string GroupDescription { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }
    class AccountGroupDB
    {
        public List<accountgroup> getAccountGroupDetails(int lvl)
        {
            accountgroup agroup;
            List<accountgroup> accountGroupList = new List<accountgroup>();
            try
            {
                string query = "select GroupLevel,GroupCode, GroupDescription, CreateTime, CreateUser " +
                    " from AccountGroup where GroupLevel = " + lvl;
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    agroup = new accountgroup();
                    agroup.GroupLevel = reader.GetInt32(0);
                    agroup.GroupCode = reader.GetString(1);
                    agroup.GroupDescription = reader.GetString(2);
                    agroup.CreateTime = reader.GetDateTime(3);
                    agroup.CreateUser = reader.GetString(4);
                    accountGroupList.Add(agroup);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return accountGroupList;
        }
        public Boolean insertAccountGroup(accountgroup agroup)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                 string updateSQL = "insert into AccountGroup " +
                    "(Grouplevel,GroupCode,GroupDescription,CreateTime,CreateUser) " +
                    "values ('" + agroup.GroupLevel + "','" 
                    + agroup.GroupCode + "','" + 
                    agroup.GroupDescription + "'," +
                    "GETDATE()" + ",'" +
                    Login.userLoggedIn + "')"; 

                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "AccountGroup", "", updateSQL) +
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
        public Boolean updateCustomerGroup(accountgroup agroup)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update AccountGroup set " +
                    "GroupDescription = " + "'" + agroup.GroupDescription + "'" +
                   " where GroupLevel = " + agroup.GroupLevel + " and GroupCode = '" + agroup.GroupCode + "'";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "AccountGroup", "", updateSQL) +
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
        public Boolean validateCustomerGroup(accountgroup sg)
        {
            Boolean status = true;
            try
            {
                if ((sg.GroupCode.Trim().Length == 0) || (sg.GroupCode == null))
                {
                    status = false;
                }
                if ((sg.GroupDescription.Trim().Length == 0) || (sg.GroupDescription == null))
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
        public static void fillAccountGroupValueCombo(System.Windows.Forms.ComboBox cmb, int level)
        {
            cmb.Items.Clear();
            try
            {
                AccountGroupDB sgdb = new AccountGroupDB();
                List<accountgroup> ValueList = sgdb.getAccountGroupDetails(level);
                foreach (accountgroup sg in ValueList)
                {
                    cmb.Items.Add(sg.GroupCode + "-" + sg.GroupDescription);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }

        }
    }
}
