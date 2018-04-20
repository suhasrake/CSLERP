using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{

    class stockgroup
    {
        public int RowID { get; set; }
        public int GroupLevel { get; set; }
        public string GroupCode { get; set; }
        public string GroupDescription { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }
    class StockGroupDB
    {
        public List<stockgroup> getStockGroupDetails(int lvl)
        {
            stockgroup sgroup;
            List<stockgroup> stockGroupList = new List<stockgroup>();
            try
            {
                string query = "select GroupLevel,GroupCode, GroupDescription, CreateTime, CreateUser " +
                    " from StockGroup where GroupLevel = " + lvl;
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sgroup = new stockgroup();
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
        public Boolean insertStockGroup(stockgroup sgroup)
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
        public Boolean updateStockGroup(stockgroup sgroup)
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
        public Boolean validateStockGroup(stockgroup sg)
        {
            Boolean status = true;
            if ((sg.GroupCode.Trim().Length == 0) || (sg.GroupCode == null))
            {
                status = false;
            }
            if ((sg.GroupDescription.Trim().Length == 0) || (sg.GroupDescription == null))
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
                StockGroupDB sgdb = new StockGroupDB();
                List<stockgroup> ValueList = sgdb.getStockGroupDetails(level);
                foreach (stockgroup sg in ValueList)
                {
                    cmb.Items.Add(sg.GroupCode + "-" + sg.GroupDescription);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }

        }

        //Group Mapping
        public static ListView getListViewForStockGroup(int lvl)
        {
            ListView lv = new ListView();
            try
            {
                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Group Code", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Group Name", -2, HorizontalAlignment.Left);
                StockGroupDB sdb = new StockGroupDB();
                List<stockgroup> groups = sdb.getStockGroupDetails(lvl);
                foreach (stockgroup sg in groups)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = false;
                    item.SubItems.Add(sg.GroupCode.ToString());
                    item.SubItems.Add(sg.GroupDescription);
                    lv.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
            }
            return lv;
        }
        public Boolean updateProductGroupMapping(string children, int grplvl, string gcode)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockGroup set " +
                    "Children = " + "'" + children + "'" +
                   " where GroupLevel = " + grplvl + " and GroupCode = '" + gcode + "'";

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
        public static string getChildrenOfAGroup(string gcode, int grplvl)
        {
            string ch = "";
            try
            {

                string query = "select Children " +
                    " from StockGroup" +
                " where GroupLevel = " + grplvl + " and GroupCode = '" + gcode + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ch = reader.IsDBNull(0) ? "" : reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying StockGroup Details");
            }
            return ch;
        }

        public static ListView getLVOfChildrenStockGroup(int lvl, string gCode)
        {
            ListView lv = new ListView();
            try
            {
                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Group Code", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Group Name", -2, HorizontalAlignment.Left);
                StockGroupDB sdb = new StockGroupDB();
                List<stockgroup> groups = sdb.getStockGroupDetails(lvl);
                string ChildList = getChildrenOfAGroup(gCode, lvl-1);
                if (ChildList.Length != 0)
                {
                    foreach (stockgroup sg in groups)
                    {

                        if (ChildList.Contains(sg.GroupCode))
                        {
                            ListViewItem item = new ListViewItem();
                            item.Checked = false;
                            item.SubItems.Add(sg.GroupCode.ToString());
                            item.SubItems.Add(sg.GroupDescription);
                            lv.Items.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (stockgroup sg in groups)
                    {
                        ListViewItem item = new ListViewItem();
                        item.Checked = false;
                        item.SubItems.Add(sg.GroupCode.ToString());
                        item.SubItems.Add(sg.GroupDescription);
                        lv.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return lv;
        }
    }
}
