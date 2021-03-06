﻿using System;
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
    public partial class SMRNHeader : System.Windows.Forms.Form
    {
        string docID = "SMRNHEADER";
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        double productvalue = 0.0;
        double taxvalue = 0.0;
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        smrnheader prevsmrnh;
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        Panel pnlModel = new Panel();
        TreeView tv = new TreeView();
        Form frmPopup = new Form();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        //popiheader prevpopi = new popiheader();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        Boolean userIsACommenter = false;
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        public SMRNHeader()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void SMRNHeader_Load(object sender, EventArgs e)
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
            ListFilteredSMRNHeader(listOption);
            //applyPrivilege();
        }
        private void ListFilteredSMRNHeader(int opt)
        {
            try
            {
                grdList.Rows.Clear();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                SMRNHeaderDB smrnhDB = new SMRNHeaderDB();
                List<smrnheader> SMRNHeaders = smrnhDB.getFilteredSMRNHeader(userString, opt, userCommentStatusString);
                if (opt == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (opt == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (opt == 3 || opt == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (smrnheader smrnh in SMRNHeaders)
                {
                    if (opt == 1)
                    {
                        if (smrnh.DocumentStatus == 99)
                            continue;
                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = smrnh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = smrnh.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerID"].Value = smrnh.CustomerID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerName"].Value = smrnh.CustomerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = smrnh.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = smrnh.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentNo"].Value = smrnh.DocumentNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentDate"].Value = smrnh.DocumentDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["SMRNNo"].Value = smrnh.SMRNNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["SMRNDate"].Value = smrnh.SMRNDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["aTrackingNo"].Value = smrnh.TrackingNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["aTrackingDate"].Value = smrnh.TrackingDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = smrnh.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = smrnh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = smrnh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwardUser"].Value = smrnh.ForwardUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ApproveUser"].Value = smrnh.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = smrnh.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = smrnh.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = smrnh.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["PreCheckStatus"].Value = smrnh.PreCheckStatus;
                    //grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatusNo"].Value = ComboFIll.getDocumentStatusString(qih.DocumentStatus);
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatusNo"].Value = smrnh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CommetnStatus"].Value = smrnh.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = smrnh.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = smrnh.ForwarderList;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in SMRN Details Listing");
            }
            setButtonVisibility("init");
            if (opt == 3)
            {
                grdList.Columns["Edit"].Visible = true;
            }
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
            //docID = Main.currentDocument;
            dtTemporaryDate.Format = DateTimePickerFormat.Custom;
            dtTemporaryDate.CustomFormat = "dd-MM-yyyy";
            dtDocumentDate.Format = DateTimePickerFormat.Custom;
            dtDocumentDate.CustomFormat = "dd-MM-yyyy";
            //dtDocumentDate.Enabled = false;
            dtSMRNDate.Format = DateTimePickerFormat.Custom;
            dtSMRNDate.CustomFormat = "dd-MM-yyyy";
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            grdSMRNHDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //create tax details table for tax breakup display
            txtComments.Text = "";

            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
        }
        //private void fillStatusCombo(System.Windows.Forms.ComboBox cmb)
        //{
        //    try
        //    {
        //        cmb.Items.Clear();
        //        for (int i = 0; i < Main.statusValues.GetLength(0); i++)
        //        {
        //            cmb.Items.Add(Main.statusValues[i, 1]);
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
                btnAddNewLine.Enabled = true;
                btnClearEntries.Enabled = true;
                //clear all grid views
                grdSMRNHDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                //----------clear temperory panels
                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                dtDocumentDate.Value = DateTime.Parse("01-01-1900");
                dtSMRNDate.Value = DateTime.Parse("01-01-1900");
                dtTemporaryDate.Value = DateTime.Parse("01-01-1900");
                txtTemporaryNo.Text = "";
                grdSMRNHDetail.Rows.Clear();
                txtDocumentNo.Text = "";
                txtSMRNNo.Text = "";
                prevsmrnh = new smrnheader();
                removeControlsFromForwarderPanelTV();
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
                tabControl1.SelectedTab = tabSMRNHeader;
                setButtonVisibility("btnNew");
                setDetailGridData(1);
            }
            catch (Exception)
            {

            }
        }


        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                AddSMRNDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddSMRNDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdSMRNHDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkSMRNDetailGridRows())
                    {
                        return false;
                    }
                }
                grdSMRNHDetail.Rows.Add();
                int kount = grdSMRNHDetail.RowCount;
                grdSMRNHDetail.Rows[kount - 1].Cells[0].Value = kount;
                grdSMRNHDetail.Rows[kount - 1].Cells["Item"].Value = "";
                grdSMRNHDetail.Rows[kount - 1].Cells["ModelNo"].Value = "";
                grdSMRNHDetail.Rows[kount - 1].Cells["ModelName"].Value = "";
                grdSMRNHDetail.Rows[kount - 1].Cells["SerialNo"].Value = "";
                grdSMRNHDetail.Rows[kount - 1].Cells["ItemDetails"].Value = " ";
                grdSMRNHDetail.Rows[kount - 1].Cells["WarrantyStatus"].Value = "";
                grdSMRNHDetail.Rows[kount - 1].Cells["ServiceApprovalStatus"].Value = "";
                grdSMRNHDetail.Rows[kount - 1].Cells["InspectionStatus"].Value = 0;
                grdSMRNHDetail.Rows[kount - 1].Cells["InspectionRemarks"].Value = "";
                grdSMRNHDetail.Rows[kount - 1].Cells["ServiceStatus"].Value = 0;
                grdSMRNHDetail.Rows[kount - 1].Cells["ServiceRemarks"].Value = " ";
                grdSMRNHDetail.Rows[kount - 1].Cells["JobIDNo"].Value = kount;
                grdSMRNHDetail.Rows[kount - 1].Cells["ProductServiceStatus"].Value = "";
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdSMRNHDetail.Rows[kount - 1].Cells["ProductServiceStatus"]);
                CatalogueValueDB.fillCatalogValueGridViewComboNew(ComboColumn2, "ProductServiceStatus");
                ComboColumn2.DropDownWidth = 300;

                grdSMRNHDetail.Rows[kount - 1].Cells["ServiceRemarks"].ReadOnly = true;
                grdSMRNHDetail.Rows[kount - 1].Cells["InspectionRemarks"].ReadOnly = true;
                grdSMRNHDetail.Rows[kount - 1].Cells["ProductServiceStatus"].ReadOnly = true;
                //grdQIDetail.Columns["Item"].ReadOnly = false;
                //grdQIDetail.Columns["ItemDetails"].ReadOnly = false;
                //grdQIDetail.Columns["WarrantyStatus"].ReadOnly = false;
                //grdQIDetail.Columns["Delete"].ReadOnly = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddQIDetailRow() : Error");
            }

            return status;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }

        private Boolean verifyAndReworkSMRNDetailGridRows()
        {
            Boolean status = true;

            try
            {
                //double quantity = 0;
                //double price = 0;
                //double cost = 0.0;
                //productvalue = 0.0;
                //taxvalue = 0.0;
                //string strtaxCode = "";

                if (grdSMRNHDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in SMRN details");
                    return false;
                }
                //clear tax details table
                TaxDetailsTable.Rows.Clear();
                for (int i = 0; i < grdSMRNHDetail.Rows.Count; i++)
                {
                    //int i = 0;
                    grdSMRNHDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if (((grdSMRNHDetail.Rows[i].Cells["Item"].Value == null) ||
                        (grdSMRNHDetail.Rows[i].Cells["ModelNo"].Value == null) ||
                        (grdSMRNHDetail.Rows[i].Cells["ModelName"].Value == null) ||
                        (grdSMRNHDetail.Rows[i].Cells["SerialNo"].Value == null)) ||
                        (grdSMRNHDetail.Rows[i].Cells["ItemDetails"].Value == null) ||
                        (grdSMRNHDetail.Rows[i].Cells["WarrantyStatus"].Value == null) ||
                         (grdSMRNHDetail.Rows[i].Cells["SerialNo"].Value.ToString().Length == 0) ||
                        (grdSMRNHDetail.Rows[i].Cells["ItemDetails"].Value.ToString().Length == 0) ||
                        (grdSMRNHDetail.Rows[i].Cells["WarrantyStatus"].Value.ToString().Length == 0))
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
                for (int i = 0; i < grdSMRNHDetail.Rows.Count - 1; i++)
                {
                    for (int j = i + 1; j < grdSMRNHDetail.Rows.Count; j++)
                    {

                        if (grdSMRNHDetail.Rows[i].Cells[1].Value.ToString() == grdSMRNHDetail.Rows[j].Cells["Item"].Value.ToString() &&
                            grdSMRNHDetail.Rows[i].Cells["ModelNo"].Value.ToString() == grdSMRNHDetail.Rows[j].Cells["ModelNo"].Value.ToString())
                        {
                            //duplicate item code
                            MessageBox.Show("Item code duplicated in OB details... please ensure correctness (" +
                                grdSMRNHDetail.Rows[i].Cells["Item"].Value.ToString() + ")");
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

        private List<smrndetail> getSMRNDetails(smrnheader smrnh)
        {
            List<smrndetail> SMRNDetails = new List<smrndetail>();
            try
            {
                smrndetail smrnd = new smrndetail();
                for (int i = 0; i < grdSMRNHDetail.Rows.Count; i++)
                {
                    smrnd = new smrndetail();
                    smrnd.DocumentID = smrnh.DocumentID;
                    smrnd.TemporaryNo = smrnh.TemporaryNo;
                    smrnd.TemporaryDate = smrnh.TemporaryDate;
                    smrnd.StockItemID = grdSMRNHDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Substring(0, grdSMRNHDetail.Rows[i].Cells["Item"].Value.ToString().Trim().IndexOf('-'));
                    smrnd.ModelNo = grdSMRNHDetail.Rows[i].Cells["ModelNo"].Value.ToString().Trim();
                    smrnd.SerialNo = grdSMRNHDetail.Rows[i].Cells["SerialNo"].Value.ToString().Trim();
                    smrnd.ItemDetails = grdSMRNHDetail.Rows[i].Cells["ItemDetails"].Value.ToString().Trim();
                    smrnd.WarrantyStatus = returnStatus(grdSMRNHDetail.Rows[i].Cells["WarrantyStatus"].Value.ToString());
                    smrnd.ServiceApprovalStatus = returnStatus(grdSMRNHDetail.Rows[i].Cells["ServiceApprovalStatus"].Value.ToString());
                    smrnd.InspectionStatus = Convert.ToInt32(grdSMRNHDetail.Rows[i].Cells["InspectionStatus"].Value);
                    smrnd.InspectionRemarks = grdSMRNHDetail.Rows[i].Cells["InspectionRemarks"].Value.ToString();
                    smrnd.ServiceStatus = Convert.ToInt32(grdSMRNHDetail.Rows[i].Cells["ServiceStatus"].Value.ToString());
                    smrnd.ServiceRemarks = grdSMRNHDetail.Rows[i].Cells["ServiceRemarks"].Value.ToString();
                    smrnd.JobIDNo = Convert.ToInt32(grdSMRNHDetail.Rows[i].Cells["JobIDNo"].Value.ToString());
                    smrnd.ProductServiceStatus = grdSMRNHDetail.Rows[i].Cells["ProductServiceStatus"].Value.ToString();
                    SMRNDetails.Add(smrnd);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("getSMRNDetails() : Error getting SMRN Details");
                SMRNDetails = null;
            }
            return SMRNDetails;
        }
        public int returnStatus(string status)
        {
            if (status.Equals("Yes"))
                return 1;
            else if (status.Equals("No"))
                return 0;
            else
                return 0;
        }
        public string showStatus(int status)
        {
            if (status == 1)
                return "Yes";
            else if (status == 0)
                return "No";
            else
                return "NA";
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
            ListFilteredSMRNHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredSMRNHeader(listOption);
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

            ListFilteredSMRNHeader(listOption);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            Boolean status = true;
            try
            {

                SMRNHeaderDB smrnDB = new SMRNHeaderDB();
                smrnheader smrnh = new smrnheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    if (!verifyAndReworkSMRNDetailGridRows())
                    {
                        return;
                    }
                    smrnh.DocumentID = docID;
                    smrnh.DocumentDate = dtDocumentDate.Value;
                    smrnh.SMRNNo = Convert.ToInt32(txtSMRNNo.Text);
                    smrnh.SMRNDate = dtSMRNDate.Value;
                    smrnh.Comments = docCmtrDB.DGVtoString(dgvComments);
                    smrnh.ForwarderList = prevsmrnh.ForwarderList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    //smrnh.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    smrnh.DocumentStatus = 1; //created
                    smrnh.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    smrnh.TemporaryNo = Convert.ToInt32(txtTemporaryNo.Text);
                    smrnh.TemporaryDate = prevsmrnh.TemporaryDate;
                }

                if (smrnDB.validateSMRNHeader(smrnh))
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
                            smrnh.CommentStatus = docCmtrDB.createCommentStatusString(prevsmrnh.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            smrnh.CommentStatus = prevsmrnh.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            smrnh.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            smrnh.CommentStatus = prevsmrnh.CommentStatus;
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
                        smrnh.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus);
                    }
                    List<smrndetail> SMRNDetails = getSMRNDetails(smrnh);
                    if (btnText.Equals("Update"))
                    {
                        if (smrnDB.updateSMRNHeaderAndDetail(smrnh, prevsmrnh, SMRNDetails))
                        {
                            MessageBox.Show("SMRN Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredSMRNHeader(listOption);
                        }
                        else
                        {
                            status = false;

                        }
                        if (!status)
                        {
                            status = false;
                            MessageBox.Show("Failed to update SMRN Details");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (smrnDB.InsertSMRNHeaderAndDetail(smrnh, SMRNDetails))
                        {
                            MessageBox.Show("SMRN Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredSMRNHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            status = false;
                            MessageBox.Show("Failed to Insert SMRN Details");
                        }
                    }
                }
                else
                {
                    status = false;
                    MessageBox.Show("SMRN Details Validation failed");
                }
            }
            catch (Exception ex)
            {
                status = false;
                MessageBox.Show("SMRN Details: Error");
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
                    SMRNHeaderDB smrnDB = new SMRNHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevsmrnh = new smrnheader();
                    prevsmrnh.PreCheckStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["PreCheckstatus"].Value.ToString());
                    if (prevsmrnh.PreCheckStatus == 1)
                    {
                        btnFinalize.Visible = false;
                        grdSMRNHDetail.Columns["Delete"].Visible = false;
                        setDetailGridData(2);
                    }
                    else
                    {
                        btnFinalize.Visible = true;
                        grdSMRNHDetail.Columns["Delete"].Visible = true;
                        setDetailGridData(1);
                    }
                    prevsmrnh.CommentStatus = grdList.Rows[e.RowIndex].Cells["CommetnStatus"].Value.ToString();
                    prevsmrnh.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    prevsmrnh.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    prevsmrnh.CustomerID = grdList.Rows[e.RowIndex].Cells["gCustomerID"].Value.ToString();
                    prevsmrnh.CustomerName = grdList.Rows[e.RowIndex].Cells["gCustomerName"].Value.ToString();
                    prevsmrnh.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    prevsmrnh.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    prevsmrnh.DocumentNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentNo"].Value.ToString());
                    prevsmrnh.DocumentDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["DocumentDate"].Value.ToString());
                    if (prevsmrnh.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    prevsmrnh.Comments = SMRNHeaderDB.getUserComments(prevsmrnh.DocumentID, prevsmrnh.TemporaryNo, prevsmrnh.TemporaryDate);

                    prevsmrnh.SMRNNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["SMRNNo"].Value.ToString());
                    prevsmrnh.SMRNDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["SMRNDate"].Value.ToString());

                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Temporary No:" + prevsmrnh.TemporaryNo + "\n" +
                            "Temporary Date:" + prevsmrnh.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "Document No:" + prevsmrnh.DocumentNo + "\n" +
                            "Document Date:" + prevsmrnh.DocumentDate.ToString("dd-MM-yyyy") + "\n" +
                            "Customer:" + prevsmrnh.CustomerName;
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevsmrnh.DocumentNo + "-" + prevsmrnh.DocumentDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_2(null, null);
                        return;
                    }
                    //--------
                    prevsmrnh.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    prevsmrnh.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    prevsmrnh.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    prevsmrnh.ForwardUser = grdList.Rows[e.RowIndex].Cells["ForwardUser"].Value.ToString();
                    prevsmrnh.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    prevsmrnh.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentStatusNo"].Value.ToString());
                    prevsmrnh.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevsmrnh.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    prevsmrnh.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    prevsmrnh.ApproveUser = grdList.Rows[e.RowIndex].Cells["ApproveUser"].Value.ToString();
                    //--comments
                    chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlCommetns.Controls.Remove(dgvComments);
                    prevsmrnh.CommentStatus = grdList.Rows[e.RowIndex].Cells["CommetnStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(prevsmrnh.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(prevsmrnh.Comments);
                    pnlCommetns.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---
                    txtDocumentNo.Text = prevsmrnh.DocumentNo.ToString();
                    txtTemporaryNo.Text = prevsmrnh.TemporaryNo.ToString();
                    dtTemporaryDate.Value = prevsmrnh.TemporaryDate;
                    try
                    {
                        dtDocumentDate.Value = prevsmrnh.DocumentDate;
                    }
                    catch (Exception)
                    {

                        dtDocumentDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtSMRNNo.Text = prevsmrnh.SMRNNo.ToString();
                    try
                    {
                        dtSMRNDate.Value = prevsmrnh.SMRNDate;
                    }
                    catch (Exception)
                    {
                        dtSMRNDate.Value = DateTime.Parse("01-01-1900");
                    }
                    List<smrndetail> SMRNDetail = SMRNHeaderDB.getSMRNDetail(prevsmrnh);
                    grdSMRNHDetail.Rows.Clear();
                    int i = 0;
                    foreach (smrndetail smrnd in SMRNDetail)
                    {
                        if (!AddSMRNDetailRow())
                        {
                            MessageBox.Show("Error found in QI details. Please correct before updating the details");
                        }
                        else
                        {
                            ////if (prevsmrnh.PreCheckStatus == 1)
                            ////{
                            ////    disableDetailGridRows();
                            ////    var cell = grdQIDetail[grdQIDetail.Columns["Delete"].Index, i] = new DataGridViewTextBoxCell();

                            ////    cell.ReadOnly = false;
                            ////}
                            grdSMRNHDetail.Rows[i].Cells["Item"].Value = smrnd.StockItemID + "-" + smrnd.StockItemName;
                            grdSMRNHDetail.Rows[i].Cells["SerialNo"].Value = smrnd.SerialNo;
                            grdSMRNHDetail.Rows[i].Cells["ModelNo"].Value = smrnd.ModelNo;
                            grdSMRNHDetail.Rows[i].Cells["ModelName"].Value = smrnd.ModelName;
                            grdSMRNHDetail.Rows[i].Cells["ItemDetails"].Value = smrnd.ItemDetails;
                            grdSMRNHDetail.Rows[i].Cells["WarrantyStatus"].Value = showStatus(smrnd.WarrantyStatus);

                            grdSMRNHDetail.Rows[i].Cells["ServiceApprovalStatus"].Value = showStatus(smrnd.ServiceApprovalStatus);
                            grdSMRNHDetail.Rows[i].Cells["InspectionStatus"].Value = smrnd.InspectionStatus;
                            ////if (prevsmrnh.ForwardUser == Login.userLoggedIn)
                            {

                                if (Convert.ToInt32(grdSMRNHDetail.Rows[i].Cells["InspectionStatus"].Value.ToString()) == 1)
                                {
                                    if (!(Convert.ToInt32(prevsmrnh.Status) == 1 && Convert.ToInt32(prevsmrnh.DocumentStatus) == 99))
                                    {
                                        //document not approved
                                        grdSMRNHDetail.Columns["ServiceApprovalStatus"].ReadOnly = false;
                                    }
                                }
                                else
                                {
                                    grdSMRNHDetail.Columns["ServiceApprovalStatus"].ReadOnly = true;
                                }
                            }

                            if (smrnd.InspectionStatus == 1)
                            {
                                grdSMRNHDetail.Rows[i].Cells["InspectionRemarks"].ReadOnly = false;
                            }
                            grdSMRNHDetail.Rows[i].Cells["InspectionRemarks"].Value = smrnd.InspectionRemarks;
                            grdSMRNHDetail.Rows[i].Cells["ServiceStatus"].Value = smrnd.ServiceStatus;
                            if (smrnd.ServiceStatus == 1)
                            {
                                grdSMRNHDetail.Rows[i].Cells["ServiceRemarks"].ReadOnly = false;
                            }
                            grdSMRNHDetail.Rows[i].Cells["ServiceRemarks"].Value = smrnd.ServiceRemarks;
                            grdSMRNHDetail.Rows[i].Cells["JobIDNo"].Value = smrnd.JobIDNo;
                            if (smrnd.ProductServiceStatus.Length == 0)
                            {
                                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdSMRNHDetail.Rows[i].Cells["ProductServiceStatus"]);
                                ComboColumn2.ValueType = typeof(String);
                            }
                            else
                                grdSMRNHDetail.Rows[i].Cells["ProductServiceStatus"].Value = smrnd.ProductServiceStatus;
                            if (prevsmrnh.DocumentStatus == 99 && prevsmrnh.Status == 1 && smrnd.ServiceApprovalStatus == 1)
                            {
                                grdSMRNHDetail.Rows[i].Cells["ProductServiceStatus"].ReadOnly = false;
                            }
                            i++;
                        }

                    }
                    if (!verifyAndReworkSMRNDetailGridRows())
                    {
                        MessageBox.Show("Error found in SMRN details. Please correct before updating the details");
                    }

                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;

                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                    tabControl1.SelectedTab = tabSMRNHeader;
                    tabControl1.Visible = true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void disableDetailGridRows()
        {

            btnAddNewLine.Enabled = false;
            btnClearEntries.Enabled = false;
            tabControl1.TabPages["tabSMRNHeader"].Enabled = false;
            grdSMRNHDetail.Columns["SerialNo"].ReadOnly = true;
            grdSMRNHDetail.Columns["Item"].ReadOnly = true;
            grdSMRNHDetail.Columns["ItemDetails"].ReadOnly = true;
            grdSMRNHDetail.Columns["WarrantyStatus"].ReadOnly = true;


        }
        private void enableDetailGridRows()
        {
            btnAddNewLine.Enabled = true;
            btnClearEntries.Enabled = true;
            tabControl1.TabPages["tabSMRNHeader"].Enabled = true;
            grdSMRNHDetail.Columns["SerialNo"].ReadOnly = false;
            grdSMRNHDetail.Columns["Item"].ReadOnly = false;
            grdSMRNHDetail.Columns["ItemDetails"].ReadOnly = false;
            grdSMRNHDetail.Columns["WarrantyStatus"].ReadOnly = false;
            //var cell = grdQIDetail[grdQIDetail.Columns["Delete"].Index, i] = new DataGridViewTextBoxCell();
            //cell.ReadOnly = true;
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
            AddSMRNDetailRow();
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            verifyAndReworkSMRNDetailGridRows();
        }

        private void ClearEntries_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = grdSMRNHDetail.Rows.Count - 1; i >= 0; i--)
                {
                    grdSMRNHDetail.Rows.RemoveAt(i);
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
                string columnName = grdSMRNHDetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        /// if(prevsmrnh.)
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdSMRNHDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkSMRNDetailGridRows();
                    }
                    if (columnName.Equals("InpectionReportLink"))
                    {
                        string smrndetail = grdSMRNHDetail.CurrentRow.Cells["Item"].Value.ToString() + ";" + grdSMRNHDetail.CurrentRow.Cells["InspectionRemarks"].Value.ToString();
                        int jobNo = Convert.ToInt32(grdSMRNHDetail.CurrentRow.Cells["JobIDNo"].Value.ToString());
                        PrintSMRNReports print = new PrintSMRNReports();
                        List<productservicereportdetail> psrdetail = SMRNHeaderDB.getPSRDetailForReport(prevsmrnh, 1, jobNo);
                        //string fname = "\\\\DEV5\\Users\\Public\\Prakash\\CSLERPDocuments\\SMRNHEADER\\0-19000101120000\\PURCHASEORDER-14.pdf.pdf";
                        string fname = "";
                        if (psrdetail.Count != 0)
                        {
                            fname = print.PrintReport(psrdetail, smrndetail);
                        }
                        else
                        {
                            MessageBox.Show("report not Prepared");
                            return;
                        }
                        FileManager.PDFViewer pdf = new FileManager.PDFViewer();
                        pdf.showFile(fname);
                        pdf.ShowDialog();
                        this.RemoveOwnedForm(pdf);
                        //btnCancel_Click_2(null, null);
                        return;
                        //showPDFFiles(fname);
                    }
                    if (columnName.Equals("FinalReportLink"))
                    {
                        string smrndetail = grdSMRNHDetail.CurrentRow.Cells["Item"].Value.ToString() + ";" + grdSMRNHDetail.CurrentRow.Cells["ServiceRemarks"].Value.ToString();
                        int jobNo = Convert.ToInt32(grdSMRNHDetail.CurrentRow.Cells["JobIDNo"].Value.ToString());
                        PrintSMRNReports print = new PrintSMRNReports();
                        List<productservicereportdetail> psrdetail = SMRNHeaderDB.getPSRDetailForReport(prevsmrnh, 2, jobNo);
                        //string fname = "\\\\DEV5\\Users\\Public\\Prakash\\CSLERPDocuments\\SMRNHEADER\\0-19000101120000\\PURCHASEORDER-14.pdf.pdf";
                        string fname = "";
                        if (psrdetail.Count != 0)
                        {
                            fname = print.PrintReport(psrdetail, smrndetail);
                        }
                        else
                        {
                            MessageBox.Show("report not Prepared");
                            return;
                        }
                        FileManager.PDFViewer pdf = new FileManager.PDFViewer();
                        pdf.showFile(fname);
                        pdf.ShowDialog();
                        this.RemoveOwnedForm(pdf);
                        //btnCancel_Click_2(null, null);
                        return;
                        //showPDFFiles(fname);

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

        private Boolean checkAvailabilityOfitem()
        {
            Boolean status = true;
            if (grdSMRNHDetail.CurrentRow.Cells["Item"].Value.ToString().Length != 0 ||
                grdSMRNHDetail.CurrentRow.Cells["ModelNo"].Value.ToString().Length != 0 ||
                grdSMRNHDetail.CurrentRow.Cells["ModelName"].Value.ToString().Length != 0)
            {
                status = false;
            }
            return status;
        }
        private void showStockItemTreeView()
        {
            removeControlsFromForwarderPanelTV();
            if (!checkAvailabilityOfitem())
            {
                DialogResult dialog = MessageBox.Show("Selected product and Model detail will removed?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdSMRNHDetail.CurrentRow.Cells["Item"].Value = "";
                    grdSMRNHDetail.CurrentRow.Cells["ModelNo"].Value = "";
                    grdSMRNHDetail.CurrentRow.Cells["ModelName"].Value = "";
                }
                else
                    return;
            }
            tv = new TreeView();
            tv.CheckBoxes = true;
            tv.Nodes.Clear();
            tv.CheckBoxes = true;
            pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
            pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Location = new Point(70, 17);
            lbl.Size = new Size(35, 13);
            lbl.Text = "Tree View For Product";
            lbl.Font = new Font("Serif", 10, FontStyle.Bold);
            lbl.ForeColor = Color.Green;
            pnlForwarder.Controls.Add(lbl);
            tv = StockItemDB.getStockItemTreeView();
            tv.Bounds = new Rectangle(new Point(50, 50), new Size(600, 200));
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
                    grdSMRNHDetail.CurrentRow.Cells["Item"].Value = s;
                    tv.CheckBoxes = true;
                    pnlForwarder.Controls.Remove(tv);
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
        private void removeControlsFromForwarderPanelTV()
        {
            try
            {
                //foreach (Control p in pnlForwarder.Controls)
                //    if (p.GetType() == typeof(TreeView) || p.GetType() == typeof(Button) || p.GetType() == typeof(ListView))
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
            lbl.ForeColor = Color.Black;
            frmPopup.Controls.Add(lbl);
            lv = ProductModelsDB.getModelsForProductListView(stockID.Substring(0, stockID.IndexOf('-')));
            if (lv.Items.Count == 0)
            {
                grdSMRNHDetail.CurrentRow.Cells["ModelNo"].Value = "NA";
                grdSMRNHDetail.CurrentRow.Cells["ModelName"].Value = "NA";
                pnlModel.Visible = false;
                return;
            }
            lv.Bounds = new Rectangle(new Point(0, 25), new Size(450, 250));
            //pnlModel.Controls.Remove(lv);
            frmPopup.Controls.Add(lv);
            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.BackColor = Color.Tan;
            lvOK.Location = new System.Drawing.Point(40, 280);
            lvOK.Click += new System.EventHandler(this.lvOK_Click4);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new System.Drawing.Point(130, 280);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click4);
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
        private void lvOK_Click4(object sender, EventArgs e)
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
                        grdSMRNHDetail.CurrentRow.Cells["ModelNo"].Value = item.SubItems[1].Text;
                        grdSMRNHDetail.CurrentRow.Cells["ModelName"].Value = item.SubItems[2].Text;
                        frmPopup.Close();
                        frmPopup.Dispose();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void lvCancel_Click4(object sender, EventArgs e)
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
        //-----
        private void showPDFFiles(string fname)
        {
            try
            {
                AxAcroPDFLib.AxAcroPDF pdf = new AxAcroPDFLib.AxAcroPDF();
                //pdf.Dock = System.Windows.Forms.DockStyle.Fill;
                pdf.Enabled = true;
                pdf.Size = new System.Drawing.Size(636, 274);
                pdf.Location = new System.Drawing.Point(0, 0);
                pdf.Name = "pdfReader";
                pdf.OcxState = pdf.OcxState;
                ////pdf.OcxState = ((System.Windows.Forms.AxHost.State)(new System.ComponentModel.ComponentResourceManager(typeof(ViewerWindow)).GetObject("pdf.OcxState")));
                pdf.TabIndex = 1;
                pnlAddEdit.Controls.Add(pdf);
                pdf.BringToFront();
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
        //private void btnTaxDetail_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string strTax = "";
        //        for (int i = 0; i < (TaxDetailsTable.Rows.Count); i++)
        //        {
        //            strTax = strTax + Convert.ToString(TaxDetailsTable.Rows[i][0]) + "-" +
        //            Convert.ToString(TaxDetailsTable.Rows[i][1]) + "\n";
        //        }
        //        DialogResult dialog = MessageBox.Show(strTax, "Tax Details", MessageBoxButtons.OK);
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Error showing tax details");
        //    }
        //}

        private void pnlAddEdit_Paint(object sender, PaintEventArgs e)
        {

        }

        private Boolean checkinspectionStatus()
        {
            Boolean status = true;
            foreach (DataGridViewRow row in grdSMRNHDetail.Rows)
            {
                int n = grdSMRNHDetail.Rows[row.Index].Cells["InspectionRemarks"].Value.ToString().Length;
                if ((Convert.ToInt32(grdSMRNHDetail.Rows[row.Index].Cells["InspectionStatus"].Value) != 1) ||
                    (grdSMRNHDetail.Rows[row.Index].Cells["InspectionRemarks"].Value.ToString().Length == 0))
                {
                    status = false;
                }
            }
            return status;
        }
        private void btnForward_Click(object sender, EventArgs e)
        {
            if (!checkinspectionStatus())
            {
                MessageBox.Show("Priliminary Report not Prepared\nor check InspectionStatus and remark.you are not Allowed to Forward");
                return;
            }
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
                            SMRNHeaderDB smrnDB = new SMRNHeaderDB();
                            prevsmrnh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevsmrnh.CommentStatus);
                            prevsmrnh.ForwardUser = approverUID;
                            prevsmrnh.ForwarderList = prevsmrnh.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (smrnDB.forwardSMRN(prevsmrnh))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(prevsmrnh, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredSMRNHeader(listOption);
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
        private void btnFinalize_Click(object sender, EventArgs e)
        {
            SMRNHeaderDB smrnDB = new SMRNHeaderDB();
            if (prevsmrnh.PreCheckStatus == 1)
            {
                MessageBox.Show("SMRNHeader is already Finalized");
                return;
            }
            DialogResult dialog = MessageBox.Show("Are you sure to Finalize the document ?", "Yes", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                prevsmrnh.PreCheckStatus = 1;
                if (smrnDB.finalizeSMRN(prevsmrnh))
                {
                    MessageBox.Show("Document Finalized");
                    closeAllPanels();
                    listOption = 1;
                    ListFilteredSMRNHeader(listOption);
                    setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                }

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
                if (!checkinspectionStatus())
                {
                    MessageBox.Show("Priliminary Report not Prepared.you are not Allowed to Forward");
                    return;
                }
                btnApprove.Enabled = false;
                removeControlsFrompnllvPanel();
                pnllv = new Panel();
                pnllv.BorderStyle = BorderStyle.FixedSingle;

                pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
                lv = POPIHeaderDB.TrackingSelectionView("POSERVICEINWARD");
                //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView2_ItemChecked2);
                lv.Bounds = new Rectangle(new Point(50, 50), new Size(500, 200));
                pnllv.Controls.Add(lv);

                Button lvOK = new Button();
                lvOK.Text = "OK";
                lvOK.Location = new Point(50, 270);
                lvOK.Click += new System.EventHandler(this.lvOK_Click2);
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
            catch (Exception)
            {
            }
        }
        private void lvOK_Click2(object sender, EventArgs e)
        {
            try
            {
                // int count = 0;
                if (lv.CheckedIndices.Count == 0)
                {
                    MessageBox.Show("Not  allowed to approve without Tracking No and Date");
                    return;
                }
                if (lv.CheckedIndices.Count > 1)
                {
                    MessageBox.Show("not allowed to select more than one item");
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        //count++;
                        prevsmrnh.TrackingNo = Convert.ToInt32(itemRow.SubItems[1].Text);
                        prevsmrnh.TrackingDate = Convert.ToDateTime(itemRow.SubItems[2].Text);
                    }
                }
                //if(count == 0)
                //{
                //    MessageBox.Show("Not  allowed to approve without Tracking No and Date");
                //    return;
                //}
                SMRNHeaderDB smrnDB = new SMRNHeaderDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevsmrnh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevsmrnh.CommentStatus);
                    prevsmrnh.DocumentNo = DocumentNumberDB.getNewNumber(docID, 2);
                    if (smrnDB.ApproveSMRNH(prevsmrnh))
                    {
                        MessageBox.Show("SMRN Document Approved");
                        if (!updateDashBoard(prevsmrnh, 2))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                        }
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredSMRNHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
                btnApprove.Enabled = true;
                pnllv.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }
        private Boolean updateDashBoard(smrnheader smrnh, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = smrnh.DocumentID;
                dsb.TemporaryNo = smrnh.TemporaryNo;
                dsb.TemporaryDate = smrnh.TemporaryDate;
                dsb.DocumentNo = smrnh.DocumentNo;
                dsb.DocumentDate = smrnh.DocumentDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = smrnh.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(prevsmrnh.DocumentID);
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
        private void lvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                btnApprove.Enabled = true;
                pnllv.Visible = false;
            }
            catch (Exception)
            {
            }
        }
        //private void listView2_ItemChecked2(object sender, ItemCheckedEventArgs e)
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
                btnFinalize.Visible = false;
                btnForward.Visible = false;
                btnApprove.Visible = false;
                btnReverse.Visible = false;
                btnGetComments.Visible = false;
                chkCommentStatus.Visible = false;
                txtComments.Visible = false;
                disableTabPages();
                clearTabPageControls();
                enableDetailGridRows();
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
                    tabControl1.SelectedTab = tabSMRNHeader;
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
                    tabControl1.SelectedTab = tabSMRNHeader;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnFinalize.Visible = true;
                    btnReverse.Visible = true;
                    disableTabPages();
                    tabControl1.SelectedTab = tabSMRNHeader;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabSMRNHeader;
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
                dgvComments.Rows.Clear();
                chkCommentStatus.Checked = false;
                txtComments.Text = "";
                grdSMRNHDetail.Rows.Clear();
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
                    string reverseStr = getReverseString(prevsmrnh.ForwarderList);
                    //do forward activities
                    prevsmrnh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevsmrnh.CommentStatus);
                    SMRNHeaderDB smrnDB = new SMRNHeaderDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevsmrnh.ForwarderList = reverseStr.Substring(0, ind);
                        prevsmrnh.ForwardUser = reverseStr.Substring(ind + 3);
                        prevsmrnh.DocumentStatus = prevsmrnh.DocumentStatus - 1;
                    }
                    else
                    {
                        prevsmrnh.ForwarderList = "";
                        prevsmrnh.ForwardUser = "";
                        prevsmrnh.DocumentStatus = 1;
                    }
                    if (smrnDB.reverseSMRNH(prevsmrnh))
                    {
                        MessageBox.Show("SMRN Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredSMRNHeader(listOption);
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
            //removeControlsFromCommenterPanel();
            //docCmtrDB = new DocCommenterDB();
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
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevsmrnh.DocumentNo + "-" + prevsmrnh.DocumentDate.ToString("yyyyMMddhhmmss"));
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
        private void removeControlsFromPaymentTermsPanel()
        {
            try
            {
                foreach (Control p in pnlForwarder.Controls)
                    if (p.GetType() == typeof(Button))
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
                string fileName = "";
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == 0)
                {
                    removePDFControls();
                    fileName = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    ////string docDir = Main.documentDirectory + "\\" + docID;
                    string subDir = prevsmrnh.DocumentNo + "-" + prevsmrnh.DocumentDate.ToString("yyyyMMddhhmmss");
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
        private void removeControlsFrompnllvPanel()
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
        private void btnSelectSMRNNo_Click(object sender, EventArgs e)
        {
            //btnSelectSMRNNo.Enabled = false;
            //removeControlsFrompnllvPanel();
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
            lv = SMRNDB.getSMRNListView();
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView2_ItemChecked);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
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
                if (!checkLVItemChecked("SMRN"))
                {
                    return;
                }
                string trlist;
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        txtSMRNNo.Text = itemRow.SubItems[1].Text;
                        dtSMRNDate.Value = Convert.ToDateTime(itemRow.SubItems[2].Text);
                    }
                }
                frmPopup.Close();
                frmPopup.Dispose();
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
        //private void listView2_ItemChecked(object sender, ItemCheckedEventArgs e)
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
        private void setDetailGridData(int opt)
        {
            //opt==1. New button
            try
            {
                if (opt == 1)
                {
                    grdSMRNHDetail.Columns["InspectionStatus"].ReadOnly = true;
                    //grdQIDetail.Columns["InspectionReportLink"].ReadOnly = true;
                    grdSMRNHDetail.Columns["ServiceApprovalStatus"].ReadOnly = true;
                    grdSMRNHDetail.Columns["ServiceStatus"].ReadOnly = true;
                    grdSMRNHDetail.Columns["ServiceRemarks"].ReadOnly = true;
                    grdSMRNHDetail.Columns["InspectionStatus"].ReadOnly = true;
                    grdSMRNHDetail.Columns["FinalReportLink"].ReadOnly = true;
                    grdSMRNHDetail.Columns["JobIDNo"].ReadOnly = true;
                    grdSMRNHDetail.Columns["ProductServiceStatus"].ReadOnly = true;
                }

                if (opt == 2)
                {
                    grdSMRNHDetail.Columns["LineNo"].ReadOnly = true;
                    grdSMRNHDetail.Columns["Item"].ReadOnly = true;
                    grdSMRNHDetail.Columns["SerialNo"].ReadOnly = true;
                    grdSMRNHDetail.Columns["ItemDetails"].ReadOnly = true;
                    grdSMRNHDetail.Columns["WarrantyStatus"].ReadOnly = true;

                    grdSMRNHDetail.Columns["InspectionStatus"].ReadOnly = true;
                    //grdQIDetail.Columns["InspectionReportLink"].ReadOnly = true;
                    grdSMRNHDetail.Columns["ServiceApprovalStatus"].ReadOnly = true;
                    grdSMRNHDetail.Columns["ServiceStatus"].ReadOnly = true;
                    grdSMRNHDetail.Columns["ServiceRemarks"].ReadOnly = true;
                    grdSMRNHDetail.Columns["InspectionStatus"].ReadOnly = true;
                    grdSMRNHDetail.Columns["FinalReportLink"].ReadOnly = true;
                    grdSMRNHDetail.Columns["JobIDNo"].ReadOnly = true;
                    grdSMRNHDetail.Columns["ProductServiceStatus"].ReadOnly = true;

                }
            }
            catch (Exception)
            {

                throw;
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
            removeControlsFromForwarderPanelTV();
        }

        private void SMRNHeader_Click(object sender, EventArgs e)
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

