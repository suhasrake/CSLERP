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
    public partial class PurchaseReturnHeader : System.Windows.Forms.Form
    {
        Boolean track = false;
        //int no = 0;
        int yy = 0;
        string docID = "";
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Boolean userIsACommenter = false;
        string chkTax = "";
        double productvalue = 0.0;
        double taxvalue = 0.0;
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        purchasereturnheader prevprh;

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
        string custid = "";
        //DateTimePicker dtp;
        Panel pnlModel = new Panel();
        Boolean isViewMOde = false;
        public PurchaseReturnHeader()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }
        private void PurchaseReturnHeader_Load(object sender, EventArgs e)
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
            ListFilteredPRHeader(listOption);
            //tabControl1.SelectedIndexChanged += new EventHandler(TabControl1_SelectedIndexChanged);
        }
        private void ListFilteredPRHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                PurchaseReturnHeaderDB prdb = new PurchaseReturnHeaderDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);

                List<purchasereturnheader> PRHeaders = prdb.getFilteredPRHeader(userString, option, userCommentStatusString);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (purchasereturnheader prh in PRHeaders)
                {
                    if (option == 1)
                    {
                        if (prh.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = prh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = prh.DocumentName;

                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = prh.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = prh.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gMRNNo"].Value = prh.MRNNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gMRNDate"].Value = prh.MRNDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPRNo"].Value = prh.PRNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPRDate"].Value = prh.PRDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerID"].Value = prh.CustomerID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerName"].Value = prh.CustomerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["PValue"].Value = prh.ProductValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["TaxAmount"].Value = prh.TaxAmount;
                    grdList.Rows[grdList.RowCount - 1].Cells["PRValue"].Value = prh.PRValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["gRemarks"].Value = prh.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = prh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["FrowardUser"].Value = prh.ForwardUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ApproveUser"].Value = prh.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = prh.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = prh.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = prh.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = prh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = prh.status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = prh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = prh.CreateUser;
                    //grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatusNo"].Value = prh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CmtStatus"].Value = prh.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = prh.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = prh.ForwarderList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in PR Listing");
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
            CustomerDB.fillCustomerComboNew(cmbCustomer);
            //TaxCodeDB.fillTaxCodeCombo(cmbTaxCode);
            cmbPRType.SelectedIndex = -1;
            dtTempDate.Format = DateTimePickerFormat.Custom;
            dtTempDate.CustomFormat = "dd-MM-yyyy";
            dtTempDate.Enabled = false;
            dtMRNDate.Format = DateTimePickerFormat.Custom;
            dtMRNDate.CustomFormat = "dd-MM-yyyy";
            dtMRNDate.Enabled = false;
            dtPRDate.Format = DateTimePickerFormat.Custom;
            dtPRDate.CustomFormat = "dd-MM-yyyy";
            txtTemporarryNo.Enabled = false;
            dtTempDate.Enabled = false;
            txtMRNNo.Enabled = false;
            dtMRNDate.Enabled = false;
            btnForward.Visible = false;
            btnApprove.Visible = false;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            cmbCustomer.TabIndex = 0;
            txtPRNo.TabIndex = 1;
            dtPRDate.TabIndex = 2;
            //dtDCDate.TabIndex = 3;
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
        }
        private void setTabIndex()
        {
            btnSelectMRN.TabIndex = 0;
            txtComments.TabIndex = 2;
            cmbCustomer.TabIndex = 1;
            //btnSelectInvOutRefNo.TabIndex = 3;
            //txtnarration.TabIndex = 4;

            btnAddLine.TabIndex = 3;
            btnCalculate.TabIndex = 4;
            btnClearEntries.TabIndex = 5;

            btnForward.TabIndex = 7;
            btnApprove.TabIndex = 8;
            btnCancel.TabIndex = 9;
            btnSave.TabIndex = 10;
            btnReverse.TabIndex = 11;
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
                grdPRDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                isViewMOde = false;
                dgvpt.Rows.Clear();
                //----------clear temperory panels

                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                pnllv.Visible = false;
                //btnQC.Enabled = true;
                //----------
                cmbCustomer.SelectedIndex = -1;
               // cmbTaxCode.SelectedIndex = -1;
                txtTemporarryNo.Text = "";
                dtTempDate.Value = DateTime.Parse("01-01-1900");
                txtMRNNo.Text = "";
                dtMRNDate.Value = DateTime.Today.Date;
                txtPRNo.Text = "";
                dtPRDate.Value = DateTime.Parse("01-01-1900");

                cmbPRType.SelectedIndex = -1;
                txtRemarks.Text = "";
                prevprh = new purchasereturnheader();
                txtProductValue.Text = "";
                txtTaxAmount.Text = "";
                txtPRValue.Text = "";
                btnProductValue.Text = "";
                btnTaxAmount.Text = "";
                //removeControlsFromCommenterPanel();
                //removeControlsFromForwarderPanel();
                //removeControlsFromLVPanel();
                
                //track = false;
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
                tabControl1.SelectedTab = tabPRType;
                setButtonVisibility("btnNew");
                cmbPRType.Enabled = true;
            }
            catch (Exception)
            {

            }
        }


        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                AddPRDetailRow();
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
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["StockReferenceNo"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["StockItemID"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["StockItemName"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["ModelNo"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["ModelName"].Value = "";
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdPRDetail.Rows[kount - 1].Cells["gTaxCode"]);
                TaxCodeDB.fillTaxCodeGridViewCombo(ComboColumn2, "");
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["Quantity"].Value = Convert.ToDouble(0);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["Price"].Value = Convert.ToDouble(0);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["Value"].Value = Convert.ToDouble(0);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["Tax"].Value = Convert.ToDouble(0);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["TaxDetails"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["BatchNo"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["SerielNo"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["ExpiryDate"].Value = Convert.ToDateTime("1900-01-01");
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["StoreLocationID"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["StoreLocationName"].Value = "";
                var BtnCell = (DataGridViewButtonCell)grdPRDetail.Rows[kount - 1].Cells["Delete"];
                BtnCell.Value = "Del";
            }

            catch (Exception ex)
            {
                MessageBox.Show("AddPRDetailRow() : Error");
            }

            return status;
        }
        private Boolean verifyAndReworkPRDetailGridRows()
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
                if (grdPRDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Purchase Return details");
                    txtProductValue.Text = productvalue.ToString();
                    txtTaxAmount.Text = taxvalue.ToString(); //fill this later
                    txtPRValue.Text = (productvalue + taxvalue).ToString();
                    btnProductValue.Text = txtProductValue.Text;
                    return false;
                }
                //clear tax details table
                TaxDetailsTable.Rows.Clear();
                for (int i = 0; i < grdPRDetail.Rows.Count; i++)
                {
                    if(grdPRDetail.Rows[i].Cells["gTaxCode"].Value == null ||
                        grdPRDetail.Rows[i].Cells["gTaxCode"].Value.ToString().Length == 0)
                    {
                        MessageBox.Show("Fill Tax Code in row " + (i + 1));
                        return false;
                    }
                    grdPRDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if ((grdPRDetail.Rows[i].Cells["StockItemName"].Value.ToString().Trim().Length == 0) ||
                        (grdPRDetail.Rows[i].Cells["StockItemID"].Value.ToString().Trim().Length == 0) ||
                         (grdPRDetail.Rows[i].Cells["Quantity"].Value == null) ||
                        (grdPRDetail.Rows[i].Cells["Quantity"].Value.ToString().Trim().Length == 0) ||
                       
                        (Convert.ToDouble(grdPRDetail.Rows[i].Cells["Quantity"].Value) == 0) ||  
                        (grdPRDetail.Rows[i].Cells["Price"].Value.ToString().Trim().Length == 0) ||
                        (Convert.ToDouble(grdPRDetail.Rows[i].Cells["Price"].Value) == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                    int str = Convert.ToInt32(grdPRDetail.Rows[i].Cells["StockReferenceNo"].Value);
                    double qunt = Convert.ToInt32(grdPRDetail.Rows[i].Cells["Quantity"].Value);
                    if (docID == "PURCHASERETURNQR")
                    {
                        if (!isViewMOde && !MRNHeaderDB.verifyRejectedStockAvailability(str, qunt))
                        {
                            MessageBox.Show("That much Qunatity Not Available .Check Row No:" + (i + 1));
                            return false;
                        }
                    }
                    else
                    {
                        if (!isViewMOde && !StockDB.verifyPresentStockAvailability(str, qunt))
                        {
                            MessageBox.Show("That much Stock Not Available.Check Row No:" + (i + 1));
                            return false;
                        }
                    }
                    quantity = Convert.ToDouble(grdPRDetail.Rows[i].Cells["Quantity"].Value);
                    price = Convert.ToDouble(grdPRDetail.Rows[i].Cells["Price"].Value);
                    if (price != 0 && quantity != 0)
                    {
                        cost = Math.Round(quantity * price, 2);
                    }

                    try
                    {
                        strtaxCode = grdPRDetail.Rows[i].Cells["gTaxCode"].Value.ToString();
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
                    grdPRDetail.Rows[i].Cells["Value"].Value = Convert.ToDouble(cost.ToString());
                    grdPRDetail.Rows[i].Cells["Tax"].Value = ttax2.ToString();
                    grdPRDetail.Rows[i].Cells["TaxDetails"].Value = strTax;
                    productvalue = productvalue + cost;
                    taxvalue = taxvalue + ttax2;

                    //- rewok tax value
                }
                txtProductValue.Text = productvalue.ToString();
                txtTaxAmount.Text = taxvalue.ToString(); //fill this later
                txtPRValue.Text = (productvalue + taxvalue).ToString();
                btnProductValue.Text = txtProductValue.Text;
                btnTaxAmount.Text = txtTaxAmount.Text;
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

        ////check for item duplication in details
        //private Boolean validateItems()
        //{
        //    Boolean status = true;
        //    try
        //    {
        //        for (int i = 0; i < grdPRDetail.Rows.Count - 1; i++)
        //        {
        //            for (int j = i + 1; j < grdPRDetail.Rows.Count; j++)
        //            {

        //                if (grdPRDetail.Rows[i].Cells[1].Value.ToString() == grdPRDetail.Rows[j].Cells["StockItemID"].Value.ToString())
        //                {
        //                    //duplicate item code
        //                    //MessageBox.Show("Item code duplicated in MRN details... please ensure correctness (" +
        //                    //    grdPRDetail.Rows[i].Cells["StockItemName"].Value.ToString() + ")");
        //                    MessageBox.Show("Item code duplicated in MRN details... please ensure correctness (" +
        //                       grdPRDetail.Rows[i].Cells["StockItemName"].Value.ToString() + ")");
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
        private List<purchasereturndetail> getPRDetails(purchasereturnheader prh)
        {
            purchasereturndetail prd = new purchasereturndetail();

            List<purchasereturndetail> PRDetails = new List<purchasereturndetail>();
            for (int i = 0; i < grdPRDetail.Rows.Count; i++)
            {
                try
                {
                    prd = new purchasereturndetail();
                    prd.DocumentID = prh.DocumentID;
                    prd.TemporaryNo = prh.TemporaryNo;
                    prd.TemporaryDate = prh.TemporaryDate;
                    prd.StockItemID = grdPRDetail.Rows[i].Cells["StockItemID"].Value.ToString().Trim();
                    prd.ModelNo = grdPRDetail.Rows[i].Cells["ModelNo"].Value.ToString().Trim();
                    prd.TaxCode = grdPRDetail.Rows[i].Cells["gTaxCode"].Value.ToString();
                    prd.Quantity = Convert.ToDouble(grdPRDetail.Rows[i].Cells["Quantity"].Value);
                    prd.Price = Convert.ToDouble(grdPRDetail.Rows[i].Cells["Price"].Value);
                    prd.Tax = Convert.ToDouble(grdPRDetail.Rows[i].Cells["Tax"].Value);
                    prd.TaxDetails = grdPRDetail.Rows[i].Cells["TaxDetails"].Value.ToString();
                    prd.BatchNo = grdPRDetail.Rows[i].Cells["BatchNo"].Value.ToString();
                    prd.SerialNo = grdPRDetail.Rows[i].Cells["SerielNo"].Value.ToString();
                    prd.ExpiryDate = Convert.ToDateTime(grdPRDetail.Rows[i].Cells["ExpiryDate"].Value);
                    prd.StoreLocationID = grdPRDetail.Rows[i].Cells["StoreLocationID"].Value.ToString().Trim();
                    prd.StockReferenceNo = Convert.ToInt32(grdPRDetail.Rows[i].Cells["StockReferenceNo"].Value);
                    PRDetails.Add(prd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("createAndUpdatePRDetails() : Error creating PR Details");
                    //status = false;
                }
            }
            return PRDetails;
        }
        //private Boolean createAndUpdatePRDetails(purchasereturnheader prh)
        //{
        //    Boolean status = true;
        //    try
        //    {
        //        PurchaseReturnHeaderDB prdb = new PurchaseReturnHeaderDB();
        //        List<purchasereturndetail> PRDetails = getPRDetails(prh);
        //        if (!prdb.updatePRDetail(PRDetails, prh))
        //        {
        //            MessageBox.Show("createAndUpdatePRDetails() : Failed to update PR Details. Please check the values");
        //            status = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("createAndUpdatePRDetails() : Error updating pr Details");
        //        status = false;
        //    }
        //    return status;
        //}

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredPRHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredPRHeader(listOption);
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

            ListFilteredPRHeader(listOption);
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {

                PurchaseReturnHeaderDB prhdb = new PurchaseReturnHeaderDB();
                purchasereturnheader prh = new purchasereturnheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;
                if (!verifyAndReworkPRDetailGridRows())
                {
                    MessageBox.Show("Validation Failed in PR Detail");
                    return;
                }
                prh.DocumentID = docID;
                prh.PRDate = dtPRDate.Value;
                prh.MRNDate = dtMRNDate.Value;
                prh.MRNNo = Convert.ToInt32(txtMRNNo.Text);
                if(cmbCustomer.SelectedIndex == -1)
                {
                    MessageBox.Show("Select Customer");
                    return;
                }
                ////////prh.CustomerID = cmbCustomer.SelectedItem.ToString().Trim().Substring(0, cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-'));
                prh.CustomerID = ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue;
                //prh.TaxCode = cmbTaxCode.SelectedItem.ToString();
                prh.Remarks = txtRemarks.Text.Trim().Replace("'","''");
                //prh.TaxCode = cmbTaxCode.SelectedItem.ToString().Trim();
                if (txtTaxAmount.Text.Length != 0)
                    prh.TaxAmount = Convert.ToDouble(txtTaxAmount.Text);
                if (txtProductValue.Text.Length != 0)
                    prh.ProductValue = Convert.ToDouble(txtProductValue.Text);
                if (txtPRValue.Text.Length != 0)
                    prh.PRValue = Convert.ToDouble(txtPRValue.Text);
                prh.Comments = docCmtrDB.DGVtoString(dgvComments).Replace("'","''");
                prh.ForwarderList = prevprh.ForwarderList;

                if (!prhdb.validatePRHeader(prh))
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    //prh.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    prh.DocumentStatus = 1; //created
                    prh.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    prh.TemporaryNo = Convert.ToInt32(txtTemporarryNo.Text);
                    prh.TemporaryDate = prevprh.TemporaryDate;
                    prh.DocumentStatus = prevprh.DocumentStatus;

                }

                if (prhdb.validatePRHeader(prh))
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
                            prh.CommentStatus = docCmtrDB.createCommentStatusString(prevprh.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            prh.CommentStatus = prevprh.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            prh.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            prh.CommentStatus = prevprh.CommentStatus;
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
                        prh.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus);
                    }
                    List<purchasereturndetail> PRDetails = getPRDetails(prh);
                    if (btnText.Equals("Update"))
                    {
                        if (prhdb.updatePRHeaderAndDetail(prh, prevprh, PRDetails))
                        {
                            MessageBox.Show("PR Header Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredPRHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update PR Product Inward Header");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (prhdb.InsertPRHeaderAndDetail(prh, PRDetails))
                        {
                            MessageBox.Show("PR Header Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredPRHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert PR  Header");
                        }
                    }
                }
                else
                {
                    status = false;
                    MessageBox.Show("PR Details Validation failed");
                }
            }
            catch (Exception ex)
            {
                status = false;
                MessageBox.Show("btnSave_Click_1() : Error");
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
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
            if (grdPRDetail.Rows.Count > 0)
            {
                if (!verifyAndReworkPRDetailGridRows())
                {

                    return;
                }
            }
            AddPRDetailRow();

        }
        private void grdPOPIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
                        verifyAndReworkPRDetailGridRows();
                    }
                    if (columnName.Equals("ViewTaxDetails"))
                    {
                        //show tax details
                        DialogResult dialog = MessageBox.Show(grdPRDetail.Rows[e.RowIndex].Cells["TaxDetails"].Value.ToString(),
                            "Line No : " + (e.RowIndex + 1), MessageBoxButtons.OK);
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
        private void btnCalculateax_Click_1(object sender, EventArgs e)
        {
            verifyAndReworkPRDetailGridRows();
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
                    setButtonVisibility(columnName);

                    prevprh = new purchasereturnheader();
                    prevprh.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();
                    prevprh.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    if (prevprh.DocumentID == "PURCHASERETURNQR")
                        cmbPRType.SelectedIndex = cmbPRType.FindString("Rejected Purchase Return");
                    else
                        cmbPRType.SelectedIndex = cmbPRType.FindString("Accepted Purchase Return");
                    prevprh.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    prevprh.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    prevprh.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    if (prevprh.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    prevprh.Comments = PurchaseReturnHeaderDB.getUserComments(prevprh.DocumentID, prevprh.TemporaryNo, prevprh.TemporaryDate);
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();

                    PurchaseReturnHeaderDB prdb = new PurchaseReturnHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];

                    prevprh.PRNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gPRNo"].Value.ToString());
                    prevprh.PRDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gPRDate"].Value.ToString());
                    prevprh.MRNNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gMRNNo"].Value.ToString());
                    prevprh.MRNDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gMRNDate"].Value.ToString());
                    prevprh.CustomerID = grdList.Rows[e.RowIndex].Cells["gCustomerID"].Value.ToString();
                    prevprh.CustomerName = grdList.Rows[e.RowIndex].Cells["gCustomerName"].Value.ToString();

                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Document Temp No:" + prevprh.TemporaryNo + "\n" +
                            "Document Temp Date:" + prevprh.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "PR No:" + prevprh.PRNo + "\n" +
                            "PR Date:" + prevprh.PRDate.ToString("dd-MM-yyyy") + "\n" +
                            "Customer:" + prevprh.CustomerName;
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevprh.TemporaryNo + "-" + prevprh.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_1(null, null);
                        return;
                    }
                    if (columnName.Equals("View"))
                    {
                        tabControl1.TabPages["tabPRDetail"].Enabled = true;
                        isViewMOde = true;
                    }
                    //--------
                    prevprh.ProductValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["PValue"].Value.ToString());
                    prevprh.TaxAmount = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["TaxAmount"].Value.ToString());
                    prevprh.PRValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["PRValue"].Value.ToString());
                    //prevprh.TaxCode = grdList.Rows[e.RowIndex].Cells["TCode"].Value.ToString();
                    prevprh.Remarks = grdList.Rows[e.RowIndex].Cells["gRemarks"].Value.ToString();
                    prevprh.status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    prevprh.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    prevprh.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    prevprh.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    prevprh.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    prevprh.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevprh.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    prevprh.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    //--comments
                    chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    prevprh.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(prevprh.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(prevprh.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---
                    txtTemporarryNo.Text = prevprh.TemporaryNo.ToString();
                    dtTempDate.Value = prevprh.TemporaryDate;
                    txtPRNo.Text = prevprh.PRNo.ToString();
                    try
                    {
                        dtPRDate.Value = prevprh.PRDate;
                    }
                    catch (Exception)
                    {
                        dtPRDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtMRNNo.Text = prevprh.MRNNo.ToString();
                    try
                    {
                        dtMRNDate.Value = prevprh.MRNDate;
                    }
                    catch (Exception)
                    {
                        dtMRNDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtProductValue.Text = prevprh.ProductValue.ToString();
                    txtTaxAmount.Text = prevprh.TaxAmount.ToString();
                    btnProductValue.Text = prevprh.ProductValue.ToString();
                    btnTaxAmount.Text = prevprh.TaxAmount.ToString();
                    txtPRValue.Text = prevprh.PRValue.ToString();
                    ////////cmbCustomer.SelectedIndex = cmbCustomer.FindString(prevprh.CustomerID);
                    cmbCustomer.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCustomer, prevprh.CustomerID);
                    //cmbTaxCode.SelectedIndex = cmbTaxCode.FindString(prevprh.TaxCode);
                    txtRemarks.Text = prevprh.Remarks.ToString();
                    List<purchasereturndetail> PRDetail = PurchaseReturnHeaderDB.getPRDetail(prevprh);
                    grdPRDetail.Rows.Clear();
                    int i = 0;
                    try
                    {
                        foreach (purchasereturndetail prd in PRDetail)
                        {
                            AddPRDetailRow();
                            try
                            {
                                grdPRDetail.Rows[i].Cells["StockItemID"].Value = prd.StockItemID;
                                grdPRDetail.Rows[i].Cells["StockItemName"].Value = prd.StockItemName;
                                grdPRDetail.Rows[i].Cells["ModelNo"].Value = prd.ModelNo;
                                grdPRDetail.Rows[i].Cells["ModelName"].Value = prd.ModelName;
                            }
                            catch (Exception ex)
                            {
                                grdPRDetail.Rows[i].Cells["Item"].Value = null;
                            }
                            grdPRDetail.Rows[i].Cells["Quantity"].Value = prd.Quantity;
                            grdPRDetail.Rows[i].Cells["gTaxCode"].Value = prd.TaxCode;
                            grdPRDetail.Rows[i].Cells["Price"].Value = prd.Price;
                            grdPRDetail.Rows[i].Cells["Value"].Value = prd.Quantity * prd.Price;
                            grdPRDetail.Rows[i].Cells["Tax"].Value = prd.Tax;
                            grdPRDetail.Rows[i].Cells["BatchNo"].Value = prd.BatchNo;
                            grdPRDetail.Rows[i].Cells["SerielNo"].Value = prd.SerialNo;
                            grdPRDetail.Rows[i].Cells["ExpiryDate"].Value = prd.ExpiryDate;
                            grdPRDetail.Rows[i].Cells["StoreLocationID"].Value = prd.StoreLocationID;
                            grdPRDetail.Rows[i].Cells["StoreLocationName"].Value = prd.StoreLocationName;
                            grdPRDetail.Rows[i].Cells["TaxDetails"].Value = prd.TaxDetails;
                            grdPRDetail.Rows[i].Cells["StockReferenceNo"].Value = prd.StockReferenceNo;

                            //grdPRDetail.Rows[i].Cells["gTaxCode"].Value = null;
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
                //if (!verifyAndReworkPRDetailGridRows())
                //{
                //    MessageBox.Show("Error found in PR details. Please correct before updating the details");
                //}
                btnSave.Text = "Update";
                pnlList.Visible = false;
                pnlAddEdit.Visible = true;
                tabControl1.SelectedTab = null;
                tabControl1.SelectedTab = tabPRHeader;

                tabControl1.Visible = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Cell click error");
                setButtonVisibility("init");
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
                PurchaseReturnHeaderDB prhdb = new PurchaseReturnHeaderDB();
                FinancialLimitDB flDB = new FinancialLimitDB();
                if (!flDB.checkEmployeeFinancialLimit(docID, Login.empLoggedIn, Convert.ToDouble(txtProductValue.Text), 0))
                {
                    MessageBox.Show("No financial power for approving this document");
                    return;
                }
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevprh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevprh.CommentStatus);
                    prevprh.PRNo = DocumentNumberDB.getNewNumber(docID, 2);
                    if (verifyAndReworkPRDetailGridRows())
                    {
                        List<purchasereturndetail> PRDetails = getPRDetails(prevprh);
                        if (prhdb.ApprovePR(prevprh, PRDetails))
                        {
                            MessageBox.Show("PR Document Approved");
                            if (!updateDashBoard(prevprh, 2))
                            {
                                MessageBox.Show("DashBoard Fail to update");
                            }
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredPRHeader(listOption);
                            setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                        }
                        else
                            MessageBox.Show("Unable to approve");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Exception : Unable to approve");
            }

        }
        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (btnSave.Text == "Save")
            {
                if (cmbPRType.SelectedIndex == -1)
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
                ///frmPopup.Dispose();
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
                            PurchaseReturnHeaderDB prhdb = new PurchaseReturnHeaderDB();
                            prevprh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevprh.CommentStatus);
                            prevprh.ForwardUser = approverUID;
                            prevprh.ForwarderList = prevprh.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (prhdb.forwardPR(prevprh))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(prevprh, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredPRHeader(listOption);
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
        private Boolean updateDashBoard(purchasereturnheader prh, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = prh.DocumentID;
                dsb.TemporaryNo = prh.TemporaryNo;
                dsb.TemporaryDate = prh.TemporaryDate;
                dsb.DocumentNo = prh.PRNo;
                dsb.DocumentDate = prh.PRDate;
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
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(prevprh.DocumentID);
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
                    string s = prevprh.ForwarderList;
                    string reverseStr = getReverseString(prevprh.ForwarderList);
                    //do forward activities
                    prevprh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevprh.CommentStatus);
                    PurchaseReturnHeaderDB prhdb = new PurchaseReturnHeaderDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevprh.ForwarderList = reverseStr.Substring(0, ind);
                        prevprh.ForwardUser = reverseStr.Substring(ind + 3);
                        prevprh.DocumentStatus = prevprh.DocumentStatus - 1;
                    }
                    else
                    {
                        prevprh.ForwarderList = "";
                        prevprh.ForwardUser = "";
                        prevprh.DocumentStatus = 1;
                    }
                    if (prhdb.reversePR(prevprh))
                    {
                        MessageBox.Show("PR Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredPRHeader(listOption);
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
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevprh.TemporaryNo + "-" + prevprh.TemporaryDate.ToString("yyyyMMddhhmmss"));
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
                    string subDir = prevprh.TemporaryNo + "-" + prevprh.TemporaryDate.ToString("yyyyMMddhhmmss");
                    dgv.Enabled = false;
                    ////DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
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
                    tabControl1.SelectedTab = tabPRType;
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
                    tabControl1.SelectedTab = tabPRType;
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

                    tabControl1.SelectedTab = tabPRType;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabPRType;
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

        private void btnSelectPO_Click(object sender, EventArgs e)
        {
            //removeControlsFromLVPanel();
            if (grdPRDetail.Rows.Count != 0)
            {
                DialogResult dialog = MessageBox.Show("Warning: Purchase return Detail will removed?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdPRDetail.Rows.Clear();
                    txtProductValue.Text = "";
                    txtTaxAmount.Text = "";
                    txtPRValue.Text = "";
                    btnProductValue.Text = "";
                    btnTaxAmount.Text = "";
                }
                else
                    return;
            }
            //btnSelectPO.Enabled = false;
            //pnlEditButtons.Enabled = false;
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

            frmPopup.Size = new Size(600, 300);
            lv = MRNHeaderDB.getMRNNoWithStockDetail(docID);
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(600, 250));
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
                    MessageBox.Show("Select one MRN");
                    return;
                }
                else
                {
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            txtMRNNo.Text = itemRow.SubItems[1].Text;
                            dtMRNDate.Text = itemRow.SubItems[2].Text;
                            cmbCustomer.SelectedIndex =
                                        Structures.ComboFUnctions.getComboIndex(cmbCustomer, itemRow.SubItems[3].Text);
                            //cmbTaxCode.SelectedItem = itemRow.SubItems[5].Text;
                            frmPopup.Close();
                            frmPopup.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
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


        private void cmbCustomer_SelectedValueChanged(object sender, EventArgs e)
        {
        }

        private void btnQC_Click(object sender, EventArgs e)
        {
        }
        private void btnSelectStockDetail_Click(object sender, EventArgs e)
        {
            if (txtMRNNo.Text.Length == 0)
            {
                MessageBox.Show("Select MRN No");
                return;
            }
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(800, 300);
            if (docID == "PURCHASERETURNQR")
            {
                lv = MRNHeaderDB.getMRNNoWiseMRNDetailLV(Convert.ToInt32(txtMRNNo.Text), dtMRNDate.Value);
            }
            else
            {
                lv = StockDB.getMRNNoWiseStockListView(Convert.ToInt32(txtMRNNo.Text), dtMRNDate.Value);
                lv.Columns[4].Width = 0;
                lv.Columns[5].Width = 0;
                lv.Columns[9].Width = 0;
                lv.Columns[10].Width = 0;
                lv.Columns[11].Width = 0;
                lv.Columns[12].Width = 0;
            }
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(800, 250));
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
        }
        private void lvOK_Click2(object sender, EventArgs e)
        {
            try
            {
                if(lv.CheckedIndices.Count != 1)
                {
                    MessageBox.Show("Select one item.");
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        if (docID == "PURCHASERETURNQR")
                        {
                            AddGridDetailRowForPRRejected(itemRow);
                        }
                        else
                        {
                            AddGridDetailRowForPR(itemRow);
                        }
                        //btnSelectStockDetail.Enabled = true;
                        frmPopup.Close();
                        frmPopup.Dispose();
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
                ///btnSelectStockDetail.Enabled = true;
                frmPopup.Close();
                frmPopup.Dispose();

            }
            catch (Exception)
            {
            }
        }
        private Boolean AddGridDetailRowForPRRejected(ListViewItem itemRow)
        {
            Boolean status = true;
            try
            {
                if (grdPRDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkPRDetailGridRows())
                    {
                        return false;
                    }
                }
                foreach (DataGridViewRow row in grdPRDetail.Rows)
                {
                    if (grdPRDetail.Rows[row.Index].Cells["StockReferenceNo"].Value.ToString() == itemRow.SubItems[1].Text)
                    {
                        MessageBox.Show("Record Alreadey selected. You are not allowed to reenter again.");
                        return false;
                    }
                }
                btnAddLine.PerformClick();
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["LineNo"].Value = grdPRDetail.RowCount;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["StockReferenceNo"].Value = itemRow.SubItems[1].Text;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["StockItemID"].Value = itemRow.SubItems[2].Text;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["StockItemName"].Value = itemRow.SubItems[3].Text;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["ModelNo"].Value = itemRow.SubItems[4].Text;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["ModelName"].Value = itemRow.SubItems[5].Text;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["Quantity"].Value = Convert.ToDouble(0);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["Price"].Value = Convert.ToDouble(itemRow.SubItems[8].Text);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["Value"].Value = Convert.ToDouble(0);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["Tax"].Value = Convert.ToDouble(0);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["BatchNo"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["SerielNo"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["ExpiryDate"].Value = DateTime.Parse("1900-01-01");

                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["StoreLocationID"].Value = itemRow.SubItems[10].Text;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["StoreLocationName"].Value = itemRow.SubItems[11].Text;
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["gTaxCode"]);
                TaxCodeDB.fillTaxCodeGridViewCombo(ComboColumn2, "");
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["gTaxCode"].Value = itemRow.SubItems[9].Text;
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        //-----
        private Boolean AddGridDetailRowForPR(ListViewItem itemRow)
        {
            Boolean status = true;
            try
            {
                if (grdPRDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkPRDetailGridRows())
                    {
                        return false;
                    }
                }
                foreach (DataGridViewRow row in grdPRDetail.Rows)
                {
                    if (grdPRDetail.Rows[row.Index].Cells["StockReferenceNo"].Value.ToString() == itemRow.SubItems[1].Text)
                    {
                        MessageBox.Show("Record Alreadey selected. You are not allowed to reenter again.");
                        return false;
                    }
                }
                btnAddLine.PerformClick();
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["LineNo"].Value = grdPRDetail.RowCount;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["StockReferenceNo"].Value = itemRow.SubItems[1].Text;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["StockItemID"].Value = itemRow.SubItems[2].Text;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["StockItemName"].Value = itemRow.SubItems[3].Text;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["ModelNo"].Value = itemRow.SubItems[4].Text;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["ModelName"].Value = itemRow.SubItems[5].Text;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["Quantity"].Value = Convert.ToDouble(0);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["Price"].Value = Convert.ToDouble(itemRow.SubItems[8].Text);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["Value"].Value = Convert.ToDouble(0);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["Tax"].Value = Convert.ToDouble(0);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["BatchNo"].Value = itemRow.SubItems[9].Text;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["SerielNo"].Value = itemRow.SubItems[10].Text;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["ExpiryDate"].Value = itemRow.SubItems[11].Text;
                // string s  = itemRow.SubItems[7].Text;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["StoreLocationID"].Value = itemRow.SubItems[12].Text;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["StoreLocationName"].Value = itemRow.SubItems[13].Text;
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["gTaxCode"]);
                TaxCodeDB.fillTaxCodeGridViewCombo(ComboColumn2, "");
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["gTaxCode"].Value = itemRow.SubItems[14].Text;
                //showModelListView(itemRow.SubItems[2].Text + "-" + itemRow.SubItems[3].Text);
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            verifyAndReworkPRDetailGridRows();
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
        }
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        //private void cmbTaxCode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        chkTax = cmbTaxCode.SelectedItem.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private void cmbTaxCode_SelectedValueChanged(object sender, EventArgs e)
        //{
        //    if (grdPRDetail.Rows.Count != 0)
        //    {
        //        if (chkTax == cmbTaxCode.SelectedItem.ToString())
        //        {
        //            return;
        //        }
        //        else
        //        {
        //            DialogResult dialog = MessageBox.Show("Warning:Purchase return Tax Detail will removed?", "Yes", MessageBoxButtons.YesNo);
        //            if (dialog == DialogResult.Yes)
        //            {
        //                //grdPRDetail.Rows.
        //                foreach (DataGridViewRow row in grdPRDetail.Rows)
        //                {
        //                    grdPRDetail.Rows[row.Index].Cells["Value"].Value = "";
        //                    grdPRDetail.Rows[row.Index].Cells["Tax"].Value = "";
        //                    grdPRDetail.Rows[row.Index].Cells["TaxDetails"].Value = "";
        //                }
        //                txtProductValue.Text = "";
        //                txtTaxAmount.Text = "";
        //                txtPRValue.Text = "";
        //                btnProductValue.Text = "";
        //                btnTaxAmount.Text = "";
        //            }
        //            else
        //            {
        //                cmbTaxCode.SelectedIndex = cmbTaxCode.FindString(chkTax);
        //                return;
        //            }

        //        }


        //    }
        //}

        private void tabPRHeader_Click(object sender, EventArgs e)
        {

        }

        private void PurchaseReturnHeader_Enter(object sender, EventArgs e)
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

        private void cmbPRType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string item = cmbPRType.SelectedItem.ToString();
                if (item == "Rejected Purchase Return")
                    docID = "PURCHASERETURNQR";
                else if (item == "Accepted Purchase Return")
                    docID = "PURCHASERETURNQA";
                cmbPRType.Enabled = false;
            }
            catch(Exception ex)
            {

            }
        }
    }
}



