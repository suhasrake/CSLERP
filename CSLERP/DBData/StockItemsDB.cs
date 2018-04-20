using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class stockitem
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
        public string Group4Code { get; set; }
        public string Group4CodeDescription { get; set; }
        public string Group5Code { get; set; }
        public string Group5CodeDescription { get; set; }
        public string Group6Code { get; set; }
        public string Group6CodeDescription { get; set; }
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
    }
    class StockItemSelection
    {
        public string StockItemID { get; set; }
        public string Name { get; set; }
        public string Group1CodeDesc { get; set; }
        public string Group2CodeDesc { get; set; }
        public string Group3CodeDesc { get; set; }
    }
    class StockItemDB
    {
        ActivityLogDB alDB = new ActivityLogDB();


        public List<stockitem> getFilteredStockItems(string userList, int opt)
        {
            stockitem sitem;
            List<stockitem> StockItems = new List<stockitem>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query1 = "select StockItemID, Name, ReorderLevel,Level1GCode,Level1GDescription, " +
                    " Level2GCode, Level2GDescription,Level3GCode,Level3GDescription,"+
                    "Level4GCode, Level4GDescription,Level5GCode,Level5GDescription,Level6GCode,Level6GDescription," +
                    "Unit,UnitDescription, " +
                    " Status,DocumentStatus,isnull(CreateUser,' '),isnull(ForwardUser,' '),isnull(ApproveUser,' '), " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList " +
                    " from ViewStockItem  " +
                    " where ((forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1))" +
                    " order by StockItemID";

                string query2 = "select StockItemID, Name, ReorderLevel,Level1GCode,Level1GDescription, " +
                    " Level2GCode, Level2GDescription,Level3GCode,Level3GDescription," +
                    "Level4GCode, Level4GDescription,Level5GCode,Level5GDescription,Level6GCode,Level6GDescription," +
                    "Unit,UnitDescription, " +
                    " Status,DocumentStatus,isnull(CreateUser,' '),isnull(ForwardUser,' '),isnull(ApproveUser,' '), " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList " +
                    " from ViewStockItem  " +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "'))" +
                    " order by StockItemID";

                string query3 = "select StockItemID, Name, ReorderLevel,Level1GCode,Level1GDescription, " +
                    " Level2GCode, Level2GDescription,Level3GCode,Level3GDescription," +
                    "Level4GCode, Level4GDescription,Level5GCode,Level5GDescription,Level6GCode,Level6GDescription," +
                    "Unit,UnitDescription, " +
                    " Status,DocumentStatus,isnull(CreateUser,' '),isnull(ForwardUser,' '),isnull(ApproveUser,' '), " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList " +
                    " from ViewStockItem  " +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99) " +
                    " order by StockItemID";
                string query6 = "select StockItemID, Name, ReorderLevel,Level1GCode,Level1GDescription, " +
                    " Level2GCode, Level2GDescription,Level3GCode,Level3GDescription," +
                    "Level4GCode, Level4GDescription,Level5GCode,Level5GDescription,Level6GCode,Level6GDescription," +
                    "Unit,UnitDescription, " +
                    " Status,DocumentStatus,isnull(CreateUser,' '),isnull(ForwardUser,' '),isnull(ApproveUser,' '), " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList " +
                    " from ViewStockItem  " +
                    " where DocumentStatus = 99" +
                    " order by Level1GCode,Level2GCode,Level3GCode";
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
                    sitem = new stockitem();
                    try
                    {
                        sitem.StockItemID = reader.GetString(0);
                        sitem.Name = reader.GetString(1);
                        sitem.ReorderLevel = reader.GetInt32(2);
                        sitem.Group1Code = reader.GetString(3);
                        sitem.Group1CodeDescription = reader.IsDBNull(4)?"": reader.GetString(4);

                        sitem.Group2Code = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        sitem.Group2CodeDescription = reader.IsDBNull(6)?"":reader.GetString(6);

                        sitem.Group3Code = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        sitem.Group3CodeDescription = reader.IsDBNull(8)?"":reader.GetString(8);
                       
                        sitem.Group4Code = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        sitem.Group4CodeDescription = reader.IsDBNull(10)?"":reader.GetString(10);
                       
                        sitem.Group5Code = reader.IsDBNull(11) ? "" : reader.GetString(11);
                        sitem.Group5CodeDescription = reader.IsDBNull(12)?"":reader.GetString(12);
                       
                        sitem.Group6Code = reader.IsDBNull(13) ? "" : reader.GetString(13);
                        sitem.Group6CodeDescription = reader.IsDBNull(14)?"":reader.GetString(14);
                      
                        sitem.Unit = reader.IsDBNull(15) ? "" : reader.GetString(15);
                        sitem.UnitDescription = reader.IsDBNull(16) ? "" : reader.GetString(16);
                        sitem.status = reader.GetInt32(17);
                        sitem.documentStatus = reader.GetInt32(18);
                        sitem.CreateUser = reader.IsDBNull(19) ? "" : reader.GetString(19);
                        sitem.ForwardUser = reader.IsDBNull(20) ? "" : reader.GetString(20);
                        sitem.ApproveUser = reader.IsDBNull(21) ? "" : reader.GetString(21);
                        sitem.CreateUserName = reader.IsDBNull(22) ? "" : reader.GetString(22);
                        sitem.ForwardUserName = reader.IsDBNull(23) ? "" : reader.GetString(23);
                        sitem.ApproveUserName = reader.IsDBNull(24) ? "" : reader.GetString(24);
                        sitem.ForwarderList = reader.IsDBNull(25) ? "" : reader.GetString(25);
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
        public List<stockitem> getStockItems()
        {
            stockitem sitem;
            List<stockitem> StockItems = new List<stockitem>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select StockItemID, Name, ReorderLevel,"+
                    "Level1GCode,Level1GDescription, Level2GCode, Level2GDescription,Level3GCode,Level3GDescription," +
                     "Level4GCode, Level4GDescription,Level5GCode,Level5GDescription,Level6GCode,Level6GDescription," +
                    " Unit,UnitDescription, " +
                    " Status,DocumentStatus,isnull(CreateUser,' '),isnull(ForwardUser,' '),isnull(ApproveUser,' '), " +
                    " isnull(CreatorName,' '),isnull(ForwarderName,' '),isnull(ApproverName,' ') " +
                    " from ViewStockItem " +
                    " order by StockItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sitem = new stockitem();
                    try
                    {
                        sitem.StockItemID = reader.GetString(0);
                        sitem.Name = reader.GetString(1);
                        sitem.ReorderLevel = reader.GetInt32(2);
                        sitem.Group1CodeDescription = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        //sitem.CategoryDescription = reader.GetString(4);
                        sitem.Group2CodeDescription = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        //sitem.GroupDescription = reader.GetString(6);
                        sitem.Group3CodeDescription = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        //sitem.TypeDescription = reader.GetString(8);
                        sitem.Group4CodeDescription = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        //sitem.CategoryDescription = reader.GetString(4);
                        sitem.Group5CodeDescription = reader.IsDBNull(11) ? "" : reader.GetString(11);
                        //sitem.GroupDescription = reader.GetString(6);
                        sitem.Group6CodeDescription = reader.IsDBNull(13) ? "" : reader.GetString(13);
                        //sitem.TypeDescription = reader.GetString(8);
                        sitem.Unit = reader.IsDBNull(15) ? "" : reader.GetString(15);
                        sitem.UnitDescription = reader.IsDBNull(16) ? "" : reader.GetString(16);

                        sitem.status = reader.GetInt32(17);
                        sitem.documentStatus = reader.GetInt32(18);
                        sitem.CreateUser = reader.GetString(19);
                        sitem.ForwardUser = reader.GetString(20);
                        sitem.ApproveUser = reader.GetString(21);
                        sitem.CreateUserName = reader.GetString(22);
                        sitem.ForwardUserName = reader.GetString(23);
                        sitem.ApproveUserName = reader.GetString(24);
                    }
                    catch (Exception)
                    {
                        //MessageBox.Show("Exception retriving StockItem");
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
        public Boolean updateStockItem(stockitem sitem, stockitem prevsitem)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockItem set " +
                    "Name='" + sitem.Name +
                    "', Level1Gcode='" + sitem.Group1Code +
                    "', Level2Gcode='" + sitem.Group2Code +
                    "', Level3Gcode='" + sitem.Group3Code +
                     "', Level4Gcode='" + sitem.Group4Code +
                    "', Level5Gcode='" + sitem.Group5Code +
                    "', Level6Gcode='" + sitem.Group6Code +
                    "', Unit='" + sitem.Unit +
                    "', ReorderLevel=" + sitem.ReorderLevel +
                    ", ForwarderList='" + sitem.ForwarderList + "'" +
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
        public Boolean insertStockItem(stockitem sitem)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into StockItem " +
                    " (StockItemID,Name,Reorderlevel,Level1GCode,Level2GCode,Level3GCode,"+
                    "Level4GCode,Level5GCode,Level6GCode,Unit,Status,DocumentStatus,CreateTime,CreateUser,ForwarderList)" +
                    "values (" +
                    "IDENT_CURRENT('StockItem')" +
                    ",'" + sitem.Name + "'," +
                    sitem.ReorderLevel + "," +
                    "'" + sitem.Group1Code + "'," +
                    "'" + sitem.Group2Code + "'," +
                    "'" + sitem.Group3Code + "'," +
                       "'" + sitem.Group4Code + "'," +
                    "'" + sitem.Group5Code + "'," +
                    "'" + sitem.Group6Code + "'," +
                    "'" + sitem.Unit + "'," +
                    sitem.status + "," +
                    sitem.documentStatus + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" +
                    ",'" + sitem.ForwarderList + "')";
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
        public Boolean validateStockItem(stockitem sitem)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                if (sitem.StockItemID.Trim().Length == 0 || sitem.StockItemID == null)
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
                //if (sitem.Group2CodeDescription.Trim().Length == 0 || sitem.Group2CodeDescription == null)
                //{
                //    return false;
                //}
                //if (sitem.Group3CodeDescription.Trim().Length == 0 || sitem.Group3CodeDescription == null)
                //{
                //    return false;
                //}
                if (sitem.Unit.Trim().Length == 0 || sitem.Unit == null)
                {
                    return false;
                }
                if (sitem.ReorderLevel == 0)
                {
                    return false;
                }
            }
            catch (Exception)
            {

            }
            return status;
        }
        public Boolean forwardStockItem(stockitem prevsitem)
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
        public Boolean ApproveStockItem(stockitem prevsitem, string id)
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
        public Boolean reverseStockITem(stockitem sitem)
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
                StockItemDB stockitemdb = new StockItemDB();
                List<stockitem> StockItems = stockitemdb.getStockItems();
                foreach (stockitem si in StockItems)
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
        public static void fillStockItemGridViewComboNew(DataGridViewComboBoxCell cmb, string CategoryName)
        {
            cmb.Items.Clear();
            try
            {
                StockItemDB stockitemdb = new StockItemDB();
                List<stockitem> StockItems = stockitemdb.getStockItems();
                //foreach (stockitem si in StockItems)
                //{
                //    //if (CategoryName.Length > 0 && CategoryName != si.Category)
                //    //{
                //    //    continue;
                //    //}
                //    if (si.status == 1)
                //    {
                //        cmb.Items.Add(si.StockItemID + "-" + si.Name);
                //    }
                //}
                foreach (stockitem si in StockItems)
                {
                    if (si.status == 1)
                    {
                        Structures.GridViewComboBoxItem ch =
                            new Structures.GridViewComboBoxItem(si.Name, si.StockItemID);
                        cmb.Items.Add(ch);
                    }
                }
                cmb.DisplayMember = "Name";  // Name Property will show(Editing)
                cmb.ValueMember = "Value";  // Value Property will save(Saving)
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
                StockItemDB stockitemdb = new StockItemDB();
                List<stockitem> StockItems = stockitemdb.getStockItems();
                foreach (stockitem si in StockItems)
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
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
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
                StockItemDB sidb = new StockItemDB();
                List<stockitem> stockitems = sidb.getFilteredStockItems("", 6);
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Item Id", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Item Name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Group1", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Group2", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Group3", -2, HorizontalAlignment.Center);
                foreach (stockitem si in stockitems)
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
                StockItemDB sidb = new StockItemDB();
                List<stockitem> stockitems = sidb.getFilteredStockItems("", 6).Where(si => si.status == 1 && si.documentStatus == 99).ToList();
                TreeNode tNode;
                string RootDesc = "";
                string SubRootDesc = "";
                string SubSubRootDesc = "";
                int j = 0;
                int k = 0;
                int l = 0;
                foreach (stockitem item in stockitems)
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
                                try
                                {
                                    tv.Nodes[n - 1].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID + "-" + item.Name);
                                }
                                catch (Exception)
                                {
                                }
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
            stockitem sitem = new stockitem();

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
                StockItemDB sidb = new StockItemDB();
                List<stockitem> SIList = sidb.getFilteredStockItems("", 6);
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItem ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItem Name", -2, HorizontalAlignment.Left);

                foreach (stockitem si in SIList)
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
        public static TreeView getStockItemTreeViewNew()
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
                //tv.Sort();
                //tv.CheckBoxes = "leaf";
                // tv.sho
                tv.FullRowSelect = true;
                StockItemDB sidb = new StockItemDB();
                List<stockitem> stockitems = sidb.getFilteredStockItems("", 6).Where(si => si.status == 1 && si.documentStatus == 99).ToList();
                TreeNode tNode;
                string RootDesc = "";
                string SubRootDesc = "";
                string SubSubRootDesc = "";
                int j = 0;
                int k = 0;
                int l = 0;
                foreach (stockitem item in stockitems)
                {
                    l++;
                    int n = tv.Nodes.Count;
                    if (item.Group1CodeDescription != RootDesc)
                    {
                        j = 0; k = 0;

                        tNode = tv.Nodes.Add(item.Group1CodeDescription);
                        tv.Nodes[n].Nodes.Add(item.Group2CodeDescription);
                        tv.Nodes[n].Nodes[j].Nodes.Add(item.Group3CodeDescription);
                        //tv.Nodes[n].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID + "-" + item.Name);
                        tv.Nodes[n].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID, item.Name);
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
                            //tv.Nodes[n - 1].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID + "-" + item.Name);
                            tv.Nodes[n - 1].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID, item.Name);
                            SubRootDesc = item.Group2CodeDescription;
                            SubSubRootDesc = item.Group3CodeDescription;
                        }
                        else
                        {
                            if (item.Group3CodeDescription != SubSubRootDesc)
                            {
                                k = k + 1;
                                tv.Nodes[n - 1].Nodes[j].Nodes.Add(item.Group3CodeDescription);
                                //tv.Nodes[n - 1].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID + "-" + item.Name);
                                tv.Nodes[n - 1].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID, item.Name);
                                SubSubRootDesc = item.Group3CodeDescription;
                            }
                            else
                            {
                                //tv.Nodes[n - 1].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID + "-" + item.Name);
                                tv.Nodes[n - 1].Nodes[j].Nodes[k].Nodes.Add(item.StockItemID, item.Name);
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

        //Gridview For StockItem Dtails
        public List<StockItemSelection> getStockItemsForGridView()
        {
            StockItemSelection sitem;
            List<StockItemSelection> StockItems = new List<StockItemSelection>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select StockItemID, Name," +
                    "Level1GDescription, Level2GDescription,Level3GDescription" +
                    " from ViewStockItem where Status = 1 and DocumentStatus = 99" +
                    " order by Name";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sitem = new StockItemSelection();
                    try
                    {
                        sitem.StockItemID = reader.GetString(0);
                        sitem.Name = reader.GetString(1);
                        sitem.Group1CodeDesc = reader.IsDBNull(2)?"":reader.GetString(2);
                        sitem.Group2CodeDesc = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        sitem.Group3CodeDesc = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Exception retriving StockItem");
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
        public DataGridView getStockItemlistGrid()
        {
            DataGridView StockItemGgrid = new DataGridView();
            try
            {
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                StockItemGgrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                StockItemGgrid.EnableHeadersVisualStyles = false;
                
                StockItemGgrid.AllowUserToAddRows = false;
                StockItemGgrid.AllowUserToDeleteRows = false;
                StockItemGgrid.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                StockItemGgrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
                StockItemGgrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                StockItemGgrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                StockItemGgrid.ColumnHeadersHeight = 27;
                StockItemGgrid.RowHeadersVisible = false;
                StockItemGgrid.Columns.Add(colChk);

                
                StockItemDB SIDB = new StockItemDB();
                List<StockItemSelection> StockSElList = SIDB.getStockItemsForGridView();

                StockItemGgrid.DataSource = StockSElList;
            }
            catch (Exception ex)
            {
            }

            return StockItemGgrid;
        }
    }
}
