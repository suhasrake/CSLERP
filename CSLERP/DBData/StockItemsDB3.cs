using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class stockitem3
    {
        public int rowID { get; set; }
        public string StockItemID { get; set; }
        public string Name { get; set; }
        public string Group1Code { get; set; }
        public string Group1CodeDescription { get; set; }
        public string Group2Code { get; set; }
        public string Group2CodeDescription { get; set; }
        public string Group3Code { get; set; }
        public string Group3CodeDescription { get; set; }
        public string Unit { get; set; }
        public string UnitDescription { get; set; }
        public int ReorderLevel { get; set; }
        public int status { get; set; }
        public int documentStatus { get; set; }
        public DateTime Createime { get; set; }
        public string CreateUser { get; set; } // user id
        public string ForwardUser { get; set; } //user id
        public string ApproveUser { get; set; } //user id
        public string CreateUserName { get; set; } // user name
        public string ForwardUserName { get; set; } //user name
        public string ApproveUserName { get; set; } //user name
        public string ForwarderList { get; set; }
        public string HSNCode { get; set; }
    }

    class StockItemDB3
    {
        ActivityLogDB alDB = new ActivityLogDB();


        public List<stockitem3> getFilteredStockItems(string userList, int opt)
        {
            stockitem3 sitem;
            List<stockitem3> StockItems = new List<stockitem3>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query1 = "select StockItemID, Name, ReorderLevel,Level1Gcode,Level1GDescription, " +
                    " Level2Gcode,Level2GDescription,Level3Gcode,Level3GDescription,Unit,UnitDescription,   " +
                    " Status,DocumentStatus,isnull(CreateUser,' '),isnull(ForwardUser,' '),isnull(ApproveUser,' '), " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList,HSNCode " +
                    " from ViewStockItem  " +
                    " where ((forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1))" +
                    " order by StockItemID";

                string query2 = "select StockItemID, Name, ReorderLevel,Level1Gcode,Level1GDescription, " +
                    " Level2Gcode,Level2GDescription,Level3Gcode,Level3GDescription,Unit,UnitDescription,   " +
                    " Status,DocumentStatus,isnull(CreateUser,' '),isnull(ForwardUser,' '),isnull(ApproveUser,' '), " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList,HSNCode " +
                    " from ViewStockItem " +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "'))" +
                    " order by StockItemID";

                string query3 = "select StockItemID, Name, ReorderLevel,Level1Gcode,Level1GDescription, " +
                    " Level2Gcode,Level2GDescription,Level3Gcode,Level3GDescription,Unit,UnitDescription,   " +
                    " Status,DocumentStatus,isnull(CreateUser,' '),isnull(ForwardUser,' '),isnull(ApproveUser,' '), " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList,HSNCode " +
                    " from ViewStockItem " +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99) " +
                    " order by StockItemID";
                string query6 = "select StockItemID, Name, ReorderLevel,Level1Gcode,Level1GDescription, " +
                    " Level2Gcode,Level2GDescription,Level3Gcode,Level3GDescription,Unit,UnitDescription,   " +
                    " Status,DocumentStatus,isnull(CreateUser,' '),isnull(ForwardUser,' '),isnull(ApproveUser,' '), " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList,HSNCode " +
                    " from ViewStockItem " +
                    " where DocumentStatus = 99" +
                    " order by Level1Gcode,Level2Gcode,Level3Gcode";
                string query = "";
                //string query = "select StockItemID, Name, ReorderLevel,Category,CategoryDescription, "+
                //    " [Group], GroupDescription,Type,TypeDescription,Unit,UnitDescription, "+
                //    " Status,DocumentStatus,isnull(CreateUser,' '),isnull(ForwardUser,' '),isnull(ApproveUser,' ') " +
                //    " CreatorName,ForwarderName,ApproverName "+
                //    " from ViewStockItems where " +
                //    " Status=0 "+
                //    " and ((forwardUser in ("+userList+") and documentstatus between 2 and 98) "+ 
                //    " or (createuser='"+Login.userLoggedIn+ "' and DocumentStatus=1))"+
                //    " order by StockItemID";
                switch (opt)
                {
                    case 1:
                        query = query1;
                        break;
                    case 2:
                        query = query2;
                        break;
                    case 3:
                        query = query3;
                        break;
                    case 6:
                        query = query6;
                        break;
                    default:
                        query = "";
                        break;
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sitem = new stockitem3();
                    try
                    {
                        sitem.StockItemID = reader.GetString(0);
                        sitem.Name = reader.IsDBNull(1) ? "" : reader.GetString(1);
                        sitem.ReorderLevel = reader.GetInt32(2);
                        sitem.Group1Code = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        if (!reader.IsDBNull(4))
                        {
                            sitem.Group1CodeDescription = reader.GetString(4);
                        }
                        else
                        {
                            sitem.Group1CodeDescription = "";
                        }
                        sitem.Group2Code = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        if (!reader.IsDBNull(6))
                        {
                            sitem.Group2CodeDescription = reader.GetString(6);
                        }
                        else
                        {
                            sitem.Group2CodeDescription = "";
                        }

                        sitem.Group3Code = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        if (!reader.IsDBNull(8))
                        {
                            sitem.Group3CodeDescription = reader.GetString(8);
                        }
                        else
                        {
                            sitem.Group3CodeDescription = "";
                        }

                        sitem.Unit = reader.IsDBNull(9)?"":reader.GetString(9);
                        sitem.UnitDescription = reader.IsDBNull(10) ? "" : reader.GetString(10);
                        sitem.status = reader.GetInt32(11);
                        sitem.documentStatus = reader.GetInt32(12);
                        sitem.CreateUser = reader.GetString(13);
                        sitem.ForwardUser = reader.IsDBNull(14) ? "" : reader.GetString(14);
                        sitem.ApproveUser = reader.IsDBNull(15) ? "" : reader.GetString(15);
                        sitem.CreateUserName = reader.IsDBNull(16) ? "" : reader.GetString(16);
                        sitem.ForwardUserName = reader.IsDBNull(17) ? "" : reader.GetString(17);
                        sitem.ApproveUserName = reader.IsDBNull(18) ? "" : reader.GetString(18);
                        sitem.HSNCode = reader.IsDBNull(20)?"": reader.GetString(20);
                        if (!reader.IsDBNull(19))
                        {
                            sitem.ForwarderList = reader.GetString(19);
                        }
                        else
                        {
                            sitem.ForwarderList = "";
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    StockItems.Add(sitem);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Stock Item Data");
            }
            return StockItems;

        }
        public List<stockitem3> getStockItems()
        {
            stockitem3 sitem;
            List<stockitem3> StockItems = new List<stockitem3>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select StockItemID, Name, ReorderLevel,Level1Gcode,Level1GDescription, " +
                    " Level2Gcode,Level2GDescription,Level3Gcode,Level3GDescription,Unit,UnitDescription, " +
                    " Status,DocumentStatus,isnull(CreateUser,' '),isnull(ForwardUser,' '),isnull(ApproveUser,' '), " +
                    " isnull(CreatorName,' '),isnull(ForwarderName,' '),isnull(ApproverName,' ') " +
                    " from ViewStockItem " +
                    " order by StockItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sitem = new stockitem3();
                    try
                    {
                        sitem.StockItemID = reader.GetString(0);
                        sitem.Name = reader.GetString(1);
                        sitem.ReorderLevel = reader.GetInt32(2);
                        sitem.Group1CodeDescription = reader.GetString(3);
                        //sitem.CategoryDescription = reader.GetString(4);
                        sitem.Group2CodeDescription = reader.GetString(5);
                        //sitem.GroupDescription = reader.GetString(6);
                        sitem.Group3CodeDescription = reader.GetString(7);
                        //sitem.TypeDescription = reader.GetString(8);
                        sitem.Unit = reader.GetString(9);
                        sitem.UnitDescription = reader.GetString(10);

                        sitem.status = reader.GetInt32(11);
                        sitem.documentStatus = reader.GetInt32(12);
                        sitem.CreateUser = reader.GetString(13);
                        sitem.ForwardUser = reader.GetString(14);
                        sitem.ApproveUser = reader.GetString(15);
                        sitem.CreateUserName = reader.GetString(16);
                        sitem.ForwardUserName = reader.GetString(17);
                        sitem.ApproveUserName = reader.GetString(18);
                    }
                    catch (Exception)
                    {
                    }
                    StockItems.Add(sitem);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Stock Item Data");
            }
            return StockItems;

        }
        public Boolean updateStockItem(stockitem3 sitem, stockitem3 prevsitem)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockItem set " +
                    "Name='" + sitem.Name +
                    "', Level1GCode='" + sitem.Group1Code +
                    "', Level2GCode='" + sitem.Group2Code +
                    "', Level3GCode='" + sitem.Group3Code +
                    "', Unit='" + sitem.Unit +
                    "', ReorderLevel=" + sitem.ReorderLevel +
                    ", Status=" + sitem.status +
                    ", ForwarderList='" + sitem.ForwarderList + "'" +
                     ", HSNCode='" + sitem.HSNCode + "'" +
                    " where StockItemID='" + prevsitem.StockItemID + "'";
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
        public Boolean insertStockItem(stockitem3 sitem)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into StockItem " +
                    " (StockItemID,Name,Reorderlevel,Level1GCode,Level2GCode,Level3GCode,Unit,Status,DocumentStatus,CreateTime,CreateUser,ForwarderList,HSNCode)" +
                    "values (" +
                    "IDENT_CURRENT('StockItem')" +
                    ",'" + sitem.Name + "'," +
                    sitem.ReorderLevel + "," +
                    "'" + sitem.Group1Code + "'," +
                    "'" + sitem.Group2Code + "'," +
                    "'" + sitem.Group3Code + "'," +
                    "'" + sitem.Unit + "'," +
                    sitem.status + "," +
                    sitem.documentStatus + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" +
                    ",'" + sitem.ForwarderList + "','" + sitem.HSNCode + "')";
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
        public Boolean validateStockItem(stockitem3 sitem)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                //if (sitem.StockItemID.Trim().Length == 0 || sitem.StockItemID == null)
                //{
                //    return false;
                //}
                if (sitem.HSNCode.Trim().Length == 0 || sitem.HSNCode == null)
                {
                    return false;
                }
                if (sitem.Name.Trim().Length == 0 || sitem.Name == null)
                {
                    return false;
                }
                if (sitem.Group1CodeDescription.Trim().Length == 0 || sitem.Group1CodeDescription == null)
                {
                    return false;
                }
                if (sitem.Group2CodeDescription.Trim().Length == 0 || sitem.Group2CodeDescription == null)
                {
                    return false;
                }
                if (sitem.Group3CodeDescription.Trim().Length == 0 || sitem.Group3CodeDescription == null)
                {
                    return false;
                }
                if (sitem.Unit.Trim().Length == 0 || sitem.Unit == null)
                {
                    return false;
                }
                if (sitem.ReorderLevel == 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return status;
        }
        public Boolean forwardStockItem(stockitem3 prevsitem)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockItem set documentstatus=" + (prevsitem.documentStatus + 1) +
                     ", forwardUser='" + prevsitem.ForwardUser + "'" +
                    ", ForwarderList='" + prevsitem.ForwarderList + "'" +
                    " where StockItemID='" + prevsitem.StockItemID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("forward", "StockItem", "", updateSQL) +
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
        public Boolean ApproveStockItem(stockitem3 prevsitem, string id)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockItem set StockItemID = '" + id +
                    "', documentstatus=99" +
                    ", status=1" +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    " where  StockItemID='" + prevsitem.StockItemID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "StockItem", "", updateSQL) +
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
        public Boolean reverseStockITem(stockitem3 sitem)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockItem set DocumentStatus=" + sitem.documentStatus +
                    ", forwardUser='" + sitem.ForwardUser + "'" +
                    ", ForwarderList='" + sitem.ForwarderList + "'" +
                    " where  StockItemID='" + sitem.StockItemID + "'";
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
        public static void fillStockItemCombo(System.Windows.Forms.ComboBox cmb, string CategoryName)
        {
            cmb.Items.Clear();
            try
            {
                StockItemDB3 stockitemdb = new StockItemDB3();
                List<stockitem3> StockItems = stockitemdb.getStockItems();
                foreach (stockitem3 si in StockItems)
                {
                    //if (CategoryName.Length > 0 && CategoryName != si.Category)
                    //{
                    //    continue;
                    //}
                    if (si.status == 1)
                    {
                        cmb.Items.Add(si.StockItemID + "-" + si.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
        }
        public static void fillStockItemGridViewCombo(DataGridViewComboBoxCell cmb, string CategoryName)
        {
            cmb.Items.Clear();
            try
            {
                StockItemDB3 stockitemdb = new StockItemDB3();
                List<stockitem3> StockItems = stockitemdb.getStockItems();
                foreach (stockitem3 si in StockItems)
                {
                    //if (CategoryName.Length > 0 && CategoryName != si.Category)
                    //{
                    //    continue;
                    //}
                    if (si.status == 1)
                    {
                        cmb.Items.Add(si.StockItemID + "-" + si.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
        }
        public static ListView ProductCodeSelectionView()
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
                StockItemDB3 sidb = new StockItemDB3();
                List<stockitem3> stockitems = sidb.getFilteredStockItems("", 6);
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Item Id", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Item Name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Cat", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Group", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Type", -2, HorizontalAlignment.Center);
                foreach (stockitem3 si in stockitems)
                {

                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(si.StockItemID);
                    item1.SubItems.Add(si.Name.ToString());
                    item1.SubItems.Add(si.Group1CodeDescription.ToString());
                    item1.SubItems.Add(si.Group2CodeDescription.ToString());
                    item1.SubItems.Add(si.Group3CodeDescription.ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }

        public static TreeView getStockItemTreeView()
        {
            TreeView tv = new TreeView();
            try
            {
                tv.CheckBoxes = true;
                //tv.Location = new System.Drawing.Point(264, 90);
                //tv.Size = new System.Drawing.Size(332, 161);
                tv.LabelEdit = true;
                tv.ShowLines = true;
                tv.CheckBoxes = true;

                //tv.CheckBoxes = "leaf";
                // tv.sho
                tv.FullRowSelect = true;
                StockItemDB3 sidb = new StockItemDB3();
                List<stockitem3> stockitems = sidb.getFilteredStockItems("", 6);
                TreeNode tNode;
                string RootDesc = "";
                string SubRootDesc = "";
                string SubSubRootDesc = "";
                int j = 0;
                int k = 0;
                int l = 0;
                foreach (stockitem3 item in stockitems)
                {
                    l++;
                    int n = tv.Nodes.Count;
                    if (item.Group1CodeDescription != RootDesc)
                    {
                        j = 0; k = 0;

                        tNode = tv.Nodes.Add(item.Group1CodeDescription);
                        tv.Nodes[n].Nodes.Add(item.Group2CodeDescription);
                        tv.Nodes[n].Nodes[j].Nodes.Add(item.Group3CodeDescription);
                        tv.Nodes[n].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID + "-" + item.Name);
                        RootDesc = item.Group1CodeDescription;
                        SubRootDesc = item.Group2CodeDescription;
                        SubSubRootDesc = item.Group3CodeDescription;
                    }
                    else
                    {
                        if (item.Group2CodeDescription != SubRootDesc)
                        {
                            j = j + 1; k = 0;
                            tv.Nodes[n - 1].Nodes.Add(item.Group2CodeDescription);
                            tv.Nodes[n - 1].Nodes[j].Nodes.Add(item.Group3CodeDescription);
                            tv.Nodes[n - 1].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID + "-" + item.Name);
                            SubRootDesc = item.Group2CodeDescription;
                            SubSubRootDesc = item.Group3CodeDescription;
                        }
                        else
                        {
                            if (item.Group3CodeDescription != SubSubRootDesc)
                            {
                                k = k + 1;
                                tv.Nodes[n - 1].Nodes[j].Nodes.Add(item.Group3CodeDescription);
                                tv.Nodes[n - 1].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID + "-" + item.Name);
                                SubSubRootDesc = item.Group3CodeDescription;
                            }
                            else
                            {
                                tv.Nodes[n - 1].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID + "-" + item.Name);
                            }
                        }
                    }
                }
                // MessageBox.Show("Count:" + l);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in TreeView");
            }
            return tv;
        }
        public static string getUnitForSelectedStockItem(string ItemID, string ItemName)
        {
            stockitem3 sitem = new stockitem3();

            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select StockItemID, Name,Unit" +
                    " from ViewStockItem where StockItemID = '" + ItemID + "' and Name = '" + ItemName + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                try
                {
                    if (reader.Read())
                    {
                        sitem.StockItemID = reader.GetString(0);
                        sitem.Name = reader.GetString(1);
                        sitem.Unit = reader.GetString(2);
                    }
                }
                catch (Exception ex)
                {
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Item Data");
            }
            return sitem.Unit;
        }
        public static ListView getStockItemListView()
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
                StockItemDB3 sidb = new StockItemDB3();
                List<stockitem3> SIList = sidb.getFilteredStockItems("", 6);
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItem ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItem Name", -2, HorizontalAlignment.Left);

                foreach (stockitem3 si in SIList)
                {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        item1.SubItems.Add(si.StockItemID.ToString());
                        item1.SubItems.Add(si.Name);
                        lv.Items.Add(item1);
                }


            }
            catch (Exception)
            {

            }
            return lv;
        }
    }
}
