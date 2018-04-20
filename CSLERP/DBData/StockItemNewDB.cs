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
    class stockitemnew
    {
        public string StockItemID { get; set; }
        public string Name { get; set; }
        public string Group1Code { get; set; }
        public string Group1CodeDescription { get; set; }
        public string Unit { get; set; }
        public int Status { get; set; }
        public int DocumentStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }

    class StockItemNewDB
    {
        public  List<stockitemnew> getStockItems()
        {
            stockitemnew si;
            List<stockitemnew> siList = new List<stockitemnew>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.StockItemID, a.Name,a.Level1Gcode,b.GroupDescription,a.Unit,a.Status,a.DocumentStatus from StockItem a, StockGroup b" +
                    " where a.Level1Gcode = b.GroupCode and b.GroupLevel = 1 and StockItemID like '__0000000000____' order by StockItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    si = new stockitemnew();
                    si.StockItemID = reader.GetString(0);
                    si.Name = reader.GetString(1);
                    si.Group1Code = reader.GetString(2);
                    si.Group1CodeDescription = reader.GetString(3);
                    si.Unit = reader.IsDBNull(4)?"":reader.GetString(4);
                    si.Status = reader.GetInt32(5);
                    si.DocumentStatus = reader.GetInt32(6);
                    siList.Add(si);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying StockNew Data");
            
            }
            return siList;
            
        }
 
        public Boolean updateStockItem(stockitemnew si )
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockItem set name='"+ si.Name +
                    "',Unit='" + si.Unit +
                    "',Status=" + si.Status+
                    " where StockItemID='" + si.StockItemID+"'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("update", "StockItem", "", updateSQL) +
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
        public Boolean insertStockItem(stockitemnew si)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into StockItem (StockItemID,Name,Level1GCode,Unit,Status,DocumentStatus,CreateTime,CreateUser)"+
                    "values (" +
                    "'" + si.StockItemID + "'," +
                     "'" + si.Name + "'," +
                    "'" + si.Group1Code + "'," +
                    "'" + si.Unit + "'," +
                    si.Status+","+
                     si.DocumentStatus + "," +
                    "GETDATE()" +","+
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("insert", "StockItem", "", updateSQL) +
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
        public Boolean validateRegion(stockitemnew si)
        {
            Boolean status = true;
            try
            {
                //if (si.StockItemID.Trim().Length == 0 || si.StockItemID == null)
                //{
                //    return false;
                //}
                if (si.Name.Trim().Length == 0 || si.Name == null)
                {
                    return false;
                }
                if (si.Unit.Trim().Length == 0 || si.Unit == null)
                {
                    return false;
                }
                if (si.Group1Code.Trim().Length == 0 || si.Group1Code == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {

            }
            return status;
        }
        //public static void fillRegionCombo(System.Windows.Forms.ComboBox cmb)
        //{
        //    cmb.Items.Clear();
        //    try
        //    {
        //        RegionDB dbrecord = new RegionDB();
        //        List<region> Regions = dbrecord.getRegions();
        //        foreach (region reg in Regions)
        //        {
        //            if (reg.status == 1)
        //            {
        //                cmb.Items.Add(reg.regionID + "-" + reg.name);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
        //    }
        //}
        //public static void fillRegionComboNew(System.Windows.Forms.ComboBox cmb)
        //{
        //    cmb.Items.Clear();
        //    try
        //    {
        //        RegionDB dbrecord = new RegionDB();
        //        List<region> Regions = dbrecord.getRegions();
        //        foreach (region reg in Regions)
        //        {
        //            if (reg.status == 1)
        //            {
        //                Structures.ComboBoxItem cbitem =
        //                    new Structures.ComboBoxItem(reg.name, reg.regionID);
        //                cmb.Items.Add(cbitem);
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
