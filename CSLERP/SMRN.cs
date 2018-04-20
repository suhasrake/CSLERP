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
    public partial class SMRN : System.Windows.Forms.Form
    {
        string docID = "SMRN";
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        smrn prevsh = new smrn();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        Panel pnlForwarder = new Panel();
        ListView lvApprover = new ListView();
        Panel pnlCmtr = new Panel();
        Form frmPopup = new Form();
        public SMRN()
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
            ListFilteredSMRN(listOption);
            //applyPrivilege();
        }
        private void ListFilteredSMRN(int option)
        {
            try
            {
                grdList.Rows.Clear();
                SMRNDB smrnhdb = new SMRNDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                List<smrn> SMRNList = smrnhdb.getFilteredSMRNHeader(userString, option);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3)
                    lblActionHeader.Text = "List of Approved Documents";

                foreach (smrn smrnh in SMRNList)
                {
                    if (option == 1)
                    {
                        if (smrnh.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentID"].Value = smrnh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentName"].Value = smrnh.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["SMRNNo"].Value = smrnh.SMRNNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["SMRNDate"].Value = smrnh.SMRNDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["CustomerID"].Value = smrnh.CustomerID;
                    grdList.Rows[grdList.RowCount - 1].Cells["CustomerName"].Value = smrnh.CustomerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["CustomerDocumentNo"].Value = smrnh.CustomerDocumentNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["CustomerDocumentDate"].Value = smrnh.CustomerDocumentDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["CourierID"].Value = smrnh.CourierID;
                    grdList.Rows[grdList.RowCount - 1].Cells["CourierName"].Value = smrnh.CourierName;
                    grdList.Rows[grdList.RowCount - 1].Cells["NoOfPacket"].Value = smrnh.NoOfPackets;
                    grdList.Rows[grdList.RowCount - 1].Cells["Remarks"].Value = smrnh.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = smrnh.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatus"].Value = smrnh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = smrnh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = smrnh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwardUser"].Value = smrnh.ForwardUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ApproveUser"].Value = smrnh.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = smrnh.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = smrnh.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = smrnh.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = smrnh.ForwarderList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in SMRN Header listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;

            grdList.Columns["Creator"].Visible = true;
            grdList.Columns["Forwarder"].Visible = true;
            grdList.Columns["Approver"].Visible = true;
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
                CustomerDB.fillCustomerComboNew(cmbCustomer);
                CustomerDB.fillCustomerComboNew(cmbCourierID);
                dtSMRNDate.Format = DateTimePickerFormat.Custom;
                dtSMRNDate.CustomFormat = "dd-MM-yyyy";
                dtSMRNDate.Enabled = false;
                dtCustomerDocDate.Format = DateTimePickerFormat.Custom;
                dtCustomerDocDate.CustomFormat = "dd-MM-yyyy";
                //dtCustomerDocDate.Value = DateTime.Now;
                txtSMRNNo.Enabled = false;
                userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
                setButtonVisibility("init");
            }
            catch (Exception)
            {

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
                //pnlForwarder.Visible = false;
                cmbCustomer.SelectedIndex = -1;
                cmbCourierID.SelectedIndex = -1;
                dtSMRNDate.CustomFormat = "dd-MM-yyyy";
                dtCustomerDocDate.CustomFormat = "dd-MM-yyyy";
                dtSMRNDate.Value = DateTime.Parse("01-01-1900");
                dtCustomerDocDate.Value = DateTime.Today.Date;
                txtCuxtomerDocNo.Text = "";
                txtSMRNNo.Text = "";
                txtRemarks.Text = "";
                txtNoOfPacket.Text = "";
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
                closeAllPanels();
                clearData();
                btnSave.Text = "Save";
                pnlOuter.Visible = true;
                pnlInner.Visible = true;
                setButtonVisibility("btnNew");
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
                SMRNDB smrnhDB = new SMRNDB();
                smrn smrnh = new smrn();
                try
                {
                    ////////smrnh.CourierID = cmbCourierID.SelectedItem.ToString().Trim().Substring(0,cmbCourierID.SelectedItem.ToString().Trim().IndexOf('-'));
                    smrnh.CourierID = ((Structures.ComboBoxItem)cmbCourierID.SelectedItem).HiddenValue;
                    //////////smrnh.CustomerID = cmbCustomer.SelectedItem.ToString().Trim().Substring(0, cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-'));
                    smrnh.CustomerID = ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue;
                }
                catch (Exception)
                {
                    smrnh.CustomerID = "";
                    smrnh.CustomerName = "";
                }
                smrnh.DocumentID = docID;
                
                smrnh.SMRNDate = dtSMRNDate.Value;
                smrnh.CustomerDocumentDate = dtCustomerDocDate.Value;
                smrnh.Remarks = txtRemarks.Text;
                smrnh.NoOfPackets = Convert.ToInt32(txtNoOfPacket.Text);
                smrnh.CustomerDocumentNo = txtCuxtomerDocNo.Text;
                
                string btnText = btnSave.Text;
                if (btnText.Equals("Update"))
                {
                    smrnh.ForwarderList = prevsh.ForwarderList;
                    smrnh.SMRNNo = Convert.ToInt32(txtSMRNNo.Text);
                    if (smrnhDB.validateSMRN(smrnh))
                    {
                        if (smrnhDB.updateSMRN(smrnh, prevsh))
                        {
                            MessageBox.Show("SMRN updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredSMRN(listOption);
                        }
                        else
                        {
                            status = false;
                            MessageBox.Show("Failed to update SMRN");
                        }
                    }
                    else
                    {
                        status = false;
                        MessageBox.Show("Smrn DEtail Entered Wrong");
                    }

                }
                else if (btnText.Equals("Save"))
                {

                    smrnh.DocumentStatus = 1;//created
                    smrnh.SMRNDate = UpdateTable.getSQLDateTime();
                    smrnh.SMRNNo = DocumentNumberDB.getNewNumber(docID, 1);
                    if (smrnhDB.validateSMRN(smrnh))
                    {
                        if (smrnhDB.insertSMRN(smrnh))
                        {
                            MessageBox.Show("SMRN Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredSMRN(listOption);
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert SMRN");
                            status = false;

                        }
                    }
                    else
                    {
                        status = false;
                        MessageBox.Show("Smrn DEtail Entered Wrong");
                    }
                }
                else
                {
                    status = false;
                    MessageBox.Show("SMRN Data Operation failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed Adding / Editing SMRN");
                status = false;
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }
        //private void btnSave_Click(object sender, EventArgs e)
        //{
            
        //}

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("View"))

                {
                    clearData();
                    setButtonVisibility(columnName);
                    
                    prevsh = new smrn();
                    int rowID = e.RowIndex;
                    //prevpheader.ConversionDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells[0].Value.ToString());
                    try
                    {
                        prevsh.DocumentID = grdList.Rows[e.RowIndex].Cells["DocumentID"].Value.ToString();
                        prevsh.DocumentName = grdList.Rows[e.RowIndex].Cells["DocumentName"].Value.ToString();
                        prevsh.SMRNNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["SMRNNo"].Value.ToString());
                        prevsh.SMRNDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["SMRNDate"].Value.ToString());
                        prevsh.ForwardUser = grdList.Rows[e.RowIndex].Cells["ForwardUser"].Value.ToString();

                        if(Login.userLoggedIn == prevsh.ForwardUser)
                        {
                            pnlInner.Enabled = false;
                            btnSave.Visible = false;
                        }

                        prevsh.CustomerDocumentNo = grdList.Rows[e.RowIndex].Cells["CustomerDocumentNo"].Value.ToString();
                        prevsh.CustomerDocumentDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["CustomerDocumentDate"].Value.ToString());
                        prevsh.CustomerID = grdList.Rows[e.RowIndex].Cells["CustomerID"].Value.ToString();
                        prevsh.CustomerName = grdList.Rows[e.RowIndex].Cells["CustomerName"].Value.ToString();
                        prevsh.CourierID = grdList.Rows[e.RowIndex].Cells["CourierID"].Value.ToString();
                        prevsh.CourierName = grdList.Rows[e.RowIndex].Cells["CourierName"].Value.ToString();
                        prevsh.Remarks = grdList.Rows[e.RowIndex].Cells["Remarks"].Value.ToString();
                        prevsh.NoOfPackets = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["NoOfPacket"].Value.ToString());
                        //prevsh.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString());
                        //prevsh.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocStat"].Value.ToString());
                        prevsh.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["CreateTime"].Value.ToString());
                        prevsh.CreateUser = grdList.Rows[e.RowIndex].Cells["CreateUser"].Value.ToString();
                        prevsh.ApproveUser = grdList.Rows[e.RowIndex].Cells["ApproveUser"].Value.ToString();

                        prevsh.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                        prevsh.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                        prevsh.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                        prevsh.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                        prevsh.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentStatus"].Value.ToString());
                        prevsh.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString());
                    }
                    catch (Exception ex)
                    {
                      
                    }
                    btnSave.Text = "Update";
                    pnlInner.Visible = true;
                    pnlOuter.Visible = true;
                    pnlList.Visible = false;
                    //cmbStatus.Enabled = true;

                    txtSMRNNo.Text = prevsh.SMRNNo.ToString();
                    // DateTime ttmp = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["TemporaryDate"].Value.ToString());

                    dtSMRNDate.Value = prevsh.SMRNDate;
                    txtCuxtomerDocNo.Text = prevsh.CustomerDocumentNo;
                    try
                    {
                        dtCustomerDocDate.Value = prevsh.CustomerDocumentDate;
                    }
                    catch (Exception ex)
                    {
                        dtCustomerDocDate.Value = DateTime.Parse("01-01-1900");
                    }

                    //dtTrackingDate.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["TrackingDate"].Value.ToString());
                    ////////cmbCustomer.SelectedIndex = cmbCustomer.FindString(prevsh.CustomerID);
                    cmbCustomer.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCustomer, prevsh.CustomerID);
                    ////////cmbCourierID.SelectedIndex = cmbCourierID.FindString(prevsh.CourierID);
                    cmbCourierID.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCourierID, prevsh.CourierID);
                    txtNoOfPacket.Text = prevsh.NoOfPackets.ToString();
                    txtRemarks.Text = prevsh.Remarks;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void btnForward_Click_1(object sender, EventArgs e)
        {
            //removeControlsFromForwarderPanel();
            //lvApprover = new ListView();
            ////lvApprover.Enabled = true;
            //lvApprover.Clear();
            //pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
            //pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(500, 300));
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            string DeptList = "Production" + Main.delimiter1 + "QC" + Main.delimiter1 + "ProductSupport";
            lvApprover = DocEmpMappingDB.ApproverLV(docID, Login.empLoggedIn);
            lvApprover.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            ///pnlForwarder.Controls.Remove(lvApprover);
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
            //pnlOuter.Controls.Add(pnlForwarder);
            //pnlOuter.BringToFront();
            //pnlForwarder.BringToFront();
            //pnlForwarder.Focus();
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
        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredSMRN(listOption);
        }
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

            ListFilteredSMRN(listOption);
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
                            SMRNDB smrnhDB = new SMRNDB();
                            prevsh.ForwardUser = approverUID;
                            prevsh.ForwarderList = prevsh.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (smrnhDB.forwardSMRN(prevsh))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(prevsh, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                pnlForwarder.Controls.Remove(lvApprover);
                                pnlForwarder.Visible = false;
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredSMRN(listOption);
                                setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
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
        private Boolean updateDashBoard(smrn smrn, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = smrn.DocumentID;
                dsb.TemporaryNo = smrn.SMRNNo;
                dsb.TemporaryDate = smrn.SMRNDate;
                //dsb.DocumentNo = smrn.ProductionPlanNo;
                //dsb.DocumentDate = smrn.ProductionPlanDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = smrn.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(prevsh.DocumentID);
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
                lblActionHeader.Visible = true;
                pnlTopButtons.Visible = true;
                pnlInner.Enabled = true;
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
                    btnSave.Visible = true;
                }
                else if (btnName == "Approve")
                {
                    pnlTopButtons.Visible = false;
                    lblActionHeader.Visible = false;
                    pnlInner.Enabled = false;
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
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
        private void pnlPDFViewer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {

        }

        private void btnApprovalPending_Click_1(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredSMRN(listOption);
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                SMRNDB smrnhdb = new SMRNDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    //prevsh.TrackingNo = DocumentNumberDB.getNewNumber(docID, 2);
                    if (smrnhdb.ApproveSMRN(prevsh))
                    {
                        MessageBox.Show("SMRN Approved");
                        if (!updateDashBoard(prevsh, 2))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                        }
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredSMRN(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnReverse_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Are you sure to Reverse the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    string reverseStr = getReverseString(prevsh.ForwarderList);
                    //do forward activities
                    //prevpopi.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevpopi.CommentStatus);
                    SMRNDB smrnhdb = new SMRNDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevsh.ForwarderList = reverseStr.Substring(0, ind);
                        prevsh.ForwardUser = reverseStr.Substring(ind + 3);
                        prevsh.DocumentStatus = prevsh.DocumentStatus - 1;
                    }
                    else
                    {
                        prevsh.ForwarderList = "";
                        prevsh.ForwardUser = "";
                        prevsh.DocumentStatus = 1;
                    }
                    if (smrnhdb.reverseSMRN(prevsh))
                    {
                        MessageBox.Show(" SMRN  Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredSMRN(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
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

        private void SMRN_Enter(object sender, EventArgs e)
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

