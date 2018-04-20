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
    public partial class TrialBalance : System.Windows.Forms.Form
    {
        string docID = "";
        string balance = "";
        Timer filterTimer = new Timer();
        Form dtpForm = new Form();
        LedgerDB brdb = new LedgerDB();
        ListView lvCopy = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        TextBox txtSearch = new TextBox();
        decimal TotDrAmnt = 0;
        decimal TotCrAmnt = 0;
        public TrialBalance()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }
        private void TrialBalance_Load(object sender, EventArgs e)
        {
            //////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;

            String a = this.Size.ToString();
            grdTrialBalance.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdTrialBalance.EnableHeadersVisualStyles = false;
            grdLedger.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdLedger.EnableHeadersVisualStyles = false;
            //tabControl1.SelectedIndexChanged += new EventHandler(TabControl1_SelectedIndexChanged);
        }
        //called only in the beginning
        private void initVariables()
        {
            docID = Main.currentDocument;
            dtTrialBalanceDate.Format = DateTimePickerFormat.Custom;
            dtTrialBalanceDate.CustomFormat = "dd-MM-yyyy";
            pnlUI.Controls.Add(pnlList);
            closeAllPanels();
            grdTrialBalance.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            pnlsearch.Visible = false;
            //userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            //userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            FinancialYearDB.fillFYIDCombo(cmbFY);
            cmbFY.SelectedIndex = cmbFY.FindString(Main.currentFY);
            setButtonVisibility("init");
            pnlList.Visible = true;
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
                txtSearchPu.Text = "";
                pnlsearch.Visible = false;
                dtTrialBalanceDate.Value = DateTime.Today;
                cmbFY.SelectedIndex = cmbFY.FindString(Main.currentFY);
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
                lblSearch.Visible = false;
                closeAllPanels();
                setButtonVisibility("btnNew");
            }
            catch (Exception)
            {

            }
        }
        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //--------
            string[] str = cmbFY.SelectedItem.ToString().Split(':');
            string s = str[0];
            string ss = str[1];
            string sss = str[2];
            DateTime FYstartDate = Convert.ToDateTime(ss.Trim());
            DateTime FYEndDate = Convert.ToDateTime(sss.Trim());
            //--------
            string colName = grdTrialBalance.Columns[e.ColumnIndex].Name;
            if (colName.Equals("Ledger") && e.RowIndex != (grdTrialBalance.Rows.Count - 1))
            {
                try
                {
                    pnlsearch.Visible = false;
                    btnExit.Visible = false;
                    //txtSearchPu.Text = "";
                    string[] yearStr = cmbFY.SelectedItem.ToString().Split(':');
                    DateTime fromDate = Convert.ToDateTime(yearStr[1].Trim());
                    DateTime todate = dtTrialBalanceDate.Value;
                    string AcCode = grdTrialBalance.Rows[e.RowIndex].Cells["AccountCode"].Value.ToString();
                    string AcName = grdTrialBalance.Rows[e.RowIndex].Cells["AccountName"].Value.ToString();
                    grdLedger.Rows.Clear();
                    LedgerDB ldb = new LedgerDB();
                    List<ledger> LedgerList = ldb.getFilteredledger(FYstartDate, fromDate, todate, AcCode);
                    decimal TotDrAmnt = 0;
                    decimal TotCrAmnt = 0;
                    int i = 0;
                    foreach (ledger ldg in LedgerList)
                    {
                        i++;
                        grdLedger.Rows.Add();
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["Sno"].Value = grdLedger.RowCount;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["gDocumentID"].Value = ldg.DocumentID;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["VoucherNo"].Value = ldg.VoucherNo;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["VoucherDate"].Value = ldg.VoucherDate;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["TransactionACName"].Value = ldg.TransactionACName;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["Narration"].Value = ldg.Narration;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["DebitAmount"].Value = ldg.DebitAmnt;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["CreditAmount"].Value = ldg.CreditAmnt;
                        TotCrAmnt = TotCrAmnt + ldg.CreditAmnt;
                        TotDrAmnt = TotDrAmnt + ldg.DebitAmnt;

                    }
                    if (i == LedgerList.Count)
                    {
                        grdLedger.Rows.Add();
                        //////var BtnCell = grdLedger.Rows[grdLedger.RowCount - 1].Cells["Ledger"];
                        //////BtnCell.Value = "";
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["VoucherDate"].Value = todate;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["Narration"].Value = "Closing Balance";
                        if (TotDrAmnt > TotCrAmnt)
                        {
                            grdLedger.Rows[grdLedger.RowCount - 1].Cells["CreditAmount"].Value = TotDrAmnt - TotCrAmnt;
                            balance = balance + "Credit:" + (TotDrAmnt - TotCrAmnt);
                        }
                        else if (TotDrAmnt < TotCrAmnt)
                        {
                            grdLedger.Rows[grdLedger.RowCount - 1].Cells["DebitAmount"].Value = TotCrAmnt - TotDrAmnt;
                            balance = balance + "Debit:" + (TotCrAmnt - TotDrAmnt);
                        }
                        grdLedger.Rows.Add();
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["TransactionACName"].Value = "Total";
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["DebitAmount"].Value = TotCrAmnt > TotDrAmnt ? TotCrAmnt : TotDrAmnt;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["CreditAmount"].Value = TotCrAmnt > TotDrAmnt ? TotCrAmnt : TotDrAmnt;
                        balance = balance + ";" + grdLedger.Rows[grdLedger.RowCount - 1].Cells["CreditAmount"].Value.ToString();
                    }

                    pnlLedger.Visible = true;
                    grdLedger.Visible = true;
                    btnClose.Visible = true;
                    txtAcCode.Text = AcCode;
                    txtAcName.Text = AcName;
                    txtFromDate.Text = fromDate.ToString("dd-MM-yyyy");
                    txtToDate.Text = todate.ToString("dd-MM-yyyy");

                    grdTrialBalance.Visible = false;
                    btnCancel.Visible = false;
                    btnExportToExcel.Visible = false;
                    btnExportToPDF.Visible = false;

                }
                catch (Exception ex)
                {
                }
            }
        }
        private void setButtonVisibility(string btnName)
        {
            try
            {
                grdTrialBalance.Visible = false;
                btnExit.Visible = true;
                pnlBottomButtons.Visible = true;
                pnlLedger.Visible = false;
                grdLedger.Visible = false;
                btnClose.Visible = false;
                grdTrialBalance.Visible = false;
                btnExportToExcel.Visible = false;
                btnExportToPDF.Visible = false;
                btnCancel.Visible = false;
                // pnlPDFShow.Visible = false;
                //----
                if (btnName == "init")
                {
                    ////btnNew.Visible = true; 24/11/2016
                    btnExit.Visible = true;

                }

                if (btnName == "View")
                {
                    grdTrialBalance.Visible = true;
                    btnExportToExcel.Visible = true;
                    btnExportToPDF.Visible = true;
                    btnCancel.Visible = true;
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
        //private void txtSearch_TextChanged(object sender, EventArgs e)
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
            grdTrialBalance.Rows.Clear();
            setButtonVisibility("init");
        }
        private List<ledger> getLedgerDetails()
        {
            ledger ldg = new ledger();
            List<ledger> VDetails = new List<ledger>();
            try
            {
                for (int i = 0; i < grdTrialBalance.Rows.Count - 1; i++)
                {
                    ldg = new ledger();
                    ldg.AccountCode = grdTrialBalance.Rows[i].Cells["AccountCode"].Value.ToString();
                    ldg.AccountName = grdTrialBalance.Rows[i].Cells["AccountName"].Value.ToString();
                    ldg.DebitAmnt = Convert.ToDecimal(grdTrialBalance.Rows[i].Cells["DrAmt"].Value.ToString());
                    ldg.CreditAmnt = Convert.ToDecimal(grdTrialBalance.Rows[i].Cells["CrAmt"].Value);
                    VDetails.Add(ldg);
                }
            }
            catch (Exception EX)
            {

            }
            return VDetails;
        }

        private void btnVieDetails_Click(object sender, EventArgs e)
        {
            if (cmbFY.SelectedIndex == -1)
            {
                MessageBox.Show("Select Financial year");
                return;
            }
            if (!Utilities.verifyFinancialYear(cmbFY.SelectedItem.ToString(), dtTrialBalanceDate.Value))
            {
                MessageBox.Show("Cheque Financial Date");
                return;
            }
            balance = "";
            setButtonVisibility("View");
            grdTrialBalance.Rows.Clear();
            pnlsearch.Visible = true;
            txtSearchPu.Text = "";
            TrialBalanceDB tbDB = new TrialBalanceDB();
            string fyID = cmbFY.SelectedItem.ToString().Substring(0, cmbFY.SelectedItem.ToString().IndexOf(':')).Trim();
            List<ledger> List = tbDB.getFilteredledger(dtTrialBalanceDate.Value, fyID);

            int i = 0;
            foreach (ledger ldg in List)
            {
                if (ldg.DebitAmnt == 0  && ldg.CreditAmnt == 0)
                {
                    continue;
                }
                i++;
                grdTrialBalance.Rows.Add();
                grdTrialBalance.Rows[grdTrialBalance.RowCount - 1].Cells["SerialNo"].Value = grdTrialBalance.RowCount;
                grdTrialBalance.Rows[grdTrialBalance.RowCount - 1].Cells["AccountCode"].Value = ldg.AccountCode;
                grdTrialBalance.Rows[grdTrialBalance.RowCount - 1].Cells["AccountName"].Value = ldg.AccountName;
                if (ldg.DebitAmnt > 0)
                {
                    grdTrialBalance.Rows[grdTrialBalance.RowCount - 1].Cells["DrAmt"].Value = ldg.DebitAmnt;
                    grdTrialBalance.Rows[grdTrialBalance.RowCount - 1].Cells["CrAmt"].Value = "0";
                    TotDrAmnt = TotDrAmnt + ldg.DebitAmnt;
                }
                else
                {
                    grdTrialBalance.Rows[grdTrialBalance.RowCount - 1].Cells["CrAmt"].Value = ldg.DebitAmnt * (-1);
                    grdTrialBalance.Rows[grdTrialBalance.RowCount - 1].Cells["DrAmt"].Value = "0";
                    TotCrAmnt = TotCrAmnt + (ldg.DebitAmnt * (-1));
                }
            }
            ////if (i == List.Count)
            {
                grdTrialBalance.Rows.Add();
                DataGridViewButtonCell BtnCell = (DataGridViewButtonCell)grdTrialBalance.Rows[grdTrialBalance.RowCount - 1].Cells["Ledger"];
                grdTrialBalance.Rows[grdTrialBalance.RowCount - 1].Cells["Ledger"].ReadOnly = true;
                //BtnCell.Dispose();
                grdTrialBalance.Rows[grdTrialBalance.RowCount - 1].Cells["AccountName"].Value = "Total";
                grdTrialBalance.Rows[grdTrialBalance.RowCount - 1].Cells["DrAmt"].Value = TotDrAmnt;
                grdTrialBalance.Rows[grdTrialBalance.RowCount - 1].Cells["CrAmt"].Value = TotCrAmnt;
                //balance = balance +";"+ grdList.Rows[grdList.RowCount - 1].Cells["CrAmt"].Value.ToString();
            }
        }
        private void btnExportToPDF_Click(object sender, EventArgs e)
        {
            List<ledger> List = getLedgerDetails();
            string fyID = cmbFY.SelectedItem.ToString().Substring(0, cmbFY.SelectedItem.ToString().IndexOf(':')).Trim();
            string ledList = cmbFY.SelectedItem.ToString() + ";" + dtTrialBalanceDate.Value.ToString("dd-MM-yyyy");
            PrintTrialBalance print = new PrintTrialBalance();
            string balance = grdTrialBalance.Rows[grdTrialBalance.RowCount - 1].Cells["DrAmt"].Value.ToString(); ;
            byte[] pdfByte = print.PrintTRBalance(ledList, List, balance);
            //setButtonVisibility("ExportToPDF");

        }
        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            List<ledger> LedgerList = getLedgerDetails();
            removeControlsFromlvPanel();
            pnlList.Controls.Remove(pnllv);
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;

            pnllv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(100, 100), new Size(600, 300));
            lv = Utilities.GridColumnSelectionView(grdTrialBalance);

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
                string fyID = cmbFY.SelectedItem.ToString().Substring(0, cmbFY.SelectedItem.ToString().IndexOf('-')).Trim();
                string heading3 = "Financial Year" + Main.delimiter1 + fyID + Main.delimiter2 +
                                     "Trial Balance Date" + Main.delimiter1 + dtTrialBalanceDate.Value;
                string heading1 = "Trial Balance";
                string heading2 = "";
                Utilities.export2Excel(heading1, heading2, heading3, grdTrialBalance, lv);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            grdTrialBalance.Visible = false;
            grdTrialBalance.Rows.Clear();
            btnExportToPDF.Visible = false;
            btnExportToExcel.Visible = false;
            btnCancel.Visible = false;
            clearData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ///pnlPDFShow.Visible = false;
           // btnClose.Visible = false;
        }

        private void cmbFY_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdTrialBalance.Rows.Clear();
            setButtonVisibility("init");
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            grdLedger.Rows.Clear();
            pnlLedger.Visible = false;
            grdLedger.Visible = false;
            btnClose.Visible = false;
            grdTrialBalance.Visible = true;
            btnCancel.Visible = true;
            btnExportToExcel.Visible = true;
            btnExportToPDF.Visible = true;
            pnlsearch.Visible = true;
            btnExit.Visible = true;
            //txtSearchPu.Text = "";
        }

        private void grdTrialBalance_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void grdTrialBalance_Sorted(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < grdTrialBalance.Rows.Count; i++)
                {
                    if (grdTrialBalance.Rows[i].Cells["SerialNo"].Value == null &&
                        grdTrialBalance.Rows[i].Cells["AccountCode"].Value == null &&
                        grdTrialBalance.Rows[i].Cells["AccountName"].Value.ToString().Trim() == "Total")
                    {
                        grdTrialBalance.Rows.RemoveAt(i);
                        break;
                    }
                }
                grdTrialBalance.Rows.Add();
                grdTrialBalance.Rows[grdTrialBalance.Rows.Count - 1].Cells["DrAmt"].Value = TotDrAmnt;
                grdTrialBalance.Rows[grdTrialBalance.Rows.Count - 1].Cells["CrAmt"].Value = TotCrAmnt;
                grdTrialBalance.Rows[grdTrialBalance.Rows.Count - 1].Cells["AccountName"].Value = "Total";
            }
            catch (Exception ex)
            {

            }
        }

        private void grdTrialBalance_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void grdTrialBalance_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtSearchPu_TextChanged(object sender, EventArgs e)
        {
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterTimer.Tick -= new System.EventHandler(this.handlefilterTimerTimeout);
            filterTimer.Tick += new System.EventHandler(this.handlefilterTimerTimeout);
            filterTimer.Interval = 500;
            filterTimer.Enabled = true;
            filterTimer.Start();
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
                foreach (DataGridViewRow row in grdTrialBalance.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearchPu.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdTrialBalance.Rows)
                    {
                        if (!row.Cells["AccountName"].Value.ToString().Trim().ToLower().Contains(txtSearchPu.Text.Trim().ToLower()))
                        {
                            row.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void grdLedger_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string docid = grdLedger.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                int vocuherNo = Convert.ToInt32(grdLedger.Rows[e.RowIndex].Cells["VoucherNo"].Value);
                DateTime voucherDate = Convert.ToDateTime(grdLedger.Rows[e.RowIndex].Cells["VoucherDate"].Value);
                if(docid.Length == 0 || docid == "OB")
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

        private void TrialBalance_Enter(object sender, EventArgs e)
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



