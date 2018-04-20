using System;
using System.Collections.Generic;
using System.Collections;
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
using System.Collections.ObjectModel;
using CSLERP.FileManager;

namespace CSLERP
{
    public partial class TapalPick : System.Windows.Forms.Form
    {
        string fileDir = "";
        string docID = "TAPAL";
        List<user> list = new List<user>();
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        TextBox txtSearch = new TextBox();
        tapal prevtapal;
        Panel pnllv = new Panel();
        ListView lv = new ListView();
        ListView lvCopy = new ListView();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        Form frmPopup = new Form();
        DataGridView grdEmpList = new DataGridView();
        static int lvl = 0;
        int no;
        public TapalPick()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void TapalPick_Load(object sender, EventArgs e)
        {
            //////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;

            String a = this.Size.ToString();
            grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdList.EnableHeadersVisualStyles = false;
            ShowControl();
            listtapal();
            //applyPrivilege();
            //btnNew.Visible = false;
        }
        private void ShowControl()
        {
            pnlList.Visible = true;
            grdList.Visible = true;
            //lvlSelect.Visible = true;
            //cmbSelectLevel.Visible = true;
        }
        private void listtapal()
        {
            try
            {
                grdList.Rows.Clear();
                clearData();
                TapalPickDB TapDB = new TapalPickDB();
                List<tapal> list = TapDB.getTapalPicFileDetails();
                int i = 1;
                foreach (tapal cg in list)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["LineNo"].Value = i;
                    grdList.Rows[grdList.RowCount - 1].Cells["RowID"].Value = cg.RowID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocID"].Value = cg.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["Date"].Value = cg.Date;
                    grdList.Rows[grdList.RowCount - 1].Cells["FileName"].Value = cg.FileName;
                    grdList.Rows[grdList.RowCount - 1].Cells["InwardDocumentType"].Value = cg.InwardDocumentType;
                    grdList.Rows[grdList.RowCount - 1].Cells["ReceivedFrom"].Value = cg.ReceivedFrom;
                    grdList.Rows[grdList.RowCount - 1].Cells["Description"].Value = cg.Description;
                    grdList.Rows[grdList.RowCount - 1].Cells["FileType"].Value = cg.FileType;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = cg.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProtectionLevel"].Value = cg.ProtectionLevel;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = cg.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = cg.CreateUser;
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            try
            {
                enableBottomButtons();
                pnlList.Visible = true;
                // btnNew.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void initVariables()
        {

            docID = Main.currentDocument;

            pnlUI.Controls.Add(pnlList);
            enableBottomButtons();
            pnlAddNew.Visible = false;
            pnlDistrbutor.Visible = false;
            grdList.Visible = true;
            TapalPickDB.fillInwardDocCombo(cmbInwardDocType, "TapalType");
            grdList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
        private void closeAllPanels()
        {
            try
            {
                pnlList.Visible = false;
                //pnlAddEdit.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        public void clearData()
        {
            try
            {
                txtFileName.Text = "";
                cmbInwardDocType.SelectedIndex = -1;
                txtReceivedFrom.Text = "";
                txtEmployees.Text = "";
                txtDescription.Text = "";
                prevtapal = new tapal();
                removeControlsFromPnlLvPanel();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                pnlUI.Visible = false;
            }
            catch (Exception)
            {

            }
        }
        private void enableBottomButtons()
        {
            ///btnNew.Visible = false;
            btnExit.Visible = true;
            pnlBottomButtons.Visible = true;
        }
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            clearData();
            grdList.Visible = true;
            pnlAddNew.Visible = false;
            pnlDistrbutor.Visible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {

                TapalPickDB agdb = new TapalPickDB();
                tapal tap = new tapal();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    //if (!System.Text.RegularExpressions.Regex.IsMatch(txtFileName.Text, @"^[0-9]+$"))
                    //{
                    //    MessageBox.Show("Group Code accepts only numeric characters");
                    //    return;
                    //}
                    //else
                    tap.DocumentID = docID;
                    tap.Date = UpdateTable.getSQLDateTime();
                    tap.ReceivedFrom = txtReceivedFrom.Text.Trim();
                    tap.Description = txtDescription.Text.Trim();
                    tap.FileName = Path.GetFileName(txtFileName.Text);
                    tap.InwardDocumentType = cmbInwardDocType.SelectedItem.ToString();
                    if (cmbInwardDocType.SelectedIndex == -1)
                    {
                        MessageBox.Show("Enter InwardDocumentType.");
                        return;
                    }
                    string nm = Path.GetFileName(txtFileName.Text);
                    //if (!System.Text.RegularExpressions.Regex.IsMatch(txtFileDesc.Text, @"^[\sa-zA-Z0-9]+$"))
                    //{
                    //    MessageBox.Show("GroupInwardDocumentType accepts only alphanumeric characters");
                    //    return;
                    //}
                    //else
                    byte[] byteArray = null;
                    FileStream fs = new FileStream(txtFileName.Text, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    tap.DocumentContent = Convert.ToBase64String(br.ReadBytes((int)fs.Length));

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }

                if (btnText.Equals("Save"))
                {
                    if (agdb.validateTapal(tap))
                    {
                        if (agdb.inserttapal(tap))
                        {
                            MessageBox.Show("Tapal Added");
                            closeAllPanels();
                            listtapal();
                            pnlAddNew.Visible = false;
                            grdList.Visible = true;
                            pnlBottomButtons.Visible = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Validation failed");
                    }
                    if (!status)
                    {
                        MessageBox.Show("Failed to save Tapal Document");
                    }
                }
                else
                {
                    MessageBox.Show("btnSave error.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                clearData();
                try
                {
                    if (columnName.Equals("Distribute") || columnName.Equals("View") || columnName.Equals("Delete"))
                    {
                        prevtapal = new tapal();
                        prevtapal.RowID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["RowID"].Value.ToString());
                        prevtapal.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocID"].Value.ToString();
                        prevtapal.Date = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["Date"].Value.ToString());
                        prevtapal.FileName = grdList.Rows[e.RowIndex].Cells["FileName"].Value.ToString();
                        prevtapal.ReceivedFrom = grdList.Rows[e.RowIndex].Cells["ReceivedFrom"].Value.ToString();
                        prevtapal.InwardDocumentType = grdList.Rows[e.RowIndex].Cells["InwardDocumentType"].Value.ToString();
                        prevtapal.FileType = grdList.Rows[e.RowIndex].Cells["FileType"].Value.ToString();
                        prevtapal.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString());
                        prevtapal.ProtectionLevel = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["ProtectionLevel"].Value.ToString());
                        prevtapal.CreateUser = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                        prevtapal.CreateTime = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["CreateTime"].Value.ToString());
                        prevtapal.Description = grdList.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                        if (columnName.Equals("View"))
                        {
                            grdList.Enabled = false;
                            string fileName = TapalPickDB.getFileFromDB(prevtapal.RowID, prevtapal.FileName);
                            fileDir = fileName;
                            System.Diagnostics.Process process = System.Diagnostics.Process.Start(fileName);
                            grdList.Enabled = true;
                            process.EnableRaisingEvents = true;
                            process.Exited += new EventHandler(myProcess_Exited);
                            process.WaitForExit();
                        }
                        if (columnName.Equals("Delete"))
                        {
                            DialogResult dialog = MessageBox.Show("Are you sure to delete the document ?", "Yes", MessageBoxButtons.YesNo);
                            if (dialog == DialogResult.Yes)
                            {
                                if (TapalPickDB.deleteTapal(prevtapal.RowID))
                                {
                                    MessageBox.Show("Deleted");
                                    listtapal();
                                }
                            }
                            else
                                return;
                        }
                        if (columnName.Equals("Distribute"))
                        {
                            showEmployeeDataGridView("NVP");
                            //showEmployeeListView();
                            //grdList.Enabled = false;
                            //btnPickFile.Enabled = false;
                        }
                        //txtFileName.Text = prevag.GroupCode;
                        //txtFileDesc.Text = prevag.GroupInwardDocumentType;
                        //pnlAddNew.Visible = true;
                        //txtFileName.ReadOnly = true;
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {

            }
        }
        private void myProcess_Exited(object sender, System.EventArgs e)
        {
            if (File.Exists(fileDir))
            {
                try
                {
                    File.Delete(fileDir);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
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
                frmPopup.Controls.Add(grdEmpList);
                grdEmpList.Columns["OfficeID"].Visible = false;
                grdEmpList.Columns["UserID"].Visible = false;
                grdEmpList.Columns["EmployeeName"].Width = 180;
                grdEmpList.Columns["OfficeName"].Width = 150;
                foreach (DataGridViewColumn column in grdEmpList.Columns)
                    column.SortMode = DataGridViewColumnSortMode.Automatic;

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
                if(selectedRowCount == 0)
                {
                    MessageBox.Show("Select Any Employee");
                    return;
                }
                //if (selectedRowCount != 1)
                //{
                //    MessageBox.Show("select Only One Item");
                //    return;
                //}
                //DataGridViewRow selectedRow = new DataGridViewRow();
                foreach (var row in checkedRows)
                {
                    user us = new user();
                    us.userEmpID = row.Cells["EmployeeID"].Value.ToString();
                    us.userEmpName = row.Cells["EmployeeName"].Value.ToString();
                    us.userID = row.Cells["UserID"].Value.ToString();
                    users = users + us.userEmpName + ";";
                    list.Add(us);
                }
                frmPopup.Close();
                frmPopup.Dispose();
                showDistributePanel(users);
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
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void removeControlsFromPnlLvPanel()
        {
            try
            {
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
            lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            lv.HideSelection = false;
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView3_ItemChecked);
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


            pnlList.Controls.Add(pnllv);
            pnllv.BringToFront();
            pnllv.Visible = true;



        }
        private void lvOK_Click2(object sender, EventArgs e)
        {
            list = new List<user>();
            string users = "";
            try
            {
                if (lv.CheckedItems.Count == 0)
                {
                    MessageBox.Show("select one Employee");
                    return;
                }
                int c = lv.CheckedIndices.Count;
                if(c > 1)
                {
                    MessageBox.Show("Only one recipient allowed");
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        user us = new user();
                        us.userEmpID = itemRow.SubItems[1].Text;
                        us.userEmpName = itemRow.SubItems[2].Text;
                        us.userID = itemRow.SubItems[3].Text;
                        users = users + us.userEmpName + ";";
                        list.Add(us);
                    }
                }
                pnllv.Visible = false;
                pnllv.Controls.Clear();
                pnlList.Controls.Remove(pnllv);
                showDistributePanel(users);
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
                pnlList.Controls.Remove(pnllv);
                grdList.Enabled = true;
                btnPickFile.Enabled = true;
            }
            catch (Exception)
            {
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //pnllv.Controls.Remove(lvCopy);
            try
            {
                addItems();
            }
            catch (Exception ex)
            {
            }
        }
        private void addItems()
        {
            foreach (ListViewItem row in lv.Items)
            {
                lv.Items[row.Index].Selected = false;
                row.BackColor = Color.White;
            }

            if (txtSearch.Text.Length != 0)
            {
                foreach (ListViewItem row in lv.Items)
                {
                    string ch = row.SubItems[2].Text;
                    if (ch.ToLower().StartsWith(txtSearch.Text))
                    {
                        lv.Items[row.Index].EnsureVisible();
                        lv.Items[row.Index].Selected = true;
                        lv.Focus();
                        return;
                    }
                }
            }
        }
        private void btnPickFile_Click(object sender, EventArgs e)
        {

            OpenFileDialog op1 = new OpenFileDialog();
            op1.Filter = "Pdf Files|*.pdf|all files|*.*"; ////"jpeg|*.jpg|bmp|*.bmp|all files|*.*"
            //op1.ShowDialog();
            //op1.Multiselect = true;
            if (op1.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = op1.FileName;
                grdList.Visible = false;
                pnlDistrbutor.Visible = false;
                pnlAddNew.Visible = true;
            }

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(txtFileName.Text);
        }
        private void showDistributePanel(string users)
        {
            pnlDistrbutor.Visible = true;
            grdList.Enabled = false;
            btnPickFile.Enabled = false;
            txtEmployees.Text = users;

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            TapalPickDB tpdb = new TapalPickDB();
            prevtapal.Date = UpdateTable.getSQLDateTime();
            try
            {
                if (list.Count != 0)
                {
                    if (tpdb.insertTapalDistribution(list, prevtapal))
                    {
                        string menuID = getMenuID();
                        try 
                        {
                            string toAddress = "";
                            toAddress = ERPUserDB.getemailIDs(list, menuID);
                            //create emaildata
                            if (toAddress.Trim().Length > 0)
                            {
                                EmailDataDB emdataDB = new EmailDataDB();
                                emaildata emdata = new emaildata();
                                emdata.ToAddress = toAddress;
                                emdata.status = 0;
                                emdata.EmailData = "Tapal from " + prevtapal.ReceivedFrom + " is forwarded to you by " + Login.userLoggedInName;
                                emdata.Subject = "Tapal From " + prevtapal.ReceivedFrom;
                                emdataDB.insertEmailData(emdata);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        //
                        tpdb.updateTapalStatus(prevtapal);
                        pnlDistrbutor.Visible = false;
                        grdList.Enabled = true;
                        btnPickFile.Enabled = true;
                        listtapal();
                    }
                    else
                    {
                        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error 1");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error 2");
            }
        }

        private void btnDstributionCancel_Click(object sender, EventArgs e)
        {
            clearData();
            grdList.Enabled = true;
            btnPickFile.Enabled = true;
            pnlDistrbutor.Visible = false;
        }

        private void pnlDistrbutor_Paint(object sender, PaintEventArgs e)
        {

        }
        private string getMenuID()
        {
            string menuID = "";
            try
            {
                foreach (Control p in Controls["pnlUI"].Controls)
                {
                    if (p.GetType() == typeof(Label))
                    {
                        Label c = (Label)p;
                        if (c.Name == "MenuItemID")
                        {
                            menuID = p.Text;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return menuID;
        }

        private void TapalPick_Enter(object sender, EventArgs e)
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


