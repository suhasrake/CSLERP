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
    class HSNMapping
    {
        public int RowID { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string HSNCode { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }

    class ProductHSNMappingDB
    {
        public  List<HSNMapping> getHSNMappingList()
        {
            HSNMapping map;
            List<HSNMapping> mapList = new List<HSNMapping>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.StockItemID, b.Name,a.ModelNo,c.ModelName,a.HSNCode,"+
                    "a.Status,a.CreateTime,a.CreateUser,a.RowID " +
                   "from ProductHSNMapping a left outer join "+
                    "StockItem b on a.StockItemID = b.StockItemID left outer join "+
                    "ProductModel c on a.StockItemID = c.StockItemID AND a.ModelNo = c.ModelNo "+
                    "order by a.StockItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    map = new HSNMapping();
                    map.StockItemID = reader.GetString(0);
                    map.StockItemName = reader.GetString(1);
                    map.ModelNo = reader.IsDBNull(2) ? "NA":reader.GetString(2);
                    map.ModelName = reader.IsDBNull(3) ? "NA":reader.GetString(3);
                    map.HSNCode = reader.GetString(4);
                    map.Status = reader.GetInt32(5);
                    map.CreateTime = reader.GetDateTime(6);
                    map.CreateUser = reader.GetString(7);
                    map.RowID = reader.GetInt32(8);
                    mapList.Add(map);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying HSNMApping Data");
            
            }
            return mapList;
            
        }
 
        public Boolean updateHSNCode(HSNMapping map )
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductHSNMapping set HSNCode='" + map.HSNCode +
                    "',Status=" + map.Status +
                    " where RowID=" + map.RowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("update", "ProductHSNMapping", "", updateSQL) +
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
        public Boolean insertHSNCOde(HSNMapping map)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into ProductHSNMapping (StockItemID,ModelNo,HSNCode,Status,CreateTime,CreateUser)" +
                    " values (" +
                    "'" + map.StockItemID + "'," +
                     "'" + map.ModelNo + "'," +
                     "'" + map.HSNCode + "'," +
                    map.Status+","+
                    "GETDATE()"+","+
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductHSNMapping", "", updateSQL) +
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
        public Boolean validateHSNMapping(HSNMapping map)
        {
            Boolean status = true;
            try
            {
                if (map.StockItemID.Trim().Length == 0 || map.StockItemID == null)
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

            }
            return status;
        }
        public static string getHSNCode(string StockID,string modelNo)
        {
            string code = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select HSNCode from ProductHSNMapping" +
                    " where StockItemID = '" + StockID +"'"+
                   " and ModelNo = '" + modelNo +"' and status = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    code = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying HSNMApping Data");

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
