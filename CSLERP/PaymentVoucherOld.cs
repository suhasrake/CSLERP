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
    public partial class PaymentVoucherOld : System.Windows.Forms.Form
    {
        string docID = "PAYMENTVOUCHER";
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        string userCommentStatusString = "";
        Form dtpForm = new Form();
        string commentStatus = "";
        Boolean AddRowClick = false;
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Boolean userIsACommenter = false;
        string Type = "";
        string SLType = "";
        Timer filterTimer = new Timer();
        double productvalue = 0.0;
        double taxvalue = 0.0;
        Boolean newClick = false;
        Boolean IsEditMode = false;
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        paymentvoucher prevvh;
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
        int ch = 0;
        int track = 0;
        Form frmPopup = new Form();
        DataGridView payeeCodeGrd = new DataGridView();
        DataGridView AccCodeGrd = new DataGridView();
        public PaymentVoucherOld()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }
        private void PaymentVoucher_Load(object sender, EventArgs e)
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
            ListFilteredVoucherHeader(listOption);
            //tabControl1.SelectedIndexChanged += new EventHandler(TabControl1_SelectedIndexChanged);
        }
        private void ListFilteredVoucherHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                PaymentVoucherDB vhDB = new PaymentVoucherDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);

                List<paymentvoucher> VHList = vhDB.getFilteredPaymentVoucherHeader(userString, option, userCommentStatusString);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (paymentvoucher vh in VHList)
                {
                    if (option == 1)
                    {
                        if (vh.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = vh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = vh.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = vh.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["VoucherNo"].Value = vh.VoucherNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["VoucherDate"].Value = vh.VoucherDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["VoucherType"].Value = vh.VoucherType;
                    grdList.Rows[grdList.RowCount - 1].Cells["VoucherAmount"].Value = vh.VoucherAmount;
                    grdList.Rows[grdList.RowCount - 1].Cells["VoucherAmountINR"].Value = vh.VoucherAmountINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["AccountCodeCredit"].Value = vh.AccountCodeCredit;
                    grdList.Rows[grdList.RowCount - 1].Cells["AccountNameCredit"].Value = vh.AccountNameCredit;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreationMode"].Value = vh.CreationMode;
                    grdList.Rows[grdList.RowCount - 1].Cells["gBookType"].Value = vh.BookType;
                    grdList.Rows[grdList.RowCount - 1].Cells["gSLType"].Value = vh.SLType;
                    grdList.Rows[grdList.RowCount - 1].Cells["gSLCode"].Value = vh.SLCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["Narration"].Value = vh.Narration;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProjectID"].Value = vh.ProjectID;
                    grdList.Rows[grdList.RowCount - 1].Cells["OfficeID"].Value = vh.OfficeID;
                    grdList.Rows[grdList.RowCount - 1].Cells["BankTransactionMode"].Value = vh.BankTransactionMode;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyID"].Value = vh.CurrencyID;
                    grdList.Rows[grdList.RowCount - 1].Cells["ExchangeRate"].Value = vh.ExchangeRate;
                    grdList.Rows[grdList.RowCount - 1].Cells["SLName"].Value = vh.SLName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = vh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["FrowardUser"].Value = vh.ForwardUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ApproveUser"].Value = vh.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = vh.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = vh.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = vh.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = vh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = vh.status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = vh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = vh.CreateUser;
                    //grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatusNo"].Value = prh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CmtStatus"].Value = vh.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = vh.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = vh.ForwarderList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Payment Voucher Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;

            grdList.Columns["Creator"].Visible = true;
            grdList.Columns["Forwarder"].Visible = true;
            grdList.Columns["Approver"].Visible = true;

        }

        //called only in the beginning
        private void initVariables()
        {

            if (getuserPrivilegeStatus() == 1)
            {
                //user is only a viewer
                listOption = 6;
            }
            docID = Main.currentDocument;
            OfficeDB.fillOfficeComboNew(cmbOfficeID);
            CurrencyDB.fillCurrencyComboNew(cmbCurrencyID);
            ProjectHeaderDB.fillprojectCombo(cmbProjectID);
            CatalogueValueDB.fillCatalogValueComboNew(cmbBankTransMode, "BankTransactionMode");
            CatalogueValueDB.fillCatalogValueComboNew(cmbBookType, "DayBookType");
            try
            {
                //////////cmbCurrencyID.SelectedIndex = cmbCurrencyID.FindString("INR");
                cmbCurrencyID.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCurrencyID, "INR");
            }
            catch
            {
                cmbCurrencyID.SelectedIndex = -1;
            }
            dtTempDate.Format = DateTimePickerFormat.Custom;
            dtTempDate.CustomFormat = "dd-MM-yyyy";
            dtVoucherDate.Format = DateTimePickerFormat.Custom;
            dtVoucherDate.CustomFormat = "dd-MM-yyyy";
            btnForward.Visible = false;
            btnApprove.Visible = false;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            grdPRDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //---
            //create tax details table for tax breakup display
            {
                TaxDetailsTable.Columns.Add("TaxItem", typeof(string));
                TaxDetailsTable.Columns.Add("TaxAmount", typeof(double));
            }
            txtComments.Text = "";

            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
            setTabIndex();
        }
        private void setTabIndex()
        {
            txtTemporarryNo.TabIndex = 0;
            dtTempDate.TabIndex = 1;
            txtvoucherType.TabIndex = 2;
            txtVoucherNo.TabIndex = 3;
            dtVoucherDate.TabIndex = 4;
            cmbBookType.TabIndex = 5;
            cmbCreditACCode.TabIndex = 6;
            cmbSLType.TabIndex = 7;
            txtPayeeCode.TabIndex = 8;
            btnSelect.TabIndex = 9;
            cmbBankTransMode.TabIndex = 10;
            cmbOfficeID.TabIndex = 11;
            cmbProjectID.TabIndex = 12;
            cmbCurrencyID.TabIndex = 13;
            txtExchangeRate.TabIndex = 14;
            txtVoucherAmount.TabIndex = 15;
            txtvoucherAmountINR.TabIndex = 16;

            btnAddLine.TabIndex = 17;
            btnCalculate.TabIndex = 18;
            btnClearEntries.TabIndex = 19;

            txtnarration.TabIndex = 20;
            txtAmountInWords.TabIndex = 21;

            btnForward.TabIndex = 22;
            btnApprove.TabIndex = 23;
            btnCancel.TabIndex = 24;
            btnSave.TabIndex = 25;
            btnReverse.TabIndex = 26;
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

        //called when new,cancel buttons are clicked.
        //called at the end of event processing for forward, approve,reverse and save
        public void clearData()
        {
            try
            {
                //clear all grid views
                grdPRDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                //----------clear temperory panels
                newClick = false;
                //isedit = false;
                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                pnllv.Visible = false;
                //grdPRDetail.Columns["ChequeNo"].Visible = true;
                //grdPRDetail.Columns["ChequeDate"].Visible = true;
                //----------
                cmbBankTransMode.Enabled = true;
                cmbProjectID.SelectedIndex = -1;
                cmbOfficeID.SelectedIndex = -1;
                cmbSLType.SelectedIndex = -1;
                ////cmbCurrencyID.SelectedIndex = -1;
                cmbCreditACCode.Items.Clear();
                cmbBankTransMode.SelectedIndex = -1;
                cmbCreditACCode.SelectedIndex = -1;
                cmbBookType.SelectedIndex = -1;
                txtAmountInWords.Text = "";
                txtTemporarryNo.Text = "";
                txtPayeeCode.Text = "";
                txtVoucherNo.Text = "";
                txtVoucherAmount.Text = "";
                txtvoucherAmountINR.Text = "";
                txtExchangeRate.Text = "";
                commentStatus = "";
                dtTempDate.Value = DateTime.Parse("01-01-1900");
                dtVoucherDate.Value = DateTime.Parse("01-01-1900");
                txtExchangeRate.Text = "1";
                cmbBookType.Enabled = true;
                txtnarration.Text = "";
                AddRowClick = false;
                prevvh = new paymentvoucher();
                removeControlsFromCommenterPanel();
                removeControlsFromForwarderPanel();
                removeControlsFromLVPanel();
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
                IsEditMode = false;
                clearData();
                newClick = true;
                btnSave.Text = "Save";
                pnlAddEdit.Visible = true;
                closeAllPanels();
                AddRowClick = false;
                pnlAddEdit.Visible = true;
                setButtonVisibility("btnNew");
            }
            catch (Exception)
            {

            }
        }
        private Boolean verifyVoucherHeaderItems()
        {
            Boolean status = true;
            if (cmbBookType.SelectedIndex == -1 || txtPayeeCode.Text.Length == 0 || SLType.Length == 0)
                status = false;
            return status;
        }
        private void btnAddLine_Click_2(object sender, EventArgs e)
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
                if (verifyVoucherHeaderItems())
                {
                    AddRowClick = true;
                    AddPRDetailRow();
                }
                else
                    MessageBox.Show("Correct Voucher header before adding lines");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
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
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["AccountCodeDebit"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["AmountDebit"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["BillNo"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["BillDate"].Value = DateTime.Today;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["ChequeNo"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["ChequeDate"].Value = DateTime.Today;
                if (AddRowClick)
                {
                    grdPRDetail.FirstDisplayedScrollingRowIndex = grdPRDetail.RowCount - 1;
                    grdPRDetail.CurrentCell = grdPRDetail.Rows[kount - 1].Cells[0];
                }
                grdPRDetail.Columns[0].Frozen = false;
                grdPRDetail.FirstDisplayedScrollingColumnIndex = 0;
                grdPRDetail.Columns["AmountDebit"].Frozen = true;
            }

            catch (Exception ex)
            {
                MessageBox.Show("AddPRDetailRow() : Error");
            }

            return status;
        }
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            verifyAndReworkVoucherDetailGridRows();
        }
        private Boolean verifyAndReworkVoucherDetailGridRows()
        {
            Boolean status = true;

            try
            {
                if (grdPRDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Voucher details");
                    txtExchangeRate.Text = "1";
                    txtVoucherAmount.Text = taxvalue.ToString(); //fill this later
                    txtvoucherAmountINR.Text = (productvalue + taxvalue).ToString();
                    return false;
                }
                // ---------------commented on 9-3-2017
                // duplicate account codes allowed to manage different bill nos
                ////if (!validateItems())
                ////{
                ////    MessageBox.Show("Validation failed");
                ////    return false;
                ////}
                //----------------
                double vamnt = 0;
                for (int i = 0; i < grdPRDetail.Rows.Count; i++)
                {

                    grdPRDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if ((grdPRDetail.Rows[i].Cells["AccountCodeDebit"].Value.ToString().Trim().Length == 0) ||
                        (grdPRDetail.Rows[i].Cells["AmountDebit"].Value.ToString().Trim().Length == 0) ||
                        (grdPRDetail.Rows[i].Cells["BillNo"].Value.ToString().Trim().Length == 0) ||
                        (grdPRDetail.Rows[i].Cells["BillDate"].Value.ToString().Trim().Length == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                    if (Convert.ToDateTime(grdPRDetail.Rows[i].Cells["BillDate"].Value).Date > DateTime.Now.Date
                        || (DateTime.Today.Date - Convert.ToDateTime(grdPRDetail.Rows[i].Cells["BillDate"].Value).Date).Days > 365)
                    {
                        MessageBox.Show("Check bill datein Row: " + (i + 1));
                        return false;
                    }
                    else if (Convert.ToDateTime(grdPRDetail.Rows[i].Cells["ChequeDate"].Value).Date > DateTime.Now.Date
                        || (DateTime.Today.Date - Convert.ToDateTime(grdPRDetail.Rows[i].Cells["ChequeDate"].Value).Date).Days > 365)
                    {
                        MessageBox.Show("Check Cheque Date IN Row: " + (i + 1));
                        return false;
                    }
                    vamnt = vamnt + Convert.ToDouble(grdPRDetail.Rows[i].Cells["AmountDebit"].Value);
                }
                txtVoucherAmount.Text = vamnt.ToString();
                txtvoucherAmountINR.Text = ((Convert.ToDouble(txtExchangeRate.Text)) * vamnt).ToString();
                txtAmountInWords.Text = NumberToString.convert(txtvoucherAmountINR.Text);
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

                        if (grdPRDetail.Rows[i].Cells["AccountCodeDebit"].Value.ToString() == grdPRDetail.Rows[j].Cells["AccountCodeDebit"].Value.ToString())
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
        private List<paymentvoucherdetail> getVoucherDetails(paymentvoucher vh)
        {
            paymentvoucherdetail vd = new paymentvoucherdetail();

            List<paymentvoucherdetail> VDetails = new List<paymentvoucherdetail>();
            for (int i = 0; i < grdPRDetail.Rows.Count; i++)
            {
                try
                {
                    vd = new paymentvoucherdetail();
                    vd.DocumentID = vh.DocumentID;
                    vd.TemporaryNo = vh.TemporaryNo;
                    vd.TemporaryDate = vh.TemporaryDate;
                    vd.AccountCodeDebit = grdPRDetail.Rows[i].Cells["AccountCodeDebit"].Value.ToString();/*.Substring(0, grdPRDetail.Rows[i].Cells["AccountCodeDebit"].Value.ToString().IndexOf('-'))*/;
                    vd.AmountDebit = Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountDebit"].Value.ToString().Trim());
                    vd.BillNo = grdPRDetail.Rows[i].Cells["BillNo"].Value.ToString();
                    vd.BillDate = Convert.ToDateTime(grdPRDetail.Rows[i].Cells["BillDate"].Value);
                    vd.ChequeNo = grdPRDetail.Rows[i].Cells["ChequeNo"].Value.ToString();
                    vd.ChequeDate = Convert.ToDateTime(grdPRDetail.Rows[i].Cells["ChequeDate"].Value);
                    VDetails.Add(vd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("createAndUpdatePRDetails() : Error creating PR Details");
                    //status = false;
                }
            }
            return VDetails;
        }
        //private Boolean createAndUpdateVoucherDetails(paymentvoucher vh)
        //{
        //    Boolean status = true;
        //    try
        //    {
        //        PaymentVoucherDB vhDB = new PaymentVoucherDB();

        //        if (!vhDB.updateVoucherDetail(VDetails, vh))
        //        {
        //            MessageBox.Show("createAndUpdatePRDetails() : Failed to update Voucher Details. Please check the values");
        //            status = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("createAndUpdatePRDetails() : Error updating Voucher Details");
        //        status = false;
        //    }
        //    return status;
        //}

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredVoucherHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredVoucherHeader(listOption);
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

            ListFilteredVoucherHeader(listOption);
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {

                PaymentVoucherDB vhDB = new PaymentVoucherDB();
                paymentvoucher vh = new paymentvoucher();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;
                if (!verifyAndReworkVoucherDetailGridRows())
                {
                    MessageBox.Show("Validation Failed in voucher Detail");
                    return;
                }
                vh.DocumentID = docID;
                vh.VoucherDate = dtVoucherDate.Value;
                vh.VoucherType = txtvoucherType.Text;
                vh.BookType = ((Structures.ComboBoxItem)cmbBookType.SelectedItem).HiddenValue;
                // vh.BookType = cmbBookType.SelectedItem.ToString().Substring(0, cmbBookType.SelectedItem.ToString().IndexOf('-'));
                string tstr = cmbCreditACCode.SelectedItem.ToString();
                vh.AccountCodeCredit = ((Structures.ComboBoxItem)cmbCreditACCode.SelectedItem).HiddenValue;
                vh.SLType = SLType;
                vh.SLCode = txtPayeeCode.Text;
                try
                {
                    vh.BankTransactionMode = ((Structures.ComboBoxItem)cmbBankTransMode.SelectedItem).HiddenValue;
                    //vh.BankTransactionMode = cmbBankTransMode.SelectedItem.ToString().Substring(0, cmbBankTransMode.SelectedItem.ToString().IndexOf('-'));
                }
                catch (Exception)
                {
                }
                try
                {
                    vh.OfficeID = ((Structures.ComboBoxItem)cmbOfficeID.SelectedItem).HiddenValue;
                }
                catch (Exception)
                {

                }
                try
                {
                    vh.ProjectID = cmbProjectID.SelectedItem.ToString();
                }
                catch (Exception)
                {
                }
                try
                {
                    //////////vh.CurrencyID = cmbCurrencyID.SelectedItem.ToString().Substring(0, cmbCurrencyID.SelectedItem.ToString().IndexOf('-'));
                    vh.CurrencyID = ((Structures.ComboBoxItem)cmbCurrencyID.SelectedItem).HiddenValue;
                }
                catch (Exception)
                {
                }
                vh.Narration = txtnarration.Text.Replace("'", "''");
                if (txtVoucherAmount.Text.Length != 0)
                    vh.VoucherAmount = Convert.ToDecimal(txtVoucherAmount.Text);
                if (txtExchangeRate.Text.Length != 0)
                    vh.ExchangeRate = Convert.ToDecimal(txtExchangeRate.Text);
                if (txtvoucherAmountINR.Text.Length != 0)
                    vh.VoucherAmountINR = Convert.ToDecimal(txtvoucherAmountINR.Text);
                vh.Comments = docCmtrDB.DGVtoString(dgvComments);
                vh.ForwarderList = prevvh.ForwarderList;

                if (!vhDB.validatePaymentVoucherHeader(vh))
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    //vh.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    vh.DocumentStatus = 1; //created
                    vh.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    vh.TemporaryNo = Convert.ToInt32(txtTemporarryNo.Text);
                    vh.TemporaryDate = prevvh.TemporaryDate;
                    vh.DocumentStatus = prevvh.DocumentStatus;
                    vh.CreationMode = prevvh.CreationMode;
                }

                if (vhDB.validatePaymentVoucherHeader(vh))
                {
                    //--create comment status string
                    docCmtrDB = new DocCommenterDB();
                    if (userIsACommenter)
                    {
                        //if the user is only a commenter and ticked the comment as final; then update his comment string as final
                        //and update the comment status
                        if (chkCommentStatus.Checked)
                        {
                            docCmtrDB = new DocCommenterDB();
                            vh.CommentStatus = docCmtrDB.createCommentStatusString(prevvh.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            vh.CommentStatus = prevvh.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            vh.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            vh.CommentStatus = prevvh.CommentStatus;
                        }
                    }
                    //----------
                    int tmpStatus = 0;
                    if (chkCommentStatus.Checked)
                    {
                        tmpStatus = 1;
                    }
                    if (txtComments.Text.Trim().Length > 0)
                    {
                        vh.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus);
                    }
                    List<paymentvoucherdetail> VDetails = getVoucherDetails(vh);
                    if(VDetails.Count == 0)
                    {
                        MessageBox.Show("Voucher detail is empty");
                        return;
                    }
                    if (btnText.Equals("Update"))
                    {
                        if (vhDB.updatePVHeaderAndDetail(vh, prevvh, VDetails,null))
                        {
                            MessageBox.Show("Payment Voucher Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredVoucherHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update Payment voucher");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        //return;
                        if (vhDB.InsertPVHeaderAndDetail(vh, VDetails,null))
                        {
                            MessageBox.Show("Payment Voucher Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredVoucherHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert Payment Voucher");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("PaymentVouucher Validation failed");
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("btnSave_Click_1() : Error");
                status = false;
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            SLType = "";
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }

        private void btnAddLine_Click_1(object sender, EventArgs e)
        {
            if (grdPRDetail.Rows.Count > 0)
            {
                if (!verifyAndReworkVoucherDetailGridRows())
                {
                    return;
                }
            }
            AddPRDetailRow();
        }
        private void grdPOPIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void btnCalculateax_Click_1(object sender, EventArgs e)
        {
            verifyAndReworkVoucherDetailGridRows();
        }

        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("LoadDocument") ||
                    columnName.Equals("View") || columnName.Equals("Print"))
                {

                    clearData();
                    cmbBookType.Enabled = false;
                    setButtonVisibility(columnName);
                    AddRowClick = false;
                    IsEditMode = true;
                    ch = 0;
                    track = 0;
                    prevvh = new paymentvoucher();
                    prevvh.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();
                    prevvh.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    prevvh.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    prevvh.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    if (prevvh.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    prevvh.Comments = PaymentVoucherDB.getUserComments(prevvh.DocumentID, prevvh.TemporaryNo, prevvh.TemporaryDate);
                    docID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();

                    PaymentVoucherDB prdb = new PaymentVoucherDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];

                    prevvh.VoucherNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["VoucherNo"].Value.ToString());
                    prevvh.VoucherDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["VoucherDate"].Value.ToString());
                    prevvh.VoucherType = grdList.Rows[e.RowIndex].Cells["VoucherType"].Value.ToString();
                    prevvh.VoucherAmount = Convert.ToDecimal(grdList.Rows[e.RowIndex].Cells["VoucherAmount"].Value.ToString());
                    prevvh.VoucherAmountINR = Convert.ToDecimal(grdList.Rows[e.RowIndex].Cells["VoucherAmountINR"].Value.ToString());
                    prevvh.AccountCodeCredit = grdList.Rows[e.RowIndex].Cells["AccountCodeCredit"].Value.ToString();
                    prevvh.AccountNameCredit = grdList.Rows[e.RowIndex].Cells["AccountNameCredit"].Value.ToString();
                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Document Temp No:" + prevvh.TemporaryNo + "\n" +
                            "Document Temp Date:" + prevvh.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "PR No:" + prevvh.VoucherNo + "\n" +
                            "PR Date:" + prevvh.VoucherDate.ToString("dd-MM-yyyy");
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevvh.TemporaryNo + "-" + prevvh.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_1(null, null);
                        return;
                    }
                    //--------
                    //prevvh.AmountDebit = Convert.ToDecimal(grdList.Rows[e.RowIndex].Cells["AmntDebit"].Value.ToString());
                    prevvh.BookType = grdList.Rows[e.RowIndex].Cells["gBookType"].Value.ToString();
                    prevvh.CreationMode = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["CreationMode"].Value.ToString());
                    prevvh.SLName = grdList.Rows[e.RowIndex].Cells["SLName"].Value.ToString();
                    prevvh.SLCode = grdList.Rows[e.RowIndex].Cells["gSLCode"].Value.ToString();
                    SLType = grdList.Rows[e.RowIndex].Cells["gSLType"].Value.ToString();
                    prevvh.SLType = grdList.Rows[e.RowIndex].Cells["gSLType"].Value.ToString();
                    prevvh.ProjectID = grdList.Rows[e.RowIndex].Cells["ProjectID"].Value.ToString();
                    prevvh.OfficeID = grdList.Rows[e.RowIndex].Cells["OfficeID"].Value.ToString();
                    prevvh.CurrencyID = grdList.Rows[e.RowIndex].Cells["gCurrencyID"].Value.ToString();
                    prevvh.BankTransactionMode = grdList.Rows[e.RowIndex].Cells["BankTransactionMode"].Value.ToString();
                    prevvh.ExchangeRate = Convert.ToDecimal(grdList.Rows[e.RowIndex].Cells["ExchangeRate"].Value.ToString());
                    prevvh.Narration = grdList.Rows[e.RowIndex].Cells["Narration"].Value.ToString();
                    prevvh.status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    prevvh.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    prevvh.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    prevvh.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    prevvh.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    prevvh.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevvh.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    prevvh.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    if (columnName.Equals("Print"))
                    {
                        //PrintPurchaseOrder ppo = new PrintPurchaseOrder();
                        pnlAddEdit.Visible = false;
                        pnlList.Visible = true;
                        grdList.Visible = true;
                        btnNew.Visible = true;
                        btnExit.Visible = true;
                        //CSLERP.PrintForms.PrintPurchaseOrder ppo = new CSLERP.PrintForms.PrintPurchaseOrder();
                        PrintPaymentVoucher ppv = new PrintPaymentVoucher();
                        List<paymentvoucherdetail> PVDetails = PaymentVoucherDB.getVoucherDetail(prevvh);
                        string fileName = ppv.PrintVoucher(prevvh, PVDetails);

                        System.Diagnostics.Process.Start(fileName);
                        btnNew.Visible = true;
                        btnExit.Visible = true;
                        return;
                    }
                    //--comments
                    chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    prevvh.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(prevvh.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(prevvh.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---
                    txtTemporarryNo.Text = prevvh.TemporaryNo.ToString();
                    dtTempDate.Value = prevvh.TemporaryDate;
                    txtVoucherNo.Text = prevvh.VoucherNo.ToString();
                    try
                    {
                        dtVoucherDate.Value = prevvh.VoucherDate;
                    }
                    catch (Exception)
                    {
                        dtVoucherDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtExchangeRate.Text = prevvh.ExchangeRate.ToString();
                    txtVoucherAmount.Text = prevvh.VoucherAmount.ToString();
                    txtAmountInWords.Text = NumberToString.convert(prevvh.VoucherAmount.ToString());
                    txtvoucherAmountINR.Text = prevvh.VoucherAmountINR.ToString();
                    cmbProjectID.SelectedIndex = cmbProjectID.FindString(prevvh.ProjectID);
                    ////////cmbCurrencyID.SelectedIndex = cmbCurrencyID.FindString(prevvh.CurrencyID);
                    cmbCurrencyID.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCurrencyID, prevvh.CurrencyID);
                    cmbOfficeID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbOfficeID, prevvh.OfficeID);
                    cmbBankTransMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbBankTransMode, prevvh.BankTransactionMode);
                    //cmbBankTransMode.SelectedIndex = cmbBankTransMode.FindString(prevvh.BankTransactionMode);
                    cmbSLType.SelectedIndex = cmbSLType.FindString(prevvh.SLType);
                    cmbBookType.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbBookType, prevvh.BookType);

                    //cmbBookType.SelectedIndex = cmbBookType.FindString(prevvh.BookType);
                    cmbCreditACCode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCreditACCode, prevvh.AccountCodeCredit);
                    txtnarration.Text = prevvh.Narration.ToString();
                    txtvoucherType.Text = prevvh.VoucherType.ToString();
                    txtPayeeCode.Text = prevvh.SLCode.ToString() ;
                    txtPayeeName.Text = prevvh.SLName;
                    txtnarration.Text = prevvh.Narration.ToString();
                    //isedit = false;
                    List<paymentvoucherdetail> VDetail = PaymentVoucherDB.getVoucherDetail(prevvh);
                    grdPRDetail.Rows.Clear();
                    int i = 0;
                    try
                    {
                        foreach (paymentvoucherdetail vd in VDetail)
                        {
                            //AddRowClick = true;
                            AddPRDetailRow();
                            grdPRDetail.Rows[i].Cells["AccountCodeDebit"].Value = vd.AccountCodeDebit;
                            grdPRDetail.Rows[i].Cells["AccountNameDebit"].Value = vd.AccountNameDebit;
                            grdPRDetail.Rows[i].Cells["AmountDebit"].Value = vd.AmountDebit;
                            grdPRDetail.Rows[i].Cells["BillNo"].Value = vd.BillNo;
                            grdPRDetail.Rows[i].Cells["BillDate"].Value = vd.BillDate;
                            grdPRDetail.Rows[i].Cells["ChequeNo"].Value = vd.ChequeNo;
                            grdPRDetail.Rows[i].Cells["ChequeDate"].Value = vd.ChequeDate;
                            i++;
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                }
                else
                {
                    return;
                }
                btnSave.Text = "Update";
                pnlList.Visible = false;
                pnlAddEdit.Visible = true;
                tabControl1.SelectedTab = tabVHeader;
                tabControl1.Visible = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("error");
            }
        }
        private void btnClearEntries_Click_1(object sender, EventArgs e)
        {
            try
            {
                for (int i = grdPRDetail.Rows.Count - 1; i >= 0; i--)
                {
                    grdPRDetail.Rows.RemoveAt(i);
                }
            }
            catch (Exception)
            {
            }
            MessageBox.Show("item cleared.");
        }

        private void btnForward_Click_1(object sender, EventArgs e)
        {
            //removeControlsFromForwarderPanel();
            //lvApprover = new ListView();
            //lvApprover.Clear();
            //pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
            //pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            lvApprover = DocEmpMappingDB.ApproverLV(docID, Login.empLoggedIn);
            lvApprover.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            //pnlForwarder.Controls.Remove(lvApprover);
            frmPopup.Controls.Add(lvApprover);

            Button lvForwrdOK = new Button();
            lvForwrdOK.BackColor = Color.Tan;
            lvForwrdOK.Text = "OK";
            lvForwrdOK.Location = new Point(40, 265);
            lvForwrdOK.Click += new System.EventHandler(this.lvForwardOK_Click);
            frmPopup.Controls.Add(lvForwrdOK);

            Button lvForwardCancel = new Button();
            lvForwardCancel.BackColor = Color.Tan;
            lvForwardCancel.Text = "CANCEL";
            lvForwardCancel.Location = new Point(130, 265);
            lvForwardCancel.Click += new System.EventHandler(this.lvForwardCancel_Click);
            frmPopup.Controls.Add(lvForwardCancel);
            ////lvForwardCancel.Visible = false;
            frmPopup.ShowDialog();
            //pnlForwarder.Visible = true;
            //pnlAddEdit.Controls.Add(pnlForwarder);
            //pnlAddEdit.BringToFront();
            //pnlForwarder.BringToFront();
            //pnlForwarder.Focus();

        }

        private void btnApprove_Click_1(object sender, EventArgs e)
        {
            try
            {
                PaymentVoucherDB prhdb = new PaymentVoucherDB();
                FinancialLimitDB flDB = new FinancialLimitDB();
                if (!flDB.checkEmployeeFinancialLimit(docID, Login.empLoggedIn, Convert.ToDouble(txtvoucherAmountINR.Text), 0))
                {
                    MessageBox.Show("No financial power for approving this document");
                    return;
                }
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevvh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevvh.CommentStatus);
                    prevvh.VoucherNo = DocumentNumberDB.getNewNumber(docID, 2);
                    if (verifyAndReworkVoucherDetailGridRows())
                    {
                        if (prhdb.ApprovePaymentVoucherHeader(prevvh))
                        {
                            MessageBox.Show("Payment Voucher Document Approved");
                            if (!updateDashBoard(prevvh, 2))
                            {
                                MessageBox.Show("DashBoard Fail to update");
                            }
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredVoucherHeader(listOption);
                            setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                        }
                        else
                            MessageBox.Show("Unable to approve");
                    }
                }
            }
            catch (Exception)
            {
            }

        }
        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            removeControlsFromCommenterPanel();
            removeControlsFromForwarderPanel();
            removeControlsFromLVPanel();
        }
        private void pnlList_Paint(object sender, PaintEventArgs e)
        {

        }
        //-----
        //comment handling procedurs and functions
        //-----
        private void btnGetComments_Click(object sender, EventArgs e)
        {
            //removeControlsFromCommenterPanel();

            //lvCmtr = new ListView();
            //lvCmtr.Clear();
            //pnlCmtr.BorderStyle = BorderStyle.FixedSingle;
            //pnlCmtr.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));

            docCmtrDB = new DocCommenterDB();
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            lvCmtr = docCmtrDB.commenterLV(docID);
            docCmtrDB.verifyCommenterList(lvCmtr, dtCmtStatus);
            lvCmtr.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            frmPopup.Controls.Add(lvCmtr);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Click);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click);
            frmPopup.Controls.Add(lvCancel);
            ////lvCancel.Visible = true;
            frmPopup.ShowDialog();
            //pnlCmtr.BringToFront();
            //pnlCmtr.Visible = true;
            //pnlComments.Controls.Add(pnlCmtr);
            //pnlComments.BringToFront();
            //pnlCmtr.BringToFront();

        }
        private void lvOK_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Update the document for sending the comment requests");
                if (lvCmtr.CheckedItems.Count > 0)
                {
                    foreach (ListViewItem itemRow in lvCmtr.Items)
                    {
                        if (itemRow.Checked)
                        {
                            //MessageBox.Show(itemRow.SubItems[1].Text);
                            commentStatus = commentStatus + itemRow.SubItems[1].Text + Main.delimiter1 +
                                itemRow.SubItems[2].Text + Main.delimiter1 +
                                "0" + Main.delimiter1 + Main.delimiter2;
                        }
                    }
                }
                else
                {
                    //if the existing commenter are removed
                    commentStatus = "Cleared";
                }
                frmPopup.Close();
                //frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_Click(object sender, EventArgs e)
        {
            try
            {
                frmPopup.Close();
                ///frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        private void lvForwardOK_Click(object sender, EventArgs e)
        {
            try
            {
                {
                    int kount = 0;
                    string approverUID = "";
                    string approverUName = "";
                    foreach (ListViewItem itemRow in lvApprover.Items)
                    {
                        if (itemRow.Checked)
                        {
                            approverUID = itemRow.SubItems[2].Text;
                            approverUName = itemRow.SubItems[1].Text;
                            kount++;
                        }
                    }
                    if (kount == 0)
                    {
                        MessageBox.Show("Select one approver");
                        return;
                    }
                    if (kount > 1)
                    {
                        MessageBox.Show("Select only one approver");
                        return;
                    }
                    else
                    {
                        DialogResult dialog = MessageBox.Show("Are you sure to forward the document ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            //do forward activities
                            PaymentVoucherDB vhDB = new PaymentVoucherDB();
                            prevvh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevvh.CommentStatus);
                            prevvh.ForwardUser = approverUID;
                            prevvh.ForwarderList = prevvh.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (vhDB.forwardPaymentVoucherHeader(prevvh))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(prevvh, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredVoucherHeader(listOption);
                                setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void lvForwardCancel_Click(object sender, EventArgs e)
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
        //-----
        private void dgvComments_CellDoubleClick(Object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                ////string columnName = grdList.Columns[e.ColumnIndex].Name;
                PrintForms.SimpleReportViewer.ShowDialog(dgvComments.Rows[e.RowIndex].Cells[3].Value.ToString(), "My Message", this);
            }
            catch (Exception ex)
            {
            }
        }
        private Boolean updateDashBoard(paymentvoucher prh, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = prh.DocumentID;
                dsb.TemporaryNo = prh.TemporaryNo;
                dsb.TemporaryDate = prh.TemporaryDate;
                dsb.DocumentNo = prh.VoucherNo;
                dsb.DocumentDate = prh.VoucherDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = prh.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(prevvh.DocumentID);
                    foreach (documentreceiver docRec in docList)
                    {
                        dsb.ToUser = docRec.EmployeeID;   //To store UserID Form DocumentReceiver for current Document
                        dsb.DocumentDate = UpdateTable.getSQLDateTime();
                        if (!ddsDB.insertDashboardAlarm(dsb))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                            status = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
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

        //return the previous forward list and forwarder 
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

        private void btnReverse_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Are you sure to Reverse the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    string s = prevvh.ForwarderList;
                    string reverseStr = getReverseString(prevvh.ForwarderList);
                    //do forward activities
                    prevvh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevvh.CommentStatus);
                    PaymentVoucherDB vhDB = new PaymentVoucherDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevvh.ForwarderList = reverseStr.Substring(0, ind);
                        prevvh.ForwardUser = reverseStr.Substring(ind + 3);
                        prevvh.DocumentStatus = prevvh.DocumentStatus - 1;
                    }
                    else
                    {
                        prevvh.ForwarderList = "";
                        prevvh.ForwardUser = "";
                        prevvh.DocumentStatus = 1;
                    }
                    if (vhDB.reversePaymentVoucherHeader(prevvh))
                    {
                        MessageBox.Show("Payment VOucher Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredVoucherHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void showPDFFile(string fname)
        {
            try
            {
                removePDFControls();
                AxAcroPDFLib.AxAcroPDF pdf = new AxAcroPDFLib.AxAcroPDF();
                pdf.Dock = System.Windows.Forms.DockStyle.Fill;
                pdf.Enabled = true;
                pdf.Location = new System.Drawing.Point(0, 0);
                pdf.Name = "pdfReader";
                pdf.OcxState = pdf.OcxState;
                ////pdf.OcxState = ((System.Windows.Forms.AxHost.State)(new System.ComponentModel.ComponentResourceManager(typeof(ViewerWindow)).GetObject("pdf.OcxState")));
                pdf.TabIndex = 1;
                pnlPDFViewer.Controls.Add(pdf);

                pdf.setShowToolbar(false);
                pdf.LoadFile(fname);
                pdf.setView("Fit");
                pdf.Visible = true;
                pdf.setZoom(100);
                pdf.setPageMode("None");
            }
            catch (Exception ex)
            {
            }
        }
        private void btnPDFClose_Click(object sender, EventArgs e)
        {
            removePDFControls();
            showPDFFileGrid();
        }
        private void removePDFControls()
        {
            try
            {
                foreach (Control p in pnlPDFViewer.Controls)
                    if (p.GetType() == typeof(AxAcroPDFLib.AxAcroPDF))
                    {
                        AxAcroPDFLib.AxAcroPDF c = (AxAcroPDFLib.AxAcroPDF)p;
                        c.Visible = false;
                        pnlPDFViewer.Controls.Remove(c);
                        c.Dispose();
                    }
            }
            catch (Exception ex)
            {
            }
        }
        private void showPDFFileGrid()
        {
            try
            {
                foreach (Control p in pnlPDFViewer.Controls)
                    if (p.GetType() == typeof(DataGridView))
                    {
                        p.Visible = true;
                    }
            }
            catch (Exception ex)
            {
            }
        }
        private void removePDFFileGridView()
        {
            try
            {
                foreach (Control p in pnlPDFViewer.Controls)
                    if (p.GetType() == typeof(DataGridView))
                    {
                        p.Dispose();
                    }
            }
            catch (Exception ex)
            {
            }
        }
        private void removeControlsFromCommenterPanel()
        {
            try
            {
                //foreach (Control p in pnlCmtr.Controls)
                //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
                //    {
                //        p.Dispose();
                //    }
                pnlCmtr.Controls.Clear();
                Control nc = pnlCmtr.Parent;
                nc.Controls.Remove(pnlCmtr);
            }
            catch (Exception ex)
            {
            }
        }
        private void removeControlsFromForwarderPanel()
        {
            try
            {
                //foreach (Control p in pnlForwarder.Controls)
                //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
                //    {
                //        p.Dispose();
                //    }
                pnlForwarder.Controls.Clear();
                Control nc = pnlForwarder.Parent;
                nc.Controls.Remove(pnlForwarder);
            }
            catch (Exception ex)
            {
            }
        }
        private void removeControlsFromLVPanel()
        {
            try
            {
                //foreach (Control p in pnllv.Controls)
                //{
                //    p.Dispose();
                //}
                pnllv.Controls.Clear();
                Control nc = pnllv.Parent;
                nc.Controls.Remove(pnllv);
            }
            catch (Exception ex)
            {
            }
        }
        private void btnListDocuments_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevvh.TemporaryNo + "-" + prevvh.TemporaryDate.ToString("yyyyMMddhhmmss"));
                pnlPDFViewer.Controls.Add(dgvDocumentList);
                dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
            }
            catch (Exception ex)
            {
            }
        }

        private void dgvDocumentList_CellContentClick(Object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView dgv = sender as DataGridView;
                string fileName = "";
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == 0)
                {
                    removePDFControls();
                    fileName = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    ////string docDir = Main.documentDirectory + "\\" + docID;
                    string subDir = prevvh.TemporaryNo + "-" + prevvh.TemporaryDate.ToString("yyyyMMddhhmmss");
                    ////DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    dgv.Enabled = false;
                    fileName = DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    ////showPDFFile(fileName);
                    ////dgv.Visible = false;
                    System.Diagnostics.Process.Start(fileName);
                    dgv.Enabled = true;
                }

            }
            catch (Exception ex)
            {
            }
        }
        private void setButtonVisibility(string btnName)
        {
            try
            {
                btnActionPending.Visible = true;
                btnApprovalPending.Visible = true;
                btnApproved.Visible = true;
                btnNew.Visible = false;
                btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                btnForward.Visible = false;
                btnApprove.Visible = false;
                btnReverse.Visible = false;
                btnGetComments.Visible = false;
                chkCommentStatus.Visible = false;
                txtComments.Visible = false;
                disableTabPages();
                clearTabPageControls();
                //----24/11/2016
                handleNewButton();
                handleGrdViewButton();
                handleGrdEditButton();
                handleGrdPrintButton();
                pnlBottomButtons.Visible = true;
                //----
                if (btnName == "init")
                {
                    ////btnNew.Visible = true; 24/11/2016
                    btnExit.Visible = true;

                }
                else if (btnName == "Commenter")
                {
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    pnlPDFViewer.Visible = true;
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    btnGetComments.Visible = false; //earlier Edit enabled this button
                    chkCommentStatus.Visible = true;
                    txtComments.Visible = true;
                }
                else if (btnName == "btnNew")
                {
                    btnNew.Visible = false; //added 24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = false;
                    tabComments.Enabled = false;
                    tabPDFViewer.Enabled = false;
                    tabControl1.SelectedTab = tabVHeader;
                }
                else if (btnName == "btnEditPanel") //called from cancel,save,forward,approve and reverse button events
                {
                    ////btnNew.Visible = true;
                    btnExit.Visible = true;
                }
                //gridview buttons
                else if (btnName == "Edit")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    btnGetComments.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = true;
                    chkCommentStatus.Visible = true;
                    txtComments.Visible = true;
                    tabControl1.SelectedTab = tabVHeader;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                    // btnQC.Visible = true;
                    disableTabPages();

                    tabControl1.SelectedTab = tabVHeader;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabVHeader;
                }
                else if (btnName == "LoadDocument")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                }

                changeListOptColor();
                pnlEditButtons.Refresh();
                //if the user privilege is only view, show only the Approved documents button
                int ups = getuserPrivilegeStatus();
                if (ups == 1 || ups == 0)
                {
                    grdList.Columns["Edit"].Visible = false;
                    grdList.Columns["Approve"].Visible = false;
                    btnActionPending.Visible = false;
                    btnApprovalPending.Visible = false;
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
        void changeListOptColor()
        {
            try
            {
                btnActionPending.UseVisualStyleBackColor = true;
                btnApproved.UseVisualStyleBackColor = true;
                btnApprovalPending.UseVisualStyleBackColor = true;
                switch (listOption)
                {
                    case 1:
                        btnActionPending.BackColor = Color.MediumAquamarine;
                        break;
                    case 2:
                        btnApprovalPending.BackColor = Color.MediumAquamarine;
                        break;
                    case 3:
                    case 6:
                        btnApproved.BackColor = Color.MediumAquamarine;
                        break;
                    default:
                        break;

                }
            }
            catch (Exception ex)
            {
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
            grdList.Columns["Approve"].Visible = false;
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                if (listOption == 1)
                {
                    grdList.Columns["Edit"].Visible = true;
                    grdList.Columns["Approve"].Visible = true;
                }
            }
        }
        void handleGrdPrintButton()
        {
            grdList.Columns["Print"].Visible = false;
            //if any one of view,add and edit
            if (Main.itemPriv[0] || Main.itemPriv[1] || Main.itemPriv[2])
            {
                //list option 1 should not have view buttuon visible (edit is avialable)
                if (listOption == 1 || listOption == 2)
                {
                    grdList.Columns["Print"].Visible = false;
                }
                else
                {
                    grdList.Columns["Print"].Visible = true;
                }
            }
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
        //call this form when new or edit buttons are clicked
        private void clearTabPageControls()
        {
            try
            {
                removePDFControls();
                removePDFFileGridView();
                removeControlsFromCommenterPanel();
                removeControlsFromLVPanel();
                removeControlsFromForwarderPanel();
                dgvComments.Rows.Clear();
                chkCommentStatus.Checked = false;
                txtComments.Text = "";
                grdPRDetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdPRDetail.Rows.Count != 0)
                {
                    DialogResult dialog = MessageBox.Show("Voucher Detail Will removed ?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        cmbBankTransMode.SelectedIndex = -1;
                        grdPRDetail.Rows.Clear();
                        txtVoucherAmount.Text = "";
                        txtvoucherAmountINR.Text = "";
                        cmbOfficeID.SelectedIndex = -1;
                    }
                    else
                        return;
                }
                showPayeeCodeDataGridView();
            }
            catch (Exception ex)
            {
            }
            ////btnSelect.Enabled = false;
            //// pnlEditButtons.Enabled = false;
            ////pnllv = new Panel();
            ////pnllv.BorderStyle = BorderStyle.FixedSingle;
            //frmPopup = new Form();
            //frmPopup.StartPosition = FormStartPosition.CenterScreen;
            //frmPopup.BackColor = Color.CadetBlue;

            //frmPopup.MaximizeBox = false;
            //frmPopup.MinimizeBox = false;
            //frmPopup.ControlBox = false;
            //frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            //frmPopup.Size = new Size(450, 300);
            //lv = ListViewFIll.getSLListView();
            //lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            ////this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            //lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            //frmPopup.Controls.Add(lv);

            //Button lvOK = new Button();
            //lvOK.BackColor = Color.Tan;
            //lvOK.Text = "OK";
            //lvOK.Location = new Point(44, 265);
            //lvOK.Click += new System.EventHandler(this.lvOK_Click1);
            //frmPopup.Controls.Add(lvOK);

            //Button lvCancel = new Button();
            //lvCancel.Text = "CANCEL";
            //lvCancel.BackColor = Color.Tan;
            //lvCancel.Location = new Point(141, 265);
            //lvCancel.Click += new System.EventHandler(this.lvCancel_Click1);
            //frmPopup.Controls.Add(lvCancel);

            //Label lblSearch = new Label();
            //lblSearch.Text = "Search";
            //lblSearch.Location = new Point(250, 267);
            //lblSearch.Size = new Size(45, 15);
            //frmPopup.Controls.Add(lblSearch);

            //txtSearch = new TextBox();
            //txtSearch.Location = new Point(300, 265);
            //txtSearch.TextChanged += new EventHandler(txtSearch_TextChangedEmp);
            //frmPopup.Controls.Add(txtSearch);

            ////pnlAddEdit.Controls.Add(pnllv);
            ////pnllv.BringToFront();
            ////pnllv.Visible = true;
            //txtSearch.Focus();
            //frmPopup.ShowDialog();
        }
        private void btnSelectPO_Click(object sender, EventArgs e)
        {

        }
        //private void lvOK_Click1(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //btnSelect.Enabled = true;
        //        //pnlEditButtons.Enabled = true;
        //        if (lv.Visible == true)
        //        {
        //            if (!checkLVItemChecked("Payee"))
        //            {
        //                return;
        //            }
        //            //btnSelect.Enabled = true;
        //            //pnlEditButtons.Enabled = true;
        //            foreach (ListViewItem itemRow in lv.Items)
        //            {
        //                if (itemRow.Checked)
        //                {
        //                    txtPayeeCode.Text = itemRow.SubItems[1].Text + "-" + itemRow.SubItems[2].Text;
        //                    SLType = itemRow.SubItems[3].Text;
        //                    if (SLType.Equals("Others"))
        //                        grdPRDetail.Columns["Select"].Visible = true;
        //                    else
        //                        grdPRDetail.Columns["Select"].Visible = false;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (!checkLVCopyItemChecked("Payee"))
        //            {
        //                return;
        //            }
        //            //btnSelect.Enabled = true;
        //            //pnlEditButtons.Enabled = true;
        //            foreach (ListViewItem itemRow in lvCopy.Items)
        //            {
        //                if (itemRow.Checked)
        //                {
        //                    txtPayeeCode.Text = itemRow.SubItems[1].Text + "-" + itemRow.SubItems[2].Text;
        //                    SLType = itemRow.SubItems[3].Text;
        //                    if (SLType.Equals("Others"))
        //                        grdPRDetail.Columns["Select"].Visible = true;
        //                    else
        //                        grdPRDetail.Columns["Select"].Visible = false;
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

        //private void lvCancel_Click1(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //btnSelect.Enabled = true;
        //        frmPopup.Close();
        //        frmPopup.Dispose();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //---------------------
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
                    txtPayeeCode.Text = row.Cells["ID"].Value.ToString() ;
                    txtPayeeName.Text = row.Cells["Name"].Value.ToString();
                    SLType = row.Cells["Type"].Value.ToString();
                    if (SLType.Equals("Others"))
                        grdPRDetail.Columns["Select"].Visible = true;
                    else
                        grdPRDetail.Columns["Select"].Visible = false;
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



        //-----
        //private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
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
        private void txtSearch_TextChangedEmp(object sender, EventArgs e)
        {
            frmPopup.Controls.Remove(lvCopy);
            addItemsEmp();
        }
        private void addItemsEmp()
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
            lvCopy.Columns.Add("Id", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Name", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Type", -2, HorizontalAlignment.Left);
            // lvCopy.Columns[4].Width = 0;
            lvCopy.Sorting = System.Windows.Forms.SortOrder.None;
            lvCopy.ColumnClick += new ColumnClickEventHandler(LvColumnClick); ;
            //this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
            //lvCopy.Location = new Point(13, 9);
            //lvCopy.Size = new Size(440, 199);
            lvCopy.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            lvCopy.Items.Clear();
            foreach (ListViewItem row in lv.Items)
            {
                string x = row.SubItems[0].Text;
                string no = row.SubItems[1].Text;
                string ch = row.SubItems[2].Text;
                string name = row.SubItems[3].Text;
                // string offNm = row.SubItems[4].Text;
                if (ch.ToLower().StartsWith(txtSearch.Text))
                {
                    ListViewItem item = new ListViewItem();
                    item.SubItems.Add(no);
                    item.SubItems.Add(ch);
                    item.SubItems.Add(name);
                    // item.SubItems.Add(offNm);
                    item.Checked = false;
                    lvCopy.Items.Add(item);
                }
            }
            lv.Visible = false;
            lvCopy.Visible = true;
            frmPopup.Controls.Add(lvCopy);
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            frmPopup.Controls.Remove(lvCopy);
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
            lvCopy.Columns.Add("Id", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Name", -2, HorizontalAlignment.Left);
            lvCopy.Sorting = System.Windows.Forms.SortOrder.None;
            lvCopy.ColumnClick += new ColumnClickEventHandler(LvColumnClick); ;
            //this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
            //lvCopy.Location = new Point(13, 9);
            //lvCopy.Size = new Size(440, 199);
            lvCopy.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
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
            frmPopup.Controls.Add(lvCopy);
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
        private void cmbBookType_SelectedValueChanged(object sender, EventArgs e)
        {
        }
        private void cmbVoucherType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string btype = ((Structures.ComboBoxItem)cmbBookType.SelectedItem).HiddenValue;
                //string btype = cmbBookType.SelectedItem.ToString().Substring(0, cmbBookType.SelectedItem.ToString().Trim().IndexOf('-'));
                if (IsEditMode && !btype.Equals(prevvh.BookType) && ch == 1 && grdPRDetail.Rows.Count != 0)
                {
                    DialogResult dialog = MessageBox.Show(" Credit Code Account and Voucher Detail Will removed ?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        cmbCreditACCode.SelectedIndex = -1;
                        grdPRDetail.Rows.Clear();
                        txtVoucherAmount.Text = "";
                        txtvoucherAmountINR.Text = "";
                        fillCreditAcCumbo(btype);
                    }
                    else
                        cmbBookType.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbBookType, prevvh.BookType);
                    //cmbBookType.FindString(prevvh.BookType);

                }
                else if (grdPRDetail.Rows.Count == 0)
                    fillCreditAcCumbo(btype);

                if (newClick)
                    ch = 0;
                if (ch == 0)
                    fillCreditAcCumbo(btype);
                ch = 1;
                if (btype == "BANKBOOK")
                {
                    docID = "BANKPAYMENTVOUCHER";
                    grdPRDetail.Columns["AccountNameDebit"].Width = 250;
                    grdPRDetail.Columns["AmountDebit"].Width = 150;
                }
                else
                {
                    docID = "CASHPAYMENTVOUCHER";
                    grdPRDetail.Columns["AccountNameDebit"].Width = 350;
                    grdPRDetail.Columns["AmountDebit"].Width = 150;
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void fillCreditAcCumbo(string BookType)
        {
            AccountDayBookCodeDB.fillAccountCodeComboNew(cmbCreditACCode, BookType);
            if (BookType.Equals("BANKBOOK"))
            {
                ////grdPRDetail.Columns["ChequeNo"].ReadOnly = false;
                ////grdPRDetail.Columns["ChequeDate"].ReadOnly = false;
                ////grdPRDetail.Columns["D2"].ReadOnly = false;
                grdPRDetail.Columns["ChequeNo"].Visible = true;
                grdPRDetail.Columns["ChequeDate"].Visible = true;
                grdPRDetail.Columns["D2"].Visible = true;

                cmbBankTransMode.Enabled = true;
            }
            else
            {
                ////grdPRDetail.Columns["ChequeNo"].ReadOnly = true;
                ////grdPRDetail.Columns["ChequeDate"].ReadOnly = true;
                ////grdPRDetail.Columns["D2"].ReadOnly = true;
                grdPRDetail.Columns["ChequeNo"].Visible = false;
                grdPRDetail.Columns["ChequeDate"].Visible = false;
                grdPRDetail.Columns["D2"].Visible = false;
                cmbBankTransMode.SelectedIndex = -1;
                cmbBankTransMode.Enabled = false;
            }
        }
        private void cmbSLType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (newClick && !Type.Equals(cmbSLType.SelectedItem.ToString()))
                {
                    txtPayeeCode.Text = "";
                }
                string slType = cmbSLType.SelectedItem.ToString();
                if (IsEditMode && !slType.Equals(prevvh.SLType) && track == 1 && grdPRDetail.Rows.Count != 0)
                {
                    DialogResult dialog = MessageBox.Show(" Credit payee Code and Voucher Detail Will removed ?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        txtPayeeCode.Text = "";
                        grdPRDetail.Rows.Clear();
                        txtVoucherAmount.Text = "";
                        txtvoucherAmountINR.Text = "";
                        if (slType.Equals("Others"))
                            ////grdPRDetail.Columns["Select"].ReadOnly = false;
                            grdPRDetail.Columns["Select"].Visible = true;
                        else
                            ////grdPRDetail.Columns["Select"].ReadOnly = true;
                            grdPRDetail.Columns["Select"].Visible = false;
                    }
                    else
                        cmbSLType.SelectedIndex = cmbSLType.FindString(prevvh.SLType);
                }
                if (grdPRDetail.Rows.Count == 0)
                {
                    txtPayeeCode.Text = "";
                    if (slType.Equals("Others"))
                        ////grdPRDetail.Columns["Select"].ReadOnly = false;
                        grdPRDetail.Columns["Select"].Visible = true;
                    else
                        ////grdPRDetail.Columns["Select"].ReadOnly = true;
                        grdPRDetail.Columns["Select"].Visible = false;
                }
                if (newClick)
                    track = 0;
                if (track == 0)
                {
                    if (slType.Equals("Others"))
                        ////grdPRDetail.Columns["Select"].ReadOnly = false;
                        grdPRDetail.Columns["Select"].Visible = true;
                    else
                        ////grdPRDetail.Columns["Select"].ReadOnly = true;
                        grdPRDetail.Columns["Select"].Visible = false;
                }
                track = 1;
                Type = cmbSLType.SelectedItem.ToString();
            }
            catch (Exception ex)
            {
            }
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
                    if (columnName.Equals("Select"))
                    {
                        if (txtPayeeCode.Text.Length != 0)
                            showInvoiceInListView(txtPayeeCode.Text);
                        else
                            MessageBox.Show("Select Payee");
                    }
                    if (columnName.Equals("D1") || columnName.Equals("D2"))
                    {
                        DateTime dt = DateTime.Today;
                        if (columnName.Equals("D1"))
                        {
                            dt = Convert.ToDateTime(grdPRDetail.Rows[e.RowIndex].Cells["BillDate"].Value);
                        }
                        else
                        {
                            dt = Convert.ToDateTime(grdPRDetail.Rows[e.RowIndex].Cells["ChequeDate"].Value);
                        }
                        //showDtPicker(e.ColumnIndex,e.RowIndex);
                        Rectangle tempRect = grdPRDetail.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                        ////dt.Location = tempRect.Location;
                        ////showDtPickerForm(tempRect.Left,tempRect.Top,tempRect.Location);
                        showDtPickerForm(Cursor.Position.X, Cursor.Position.Y, tempRect.Location, dt);
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

        //private void showAccountLIstView()
        //{
        //    //removeControlsFromLVPanel();
        //    //pnllv = new Panel();
        //    //pnllv.BorderStyle = BorderStyle.FixedSingle;

        //    //pnllv.Bounds = new Rectangle(new Point(24, 32), new Size(477, 282));
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
        //    //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked1);
        //    lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
        //    frmPopup.Controls.Add(lv);

        //    Button lvOK = new Button();
        //    lvOK.BackColor = Color.Tan;
        //    lvOK.Text = "OK";
        //    lvOK.Location = new Point(44, 265);
        //    lvOK.Click += new System.EventHandler(this.lvOK_Click2);
        //    frmPopup.Controls.Add(lvOK);

        //    Button lvCancel = new Button();
        //    lvCancel.Text = "CANCEL";
        //    lvCancel.BackColor = Color.Tan;
        //    lvCancel.Location = new Point(141, 265);
        //    lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
        //    frmPopup.Controls.Add(lvCancel);

        //    Label lblSearch = new Label();
        //    lblSearch.Text = "Search";
        //    lblSearch.Location = new Point(250, 267);
        //    lblSearch.Size = new Size(45, 15);
        //    frmPopup.Controls.Add(lblSearch);

        //    txtSearch = new TextBox();
        //    txtSearch.Location = new Point(300, 265);
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
        //                    grdPRDetail.CurrentRow.Cells["AccountCodeDebit"].Value = itemRow.SubItems[1].Text;
        //                    grdPRDetail.CurrentRow.Cells["AccountNameDebit"].Value = itemRow.SubItems[2].Text;
        //                    itemRow.Checked = false;
        //                    frmPopup.Close();
        //                    frmPopup.Dispose();
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
        //                    grdPRDetail.CurrentRow.Cells["AccountCodeDebit"].Value = itemRow.SubItems[1].Text + "-" + itemRow.SubItems[2].Text;
        //                    itemRow.Checked = false;
        //                    frmPopup.Close();
        //                    frmPopup.Dispose();
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
        //        frmPopup.Close();
        //        frmPopup.Dispose();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //==================
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
                    grdPRDetail.CurrentRow.Cells["AccountCodeDebit"].Value = row.Cells["AccountCode"].Value.ToString();
                    grdPRDetail.CurrentRow.Cells["AccountNameDebit"].Value = row.Cells["AccountName"].Value.ToString();
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
                payeeCodeGrd.CurrentCell = null;
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
        private void showInvoiceInListView(string payeecode)
        {
            //removeControlsFromLVPanel();
            //pnlAddEdit.Controls.Remove(pnllv);
            //pnllv = new Panel();
            //pnllv.BorderStyle = BorderStyle.FixedSingle;

            //pnllv.Bounds = new Rectangle(new Point(24, 32), new Size(477, 282));
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(600, 300);
            lv = InvoiceInHeaderDB.getINvoiceInDetailListView(payeecode);
            lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(600, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Click3);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click3);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();

        }
        private void lvOK_Click3(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                foreach (ListViewItem itemRow in lv.Items)
                {

                    if (itemRow.Checked)
                    {
                        count++;
                    }
                }
                if (count != 1)
                {
                    MessageBox.Show("Select One item.");
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {

                    if (itemRow.Checked)
                    {
                        grdPRDetail.CurrentRow.Cells["BillNo"].Value = itemRow.SubItems[3].Text;
                        grdPRDetail.CurrentRow.Cells["BillDate"].Value = Convert.ToDateTime(itemRow.SubItems[4].Text);
                        frmPopup.Close();
                        frmPopup.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Adding Error TO Gridview");
            }
        }
        private void lvCancel_Click3(object sender, EventArgs e)
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
        private void showDtPickerForm(int left, int top, Point location, DateTime dtvalue)
        {
            if (left > Screen.PrimaryScreen.Bounds.Width - 250)
            {
                left = Screen.PrimaryScreen.Bounds.Width - 250;
            }
            dtpForm = new Form();
            dtpForm.StartPosition = FormStartPosition.Manual;
            dtpForm.Size = new Size(200, 100);
            dtpForm.Location = new Point(left, top);
            //dtpForm.Location = location;
            ////dtpForm.StartPosition = FormStartPosition.CenterScreen;
            DateTimePicker dt = new DateTimePicker();
            dt.Format = DateTimePickerFormat.Custom;
            dt.CustomFormat = "dd-MM-yyyy";
            dt.ValueChanged += new EventHandler(cellDateTimePickerValueChanged);
            dt.Value = dtvalue;
            dtpForm.Controls.Add(dt);
            {
                ////dt.Location = new Point(10,10);
                dt.Width = 150;
                dt.Height = 100;
                dt.Visible = true;
                dt.ShowUpDown = true;
                ////dt.Show();
                System.Windows.Forms.SendKeys.Send("%{DOWN}");
            }
            dtpForm.ShowDialog();
        }
        void cellDateTimePickerValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            ////grdProductionPlanDetail.Rows[grdProductionPlanDetail.RowCount - 1].Cells["gProcessID"]
            grdPRDetail.Rows[grdPRDetail.CurrentCell.RowIndex].Cells[grdPRDetail.CurrentCell.ColumnIndex - 1].Value = dtp.Value;
            //dataGridView1.CurrentCell.Value = cellDateTimePicker.Value.ToString();//convert the date as per your format
            //cellDateTimePicker.Visible = false;
            ////dtp.Dispose();
            ////dtpForm.Dispose();

        }

        private void btnCalculate_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (grdPRDetail.Rows.Count == 0 || txtExchangeRate.Text.Length == 0)
                {
                    MessageBox.Show("Fill Voucher Detail Grid or Exchange Rate.");
                    return;
                }
                if (!verifyAndReworkVoucherDetailGridRows())
                {
                    MessageBox.Show("Voucher Detail validation Failed");
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void txtExchangeRate_TextChanged(object sender, EventArgs e)
        {
            if (IsEditMode && txtVoucherAmount.Text.Length != 0 && txtvoucherAmountINR.Text.Length != 0)
            {
                txtvoucherAmountINR.Text = "";
                txtVoucherAmount.Text = "";
            }
        }
        private void cmbCreditACCode_SelectedIndexChanged(object sender, EventArgs e)
        {

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

        private void btnClearEntries_Click(object sender, EventArgs e)
        {
            try
            {
                grdPRDetail.Rows.Clear();
                MessageBox.Show("Items Cleared");
            }
            catch(Exception ex)
            {

            }
        }

        private void PaymentVoucherOld_Enter(object sender, EventArgs e)
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



