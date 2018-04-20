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
using CSLERP.PrintForms;

namespace CSLERP
{
    public partial class BREntry : System.Windows.Forms.Form
    {
        //DateTimePicker oDateTimePicker;
        string docID = "BREntry";
        string userString = "";
        string userCommentStatusString = "";
        Dictionary<int, string> rowid = new Dictionary<int, string>();
        List<brentry> VDetails = new List<brentry>();
        List<brentry> prevdet = new List<brentry>();
        Form dtpForm = new Form();
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        BREntryDB brdb = new BREntryDB();
        ListView lvCopy = new ListView();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        TextBox txtSearch = new TextBox();
        Panel pnlModel = new Panel();
        ListView exlv = new ListView();
        public BREntry()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }
        private void BREntry_Load(object sender, EventArgs e)
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
            ////ListFilteredBR(listOption);
        }
        private void ListFilteredBR(int option)
        {
            try
            {
                grdList.Rows.Clear();
                BREntryDB brdb = new BREntryDB();
                List<brentry> BREList = brdb.getFilteredBREntry(option, dtFromDate.Value, dtToDate.Value, txtBankAccountCode.Text);
                    grdList.Columns["dt"].Visible = true;
                btnExportToExcell.Visible = true;
                btnUpdate.Visible = true;
                foreach (brentry br in BREList)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["bRowID"].Value = br.rowid;
                    grdList.Rows[grdList.RowCount - 1].Cells["bSerialNo"].Value = grdList.RowCount;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = br.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["SLName"].Value = br.SLName;
                    grdList.Rows[grdList.RowCount - 1].Cells["AmountDebit"].Value = br.AmountDebit;
                    grdList.Rows[grdList.RowCount - 1].Cells["AmountCredit"].Value = br.AmountCredit;
                    grdList.Rows[grdList.RowCount - 1].Cells["BankReferenceNo"].Value = br.Bankrefno;
                    grdList.Rows[grdList.RowCount - 1].Cells["BankReferenceDate"].Value = br.bankrefdate;
                    grdList.Rows[grdList.RowCount - 1].Cells["bVoucherNo"].Value = br.VoucherNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["bVoucherDate"].Value = br.VoucherDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["bAccountName"].Value = br.AccountName;
                    if (br.BankDate == DateTime.Parse("01-01-1900"))
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["bBankDate"].Value = "NULL";
                    }
                    else
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["bBankDate"].Value = br.BankDate;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in PR Listing");
            }
            setButtonVisibility("init");
            ////pnlList.Visible = true;
        }

        private void initVariables()
        {
            if (getuserPrivilegeStatus() == 1)
            {
                listOption = 1;
            }
            dtFromDate.Enabled = true;
            dtToDate.Enabled = true;
            txtBankAccountCode.Enabled = false;
            txtBankAccountName.Enabled = false;
            btnExportToExcell.Visible = false;
            btnUpdate.Visible = false;
            docID = Main.currentDocument;
            dtFromDate.Format = DateTimePickerFormat.Custom;
            dtFromDate.CustomFormat = "dd-MM-yyyy";
            dtToDate.Format = DateTimePickerFormat.Custom;
            dtToDate.CustomFormat = "dd-MM-yyyy";
            dtFromDate.Value = DateTime.Parse(Main.currentFYStartDate);
            ////dtFromDate.Value = DateTime.Parse("01-04-" + Convert.ToString(DateTime.Now.Year - 1) + "");
            //dtFromDate.Value = DateTime.Now.Year - 1;
            dtToDate.Value = DateTime.Now;
            pnlUI.Controls.Add(pnlList);
            closeAllPanels();
            pnlList.Visible = true;
            grdList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
        }

        private void closeAllPanels()
        {
            try
            {
                pnlList.Visible = false;
            }
            catch (Exception)
            {
            }
        }

        public void clearData()
        {
            try
            {
                //prevjvh = new JournalVoucherHeader();
                grdList.Rows.Clear();
                //dgvComments.Rows.Clear();
                pnlForwarder.Visible = false;
                pnllv.Visible = true;
                txtBankAccountCode.Text = "";
                txtBankAccountName.Text = "";
                dtFromDate.Value = DateTime.Parse(Main.currentFYStartDate);
                dtToDate.Value = DateTime.Now;
                dtToDate.Value = DateTime.Now;
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
                closeAllPanels();
                setButtonVisibility("btnNew");
            }
            catch (Exception)
            {
            }
        }

        private Boolean verifyAndReworkBankData()
        {
            Boolean status = true;
            try
            {
                if (txtBankAccountName.Text == null || txtBankAccountName.Text.Length == 0 || txtBankAccountCode.Text == null ||
                    txtBankAccountCode.Text.Length == 0)
                {
                    MessageBox.Show("values not selected");
                    return false;
                }
                if (dtFromDate.Value == DateTime.Parse("01-01-1900") || dtToDate.Value == DateTime.Parse("01-01-1900"))
                {
                    MessageBox.Show("date not entered");
                    return false;
                }
                if (cmbListOption.SelectedIndex == -1)
                {
                    MessageBox.Show("Select a list option");
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return status;
        }

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredBR(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredBR(listOption);
        }

        private void btnApproved_Click(object sender, EventArgs e)
        {
            if (getuserPrivilegeStatus() == 2)
            {
                listOption = 6; //viewer
            }
            else
            {
                listOption = 3;
            }
            ListFilteredBR(listOption);
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }

        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("dt"))
                {
                    string str = "";
                    DateTime DT = DateTime.Today;
                    if (columnName.Equals("dt"))
                    {
                        str = grdList.Rows[e.RowIndex].Cells["bBankDate"].Value.ToString();
                        if (str == "NULL")
                        {
                            DT = DateTime.Parse("01-01-1900");
                            ////DT = DateTime.MinValue;
                        }
                        else
                        {
                            DT = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bBankDate"].Value);
                        }
                    }
                    Rectangle tempRect = grdList.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                    showDtPickerForm(Cursor.Position.X, Cursor.Position.Y, tempRect.Location, DT);
                }

            }
            catch (Exception ex)
            {
            }
        }

        void cellDateTimePickerValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            if (dtp.Value > DateTime.Now)
            {
                MessageBox.Show("Bank date cannot be more than today");
                return;
            }
            grdList.Rows[grdList.CurrentCell.RowIndex].Cells[grdList.CurrentCell.ColumnIndex - 1].Value = dtp.Value;
            if (!rowid.ContainsKey(Convert.ToInt32(grdList.Rows[grdList.CurrentCell.RowIndex].Cells["bRowID"].Value)))
            {
                rowid.Add(Convert.ToInt32(grdList.Rows[grdList.CurrentCell.RowIndex].Cells["bRowID"].Value), grdList.Rows[grdList.CurrentCell.RowIndex].Cells["gDocumentID"].Value.ToString());
            }
        }

        void removedateschecked(object sender, EventArgs e,DateTimePicker dt)
        {
            CheckBox chck = (CheckBox)sender;
            if (chck.Checked)
            {
                dt.Visible = false;
                grdList.Rows[grdList.CurrentCell.RowIndex].Cells[grdList.CurrentCell.ColumnIndex - 1].Value = "NULL";
                if (!rowid.ContainsKey(Convert.ToInt32(grdList.Rows[grdList.CurrentCell.RowIndex].Cells["bRowID"].Value)))
                {
                    rowid.Add(Convert.ToInt32(grdList.Rows[grdList.CurrentCell.RowIndex].Cells["bRowID"].Value), grdList.Rows[grdList.CurrentCell.RowIndex].Cells["gDocumentID"].Value.ToString());
                }
            }
            else
            {
                dt.Visible = true;
                grdList.Rows[grdList.CurrentCell.RowIndex].Cells[grdList.CurrentCell.ColumnIndex - 1].Value = dt.Value;
            }

        }

        private void showDtPickerForm(int left, int top, Point location, DateTime dtvalue)
        {
            try
            {
                if (left > Screen.PrimaryScreen.Bounds.Width - 250)
                {
                    left = Screen.PrimaryScreen.Bounds.Width - 250;
                }
                dtpForm = new Form();
                dtpForm.StartPosition = FormStartPosition.Manual;
                dtpForm.ShowIcon = false;
                dtpForm.MinimizeBox = false;
                dtpForm.MaximizeBox = false;
                dtpForm.Size = new Size(200, 100);
                dtpForm.Location = new Point(left, top);
                DateTimePicker dt = new DateTimePicker();
                dt.Format = DateTimePickerFormat.Custom;
                dt.CustomFormat = "dd-MM-yyyy";
                dt.ValueChanged += new EventHandler(cellDateTimePickerValueChanged);
                dt.Value = dtvalue;
                if(dt.Value.ToString("dd-MM-yyyy") == "01-01-1900")
                {
                    dt.Value = DateTime.Now.Date;
                }
                dtpForm.Controls.Add(dt);
                CheckBox rmdt = new CheckBox();
                rmdt.Checked = false;
                rmdt.CheckedChanged += (sender, e) => removedateschecked(sender, e, dt);

                dtpForm.Controls.Add(rmdt);

                {
                    dt.Width = 150;
                    dt.Height = 100;
                    dt.Visible = true;
                    dt.ShowUpDown = true;
                    rmdt.Text = "Remove Date";
                    rmdt.AutoSize = true;
                    rmdt.Visible = true;
                    rmdt.Location = new Point(5, 25); //vertical
                    System.Windows.Forms.SendKeys.Send("%{DOWN}");
                    dtpForm.ShowDialog();

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void pnlList_Paint(object sender, PaintEventArgs e)
        {
        }
        private string getReverseString(string forwarderList)
        {
            string reverseString = "";
            try
            {
                string prevUser = "";
                string[] lst1 = forwarderList.Split(Main.delimiter2);
                for (int i = 0; i < lst1.Length - 1; i++)
                {
                    if (lst1[i].Trim().Length > 1)
                    {
                        string[] lst2 = lst1[i].Split(Main.delimiter1);

                        if (i == (lst1.Length - 2))
                        {
                            if (reverseString.Trim().Length > 0)
                            {
                                reverseString = reverseString + "!@#" + prevUser;
                            }
                        }
                        else
                        {
                            reverseString = reverseString + lst2[0] + Main.delimiter1 + lst2[1] + Main.delimiter1 + Main.delimiter2;
                            prevUser = lst2[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return reverseString;
        }
        //private void btnReverse_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DialogResult dialog = MessageBox.Show("Are you sure to Reverse the document ?", "Yes", MessageBoxButtons.YesNo);
        //        if (dialog == DialogResult.Yes)
        //        {
        //            string s = prevjvh.ForwarderList;
        //            string reverseStr = getReverseString(prevjvh.ForwarderList);
        //            //do forward activities
        //            prevjvh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevjvh.CommentStatus);
        //            JournalVoucherDB jvhDB = new JournalVoucherDB();
        //            if (reverseStr.Trim().Length > 0)
        //            {
        //                int ind = reverseStr.IndexOf("!@#");
        //                prevjvh.ForwarderList = reverseStr.Substring(0, ind);
        //                prevjvh.ForwardUser = reverseStr.Substring(ind + 3);
        //                prevjvh.DocumentStatus = prevjvh.DocumentStatus - 1;
        //            }
        //            else
        //            {
        //                prevjvh.ForwarderList = "";
        //                prevjvh.ForwardUser = "";
        //                prevjvh.DocumentStatus = 1;
        //            }
        //            if (jvhDB.reverseJournalVoucherHeader(prevjvh))
        //            {
        //                MessageBox.Show("PR Document Reversed");
        //                closeAllPanels();
        //                setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        private void btnListDocuments_Click(object sender, EventArgs e)
        {
        }
        private void dgvDocumentList_CellContentClick(Object sender, DataGridViewCellEventArgs e)
        {
        }
        private void setButtonVisibility(string btnName)
        {
            try
            {
                ////btnExportToExcell.Visible = false;
                btnUpdate.Visible = false;
                grdList.Visible = false;
                btnExit.Visible = true;
                pnlBottomButtons.Visible = true;
                if (btnName == "init")
                {
                    btnExit.Visible = true;
                }
                else if (btnName == "btnCheck")
                {
                    grdList.Visible = true;
                    btnUpdate.Visible = true;
                    ////btnExportToExcell.Visible = true;
                }
                int ups = getuserPrivilegeStatus();
                if (ups == 1 || ups == 0)
                {
                    if (ups == 1 && listOption == 2)
                    {
                        grdList.Columns["dt"].Visible = false;
                        btnUpdate.Visible = false;
                    }
                    else if (ups == 1 && listOption == 1)
                    {
                        grdList.Columns["dt"].Visible = false;
                        btnUpdate.Visible = false;
                    }
                    else if (listOption == 1)
                    {
                        grdList.Columns["dt"].Visible = false;
                        btnUpdate.Visible = false;
                    }
                    else
                    {
                        grdList.Columns["dt"].Visible = true;
                        btnUpdate.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        void handleNewButton()
        {
        }
        void handleGrdEditButton()
        {
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
            lvCopy.Columns.Add("BankAccountCode", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("BankAccountName", -2, HorizontalAlignment.Left);
            lvCopy.Sorting = System.Windows.Forms.SortOrder.None;
            lvCopy.ColumnClick += new ColumnClickEventHandler(LvColumnClick); ;
            //this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
            lvCopy.Bounds = new Rectangle(new Point(17, 14), new Size(443, 212));
            lvCopy.Items.Clear();
            foreach (ListViewItem row in lv.Items)
            {
                string x = row.SubItems[0].Text;
                string no = row.SubItems[1].Text;
                string ch = row.SubItems[2].Text;
                if (ch.ToLower().StartsWith(txtSearch.Text))
                {
                    ListViewItem item = new ListViewItem();
                    item.SubItems.Add(no);
                    item.SubItems.Add(ch);
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
        private void txtPONo_TextChanged(object sender, EventArgs e)
        {
        }
        private void cmbCustomer_SelectedValueChanged(object sender, EventArgs e)
        {
        }
        private void tabPRHeader_Click(object sender, EventArgs e)
        {
        }
        private void removeControlsFromlvPanel()
        {
            try
            {
                //foreach (Control p in pnllv.Controls)
                //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
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
        private void showBankLIstView()
        {
            removeControlsFromlvPanel();
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;

            pnllv.Bounds = new Rectangle(new Point(24, 32), new Size(477, 282));
            lv = BREntryDB.getBankAccountCodeListView();
            lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked3);
            lv.Bounds = new Rectangle(new Point(17, 14), new Size(443, 212));
            pnllv.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Location = new Point(44, 246);
            lvOK.Click += new System.EventHandler(this.lvOK_Click3);
            pnllv.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Location = new Point(141, 246);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click3);
            pnllv.Controls.Add(lvCancel);

            Label lblSearch = new Label();
            lblSearch.Text = "Find";
            lblSearch.Location = new Point(260, 250);
            lblSearch.Size = new Size(37, 15);
            pnllv.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Location = new Point(303, 246);
            txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            pnllv.Controls.Add(txtSearch);

            pnlList.Controls.Add(pnllv);
            pnllv.BringToFront();
            pnllv.Visible = true;
        }
        private void lvOK_Click3(object sender, EventArgs e)
        {
            try
            {
                if (lv.Visible == true)
                {
                    if (lv.CheckedIndices.Count == 0)
                    {
                        MessageBox.Show("Select one Bank.");
                        return;
                    }
                    if (lv.CheckedIndices.Count > 1)
                    {
                        MessageBox.Show("Not allowed to select more than one Bank.");
                        return;
                    }
                    foreach (ListViewItem itemRow in lv.Items)
                    {

                        if (itemRow.Checked)
                        {
                            txtBankAccountCode.Text = itemRow.SubItems[1].Text;
                            txtBankAccountName.Text = itemRow.SubItems[2].Text;
                            itemRow.Checked = false;
                            pnllv.Controls.Remove(lv);
                            pnllv.Controls.Clear();
                            pnllv.Visible = false;
                            pnlList.Controls.Remove(pnllv);
                        }
                    }
                }
                else
                {
                    if (lvCopy.CheckedIndices.Count == 0)
                    {
                        MessageBox.Show("Select one Bank.");
                        return;
                    }
                    if (lvCopy.CheckedIndices.Count > 1)
                    {
                        MessageBox.Show("Not allowed to select more than one Bank.");
                        return;
                    }
                    foreach (ListViewItem itemRow in lvCopy.Items)
                    {

                        if (itemRow.Checked)
                        {
                            txtBankAccountCode.Text = itemRow.SubItems[1].Text;
                            txtBankAccountName.Text = itemRow.SubItems[2].Text;
                            itemRow.Checked = false;
                            pnllv.Controls.Remove(lvCopy);
                            pnllv.Controls.Clear();
                            pnllv.Visible = false;
                            pnlList.Controls.Remove(pnllv);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void lvCancel_Click3(object sender, EventArgs e)
        {
            try
            {
                lv.CheckBoxes = false;
                lv.CheckBoxes = true;
                //pnllv.Controls.Remove(lv);
                pnllv.Controls.Clear();
                pnllv.Visible = false;
                pnlList.Controls.Remove(pnllv);
            }
            catch (Exception)
            {
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            clearData();
            grdList.Visible = false;
            btnUpdate.Visible = false;
            ////btnExportToExcell.Visible = false;
            showBankLIstView();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            removeControlsFromlvPanel();
            if (verifyAndReworkBankData())
            {

                {
                    if (listOption == 1)
                    {
                        ListFilteredBR(listOption);
                        setButtonVisibility("btnCheck");
                    }
                    if (listOption == 2)
                    {
                        ListFilteredBR(listOption);
                        setButtonVisibility("btnCheck");
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string pay = "BANKPAYMENTVOUCHER";
            string recpt = "BANKRECEIPTVOUCHER";
            BREntryDB brdb = new BREntryDB();
            brentry bd = new brentry();
            try
            {
                //List<brentry> brlist = getVoucherDetails();
                foreach (DataGridViewRow drw in grdList.Rows)
                {
                    if (rowid.ContainsKey(Convert.ToInt32(drw.Cells["bRowID"].Value)) && (rowid[Convert.ToInt32(drw.Cells["bRowID"].Value)] == drw.Cells["gDocumentID"].Value.ToString()))
                    {
                        if (drw.Cells["bBankDate"].Value.ToString() == "NULL" ||
                        Convert.ToDateTime(drw.Cells["bBankDate"].Value) == DateTime.Parse("01-01-1900"))
                        {
                            bd = new brentry();
                            bd.rowid = Convert.ToInt32(drw.Cells["bRowID"].Value);
                            bd.DocumentID = drw.Cells["gDocumentID"].Value.ToString();
                            bd.Bankrefno = drw.Cells["BankReferenceNO"].Value.ToString();
                            bd.bankrefdate = Convert.ToDateTime(drw.Cells["BankReferenceDate"].Value);
                            bd.VoucherDate = Convert.ToDateTime(drw.Cells["bVoucherDate"].Value);
                            bd.BankDate = DateTime.MinValue;
                        }
                        else
                        {
                            if (Convert.ToDateTime(drw.Cells["bBankDate"].Value) <= UpdateTable.getSQLDateTime())
                            {
                                bd = new brentry();
                                bd.rowid = Convert.ToInt32(drw.Cells["bRowID"].Value);
                                bd.DocumentID = drw.Cells["gDocumentID"].Value.ToString();
                                bd.Bankrefno = drw.Cells["BankReferenceNO"].Value.ToString();
                                bd.bankrefdate = Convert.ToDateTime(drw.Cells["BankReferenceDate"].Value);
                                bd.VoucherDate = Convert.ToDateTime(drw.Cells["bVoucherDate"].Value);
                                bd.BankDate = Convert.ToDateTime(drw.Cells["bBankDate"].Value);
                            }                            
                        }
                        VDetails.Add(bd);
                    }
                }

                foreach (brentry br in VDetails)
                {
                    if (br.DocumentID == pay)
                    {
                        brdb.updateVoucherForBR(br, 1);
                    }
                    else if (br.DocumentID == recpt)
                    {
                        brdb.updateVoucherForBR(br, 2);
                    }
                }
                ListFilteredBR(listOption);
                grdList.Visible = true;
                btnUpdate.Visible = true;
                MessageBox.Show("Updated");
            }
            catch (Exception ex)
            {
                MessageBox.Show("UPDATE FAILED");
                return;
            }
        }

        private Boolean verifycheque()
        {
            Boolean status = true;
            for (int i = 0; i < grdList.Rows.Count; i++)
            {
                if (grdList.Rows[i].Cells["bChequeDate"].Value.ToString().Equals(" ") ||
                    grdList.Rows[i].Cells["bChequeNo"].Value.ToString().Equals(" "))
                {
                    grdList.Rows[i].Cells["bChequeno"].Value = "NULL";
                    status = false;
                }
            }
            return status;
        }
        private Boolean verifyupdate()
        {
            Boolean status = true;
            for (int i = 0; i < grdList.Rows.Count; i++)
            {
                if (grdList.Rows[i].Cells["bBankDate"].Value.ToString().Equals("NULL") || Convert.ToDateTime(grdList.Rows[i].Cells["bBankDate"].Value) == DateTime.Parse("01-01-1900") ||
                    (!(Convert.ToDateTime(grdList.Rows[i].Cells["bChequeDate"].Value) >= Convert.ToDateTime(grdList.Rows[i].Cells["bVoucherDate"].Value))))
                {
                    grdList.Rows[i].Cells["bBankDate"].Value = "NULL";
                }
                else
                {
                    if (!(Convert.ToDateTime(grdList.Rows[i].Cells["bBankDate"].Value) >= Convert.ToDateTime(grdList.Rows[i].Cells["bChequeDate"].Value))
                        || !(Convert.ToDateTime(grdList.Rows[i].Cells["bBankDate"].Value) >= Convert.ToDateTime(grdList.Rows[i].Cells["bVoucherDate"].Value)))
                    {
                        MessageBox.Show("Bank Date is less than cheque Date or Voucher Date of row" + i + 1);
                        grdList.Rows[i].Cells["bBankDate"].Value = "NULL";
                        return false;
                    }
                }
            }
            return status;
        }
        private Boolean verifychequegrid()
        {
            Boolean status = true;
            {
                if (grdList.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in grid");
                    status = false;
                }
            }
            return status;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            removeControlsFromlvPanel();
            grdList.Visible = false;
            btnUpdate.Visible = false;
            btnExportToExcell.Visible = false;
            if (cmbListOption.SelectedItem.ToString() == "ALL")
            {
                listOption = 1;
            }
            else if (cmbListOption.SelectedItem.ToString() == "PENDING")
            {
                listOption = 2;
            }
        }
        private void dtFromDate_ValueChanged(object sender, EventArgs e)
        {
            removeControlsFromlvPanel();
        }
        string updatefailed = "";
        string update = "";

        private List<brentry> getChequeDetails()
        {
            brentry br = new brentry();
            Boolean status = true;
            List<brentry> VDetails = new List<brentry>();
            try
            {
                for (int i = 0; i < grdList.Rows.Count; i++)
                {
                    br = new brentry();
                    br.rowid = Convert.ToInt32(grdList.Rows[i].Cells["bRowID"].Value);
                    br.DocumentID = grdList.Rows[i].Cells["gDocumentID"].Value.ToString();
                    br.Bankrefno = grdList.Rows[i].Cells["bChequeNo"].Value.ToString();
                    br.bankrefdate = Convert.ToDateTime(grdList.Rows[i].Cells["bChequeDate"].Value);
                    br.VoucherDate = Convert.ToDateTime(grdList.Rows[i].Cells["bVoucherDate"].Value);
                    VDetails.Add(br);
                }
            }
            catch (Exception EX)
            {

            }
            return VDetails;
        }
        private void ExportToExcell_Click(object sender, EventArgs e)
        {
            try
            {
                if (verifychequegrid())
                {
                    removeControlsFromPnlLvPanel();
                    pnllv = new Panel();
                    pnllv.BorderStyle = BorderStyle.FixedSingle;

                    pnllv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(100, 100), new Size(600, 300));
                    exlv = Utilities.GridColumnSelectionView(grdList);
                    exlv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(50, 50), new Size(500, 200));
                    pnllv.Controls.Add(exlv);

                    System.Windows.Forms.Label pnlHeading = new System.Windows.Forms.Label();
                    pnlHeading.Size = new Size(300, 20);
                    pnlHeading.Location = new System.Drawing.Point(50, 20);
                    pnlHeading.Text = "Select Gridview Colums to Export";
                    pnlHeading.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                    pnlHeading.ForeColor = Color.Black;
                    pnllv.Controls.Add(pnlHeading);

                    System.Windows.Forms.Button exlvOK = new System.Windows.Forms.Button();
                    exlvOK.Text = "OK";
                    exlvOK.Location = new System.Drawing.Point(50, 270);
                    exlvOK.Click += new System.EventHandler(this.exlvOK_Click1);
                    pnllv.Controls.Add(exlvOK);

                    System.Windows.Forms.Button exlvCancel = new System.Windows.Forms.Button();
                    exlvCancel.Text = "Cancel";
                    exlvCancel.Location = new System.Drawing.Point(150, 270);
                    exlvCancel.Click += new System.EventHandler(this.exlvCancel_Click2);
                    pnllv.Controls.Add(exlvCancel);
                    pnlList.Controls.Add(pnllv);
                    pnllv.BringToFront();
                    pnllv.Visible = true;
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
        private void exlvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                pnllv.Visible = false;
                string heading1 = "BREntry ";
                string heading2 = "";
                string heading3 = "BankCode" + Main.delimiter1 + txtBankAccountCode.Text + Main.delimiter2 +
                    "BankName" + Main.delimiter1 + txtBankAccountName.Text + Main.delimiter2 +
                    "Form Date" + Main.delimiter1 + dtFromDate.Value.ToString("dd/MM/yyyy") + Main.delimiter2 +
                    "To Date" + Main.delimiter1 + dtToDate.Value.ToString("dd/MM/yyyy");
                Utilities.export2Excel(heading1, heading2, heading3, grdList, exlv);
            }
            catch (Exception ex)
            {
            }
        }
        private void exlvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                removeControlsFromPnlLvPanel();
                pnllv.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void dtToDate_ValueChanged(object sender, EventArgs e)
        {
            removeControlsFromlvPanel();
        }

        private void grdList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex == 6)
            //{
            //    oDateTimePicker = new DateTimePicker();
            //    grdList.Controls.Add(oDateTimePicker);
            //    oDateTimePicker.Visible = false;
            //    oDateTimePicker.Format = DateTimePickerFormat.Short; 
            //    oDateTimePicker.TextChanged += new EventHandler(dateTimePicker_OnTextChange);
            //    oDateTimePicker.Visible = true;
            //    Rectangle oRectangle = grdList.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
            //    oDateTimePicker.Size = new Size(oRectangle.Width, oRectangle.Height);
            //    oDateTimePicker.Location = new Point(oRectangle.X, oRectangle.Y);
            //    oDateTimePicker.CloseUp += new EventHandler(oDateTimePicker_CloseUp);
            //}
        }

        private void grdList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string docid = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                int vocuherNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["bVoucherNo"].Value);
                DateTime voucherDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bVoucherDate"].Value);
                if (docid.Length == 0 || docid == "OB")
                {
                    return;
                }
                FileManager.ShowVoucherDetails showVoucher = new FileManager.ShowVoucherDetails(docid, vocuherNo, voucherDate);
                showVoucher.ShowDialog();
                this.RemoveOwnedForm(showVoucher);
            }
            catch (Exception ex)
            {
            }
        }

        private void grdList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //    try
            //    {
            //        if (grdList.Columns[e.ColumnIndex].Name == "bBankDate")
            //        {
            //            brentry br = new brentry();
            //            Boolean status = true;
            //if (Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bBankDate"].Value) != null &&
            //        Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bBankDate"].Value) <= UpdateTable.getSQLDateTime())
            //{
            //    MessageBox.Show("Enter Date less than today!!!");
            //    return;
            //}
            //br = new brentry();
            //br.rowid = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["bRowID"].Value);
            //br.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
            //br.Bankrefno = grdList.Rows[e.RowIndex].Cells["BankReferenceNO"].Value.ToString();
            //br.bankrefdate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["BankReferenceDate"].Value);
            //br.BankDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bBankDate"].Value);
            //br.VoucherDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bVoucherDate"].Value);
            //        VDetails.Add(br);
            //    }

            //}
            //catch (Exception ex)
            //{

            //}

        }

        private void grdList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    if (grdList.IsCurrentCellDirty)
            //    {
            //        var currentcell = grdList.CurrentCellAddress;
            //        var sendingCB = sender as DataGridViewComboBoxEditingControl;
            //        DataGridViewComboBoxCell cel = (DataGridViewComboBoxCell)grdList.Rows[currentcell.Y].Cells[3];
            //        brentry br = new brentry();
            //        Boolean status = true;
            //        if (Convert.ToDateTime(grdList.Rows[currentcell.Y].Cells["bBankDate"].Value) != null &&
            //                Convert.ToDateTime(grdList.Rows[currentcell.Y].Cells["bBankDate"].Value) <= UpdateTable.getSQLDateTime())
            //        {
            //            MessageBox.Show("Enter Date less than today!!!");
            //            return;
            //        }
            //        br = new brentry();
            //        br.rowid = Convert.ToInt32(grdList.Rows[currentcell.Y].Cells["bRowID"].Value);
            //        br.DocumentID = grdList.Rows[currentcell.Y].Cells["gDocumentID"].Value.ToString();
            //        br.Bankrefno = grdList.Rows[currentcell.Y].Cells["BankReferenceNO"].Value.ToString();
            //        br.bankrefdate = Convert.ToDateTime(grdList.Rows[currentcell.Y].Cells["BankReferenceDate"].Value);
            //        br.BankDate = Convert.ToDateTime(grdList.Rows[currentcell.Y].Cells["bBankDate"].Value);
            //        br.VoucherDate = Convert.ToDateTime(grdList.Rows[currentcell.Y].Cells["bVoucherDate"].Value);
            //        VDetails.Add(br);
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void grdList_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //try
            //{
            //    if (grdList.Columns[e.ColumnIndex].Name == "bBankDate")
            //    {
            //        brentry br = new brentry();
            //        Boolean status = true;
            //        if (Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bBankDate"].Value) != null &&
            //                Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bBankDate"].Value) <= UpdateTable.getSQLDateTime())
            //        {
            //            MessageBox.Show("Enter Date less than today!!!");
            //            return;
            //        }
            //        br = new brentry();
            //        br.rowid = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["bRowID"].Value);
            //        br.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
            //        br.Bankrefno = grdList.Rows[e.RowIndex].Cells["BankReferenceNO"].Value.ToString();
            //        br.bankrefdate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["BankReferenceDate"].Value);
            //        br.BankDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bBankDate"].Value);
            //        br.VoucherDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bVoucherDate"].Value);
            //        VDetails.Add(br);
            //    }

            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void grdList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    if (grdList.Columns[e.ColumnIndex].Name == "bBankDate")
            //    {
            //        brentry br = new brentry();
            //        Boolean status = true;
            //        if (Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bBankDate"].Value) != null &&
            //                Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bBankDate"].Value) <= UpdateTable.getSQLDateTime())
            //        {
            //            MessageBox.Show("Enter Date less than today!!!");
            //            return;
            //        }
            //        br = new brentry();
            //        br.rowid = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["bRowID"].Value);
            //        br.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
            //        br.Bankrefno = grdList.Rows[e.RowIndex].Cells["BankReferenceNO"].Value.ToString();
            //        br.bankrefdate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["BankReferenceDate"].Value);
            //        br.BankDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bBankDate"].Value);
            //        br.VoucherDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bVoucherDate"].Value);
            //        VDetails.Add(br);
            //    }

            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void grdList_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //try
            //{
            //    if (grdList.Columns[e.ColumnIndex].Name == "bBankDate")
            //    {
            //        brentry br = new brentry();
            //        Boolean status = true;
            //        if (Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bBankDate"].Value) != null &&
            //                Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bBankDate"].Value) <= UpdateTable.getSQLDateTime())
            //        {
            //            MessageBox.Show("Enter Date less than today!!!");
            //            return;
            //        }
            //        br = new brentry();
            //        br.rowid = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["bRowID"].Value);
            //        br.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
            //        br.Bankrefno = grdList.Rows[e.RowIndex].Cells["BankReferenceNO"].Value.ToString();
            //        br.bankrefdate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["BankReferenceDate"].Value);
            //        br.BankDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bBankDate"].Value);
            //        br.VoucherDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bVoucherDate"].Value);
            //        VDetails.Add(br);
            //    }

            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void gridList_cellcontentchanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                brentry br = new brentry();
                Boolean status = true;
                //if (grdList.Rows[e.RowIndex].Cells["bBankDate"].Value.ToString() != "Null" &&
                //        Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bBankDate"].Value) <= UpdateTable.getSQLDateTime())
                //{
                //    MessageBox.Show("Enter Date less than today!!!");
                //    return;
                //}
                var rwid = VDetails.FirstOrDefault(x => x.rowid == Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["bRowID"].Value)
                                                    && x.BankDate == Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bBankDate"].Value));
                if (rwid == null)
                {
                    br = new brentry();
                    br.rowid = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["bRowID"].Value);
                    br.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    br.Bankrefno = grdList.Rows[e.RowIndex].Cells["BankReferenceNO"].Value.ToString();
                    br.bankrefdate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["BankReferenceDate"].Value);
                    if (grdList.Rows[e.RowIndex].Cells["bBankDate"].Value.ToString() != "NULL")
                    {
                        br.BankDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bBankDate"].Value);
                    }
                    else
                    {
                        br.BankDate = Convert.ToDateTime("NULL");
                    }
                    br.VoucherDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bVoucherDate"].Value);
                    VDetails.Add(br);
                }



            }
            catch (Exception ex)
            {

            }
        }

        private void BREntry_Enter(object sender, EventArgs e)
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





