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
    public partial class InvoiceInHeader : System.Windows.Forms.Form
    {
        //Boolean track = false;
        //int no = 0;
        TreeView tv = new TreeView();
        string docID = "";
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        decimal TotalToAdjust = 0;
        decimal TotalExpense = 0;
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Boolean userIsACommenter = false;
        //string chkTemp = "";
        string custID = "";
        RichTextBox txtDesc = new RichTextBox();
        double productvalue = 0.0;
        DataGridView voucherGrid = new DataGridView();
        DataGridView expenseGrid = new DataGridView();
        double taxvalue = 0.0;
        List<invoiceinpayments> payList = new List<invoiceinpayments>();
        List<invoiceinexpense> expList = new List<invoiceinexpense>();
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        invoiceinheader previnvoice;
        Form frmPopup = new Form();
        Boolean isValidate = true;
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        TabControl tab = new TabControl();
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        Panel pnlModel = new Panel();
        Boolean chkQuant = true;
        Boolean AddRowClick = false;
        Boolean isViewMode = false;
        //DateTimePicker dtp;
        public InvoiceInHeader()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }
        private void InvoiceInHeader_Load(object sender, EventArgs e)
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
            ListFilteredInvoiceInHeader(listOption);
        }
        private void ListFilteredInvoiceInHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                isValidate = true;
                isViewMode = false;
                InvoiceInHeaderDB inhdb = new InvoiceInHeaderDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);

                List<invoiceinheader> InvoiceInHeaderList = inhdb.getFilteredInvoiceInHeader(userString, option, userCommentStatusString);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (invoiceinheader inh in InvoiceInHeaderList)
                {
                    if (option == 1)
                    {
                        if (inh.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gRowID"].Value = inh.RowID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = inh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = inh.DocumentName;

                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = inh.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = inh.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentNo"].Value = inh.DocumentNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentDate"].Value = inh.DocumentDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gMRNNo"].Value = inh.MRNNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gMRNDate"].Value = inh.MRNDate;
                    if (inh.DocumentID.Equals("POINVOICEIN"))
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["gPONo"].Value = inh.PONos;
                        grdList.Rows[grdList.RowCount - 1].Cells["gPODate"].Value = inh.PODates;
                    }
                    else
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["gPONo"].Value = inh.MRNNo;
                        grdList.Rows[grdList.RowCount - 1].Cells["gPODate"].Value = inh.MRNDate;
                    }
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerID"].Value = inh.CustomerID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerName"].Value = inh.CustomerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["SupplierInvoiceNo"].Value = inh.SupplierInvoiceNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["SupplierInvoiceDate"].Value = inh.SupplierInvoiceDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyID"].Value = inh.CurrencyID;
                    grdList.Rows[grdList.RowCount - 1].Cells["ExchangeRate"].Value = inh.ExchangeRate;
                    //grdList.Rows[grdList.RowCount - 1].Cells["TCode"].Value = inh.TaxCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["FreightCharge"].Value = inh.FreightCharge;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProductValue"].Value = inh.ProductValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProductValueINR"].Value = inh.ProductValueINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProductTax"].Value = inh.ProductTax;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProductTaxINR"].Value = inh.ProductTaxINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["InvoiceValue"].Value = inh.InvoiceValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["InvoiceValueINR"].Value = inh.InvoiceValueINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["AdvancePaymentVoucher"].Value = inh.AdvancePaymentVouchers;
                    grdList.Rows[grdList.RowCount - 1].Cells["gRemarks"].Value = inh.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = inh.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["CmtStatus"].Value = inh.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = inh.status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = inh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = inh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwardUser"].Value = inh.ForwardUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ApproveUser"].Value = inh.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = inh.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = inh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = inh.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = inh.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = inh.ForwarderList;

                    grdList.Rows[grdList.RowCount - 1].Cells["PJVTNo"].Value = inh.PJVTNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["PJVTDate"].Value = inh.PJVTDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["PJVNo"].Value = inh.PJVNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["PJVDate"].Value = inh.PJVDate;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Invoice Listing");
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
            CurrencyDB.fillCurrencyComboNew(cmbCurrencyID);

            //TaxCodeDB.fillTaxCodeCombo(cmbTaxCode);
            dtTempDate.Format = DateTimePickerFormat.Custom;
            dtTempDate.CustomFormat = "dd-MM-yyyy";
            dtTempDate.Enabled = false;
            dtMRNDate.Format = DateTimePickerFormat.Custom;
            lblExpTotal.Text = "";
            lblAdvTotal.Text = "";
            cmbCurrencyID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCurrencyID, "INR");

            dtMRNDate.CustomFormat = "dd-MM-yyyy";
            dtMRNDate.Enabled = false;
            //dtPODate.Format = DateTimePickerFormat.Custom;
            //dtPODate.CustomFormat = "dd-MM-yyyy";
            dtDocumentDate.Format = DateTimePickerFormat.Custom;
            dtDocumentDate.CustomFormat = "dd-MM-yyyy";
            dtDocumentDate.Enabled = false;
            dtSupplierInvoiceDate.Format = DateTimePickerFormat.Custom;
            dtSupplierInvoiceDate.CustomFormat = "dd-MM-yyyy";
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            grdInvoiceInDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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
            txtDocumentNo.TabIndex = 2;
            dtDocumentDate.TabIndex = 3;
            txtMRNNo.TabIndex = 4;
            btnSelectMRNNo.TabIndex = 5;
            dtMRNDate.TabIndex = 6;
            txtSupplInvoiceNo.TabIndex = 7;
            dtSupplierInvoiceDate.TabIndex = 8;
            txtPONos.TabIndex = 9;
            txtPODates.TabIndex = 10;
            txtCustomerName.TabIndex = 11;
            ///txtFreightCharge.TabIndex = 12;
            cmbCurrencyID.TabIndex = 13;
            txtExchangeRate.TabIndex = 14;
           // txtAdvPaymentVouchers.TabIndex = 15;
            txtRemarks.TabIndex = 16;

            btnForward.TabIndex = 0;
            btnApprove.TabIndex = 1;
            btnCancel.TabIndex = 2;
            btnSave.TabIndex = 3;
            btnReverse.TabIndex = 4;
            btnUpdatePJV.TabIndex = 5;
            chkPJVApprove.TabIndex = 6;
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
                grdInvoiceInDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                payList.Clear();
                expList.Clear();
                TotalToAdjust = 0;
                TotalExpense = 0;
                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                pnllv.Visible = false;
                custID = "";
                // btnQC.Enabled = true;
                //----------
                lblExpTotal.Text = "";
                lblAdvTotal.Text = "";
                cmbSelectType.SelectedIndex = -1;
                cmbCurrencyID.SelectedIndex = -1;
                cmbCurrencyID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCurrencyID, "INR");
                //cmbTaxCode.SelectedIndex = -1;
                isViewMode = false;
                txtTemporarryNo.Text = "0";
                dtTempDate.Value = DateTime.Parse("1900-01-01");
                txtDocumentNo.Text = "";
                dtDocumentDate.Value = DateTime.Parse("1900-01-01");
                txtMRNNo.Text = "";
                dtMRNDate.Value = DateTime.Parse("1900-01-01");
                txtSupplInvoiceNo.Text = "";
                dtSupplierInvoiceDate.Value = DateTime.Now;
                txtPONos.Text = "";
                txtPODates.Text = "";
                //dtPODate.Value = DateTime.Today.Date;
                txtCustomerName.Text = "";
               /// txtFreightCharge.Text = "";
                ///txtAdvPaymentVouchers.Text = "";
                txtRemarks.Text = "";
                txtProductValue.Text = "";
                txtProductTax.Text = "";
                txtInvoiceValue.Text = "";
                txtInvoiceValueINR.Text = "";
                txtProductTaxINR.Text = "";
                btnProductValue.Text = "";
                btnTaxAmount.Text = "";
                txtProductValueINR.Text = "";
                //txtExchangeRate.Text = "";
                txtCustomerID.Text = "";
                commentStatus = "";
                chkPJVApprove.Checked = false;
                tabInvoiceInType.Enabled = true;
                tabInvoiceInType.Visible = true;
                tabInvoiceInHeader.Visible = false;
                tabInvoiceInDetail.Visible = false;
                AddRowClick = false;
                previnvoice = new invoiceinheader();
                isValidate = true;
                //removeControlsFromForwarderPanelTV();
                //removeControlsPnllvPanel();
                //removeControlsFromModelPanel();
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
                //track = true;
                clearData();
                //yy = 1;
                btnSave.Text = "Save";
                pnlAddEdit.Visible = true;
                closeAllPanels();
                pnlAddEdit.Visible = true;
                AddRowClick = false;
                custID = "";
                cmbSelectType.Enabled = true;
                setButtonVisibility("btnNew");
                isValidate = true;
                isViewMode = false;
            }
            catch (Exception)
            {

            }
        }


        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                AddInvoiceDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddInvoiceDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdInvoiceInDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkInvoiceDetailGridRows())
                    {
                        return false;
                    }
                }
                grdInvoiceInDetail.Rows.Add();
                int kount = grdInvoiceInDetail.RowCount;
                grdInvoiceInDetail.Rows[kount - 1].Cells["LineNo"].Value = kount;
                grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["Item"].Value = "";
                grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["ItemName"].Value = "";
                grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["ModelNo"].Value = "";
                grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["ModelName"].Value = "";
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdInvoiceInDetail.Rows[kount - 1].Cells["gTaxCode"]);
                TaxCodeDB.fillTaxCodeGridViewCombo(ComboColumn2, "");
                grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["Quantity"].Value = "";
                grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["Price"].Value = "";
                grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["Value"].Value = "";
                grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["Tax"].Value = "";
                grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["TaxDetails"].Value = "";
                if (AddRowClick)
                {
                    grdInvoiceInDetail.FirstDisplayedScrollingRowIndex = grdInvoiceInDetail.RowCount - 1;
                    grdInvoiceInDetail.CurrentCell = grdInvoiceInDetail.Rows[kount - 1].Cells[0];
                }
                //grdInvoiceInDetail.Columns[0].Frozen = false;
                grdInvoiceInDetail.FirstDisplayedScrollingColumnIndex = 0;
                //grdInvoiceInDetail.Columns["Sel"].Frozen = true;
            }

            catch (Exception ex)
            {
                MessageBox.Show("AddInvoiceDetailRow() : Error");
            }

            return status;
        }
        private Boolean verifyAndReworkInvoiceDetailGridRows()
        {
            Boolean status = true;

            try
            {
                double quantity = 0;
                double price = 0;
                double cost = 0.0;
                productvalue = 0.0;
                taxvalue = 0.0;
                string strtaxCode = "";
                if (grdInvoiceInDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in InvoiceIn details");
                    txtProductValue.Text = productvalue.ToString();
                    txtProductValueINR.Text = (Convert.ToDouble(txtProductValue.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    txtProductTax.Text = taxvalue.ToString(); //fill this later
                    txtProductTaxINR.Text = (Convert.ToDouble(txtProductTax.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    txtInvoiceValue.Text = (productvalue + taxvalue).ToString();
                    txtInvoiceValueINR.Text = (Convert.ToDouble(txtInvoiceValue.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    btnProductValue.Text = txtProductValue.Text;
                    btnTaxAmount.Text = txtProductTax.Text;
                    return false;
                }
                //clear tax details table
                TaxDetailsTable.Rows.Clear();
                if (!validateItems())
                {
                    MessageBox.Show("Validation failed");
                    return false;
                }
                for (int i = 0; i < grdInvoiceInDetail.Rows.Count; i++)
                {

                    grdInvoiceInDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if (grdInvoiceInDetail.Rows[i].Cells["gTaxCode"].Value == null)
                    {
                        MessageBox.Show("Fill Tax Code in row " + (i + 1));
                        return false;
                    }
                    if (docID == "WOINVOICEIN" && !isViewMode)
                    {
                        int refNo = Convert.ToInt32(grdInvoiceInDetail.Rows[i].Cells["ItemRefNo"].Value.ToString());
                        double quant = Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["Quantity"].Value.ToString());
                        workorderdetail wod = WorkOrderDB.getRefNoWiseWODetail(refNo);
                        if (wod.Quantity < quant)
                        {
                            MessageBox.Show("Entered quantity in Row: " + (i + 1) + " is more than available workorder quantity.\nWO Quantity: " + wod.Quantity);
                            return false;
                        }
                        double Invprice = Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["Price"].Value);
                        if (wod.Price < Invprice)
                        {
                            MessageBox.Show("Entered price in Row: " + (i + 1) + " is more than available workorder price.\nWO Price: " + wod.Price);
                            return false;
                        }
                    }
                    else if (docID.Equals("POINVOICEIN") && !isViewMode)
                    {
                        double Invprice = Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["Price"].Value);
                        podetail pod = PurchaseOrderDB.getPOQuantAndPRiceFromRowID(Convert.ToInt32(grdInvoiceInDetail.Rows[i].Cells["ItemRefNo"].Value));
                        double poprice = pod.Price;
                        if (Invprice > poprice)
                        {
                            MessageBox.Show("Price in Row: " + (i + 1) + " is more than PO Price.\nPO Price: " + poprice);
                            return false;
                        }
                        //double quant = Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["Quantity"].Value.ToString());
                        //if (pod.Quantity < quant)
                        //{
                        //    MessageBox.Show("Entered quantity is more than available PO quantity.\n Check row: " + (i + 1));
                        //    return false;
                        //}
                    }
                    else if (docID.Equals("POGENERALINVOICEIN") && !isViewMode)
                    {
                        double Invprice = Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["Price"].Value);
                        pogeneraldetail pogend = PurchaseOrderGeneralDB.getRefNoWisePriceINPOGen(Convert.ToInt32(grdInvoiceInDetail.Rows[i].Cells["ItemRefNo"].Value));
                        double poprice = pogend.Price;
                        
                        if (Invprice > poprice)
                        {
                            MessageBox.Show("Price in Row: " + (i + 1) + " is more than PO Price.\nPO Price: " + poprice);
                            return false;
                        }
                        double quant = Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["Quantity"].Value.ToString());
                        if (pogend.Quantity < quant)
                        {
                            MessageBox.Show("Entered quantity in Row: " + (i + 1) + " is more than available PO quantity.\nPO Quantity: " + pogend.Quantity);
                            return false;
                        }
                    }
                    if ((grdInvoiceInDetail.Rows[i].Cells["Item"].Value == null) ||
                        (grdInvoiceInDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Length == 0) ||
                        (grdInvoiceInDetail.Rows[i].Cells["Quantity"].Value == null) ||
                        (grdInvoiceInDetail.Rows[i].Cells["Quantity"].Value.ToString().Trim().Length == 0) ||
                        (Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["Quantity"].Value.ToString().Trim()) == 0) ||
                         (grdInvoiceInDetail.Rows[i].Cells["Price"].Value == null) ||
                        (grdInvoiceInDetail.Rows[i].Cells["Price"].Value.ToString().Trim().Length == 0) ||
                        (Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["Price"].Value.ToString().Trim()) == 0))

                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                  
                    quantity = Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["Quantity"].Value);
                    price = Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["Price"].Value);
                    if (price != 0 && quantity != 0)
                    {
                        cost = Math.Round(quantity * price, 2);
                    }

                    try
                    {
                        strtaxCode = grdInvoiceInDetail.Rows[i].Cells["gTaxCode"].Value.ToString();
                    }
                    catch (Exception)
                    {
                        strtaxCode = "";
                    }
                    System.Data.DataTable TaxData = TaxCodeWorkingDB.calculateTax(strtaxCode, cost);
                    double ttax1 = 0.0;
                    double ttax2 = 0.0;
                    string strTax = "";
                    for (int j = 0; j < TaxData.Rows.Count; j++)
                    {
                        string tstr = "";
                        try
                        {
                            tstr = TaxData.Rows[j][7].ToString().Trim().Substring(0, TaxData.Rows[j][7].ToString().Trim().IndexOf('-'));
                            if (!(tstr.Length == 0 && tstr.Equals("Dummy")))
                            {
                                ttax1 = Convert.ToDouble(TaxData.Rows[j][6]);
                                string a = Convert.ToString(TaxData.Rows[j][1]);
                                string b = Convert.ToString(TaxData.Rows[j][6]);
                                string c = Convert.ToString(TaxData.Rows[j][7]);
                                strTax = strTax + tstr + "-" +
                                    Convert.ToString(TaxData.Rows[j][6]) + "\n";
                                int taxcodefound = 0;
                                for (int k = 0; k < (TaxDetailsTable.Rows.Count); k++)
                                {
                                    if (TaxDetailsTable.Rows[k][0].ToString().Trim().Equals(tstr))
                                    {
                                        TaxDetailsTable.Rows[k][1] = Convert.ToDouble(TaxDetailsTable.Rows[k][1]) +
                                            Convert.ToDouble(TaxData.Rows[j][6]);
                                        taxcodefound = 1;
                                    }
                                }
                                if (taxcodefound == 0)
                                {
                                    TaxDetailsTable.Rows.Add();
                                    TaxDetailsTable.Rows[TaxDetailsTable.Rows.Count - 1][0] = tstr;
                                    TaxDetailsTable.Rows[TaxDetailsTable.Rows.Count - 1][1] =
                                       Convert.ToDouble(TaxData.Rows[j][6]);
                                }
                            }
                            else
                            {
                                ttax1 = 0.0;
                            }
                        }
                        catch (Exception)
                        {
                            ttax1 = 0.0;
                        }
                        ttax2 = ttax2 + ttax1;
                    }
                    grdInvoiceInDetail.Rows[i].Cells["Value"].Value = Convert.ToDouble(cost.ToString());
                    grdInvoiceInDetail.Rows[i].Cells["Tax"].Value = ttax2.ToString();
                    grdInvoiceInDetail.Rows[i].Cells["TaxDetails"].Value = strTax;
                    productvalue = productvalue + cost;
                    taxvalue = taxvalue + ttax2;

                    //- rewok tax value
                }
                txtProductValue.Text = productvalue.ToString();
                txtProductValueINR.Text = (Convert.ToDouble(txtProductValue.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                txtProductTax.Text = taxvalue.ToString(); //fill this later
                txtProductTaxINR.Text = (Convert.ToDouble(txtProductTax.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                txtInvoiceValue.Text = (productvalue + taxvalue).ToString();
                txtInvoiceValueINR.Text = (Convert.ToDouble(txtInvoiceValue.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                btnProductValue.Text = txtProductValue.Text;
                btnTaxAmount.Text = txtProductTax.Text;

            }
            catch (Exception ex)
            {

                return false;
            }
            return status;
        }

        //check for item duplication in details
        private Boolean validateItems()
        {
            Boolean status = true;
            try
            {
                for (int i = 0; i < grdInvoiceInDetail.Rows.Count - 1; i++)
                {
                    for (int j = i + 1; j < grdInvoiceInDetail.Rows.Count; j++)
                    {
                        if (grdInvoiceInDetail.Rows[i].Cells["ItemRefNo"].Value.ToString() ==
                            grdInvoiceInDetail.Rows[j].Cells["ItemRefNo"].Value.ToString())
                        {
                            MessageBox.Show("Item code duplicated in Invoice details...between Row " + (j + 1) + " and " + (i + 1) + " please ensure correctness (" +
                                grdInvoiceInDetail.Rows[i].Cells["ItemName"].Value.ToString() + ")");
                            status = false;
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

        private List<invoiceindetail> getInvoiceInDetails(invoiceinheader inh)
        {
            List<invoiceindetail> InvoiceDetailList = new List<invoiceindetail>();
            try
            {
                invoiceindetail ind = new invoiceindetail();
                for (int i = 0; i < grdInvoiceInDetail.Rows.Count; i++)
                {
                    ind = new invoiceindetail();
                    ind.DocumentID = inh.DocumentID;
                    ind.TemporaryNo = inh.TemporaryNo;
                    ind.TemporaryDate = inh.TemporaryDate;
                    ind.ItemReferenceNo = Convert.ToInt32(grdInvoiceInDetail.Rows[i].Cells["ItemRefNo"].Value.ToString());
                    ind.StockItemID = grdInvoiceInDetail.Rows[i].Cells["Item"].Value.ToString().Trim();//.Substring(0, grdInvoiceInDetail.Rows[i].Cells["Item"].Value.ToString().Trim().IndexOf('-'));
                    ind.StockItemName = grdInvoiceInDetail.Rows[i].Cells["ItemName"].Value.ToString().Trim();
                    ind.TaxCode = grdInvoiceInDetail.Rows[i].Cells["gTaxCode"].Value.ToString();
                    ind.Quantity = Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["Quantity"].Value);
                    if (ind.DocumentID.Equals("POINVOICEIN"))
                    {
                        ind.ModelNo = grdInvoiceInDetail.Rows[i].Cells["ModelNo"].Value.ToString();
                        ind.ModelName = grdInvoiceInDetail.Rows[i].Cells["ModelName"].Value.ToString();
                    }
                    else
                    {
                        ind.ModelNo = "";
                        ind.ModelName = "";
                    }
                    ind.Price = Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["Price"].Value);
                    ind.Tax = Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["Tax"].Value);
                    ind.TaxDetails = grdInvoiceInDetail.Rows[i].Cells["TaxDetails"].Value.ToString();
                    InvoiceDetailList.Add(ind);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("getInvoiceInDetails() : Error updating Invoice Details");
                InvoiceDetailList = null;
            }
            return InvoiceDetailList;
        }

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredInvoiceInHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredInvoiceInHeader(listOption);
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

            ListFilteredInvoiceInHeader(listOption);
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {

                InvoiceInHeaderDB inhdb = new InvoiceInHeaderDB();
                invoiceinheader inh = new invoiceinheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    if (!verifyAndReworkInvoiceDetailGridRows())
                    {
                        return;
                    }
                    isValidate = true;
                    inh.DocumentID = docID;
                    inh.DocumentDate = dtDocumentDate.Value;
                    // inh.CustomerID = c.SelectedItem.ToString().Trim().Substring(0, cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-'));
                    //inh.PONo =Convert.ToInt32(txtPONo.Text);
                    //inh.PODate = dtPODate.Value;
                    inh.MRNNo = Convert.ToInt32(txtMRNNo.Text);
                    inh.MRNDate = dtMRNDate.Value;
                    inh.SupplierInvoiceNo = txtSupplInvoiceNo.Text;
                    inh.SupplierInvoiceDate = dtSupplierInvoiceDate.Value;

                    inh.ExchangeRate = Convert.ToDecimal(txtExchangeRate.Text);
                    ////////inh.CurrencyID = cmbCurrencyID.SelectedItem.ToString().Trim().Substring(0, cmbCurrencyID.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
                    inh.CurrencyID = ((Structures.ComboBoxItem)cmbCurrencyID.SelectedItem).HiddenValue;
                    //mrnh.TransporterType = cmbTransporationType.SelectedItem.ToString().Trim().Substring(0, cmbTransporationType.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
                    inh.Remarks = txtRemarks.Text;

                    inh.CustomerID = txtCustomerID.Text;

                    inh.FreightCharge = 0;
                    //inh.TaxCode = cmbTaxCode.SelectedItem.ToString();
                    inh.AdvancePaymentVouchers ="";
                    inh.ProductValue = Convert.ToDouble(txtProductValue.Text);
                    inh.ProductValueINR = Convert.ToDouble(txtProductValueINR.Text);
                    inh.InvoiceValue = Convert.ToDouble(txtInvoiceValue.Text);
                    inh.ProductTax = Convert.ToDouble(txtProductTax.Text);
                    inh.ProductTaxINR = Convert.ToDouble(txtProductTaxINR.Text);
                    inh.InvoiceValueINR = Convert.ToDouble(txtInvoiceValueINR.Text);
                    //mrnh.ReferenceNo = txtReference.Text;
                    inh.Comments = docCmtrDB.DGVtoString(dgvComments).Replace("'", "''");
                    inh.ForwarderList = previnvoice.ForwarderList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("btnSave_Click_1() : 1-" + ex.ToString());
                    return;
                }

                if (!inhdb.validateInvoiceInHeader(inh))
                {
                    MessageBox.Show("btnSave_Click_1() : 2-Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    //inh.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    inh.DocumentStatus = 1; //created
                    inh.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    inh.TemporaryNo = Convert.ToInt32(txtTemporarryNo.Text);
                    inh.TemporaryDate = previnvoice.TemporaryDate;
                    inh.DocumentStatus = previnvoice.DocumentStatus;
                    // inh.QCStatus = prevmrn.QCStatus;
                }
                //Replacing single quotes
                inh = verifyHeaderInputString(inh);
                verifyDetailInputString();


                if (inhdb.validateInvoiceInHeader(inh))
                {
                    if (InvoiceInHeaderDB.IsSupplnvoiceNoFoundInPrevInvoiceIn(inh))
                    {
                        MessageBox.Show("Already InvoiceIn Available in this Supplier invoice no.Check Supplier invoice no you have entered.");
                        return;
                    }
                    //--create comment status string
                    docCmtrDB = new DocCommenterDB();
                    if (userIsACommenter)
                    {
                        //if the user is only a commenter and ticked the comment as final; then update his comment string as final
                        //and update the comment status
                        if (txtComments.Text != null && txtComments.Text.Trim().Length != 0)
                        {
                            docCmtrDB = new DocCommenterDB();
                            inh.CommentStatus = docCmtrDB.createCommentStatusString(previnvoice.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            inh.CommentStatus = previnvoice.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            inh.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            inh.CommentStatus = previnvoice.CommentStatus;
                        }
                    }
                    //----------
                    int tmpStatus = 1;

                    if (txtComments.Text.Trim().Length > 0)
                    {
                        inh.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus).Replace("'", "''");
                    }
                    List<invoiceindetail> InvoiceDetail = getInvoiceInDetails(inh);
                    if (docID.Equals("POINVOICEIN"))
                    {
                        MRNHeaderDB mrnhdb = new MRNHeaderDB();
                        List<mrndetail> MRNDetails = mrnhdb.getItemwiseTotalMRNDetail(inh.MRNNo, inh.MRNDate);
                        showItemWiseTotalQuantityForSelectedMRN(InvoiceDetail, MRNDetails);
                        foreach (invoiceindetail grdinv in InvoiceDetail)
                        {
                            Boolean isAvail = MRNDetails.Any(track => track.StockItemID == grdinv.StockItemID &&
                                                                    track.ModelNo == grdinv.ModelNo);
                            if (!isAvail)
                            {
                                MessageBox.Show(grdinv.StockItemID + ": Item is not available IN MRN.\nNot Allowed TO Save");  //For Quantity Not Equal to MRNQuantity
                                return;
                            }
                        }
                        foreach (mrndetail mrnd in MRNDetails)
                        {
                            bool isAvail = InvoiceDetail.Any(track => track.StockItemID == mrnd.StockItemID && track.ModelNo == mrnd.ModelNo);
                            if (!isAvail)
                            {
                                MessageBox.Show(mrnd.StockItemName + ": Not Available In INvoiceIN Detail(All Item of MRN Should available.\nNot Allowed TO Save)");  //For Not Available in INvoiceIn Detail
                                return;
                            }
                        }
                        if (!isValidate)
                        {
                            MessageBox.Show("Item Quantity Should equal to Total Item Quantity IN MRN.\nNot Allowed TO Save");  //For Quantity Not Equal to MRNQuantity
                            return;
                        }
                        DialogResult dialog = MessageBox.Show("Are you sure to " + btnSave.Text + " the document ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.No)
                        {
                            return;
                        }
                    }
                    else if (docID.Equals("WOINVOICEIN"))
                    {
                        //List<invoiceinheader> IIHList = InvoiceInHeaderDB.getInvoiceListAgainstOneWO(inh.MRNNo, inh.MRNDate); //MRNno : WOno & MRnDate: WO Date
                        showInvoiceIssuedDetailAgainstWOListView(InvoiceDetail, inh);
                        if (!isValidate)
                        {
                            MessageBox.Show("Enterd Quantity is more than the Wo Qunatity. Not Allowed TO Save/Update.");
                            return;
                        }
                        DialogResult dialog = MessageBox.Show("Are you sure to " + btnSave.Text + " the document ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.No)
                        {
                            return;
                        }
                    }
                    else if (docID.Equals("POGENERALINVOICEIN"))
                    {
                        //List<invoiceinheader> IIHList = InvoiceInHeaderDB.getInvoiceListAgainstOneWO(inh.MRNNo, inh.MRNDate); //MRNno : WOno & MRnDate: WO Date
                        showInvoiceIssuedDetailAgainstPOGenListView(InvoiceDetail, inh);
                        if (!isValidate)
                        {
                            MessageBox.Show("Enterd Quantity is more than the PO gen Qunatity. Not Allowed TO Save/Update.");
                            return;
                        }
                        DialogResult dialog = MessageBox.Show("Are you sure to " + btnSave.Text + " the document ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.No)
                        {
                            return;
                        }
                    }
                    //return;
                    //foreach()
                    if (btnText.Equals("Update"))
                    {
                        if (inhdb.updateInvoiceINHeaderAndDetail(inh, previnvoice, InvoiceDetail, payList,expList))
                        {
                            createPJV(previnvoice.TemporaryNo, previnvoice.TemporaryDate);
                            MessageBox.Show("Invoice In Header Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredInvoiceInHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update Invoie In Header");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        int TNo = 0;

                        if (inhdb.InsertInvoiceINHeaderAndDetail(inh, InvoiceDetail, out TNo,payList, expList))
                        {
                            createPJV(TNo, UpdateTable.getSQLDateTime());
                            MessageBox.Show("Invoice Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredInvoiceInHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert Invoice");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invoice Validation failed");
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
        private void showInvoiceIssuedDetailAgainstWOListView(List<invoiceindetail> InDList, invoiceinheader inh)
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

                frmPopup.Size = new Size(800, 300);
                lv = getInvoicePreparedDetailListViewWRTWO(InDList, inh);
                lv.Bounds = new Rectangle(new Point(0, 0), new Size(800, 250));
                frmPopup.Controls.Add(lv);

                Button lvCancel = new Button();
                lvCancel.BackColor = Color.Tan;
                lvCancel.Text = "CLOSE";
                lvCancel.Location = new Point(20, 265);
                lvCancel.Click += new System.EventHandler(this.lvCancel_Click3);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {

            }
        }
        private ListView getInvoicePreparedDetailListViewWRTWO(List<invoiceindetail> IinHList, invoiceinheader inh)
        {
            ListView lv = new ListView();
            try
            {
                //return;
                lv.View = System.Windows.Forms.View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                //lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;

                lv.Columns.Add("WO No", -2, HorizontalAlignment.Center);
                lv.Columns.Add("WO Date", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Item ID", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Item Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("WO Quant", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Issued Quant(A)", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Present Quant(B)", -2, HorizontalAlignment.Center);
                lv.Columns.Add("A + B", -2, HorizontalAlignment.Center);
                //lv.Columns[2].Width = 0;
                //lv.Columns[3].Width = 200;
                //lv.Columns[4].Width = 0;
                //lv.Columns[5].Width = 150;
                foreach (invoiceindetail iid in IinHList)
                {
                    double ItemIssueQuant = InvoiceInHeaderDB.getItemWiseTotalQuantOFWOIssuedInvoiceIn(iid.ItemReferenceNo);
                    double woQuant = WorkOrderDB.getRefNoWiseWODetail(iid.ItemReferenceNo).Quantity;
                    ListViewItem item1 = new ListViewItem(inh.MRNNo.ToString());
                    item1.SubItems.Add(inh.MRNDate.ToShortDateString());
                    item1.SubItems.Add(iid.StockItemID);
                    item1.SubItems.Add(iid.StockItemName);
                    item1.SubItems.Add(woQuant.ToString());          //For WO Quantity
                    item1.SubItems.Add(ItemIssueQuant.ToString());  // Total Issued Quant in all Invoice against perticular WO Item
                    item1.SubItems.Add(iid.Quantity.ToString());    //For Enter Quantity IN Current INVOICEIn
                    item1.SubItems.Add((ItemIssueQuant + iid.Quantity).ToString()); //Issue quant + written quant
                    if ((ItemIssueQuant + iid.Quantity) > woQuant)
                    {
                        isValidate = false;
                        item1.BackColor = Color.Magenta;
                    }
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            //track = true;
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            //tabControl1.SelectedTab = tabInvoiceInHeader;
            setButtonVisibility("btnEditPanel");

        }

        private void btnAddLine_Click_1(object sender, EventArgs e)
        {
            AddRowClick = true;
            AddInvoiceDetailRow();
        }
        private void grdPOPIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                string columnName = grdInvoiceInDetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdInvoiceInDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkInvoiceDetailGridRows();
                    }
                    if (columnName.Equals("ViewTaxDetails"))
                    {
                        //show tax details
                        DialogResult dialog = MessageBox.Show(grdInvoiceInDetail.Rows[e.RowIndex].Cells["TaxDetails"].Value.ToString(),
                            "Line No : " + (e.RowIndex + 1), MessageBoxButtons.OK);
                    }
                    if (columnName.Equals("Sel"))
                    {
                        showStockItemsForGridDetail();
                    }
                }

                catch (Exception)
                {

                }
            }
            catch (Exception ex)
            {

            }
        }
        //-----

        private Boolean checkAvailabilityOfitem()
        {
            Boolean status = true;
            if (grdInvoiceInDetail.CurrentRow.Cells["Item"].Value.ToString().Length != 0 ||
                grdInvoiceInDetail.CurrentRow.Cells["ModelNo"].Value.ToString().Length != 0 ||
                grdInvoiceInDetail.CurrentRow.Cells["ModelName"].Value.ToString().Length != 0)
            {
                status = false;
            }
            return status;
        }

        //---------------

        //private void showserviceItemsTreeView()
        //{
        //    try
        //    {
        //        removeControlsFromForwarderPanelTV();
        //        tv = new TreeView();
        //        tv.CheckBoxes = true;
        //        tv.Nodes.Clear();
        //        tv.CheckBoxes = true;
        //        pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
        //        pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
        //        Label lbl = new Label();
        //        lbl.AutoSize = true;
        //        lbl.Location = new Point(50, 8);
        //        lbl.Size = new Size(35, 13);

        //        lbl.Font = new Font("Serif", 10, FontStyle.Bold);
        //        lbl.ForeColor = Color.Green;
        //        lbl.Text = "Tree View For Service";
        //        tv = ServiceItemsDB.getServiceItemTreeView();
        //        pnlForwarder.Controls.Add(lbl);
        //        tv.Bounds = new Rectangle(new Point(50, 30), new Size(600, 220));
        //        pnlForwarder.Controls.Remove(tv);
        //        pnlForwarder.Controls.Add(tv);
        //        //tv.cl
        //        tv.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterCheck);
        //        Button lvForwrdOK = new Button();
        //        lvForwrdOK.Text = "OK";
        //        lvForwrdOK.BackColor = Color.LightGreen;
        //        lvForwrdOK.Size = new Size(100, 30);
        //        lvForwrdOK.Location = new Point(50, 260);
        //        lvForwrdOK.Click += new System.EventHandler(this.tvserviceOK_Click);
        //        pnlForwarder.Controls.Add(lvForwrdOK);

        //        Button lvForwardCancel = new Button();
        //        lvForwardCancel.Text = "Cancel";
        //        lvForwardCancel.BackColor = Color.LightGreen;
        //        lvForwardCancel.Size = new Size(100, 30);
        //        lvForwardCancel.Location = new Point(150, 260);
        //        lvForwardCancel.Click += new System.EventHandler(this.tvServCancel_Click);
        //        pnlForwarder.Controls.Add(lvForwardCancel);
        //        ////lvForwardCancel.Visible = false;
        //        //tv.CheckBoxes = true;
        //        pnlForwarder.Visible = true;
        //        pnlAddEdit.Controls.Add(pnlForwarder);
        //        pnlAddEdit.BringToFront();
        //        pnlForwarder.BringToFront();
        //        pnlForwarder.Focus();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //private void tvserviceOK_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        List<string> ItemList = GetServCheckedNodes(tv.Nodes);
        //        if (ItemList.Count > 1 || ItemList.Count == 0)
        //        {
        //            MessageBox.Show("select only one item");
        //            return;
        //        }
        //        foreach (string s in ItemList)
        //        {
        //            grdInvoiceInDetail.CurrentRow.Cells["Item"].Value = s;
        //            //grdPOPIDetail.CurrentRow.Cells["ServiceItem"].Value = s;
        //            tv.CheckBoxes = true;
        //            pnlForwarder.Controls.Remove(tv);
        //            pnlForwarder.Visible = false;
        //        }

        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //public List<string> GetServCheckedNodes(TreeNodeCollection nodes)
        //{
        //    List<string> nodeList = new List<string>();
        //    try
        //    {

        //        if (nodes == null)
        //        {
        //            return nodeList;
        //        }

        //        foreach (TreeNode childNode in nodes)
        //        {
        //            if (childNode.Checked)
        //            {
        //                nodeList.Add(childNode.Text);
        //            }
        //            nodeList.AddRange(GetCheckedNodes(childNode.Nodes));
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return nodeList;
        //}
        //private void tvServCancel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //lvApprover.CheckBoxes = false;
        //        //lvApprover.CheckBoxes = true;
        //        tv.CheckBoxes = true;
        //        pnlForwarder.Controls.Remove(tv);
        //        pnlForwarder.Visible = false;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //private void tvServ_AfterCheck(object sender, TreeViewEventArgs e)
        //{
        //    if (e.Node.Checked == true)
        //    {
        //        if (e.Node.Nodes.Count != 0)
        //        {
        //            MessageBox.Show("you are not allowed to select group");
        //            e.Node.Checked = false;
        //        }
        //    }
        //}
        private void showServiceItemListFromPOGEneral(pogeneralheader poh)
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(800, 300);
            lv = PurchaseOrderGeneralDB.getPODetailListView(poh);
            lv.Columns[1].Width = 0;
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(800, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_ClickDetGen);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_ClickDetGen);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
        }
        private void lvOK_ClickDetGen(object sender, EventArgs e)
        {
            try
            {
                if (!checkLVItemChecked("Item"))
                {
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        grdInvoiceInDetail.CurrentRow.Cells["ItemRefNo"].Value = itemRow.SubItems[1].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["Item"].Value = itemRow.SubItems[2].Text;//+ "-" + itemRow.SubItems[3].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["ItemName"].Value = itemRow.SubItems[3].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["Quantity"].Value = itemRow.SubItems[7].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["Price"].Value = itemRow.SubItems[8].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["gTaxCode"].Value = itemRow.SubItems[4].Text;
                        frmPopup.Close();
                        frmPopup.Dispose();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void lvCancel_ClickDetGen(object sender, EventArgs e)
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
        //-------------------
        private void showServiceItemListFromWO(workorderheader woh)
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(800, 300);
            lv = WorkOrderDB.getWODetailListView(woh);
            lv.Columns[1].Width = 0;
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(800, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_ClickDet);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_ClickDet);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
        }
        private void lvOK_ClickDet(object sender, EventArgs e)
        {
            try
            {
                if (!checkLVItemChecked("Item"))
                {
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        grdInvoiceInDetail.CurrentRow.Cells["ItemRefNo"].Value = itemRow.SubItems[1].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["Item"].Value = itemRow.SubItems[2].Text;//+ "-" + itemRow.SubItems[3].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["ItemName"].Value = itemRow.SubItems[3].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["Quantity"].Value = itemRow.SubItems[7].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["Price"].Value = itemRow.SubItems[8].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["gTaxCode"].Value = itemRow.SubItems[4].Text;
                        frmPopup.Close();
                        frmPopup.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void lvCancel_ClickDet(object sender, EventArgs e)
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


        private void showStockItemsForGridDetail()
        {
            removeControlsFromForwarderPanelTV();
            //pnlAddEdit.Controls.Remove(pnlForwarder);
            if (!checkAvailabilityOfitem())
            {
                DialogResult dialog = MessageBox.Show("Selected product and Model detail will removed?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdInvoiceInDetail.CurrentRow.Cells["Item"].Value = "";
                    grdInvoiceInDetail.CurrentRow.Cells["ItemName"].Value = "";
                    grdInvoiceInDetail.CurrentRow.Cells["ModelNo"].Value = "";
                    grdInvoiceInDetail.CurrentRow.Cells["ModelName"].Value = "";
                }
                else
                    return;
            }
            if (docID.Equals("WOINVOICEIN"))
            {
                workorderheader woh = new workorderheader();
                woh.DocumentID = "WORKORDER";
                woh.WONo = Convert.ToInt32(txtMRNNo.Text);
                woh.WODate = dtMRNDate.Value;
                showServiceItemListFromWO(woh);
            }
            else if (docID.Equals("POINVOICEIN"))
            {
                showListViewOfSelectedMRNItems();
            }
            else if (docID.Equals("POGENERALINVOICEIN"))
            {
                pogeneralheader poh = new pogeneralheader();
                poh.DocumentID = "POGENERAL";
                poh.PONo = Convert.ToInt32(txtMRNNo.Text);
                poh.PODate = dtMRNDate.Value;
                showServiceItemListFromPOGEneral(poh);
            }
        }
        //private void tvOK_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        List<string> ItemList = GetCheckedNodes(tv.Nodes);
        //        if (ItemList.Count > 1 || ItemList.Count == 0)
        //        {
        //            MessageBox.Show("select only one item");
        //            return;
        //        }
        //        foreach (string s in ItemList)
        //        {
        //            grdInvoiceInDetail.CurrentRow.Cells["Item"].Value = s;
        //            tv.CheckBoxes = true;
        //            pnlForwarder.Controls.Remove(tv);
        //            pnlForwarder.Visible = false;
        //            showModelListView(s);
        //        }

        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //public List<string> GetCheckedNodes(TreeNodeCollection nodes)
        //{
        //    List<string> nodeList = new List<string>();
        //    try
        //    {

        //        if (nodes == null)
        //        {
        //            return nodeList;
        //        }

        //        foreach (TreeNode childNode in nodes)
        //        {
        //            if (childNode.Checked)
        //            {
        //                nodeList.Add(childNode.Text);
        //            }
        //            nodeList.AddRange(GetCheckedNodes(childNode.Nodes));
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return nodeList;
        //}
        //private void tvCancel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //lvApprover.CheckBoxes = false;
        //        //lvApprover.CheckBoxes = true;
        //        tv.CheckBoxes = true;
        //        pnlForwarder.Controls.Remove(tv);
        //        pnlForwarder.Visible = false;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //private void tv_AfterCheck(object sender, TreeViewEventArgs e)
        //{
        //    if (e.Node.Checked == true)
        //    {
        //        if (e.Node.Nodes.Count != 0)
        //        {
        //            MessageBox.Show("you are not allowed to select group");
        //            e.Node.Checked = false;
        //        }
        //    }
        //}
        //private void showListViewForWorkOrder()
        //{
        //    try
        //    {
        //        frmPopup = new Form();
        //        frmPopup.StartPosition = FormStartPosition.CenterScreen;
        //        frmPopup.BackColor = Color.CadetBlue;

        //        frmPopup.MaximizeBox = false;
        //        frmPopup.MinimizeBox = false;
        //        frmPopup.ControlBox = false;
        //        frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

        //        frmPopup.Size = new Size(450, 300);
        //        lv = CatalogueValueDB.getCatalogValueListView("ServiceLookup");
        //        //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView2_ItemCheck3);
        //        //lv.MultiSelect = false;
        //        lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
        //        frmPopup.Controls.Add(lv);

        //        Button lvOK = new Button();
        //        lvOK.BackColor = Color.Tan;
        //        lvOK.Text = "OK";
        //        lvOK.Location = new Point(40, 265);
        //        lvOK.Click += new System.EventHandler(this.lvOK_Clicked3);
        //        frmPopup.Controls.Add(lvOK);

        //        Button lvCancel = new Button();
        //        lvCancel.BackColor = Color.Tan;
        //        lvCancel.Text = "CANCEL";
        //        lvCancel.Location = new Point(130, 265);
        //        lvCancel.Click += new System.EventHandler(this.lvCancel_Clicked3);
        //        frmPopup.Controls.Add(lvCancel);
        //        frmPopup.ShowDialog();
        //        //pnlAddEdit.Controls.Add(pnllv);
        //        //pnllv.BringToFront();
        //        //pnllv.Visible = true;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //private void lvOK_Clicked3(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (!checkLVItemChecked("Item"))
        //        {
        //            return;
        //        }
        //        foreach (ListViewItem itemRow in lv.Items)
        //        {
        //            if (itemRow.Checked)
        //            {
        //                grdInvoiceInDetail.CurrentRow.Cells["Item"].Value = itemRow.SubItems[1].Text + "-" + itemRow.SubItems[2].Text;
        //                frmPopup.Close();
        //                frmPopup.Dispose();
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //private void lvCancel_Clicked3(object sender, EventArgs e)
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
        private void showListViewOfSelectedMRNItems()
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

                frmPopup.Size = new Size(900, 300);
                lv = MRNHeaderDB.getMRNDetailListView(Convert.ToInt32(txtMRNNo.Text.Trim()), Convert.ToDateTime(dtMRNDate.Value), "MRN");
                lv.Bounds = new Rectangle(new Point(0, 0), new Size(900, 250));
                frmPopup.Controls.Add(lv);

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new Point(40, 265);
                lvOK.Click += new System.EventHandler(this.lvOK_Clicked6);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.BackColor = Color.Tan;
                lvCancel.Text = "CANCEL";
                lvCancel.Location = new Point(130, 265);
                lvCancel.Click += new System.EventHandler(this.lvCancel_Click6);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void lvOK_Clicked6(object sender, EventArgs e)
        {
            try
            {
                if (!checkLVItemChecked("Item"))
                {
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        grdInvoiceInDetail.CurrentRow.Cells["Item"].Value = itemRow.SubItems[3].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["ItemName"].Value = itemRow.SubItems[4].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["ModelNo"].Value = itemRow.SubItems[5].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["ModelName"].Value = itemRow.SubItems[6].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["Quantity"].Value = itemRow.SubItems[7].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["Price"].Value = itemRow.SubItems[8].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["gTaxCode"].Value = itemRow.SubItems[9].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["ItemRefNo"].Value = itemRow.SubItems[10].Text;

                    }
                }
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        private void lvCancel_Click6(object sender, EventArgs e)
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
        private void removeControlsFromForwarderPanelTV()
        {
            try
            {
                //foreach (Control p in pnlForwarder.Controls)
                //    if (p.GetType() == typeof(TreeView) || p.GetType() == typeof(Button) || p.GetType() == typeof(ListView))
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
        private void showModelListView(string stockID)
        {
            //removeControlsFromModelPanel();
            //lv = new ListView();
            //lv.CheckBoxes = true;
            //lv.Items.Clear();
            //pnlModel.BorderStyle = BorderStyle.FixedSingle;
            //pnlModel.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 310);
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Location = new Point(5, 5);
            lbl.Size = new Size(300, 20);
            lbl.Text = "ListView For Model";
            lbl.Font = new Font("Serif", 10, FontStyle.Bold);
            lbl.ForeColor = Color.Green;
            frmPopup.Controls.Add(lbl);
            lv = ProductModelsDB.getModelsForProductListView(stockID.Substring(0, stockID.IndexOf('-')));
            if (lv.Items.Count == 0)
            {
                grdInvoiceInDetail.CurrentRow.Cells["ModelNo"].Value = "NA";
                grdInvoiceInDetail.CurrentRow.Cells["ModelName"].Value = "NA";
                pnlModel.Visible = false;
                return;
            }
            lv.Bounds = new Rectangle(new Point(0, 25), new Size(450, 250));
            //frmPopup.Controls.Remove(lv);
            frmPopup.Controls.Add(lv);
            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.BackColor = Color.Tan;
            lvOK.Location = new System.Drawing.Point(40, 280);
            lvOK.Click += new System.EventHandler(this.lvOK_Click2);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new System.Drawing.Point(130, 280);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
            ////lvForwardCancel.Visible = false;
            //tv.CheckBoxes = true;
            //pnlModel.Visible = true;
            //pnlAddEdit.Controls.Add(pnlModel);
            //pnlAddEdit.BringToFront();
            //pnlModel.BringToFront();
            //pnlModel.Focus();
        }
        private void lvOK_Click2(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                foreach (ListViewItem item in lv.Items)
                {
                    if (item.Checked == true)
                    {
                        count++;
                    }
                }
                if (count != 1)
                {
                    MessageBox.Show("select one item");
                    return;
                }
                foreach (ListViewItem item in lv.Items)
                {
                    if (item.Checked == true)
                    {
                        grdInvoiceInDetail.CurrentRow.Cells["ModelNo"].Value = item.SubItems[1].Text;
                        grdInvoiceInDetail.CurrentRow.Cells["ModelName"].Value = item.SubItems[2].Text;
                        frmPopup.Close();
                        frmPopup.Dispose();
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
                //lvApprover.CheckBoxes = false;
                //lvApprover.CheckBoxes = true;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        private void removeControlsFromModelPanel()
        {
            try
            {
                //foreach (Control p in pnlModel.Controls)
                //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
                //    {
                //        p.Dispose();
                //    }
                pnlModel.Controls.Clear();
                Control nc = pnlModel.Parent;
                nc.Controls.Remove(pnlModel);
            }
            catch (Exception ex)
            {
            }
        }

        //-----
        private void btnCalculateax_Click_1(object sender, EventArgs e)
        {
            verifyAndReworkInvoiceDetailGridRows();
        }

        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("LoadDocument") ||
                    columnName.Equals("View"))
                {

                    clearData();
                    //track = true;
                    setButtonVisibility(columnName);
                    AddRowClick = false;
                    previnvoice = new invoiceinheader();
                    chkPJVApprove.Checked = false;

                    // previnvoice.QCStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["QCStatus"].Value.ToString());
                    //if (columnName.Equals("Approve") && previnvoice.QCStatus == 1)
                    //{

                    //    btnForward.Visible = false;
                    //    btnApprove.Visible = false;
                    //    btnReverse.Visible = false;
                    //    btnQC.Visible = false;
                    //    btnQCCompleted.Visible = true;
                    //}
                    previnvoice.RowID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gRowID"].Value.ToString());
                    previnvoice.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();
                    previnvoice.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    if (previnvoice.DocumentID.Equals("POINVOICEIN"))
                        cmbSelectType.SelectedIndex = cmbSelectType.FindString("MRN");
                    else if (previnvoice.DocumentID.Equals("WOINVOICEIN"))
                        cmbSelectType.SelectedIndex = cmbSelectType.FindString("Work Order");
                    else
                        cmbSelectType.SelectedIndex = cmbSelectType.FindString("PO General");
                    previnvoice.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    //previnvoice.Reference = grdList.Rows[e.RowIndex].Cells["Reference"].Value.ToString();
                    previnvoice.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    previnvoice.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    previnvoice.DocumentNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentNo"].Value.ToString());
                    previnvoice.DocumentDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["DocumentDate"].Value.ToString());

                    previnvoice.PJVTNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["PJVTNo"].Value.ToString());
                    previnvoice.PJVTDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["PJVTDate"].Value.ToString());
                    previnvoice.PJVNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["PJVNo"].Value.ToString());
                    previnvoice.PJVDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["PJVDate"].Value.ToString());

                    if (previnvoice.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    previnvoice.Comments = InvoiceInHeaderDB.getUserComments(previnvoice.DocumentID, previnvoice.TemporaryNo, previnvoice.TemporaryDate);
                    docID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    //setPODetailColumns(docID);

                    InvoiceInHeaderDB popidb = new InvoiceInHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    if (columnName == "View")
                    {
                        tabControl1.TabPages["tabInvoiceInDetail"].Enabled = true;
                        tabControl1.TabPages["tabInvoiceInHeader"].Enabled = true;
                        isViewMode = true;
                    }

                    previnvoice.MRNNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gMRNNo"].Value.ToString());
                    previnvoice.MRNDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gMRNDate"].Value.ToString());
                    previnvoice.CustomerID = grdList.Rows[e.RowIndex].Cells["gCustomerID"].Value.ToString();
                    previnvoice.CustomerName = grdList.Rows[e.RowIndex].Cells["gCustomerName"].Value.ToString();

                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Document Temp No:" + previnvoice.TemporaryNo + "\n" +
                            "Document Temp Date:" + previnvoice.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "Document No:" + previnvoice.DocumentNo + "\n" +
                            "Document Date:" + previnvoice.DocumentDate.ToString("dd-MM-yyyy");
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = previnvoice.TemporaryNo + "-" + previnvoice.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_1(null, null);
                        return;
                    }
                    //--------

                    previnvoice.PONos = grdList.Rows[e.RowIndex].Cells["gPONo"].Value.ToString();
                    previnvoice.PODates = grdList.Rows[e.RowIndex].Cells["gPODate"].Value.ToString();
                    previnvoice.SupplierInvoiceNo = grdList.Rows[e.RowIndex].Cells["SupplierInvoiceNo"].Value.ToString();
                    previnvoice.SupplierInvoiceDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["SupplierInvoiceDate"].Value.ToString());
                    //previnvoice.TaxCode = grdList.Rows[e.RowIndex].Cells["TCode"].Value.ToString();
                    previnvoice.CurrencyID = grdList.Rows[e.RowIndex].Cells["gCurrencyID"].Value.ToString();
                    previnvoice.ExchangeRate = Convert.ToDecimal(grdList.Rows[e.RowIndex].Cells["ExchangeRate"].Value.ToString());
                    previnvoice.FreightCharge = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["FreightCharge"].Value.ToString());
                    previnvoice.ProductValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gProductValue"].Value.ToString());
                    previnvoice.ProductTax = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["ProductTax"].Value.ToString());
                    previnvoice.ProductTaxINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["ProductTaxINR"].Value.ToString());
                    previnvoice.InvoiceValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["InvoiceValue"].Value.ToString());
                    previnvoice.InvoiceValueINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["InvoiceValueINR"].Value.ToString());
                    previnvoice.AdvancePaymentVouchers = grdList.Rows[e.RowIndex].Cells["AdvancePaymentVoucher"].Value.ToString();
                    previnvoice.ProductValueINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["ProductValueINR"].Value.ToString());

                    previnvoice.Remarks = grdList.Rows[e.RowIndex].Cells["gRemarks"].Value.ToString();
                    //previnvoice.Comments = grdList.Rows[e.RowIndex].Cells["Comments"].Value.ToString();
                    previnvoice.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();
                    previnvoice.status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    previnvoice.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    previnvoice.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    previnvoice.ForwardUser = grdList.Rows[e.RowIndex].Cells["ForwardUser"].Value.ToString();
                    previnvoice.ApproveUser = grdList.Rows[e.RowIndex].Cells["ApproveUser"].Value.ToString();
                    previnvoice.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    previnvoice.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    previnvoice.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();

                    previnvoice.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    previnvoice.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    previnvoice.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();

                    payList = InvoiceInHeaderDB.getInvoiceInAdvPaymentDetails(previnvoice.TemporaryNo, previnvoice.TemporaryDate,previnvoice.DocumentID);
                    decimal TotalAdv = payList.Sum(pay => pay.Amount);
                    if (TotalAdv != 0)
                    {
                        lblAdvTotal.Text = TotalAdv.ToString();
                    }
                    else
                    {
                        lblAdvTotal.Text = "";
                    }

                    expList = InvoiceInHeaderDB.getExpenseDetialForInvoiceIN(previnvoice);
                    decimal TotalExp = expList.Sum(exp => exp.Amount);
                    if (TotalExp != 0)
                    {
                        lblExpTotal.Text = TotalExp.ToString();
                    }
                    else
                    {
                        lblExpTotal.Text = "";
                    }

                    //--comments
                    ///chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    previnvoice.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(previnvoice.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(previnvoice.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---


                    // txtReference.Text = prevmrn.Reference.ToString();
                    txtTemporarryNo.Text = previnvoice.TemporaryNo.ToString();
                    dtTempDate.Value = previnvoice.TemporaryDate;
                    //dtTempDate.Value = previnvoice.TemporaryDate;

                    txtMRNNo.Text = previnvoice.MRNNo.ToString();
                    try
                    {
                        dtMRNDate.Value = previnvoice.MRNDate;
                    }
                    catch (Exception ex)
                    {
                        dtMRNDate.Value = DateTime.Parse("01-01-1900");
                    }

                    txtDocumentNo.Text = previnvoice.DocumentNo.ToString();
                    dtDocumentDate.Value = previnvoice.DocumentDate;
                    txtSupplInvoiceNo.Text = previnvoice.SupplierInvoiceNo.ToString();
                    dtSupplierInvoiceDate.Value = previnvoice.SupplierInvoiceDate;
                    txtProductValue.Text = previnvoice.ProductValue.ToString();
                    txtCustomerID.Text = previnvoice.CustomerID.ToString();
                    txtCustomerName.Text = previnvoice.CustomerName.ToString();
                    //txtFreightCharge.Text = previnvoice.FreightCharge.ToString();
                    //txtAdvPaymentVouchers.Text = previnvoice.AdvancePaymentVouchers.ToString();
                    txtRemarks.Text = previnvoice.Remarks.ToString();
                    txtProductTax.Text = previnvoice.ProductTax.ToString();
                    txtProductTaxINR.Text = previnvoice.ProductTaxINR.ToString();
                    txtInvoiceValue.Text = previnvoice.InvoiceValue.ToString();
                    txtInvoiceValueINR.Text = previnvoice.InvoiceValueINR.ToString();
                    
                    txtProductValueINR.Text = previnvoice.ProductValueINR.ToString();
                    //cmbTaxCode.SelectedIndex = cmbTaxCode.FindString(previnvoice.TaxCode);
                    //////////cmbCurrencyID.SelectedIndex = cmbCurrencyID.FindString(previnvoice.CurrencyID);
                    cmbCurrencyID.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCurrencyID, previnvoice.CurrencyID);
                    txtExchangeRate.Text = previnvoice.ExchangeRate.ToString();
                    //cmbCustomer.Text = prevpopi.CustomerName;
                    txtPONos.Text = previnvoice.PONos.ToString();
                    txtPODates.Text = previnvoice.PODates.ToString();
                    //try
                    //{
                    //    dtPODate.Value = previnvoice.PODate;
                    //}
                    //catch (Exception)
                    //{

                    //    dtPODate.Value = DateTime.Parse("01-01-1900");
                    //}

                    if (previnvoice.DocumentID.Equals("POINVOICEIN"))
                    {
                        grdInvoiceInDetail.Columns["ModelNo"].Visible = false;
                        grdInvoiceInDetail.Columns["ModelName"].Visible = false;
                        btnShowPOMRNDetail.Text = "View PO/MRN Det";
                    }
                    else if (previnvoice.DocumentID.Equals("WOINVOICEIN"))
                    {
                        grdInvoiceInDetail.Columns["ModelNo"].Visible = false;
                        grdInvoiceInDetail.Columns["ModelName"].Visible = false;
                        btnShowPOMRNDetail.Text = "View WO Det";
                    }
                    else
                    {
                        grdInvoiceInDetail.Columns["ModelNo"].Visible = false;
                        grdInvoiceInDetail.Columns["ModelName"].Visible = false;
                        btnShowPOMRNDetail.Text = "View POGen Det";
                    }
                    List<invoiceindetail> invoicedetailList = InvoiceInHeaderDB.getInvoiceDetail(previnvoice);
                    grdInvoiceInDetail.Rows.Clear();
                    int i = 0;
                    try
                    {
                        foreach (invoiceindetail ind in invoicedetailList)
                        {
                            if (!AddInvoiceDetailRow())
                            {
                                MessageBox.Show("Adding row to detail grid failed.");
                            }
                            else
                            {
                                try
                                {
                                    grdInvoiceInDetail.Rows[i].Cells["Item"].Value = ind.StockItemID;
                                    grdInvoiceInDetail.Rows[i].Cells["ItemName"].Value = ind.StockItemName;
                                }
                                catch (Exception ex)
                                {
                                    grdInvoiceInDetail.Rows[i].Cells["Item"].Value = "";
                                    grdInvoiceInDetail.Rows[i].Cells["ItemName"].Value = "";
                                }
                                grdInvoiceInDetail.Rows[i].Cells["ModelNo"].Value = ind.ModelNo;
                                grdInvoiceInDetail.Rows[i].Cells["ModelName"].Value = ind.ModelName;
                                grdInvoiceInDetail.Rows[i].Cells["gTaxCode"].Value = ind.TaxCode;
                                grdInvoiceInDetail.Rows[i].Cells["Quantity"].Value = ind.Quantity;
                                grdInvoiceInDetail.Rows[i].Cells["Price"].Value = ind.Price;
                                grdInvoiceInDetail.Rows[i].Cells["Value"].Value = ind.Quantity * ind.Price;
                                grdInvoiceInDetail.Rows[i].Cells["Tax"].Value = ind.Tax;
                                grdInvoiceInDetail.Rows[i].Cells["TaxDetails"].Value = ind.TaxDetails;
                                grdInvoiceInDetail.Rows[i].Cells["ItemRefNo"].Value = ind.ItemReferenceNo;
                                i++;
                            }


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
                if (!verifyAndReworkInvoiceDetailGridRows())
                {
                    MessageBox.Show("Error found in Invoice details. Please correct before updating the details");
                }
                if (columnName == "View")
                {
                    btnUpdatePJV.Visible = true;
                    btnUpdatePJV.Text = "Show PJV";
                }
                else
                {
                    btnUpdatePJV.Text = "Update PJV";
                }
                btnSave.Text = "Update";
                pnlList.Visible = false;
                pnlAddEdit.Visible = true;
                tabControl1.SelectedTab = tabInvoiceInHeader;
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
                DialogResult dialog = MessageBox.Show("Are you sure to clear all entries in grid detail ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdInvoiceInDetail.Rows.Clear();
                    MessageBox.Show("Grid items cleared.");
                    verifyAndReworkInvoiceDetailGridRows();
                }
            }
            catch (Exception)
            {
            }
        }
        private Boolean updateDashBoard(invoiceinheader invoice, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = invoice.DocumentID;
                dsb.TemporaryNo = invoice.TemporaryNo;
                dsb.TemporaryDate = invoice.TemporaryDate;
                dsb.DocumentNo = invoice.DocumentNo;
                dsb.DocumentDate = invoice.DocumentDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = invoice.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(invoice.DocumentID);
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
                InvoiceInHeaderDB inhdb = new InvoiceInHeaderDB();
                FinancialLimitDB flDB = new FinancialLimitDB();
                isValidate = true;
                if (!flDB.checkEmployeeFinancialLimit(docID, Login.empLoggedIn, Convert.ToDouble(txtProductValueINR.Text), 0))
                {
                    MessageBox.Show("No financial power for approving this document");
                    return;
                }
                if (chkPJVApprove.Checked == false)
                {
                    MessageBox.Show("Purchase Journal is not approved. Check For approve purchase journal option.");
                    return;
                }
                if (docID.Equals("WOINVOICEIN"))
                {
                    //List<invoiceinheader> IIHList = InvoiceInHeaderDB.getInvoiceListAgainstOneWO(inh.MRNNo, inh.MRNDate); //MRNno : WOno & MRnDate: WO Date
                    List<invoiceindetail> InvoiceDetail = getInvoiceInDetails(previnvoice);
                    showInvoiceIssuedDetailAgainstWOListView(InvoiceDetail, previnvoice);
                    if (!isValidate)
                    {
                        MessageBox.Show("Enterd Quantity is more than the Wo Qunatity. Not Allowed TO Save/Update.");
                        return;
                    }
                }
                else if (docID.Equals("POGENERALINVOICEIN"))
                {
                    List<invoiceindetail> InvoiceDetail = getInvoiceInDetails(previnvoice);
                    showInvoiceIssuedDetailAgainstPOGenListView(InvoiceDetail, previnvoice);
                    if (!isValidate)
                    {
                        MessageBox.Show("Enterd Quantity is more than the Wo Qunatity. Not Allowed TO Save/Update.");
                        return;
                    }
                }
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    previnvoice.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(previnvoice.CommentStatus);
                    if (previnvoice.status != 96)
                    {
                        previnvoice.DocumentNo = DocumentNumberDB.getNewNumber(docID, 2);
                    }
                    if (inhdb.ApproveInvoiceInHeader(previnvoice))
                    {
                        MessageBox.Show("Invoice Document Approved");
                        //Auto generate purchase PJV
                        //createPJV(2);

                        //-----------------
                        if (!updateDashBoard(previnvoice, 2))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                        }
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredInvoiceInHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                    else
                        MessageBox.Show("Unable to approve");
                }
            }
            catch (Exception)
            {
            }
        }
        public void createPJV(int InvTempNum, DateTime InvTempDate)
        {
            //opt-1:temporary,2:Approved
            PJVHeader pjvh = new PJVHeader();
            PJVDetail pjvd = new PJVDetail();
            //create new pjv temporary number
            pjvh.InvReferenceNo = previnvoice.RowID;
            pjvh.DocumentID = "PJV";


            if (previnvoice.PJVTNo == 0) //JV Not available
            {
                pjvh.TemporaryNo = DocumentNumberDB.getNewNumber("PJV", 1);
                pjvh.TemporaryDate = UpdateTable.getSQLDateTime();
            }
            else
            {
                pjvh.TemporaryNo = previnvoice.PJVTNo;
                pjvh.TemporaryDate = previnvoice.PJVTDate;
            }
            if (previnvoice.PJVNo == 0) //To be updaate at time of approving invoice
            {
                pjvh.JournalNo = 0;
                pjvh.JournalDate = DateTime.Parse("1900-01-01");
            }
            else //If JV approved and Invoice unlocked
            {
                pjvh.JournalNo = previnvoice.PJVNo;
                pjvh.JournalDate = previnvoice.PJVDate;
            }

            //pjvh.TemporaryNo = DocumentNumberDB.getNewNumber("PJV", 1);
            //pjvh.TemporaryDate = UpdateTable.getSQLDateTime();
            //if (opt == 2)
            //{
            //    pjvh.JournalNo = DocumentNumberDB.getNewNumber("PJV", 2);
            //    pjvh.JournalDate = pjvh.TemporaryDate;
            //}
            //else
            //{
            //    pjvh.JournalNo = 0;
            //    pjvh.JournalDate = DateTime.Parse("1900-01-01");
            //}
            pjvh.Narration = "Puchase against Invoice No " + previnvoice.DocumentNo + "," +
                "Dated " + previnvoice.DocumentDate.ToString("dd-MM-yyyy") + "," +
                "Party:" + previnvoice.CustomerName;
            pjvh.CreateUser = Login.userLoggedIn;
            pjvh.CreateTime = pjvh.TemporaryDate;

            pjvh.InvTempNo = InvTempNum;
            pjvh.InvTempDate = InvTempDate;
            pjvh.TaxDetail = getTaxDetails();
            pjvh.InvDocumentID = docID;
            if (previnvoice.CustomerID != null)
                pjvh.Customer = previnvoice.CustomerID;
            else
            {
                invoiceinheader inh = new invoiceinheader();
                inh.DocumentID = docID;
                inh.TemporaryNo = InvTempNum;
                inh.TemporaryDate = InvTempDate;

                pjvh.Customer = InvoiceInHeaderDB.getCustIDOfINvoiceIN(inh);
            }
            pjvh.Amtount = Convert.ToDouble(txtInvoiceValueINR.Text);
            pjvh.TaxAmount = Convert.ToDouble(txtProductTaxINR.Text);
            pjvh.status = 0;
            pjvh.DocumentStatus = 1;
            PJVDB.InsertPJVHeaderAndDetail(pjvh);
        }
        //private Boolean updateStockDetailForMRN()
        //{
        //    Boolean status = false;
        //    StockDB sdb = new StockDB();
        //    mrnheader mrnh = MRNHeaderDB.getMRNNoAndDate(prevmrn.TemporaryNo, prevmrn.TemporaryDate);
        //    prevmrn.MRNNo = mrnh.MRNNo;
        //    prevmrn.MRNDate = mrnh.MRNDate;
        //    mrndetail mrnd = new mrndetail();
        //    List<mrndetail> MRNDetails = new List<mrndetail>();
        //    try
        //    {
        //        for (int i = 0; i < grdInvoiceInDetail.Rows.Count; i++)
        //        {
        //            mrnd = new mrndetail();
        //            mrnd.StockItemID = grdInvoiceInDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Substring(0, grdInvoiceInDetail.Rows[i].Cells["Item"].Value.ToString().Trim().IndexOf('-'));
        //            mrnd.Quantity = Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["Quantity"].Value);
        //            mrnd.Price = Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["Price"].Value);
        //            mrnd.Tax = Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["Tax"].Value);
        //            mrnd.TaxDetails = grdInvoiceInDetail.Rows[i].Cells["TaxDetails"].Value.ToString();
        //            mrnd.BatchNo = grdInvoiceInDetail.Rows[i].Cells["BatchNo"].Value.ToString();
        //            mrnd.SerialNo = grdInvoiceInDetail.Rows[i].Cells["SerielNo"].Value.ToString();
        //            mrnd.ExpiryDate = Convert.ToDateTime(grdInvoiceInDetail.Rows[i].Cells["ExpiryDate"].Value);
        //            mrnd.StoreLocationID = grdInvoiceInDetail.Rows[i].Cells["StoreLocationID"].Value.ToString().Trim().Substring(0, grdInvoiceInDetail.Rows[i].Cells["StoreLocationID"].Value.ToString().Trim().IndexOf('-'));
        //            if (grdInvoiceInDetail.Rows[i].Cells["QuantityAccepted"].Value == null)
        //            {
        //                mrnd.QuantityAccepted = 0;
        //            }
        //            else
        //                mrnd.QuantityAccepted = Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["QuantityAccepted"].Value);
        //            //string s = grdMRNDetail.Rows[i].Cells["QuantityRejected"].Value.ToString();
        //            if (grdInvoiceInDetail.Rows[i].Cells["QuantityRejected"].Value == null)
        //            {
        //                mrnd.QuantityRejected = 0;
        //            }
        //            else
        //                mrnd.QuantityRejected = Convert.ToDouble(grdInvoiceInDetail.Rows[i].Cells["QuantityRejected"].Value);
        //            MRNDetails.Add(mrnd);
        //        }
        //        if (sdb.insertStockFromMRN(MRNDetails, prevmrn))
        //        {
        //            status = true;
        //            MessageBox.Show("StockItem details Updated");
        //        }
        //        else
        //        {
        //            status = false;
        //            MessageBox.Show("Fails to update StockItem details.");
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        status = false;
        //        MessageBox.Show("Fails to update StockItem details.");
        //    }
        //    return status;
        //}
        //private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        customer cust = new customer();
        //        custid = cmbCustomer.SelectedItem.ToString().Trim().Substring(0, cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-'));

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //removeControlsFromForwarderPanelTV();
            //removeControlsPnllvPanel();
            //removeControlsFromModelPanel();
            //removeControlsFromCommenterPanel();
            if (btnSave.Text == "Save")
            {
                if (cmbSelectType.SelectedIndex == -1)
                {
                    tabControl1.SelectedIndex = 0;
                }
            }
            else
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    tabControl1.SelectedIndex = 1;
                }
            }
            try
            {
                if (tabControl1.SelectedTab == tabPDFViewer)
                {
                    int n = pnlPDFViewer.Controls.OfType<Control>().Where(c => c is DataGridView).Count();

                    if (n != 0)
                    {
                        pnlPDFViewer.Focus();
                    }
                    else
                    {
                        pnlPDFViewer.Controls.Clear();
                        DataGridView dgvDocumentList = new DataGridView(); ;
                        dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, previnvoice.TemporaryNo + "-" + previnvoice.TemporaryDate.ToString("yyyyMMddhhmmss"));
                        dgvDocumentList.Size = new Size(870, 300);
                        pnlPDFViewer.Controls.Add(dgvDocumentList);
                        dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
                        if (previnvoice.status == 1 && previnvoice.DocumentStatus == 99)
                        {
                            if (Main.itemPriv[3])
                            {
                                dgvDocumentList.Columns["Edit"].Visible = true;
                                dgvDocumentList.Columns["Delete"].Visible = true;
                            }
                            else
                            {
                                dgvDocumentList.Columns["Edit"].Visible = false;
                                dgvDocumentList.Columns["Delete"].Visible = false;
                            }
                        }
                        dgvDocumentList.ClearSelection();
                        dgvComments.CurrentCell = null;
                        btnCancel.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
            }
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
                //frmPopup.Dispose();
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
                            InvoiceInHeaderDB inhdb = new InvoiceInHeaderDB();
                            previnvoice.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(previnvoice.CommentStatus);
                            previnvoice.ForwardUser = approverUID;
                            previnvoice.ForwarderList = previnvoice.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (inhdb.forwardInvoiceInHeader(previnvoice))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(previnvoice, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredInvoiceInHeader(listOption);
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
                    string s = previnvoice.ForwarderList;
                    string reverseStr = getReverseString(previnvoice.ForwarderList);
                    //do forward activities
                    previnvoice.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(previnvoice.CommentStatus);
                    InvoiceInHeaderDB inhdb = new InvoiceInHeaderDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        previnvoice.ForwarderList = reverseStr.Substring(0, ind);
                        previnvoice.ForwardUser = reverseStr.Substring(ind + 3);
                        previnvoice.DocumentStatus = previnvoice.DocumentStatus - 1;
                    }
                    else
                    {
                        previnvoice.ForwarderList = "";
                        previnvoice.ForwardUser = "";
                        previnvoice.DocumentStatus = 1;
                    }
                    if (inhdb.reverseInvoiceInHeader(previnvoice))
                    {
                        MessageBox.Show("Invoice Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredInvoiceInHeader(listOption);
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
        private void btnListDocuments_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, previnvoice.TemporaryNo + "-" + previnvoice.TemporaryDate.ToString("yyyyMMddhhmmss"));
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
                string colName1 = dgv.Columns[e.ColumnIndex].Name;
                if (e.ColumnIndex == 0)
                {
                    removePDFControls();
                    fileName = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    ////string docDir = Main.documentDirectory + "\\" + docID;
                    string subDir = previnvoice.TemporaryNo + "-" + previnvoice.TemporaryDate.ToString("yyyyMMddhhmmss");
                    ////DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    dgv.Enabled = false;
                    fileName = DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    ////showPDFFile(fileName);
                    ////dgv.Visible = false;
                    System.Diagnostics.Process.Start(fileName);
                    ///System.Threading.Thread.Sleep(10000);
                    dgv.Enabled = true;
                }
                else if (colName1 == "Edit")
                {
                    frmPopup = new Form();
                    frmPopup.StartPosition = FormStartPosition.CenterScreen;
                    frmPopup.BackColor = Color.CadetBlue;
                    frmPopup.MaximizeBox = false;
                    frmPopup.MinimizeBox = false;
                    frmPopup.ControlBox = false;
                    frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;
                    frmPopup.Size = new Size(360, 170);

                    Label head = new Label();
                    head.AutoSize = true;
                    head.Location = new System.Drawing.Point(3, 3);
                    head.Name = "label2";
                    head.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                    head.ForeColor = Color.White;
                    head.Size = new System.Drawing.Size(146, 13);
                    head.Text = "Modify Description";
                    frmPopup.Controls.Add(head);

                    txtDesc = new RichTextBox();
                    txtDesc.Text = dgv.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                    txtDesc.Bounds = new Rectangle(new Point(3, 25), new Size(345, 111));
                    frmPopup.Controls.Add(txtDesc);

                    Button lvOK = new Button();
                    lvOK.Text = "Update";
                    lvOK.BackColor = Color.Tan;
                    lvOK.Location = new System.Drawing.Point(210, 142);
                    lvOK.Size = new System.Drawing.Size(64, 23);
                    lvOK.Cursor = Cursors.Hand;
                    lvOK.Click += (s, t) =>
                    {
                        try
                        {
                            if (String.IsNullOrEmpty(txtDesc.Text.Trim()))
                            {
                                MessageBox.Show("Description is empty");
                                return;
                            }
                            //Update desc here
                            int rowid = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["RowID"].Value);
                            if (DocumentStorageDB.UpdateDocumentDesc(rowid, txtDesc.Text.Trim().Replace("'", "''")))
                            {
                                MessageBox.Show("Updated Sucessfully.");
                                pnlPDFViewer.Controls.Clear();
                                tabControl1.SelectedIndex = 0;
                                tabControl1.SelectedIndex = 4;
                            }
                            frmPopup.Close();
                            frmPopup.Dispose();
                        }
                        catch (Exception ex)
                        {

                        }
                    };
                    frmPopup.Controls.Add(lvOK);

                    Button lvCancel = new Button();
                    lvCancel.Text = "Close";
                    lvCancel.BackColor = Color.Tan;
                    lvCancel.Location = new System.Drawing.Point(273, 142);
                    lvCancel.Size = new System.Drawing.Size(73, 23);
                    lvCancel.Cursor = Cursors.Hand;
                    lvCancel.Click += new System.EventHandler(this.lvCancel_ClickDesc);
                    frmPopup.Controls.Add(lvCancel);

                    frmPopup.ShowDialog();
                }
                if (colName1 == "Delete")
                {
                    int rowid = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["RowID"].Value);
                    DialogResult dialog = MessageBox.Show("Are you sure to to delete the Document?", "Confirmation", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        if (DocumentStorageDB.deleteDocument(rowid))
                        {
                            MessageBox.Show("Deleted Sucessfully.");
                            dgv.Rows.RemoveAt(e.RowIndex);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void lvCancel_ClickDesc(object sender, EventArgs e)
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
                btnUpdatePJV.Visible = false;
                chkPJVApprove.Visible = false;
                chkPJVApprove.Checked = false;
                // btnQC.Visible = false;
                // btnQCCompleted.Visible = false;
                btnGetComments.Visible = false;
                //chkCommentStatus.Visible = false;
                txtComments.Visible = false;
                disableTabPages();
                clearTabPageControls();
                //----24/11/2016
                handleNewButton();
                handleGrdViewButton();
                handleGrdEditButton();
                pnlBottomButtons.Visible = true;
                //----
                if (btnName == "init")
                {
                    ////btnNew.Visible = true; 24/11/2016
                    btnExit.Visible = true;

                }
                else if (btnName == "Commenter")
                {
                    pnlBottomButtons.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    pnlPDFViewer.Visible = true;
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    btnGetComments.Visible = false; //earlier Edit enabled this button
                    ////chkCommentStatus.Visible = true;
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
                    tabControl1.SelectedTab = tabInvoiceInType;
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
                    ///chkCommentStatus.Visible = true;
                    txtComments.Visible = true;
                    //tabControl1.SelectedTab = tabInvoiceInHeader;
                    tabControl1.SelectedTab = tabInvoiceInDetail;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                    chkPJVApprove.Visible = true;
                    btnUpdatePJV.Visible = true;
                    //btnQC.Visible = true;
                    disableTabPages();

                    // tabControl1.SelectedTab = tabInvoiceInHeader;
                    tabControl1.SelectedTab = tabInvoiceInDetail;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    //tabControl1.SelectedTab = tabInvoiceInHeader;
                    tabControl1.SelectedTab = tabInvoiceInDetail;
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
                removeControlsFromForwarderPanel();
                dgvComments.Rows.Clear();
                //chkCommentStatus.Checked = false;
                txtComments.Text = "";
                grdInvoiceInDetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSelectPO_Click(object sender, EventArgs e)
        {

            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;
            custID = "";
            
            if (txtMRNNo.Text.Length != 0)
            {
                DialogResult result = MessageBox.Show(lblHeaderNo.Text + " and " + lblHeaderDate.Text + " will be removed.", "Yes", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    txtMRNNo.Text = "";
                    dtMRNDate.Value = DateTime.Parse("1900-01-01");
                    txtPONos.Text = "";
                    txtPODates.Text = "";
                    payList.Clear();
                    TotalToAdjust = 0;
                    TotalExpense = 0;
                    txtSupplInvoiceNo.Text = "";
                    dtSupplierInvoiceDate.Value = DateTime.Parse("1900-01-01");
                    txtCustomerID.Text = "";
                    txtCustomerName.Text = "";
                    grdInvoiceInDetail.Rows.Clear();
                }
                else
                    return;
            }
            //pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
            if (docID.Equals("WOINVOICEIN"))
            {
                lv = WorkOrderDB.getWOHeaderListView();
                frmPopup.Size = new Size(600, 300);
                lv.Bounds = new Rectangle(new Point(0, 0), new Size(600, 250));
                lv.Columns[4].Width = 340;
                lv.Columns[4].TextAlign = HorizontalAlignment.Left;
            }
            else if (docID.Equals("POINVOICEIN"))
            {
                lv = MRNHeaderDB.getMRNHeaderListView();
                frmPopup.Size = new Size(980, 300);
                lv.Bounds = new Rectangle(new Point(0, 0), new Size(980, 250));
                lv.Columns[8].Width = 300;
                lv.Columns[8].TextAlign = HorizontalAlignment.Left;
            }
            else if (docID.Equals("POGENERALINVOICEIN"))
            {
                lv = PurchaseOrderGeneralDB.getPOHeaderListView();
                frmPopup.Size = new Size(600, 300);
                lv.Bounds = new Rectangle(new Point(0, 0), new Size(600, 250));
                lv.Columns[4].Width = 340;
                lv.Columns[4].TextAlign = HorizontalAlignment.Left;
            }
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Click1);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click1);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
        }
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                if (!checkLVItemChecked("PO"))
                {
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        if (docID.Equals("WOINVOICEIN"))
                        {
                            txtMRNNo.Text = itemRow.SubItems[1].Text;
                            dtMRNDate.Value = Convert.ToDateTime(itemRow.SubItems[2].Text);
                            txtCustomerID.Text = itemRow.SubItems[3].Text;
                            //dtPODate.Value = Convert.ToDateTime(itemRow.SubItems[2].Text);
                            txtCustomerName.Text = itemRow.SubItems[4].Text;
                        }
                        else if (docID.Equals("POINVOICEIN"))
                        {
                            if(Convert.ToInt32(itemRow.SubItems[9].Text) != 0)
                            {
                                MessageBox.Show("Invoice already prepeared against selected MRN. Not allowed to re-create");
                                return;
                            }
                            txtMRNNo.Text = itemRow.SubItems[1].Text;
                            dtMRNDate.Value = Convert.ToDateTime(itemRow.SubItems[2].Text);
                            txtPONos.Text = itemRow.SubItems[3].Text;
                            txtPODates.Text = itemRow.SubItems[4].Text;
                            txtSupplInvoiceNo.Text = itemRow.SubItems[5].Text;
                            dtSupplierInvoiceDate.Value = Convert.ToDateTime(itemRow.SubItems[6].Text);
                            txtCustomerID.Text = itemRow.SubItems[7].Text;
                            txtCustomerName.Text = itemRow.SubItems[8].Text;
                        }
                        else if (docID.Equals("POGENERALINVOICEIN"))
                        {
                            txtMRNNo.Text = itemRow.SubItems[1].Text;
                            dtMRNDate.Value = Convert.ToDateTime(itemRow.SubItems[2].Text);
                            txtCustomerID.Text = itemRow.SubItems[3].Text;
                            //dtPODate.Value = Convert.ToDateTime(itemRow.SubItems[2].Text);
                            txtCustomerName.Text = itemRow.SubItems[4].Text;
                        }
                        frmPopup.Close();
                        frmPopup.Dispose();
                        break;
                    }
                }
                //}
            }
            catch (Exception ex)
            {
            }
        }

        private void lvCancel_Click1(object sender, EventArgs e)
        {
            try
            {
                //btnSelectMRNNo.Enabled = true;
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

        private void txtPONo_TextChanged(object sender, EventArgs e)
        {
            //txtReference.Enabled = false;
            //txtReference.Text = "";
        }
        private void txtDCNo_TextChanged(object sender, EventArgs e)
        {
        }
        private void txtInvoiceNo_TextChanged(object sender, EventArgs e)
        {
        }
        //private void AddGridDetailRowForPO()
        //{
        //    string[] str = chkTemp.Split(';');
        //    List<podetail> PODetail = PurchaseOrderDB.getPOPIDetailForMR(Convert.ToInt32(str[0]), Convert.ToDateTime(str[1]));
        //    if (PODetail.Count > 0)
        //    {
        //        foreach (podetail pod in PODetail)
        //        {
        //            grdInvoiceInDetail.Rows.Add();
        //            grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["LineNo"].Value = grdInvoiceInDetail.RowCount;
        //            DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["Item"]);
        //            grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["Item"].Value = PurchaseOrderDB.fillMRStockItemGridViewComboWithValue(ComboColumn2, PODetail, grdInvoiceInDetail.RowCount);
        //            grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["OrderedQuantity"].Value = pod.Quantity;
        //            grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["Unit"].Value = pod.Unit;
        //            grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["OrderedPrice"].Value = pod.Price;
        //            grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["OrderedTax"].Value = pod.Tax;
        //            grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["Quantity"].Value = "";
        //            grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["Price"].Value = "";
        //            grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["Value"].Value = "";
        //            grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["Tax"].Value = "";
        //            grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["BatchNo"].Value = "";
        //            grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["SerielNo"].Value = "";
        //            grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["ExpiryDate"].Value = "";
        //            DataGridViewComboBoxCell ComboColumn1 = (DataGridViewComboBoxCell)(grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["StoreLocationID"]);
        //            CatalogueValueDB.fillCatalogValueGridViewCombo(ComboColumn1, "StoreLocation");
        //            grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["QuantityAccepted"].Value = 0;
        //            grdInvoiceInDetail.Rows[grdInvoiceInDetail.RowCount - 1].Cells["QuantityRejected"].Value = 0;
        //            grdInvoiceInDetail.Columns["OrderedQuantity"].Visible = true;
        //            grdInvoiceInDetail.Columns["OrderedPrice"].Visible = true;
        //            grdInvoiceInDetail.Columns["OrderedTax"].Visible = true;
        //            grdInvoiceInDetail.Columns["QuantityAccepted"].Visible = false;
        //            grdInvoiceInDetail.Columns["QuantityRejected"].Visible = false;
        //        }
        //    }
        //    else
        //    {
        //        AddPOPIDetailRow();
        //    }
        //}

        //private void cmbCustomer_SelectedValueChanged(object sender, EventArgs e)
        //{
        //    if (track)
        //        return;
        //    try
        //    {
        //        if (txtPONo.Text.Length != 0)
        //        {
        //            string s = cmbCustomer.FindString(custid).ToString();
        //            string ss = cmbCustomer.SelectedIndex.ToString();
        //            if (s == ss)
        //            {
        //                return;
        //            }
        //            DialogResult dialog = MessageBox.Show("Warning:\nPONo , PODate,TaxCode,MRNValue, Product Value and MRN detail will be removed", "", MessageBoxButtons.OKCancel);
        //            if (dialog == DialogResult.OK)
        //            {
        //                s = cmbCustomer.SelectedIndex.ToString();
        //                txtPONo.Text = "";
        //                dtPODate.Text = DateTime.Now.ToString();
        //                cmbTaxCode.SelectedIndex = -1;
        //                txtReference.Enabled = true;
        //                txtMRNValue.Text = "";
        //                txtTaxAmount.Text = "";
        //                txtProductValue.Text = "";
        //                btnPOProdValue.Text = "";
        //                btnPOTaxValue.Text = "";
        //                grdInvoiceInDetail.Rows.Clear();
        //                //track = true;
        //            }
        //            else
        //            {
        //                cmbCustomer.SelectedIndex = cmbCustomer.FindString(custid);
        //                //track = true;
        //                return;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //private void btnQC_Click(object sender, EventArgs e)
        //{
        //    pnllv = new Panel();
        //    pnllv.BorderStyle = BorderStyle.FixedSingle;
        //    btnQC.Enabled = false;
        //    pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
        //    lv = EmployeePostingDB.EmpListViewForQC();
        //    this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked1);
        //    lv.Bounds = new Rectangle(new Point(50, 50), new Size(500, 200));
        //    pnllv.Controls.Add(lv);

        //    Button lvOK = new Button();
        //    lvOK.Text = "OK";
        //    lvOK.Location = new Point(50, 270);
        //    lvOK.Click += new System.EventHandler(this.lvOK_Click2);
        //    pnllv.Controls.Add(lvOK);

        //    Button lvCancel = new Button();
        //    lvCancel.Text = "Cancel";
        //    lvCancel.Location = new Point(150, 270);
        //    lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
        //    pnllv.Controls.Add(lvCancel);

        //    pnlAddEdit.Controls.Add(pnllv);
        //    pnllv.BringToFront();
        //    pnllv.Visible = true;
        //}
        //private void lvOK_Click2(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        foreach (ListViewItem itemRow in lv.Items)
        //        {
        //            if (itemRow.Checked)
        //            {
        //                DialogResult dialog = MessageBox.Show("Are you sure to forward the document ?", "Yes", MessageBoxButtons.YesNo);
        //                if (dialog == DialogResult.Yes)
        //                {
        //                    if (prevmrn.DocumentStatus != 99)
        //                    {
        //                        string approverUID = itemRow.SubItems[3].Text;
        //                        string approverUName = itemRow.SubItems[2].Text;
        //                        MRNHeaderDB mrnhdb = new MRNHeaderDB();
        //                        prevmrn.ForwardUser = approverUID;
        //                        prevmrn.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevmrn.CommentStatus);
        //                        prevmrn.QCStatus = 1;
        //                        prevmrn.ForwarderList = prevmrn.ForwarderList + approverUName + Main.delimiter1 +
        //                            approverUID + Main.delimiter1 + Main.delimiter2;
        //                        if (mrnhdb.forwardMRN(prevmrn))
        //                        {
        //                            pnlCmtr.Visible = false;
        //                            pnlCmtr.Controls.Remove(lvApprover);
        //                            MessageBox.Show("Document Forwarded");
        //                            closeAllPanels();
        //                            ListFilteredMRNHeader(1);
        //                            setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
        //                        }
        //                    }
        //                    else
        //                        MessageBox.Show("Document can not be forward for QC as It already Approved");
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
        //        btnQC.Enabled = true;
        //        pnllv.Visible = false;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //-----
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

        //private void btnQCCompleted_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        DialogResult dialog = MessageBox.Show("Are you sure to send the document back?", "Yes", MessageBoxButtons.YesNo);
        //        if (dialog == DialogResult.Yes)
        //        {
        //            string s = prevmrn.ForwarderList;
        //            string reverseStr = getReverseString(prevmrn.ForwarderList);
        //            //do forward activities
        //            prevmrn.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevmrn.CommentStatus);
        //            MRNHeaderDB mrnhdb = new MRNHeaderDB();
        //            if (reverseStr.Trim().Length > 0)
        //            {
        //                int ind = reverseStr.IndexOf("!@#");
        //                prevmrn.ForwarderList = reverseStr.Substring(0, ind);
        //                prevmrn.ForwardUser = reverseStr.Substring(ind + 3);
        //                prevmrn.DocumentStatus = prevmrn.DocumentStatus - 1;
        //                prevmrn.QCStatus = 99;
        //            }
        //            else
        //            {
        //                prevmrn.ForwarderList = "";
        //                prevmrn.ForwardUser = "";
        //                prevmrn.DocumentStatus = 1;
        //                prevmrn.QCStatus = 99;
        //            }
        //            if (mrnhdb.reverseMRN(prevmrn))
        //            {
        //                MessageBox.Show("MRN Document sent back");
        //                closeAllPanels();
        //                ListFilteredMRNHeader(1);
        //                setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        private void btnCalculate_Click(object sender, EventArgs e)
        {

            if (txtExchangeRate.Text.Trim().Length == 0)
            {
                MessageBox.Show("Fill Exchange Rate");
                return;
            }
            verifyAndReworkInvoiceDetailGridRows();
        }

        private void btnTaxDetails_Click(object sender, EventArgs e)
        {
            try
            {
                string strTax = "";
                for (int i = 0; i < (TaxDetailsTable.Rows.Count); i++)
                {
                    strTax = strTax + Convert.ToString(TaxDetailsTable.Rows[i][0]) + "-" +
                    Convert.ToString(TaxDetailsTable.Rows[i][1]) + "\n";
                }
                DialogResult dialog = MessageBox.Show(strTax, "Tax Details", MessageBoxButtons.OK);
            }
            catch (Exception)
            {
                MessageBox.Show("Error showing tax details");
            }
        }
        public string getTaxDetails()
        {
            string strTax = "";
            try
            {
                for (int i = 0; i < (TaxDetailsTable.Rows.Count); i++)
                {
                    strTax = strTax + Convert.ToString(TaxDetailsTable.Rows[i][0]) + "-" +
                    Convert.ToString(TaxDetailsTable.Rows[i][1]) + "\n";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("getTaxDetails() : Error - " + ex.ToString());
            }
            return strTax;
        }

        int a = 0;
        private void grdMRNDetail_EditingControlShowing_1(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //a = 0;
            //DataGridView dgv = sender as DataGridView;
            //DataGridViewColumn dgvCol = dgv.CurrentCell.OwningColumn;
            //if (dgvCol.Name == "StoreLocationID")
            //{
            //    a = 1;
            //}
            //if (dgvCol.Name == "Item")
            //{
            //    if (e.Control is ComboBox)
            //    {
            //        ComboBox combo = e.Control as ComboBox;
            //        if (combo != null)
            //        {
            //            combo.SelectedIndexChanged -= new EventHandler(ComboBox_SelectedIndexChanged);
            //            combo.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
            //        }
            //    }
            //}
        }
        //private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (a != 1)
        //        {
        //            ComboBox cb = (ComboBox)sender;
        //            string item = cb.Text;
        //            string ItemID = item.Substring(0, item.IndexOf('-'));
        //            string ItemName = item.Substring(item.IndexOf('-') + 1);
        //            if (item != null)
        //            {
        //                grdInvoiceInDetail.Rows[grdInvoiceInDetail.CurrentRow.Index].Cells["Unit"].Value = StockItemDB.getUnitForSelectedStockItem(ItemID, ItemName);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //}

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dtMRNDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtMRNNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabMRNHeader_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        //private void cmbTaxCode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (grdInvoiceInDetail.Rows.Count != 0)
        //    {
        //        string str = cmbTaxCode.SelectedItem.ToString();
        //        if (previnvoice.TaxCode != null && previnvoice.TaxCode != str)
        //        {
        //            DialogResult dialog = MessageBox.Show("Warning:\n Invoice Detail will be Removed", "", MessageBoxButtons.OKCancel);
        //            if (dialog == DialogResult.OK)
        //            {
        //                //grdInvoiceInDetail.Rows.Clear();
        //                ///int n = grdInvoiceInDetail.RowCount;
        //                foreach (DataGridViewRow row in grdInvoiceInDetail.Rows)
        //                {
        //                    grdInvoiceInDetail.Rows[row.Index].Cells["Value"].Value = "";
        //                    grdInvoiceInDetail.Rows[row.Index].Cells["Tax"].Value = "";
        //                }
        //            }
        //            else
        //            {
        //                cmbTaxCode.SelectedIndex = cmbTaxCode.FindString(previnvoice.TaxCode);
        //            }
        //        }
        //    }
        //}

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void cmbSelectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string potype = cmbSelectType.SelectedItem.ToString();
                //.Trim().Substring(0, cmbPOType.SelectedItem.ToString().Trim().IndexOf('-'));
                if (potype == "Work Order")
                {
                    docID = "WOINVOICEIN";
                    //chkDocID = "POPRODUCTINWARD";
                }
                else if (potype == "MRN")
                {
                    docID = "POINVOICEIN";
                    //chkDocID = "POSERVICEINWARD";
                }
                else if (potype == "PO General")
                {
                    docID = "POGENERALINVOICEIN";
                    //chkDocID = "POSERVICEINWARD";
                }
                setDetailGridColumns(docID);
                cmbSelectType.Enabled = false;
            }
            catch (Exception)
            {
            }
        }
        private void setDetailGridColumns(string docId)
        {
            if (docId.Equals("WOINVOICEIN"))
            {
                grdInvoiceInDetail.Columns["ModelNo"].Visible = false;
                grdInvoiceInDetail.Columns["ModelName"].Visible = false;
                lblHeaderNo.Text = "WO No";
                lblHeaderDate.Text = "WO Date";
                txtSupplInvoiceNo.Enabled = true;
                dtSupplierInvoiceDate.Enabled = true;
                btnShowPOMRNDetail.Text = "View WO Det";
            }
            else if (docId.Equals("POINVOICEIN"))
            {
                grdInvoiceInDetail.Columns["ModelNo"].Visible = false;
                grdInvoiceInDetail.Columns["ModelName"].Visible = false;
                lblHeaderNo.Text = "MRN No";
                lblHeaderDate.Text = "MRN Date";
                txtSupplInvoiceNo.Enabled = false;
                dtSupplierInvoiceDate.Enabled = false;

                btnShowPOMRNDetail.Text = "View PO/MRN Det";
            }
            else if (docId.Equals("POGENERALINVOICEIN"))
            {
                grdInvoiceInDetail.Columns["ModelNo"].Visible = false;
                grdInvoiceInDetail.Columns["ModelName"].Visible = false;
                lblHeaderNo.Text = "PO No";
                lblHeaderDate.Text = "PO Date";
                txtSupplInvoiceNo.Enabled = true;
                dtSupplierInvoiceDate.Enabled = true;
                btnShowPOMRNDetail.Text = "View PO Det";
            }
        }

        private void dtSupplierInvoiceDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtExchangeRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdInvoiceInDetail.Rows.Count != 0 && txtInvoiceValue.Text.Length != 0
                    && txtProductValue.Text.Length != 0 && txtExchangeRate.Text.Length != 0)
                {
                    double dd = Convert.ToDouble(txtExchangeRate.Text);
                    txtProductValueINR.Text = (Convert.ToDouble(txtProductValue.Text) * dd).ToString();
                    txtInvoiceValueINR.Text = (Convert.ToDouble(txtInvoiceValue.Text) * dd).ToString();
                    txtProductTaxINR.Text = (Convert.ToDouble(txtProductTax.Text) * dd).ToString();
                }
                if (txtExchangeRate.Text.Length == 0)
                {
                    txtProductValueINR.Text = "";
                    txtInvoiceValueINR.Text = "";
                    txtProductTaxINR.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
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

        private void ShowPOMRNDetail_Click(object sender, EventArgs e)
        {
            if (txtMRNNo.Text.Length == 0 || (docID == "POINVOICEIN" && txtPONos.Text.Length == 0))
            {
                MessageBox.Show("Select MRN/WO Details.");
                return;
            }
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(850, 300);
            if (docID == "POINVOICEIN")
                tab = getTabPOMRN();
            else if (docID == "WOINVOICEIN")
                tab = getTabWorkOrder();
            else
                tab = getTabPurchaseOrderGeneral();
            tab.Bounds = new Rectangle(new Point(0, 0), new Size(850, 270));
            frmPopup.Controls.Add(tab);

            Button lvForwardCancel = new Button();
            lvForwardCancel.BackColor = Color.DeepSkyBlue;
            lvForwardCancel.FlatStyle = FlatStyle.Flat;
            lvForwardCancel.FlatAppearance.BorderColor = Color.Red;
            lvForwardCancel.Text = "CLOSE";
            lvForwardCancel.Location = new Point(10, 272);
            lvForwardCancel.Click += new System.EventHandler(this.lvForwardCancel_Click);
            frmPopup.Controls.Add(lvForwardCancel);
            frmPopup.ShowDialog();
        }
        private TabControl getTabPOMRN()
        {
            TabControl contr = new TabControl();

            try
            {
                TabPage page1 = new TabPage();
                page1.BackColor = Color.Green;
                page1.Location = new System.Drawing.Point(0, 0);
                page1.Name = "tabPO";
                page1.Size = new System.Drawing.Size(850, 270);
                page1.Text = "PO Detail";
                ListView lv1 = getStockDetailForAllSelectedPOs(txtPONos.Text, txtPODates.Text);
                lv1.Bounds = new Rectangle(new Point(0, 0), new Size(848, 242));
                page1.Controls.Add(lv1);

                TabPage page2 = new TabPage();
                page2.BackColor = Color.Green;
                page2.Location = new System.Drawing.Point(0, 0);
                page2.Name = "tabMRN";
                page2.Size = new System.Drawing.Size(850, 270);
                page2.Text = "MRN Detail";
                ListView lv2 = getAvailableStockDetailForAllSelectedPOs(txtPONos.Text, txtPODates.Text);

                lv2.Bounds = new Rectangle(new Point(0, 0), new Size(848, 242));
                page2.Controls.Add(lv2);

                contr.Controls.Add(page1);
                contr.Controls.Add(page2);
            }
            catch (Exception ex)
            {

            }

            return contr;
        }
        private TabControl getTabWorkOrder()
        {
            TabControl contr = new TabControl();

            try
            {


                TabPage page2 = new TabPage();
                page2.BackColor = Color.Green;
                page2.Location = new System.Drawing.Point(0, 0);
                page2.Name = "tabMRN";
                page2.Size = new System.Drawing.Size(850, 270);
                page2.Text = "WorkOrder Detail";
                workorderheader woh = new workorderheader();
                woh.DocumentID = "WORKORDER";
                woh.WONo = Convert.ToInt32(txtMRNNo.Text);
                woh.WODate = dtMRNDate.Value;
                ListView lv2 = WorkOrderDB.getWODetailListView(woh);
                lv2.Columns[0].Width = 0;
                lv2.Bounds = new Rectangle(new Point(0, 0), new Size(848, 242));
                page2.Controls.Add(lv2);

                TabPage page1 = new TabPage();
                page1.BackColor = Color.Green;
                page1.Location = new System.Drawing.Point(0, 0);
                page1.Name = "tabPO";
                page1.Size = new System.Drawing.Size(850, 270);
                page1.Text = "Billed Detail";
                ListView lv1 = getListViewToShowIssuedInvoiceAgainstWO(Convert.ToInt32(txtMRNNo.Text), dtMRNDate.Value);
                lv1.Bounds = new Rectangle(new Point(0, 0), new Size(848, 242));
                page1.Controls.Add(lv1);


                contr.Controls.Add(page2);
                contr.Controls.Add(page1);
            }
            catch (Exception ex)
            {

            }

            return contr;
        }
        private ListView getListViewToShowIssuedInvoiceAgainstWO(int wono, DateTime wodate)
        {
            ListView lvTot = new ListView();
            try
            {

                lvTot.View = System.Windows.Forms.View.Details;
                lvTot.LabelEdit = true;
                lvTot.AllowColumnReorder = true;
                //lvTot.CheckBoxes = true;
                lvTot.FullRowSelect = true;
                lvTot.GridLines = true;
                lvTot.Sorting = System.Windows.Forms.SortOrder.Ascending;
                lvTot.Columns.Add("Doc ID", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Invoice No", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Invoice Date", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("WO No", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("WO Date", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Item ID", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Item Name", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("WO Quant", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Issue Quant", -2, HorizontalAlignment.Center);
                List<invoiceinheader> invInList = InvoiceInHeaderDB.getInvoiceListAgainstOneWO(wono, wodate);

                foreach (invoiceinheader inh in invInList)
                {
                    ListViewItem item1 = new ListViewItem(inh.DocumentID.ToString());
                    item1.SubItems.Add(inh.DocumentNo.ToString());
                    item1.SubItems.Add(inh.DocumentDate.ToString("yyyy-MM-dd"));
                    item1.SubItems.Add(inh.MRNNo.ToString());                     //For WONo
                    item1.SubItems.Add(inh.MRNDate.ToString("yyyy-MM-dd"));  //For WO Date
                    item1.SubItems.Add(inh.CreateUser);   //For Item Code
                    item1.SubItems.Add(inh.CreatorName);  //For Item name
                    double woQuant = WorkOrderDB.getRefNoWiseWODetail(inh.RowID).Quantity; // Row ID of Wo Item in WOHEader Table
                    item1.SubItems.Add(woQuant.ToString());   //WO Quant
                    item1.SubItems.Add(inh.InvoiceValue.ToString()); //For Issued Quantity

                    lvTot.Items.Add(item1);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception in showing PODetail Listview");
            }
            return lvTot;
        }
        private ListView getStockDetailForAllSelectedPOs(string POnos, string PODates)
        {
            ListView lvTot = new ListView();
            try
            {

                lvTot.View = System.Windows.Forms.View.Details;
                lvTot.LabelEdit = true;
                lvTot.AllowColumnReorder = true;
                //lvTot.CheckBoxes = true;
                lvTot.FullRowSelect = true;
                lvTot.GridLines = true;
                lvTot.Sorting = System.Windows.Forms.SortOrder.Ascending;
                lvTot.Columns.Add("PO No", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("PO Date", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("StockItem ID", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("StockItem Name", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Model No", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Model Name", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Quantity", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Price", -2, HorizontalAlignment.Center);
                lvTot.Columns[5].Width = 150;
                lvTot.Columns[3].Width = 280;
                string[] poNoArr = POnos.Split(';');
                string[] poDatesArr = PODates.Split(';');
                for (int i = 1; i < poNoArr.Length - 1; i++)
                {
                    ListView lv1 =
                        PurchaseOrderDB.getPODetailForInvoiceIN(Convert.ToInt32(poNoArr[i]), Convert.ToDateTime(poDatesArr[i]));
                    lvTot.Items.AddRange((from ListViewItem item in lv1.Items
                                          select (ListViewItem)item.Clone()).ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception in showing PODetail Listview");
            }
            return lvTot;
        }
        private ListView getAvailableStockDetailForAllSelectedPOs(string POnos, string PODates)
        {
            ListView lvTot = new ListView();
            try
            {

                lvTot.View = System.Windows.Forms.View.Details;
                lvTot.LabelEdit = true;
                lvTot.AllowColumnReorder = true;
                //lvTot.CheckBoxes = true;
                lvTot.FullRowSelect = true;
                lvTot.GridLines = true;
                lvTot.Sorting = System.Windows.Forms.SortOrder.Ascending;
                lvTot.Columns.Add("MRN No", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("MRN Date", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("PO No", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("PO Date", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("StockItem ID", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("StockItem Name", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Model No", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Model Name", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Quant", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Acpt Quan", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Rej Quan", -2, HorizontalAlignment.Center);
                lvTot.Columns[1].Width = 0;
                lvTot.Columns[6].Width = 0;
                lvTot.Columns[3].Width = 0;
                lvTot.Columns[5].Width = 250;
                lvTot.Columns[7].Width = 100;
                string[] poNoArr = POnos.Split(';');
                string[] poDatesArr = PODates.Split(';');
                for (int i = 1; i < poNoArr.Length - 1; i++)
                {
                    ListView lv1 =
                        MRNHeaderDB.getStockDetailsForOnePOListView(Convert.ToInt32(poNoArr[i]), Convert.ToDateTime(poDatesArr[i]));
                    lvTot.Items.AddRange((from ListViewItem item in lv1.Items
                                          select (ListViewItem)item.Clone()).ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception in showing PODetail Listview");
            }
            return lvTot;
        }

        private void showItemWiseTotalQuantityForSelectedMRN(List<invoiceindetail> iid, List<mrndetail> MRNDetails)
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

                frmPopup.Size = new Size(750, 310);

                Label lbl = new Label();
                lbl.AutoSize = true;
                lbl.Location = new Point(5, 5);
                lbl.Size = new Size(300, 20);
                lbl.Text = "Item Wise Total Quantity IN MRN";
                lbl.Font = new Font("Serif", 10, FontStyle.Bold);
                lbl.ForeColor = Color.Green;
                frmPopup.Controls.Add(lbl);

                lv = getItemWiseTotalQuantityListView(iid, MRNDetails);
                lv.Bounds = new Rectangle(new Point(0, 25), new Size(750, 250));
                frmPopup.Controls.Add(lv);

                Button lvCancel = new Button();
                lvCancel.BackColor = Color.Tan;
                lvCancel.Text = "CLOSE";
                lvCancel.Location = new Point(20, 280);
                lvCancel.Click += new System.EventHandler(this.lvCancel_Click3);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {

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
        private ListView getItemWiseTotalQuantityListView(List<invoiceindetail> iid, List<mrndetail> MRNDetails)
        {
            ListView lv = new ListView();
            try
            {
                lv.View = System.Windows.Forms.View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                lv.Columns.Add("StockItem ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItem Name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("MOdel No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("MOdel Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Tot Quant(MRN)", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Invoice Quant", -2, HorizontalAlignment.Center);

                foreach (mrndetail mrnd in MRNDetails)
                {
                    ListViewItem item1 = new ListViewItem(mrnd.StockItemID);
                    item1.SubItems.Add(mrnd.StockItemName);
                    item1.SubItems.Add(mrnd.ModelNo.ToString());
                    item1.SubItems.Add(mrnd.ModelName);
                    item1.SubItems.Add(mrnd.Quantity.ToString());

                    int count = iid.Where(track => track.StockItemID == mrnd.StockItemID && track.ModelNo == mrnd.ModelNo).Count();
                    if (count == 1)
                    {
                        invoiceindetail AvailIID = iid.SingleOrDefault(track => track.StockItemID == mrnd.StockItemID && track.ModelNo == mrnd.ModelNo);
                        if (AvailIID.Quantity != mrnd.Quantity)
                        {
                            item1.BackColor = Color.Magenta;  //For Quantity Not Equal to MRNQuantity
                            isValidate = false;
                        }
                        item1.SubItems.Add(AvailIID.Quantity.ToString());
                    }
                    else if (count > 1)
                    {
                        List<invoiceindetail> InvInAvailList = iid.Where(track => track.StockItemID == mrnd.StockItemID && track.ModelNo == mrnd.ModelNo).ToList();
                        double sumOfBilledQuant = InvInAvailList.Sum(inv => inv.Quantity);
                        if (sumOfBilledQuant != mrnd.Quantity)
                        {
                            item1.BackColor = Color.Magenta;  //For Quantity Not Equal to MRNQuantity
                            isValidate = false;
                        }
                        item1.SubItems.Add(sumOfBilledQuant.ToString());
                    }
                    else
                    {
                        item1.BackColor = Color.Tan; //For Not Available in INvoiceIn Detail
                        isValidate = false;
                    }

                    //if (AvailIID != null)
                    //{
                    //    if (AvailIID.Quantity != mrnd.Quantity)
                    //        item1.BackColor = Color.Magenta;  //For Quantity Not Equal to MRNQuantity
                    //    item1.SubItems.Add(AvailIID.Quantity.ToString());
                    //}
                    //else
                    //    item1.BackColor = Color.Tan; //For Not Available in INvoiceIn Detail

                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }


        //-- Validating Header and Detail String For Single Quotes

        private invoiceinheader verifyHeaderInputString(invoiceinheader iih)
        {
            try
            {
                iih.Remarks = Utilities.replaceSingleQuoteWithDoubleSingleQuote(iih.Remarks);
                iih.CustomerID = Utilities.replaceSingleQuoteWithDoubleSingleQuote(iih.CustomerID);
                iih.AdvancePaymentVouchers = Utilities.replaceSingleQuoteWithDoubleSingleQuote(iih.AdvancePaymentVouchers);
            }
            catch (Exception ex)
            {
            }
            return iih;
        }
        private void verifyDetailInputString()
        {
            try
            {
                for (int i = 0; i < grdInvoiceInDetail.Rows.Count; i++)
                {
                    grdInvoiceInDetail.Rows[i].Cells["Item"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdInvoiceInDetail.Rows[i].Cells["Item"].Value.ToString());
                    grdInvoiceInDetail.Rows[i].Cells["ModelNo"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdInvoiceInDetail.Rows[i].Cells["ModelNo"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }
        private TabControl getTabPurchaseOrderGeneral()
        {
            TabControl contr = new TabControl();

            try
            {


                TabPage page2 = new TabPage();
                page2.BackColor = Color.Green;
                page2.Location = new System.Drawing.Point(0, 0);
                page2.Name = "tabMRN";
                page2.Size = new System.Drawing.Size(850, 270);
                page2.Text = "PO General Detail";
                pogeneralheader poh = new pogeneralheader();
                poh.DocumentID = "POGENERAL";
                poh.PONo = Convert.ToInt32(txtMRNNo.Text);
                poh.PODate = dtMRNDate.Value;
                ListView lv2 = PurchaseOrderGeneralDB.getPODetailListView(poh);
                lv2.Columns[0].Width = 0;
                lv2.Bounds = new Rectangle(new Point(0, 0), new Size(848, 242));
                page2.Controls.Add(lv2);

                TabPage page1 = new TabPage();
                page1.BackColor = Color.Green;
                page1.Location = new System.Drawing.Point(0, 0);
                page1.Name = "tabPO";
                page1.Size = new System.Drawing.Size(850, 270);
                page1.Text = "Billed Detail";
                ListView lv1 = getListViewToShowIssuedInvoiceAgainstPOGeneral(Convert.ToInt32(txtMRNNo.Text), dtMRNDate.Value);
                lv1.Bounds = new Rectangle(new Point(0, 0), new Size(848, 242));
                page1.Controls.Add(lv1);


                contr.Controls.Add(page2);
                contr.Controls.Add(page1);
            }
            catch (Exception ex)
            {

            }

            return contr;
        }
        private ListView getListViewToShowIssuedInvoiceAgainstPOGeneral(int pono, DateTime podate)
        {
            ListView lvTot = new ListView();
            try
            {

                lvTot.View = System.Windows.Forms.View.Details;
                lvTot.LabelEdit = true;
                lvTot.AllowColumnReorder = true;
                //lvTot.CheckBoxes = true;
                lvTot.FullRowSelect = true;
                lvTot.GridLines = true;
                lvTot.Sorting = System.Windows.Forms.SortOrder.Ascending;
                lvTot.Columns.Add("Doc ID", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Invoice No", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Invoice Date", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("PO No", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("PO Date", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Item ID", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Item Name", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("PO Quant", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Issue Quant", -2, HorizontalAlignment.Center);
                List<invoiceinheader> invInList = PurchaseOrderGeneralDB.getInvoiceListAgainstOnePOGen(pono, podate);

                foreach (invoiceinheader inh in invInList)
                {
                    ListViewItem item1 = new ListViewItem(inh.DocumentID.ToString());
                    item1.SubItems.Add(inh.DocumentNo.ToString());
                    item1.SubItems.Add(inh.DocumentDate.ToString("yyyy-MM-dd"));
                    item1.SubItems.Add(inh.MRNNo.ToString());                     //For WONo
                    item1.SubItems.Add(inh.MRNDate.ToString("yyyy-MM-dd"));  //For WO Date
                    item1.SubItems.Add(inh.CreateUser);   //For Item Code
                    item1.SubItems.Add(inh.CreatorName);  //For Item name
                    double poQuant = PurchaseOrderGeneralDB.getRefNoWiseQuantINPOGen(inh.RowID); // Row ID of Po Item in POHEader Table
                    item1.SubItems.Add(poQuant.ToString());   //PO Quant
                    item1.SubItems.Add(inh.InvoiceValue.ToString()); //For Issued Quantity

                    lvTot.Items.Add(item1);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception in showing PODetail Listview");
            }
            return lvTot;
        }
        private void showInvoiceIssuedDetailAgainstPOGenListView(List<invoiceindetail> InDList, invoiceinheader inh)
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

                frmPopup.Size = new Size(800, 300);
                lv = getInvoicePreparedDetailListViewWRTPOGen(InDList, inh);
                lv.Bounds = new Rectangle(new Point(0, 0), new Size(800, 250));
                frmPopup.Controls.Add(lv);

                Button lvCancel = new Button();
                lvCancel.BackColor = Color.Tan;
                lvCancel.Text = "CLOSE";
                lvCancel.Location = new Point(20, 265);
                lvCancel.Click += new System.EventHandler(this.lvCancel_Click3);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {

            }
        }
        private ListView getInvoicePreparedDetailListViewWRTPOGen(List<invoiceindetail> IinHList, invoiceinheader inh)
        {
            ListView lv = new ListView();
            try
            {
                //return;
                lv.View = System.Windows.Forms.View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                //lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;

                lv.Columns.Add("PO No", -2, HorizontalAlignment.Center);
                lv.Columns.Add("PO Date", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Item ID", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Item Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("PO Quant", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Issued Quant(A)", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Present Quant(B)", -2, HorizontalAlignment.Center);
                lv.Columns.Add("A + B", -2, HorizontalAlignment.Center);
                foreach (invoiceindetail iid in IinHList)
                {
                    double ItemIssueQuant = PurchaseOrderGeneralDB.getItemWiseTotalQuantOFPOIssuedInvoiceIn(iid.ItemReferenceNo);
                    double poQuant = PurchaseOrderGeneralDB.getRefNoWiseQuantINPOGen(iid.ItemReferenceNo);
                    ListViewItem item1 = new ListViewItem(inh.MRNNo.ToString());
                    item1.SubItems.Add(inh.MRNDate.ToShortDateString());
                    item1.SubItems.Add(iid.StockItemID);
                    item1.SubItems.Add(iid.StockItemName);
                    item1.SubItems.Add(poQuant.ToString());          //For WO Quantity
                    item1.SubItems.Add(ItemIssueQuant.ToString());  // Total Issued Quant in all Invoice against perticular WO Item
                    item1.SubItems.Add(iid.Quantity.ToString());    //For Enter Quantity IN Current INVOICEIn
                    item1.SubItems.Add((ItemIssueQuant + iid.Quantity).ToString()); //Issue quant + written quant
                    if ((ItemIssueQuant + iid.Quantity) > poQuant)
                    {
                        isValidate = false;
                        item1.BackColor = Color.Magenta;
                    }
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }

        private void chkPVHApprove_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdatePJV_Click(object sender, EventArgs e)
        {
            try
            {
                string txt = btnUpdatePJV.Text;
                EditJV showJv;
                invoiceinheader iih = new invoiceinheader();
                string invstr = previnvoice.DocumentID + ";" + previnvoice.TemporaryNo + ";" + previnvoice.TemporaryDate;
                if (txt == "Update PJV")
                {

                    showJv = new EditJV("PJV", invstr, previnvoice.InvoiceValueINR, true);
                }
                else
                {
                    showJv = new EditJV("PJV", invstr, previnvoice.InvoiceValueINR, false);
                }
                showJv.Text = "PJV";
                showJv.ShowDialog();
                this.RemoveOwnedForm(showJv);
            }
            catch (Exception ex)
            {
            }
        }

        private void InvoiceInHeader_Enter(object sender, EventArgs e)
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

        private void btnSelAdvPayments_Click(object sender, EventArgs e)
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

                frmPopup.Size = new Size(1100, 370);
                string InvDocID = docID;
                int InvTempNo = Convert.ToInt32(txtTemporarryNo.Text);
                DateTime InvTempDate = Convert.ToDateTime(dtTempDate.Value);
                voucherGrid = InvoiceInHeaderDB.getGridViewOFPVForAdvAdjustment(txtCustomerID.Text.Trim());
                voucherGrid.Bounds = new Rectangle(new Point(0, 27), new Size(1100, 300));
                List<invoiceinpayments> PayListAvail = InvoiceInHeaderDB.getInvoiceInAdvPaymentDetails(InvTempNo, InvTempDate, InvDocID);//If invoice paid against PV
                List<int> RowIndexList = new List<int>();

                foreach (DataGridViewRow row in voucherGrid.Rows)
                {
                    int PVRNo = Convert.ToInt32(row.Cells["VoucherNO"].Value);
                    DateTime PVRDate = Convert.ToDateTime(row.Cells["VoucherDate"].Value);
                    string PVDocID = row.Cells["VoucherID"].Value.ToString();
                    decimal amtAdjusted = Convert.ToDecimal(row.Cells["AmountAdjusted"].Value);
                    decimal TDSAdjusted = Convert.ToDecimal(row.Cells["TDSAdjusted"].Value);
                    decimal VoucherAmnt = Convert.ToDecimal(row.Cells["VoucherAmt"].Value);

                    invoiceinpayments invPayment = payList.FirstOrDefault(pay => pay.PVDocumentID == PVDocID && pay.PVNo == PVRNo && pay.PVDate == PVRDate);
                    invoiceinpayments invPaymentAvail = new invoiceinpayments();
                    if (invPayment != null)
                    {
                        invPaymentAvail = PayListAvail.FirstOrDefault(pay => pay.PVDocumentID == PVDocID && pay.PVNo == invPayment.PVNo && pay.PVDate == invPayment.PVDate);
                    }

                    if (invPayment != null && invPaymentAvail != null)
                    {
                        row.Cells["AmountAdjusted"].Value = amtAdjusted - invPaymentAvail.Amount;
                        row.Cells["AmountToAdjust"].Value = invPayment.Amount;
                    }
                    else if (invPayment != null && invPaymentAvail == null)
                    {
                        row.Cells["AmountAdjusted"].Value = amtAdjusted;
                        row.Cells["AmountToAdjust"].Value = invPayment.Amount;
                    }
                    else if (VoucherAmnt <= amtAdjusted) //If full invoice amount is adjusted
                    {
                        //RowIndexList.Add(row.Index); //To be remove after looping
                        row.Visible = false;
                    }

                    //TDS Calculation

                    if (invPayment != null && invPaymentAvail != null)
                    {
                        row.Cells["TDSAdjusted"].Value = TDSAdjusted - invPaymentAvail.TDSAmount;
                        row.Cells["TDSToAdjust"].Value = invPayment.TDSAmount;
                    }
                    else if (invPayment != null && invPaymentAvail == null)
                    {
                        row.Cells["TDSAdjusted"].Value = TDSAdjusted;
                        row.Cells["TDSToAdjust"].Value = invPayment.TDSAmount;
                    }

                }
                voucherGrid.Columns["AmountToAdjust"].DefaultCellStyle.BackColor = Color.MistyRose;
                voucherGrid.Columns["TDSToAdjust"].DefaultCellStyle.BackColor = Color.MistyRose;
                frmPopup.Controls.Add(voucherGrid);

                Label lblcol = new Label();
                lblcol.BackColor = Color.MistyRose;
                lblcol.Size = new Size(30, 10);
                lblcol.Location = new System.Drawing.Point(700, 10);
                frmPopup.Controls.Add(lblcol);

                Label lblcoval = new Label();
                lblcoval.Text = "Requiered fields";
                lblcoval.Location = new System.Drawing.Point(730, 8);
                frmPopup.Controls.Add(lblcoval);

                Button grdOK = new Button();
                grdOK.BackColor = Color.Tan;
                grdOK.Text = "OK";
                grdOK.Location = new System.Drawing.Point(20, 335);
                grdOK.Click += new System.EventHandler(this.grdOK_ClickPV);
                frmPopup.Controls.Add(grdOK);

                Button grdCancel = new Button();
                grdCancel.Text = "CANCEL";
                grdCancel.BackColor = Color.Tan;
                grdCancel.Location = new System.Drawing.Point(110, 335);
                grdCancel.Click += new System.EventHandler(this.grdCancel_ClickPV);
                frmPopup.Controls.Add(grdCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdOK_ClickPV(object sender, EventArgs e)
        {
            //string InivDetails = "";
            try
            {
                var checkedRows = from DataGridViewRow r in voucherGrid.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                if (checkedRows.Count() == 0)
                {
                    MessageBox.Show("Select one Voucher.");
                    return;
                }
                TotalToAdjust = 0;
                foreach (var row in checkedRows)
                {
                    decimal pvAmnt = Convert.ToDecimal(row.Cells["VoucherAmt"].Value);
                    decimal AdjAmnt = Convert.ToDecimal(row.Cells["AmountAdjusted"].Value);
                    decimal ToAdjAmnt = Convert.ToDecimal(row.Cells["AmountToAdjust"].Value);

                    decimal TDSAmnt = Convert.ToDecimal(row.Cells["TDSAdjusted"].Value);
                    decimal ToTDSAmnt = Convert.ToDecimal(row.Cells["TDSToAdjust"].Value);

                    if (ToAdjAmnt == 0 || (AdjAmnt + ToAdjAmnt) > pvAmnt)
                    {
                        MessageBox.Show("Row Selection Error. Please check Amount to Adjust.");
                        TotalToAdjust = 0;
                        return;
                    }
                    if (ToTDSAmnt > ToAdjAmnt)
                    {
                        MessageBox.Show("TDS amount should not be more than Amount to be adjusted.");
                        return;
                    }
                    TotalToAdjust = TotalToAdjust + ToAdjAmnt;
                }

                invoiceinpayments pay = new invoiceinpayments();
                payList.Clear();
                foreach (var row in checkedRows)
                {
                    //InivDetails = InivDetails + row.Cells["DocID"].Value.ToString() + Main.delimiter1 + row.Cells["DocNO"].Value.ToString() +
                    //                            Main.delimiter1 + Convert.ToDateTime(row.Cells["DocDate"].Value).ToString("yyyy-MM-dd") + Main.delimiter2;
                    pay = new invoiceinpayments();
                    pay.PVDocumentID = row.Cells["VoucherID"].Value.ToString();
                    pay.PVNo = Convert.ToInt32(row.Cells["VoucherNO"].Value);
                    pay.PVDate = Convert.ToDateTime(row.Cells["VoucherDate"].Value);
                    pay.PVTemporaryNo = Convert.ToInt32(row.Cells["VoucherTempNo"].Value);
                    pay.PVTemporaryDate = Convert.ToDateTime(row.Cells["VoucherTempDate"].Value);
                    pay.CustomerID = txtCustomerID.Text.Trim();
                    pay.Amount = Convert.ToDecimal(row.Cells["AmountToAdjust"].Value);
                    pay.TDSAmount = Convert.ToDecimal(row.Cells["TDSToAdjust"].Value);
                    payList.Add(pay);
                }
                lblAdvTotal.Text = TotalToAdjust.ToString();
                //txtBillDetails.Text = InivDetails;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        private void grdCancel_ClickPV(object sender, EventArgs e)
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

        private void btnExpenseDetails_Click(object sender, EventArgs e)
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

                frmPopup.Size = new Size(450, 370);
                invoiceinheader iih = new invoiceinheader();
                iih.DocumentID = docID;
                iih.TemporaryNo = Convert.ToInt32(txtTemporarryNo.Text);
                iih.TemporaryDate = Convert.ToDateTime(dtTempDate.Value);
                expenseGrid = InvoiceInHeaderDB.getGridViewOFInvoiceExpense();
                expenseGrid.Bounds = new Rectangle(new Point(0, 27), new Size(450, 300));
                //expList = InvoiceInHeaderDB.getExpenseDetialForInvoiceIN(iih);
                if (expList.Count != 0)
                {
                    foreach (DataGridViewRow row in expenseGrid.Rows)
                    {
                        int expID = Convert.ToInt32(row.Cells["ExpenseID"].Value);
                        invoiceinexpense invExp = expList.FirstOrDefault(pay => pay.ExpenseID == expID);
                        if (invExp != null)
                        {
                            row.Cells["Amount"].Value = invExp.Amount;
                        }
                    }
                }
                frmPopup.Controls.Add(expenseGrid);

                Button grdOK = new Button();
                grdOK.BackColor = Color.Tan;
                grdOK.Text = "OK";
                grdOK.Location = new System.Drawing.Point(20, 335);
                grdOK.Click += new System.EventHandler(this.grdOK_ClickExp);
                frmPopup.Controls.Add(grdOK);

                Button grdCancel = new Button();
                grdCancel.Text = "CANCEL";
                grdCancel.BackColor = Color.Tan;
                grdCancel.Location = new System.Drawing.Point(110, 335);
                grdCancel.Click += new System.EventHandler(this.grdCancel_ClickExp);
                frmPopup.Controls.Add(grdCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        private void grdOK_ClickExp(object sender, EventArgs e)
        {
            try
            {
                var checkedRows = from DataGridViewRow r in expenseGrid.Rows
                                      where Convert.ToDecimal(r.Cells["Amount"].Value) != 0
                                      select r;
                
                if (checkedRows.Count() == 0)
                {
                    MessageBox.Show("Select one Expense List.");
                    return;
                }
                TotalExpense = 0;
                foreach (var row in checkedRows)
                {
                    decimal expAMnt = Convert.ToDecimal(row.Cells["Amount"].Value);
                    
                    if (expAMnt == 0 )
                    {
                        MessageBox.Show("Row Selection Error. Please check Expense amount");
                        TotalExpense = 0;
                        return;
                    }
                    TotalExpense = TotalExpense + expAMnt;
                }

                invoiceinexpense exp = new invoiceinexpense();
                expList.Clear();
                foreach (var row in checkedRows)
                {
                    exp = new invoiceinexpense();
                    exp.ExpenseID = Convert.ToInt32(row.Cells["ExpenseID"].Value);
                    exp.Amount = Convert.ToDecimal(row.Cells["Amount"].Value);
                    expList.Add(exp);
                }
                lblExpTotal.Text = TotalExpense.ToString();
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("Check Grid Rows Amount column");
            }
        }
        private void grdCancel_ClickExp(object sender, EventArgs e)
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

        private void cmbCurrencyID_SelectedIndexChanged(object sender, EventArgs e)
        {
           try
            {
                string CurID = ((Structures.ComboBoxItem)cmbCurrencyID.SelectedItem).HiddenValue;
                if(CurID == "INR")
                {
                    txtExchangeRate.Text = "1";
                    txtExchangeRate.Enabled = false;
                }
                else
                {
                    txtExchangeRate.Text = "";
                    txtExchangeRate.Enabled = true;
                }
            }
            catch(Exception ex)
            {
                txtExchangeRate.Text = "";
                txtExchangeRate.Enabled = true;
            }
        }
    }
}



