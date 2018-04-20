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
using CSLERP.PrintForms;
using CSLERP.FileManager;
using Microsoft.Office.Interop.Excel;

namespace CSLERP
{
    public partial class CustomerOB : System.Windows.Forms.Form
    {
        string docID = "CUSTOMEROB";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        CustomerOBheader preacchdr;
        Panel pnlForwarder = new Panel();
        Panel pnlModel = new System.Windows.Forms.Panel();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        System.Windows.Forms.TextBox txtSearch;
        ListView lvCopy = new ListView();
        ListView exlv = new ListView();
        Form frmPopup = new Form();
        Timer filterTimer = new Timer();
        DataGridView grdCustList = new DataGridView();
        public CustomerOB()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void CustomerOB_Load(object sender, EventArgs e)
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
            FilteredCstOBList(listOption);
            //applyPrivilege();
        }
        private void FilteredCstOBList(int opt)
        {
            try
            {
                grdList.Rows.Clear();
                CustomerOBDB AccDB = new CustomerOBDB();
                List<CustomerOBheader> AccList = CustomerOBDB.getCustomerOblist(opt);
                if (opt == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                //else if (opt == 2)
                //    lblActionHeader.Text = "List of Approved Documents";
                else if (opt == 2 || opt == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (CustomerOBheader accob in AccList)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = accob.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentNo"].Value = accob.DocumentNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentDate"].Value = accob.DocumentDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["FYID"].Value = accob.FYID;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = accob.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = accob.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = accob.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = accob.CreatorName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;

            try
            {
                grdList.Columns["gCreateUser"].Visible = true;
                grdList.Columns["Creator"].Visible = true;
            }
            catch (Exception ex)
            {
            }
        }

        private void initVariables()
        {

            if (getuserPrivilegeStatus() == 1)
            {
                //user is only a viewer
                listOption = 6;
            }
            docID = Main.currentDocument;
            dtDocDate.Format = DateTimePickerFormat.Custom;
            dtDocDate.CustomFormat = "dd-MM-yyyy";
            dtDocDate.Enabled = false;
            FinancialYearDB.fillFYIDCombo(cmdFYID);
            ////dtSMRNHeaderDate.Enabled = true;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            ///userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            ///userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
        }
        private void closeAllPanels()
        {
            try
            {
                pnlList.Visible = false;
                pnlAddEdit.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        public void clearData()
        {
            try
            {
                grdCustomerOB.Rows.Clear();
                //----------clear temperory panels
                //      clearTabPageControls();
                pnlForwarder.Visible = false;
                dtDocDate.Value = DateTime.Parse("01-01-1900");
                txtDocNo.Text = "";
                cmdFYID.SelectedIndex = -1;
                //  btnSelectStockItemID.Enabled = true;
                handletab();
                preacchdr = new CustomerOBheader();
                removeControlsFromPnlLvPanel();
            }
            catch (Exception ex)
            {

            }
        }
        void handletab()
        {
            cmdFYID.Enabled = true;
            btnAddLine.Enabled = true;
            txtcredittotal.Enabled = true;
            txtdebittotal.Enabled = true;
            grdCustomerOB.Enabled = true;
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                clearData();
                btnSave.Text = "Save";
                pnlAddEdit.Visible = true;
                closeAllPanels();
                txtdebittotal.Visible = false;
                txtcredittotal.Visible = false;
                pnlAddEdit.Visible = true;
                cmdFYID.SelectedValue = -1;
                setButtonVisibility("btnNew");
            }
            catch (Exception)
            {

            }
        }
        private void btnClearEntries_Click(object sender, EventArgs e)
        {
            grdCustomerOB.Rows.Clear();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }
        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                AddCustomerOBDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private Boolean AddCustomerOBDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdCustomerOB.Rows.Count > 0)
                {
                    if (!verifyAndReworkCstGridRows())
                    {
                        return false;
                    }
                }
                grdCustomerOB.Rows.Add();
                int kount = grdCustomerOB.RowCount;
                // grdProductTestDetail.Rows[kount - 1].Cells["AccLineNo"].Value = kount;
                grdCustomerOB.Rows[kount - 1].Cells["AccSerialNo"].Value = kount;
                grdCustomerOB.Rows[kount - 1].Cells["AccCustomerCode"].Value = "";
                grdCustomerOB.Rows[kount - 1].Cells["AccCustomerName"].Value = "";
                grdCustomerOB.Rows[kount - 1].Cells["AccBalanceDebit"].Value = 0;
                grdCustomerOB.Rows[kount - 1].Cells["AccBalanceCredit"].Value = 0;
                grdCustomerOB.FirstDisplayedScrollingRowIndex = grdCustomerOB.RowCount - 1;
                grdCustomerOB.CurrentCell = grdCustomerOB.Rows[kount - 1].Cells[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddCustomerDetailRow() : Error");
            }

            return status;
        }

        private Boolean verifyAndReworkCstGridRows()
        {
            decimal crTotal = 0;
            decimal dbTotal = 0;
            Boolean status = true;

            try
            {
                if (grdCustomerOB.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Customer details");
                    txtcredittotal.Visible = false;
                    txtdebittotal.Visible = false;
                    return false;
                }
                for (int i = 0; i < grdCustomerOB.Rows.Count; i++)
                {
                    if (((Convert.ToDecimal(grdCustomerOB.Rows[i].Cells["AccBalanceDebit"].Value) != 0) &&
                        (Convert.ToDecimal(grdCustomerOB.Rows[i].Cells["AccBalanceCredit"].Value) != 0)) ||
                            ((Convert.ToDecimal(grdCustomerOB.Rows[i].Cells["AccBalanceDebit"].Value) == 0) &&
                        (Convert.ToDecimal(grdCustomerOB.Rows[i].Cells["AccBalanceCredit"].Value) == 0)))

                    {
                        MessageBox.Show("Enter either Debrit or Credit Row:" + (i + 1));
                        return false;
                    }
                    grdCustomerOB.Rows[i].Cells["AccSerialNo"].Value = (i + 1);
                    if ((grdCustomerOB.Rows[i].Cells["AccCustomerCode"].Value.ToString().Trim().Length == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                    crTotal += (Convert.ToDecimal(grdCustomerOB.Rows[i].Cells["AccBalanceCredit"].Value));
                    dbTotal += (Convert.ToDecimal(grdCustomerOB.Rows[i].Cells["AccBalanceDebit"].Value));
                }
                txtcredittotal.Text = crTotal.ToString();
                txtdebittotal.Text = dbTotal.ToString();
                txtcredittotal.Visible = true;
                txtdebittotal.Visible = true;
            }

            catch (Exception ex)
            {
                return false;
            }
            return status;
        }
        private List<CustomerOBdetail> getCustomerOBDetailList(CustomerOBheader Acchr)
        {
            List<CustomerOBdetail> CSOBDetails = new List<CustomerOBdetail>();
            try
            {
                CustomerOBdetail csdb = new CustomerOBdetail();
                for (int i = 0; i < grdCustomerOB.Rows.Count; i++)
                {
                    csdb = new CustomerOBdetail();
                    csdb.DocumentID = Acchr.DocumentID;
                    csdb.DocumentNo = Acchr.DocumentNo;
                    csdb.DocumentDate = Acchr.DocumentDate;
                    csdb.FYID = Acchr.FYID;
                    csdb.AccountCode = grdCustomerOB.Rows[i].Cells["AccCustomerCode"].Value.ToString();
                    csdb.AccountName = grdCustomerOB.Rows[i].Cells["AccCustomerName"].Value.ToString();
                    csdb.SerialNo = Convert.ToInt32(grdCustomerOB.Rows[i].Cells["AccSerialNo"].Value.ToString());
                    if (grdCustomerOB.Rows[i].Cells["AccBalanceDebit"].Value != null)
                    {
                        csdb.BalanceDebit = Convert.ToDecimal(grdCustomerOB.Rows[i].Cells["AccBalanceDebit"].Value.ToString());
                    }
                    if (grdCustomerOB.Rows[i].Cells["AccBalanceCredit"].Value != null)
                    {
                        csdb.BalanceCredit = Convert.ToDecimal(grdCustomerOB.Rows[i].Cells["AccBalanceCredit"].Value.ToString());
                    }
                    CSOBDetails.Add(csdb);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("getCustomerOBDetailList() : Error getting CustomerOB Detail grid Values");
                CSOBDetails = null;
            }
            return CSOBDetails;
        }
        private void grdProductTestDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdCustomerOB.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("AccDelete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdCustomerOB.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkCstGridRows();
                    }
                    if (columnName.Equals("AccSelect"))
                    {
                        showCustomerGridview();
                    }
                }
                catch (Exception)
                {

                }
            }
            catch (Exception ex)
            {

            }
        }
        //-------------
        private void showCustomerGridview()
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

                frmPopup.Size = new Size(700, 370);


                System.Windows.Forms.Label lblSearch = new System.Windows.Forms.Label();
                lblSearch.Location = new System.Drawing.Point(340, 10);
                lblSearch.Text = "Search by Name";
                lblSearch.AutoSize = true;
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new System.Windows.Forms.TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(460, 8);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtCustSearch_TextChangedInEmpGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                CustomerOBDB custDB = new CustomerOBDB();
                grdCustList = custDB.getGridViewForCustomerDetailsList();

                grdCustList.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 30), new Size(700, 300));
                frmPopup.Controls.Add(grdCustList);

                System.Windows.Forms.Button lvOK = new System.Windows.Forms.Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 340);
                lvOK.Click += new System.EventHandler(this.grdCustOK_Click1);
                frmPopup.Controls.Add(lvOK);

                System.Windows.Forms.Button lvCancel = new System.Windows.Forms.Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 340);
                lvCancel.Click += new System.EventHandler(this.grdCustCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdCustOK_Click1(object sender, EventArgs e)
        {
            try
            {
                var checkedRows = from DataGridViewRow r in grdCustList.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("Select one Customer");
                    return;
                }
                foreach (var row in checkedRows)
                {
                    if (validateItems(row.Cells["CustomerID"].Value.ToString()))
                    {
                        grdCustomerOB.CurrentRow.Cells["AccCustomerCode"].Value = row.Cells["CustomerID"].Value.ToString();
                        grdCustomerOB.CurrentRow.Cells["AccCustomerName"].Value = row.Cells["CustomerName"].Value.ToString();
                    }
                    //txtCustomer.Text = row.Cells["CustomerID"].Value.ToString() + "-" +
                    //            Environment.NewLine + row.Cells["CustomerName"].Value.ToString();
                }

                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        private void grdCustCancel_Click1(object sender, EventArgs e)
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
        private void txtCustSearch_TextChangedInEmpGridList(object sender, EventArgs e)
        {
            try
            {
                filterTimer.Enabled = false;
                filterTimer.Stop();
                filterTimer.Tick -= new System.EventHandler(this.handlefilterTimerTimeout);
                filterTimer.Tick += new System.EventHandler(this.handlefilterTimerTimeout);
                filterTimer.Interval = 500;
                filterTimer.Enabled = true;
                filterTimer.Start();

            }
            catch (Exception ex)
            {

            }
        }
        private void handlefilterTimerTimeout(object sender, EventArgs e)
        {
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterCustGridData();
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }
        private void filterCustGridData()
        {
            try
            {
                grdCustList.CurrentCell = null;
                foreach (DataGridViewRow row in grdCustList.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdCustList.Rows)
                    {
                        if (!row.Cells["CustomerName"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
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
        //-------------
        //private void ShowCstListView()
        //{
        //    //removeControlsFromPnlLvPanel();
        //    //pnllv = new Panel();
        //    //pnllv.BorderStyle = BorderStyle.FixedSingle;
        //    //pnllv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(100, 100), new Size(600, 300));
        //    frmPopup = new Form();
        //    frmPopup.StartPosition = FormStartPosition.CenterScreen;
        //    frmPopup.BackColor = Color.CadetBlue;

        //    frmPopup.MaximizeBox = false;
        //    frmPopup.MinimizeBox = false;
        //    frmPopup.ControlBox = false;
        //    frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

        //    frmPopup.Size = new Size(450, 300);
        //    lv = CustomerDB.getCustomerListView();
        //    lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
        //    //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView3_ItemChecked);
        //    lv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new Size(450, 250));
        //    frmPopup.Controls.Add(lv);

        //    System.Windows.Forms.Button lvOK = new System.Windows.Forms.Button();
        //    lvOK.BackColor = Color.Tan;
        //    lvOK.Text = "OK";
        //    lvOK.Location = new System.Drawing.Point(44, 265);
        //    lvOK.Click += new System.EventHandler(this.lvOK_Click2);
        //    frmPopup.Controls.Add(lvOK);

        //    System.Windows.Forms.Button lvCancel = new System.Windows.Forms.Button();
        //    lvCancel.Text = "CANCEL";
        //    lvCancel.BackColor = Color.Tan;
        //    lvCancel.Location = new System.Drawing.Point(141, 265);
        //    lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
        //    frmPopup.Controls.Add(lvCancel);

        //    System.Windows.Forms.Label lblSearch = new System.Windows.Forms.Label();
        //    lblSearch.Text = "Search";
        //    lblSearch.Location = new System.Drawing.Point(250, 267);
        //    lblSearch.Size = new Size(45, 15);
        //    frmPopup.Controls.Add(lblSearch);

        //    txtSearch = new System.Windows.Forms.TextBox();
        //    txtSearch.Location = new System.Drawing.Point(300, 265);
        //    txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
        //    frmPopup.Controls.Add(txtSearch);
        //    frmPopup.ShowDialog();

        //    //pnlAddEdit.Controls.Add(pnllv);
        //    //pnllv.BringToFront();
        //    //pnllv.Visible = true;
        //}
        //private void lvOK_Click2(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        //btnSelect.Enabled = true;
        //        //pnlEditButtons.Enabled = true;
        //        if (lv.Visible == true)
        //        {
        //            if (lv.CheckedIndices.Count == 0)
        //            {
        //                MessageBox.Show("Select one Customer.");
        //                return;
        //            }
        //            if (lv.CheckedIndices.Count > 1)
        //            {
        //                MessageBox.Show("Not allowed to select more than one Customer.");
        //                return;
        //            }
        //            foreach (ListViewItem itemRow in lv.Items)
        //            {
        //                if (itemRow.Checked)
        //                {

        //                    if (validateItems(itemRow.SubItems[1].Text))
        //                    {
        //                        grdCustomerOB.CurrentRow.Cells["AccCustomerCode"].Value = itemRow.SubItems[1].Text;
        //                        grdCustomerOB.CurrentRow.Cells["AccCustomerName"].Value = itemRow.SubItems[2].Text;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (lvCopy.CheckedIndices.Count == 0)
        //            {
        //                MessageBox.Show("Select one Customer.");
        //                return;
        //            }
        //            if (lvCopy.CheckedIndices.Count > 1)
        //            {
        //                MessageBox.Show("Not allowed to select more than one Customer.");
        //                return;
        //            }
        //            foreach (ListViewItem itemRow in lvCopy.Items)
        //            {
        //                if (itemRow.Checked)
        //                {
        //                    if (validateItems(itemRow.SubItems[1].Text))
        //                    {
        //                        grdCustomerOB.CurrentRow.Cells["AccCustomerCode"].Value = itemRow.SubItems[1].Text;
        //                        grdCustomerOB.CurrentRow.Cells["AccCustomerName"].Value = itemRow.SubItems[2].Text;
        //                    }
        //                }
        //            }

        //        }
        //        frmPopup.Close();
        //        frmPopup.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private void lvCancel_Click2(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        frmPopup.Close();
        //        frmPopup.Dispose();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //private void LvColumnClick(object o, ColumnClickEventArgs e)
        //{
        //    try
        //    {
        //        if (lv.Visible == true)
        //        {
        //            string first = lv.Items[0].SubItems[e.Column].Text;
        //            string last = lv.Items[lv.Items.Count - 1].SubItems[e.Column].Text;
        //            System.Windows.Forms.SortOrder sort_order1 = SortingListView.getSortedOrder(first, last);
        //            this.lv.ListViewItemSorter = new SortingListView(e.Column, sort_order1);
        //        }
        //        else
        //        {
        //            string first = lvCopy.Items[0].SubItems[e.Column].Text;
        //            string last = lvCopy.Items[lvCopy.Items.Count - 1].SubItems[e.Column].Text;
        //            System.Windows.Forms.SortOrder sort_order1 = SortingListView.getSortedOrder(first, last);
        //            this.lvCopy.ListViewItemSorter = new SortingListView(e.Column, sort_order1);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Sorting error");
        //    }
        //}
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
        private Boolean validateItems(string item)
        {
            Boolean status = true;
            try
            {
                foreach (DataGridViewRow row in grdCustomerOB.Rows)
                {
                    if (grdCustomerOB.Rows[row.Index].Cells["AccCustomerCode"].Value.ToString() == item)
                    {
                        MessageBox.Show("code Duplication Not allowed");
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        private Boolean validateItemsmain(string item)
        {
            Boolean status = true;
            try
            {
                foreach (DataGridViewRow row in grdList.Rows)
                {
                    if (grdList.Rows[row.Index].Cells["FYID"].Value.ToString() == item)
                    {
                        MessageBox.Show("This year already has Customer List");
                    }
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        string lmn = "";
        private void btnSave_Click(object sender, EventArgs e)
        {

            Boolean status = true;
            try
            {
                CustomerOBDB AccDB = new CustomerOBDB();
                CustomerOBheader acclist = new CustomerOBheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;
                if (!verifyAndReworkCstGridRows())
                {
                    MessageBox.Show("Error found IN CustomerOb details. Please correct before updating the details");
                    return;
                }
                try
                {
                    acclist.DocumentID = docID;
                    acclist.FYID = cmdFYID.SelectedItem.ToString().Trim().Substring(0, cmdFYID.SelectedItem.ToString().Trim().IndexOf(':')).Trim();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    status = false;
                    return;
                }
                //if (btnText.Equals("Save"))
                //{
                //    acclist.DocumentNo = DocumentNumberDB.getNewNumber(docID, 1);
                //    acclist.DocumentDate = UpdateTable.getSQLDateTime();
                //}
                //else
                //{
                //    acclist.DocumentNo = Convert.ToInt32(txtDocNo.Text);
                //    acclist.DocumentDate = preacchdr.DocumentDate;
                //}
                ////if (Convert.ToDecimal(txtcredittotal.Text) != Convert.ToDecimal(txtdebittotal.Text))
                ////{
                ////    MessageBox.Show("Debit and Credit total should be equal.");
                ////    return;
                ////}
                if (!AccDB.validateAccOB(acclist))
                {
                    MessageBox.Show("Validation Failed");
                    status = false;
                    return;
                }

                if (btnText.Equals("Save"))
                {
                    if (AccDB.checkFYID(acclist.FYID))
                    {
                        acclist.DocumentDate = UpdateTable.getSQLDateTime();
                        List<CustomerOBdetail> CSOBDetails = getCustomerOBDetailList(acclist);
                        if (AccDB.InsertCustHeaderAndDetail(acclist, CSOBDetails))
                        {
                            MessageBox.Show("Details Added");
                            closeAllPanels();
                            listOption = 1;
                            FilteredCstOBList(listOption);
                            pnlAddEdit.Visible = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Document Exist For this Financial year");
                        status = false;
                    }
                    if (!status)
                    {
                        MessageBox.Show("Failed to Insert");
                        status = false;
                    }
                }
                else if (btnText.Equals("Update"))
                {
                    acclist.DocumentNo = Convert.ToInt32(txtDocNo.Text);
                    acclist.DocumentDate = preacchdr.DocumentDate;
                    List<CustomerOBdetail> CSOBDetails = getCustomerOBDetailList(acclist);
                    if (AccDB.updateCustHeaderAndDetail(acclist, CSOBDetails))
                    {
                        MessageBox.Show(" details updated");
                        closeAllPanels();
                        listOption = 1;
                        FilteredCstOBList(listOption);
                    }
                    else
                    {
                        status = false;
                    }
                    if (!status)
                    {
                        MessageBox.Show("Failed to update");
                    }
                }
                else
                {
                    MessageBox.Show("Validation failed");
                    status = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Validation failed");
                status = false;
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }
        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Finalize") || columnName.Equals("Edit") || columnName.Equals("View"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    preacchdr = new CustomerOBheader();
                    preacchdr.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    preacchdr.DocumentNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentNo"].Value.ToString());
                    preacchdr.DocumentDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["DocumentDate"].Value.ToString());
                    preacchdr.FYID = grdList.Rows[e.RowIndex].Cells["FYID"].Value.ToString();
                    txtDocNo.Text = preacchdr.DocumentNo.ToString();
                    dtDocDate.Value = preacchdr.DocumentDate;
                    cmdFYID.SelectedIndex = cmdFYID.FindString(preacchdr.FYID);
                    dtDocDate.Value = preacchdr.DocumentDate;
                    List<CustomerOBdetail> Accdet = CustomerOBDB.getCustomerOBDetail(preacchdr);
                    grdCustomerOB.Rows.Clear();
                    int i = 0;
                    foreach (CustomerOBdetail det in Accdet)
                    {
                        if (!AddCustomerOBDetailRow())
                        {
                            MessageBox.Show("Error found in Customer Detail. Please correct before updating the details");
                        }
                        else
                        {
                            grdCustomerOB.Rows[i].Cells["AccSerialNo"].Value = det.SerialNo;
                            grdCustomerOB.Rows[i].Cells["AccCustomerCode"].Value = det.AccountCode;
                            grdCustomerOB.Rows[i].Cells["AccCustomerName"].Value = det.AccountName;
                            grdCustomerOB.Rows[i].Cells["AccBalanceDebit"].Value = det.BalanceDebit;
                            grdCustomerOB.Rows[i].Cells["AccBalanceCredit"].Value = det.BalanceCredit;
                            i++;
                            txtcredittotal.Text = (Convert.ToDecimal(txtcredittotal.Text) + Convert.ToDecimal(det.BalanceCredit)).ToString();
                            txtdebittotal.Text = (Convert.ToDecimal(txtdebittotal.Text) + Convert.ToDecimal(det.BalanceDebit)).ToString();
                        }
                    }

                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void pnlUI_Paint(object sender, PaintEventArgs e)
        {
        }
        private void btnCancel_Click_2(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }
        private void pnlAddEdit_Paint(object sender, PaintEventArgs e)
        {
        }
        private void btnFinalize_Click(object sender, EventArgs e)
        {
            try
            {
                CustomerOBDB CustDB = new CustomerOBDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Finalize the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (CustDB.FinalizeCustomerOB(preacchdr))
                    {
                        MessageBox.Show(" Document Finalized");
                        closeAllPanels();
                        listOption = 1;
                        FilteredCstOBList(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                    else
                        MessageBox.Show("Failed to Finalize");
                }
            }
            catch (Exception)
            {
            }
        }
        private void setButtonVisibility(string btnName)
        {
            try
            {
                btnActionPending.Visible = true;
                btnApproved.Visible = true;
                btnNew.Visible = false;
                btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                btnFinalize.Visible = false;
                disableTabPages();
                handleNewButton();
                handleGrdViewButton();
                handleGrdEditButton();
                pnlBottomButtons.Visible = true;
                //----
                if (btnName == "init")
                {
                    ////btnNew.Visible = true; 24/11/2016
                    btnExit.Visible = true;
                }
                else if (btnName == "btnNew")
                {
                    btnNew.Visible = false; //added 24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    enableTabPages();
                    tabControl1.SelectedTab = tabProductTestHeader;
                }
                else if (btnName == "btnEditPanel") //called from cancel,save,forward,approve and reverse button events
                {
                    ////btnNew.Visible = true;
                    btnExit.Visible = true;
                }
                else if (btnName == "Finalize")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnFinalize.Visible = true;
                    disableTabPages();
                    tabControl1.SelectedTab = tabProductTestHeader;
                }
                else if (btnName == "View")
                {
                    enableTabPages();
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    dtDocDate.Enabled = false;
                    txtDocNo.Enabled = false;
                    cmdFYID.Enabled = false;
                    btnAddLine.Enabled = false;
                    txtcredittotal.Enabled = false;
                    txtdebittotal.Enabled = false;
                    grdCustomerOB.Enabled = false;
                    btnExporttoExcel.Enabled = true;
                    //  disableTabPages();
                }
                else if (btnName == "Edit")
                {
                    cmdFYID.Enabled = false;
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    enableTabPages();
                }
                pnlEditButtons.Refresh();
                int ups = getuserPrivilegeStatus();
                if (ups == 1 || ups == 0)
                {
                    grdList.Columns["Finalize"].Visible = false;
                    btnActionPending.Visible = false;
                    btnApproved.Visible = false;
                    if (ups == 1)
                    {
                        grdList.Columns["View"].Visible = true;
                    }
                    else
                    {
                        grdList.Columns["View"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        void handleGrdViewButton()
        {
            grdList.Columns["View"].Visible = false;
            //if any one of view,add and edit
            if (Main.itemPriv[0] || Main.itemPriv[1] || Main.itemPriv[2])
            {
                //list option 1 should not have view buttuon visible (edit is avialable)
                if (listOption != 1)
                {
                    grdList.Columns["View"].Visible = true;
                }
            }
        }
        void handleNewButton()
        {
            btnNew.Visible = false;
            if (Main.itemPriv[1])
            {
                btnNew.Visible = true;
            }
        }
        void handleGrdEditButton()
        {
            grdList.Columns["Edit"].Visible = false;
            grdList.Columns["Finalize"].Visible = false;
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                if (listOption == 1)
                {
                    grdList.Columns["Edit"].Visible = true;
                    grdList.Columns["Finalize"].Visible = true;
                }
            }
        }
        int getuserPrivilegeStatus()
        {
            try
            {
                if (Main.itemPriv[0] && !Main.itemPriv[1] && !Main.itemPriv[2]) //only view
                    return 1;
                else if (Main.itemPriv[0] && (Main.itemPriv[1] || Main.itemPriv[2])) //view add and edit
                    return 2;
                else if (!Main.itemPriv[0] && (Main.itemPriv[1] || Main.itemPriv[2])) //view add and edit
                    return 3;
                else if (!Main.itemPriv[0] && !Main.itemPriv[1] || !Main.itemPriv[2]) //no privilege
                    return 0;
                else
                    return -1;
            }
            catch (Exception ex)
            {
            }
            return 0;
        }

        private void disableTabPages()
        {
            foreach (TabPage tp in tabControl1.TabPages)
            {
                tp.Enabled = false; ;
            }
        }
        private void enableTabPages()
        {
            foreach (TabPage tp in tabControl1.TabPages)
            {
                tp.Enabled = true;
            }
        }
        private void btnListDocuments_Click_1(object sender, EventArgs e)
        {

        }

        private void btnCloseDocument_Click(object sender, EventArgs e)
        {
            //removePDFControls();
            //showPDFFileGrid();
        }
        //private void showPDFFile(string fname)
        //{
        //    try
        //    {
        //        removePDFControls();
        //        AxAcroPDFLib.AxAcroPDF pdf = new AxAcroPDFLib.AxAcroPDF();
        //        pdf.Dock = System.Windows.Forms.DockStyle.Fill;
        //        pdf.Enabled = true;
        //        pdf.Location = new System.Drawing.Point(0, 0);
        //        pdf.Name = "pdfReader";
        //        pdf.OcxState = pdf.OcxState;
        //        ////pdf.OcxState = ((System.Windows.Forms.AxHost.State)(new System.ComponentModel.ComponentResourceManager(typeof(ViewerWindow)).GetObject("pdf.OcxState")));
        //        pdf.TabIndex = 1;

        //        pdf.setShowToolbar(false);
        //        pdf.LoadFile(fname);
        //        pdf.setView("Fit");
        //        pdf.Visible = true;
        //        pdf.setZoom(100);
        //        pdf.setPageMode("None");
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //private void btnPDFClose_Click(object sender, EventArgs e)
        //{
        //    removePDFControls();
        //    showPDFFileGrid();
        //}
        //private void removePDFControls()
        //{
        //    try
        //    {
        //        foreach (Control p in pnlPDFViewer.Controls)
        //            if (p.GetType() == typeof(AxAcroPDFLib.AxAcroPDF))
        //            {
        //                AxAcroPDFLib.AxAcroPDF c = (AxAcroPDFLib.AxAcroPDF)p;
        //                c.Visible = false;
        //                pnlPDFViewer.Controls.Remove(c);
        //                c.Dispose();
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //private void showPDFFileGrid()
        //{
        //    try
        //    {
        //        foreach (Control p in pnlPDFViewer.Controls)
        //            if (p.GetType() == typeof(DataGridView))
        //            {
        //                p.Visible = true;
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //private void removePDFFileGridView()
        //{
        //    try
        //    {
        //        foreach (Control p in pnlPDFViewer.Controls)
        //            if (p.GetType() == typeof(DataGridView))
        //            {
        //                p.Dispose();
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        private void dgvDocumentList_CellContentClick(Object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    DataGridView dgv = sender as DataGridView;
            //    string fileName = "";
            //    if (e.RowIndex < 0)
            //        return;
            //    if (e.ColumnIndex == 0)
            //    {
            //        removePDFControls();
            //        fileName = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            //        ////string docDir = Main.documentDirectory + "\\" + docID;
            //        string subDir = prevsptheader.TemplateNo + "-" + prevsptheader.TemplateDate.ToString("yyyyMMddhhmmss");
            //        DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
            //        fileName = DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
            //        ////showPDFFile(fileName);
            //        ////dgv.Visible = false;
            //        System.Diagnostics.Process.Start(fileName);
            //    }

            // } 
            //catch (Exception ex)
            //{
            //}
        }


        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            FilteredCstOBList(listOption);
        }

        private void btnApproved_Click(object sender, EventArgs e)
        {
            listOption = 2;
            FilteredCstOBList(listOption);
        }
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
        private void removeControlsFromForwarderPanelTV()
        {
            try
            {
                //foreach (Control p in pnlForwarder.Controls)
                //    if (p.GetType() == typeof(TreeView) || p.GetType() == typeof(System.Windows.Forms.Button))
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
        // private void btnSelectSMRNHeaderNo_Click(object sender, EventArgs e)
        //{
        //    if (grdProductTestDetail.Rows.Count != 0)
        //    {
        //        DialogResult dialog = MessageBox.Show("Grid Detail will be removed", "Yes", MessageBoxButtons.YesNo);
        //        if (dialog == DialogResult.Yes)
        //        {
        //            grdProductTestDetail.Rows.Clear();
        //        }
        //        else
        //            return;
        //    }
        //    removeControlsFromForwarderPanelTV();
        //    tv = new TreeView();
        //    tv.CheckBoxes = true;
        //    tv.Nodes.Clear();
        //    tv.CheckBoxes = true;
        //    pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
        //    pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));

        //    tv = StockItemDB.getStockItemTreeView();
        //    tv.Bounds = new Rectangle(new Point(50, 50), new Size(600, 200));
        //    pnlForwarder.Controls.Remove(tv);
        //    pnlForwarder.Controls.Add(tv);
        //    //tv.cl
        //    tv.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterCheck);
        //    Button lvForwrdOK = new Button();
        //    lvForwrdOK.Text = "OK";
        //    lvForwrdOK.Size = new Size(150, 20);
        //    lvForwrdOK.Location = new Point(50, 270);
        //    lvForwrdOK.Click += new System.EventHandler(this.tvOK_Click);
        //    pnlForwarder.Controls.Add(lvForwrdOK);

        //    Button lvForwardCancel = new Button();
        //    lvForwardCancel.Text = "Cancel";
        //    lvForwardCancel.Size = new Size(150, 20);
        //    lvForwardCancel.Location = new Point(250, 270);
        //    lvForwardCancel.Click += new System.EventHandler(this.tvCancel_Click);
        //    pnlForwarder.Controls.Add(lvForwardCancel);
        //    ////lvForwardCancel.Visible = false;
        //    //tv.CheckBoxes = true;
        //    pnlForwarder.Visible = true;
        //    pnlAddEdit.Controls.Add(pnlForwarder);
        //    pnlAddEdit.BringToFront();
        //    pnlForwarder.BringToFront();
        //    pnlForwarder.Focus();
        // }

        private void tvOK_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    List<string> ItemList = GetCheckedNodes(tv.Nodes);
            //    if (ItemList.Count > 1 || ItemList.Count == 0)
            //    {
            //        MessageBox.Show("select only one item");
            //        return;
            //    }
            //    foreach (string s in ItemList)
            //    {

            //      //  txtStockItemId.Text = s;
            //        tv.CheckBoxes = true;
            //        pnlForwarder.Controls.Remove(lvApprover);
            //        pnlForwarder.Visible = false;
            //        showModelListView(s);
            //    }

            //}
            //catch (Exception)
            //{
            //}
        }
        //public List<string> GetCheckedNodes(TreeNodeCollection nodes)
        //{
        //    List<string> nodeList = new List<string>();
        //    try
        //    {

        //        if (nodes == null)
        //        {
        //            return nodeList;
        //        }

        //        foreach (TreeNode childNode in nodes)
        //        {
        //            if (childNode.Checked)
        //            {
        //                nodeList.Add(childNode.Text);
        //            }
        //            nodeList.AddRange(GetCheckedNodes(childNode.Nodes));
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return nodeList;
        //}
        //private void tvCancel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        tv.CheckBoxes = true;
        //        pnlForwarder.Controls.Remove(lvApprover);
        //        pnlForwarder.Visible = false;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //private void tv_AfterCheck(object sender, TreeViewEventArgs e)
        //{
        //    if (e.Node.Checked == true)
        //    {
        //        if (e.Node.Nodes.Count != 0)
        //        {
        //            MessageBox.Show("you are not allowed to select group");
        //            e.Node.Checked = false;
        //        }
        //    }
        ////}
        //private void showModelListView(string stockID)
        //{
        //    removeControlsFromModelPanel();
        //    lv = new ListView();
        //    lv.CheckBoxes = true;
        //    lv.Items.Clear();
        //    pnlModel.BorderStyle = BorderStyle.FixedSingle;
        //    pnlModel.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
        //    Label lbl = new Label();
        //    lbl.AutoSize = true;
        //    lbl.Location = new Point(70, 17);
        //    lbl.Size = new Size(35, 13);
        //    lbl.Text = "ListView For Model";
        //    lbl.Font = new Font("Serif", 10, FontStyle.Bold);
        //    lbl.ForeColor = Color.Green;
        //    pnlModel.Controls.Add(lbl);
        //    lv = ProductModelsDB.getModelsForProductListView(stockID.Substring(0, stockID.IndexOf('-')));
        //    if (lv.Items.Count == 0)
        //    {
        //        //txtModelNo.Text = "NA";
        //        //txtModelName.Text = "NA";
        //        pnlModel.Visible = false;
        //        return;
        //    }
        //    lv.Bounds = new Rectangle(new Point(50, 50), new Size(600, 200));
        //    pnlModel.Controls.Remove(lv);
        //    pnlModel.Controls.Add(lv);
        //    Button lvOK = new Button();
        //    lvOK.Text = "OK";
        //    lvOK.Size = new Size(150, 20);
        //    lvOK.Location = new Point(50, 270);
        //    lvOK.Click += new System.EventHandler(this.lvOK_Click3);
        //    pnlModel.Controls.Add(lvOK);

        //    Button lvCancel = new Button();
        //    lvCancel.Text = "Cancel";
        //    lvCancel.Size = new Size(150, 20);
        //    lvCancel.Location = new Point(250, 270);
        //    lvCancel.Click += new System.EventHandler(this.lvCancel_Click3);
        //    pnlModel.Controls.Add(lvCancel);
        //    pnlModel.Visible = true;
        //    pnlAddEdit.Controls.Add(pnlModel);
        //    pnlAddEdit.BringToFront();
        //    pnlModel.BringToFront();
        //    pnlModel.Focus();
        //}
        //private void lvOK_Click3(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int count = 0;
        //        foreach (ListViewItem item in lv.Items)
        //        {
        //            if (item.Checked == true)
        //            {
        //                count++;
        //            }
        //        }
        //        if (count != 1)
        //        {
        //            MessageBox.Show("select one item");
        //            return;
        //        }
        //        foreach (ListViewItem item in lv.Items)
        //        {
        //            //if (item.Checked == true)
        //            //{
        //            //    string id = txtStockItemId.Text.Substring(0, txtStockItemId.Text.IndexOf('-'));
        //            //    if (ProductTestTemplateDB.checkProductAvailability(id, item.SubItems[1].Text))
        //            //    {
        //            //        txtModelNo.Text = item.SubItems[1].Text;
        //            //        txtModelName.Text = item.SubItems[2].Text;
        //            //        pnlModel.Controls.Remove(lv);
        //            //        pnlModel.Visible = false;
        //            //    }
        //           // }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //private void lvCancel_Click3(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        lv.CheckBoxes = true;
        //        pnlModel.Controls.Remove(lv);
        //        pnlModel.Visible = false;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //private void removeControlsFromModelPanel()
        //{
        //    try
        //    {
        //        foreach (Control p in pnlModel.Controls)
        //            if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
        //            {
        //                p.Dispose();
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //private void btnListDocuments_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //removePDFFileGridView();
        //        //removePDFControls();
        //        //DataGridView dgvDocumentList = new DataGridView();
        //        //pnlPDFViewer.Controls.Remove(dgvDocumentList);
        //        //dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevsptheader.TemplateNo + "-" + prevsptheader.TemplateDate.ToString("yyyyMMddhhmmss"));
        //        //pnlPDFViewer.Controls.Add(dgvDocumentList);
        //        //dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        private void btnCloseDocument_Click_1(object sender, EventArgs e)
        {
            //removePDFControls();
            //showPDFFileGrid();
        }
        private void tabPSReport_Click(object sender, EventArgs e)
        {

        }

        private void tabProductTestDetail_Click(object sender, EventArgs e)
        {

        }

        private void txtAccCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtBalDebit_TextChanged(object sender, EventArgs e)
        {
            //  txtBalCredit.Enabled = false;
        }

        private void txtBalCredit_TextChanged(object sender, EventArgs e)
        {
            // txtBalDebit.Enabled = false;
        }

        private void grdProductTestDetail_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }
        //    private void sum(DataGridView dgv)
        //{
        //    decimal SumDebit = 0;
        //    decimal SumCredit = 0;
        //    for(int i=0;i<dgv.Rows.Count;i++)
        //    {
        //        SumDebit += Convert.ToDecimal(dgv.Rows[i].Cells["AccBalanceDebit"].Value);
        //        SumCredit += Convert.ToDecimal(dgv.Rows[i].Cells["AccBalanceCredit"].Value);
        //    }
        //    txtcredittotal.Visible = true;
        //    txtdebittotal.Visible = true;
        //    txtdebittotal.Text = SumDebit.ToString();
        //    txtcredittotal.Text =SumCredit.ToString();
        //}

        private void txtdebittotal_TextChanged(object sender, EventArgs e)
        {

        }
        //private void txtSearch_TextChanged(object sender, EventArgs e)
        //{
        //    frmPopup.Controls.Remove(lvCopy);
        //    addItems();
        //}
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
        //private void addItems()
        //{
        //    lvCopy = new ListView();
        //    lvCopy.View = System.Windows.Forms.View.Details;
        //    lvCopy.LabelEdit = true;
        //    lvCopy.AllowColumnReorder = true;
        //    lvCopy.CheckBoxes = true;
        //    lvCopy.FullRowSelect = true;
        //    lvCopy.GridLines = true;
        //    lvCopy.Sorting = System.Windows.Forms.SortOrder.Ascending;
        //    lvCopy.Columns.Add("Select", -2, HorizontalAlignment.Left);
        //    lvCopy.Columns.Add("Id", -2, HorizontalAlignment.Left);
        //    lvCopy.Columns.Add("Name", -2, HorizontalAlignment.Left);
        //    lvCopy.Sorting = System.Windows.Forms.SortOrder.None;
        //    lvCopy.ColumnClick += new ColumnClickEventHandler(LvColumnClick); ;
        //    //this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
        //    //lvCopy.Location = new Point(13, 9);
        //    //lvCopy.Size = new Size(440, 199);
        //    lvCopy.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new Size(450, 250));
        //    lvCopy.Items.Clear();
        //    foreach (ListViewItem row in lv.Items)
        //    {
        //        string x = row.SubItems[0].Text;
        //        string no = row.SubItems[1].Text;
        //        string ch = row.SubItems[2].Text;
        //        if (ch.ToLower().StartsWith(txtSearch.Text))
        //        {
        //            ListViewItem item = new ListViewItem();
        //            item.SubItems.Add(no);
        //            item.SubItems.Add(ch);
        //            item.Checked = false;
        //            lvCopy.Items.Add(item);
        //        }
        //    }
        //    lv.Visible = false;
        //    lvCopy.Visible = true;
        //    frmPopup.Controls.Add(lvCopy);
        //}

        private void btnExporttoExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (verifyAndReworkCstGridRows())
                {
                    //removeControlsFromPnlLvPanel();
                    //pnllv = new Panel();
                    //pnllv.BorderStyle = BorderStyle.FixedSingle;
                    //grdCustomerOB.Enabled = false;
                    //pnllv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(100, 100), new Size(600, 300));

                    frmPopup = new Form();
                    frmPopup.StartPosition = FormStartPosition.CenterScreen;
                    frmPopup.BackColor = Color.CadetBlue;

                    frmPopup.MaximizeBox = false;
                    frmPopup.MinimizeBox = false;
                    frmPopup.ControlBox = false;
                    frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                    frmPopup.Size = new Size(450, 310);
                    exlv = Utilities.GridColumnSelectionView(grdCustomerOB);
                    exlv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0,25), new Size(450, 250));
                    frmPopup.Controls.Add(exlv);

                    System.Windows.Forms.Label pnlHeading = new System.Windows.Forms.Label();
                    pnlHeading.Size = new Size(300, 20);
                    
                    pnlHeading.Location = new System.Drawing.Point(5, 5);
                    pnlHeading.Text = "Select Gridview Colums to Export";
                    pnlHeading.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                    pnlHeading.ForeColor = Color.Black;
                    frmPopup.Controls.Add(pnlHeading);

                    System.Windows.Forms.Button exlvOK = new System.Windows.Forms.Button();
                    exlvOK.Text = "OK";
                    exlvOK.BackColor = Color.Tan;
                    exlvOK.Location = new System.Drawing.Point(40, 280);
                    exlvOK.Click += new System.EventHandler(this.exlvOK_Click1);
                    frmPopup.Controls.Add(exlvOK);

                    System.Windows.Forms.Button exlvCancel = new System.Windows.Forms.Button();
                    exlvCancel.Text = "CANCEL";
                    exlvCancel.BackColor = Color.Tan;
                    exlvCancel.Location = new System.Drawing.Point(130, 280);
                    exlvCancel.Click += new System.EventHandler(this.exlvCancel_Click2);
                    frmPopup.Controls.Add(exlvCancel);

                    frmPopup.ShowDialog();
                    //pnlAddEdit.Controls.Add(pnllv);
                    //pnllv.BringToFront();
                    //pnllv.Visible = true;
                }
                else
                {
                    MessageBox.Show("Error in grid data. Export failed");
                }
            }
            catch (Exception ex)
            {
            }
        }


        private void exlvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                //removeControlsFromPnlLvPanel();
                //grdCustomerOB.Enabled = true;
                //pnllv.Visible = false;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        private void exlvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                
                string heading1 = "Customer OB ";
                string heading2 = "Accounting Year - " + cmdFYID.SelectedItem.ToString().Trim().Substring(0, cmdFYID.SelectedItem.ToString().Trim().IndexOf(':')).Trim();
                Utilities.export2Excel(heading1, heading2, "", grdCustomerOB, exlv);
                frmPopup.Close();
                frmPopup.Dispose();
                //grdCustomerOB.Enabled = true;
            }
            catch (Exception ex)
            {
            }
        }

        private void CustomerOB_Enter(object sender, EventArgs e)
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





