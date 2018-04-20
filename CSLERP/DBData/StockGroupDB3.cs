using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{

    class stockgroup3
    {
        public int RowID { get; set; }
        public int GroupLevel { get; set; }
        public string GroupCode { get; set; }
        public string GroupDescription { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }
    class StockGroupDB3
    {
        public List<stockgroup3> getStockGroupDetails(int lvl)
        {
            stockgroup3 sgroup;
            List<stockgroup3> stockGroupList = new List<stockgroup3>();
            try
            {
                string query = "select GroupLevel,GroupCode, GroupDescription, CreateTime, CreateUser " +
                    " from StockGroup where GroupLevel = " + lvl + " order by GroupDescription asc";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sgroup = new stockgroup3();
                    sgroup.GroupLevel = reader.GetInt32(0); 
                    sgroup.GroupCode = reader.GetString(1);
                    sgroup.GroupDescription = reader.GetString(2);
                    sgroup.CreateTime = reader.GetDateTime(3);
                    sgroup.CreateUser = reader.GetString(4); 

                    stockGroupList.Add(sgroup);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Terms and Conditions Details");
            }
            return stockGroupList;
        }
        public Boolean insertStockGroup(stockgroup3 sgroup)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                 string updateSQL = "insert into StockGroup " +
                    "(Grouplevel,GroupCode,GroupDescription,CreateTime,CreateUser) " +
                    "values ('" + sgroup.GroupLevel + "','" 
                    + sgroup.GroupCode + "','" + 
                    sgroup.GroupDescription + "'," +
                    "GETDATE()" + ",'" +
                    Login.userLoggedIn + "')"; 

                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "StockGroup", "", updateSQL) +
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
        public Boolean updateStockGroup(stockgroup3 sgroup)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockGroup set " +
                    "GroupDescription = " + "'" + sgroup.GroupDescription + "'" +
                   " where GroupLevel = " + sgroup.GroupLevel + " and GroupCode = '" + sgroup.GroupCode + "'";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "StockGroup", "", updateSQL) +
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
        public Boolean validateStockGroup(stockgroup3 sg)
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
                StockGroupDB3 sgdb = new StockGroupDB3();
                List<stockgroup3> ValueList = sgdb.getStockGroupDetails(level);
                foreach (stockgroup3 sg in ValueList)
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
