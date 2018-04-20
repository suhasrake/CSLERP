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

namespace CSLERP
{
    public partial class MRNHeader : System.Windows.Forms.Form
    {
        Boolean track = false;
        //int no = 0;
        int yy = 0;
        string docID = "MRN";
        Boolean isValidate = true;
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Boolean userIsACommenter = false;
        string chkTemp = "";
        double productvalue = 0.0;
        double taxvalue = 0.0;
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        mrnheader prevmrn;
        Form dtpForm = new Form();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        RichTextBox txtDesc = new RichTextBox();
        Form frmPopup = new Form();
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        string custid = "";
        Timer filterTimer1 = new Timer();
        CheckedListBox chkBoxCustomer = new CheckedListBox();
        Boolean approveCellClick = true;
        TextBox txtSearchCust = new TextBox();
        //DateTimePicker dtp;
        Timer filterTimer = new Timer();
        string colName = "";
        DataGridView grdCustList = new DataGridView();
        List<mrndetail> mrnListQuantCheck = new List<mrndetail>();
        Boolean isViewMode = false;
        Boolean AddRowClick = false;
        public MRNHeader()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }

        private void MRNHeader_Load(object sender, EventArgs e)
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
            ListFilteredMRNHeader(listOption);
            //tabControl1.SelectedIndexChanged += new EventHandler(TabControl1_SelectedIndexChanged);
        }
        private void ListFilteredMRNHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                MRNHeaderDB mihdb = new MRNHeaderDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);

                List<mrnheader> MRNHeaders = mihdb.getFilteredMRNHeader(userString, option, userCommentStatusString);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (mrnheader mrnh in MRNHeaders)
                {
                    if (option == 1)
                    {
                        if (mrnh.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = mrnh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = mrnh.DocumentName;

                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = mrnh.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = mrnh.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gMRNNo"].Value = mrnh.MRNNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gMRNDate"].Value = mrnh.MRNDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerID"].Value = mrnh.CustomerID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerName"].Value = mrnh.CustomerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPONo"].Value = mrnh.PONOs;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPODate"].Value = mrnh.PODates;
                    grdList.Rows[grdList.RowCount - 1].Cells["Reference"].Value = mrnh.Reference;
                    grdList.Rows[grdList.RowCount - 1].Cells["DCNo"].Value = mrnh.DCNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["DCDate"].Value = mrnh.DCDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["InvoiceNo"].Value = mrnh.InvoiceNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["InvoiceDate"].Value = mrnh.InvoiceDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["TransportationMode"].Value = mrnh.TransportationMode;
                    grdList.Rows[grdList.RowCount - 1].Cells["TransporterType"].Value = mrnh.TransporterType;
                    grdList.Rows[grdList.RowCount - 1].Cells["TransporterName"].Value = mrnh.TransporterName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProductValue"].Value = mrnh.ProductValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["TaxAmount"].Value = mrnh.TaxAmount;
                    grdList.Rows[grdList.RowCount - 1].Cells["MRNValue"].Value = mrnh.MRNValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProductValueINR"].Value = mrnh.ProductValueINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["TaxAmountINR"].Value = mrnh.TaxAmountINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["MRNValueINR"].Value = mrnh.MRNValueINR;

                    grdList.Rows[grdList.RowCount - 1].Cells["gRemarks"].Value = mrnh.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = mrnh.status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = mrnh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["QCStatus"].Value = mrnh.QCStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = mrnh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = mrnh.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = mrnh.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = mrnh.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatusNo"].Value = ComboFIll.getDocumentStatusString(mrnh.DocumentStatus);
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = mrnh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatusNo"].Value = mrnh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CmtStatus"].Value = mrnh.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = mrnh.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = mrnh.ForwarderList;

                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyID"].Value = mrnh.CurrencyID;
                    grdList.Rows[grdList.RowCount - 1].Cells["ExchangeRate"].Value = mrnh.ExchangeRate;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in MRN Listing");
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
            CurrencyDB.fillCurrencyComboNew(cmbCurrency);
            CatalogueValueDB.fillCatalogValueComboNew(cmbTransporationMode, "TransportationMode");
            CatalogueValueDB.fillCatalogValueComboNew(cmbTransporationType, "TransporterType");
            //CustomerDB.fillCustomerComboNew(cmbCustomer);
            //TaxCodeDB.fillTaxCodeCombo(cmbTaxCode);

            dtTempDate.Format = DateTimePickerFormat.Custom;
            dtTempDate.CustomFormat = "dd-MM-yyyy";
            dtTempDate.Enabled = false;
            dtMRNDate.Format = DateTimePickerFormat.Custom;
            dtMRNDate.CustomFormat = "dd-MM-yyyy";
            dtMRNDate.Enabled = false;
            //dtPODate.Format = DateTimePickerFormat.Custom;
            //dtPODate.CustomFormat = "dd-MM-yyyy";

            dtDCDate.Format = DateTimePickerFormat.Custom;
            dtDCDate.CustomFormat = "dd-MM-yyyy";
            dtInvoiceDate.Format = DateTimePickerFormat.Custom;
            dtInvoiceDate.CustomFormat = "dd-MM-yyyy";
            //txtPaymentTerms.Enabled = false;
            txtTemporarryNo.Enabled = false;
            dtTempDate.Enabled = false;
            txtMRNNo.Enabled = false;
            dtMRNDate.Enabled = false;
            btnForward.Visible = false;
            btnApprove.Visible = false;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            grdMRNDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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
            txtMRNNo.TabIndex = 2;
            dtMRNDate.TabIndex = 3;
            btnSelCustomer.TabIndex = 4;
            txtPONos.TabIndex = 5;
            btnSelectPO.TabIndex = 6;
            txtPODates.TabIndex = 7;
            cmbTransporationMode.TabIndex = 8;
            cmbTransporationType.TabIndex = 9;
            txtTransporterName.TabIndex = 10;
            txtReference.TabIndex = 11;
            txtDCNo.TabIndex = 12;
            dtDCDate.TabIndex = 13;
            txtInvoiceNo.TabIndex = 14;
            dtInvoiceDate.TabIndex = 15;
            cmbCurrency.TabIndex = 16;
            txtINRInversionRate.TabIndex = 17;
            txtRemarks.TabIndex = 18;

            btnForward.TabIndex = 0;
            btnApprove.TabIndex = 1;
            btnCancel.TabIndex = 2;
            btnSave.TabIndex = 3;
            btnReverse.TabIndex = 4;
            btnQC.TabIndex = 5;
            btnQCCompleted.TabIndex = 6;
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
                track = true;
                //clear all grid views
                grdMRNDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                isValidate = true;
                btnPOProdValue.Visible = false;
                btnPOTaxValue.Visible = false;
                lblPOpProdValue.Visible = false;
                lblPOTaxValue.Visible = false;
                enableMRNDetailGridColumns();
                EnableQCApprovedGrdDetailsVisibility();
                //----------clear temperory panels

                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                pnllv.Visible = false;
                btnQC.Enabled = true;
                //----------
                cmbTransporationType.SelectedIndex = -1;
                cmbTransporationMode.SelectedIndex = -1;
                txtCustName.Text = "";
                txtCustomerID.Text = "";
                //cmbTaxCode.SelectedIndex = -1;
                txtTemporarryNo.Text = "";
                dtTempDate.Value = DateTime.Parse("1900-01-01");
                txtMRNNo.Text = "";
                dtMRNDate.Value = DateTime.Parse("1900-01-01");
                txtPONos.Text = "";
                txtPODates.Text = "";
                //dtPODate.Value = DateTime.Today.Date;
                dtDCDate.Value = DateTime.Today.Date;
                dtInvoiceDate.Value = DateTime.Today.Date;
                txtRemarks.Text = "";
                txtReference.Text = "";
                txtTransporterName.Text = "";
                txtInvoiceNo.Text = "";
                txtDCNo.Text = "";
                txtProductValue.Text = "";
                txtTaxAmount.Text = "";
                txtMRNValue.Text = "";
                btnPOProdValue.Text = "";
                btnPOTaxValue.Text = "";
                btnProductValue.Text = "";
                btnTaxAmount.Text = "";
                cmbCurrency.SelectedIndex = -1;
                txtINRInversionRate.Text = "";
                txtProductValueINR.Text = "";
                txtTaxAmountINR.Text = "";
                txtMRNValueINR.Text = "";
                track = false;
                approveCellClick = false;
                prevmrn = new mrnheader();
                isViewMode = false;
                //removeControlsFromPnllvPanel();
                AddRowClick = false;
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
                yy = 1;
                btnSave.Text = "Save";
                pnlAddEdit.Visible = true;
                closeAllPanels();
                pnlAddEdit.Visible = true;
                setButtonVisibility("btnNew");
                isViewMode = false;
                AddRowClick = false;
            }
            catch (Exception)
            {

            }
        }


        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                AddMRNDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddMRNDetailRow()
        {
            Boolean status = true;
            try
            {

                if (grdMRNDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkMRNDetailGridRows())
                    {
                        return false;
                    }
                }
                grdMRNDetail.Rows.Add();
                int kount = grdMRNDetail.RowCount;
                grdMRNDetail.Rows[kount - 1].Cells["LineNo"].Value = kount;
                //DataGridViewComboBoxCell ComboColumn1 = (DataGridViewComboBoxCell)(grdMRNDetail.Rows[kount - 1].Cells["Item"]);
                //if (txtPONo.Text.Length != 0)
                //    PurchaseOrderDB.fillMRStockItemGridViewCombo(ComboColumn1, txtPONo.Text, dtPODate.Value);
                //else
                //    StockItemDB.fillStockItemGridViewCombo(ComboColumn1, "");
                DataGridViewComboBoxCell ComboColumn3 = (DataGridViewComboBoxCell)(grdMRNDetail.Rows[kount - 1].Cells["gTaxCode"]);
                TaxCodeDB.fillTaxCodeGridViewCombo(ComboColumn3, "");
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["Item"].Value = "";
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["PORefNo"].Value = "";
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["Unit"].Value = "";
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["ModelNo"].Value = "";
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["ModelName"].Value = "";
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["PONo"].Value = "";
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["PODate"].Value = "";
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["OrderedQuantity"].Value = Convert.ToDouble(0);
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["OrderedPrice"].Value = Convert.ToDouble(0);
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["OrderedTax"].Value = Convert.ToDouble(0);
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["Quantity"].Value = Convert.ToDouble(0);
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["Price"].Value = Convert.ToDouble(0);
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["Value"].Value = Convert.ToDouble(0);
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["PriceINR"].Value = Convert.ToDouble(0);
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["TaxINR"].Value = Convert.ToDouble(0);
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["Tax"].Value = Convert.ToDouble(0);
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["TaxDetails"].Value = "";
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["BatchNo"].Value = "";
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["SerielNo"].Value = "";
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["ExpiryDate"].Value = DateTime.Parse("1900-01-01");
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["StoreLocationID"]);
                CatalogueValueDB.fillCatalogValueGridViewComboNew(ComboColumn2, "StoreLocation");
                //-------------27/10/2017
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["StoreLocationID"].Value = Main.MainStore;
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["StoreLocationID"].ReadOnly = true;
                //-------------
                ComboColumn2.DropDownWidth = 300;
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["QuantityAccepted"].Value = Convert.ToDouble(0);
                grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["QuantityRejected"].Value = Convert.ToDouble(0);
                var BtnCell = (DataGridViewButtonCell)grdMRNDetail.Rows[kount - 1].Cells["Delete"];
                BtnCell.Value = "Del";
                if (AddRowClick)
                {
                    grdMRNDetail.FirstDisplayedScrollingRowIndex = grdMRNDetail.RowCount - 1;
                    grdMRNDetail.CurrentCell = grdMRNDetail.Rows[kount - 1].Cells[0];
                }
                grdMRNDetail.Columns[0].Frozen = false;
                grdMRNDetail.FirstDisplayedScrollingColumnIndex = 0;
                grdMRNDetail.Columns["Sel"].Frozen = true;
                //if (yy == 1)
                //{
                //    if (txtPONos.Text.Length != 0)
                //    {
                //        grdMRNDetail.Columns["OrderedQuantity"].Visible = true;
                //        grdMRNDetail.Columns["OrderedPrice"].Visible = true;
                //        grdMRNDetail.Columns["OrderedTax"].Visible = true;
                //    }
                //    else
                //    {
                //        grdMRNDetail.Columns["OrderedQuantity"].Visible = false;
                //        grdMRNDetail.Columns["OrderedPrice"].Visible = false;
                //        grdMRNDetail.Columns["OrderedTax"].Visible = false;
                //    }
                //}
                //yy = 0;
            }

            catch (Exception ex)
            {
                MessageBox.Show("AddMRNDetailRow() : Error");
            }

            return status;
        }
        private Boolean verifyAndReworkMRNDetailGridRows()
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
                if (grdMRNDetail.Rows.Count <= 0)
                {
                    ////MessageBox.Show("No entries in MRN details");
                    txtProductValue.Text = productvalue.ToString();
                    txtTaxAmount.Text = taxvalue.ToString(); //fill this later
                    txtMRNValue.Text = (productvalue + taxvalue).ToString();
                    btnProductValue.Text = txtProductValue.Text;
                    //btnPOProdValue.Text = 
                    return status;
                }
                //clear tax details table
                TaxDetailsTable.Rows.Clear();
                if (!isViewMode)
                {
                    if (cmbCurrency.SelectedIndex == -1 || txtINRInversionRate.Text.Length == 0)
                    {
                        MessageBox.Show("Fill Currency Or Exchange rate");
                        return false;
                    }
                }

                for (int i = 0; i < grdMRNDetail.Rows.Count; i++)
                {
                    if (grdMRNDetail.Rows[i].Cells["gTaxCode"].Value == null ||
                        grdMRNDetail.Rows[i].Cells["gTaxCode"].Value.ToString().Length == 0)
                    {
                        MessageBox.Show("Fill Tax Code in row " + (i + 1));
                        return false;
                    }
                    if (grdMRNDetail.Rows[i].Cells["StoreLocationID"].Value == null ||
                        grdMRNDetail.Rows[i].Cells["StoreLocationID"].Value.ToString().Trim().Length == 0)
                    {
                        MessageBox.Show("Fill Store Location in row " + (i + 1));
                        return false;
                    }
                    double MRNprice = Convert.ToDouble(grdMRNDetail.Rows[i].Cells["Price"].Value);
                    double poprice = PurchaseOrderDB.getPODetailsFromRowID(Convert.ToInt32(grdMRNDetail.Rows[i].Cells["PORefNo"].Value)).Price;
                    if (MRNprice > poprice)
                    {
                        MessageBox.Show("Price in Row: " + (i + 1) + " is more than PO Price.\nPO Price: " + poprice);
                        return false;
                    }
                    grdMRNDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if ((grdMRNDetail.Rows[i].Cells["Item"].Value == null) ||
                        (grdMRNDetail.Rows[i].Cells["PORefNo"].Value == null) ||
                        (grdMRNDetail.Rows[i].Cells["ModelNo"].Value == null) ||
                        (grdMRNDetail.Rows[i].Cells["ModelName"].Value == null) ||
                          (grdMRNDetail.Rows[i].Cells["Quantity"].Value == null) ||
                        (grdMRNDetail.Rows[i].Cells["Price"].Value == null) ||
                        (grdMRNDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Length == 0) ||
                        (grdMRNDetail.Rows[i].Cells["PORefNo"].Value.ToString().Trim().Length == 0) ||
                        (grdMRNDetail.Rows[i].Cells["ModelNo"].Value.ToString().Trim().Length == 0) ||
                        (grdMRNDetail.Rows[i].Cells["ModelName"].Value.ToString().Trim().Length == 0) ||
                        (grdMRNDetail.Rows[i].Cells["Quantity"].Value.ToString().Trim().Length == 0) ||
                        (grdMRNDetail.Rows[i].Cells["Price"].Value.ToString().Trim().Length == 0) ||
                         (Convert.ToDouble(grdMRNDetail.Rows[i].Cells["Quantity"].Value) == 0) ||
                         (Convert.ToDouble(grdMRNDetail.Rows[i].Cells["Price"].Value) == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                    quantity = Convert.ToDouble(grdMRNDetail.Rows[i].Cells["Quantity"].Value);
                    price = Convert.ToDouble(grdMRNDetail.Rows[i].Cells["Price"].Value);
                    if (price != 0 && quantity != 0)
                    {
                        cost = Math.Round(quantity * price, 2);
                    }

                    try
                    {
                        strtaxCode = grdMRNDetail.Rows[i].Cells["gTaxCode"].Value.ToString();
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
                    grdMRNDetail.Rows[i].Cells["Value"].Value = Convert.ToDouble(cost.ToString());
                    grdMRNDetail.Rows[i].Cells["Tax"].Value = ttax2;
                    grdMRNDetail.Rows[i].Cells["TaxINR"].Value = ttax2 * Convert.ToDouble(txtINRInversionRate.Text);
                    grdMRNDetail.Rows[i].Cells["TaxDetails"].Value = strTax;

                    double prc = Convert.ToDouble(grdMRNDetail.Rows[i].Cells["Price"].Value);
                    grdMRNDetail.Rows[i].Cells["PriceINR"].Value = prc * Convert.ToDouble(txtINRInversionRate.Text);

                    productvalue = productvalue + cost;
                    taxvalue = taxvalue + ttax2;

                    //- rewok tax value
                }
                double INRValue = Convert.ToDouble(txtINRInversionRate.Text);
                txtProductValue.Text = productvalue.ToString();
                txtTaxAmount.Text = taxvalue.ToString(); //fill this later
                txtMRNValue.Text = (productvalue + taxvalue).ToString();

                txtProductValueINR.Text = (productvalue * INRValue).ToString();
                txtTaxAmountINR.Text = (taxvalue * INRValue).ToString(); //fill this later
                txtMRNValueINR.Text = ((productvalue + taxvalue) * INRValue).ToString();

                btnProductValue.Text = txtProductValue.Text;
                btnTaxAmount.Text = txtTaxAmount.Text;
                if (!validateItems())
                {
                    MessageBox.Show("Validation failed");
                }
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
                if (!isViewMode)
                {
                    for (int i = 0; i < grdMRNDetail.Rows.Count - 1; i++)
                    {
                        for (int j = i + 1; j < grdMRNDetail.Rows.Count; j++)
                        {

                            if (grdMRNDetail.Rows[i].Cells[1].Value.ToString() == grdMRNDetail.Rows[j].Cells["Item"].Value.ToString() &&
                                grdMRNDetail.Rows[i].Cells["ModelNo"].Value.ToString() == grdMRNDetail.Rows[j].Cells["ModelNo"].Value.ToString())
                            {
                                //duplicate item code
                                MessageBox.Show("Item code duplicated in MRN details... please ensure correctness (" +
                                    grdMRNDetail.Rows[i].Cells["Item"].Value.ToString() + ")");
                            }
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

        private List<mrndetail> getMRNDetails(mrnheader mrnh)
        {
            List<mrndetail> MRNDetails = new List<mrndetail>();
            try
            {
                mrndetail mrnd = new mrndetail();
                for (int i = 0; i < grdMRNDetail.Rows.Count; i++)
                {
                    mrnd = new mrndetail();
                    mrnd.DocumentID = mrnh.DocumentID;
                    mrnd.TemporaryNo = mrnh.TemporaryNo;
                    mrnd.TemporaryDate = mrnh.TemporaryDate;
                    mrnd.PORefNo = Convert.ToInt32(grdMRNDetail.Rows[i].Cells["PORefNo"].Value);
                    mrnd.ModelNo = grdMRNDetail.Rows[i].Cells["ModelNo"].Value.ToString();
                    mrnd.ModelName = grdMRNDetail.Rows[i].Cells["ModelName"].Value.ToString();
                    mrnd.PONO = Convert.ToInt32(grdMRNDetail.Rows[i].Cells["PONo"].Value.ToString());
                    mrnd.PODate = Convert.ToDateTime(grdMRNDetail.Rows[i].Cells["PODate"].Value.ToString());
                    mrnd.TaxCode = grdMRNDetail.Rows[i].Cells["gTaxCode"].Value.ToString();
                    mrnd.StockItemID = grdMRNDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Substring(0, grdMRNDetail.Rows[i].Cells["Item"].Value.ToString().Trim().IndexOf('-'));
                    mrnd.StockItemName = grdMRNDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Substring(grdMRNDetail.Rows[i].Cells["Item"].Value.ToString().Trim().IndexOf('-') + 1);
                    mrnd.Quantity = Convert.ToDouble(grdMRNDetail.Rows[i].Cells["Quantity"].Value);
                    mrnd.Price = Convert.ToDouble(grdMRNDetail.Rows[i].Cells["Price"].Value);
                    mrnd.Tax = Convert.ToDouble(grdMRNDetail.Rows[i].Cells["Tax"].Value);
                    mrnd.PriceINR = Convert.ToDouble(grdMRNDetail.Rows[i].Cells["PriceINR"].Value);
                    mrnd.TaxINR = Convert.ToDouble(grdMRNDetail.Rows[i].Cells["TaxINR"].Value);
                    mrnd.TaxDetails = grdMRNDetail.Rows[i].Cells["TaxDetails"].Value.ToString();
                    mrnd.BatchNo = grdMRNDetail.Rows[i].Cells["BatchNo"].Value.ToString();
                    mrnd.SerialNo = grdMRNDetail.Rows[i].Cells["SerielNo"].Value.ToString();
                    mrnd.ExpiryDate = Convert.ToDateTime(grdMRNDetail.Rows[i].Cells["ExpiryDate"].Value);
                    mrnd.StoreLocationID = grdMRNDetail.Rows[i].Cells["StoreLocationID"].Value.ToString();//.Trim().Substring(0, grdMRNDetail.Rows[i].Cells["StoreLocationID"].Value.ToString().Trim().IndexOf('-'));
                    if (grdMRNDetail.Rows[i].Cells["QuantityAccepted"].Value == null ||
                        grdMRNDetail.Rows[i].Cells["QuantityAccepted"].Value.ToString().Length == 0)
                    {
                        mrnd.QuantityAccepted = 0;
                    }
                    else
                        mrnd.QuantityAccepted = Convert.ToDouble(grdMRNDetail.Rows[i].Cells["QuantityAccepted"].Value);
                    //string s = grdMRNDetail.Rows[i].Cells["QuantityRejected"].Value.ToString();
                    if (grdMRNDetail.Rows[i].Cells["QuantityRejected"].Value == null ||
                        grdMRNDetail.Rows[i].Cells["QuantityRejected"].Value.ToString().Length == 0)
                    {
                        mrnd.QuantityRejected = 0;
                    }
                    else
                        mrnd.QuantityRejected = Convert.ToDouble(grdMRNDetail.Rows[i].Cells["QuantityRejected"].Value);
                    MRNDetails.Add(mrnd);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("createAndUpdateMRNDetails() : Error updating MRN Details");
                MRNDetails = null;
            }
            return MRNDetails;
        }

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            txtSearchMaingrid.Text = "";
            ListFilteredMRNHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            txtSearchMaingrid.Text = "";
            ListFilteredMRNHeader(listOption);
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
            txtSearchMaingrid.Text = "";
            ListFilteredMRNHeader(listOption);
        }
        private mrnheader fillMRNHeader(mrnheader mrnh)
        {
            try
            {
                if (!verifyAndReworkMRNDetailGridRows())
                {
                    MessageBox.Show("Validation Failed in MRN Detail");
                }
                mrnh.DocumentID = docID;
                mrnh.MRNDate = dtMRNDate.Value;
                //string n = cmbCustomer.
                ////////mrnh.CustomerID = cmbCustomer.SelectedItem.ToString().Trim().Substring(0, cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-'));
                mrnh.CustomerID = txtCustomerID.Text.Trim();
                mrnh.PONOs = txtPONos.Text;
                mrnh.PODates = txtPODates.Text;
                mrnh.DCNo = txtDCNo.Text;
                mrnh.DCDate = dtDCDate.Value;
                if (txtReference.Text.Length != 0)
                    mrnh.Reference = txtReference.Text;
                else
                    mrnh.Reference = "";
                mrnh.InvoiceNo = txtInvoiceNo.Text;
                mrnh.InvoiceDate = dtInvoiceDate.Value;
                mrnh.TransportationMode = ((Structures.ComboBoxItem)cmbTransporationMode.SelectedItem).HiddenValue;
                //mrnh.TransportationMode = cmbTransporationMode.SelectedItem.ToString().Trim().Substring(0, cmbTransporationMode.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
                mrnh.TransporterType = ((Structures.ComboBoxItem)cmbTransporationType.SelectedItem).HiddenValue;
                //mrnh.TransporterType = cmbTransporationType.SelectedItem.ToString().Trim().Substring(0, cmbTransporationType.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
                mrnh.Remarks = txtRemarks.Text;
                mrnh.TransporterName = txtTransporterName.Text;
                //mrnh.TaxCode = cmbTaxCode.SelectedItem.ToString();

                if (txtTaxAmount.Text.Length != 0)
                    mrnh.TaxAmount = Convert.ToDouble(txtTaxAmount.Text);
                if (txtProductValue.Text.Length != 0)
                    mrnh.ProductValue = Convert.ToDouble(txtProductValue.Text);
                if (txtMRNValue.Text.Length != 0)
                    mrnh.MRNValue = Convert.ToDouble(txtMRNValue.Text);
                if (txtTaxAmountINR.Text.Length != 0)
                    mrnh.TaxAmountINR = Convert.ToDouble(txtTaxAmountINR.Text);
                if (txtProductValueINR.Text.Length != 0)
                    mrnh.ProductValueINR = Convert.ToDouble(txtProductValueINR.Text);
                if (txtMRNValueINR.Text.Length != 0)
                    mrnh.MRNValueINR = Convert.ToDouble(txtMRNValueINR.Text);

                if (cmbCurrency.SelectedIndex != -1)
                    mrnh.CurrencyID = ((Structures.ComboBoxItem)cmbCurrency.SelectedItem).HiddenValue;
                if (txtINRInversionRate.Text.Length != 0)
                    mrnh.ExchangeRate = Convert.ToDecimal(txtINRInversionRate.Text);


                //mrnh.ReferenceNo = txtReference.Text;tode
                mrnh.Comments = docCmtrDB.DGVtoString(dgvComments).Replace("'", "''");
                mrnh.ForwarderList = prevmrn.ForwarderList;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Validation failed in MRN Header");
            }
            return mrnh;
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {
                if (!verifyAndReworkMRNDetailGridRows())
                {
                    return;
                }
                MRNHeaderDB rnhdb = new MRNHeaderDB();
                mrnheader mh = new mrnheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;
                mrnheader mrnh = fillMRNHeader(mh);
                isValidate = true;

                if (!rnhdb.validateMRNHeader(mrnh, 0))
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    //mrnh.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    mrnh.DocumentStatus = 1; //created
                    mrnh.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    mrnh.TemporaryNo = Convert.ToInt32(txtTemporarryNo.Text);
                    mrnh.TemporaryDate = prevmrn.TemporaryDate;
                    mrnh.DocumentStatus = prevmrn.DocumentStatus;
                    mrnh.QCStatus = prevmrn.QCStatus;
                }
                if (MRNHeaderDB.IsInvoiceNoFoundInPrevMRN(mrnh))
                {
                    MessageBox.Show("Invoice number "+mrnh.InvoiceNo + " from same supplier already available in system. Modify invoice number and save");
                    return;
                }
                //Replacing single quotes
                mrnh = verifyHeaderInputString(mrnh);
                verifyDetailInputString();
                if (rnhdb.validateMRNHeader(mrnh, 0))
                {
                    //--create comment status string
                    docCmtrDB = new DocCommenterDB();
                    if (userIsACommenter)
                    {
                        //if the user is only a commenter and ticked the comment as final; then update his comment string as final
                        //and update the comment status
                        if (txtComments.Text != null && txtComments.Text.Trim().Length != 0)
                        {
                            docCmtrDB = new DocCommenterDB();
                            mrnh.CommentStatus = docCmtrDB.createCommentStatusString(prevmrn.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            mrnh.CommentStatus = prevmrn.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            mrnh.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            mrnh.CommentStatus = prevmrn.CommentStatus;
                        }
                    }
                    //----------
                    int tmpStatus = 1;
                    if (txtComments.Text.Trim().Length > 0)
                    {
                        mrnh.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus).Replace("'", "''"); ;
                    }
                    List<mrndetail> MRNDetails = getMRNDetails(mrnh);
                    if (mrnh.QCStatus == 0)
                    {
                        showQuantityAvailableForAllProductListView(MRNDetails, 1); //opt 1 : save
                        if (!isValidate)
                        {
                            MessageBox.Show("Enterd Quantity is more than the PO Qunatity. Not Allowed TO Save/Update.");
                            return;
                        }
                        DialogResult dialog = MessageBox.Show("Are you sure to " + btnSave.Text + " the document ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.No)
                        {
                            return;
                        }
                    }
                    if (btnText.Equals("Update"))
                    {
                        if (mrnh.QCStatus == 1)
                        {
                            if (!verifyAcceptedAndRejectedQuant(MRNDetails))
                            {
                                return;
                            }
                        }
                        if (rnhdb.updateMRNHeaderAndDetail(mrnh, prevmrn, MRNDetails))
                        {
                            MessageBox.Show("MRN Header Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredMRNHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update MRN Header");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (rnhdb.InsertMRNHeaderAndDetail(mrnh, MRNDetails))
                        {
                            MessageBox.Show("MRN Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredMRNHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert MRN  Header");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("MRN Details Validation failed");
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
        private Boolean verifyAcceptedAndRejectedQuant(List<mrndetail> MRNDetails)
        {
            Boolean status = true;
            int count = 0;
            try
            {
                foreach (mrndetail mrnd in MRNDetails)
                {
                    count++;
                    if (mrnd.Quantity != (mrnd.QuantityAccepted + mrnd.QuantityRejected))
                    {
                        MessageBox.Show("Accepted and Rejected Quantity Total should equal with order Quantity\nCheck At Row No: " + count);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Check Accepted and Rejected Quantity");
                status = false;
            }
            return status;
        }
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            track = true;
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }

        private void btnAddLine_Click_1(object sender, EventArgs e)
        {
            if (cmbCurrency.SelectedIndex == -1 || txtINRInversionRate.Text == null || Convert.ToDouble(txtINRInversionRate.Text) == 0)
            {
                MessageBox.Show("Currency ID/Exchange Rate is empty");
                return;
            }
            AddRowClick = true;
            AddMRNDetailRow();
            //if (txtPONo.Text.Length == 0)
            //    AddPOPIDetailRow();
            //else
            //{
            //    AddPOPIDetailRowForPO();
            //}
        }
        void cellDateTimePickerValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            ////grdProductionPlanDetail.Rows[grdProductionPlanDetail.RowCount - 1].Cells["gProcessID"]
            grdMRNDetail.Rows[grdMRNDetail.CurrentCell.RowIndex].Cells[grdMRNDetail.CurrentCell.ColumnIndex - 1].Value = dtp.Value;
            //dataGridView1.CurrentCell.Value = cellDateTimePicker.Value.ToString();//convert the date as per your format
            //cellDateTimePicker.Visible = false;
            ////dtp.Dispose();
            ////dtpForm.Dispose();

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
        private ListView getLVForAllSelectedPOs(string POnos, string PODates)
        {
            ListView lvTot = new ListView();
            try
            {
                lvTot.View = System.Windows.Forms.View.Details;
                lvTot.LabelEdit = true;
                lvTot.AllowColumnReorder = true;
                lvTot.CheckBoxes = true;
                lvTot.FullRowSelect = true;
                lvTot.GridLines = true;
                lvTot.Sorting = System.Windows.Forms.SortOrder.Ascending;
                //PurchaseOrderDB podb = new PurchaseOrderDB();
                //List<poheader> POHeaders = podb.getFilteredPurchaseOrderHeader("", 6, "");
                ////int index = 0;
                lvTot.Columns.Add("Sel", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("PO NO", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("PO Date", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Product", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Unit", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Model No", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Model Name", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Ordered Quant", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Ordered Price", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Ordered Tax", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("Tax Code", -2, HorizontalAlignment.Center);
                lvTot.Columns.Add("PORef No", -2, HorizontalAlignment.Center);
                string[] poNoArr = POnos.Split(';');
                string[] poDatesArr = PODates.Split(';');
                for (int i = 1; i < poNoArr.Length - 1; i++)
                {
                    chkTemp = PurchaseOrderDB.getTempNo(Convert.ToInt32(poNoArr[i]), Convert.ToDateTime(poDatesArr[i]));
                    string[] str = chkTemp.Split(';');
                    List<podetail> PODetail = PurchaseOrderDB.getPODetailForMRNDetail(Convert.ToInt32(str[0]), Convert.ToDateTime(str[1]));
                    ListView lv1 =
                        ListViewForMRN(PODetail, Convert.ToInt32(poNoArr[i]), Convert.ToDateTime(poDatesArr[i]));
                    lvTot.Items.AddRange((from ListViewItem item in lv1.Items
                                          select (ListViewItem)item.Clone()).ToArray());
                    //lvTot = lvTot.(lv);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception in showing PODetail Listview");
            }
            return lvTot;
        }
        private void grdPOPIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                string columnName = grdMRNDetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdMRNDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkMRNDetailGridRows();
                    }
                    if (columnName.Equals("ViewTaxDetails"))
                    {
                        //show tax details
                        DialogResult dialog = MessageBox.Show(grdMRNDetail.Rows[e.RowIndex].Cells["TaxDetails"].Value.ToString(),
                            "Line No : " + (e.RowIndex + 1), MessageBoxButtons.OK);
                    }
                    if (columnName.Equals("Sel"))
                    {
                        string[] poList = txtPONos.Text.Trim().Split(';');
                        string[] DateList = txtPODates.Text.Trim().Split(';');
                        if (txtPONos.Text.Length == 0)
                        {
                            MessageBox.Show("Select PO NO");
                            return;
                        }
                        if (grdMRNDetail.CurrentRow.Cells["Item"].Value.ToString().Length != 0)
                        {
                            try
                            {
                                DialogResult dialog = MessageBox.Show("Product Detail will removed?", "Yes", MessageBoxButtons.YesNo);
                                int count = grdMRNDetail.Columns.Count;
                                if (dialog == DialogResult.Yes)
                                {
                                    for (int x = 0; x < count; x++)
                                    {
                                        if (grdMRNDetail.CurrentRow.Cells[x].GetType() != (typeof(Button)))
                                        {
                                            grdMRNDetail.CurrentRow.Cells[x].Value = "";
                                            if (grdMRNDetail.Columns[grdMRNDetail.CurrentRow.Cells[x].ColumnIndex].Name == "ExpiryDate")
                                                grdMRNDetail.CurrentRow.Cells[x].Value = DateTime.Parse("1900-01-01");
                                        }
                                    }
                                }
                                else
                                    return;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        //string[] str = chkTemp.Split(';');
                        //List<podetail> PODetail = PurchaseOrderDB.getPODetailForMRNDetail(Convert.ToInt32(str[0]), Convert.ToDateTime(str[1]));
                        showproductListViewForPO();
                    }
                    if (columnName.Equals("dtButton"))
                    {
                        DateTime dt = DateTime.Today;
                        if (columnName.Equals("dtButton"))
                        {
                            if (grdMRNDetail.Rows[e.RowIndex].Cells["ExpiryDate"].Value.ToString().Trim().Length == 0)
                                dt = Convert.ToDateTime(DateTime.Today);
                            else
                                dt = Convert.ToDateTime(grdMRNDetail.Rows[e.RowIndex].Cells["ExpiryDate"].Value);
                        }
                        //showDtPicker(e.ColumnIndex,e.RowIndex);
                        Rectangle tempRect = grdMRNDetail.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
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
        private void showproductListViewForPO()
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(1200, 400);
            lv = getLVForAllSelectedPOs(txtPONos.Text, txtPODates.Text);
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView3_ItemChecked);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(1200, 350));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 365);
            lvOK.Click += new System.EventHandler(this.lvOK_Click3);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 365);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click3);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
            //pnlAddEdit.Controls.Add(pnllv);
            //pnllv.BringToFront();
            //pnllv.Visible = true;
        }
        public ListView ListViewForMRN(List<podetail> PODetail, int poNo, DateTime poDate)
        {
            ListView lv = new ListView();
            try
            {

                //lv.View = View.Details;
                lv.View = System.Windows.Forms.View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                //PurchaseOrderDB podb = new PurchaseOrderDB();
                //List<poheader> POHeaders = podb.getFilteredPurchaseOrderHeader("", 6, "");
                ////int index = 0;
                lv.Columns.Add("Sel", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO NO", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO Date", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Product", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Unit", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Model No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Model Name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Ordered Quant", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Ordered Price", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Ordered Tax", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Tax Code", -2, HorizontalAlignment.Center);
                lv.Columns.Add("PORef No", -2, HorizontalAlignment.Left);
                foreach (podetail pod in PODetail)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(poNo.ToString());
                    item1.SubItems.Add(poDate.ToShortDateString());
                    item1.SubItems.Add(pod.StockItemID + "-" + pod.StockItemName);
                    item1.SubItems.Add(pod.Unit);
                    item1.SubItems.Add(pod.ModelNo);
                    item1.SubItems.Add(pod.ModelName);
                    item1.SubItems.Add(pod.Quantity.ToString());
                    item1.SubItems.Add(pod.Price.ToString());
                    item1.SubItems.Add(pod.Tax.ToString());
                    item1.SubItems.Add(pod.TaxCode.ToString());
                    item1.SubItems.Add(pod.RowID.ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        private void lvOK_Click3(object sender, EventArgs e)
        {
            try
            {

                int kount = 0;
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        kount++;
                    }
                }
                if (kount != 1)
                {
                    MessageBox.Show("Select one PO");
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        grdMRNDetail.CurrentRow.Cells["LineNo"].Value = grdMRNDetail.RowCount;
                        grdMRNDetail.CurrentRow.Cells["PORefNo"].Value = itemRow.SubItems[11].Text;
                        grdMRNDetail.CurrentRow.Cells["Item"].Value = itemRow.SubItems[3].Text;
                        grdMRNDetail.CurrentRow.Cells["PONo"].Value = itemRow.SubItems[1].Text;
                        grdMRNDetail.CurrentRow.Cells["PODate"].Value = itemRow.SubItems[2].Text;
                        grdMRNDetail.CurrentRow.Cells["ModelNo"].Value = itemRow.SubItems[5].Text;
                        grdMRNDetail.CurrentRow.Cells["ModelName"].Value = itemRow.SubItems[6].Text;
                        grdMRNDetail.CurrentRow.Cells["OrderedQuantity"].Value = Convert.ToDouble(itemRow.SubItems[7].Text);
                        grdMRNDetail.CurrentRow.Cells["Unit"].Value = itemRow.SubItems[4].Text;
                        grdMRNDetail.CurrentRow.Cells["OrderedPrice"].Value = Convert.ToDouble(itemRow.SubItems[8].Text);
                        grdMRNDetail.CurrentRow.Cells["OrderedTax"].Value = Convert.ToDouble(itemRow.SubItems[9].Text);
                        grdMRNDetail.CurrentRow.Cells["gTaxCode"].Value = itemRow.SubItems[10].Text;
                        // grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["Quantity"].Value = "";
                        grdMRNDetail.CurrentRow.Cells["Price"].Value = Convert.ToDouble(itemRow.SubItems[8].Text);
                        grdMRNDetail.CurrentRow.Cells["PriceINR"].Value = Convert.ToDouble(grdMRNDetail.CurrentRow.Cells["Price"].Value) * Convert.ToDouble(txtINRInversionRate.Text);
                        //grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["Tax"].Value = "";
                        //grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["BatchNo"].Value = "";
                        //grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["SerielNo"].Value = "";
                        ////grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["ExpiryDate"].Value = "";
                        //DataGridViewComboBoxCell ComboColumn1 = (DataGridViewComboBoxCell)(grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["StoreLocationID"]);
                        //CatalogueValueDB.fillCatalogValueGridViewCombo(ComboColumn1, "StoreLocation");
                        //grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["QuantityAccepted"].Value = 0;
                        //grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["QuantityRejected"].Value = 0;
                        grdMRNDetail.Columns["OrderedQuantity"].Visible = true;
                        grdMRNDetail.Columns["OrderedPrice"].Visible = true;
                        grdMRNDetail.Columns["OrderedTax"].Visible = true;
                        grdMRNDetail.Columns["QuantityAccepted"].Visible = false;
                        grdMRNDetail.Columns["QuantityRejected"].Visible = false;
                    }
                }
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        private void lvCancel_Click3(object sender, EventArgs e)
        {
            try
            {
                //btnSelectPO.Enabled = true;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        //-----
        //private void listView3_ItemChecked(object sender, ItemCheckedEventArgs e)
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

        private void btnCalculateax_Click_1(object sender, EventArgs e)
        {
            verifyAndReworkMRNDetailGridRows();
        }
        private void DisableQCApprovedGrdDetailsVisibility()
        {
            //tabControl1.TabPages["tabMRNDetail"].Enabled = true;
            btnAddLine.Enabled = false;
            btnClearEntries.Enabled = false;
            btnCalculate.Enabled = false;
            grdMRNDetail.Columns["Quantity"].ReadOnly = true;
            grdMRNDetail.Columns["BatchNo"].ReadOnly = true;
            grdMRNDetail.Columns["SerielNo"].ReadOnly = true;
            grdMRNDetail.Columns["ExpiryDate"].ReadOnly = true;
            grdMRNDetail.Columns["StoreLocationID"].ReadOnly = true;
            grdMRNDetail.Columns["gTaxCode"].ReadOnly = true;
            //grdMRNDetail.Columns["Item"].ReadOnly = true;
            grdMRNDetail.Columns["Delete"].Visible = false;
            grdMRNDetail.Columns["Sel"].Visible = false;
            grdMRNDetail.Columns["dtButton"].Visible = false;
            //foreach (DataGridViewColumn col in grdMRNDetail.Columns)
            //{
            //    if (col.GetType() == typeof(DataGridViewButtonColumn))
            //        col.Visible = false;
            //}
        }
        private void EnableQCApprovedGrdDetailsVisibility()
        {
            ///tabControl1.TabPages["tabMRNDetail"].Enabled = false;
            btnAddLine.Enabled = true;
            btnClearEntries.Enabled = true;
            btnCalculate.Enabled = true;
            grdMRNDetail.Columns["Quantity"].ReadOnly = false;
            grdMRNDetail.Columns["BatchNo"].ReadOnly = false;
            grdMRNDetail.Columns["SerielNo"].ReadOnly = false;
            //grdMRNDetail.Columns["ExpiryDate"].ReadOnly = false;
            grdMRNDetail.Columns["StoreLocationID"].ReadOnly = false;
            grdMRNDetail.Columns["gTaxCode"].ReadOnly = false;
            grdMRNDetail.Columns["Item"].ReadOnly = true;
            grdMRNDetail.Columns["Delete"].Visible = true;
            grdMRNDetail.Columns["Sel"].Visible = true;
            grdMRNDetail.Columns["dtButton"].Visible = true;
            //foreach (DataGridViewColumn col in grdMRNDetail.Columns)
            //{
            //    if (col.GetType() == typeof(DataGridViewButtonColumn))
            //        col.Visible = true;
            //    else
            //        col.ReadOnly = false;
            //}
        }
        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                grdList.Rows[e.RowIndex].Selected = true;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("LoadDocument") ||
                    columnName.Equals("View"))
                {

                    clearData();
                    //track = true;
                    setButtonVisibility(columnName);
                    AddRowClick = false;
                    prevmrn = new mrnheader();
                    prevmrn.QCStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["QCStatus"].Value.ToString());
                    if (columnName.Equals("Approve") && prevmrn.QCStatus == 1)
                    {

                        btnForward.Visible = false;
                        btnApprove.Visible = false;
                        btnReverse.Visible = false;
                        btnQC.Visible = false;
                        btnQCCompleted.Visible = true;
                        disableTabPages();
                    }
                    if (columnName.Equals("Approve"))
                        approveCellClick = true;
                    if (columnName.Equals("Edit") && prevmrn.QCStatus == 99)
                    {
                        //disableTabPages();
                        tabControl1.TabPages["tabMRNHeader"].Enabled = false;
                        btnSave.Visible = false;
                        DisableQCApprovedGrdDetailsVisibility();
                    }
                    if (columnName.Equals("View"))
                    {
                        isViewMode = true;
                        tabControl1.TabPages["tabMRNDetail"].Enabled = true;
                    }
                    prevmrn.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();
                    prevmrn.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    prevmrn.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    prevmrn.Reference = grdList.Rows[e.RowIndex].Cells["Reference"].Value.ToString();
                    prevmrn.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    prevmrn.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    if (prevmrn.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    prevmrn.Comments = MRNHeaderDB.getUserComments(prevmrn.DocumentID, prevmrn.TemporaryNo, prevmrn.TemporaryDate);
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    //setPODetailColumns(docID);

                    MRNHeaderDB popidb = new MRNHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];



                    prevmrn.MRNNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gMRNNo"].Value.ToString());
                    prevmrn.MRNDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gMRNDate"].Value.ToString());
                    prevmrn.CustomerID = grdList.Rows[e.RowIndex].Cells["gCustomerID"].Value.ToString();
                    prevmrn.CustomerName = grdList.Rows[e.RowIndex].Cells["gCustomerName"].Value.ToString();

                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Document Temp No:" + prevmrn.TemporaryNo + "\n" +
                            "Document Temp Date:" + prevmrn.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "MRN No:" + prevmrn.MRNNo + "\n" +
                            "MRN Date:" + prevmrn.MRNDate.ToString("dd-MM-yyyy") + "\n" +
                            "Customer:" + prevmrn.CustomerName;
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevmrn.TemporaryNo + "-" + prevmrn.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_1(null, null);
                        return;
                    }
                    //--------

                    prevmrn.PONOs = grdList.Rows[e.RowIndex].Cells["gPONo"].Value.ToString();
                    prevmrn.PODates = grdList.Rows[e.RowIndex].Cells["gPODate"].Value.ToString();
                    prevmrn.DCNo = grdList.Rows[e.RowIndex].Cells["DCNo"].Value.ToString();
                    prevmrn.DCDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["DCDate"].Value.ToString());
                    prevmrn.Reference = grdList.Rows[e.RowIndex].Cells["Reference"].Value.ToString();
                    prevmrn.InvoiceNo = grdList.Rows[e.RowIndex].Cells["InvoiceNo"].Value.ToString();
                    prevmrn.InvoiceDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["InvoiceDate"].Value.ToString());
                    //prevmrn.FreightCharge = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["DCDate"].Value.ToString());
                    prevmrn.TransportationMode = grdList.Rows[e.RowIndex].Cells["TransportationMode"].Value.ToString();
                    prevmrn.TransporterType = grdList.Rows[e.RowIndex].Cells["TransporterType"].Value.ToString();
                    prevmrn.TransporterName = grdList.Rows[e.RowIndex].Cells["TransporterName"].Value.ToString();
                    prevmrn.ProductValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gProductValue"].Value.ToString());
                    prevmrn.TaxAmount = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["TaxAmount"].Value.ToString());
                    prevmrn.MRNValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["MRNValue"].Value.ToString());
                    prevmrn.ProductValueINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["ProductValueINR"].Value.ToString());
                    prevmrn.TaxAmountINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["TaxAmountINR"].Value.ToString());
                    prevmrn.MRNValueINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["MRNValueINR"].Value.ToString());
                    //prevmrn.TaxCode = grdList.Rows[e.RowIndex].Cells["TCode"].Value.ToString();
                    prevmrn.Remarks = grdList.Rows[e.RowIndex].Cells["gRemarks"].Value.ToString();
                    prevmrn.status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    prevmrn.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    prevmrn.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    prevmrn.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    prevmrn.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    prevmrn.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevmrn.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    prevmrn.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    prevmrn.CurrencyID = grdList.Rows[e.RowIndex].Cells["gCurrencyID"].Value.ToString();
                    prevmrn.ExchangeRate = Convert.ToDecimal(grdList.Rows[e.RowIndex].Cells["ExchangeRate"].Value.ToString());
                    //--comments

                    ////chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    prevmrn.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(prevmrn.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(prevmrn.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---


                    txtReference.Text = prevmrn.Reference.ToString();
                    txtTemporarryNo.Text = prevmrn.TemporaryNo.ToString();
                    dtTempDate.Value = prevmrn.TemporaryDate;
                    dtTempDate.Value = prevmrn.TemporaryDate;

                    txtMRNNo.Text = prevmrn.MRNNo.ToString();
                    try
                    {
                        dtMRNDate.Value = prevmrn.MRNDate;
                    }
                    catch (Exception)
                    {
                        dtMRNDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtProductValue.Text = prevmrn.ProductValue.ToString();
                    txtTaxAmount.Text = prevmrn.TaxAmount.ToString();
                    txtMRNValue.Text = prevmrn.MRNValue.ToString();
                    ////////cmbCustomer.SelectedIndex = cmbCustomer.FindString(prevmrn.CustomerID);
                    //cmbCustomer.SelectedIndex =
                    //    Structures.ComboFUnctions.getComboIndex(cmbCustomer, prevmrn.CustomerID);
                    txtCustomerID.Text = prevmrn.CustomerID;
                    txtCustName.Text = prevmrn.CustomerName;
                    cmbCurrency.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCurrency, prevmrn.CurrencyID);
                    txtINRInversionRate.Text = prevmrn.ExchangeRate.ToString();
                    //cmbTaxCode.SelectedIndex = cmbTaxCode.FindString(prevmrn.TaxCode);
                    //cmbCustomer.Text = prevpopi.CustomerName;
                    txtPONos.Text = prevmrn.PONOs.ToString();
                    txtPODates.Text = prevmrn.PODates.ToString();
                    txtDCNo.Text = prevmrn.DCNo;
                    try
                    {
                        dtDCDate.Value = prevmrn.DCDate;
                    }
                    catch (Exception)
                    {
                        dtDCDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtInvoiceNo.Text = prevmrn.InvoiceNo;
                    try
                    {
                        dtInvoiceDate.Value = prevmrn.InvoiceDate;
                    }
                    catch (Exception)
                    {
                        dtInvoiceDate.Value = DateTime.Parse("01-01-1900");
                    }
                    cmbTransporationMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbTransporationMode, grdList.Rows[e.RowIndex].Cells["TransportationMode"].Value.ToString());
                    //cmbTransporationMode.SelectedIndex = cmbTransporationMode.FindString();
                    cmbTransporationType.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbTransporationType, grdList.Rows[e.RowIndex].Cells["TransporterType"].Value.ToString());
                    //cmbTransporationType.SelectedIndex = cmbTransporationType.FindString(grdList.Rows[e.RowIndex].Cells["TransporterType"].Value.ToString());
                    txtTransporterName.Text = prevmrn.TransporterName.ToString();
                    txtRemarks.Text = prevmrn.Remarks.ToString();
                    List<mrndetail> MRNDetail = MRNHeaderDB.getMRNDetail(prevmrn);
                    grdMRNDetail.Rows.Clear();
                    int i = 0;

                    btnPOProdValue.Visible = false;
                    btnPOTaxValue.Visible = false;
                    lblPOpProdValue.Visible = false;
                    lblPOTaxValue.Visible = false;
                    grdMRNDetail.Columns["QuantityAccepted"].Visible = true;
                    grdMRNDetail.Columns["QuantityRejected"].Visible = true;
                    try
                    {
                        foreach (mrndetail mrnd in MRNDetail)
                        {
                            if (!AddMRNDetailRow())
                            {
                                MessageBox.Show("Error found in MRN details. Please correct before updating the details");
                            }
                            else
                            {
                                try
                                {
                                    grdMRNDetail.Rows[i].Cells["Item"].Value = mrnd.StockItemID + "-" + mrnd.StockItemName;
                                }
                                catch (Exception ex)
                                {
                                    grdMRNDetail.Rows[i].Cells["Item"].Value = null;
                                }
                                podetail podetail = PurchaseOrderDB.getPODetailsFromRowID(mrnd.PORefNo);
                                grdMRNDetail.Rows[i].Cells["OrderedPrice"].Value = podetail.Price;
                                grdMRNDetail.Rows[i].Cells["OrderedTax"].Value = podetail.Tax;
                                grdMRNDetail.Rows[i].Cells["OrderedQuantity"].Value = podetail.Quantity;

                                grdMRNDetail.Rows[i].Cells["PORefNo"].Value = mrnd.PORefNo;
                                grdMRNDetail.Rows[i].Cells["PONo"].Value = mrnd.PONO;
                                grdMRNDetail.Rows[i].Cells["PODate"].Value = mrnd.PODate;
                                grdMRNDetail.Rows[i].Cells["Unit"].Value = mrnd.Unit;
                                grdMRNDetail.Rows[i].Cells["Quantity"].Value = mrnd.Quantity;
                                grdMRNDetail.Rows[i].Cells["ModelNo"].Value = mrnd.ModelNo;
                                grdMRNDetail.Rows[i].Cells["ModelName"].Value = mrnd.ModelName;
                                grdMRNDetail.Rows[i].Cells["gTaxCode"].Value = mrnd.TaxCode;
                                grdMRNDetail.Rows[i].Cells["Price"].Value = mrnd.Price;

                                grdMRNDetail.Rows[i].Cells["Value"].Value = mrnd.Quantity * mrnd.Price;
                                grdMRNDetail.Rows[i].Cells["Tax"].Value = mrnd.Tax;
                                grdMRNDetail.Rows[i].Cells["PriceINR"].Value = mrnd.PriceINR;
                                grdMRNDetail.Rows[i].Cells["TaxINR"].Value = mrnd.TaxINR;
                                grdMRNDetail.Rows[i].Cells["BatchNo"].Value = mrnd.BatchNo;
                                grdMRNDetail.Rows[i].Cells["SerielNo"].Value = mrnd.SerialNo;
                                grdMRNDetail.Rows[i].Cells["ExpiryDate"].Value = mrnd.ExpiryDate;
                                grdMRNDetail.Rows[i].Cells["StoreLocationID"].Value = mrnd.StoreLocationID; // + "-" + mrnd.Description;
                                grdMRNDetail.Rows[i].Cells["QuantityAccepted"].Value = mrnd.QuantityAccepted;
                                grdMRNDetail.Rows[i].Cells["QuantityRejected"].Value = mrnd.QuantityRejected;
                                grdMRNDetail.Rows[i].Cells["TaxDetails"].Value = mrnd.TaxDetails;
                                if (prevmrn.QCStatus == 1 && columnName.Equals("Edit"))
                                {
                                    disableMRNDetailGridColumns();
                                    tabControl1.TabPages["tabMRNHeader"].Enabled = false;

                                }
                                i++;
                                //grdMRNDetail.Columns["OrderedQuantity"].Visible = false;
                                //grdMRNDetail.Columns["OrderedPrice"].Visible = false;
                                //grdMRNDetail.Columns["OrderedTax"].Visible = false;

                                //grdMRNDetail.CellValueChanged
                                //    productvalue = productvalue + popid.Quantity * popid.Price;
                                //    taxvalue = taxvalue + popid.Tax;
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

                if (!verifyAndReworkMRNDetailGridRows())
                {
                    MessageBox.Show("Error found in MRN details. Please correct before updating the details");
                }
                btnSave.Text = "Update";
                pnlList.Visible = false;
                pnlAddEdit.Visible = true;
                tabControl1.SelectedTab = tabMRNHeader;
                tabControl1.Visible = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("error");
            }
        }
        private void disableMRNDetailGridColumns()
        {
            try
            {
                btnAddLine.Enabled = false;
                btnClearEntries.Enabled = false;
                btnCalculate.Enabled = false;

                grdMRNDetail.Columns["Quantity"].ReadOnly = true;
                grdMRNDetail.Columns["BatchNo"].ReadOnly = true;
                grdMRNDetail.Columns["SerielNo"].ReadOnly = true;
                //grdMRNDetail.Columns["ExpiryDate"].ReadOnly = true;
                grdMRNDetail.Columns["StoreLocationID"].ReadOnly = true;
                grdMRNDetail.Columns["Item"].ReadOnly = true;
                grdMRNDetail.Columns["gTaxCode"].ReadOnly = true;
                grdMRNDetail.Columns["Delete"].Visible = false;
                grdMRNDetail.Columns["Sel"].Visible = false;
                grdMRNDetail.Columns["dtButton"].Visible = false;
                grdMRNDetail.Columns["QuantityAccepted"].ReadOnly = false;
                grdMRNDetail.Columns["QuantityRejected"].ReadOnly = false;
                grdMRNDetail.Columns["QuantityAccepted"].Visible = true;
                grdMRNDetail.Columns["QuantityRejected"].Visible = true;
            }
            catch (Exception ex)
            {
            }
        }
        private void enableMRNDetailGridColumns()
        {
            try
            {
                btnAddLine.Enabled = true;
                btnClearEntries.Enabled = true;
                btnCalculate.Enabled = true;
                grdMRNDetail.Columns["Quantity"].ReadOnly = false;
                grdMRNDetail.Columns["BatchNo"].ReadOnly = false;
                grdMRNDetail.Columns["SerielNo"].ReadOnly = false;
                //grdMRNDetail.Columns["ExpiryDate"].ReadOnly = false;
                grdMRNDetail.Columns["StoreLocationID"].ReadOnly = false;
                grdMRNDetail.Columns["Item"].ReadOnly = false;
                grdMRNDetail.Columns["gTaxCode"].ReadOnly = false;
                grdMRNDetail.Columns["Delete"].Visible = true;
                grdMRNDetail.Columns["Sel"].Visible = true;
                grdMRNDetail.Columns["dtButton"].Visible = true;
                grdMRNDetail.Columns["QuantityAccepted"].ReadOnly = true;
                grdMRNDetail.Columns["QuantityRejected"].ReadOnly = true;
                grdMRNDetail.Columns["QuantityAccepted"].Visible = false;
                grdMRNDetail.Columns["QuantityRejected"].Visible = false;
            }
            catch (Exception ex)
            {
            }
        }
        private void btnClearEntries_Click_1(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Are you sure to clear all entries in grid detail ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdMRNDetail.Rows.Clear();
                    MessageBox.Show("Grid items cleared.");
                    verifyAndReworkMRNDetailGridRows();
                }
            }
            catch (Exception)
            {
            }

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
                isValidate = true;
                MRNHeaderDB mrnhdb = new MRNHeaderDB();
                FinancialLimitDB flDB = new FinancialLimitDB();
                //if (!flDB.checkEmployeeFinancialLimit(docID, Login.empLoggedIn, Convert.ToDouble(txtProductValue.Text), 0))
                //{
                //    MessageBox.Show("No financial power for approving this document");
                //    return;
                //}
                List<mrndetail> MRNDetails = getMRNDetails(prevmrn);
                if (prevmrn.QCStatus != 99)
                {
                    MessageBox.Show("Approval fails , MRN is not approved by QC");
                    return;
                }
               
                showQuantityAvailableForAllProductListView(MRNDetails, 2); //opt 2 : approve
                if (!isValidate)
                {
                    ///showQuantityAvailableForAllProductListView(MRNDetails);
                    MessageBox.Show("Enterd Quantity is more than the PO Qunatity. Not Allowed TO Approve.");
                    return;
                }
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {

                    prevmrn.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevmrn.CommentStatus);
                    if (prevmrn.status != 96)
                    {
                        prevmrn.MRNNo = DocumentNumberDB.getNewNumber(docID, 2);
                    }

                    //else
                    //    prevmrn.MRNNo = prevmrn.MRNNo;
                    // prevmrn.MRNDate = DateTime.Now;

                    if (mrnhdb.ApproveMRN(prevmrn))
                    {
                        if (!updateDashBoard(prevmrn, 2))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                        }
                        if (updateStockDetailForMRN())
                        {
                            MessageBox.Show("MRN Document Approved");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredMRNHeader(listOption);
                            setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                        }
                        else
                            MessageBox.Show("Unable to update stock details");
                    }
                    else
                        MessageBox.Show("Unable to approve");
                }
            }
            catch (Exception)
            {
            }

        }
        private Boolean updateStockDetailForMRN()
        {
            Boolean status = false;
            StockDB sdb = new StockDB();
            mrnheader mrnh = MRNHeaderDB.getMRNNoAndDate(prevmrn.TemporaryNo, prevmrn.TemporaryDate);
            prevmrn.MRNNo = mrnh.MRNNo;
            prevmrn.MRNDate = mrnh.MRNDate;
            mrndetail mrnd = new mrndetail();
            List<mrndetail> MRNDetails = new List<mrndetail>();
            try
            {
                for (int i = 0; i < grdMRNDetail.Rows.Count; i++)
                {
                    mrnd = new mrndetail();
                    mrnd.StockItemID = grdMRNDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Substring(0, grdMRNDetail.Rows[i].Cells["Item"].Value.ToString().Trim().IndexOf('-'));
                    mrnd.ModelNo = grdMRNDetail.Rows[i].Cells["ModelNo"].Value.ToString();
                    mrnd.Quantity = Convert.ToDouble(grdMRNDetail.Rows[i].Cells["Quantity"].Value);
                    mrnd.PriceINR = Convert.ToDouble(grdMRNDetail.Rows[i].Cells["PriceINR"].Value);  //updating INR price in stock instead of general price
                    mrnd.TaxINR = Convert.ToDouble(grdMRNDetail.Rows[i].Cells["TaxINR"].Value);     //updating INR Tax in stock instead of general Tax
                    mrnd.TaxDetails = grdMRNDetail.Rows[i].Cells["TaxDetails"].Value.ToString();
                    mrnd.BatchNo = grdMRNDetail.Rows[i].Cells["BatchNo"].Value.ToString();
                    mrnd.SerialNo = grdMRNDetail.Rows[i].Cells["SerielNo"].Value.ToString();
                    mrnd.ExpiryDate = Convert.ToDateTime(grdMRNDetail.Rows[i].Cells["ExpiryDate"].Value);
                    mrnd.StoreLocationID = grdMRNDetail.Rows[i].Cells["StoreLocationID"].Value.ToString();//.Trim().Substring(0, grdMRNDetail.Rows[i].Cells["StoreLocationID"].Value.ToString().Trim().IndexOf('-'));
                    if (grdMRNDetail.Rows[i].Cells["QuantityAccepted"].Value == null)
                    {
                        mrnd.QuantityAccepted = 0;
                    }
                    else
                        mrnd.QuantityAccepted = Convert.ToDouble(grdMRNDetail.Rows[i].Cells["QuantityAccepted"].Value);
                    //string s = grdMRNDetail.Rows[i].Cells["QuantityRejected"].Value.ToString();
                    if (grdMRNDetail.Rows[i].Cells["QuantityRejected"].Value == null)
                    {
                        mrnd.QuantityRejected = 0;
                    }
                    else
                        mrnd.QuantityRejected = Convert.ToDouble(grdMRNDetail.Rows[i].Cells["QuantityRejected"].Value);
                    MRNDetails.Add(mrnd);
                }
                if (sdb.insertStockFromMRN(MRNDetails, prevmrn))
                {
                    status = true;
                    MessageBox.Show("StockItem details Updated");
                }
                else
                {
                    status = false;
                    MessageBox.Show("Fails to update StockItem details.");
                }
            }

            catch (Exception ex)
            {
                status = false;
                MessageBox.Show("Fails to update StockItem details.");
            }
            return status;
        }
        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                customer cust = new customer();
                ////////custid = cmbCustomer.SelectedItem.ToString().Trim().Substring(0, cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-'));

            }
            catch (Exception ex)
            {

            }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab == tabMRNDetail)
                {
                    validateItems();
                    mrnheader mh = new mrnheader();
                    MRNHeaderDB mhdb = new MRNHeaderDB();
                    mrnheader mrnh = fillMRNHeader(mh);
                    if (!mhdb.validateMRNHeader(mrnh, 1) && !isViewMode)
                    {
                        tabMRNDetail.Enabled = false;
                        return;
                    }
                }
                if (tabControl1.SelectedTab == tabMRNHeader)
                {
                    tabMRNDetail.Enabled = true;
                }
                if (approveCellClick)
                {
                    tabControl1.TabPages["tabMRNHeader"].Enabled = false;
                    tabControl1.TabPages["tabMRNDetail"].Enabled = false;
                    //tabMRNHeader.
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
                            dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevmrn.TemporaryNo + "-" + prevmrn.TemporaryDate.ToString("yyyyMMddhhmmss"));
                            dgvDocumentList.Size = new Size(870, 300);
                            pnlPDFViewer.Controls.Add(dgvDocumentList);
                            dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
                            if (prevmrn.status == 1 && prevmrn.DocumentStatus == 99)
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
            //docCmtrDB = new DocCommenterDB();
            //lvCmtr = new ListView();
            //lvCmtr.Clear();
            //pnlCmtr.BorderStyle = BorderStyle.FixedSingle;
            //pnlCmtr.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
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
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        private Boolean updateDashBoard(mrnheader mrnh, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = mrnh.DocumentID;
                dsb.TemporaryNo = mrnh.TemporaryNo;
                dsb.TemporaryDate = mrnh.TemporaryDate;
                dsb.DocumentNo = mrnh.MRNNo;
                dsb.DocumentDate = mrnh.MRNDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = mrnh.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(prevmrn.DocumentID);
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
                            MRNHeaderDB mrnhdb = new MRNHeaderDB();
                            prevmrn.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevmrn.CommentStatus);
                            prevmrn.ForwardUser = approverUID;
                            prevmrn.ForwarderList = prevmrn.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (mrnhdb.forwardMRN(prevmrn))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(prevmrn, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredMRNHeader(listOption);
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
                    string s = prevmrn.ForwarderList;
                    string reverseStr = getReverseString(prevmrn.ForwarderList);
                    //do forward activities
                    prevmrn.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevmrn.CommentStatus);
                    MRNHeaderDB mrnhdb = new MRNHeaderDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevmrn.ForwarderList = reverseStr.Substring(0, ind);
                        prevmrn.ForwardUser = reverseStr.Substring(ind + 3);
                        prevmrn.DocumentStatus = prevmrn.DocumentStatus - 1;
                    }
                    else
                    {
                        prevmrn.ForwarderList = "";
                        prevmrn.ForwardUser = "";
                        prevmrn.DocumentStatus = 1;
                    }
                    //check for any transaction on this MRN
                    //////if (!mrnhdb.checkForIssuesFromMRN(prevmrn))
                    //////{
                    //////    //no issues (purchase return and Invoice) from MRN

                    //////}
                    //////else
                    //////{
                    //////    MessageBox.Show("Stock has been issued from MRN. Reversal request denied");
                    //////}
                    if (mrnhdb.reverseMRN(prevmrn))
                    {
                        //Reverse the stock accounted, if no stock is issued from the MRN
                        MessageBox.Show("MRN Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredMRNHeader(listOption);
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
        private void removeControlsFromPnllvPanel()
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
        private void btnListDocuments_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevmrn.TemporaryNo + "-" + prevmrn.TemporaryDate.ToString("yyyyMMddhhmmss"));
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
                    string subDir = prevmrn.TemporaryNo + "-" + prevmrn.TemporaryDate.ToString("yyyyMMddhhmmss");
                    dgv.Enabled = false;
                    ////DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    fileName = DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    ////showPDFFile(fileName);
                    ////dgv.Visible = false;
                    System.Diagnostics.Process.Start(fileName);
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
                                tabControl1.SelectedIndex = 3;
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
                btnQC.Visible = false;
                btnQCCompleted.Visible = false;
                btnGetComments.Visible = false;
                /////chkCommentStatus.Visible = false;
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
                    ///chkCommentStatus.Visible = true;
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
                    tabControl1.SelectedTab = tabMRNHeader;
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
                    tabControl1.SelectedTab = tabMRNHeader;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                    btnQC.Visible = true;
                    disableTabPages();

                    tabControl1.SelectedTab = tabMRNHeader;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabMRNHeader;
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
                //removeControlsFromCommenterPanel();
                //removeControlsFromForwarderPanel();
                removeControlsFromPnllvPanel();
                dgvComments.Rows.Clear();
                ////chkCommentStatus.Checked = false;
                txtComments.Text = "";
                grdMRNDetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSelectPO_Click(object sender, EventArgs e)
        {
            if (txtCustomerID.Text.Trim().Length == 0)
            {
                MessageBox.Show("select one customer");
                return;
            }
            if (txtReference.Text.Trim().Length != 0)
            {
                DialogResult dialog = MessageBox.Show("Warning: \nReference will be removed", "", MessageBoxButtons.OKCancel);
                if (dialog == DialogResult.OK)
                    txtReference.Text = "";
                else
                    return;
            }
            if (txtPONos.Text.Length != 0 || grdMRNDetail.Rows.Count > 0)
            {
                DialogResult dialog = MessageBox.Show("Warning:\nPONo , PODate,TaxCode,MRNValue, Product Value and MRN detail will be removed", "", MessageBoxButtons.OKCancel);
                if (dialog == DialogResult.OK)
                {
                    grdMRNDetail.Rows.Clear();
                    txtMRNValue.Text = "";
                    txtTaxAmount.Text = "";
                    txtProductValue.Text = "";
                }
                else
                    return;
            }
            //btnSelectPO.Enabled = false;
            //removeControlsFromPnllvPanel();
            //pnllv = new Panel();
            //pnllv.BorderStyle = BorderStyle.FixedSingle;

            //pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(900, 400);
            lv = PurchaseOrderDB.PODetailForMRN(custid);
            ///this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(900, 350));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 365);
            lvOK.Click += new System.EventHandler(this.lvOK_Click1);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 365);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click1);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
            //pnlAddEdit.Controls.Add(pnllv);
            //pnllv.BringToFront();
            //pnllv.Visible = true;
        }
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                //btnSelectPO.Enabled = true;
                //pnllv.Visible = false;
                int kount = 0;
                int n = 0;
                string poNos = ";";
                string poDates = ";";
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        kount++;
                    }
                }
                if (kount == 0)
                {
                    MessageBox.Show("Select one PO");
                    return;
                }
                else
                {
                    foreach (ListViewItem itemRow in lv.Items)
                    {

                        if (itemRow.Checked)
                        {
                            n++;
                            poNos = poNos + itemRow.SubItems[3].Text + ";";
                            poDates = poDates + itemRow.SubItems[4].Text + ";";
                            btnPOProdValue.Text = itemRow.SubItems[6].Text;
                            btnPOTaxValue.Text = itemRow.SubItems[7].Text;
                            if (n == 1)
                            {
                                cmbCurrency.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCurrency, itemRow.SubItems[8].Text);
                                txtINRInversionRate.Text = itemRow.SubItems[9].Text;
                            }
                            btnPOProdValue.Visible = true;
                            btnPOTaxValue.Visible = true;
                            lblPOpProdValue.Visible = true;
                            lblPOTaxValue.Visible = true;
                            frmPopup.Close();
                            frmPopup.Dispose();
                            //AddGridDetailRowForPO();
                        }
                    }
                }
                txtPONos.Text = poNos;
                txtPODates.Text = poDates;
            }
            catch (Exception ex)
            {
            }
        }

        private void lvCancel_Click1(object sender, EventArgs e)
        {
            try
            {
                //btnSelectPO.Enabled = true;
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
        //            grdMRNDetail.Rows.Add();
        //            grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["LineNo"].Value = grdMRNDetail.RowCount;
        //            DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["Item"]);
        //            grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["Item"].Value = PurchaseOrderDB.fillMRStockItemGridViewComboWithValue(ComboColumn2, PODetail, grdMRNDetail.RowCount);
        //            grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["OrderedQuantity"].Value = pod.Quantity;
        //            grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["Unit"].Value = pod.Unit;
        //            grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["OrderedPrice"].Value = pod.Price;
        //            grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["OrderedTax"].Value = pod.Tax;
        //            grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["Quantity"].Value = "";
        //            grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["Price"].Value = pod.Price;
        //            grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["Value"].Value = "";
        //            grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["Tax"].Value = "";
        //            grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["BatchNo"].Value = "";
        //            grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["SerielNo"].Value = "";
        //            grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["ExpiryDate"].Value = "";
        //            DataGridViewComboBoxCell ComboColumn1 = (DataGridViewComboBoxCell)(grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["StoreLocationID"]);
        //            CatalogueValueDB.fillCatalogValueGridViewCombo(ComboColumn1, "StoreLocation");
        //            grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["QuantityAccepted"].Value = 0;
        //            grdMRNDetail.Rows[grdMRNDetail.RowCount - 1].Cells["QuantityRejected"].Value = 0;
        //            grdMRNDetail.Columns["OrderedQuantity"].Visible = true;
        //            grdMRNDetail.Columns["OrderedPrice"].Visible = true;
        //            grdMRNDetail.Columns["OrderedTax"].Visible = true;
        //            grdMRNDetail.Columns["QuantityAccepted"].Visible = false;
        //            grdMRNDetail.Columns["QuantityRejected"].Visible = false;
        //        }
        //    }
        //    else
        //    {
        //        AddPOPIDetailRow();
        //    }
        //}

        private void cmbCustomer_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void btnQC_Click(object sender, EventArgs e)
        {
            //removeControlsFromPnllvPanel();
            //pnllv = new Panel();
            //pnllv.BorderStyle = BorderStyle.FixedSingle;
            ////btnQC.Enabled = false;
            //pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            string DeptList = "Production" + Main.delimiter1 + "QC";
            lv = EmployeePostingDB.EmpListViewForQC(DeptList, "MRN");
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked1);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Click2);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
            //pnlAddEdit.Controls.Add(pnllv);
            //pnllv.BringToFront();
            //pnllv.Visible = true;
        }
        private void lvOK_Click2(object sender, EventArgs e)
        {
            try
            {
                if (!checkLVItemChecked("QC"))
                {
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        DialogResult dialog = MessageBox.Show("Are you sure to forward the document ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            if (prevmrn.DocumentStatus != 99)
                            {
                                string approverUID = itemRow.SubItems[3].Text;
                                string approverUName = itemRow.SubItems[2].Text;
                                MRNHeaderDB mrnhdb = new MRNHeaderDB();
                                prevmrn.ForwardUser = approverUID;
                                prevmrn.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevmrn.CommentStatus);
                                prevmrn.QCStatus = 1;
                                prevmrn.ForwarderList = prevmrn.ForwarderList + approverUName + Main.delimiter1 +
                                    approverUID + Main.delimiter1 + Main.delimiter2;
                                if (mrnhdb.forwardMRN(prevmrn))
                                {
                                    frmPopup.Close();
                                    frmPopup.Dispose();
                                    MessageBox.Show("Document Forwarded");
                                    if (!updateDashBoard(prevmrn, 1))
                                    {
                                        MessageBox.Show("DashBoard Fail to update");
                                    }
                                    closeAllPanels();
                                    listOption = 1;
                                    ListFilteredMRNHeader(listOption);
                                    setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                                }
                            }
                            else
                                MessageBox.Show("Document can not be forward for QC as It already Approved");
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
                //btnQC.Enabled = true;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
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

        private void btnQCCompleted_Click(object sender, EventArgs e)
        {
            try
            {
                List<mrndetail> MRNDetails = getMRNDetails(prevmrn);
                if (!verifyAcceptedAndRejectedQuant(MRNDetails))
                {
                    return;
                }
                DialogResult dialog = MessageBox.Show("Are you sure to send the document back?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    string s = prevmrn.ForwarderList;
                    string reverseStr = getReverseString(prevmrn.ForwarderList);
                    //do forward activities
                    prevmrn.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevmrn.CommentStatus);
                    MRNHeaderDB mrnhdb = new MRNHeaderDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevmrn.ForwarderList = reverseStr.Substring(0, ind);
                        prevmrn.ForwardUser = reverseStr.Substring(ind + 3);
                        prevmrn.DocumentStatus = prevmrn.DocumentStatus - 1;
                        prevmrn.QCStatus = 99;
                    }
                    else
                    {
                        prevmrn.ForwarderList = "";
                        prevmrn.ForwardUser = "";
                        prevmrn.DocumentStatus = 1;
                        prevmrn.QCStatus = 99;
                    }
                    if (mrnhdb.reverseMRN(prevmrn))
                    {
                        ////////no stock reversal required
                        MessageBox.Show("MRN forwarded to stock custodian");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredMRNHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (cmbCurrency.SelectedIndex == -1 || txtINRInversionRate.Text == null || Convert.ToDouble(txtINRInversionRate.Text) == 0)
            {
                MessageBox.Show("Currency ID/Exchange Rate is empty");
                return;
            }
            verifyAndReworkMRNDetailGridRows();
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
        int a = 0;
        private void grdMRNDetail_EditingControlShowing_1(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            a = 0;
            DataGridView dgv = sender as DataGridView;
            DataGridViewColumn dgvCol = dgv.CurrentCell.OwningColumn;
            if (dgvCol.Name == "StoreLocationID")
            {
                a = 1;
            }
            if (dgvCol.Name == "Item")
            {
                if (e.Control is ComboBox)
                {
                    ComboBox combo = e.Control as ComboBox;
                    if (combo != null)
                    {
                        combo.SelectedIndexChanged -= new EventHandler(ComboBox_SelectedIndexChanged);
                        combo.SelectedIndexChanged += new EventHandler(ComboBox_SelectedIndexChanged);
                    }
                }
            }
        }
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (a != 1)
                {
                    ComboBox cb = (ComboBox)sender;
                    string item = cb.Text;
                    string ItemID = item.Substring(0, item.IndexOf('-'));
                    string ItemName = item.Substring(item.IndexOf('-') + 1);
                    if (item != null)
                    {
                        grdMRNDetail.Rows[grdMRNDetail.CurrentRow.Index].Cells["Unit"].Value = StockItemDB.getUnitForSelectedStockItem(ItemID, ItemName);
                    }
                }
            }
            catch (Exception ex)
            {
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
        private ListView getStockAvailabilityWRTPOListView(List<mrndetail> MRNList, int opt)
        {
            ListView lv = new ListView();
            try
            {

                lv.View = System.Windows.Forms.View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                //lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;

                lv.Columns.Add("PO No", -2, HorizontalAlignment.Center);
                lv.Columns.Add("PO Date", -2, HorizontalAlignment.Center);
                lv.Columns.Add("StockItem ID", -2, HorizontalAlignment.Center);
                lv.Columns.Add("StockItem Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Model No", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Model Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("PO Quant", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Received Quant(A)", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Present Quant(B)", -2, HorizontalAlignment.Center);
                lv.Columns.Add("A + B", -2, HorizontalAlignment.Center);
                lv.Columns[2].Width = 0;
                lv.Columns[3].Width = 200;
                lv.Columns[4].Width = 0;
                lv.Columns[5].Width = 150;
                foreach (mrndetail mrnd in MRNList)
                {
                    mrndetail mrndEq = MRNHeaderDB.getItemWiseTotalQuantForPerticularPOInMRN(mrnd.PORefNo);
                    ///double poQuant = PurchaseOrderDB.getPOQuantityFor(mrnd.PONO, mrnd.PODate, mrnd.StockItemID, mrnd.ModelNo);
                    double poQuant = PurchaseOrderDB.getPOQuantityFor(mrnd.PORefNo);
                    ListViewItem item1 = new ListViewItem(mrnd.PONO.ToString());
                    item1.SubItems.Add(mrnd.PODate.ToShortDateString());
                    item1.SubItems.Add(mrnd.StockItemID);
                    item1.SubItems.Add(mrnd.StockItemName);
                    item1.SubItems.Add(mrnd.ModelNo);
                    item1.SubItems.Add(mrnd.ModelName);
                    item1.SubItems.Add(poQuant.ToString());          //For PO Quantity
                    item1.SubItems.Add(mrndEq.Quantity.ToString());  // For Total Accepted Quantity In All MRN for perticular Item
                    if (opt == 1) //For saving
                    {
                        item1.SubItems.Add(mrnd.Quantity.ToString());    //For Accepted Quantity IN Current MRN
                        item1.SubItems.Add((mrnd.Quantity + mrndEq.Quantity).ToString());
                        if ((mrndEq.Quantity + mrnd.Quantity) > poQuant)
                        {
                            isValidate = false;
                            item1.BackColor = Color.Magenta;
                        }
                    }
                    else //For approve
                    {
                        item1.SubItems.Add(mrnd.QuantityAccepted.ToString());    //For Accepted Quantity IN Current MRN
                        item1.SubItems.Add((mrnd.QuantityAccepted + mrndEq.Quantity).ToString());
                        if ((mrndEq.Quantity + mrnd.QuantityAccepted) > poQuant)
                        {
                            isValidate = false;
                            item1.BackColor = Color.Magenta;
                        }
                    }
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        private void showQuantityAvailableForAllProductListView(List<mrndetail> MRNList, int opt)
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
                lv = getStockAvailabilityWRTPOListView(MRNList, opt);
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

        //private Boolean validateMRNDetailProductquantity(List<mrndetail> mrnList)
        //{
        //    Boolean stat = true;
        //    foreach(mrndetail mrn in mrnList)
        //    {
        //        mrndetail mrnd = MRNHeaderDB.getItemWiseTotalQuantForPerticularPOInMRN(mrn.PORefNo);
        //        //double PoQuant = PurchaseOrderDB.getPOQuantityFor(mrn.PONO,mrn.PODate,mrn.StockItemID,mrn.ModelNo);
        //        double PoQuant = PurchaseOrderDB.getPOQuantityFor(mrn.PORefNo);
        //        double totQuant = mrnd.Quantity;
        //        if((totQuant+mrn.QuantityAccepted) > PoQuant)
        //        {
        //            return false;
        //        }
        //    }

        //    return stat;
        //}

        //-- Validating Header and Detail String For Single Quotes

        private mrnheader verifyHeaderInputString(mrnheader mrnh)
        {
            try
            {
                mrnh.Remarks = Utilities.replaceSingleQuoteWithDoubleSingleQuote(mrnh.Remarks);
                mrnh.Reference = Utilities.replaceSingleQuoteWithDoubleSingleQuote(mrnh.Reference);
                mrnh.CustomerID = Utilities.replaceSingleQuoteWithDoubleSingleQuote(mrnh.CustomerID);
                mrnh.DCNo = Utilities.replaceSingleQuoteWithDoubleSingleQuote(mrnh.DCNo);
                mrnh.InvoiceNo = Utilities.replaceSingleQuoteWithDoubleSingleQuote(mrnh.InvoiceNo);
                mrnh.TransporterName = Utilities.replaceSingleQuoteWithDoubleSingleQuote(mrnh.TransporterName);
            }
            catch (Exception ex)
            {
            }
            return mrnh;
        }
        private void verifyDetailInputString()
        {
            try
            {
                for (int i = 0; i < grdMRNDetail.Rows.Count; i++)
                {
                    grdMRNDetail.Rows[i].Cells["Item"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdMRNDetail.Rows[i].Cells["Item"].Value.ToString());
                    grdMRNDetail.Rows[i].Cells["BatchNo"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdMRNDetail.Rows[i].Cells["BatchNo"].Value.ToString());
                    grdMRNDetail.Rows[i].Cells["SerielNo"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdMRNDetail.Rows[i].Cells["SerielNo"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void txtINRInversionRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdMRNDetail.Rows.Count != 0 && txtMRNValue.Text.Length != 0
                    && txtProductValue.Text.Length != 0 && txtINRInversionRate.Text.Length != 0)
                {
                    double dd = Convert.ToDouble(txtINRInversionRate.Text);
                    txtProductValueINR.Text = (Convert.ToDouble(txtProductValue.Text) * dd).ToString();
                    txtMRNValueINR.Text = (Convert.ToDouble(txtMRNValue.Text) * dd).ToString();
                    txtTaxAmountINR.Text = (Convert.ToDouble(txtTaxAmount.Text) * dd).ToString();
                }
                if (txtINRInversionRate.Text.Length == 0)
                {
                    txtProductValueINR.Text = "";
                    txtMRNValueINR.Text = "";
                    txtTaxAmountINR.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private void MRNHeader_Enter(object sender, EventArgs e)
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

        private void txtSearchMaingrid_TextChanged(object sender, EventArgs e)
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
            filterGridData(colName);
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }
        private void filterGridData(string clm)
        {
            try
            {
                foreach (DataGridViewRow row in grdList.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearchMaingrid.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdList.Rows)
                    {
                        if (clm == null || clm.Length == 0)
                        {
                            if (!row.Cells["Reference"].Value.ToString().Trim().ToLower().Contains(txtSearchMaingrid.Text.Trim().ToLower()))
                            {
                                row.Visible = false;
                            }
                        }
                        else
                        {

                            if (!row.Cells[clm].FormattedValue.ToString().Trim().ToLower().Contains(txtSearchMaingrid.Text.Trim().ToLower()))
                            {
                                row.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void grdList_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                colName = grdList.Columns[e.ColumnIndex].Name;
                foreach (DataGridViewColumn col in grdList.Columns)
                {
                    col.HeaderCell.Style.BackColor = Color.LightBlue;
                }
                grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdList.Columns[e.ColumnIndex].HeaderCell.Style.BackColor = Color.Magenta;
            }
            catch (Exception ex)
            {
            }
        }
        private void btnSelCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCustomerID.Text.Trim().Length != 0)
                {
                    DialogResult dialog = MessageBox.Show("Warning:\nPONo , PODate,TaxCode,MRNValue, Product Value and MRN detail will be removed", "", MessageBoxButtons.OKCancel);
                    if (dialog == DialogResult.OK)
                    {
                        txtPONos.Text = "";
                        txtPODates.Text = "";
                        //cmbTaxCode.SelectedIndex = -1;
                        txtMRNValue.Text = "";
                        txtTaxAmount.Text = "";
                        txtProductValue.Text = "";
                        btnPOProdValue.Text = "";
                        btnPOTaxValue.Text = "";
                        grdMRNDetail.Rows.Clear();
                        //track = true;
                    }
                    else
                        return;
                }
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.CadetBlue;

                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(800, 400);

                chkBoxCustomer = new CheckedListBox();
                chkBoxCustomer.BackColor = System.Drawing.SystemColors.InactiveCaption;
                chkBoxCustomer.ColumnWidth = 80;
                chkBoxCustomer.FormattingEnabled = true;
                chkBoxCustomer.Items.AddRange(new object[] { "Customer", "Supplier", "Contractor", "Transporter", "Others" });
                chkBoxCustomer.Location = new System.Drawing.Point(69, 22);
                chkBoxCustomer.MultiColumn = true;
                chkBoxCustomer.SetItemChecked(1, true);
                chkBoxCustomer.Name = "chkBoxCustomer";
                chkBoxCustomer.Size = new System.Drawing.Size(446, 19);
                chkBoxCustomer.Location = new Point(10, 10);
                chkBoxCustomer.CheckOnClick = true;
                chkBoxCustomer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkBoxCustomer_MouseUp);
                frmPopup.Controls.Add(chkBoxCustomer);

                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(340, 38);
                lblSearch.Text = "Search by Name";
                lblSearch.AutoSize = true;
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearchCust = new TextBox();
                txtSearchCust.Size = new Size(220, 18);
                txtSearchCust.Location = new System.Drawing.Point(460, 36);
                txtSearchCust.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearchCust.ForeColor = Color.Black;
                txtSearchCust.TextChanged += new System.EventHandler(this.txtCustSearch_TextChangedInEmpGridList);
                txtSearchCust.TabIndex = 0;
                txtSearchCust.Focus();
                frmPopup.Controls.Add(txtSearchCust);

                CustomerDB custDB = new CustomerDB();
                grdCustList = custDB.getGridViewForCustomerListNew("Supplier");

                grdCustList.Bounds = new Rectangle(new Point(0, 60), new Size(800, 300));
                frmPopup.Controls.Add(grdCustList);

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 370);
                lvOK.Click += new System.EventHandler(this.grdCustOK_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 370);
                lvCancel.Click += new System.EventHandler(this.grdCustCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdCustOK_Click1(object sender, EventArgs e)
        {
            try
            {
                var checkedRows = from DataGridViewRow r in grdCustList.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("Select one Customer");
                    return;
                }
                string trlist;
                trlist = "";
                foreach (var row in checkedRows)
                {
                    customer cust = new customer();
                    txtCustomerID.Text = row.Cells["ID"].Value.ToString();
                    txtCustName.Text = row.Cells["Name"].Value.ToString();
                    custid = txtCustomerID.Text;
                    //cust = CustomerDB.getCustomerDetails(txtCustomerID.Text);
                    //txtBillingAddress.Text = cust.BillingAddress;
                    //txtDeliveryAddress.Text = cust.DeliveryAddress;
                    //string custid = row.Cells["CustomerID"].Value.ToString();
                    //customer cust = CustomerDB.getCustomerDetails(custid);
                    //string AddCust = cust.BillingAddress;
                    //txtDeliveryAddress.Text = AddCust;
                }

                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        private void grdCustCancel_Click1(object sender, EventArgs e)
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
        private void txtCustSearch_TextChangedInEmpGridList(object sender, EventArgs e)
        {
            try
            {
                filterTimer1.Enabled = false;
                filterTimer1.Stop();
                filterTimer1.Tick -= new System.EventHandler(this.handlefilterTimerTimeout1);
                filterTimer1.Tick += new System.EventHandler(this.handlefilterTimerTimeout1);
                filterTimer1.Interval = 500;
                filterTimer1.Enabled = true;
                filterTimer1.Start();

            }
            catch (Exception ex)
            {

            }
        }
        private void handlefilterTimerTimeout1(object sender, EventArgs e)
        {
            filterTimer1.Enabled = false;
            filterTimer1.Stop();
            filterCustGridData();
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }
        private void filterCustGridData()
        {
            try
            {
                grdCustList.CurrentCell = null;
                foreach (DataGridViewRow row in grdCustList.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearchCust.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdCustList.Rows)
                    {
                        if (!row.Cells["Name"].Value.ToString().Trim().ToLower().Contains(txtSearchCust.Text.Trim().ToLower()))
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
        private void chkBoxCustomer_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                string checkedStr = "";
                txtSearchCust.Text = "";
                int n = 1;
                foreach (var item in chkBoxCustomer.CheckedItems)
                {
                    if (chkBoxCustomer.CheckedItems.Count == n)
                        checkedStr = checkedStr + item.ToString();
                    else
                        checkedStr = checkedStr + item.ToString() + Main.delimiter1;
                    n++;
                }
                frmPopup.Controls.Remove(grdCustList);
                CustomerDB custDB = new CustomerDB();
                grdCustList = custDB.getGridViewForCustomerListNew(checkedStr);

                grdCustList.Bounds = new Rectangle(new Point(0, 60), new Size(800, 300));
                frmPopup.Controls.Add(grdCustList);
            }
            catch (Exception ex)
            {
            }
        }
    }
}



