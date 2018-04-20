using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class serviceitem
    {
        public int rowID { get; set; }
        public string ServiceItemID { get; set; }
        public string Name { get; set; }
        public string Group1Code { get; set; }
        public string Group1CodeDescription { get; set; }
        public string Group2Code { get; set; }
        public string Group2CodeDescription { get; set; }
        public string Group3Code { get; set; }
        public string Group3CodeDescription { get; set; }
        //public string Unit { get; set; }
        //public string UnitDescription { get; set; }
        //public int ReorderLevel { get; set; }
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

    class ServiceItemsDB
    {
        ActivityLogDB alDB = new ActivityLogDB();


        public List<serviceitem> getFilteredServiceItems(string userList, int opt)
        {
            serviceitem sitem;
            List<serviceitem> ServiceItems = new List<serviceitem>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query1 = "select ServiceItemID, Name,Level1Gcode,Level1GDescription, " +
                    " Level2Gcode,Level2GDescription,Level3Gcode,Level3GDescription," +
                    " Status,DocumentStatus,isnull(CreateUser,' ') CreateUser,isnull(ForwardUser,' ') ForwardUser,isnull(ApproveUser,' ') ApproveUser, " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList,HSNCode " +
                    " from ViewServiceItem  " +
                    " where ((forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1))" +
                    " order by ServiceItemID";

                string query2 = "select ServiceItemID, Name,Level1Gcode,Level1GDescription, " +
                    " Level2Gcode,Level2GDescription,Level3Gcode,Level3GDescription," +
                    " Status,DocumentStatus,isnull(CreateUser,' ') CreateUser,isnull(ForwardUser,' ') ForwardUser,isnull(ApproveUser,' ') ApproveUser, " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList,HSNCode " +
                    " from ViewServiceItem " +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "'))" +
                    " order by ServiceItemID";

                string query3 = "select ServiceItemID, Name,Level1Gcode,Level1GDescription, " +
                    " Level2Gcode,Level2GDescription,Level3Gcode,Level3GDescription," +
                    " Status,DocumentStatus,isnull(CreateUser,' ') CreateUser,isnull(ForwardUser,' ') ForwardUser,isnull(ApproveUser,' ') ApproveUser, " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList,HSNCode " +
                    " from ViewServiceItem " +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99) " +
                    " order by ServiceItemID";
                string query6 = "select ServiceItemID, Name,Level1Gcode,Level1GDescription, " +
                    " Level2Gcode,Level2GDescription,Level3Gcode,Level3GDescription," +
                    " Status,DocumentStatus,isnull(CreateUser,' ') CreateUser,isnull(ForwardUser,' ') ForwardUser,isnull(ApproveUser,' ') ApproveUser, " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList,HSNCode " +
                    " from ViewServiceItem " +
                    " where DocumentStatus = 99" +
                    " order by Level1Gcode,Level2Gcode,Level3Gcode";
                string query = "";
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
                    sitem = new serviceitem();
                    try
                    {
                        sitem.ServiceItemID = reader.GetString(0);
                        sitem.Name = reader.GetString(1);
                        sitem.Group1Code = reader.GetString(2);
                        if (!reader.IsDBNull(3))
                        {
                            sitem.Group1CodeDescription = reader.GetString(3);
                        }
                        else
                        {
                            sitem.Group1CodeDescription = "";
                        }
                        sitem.Group2Code = reader.GetString(4);
                        if (!reader.IsDBNull(5))
                        {
                            sitem.Group2CodeDescription = reader.GetString(5);
                        }
                        else
                        {
                            sitem.Group2CodeDescription = "";
                        }

                        sitem.Group3Code = reader.GetString(6);
                        if (!reader.IsDBNull(7))
                        {
                            sitem.Group3CodeDescription = reader.GetString(7);
                        }
                        else
                        {
                            sitem.Group3CodeDescription = "";
                        }

                        sitem.status = reader.GetInt32(8);
                        sitem.documentStatus = reader.GetInt32(9);
                        sitem.CreateUser = reader.GetString(10);
                        sitem.ForwardUser = reader.GetString(11);
                        sitem.ApproveUser = reader.GetString(12);
                        sitem.CreateUserName = reader.GetString(13);
                        sitem.ForwardUserName = reader.GetString(14);
                        sitem.ApproveUserName = reader.GetString(15);
                        sitem.HSNCode = reader.IsDBNull(17)?"":reader.GetString(17);
                        if (!reader.IsDBNull(16))
                        {
                            sitem.ForwarderList = reader.GetString(16);
                        }
                        else
                        {
                            sitem.ForwarderList = "";
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    ServiceItems.Add(sitem);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Service Item Data");
            }
            return ServiceItems;

        }
        public List<serviceitem> getServiceItems()
        {
            serviceitem sitem;
            List<serviceitem> ServiceItems = new List<serviceitem>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select ServiceItemID, Name,Level1Gcode,Level1GDescription, " +
                    " Level2Gcode,Level2GDescription,Level3Gcode,Level3GDescription, " +
                    " Status,DocumentStatus,isnull(CreateUser,' '),isnull(ForwardUser,' '),isnull(ApproveUser,' '), " +
                    " isnull(CreatorName,' '),isnull(ForwarderName,' '),isnull(ApproverName,' ') " +
                    " from ViewServiceItem " +
                    " order by ServiceItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sitem = new serviceitem();
                    try
                    {
                        sitem.ServiceItemID = reader.GetString(0);
                        sitem.Name = reader.GetString(1);
                        sitem.Group1CodeDescription = reader.GetString(2);
                        //sitem.CategoryDescription = reader.GetString(4);
                        sitem.Group2CodeDescription = reader.GetString(3);
                        //sitem.GroupDescription = reader.GetString(6);
                        sitem.Group3CodeDescription = reader.GetString(4);
                        //sitem.TypeDescription = reader.GetString(8);

                        sitem.status = reader.GetInt32(5);
                        sitem.documentStatus = reader.GetInt32(6);
                        sitem.CreateUser = reader.GetString(7);
                        sitem.ForwardUser = reader.GetString(8);
                        sitem.ApproveUser = reader.GetString(9);
                        sitem.CreateUserName = reader.GetString(10);
                        sitem.ForwardUserName = reader.GetString(11);
                        sitem.ApproveUserName = reader.GetString(12);
                    }
                    catch (Exception)
                    {
                    }
                    ServiceItems.Add(sitem);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Service Item Data");
            }
            return ServiceItems;

        }
        public Boolean updateServiceItem(serviceitem sitem, serviceitem prevsitem)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ServiceItem set " +
                    "Name='" + sitem.Name +
                    "', Level1GCode='" + sitem.Group1Code +
                    "', Level2GCode='" + sitem.Group2Code +
                    "', Level3GCode='" + sitem.Group3Code +
                    "', Status=" + sitem.status +
                    ", ForwarderList='" + sitem.ForwarderList + "'" +
                    ", HSNCode='" + sitem.HSNCode + "'" +
                    " where ServiceItemID='" + prevsitem.ServiceItemID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ServiceItem", "", updateSQL) +
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
        public Boolean insertServiceItem(serviceitem sitem)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into ServiceItem " +
                    " (ServiceItemID,Name,Level1GCode,Level2GCode,Level3GCode,Status,DocumentStatus,CreateTime,CreateUser,ForwarderList,HSNCode)" +
                    "values (" +
                    "IDENT_CURRENT('ServiceItem')" +
                    ",'" + sitem.Name + "'," +
                    "'" + sitem.Group1Code + "'," +
                    "'" + sitem.Group2Code + "'," +
                    "'" + sitem.Group3Code + "'," +
                    sitem.status + "," +
                    sitem.documentStatus + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" +
                    ",'" + sitem.ForwarderList + "','"+sitem.HSNCode+"')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "ServiceItem", "", updateSQL) +
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
        public Boolean validateServiceItem(serviceitem sitem)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                //if (sitem.ServiceItemID.Trim().Length == 0 || sitem.ServiceItemID == null)
                //{
                //    return false;
                //}
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
            }
            catch (Exception)
            {

            }
            return status;
        }
        public Boolean forwardServiceItem(serviceitem prevsitem)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ServiceItem set documentstatus=" + (prevsitem.documentStatus + 1) +
                     ", forwardUser='" + prevsitem.ForwardUser + "'" +
                    ", ForwarderList='" + prevsitem.ForwarderList + "'" +
                    " where ServiceItemID='" + prevsitem.ServiceItemID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("forward", "ServiceItem", "", updateSQL) +
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
        public Boolean ApproveServiceItem(serviceitem prevsitem, string id)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ServiceItem set ServiceItemID = '" + id +
                    "', documentstatus=99" +
                    ", status=1" +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    " where  ServiceItemID='" + prevsitem.ServiceItemID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "ServiceItem", "", updateSQL) +
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
        public Boolean reverseServiceITem(serviceitem sitem)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ServiceItem set DocumentStatus=" + sitem.documentStatus +
                    ", forwardUser='" + sitem.ForwardUser + "'" +
                    ", ForwarderList='" + sitem.ForwarderList + "'" +
                    " where  ServiceItemID='" + sitem.ServiceItemID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ServiceItem", "", updateSQL) +
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
        public static void fillServiceItemCombo(System.Windows.Forms.ComboBox cmb, string CategoryName)
        {
            cmb.Items.Clear();
            try
            {
                ServiceItemsDB serviceitemdb = new ServiceItemsDB();
                List<serviceitem> ServItems = serviceitemdb.getServiceItems();
                foreach (serviceitem si in ServItems)
                {
                    if (si.status == 1)
                    {
                        cmb.Items.Add(si.ServiceItemID + "-" + si.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
        }
        public static void fillServiceItemGridViewCombo(DataGridViewComboBoxCell cmb, string CategoryName)
        {
            cmb.Items.Clear();
            try
            {
                ServiceItemsDB serviceitemdb = new ServiceItemsDB();
                List<serviceitem> ServItems = serviceitemdb.getServiceItems();
                foreach (serviceitem si in ServItems)
                {
                    if (si.status == 1)
                    {
                        cmb.Items.Add(si.ServiceItemID + "-" + si.Name);
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
                ServiceItemsDB sidb = new ServiceItemsDB();
                List<serviceitem> serviceitems = sidb.getFilteredServiceItems("", 6);
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Item Id", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Item Name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Cat", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Group", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Type", -2, HorizontalAlignment.Center);
                foreach (serviceitem si in serviceitems)
                {

                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(si.ServiceItemID);
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

        public static TreeView getServiceItemTreeView()
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
                ServiceItemsDB sidb = new ServiceItemsDB();
                List<serviceitem> Serviceitems = sidb.getFilteredServiceItems("", 6).Where(si => si.status == 1 && si.documentStatus == 99).ToList();
                TreeNode tNode;
                string RootDesc = "";
                string SubRootDesc = "";
                string SubSubRootDesc = "";
                int j = 0;
                int k = 0;
                int l = 0;
                foreach (serviceitem item in Serviceitems)
                {
                    l++;
                    int n = tv.Nodes.Count;
                    if (item.Group1CodeDescription != RootDesc)
                    {
                        j = 0; k = 0;

                        tNode = tv.Nodes.Add(item.Group1CodeDescription);
                        tv.Nodes[n].Nodes.Add(item.Group2CodeDescription);
                        tv.Nodes[n].Nodes[j].Nodes.Add(item.Group3CodeDescription);
                        tv.Nodes[n].Nodes[j].Nodes[k].Nodes.Add(item.ServiceItemID + "-" + item.Name);
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
                            tv.Nodes[n - 1].Nodes[j].Nodes[k].Nodes.Add(item.ServiceItemID + "-" + item.Name);
                            SubRootDesc = item.Group2CodeDescription;
                            SubSubRootDesc = item.Group3CodeDescription;
                        }
                        else
                        {
                            if (item.Group3CodeDescription != SubSubRootDesc)
                            {
                                k = k + 1;
                                tv.Nodes[n - 1].Nodes[j].Nodes.Add(item.Group3CodeDescription);
                                tv.Nodes[n - 1].Nodes[j].Nodes[k].Nodes.Add(item.ServiceItemID + "-" + item.Name);
                                SubSubRootDesc = item.Group3CodeDescription;
                            }
                            else
                            {
                                tv.Nodes[n - 1].Nodes[j].Nodes[k].Nodes.Add(item.ServiceItemID + "-" + item.Name);
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
        //public static string getUnitForSelectedServiceItem(string ItemID, string ItemName)
        //{
        //    serviceitem sitem = new serviceitem();

        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "select ServiceItemID, Name,Unit" +
        //            " from ViewServiceItem where ServiceItemID = '" + ItemID + "' and Name = '" + ItemName + "'";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        try
        //        {
        //            if (reader.Read())
        //            {
        //                sitem.ServiceItemID = reader.GetString(0);
        //                sitem.Name = reader.GetString(1);
        //                sitem.Unit = reader.GetString(2);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error querying Service Item Data");
        //    }
        //    return sitem.Unit;
        //}
        public static ListView getServiceItemListView()
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
                ServiceItemsDB sidb = new ServiceItemsDB();
                List<serviceitem> SIList = sidb.getFilteredServiceItems("", 6);
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ServiceItem ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ServiceItem Name", -2, HorizontalAlignment.Left);

                foreach (serviceitem si in SIList)
                {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        item1.SubItems.Add(si.ServiceItemID.ToString());
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
