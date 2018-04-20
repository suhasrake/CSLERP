using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{

    class customergroup
    {
        public int RowID { get; set; }
        public int GroupLevel { get; set; }
        public string GroupCode { get; set; }
        public string GroupDescription { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }
    class CustomerGroupDB
    {
        public List<customergroup> getCustomerGroupDetails(int lvl)
        {
            customergroup cgroup;
            List<customergroup> customerGroupList = new List<customergroup>();
            try
            {
                string query = "select GroupLevel,GroupCode, GroupDescription, CreateTime, CreateUser " +
                    " from CustomerGroup where GroupLevel = " + lvl;
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cgroup = new customergroup();
                    cgroup.GroupLevel = reader.GetInt32(0);
                    cgroup.GroupCode = reader.GetString(1);
                    cgroup.GroupDescription = reader.GetString(2);
                    cgroup.CreateTime = reader.GetDateTime(3);
                    cgroup.CreateUser = reader.GetString(4);

                    customerGroupList.Add(cgroup);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return customerGroupList;
        }
        public Boolean insertCustomerGroup(customergroup cgroup)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                 string updateSQL = "insert into CustomerGroup " +
                    "(Grouplevel,GroupCode,GroupDescription,CreateTime,CreateUser) " +
                    "values ('" + cgroup.GroupLevel + "','" 
                    + cgroup.GroupCode + "','" + 
                    cgroup.GroupDescription + "'," +
                    "GETDATE()" + ",'" +
                    Login.userLoggedIn + "')"; 

                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "CustomerGroup", "", updateSQL) +
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
        public Boolean updateCustomerGroup(customergroup sgroup)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update CustomerGroup set " +
                    "GroupDescription = " + "'" + sgroup.GroupDescription + "'" +
                   " where GroupLevel = " + sgroup.GroupLevel + " and GroupCode = '" + sgroup.GroupCode + "'";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "CustomerGroup", "", updateSQL) +
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
        public Boolean validateCustomerGroup(customergroup sg)
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
        public static void fillCustomerGroupValueCombo(System.Windows.Forms.ComboBox cmb, int level)
        {
            cmb.Items.Clear();
            try
            {
                CustomerGroupDB sgdb = new CustomerGroupDB();
                List<customergroup> ValueList = sgdb.getCustomerGroupDetails(level);
                foreach (customergroup sg in ValueList)
                {
                    //cmb.Items.Add(sg.GroupCode + "-" + sg.GroupDescription);
                    Structures.ComboBoxItem cbitem =
                            new Structures.ComboBoxItem(sg.GroupDescription, sg.GroupCode);
                    cmb.Items.Add(cbitem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }

        }
    }
}
