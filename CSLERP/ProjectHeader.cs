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
    public partial class ProjectHeader : System.Windows.Forms.Form
    {
        string docID = "PROJECT";
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        projectheader prevpheader ;
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        Panel pnlForwarder = new Panel();
        ListView lvApprover = new ListView();
        Panel pnlCmtr = new Panel();
        Form frmPopup = new Form();
        public ProjectHeader()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }

        private void ProjectHeader_Load(object sender, EventArgs e)
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
            ListFilteredProductHeader(listOption);
            //applyPrivilege();
        }

        private void ListFilteredProductHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                ProjectHeaderDB phdb = new ProjectHeaderDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                List<projectheader> pheaderlist = phdb.getFilteredProjectHeader(userString, option);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3)
                    lblActionHeader.Text = "List of Approved Documents";

                foreach (projectheader pheader in pheaderlist)
                {
                    if (option == 1)
                    {
                        if (pheader.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentID"].Value = pheader.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentName"].Value = pheader.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["TemporaryNo"].Value = pheader.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["TemporaryDate"].Value = pheader.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["TrackingNo"].Value = pheader.TrackingNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["TrackingDate"].Value = pheader.TrackingDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProjectID"].Value = pheader.ProjectID;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProjectManager"].Value = pheader.ProjectManager;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProjectManagerName"].Value = pheader.ProjectManagerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ShortDescription"].Value = pheader.ShortDescription;
                    grdList.Rows[grdList.RowCount - 1].Cells["CustomerID"].Value = pheader.CustomerID;
                    grdList.Rows[grdList.RowCount - 1].Cells["CustomerName"].Value = pheader.CustomerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["OfficeID"].Value = pheader.OfficeID;
                    grdList.Rows[grdList.RowCount - 1].Cells["OfficeName"].Value = pheader.OfficeName;
                    grdList.Rows[grdList.RowCount - 1].Cells["StartDate"].Value = pheader.StartDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["TargetDate"].Value = pheader.TargetDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = pheader.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatus"].Value = ComboFIll.getDocumentStatusString(pheader.DocumentStatus);
                    grdList.Rows[grdList.RowCount - 1].Cells["DocStat"].Value = pheader.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = pheader.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = pheader.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwardUser"].Value = pheader.ForwardUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ApproveUser"].Value = pheader.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = pheader.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = pheader.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = pheader.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwarderLists"].Value = pheader.ForwarderList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in product Header listing");
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
                ProjectHeaderDB.fillCustomerForProjectComboNew(cmbCustomer);
                EmployeeDB.fillEmpListCombo(cmbProjectManager);
                OfficeDB.fillOfficeComboNew(cmbOfficeID);
                fillStatusCombo(cmbStatus);
                dtTemporaryDate.Format = DateTimePickerFormat.Custom;
                dtTemporaryDate.CustomFormat = "dd-MM-yyyy";
                dtTemporaryDate.Enabled = false;
                dtTrackingDate.Format = DateTimePickerFormat.Custom;
                dtTrackingDate.CustomFormat = "dd-MM-yyyy";
                dtTrackingDate.Enabled = false;
                dtStartDate.Format = DateTimePickerFormat.Custom;
                dtStartDate.CustomFormat = "dd-MM-yyyy";
                //dtStartDate.Enabled = false;
                dtTargetDate.Format = DateTimePickerFormat.Custom;
                dtTargetDate.CustomFormat = "dd-MM-yyyy";
                //dtTargetDate.Enabled = false;
                txtTemporaryNo.Enabled = false;
                txtTrackingNo.Enabled = false;
                userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
                setButtonVisibility("init");
                setTabIndex();
            }
            catch (Exception)
            {

            }
        }
        private void setTabIndex()
        {
            txtTemporaryNo.TabIndex = 0;
            dtTemporaryDate.TabIndex = 1;
            txtTrackingNo.TabIndex = 2;
            dtTrackingDate.TabIndex = 3;
            cmbCustomer.TabIndex = 4;
            txtProjectID.TabIndex = 5;
            cmbProjectManager.TabIndex = 6;
            cmbOfficeID.TabIndex = 7;
            dtStartDate.TabIndex = 8;
            dtTargetDate.TabIndex = 9;
            txtShortDescription.TabIndex = 10;

            btnApprove.TabIndex = 0;
            btnForward.TabIndex = 1;
            btnCancel.TabIndex = 2;
            btnSave.TabIndex = 3;
            btnReverse.TabIndex = 4;
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
                removeControlsFromForwarderPanel();
                cmbCustomer.SelectedIndex = -1;
                cmbProjectManager.SelectedIndex = -1;
                dtStartDate.CustomFormat = "dd-MM-yyyy";
                dtTargetDate.CustomFormat = "dd-MM-yyyy";
                dtTemporaryDate.CustomFormat = "dd-MM-yyyy";
                dtTrackingDate.CustomFormat = "dd-MM-yyyy";

                dtStartDate.Value = DateTime.Today.Date;
                dtTargetDate.Value = DateTime.Today.Date;
                dtTemporaryDate.Value = DateTime.Parse("01-01-1900");
                dtTrackingDate.Value = DateTime.Parse("01-01-1900");
                txtTrackingNo.Text = "";
                txtTemporaryNo.Text = "";
                txtProjectID.Text = "";
                txtShortDescription.Text = "";
                cmbOfficeID.SelectedIndex = -1;
                prevpheader = new projectheader();
               
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
                ProjectHeaderDB phdb = new ProjectHeaderDB();
                projectheader pheader = new projectheader();
                try
                {
                    pheader.ProjectManager = cmbProjectManager.SelectedItem.ToString().Trim().Substring(cmbProjectManager.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                    //pheader.ProjectManagerName = cmbCustomer.SelectedItem.ToString().Trim().Substring(cmbProjectManager.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                    //////////pheader.CustomerID = cmbCustomer.SelectedItem.ToString().Trim().Substring(0, cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-'));
                    pheader.CustomerID = ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue;
                    
                    //pheader.CustomerName = cmbCustomer.SelectedItem.ToString().Trim().Substring(cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                }
                catch (Exception)
                {
                    pheader.CustomerID = "";
                    pheader.CustomerName = "";
                }
                pheader.DocumentID = docID;
                pheader.OfficeID = ((Structures.ComboBoxItem)cmbOfficeID.SelectedItem).HiddenValue;
                //pheader.TemporaryDate = UpdateTable.getSQLDateTime();
                pheader.TrackingDate = dtTrackingDate.Value;
                pheader.ProjectID = txtProjectID.Text;
                pheader.ShortDescription = txtShortDescription.Text;
                pheader.StartDate = dtStartDate.Value;
                pheader.TargetDate = dtTargetDate.Value;
                // pheader.Status = ComboFIll.getStatusCode(cmbStatus.SelectedItem.ToString());
                pheader.ForwarderList = prevpheader.ForwarderList;
                string btnText = btnSave.Text;

                //Replacing single quotes
                pheader = verifyHeaderInputString(pheader);

                if (btnText.Equals("Update"))
                {
                    if (phdb.validateProjectHeader(pheader))
                    {
                        if (phdb.updateProjectHeader(pheader, prevpheader))
                        {
                            MessageBox.Show("Project Header updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredProductHeader(listOption);
                        }
                        else
                        {
                            status = false;
                            MessageBox.Show("Failed to update Project Header");
                        }
                    }
                    else
                    {
                        status = false;
                        MessageBox.Show("Failed to update Project Header");
                    }

                }
                else if (btnText.Equals("Save"))
                {

                    pheader.DocumentStatus = 1;//created
                    pheader.TemporaryDate = UpdateTable.getSQLDateTime();
                    pheader.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    if (phdb.validateProjectHeader(pheader))
                    {
                        if (phdb.insertProjectHeader(pheader))
                        {
                            MessageBox.Show("Project Header Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredProductHeader(listOption);
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Project Header");
                            status = false;

                        }
                    }
                    else
                    {
                        status = false;
                        MessageBox.Show("Project heder entered wrong.");
                    }
                }
                else
                {
                    status = false;
                    MessageBox.Show("Project Header Data operation failed");
                }
            }
            catch (Exception)
            {
                status = false;
                MessageBox.Show("Failed Adding / Editing Project Header");
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
                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("LoadDocument") ||
                    columnName.Equals("View") || columnName.Equals("ViewDocument"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    
                    prevpheader = new projectheader();
                    int rowID = e.RowIndex;
                    //prevpheader.ConversionDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells[0].Value.ToString());
                    try
                    {
                        prevpheader.DocumentID = grdList.Rows[e.RowIndex].Cells["DocumentID"].Value.ToString();
                        prevpheader.DocumentName = grdList.Rows[e.RowIndex].Cells["DocumentName"].Value.ToString();
                        prevpheader.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["TemporaryNo"].Value.ToString());
                        prevpheader.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["TemporaryDate"].Value.ToString());
                        prevpheader.TrackingNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["TrackingNo"].Value.ToString());
                        prevpheader.TrackingDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["TrackingDate"].Value.ToString());
                        prevpheader.CustomerID = grdList.Rows[e.RowIndex].Cells["CustomerID"].Value.ToString();
                        prevpheader.OfficeID = grdList.Rows[e.RowIndex].Cells["OfficeID"].Value.ToString();
                        prevpheader.CustomerName = grdList.Rows[e.RowIndex].Cells["CustomerName"].Value.ToString();
                        //--------Load Documents
                        if (columnName.Equals("LoadDocument"))
                        {
                            string hdrString = "Project Temp No:" + prevpheader.TemporaryNo + "\n" +
                                "Project Temp Date:" + prevpheader.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                                "Tracking No:" + prevpheader.TrackingNo + "\n" +
                                "Tracking Date:" + prevpheader.TrackingDate.ToString("dd-MM-yyyy") + "\n" +
                                "Customer:" + prevpheader.CustomerName;
                            string dicDir = Main.documentDirectory + "\\" + docID;
                            string subDir = prevpheader.TemporaryNo + "-" + prevpheader.TemporaryDate.ToString("yyyyMMddhhmmss");
                            FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                            load.ShowDialog();
                            this.RemoveOwnedForm(load);
                            btnCancel_Click_1(null, null);
                            return;
                        }
                        //--------
                        if (columnName.Equals("ViewDocument"))
                        {                    
                            return;
                        }
                        prevpheader.ProjectID = grdList.Rows[e.RowIndex].Cells["ProjectID"].Value.ToString();
                        prevpheader.ProjectManager = grdList.Rows[e.RowIndex].Cells["ProjectManager"].Value.ToString();
                        prevpheader.ProjectManagerName = grdList.Rows[e.RowIndex].Cells["ProjectManagerName"].Value.ToString();
                        prevpheader.ShortDescription = grdList.Rows[e.RowIndex].Cells["ShortDescription"].Value.ToString();
                        
                        prevpheader.StartDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["StartDate"].Value.ToString());
                        prevpheader.TargetDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["TargetDate"].Value.ToString());
                        prevpheader.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString());
                        prevpheader.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocStat"].Value.ToString());
                        prevpheader.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["CreateTime"].Value.ToString());
                        prevpheader.CreateUser = grdList.Rows[e.RowIndex].Cells["CreateUser"].Value.ToString();
                        prevpheader.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                        prevpheader.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                        prevpheader.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                        prevpheader.ForwarderList = grdList.Rows[e.RowIndex].Cells["ForwarderLists"].Value.ToString();
                    }
                    catch (Exception ex)
                    {
                      
                    }
                    btnSave.Text = "Update";
                    pnlInner.Visible = true;
                    pnlOuter.Visible = true;
                    pnlList.Visible = false;
                    //cmbStatus.Enabled = true;

                    txtTemporaryNo.Text = grdList.Rows[e.RowIndex].Cells["TemporaryNo"].Value.ToString();
                    DateTime ttmp = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["TemporaryDate"].Value.ToString());

                    dtTemporaryDate.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["TemporaryDate"].Value.ToString());
                    txtTrackingNo.Text = grdList.Rows[e.RowIndex].Cells["TrackingNo"].Value.ToString();
                    try
                    {
                        dtTrackingDate.Value = prevpheader.TrackingDate;
                    }
                    catch (Exception)
                    {
                        dtTrackingDate.Value = DateTime.Parse("01-01-1900");
                    }

                    //dtTrackingDate.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["TrackingDate"].Value.ToString());
                    cmbCustomer.SelectedIndex = cmbCustomer.FindStringExact(grdList.Rows[e.RowIndex].Cells["CustomerID"].Value.ToString() + "-" + grdList.Rows[e.RowIndex].Cells["CustomerName"].Value.ToString());
                    cmbCustomer.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCustomer, grdList.Rows[e.RowIndex].Cells["CustomerID"].Value.ToString());
                    cmbOfficeID.SelectedIndex =
                       Structures.ComboFUnctions.getComboIndex(cmbOfficeID, grdList.Rows[e.RowIndex].Cells["OfficeID"].Value.ToString());
                    //cmbStatus.SelectedIndex = cmbStatus.FindStringExact(ComboFIll.getStatusString(Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString())));
                    txtProjectID.Text = grdList.Rows[e.RowIndex].Cells["ProjectID"].Value.ToString();
                    ////////cmbProjectManager.SelectedIndex = cmbProjectManager.FindString(grdList.Rows[e.RowIndex].Cells["ProjectManagerName"].Value.ToString() + "-" + grdList.Rows[e.RowIndex].Cells["ProjectManager"].Value.ToString());
                    string pname = grdList.Rows[e.RowIndex].Cells["ProjectManagerName"].Value.ToString() + "-" + grdList.Rows[e.RowIndex].Cells["ProjectManager"].Value.ToString();
                    cmbProjectManager.SelectedIndex =
                        cmbProjectManager.FindStringExact(pname);
                    ////cmbProjectManager.SelectedIndex =
                    ////    Structures.ComboFUnctions.getComboIndex(cmbProjectManager, grdList.Rows[e.RowIndex].Cells["ProjectManagerName"].Value.ToString() + "-" + grdList.Rows[e.RowIndex].Cells["ProjectManager"].Value.ToString());
                    //dtStartDate.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["StartDate"].Value.ToString());
                    dtStartDate.Value = prevpheader.StartDate;
                    //dtTargetDate.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["TargetDate"].Value.ToString());
                    dtTargetDate.Value = prevpheader.TargetDate;
                    txtShortDescription.Text = grdList.Rows[e.RowIndex].Cells["ShortDescription"].Value.ToString();

                  
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void btnForward_Click_1(object sender, EventArgs e)
        {
            //removeControlsFromForwarderPanel();
            //vApprover = new ListView();
            //lvApprover.Enabled = true;
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
            lvApprover = DocEmpMappingDB.ApproverLV(docID, Login.empLoggedIn);
            lvApprover.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            // pnlForwarder.Controls.Remove(lvApprover);
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
        //private void btnForward_Click(object sender, EventArgs e)
        //{
            
        //}
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
            ListFilteredProductHeader(listOption);
        }
        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredProductHeader(listOption);
            //checkPoint = 1;
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

            ListFilteredProductHeader(listOption);
        }
        private void btnApprove_Click_1(object sender, EventArgs e)
        {
            try
            {
                ProjectHeaderDB phdb = new ProjectHeaderDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevpheader.TrackingNo = DocumentNumberDB.getNewNumber(docID, 2);
                    if (phdb.ApproveProjectHeader(prevpheader))
                    {
                        MessageBox.Show("Project Header Approved");
                        if (!updateDashBoard(prevpheader, 2))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                        }
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredProductHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        //private void btnApprove_Click(object sender, EventArgs e)
        //{
            
        //}
        

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
        private Boolean updateDashBoard(projectheader prh, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = prh.DocumentID;
                dsb.TemporaryNo = prh.TemporaryNo;
                dsb.TemporaryDate = prh.TemporaryDate;
                dsb.DocumentNo = prh.TrackingNo;
                dsb.DocumentDate = prh.TrackingDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = prh.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(prevpheader.DocumentID);
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
                            ProjectHeaderDB phdb = new ProjectHeaderDB();
                           
                            prevpheader.ForwardUser = approverUID;
                            prevpheader.ForwarderList = prevpheader.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (phdb.forwardProjectHeader(prevpheader))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(prevpheader, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                pnlForwarder.Controls.Remove(lvApprover);
                                pnlForwarder.Visible = false;
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredProductHeader(listOption);
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
        private void btnReverse_Click_1(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Are you sure to Reverse the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    string reverseStr = getReverseString(prevpheader.ForwarderList);
                    //do forward activities
                    //prevpopi.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevpopi.CommentStatus);
                    ProjectHeaderDB phdb = new ProjectHeaderDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevpheader.ForwarderList = reverseStr.Substring(0, ind);
                        prevpheader.ForwardUser = reverseStr.Substring(ind + 3);
                        prevpheader.DocumentStatus = prevpheader.DocumentStatus - 1;
                    }
                    else
                    {
                        prevpheader.ForwarderList = "";
                        prevpheader.ForwardUser = "";
                        prevpheader.DocumentStatus = 1;
                    }
                    if (phdb.reverseProjectHeader(prevpheader))
                    {
                        MessageBox.Show("ProjectHeader Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredProductHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        //private void btnReverse_Click(object sender, EventArgs e)
        //{
            
        //}
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
                //else if (btnName == "Commenter")
                //{
                //    btnCancel.Visible = true;
                //    btnSave.Visible = true;
                //}
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
                else if (btnName == "LoadDocument")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                }
                else if (btnName == "ViewDocument")
                {
                    pnlTopButtons.Visible = false;
                    btnNew.Visible = false;
                    btnActionPending.Visible = false;
                    btnApprovalPending.Visible = false;
                    btnApproved.Visible = false;
                    pnlInner.Visible = false;
                    pnlOuter.Visible = false;
                    grdList.Visible = false;
                    pnlPDFViewer.Visible = true;
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
        private void btnListDocument_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevpheader.TemporaryNo + "-" + prevpheader.TemporaryDate.ToString("yyyyMMddhhmmss"));
                pnlPDFViewer.Controls.Add(dgvDocumentList);
                dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
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
        private void btnClosePDF_Click(object sender, EventArgs e)
        {
            removePDFControls();
            showPDFFileGrid();
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
                    string subDir = prevpheader.TemporaryNo + "-" + prevpheader.TemporaryDate.ToString("yyyyMMddhhmmss");
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
        private void btnClose_Click_1(object sender, EventArgs e)
        {
            removePDFFileGridView();
            removePDFControls();
            pnlPDFViewer.Visible = false;
            pnlList.Visible = true;
            grdList.Visible = true;
            btnNew.Visible = true;
            btnExit.Visible = true;
            pnlTopButtons.Visible = true;
            btnActionPending.Visible = true;
            btnApprovalPending.Visible = true;
            btnApproved.Visible = true;
        }

        private void pnlPDFViewer_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlInner_Paint(object sender, PaintEventArgs e)
        {

        }

        //-- Validating Header and Detail String For Single Quotes

        private projectheader verifyHeaderInputString(projectheader pheader)
        {
            try
            {
                pheader.CustomerID = Utilities.replaceSingleQuoteWithDoubleSingleQuote(pheader.CustomerID);
                pheader.ProjectID = Utilities.replaceSingleQuoteWithDoubleSingleQuote(pheader.ProjectID);
                pheader.ShortDescription = Utilities.replaceSingleQuoteWithDoubleSingleQuote(pheader.ShortDescription);
            }
            catch (Exception ex)
            {
            }
            return pheader;
        }

        private void ProjectHeader_Enter(object sender, EventArgs e)
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

