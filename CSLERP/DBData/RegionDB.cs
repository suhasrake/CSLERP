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
    class region
    {
        public string regionID { get; set; }
        public string name { get; set; }
        public int status { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
    }

    class RegionDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public  List<region> getRegions()
        {
            region reg;
            List<region> Regions = new List<region>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RegionID, Name,status "+
                    "from Region order by RegionID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    reg = new region();
                    reg.regionID = reader.GetString(0);
                    reg.name = reader.GetString(1);
                    reg.status = reader.GetInt32(2);
                    Regions.Add(reg);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Region Data");
            
            }
            return Regions;
            
        }
 
        public Boolean updateRegion(region reg )
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update Region set name='"+reg.name + 
                    "',Status="+reg.status+
                    " where RegionID='" + reg.regionID+"'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("update", "Region", "", updateSQL) +
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
        public Boolean insertRegion(region reg)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into Region (RegionID,Name,Status,CreateTime,CreateUser)"+
                    "values (" +
                    "'" + reg.regionID + "'," +
                     "'" + reg.name + "'," +
                    reg.status+","+
                    "GETDATE()"+","+
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("insert", "Region", "", updateSQL) +
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
        public Boolean validateRegion(region reg)
        {
            Boolean status = true;
            try
            {
                if (reg.regionID.Trim().Length == 0 || reg.regionID == null)
                {
                    return false;
                }
                if (reg.name.Trim().Length == 0 || reg.name == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {

            }
            return status;
        }
        public static void fillRegionCombo(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                RegionDB dbrecord = new RegionDB();
                List<region> Regions = dbrecord.getRegions();
                foreach (region reg in Regions)
                {
                    if (reg.status == 1)
                    {
                        cmb.Items.Add(reg.regionID + "-" + reg.name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
        }
        public static void fillRegionComboNew(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                RegionDB dbrecord = new RegionDB();
                List<region> Regions = dbrecord.getRegions();
                foreach (region reg in Regions)
                {
                    if (reg.status == 1)
                    {
                        Structures.ComboBoxItem cbitem =
                            new Structures.ComboBoxItem(reg.name, reg.regionID);
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
