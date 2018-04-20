using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class accountcode
    {
        public int rowID { get; set; }
        public string AccountCode { get; set; }
        public string Name { get; set; }
        public string GroupLevel1 { get; set; }
        public string GroupLevel1Description { get; set; }
        public string GroupLevel2 { get; set; }
        public string GroupLevel2eDescription { get; set; }
        public string GroupLevel3 { get; set; }
        public string GroupLevel3Description { get; set; }
        public string GroupLevel4 { get; set; }
        public string GroupLevel4Description { get; set; }
        public string GroupLevel5 { get; set; }
        public string GroupLevel5Description { get; set; }
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

    class AccountCodeDB
    {
        ActivityLogDB alDB = new ActivityLogDB();


        public List<accountcode> getFilteredAccountDetails(string userList, int opt)
        {
            accountcode accode;
            List<accountcode> ACItems = new List<accountcode>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query1 = "select AccountCode, Name," +
                    " GroupLevel1, GroupLevel1Description,GroupLevel2, GroupLevel2Description,GroupLevel3, GroupLevel3Description, " +
                    " GroupLevel4, GroupLevel4Description,GroupLevel5, GroupLevel5Description," +
                    " Status,DocumentStatus,CreateUser,ForwardUser,ApproveUser, " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList " +
                    " from ViewAccountCode  " +
                    " where ((forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1))";

                string query2 = "select AccountCode, Name," +
                    " GroupLevel1, GroupLevel1Description,GroupLevel2, GroupLevel2Description,GroupLevel3, GroupLevel3Description, " +
                    " GroupLevel4, GroupLevel4Description,GroupLevel5, GroupLevel5Description," +
                    " Status,DocumentStatus,CreateUser,ForwardUser,ApproveUser, " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList " +
                    " from ViewAccountCode  " +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "'))";

                string query3 = "select AccountCode, Name," +
                    " GroupLevel1, GroupLevel1Description,GroupLevel2, GroupLevel2Description,GroupLevel3, GroupLevel3Description, " +
                     " GroupLevel4, GroupLevel4Description,GroupLevel5, GroupLevel5Description," +
                    " Status,DocumentStatus,CreateUser,ForwardUser,ApproveUser, " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList " +
                    " from ViewAccountCode  " +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99 and status = 1)";
                string query6 = "select AccountCode, Name," +
                    " GroupLevel1, GroupLevel1Description,GroupLevel2, GroupLevel2Description,GroupLevel3, GroupLevel3Description, " +
                     " GroupLevel4, GroupLevel4Description,GroupLevel5, GroupLevel5Description," +
                    " Status,DocumentStatus,CreateUser,ForwardUser,ApproveUser, " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList " +
                    " from ViewAccountCode  " +
                    " where DocumentStatus = 99 and Status = 1" +
                    " order by GroupLevel1,GroupLevel2,GroupLevel3";
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
                    accode = new accountcode();
                    accode.AccountCode = reader.GetString(0);
                    accode.Name = reader.GetString(1);
                    accode.GroupLevel1 = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                    {
                        accode.GroupLevel1Description = reader.GetString(3);
                    }
                    else
                    {
                        accode.GroupLevel1Description = "";
                    }
                    accode.GroupLevel2 = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                    {
                        accode.GroupLevel2eDescription = reader.GetString(5);
                    }
                    else
                    {
                        accode.GroupLevel2eDescription = "";
                    }

                    accode.GroupLevel3 = reader.GetString(6);
                    if (!reader.IsDBNull(7))
                    {
                        accode.GroupLevel3Description = reader.GetString(7);
                    }
                    else
                    {
                        accode.GroupLevel3Description = "";
                    }
                    accode.GroupLevel4 = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    accode.GroupLevel4Description = reader.IsDBNull(9)? "" : reader.GetString(9);
                    accode.GroupLevel5 = reader.IsDBNull(10) ? "" : reader.GetString(10);
                    accode.GroupLevel5Description = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    accode.status = reader.GetInt32(12);
                    accode.documentStatus = reader.GetInt32(13);
                    accode.CreateUser = reader.GetString(14);
                    accode.ForwardUser = reader.GetString(15);
                    accode.ApproveUser = reader.GetString(16);
                    accode.CreateUserName = reader.GetString(17);
                    accode.ForwardUserName = reader.GetString(18);
                    accode.ApproveUserName = reader.GetString(19);
                    if (!reader.IsDBNull(20))
                    {
                        accode.ForwarderList = reader.GetString(20);
                    }
                    else
                    {
                        accode.ForwarderList = "";
                    }
                    ACItems.Add(accode);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return ACItems;
        }
        public List<accountcode> getAccoutDetails()
        {
            accountcode acc;
            List<accountcode> ACList = new List<accountcode>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select AccountCode, Name," +
                    " GroupLevel1, GroupLevel1Description,GroupLevel2, GroupLevel2Description,GroupLevel3, GroupLevel3Description, " +
                    "GroupLevel4, GroupLevel4Description,GroupLevel5, GroupLevel5Description, " + 
                    " Status,DocumentStatus,CreateUser,ForwardUser,ApproveUser, " +
                    " CreatorName,ForwarderName,ApproverName,ForwarderList " +
                    " from ViewAccountCode  " +
                    " order by AccountCode";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    acc = new accountcode();
                    acc.AccountCode = reader.GetString(0);
                    acc.Name = reader.GetString(1);
                    acc.GroupLevel1 = reader.GetString(2);
                    acc.GroupLevel1Description = reader.GetString(3);
                    acc.GroupLevel2 = reader.GetString(4);
                    acc.GroupLevel2eDescription = reader.GetString(5);
                    acc.GroupLevel3 = reader.GetString(6);
                    acc.GroupLevel3Description = reader.GetString(7);
                    acc.GroupLevel4 = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    acc.GroupLevel4Description = reader.IsDBNull(9) ? "" : reader.GetString(9);
                    acc.GroupLevel5 = reader.IsDBNull(10) ? "" : reader.GetString(10);
                    acc.GroupLevel5Description = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    acc.status = reader.GetInt32(12);
                    acc.documentStatus = reader.GetInt32(13);
                    acc.CreateUser = reader.GetString(14);
                    acc.ForwardUser = reader.GetString(15);
                    acc.ApproveUser = reader.GetString(16);
                    acc.CreateUserName = reader.GetString(17);
                    acc.ForwardUserName = reader.GetString(18);
                    acc.ApproveUserName = reader.GetString(19);
                    ACList.Add(acc);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return ACList;

        }
        public Boolean updateAccountCodeDetails(accountcode accode, accountcode prevaccode)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update AccountCode set " +
                    "Name='" + accode.Name +
                    "', GroupLevel1='" + accode.GroupLevel1 +
                    "', GroupLevel2='" + accode.GroupLevel2 +
                    "', GroupLevel3='" + accode.GroupLevel3 +
                     "', GroupLevel4='" + accode.GroupLevel4 +
                    "', GroupLevel5='" + accode.GroupLevel5 +
                    "', ForwarderList='" + accode.ForwarderList + "'" +
                      ", Status=" + accode.status +
                    " where AccountCode='" + prevaccode.AccountCode + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "AccoutCode", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean insertAccountCodeDetails(accountcode accode)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into AccountCode " +
                    " (AccountCode,Name,GroupLevel1,GroupLevel2,GroupLevel3,GroupLevel4,GroupLevel5,Status,DocumentStatus,CreateTime,CreateUser,ForwarderList)" +
                    "values (" +
                    "IDENT_CURRENT('AccountCode')" +
                    ",'" + accode.Name + "'," +
                    "'" + accode.GroupLevel1 + "'," +
                    "'" + accode.GroupLevel2 + "'," +
                    "'" + accode.GroupLevel3 + "'," +
                       "'" + accode.GroupLevel4 + "'," +
                    "'" + accode.GroupLevel5 + "'," +
                    accode.status + "," +
                    accode.documentStatus + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" +
                    ",'" + accode.ForwarderList + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "AccountCode", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean validateAccountCodeDetail(accountcode accode)
        {
            Boolean status = true;
            try
            {
                if (accode.Name.Trim().Length == 0 || accode.Name == null)
                {
                    return false;
                }
                if (accode.GroupLevel1.Trim().Length == 0 || accode.GroupLevel1 == null)
                {
                    return false;
                }
                if (accode.GroupLevel2.Trim().Length == 0 || accode.GroupLevel2 == null)
                {
                    return false;
                }
                if (accode.GroupLevel3.Trim().Length == 0 || accode.GroupLevel3 == null)
                {
                    return false;
                }
                if (accode.GroupLevel4.Trim().Length == 0 || accode.GroupLevel4 == null)
                {
                    return false;
                }
                if (accode.GroupLevel5.Trim().Length == 0 || accode.GroupLevel5 == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                return false;
            }
            return status;
        }
        public Boolean forwardAccountCode(accountcode prevaccode)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update AccountCode set documentstatus=" + (prevaccode.documentStatus + 1) +
                     ", forwardUser='" + prevaccode.ForwardUser + "'" +
                    ", ForwarderList='" + prevaccode.ForwarderList + "'" +
                    " where AccountCode='" + prevaccode.AccountCode + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("forward", "AccountCode", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean ApproveAccountCode(accountcode prevaccode, string id)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update AccountCode set AccountCode = '" + id +
                    "', documentstatus=99" +
                    ", status=1" +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    " where  AccountCode='" + prevaccode.AccountCode + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "AccountCode", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean reverseAccountCode(accountcode accode)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update AccountCode set DocumentStatus=" + accode.documentStatus +
                    ", forwardUser='" + accode.ForwardUser + "'" +
                    ", ForwarderList='" + accode.ForwarderList + "'" +
                    " where  AccountCode='" + accode.AccountCode + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "AccountCode", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public List<accountcode> getAccoutCodesList()
        {
            accountcode acc;
            List<accountcode> ACList = new List<accountcode>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select AccountCode, Name" +
                    " from AccountCode  " +
                    " order by AccountCode";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    acc = new accountcode();
                    acc.AccountCode = reader.GetString(0);
                    acc.Name = reader.GetString(1);
                    ACList.Add(acc);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return ACList;

        }
        //public static void fillStockItemCombo(System.Windows.Forms.ComboBox cmb, string CategoryName)
        //{
        //    cmb.Items.Clear();
        //    try
        //    {
        //        StockItemDB stockitemdb = new StockItemDB();
        //        List<stockitem> StockItems = stockitemdb.getStockItems();
        //        foreach (stockitem si in StockItems)
        //        {
        //            //if (CategoryName.Length > 0 && CategoryName != si.Category)
        //            //{
        //            //    continue;
        //            //}
        //            if (si.status == 1)
        //            {
        //                cmb.Items.Add(si.StockItemID + "-" + si.Name);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
        //    }
        //}
        //public static void fillStockItemGridViewCombo(DataGridViewComboBoxCell cmb, string CategoryName)
        //{
        //    cmb.Items.Clear();
        //    try
        //    {
        //        StockItemDB stockitemdb = new StockItemDB();
        //        List<stockitem> StockItems = stockitemdb.getStockItems();
        //        foreach (stockitem si in StockItems)
        //        {
        //            //if (CategoryName.Length > 0 && CategoryName != si.Category)
        //            //{
        //            //    continue;
        //            //}
        //            if (si.status == 1)
        //            {
        //                cmb.Items.Add(si.StockItemID + "-" + si.Name);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
        //    }
        //}
        public static ListView getAccountCodeListView()
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
                AccountCodeDB ACDb = new AccountCodeDB();
                List<accountcode> acList = ACDb.getFilteredAccountDetails("", 6);
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Account code", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Account Name", -2, HorizontalAlignment.Left);
                foreach (accountcode ac in acList)
                {

                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(ac.AccountCode);
                    item1.SubItems.Add(ac.Name.ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lv;
        }

        public List<accountcode> getAccountDetailsList()
        {
            accountcode accode;
            List<accountcode> ACItems = new List<accountcode>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select AccountCode, Name," +
                    " GroupLevel1, GroupLevel1Description,GroupLevel2, GroupLevel2Description,GroupLevel3, GroupLevel3Description, " +
                    " Status,DocumentStatus" +
                    " from ViewAccountCode " +
                    " where DocumentStatus = 99 and status = 1" +
                    " order by AccountCode desc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    accode = new accountcode();
                    accode.AccountCode = reader.GetString(0);
                    accode.Name = reader.GetString(1);
                    accode.GroupLevel1 = reader.GetString(2);
                    accode.GroupLevel1Description = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    accode.GroupLevel2 = reader.GetString(4);
                    accode.GroupLevel2eDescription = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    accode.GroupLevel3 = reader.GetString(6);
                    accode.GroupLevel3Description = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    accode.status = reader.GetInt32(8);
                    accode.documentStatus = reader.GetInt32(9);
                    ACItems.Add(accode);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return ACItems;
        }
        public static DataGridView getGridViewForAccountCode()
        {

            DataGridView grdPOPI = new DataGridView();
            try
            {
                string[] strColArr = { "AccountCode", "AccountName"};
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                };

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdPOPI.EnableHeadersVisualStyles = false;
                grdPOPI.AllowUserToAddRows = false;
                grdPOPI.AllowUserToDeleteRows = false;
                grdPOPI.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdPOPI.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdPOPI.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdPOPI.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdPOPI.ColumnHeadersHeight = 27;
                grdPOPI.RowHeadersVisible = false;
                grdPOPI.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;
                grdPOPI.Columns.Add(colChk);
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str;
                    colArr[index].HeaderText = str;
                    colArr[index].ReadOnly = true;
                    grdPOPI.Columns.Add(colArr[index]);
                }
                AccountCodeDB ACDb = new AccountCodeDB();
                List<accountcode> acList = ACDb.getAccountDetailsList();
                foreach (accountcode acc in acList)
                {
                    grdPOPI.Rows.Add();
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[0]].Value = acc.AccountCode;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[1]].Value = acc.Name;
                }
            }
            catch (Exception ex)
            {
            }

            return grdPOPI;
        }

    }
}
