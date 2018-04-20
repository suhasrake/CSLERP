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
    public partial class IndentStationery : System.Windows.Forms.Form
    {
        string docID = "INDENTSTATIONERY";
        string forwarderList = "";
        string approverList = "";
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        //double productvalue = 0.0;
        //double taxvalue = 0.0;
        indentheader previheader;
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        ////Boolean captureChange = false;
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        TreeView tv = new TreeView();
        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        double productvalue = 0.0;
        double taxvalue = 0.0;
        Boolean userIsACommenter = false;
        Timer filterTimer1 = new Timer();
        Panel pnlModel = new Panel();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        DataGridView grdStock = new DataGridView();
        Form frmPopup = new Form();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        TextBox txtSearch = new TextBox();
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        int descClickRowIndex = -1;
        RichTextBox txtDesc = new RichTextBox();
        Boolean AddRowClick = false;
        Boolean isViewMode = false;
        DataGridView grdCustList = new DataGridView();
        DataGridView grdPOList = new DataGridView();
        Dictionary<string, string[]> dictItemWiseTOt = new Dictionary<string, string[]>();
        string colClicked = "";
        Boolean isNewClick = false;
        public IndentStationery()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void IndentHeader_Load(object sender, EventArgs e)
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
            ListFilteredIndentHeader(listOption);
            //applyPrivilege();
        }

        private void ListFilteredIndentHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                IndentHeaderDB ihdb = new IndentHeaderDB();

                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                List<indentheader> IHeaders = ihdb.getFilteredIndentHeader(userString, option, userCommentStatusString).Where(ind => ind.DocumentID == docID).ToList();
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                grdList.Columns["POPrint"].Visible = false;
                foreach (indentheader ih in IHeaders)
                {
                    if (option == 1)
                    {
                        if (ih.DocumentStatus == 99)
                            continue;
                    }
                    grdList.Rows.Add();

                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = ih.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = ih.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = ih.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = ih.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gIndentNo"].Value = ih.IndentNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gIndentDate"].Value = ih.IndentDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gReferenceinternalOrders"].Value = ih.ReferenceInternalOrders;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProductCodes"].Value = ih.ProductCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTargetDate"].Value = ih.TargetDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["PurchaseSource"].Value = ih.PurchaseSource;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyID"].Value = ih.CurrencyID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyName"].Value = ih.CurrencyName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gExchangeRate"].Value = ih.ExchangeRate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProductValue"].Value = ih.ProductValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProductValueINR"].Value = ih.ProductValueINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["gRemarks"].Value = ih.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = ih.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = ih.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gAcceptanceStatus"].Value = ih.AcceptanceStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = ih.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = ih.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreator"].Value = ih.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gForwarder"].Value = ih.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gApprover"].Value = ih.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ApproveUser"].Value = ih.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ComntStatus"].Value = ih.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = ih.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwarderLists"].Value = ih.ForwarderList;
                    grdList.Rows[grdList.RowCount - 1].Cells["ClosingStatus"].Value = ih.ClosingStatus;
                    if (ih.ClosingStatus == 1)
                    {
                        grdList.Rows[grdList.RowCount - 1].DefaultCellStyle.BackColor = Color.Tan;
                    }
                    if (option == 6 || option == 3)
                    {
                        grdList.Columns["POPrint"].Visible = true;
                        if (ih.POno == 0)
                        {
                            grdList.Rows[grdList.RowCount - 1].Cells["POPrint"].Style.BackColor = Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Indent Header Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;
            isNewClick = false;
            isViewMode = false;
            grdList.Columns["gCreator"].Visible = true;
            grdList.Columns["gForwarder"].Visible = true;
            grdList.Columns["gApprover"].Visible = true;

            //Main.itemPriv[0]: View
            //Main.itemPriv[1]: Add
            //Main.itemPriv[2]: Edit
            //Main.itemPriv[3]: Delete
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

        private void initVariables()
        {

            if (getuserPrivilegeStatus() == 1)
            {
                //user is only a viewer
                listOption = 6;
            }
            fillStatusCombo(cmbStatus);
            //StockItemDB.fillStockItemCombo(cmbProductCodes, "Products");
            CurrencyDB.fillCurrencyComboNew(cmbCurrency);
            cmbCurrency.SelectedIndex =
                    Structures.ComboFUnctions.getComboIndex(cmbCurrency, "INR");
            txtExchangeRate.Text = "1";
            dtTemporaryDate.Format = DateTimePickerFormat.Custom;
            dtTemporaryDate.CustomFormat = "dd-MM-yyyy";
            dtTemporaryDate.Enabled = false;
            dtIndentDate.Format = DateTimePickerFormat.Custom;
            dtIndentDate.CustomFormat = "dd-MM-yyyy";
            dtIndentDate.Enabled = false;
            dtTargetDate.Format = DateTimePickerFormat.Custom;
            dtTargetDate.CustomFormat = "dd-MM-yyyy";

            txtTemporaryNo.Enabled = false;
            txtIndentNo.Enabled = false;
            btnForward.Visible = false;
            btnApprove.Visible = false;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            grdIndentDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            lv.Visible = false;
            txtComments.Text = "";

            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
            TaxDetailsTable.Columns.Add("TaxItem", typeof(string));
            TaxDetailsTable.Columns.Add("TaxAmount", typeof(double));
            setTabIndex();
        }
        private void setTabIndex()
        {
            txtTemporaryNo.TabIndex = 0;
            dtTemporaryDate.TabIndex = 1;
            txtIndentNo.TabIndex = 2;
            dtIndentDate.TabIndex = 3;
            dtTargetDate.TabIndex = 4;
            cmbCurrency.TabIndex = 7;
            txtExchangeRate.TabIndex = 8;
            txtProductCodes.TabIndex = 9;
            btnSelectProductCodes.TabIndex = 10;
            txtPurchaseSource.TabIndex = 11;
            btnPurchaseSource.TabIndex = 12;
            txtRemarks.TabIndex = 13;

            btnForward.TabIndex = 0;
            btnApprove.TabIndex = 1;
            btnCancel.TabIndex = 3;
            btnSave.TabIndex = 2;
            btnReverse.TabIndex = 4;
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
                grdIndentDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                //----------clear temperory panels

                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                //----------
                cmbStatus.SelectedIndex = -1;
                cmbCurrency.SelectedIndex =
                     Structures.ComboFUnctions.getComboIndex(cmbCurrency, "INR");
                txtExchangeRate.Text = "1";
                txtProductValue.Text = "";
                txtProductValueINR.Text = "";
                grdIndentDetail.Rows.Clear();
                txtProductCodes.Text = "";
                txtTemporaryNo.Text = "";
                txtIndentNo.Text = "";
                txtPurchaseSource.Text = "";
                txtRemarks.Text = "";
                btnProdcutValue.Text = "";
                dtTemporaryDate.Value = DateTime.Parse("01-01-1900");
                dtIndentDate.Value = DateTime.Parse("01-01-1900");
                dtTargetDate.Value = DateTime.Today.Date;
                previheader = new indentheader();
                isViewMode = false;
                AddRowClick = false;
                colClicked = "";
                isNewClick = false;
                commentStatus = "";
                //removeControlsFrompnllv();
                //removeControlsFromModelPanel();
                //removeControlsFromForwarderPanelTV();
            }
            catch (Exception)
            {

            }
        }
        private void btnExit_Click_1(object sender, EventArgs e)
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
        private void btnNew_Click_1(object sender, EventArgs e)
        {
            try
            {
                clearData();
                btnSave.Text = "Save";
                pnlAddEdit.Visible = true;
                closeAllPanels();
                pnlAddEdit.Visible = true;
                tabControl1.SelectedTab = tabIndentHeader;
                //cmbPOType.Enabled = true;
                setButtonVisibility("btnNew");

                isViewMode = false;
                AddRowClick = false;
                isNewClick = true;
            }
            catch (Exception)
            {

            }
        }
        //private void btnNew_Click(object sender, EventArgs e)
        //{

        //}


        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                AddIndentDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddIndentDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdIndentDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkIndentDetailGridRows())
                    {
                        return false;
                    }
                }
                grdIndentDetail.Rows.Add();
                int kount = grdIndentDetail.RowCount;
                grdIndentDetail.Rows[kount - 1].Cells[0].Value = kount;
                grdIndentDetail.Rows[kount - 1].Cells["Item"].Value = "";
                grdIndentDetail.Rows[kount - 1].Cells["ModelNo"].Value = "";
                grdIndentDetail.Rows[kount - 1].Cells["ModelName"].Value = "";
                grdIndentDetail.Rows[kount - 1].Cells["ModelDetails"].Value = "";
                grdIndentDetail.Rows[kount - 1].Cells["LastPurchasePrice"].Value = 0;
                grdIndentDetail.Rows[kount - 1].Cells["QuotedPrice"].Value = 0;
                grdIndentDetail.Rows[kount - 1].Cells["ExpectedPurchasePrice"].Value = 0;
                grdIndentDetail.Rows[kount - 1].Cells["QuotationNo"].Value = 0;
                //grdIndentDetail.Rows[kount - 1].Cells["QuotationNo"].ReadOnly = true;
                grdIndentDetail.Rows[kount - 1].Cells["QuantityRequired"].Value = 0;
                grdIndentDetail.Rows[kount - 1].Cells["Stock"].Value = 0;
                grdIndentDetail.Rows[kount - 1].Cells["BufferQuantity"].Value = 0;
                grdIndentDetail.Rows[kount - 1].Cells["WarrantyDays"].Value = 0;
                var BtnCell = (DataGridViewButtonCell)grdIndentDetail.Rows[kount - 1].Cells["Delete"];
                BtnCell.Value = "Del";
                if (AddRowClick)
                {
                    grdIndentDetail.FirstDisplayedScrollingRowIndex = grdIndentDetail.RowCount - 1;
                    grdIndentDetail.CurrentCell = grdIndentDetail.Rows[kount - 1].Cells[0];
                }
                grdIndentDetail.Columns[0].Frozen = false;
                grdIndentDetail.FirstDisplayedScrollingColumnIndex = 0;
                grdIndentDetail.Columns["selDesc"].Frozen = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddIndentDetailRow() : Error");
            }

            return status;
        }
        private void btnCancel_Click_2(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }

        //private void btnCancel_Click(object sender, EventArgs e)
        //{
        //    clearData();
        //    closeAllPanels();
        //    pnlList.Visible = true;
        //    setButtonVisibility("btnEditPanel");
        //    //pnlBottomActions.Visible = true;
        //}

        private Boolean verifyAndReworkIndentDetailGridRows()
        {
            Boolean status = true;
            double total = 0;
            try
            {
                if (grdIndentDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Indent details");
                    btnProdcutValue.Text = "";
                    btnProductValueINR.Text = "";
                    txtProductValueINR.Text = "";
                    txtProductValue.Text = "";
                    return false;
                }
                if (!isViewMode && (txtExchangeRate.Text.Trim().Length == 0 || Convert.ToDouble(txtExchangeRate.Text.Trim()) == 0))
                {
                    MessageBox.Show("Fill Exchange Rate");
                    return false;
                }
                //clear tax details table
                TaxDetailsTable.Rows.Clear();
                for (int i = 0; i < grdIndentDetail.Rows.Count; i++)
                {

                    grdIndentDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    double qr = Convert.ToDouble(grdIndentDetail.Rows[i].Cells["QuantityRequired"].Value);
                    double st = Convert.ToDouble(grdIndentDetail.Rows[i].Cells["Stock"].Value);
                    double bq = Convert.ToDouble(grdIndentDetail.Rows[i].Cells["BufferQuantity"].Value);
                    double qtbp = qr + bq - st;
                    grdIndentDetail.Rows[i].Cells["QtyToBeProcured"].Value = qtbp;

                    if ((grdIndentDetail.Rows[i].Cells["Item"].Value == null) ||
                        (grdIndentDetail.Rows[i].Cells["ModelNo"].Value.ToString().Length == 0) ||
                        (grdIndentDetail.Rows[i].Cells["ModelName"].Value.ToString().Length == 0) ||
                        (grdIndentDetail.Rows[i].Cells["ModelDetails"].Value == null) ||
                        (Convert.ToDouble(grdIndentDetail.Rows[i].Cells["ExpectedPurchasePrice"].Value) == 0) ||
                        (qr <= 0) || (qtbp <= 0))

                    {
                        MessageBox.Show("Check values in row " + (i + 1));
                        return false;
                    }
                    double expPrice = Convert.ToDouble(grdIndentDetail.Rows[i].Cells["ExpectedPurchasePrice"].Value);
                    grdIndentDetail.Rows[i].Cells["Value"].Value = qtbp * expPrice;
                    total = total + (qtbp * expPrice);
                }
                btnProdcutValue.Text = total.ToString();
                btnProductValueINR.Text = (total * Convert.ToDouble(txtExchangeRate.Text.Trim())).ToString();
                txtProductValueINR.Text = (total * Convert.ToDouble(txtExchangeRate.Text.Trim())).ToString();
                txtProductValue.Text = total.ToString();
                if (!validateItems() && !isViewMode)
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
                for (int i = 0; i < grdIndentDetail.Rows.Count - 1; i++)
                {
                    for (int j = i + 1; j < grdIndentDetail.Rows.Count; j++)
                    {

                        if (grdIndentDetail.Rows[i].Cells["Item"].Value.ToString() == grdIndentDetail.Rows[j].Cells["Item"].Value.ToString() &&
                            grdIndentDetail.Rows[i].Cells["ModelDetails"].Value.ToString() == grdIndentDetail.Rows[j].Cells["ModelDetails"].Value.ToString())
                        {
                            //duplicate item code
                            MessageBox.Show("Item duplicated in Indent details... please ensure correctness (" +
                                grdIndentDetail.Rows[i].Cells["Item"].Value.ToString() + ")");
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

        private List<indentdetail> getIndentDetails(indentheader ih)
        {
            List<indentdetail> IDetails = new List<indentdetail>();
            try
            {
                indentdetail id = new indentdetail();

                for (int i = 0; i < grdIndentDetail.Rows.Count; i++)
                {
                    id = new indentdetail();
                    id.DocumentID = ih.DocumentID;
                    id.TemporaryNo = ih.TemporaryNo;
                    id.TemporaryDate = ih.TemporaryDate;
                    id.StockItemID = grdIndentDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Substring(0, grdIndentDetail.Rows[i].Cells["Item"].Value.ToString().Trim().IndexOf('-'));

                    id.LastPurchasedPrice = Convert.ToDouble(grdIndentDetail.Rows[i].Cells["LastPurchasePrice"].Value);
                    id.ModelDetails = grdIndentDetail.Rows[i].Cells["ModelDetails"].Value.ToString();
                    id.ModelNo = grdIndentDetail.Rows[i].Cells["ModelNo"].Value.ToString();
                    id.QuotedPrice = Convert.ToDouble(grdIndentDetail.Rows[i].Cells["QuotedPrice"].Value);
                    id.ExpectedPurchasePrice = Convert.ToDouble(grdIndentDetail.Rows[i].Cells["ExpectedPurchasePrice"].Value);
                    id.QuotationNo = grdIndentDetail.Rows[i].Cells["QuotationNo"].Value.ToString();
                    id.Quantity = Convert.ToDouble(grdIndentDetail.Rows[i].Cells["QuantityRequired"].Value);
                    id.Stock = Convert.ToDouble(grdIndentDetail.Rows[i].Cells["Stock"].Value);
                    id.BufferQuantity = Convert.ToDouble(grdIndentDetail.Rows[i].Cells["BufferQuantity"].Value);
                    id.WarrantyDays = Convert.ToInt32(grdIndentDetail.Rows[i].Cells["WarrantyDays"].Value);
                    IDetails.Add(id);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("getIndentDetails() : Error getting Indent Details");
                IDetails = null;
            }
            return IDetails;
        }

        private void btnActionPending_Click_1(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredIndentHeader(listOption);
        }

        private void btnApprovalPending_Click_1(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredIndentHeader(listOption);
        }

        //private void btnApproved_Click(object sender, EventArgs e)
        //{

        //}

        private void btnApproved_Click_1(object sender, EventArgs e)
        {
            if (getuserPrivilegeStatus() == 2)
            {
                listOption = 6; //viewer
            }
            else
            {
                listOption = 3;
            }

            ListFilteredIndentHeader(listOption);
        }
        ////private void btnSave_Click_1(object sender, EventArgs e)
        ////{

        ////}

        ////private void btnCancel_Click_1(object sender, EventArgs e)
        ////{

        ////}

        private void btnAddLine_Click_1(object sender, EventArgs e)
        {
            AddIndentDetailRow();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {

                IndentHeaderDB idb = new IndentHeaderDB();
                indentheader ih = new indentheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    if (!verifyAndReworkIndentDetailGridRows())
                    {
                        return;
                    }
                    ih.DocumentID = docID;
                    ih.IndentDate = dtIndentDate.Value;
                    ih.TargetDate = dtTargetDate.Value;
                    //ih.ReferenceInternalOrders = txtReferenceInternalNo.Text;
                    ih.ProductCode = txtProductCodes.Text;
                    ih.Remarks = txtRemarks.Text;
                    ih.PurchaseSource = txtPurchaseSource.Text;
                    ih.Comments = docCmtrDB.DGVtoString(dgvComments).Replace("'", "''");
                    ih.ForwarderList = previheader.ForwarderList;
                    ih.ProductValue = Convert.ToDouble(txtProductValue.Text);
                    ih.ProductValueINR = Convert.ToDouble(txtProductValueINR.Text);
                    ih.CurrencyID = ((Structures.ComboBoxItem)cmbCurrency.SelectedItem).HiddenValue;
                    ih.ExchangeRate = Convert.ToDouble(txtExchangeRate.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }

                if (!idb.validateIndentHeader(ih, 2))
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    //ih.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    ih.DocumentStatus = 1; //created
                    ih.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    ih.TemporaryNo = Convert.ToInt32(txtTemporaryNo.Text);
                    ih.TemporaryDate = previheader.TemporaryDate;
                    ih.DocumentStatus = previheader.DocumentStatus;
                }
                //Replacing single quotes
                ih = verifyHeaderInputString(ih);
                verifyDetailInputString();
                if (idb.validateIndentHeader(ih, 2))
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
                            ih.CommentStatus = docCmtrDB.createCommentStatusString(previheader.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            ih.CommentStatus = previheader.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            ih.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            ih.CommentStatus = previheader.CommentStatus;
                        }
                    }
                    //----------
                    int tmpStatus = 1;
                    if (txtComments.Text.Trim().Length > 0)
                    {
                        ih.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus).Replace("'", "''");
                    }
                    List<indentdetail> INDList = getIndentDetails(ih);
                    if (btnText.Equals("Update"))
                    {
                        if (idb.updateindHeaderAndDetail(ih, previheader, INDList))
                        {
                            MessageBox.Show("Indent Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredIndentHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update indent Header");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (idb.InsertIndHeaderAndDetail(ih, INDList))
                        {
                            MessageBox.Show("Indent Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredIndentHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert Indent");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Indent Header Validation failed");
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("createAndUpdateIHDetails() : Error");
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
                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("LoadDocument") || columnName == "Print" ||
                    columnName.Equals("View") || columnName == "CloseRequest" || columnName == "Close" || columnName == "POPrint" )
                {
                    clearData();
                    setButtonVisibility(columnName);
                    colClicked = columnName;
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    IndentHeaderDB idb = new IndentHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    previheader = new indentheader();
                    AddRowClick = false;
                    previheader.CommentStatus = grdList.Rows[e.RowIndex].Cells["ComntStatus"].Value.ToString();
                    previheader.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    previheader.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    previheader.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    previheader.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    if (previheader.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    if (columnName == "View")
                    {
                        isViewMode = true;
                        tabControl1.TabPages["tabIndentDetail"].Enabled = true;
                        //tabControl1.TabPages["tabIODetails"].Enabled = true;
                    }
                    previheader.Comments = IndentHeaderDB.getUserComments(previheader.DocumentID, previheader.TemporaryNo, previheader.TemporaryDate);
                    previheader.IndentNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gIndentNo"].Value.ToString());
                    previheader.IndentDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gIndentDate"].Value.ToString());

                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Document Temp No:" + previheader.TemporaryNo + "\n" +
                            "Document Temp Date:" + previheader.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "Indent No:" + previheader.IndentNo + "\n" +
                            "Indent Date:" + previheader.IndentDate.ToString("dd-MM-yyyy");
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = previheader.TemporaryNo + "-" + previheader.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_2(null, null);
                        return;
                    }
                    //--------

                    previheader.ReferenceInternalOrders = grdList.Rows[e.RowIndex].Cells["gReferenceInternalOrders"].Value.ToString();
                    previheader.ProductCode = grdList.Rows[e.RowIndex].Cells["gProductCodes"].Value.ToString();
                    previheader.TargetDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTargetDate"].Value.ToString());
                    previheader.Remarks = grdList.Rows[e.RowIndex].Cells["gRemarks"].Value.ToString();
                    previheader.PurchaseSource = grdList.Rows[e.RowIndex].Cells["PurchaseSource"].Value.ToString();
                    previheader.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    previheader.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    previheader.AcceptanceStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gAcceptanceStatus"].Value.ToString());
                    previheader.ClosingStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["ClosingStatus"].Value.ToString());
                    previheader.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    previheader.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    previheader.CreatorName = grdList.Rows[e.RowIndex].Cells["gCreator"].Value.ToString();
                    previheader.ForwarderName = grdList.Rows[e.RowIndex].Cells["gForwarder"].Value.ToString();
                    previheader.ApproverName = grdList.Rows[e.RowIndex].Cells["gApprover"].Value.ToString();
                    previheader.ApproveUser = grdList.Rows[e.RowIndex].Cells["ApproveUser"].Value.ToString();
                    previheader.ForwarderList = grdList.Rows[e.RowIndex].Cells["ForwarderLists"].Value.ToString();

                    previheader.ProductValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gProductValue"].Value.ToString());
                    previheader.ProductValueINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gProductValueINR"].Value.ToString());
                    previheader.ExchangeRate = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gExchangeRate"].Value.ToString());
                    previheader.CurrencyID = grdList.Rows[e.RowIndex].Cells["gCurrencyID"].Value.ToString();
                    if (columnName == "Print")
                    {
                        PrinttempIndent tempPrint = new PrinttempIndent();
                        List<indentdetail> InDetail = IndentHeaderDB.getIndentDetail(previheader);
                        tempPrint.PrintIndent(previheader, InDetail);
                        btnExit.Visible = true;
                        return;
                    }
                    //--comments
                    ///chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    previheader.CommentStatus = grdList.Rows[e.RowIndex].Cells["ComntStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(previheader.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(previheader.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---------------
                    if (columnName == "CloseRequest")
                    {
                        if (previheader.ClosingStatus == 1)
                        {
                            MessageBox.Show("Previous closing request is pending.");
                            btnExit.Visible = true;
                            return;
                        }
                        showCLosingRequestPopUp();
                        ListFilteredIndentHeader(listOption);
                        return;
                    }
                    if (columnName == "Close")
                    {
                        if (previheader.ApproveUser != Login.userLoggedIn)
                        {
                            MessageBox.Show("Only approve user can close Indent Product.");
                            btnExit.Visible = true;
                            return;
                        }
                        if (previheader.ClosingStatus == 0)
                        {
                            MessageBox.Show("No request for closing this Indent Product.");
                            btnExit.Visible = true;
                            return;
                        }
                        showpopUpForCLosing();
                        ListFilteredIndentHeader(listOption);
                        return;
                    }
                    //----------

                    txtExchangeRate.Text = previheader.ExchangeRate.ToString();
                    txtProductValue.Text = previheader.ProductValue.ToString();
                    txtProductValueINR.Text = previheader.ProductValueINR.ToString();
                    cmbCurrency.SelectedIndex =
                       Structures.ComboFUnctions.getComboIndex(cmbCurrency, previheader.CurrencyID);

                    txtTemporaryNo.Text = previheader.TemporaryNo.ToString();
                    dtTemporaryDate.Value = previheader.TemporaryDate;
                    txtIndentNo.Text = previheader.IndentNo.ToString();
                    try
                    {
                        dtIndentDate.Value = previheader.IndentDate;
                    }
                    catch (Exception)
                    {
                        dtIndentDate.Value = DateTime.Parse("01-01-1900");
                    }
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(ComboFIll.getStatusString(Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString())));
                    txtProductCodes.Text = previheader.ProductCode;
                    //txtReferenceInternalNo.Text = previheader.ReferenceInternalOrders;
                    try
                    {
                        dtTargetDate.Value = previheader.TargetDate;
                    }
                    catch (Exception)
                    {
                        dtTargetDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtRemarks.Text = previheader.Remarks.ToString();
                    txtPurchaseSource.Text = previheader.PurchaseSource.ToString();
                    List<indentdetail> IDetail = IndentHeaderDB.getIndentDetail(previheader);
                    grdIndentDetail.Rows.Clear();
                    int i = 0;
                    foreach (indentdetail id in IDetail)
                    {
                        if (!AddIndentDetailRow())
                        {
                            MessageBox.Show("Error found in Indent details. Please correct before updating the details");
                        }
                        else
                        {
                            grdIndentDetail.Rows[i].Cells["Item"].Value = id.StockItemID + "-" + id.StockItemName;
                            grdIndentDetail.Rows[i].Cells["ModelDetails"].Value = id.ModelDetails;
                            grdIndentDetail.Rows[i].Cells["ModelNo"].Value = id.ModelNo;
                            grdIndentDetail.Rows[i].Cells["ModelName"].Value = id.ModelName;

                            grdIndentDetail.Rows[i].Cells["LastPurchasePrice"].Value = id.LastPurchasedPrice;
                            grdIndentDetail.Rows[i].Cells["QuotedPrice"].Value = id.QuotedPrice;
                            grdIndentDetail.Rows[i].Cells["ExpectedPurchasePrice"].Value = id.ExpectedPurchasePrice;
                            grdIndentDetail.Rows[i].Cells["QuotationNo"].Value = id.QuotationNo;
                            grdIndentDetail.Rows[i].Cells["QuantityRequired"].Value = id.Quantity;
                            grdIndentDetail.Rows[i].Cells["Stock"].Value = id.Stock;
                            grdIndentDetail.Rows[i].Cells["BufferQuantity"].Value = id.BufferQuantity;
                            grdIndentDetail.Rows[i].Cells["WarrantyDays"].Value = id.WarrantyDays;
                            i++;
                        }

                    }
                    if (!verifyAndReworkIndentDetailGridRows())
                    {
                        MessageBox.Show("Error found in Indent details. Please correct before updating the details");
                    }
                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                    tabControl1.SelectedTab = tabIndentHeader;
                    tabControl1.Visible = true;

                    if (columnName.Equals("POPrint"))
                    {
                        //PrintPurchaseOrder ppo = new PrintPurchaseOrder();
                        pnlAddEdit.Visible = false;
                        pnlList.Visible = true;
                        grdList.Visible = true;
                        btnNew.Visible = true;
                        btnExit.Visible = true;
                        List<poheader> pohdr = IndentHeaderDB.getPurchaseOrderHeader(previheader);
                        if (pohdr.Count == 1)
                        {
                            poheader phdt = pohdr.FirstOrDefault();
                            //CSLERP.PrintForms.PrintPurchaseOrder ppo = new CSLERP.PrintForms.PrintPurchaseOrder();
                            PrintPurchaseOrder ppo = new PrintPurchaseOrder();
                            poheader poh = new poheader();

                            List<podetail> PODetails = PurchaseOrderDB.getPurchaseOrderDetails(phdt);
                            getTaxDetails(PODetails);
                            string taxstr = getTasDetailStr();
                            ppo.PrintPO(phdt, PODetails, taxstr);
                            btnNew.Visible = true;
                            btnExit.Visible = true;
                            return;
                        }
                        else if (pohdr.Count < 1)
                        {
                            MessageBox.Show("PO not prepared!!!");
                            return;
                        }
                        else if (pohdr.Count > 1)
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

                                IndentHeaderDB IndnthdrDB = new IndentHeaderDB();
                                grdPOList = getGridViewForPreparedPOList(pohdr);
                                grdPOList.Bounds = new Rectangle(new Point(0, 0), new Size(800, 360));
                                frmPopup.Controls.Add(grdPOList);

                                Button lvCancel = new Button();
                                lvCancel.Text = "CANCEL";
                                lvCancel.BackColor = Color.Tan;
                                lvCancel.Location = new System.Drawing.Point(10, 370);
                                lvCancel.Click += new System.EventHandler(this.grdPOCancel_Click1);
                                frmPopup.Controls.Add(lvCancel);
                                frmPopup.ShowDialog();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                setButtonVisibility("init");
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
                    string prevComment = previheader.Comments;
                    int tmpStatus = 1;
                    string newComments = docCmtrDB.processNewComment(dgvComments, comment, Login.userLoggedIn, Login.userLoggedInName, tmpStatus);

                    if (IndentHeaderDB.RequestTOCloseIOHeader(previheader, newComments))
                    {
                        MessageBox.Show("Closing request sent");
                        ListFilteredIndentHeader(listOption);
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
                if (IndentHeaderDB.CloseIOHeader(previheader))
                {
                    MessageBox.Show("Internal Order closed.");
                    ListFilteredIndentHeader(listOption);

                }
                else
                {
                    MessageBox.Show("Fail to close Internal Order");
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
                string prevComment = previheader.Comments;
                int tmpStatus = 1;
                string newComments = docCmtrDB.processNewComment(dgvComments, comment, Login.userLoggedIn, Login.userLoggedInName, tmpStatus);

                if (IndentHeaderDB.RejectClosingRequest(previheader, newComments))
                {
                    MessageBox.Show("Closing request rejected.");
                    ListFilteredIndentHeader(listOption);
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
        //-------------------

        private void btnClearEntries_Click_1(object sender, EventArgs e)
        {
            try
            {
                for (int i = grdIndentDetail.Rows.Count - 1; i >= 0; i--)
                {
                    grdIndentDetail.Rows.RemoveAt(i);
                }
            }
            catch (Exception)
            {
            }
            MessageBox.Show("item cleared.");
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            AddRowClick = true;
            AddIndentDetailRow();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Are you sure to clear all entries in grid detail ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdIndentDetail.Rows.Clear();
                    MessageBox.Show("Grid items cleared.");
                    verifyAndReworkIndentDetailGridRows();
                }
            }
            catch (Exception)
            {
            }
        }
        private void btnForward_Click(object sender, EventArgs e)
        {
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
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                IndentHeaderDB idb = new IndentHeaderDB();
                //indentheader popih = new indentheader();
                FinancialLimitDB flDB = new FinancialLimitDB();
                if (!flDB.checkEmployeeFinancialLimit(docID, Login.empLoggedIn, Convert.ToDouble(txtProductValue.Text), 0))
                {
                    MessageBox.Show("No financial power for approving this document");
                    return;
                }
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    previheader.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(previheader.CommentStatus);
                    if (previheader.Status != 96)
                    {
                        previheader.IndentNo = DocumentNumberDB.getNewNumber(docID, 2);
                    }
                    if (idb.ApproveIndentHeader(previheader))
                    {
                        MessageBox.Show("Indent Document Approved");
                        if (!updateDashBoard(previheader, 2))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                        }
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredIndentHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception)
            {
            }
        }


        private void grdIndentDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdIndentDetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdIndentDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkIndentDetailGridRows();
                    }
                    if (columnName.Equals("SelectQuotation"))
                    {
                        if (grdIndentDetail.Rows[e.RowIndex].Cells["Item"].Value.ToString().Length == 0)
                        {
                            MessageBox.Show("select item.");
                            return;
                        }
                        string stid = grdIndentDetail.Rows[e.RowIndex].Cells["Item"].Value.ToString();
                        stid = stid.Substring(0, stid.IndexOf('-'));
                        ShowQuotationDetails(stid);
                    }
                    if (columnName.Equals("Sel"))
                    {
                        showStockDataGridView();
                    }
                    if (columnName.Equals("SelDesc"))
                    {
                        descClickRowIndex = e.RowIndex;
                        string strTest = grdIndentDetail.Rows[e.RowIndex].Cells["ModelDetails"].Value.ToString().Trim();
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
                    MessageBox.Show("Details is empty");
                    return;
                }
                grdIndentDetail.Rows[descClickRowIndex].Cells["ModelDetails"].Value = txtDesc.Text.Trim();
                grdIndentDetail.FirstDisplayedScrollingRowIndex = grdIndentDetail.Rows[descClickRowIndex].Index;
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
        //-----

        private Boolean checkAvailabilityOfitem()
        {
            Boolean status = true;
            if (grdIndentDetail.CurrentRow.Cells["Item"].Value.ToString().Length != 0 ||
                grdIndentDetail.CurrentRow.Cells["ModelNo"].Value.ToString().Length != 0 ||
                grdIndentDetail.CurrentRow.Cells["ModelName"].Value.ToString().Length != 0)
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
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInStkGridList);
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
                grdIndentDetail.CurrentRow.Cells["Item"].Value = iolist;
                ///showModelListView(iolist);
                frmPopup.Close();
                frmPopup.Dispose();
                showModelListView(iolist);
                string id = grdIndentDetail.CurrentRow.Cells["Item"].Value.ToString();
                string modelNo = grdIndentDetail.CurrentRow.Cells["ModelNo"].Value.ToString();
                grdIndentDetail.CurrentRow.Cells["LastPurchasePrice"].Value = StockDB.getLastPurchasePriceForMRN(id, modelNo);
                grdIndentDetail.CurrentRow.Cells["Stock"].Value = StockDB.getTotalStockForIndent(id, modelNo);

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
        private void txtSearch_TextChangedInStkGridList(object sender, EventArgs e)
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
            filterGridDataStk();
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }

        private void filterGridDataStk()
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

        //private void showStockItemTreeView()
        //{
        //    //removeControlsFromForwarderPanelTV();
        //    if (!checkAvailabilityOfitem())
        //    {
        //        DialogResult dialog = MessageBox.Show("Selected product and Model detail will removed?", "Yes", MessageBoxButtons.YesNo);
        //        if (dialog == DialogResult.Yes)
        //        {
        //            grdIndentDetail.CurrentRow.Cells["Item"].Value = "";
        //            grdIndentDetail.CurrentRow.Cells["ModelNo"].Value = "";
        //            grdIndentDetail.CurrentRow.Cells["ModelName"].Value = "";
        //        }
        //        else
        //            return;
        //    }
        //    frmPopup = new Form();
        //    frmPopup.StartPosition = FormStartPosition.CenterScreen;
        //    frmPopup.BackColor = Color.CadetBlue;

        //    frmPopup.MaximizeBox = false;
        //    frmPopup.MinimizeBox = false;
        //    frmPopup.ControlBox = false;
        //    frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

        //    frmPopup.Size = new Size(450, 310);
        //    tv = new TreeView();
        //    tv.CheckBoxes = true;
        //    tv.Nodes.Clear();
        //    tv.CheckBoxes = true;
        //    // pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
        //    //pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
        //    Label lbl = new Label();
        //    lbl.AutoSize = true;
        //    lbl.Location = new Point(5, 5);
        //    lbl.Size = new Size(300, 20);
        //    lbl.Text = "Tree View For Product";
        //    lbl.Font = new Font("Serif", 10, FontStyle.Bold);
        //    lbl.ForeColor = Color.Black;
        //    frmPopup.Controls.Add(lbl);
        //    tv = StockItemDB.getStockItemTreeView();
        //    tv.Bounds = new Rectangle(new Point(0, 25), new Size(450, 250));
        //    //pnlForwarder.Controls.Remove(tv);
        //    frmPopup.Controls.Add(tv);
        //    //tv.cl
        //    tv.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterCheck);
        //    Button lvForwrdOK = new Button();
        //    lvForwrdOK.Text = "OK";
        //    lvForwrdOK.BackColor = Color.Tan;
        //    lvForwrdOK.Location = new System.Drawing.Point(40, 280);
        //    lvForwrdOK.Click += new System.EventHandler(this.tvOK_Click);
        //    frmPopup.Controls.Add(lvForwrdOK);

        //    Button lvForwardCancel = new Button();
        //    lvForwardCancel.Text = "CANCEL";
        //    lvForwardCancel.BackColor = Color.Tan;
        //    lvForwardCancel.Location = new System.Drawing.Point(130, 280);
        //    lvForwardCancel.Click += new System.EventHandler(this.tvCancel_Click);
        //    frmPopup.Controls.Add(lvForwardCancel);
        //    frmPopup.ShowDialog();
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
        //            grdIndentDetail.CurrentRow.Cells["Item"].Value = s;
        //            tv.CheckBoxes = true;
        //            frmPopup.Close();
        //            frmPopup.Dispose();
        //            //pnlForwarder.Controls.Remove(tv);
        //            //pnlForwarder.Visible = false;
        //            showModelListView(s);
        //            string id = grdIndentDetail.CurrentRow.Cells["Item"].Value.ToString();
        //            string modelNo = grdIndentDetail.CurrentRow.Cells["ModelNo"].Value.ToString();
        //            grdIndentDetail.CurrentRow.Cells["LastPurchasePrice"].Value = StockDB.getLastPurchasePriceForMRN(id, modelNo);
        //            grdIndentDetail.CurrentRow.Cells["Stock"].Value = StockDB.getTotalStockForIndent(id, modelNo);
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
        //        frmPopup.Close();
        //        frmPopup.Dispose();
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
        //private void removeControlsFromForwarderPanelTV()
        //{
        //    try
        //    {
        //        //foreach (Control p in pnlForwarder.Controls)
        //        //    if (p.GetType() == typeof(TreeView) || p.GetType() == typeof(Button) || p.GetType() == typeof(ListView))
        //        //    {
        //        //        p.Dispose();
        //        //    }
        //        pnlForwarder.Controls.Clear();
        //        Control nc = pnlForwarder.Parent;
        //        nc.Controls.Remove(pnlForwarder);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
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
                grdIndentDetail.CurrentRow.Cells["ModelNo"].Value = "NA";
                grdIndentDetail.CurrentRow.Cells["ModelName"].Value = "NA";
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
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click1);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
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
                        grdIndentDetail.CurrentRow.Cells["ModelNo"].Value = item.SubItems[1].Text;
                        grdIndentDetail.CurrentRow.Cells["ModelName"].Value = item.SubItems[2].Text;

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
        private void removeControlsFrompnllv()
        {
            try
            {
                //foreach (Control p in pnlModel.Controls)
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
        //-----



        private void btnCalculate_Click(object sender, EventArgs e)
        {
            verifyAndReworkIndentDetailGridRows();
        }
        private void ShowQuotationDetails(string stockID)
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            lv = QIHeaderDB.QIPriceSelectionView(stockID);
            ////this.lv.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listView1_ItemCheck);
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);

            lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            frmPopup.Controls.Add(lv);

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
            frmPopup.ShowDialog();
            //pnlAddEdit.Controls.Add(pnllv);
            //pnllv.BringToFront();
            //pnllv.Visible = true;
        }
        private void lvOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkLVItemChecked("Quotation"))
                {
                    return;
                }
                string trlist = "";
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        //MessageBox.Show(itemRow.SubItems[1].Text);
                        trlist = trlist + itemRow.SubItems[1].Text + "(" + itemRow.SubItems[2].Text + ");";
                        grdIndentDetail.CurrentRow.Cells["QuotationNo"].Value = itemRow.SubItems[2].Text;
                        grdIndentDetail.CurrentRow.Cells["QuotedPrice"].Value = itemRow.SubItems[6].Text;
                    }
                }
                frmPopup.Close();
                frmPopup.Dispose();
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
        private void lv_ItemCheck1(object sender, System.Windows.Forms.ItemCheckEventArgs e)
        {
            try
            {
                if (e.CurrentValue == CheckState.Unchecked)
                {
                    MessageBox.Show("Test 1");
                }
                else if ((e.CurrentValue == CheckState.Checked))
                {
                    MessageBox.Show("Test 2");
                }
            }
            catch (Exception)
            {
            }
        }


        private void btnSelectProductCodes_Click(object sender, EventArgs e)
        {
            if (txtProductCodes.Text.Trim().Length != 0)
            {
                DialogResult dialog = MessageBox.Show("Selected Product Codes Will be removed ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    txtProductCodes.Text = "";
                }
                else
                    return;
            }
            showEmployeeDataGridView(txtProductCodes.Text);
        }

        //showing gridview for stockitem
        private void showEmployeeDataGridView(string selectedCode)
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
                if (selectedRowCount == 0)
                {
                    MessageBox.Show("Select Any Product");
                    return;
                }
                foreach (var row in checkedRows)
                {
                    iolist = iolist + row.Cells["StockItemID"].Value.ToString() + "-" + row.Cells["Name"].Value.ToString() + ";" + Environment.NewLine;
                }
                txtProductCodes.Text = iolist;
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
        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //-----
        //comment handling procedurs and functions
        //-----
        private void btnSelectCommenters_Click(object sender, EventArgs e)
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
            lvOK.Click += new System.EventHandler(this.lvCmntOK_Click);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCmntCancel_Click);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
        }
        private void lvCmntOK_Click(object sender, EventArgs e)
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

        private void lvCmntCancel_Click(object sender, EventArgs e)
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
                            IndentHeaderDB idb = new IndentHeaderDB();
                            previheader.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(previheader.CommentStatus);
                            previheader.ForwardUser = approverUID;
                            previheader.ForwarderList = previheader.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (idb.forwardIndentHeader(previheader))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(previheader, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredIndentHeader(listOption);
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
        private Boolean updateDashBoard(indentheader ih, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = ih.DocumentID;
                dsb.TemporaryNo = ih.TemporaryNo;
                dsb.TemporaryDate = ih.TemporaryDate;
                dsb.DocumentNo = ih.IndentNo;
                dsb.DocumentDate = ih.IndentDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = ih.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(previheader.DocumentID);
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
                    string reverseStr = getReverseString(previheader.ForwarderList);
                    //do forward activities
                    previheader.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(previheader.CommentStatus);
                    IndentHeaderDB idb = new IndentHeaderDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        previheader.ForwarderList = reverseStr.Substring(0, ind);
                        previheader.ForwardUser = reverseStr.Substring(ind + 3);
                        previheader.DocumentStatus = previheader.DocumentStatus - 1;
                    }
                    else
                    {
                        previheader.ForwarderList = "";
                        previheader.ForwardUser = "";
                        previheader.DocumentStatus = 1;
                    }
                    if (idb.reverseIndentHeader(previheader))
                    {
                        MessageBox.Show("Indent Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredIndentHeader(listOption);
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
        //private void btnPDFClose_Click(object sender, EventArgs e)
        //{

        //}
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
        private void btnListDocument_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, previheader.TemporaryNo + "-" + previheader.TemporaryDate.ToString("yyyyMMddhhmmss"));
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
        //private void btnListDocuments_Click(object sender, EventArgs e)
        //{

        //}

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
                    string subDir = previheader.TemporaryNo + "-" + previheader.TemporaryDate.ToString("yyyyMMddhhmmss");
                    ////DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    dgv.Enabled = false;
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
                    tabControl1.SelectedTab = tabIndentHeader;
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
                    /////chkCommentStatus.Visible = true;
                    txtComments.Visible = true;
                    tabControl1.SelectedTab = tabIndentHeader;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                    disableTabPages();
                    tabControl1.SelectedTab = tabIndentHeader;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabIndentHeader;
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
                grdIndentDetail.Rows.Clear();
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
        //-- Validating Header and Detail String For Single Quotes

        private indentheader verifyHeaderInputString(indentheader ih)
        {
            try
            {
                ih.Remarks = Utilities.replaceSingleQuoteWithDoubleSingleQuote(ih.Remarks);
                ih.PurchaseSource = Utilities.replaceSingleQuoteWithDoubleSingleQuote(ih.PurchaseSource);
            }
            catch (Exception ex)
            {
            }
            return ih;
        }
        private void verifyDetailInputString()
        {
            try
            {
                for (int i = 0; i < grdIndentDetail.Rows.Count; i++)
                {
                    grdIndentDetail.Rows[i].Cells["Item"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdIndentDetail.Rows[i].Cells["Item"].Value.ToString());
                    grdIndentDetail.Rows[i].Cells["ModelDetails"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdIndentDetail.Rows[i].Cells["ModelDetails"].Value.ToString());
                    //grdIndentDetail.Rows[i].Cells["WorkLocation"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdIndentDetail.Rows[i].Cells["WorkLocation"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                        dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, previheader.TemporaryNo + "-" + previheader.TemporaryDate.ToString("yyyyMMddhhmmss"));
                        dgvDocumentList.Size = new Size(870, 300);
                        pnlPDFViewer.Controls.Add(dgvDocumentList);
                        dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
                        if (previheader.Status == 1 && previheader.DocumentStatus == 99)
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

        private void grdIndentDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            //    {
            //        grdIndentDetail.Rows[e.RowIndex].Selected = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
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

        private void btnPurchaseSource_Click(object sender, EventArgs e)
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
                grdCustList = custDB.getGridViewForCustomerList("Supplier");
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
                if (selectedRowCount == 0)
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
                txtPurchaseSource.Text = trlist;
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

        private void grdPOCancel_Click1(object sender, EventArgs e)
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

        private void txtExchangeRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtExchangeRate.Text.Trim().Length != 0 && btnProdcutValue.Text.Length != 0 && grdIndentDetail.Rows.Count != 0
                                           && Convert.ToDouble(txtExchangeRate.Text.Trim()) != 0)
                {
                    double total = Convert.ToDouble(btnProdcutValue.Text);
                    btnProdcutValue.Text = total.ToString();
                    btnProductValueINR.Text = (total * Convert.ToDouble(txtExchangeRate.Text.Trim())).ToString();
                    txtProductValueINR.Text = (total * Convert.ToDouble(txtExchangeRate.Text.Trim())).ToString();
                    txtProductValue.Text = total.ToString();
                }
                //else if (Convert.ToDouble(txtExchangeRate.Text.Trim()) == 0)
                //{
                //    btnProdcutValue.Text = "0";
                //    btnProductValueINR.Text = "0";
                //    txtProductValueINR.Text = "0";
                //    txtProductValue.Text = "0";
                //}
            }
            catch (Exception ex)
            {
            }

        }



        public DataGridView getGridViewForPreparedPOList(List<poheader> pohdr)
        {
            DataGridView grdpo = new DataGridView();
            try
            {


                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdpo.EnableHeadersVisualStyles = false;
                grdpo.AllowUserToAddRows = false;
                grdpo.AllowUserToDeleteRows = false;
                grdpo.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdpo.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdpo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdpo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdpo.ColumnHeadersHeight = 27;
                grdpo.RowHeadersVisible = false;
                grdpo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grdpo.Columns.Clear();
                grdpo.Columns.Add("PODOcumentID", "PO DocumentID");
                grdpo.Columns.Add("PONo", "PONo");
                grdpo.Columns.Add("PODate", "PO Date");
                grdpo.Columns.Add("CustomerID", "CustomerID");
                grdpo.Columns.Add("Customer", "Customer");
                grdpo.Columns.Add("TemporaryNo", "TemporaryNo");
                grdpo.Columns.Add("TemporaryDate", "TemporaryDate");
                grdpo.Columns["TemporaryDate"].DefaultCellStyle.Format = "dd-MM-yyyy";
                grdpo.Columns["PODate"].DefaultCellStyle.Format = "dd-MM-yyyy";
                DataGridViewButtonColumn btncol = new DataGridViewButtonColumn();
                btncol.Name = "POPrint";
                btncol.Text = "PO Print";
                grdpo.Rows.Clear();
                btncol.UseColumnTextForButtonValue = true;
                grdpo.Columns.Add(btncol);
                foreach (poheader phr in pohdr)
                {
                    grdpo.Rows.Add();
                    grdpo.Rows[grdpo.RowCount - 1].Cells["PODocumentID"].Value = phr.DocumentID;
                    grdpo.Rows[grdpo.RowCount - 1].Cells["PONo"].Value = phr.PONo;
                    grdpo.Rows[grdpo.RowCount - 1].Cells["PODate"].Value = phr.PODate.Date;
                    grdpo.Rows[grdpo.RowCount - 1].Cells["CustomerID"].Value = phr.CustomerID;
                    grdpo.Rows[grdpo.RowCount - 1].Cells["Customer"].Value = phr.CustomerName;
                    grdpo.Rows[grdpo.RowCount - 1].Cells["TemporaryNo"].Value = phr.TemporaryNo;
                    grdpo.Rows[grdpo.RowCount - 1].Cells["TemporaryDate"].Value = phr.TemporaryDate.Date;
                    grdpo.Rows[grdpo.RowCount - 1].ReadOnly = true;
                }

                grdpo.CellContentClick += (s, e) =>
                 {
                     if (e.RowIndex < 0)
                     {
                         return;
                     }
                     string col = grdpo.Columns[e.ColumnIndex].Name;

                     if (col == "POPrint")
                     {
                         poheader pohTemp = new poheader();
                         pohTemp.DocumentID = grdpo.Rows[e.RowIndex].Cells["PODocumentID"].Value.ToString();
                         pohTemp.TemporaryNo = Convert.ToInt32(grdpo.Rows[e.RowIndex].Cells["TemporaryNo"].Value);
                         pohTemp.TemporaryDate = Convert.ToDateTime(grdpo.Rows[e.RowIndex].Cells["TemporaryDate"].Value);

                         poheader poh = pohdr.FirstOrDefault(po => po.DocumentID == pohTemp.DocumentID && po.TemporaryNo == pohTemp.TemporaryNo &&
                                                                    po.TemporaryDate == pohTemp.TemporaryDate);
                         if(poh == null)
                         {
                             MessageBox.Show("Unable to get PO Header detail.");
                             return;
                         }

                         PrintPurchaseOrder ppo = new PrintPurchaseOrder();
                         List<podetail> PODetails = PurchaseOrderDB.getPurchaseOrderDetails(poh);
                         getTaxDetails(PODetails);
                         string taxstr = getTasDetailStr();
                         ppo.PrintPO(poh, PODetails, taxstr);
                         btnNew.Visible = true;
                         btnExit.Visible = true;
                         frmPopup.Close();
                         frmPopup.Dispose();
                     }
                 };
            }
            catch (Exception ex)
            {
            }
            return grdpo;
        }

        private void IndentStationery_Activated(object sender, EventArgs e)
        {
            MessageBox.Show("IndentStationery_Activated");
        }

        private void IndentStationery_Enter(object sender, EventArgs e)
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

