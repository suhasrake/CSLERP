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
using CSLERP.FileManager;

namespace CSLERP
{
    public partial class TaxItem : System.Windows.Forms.Form
    {
        ListView lvCopy = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        TextBox txtSearch = new TextBox();
        Form frmPopup = new Form();
        Timer filterTimer = new Timer();
        DataGridView AccCodeGrd = new DataGridView();
        string btnClicked = "";
        public TaxItem()
        {
            try
            {
                InitializeComponent();
 
            }
            catch (Exception)
            {
            }
        }
        private void TaxItem_Load(object sender, EventArgs e)
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
            ListTaxItem();
            applyPrivilege();
        }
        private void ListTaxItem()
        {
            try
            {
                grdList.Rows.Clear();
                TaxItemDB taxitemdb = new TaxItemDB();
                List<taxitem> TaxItems = taxitemdb.getTaxItems();
                foreach (taxitem ti in TaxItems)
                {
                    grdList.Rows.Add(ti.TaxItemID, ti.Description, ti.AccountCodeIN, ti.AccountCodeOUT,
                        ComboFIll.getStatusString(ti.status));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in Tax Item listing");
            }
            enableBottomButtons();
            pnlList.Visible = true;
            grdList.Visible = true;
        }

        private void initVariables()
        {
            try
            {
                
                fillStatusCombo(cmbStatus);
                //RegionDB.fillRegionCombo(cmbAccountCode);
            }
            catch (Exception)
            {
            }
        }

        private void applyPrivilege()
        {
            try
            {
                if (Main.itemPriv[1])
                {
                    btnNew.Visible = true;
                }
                else
                {
                    btnNew.Visible = false;
                }
                if (Main.itemPriv[2])
                {
                    grdList.Columns["Edit"].Visible = true;
                }
                else
                {
                    grdList.Columns["Edit"].Visible = false;
                }
            }
            catch (Exception)
            {
            }
        }
        private void closeAllPanels()
        {
            try
            {
                pnlInner.Visible = false;
                pnlOuter.Visible = false;
                pnlList.Visible = false;
            }
            catch (Exception)
            {

            }
        }


        private void fillStatusCombo(System.Windows.Forms.ComboBox cmb)
        {
            try
            {
                cmb.Items.Clear();
                for (int i = 0; i < Main.statusValues.GetLength(0); i++)
                {
                    cmb.Items.Add(Main.statusValues[i, 1]);
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                clearData();
                enableBottomButtons();
                pnlList.Visible = true;
                grdList.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        public void clearData()
        {
            try
            {
                txtID.Text = "";
                txtDescription.Text = "";
                cmbStatus.SelectedIndex = 0;
                txtAccountCodeIN.Text = "";
                txtAccountNameIN.Text = "";
                txtAccountCodeOUT.Text = "";
                txtAccountNameOut.Text = "";
            }
            catch (Exception)
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
                closeAllPanels();
                clearData();
                btnSave.Text = "Save";
                pnlOuter.Visible = true;
                pnlInner.Visible = true;
                txtID.Enabled = true;
                txtDescription.Enabled = true;
                txtAccountCodeIN.Enabled = false;
                txtAccountNameIN.Enabled=false;
                txtAccountCodeOUT.Enabled = false;
                txtAccountNameOut.Enabled = false;
                cmbStatus.Enabled = true;
                disableBottomButtons();
            }
            catch (Exception)
            {

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                taxitem ti = new taxitem();
                TaxItemDB taxitemdb = new TaxItemDB();
                ti.TaxItemID = txtID.Text;
                ti.Description = txtDescription.Text;
                ti.AccountCodeIN = txtAccountCodeIN.Text;
                ti.AccountNameIN = txtAccountNameIN.Text;
                ti.AccountCodeOUT = txtAccountCodeOUT.Text;
                ti.AccountNameOUT = txtAccountNameOut.Text;
                ti.status = ComboFIll.getStatusCode(cmbStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (taxitemdb.validateTaxItem(ti))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (taxitemdb.updateTaxItem(ti))
                        {
                            MessageBox.Show("Tax Item updated");
                            closeAllPanels();
                            ListTaxItem();
                            
                        }
                        else
                        {
                            MessageBox.Show("Failed to Tax Item");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {

                        if (taxitemdb.insertTaxItem(ti))
                        {
                            MessageBox.Show("Tax Item Added");
                            closeAllPanels();
                            ListTaxItem();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Tax Item");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Tax Item Data Validation failed");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing Tax Item");
            }

        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == 5)
                {
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlInner.Visible = true;
                    pnlOuter.Visible = true;
                    pnlList.Visible = false;
                    txtID.Enabled = false;
                    txtAccountCodeIN.Enabled = false;
                    txtAccountNameIN.Enabled = false;
                    txtDescription.Enabled = true;
                    cmbStatus.Enabled = true;
                    TaxItemDB tdb = new TaxItemDB();
                    txtID.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtDescription.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    txtAccountCodeIN.Text = grdList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    txtAccountNameIN.Text = tdb.getaccountname(txtAccountCodeIN.Text);
                    txtAccountCodeOUT.Text = grdList.Rows[e.RowIndex].Cells[3].Value.ToString();
                    txtAccountNameOut.Text = tdb.getaccountname(txtAccountCodeOUT.Text);
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[4].Value.ToString());
                    disableBottomButtons();
                }
            }
            catch (Exception)
            {
            }
        }
        private void disableBottomButtons()
        {
            btnNew.Visible = false;
            btnExit.Visible = false;
        }
        private void enableBottomButtons()
        {
            btnNew.Visible = true;
            btnExit.Visible = true;
        }

        //private void btnSelect_Click(object sender, EventArgs e)
        //{
        //    removeControlsFromlvPanel();
        //   // pnlInner.Controls.Remove(pnllv);
        //    //btnSelect.Enabled = false;
        //    pnllv = new Panel();
        //    pnllv.BorderStyle = BorderStyle.FixedSingle;

        //    pnllv.Bounds = new Rectangle(new Point(28, 16), new Size(474, 282));
        //    lv = AccountCodeDB.getAccountCodeListView();
        //    lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
        //    //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked1);
        //    lv.Bounds = new Rectangle(new Point(12, 9), new Size(455, 225));
        //    pnllv.Controls.Add(lv);

        //    Button lvOK = new Button();
        //    lvOK.Text = "OK";
        //    lvOK.Location = new Point(47, 248);
        //    lvOK.Click += new System.EventHandler(this.lvOK_Click2);
        //    pnllv.Controls.Add(lvOK);

        //    Button lvCancel = new Button();
        //    lvCancel.Text = "Cancel";
        //    lvCancel.Location = new Point(143, 248);
        //    lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
        //    pnllv.Controls.Add(lvCancel);

        //    Label lblSearch = new Label();
        //    lblSearch.Text = "Find";
        //    lblSearch.Location = new Point(306, 248);
        //    lblSearch.Size = new Size(37, 15);
        //    pnllv.Controls.Add(lblSearch);

        //    txtSearch = new TextBox();
        //    txtSearch.Location = new Point(354, 248);
        //    txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
        //    pnllv.Controls.Add(txtSearch);

        //    pnlInner.Controls.Add(pnllv);
        //    pnllv.BringToFront();
        //    pnllv.Visible = true;

        //    txtSearch.Focus();
        //}
        ////private void txtSearch_TextChanged(object sender, EventArgs e)
        //{
        //    pnllv.Controls.Remove(lvCopy);
        //    addItems();
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
        //    lvCopy.Columns.Add("AccountCode", -2, HorizontalAlignment.Left);
        //    lvCopy.Columns.Add("AccountName", -2, HorizontalAlignment.Left);
        //    lvCopy.Sorting = System.Windows.Forms.SortOrder.None;
        //    lvCopy.ColumnClick += new ColumnClickEventHandler(LvColumnClick); ;
        //    //this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
        //    //lvCopy.Location = new Point(13, 9);
        //    //lvCopy.Size = new Size(440, 199);
        //    lvCopy.Bounds = new Rectangle(new Point(17, 14), new Size(443, 212));
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
        //    pnllv.Controls.Add(lvCopy);
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
        //private void lvOK_Click2(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (lv.Visible == true)
        //        {
        //            if (!checkLVItemChecked("Account"))
        //            {
        //                return;
        //            }
        //            foreach (ListViewItem itemRow in lv.Items)
        //            {

        //                if (itemRow.Checked)
        //                {
        //                    if (btnClicked == "btnSelectIN")
        //                    {
        //                        txtAccountCodeIN.Text = itemRow.SubItems[1].Text;
        //                        txtAccountNameIN.Text = itemRow.SubItems[2].Text;
        //                    }
        //                    if (btnClicked == "btnSelectOUT")
        //                    {
        //                        txtAccountCodeOUT.Text = itemRow.SubItems[1].Text;
        //                        txtAccountNameOut.Text = itemRow.SubItems[2].Text;
        //                    }
        //                    //btnSelect.Enabled = true;
        //                    itemRow.Checked = false;
        //                    pnllv.Controls.Remove(lv);
        //                    pnllv.Visible = false;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (!checkLVCopyItemChecked("Account"))
        //            {
        //                return;
        //            }
                    
        //            foreach (ListViewItem itemRow in lvCopy.Items)
        //            {
        //                if (itemRow.Checked)
        //                {
        //                    txtAccountCodeIN.Text = itemRow.SubItems[1].Text;
        //                    txtAccountNameIN.Text = itemRow.SubItems[2].Text;
        //                    //btnSelect.Enabled = true;
        //                    itemRow.Checked = false;
        //                    pnllv.Controls.Remove(lvCopy);
        //                    pnllv.Visible = false;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //private void lvCancel_Click2(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //btnSelect.Enabled = true;
        //        lv.CheckBoxes = false;
        //        lv.CheckBoxes = true;
        //        pnllv.Controls.Remove(lv);
        //        pnllv.Visible = false;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //private void listView1_ItemChecked1(object sender, ItemCheckedEventArgs e)
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
        //private void txtSearch_TextChangedparty(object sender, EventArgs e)
        //{
        //    pnllv.Controls.Remove(lvCopy);
        //    addItemsparty();
        //}
        //private void addItemsparty()
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
        //    lvCopy.Columns.Add("Type", -2, HorizontalAlignment.Left);
        //    lvCopy.Sorting = System.Windows.Forms.SortOrder.None;
        //    lvCopy.ColumnClick += new ColumnClickEventHandler(LvColumnClick); ;
        //    this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
        //    //lvCopy.Location = new Point(13, 9);
        //    //lvCopy.Size = new Size(440, 199);
        //    lvCopy.Bounds = new Rectangle(new Point(17, 14), new Size(443, 212));
        //    lvCopy.Items.Clear();
        //    foreach (ListViewItem row in lv.Items)
        //    {
        //        string x = row.SubItems[0].Text;
        //        string no = row.SubItems[1].Text;
        //        string ch = row.SubItems[2].Text;
        //        string name = row.SubItems[3].Text;
        //        if (ch.ToLower().StartsWith(txtSearch.Text))
        //        {
        //            ListViewItem item = new ListViewItem();
        //            item.SubItems.Add(no);
        //            item.SubItems.Add(ch);
        //            item.SubItems.Add(name);
        //            item.Checked = false;
        //            lvCopy.Items.Add(item);
        //        }
        //    }
        //    lv.Visible = false;
        //    lvCopy.Visible = true;
        //    pnllv.Controls.Add(lvCopy);
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
        private void removeControlsFromlvPanel()
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

        private void btnSelectOUT_Click(object sender, EventArgs e)
        {
            try
            {
                showDebitAccountCodeOutDataGridView();
            }
            catch (Exception ex)
            {
            }
        }

        private void showDebitAccountCodeOutDataGridView()
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

                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(200, 5);
                lblSearch.AutoSize = true;
                lblSearch.Text = "Search by Name";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(320, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInAccCodeGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                AccCodeGrd = AccountCodeDB.getGridViewForAccountCode();
                AccCodeGrd.Bounds = new Rectangle(new Point(0, 27), new Size(550, 300));
                frmPopup.Controls.Add(AccCodeGrd);
                AccCodeGrd.Columns["AccountName"].Width = 380;

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grdAccCOdeOK_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
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
                    txtAccountCodeOUT.Text = row.Cells["AccountCode"].Value.ToString();
                    txtAccountNameOut.Text = row.Cells["AccountName"].Value.ToString();
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

        private void btnSelectIN_Click(object sender, EventArgs e)
        {
            try
            {
                showInAccountCodeDataGridView();
            }
            catch (Exception ex)
            {
            }
        }
        private void showInAccountCodeDataGridView()
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

                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(200, 5);
                lblSearch.AutoSize = true;
                lblSearch.Text = "Search by Name";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(320, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInAccINCodeGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                AccCodeGrd = AccountCodeDB.getGridViewForAccountCode();
                AccCodeGrd.Bounds = new Rectangle(new Point(0, 27), new Size(550, 300));
                frmPopup.Controls.Add(AccCodeGrd);
                AccCodeGrd.Columns["AccountName"].Width = 380;

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grdAccINCOdeOK_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 335);
                lvCancel.Click += new System.EventHandler(this.grdAccINCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdAccINCOdeOK_Click1(object sender, EventArgs e)
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
                    txtAccountCodeIN.Text = row.Cells["AccountCode"].Value.ToString();
                    txtAccountNameIN.Text = row.Cells["AccountName"].Value.ToString();
                }

                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void txtSearch_TextChangedInAccINCodeGridList(object sender, EventArgs e)
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
        private void handlefilterAccINTimerTimeout(object sender, EventArgs e)
        {
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterAccCodeGridData();
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }
        private void filterAccCodeINGridData()
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
        private void grdAccINCancel_Click1(object sender, EventArgs e)
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

        private void TaxItem_Enter(object sender, EventArgs e)
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


