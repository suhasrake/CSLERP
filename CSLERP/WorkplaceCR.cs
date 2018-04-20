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
    public partial class WorkplaceCR : System.Windows.Forms.Form
    {
        string docID = "";
        string forwarderList = "";
        string approverList = "";
        TextBox txtSearch = new TextBox();
        string userString = "";
        RichTextBox txt = new RichTextBox();
        ListView lvCopy = new ListView();
        Panel pnllv = new Panel();
        ListView lv = new ListView();
        string roles = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        workplacecr prevwp;
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        Panel pnlForwarder = new Panel();
        ListView lvApprover = new ListView();
        Panel pnlCmtr = new Panel();
        Form frmPopup = new Form();
        public WorkplaceCR()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }

        private void SMRN_Load(object sender, EventArgs e)
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
            roles = getEmplRoleStr();
            ListFilteredWCR(listOption);

            //applyPrivilege();
        }
        private void ListFilteredWCR(int option)
        {
            try
            {

                grdList.Rows.Clear();
                WorkplaceCRDB crdb = new WorkplaceCRDB();
                List<workplacecr> CRList = new List<workplacecr>();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);

                if (roles.Equals("MO") || roles.Equals("MM"))
                {
                    CRList = crdb.getFilteredComplaintsForMO(option);
                }
                else
                {
                    CRList = crdb.getFilteredComplaintsForUser(option);
                }
                if (option == 1)
                {
                    lblActionHeader.Text = "Complaints registered";
                    btnComplaintReg.Focus();
                }
                if (option == 2)
                {
                    lblActionHeader.Text = "Complaints accepted by the Maintenance Officer";
                    btnCmplntAccepted.Focus();
                }
                else if (option == 3)
                {
                    lblActionHeader.Text = "Complaint history";
                    btnCmplntStatus.Focus();
                }

                foreach (workplacecr cr in CRList)
                {
                    if (option == 1)
                    {
                        if (cr.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["RowID"].Value = cr.rowID;
                    grdList.Rows[grdList.RowCount - 1].Cells["EmpId"].Value = cr.EmployeeID;
                    grdList.Rows[grdList.RowCount - 1].Cells["EmployeeName"].Value = cr.EmployeeName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ComplaintType"].Value = cr.ComplaintType;
                    grdList.Rows[grdList.RowCount - 1].Cells["ComplaintDesc"].Value = cr.ComplaintDescription;
                    grdList.Rows[grdList.RowCount - 1].Cells["AcceptTime"].Value = cr.AcceptTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["CloseTime"].Value = cr.CloseTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["Remarks"].Value = cr.Remarks;

                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatus"].Value = cr.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = valuetostring(cr.Status, cr.DocumentStatus);
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = cr.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = cr.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = cr.Creator;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in WorkPlaceCR Header listing");
            }
            setButtonVisibility("init");
            if (listOption == 2 || listOption == 3)
            {
                grdList.Columns["View"].Visible = true;
                grdList.Columns["Edit"].Visible = false;
            }
            else
            {
                grdList.Columns["View"].Visible = false;
                grdList.Columns["Edit"].Visible = true;
            }
            if (roles.Contains("MM") || roles.Contains("MO"))
            {
                if (listOption == 2)
                {
                    grdList.Columns["View"].Visible = false;
                    grdList.Columns["Edit"].Visible = true;
                }
            }

            if (roles.Contains("MO") && listOption == 1)
            {
                grdList.Columns["Edit"].HeaderText = "Accept";
                foreach (DataGridViewRow row in grdList.Rows)
                {
                    DataGridViewButtonColumn col = (DataGridViewButtonColumn)grdList.Columns["Edit"];
                    col.Text = "Accept";
                }
            }
            else if (roles.Contains("MM") && (listOption == 1 || listOption == 2))
            {
                grdList.Columns["Edit"].HeaderText = "Close";
                foreach (DataGridViewRow row in grdList.Rows)
                {
                    DataGridViewButtonColumn col = (DataGridViewButtonColumn)grdList.Columns["Edit"];
                    col.Text = "Close";
                }
            }
            else if (roles.Contains("MO") && listOption == 2)
            {
                grdList.Columns["Edit"].HeaderText = "Close";
                foreach (DataGridViewRow row in grdList.Rows)
                {
                    DataGridViewButtonColumn col = (DataGridViewButtonColumn)grdList.Columns["Edit"];
                    col.Text = "Close";
                }
            }
            else
            {
                grdList.Columns["Edit"].HeaderText = "Edit";
                foreach (DataGridViewRow row in grdList.Rows)
                {
                    DataGridViewButtonColumn col = (DataGridViewButtonColumn)grdList.Columns["Edit"];
                    col.Text = "Edit";
                }
            }
            pnlList.Visible = true;

        }

        private void initVariables()
        {
            try
            {
                if (getuserPrivilegeStatus() == 1)
                {
                    //user is only a viewer
                    listOption = 6;
                }
                CatalogueValueDB.fillCatalogValueComboNew(cmbComplaintType, "WorkplaceComplaints");
                txtEmpId.Text = Login.empLoggedIn;
                dtAcceptTime.Value = UpdateTable.getSQLDateTime();
                dtCloseTime.Value = UpdateTable.getSQLDateTime();
                dtAcceptTime.Format = DateTimePickerFormat.Custom;
                dtCloseTime.Format = DateTimePickerFormat.Custom;
                dtAcceptTime.CustomFormat = "dd-MM-yyyy HH:mm:ss";
                dtCloseTime.CustomFormat = "dd-MM-yyyy HH:mm:ss";
                txtEmpId.Enabled = false;
                userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
                setButtonVisibility("init");
            }
            catch (Exception)
            {
                MessageBox.Show("Exception");
            }
        }
        private void closeAllPanels()
        {
            try
            {
                pnlForwarder.Visible = false;
                pnlInner.Visible = false;
                pnlOuter.Visible = false;
                pnlList.Visible = false;

            }
            catch (Exception)
            {

            }
        }
        public void clearData()
        {
            try
            {
                
                txtEmpId.Text = "";
                dtCloseTime.Value = DateTime.Now;
                dtAcceptTime.Value = DateTime.Now;
                txtRemarks.Text = "";
                txtComplDesc.Text = "";
                prevwp = new workplacecr();
                removeControlsFrompnllvPanel();
                cmbComplaintType.SelectedIndex = -1;
            }
            catch (Exception)
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
                closeAllPanels();
                clearData();
                btnSave.Text = "Save";
                pnlOuter.Visible = true;
                pnlInner.Visible = true;
                setButtonVisibility("btnNew");
                txtEmpId.Text = Login.empLoggedIn;
                enableControlsApprove();
                lblAcceptTime.Visible = false;
                dtAcceptTime.Visible = false;
                lblCloseTime.Visible = false;
                dtCloseTime.Visible = false;
            }
            catch (Exception)
            {

            }
        }
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                clearData();
                pnlList.Visible = true;
                setButtonVisibility("btnEditPanel");
                ListFilteredWCR(listOption);
            }
            catch (Exception)
            {

            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {
                removeControlsFrompnllvPanel();
                WorkplaceCRDB wpDB = new WorkplaceCRDB();
                workplacecr wp = new workplacecr();
                ////wp.ComplaintType = cmbComplaintType.SelectedItem.ToString().Trim().Substring(0, cmbComplaintType.SelectedItem.ToString().Trim().IndexOf('-'));
                wp.ComplaintType = ((Structures.ComboBoxItem)cmbComplaintType.SelectedItem).HiddenValue;
                wp.EmployeeID = txtEmpId.Text;
                wp.Remarks = txtRemarks.Text;
                wp.ComplaintDescription = txtComplDesc.Text;

                string btnText = btnSave.Text;
                if (btnText.Equals("Update"))
                {
                    if (wpDB.validateComplaintReg(wp))
                    {
                        if (wpDB.updateComplaintReg(wp, prevwp))
                        {
                            MessageBox.Show("Complaint updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredWCR(listOption);
                            setButtonVisibility("btnEditPanel");
                            btnComplaintReg.Focus();
                        }
                        else
                        {
                            status = false;
                            MessageBox.Show("Failed to update Complaint");
                        }
                    }
                    else
                    {
                        status = false;
                        MessageBox.Show("Validation Failed: please check data entered");
                    }

                }
                else if (btnText.Equals("Save"))
                {
                    //if (!MovementRegisterDB.checkMovementStatus(mr))
                    //{
                    //    MessageBox.Show("Not allowed. Already One movement is on process");
                    //    return;
                    //}
                    wp.Status = 1;//created
                    wp.DocumentStatus = 1;
                    if (wpDB.validateComplaintReg(wp))
                    {
                        if (wpDB.insertComplaintReg(wp))
                        {
                            MessageBox.Show("Complaint Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredWCR(listOption);
                            btnComplaintReg.Focus();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Complaint");
                            status = false;

                        }
                    }
                    else
                    {
                        status = false;
                        MessageBox.Show("Validation Failed: please check data entered");
                    }
                }
                else
                {
                    status = false;
                    MessageBox.Show("Compalint Data Operation failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed Adding / Editing Complaint");
                status = false;
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }
        private void disableControlsOfInner()
        {
            txtComplDesc.Enabled = false;
            cmbComplaintType.Enabled = false;
            btnAddComment.Enabled = false;
        }
        private void enableControlsApprove()
        {
            txtComplDesc.Enabled = true;
            cmbComplaintType.Enabled = true;
            btnAddComment.Enabled = true;
        }
        private string getEmplRoleStr()
        {
            string role = "";
            WorkplaceCRDB wpdb = new WorkplaceCRDB();
            role = WorkplaceCRDB.getEmployeeRoleForMOOrMM();
            return role;
        }
        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit") || columnName.Equals("View") || columnName.Equals("Accept"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    enableControlsApprove();
                    pnlInner.Enabled = true;
                    prevwp = new workplacecr();
                    int rowID = e.RowIndex;
                    try
                    {
                        prevwp.rowID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["RowID"].Value.ToString());
                        prevwp.EmployeeID = grdList.Rows[e.RowIndex].Cells["EmpId"].Value.ToString();
                        prevwp.EmployeeName = grdList.Rows[e.RowIndex].Cells["EmployeeName"].Value.ToString();
                        prevwp.AcceptTime = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["AcceptTime"].Value.ToString());
                        prevwp.CloseTime = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["CloseTime"].Value.ToString());
                        prevwp.Remarks = grdList.Rows[e.RowIndex].Cells["Remarks"].Value.ToString();
                        prevwp.ComplaintDescription = grdList.Rows[e.RowIndex].Cells["ComplaintDesc"].Value.ToString();
                        prevwp.ComplaintType = grdList.Rows[e.RowIndex].Cells["ComplaintType"].Value.ToString();
                        //prevwp.DocumentStatus = stringtovalue(grdList.Rows[e.RowIndex].Cells["DocumentStatus"].Value.ToString());
                        prevwp.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentStatus"].Value.ToString());
                        prevwp.Status = stringtovalue(grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString());
                        setvisiblityOfEditBtn();
                        if (roles.Equals("MM"))
                        {
                            disableControlsOfInner();
                            if ((prevwp.Status != 6 && prevwp.Status != 5) && listOption != 3)
                            {
                                btnClose.Visible = true;
                                btnAddComment.Enabled = true;
                            }
                        }
                        else if (roles.Equals("MO"))
                        {
                            if (listOption == 1)
                            {
                                disableControlsOfInner();
                                btnAddComment.Enabled = true;
                                btnAccept.Visible = true;
                                //btnReject.Visible = true;
                            }
                            else if (listOption == 2)
                            {
                                disableControlsOfInner();
                                btnAddComment.Enabled = true;
                                btnClose.Visible = true;
                            }
                            else
                            {
                                disableControlsOfInner();
                            }
                        }
                        else
                        {
                            if (listOption == 1)
                            {
                                if (prevwp.DocumentStatus == 3)
                                {
                                    disableControlsOfInner();
                                    btnSave.Visible = true;
                                    btnAddComment.Enabled = true;
                                }
                                else
                                {
                                    enableControlsApprove();
                                    btnSave.Visible = true;
                                    btnCancelComplaint.Visible = true;
                                }
                            }
                            else if (listOption == 3)
                            {
                                disableControlsOfInner();
                                if (prevwp.Status == 3 || prevwp.Status == 5)
                                {
                                    if (WorkplaceCRDB.IsAllowToRejectClosedComplaint(prevwp))
                                    {
                                        btnRejectCLosing.Visible = true;
                                        btnAddComment.Enabled = true;
                                    }
                                }
                            }
                            else
                            {
                                disableControlsOfInner();
                            }
                        }
                        setvisiblityOfTime();
                        if (prevwp.Status == 2 || prevwp.Status == 5)
                        {
                            lblAcceptTime.Visible = true;
                            dtAcceptTime.Visible = true;
                            if (prevwp.Status == 5)
                            {
                                lblCloseTime.Visible = true;
                                dtCloseTime.Visible = true;
                            }
                        }
                        else if (prevwp.Status == 3)
                        {
                            lblCloseTime.Visible = true;
                            dtCloseTime.Visible = true;
                        }
                        else if (prevwp.Status == 6)
                        {
                            lblCloseTime.Visible = true;
                            dtCloseTime.Visible = true;
                            string strr = prevwp.AcceptTime.ToString();
                            string strr1 = DateTime.MinValue.ToString();
                            if (!(prevwp.AcceptTime.ToString().Equals(DateTime.MinValue.ToString())) && prevwp.DocumentStatus != 3)
                            {
                                lblAcceptTime.Visible = true;
                                dtAcceptTime.Visible = true;
                            }
                        }
                        prevwp.Remarks = grdList.Rows[e.RowIndex].Cells["Remarks"].Value.ToString();
                        prevwp.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["CreateTime"].Value.ToString());
                        prevwp.CreateUser = grdList.Rows[e.RowIndex].Cells["CreateUser"].Value.ToString();
                        prevwp.AcceptTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["AcceptTime"].Value.ToString());
                        prevwp.CloseTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["CloseTime"].Value.ToString());
                    }
                    catch (Exception ex)
                    {

                    }
                    Boolean str = btnAddComment.Enabled;
                    btnSave.Text = "Update";
                    pnlInner.Visible = true;
                    pnlOuter.Visible = true;
                    pnlList.Visible = false;

                    txtEmpId.Text = prevwp.EmployeeID.ToString();
                    txtComplDesc.Text = prevwp.ComplaintDescription;
                    ////////cmbComplaintType.SelectedIndex = cmbComplaintType.FindString(prevwp.ComplaintType);
                    cmbComplaintType.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbComplaintType, prevwp.ComplaintType);
                    txtRemarks.Text = prevwp.Remarks;
                    dtAcceptTime.Value = prevwp.AcceptTime;
                    dtCloseTime.Value = prevwp.CloseTime;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void setvisiblityOfTime()
        {
            lblAcceptTime.Visible = false;
            dtAcceptTime.Visible = false;
            lblCloseTime.Visible = false;
            dtCloseTime.Visible = false;
        }
        public void setvisiblityOfEditBtn()
        {
            btnAccept.Visible = false;
            //btnReject.Visible = false;
            btnClose.Visible = false;
            btnCancelComplaint.Visible = false;
            btnSave.Visible = false;
            btnRejectCLosing.Visible = false;
        }
        private void setButtonVisibility(string btnName)
        {
            try
            {
                lblActionHeader.Visible = true;
                pnlTopButtons.Visible = true;
                pnlInner.Enabled = true;
                btnCmplntStatus.Visible = true;
                btnComplaintReg.Visible = true;
                btnCmplntAccepted.Visible = true;
                btnRejectCLosing.Visible = false;

                btnNew.Visible = false;
                btnExit.Visible = false;

                btnCancel.Visible = false;
                btnSave.Visible = false;
                // btnReject.Visible = false;
                btnAccept.Visible = false;
                btnClose.Visible = false;
                btnCancelComplaint.Visible = false;

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
                    pnlTopButtons.Visible = false;
                    lblActionHeader.Visible = false;
                    btnNew.Visible = false; //added 24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (btnName == "btnEditPanel") //called from cancel,save,forward,approve and reverse button events
                {
                    ////btnNew.Visible = true;
                    btnExit.Visible = true;
                }
                //gridview buttons
                else if (btnName == "Edit")
                {
                    pnlTopButtons.Visible = false;
                    lblActionHeader.Visible = false;
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnCancelComplaint.Visible = true;
                    btnSave.Visible = true;
                    // btnReject.Visible = false;
                    btnAccept.Visible = false;

                }
                else if (btnName == "View")
                {

                    pnlTopButtons.Visible = false;
                    lblActionHeader.Visible = false;
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    //disableControlsInnerPanel();
                    //pnlInner.Enabled = false;
                }
                pnlEditButtons.Refresh();
                //if the user privilege is only view, show only the Approved documents button
                // int ups = getuserPrivilegeStatus();
                //if (ups == 1 || ups == 0)
                //{
                //    grdList.Columns["Edit"].Visible = false;

                //   // btnActionPending.Visible = false;
                //    btnApprovalPending.Visible = false;
                //    btnApproved.Visible = false;
                //    if (ups == 1)
                //    {
                //        grdList.Columns["View"].Visible = true;
                //    }
                //    else
                //    {
                //        grdList.Columns["View"].Visible = false;
                //    }
                //}
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
                /////if (roles.Contains("MO") || roles.Contains("MM"))  -- removed MM on 7-8-2017
                if (roles.Contains("MO"))
                {
                    btnNew.Visible = false;
                }
                else
                {
                    btnNew.Visible = true;
                }
            }
        }
        void handleGrdEditButton()
        {
            grdList.Columns["Edit"].Visible = false;
            //grdList.Columns["Approve"].Visible = false;
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                if (listOption == 1)
                {
                    grdList.Columns["Edit"].Visible = true;
                    //grdList.Columns["Approve"].Visible = true;
                }
            }
        }
        void handleGrdViewButton()
        {
            grdList.Columns["View"].Visible = false;
            // if any one of view,add and edit
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
        private void btnApprovalPending_Click_1(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredWCR(listOption);
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                removeControlsFrompnllvPanel();
                WorkplaceCRDB wpdb = new WorkplaceCRDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Accept the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevwp.Remarks = txtRemarks.Text;
                    if (wpdb.AcceptComplaint(prevwp))
                    {
                        MessageBox.Show("Complaint Accepted");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredWCR(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                        btnComplaintReg.Focus();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void btnReceived_Click(object sender, EventArgs e)
        {
            listOption = 3;
            ListFilteredWCR(listOption);
        }

        private void btnCancelDocument_Click(object sender, EventArgs e)
        {
            try
            {
                removeControlsFrompnllvPanel();
                WorkplaceCRDB wpdb = new WorkplaceCRDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Cancel the Complaint ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (wpdb.CancelComplaintByUser(prevwp))
                    {
                        MessageBox.Show("Complaint Cancelled");
                        clearData();
                        pnlInner.Visible = false;
                        pnlOuter.Visible = false;
                        listOption = 1;
                        setButtonVisibility("btnEditPanel");

                        ListFilteredWCR(listOption);
                        btnComplaintReg.Focus();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                removeControlsFrompnllvPanel();
                WorkplaceCRDB wpdb = new WorkplaceCRDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Reject the Complaint ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevwp.Remarks = txtRemarks.Text;
                    if (wpdb.RejectComplaintRegByMO(prevwp))
                    {
                        MessageBox.Show("complaint Rejected");
                        clearData();
                        pnlInner.Visible = false;
                        pnlOuter.Visible = false;
                        listOption = 1;
                        setButtonVisibility("btnEditPanel");
                        ListFilteredWCR(listOption);
                        btnComplaintReg.Focus();
                    }
                }
            }
            catch (Exception)
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
        private void btnAddRemarks_Click(object sender, EventArgs e)
        {
            //removeControlsFrompnllvPanel();


            //pnllv = new Panel();
            //pnllv.BorderStyle = BorderStyle.FixedSingle;

            //pnllv.Bounds = new Rectangle(new Point(200, 30), new Size(400, 300));
            //pnllv.BackColor = Color.Gray;
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            txt = new RichTextBox();
            txt.Bounds = new Rectangle(new Point(25, 25), new Size(350, 200));
            frmPopup.Controls.Add(txt);

            Label lblHeader = new Label();
            lblHeader.Size = new Size(300, 20);
            lblHeader.Text = "Type your Comments             ";
            lblHeader.Location = new Point(50, 10);
            frmPopup.Controls.Add(lblHeader);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Location = new Point(100, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Click5);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Location = new Point(200, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click5);
            frmPopup.Controls.Add(lvCancel);

            //pnlInner.Controls.Add(pnllv);
            //pnllv.BringToFront();
            //pnllv.Visible = true;
            txt.Focus();
            frmPopup.ShowDialog();
        }
        private void lvOK_Click5(object sender, EventArgs e)
        {
            try
            {
                //pnllv.Visible = true;
                if (txt.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please Add Comment");
                    return;
                }
                txtRemarks.Text = txtRemarks.Text + Environment.NewLine + Login.userLoggedInName +
                            " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + txt.Text;
                //btnAddComment.Enabled = true;
                //pnllv.Visible = false;
                txtRemarks.ReadOnly = true;
                ContextMenu blankContextMenu = new ContextMenu();
                txtRemarks.ContextMenu = blankContextMenu;
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
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        private void txtComment_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void btnRegisteredCompl_Click_1(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredWCR(listOption);
        }

        private void btnAcceptedComp_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredWCR(listOption);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                removeControlsFrompnllvPanel();
                WorkplaceCRDB wpdb = new WorkplaceCRDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Close the Complaint ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevwp.Remarks = txtRemarks.Text;
                    if (roles.Contains("MM"))
                    {
                        string remark = Login.userLoggedInName + " : " +
                            UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Request Closed";
                        prevwp.Remarks = txtRemarks.Text + Environment.NewLine + remark;
                        if (wpdb.CloseComplaintByMM(prevwp))
                        {
                            MessageBox.Show("complaint Closed");
                            clearData();
                            pnlInner.Visible = false;
                            pnlOuter.Visible = false;
                            listOption = 1;
                            setButtonVisibility("btnEditPanel");
                            ListFilteredWCR(listOption);
                            btnComplaintReg.Focus();
                        }
                    }
                    else
                    {
                        string remark = Login.userLoggedInName + " : " +
                            UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Work Completetd";
                        prevwp.Remarks = txtRemarks.Text + Environment.NewLine + remark;
                        if (wpdb.CloseComplaintByMO(prevwp))
                        {
                            MessageBox.Show("complaint Closed");
                            clearData();
                            pnlInner.Visible = false;
                            pnlOuter.Visible = false;
                            listOption = 1;
                            setButtonVisibility("btnEditPanel");
                            ListFilteredWCR(listOption);
                        }
                    }

                }
            }
            catch (Exception)
            {
            }
        }
        private void btnComplaintStatus_Click(object sender, EventArgs e)
        {
            listOption = 3;
            ListFilteredWCR(listOption);
        }

        private void btnRejectCLosing_Click(object sender, EventArgs e)
        {
            try
            {
                removeControlsFrompnllvPanel();
                WorkplaceCRDB wpdb = new WorkplaceCRDB();
                DialogResult dialog = MessageBox.Show("Are you sure to reject the MO action?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    string remark = Login.userLoggedInName + " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Work done is rejected";
                    prevwp.Remarks = txtRemarks.Text + Environment.NewLine + remark;
                    prevwp.AcceptTime = DateTime.Parse("01-01-1900");
                    prevwp.CloseTime = DateTime.Parse("01-01-1900");
                    if (wpdb.RejectComplaintRegByUser(prevwp))
                    {
                        MessageBox.Show("Complaint re-Opened");
                        clearData();
                        pnlInner.Visible = false;
                        pnlOuter.Visible = false;
                        listOption = 1;
                        setButtonVisibility("btnEditPanel");
                        ListFilteredWCR(listOption);
                        btnComplaintReg.Focus();
                    }
                }
            }
            catch (Exception)
            {
            }
        }


        public string valuetostring(int stat, int docstat)
        {
            string status = "";
            if (stat == 1 && docstat == 1)
            {
                status = "Created";
            }
            else if (stat == 2 && (docstat == 1 || docstat == 3))
            {
                status = "Accepted";
            }
            else if (stat == 3 && docstat == 2)
            {
                status = "Rejected";
            }
            else if (stat == 4 && docstat == 1)
            {
                status = "Cancelled";
            }
            else if (stat == 5 && docstat == 2)
            {
                status = "Completed";
            }
            else if (stat == 6 && (docstat == 1 || docstat == 3))
            {
                status = "Closed By MM";
            }
            else if (stat == 1 && docstat == 3)
            {
                status = "Reversed";
            }
            return status;
        }
        public int stringtovalue(string status)
        {
            int stat = 0;
            if (status.Equals("Created") || status.Equals("Reversed"))
            {
                stat = 1;
            }
            else if (status.Equals("Accepted"))
            {
                stat = 2;
            }
            else if (status.Equals("Rejected"))
            {
                stat = 3;
            }
            else if (status.Equals("Cancelled"))
            {
                stat = 4;
            }
            else if (status.Equals("Completed"))
            {
                stat = 5;
            }
            else if (status.Equals("Closed By MM"))
            {
                stat = 6;
            }
            return stat;
        }

        private void WorkplaceCR_Enter(object sender, EventArgs e)
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

