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
    public partial class POINStatus : System.Windows.Forms.Form
    {
        string docID = "";
        string forwarderList = "";
        string approverList = "";
        string trackerList = "";
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        double productvalue = 0.0;
        double taxvalue = 0.0;
        popiheader prevpopi = new popiheader();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        DataGridView dgvpt; // grid view for Payment Terms
        ////Boolean captureChange = false;
        public POINStatus()
        {
            try
            {
                InitializeComponent();
                this.FormBorderStyle = FormBorderStyle.None;
                Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
                initVariables();
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                this.Width -= 100;
                this.Height -= 100;
                this.FormBorderStyle = FormBorderStyle.Fixed3D;
                String a = this.Size.ToString();
                grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdList.EnableHeadersVisualStyles = false;
                ListFilteredStockOBHeader(1);
                applyPrivilege(3);
            }
            catch (Exception)
            {

            }
        }

        private void ListFilteredStockOBHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                POPIHeaderDB popihdb = new POPIHeaderDB();
                forwarderList = demDB.getForwarders(docID, Main.empLoggedIn);
                approverList = demDB.getApprovers(docID, Main.empLoggedIn);
               
                List<popiheader> POPIHeaders = popihdb.getFilteredPOPIHeader(forwarderList, option);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3)
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
                        if (!(popih.CreateUser.Equals(Main.userLoggedIn) ||
                            popih.ForwardUser.Equals(Main.userLoggedIn) ||
                            popih.ApproveUser.Equals(Main.userLoggedIn)))
                        {
                            //if not relevent to the user looged in
                            continue;
                        }
                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = popih.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = popih.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = popih.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = popih.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTrackingNo"].Value = popih.TrackingNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTrackingDate"].Value = popih.TrackingDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerID"].Value = popih.CustomerID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerName"].Value = popih.CustomerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerPONo"].Value = popih.CustomerPONO;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerPODate"].Value = popih.CustomerPODate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDeliveryDate"].Value = popih.DeliveryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPaymentTerms"].Value = popih.PaymentTerms;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPaymentMode"].Value = popih.PaymentMode;
                    grdList.Rows[grdList.RowCount - 1].Cells["gFreightTerms"].Value = popih.FreightTerms;
                    grdList.Rows[grdList.RowCount - 1].Cells["gFreightCharge"].Value = popih.FreightCharge;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyID"].Value = popih.CurrencyID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTaxCode"].Value = popih.TaxCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["gBillingAddress"].Value = popih.BillingAddress;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDeliveryAddress"].Value = popih.DeliveryAddress;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProductValue"].Value = popih.ProductValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTaxAmount"].Value = popih.TaxAmount;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPOValue"].Value = popih.POValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["gRemarks"].Value = popih.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = popih.status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = popih.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = popih.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = popih.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = popih.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = popih.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatusNo"].Value = ComboFIll.getDocumentStatusString(popih.DocumentStatus);
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = popih.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatusNo"].Value = popih.DocumentStatus;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in PO Product Inward Listing");
            }
            try
            {
                if (option == 1)
                {
                    grdList.Columns["Edit"].Visible = true;
                    grdList.Columns["Approve"].Visible = true;
                }
                else
                {
                    grdList.Columns["Edit"].Visible = false;
                    grdList.Columns["Approve"].Visible = false;
                }
                grdList.Columns["Creator"].Visible = true;
                grdList.Columns["Forwarder"].Visible = true;
                grdList.Columns["Approver"].Visible = true;
                enableBottomButtons();
                pnlList.Visible = true;
            }
            catch (Exception ex)
            {
            }
        }

        private void initVariables()
        {
            docID = Main.currentDocument;
            trackerList = drDB.getTrackerss(docID, Main.empLoggedIn);

            CurrencyDB.fillCurrencyCombo(cmbCurrency);
            //CatalogueValueDB.fillCatalogValueCombo(cmbPaymentTerms, "PaymentTerms");
            CatalogueValueDB.fillCatalogValueCombo(cmbPaymentMode, "PaymentMode");
            CatalogueValueDB.fillCatalogValueCombo(cmbFreightTerms, "Freight");
            CatalogueValueDB.fillCatalogValueCombo(cmbPOType, "POType");
            CustomerDB.fillCustomerCombo(cmbCustomer);
            TaxCodeDB.fillTaxCodeCombo(cmbTaxCode);
            ////CatalogueValueDB.fillCatalogValueListBox(lstServiceItems, "ServiceLookup");
            
            dtTempDate.Format = DateTimePickerFormat.Custom;
            dtTempDate.CustomFormat = "dd-MM-yyyy";
            dtTempDate.Enabled = false;
            dtTrackDate.Format = DateTimePickerFormat.Custom;
            dtTrackDate.CustomFormat = "dd-MM-yyyy";
            dtTrackDate.Enabled = false;
            dtCustomerPODate.Format = DateTimePickerFormat.Custom;
            dtCustomerPODate.CustomFormat = "dd-MM-yyyy";
            ////dtCustomerPODate.Enabled = false;
            dtdeliveryDate.Format = DateTimePickerFormat.Custom;
            dtdeliveryDate.CustomFormat = "dd-MM-yyyy";
            ////dtdeliveryDate.Enabled = false;
            txtPaymentTerms.Enabled = false;
            txtTemporarryNo.Enabled = false;
            dtTempDate.Enabled = false;
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
            cmbCustomer.TabIndex = 0;
            txtCustomerPONo.TabIndex = 1;
            dtCustomerPODate.TabIndex = 2;
            dtdeliveryDate.TabIndex = 3;
           // cmbPaymentTerms.TabIndex = 4;
            cmbCurrency.TabIndex = 4;
            cmbTaxCode.TabIndex = 5;
            grdPOPIDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
        private void applyPrivilege(int opt)
        {
            try
            {
                if (opt == 1 || opt == 3)
                {
                    if (Main.itemPriv[1])
                    {
                        btnNew.Visible = true;
                    }
                    else
                    {
                        btnNew.Visible = false;
                    }
                }
                if (opt == 2 || opt == 3)
                {
                    if (Main.itemPriv[2])
                    {
                        grdList.Columns["Edit"].Visible = true;
                    }
                    else
                    {
                        grdList.Columns["Edit"].Visible = false;
                    }
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
                cmbTaxCode.SelectedIndex = -1;
                cmbPaymentMode.SelectedIndex = -1;
                cmbCustomer.SelectedIndex = -1;
                cmbCurrency.SelectedIndex = -1;
                try
                {
                    cmbFreightTerms.SelectedItem = null;
                }
                catch (Exception ex)
                {
                }
                cmbPOType.SelectedItem = null;
                grdPOPIDetail.Rows.Clear();
                txtTemporarryNo.Text = "";
                dtTempDate.Value = DateTime.Parse("01-01-1900");
                txtTrackingNo.Text = "";
                dtTrackDate.Value = DateTime.Parse("01-01-1900");
                txtCustomerPONo.Text = "";
                dtCustomerPODate.Value = DateTime.Today.Date;
                dtdeliveryDate.Value = DateTime.Today.Date;
                ////dtCustomerPODate.Value = DateTime.Parse("01-01-1900");
                ////dtdeliveryDate.Value = DateTime.Parse("01-01-1900");
                txtFreightCharge.Text = "";
                txtBillingAddress.Text = "";
                txtDeliveryAddress.Text = "";
                txtProductValue.Text = "";
                txtTaxAmount.Text = "";
                txtPOValue.Text = "";
                txtRemarks.Text = "";
                btnProductValue.Text = "0";
                btnTaxAmount.Text = "0";
                tabPOType.Enabled = true;
                tabPOType.Visible = true;
                tabPOHeader.Visible = false;
                tabPODetail.Visible = false;
                txtPaymentTerms.Text = "";
                //cmbPaymentTerms.ResetText();
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
                pnlBottomActions.Visible = false;
                pnlAddEdit.Visible = true;
                disableBottomButtons();
                closeAllPanels();
                pnlAddEdit.Visible = true;
                disableBottomButtons();
                tabControl1.SelectedTab = tabPOType;
                cmbPOType.Enabled = true;
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
                MessageBox.Show(ex.ToString());
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
                DataGridViewComboBoxCell ComboColumn1 = (DataGridViewComboBoxCell)(grdPOPIDetail.Rows[kount - 1].Cells["Item"]);
                if (docID == "POPRODUCTINWARD")
                {
                    StockItemDB.fillStockItemGridViewCombo(ComboColumn1, "");
                }
                else
                {
                    CatalogueValueDB.fillCatalogValueGridViewCombo(ComboColumn1, "ServiceLookup");
                }
                    
                ComboColumn1.DropDownWidth = 300;

                grdPOPIDetail.Rows[kount - 1].Cells["Quantity"].Value = 0;
                grdPOPIDetail.Rows[kount - 1].Cells["Price"].Value = 0;
                grdPOPIDetail.Rows[kount - 1].Cells["Value"].Value = 0;
                grdPOPIDetail.Rows[kount - 1].Cells["Tax"].Value = 0;
                grdPOPIDetail.Rows[kount - 1].Cells["WarrantyDays"].Value = 0;
                grdPOPIDetail.Rows[kount - 1].Cells["TaxDetails"].Value = " ";
                grdPOPIDetail.Rows[kount - 1].Cells["CustomerItemDescription"].Value = " ";
                var BtnCell = (DataGridViewButtonCell)grdPOPIDetail.Rows[kount - 1].Cells["Delete"];
                BtnCell.Value = "Del";
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddPOPIDetailRow() : Error");
            }

            return status;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ////pnlTaxCodeWorking.Visible = false;
            closeAllPanels();
            ////btnNew.Visible = true;
            applyPrivilege(1);
            btnExit.Visible = true;
            pnlList.Visible = true;
            enableBottomButtons();
            pnlBottomActions.Visible = true;
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
                    txtTaxAmount.Text = taxvalue.ToString(); //fill this later
                    txtPOValue.Text = (productvalue + taxvalue).ToString();
                    btnProductValue.Text = txtProductValue.Text;
                    return false;
                }
                //clear tax details table
                TaxDetailsTable.Rows.Clear();
                for (int i = 0; i < grdPOPIDetail.Rows.Count; i++)
                {

                    grdPOPIDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if ((grdPOPIDetail.Rows[i].Cells["Item"].Value == null) ||
                        (grdPOPIDetail.Rows[i].Cells["CustomerItemDescription"].Value == null) ||
                        (grdPOPIDetail.Rows[i].Cells["CustomerItemDescription"].Value.ToString().Trim().Length==0) ||
                        (grdPOPIDetail.Rows[i].Cells["Quantity"].Value == null) ||
                        (grdPOPIDetail.Rows[i].Cells["Price"].Value == null) ||
                        (Convert.ToDouble(grdPOPIDetail.Rows[i].Cells["Quantity"].Value) == 0) ||
                        (Convert.ToDouble(grdPOPIDetail.Rows[i].Cells["Price"].Value) == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }

                    quantity = Convert.ToDouble(grdPOPIDetail.Rows[i].Cells["Quantity"].Value);
                    price = Convert.ToDouble(grdPOPIDetail.Rows[i].Cells["Price"].Value);
                    if (price != 0 && quantity != 0)
                    {
                        cost = Math.Round(quantity * price, 2);
                    }

                    try
                    {
                        strtaxCode = cmbTaxCode.SelectedItem.ToString().Trim();
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
                    grdPOPIDetail.Rows[i].Cells["Value"].Value = Convert.ToDouble(cost.ToString());
                    grdPOPIDetail.Rows[i].Cells["Tax"].Value = ttax2.ToString();
                    grdPOPIDetail.Rows[i].Cells["TaxDetails"].Value = strTax;
                    productvalue = productvalue + cost;
                    taxvalue = taxvalue + ttax2;

                    //- rewok tax value
                }
                txtProductValue.Text = productvalue.ToString();
                txtTaxAmount.Text = taxvalue.ToString(); //fill this later
                txtPOValue.Text = (productvalue + taxvalue).ToString();
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
        private Boolean validateItems()
        {
            Boolean status = true;
            try
            {
                if (docID == "POPRODUCTINWARD")
                {

                    for (int i = 0; i < grdPOPIDetail.Rows.Count - 1; i++)
                    {
                        for (int j = i + 1; j < grdPOPIDetail.Rows.Count; j++)
                        {

                            if (grdPOPIDetail.Rows[i].Cells[1].Value.ToString() == grdPOPIDetail.Rows[j].Cells["Item"].Value.ToString())
                            {
                                //duplicate item code
                                MessageBox.Show("Item code duplicated in PO details... please ensure correctness (" +
                                    grdPOPIDetail.Rows[i].Cells["Item"].Value.ToString() + ")");
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

        private Boolean createAndUpdatePOPIDetails(popiheader popih)
        {
            Boolean status = true;
            try
            {
                POPIHeaderDB popihdb = new POPIHeaderDB();
                popidetail popid = new popidetail();

                List<popidetail> POPIDetails = new List<popidetail>();
                for (int i = 0; i < grdPOPIDetail.Rows.Count; i++)
                {
                    try
                    {
                        popid = new popidetail();
                        popid.DocumentID = popih.DocumentID;
                        popid.TemporaryNo = popih.TemporaryNo;
                        popid.TemporaryDate = popih.TemporaryDate;
                        popid.StockItemID = grdPOPIDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Substring(0, grdPOPIDetail.Rows[i].Cells["Item"].Value.ToString().Trim().IndexOf('-'));
                        popid.CustomerItemDescription = grdPOPIDetail.Rows[i].Cells["CustomerItemDescription"].Value.ToString().Trim();
                        popid.WorkDescription = " ";
                        ////if (popih.DocumentID == "POPRODUCTINWARD")
                        ////{
                        ////    popid.StockItemID = grdPOPIDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Substring(0, grdPOPIDetail.Rows[i].Cells["Item"].Value.ToString().Trim().IndexOf('-'));
                        ////    popid.CustomerItemDescription = grdPOPIDetail.Rows[i].Cells["CustomerItemDescription"].Value.ToString().Trim();
                        ////    popid.WorkDescription = " ";
                        ////}
                        ////if (popih.DocumentID == "POSERVICEINWARD")
                        ////{
                        ////    popid.StockItemID = grdPOPIDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Substring(0, grdPOPIDetail.Rows[i].Cells["Item"].Value.ToString().Trim().IndexOf('-'));
                        ////    popid.CustomerItemDescription = grdPOPIDetail.Rows[i].Cells["CustomerItemDescription"].Value.ToString().Trim();
                        ////    popid.WorkDescription = " ";
                        ////}
                        popid.Quantity = Convert.ToDouble(grdPOPIDetail.Rows[i].Cells["Quantity"].Value);
                        popid.Price = Convert.ToDouble(grdPOPIDetail.Rows[i].Cells["Price"].Value);
                        popid.Tax = Convert.ToDouble(grdPOPIDetail.Rows[i].Cells["Tax"].Value);
                        popid.WarrantyDays = Convert.ToInt32(grdPOPIDetail.Rows[i].Cells["WarrantyDays"].Value);

                        popid.TaxDetails = grdPOPIDetail.Rows[i].Cells["TaxDetails"].Value.ToString();

                        POPIDetails.Add(popid);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("createAndUpdatePOPIDetails() : Error creating PO Details");
                        status = false;
                    }
                }
                if (!popihdb.updatePOPIDetail(POPIDetails, popih))
                {
                    MessageBox.Show("createAndUpdatePOPIDetails() : Failed to update PO Details. Please check the values");
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("createAndUpdateBOMDetails() : Error updating BOM Details");
                status = false;
            }
            return status;
        }

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            applyPrivilege(3);
            ListFilteredStockOBHeader(1);
           
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            applyPrivilege(3);
            ListFilteredStockOBHeader(2);
            
        }

        private void btnApproved_Click(object sender, EventArgs e)
        {
            applyPrivilege(3);
            ListFilteredStockOBHeader(3);
           
        }

        private void btnFollowup_Click(object sender, EventArgs e)
        {
            applyPrivilege(3);
            ListFilteredStockOBHeader(4);
            
        }

        private void disableBottomButtons()
        {
            btnNew.Visible = false;
            btnExit.Visible = false;
        }

        private void enableBottomButtons()
        {
            ////btnNew.Visible = true;
            applyPrivilege(1);
            btnExit.Visible = true;
            pnlBottomActions.Visible = true;
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
                    popih.CustomerID = cmbCustomer.SelectedItem.ToString().Trim().Substring(0, cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-'));
                    popih.CustomerPONO = txtCustomerPONo.Text;
                    popih.CustomerPODate = dtCustomerPODate.Value;
                    popih.DeliveryDate = dtdeliveryDate.Value;
                    popih.PaymentTerms = txtPaymentTerms.Text;
                    popih.PaymentMode = cmbPaymentMode.SelectedItem.ToString().Trim().Substring(0, cmbPaymentMode.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
                    popih.FreightTerms = cmbFreightTerms.SelectedItem.ToString().Trim().Substring(0, cmbFreightTerms.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
                    popih.FreightCharge = Convert.ToDouble(txtFreightCharge.Text);
                    popih.CurrencyID = cmbCurrency.SelectedItem.ToString().Trim().Substring(0, cmbCurrency.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
                    popih.TaxCode = cmbTaxCode.SelectedItem.ToString().Trim();
                    popih.BillingAddress = txtBillingAddress.Text;
                    popih.DeliveryAddress = txtDeliveryAddress.Text;
                    popih.ProductValue = Convert.ToDouble(txtProductValue.Text);
                    popih.TaxAmount = Convert.ToDouble(txtTaxAmount.Text);
                    popih.POValue = Convert.ToDouble(txtPOValue.Text);
                    popih.Remarks = txtRemarks.Text;
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
                    popih.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    popih.DocumentStatus = 1; //created
                    popih.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    popih.TemporaryNo = Convert.ToInt32(txtTemporarryNo.Text);
                    popih.TemporaryDate = prevpopi.TemporaryDate;
                    popih.DocumentStatus = prevpopi.DocumentStatus;
                }

                if (popidb.validatePOPIHeader(popih))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (popidb.updatePOPIHeader(popih))
                        {
                            if (createAndUpdatePOPIDetails(popih))
                            {
                                MessageBox.Show("PO Product Inward Details updated");
                                closeAllPanels();
                                ListFilteredStockOBHeader(1);
                                pnlBottomActions.Visible = true;
                            }
                            else
                            {
                                status = false;
                            }
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
                        if (popidb.insertPOPIHeader(popih))
                        {
                            if (createAndUpdatePOPIDetails(popih))
                            {
                                MessageBox.Show("PO Product Inward Details Added");
                                closeAllPanels();
                                ListFilteredStockOBHeader(1);
                                pnlBottomActions.Visible = true;
                            }
                            else
                            {
                                status = false;
                                popidb.deletePOPIHeader(popih);
                            }
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("btnSave_Click_1() : Error");
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            pnlBottomActions.Visible = true;
            enableBottomButtons();
        }

        private void btnAddLine_Click_1(object sender, EventArgs e)
        {
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
                    ////if (columnName.Equals("Select"))
                    ////{
                    ////    //show service list lookup table
                       
                    ////    lstServiceItems.Visible = true;
                    ////}
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
            verifyAndReworkPOPIDetailGridRows();
        }

        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit") || columnName.Equals("Approve"))
                {
                    tabPOType.Enabled = false;
                    tabPOType.Visible = false;
                    tabPOHeader.Enabled = true;
                    tabPOHeader.Visible = true;
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    setPODetailColumns(docID);
                    //tabPOHeader.Visible = true;
                    //tabPODetail.Visible = true;
                    //captureChange = false;
                    POPIHeaderDB popidb = new POPIHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevpopi = new popiheader();
                    prevpopi.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    prevpopi.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    prevpopi.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    prevpopi.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    prevpopi.TrackingNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTrackingNo"].Value.ToString());
                    prevpopi.TrackingDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTrackingDate"].Value.ToString());
                    prevpopi.CustomerID = grdList.Rows[e.RowIndex].Cells["gCustomerID"].Value.ToString();
                    prevpopi.CustomerName = grdList.Rows[e.RowIndex].Cells["gCustomerName"].Value.ToString();
                    prevpopi.CustomerPONO = grdList.Rows[e.RowIndex].Cells["gCustomerPONo"].Value.ToString();
                    prevpopi.CustomerPODate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCustomerPODate"].Value.ToString());
                    prevpopi.DeliveryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gDeliveryDate"].Value.ToString());
                    prevpopi.PaymentTerms = grdList.Rows[e.RowIndex].Cells["gPaymentTerms"].Value.ToString();
                    prevpopi.PaymentMode = grdList.Rows[e.RowIndex].Cells["gPaymentMode"].Value.ToString();
                    prevpopi.FreightTerms = grdList.Rows[e.RowIndex].Cells["gFreightTerms"].Value.ToString();
                    prevpopi.FreightCharge = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gFreightCharge"].Value.ToString());
                    prevpopi.CurrencyID = grdList.Rows[e.RowIndex].Cells["gCurrencyID"].Value.ToString();
                    prevpopi.TaxCode = grdList.Rows[e.RowIndex].Cells["gTaxCode"].Value.ToString();
                    prevpopi.BillingAddress = grdList.Rows[e.RowIndex].Cells["gBillingAddress"].Value.ToString();
                    prevpopi.DeliveryAddress = grdList.Rows[e.RowIndex].Cells["gDeliveryAddress"].Value.ToString();
                    prevpopi.ProductValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gProductValue"].Value.ToString());
                    prevpopi.TaxAmount = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gTaxAmount"].Value.ToString());
                    prevpopi.POValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gPOValue"].Value.ToString());
                    prevpopi.Remarks = grdList.Rows[e.RowIndex].Cells["gRemarks"].Value.ToString();
                    prevpopi.status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    prevpopi.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    prevpopi.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    prevpopi.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    prevpopi.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    prevpopi.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevpopi.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();

                    txtTemporarryNo.Text = prevpopi.TemporaryNo.ToString();
                    dtTempDate.Value = prevpopi.TemporaryDate;
                    dtTempDate.Value = prevpopi.TemporaryDate;

                    txtTrackingNo.Text = prevpopi.TrackingNo.ToString();
                    try
                    {
                        dtTrackDate.Value = prevpopi.TrackingDate;
                    }
                    catch (Exception)
                    {
                        dtTrackDate.Value = DateTime.Parse("01-01-1900");
                    }

                    cmbCustomer.SelectedIndex = cmbCustomer.FindString(prevpopi.CustomerID);

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
                    // cmbPaymentTerms.SelectedIndex = cmbPaymentTerms.FindString(prevpopi.PaymentTerms);
                    txtPaymentTerms.Text = prevpopi.PaymentTerms;
                    cmbPaymentMode.SelectedIndex = cmbPaymentMode.FindString(grdList.Rows[e.RowIndex].Cells[12].Value.ToString());
                    cmbFreightTerms.SelectedIndex = cmbFreightTerms.FindString(prevpopi.FreightTerms);
                    cmbCurrency.SelectedIndex = cmbCurrency.FindString(prevpopi.CurrencyID);
                    //cmbCurrency.Text = prevpopi.CurrencyID;
                    cmbTaxCode.SelectedIndex = cmbTaxCode.FindString(prevpopi.TaxCode);
                    //cmbTaxCode.Text = prevpopi.TaxCode;
                    txtBillingAddress.Text = prevpopi.BillingAddress;
                    txtDeliveryAddress.Text = prevpopi.DeliveryAddress;
                    txtProductValue.Text = prevpopi.ProductValue.ToString();
                    txtTaxAmount.Text = prevpopi.TaxAmount.ToString();
                    txtPOValue.Text = prevpopi.POValue.ToString();
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
                                grdPOPIDetail.Rows[i].Cells["Item"].Value = popid.StockItemID + "-" + popid.StockItemName;
                            }
                            catch (Exception)
                            {
                                grdPOPIDetail.Rows[i].Cells["Item"].Value = null;
                            }
                            grdPOPIDetail.Rows[i].Cells["Quantity"].Value = popid.Quantity;
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
                    pnlBottomActions.Visible = false;

                    if (columnName.Equals("Edit"))
                    {
                        btnSave.Visible = true;
                        btnApprove.Visible = false;
                        btnForward.Visible = false;
                    }
                    if (columnName.Equals("Approve"))
                    {
                        btnSave.Visible = false;
                        if (approverList.Length == 0)
                        {
                            btnApprove.Visible = true;
                            btnForward.Visible = false;
                        }
                        else
                        {
                            if (Main.userLoggedIn.Equals(prevpopi.CreateUser))
                            {
                                btnApprove.Visible = false;
                                btnForward.Visible = true;
                            }
                            else
                            {
                                btnApprove.Visible = true;
                                btnForward.Visible = true;
                            }
                        }
                        //captureChange = true;
                    }
                    tabControl1.SelectedTab = tabPOHeader;
                    tabControl1.Visible = true;
                    
                    disableBottomButtons();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnClearEntries_Click_1(object sender, EventArgs e)
        {
            try
            {
                for (int i = grdPOPIDetail.Rows.Count - 1; i >= 0; i--)
                {
                    grdPOPIDetail.Rows.RemoveAt(i);
                }
            }
            catch (Exception)
            {
            }
            MessageBox.Show("item cleared.");
        }

        private void btnForward_Click_1(object sender, EventArgs e)
        {
            try
            {
                POPIHeaderDB popih = new POPIHeaderDB();
                DialogResult dialog = MessageBox.Show("Are you sure to forward the document for Approval ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (popih.forwardPOPI(prevpopi))
                    {
                        MessageBox.Show("PO Product Inward Document Forwarded");
                        closeAllPanels();
                        ListFilteredStockOBHeader(1);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnApprove_Click_1(object sender, EventArgs e)
        {
            try
            {
                POPIHeaderDB popihdb = new POPIHeaderDB();
                popiheader popih = new popiheader();
                DialogResult dialog = MessageBox.Show("Are you sure to forward the document for Approval ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevpopi.TrackingNo = DocumentNumberDB.getNewNumber(docID, 2);
                    if (popihdb.ApprovePOPI(prevpopi))
                    {
                        MessageBox.Show("PO Product Inward Document Approved");
                        closeAllPanels();
                        ListFilteredStockOBHeader(1);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                customer cust = new customer();
                string custid = cmbCustomer.SelectedItem.ToString().Trim().Substring(0, cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-'));
                cust = CustomerDB.getCustomerDetails(custid);
                txtBillingAddress.Text = cust.BillingAddress;
                txtDeliveryAddress.Text = cust.DeliveryAddress;
            }
            catch (Exception ex)
            {

            }
        }

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
        }

        private void cmbPOType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string potype = cmbPOType.SelectedItem.ToString().Trim().Substring(0, cmbPOType.SelectedItem.ToString().Trim().IndexOf('-'));
                if (potype == "Product")
                {
                    docID = "POPRODUCTINWARD";
                }
                else if (potype == "Service")
                {
                    docID = "POSERVICEINWARD";
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
                    //grdPOPIDetail.Columns["WorkDescription"].Visible = false;
                    //grdPOPIDetail.Columns["Select"].Visible = false;
                    //grdPOPIDetail.Columns["Item"].Visible = true;
                    //grdPOPIDetail.Columns["CustomerItemDescription"].Visible = true;
                }
                else if (docID == "POSERVICEINWARD")
                {
                    //grdPOPIDetail.Columns["WorkDescription"].Visible = true;
                    //grdPOPIDetail.Columns["Select"].Visible = true;
                    //grdPOPIDetail.Columns["Item"].Visible = false;
                    //grdPOPIDetail.Columns["CustomerItemDescription"].Visible = false;
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
                if (cmbFreightTerms.SelectedItem.ToString().Trim().Substring(0, cmbFreightTerms.SelectedItem.ToString().Trim().IndexOf('-')).Trim().Equals("Extra"))
                {
                    txtFreightCharge.Enabled = true;
                }
                else
                {
                    txtFreightCharge.Text = "0";
                    txtFreightCharge.Enabled = false;
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void btnPaymentTermsSelection_Click(object sender, EventArgs e)
        {
            btnPaymentTermsSelection.Enabled = false;
            //dgvpt = PTDefinitionDB.AcceptPaymentTerms("1-2-2;2-5-5;3-9-9;4-10-10;4-100-100");
            dgvpt = PTDefinitionDB.AcceptPaymentTerms(txtPaymentTerms.Text);
            pnldgv = new Panel();
            pnldgv.BorderStyle = BorderStyle.FixedSingle;

            pnldgv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
            //----------

            pnldgv.Controls.Add(dgvpt);
            dgvpt.Visible = true;
            pnldgv.Visible = true;
            //------

            Button dgvptOK = new Button();
            dgvptOK.Text = "OK";
            dgvptOK.Location = new Point(100, 270);
            dgvptOK.Click += new System.EventHandler(this.dgvptOK_Click);
            pnldgv.Controls.Add(dgvptOK);

            Button dgvptCancel = new Button();
            dgvptCancel.Text = "Cancel";
            dgvptCancel.Location = new Point(200, 270);
            dgvptCancel.Click += new System.EventHandler(this.dgvptCancel_Click);
            pnldgv.Controls.Add(dgvptCancel);

            Button dgvptAddRow = new Button();
            dgvptAddRow.Text = "Add Credit Row";
            dgvptAddRow.Location = new Point(300, 270);
            dgvptAddRow.Click += new System.EventHandler(this.dgvptAddRow_Click);
            pnldgv.Controls.Add(dgvptAddRow);

            pnlAddEdit.Controls.Add(pnldgv);
            pnldgv.BringToFront();
            pnldgv.Visible = true;
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
                            val1 = dgvpt.Rows[i].Cells["Code"].Value.ToString();
                            val2 = dgvpt.Rows[i].Cells["Percentage"].Value.ToString();
                            val3 = dgvpt.Rows[i].Cells["CreditPeriod"].Value.ToString();
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
                pnldgv.Visible = false;
                btnPaymentTermsSelection.Enabled = true;

            }
            catch (Exception ex)
            {
            }
        }

        private void dgvptCancel_Click(object sender, EventArgs e)
        {
            try
            {
                btnPaymentTermsSelection.Enabled = true;
                pnldgv.Visible = false;
            }
            catch (Exception)
            {
            }
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
    }
}

