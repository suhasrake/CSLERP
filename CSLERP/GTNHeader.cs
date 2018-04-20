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
    public partial class GTNHeader : System.Windows.Forms.Form
    {
        string docID = "GTN";
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        //double productvalue = 0.0;
        //double taxvalue = 0.0;
        Boolean IsEditMode = false;
        Boolean IsViewMode = false;
        Boolean userIsACommenter = false;
        Boolean track = false;
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        gtnheader prevgtn;
        //System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        Panel pnlModel = new Panel();
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        int ch = 0;
        Form frmPopup = new Form();
        DataGridView grdForStockList = new DataGridView();
        TextBox txtSearch = new TextBox();
        Boolean AddRowClick = false;
        public GTNHeader()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {

            }
        }

        private void GTNHeader_Load(object sender, EventArgs e)
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
            ListFilteredGTNHeader(listOption);
        }
        private void ListFilteredGTNHeader(int option)
        {
            //int a = 0;
            try
            {

                grdList.Rows.Clear();
                GTNDB gdb = new GTNDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);

                List<gtnheader> GTNHeaders = gdb.getFilteredGTNHeader(userString, option, userCommentStatusString);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (gtnheader gtnh in GTNHeaders)
                {
                    if (option == 1)
                    {
                        if (gtnh.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = gtnh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = gtnh.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = gtnh.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = gtnh.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["GTNNo"].Value = gtnh.GTNNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["GTNDate"].Value = gtnh.GTNDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["FromLocation"].Value = gtnh.FromLocation;
                    grdList.Rows[grdList.RowCount - 1].Cells["FromLocationName"].Value = gtnh.FromLocationName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ToLocation"].Value = gtnh.ToLocation;
                    grdList.Rows[grdList.RowCount - 1].Cells["ToLocationName"].Value = gtnh.ToLocationName;
                    grdList.Rows[grdList.RowCount - 1].Cells["MaterialValue"].Value = gtnh.MaterialValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["gRemarks"].Value = gtnh.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = gtnh.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["CmtStatus"].Value = gtnh.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["AcceptanceDate"].Value = gtnh.AcceptDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["AcceptanceStatus"].Value = gtnh.AcceptStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["AcceptedUser"].Value = gtnh.AcceptUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = gtnh.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = gtnh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = gtnh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = gtnh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = gtnh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = gtnh.ForwardUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = gtnh.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = gtnh.ForwarderList;
                    grdList.Rows[grdList.RowCount - 1].Cells["ReceiveStatus"].Value = gtnh.ReceiveStatus;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GTN Product Inward Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;
            //if(a == 1)
            //{
            //    grdList.Columns["Edit"].Visible = false;
            //}
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
            StoreEmpMappingDB.fillLocationCombo(cmbFromLocation);
            //StoreEmpMappingDB.fillLocationCombo(cmbTOLocation);
            //fillCumboToLocation();
            //StoreEmpMappingDB.fillToLocationCombo(cmbTOLocation, Login.empLoggedIn);
            cmbTOLocation.Width = 250;
            cmbFromLocation.Width = 250;
            dtTempDate.Format = DateTimePickerFormat.Custom;
            dtTempDate.CustomFormat = "dd-MM-yyyy";
            dtTempDate.Enabled = false;
            dtGTNDate.Format = DateTimePickerFormat.Custom;
            dtGTNDate.CustomFormat = "dd-MM-yyyy";
            dtGTNDate.Enabled = false;
            txtTemporarryNo.Enabled = false;
            dtTempDate.Enabled = false;
            txtGTNNo.Enabled = false;
            dtGTNDate.Enabled = false;
            btnForward.Visible = false;
            //btnApprove.Visible = false;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            grdGTNDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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
            txtGTNNo.TabIndex = 2;
            dtGTNDate.TabIndex = 3;
            cmbFromLocation.TabIndex = 4;
            cmbTOLocation.TabIndex = 5;
            txtMaterialValue.TabIndex = 6;
            txtRemarks.TabIndex = 7;

            btnForward.TabIndex = 0;
            btnCancel.TabIndex = 1;
            btnSave.TabIndex = 2;
            btnReverse.TabIndex = 3;
            btnSendStock.TabIndex = 4;
            btnAccept.TabIndex = 4;
        }
        private void fillCumboToLocation()
        {

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
                grdGTNDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                //----------clear temperory panels
                ///cmbTOLocation.Enabled = false;
                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                //----------
                cmbTOLocation.SelectedIndex = -1;
                cmbFromLocation.SelectedIndex = -1;
                txtTemporarryNo.Text = "";
                dtTempDate.Value = DateTime.Parse("01-01-1900");
                txtGTNNo.Text = "";
                dtGTNDate.Value = DateTime.Parse("01-01-1900");
                txtRemarks.Text = "";
                txtMaterialValue.Text = "";
                grdGTNDetail.Columns["Unit"].Visible = true;
                grdGTNDetail.Columns["AvailableQuantity"].Visible = true;
                track = false;
                prevgtn = new gtnheader();
                AddRowClick = false;
                //removeControlsFromPnllvPanel();
                //IsViewMode = false;
                //IsEditMode = false;
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
                tabControl1.SelectedTab = tabGTNHeader;
                IsEditMode = false;
                //cmbTOLocation.Items.Clear();
                setButtonVisibility("btnNew");
                AddRowClick = false;
                // cmbTOLocation.Enabled = false;

            }
            catch (Exception)
            {

            }
        }


        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                AddGTNDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddGTNDetailRow()
        {
            Boolean status = true;
            //AddRowClick = true;
            try
            {
                if (grdGTNDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkGTNDetailGridRows())
                    {
                        return false;
                    }
                }
                grdGTNDetail.Rows.Add();
                int kount = grdGTNDetail.RowCount;
                grdGTNDetail.Rows[kount - 1].Cells["LineNo"].Value = kount;
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["Item"].Value = "";
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["AvailableQuantity"].Value = "";
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["ModelNo"].Value = "";
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["ModelName"].Value = "";
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["Quantity"].Value = "";
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["StockRefNo"].Value = "";
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["MRNNo"].Value = "";
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["MRNDate"].Value = "";
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["BatchNo"].Value = "";
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["SerialNo"].Value = "";
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["ExpiryDate"].Value = "";
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["PurchaseQuantity"].Value = "";
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["PurchasePrice"].Value = "";
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["PurchaseTax"].Value = "";
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["SupplierID"].Value = "";
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["SupplierName"].Value = "";
                if (AddRowClick)
                {
                    grdGTNDetail.FirstDisplayedScrollingRowIndex = grdGTNDetail.RowCount - 1;
                    grdGTNDetail.CurrentCell = grdGTNDetail.Rows[kount - 1].Cells[0];
                }
                //grdPODetail.Columns[0].Frozen = false;
                grdGTNDetail.FirstDisplayedScrollingColumnIndex = 0;
                //grdPODetail.Columns["selDesc"].Frozen = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("AddGTNDetailRow() : Error");
            }

            return status;
        }

        private Boolean verifyAndReworkGTNDetailGridRows()
        {
            Boolean status = true;

            try
            {

                if (grdGTNDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in GTN details");
                    return false;
                }
                for (int i = 0; i < grdGTNDetail.Rows.Count; i++)
                {

                    grdGTNDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if ((grdGTNDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Length == 0) ||
                        (grdGTNDetail.Rows[i].Cells["ModelNo"].Value.ToString().Trim().Length == 0) ||
                        (grdGTNDetail.Rows[i].Cells["ModelName"].Value.ToString().Trim().Length == 0) ||
                        (grdGTNDetail.Rows[i].Cells["Quantity"].Value == null) ||
                        (grdGTNDetail.Rows[i].Cells["Quantity"].Value.ToString().Length == 0) ||
                        Convert.ToDouble(grdGTNDetail.Rows[i].Cells["Quantity"].Value) == 0)
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }

                    if (!IsViewMode)
                    {
                        GTNDB gtndb = new GTNDB();
                        int refID = Convert.ToInt32(grdGTNDetail.Rows[i].Cells["StockRefNo"].Value.ToString());
                        double Quant = Convert.ToDouble(grdGTNDetail.Rows[i].Cells["Quantity"].Value);
                        if (!gtndb.CheckStockAvailability(refID, Quant))
                        {
                            MessageBox.Show(" Quantity Not available. Recheck Entered Quantity. Check row: " + (i + 1));
                            return false;
                        }
                    }

                }
                if (!IsViewMode)
                {
                    if (!validateItems())
                    {
                        //MessageBox.Show("Validation failed");
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return status;
        }

        //check for item duplication in details
        private Boolean validateItems()
        {
            Boolean status = true;
            try
            {
                for (int i = 0; i < grdGTNDetail.Rows.Count - 1; i++)
                {
                    for (int j = i + 1; j < grdGTNDetail.Rows.Count; j++)
                    {

                        if (grdGTNDetail.Rows[i].Cells[1].Value.ToString() == grdGTNDetail.Rows[j].Cells["Item"].Value.ToString() &&
                            grdGTNDetail.Rows[i].Cells["ModelNo"].Value.ToString() == grdGTNDetail.Rows[j].Cells["ModelNo"].Value.ToString())
                        {
                            //duplicate item code
                            MessageBox.Show("Item code duplicated in GTN details... please ensure correctness (" +
                                grdGTNDetail.Rows[i].Cells["Item"].Value.ToString() + ")");
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
        private List<gtndetail> getGTNDetailList(gtnheader gtnh)
        {
            List<gtndetail> GTNDetails = new List<gtndetail>();
            gtndetail gtnd = new gtndetail();
            for (int i = 0; i < grdGTNDetail.Rows.Count; i++)
            {
                try
                {
                    //if (Convert.ToDouble(grdGTNDetail.Rows[i].Cells["Quantity"].Value)
                    //    > Convert.ToDouble(grdGTNDetail.Rows[i].Cells["AvailableQuantity"].Value))
                    //{
                    //    MessageBox.Show("Qunatity should less than Available Quantity");
                    //    return false;
                    //}
                    gtnd = new gtndetail();
                    gtnd.DocumentID = gtnh.DocumentID;
                    gtnd.TemporaryNo = gtnh.TemporaryNo;
                    gtnd.TemporaryDate = gtnh.TemporaryDate;
                    gtnd.StockitemID = grdGTNDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Substring(0, grdGTNDetail.Rows[i].Cells["Item"].Value.ToString().Trim().IndexOf('-'));
                    gtnd.Quantity = Convert.ToDouble(grdGTNDetail.Rows[i].Cells["Quantity"].Value);
                    gtnd.ModelNo = grdGTNDetail.Rows[i].Cells["ModelNo"].Value.ToString();
                    gtnd.MRNNo = Convert.ToInt32(grdGTNDetail.Rows[i].Cells["MRNNo"].Value);
                    gtnd.MRNDate = Convert.ToDateTime(grdGTNDetail.Rows[i].Cells["MRNDate"].Value);
                    gtnd.BatchNo = grdGTNDetail.Rows[i].Cells["BatchNo"].Value.ToString();
                    gtnd.SerialNo = grdGTNDetail.Rows[i].Cells["SerialNo"].Value.ToString();
                    gtnd.ExpiryDate = Convert.ToDateTime(grdGTNDetail.Rows[i].Cells["ExpiryDate"].Value);
                    gtnd.PurchaseQuantity = Convert.ToDouble(grdGTNDetail.Rows[i].Cells["PurchaseQuantity"].Value);
                    gtnd.PurchasePrice = Convert.ToDouble(grdGTNDetail.Rows[i].Cells["PurchasePrice"].Value);
                    gtnd.PurchaseTax = Convert.ToDouble(grdGTNDetail.Rows[i].Cells["PurchaseTax"].Value);
                    gtnd.SupplierID = grdGTNDetail.Rows[i].Cells["SupplierID"].Value.ToString().Trim();
                    gtnd.refNo = Convert.ToInt32(grdGTNDetail.Rows[i].Cells["StockRefNo"].Value);
                    GTNDetails.Add(gtnd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("createAndUpdateGTNDetails() : Error creating GTN Details");
                }
            }
            return GTNDetails;
        }

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredGTNHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredGTNHeader(listOption);
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

            ListFilteredGTNHeader(listOption);
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {

                GTNDB gtndb = new GTNDB();
                gtnheader gtnh = new gtnheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    if (!verifyAndReworkGTNDetailGridRows())
                    {
                        return;
                    }
                    gtnh.DocumentID = docID;
                    gtnh.GTNDate = dtGTNDate.Value;
                    gtnh.Remarks = txtRemarks.Text;
                    gtnh.MaterialValue = Convert.ToDecimal(txtMaterialValue.Text);
                    gtnh.FromLocation = cmbFromLocation.SelectedItem.ToString().Trim().Substring(0, cmbFromLocation.SelectedItem.ToString().Trim().IndexOf('-'));
                    gtnh.ToLocation = cmbTOLocation.SelectedItem.ToString().Trim().Substring(0, cmbTOLocation.SelectedItem.ToString().Trim().IndexOf('-'));
                    gtnh.Comments = docCmtrDB.DGVtoString(dgvComments).Replace("'", "''");
                    gtnh.ForwarderList = prevgtn.ForwarderList;
                    //prevgtn.FromLocation = gtnh.FromLocation;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }

                if (!gtndb.validateGTNHeader(gtnh))
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    //gtnh.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    gtnh.DocumentStatus = 1; //created
                    gtnh.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    gtnh.TemporaryNo = Convert.ToInt32(txtTemporarryNo.Text);
                    gtnh.TemporaryDate = prevgtn.TemporaryDate;
                    gtnh.DocumentStatus = prevgtn.DocumentStatus;
                }
                //Replacing single quotes
                gtnh = verifyHeaderInputString(gtnh);
                verifyDetailInputString();
                if (gtndb.validateGTNHeader(gtnh))
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
                            gtnh.CommentStatus = docCmtrDB.createCommentStatusString(prevgtn.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            gtnh.CommentStatus = prevgtn.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            gtnh.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            gtnh.CommentStatus = prevgtn.CommentStatus;
                        }
                    }
                    //----------
                    int tmpStatus = 1;
                    if (txtComments.Text.Trim().Length > 0)
                    {
                        gtnh.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus).Replace("'", "''");
                    }
                    List<gtndetail> GTNDetails = getGTNDetailList(gtnh);
                    if (btnText.Equals("Update"))
                    {
                        if (gtndb.updateGTNHeaderAndDetail(gtnh, prevgtn, GTNDetails))
                        {
                            MessageBox.Show("GTN Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredGTNHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update GTN Header");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (gtndb.InsertGTNHeaderAndDetail(gtnh, GTNDetails))
                        {
                            MessageBox.Show("GTN Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredGTNHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert  GTN Header");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("GTN Validation failed");
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
            track = true;
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
            pnllv.Visible = false;
            //removeControlsFromPnllvPanel();
            //ListFilteredGTNHeader(1);
            //IsViewMode = true;
        }

        private void btnAddLine_Click_1(object sender, EventArgs e)
        {
            //if (grdGTNDetail.Rows.Count > 0)
            //{
            //    if (!verifyAndReworkGTNDetailGridRows())
            //    {
            //        return;
            //    }
            //}
            AddRowClick = true;
            AddGTNDetailRow();

        }

        private void grdPOPIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdGTNDetail.Columns[e.ColumnIndex].Name;
                ///grdGTNDetail.Rows[e.RowIndex].Selected = true;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdGTNDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkGTNDetailGridRows();
                    }
                    if (columnName.Equals("Select"))
                    {
                        if (cmbFromLocation.SelectedIndex != -1)
                            ShowStoreWiseStockListView();
                        else
                            MessageBox.Show("Select From Lcoation");
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

        //private void btnCalculateax_Click_1(object sender, EventArgs e)
        //{
        //    verifyAndReworkGTNDetailGridRows();
        //}

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
                    IsEditMode = true;
                    AddRowClick = false;
                    clearData();
                    ///grdList.Rows[e.RowIndex].Selected = true;
                    setButtonVisibility(columnName);
                    if (columnName.Equals("View"))
                    {
                        IsViewMode = true;
                        tabControl1.TabPages["tabGTNDetail"].Enabled = true;
                    }
                    prevgtn = new gtnheader();
                    prevgtn.ReceiveStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["ReceiveStatus"].Value.ToString());
                    prevgtn.ToLocation = grdList.Rows[e.RowIndex].Cells["ToLocation"].Value.ToString();
                    if (columnName.Equals("Approve") && prevgtn.ReceiveStatus == 1)
                    {
                        if (StoreEmpMappingDB.CheckAcceptUser(Login.empLoggedIn, prevgtn.ToLocation))
                        {
                            btnForward.Visible = false;
                            btnReverse.Visible = true;
                            btnSendStock.Visible = false;
                            btnAccept.Visible = true;
                            tabControl1.TabPages["tabGTNDetail"].Enabled = true;
                        }
                    }
                    if (columnName.Equals("Edit"))
                    {
                        if (StoreEmpMappingDB.CheckAcceptUser(Login.empLoggedIn, prevgtn.ToLocation) && (prevgtn.ReceiveStatus == 1))
                        {
                            disableTabPages();
                            tabControl1.TabPages["tabGTNDetail"].Enabled = true;
                            btnSave.Visible = false;
                        }
                    }
                    prevgtn.MaterialValue = Convert.ToDecimal(grdList.Rows[e.RowIndex].Cells["MaterialValue"].Value.ToString());
                    prevgtn.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();
                    prevgtn.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    prevgtn.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    prevgtn.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    prevgtn.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    if (prevgtn.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    prevgtn.Comments = GTNDB.getUserComments(prevgtn.DocumentID, prevgtn.TemporaryNo, prevgtn.TemporaryDate);
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    GTNDB gtndb = new GTNDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevgtn.GTNNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["GTNNo"].Value.ToString());
                    prevgtn.GTNDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["GTNDate"].Value.ToString());

                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Document Temp No:" + prevgtn.TemporaryNo + "\n" +
                            "Document Temp Date:" + prevgtn.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "GTN No:" + prevgtn.GTNNo + "\n" +
                            "GTN Date:" + prevgtn.GTNDate.ToString("dd-MM-yyyy");
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevgtn.TemporaryNo + "-" + prevgtn.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_1(null, null);
                        return;
                    }
                    //--------
                    prevgtn.Remarks = grdList.Rows[e.RowIndex].Cells["gRemarks"].Value.ToString();
                    prevgtn.AcceptUser = grdList.Rows[e.RowIndex].Cells["AcceptedUser"].Value.ToString();
                    prevgtn.AcceptStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["AcceptanceStatus"].Value);
                    prevgtn.AcceptDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["AcceptanceDate"].Value.ToString());
                    prevgtn.FromLocation = grdList.Rows[e.RowIndex].Cells["FromLocation"].Value.ToString();
                    //prevgtn.FromLocation = grdList.Rows[e.RowIndex].Cells["FromLocation"].Value.ToString();
                    prevgtn.FromLocationName = grdList.Rows[e.RowIndex].Cells["FromLocationName"].Value.ToString();
                    prevgtn.ToLocationName = grdList.Rows[e.RowIndex].Cells["ToLocationName"].Value.ToString();
                    prevgtn.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    prevgtn.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    prevgtn.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    prevgtn.CreateUser = grdList.Rows[e.RowIndex].Cells["CreateUser"].Value.ToString();
                    if (Login.userLoggedIn != prevgtn.CreateUser)
                    {
                        cmbFromLocation.Enabled = false;
                        cmbTOLocation.Enabled = false;
                    }
                    //prevgtn.Cre = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    prevgtn.ForwardUser = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevgtn.ApproveUser = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    prevgtn.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    //--comments
                    ///chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    prevgtn.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(prevgtn.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(prevgtn.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---
                    //txtReferenceNo.Text = prevpopi.ReferenceNo.ToString();
                    txtTemporarryNo.Text = prevgtn.TemporaryNo.ToString();
                    dtTempDate.Value = prevgtn.TemporaryDate;
                    //dtTempDate.Value = prevpopi.TemporaryDate;
                    txtMaterialValue.Text = prevgtn.MaterialValue.ToString();
                    txtGTNNo.Text = prevgtn.GTNNo.ToString();
                    try
                    {
                        dtGTNDate.Value = prevgtn.GTNDate;
                    }
                    catch (Exception)
                    {
                        dtGTNDate.Value = DateTime.Parse("01-01-1900");
                    }
                    //cmbFromLocation.SelectedIndex = -1;
                    //cmbTOLocation.SelectedIndex = -1;
                    cmbFromLocation.SelectedIndex = cmbFromLocation.FindString(prevgtn.FromLocation);
                    cmbTOLocation.SelectedIndex = cmbTOLocation.FindString(prevgtn.ToLocation);
                    //cmbCustomer.Text = prevpopi.CustomerName;
                    //int ii = cmbFromLocation.FindString(prevgtn.FromLocation);
                    txtRemarks.Text = prevgtn.Remarks;
                    List<gtndetail> GTNDetail = GTNDB.getGTNDetail(prevgtn);
                    grdGTNDetail.Rows.Clear();
                    int i = 0;
                    foreach (gtndetail gtnd in GTNDetail)
                    {
                        if (!AddGTNDetailRow())
                        {
                            MessageBox.Show("Error found in GTN details. Please correct before updating the details");
                        }
                        else
                        {
                            grdGTNDetail.Columns["Unit"].Visible = false;
                            //grdGTNDetail.Columns["AvailableQuantity"].Visible = false;
                            try
                            {
                                grdGTNDetail.Rows[i].Cells["Item"].Value = gtnd.StockitemID + "-" + gtnd.StockItemName;
                            }
                            catch (Exception)
                            {
                                grdGTNDetail.Rows[i].Cells["Item"].Value = null;
                            }
                            grdGTNDetail.Rows[i].Cells["Quantity"].Value = gtnd.Quantity;
                            grdGTNDetail.Rows[i].Cells["ModelNo"].Value = gtnd.ModelNo;
                            grdGTNDetail.Rows[i].Cells["ModelName"].Value = gtnd.ModelName;
                            grdGTNDetail.Rows[i].Cells["MRNNo"].Value = gtnd.MRNNo;
                            grdGTNDetail.Rows[i].Cells["MRNDate"].Value = gtnd.MRNDate;
                            grdGTNDetail.Rows[i].Cells["Quantity"].Value = gtnd.Quantity;
                            grdGTNDetail.Rows[i].Cells["PurchaseQuantity"].Value = gtnd.PurchaseQuantity;
                            grdGTNDetail.Rows[i].Cells["PurchasePrice"].Value = gtnd.PurchasePrice;
                            grdGTNDetail.Rows[i].Cells["PurchaseTax"].Value = gtnd.PurchaseTax;
                            grdGTNDetail.Rows[i].Cells["BatchNo"].Value = gtnd.BatchNo;
                            grdGTNDetail.Rows[i].Cells["SerialNo"].Value = gtnd.SerialNo;
                            grdGTNDetail.Rows[i].Cells["ExpiryDate"].Value = gtnd.ExpiryDate;
                            grdGTNDetail.Rows[i].Cells["SupplierID"].Value = gtnd.SupplierID;
                            grdGTNDetail.Rows[i].Cells["SupplierName"].Value = gtnd.SupplierName;
                            grdGTNDetail.Rows[i].Cells["StockRefNo"].Value = gtnd.refNo;
                            grdGTNDetail.Rows[i].Cells["AvailableQuantity"].Value = StockDB.getAvailiableStockQuantity(gtnd.refNo);
                            //if (prevgtn.ReceiveStatus == 1 && columnName.Equals("Edit"))
                            //{
                            //    tabControl1.TabPages["tabMRNHeader"].Enabled = false;
                            //}

                            i++;
                        }

                    }
                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                    tabControl1.SelectedTab = tabGTNHeader;
                    tabControl1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private void btnClearEntries_Click_1(object sender, EventArgs e)
        {
            try
            {

                DialogResult dialog = MessageBox.Show("Are you sure to clear all entries in grid detail ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdGTNDetail.Rows.Clear();
                    MessageBox.Show("Grid items cleared.");
                    verifyAndReworkGTNDetailGridRows();
                }
            }
            catch (Exception)
            {
            }
        }
        private Boolean checkAvailabilityOfitem()
        {
            Boolean status = true;
            if (grdGTNDetail.CurrentRow.Cells["Item"].Value.ToString().Length != 0 ||
                grdGTNDetail.CurrentRow.Cells["ModelNo"].Value.ToString().Length != 0 ||
                grdGTNDetail.CurrentRow.Cells["ModelName"].Value.ToString().Length != 0)
            {
                status = false;
            }
            return status;
        }
        private DataGridView getLocationWiseStockListGridView(List<stock> StockList)
        {
            grdForStockList = new DataGridView();
            try
            {
                grdForStockList.AllowUserToAddRows = false;
                grdForStockList.AllowUserToDeleteRows = false;
                grdForStockList.AllowUserToOrderColumns = true;
                grdForStockList.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdForStockList.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdForStockList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystrokeOrF2;
                grdForStockList.GridColor = System.Drawing.SystemColors.ActiveCaption;
                //grdForStockList.Location = new System.Drawing.Point(8, 16);
                grdForStockList.RowHeadersVisible = false;
                grdForStockList.EnableHeadersVisualStyles = false;
                grdForStockList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grdForStockList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                grdForStockList.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
                grdForStockList.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdForStockList.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular);
                //grdForStockList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
                string[] collArr = new string[] { "StockRefNo", "StockItemID" , "StockItemName" , "ModelNo" ,
                "ModelName" , "MRNNo" , "MRNDate","PresentStock","BAtchNo","SerialNO","ExpiryDate","PurchaseQuantity","PurchasePrice",
                "PurchaseTax","SupplierId","SupplierName"
                };
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                DataGridViewCellStyle style = new DataGridViewCellStyle();
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.HeaderCell.Style.BackColor = Color.LightSeaGreen;
                colChk.HeaderCell.Style.ForeColor = Color.Black;
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;
                
                colChk.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                grdForStockList.Columns.Add(colChk);
                foreach (string str in collArr)
                {
                    int index = Array.IndexOf(collArr, str);
                    style = new DataGridViewCellStyle();
                    style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    if (index == 6 || index == 10)
                    {
                        style.Format = "dd-MM-yyyy";
                    }
                    if (index == 3)
                        col.Width = 200;
                    else
                        col.Width = 100;
                    col = new DataGridViewTextBoxColumn();
                    col.DefaultCellStyle = style;
                    col.HeaderCell.Style.BackColor = Color.LightSeaGreen;
                    col.Name = str;
                    col.HeaderText = str;
                    grdForStockList.Columns.Add(col);
                }
                grdForStockList.Columns["StockRefNo"].Visible = false;
                foreach (stock stk in StockList)
                {
                    var index = grdForStockList.Rows.Add();
                    grdForStockList.Rows[grdForStockList.RowCount - 1].Cells["StockRefNo"].Value = stk.RowID;
                    grdForStockList.Rows[grdForStockList.RowCount - 1].Cells["StockItemID"].Value = stk.StockItemID;
                    grdForStockList.Rows[grdForStockList.RowCount - 1].Cells["StockItemName"].Value = stk.StockItemName;
                    grdForStockList.Rows[grdForStockList.RowCount - 1].Cells["ModelNo"].Value = stk.ModelNo;
                    grdForStockList.Rows[grdForStockList.RowCount - 1].Cells["ModelName"].Value = stk.ModelName;
                    grdForStockList.Rows[grdForStockList.RowCount - 1].Cells["MRNNo"].Value = stk.MRNNo;
                    grdForStockList.Rows[grdForStockList.RowCount - 1].Cells["MRNDate"].Value = stk.MRNDate;
                    grdForStockList.Rows[grdForStockList.RowCount - 1].Cells["PresentStock"].Value = stk.PresentStock;
                    grdForStockList.Rows[grdForStockList.RowCount - 1].Cells["BAtchNo"].Value = stk.BatchNo;
                    grdForStockList.Rows[grdForStockList.RowCount - 1].Cells["SerialNO"].Value = stk.SerielNo;
                    grdForStockList.Rows[grdForStockList.RowCount - 1].Cells["ExpiryDate"].Value = stk.ExpiryDate;
                    grdForStockList.Rows[grdForStockList.RowCount - 1].Cells["PurchaseQuantity"].Value = stk.PurchaseQuantity;
                    grdForStockList.Rows[grdForStockList.RowCount - 1].Cells["PurchasePrice"].Value = stk.PurchasePrice;
                    grdForStockList.Rows[grdForStockList.RowCount - 1].Cells["PurchaseTax"].Value = stk.PurchaseTax;
                    grdForStockList.Rows[grdForStockList.RowCount - 1].Cells["SupplierId"].Value = stk.SupplierID;
                    grdForStockList.Rows[grdForStockList.RowCount - 1].Cells["SupplierName"].Value = stk.SupplierName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error In adding Stock Grid Rows");
            }
            return grdForStockList;
        }
        private void ShowStoreWiseStockListView()
        {
            if (!checkAvailabilityOfitem())
            {
                DialogResult dialog = MessageBox.Show("Selected product and Model detail will removed?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    for (int i = 0; i < grdGTNDetail.ColumnCount; i++)
                    {
                        if (grdGTNDetail.CurrentRow.Cells[i].GetType() != typeof(Button))
                        {
                            grdGTNDetail.CurrentRow.Cells[i].Value = "";
                        }
                    }
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

            frmPopup.Size = new Size(1300, 370);

            Label lblSearch = new Label();
            lblSearch.Location = new System.Drawing.Point(950, 5);
            lblSearch.Text = "Search Here";
            lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
            lblSearch.ForeColor = Color.Black;
            frmPopup.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Size = new Size(220, 18);
            txtSearch.Location = new System.Drawing.Point(1050, 3);
            txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
            txtSearch.ForeColor = Color.Black;
            txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            txtSearch.TabIndex = 0;
            txtSearch.Focus();
            frmPopup.Controls.Add(txtSearch);

            string location = cmbFromLocation.SelectedItem.ToString().Trim().Substring(0, cmbFromLocation.SelectedItem.ToString().Trim().IndexOf('-'));
            List<stock> StockList = StockDB.getStoreWiseStockDetail(location);
            grdForStockList = getLocationWiseStockListGridView(StockList);
            grdForStockList.Bounds = new Rectangle(new Point(0, 27), new Size(1300, 300));
            frmPopup.Controls.Add(grdForStockList);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new System.Drawing.Point(20, 335);
            lvOK.Click += new System.EventHandler(this.lvOK_Click1);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new System.Drawing.Point(110, 335);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click1);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
        }
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                var checkedRows = from DataGridViewRow r in grdForStockList.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if(selectedRowCount != 1)
                {
                    MessageBox.Show("select Only One Item");
                    return;
                }
                DataGridViewRow selectedRow = new DataGridViewRow();
                foreach(var row in checkedRows)
                {
                    selectedRow = row;
                }
                for (int i = 0; i < grdGTNDetail.Rows.Count - 1; i++)
                {
                    if ((grdGTNDetail.Rows[i].Cells["StockRefNo"].Value.ToString() == selectedRow.Cells["StockRefNo"].Value.ToString()))
                    {
                        MessageBox.Show("not allowed to select same row");
                        return;
                    }
                }
                AddGridDetailRowForGTN(selectedRow);
                frmPopup.Close();
                frmPopup.Dispose();
                //pnllv.Visible = false;
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
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //filterGridData();
            try
            {
                //DataTable dt = (DataTable)grdForStockList.mas;
                //int n = 0;
                //(grdForStockList.DataSource as DataTable).DefaultView.RowFilter = string.Format("Name LIKE '%{0}%'", txtSearch.Text);
                //grdForStockList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                filterGridData();
                ///grdForStockList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            }
            catch (Exception ex)
            {

            }
        }
        private void filterGridData()
        {
            try
            {
                foreach (DataGridViewRow row in grdForStockList.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {

                    foreach (DataGridViewRow row in grdForStockList.Rows)
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
        private Boolean AddGridDetailRowForGTN(DataGridViewRow grdRow)
        {
            Boolean status = true;
            try
            {
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["LineNo"].Value = grdGTNDetail.RowCount;
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["StockRefNo"].Value = grdRow.Cells["StockRefNo"].Value;
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["Item"].Value = grdRow.Cells["StockItemID"].Value + "-" + grdRow.Cells["StockItemName"].Value;
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["ModelNo"].Value = grdRow.Cells["ModelNo"].Value;
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["ModelName"].Value = grdRow.Cells["ModelName"].Value;
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["AvailableQuantity"].Value = grdRow.Cells["PresentStock"].Value;
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["Quantity"].Value = "";
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["MRNNo"].Value = grdRow.Cells["MRNNo"].Value;
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["MRNDate"].Value = grdRow.Cells["MRNDate"].Value;
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["BatchNo"].Value = grdRow.Cells["BAtchNo"].Value;
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["SerialNo"].Value = grdRow.Cells["SerialNO"].Value;
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["ExpiryDate"].Value = grdRow.Cells["ExpiryDate"].Value;
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["PurchaseQuantity"].Value = grdRow.Cells["PurchaseQuantity"].Value;
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["PurchasePrice"].Value = grdRow.Cells["PurchasePrice"].Value;
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["PurchaseTax"].Value = grdRow.Cells["PurchaseTax"].Value;
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["SupplierID"].Value = grdRow.Cells["SupplierId"].Value;
                grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["SupplierName"].Value = grdRow.Cells["SupplierName"].Value;
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        //private Boolean AddGridDetailRowForGTN(ListViewItem itemRow)
        //{
        //    Boolean status = true;
        //    try
        //    {
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["LineNo"].Value = grdGTNDetail.RowCount;
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["StockRefNo"].Value = itemRow.SubItems[1].Text;
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["Item"].Value = itemRow.SubItems[2].Text + "-" + itemRow.SubItems[3].Text;
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["ModelNo"].Value = itemRow.SubItems[4].Text;
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["ModelName"].Value = itemRow.SubItems[5].Text;
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["AvailableQuantity"].Value = itemRow.SubItems[8].Text;
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["Quantity"].Value = "";
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["MRNNo"].Value = itemRow.SubItems[6].Text;
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["MRNDate"].Value = itemRow.SubItems[7].Text;
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["BatchNo"].Value = itemRow.SubItems[9].Text;
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["SerialNo"].Value = itemRow.SubItems[10].Text;
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["ExpiryDate"].Value = itemRow.SubItems[11].Text;
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["PurchaseQuantity"].Value = itemRow.SubItems[12].Text;
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["PurchasePrice"].Value = itemRow.SubItems[13].Text;
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["PurchaseTax"].Value = itemRow.SubItems[14].Text;
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["SupplierID"].Value = itemRow.SubItems[15].Text;
        //        grdGTNDetail.Rows[grdGTNDetail.RowCount - 1].Cells["SupplierName"].Value = itemRow.SubItems[16].Text;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return status;
        //}
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
            lvForwrdOK.Location = new System.Drawing.Point(40, 265);
            lvForwrdOK.Click += new System.EventHandler(this.lvForwardOK_Click);
            frmPopup.Controls.Add(lvForwrdOK);

            Button lvForwardCancel = new Button();
            lvForwardCancel.Text = "CANCEL";
            lvForwardCancel.BackColor = Color.Tan;
            lvForwardCancel.Location = new System.Drawing.Point(130, 265);
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
        private void pnlList_Paint(object sender, PaintEventArgs e)
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
                ////frmPopup.Dispose();
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
                            GTNDB gtndb = new GTNDB();
                            prevgtn.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevgtn.CommentStatus);
                            prevgtn.ForwardUser = approverUID;
                            prevgtn.ForwarderList = prevgtn.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (gtndb.forwardGTN(prevgtn))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(prevgtn, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredGTNHeader(listOption);
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
                    string reverseStr = getReverseString(prevgtn.ForwarderList);
                    //do forward activities
                    prevgtn.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevgtn.CommentStatus);
                    GTNDB gtndb = new GTNDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevgtn.ForwarderList = reverseStr.Substring(0, ind);
                        prevgtn.ForwardUser = reverseStr.Substring(ind + 3);
                        prevgtn.DocumentStatus = prevgtn.DocumentStatus - 1;
                        prevgtn.ReceiveStatus = 0;
                    }
                    else
                    {
                        prevgtn.ForwarderList = "";
                        prevgtn.ForwardUser = "";
                        prevgtn.DocumentStatus = 1;
                        prevgtn.ReceiveStatus = 0;
                    }
                    if (gtndb.reverseGTN(prevgtn))
                    {
                        MessageBox.Show("GTN Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredGTNHeader(listOption);
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
        private void btnListDocuments_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevgtn.TemporaryNo + "-" + prevgtn.TemporaryDate.ToString("yyyyMMddhhmmss"));
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
                    string subDir = prevgtn.TemporaryNo + "-" + prevgtn.TemporaryDate.ToString("yyyyMMddhhmmss");
                    ////DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    dgv.Enabled = false;
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

                cmbFromLocation.Enabled = true;
                cmbTOLocation.Enabled = true;
                btnSendStock.Visible = false;
                btnActionPending.Visible = true;
                btnApprovalPending.Visible = true;
                //btnApproved.Visible = true;
                btnNew.Visible = false;
                btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                btnForward.Visible = false;
                // btnApprove.Visible = false;
                btnReverse.Visible = false;
                btnAccept.Visible = false;
                btnGetComments.Visible = false;
                ////chkCommentStatus.Visible = false;
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
                    tabControl1.SelectedTab = tabGTNHeader;
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
                    tabControl1.SelectedTab = tabGTNDetail;

                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    //btnApprove.Visible = true;
                    btnReverse.Visible = true;
                    btnSendStock.Visible = true;
                    disableTabPages();
                    tabControl1.SelectedTab = tabGTNDetail;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabGTNDetail;
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
                ////chkCommentStatus.Checked = false;
                txtComments.Text = "";
                grdGTNDetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }
        private void cmbFromLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbFromLocation.SelectedItem != null && ch == 0)
                {
                    ch = 1;
                    if (!IsEditMode)
                    {
                        string Floc = cmbFromLocation.SelectedItem.ToString().Trim().Substring(0, cmbFromLocation.SelectedItem.ToString().Trim().IndexOf('-'));
                        if (!StoreEmpMappingDB.varifyLocationEmpMapping(Floc, Login.empLoggedIn))
                        {
                            MessageBox.Show("Selected Location not related to Current User");
                            cmbFromLocation.SelectedItem = null;
                            Floc = "";

                        }
                        else
                        {
                            GridGTNDetailRemove(Floc);
                        }
                        CatalogueValueDB.fillToLocationCombo(cmbTOLocation, Floc);
                        ch = 0;
                    }
                    else
                    {
                        if ((Login.userLoggedIn == prevgtn.CreateUser))
                        {
                            string Floc = cmbFromLocation.SelectedItem.ToString().Trim().Substring(0, cmbFromLocation.SelectedItem.ToString().Trim().IndexOf('-'));
                            if (Floc != prevgtn.FromLocation)
                            {
                                if (!StoreEmpMappingDB.varifyLocationEmpMapping(Floc, Login.empLoggedIn))
                                {
                                    MessageBox.Show("Selected Location not related to Current User");
                                    cmbFromLocation.SelectedIndex = cmbFromLocation.FindString(prevgtn.FromLocation);
                                }
                                else
                                {
                                    GridGTNDetailRemove(Floc);
                                }
                            }
                        }
                        StoreEmpMappingDB.fillLocationCombo(cmbTOLocation);
                        ch = 0;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void GridGTNDetailRemove(string Floc)
        {
            if (grdGTNDetail.Rows.Count > 0)
            {
                DialogResult dialog = MessageBox.Show("Warning: \nGTN Details value will be removed", "", MessageBoxButtons.OKCancel);
                if (dialog == DialogResult.OK)
                {
                    grdGTNDetail.Rows.Clear();
                    CatalogueValueDB.fillToLocationCombo(cmbTOLocation, Floc);
                }
                else
                {
                    cmbFromLocation.SelectedIndex = cmbFromLocation.FindString(prevgtn.FromLocation);
                }
            }

        }
        private void btnSendStock_Click(object sender, EventArgs e)
        {
            try
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
                string toLoc = cmbTOLocation.SelectedItem.ToString().Trim().Substring(0, cmbTOLocation.SelectedItem.ToString().Trim().IndexOf('-'));
                lvApprover = StoreEmpMappingDB.ListStoreMappingEmp(toLoc);
                lvApprover.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
                //pnlForwarder.Controls.Remove(lvApprover);
                frmPopup.Controls.Add(lvApprover);

                Button lvForwrdOK = new Button();
                lvForwrdOK.BackColor = Color.Tan;
                lvForwrdOK.Text = "OK";
                lvForwrdOK.Location = new System.Drawing.Point(44, 265);
                lvForwrdOK.Click += new System.EventHandler(this.lvSendOK_Click);
                frmPopup.Controls.Add(lvForwrdOK);

                Button lvForwardCancel = new Button();
                lvForwardCancel.Text = "CANCEL";
                lvForwardCancel.BackColor = Color.Tan;
                lvForwardCancel.Location = new System.Drawing.Point(141, 265);
                lvForwardCancel.Click += new System.EventHandler(this.lvSendCancel_Click);
                frmPopup.Controls.Add(lvForwardCancel);
                ////lvForwardCancel.Visible = false;

                //pnlForwarder.Visible = true;
                //pnlAddEdit.Controls.Add(pnlForwarder);
                //pnlAddEdit.BringToFront();
                //pnlForwarder.BringToFront();
                //pnlForwarder.Focus();
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void lvSendOK_Click(object sender, EventArgs e)
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
                            approverUName = itemRow.SubItems[3].Text;
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
                        DialogResult dialog = MessageBox.Show("Are you sure to send the document to selected receiver?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            //do forward activities
                            GTNDB gtndb = new GTNDB();
                            prevgtn.ReceiveStatus = 1;
                            prevgtn.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevgtn.CommentStatus);
                            prevgtn.ForwardUser = approverUID;
                            prevgtn.ForwarderList = prevgtn.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (gtndb.forwardGTN(prevgtn))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(prevgtn, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredGTNHeader(listOption);
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

        private void lvSendCancel_Click(object sender, EventArgs e)
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

        private void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                GTNDB gtndb = new GTNDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Accept the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevgtn.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevgtn.CommentStatus);
                    if (prevgtn.Status != 96)
                    {
                        prevgtn.GTNNo = DocumentNumberDB.getNewNumber(docID, 2);
                    }
                        
                    if (!verifyAndReworkGTNDetailGridRows())
                    {
                        return;
                    }
                    if (gtndb.AcceptGTN(prevgtn))
                    {
                        MessageBox.Show("GTN Document Accepted");
                        List<gtndetail> GTNDetails = getGTNDetailList(prevgtn);
                        if (!updateDashBoard(prevgtn, 2))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                        }
                        prevgtn.GTNDate = UpdateTable.getSQLDateTime();
                        if (gtndb.updateGTNInStockDetail(prevgtn, GTNDetails))
                        {
                            MessageBox.Show("GTN Details updated in stock");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredGTNHeader(listOption);
                            setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                        }
                        else
                            MessageBox.Show("Fails to update GTN Details in stock");
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private Boolean updateDashBoard(gtnheader gtnh, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = gtnh.DocumentID;
                dsb.TemporaryNo = gtnh.TemporaryNo;
                dsb.TemporaryDate = gtnh.TemporaryDate;
                dsb.DocumentNo = gtnh.GTNNo;
                dsb.DocumentDate = gtnh.GTNDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = gtnh.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(prevgtn.DocumentID);
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
        private void cmbTOLocation_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string s = cmbTOLocation.SelectedItem.ToString();
            int n = cmbTOLocation.SelectedIndex;
            if (cmbFromLocation.SelectedIndex == -1)
            {
                MessageBox.Show("Select From Location");
                cmbTOLocation.SelectedItem = null;
                return;
            }
            if (cmbFromLocation.SelectedIndex != -1 && cmbFromLocation.SelectedIndex != -1)
            {
                string Floc = cmbFromLocation.SelectedItem.ToString().Trim().Substring(0, cmbFromLocation.SelectedItem.ToString().Trim().IndexOf('-'));
                string Tloc = cmbTOLocation.SelectedItem.ToString().Trim().Substring(0, cmbTOLocation.SelectedItem.ToString().Trim().IndexOf('-'));
                if (Floc == Tloc)
                {
                    MessageBox.Show("From Location and To Location should be different");
                    cmbTOLocation.SelectedIndex = cmbTOLocation.FindString(prevgtn.ToLocation);
                }
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
           
            
        }
        //-- Validating Header and Detail String For Single Quotes

        private gtnheader verifyHeaderInputString(gtnheader gtnh)
        {
            try
            {
                gtnh.Remarks = Utilities.replaceSingleQuoteWithDoubleSingleQuote(gtnh.Remarks);
            }
            catch (Exception ex)
            {
            }
            return gtnh;
        }
        private void verifyDetailInputString()
        {
            try
            {
                for (int i = 0; i < grdGTNDetail.Rows.Count; i++)
                {
                    grdGTNDetail.Rows[i].Cells["Item"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdGTNDetail.Rows[i].Cells["Item"].Value.ToString());
                    grdGTNDetail.Rows[i].Cells["ModelNo"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdGTNDetail.Rows[i].Cells["ModelNo"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void grdGTNDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {      
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    grdGTNDetail.Rows[e.RowIndex].Selected = true;
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

        private void GTNHeader_Enter(object sender, EventArgs e)
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

