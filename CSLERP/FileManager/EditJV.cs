using CSLERP.DBData;
using CSLERP.PrintForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLERP.FileManager
{

    public partial class EditJV : Form
    {
        string JVdocID;
        string InvDociD;
        int InvTemporaryNo;
        DateTime InvTemporaryDate;
        Boolean isUpdate;
        double InvAmount = 0;
        Boolean AddRowClick = false;
        Form frmPopup = new Form();
        TextBox txtSearch = new TextBox();
        DataGridView AccCodeGrd = new DataGridView();
        DataGridView payeeCodeGrd = new DataGridView();
        Timer filterTimer = new Timer();
        public EditJV(string JVdocid, string INvStr,double Invamount, Boolean toupdate)
        {
            InitializeComponent();
            JVdocID = JVdocid;
            string[] strINv = INvStr.Split(';');
            InvDociD = strINv[0];
            InvTemporaryNo = Convert.ToInt32(strINv[1]);
            InvTemporaryDate = Convert.ToDateTime(strINv[2]);
            isUpdate = toupdate;
            InvAmount = Invamount;
            initVariable();
        }
        private void initVariable()
        {
            try
            {
                dtTempDate.Format = DateTimePickerFormat.Custom;
                dtTempDate.CustomFormat = "dd-MM-yyyy";
                dtINVTempDate.Format = DateTimePickerFormat.Custom;
                dtINVTempDate.CustomFormat = "dd-MM-yyyy";
                dtJournalDate.Format = DateTimePickerFormat.Custom;
                dtJournalDate.CustomFormat = "dd-MM-yyyy";
                ShowAllDetails();
                btnClose.Focus();
                if (isUpdate)
                    btnUpdate.Visible = true;
                else
                    btnUpdate.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("initVariable() : Error");
            }
        }
        private Boolean AddPRDetailRow()
        {
            Boolean status = true;
            try
            {

                grdPRDetail.Rows.Add();
                int kount = grdPRDetail.RowCount;
                grdPRDetail.Rows[kount - 1].Cells["LineNo"].Value = kount;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["AccountCode"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["AccountName"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["PartyCode"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["PartyName"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["gSLType"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["AmountDebit"].Value = Convert.ToDecimal(0);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["AmountCredit"].Value = Convert.ToDecimal(0);
                if (AddRowClick)
                {
                    grdPRDetail.FirstDisplayedScrollingRowIndex = grdPRDetail.RowCount - 1;
                    grdPRDetail.CurrentCell = grdPRDetail.Rows[kount - 1].Cells[0];
                }
                grdPRDetail.FirstDisplayedScrollingColumnIndex = 0;
            }

            catch (Exception ex)
            {
                MessageBox.Show("AddPRDetailRow() : Error");
            }

            return status;
        }
        private void ShowAllDetails()
        {

            try
            {
                if (JVdocID == "PJV")
                {
                    invoiceinheader iih = new invoiceinheader();
                    iih.DocumentID = InvDociD;
                    iih.TemporaryNo = InvTemporaryNo;
                    iih.TemporaryDate = InvTemporaryDate;

                    PJVHeader pjvh = PJVDB.getPJVHeaderPerInvoiceIN(iih);
                    if(pjvh.TemporaryNo == 0)
                    {
                        MessageBox.Show("PJV Not prepared");
                        return;
                    }
                    txtTemporarryNo.Text = pjvh.TemporaryNo.ToString();
                    dtTempDate.Value = pjvh.TemporaryDate;
                    txtJournalNo.Text = pjvh.JournalNo.ToString();
                    dtJournalDate.Value = pjvh.JournalDate;
                    txtINVTempNo.Text = pjvh.InvTempNo.ToString();
                    dtINVTempDate.Value = pjvh.InvTempDate;
                    txtnarration.Text = pjvh.Narration.ToString();

                    List<PJVDetail> PJVDetail = PJVDB.getPJVDetail(pjvh);
                    grdPRDetail.Rows.Clear();
                    int i = 0;
                    decimal totCredit = 0;
                    decimal totDebit = 0;
                    foreach (PJVDetail vd in PJVDetail)
                    {
                        AddPRDetailRow();
                        grdPRDetail.Rows[i].Cells["AccountCode"].Value = vd.AccountCode;
                        grdPRDetail.Rows[i].Cells["AccountName"].Value = vd.AccountName;
                        grdPRDetail.Rows[i].Cells["AmountDebit"].Value = vd.AmountDebit;
                        totDebit = totDebit + vd.AmountDebit;
                        grdPRDetail.Rows[i].Cells["AmountCredit"].Value = vd.AmountCredit;
                        totCredit = totCredit + vd.AmountCredit;

                        grdPRDetail.Rows[i].Cells["PartyCode"].Value = vd.SLCode;
                        grdPRDetail.Rows[i].Cells["PartyName"].Value = vd.SLName;
                        grdPRDetail.Rows[i].Cells["gSLType"].Value = vd.SLType;
                        i++;
                    }
                    txtTotalCreditAmnt.Text = totCredit.ToString();
                    txtTotalDebitAmnt.Text = totDebit.ToString();
                    txtAmountInWords.Text = NumberToString.convert(txtTotalDebitAmnt.Text);
                }
                else if (JVdocID == "SJV")
                {
                    invoiceoutheader ioh = new invoiceoutheader();
                    ioh.DocumentID = InvDociD;
                    ioh.TemporaryNo = InvTemporaryNo;
                    ioh.TemporaryDate = InvTemporaryDate;

                    SJVHeader sjvh = SJVDB.getSJVHeaderPerInvoiceOut(ioh);
                    if (sjvh.TemporaryNo == 0)
                    {
                        MessageBox.Show("SJV Not prepared");
                        return;
                    }
                    txtTemporarryNo.Text = sjvh.TemporaryNo.ToString();
                    dtTempDate.Value = sjvh.TemporaryDate;
                    txtJournalNo.Text = sjvh.JournalNo.ToString();
                    dtJournalDate.Value = sjvh.JournalDate;
                    txtINVTempNo.Text = sjvh.InvTempNo.ToString();
                    dtINVTempDate.Value = sjvh.InvTempDate;
                    txtnarration.Text = sjvh.Narration.ToString();

                    List<SJVDetail> SJVdetail = SJVDB.getSJVDetail(sjvh);
                    grdPRDetail.Rows.Clear();
                    int i = 0;
                    decimal totCredit = 0;
                    decimal totDebit = 0;
                    foreach (SJVDetail vd in SJVdetail)
                    {
                        AddPRDetailRow();
                        grdPRDetail.Rows[i].Cells["AccountCode"].Value = vd.AccountCode;
                        grdPRDetail.Rows[i].Cells["AccountName"].Value = vd.AccountName;
                        grdPRDetail.Rows[i].Cells["AmountDebit"].Value = vd.AmountDebit;
                        totDebit = totDebit + vd.AmountDebit;
                        grdPRDetail.Rows[i].Cells["AmountCredit"].Value = vd.AmountCredit;
                        totCredit = totCredit + vd.AmountCredit;

                        grdPRDetail.Rows[i].Cells["PartyCode"].Value = vd.SLCode;
                        grdPRDetail.Rows[i].Cells["PartyName"].Value = vd.SLName;
                        grdPRDetail.Rows[i].Cells["gSLType"].Value = vd.SLType;
                        i++;
                    }
                    txtTotalCreditAmnt.Text = totCredit.ToString();
                    txtTotalDebitAmnt.Text = totDebit.ToString();
                    txtAmountInWords.Text = NumberToString.convert(txtTotalDebitAmnt.Text);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in showing details");
            }
        }
        private void addGridDetail()
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!verifyAndReworkVoucherDetailGridRows())
                {
                    MessageBox.Show("Validation Failed for Purhcase journal Detail");
                    return;
                }
               
                if (JVdocID == "PJV")
                {
                    PJVHeader pjvh = new PJVHeader();
                    pjvh.TemporaryNo = Convert.ToInt32(txtTemporarryNo.Text);
                    pjvh.TemporaryDate = dtTempDate.Value;
                    pjvh.DocumentID = JVdocID;
                    List<PJVDetail> PJVList = getVoucherDetails(pjvh);
                    if (Convert.ToDecimal(txtTotalCreditAmnt.Text) != Convert.ToDecimal(txtTotalDebitAmnt.Text))
                    {
                        MessageBox.Show("Debit and Credit total should be equal.");
                        return;
                    }
                    if (InvAmount != Convert.ToDouble(txtTotalCreditAmnt.Text))
                    {
                        MessageBox.Show("Journal amount should be equal to invoice amount.");
                        return;
                    }
                    if (PJVDB.UpdatePJVHeaderDetailDuringApproveInvoiceIN(PJVList, pjvh))
                    {
                        MessageBox.Show("Purchase journal updated.");

                    }
                }
                else if(JVdocID == "SJV")
                {
                    SJVHeader sjvh = new SJVHeader();
                    sjvh.TemporaryNo = Convert.ToInt32(txtTemporarryNo.Text);
                    sjvh.TemporaryDate = dtTempDate.Value;
                    sjvh.DocumentID = JVdocID;
                    List<SJVDetail> SJVList = getSalesVoucherDetails(sjvh);
                    if (Convert.ToDecimal(txtTotalCreditAmnt.Text) != Convert.ToDecimal(txtTotalDebitAmnt.Text))
                    {
                        MessageBox.Show("Debit and Credit total should be equal.");
                        return;
                    }
                    if (InvAmount != Convert.ToDouble(txtTotalCreditAmnt.Text))
                    {
                        MessageBox.Show("Journal amount should be equal to invoice amount.");
                        return;
                    }
                    if (SJVDB.UpdateSJVHeaderDetailDuringApproveInvoiceIN(SJVList, sjvh))
                    {
                        MessageBox.Show("Sales journal updated.");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update");
            }
        }
        private List<SJVDetail> getSalesVoucherDetails(SJVHeader sjvh)
        {

            SJVDetail Sjvd = new SJVDetail();

            List<SJVDetail> SJVDetails = new List<SJVDetail>();
            for (int i = 0; i < grdPRDetail.Rows.Count; i++)
            {
                try
                {
                    Sjvd = new SJVDetail();
                    Sjvd.DocumentID = sjvh.DocumentID;
                    Sjvd.TemporaryNo = sjvh.TemporaryNo;
                    Sjvd.TemporaryDate = sjvh.TemporaryDate;
                    Sjvd.AccountCode = grdPRDetail.Rows[i].Cells["AccountCode"].Value.ToString();
                    Sjvd.SLType = grdPRDetail.Rows[i].Cells["gSLType"].Value.ToString();
                    Sjvd.SLCode = grdPRDetail.Rows[i].Cells["PartyCode"].Value.ToString();
                    Sjvd.InvDocumentID = InvDociD;
                    Sjvd.InvTempNo = InvTemporaryNo;
                    Sjvd.InvTempDate = InvTemporaryDate;
                    if (grdPRDetail.Rows[i].Cells["AmountDebit"].Value != null)
                        Sjvd.AmountDebit = Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountDebit"].Value.ToString().Trim());
                    else
                        Sjvd.AmountDebit = 0;
                    if (grdPRDetail.Rows[i].Cells["AmountCredit"].Value != null)
                        Sjvd.AmountCredit = Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountCredit"].Value.ToString().Trim());
                    else
                        Sjvd.AmountCredit = 0;
                    SJVDetails.Add(Sjvd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("getVoucherDetails() : Error creating Purchase Journal Details");
                    //status = false;
                }
            }
            return SJVDetails;
        }
        private List<PJVDetail> getVoucherDetails(PJVHeader pjvh)
        {
            
            PJVDetail Pjvd = new PJVDetail();

            List<PJVDetail> PJVDetails = new List<PJVDetail>();
            for (int i = 0; i < grdPRDetail.Rows.Count; i++)
            {
                try
                {
                    Pjvd = new PJVDetail();
                    Pjvd.DocumentID = pjvh.DocumentID;
                    Pjvd.TemporaryNo = pjvh.TemporaryNo;
                    Pjvd.TemporaryDate = pjvh.TemporaryDate;
                    Pjvd.AccountCode = grdPRDetail.Rows[i].Cells["AccountCode"].Value.ToString();
                    Pjvd.SLType = grdPRDetail.Rows[i].Cells["gSLType"].Value.ToString();
                    Pjvd.SLCode = grdPRDetail.Rows[i].Cells["PartyCode"].Value.ToString();
                    Pjvd.InvDocumentID = InvDociD;
                    Pjvd.InvTempNo = InvTemporaryNo;
                    Pjvd.InvTempDate = InvTemporaryDate;
                    if (grdPRDetail.Rows[i].Cells["AmountDebit"].Value != null)
                        Pjvd.AmountDebit = Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountDebit"].Value.ToString().Trim());
                    else
                        Pjvd.AmountDebit = 0;
                    if (grdPRDetail.Rows[i].Cells["AmountCredit"].Value != null)
                        Pjvd.AmountCredit = Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountCredit"].Value.ToString().Trim());
                    else
                        Pjvd.AmountCredit = 0;
                    PJVDetails.Add(Pjvd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("getVoucherDetails() : Error creating Purchase Journal Details");
                    //status = false;
                }
            }
            return PJVDetails;
        }
        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdPRDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkVoucherDetailGridRows())
                    {
                        return;
                    }
                }
                AddRowClick = true;
                AddPRDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                verifyAndReworkVoucherDetailGridRows();
            }
            catch (Exception ex)
            {
            }
        }

        private Boolean verifyAndReworkVoucherDetailGridRows()
        {
            Boolean status = true;

            try
            {
                if (grdPRDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Journal details");
                    return false;
                }
                if (!validateItems())
                {
                    MessageBox.Show("Validation failed");
                    return false;
                }
                int debitCount = 0, creditCount = 0;
                Decimal amntDebit = 0;
                Decimal amntCredit = 0;
                for (int i = 0; i < grdPRDetail.Rows.Count; i++)
                {
                    if ((grdPRDetail.Rows[i].Cells["AmountDebit"].Value == null) ||
                                (grdPRDetail.Rows[i].Cells["AmountDebit"].Value.ToString().Trim().Length == 0))
                    {
                        grdPRDetail.Rows[i].Cells["AmountCredit"].Value = Convert.ToDecimal(0);
                    }
                    if ((grdPRDetail.Rows[i].Cells["AmountCredit"].Value == null) ||
                                (grdPRDetail.Rows[i].Cells["AmountCredit"].Value.ToString().Trim().Length == 0))
                    {
                        grdPRDetail.Rows[i].Cells["AmountCredit"].Value = Convert.ToDecimal(0);
                    }
                    if (((Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountDebit"].Value) != 0) &&
                       (Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountCredit"].Value) != 0)) ||
                           ((Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountDebit"].Value) == 0) &&
                       (Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountCredit"].Value) == 0)))

                    {
                        MessageBox.Show("Enter either debit or credit value.Row:" + (i + 1));
                        return false;
                    }
                    grdPRDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if ((grdPRDetail.Rows[i].Cells["AccountCode"].Value == null) ||
                        (grdPRDetail.Rows[i].Cells["AccountCode"].Value.ToString().Trim().Length == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                    if ((Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountDebit"].Value) != 0))
                        debitCount++;
                    else
                        creditCount++;
                    amntDebit = amntDebit + Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountDebit"].Value);
                    amntCredit = amntCredit + Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountCredit"].Value);
                }
                if ((grdPRDetail.Rows.Count != 1) && (debitCount > 1 && creditCount > 1))
                {
                    MessageBox.Show("Check the debit and Credit columns in detail.");
                    return false;
                }
                txtTotalCreditAmnt.Text = amntCredit.ToString();
                txtTotalDebitAmnt.Text = amntDebit.ToString();
            }
            catch (Exception ex)
            {

                return false;
            }
            return status;
        }
        private Boolean validateItems()
        {
            Boolean status = true;
            try
            {
                for (int i = 0; i < grdPRDetail.Rows.Count - 1; i++)
                {
                    for (int j = i + 1; j < grdPRDetail.Rows.Count; j++)
                    {

                        if (grdPRDetail.Rows[i].Cells["AccountCode"].Value.ToString() == grdPRDetail.Rows[j].Cells["AccountCode"].Value.ToString())
                        {
                            MessageBox.Show("AccountCode code duplicated.");
                            return false;
                        }
                    }

                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        private void grdPRDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdPRDetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdPRDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkVoucherDetailGridRows();
                    }
                    if (columnName.Equals("SelAcDebit"))
                    {
                        showAccountCodeDataGridView();
                    }
                    if (columnName.Equals("SelParty"))
                    {
                        showPayeeCodeDataGridView();
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
                    grdPRDetail.CurrentRow.Cells["AccountCode"].Value = row.Cells["AccountCode"].Value.ToString();
                    grdPRDetail.CurrentRow.Cells["AccountName"].Value = row.Cells["AccountName"].Value.ToString();

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
                    grdPRDetail.CurrentRow.Cells["PartyCode"].Value = row.Cells["ID"].Value.ToString();
                    grdPRDetail.CurrentRow.Cells["PartyName"].Value = row.Cells["Name"].Value.ToString();
                    grdPRDetail.CurrentRow.Cells["gSLType"].Value = row.Cells["Type"].Value.ToString();

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
    }
}
