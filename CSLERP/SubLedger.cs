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
    public partial class SubLedger : System.Windows.Forms.Form
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
        decimal TotDrAmntBal = 0;
        decimal TotCrAmntBal = 0;
        decimal TotOBValue = 0;
        decimal TotDebAmnt = 0;
        decimal TotCredAmnt = 0;
        ListView exlv = new ListView();
        DataGridView payeeCodeGrd = new DataGridView();
        public SubLedger()
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
            dtToDate.Value = UpdateTable.getSQLDateTime();
            dtToDate.CustomFormat = "dd-MM-yyyy";
            btnExportToExcel.Visible = false;
            chktransacted.Checked = true;
            btnCancel.Visible = false;
            lblSearch.Visible = false;
            TxtSearchNamr.Visible = false;
            pnlUI.Controls.Add(pnlList);
            closeAllPanels();
            grdLedger.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdLedger.EnableHeadersVisualStyles = false;
            grdList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            FinancialYearDB.fillFYIDCombo(cmbFYID);
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
            grdList.Visible = false;
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
                CustomerType = "";
                cmbFYID.SelectedIndex = cmbFYID.FindString(Main.currentFY);
                dtFromDate.Value = UpdateTable.getSQLDateTime().AddMonths(-1);
                dtToDate.Value = UpdateTable.getSQLDateTime();
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

        private void dtFromDate_ValueChanged(object sender, EventArgs e)
        {
            btnClose.PerformClick();

            grdList.Rows.Clear();
            TxtSearchNamr.Text = "";
        }
        //private List<ledger> getLedgerDetails()
        //{
        //    ledger ldg = new ledger();
        //    List<ledger> VDetails = new List<ledger>();
        //    try
        //    {
        //        for (int i = 0; i < grdLedger.Rows.Count - 2; i++)
        //        {
        //            ldg = new ledger();
        //            ldg.DocumentID = grdLedger.Rows[i].Cells["gDocumentID"].Value.ToString();
        //            ldg.VoucherNo = Convert.ToInt32(grdLedger.Rows[i].Cells["VoucherNo"].Value.ToString());
        //            ldg.VoucherDate = Convert.ToDateTime(grdLedger.Rows[i].Cells["VoucherDate"].Value);
        //            ldg.TransactionACName = grdLedger.Rows[i].Cells["TransactionACName"].Value.ToString();
        //            ldg.Narration = grdLedger.Rows[i].Cells["Narration"].Value.ToString();
        //            ldg.DebitAmnt = Convert.ToDecimal(grdLedger.Rows[i].Cells["DrAmt"].Value.ToString());
        //            ldg.CreditAmnt = Convert.ToDecimal(grdLedger.Rows[i].Cells["CrAmt"].Value);
        //            VDetails.Add(ldg);
        //        }
        //    }
        //    catch (Exception EX)
        //    {

        //    }
        //    return VDetails;
        //}
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
            try
            {
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
                lblSearch.Visible = true;
                TxtSearchNamr.Visible = true;
                //btnExportToPDF.Visible = true;
                btnCancel.Visible = true;
                btnClose.PerformClick();
                btnExportToExcel.Visible = true;
                //--------
                string[] str = cmbFYID.SelectedItem.ToString().Split(':');
                string s = str[0];
                string ss = str[1];
                string sss = str[2];
                DateTime FYstartDate = Convert.ToDateTime(ss.Trim());
                DateTime FYEndDate = Convert.ToDateTime(sss.Trim());
                List<string> testAvail = new List<string>();
                //--------

                SubLedgerDB ldb = new SubLedgerDB();
                //get all OB for all Party at FY start
                List<CustomerOBdetail> custObList = CustomerOBDB.getCustomerOBListForPericualrFY(s);  //AccountCode : SLCode, Account Name : SLName

                //Accountwise debit total and credit total till from date
                List<sledger> ledgerListFromdt = SubLedgerDB.getPartyWiseDrCrTotTillFromDate(FYstartDate, dtFromDate.Value); //12rows


                foreach (sledger ldg in ledgerListFromdt)
                {
                    CustomerOBdetail accList = custObList.FirstOrDefault(acc => acc.AccountCode == ldg.SLCode);
                    if (accList == null) //If New Account found
                    {
                        CustomerOBdetail custNew = new CustomerOBdetail();
                        custNew.AccountCode = ldg.SLCode;
                        custNew.AccountName = ldg.SLName;
                        custNew.OBValue = (ldg.DebitAmnt - ldg.CreditAmnt);
                        custObList.Add(custNew);
                        testAvail.Add(ldg.SLCode);
                    }
                }
                foreach (CustomerOBdetail cust in custObList)
                {
                    sledger ldg = ledgerListFromdt.FirstOrDefault(ldger => ldger.SLCode == cust.AccountCode);
                    if (ldg != null && !testAvail.Contains(ldg.SLCode))
                    {
                        cust.OBValue = cust.OBValue + (ldg.DebitAmnt - ldg.CreditAmnt); //updating new ob value till from date
                    }
                }
                List<sledger> ledgerListwithinperiod = SubLedgerDB.getPartyWiseDrCrTotWithinPeriod(dtFromDate.Value, dtToDate.Value);
                TotDrAmntBal = 0;
                TotCrAmntBal = 0;
                TotOBValue = 0;
                TotDebAmnt = 0;
                TotCredAmnt = 0;

                int i = 0;
                //List<AccountOBdetail> acclIst = accObList.OrderBy(acc => acc.AccountName).ToList();
                foreach (CustomerOBdetail cust in custObList)
                {
                    i++;
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["SiNo"].Value = grdList.RowCount;
                    grdList.Rows[grdList.RowCount - 1].Cells["PartyCode"].Value = cust.AccountCode; //For party code
                    grdList.Rows[grdList.RowCount - 1].Cells["PartyName"].Value = cust.AccountName; //For party name
                    grdList.Rows[grdList.RowCount - 1].Cells["OpeningBalance"].Value = cust.OBValue;
                    TotOBValue = TotOBValue + cust.OBValue;
                    sledger ldgInPeriod = ledgerListwithinperiod.FirstOrDefault(ldgr => ldgr.SLCode == cust.AccountCode);
                    if (ldgInPeriod != null)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["DebitAmount"].Value = ldgInPeriod.DebitAmnt; //withing period total debit amount
                        grdList.Rows[grdList.RowCount - 1].Cells["CreditAmount"].Value = ldgInPeriod.CreditAmnt;  ////withing period total credit amount

                        TotDebAmnt = TotDebAmnt + ldgInPeriod.DebitAmnt;
                        TotCredAmnt = TotCredAmnt + ldgInPeriod.CreditAmnt;
                        grdList.Rows[grdList.RowCount - 1].Cells["gBalance"].Value = cust.OBValue + (ldgInPeriod.DebitAmnt - ldgInPeriod.CreditAmnt);
                    }
                    else
                    {
                        //No transaction found within time period
                        grdList.Rows[grdList.RowCount - 1].Cells["DebitAmount"].Value = Convert.ToDecimal(0);
                        grdList.Rows[grdList.RowCount - 1].Cells["CreditAmount"].Value = Convert.ToDecimal(0);
                        grdList.Rows[grdList.RowCount - 1].Cells["gBalance"].Value = cust.OBValue;
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
                foreach (sledger ldgrnew in ledgerListwithinperiod)
                {
                    CustomerOBdetail CustList = custObList.FirstOrDefault(acc => acc.AccountCode == ldgrnew.SLCode);
                    if (CustList == null) //If New Account found OB is nill
                    {
                        i++;
                        grdList.Rows.Add();
                        grdList.Rows[grdList.RowCount - 1].Cells["SiNo"].Value = grdList.RowCount;
                        grdList.Rows[grdList.RowCount - 1].Cells["PartyCode"].Value = ldgrnew.SLCode;
                        grdList.Rows[grdList.RowCount - 1].Cells["PartyName"].Value = ldgrnew.SLName;
                        grdList.Rows[grdList.RowCount - 1].Cells["OpeningBalance"].Value = Convert.ToDecimal(0);
                        grdList.Rows[grdList.RowCount - 1].Cells["DebitAmount"].Value = ldgrnew.DebitAmnt; //withing period total debit amount
                        grdList.Rows[grdList.RowCount - 1].Cells["CreditAmount"].Value = ldgrnew.CreditAmnt;  ////withing period total credit amount
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
                    grdList.Rows[grdList.RowCount - 1].Cells["PartyName"].Value = "Total";
                    grdList.Rows[grdList.RowCount - 1].Cells["OpeningBalance"].Value = TotOBValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["DebitAmount"].Value = TotDebAmnt;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreditAmount"].Value = TotCredAmnt;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreditBalance"].Value = TotCrAmntBal < Convert.ToDecimal(0) ? TotCrAmntBal * (-1) : TotCrAmntBal;
                    grdList.Rows[grdList.RowCount - 1].Cells["DebitBalance"].Value = TotDrAmntBal < Convert.ToDecimal(0) ? TotDrAmntBal * (-1) : TotDrAmntBal;
                }
                chktransacted.Checked = false;
                chktransacted.Checked = true;
            }
            catch (Exception ex)
            {
            }


            ////if (txtBankAccountCode.Text.Length == 0)
            //{
            //    List<sledger> LedgerList = ldb.getsledger(FYstartDate, dtFromDate.Value, dtToDate.Value, txtCustomerID.Text, CustomerType);
            //    decimal TotDrAmnt = 0;
            //    decimal TotCrAmnt = 0;
            //    int i = 0;
            //    foreach (sledger ldg in LedgerList)
            //    {
            //        i++;
            //        grdList.Rows.Add();
            //        grdList.Rows[grdList.RowCount - 1].Cells["SerialNo"].Value = grdList.RowCount;
            //        grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = ldg.DocumentID;
            //        grdList.Rows[grdList.RowCount - 1].Cells["VoucherNo"].Value = ldg.VoucherNo;
            //        grdList.Rows[grdList.RowCount - 1].Cells["VoucherDate"].Value = ldg.VoucherDate;
            //        grdList.Rows[grdList.RowCount - 1].Cells["TransactionACName"].Value = ldg.TransactionACName;
            //        grdList.Rows[grdList.RowCount - 1].Cells["Narration"].Value = ldg.Narration;
            //        grdList.Rows[grdList.RowCount - 1].Cells["DrAmt"].Value = ldg.DebitAmnt;
            //        grdList.Rows[grdList.RowCount - 1].Cells["CrAmt"].Value = ldg.CreditAmnt;
            //        TotCrAmnt += ldg.CreditAmnt;
            //        TotDrAmnt += ldg.DebitAmnt;

            //    }
            //    if (i == LedgerList.Count)
            //    {

            //        grdList.Rows.Add();
            //        grdList.Rows[grdList.RowCount - 1].Cells["VoucherDate"].Value = dtToDate.Value;
            //        grdList.Rows[grdList.RowCount - 1].Cells["TransactionACName"].Value = "Balance";
            //        if (TotDrAmnt > TotCrAmnt)
            //        {
            //            grdList.Rows[grdList.RowCount - 1].Cells["CrAmt"].Value = TotDrAmnt - TotCrAmnt;
            //            balance = balance + "Credit:" + (TotDrAmnt - TotCrAmnt);
            //        }
            //        else if (TotDrAmnt < TotCrAmnt)
            //        {
            //            grdList.Rows[grdList.RowCount - 1].Cells["DrAmt"].Value = TotCrAmnt - TotDrAmnt;
            //            balance = balance + "Debit:" + (TotCrAmnt - TotDrAmnt);
            //        }
            //        grdList.Rows.Add();
            //        grdList.Rows[grdList.RowCount - 1].Cells["TransactionACName"].Value = "Total Amount";
            //        grdList.Rows[grdList.RowCount - 1].Cells["DrAmt"].Value = TotCrAmnt > TotDrAmnt ? TotCrAmnt : TotDrAmnt;
            //        grdList.Rows[grdList.RowCount - 1].Cells["CrAmt"].Value = TotCrAmnt > TotDrAmnt ? TotCrAmnt : TotDrAmnt;
            //        balance = balance + ";" + grdList.Rows[grdList.RowCount - 1].Cells["CrAmt"].Value.ToString();
            //    }
            //}
        }
        private void btnExportToExcel_Click(object sender, EventArgs e)
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
                string heading1 = "SUB LEDGER";
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
            Cancl();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            ///pnlPDFShow.Visible = false;
           // btnClose.Visible = false;
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
            btnClose.PerformClick();
            TxtSearchNamr.Text = "";
        }

        private void dtToDate_ValueChanged(object sender, EventArgs e)
        {
            btnClose.PerformClick();
            grdList.Rows.Clear();
            TxtSearchNamr.Text = "";
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

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string colName = grdList.Columns[e.ColumnIndex].Name;
                if (colName == "Details" && e.RowIndex != grdList.Rows.Count - 1)
                {
                    string partycode = grdList.Rows[e.RowIndex].Cells["PartyCode"].Value.ToString();
                    string partyname = grdList.Rows[e.RowIndex].Cells["PartyName"].Value.ToString();
                    List<sledger> ledgerList = SubLedgerDB.GetLedgerDetailsPerParty(partycode, dtFromDate.Value, dtToDate.Value);

                    txtAcCode.Text = partycode;
                    txtAcName.Text = partyname;
                    txtFromDate.Text = dtFromDate.Value.ToString("dd-MM-yyyy");
                    txtToDate.Text = dtToDate.Value.ToString("dd-MM-yyyy");
                    decimal TotDrAmnt = 0;
                    decimal TotCrAmnt = 0;
                    int i = 0;

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
                    foreach (sledger ldg in ledgerList)
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
                        grdLedger.Rows[grdLedger.RowCount - 1].Cells["VoucherDate"].Value = dtToDate.Value;
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
                    "Party Code " + Main.delimiter1 + txtAcCode.Text + Main.delimiter2 + "Party Name " + Main.delimiter1 + txtAcName.Text;
                string heading1 = "SUB LEDGER";
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

                        if (!row.Cells["PartyName"].Value.ToString().Trim().ToLower().Contains(TxtSearchNamr.Text.Trim().ToLower()))
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
        private Boolean checkTransacted(int rowNo)
        {
            Boolean stat = true;
            if (Convert.ToDecimal(grdList.Rows[rowNo].Cells["OpeningBalance"].Value.ToString()) == 0 &&
                              Convert.ToDecimal(grdList.Rows[rowNo].Cells["DebitAmount"].Value.ToString()) == 0 &&
                              Convert.ToDecimal(grdList.Rows[rowNo].Cells["CreditAmount"].Value.ToString()) == 0 &&
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
                    if (row.Cells["SiNo"].Value != null)
                    {
                        i++;
                        grdList.Rows[row.Index].Cells["SiNo"].Value = i;
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
                    if (grdList.Rows[i].Cells["SiNo"].Value == null &&
                        grdList.Rows[i].Cells["PartyCode"].Value == null &&
                        grdList.Rows[i].Cells["PartyName"].Value.ToString().Trim() == "Total")
                    {
                        grdList.Rows.RemoveAt(i);
                        break;
                    }
                }
                grdList.Rows.Add();
                grdList.Rows[grdList.RowCount - 1].Cells["PartyName"].Value = "Total";
                grdList.Rows[grdList.RowCount - 1].Cells["OpeningBalance"].Value = TotOBValue;
                grdList.Rows[grdList.RowCount - 1].Cells["DebitAmount"].Value = TotDebAmnt;
                grdList.Rows[grdList.RowCount - 1].Cells["CreditAmount"].Value = TotCredAmnt;
                grdList.Rows[grdList.RowCount - 1].Cells["CreditBalance"].Value = TotCrAmntBal < 0 ? TotCrAmntBal * (-1) : TotCrAmntBal;
                grdList.Rows[grdList.RowCount - 1].Cells["DebitBalance"].Value = TotDrAmntBal < 0 ? TotDrAmntBal * (-1) : TotDrAmntBal;
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

        private void SubLedger_Enter(object sender, EventArgs e)
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



