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
    class serviceHSNMapping
    {
        public int RowID { get; set; }
        public string ServiceItemID { get; set; }
        public string ServiceItemName { get; set; }
        public string HSNCode { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }

    class ServiceHSNMappingDB
    {
        public  List<serviceHSNMapping> getServiceHSNMappingList()
        {
            serviceHSNMapping map;
            List<serviceHSNMapping> mapList = new List<serviceHSNMapping>();
            SqlConnection conn = new SqlConnection(Login.connString);
            try
            {
                
                string query = "select a.ServiceItemID, b.Description,a.HSNCode,"+
                    "a.Status,a.CreateTime,a.CreateUser,a.RowID " +
                   "from ServiceHSNMapping a left outer join "+
                    "CatalogueValue b on a.ServiceItemID = b.CatalogueValueID " +
                    " and b.CatalogueID = 'ServiceLookup' order by a.ServiceItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    map = new serviceHSNMapping();
                    map.ServiceItemID = reader.GetString(0);
                    map.ServiceItemName = reader.GetString(1);
                    map.HSNCode = reader.GetString(2);
                    map.Status = reader.GetInt32(3);
                    map.CreateTime = reader.GetDateTime(4);
                    map.CreateUser = reader.GetString(5);
                    map.RowID = reader.GetInt32(6);
                    mapList.Add(map);
                }             
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying  Service HSNMApping Data");
            }
            finally
            {
                conn.Close();
            }
            return mapList;
            
        }
 
        public Boolean updateHSNCode(serviceHSNMapping map )
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ServiceHSNMapping set HSNCode='" + map.HSNCode +
                    "',Status=" + map.Status +
                    " where RowID=" + map.RowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("update", "ServiceHSNMapping", "", updateSQL) +
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
        public Boolean insertHSNCOde(serviceHSNMapping map)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into ServiceHSNMapping (ServiceItemID,HSNCode,Status,CreateTime,CreateUser)" +
                    " values (" +
                    "'" + map.ServiceItemID + "'," +
                     "'" + map.HSNCode + "'," +
                    map.Status+","+
                    "GETDATE()"+","+
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("insert", "ServiceHSNMapping", "", updateSQL) +
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
        public Boolean validateHSNMapping(serviceHSNMapping map)
        {
            Boolean status = true;
            try
            {
                if (map.ServiceItemID.Trim().Length == 0 || map.ServiceItemID == null)
                {
                    return false;
                }
                if (map.HSNCode.Trim().Length == 0 || map.HSNCode == null || Convert.ToInt32(map.HSNCode) == 0)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return status;
        }
        public static string getHSNCode(string serviceItemID)
        {
            string code = "";
            SqlConnection conn = new SqlConnection(Login.connString);
            try
            {
                string query = "select HSNCode from ServiceHSNMapping" +
                    " where ServiceItemID = '" + serviceItemID + "' and status = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    code = reader.GetString(0);
                } 
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying HSNMApping Data");
            }
            finally
            {
                conn.Close();
            }
            return code;

        }
        //public static void fillHSNCodeMappingComboNew(System.Windows.Forms.ComboBox cmb)
        //{
        //    cmb.Items.Clear();
        //    try
        //    {
        //        ProductHSNMappingDB sdb = new ProductHSNMappingDB();
        //        List<HSNMapping> MapList = sdb.getHSNMappingList();
        //        foreach (HSNMapping map in MapList)
        //        {
        //            if (map.Status == 1)
        //            {
        //                //Structures.ComboBoxItem cbitem =
        //                //    new Structures.ComboBoxItem(map.StateName, map.StateCode);
        //                //cmb.Items.Add(cbitem);
        //                cmb.Items.Add(map.StockItemID+"-"+map.HSNCode);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        //    }
        //}

    }
}
