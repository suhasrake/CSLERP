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
    public partial class SMRNServicedList : System.Windows.Forms.Form
    {
        string docID = "SMRNSERVICEDLIST";
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        string tempNo = "";
        DateTime tempDate = DateTime.Now;
        string userString = "";
        string userCommentStatusString = "";
        //string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        smrnservicedlist prevservlist;
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
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
        Form frmPopup = new Form();
        public SMRNServicedList()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void SMRNServicedList_Load(object sender, EventArgs e)
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
            ListFilteredSMRNServiceList(listOption);
            //applyPrivilege();
        }
        private void ListFilteredSMRNServiceList(int opt)
        {
            try
            {
                grdList.Rows.Clear();
                SMRNServicedListDB ServListDB = new SMRNServicedListDB();
                List<smrnservicedlist> ServList = ServListDB.getFilteredServicedList(userString, opt, userCommentStatusString);
                if (opt == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (opt == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (opt == 3 || opt == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (smrnservicedlist list in ServList)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = list.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["ListNo"].Value = list.ListNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["ListDate"].Value = list.ListDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTrackingNo"].Value = list.TrackingNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTrackingdate"].Value = list.TrackingDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerPONo"].Value = list.CustomerPONo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerPODate"].Value = list.CustomerPODate;
                    grdList.Rows[grdList.RowCount - 1].Cells["Customer"].Value = list.CustomerID;
                    //grdList.Rows[grdList.RowCount - 1].Cells["JobIDNo"].Value = list.JobIDNo;
                    //grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = list.Status;
                    //grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatus"].Value = list.DocumentStatus;
                    //grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = list.CreateTime;
                    //grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = list.CreateUser;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in SMRNService Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;

            //try
            //{
            //    //grdList.Columns["gCreateUser"].Visible = true;
            //    // grdList.Columns["Forwarder"].Visible = true;
            //    // grdList.Columns["Approver"].Visible = true;
            //}
            //catch (Exception ex)
            //{
            //}
        }

        private void initVariables()
        {

            if (getuserPrivilegeStatus() == 1)
            {
                //user is only a viewer
                listOption = 1;
            }
            docID = Main.currentDocument;
            dtListDate.Format = DateTimePickerFormat.Custom;
            dtListDate.CustomFormat = "dd-MM-yyyy";
            dtListDate.Enabled = false;
            dtTrackingDate.Format = DateTimePickerFormat.Custom;
            dtTrackingDate.CustomFormat = "dd-MM-yyyy";
            dtCustPODate.Format = DateTimePickerFormat.Custom;
            dtCustPODate.CustomFormat = "dd-MM-yyyy";

            ////dtSMRNHeaderDate.Enabled = true;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
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
        //private void applyPrivilege()
        //{
        //    try
        //    {
        //        if (Main.itemPriv[1])
        //        {
        //            btnNew.Visible = true;
        //        }
        //        else
        //        {
        //            btnNew.Visible = false;
        //        }
        //        if (Main.itemPriv[2])
        //        {
        //            grdList.Columns["Edit"].Visible = true;
        //        }
        //        else
        //        {
        //            grdList.Columns["Edit"].Visible = false;
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
                //clear all grid views
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                grdSMRNServList.Rows.Clear();
                //----------clear temperory panels
                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                //dtSMRNHeaderDate.Value = DateTime.Parse("01-01-1900");
                dtListDate.Value = DateTime.Parse("01-01-1900");
                txtListNo.Text = "";
                txtCustomer.Text = "";
                txtCustPONo.Text = "";
                txtTrackingNo.Text = "";
                btnSelectSMRNHeaderNo.Enabled = true;
                grdSMRNServList.ReadOnly = false;
                //txtSMRNHeaderNo.Text = "";
                prevservlist = new smrnservicedlist();
                removeControlsFromPnlLvPanel();
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
                btnSelectSMRNHeaderNo.Enabled = true;
                grdSMRNServList.ReadOnly = false;
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
        private void btnCancel_Click(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            Boolean status = true;
            try
            {
                List<smrnservicedlist> SMRNServList;
                SMRNServicedListDB ServListDB = new SMRNServicedListDB();
                smrnservicedlist ServList = new smrnservicedlist();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;
                try
                {
                    ServList.DocumentID = docID;
                    smrnservicedlist smrnserv;
                    SMRNServList = new List<smrnservicedlist>();
                    for (int i = 0; i < grdSMRNServList.Rows.Count; i++)
                    {
                        try
                        {
                            DataGridViewCheckBoxCell cell = grdSMRNServList.Rows[i].Cells["check"] as DataGridViewCheckBoxCell;
                            if (cell.Value != null)
                            {
                                if (Convert.ToBoolean(cell.Value) == true)
                                {
                                    smrnserv = new smrnservicedlist();
                                    smrnserv.SMRNHeaderNo = Convert.ToInt32(grdSMRNServList.Rows[i].Cells["gSMRNHeaderNo"].Value.ToString().Trim());
                                    smrnserv.SMRNHeaderDate = Convert.ToDateTime(grdSMRNServList.Rows[i].Cells["gSMRNHeaderDate"].Value.ToString().Trim());
                                    smrnserv.JobIDNo = Convert.ToInt32(grdSMRNServList.Rows[i].Cells["JobId"].Value.ToString().Trim());
                                    smrnserv.ListNo = Convert.ToInt32(grdSMRNServList.Rows[i].Cells["gTemporaryNo"].Value.ToString().Trim());     /// For storing TemporaryNo of SMRNDetail
                                    smrnserv.ListDate = Convert.ToDateTime(grdSMRNServList.Rows[i].Cells["gTemporaryDate"].Value.ToString().Trim()); ////for storing temporaryDate of SMRNdetail
                                    SMRNServList.Add(smrnserv);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("createAndUpdateSMRNDetails() : Error creating SMRN Details");
                            status = false;
                            return;
                        }
                    }
                    ServList.Status = 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    ServList.ListNo = DocumentNumberDB.getNewNumber(docID, 1);
                    ServList.DocumentStatus = 1;
                    ServList.ListDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    ServList.ListNo = Convert.ToInt32(txtListNo.Text);
                    ServList.DocumentStatus = prevservlist.DocumentStatus;
                    ServList.ListDate = prevservlist.ListDate;
                }
                if (btnText.Equals("Save"))
                {
                    if (ServListDB.updateSMRNServicedList(ServList, SMRNServList, 1))
                    {
                        MessageBox.Show("SMRNService List Details Added");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredSMRNServiceList(listOption);
                    }
                    else
                    {
                        status = false;
                    }
                    if (!status)
                    {
                        MessageBox.Show("Failed to Insert SMRNServiceList");
                        status = false;
                    }
                }
                else
                {
                    MessageBox.Show("SMRNServiceList Validation failed");
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("SMRNServiceList Validation failed");
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
                if (columnName.Equals("Finalize") || columnName.Equals("View") || columnName.Equals("Print"))
                {
                    if (columnName.Equals("Finalize"))
                    {
                        grdSMRNServList.Columns["Check"].Visible = false;
                        grdSMRNServList.ReadOnly = true;
                        btnSelectSMRNHeaderNo.Enabled = false;
                    }
                    clearData();
                    setButtonVisibility(columnName);
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    QIHeaderDB qidb = new QIHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevservlist = new smrnservicedlist();
                    prevservlist.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    prevservlist.ListNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["ListNo"].Value.ToString());
                    prevservlist.ListDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["ListDate"].Value.ToString());
                    prevservlist.TrackingNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTrackingNo"].Value.ToString());
                    prevservlist.TrackingDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gtrackingDate"].Value.ToString());
                    prevservlist.CustomerPONo = grdList.Rows[e.RowIndex].Cells["gCustomerPONo"].Value.ToString();
                    prevservlist.CustomerPODate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCustomerPODate"].Value.ToString());
                    prevservlist.CustomerID = grdList.Rows[e.RowIndex].Cells["Customer"].Value.ToString();
                    if (columnName.Equals("Print"))
                    {
                        pnlAddEdit.Visible = false;
                        pnlList.Visible = true;
                        grdList.Visible = true;
                        btnNew.Visible = true;
                        btnExit.Visible = true;
                        PrintSMRNServicedList printServ = new PrintSMRNServicedList();
                        List<smrndetail> SMRNDetail = SMRNServicedListDB.getSMRNDetailForServiceList(prevservlist, 1);
                        printServ.PrintServicedList(SMRNDetail, prevservlist);
                        btnNew.Visible = true;
                        btnExit.Visible = true;
                        return;
                    }
                    txtListNo.Text = prevservlist.ListNo.ToString();
                    txtTrackingNo.Text = prevservlist.TrackingNo.ToString();
                    txtCustomer.Text = prevservlist.CustomerID;
                    txtCustPONo.Text = prevservlist.CustomerPONo;
                    dtCustPODate.Value = prevservlist.CustomerPODate;
                    dtListDate.Value = prevservlist.ListDate;

                    if (!AddSMRNServListRowForFinalize(prevservlist))
                    {
                        MessageBox.Show("failed to add rows in grdSMRNServList");
                    }
                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;

                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private Boolean AddSMRNServListRowForFinalize(smrnservicedlist prevservlist)
        {
            Boolean status = true;
            try
            {
                List<smrndetail> SMRNDetail = SMRNServicedListDB.getSMRNDetailForServiceList(prevservlist, 1);
                grdSMRNServList.Rows.Clear();
                int i = 0;
                foreach (smrndetail smrnd in SMRNDetail)
                {
                    if (!AddSMRNDetailRow())
                    {
                        status = false;
                        MessageBox.Show("Error found in Adding rows to SMRNGrdiDetail.");
                    }
                    else
                    {
                        grdSMRNServList.Rows[i].Cells["Item"].Value = smrnd.StockItemID + "-" + smrnd.StockItemName;
                        grdSMRNServList.Rows[i].Cells["SerialNo"].Value = smrnd.SerialNo;
                        grdSMRNServList.Rows[i].Cells["ItemDetails"].Value = smrnd.ItemDetails;
                        grdSMRNServList.Rows[i].Cells["WarrantyStatus"].Value = showStatus(smrnd.WarrantyStatus);
                        grdSMRNServList.Rows[i].Cells["ProductServiceStatus"].Value = smrnd.ProductServiceStatus;
                        grdSMRNServList.Rows[i].Cells["gSMRNHeaderNo"].Value = smrnd.InspectionStatus;  // for SMRNHeaderNo
                        grdSMRNServList.Rows[i].Cells["gSMRNHeaderDate"].Value = smrnd.TemporaryDate;  // for SMRN HEader date
                        grdSMRNServList.Rows[i].Cells["gTemporaryNo"].Value = smrnd.TemporaryNo;
                        grdSMRNServList.Rows[i].Cells["gTemporarydate"].Value = smrnd.TemporaryDate;
                        grdSMRNServList.Rows[i].Cells["JobId"].Value = smrnd.JobIDNo;
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
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
        private void pnlAddEdit_Paint(object sender, PaintEventArgs e)
        {

        }
        private void btnFinalize_Click(object sender, EventArgs e)
        {
            try
            {
                SMRNServicedListDB ServListDB = new SMRNServicedListDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Finalize the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (ServListDB.FinalizeSMRNServicedList(prevservlist))
                    {
                        MessageBox.Show("SMRNService List Finalized");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredSMRNServiceList(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                    else
                        MessageBox.Show("Failed to Finalize");
                }
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
                btnDelete.Visible = false;
                btnApproved.Visible = true;
                btnNew.Visible = false;
                btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                btnFinalize.Visible = false;
                //btnApprove.Visible = false;
                //btnReverse.Visible = false;
                //btnGetComments.Visible = false;
                //chkCommentStatus.Visible = false;
                //txtComments.Visible = false;
                disableTabPages();
                clearTabPageControls();
                //----24/11/2016
                handleNewButton();
                handleGrdViewButton();
                handleGrdEditButton();
                handleGrdPrintButton();
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
                    //tabComments.Enabled = false;
                    tabPDFViewer.Enabled = false;
                    tabControl1.SelectedTab = tabServiceList;
                }
                else if (btnName == "btnEditPanel") //called from cancel,save,forward,approve and reverse button events
                {
                    ////btnNew.Visible = true;
                    btnExit.Visible = true;
                }
                //gridview buttons
                else if (btnName == "Finalize")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnFinalize.Visible = true;
                    btnDelete.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabServiceList;
                }
                //else if (btnName == "Finalize")
                //{
                //    pnlBottomButtons.Visible = false; //24/11/2016
                //    btnCancel.Visible = true;
                //    btnFinalize.Visible = true;
                //    btnDelete.Visible = true;
                //    //btnReverse.Visible = true;
                //    disableTabPages();
                //    tabControl1.SelectedTab = tabServiceList;
                //}
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    //tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabServiceList;
                }
                pnlEditButtons.Refresh();
                //if the user privilege is only view, show only the Approved documents button
                int ups = getuserPrivilegeStatus();
                if (ups == 1 || ups == 0)
                {
                    grdList.Columns["Finalize"].Visible = false;
                    btnActionPending.Visible = false;
                    //btnApprovalPending.Visible = false;
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
            grdList.Columns["Finalize"].Visible = false;
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                if (listOption == 1)
                {
                    grdList.Columns["Finalize"].Visible = true;
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
        void handleGrdPrintButton()
        {
            grdList.Columns["Print"].Visible = false;
            //if any one of view,add and edit
            if (Main.itemPriv[0] || Main.itemPriv[1] || Main.itemPriv[2])
            {
                //list option 1 should not have view buttuon visible (edit is avialable)
                if (listOption == 1)
                {
                    grdList.Columns["Print"].Visible = false;
                }
                else
                {
                    grdList.Columns["Print"].Visible = true;
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
        private void btnListDocuments_Click_1(object sender, EventArgs e)
        {

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
                    string subDir = prevservlist.ListNo + "-" + prevservlist.ListDate.ToString("yyyyMMddhhmmss");
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

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredSMRNServiceList(listOption);
        }

        private void btnApproved_Click(object sender, EventArgs e)
        {
            //if (getuserPrivilegeStatus() == 2)
            //{
            //    listOption = 6; //viewer
            //}
            //else
            //{
            //    listOption = 3;
            //}
            //ListFilteredPSRHeader(listOption);
            listOption = 2;
            ListFilteredSMRNServiceList(listOption);
        }
        private void removeControlsFromPnlLvPanel()
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
        private void btnSelectSMRNHeaderNo_Click(object sender, EventArgs e)
        {
            ////btnSelectSMRNHeaderNo.Enabled = false;
            //removeControlsFromPnlLvPanel();
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
            lv = SMRNHeaderDB.getTempNoForServiceListView();
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            frmPopup.Controls.Add(lv);

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
            frmPopup.ShowDialog();
            //pnlAddEdit.Controls.Add(pnllv);
            //pnllv.BringToFront();
            //pnllv.Visible = true;
        }

        private void lvOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkLVItemChecked("SMRN"))
                {
                    return;
                }
                //btnSelectSMRNHeaderNo.Enabled = true;
                //pnllv.Visible = false;
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        txtTrackingNo.Text = itemRow.SubItems[1].Text;
                        dtTrackingDate.Value = Convert.ToDateTime(itemRow.SubItems[2].Text);
                        txtCustPONo.Text = itemRow.SubItems[5].Text;
                        txtCustomer.Text = itemRow.SubItems[3].Text + "-" + itemRow.SubItems[4].Text;
                        dtCustPODate.Value = Convert.ToDateTime(itemRow.SubItems[6].Text);
                        //tempNo = itemRow.SubItems[7].Text;
                        //tempDate = Convert.ToDateTime(itemRow.SubItems[8].Text);
                        smrnservicedlist tempServ = new smrnservicedlist();
                        tempServ.TrackingNo = Convert.ToInt32(itemRow.SubItems[1].Text);
                        tempServ.TrackingDate = Convert.ToDateTime(itemRow.SubItems[2].Text);
                        AddSMRNServListRow(tempServ);
                    }
                }
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        private void AddSMRNServListRow(smrnservicedlist tempServ)
        {
            List<smrndetail> SMRNDetail = SMRNServicedListDB.getSMRNDetailForServiceList(tempServ, 2);
            grdSMRNServList.Rows.Clear();
            grdSMRNServList.Columns["Check"].Visible = true;
            int i = 0;
            foreach (smrndetail smrnd in SMRNDetail)
            {
                if (!AddSMRNDetailRow())
                {
                    MessageBox.Show("Error found in Adding rows to SMRNGrdiDetail.");
                }
                else
                {

                    grdSMRNServList.Rows[i].Cells["Item"].Value = smrnd.StockItemID + "-" + smrnd.StockItemName;
                    grdSMRNServList.Rows[i].Cells["SerialNo"].Value = smrnd.SerialNo;
                    grdSMRNServList.Rows[i].Cells["ItemDetails"].Value = smrnd.ItemDetails;
                    grdSMRNServList.Rows[i].Cells["WarrantyStatus"].Value = showStatus(smrnd.WarrantyStatus);
                    grdSMRNServList.Rows[i].Cells["ProductServiceStatus"].Value = smrnd.ProductServiceStatus;
                    grdSMRNServList.Rows[i].Cells["gSMRNHeaderNo"].Value = smrnd.InspectionStatus;  // for SMRNHeaderNo
                    grdSMRNServList.Rows[i].Cells["gSMRNHeaderDate"].Value = smrnd.TemporaryDate;  // for SMRN HEader date
                    grdSMRNServList.Rows[i].Cells["gTemporaryNo"].Value = smrnd.TemporaryNo;
                    grdSMRNServList.Rows[i].Cells["gTemporarydate"].Value = smrnd.TemporaryDate;
                    grdSMRNServList.Rows[i].Cells["JobId"].Value = smrnd.JobIDNo;
                }
                i++;
            }
        }
        private Boolean AddSMRNDetailRow()
        {
            Boolean status = true;
            try
            {
                grdSMRNServList.Rows.Add();
                int kount = grdSMRNServList.RowCount;
                grdSMRNServList.Rows[kount - 1].Cells[0].Value = kount;
                grdSMRNServList.Rows[kount - 1].Cells["Item"].Value = "";
                grdSMRNServList.Rows[kount - 1].Cells["SerialNo"].Value = "";
                grdSMRNServList.Rows[kount - 1].Cells["ItemDetails"].Value = " ";
                grdSMRNServList.Rows[kount - 1].Cells["WarrantyStatus"].Value = "";
                grdSMRNServList.Rows[kount - 1].Cells["JobId"].Value = "";
                grdSMRNServList.Rows[kount - 1].Cells["ProductServiceStatus"].Value = "";
                grdSMRNServList.Rows[kount - 1].Cells["gSMRNHeaderNo"].Value = "";
                grdSMRNServList.Rows[kount - 1].Cells["gSMRNHeaderDate"].Value = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddSMRNDetailRow() : Error");
            }

            return status;
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

        //private void btnSelectJobID_Click(object sender, EventArgs e)
        //{

        //    btnSelectJobID.Enabled = false;
        //    if (txtSMRNHeaderNo.Text.Length == 0)
        //    {
        //        MessageBox.Show("Select SMRN Temporary No");
        //        btnSelectJobID.Enabled = true;
        //        return;
        //    }
        //    if (cmbReportType.SelectedIndex == -1)
        //    {
        //        MessageBox.Show("Select Report Type");
        //        btnSelectJobID.Enabled = true;
        //        return;
        //    }
        //    removeControlsFromPnlLvPanel();
        //    pnllv = new Panel();
        //    pnllv.BorderStyle = BorderStyle.FixedSingle;

        //    pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
        //    int tempNo = Convert.ToInt32(txtSMRNHeaderNo.Text);
        //    DateTime tempDate = dtSMRNHeaderDate.Value;
        //    int ReportType = Convert.ToInt32(cmbReportType.SelectedItem.ToString().Trim().Substring(0, cmbReportType.SelectedItem.ToString().Trim().IndexOf('-')));
        //    lv = SMRNHeaderDB.getSMRNJOBIDNoListView(tempNo, tempDate, ReportType);
        //    this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView2_ItemChecked);
        //    lv.Bounds = new Rectangle(new Point(50, 50), new Size(500, 200));
        //    pnllv.Controls.Add(lv);

        //    Button lvOK = new Button();
        //    lvOK.Text = "OK";
        //    lvOK.Location = new Point(50, 270);
        //    lvOK.Click += new System.EventHandler(this.lvOK_Click1);
        //    pnllv.Controls.Add(lvOK);

        //    Button lvCancel = new Button();
        //    lvCancel.Text = "Cancel";
        //    lvCancel.Location = new Point(150, 270);
        //    lvCancel.Click += new System.EventHandler(this.lvCancel_Click1);
        //    pnllv.Controls.Add(lvCancel);

        //    pnlAddEdit.Controls.Add(pnllv);
        //    pnllv.BringToFront();
        //    pnllv.Visible = true;
        //}
        //private void lvOK_Click1(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        btnSelectJobID.Enabled = true;
        //        pnllv.Visible = false;
        //        ////ArrayList lviItemsArrayList = new ArrayList();
        //        string trlist;
        //        if (lv.CheckedItems.Count > 0)
        //        {
        //            trlist = "";
        //            foreach (ListViewItem itemRow in lv.Items)
        //            {
        //                if (itemRow.Checked)
        //                {
        //                    btnSelectJobID.Enabled = true;
        //                    txtJobIDNo.Text = itemRow.SubItems[1].Text;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //private void lvCancel_Click1(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        btnSelectJobID.Enabled = true;
        //        pnllv.Visible = false;
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        ////-----
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

        private void btnListDocuments_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevservlist.ListNo + "-" + prevservlist.ListDate.ToString("yyyyMMddhhmmss"));
                pnlPDFViewer.Controls.Add(dgvDocumentList);
                dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
            }
            catch (Exception ex)
            {
            }
        }

        private void btnCloseDocument_Click_1(object sender, EventArgs e)
        {
            removePDFControls();
            showPDFFileGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            bool status = true;
            try
            {
                DialogResult dialog = MessageBox.Show("Are you sure to delete the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    SMRNServicedListDB ServListDB = new SMRNServicedListDB();
                    smrnservicedlist smrnserv;
                    List<smrnservicedlist> SMRNServList = new List<smrnservicedlist>();
                    for (int i = 0; i < grdSMRNServList.Rows.Count; i++)
                    {
                        smrnserv = new smrnservicedlist();
                        smrnserv.SMRNHeaderNo = Convert.ToInt32(grdSMRNServList.Rows[i].Cells["gSMRNHeaderNo"].Value.ToString().Trim());
                        smrnserv.SMRNHeaderDate = Convert.ToDateTime(grdSMRNServList.Rows[i].Cells["gSMRNHeaderDate"].Value.ToString().Trim());
                        smrnserv.JobIDNo = Convert.ToInt32(grdSMRNServList.Rows[i].Cells["JobId"].Value.ToString().Trim());
                        smrnserv.ListNo = Convert.ToInt32(grdSMRNServList.Rows[i].Cells["gTemporaryNo"].Value.ToString().Trim());     /// For storing TemporaryNo of SMRNDetail
                        smrnserv.ListDate = Convert.ToDateTime(grdSMRNServList.Rows[i].Cells["gTemporaryDate"].Value.ToString().Trim()); ////for storing temporaryDate of SMRNdetail
                        SMRNServList.Add(smrnserv);
                    }
                    if (ServListDB.updateSMRNServicedList(prevservlist, SMRNServList, 2))
                    {
                        MessageBox.Show("SMRNService List Deleted");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredSMRNServiceList(listOption);
                        //pnlBottomActions.Visible = true;
                    }
                    else
                    {
                        MessageBox.Show("Failed to Delete");
                        status = false;
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
            }
            if (!status)
            {
                MessageBox.Show("Failed to Delete");
                status = false;
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }

        private void tabPSReport_Click(object sender, EventArgs e)
        {

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

        private void grdSMRNServList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //removeControlsFromPnlLvPanel();
        }

        private void SMRNServicedList_Enter(object sender, EventArgs e)
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

