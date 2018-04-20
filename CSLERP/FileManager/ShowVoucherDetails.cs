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

    public partial class ShowVoucherDetails : Form
    {
        string docID;
        int voucherNo;
        DateTime voucherDate;
        public ShowVoucherDetails(string docid, int vno, DateTime vdate)
        {
            InitializeComponent();
            docID = docid;
            voucherNo = vno;
            voucherDate = vdate;
            initVariable();
        }
        private void initVariable()
        {
            try
            {
                OfficeDB.fillOfficeComboNew(cmbOfficeID);
                CurrencyDB.fillCurrencyComboNew(cmbCurrencyID);
                ProjectHeaderDB.fillprojectCombo(cmbProjectID);
                CatalogueValueDB.fillCatalogValueComboNew(cmbBankTransMode, "BankTransactionMode");
                CatalogueValueDB.fillCatalogValueComboNew(cmbBookType, "DayBookType");
                dtTempDate.Format = DateTimePickerFormat.Custom;
                dtTempDate.CustomFormat = "dd-MM-yyyy";
                dtVoucherDate.Format = DateTimePickerFormat.Custom;
                dtVoucherDate.CustomFormat = "dd-MM-yyyy";
                ShowAllDetails();
                btnClose.Focus();
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
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["AmountDebit"].Value = Convert.ToDecimal(0);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["AmountCredit"].Value = Convert.ToDecimal(0);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["ChequeNo"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["ChequeDate"].Value = DateTime.Parse("1900-01-01");
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["PartyCode"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["PartyName"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["SLType"].Value = "";
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
                if (docID == "CASHPAYMENTVOUCHER" || docID == "BANKPAYMENTVOUCHER")
                {
                    grdPRDetail.Columns["PartyCode"].Visible = false;
                    grdPRDetail.Columns["PartyName"].Visible = false;
                    grdPRDetail.Columns["SLType"].Visible = false;
                    grdPRDetail.Columns["ChequeNo"].Visible = true;
                    grdPRDetail.Columns["ChequeDate"].Visible = true;
                    paymentvoucher vh = new paymentvoucher();
                    vh.DocumentID = docID;
                    vh.VoucherNo = voucherNo;
                    vh.VoucherDate = voucherDate;
                    paymentvoucher pvh = PaymentVoucherDB.getVoucherHeaderForTrialBalance(vh);
                    txtTemporarryNo.Text = pvh.TemporaryNo.ToString();
                    dtTempDate.Value = pvh.TemporaryDate;
                    txtVoucherNo.Text = pvh.VoucherNo.ToString();
                    dtVoucherDate.Value = pvh.VoucherDate;

                    txtvoucherType.Text = pvh.VoucherType;
                    cmbBookType.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbBookType, pvh.BookType);
                    txtPayeeCode.Text = pvh.SLCode.ToString();
                    txtPayeeName.Text = pvh.SLName;
                    txtBillDetails.Text = pvh.BillDetails;
                    cmbBankTransMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbBankTransMode, pvh.BankTransactionMode);
                    cmbOfficeID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbOfficeID, pvh.OfficeID);
                    cmbProjectID.SelectedIndex = cmbProjectID.FindString(pvh.ProjectID);
                    cmbCurrencyID.SelectedIndex =
                       Structures.ComboFUnctions.getComboIndex(cmbCurrencyID, pvh.CurrencyID);

                    txtExchangeRate.Text = pvh.ExchangeRate.ToString();
                    txtVoucherAmount.Text = pvh.VoucherAmount.ToString();
                    
                    txtvoucherAmountINR.Text = pvh.VoucherAmountINR.ToString();
                    txtAmountInWords.Text = NumberToString.convert(pvh.VoucherAmount.ToString()).Trim().Replace("INR", pvh.CurrencyID);
                    txtnarration.Text = pvh.Narration.ToString();
                    List<paymentvoucherdetail> VDetail = PaymentVoucherDB.getVoucherDetail(pvh);
                    grdPRDetail.Rows.Clear();
                    int i = 0;
                    decimal totCredit = 0;
                    decimal totDebit = 0;
                    foreach (paymentvoucherdetail vd in VDetail)
                    {
                        AddPRDetailRow();
                        grdPRDetail.Rows[i].Cells["AccountCode"].Value = vd.AccountCode;
                        grdPRDetail.Rows[i].Cells["AccountName"].Value = vd.AccountName;
                        grdPRDetail.Rows[i].Cells["AmountDebit"].Value = vd.AmountDebit;
                        totDebit = totDebit + vd.AmountDebit;
                        grdPRDetail.Rows[i].Cells["AmountCredit"].Value = vd.AmountCredit;
                        totCredit = totCredit + vd.AmountCredit;
                        grdPRDetail.Rows[i].Cells["ChequeNo"].Value = vd.ChequeNo;
                        grdPRDetail.Rows[i].Cells["ChequeDate"].Value = vd.ChequeDate;
                        i++;
                    }
                    txtTotalCreditAmnt.Text = totCredit.ToString();
                    txtTotalDebitAmnt.Text = totDebit.ToString();
                }
                else if (docID == "BANKRECEIPTVOUCHER" || docID == "CASHRECEIPTVOUCHER")
                {
                    grdPRDetail.Columns["PartyCode"].Visible = false;
                    grdPRDetail.Columns["PartyName"].Visible = false;
                    grdPRDetail.Columns["SLType"].Visible = false;
                    grdPRDetail.Columns["ChequeNo"].Visible = true;
                    grdPRDetail.Columns["ChequeDate"].Visible = true;
                    ReceiptVoucherHeader vh = new ReceiptVoucherHeader();
                    vh.DocumentID = docID;
                    vh.VoucherNo = voucherNo;
                    vh.VoucherDate = voucherDate;
                    ReceiptVoucherHeader rvh = ReceiptVoucherDB.getReceiptVoucherHeaderForTrailbalance(vh);
                    txtTemporarryNo.Text = rvh.TemporaryNo.ToString();
                    dtTempDate.Value = rvh.TemporaryDate;
                    txtVoucherNo.Text = rvh.VoucherNo.ToString();
                    dtVoucherDate.Value = rvh.VoucherDate;
                    txtvoucherType.Text = rvh.VoucherType;
                    cmbBookType.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbBookType, rvh.BookType);
                    txtPayeeCode.Text = rvh.SLCode.ToString();
                    txtPayeeName.Text = rvh.SLName;
                    txtBillDetails.Text = rvh.BillDetails;
                    cmbBankTransMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbBankTransMode, rvh.BankTransactionMode);
                    cmbOfficeID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbOfficeID, rvh.OfficeID);
                    cmbProjectID.SelectedIndex = cmbProjectID.FindString(rvh.ProjectID);
                    cmbCurrencyID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCurrencyID, rvh.CurrencyID);
                    txtExchangeRate.Text = rvh.ExchangeRate.ToString();
                    txtVoucherAmount.Text = rvh.VoucherAmount.ToString();
                   
                    txtvoucherAmountINR.Text = rvh.VoucherAmountINR.ToString();
                    txtAmountInWords.Text = NumberToString.convert(rvh.VoucherAmount.ToString()).Trim().Replace("INR", rvh.CurrencyID);
                    txtnarration.Text = rvh.Narration.ToString();
                    List<ReceiptVoucherDetail> VDetail = ReceiptVoucherDB.getVoucherDetail(rvh);
                    grdPRDetail.Rows.Clear();
                    int i = 0;
                    decimal totCredit = 0;
                    decimal totDebit = 0;
                    foreach (ReceiptVoucherDetail vd in VDetail)
                    {
                        AddPRDetailRow();
                        grdPRDetail.Rows[i].Cells["AccountCode"].Value = vd.AccountCode;
                        grdPRDetail.Rows[i].Cells["AccountName"].Value = vd.AccountName;
                        grdPRDetail.Rows[i].Cells["AmountDebit"].Value = vd.AmountDebit;
                        totDebit = totDebit + vd.AmountDebit;
                        grdPRDetail.Rows[i].Cells["AmountCredit"].Value = vd.AmountCredit;
                        totCredit = totCredit + vd.AmountCredit;
                        grdPRDetail.Rows[i].Cells["ChequeNo"].Value = vd.ChequeNo;
                        grdPRDetail.Rows[i].Cells["ChequeDate"].Value = vd.ChequeDate;
                        i++;
                    }
                    txtTotalCreditAmnt.Text = totCredit.ToString();
                    txtTotalDebitAmnt.Text = totDebit.ToString();
                }
                else if (docID == "JOURNALVOUCHER" || docID == "PJV" || docID == "SJV" )
                {
                    grdPRDetail.Columns["PartyCode"].Visible = true;
                    grdPRDetail.Columns["PartyName"].Visible = true;
                    grdPRDetail.Columns["SLType"].Visible = true;
                    grdPRDetail.Columns["ChequeNo"].Visible = false;
                    grdPRDetail.Columns["ChequeDate"].Visible = false;
                    JournalVoucherHeader jvhTemp = new JournalVoucherHeader();
                    jvhTemp.DocumentID = docID;
                    jvhTemp.JournalNo = voucherNo;
                    jvhTemp.JournalDate = voucherDate;
                    JournalVoucherHeader jvh = JournalVoucherDB.getJournalHeaderForTrialBalance(jvhTemp);
                    txtTemporarryNo.Text = jvh.TemporaryNo.ToString();
                    //txtvoucherType.Text = "Journal";
                    dtTempDate.Value = jvh.TemporaryDate;
                    //txtAmountInWords.Text = NumberToString.convert(txtTotalDebitAmnt.Text);
                    txtVoucherNo.Text = jvh.JournalNo.ToString();
                    dtVoucherDate.Value = jvh.JournalDate;
                    txtnarration.Text = jvh.Narration.ToString();
                    decimal totDebit = 0;
                    decimal totCredit = 0;
                    List<JournalVoucherDetail> JVDetail = JournalVoucherDB.getJournalVoucherDetail(jvh);
                    grdPRDetail.Rows.Clear();
                    int i = 0;
                    foreach (JournalVoucherDetail jvd in JVDetail)
                    {
                        AddPRDetailRow();
                        grdPRDetail.Rows[i].Cells["AccountCode"].Value = jvd.AccountCode;
                        grdPRDetail.Rows[i].Cells["AccountName"].Value = jvd.AccountName;
                        grdPRDetail.Rows[i].Cells["PartyCode"].Value = jvd.SLCode;
                        grdPRDetail.Rows[i].Cells["PartyName"].Value = jvd.SLName;
                        grdPRDetail.Rows[i].Cells["SLType"].Value = jvd.SLType;
                        grdPRDetail.Rows[i].Cells["AmountDebit"].Value = jvd.AmountDebit;
                        grdPRDetail.Rows[i].Cells["AmountCredit"].Value = jvd.AmountCredit;
                        totDebit = totDebit + jvd.AmountDebit;
                        totCredit = totCredit + jvd.AmountCredit;
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
    }
    public class voucherheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public int CreationMode { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public string VoucherType { get; set; }
        public string BankTransactionMode { get; set; }
        public string CurrencyID { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal VoucherAmountINR { get; set; }
        public string BookType { get; set; }
        public decimal VoucherAmount { get; set; }
        public string AccountCodeCredit { get; set; }
        public string AccountCodeDebit { get; set; }
        public string AccountNameCredit { get; set; }
        public string AccountNameDebit { get; set; }
        public decimal AmountDebit { get; set; }
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public string Narration { get; set; }
        public string BillDetails { get; set; }
        public string ProjectID { get; set; }
        public string OfficeID { get; set; }
        public string OfficeName { get; set; }
        public string SLType { get; set; }
        public string SLCode { get; set; }
        public string SLName { get; set; }
    }
    public class voucherdetail
    {
        public string DocumentID { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal AmountDebit { get; set; }
        public decimal AmountDebitINR { get; set; }
        public decimal AmountCreditINR { get; set; }
        public decimal AmountCredit { get; set; }
        public DateTime BankDate { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
    }
}
