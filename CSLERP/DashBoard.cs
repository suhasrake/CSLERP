using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using CSLERP.DBData;
using System.Diagnostics;
using CSLERP.FileManager;

namespace CSLERP
{
    public partial class DashBoard : System.Windows.Forms.Form
    {

        dashboardalarm prevdsb = new dashboardalarm();
        tapalDistribution prevtd = new tapalDistribution();
        TextBox txtSearch = new TextBox();
        Panel pnllv = new Panel();
        ListView lv = new ListView();
        ListView lvCopy = new ListView();
        List<user> list = new List<user>();
        string filDir = "";
        Form frmPopup = new Form();
        DataGridView grdEmpList = new DataGridView();
        public DashBoard()
        {
            InitializeComponent();
        }
        private void DashBoard_Load(object sender, EventArgs e)
        {
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, 1135, 600, 20, 20));
            ShowIncomingDocumentList();
            ShowIncomingTapalList();
            ShowMovementRegisterList();
        }
        private void ShowIncomingDocumentList()
        {
            ////Boolean stat = true;
            try
            {
                grdDocumentList.Rows.Clear();
                DashboardDB dsDB = new DashboardDB();
                List<dashboardalarm> DSList = dsDB.getDashboardAlarms(Login.userLoggedIn);
                foreach (dashboardalarm dsl in DSList)
                {
                    grdDocumentList.Rows.Add();
                    grdDocumentList.Rows[grdDocumentList.RowCount - 1].Cells["LineNo"].Value = grdDocumentList.RowCount;
                    grdDocumentList.Rows[grdDocumentList.RowCount - 1].Cells["DocumentID"].Value = dsl.DocumentID;
                    grdDocumentList.Rows[grdDocumentList.RowCount - 1].Cells["DocumentName"].Value = dsl.DocumentName;
                    grdDocumentList.Rows[grdDocumentList.RowCount - 1].Cells["TemporaryNo"].Value = dsl.TemporaryNo;
                    grdDocumentList.Rows[grdDocumentList.RowCount - 1].Cells["TemporaryDate"].Value = dsl.TemporaryDate;
                    grdDocumentList.Rows[grdDocumentList.RowCount - 1].Cells["DocumentNo"].Value = dsl.DocumentNo;
                    grdDocumentList.Rows[grdDocumentList.RowCount - 1].Cells["DocumentDate"].Value = dsl.DocumentDate;
                    grdDocumentList.Rows[grdDocumentList.RowCount - 1].Cells["ActivityType"].Value = getActivityType(dsl.ActivityType);
                    grdDocumentList.Rows[grdDocumentList.RowCount - 1].Cells["FromUser"].Value = dsl.FromUserName;
                }
                if (grdDocumentList.Rows.Count > 0)
                {
                    pnlShowCurrentDocument.Visible = true;
                }
                else
                {
                    pnlShowCurrentDocument.Visible = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in showing DocumentList Status");
            }
            ////return stat;
        }
        private string getActivityType(int status)
        {
            if (status == 1)
                return "Created";
            else if (status == 2)
                return "Forwarded";
            else
                return "Approved";
        }
        private int setActivityType(string type)
        {
            if (type.Equals("Created"))
                return 1;
            else if (type.Equals("Forwarded"))
                return 2;
            else
                return 3;
        }
        private void grdDocumentList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DashboardDB dsDB = new DashboardDB();
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdDocumentList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Acknowledge") || columnName.Equals("Close"))
                {
                    prevdsb.DocumentID = grdDocumentList.Rows[e.RowIndex].Cells["DocumentID"].Value.ToString();
                    prevdsb.DocumentName = grdDocumentList.Rows[e.RowIndex].Cells["DocumentName"].Value.ToString();
                    prevdsb.TemporaryNo = Convert.ToInt32(grdDocumentList.Rows[e.RowIndex].Cells["TemporaryNo"].Value.ToString());
                    prevdsb.TemporaryDate = Convert.ToDateTime(grdDocumentList.Rows[e.RowIndex].Cells["TemporaryDate"].Value.ToString());
                    prevdsb.DocumentNo = Convert.ToInt32(grdDocumentList.Rows[e.RowIndex].Cells["DocumentNo"].Value.ToString());
                    prevdsb.DocumentDate = Convert.ToDateTime(grdDocumentList.Rows[e.RowIndex].Cells["DocumentDate"].Value);
                    prevdsb.ActivityType = setActivityType(grdDocumentList.Rows[e.RowIndex].Cells["ActivityType"].Value.ToString());
                    prevdsb.FromUserName = grdDocumentList.Rows[e.RowIndex].Cells["FromUser"].Value.ToString();
                    if (columnName.Equals("Acknowledge"))
                        prevdsb.AckStatus = 1;
                    else
                        prevdsb.AckStatus = 0;
                    if (prevdsb.AckStatus == 1)
                    {
                        if (dsDB.updateDashboardAlarm(prevdsb, Login.userLoggedIn))
                        {
                            MessageBox.Show("Ack Status Updated");
                        }
                        else
                            MessageBox.Show("Failed to Updated");
                    }
                    else
                    {
                        if (dsDB.DeleteDashboardAlarm(prevdsb, Login.userLoggedIn))
                        {
                            MessageBox.Show("Dasboard detail deleted");
                        }
                        else
                            MessageBox.Show("Failed to delete");
                    }


                }
                ShowIncomingDocumentList();
            }
            catch (Exception ex)
            {
            }
        }

        private void tmrDashBoard_Tick(object sender, EventArgs e)
        {
            try
            {
                ////MessageBox.Show("Timer");
                long idleTime = IdleTimeFinder.IdleTimeFinder.GetIdleTime();
                ////MessageBox.Show("idleTime : "+idleTime);
                if (idleTime > 600000)
                {
                    //ERP idle for 10 minutes.....force logout  
                    ////MessageBox.Show("ERP idle for 10 minutes.....force logout ");
                    Application.Exit();
                }
                
                ShowIncomingDocumentList();
                ShowIncomingTapalList();
                ShowMovementRegisterList();
            }
            catch (Exception)
            {
            }
        }

        private void grdDocumentList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //do nothing
        }
        private void ShowIncomingTapalList()
        {
            try
            {
                grdTapaList.Rows.Clear();
                TapalPickDB tpdb = new TapalPickDB();
                List<tapalDistribution> TPList = tpdb.getTapalListInDashBoard(Login.userLoggedIn);
                foreach (tapalDistribution td in TPList)
                {
                    grdTapaList.Rows.Add();
                    grdTapaList.Rows[grdTapaList.RowCount - 1].Cells["SINo"].Value = grdTapaList.RowCount;
                    grdTapaList.Rows[grdTapaList.RowCount - 1].Cells["RowId"].Value = td.RowID;
                    grdTapaList.Rows[grdTapaList.RowCount - 1].Cells["DocID"].Value = td.DocumentID;
                    grdTapaList.Rows[grdTapaList.RowCount - 1].Cells["TapalReference"].Value = td.TapalReference;
                    grdTapaList.Rows[grdTapaList.RowCount - 1].Cells["Date"].Value = td.Date;
                    grdTapaList.Rows[grdTapaList.RowCount - 1].Cells["FileName"].Value = td.FileName;
                    grdTapaList.Rows[grdTapaList.RowCount - 1].Cells["InwardDocumentType"].Value = td.InwardDocumentType;
                    grdTapaList.Rows[grdTapaList.RowCount - 1].Cells["ReceivedFrom"].Value = td.ReceivedFrom;
                    grdTapaList.Rows[grdTapaList.RowCount - 1].Cells["Sender"].Value = td.Creator;
                    grdTapaList.Rows[grdTapaList.RowCount - 1].Cells["Description"].Value = td.Description;
                }
                if (grdTapaList.Rows.Count > 0)
                {
                    pnlTapal.Visible = true;
                }
                else
                {
                    pnlTapal.Visible = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in showing DocumentList Status");
            }
        }

        private void grdTapaList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                TapalPickDB tpdb = new TapalPickDB();
                if (e.RowIndex < 0)
                    return;
                string columnName = grdTapaList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("View") || columnName.Equals("Move") || columnName.Equals("Delete"))
                {
                    prevtd.RowID = Convert.ToInt32(grdTapaList.Rows[e.RowIndex].Cells["RowId"].Value.ToString());
                    prevtd.DocumentID = grdTapaList.Rows[e.RowIndex].Cells["DocID"].Value.ToString();
                    prevtd.TapalReference = Convert.ToInt32(grdTapaList.Rows[e.RowIndex].Cells["TapalReference"].Value.ToString());
                    prevtd.Date = Convert.ToDateTime(grdTapaList.Rows[e.RowIndex].Cells["Date"].Value.ToString());
                    prevtd.FileName = grdTapaList.Rows[e.RowIndex].Cells["FileName"].Value.ToString();
                    prevtd.InwardDocumentType = grdTapaList.Rows[e.RowIndex].Cells["InwardDocumentType"].Value.ToString();
                    prevtd.ReceivedFrom = grdTapaList.Rows[e.RowIndex].Cells["ReceivedFrom"].Value.ToString();
                    prevtd.UserName = grdTapaList.Rows[e.RowIndex].Cells["Sender"].Value.ToString();
                    if (columnName.Equals("View"))
                    {
                        grdTapaList.Enabled = false;
                        string path = TapalPickDB.getFileFromDB(prevtd.TapalReference, prevtd.FileName);
                        filDir = path;
                        System.Diagnostics.Process process = System.Diagnostics.Process.Start(path);
                        grdTapaList.Enabled = true;
                        process.EnableRaisingEvents = true;
                        process.Exited += new EventHandler(myProcess_Exited);
                        process.WaitForExit();
                        tpdb.ChangeTapalStatus(prevtd.RowID, 3);

                    }
                    if (columnName.Equals("Move"))
                    {
                        showEmployeeDataGridView("NVP");
                        //showEmployeeListView();
                        // ShowIncomingTapalList();
                    }
                    if (columnName.Equals("Delete"))
                    {
                        if (columnName.Equals("Delete"))
                        {
                            DialogResult dialog = MessageBox.Show("Are you sure to delete the document ?", "Yes", MessageBoxButtons.YesNo);
                            if (dialog == DialogResult.Yes)
                            {
                                if (tpdb.ChangeTapalStatus(prevtd.RowID, 2))
                                {
                                    MessageBox.Show("Tapal Removed");
                                }
                                else
                                {
                                    MessageBox.Show("Failed to remove Tapal");
                                }
                                ShowIncomingTapalList();
                            }
                            else
                                return;
                        }

                    }
                }
                ShowIncomingDocumentList();
            }
            catch (Exception ex)
            {
            }
        }
        private void myProcess_Exited(object sender, System.EventArgs e)
        {
            if (File.Exists(filDir))
            {
                try
                {
                    File.Delete(filDir);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this.ToString() + "-" +
                        System.Reflection.MethodBase.GetCurrentMethod().Name + "() - Error : " + ex.ToString());
                }
            }
        }
        //showing Emp List In Grid
        private void showEmployeeDataGridView(string Office)
        {
            try
            {
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.CadetBlue;

                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(500, 370);

                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(120, 5);
                lblSearch.AutoSize = true;
                lblSearch.Text = "Search by Name";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(250, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInEmpGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                EmployeeDB empDB = new EmployeeDB();
                grdEmpList = empDB.getEmployeelistGrid(Office);

                grdEmpList.Bounds = new Rectangle(new Point(0, 27), new Size(500, 300));
                grdEmpList.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.grdEmplList_ColumnHeaderMouseClick);
                frmPopup.Controls.Add(grdEmpList);
                grdEmpList.Columns["OfficeID"].Visible = false;
                grdEmpList.Columns["UserID"].Visible = false;
                grdEmpList.Columns["EmployeeName"].Width = 180;
                grdEmpList.Columns["OfficeName"].Width = 150;
                foreach (DataGridViewColumn column in grdEmpList.Columns)
                    column.SortMode = DataGridViewColumnSortMode.Programmatic;

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grdOK_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 335);
                lvCancel.Click += new System.EventHandler(this.grdCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdEmplList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //try
            //{
            //    DataGridViewColumn newColumn = grdEmpList.Columns[e.ColumnIndex];
            //    DataGridViewColumn oldColumn = grdEmpList.SortedColumn;
            //    ListSortDirection direction;

            //    // If oldColumn is null, then the DataGridView is not sorted.
            //    if (oldColumn != null)
            //    {
            //        // Sort the same column again, reversing the SortOrder.
            //        if (oldColumn == newColumn &&
            //            grdEmpList.SortOrder == SortOrder.Ascending)
            //        {
            //            direction = ListSortDirection.Descending;
            //        }
            //        else
            //        {
            //            // Sort a new column and remove the old SortGlyph.
            //            direction = ListSortDirection.Ascending;
            //            oldColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
            //        }
            //    }
            //    else
            //    {
            //        direction = ListSortDirection.Ascending;
            //    }

            //    // Sort the selected column.
            //    grdEmpList.Sort(newColumn, direction);
            //    newColumn.HeaderCell.SortGlyphDirection =
            //        direction == ListSortDirection.Ascending ?
            //        SortOrder.Ascending : SortOrder.Descending;
            //}
            //catch (Exception ex)
            //{
            //}
        }
        private void grdOK_Click1(object sender, EventArgs e)
        {
            list = new List<user>();
            string users = "";
            try
            {
                var checkedRows = from DataGridViewRow r in grdEmpList.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("select Only One recipient");
                    return;
                }
                //DataGridViewRow selectedRow = new DataGridViewRow();
                foreach (var row in checkedRows)
                {
                    string empName = row.Cells["EmployeeName"].Value.ToString();
                    string userID = row.Cells["UserID"].Value.ToString();
                    if (moveTapal(userID))
                    {
                        MessageBox.Show("Tapal moved To :" + empName);
                    }
                    else
                        MessageBox.Show("Failed to move Tapal");
                }
                frmPopup.Close();
                frmPopup.Dispose();
                ///showDistributePanel(users);
                ShowIncomingTapalList();
            }
            catch (Exception)
            {
            }
        }
        private void grdCancel_Click1(object sender, EventArgs e)
        {
            try
            {
                frmPopup.Close();
                frmPopup.Dispose();
                ShowIncomingTapalList();
            }
            catch (Exception)
            {
            }
        }
        private void txtSearch_TextChangedInEmpGridList(object sender, EventArgs e)
        {
            try
            {
                //grdForStockList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                filterGridData();
                ///grdForStockList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {

            }
        }
        private void filterGridData()
        {
            try
            {
                grdEmpList.CurrentCell = null;
                foreach (DataGridViewRow row in grdEmpList.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdEmpList.Rows)
                    {
                        if (!row.Cells["EmployeeName"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
                        {
                            row.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Searching");
            }
        }
        //---------------------
        private void LvColumnClick(object o, ColumnClickEventArgs e)
        {
            try
            {
                if (lv.Visible == true)
                {
                    string first = lv.Items[0].SubItems[e.Column].Text;
                    string last = lv.Items[lv.Items.Count - 1].SubItems[e.Column].Text;
                    System.Windows.Forms.SortOrder sort_order1 = SortingListView.getSortedOrder(first, last);
                    this.lv.ListViewItemSorter = new SortingListView(e.Column, sort_order1);
                }
                else
                {
                    string first = lvCopy.Items[0].SubItems[e.Column].Text;
                    string last = lvCopy.Items[lvCopy.Items.Count - 1].SubItems[e.Column].Text;
                    System.Windows.Forms.SortOrder sort_order1 = SortingListView.getSortedOrder(first, last);
                    this.lvCopy.ListViewItemSorter = new SortingListView(e.Column, sort_order1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorting error");
            }
        }
        //private void listView3_ItemChecked(object sender, ItemCheckedEventArgs e)
        //{
        //    try
        //    {
        //        if (lv.CheckedIndices.Count > 1)
        //        {
        //            MessageBox.Show("Cannot select more than one item");
        //            e.Item.Checked = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        private void removeControlsFromPnlLvPanel()
        {
            try
            {
                //foreach (Control p in pnllv.Controls)
                //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(System.Windows.Forms.Button))
                //    {
                //        p.Dispose();
                //    }
                pnllv.Controls.Clear();
                Control nc = pnllv.Parent;
                nc.Controls.Remove(pnllv);
            }
            catch (Exception ex)
            {
            }
        }
        private void showEmployeeListView()
        {
            removeControlsFromPnlLvPanel();
            //pnllv.Controls.Clear();
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;
            pnllv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(100, 100), new Size(600, 300));
            lv = ERPUserDB.getEmployeeUserlv();
            removeMeFromLV();
            lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            ////////this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView3_ItemChecked);
            lv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(50, 50), new Size(500, 200));
            pnllv.Controls.Add(lv);

            System.Windows.Forms.Button lvOK = new System.Windows.Forms.Button();
            lvOK.Text = "OK";
            lvOK.Location = new System.Drawing.Point(50, 270);
            lvOK.Click += new System.EventHandler(this.lvOK_Click2);
            pnllv.Controls.Add(lvOK);

            System.Windows.Forms.Button lvCancel = new System.Windows.Forms.Button();
            lvCancel.Text = "Cancel";
            lvCancel.Location = new System.Drawing.Point(150, 270);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
            pnllv.Controls.Add(lvCancel);

            System.Windows.Forms.Label lblSearch = new System.Windows.Forms.Label();
            lblSearch.Text = "Find";
            lblSearch.Location = new System.Drawing.Point(265, 275);
            lblSearch.Size = new Size(37, 15);
            pnllv.Controls.Add(lblSearch);

            txtSearch = new System.Windows.Forms.TextBox();
            txtSearch.Location = new System.Drawing.Point(305, 270);
            txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            pnllv.Controls.Add(txtSearch);


            pnlUI.Controls.Add(pnllv);
            pnllv.BringToFront();
            pnllv.Visible = true;
        }
        private void lvOK_Click2(object sender, EventArgs e)
        {
            
            try
            {
                if (lv.Visible == true)
                {
                    try
                    {
                        if (lv.CheckedIndices.Count > 1)
                        {
                            MessageBox.Show("Only one recipient allowed");
                            return;
                        }
                        if (lv.CheckedItems.Count == 0)
                        {
                            MessageBox.Show("select one recipient");
                            return;
                        }
                    }
                    catch (Exception)
                    {
                    }
                   
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            if (moveTapal(itemRow.SubItems[3].Text))
                            {
                                MessageBox.Show("Tapal moved To :" + itemRow.SubItems[3].Text);
                            }
                            else
                                MessageBox.Show("Failed to move Tapal");
                        }
                    }

                }
                else
                {
                    try
                    {
                        if (lvCopy.CheckedItems.Count == 0)
                        {
                            MessageBox.Show("select recipient");
                            return;
                        }
                        if (lvCopy.CheckedIndices.Count > 1)
                        {
                            MessageBox.Show("Only one recipient allowed");
                            return;
                        }
                    }
                    catch (Exception)
                    {
                    }
                    foreach (ListViewItem itemRow in lvCopy.Items)
                    {
                        if (itemRow.Checked)
                        {
                            if (moveTapal(itemRow.SubItems[3].Text))
                            {
                                MessageBox.Show("Tapal moved To :" + itemRow.SubItems[3].Text);
                            }
                            else
                                MessageBox.Show("Failed to move Tapal");
                        }
                    }

                }
                pnllv.Visible = false;
                pnllv.Controls.Clear();
                pnlUI.Controls.Remove(pnllv);
                ShowIncomingTapalList();
                //showDistributePanel(users);
            }
            catch (Exception ex)
            {
            }
        }

        private void removeMeFromLV()
        {

            try
            {
                int i = 0;
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.SubItems[2].Text == Login.userLoggedInName &&
                        itemRow.SubItems[1].Text == Login.empLoggedIn)
                    {
                        lv.Items[i].Remove();
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void lvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                pnllv.Visible = false;
                pnllv.Controls.Clear();
                pnllv.Controls.Remove(pnllv);
                ShowIncomingTapalList();
            }
            catch (Exception)
            {
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            pnllv.Controls.Remove(lvCopy);
            addItems();
        }
        private void addItems()
        {
            lvCopy = new ListView();
            lvCopy.View = System.Windows.Forms.View.Details;
            lvCopy.LabelEdit = true;
            lvCopy.AllowColumnReorder = true;
            lvCopy.CheckBoxes = true;
            lvCopy.FullRowSelect = true;
            lvCopy.GridLines = true;
            lvCopy.Sorting = System.Windows.Forms.SortOrder.Ascending;
            lvCopy.Columns.Add("Select", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Id", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Name", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("UserID", -2, HorizontalAlignment.Left);
            lvCopy.Columns[3].Width = 0;
            lvCopy.Sorting = System.Windows.Forms.SortOrder.None;
            lvCopy.ColumnClick += new ColumnClickEventHandler(LvColumnClick); ;
            //this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
            lvCopy.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(50, 50), new Size(500, 200));
            lvCopy.Items.Clear();
            foreach (ListViewItem row in lv.Items)
            {
                string x = row.SubItems[0].Text;
                string no = row.SubItems[1].Text;
                string ch = row.SubItems[2].Text;
                string user = row.SubItems[3].Text;
                if (ch.ToLower().StartsWith(txtSearch.Text))
                {
                    ListViewItem item = new ListViewItem();
                    item.SubItems.Add(no);
                    item.SubItems.Add(ch);
                    item.SubItems.Add(user);
                    item.Checked = false;
                    lvCopy.Items.Add(item);
                }
            }
            lv.Visible = false;
            lvCopy.Visible = true;
            pnllv.Controls.Add(lvCopy);
        }
        //private void CopylistView2_ItemChecked(object sender, ItemCheckedEventArgs e)
        //{
        //    try
        //    {
        //        if (lvCopy.CheckedIndices.Count > 1)
        //        {
        //            MessageBox.Show("Cannot select more than one item");
        //            e.Item.Checked = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        private Boolean moveTapal(string userID)
        {
            Boolean status = true;
            try
            {
                TapalPickDB tpdb = new TapalPickDB();
                prevtd.Date = DateTime.Now.Date;
                if (tpdb.MoveTapal(userID, prevtd))
                {
                    status = true;
                }
                else
                {
                    //pnlDistrbutor.Visible = false;
                    status = false;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        //Movement Detail Showing on Dashboard
        private void ShowMovementRegisterList()
        {
            try
            {
                grdMovementReg.Rows.Clear();
                MovementRegisterDB mrdb = new MovementRegisterDB();
                List<movementregister> MRList = mrdb.getMovementRegForDashboard();
                foreach (movementregister mr in MRList)
                {
                    grdMovementReg.Rows.Add();
                    grdMovementReg.Rows[grdMovementReg.RowCount - 1].Cells["Sno"].Value = grdMovementReg.RowCount;
                    grdMovementReg.Rows[grdMovementReg.RowCount - 1].Cells["moveDate"].Value = mr.CreateTime;
                    grdMovementReg.Rows[grdMovementReg.RowCount - 1].Cells["From"].Value = mr.EmployeeName;
                    grdMovementReg.Rows[grdMovementReg.RowCount - 1].Cells["Purpose"].Value = mr.Purpose;
                    grdMovementReg.Rows[grdMovementReg.RowCount - 1].Cells["PlannedExitTime"].Value = mr.ExitTimePlanned;
                    grdMovementReg.Rows[grdMovementReg.RowCount - 1].Cells["PlannedReturnTime"].Value = mr.ReturnTimePlanned;
                    grdMovementReg.Rows[grdMovementReg.RowCount - 1].Cells["Status"].Value = getMovementStatus(mr.Status, mr.DocumentStatus);
                    grdMovementReg.Rows[grdMovementReg.RowCount - 1].Cells["DocumentStatus"].Value = mr.DocumentStatus;
                }
                if (grdMovementReg.Rows.Count > 0)
                {
                    pnlMovementReg.Visible = true;
                }
                else
                {
                    pnlMovementReg.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in showing MOvementRegList");
            }
        }
        private string getMovementStatus(int status, int docStatus)
        {
            string stat = "";
            if (status == 1 && docStatus == 1)
                stat = "Received";
            else if (status == 1 && docStatus == 2)
                stat = "Approved";
            return stat;
        }

        private void grdMovementReg_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DashBoard_Enter(object sender, EventArgs e)
        {
            try
            {
                string frmname = this.Name;
                string menuid = Main.menuitems.Where(x => x.pageLink == frmname).Select(x => x.menuItemID).FirstOrDefault().ToString();
                Main.itemPriv = Utilities.fillItemPrivileges(Main.userOptionArray, menuid);
            }
            catch (Exception ex)
            {
            }
        }
    }

}

