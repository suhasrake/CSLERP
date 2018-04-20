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
    public partial class IndentHeader : System.Windows.Forms.Form
    {
        string docID = "INDENT";
        string forwarderList = "";
        string approverList = "";
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        //double productvalue = 0.0;
        //double taxvalue = 0.0;
        indentheader previheader = new indentheader();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        ////Boolean captureChange = false;
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list

        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        double productvalue = 0.0;
        double taxvalue = 0.0;
        Boolean userIsACommenter = false;

        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        //popiheader prevpopi = new popiheader();

        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();

        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();

        public IndentHeader()
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
                ListFilteredIndentHeader(listOption);
                //applyPrivilege();
            }
            catch (Exception)
            {

            }
        }

        private void ListFilteredIndentHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                IndentHeaderDB ihdb = new IndentHeaderDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                List<indentheader> IHeaders = ihdb.getFilteredIndentHeader(userString, option, userCommentStatusString);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (indentheader ih in IHeaders)
                {
                    if (option == 1)
                    {
                        if (ih.DocumentStatus == 99)
                            continue;
                    }
                    //else
                    //{
                    //    if (!(ih.CreateUser.Equals(Login.userLoggedIn) ||
                    //        ih.ForwardUser.Equals(Login.userLoggedIn) ||
                    //        ih.ApproveUser.Equals(Login.userLoggedIn)))
                    //    {
                    //        //if not relevent to the user looged in
                    //        continue;
                    //    }
                    //}
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
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyID"].Value = ih.CurrencyID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyName"].Value = ih.CurrencyName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gRemarks"].Value = ih.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = ih.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = ih.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gAcceptanceStatus"].Value = ih.AcceptanceStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = ih.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = ih.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreator"].Value = ih.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gForwarder"].Value = ih.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gApprover"].Value = ih.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ComntStatus"].Value = ih.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = ih.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwarderLists"].Value = ih.ForwarderList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Indent Header Listing");
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
            fillStatusCombo(cmbStatus);
            //StockItemDB.fillStockItemCombo(cmbProductCodes, "Products");
            CurrencyDB.fillCurrencyCombo(cmbCurrency);
            dtTemporaryDate.Format = DateTimePickerFormat.Custom;
            dtTemporaryDate.CustomFormat = "dd-MM-yyyy";
            dtTemporaryDate.Enabled = false;
            dtIndentDate.Format = DateTimePickerFormat.Custom;
            dtIndentDate.CustomFormat = "dd-MM-yyyy";
            dtIndentDate.Enabled = false;
            dtTargetDate.Format = DateTimePickerFormat.Custom;
            
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
                cmbCurrency.SelectedIndex = -1;
                grdIndentDetail.Rows.Clear();
                txtProductCodes.Text = "";
                txtTemporaryNo.Text = "";
                txtIndentNo.Text = "";
                txtReferenceInternalNo.Text = "";
                txtRemarks.Text = "";
                dtTemporaryDate.Value = DateTime.Parse("01-01-1900");
                dtIndentDate.Value = DateTime.Parse("01-01-1900");
                dtTargetDate.Value = DateTime.Today.Date;
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
                MessageBox.Show(ex.ToString());
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
                DataGridViewComboBoxCell ComboColumn1 = (DataGridViewComboBoxCell)(grdIndentDetail.Rows[kount - 1].Cells["Item"]);
                StockItemDB.fillStockItemGridViewCombo(ComboColumn1, "");
                ComboColumn1.DropDownWidth = 300;

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

            try
            {
                if (grdIndentDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in PO Product Inward details");
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
                        (grdIndentDetail.Rows[i].Cells["ModelDetails"].Value == null) ||
                        (Convert.ToDouble(grdIndentDetail.Rows[i].Cells["ExpectedPurchasePrice"].Value) == 0) ||
                        (qr == 0) || (qtbp == 0))

                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                   
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
                //if (docID == "INDENTHEADER")
                //{

                for (int i = 0; i < grdIndentDetail.Rows.Count - 1; i++)
                {
                    for (int j = i + 1; j < grdIndentDetail.Rows.Count; j++)
                    {

                        if (grdIndentDetail.Rows[i].Cells[1].Value.ToString() == grdIndentDetail.Rows[j].Cells["Item"].Value.ToString())
                        {
                            //duplicate item code
                            MessageBox.Show("Item code duplicated in OB details... please ensure correctness (" +
                                grdIndentDetail.Rows[i].Cells["Item"].Value.ToString() + ")");
                        }
                    }
                }
                //}
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        private Boolean createAndUpdateIndentDetails(indentheader ih)
        {
            Boolean status = true;
            try
            {
                IndentHeaderDB ihdb = new IndentHeaderDB();
                indentdetail id = new indentdetail();

                List<indentdetail> IDetails = new List<indentdetail>();
                for (int i = 0; i < grdIndentDetail.Rows.Count; i++)
                {
                    try
                    {
                        id = new indentdetail();
                        id.DocumentID = ih.DocumentID;
                        id.TemporaryNo = ih.TemporaryNo;
                        id.TemporaryDate = ih.TemporaryDate;
                        id.StockItemID = grdIndentDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Substring(0, grdIndentDetail.Rows[i].Cells["Item"].Value.ToString().Trim().IndexOf('-'));
                        id.ModelDetails = grdIndentDetail.Rows[i].Cells["ModelDetails"].Value.ToString();
                        id.LastPurchasedPrice = Convert.ToDouble(grdIndentDetail.Rows[i].Cells["LastPurchasePrice"].Value);
                        id.QuotedPrice = Convert.ToDouble(grdIndentDetail.Rows[i].Cells["QuotedPrice"].Value);
                        id.ExpectedPurchasePrice = Convert.ToDouble(grdIndentDetail.Rows[i].Cells["ExpectedPurchasePrice"].Value);
                        id.QuotationNo = grdIndentDetail.Rows[i].Cells["QuotationNo"].Value.ToString();
                        id.Quantity = Convert.ToDouble(grdIndentDetail.Rows[i].Cells["QuantityRequired"].Value);
                        id.Stock = Convert.ToDouble(grdIndentDetail.Rows[i].Cells["Stock"].Value);
                        id.BufferQuantity = Convert.ToDouble(grdIndentDetail.Rows[i].Cells["BufferQuantity"].Value);
                        id.WarrantyDays = Convert.ToInt32(grdIndentDetail.Rows[i].Cells["WarrantyDays"].Value);
                        IDetails.Add(id);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("createAndUpdateIndentDetails() : Error creating Indent Details");
                        status = false;
                    }
                }
                if (!ihdb.UpdateIndentDetail(IDetails, ih))
                {
                    MessageBox.Show("createAndUpdateIndentDetails() : Failed to update Indent Details. Please check the values");
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("createAndUpdateIndentDetails() : Error updating Indent Details");
                status = false;
            }
            return status;
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
                    ih.ReferenceInternalOrders = txtReferenceInternalNo.Text;
                    ih.ProductCode = txtProductCodes.Text;
                    ih.CurrencyID = cmbCurrency.SelectedItem.ToString().Trim().Substring(0, cmbCurrency.SelectedItem.ToString().Trim().IndexOf('-'));
                    ih.Remarks = txtRemarks.Text;
                    //ih.Status = ComboFIll.getStatusCode(cmbStatus.SelectedItem.ToString());
                    ih.Comments = docCmtrDB.DGVtoString(dgvComments);
                    ih.ForwarderList = previheader.ForwarderList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }

                if (!idb.validateIndentHeader(ih))
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    ih.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    ih.DocumentStatus = 1; //created
                    ih.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    ih.TemporaryNo = Convert.ToInt32(txtTemporaryNo.Text);
                    ih.TemporaryDate = previheader.TemporaryDate;
                    ih.DocumentStatus = previheader.DocumentStatus;
                }

                if (idb.validateIndentHeader(ih))
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
                    int tmpStatus = 0;
                    if (chkCommentStatus.Checked)
                    {
                        tmpStatus = 1;
                    }
                    if (txtComments.Text.Trim().Length > 0)
                    {
                        ih.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus);
                    }
                    if (btnText.Equals("Update"))
                    {
                        if (idb.updateIndentHeader(ih))
                        {
                            if (createAndUpdateIndentDetails(ih))
                            {
                                MessageBox.Show("Indent Details updated");
                                closeAllPanels();
                                ListFilteredIndentHeader(1);
                                //pnlBottomActions.Visible = true;
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
                            MessageBox.Show("Failed to update indent Header");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (idb.insertIndentHeader(ih))
                        {
                            if (createAndUpdateIndentDetails(ih))
                            {
                                MessageBox.Show("Indent Details Added");
                                closeAllPanels();
                                ListFilteredIndentHeader(1);
                                //pnlBottomActions.Visible = true;
                            }
                            else
                            {
                                status = false;
                                idb.deleteIndentHeader(ih);
                            }
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert Indent Header");
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
                MessageBox.Show("createAndUpdateIHDetails() : Error");
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
                    IndentHeaderDB idb = new IndentHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    previheader = new indentheader();
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
                    previheader.CurrencyID = grdList.Rows[e.RowIndex].Cells["gCurrencyID"].Value.ToString();
                    previheader.Remarks = grdList.Rows[e.RowIndex].Cells["gRemarks"].Value.ToString();
                    previheader.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    previheader.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    previheader.AcceptanceStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gAcceptanceStatus"].Value.ToString());
                    previheader.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    previheader.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    previheader.CreatorName = grdList.Rows[e.RowIndex].Cells["gCreator"].Value.ToString();
                    previheader.ForwarderName = grdList.Rows[e.RowIndex].Cells["gForwarder"].Value.ToString();
                    previheader.ApproverName = grdList.Rows[e.RowIndex].Cells["gApprover"].Value.ToString();
                    previheader.ForwarderList = grdList.Rows[e.RowIndex].Cells["ForwarderLists"].Value.ToString();
                    //--comments
                    chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    previheader.CommentStatus = grdList.Rows[e.RowIndex].Cells["ComntStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(previheader.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(previheader.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

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
                    cmbCurrency.SelectedIndex = cmbCurrency.FindString(previheader.CurrencyID);
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(ComboFIll.getStatusString(Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString())));
                    txtProductCodes.Text = previheader.ProductCode;
                    txtReferenceInternalNo.Text = previheader.ReferenceInternalOrders;
                    try
                    {
                        dtTargetDate.Value = previheader.TargetDate;
                    }
                    catch (Exception)
                    {

                        dtTargetDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtRemarks.Text = previheader.Remarks.ToString();

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
            AddIndentDetailRow();
        }

        private void button2_Click_1(object sender, EventArgs e)
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
        private void btnForward_Click(object sender, EventArgs e)
        {
            removeControlsFromForwarderPanel();
            lvApprover = new ListView();
            lvApprover.Clear();
            pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
            pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));

            lvApprover = DocEmpMappingDB.ApproverLV(docID, Login.empLoggedIn);
            lvApprover.Bounds = new Rectangle(new Point(50, 50), new Size(500, 200));
            pnlForwarder.Controls.Remove(lvApprover);
            pnlForwarder.Controls.Add(lvApprover);

            Button lvForwrdOK = new Button();
            lvForwrdOK.Text = "OK";
            lvForwrdOK.Size = new Size(150, 20);
            lvForwrdOK.Location = new Point(50, 270);
            lvForwrdOK.Click += new System.EventHandler(this.lvForwardOK_Click);
            pnlForwarder.Controls.Add(lvForwrdOK);

            Button lvForwardCancel = new Button();
            lvForwardCancel.Text = "Cancel";
            lvForwardCancel.Size = new Size(150, 20);
            lvForwardCancel.Location = new Point(250, 270);
            lvForwardCancel.Click += new System.EventHandler(this.lvForwardCancel_Click);
            pnlForwarder.Controls.Add(lvForwardCancel);
            ////lvForwardCancel.Visible = false;

            pnlForwarder.Visible = true;
            pnlAddEdit.Controls.Add(pnlForwarder);
            pnlAddEdit.BringToFront();
            pnlForwarder.BringToFront();
            pnlForwarder.Focus();
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                IndentHeaderDB idb = new IndentHeaderDB();
                //indentheader popih = new indentheader();
                //FinancialLimitDB flDB = new FinancialLimitDB();
                //if (!flDB.checkEmployeeFinancialLimit(docID, Login.empLoggedIn, Convert.ToDouble(txtProductValue.Text), 0))
                //{
                //    MessageBox.Show("No financial power for approving this document");
                //    return;
                //}
                DialogResult dialog = MessageBox.Show("Are you sure to forward the document for Approval ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    previheader.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(previheader.CommentStatus);
                    previheader.IndentNo = DocumentNumberDB.getNewNumber(docID, 2);
                    if (idb.ApproveIndentHeader(previheader))
                    {
                        MessageBox.Show("PO Product Inward Document Approved");
                        closeAllPanels();
                        ListFilteredIndentHeader(1);
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
                        string stid = grdIndentDetail.Rows[e.RowIndex].Cells["Item"].Value.ToString();
                        stid = stid.Substring(0, stid.IndexOf('-'));
                        ShowQuotationDetails(stid);
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void IndentHeader_Load(object sender, EventArgs e)
        {

        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            verifyAndReworkIndentDetailGridRows();
        }
        private void ShowQuotationDetails(string stockID)
        {
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;

            pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
            lv = QIHeaderDB.QIPriceSelectionView(stockID);
            ////this.lv.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.listView1_ItemCheck);
            this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);

            lv.Bounds = new Rectangle(new Point(50, 50), new Size(500, 200));
            pnllv.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Location = new Point(50, 270);
            lvOK.Click += new System.EventHandler(this.lvOK_Click);
            pnllv.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Location = new Point(150, 270);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click);
            pnllv.Controls.Add(lvCancel);

            pnlAddEdit.Controls.Add(pnllv);
            pnllv.BringToFront();
            pnllv.Visible = true;
        }
        private void lvOK_Click(object sender, EventArgs e)
        {
            try
            {
                pnllv.Visible = false;
                ////ArrayList lviItemsArrayList = new ArrayList();
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
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_Click(object sender, EventArgs e)
        {
            try
            {
                pnllv.Visible = false;
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

        ////private void listView1_ItemCheck(object sender, ItemCheckEventArgs e)
        ////{
        ////    if (lv.CheckedIndices.Count > 1)
        ////    {
        ////        MessageBox.Show("Cannot select more than 1-1");
        ////    }
        ////}

        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
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

        private void btnReferenceIO_Click(object sender, EventArgs e)
        {
            btnReferenceIO.Enabled = false;
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;

            pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
            lv = InternalOrderDB.ReferenceIOSelectionView("IOPRODUCT");

            string strArry = txtReferenceInternalNo.Text.ToString();
            /*
            int slen = strArry.IndexOf('(');
            string ino = strArry.Substring(0, slen);
            string idate = strArry.Substring(slen + 1, strArry.Length - (slen + 1)).Replace(");" , "");
            foreach (ListViewItem itemRow in lv.Items)
            {
                if ((itemRow.SubItems[2].Text.Trim().Equals(ino.Trim())) &&
                    (itemRow.SubItems[3].Text.Trim().Equals(idate.Trim())))
                {
                    itemRow.Checked = true;
                }

            }*/
            this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView2_ItemChecked);
            lv.Bounds = new Rectangle(new Point(50, 50), new Size(500, 200));
            pnllv.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Location = new Point(50, 270);
            lvOK.Click += new System.EventHandler(this.lvOK_Clicked);
            pnllv.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Location = new Point(150, 270);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Clicked);
            pnllv.Controls.Add(lvCancel);

            pnlAddEdit.Controls.Add(pnllv);
            pnllv.BringToFront();
            pnllv.Visible = true;
        }
        private void lvOK_Clicked(object sender, EventArgs e)
        {
            try
            {
                btnReferenceIO.Enabled = true;
                pnllv.Visible = false;
                ////ArrayList lviItemsArrayList = new ArrayList();
                string iolist = "";
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        iolist = iolist + itemRow.SubItems[2].Text + "(" + itemRow.SubItems[3].Text + ");";
                    }
                }
                txtReferenceInternalNo.Text = iolist;
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_Clicked(object sender, EventArgs e)
        {
            try
            {
                btnReferenceIO.Enabled = true;
                pnllv.Visible = false;
            }
            catch (Exception)
            {
            }
        }
        private void listView2_ItemChecked(object sender, ItemCheckedEventArgs e)
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
        private void btnSelectProductCodes_Click(object sender, EventArgs e)
        {
            btnSelectProductCodes.Enabled = false;
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;

            pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
            lv = StockItemDB.ProductCodeSelectionView();
            string[] strArry = txtProductCodes.Text.Split(new string[] { ";" }, StringSplitOptions.None);
            for (int i = 0; i < strArry.Length; i++)
            {
                try
                {
                    string pcode = strArry[i];
                    int slen = strArry[i].IndexOf('-');
                    string sid = strArry[i].Substring(0, slen);
                    string sname = strArry[i].Substring(slen + 1, strArry[i].Length - (slen + 1));
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if ((itemRow.SubItems[1].Text.Trim().Equals(sid.Trim())) &&
                            (itemRow.SubItems[2].Text.Trim().Equals(sname.Trim())))
                        {
                            itemRow.Checked = true;
                        }

                    }
                }
                catch (Exception ex)
                {
                }
            }
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
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
            pnllv.Controls.Add(lvCancel);

            pnlAddEdit.Controls.Add(pnllv);
            pnllv.BringToFront();
            pnllv.Visible = true;
        }
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                btnSelectProductCodes.Enabled = true;
                pnllv.Visible = false;
                string iolist = "";
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        iolist = iolist + itemRow.SubItems[1].Text + "-" + itemRow.SubItems[2].Text + ";";
                        
                    }
                }
                txtProductCodes.Text = iolist;
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                btnSelectProductCodes.Enabled = true;
                pnllv.Visible = false;
            }
            catch (Exception)
            {
            }
        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //-----
        //comment handling procedurs and functions
        //-----
        private void btnSelectCommenters_Click(object sender, EventArgs e)
        {
            removeControlsFromCommenterPanel();
            docCmtrDB = new DocCommenterDB();
            lvCmtr = new ListView();
            lvCmtr.Clear();
            pnlCmtr.BorderStyle = BorderStyle.FixedSingle;
            pnlCmtr.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
            lvCmtr = docCmtrDB.commenterLV(docID);
            docCmtrDB.verifyCommenterList(lvCmtr, dtCmtStatus);
            lvCmtr.Bounds = new Rectangle(new Point(50, 50), new Size(500, 200));
            pnlCmtr.Controls.Add(lvCmtr);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Size = new Size(150, 20);
            lvOK.Location = new Point(50, 270);
            lvOK.Click += new System.EventHandler(this.lvCmntOK_Click);
            pnlCmtr.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Size = new Size(150, 20);
            lvCancel.Location = new Point(250, 270);
            lvCancel.Click += new System.EventHandler(this.lvCmntCancel_Click);
            pnlCmtr.Controls.Add(lvCancel);
            ////lvCancel.Visible = true;

            pnlCmtr.BringToFront();
            pnlCmtr.Visible = true;
            pnlComments.Controls.Add(pnlCmtr);
            pnlComments.BringToFront();
            pnlCmtr.BringToFront();
        }
        //private void btnGetComments_Click(object sender, EventArgs e)
        //{
            

        //}
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
                pnlCmtr.Visible = false;
                pnlCmtr.Controls.Remove(lvCmtr);
            }
            catch (Exception)
            {
            }
        }

        private void lvCmntCancel_Click(object sender, EventArgs e)
        {
            try
            {
                lvCmtr.CheckBoxes = false;
                lvCmtr.CheckBoxes = true;
                pnlCmtr.Visible = false;
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
                                pnlCmtr.Visible = false;
                                pnlCmtr.Controls.Remove(lvApprover);
                                MessageBox.Show("Document Forwarded");
                                closeAllPanels();
                                ListFilteredIndentHeader(1);
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
                lvApprover.CheckBoxes = false;
                lvApprover.CheckBoxes = true;
                pnlForwarder.Controls.Remove(lvApprover);
                pnlForwarder.Visible = false;
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
                        MessageBox.Show("PO Product Inward Document Reversed");
                        closeAllPanels();
                        ListFilteredIndentHeader(1);
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
                foreach (Control p in pnlCmtr.Controls)
                    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
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
                foreach (Control p in pnlForwarder.Controls)
                    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
                    {
                        p.Dispose();
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
                if (e.ColumnIndex == 0)
                {
                    removePDFControls();
                    fileName = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    string docDir = Main.documentDirectory + "\\" + docID;
                    string subDir = previheader.TemporaryNo + "-" + previheader.TemporaryDate.ToString("yyyyMMddhhmmss");
                    string fname = docDir + "\\" + subDir + "\\" + fileName;
                    showPDFFile(fname);
                    dgv.Visible = false;
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
                    tabControl1.SelectedTab = tabIndentHeader;
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
                    btnGetComments.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = true;
                    chkCommentStatus.Visible = true;
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
                removeControlsFromCommenterPanel();
                removeControlsFromForwarderPanel();
                dgvComments.Rows.Clear();
                chkCommentStatus.Checked = false;
                txtComments.Text = "";
                grdIndentDetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }

       
    }
}

