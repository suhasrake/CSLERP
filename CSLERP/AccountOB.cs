using Microsoft.Office.Interop.Excel;
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


namespace CSLERP
{
    public partial class AccountOB : System.Windows.Forms.Form
    {
        string docID = "ACCOUNTOB";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        AccountOBheader preacchdr = new AccountOBheader();
        Panel pnlForwarder = new Panel();
        //Panel pnlModel = new System.Windows.Forms.Panel();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        //ListView lvCmtr = new ListView(); // list view for choice / selection list
        //ListView lvApprover = new ListView();
        System.Windows.Forms.TextBox txtSearch;
        ListView lvCopy = new ListView();
        ListView exlv = new ListView();
        Timer filterTimer = new Timer();
        Form frmPopup = new Form();
        DataGridView AccCodeGrd = new DataGridView();
        public AccountOB()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void AccountOB_Load(object sender, EventArgs e)
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
            FilteredAccOBList(listOption);
            //applyPrivilege();
        }
        private void FilteredAccOBList(int opt)
        {
            try
            {
                grdList.Rows.Clear();
                AccountOBDB AccDB = new AccountOBDB();
                List<AccountOBheader> AccList = AccountOBDB.getAccountOblist(opt);
                if (opt == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                //else if (opt == 2)
                //    lblActionHeader.Text = "List of Approved Documents";
                else if (opt == 2 || opt == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (AccountOBheader accob in AccList)
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
                MessageBox.Show("Error in AccountOB Listing");
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
            FinancialYearDB.fillFYIDCombo(cmbFYID);
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
                grdAccountOB.Rows.Clear();
                //----------clear temperory panels
                //      clearTabPageControls();
                pnlForwarder.Visible = false;
                dtDocDate.Value = DateTime.Parse("1900-01-01");
                cmbFYID.SelectedIndex = -1;
                txtDocNo.Text = "";
                //  btnSelectStockItemID.Enabled = true;
                handletab();
                //removeControlsFromPnlLvPanel();
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
                cmbFYID.SelectedValue = -1;
                setButtonVisibility("btnNew");
            }
            catch (Exception)
            {

            }
        }
        private void btnClearEntries_Click(object sender, EventArgs e)
        {
            grdAccountOB.Rows.Clear();
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
                AddAccountOBDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
        }
        private Boolean AddAccountOBDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdAccountOB.Rows.Count > 0)
                {
                    if (!verifyAndReworkAccGridRows())
                    {
                        return false;
                    }
                }
                grdAccountOB.Rows.Add();
                int kount = grdAccountOB.RowCount;
                // grdProductTestDetail.Rows[kount - 1].Cells["AccLineNo"].Value = kount;
                grdAccountOB.Rows[kount - 1].Cells["AccSerialNo"].Value = kount;
                grdAccountOB.Rows[kount - 1].Cells["AccAccountCode"].Value = "";
                grdAccountOB.Rows[kount - 1].Cells["AccAccountName"].Value = "";
                grdAccountOB.Rows[kount - 1].Cells["AccBalanceDebit"].Value = 0;
                grdAccountOB.Rows[kount - 1].Cells["AccBalanceCredit"].Value = 0;
                grdAccountOB.FirstDisplayedScrollingRowIndex = grdAccountOB.RowCount - 1;
                grdAccountOB.CurrentCell = grdAccountOB.Rows[kount - 1].Cells[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddAccountDetailRow() : Error");
                status = false;
            }

            return status;
        }

        private Boolean verifyAndReworkAccGridRows()
        {
            decimal crTotal = 0;
            decimal dbTotal = 0;
            Boolean status = true;
            try
            {
                int ir = grdAccountOB.Rows.Count;
                if (grdAccountOB.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Account details");
                    txtcredittotal.Visible = false;
                    txtdebittotal.Visible = false;
                    return false;
                }

                for (int i = 0; i < grdAccountOB.Rows.Count; i++)
                {
                    if (((Convert.ToDecimal(grdAccountOB.Rows[i].Cells["AccBalanceDebit"].Value) != 0) &&
                        (Convert.ToDecimal(grdAccountOB.Rows[i].Cells["AccBalanceCredit"].Value) != 0)) ||
                            ((Convert.ToDecimal(grdAccountOB.Rows[i].Cells["AccBalanceDebit"].Value) == 0) &&
                        (Convert.ToDecimal(grdAccountOB.Rows[i].Cells["AccBalanceCredit"].Value) == 0)))

                    {
                        MessageBox.Show("Enter either Debrit or Credit Row:" + (i + 1));
                        return false;
                    }
                    grdAccountOB.Rows[i].Cells["AccSerialNo"].Value = (i + 1);
                    if ((grdAccountOB.Rows[i].Cells["AccAccountCode"].Value.ToString().Trim().Length == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                    decimal dd = Convert.ToDecimal(grdAccountOB.Rows[i].Cells["AccBalanceCredit"].Value);
                    decimal dd1 = Convert.ToDecimal(grdAccountOB.Rows[i].Cells["AccBalanceDebit"].Value);
                    crTotal = crTotal + (Convert.ToDecimal(grdAccountOB.Rows[i].Cells["AccBalanceCredit"].Value));
                    dbTotal = dbTotal + (Convert.ToDecimal(grdAccountOB.Rows[i].Cells["AccBalanceDebit"].Value));
                }
                txtcredittotal.Text = crTotal.ToString();
                txtdebittotal.Text = dbTotal.ToString();
                txtcredittotal.Visible = true;
                txtdebittotal.Visible = true;
            }

            catch (Exception ex)
            {
                MessageBox.Show("AccountOB Detail Validation Failed");
                return false;
            }
            return status;
        }
        private List<AccountOBdetail> getAccountOBDetailList(AccountOBheader Acchr)
        {
            List<AccountOBdetail> AccobDetails = new List<AccountOBdetail>();
            try
            {
                AccountOBdetail accd = new AccountOBdetail();

                for (int i = 0; i < grdAccountOB.Rows.Count; i++)
                {
                    accd = new AccountOBdetail();
                    accd.DocumentID = Acchr.DocumentID;
                    accd.DocumentNo = Acchr.DocumentNo;
                    accd.DocumentDate = Acchr.DocumentDate;
                    accd.FYID = Acchr.FYID;
                    accd.AccountCode = grdAccountOB.Rows[i].Cells["AccAccountCode"].Value.ToString();
                    accd.AccountName = grdAccountOB.Rows[i].Cells["AccAccountName"].Value.ToString();
                    accd.SerialNo = Convert.ToInt32(grdAccountOB.Rows[i].Cells["AccSerialNo"].Value.ToString());
                    if (grdAccountOB.Rows[i].Cells["AccBalanceDebit"].Value != null)
                    {
                        accd.BalanceDebit = Convert.ToDecimal(grdAccountOB.Rows[i].Cells["AccBalanceDebit"].Value.ToString());
                    }
                    if (grdAccountOB.Rows[i].Cells["AccBalanceCredit"].Value != null)
                    {
                        accd.BalanceCredit = Convert.ToDecimal(grdAccountOB.Rows[i].Cells["AccBalanceCredit"].Value.ToString());
                    }
                    AccobDetails.Add(accd);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("getAccountOBDetailList() : Error updating AccountOB Details");
                AccobDetails = null;
            }
            return AccobDetails;
        }
        private void grdProductTestDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdAccountOB.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("AccDelete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdAccountOB.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkAccGridRows();
                    }
                    if (columnName.Equals("AccSelect"))
                    {
                        showAccountCodeDataGridView();
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
        //----------------
        private void showAccountCodeDataGridView()
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

                frmPopup.Size = new Size(550, 370);

                System.Windows.Forms.Label lblSearch = new System.Windows.Forms.Label();
                lblSearch.Location = new System.Drawing.Point(200, 5);
                lblSearch.AutoSize = true;
                lblSearch.Text = "Search by Name";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new System.Windows.Forms.TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(320, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInAccCodeGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                AccCodeGrd = AccountCodeDB.getGridViewForAccountCode();
                AccCodeGrd.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 27), new Size(550, 300));
                frmPopup.Controls.Add(AccCodeGrd);
                AccCodeGrd.Columns["AccountName"].Width = 380;

                System.Windows.Forms.Button lvOK = new System.Windows.Forms.Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grdAccCOdeOK_Click1);
                frmPopup.Controls.Add(lvOK);

                System.Windows.Forms.Button lvCancel = new System.Windows.Forms.Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 335);
                lvCancel.Click += new System.EventHandler(this.grdAccCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdAccCOdeOK_Click1(object sender, EventArgs e)
        {
            string users = "";
            try
            {
                var checkedRows = from DataGridViewRow r in AccCodeGrd.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("Select one AccCode");
                    return;
                }

                foreach (var row in checkedRows)
                {
                    if (validateItems(row.Cells["AccountCode"].Value.ToString()))
                    {
                        grdAccountOB.CurrentRow.Cells["AccAccountCode"].Value = row.Cells["AccountCode"].Value.ToString();
                        grdAccountOB.CurrentRow.Cells["AccAccountName"].Value = row.Cells["AccountName"].Value.ToString();
                    }
                    //grdPRDetail.CurrentRow.Cells["AccountCodeDebit"].Value = row.Cells["AccountCode"].Value.ToString();
                    //grdPRDetail.CurrentRow.Cells["AccountNameDebit"].Value = row.Cells["AccountName"].Value.ToString();
                }

                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void txtSearch_TextChangedInAccCodeGridList(object sender, EventArgs e)
        {
            try
            {
                filterTimer.Enabled = false;
                filterTimer.Stop();
                filterTimer.Tick -= new System.EventHandler(this.handlefilterAccTimerTimeout);
                filterTimer.Tick += new System.EventHandler(this.handlefilterAccTimerTimeout);
                filterTimer.Interval = 500;
                filterTimer.Enabled = true;
                filterTimer.Start();
            }
            catch (Exception ex)
            {

            }
        }
        private void handlefilterAccTimerTimeout(object sender, EventArgs e)
        {
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterAccCodeGridData();
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }
        private void filterAccCodeGridData()
        {
            try
            {
                AccCodeGrd.CurrentCell = null;
                foreach (DataGridViewRow row in AccCodeGrd.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in AccCodeGrd.Rows)
                    {
                        if (!row.Cells["AccountName"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
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
        private void grdAccCancel_Click1(object sender, EventArgs e)
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

        //------------
        //private void ShowAccListView()
        //{
        //    //removeControlsFromPnlLvPanel();
        //    //grdAccountOB.Enabled = false;
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

        //    lv = AccountCodeDB.getAccountCodeListView();
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
        //                MessageBox.Show("Select one Account.");
        //                return;
        //            }
        //            if (lv.CheckedIndices.Count > 1)
        //            {
        //                MessageBox.Show("Not allowed to select more than one Account.");
        //                return;
        //            }
        //            foreach (ListViewItem itemRow in lv.Items)
        //            {
        //                if (itemRow.Checked)
        //                {
        //                    if (validateItems(itemRow.SubItems[1].Text))
        //                    {
        //                        grdAccountOB.CurrentRow.Cells["AccAccountCode"].Value = itemRow.SubItems[1].Text;
        //                        grdAccountOB.CurrentRow.Cells["AccAccountName"].Value = itemRow.SubItems[2].Text;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (lvCopy.CheckedIndices.Count == 0)
        //            {
        //                MessageBox.Show("Select one Account.");
        //                return;
        //            }
        //            if (lvCopy.CheckedIndices.Count > 1)
        //            {
        //                MessageBox.Show("Not allowed to select more than one Account.");
        //                return;
        //            }
        //            foreach (ListViewItem itemRow in lvCopy.Items)
        //            {
        //                if (itemRow.Checked)
        //                {
        //                    if (validateItems(itemRow.SubItems[1].Text))
        //                    {
        //                        grdAccountOB.CurrentRow.Cells["AccAccountCode"].Value = itemRow.SubItems[1].Text;
        //                        grdAccountOB.CurrentRow.Cells["AccAccountName"].Value = itemRow.SubItems[2].Text;
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
        //        //grdAccountOB.Enabled = true;
        //        //pnllv.Visible = false;
        //        //pnllv.Controls.Remove(lv);
        //        //pnllv.Controls.Remove(lvCopy);
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
                foreach (DataGridViewRow row in grdAccountOB.Rows)
                {
                    if (grdAccountOB.Rows[row.Index].Cells["AccAccountCode"].Value.ToString() == item)
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
                        MessageBox.Show("This year already has An Account");
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
                AccountOBDB AccDB = new AccountOBDB();
                AccountOBheader acclist = new AccountOBheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;
                if (!verifyAndReworkAccGridRows())
                {
                    MessageBox.Show("Error found in AccountOb details. Please correct before updating the details");
                    return;
                }
                try
                {
                    acclist.DocumentID = docID;
                    lmn = cmbFYID.SelectedItem.ToString();
                    acclist.FYID = cmbFYID.SelectedItem.ToString().Trim().Substring(0, cmbFYID.SelectedItem.ToString().Trim().IndexOf(':')).Trim();
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
                if (Convert.ToDecimal(txtcredittotal.Text) != Convert.ToDecimal(txtdebittotal.Text))
                {
                    MessageBox.Show("Debit and Credit total should be equal.");
                    return;
                }
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
                        List<AccountOBdetail> AccobDetails = getAccountOBDetailList(acclist);
                        if(AccobDetails == null)
                        {
                            MessageBox.Show("AccountOB Detail Validation Failed(Check GridDetail)");
                            return;
                        }
                        if (AccDB.InsertAccHeaderAndDetail(acclist, AccobDetails))
                        {
                            MessageBox.Show("AccountOB Details Added");
                            closeAllPanels();
                            listOption = 1;
                            FilteredAccOBList(listOption);
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
                    List<AccountOBdetail> AccobDetails = getAccountOBDetailList(acclist);
                    if (AccobDetails == null)
                    {
                        MessageBox.Show("AccountOB Detail Validation Failed(Check GridDetail)");
                        return;
                    }
                    if (AccDB.updateAccHeaderAndDetail(acclist, preacchdr, AccobDetails))
                    {
                        MessageBox.Show("AccountOB details updated");
                        closeAllPanels();
                        listOption = 1;
                        FilteredAccOBList(listOption);
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
                    preacchdr = new AccountOBheader();
                    preacchdr.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    preacchdr.DocumentNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentNo"].Value.ToString());
                    preacchdr.DocumentDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["DocumentDate"].Value.ToString());
                    preacchdr.FYID = grdList.Rows[e.RowIndex].Cells["FYID"].Value.ToString();
                    txtDocNo.Text = preacchdr.DocumentNo.ToString();
                    dtDocDate.Value = preacchdr.DocumentDate;
                    cmbFYID.SelectedIndex = cmbFYID.FindString(preacchdr.FYID);
                    dtDocDate.Value = preacchdr.DocumentDate;
                    List<AccountOBdetail> Accdet = AccountOBDB.getAccountOBDetail(preacchdr);
                    grdAccountOB.Rows.Clear();
                    int i = 0;
                    foreach (AccountOBdetail det in Accdet)
                    {
                        if (!AddAccountOBDetailRow())
                        {
                            MessageBox.Show("Error found in Account Detail. Not able to add rows into grid.");
                        }
                        else
                        {
                            grdAccountOB.Rows[i].Cells["AccSerialNo"].Value = det.SerialNo;
                            grdAccountOB.Rows[i].Cells["AccAccountCode"].Value = det.AccountCode;
                            grdAccountOB.Rows[i].Cells["AccAccountName"].Value = det.AccountName;
                            grdAccountOB.Rows[i].Cells["AccBalanceDebit"].Value = det.BalanceDebit;
                            grdAccountOB.Rows[i].Cells["AccBalanceCredit"].Value = det.BalanceCredit;
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
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
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
                AccountOBDB ProdTempDB = new AccountOBDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Finalize the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (ProdTempDB.FinalizeAccountOB(preacchdr))
                    {
                        MessageBox.Show(" Document Finalized");
                        closeAllPanels();
                        listOption = 1;
                        FilteredAccOBList(listOption);
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
                    tabControl1.SelectedTab = tabAccountOB;
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
                    //disableTabPages();
                    tabControl1.SelectedTab = tabAccountOB;
                }
                else if (btnName == "View")
                {
                    enableTabPages();
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    dtDocDate.Enabled = false;
                    txtDocNo.Enabled = false;
                    cmbFYID.Enabled = false;
                    btnAddLine.Enabled = false;
                    txtcredittotal.Enabled = false;
                    txtdebittotal.Enabled = false;
                    grdAccountOB.Enabled = false;
                    btnExporttoExcell.Enabled = true;
                    //  disableTabPages();
                }
                else if (btnName == "Edit")
                {
                    cmbFYID.Enabled = false;
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
        void handletab()
        {
            cmbFYID.Enabled = true;
            btnAddLine.Enabled = true;
            txtcredittotal.Enabled = true;
            txtdebittotal.Enabled = true;
            grdAccountOB.Enabled = true;
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
        }
      
        private void dgvDocumentList_CellContentClick(Object sender, DataGridViewCellEventArgs e)
        {
            
        }
      

        private void txtCreditPeriods_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtValidityDays_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            FilteredAccOBList(listOption);
        }

        private void btnApproved_Click(object sender, EventArgs e)
        {
            listOption = 2;
            FilteredAccOBList(listOption);
        }
        //private void removeControlsFromPnlLvPanel()
        //{
        //    try
        //    {
        //        //foreach (Control p in pnllv.Controls)
        //        //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(System.Windows.Forms.Button))
        //        //    {
        //        //        p.Dispose();
        //        //    }
        //        pnllv.Controls.Clear();
        //        Control nc = pnllv.Parent;
        //        nc.Controls.Remove(pnllv);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //private void removeControlsFromForwarderPanelTV()
        //{
        //    try
        //    {
        //        foreach (Control p in pnlForwarder.Controls)
        //            if (p.GetType() == typeof(TreeView) || p.GetType() == typeof(System.Windows.Forms.Button))
        //            {
        //                p.Dispose();
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
       
        private void tvOK_Click(object sender, EventArgs e)
        {
           
        }
        
        private void btnCloseDocument_Click_1(object sender, EventArgs e)
        {
           
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
        }

        private void txtBalCredit_TextChanged(object sender, EventArgs e)
        {
        }

        private void grdProductTestDetail_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
        }

        private void txtdebittotal_TextChanged(object sender, EventArgs e)
        {

        }
        //private void txtSearch_TextChanged(object sender, EventArgs e)
        //{
        //    frmPopup.Controls.Remove(lvCopy);
        //    addItems();
        //}
        ////private void CopylistView2_ItemChecked(object sender, ItemCheckedEventArgs e)
        ////{
        ////    try
        ////    {
        ////        if (lvCopy.CheckedIndices.Count > 1)
        ////        {
        ////            MessageBox.Show("Cannot select more than one item");
        ////            e.Item.Checked = false;
        ////        }
        ////    }
        ////    catch (Exception)
        ////    {
        ////    }
        ////}
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
        //   // this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);          
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

        private void btnExporttoExcell_Click(object sender, EventArgs e)
        {
            try
            {
                if (verifyAndReworkAccGridRows())
                {
                    //removeControlsFromPnlLvPanel();
                    //grdAccountOB.Enabled = false;
                    //pnllv = new Panel();
                    //pnllv.BorderStyle = BorderStyle.FixedSingle;

                    //pnllv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(100, 100), new Size(600, 300));
                    frmPopup = new Form();
                    frmPopup.StartPosition = FormStartPosition.CenterScreen;
                    frmPopup.BackColor = Color.CadetBlue;

                    frmPopup.MaximizeBox = false;
                    frmPopup.MinimizeBox = false;
                    frmPopup.ControlBox = false;
                    frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                    frmPopup.Size = new Size(450, 310);
                    exlv = Utilities.GridColumnSelectionView(grdAccountOB);
                    exlv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 25), new System.Drawing.Size(450, 250));
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
                    MessageBox.Show("Error: No Data In Grid.");
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
                frmPopup.Close();
                frmPopup.Dispose();
                //grdAccountOB.Enabled = true;
                //removeControlsFromPnlLvPanel();
                //pnllv.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void exlvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                //grdAccountOB.Enabled = true;
                //pnllv.Visible = false;
              
                string heading1 = "Account OB ";
                string heading2 = "Accounting Year - " + cmbFYID.SelectedItem.ToString().Trim().Substring(0, cmbFYID.SelectedItem.ToString().Trim().IndexOf(':')).Trim();
                Utilities.export2Excel(heading1, heading2, "", grdAccountOB, exlv);
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export to Excell error");
            }
        }

        private void AccountOB_Enter(object sender, EventArgs e)
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



