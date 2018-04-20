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
    public partial class FillChequeDetails : System.Windows.Forms.Form
    {
        string docID = "FillChequeDetails";
        string update = "UPDATED Rows:";
        string updatefailed = "Validation Failed Rows:";
        string bankcode = "";
        string userString = "";
        string userCommentStatusString = "";
        Form dtpForm = new Form();
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Boolean userIsACommenter = false;
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        JournalVoucherHeader prevjvh = new JournalVoucherHeader();
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
        public FillChequeDetails()
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
        private void FillChequeDetails_Load(object sender, EventArgs e)
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
            ListFilteredBR();
        }
        private void ListFilteredBR()
        {
            try
            {
                grdList.Rows.Clear();
                FillChequeDetailsDB brdb = new FillChequeDetailsDB();
                string docid = "BANKPAYMENTVOUCHER";
                List<chequedet> BREList = brdb.getFilteredchequedet(dtFromDate.Value, dtToDate.Value, txtBankAccountCode.Text, docid);         
                foreach (chequedet br in BREList)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["bRowID"].Value = br.rowid;
                    grdList.Rows[grdList.RowCount - 1].Cells["bSerialNo"].Value = grdList.RowCount;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = br.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["bPartyName"].Value = br.PartyName;
                    grdList.Rows[grdList.RowCount - 1].Cells["bAmountReceived"].Value = br.AmountRecieved;
                    grdList.Rows[grdList.RowCount - 1].Cells["bAmountPaid"].Value = br.AmountPaid;
                    grdList.Rows[grdList.RowCount - 1].Cells["bChequeNo"].Value = br.ChequeNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["bChequeDate"].Value = br.ChequeDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["bVoucherNo"].Value = br.VoucherNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["bVoucherDate"].Value = br.VoucherDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["bAccountName"].Value = br.BankAccountName;
                    grdList.Rows[grdList.RowCount - 1].Cells["bTemporaryNo"].Value = br.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["bTemporaryDate"].Value = br.TemporaryDate;
                    if (br.BankDate == DateTime.Parse("01-01-1900"))
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["bBankDate"].Value = "NULL";
                    }
                    else
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["bBankDate"].Value = br.BankDate;//.ToString("dd-MM-yyyy");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;
        }
        private void initVariables()
        {

            if (getuserPrivilegeStatus() == 1)
            {
                listOption = 6;
            }
            dtFromDate.Enabled = true;
            dtToDate.Enabled = true;
            txtBankAccountCode.Enabled = false;
            txtBankAccountName.Enabled = false;
            docID = Main.currentDocument;
            dtFromDate.Format = DateTimePickerFormat.Custom;
            dtFromDate.CustomFormat = "dd-MM-yyyy";
            dtToDate.Format = DateTimePickerFormat.Custom;
            dtToDate.CustomFormat = "dd-MM-yyyy";
            pnlUI.Controls.Add(pnlList);
            closeAllPanels();
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
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        public void clearData()
        {
            try
            {
                grdList.Rows.Clear();
                dgvComments.Rows.Clear();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                pnllv.Visible = false;

                txtBankAccountCode.Text = "";
                txtBankAccountName.Text = "";
                dtFromDate.Value = DateTime.Parse("01-01-1900");
                dtToDate.Value = DateTime.Parse("01-01-1900");
                removeControlsFromlvPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                pnlUI.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
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
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                return false;
            }
            return status;
        }
        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredBR();
        }
        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredBR();
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
            ListFilteredBR();
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
                    DateTime DT = DateTime.Today;
                    DT = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["bChequeDate"].Value);
                    Rectangle tempRect = grdList.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                    showDtPickerForm(Cursor.Position.X, Cursor.Position.Y, tempRect.Location, DT);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        void cellDateTimePickerValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            grdList.Rows[grdList.CurrentCell.RowIndex].Cells[grdList.CurrentCell.ColumnIndex - 1].Value = dtp.Value;

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
                dtpForm.Size = new Size(200, 100);
                dtpForm.Location = new Point(left, top);
                DateTimePicker dt = new DateTimePicker();
                dt.Format = DateTimePickerFormat.Custom;
                dt.CustomFormat = "dd-MM-yyyy";
                dt.ValueChanged += new EventHandler(cellDateTimePickerValueChanged);
                dt.Value = dtvalue;
                dtpForm.Controls.Add(dt);
                {
                    dt.Width = 150;
                    dt.Height = 100;
                    dt.Visible = true;
                    dt.ShowUpDown = true;
                    System.Windows.Forms.SendKeys.Send("%{DOWN}");
                    dtpForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
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
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return reverseString;
        }
        private void btnReverse_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Are you sure to Reverse the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    string s = prevjvh.ForwarderList;
                    string reverseStr = getReverseString(prevjvh.ForwarderList);
                    //do forward activities
                    prevjvh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevjvh.CommentStatus);
                    JournalVoucherDB jvhDB = new JournalVoucherDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevjvh.ForwarderList = reverseStr.Substring(0, ind);
                        prevjvh.ForwardUser = reverseStr.Substring(ind + 3);
                        prevjvh.DocumentStatus = prevjvh.DocumentStatus - 1;
                    }
                    else
                    {
                        prevjvh.ForwarderList = "";
                        prevjvh.ForwardUser = "";
                        prevjvh.DocumentStatus = 1;
                    }
                    if (jvhDB.reverseJournalVoucherHeader(prevjvh))
                    {
                        MessageBox.Show("PR Document Reversed");
                        closeAllPanels();
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
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
                string update = "";
                string updatefailed = "";
                btnUpdate.Visible = false;
                grdList.Visible = false;
                btnExit.Visible = true;
                removeControlsFromlvPanel();
                pnlBottomButtons.Visible = true;
                if (btnName == "init")
                {
                    btnExit.Visible = true;
                }      
                else if (btnName == "btnCheck")
                {
                    grdList.Visible = true;
                    btnUpdate.Visible = true;           
                }             
                int ups = getuserPrivilegeStatus();
                if (ups == 1 || ups == 0)
                {
                    if (ups == 1)
                    {
                        grdList.Columns["bChequeNo"].ReadOnly = true;
                        grdList.Columns["dt"].Visible = false;
                        btnUpdate.Visible = false;
                    }
                    else
                    {
                        grdList.Columns["bChequeNo"].ReadOnly = false;
                        grdList.Columns["dt"].Visible = true;
                        btnUpdate.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
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
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
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
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
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
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
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
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
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
                ////////MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                

            }
        }
        private void showBankLIstView()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void lvOK_Click3(object sender, EventArgs e)
        {
            try
            {
                if (lv.Visible == true)
                {
                    if (!checkLVItemChecked("Account"))
                    {
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
                            pnllv.Visible = false;
                        }
                    }
                }
                else
                {
                    if (!checkLVCopyItemChecked("Account"))
                    {
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
                            pnllv.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void lvCancel_Click3(object sender, EventArgs e)
        {
            try
            {
                lv.CheckBoxes = false;
                lv.CheckBoxes = true;
                pnllv.Controls.Remove(lv);
                pnllv.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        //private void listView1_ItemChecked3(object sender, ItemCheckedEventArgs e)
        //{
        //    try
        //    {
        //        if (lv.CheckedIndices.Count > 1)
        //        {
        //            MessageBox.Show("Cannot select more than one item");
        //            e.Item.Checked = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        //    }
        //}
        private void btnCalculate_Click(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            btnUpdate.Visible = false;
            grdList.Visible = false;
            showBankLIstView();
        }
        private void btnCheck_Click(object sender, EventArgs e)
        {
            try
            {
                removeControlsFromlvPanel();
                if (verifyAndReworkBankData())
                {
                    ListFilteredBR();
                    setButtonVisibility("btnCheck");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            FillChequeDetailsDB brdb = new FillChequeDetailsDB();
            Boolean status = true;
            try
            {
                List<chequedet> brlist = getChequeDetails();
                if (brdb.updateChequeDetailpay(brlist))
                {
                    MessageBox.Show(update + "\n" + updatefailed);
                }
                else
                {
                    MessageBox.Show("Failed to update");
                }
                ListFilteredBR();
                btnUpdate.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                return;
            }
        }
        private Boolean verifyupdate()
        {
            Boolean status = true;
            try
            {
                for (int i = 0; i < grdList.Rows.Count; i++)
                {
                    if (grdList.Rows[i].Cells["bBankDate"].Value.ToString().Equals("NULL") || Convert.ToDateTime(grdList.Rows[i].Cells["bBankDate"].Value) == DateTime.Parse("01-01-1900"))
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        private Boolean verifycheque()
        {
            Boolean status = true;
            {

                try
                {
                    for (int i = 0; i < grdList.Rows.Count; i++)
                    {
                        if (!(Convert.ToDateTime(grdList.Rows[i].Cells["bChequeDate"].Value) >= Convert.ToDateTime(grdList.Rows[i].Cells["bVoucherDate"].Value)))
                        {
                            MessageBox.Show("Cheque date is less than voucher date in row" + i + 1);
                            //status=false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                    status = false;
                }
            }
            return status;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {         
        }
        private void dtFromDate_ValueChanged(object sender, EventArgs e)
        {
            removeControlsFromlvPanel();
        }
        private List<chequedet> getChequeDetails()
        {
            update = "UPDATED Rows:";
            updatefailed = "Validation Failed Rows:";
            chequedet br = new chequedet();
            Boolean status = true;
            List<chequedet> VDetails = new List<chequedet>();
            try
            {
                for (int i = 0; i < grdList.Rows.Count; i++)
                {
                    if (!(Convert.ToDateTime(grdList.Rows[i].Cells["bChequeDate"].Value) >= Convert.ToDateTime(grdList.Rows[i].Cells["bVoucherDate"].Value)))
                    {
                        updatefailed += i + 1;
                    }
                    else
                    {
                        br = new chequedet();
                        br.rowid = Convert.ToInt32(grdList.Rows[i].Cells["bRowID"].Value);
                        br.DocumentID = grdList.Rows[i].Cells["gDocumentID"].Value.ToString();
                        br.TemporaryNo = Convert.ToInt32(grdList.Rows[i].Cells["bTemporaryNO"].Value);
                        br.TemporaryDate = Convert.ToDateTime(grdList.Rows[i].Cells["bTemporaryDate"].Value);
                        br.ChequeNo = grdList.Rows[i].Cells["bChequeNo"].Value.ToString();
                        br.ChequeDate = Convert.ToDateTime(grdList.Rows[i].Cells["bChequeDate"].Value);
                        br.VoucherDate = Convert.ToDateTime(grdList.Rows[i].Cells["bVoucherDate"].Value);
                        VDetails.Add(br);
                        update += i + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return VDetails;
        }
        private Boolean checkLVItemChecked(string str)
        {
            Boolean status = true;
            try
            {
                if (lv.CheckedIndices.Count > 1)
                {
                    MessageBox.Show("Only one " + str + " allowed");
                    return false;
                }
                if (lv.CheckedItems.Count == 0)
                {
                    MessageBox.Show("select one " + str);
                    return false;
                }
            }
            catch (Exception)
            {
            }
            return status;
        }
        private Boolean checkLVCopyItemChecked(string str)
        {
            Boolean status = true;
            try
            {
                if (lvCopy.CheckedIndices.Count > 1)
                {
                    MessageBox.Show("Only one " + str + " allowed");
                    return false;
                }
                if (lvCopy.CheckedItems.Count == 0)
                {
                    MessageBox.Show("select one " + str);
                    return false;
                }
            }
            catch (Exception)
            {
            }
            return status;
        }

        private void dtToDate_ValueChanged(object sender, EventArgs e)
        {
            removeControlsFromlvPanel();
        }

        private void FillChequeDetails_Enter(object sender, EventArgs e)
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



