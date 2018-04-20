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
using CSLERP.PrintForms;
using CSLERP.FileManager;

namespace CSLERP
{
    public partial class InvoiceOutHeader : System.Windows.Forms.Form
    {
        string docID = "";
        Boolean isViewMode = false;
        string forwarderList = "";
        string approverList = "";
        decimal TotalToAdjust = 0;
        string userString = "";
        RichTextBox txtDesc = new RichTextBox();
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        double productvalue = 0.0;
        double taxvalue = 0.0;
        TextBox tb = new TextBox();
        Boolean isNewClicked = false;
        DataGridView voucherGrid = new DataGridView();
        Boolean userIsACommenter = false;
        string chkTax = "";
        DataGridView grdCustList = new DataGridView();
        string custid = "";
        Boolean track = false;
        List<invoiceoutreceipts> receiveList = new List<invoiceoutreceipts>();
        string colName = "";
        CheckedListBox chkBoxCustomer = new CheckedListBox();
        TextBox txtSearchCust = new TextBox();
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        invoiceoutheader previoh;
        string POStockinfo = "";
        ListView lvTot = new ListView();
        Timer filterTimer = new Timer();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        Form frmPopup = new Form();
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        string PODocIdWRTIO = "";
        DataGridView projGRid = new DataGridView();
        Boolean AddRowClick = false;
        public InvoiceOutHeader()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void InvoiceOutHeader_Load(object sender, EventArgs e)
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
            ListFilteredInvoiceOutHeader(listOption);
        }
        private void ListFilteredInvoiceOutHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                InvoiceOutHeaderDB iohdb = new InvoiceOutHeaderDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);

                List<invoiceoutheader> IOHeaders = iohdb.getFilteredInvoiceOutHeader(userString, option, userCommentStatusString);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                else if (option == 4)
                    lblActionHeader.Text = "List of Cancelled Documents";
                foreach (invoiceoutheader ioh in IOHeaders)
                {
                    if (option == 1)
                    {
                        if (ioh.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = ioh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["RowID"].Value = ioh.RowID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = ioh.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = ioh.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = ioh.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["InvoiceNo"].Value = ioh.InvoiceNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["InvoiceDate"].Value = ioh.InvoiceDate;

                    grdList.Rows[grdList.RowCount - 1].Cells["PONo"].Value = ioh.TrackingNos;
                    grdList.Rows[grdList.RowCount - 1].Cells["PODate"].Value = ioh.TrackingDates;
                    grdList.Rows[grdList.RowCount - 1].Cells["ConsigneeID"].Value = ioh.ConsigneeID;
                    grdList.Rows[grdList.RowCount - 1].Cells["ConsigneeName"].Value = ioh.ConsigneeName;
                    grdList.Rows[grdList.RowCount - 1].Cells["TermsOfPayment"].Value = ioh.TermsOfPayment;
                    grdList.Rows[grdList.RowCount - 1].Cells["Description"].Value = ioh.Description;
                    grdList.Rows[grdList.RowCount - 1].Cells["TransportationMode"].Value = ioh.TransportationMode;
                    grdList.Rows[grdList.RowCount - 1].Cells["TransportationModeName"].Value = ioh.TransportationModeName;
                    grdList.Rows[grdList.RowCount - 1].Cells["TransportationType"].Value = ioh.TransportationType;
                    grdList.Rows[grdList.RowCount - 1].Cells["TransportationTypeName"].Value = ioh.TransportationTypeName;
                    grdList.Rows[grdList.RowCount - 1].Cells["BankAcReference"].Value = ioh.BankAcReference;
                    grdList.Rows[grdList.RowCount - 1].Cells["Transporter"].Value = ioh.Transporter;
                    grdList.Rows[grdList.RowCount - 1].Cells["TransporterName"].Value = ioh.TransporterName;

                    grdList.Rows[grdList.RowCount - 1].Cells["gDeliveryAddress"].Value = ioh.DeliveryAddress;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStateCode"].Value = ioh.DeliveryStateCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["SpecialNote"].Value = ioh.SpecialNote;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyID"].Value = ioh.CurrencyID;
                    //grdList.Rows[grdList.RowCount - 1].Cells["gTaxCode"].Value = ioh.TaxCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProjectID"].Value = ioh.ProjectID;
                    grdList.Rows[grdList.RowCount - 1].Cells["INRConversionRate"].Value = ioh.INRConversionRate;
                    grdList.Rows[grdList.RowCount - 1].Cells["ADCode"].Value = ioh.ADCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["EntryPort"].Value = ioh.EntryPort;
                    grdList.Rows[grdList.RowCount - 1].Cells["ExitPort"].Value = ioh.ExitPort;
                    grdList.Rows[grdList.RowCount - 1].Cells["FinalDestinationCountryID"].Value = ioh.FinalDestinatoinCountryID;
                    grdList.Rows[grdList.RowCount - 1].Cells["FinalDestinationCountryName"].Value = ioh.FinalDestinatoinCountryName;
                    grdList.Rows[grdList.RowCount - 1].Cells["OriginCountryID"].Value = ioh.OriginCountryID;
                    grdList.Rows[grdList.RowCount - 1].Cells["OriginCountryName"].Value = ioh.OriginCountryName;
                    grdList.Rows[grdList.RowCount - 1].Cells["FinalDestinationPlace"].Value = ioh.FinalDestinationPlace;
                    grdList.Rows[grdList.RowCount - 1].Cells["PreCarriageTransportationMode"].Value = ioh.PreCarriageTransportationMode;
                    grdList.Rows[grdList.RowCount - 1].Cells["PreCarriageTransportationName"].Value = ioh.PreCarriageTransportationName;
                    grdList.Rows[grdList.RowCount - 1].Cells["PreCarrierReceiptPlace"].Value = ioh.PreCarrierReceiptPlace;
                    grdList.Rows[grdList.RowCount - 1].Cells["termsOfDelivery"].Value = ioh.TermsOfDelivery;
                    grdList.Rows[grdList.RowCount - 1].Cells["gRemarks"].Value = ioh.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = ioh.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["CmtStatus"].Value = ioh.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = ioh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwardUser"].Value = ioh.ForwardUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ApproveUser"].Value = ioh.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = ioh.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = ioh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = ioh.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = ioh.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = ioh.ForwarderList;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = ioh.status;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatus"].Value = ioh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProductValue"].Value = ioh.ProductValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["TaxAmount"].Value = ioh.TaxAmount;
                    grdList.Rows[grdList.RowCount - 1].Cells["InvoiceAmount"].Value = ioh.InvoiceAmount;
                    grdList.Rows[grdList.RowCount - 1].Cells["INRAmount"].Value = ioh.INRAmount;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProductValueINR"].Value = ioh.ProductValueINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["TaxAmountINR"].Value = ioh.TaxAmountINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["FreightCharge"].Value = ioh.FreightCharge;
                    grdList.Rows[grdList.RowCount - 1].Cells["ReverseCharge"].Value = ioh.ReverseCharge;

                    grdList.Rows[grdList.RowCount - 1].Cells["SJVTNo"].Value = ioh.SJVTNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["SJVTDate"].Value = ioh.SJVTDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["SJVNo"].Value = ioh.SJVNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["SJVDate"].Value = ioh.SJVDate;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Invoice Out Listing");
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
            lblAdvTotal.Text = "";
            docID = Main.currentDocument;
            CatalogueValueDB.fillCatalogValueComboNew(cmbTransportationMode, "TransportationMode");
            CatalogueValueDB.fillCatalogValueComboNew(cmbTransportationType, "TransporterType");
            CatalogueValueDB.fillCatalogValueComboNew(cmbInvoiceType, "InvoiceType");
            StateDB.fillStateComboNew(cmbStateCode);
            CustomerDB.fillLedgerTypeComboNew(cmbTransporter, "Transporter");
            CurrencyDB.fillCurrencyComboNew(cmbCurrency);
            CatalogueValueDB.fillCatalogValueComboNew(cmbOriginCountryID, "Country");
            CatalogueValueDB.fillCatalogValueComboNew(cmbFinalDestinationCountryID, "Country");
            CatalogueValueDB.fillCatalogValueComboNew(cmbPreCarriageTransMode, "TransportationMode");
            CatalogueValueDB.fillCatalogValueComboNew(cmbADCode, "ADCODES");
            dtTempDate.Format = DateTimePickerFormat.Custom;
            dtTempDate.CustomFormat = "dd-MM-yyyy";
            dtTempDate.Enabled = false;
            dtInvoiceDate.Format = DateTimePickerFormat.Custom;
            dtInvoiceDate.CustomFormat = "dd-MM-yyyy";
            dtInvoiceDate.Enabled = false;
            cmbRevCharge.SelectedIndex = cmbRevCharge.FindString("No");
            cmbCurrency.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCurrency, "INR");
            txtTemporarryNo.Enabled = false;
            dtTempDate.Enabled = false;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            isViewMode = false;
            //create tax details table for tax breakup display
            {
                TaxDetailsTable.Columns.Add("TaxItem", typeof(string));
                TaxDetailsTable.Columns.Add("TaxAmount", typeof(double));
            }
            txtPOTrackNos.TabIndex = 1;
            cmbCurrency.TabIndex = 3;
            grdInvoiceOutDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //---
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
            txtInvoiceNo.TabIndex = 2;
            dtInvoiceDate.TabIndex = 3;
            btnSelCustomer.TabIndex = 4;
            txtPOTrackNos.TabIndex = 5;
            btnSelectPOInwardNo.TabIndex = 6;
            txtPOTrackDates.TabIndex = 7;
            cmbTransportationMode.TabIndex = 8;
            cmbTransportationType.TabIndex = 9;
            cmbTransporter.TabIndex = 10;
            txtPaymentTerms.TabIndex = 11;
            txtBankRefNo.TabIndex = 12;
            btnBankAcRefNo.TabIndex = 13;
            btnSelProjectID.TabIndex = 14;
            cmbCurrency.TabIndex = 15;
            txtINRInversionRate.TabIndex = 16;
            txtFreightCharge.TabIndex = 17;
            cmbRevCharge.TabIndex = 18;
            txtDeliveryAddress.TabIndex = 19;
            cmbStateCode.TabIndex = 20;
            txtRemarks.TabIndex = 21;
            txtSpecialNote.TabIndex = 22;

            cmbOriginCountryID.TabIndex = 0;
            cmbFinalDestinationCountryID.TabIndex = 1;
            txtFinalDestinationPlace.TabIndex = 2;
            cmbPreCarriageTransMode.TabIndex = 3;
            txtPrecarrierReceivedPlace.TabIndex = 4;
            txtTermsOfDelivery.TabIndex = 5;
            txtEntryPort.TabIndex = 6;
            txtExitPort.TabIndex = 7;
            cmbADCode.TabIndex = 8;

            btnForward.TabIndex = 0;
            btnApprove.TabIndex = 1;
            btnCancel.TabIndex = 2;
            btnSave.TabIndex = 3;
            btnReverse.TabIndex = 4;
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
                isViewMode = false;
                isNewClicked = false;
                //clear all grid views
                grdInvoiceOutDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                receiveList.Clear();
                TotalToAdjust = 0;
                //----------clear temperory panels
                grdInvoiceOutDetail.Columns["POQuantity"].Visible = true;
                grdInvoiceOutDetail.Columns["StockQuantity"].Visible = true;
                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                //----------
                lblAdvTotal.Text = "";
                txtPaymentTerms.Text = "";
                cmbRevCharge.SelectedIndex = cmbRevCharge.FindString("No");
                txtCustomerID.Text = "";
                txtCustName.Text = "";
                cmbCurrency.SelectedIndex = -1;
                cmbCurrency.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCurrency, "INR");
                chkPJVApprove.Checked = false;
                cmbTransportationMode.SelectedIndex = -1;
                cmbTransportationType.SelectedIndex = -1;
                cmbTransporter.SelectedIndex = -1;
                cmbOriginCountryID.SelectedIndex = -1;
                cmbFinalDestinationCountryID.SelectedIndex = -1;
                cmbPreCarriageTransMode.SelectedIndex = -1;
                txtProjectID.Text = "";
                cmbADCode.SelectedIndex = -1;
                cmbInvoiceType.SelectedItem = null;

                txtTemporarryNo.Text = "0";
                dtTempDate.Value = DateTime.Parse("1900-01-01");
                txtInvoiceNo.Text = "";
                dtInvoiceDate.Value = DateTime.Parse("1900-01-01");
                txtPOTrackNos.Text = "";
                txtPOTrackDates.Text = "";

                //txtINRInversionRate.Text = "0";
                txtInvoiceValueINR.Text = "";
                txtFinalDestinationPlace.Text = "";
                txtPrecarrierReceivedPlace.Text = "";
                cmbStateCode.SelectedIndex = -1;
                txtDeliveryAddress.Text = "";
                txtBankRefNo.Text = "";
                txtTermsOfDelivery.Text = "";
                txtFinalDestinationPlace.Text = "";
                txtEntryPort.Text = "";
                txtExitPort.Text = "";
                txtTaxAmount.Text = "0";
                txtProductTaxINR.Text = "0";
                txtInvoiceAmount.Text = "0";
                txtRemarks.Text = "";
                txtSpecialNote.Text = "";
                txtProductValue.Text = "0";
                txtProductValueINR.Text = "0";
                btnProductValue.Text = "0";
                btnTaxAmount.Text = "0";
                tabInvoiceType.Enabled = true;
                tabInvoiceType.Visible = true;
                tabInvoiceHeader.Visible = false;
                tabInvoiceDetail.Visible = false;
                //txtPaymentTerms.Text = "";
                track = false;
                previoh = new invoiceoutheader();
                AddRowClick = false;
                txtFreightCharge.Text = "0";
                commentStatus = "";
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
                isNewClicked = true;
                btnSave.Text = "Save";
                pnlAddEdit.Visible = true;
                closeAllPanels();
                pnlAddEdit.Visible = true;
                tabControl1.SelectedTab = tabInvoiceType;
                cmbInvoiceType.Enabled = true;
                isViewMode = false;
                setButtonVisibility("btnNew");
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
                AddIODetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddIODetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdInvoiceOutDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkIODetailGridRows())
                    {
                        return false;
                    }
                }
                grdInvoiceOutDetail.Rows.Add();
                int kount = grdInvoiceOutDetail.RowCount;
                grdInvoiceOutDetail.Rows[kount - 1].Cells["LineNo"].Value = kount;
                //grdInvoiceOutDetail.Rows[kount - 1].Cells["LineNo"].Selected = true;

                grdInvoiceOutDetail.Rows[kount - 1].Cells["POItemRefNo"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["StockReferenceNo"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["gPONo"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["gPODate"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["Item"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["ItemName"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["Location"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["ModelNo"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["ModelName"].Value = "";
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdInvoiceOutDetail.Rows[kount - 1].Cells["gTaxCode"]);
                TaxCodeDB.fillTaxCodeGridViewCombo(ComboColumn2, "");
                grdInvoiceOutDetail.Rows[kount - 1].Cells["Unit"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["Quantity"].Value = 0;
                grdInvoiceOutDetail.Rows[kount - 1].Cells["Price"].Value = 0;
                grdInvoiceOutDetail.Rows[kount - 1].Cells["Value"].Value = 0;
                grdInvoiceOutDetail.Rows[kount - 1].Cells["Tax"].Value = 0;
                grdInvoiceOutDetail.Rows[kount - 1].Cells["WarrantyDays"].Value = 0;
                grdInvoiceOutDetail.Rows[kount - 1].Cells["TaxDetails"].Value = " ";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["CustomerItemDescription"].Value = " ";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["MRNNo"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["MRNDate"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["BatchNo"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["SerielNo"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["ExpiryDate"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["PurchaseQuantity"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["PurchasePrice"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["PurchaseTax"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["SupplierID"].Value = "";
                grdInvoiceOutDetail.Rows[kount - 1].Cells["SupplierName"].Value = "";
                if (AddRowClick)
                {
                    grdInvoiceOutDetail.FirstDisplayedScrollingRowIndex = grdInvoiceOutDetail.RowCount - 1;
                    grdInvoiceOutDetail.CurrentCell = grdInvoiceOutDetail.Rows[kount - 1].Cells[0];
                }
                grdInvoiceOutDetail.FirstDisplayedScrollingColumnIndex = 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show("AddIODetailRow() : Error");
            }

            return status;
        }
        private Boolean getTotalissuedStockOfAItemWRTStock()
        {
            Boolean stat = true;
            try
            {
                foreach (DataGridViewRow row1 in grdInvoiceOutDetail.Rows)
                {
                    int refNo = Convert.ToInt32(row1.Cells["StockReferenceNo"].Value);

                    double Quant = 0;
                    foreach (DataGridViewRow row2 in grdInvoiceOutDetail.Rows)
                    {
                        if (row2.Cells["StockReferenceNo"].Value.ToString() == row1.Cells["StockReferenceNo"].Value.ToString())
                        {
                            Quant = Quant + Convert.ToDouble(row2.Cells["Quantity"].Value);
                        }
                    }
                    if (!StockDB.verifyPresentStockAvailability(refNo, Quant))
                    {
                        MessageBox.Show("Total Issue Quantity For Item : <" + row1.Cells["ItemName"].Value.ToString() +
                            "> is More than the Available Stock Quantity");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return stat;
        }
        private Boolean getTotalissuedStockOfAItemWRTPO()
        {
            Boolean stat = true;
            try
            {
                foreach (DataGridViewRow row1 in grdInvoiceOutDetail.Rows)
                {
                    int refNo = Convert.ToInt32(row1.Cells["POItemRefNo"].Value);

                    double Quant = 0;
                    foreach (DataGridViewRow row2 in grdInvoiceOutDetail.Rows)
                    {
                        if (row2.Cells["POItemRefNo"].Value.ToString() == row1.Cells["POItemRefNo"].Value.ToString())
                        {
                            Quant = Quant + Convert.ToDouble(row2.Cells["Quantity"].Value);
                        }
                    }
                    if (!POPIHeaderDB.checkAvailableQuantityForInvoiceOut(refNo, Quant))
                    {
                        MessageBox.Show("Total Issue Quantity For Item : <" + row1.Cells["ItemName"].Value.ToString() +
                            "> is More than the Available PO Quantity");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return stat;
        }
        private Boolean verifyAndReworkIODetailGridRows()
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

                if (grdInvoiceOutDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in InvoiceOut details");
                    txtProductValue.Text = productvalue.ToString();
                    txtTaxAmount.Text = taxvalue.ToString(); //fill this later
                    txtInvoiceAmount.Text = (productvalue + taxvalue).ToString();
                    btnProductValue.Text = txtProductValue.Text;
                    btnTaxAmount.Text = txtTaxAmount.Text;
                    txtProductValueINR.Text = (Convert.ToDouble(txtProductValue.Text) * Convert.ToDouble(txtINRInversionRate.Text)).ToString();
                    txtProductTaxINR.Text = (Convert.ToDouble(txtTaxAmount.Text) * Convert.ToDouble(txtINRInversionRate.Text)).ToString();
                    txtInvoiceValueINR.Text = (Convert.ToDouble(txtInvoiceAmount.Text) * Convert.ToDouble(txtINRInversionRate.Text)).ToString();
                    return false;
                }
                if (cmbCurrency.SelectedIndex == -1)
                {
                    MessageBox.Show("select Currency Value");
                    return false;
                }
                if (txtINRInversionRate.Text == null || txtINRInversionRate.Text.Trim().Length == 0 || Convert.ToDouble(txtINRInversionRate.Text.Trim()) == 0)
                {
                    MessageBox.Show("Fill INR Conversion Rate");
                    return false;
                }
                //clear tax details table
                TaxDetailsTable.Rows.Clear();
                if (!isViewMode)
                {
                    if (docID.Equals("PRODUCTINVOICEOUT") || docID.Equals("PRODUCTEXPORTINVOICEOUT"))
                    {
                        if (!getTotalissuedStockOfAItemWRTStock())
                        {
                            return false;
                        }
                    }
                    if (!docID.Equals("SERVICERCINVOICEOUT"))
                    {
                        if (!getTotalissuedStockOfAItemWRTPO())
                        {
                            return false;
                        }
                    }
                }
                for (int i = 0; i < grdInvoiceOutDetail.Rows.Count; i++)
                {
                    if (grdInvoiceOutDetail.Rows[i].Cells["gTaxCode"].Value == null ||
                        grdInvoiceOutDetail.Rows[i].Cells["gTaxCode"].Value.ToString().Length == 0)
                    {
                        MessageBox.Show("Fill TaxCode in row " + (i + 1));
                        return false;
                    }
                    grdInvoiceOutDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if ((grdInvoiceOutDetail.Rows[i].Cells["Item"].Value == null) ||
                        (grdInvoiceOutDetail.Rows[i].Cells["ItemName"].Value == null) ||
                        (grdInvoiceOutDetail.Rows[i].Cells["CustomerItemDescription"].Value == null) ||
                        (grdInvoiceOutDetail.Rows[i].Cells["CustomerItemDescription"].Value.ToString().Trim().Length == 0) ||
                         (grdInvoiceOutDetail.Rows[i].Cells["Quantity"].Value == null) ||
                        (grdInvoiceOutDetail.Rows[i].Cells["Quantity"].Value.ToString().Length == 0) ||
                        (grdInvoiceOutDetail.Rows[i].Cells["Price"].Value.ToString().Length == 0) ||
                        (Convert.ToDouble(grdInvoiceOutDetail.Rows[i].Cells["Quantity"].Value) == 0) ||
                        (Convert.ToDouble(grdInvoiceOutDetail.Rows[i].Cells["Price"].Value) == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));

                        return false;
                    }
                    if (!isViewMode)
                    {
                        int Refno = Convert.ToInt32(grdInvoiceOutDetail.Rows[i].Cells["POItemRefNo"].Value.ToString());
                        double Quant = Convert.ToInt32(grdInvoiceOutDetail.Rows[i].Cells["Quantity"].Value.ToString());
                        if (!docID.Equals("SERVICERCINVOICEOUT"))
                        {
                            if (!POPIHeaderDB.checkAvailableQuantityForInvoiceOut(Refno, Quant))
                            {
                                MessageBox.Show("Quantity entered is more than PO quantity. Row:" + (i + 1));
                                return false;
                            }
                        }
                        if (docID.Equals("PRODUCTINVOICEOUT") || docID.Equals("PRODUCTEXPORTINVOICEOUT"))
                        {
                            int StockRefno = Convert.ToInt32(grdInvoiceOutDetail.Rows[i].Cells["StockReferenceNo"].Value.ToString());
                            if (!StockDB.verifyPresentStockAvailability(StockRefno, Quant))
                            {
                                MessageBox.Show("Quantity not enough for issue.Row:" + (i + 1));
                                return false;
                            }
                        }
                    }

                    quantity = Convert.ToDouble(grdInvoiceOutDetail.Rows[i].Cells["Quantity"].Value);
                    price = Convert.ToDouble(grdInvoiceOutDetail.Rows[i].Cells["Price"].Value);
                    if (price != 0 && quantity != 0)
                    {
                        cost = Math.Round(quantity * price, 2);
                    }

                    try
                    {
                        strtaxCode = grdInvoiceOutDetail.Rows[i].Cells["gTaxCode"].Value.ToString();
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
                        catch (Exception ex)
                        {
                            ttax1 = 0.0;
                        }
                        ttax2 = ttax2 + ttax1;
                    }
                    grdInvoiceOutDetail.Rows[i].Cells["Value"].Value = Convert.ToDouble(cost.ToString());
                    grdInvoiceOutDetail.Rows[i].Cells["Tax"].Value = ttax2.ToString();
                    grdInvoiceOutDetail.Rows[i].Cells["TaxDetails"].Value = strTax;
                    productvalue = productvalue + cost;
                    taxvalue = taxvalue + ttax2;

                    //- rewok tax value
                }
                txtProductValue.Text = productvalue.ToString();
                txtTaxAmount.Text = taxvalue.ToString(); //fill this later

                btnProductValue.Text = txtProductValue.Text;
                btnTaxAmount.Text = txtTaxAmount.Text;
                txtProductValueINR.Text = (Convert.ToDouble(txtProductValue.Text) * Convert.ToDouble(txtINRInversionRate.Text)).ToString();
                txtProductTaxINR.Text = (Convert.ToDouble(txtTaxAmount.Text) * Convert.ToDouble(txtINRInversionRate.Text)).ToString();
                //txtInvoiceValueINR.Text = (Convert.ToDouble(txtInvoiceAmount.Text) * Convert.ToDouble(txtINRInversionRate.Text)).ToString();
                if (txtFreightCharge.Text.Length == 0 || Convert.ToDouble(txtFreightCharge.Text) == 0)
                {
                    txtFreightForwarding.Text = "0";
                    txtFreightForwardingINR.Text = "0";
                }
                else
                {
                    txtFreightForwarding.Text = (Convert.ToDouble(txtFreightCharge.Text) + Convert.ToDouble(txtTaxAmount.Text)).ToString();
                    txtFreightForwardingINR.Text = (Convert.ToDouble(txtFreightForwarding.Text) * Convert.ToDouble(txtINRInversionRate.Text)).ToString();
                }

                txtInvoiceValueINR.Text = (Convert.ToDouble(txtFreightForwarding.Text) * Convert.ToDouble(txtINRInversionRate.Text)).ToString();

                double freightValue = Convert.ToDouble(txtFreightForwarding.Text);
                txtInvoiceAmount.Text = (productvalue + taxvalue + freightValue).ToString();
                txtInvoiceValueINR.Text = (Convert.ToDouble(txtInvoiceAmount.Text) * Convert.ToDouble(txtINRInversionRate.Text)).ToString();
                //if (!validateItems())
                //{
                //    MessageBox.Show("Validation failed");
                //}
            }
            catch (Exception ex)
            {
                return false;
            }
            return status;
        }
        private List<invoiceoutdetail> getIOHandDetails(invoiceoutheader ioh)
        {
            List<invoiceoutdetail> IODetails = new List<invoiceoutdetail>();
            try
            {
                invoiceoutdetail iod = new invoiceoutdetail();
                for (int i = 0; i < grdInvoiceOutDetail.Rows.Count; i++)
                {
                    iod = new invoiceoutdetail();
                    iod.DocumentID = ioh.DocumentID;
                    iod.TemporaryNo = ioh.TemporaryNo;
                    iod.TemporaryDate = ioh.TemporaryDate;
                    iod.PODate = Convert.ToDateTime(grdInvoiceOutDetail.Rows[i].Cells["gPODate"].Value);
                    iod.PONo = Convert.ToInt32(grdInvoiceOutDetail.Rows[i].Cells["gPONo"].Value);
                    iod.StockItemID = grdInvoiceOutDetail.Rows[i].Cells["Item"].Value.ToString().Trim();
                    iod.StockItemName = grdInvoiceOutDetail.Rows[i].Cells["ItemName"].Value.ToString().Trim();
                    iod.ModelNo = grdInvoiceOutDetail.Rows[i].Cells["ModelNo"].Value.ToString().Trim();
                    iod.ModelName = grdInvoiceOutDetail.Rows[i].Cells["ModelName"].Value.ToString().Trim();
                    iod.CustomerItemDescription = grdInvoiceOutDetail.Rows[i].Cells["CustomerItemDescription"].Value.ToString().Trim();
                    //iod.WorkDescription = " ";
                    iod.TaxCode = grdInvoiceOutDetail.Rows[i].Cells["gTaxCode"].Value.ToString();
                    iod.POItemReferenceNo = Convert.ToInt32(grdInvoiceOutDetail.Rows[i].Cells["POItemRefNo"].Value);
                    iod.Quantity = Convert.ToDouble(grdInvoiceOutDetail.Rows[i].Cells["Quantity"].Value);
                    iod.Price = Convert.ToDouble(grdInvoiceOutDetail.Rows[i].Cells["Price"].Value);
                    iod.Tax = Convert.ToDouble(grdInvoiceOutDetail.Rows[i].Cells["Tax"].Value);
                    iod.WarrantyDays = Convert.ToInt32(grdInvoiceOutDetail.Rows[i].Cells["WarrantyDays"].Value);
                    iod.TaxDetails = grdInvoiceOutDetail.Rows[i].Cells["TaxDetails"].Value.ToString();

                    if (docID.Equals("PRODUCTINVOICEOUT") || docID.Equals("PRODUCTEXPORTINVOICEOUT"))
                    {
                        iod.MRNNo = Convert.ToInt32(grdInvoiceOutDetail.Rows[i].Cells["MRNNo"].Value);
                        iod.MRNDate = Convert.ToDateTime(grdInvoiceOutDetail.Rows[i].Cells["MRNDate"].Value);
                        iod.BatchNo = grdInvoiceOutDetail.Rows[i].Cells["BatchNo"].Value.ToString();
                        iod.SerielNo = grdInvoiceOutDetail.Rows[i].Cells["SerielNo"].Value.ToString();
                        iod.ExpiryDate = Convert.ToDateTime(grdInvoiceOutDetail.Rows[i].Cells["ExpiryDate"].Value);
                        iod.PurchaseQuantity = Convert.ToDouble(grdInvoiceOutDetail.Rows[i].Cells["PurchaseQuantity"].Value);
                        iod.PurchasePrice = Convert.ToDouble(grdInvoiceOutDetail.Rows[i].Cells["PurchasePrice"].Value);
                        iod.PurchaseTax = Convert.ToDouble(grdInvoiceOutDetail.Rows[i].Cells["PurchaseTax"].Value);
                        iod.SupplierID = grdInvoiceOutDetail.Rows[i].Cells["SupplierID"].Value.ToString().Trim();
                        iod.StockReferenceNo = Convert.ToInt32(grdInvoiceOutDetail.Rows[i].Cells["StockReferenceNo"].Value);
                    }
                    IODetails.Add(iod);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("createAndUpdateIODetails() : Error updating IO Details");
                IODetails = null;
            }
            return IODetails;
        }

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredInvoiceOutHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredInvoiceOutHeader(listOption);
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
            ListFilteredInvoiceOutHeader(listOption);
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {

                InvoiceOutHeaderDB iohdb = new InvoiceOutHeaderDB();
                invoiceoutheader ioh = new invoiceoutheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    if (!verifyAndReworkIODetailGridRows())
                    {
                        return;
                    }
                    ioh.DocumentID = docID;
                    ioh.InvoiceDate = dtInvoiceDate.Value;

                    ioh.ConsigneeID = txtCustomerID.Text;
                    ioh.TrackingNos = txtPOTrackNos.Text;
                    ioh.TrackingDates = txtPOTrackDates.Text;

                    if ((docID != "PRODUCTEXPORTINVOICEOUT") && (docID != "SERVICEEXPORTINVOICEOUT"))
                    {
                        if (cmbStateCode.SelectedIndex == -1)
                        {
                            MessageBox.Show("Delivery state code is mandatory.");
                            return;
                        }
                        ioh.DeliveryStateCode = ((Structures.ComboBoxItem)cmbStateCode.SelectedItem).HiddenValue;
                    }
                    else
                    {
                        ioh.DeliveryStateCode = "";
                    }

                    ioh.DeliveryAddress = txtDeliveryAddress.Text.Trim();


                    if ((docID == "PRODUCTEXPORTINVOICEOUT") || (docID == "PRODUCTINVOICEOUT"))
                    {
                        ioh.TransportationMode = ((Structures.ComboBoxItem)cmbTransportationMode.SelectedItem).HiddenValue;
                        ioh.TransportationType = ((Structures.ComboBoxItem)cmbTransportationType.SelectedItem).HiddenValue;
                        ioh.Transporter = ((Structures.ComboBoxItem)cmbTransporter.SelectedItem).HiddenValue;
                    }
                    else
                    {
                        ioh.TransportationMode = "";
                        ioh.TransportationType = "";
                        ioh.Transporter = "";
                    }
                    ioh.TermsOfPayment = txtPaymentTerms.Text.Trim().Replace("'","''");

                    ioh.CurrencyID = ((Structures.ComboBoxItem)cmbCurrency.SelectedItem).HiddenValue;

                    ioh.ProjectID = txtProjectID.Text.ToString().Trim();
                    ioh.Remarks = txtRemarks.Text;
                    ioh.ReverseCharge = cmbRevCharge.SelectedItem.ToString();
                    ioh.SpecialNote = txtSpecialNote.Text;
                    if ((docID == "PRODUCTEXPORTINVOICEOUT") || (docID == "SERVICEEXPORTINVOICEOUT"))
                    {
                        ioh.OriginCountryID = ((Structures.ComboBoxItem)cmbOriginCountryID.SelectedItem).HiddenValue;
                        //ioh.OriginCountryID = cmbOriginCountryID.SelectedItem.ToString().Trim().Substring(0, cmbOriginCountryID.SelectedItem.ToString().Trim().IndexOf('-'));
                        ioh.FinalDestinatoinCountryID = ((Structures.ComboBoxItem)cmbFinalDestinationCountryID.SelectedItem).HiddenValue;
                        ioh.FinalDestinationPlace = txtFinalDestinationPlace.Text;
                        ioh.PreCarriageTransportationMode = ((Structures.ComboBoxItem)cmbPreCarriageTransMode.SelectedItem).HiddenValue;
                        //ioh.PreCarriageTransportationMode = cmbPreCarriageTransMode.SelectedItem.ToString().Trim().Substring(0, cmbPreCarriageTransMode.SelectedItem.ToString().Trim().IndexOf('-'));
                        ioh.PreCarrierReceiptPlace = txtPrecarrierReceivedPlace.Text;
                        ioh.TermsOfDelivery = txtTermsOfDelivery.Text;
                        ioh.EntryPort = txtEntryPort.Text;
                        ioh.ExitPort = txtExitPort.Text;
                        ioh.ADCode = ((Structures.ComboBoxItem)cmbADCode.SelectedItem).HiddenValue;
                        //ioh.ADCode = cmbADCode.SelectedItem.ToString().Trim().Substring(0, cmbADCode.SelectedItem.ToString().Trim().IndexOf('-'));
                    }
                    ioh.INRConversionRate = Convert.ToDouble(txtINRInversionRate.Text);
                    // ioh.DeliveryAddress = txtDeliveryAddress.Text;
                    ioh.FreightCharge = Convert.ToDouble(txtFreightCharge.Text);
                    ioh.BankAcReference = Convert.ToInt32(txtBankRefNo.Text);
                    ioh.ProductValue = Convert.ToDouble(txtProductValue.Text);
                    ioh.TaxAmount = Convert.ToDouble(txtTaxAmount.Text);
                    ioh.ProductValueINR = Convert.ToDouble(txtProductValueINR.Text);
                    ioh.TaxAmountINR = Convert.ToDouble(txtProductTaxINR.Text);
                    ioh.InvoiceAmount = Convert.ToDouble(txtInvoiceAmount.Text);
                    ioh.INRAmount = Convert.ToDouble(txtInvoiceValueINR.Text);
                    // ioh.ReferenceNo = txtReferenceNo.Text;
                    ioh.Comments = docCmtrDB.DGVtoString(dgvComments).Replace("'","''");
                    ioh.ForwarderList = previoh.ForwarderList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }

                if (!iohdb.validateInvoiceOutHeader(ioh))
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    //ioh.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    ioh.DocumentStatus = 1; //created
                    ioh.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    ioh.TemporaryNo = Convert.ToInt32(txtTemporarryNo.Text);
                    ioh.TemporaryDate = previoh.TemporaryDate;
                    ioh.DocumentStatus = previoh.DocumentStatus;
                }
                //Replacing single quotes
                ioh = verifyHeaderInputString(ioh);
                verifyDetailInputString();

                if (iohdb.validateInvoiceOutHeader(ioh))
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
                            ioh.CommentStatus = docCmtrDB.createCommentStatusString(previoh.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            ioh.CommentStatus = previoh.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            ioh.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            ioh.CommentStatus = previoh.CommentStatus;
                        }
                    }
                    //----------
                    int tmpStatus = 1;

                    if (txtComments.Text.Trim().Length > 0)
                    {
                        ioh.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus).Replace("'","''");
                    }
                    List<invoiceoutdetail> IOHList = getIOHandDetails(ioh);
                    if (!docID.Equals("SERVICERCINVOICEOUT"))
                    {

                        showQuantityAvailableForAllProductListView(IOHList);
                        if (!validateIODetailProductquantity(IOHList))
                        {
                            MessageBox.Show("Enterd Quantity is more than the PO Qunatity. Not Allowed TO Save/Update.");
                            return;
                        }
                    }
                    DialogResult dialog = MessageBox.Show("Are you sure to " + btnSave.Text + " the document ?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.No)
                    {
                        return;
                    }
                    if (btnText.Equals("Update"))
                    {

                        if (iohdb.updateInvoiceOutHeaderAndDetail(ioh, previoh, IOHList, receiveList))
                        {
                            createSJV(previoh.TemporaryNo, previoh.TemporaryDate);
                            MessageBox.Show("Invoice Out Header updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredInvoiceOutHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update Invoice Out Header");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        int TNo = 0;
                        ///return;
                        if (iohdb.InsertInvoiceOutHeaderAndDetail(ioh, IOHList, out TNo, receiveList))
                        {
                            createSJV(TNo, UpdateTable.getSQLDateTime());
                            MessageBox.Show("Invoice Out Header Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredInvoiceOutHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert Invoice Out Header");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invoice Out Header Validation failed");
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
        //onley called in time of save or update
        public void createSJV(int InvTempNum, DateTime InvTempDate)
        {
            //opt-1:temporary,2:Approved
            try
            {
                SJVHeader sjvh = new SJVHeader();

                sjvh.InvReferenceNo = previoh.RowID;
                sjvh.DocumentID = "SJV";

                //New: previoh.SJVTNo = 0
                //Update : previoh.SJVTNo = XX

                if (previoh.SJVTNo == 0) //JV Not available
                {
                    sjvh.TemporaryNo = DocumentNumberDB.getNewNumber("SJV", 1);
                    sjvh.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    sjvh.TemporaryNo = previoh.SJVTNo;
                    sjvh.TemporaryDate = previoh.SJVTDate;
                }
                if (previoh.SJVNo == 0) //To be updaate at time of approving invoice
                {
                    sjvh.JournalNo = 0;
                    sjvh.JournalDate = DateTime.Parse("1900-01-01");
                }
                else //If JV approved and Invoice unlocked
                {
                    sjvh.JournalNo = previoh.SJVNo;
                    sjvh.JournalDate = previoh.SJVDate;
                }


                sjvh.Narration = "Sales against Invoice No " + previoh.InvoiceNo + "," +
                    "Dated " + previoh.InvoiceDate.ToString("dd-MM-yyyy") + "," +
                    "Party:" + previoh.ConsigneeName;
                sjvh.CreateUser = Login.userLoggedIn;
                sjvh.CreateTime = sjvh.TemporaryDate;

                sjvh.InvTempNo = InvTempNum;
                sjvh.InvTempDate = InvTempDate;
                sjvh.TaxDetail = getTaxDetails();
                sjvh.InvDocumentID = docID;
                if (previoh.ConsigneeID != null)
                    sjvh.Customer = previoh.ConsigneeID;
                else
                {
                    invoiceoutheader ioh = new invoiceoutheader();
                    ioh.DocumentID = docID;
                    ioh.TemporaryNo = InvTempNum;
                    ioh.TemporaryDate = InvTempDate;

                    sjvh.Customer = InvoiceOutHeaderDB.getCustIDOfINvoiceOUT(ioh);
                }
                sjvh.Amtount = Convert.ToDouble(txtInvoiceValueINR.Text);
                sjvh.TaxAmount = Convert.ToDouble(txtProductTaxINR.Text);
                sjvh.status = 0;
                sjvh.DocumentStatus = 1;
                SJVDB.InsertSJVHeaderAndDetail(sjvh);
            }
            catch (Exception ex)
            {
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
            AddRowClick = true;
            AddIODetailRow();
        }

        private void grdPOPIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdInvoiceOutDetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdInvoiceOutDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkIODetailGridRows();
                    }
                    if (columnName.Equals("TaxView"))
                    {
                        //show tax details
                        DialogResult dialog = MessageBox.Show(grdInvoiceOutDetail.Rows[e.RowIndex].Cells["TaxDetails"].Value.ToString(),
                            "Line No : " + (e.RowIndex + 1), MessageBoxButtons.OK);
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

        private void btnCalculateax_Click_1(object sender, EventArgs e)
        {
            //if (cmbTaxCode.SelectedIndex == -1)
            //{
            //    MessageBox.Show("Select Tax Code");
            //    return;
            //}
            if (txtINRInversionRate.Text.Length == 0)
            {
                MessageBox.Show("Fill INR Conv. Rate");
                return;
            }
            verifyAndReworkIODetailGridRows();
        }

        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                grdList.Rows[e.RowIndex].Selected = true;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("LoadDocument") || columnName.Equals("Print") ||
                    columnName.Equals("View"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    chkPJVApprove.Checked = false;
                   
                    if (columnName.Equals("View"))
                    {
                        isViewMode = true;
                        tabControl1.TabPages["tabInvoiceDetail"].Enabled = true;
                        tabControl1.TabPages["tabInvoiceHeader"].Enabled = true;
                    }
                    previoh = new invoiceoutheader();
                    AddRowClick = false;
                    previoh.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();
                    previoh.RowID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["RowID"].Value.ToString());
                    previoh.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    previoh.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    // previoh.ReferenceNo = grdList.Rows[e.RowIndex].Cells["ReferenceNo"].Value.ToString();
                    previoh.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    previoh.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());

                    previoh.SJVTNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["SJVTNo"].Value.ToString());
                    previoh.SJVTDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["SJVTDate"].Value.ToString());
                    previoh.SJVNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["SJVNo"].Value.ToString());
                    previoh.SJVDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["SJVDate"].Value.ToString());

                    if ((previoh.DocumentID == "PRODUCTEXPORTINVOICEOUT") || (previoh.DocumentID == "SERVICEEXPORTINVOICEOUT"))
                    {
                        previoh.OriginCountryID = grdList.Rows[e.RowIndex].Cells["OriginCountryID"].Value.ToString();
                        previoh.OriginCountryName = grdList.Rows[e.RowIndex].Cells["OriginCountryName"].Value.ToString();

                        previoh.FinalDestinatoinCountryID = grdList.Rows[e.RowIndex].Cells["FinalDestinationCountryID"].Value.ToString();
                        previoh.FinalDestinatoinCountryName = grdList.Rows[e.RowIndex].Cells["FinalDestinationCountryName"].Value.ToString();

                        previoh.FinalDestinationPlace = grdList.Rows[e.RowIndex].Cells["FinalDestinationPlace"].Value.ToString();

                        previoh.PreCarriageTransportationMode = grdList.Rows[e.RowIndex].Cells["PreCarriageTransportationMode"].Value.ToString();
                        previoh.PreCarriageTransportationName = grdList.Rows[e.RowIndex].Cells["PreCarriageTransportationName"].Value.ToString();

                        previoh.PreCarrierReceiptPlace = grdList.Rows[e.RowIndex].Cells["PreCarrierReceiptPlace"].Value.ToString();
                        previoh.TermsOfDelivery = grdList.Rows[e.RowIndex].Cells["TermsOfDelivery"].Value.ToString();
                        previoh.EntryPort = grdList.Rows[e.RowIndex].Cells["EntryPort"].Value.ToString();
                        previoh.ExitPort = grdList.Rows[e.RowIndex].Cells["ExitPort"].Value.ToString();
                        previoh.ADCode = grdList.Rows[e.RowIndex].Cells["ADCode"].Value.ToString();

                        cmbOriginCountryID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbOriginCountryID, previoh.OriginCountryID);
                        cmbFinalDestinationCountryID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbFinalDestinationCountryID, previoh.FinalDestinatoinCountryID);
                        //cmbFinalDestinationCountryID.SelectedIndex = cmbFinalDestinationCountryID.FindString(previoh.FinalDestinatoinCountryID);
                        txtFinalDestinationPlace.Text = previoh.FinalDestinationPlace;
                        //cmbPreCarriageTransMode.SelectedIndex = cmbPreCarriageTransMode.FindString(previoh.PreCarriageTransportationMode);
                        cmbPreCarriageTransMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbPreCarriageTransMode, previoh.PreCarriageTransportationMode);
                        txtPrecarrierReceivedPlace.Text = previoh.PreCarrierReceiptPlace;
                        txtTermsOfDelivery.Text = previoh.TermsOfDelivery;
                        txtEntryPort.Text = previoh.EntryPort;
                        txtExitPort.Text = previoh.ExitPort;
                        cmbADCode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbADCode, previoh.ADCode);
                        //cmbADCode.SelectedIndex = cmbADCode.FindString(previoh.ADCode);
                    }
                    if (previoh.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    previoh.Comments = InvoiceOutHeaderDB.getUserComments(previoh.DocumentID, previoh.TemporaryNo, previoh.TemporaryDate);
                    docID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    PODocIdWRTIO = getPODocIDForSelectedIO(docID);
                    setPODetailColumns(docID);
                    setIOTabColumns(docID);
                    InvoiceOutHeaderDB iohdb = new InvoiceOutHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];


                    previoh.BankAcReference = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["BankAcReference"].Value.ToString());
                    previoh.InvoiceNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["InvoiceNo"].Value.ToString());
                    previoh.InvoiceDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["InvoiceDate"].Value.ToString());
                    previoh.ConsigneeID = grdList.Rows[e.RowIndex].Cells["ConsigneeID"].Value.ToString();
                    previoh.ConsigneeName = grdList.Rows[e.RowIndex].Cells["ConsigneeName"].Value.ToString();

                    previoh.DeliveryAddress = grdList.Rows[e.RowIndex].Cells["gDeliveryAddress"].Value.ToString();
                    previoh.DeliveryStateCode = grdList.Rows[e.RowIndex].Cells["gStateCode"].Value.ToString();

                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Document Temp No:" + previoh.TemporaryNo + "\n" +
                            "Document Temp Date:" + previoh.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "Invoice No:" + previoh.InvoiceNo + "\n" +
                            "Invoice Date:" + previoh.InvoiceDate.ToString("dd-MM-yyyy");
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = previoh.TemporaryNo + "-" + previoh.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_1(null, null);
                        return;
                    }
                    //--------

                    previoh.TrackingNos = grdList.Rows[e.RowIndex].Cells["PONo"].Value.ToString();
                    previoh.TrackingDates = grdList.Rows[e.RowIndex].Cells["PODate"].Value.ToString();
                    previoh.TermsOfPayment = grdList.Rows[e.RowIndex].Cells["TermsOfPayment"].Value.ToString();
                    previoh.Description = grdList.Rows[e.RowIndex].Cells["Description"].Value.ToString(); //terms of payment Name

                    if ((previoh.DocumentID == "PRODUCTEXPORTINVOICEOUT") || (previoh.DocumentID == "PRODUCTINVOICEOUT"))
                    {
                        previoh.TransportationMode = grdList.Rows[e.RowIndex].Cells["TransportationMode"].Value.ToString();
                        previoh.TransportationModeName = grdList.Rows[e.RowIndex].Cells["TransportationModeName"].Value.ToString();

                        previoh.TransportationType = grdList.Rows[e.RowIndex].Cells["TransportationType"].Value.ToString();
                        previoh.TransportationTypeName = grdList.Rows[e.RowIndex].Cells["TransportationTypeName"].Value.ToString();

                        previoh.Transporter = grdList.Rows[e.RowIndex].Cells["Transporter"].Value.ToString();
                        previoh.TransporterName = grdList.Rows[e.RowIndex].Cells["TransporterName"].Value.ToString();

                        cmbTransportationMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbTransportationMode, previoh.TransportationMode);
                        cmbTransportationType.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbTransportationType, previoh.TransportationType);
                        cmbTransporter.SelectedIndex = cmbTransporter.FindString(previoh.Transporter);
                        cmbTransporter.SelectedIndex =
                            Structures.ComboFUnctions.getComboIndex(cmbTransporter, previoh.Transporter);
                    }
                    previoh.CurrencyID = grdList.Rows[e.RowIndex].Cells["gCurrencyID"].Value.ToString();
                    //previoh.TaxCode = grdList.Rows[e.RowIndex].Cells["gTaxCode"].Value.ToString();
                    previoh.ProjectID = grdList.Rows[e.RowIndex].Cells["ProjectID"].Value.ToString();
                    previoh.INRConversionRate = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["INRConversionRate"].Value.ToString());

                    previoh.FreightCharge = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["FreightCharge"].Value.ToString());
                    previoh.ProductValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gProductValue"].Value.ToString());
                    previoh.TaxAmount = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["TaxAmount"].Value.ToString());
                    previoh.ProductValueINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["ProductValueINR"].Value.ToString());
                    previoh.TaxAmountINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["TaxAmountINR"].Value.ToString());
                    previoh.InvoiceAmount = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["InvoiceAmount"].Value.ToString());
                    previoh.INRAmount = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["INRAmount"].Value.ToString());
                    previoh.Remarks = grdList.Rows[e.RowIndex].Cells["gRemarks"].Value.ToString();
                    previoh.ReverseCharge = grdList.Rows[e.RowIndex].Cells["ReverseCharge"].Value.ToString();
                    previoh.SpecialNote = grdList.Rows[e.RowIndex].Cells["SpecialNote"].Value.ToString();
                    previoh.status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString());
                    previoh.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentStatus"].Value.ToString());
                    previoh.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["CreateTime"].Value.ToString());
                    previoh.CreateUser = grdList.Rows[e.RowIndex].Cells["CreateUser"].Value.ToString();
                    previoh.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    previoh.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    previoh.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    previoh.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    //cmbTaxCode.SelectedIndex = cmbTaxCode.FindString(previoh.TaxCode);

                    receiveList = InvoiceOutHeaderDB.getInvoiceOutAdvPaymentDetails(previoh.TemporaryNo, previoh.TemporaryDate, previoh.DocumentID);
                    decimal TotalAdv = receiveList.Sum(pay => pay.Amount);
                    if (TotalAdv != 0)
                    {
                        lblAdvTotal.Text = TotalAdv.ToString();
                    }
                    else
                    {
                        lblAdvTotal.Text = "";
                    }
                    //Printing
                    if (columnName.Equals("Print"))
                    {
                        //PrintPurchaseOrder ppo = new PrintPurchaseOrder();
                        pnlAddEdit.Visible = false;
                        pnlList.Visible = true;
                        grdList.Visible = true;
                        btnNew.Visible = true;
                        btnExit.Visible = true;
                        //CSLERP.PrintForms.PrintPurchaseOrder ppo = new CSLERP.PrintForms.PrintPurchaseOrder();
                        PrintInvoiceOut pio = new PrintInvoiceOut();
                        invoiceoutheader ioh = new invoiceoutheader();
                        List<invoiceoutdetail> IODetails = InvoiceOutHeaderDB.getInvoiceOutDetail(previoh);
                        foreach (invoiceoutdetail iod in IODetails)
                        {
                            if (iod.HSNCode.Trim().Length <= 0)
                            {
                                MessageBox.Show("HSN Code not available for " + iod.StockItemName+". Print aborted....");
                                return;
                            }
                        }
                        getTaxDetails(IODetails);
                        string taxstr = getTasDetailStr();
                        //string taxstr = "";
                        ////taxDetails4Print(IODetails, previoh.DocumentID);
                        if (previoh.DocumentID == "PRODUCTEXPORTINVOICEOUT" || previoh.DocumentID == "SERVICEEXPORTINVOICEOUT")
                        {
                            PrintExportInvoiceOut expPrint = new PrintExportInvoiceOut();
                            expPrint.PrintExportIO(previoh, IODetails, taxstr, PODocIdWRTIO);
                        }
                        else
                        {
                            pio.PrintIO(previoh, IODetails, taxstr, PODocIdWRTIO);
                        }
                        btnNew.Visible = true;
                        btnExit.Visible = true;
                        return;
                    }

                    //

                    //--comments
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    previoh.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(previoh.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(previoh.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---
                    //txtReferenceNo.Text = previoh.ReferenceNo.ToString();
                    txtTemporarryNo.Text = previoh.TemporaryNo.ToString();
                    dtTempDate.Value = previoh.TemporaryDate;
                    dtTempDate.Value = previoh.TemporaryDate;

                    txtDeliveryAddress.Text = previoh.DeliveryAddress;
                    cmbStateCode.SelectedIndex =
                         Structures.ComboFUnctions.getComboIndex(cmbStateCode, previoh.DeliveryStateCode);

                    //dtValidityDate.Value = prevpopi.ValidityDays.ToString();
                    txtInvoiceNo.Text = previoh.InvoiceNo.ToString();
                    try
                    {
                        dtInvoiceDate.Value = previoh.InvoiceDate;
                    }
                    catch (Exception)
                    {
                        dtInvoiceDate.Value = DateTime.Parse("01-01-1900");
                    }

                    ////////cmbConsignee.SelectedIndex = cmbConsignee.FindString(previoh.ConsigneeID);
                    //cmbConsignee.SelectedIndex =
                    //    Structures.ComboFUnctions.getComboIndex(cmbConsignee, previoh.ConsigneeID);
                    txtCustomerID.Text = previoh.ConsigneeID;
                    txtCustName.Text = previoh.ConsigneeName;
                    if (previoh.ProjectID.Length != 0)
                        txtProjectID.Text = previoh.ProjectID;
                    //cmbCustomer.Text = prevpopi.CustomerName;
                    txtPOTrackNos.Text = previoh.TrackingNos.ToString();
                    txtPOTrackDates.Text = previoh.TrackingDates;
                    try
                    {
                        dtInvoiceDate.Value = previoh.InvoiceDate;
                    }
                    catch (Exception)
                    {
                        dtInvoiceDate.Value = DateTime.Parse("01-01-1900");
                    }
                    //cmbPaymentTerms.SelectedIndex = cmbPaymentTerms.FindString(previoh.TermsOfPayment);
                    //cmbPaymentTerms.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbPaymentTerms, previoh.TermsOfPayment);
                    ////////cmbConsignee.SelectedIndex = cmbConsignee.FindString(previoh.ConsigneeID);
                    //cmbConsignee.SelectedIndex =
                    //    Structures.ComboFUnctions.getComboIndex(cmbConsignee, previoh.ConsigneeID);

                    ////////cmbCurrency.SelectedIndex = cmbCurrency.FindString(previoh.CurrencyID);
                    cmbCurrency.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCurrency, previoh.CurrencyID);
                    if(previoh.ReverseCharge == null || previoh.ReverseCharge.Length == 0)
                    {
                        cmbRevCharge.SelectedIndex = cmbRevCharge.FindString("No");
                    }
                    else
                    {
                        cmbRevCharge.SelectedIndex = cmbRevCharge.FindString(previoh.ReverseCharge);
                    }
                    
                    txtPaymentTerms.Text = previoh.TermsOfPayment;
                    txtINRInversionRate.Text = previoh.INRConversionRate.ToString();
                    //txtDeliveryAddress.Text = previoh.DeliveryAddress;
                    txtProductValue.Text = previoh.ProductValue.ToString();
                    txtTaxAmount.Text = previoh.TaxAmount.ToString();
                    txtProductValueINR.Text = previoh.ProductValueINR.ToString();
                    txtProductTaxINR.Text = previoh.TaxAmountINR.ToString();
                    txtInvoiceAmount.Text = previoh.InvoiceAmount.ToString();
                    txtRemarks.Text = previoh.Remarks.ToString();
                    txtSpecialNote.Text = previoh.SpecialNote.ToString();
                    txtInvoiceValueINR.Text = previoh.INRAmount.ToString();
                    txtBankRefNo.Text = previoh.BankAcReference.ToString();
                    txtFreightCharge.Text = previoh.FreightCharge.ToString();
                    txtFreightForwarding.Text = previoh.FreightCharge.ToString();
                    txtFreightForwardingINR.Text = (previoh.FreightCharge * previoh.INRConversionRate).ToString();
                    //txtFreightCharge.Text = previoh.FreightCharge.ToString();
                    List<invoiceoutdetail> IODetail = InvoiceOutHeaderDB.getInvoiceOutDetail(previoh);
                    grdInvoiceOutDetail.Rows.Clear();
                    int i = 0;
                    foreach (invoiceoutdetail iod in IODetail)
                    {
                        if (!AddIODetailRow())
                        {
                            MessageBox.Show("Error found in PO details. Please correct before updating the details");
                        }
                        else
                        {
                            ////DataGridViewComboBoxCell ComboColumn1 = new DataGridViewComboBoxCell();
                            ////StockItemDB.fillStockItemGridViewCombo(ComboColumn1, "");

                            try
                            {
                                grdInvoiceOutDetail.Rows[i].Cells["Item"].Value = iod.StockItemID;
                                grdInvoiceOutDetail.Rows[i].Cells["ItemName"].Value = iod.StockItemName;
                            }
                            catch (Exception)
                            {
                                grdInvoiceOutDetail.Rows[i].Cells["Item"].Value = null;
                                grdInvoiceOutDetail.Rows[i].Cells["ItemName"].Value = null;
                            }
                            grdInvoiceOutDetail.Rows[i].Cells["gPONo"].Value = iod.PONo;
                            grdInvoiceOutDetail.Rows[i].Cells["gPODate"].Value = iod.PODate;
                            grdInvoiceOutDetail.Rows[i].Cells["gTaxCode"].Value = iod.TaxCode;
                            grdInvoiceOutDetail.Rows[i].Cells["ModelNo"].Value = iod.ModelNo;
                            grdInvoiceOutDetail.Rows[i].Cells["ModelName"].Value = iod.ModelName;
                            grdInvoiceOutDetail.Rows[i].Cells["POItemRefNo"].Value = iod.POItemReferenceNo;
                            grdInvoiceOutDetail.Rows[i].Cells["POQuantity"].Value = POPIHeaderDB.getPOQuantityForInvoiceOut(iod.POItemReferenceNo);
                            grdInvoiceOutDetail.Rows[i].Cells["Quantity"].Value = iod.Quantity;
                            grdInvoiceOutDetail.Rows[i].Cells["Location"].Value = POPIHeaderDB.getPOLocaationForInvoiceOut(iod.POItemReferenceNo);
                            grdInvoiceOutDetail.Rows[i].Cells["CustomerItemDescription"].Value = iod.CustomerItemDescription;
                            ////grdPOPIDetail.Rows[i].Cells["CustomerItemDescription"].Value = popid.CustomerItemDescription;
                            grdInvoiceOutDetail.Rows[i].Cells["Price"].Value = iod.Price;
                            grdInvoiceOutDetail.Rows[i].Cells["Unit"].Value = iod.Unit;
                            grdInvoiceOutDetail.Rows[i].Cells["Value"].Value = iod.Quantity * iod.Price;
                            grdInvoiceOutDetail.Rows[i].Cells["Tax"].Value = iod.Tax;
                            grdInvoiceOutDetail.Rows[i].Cells["WarrantyDays"].Value = iod.WarrantyDays;
                            grdInvoiceOutDetail.Rows[i].Cells["TaxDetails"].Value = iod.TaxDetails;
                            if (docID.Equals("PRODUCTINVOICEOUT") || docID.Equals("PRODUCTEXPORTINVOICEOUT"))
                            {
                                grdInvoiceOutDetail.Rows[i].Cells["MRNNo"].Value = iod.MRNNo;
                                grdInvoiceOutDetail.Rows[i].Cells["MRNDate"].Value = iod.MRNDate;
                                grdInvoiceOutDetail.Rows[i].Cells["BatchNo"].Value = iod.BatchNo;
                                grdInvoiceOutDetail.Rows[i].Cells["SerielNo"].Value = iod.SerielNo;
                                grdInvoiceOutDetail.Rows[i].Cells["ExpiryDate"].Value = iod.ExpiryDate;
                                grdInvoiceOutDetail.Rows[i].Cells["PurchaseQuantity"].Value = iod.PurchaseQuantity;
                                grdInvoiceOutDetail.Rows[i].Cells["PurchasePrice"].Value = iod.PurchasePrice;
                                grdInvoiceOutDetail.Rows[i].Cells["PurchaseTax"].Value = iod.PurchaseTax;
                                grdInvoiceOutDetail.Rows[i].Cells["SupplierID"].Value = iod.SupplierID;
                                grdInvoiceOutDetail.Rows[i].Cells["SupplierName"].Value = iod.SupplierName;
                                grdInvoiceOutDetail.Rows[i].Cells["StockReferenceNo"].Value = iod.StockReferenceNo;
                                grdInvoiceOutDetail.Rows[i].Cells["StockQuantity"].Value = StockDB.getAvailiableStockQuantity(iod.StockReferenceNo);
                            }
                            i++;
                            productvalue = productvalue + iod.Quantity * iod.Price;
                            taxvalue = taxvalue + iod.Tax;
                        }

                    }
                    if (docID.Equals("PRODUCTINVOICEOUT") || docID.Equals("PRODUCTEXPORTINVOICEOUT"))
                    {
                        grdInvoiceOutDetail.Columns["POQuantity"].Visible = true;
                        grdInvoiceOutDetail.Columns["StockQuantity"].Visible = true;
                        grdInvoiceOutDetail.Columns["Location"].Visible = false;
                    }
                    else
                    {
                        grdInvoiceOutDetail.Columns["Location"].Visible = true;
                        grdInvoiceOutDetail.Columns["POQuantity"].Visible = true;
                        grdInvoiceOutDetail.Columns["StockQuantity"].Visible = false;
                    }
                    if (!verifyAndReworkIODetailGridRows())
                    {
                        MessageBox.Show("Error found in Invoice Out details. Please correct before updating the details");
                    }
                    btnSave.Text = "Update";
                    if (columnName == "View")
                    {
                        btnUpdatePJV.Visible = true;
                        btnUpdatePJV.Text = "Show SJV";
                    }
                    else
                    {
                        btnUpdatePJV.Text = "Update SJV";
                    }
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                    tabControl1.SelectedTab = tabInvoiceHeader;
                    tabControl1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                setButtonVisibility("init");
            }
        }

        private void btnClearEntries_Click_1(object sender, EventArgs e)
        {
            try
            {

                DialogResult dialog = MessageBox.Show("Are you sure to clear all entries in grid detail ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdInvoiceOutDetail.Rows.Clear();
                    MessageBox.Show("Grid items cleared.");
                    verifyAndReworkIODetailGridRows();
                }

            }
            catch (Exception)
            {
            }
        }
        private Boolean updateDashBoard(invoiceoutheader ioh, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = ioh.DocumentID;
                dsb.TemporaryNo = ioh.TemporaryNo;
                dsb.TemporaryDate = ioh.TemporaryDate;
                dsb.DocumentNo = ioh.InvoiceNo;
                dsb.DocumentDate = ioh.InvoiceDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = ioh.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(previoh.DocumentID);
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
                if (!checkAvailableStock())
                {
                    return;
                }
                if (!verifyAndReworkIODetailGridRows())
                {
                    return;
                }
                FinancialLimitDB flDB = new FinancialLimitDB();
                if (!flDB.checkEmployeeFinancialLimit(docID, Login.empLoggedIn, Convert.ToDouble(txtInvoiceValueINR.Text), 0))
                {
                    MessageBox.Show("No financial power for approving this document");
                    return;
                }
                if (chkPJVApprove.Checked == false)
                {
                    MessageBox.Show("Sales Journal is not approved. Check For approve Sales journal option.");
                    return;
                }
                if (!docID.Equals("SERVICERCINVOICEOUT"))
                {
                    List<invoiceoutdetail> IOHList = getIOHandDetails(previoh);

                    if (!validateIODetailProductquantity(IOHList))
                    {
                        showQuantityAvailableForAllProductListView(IOHList);
                        MessageBox.Show("Enterd Quantity is more than the PO Qunatity. Not Allowed TO Approve.");
                        return;
                    }
                }
                InvoiceOutHeaderDB iohdb = new InvoiceOutHeaderDB();

                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    previoh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(previoh.CommentStatus);
                    if (previoh.status != 96)
                    {
                        previoh.InvoiceNo = DocumentNumberDB.getNewNumber(docID, 2);
                    }
                    if (iohdb.ApproveInvoiceOut(previoh))
                    {
                        MessageBox.Show("Invoice Out Document Approved");
                        //added on 24/9/2017
                        if ((previoh.DocumentID == "PRODUCTEXPORTINVOICEOUT") || (previoh.DocumentID == "PRODUCTINVOICEOUT"))
                        {
                            List<invoiceoutdetail> IODetails = getInvOutDetails(previoh);
                            iohdb.updateIOInStock(IODetails);
                        }


                        if (!updateDashBoard(previoh, 2))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                        }
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredInvoiceOutHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception)
            {
            }

        }
        private Boolean checkAvailableStock()
        {
            Boolean status = true;
            for (int i = 0; i < grdInvoiceOutDetail.Rows.Count; i++)
            {
                double Quant = Convert.ToInt32(grdInvoiceOutDetail.Rows[i].Cells["Quantity"].Value.ToString());
                if (docID.Equals("PRODUCTINVOICEOUT") || docID.Equals("PRODUCTEXPORTINVOICEOUT"))
                {
                    int StockRefno = Convert.ToInt32(grdInvoiceOutDetail.Rows[i].Cells["StockReferenceNo"].Value.ToString());
                    if (!StockDB.verifyPresentStockAvailability(StockRefno, Quant))
                    {
                        MessageBox.Show("Quantity not available in PO. Check Available Quantity.Row:" + (i + 1));
                        return false;
                    }
                }
            }
            return status;
        }
        //private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        customer cust = new customer();
        //        //////custid = cmbConsignee.SelectedItem.ToString().Trim().Substring(0, cmbConsignee.SelectedItem.ToString().Trim().IndexOf('-'));
        //        custid = ((Structures.ComboBoxItem)cmbConsignee.SelectedItem).HiddenValue;
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        private void btnViewTaxDetails_Click(object sender, EventArgs e)
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


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // New PO
            if (btnSave.Text == "Save")
            {
                if (cmbInvoiceType.SelectedIndex == -1)
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
                        dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, previoh.TemporaryNo + "-" + previoh.TemporaryDate.ToString("yyyyMMddhhmmss"));
                        dgvDocumentList.Size = new Size(870, 300);
                        pnlPDFViewer.Controls.Add(dgvDocumentList);
                        dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
                        if (previoh.status == 1 && previoh.DocumentStatus == 99)
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

        private void cmbPOType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //ProductExportInvoice
                //ServiceExportInvoice
                //ProductInvoice
                //ServiceInvoice
                ////string potype = cmbInvoiceType.SelectedItem.ToString().Trim();
                string potype = ((Structures.ComboBoxItem)cmbInvoiceType.SelectedItem).HiddenValue;
                if (potype == "ProductExportInvoice")
                {
                    docID = "PRODUCTEXPORTINVOICEOUT";
                    PODocIdWRTIO = getPODocIDForSelectedIO(docID);
                }
                else if (potype == "ServiceExportInvoice")
                {
                    docID = "SERVICEEXPORTINVOICEOUT";
                    PODocIdWRTIO = getPODocIDForSelectedIO(docID);
                }
                else if (potype == "ProductInvoice")
                {
                    docID = "PRODUCTINVOICEOUT";
                    PODocIdWRTIO = getPODocIDForSelectedIO(docID);
                }
                else if (potype == "ServiceInvoice")
                {
                    docID = "SERVICEINVOICEOUT";
                    PODocIdWRTIO = getPODocIDForSelectedIO(docID);
                }
                else if (potype == "RateContractInvoice")
                {
                    docID = "SERVICERCINVOICEOUT";
                    PODocIdWRTIO = getPODocIDForSelectedIO(docID);
                }
                setIOTabColumns(docID);
                setPODetailColumns(docID);
                cmbInvoiceType.Enabled = false;
            }
            catch (Exception ex)
            {
            }
        }
        private string getPODocIDForSelectedIO(string iodocid)
        {
            string podocid = "";
            switch (docID)
            {
                case "PRODUCTEXPORTINVOICEOUT":
                    podocid = "POPRODUCTINWARD";
                    break;
                case "SERVICEEXPORTINVOICEOUT":
                    podocid = "POSERVICEINWARD";
                    break;
                case "PRODUCTINVOICEOUT":
                    podocid = "POPRODUCTINWARD";
                    break;
                case "SERVICEINVOICEOUT":
                    podocid = "POSERVICEINWARD";
                    break;
                case "SERVICERCINVOICEOUT":
                    podocid = "POSERVICERCINWARD";
                    break;
                default:
                    podocid = "";
                    break;
            }
            return podocid;
        }
        private void setPODetailColumns(string docID)
        {
            try
            {
                if ((docID == "PRODUCTEXPORTINVOICEOUT") || (docID == "PRODUCTINVOICEOUT"))
                {
                    grdInvoiceOutDetail.Columns["MRNNo"].Visible = true;
                    grdInvoiceOutDetail.Columns["MRNDate"].Visible = true;
                    grdInvoiceOutDetail.Columns["BatchNo"].Visible = true;
                    grdInvoiceOutDetail.Columns["ExpiryDate"].Visible = true;
                    grdInvoiceOutDetail.Columns["SerielNo"].Visible = true;
                    grdInvoiceOutDetail.Columns["PurchaseQuantity"].Visible = true;
                    grdInvoiceOutDetail.Columns["PurchasePrice"].Visible = true;
                    grdInvoiceOutDetail.Columns["PurchaseTax"].Visible = true;
                    grdInvoiceOutDetail.Columns["SupplierID"].Visible = true;
                    grdInvoiceOutDetail.Columns["SupplierName"].Visible = true;
                    grdInvoiceOutDetail.Columns["StockReferenceNo"].Visible = true;
                    grdInvoiceOutDetail.Columns["ModelNo"].Visible = true;
                    grdInvoiceOutDetail.Columns["ModelName"].Visible = true;
                }
                else
                {
                    grdInvoiceOutDetail.Columns["MRNNo"].Visible = false;
                    grdInvoiceOutDetail.Columns["MRNDate"].Visible = false;
                    grdInvoiceOutDetail.Columns["BatchNo"].Visible = false;
                    grdInvoiceOutDetail.Columns["ExpiryDate"].Visible = false;
                    grdInvoiceOutDetail.Columns["SerielNo"].Visible = false;
                    grdInvoiceOutDetail.Columns["PurchaseQuantity"].Visible = false;
                    grdInvoiceOutDetail.Columns["PurchasePrice"].Visible = false;
                    grdInvoiceOutDetail.Columns["PurchaseTax"].Visible = false;
                    grdInvoiceOutDetail.Columns["SupplierID"].Visible = false;
                    grdInvoiceOutDetail.Columns["SupplierName"].Visible = false;
                    grdInvoiceOutDetail.Columns["StockReferenceNo"].Visible = false;
                    grdInvoiceOutDetail.Columns["ModelNo"].Visible = false;
                    grdInvoiceOutDetail.Columns["ModelName"].Visible = false;
                }
            }
            catch (Exception)
            {
            }
        }
        private void setIOTabColumns(string docID)
        {
            try
            {
                if ((docID == "PRODUCTEXPORTINVOICEOUT") || (docID == "SERVICEEXPORTINVOICEOUT"))
                {
                    // tabControl1.TabPages.Add(tabExportDetail);
                    TabPage tp = tabControl1.TabPages["tabExportDetail"];
                    tp.Enabled = true;
                    cmbStateCode.Enabled = false;
                    //tp.Visible = true;
                }
                else
                {
                    //tabControl1.TabPages.Remove(tabExportDetail);
                    TabPage tp = tabControl1.TabPages["tabExportDetail"];
                    tp.Enabled = false;
                    cmbStateCode.Enabled = true;
                    //tp.Visible = false;
                }
            }
            catch (Exception)
            {
            }
        }

        private void cmbFreightTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void pnlList_Paint(object sender, PaintEventArgs e)
        {

        }
        //-----
        //comment handling procedurs and functions
        //-----
        private void btnGetComments_Click(object sender, EventArgs e)
        {
            ///*removeControlsFromCommenterPane*/l();
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
                            InvoiceOutHeaderDB iohdb = new InvoiceOutHeaderDB();
                            previoh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(previoh.CommentStatus);
                            previoh.ForwardUser = approverUID;
                            previoh.ForwarderList = previoh.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (iohdb.forwardInvoiceHeader(previoh))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(previoh, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredInvoiceOutHeader(listOption);
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
                tp.Enabled = false;
                tp.BackgroundImage = null;
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
                    string reverseStr = getReverseString(previoh.ForwarderList);
                    //do forward activities
                    previoh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(previoh.CommentStatus);
                    InvoiceOutHeaderDB iohdb = new InvoiceOutHeaderDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        previoh.ForwarderList = reverseStr.Substring(0, ind);
                        previoh.ForwardUser = reverseStr.Substring(ind + 3);
                        previoh.DocumentStatus = previoh.DocumentStatus - 1;
                    }
                    else
                    {
                        previoh.ForwarderList = "";
                        previoh.ForwardUser = "";
                        previoh.DocumentStatus = 1;
                    }
                    if (iohdb.reverseInvoiceOut(previoh))
                    {
                        MessageBox.Show("Invoice Out Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredInvoiceOutHeader(listOption);
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
        //private void removeControlsFromPayTermPanel()
        //{
        //    try
        //    {
        //        foreach (Control p in pnldgv.Controls)
        //            if (p.GetType() == typeof(DataGridView) || p.GetType() == typeof(Button))
        //            {
        //                p.Dispose();
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        private void btnListDocuments_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, previoh.TemporaryNo + "-" + previoh.TemporaryDate.ToString("yyyyMMddhhmmss"));
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
                    string subDir = previoh.TemporaryNo + "-" + previoh.TemporaryDate.ToString("yyyyMMddhhmmss");
                    ////DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    dgv.Enabled = false;
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
                                tabControl1.SelectedIndex = 5;
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
                btnCanceled.Visible = true; //cancelled option
                btnNew.Visible = false;
                btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                btnForward.Visible = false;
                btnApprove.Visible = false;
                btnUpdatePJV.Visible = false;
                chkPJVApprove.Visible = false;
                btnReverse.Visible = false;
                btnGetComments.Visible = false;
                txtComments.Visible = false;
                disableTabPages();
                clearTabPageControls();
                //----24/11/2016
                handleNewButton();
                handleGrdViewButton();
                handleGrdEditButton();
                //handleGrdPrintButton();
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
                    tabControl1.SelectedTab = tabInvoiceType;
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
                    txtComments.Visible = true;
                    tabControl1.SelectedTab = tabInvoiceDetail;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnUpdatePJV.Visible = true;
                    chkPJVApprove.Visible = true;
                    btnReverse.Visible = true;
                    disableTabPages();
                    tabControl1.SelectedTab = tabInvoiceDetail;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabInvoiceDetail;
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
                    btnCanceled.Visible = false;
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

                if (listOption == 4 && !isNewClicked)
                {
                    foreach (TabPage tp in tabControl1.TabPages)
                    {
                        tp.BackgroundImage = Properties.Resources._003_1;
                        //tp.BackgroundImage = Image.FromFile(@"D:\Smruti\Projects\ERP\CSLERP\CSLERP\Resources\Cancel_New.png");
                    }
                    //tabControl1.TabPages[].BackgroundImage = Properties.Resources._003;
                    btnApproved.UseVisualStyleBackColor = true;
                    btnCanceled.BackColor = Color.MediumAquamarine;
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
                btnCanceled.UseVisualStyleBackColor = true;
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
                txtComments.Text = "";
                grdInvoiceOutDetail.Rows.Clear();
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
        private void btnSelectPOInwardNo_Click(object sender, EventArgs e)
        {
            if (txtCustomerID.Text.Length == 0)
            {
                MessageBox.Show("select one Consignee");
                return;
            }
            if (grdInvoiceOutDetail.Rows.Count != 0)
            {
                DialogResult dialog = MessageBox.Show("Warning: InvoiceOut Details will removed?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdInvoiceOutDetail.Rows.Clear();
                    txtProductValue.Text = "";
                    txtTaxAmount.Text = "";
                    txtInvoiceAmount.Text = "";
                    txtInvoiceValueINR.Text = "";
                    txtProductValueINR.Text = "";
                    txtProductTaxINR.Text = "";
                    btnProductValue.Text = "";
                    txtDeliveryAddress.Text = "";
                    cmbStateCode.SelectedIndex = -1;
                    btnTaxAmount.Text = "";
                    txtProjectID.Text = "" ;
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

            frmPopup.Size = new Size(830, 300);
            string custid = txtCustomerID.Text;
            lv = POPIHeaderDB.SelectPODetailForInvoiceOut(PODocIdWRTIO, custid);
            // this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(830, 250));
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

            //pnlAddEdit.Controls.Add(pnllv);
            //pnllv.BringToFront();
            //pnllv.Visible = true;
        }
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                //btnSelectPO.Enabled = true;

                int kount = 0;
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
                string trackNo = "";
                string trackDate = "";
                string projID = "";
                int c = 0;
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        c++;
                        //btnSelectPOInwardNo.Enabled = true;
                        trackNo = trackNo + itemRow.SubItems[1].Text + ";";
                        trackDate = trackDate + itemRow.SubItems[2].Text + ";";
                        if (c == 1)
                        {
                            projID = itemRow.SubItems[6].Text;
                            txtDeliveryAddress.Text = itemRow.SubItems[7].Text;
                            txtPaymentTerms.Text = PTDefinitionDB.getPaymentTermString(itemRow.SubItems[8].Text);
                        }
                        //txtPOTrackNo.Text = itemRow.SubItems[1].Text;
                        //dtPOTrackDate.Value = Convert.ToDateTime(itemRow.SubItems[2].Text);
                    }
                }

                txtPOTrackNos.Text = trackNo;
                txtPOTrackDates.Text = trackDate;
                txtProjectID.Text = projID;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        private void lvCancel_Click1(object sender, EventArgs e)
        {
            try
            {
                //btnSelectPOInwardNo.Enabled = true;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        private void tabExportDetail_Click(object sender, EventArgs e)
        {

        }
        private void listView_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {

        }
        private ListView getLVForAllSelectedPO(string trackNos, string trackDates, string podocID)
        {

            try
            {
                lvTot = new ListView();
                lvTot.View = System.Windows.Forms.View.Details;
                lvTot.LabelEdit = true;
                lvTot.AllowColumnReorder = true;
                lvTot.CheckBoxes = true;
                lvTot.FullRowSelect = true;
                lvTot.GridLines = true;
                lvTot.Sorting = System.Windows.Forms.SortOrder.Ascending;
                lvTot.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("RefNo", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TempNo", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TempDate", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("PONo", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("PODate", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("StockItemID", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("stockItemName", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("ModelNo", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("ModelName", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Location", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Cust.ItemDescription", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Quantity", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Price", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("WarrantyDays", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TaxCode", -2, HorizontalAlignment.Left);

                lvTot.Columns[1].Width = 0;
                lvTot.Columns[2].Width = 0;
                lvTot.Columns[3].Width = 0;
                lvTot.Columns[8].Width = 0;
                lvTot.Columns[9].Width = 0;
                lvTot.Columns[7].Width = 300;
                lvTot.Columns[10].Width = 200;
                lvTot.Columns[11].Width = 300;
                if (podocID == "POPRODUCTINWARD")
                    lvTot.Columns[10].Width = 0;
                //else

                string[] trackNoArr = trackNos.Split(';');
                string[] trackDatesArr = trackDates.Split(';');
                for (int i = 0; i < trackNoArr.Length - 1; i++)
                {
                    ListView lv =
                        POPIHeaderDB.getPONoWiseStockListView(Convert.ToInt32(trackNoArr[i]), Convert.ToDateTime(trackDatesArr[i]), podocID);
                    lvTot.Items.AddRange((from ListViewItem item in lv.Items
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



        //private int getTotalListViewLength(ListView lv)
        //{
        //    int width = 0;
        //    List<int> widthArr = new List<int>();
        //    foreach(ListViewItem item in lv.Items)
        //    {
        //        int ItemWid = 0;
        //        int i = 0;
        //        foreach(ListViewItem.ListViewSubItem subItem in item.SubItems)
        //        {
        //            if (lv.Columns[i].Width != 0)
        //            {
        //                int subItemWid = subItem.Text.Length;
        //                int colHdrWid = lv.Columns[i].Text.Length;
        //                if (subItemWid > colHdrWid)
        //                    ItemWid = ItemWid + subItemWid;
        //                else
        //                    ItemWid = ItemWid + colHdrWid;
        //            }
        //            i++;
        //        }
        //        widthArr.Add(ItemWid);
        //    }
        //    width = widthArr.Max();
        //    return width;
        //}
        private void btnSelectItem_Click(object sender, EventArgs e)
        {
            if (txtPOTrackNos.Text.Length == 0)
            {
                MessageBox.Show("Select one PO tracking No");
                return;
            }
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;
            List<string> SelectedPOS = new List<string>();
            foreach (DataGridViewRow row in grdInvoiceOutDetail.Rows)
            {
                SelectedPOS.Add(row.Cells["POItemRefNo"].Value.ToString());
            }
            lv = getLVForAllSelectedPO(txtPOTrackNos.Text, txtPOTrackDates.Text, PODocIdWRTIO);
            if (SelectedPOS.Count != 0)
            {
                foreach (ListViewItem item in lv.Items)
                {
                    if (SelectedPOS.Contains(item.SubItems[1].Text)) //if corresponding poitem(RowID) available in grid
                    {
                        item.BackColor = Color.Yellow;
                    }
                }
            }
            //int width = getTotalListViewLength(lv);
            frmPopup.Size = new Size(1300, 400);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(1300, 350));

            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 365);
            lvOK.Click += new System.EventHandler(this.lvOK_Click2);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 365);
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
                    MessageBox.Show("Select only one item.");
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        if (docID.Equals("PRODUCTINVOICEOUT") || docID.Equals("PRODUCTEXPORTINVOICEOUT"))
                        {
                            POStockinfo = "";
                            //btnSelectItem.Enabled = true;
                            //pnllv.Visible = false;
                            string ProductID = itemRow.SubItems[6].Text;
                            string ModelNo = itemRow.SubItems[8].Text;
                            //[POStockrefNo,CustItemDesc,POQuty,Price,WarrantyDays,Taxcode,pono,podate]
                            POStockinfo = itemRow.SubItems[1].Text + Main.delimiter1 + itemRow.SubItems[11].Text + Main.delimiter1 + itemRow.SubItems[12].Text +
                                            Main.delimiter1 + itemRow.SubItems[13].Text + Main.delimiter1 + itemRow.SubItems[14].Text + Main.delimiter1 + itemRow.SubItems[15].Text +
                                            Main.delimiter1 + itemRow.SubItems[4].Text + Main.delimiter1 + itemRow.SubItems[5].Text;
                            frmPopup.Close();
                            frmPopup.Dispose();
                            showStockListView(ProductID, ModelNo);
                        }
                        else
                        {
                            AddGridDetailRowForService(itemRow);
                            //btnSelectItem.Enabled = true;
                            frmPopup.Close();
                            frmPopup.Dispose();
                        }
                        //AddGridDetailRowForIO(itemRow,1);
                    }
                }


            }
            catch (Exception ex)
            {
            }
        }
        private void lvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                //btnSelectItem.Enabled = true;
                frmPopup.Close();
                frmPopup.Dispose();

            }
            catch (Exception)
            {
            }
        }
        private void showStockListView(string pID, string MNo)
        {
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

            //frmPopup.Size = new Size(700, 300);
            lv = StockDB.getProductWiseStockListView(pID, MNo);
            //ListViewColumnSorter lvwColumnSorter;
            //int width = getTotalListViewLength(lv);
            frmPopup.Size = new Size(1300, 400);
            //lv.Bounds = new Rectangle(new Point(0, 0), new Size(700, 250));
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(1300, 350));
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
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
            //pnlAddEdit.Controls.Add(pnllv);
            //pnllv.BringToFront();
            //pnllv.Visible = true;
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
                    MessageBox.Show("Select only one item.");
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        AddGridDetailRowForProduct(itemRow);
                    }
                }
                //btnSelectItem.Enabled = true;
                frmPopup.Close();
                frmPopup.Dispose();

            }
            catch (Exception ex)
            {
            }
        }
        //-----
        private Boolean AddGridDetailRowForProduct(ListViewItem itemRow)
        {
            Boolean status = true;
            try
            {

                ////if (grdInvoiceOutDetail.Rows.Count > 0)
                ////{
                ////    foreach (DataGridViewRow row in grdInvoiceOutDetail.Rows)
                ////    {
                ////        if (grdInvoiceOutDetail.Rows[row.Index].Cells["StockReferenceNo"].Value.ToString() == itemRow.SubItems[1].Text)
                ////        {
                ////            MessageBox.Show("Record Alreadey selected. You are not allowed to reenter again.");
                ////            return false;
                ////        }
                ////    }
                ////    //if (!verifyAndReworkIODetailGridRows())
                ////    //{
                ////    //    return false;
                ////    //}

                ////}

                string[] POInfo = POStockinfo.Split(Main.delimiter1);
                //grdInvoiceOutDetail.Rows.Add();
                AddRowClick = true;
                if (!AddIODetailRow())
                {
                    MessageBox.Show("Adding Rows Error");
                    return false;
                }
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["LineNo"].Value = grdInvoiceOutDetail.RowCount;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["POItemRefNo"].Value = POInfo[0];
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["StockReferenceNo"].Value = itemRow.SubItems[1].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["gPONo"].Value = POInfo[6];
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["gPODate"].Value = POInfo[7];
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["Item"].Value = itemRow.SubItems[2].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["ItemName"].Value = itemRow.SubItems[3].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["ModelNo"].Value = itemRow.SubItems[4].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["ModelName"].Value = itemRow.SubItems[5].Text;
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["gTaxCode"]);
                TaxCodeDB.fillTaxCodeGridViewCombo(ComboColumn2, "");
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["gTaxCode"].Value = POInfo[5];
                //Location
                //grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["Location"].Value = POInfo[8];

                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["CustomerItemDescription"].Value = POInfo[1];
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["POQuantity"].Value = POInfo[2];
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["StockQuantity"].Value = itemRow.SubItems[8].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["Price"].Value = POInfo[3];
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["Value"].Value = 0;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["Tax"].Value = 0;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["Quantity"].Value = 0;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["WarrantyDays"].Value = POInfo[4];

                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["MRNNo"].Value = itemRow.SubItems[6].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["MRNDate"].Value = itemRow.SubItems[7].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["BatchNo"].Value = itemRow.SubItems[11].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["SerielNo"].Value = itemRow.SubItems[12].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["ExpiryDate"].Value = itemRow.SubItems[13].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["PurchaseQuantity"].Value = itemRow.SubItems[14].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["PurchasePrice"].Value = itemRow.SubItems[15].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["PurchaseTax"].Value = itemRow.SubItems[16].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["SupplierID"].Value = itemRow.SubItems[17].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["SupplierName"].Value = itemRow.SubItems[18].Text;
                grdInvoiceOutDetail.Columns["StockQuantity"].Visible = true;
                grdInvoiceOutDetail.Columns["POQuantity"].Visible = true;
                grdInvoiceOutDetail.Columns["Location"].Visible = false;
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        private Boolean AddGridDetailRowForService(ListViewItem itemRow)
        {
            Boolean status = true;
            try
            {

                if (grdInvoiceOutDetail.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in grdInvoiceOutDetail.Rows)
                    {
                        if (grdInvoiceOutDetail.Rows[row.Index].Cells["POItemRefNo"].Value.ToString() == itemRow.SubItems[1].Text)
                        {
                            MessageBox.Show("Record Alreadey selected. You are not allowed to reenter again.");
                            return false;
                        }
                    }
                    //if (!verifyAndReworkIODetailGridRows())
                    //{
                    //    return false;
                    //}
                }
                AddRowClick = true;
                if (!AddIODetailRow())
                {
                    MessageBox.Show("Adding Rows Error");
                    return false;
                }
                //lv.Columns.Add("Select", -2, HorizontalAlignment.Left); 0
                //lv.Columns.Add("RefNo", -2, HorizontalAlignment.Left); 1
                //lv.Columns.Add("TempNo", -2, HorizontalAlignment.Left); 2
                //lv.Columns.Add("TempDate", -2, HorizontalAlignment.Left); 3
                //lv.Columns.Add("TrackingNo", -2, HorizontalAlignment.Left); 4
                //lv.Columns.Add("TrackingDate", -2, HorizontalAlignment.Left); 5
                //lv.Columns.Add("StockItemID", -2, HorizontalAlignment.Left); 6 
                //lv.Columns.Add("stockItemName", -2, HorizontalAlignment.Left); 7
                //lv.Columns.Add("ModelNo", -2, HorizontalAlignment.Left); 8 
                //lv.Columns.Add("ModelName", -2, HorizontalAlignment.Left); 9 
                //lv.Columns.Add("Location", -2, HorizontalAlignment.Left); 10
                //lv.Columns.Add("Cust.ItemDescription", -2, HorizontalAlignment.Left); 11
                //lv.Columns.Add("Quantity", -2, HorizontalAlignment.Left); 12
                //lv.Columns.Add("Price", -2, HorizontalAlignment.Left); 13
                //lv.Columns.Add("WarrantyDays", -2, HorizontalAlignment.Left); 14
                //lv.Columns.Add("TaxCode", -2, HorizontalAlignment.Left); 15
                //grdInvoiceOutDetail.Rows.Add();
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["LineNo"].Value = grdInvoiceOutDetail.RowCount;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["POItemRefNo"].Value = itemRow.SubItems[1].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["Item"].Value = itemRow.SubItems[6].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["ItemName"].Value = itemRow.SubItems[7].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["ModelNo"].Value = itemRow.SubItems[8].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["ModelName"].Value = itemRow.SubItems[9].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["gPONo"].Value = itemRow.SubItems[4].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["gPODate"].Value = itemRow.SubItems[5].Text;
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["gTaxCode"]);
                TaxCodeDB.fillTaxCodeGridViewCombo(ComboColumn2, "");
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["gTaxCode"].Value = itemRow.SubItems[15].Text;
                ///Location
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["Location"].Value = itemRow.SubItems[10].Text;

                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["CustomerItemDescription"].Value = itemRow.SubItems[11].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["POQuantity"].Value = itemRow.SubItems[12].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["Quantity"].Value = 0;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["Price"].Value = itemRow.SubItems[13].Text;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["Value"].Value = 0;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["Tax"].Value = 0;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Cells["WarrantyDays"].Value = itemRow.SubItems[14].Text;
                grdInvoiceOutDetail.Columns["StockQuantity"].Visible = false;
                grdInvoiceOutDetail.Columns["POQuantity"].Visible = true;
                grdInvoiceOutDetail.Columns["Location"].Visible = true;
                grdInvoiceOutDetail.Rows[grdInvoiceOutDetail.RowCount - 1].Selected = true;
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        //private void cmbConsignee_SelectedValueChanged(object sender, EventArgs e)
        //{
        //    if (track)
        //        return;
        //    try
        //    {
        //        if (txtPOTrackNos.Text.Length != 0)
        //        {
        //            string s = cmbConsignee.FindString(custid).ToString();
        //            string ss = cmbConsignee.SelectedIndex.ToString();
        //            if (s == ss)
        //            {
        //                return;
        //            }
        //            DialogResult dialog = MessageBox.Show("Warning:\nPONo , PODate, Product Value, tax, InvoiceAmount and InoviceOut detail will be removed", "", MessageBoxButtons.OKCancel);
        //            if (dialog == DialogResult.OK)
        //            {
        //                s = cmbConsignee.SelectedIndex.ToString();
        //                txtPOTrackNos.Text = "";
        //                //dtPOTrackDate.Text = DateTime.Now.ToString();
        //                //cmbTaxCode.SelectedIndex = -1;
        //                txtTaxAmount.Text = "";
        //                txtProductValue.Text = "";
        //                txtInvoiceAmount.Text = "";
        //                txtInvoiceValueINR.Text = "";

        //                btnProductValue.Text = "";
        //                btnTaxAmount.Text = "";
        //                grdInvoiceOutDetail.Rows.Clear();
        //                //track = true;
        //            }
        //            else
        //            {
        //                cmbConsignee.SelectedIndex = cmbConsignee.FindString(custid);
        //                //track = true;
        //                return;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        private void txtINRInversionRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdInvoiceOutDetail.Rows.Count != 0 && txtInvoiceAmount.Text.Length != 0
                    && txtProductValue.Text.Length != 0 && txtINRInversionRate.Text.Length != 0)
                {
                    double dd = Convert.ToDouble(txtINRInversionRate.Text);
                    txtProductValueINR.Text = (Convert.ToDouble(txtProductValue.Text) * dd).ToString();
                    txtInvoiceValueINR.Text = (Convert.ToDouble(txtInvoiceAmount.Text) * dd).ToString();
                    txtProductTaxINR.Text = (Convert.ToDouble(txtTaxAmount.Text) * dd).ToString();
                    txtFreightForwardingINR.Text = (Convert.ToDouble(txtFreightForwarding.Text) * dd).ToString();
                }
                if (txtINRInversionRate.Text.Length == 0)
                {
                    txtProductValueINR.Text = "";
                    txtInvoiceValueINR.Text = "";
                    txtProductTaxINR.Text = "";
                    txtFreightForwardingINR.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private List<invoiceoutdetail> getInvOutDetails(invoiceoutheader ioh)
        {
            invoiceoutdetail iod = new invoiceoutdetail();

            List<invoiceoutdetail> IODetails = new List<invoiceoutdetail>();
            for (int i = 0; i < grdInvoiceOutDetail.Rows.Count; i++)
            {
                try
                {
                    iod = new invoiceoutdetail();
                    iod.DocumentID = ioh.DocumentID;
                    iod.TemporaryNo = ioh.TemporaryNo;
                    iod.TemporaryDate = ioh.TemporaryDate;
                    iod.StockItemID = grdInvoiceOutDetail.Rows[i].Cells["Item"].Value.ToString().Trim();
                    iod.PONo = Convert.ToInt32(grdInvoiceOutDetail.Rows[i].Cells["gPONo"].Value.ToString());
                    iod.PODate = Convert.ToDateTime(grdInvoiceOutDetail.Rows[i].Cells["gPODate"].Value.ToString());
                    iod.ModelNo = grdInvoiceOutDetail.Rows[i].Cells["ModelNo"].Value.ToString().Trim();
                    iod.TaxCode = grdInvoiceOutDetail.Rows[i].Cells["gTaxCode"].Value.ToString();
                    iod.Quantity = Convert.ToDouble(grdInvoiceOutDetail.Rows[i].Cells["Quantity"].Value);
                    iod.Price = Convert.ToDouble(grdInvoiceOutDetail.Rows[i].Cells["Price"].Value);
                    iod.Tax = Convert.ToDouble(grdInvoiceOutDetail.Rows[i].Cells["Tax"].Value);
                    iod.TaxDetails = grdInvoiceOutDetail.Rows[i].Cells["TaxDetails"].Value.ToString();
                    iod.BatchNo = grdInvoiceOutDetail.Rows[i].Cells["BatchNo"].Value.ToString();
                    iod.SerielNo = grdInvoiceOutDetail.Rows[i].Cells["SerielNo"].Value.ToString();
                    iod.ExpiryDate = Convert.ToDateTime(grdInvoiceOutDetail.Rows[i].Cells["ExpiryDate"].Value);
                    ////iod.StoreLocationID = grdPRDetail.Rows[i].Cells["StoreLocationID"].Value.ToString().Trim();
                    iod.StockReferenceNo = Convert.ToInt32(grdInvoiceOutDetail.Rows[i].Cells["StockReferenceNo"].Value);
                    IODetails.Add(iod);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("getInvOutDetails() : Error creating Invoice Out Details");
                    //status = false;
                }
            }
            return IODetails;
        }
        private void getTaxDetails(List<invoiceoutdetail> iodList)
        {
            try
            {
                double quantity = 0;
                double price = 0;
                double cost = 0.0;
                productvalue = 0.0;
                taxvalue = 0.0;
                string strtaxCode = "";
                //clear tax details table
                TaxDetailsTable.Rows.Clear();
                foreach (invoiceoutdetail iod in iodList)
                {
                    quantity = iod.Quantity;
                    price = iod.Price;
                    if (price != 0 && quantity != 0)
                    {
                        cost = Math.Round(quantity * price, 2);
                    }

                    try
                    {
                        strtaxCode = iod.TaxCode;
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
                        catch (Exception ex)
                        {
                            ttax1 = 0.0;
                        }
                        ttax2 = ttax2 + ttax1;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private string getTasDetailStr()
        {
            string strTax = "";
            try
            {
                for (int i = 0; i < (TaxDetailsTable.Rows.Count); i++)
                {
                    strTax = strTax + Convert.ToString(TaxDetailsTable.Rows[i][0]) + Main.delimiter1 +
                    Convert.ToString(TaxDetailsTable.Rows[i][1]) + Main.delimiter2;
                }
                //DialogResult dialog = MessageBox.Show(strTax, "Tax Details", MessageBoxButtons.OK);
            }
            catch (Exception)
            {
                MessageBox.Show("Error showing tax details");
            }
            return strTax;
        }
        private string DocIDStr()
        {
            string docIDString = "";
            if ((docID == "PRODUCTEXPORTINVOICEOUT") || (docID == "PRODUCTINVOICEOUT"))
            {
                docIDString = "PRODUCTEXPORTINVOICEOUT" + ";" + "PRODUCTINVOICEOUT";
            }
            else
            {
                docIDString = "SERVICEEXPORTINVOICEOUT" + ";" + "SERVICEINVOICEOUT";
            }
            return docIDString;
        }
        private ListView getStockAvailabilityWRTPOListView(List<invoiceoutdetail> IOHList, string podocID)
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
                lv.Columns.Add("StockItem ID", -2, HorizontalAlignment.Center);
                lv.Columns.Add("StockItem Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Model No", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Model Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("PO Quant", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Issued Quant(A)", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Present Quant(B)", -2, HorizontalAlignment.Center);
                lv.Columns.Add("A + B", -2, HorizontalAlignment.Center);
                lv.Columns[2].Width = 0;
                lv.Columns[3].Width = 200;
                lv.Columns[4].Width = 0;
                lv.Columns[5].Width = 150;
                foreach (invoiceoutdetail iod in IOHList)
                {
                    invoiceoutdetail iodEq = InvoiceOutHeaderDB.getItemWiseTotalQuantForPerticularPOInInvoiceOUt(iod.POItemReferenceNo, DocIDStr());
                    double poQuant = POPIHeaderDB.getPOQuantityForInvoiceOut(iod.POItemReferenceNo);
                    ListViewItem item1 = new ListViewItem(iod.PONo.ToString());
                    item1.SubItems.Add(iod.PODate.ToShortDateString());
                    item1.SubItems.Add(iod.StockItemID);
                    item1.SubItems.Add(iod.StockItemName);
                    item1.SubItems.Add(iod.ModelNo);
                    item1.SubItems.Add(iod.ModelName);
                    item1.SubItems.Add(poQuant.ToString());          //For PO Quantity
                    item1.SubItems.Add(iodEq.Quantity.ToString());  // For Total Accepted Quantity In All MRN for perticular Item
                    item1.SubItems.Add(iod.Quantity.ToString());    //For Enter Quantity IN Current MRN
                    item1.SubItems.Add((iod.Quantity + iodEq.Quantity).ToString());
                    if ((iodEq.Quantity + iod.Quantity) > poQuant)
                    {
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
        private void showQuantityAvailableForAllProductListView(List<invoiceoutdetail> IODList)
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
                lv = getStockAvailabilityWRTPOListView(IODList, PODocIdWRTIO);
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

        private Boolean validateIODetailProductquantity(List<invoiceoutdetail> iodList)
        {
            Boolean stat = true;
            foreach (invoiceoutdetail iod in iodList)
            {
                invoiceoutdetail iodEQ = InvoiceOutHeaderDB.getItemWiseTotalQuantForPerticularPOInInvoiceOUt(iod.POItemReferenceNo, DocIDStr());
                double poQuant = POPIHeaderDB.getPOQuantityForInvoiceOut(iod.POItemReferenceNo);
                double totQuant = iodEQ.Quantity;
                if ((totQuant + iod.Quantity) > poQuant)
                {
                    return false;
                }
            }
            return stat;
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

        private void btnBankAcRefNo_Click(object sender, EventArgs e)
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(600, 300);
            lv = CompanyBankDB.getBankListView(1); // Cellcomm Solution Company ID = 1
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(600, 250));

            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_ClickComp);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_ClickComp);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
        }

        private void lvOK_ClickComp(object sender, EventArgs e)
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
                    MessageBox.Show("Select one Bank");
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        txtBankRefNo.Text = itemRow.SubItems[1].Text;
                    }
                }
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        private void lvCancel_ClickComp(object sender, EventArgs e)
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

        //-- Validating Header and Detail String For Single Quotes

        private invoiceoutheader verifyHeaderInputString(invoiceoutheader ioh)
        {
            try
            {
                ioh.Remarks = Utilities.replaceSingleQuoteWithDoubleSingleQuote(ioh.Remarks);
                ioh.SpecialNote = Utilities.replaceSingleQuoteWithDoubleSingleQuote(ioh.SpecialNote);
                ioh.DeliveryAddress = Utilities.replaceSingleQuoteWithDoubleSingleQuote(ioh.DeliveryAddress);
                ioh.ConsigneeID = Utilities.replaceSingleQuoteWithDoubleSingleQuote(ioh.ConsigneeID);
                if ((docID == "PRODUCTEXPORTINVOICEOUT") || (docID == "SERVICEEXPORTINVOICEOUT"))
                {
                    ioh.FinalDestinationPlace = Utilities.replaceSingleQuoteWithDoubleSingleQuote(ioh.FinalDestinationPlace);
                    ioh.PreCarrierReceiptPlace = Utilities.replaceSingleQuoteWithDoubleSingleQuote(ioh.PreCarrierReceiptPlace);
                    ioh.TermsOfDelivery = Utilities.replaceSingleQuoteWithDoubleSingleQuote(ioh.TermsOfDelivery);
                    ioh.EntryPort = Utilities.replaceSingleQuoteWithDoubleSingleQuote(ioh.EntryPort);
                    ioh.ExitPort = Utilities.replaceSingleQuoteWithDoubleSingleQuote(ioh.ExitPort);
                }
            }
            catch (Exception ex)
            {
            }
            return ioh;
        }
        private void verifyDetailInputString()
        {
            try
            {
                for (int i = 0; i < grdInvoiceOutDetail.Rows.Count; i++)
                {
                    grdInvoiceOutDetail.Rows[i].Cells["Item"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdInvoiceOutDetail.Rows[i].Cells["Item"].Value.ToString());
                    grdInvoiceOutDetail.Rows[i].Cells["ItemName"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdInvoiceOutDetail.Rows[i].Cells["ItemName"].Value.ToString());
                    grdInvoiceOutDetail.Rows[i].Cells["ModelNo"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdInvoiceOutDetail.Rows[i].Cells["ModelNo"].Value.ToString());
                    grdInvoiceOutDetail.Rows[i].Cells["CustomerItemDescription"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdInvoiceOutDetail.Rows[i].Cells["CustomerItemDescription"].Value.ToString());
                    grdInvoiceOutDetail.Rows[i].Cells["SupplierID"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdInvoiceOutDetail.Rows[i].Cells["SupplierID"].Value.ToString());
                    grdInvoiceOutDetail.Rows[i].Cells["SupplierName"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdInvoiceOutDetail.Rows[i].Cells["SupplierName"].Value.ToString());
                    grdInvoiceOutDetail.Rows[i].Cells["BatchNo"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdInvoiceOutDetail.Rows[i].Cells["BatchNo"].Value.ToString());
                    grdInvoiceOutDetail.Rows[i].Cells["SerielNo"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdInvoiceOutDetail.Rows[i].Cells["SerielNo"].Value.ToString());
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
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
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdList.Rows)
                    {
                        if (clm == null || clm.Length == 0)
                        {
                            if (!row.Cells["PONo"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
                            {
                                row.Visible = false;
                            }
                        }
                        else
                        {

                            if (!row.Cells[clm].FormattedValue.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
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

        private void txtFreightCharge_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtINRInversionRate.Text.Length == 0 || Convert.ToDouble(txtINRInversionRate.Text) == 0)
                {
                    txtFreightForwarding.Text = "0";
                    txtFreightForwardingINR.Text = "0";
                }
                else
                {
                    txtFreightForwarding.Text = (Convert.ToDouble(txtFreightCharge.Text) + Convert.ToDouble(txtTaxAmount.Text)).ToString();
                    txtFreightForwardingINR.Text = (Convert.ToDouble(txtFreightForwarding.Text) * Convert.ToDouble(txtINRInversionRate.Text)).ToString();
                }
                txtInvoiceAmount.Text = (Convert.ToDouble(txtProductValue.Text) + Convert.ToDouble(txtFreightForwarding.Text) + Convert.ToDouble(txtTaxAmount.Text)).ToString();
                txtInvoiceValueINR.Text = (Convert.ToDouble(txtInvoiceAmount.Text) * Convert.ToDouble(txtINRInversionRate.Text)).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error");
            }
        }

        private void btnUpdatePJV_Click(object sender, EventArgs e)
        {
            try
            {
                string txt = btnUpdatePJV.Text;
                EditJV showJv;
                invoiceinheader iih = new invoiceinheader();
                string invstr = previoh.DocumentID + ";" + previoh.TemporaryNo + ";" + previoh.TemporaryDate;
                if (txt == "Update SJV")
                {
                    showJv = new EditJV("SJV", invstr, previoh.INRAmount, true);
                }
                else
                {
                    showJv = new EditJV("SJV", invstr, previoh.INRAmount, false);
                }
                showJv.Text = "SJV";
                showJv.ShowDialog();
                this.RemoveOwnedForm(showJv);
            }
            catch (Exception ex)
            {
            }
        }

        private void chkPJVApprove_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnSelCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPOTrackNos.Text.Length != 0)
                {
                    DialogResult dialog = MessageBox.Show("Warning:\nPONo , PODate, Product Value, tax, InvoiceAmount and InoviceOut detail will be removed", "", MessageBoxButtons.OKCancel);
                    if (dialog == DialogResult.OK)
                    {
                        txtPOTrackNos.Text = "";
                        txtPOTrackDates.Text = "";
                        txtTaxAmount.Text = "";
                        txtProductValue.Text = "";
                        txtInvoiceAmount.Text = "";
                        txtInvoiceValueINR.Text = "";
                        txtDeliveryAddress.Text = "";
                        cmbStateCode.SelectedIndex = -1;
                        btnProductValue.Text = "";
                        btnTaxAmount.Text = "";
                        grdInvoiceOutDetail.Rows.Clear();
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
                chkBoxCustomer.SetItemChecked(0, true);
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
                grdCustList = custDB.getGridViewForCustomerListNew("Customer");

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
                    ///cust = CustomerDB.getCustomerDetails(txtCustomerID.Text.Trim());
                    //txtDeliveryAddress.Text = cust.DeliveryAddress;
                    /// txtStateCode.Text = cust.StateCode;
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
                filterCustGridData();
            }
            catch (Exception ex)
            {

            }
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
                txtSearch.Text = "";
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

        private void InvoiceOutHeader_Enter(object sender, EventArgs e)
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

        private void btnAdvReceipts_Click(object sender, EventArgs e)
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

                frmPopup.Size = new Size(800, 370);
                string InvDocID = docID;
                int InvTempNo = Convert.ToInt32(txtTemporarryNo.Text);
                DateTime InvTempDate = Convert.ToDateTime(dtTempDate.Value);

                voucherGrid = InvoiceOutHeaderDB.getGridViewOFRVForAdvAdjustment(txtCustomerID.Text.Trim());
                voucherGrid.Bounds = new Rectangle(new Point(0, 27), new Size(800, 300));
                List<invoiceoutreceipts> ReceiveListAvail = InvoiceOutHeaderDB.getInvoiceOutAdvPaymentDetails(InvTempNo, InvTempDate, InvDocID);//If invoice paid against RV
                foreach (DataGridViewRow row in voucherGrid.Rows)
                {
                    string RVDocID = row.Cells["VoucherID"].Value.ToString();
                    int RVRNo = Convert.ToInt32(row.Cells["VoucherNO"].Value);
                    DateTime RVRDate = Convert.ToDateTime(row.Cells["VoucherDate"].Value);
                    decimal amtAdjusted = Convert.ToDecimal(row.Cells["AmountAdjusted"].Value);
                    decimal VoucherAmnt = Convert.ToDecimal(row.Cells["VoucherAmt"].Value);

                    invoiceoutreceipts invReceipt = receiveList.FirstOrDefault(rec => rec.RVDocumentID == RVDocID  && rec.RVNo == RVRNo && rec.RVDate == RVRDate);
                    invoiceoutreceipts invReceiptAvail = new invoiceoutreceipts();
                    if (invReceipt != null)
                    {
                        invReceiptAvail = ReceiveListAvail.FirstOrDefault(rec => rec.RVDocumentID == invReceipt.RVDocumentID && rec.RVNo == invReceipt.RVNo && rec.RVDate == invReceipt.RVDate);
                    }

                    if (invReceipt != null && invReceiptAvail != null)
                    {
                        row.Cells["AmountAdjusted"].Value = amtAdjusted - invReceiptAvail.Amount;
                        row.Cells["AmountToAdjust"].Value = invReceipt.Amount;
                    }
                    else if (invReceipt != null && invReceiptAvail == null)
                    {
                        row.Cells["AmountAdjusted"].Value = amtAdjusted;
                        row.Cells["AmountToAdjust"].Value = invReceipt.Amount;
                    }
                    else if (VoucherAmnt <= amtAdjusted) //If full invoice amount is adjusted
                    {
                        row.Visible = false;
                    }
                }
                voucherGrid.Columns["AmountToAdjust"].DefaultCellStyle.BackColor = Color.MistyRose;            
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
                    if (ToAdjAmnt == 0 || (AdjAmnt + ToAdjAmnt) > pvAmnt)
                    {
                        MessageBox.Show("Row Selection Error. Please check Amount to Adjust.");
                        TotalToAdjust = 0;
                        return;
                    }
                    TotalToAdjust = TotalToAdjust + ToAdjAmnt;
                }

                invoiceoutreceipts rec = new invoiceoutreceipts();
                receiveList.Clear();
                foreach (var row in checkedRows)
                {
                    //InivDetails = InivDetails + row.Cells["DocID"].Value.ToString() + Main.delimiter1 + row.Cells["DocNO"].Value.ToString() +
                    //                            Main.delimiter1 + Convert.ToDateTime(row.Cells["DocDate"].Value).ToString("yyyy-MM-dd") + Main.delimiter2;
                    rec = new invoiceoutreceipts();
                    rec.RVDocumentID = row.Cells["VoucherID"].Value.ToString();
                    rec.RVNo = Convert.ToInt32(row.Cells["VoucherNO"].Value);
                    rec.RVDate = Convert.ToDateTime(row.Cells["VoucherDate"].Value);
                    rec.RVTemporaryNo = Convert.ToInt32(row.Cells["VoucherTempNo"].Value);
                    rec.RVTemporaryDate = Convert.ToDateTime(row.Cells["VoucherTempDate"].Value);
                    rec.CustomerID = txtCustomerID.Text.Trim();
                    rec.Amount = Convert.ToDecimal(row.Cells["AmountToAdjust"].Value);
                    receiveList.Add(rec);
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

        private void cmbCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string CurID = ((Structures.ComboBoxItem)cmbCurrency.SelectedItem).HiddenValue;
                if (CurID == "INR")
                {
                    txtINRInversionRate.Text = "1";
                    txtINRInversionRate.Enabled = false;
                }
                else
                {
                    txtINRInversionRate.Text = "";
                    txtINRInversionRate.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                txtINRInversionRate.Text = "";
                txtINRInversionRate.Enabled = true;
            }
        }

        private void btnSelProjectID_Click(object sender, EventArgs e)
        {
            try
            {
                showProjectIDDataGridView();
            }
            catch (Exception ex)
            {
            }
        }
        //showing gridview for ProjectID
        private void showProjectIDDataGridView()
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
                lblSearch.Location = new System.Drawing.Point(240, 5);
                lblSearch.AutoSize = true;
                lblSearch.Text = "Search by ProjectID";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                tb = new TextBox();
                tb.Size = new Size(200, 18);
                tb.Location = new System.Drawing.Point(380, 3);
                tb.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                tb.ForeColor = Color.Black;
                tb.TextChanged += new System.EventHandler(this.tb_TextChangedInEmpGridList);
                tb.TabIndex = 0;
                tb.Focus();
                frmPopup.Controls.Add(tb);

                ProjectHeaderDB projDB = new ProjectHeaderDB();
                projGRid = projDB.getGridViewForProjectList();

                projGRid.Bounds = new Rectangle(new Point(0, 27), new Size(610, 300));
                frmPopup.Controls.Add(projGRid);
                foreach (DataGridViewColumn cells in projGRid.Columns)
                {
                    if (cells.CellType != typeof(DataGridViewCheckBoxCell))
                        cells.ReadOnly = true;
                }
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
            try
            {
                var checkedRows = from DataGridViewRow r in projGRid.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("Select one Project");
                    return;
                }
                foreach (var row in checkedRows)
                {
                    //cmbProjectID.SelectedIndex = cmbProjectID.FindString(row.Cells["ProjectID"].Value.ToString());
                    txtProjectID.Text = row.Cells["ProjectID"].Value.ToString();
                }
                //txtProductCodes.Text = iolist;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
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
        private void tb_TextChangedInEmpGridList(object sender, EventArgs e)
        {
            try
            {
                filterProjGridData();
            }
            catch (Exception ex)
            {

            }
        }
        private void filterProjGridData()
        {
            try
            {
                projGRid.CurrentCell = null;
                foreach (DataGridViewRow row in projGRid.Rows)
                {
                    row.Visible = true;
                }
                if (tb.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in projGRid.Rows)
                    {
                        if (!row.Cells["ProjectID"].Value.ToString().Trim().ToLower().Contains(tb.Text.Trim().ToLower()))
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

        private void btnCanceled_Click(object sender, EventArgs e)
        {
            try
            {
                if (getuserPrivilegeStatus() == 2)
                {
                    listOption = 7; //viewer
                }
                else
                {
                    listOption = 4;
                }
                ListFilteredInvoiceOutHeader(listOption);
            }
            catch (Exception ex)
            {
            }
           
        }

        private System.Data.DataTable taxDetails4Print(List<invoiceoutdetail> IODetails,string documentID)
        {
            int HSNLength = 0;
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataTable dtTax = new System.Data.DataTable();
            try
            {
                if (documentID == "PRODUCTINVOICE")
                {
                    HSNLength = 4;
                }
                else if (documentID == "SERVICEINVOICE")
                {
                    HSNLength = 6;
                }

                {
                    dt.Columns.Add("HSNCode", typeof(string));
                    dt.Columns.Add("TaxCode", typeof(string));
                    dt.Columns.Add("Amount", typeof(double));
                    dt.Columns.Add("TaxAmount", typeof(double));
                    dt.Columns.Add("TaxItem", typeof(string));
                    dt.Columns.Add("TaxItemPercentage", typeof(double));
                    dt.Columns.Add("TaxItemAmount", typeof(double));
                }
                //fill hsn code wise tax details in dt
                foreach (invoiceoutdetail iod in IODetails)
                {
                    string tstr = iod.TaxDetails;
                    string[] lst1 = tstr.Split('\n');
                    for (int j = 0; j < lst1.Length - 1; j++)
                    {
                        string[] lst2 = lst1[j].Split('-');
                        if (Convert.ToDouble(lst2[1]) > 0)
                        {
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1][0] = iod.HSNCode.Substring(0, HSNLength);
                            dt.Rows[dt.Rows.Count - 1][1] = iod.TaxCode;
                            dt.Rows[dt.Rows.Count - 1][2] = iod.Quantity * iod.Price;
                            dt.Rows[dt.Rows.Count - 1][3] = iod.Tax;
                            dt.Rows[dt.Rows.Count - 1][4] = lst2[0];
                            dt.Rows[dt.Rows.Count - 1][5] = iod.HSNCode; //need to replace with percentage
                            dt.Rows[dt.Rows.Count - 1][6] = lst2[1];
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }

            try
            {
                //fill tax rate for each tax item in dt
                TaxCodeWorkingDB tcwdb = new TaxCodeWorkingDB();
                List<taxcodeworking> tcwDetails = tcwdb.getTaxCodeDetails();
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    foreach (taxcodeworking tcwd in tcwDetails)
                    {
                        if (dt.Rows[i][1].ToString() == tcwd.TaxCode && dt.Rows[i][4].ToString() == tcwd.TaxItemName)
                        {
                            dt.Rows[i][5] = tcwd.OperatorValue;
                            break;
                        }
                    }

                }
                //prepare HSN Code wise totals in a new table
                System.Data.DataTable dttotal = new System.Data.DataTable();
                dttotal = dt.Copy();
                dttotal.Clear();

                for (int i = 0; i < (dt.Rows.Count); i++)
                {

                    Boolean fount = false;
                    string tstr1 = dt.Rows[i][0].ToString(); 
                    string tstr2 = dt.Rows[i][4].ToString();
                    string tstr3 = dt.Rows[i][5].ToString();
                    for (int j = 0; j < (dttotal.Rows.Count); j++)
                    {
                        string tstr4 = dttotal.Rows[j][0].ToString(); 
                        string tstr5 = dttotal.Rows[j][4].ToString();
                        string tstr6 = dttotal.Rows[j][5].ToString();

                        if (tstr1 == tstr4 && tstr2 == tstr5 && tstr3 == tstr6)
                        {
                            dttotal.Rows[j][2] = Convert.ToDouble(dttotal.Rows[j][2].ToString()) + 
                                Convert.ToDouble(dt.Rows[i][2].ToString());
                            dttotal.Rows[j][6] = Convert.ToDouble(dttotal.Rows[j][6].ToString()) +
                                Convert.ToDouble(dt.Rows[i][6].ToString());
                            fount = true;
                        }
                    }
                    if (!fount)
                    {
                        dttotal.ImportRow(dt.Rows[i]);
                    }
                }
                string tstr = "";
                ////for (int i = 0; i < (dttotal.Rows.Count); i++)
                ////{
                ////    tstr = tstr+
                ////        dttotal.Rows[i][0].ToString() + "," +
                ////        dttotal.Rows[i][1].ToString() + "," +
                ////        dttotal.Rows[i][2].ToString() + "," +
                ////        dttotal.Rows[i][3].ToString() + "," +
                ////        dttotal.Rows[i][4].ToString() + "," +
                ////        dttotal.Rows[i][5].ToString() + "," +
                ////        dttotal.Rows[i][6].ToString() + "\n";
                   
                ////}
                ////MessageBox.Show(tstr);
                //create print table
                tstr = "";
                //find distinct tax item in dttotal
                DataTable dtDistinct = dttotal.AsEnumerable().GroupBy(row => row.Field<string>("TaxItem")).Select(group => group.First()).CopyToDataTable();
                ////for (int i = 0; i < (dtDistinct.Rows.Count); i++)
                ////{
                ////    tstr = tstr +
                ////        dtDistinct.Rows[i][0].ToString() + "," +
                ////        dtDistinct.Rows[i][1].ToString() + "," +
                ////        dtDistinct.Rows[i][2].ToString() + "," +
                ////        dtDistinct.Rows[i][3].ToString() + "," +
                ////        dtDistinct.Rows[i][4].ToString() + "," +
                ////        dtDistinct.Rows[i][5].ToString() + "," +
                ////        dtDistinct.Rows[i][6].ToString() + "\n";

                ////}
                ////MessageBox.Show(tstr);
                
                //create columns in dttax table. dynamically creating the columns for each tax item
                {
                    dtTax.Columns.Add("HSNCode", typeof(string));
                    dtTax.Columns.Add("Amount", typeof(double));
                    for (int i=0; i<dtDistinct.Rows.Count && i < 3;i++)
                    {
                        dtTax.Columns.Add(dtDistinct.Rows[i][4].ToString(), typeof(string));
                        dtTax.Columns.Add(dtDistinct.Rows[i][4].ToString()+"Amount", typeof(double));
                    }
                    dtTax.Columns.Add("Total", typeof(double));
                }
                //add data in dttax table
                for (int i = 0; i < (dttotal.Rows.Count); i++)
                {
                    Boolean hsnFount = false;
                    string tstr1 = dttotal.Rows[i][0].ToString(); //for domestic
                    int j = 0;
                    for (j = 0; j < (dtTax.Rows.Count); j++)
                    {
                        string tstr2 = dtTax.Rows[j][0].ToString(); //for domestic
                        if (tstr1 == tstr2)
                        {
                            hsnFount = true;
                            break;
                        }
                    }
                    if (!hsnFount)
                    {
                        dtTax.Rows.Add();
                        j = dtTax.Rows.Count - 1;
                        dtTax.Rows[j][0] = tstr1;
                        dtTax.Rows[j][1] = dttotal.Rows[i][2]; ;
                    }
                    string tstr3 = dttotal.Rows[i][4].ToString();
                    string tstr4 = dttotal.Rows[i][4].ToString() + "Amount";
                    try
                    {
                        dtTax.Rows[j][tstr3] = dttotal.Rows[i][5];
                        dtTax.Rows[j][tstr4] = dttotal.Rows[i][6];
                        string t1 = String.IsNullOrEmpty(dtTax.Rows[j]["Total"].ToString()) ? "0" : dtTax.Rows[j]["Total"].ToString();
                        string t2 = String.IsNullOrEmpty(dttotal.Rows[i][6].ToString()) ? "0" : dttotal.Rows[i][6].ToString();
                        double d1 = Convert.ToDouble(t1) + Convert.ToDouble(t2);
                        dtTax.Rows[j]["Total"] = d1;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error creating HSN wise tax summary");
                    }
                }
                tstr = "";
                ////double td1=0, td2=0, td3 = 0;
                ////for (int i = 0; i < (dtTax.Rows.Count); i++)
                ////{
                ////    for (int j=0; j<dtTax.Columns.Count;j++)
                ////    {
                ////        tstr = tstr + dtTax.Rows[i][j].ToString() + ",";
                ////    }
                ////    tstr = tstr+"\n";
                ////    td1 = td1 + Convert.ToDouble(dtTax.Rows[i][1].ToString());
                ////    td2 = td2 + Convert.ToDouble(dtTax.Rows[i][dtTax.Columns.Count-1].ToString());
                ////}

                ////MessageBox.Show(tstr);
                ////MessageBox.Show(td1.ToString());
                ////MessageBox.Show(td2.ToString());
            }

            catch (Exception ex)
            {
                MessageBox.Show("taxDetails4Print() : Error - " + ex.ToString());
            }
            return dtTax;
        }
    }
}

