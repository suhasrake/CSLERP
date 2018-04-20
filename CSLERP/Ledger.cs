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
    public partial class Ledger : System.Windows.Forms.Form
    {
        string docID = "";
        Timer filterTimer = new Timer();
        string balance = "";
        Form frmPopup = new Form();
        Form dtpForm = new Form();
        DataGridView grdldg = new DataGridView();
        LedgerDB brdb = new LedgerDB();
        ListView lvCopy = new ListView();
        ListView exlv = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        TextBox txtSearch = new TextBox();
        List<int> RowIndexList = new List<int>();
        decimal TotDrAmntBal = 0;
        decimal TotCrAmntBal = 0;
        decimal TotOBValue = 0;
        decimal TotDebAmnt = 0;
        decimal TotCredAmnt = 0;

        public Ledger()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }

        private void Ledger_Load(object sender, EventArgs e)
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
            dtFromDate.Value = DateTime.Now.AddMonths(-1);
            dtToDate.Format = DateTimePickerFormat.Custom;
            grdLedger.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdLedger.EnableHeadersVisualStyles = false;
            dtToDate.CustomFormat = "dd-MM-yyyy";
            dtToDate.Value = DateTime.Now;
            pnlUI.Controls.Add(pnlList);
            btnExportToExcel.Visible = false;
            chktransacted.Checked = true;
            btnCancel.Visible = false;
            lblSearch.Visible = false;
            TxtSearchNamr.Visible = false;
            //btnExportToPDF.Visible = false;
            closeAllPanels();
            grdList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            FinancialYearDB.fillFYIDCombo(cmbFYID);
            pnlLedger.Visible = false;
            cmbFYID.SelectedIndex = cmbFYID.FindString(Main.currentFY);

            //---------
            string[] str = cmbFYID.SelectedItem.ToString().Split(':');
            string s = str[0];
            string ss = str[1];
            string sss = str[2];
            DateTime FYstartDate = Convert.ToDateTime(ss.Trim());
            DateTime FYEndDate = Convert.ToDateTime(sss.Trim());
            
          
            if (dtFromDate.Value.Date < FYstartDate.Date)
            {
                dtFromDate.Value = FYstartDate.Date;
            }

            //userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            //userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
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
                //txtBankAccountCode.Text = "";
                //txtBankAccountName.Text = "";
                dtFromDate.Value = DateTime.Now.AddMonths(-1);
                dtToDate.Value = DateTime.Today;
                cmbFYID.SelectedIndex = cmbFYID.FindString(Main.currentFY);
                removeControlsFromlvPanel();
                pnlLedger.Visible = false;

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
        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string colName = grdList.Columns[e.ColumnIndex].Name;
                if (colName == "Details" && e.RowIndex != grdList.Rows.Count - 1)
                {
                    string accode = grdList.Rows[e.RowIndex].Cells["AccountCode"].Value.ToString();
                    string acname = grdList.Rows[e.RowIndex].Cells["AccountName"].Value.ToString();
                    List<ledger> ledgerList = LedgerDB.GetLedgerDetailsPerCustomer(accode, dtFromDate.Value, dtToDate.Value);

                    txtAcCode.Text = accode;
                    txtAcName.Text = acname;
                    txtFromDate.Text = dtFromDate.Value.ToString("dd-MM-yyyy");
                    txtToDate.Text = dtToDate.Value.ToString("dd-MM-yyyy");
                    decimal TotDrAmnt = 0;
                    decimal TotCrAmnt = 0;
                    int i = 0;
                    //For OB
                    balance = "";
                    grdLedger.Rows.Add();
                    grdLedger.Rows[grdLedger.RowCount - 1].Cells["Sno"].Value = grdLedger.RowCount;
                    grdLedger.Rows[grdLedger.RowCount - 1].Cells["gDocumentID"].Value = "OB";
                    grdLedger.Rows[grdLedger.RowCount - 1].Cells["VoucherNo"].Value = 0;
                    grdLedger.Rows[grdLedger.RowCount - 1].Cells["VoucherDate"].Value = dtFromDate.Value;
                    grdLedger.Rows[grdLedger.RowCount - 1].Cells["TransactionACName"].Value = "Opening Balance";
                    grdLedger.Rows[grdLedger.RowCount - 1].Cells["Narration"].Value = "";
                    decimal ob = Convert.ToDecimal(grdList.Rows[e.RowIndex].Cells["OpeningBalance"].Value.ToString());
                    if (ob >= 0)
                    {
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["gDebitAmount"].Value = ob;
                        TotDrAmnt = TotDrAmnt + ob;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["gCreditAmount"].Value = Convert.ToDecimal(0);
                    }
                    else
                    {
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["gDebitAmount"].Value = Convert.ToDecimal(0);
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["gCreditAmount"].Value = ob * (-1);
                        TotCrAmnt = TotCrAmnt + (ob * (-1));
                    }

                    foreach (ledger ldg in ledgerList)
                    {
                        i++;
                        grdLedger.Rows.Add();
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["Sno"].Value = grdLedger.RowCount;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["gDocumentID"].Value = ldg.DocumentID;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["VoucherNo"].Value = ldg.VoucherNo;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["VoucherDate"].Value = ldg.VoucherDate;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["TransactionACName"].Value = ldg.TransactionACName;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["Narration"].Value = ldg.Narration;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["gDebitAmount"].Value = ldg.DebitAmnt;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["gCreditAmount"].Value = ldg.CreditAmnt;
                        TotCrAmnt = TotCrAmnt + ldg.CreditAmnt;
                        TotDrAmnt = TotDrAmnt + ldg.DebitAmnt;

                    }
                    if (i == ledgerList.Count)
                    {
                        grdLedger.Rows.Add();
                        //////var BtnCell = grdLedger.Rows[grdLedger.RowCount - 1].Cells["Ledger"];
                        //////BtnCell.Value = "";
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["VoucherDate"].Value = dtToDate.Value.ToString("yyyy-MM-dd");
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["Narration"].Value = "Closing Balance";
                        if (TotDrAmnt > TotCrAmnt)
                        {
                            grdLedger.Rows[grdLedger.RowCount - 1].Cells["gCreditAmount"].Value = TotDrAmnt - TotCrAmnt;
                            balance = balance + "Credit:" + (TotDrAmnt - TotCrAmnt);
                        }
                        else if (TotDrAmnt < TotCrAmnt)
                        {
                            grdLedger.Rows[grdLedger.RowCount - 1].Cells["gDebitAmount"].Value = TotCrAmnt - TotDrAmnt;
                            balance = balance + "Debit:" + (TotCrAmnt - TotDrAmnt);
                        }
                        grdLedger.Rows.Add();
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["TransactionACName"].Value = "Total";
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["gDebitAmount"].Value = TotCrAmnt > TotDrAmnt ? TotCrAmnt : TotDrAmnt;
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["gCreditAmount"].Value = TotCrAmnt > TotDrAmnt ? TotCrAmnt : TotDrAmnt;
                        balance = balance + ";" + grdLedger.Rows[grdLedger.RowCount - 1].Cells["gCreditAmount"].Value.ToString();
                    }
                    pnlLedger.Visible = true;
                    grdLedger.Visible = true;
                    btnExportToExcelLdg.Visible = true;
                    btnExprtToPDFLdg.Visible = true;
                    btnClose.Visible = true;
                    grdList.Visible = false;
                    chktransacted.Visible = false;
                    lblSearch.Visible = false;
                    TxtSearchNamr.Visible = false;
                    btnCancel.Visible = false;
                    btnExportToExcel.Visible = false;
                    // btnExportToPDF.Visible = false;
                    btnExit.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("cellclick error");
            }
        }
        private void pnlList_Paint(object sender, PaintEventArgs e)
        {
        }
        private void setButtonVisibility(string btnName)
        {
            try
            {
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
            btnClose.PerformClick();

            grdList.Rows.Clear();
            TxtSearchNamr.Text = "";
        }

        private void btnVieDetails_Click(object sender, EventArgs e)
        {
            //--------
            try
            {
                if (cmbFYID.SelectedIndex == -1)
                {
                    MessageBox.Show("Select Financial Year");
                    return;
                }
                string[] str = cmbFYID.SelectedItem.ToString().Split(':');
                string s = str[0];
                string ss = str[1];
                string sss = str[2];
                DateTime FYstartDate = Convert.ToDateTime(ss.Trim());
                DateTime FYEndDate = Convert.ToDateTime(sss.Trim());
                if (dtFromDate.Value.Date < FYstartDate.Date || dtToDate.Value.Date > FYEndDate.Date ||
                   dtToDate.Value.Date < FYstartDate.Date || dtFromDate.Value.Date > FYEndDate.Date)
                {
                    MessageBox.Show("Dates should be within the FY");
                    return;
                }

                if (dtFromDate.Value.Date > dtToDate.Value.Date)
                {
                    MessageBox.Show("From / To Date mismatch");
                    return;
                }

                balance = "";

                grdList.Visible = true;
                grdList.Rows.Clear();
                btnExportToExcel.Visible = true;
                TxtSearchNamr.Text = "";
                lblSearch.Visible = true;
                TxtSearchNamr.Visible = true;
                btnCancel.Visible = true;
                btnClose.PerformClick();
                List<string> testList = new List<string>();
                btnExportToExcel.Visible = true;
                // btnExportToPDF.Visible = true;
                btnCancel.Visible = true;
                LedgerDB ldb = new LedgerDB();
                //get all OB for all account at FY start
                List<AccountOBdetail> accObList = AccountOBDB.getAccountOBListForPericualrFY(s);  //10 rows

                //Accountwise debit total and credit total till from date
                List<ledger> ledgerListFromdt = LedgerDB.getAccountWiseDrCrTotTillFromDate(FYstartDate, dtFromDate.Value); //12rows


                foreach (ledger ldg in ledgerListFromdt)
                {
                    AccountOBdetail accList = accObList.FirstOrDefault(acc => acc.AccountCode == ldg.AccountCode);
                    if (accList == null) //If New Account found
                    {
                        AccountOBdetail accNew = new AccountOBdetail();
                        accNew.AccountCode = ldg.AccountCode;
                        accNew.AccountName = ldg.AccountName;
                        accNew.OBValue = (ldg.DebitAmnt - ldg.CreditAmnt);
                        accObList.Add(accNew);
                        testList.Add(ldg.AccountCode);
                    }
                }
                foreach (AccountOBdetail acc in accObList)
                {
                    ledger ldg = ledgerListFromdt.FirstOrDefault(ldger => ldger.AccountCode == acc.AccountCode);
                    if (ldg != null && !testList.Contains(ldg.AccountCode))
                    {
                        acc.OBValue = acc.OBValue + (ldg.DebitAmnt - ldg.CreditAmnt); //updating new ob value till from date
                    }
                }
                List<ledger> ledgerListwithinperiod = LedgerDB.getAccountWiseDrCrTotWithinPeriod(dtFromDate.Value, dtToDate.Value);

                //List<ledger> LedgerList = ldb.getFilteredledger(FYstartDate, dtFromDate.Value, dtToDate.Value, txtBankAccountCode.Text);

                TotDrAmntBal = 0;
                TotCrAmntBal = 0;
                TotOBValue = 0;
                TotDebAmnt = 0;
                TotCredAmnt = 0;
                int i = 0;
                //List<AccountOBdetail> acclIst = accObList.OrderBy(acc => acc.AccountName).ToList();
                foreach (AccountOBdetail acc in accObList)
                {
                    i++;
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["SerialNo"].Value = grdList.RowCount;
                    grdList.Rows[grdList.RowCount - 1].Cells["AccountCode"].Value = acc.AccountCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["AccountName"].Value = acc.AccountName;
                    grdList.Rows[grdList.RowCount - 1].Cells["OpeningBalance"].Value = acc.OBValue;
                    TotOBValue = TotOBValue + acc.OBValue;
                    ledger ldgInPeriod = ledgerListwithinperiod.FirstOrDefault(ldgr => ldgr.AccountCode == acc.AccountCode);
                    if (ldgInPeriod != null)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["DebitAmount"].Value = ldgInPeriod.DebitAmnt; //withing period total debit amount
                        grdList.Rows[grdList.RowCount - 1].Cells["CreditAmout"].Value = ldgInPeriod.CreditAmnt;  ////withing period total credit amount

                        TotDebAmnt = TotDebAmnt + ldgInPeriod.DebitAmnt;
                        TotCredAmnt = TotCredAmnt + ldgInPeriod.CreditAmnt;
                        grdList.Rows[grdList.RowCount - 1].Cells["gBalance"].Value = acc.OBValue + (ldgInPeriod.DebitAmnt - ldgInPeriod.CreditAmnt);
                    }
                    else
                    {
                        //No transaction found within time period
                        grdList.Rows[grdList.RowCount - 1].Cells["DebitAmount"].Value = Convert.ToDecimal(0);
                        grdList.Rows[grdList.RowCount - 1].Cells["CreditAmout"].Value = Convert.ToDecimal(0);
                        grdList.Rows[grdList.RowCount - 1].Cells["gBalance"].Value = acc.OBValue;
                    }
                    decimal balancetst = Convert.ToDecimal(grdList.Rows[grdList.RowCount - 1].Cells["gBalance"].Value);
                    if (balancetst > 0)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["DebitBalance"].Value = balancetst;
                        grdList.Rows[grdList.RowCount - 1].Cells["CreditBalance"].Value = Convert.ToDecimal(0);
                    }
                    else
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["CreditBalance"].Value = balancetst * (-1);
                        grdList.Rows[grdList.RowCount - 1].Cells["DebitBalance"].Value = Convert.ToDecimal(0);
                    }
                    TotDrAmntBal = TotDrAmntBal + Convert.ToDecimal(grdList.Rows[grdList.RowCount - 1].Cells["DebitBalance"].Value);
                    TotCrAmntBal = TotCrAmntBal + Convert.ToDecimal(grdList.Rows[grdList.RowCount - 1].Cells["CreditBalance"].Value);
                }
                foreach (ledger ldgrnew in ledgerListwithinperiod)
                {
                    AccountOBdetail accList = accObList.FirstOrDefault(acc => acc.AccountCode == ldgrnew.AccountCode);
                    if (accList == null) //If New Account found OB is nill
                    {
                        i++;
                        grdList.Rows.Add();
                        grdList.Rows[grdList.RowCount - 1].Cells["SerialNo"].Value = grdList.RowCount;
                        grdList.Rows[grdList.RowCount - 1].Cells["AccountCode"].Value = ldgrnew.AccountCode;
                        grdList.Rows[grdList.RowCount - 1].Cells["AccountName"].Value = ldgrnew.AccountName;
                        grdList.Rows[grdList.RowCount - 1].Cells["OpeningBalance"].Value = Convert.ToDecimal(0);
                        grdList.Rows[grdList.RowCount - 1].Cells["DebitAmount"].Value = ldgrnew.DebitAmnt; //withing period total debit amount
                        grdList.Rows[grdList.RowCount - 1].Cells["CreditAmout"].Value = ldgrnew.CreditAmnt;  ////withing period total credit amount
                        //TotOBValue = TotOBValue + acc.OBValue;
                        TotDebAmnt = TotDebAmnt + ldgrnew.DebitAmnt;
                        TotCredAmnt = TotCredAmnt + ldgrnew.CreditAmnt;
                        grdList.Rows[grdList.RowCount - 1].Cells["gBalance"].Value = (ldgrnew.DebitAmnt - ldgrnew.CreditAmnt);

                        decimal balancetst = Convert.ToDecimal(grdList.Rows[grdList.RowCount - 1].Cells["gBalance"].Value);
                        if (balancetst > 0)
                        {
                            grdList.Rows[grdList.RowCount - 1].Cells["DebitBalance"].Value = balancetst;
                            grdList.Rows[grdList.RowCount - 1].Cells["CreditBalance"].Value = Convert.ToDecimal(0);
                        }
                        else
                        {
                            grdList.Rows[grdList.RowCount - 1].Cells["CreditBalance"].Value = balancetst * (-1);
                            grdList.Rows[grdList.RowCount - 1].Cells["DebitBalance"].Value = Convert.ToDecimal(0);
                        }
                        TotDrAmntBal = TotDrAmntBal + Convert.ToDecimal(grdList.Rows[grdList.RowCount - 1].Cells["DebitBalance"].Value);
                        TotCrAmntBal = TotCrAmntBal + Convert.ToDecimal(grdList.Rows[grdList.RowCount - 1].Cells["CreditBalance"].Value);
                    }
                }
                if (i == grdList.Rows.Count)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["AccountName"].Value = "Total";
                    grdList.Rows[grdList.RowCount - 1].Cells["OpeningBalance"].Value = TotOBValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["DebitAmount"].Value = TotDebAmnt;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreditAmout"].Value = TotCredAmnt;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreditBalance"].Value = TotCrAmntBal < 0 ? TotCrAmntBal * (-1) : TotCrAmntBal;
                    grdList.Rows[grdList.RowCount - 1].Cells["DebitBalance"].Value = TotDrAmntBal < 0 ? TotDrAmntBal * (-1) : TotDrAmntBal;
                }
                chktransacted.Checked = false;
                chktransacted.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Check All ");
            }
        }
        private void btnExportToPDF_Click(object sender, EventArgs e)
        {


        }
        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            //List<AccountOBdetail> LedgerDetails = getLedgerDetails();

            try
            {
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.CadetBlue;

                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(450, 310);
                frmPopup.Location = new Point(0, 0);
                System.Windows.Forms.Label pnlHeading1 = new System.Windows.Forms.Label();
                pnlHeading1.Size = new Size(300, 20);
                pnlHeading1.Location = new System.Drawing.Point(5, 5);
                pnlHeading1.Text = "Select Gridview Colums to Export";
                pnlHeading1.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                pnlHeading1.ForeColor = Color.Black;
                frmPopup.Controls.Add(pnlHeading1);

                exlv = Utilities.GridColumnSelectionView(grdList);
                exlv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 25), new Size(450, 250));
                frmPopup.Controls.Add(exlv);

                System.Windows.Forms.Button exlvOK = new System.Windows.Forms.Button();
                exlvOK.Text = "OK";
                exlvOK.BackColor = Color.Tan;
                exlvOK.Location = new System.Drawing.Point(40, 280);
                exlvOK.Click += new System.EventHandler(this.exlvOK_Click1);
                frmPopup.Controls.Add(exlvOK);

                System.Windows.Forms.Button exlvCancel = new System.Windows.Forms.Button();
                exlvCancel.Text = "Cancel";
                exlvCancel.BackColor = Color.Tan;
                exlvCancel.Location = new System.Drawing.Point(130, 280);
                exlvCancel.Click += new System.EventHandler(this.exlvCancel_Click2);
                frmPopup.Controls.Add(exlvCancel);

                frmPopup.ShowDialog();
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
                if (cmbFYID.SelectedIndex == -1)
                {
                    MessageBox.Show("Select Financial Year");
                    return;
                }
                int c = grdList.Rows.Count;
                string[] str = cmbFYID.SelectedItem.ToString().Split(':');
                string s = str[0];
                string ss = str[1];
                string sss = str[2];
                DateTime FYstartDate = Convert.ToDateTime(ss.Trim());
                DateTime FYEndDate = Convert.ToDateTime(sss.Trim());
                string heading3 = "Financial Year" + Main.delimiter1 + s + Main.delimiter2 + "From Date" + Main.delimiter1 +
                    dtFromDate.Value.ToString("dd-MM-yyyy") + Main.delimiter2 + "To Date" + Main.delimiter1 + dtToDate.Value.ToString("dd-MM-yyyy");
                string heading1 = "LEDGER";
                string heading2 = "";
                Utilities.export2Excel(heading1, heading2, heading3, grdList, exlv);
                frmPopup.Close();
                frmPopup.Dispose();
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
            }
            catch (Exception ex)
            {
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            grdList.Visible = false;
            grdList.Rows.Clear();
            TxtSearchNamr.Text = "";
            chktransacted.Checked = true;
            TxtSearchNamr.Visible = false;
            lblSearch.Visible = false;
            //btnExportToPDF.Visible = false;
            btnExportToExcel.Visible = false;
            btnCancel.Visible = false;
            clearData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ///pnlPDFShow.Visible = false;
           // btnClose.Visible = false;
        }

        private void cmbFYID_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Rows.Clear();
            btnClose.PerformClick();
            TxtSearchNamr.Text = "";
            //removeControlsFromlvPanel();
        }

        private void dtToDate_ValueChanged(object sender, EventArgs e)
        {
            btnClose.PerformClick();
            grdList.Rows.Clear();
            TxtSearchNamr.Text = "";
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

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            grdLedger.Rows.Clear();
            pnlLedger.Visible = false;
            grdLedger.Visible = false;
            btnClose.Visible = false;
            lblSearch.Visible = true;
            chktransacted.Visible = true;
            TxtSearchNamr.Visible = true;
            btnExportToExcelLdg.Visible = false;
            btnExprtToPDFLdg.Visible = false;
            grdList.Visible = true;
            btnCancel.Visible = true;
            btnExportToExcel.Visible = true;
            // btnExportToPDF.Visible = true;
            btnExit.Visible = true;
        }

        private void grdLedger_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string docid = grdLedger.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                int vocuherNo = Convert.ToInt32(grdLedger.Rows[e.RowIndex].Cells["VoucherNo"].Value);
                DateTime voucherDate = Convert.ToDateTime(grdLedger.Rows[e.RowIndex].Cells["VoucherDate"].Value);
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

        private void btnExportToExcelLdg_Click(object sender, EventArgs e)
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

                frmPopup.Size = new Size(450, 310);
                frmPopup.Location = new Point(0, 0);
                System.Windows.Forms.Label pnlHeading1 = new System.Windows.Forms.Label();
                pnlHeading1.Size = new Size(300, 20);
                pnlHeading1.Location = new System.Drawing.Point(5, 5);
                pnlHeading1.Text = "Select Gridview Colums to Export";
                pnlHeading1.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                pnlHeading1.ForeColor = Color.Black;
                frmPopup.Controls.Add(pnlHeading1);

                exlv = Utilities.GridColumnSelectionView(grdLedger);
                exlv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 25), new Size(450, 250));
                frmPopup.Controls.Add(exlv);

                System.Windows.Forms.Button exlvOK = new System.Windows.Forms.Button();
                exlvOK.Text = "OK";
                exlvOK.BackColor = Color.Tan;
                exlvOK.Location = new System.Drawing.Point(40, 280);
                exlvOK.Click += new System.EventHandler(this.exlvOK_Click2);
                frmPopup.Controls.Add(exlvOK);

                System.Windows.Forms.Button exlvCancel = new System.Windows.Forms.Button();
                exlvCancel.Text = "Cancel";
                exlvCancel.BackColor = Color.Tan;
                exlvCancel.Location = new System.Drawing.Point(130, 280);
                exlvCancel.Click += new System.EventHandler(this.exlvCancel_Click3);
                frmPopup.Controls.Add(exlvCancel);

                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void exlvOK_Click2(object sender, EventArgs e)
        {
            try
            {
                pnllv.Visible = false;
                if (cmbFYID.SelectedIndex == -1)
                {
                    MessageBox.Show("Select Financial Year");
                    return;
                }
                int c = grdList.Rows.Count;
                string[] str = cmbFYID.SelectedItem.ToString().Split(':');
                string s = str[0];
                string ss = str[1];
                string sss = str[2];
                DateTime FYstartDate = Convert.ToDateTime(ss.Trim());
                DateTime FYEndDate = Convert.ToDateTime(sss.Trim());
                string heading3 = "Financial Year" + Main.delimiter1 + s + Main.delimiter2 + "From Date" + Main.delimiter1 +
                   txtFromDate.Text + Main.delimiter2 + "To Date" + Main.delimiter1 + txtToDate.Text + Main.delimiter2 +
                    "Account Code " + Main.delimiter1 + txtAcCode.Text + Main.delimiter2 + "Account Name " + Main.delimiter1 + txtAcName.Text;
                string heading1 = "LEDGER";
                string heading2 = "";
                Utilities.export2Excel(heading1, heading2, heading3, grdLedger, exlv);
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        private void exlvCancel_Click3(object sender, EventArgs e)
        {
            try
            {
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        private void btnExprtToPDFLdg_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbFYID.SelectedIndex == -1)
                {
                    MessageBox.Show("Select Financial Year");
                    return;
                }
                string[] str = cmbFYID.SelectedItem.ToString().Split(':');
                string s = str[0];
                string ss = str[1];
                string sss = str[2];
                DateTime FYstartDate = Convert.ToDateTime(ss.Trim());
                DateTime FYEndDate = Convert.ToDateTime(sss.Trim());

                List<ledger> LedgerDetails = getLedgerDetails();
                string ledList = txtAcName.Text + ";" +
                    dtFromDate.Value.ToString("dd-MM-yyyy") +
                                    ";" + dtToDate.Value.ToString("dd-MM-yyyy");
                PrintLedgers print = new PrintLedgers();
                print.Printledger(ledList, LedgerDetails, balance);
                setButtonVisibility("ExportToPDF");
            }
            catch (Exception ex)
            {

            }
        }
        private List<ledger> getLedgerDetails()
        {
            ledger ldg = new ledger();
            List<ledger> ldgList = new List<ledger>();
            try
            {
                for (int i = 0; i < grdLedger.Rows.Count - 2; i++)
                {
                    ldg = new ledger();
                    ldg.DocumentID = grdLedger.Rows[i].Cells["gDocumentID"].Value.ToString();
                    ldg.VoucherNo = Convert.ToInt32(grdLedger.Rows[i].Cells["VoucherNo"].Value.ToString());
                    ldg.VoucherDate = Convert.ToDateTime(grdLedger.Rows[i].Cells["VoucherDate"].Value.ToString());
                    ldg.TransactionACName = grdLedger.Rows[i].Cells["TransactionACName"].Value.ToString();
                    ldg.Narration = grdLedger.Rows[i].Cells["Narration"].Value.ToString().Replace("'", "''");
                    ldg.DebitAmnt = Convert.ToDecimal(grdLedger.Rows[i].Cells["gDebitAmount"].Value.ToString());
                    ldg.CreditAmnt = Convert.ToDecimal(grdLedger.Rows[i].Cells["gCreditAmount"].Value.ToString());
                    ldgList.Add(ldg);
                }
            }
            catch (Exception EX)
            {

            }
            return ldgList;
        }

        private void TxtSearchNamr_TextChanged(object sender, EventArgs e)
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
                foreach (DataGridViewRow row in grdList.Rows)
                {
                    if (chktransacted.Checked == true)
                    {
                        if (checkTransacted(row.Index))
                        {
                            row.Visible = true;
                        }
                        else
                        {
                            row.Visible = false;
                        }
                    }
                    else
                        row.Visible = true;
                }

                if (TxtSearchNamr.Text.Length != 0)
                {

                    foreach (DataGridViewRow row in grdList.Rows)
                    {
                       
                        if (!row.Cells["AccountName"].Value.ToString().Trim().ToLower().Contains(TxtSearchNamr.Text.Trim().ToLower()))
                        {
                            row.Visible = false;
                        }
                        else
                        {
                            if (chktransacted.Checked == true)
                            {
                                if (checkTransacted(row.Index))
                                {
                                    row.Visible = true;
                                }
                                else
                                {
                                    row.Visible = false;
                                }
                            }
                        }
                    }

                }
                updateSerielNo();
            }
            catch (Exception ex)
            {

            }
        }

        private void chktransacted_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

                TxtSearchNamr.Text = "";
                if (chktransacted.Checked == true)
                {

                    foreach (DataGridViewRow row in grdList.Rows)
                    {
                        if (row.Visible == true)
                        {
                            if (!checkTransacted(row.Index))
                            {
                                row.Visible = false;
                            }
                        }
                    }
                    updateSerielNo();
                }
                else
                {
                    foreach (DataGridViewRow row in grdList.Rows)
                    {
                        row.Visible = true;
                    }
                    updateSerielNo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception checking: " + ex.ToString());
            }
        }
        private Boolean checkTransacted(int rowNo)
        {
            Boolean stat = true;
            if (Convert.ToDecimal(grdList.Rows[rowNo].Cells["OpeningBalance"].Value.ToString()) == 0 &&
                              Convert.ToDecimal(grdList.Rows[rowNo].Cells["DebitAmount"].Value.ToString()) == 0 &&
                              Convert.ToDecimal(grdList.Rows[rowNo].Cells["CreditAmout"].Value.ToString()) == 0 &&
                              Convert.ToDecimal(grdList.Rows[rowNo].Cells["DebitBalance"].Value.ToString()) == 0 &&
                              Convert.ToDecimal(grdList.Rows[rowNo].Cells["CreditBalance"].Value.ToString()) == 0)
            {
                stat = false;
            }
            return stat;
        }
        private void updateSerielNo()
        {
            int i = 0;
            foreach (DataGridViewRow row in grdList.Rows)
            {
                if (row.Visible == true)
                {
                    if (row.Cells["SerialNo"].Value != null)
                    {
                        i++;
                        grdList.Rows[row.Index].Cells["SerialNo"].Value = i;
                    }
                }
            }
        }
        private void grdList_Sorted(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < grdList.Rows.Count; i++)
                {
                    if (grdList.Rows[i].Cells["SerialNo"].Value == null &&
                        grdList.Rows[i].Cells["AccountCode"].Value == null &&
                        grdList.Rows[i].Cells["AccountName"].Value.ToString().Trim() == "Total")
                    {
                        grdList.Rows.RemoveAt(i);
                        break;
                    }
                }
                grdList.Rows.Add();
                grdList.Rows[grdList.RowCount - 1].Cells["AccountName"].Value = "Total";
                grdList.Rows[grdList.RowCount - 1].Cells["OpeningBalance"].Value = TotOBValue;
                grdList.Rows[grdList.RowCount - 1].Cells["DebitAmount"].Value = TotDebAmnt;
                grdList.Rows[grdList.RowCount - 1].Cells["CreditAmout"].Value = TotCredAmnt;
                grdList.Rows[grdList.RowCount - 1].Cells["CreditBalance"].Value = TotCrAmntBal < 0 ? TotCrAmntBal * (-1) : TotCrAmntBal;
                grdList.Rows[grdList.RowCount - 1].Cells["DebitBalance"].Value = TotDrAmntBal < 0 ? TotDrAmntBal * (-1) : TotDrAmntBal;
                updateSerielNo();
            }
            catch (Exception ex)
            {

            }
        }

        private void Ledger_Enter(object sender, EventArgs e)
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



