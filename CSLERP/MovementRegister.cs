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
    public partial class MovementRegister : System.Windows.Forms.Form
    {
        string docID = "";
        string forwarderList = "";
        string approverList = "";
        TextBox txtSearch = new TextBox();
        string userString = "";
        Boolean isApprover = false;
        RichTextBox commentText = new RichTextBox();
        ListView lvCopy = new ListView();
        Panel pnllv = new Panel();
        ListView lv = new ListView();
        int listOption = 3; //
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        movementregister prevmr;
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        Panel pnlForwarder = new Panel();
        ListView lvApprover = new ListView();
        Panel pnlCmtr = new Panel();
        List<user> emailUserList = new List<user>();
        Form frmPopup = new Form();
        public MovementRegister()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }

        private void MR_Load(object sender, EventArgs e)
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

            if (isApprover)
            {
                btnReceived.Visible = true;
                btnApprovalsGiven.Visible = true;
                btnApprovalPending.Visible = true;
                btnApproved.Visible = true;

                listOption = 3;
                ////ListFilteredMR(listOption);
            }
            else
            {
                btnReceived.Visible = false;
                btnApprovalsGiven.Visible = false;
                btnApprovalPending.Visible = true;
                btnApproved.Visible = true;
                listOption = 1;
                ////ListFilteredMR(listOption);
            }
            ListFilteredMR(listOption);
            //applyPrivilege();
        }
        private void ListFilteredMR(int option)
        {
            string app = "";
            int docstat = 0;
            try
            {

                grdList.Rows.Clear();
                MovementRegisterDB mrdb = new MovementRegisterDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                List<movementregister> MRList = mrdb.getFilteredmovementregister(option);
                if (option == 1)
                {
                    lblActionHeader.Text = "Documents waiting for approval";
                    btnApprovalPending.Focus();
                }
                if (option == 2)
                {
                    lblActionHeader.Text = "Documents approved by approver";
                    btnApproved.Focus();
                }
                else if (option == 3)
                {
                    lblActionHeader.Text = "Documents received for approval";
                    btnReceived.Focus();
                }
                else if (option == 4)
                {
                    lblActionHeader.Text = "Documents approved by " + Login.userLoggedInName;
                    btnApprovalsGiven.Focus();
                }

                foreach (movementregister mr in MRList)
                {
                    if (option == 1)
                    {
                        if (mr.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    if (DateTime.Now.Subtract(mr.CreateTime).TotalDays < 30)
                    {
                        grdList.Rows.Add();
                        grdList.Rows[grdList.RowCount - 1].Cells["RowID"].Value = mr.rowID;
                        grdList.Rows[grdList.RowCount - 1].Cells["EmpId"].Value = mr.EmployeeID;
                        grdList.Rows[grdList.RowCount - 1].Cells["EmployeeName"].Value = mr.EmployeeName;
                        grdList.Rows[grdList.RowCount - 1].Cells["PlannedExitTime"].Value = mr.ExitTimePlanned;
                        grdList.Rows[grdList.RowCount - 1].Cells["PlannedReturnTime"].Value = mr.ReturnTimePlanned;
                        grdList.Rows[grdList.RowCount - 1].Cells["Purpose"].Value = mr.Purpose;
                        grdList.Rows[grdList.RowCount - 1].Cells["ModeOfTravel"].Value = mr.ModeOfTravel;
                        grdList.Rows[grdList.RowCount - 1].Cells["OutTime"].Value = mr.OutTime;
                        grdList.Rows[grdList.RowCount - 1].Cells["InTime"].Value = mr.InTime;
                        grdList.Rows[grdList.RowCount - 1].Cells["Comment"].Value = mr.Comments;
                        grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = mr.Status;
                        grdList.Rows[grdList.RowCount - 1].Cells["MovementStatus"].Value = MovementRegisterDB.valuetostring(mr.DocumentStatus,mr.Status);
                        grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = mr.CreateTime;
                        grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = mr.CreateUser;
                        grdList.Rows[grdList.RowCount - 1].Cells["ApproveUser"].Value = mr.ApproveUser;
                        grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = mr.Approver;
                        grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = mr.Creator;
                        grdList.Rows[grdList.RowCount - 1].Cells["ApproveTime"].Value = mr.ApproveTime;
                        if (listOption == 3)
                        {
                            app = mr.ApproveUser;
                            docstat = mr.DocumentStatus;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Movement Register listing");
            }
            setButtonVisibility("init");
            if (option == 2)
            {
                grdList.Columns["View"].Visible = true;
                grdList.Columns["Edit"].Visible = false;
                grdList.Columns["Cancel"].Visible = false;
            }
            else if (option == 4)
            {
                grdList.Columns["View"].Visible = false;
                grdList.Columns["Edit"].Visible = false;
                grdList.Columns["Cancel"].Visible = true;
            }
            else
            {
                grdList.Columns["View"].Visible = false;
                grdList.Columns["Edit"].Visible = true;
                grdList.Columns["Cancel"].Visible = false;
            }
            if (Login.userLoggedIn.Equals(app) && listOption == 3)
            {
                if (docstat == 5)
                {
                    grdList.Columns["Edit"].HeaderText = "Accept/Cancel";
                    foreach (DataGridViewRow row in grdList.Rows)
                    {
                        DataGridViewButtonColumn col = (DataGridViewButtonColumn)grdList.Columns["Edit"];
                        col.Text = "Accept/Cancel";
                    }
                }
                else
                {
                    grdList.Columns["Edit"].HeaderText = "Approve/Reject";
                    foreach (DataGridViewRow row in grdList.Rows)
                    {
                        DataGridViewButtonColumn col = (DataGridViewButtonColumn)grdList.Columns["Edit"];
                        col.Text = "Approve/Reject";
                    }
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
            //if (roles.Contains("MO") && listOption == 1)
            //{

            //}
            //else if (roles.Contains("MM") && listOption == 1)
            //{
            //    grdList.Columns["Edit"].HeaderText = "Close";
            //    foreach (DataGridViewRow row in grdList.Rows)
            //    {
            //        DataGridViewButtonColumn col = (DataGridViewButtonColumn)grdList.Columns["Edit"];
            //        col.Text = "Close";
            //    }
            //}
            pnlList.Visible = true;
            if (grdList.RowCount == 0)
            {
                grdList.Visible = false;
            }
            else
            {
                grdList.Visible = true;
            }
            //grdList.Columns["Creator"].Visible = true;
            //grdList.Columns["Forwarder"].Visible = true;
            //grdList.Columns["Approver"].Visible = true;
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
                ////CatalogueValueDB.fillCatalogValueCombo(cmbModeOfTransport, "TravelMode");
                CatalogueValueDB.fillCatalogValueComboNew(cmbModeOfTransport, "TravelMode");
                CatalogueValueDB.fillCatalogValueComboNew(CmbPurpose, "MovementPurpose");
                txtEmpId.Text = Login.empLoggedIn;
                dtPlannedExitTime.Value = UpdateTable.getSQLDateTime();
                dtplannedRturnTime.Value = UpdateTable.getSQLDateTime();
                dtplannedRturnTime.Format = DateTimePickerFormat.Custom;
                dtPlannedExitTime.Format = DateTimePickerFormat.Custom;
                dtplannedRturnTime.CustomFormat = "dd-MM-yyyy HH:mm:ss";
                dtPlannedExitTime.CustomFormat = "dd-MM-yyyy HH:mm:ss";

                dtInTime.Value = DateTime.Now;
                dtOutTime.Value = DateTime.Now;
                dtInTime.Format = DateTimePickerFormat.Custom;
                dtOutTime.Format = DateTimePickerFormat.Custom;
                dtInTime.CustomFormat = "dd-MM-yyyy HH:mm:ss";
                dtOutTime.CustomFormat = "dd-MM-yyyy HH:mm:ss";

                txtEmpId.Enabled = false;
                userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
                setButtonVisibility("init");
                isApprover = EmployeeDB.checkEmployeeRole(Login.empLoggedIn, "MovementApproval");
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

        //private void btnCancel_Click(object sender, EventArgs e)
        //{

        //}

        public void clearData()
        {
            try
            {
                txtEmpId.Text = "";
                dtPlannedExitTime.Value = DateTime.Now;
                dtplannedRturnTime.Value = DateTime.Now;
                dtInTime.Value = DateTime.Now;
                dtOutTime.Value = DateTime.Now;
                txtComment.Text = "";
                txtApprover.Text = "";
                empPicture.Image = null;
                prevmr = new movementregister();
                //removeControlsFromlvPanel();
                cmbModeOfTransport.SelectedIndex = -1;
                CmbPurpose.SelectedIndex = -1;
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
                lblOutTime.Visible = false;
                dtOutTime.Visible = false;
                lblInTime.Visible = false;
                dtInTime.Visible = false;
                lblEmployeeName.Text = Login.userLoggedInName;
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
                removeControlsFromlvPanel();
                MovementRegisterDB mrDB = new MovementRegisterDB();
                movementregister mr = new movementregister();
                ////mr.ModeOfTravel = cmbModeOfTransport.SelectedItem.ToString().Trim().Substring(0, cmbModeOfTransport.SelectedItem.ToString().Trim().IndexOf('-'));
                mr.ModeOfTravel = ((Structures.ComboBoxItem)cmbModeOfTransport.SelectedItem).HiddenValue;
                mr.EmployeeID = txtEmpId.Text;
                mr.ExitTimePlanned = dtPlannedExitTime.Value;
                mr.ReturnTimePlanned = dtplannedRturnTime.Value;
                mr.Comments = txtComment.Text;
                mr.ApproveUser = txtApprover.Text.Substring(0, txtApprover.Text.IndexOf('-'));
                ////mr.Purpose = CmbPurpose.SelectedItem.ToString().Trim().Substring(0, CmbPurpose.SelectedItem.ToString().Trim().IndexOf('-'));
              
               

                mr.Comments = txtComment.Text.Replace("'","''");
                mr.Purpose = ((Structures.ComboBoxItem)CmbPurpose.SelectedItem).HiddenValue;
                string btnText = btnSave.Text;
                if (btnText.Equals("Update"))
                {
                    if (mrDB.validateMovementReg(mr))
                    {
                        if (mrDB.updateMovementReg(mr, prevmr))
                        {
                            MessageBox.Show("Movement updated");
                            if (mr.ApproveUser != prevmr.ApproveUser)
                            {
                                prepareEmail();
                            }
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredMR(listOption);
                            setButtonVisibility("btnEditPanel");
                            btnApprovalPending.Focus();
                        }
                        else
                        {
                            status = false;
                            MessageBox.Show("Failed to update Movement");
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
                    string remark = Login.userLoggedInName +
                          " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Movement Created" + "\n";
                    mr.Comments = txtComment.Text.Replace("'","''") + Environment.NewLine + remark;
                    if (!MovementRegisterDB.checkMovementStatus(mr))
                    {
                        MessageBox.Show("Not allowed. Already One movement is on process");
                        return;
                    }
                    mr.Status = 1;//created
                    mr.DocumentStatus = 1;
                    if (mrDB.validateMovementReg(mr))
                    {
                        if (mrDB.insertMovementReg(mr))
                        {
                            MessageBox.Show("Movement Added");
                            //send email
                            {
                                prepareEmail();
                            }
                            //----email
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredMR(listOption);
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Movement");
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
                    MessageBox.Show("Movement Data Operation failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed Adding / Editing Movement");
                status = false;
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }

        private void prepareEmail()
        {
            {
                string menuID = getMenuID();
                try
                {
                    string toAddress = "";
                    toAddress = ERPUserDB.getemailIDs(emailUserList, menuID);
                    //create emaildata
                    if (toAddress.Trim().Length > 0)
                    {
                        EmailDataDB emdataDB = new EmailDataDB();
                        emaildata emdata = new emaildata();
                        emdata.ToAddress = toAddress;
                        emdata.status = 0;
                        emdata.EmailData = "Movement request from " + Login.userLoggedInName + ". Planned exit time : " + dtPlannedExitTime.Value.ToString("dd-MM-yyyy HH:mm:ss");
                        emdata.Subject = "Movement request from " + Login.userLoggedInName + ". Planned exit time : " + dtPlannedExitTime.Value.ToString("dd-MM-yyyy HH:mm:ss");
                        emdataDB.insertEmailData(emdata);
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        private string getMenuID()
        {
            string menuID = "";
            try
            {
                foreach (Control p in Controls["pnlUI"].Controls)
                {
                    if (p.GetType() == typeof(Label))
                    {
                        Label c = (Label)p;
                        if (c.Name == "MenuItemID")
                        {
                            menuID = p.Text;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return menuID;
        }

        private void disableControlsApprove()
        {
            txtApprover.Enabled = false;
            CmbPurpose.Enabled = false;
            dtPlannedExitTime.Enabled = false;
            dtplannedRturnTime.Enabled = false;
            cmbModeOfTransport.Enabled = false;
            btnSelectApprover.Enabled = false;
        }
        private void enableControlsApprove()
        {
            txtApprover.Enabled = true;
            CmbPurpose.Enabled = true;
            dtPlannedExitTime.Enabled = true;
            dtplannedRturnTime.Enabled = true;
            cmbModeOfTransport.Enabled = true;
            btnSelectApprover.Enabled = true;
        }
        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit") || columnName.Equals("View") || columnName.Equals("Cancel"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    enableControlsApprove();
                    prevmr = new movementregister();
                    int rowID = e.RowIndex;
                    try
                    {
                        prevmr.rowID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["RowID"].Value.ToString());
                        prevmr.EmployeeID = grdList.Rows[e.RowIndex].Cells["EmpId"].Value.ToString();
                        prevmr.ExitTimePlanned = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["PlannedExitTime"].Value.ToString());
                        prevmr.ReturnTimePlanned = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["PlannedReturnTime"].Value.ToString());
                        prevmr.Purpose = grdList.Rows[e.RowIndex].Cells["Purpose"].Value.ToString();
                        prevmr.ModeOfTravel = grdList.Rows[e.RowIndex].Cells["ModeOfTravel"].Value.ToString();
                        prevmr.ApproveUser = grdList.Rows[e.RowIndex].Cells["ApproveUser"].Value.ToString();
                        prevmr.Approver = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                        prevmr.DocumentStatus = MovementRegisterDB.stringtovalue(grdList.Rows[e.RowIndex].Cells["MovementStatus"].Value.ToString());
                        prevmr.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString());
                        txtEmpId.Text = prevmr.EmployeeID.ToString();
                        lblEmployeeName.Text = grdList.Rows[e.RowIndex].Cells["EmployeeName"].Value.ToString();

                        //// following block commented on 19-07-2017
                        //////if (prevmr.DocumentStatus == 10)
                        //////{
                        //////    clearData();
                        //////    pnlInner.Visible = false;
                        //////    pnlOuter.Visible = false;
                        //////    setButtonVisibility("btnEditPanel");
                        //////    MessageBox.Show("Movement Rejected, not able to Edit.");
                        //////    return;
                        //////}
                        if (prevmr.DocumentStatus == 2 && columnName.Equals("Cancel"))
                        {
                            btnCancelDocument.Visible = true;
                        }
                        else
                        {
                            btnCancelDocument.Visible = false;
                            if (prevmr.DocumentStatus != 1)
                            {
                                pnlInner.Enabled = false;
                            }
                        }
                        //////if (prevmr.DocumentStatus != 2 && columnName.Equals("Cancel"))
                        //////{
                        //////    clearData();
                        //////    pnlInner.Visible = false;
                        //////    pnlOuter.Visible = false;
                        //////    setButtonVisibility("btnEditPanel");
                        //////    MessageBox.Show("Not allowed to Cancel.");
                        //////    return;
                        //////}
                        if (listOption == 3 || listOption == 4)
                        {
                            try
                            {
                                //byte[] data = (byte[])grdList.Rows[e.RowIndex].Cells["EmpImage"].Value;
                                byte[] data = EmployeeDB.getPictureOfEmployee(txtEmpId.Text);
                                MemoryStream ms = new MemoryStream(data);
                                empPicture.Image = Image.FromStream(ms);
                            }
                            catch (Exception ex)
                            {
                                empPicture.Image = null;
                            }
                        }
                        else
                        {
                            empPicture.Image = null;
                        }
                        //if(prevmr.DocumentStatus == 99)
                        //{

                        //}
                        if (Login.userLoggedIn.Equals(prevmr.ApproveUser) && prevmr.Status == 1 && listOption != 4)  // Received Click
                        {
                            btnSave.Visible = false;
                            btnCancelDocument.Visible = false;
                            if(prevmr.DocumentStatus==5)
                            {
                                btnApprove.Visible = false;
                                btnReject.Visible = false;
                                btnAccptMovCancel.Visible = true;
                                btnRejMovCancel.Visible = true;
                            }
                            else
                            {
                                btnApprove.Visible = true;
                                btnReject.Visible = true;
                            }                          
                            disableControlsApprove();
                        }
                        else if (columnName.Equals("View"))
                        {
                            pnlInner.Enabled = false;
                            btnSave.Visible = false;
                            btnApprove.Visible = false;
                            btnReject.Visible = false;
                            btnCancelDocument.Visible = false;
                            if (prevmr.DocumentStatus == 2)
                            {
                                btnCancelApprvdMvmnt.Visible = true;
                            }
                            else
                            {
                                btnCancelApprvdMvmnt.Visible = false;
                            }
                            btnAccptMovCancel.Visible = false;
                            btnRejMovCancel.Visible = false;
                        }
                        else if (columnName.Equals("Edit"))
                        {
                            btnSave.Visible = true;
                            btnApprove.Visible = false;
                            btnReject.Visible = false;
                            btnCancelDocument.Visible = true;
                        }
                        else if (columnName.Equals("Cancel"))
                        {
                            disableControlsApprove();
                            btnAddComment.Enabled = true;
                        }
                        prevmr.OutTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["OutTime"].Value.ToString());
                        prevmr.InTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["InTime"].Value.ToString());
                        if (prevmr.DocumentStatus == 3)
                        {
                            lblOutTime.Visible = true;
                            dtOutTime.Visible = true;
                        }
                        else
                        {
                            lblOutTime.Visible = false;
                            dtOutTime.Visible = false;
                        }
                        if (prevmr.DocumentStatus == 99)
                        {
                            lblInTime.Visible = true;
                            dtInTime.Visible = true;
                            lblOutTime.Visible = true;
                            dtOutTime.Visible = true;
                        }
                        else
                        {
                            lblInTime.Visible = false;
                            dtInTime.Visible = false;

                        }
                        prevmr.Comments = grdList.Rows[e.RowIndex].Cells["Comment"].Value.ToString();


                        prevmr.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["CreateTime"].Value.ToString());
                        prevmr.CreateUser = grdList.Rows[e.RowIndex].Cells["CreateUser"].Value.ToString();

                    }
                    catch (Exception ex)
                    {

                    }
                    btnSave.Text = "Update";
                    pnlInner.Visible = true;
                    pnlOuter.Visible = true;
                    pnlList.Visible = false;
                    //cmbStatus.Enabled = true;

                    txtEmpId.Text = prevmr.EmployeeID.ToString();
                    //DateTime ttmp = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["TemporaryDate"].Value.ToString());
                    ////CmbPurpose.SelectedIndex = CmbPurpose.FindString(prevmr.Purpose);
                    CmbPurpose.SelectedIndex =
                       Structures.ComboFUnctions.getComboIndex(CmbPurpose, prevmr.Purpose);
                    //txtPurpose.Text = prevmr.Purpose;

                    //dtTrackingDate.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["TrackingDate"].Value.ToString());
                    ////cmbModeOfTransport.SelectedIndex = cmbModeOfTransport.FindString(prevmr.ModeOfTravel);
                    cmbModeOfTransport.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbModeOfTransport, prevmr.ModeOfTravel);
                    //cmbCourierID.SelectedIndex = cmbCourierID.FindString(prevsh.CourierID);
                    txtApprover.Text = prevmr.ApproveUser + "-" + prevmr.Approver;
                    txtComment.Text = prevmr.Comments;
                    dtplannedRturnTime.Value = prevmr.ReturnTimePlanned;
                    dtPlannedExitTime.Value = prevmr.ExitTimePlanned;
                    dtOutTime.Value = prevmr.OutTime;
                    dtInTime.Value = prevmr.InTime;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void removeControlsFromApproverPanel()
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
        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredMR(listOption);
        }
        private void btnApproved_Click_1(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredMR(listOption);
        }

        private void setButtonVisibility(string btnName)
        {
            try
            {
                lblActionHeader.Visible = true;
                pnlTopButtons.Visible = true;
                pnlInner.Enabled = true;
                ////btnReceived.Visible = true;
                ////btnApprovalPending.Visible = true;
                ////btnApproved.Visible = true;
                btnNew.Visible = false;
                btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                btnReject.Visible = false;
                btnApprove.Visible = false;
                btnCancelDocument.Visible = false;
                handleNewButton();
                handleGrdViewButton();
                handleGrdCancelButton();
                handleGrdEditButton();
                pnlBottomButtons.Visible = true;
                btnCancelApprvdMvmnt.Visible = false;
                btnAccptMovCancel.Visible = false;
                btnRejMovCancel.Visible = false;
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
                    btnCancelDocument.Visible = true;
                    btnSave.Visible = true;
                    btnReject.Visible = false;
                    btnApprove.Visible = false;
                }
                else if (btnName == "View")
                {
                    pnlTopButtons.Visible = false;
                    lblActionHeader.Visible = false;
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    //disableControlsInnerPanel();
                    pnlInner.Enabled = false;
                }
                else if (btnName == "Cancel")
                {
                    pnlTopButtons.Visible = false;
                    lblActionHeader.Visible = false;
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnCancelDocument.Visible = true;
                    btnSave.Visible = false;
                    btnReject.Visible = false;
                    btnApprove.Visible = false;
                }
                pnlEditButtons.Refresh();

            }
            catch (Exception ex)
            {
            }
        }
        private void setButtonVisibilityOld(string btnName)
        {
            try
            {
                lblActionHeader.Visible = true;
                pnlTopButtons.Visible = true;
                pnlInner.Enabled = true;
                btnReceived.Visible = true;

                btnApprovalPending.Visible = true;
                btnApproved.Visible = true;
                btnNew.Visible = false;
                btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                btnReject.Visible = false;
                btnApprove.Visible = false;
                btnCancelDocument.Visible = false;
                handleNewButton();
                handleGrdViewButton();
                handleGrdCancelButton();
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
                    btnCancelDocument.Visible = true;
                    btnSave.Visible = true;
                    btnReject.Visible = false;
                    btnApprove.Visible = false;
                }
                else if (btnName == "View")
                {
                    pnlTopButtons.Visible = false;
                    lblActionHeader.Visible = false;
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    //disableControlsInnerPanel();
                    pnlInner.Enabled = false;
                }
                else if (btnName == "Cancel")
                {
                    pnlTopButtons.Visible = false;
                    lblActionHeader.Visible = false;
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnCancelDocument.Visible = true;
                    btnSave.Visible = false;
                    btnReject.Visible = false;
                    btnApprove.Visible = false;
                }
                pnlEditButtons.Refresh();
                if (isApprover)
                {
                    btnReceived.Visible = true;
                    btnApprovalsGiven.Visible = true;
                    listOption = 3;
                    ListFilteredMR(listOption);
                }
                else
                {
                    btnReceived.Visible = false;
                    btnApprovalsGiven.Visible = false;
                    listOption = 1;
                    ListFilteredMR(listOption);
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
            //grdList.Columns["Approve"].Visible = false;
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                if (listOption != 2 && listOption != 4)
                {
                    grdList.Columns["Edit"].Visible = true;
                    //grdList.Columns["Approve"].Visible = true;
                }
            }
        }
        void handleGrdCancelButton()
        {
            grdList.Columns["Cancel"].Visible = false;
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                if (listOption == 4)
                {
                    grdList.Columns["Cancel"].Visible = true;
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
                if (listOption == 2)
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
        private void pnlPDFViewer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {

        }

        private void btnApprovalPending_Click_1(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredMR(listOption);
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                removeControlsFromlvPanel();
                MovementRegisterDB mrdb = new MovementRegisterDB();
                string remark = Login.userLoggedInName +
                              " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Movement Approved" + "\n";
                prevmr.Comments = txtComment.Text.Replace("'", "''") + Environment.NewLine + remark;
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    //prevmr.Comments = txtComment.Text;
                    if (mrdb.ApproveMR(prevmr))
                    {
                        MessageBox.Show("Movement Approved");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredMR(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                        btnApprovalPending.Focus();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void removeControlsFromlvPanel()
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
        private void LvColumnClick(object o, ColumnClickEventArgs e)
        {
            try
            {
                if (lv.Visible == true)
                {
                    string first = lv.Items[0].SubItems[e.Column].Text;
                    string last = lv.Items[lv.Items.Count - 1].SubItems[e.Column].Text;
                    System.Windows.Forms.SortOrder sort_order1 = SortingListView.getSortedOrder(first, last);
                    this.lv.ListViewItemSorter = new SortingListView(e.Column, sort_order1);
                }
                else
                {
                    string first = lvCopy.Items[0].SubItems[e.Column].Text;
                    string last = lvCopy.Items[lvCopy.Items.Count - 1].SubItems[e.Column].Text;
                    System.Windows.Forms.SortOrder sort_order1 = SortingListView.getSortedOrder(first, last);
                    this.lvCopy.ListViewItemSorter = new SortingListView(e.Column, sort_order1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorting error");
            }
        }
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
        private void showApproverLIstView()
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            lv = EmployeeDB.MovementRegApproverListView("MovementApproval");
            lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            lv.Columns[3].Width = 0;
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Click1);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new Point(141, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click1);
            frmPopup.Controls.Add(lvCancel);

            Label lblSearch = new Label();
            lblSearch.Text = "Search";
            lblSearch.Location = new Point(250, 267);
            lblSearch.Size = new Size(45, 15);
            frmPopup.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Location = new Point(300, 265);
            txtSearch.TextChanged += new EventHandler(txtSearch_TextChangedEmp);
            frmPopup.Controls.Add(txtSearch);
            txtSearch.Focus();
            frmPopup.ShowDialog();
        }
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                emailUserList = new List<user>();
                if (lv.Visible == true)
                {
                    if (!checkLVItemChecked("Approver"))
                    {
                        return;
                    }
                    //btnSelectApprover.Enabled = true;
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            if (!itemRow.SubItems[1].Text.Equals(Login.empLoggedIn))
                            {
                                txtApprover.Text = itemRow.SubItems[3].Text + "-" + itemRow.SubItems[2].Text;
                                itemRow.Checked = false;
                                frmPopup.Close();
                                frmPopup.Dispose();
                                //add to email user list

                                user us = new user();
                                us.userEmpID = itemRow.SubItems[1].Text;
                                us.userEmpName = itemRow.SubItems[2].Text;
                                us.userID = itemRow.SubItems[3].Text;
                                emailUserList.Add(us);
                            }
                            else
                            {
                                MessageBox.Show("Not allowed to select same employee as employee logged In");
                                itemRow.Checked = false;
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (!checkLVCopyItemChecked("Approver"))
                    {
                        return;
                    }
                    //btnSelectApprover.Enabled = true;
                    foreach (ListViewItem itemRow in lvCopy.Items)
                    {
                        if (itemRow.Checked)
                        {
                            if (!itemRow.SubItems[1].Text.Equals(Login.empLoggedIn))
                            {
                                txtApprover.Text = itemRow.SubItems[3].Text + "-" + itemRow.SubItems[2].Text;
                                itemRow.Checked = false;
                                frmPopup.Close();
                                frmPopup.Dispose();
                            }
                            else
                            {
                                MessageBox.Show("Not allowed to select same employee as employee logged In");
                                itemRow.Checked = false;
                                return;
                            }
                        }
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
                //btnSelectApprover.Enabled = true;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        private void txtSearch_TextChangedEmp(object sender, EventArgs e)
        {
            frmPopup.Controls.Remove(lvCopy);
            addItemsEmp();
        }
        private void addItemsEmp()
        {
            lvCopy = new ListView();
            lvCopy.View = System.Windows.Forms.View.Details;
            lvCopy.LabelEdit = true;
            lvCopy.AllowColumnReorder = true;
            lvCopy.CheckBoxes = true;
            lvCopy.FullRowSelect = true;
            lvCopy.GridLines = true;
            lvCopy.Sorting = System.Windows.Forms.SortOrder.Ascending;
            lvCopy.Columns.Add("Select", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Emp Id", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Emp Name", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("User ID", -2, HorizontalAlignment.Left);

            lvCopy.Sorting = System.Windows.Forms.SortOrder.None;
            lvCopy.ColumnClick += new ColumnClickEventHandler(LvColumnClick); ;
            //this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
            //lvCopy.Location = new Point(13, 9);
            //lvCopy.Size = new Size(440, 199);
            lvCopy.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            lvCopy.Items.Clear();
            foreach (ListViewItem row in lv.Items)
            {
                string x = row.SubItems[0].Text;
                string no = row.SubItems[1].Text;
                string ch = row.SubItems[2].Text;
                string name = row.SubItems[3].Text;
                if (ch.ToLower().StartsWith(txtSearch.Text))
                {
                    ListViewItem item = new ListViewItem();
                    item.SubItems.Add(no);
                    item.SubItems.Add(ch);
                    item.SubItems.Add(name);
                    item.Checked = false;
                    lvCopy.Items.Add(item);
                }
            }
            lvCopy.Columns[3].Width = 0;
            lv.Visible = false;
            lvCopy.Visible = true;
            frmPopup.Controls.Add(lvCopy);
        }
        //private void CopylistView2_ItemChecked(object sender, ItemCheckedEventArgs e)
        //{
        //    try
        //    {
        //        if (lvCopy.CheckedIndices.Count > 1)
        //        {
        //            MessageBox.Show("Cannot select more than one item");
        //            e.Item.Checked = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        private void btnSelectApprover_Click(object sender, EventArgs e)
        {
            try
            {
                showApproverLIstView();
            }
            catch (Exception ex)
            {
            }
        }

        private void btnReceived_Click(object sender, EventArgs e)
        {
            listOption = 3;
            ListFilteredMR(listOption);
        }

        private void btnCancelDocument_Click(object sender, EventArgs e)
        {
            try
            {
                removeControlsFromlvPanel();
                MovementRegisterDB mrdb = new MovementRegisterDB();

                DialogResult dialog = MessageBox.Show("Are you sure to Cancel the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    int opt = 0;
                    if (listOption == 1)
                    {
                        opt = 1;
                        string remark = Login.userLoggedInName + " : " +
                           UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Document cancelled by the User";
                        prevmr.Comments = txtComment.Text.Replace("'", "''") + Environment.NewLine + remark;
                    }
                    else
                    {
                        opt = 2;
                        string remark = Login.userLoggedInName + " : " +
                            UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Document cancelled by the approver";
                        prevmr.Comments = txtComment.Text.Replace("'", "''") + Environment.NewLine + remark;
                    }
                    if (mrdb.CancelMovementReg(prevmr, opt))
                    {
                        MessageBox.Show("Movement Cancelled");
                        clearData();
                        pnlInner.Visible = false;
                        pnlOuter.Visible = false;
                        setButtonVisibility("btnEditPanel");
                        listOption = 1;
                        ListFilteredMR(listOption);
                        btnApprovalPending.Focus();
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
                removeControlsFromlvPanel();
                MovementRegisterDB mrdb = new MovementRegisterDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Reject the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    //if (txtComment.Text.Length == 0)
                    //{
                    //    MessageBox.Show("Give the comment For Rejection");
                    //    return;
                    //}
                    string remark = Login.userLoggedInName +
                               " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Movement Rejected" + "\n";
                    prevmr.Comments = txtComment.Text.Replace("'","''") + Environment.NewLine + remark;
                    if (mrdb.RejectMovementReg(prevmr))
                    {
                        MessageBox.Show("Movement Rejected");
                        clearData();
                        pnlInner.Visible = false;
                        pnlOuter.Visible = false;
                        setButtonVisibility("btnEditPanel");
                        listOption = 1;
                        ListFilteredMR(listOption);
                        btnApprovalPending.Focus();
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
            try
            {
                //removeControlsFrompnllvPanel();

                commentText = new RichTextBox();
                //pnllv = new Panel();
                //pnllv.BorderStyle = BorderStyle.FixedSingle;
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.CadetBlue;

                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(450, 300);
                //pnllv.Bounds = new Rectangle(new Point(200, 30), new Size(400, 300));
                //pnllv.BackColor = Color.Gray;
                commentText.Bounds = new Rectangle(new Point(25, 25), new Size(350, 200));
                frmPopup.Controls.Add(commentText);

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
                commentText.Focus();
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
                //removeControlsFromlvPanel();
            }
        }
        private void lvOK_Click5(object sender, EventArgs e)
        {
            try
            {
                pnllv.Visible = true;
                if (commentText.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please Add Comment");
                    return;
                }
                txtComment.Text = txtComment.Text + Environment.NewLine + Login.userLoggedInName +
                    " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + commentText.Text;
                //btnAddComment.Enabled = true;
                //pnllv.Visible = false;
                txtComment.ReadOnly = true;
                ContextMenu blankContextMenu = new ContextMenu();
                txtComment.ContextMenu = blankContextMenu;
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
                //btnAddComment.Enabled = true;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        ////////public string valuetostring(int stat)
        ////////{
        ////////    string status = "";
        ////////    if (stat == 1)
        ////////    {
        ////////        status = "Created";
        ////////    }
        ////////    else if (stat == 2)
        ////////    {
        ////////        status = "Approved";
        ////////    }
        ////////    else if (stat == 3)
        ////////    {
        ////////        status = "Out";
        ////////    }
        ////////    else if (stat == 10)
        ////////    {
        ////////        status = "Rejected";
        ////////    }
        ////////    else if (stat == 99)
        ////////    {
        ////////        status = "In";
        ////////    }
        ////////    else if (stat == 98)
        ////////    {
        ////////        status = "Canceled";
        ////////    }
        ////////    return status;
        ////////}
        ////////public int stringtovalue(string status)
        ////////{
        ////////    int stat = 0;
        ////////    if (status.Equals("Created"))
        ////////    {
        ////////        stat = 1;
        ////////    }
        ////////    else if (status.Equals("Approved"))
        ////////    {
        ////////        stat = 2;
        ////////    }
        ////////    else if (status.Equals("Out"))
        ////////    {
        ////////        stat = 3;
        ////////    }
        ////////    else if (status.Equals("Rejected"))
        ////////    {
        ////////        stat = 10;
        ////////    }
        ////////    else if (status.Equals("In"))
        ////////    {
        ////////        stat = 99;
        ////////    }
        ////////    else if (status.Equals("Canceled"))
        ////////    {
        ////////        stat = 98;
        ////////    }
        ////////    return stat;
        ////////}

        private void txtComment_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void ApprovalsGiven_Click(object sender, EventArgs e)
        {
            listOption = 4;
            ListFilteredMR(listOption);
        }

        private void cmbModeOfTransport_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pnlInner_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnReceived_Click_1(object sender, EventArgs e)
        {
            listOption = 3;
            ListFilteredMR(listOption);
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
        private Boolean checkLVCopyItemChecked(string str)
        {
            Boolean status = true;
            try
            {
                if (lvCopy.CheckedIndices.Count > 1)
                {
                    MessageBox.Show("Only one " + str + " allowed");
                    return false;
                }
                if (lvCopy.CheckedItems.Count == 0)
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

        private void btnCancelApprvdMvmnt_Click(object sender, EventArgs e)
        {
            try
            {
                removeControlsFromlvPanel();
                MovementRegisterDB mrdb = new MovementRegisterDB();
                string remark = Login.userLoggedInName +
                              " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Request for Cancel" + "\n";
                prevmr.Comments = txtComment.Text.Replace("'", "''") + Environment.NewLine + remark;
                DialogResult dialog = MessageBox.Show("Are you sure to get the document Cancelled?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (mrdb.CancelReqforApprovedMR(prevmr))
                    {
                        MessageBox.Show("Cancel Request Sent");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredMR(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                        btnApprovalPending.Focus();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnAccptMovCancel_Click(object sender, EventArgs e)
        {
            try
            {
                removeControlsFromlvPanel();
                MovementRegisterDB mrdb = new MovementRegisterDB();
                string remark = Login.userLoggedInName +
                              " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Cancel Request Approved" + "\n";
                prevmr.Comments = txtComment.Text.Replace("'", "''") + Environment.NewLine + remark;
                DialogResult dialog = MessageBox.Show("Are you sure to Cancel the Approved document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (mrdb.ApproveReqforCancelledMR(prevmr))
                    {
                        MessageBox.Show("Cancel Request Approved");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredMR(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                        btnApprovalPending.Focus();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnRejMovCancel_Click(object sender, EventArgs e)
        {
            try
            {
                removeControlsFromlvPanel();
                MovementRegisterDB mrdb = new MovementRegisterDB();
                string remark = Login.userLoggedInName +
                              " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Cancel Request Rejected" + "\n";
                prevmr.Comments = txtComment.Text.Replace("'", "''") + Environment.NewLine + remark;
                DialogResult dialog = MessageBox.Show("Are you sure to Reject the document Cancel ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (mrdb.RejReqforCancelledMR(prevmr))
                    {
                        MessageBox.Show("Cancel Request Rejected");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredMR(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                        btnApprovalPending.Focus();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void MovementRegister_Enter(object sender, EventArgs e)
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

