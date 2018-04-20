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
    public partial class StockHoldingHeader : System.Windows.Forms.Form
    {
        Boolean check = false;
        string docID = "STOCKHOLDING";
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Boolean userIsACommenter = false;
        string chkTemp = "";
        ////double productvalue = 0.0;
        ////double taxvalue = 0.0;
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        stockholdingheader prevshh;
        Form frmPopup = new Form();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();

        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        //string custid = "";
        //DateTimePicker dtp;
        public StockHoldingHeader()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }
        private void StockHoldingHeader_Load(object sender, EventArgs e)
        {
            ////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;
            ////this.FormBorderStyle = FormBorderStyle.Fixed3D;
            String a = this.Size.ToString();
            grd.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grd.EnableHeadersVisualStyles = false;
            ListFilteredStockHoldingHeader(listOption);
        }
        private void ListFilteredStockHoldingHeader(int option)
        {
            try
            {
                grd.Rows.Clear();
                StockHoldingHeaderDB shhdb = new StockHoldingHeaderDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);

                List<stockholdingheader> StockHoldingHeaderList = shhdb.getFilteredStockHoldingHeader(userString, option, userCommentStatusString);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (stockholdingheader shh in StockHoldingHeaderList)
                {
                    if (option == 1)
                    {
                        if (shh.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grd.Rows.Add();
                    grd.Rows[grd.RowCount - 1].Cells["gDocumentID"].Value = shh.DocumentID;
                    // grd.Rows[grd.RowCount - 1].Cells["gDocumentName"].Value = shh.DocumentName;

                    grd.Rows[grd.RowCount - 1].Cells["gTemporaryNo"].Value = shh.TemporaryNo;
                    grd.Rows[grd.RowCount - 1].Cells["gTemporaryDate"].Value = shh.TemporaryDate;
                    grd.Rows[grd.RowCount - 1].Cells["DocumentNo"].Value = shh.DocumentNo;
                    grd.Rows[grd.RowCount - 1].Cells["DocumentDate"].Value = shh.DocumentDate;
                    grd.Rows[grd.RowCount - 1].Cells["gStoreLocationID"].Value = shh.StoreLocationID;
                    grd.Rows[grd.RowCount - 1].Cells["gRemarks"].Value = shh.Remarks;
                    grd.Rows[grd.RowCount - 1].Cells["gComments"].Value = shh.Comments;
                    grd.Rows[grd.RowCount - 1].Cells["CmtStatus"].Value = shh.CommentStatus;
                    grd.Rows[grd.RowCount - 1].Cells["gCreateUser"].Value = shh.CreateUser;
                    grd.Rows[grd.RowCount - 1].Cells["gCreateTime"].Value = shh.CreateTime;
                    grd.Rows[grd.RowCount - 1].Cells["ForwardUser"].Value = shh.ForwardUser;
                    grd.Rows[grd.RowCount - 1].Cells["ApproveUser"].Value = shh.ApproveUser;
                    grd.Rows[grd.RowCount - 1].Cells["Forwarders"].Value = shh.ForwarderList;
                    grd.Rows[grd.RowCount - 1].Cells["gStatus"].Value = shh.Status;
                    grd.Rows[grd.RowCount - 1].Cells["gDocumentStatus"].Value = shh.DocumentStatus;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Stock Holding Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;

            grd.Columns["gCreateUser"].Visible = true;
            grd.Columns["ForwardUser"].Visible = true;
            grd.Columns["ApproveUser"].Visible = true;

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
            CatalogueValueDB.fillCatalogValueComboNew(cmbStoreLocationID, "StoreLocation");
            dtTempDate.Format = DateTimePickerFormat.Custom;
            dtTempDate.CustomFormat = "dd-MM-yyyy";
            dtTempDate.Enabled = false;
            dtDocumentDate.Format = DateTimePickerFormat.Custom;
            dtDocumentDate.CustomFormat = "dd-MM-yyyy";
            dtDocumentDate.Enabled = false;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            grdStockHoldingDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            txtComments.Text = "";

            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
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

                grdStockHoldingDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                //----------clear temperory panels
                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                pnllv.Visible = false;
                grdStockHoldingDetail.Columns["InwardDocumentID"].Visible = true;
                grdStockHoldingDetail.Columns["InwardDocumentNo"].Visible = true;
                grdStockHoldingDetail.Columns["InwardDocumentDate"].Visible = true;
                cmbStoreLocationID.SelectedIndex = -1;
                txtTemporarryNo.Text = "";
                dtTempDate.Value = DateTime.Parse("01-01-1900");
                txtDocumentNo.Text = "";
                dtDocumentDate.Value = DateTime.Parse("01-01-1900");
                txtRemarks.Text = "";
                prevshh = new stockholdingheader();
                
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
                AddStockHoldingDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddStockHoldingDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdStockHoldingDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkStockHoldingHeaderGridRows())
                    {
                        return false;
                    }
                }
                grdStockHoldingDetail.Rows.Add();
                int kount = grdStockHoldingDetail.RowCount;
                grdStockHoldingDetail.Rows[kount - 1].Cells["LineNo"].Value = kount;

                grdStockHoldingDetail.Rows[kount - 1].Cells["StockItemID"].Value = "";
                grdStockHoldingDetail.Rows[kount - 1].Cells["StockItemName"].Value = "";
                grdStockHoldingDetail.Rows[kount - 1].Cells["ModelNo"].Value = "";
                grdStockHoldingDetail.Rows[kount - 1].Cells["ModelName"].Value = "";
                grdStockHoldingDetail.Rows[grdStockHoldingDetail.RowCount - 1].Cells["Quantity"].Value = 0;
                grdStockHoldingDetail.Rows[grdStockHoldingDetail.RowCount - 1].Cells["InwardDocumentID"].Value = "";
                grdStockHoldingDetail.Rows[grdStockHoldingDetail.RowCount - 1].Cells["InwardDocumentNo"].Value = "";
                grdStockHoldingDetail.Rows[grdStockHoldingDetail.RowCount - 1].Cells["InwardDocumentDate"].Value = "";
                grdStockHoldingDetail.Rows[grdStockHoldingDetail.RowCount - 1].Cells["StockReferenceNo"].Value = "";

                var BtnCell = (DataGridViewButtonCell)grdStockHoldingDetail.Rows[kount - 1].Cells["Delete"];
                BtnCell.Value = "Del";

            }

            catch (Exception ex)
            {
                MessageBox.Show("AddStockDetailRow() : Error");
            }

            return status;
        }
        private Boolean verifyAndReworkStockHoldingHeaderGridRows()
        {
            Boolean status = true;

            try
            {
                if (grdStockHoldingDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Stock details");
                    return false;
                }
                for (int i = 0; i < grdStockHoldingDetail.Rows.Count; i++)
                {
                    grdStockHoldingDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if ((grdStockHoldingDetail.Rows[i].Cells["StockItemID"].Value.ToString().Trim().Length == 0) ||
                        (grdStockHoldingDetail.Rows[i].Cells["ModelNo"].Value.ToString().Trim().Length == 0) ||
                            (grdStockHoldingDetail.Rows[i].Cells["ModelName"].Value.ToString().Trim().Length == 0) ||
                            (grdStockHoldingDetail.CurrentRow.Cells["Quantity"].Value.ToString().Length == 0) ||
                             (Convert.ToDouble(grdStockHoldingDetail.CurrentRow.Cells["Quantity"].Value.ToString()) == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                    int str = Convert.ToInt32(grdStockHoldingDetail.Rows[i].Cells["StockReferenceNo"].Value);
                    double qunt = Convert.ToInt32(grdStockHoldingDetail.Rows[i].Cells["Quantity"].Value);
                    if (!StockDB.verifyPresentStockAvailability(str, qunt))
                    {
                        MessageBox.Show("That much Stock Not Available");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Check rows");
                return false;
            }
            return status;
        }


        private List<stockholdingdetail> getStockHoldingDetail(stockholdingheader shh)
        {
            List<stockholdingdetail> StockHoldingHeaderDetailList = new List<stockholdingdetail>();
            try
            {
                stockholdingdetail shd = new stockholdingdetail();
                for (int i = 0; i < grdStockHoldingDetail.Rows.Count; i++)
                {

                    shd = new stockholdingdetail();
                    shd.DocumentID = shh.DocumentID;
                    shd.TemporaryNo = shh.TemporaryNo;
                    shd.TemporaryDate = shh.TemporaryDate;
                    shd.StockItemID = grdStockHoldingDetail.Rows[i].Cells["StockItemID"].Value.ToString();
                    shd.ModelNo = grdStockHoldingDetail.Rows[i].Cells["ModelNo"].Value.ToString();
                    shd.Quantity = Convert.ToDouble(grdStockHoldingDetail.Rows[i].Cells["Quantity"].Value);
                    shd.InwardDocumentID = grdStockHoldingDetail.Rows[i].Cells["InwardDocumentID"].Value.ToString();
                    shd.InwardDocumentNo = grdStockHoldingDetail.Rows[i].Cells["InwardDocumentNo"].Value.ToString();
                    shd.InwardDocumentDate = Convert.ToDateTime(grdStockHoldingDetail.Rows[i].Cells["InwardDocumentDate"].Value);
                    shd.StockReferenceNo = Convert.ToInt32(grdStockHoldingDetail.Rows[i].Cells["StockReferenceNo"].Value.ToString());
                    StockHoldingHeaderDetailList.Add(shd);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("createAndUpdateInvoiceDetails() : Error updating Invoice Details");
                StockHoldingHeaderDetailList = null;
            }
            return StockHoldingHeaderDetailList;
        }

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredStockHoldingHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredStockHoldingHeader(listOption);
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

            ListFilteredStockHoldingHeader(listOption);
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {

                StockHoldingHeaderDB shhdb = new StockHoldingHeaderDB();
                stockholdingheader shh = new stockholdingheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    //shh.StoreLocationID = ((Structures.ComboBoxItem)cmbStoreLocationID.SelectedItem).HiddenValue;
                    if (!verifyAndReworkStockHoldingHeaderGridRows())
                    {
                        return;
                    }
                    shh.DocumentID = docID;
                    shh.DocumentDate = dtDocumentDate.Value;
                    shh.StoreLocationID = ((Structures.ComboBoxItem)cmbStoreLocationID.SelectedItem).HiddenValue;
                    shh.Remarks = txtRemarks.Text;

                    shh.Comments = docCmtrDB.DGVtoString(dgvComments);
                    shh.ForwarderList = prevshh.ForwarderList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }

                if (!shhdb.validateStockHoldingHeader(shh))
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    //shh.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    shh.DocumentStatus = 1; //created
                    shh.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    shh.TemporaryNo = Convert.ToInt32(txtTemporarryNo.Text);
                    shh.TemporaryDate = prevshh.TemporaryDate;
                    shh.DocumentStatus = prevshh.DocumentStatus;
                    // inh.QCStatus = prevmrn.QCStatus;
                }

                if (shhdb.validateStockHoldingHeader(shh))
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
                            shh.CommentStatus = docCmtrDB.createCommentStatusString(prevshh.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            shh.CommentStatus = prevshh.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            shh.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            shh.CommentStatus = prevshh.CommentStatus;
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
                        shh.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus);
                    }
                    List<stockholdingdetail> SHDetailList = getStockHoldingDetail(shh);
                    if (btnText.Equals("Update"))
                    {
                        if (shhdb.updateSHHeaderAndDetail(shh, prevshh, SHDetailList))
                        {
                            MessageBox.Show("Stock Header Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredStockHoldingHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update Stock Product Inward Header");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        //return;
                        if (shhdb.InsertSHHeaderAndDetail(shh, SHDetailList))
                        {
                            MessageBox.Show("Stock Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredStockHoldingHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert Stock  Header");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Stock Details Validation failed");
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
            //track = true;
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }

        private void btnAddLine_Click_1(object sender, EventArgs e)
        {
            AddStockHoldingDetailRow();
        }
        private void grdPOPIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (e.RowIndex < 0)
                    return;

                string columnName = grdStockHoldingDetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdStockHoldingDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkStockHoldingHeaderGridRows();
                    }
                    if (columnName.Equals("ViewTaxDetails"))
                    {
                        //show tax details
                        DialogResult dialog = MessageBox.Show(grdStockHoldingDetail.Rows[e.RowIndex].Cells["TaxDetails"].Value.ToString(),
                            "Line No : " + (e.RowIndex + 1), MessageBoxButtons.OK);
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
        private void btnSelectItems_Click(object sender, EventArgs e)
        {


            //btnSelectItems.Enabled = false;
            string loc = ((Structures.ComboBoxItem)cmbStoreLocationID.SelectedItem).HiddenValue;
            showstoreWiseListView(loc);
            //btnSelectItems.Enabled = true;
        }
        private void showstoreWiseListView(string loc)
        {

            //removeControlsFromLVPanel();
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
            lv = StockDB.getStoreWiseItemDetailForStockHolding(loc);
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
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

            //pnlAddEdit.Controls.Add(pnllv);
            //pnllv.BringToFront();
            //pnllv.Visible = true;

        }
        private void lvOK_Click2(object sender, EventArgs e)
        {
            try
            {
               // btnSelectItems.Enabled = true;
                //nllv.Visible = false;

                int kount = 0;
                foreach (ListViewItem itemRow in lv.Items)
                {

                    if (itemRow.Checked)
                    {
                        foreach (DataGridViewRow row in grdStockHoldingDetail.Rows)
                        {
                            //int str = grdStockHoldingDetail.Rows[i].Cells["StockReferenceNo"].Value);
                            //string s = itemRow.SubItems[1].Text;

                            if (grdStockHoldingDetail.Rows[row.Index].Cells["StockReferenceNo"].Value.ToString() == itemRow.SubItems[1].Text)
                            {
                                MessageBox.Show("selecting same item");
                                return;
                            }
                        }

                        AddGridDetailRowForLocation(itemRow);
                       // btnSelectItems.Enabled = true;
                        //btnSelectItems.Enabled = false;
                    }
                }
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

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
        private void AddGridDetailRowForLocation(ListViewItem itemRow)
        {

            grdStockHoldingDetail.Rows.Add();
            grdStockHoldingDetail.Rows[grdStockHoldingDetail.RowCount - 1].Cells["LineNo"].Value = grdStockHoldingDetail.RowCount;
            grdStockHoldingDetail.Rows[grdStockHoldingDetail.RowCount - 1].Cells["StockItemID"].Value = itemRow.SubItems[2].Text;
            grdStockHoldingDetail.Rows[grdStockHoldingDetail.RowCount - 1].Cells["StockItemName"].Value = itemRow.SubItems[3].Text;
            grdStockHoldingDetail.Rows[grdStockHoldingDetail.RowCount - 1].Cells["ModelNo"].Value = itemRow.SubItems[4].Text;
            grdStockHoldingDetail.Rows[grdStockHoldingDetail.RowCount - 1].Cells["MOdelName"].Value = itemRow.SubItems[5].Text;
            grdStockHoldingDetail.Rows[grdStockHoldingDetail.RowCount - 1].Cells["Quantity"].Value = "";

            grdStockHoldingDetail.Rows[grdStockHoldingDetail.RowCount - 1].Cells["InwardDocumentID"].Value = itemRow.SubItems[6].Text;
            grdStockHoldingDetail.Rows[grdStockHoldingDetail.RowCount - 1].Cells["InwardDocumentNo"].Value = itemRow.SubItems[7].Text;
            grdStockHoldingDetail.Rows[grdStockHoldingDetail.RowCount - 1].Cells["InwardDocumentDate"].Value = itemRow.SubItems[8].Text;
            grdStockHoldingDetail.Rows[grdStockHoldingDetail.RowCount - 1].Cells["StockReferenceNo"].Value = itemRow.SubItems[1].Text;


        }
        //-----
        //private void btnCalculateax_Click_1(object sender, EventArgs e)
        //{
        //    verifyAndReworkStockHoldingHeaderGridRows();
        //}

        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grd.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("LoadDocument") ||
                    columnName.Equals("View"))
                {

                    clearData();
                    //track = true;
                    setButtonVisibility(columnName);

                    prevshh = new stockholdingheader();

                    prevshh.CommentStatus = grd.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();
                    prevshh.DocumentID = grd.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    prevshh.TemporaryNo = Convert.ToInt32(grd.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    prevshh.TemporaryDate = DateTime.Parse(grd.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    prevshh.DocumentDate = DateTime.Parse(grd.Rows[e.RowIndex].Cells["DocumentDate"].Value.ToString());
                    if (prevshh.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    prevshh.Comments = StockHoldingHeaderDB.getUserComments(prevshh.DocumentID, prevshh.TemporaryNo, prevshh.TemporaryDate);
                    docID = grd.Rows[e.RowIndex].Cells[0].Value.ToString();
                    StockHoldingHeaderDB popshh = new StockHoldingHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grd.Rows[rowID];
                    prevshh.DocumentNo = Convert.ToInt32(grd.Rows[e.RowIndex].Cells["DocumentNo"].Value.ToString());
                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Document Temp No:" + prevshh.TemporaryNo + "\n" +
                            "Document Temp Date:" + prevshh.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "Document No:" + prevshh.DocumentNo + "\n" +
                            "Document Date:" + prevshh.DocumentDate.ToString("dd-MM-yyyy");
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevshh.TemporaryNo + "-" + prevshh.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_1(null, null);
                        return;
                    }
                    //--------
                    prevshh.StoreLocationID = grd.Rows[e.RowIndex].Cells["gStoreLocationID"].Value.ToString();
                    prevshh.Remarks = grd.Rows[e.RowIndex].Cells["gRemarks"].Value.ToString();
                    prevshh.CommentStatus = grd.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();
                    prevshh.Status = Convert.ToInt32(grd.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    prevshh.DocumentStatus = Convert.ToInt32(grd.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    prevshh.CreateUser = grd.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    prevshh.ForwardUser = grd.Rows[e.RowIndex].Cells["ForwardUser"].Value.ToString();
                    prevshh.ApproveUser = grd.Rows[e.RowIndex].Cells["ApproveUser"].Value.ToString();
                    prevshh.CreateTime = DateTime.Parse(grd.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    prevshh.CreateUser = grd.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    prevshh.ForwarderList = grd.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    //--comments
                    chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    prevshh.CommentStatus = grd.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(prevshh.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(prevshh.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---
                    txtTemporarryNo.Text = prevshh.TemporaryNo.ToString();
                    dtTempDate.Value = prevshh.TemporaryDate;
                    txtDocumentNo.Text = prevshh.DocumentNo.ToString();
                    dtDocumentDate.Value = Convert.ToDateTime(prevshh.DocumentDate.ToString());

                    txtRemarks.Text = prevshh.Remarks.ToString();
                    cmbStoreLocationID.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbStoreLocationID, prevshh.StoreLocationID.ToString());
                    //cmbStoreLocationID.SelectedIndex = cmbStoreLocationID.FindString(prevshh.StoreLocationID.ToString());
                    List<stockholdingdetail> StockHoldingHeaderdetailList = StockHoldingHeaderDB.getStockHoldingHeaderDetail(prevshh);
                    grdStockHoldingDetail.Rows.Clear();
                    int i = 0;
                    try
                    {
                        foreach (stockholdingdetail shd in StockHoldingHeaderdetailList)
                        {
                            AddStockHoldingDetailRow();
                            try
                            {
                                grdStockHoldingDetail.Rows[i].Cells["StockItemID"].Value = shd.StockItemID;
                            }
                            catch (Exception ex)
                            {
                                grdStockHoldingDetail.Rows[i].Cells["StockItemID"].Value = null;
                            }
                            grdStockHoldingDetail.Rows[i].Cells["Quantity"].Value = shd.Quantity;
                            grdStockHoldingDetail.Rows[i].Cells["InwardDocumentID"].Value = shd.InwardDocumentID;
                            grdStockHoldingDetail.Rows[i].Cells["InwardDocumentNo"].Value = shd.InwardDocumentNo;
                            grdStockHoldingDetail.Rows[i].Cells["InwardDocumentDate"].Value = shd.InwardDocumentDate;
                            grdStockHoldingDetail.Rows[i].Cells["StockReferenceNo"].Value = shd.StockReferenceNo;
                            grdStockHoldingDetail.Rows[i].Cells["StockItemName"].Value = shd.StockItemName;
                            grdStockHoldingDetail.Rows[i].Cells["ModelNo"].Value = shd.ModelNo;
                            grdStockHoldingDetail.Rows[i].Cells["ModelName"].Value = shd.ModelName;
                            grdStockHoldingDetail.Columns["InwardDocumentID"].Visible = false;
                            grdStockHoldingDetail.Columns["InwardDocumentNo"].Visible = false;
                            grdStockHoldingDetail.Columns["InwardDocumentDate"].Visible = false;
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
                //if (!verifyAndReworkStockHoldingHeaderGridRows())
                //{
                //    MessageBox.Show("Error found in Stock details. Please correct before updating the details");
                //}
                btnSave.Text = "Update";
                pnlList.Visible = false;
                pnlAddEdit.Visible = true;
                tabControl1.SelectedTab = tabStockHoldingHeader;
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
                for (int i = grdStockHoldingDetail.Rows.Count - 1; i >= 0; i--)
                {
                    grdStockHoldingDetail.Rows.RemoveAt(i);
                }
            }
            catch (Exception)
            {
            }
            MessageBox.Show("item cleared.");
        }
        private Boolean updateDashBoard(stockholdingheader shh, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = shh.DocumentID;
                dsb.TemporaryNo = shh.TemporaryNo;
                dsb.TemporaryDate = shh.TemporaryDate;
                dsb.DocumentNo = shh.DocumentNo;
                dsb.DocumentDate = shh.DocumentDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = shh.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(shh.DocumentID);
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
                if (!verifyAndReworkStockHoldingHeaderGridRows())
                {
                    return;
                }
                StockHoldingHeaderDB shhdb = new StockHoldingHeaderDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevshh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevshh.CommentStatus);
                    prevshh.DocumentNo = DocumentNumberDB.getNewNumber(docID, 2);
                    // prevmrn.MRNDate = DateTime.Now;

                    if (shhdb.ApproveStockHoldingHeader(prevshh))
                    {
                        MessageBox.Show("StockOnHold Document Approved");
                        List<stockholdingdetail> stockDetail = getStockList();
                        if (shhdb.updatePRInStock(stockDetail))
                        {
                            MessageBox.Show("Stock Document Updated");
                            if (!updateDashBoard(prevshh, 2))
                            {
                                MessageBox.Show("DashBoard Fail to update");
                            }
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredStockHoldingHeader(listOption);
                            setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                        }


                    }
                    else
                        MessageBox.Show("Unable to approve");
                }
            }
            catch (Exception)
            {
            }

        }
        private List<stockholdingdetail> getStockList()
        {
            List<stockholdingdetail> StockHoldingHeaderDetailList = new List<stockholdingdetail>();
            stockholdingdetail shd;
            for (int i = 0; i < grdStockHoldingDetail.Rows.Count; i++)
            {
                try
                {
                    shd = new stockholdingdetail();
                    shd.StockItemID = grdStockHoldingDetail.Rows[i].Cells["StockItemID"].Value.ToString();
                    shd.Quantity = Convert.ToDouble(grdStockHoldingDetail.Rows[i].Cells["Quantity"].Value);
                    shd.InwardDocumentID = grdStockHoldingDetail.Rows[i].Cells["InwardDocumentID"].Value.ToString();
                    shd.InwardDocumentNo = grdStockHoldingDetail.Rows[i].Cells["InwardDocumentNo"].Value.ToString();
                    shd.InwardDocumentDate = Convert.ToDateTime(grdStockHoldingDetail.Rows[i].Cells["InwardDocumentDate"].Value);
                    shd.StockReferenceNo = Convert.ToInt32(grdStockHoldingDetail.Rows[i].Cells["StockReferenceNo"].Value.ToString());
                    StockHoldingHeaderDetailList.Add(shd);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fails to retrive stock details from grdStockHoldingDetail");
                }

            }
            return StockHoldingHeaderDetailList;
        }

        private Boolean clickApprove(List<stockholdingdetail> StockHoldingdetail)
        {
            Boolean status = true;
            try
            {
                foreach (stockholdingdetail sdh in StockHoldingdetail)
                {
                    int r = sdh.StockReferenceNo;
                    double d = sdh.Quantity;

                    //double lv = StockHoldingHeaderDetailList.Add(sdh);

                }
            }
            catch (Exception ex)
            {
            }
            return status;

        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
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
                            StockHoldingHeaderDB shhdb = new StockHoldingHeaderDB();
                            prevshh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevshh.CommentStatus);
                            prevshh.ForwardUser = approverUID;
                            prevshh.ForwarderList = prevshh.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (shhdb.forwardStockHoldingHeader(prevshh))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(prevshh, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredStockHoldingHeader(listOption);
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
                    string s = prevshh.ForwarderList;
                    string reverseStr = getReverseString(prevshh.ForwarderList);
                    //do forward activities
                    prevshh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevshh.CommentStatus);
                    StockHoldingHeaderDB shhdb = new StockHoldingHeaderDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevshh.ForwarderList = reverseStr.Substring(0, ind);
                        prevshh.ForwardUser = reverseStr.Substring(ind + 3);
                        prevshh.DocumentStatus = prevshh.DocumentStatus - 1;
                    }
                    else
                    {
                        prevshh.ForwarderList = "";
                        prevshh.ForwardUser = "";
                        prevshh.DocumentStatus = 1;
                    }
                    if (shhdb.reverseStockHoldingHeader(prevshh))
                    {
                        MessageBox.Show("StockHolding Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredStockHoldingHeader(listOption);
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
        private void btnListDocuments_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevshh.TemporaryNo + "-" + prevshh.TemporaryDate.ToString("yyyyMMddhhmmss"));
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
                    string subDir = prevshh.TemporaryNo + "-" + prevshh.TemporaryDate.ToString("yyyyMMddhhmmss");
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
                    tabControl1.SelectedTab = tabStockHoldingHeader;
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
                    tabControl1.SelectedTab = tabStockHoldingHeader;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                    //btnQC.Visible = true;
                    disableTabPages();

                    tabControl1.SelectedTab = tabStockHoldingHeader;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabStockHoldingHeader;
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
                    grd.Columns["Edit"].Visible = false;
                    grd.Columns["Approve"].Visible = false;
                    btnActionPending.Visible = false;
                    btnApprovalPending.Visible = false;
                    btnApproved.Visible = false;
                    if (ups == 1)
                    {
                        grd.Columns["View"].Visible = true;
                    }
                    else
                    {
                        grd.Columns["View"].Visible = false;
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
            grd.Columns["Edit"].Visible = false;
            grd.Columns["Approve"].Visible = false;
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                if (listOption == 1)
                {
                    grd.Columns["Edit"].Visible = true;
                    grd.Columns["Approve"].Visible = true;
                }
            }
        }

        void handleGrdViewButton()
        {
            grd.Columns["View"].Visible = false;
            //if any one of view,add and edit
            if (Main.itemPriv[0] || Main.itemPriv[1] || Main.itemPriv[2])
            {
                //list option 1 should not have view buttuon visible (edit is avialable)
                if (listOption != 1)
                {
                    grd.Columns["View"].Visible = true;
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
                removeControlsFromCommenterPanel();
                removeControlsFromForwarderPanel();
                dgvComments.Rows.Clear();
                chkCommentStatus.Checked = false;
                txtComments.Text = "";
                grdStockHoldingDetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSelectPO_Click(object sender, EventArgs e)
        {
            //if (cmbCustomer.SelectedIndex == -1)
            //{
            //    MessageBox.Show("select one customer");
            //    return;
            //}
            //if (txtReference.Text.Trim().Length != 0)
            //{
            //    DialogResult dialog = MessageBox.Show("Warning: \nReference will be removed", "", MessageBoxButtons.OKCancel);
            //    if (dialog == DialogResult.OK)
            //        txtReference.Text = "";
            //    else
            //        return;
            //}
            //if (txtPONo.Text.Length != 0 || grdInvoiceInDetail.Rows.Count > 0)
            //{
            //    DialogResult dialog = MessageBox.Show("Warning:\nPONo , PODate,TaxCode,MRNValue, Product Value and MRN detail will be removed", "", MessageBoxButtons.OKCancel);
            //    if (dialog == DialogResult.OK)
            //    {
            //        grdInvoiceInDetail.Rows.Clear();
            //        txtMRNValue.Text = "";
            //        txtTaxAmount.Text = "";
            //        txtProductValue.Text = "";
            //    }
            //    else
            //        return;
            //}
            //btnSelectPO.Enabled = false;

            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;

            pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
            lv = MRNHeaderDB.getMRNHeaderListView();
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            lv.Bounds = new Rectangle(new Point(50, 50), new Size(500, 200));
            pnllv.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Location = new Point(50, 270);
            lvOK.Click += new System.EventHandler(this.lvOK_Click1);
            pnllv.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Location = new Point(150, 270);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click1);
            pnllv.Controls.Add(lvCancel);

            pnlAddEdit.Controls.Add(pnllv);
            pnllv.BringToFront();
            pnllv.Visible = true;
        }
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                //btnSelectPO.Enabled = true;
                pnllv.Visible = false;
                int kount = 0;
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        kount++;
                    }
                }
                if (kount == 0)
                {
                    MessageBox.Show("Select one PO");
                    return;
                }
                if (kount > 1)
                {
                    MessageBox.Show("Select only one PO");
                    return;
                }
                else
                {
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            //MessageBox.Show(itemRow.SubItems[1].Text);
                            //trlist = trlist + itemRow.SubItems[3].Text + "(" + itemRow.SubItems[4].Text + ");";
                            txtTemporarryNo.Text = itemRow.SubItems[1].Text;
                            dtTempDate.Value = Convert.ToDateTime(itemRow.SubItems[2].Text);
                            txtDocumentNo.Text = itemRow.SubItems[3].Text;
                            dtDocumentDate.Value = Convert.ToDateTime(itemRow.SubItems[4].Text);
                            //cmbStoreLocationID.Value = Convert.ToDateTime(itemRow.SubItems[4].Text);
                            txtRemarks.Text = itemRow.SubItems[5].Text;
                            //dtSupplierInvoiceDate.Value = Convert.ToDateTime(itemRow.SubItems[6].Text);
                            //txtCustomerName.Text = itemRow.SubItems[7].Text;
                            //AddGridDetailRowForPO();
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
                //  btnSelectMRNNo.Enabled = true;
                pnllv.Visible = false;
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
            //txtReference.Enabled = false;
            //txtReference.Text = "";
        }
        private void txtDCNo_TextChanged(object sender, EventArgs e)
        {
        }
        private void txtInvoiceNo_TextChanged(object sender, EventArgs e)
        {
        }

        private void listView1_ItemChecked1(object sender, ItemCheckedEventArgs e)
        {
            try
            {
                if (lv.CheckedIndices.Count > 1)
                {
                    MessageBox.Show("Cannot select more than one item");
                    e.Item.Checked = false;
                }
            }
            catch (Exception)
            {
            }
        }



        private void btnCalculate_Click(object sender, EventArgs e)
        {
            verifyAndReworkStockHoldingHeaderGridRows();
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


        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dtMRNDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtMRNNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabMRNHeader_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void cmbTaxCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkTemp = ((Structures.ComboBoxItem)cmbStoreLocationID.SelectedItem).HiddenValue;
            //chkTemp = cmbStoreLocationID.SelectedItem.ToString().Trim().Substring(0, cmbStoreLocationID.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void txtTemporarryNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbStoreLocationID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (grdStockHoldingDetail.Rows.Count != 0)
            {
                if (chkTemp == ((Structures.ComboBoxItem)cmbStoreLocationID.SelectedItem).HiddenValue)
                {
                    return;
                }
                else
                {
                    DialogResult dialog = MessageBox.Show("Warning:\n Stock Holding Detail will be Removed", "", MessageBoxButtons.OKCancel);
                    if (dialog == DialogResult.OK)
                    {

                        //foreach (DataGridViewRow row in grdStockHoldingDetail.Rows)
                        //{
                        //    grdStockHoldingDetail.Rows[row.Index].Cells["StoreLocationID"].Value = "";

                        //}

                        grdStockHoldingDetail.Rows.Clear();

                        //cmbStoreLocationID.SelectedIndex = cmbStoreLocationID.FindString(prevshh.StoreLocationID);
                    }
                    else
                        cmbStoreLocationID.SelectedIndex = cmbStoreLocationID.FindString(prevshh.StoreLocationID);
                }
            }

        }

        private void pnlEditButtons_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtRemarks_TextChanged(object sender, EventArgs e)
        {

        }

        private void StockHoldingHeader_Enter(object sender, EventArgs e)
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

