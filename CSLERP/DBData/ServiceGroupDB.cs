using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{

    class servicegroup
    {
        public int RowID { get; set; }
        public int GroupLevel { get; set; }
        public string GroupCode { get; set; }
        public string GroupDescription { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }
    class ServiceGroupDB
    {
        public List<servicegroup> getServiceGroupDetails(int lvl)
        {
            servicegroup sgroup;
            List<servicegroup> GroupList = new List<servicegroup>();
            try
            {
                string query = "select GroupLevel,GroupCode, GroupDescription, CreateTime, CreateUser " +
                    " from ServiceGroup where GroupLevel = " + lvl + " order by GroupDescription asc";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sgroup = new servicegroup();
                    sgroup.GroupLevel = reader.GetInt32(0); 
                    sgroup.GroupCode = reader.GetString(1);
                    sgroup.GroupDescription = reader.GetString(2);
                    sgroup.CreateTime = reader.GetDateTime(3);
                    sgroup.CreateUser = reader.GetString(4);

                    GroupList.Add(sgroup);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Service Group Details");
            }
            return GroupList;
        }
        public Boolean insertServiceGroup(servicegroup sgroup)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                 string updateSQL = "insert into ServiceGroup " +
                    "(Grouplevel,GroupCode,GroupDescription,CreateTime,CreateUser) " +
                    "values ('" + sgroup.GroupLevel + "','" 
                    + sgroup.GroupCode + "','" + 
                    sgroup.GroupDescription + "'," +
                    "GETDATE()" + ",'" +
                    Login.userLoggedIn + "')"; 

                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "ServiceGroup", "", updateSQL) +
                    Main.QueryDelimiter;
                
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        public Boolean updateServiceGroup(servicegroup sgroup)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ServiceGroup set " +
                    "GroupDescription = " + "'" + sgroup.GroupDescription + "'" +
                   " where GroupLevel = " + sgroup.GroupLevel + " and GroupCode = '" + sgroup.GroupCode + "'";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ServiceGroup", "", updateSQL) +
                Main.QueryDelimiter;

                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        public Boolean validateServiceGroup(servicegroup sg)
        {
            Boolean status = true;
            if((sg.GroupCode.Trim().Length == 0) || (sg.GroupCode == null))
            {
                status = false;
            }
            if((sg.GroupDescription.Trim().Length == 0) || ( sg.GroupDescription == null))
            {
                status = false;
            }
            return status;
        }
        public static void fillGroupValueCombo(System.Windows.Forms.ComboBox cmb, int level)
        {
            cmb.Items.Clear();
            try
            {
                ServiceGroupDB sgdb = new ServiceGroupDB();
                List < servicegroup > ValueList = sgdb.getServiceGroupDetails(level);
                foreach (servicegroup sg in ValueList)
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
