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

namespace CSLERP
{
    public partial class PurchaseOrder : System.Windows.Forms.Form
    {
        string docID = "PURCHASEORDER";
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        string forwarderList = "";
        string approverList = "";
        double productvalue = 0.0;
        double taxvalue = 0.0;
        poheader prevpoh ;
        Timer filterTimer = new Timer();
        Timer filterTimer1 = new Timer();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        Panel pnllv = new Panel();
        ListView lv = new ListView();
        TreeView tv = new TreeView();
        Panel pnlModel = new Panel();
        string userString = "";
        string colName = "";
        DataGridView grdStock = new DataGridView();
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Boolean userIsACommenter = false;
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        Form frmPopup = new Form();
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        int descClickRowIndex = -1;
        RichTextBox txtDesc = new RichTextBox();
        Boolean AddRowClick = false;
        DataGridView grdCustList = new DataGridView();
        Dictionary<string, string[]> dictItemWiseTOt = new Dictionary<string, string[]>();
        string colClicked = "";
        Boolean isNewClick = false;
        TextBox txtSearch = new TextBox();
        List<doctcmapping> TCList = new List<doctcmapping>();
        CheckedListBox chkBoxCustomer = new CheckedListBox();
        DataGridView grdTC = new DataGridView();
        public PurchaseOrder()
        {
            try
            {
                InitializeComponent();
                //////this.FormBorderStyle = FormBorderStyle.None;
                Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
                initVariables();
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                this.Width -= 100;
                this.Height -= 100;
                
                String a = this.Size.ToString();
                grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdList.EnableHeadersVisualStyles = false;
                grdIndentDetail.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdIndentDetail.EnableHeadersVisualStyles = false;
                grdItemWiseTotalQuant.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdItemWiseTotalQuant.EnableHeadersVisualStyles = false;
                ListFilteredPOHeader(listOption);
            }
            catch (Exception)
            {

            }
        }

        private void ListFilteredPOHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                PurchaseOrderDB podb = new PurchaseOrderDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                List<poheader> POHeaders = podb.getFilteredPurchaseOrderHeader(userString, option, userCommentStatusString);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (poheader poh in POHeaders)
                {
                    if (option == 1)
                    {
                        if (poh.DocumentStatus == 99)
                            continue;
                    }

                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["Did"].Value = poh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["Dname"].Value = poh.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["TempNo"].Value = poh.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["TempDate"].Value = poh.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["PONo"].Value = poh.PONo;
                    grdList.Rows[grdList.RowCount - 1].Cells["PODate"].Value = poh.PODate;
                    grdList.Rows[grdList.RowCount - 1].Cells["RefIndent"].Value = poh.ReferenceIndent;
                    grdList.Rows[grdList.RowCount - 1].Cells["RefQuot"].Value = poh.ReferenceQuotation;
                    grdList.Rows[grdList.RowCount - 1].Cells["CustID"].Value = poh.CustomerID;
                    grdList.Rows[grdList.RowCount - 1].Cells["CustName"].Value = poh.CustomerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["CurID"].Value = poh.CurrencyID;
                    grdList.Rows[grdList.RowCount - 1].Cells["ExchangeRate"].Value = poh.ExchangeRate;
                    grdList.Rows[grdList.RowCount - 1].Cells["DelvPeriod"].Value = poh.DeliveryPeriod;
                    grdList.Rows[grdList.RowCount - 1].Cells["ValidityPeriod"].Value = poh.validityPeriod;
                    grdList.Rows[grdList.RowCount - 1].Cells["TaxTerm"].Value = poh.TaxTerms;
                    //grdList.Rows[grdList.RowCount - 1].Cells["TCode"].Value = poh.TaxCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["ModeOfPayment"].Value = poh.ModeOfPayment;
                    grdList.Rows[grdList.RowCount - 1].Cells["PaymentTerm"].Value = poh.PaymentTerms;
                    grdList.Rows[grdList.RowCount - 1].Cells["TransportationMode"].Value = poh.TransportationMode;
                   grdList.Rows[grdList.RowCount - 1].Cells["Country"].Value = poh.CountryID;
                    grdList.Rows[grdList.RowCount - 1].Cells["FreightTerm"].Value = poh.FreightTerms;
                    grdList.Rows[grdList.RowCount - 1].Cells["FreightCharge"].Value = poh.FreightCharge;
                    grdList.Rows[grdList.RowCount - 1].Cells["DelvAddress"].Value = poh.DeliveryAddress;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProdValue"].Value = poh.ProductValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProductValueINR"].Value = poh.ProductValueINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["POValueINR"].Value = poh.POValueINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["TaxAmount"].Value = poh.TaxAmount;
                    grdList.Rows[grdList.RowCount - 1].Cells["TaxAmountINR"].Value = poh.TaxAmountINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["POValue"].Value = poh.POValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["Remark"].Value = poh.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["TermsAndCondition"].Value = poh.TermsAndCondition;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = poh.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatus"].Value = poh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = poh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = poh.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = poh.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = poh.ApproverName;
                    //grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatusNo"].Value = ComboFIll.getDocumentStatusString(woh.DocumentStatus);
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = poh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["SpecialNote"].Value = poh.SpecialNote;
                    grdList.Rows[grdList.RowCount - 1].Cells["ComStatus"].Value = poh.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comment"].Value = poh.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = poh.ForwarderList;

                    grdList.Rows[grdList.RowCount - 1].Cells["PartialShipment"].Value = poh.PartialShipment;
                    grdList.Rows[grdList.RowCount - 1].Cells["Transhipment"].Value = poh.Transhipment;
                    grdList.Rows[grdList.RowCount - 1].Cells["PackingSpec"].Value = poh.PackingSpec;
                    grdList.Rows[grdList.RowCount - 1].Cells["PriceBasis"].Value = poh.PriceBasis;
                    grdList.Rows[grdList.RowCount - 1].Cells["DeliveryAt"].Value = poh.DeliveryAt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Purchase Order Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;
            isNewClick = false;
            grdList.Columns["Creator"].Visible = true;
            grdList.Columns["Forwarder"].Visible = true;
            grdList.Columns["Approver"].Visible = true;


        }
        private void setTabIndex()
        {
            txtTemporaryNo.TabIndex = 0;
            dtTemporaryDate.TabIndex = 1;
            txtPONo.TabIndex = 2;
            dtPODate.TabIndex = 3;
            btnSelectCustomer.TabIndex = 4;
            cmbFreight.TabIndex = 5;
            txtFreightCharge.TabIndex = 6;
            txtDeliveryPeriod.TabIndex = 7;
            txtValidityPeriod.TabIndex = 8;
            cmbPartialShipment.TabIndex = 9;
            cmbTranshipment.TabIndex = 10;
            cmbpricebasis.TabIndex = 11;
            txtDelvAt.TabIndex = 12;

            cmbTaxTerms.TabIndex = 13;
            txtPaymentTerms.TabIndex = 14;
            btnPaymentTermSelect.TabIndex = 15;
            cmbPaymentMode.TabIndex = 16;
            cmbTransMode.TabIndex = 17;
            
            txtPackingSpec.TabIndex = 18;
            btnShowPackingSpec.TabIndex = 19;
            txtReferenceIndent.TabIndex = 20;
            btnReferenceIndent.TabIndex = 21;
            txtReferenceQuotation.TabIndex = 22;
            btnReferenceQuotation.TabIndex = 23;
            txtTermsAndCondition.TabIndex = 24;
            btnSelectTermsAndCondition.TabIndex = 25;
            cmbCurrency.TabIndex = 26;
            txtExchangeRate.TabIndex = 27;
            txtDeliveryAddress.TabIndex = 28;
            btnSelDelvAddress.TabIndex = 29;
            txtRemarks.TabIndex = 30;
            txtSpecialNote.TabIndex = 31;
        }
        private void initVariables()
        {
            DocTcMappingDB tcmapdb = new DocTcMappingDB();
            TCList = tcmapdb.getdocTCList(docID);
            if (getuserPrivilegeStatus() == 1)
            {
                //user is only a viewer
                listOption = 6;
            }
            docID = Main.currentDocument;

            //CustomerDB.fillCustomerComboNew(cmbCustomer);
            //CatalogueValueDB.fillCatalogValueCombo(cmbPaymentTerms, "PaymentTerms");
            CatalogueValueDB.fillCatalogValueComboNew(cmbPaymentMode, "PaymentMode");
            CatalogueValueDB.fillCatalogValueComboNew(cmbFreight, "Freight");
            CatalogueValueDB.fillCatalogValueComboNew(cmbTaxTerms, "TaxStatus");
            CatalogueValueDB.fillCatalogValueComboNew(cmbTransMode, "TransportationMode");
            CurrencyDB.fillCurrencyComboNew(cmbCurrency);
            CatalogueValueDB.fillCatalogValueComboNew(cmbpricebasis, "PriceBasis");
            cmbTranshipment.SelectedIndex = cmbTranshipment.FindString("No");
            cmbPartialShipment.SelectedIndex = cmbPartialShipment.FindString("No");
            //fillStatusCombo(cmbStatus);
            //TaxCodeDB.fillTaxCodeCombo(cmbTaxCode);
            cmbpricebasis.SelectedIndex = 0;
            dtTemporaryDate.Format = DateTimePickerFormat.Custom;
            dtTemporaryDate.CustomFormat = "dd-MM-yyyy";
            dtTemporaryDate.Enabled = false;
            dtPODate.Format = DateTimePickerFormat.Custom;
            dtPODate.CustomFormat = "dd-MM-yyyy";
            dtPODate.Enabled = false;
            txtTermsAndCondition.Enabled = false;
            txtTemporaryNo.Enabled = false;
            txtPONo.Enabled = false;
            //txtCreditPeriods.Enabled = true;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            //cmbCustomer.TabIndex = 0;
            grdPODetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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
        private void fillStatusCombo(System.Windows.Forms.ComboBox cmb)
        {
            try
            {
                cmb.Items.Clear();
                for (int i = 0; i < Main.statusValues.GetLength(0); i++)
                {
                    cmb.Items.Add(Main.statusValues[i, 1]);
                }
            }
            catch (Exception)
            {

            }
        }
        //private void applyPrivilege()
        //{
        //    try
        //    {
        //        if (Main.itemPriv[1])
        //        {
        //            btnNew.Visible = true;
        //        }
        //        else
        //        {
        //            btnNew.Visible = false;
        //        }
        //        if (Main.itemPriv[2])
        //        {
        //            grdList.Columns["Edit"].Visible = true;
        //        }
        //        else
        //        {
        //            grdList.Columns["Edit"].Visible = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

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

        public void clearData()
        {
            try
            {
                txtSearchPu.Text = "";
                //clear all grid views
                grdPODetail.Rows.Clear();
                dgvComments.Rows.Clear();
                //dgvpt.Rows.Clear();
                //----------clear temperory panels

                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                //----------
                cmbTranshipment.SelectedIndex = cmbTranshipment.FindString("No");
                cmbPartialShipment.SelectedIndex = cmbPartialShipment.FindString("No");
                cmbpricebasis.SelectedIndex = 0;
                txtPackingSpec.Text = "";
                dtTemporaryDate.Value = DateTime.Parse("1900-01-01");
                dtPODate.Value = DateTime.Parse("1900-01-01");
                grdPODetail.Rows.Clear();
                txtDeliveryPeriod.Text = "";
                txtTemporaryNo.Text = "";
                txtPONo.Text = "";
                cmbpricebasis.SelectedIndex = 0;
                txtDelvAt.Text = "";
                txtDeliveryAddress.Text = "";
                txtSpecialNote.Text = "";
                txtRemarks.Text = "";
                txtTaxAmountINR.Text = "";
                txtFreightCharge.Text = "";
                txtReferenceIndent.Text = "";
                txtReferenceQuotation.Text = "";
                txtTermsAndCondition.Text = "";
                txtPaymentTerms.Text = "";
                txtPOTax.Text = "";
                txtExchangeRate.Text = "";
                txtPOValue.Text = "";
                txtPOValueINR.Text = "";
                txtProductValue.Text = "";
                txtProductValueINR.Text = "";
                txtValidityPeriod.Text = "";
                btnProductValue.Text = "0";
                btnTaxAmount.Text = "0";
                prevpoh = new poheader();
                removeControlsFromForwarderPanelTV();
                txtCustomer.Text = "";
                cmbCurrency.SelectedIndex = -1;
                //cmbTaxCode.SelectedIndex = -1;
                cmbPaymentMode.SelectedIndex = -1;
                cmbTransMode.SelectedIndex = -1;
                descClickRowIndex = -1;
                AddRowClick = false;
                isNewClick = false;
                colClicked = "";
                commentStatus = "";
                grdIndentDetail.Rows.Clear();
                tabControl1.TabPages["tabIndentDetail"].Visible = false;
                grdIndentDetail.Visible = false;
                grdItemWiseTotalQuant.Visible = false;
                grdItemWiseTotalQuant.Rows.Clear();
                colClicked = "";
                isNewClick = false;
                try
                {
                    cmbFreight.SelectedItem = null;
                }
                catch (Exception ex)
                {

                }
                try
                {
                    cmbTaxTerms.SelectedItem = null;
                }
                catch (Exception)
                {
                }
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
                tabControl1.SelectedTab = tabPOHeader;
                tabPOHeader.Enabled = true;
                setButtonVisibility("btnNew");
                txtTermsAndCondition.Text = string.Join(";", TCList.Select(c => c.ParagraphID.ToString()).ToArray<string>());
                AddRowClick = false;
                isNewClick = true;
            }
            catch (Exception)
            {

            }
        }
        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                AddPODetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddPODetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdPODetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkPODetailGridRows())
                    {
                        return false;
                    }
                }
                grdPODetail.Rows.Add();
                int kount = grdPODetail.RowCount;
                grdPODetail.Rows[kount - 1].Cells[0].Value = kount;
                grdPODetail.Rows[kount - 1].Cells["Item"].Value = "";
                grdPODetail.Rows[kount - 1].Cells["ModelNo"].Value = "";
                grdPODetail.Rows[kount - 1].Cells["ModelName"].Value = "";
                grdPODetail.Rows[kount - 1].Cells["Description"].Value = "";
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdPODetail.Rows[kount - 1].Cells["gTCode"]);
                TaxCodeDB.fillTaxCodeGridViewCombo(ComboColumn2, "");
                grdPODetail.Rows[kount - 1].Cells["Unit"].Value = "";
                grdPODetail.Rows[kount - 1].Cells["Quantity"].Value = 0;
                grdPODetail.Rows[kount - 1].Cells["Price"].Value = 0;
                grdPODetail.Rows[kount - 1].Cells["Value"].Value = 0;
                grdPODetail.Rows[kount - 1].Cells["Tax"].Value = 0;
                grdPODetail.Rows[kount - 1].Cells["WarrantyDays"].Value = 0;
                grdPODetail.Rows[kount - 1].Cells["TaxDetails"].Value = " ";
                var BtnCell = (DataGridViewButtonCell)grdPODetail.Rows[kount - 1].Cells["Delete"];
                BtnCell.Value = "Del";
                if (AddRowClick)
                {
                    grdPODetail.FirstDisplayedScrollingRowIndex = grdPODetail.RowCount - 1;
                    grdPODetail.CurrentCell = grdPODetail.Rows[kount - 1].Cells[0];
                }
                //grdPODetail.Columns[0].Frozen = false;
                grdPODetail.FirstDisplayedScrollingColumnIndex = 0;
                //grdPODetail.Columns["selDesc"].Frozen = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddPODetailRow() : Error");
            }

            return status;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            closeAllPanels();
            btnNew.Visible = true;
            btnExit.Visible = true;
            pnlList.Visible = true;
        }

        private Boolean verifyAndReworkPODetailGridRows()
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

                if (grdPODetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Purchase Order details");
                    btnProductValue.Text = productvalue.ToString();
                    btnTaxAmount.Text = taxvalue.ToString(); //fill this later
                    txtProductValue.Text = productvalue.ToString();
                    txtProductValueINR.Text = (Convert.ToDouble(txtProductValue.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    txtPOTax.Text = taxvalue.ToString(); //fill this later
                    txtTaxAmountINR.Text = (Convert.ToDouble(txtPOTax.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    txtPOValue.Text = (productvalue + taxvalue).ToString();
                    txtPOValueINR.Text = (Convert.ToDouble(txtPOValue.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    return false;
                }
                //clear tax details table
                TaxDetailsTable.Rows.Clear();
                for (int i = 0; i < grdPODetail.Rows.Count; i++)
                {

                    grdPODetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if(grdPODetail.Rows[i].Cells["gTCode"].Value == null)
                    {
                        MessageBox.Show("Fill TaxCode in row " + (i + 1));
                        return false;
                    }
                    if ((grdPODetail.Rows[i].Cells["Item"].Value == null) ||
                        (grdPODetail.Rows[i].Cells["Description"].Value == null) ||
                         (grdPODetail.Rows[i].Cells["ModelNo"].Value.ToString().Length == 0) ||
                          (grdPODetail.Rows[i].Cells["ModelName"].Value.ToString().Length == 0) ||
                        (grdPODetail.Rows[i].Cells["Description"].Value.ToString().Trim().Length == 0) ||
                        (grdPODetail.Rows[i].Cells["Quantity"].Value == null) ||
                        (grdPODetail.Rows[i].Cells["Price"].Value == null) ||
                        (Convert.ToDouble(grdPODetail.Rows[i].Cells["Quantity"].Value) == 0) ||
                        (Convert.ToDouble(grdPODetail.Rows[i].Cells["Price"].Value) == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }

                    quantity = Convert.ToDouble(grdPODetail.Rows[i].Cells["Quantity"].Value);
                    price = Convert.ToDouble(grdPODetail.Rows[i].Cells["Price"].Value);
                    if (price != 0 && quantity != 0)
                    {
                        cost = Math.Round(quantity * price, 2);
                    }

                    try
                    {
                        strtaxCode = grdPODetail.Rows[i].Cells["gTCode"].Value.ToString();
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
                    grdPODetail.Rows[i].Cells["Value"].Value = Convert.ToDouble(cost.ToString());
                    grdPODetail.Rows[i].Cells["Tax"].Value = ttax2.ToString();
                    grdPODetail.Rows[i].Cells["TaxDetails"].Value = strTax;
                    productvalue = productvalue + cost;
                    taxvalue = taxvalue + ttax2;
                }
                btnProductValue.Text = productvalue.ToString();
                btnTaxAmount.Text = taxvalue.ToString(); //fill this later
                                                         //btnPOValue.Text = (productvalue + taxvalue).ToString();
                txtProductValue.Text = productvalue.ToString();
                txtProductValueINR.Text = (Convert.ToDouble(txtProductValue.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                txtPOTax.Text = taxvalue.ToString(); //fill this later
                txtTaxAmountINR.Text = (Convert.ToDouble(txtPOTax.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                txtPOValue.Text = (productvalue + taxvalue).ToString();
                txtPOValueINR.Text = (Convert.ToDouble(txtPOValue.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();

                if (!validateItems())
                {
                    MessageBox.Show("Validation failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Check Purchase ORder detail.");
                return false;
            }
            return status;
        }
        private Boolean validateItems()
        {
            Boolean status = true;
            try
            {
                if (docID == "PURCHASEORDER")
                {

                    for (int i = 0; i < grdPODetail.Rows.Count - 1; i++)
                    {
                        for (int j = i + 1; j < grdPODetail.Rows.Count; j++)
                        {

                            if (grdPODetail.Rows[i].Cells[1].Value.ToString() == grdPODetail.Rows[j].Cells["Item"].Value.ToString() &&
                                grdPODetail.Rows[i].Cells["ModelNo"].Value.ToString() == grdPODetail.Rows[j].Cells["ModelNo"].Value.ToString())
                            {
                                //duplicate item code
                                MessageBox.Show("Item code duplicated in OB details... please ensure correctness (" +
                                    grdPODetail.Rows[i].Cells["Item"].Value.ToString() + ")");
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

        private List<podetail> getPODetails(poheader poh)
        {
            List<podetail> PODetails = new List<podetail>();
            try
            {
                podetail pod = new podetail();
                for (int i = 0; i < grdPODetail.Rows.Count; i++)
                {
                    pod = new podetail();
                    pod.DocumentID = poh.DocumentID;
                    pod.TemporaryNo = poh.TemporaryNo;
                    pod.TemporaryDate = poh.TemporaryDate;
                    pod.StockItemID = grdPODetail.Rows[i].Cells["Item"].Value.ToString().Trim().Substring(0, grdPODetail.Rows[i].Cells["Item"].Value.ToString().Trim().IndexOf('-'));
                    pod.ModelNo = grdPODetail.Rows[i].Cells["ModelNo"].Value.ToString().Trim();
                    pod.Description = grdPODetail.Rows[i].Cells["Description"].Value.ToString().Trim();
                    pod.TaxCode = grdPODetail.Rows[i].Cells["gTCode"].Value.ToString().Trim();
                    if (grdPODetail.Rows[i].Cells["Unit"].Value != null)
                    {
                        pod.Unit = grdPODetail.Rows[i].Cells["Unit"].Value.ToString().Trim();
                    }
                    else
                    {
                        pod.Unit = "";
                    }
                    pod.Quantity = Convert.ToDouble(grdPODetail.Rows[i].Cells["Quantity"].Value);
                    pod.Price = Convert.ToDouble(grdPODetail.Rows[i].Cells["Price"].Value);
                    pod.Tax = Convert.ToDouble(grdPODetail.Rows[i].Cells["Tax"].Value);
                    pod.WarrantyDays = Convert.ToInt32(grdPODetail.Rows[i].Cells["WarrantyDays"].Value);
                    pod.TaxDetails = grdPODetail.Rows[i].Cells["TaxDetails"].Value.ToString();
                    PODetails.Add(pod);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("getPODetails() : Error getting Purchase Order Details");
                PODetails = null;
            }
            return PODetails;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            Boolean status = true;
            try
            {

                PurchaseOrderDB podb = new PurchaseOrderDB();
                poheader poh = new poheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    if (!verifyAndReworkPODetailGridRows())
                    {
                        return;
                    }
                    poh.DocumentID = docID;
                    poh.PODate = dtPODate.Value;
                    poh.CustomerID = txtCustomer.Text.ToString().Trim().Substring(0, txtCustomer.Text.ToString().Trim().IndexOf('-')).Trim();      
                    poh.CurrencyID = ((Structures.ComboBoxItem)cmbCurrency.SelectedItem).HiddenValue;
                    poh.ExchangeRate = Convert.ToDecimal(txtExchangeRate.Text);
                    poh.DeliveryPeriod = Convert.ToInt32(txtDeliveryPeriod.Text);
                    poh.TaxTerms = ((Structures.ComboBoxItem)cmbTaxTerms.SelectedItem).HiddenValue;
                  
                    poh.PaymentTerms = txtPaymentTerms.Text.ToString();
                    poh.PriceBasis = ((Structures.ComboBoxItem)cmbpricebasis.SelectedItem).HiddenValue;
                    poh.DeliveryAt = txtDelvAt.Text.Trim().Replace("'", "''");
                    poh.FreightTerms = ((Structures.ComboBoxItem)cmbFreight.SelectedItem).HiddenValue;
                    //poh.FreightTerms = cmbFreight.SelectedItem.ToString().Trim().Substring(0, cmbFreight.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
                    poh.ReferenceIndent = txtReferenceIndent.Text.ToString();
                    poh.ReferenceQuotation = txtReferenceQuotation.Text.ToString();
                    poh.TermsAndCondition = txtTermsAndCondition.Text.ToString();
                    //qih.DocumentNo = Convert.ToInt32(txtDocumentNo.Text);
                    poh.DeliveryAddress = txtDeliveryAddress.Text;
                    poh.ModeOfPayment = ((Structures.ComboBoxItem)cmbPaymentMode.SelectedItem).HiddenValue;
                    poh.TransportationMode = ((Structures.ComboBoxItem)cmbTransMode.SelectedItem).HiddenValue;
                    //poh.ModeOfPayment = cmbPaymentMode.SelectedItem.ToString().Trim().Substring(0, cmbPaymentMode.SelectedItem.ToString().Trim().IndexOf('-')).Trim();

                    poh.PackingSpec = txtPackingSpec.Text.Trim().Replace("'","''");
                    poh.PartialShipment = cmbPartialShipment.SelectedItem.ToString();
                    poh.Transhipment = cmbTranshipment.SelectedItem.ToString();
                    poh.ProductValue = Convert.ToDouble(txtProductValue.Text);
                    poh.ProductValueINR = Convert.ToDouble(txtProductValueINR.Text);
                    poh.DeliveryPeriod = Convert.ToInt32(txtDeliveryPeriod.Text);
                    poh.validityPeriod = Convert.ToInt32(txtValidityPeriod.Text);
                    poh.TaxAmount = Convert.ToDouble(txtPOTax.Text);
                    poh.TaxAmountINR = Convert.ToDouble(txtTaxAmountINR.Text);
                    poh.POValue = Convert.ToDouble(txtPOValue.Text);
                    poh.POValueINR = Convert.ToDouble(txtPOValueINR.Text);
                    poh.SpecialNote = txtSpecialNote.Text.Trim();
                    poh.Remarks = txtRemarks.Text;
                    poh.FreightCharge = Convert.ToDouble(txtFreightCharge.Text);
                    poh.Comments = docCmtrDB.DGVtoString(dgvComments).Replace("'","''");
                    poh.ForwarderList = prevpoh.ForwarderList;

                    if (poh.SpecialNote.Trim().Length > 1000)
                    {
                        MessageBox.Show("Special note character length should not more than 1000");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (!podb.validatePurchaseOrderHeader(poh))
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    //poh.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    poh.DocumentStatus = 1; //created
                    poh.TemporaryDate = UpdateTable.getSQLDateTime();
                }

                else
                {
                    poh.TemporaryNo = Convert.ToInt32(txtTemporaryNo.Text);
                    poh.TemporaryDate = prevpoh.TemporaryDate;
                    poh.DocumentStatus = prevpoh.DocumentStatus;
                }
                //Replacing single quotes
                poh = verifyHeaderInputString(poh);
                verifyDetailInputString();

                if (podb.validatePurchaseOrderHeader(poh))
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
                            poh.CommentStatus = docCmtrDB.createCommentStatusString(prevpoh.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            poh.CommentStatus = prevpoh.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            poh.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            poh.CommentStatus = prevpoh.CommentStatus;
                        }
                    }
                    //----------
                    int tmpStatus = 1;
                    if (txtComments.Text.Trim().Length > 0)
                    {
                        poh.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus).Replace("'", "''");
                    }
                    List<podetail> PODetails = getPODetails(poh); ;
                    if (btnText.Equals("Update"))
                    {
                        if (podb.updatePOHeaderAndDetail(poh, prevpoh, PODetails))
                        {
                            MessageBox.Show("Purchase Order details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredPOHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update Purchase Order Header");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (podb.InsertPOHeaderAndDetail(poh, PODetails))
                        {
                            MessageBox.Show("Purchase Order Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredPOHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert Purchase Order Header");
                        }
                    }
                }
                else
                {
                    status = false;
                    MessageBox.Show("Purchase Order Header Validation failed");
                }
            }
            catch (Exception ex)
            {
                status = false;
                MessageBox.Show("createAndUpdatePODetails() : Error");
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }
        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            txtSearchPu.Text = "";
            try
            {
                if (e.RowIndex < 0)
                    return;
                grdList.Rows[e.RowIndex].Selected = true;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("gEdit") || columnName.Equals("gApprove") || columnName.Equals("gLoadDocument") ||
                    columnName.Equals("gView") || columnName.Equals("gPrint"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    AddRowClick = false;
                    colClicked = columnName;
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    PurchaseOrderDB wodb = new PurchaseOrderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevpoh = new poheader();
                    prevpoh.CommentStatus = grdList.Rows[e.RowIndex].Cells["ComStatus"].Value.ToString();
                    prevpoh.DocumentID = grdList.Rows[e.RowIndex].Cells["Did"].Value.ToString();
                    prevpoh.DocumentName = grdList.Rows[e.RowIndex].Cells["Dname"].Value.ToString();
                    prevpoh.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["TempNo"].Value.ToString());
                    prevpoh.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["TempDate"].Value.ToString());

                    if (prevpoh.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    prevpoh.Comments = PurchaseOrderDB.getUserComments(prevpoh.DocumentID, prevpoh.TemporaryNo, prevpoh.TemporaryDate);

                    prevpoh.PONo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["PONo"].Value.ToString());
                    prevpoh.PODate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["PODate"].Value.ToString());
                    prevpoh.ReferenceIndent = grdList.Rows[e.RowIndex].Cells["RefIndent"].Value.ToString();
                    prevpoh.ReferenceQuotation = grdList.Rows[e.RowIndex].Cells["RefQuot"].Value.ToString();

                    prevpoh.CustomerID = grdList.Rows[e.RowIndex].Cells["CustID"].Value.ToString();
                    prevpoh.CustomerName = grdList.Rows[e.RowIndex].Cells["CustName"].Value.ToString();
                    //--------Load Documents
                    if (columnName.Equals("gLoadDocument"))
                    {
                        string hdrString = "Document Temp No:" + prevpoh.TemporaryNo + "\n" +
                            "Document Temp Date:" + prevpoh.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "PO No:" + prevpoh.PONo + "\n" +
                            "PO Date:" + prevpoh.PODate.ToString("dd-MM-yyyy") + "\n" +
                            "Customer:" + prevpoh.CustomerName;
                        //poheader po =PurchaseOrderDB.getPODetailForMR(prevpoh.PONo, prevpoh.PODate);
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevpoh.TemporaryNo + "-" + prevpoh.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_2(null, null);
                        return;
                    }
                    //--------
                    prevpoh.CountryID = grdList.Rows[e.RowIndex].Cells["Country"].Value.ToString();
                    prevpoh.CurrencyID = grdList.Rows[e.RowIndex].Cells["CurID"].Value.ToString();
                    prevpoh.ExchangeRate = Convert.ToDecimal(grdList.Rows[e.RowIndex].Cells["ExchangeRate"].Value.ToString());
                    prevpoh.DeliveryPeriod = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DelvPeriod"].Value.ToString());
                    prevpoh.validityPeriod = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["ValidityPeriod"].Value.ToString());
                    prevpoh.TaxTerms = grdList.Rows[e.RowIndex].Cells["TaxTerm"].Value.ToString();
                    //prevpoh.TaxCode = grdList.Rows[e.RowIndex].Cells["TCode"].Value.ToString();
                    prevpoh.PaymentTerms = grdList.Rows[e.RowIndex].Cells["PaymentTerm"].Value.ToString();
                    prevpoh.ModeOfPayment = grdList.Rows[e.RowIndex].Cells["ModeOfPayment"].Value.ToString();
                    prevpoh.TransportationMode = grdList.Rows[e.RowIndex].Cells["TransportationMode"].Value.ToString();
                    //prevpoh.CreditPeriod = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gCreditPeriod"].Value.ToString());
                    prevpoh.FreightTerms = grdList.Rows[e.RowIndex].Cells["FreightTerm"].Value.ToString();
                    prevpoh.FreightCharge = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["FreightCharge"].Value.ToString());

                    prevpoh.DeliveryAddress = grdList.Rows[e.RowIndex].Cells["DelvAddress"].Value.ToString();
                    prevpoh.ProductValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["ProdValue"].Value.ToString());
                    prevpoh.ProductValueINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["ProductValueINR"].Value.ToString());
                    prevpoh.TaxAmount = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["TaxAmount"].Value.ToString());
                    prevpoh.TaxAmountINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["TaxAmountINR"].Value.ToString());
                    prevpoh.POValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["POValue"].Value.ToString());
                    prevpoh.POValueINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["POValueINR"].Value.ToString());

                    prevpoh.Transhipment = grdList.Rows[e.RowIndex].Cells["Transhipment"].Value.ToString();
                    prevpoh.PartialShipment = grdList.Rows[e.RowIndex].Cells["PartialShipment"].Value.ToString();
                    prevpoh.PackingSpec = grdList.Rows[e.RowIndex].Cells["PackingSpec"].Value.ToString();
                    prevpoh.PriceBasis = grdList.Rows[e.RowIndex].Cells["PriceBasis"].Value.ToString();
                    prevpoh.DeliveryAt = grdList.Rows[e.RowIndex].Cells["DeliveryAt"].Value.ToString();

                    prevpoh.Remarks = grdList.Rows[e.RowIndex].Cells["Remark"].Value.ToString();
                    prevpoh.TermsAndCondition = grdList.Rows[e.RowIndex].Cells["TermsAndCondition"].Value.ToString();
                    prevpoh.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString());
                    prevpoh.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentStatus"].Value.ToString());
                    prevpoh.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["CreateTime"].Value.ToString());
                    prevpoh.CreateUser = grdList.Rows[e.RowIndex].Cells["CreateUser"].Value.ToString();
                    prevpoh.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    prevpoh.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevpoh.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    prevpoh.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    prevpoh.SpecialNote = grdList.Rows[e.RowIndex].Cells["SpecialNote"].Value.ToString().Trim();
                    //cmbTaxCode.SelectedIndex = cmbTaxCode.FindString(prevpoh.TaxCode);

                    //--comments
                    ///chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    prevpoh.CommentStatus = grdList.Rows[e.RowIndex].Cells["ComStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(prevpoh.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(prevpoh.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---

                    txtCustomer.Text = prevpoh.CustomerID + "-" + Environment.NewLine + prevpoh.CustomerName;
                    cmbCurrency.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCurrency, prevpoh.CurrencyID);
                    cmbPaymentMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbPaymentMode, prevpoh.ModeOfPayment);
                    //cmbPaymentMode.SelectedIndex = cmbPaymentMode.FindString(prevpoh.ModeOfPayment);
                    //cmbFreight.SelectedIndex = cmbFreight.FindString(prevpoh.FreightTerms);
                    cmbFreight.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbFreight, prevpoh.FreightTerms);
                    cmbTaxTerms.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbTaxTerms, prevpoh.TaxTerms);
                    cmbTransMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbTransMode, prevpoh.TransportationMode);
                    ///cmbpricebasis.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbpricebasis, prevpoh.PriceBasis);
                    txtDelvAt.Text = prevpoh.DeliveryAt;
                    if (prevpoh.PriceBasis == null || prevpoh.PriceBasis.Trim().Length == 0)
                        cmbpricebasis.SelectedIndex = 0;
                    else
                        cmbpricebasis.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbpricebasis, prevpoh.PriceBasis);
                    if (prevpoh.Transhipment == null || prevpoh.Transhipment.Trim().Length == 0)   
                        cmbTranshipment.SelectedIndex = cmbTranshipment.FindString("No");
                    else
                        cmbTranshipment.SelectedIndex = cmbTranshipment.FindString(prevpoh.Transhipment);

                    if (prevpoh.PartialShipment == null || prevpoh.PartialShipment.Trim().Length == 0)   
                        cmbPartialShipment.SelectedIndex = cmbPartialShipment.FindString("No"); 
                    else
                        cmbPartialShipment.SelectedIndex = cmbPartialShipment.FindString(prevpoh.PartialShipment);
                    txtPackingSpec.Text = prevpoh.PackingSpec;

                    txtSpecialNote.Text = prevpoh.SpecialNote.ToString().Trim();
                    txtExchangeRate.Text = prevpoh.ExchangeRate.ToString();
                    txtPOValueINR.Text = prevpoh.POValueINR.ToString();
                    txtPOValue.Text = prevpoh.POValue.ToString();
                    txtProductValue.Text = prevpoh.ProductValue.ToString();
                    txtProductValueINR.Text = prevpoh.ProductValueINR.ToString();
                    txtPOTax.Text = prevpoh.TaxAmount.ToString();
                    txtTaxAmountINR.Text = prevpoh.TaxAmountINR.ToString();

                    txtPaymentTerms.Text = prevpoh.PaymentTerms.ToString();
                    txtTemporaryNo.Text = prevpoh.TemporaryNo.ToString();
                    try
                    {
                        dtTemporaryDate.Value = prevpoh.TemporaryDate;
                    }
                    catch (Exception)
                    {

                        dtTemporaryDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtPONo.Text = prevpoh.PONo.ToString();
                    try
                    {
                        dtPODate.Value = prevpoh.PODate;
                    }
                    catch (Exception)
                    {
                        dtPODate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtDeliveryAddress.Text = prevpoh.DeliveryAddress;
                    //txtCreditPeriod.Text = prevpoh.CreditPeriod.ToString();
                    txtDeliveryPeriod.Text = prevpoh.DeliveryPeriod.ToString();
                    txtValidityPeriod.Text = prevpoh.validityPeriod.ToString();
                    txtFreightCharge.Text = prevpoh.FreightCharge.ToString();
                    txtReferenceIndent.Text = prevpoh.ReferenceIndent;
                    txtReferenceQuotation.Text = prevpoh.ReferenceQuotation;
                    txtTermsAndCondition.Text = prevpoh.TermsAndCondition;
                    btnProductValue.Text = prevpoh.ProductValue.ToString();
                    btnTaxAmount.Text = prevpoh.TaxAmount.ToString();
                    // btnPOValue.Text = prevpoh.POValue.ToString();
                    txtRemarks.Text = prevpoh.Remarks;

                    List<podetail> PODetail = PurchaseOrderDB.getPurchaseOrderDetails(prevpoh);
                    grdPODetail.Rows.Clear();
                    int i = 0;
                    foreach (podetail pod in PODetail)
                    {
                        if (!AddPODetailRow())
                        {
                            MessageBox.Show("Error found in Purchase Order Detail. Please correct before updating the details");
                        }
                        else
                        {
                            //grdWODetail.Rows[i].Cells["Item"].Value = qid.StockItemID + "-" + qid.StockItemName;
                            grdPODetail.Rows[i].Cells["Item"].Value = pod.StockItemID + "-" + pod.StockItemName;
                            grdPODetail.Rows[i].Cells["ModelNo"].Value = pod.ModelNo;
                            grdPODetail.Rows[i].Cells["ModelName"].Value = pod.ModelName;
                            grdPODetail.Rows[i].Cells["Description"].Value = pod.Description;
                            grdPODetail.Rows[i].Cells["gTCode"].Value = pod.TaxCode;
                            grdPODetail.Rows[i].Cells["Unit"].Value = pod.Unit;
                            grdPODetail.Rows[i].Cells["Quantity"].Value = pod.Quantity;
                            grdPODetail.Rows[i].Cells["Price"].Value = pod.Price;
                            grdPODetail.Rows[i].Cells["Value"].Value = pod.Quantity * pod.Price;
                            grdPODetail.Rows[i].Cells["Tax"].Value = pod.Tax;
                            grdPODetail.Rows[i].Cells["WarrantyDays"].Value = pod.WarrantyDays;
                            grdPODetail.Rows[i].Cells["TaxDetails"].Value = pod.TaxDetails;
                            i++;
                            productvalue = productvalue + pod.Quantity * pod.Price;
                            taxvalue = taxvalue + pod.Tax;
                        }

                    }
                    if (columnName.Equals("gPrint"))
                    {
                        //PrintPurchaseOrder ppo = new PrintPurchaseOrder();
                        pnlAddEdit.Visible = false;
                        pnlList.Visible = true;
                        grdList.Visible = true;
                        btnNew.Visible = true;
                        btnExit.Visible = true;
                        //CSLERP.PrintForms.PrintPurchaseOrder ppo = new CSLERP.PrintForms.PrintPurchaseOrder();
                        PrintPurchaseOrder ppo = new PrintPurchaseOrder();
                        poheader poh = new poheader();
                        List<podetail> PODetails = PurchaseOrderDB.getPurchaseOrderDetails(prevpoh);
                        getTaxDetails(PODetails);
                        string taxstr = getTasDetailStr();
                        ppo.PrintPO(prevpoh, PODetails, taxstr);
                        btnNew.Visible = true;
                        btnExit.Visible = true;
                        return;
                    }
                    if (columnName == "gEdit" || columnName == "gView")
                    {
                        //if(columnName == "gView")
                        //{
                        //    tabControl1.TabPages["tabIndentDetail"].Enabled = true;
                           
                        //    tabControl1.TabPages["tabPODetail"].Enabled = true;
                         
                        //}
                        string refIOStr = prevpoh.ReferenceIndent;
                        getIndentDetails(refIOStr);
                    }
                    if (!verifyAndReworkPODetailGridRows())
                    {
                        MessageBox.Show("Error found in Purchase Order details. Please correct before updating the details");
                    }
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                    tabControl1.SelectedTab = tabPOHeader;
                    tabControl1.Visible = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                setButtonVisibility("init");
            }
        }
        private void getIndentDetails(string refINStr)
        {
            try
            {
                grdIndentDetail.Rows.Clear();
                grdItemWiseTotalQuant.Rows.Clear();
                tabControl1.TabPages["tabIndentDetail"].Visible = true;
                tabControl2.SelectedTab = tabControl2.TabPages["tabDetail"];
                grdIndentDetail.Visible = true;
                grdItemWiseTotalQuant.Visible = true;
                dictItemWiseTOt = new Dictionary<string, string[]>();
                string[] substrRef = refINStr.Trim().Split(';');
                foreach (string indstr in substrRef)
                {
                    string subustr = indstr.Trim();
                    if (subustr.Length != 0)
                    {
                        string docid = subustr.Trim().Substring(0, subustr.Trim().IndexOf('('));


                        //int INNo = Convert.ToInt32(iostr.Trim().Substring(0, iostr.IndexOf('(')));
                        int findex = subustr.IndexOf('(');
                        int sindex = subustr.IndexOf(')'); 
                        string tstr = subustr.Substring(findex + 1, (sindex - findex) - 1).Trim();
                        string[] indentNoArr = tstr.Split(Main.delimiter1);
                        DateTime INDate = Convert.ToDateTime(indentNoArr[1]);
                        int INNo = Convert.ToInt32(indentNoArr[0]);
                        List<indentdetail> IndentListTemp = new List<indentdetail>();
                        List<indentgeneraldetail> IndentGenList = new List<indentgeneraldetail>();
                        if (docid == "INDENTGENERAL")
                        {
                            IndentGenList = IndentGeneralDB.getIndentGeneralDetailsForPO(docid,INNo,INDate);
                            addGridIndentDetailForGeneral(IndentGenList, INNo, INDate);
                        }
                        else
                        {
                            IndentListTemp = IndentHeaderDB.getIndentDetailForPO(INNo, INDate, docid);
                            addGridIndentDetail(IndentListTemp, INNo, INDate);
                        }
                        //List<indentdetail> IndentListTemp = IndentHeaderDB.getIndentDetailForPO(INNo, INDate, docid);
                        
                    }
                }
                int j = 0;
                foreach (KeyValuePair<string, string[]> kvp in dictItemWiseTOt)
                {
                    grdItemWiseTotalQuant.Rows.Add();
                    grdItemWiseTotalQuant.Rows[j].Cells["ItemID"].Value = kvp.Key;
                    grdItemWiseTotalQuant.Rows[j].Cells["StockItemName"].Value = kvp.Value[0];
                    grdItemWiseTotalQuant.Rows[j].Cells["TotalQuantity"].Value = kvp.Value[1];
                    j++;
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void addGridIndentDetailForGeneral(List<indentgeneraldetail> IndentList, int DocNo, DateTime DocDATE)
        {
            try
            {
                int i = grdIndentDetail.Rows.Count;
                foreach (indentgeneraldetail ind in IndentList)
                {
                    grdIndentDetail.Rows.Add();
                    grdIndentDetail.Rows[i].Cells["gDocID"].Value = ind.DocumentName;
                    grdIndentDetail.Rows[i].Cells["gIndentNo"].Value = DocNo.ToString();
                    grdIndentDetail.Rows[i].Cells["gIndentDate"].Value = DocDATE.ToString("yyyy-MM-dd");
                    grdIndentDetail.Rows[i].Cells["gItemID"].Value = "";
                    grdIndentDetail.Rows[i].Cells["gItemName"].Value = "";
                    grdIndentDetail.Rows[i].Cells["gDescription"].Value = ind.ItemDetail;
                    grdIndentDetail.Rows[i].Cells["gQuantity"].Value = ind.Quantity;
                    grdIndentDetail.Rows[i].Cells["gWarranty"].Value = ind.WarrantyDays;
                    grdIndentDetail.Rows[i].Cells["gApproverName"].Value = ind.Approver;// ind.ModelName;   //Approver Name
                    i++;
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void addGridIndentDetail(List<indentdetail> IndentList, int INNo, DateTime INdate)
        {
            try
            {
                int i = grdIndentDetail.Rows.Count;
                foreach (indentdetail ind in IndentList)
                {
                    grdIndentDetail.Rows.Add();
                    grdIndentDetail.Rows[i].Cells["gDocID"].Value = ind.DocumentName;
                    grdIndentDetail.Rows[i].Cells["gIndentNo"].Value = INNo.ToString();
                    grdIndentDetail.Rows[i].Cells["gIndentDate"].Value = INdate.ToString("yyyy-MM-dd");
                    grdIndentDetail.Rows[i].Cells["gItemID"].Value = ind.StockItemID;
                    grdIndentDetail.Rows[i].Cells["gItemName"].Value = ind.StockItemName;
                    grdIndentDetail.Rows[i].Cells["gDescription"].Value = ind.ModelDetails;
                    grdIndentDetail.Rows[i].Cells["gQuantity"].Value = ind.Quantity;
                    grdIndentDetail.Rows[i].Cells["gWarranty"].Value = ind.WarrantyDays;
                    grdIndentDetail.Rows[i].Cells["gApproverName"].Value = ind.ModelName;   //Approver Name
                    if (dictItemWiseTOt.ContainsKey(ind.StockItemID))
                    {
                        double quant = Convert.ToDouble(dictItemWiseTOt[ind.StockItemID][1]);
                        double tot = quant + ind.Quantity;
                        dictItemWiseTOt[ind.StockItemID] = new string[] { ind.StockItemName, tot.ToString() };
                    }
                    else
                    {
                        dictItemWiseTOt.Add(ind.StockItemID, new string[] { ind.StockItemName, ind.Quantity.ToString() });
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void pnlUI_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCancel_Click_2(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }

        private void btnAddNewLine_Click(object sender, EventArgs e)
        {
            AddRowClick = true;
            AddPODetailRow();
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
           
            if (txtExchangeRate.Text.Length == 0)
            {
                MessageBox.Show("Fill exchange rate");
                return;
            }
            verifyAndReworkPODetailGridRows();
        }

        private void ClearEntries_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult dialog = MessageBox.Show("Are you sure to clear all entries in grid detail ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdPODetail.Rows.Clear();
                    MessageBox.Show("item cleared.");
                    verifyAndReworkPODetailGridRows();
                }
               
            }
            catch (Exception)
            {
            }
           
        }

        private void grdQIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdPODetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdPODetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkPODetailGridRows();
                    }
                    if (columnName.Equals("TaxView"))
                    {
                        //show tax details
                        DialogResult dialog = MessageBox.Show(grdPODetail.Rows[e.RowIndex].Cells["TaxDetails"].Value.ToString(),
                            "Line No : " + (e.RowIndex + 1), MessageBoxButtons.OK);
                    }
                    if (columnName.Equals("Sel"))
                    {
                        showStockDataGridView();
                    }
                    if (columnName.Equals("SelDesc"))
                    {
                        descClickRowIndex = e.RowIndex;
                        string strTest = grdPODetail.Rows[e.RowIndex].Cells["Description"].Value.ToString().Trim();
                        showPopUpForDescription(strTest);
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
        //----
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
                grdPODetail.Rows[descClickRowIndex].Cells["Description"].Value = txtDesc.Text.Trim();     
                grdPODetail.FirstDisplayedScrollingRowIndex = grdPODetail.Rows[descClickRowIndex].Index;
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
        //--------------
        private Boolean checkAvailabilityOfitem()
        {
            Boolean status = true;
            if (grdPODetail.CurrentRow.Cells["Item"].Value.ToString().Length != 0 ||
                grdPODetail.CurrentRow.Cells["ModelNo"].Value.ToString().Length != 0 ||
                grdPODetail.CurrentRow.Cells["ModelName"].Value.ToString().Length != 0)
            {
                status = false;
            }
            return status;
        }
        //showing gridview for stockitem
        private void showStockDataGridView()
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

                frmPopup.Size = new Size(900, 370);

                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(550, 5);
                lblSearch.AutoSize = true;
                lblSearch.Text = "Search by Name";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Size = new Size(200, 18);
                txtSearch.Location = new System.Drawing.Point(680, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInEmpGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

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
                grdPODetail.CurrentRow.Cells["Item"].Value = iolist;
                ///showModelListView(iolist);
                frmPopup.Close();
                frmPopup.Dispose();
                showModelListView(iolist);
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
            filterGridData();
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }

        private void filterGridData()
        {
            try
            {
                grdStock.CurrentCell = null;
                foreach (DataGridViewRow row in grdStock.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdStock.Rows)
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

        //---------------
        //private void showStockItemTreeView()
        //{
        //    removeControlsFromForwarderPanelTV();
        //    if (!checkAvailabilityOfitem())
        //    {
        //        DialogResult dialog = MessageBox.Show("Selected product and Model detail will removed?", "Yes", MessageBoxButtons.YesNo);
        //        if (dialog == DialogResult.Yes)
        //        {
        //            grdPODetail.CurrentRow.Cells["Item"].Value = "";
        //            grdPODetail.CurrentRow.Cells["ModelNo"].Value = "";
        //            grdPODetail.CurrentRow.Cells["ModelName"].Value = "";
        //        }
        //        else
        //            return;
        //    }
        //    tv = new TreeView();
        //    tv.CheckBoxes = true;
        //    tv.Nodes.Clear();
        //    tv.CheckBoxes = true;
        //    pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
        //    pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
        //    Label lbl = new Label();
        //    lbl.AutoSize = true;
        //    lbl.Location = new Point(70, 17);
        //    lbl.Size = new Size(35, 13);
        //    lbl.Text = "Tree View For Product";
        //    lbl.Font = new Font("Serif", 10, FontStyle.Bold);
        //    lbl.ForeColor = Color.Green;
        //    pnlForwarder.Controls.Add(lbl);
        //    tv = StockItemDB.getStockItemTreeViewNew();
        //    tv.Bounds = new Rectangle(new Point(50, 50), new Size(600, 200));
        //    pnlForwarder.Controls.Remove(tv);
        //    pnlForwarder.Controls.Add(tv);
        //    //tv.cl
        //    tv.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterCheck);
        //    Button lvForwrdOK = new Button();
        //    lvForwrdOK.Text = "OK";
        //    lvForwrdOK.Size = new Size(150, 20);
        //    lvForwrdOK.Location = new Point(50, 270);
        //    lvForwrdOK.Click += new System.EventHandler(this.tvOK_Click);
        //    pnlForwarder.Controls.Add(lvForwrdOK);

        //    Button lvForwardCancel = new Button();
        //    lvForwardCancel.Text = "Cancel";
        //    lvForwardCancel.Size = new Size(150, 20);
        //    lvForwardCancel.Location = new Point(250, 270);
        //    lvForwardCancel.Click += new System.EventHandler(this.tvCancel_Click);
        //    pnlForwarder.Controls.Add(lvForwardCancel);
        //    ////lvForwardCancel.Visible = false;
        //    //tv.CheckBoxes = true;
        //    pnlForwarder.Visible = true;
        //    pnlAddEdit.Controls.Add(pnlForwarder);
        //    pnlAddEdit.BringToFront();
        //    pnlForwarder.BringToFront();
        //    pnlForwarder.Focus();
        //}
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
        //            grdPODetail.CurrentRow.Cells["Item"].Value = s;
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
        //                //nodeList.Add(childNode.Text);
        //                nodeList.Add(childNode.Name + "-" + childNode.Text);
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
                grdPODetail.CurrentRow.Cells["ModelNo"].Value = "NA";
                grdPODetail.CurrentRow.Cells["ModelName"].Value = "NA";
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
            lvOK.Click += new System.EventHandler(this.lvOK_Click2);
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
                        grdPODetail.CurrentRow.Cells["ModelNo"].Value = item.SubItems[1].Text;
                        grdPODetail.CurrentRow.Cells["ModelName"].Value = item.SubItems[2].Text;
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
        private void btnTaxDetail_Click(object sender, EventArgs e)
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
        private string getTasDetailStr()
        {
            string strTax = "";
            //verifyAndReworkPODetailGridRows();
            for (int i = 0; i < (TaxDetailsTable.Rows.Count); i++)
            {
                strTax = strTax + Convert.ToString(TaxDetailsTable.Rows[i][0]) + Main.delimiter1 +
                Convert.ToString(TaxDetailsTable.Rows[i][1]) + Main.delimiter2;
            }
            return strTax;
        }
        private void pnlAddEdit_Paint(object sender, PaintEventArgs e)
        {

        }
        private void cmbTaxTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string TaxTrm = ((Structures.ComboBoxItem)cmbTaxTerms.SelectedItem).HiddenValue;
            //if (TaxTrm.Trim().Equals("TaxSeperate"))
            //{
            //    cmbTaxCode.Enabled = true;
            //}
            //else
            //{
            //    cmbTaxCode.Text = "NoTax";
            //    cmbTaxCode.Enabled = false;
            //}
        }

        private void cmbFreight_SelectedIndexChanged(object sender, EventArgs e)
        {
            string freight = ((Structures.ComboBoxItem)cmbFreight.SelectedItem).HiddenValue;
            if (freight.Trim().Equals("Extra"))
            {
                txtFreightCharge.Enabled = true;
            }
            else
            {
                txtFreightCharge.Text = "0";
                txtFreightCharge.Enabled = false;
            }
        }

        private void btnApproved_Click(object sender, EventArgs e)
        {
            txtSearchPu.Text = "";
            if (getuserPrivilegeStatus() == 2)
            {
                listOption = 6; //viewer
            }
            else
            {
                listOption = 3;
            }

            ListFilteredPOHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            txtSearchPu.Text = "";
            listOption = 2;
            ListFilteredPOHeader(listOption);
        }

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            txtSearchPu.Text = "";
            listOption = 1;
            ListFilteredPOHeader(listOption);
        }
        private Boolean updateDashBoard(poheader poh, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = poh.DocumentID;
                dsb.TemporaryNo = poh.TemporaryNo;
                dsb.TemporaryDate = poh.TemporaryDate;
                dsb.DocumentNo = poh.PONo;
                dsb.DocumentDate = poh.PODate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = poh.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(prevpoh.DocumentID);
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
        private void btnForward_Click(object sender, EventArgs e)
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
            ////frmPopup.Visible = false;
            frmPopup.ShowDialog();
            //pnlForwarder.Visible = true;
            //pnlAddEdit.Controls.Add(pnlForwarder);
            //pnlAddEdit.BringToFront();
            //pnlForwarder.BringToFront();
            //pnlForwarder.Focus();
        }
        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //customer cust = new customer();
                //////////string custid = cmbCustomer.SelectedItem.ToString().Trim().Substring(0, cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-'));
                //string custid = ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue;
                //cust = CustomerDB.getCustomerDetails(custid);
                //txtDeliveryAddress.Text = cust.DeliveryAddress.ToString();
                //txtDeliveryAddress.Text = "";
                txtReferenceQuotation.Clear();
                //MessageBox.Show("enter quotation");
            }
            catch (Exception ex)
            {

            }
        }
        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                PurchaseOrderDB podb = new PurchaseOrderDB();
                // poheader poh = new poheader();
                FinancialLimitDB flDB = new FinancialLimitDB();
                if (!flDB.checkEmployeeFinancialLimit(docID, Login.empLoggedIn, Convert.ToDouble(txtProductValueINR.Text), 0))
                {
                    MessageBox.Show("No financial power for approving this document");
                    return;
                }
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the PO ?", "Yes", MessageBoxButtons.YesNo);

                if (dialog == DialogResult.Yes)
                {
                    prevpoh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevpoh.CommentStatus);
                    if (prevpoh.Status != 96)
                    {
                        prevpoh.PONo = DocumentNumberDB.getNewNumber(docID, 2);
                    }
                    //prevpoh.PONo = DocumentNumberDB.getNewNumber(docID, 2);
                    if (podb.ApprovePurchaseOrder(prevpoh))
                    {
                        MessageBox.Show("Purchase Order Document Approved");
                        if (!updateDashBoard(prevpoh, 2))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                        }
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredPOHeader(listOption);
                    }
                }
            }
            catch (Exception)
            {
            }
        }
 
        private void btnReferenceIndent_Click(object sender, EventArgs e)
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(600, 300);
            lv = IndentHeaderDB.ReferenceIndentHeaderSelectionView();
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(600, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Clicked);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Clicked);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
        }
        private void lvOK_Clicked(object sender, EventArgs e)
        {
            try
            {
                
                string ihlist = "";
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        ihlist = ihlist + itemRow.SubItems[1].Text +
                            "(" + itemRow.SubItems[3].Text + Main.delimiter1 + itemRow.SubItems[4].Text + ");" + Environment.NewLine;
                    }
                }
                txtReferenceIndent.Text = ihlist;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_Clicked(object sender, EventArgs e)
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

        private void btnReferenceQuotation_Click(object sender, EventArgs e)
        {
            //btnReferenceQuotation.Enabled = false;
            //removeControlsFromPnllvPanel();
            string cid;
            try
            {
                ////////cid = cmbCustomer.SelectedItem.ToString().Trim().Substring(0, cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-'));
                cid = txtCustomer.Text.ToString().Trim().Substring(0, txtCustomer.Text.ToString().Trim().IndexOf('-')).Trim();
            }
            catch (Exception ex)
            {
                cid = "";
            }
            if (cid.Trim().Length == 0 || cid == null)
            {
                MessageBox.Show("select The Customer.");
                //btnReferenceQuotation.Enabled = true;
                return;
            }

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

            frmPopup.Size = new Size(450, 300);
            lv = QIHeaderDB.ReferenceQuotationSelctionView(cid);
            string[] strArry = txtReferenceQuotation.Text.Split(new string[] { ";" }, StringSplitOptions.None);
            for (int i = 0; i < strArry.Length; i++)
            {
                try
                {
                    string refq = strArry[i];
                    int slen = strArry[i].IndexOf('(');
                    string qno = strArry[i].Substring(0, slen);
                    string qdate = strArry[i].Substring(slen + 1, strArry[i].Length - (slen + 1)).Replace(")", "");
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if ((itemRow.SubItems[2].Text.Trim().Equals(qno.Trim())) &&
                            (itemRow.SubItems[3].Text.Trim().Equals(qdate.Trim())))
                        {
                            itemRow.Checked = true;
                        }

                    }
                }
                catch (Exception ex)
                {
                }
            }
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Clicked1);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Clicked1);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
            //pnlAddEdit.Controls.Add(pnllv);
            //pnllv.BringToFront();
            //pnllv.Visible = true;
        }
        private void lvOK_Clicked1(object sender, EventArgs e)
        {
            try
            {
                
                string qhlist = "";
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        qhlist = qhlist + itemRow.SubItems[2].Text + "(" + itemRow.SubItems[3].Text + ");";
                    }
                }
                txtReferenceQuotation.Text = qhlist;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_Clicked1(object sender, EventArgs e)
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

        private void cmbTaxCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //verifyAndReworkPODetailGridRows();
        }


        //-------
        private void btnPaymentTermSelect_Click(object sender, EventArgs e)
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
        private void dgvptOK_Click(object sender, EventArgs e)
        {
            try
            {
                int tperc = 0;
                int totperc = 0;
                int tcrdays = 0;
                int pcrdays = 0;
                int tval = 0;
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

        private void btnSelectTermsAndCondition_Click(object sender, EventArgs e)
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

                frmPopup.Size = new Size(700, 300);
                grdTC = DocTcMappingDB.getGridViewForTCMapping(docID, TCList);
                string[] strArry = txtTermsAndCondition.Text.Split(new string[] { ";" }, StringSplitOptions.None);
                for (int i = 0; i < strArry.Length; i++)
                {
                    try
                    {
                        foreach (DataGridViewRow row in grdTC.Rows)
                        {
                            if (row.Cells["ParagraphID"].Value.ToString().Equals(strArry[i]))
                            {
                                row.Cells["Select"].Value = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                foreach (DataGridViewRow row in grdTC.Rows)
                {
                    row.Cells[3].ToolTipText = "Double click to see detail";
                }
                grdTC.Bounds = new Rectangle(new Point(0, 0), new Size(700, 250));
                grdTC.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdTC_CellDoubleClick);
                frmPopup.Controls.Add(grdTC);

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new Point(40, 265);
                lvOK.Click += new System.EventHandler(this.lvOK_Clicked2);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.BackColor = Color.Tan;
                lvCancel.Text = "CANCEL";
                lvCancel.Location = new Point(130, 265);
                lvCancel.Click += new System.EventHandler(this.lvCancel_Clicked2);
                frmPopup.Controls.Add(lvCancel);

                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void lvOK_Clicked2(object sender, EventArgs e)
        {
            try
            {
                var checkedRows = from DataGridViewRow r in grdTC.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount == 0)
                {
                    MessageBox.Show("Select one Terms and condition.");
                    return;
                }
                string tclist = "";
                foreach (var row in checkedRows)
                {
                    tclist = tclist + row.Cells["ParagraphID"].Value.ToString() + ";";
                }
                txtTermsAndCondition.Text = tclist;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_Clicked2(object sender, EventArgs e)
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
        private void grdTC_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if(e.ColumnIndex == 3)
                {
                    string TC = grdTC.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    string TCHead = grdTC.Rows[e.RowIndex].Cells[e.ColumnIndex-1].Value.ToString();
                    PrintForms.SimpleReportViewer.ShowDialog(TC,TCHead , this);
                }
            }
            catch(Exception ex)
            {

            }
        }
        private void cmbStatus_SelectedIndexChanged_1(object sender, EventArgs e)
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
                            PurchaseOrderDB wodb = new PurchaseOrderDB();
                            prevpoh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevpoh.CommentStatus);
                            prevpoh.ForwardUser = approverUID;
                            prevpoh.ForwarderList = prevpoh.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (wodb.ForwardPurchaseOrder(prevpoh))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(prevpoh, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredPOHeader(listOption);
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
                    string reverseStr = getReverseString(prevpoh.ForwarderList);
                    //do forward activities
                    prevpoh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevpoh.CommentStatus);
                    PurchaseOrderDB podb = new PurchaseOrderDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevpoh.ForwarderList = reverseStr.Substring(0, ind);
                        prevpoh.ForwardUser = reverseStr.Substring(ind + 3);
                        prevpoh.DocumentStatus = prevpoh.DocumentStatus - 1;
                    }
                    else
                    {
                        prevpoh.ForwarderList = "";
                        prevpoh.ForwardUser = "";
                        prevpoh.DocumentStatus = 1;
                    }
                    if (podb.reversePO(prevpoh))
                    {
                        MessageBox.Show("PO Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredPOHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnListDocument_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevpoh.TemporaryNo + "-" + prevpoh.TemporaryDate.ToString("yyyyMMddhhmmss"));
                pnlPDFViewer.Controls.Add(dgvDocumentList);
                dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
            }
            catch (Exception ex)
            {
            }
        }

        private void btnCloseDocument_Click(object sender, EventArgs e)
        {
            removePDFControls();
            showPDFFileGrid();
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
        private void removeControlsFromPaymentTermsPanel()
        {
            try
            {
                //foreach (Control p in pnldgv.Controls)
                //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
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
        private void dgvDocumentList_CellContentClick(Object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView dgv = sender as DataGridView;
                string fileName = "";
                if (e.RowIndex < 0)
                    return;
                string colName = dgv.Columns[e.ColumnIndex].Name;
                if (e.ColumnIndex == 0)
                {
                    removePDFControls();
                    fileName = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    ////string docDir = Main.documentDirectory + "\\" + docID;
                    string subDir = prevpoh.TemporaryNo + "-" + prevpoh.TemporaryDate.ToString("yyyyMMddhhmmss");
                    dgv.Enabled = false;
                    ////DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    fileName = DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    ////showPDFFile(fileName);
                    ////dgv.Visible = false;
                    System.Diagnostics.Process.Start(fileName);
                    dgv.Enabled = true;
                }
                else if (colName == "Edit")
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
                if (colName == "Delete")
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
                //chkCommentStatus.Visible = false;
                txtComments.Visible = false;
                disableTabPages();
                clearTabPageControls();
                //----24/11/2016
                handleNewButton();
                handleGrdViewButton();
                handleGrdEditButton();
                ////handleGrdPrintButton();
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
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabPOHeader;
                }
                else if (btnName == "btnEditPanel") //called from cancel,save,forward,approve and reverse button events
                {
                    ////btnNew.Visible = true;
                    btnExit.Visible = true;
                }
                //gridview buttons
                else if (btnName == "gEdit")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    btnGetComments.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = true;
                    ////chkCommentStatus.Visible = true;
                    txtComments.Visible = true;
                    tabControl1.SelectedTab = tabPODetail;
                }
                else if (btnName == "gApprove")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                    disableTabPages();
                    tabControl1.SelectedTab = tabPODetail;
                }
                else if (btnName == "gView")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    //disableTabPages();
                    enableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    tabControl1.SelectedTab = tabPODetail;
                }
                else if (btnName == "gLoadDocument")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                }
                changeListOptColor();

                pnlEditButtons.Refresh();
                //if the user privilege is only view, show only the Approved documents button
                int ups = getuserPrivilegeStatus();
                if (ups == 1 || ups == 0)
                {
                    grdList.Columns["gEdit"].Visible = false;
                    grdList.Columns["gApprove"].Visible = false;
                    btnActionPending.Visible = false;
                    btnApprovalPending.Visible = false;
                    btnApproved.Visible = false;
                    if (ups == 1)
                    {
                        grdList.Columns["gView"].Visible = true;
                    }
                    else
                    {
                        grdList.Columns["gView"].Visible = false;
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
            grdList.Columns["gEdit"].Visible = false;
            grdList.Columns["gApprove"].Visible = false;
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                if (listOption == 1)
                {
                    grdList.Columns["gEdit"].Visible = true;
                    grdList.Columns["gApprove"].Visible = true;
                }
            }
        }

        void handleGrdViewButton()
        {
            grdList.Columns["gView"].Visible = false;
            //if any one of view,add and edit
            if (Main.itemPriv[0] || Main.itemPriv[1] || Main.itemPriv[2])
            {
                //list option 1 should not have view buttuon visible (edit is avialable)
                if (listOption != 1)
                {
                    grdList.Columns["gView"].Visible = true;
                }
            }
        }
        void handleGrdPrintButton()
        {
            grdList.Columns["gPrint"].Visible = false;
            //if any one of view,add and edit
            if (Main.itemPriv[0] || Main.itemPriv[1] || Main.itemPriv[2])
            {
                //list option 1 should not have view buttuon visible (edit is avialable)
                if (listOption == 1 || listOption == 2)
                {
                    grdList.Columns["gPrint"].Visible = false;
                }
                else
                {
                    grdList.Columns["gPrint"].Visible = true;
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
                ///chkCommentStatus.Checked = false;
                txtComments.Text = "";
                grdPODetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }

        private void txtExchangeRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdPODetail.Rows.Count != 0 && txtPOValue.Text.Length != 0
                    && txtProductValue.Text.Length != 0 && txtExchangeRate.Text.Length != 0)
                {
                    double dd = Convert.ToDouble(txtExchangeRate.Text);
                    txtProductValueINR.Text = (Convert.ToDouble(txtProductValue.Text) * dd).ToString();
                    txtPOValueINR.Text = (Convert.ToDouble(txtPOValue.Text) * dd).ToString();
                    txtTaxAmountINR.Text = (Convert.ToDouble(txtPOTax.Text) * dd).ToString();
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //removeControlsFromCommenterPanel();
            //removeControlsFromForwarderPanel();
            try
            {
                removeControlsFromForwarderPanelTV();
                string selTab = tabControl1.SelectedTab.Name;
                if (selTab == "tabIndentDetail" && (colClicked == "gEdit" || isNewClick == true))
                {

                    if (txtReferenceIndent.Text.Trim().Length != 0)
                    {
                        string refINStr = txtReferenceIndent.Text.Trim();
                        getIndentDetails(refINStr);
                    }
                    else
                        MessageBox.Show("Reference Indent is Empty ");
                }
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
                        dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevpoh.TemporaryNo + "-" + prevpoh.TemporaryDate.ToString("yyyyMMddhhmmss"));
                        dgvDocumentList.Size = new Size(870, 300);
                        pnlPDFViewer.Controls.Add(dgvDocumentList);
                        dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
                        if (prevpoh.Status == 1 && prevpoh.DocumentStatus == 99)
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

        private void btnSelDelvAddress_Click(object sender, EventArgs e)
        {
            //if(cmbCustomer.SelectedIndex == -1)
            //{
            //    MessageBox.Show("Select Customer");
            //    return;
            //}
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            lv = CompanyAddressDB.getCustomerAddListView(1);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_ClickedAdd);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_ClickedAdd);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
        }
        private void lvOK_ClickedAdd(object sender, EventArgs e)
        {
            try
            {
                if(lv.CheckedIndices.Count != 1)
                {
                    MessageBox.Show("Select only one Address");
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        txtDeliveryAddress.Text = itemRow.SubItems[2].Text;
                    }
                }
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_ClickedAdd(object sender, EventArgs e)
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
        private void getTaxDetails(List<podetail> pod)
        {
            try
            {
                double quantity = 0;
                double price = 0;
                double cost = 0.0;
                productvalue = 0.0;
                taxvalue = 0.0;
                string strtaxCode = "";
                TaxDetailsTable.Rows.Clear();
                //for (int i = 0; i < grdPODetail.Rows.Count; i++)
                foreach (podetail det in pod)
                {
                    //quantity = Convert.ToDouble(grdPODetail.Rows[i].Cells["Quantity"].Value);
                    quantity = Convert.ToDouble(det.Quantity);
                    //price = Convert.ToDouble(grdPODetail.Rows[i].Cells["Price"].Value);
                    price = Convert.ToDouble(det.Price);
                    if (price != 0 && quantity != 0)
                    {
                        cost = Math.Round(quantity * price, 2);
                    }

                    try
                    {
                        strtaxCode = det.TaxCode;
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
                }
            }
            catch (Exception ex)
            {

            }
        }

        //-- Validating Header and Detail String For Single Quotes

        private poheader verifyHeaderInputString(poheader poh)
        {
            try
            {
                poh.SpecialNote = Utilities.replaceSingleQuoteWithDoubleSingleQuote(poh.SpecialNote);
                poh.Remarks = Utilities.replaceSingleQuoteWithDoubleSingleQuote(poh.Remarks);
                poh.DeliveryAddress = Utilities.replaceSingleQuoteWithDoubleSingleQuote(poh.DeliveryAddress);
                poh.CustomerID = Utilities.replaceSingleQuoteWithDoubleSingleQuote(poh.CustomerID);
                poh.TermsAndCondition = Utilities.replaceSingleQuoteWithDoubleSingleQuote(poh.TermsAndCondition);
                poh.ReferenceIndent = Utilities.replaceSingleQuoteWithDoubleSingleQuote(poh.ReferenceIndent);
                poh.ReferenceQuotation = Utilities.replaceSingleQuoteWithDoubleSingleQuote(poh.ReferenceQuotation);
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
                for (int i = 0; i < grdPODetail.Rows.Count; i++)
                {
                    grdPODetail.Rows[i].Cells["Item"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdPODetail.Rows[i].Cells["Item"].Value.ToString());
                    grdPODetail.Rows[i].Cells["ModelNo"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdPODetail.Rows[i].Cells["ModelNo"].Value.ToString());
                    grdPODetail.Rows[i].Cells["Description"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdPODetail.Rows[i].Cells["Description"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSelectCustomer_Click(object sender, EventArgs e)
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
                chkBoxCustomer.Items.AddRange(new object[] {"Customer","Supplier","Contractor","Transporter","Others"});
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

                txtSearch = new TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(460, 36);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtCustSearch_TextChangedInEmpGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

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
                    txtCustomer.Text = row.Cells["ID"].Value.ToString() + "-" +
                                Environment.NewLine + row.Cells["Name"].Value.ToString();
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
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdCustList.Rows)
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
                if (txtSearchPu.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdList.Rows)
                    {
                        if (clm == null || clm.Length == 0)
                        {
                            if (!row.Cells["Dname"].Value.ToString().Trim().ToLower().Contains(txtSearchPu.Text.Trim().ToLower()))
                            {
                                row.Visible = false;
                            }
                        }
                        else
                        {

                            if (!row.Cells[clm].FormattedValue.ToString().Trim().ToLower().Contains(txtSearchPu.Text.Trim().ToLower()))
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

        private void PurchaseOrder_Enter(object sender, EventArgs e)
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

        private void btnShowPackingSpec_Click(object sender, EventArgs e)
        {
            try
            {
                showPopUpForPackingSpec(txtPackingSpec.Text.Trim());
            }
            catch (Exception ex)
            {
            }
        }
        private void showPopUpForPackingSpec(string str)
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
            head.Text = "Fill Specification Below";
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
            lvOK.Click += new System.EventHandler(this.lvOK_Clickpack);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new System.Drawing.Point(273, 142);
            lvCancel.Size = new System.Drawing.Size(73, 23);
            lvCancel.Cursor = Cursors.Hand;
            lvCancel.Click += new System.EventHandler(this.lvCancel_ClickPack);
            frmPopup.Controls.Add(lvCancel);

            frmPopup.ShowDialog();
        }
        private void lvOK_Clickpack(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtDesc.Text.Trim()))
                {
                    MessageBox.Show("Specification is empty");
                    return;
                }
                txtPackingSpec.Text = txtDesc.Text.Trim();
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        private void lvCancel_ClickPack(object sender, EventArgs e)
        {
            try
            {
                txtDesc.Text = "";
                frmPopup.Close();
                frmPopup.Dispose();

            }
            catch (Exception)
            {
            }
        }
    }

}

