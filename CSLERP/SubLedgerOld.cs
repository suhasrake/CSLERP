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
    public partial class SubLedgerOld : System.Windows.Forms.Form
    {
        string docID = "";
        string balance = "";
        Form dtpForm = new Form();
        LedgerDB brdb = new LedgerDB();
        Timer filterTimer = new Timer();
        ListView lvCopy = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        TextBox txtSearch = new TextBox();
        string CustomerType = "";
        Form frmPopup = new Form();
        DataGridView payeeCodeGrd = new DataGridView();
        public SubLedgerOld()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }
        private void SubLedger_Load(object sender, EventArgs e)
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
            //tabControl1.SelectedIndexChanged += new EventHandler(TabControl1_SelectedIndexChanged);
        }
        //called only in the beginning
        private void initVariables()
        {
            docID = Main.currentDocument;
            dtFromDate.Format = DateTimePickerFormat.Custom;
            dtFromDate.CustomFormat = "dd-MM-yyyy";
            dtFromDate.Value = UpdateTable.getSQLDateTime().AddMonths(-1);
            dtToDate.Format = DateTimePickerFormat.Custom;
            dtToDate.CustomFormat = "dd-MM-yyyy";
            pnlUI.Controls.Add(pnlList);
            closeAllPanels();
            grdList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            FinancialYearDB.fillFYIDCombo(cmbFYID);
            cmbFYID.SelectedIndex = cmbFYID.FindString(Main.currentFY);
            //userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            //userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
            pnlList.Visible = true;
            Cancl();
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
                balance = "";
                pnllv.Visible = false;
                ////txtBankAccountCode.Text = "";
                ////txtBankAccountName.Text = "";
                txtCustomerID.Text = "";
                txtCustomerName.Text = "";
                CustomerType = "";
                cmbFYID.SelectedIndex = cmbFYID.FindString(Main.currentFY);
                dtFromDate.Value = UpdateTable.getSQLDateTime().AddMonths(-1);
                dtToDate.Value = UpdateTable.getSQLDateTime();
                removeControlsFromlvPanel();
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
        private void setButtonVisibility(string btnName)
        {
            try
            {
                ////btnClr.Visible = false;
                grdList.Visible = false;
                btnExit.Visible = true;
                pnlBottomButtons.Visible = true;
                // pnlPDFShow.Visible = false;
                //----
                if (btnName == "init")
                {
                    ////btnNew.Visible = true; 24/11/2016
                    btnExit.Visible = true;

                }
                if (btnName == "ExportToPDF")
                {
                    //pnlPDFShow.Visible = true;
                }
            }
            catch (Exception ex)
            {
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
            lvCopy.Columns.Add("AccountCode", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("AccountName", -2, HorizontalAlignment.Left);
            lvCopy.Sorting = System.Windows.Forms.SortOrder.None;
            lvCopy.ColumnClick += new ColumnClickEventHandler(LvColumnClick); ;
            //this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
            //lvCopy.Location = new Point(13, 9);
            //lvCopy.Size = new Size(440, 199);
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
        private void dtFromDate_ValueChanged(object sender, EventArgs e)
        {
            grdList.Rows.Clear();
        }
        private List<ledger> getLedgerDetails()
        {
            ledger ldg = new ledger();
            List<ledger> VDetails = new List<ledger>();
            try
            {
                for (int i = 0; i < grdList.Rows.Count - 2; i++)
                {
                    ldg = new ledger();
                    ldg.DocumentID = grdList.Rows[i].Cells["gDocumentID"].Value.ToString();
                    ldg.VoucherNo = Convert.ToInt32(grdList.Rows[i].Cells["VoucherNo"].Value.ToString());
                    ldg.VoucherDate = Convert.ToDateTime(grdList.Rows[i].Cells["VoucherDate"].Value);
                    ldg.TransactionACName = grdList.Rows[i].Cells["TransactionACName"].Value.ToString();
                    ldg.Narration = grdList.Rows[i].Cells["Narration"].Value.ToString();
                    ldg.DebitAmnt = Convert.ToDecimal(grdList.Rows[i].Cells["DrAmt"].Value.ToString());
                    ldg.CreditAmnt = Convert.ToDecimal(grdList.Rows[i].Cells["CrAmt"].Value);
                    VDetails.Add(ldg);
                }
            }
            catch (Exception EX)
            {

            }
            return VDetails;
        }
        private Boolean verifyFinancialYear()
        {
            Boolean stat = true;
            try
            {

                //--------
                string[] str = cmbFYID.SelectedItem.ToString().Split(':');
                string s = str[0];
                string ss = str[1];
                string sss = str[2];
                DateTime FYstartDate = Convert.ToDateTime(ss.Trim());
                DateTime FYEndDate = Convert.ToDateTime(sss.Trim());
                //--------
                if (dtFromDate.Value.Date < FYstartDate.Date || dtFromDate.Value.Date > FYEndDate.Date)
                {
                    MessageBox.Show("Correct From Date");
                    return false;
                }
                if (dtToDate.Value.Date < FYstartDate.Date || dtToDate.Value.Date > FYEndDate.Date)
                {
                    MessageBox.Show("Correct To Date");
                    return false;
                }
                if (dtFromDate.Value.Date > dtToDate.Value.Date)
                {
                    MessageBox.Show("Correct From Date and To Date");
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return stat;
        }
        private void btnVieDetails_Click(object sender, EventArgs e)
        {
            if (txtCustomerID.Text.Length == 0)
            {
                MessageBox.Show("Select Customer");
                return;
            }
            if (cmbFYID.SelectedIndex == -1)
            {
                MessageBox.Show("Select Financial Year");
                return;
            }
            if (!verifyFinancialYear())
            {
                return;
            }
            Cancl();
            balance = "";
            grdList.Visible = true;
            grdList.Rows.Clear();
            btnExportToExcel.Visible = true;
            btnExportToPDF.Visible = true;
            btnCancel.Visible = true;
            removeControlsFromlvPanel();
            //--------
            string[] str = cmbFYID.SelectedItem.ToString().Split(':');
            string s = str[0];
            string ss = str[1];
            string sss = str[2];
            DateTime FYstartDate = Convert.ToDateTime(ss.Trim());
            DateTime FYEndDate = Convert.ToDateTime(sss.Trim());
            //--------

            SubLedgerDB ldb = new SubLedgerDB();
            ////if (txtBankAccountCode.Text.Length == 0)
            {
                List<sledger> LedgerList = ldb.getsledger(FYstartDate, dtFromDate.Value, dtToDate.Value, txtCustomerID.Text, CustomerType);
                decimal TotDrAmnt = 0;
                decimal TotCrAmnt = 0;
                int i = 0;
                foreach (sledger ldg in LedgerList)
                {
                    i++;
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["SerialNo"].Value = grdList.RowCount;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = ldg.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["VoucherNo"].Value = ldg.VoucherNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["VoucherDate"].Value = ldg.VoucherDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["TransactionACName"].Value = ldg.TransactionACName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Narration"].Value = ldg.Narration;
                    grdList.Rows[grdList.RowCount - 1].Cells["DrAmt"].Value = ldg.DebitAmnt;
                    grdList.Rows[grdList.RowCount - 1].Cells["CrAmt"].Value = ldg.CreditAmnt;
                    TotCrAmnt += ldg.CreditAmnt;
                    TotDrAmnt += ldg.DebitAmnt;

                }
                if (i == LedgerList.Count)
                {

                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["VoucherDate"].Value = dtToDate.Value;
                    grdList.Rows[grdList.RowCount - 1].Cells["TransactionACName"].Value = "Balance";
                    if (TotDrAmnt > TotCrAmnt)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["CrAmt"].Value = TotDrAmnt - TotCrAmnt;
                        balance = balance + "Credit:" + (TotDrAmnt - TotCrAmnt);
                    }
                    else if (TotDrAmnt < TotCrAmnt)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["DrAmt"].Value = TotCrAmnt - TotDrAmnt;
                        balance = balance + "Debit:" + (TotCrAmnt - TotDrAmnt);
                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["TransactionACName"].Value = "Total Amount";
                    grdList.Rows[grdList.RowCount - 1].Cells["DrAmt"].Value = TotCrAmnt > TotDrAmnt ? TotCrAmnt : TotDrAmnt;
                    grdList.Rows[grdList.RowCount - 1].Cells["CrAmt"].Value = TotCrAmnt > TotDrAmnt ? TotCrAmnt : TotDrAmnt;
                    balance = balance + ";" + grdList.Rows[grdList.RowCount - 1].Cells["CrAmt"].Value.ToString();
                }
            }
            ////else
            ////{
            ////    List<sledger> LedgerList = ldb.getFilteredsledger(dtFromDate.Value, dtToDate.Value, txtBankAccountCode.Text, txtCustomerID.Text, CustomerType);
            ////    decimal TotDrAmnt = 0;
            ////    decimal TotCrAmnt = 0;
            ////    int i = 0;
            ////    foreach (sledger ldg in LedgerList)
            ////    {
            ////        i++;
            ////        grdList.Rows.Add();
            ////        grdList.Rows[grdList.RowCount - 1].Cells["SerialNo"].Value = grdList.RowCount;
            ////        grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = ldg.DocumentID;
            ////        grdList.Rows[grdList.RowCount - 1].Cells["VoucherNo"].Value = ldg.VoucherNo;
            ////        grdList.Rows[grdList.RowCount - 1].Cells["VoucherDate"].Value = ldg.VoucherDate.ToString("dd-MM-yyyy");
            ////        grdList.Rows[grdList.RowCount - 1].Cells["TransactionACName"].Value = ldg.TransactionACName;
            ////        grdList.Rows[grdList.RowCount - 1].Cells["Narration"].Value = ldg.Narration;
            ////        grdList.Rows[grdList.RowCount - 1].Cells["DrAmt"].Value = ldg.DebitAmnt;
            ////        grdList.Rows[grdList.RowCount - 1].Cells["CrAmt"].Value = ldg.CreditAmnt;
            ////        TotCrAmnt += ldg.CreditAmnt;
            ////        TotDrAmnt += ldg.DebitAmnt;

            ////    }
            ////    if (i == LedgerList.Count)
            ////    {

            ////        grdList.Rows.Add();
            ////        grdList.Rows[grdList.RowCount - 1].Cells["VoucherDate"].Value = dtToDate.Value.ToString("dd-MM-yyyy");
            ////        grdList.Rows[grdList.RowCount - 1].Cells["TransactionACName"].Value = "Balance";
            ////        if (TotDrAmnt > TotCrAmnt)
            ////        {
            ////            grdList.Rows[grdList.RowCount - 1].Cells["CrAmt"].Value = TotDrAmnt - TotCrAmnt;
            ////            balance = balance + "Credit:" + (TotDrAmnt - TotCrAmnt);
            ////        }
            ////        else if (TotDrAmnt < TotCrAmnt)
            ////        {
            ////            grdList.Rows[grdList.RowCount - 1].Cells["DrAmt"].Value = TotCrAmnt - TotDrAmnt;
            ////            balance = balance + "Debit:" + (TotCrAmnt - TotDrAmnt);
            ////        }
            ////        grdList.Rows.Add();
            ////        grdList.Rows[grdList.RowCount - 1].Cells["TransactionACName"].Value = "Total Amount";
            ////        grdList.Rows[grdList.RowCount - 1].Cells["DrAmt"].Value = TotCrAmnt > TotDrAmnt ? TotCrAmnt : TotDrAmnt;
            ////        grdList.Rows[grdList.RowCount - 1].Cells["CrAmt"].Value = TotCrAmnt > TotDrAmnt ? TotCrAmnt : TotDrAmnt;
            ////        balance = balance + ";" + grdList.Rows[grdList.RowCount - 1].Cells["CrAmt"].Value.ToString();
            ////    }
            ////}
        }
        private void btnExportToPDF_Click(object sender, EventArgs e)
        {
            if (grdList.Rows.Count == 0)
            {
                MessageBox.Show("No rows in grid detail");
                return;
            }
            List<ledger> LedgerList = getLedgerDetails();
            string ledList = 
                txtCustomerID.Text + "-" + txtCustomerName.Text + ";" + 
                dtFromDate.Value.ToString("yyyy-MM-dd") +
                                ";" + dtToDate.Value.ToString("yyyy-MM-dd");
            PrintLedgers print = new PrintLedgers();
            print.Printledger(ledList, LedgerList, balance);
            //setButtonVisibility("ExportToPDF");

        }
        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            if(grdList.Rows.Count == 0)
            {
                MessageBox.Show("No rows in grid detail");
                return;
            }
            List<ledger> LedgerList = getLedgerDetails();


            removeControlsFromlvPanel();
            //pnlList.Controls.Remove(pnllv);
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;

            pnllv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(100, 100), new Size(600, 300));
            lv = Utilities.GridColumnSelectionView(grdList);

            lv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(50, 50), new Size(500, 200));
            pnllv.Controls.Add(lv);

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
        private void exlvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                pnllv.Visible = false;
                string heading3 = "Party :" +
                    Main.delimiter1 + txtCustomerID.Text + "-" + txtCustomerName.Text + Main.delimiter2 + "From Date" + Main.delimiter1 +
                    dtFromDate.Value.ToString("yyyy-MM-dd") + Main.delimiter2 + "To Date" + Main.delimiter1 + dtToDate.Value.ToString("yyyy-MM-dd");
                string heading1 = "PARTY LEDGER";
                string heading2 = "";
                Utilities.export2Excel(heading1, heading2, heading3, grdList, lv);
            }
            catch (Exception ex)
            {
            }
        }
        private void exlvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                removeControlsFromlvPanel();
                pnllv.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }
        private void brnSelectAccCode_Click(object sender, EventArgs e)
        {
            grdList.Rows.Clear();
            removeControlsFromlvPanel();
            pnlList.Controls.Remove(pnllv);
            ////btnSelectAccCode.Enabled = false;
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;

            pnllv.Bounds = new Rectangle(new Point(311, 46), new Size(566, 281));
            lv = AccountCodeDB.getAccountCodeListView();
            lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked1);
            lv.Bounds = new Rectangle(new Point(35, 13), new Size(500, 225));
            pnllv.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Location = new Point(71, 245);
            lvOK.Click += new System.EventHandler(this.lvOK_Click2);
            pnllv.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Location = new Point(187, 245);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
            pnllv.Controls.Add(lvCancel);

            Label lblSearch = new Label();
            lblSearch.Text = "Find";
            lblSearch.Location = new Point(364, 249);
            lblSearch.Size = new Size(37, 15);
            pnllv.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Location = new Point(414, 246);
            txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            pnllv.Controls.Add(txtSearch);

            pnlList.Controls.Add(pnllv);
            pnllv.BringToFront();
            pnllv.Visible = true;

            txtSearch.Focus();
        }
        private void lvOK_Click2(object sender, EventArgs e)
        {
            try
            {
                if (lv.Visible == true)
                {
                    if (!checkLVItemChecked("Item"))
                    {
                        return;
                    }
                    foreach (ListViewItem itemRow in lv.Items)
                    {

                        if (itemRow.Checked)
                        {
                            ////txtBankAccountCode.Text = itemRow.SubItems[1].Text;
                            ////txtBankAccountName.Text = itemRow.SubItems[2].Text;
                            ////btnSelectAccCode.Enabled = true;
                            itemRow.Checked = false;
                            pnllv.Controls.Remove(lv);
                            pnllv.Visible = false;
                            grdList.Visible = false;
                            btnExportToExcel.Visible = false;
                            btnCancel.Visible = false;
                            btnExportToPDF.Visible = false;
                            Cancl();
                        }
                    }
                }
                else
                {
                    if (!checkLVItemChecked("Item"))
                    {
                        return;
                    }
                    foreach (ListViewItem itemRow in lvCopy.Items)
                    {

                        if (itemRow.Checked)
                        {
                            ////txtBankAccountCode.Text = itemRow.SubItems[1].Text;
                            ////txtBankAccountName.Text = itemRow.SubItems[2].Text;
                            ////btnSelectAccCode.Enabled = true;
                            itemRow.Checked = false;
                            pnllv.Controls.Remove(lvCopy);
                            pnllv.Visible = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void lvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                ////btnSelectAccCode.Enabled = true;
                lv.CheckBoxes = false;
                lv.CheckBoxes = true;
                pnllv.Controls.Remove(lv);
                pnllv.Visible = false;
            }
            catch (Exception)
            {
            }
        }
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            grdList.Visible = false;
            grdList.Rows.Clear();
            btnExportToPDF.Visible = false;
            btnExportToExcel.Visible = false;
            btnCancel.Visible = false;
            clearData();
            Cancl();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ///pnlPDFShow.Visible = false;
           // btnClose.Visible = false;
        }

        private void btnCustomerSelect_Click(object sender, EventArgs e)
        {
            showPayeeCodeDataGridView();
        }
        private void showPayeeCodeDataGridView()
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

                frmPopup.Size = new Size(610, 370);

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
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInEmpGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                payeeCodeGrd = ListViewFIll.getGridViewForPayeeDetails();

                payeeCodeGrd.Bounds = new Rectangle(new Point(0, 27), new Size(610, 300));
                frmPopup.Controls.Add(payeeCodeGrd);
                payeeCodeGrd.Columns["Name"].Width = 340;

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
            string users = "";
            try
            {
                var checkedRows = from DataGridViewRow r in payeeCodeGrd.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("Select one Payee");
                    return;
                }

                foreach (var row in checkedRows)
                {
                    txtCustomerID.Text = row.Cells["ID"].Value.ToString();
                    txtCustomerName.Text = row.Cells["Name"].Value.ToString();
                    CustomerType = row.Cells["Type"].Value.ToString();
                    grdList.Rows.Clear();
                }

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
            filterGridData();
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }
        private void filterGridData()
        {
            try
            {
                payeeCodeGrd.CurrentCell = null;
                foreach (DataGridViewRow row in payeeCodeGrd.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in payeeCodeGrd.Rows)
                    {
                        if (!row.Cells["Name"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
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

        //private void showCustomerLIstView()
        //{
        //    grdList.Rows.Clear();
        //    removeControlsFromlvPanel();
        //    //pnlList.Controls.Remove(pnllv);
        //    //btnCustomerSelect.Enabled = false;
        //    pnllv = new Panel();
        //    pnllv.BorderStyle = BorderStyle.FixedSingle;

        //    pnllv.Bounds = new Rectangle(new Point(311, 46), new Size(566, 281));
        //    lv = ListViewFIll.getSLListView();
        //    lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
        //    //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked3);
        //    lv.Bounds = new Rectangle(new Point(35, 13), new Size(500, 225));
        //    pnllv.Controls.Add(lv);

        //    Button lvOK = new Button();
        //    lvOK.Text = "OK";
        //    lvOK.Location = new Point(71, 245);
        //    lvOK.Click += new System.EventHandler(this.lvOK_Click3);
        //    pnllv.Controls.Add(lvOK);

        //    Button lvCancel = new Button();
        //    lvCancel.Text = "Cancel";
        //    lvCancel.Location = new Point(187, 245);
        //    lvCancel.Click += new System.EventHandler(this.lvCancel_Click3);
        //    pnllv.Controls.Add(lvCancel);

        //    Label lblSearch = new Label();
        //    lblSearch.Text = "Find";
        //    lblSearch.Location = new Point(364, 249);
        //    lblSearch.Size = new Size(37, 15);
        //    pnllv.Controls.Add(lblSearch);

        //    txtSearch = new TextBox();
        //    txtSearch.Location = new Point(414, 246);
        //    txtSearch.TextChanged += new EventHandler(txtSearch_TextChangedparty);
        //    pnllv.Controls.Add(txtSearch);

        //    pnlList.Controls.Add(pnllv);
        //    pnllv.BringToFront();
        //    pnllv.Visible = true;
        //    txtSearch.Focus();
        //}
        //private void lvOK_Click3(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (lv.Visible == true)
        //        {
        //            if (!checkLVItemChecked("Customer"))
        //            {
        //                return;
        //            }
        //            foreach (ListViewItem itemRow in lv.Items)
        //            {
        //                if (itemRow.Checked)
        //                {

        //                    txtCustomerID.Text = itemRow.SubItems[1].Text;
        //                    txtCustomerName.Text = itemRow.SubItems[2].Text;
        //                    CustomerType = itemRow.SubItems[3].Text;
        //                    //txtRefNo.Text = "";
        //                    //dtRefDate.Value = DateTime.Parse("01-01-1900");
        //                    //btnCustomerSelect.Enabled = true;
        //                    itemRow.Checked = false;
        //                    pnllv.Controls.Remove(lv);
        //                    pnllv.Visible = false;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (!checkLVCopyItemChecked("Customer"))
        //            {
        //                return;
        //            }
        //            foreach (ListViewItem itemRow in lvCopy.Items)
        //            {

        //                if (itemRow.Checked)
        //                {
        //                    txtCustomerID.Text = itemRow.SubItems[1].Text;
        //                    txtCustomerName.Text = itemRow.SubItems[2].Text;
        //                    CustomerType = itemRow.SubItems[3].Text;
        //                    //btnCustomerSelect.Enabled = true;
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
        //private void lvCancel_Click3(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //btnCustomerSelect.Enabled = true;
        //        lv.CheckBoxes = false;
        //        lv.CheckBoxes = true;
        //        pnllv.Controls.Remove(lv);
        //        pnllv.Visible = false;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        ////private void listView1_ItemChecked3(object sender, ItemCheckedEventArgs e)
        ////{
        ////    try
        ////    {
        ////        if (lv.CheckedIndices.Count > 1)
        ////        {
        ////            MessageBox.Show("Cannot select more than one item");
        ////            e.Item.Checked = false;
        ////        }
        ////    }
        ////    catch (Exception)
        ////    {
        ////    }
        ////}
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

        private void cmbFYID_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Rows.Clear();
            removeControlsFromlvPanel();
        }

        private void dtToDate_ValueChanged(object sender, EventArgs e)
        {
            grdList.Rows.Clear();
        }

        private void btnClr_Click(object sender, EventArgs e)
        {
            ////txtBankAccountCode.Clear();
            ////txtBankAccountName.Clear();
            grdList.Visible = false;
            btnExportToExcel.Visible = false;
            btnCancel.Visible = false;
            btnExportToPDF.Visible = false;
            Cancl();
        }
        public void Cancl()
        {
            ////if (txtBankAccountCode.Text.Length == 0)
            ////{
            ////    btnClr.Visible = false;
            ////}
            ////else
            ////{
            ////    btnClr.Visible = true;
            ////}
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

        private void grdList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string docid = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                int vocuherNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["VoucherNo"].Value);
                DateTime voucherDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["VoucherDate"].Value);
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

        private void pnlList_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SubLedgerOld_Enter(object sender, EventArgs e)
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



