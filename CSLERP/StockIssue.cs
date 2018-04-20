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
using CSLERP.FileManager;

namespace CSLERP
{
    public partial class StockIssue : System.Windows.Forms.Form
    {
        Boolean track = false;
        //int no = 0;
        int yy = 0;
        string docID = "STOCKISSUE";
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Boolean New_Click = false;
        string chkTax = "";
        double productvalue = 0.0;
        double taxvalue = 0.0;
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        stockissueheader prevsih;
        DataGridView grdStock = new DataGridView();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnlForwarder = new Panel();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        string custid = "";
        Boolean isViewMode = false;
        Boolean isEditMode = false;
        Panel pnlModel = new Panel();
        Form frmPopup = new Form();
        Boolean AddRowClick = false;
        TextBox txtSearch = new TextBox();
        string product = "";
        string selModelNO = "";
        double productQuant = 0;
        int rowIndex = 0;
        public StockIssue()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }
        private void StockIssue_Load(object sender, EventArgs e)
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
            ListFilteredSIHeader(listOption);
            //tabControl1.SelectedIndexChanged += new EventHandler(TabControl1_SelectedIndexChanged);
        }
        private void ListFilteredSIHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                StockIssueDB sidb = new StockIssueDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);

                List<stockissueheader> SIHeaders = sidb.getFilteredSIHeader(userString, option);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (stockissueheader sih in SIHeaders)
                {
                    if (option == 1)
                    {
                        if (sih.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = sih.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = sih.DocumentName;

                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = sih.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = sih.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gReferenceNo"].Value = sih.ReferenceNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gReferenceDate"].Value = sih.ReferenceDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentNo"].Value = sih.DocumentNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentDate"].Value = sih.DocumentDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["IssueType"].Value = sih.IssueType;
                    grdList.Rows[grdList.RowCount - 1].Cells["ToLocation"].Value = sih.ToLocation;
                    grdList.Rows[grdList.RowCount - 1].Cells["ToLocationName"].Value = sih.ToLocationName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gRemarks"].Value = sih.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = sih.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["FrowardUser"].Value = sih.ForwardUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ApproveUser"].Value = sih.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = sih.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = sih.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = sih.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = sih.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = sih.status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = sih.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = sih.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = sih.ForwarderList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in StockIssue Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;
            isViewMode = false;
            isEditMode = false;
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
            dtTempDate.Format = DateTimePickerFormat.Custom;
            dtTempDate.CustomFormat = "dd-MM-yyyy";
            dtTempDate.Enabled = false;
            dtReferenceDate.Format = DateTimePickerFormat.Custom;
            dtReferenceDate.CustomFormat = "dd-MM-yyyy";
            dtReferenceDate.Enabled = false;
            dtDocumentDate.Format = DateTimePickerFormat.Custom;
            dtDocumentDate.CustomFormat = "dd-MM-yyyy";
            txtTemporarryNo.Enabled = false;
            dtTempDate.Enabled = false;
            txtReferenceNo.Enabled = false;
            dtReferenceDate.Enabled = false;
            btnForward.Visible = false;
            btnApprove.Visible = false;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            //dtDCDate.TabIndex = 3;
            grdSIDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //---
            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            //userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
            setTabIndex();
        }

        private void setTabIndex()
        {
            txtTemporarryNo.TabIndex = 0;
            dtTempDate.TabIndex = 1;
            txtDocumentNo.TabIndex = 2;
            dtDocumentDate.TabIndex = 3;
            cmbIssueType.TabIndex = 4;
            txtReferenceNo.TabIndex = 5;
            btnSelectRefNo.TabIndex = 6;
            dtReferenceDate.TabIndex = 7;
            txtToLocation.TabIndex = 8;
            txtRemarks.TabIndex = 9;

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
                New_Click = false;
                grdSIDetail.Rows.Clear();
                clearTabPageControls();
                isViewMode = false;
                pnlForwarder.Visible = false;
                pnllv.Visible = false;
                cmbIssueType.SelectedIndex = -1;
                track = false;
                txtTemporarryNo.Text = "";
                dtTempDate.Value = DateTime.Parse("01-01-1900");
                txtReferenceNo.Text = "";
                dtReferenceDate.Value = DateTime.Today.Date;
                txtDocumentNo.Text = "";
                dtDocumentDate.Value = DateTime.Parse("01-01-1900");
                txtRemarks.Text = "";
                txtToLocation.Text = "";
                prevsih = new stockissueheader();
                isEditMode = false;
                AddRowClick = false;
                grdStock.Rows.Clear();
                product = "";
                selModelNO = "";
                productQuant = 0;
                rowIndex = -1;
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
                track = true;

                clearData();
                isViewMode = false;
                isEditMode = false;
                btnSave.Text = "Save";
                pnlAddEdit.Visible = true;
                closeAllPanels();
                pnlAddEdit.Visible = true;
                setButtonVisibility("btnNew");
                New_Click = true;
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
                AddRowClick = true;
                AddSIDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddSIDetailRow()
        {
            Boolean status = true;
            //AddRowClick = true;
            try
            {
                if (grdSIDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkSIDetailGridRows())
                    {
                        return false;
                    }
                }
                grdSIDetail.Rows.Add();
                int kount = grdSIDetail.RowCount;
                grdSIDetail.Rows[kount - 1].Cells["LineNo"].Value = kount;
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["StockItemID"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["StockItemName"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["ModelNo"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["ModelName"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["IssueQuantity"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["StockReferenceNo"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["MRNNo"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["MRNDate"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["BatchNo"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["SerielNo"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["ExpiryDate"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["PurchaseQuantity"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["PurchasePrice"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["PurchaseTax"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["SupplierID"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["SupplierName"].Value = "";
                if (AddRowClick)
                    grdSIDetail.FirstDisplayedScrollingRowIndex = grdSIDetail.RowCount - 1;
            }

            catch (Exception ex)
            {
                MessageBox.Show("AddSIDetailRow() : Error");
            }

            return status;
        }
        private Boolean verifyAndReworkSIDetailGridRows()
        {
            Boolean status = true;

            try
            {
                if (grdSIDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Issue details");
                    return false;
                }
                if (!isViewMode)
                {
                    for (int i = 0; i < grdSIDetail.Rows.Count; i++)
                    {

                        grdSIDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                        if ((grdSIDetail.Rows[i].Cells["StockItemName"].Value.ToString().Trim().Length == 0) ||
                            (grdSIDetail.Rows[i].Cells["StockItemID"].Value.ToString().Trim().Length == 0) ||
                            (Convert.ToDouble(grdSIDetail.Rows[i].Cells["IssueQuantity"].Value.ToString().Trim().Length) == 0))
                        {
                            MessageBox.Show("Fill values in row " + (i + 1));
                            return false;
                        }
                        int str = Convert.ToInt32(grdSIDetail.Rows[i].Cells["StockReferenceNo"].Value);
                        double qunt = Convert.ToInt32(grdSIDetail.Rows[i].Cells["IssueQuantity"].Value);
                        if (!StockDB.verifyPresentStockAvailability(str, qunt))
                        {
                            MessageBox.Show("That much Stock Not Available.Check Row No:" + (i + 1));
                            return false;
                        }
                    }
                }
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
        private List<stockissuedetail> getSIDetails(stockissueheader sih)
        {
            stockissuedetail sid = new stockissuedetail();

            List<stockissuedetail> SIDetails = new List<stockissuedetail>();
            for (int i = 0; i < grdSIDetail.Rows.Count; i++)
            {
                try
                {
                    sid = new stockissuedetail();
                    sid.DocumentID = sih.DocumentID;
                    sid.TemporaryNo = sih.TemporaryNo;
                    sid.TemporaryDate = sih.TemporaryDate;
                    sid.StockItemID = grdSIDetail.Rows[i].Cells["StockItemID"].Value.ToString().Trim();
                    sid.ModelNo = grdSIDetail.Rows[i].Cells["ModelNo"].Value.ToString().Trim();
                    sid.IssueQuantity = Convert.ToDouble(grdSIDetail.Rows[i].Cells["IssueQuantity"].Value);
                    sid.MRNNo = Convert.ToInt32(grdSIDetail.Rows[i].Cells["MRNNo"].Value);
                    sid.MRNDate = Convert.ToDateTime(grdSIDetail.Rows[i].Cells["MRNDate"].Value);
                    sid.BatchNo = grdSIDetail.Rows[i].Cells["BatchNo"].Value.ToString();
                    sid.SerialNo = grdSIDetail.Rows[i].Cells["SerielNo"].Value.ToString();
                    sid.ExpiryDate = Convert.ToDateTime(grdSIDetail.Rows[i].Cells["ExpiryDate"].Value);
                    sid.PurchaseQuantity = Convert.ToDouble(grdSIDetail.Rows[i].Cells["PurchaseQuantity"].Value);
                    sid.PurchasePrice = Convert.ToDouble(grdSIDetail.Rows[i].Cells["PurchasePrice"].Value);
                    sid.PurchaseTax = Convert.ToDouble(grdSIDetail.Rows[i].Cells["PurchaseTax"].Value);
                    sid.SupplierID = grdSIDetail.Rows[i].Cells["SupplierID"].Value.ToString().Trim();
                    sid.StockReferenceNo = Convert.ToInt32(grdSIDetail.Rows[i].Cells["StockReferenceNo"].Value);
                    SIDetails.Add(sid);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("createAndUpdateSIDetails() : Error creating StockIssue Details");
                    //status = false;
                }
            }
            return SIDetails;
        }
        //private Boolean createAndUpdateSIDetails(stockissueheader sih)
        //{
        //    Boolean status = true;
        //    try
        //    {
        //        StockIssueDB sidb = new StockIssueDB();
        //        List<stockissuedetail> SIDetails = getSIDetails(sih);
        //        if (!sidb.updateSIDetail(SIDetails, sih))
        //        {
        //            MessageBox.Show("createAndUpdateSIDetails() : Failed to update Issue Details. Please check the values");
        //            status = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("createAndUpdateSIDetails() : Error updating SI Details");
        //        status = false;
        //    }
        //    return status;
        //}

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredSIHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredSIHeader(listOption);
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

            ListFilteredSIHeader(listOption);
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {

                StockIssueDB sidb = new StockIssueDB();
                stockissueheader sih = new stockissueheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;
                if (!verifyAndReworkSIDetailGridRows())
                {
                    MessageBox.Show("Validation Failed in StockIssue Detail");
                    return;
                }
                sih.DocumentID = docID;
                sih.DocumentDate = dtDocumentDate.Value;
                sih.ReferenceDate = dtReferenceDate.Value;
                sih.ReferenceNo = Convert.ToInt32(txtReferenceNo.Text);
                sih.IssueType = Convert.ToInt32(cmbIssueType.SelectedItem.ToString().Trim().Substring(0, cmbIssueType.SelectedItem.ToString().Trim().IndexOf('-')));
                sih.ToLocation = txtToLocation.Text.ToString();
                sih.Remarks = txtRemarks.Text;
                sih.ForwarderList = prevsih.ForwarderList;
                //Replacing single quotes
                sih = verifyHeaderInputString(sih);
                verifyDetailInputString();
                if (!sidb.validateSIHeader(sih))
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    //sih.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    sih.DocumentStatus = 1; //created
                    sih.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    sih.TemporaryNo = Convert.ToInt32(txtTemporarryNo.Text);
                    sih.TemporaryDate = prevsih.TemporaryDate;
                    sih.DocumentStatus = prevsih.DocumentStatus;

                }
                List<stockissuedetail> SIDetails = getSIDetails(sih);
                if (sidb.validateSIHeader(sih))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (sidb.updateSIHeaderAndDetail(sih,prevsih,SIDetails))
                        {
                            MessageBox.Show("SI Header Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredSIHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update SI Header");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (sidb.InsertSIHeaderAndDetail(sih, SIDetails))
                        {
                            MessageBox.Show("StockIssue Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredSIHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert SI  Header");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("SI Details Validation failed");
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

        private void btnCancel_Click_1(object sender, EventArgs e)
        {

            try
            {

                DialogResult dialog = MessageBox.Show("Are you sure to Cancel ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    track = true;
                    clearData();
                    closeAllPanels();
                    pnlList.Visible = true;
                    setButtonVisibility("btnEditPanel");
                }
            }
            catch (Exception)
            {
            }
           
        }

        private void btnAddLine_Click_1(object sender, EventArgs e)
        {
            AddRowClick = true;
            AddSIDetailRow();
        }
        private void grdPOPIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                string columnName = grdSIDetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdSIDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkSIDetailGridRows();
                    }
                    if (columnName.Equals("Sel"))
                    {
                        if (cmbIssueType.SelectedIndex == -1 && txtToLocation.Text.Length == 0)
                        {
                            MessageBox.Show("Select IssueType or TO LOCation");
                            return;
                        }
                        if (grdSIDetail.CurrentRow.Cells["StockReferenceNo"].Value.ToString().Length != 0)
                        {
                            DialogResult dialog = MessageBox.Show(" Stock Details will be removed ?", "Yes", MessageBoxButtons.YesNo);
                            if (dialog == DialogResult.Yes)
                            {
                                //grdSIDetail.Rows.Clear();
                                //AddSIDetailRow();
                                clearGridDetailColumnValues(e.RowIndex);
                            }
                            else
                                return;

                        }
                        rowIndex = e.RowIndex;
                        ShowItemListView();
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
        private void clearGridDetailColumnValues(int rowIndex)
        {
            grdSIDetail.Rows[rowIndex].Cells["StockReferenceNo"].Value = "";
            grdSIDetail.Rows[rowIndex].Cells["StockItemID"].Value ="";
            grdSIDetail.Rows[rowIndex].Cells["StockItemName"].Value = "";
            grdSIDetail.Rows[rowIndex].Cells["ModelNo"].Value = "";
            grdSIDetail.Rows[rowIndex].Cells["ModelName"].Value = "";
            grdSIDetail.Rows[rowIndex].Cells["IssueQuantity"].Value = "";
            grdSIDetail.Rows[rowIndex].Cells["MRNNo"].Value = "";
            grdSIDetail.Rows[rowIndex].Cells["MRNDate"].Value = "";
            grdSIDetail.Rows[rowIndex].Cells["BatchNo"].Value = "";
            grdSIDetail.Rows[rowIndex].Cells["SerielNo"].Value = "";
            grdSIDetail.Rows[rowIndex].Cells["ExpiryDate"].Value = "";
            grdSIDetail.Rows[rowIndex].Cells["PurchaseQuantity"].Value = "";
            grdSIDetail.Rows[rowIndex].Cells["PurchasePrice"].Value = "";
            grdSIDetail.Rows[rowIndex].Cells["PurchaseTax"].Value = "";
            grdSIDetail.Rows[rowIndex].Cells["SupplierID"].Value = "";
            grdSIDetail.Rows[rowIndex].Cells["SupplierName"].Value = "";
        }
        private void btnCalculateax_Click_1(object sender, EventArgs e)
        {
            verifyAndReworkSIDetailGridRows();
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
                    //isEditMode = true;
                    AddRowClick = false;
                    prevsih = new stockissueheader();
                    prevsih.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    prevsih.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    prevsih.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    prevsih.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    isEditMode = true;
                    if (columnName.Equals("View"))
                    {
                        //grdProductionPlanDetai
                        tabControl1.TabPages["tabSIDetail"].Enabled = true;
                        isViewMode = true;
                        //grdProductionPlanDetail.Columns["gProcessStatus"].Visible = true;
                        //grdProductionPlanDetail.Columns["Remarks"].Visible = true;
                        //grdProductionPlanDetail.Columns["ActualStartTime"].Visible = true;
                        //grdProductionPlanDetail.Columns["ActualEndTime"].Visible = true;
                        //grdProductionPlanDetail.Enabled = true;
                        //grdProductionPlanDetail.ReadOnly = true;
                    }
                    //PurchaseReturnHeaderDB prdb = new PurchaseReturnHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];

                    prevsih.DocumentNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentNo"].Value.ToString());
                    prevsih.DocumentDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gDocumentDate"].Value.ToString());
                    prevsih.ReferenceNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gReferenceNo"].Value.ToString());
                    prevsih.ReferenceDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gReferenceDate"].Value.ToString());
                    prevsih.IssueType = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["IssueType"].Value.ToString());
                    prevsih.ToLocation = grdList.Rows[e.RowIndex].Cells["ToLocation"].Value.ToString();

                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Document Temp No:" + prevsih.TemporaryNo + "\n" +
                            "Document Temp Date:" + prevsih.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "Doc No:" + prevsih.DocumentNo + "\n" +
                            "Doc Date:" + prevsih.DocumentDate.ToString("dd-MM-yyyy");
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevsih.TemporaryNo + "-" + prevsih.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_1(null, null);
                        return;
                    }
                    //--------
                    prevsih.Remarks = grdList.Rows[e.RowIndex].Cells["gRemarks"].Value.ToString();
                    prevsih.status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    prevsih.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    prevsih.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    prevsih.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    prevsih.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    prevsih.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevsih.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    prevsih.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    //---
                    txtTemporarryNo.Text = prevsih.TemporaryNo.ToString();
                    dtTempDate.Value = prevsih.TemporaryDate;
                    txtDocumentNo.Text = prevsih.DocumentNo.ToString();
                    try
                    {
                        dtDocumentDate.Value = prevsih.DocumentDate;
                    }
                    catch (Exception)
                    {
                        dtDocumentDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtReferenceNo.Text = prevsih.ReferenceNo.ToString();
                    try
                    {
                        dtReferenceDate.Value = prevsih.ReferenceDate;
                    }
                    catch (Exception)
                    {
                        dtReferenceDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtToLocation.Text = prevsih.ToLocation.ToString();
                    txtDocumentNo.Text = prevsih.DocumentNo.ToString();
                    track = true;
                    cmbIssueType.SelectedIndex = cmbIssueType.FindString(prevsih.IssueType.ToString());
                    track = false;
                    txtRemarks.Text = prevsih.Remarks.ToString();
                    if (prevsih.IssueType == 1)
                    {
                        ProductionPlanHeaderDB plandb = new ProductionPlanHeaderDB();
                        productionplanheader plan = plandb.getUnClosedProductionPlanForStockIssue()
                            .FirstOrDefault(p => p.ProductionPlanNo == prevsih.ReferenceNo && p.ProductionPlanDate == prevsih.ReferenceDate);
                        if(plan != null)
                        {
                            product = plan.StockItemID + "-" + plan.StockItemName;
                            productQuant = Convert.ToDouble(plan.Quantity);
                            selModelNO = plan.ModelNo;
                        }
                    }
                    List<stockissuedetail> PRDetail = StockIssueDB.getPRDetail(prevsih);
                    grdSIDetail.Rows.Clear();
                    int i = 0;
                    try
                    {
                        foreach (stockissuedetail sid in PRDetail)
                        {
                            
                            AddSIDetailRow();
                            try
                            {
                                grdSIDetail.Rows[i].Cells["StockItemID"].Value = sid.StockItemID;
                                grdSIDetail.Rows[i].Cells["StockItemName"].Value = sid.StockItemName;
                                grdSIDetail.Rows[i].Cells["ModelNo"].Value = sid.ModelNo;
                                grdSIDetail.Rows[i].Cells["ModelName"].Value = sid.ModelName;
                            }
                            catch (Exception ex)
                            {
                                grdSIDetail.Rows[i].Cells["Item"].Value = null;
                            }
                            grdSIDetail.Rows[i].Cells["StockReferenceNo"].Value = sid.StockReferenceNo;
                            grdSIDetail.Rows[i].Cells["MRNNo"].Value = sid.MRNNo;
                            grdSIDetail.Rows[i].Cells["MRNDate"].Value = sid.MRNDate;
                            grdSIDetail.Rows[i].Cells["AvailableQuantity"].Value = StockDB.getAvailiableStockQuantity(sid.StockReferenceNo);
                            grdSIDetail.Rows[i].Cells["IssueQuantity"].Value = sid.IssueQuantity;
                            grdSIDetail.Rows[i].Cells["PurchaseQuantity"].Value = sid.PurchaseQuantity;
                            grdSIDetail.Rows[i].Cells["PurchasePrice"].Value = sid.PurchasePrice;
                            grdSIDetail.Rows[i].Cells["PurchaseTax"].Value = sid.PurchaseTax;
                            grdSIDetail.Rows[i].Cells["BatchNo"].Value = sid.BatchNo;
                            grdSIDetail.Rows[i].Cells["SerielNo"].Value = sid.SerialNo;
                            grdSIDetail.Rows[i].Cells["ExpiryDate"].Value = sid.ExpiryDate;
                            grdSIDetail.Rows[i].Cells["SupplierID"].Value = sid.SupplierID;
                            grdSIDetail.Rows[i].Cells["SupplierName"].Value = sid.SupplierName;
                            
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
                //if (columnName.Equals("Edit"))
                //{
                //    verifyAndReworkSIDetailGridRows();
                //}
                btnSave.Text = "Update";
                pnlList.Visible = false;
                pnlAddEdit.Visible = true;
                tabControl1.SelectedTab = tabSIHeader;
                tabControl1.Visible = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("error");
            }
        }
        private void btnClearEntries_Click_1(object sender, EventArgs e)
        {
            try
            {
                for (int i = grdSIDetail.Rows.Count - 1; i >= 0; i--)
                {
                    grdSIDetail.Rows.RemoveAt(i);
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

        }

        private void btnApprove_Click_1(object sender, EventArgs e)
        {
            try
            {
                StockIssueDB sidb = new StockIssueDB();
                for (int i = 0; i < grdSIDetail.Rows.Count; i++)
                {
                    int str = Convert.ToInt32(grdSIDetail.Rows[i].Cells["StockReferenceNo"].Value);
                    double qunt = Convert.ToInt32(grdSIDetail.Rows[i].Cells["IssueQuantity"].Value);
                    if (!StockDB.verifyPresentStockAvailability(str, qunt))
                    {
                        MessageBox.Show("That much Stock Not Available.Not allowed to Approve. Check Row:" +( i+1));
                        return;
                    }
                }
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevsih.DocumentNo = DocumentNumberDB.getNewNumber(docID, 2);
                    if (verifyAndReworkSIDetailGridRows())
                    {
                        if (sidb.ApprovePR(prevsih))
                        {
                            MessageBox.Show("PR Document Approved");
                            List<stockissuedetail> SRDetails = getSIDetails(prevsih);
                            if (sidb.updateSIInStock(SRDetails))
                            {
                                MessageBox.Show("StockIssue Details updated in stock");
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredSIHeader(listOption);
                                setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                            }
                        }
                        else
                            MessageBox.Show("Unable to approve");
                    }
                }
            }
            catch (Exception)
            {
            }

        }
        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void pnlList_Paint(object sender, PaintEventArgs e)
        {

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
                            StockIssueDB sidb = new StockIssueDB();
                            prevsih.ForwardUser = approverUID;
                            prevsih.ForwarderList = prevsih.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (sidb.forwardPR(prevsih))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredSIHeader(listOption);
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
                    string s = prevsih.ForwarderList;
                    string reverseStr = getReverseString(prevsih.ForwarderList);
                    //do forward activities
                    StockIssueDB sidb = new StockIssueDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevsih.ForwarderList = reverseStr.Substring(0, ind);
                        prevsih.ForwardUser = reverseStr.Substring(ind + 3);
                        prevsih.DocumentStatus = prevsih.DocumentStatus - 1;
                    }
                    else
                    {
                        prevsih.ForwarderList = "";
                        prevsih.ForwardUser = "";
                        prevsih.DocumentStatus = 1;
                    }
                    if (sidb.reversePR(prevsih))
                    {
                        MessageBox.Show("StockIssue Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredSIHeader(listOption);
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
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevsih.TemporaryNo + "-" + prevsih.TemporaryDate.ToString("yyyyMMddhhmmss"));
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
                    string subDir = prevsih.TemporaryNo + "-" + prevsih.TemporaryDate.ToString("yyyyMMddhhmmss");
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
                else if (btnName == "btnNew")
                {
                    btnNew.Visible = false; //added 24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = false;
                    tabPDFViewer.Enabled = false;
                    tabControl1.SelectedTab = tabSIHeader;
                }
                else if (btnName == "btnEditPanel") //called from cancel,save,forward,approve and reverse button events
                {
                    btnNew.Visible = true;
                    btnExit.Visible = true;
                }
                //gridview buttons
                else if (btnName == "Edit")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabSIHeader;
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

                    tabControl1.SelectedTab = tabSIHeader;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabSIHeader;
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
        //call this form when new or edit buttons are clicked
        private void clearTabPageControls()
        {
            try
            {
                removePDFControls();
                removePDFFileGridView();
                removeControlsFromLVPanel();
                removeControlsFromForwarderPanel();
                grdSIDetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSelectPO_Click(object sender, EventArgs e)
        {
            //removeControlsFromLVPanel();
            //btnSelectRefNo.Enabled = false;
            if (cmbIssueType.SelectedIndex == -1)
            {
                MessageBox.Show("select issue type");
                //btnSelectRefNo.Enabled = true;
                return;
            }
            if (txtReferenceNo.Text.Length != 0)
            {
                DialogResult dialog = MessageBox.Show("Warning: Reference No and Date will removed?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    //grdSIDetail.Rows.Clear();
                    txtReferenceNo.Text = "";
                    dtReferenceDate.Value = DateTime.Parse("01-01-1900");
                }
                else
                {
                    //btnSelectRefNo.Enabled = true;
                    return;
                }
            }

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
            int type = Convert.ToInt32(cmbIssueType.SelectedItem.ToString().Substring(0, cmbIssueType.SelectedItem.ToString().IndexOf('-')));
            if (type == 1)
                lv = ProductionPlanHeaderDB.getPlanNoListView();
            else
                lv = SMRNHeaderDB.getLVServiceForStockIssue();
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
                    MessageBox.Show("Select one Item");
                    return;
                }
                else
                {
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            txtReferenceNo.Text = itemRow.SubItems[1].Text;
                            dtReferenceDate.Text = itemRow.SubItems[2].Text;
                            int type = Convert.ToInt32(cmbIssueType.SelectedItem.ToString().Substring(0, cmbIssueType.SelectedItem.ToString().IndexOf('-')));
                            if(type == 1)
                            {
                                product = itemRow.SubItems[3].Text;
                                productQuant = Convert.ToDouble(itemRow.SubItems[6].Text);
                                selModelNO = itemRow.SubItems[4].Text;
                            }
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
        private void ShowItemListView()
        {

            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(1100, 400);
            
            Label lblSearch = new Label();
            lblSearch.Location = new System.Drawing.Point(600, 5);
            lblSearch.AutoSize = true;
            lblSearch.Text = "Search by Name";
            lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
            lblSearch.ForeColor = Color.Black;
            frmPopup.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Size = new Size(220, 18);
            txtSearch.Location = new System.Drawing.Point(720, 3);
            txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
            txtSearch.ForeColor = Color.Black;
            txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInDocGridList);
            txtSearch.TabIndex = 0;
            txtSearch.Focus();
            frmPopup.Controls.Add(txtSearch);
            
            grdStock = StockDB.getGridViewOfFactoryWiseStock();
            grdStock.Bounds = new Rectangle(new Point(0, 30), new Size(1100, 320));
            frmPopup.Controls.Add(grdStock);

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
        }
        private void lvOK_Click2(object sender, EventArgs e)
        {
            try
            {
                var checkedRows = from DataGridViewRow r in grdStock.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("Select one Stock");
                    return;
                }
                foreach (var row in checkedRows)
                {
                    //return;
                    if (grdSIDetail.Rows.Count != 1)
                    {
                        for (int i = 0; i < grdSIDetail.Rows.Count; i++)
                        {
                            if (grdSIDetail.Rows[i].Cells["StockReferenceNo"].Value.ToString() == row.Cells["StockRefNo"].Value.ToString())
                            {
                                MessageBox.Show("not allowed to select same Stock Ref No");
                                return;
                            }
                        }
                        AddGridDetailRowForSI(row);
                        frmPopup.Close();
                        frmPopup.Dispose();
                    }
                    else
                    {
                        AddGridDetailRowForSI(row);
                        frmPopup.Close();
                        frmPopup.Dispose();
                    }
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

        private void lvCancel_Click2(object sender, EventArgs e)
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
        private void txtSearch_TextChangedInDocGridList(object sender, EventArgs e)
        {
            try
            {
                //grdForStockList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                filterGridDocData();
                ///grdForStockList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {

            }
        }
        private void filterGridDocData()
        {
            try
            {
                grdStock.CurrentCell = null;
                foreach (DataGridViewRow row in grdStock.Rows)
                {
                    row.Visible = true;
                    row.Cells["Select"].Value = false;
                }
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdStock.Rows)
                    {
                        if (!row.Cells["StockItemName"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
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
        //-----
        private Boolean AddGridDetailRowForSI(DataGridViewRow itemRow)
        {
            Boolean status = true;
            try
            {
                grdSIDetail.Rows[rowIndex].Cells["LineNo"].Value = grdSIDetail.RowCount;
                grdSIDetail.Rows[rowIndex].Cells["StockReferenceNo"].Value = itemRow.Cells["StockRefNo"].Value;
                grdSIDetail.Rows[rowIndex].Cells["StockItemID"].Value = itemRow.Cells["StockItemID"].Value;
                grdSIDetail.Rows[rowIndex].Cells["StockItemName"].Value = itemRow.Cells["StockItemName"].Value;
                grdSIDetail.Rows[rowIndex].Cells["ModelNo"].Value = itemRow.Cells["ModelNo"].Value;
                grdSIDetail.Rows[rowIndex].Cells["ModelName"].Value = itemRow.Cells["ModelName"].Value; 
                grdSIDetail.Rows[rowIndex].Cells["AvailableQuantity"].Value = itemRow.Cells["PresentStock"].Value;
                grdSIDetail.Rows[rowIndex].Cells["IssueQuantity"].Value = "";
                grdSIDetail.Rows[rowIndex].Cells["MRNNo"].Value = itemRow.Cells["MRNNo"].Value;
                grdSIDetail.Rows[rowIndex].Cells["MRNDate"].Value = itemRow.Cells["MRNDate"].Value;
                grdSIDetail.Rows[rowIndex].Cells["BatchNo"].Value = itemRow.Cells["BatchNo"].Value;
                grdSIDetail.Rows[rowIndex].Cells["SerielNo"].Value = itemRow.Cells["SerielNo"].Value;
                grdSIDetail.Rows[rowIndex].Cells["ExpiryDate"].Value = itemRow.Cells["ExpiryDate"].Value;
                grdSIDetail.Rows[rowIndex].Cells["PurchaseQuantity"].Value = itemRow.Cells["PurchaseQuanity"].Value;
                grdSIDetail.Rows[rowIndex].Cells["PurchasePrice"].Value = itemRow.Cells["PurchasePrice"].Value;
                grdSIDetail.Rows[rowIndex].Cells["PurchaseTax"].Value = itemRow.Cells["PurchaseTax"].Value;
                grdSIDetail.Rows[rowIndex].Cells["SupplierID"].Value = itemRow.Cells["SupplierID"].Value;
                grdSIDetail.Rows[rowIndex].Cells["SupplierName"].Value = itemRow.Cells["SupplierName"].Value;
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            // verifyAndReworkPRDetailGridRows();
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
        ////int a = 0;



        private void cmbIssueType_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (!track && isEditMode && cmbIssueType.SelectedIndex != cmbIssueType.Items.IndexOf(prevsih.IssueType + "-" + prevsih.ToLocation))
                {
                    DialogResult dialog = MessageBox.Show("Detail remove?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        txtReferenceNo.Text = "";
                        dtReferenceDate.Value = DateTime.Parse("1900-01-01");
                        txtToLocation.Text = cmbIssueType.SelectedItem.ToString().Trim().Substring(cmbIssueType.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                    }
                    else
                    {
                        track = true;
                        cmbIssueType.SelectedIndex = cmbIssueType.FindString(prevsih.IssueType.ToString());
                        track = false;
                    }
                }
                else if (New_Click)
                {
                    txtToLocation.Text = cmbIssueType.SelectedItem.ToString().Trim().Substring(cmbIssueType.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                }
            }
            catch (Exception ex)
            {
            }
        }
        //-----
        //-- Validating Header and Detail String For Single Quotes

        private stockissueheader verifyHeaderInputString(stockissueheader sih)
        {
            try
            {
                sih.Remarks = Utilities.replaceSingleQuoteWithDoubleSingleQuote(sih.Remarks);
            }
            catch (Exception ex)
            {
            }
            return sih;
        }
        private void verifyDetailInputString()
        {
            try
            {
                for (int i = 0; i < grdSIDetail.Rows.Count; i++)
                {
                    grdSIDetail.Rows[i].Cells["StockItemID"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdSIDetail.Rows[i].Cells["StockItemID"].Value.ToString());
                    grdSIDetail.Rows[i].Cells["ModelNo"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdSIDetail.Rows[i].Cells["ModelNo"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void grdSIDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    grdSIDetail.Rows[e.RowIndex].Selected = true;
                }
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

        private void btnSelBOMProd_Click(object sender, EventArgs e)
        {
            BOMDB bomdb = new BOMDB();
            
            if(product.Length == 0)
            {
                MessageBox.Show("Not able to fetch Product");
                return;
            }
            string[] ProductStrArr = product.Split('-');
            string ProdID = ProductStrArr[0];
            string ProdName = ProductStrArr[1];
            if (!BOMDB.checkBOMPrepaaredForAnItem(ProdID))
            {
                MessageBox.Show("BOM Not Available");
                return;
            }
            ShowBOMStocks bomPopUp = new ShowBOMStocks(product,productQuant, selModelNO);
            bomPopUp.ShowDialog();
            List<stock> selectedStockDetail = new List<stock>();
            if (bomPopUp.DialogResult == DialogResult.OK)
            {
                selectedStockDetail = bomPopUp.StockDetail;
            }
            else if (bomPopUp.DialogResult == DialogResult.Cancel)
            {

            }
            this.RemoveOwnedForm(bomPopUp);
            if (selectedStockDetail.Count != 0)
            {
                AddGridDetailRowForSIWRTBOM(selectedStockDetail);
            }
        }
        private Boolean AddGridDetailRowForSIWRTBOM(List<stock> stockList)
        {
            Boolean status = true;
            try
            {
                grdSIDetail.Rows.Clear();
                foreach(stock stk in stockList)
                {
                    AddRowClick = true;
                    if (!AddSIDetailRow())
                    {
                        MessageBox.Show("Adding Row Error");
                        return false;
                    }
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["LineNo"].Value = grdSIDetail.RowCount;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["StockReferenceNo"].Value = stk.RowID;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["StockItemID"].Value = stk.StockItemID;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["StockItemName"].Value = stk.StockItemName;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["ModelNo"].Value = stk.ModelNo;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["ModelName"].Value = stk.ModelName;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["AvailableQuantity"].Value = stk.PresentStock;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["IssueQuantity"].Value = stk.StockOnHold; // Issued Quanitty
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["MRNNo"].Value = stk.MRNNo;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["MRNDate"].Value = stk.MRNDate;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["BatchNo"].Value = stk.BatchNo;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["SerielNo"].Value = stk.SerielNo;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["ExpiryDate"].Value = stk.ExpiryDate;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["PurchaseQuantity"].Value = stk.PurchaseQuantity;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["PurchasePrice"].Value =stk.PurchasePrice;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["PurchaseTax"].Value = stk.PurchaseTax;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["SupplierID"].Value = stk.SupplierID;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["SupplierName"].Value = stk.SupplierName;
                }
             
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        private void StockIssue_Enter(object sender, EventArgs e)
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



