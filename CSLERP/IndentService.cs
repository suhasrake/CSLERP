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
    public partial class IndentService : System.Windows.Forms.Form
    {
        string docID = "INDENTSERVICE";
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        string forwarderList = "";
        string approverList = "";
        double productvalue = 0.0;
        double taxvalue = 0.0;
        indentserviceheader prevish;
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        Panel pnllv = new Panel();
        ListView lv = new ListView();
        TextBox txtSearch = new TextBox();
        TextBox txtSearchDet = new TextBox();
        string userString = "";
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
        string ColSelStr = "";
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        int descClickRowIndex = -1;
        RichTextBox txtDesc = new RichTextBox();
        Boolean AddRowClick = false;
        DataGridView grdRefSel = new DataGridView();
        DataGridView grdSelPODetail = new DataGridView();
        DataGridView grdCustList = new DataGridView();
        TreeView tv = new TreeView();
        public IndentService()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void WorkOrderRequest_Load(object sender, EventArgs e)
        {
            ////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;
            ////this.FormBorderStyle = FormBorderStyle.Fixed3D;
            String a = this.Size.ToString();
            grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdList.EnableHeadersVisualStyles = false;
            ListFilteredIndentServiceHeader(listOption);
            //applyPrivilege();
        }
        private void ListFilteredIndentServiceHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                IndentServiceDB isdb = new IndentServiceDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                List<indentserviceheader> ISHeaders = isdb.getFilteredIndentServiceHeaders(userString, option, userCommentStatusString);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (indentserviceheader ish in ISHeaders)
                {
                    if (option == 1)
                    {
                        if (ish.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = ish.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = ish.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = ish.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = ish.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gWORequestNo"].Value = ish.WORequestNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gWORequestDate"].Value = ish.WORequestDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gReferenceInternalOrder"].Value = ish.ReferenceInternalOrder;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerID"].Value = ish.CustomerID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerName"].Value = ish.CustomerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyID"].Value = ish.CurrencyID;
                    grdList.Rows[grdList.RowCount - 1].Cells["ExchangeRate"].Value = ish.ExchangeRate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyName"].Value = ish.CurrencyName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStartDate"].Value = ish.StartDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTargetDate"].Value = ish.TargetDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPaymentTerms"].Value = ish.PaymentTerms;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPaymentMode"].Value = ish.PaymentMode;
                    grdList.Rows[grdList.RowCount - 1].Cells["gServiceValue"].Value = ish.ServiceValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTaxAmount"].Value = ish.TaxAmount;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTotalAmount"].Value = ish.TotalAmount;
                    grdList.Rows[grdList.RowCount - 1].Cells["ServiceValueINR"].Value = ish.ServiceValueINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["TaxAmountINR"].Value = ish.TaxAmountINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["TotalAmountINR"].Value = ish.TotalAmountINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["gRemarks"].Value = ish.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = ish.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = ish.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = ish.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreator"].Value = ish.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gForwarder"].Value = ish.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gApprover"].Value = ish.ApproverName;
                    //grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatusNo"].Value = ComboFIll.getDocumentStatusString(woh.DocumentStatus);
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = ish.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ContractorReference"].Value = ish.ContractorReference;
                    grdList.Rows[grdList.RowCount - 1].Cells["ComntStatus"].Value = ish.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = ish.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwarderLists"].Value = ish.ForwarderList;
                    //if (IndentServiceDB.isWOPreparedForIS(ish.WORequestNo, ish.WORequestDate))
                    //{
                    //    grdList.Rows[grdList.RowCount - 1].DefaultCellStyle.BackColor = Color.Tan;
                    //}
                    if (ish.NoOfWOFound > 0)
                    {
                        grdList.Rows[grdList.RowCount - 1].DefaultCellStyle.BackColor = Color.Tan;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in WO Request Listing");
            }

            setButtonVisibility("init");
            pnlList.Visible = true;

            grdList.Columns["gCreator"].Visible = true;
            grdList.Columns["gForwarder"].Visible = true;
            grdList.Columns["gApprover"].Visible = true;
        }
        private void initVariables()
        {

            if (getuserPrivilegeStatus() == 1)
            {
                //user is only a viewer
                listOption = 6;
            }
            docID = Main.currentDocument;
            //CustomerDB.fillLedgerTypeComboNew(cmbCustomer, "Customer");
            //CustomerDB.fillCustomerComboNew(cmbCustomer);
            CatalogueValueDB.fillCatalogValueComboNew(cmbPaymentMode, "PaymentMode");
            CurrencyDB.fillCurrencyComboNew(cmbCurrency);
            cmbCurrency.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCurrency, "INR");
            //OfficeDB.fillOfficeComboNew(cmbOfficeID);
            //TaxCodeDB.fillTaxCodeCombo(cmbTaxCode);

            dtTemporaryDate.Format = DateTimePickerFormat.Custom;
            dtTemporaryDate.CustomFormat = "dd-MM-yyyy";
            dtTemporaryDate.Enabled = false;
            dtWORequestDate.Format = DateTimePickerFormat.Custom;
            dtWORequestDate.CustomFormat = "dd-MM-yyyy";
            dtWORequestDate.Enabled = false;
            dtStartDate.Format = DateTimePickerFormat.Custom;
            dtStartDate.CustomFormat = "dd-MM-yyyy";
            dtStartDate.Enabled = true;
            dtTargetDate.Format = DateTimePickerFormat.Custom;
            dtTargetDate.CustomFormat = "dd-MM-yyyy";
            dtTargetDate.Enabled = true;
            txtExchangeRate.Text = "1";
            txtPaymentTerms.Enabled = false;
            txtTemporaryNo.Enabled = false;
            txtWORequestNo.Enabled = false;
            //txtCreditPeriods.Enabled = true;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            grdWODetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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
            txtTemporaryNo.TabIndex = 0;
            dtTemporaryDate.TabIndex = 1;
            txtWORequestNo.TabIndex = 2;
            dtWORequestDate.TabIndex = 3;
            dtStartDate.TabIndex = 4;
            dtTargetDate.TabIndex = 5;
            txtReferenceServiceInward.TabIndex = 6;
            btnSelRefServiceInward.TabIndex = 7;
            btnPAFService.TabIndex = 8;
            txtCustomer.TabIndex = 9;
            txtProjectID.TabIndex = 10;
            txtOffice.TabIndex = 11;
            txtPaymentTerms.TabIndex = 12;
            btnPaymentTerm.TabIndex = 13;
            cmbPaymentMode.TabIndex = 14;
            txtContractor.TabIndex = 15;
            btnSelectContractor.TabIndex = 16;
            txtContractorRef.TabIndex = 17;
            btnShowContRefPopup.TabIndex = 18;
            cmbCurrency.TabIndex = 19;
            txtExchangeRate.TabIndex = 20;
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

        public void clearData()
        {
            try
            {
                //clear all grid views
                grdWODetail.Rows.Clear();
                dgvComments.Rows.Clear();
                //dgvpt.Rows.Clear();
                //----------clear temperory panels

                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                //----------
                cmbCurrency.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCurrency, "INR");
                txtProjectID.Text = "";
                txtOffice.Text = "";
                txtCustomer.Text = "";
                cmbPaymentMode.SelectedIndex = -1;
                dtTemporaryDate.Value = DateTime.Parse("1900-01-01");
                dtWORequestDate.Value = DateTime.Parse("1900-01-01");
                dtStartDate.Value = DateTime.Today.Date;
                dtTargetDate.Value = DateTime.Today.Date;

                grdWODetail.Rows.Clear();
                txtPaymentTerms.Text = "";
                txtContractor.Text = "";
                txtContractorRef.Text = "";
                txtTemporaryNo.Text = "";
                txtWORequestNo.Text = "";
                txtRemarks.Text = "";

                txtExchangeRate.Text = "1";
                txtServiceValue.Text = "";
                txtServiceValueINR.Text = "";
                txtTaxAmount.Text = "";
                txtTaxAmountINR.Text = "";
                txtTotalValue.Text = "";
                txtTotalValueINR.Text = "";

                txtReferenceServiceInward.Text = "";
                btnProductValue.Text = "0";
                btnTaxAmount.Text = "0";
                prevish = new indentserviceheader();
                removeControlsPaymentTermPanel();
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
                clearData();
                btnSave.Text = "Save";
                pnlAddEdit.Visible = true;
                closeAllPanels();
                pnlAddEdit.Visible = true;
                tabControl1.SelectedTab = tabWOHeader;
                tabWOHeader.Enabled = true;
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
                AddISDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddISDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdWODetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkISDetailGridRows())
                    {
                        return false;
                    }
                }
                grdWODetail.Rows.Add();
                int kount = grdWODetail.RowCount;
                grdWODetail.Rows[kount - 1].Cells[0].Value = kount;
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdWODetail.Rows[kount - 1].Cells["gTCode"]);
                TaxCodeDB.fillTaxCodeGridViewCombo(ComboColumn2, "");
                grdWODetail.Rows[kount - 1].Cells["POItemRefNo"].Value = Convert.ToInt32(0);
                grdWODetail.Rows[kount - 1].Cells["WorkDescription"].Value = " ";
                grdWODetail.Rows[kount - 1].Cells["WorkLocation"].Value = "";
                grdWODetail.Rows[kount - 1].Cells["Quantity"].Value = Convert.ToDouble(0);
                grdWODetail.Rows[kount - 1].Cells["Price"].Value = Convert.ToDouble(0);
                grdWODetail.Rows[kount - 1].Cells["Rate"].Value = Convert.ToDouble(0);
                grdWODetail.Rows[kount - 1].Cells["Value"].Value = Convert.ToDouble(0);
                grdWODetail.Rows[kount - 1].Cells["Tax"].Value = Convert.ToDouble(0);
                grdWODetail.Rows[kount - 1].Cells["WarrantyDays"].Value = 0;
                grdWODetail.Rows[kount - 1].Cells["TaxDetails"].Value = " ";
                var BtnCell = (DataGridViewButtonCell)grdWODetail.Rows[kount - 1].Cells["Delete"];
                BtnCell.Value = "Del";
                if (AddRowClick)
                    grdWODetail.FirstDisplayedScrollingRowIndex = grdWODetail.RowCount - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddWODetailRow() : Error");
            }

            return status;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            closeAllPanels();
            btnNew.Visible = true;
            btnExit.Visible = true;
            pnlList.Visible = true;
            //enableBottomButtons();
            //pnlBottomActions.Visible = true;
        }

        private Boolean verifyAndReworkISDetailGridRows()
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

                if (grdWODetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Work Order Grid details");
                    btnProductValue.Text = productvalue.ToString();
                    btnTaxAmount.Text = taxvalue.ToString(); //fill this later
                    //btnProductValue.Text = txtServiceValue.Text;

                    txtServiceValue.Text = productvalue.ToString();
                    txtServiceValueINR.Text = (Convert.ToDouble(txtServiceValue.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    txtTaxAmount.Text = taxvalue.ToString(); //fill this later
                    txtTaxAmountINR.Text = (Convert.ToDouble(txtTaxAmount.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    txtTotalValue.Text = (productvalue + taxvalue).ToString();
                    txtTotalValueINR.Text = (Convert.ToDouble(txtTotalValue.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                    return false;
                }
                //clear tax details table
                TaxDetailsTable.Rows.Clear();
                for (int i = 0; i < grdWODetail.Rows.Count; i++)
                {
                    if (grdWODetail.Rows[i].Cells["gTCode"].Value == null ||
                        grdWODetail.Rows[i].Cells["gTCode"].Value.ToString().Length == 0)
                    {
                        MessageBox.Show("Fill TaxCode in row " + (i + 1));
                        return false;
                    }
                    if ((Convert.ToDouble(grdWODetail.Rows[i].Cells["Price"].Value) != 0) &&
                        (Convert.ToDouble(grdWODetail.Rows[i].Cells["Price"].Value) <= Convert.ToDouble(grdWODetail.Rows[i].Cells["Rate"].Value)))
                    {
                        MessageBox.Show("WARNING:\n Indent price is equal/more than po price.Row NO: " + (i + 1));
                    }
                    if (Convert.ToDouble(grdWODetail.Rows[i].Cells["Rate"].Value) <= 0)
                    {
                        MessageBox.Show("Indent Price should more than zero. Check Row: " + (i + 1));
                        return false;
                    }
                    grdWODetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if (((grdWODetail.Rows[i].Cells["WorkDescription"].Value == null) ||
                        (grdWODetail.Rows[i].Cells["WorkLocation"].Value == null)) ||
                        (grdWODetail.Rows[i].Cells["WorkDescription"].Value.ToString().Trim().Length == 0) ||
                        (grdWODetail.Rows[i].Cells["WorkLocation"].Value.ToString().Trim().Length == 0) ||
                        (grdWODetail.Rows[i].Cells["Quantity"].Value == null) ||
                        (grdWODetail.Rows[i].Cells["Rate"].Value == null) ||
                         (grdWODetail.Rows[i].Cells["Value"].Value == null) ||
                        (Convert.ToDouble(grdWODetail.Rows[i].Cells["Quantity"].Value) == 0) ||
                        (Convert.ToDouble(grdWODetail.Rows[i].Cells["Rate"].Value) == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }

                    quantity = Convert.ToDouble(grdWODetail.Rows[i].Cells["Quantity"].Value);
                    price = Convert.ToDouble(grdWODetail.Rows[i].Cells["Rate"].Value);
                    if (price != 0 && quantity != 0)
                    {
                        cost = Math.Round(quantity * price, 2);
                    }

                    try
                    {
                        strtaxCode = grdWODetail.Rows[i].Cells["gTCode"].Value.ToString();
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
                    grdWODetail.Rows[i].Cells["Value"].Value = Convert.ToDouble(cost.ToString());
                    grdWODetail.Rows[i].Cells["Tax"].Value = Convert.ToDouble(ttax2);
                    grdWODetail.Rows[i].Cells["TaxDetails"].Value = strTax;
                    productvalue = productvalue + cost;
                    taxvalue = taxvalue + ttax2;

                    //- rewok tax value
                }
                btnProductValue.Text = productvalue.ToString();
                btnTaxAmount.Text = taxvalue.ToString(); //fill this later

                txtServiceValue.Text = productvalue.ToString();
                txtServiceValueINR.Text = (Convert.ToDouble(txtServiceValue.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                txtTaxAmount.Text = taxvalue.ToString(); //fill this later
                txtTaxAmountINR.Text = (Convert.ToDouble(txtTaxAmount.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();
                txtTotalValue.Text = (productvalue + taxvalue).ToString();
                txtTotalValueINR.Text = (Convert.ToDouble(txtTotalValue.Text) * Convert.ToDouble(txtExchangeRate.Text)).ToString();

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
        private Boolean validateItems()
        {
            Boolean status = true;
            try
            {
                if (docID == "INDENTSERVICE")
                {

                    for (int i = 0; i < grdWODetail.Rows.Count - 1; i++)
                    {
                        for (int j = i + 1; j < grdWODetail.Rows.Count; j++)
                        {

                            if (grdWODetail.Rows[i].Cells["Item"].Value.ToString() == grdWODetail.Rows[j].Cells["Item"].Value.ToString() &&
                                grdWODetail.Rows[i].Cells["WorkLocation"].Value.ToString() == grdWODetail.Rows[j].Cells["WorkLocation"].Value.ToString())
                            {
                                //duplicate item code
                                MessageBox.Show("Item + Work Location duplicated in details... please ensure correctness for (" +
                                    grdWODetail.Rows[i].Cells["WorkDescription"].Value.ToString() + ")");
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

        private List<indentservicedetail> getISDetails(indentserviceheader ish)
        {
            List<indentservicedetail> ISDetails = new List<indentservicedetail>();
            try
            {
                indentservicedetail isd = new indentservicedetail();
                for (int i = 0; i < grdWODetail.Rows.Count; i++)
                {
                    isd = new indentservicedetail();
                    isd.DocumentID = ish.DocumentID;
                    isd.TemporaryNo = ish.TemporaryNo;
                    isd.TemporaryDate = ish.TemporaryDate;
                    isd.StockItemID = grdWODetail.Rows[i].Cells["Item"].Value.ToString().Substring(0, grdWODetail.Rows[i].Cells["Item"].Value.ToString().IndexOf('-'));
                    isd.TaxCode = grdWODetail.Rows[i].Cells["gTCode"].Value.ToString();
                    isd.WorkDescription = grdWODetail.Rows[i].Cells["WorkDescription"].Value.ToString().Trim();
                    isd.WorkLocation = grdWODetail.Rows[i].Cells["WorkLocation"].Value.ToString().Trim();
                    isd.Quantity = Convert.ToDouble(grdWODetail.Rows[i].Cells["Quantity"].Value);
                    isd.Rate = Convert.ToDouble(grdWODetail.Rows[i].Cells["Rate"].Value);
                    isd.POItemRefNo = Convert.ToInt32(grdWODetail.Rows[i].Cells["POItemRefNo"].Value);
                    isd.Tax = Convert.ToDouble(grdWODetail.Rows[i].Cells["Tax"].Value);
                    isd.WarrantyDays = Convert.ToInt32(grdWODetail.Rows[i].Cells["WarrantyDays"].Value);

                    isd.TaxDetails = grdWODetail.Rows[i].Cells["TaxDetails"].Value.ToString();

                    ISDetails.Add(isd);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("getISDetails() : Error getting Indent Service Details");
                ISDetails = null;
            }
            return ISDetails;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            Boolean status = true;
            try
            {

                IndentServiceDB isdb = new IndentServiceDB();
                indentserviceheader ish = new indentserviceheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    if (!verifyAndReworkISDetailGridRows())
                    {
                        return;
                    }
                    ish.DocumentID = docID;
                    ish.CustomerID = txtContractor.Text.Trim().Substring(0, txtContractor.Text.Trim().IndexOf('-')).Trim(); //Contractor ID
                    ish.WORequestDate = dtWORequestDate.Value;
                    ish.ReferenceInternalOrder = txtReferenceServiceInward.Text.ToString(); //Reference Service Inward 
                    ish.StartDate = dtStartDate.Value;
                    ish.TargetDate = dtTargetDate.Value;
                    ish.CurrencyID = ((Structures.ComboBoxItem)cmbCurrency.SelectedItem).HiddenValue;
                    ish.PaymentMode = ((Structures.ComboBoxItem)cmbPaymentMode.SelectedItem).HiddenValue;
                    ish.PaymentTerms = txtPaymentTerms.Text;
                    ish.ExchangeRate = Convert.ToDecimal(txtExchangeRate.Text);
                    ish.ServiceValue = Convert.ToDouble(txtServiceValue.Text);
                    ish.ServiceValueINR = Convert.ToDouble(txtServiceValueINR.Text);
                    ish.TaxAmount = Convert.ToDouble(txtTaxAmount.Text);
                    ish.TaxAmountINR = Convert.ToDouble(txtTaxAmountINR.Text);
                    ish.TotalAmount = Convert.ToDouble(txtTotalValue.Text);
                    ish.TotalAmountINR = Convert.ToDouble(txtTotalValueINR.Text);
                    ish.Remarks = txtRemarks.Text;
                    ish.ContractorReference = txtContractorRef.Text.Trim().Replace("'", "''");
                    ish.Comments = docCmtrDB.DGVtoString(dgvComments).Replace("'", "''");
                    ish.ForwarderList = prevish.ForwarderList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    // woh.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    ish.DocumentStatus = 1; //created
                    ish.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    ish.TemporaryNo = Convert.ToInt32(txtTemporaryNo.Text);
                    ish.TemporaryDate = prevish.TemporaryDate;
                    ish.DocumentStatus = prevish.DocumentStatus;
                }
                //Replacing single quotes
                ish = verifyHeaderInputString(ish);
                verifyDetailInputString();
                if (isdb.validateIndentServiceHeader(ish))
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
                            ish.CommentStatus = docCmtrDB.createCommentStatusString(prevish.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            ish.CommentStatus = prevish.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            ish.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            ish.CommentStatus = prevish.CommentStatus;
                        }
                    }
                    //----------
                    int tmpStatus = 1;
                    if (txtComments.Text.Trim().Length > 0)
                    {
                        ish.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus).Replace("'", "''");
                    }
                    List<indentservicedetail> ISDetails = getISDetails(ish);
                    if (btnText.Equals("Update"))
                    {
                        if (isdb.updateIndentServiceHeaderAndDetail(ish, prevish, ISDetails))
                        {
                            MessageBox.Show("Indent Service details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredIndentServiceHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update Indent Service header");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (isdb.InsertIndentServiceHeaderAndDetail(ish, ISDetails))
                        {
                            MessageBox.Show("Indent Service Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredIndentServiceHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to InsertIndent Service Header");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Indent Service Header Validation failed");
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("createAndUpdateWODetails() : Error");
                status = false;
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }
        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
                    setButtonVisibility(columnName);
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    IndentServiceDB isdb = new IndentServiceDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevish = new indentserviceheader();
                    prevish.CommentStatus = grdList.Rows[e.RowIndex].Cells["ComntStatus"].Value.ToString();
                    prevish.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    prevish.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    prevish.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    prevish.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    if (prevish.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    if (columnName == "View")
                    {
                        tabControl1.TabPages["tabWODetail"].Enabled = true;
                    }
                    prevish.Comments = IndentServiceDB.getUserComments(prevish.DocumentID, prevish.TemporaryNo, prevish.TemporaryDate);

                    prevish.WORequestNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gWORequestNo"].Value.ToString());
                    prevish.WORequestDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gWORequestDate"].Value.ToString());
                    prevish.ReferenceInternalOrder = grdList.Rows[e.RowIndex].Cells["gReferenceInternalOrder"].Value.ToString(); // Reference POService Inward

                    prevish.CustomerID = grdList.Rows[e.RowIndex].Cells["gCustomerID"].Value.ToString(); ///Contractor ID
                    prevish.CustomerName = grdList.Rows[e.RowIndex].Cells["gCustomerName"].Value.ToString(); ///Contractor Name
                    //Closing INdent Service
                    //if (columnName == "Close")
                    //{
                    //    DialogResult dialog = MessageBox.Show("Are you sure to close indent ?", "Alert", MessageBoxButtons.YesNo);
                    //    if (dialog == DialogResult.Yes)
                    //    {
                    //        if (IndentServiceDB.CloseIndentService(prevish))
                    //        {
                    //            MessageBox.Show("Indent service closed.");
                    //            grdList.Rows.RemoveAt(e.RowIndex);
                    //        }
                    //        else
                    //        {
                    //            MessageBox.Show("Fail to close Indent service");
                    //        }
                    //    }
                    //    btnExit.Visible = true;
                    //    return;
                    //}
                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Document Temp No:" + prevish.TemporaryNo + "\n" +
                            "Document Temp Date:" + prevish.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "WORequest No:" + prevish.WORequestNo + "\n" +
                            "WORequest Date:" + prevish.WORequestDate.ToString("dd-MM-yyyy") + "\n" +
                            "Customer:" + prevish.CustomerName;
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevish.TemporaryNo + "-" + prevish.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_2(null, null);
                        return;
                    }
                    //--------
                    prevish.CurrencyID = grdList.Rows[e.RowIndex].Cells["gCurrencyID"].Value.ToString();
                    prevish.CurrencyName = grdList.Rows[e.RowIndex].Cells["gCurrencyName"].Value.ToString();
                    prevish.ExchangeRate = Convert.ToDecimal(grdList.Rows[e.RowIndex].Cells["ExchangeRate"].Value.ToString());
                    prevish.StartDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gStartDate"].Value.ToString());
                    prevish.TargetDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTargetDate"].Value.ToString());
                    prevish.PaymentTerms = grdList.Rows[e.RowIndex].Cells["gPaymentTerms"].Value.ToString();
                    prevish.PaymentMode = grdList.Rows[e.RowIndex].Cells["gPaymentMode"].Value.ToString();
                    prevish.ServiceValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gServiceValue"].Value.ToString());
                    prevish.TotalAmount = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gTotalAmount"].Value.ToString());
                    prevish.TaxAmount = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gTaxAmount"].Value.ToString());
                    prevish.ServiceValueINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["ServiceValueINR"].Value.ToString());
                    prevish.TotalAmountINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["TotalAmountINR"].Value.ToString());
                    prevish.TaxAmountINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["TaxAmountINR"].Value.ToString());
                    prevish.Remarks = grdList.Rows[e.RowIndex].Cells["gRemarks"].Value.ToString();
                    prevish.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    prevish.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    prevish.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    prevish.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    prevish.CreatorName = grdList.Rows[e.RowIndex].Cells["gCreator"].Value.ToString();
                    prevish.ForwarderName = grdList.Rows[e.RowIndex].Cells["gForwarder"].Value.ToString();
                    prevish.ApproverName = grdList.Rows[e.RowIndex].Cells["gApprover"].Value.ToString();
                    prevish.ForwarderList = grdList.Rows[e.RowIndex].Cells["ForwarderLists"].Value.ToString();
                    prevish.ContractorReference = grdList.Rows[e.RowIndex].Cells["ContractorReference"].Value.ToString();
                    //--comments
                    //chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    prevish.CommentStatus = grdList.Rows[e.RowIndex].Cells["ComntStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(prevish.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(prevish.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---
                    txtContractor.Text = prevish.CustomerID + "-" + prevish.CustomerName;
                    cmbCurrency.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCurrency, prevish.CurrencyID);
                    //cmbPaymentTerms.SelectedIndex = cmbPaymentTerms.FindString(grdList.Rows[e.RowIndex].Cells["gPaymentTerms"].Value.ToString());
                    cmbPaymentMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbPaymentMode, prevish.PaymentMode);

                    string[] mainstrRef = prevish.ReferenceInternalOrder.Trim().Split(Main.delimiter1);
                    foreach (string str in mainstrRef)
                    {
                        if (str.Length != 0)
                        {
                            string[] substrRef = str.Trim().Split(';');
                            string DocIDStr = substrRef[0].Trim();
                            string noDateStr = substrRef[1].Trim();
                            int trackNo1 = Convert.ToInt32(noDateStr.Substring(0, noDateStr.IndexOf('(')));
                            int findex = noDateStr.IndexOf('(');
                            int sindex = noDateStr.IndexOf(')');
                            string tstr = noDateStr.Substring(findex + 1, (sindex - findex) - 1).Trim();
                            DateTime trackDate1 = Convert.ToDateTime(tstr);
                            string detStr = POPIHeaderDB.getPOPIServiceDetailForIndentService(trackNo1, trackDate1, DocIDStr);
                            string[] getsplittedStr = detStr.Split(Main.delimiter1);
                            txtCustomer.Text = txtCustomer.Text + getsplittedStr[1] + Environment.NewLine;
                            txtOffice.Text = txtOffice.Text + getsplittedStr[0] + Environment.NewLine;
                            txtProjectID.Text = txtProjectID.Text + getsplittedStr[2] + Environment.NewLine;
                        }
                    }

                    txtContractorRef.Text = prevish.ContractorReference;
                    txtReferenceServiceInward.Text = prevish.ReferenceInternalOrder;
                    txtTemporaryNo.Text = prevish.TemporaryNo.ToString();
                    try
                    {
                        dtTemporaryDate.Value = prevish.TemporaryDate;
                    }
                    catch (Exception)
                    {

                        dtTemporaryDate.Value = DateTime.Parse("1900-01-01");
                    }
                    txtWORequestNo.Text = prevish.WORequestNo.ToString();
                    try
                    {
                        dtWORequestDate.Value = prevish.WORequestDate;
                    }
                    catch (Exception)
                    {
                        dtWORequestDate.Value = DateTime.Parse("1900-01-01");
                    }

                    txtExchangeRate.Text = prevish.ExchangeRate.ToString();
                    txtServiceValue.Text = prevish.ServiceValue.ToString();
                    txtServiceValueINR.Text = prevish.ServiceValueINR.ToString();
                    txtTaxAmount.Text = prevish.TaxAmount.ToString();
                    txtTaxAmountINR.Text = prevish.TaxAmountINR.ToString();
                    txtTotalValue.Text = prevish.TotalAmount.ToString();
                    txtTotalValueINR.Text = prevish.TotalAmountINR.ToString();

                    dtStartDate.Value = prevish.StartDate;
                    dtTargetDate.Value = prevish.TargetDate;
                    txtPaymentTerms.Text = prevish.PaymentTerms;
                    btnProductValue.Text = prevish.ServiceValue.ToString();
                    btnTaxAmount.Text = prevish.TaxAmount.ToString();
                    txtRemarks.Text = prevish.Remarks;
                    txtReferenceServiceInward.Text = prevish.ReferenceInternalOrder.ToString();
                    List<indentservicedetail> ISDetail = IndentServiceDB.getIndentServiceDetails(prevish);
                    grdWODetail.Rows.Clear();
                    int i = 0;
                    foreach (indentservicedetail isd in ISDetail)
                    {
                        if (!AddISDetailRow())
                        {
                            MessageBox.Show("Error found in Indent Service Detail. Please correct before updating the details");
                        }
                        else
                        {
                            grdWODetail.Rows[i].Cells["Item"].Value = isd.StockItemID + "-" + isd.Description;
                            grdWODetail.Rows[i].Cells["gTCode"].Value = isd.TaxCode;
                            grdWODetail.Rows[i].Cells["WorkDescription"].Value = isd.WorkDescription;
                            grdWODetail.Rows[i].Cells["WorkLocation"].Value = isd.WorkLocation;
                            grdWODetail.Rows[i].Cells["Quantity"].Value = isd.Quantity;
                            grdWODetail.Rows[i].Cells["POItemRefNo"].Value = isd.POItemRefNo;
                            grdWODetail.Rows[i].Cells["Price"].Value = POPIHeaderDB.getPOItemWisePrice(isd.POItemRefNo);
                            grdWODetail.Rows[i].Cells["Rate"].Value = isd.Rate;
                            grdWODetail.Rows[i].Cells["Value"].Value = isd.Quantity * isd.Rate;
                            grdWODetail.Rows[i].Cells["Tax"].Value = isd.Tax;
                            grdWODetail.Rows[i].Cells["WarrantyDays"].Value = isd.WarrantyDays;
                            grdWODetail.Rows[i].Cells["TaxDetails"].Value = isd.TaxDetails;
                            i++;
                            productvalue = productvalue + isd.Quantity * isd.Rate;
                            taxvalue = taxvalue + isd.Tax;
                        }

                    }
                    if (!verifyAndReworkISDetailGridRows())
                    {
                        MessageBox.Show("Error found in Indent Service details. Please correct before updating the details");
                    }
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                    tabControl1.SelectedTab = tabWOHeader;
                    tabControl1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                setButtonVisibility("init");
                pnlList.Visible = true;
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
            AddISDetailRow();
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            //if (cmbTaxCode.SelectedIndex == -1)
            //{
            //    MessageBox.Show("select tax Code");
            //    return;
            //}
            try
            {
                if (txtExchangeRate.Text.Length == 0)
                {
                    MessageBox.Show("Fill exchange rate");
                    return;
                }
                verifyAndReworkISDetailGridRows();
            }
            catch (Exception ex)
            {
            }
        }

        private void ClearEntries_Click(object sender, EventArgs e)
        {
            try
            {

                DialogResult dialog = MessageBox.Show("Are you sure to clear all entries in grid detail ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdWODetail.Rows.Clear();
                    MessageBox.Show("Grid items cleared.");
                    verifyAndReworkISDetailGridRows();
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
                string columnName = grdWODetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdWODetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkISDetailGridRows();
                    }
                    if (columnName.Equals("TaxView"))
                    {
                        //show tax details
                        DialogResult dialog = MessageBox.Show(grdWODetail.Rows[e.RowIndex].Cells["TaxDetails"].Value.ToString(),
                            "Line No : " + (e.RowIndex + 1), MessageBoxButtons.OK);
                    }
                    if (columnName.Equals("Sel"))
                    {
                        showserviceItemsTreeView();
                    }
                    if (columnName.Equals("SelDesc"))
                    {
                        descClickRowIndex = e.RowIndex;
                        string strTest = grdWODetail.Rows[e.RowIndex].Cells["WorkDescription"].Value.ToString().Trim();
                        showPopUpForDescription(strTest);
                    }
                    if (columnName.Equals("SelLoc"))
                    {
                        descClickRowIndex = e.RowIndex;
                        string strTest = grdWODetail.Rows[e.RowIndex].Cells["WorkLocation"].Value.ToString().Trim();
                        showPopUpForLocation(strTest);
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
        private void showPopUpForLocation(string str)
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
            lvOK.Click += new System.EventHandler(this.lvOK_Click5Loc);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new System.Drawing.Point(273, 142);
            lvCancel.Size = new System.Drawing.Size(73, 23);
            lvCancel.Cursor = Cursors.Hand;
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click5Loc);
            frmPopup.Controls.Add(lvCancel);

            frmPopup.ShowDialog();
        }
        private void lvOK_Click5Loc(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtDesc.Text.Trim()))
                {
                    MessageBox.Show("Location is empty");
                    return;
                }
                grdWODetail.Rows[descClickRowIndex].Cells["WorkLocation"].Value = txtDesc.Text.Trim();
                grdWODetail.FirstDisplayedScrollingRowIndex = grdWODetail.Rows[descClickRowIndex].Index;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        private void lvCancel_Click5Loc(object sender, EventArgs e)
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
                grdWODetail.Rows[descClickRowIndex].Cells["WorkDescription"].Value = txtDesc.Text.Trim();
                grdWODetail.FirstDisplayedScrollingRowIndex = grdWODetail.Rows[descClickRowIndex].Index;
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
        private void removeControlsFromForwarderPanelTV()
        {
            try
            {
                pnlForwarder.Controls.Clear();
                Control nc = pnlForwarder.Parent;
                nc.Controls.Remove(pnlForwarder);
            }
            catch (Exception ex)
            {
            }
        }
        private void showserviceItemsTreeView()
        {
            removeControlsFromForwarderPanelTV();
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
            lbl.Text = "Tree View For Service";
            tv = ServiceItemsDB.getServiceItemTreeView();
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
                    grdWODetail.CurrentRow.Cells["Item"].Value = s;
                    //grdPOPIDetail.CurrentRow.Cells["ServiceItem"].Value = s;
                    tv.CheckBoxes = true;
                    pnlForwarder.Controls.Remove(tv);
                    pnlForwarder.Visible = false;
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
        private void lvOK_Clicked3(object sender, EventArgs e)
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
                        grdWODetail.CurrentRow.Cells["Item"].Value = itemRow.SubItems[1].Text + "-" + itemRow.SubItems[2].Text;
                        frmPopup.Close();
                        frmPopup.Dispose();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_Clicked3(object sender, EventArgs e)
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
        //private void listView2_ItemCheck3(object sender, ItemCheckedEventArgs e)
        //{
        //    int c = lv.Items.Count;
        //    if (lv.CheckedItems.Count > 1)
        //    {
        //        MessageBox.Show("Cannot select more than one item");
        //        e.Item.Checked = false;
        //    }
        //}
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

        private void pnlAddEdit_Paint(object sender, PaintEventArgs e)
        {

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

            ListFilteredIndentServiceHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredIndentServiceHeader(listOption);
        }
        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredIndentServiceHeader(listOption);
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
            ////lvForwardCancel.Visible = false;
            frmPopup.ShowDialog();
            //pnlForwarder.Visible = true;
            //pnlAddEdit.Controls.Add(pnlForwarder);
            //pnlAddEdit.BringToFront();
            //pnlForwarder.BringToFront();
            //pnlForwarder.Focus();
        }
        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private Boolean updateDashBoard(indentserviceheader ish, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = ish.DocumentID;
                dsb.TemporaryNo = ish.TemporaryNo;
                dsb.TemporaryDate = ish.TemporaryDate;
                dsb.DocumentNo = ish.WORequestNo;
                dsb.DocumentDate = ish.WORequestDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = ish.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(prevish.DocumentID);
                    foreach (documentreceiver docRec in docList)
                    {
                        dsb.ToUser = docRec.EmployeeID;   //To store UserID Form DocumentReceiver for current Document
                        dsb.DocumentDate = UpdateTable.getSQLDateTime();
                        if (!ddsDB.insertDashboardAlarm(dsb))
                        {
                            MessageBox.Show("Failed to update DashBoard");
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
        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                IndentServiceDB isdb = new IndentServiceDB();
                ////popiheader popih = new popiheader();
                FinancialLimitDB flDB = new FinancialLimitDB();
                if (!flDB.checkEmployeeFinancialLimit(docID, Login.empLoggedIn, Convert.ToDouble(btnProductValue.Text), 0))
                {
                    MessageBox.Show("No financial power for approving this document");
                    return;
                }
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevish.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevish.CommentStatus);
                    if (prevish.Status != 96)
                    {
                        prevish.WORequestNo = DocumentNumberDB.getNewNumber(docID, 2);
                    }
                    if (isdb.ApproveIndentService(prevish))
                    {
                        MessageBox.Show("Indent Service Document Approved");
                        if (!updateDashBoard(prevish, 2))
                        {
                            MessageBox.Show("Failed to update DashBoard");
                        }
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredIndentServiceHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnReferenceInternalOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtReferenceServiceInward.Text.Length != 0)
                {
                    DialogResult dialog = MessageBox.Show("Inident service Detail will removed ?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        txtReferenceServiceInward.Text = "";
                        txtCustomer.Text = "";
                        txtProjectID.Text = "";
                        txtOffice.Text = "";
                        grdWODetail.Rows.Clear();
                    }
                    else
                    {
                        return;
                    }
                }
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.CadetBlue;

                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(1100, 370);

                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(720, 5);
                lblSearch.AutoSize = true;
                lblSearch.Text = "Search by RefNo";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(850, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInEmpGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                POPIHeaderDB popiDB = new POPIHeaderDB();
                grdRefSel = popiDB.getGridViewForGivenListOfItems("POSERVICEINWARD");
                grdRefSel.Bounds = new Rectangle(new Point(0, 27), new Size(1100, 300));
                grdRefSel.Columns["ProdValue"].Visible = false;
                grdRefSel.Columns["TaxAmount"].Visible = false;
                grdRefSel.Columns["ProjectID"].Width = 150;
                grdRefSel.Columns["OfficeName"].Width = 120;
                frmPopup.Controls.Add(grdRefSel);

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
                var checkedRows = from DataGridViewRow r in grdRefSel.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount == 0)
                {
                    MessageBox.Show("Select one PO");
                    return;
                }
                string trlist = "";
                txtCustomer.Text = "";
                txtOffice.Text = "";
                txtProjectID.Text = "";
                foreach (var row in checkedRows)
                {
                    trlist = trlist + "POSERVICEINWARD" + ";" + row.Cells["TrackingNo"].Value.ToString() + "("
                                + Convert.ToDateTime(row.Cells["TrackingDate"].Value).ToString("yyyy-MM-dd") + ")" + Main.delimiter1 + Environment.NewLine;
                    txtCustomer.Text = txtCustomer.Text + row.Cells["CustName"].Value.ToString() + Environment.NewLine;
                    txtOffice.Text = txtOffice.Text + row.Cells["OfficeName"].Value.ToString() + Environment.NewLine;
                    txtProjectID.Text = txtProjectID.Text + row.Cells["ProjectID"].Value.ToString() + Environment.NewLine;
                }
                txtReferenceServiceInward.Text = trlist;
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
        private void txtSearch_TextChangedInEmpGridList(object sender, EventArgs e)
        {
            try
            {
                filterGridData();
            }
            catch (Exception ex)
            {

            }
        }
        private void filterGridData()
        {
            try
            {
                grdRefSel.CurrentCell = null;
                foreach (DataGridViewRow row in grdRefSel.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdRefSel.Rows)
                    {
                        if (!row.Cells["ReferenceNo"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
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
        //-----------------
        private void lvOK_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (!checkLVItemChecked("Item"))
                {
                    return;
                }
                string iolist = "";
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        iolist = iolist + itemRow.SubItems[2].Text + "(" + itemRow.SubItems[3].Text + ");";
                    }
                }
                txtReferenceServiceInward.Text = iolist;
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

        //private void listView2_ItemCheck(object sender, ItemCheckedEventArgs e)
        //{
        //    int c = lv.Items.Count;
        //    if (lv.CheckedItems.Count > 1)
        //    {
        //        MessageBox.Show("Cannot select more than one item");
        //        e.Item.Checked = false;
        //    }

        //}

        private void txtPaymentTerms_TextChanged(object sender, EventArgs e)
        {
        }
        private void btnPaymentTerm_Click(object sender, EventArgs e)
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
            //pnlAddEdit.Controls.Add(pnldgv);
            //pnldgv.BringToFront();
            //pnldgv.Visible = true;
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
                //pnldgv.Controls.Clear();
                //pnlAddEdit.Controls.Remove(pnldgv);
                //pnldgv.Visible = false;
                //btnPaymentTerm.Enabled = true;

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

                //pnldgv.Controls.Clear();
                //pnlAddEdit.Controls.Remove(pnldgv);
                ////btnPaymentTerm.Enabled = true;
                //pnldgv.Visible = false;
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
        //-----
        //comment handling procedurs and functions
        //-----
        private void btnSelectCommenters_Click(object sender, EventArgs e)
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
                            IndentServiceDB isdb = new IndentServiceDB();
                            prevish.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevish.CommentStatus);
                            prevish.ForwardUser = approverUID;
                            prevish.ForwarderList = prevish.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (isdb.forwardIndentService(prevish))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(prevish, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredIndentServiceHeader(listOption);
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
                    string reverseStr = getReverseString(prevish.ForwarderList);
                    //do forward activities
                    prevish.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevish.CommentStatus);
                    IndentServiceDB isdb = new IndentServiceDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevish.ForwarderList = reverseStr.Substring(0, ind);
                        prevish.ForwardUser = reverseStr.Substring(ind + 3);
                        prevish.DocumentStatus = prevish.DocumentStatus - 1;
                    }
                    else
                    {
                        prevish.ForwarderList = "";
                        prevish.ForwardUser = "";
                        prevish.DocumentStatus = 1;
                    }
                    if (isdb.reverseIndentService(prevish))
                    {
                        MessageBox.Show("Indent Service Request Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredIndentServiceHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnViewDocument_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevish.TemporaryNo + "-" + prevish.TemporaryDate.ToString("yyyyMMddhhmmss"));
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
        private void removeControlsPnllvPanel()
        {
            try
            {
                //foreach (Control p in pnlForwarder.Controls)
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
        private void removeControlsPaymentTermPanel()
        {
            try
            {
                //foreach (Control p in pnlForwarder.Controls)
                //    if (p.GetType() == typeof(DataGridView) || p.GetType() == typeof(Button))
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
                    string subDir = prevish.TemporaryNo + "-" + prevish.TemporaryDate.ToString("yyyyMMddhhmmss");
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
                ///handleGrdCloseButton();
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
                    tabControl1.SelectedTab = tabWOHeader;
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
                    tabControl1.SelectedTab = tabWOHeader;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                    disableTabPages();
                    tabControl1.SelectedTab = tabWOHeader;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabWOHeader;
                }
                else if (btnName == "LoadDocument")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                }


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
        //void handleGrdCloseButton()
        //{
        //    grdList.Columns["Close"].Visible = false;
        //    //if any one of add and edit
        //    if (Main.itemPriv[1] || Main.itemPriv[2])
        //    {
        //        //list option 1 should not have close buttuon visible (close is avialable)
        //        if (listOption != 1)
        //        {
        //            grdList.Columns["Close"].Visible = true;
        //        }
        //    }
        //}
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
                /////chkCommentStatus.Checked = false;
                txtComments.Text = "";
                grdWODetail.Rows.Clear();
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
        //private void btnSelectTermsAndCond_Click(object sender, EventArgs e)
        //{
        //    ////btnSelectTermsAndCond.Enabled = false;
        //    //removeControlsFromPnllvPanel();
        //    //pnllv = new Panel();
        //    //pnllv.BorderStyle = BorderStyle.FixedSingle;
        //    ////btnSelectTermsAndCond.Enabled = false;
        //    //pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
        //    frmPopup = new Form();
        //    frmPopup.StartPosition = FormStartPosition.CenterScreen;
        //    frmPopup.BackColor = Color.CadetBlue;

        //    frmPopup.MaximizeBox = false;
        //    frmPopup.MinimizeBox = false;
        //    frmPopup.ControlBox = false;
        //    frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

        //    frmPopup.Size = new Size(450, 300);
        //    lv = TermsAndConditionsDB.ReferenceTermsAndConditionSelectionView();
        //    string[] strArry = btnSelectTermsAndCond.Text.Split(new string[] { ";" }, StringSplitOptions.None);
        //    for (int i = 0; i < strArry.Length; i++)
        //    {
        //        try
        //        {
        //            foreach (ListViewItem itemRow in lv.Items)
        //            {
        //                if (itemRow.SubItems[1].Text.Trim().Equals(strArry[i]))
        //                {
        //                    itemRow.Checked = true;
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //        }
        //    }
        //    //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView2_ItemCheck);
        //    //lv.MultiSelect = false;
        //    lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
        //    frmPopup.Controls.Add(lv);

        //    Button lvOK = new Button();
        //    lvOK.BackColor = Color.Tan;
        //    lvOK.Text = "OK";
        //    lvOK.Location = new Point(40, 265);
        //    lvOK.Click += new System.EventHandler(this.lvOK_Clicked2);
        //    frmPopup.Controls.Add(lvOK);

        //    Button lvCancel = new Button();
        //    lvCancel.BackColor = Color.Tan;
        //    lvCancel.Text = "CANCEL";
        //    lvCancel.Location = new Point(130, 265);
        //    lvCancel.Click += new System.EventHandler(this.lvCancel_Clicked2);
        //    frmPopup.Controls.Add(lvCancel);
        //    frmPopup.ShowDialog();
        //    //pnlAddEdit.Controls.Add(pnllv);
        //    //pnllv.BringToFront();
        //    //pnllv.Visible = true;
        //}
        //private void lvOK_Clicked2(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //pnllv.Visible = false;
        //        //btnSelectTermsAndCond.Enabled = true;
        //        ////ArrayList lviItemsArrayList = new ArrayList();
        //        string tclist = "";
        //        foreach (ListViewItem itemRow in lv.Items)
        //        {
        //            if (itemRow.Checked)
        //            {
        //                tclist = tclist + itemRow.SubItems[1].Text + ";";
        //            }
        //        }
        //        txtTermsAndCond.Text = tclist;
        //        frmPopup.Close();
        //        frmPopup.Dispose();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //private void lvCancel_Clicked2(object sender, EventArgs e)
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

        private void tabWOHeader_Click(object sender, EventArgs e)
        {

        }

        private void txtExchangeRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdWODetail.Rows.Count != 0 && txtServiceValue.Text.Length != 0
                    && txtTotalValue.Text.Length != 0 && txtExchangeRate.Text.Length != 0)
                {
                    double dd = Convert.ToDouble(txtExchangeRate.Text);
                    txtServiceValueINR.Text = (Convert.ToDouble(txtServiceValue.Text) * dd).ToString();
                    txtTotalValueINR.Text = (Convert.ToDouble(txtTotalValue.Text) * dd).ToString();
                    txtTaxAmountINR.Text = (Convert.ToDouble(txtTaxAmount.Text) * dd).ToString();
                }
                if (txtExchangeRate.Text.Length == 0)
                {
                    txtServiceValueINR.Text = "";
                    txtTotalValueINR.Text = "";
                    txtTaxAmountINR.Text = "";
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            removeControlsPaymentTermPanel();
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
                        dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevish.TemporaryNo + "-" + prevish.TemporaryDate.ToString("yyyyMMddhhmmss"));
                        dgvDocumentList.Size = new Size(870, 300);
                        pnlPDFViewer.Controls.Add(dgvDocumentList);
                        dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
                        if (prevish.Status == 1 && prevish.DocumentStatus == 99)
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
        //-- Validating Header and Detail String For Single Quotes

        private indentserviceheader verifyHeaderInputString(indentserviceheader ish)
        {
            try
            {
                ish.Remarks = Utilities.replaceSingleQuoteWithDoubleSingleQuote(ish.Remarks);
                ish.ReferenceInternalOrder = Utilities.replaceSingleQuoteWithDoubleSingleQuote(ish.ReferenceInternalOrder);
                ish.CustomerID = Utilities.replaceSingleQuoteWithDoubleSingleQuote(ish.CustomerID);
            }
            catch (Exception ex)
            {
            }
            return ish;
        }
        private void verifyDetailInputString()
        {
            try
            {
                for (int i = 0; i < grdWODetail.Rows.Count; i++)
                {
                    grdWODetail.Rows[i].Cells["Item"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdWODetail.Rows[i].Cells["Item"].Value.ToString());
                    grdWODetail.Rows[i].Cells["WorkDescription"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdWODetail.Rows[i].Cells["WorkDescription"].Value.ToString());
                    grdWODetail.Rows[i].Cells["WorkLocation"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdWODetail.Rows[i].Cells["WorkLocation"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }




        private void btnSelectContractor_Click(object sender, EventArgs e)
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

                frmPopup.Size = new Size(510, 370);

                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(150, 5);
                lblSearch.Text = "Search by Name";
                lblSearch.AutoSize = true;
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(280, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtCustSearch_TextChangedInEmpGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                CustomerDB custDB = new CustomerDB();
                grdCustList = custDB.getGridViewForCustomerList("Contractor");
                grdCustList.Bounds = new Rectangle(new Point(0, 27), new Size(510, 300));
                frmPopup.Controls.Add(grdCustList);

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grdCustOK_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 335);
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
                    MessageBox.Show("Select one item");
                    return;
                }
                string trlist;
                trlist = "";
                foreach (var row in checkedRows)
                {
                    trlist = trlist + row.Cells["CustomerID"].Value.ToString() + " - "
                                + row.Cells["CustomerName"].Value + ";" + Environment.NewLine;
                }
                txtContractor.Text = trlist;

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
                        if (!row.Cells["CustomerName"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
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
        private void btnPAFService_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtReferenceServiceInward.Text.Length != 0)
                {
                    DialogResult dialog = MessageBox.Show("Inident service Detail will removed ?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        txtReferenceServiceInward.Text = "";
                        txtCustomer.Text = "";
                        txtProjectID.Text = "";
                        txtOffice.Text = "";
                        grdWODetail.Rows.Clear();
                    }
                    else
                    {
                        return;
                    }
                }
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.CadetBlue;

                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(1100, 370);

                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(720, 5);
                lblSearch.AutoSize = true;
                lblSearch.Text = "Search by RefNo";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(850, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInEmpGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                POPIHeaderDB popiDB = new POPIHeaderDB();
                grdRefSel = popiDB.getGridViewForGivenListOfItems("PAFSERVICEINWARD");
                grdRefSel.Bounds = new Rectangle(new Point(0, 27), new Size(1100, 300));
                grdRefSel.Columns["ProdValue"].Visible = false;
                grdRefSel.Columns["TaxAmount"].Visible = false;
                grdRefSel.Columns["ProjectID"].Width = 150;
                grdRefSel.Columns["OfficeName"].Width = 120;
                frmPopup.Controls.Add(grdRefSel);

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grdOK_PAFClick1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 335);
                lvCancel.Click += new System.EventHandler(this.grdCancel_PAFClick1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdOK_PAFClick1(object sender, EventArgs e)
        {
            try
            {
                var checkedRows = from DataGridViewRow r in grdRefSel.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount == 0)
                {
                    MessageBox.Show("Select one PO");
                    return;
                }
                string trlist = "";
                txtCustomer.Text = "";
                txtOffice.Text = "";
                txtProjectID.Text = "";
                foreach (var row in checkedRows)
                {
                    trlist = trlist + "PAFSERVICEINWARD" + ";" + row.Cells["TrackingNo"].Value.ToString() + "("
                                + Convert.ToDateTime(row.Cells["TrackingDate"].Value).ToString("yyyy-MM-dd") + ")" + Main.delimiter1 + Environment.NewLine;

                    txtCustomer.Text = txtCustomer.Text + row.Cells["CustName"].Value.ToString() + Environment.NewLine;
                    txtOffice.Text = txtOffice.Text + row.Cells["OfficeName"].Value.ToString() + Environment.NewLine;
                    txtProjectID.Text = txtProjectID.Text + row.Cells["ProjectID"].Value.ToString() + Environment.NewLine;

                }
                txtReferenceServiceInward.Text = trlist;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        private void grdCancel_PAFClick1(object sender, EventArgs e)
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

        private void grdList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    grdList.Rows[e.RowIndex].Selected = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSelServiceDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtReferenceServiceInward.Text.Trim().Length == 0)
                {
                    MessageBox.Show("select ref Service Inward.");
                    return;
                }
                ColSelStr = "";
                showItemDetailsForSelectedService();

            }
            catch (Exception ex)
            {
            }
        }
        private ListView getLVForAllSelectedPO(string fullRefTrackStr)
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
                lvTot.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("RefNo", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TempNo", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TempDate", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TrackingNo", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TrackingDate", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("StockItemID", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("stockItemName", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Cust.ItemDescription", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Location", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Quantity", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Price", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("WarrantyDays", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TaxCode", -2, HorizontalAlignment.Left);
                string[] mainStr = fullRefTrackStr.Trim().Split(Main.delimiter1);
                foreach (string str in mainStr)
                {
                    if (str.Length != 0)
                    {
                        string[] strRef = str.Trim().Split(';');
                        string DocIDStr = strRef[0]; //DocID
                        int trackNo1 = Convert.ToInt32(strRef[1].Substring(0, strRef[1].IndexOf('('))); //TrackNo
                        int findex = strRef[1].IndexOf('(');
                        int sindex = strRef[1].IndexOf(')');
                        string tstr = strRef[1].Substring(findex + 1, (sindex - findex) - 1);
                        DateTime trackDate1 = Convert.ToDateTime(tstr); //TrackDate

                        ListView lv =
                          POPIHeaderDB.getPONoWiseStockListViewForIndentService(trackNo1, trackDate1, DocIDStr);
                        lvTot.Items.AddRange((from ListViewItem item in lv.Items
                                              select (ListViewItem)item.Clone()).ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception in showing PODetail Listview.Check Reference Tracking No. ");
            }
            return lvTot;
        }
        private void showItemDetailsForSelectedService()
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(1100, 370);

            Label lblSearch = new Label();
            lblSearch.Location = new System.Drawing.Point(720, 5);
            lblSearch.AutoSize = true;
            lblSearch.Text = "Search Here";
            lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
            lblSearch.ForeColor = Color.Black;
            frmPopup.Controls.Add(lblSearch);

            txtSearchDet = new TextBox();
            txtSearchDet.Size = new Size(220, 18);
            txtSearchDet.Location = new System.Drawing.Point(850, 3);
            txtSearchDet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
            txtSearchDet.ForeColor = Color.Black;
            txtSearchDet.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInDetailList);
            txtSearchDet.TabIndex = 0;
            txtSearchDet.Focus();
            frmPopup.Controls.Add(txtSearchDet);

            POPIHeaderDB popidb = new POPIHeaderDB();
            grdSelPODetail = popidb.getPONoWiseStockGridViewForIndentService(txtReferenceServiceInward.Text.Trim());
            grdSelPODetail.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdSelPODetail_ColumnHeaderMouseDoubleClick);
            grdSelPODetail.Bounds = new Rectangle(new Point(0, 27), new Size(1100, 300));
            frmPopup.Controls.Add(grdSelPODetail);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(20, 335);
            lvOK.Click += new System.EventHandler(this.lvOK_ClickItem);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(110, 335);
            lvCancel.Click += new System.EventHandler(this.lvCancel_ClickItem);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
        }
        private void lvOK_ClickItem(object sender, EventArgs e)
        {
            try
            {
                var checkedRows = from DataGridViewRow r in grdSelPODetail.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount == 0)
                {
                    MessageBox.Show("Select one Item");
                    return;
                }
                foreach (var row in checkedRows)
                {
                    AddGridRowsForSelectedItem(row);
                }
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_ClickItem(object sender, EventArgs e)
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
        private void txtSearch_TextChangedInDetailList(object sender, EventArgs e)
        {
            try
            {
                filterGridDetailData();
            }
            catch (Exception ex)
            {

            }
        }
        private void filterGridDetailData()
        {
            try
            {
                foreach (DataGridViewRow row in grdSelPODetail.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearchDet.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdSelPODetail.Rows)
                    {
                        if (ColSelStr == null || ColSelStr.Length == 0)
                        {
                            if (!row.Cells["stockItemName"].Value.ToString().Trim().ToLower().Contains(txtSearchDet.Text.Trim().ToLower()))
                            {
                                row.Visible = false;
                            }
                        }
                        else
                        {

                            if (!row.Cells[ColSelStr].FormattedValue.ToString().Trim().ToLower().Contains(txtSearchDet.Text.Trim().ToLower()))
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
        private void grdSelPODetail_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                ///txtSearchDet.Text = "";
                ColSelStr = grdSelPODetail.Columns[e.ColumnIndex].Name;
                foreach (DataGridViewColumn col in grdSelPODetail.Columns)
                {
                    col.HeaderCell.Style.BackColor = Color.LightSeaGreen;
                }
                grdSelPODetail.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdSelPODetail.Columns[e.ColumnIndex].HeaderCell.Style.BackColor = Color.Magenta;
            }
            catch (Exception ex)
            {
            }
        }
        private void AddGridRowsForSelectedItem(DataGridViewRow row)
        {
            try
            {
                grdWODetail.Rows.Add();
                int kount = grdWODetail.RowCount;
                grdWODetail.Rows[kount - 1].Cells[0].Value = kount;
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdWODetail.Rows[kount - 1].Cells["gTCode"]);
                TaxCodeDB.fillTaxCodeGridViewCombo(ComboColumn2, "");
                grdWODetail.Rows[kount - 1].Cells["POItemRefNo"].Value = Convert.ToInt32(0);
                grdWODetail.Rows[kount - 1].Cells["WorkDescription"].Value = " ";
                grdWODetail.Rows[kount - 1].Cells["WorkLocation"].Value = "";
                grdWODetail.Rows[kount - 1].Cells["Quantity"].Value = Convert.ToDouble(0);
                grdWODetail.Rows[kount - 1].Cells["Price"].Value = Convert.ToDouble(0);
                grdWODetail.Rows[kount - 1].Cells["Rate"].Value = Convert.ToDouble(0);
                grdWODetail.Rows[kount - 1].Cells["Value"].Value = Convert.ToDouble(0);
                grdWODetail.Rows[kount - 1].Cells["Tax"].Value = Convert.ToDouble(0);
                grdWODetail.Rows[kount - 1].Cells["WarrantyDays"].Value = 0;
                grdWODetail.Rows[kount - 1].Cells["TaxDetails"].Value = " ";

                grdWODetail.Rows[kount - 1].Cells["Item"].Value = row.Cells["StockItemID"].Value.ToString()
                                                                                    + "-" + row.Cells["stockItemName"].Value.ToString();
                grdWODetail.Rows[kount - 1].Cells["POItemRefNo"].Value = Convert.ToInt32(row.Cells["RefNo"].Value);
                grdWODetail.Rows[kount - 1].Cells["WorkDescription"].Value = row.Cells["Cust.ItemDescription"].Value;
                grdWODetail.Rows[kount - 1].Cells["WorkLocation"].Value = row.Cells["Location"].Value;
                grdWODetail.Rows[kount - 1].Cells["Quantity"].Value = Convert.ToDouble(row.Cells["Quantity"].Value);
                grdWODetail.Rows[kount - 1].Cells["Price"].Value = Convert.ToDouble(row.Cells["Price"].Value);
                grdWODetail.Rows[kount - 1].Cells["gTCode"].Value = row.Cells["TaxCode"].Value.ToString();
                grdWODetail.Rows[kount - 1].Cells["WarrantyDays"].Value = Convert.ToInt32(row.Cells["WarrantyDays"].Value);
                grdWODetail.Rows[kount - 1].Cells["Value"].Value = Convert.ToDouble(0);
                grdWODetail.Rows[kount - 1].Cells["Tax"].Value = Convert.ToDouble(0);

            }
            catch (Exception ex)
            {
            }
        }

        private void IndentService_Enter(object sender, EventArgs e)
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
                showPopUpForContractorRef(txtContractorRef.Text.Trim());
            }
            catch (Exception ex)
            {
            }
        }
        private void showPopUpForContractorRef(string str)
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
            head.Text = "Fill Text Below";
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
                    MessageBox.Show("Text is empty");
                    return;
                }
                txtContractorRef.Text = txtDesc.Text.Trim();
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


        //Gridview for PODetails
        private ListView getGridViewForAllSelectedPO(string fullRefTrackStr)
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
                lvTot.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("RefNo", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TempNo", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TempDate", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TrackingNo", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TrackingDate", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("StockItemID", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("stockItemName", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Cust.ItemDescription", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Location", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Quantity", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Price", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("WarrantyDays", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TaxCode", -2, HorizontalAlignment.Left);
                string[] mainStr = fullRefTrackStr.Trim().Split(Main.delimiter1);
                foreach (string str in mainStr)
                {
                    if (str.Length != 0)
                    {
                        string[] strRef = str.Trim().Split(';');
                        string DocIDStr = strRef[0]; //DocID
                        int trackNo1 = Convert.ToInt32(strRef[1].Substring(0, strRef[1].IndexOf('('))); //TrackNo
                        int findex = strRef[1].IndexOf('(');
                        int sindex = strRef[1].IndexOf(')');
                        string tstr = strRef[1].Substring(findex + 1, (sindex - findex) - 1);
                        DateTime trackDate1 = Convert.ToDateTime(tstr); //TrackDate

                        ListView lv =
                          POPIHeaderDB.getPONoWiseStockListViewForIndentService(trackNo1, trackDate1, DocIDStr);
                        lvTot.Items.AddRange((from ListViewItem item in lv.Items
                                              select (ListViewItem)item.Clone()).ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception in showing PODetail Listview.Check Reference Tracking No. ");
            }
            return lvTot;
        }
    }

}

