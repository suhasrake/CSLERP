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
using System.Globalization;
using System.Collections;

namespace CSLERP
{
    public partial class POPIHeader : System.Windows.Forms.Form
    {
        string[] docList = { "POPRODUCTINWARD", "POSERVICEINWARD", "PAFPRODUCTINWARD", "PAFSERVICEINWARD" };
        string docID = "";
        string chkDocID = "";
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        TextBox txtSearchCust = new TextBox();
        TextBox txtSearchStock = new TextBox();
        Timer filterTimer1 = new Timer();
        CheckedListBox chkBoxCustomer = new CheckedListBox();
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        double productvalue = 0.0;
        double taxvalue = 0.0;
        Boolean userIsACommenter = false;
        ListView lv = new ListView();
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        popiheader prevpopi;
        TreeView tv = new TreeView();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        Panel pnlModel = new Panel();
        DataGridView grdStock = new DataGridView();
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Form frmPopup = new Form();
        int descClickRowIndex = -1;
        RichTextBox txtDesc = new RichTextBox();
        Boolean AddRowClick = false;
        Timer filterTimer = new Timer();
        DataGridView grdCustList = new DataGridView();
        string colName = "";
        DataGridView projGRid = new DataGridView();
        TextBox tb = new TextBox();
        List<documentreceiver> DocREcvList = new List<documentreceiver>();
        string DocRecvListstr = "";
        Boolean isViewMode = false;
        public POPIHeader()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void POPIHeader_Load(object sender, EventArgs e)
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
            DocREcvList = Main.DocumentReceivers.Where(rec => docList.Contains(rec.DocumentID)).ToList();
            DocRecvListstr = getRecvDocString(DocREcvList);
            ListFilteredStockOBHeader(listOption);
        }
        private string getRecvDocString(List<documentreceiver> rcvlist)
        {
            string strList = "";
            if (rcvlist.Count == 0)
            {
                strList = "";
            }
            else
            {
                ArrayList arrStr = new ArrayList();
                foreach (documentreceiver rcvr in rcvlist)
                {
                    string qry = "( DocumentID = '" + rcvr.DocumentID + "' and OfficeID = '" + rcvr.OfficeID + "')";
                    if (docList.Contains(rcvr.DocumentID) && !arrStr.Contains(qry))
                    {
                        arrStr.Add("( DocumentID = '" + rcvr.DocumentID + "' and OfficeID = '" + rcvr.OfficeID + "')");
                    }
                }
                strList = string.Join(" or ", arrStr.ToArray());
            }
            return strList;
        }
        private void ListFilteredStockOBHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                POPIHeaderDB popihdb = new POPIHeaderDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                //foreach()
                List<popiheader> POPIHeaders = popihdb.getFilteredPOPIHeader(userString, option, userCommentStatusString, DocRecvListstr);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (popiheader popih in POPIHeaders)
                {
                    if (option == 1)
                    {
                        if (popih.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = popih.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = popih.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ReferenceNo"].Value = popih.ReferenceNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = popih.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = popih.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTrackingNo"].Value = popih.TrackingNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTrackingDate"].Value = popih.TrackingDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerID"].Value = popih.CustomerID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerName"].Value = popih.CustomerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["OfficeID"].Value = popih.OfficeID;
                    grdList.Rows[grdList.RowCount - 1].Cells["OfficeName"].Value = popih.OfficeName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerPONo"].Value = popih.CustomerPONO;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerPODate"].Value = popih.CustomerPODate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDeliveryDate"].Value = popih.DeliveryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["ValidityDate"].Value = popih.ValidityDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPaymentTerms"].Value = popih.PaymentTerms;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPaymentMode"].Value = popih.PaymentMode;
                    grdList.Rows[grdList.RowCount - 1].Cells["gFreightTerms"].Value = popih.FreightTerms;
                    grdList.Rows[grdList.RowCount - 1].Cells["gFreightCharge"].Value = popih.FreightCharge;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyID"].Value = popih.CurrencyID;
                    grdList.Rows[grdList.RowCount - 1].Cells["ExchangeRate"].Value = popih.ExchangeRate;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProjectID"].Value = popih.ProjectID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gBillingAddress"].Value = popih.BillingAddress;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDeliveryAddress"].Value = popih.DeliveryAddress;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProductValue"].Value = popih.ProductValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTaxAmount"].Value = popih.TaxAmount;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPOValue"].Value = popih.POValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProductValueINR"].Value = popih.ProductValueINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["TaxAmountINR"].Value = popih.TaxAmountINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["POValueINR"].Value = popih.POValueINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["gRemarks"].Value = popih.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = popih.status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = popih.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["ClosingStatus"].Value = popih.ClosingStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = popih.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = popih.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ApproveUser"].Value = popih.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = popih.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = popih.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatusNo"].Value = ComboFIll.getDocumentStatusString(popih.DocumentStatus);
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = popih.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatusNo"].Value = popih.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CmtStatus"].Value = popih.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = popih.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = popih.ForwarderList;
                    if (popih.ClosingStatus == 1)
                    {
                        grdList.Rows[grdList.RowCount - 1].DefaultCellStyle.BackColor = Color.Tan;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in PO Product Inward Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;
            txtSearch.Text = "";
            //filterGridData();
            ////grdList.Columns["Creator"].Visible = true;
            ////grdList.Columns["Forwarder"].Visible = true;
            ////grdList.Columns["Approver"].Visible = true;

            //Main.itemPriv[0]: View
            //Main.itemPriv[1]: Add
            //Main.itemPriv[2]: Edit
            //Main.itemPriv[3]: Delete
            ///If one person does not have previlise for either Edit or add , close/Close request button visible to false.
            if ((option == 3 || option == 6) && (Main.itemPriv[1] || Main.itemPriv[2]))
            {
                grdList.Columns["CloseRequest"].Visible = true;
                grdList.Columns["Close"].Visible = true;
            }
            else
            {
                grdList.Columns["CloseRequest"].Visible = false;
                grdList.Columns["Close"].Visible = false;
            }

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
            OfficeDB.fillOfficeComboNew(cmbOfficeID);
            CatalogueValueDB.fillCatalogValueComboNew(cmbPaymentMode, "PaymentMode");
            CatalogueValueDB.fillCatalogValueComboNew(cmbFreightTerms, "Freight");
            CatalogueValueDB.fillCatalogValueComboNew(cmbPOType, "POType");
            //CustomerDB.fillLedgerTypeComboNew(cmbCustomer, "Customer");
            //cmbCustomer.DropDownWidth = cmbCustomer.DropDownWidth + 50;
            //TaxCodeDB.fillTaxCodeCombo(cmbTaxCode);
            ProjectHeaderDB.fillprojectCombo(cmbProjectID);

            dtTempDate.Format = DateTimePickerFormat.Custom;
            dtTempDate.CustomFormat = "dd-MM-yyyy";
            dtTempDate.Enabled = false;
            dtTrackDate.Format = DateTimePickerFormat.Custom;
            dtTrackDate.CustomFormat = "dd-MM-yyyy";
            dtTrackDate.Enabled = false;
            dtCustomerPODate.Format = DateTimePickerFormat.Custom;
            dtCustomerPODate.CustomFormat = "dd-MM-yyyy";

            dtdeliveryDate.Format = DateTimePickerFormat.Custom;
            dtdeliveryDate.CustomFormat = "dd-MM-yyyy";
            dtValidityDate.Format = DateTimePickerFormat.Custom;
            dtValidityDate.CustomFormat = "dd-MM-yyyy";
            txtPaymentTerms.Enabled = false;
            txtTemporarryNo.Enabled = false;
            dtTempDate.Enabled = false;
            chkDuplicateRow.Checked = false;
            txtTrackingNo.Enabled = false;
            dtTrackDate.Enabled = false;
            btnForward.Visible = false;
            btnApprove.Visible = false;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();

            //create tax details table for tax breakup display
            {
                TaxDetailsTable.Columns.Add("TaxItem", typeof(string));
                TaxDetailsTable.Columns.Add("TaxAmount", typeof(double));
            }

            grdPOPIDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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
            txtTrackingNo.TabIndex = 2;
            dtTrackDate.TabIndex = 3;
            txtReferenceNo.TabIndex = 4;
            btnSelCustomer.TabIndex = 5;
            txtCustomerPONo.TabIndex = 6;
            dtCustomerPODate.TabIndex = 7;
            dtdeliveryDate.TabIndex = 8;
            dtValidityDate.TabIndex = 9;
            txtPaymentTerms.TabIndex = 10;
            btnPaymentTermsSelection.TabIndex = 11;
            cmbPaymentMode.TabIndex = 12;
            cmbFreightTerms.TabIndex = 13;
            txtFreightCharge.TabIndex = 14;
            cmbOfficeID.TabIndex = 15;
            cmbProjectID.TabIndex = 16;
            cmbCurrency.TabIndex = 17;
            txtExchangeRate.TabIndex = 18;
            txtBillingAddress.TabIndex = 19;
            txtDeliveryAddress.TabIndex = 20;
            txtRemarks.TabIndex = 21;
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
                grdPOPIDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                //----------clear temperory panels

                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                //----------
                //cmbTaxCode.SelectedIndex = -1;
                cmbProjectID.SelectedIndex = -1;
                chkDuplicateRow.Checked = false;
                cmbPaymentMode.SelectedIndex = -1;
                txtCustomerID.Text = "";
                txtCustName.Text = "";
                cmbCurrency.SelectedIndex = -1;
                cmbOfficeID.SelectedIndex = -1;
                try
                {
                    cmbFreightTerms.SelectedItem = null;
                }
                catch (Exception ex)
                {
                }
                cmbPOType.SelectedItem = null;

                txtTemporarryNo.Text = "";
                dtTempDate.Value = DateTime.Parse("1900-01-01");
                txtTrackingNo.Text = "";
                dtTrackDate.Value = DateTime.Parse("1900-01-01");
                txtCustomerPONo.Text = "";
                dtCustomerPODate.Value = DateTime.Today.Date;
                dtdeliveryDate.Value = DateTime.Today.Date;
                dtValidityDate.Value = DateTime.Today.Date;
                txtFreightCharge.Text = "";
                txtBillingAddress.Text = "";
                // txtValidityDays.Text = "";
                txtDeliveryAddress.Text = "";
                txtProductValue.Text = "";
                txtTaxAmount.Text = "";
                txtPOValue.Text = "";
                txtExchangeRate.Text = "";
                txtProductValueINR.Text = "";
                txtTaxAmountINR.Text = "";
                txtPOValueINR.Text = "";
                txtRemarks.Text = "";
                btnProductValue.Text = "0";
                btnTaxAmount.Text = "0";
                tabPOType.Enabled = true;
                tabPOType.Visible = true;
                tabPOHeader.Visible = false;
                tabPODetail.Visible = false;
                txtPaymentTerms.Text = "";
                txtReferenceNo.Text = "";
                commentStatus = "";
                prevpopi = new popiheader();
                removeControlsFromCommenterPanel();
                removeControlsFromForwarderPanel();
                removeControlsFromModelPanel();
                removeControlsFromPayTermPanel();
                removeControlsFromForwarderPanelTV();
                descClickRowIndex = -1;
                AddRowClick = false;
                isViewMode = false;
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
                btnSave.Text = "Save";
                pnlAddEdit.Visible = true;
                closeAllPanels();
                pnlAddEdit.Visible = true;
                tabControl1.SelectedTab = tabPOType;
                cmbPOType.Enabled = true;
                setButtonVisibility("btnNew");
                AddRowClick = false;
                chkDuplicateRow.Checked = false;
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
                AddPOPIDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddPOPIDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdPOPIDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkPOPIDetailGridRows())
                    {
                        return false;
                    }
                }
                grdPOPIDetail.Rows.Add();
                int kount = grdPOPIDetail.RowCount;
                grdPOPIDetail.Rows[kount - 1].Cells["LineNo"].Value = kount;
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdPOPIDetail.Rows[kount - 1].Cells["gTaxCode"]);
                TaxCodeDB.fillTaxCodeGridViewCombo(ComboColumn2, "");
                if (docID == "POPRODUCTINWARD" || docID == "PAFPRODUCTINWARD")
                {
                    grdPOPIDetail.Rows[kount - 1].Cells["ProductItem"].Value = "";
                    grdPOPIDetail.Rows[kount - 1].Cells["gOfficeID"].Value = "";
                    grdPOPIDetail.Rows[kount - 1].Cells["ServiceItem"].Value = "";
                    grdPOPIDetail.Rows[kount - 1].Cells["ModelNo"].Value = "";
                    grdPOPIDetail.Rows[kount - 1].Cells["ModelName"].Value = "";
                    grdPOPIDetail.Columns["ServiceItem"].Visible = false;
                    setDetailColumnForProduct();
                }
                else if (docID == "POSERVICEINWARD" || docID == "PAFSERVICEINWARD")
                {
                    //DataGridViewComboBoxCell ComboColumn1 = (DataGridViewComboBoxCell)(grdPOPIDetail.Rows[kount - 1].Cells["ServiceItem"]);
                    //CatalogueValueDB.fillCatalogValueGridViewComboNew(ComboColumn1, "ServiceLookup");
                    grdPOPIDetail.Rows[kount - 1].Cells["gOfficeID"].Value = "";
                    grdPOPIDetail.Rows[kount - 1].Cells["ProductItem"].Value = "";
                    grdPOPIDetail.Rows[kount - 1].Cells["ServiceItem"].Value = "";
                    grdPOPIDetail.Rows[kount - 1].Cells["ModelNo"].Value = "NA";
                    grdPOPIDetail.Rows[kount - 1].Cells["ModelName"].Value = "NA";
                    //ComboColumn1.DropDownWidth = 300;
                    setDetailColumnForService();
                }
                else
                {
                    //DataGridViewComboBoxCell ComboColumn1 = (DataGridViewComboBoxCell)(grdPOPIDetail.Rows[kount - 1].Cells["ServiceItem"]);
                    //CatalogueValueDB.fillCatalogValueGridViewComboNew(ComboColumn1, "ServiceLookup");
                    grdPOPIDetail.Rows[kount - 1].Cells["ProductItem"].Value = "";
                    grdPOPIDetail.Rows[kount - 1].Cells["gOfficeID"].Value = "";
                    grdPOPIDetail.Rows[kount - 1].Cells["ServiceItem"].Value = "";
                    grdPOPIDetail.Rows[kount - 1].Cells["ModelNo"].Value = "NA";
                    grdPOPIDetail.Rows[kount - 1].Cells["ModelName"].Value = "NA";
                    //ComboColumn1.DropDownWidth = 300;
                    setDetailColumnForServiceRC();
                }
                grdPOPIDetail.Rows[kount - 1].Cells["Quantity"].Value = 0;
                grdPOPIDetail.Rows[kount - 1].Cells["Price"].Value = 0;
                grdPOPIDetail.Rows[kount - 1].Cells["Value"].Value = 0;
                grdPOPIDetail.Rows[kount - 1].Cells["Tax"].Value = 0;
                grdPOPIDetail.Rows[kount - 1].Cells["WarrantyDays"].Value = 0;

                grdPOPIDetail.Rows[kount - 1].Cells["TaxDetails"].Value = " ";
                grdPOPIDetail.Rows[kount - 1].Cells["CustomerItemDescription"].Value = " ";
                if (chkDuplicateRow.Checked && grdPOPIDetail.Rows.Count > 1)
                {
                    fillDuplicateRow();
                    chkDuplicateRow.Checked = false;
                }

                if (AddRowClick)
                {
                    grdPOPIDetail.FirstDisplayedScrollingRowIndex = grdPOPIDetail.RowCount - 1;
                    grdPOPIDetail.CurrentCell = grdPOPIDetail.Rows[kount - 1].Cells[0];
                }
                grdPOPIDetail.Columns[0].Frozen = false;
                grdPOPIDetail.FirstDisplayedScrollingColumnIndex = 0;
                grdPOPIDetail.Columns["selDesc"].Frozen = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("AddPOPIDetailRow() : Error");
            }

            return status;
        }
        private void fillDuplicateRow()
        {
            try
            {
                int kount = grdPOPIDetail.RowCount;
                if (docID.Equals("POPRODUCTINWARD") || docID.Equals("PAFPRODUCTINWARD"))
                {
                    grdPOPIDetail.Rows[kount - 1].Cells["ProductItem"].Value = grdPOPIDetail.Rows[kount - 2].Cells["ProductItem"].Value;
                    grdPOPIDetail.Rows[kount - 1].Cells["ModelName"].Value = grdPOPIDetail.Rows[kount - 2].Cells["ModelName"].Value;
                    grdPOPIDetail.Rows[kount - 1].Cells["ModelNo"].Value = grdPOPIDetail.Rows[kount - 2].Cells["ModelNo"].Value;
                }
                else
                {
                    grdPOPIDetail.Rows[kount - 1].Cells["ServiceItem"].Value = grdPOPIDetail.Rows[kount - 2].Cells["ServiceItem"].Value;
                }
                
                grdPOPIDetail.Rows[kount - 1].Cells["Quantity"].Value = grdPOPIDetail.Rows[kount - 2].Cells["Quantity"].Value;
                grdPOPIDetail.Rows[kount - 1].Cells["gTaxCode"].Value = grdPOPIDetail.Rows[kount - 2].Cells["gTaxCode"].Value;
                grdPOPIDetail.Rows[kount - 1].Cells["CustomerItemDescription"].Value = grdPOPIDetail.Rows[kount - 2].Cells["CustomerItemDescription"].Value;
                grdPOPIDetail.Rows[kount - 1].Cells["Price"].Value = grdPOPIDetail.Rows[kount - 2].Cells["Price"].Value;
                grdPOPIDetail.Rows[kount - 1].Cells["Value"].Value = grdPOPIDetail.Rows[kount - 2].Cells["Value"].Value;
                grdPOPIDetail.Rows[kount - 1].Cells["Tax"].Value = grdPOPIDetail.Rows[kount - 2].Cells["Tax"].Value;
                grdPOPIDetail.Rows[kount - 1].Cells["WarrantyDays"].Value = grdPOPIDetail.Rows[kount - 2].Cells["WarrantyDays"].Value;
                grdPOPIDetail.Rows[kount - 1].Cells["TaxDetails"].Value = grdPOPIDetail.Rows[kount - 2].Cells["TaxDetails"].Value;
            }
            catch(Exception ex)
            {

            }
        }
        private void setDetailColumnForProduct()
        {
            grdPOPIDetail.Columns["ServiceItem"].Visible = false;
            grdPOPIDetail.Columns["ProductItem"].Visible = true;
            grdPOPIDetail.Columns["sel"].Visible = true;
            grdPOPIDetail.Columns["selLocation"].Visible = false;
            grdPOPIDetail.Columns["ModelNo"].Visible = true;
            grdPOPIDetail.Columns["ModelName"].Visible = true;
            grdPOPIDetail.Columns["gOfficeID"].Visible = false;
            grdPOPIDetail.Columns["Quantity"].Visible = true;
            grdPOPIDetail.Columns["Value"].Visible = true;
            grdPOPIDetail.Columns["WarrantyDays"].Visible = true;
        }
        private void setDetailColumnForService()
        {
            grdPOPIDetail.Columns["ServiceItem"].Visible = true;
            grdPOPIDetail.Columns["ProductItem"].Visible = false;
            grdPOPIDetail.Columns["sel"].Visible = true;
            grdPOPIDetail.Columns["selLocation"].Visible = true;
            grdPOPIDetail.Columns["ModelNo"].Visible = false;
            grdPOPIDetail.Columns["ModelName"].Visible = false;
            grdPOPIDetail.Columns["gOfficeID"].Visible = true;
            grdPOPIDetail.Columns["Quantity"].Visible = true;
            grdPOPIDetail.Columns["Value"].Visible = true;
            grdPOPIDetail.Columns["WarrantyDays"].Visible = true;
        }
        private void setDetailColumnForServiceRC()
        {
            grdPOPIDetail.Columns["ServiceItem"].Visible = true;
            grdPOPIDetail.Columns["ProductItem"].Visible = false;
            grdPOPIDetail.Columns["sel"].Visible = true;
            grdPOPIDetail.Columns["selLocation"].Visible = true;
            grdPOPIDetail.Columns["ModelNo"].Visible = false;
            grdPOPIDetail.Columns["ModelName"].Visible = false;
            grdPOPIDetail.Columns["gOfficeID"].Visible = true;
            grdPOPIDetail.Columns["Quantity"].Visible = false;
            grdPOPIDetail.Columns["Value"].Visible = false;
            grdPOPIDetail.Columns["WarrantyDays"].Visible = false;
        }
        private Boolean verifyAndReworkPOPIDetailGridRows()
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

                if (grdPOPIDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in PO Product Inward details");
                    txtProductValue.Text = productvalue.ToString();
                    txtProductValueINR.Text = (Convert.ToDouble(txtProductValue.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    txtTaxAmount.Text = taxvalue.ToString(); //fill this later
                    txtTaxAmountINR.Text = (Convert.ToDouble(txtTaxAmount.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    txtPOValue.Text = (productvalue + taxvalue).ToString();
                    txtPOValueINR.Text = (Convert.ToDouble(txtPOValue.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    btnTaxAmount.Text = txtTaxAmount.Text;
                    btnProductValue.Text = txtProductValue.Text;
                    return false;
                }
                ////clear tax details table
                //if (!validateItems())
                //{
                //    return false;
                //}
                TaxDetailsTable.Rows.Clear();
                for (int i = 0; i < grdPOPIDetail.Rows.Count; i++)
                {
                    //int count = from grdPOPIDetail.Rows 
                    grdPOPIDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if (!isViewMode)
                    {
                        if (grdPOPIDetail.Rows[i].Cells["gTaxCode"].Value == null)
                        {
                            MessageBox.Show("Fill Tax Code values in row " + (i + 1));
                            return false;
                        }
                        if (docID == "POPRODUCTINWARD" || docID == "POSERVICEINWARD" || docID == "PAFPRODUCTINWARD" || docID == "PAFSERVICEINWARD")
                        {
                            if (((grdPOPIDetail.Rows[i].Cells["ProductItem"].Value == null || grdPOPIDetail.Rows[i].Cells["ProductItem"].Value.ToString().Trim().Length == 0) && (chkDocID == "POPRODUCTINWARD" || docID == "PAFPRODUCTINWARD")) ||
                            ((grdPOPIDetail.Rows[i].Cells["ServiceItem"].Value == null || grdPOPIDetail.Rows[i].Cells["ServiceItem"].Value.ToString().Trim().Length == 0) && (chkDocID == "POSERVICEINWARD" || docID == "PAFSERVICEINWARD")) ||
                            (grdPOPIDetail.Rows[i].Cells["CustomerItemDescription"].Value == null) ||
                            (grdPOPIDetail.Rows[i].Cells["ModelNo"].Value == null) ||
                            (grdPOPIDetail.Rows[i].Cells["ModelName"].Value == null) ||
                            (grdPOPIDetail.Rows[i].Cells["CustomerItemDescription"].Value.ToString().Trim().Length == 0) ||
                            (grdPOPIDetail.Rows[i].Cells["Quantity"].Value == null) ||
                            (grdPOPIDetail.Rows[i].Cells["Price"].Value == null) ||
                            (grdPOPIDetail.Rows[i].Cells["Quantity"].Value.ToString().Trim().Length == 0) ||
                            (grdPOPIDetail.Rows[i].Cells["Price"].Value.ToString().Trim().Length == 0) ||
                            (Convert.ToDouble(grdPOPIDetail.Rows[i].Cells["Quantity"].Value) == 0) ||
                            (Convert.ToDouble(grdPOPIDetail.Rows[i].Cells["Price"].Value) == 0))
                            {
                                MessageBox.Show("Fill values in row " + (i + 1));
                                return false;
                            }
                            //////if (docID == "POSERVICEINWARD" || docID == "PAFSERVICEINWARD")
                            //////{
                            //////    if (grdPOPIDetail.Rows[i].Cells["gOfficeID"].Value == null ||
                            //////        grdPOPIDetail.Rows[i].Cells["gOfficeID"].Value.ToString().Length == 0)
                            //////    {
                            //////        MessageBox.Show("Fill OfficeID in row " + (i + 1));
                            //////        return false;
                            //////    }
                            //////}
                        }
                        else
                        {
                            if ((grdPOPIDetail.Rows[i].Cells["ServiceItem"].Value == null && chkDocID == "POSERVICERCINWARD") ||
                           (grdPOPIDetail.Rows[i].Cells["CustomerItemDescription"].Value == null) ||
                           (grdPOPIDetail.Rows[i].Cells["ModelNo"].Value == null) ||
                           (grdPOPIDetail.Rows[i].Cells["ModelName"].Value == null) ||
                           (grdPOPIDetail.Rows[i].Cells["CustomerItemDescription"].Value.ToString().Trim().Length == 0) ||
                           (grdPOPIDetail.Rows[i].Cells["Price"].Value == null) ||
                           (grdPOPIDetail.Rows[i].Cells["Price"].Value.ToString().Trim().Length == 0) ||
                           grdPOPIDetail.Rows[i].Cells["gOfficeID"].Value == null ||
                           grdPOPIDetail.Rows[i].Cells["gOfficeID"].Value.ToString().Length == 0 ||
                           (Convert.ToDouble(grdPOPIDetail.Rows[i].Cells["Price"].Value) == 0))
                            {
                                MessageBox.Show("Fill values in row " + (i + 1));
                                return false;
                            }
                        }
                    }
                    if (docID == "POSERVICERCINWARD")
                    {
                        quantity = 1;
                    }
                    else
                    {
                        quantity = Convert.ToDouble(grdPOPIDetail.Rows[i].Cells["Quantity"].Value);
                    }
                    price = Convert.ToDouble(grdPOPIDetail.Rows[i].Cells["Price"].Value);
                    if (price != 0 && quantity != 0)
                    {
                        cost = Math.Round(quantity * price, 2);
                    }

                    try
                    {
                        strtaxCode = grdPOPIDetail.Rows[i].Cells["gTaxCode"].Value.ToString().Trim();
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
                    grdPOPIDetail.Rows[i].Cells["Value"].Value = Convert.ToDouble(cost.ToString());
                    grdPOPIDetail.Rows[i].Cells["Tax"].Value = ttax2.ToString();
                    grdPOPIDetail.Rows[i].Cells["TaxDetails"].Value = strTax;
                    productvalue = productvalue + cost;
                    taxvalue = taxvalue + ttax2;

                    //- rewok tax value
                }
                try
                {
                    double dval = Convert.ToDouble(txtExchangeRate.Text);
                }
                catch (Exception ex)
                {
                    txtExchangeRate.Text = "1";
                }
                txtProductValue.Text = productvalue.ToString();
                txtProductValueINR.Text = (Convert.ToDouble(txtProductValue.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                txtTaxAmount.Text = taxvalue.ToString(); //fill this later
                txtTaxAmountINR.Text = (Convert.ToDouble(txtTaxAmount.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                txtPOValue.Text = (productvalue + taxvalue).ToString();
                txtPOValueINR.Text = (Convert.ToDouble(txtPOValue.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();

                btnProductValue.Text = txtProductValue.Text;
                btnTaxAmount.Text = txtTaxAmount.Text;


            }
            catch (Exception ex)
            {
                return false;
            }
            return status;
        }

        //check for item duplication in details
        //private Boolean validateItems()
        //{
        //    Boolean status = true;
        //    try
        //    {
        //        if (docID == "POPRODUCTINWARD" || docID == "PAFPRODUCTINWARD")
        //        {

        //            for (int i = 0; i < grdPOPIDetail.Rows.Count - 1; i++)
        //            {
        //                for (int j = i + 1; j < grdPOPIDetail.Rows.Count; j++)
        //                {

        //                    if (grdPOPIDetail.Rows[i].Cells["ProductItem"].Value.ToString() == grdPOPIDetail.Rows[j].Cells["ProductItem"].Value.ToString()
        //                        && grdPOPIDetail.Rows[i].Cells["ModelNo"].Value.ToString() == grdPOPIDetail.Rows[j].Cells["ModelNo"].Value.ToString())
        //                    {
        //                        //duplicate item code
        //                        MessageBox.Show("Duplicate Item not Allowed.Item code duplicated in PO details..(" +
        //                            grdPOPIDetail.Rows[i].Cells["ProductItem"].Value.ToString() + ")");
        //                        status = false;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        status = false;
        //    }
        //    return status;
        //}

        private List<popidetail> getPOPIDetails(popiheader popih)
        {
            List<popidetail> POPIDetails = new List<popidetail>();
            try
            {
                popidetail popid = new popidetail();
                for (int i = 0; i < grdPOPIDetail.Rows.Count; i++)
                {
                    popid = new popidetail();
                    popid.DocumentID = popih.DocumentID;
                    popid.TemporaryNo = popih.TemporaryNo;
                    popid.TemporaryDate = popih.TemporaryDate;
                    if (popid.DocumentID.Equals("POPRODUCTINWARD") || popid.DocumentID.Equals("PAFPRODUCTINWARD"))
                    {
                        popid.StockItemID = grdPOPIDetail.Rows[i].Cells["ProductItem"].Value.ToString().Trim().Substring(0, grdPOPIDetail.Rows[i].Cells["ProductItem"].Value.ToString().Trim().IndexOf('-'));
                        popid.ModelNo = grdPOPIDetail.Rows[i].Cells["ModelNo"].Value.ToString();
                        popid.Location = "";
                    }
                    else
                    {
                        popid.StockItemID = grdPOPIDetail.Rows[i].Cells["ServiceItem"].Value.ToString().Trim().Substring(0, grdPOPIDetail.Rows[i].Cells["ServiceItem"].Value.ToString().Trim().IndexOf('-'));
                        popid.Location = grdPOPIDetail.Rows[i].Cells["gOfficeID"].Value.ToString();
                    }
                    popid.CustomerItemDescription = grdPOPIDetail.Rows[i].Cells["CustomerItemDescription"].Value.ToString().Trim();
                    popid.WorkDescription = " ";

                    popid.Quantity = Convert.ToDouble(grdPOPIDetail.Rows[i].Cells["Quantity"].Value);
                    popid.Price = Convert.ToDouble(grdPOPIDetail.Rows[i].Cells["Price"].Value);
                    popid.TaxCode = grdPOPIDetail.Rows[i].Cells["gTaxCode"].Value.ToString();
                    popid.Tax = Convert.ToDouble(grdPOPIDetail.Rows[i].Cells["Tax"].Value);
                    popid.WarrantyDays = Convert.ToInt32(grdPOPIDetail.Rows[i].Cells["WarrantyDays"].Value);

                    popid.TaxDetails = grdPOPIDetail.Rows[i].Cells["TaxDetails"].Value.ToString();

                    POPIDetails.Add(popid);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("getPOPIDetails() : Error getting POPIDetails");
                POPIDetails = null;
            }
            return POPIDetails;
        }

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            txtSearch.Text = "";
            ListFilteredStockOBHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            txtSearch.Text = "";
            ListFilteredStockOBHeader(listOption);
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
            txtSearch.Text = "";
            ListFilteredStockOBHeader(listOption);
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {

                POPIHeaderDB popidb = new POPIHeaderDB();
                popiheader popih = new popiheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    if (!verifyAndReworkPOPIDetailGridRows())
                    {
                        return;
                    }
                    popih.DocumentID = docID;
                    popih.TrackingDate = dtTrackDate.Value;
                    ////////popih.CustomerID = cmbCustomer.SelectedItem.ToString().Trim().Substring(0, cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-'));
                    popih.CustomerID = txtCustomerID.Text.Trim();
                    popih.CustomerPONO = txtCustomerPONo.Text;
                    popih.CustomerPODate = dtCustomerPODate.Value;
                    popih.DeliveryDate = dtdeliveryDate.Value;
                    popih.ValidityDate = dtValidityDate.Value;
                    popih.PaymentTerms = txtPaymentTerms.Text;
                    //return;
                    popih.PaymentMode = ((Structures.ComboBoxItem)cmbPaymentMode.SelectedItem).HiddenValue;
                    //popih.PaymentMode = cmbPaymentMode.SelectedItem.ToString().Trim().Substring(0, cmbPaymentMode.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
                    popih.FreightTerms = ((Structures.ComboBoxItem)cmbFreightTerms.SelectedItem).HiddenValue;
                    //popih.FreightTerms = cmbFreightTerms.SelectedItem.ToString().Trim().Substring(0, cmbFreightTerms.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
                    popih.FreightCharge = Convert.ToDouble(txtFreightCharge.Text);
                    //////////popih.CurrencyID = cmbCurrency.SelectedItem.ToString().Trim().Substring(0, cmbCurrency.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
                    popih.CurrencyID = ((Structures.ComboBoxItem)cmbCurrency.SelectedItem).HiddenValue;
                    popih.ExchangeRate = Convert.ToDecimal(txtExchangeRate.Text);
                    //popih.TaxCode = cmbTaxCode.SelectedItem.ToString().Trim();
                    try
                    {
                        popih.ProjectID = cmbProjectID.SelectedItem.ToString().Trim();
                    }
                    catch (Exception)
                    {
                        popih.ProjectID = "";
                    }
                    popih.OfficeID = ((Structures.ComboBoxItem)cmbOfficeID.SelectedItem).HiddenValue;
                    popih.BillingAddress = txtBillingAddress.Text;
                    popih.DeliveryAddress = txtDeliveryAddress.Text;
                    popih.ProductValue = Convert.ToDouble(txtProductValue.Text);
                    popih.TaxAmount = Convert.ToDouble(txtTaxAmount.Text);
                    popih.POValue = Convert.ToDouble(txtPOValue.Text);
                    popih.ProductValueINR = Convert.ToDouble(txtProductValueINR.Text);
                    popih.TaxAmountINR = Convert.ToDouble(txtTaxAmountINR.Text);
                    popih.POValueINR = Convert.ToDouble(txtPOValueINR.Text);
                    popih.Remarks = txtRemarks.Text;
                    popih.ReferenceNo = txtReferenceNo.Text;
                    popih.Comments = docCmtrDB.DGVtoString(dgvComments).Replace("'","''");
                    popih.ForwarderList = prevpopi.ForwarderList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }

                if (!popidb.validatePOPIHeader(popih))
                {
                    MessageBox.Show("Validation failed");
                    return;
                }

                if (btnText.Equals("Save"))
                {
                    //popih.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    popih.DocumentStatus = 1; //created
                    popih.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    popih.TemporaryNo = Convert.ToInt32(txtTemporarryNo.Text);
                    popih.TemporaryDate = prevpopi.TemporaryDate;
                    popih.DocumentStatus = prevpopi.DocumentStatus;
                }
                //Replacing single quotes
                popih = verifyHeaderInputString(popih);
                verifyDetailInputString();
                if (popidb.validatePOPIHeader(popih))
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
                            popih.CommentStatus = docCmtrDB.createCommentStatusString(prevpopi.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            popih.CommentStatus = prevpopi.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            popih.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            popih.CommentStatus = prevpopi.CommentStatus;
                        }
                    }
                    //----------
                    int tmpStatus = 1;
                    //if (chkCommentStatus.Checked)
                    //{
                    //    tmpStatus = 1;
                    //}
                    if (txtComments.Text.Trim().Length > 0)
                    {
                        popih.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus).Replace("'", "''");
                    }

                    List<popidetail> POPIDetails = getPOPIDetails(popih);
                    if (btnText.Equals("Update"))
                    {
                        if (POPIHeaderDB.checkCustomerPONoAndDatePresence(popih, 2))
                        {
                            DialogResult dialog = MessageBox.Show("CustomerPO No is Duplicated,\nDo you wanat to Continue ?", "Yes", MessageBoxButtons.YesNo);
                            if (dialog == DialogResult.No)
                            {
                                return;
                            }
                        }
                        if (popidb.updatePOPIHeaderAndDetail(popih, prevpopi, POPIDetails))
                        {
                            MessageBox.Show("PO Product Inward Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredStockOBHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update PO Product Inward Header");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (POPIHeaderDB.checkCustomerPONoAndDatePresence(popih, 1))
                        {
                            DialogResult dialog = MessageBox.Show("CustomerPO No is Duplicated,\nDo you want to Continue ?", "Alert", MessageBoxButtons.YesNo);
                            if (dialog == DialogResult.No)
                            {
                                return;
                            }
                        }
                        if (popidb.InsertPOPIHeaderAndDetail(popih, POPIDetails))
                        {
                            MessageBox.Show("PO Product Inward Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredStockOBHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert Stock PO Product Inward Header");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Stock PO Product Inward Validation failed");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("btnSave_Click_1() : Error");
                return;
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save

            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }

        private void btnAddLine_Click_1(object sender, EventArgs e)
        {
            AddRowClick = true;
            AddPOPIDetailRow();
        }

        private void grdPOPIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdPOPIDetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdPOPIDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkPOPIDetailGridRows();
                    }
                    if (columnName.Equals("TaxView"))
                    {
                        //show tax details
                        DialogResult dialog = MessageBox.Show(grdPOPIDetail.Rows[e.RowIndex].Cells["TaxDetails"].Value.ToString(),
                            "Line No : " + (e.RowIndex + 1), MessageBoxButtons.OK);
                    }
                    if (columnName.Equals("sel"))
                    {
                        int opt = 0;
                        if (docID == "POPRODUCTINWARD" || docID == "PAFPRODUCTINWARD")
                        {
                            //opt = 1; //For Product
                            showStockDataGridView();
                        }
                        else if (docID == "POSERVICEINWARD" || docID == "PAFSERVICEINWARD")
                        {
                            opt = 2; //For service
                            showStockItemTreeView(opt);
                        }
                        else
                        {
                            opt = 2; //For RateContract(service)
                            showStockItemTreeView(opt);
                        }
                        
                    }
                    if (columnName.Equals("selDesc"))
                    {
                        descClickRowIndex = e.RowIndex;
                        string strTest = grdPOPIDetail.Rows[e.RowIndex].Cells["CustomerItemDescription"].Value.ToString().Trim();
                        showPopUpForDescription(strTest);
                    }
                    if (columnName.Equals("selLocation"))
                    {
                        descClickRowIndex = e.RowIndex;
                        string strTest = grdPOPIDetail.Rows[e.RowIndex].Cells["gOfficeID"].Value.ToString().Trim();
                        showPopUpForLOcation(strTest);
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

        //showing gridview for stockitem
        private void showStockDataGridView()
        {
            try
            {
                if ((grdPOPIDetail.CurrentRow.Cells["ProductItem"].Value.ToString().Length != 0))
                {
                    DialogResult dialog = MessageBox.Show("Selected PO detail will removed?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        grdPOPIDetail.CurrentRow.Cells["ProductItem"].Value = "";
                        grdPOPIDetail.CurrentRow.Cells["ModelNo"].Value = "";
                        grdPOPIDetail.CurrentRow.Cells["ModelName"].Value = "";
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

                frmPopup.Size = new Size(900, 370);

                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(550, 5);
                lblSearch.AutoSize = true;
                lblSearch.Text = "Search by Name";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearchStock = new TextBox();
                txtSearchStock.Size = new Size(200, 18);
                txtSearchStock.Location = new System.Drawing.Point(680, 3);
                txtSearchStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearchStock.ForeColor = Color.Black;
                txtSearchStock.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInEmpGridList);
                txtSearchStock.TabIndex = 0;
                txtSearchStock.Focus();
                frmPopup.Controls.Add(txtSearchStock);

                StockItemDB stkDB = new StockItemDB();
                grdStock = stkDB.getStockItemlistGrid();

                grdStock.Bounds = new Rectangle(new Point(0, 27), new Size(900, 300));
                frmPopup.Controls.Add(grdStock);
                grdStock.Columns["StockItemID"].Width = 150;
                grdStock.Columns["Name"].Width = 350;
                grdStock.Columns["Group1CodeDesc"].Width = 110;
                grdStock.Columns["Group2CodeDesc"].Width = 110;
                grdStock.Columns["Group3CodeDesc"].Width = 110;
                foreach (DataGridViewColumn cells in grdStock.Columns)
                {
                    if (cells.CellType != typeof(DataGridViewCheckBoxCell))
                        cells.ReadOnly = true;
                }
                //grdStock.ReadOnly = true;
                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grdStkOK_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 335);
                lvCancel.Click += new System.EventHandler(this.grdStkCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdStkOK_Click1(object sender, EventArgs e)
        {
            try
            {
                string iolist = "";
                var checkedRows = from DataGridViewRow r in grdStock.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("Select one Product");
                    return;
                }
                foreach (var row in checkedRows)
                {
                    iolist = row.Cells["StockItemID"].Value.ToString() + "-" + row.Cells["Name"].Value.ToString();
                }
                grdPOPIDetail.CurrentRow.Cells["ProductItem"].Value = iolist;
                ///showModelListView(iolist);
                frmPopup.Close();
                frmPopup.Dispose();
                showModelListView(iolist);
            }
            catch (Exception)
            {
            }
        }
        private void grdStkCancel_Click1(object sender, EventArgs e)
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
        private void txtSearch_TextChangedInEmpGridList(object sender, EventArgs e)
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
            filterGridData1();
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }

        private void filterGridData1()
        {
            try
            {
                grdStock.CurrentCell = null;
                foreach (DataGridViewRow row in grdStock.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearchStock.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdStock.Rows)
                    {
                        if (!row.Cells["Name"].Value.ToString().Trim().ToLower().Contains(txtSearchStock.Text.Trim().ToLower()))
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





        //-----------
        private void showPopUpForLOcation(string str)
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
            head.Text = "Fill Location Below";
            frmPopup.Controls.Add(head);

            txtDesc = new RichTextBox();
            txtDesc.Text = str;
            txtDesc.Bounds = new Rectangle(new Point(3, 25), new Size(345, 111));
            frmPopup.Controls.Add(txtDesc);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.BackColor = Color.Tan;
            lvOK.Location = new System.Drawing.Point(210, 142);
            lvOK.Size = new System.Drawing.Size(64, 23);
            lvOK.Cursor = Cursors.Hand;
            lvOK.Click += new System.EventHandler(this.lvOK_ClickLOc);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new System.Drawing.Point(273, 142);
            lvCancel.Size = new System.Drawing.Size(73, 23);
            lvCancel.Cursor = Cursors.Hand;
            lvCancel.Click += new System.EventHandler(this.lvCancel_ClickLoc);
            frmPopup.Controls.Add(lvCancel);

            frmPopup.ShowDialog();
        }
        private void lvOK_ClickLOc(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtDesc.Text.Trim()))
                {
                    MessageBox.Show("Location is empty");
                    return;
                }
                grdPOPIDetail.Rows[descClickRowIndex].Cells["gOfficeID"].Value = txtDesc.Text.Trim();
                grdPOPIDetail.FirstDisplayedScrollingRowIndex = grdPOPIDetail.Rows[descClickRowIndex].Index;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        private void lvCancel_ClickLoc(object sender, EventArgs e)
        {
            try
            {
                txtDesc.Text = "";
                descClickRowIndex = -1;
                frmPopup.Close();
                frmPopup.Dispose();

            }
            catch (Exception)
            {
            }
        }
        private void showPopUpForDescription(string str)
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
            head.Text = "Fill Description Below";
            frmPopup.Controls.Add(head);

            txtDesc = new RichTextBox();
            txtDesc.Text = str;
            txtDesc.Bounds = new Rectangle(new Point(3, 25), new Size(345, 111));
            frmPopup.Controls.Add(txtDesc);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.BackColor = Color.Tan;
            lvOK.Location = new System.Drawing.Point(210, 142);
            lvOK.Size = new System.Drawing.Size(64, 23);
            lvOK.Cursor = Cursors.Hand;
            lvOK.Click += new System.EventHandler(this.lvOK_Click5);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new System.Drawing.Point(273, 142);
            lvCancel.Size = new System.Drawing.Size(73, 23);
            lvCancel.Cursor = Cursors.Hand;
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click5);
            frmPopup.Controls.Add(lvCancel);

            frmPopup.ShowDialog();
        }
        private void lvOK_Click5(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtDesc.Text.Trim()))
                {
                    MessageBox.Show("Description is empty");
                    return;
                }
                grdPOPIDetail.Rows[descClickRowIndex].Cells["CustomerItemDescription"].Value = txtDesc.Text.Trim();
                grdPOPIDetail.FirstDisplayedScrollingRowIndex = grdPOPIDetail.Rows[descClickRowIndex].Index;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        private void lvCancel_Click5(object sender, EventArgs e)
        {
            try
            {
                txtDesc.Text = "";
                descClickRowIndex = -1;
                frmPopup.Close();
                frmPopup.Dispose();

            }
            catch (Exception)
            {
            }
        }
        private Boolean checkAvailabilityOfitem()
        {
            Boolean status = true;
            if (grdPOPIDetail.CurrentRow.Cells["ServiceItem"].Value.ToString().Length != 0)
            {
                status = false;
            }
            return status;
        }
        private void showStockItemTreeView(int opt)
        {
            removeControlsFromForwarderPanelTV();
            if (!checkAvailabilityOfitem())
            {
                DialogResult dialog = MessageBox.Show("Selected PO detail will removed?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdPOPIDetail.CurrentRow.Cells["ProductItem"].Value = "";
                    grdPOPIDetail.CurrentRow.Cells["ServiceItem"].Value = "";
                    grdPOPIDetail.CurrentRow.Cells["ModelNo"].Value = "";
                    grdPOPIDetail.CurrentRow.Cells["ModelName"].Value = "";
                }
                else
                    return;
            }
            tv = new TreeView();
            tv.CheckBoxes = true;
            tv.Nodes.Clear();
            tv.CheckBoxes = true;
            pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
            pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Location = new Point(50, 8);
            lbl.Size = new Size(35, 13);

            lbl.Font = new Font("Serif", 10, FontStyle.Bold);
            lbl.ForeColor = Color.Green;

            if (opt == 2)
            {
            //    lbl.Text = "Tree View For Product";
            //    tv = StockItemDB.getStockItemTreeView();
            //}
            //else
            //{
                lbl.Text = "Tree View For Service";
                tv = ServiceItemsDB.getServiceItemTreeView();
            }
            pnlForwarder.Controls.Add(lbl);
            tv.Bounds = new Rectangle(new Point(50, 30), new Size(600, 220));
            pnlForwarder.Controls.Remove(tv);
            pnlForwarder.Controls.Add(tv);
            //tv.cl
            tv.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterCheck);
            Button lvForwrdOK = new Button();
            lvForwrdOK.Text = "OK";
            lvForwrdOK.BackColor = Color.LightGreen;
            lvForwrdOK.Size = new Size(100, 30);
            lvForwrdOK.Location = new Point(50, 260);
            lvForwrdOK.Click += new System.EventHandler(this.tvOK_Click);
            pnlForwarder.Controls.Add(lvForwrdOK);

            Button lvForwardCancel = new Button();
            lvForwardCancel.Text = "Cancel";
            lvForwardCancel.BackColor = Color.LightGreen;
            lvForwardCancel.Size = new Size(100, 30);
            lvForwardCancel.Location = new Point(150, 260);
            lvForwardCancel.Click += new System.EventHandler(this.tvCancel_Click);
            pnlForwarder.Controls.Add(lvForwardCancel);
            ////lvForwardCancel.Visible = false;
            //tv.CheckBoxes = true;
            pnlForwarder.Visible = true;
            pnlAddEdit.Controls.Add(pnlForwarder);
            pnlAddEdit.BringToFront();
            pnlForwarder.BringToFront();
            pnlForwarder.Focus();
        }
        private void tvOK_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> ItemList = GetCheckedNodes(tv.Nodes);
                if (ItemList.Count > 1 || ItemList.Count == 0)
                {
                    MessageBox.Show("select only one item");
                    return;
                }
                foreach (string s in ItemList)
                {
                    if (docID == "POPRODUCTINWARD" || docID == "PAFPRODUCTINWARD")
                    {
                        grdPOPIDetail.CurrentRow.Cells["ProductItem"].Value = s;
                        tv.CheckBoxes = true;
                        pnlForwarder.Controls.Remove(tv);
                        pnlForwarder.Visible = false;
                        showModelListView(s);
                    }
                    else
                    {
                        grdPOPIDetail.CurrentRow.Cells["ServiceItem"].Value = s;
                        tv.CheckBoxes = true;
                        pnlForwarder.Controls.Remove(tv);
                        pnlForwarder.Visible = false;
                    }

                }

            }
            catch (Exception)
            {
            }
        }
        public List<string> GetCheckedNodes(TreeNodeCollection nodes)
        {
            List<string> nodeList = new List<string>();
            try
            {

                if (nodes == null)
                {
                    return nodeList;
                }

                foreach (TreeNode childNode in nodes)
                {
                    if (childNode.Checked)
                    {
                        nodeList.Add(childNode.Text);
                    }
                    nodeList.AddRange(GetCheckedNodes(childNode.Nodes));
                }

            }
            catch (Exception ex)
            {
            }
            return nodeList;
        }
        private void tvCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //lvApprover.CheckBoxes = false;
                //lvApprover.CheckBoxes = true;
                tv.CheckBoxes = true;
                pnlForwarder.Controls.Remove(tv);
                pnlForwarder.Visible = false;
            }
            catch (Exception)
            {
            }
        }
        private void tv_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Checked == true)
            {
                if (e.Node.Nodes.Count != 0)
                {
                    MessageBox.Show("you are not allowed to select group");
                    e.Node.Checked = false;
                }
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
                grdPOPIDetail.CurrentRow.Cells["ModelNo"].Value = "NA";
                grdPOPIDetail.CurrentRow.Cells["ModelName"].Value = "NA";
                pnlModel.Visible = false;
                return;
            }
            lv.Bounds = new Rectangle(new Point(0, 25), new Size(450, 250));
            //pnlModel.Controls.Remove(lv);
            frmPopup.Controls.Add(lv);
            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.BackColor = Color.Tan;
            lvOK.Location = new Point(40, 280);
            lvOK.Click += new System.EventHandler(this.lvOK_Click1);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new Point(130, 280);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click1);
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
        private void lvOK_Click1(object sender, EventArgs e)
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
                        grdPOPIDetail.CurrentRow.Cells["ModelNo"].Value = item.SubItems[1].Text;
                        grdPOPIDetail.CurrentRow.Cells["ModelName"].Value = item.SubItems[2].Text;
                        frmPopup.Close();
                        frmPopup.Dispose();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void lvCancel_Click1(object sender, EventArgs e)
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
            if (txtExchangeRate.Text.Length == 0)
            {
                MessageBox.Show("Fill Exchange Rate");
                return;
            }
            verifyAndReworkPOPIDetailGridRows();
            chkDuplicateRow.Checked = false;
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
                    columnName.Equals("View") || columnName == "CloseRequest" || columnName == "Close")
                {
                    clearData();
                    setButtonVisibility(columnName);
                    prevpopi = new popiheader();
                    AddRowClick = false;
                    prevpopi.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();
                    prevpopi.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    chkDocID = prevpopi.DocumentID;
                    prevpopi.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    prevpopi.ReferenceNo = grdList.Rows[e.RowIndex].Cells["ReferenceNo"].Value.ToString();
                    prevpopi.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    prevpopi.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    if (prevpopi.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commenting and viewing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    if (columnName.Equals("View"))
                    {
                        tabControl1.TabPages["tabPODetail"].Enabled = true;
                        tabControl1.TabPages["tabPOHeader"].Enabled = true;
                        isViewMode = true;
                    }
                    prevpopi.Comments = POPIHeaderDB.getUserComments(prevpopi.DocumentID, prevpopi.TemporaryNo, prevpopi.TemporaryDate);
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    setPODetailColumns(docID);

                    POPIHeaderDB popidb = new POPIHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];

                    prevpopi.TrackingNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTrackingNo"].Value.ToString());
                    prevpopi.TrackingDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTrackingDate"].Value.ToString());
                    prevpopi.CustomerID = grdList.Rows[e.RowIndex].Cells["gCustomerID"].Value.ToString();
                    prevpopi.CustomerName = grdList.Rows[e.RowIndex].Cells["gCustomerName"].Value.ToString();

                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Document Temp No:" + prevpopi.TemporaryNo + "\n" +
                            "Document Temp Date:" + prevpopi.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "Tracking No:" + prevpopi.TrackingNo + "\n" +
                            "Tracking Date:" + prevpopi.TrackingDate.ToString("dd-MM-yyyy") + "\n" +
                            "Customer:" + prevpopi.CustomerName;
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevpopi.TemporaryNo + "-" + prevpopi.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_1(null, null);
                        return;
                    }
                    //--------

                    prevpopi.CustomerPONO = grdList.Rows[e.RowIndex].Cells["gCustomerPONo"].Value.ToString();
                    prevpopi.CustomerPODate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCustomerPODate"].Value.ToString());
                    prevpopi.DeliveryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gDeliveryDate"].Value.ToString());
                    prevpopi.ValidityDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["ValidityDate"].Value.ToString());
                    prevpopi.PaymentTerms = grdList.Rows[e.RowIndex].Cells["gPaymentTerms"].Value.ToString();
                    prevpopi.PaymentMode = grdList.Rows[e.RowIndex].Cells["gPaymentMode"].Value.ToString();
                    prevpopi.FreightTerms = grdList.Rows[e.RowIndex].Cells["gFreightTerms"].Value.ToString();
                    prevpopi.FreightCharge = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gFreightCharge"].Value.ToString());
                    prevpopi.CurrencyID = grdList.Rows[e.RowIndex].Cells["gCurrencyID"].Value.ToString();
                    prevpopi.ExchangeRate = Convert.ToDecimal(grdList.Rows[e.RowIndex].Cells["ExchangeRate"].Value.ToString());
                    prevpopi.ProjectID = grdList.Rows[e.RowIndex].Cells["ProjectID"].Value.ToString();
                    prevpopi.BillingAddress = grdList.Rows[e.RowIndex].Cells["gBillingAddress"].Value.ToString();
                    prevpopi.DeliveryAddress = grdList.Rows[e.RowIndex].Cells["gDeliveryAddress"].Value.ToString();
                    prevpopi.ProductValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gProductValue"].Value.ToString());
                    prevpopi.TaxAmount = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gTaxAmount"].Value.ToString());
                    prevpopi.POValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gPOValue"].Value.ToString());
                    prevpopi.OfficeID = grdList.Rows[e.RowIndex].Cells["OfficeID"].Value.ToString();
                    prevpopi.ProductValueINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["ProductValueINR"].Value.ToString());
                    prevpopi.TaxAmountINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["TaxAmountINR"].Value.ToString());
                    prevpopi.POValueINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["POValueINR"].Value.ToString());
                    prevpopi.Remarks = grdList.Rows[e.RowIndex].Cells["gRemarks"].Value.ToString();
                    prevpopi.status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    prevpopi.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    prevpopi.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    prevpopi.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    prevpopi.ApproveUser = grdList.Rows[e.RowIndex].Cells["ApproveUser"].Value.ToString();
                    prevpopi.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    prevpopi.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevpopi.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    prevpopi.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    prevpopi.ClosingStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["ClosingStatus"].Value.ToString());
                    //--comments
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    prevpopi.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(prevpopi.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(prevpopi.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---
                    //--
                    string prevComments = prevpopi.Comments;
                    if (columnName == "CloseRequest")
                    {
                        if (prevpopi.ClosingStatus == 1)
                        {
                            MessageBox.Show("Previous closing request is pending.");
                            btnExit.Visible = true;
                            return;
                        }
                        showCLosingRequestPopUp();
                        btnExit.Visible = true;
                        //ListFilteredStockOBHeader(listOption);

                        return;
                    }
                    if (columnName == "Close")
                    {
                        if (prevpopi.ApproveUser != Login.userLoggedIn)
                        {
                            MessageBox.Show("Only approve user can close PO inward.");
                            btnExit.Visible = true;
                            return;
                        }
                        if (prevpopi.ClosingStatus == 0)
                        {
                            MessageBox.Show("No request for closing this PO inward");
                            btnExit.Visible = true;
                            return;
                        }
                        showpopUpForCLosing();
                        btnExit.Visible = true;
                        //ListFilteredStockOBHeader(listOption);
                        return;
                    }
                    //---



                    txtReferenceNo.Text = prevpopi.ReferenceNo.ToString();
                    txtTemporarryNo.Text = prevpopi.TemporaryNo.ToString();
                    dtTempDate.Value = prevpopi.TemporaryDate;
                    dtTempDate.Value = prevpopi.TemporaryDate;
                    //dtValidityDate.Value = prevpopi.ValidityDays.ToString();
                    txtTrackingNo.Text = prevpopi.TrackingNo.ToString();
                    try
                    {
                        dtTrackDate.Value = prevpopi.TrackingDate;
                    }
                    catch (Exception)
                    {
                        dtTrackDate.Value = DateTime.Parse("01-01-1900");
                    }

                    txtCustomerID.Text = prevpopi.CustomerID;
                    txtCustName.Text = prevpopi.CustomerName;
                    if (prevpopi.ProjectID.Length != 0)
                        cmbProjectID.SelectedIndex = cmbProjectID.FindString(prevpopi.ProjectID);
                    cmbOfficeID.SelectedIndex =
                      Structures.ComboFUnctions.getComboIndex(cmbOfficeID, prevpopi.OfficeID);
                    //cmbCustomer.Text = prevpopi.CustomerName;
                    txtCustomerPONo.Text = prevpopi.CustomerPONO;
                    try
                    {
                        dtCustomerPODate.Value = prevpopi.CustomerPODate;
                    }
                    catch (Exception)
                    {

                        dtTrackDate.Value = DateTime.Parse("01-01-1900");
                    }
                    try
                    {
                        dtdeliveryDate.Value = prevpopi.DeliveryDate;
                    }
                    catch (Exception)
                    {
                        dtTrackDate.Value = DateTime.Parse("01-01-1900");
                    }
                    try
                    {
                        dtValidityDate.Value = prevpopi.ValidityDate;
                    }
                    catch (Exception)
                    {
                        dtValidityDate.Value = DateTime.Parse("01-01-1900");
                    }
                    // cmbPaymentTerms.SelectedIndex = cmbPaymentTerms.FindString(prevpopi.PaymentTerms);
                    txtPaymentTerms.Text = prevpopi.PaymentTerms;
                    cmbPaymentMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbPaymentMode, prevpopi.PaymentMode);
                    //cmbPaymentMode.SelectedIndex = cmbPaymentMode.FindString(grdList.Rows[e.RowIndex].Cells["gPaymentMode"].Value.ToString());
                    cmbFreightTerms.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbFreightTerms, prevpopi.FreightTerms);
                    //cmbFreightTerms.SelectedIndex = cmbFreightTerms.FindString(prevpopi.FreightTerms);
                    //////////cmbCurrency.SelectedIndex = cmbCurrency.FindString(prevpopi.CurrencyID);
                    cmbCurrency.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCurrency, prevpopi.CurrencyID);
                    //cmbCurrency.Text = prevpopi.CurrencyID;
                    //cmbTaxCode.Text = prevpopi.TaxCode;
                    txtBillingAddress.Text = prevpopi.BillingAddress;
                    txtDeliveryAddress.Text = prevpopi.DeliveryAddress;
                    txtProductValue.Text = prevpopi.ProductValue.ToString();
                    txtTaxAmount.Text = prevpopi.TaxAmount.ToString();
                    txtPOValue.Text = prevpopi.POValue.ToString();
                    txtExchangeRate.Text = prevpopi.ExchangeRate.ToString();
                    txtProductValueINR.Text = prevpopi.ProductValueINR.ToString();
                    txtTaxAmountINR.Text = prevpopi.TaxAmountINR.ToString();
                    txtPOValueINR.Text = prevpopi.POValueINR.ToString();

                    txtRemarks.Text = prevpopi.Remarks.ToString();
                    txtFreightCharge.Text = prevpopi.FreightCharge.ToString();
                    List<popidetail> POPIDetail = POPIHeaderDB.getPOPIDetail(prevpopi);
                    grdPOPIDetail.Rows.Clear();
                    int i = 0;
                    foreach (popidetail popid in POPIDetail)
                    {
                        if (!AddPOPIDetailRow())
                        {
                            MessageBox.Show("Error found in PO details. Please correct before updating the details");
                        }
                        else
                        {
                            ////DataGridViewComboBoxCell ComboColumn1 = new DataGridViewComboBoxCell();
                            ////StockItemDB.fillStockItemGridViewCombo(ComboColumn1, "");

                            try
                            {
                                if (prevpopi.DocumentID.Equals("POPRODUCTINWARD") || prevpopi.DocumentID.Equals("PAFPRODUCTINWARD"))
                                {
                                    grdPOPIDetail.Rows[i].Cells["ProductItem"].Value = popid.StockItemID + "-" + popid.StockItemName;
                                    grdPOPIDetail.Rows[i].Cells["ModelName"].Value = popid.ModelName;
                                    grdPOPIDetail.Rows[i].Cells["ModelNo"].Value = popid.ModelNo;
                                    grdPOPIDetail.Rows[i].Cells["gOfficeID"].Value = "";
                                }
                                else
                                {
                                    grdPOPIDetail.Rows[i].Cells["ServiceItem"].Value = popid.StockItemID + "-" + popid.StockItemName;
                                    grdPOPIDetail.Rows[i].Cells["gOfficeID"].Value = popid.Location;
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            grdPOPIDetail.Rows[i].Cells["Quantity"].Value = popid.Quantity;
                            grdPOPIDetail.Rows[i].Cells["gTaxCode"].Value = popid.TaxCode;
                            grdPOPIDetail.Rows[i].Cells["CustomerItemDescription"].Value = popid.CustomerItemDescription;
                            ////grdPOPIDetail.Rows[i].Cells["CustomerItemDescription"].Value = popid.CustomerItemDescription;
                            grdPOPIDetail.Rows[i].Cells["Price"].Value = popid.Price;
                            grdPOPIDetail.Rows[i].Cells["Value"].Value = popid.Quantity * popid.Price;
                            grdPOPIDetail.Rows[i].Cells["Tax"].Value = popid.Tax;
                            grdPOPIDetail.Rows[i].Cells["WarrantyDays"].Value = popid.WarrantyDays;
                            grdPOPIDetail.Rows[i].Cells["TaxDetails"].Value = popid.TaxDetails;
                            i++;
                            productvalue = productvalue + popid.Quantity * popid.Price;
                            taxvalue = taxvalue + popid.Tax;
                        }

                    }
                    if (!verifyAndReworkPOPIDetailGridRows())
                    {
                        MessageBox.Show("Error found in PO details. Please correct before updating the details");
                    }
                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                    tabControl1.SelectedTab = tabPOHeader;
                    tabControl1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        //----
        private void showCLosingRequestPopUp()
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
                frmPopup.Size = new Size(360, 170);

                Label head = new Label();
                head.AutoSize = true;
                head.Location = new System.Drawing.Point(3, 3);
                head.Name = "label2";
                head.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                head.ForeColor = Color.White;
                head.Size = new System.Drawing.Size(146, 13);
                head.Text = "Give Comment";
                frmPopup.Controls.Add(head);

                txtDesc = new RichTextBox();
                txtDesc.Text = "";
                txtDesc.Bounds = new Rectangle(new Point(3, 25), new Size(345, 111));
                frmPopup.Controls.Add(txtDesc);

                Button lvOK = new Button();
                lvOK.Text = "OK";
                lvOK.BackColor = Color.Tan;
                lvOK.Location = new System.Drawing.Point(210, 142);
                lvOK.Size = new System.Drawing.Size(64, 23);
                lvOK.Cursor = Cursors.Hand;
                lvOK.Click += new System.EventHandler(this.lvOK_ClickReqClose);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(273, 142);
                lvCancel.Size = new System.Drawing.Size(73, 23);
                lvCancel.Cursor = Cursors.Hand;
                lvCancel.Click += new System.EventHandler(this.lvCancel_ClickReqClose);
                frmPopup.Controls.Add(lvCancel);

                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {

            }
        }
        private void lvOK_ClickReqClose(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtDesc.Text.Trim()))
                {
                    MessageBox.Show("Comment is empty");
                    return;
                }
                DialogResult dialog = MessageBox.Show("Are you sure to send request for closing ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    string comment = txtDesc.Text.Trim();
                    string prevComment = prevpopi.Comments;
                    int tmpStatus = 1;
                    string newComments = docCmtrDB.processNewComment(dgvComments, comment, Login.userLoggedIn, Login.userLoggedInName, tmpStatus);

                    if (POPIHeaderDB.RequestTOClosePOHeader(prevpopi, newComments))
                    {
                        MessageBox.Show("Closing request sent");
                        grdList.CurrentRow.Cells["ClosingStatus"].Value = 1;
                        grdList.CurrentRow.DefaultCellStyle.BackColor = Color.Tan;
                        //ListFilteredStockOBHeader(listOption);
                    }
                    else
                    {
                        MessageBox.Show("Closing request failed");
                    }
                }
                frmPopup.Close();
                frmPopup.Dispose();
                //setButtonVisibility("btnEditPanel");
            }
            catch (Exception ex)
            {

            }
        }

        private void lvCancel_ClickReqClose(object sender, EventArgs e)
        {
            try
            {
                txtDesc.Text = "";
                frmPopup.Close();
                frmPopup.Dispose();
                //setButtonVisibility("btnEditPanel");

            }
            catch (Exception)
            {
            }
        }

        private void showpopUpForCLosing()
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
                frmPopup.Size = new Size(360, 80);

                Label head = new Label();
                head.AutoSize = true;
                head.Location = new System.Drawing.Point(3, 3);
                head.Name = "label2";
                head.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                head.ForeColor = Color.White;
                head.Size = new System.Drawing.Size(146, 13);
                head.Text = "Request For CLosing. Are You Sure To Close !! ";
                frmPopup.Controls.Add(head);

                Button lvCLose = new Button();
                lvCLose.Text = "Close";
                lvCLose.BackColor = Color.Tan;
                lvCLose.Location = new System.Drawing.Point(20, 52);
                //lvCLose.Size = new System.Drawing.Size(64, 23);
                lvCLose.Cursor = Cursors.Hand;
                lvCLose.Click += new System.EventHandler(this.lvCLose_ClickClose);
                frmPopup.Controls.Add(lvCLose);

                Button lvReject = new Button();
                lvReject.Text = "Reject";
                lvReject.BackColor = Color.Tan;
                lvReject.Location = new System.Drawing.Point(100, 52);
                //lvReject.Size = new System.Drawing.Size(120, 23);
                lvReject.Cursor = Cursors.Hand;
                lvReject.Click += new System.EventHandler(this.lvReject_ClickClose);
                frmPopup.Controls.Add(lvReject);

                Button lvCancel = new Button();
                lvCancel.Text = "Back";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(180, 52);
                //lvCancel.Size = new System.Drawing.Size(90, 23);
                lvCancel.Cursor = Cursors.Hand;
                lvCancel.Click += new System.EventHandler(this.lvCancel_ClickClose);
                frmPopup.Controls.Add(lvCancel);



                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {

            }
        }
        private void lvCLose_ClickClose(object sender, EventArgs e)
        {
            try
            {
                if (POPIHeaderDB.ClosePOHeader(prevpopi))
                {
                    MessageBox.Show("PO inward closed.");
                    //ListFilteredStockOBHeader(listOption);
                    grdList.Rows.Remove(grdList.CurrentRow);
                }
                else
                {
                    MessageBox.Show("Fail to close PO inward");
                }

                frmPopup.Close();
                frmPopup.Dispose();
                //setButtonVisibility("btnEditPanel");
            }
            catch (Exception ex)
            {

            }
        }
        private void lvReject_ClickClose(object sender, EventArgs e)
        {
            try
            {
                string comment = "Closing request rejected";
                string prevComment = prevpopi.Comments;
                int tmpStatus = 1;
                string newComments = docCmtrDB.processNewComment(dgvComments, comment, Login.userLoggedIn, Login.userLoggedInName, tmpStatus);

                if (POPIHeaderDB.RejectClosingRequest(prevpopi, newComments))
                {
                    MessageBox.Show("Closing request rejected.");
                    grdList.CurrentRow.Cells["ClosingStatus"].Value = 0;
                    if ((grdList.CurrentRow.Index % 2) == 0)
                        grdList.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                    else
                        grdList.CurrentRow.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                    //ListFilteredStockOBHeader(listOption);
                }
                else
                {
                    MessageBox.Show("Failed to reject");
                }
                frmPopup.Close();
                frmPopup.Dispose();
                //setButtonVisibility("btnEditPanel");
            }
            catch (Exception ex)
            {

            }
        }
        private void lvCancel_ClickClose(object sender, EventArgs e)
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
        //===
        private void btnClearEntries_Click_1(object sender, EventArgs e)
        {
            try
            {


                DialogResult dialog = MessageBox.Show("Are you sure to clear all entries in grid detail ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdPOPIDetail.Rows.Clear();
                    MessageBox.Show("Grid items cleared.");
                    verifyAndReworkPOPIDetailGridRows();
                    chkDuplicateRow.Checked = false;
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
                POPIHeaderDB popihdb = new POPIHeaderDB();
                DashboardDB dsDB = new DashboardDB();
                FinancialLimitDB flDB = new FinancialLimitDB();
                if (!flDB.checkEmployeeFinancialLimit(docID, Login.empLoggedIn, Convert.ToDouble(txtProductValueINR.Text), 0))
                {
                    MessageBox.Show("No financial power for approving this document");
                    return;
                }
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevpopi.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevpopi.CommentStatus);
                    if (prevpopi.status != 96)
                    {

                        if (prevpopi.DocumentID == "PAFPRODUCTINWARD")  //For Product PAF
                            prevpopi.TrackingNo = DocumentNumberDB.getNewNumber("POPRODUCTINWARD", 2);
                        else if (prevpopi.DocumentID == "PAFSERVICEINWARD") //For Service PAF
                            prevpopi.TrackingNo = DocumentNumberDB.getNewNumber("POSERVICEINWARD", 2);
                        else
                            prevpopi.TrackingNo = DocumentNumberDB.getNewNumber(docID, 2);

                        //prevpopi.TrackingNo = DocumentNumberDB.getNewNumber(docID, 2);
                    }
                    if (popihdb.ApprovePOPI(prevpopi))
                    {
                        MessageBox.Show("PO Product Inward Document Approved");
                        if (!updateDashBoard(prevpopi, 2))
                        {
                            MessageBox.Show("Failed to update DashBoard info");
                        }
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredStockOBHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception)
            {
            }

        }
        private Boolean updateDashBoard(popiheader popih, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = popih.DocumentID;
                dsb.TemporaryNo = popih.TemporaryNo;
                dsb.TemporaryDate = popih.TemporaryDate;
                dsb.DocumentNo = popih.TrackingNo;
                dsb.DocumentDate = popih.TrackingDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = popih.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(prevpopi.DocumentID);
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
        //private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        customer cust = new customer();
        //        ////////string custid = cmbCustomer.SelectedItem.ToString().Trim().Substring(0, cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-'));
        //        string custid = ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue;
        //        cust = CustomerDB.getCustomerDetails(custid);
        //        txtBillingAddress.Text = cust.BillingAddress;
        //        txtDeliveryAddress.Text = cust.DeliveryAddress;
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
            removeControlsFromCommenterPanel();
            removeControlsFromForwarderPanel();
            removeControlsFromModelPanel();
            removeControlsFromPayTermPanel();
            //removeControlsFromForwarderPanelTV();
            //New PO
            if (btnSave.Text == "Save")
            {
                if (cmbPOType.SelectedIndex == -1)
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
                        dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevpopi.TemporaryNo + "-" + prevpopi.TemporaryDate.ToString("yyyyMMddhhmmss"));
                        dgvDocumentList.Size = new Size(870, 300);
                        pnlPDFViewer.Controls.Add(dgvDocumentList);
                        dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
                        if (prevpopi.status == 1 && prevpopi.DocumentStatus == 99)
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
                //string potype = cmbPOType.SelectedItem.ToString().Trim().Substring(0, cmbPOType.SelectedItem.ToString().Trim().IndexOf('-'));
                string potype = ((Structures.ComboBoxItem)cmbPOType.SelectedItem).HiddenValue;
                if (potype == "Product")
                {
                    docID = "POPRODUCTINWARD";
                    chkDocID = "POPRODUCTINWARD";
                }
                else if (potype == "Service")
                {
                    docID = "POSERVICEINWARD";
                    chkDocID = "POSERVICEINWARD";
                }
                else if (potype == "RateContract")
                {
                    docID = "POSERVICERCINWARD";
                    chkDocID = "POSERVICERCINWARD";
                }
                else if (potype == "ProductPAF")
                {
                    docID = "PAFPRODUCTINWARD";
                    chkDocID = "PAFPRODUCTINWARD";
                }
                else if (potype == "ServicePAF")
                {
                    docID = "PAFSERVICEINWARD";
                    chkDocID = "PAFSERVICEINWARD";
                }
                setPODetailColumns(docID);
                cmbPOType.Enabled = false;
            }
            catch (Exception)
            {
            }
        }
        private void setPODetailColumns(string docID)
        {
            try
            {
                if (docID == "POPRODUCTINWARD")
                {

                }
                else if (docID == "POSERVICEINWARD")
                {

                }
            }
            catch (Exception)
            {
            }
        }

        private void cmbFreightTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string fterm = ((Structures.ComboBoxItem)cmbFreightTerms.SelectedItem).HiddenValue;
                if (fterm.Trim().Equals("Extra"))
                {
                    txtFreightCharge.Enabled = true;
                }
                else
                {
                    txtFreightCharge.Text = "0";
                    txtFreightCharge.Enabled = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void removeControlsFromPayTermPanel()
        {
            try
            {
                //foreach (Control p in pnldgv.Controls)
                //    if (p.GetType() == typeof(Button))
                //    {
                //        p.Dispose();
                //    }
                pnldgv.Controls.Clear();
                Control nc = pnldgv.Parent;
                nc.Controls.Remove(pnldgv);
            }
            catch (Exception ex)
            {
            }
        }
        private void btnPaymentTermsSelection_Click(object sender, EventArgs e)
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

                frmPopup.Size = new Size(505, 300);

                dgvpt = new DataGridView();
                dgvpt = PTDefinitionDB.AcceptPaymentTerms(txtPaymentTerms.Text);
                dgvpt.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new Size(505, 250));
                frmPopup.Controls.Add(dgvpt);

                Button dgvptOK = new Button();
                dgvptOK.BackColor = Color.Tan;
                dgvptOK.Text = "OK";
                dgvptOK.Location = new Point(44, 265);
                dgvptOK.Click += new System.EventHandler(this.dgvptOK_Click);
                frmPopup.Controls.Add(dgvptOK);

                Button dgvptCancel = new Button();
                dgvptCancel.Text = "CANCEL";
                dgvptCancel.BackColor = Color.Tan;
                dgvptCancel.Location = new Point(141, 265);
                dgvptCancel.Click += new System.EventHandler(this.dgvptCancel_Click);
                frmPopup.Controls.Add(dgvptCancel);

                Button dgvptAddRow = new Button();
                dgvptAddRow.Text = "Add Credit Row";
                dgvptAddRow.BackColor = Color.Tan;
                dgvptAddRow.Location = new Point(300, 265);
                dgvptAddRow.Click += new System.EventHandler(this.dgvptAddRow_Click);
                frmPopup.Controls.Add(dgvptAddRow);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
                //btnPaymentTermsSelection.Enabled = true;

            }
        }
        private void dgvptOK_Click(object sender, EventArgs e)
        {
            try
            {
                int tperc = 0;
                int totperc = 0;
                int tcrdays = 0;
                ////int pcrdays = 0;
                ////int tval = 0;
                string tstr = "";
                string valStr = "";
                for (int i = 0; i < dgvpt.Rows.Count; i++)
                {
                    try
                    {
                        tperc = Convert.ToInt32(dgvpt.Rows[i].Cells["Percentage"].Value);
                        tstr = dgvpt.Rows[i].Cells["Description"].Value.ToString();
                        tcrdays = Convert.ToInt32(dgvpt.Rows[i].Cells["CreditPeriod"].Value);
                    }
                    catch (Exception)
                    {
                        tperc = 0;
                        tstr = "";
                        tcrdays = 0;
                    }
                    totperc = totperc + tperc;
                    if (tstr.Equals("Credit"))
                    {
                        if (!((tcrdays == 0 && tperc == 0) || (tcrdays != 0 && tperc != 0)))
                        {
                            MessageBox.Show("Error in credit entries");
                            return;
                        }
                    }
                    else
                    {
                        if (tcrdays > 0)
                        {
                            MessageBox.Show("Error in credit days");
                            return;
                        }
                    }
                    if (tperc > 0)
                    {
                        try
                        {
                            string val1, val2, val3;
                            //val1 = dgvpt.Rows[i].Cells["Code"].Value.ToString();
                            val2 = dgvpt.Rows[i].Cells["Percentage"].Value.ToString();
                            val3 = dgvpt.Rows[i].Cells["CreditPeriod"].Value.ToString();
                            val1 = dgvpt.Rows[i].Cells["ID"].Value.ToString();
                            valStr = valStr + val1 + "-" + val2 + "-" + val3 + ";";
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                if (totperc != 100)
                {
                    MessageBox.Show("Error in percentage values");
                    return;
                }
                txtPaymentTerms.Text = valStr.ToString();
                removeControlsFromFrmPopup();
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        private void dgvptCancel_Click(object sender, EventArgs e)
        {
            try
            {
                removeControlsFromFrmPopup();
                frmPopup.Close();
                frmPopup.Dispose();

            }
            catch (Exception)
            {
            }
        }
        private void removeControlsFromFrmPopup()
        {
            frmPopup.Controls.Clear();
        }
        private void dgvptAddRow_Click(object sender, EventArgs e)
        {
            try
            {
                ////int i = dgv.Rows.Count;
                dgvpt.Rows.Add();
                dgvpt.Rows[dgvpt.Rows.Count - 1].Cells["Code"].Value = dgvpt.Rows[dgvpt.Rows.Count - 2].Cells["Code"].Value;
                dgvpt.Rows[dgvpt.Rows.Count - 1].Cells["ID"].Value = dgvpt.Rows[dgvpt.Rows.Count - 2].Cells["ID"].Value;
                dgvpt.Rows[dgvpt.Rows.Count - 1].Cells["Description"].Value = dgvpt.Rows[dgvpt.Rows.Count - 2].Cells["Description"].Value;
                dgvpt.Rows[dgvpt.Rows.Count - 1].Cells["Percentage"].Value = 0;
                dgvpt.Rows[dgvpt.Rows.Count - 1].Cells["CreditPeriod"].Value = 0;
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
            ////removeControlsFromCommenterPanel();

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
                            POPIHeaderDB popihdb = new POPIHeaderDB();
                            prevpopi.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevpopi.CommentStatus);
                            prevpopi.ForwardUser = approverUID;
                            prevpopi.ForwarderList = prevpopi.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (popihdb.forwardPOPI(prevpopi))
                            {
                                MessageBox.Show("Document Forwarded");
                                if (updateDashBoard(prevpopi, 1))
                                {
                                    frmPopup.Close();
                                    frmPopup.Dispose();
                                    closeAllPanels();
                                    listOption = 1;
                                    ListFilteredStockOBHeader(listOption);
                                    setButtonVisibility("btnEditPanel");//activites are same for cance, forward,approce and reverse
                                }
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
                    string reverseStr = getReverseString(prevpopi.ForwarderList);
                    //do forward activities
                    prevpopi.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevpopi.CommentStatus);
                    POPIHeaderDB popihdb = new POPIHeaderDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevpopi.ForwarderList = reverseStr.Substring(0, ind);
                        prevpopi.ForwardUser = reverseStr.Substring(ind + 3);
                        prevpopi.DocumentStatus = prevpopi.DocumentStatus - 1;
                    }
                    else
                    {
                        prevpopi.ForwarderList = "";
                        prevpopi.ForwardUser = "";
                        prevpopi.DocumentStatus = 1;
                    }
                    if (popihdb.reversePOPI(prevpopi))
                    {
                        MessageBox.Show("PO Product Inward Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredStockOBHeader(listOption);
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
                //    if (p.GetType() == typeof(TreeView) || p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
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
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevpopi.TemporaryNo + "-" + prevpopi.TemporaryDate.ToString("yyyyMMddhhmmss"));
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
                    string subDir = prevpopi.TemporaryNo + "-" + prevpopi.TemporaryDate.ToString("yyyyMMddhhmmss");
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
                                tabControl1.SelectedIndex = -1;
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
                btnGetComments.Visible = false;
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
                    pnlBottomButtons.Visible = false; //24/11/2016
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
                    tabControl1.SelectedTab = tabPOType;
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
                    tabControl1.SelectedTab = tabPODetail;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                    disableTabPages();
                    tabControl1.SelectedTab = tabPODetail;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabPODetail;
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
                txtComments.Text = "";
                grdPOPIDetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }

        private void txtExchangeRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdPOPIDetail.Rows.Count != 0 && txtPOValue.Text.Length != 0
                    && txtProductValue.Text.Length != 0 && txtExchangeRate.Text.Length != 0)
                {
                    double dd = Convert.ToDouble(txtExchangeRate.Text);
                    txtProductValueINR.Text = (Convert.ToDouble(txtProductValue.Text) * dd).ToString();
                    txtPOValueINR.Text = (Convert.ToDouble(txtPOValue.Text) * dd).ToString();
                    txtTaxAmountINR.Text = (Convert.ToDouble(txtTaxAmount.Text) * dd).ToString();
                }
                if (txtExchangeRate.Text.Length == 0)
                {
                    txtProductValueINR.Text = "";
                    txtPOValueINR.Text = "";
                    txtTaxAmountINR.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        //-- Validating Header and Detail String For Single Quotes

        private popiheader verifyHeaderInputString(popiheader poh)
        {
            try
            {
                poh.BillingAddress = Utilities.replaceSingleQuoteWithDoubleSingleQuote(poh.BillingAddress);
                poh.Remarks = Utilities.replaceSingleQuoteWithDoubleSingleQuote(poh.Remarks);
                poh.ReferenceNo = Utilities.replaceSingleQuoteWithDoubleSingleQuote(poh.ReferenceNo);
                poh.DeliveryAddress = Utilities.replaceSingleQuoteWithDoubleSingleQuote(poh.DeliveryAddress);
                poh.CustomerID = Utilities.replaceSingleQuoteWithDoubleSingleQuote(poh.CustomerID);
                poh.CustomerPONO = Utilities.replaceSingleQuoteWithDoubleSingleQuote(poh.CustomerPONO);
                poh.ProjectID = Utilities.replaceSingleQuoteWithDoubleSingleQuote(poh.ProjectID);
            }
            catch (Exception ex)
            {
            }
            return poh;
        }
        private void verifyDetailInputString()
        {
            try
            {
                for (int i = 0; i < grdPOPIDetail.Rows.Count; i++)
                {
                    if (docID == "POPRODUCTINWARD" || docID == "PAFPRODUCTINWARD")
                    {
                        grdPOPIDetail.Rows[i].Cells["ProductItem"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdPOPIDetail.Rows[i].Cells["ProductItem"].Value.ToString());
                        grdPOPIDetail.Rows[i].Cells["ModelNo"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdPOPIDetail.Rows[i].Cells["ModelNo"].Value.ToString());
                    }
                    else
                    {
                        try
                        {
                            grdPOPIDetail.Rows[i].Cells["ServiceItem"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdPOPIDetail.Rows[i].Cells["ServiceItem"].Value.ToString());
                            grdPOPIDetail.Rows[i].Cells["gOfficeID"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdPOPIDetail.Rows[i].Cells["gOfficeID"].Value.ToString());
                        }
                        catch (Exception ex)
                        {
                            grdPOPIDetail.Rows[i].Cells["ServiceItem"].Value = "";
                            grdPOPIDetail.Rows[i].Cells["gOfficeID"].Value = "";
                        }
                    }
                    grdPOPIDetail.Rows[i].Cells["CustomerItemDescription"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdPOPIDetail.Rows[i].Cells["CustomerItemDescription"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
            }
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
                            if (!row.Cells["gCustomerPONo"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
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
        ////////private void txtSearch_TextChanged(object sender, EventArgs e)
        ////////{
        ////////    filterGridData();
        ////////}
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ////MessageBox.Show("txtSearch_TextChanged() : started");
            ////filterTimer = new Timer();
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

        private void btnSelProjectID_Click(object sender, EventArgs e)
        {
            showProjectIDDataGridView();
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
                    cmbProjectID.SelectedIndex = cmbProjectID.FindString(row.Cells["ProjectID"].Value.ToString());
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
        private string getDocIDCondition()
        {
            string cstr = "";
            //serach each string in docment list to mail.documentlist
            //if found string=string="<value>"
            return cstr;
        }

        private void POPIHeader_Enter(object sender, EventArgs e)
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

        private void btnSelCustomer_Click(object sender, EventArgs e)
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
                    cust = CustomerDB.getCustomerDetails(txtCustomerID.Text);
                    txtBillingAddress.Text = cust.BillingAddress;
                    txtDeliveryAddress.Text = cust.DeliveryAddress;
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
    }
}

