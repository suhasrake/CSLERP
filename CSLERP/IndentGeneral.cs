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
    public partial class IndentGeneral : System.Windows.Forms.Form
    {
        string docID = "INDENTGENERAL";
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        string forwarderList = "";
        string approverList = "";
        indentgeneralheader previgh;
        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Boolean userIsACommenter = false;
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        System.Data.DataTable dtCmtStatus = new DataTable();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        Form frmPopup = new Form();
        DataGridView grdPOList = new DataGridView();
        double productvalue = 0.0;
        double taxvalue = 0.0;
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        int descClickRowIndex = -1;
        RichTextBox txtDesc = new RichTextBox();
        Boolean AddRowClick = false;
        TextBox txtSearch = new TextBox();
        DataGridView grdCustList = new DataGridView();
        Boolean isViewMode = false;
        public IndentGeneral()
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
            ListFilteredIGHeader(listOption);
            //applyPrivilege();
        }
        private void ListFilteredIGHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                grdList.Columns["POPrint"].Visible = false;
                IndentGeneralDB igdb = new IndentGeneralDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                List<indentgeneralheader> IGHeaders = igdb.getFilteredIndnetGeneralHeaders(userString, option, userCommentStatusString);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (indentgeneralheader igh in IGHeaders)
                {
                    if (option == 1)
                    {
                        if (igh.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = igh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = igh.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = igh.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = igh.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentNo"].Value = igh.DocumentNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentDate"].Value = igh.DocumentDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["ReferenceNo"].Value = igh.ReferenceNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyID"].Value = igh.CurrencyID;
                    grdList.Rows[grdList.RowCount - 1].Cells["ExchangeRate"].Value = igh.ExchangeRate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyName"].Value = igh.CurrencyName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTargetDate"].Value = igh.TargetDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["PurchaseSource"].Value = igh.PurchaseSource;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = igh.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = igh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = igh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreator"].Value = igh.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gForwarder"].Value = igh.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gApprover"].Value = igh.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProductValue"].Value = igh.ProductValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = igh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProductValueINR"].Value = igh.ProductValueINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["ComntStatus"].Value = igh.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = igh.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwarderLists"].Value = igh.ForwarderList;
                    if (option == 6 || option == 3)
                    {
                        grdList.Columns["POPrint"].Visible = true;
                        if (igh.pono == 0)
                        {
                            grdList.Rows[grdList.RowCount - 1].Cells["POPrint"].Style.BackColor = Color.Red;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Indent General Listing");
            }

            setButtonVisibility("init");
            pnlList.Visible = true;
            isViewMode = false;
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
            CurrencyDB.fillCurrencyComboNew(cmbCurrency);
            dtTemporaryDate.Format = DateTimePickerFormat.Custom;
            dtTemporaryDate.Value = DateTime.Parse("1900-01-01");
            dtTemporaryDate.CustomFormat = "dd-MM-yyyy";
            dtTemporaryDate.Enabled = false;
            dtDocumntDate.Format = DateTimePickerFormat.Custom;
            dtDocumntDate.Value = DateTime.Parse("1900-01-01");
            dtDocumntDate.CustomFormat = "dd-MM-yyyy";
            dtDocumntDate.Enabled = false;
            dtTargetDate.Format = DateTimePickerFormat.Custom;
            dtTargetDate.Value = DateTime.Parse("1900-01-01");
            dtTargetDate.CustomFormat = "dd-MM-yyyy";
            dtTargetDate.Enabled = true;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            grdWODetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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
            txtDocumentNo.TabIndex = 2;
            dtDocumntDate.TabIndex = 3;
            txtReferenceNo.TabIndex = 4;
            dtTargetDate.TabIndex = 5;
            cmbCurrency.TabIndex = 6;
            txtExchangeRate.TabIndex = 7;
            txtPurchaseSource.TabIndex = 8;
            btnSelPurchaseSource.TabIndex = 9;

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
                cmbCurrency.SelectedIndex = -1;
                dtTemporaryDate.Value = DateTime.Parse("01-01-1900");
                dtDocumntDate.Value = DateTime.Parse("01-01-1900");
                dtTargetDate.Value = DateTime.Today.Date;

                grdWODetail.Rows.Clear();
                txtReferenceNo.Text = "";
                txtTemporaryNo.Text = "";
                txtDocumentNo.Text = "";
                btnTotalValueINR.Text = "";
                btnTotalValue.Text = "";
                txtPurchaseSource.Text = "";
                txtProductValue.Text = "";
                txtProductValueINR.Text = "";
                cmbCurrency.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCurrency, "INR");
                txtExchangeRate.Text = "1";
                previgh = new indentgeneralheader();
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
                tabControl1.SelectedTab = tabWOHeader;
                tabWOHeader.Enabled = true;
                setButtonVisibility("btnNew");
                AddRowClick = false;
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
                AddIGDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddIGDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdWODetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkIGDetailGridRows())
                    {
                        return false;
                    }
                }
                grdWODetail.Rows.Add();
                int kount = grdWODetail.RowCount;
                grdWODetail.Rows[kount - 1].Cells[0].Value = kount;
                grdWODetail.Rows[kount - 1].Cells["ItemDetail"].Value = " ";
                grdWODetail.Rows[kount - 1].Cells["Quantity"].Value = 0;
                grdWODetail.Rows[kount - 1].Cells["ExpectedPurchasePrice"].Value = 0;
                grdWODetail.Rows[kount - 1].Cells["WarrantyDays"].Value = 0;
                var BtnCell = (DataGridViewButtonCell)grdWODetail.Rows[kount - 1].Cells["Delete"];
                BtnCell.Value = "Del";
                if (AddRowClick)
                {
                    grdWODetail.FirstDisplayedScrollingRowIndex = grdWODetail.RowCount - 1;
                    grdWODetail.CurrentCell = grdWODetail.Rows[kount - 1].Cells[0];
                }
                grdWODetail.FirstDisplayedScrollingColumnIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddIGDetailRow() : Error");
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

        private Boolean verifyAndReworkIGDetailGridRows()
        {
            Boolean status = true;
            double total = 0;
            try
            {
                if (grdWODetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Indent General Grid details");
                    btnTotalValueINR.Text = "";
                    btnTotalValue.Text = "";
                    return false;
                }
                if (!isViewMode && (txtExchangeRate.Text.Trim().Length == 0 || Convert.ToDouble(txtExchangeRate.Text.Trim()) == 0))
                {
                    MessageBox.Show("Fill Exchange Rate");
                    return false;
                }
                for (int i = 0; i < grdWODetail.Rows.Count; i++)
                {
                    grdWODetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if ((grdWODetail.Rows[i].Cells["ItemDetail"].Value == null) ||
                        (grdWODetail.Rows[i].Cells["ItemDetail"].Value.ToString().Trim().Length == 0) ||
                        (grdWODetail.Rows[i].Cells["Quantity"].Value == null) ||
                        (grdWODetail.Rows[i].Cells["ExpectedPurchasePrice"].Value == null) ||
                        (Convert.ToDouble(grdWODetail.Rows[i].Cells["Quantity"].Value) == 0) ||
                        (Convert.ToDouble(grdWODetail.Rows[i].Cells["ExpectedPurchasePrice"].Value) == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                    double quant = Convert.ToDouble(grdWODetail.Rows[i].Cells["Quantity"].Value);
                    double price = Convert.ToDouble(grdWODetail.Rows[i].Cells["ExpectedPurchasePrice"].Value);
                    grdWODetail.Rows[i].Cells["Value"].Value = quant * price;
                    total = total + (quant * price);
                }
                btnTotalValueINR.Text = (total * Convert.ToDouble(txtExchangeRate.Text.Trim())).ToString();
                btnTotalValue.Text = total.ToString();
                txtProductValueINR.Text = (total * Convert.ToDouble(txtExchangeRate.Text.Trim())).ToString();
                txtProductValue.Text = total.ToString();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Check Values of Rows");
                return false;
            }
            return status;
        }

        private List<indentgeneraldetail> getIGDetails(indentgeneralheader igh)
        {
            List<indentgeneraldetail> IGDetails = new List<indentgeneraldetail>();
            try
            {
                indentgeneraldetail igd = new indentgeneraldetail();
                for (int i = 0; i < grdWODetail.Rows.Count; i++)
                {
                    igd = new indentgeneraldetail();
                    igd.DocumentID = igh.DocumentID;
                    igd.TemporaryNo = igh.TemporaryNo;
                    igd.TemporaryDate = igh.TemporaryDate;
                    igd.ItemDetail = grdWODetail.Rows[i].Cells["ItemDetail"].Value.ToString();
                    igd.Quantity = Convert.ToDouble(grdWODetail.Rows[i].Cells["Quantity"].Value);
                    igd.ExpectedPurchasePrice = Convert.ToDouble(grdWODetail.Rows[i].Cells["ExpectedPurchasePrice"].Value);
                    igd.WarrantyDays = Convert.ToInt32(grdWODetail.Rows[i].Cells["WarrantyDays"].Value);
                    IGDetails.Add(igd);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("getWODetails() : Error getting Indent General Details");
                IGDetails = null;
            }
            return IGDetails;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            Boolean status = true;
            try
            {

                IndentGeneralDB igdb = new IndentGeneralDB();
                indentgeneralheader igh = new indentgeneralheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    if (txtExchangeRate.Text.Trim().Length == 0 || Convert.ToDouble(txtExchangeRate.Text.Trim()) == 0)
                    {
                        MessageBox.Show("Fill Exchange Rate");
                        return;
                    }
                    if (!verifyAndReworkIGDetailGridRows())
                    {
                        return;
                    }
                    igh.DocumentID = docID;
                    igh.DocumentDate = dtDocumntDate.Value;
                    igh.ReferenceNo = txtReferenceNo.Text.ToString();
                    igh.PurchaseSource = txtPurchaseSource.Text;
                    igh.TargetDate = dtTargetDate.Value;
                    igh.CurrencyID = ((Structures.ComboBoxItem)cmbCurrency.SelectedItem).HiddenValue;
                    igh.ExchangeRate = Convert.ToDecimal(txtExchangeRate.Text);
                    igh.ProductValue = Convert.ToDouble(txtProductValue.Text);
                    igh.ProductValueINR = Convert.ToDouble(txtProductValueINR.Text);
                    igh.Comments = docCmtrDB.DGVtoString(dgvComments).Replace("'", "''"); ;
                    igh.ForwarderList = previgh.ForwarderList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    // woh.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    igh.DocumentStatus = 1; //created
                    igh.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    igh.TemporaryNo = Convert.ToInt32(txtTemporaryNo.Text);
                    igh.TemporaryDate = previgh.TemporaryDate;
                    igh.DocumentStatus = previgh.DocumentStatus;
                }
                //Replacing single quotes
                igh = verifyHeaderInputString(igh);
                verifyDetailInputString();
                if (igdb.validateIndentGeneralHeader(igh))
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
                            igh.CommentStatus = docCmtrDB.createCommentStatusString(previgh.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            igh.CommentStatus = previgh.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            igh.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            igh.CommentStatus = previgh.CommentStatus;
                        }
                    }
                    //----------
                    int tmpStatus = 1;

                    if (txtComments.Text.Trim().Length > 0)
                    {
                        igh.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus).Replace("'", "''");
                    }
                    List<indentgeneraldetail> IGDetails = getIGDetails(igh);
                    if (btnText.Equals("Update"))
                    {
                        if (igdb.updateIndentGeneralHeaderAndDetail(igh, previgh, IGDetails))
                        {
                            MessageBox.Show("Indent General details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredIGHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update Indent General ");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (igdb.InsertIndentGeneralHeaderAndDetail(igh, IGDetails))
                        {
                            MessageBox.Show("Indent General details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredIGHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert Indent General  Header");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Indent General Header Validation failed");
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
                    columnName.Equals("View") || columnName.Equals("POPrint"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    IndentGeneralDB igdb = new IndentGeneralDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    previgh = new indentgeneralheader();
                    previgh.CommentStatus = grdList.Rows[e.RowIndex].Cells["ComntStatus"].Value.ToString();
                    previgh.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    previgh.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    previgh.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    previgh.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    if (previgh.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    if (columnName.Equals("View"))
                        isViewMode = true;
                    previgh.Comments = IndentGeneralDB.getUserComments(previgh.DocumentID, previgh.TemporaryNo, previgh.TemporaryDate);

                    previgh.DocumentNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentNo"].Value.ToString());
                    previgh.DocumentDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["DocumentDate"].Value.ToString());
                    previgh.ReferenceNo = grdList.Rows[e.RowIndex].Cells["ReferenceNo"].Value.ToString();
                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Document Temp No:" + previgh.TemporaryNo + "\n" +
                            "Document Temp Date:" + previgh.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "Document No:" + previgh.DocumentNo + "\n" +
                            "Document Date:" + previgh.DocumentDate.ToString("dd-MM-yyyy");
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = previgh.TemporaryNo + "-" + previgh.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_2(null, null);
                        return;
                    }
                    //--------
                    previgh.CurrencyID = grdList.Rows[e.RowIndex].Cells["gCurrencyID"].Value.ToString();
                    previgh.CurrencyName = grdList.Rows[e.RowIndex].Cells["gCurrencyName"].Value.ToString();
                    previgh.ExchangeRate = Convert.ToDecimal(grdList.Rows[e.RowIndex].Cells["ExchangeRate"].Value.ToString());
                    previgh.TargetDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTargetDate"].Value.ToString());
                    previgh.PurchaseSource = grdList.Rows[e.RowIndex].Cells["PurchaseSource"].Value.ToString();
                    previgh.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    previgh.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    previgh.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    previgh.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    previgh.CreatorName = grdList.Rows[e.RowIndex].Cells["gCreator"].Value.ToString();
                    previgh.ForwarderName = grdList.Rows[e.RowIndex].Cells["gForwarder"].Value.ToString();
                    previgh.ApproverName = grdList.Rows[e.RowIndex].Cells["gApprover"].Value.ToString();
                    previgh.ForwarderList = grdList.Rows[e.RowIndex].Cells["ForwarderLists"].Value.ToString();

                    previgh.ProductValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["ProductValue"].Value.ToString());
                    previgh.ProductValueINR = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["ProductValueINR"].Value.ToString());
                    //--comments
                    /////chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    previgh.CommentStatus = grdList.Rows[e.RowIndex].Cells["ComntStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(previgh.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(previgh.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    cmbCurrency.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCurrency, previgh.CurrencyID);

                    txtTemporaryNo.Text = previgh.TemporaryNo.ToString();
                    try
                    {
                        dtTemporaryDate.Value = previgh.TemporaryDate;
                    }
                    catch (Exception)
                    {
                        dtTemporaryDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtDocumentNo.Text = previgh.DocumentNo.ToString();
                    try
                    {
                        dtDocumntDate.Value = previgh.DocumentDate;
                    }
                    catch (Exception)
                    {
                        dtDocumntDate.Value = DateTime.Parse("1900-01-01");
                    }
                    txtReferenceNo.Text = previgh.ReferenceNo;
                    txtExchangeRate.Text = previgh.ExchangeRate.ToString();
                    dtTargetDate.Value = previgh.TargetDate;
                    txtPurchaseSource.Text = previgh.PurchaseSource;
                    txtProductValue.Text = previgh.ProductValue.ToString();
                    txtProductValueINR.Text = previgh.ProductValueINR.ToString();

                    List<indentgeneraldetail> IGDetail = IndentGeneralDB.getIndentGeneralDetails(previgh);
                    grdWODetail.Rows.Clear();
                    int i = 0;
                    foreach (indentgeneraldetail igd in IGDetail)
                    {
                        if (!AddIGDetailRow())
                        {
                            MessageBox.Show("Error found in Indent General Detail. Please correct before updating the details");
                        }
                        else
                        {
                            grdWODetail.Rows[i].Cells["ItemDetail"].Value = igd.ItemDetail;
                            grdWODetail.Rows[i].Cells["Quantity"].Value = igd.Quantity;
                            grdWODetail.Rows[i].Cells["ExpectedPurchasePrice"].Value = igd.ExpectedPurchasePrice;
                            grdWODetail.Rows[i].Cells["WarrantyDays"].Value = igd.WarrantyDays;
                            i++;
                        }
                    }
                    if (!verifyAndReworkIGDetailGridRows())
                    {
                        MessageBox.Show("Error found in Indent General details. Please correct before updating the details");
                    }
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                    tabControl1.SelectedTab = tabWOHeader;
                    tabControl1.Visible = true;

                    if (columnName.Equals("POPrint"))
                    {
                        //PrintPurchaseOrder ppo = new PrintPurchaseOrder();
                        pnlAddEdit.Visible = false;
                        pnlList.Visible = true;
                        grdList.Visible = true;
                        btnNew.Visible = true;
                        btnExit.Visible = true;
                        List<poheader> pohdr = IndentGeneralDB.getPurchaseOrderHeader(previgh);

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
                //grdpo.Columns.Add("PORefQuation", "PORefQuation");
                //grdpo.Columns.Add("TransportationMode", "TransportationMode");
                //grdpo.Columns.Add("DeliveryAddress", "DeliveryAddress");
                //grdpo.Columns.Add("FreightTerms", "FreightTerms");
                //grdpo.Columns.Add("DeliveryPeriod", "DeliveryPeriod");
                //grdpo.Columns.Add("Taxterms", "Taxterms");
                //grdpo.Columns.Add("ModeOfPayment", "ModeOfPayment");
                //grdpo.Columns.Add("TermsAndConditions", "TermsAndConditions");
                //grdpo.Columns.Add("PaymentTerms", "PaymentTerms");
                //grdpo.Columns.Add("CurrencyID", "CurrencyID");
                //grdpo.Columns.Add("POValue", "POValue");
                //grdpo.Columns.Add("POTaxAmount", "POTaxAmount");
                //grdpo.Columns["PODOcumentID"].Frozen = true;
                //grdpo.Columns["PONo"].Frozen = true;
                //grdpo.Columns["PODate"].Frozen = true;
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
                    //grdpo.Rows[grdpo.RowCount - 1].Cells["PORefQuation"].Value = phr.ReferenceQuotation;
                    //grdpo.Rows[grdpo.RowCount - 1].Cells["TransportationMode"].Value = phr.TransportationMode;
                    //grdpo.Rows[grdpo.RowCount - 1].Cells["DeliveryAddress"].Value = phr.DeliveryAddress;
                    //grdpo.Rows[grdpo.RowCount - 1].Cells["FreightTerms"].Value = phr.FreightTerms;
                    //grdpo.Rows[grdpo.RowCount - 1].Cells["DeliveryPeriod"].Value = phr.DeliveryPeriod;
                    //grdpo.Rows[grdpo.RowCount - 1].Cells["Taxterms"].Value = phr.TaxTerms;
                    //grdpo.Rows[grdpo.RowCount - 1].Cells["ModeOfPayment"].Value = phr.ModeOfPayment;
                    //grdpo.Rows[grdpo.RowCount - 1].Cells["TermsAndConditions"].Value = phr.TermsAndCondition;
                    //grdpo.Rows[grdpo.RowCount - 1].Cells["PaymentTerms"].Value = phr.PaymentTerms;
                    //grdpo.Rows[grdpo.RowCount - 1].Cells["CurrencyID"].Value = phr.CurrencyID;
                    //grdpo.Rows[grdpo.RowCount - 1].Cells["POValue"].Value = phr.POValue;
                    //grdpo.Rows[grdpo.RowCount - 1].Cells["POTaxAmount"].Value = phr.TaxAmount;
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
                        if (poh == null)
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
            AddIGDetailRow();
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            //if (cmbTaxCode.SelectedIndex == -1)
            //{
            //    MessageBox.Show("select tax Code");
            //    return;
            //}
            if (txtExchangeRate.Text.Length == 0)
            {
                MessageBox.Show("Fill exchange rate");
                return;
            }
            verifyAndReworkIGDetailGridRows();
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
                    verifyAndReworkIGDetailGridRows();
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
                        verifyAndReworkIGDetailGridRows();
                    }
                    if (columnName.Equals("SelDesc"))
                    {
                        descClickRowIndex = e.RowIndex;
                        string strTest = grdWODetail.Rows[e.RowIndex].Cells["ItemDetail"].Value.ToString().Trim();
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
                    MessageBox.Show("Description is empty");
                    return;
                }
                grdWODetail.Rows[descClickRowIndex].Cells["ItemDetail"].Value = txtDesc.Text.Trim();
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

            ListFilteredIGHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredIGHeader(listOption);
        }
        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredIGHeader(listOption);
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
            frmPopup.ShowDialog();
        }
        private Boolean updateDashBoard(indentgeneralheader igh, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = igh.DocumentID;
                dsb.TemporaryNo = igh.TemporaryNo;
                dsb.TemporaryDate = igh.TemporaryDate;
                dsb.DocumentNo = igh.DocumentNo;
                dsb.DocumentDate = igh.DocumentDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = igh.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(previgh.DocumentID);
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
        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                IndentGeneralDB igdb = new IndentGeneralDB();
                FinancialLimitDB flDB = new FinancialLimitDB();
                if (!flDB.checkEmployeeFinancialLimit(docID, Login.empLoggedIn, Convert.ToDouble(txtProductValue.Text), 0))
                {
                    MessageBox.Show("No financial power for approving this document");
                    return;
                }
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    previgh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(previgh.CommentStatus);
                    if (previgh.Status != 96)
                    {
                        previgh.DocumentNo = DocumentNumberDB.getNewNumber(docID, 2);
                    }
                    if (igdb.ApproveIndentGeneral(previgh))
                    {
                        MessageBox.Show("indent General Document Approved");
                        if (!updateDashBoard(previgh, 2))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                        }
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredIGHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception)
            {
            }
        }


        private void txtPaymentTerms_TextChanged(object sender, EventArgs e)
        {
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
                            IndentGeneralDB igdb = new IndentGeneralDB();
                            previgh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(previgh.CommentStatus);
                            previgh.ForwardUser = approverUID;
                            previgh.ForwarderList = previgh.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (igdb.forwardIndentGeneral(previgh))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(previgh, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredIGHeader(listOption);
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
                    string reverseStr = getReverseString(previgh.ForwarderList);
                    //do forward activities
                    previgh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(previgh.CommentStatus);
                    IndentGeneralDB igdb = new IndentGeneralDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        previgh.ForwarderList = reverseStr.Substring(0, ind);
                        previgh.ForwardUser = reverseStr.Substring(ind + 3);
                        previgh.DocumentStatus = previgh.DocumentStatus - 1;
                    }
                    else
                    {
                        previgh.ForwarderList = "";
                        previgh.ForwardUser = "";
                        previgh.DocumentStatus = 1;
                    }
                    if (igdb.reverseIndentGeneral(previgh))
                    {
                        MessageBox.Show("Indent General Request Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredIGHeader(listOption);
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
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, previgh.TemporaryNo + "-" + previgh.TemporaryDate.ToString("yyyyMMddhhmmss"));
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
        private void dgvDocumentList_CellContentClick(Object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView dgv = sender as DataGridView;
                string colName = dgv.Columns[e.ColumnIndex].Name;
                string fileName = "";
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == 0)
                {
                    removePDFControls();
                    fileName = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    ////string docDir = Main.documentDirectory + "\\" + docID;
                    string subDir = previgh.TemporaryNo + "-" + previgh.TemporaryDate.ToString("yyyyMMddhhmmss");
                    dgv.Enabled = false;
                    ////DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    fileName = DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    ////showPDFFile(fileName);
                    ////dgv.Visible = false;
                    System.Diagnostics.Process.Start(fileName);
                    dgv.Enabled = true;
                }
                else if(colName == "Edit")
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
                    //chkCommentStatus.Visible = true;
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
                    //chkCommentStatus.Visible = true;
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
                dgvComments.Rows.Clear();
                ///chkCommentStatus.Checked = false;
                txtComments.Text = "";
                grdWODetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }


        private void tabWOHeader_Click(object sender, EventArgs e)
        {

        }

        private void txtExchangeRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (btnTotalValue.Text.Trim().Length != 0 && grdWODetail.Rows.Count != 0 && txtExchangeRate.Text.Trim().Length != 0)
                {
                    btnTotalValueINR.Text = (Convert.ToDouble(btnTotalValue.Text.Trim()) * Convert.ToDouble(txtExchangeRate.Text.Trim())).ToString();
                    txtProductValueINR.Text = (Convert.ToDouble(btnTotalValue.Text.Trim()) * Convert.ToDouble(txtExchangeRate.Text.Trim())).ToString();
                    txtProductValue.Text = btnTotalValue.Text;
                }
            }
            catch (Exception ex)
            {

            }

        }
        //private Boolean checkLVItemChecked(string str)
        //{
        //    Boolean status = true;
        //    try
        //    {
        //        if (lv.CheckedIndices.Count > 1)
        //        {
        //            MessageBox.Show("Only one " + str + " allowed");
        //            return false;
        //        }
        //        if (lv.CheckedItems.Count == 0)
        //        {
        //            MessageBox.Show("select one " + str);
        //            return false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return status;
        //}

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
                        DataGridView dgvDocumentList = new DataGridView();;
                        dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, previgh.TemporaryNo + "-" + previgh.TemporaryDate.ToString("yyyyMMddhhmmss"));
                        dgvDocumentList.Size = new Size(870, 300);
                        pnlPDFViewer.Controls.Add(dgvDocumentList);
                        dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
                        if (previgh.Status == 1 && previgh.DocumentStatus == 99)
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

        private indentgeneralheader verifyHeaderInputString(indentgeneralheader igh)
        {
            try
            {
                igh.ReferenceNo = Utilities.replaceSingleQuoteWithDoubleSingleQuote(igh.ReferenceNo);
                igh.PurchaseSource = Utilities.replaceSingleQuoteWithDoubleSingleQuote(igh.PurchaseSource);
            }
            catch (Exception ex)
            {
            }
            return igh;
        }
        private void verifyDetailInputString()
        {
            try
            {
                for (int i = 0; i < grdWODetail.Rows.Count; i++)
                {
                    grdWODetail.Rows[i].Cells["ItemDetail"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdWODetail.Rows[i].Cells["ItemDetail"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSelPurchaseSource_Click(object sender, EventArgs e)
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

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                verifyAndReworkIGDetailGridRows();
            }
            catch (Exception ex)
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

        private void IndentGeneral_Enter(object sender, EventArgs e)
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

