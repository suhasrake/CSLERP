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
    public partial class QIHeader : System.Windows.Forms.Form
    {
        string docID = "QUOTATIONINWARD";
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        double productvalue = 0.0;
        double taxvalue = 0.0;
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        qiheader prevqi;
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        ListView lv = new ListView();
        Panel pnlModel = new Panel();
        Form frmPopup = new Form();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        //popiheader prevpopi = new popiheader();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        Boolean userIsACommenter = false;
        TreeView tv = new TreeView();
        static TreeView tvCopy = new TreeView();
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        public QIHeader()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void QIHeader_Load(object sender, EventArgs e)
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
            ListFilteredQIHeader(listOption);
            //applyPrivilege();
        }
        private void ListFilteredQIHeader(int opt)
        {
            try
            {
                grdList.Rows.Clear();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                QIHeaderDB qihdb = new QIHeaderDB();
                List<qiheader> QIHeaders = qihdb.getFilteredQIHeader(userString, opt, userCommentStatusString);
                if (opt == 1)
                    lblActionHeader.Text = Main.currentFormDescription + " List of Action Pending Documents";
                else if (opt == 2)
                    lblActionHeader.Text = Main.currentFormDescription + " List of In-Process Documents";
                else if (opt == 3 || opt == 6)
                    lblActionHeader.Text = Main.currentFormDescription + " List of Approved Documents";
                foreach (qiheader qih in QIHeaders)
                {
                    if (opt == 1)
                    {
                        if (qih.DocumentStatus == 99)
                            continue;
                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = qih.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = qih.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerID"].Value = qih.CustomerID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerName"].Value = qih.CustomerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = qih.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = qih.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentNo"].Value = qih.DocumentNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentDate"].Value = qih.DocumentDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["QuotNo"].Value = qih.QuotationNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["QuotationDate"].Value = qih.QuotationDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["ValidityDays"].Value = qih.ValidityDays;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyID"].Value = qih.CurrencyID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyName"].Value = qih.CurrencyName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPaymentTerms"].Value = qih.PaymentTerms;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPaymentMode"].Value = qih.PaymentMode;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreditPeriod"].Value = qih.CreditPeriod;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = qih.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = qih.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = qih.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = qih.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = qih.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = qih.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatusNo"].Value = ComboFIll.getDocumentStatusString(qih.DocumentStatus);
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatusNo"].Value = qih.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CommetnStatus"].Value = qih.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = qih.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = qih.ForwarderList;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Quotation Inward Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;

            grdList.Columns["Creator"].Visible = true;
            grdList.Columns["Forwarder"].Visible = true;
            grdList.Columns["Approver"].Visible = true;
        }

        private void initVariables()
        {

            if (getuserPrivilegeStatus() == 1)
            {
                //user is only a viewer
                listOption = 6;
            }
            docID = Main.currentDocument;
            CustomerDB.fillLedgerTypeComboNew(cmbCustomer,"Supplier");
            cmbCustomer.DropDownWidth = cmbCustomer.DropDownWidth + 100;
            //CatalogueValueDB.fillCatalogValueCombo(cmbPaymentTerms, "PaymentTerms");
            CatalogueValueDB.fillCatalogValueComboNew(cmbPaymentMode, "PaymentMode");
            CurrencyDB.fillCurrencyComboNew(cmbCurrencyID);
            fillStatusCombo(cmbStatus);
            dtDocumentDate.Format = DateTimePickerFormat.Custom;
            dtDocumentDate.CustomFormat = "dd-MM-yyyy";
            dtDocumentDate.Enabled = false;
            dtQuotationDate.Format = DateTimePickerFormat.Custom;
            dtQuotationDate.CustomFormat = "dd-MM-yyyy";
            dtTemporaryDate.Format = DateTimePickerFormat.Custom;
            dtTemporaryDate.CustomFormat = "dd-MM-yyyy";
            dtQuotationDate.Enabled = true;
            txtDocumentNo.Enabled = false;
            txtQuotationNo.Enabled = true;
            //txtCreditPeriods.Enabled = true;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            cmbCustomer.TabIndex = 0;
            grdQIDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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
                //clear all grid views
                grdQIDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                //----------clear temperory panels
                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;

                cmbCustomer.SelectedIndex = -1;
                cmbStatus.SelectedIndex = -1;
                cmbCurrencyID.SelectedIndex = -1;
                txtPaymentTerms.Text = "";
                try
                {
                    cmbPaymentMode.SelectedItem = null;
                }
                catch (Exception)
                {
                }
                dtDocumentDate.Value = DateTime.Parse("01-01-1900");
                dtQuotationDate.Value = DateTime.Today;
                dtTemporaryDate.Value = DateTime.Parse("01-01-1900");
                txtTemporaryNo.Text = "";
                grdQIDetail.Rows.Clear();
                //txtCreditPeriods.Text = "";
                txtDocumentNo.Text = "";
                txtQuotationNo.Text = "";
                txtValidityDays.Text = "";
                btnProductValue.Text = "0";
                btnTaxAmount.Text = "0";
                //tabQIHeader.Visible = false;
                //tabQIDetail.Visible = false;
                prevqi = new qiheader();
                //removeControlsFromCommenterPanel();
                //removeControlsFromForwarderPanel();
                /////removeControlsFromForwarderPanelTV();
                //removeControlsFromModelPanel();
                //removeControlsFromPaymentTermsPanel();
                
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
                //pnlBottomActions.Visible = false;
                pnlAddEdit.Visible = true;
                //disableBottomButtons();
                closeAllPanels();
                pnlAddEdit.Visible = true;
                //disableBottomButtons();

                tabControl1.SelectedTab = tabQIHeader;
                setButtonVisibility("btnNew");
            }
            catch (Exception)
            {

            }
        }


        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                AddQIDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddQIDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdQIDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkQIDetailGridRows())
                    {
                        return false;
                    }
                }
                grdQIDetail.Rows.Add();
                int kount = grdQIDetail.RowCount;
                grdQIDetail.Rows[kount - 1].Cells[0].Value = kount;
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdQIDetail.Rows[kount - 1].Cells["TCode"]);
                fillTaxCodeGridViewCombo(ComboColumn2, "");
                grdQIDetail.Rows[kount - 1].Cells["Item"].Value = "";
                grdQIDetail.Rows[kount - 1].Cells["ModelName"].Value = "";
                grdQIDetail.Rows[kount - 1].Cells["ModelNo"].Value = "";
                grdQIDetail.Rows[kount - 1].Cells["ModelDetails"].Value = " ";
                grdQIDetail.Rows[kount - 1].Cells["Quantity"].Value = 0;
                grdQIDetail.Rows[kount - 1].Cells["Price"].Value = 0;
                grdQIDetail.Rows[kount - 1].Cells["Value"].Value = 0;
                grdQIDetail.Rows[kount - 1].Cells["Tax"].Value = 0;
                grdQIDetail.Rows[kount - 1].Cells["WarrantyDays"].Value = 0;
                grdQIDetail.Rows[kount - 1].Cells["TaxDetails"].Value = " ";
                var BtnCell = (DataGridViewButtonCell)grdQIDetail.Rows[kount - 1].Cells["Delete"];
                BtnCell.Value = "Del";
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddQIDetailRow() : Error");
            }

            return status;
        }
        public static void fillTaxCodeGridViewCombo(DataGridViewComboBoxCell cmb, string CategoryName)
        {
            cmb.Items.Clear();
            try
            {
                TaxCodeDB tcdb = new TaxCodeDB();
                List<taxcode> TaxCodes = tcdb.getTaxCode();
                //foreach (taxcode tc in TaxCodes)
                //{
                //    cmb.Items.Add(tc.TaxCode);
                //}
                foreach (taxcode tc in TaxCodes)
                {
                    if (tc.status == 1)
                    {
                        Structures.GridViewComboBoxItem ch =
                            new Structures.GridViewComboBoxItem(tc.Description, tc.TaxCode);
                        cmb.Items.Add(ch);
                    }
                }
                cmb.DisplayMember = "Name";  // Name Property will show(Editing)
                cmb.ValueMember = "Value";  // Value Property will save(Saving)
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //closeAllPanels();
            //btnNew.Visible = true;
            //btnExit.Visible = true;
            //pnlList.Visible = true;
            //enableBottomButtons();
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }

        private Boolean verifyAndReworkQIDetailGridRows()
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

                if (grdQIDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Quotation Inward details");
                    return false;
                }
                //clear tax details table
                TaxDetailsTable.Rows.Clear();
                for (int i = 0; i < grdQIDetail.Rows.Count; i++)
                {
                    //int i = 0;
                    grdQIDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if (((grdQIDetail.Rows[i].Cells["Item"].Value == null) &&
                        (grdQIDetail.Rows[i].Cells["ModelNo"].Value.ToString().Length == 0) ||
                        (grdQIDetail.Rows[i].Cells["ModelName"].Value.ToString().Length == 0) ||
                        (grdQIDetail.Rows[i].Cells["TCode"].Value == null)) ||
                        (grdQIDetail.Rows[i].Cells["ModelDetails"].Value == null) ||
                        (grdQIDetail.Rows[i].Cells["Quantity"].Value == null) ||
                        (grdQIDetail.Rows[i].Cells["Price"].Value == null) ||
                        (Convert.ToDouble(grdQIDetail.Rows[i].Cells["Quantity"].Value) == 0) ||
                        (Convert.ToDouble(grdQIDetail.Rows[i].Cells["Price"].Value) == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }

                    quantity = Convert.ToDouble(grdQIDetail.Rows[i].Cells["Quantity"].Value);
                    price = Convert.ToDouble(grdQIDetail.Rows[i].Cells["Price"].Value);
                    if (price != 0 && quantity != 0)
                    {
                        cost = Math.Round(quantity * price, 2);
                    }

                    try
                    {
                        strtaxCode = grdQIDetail.Rows[i].Cells["TCode"].Value.ToString().Trim();
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
                                    try
                                    {
                                        string tstr1 = TaxDetailsTable.Rows[k][0].ToString().Trim();
                                    }
                                    catch (Exception ex)
                                    {

                                    }

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
                    grdQIDetail.Rows[i].Cells["Value"].Value = Convert.ToDouble(cost.ToString());
                    grdQIDetail.Rows[i].Cells["Tax"].Value = ttax2.ToString();
                    grdQIDetail.Rows[i].Cells["TaxDetails"].Value = strTax;
                    productvalue = productvalue + cost;
                    taxvalue = taxvalue + ttax2;


                    btnProductValue.Text = productvalue.ToString();
                    btnTaxAmount.Text = taxvalue.ToString();
                }
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
                if (docID == "QUOTATIONINWARD")
                {

                    for (int i = 0; i < grdQIDetail.Rows.Count - 1; i++)
                    {
                        for (int j = i + 1; j < grdQIDetail.Rows.Count; j++)
                        {

                            if (grdQIDetail.Rows[i].Cells["Item"].Value.ToString() == grdQIDetail.Rows[j].Cells["Item"].Value.ToString() &&
                                grdQIDetail.Rows[i].Cells["ModelNo"].Value.ToString() == grdQIDetail.Rows[j].Cells["ModelNo"].Value.ToString())
                            {
                                //duplicate item code
                                MessageBox.Show("Item code duplicated in OB details... please ensure correctness (" +
                                    grdQIDetail.Rows[i].Cells["Item"].Value.ToString() + ")");
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

        private List<qidetail> getQIDetails(qiheader qih)
        {
            List<qidetail> QIDetails = new List<qidetail>();
            try
            {
                qidetail qid = new qidetail();
                for (int i = 0; i < grdQIDetail.Rows.Count; i++)
                {
                    qid = new qidetail();
                    qid.DocumentID = qih.DocumentID;
                    qid.TemporaryNo = qih.TemporaryNo;
                    qid.TemporaryDate = qih.TemporaryDate;
                    qid.StockItemID = grdQIDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Substring(0, grdQIDetail.Rows[i].Cells["Item"].Value.ToString().Trim().IndexOf('-'));
                    qid.TaxCode = grdQIDetail.Rows[i].Cells["TCode"].Value.ToString();
                    qid.ModelNo = grdQIDetail.Rows[i].Cells["ModelNo"].Value.ToString().Trim();
                    qid.ModelName = grdQIDetail.Rows[i].Cells["ModelName"].Value.ToString().Trim();
                    qid.ModelDetails = grdQIDetail.Rows[i].Cells["ModelDetails"].Value.ToString().Trim();
                    qid.Quantity = Convert.ToDouble(grdQIDetail.Rows[i].Cells["Quantity"].Value);
                    qid.Price = Convert.ToDouble(grdQIDetail.Rows[i].Cells["Price"].Value);
                    qid.Tax = Convert.ToDouble(grdQIDetail.Rows[i].Cells["Tax"].Value);
                    qid.WarrantyDays = Convert.ToInt32(grdQIDetail.Rows[i].Cells["WarrantyDays"].Value);
                    qid.TaxDetails = grdQIDetail.Rows[i].Cells["TaxDetails"].Value.ToString();

                    QIDetails.Add(qid);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("getQIDetails() : Error getting Quotation Details");
                QIDetails = null;
            }
            return QIDetails;
        }
        //private void disableBottomButtons()
        //{
        //    btnNew.Visible = false;
        //    btnExit.Visible = false;
        //}
        //private void enableBottomButtons()
        //{
        //    btnNew.Visible = true;
        //    btnExit.Visible = true;
        //    pnlBottomActions.Visible = true;
        //} 
        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredQIHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredQIHeader(listOption);
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

            ListFilteredQIHeader(listOption);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            Boolean status = true;
            try
            {

                QIHeaderDB qidb = new QIHeaderDB();
                qiheader qih = new qiheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    if (!verifyAndReworkQIDetailGridRows())
                    {
                        return;
                    }
                    qih.DocumentID = docID;
                    ////////qih.CustomerID = cmbCustomer.SelectedItem.ToString().Trim().Substring(0, cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-'));
                    qih.CustomerID = ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue;
                    qih.DocumentDate = dtDocumentDate.Value;
                    qih.QuotationNo = txtQuotationNo.Text;
                    qih.QuotationDate = dtQuotationDate.Value;
                    //qih.DocumentDate = dtQuotationDate.Value;
                    qih.ValidityDays = Convert.ToInt32(txtValidityDays.Text);
                    //////////qih.CurrencyID = cmbCurrencyID.SelectedItem.ToString().Trim().Substring(0, cmbCurrencyID.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
                    qih.CurrencyID = ((Structures.ComboBoxItem)cmbCurrencyID.SelectedItem).HiddenValue;
                    qih.PaymentTerms = txtPaymentTerms.Text;
                    qih.PaymentMode = ((Structures.ComboBoxItem)cmbPaymentMode.SelectedItem).HiddenValue;
                    //qih.PaymentMode = cmbPaymentMode.SelectedItem.ToString().Trim().Substring(0, cmbPaymentMode.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
                    //qih.CreditPeriod = Convert.ToInt32(txtCreditPeriods.Text);
                    //qih.Status = ComboFIll.getStatusCode(cmbStatus.SelectedItem.ToString());
                    qih.Comments = docCmtrDB.DGVtoString(dgvComments);
                    qih.ForwarderList = prevqi.ForwarderList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    //qih.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    qih.DocumentStatus = 1; //created
                    qih.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    qih.TemporaryNo = Convert.ToInt32(txtTemporaryNo.Text);
                    qih.TemporaryDate = prevqi.TemporaryDate;
                }

                if (qidb.validateQIHeader(qih))
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
                            qih.CommentStatus = docCmtrDB.createCommentStatusString(prevqi.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            qih.CommentStatus = prevqi.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            qih.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            qih.CommentStatus = prevqi.CommentStatus;
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
                        qih.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus);
                    }
                    List<qidetail> QIDetails = getQIDetails(qih);
                    if (btnText.Equals("Update"))
                    {
                        if (qidb.updateQIHeaderAndDetail(qih, prevqi,QIDetails))
                        {
                            MessageBox.Show("Quotation Inward Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredQIHeader(listOption);
                        }
                        else
                        {
                            status = false;

                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update QI Product Inward Header");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (qidb.InsertQIHeaderAndDetail(qih, QIDetails))
                        {
                            MessageBox.Show("QI Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredQIHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert Quotation Header");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Quotation Header Validation failed");
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("createAndUpdateQIDetails() : Error");
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
                    QIHeaderDB qidb = new QIHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevqi = new qiheader();
                    prevqi.CommentStatus = grdList.Rows[e.RowIndex].Cells["CommetnStatus"].Value.ToString();
                    prevqi.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    prevqi.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    prevqi.CustomerID = grdList.Rows[e.RowIndex].Cells["gCustomerID"].Value.ToString();
                    prevqi.CustomerName = grdList.Rows[e.RowIndex].Cells["gCustomerName"].Value.ToString();
                    prevqi.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    prevqi.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    prevqi.DocumentNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentNo"].Value.ToString());
                    prevqi.DocumentDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["DocumentDate"].Value.ToString());
                    if (prevqi.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    prevqi.Comments = QIHeaderDB.getUserComments(prevqi.DocumentID, prevqi.TemporaryNo, prevqi.TemporaryDate);

                    prevqi.QuotationNo = grdList.Rows[e.RowIndex].Cells["QuotNo"].Value.ToString();
                    prevqi.QuotationDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["QuotationDate"].Value.ToString());

                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Temporary No:" + prevqi.TemporaryNo + "\n" +
                            "Temporary Date:" + prevqi.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "Document No:" + prevqi.DocumentNo + "\n" +
                            "Document Date:" + prevqi.DocumentDate.ToString("dd-MM-yyyy") + "\n" +
                            "Customer:" + prevqi.CustomerName;
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevqi.DocumentNo + "-" + prevqi.DocumentDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_2(null, null);
                        return;
                    }
                    //--------
                    prevqi.ValidityDays = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["ValidityDays"].Value.ToString());
                    prevqi.CurrencyID = grdList.Rows[e.RowIndex].Cells["gCurrencyID"].Value.ToString();
                    prevqi.PaymentTerms = grdList.Rows[e.RowIndex].Cells["gPaymentTerms"].Value.ToString();
                    prevqi.CreditPeriod = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gCreditPeriod"].Value.ToString());
                    prevqi.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    prevqi.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    prevqi.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    prevqi.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    prevqi.PaymentTerms = grdList.Rows[e.RowIndex].Cells["gPaymentTerms"].Value.ToString();
                    prevqi.PaymentMode = grdList.Rows[e.RowIndex].Cells["gPaymentMode"].Value.ToString();
                    prevqi.CreditPeriod = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gCreditPeriod"].Value.ToString());

                    prevqi.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentStatusNo"].Value.ToString());
                    prevqi.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevqi.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    prevqi.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();

                    //--comments
                    chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlCommetns.Controls.Remove(dgvComments);
                    prevqi.CommentStatus = grdList.Rows[e.RowIndex].Cells["CommetnStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(prevqi.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(prevqi.Comments);
                    pnlCommetns.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---
                    ////////cmbCustomer.SelectedIndex = cmbCustomer.FindString(prevqi.CustomerID);
                    cmbCustomer.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCustomer, prevqi.CustomerID);
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(ComboFIll.getStatusString(Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString())));
                    cmbCurrencyID.SelectedIndex = cmbCurrencyID.FindString(prevqi.CurrencyID);
                    cmbCurrencyID.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCurrencyID, prevqi.CurrencyID);
                    txtDocumentNo.Text = prevqi.DocumentNo.ToString();
                    txtTemporaryNo.Text = prevqi.TemporaryNo.ToString();
                    dtTemporaryDate.Value = prevqi.TemporaryDate;
                    try
                    {
                        dtDocumentDate.Value = prevqi.DocumentDate;
                    }
                    catch (Exception)
                    {

                        dtDocumentDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtQuotationNo.Text = prevqi.QuotationNo;
                    try
                    {
                        dtQuotationDate.Value = prevqi.QuotationDate;
                    }
                    catch (Exception)
                    {
                        dtQuotationDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtPaymentTerms.Text = prevqi.PaymentTerms;
                    // cmbPaymentTerms.SelectedIndex = cmbPaymentTerms.FindString(prevqi.PaymentTerms);
                    cmbPaymentMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbPaymentMode, prevqi.PaymentMode);
                    //cmbPaymentMode.SelectedIndex = cmbPaymentMode.FindString(prevqi.PaymentMode);
                    //txtCreditPeriods.Text = prevqi.CreditPeriod.ToString();
                    txtValidityDays.Text = prevqi.ValidityDays.ToString();
                    List<qidetail> QIDetail = QIHeaderDB.getQIDetail(prevqi);
                    grdQIDetail.Rows.Clear();
                    int i = 0;
                    foreach (qidetail qid in QIDetail)
                    {
                        if (!AddQIDetailRow())
                        {
                            MessageBox.Show("Error found in QI details. Please correct before updating the details");
                        }
                        else
                        {
                            grdQIDetail.Rows[i].Cells["Item"].Value = qid.StockItemID + "-" + qid.StockItemName;
                            grdQIDetail.Rows[i].Cells["TCode"].Value = qid.TaxCode;
                            grdQIDetail.Rows[i].Cells["ModelNo"].Value = qid.ModelNo;
                            grdQIDetail.Rows[i].Cells["ModelName"].Value = qid.ModelName;
                            grdQIDetail.Rows[i].Cells["ModelDetails"].Value = qid.ModelDetails;
                            grdQIDetail.Rows[i].Cells["Quantity"].Value = qid.Quantity;
                            grdQIDetail.Rows[i].Cells["Price"].Value = qid.Price;
                            grdQIDetail.Rows[i].Cells["Value"].Value = qid.Quantity * qid.Price;
                            grdQIDetail.Rows[i].Cells["Tax"].Value = qid.Tax;
                            grdQIDetail.Rows[i].Cells["WarrantyDays"].Value = qid.WarrantyDays;
                            grdQIDetail.Rows[i].Cells["TaxDetails"].Value = qid.TaxDetails;
                            i++;
                            productvalue = productvalue + qid.Quantity * qid.Price;
                            taxvalue = taxvalue + qid.Tax;
                        }
                    }
                    if (!verifyAndReworkQIDetailGridRows())
                    {
                        MessageBox.Show("Error found in QI details. Please correct before updating the details");
                    }
                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;

                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                    tabControl1.SelectedTab = tabQIHeader;
                    tabControl1.Visible = true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
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
            AddQIDetailRow();
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            verifyAndReworkQIDetailGridRows();
        }

        private void ClearEntries_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = grdQIDetail.Rows.Count - 1; i >= 0; i--)
                {
                    grdQIDetail.Rows.RemoveAt(i);
                }
            }
            catch (Exception)
            {
            }
            MessageBox.Show("item cleared.");
        }

        private void grdQIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdQIDetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdQIDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkQIDetailGridRows();
                    }
                    if (columnName.Equals("TaxView"))
                    {
                        //show tax details
                        DialogResult dialog = MessageBox.Show(grdQIDetail.Rows[e.RowIndex].Cells["TaxDetails"].Value.ToString(),
                            "Line No : " + (e.RowIndex + 1), MessageBoxButtons.OK);
                    }
                    if (columnName.Equals("Sel"))
                    {
                        showStockItemTreeView();
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
        private void showStockItemTreeView()
        {
            pnlForwarder.Controls.Clear();
            removeControlsFromForwarderPanel();
            if (!checkAvailabilityOfitem())
            {
                DialogResult dialog = MessageBox.Show("Selected product and Model detail will removed?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdQIDetail.CurrentRow.Cells["Item"].Value = "";
                    grdQIDetail.CurrentRow.Cells["ModelNo"].Value = "";
                    grdQIDetail.CurrentRow.Cells["ModelName"].Value = "";
                }
                else
                    return;
            }
            try
            {
                tv = new TreeView();
                tv.CheckBoxes = true;
                tv.Nodes.Clear();
                tv.CheckBoxes = true;
                pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
                pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
                tv = StockItemDB.getStockItemTreeView(); ;
                tv.Bounds = new Rectangle(new Point(50, 50), new Size(600, 200));
            }
            catch (Exception)
            {
            }
            pnlForwarder.Controls.Remove(tv);
            pnlForwarder.Controls.Add(tv);
            //tv.cl
            tv.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterCheck);
            Button lvForwrdOK = new Button();
            lvForwrdOK.Text = "OK";
            lvForwrdOK.Size = new Size(150, 20);
            lvForwrdOK.Location = new Point(50, 270);
            lvForwrdOK.Click += new System.EventHandler(this.tvOK_Click);
            pnlForwarder.Controls.Add(lvForwrdOK);

            Button lvForwardCancel = new Button();
            lvForwardCancel.Text = "Cancel";
            lvForwardCancel.Size = new Size(150, 20);
            lvForwardCancel.Location = new Point(250, 270);
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
                    grdQIDetail.CurrentRow.Cells["Item"].Value = s;
                    tv.CheckBoxes = true;
                    pnlForwarder.Controls.Remove(lvApprover);
                    pnlForwarder.Visible = false;
                    showModelListView(s);
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
                pnlForwarder.Controls.Remove(lvApprover);
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
        private Boolean checkAvailabilityOfitem()
        {
            Boolean status = true;
            if (grdQIDetail.CurrentRow.Cells["Item"].Value.ToString().Length != 0 ||
                grdQIDetail.CurrentRow.Cells["ModelNo"].Value.ToString().Length != 0 ||
                grdQIDetail.CurrentRow.Cells["ModelName"].Value.ToString().Length != 0)
            {
                status = false;
            }
            return status;
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
                grdQIDetail.CurrentRow.Cells["ModelNo"].Value = "NA";
                grdQIDetail.CurrentRow.Cells["ModelName"].Value = "NA";
                pnlModel.Visible = false;
                return;
            }
            lv.Bounds = new Rectangle(new Point(0, 25), new Size(450, 200));
            ///frmPopup.Controls.Remove(lv);
            frmPopup.Controls.Add(lv);
            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.BackColor = Color.Tan;
            lvOK.Location = new System.Drawing.Point(40, 280);
            lvOK.Click += new System.EventHandler(this.lvOK_Click3);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new System.Drawing.Point(130, 280);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click3);
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
        private void lvOK_Click3(object sender, EventArgs e)
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
                        grdQIDetail.CurrentRow.Cells["ModelNo"].Value = item.SubItems[1].Text;
                        grdQIDetail.CurrentRow.Cells["ModelName"].Value = item.SubItems[2].Text;
                        frmPopup.Close();
                        frmPopup.Dispose();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void lvCancel_Click3(object sender, EventArgs e)
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

        //private void cmbPaymentTerms_SelectedIndexChanged_1(object sender, EventArgs e)
        //{

        //    if (cmbPaymentTerms.SelectedItem.ToString().Trim().Substring(0, cmbPaymentTerms.SelectedItem.ToString().Trim().IndexOf('-')).Trim().Equals("Credit"))
        //    {
        //        txtCreditPeriods.Enabled = true;
        //    }
        //    else
        //    {
        //        txtCreditPeriods.Text = "0";
        //        txtCreditPeriods.Enabled = false;
        //    }
        //}
        private void btnForward_Click(object sender, EventArgs e)
        {
            //pnlForwarder.Controls.Clear();
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
                            QIHeaderDB popihdb = new QIHeaderDB();
                            prevqi.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevqi.CommentStatus);
                            prevqi.ForwardUser = approverUID;
                            prevqi.ForwarderList = prevqi.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (popihdb.forwardQI(prevqi))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(prevqi, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredQIHeader(listOption);
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
        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                QIHeaderDB qidb = new QIHeaderDB();
                FinancialLimitDB flDB = new FinancialLimitDB();
                if (!flDB.checkEmployeeFinancialLimit(docID, Login.empLoggedIn, Convert.ToDouble(btnProductValue.Text), 0))
                {
                    MessageBox.Show("No financial power for approving this document");
                    return;
                }
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevqi.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevqi.CommentStatus);
                    prevqi.DocumentNo = DocumentNumberDB.getNewNumber(docID, 2);
                    if (qidb.ApproveQI(prevqi))
                    {
                        MessageBox.Show("QI Approved");
                        if (!updateDashBoard(prevqi, 2))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                        }
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredQIHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private Boolean updateDashBoard(qiheader qih, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = qih.DocumentID;
                dsb.TemporaryNo = qih.TemporaryNo;
                dsb.TemporaryDate = qih.TemporaryDate;
                dsb.DocumentNo = qih.DocumentNo;
                dsb.DocumentDate = qih.DocumentDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = qih.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(prevqi.DocumentID);
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
                    tabControl1.SelectedTab = tabQIHeader;
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
                    tabControl1.SelectedTab = tabQIHeader;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                    disableTabPages();
                    tabControl1.SelectedTab = tabQIHeader;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabQIHeader;
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
        private void clearTabPageControls()
        {
            try
            {
                removePDFControls();
                removePDFFileGridView();
                removeControlsFromCommenterPanel();
                removeControlsFromForwarderPanel();
                removeControlsFromModelPanel();
                removeControlsFromPaymentTermsPanel();
                dgvComments.Rows.Clear();
                chkCommentStatus.Checked = false;
                txtComments.Text = "";
                grdQIDetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }
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
        private void btnReverse_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Are you sure to Reverse the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    string reverseStr = getReverseString(prevqi.ForwarderList);
                    //do forward activities
                    prevqi.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevqi.CommentStatus);
                    QIHeaderDB qidb = new QIHeaderDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevqi.ForwarderList = reverseStr.Substring(0, ind);
                        prevqi.ForwardUser = reverseStr.Substring(ind + 3);
                        prevqi.DocumentStatus = prevqi.DocumentStatus - 1;
                    }
                    else
                    {
                        prevqi.ForwarderList = "";
                        prevqi.ForwardUser = "";
                        prevqi.DocumentStatus = 1;
                    }
                    if (qidb.reverseQI(prevqi))
                    {
                        MessageBox.Show("QI Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredQIHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception ex)
            {
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
        private void btnGetComments_Click(object sender, EventArgs e)
        {
            ///removeControlsFromCommenterPanel();

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
            //pnlCommetns.Controls.Add(pnlCmtr);
            //pnlCommetns.BringToFront();
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
        private void btnListDocuments_Click_1(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevqi.DocumentNo + "-" + prevqi.DocumentDate.ToString("yyyyMMddhhmmss"));
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
        //private void removeControlsFromForwarderPanelTV()
        //{
        //    try
        //    {
        //        //foreach (Control p in pnlForwarder.Controls)
        //        //    if (p.GetType() == typeof(TreeView) || p.GetType() == typeof(Button))
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
        private void removeControlsFromPaymentTermsPanel()
        {
            try
            {
                //foreach (Control p in pnlForwarder.Controls)
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
                    string subDir = prevqi.DocumentNo + "-" + prevqi.DocumentDate.ToString("yyyyMMddhhmmss");
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


        private void btnSelectPaymentTerms_Click(object sender, EventArgs e)
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
                removeControlsFromFrmPopup();
                frmPopup.Close();
                frmPopup.Dispose();

            }
            catch (Exception ex)
            {
            }
        }
        private void removeControlsFromFrmPopup()
        {
            frmPopup.Controls.Clear();
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            removeControlsFromCommenterPanel();
            removeControlsFromForwarderPanel();
            //removeControlsFromForwarderPanelTV();
            removeControlsFromModelPanel();
            removeControlsFromPaymentTermsPanel();
        }

        private void QIHeader_Enter(object sender, EventArgs e)
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

