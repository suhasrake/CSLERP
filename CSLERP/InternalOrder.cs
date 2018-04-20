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
using System.Collections;

namespace CSLERP
{
    public partial class InternalOrder : System.Windows.Forms.Form
    {
        string docID = "";
        Boolean isViewMode = false;
        string chkDocID = "";
        string subColName1 = "";
        string poDocID = "";
        string forwarderList = "";
        string approverList = "";
        Timer filterTimer1 = new Timer();
        Timer filterTimer = new Timer();
        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        Boolean isEditClick = false;
        Boolean userIsACommenter = false;
        DataGridView grdStock = new DataGridView();
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        TextBox txtSearch = new TextBox();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        DataGridView grdRefSel = new DataGridView();
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        ioheader previoh;
        //System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
                                      ////Boolean captureChange = false;
        Form frmPopup = new Form();
        TreeView tv = new TreeView();
        Panel pnlModel = new Panel();
        int descClickRowIndex = -1;
        RichTextBox txtDesc = new RichTextBox();
        Boolean AddRowClick = false;
        Boolean isChangedSpecDet = false;
        TextBox txtSearch1 = new TextBox();
        DataGridView grdCustList = new DataGridView();
        string chkType = "";
        Dictionary<string, string[]> dictItemWiseTOt = new Dictionary<string, string[]>();
        string colClicked = "";
        Boolean isNewClick = false;
        List<iorequirements> iorequirementsfilled = new List<iorequirements>();
        public InternalOrder()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void InternalOrder_Load(object sender, EventArgs e)
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
            grdTrackingDetail.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdTrackingDetail.EnableHeadersVisualStyles = false;
            grdItemWiseTotalQuant.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdItemWiseTotalQuant.EnableHeadersVisualStyles = false;
            ListFilteredIOHeader(listOption);
            //applyPrivilege();
        }
        private void ListFilteredIOHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                InternalOrderDB iodb = new InternalOrderDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                List<ioheader> IOHeaders = iodb.getFilteredIOHeader(userString, option, userCommentStatusString);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (ioheader ioh in IOHeaders)
                {
                    if (option == 1)
                    {
                        if (ioh.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {
                        ////if (!(ioh.CreateUser.Equals(Login.userLoggedIn) ||
                        ////    ioh.ForwardUser.Equals(Login.userLoggedIn) ||
                        ////    ioh.ApproveUser.Equals(Login.userLoggedIn)))
                        ////{
                        ////    //if not relevent to the user looged in
                        ////    continue;
                        ////}
                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["aa"].Value = ioh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["DName"].Value = ioh.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["TempNo"].Value = ioh.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["TempDate"].Value = ioh.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["InternalOrderNo"].Value = ioh.InternalOrderNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["InternalOrderDate"].Value = ioh.InternalOrderDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["ReferenceTrackingNo"].Value = ioh.ReferenceTrackingNos;
                    grdList.Rows[grdList.RowCount - 1].Cells["CustID"].Value = ioh.CustomerID;
                    grdList.Rows[grdList.RowCount - 1].Cells["CustName"].Value = ioh.CustomerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["TargetDate"].Value = ioh.TargetDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["Remarks"].Value = ioh.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = ioh.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatus"].Value = ioh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["ClosingStatus"].Value = ioh.ClosingStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["AcceptanceStatus"].Value = ioh.AcceptanceStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = ioh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = ioh.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = ioh.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = ioh.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatusNo"].Value = ComboFIll.getDocumentStatusString(ioh.DocumentStatus);
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = ioh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ApproveUser"].Value = ioh.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatusNo"].Value = ioh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CmntStatus"].Value = ioh.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = ioh.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = ioh.ForwarderList;
                    grdList.Rows[grdList.RowCount - 1].Cells["SEFID"].Value = ioh.SEFID;
                    if (ioh.isPlanPrepared)
                    {
                        grdList.Rows[grdList.RowCount - 1].DefaultCellStyle.BackColor = Color.Magenta;
                    }
                    if (ioh.ClosingStatus == 1)
                    {
                        grdList.Rows[grdList.RowCount - 1].DefaultCellStyle.BackColor = Color.Tan;
                    }

                    grdList.Columns["DName"].Width = 150;
                    grdList.Columns["Remarks"].Width = 250;
                    grdList.Columns["CreateTime"].Width = 150;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Internal Order Inward Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;
            isEditClick = false;
            isNewClick = false;
            grdList.Columns["Creator"].Visible = true;
            grdList.Columns["Forwarder"].Visible = true;
            grdList.Columns["Approver"].Visible = true;
            string[] subDocList = SubDocumentReceiverDB.getEmpWiseSubDocList(Login.empLoggedIn, "IOPRODUCT").ToArray();
            //Main.itemPriv[0]: View
            //Main.itemPriv[1]: Add
            //Main.itemPriv[2]: Edit
            //Main.itemPriv[3]: Delete
            if (subDocList.Count() != 0 && (option == 3 || option == 6) && (Main.itemPriv[1] || Main.itemPriv[2]))
            {
                grdList.Columns["CloseRequest"].Visible = true;
                grdList.Columns["Close"].Visible = true;
            }
            else if(subDocList.Count() == 0)
            {
                grdList.Columns["CloseRequest"].Visible = false;
                grdList.Columns["Close"].Visible = true;
            }
            else
            {
                grdList.Columns["CloseRequest"].Visible = false;
                grdList.Columns["Close"].Visible = false;
            }
        }

        private void initVariables()
        {
            if (getuserPrivilegeStatus() == 1)
            {
                //user is only a viewer
                listOption = 6;
            }
            docID = Main.currentDocument;
            CatalogueValueDB.fillCatalogValueComboNew(cmbIOType, "IOTYPE");
            CatalogueValueDB.fillCatalogValueComboNew(cmbProductType, "SEFType");
            //CustomerDB.fillCustomerComboNew(cmbCustomer);
            ////CatalogueValueDB.fillCatalogValueListBox(lstServiceItems, "ServiceLookup");

            dtTempDate.Format = DateTimePickerFormat.Custom;
            dtTempDate.CustomFormat = "dd-MM-yyyy";
            dtTempDate.Enabled = false;
            dtInternalOrderDate.Format = DateTimePickerFormat.Custom;
            dtInternalOrderDate.CustomFormat = "dd-MM-yyyy";
            dtInternalOrderDate.Enabled = false;
            dtTargetDate.Format = DateTimePickerFormat.Custom;
            dtTargetDate.CustomFormat = "dd-MM-yyyy";
            txtTemporarryNo.Enabled = false;
            dtTempDate.Enabled = false;
            txtInternalOrderNo.Enabled = false;
            dtInternalOrderDate.Enabled = false;
            btnForward.Visible = false;
            btnApprove.Visible = false;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            txtReferenceTrackingNo.TabIndex = 1;
            dtTargetDate.TabIndex = 2;
            grdIODetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            lv.Visible = false;
            isViewMode = false;
            isEditClick = false;
            isChangedSpecDet = false;
            //---
            txtComments.Text = "";
            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
            setTabIndex();
            List<documentreceiver> li = Main.DocumentReceivers;
        }
        private void setTabIndex()
        {
            txtTemporarryNo.TabIndex = 0;
            dtTempDate.TabIndex = 1;
            txtInternalOrderNo.TabIndex = 2;
            dtInternalOrderDate.TabIndex = 3;
            txtReferenceTrackingNo.TabIndex = 4;
            btnSelectTrackingNo.TabIndex = 5;
            btnIOReferenceOption.TabIndex = 6;
            btncustomerselect.TabIndex = 7;
            dtTargetDate.TabIndex = 8;
            cmbProductType.TabIndex = 9;
            txtRemarks.TabIndex = 10;

            btnForward.TabIndex = 0;
            btnApprove.TabIndex = 1;
            btnCancel.TabIndex = 2;
            btnSave.TabIndex = 3;
            btnReverse.TabIndex = 4;
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
                grdIODetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                //----------clear temperory panels
                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                cmbProductType.Enabled = true;
                isEditClick = false;
                //----------
                txtcustomer.Text = "";
                //cmbCustomer.SelectedIndex = -1;
                cmbIOType.SelectedIndex = -1;
                cmbProductType.SelectedIndex = -1;
                grdIODetail.Rows.Clear();
                txtTemporarryNo.Text = "";
                dtTempDate.Value = DateTime.Parse("1900-01-01");
                txtInternalOrderNo.Text = "";
                dtInternalOrderDate.Value = DateTime.Parse("1900-01-01");
                txtReferenceTrackingNo.Text = "";
                dtTargetDate.Value = DateTime.Today.Date;
                txtRemarks.Text = "";
                tabIOType.Enabled = true;
                tabIOType.Visible = true;
                tabIOHeader.Visible = false;
                tabIODetail.Visible = false;
                previoh = new ioheader();
                iorequirementsfilled = new List<iorequirements>();
                descClickRowIndex = -1;
                AddRowClick = false;
                isViewMode = false;
                isEditClick = false;
                isChangedSpecDet = false;
                commentStatus = "";
                chkType = "";
                isNewClick = false;
                grdTrackingDetail.Rows.Clear();
                tabControl1.TabPages["tabPODetail"].Visible = false;
                grdTrackingDetail.Visible = false;
                grdItemWiseTotalQuant.Visible = false;
                grdItemWiseTotalQuant.Rows.Clear();
                colClicked = "";
                removeControlsFromForwarderPanelTV();
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
                closeAllPanels();
                pnlAddEdit.Visible = true;
                tabControl1.SelectedTab = tabIOType;
                cmbIOType.Enabled = true;
                setButtonVisibility("btnNew");
                AddRowClick = false;
                isViewMode = false;
                isEditClick = false;
                isChangedSpecDet = false;
                chkType = "";
                isNewClick = true;
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

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                AddIODetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddIODetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdIODetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkIODetailGridRows())
                    {
                        return false;
                    }
                }
                grdIODetail.Rows.Add();
                int kount = grdIODetail.RowCount;
                grdIODetail.Rows[kount - 1].Cells[0].Value = kount;
                if (docID == "IOPRODUCT")
                {
                    grdIODetail.Rows[kount - 1].Cells["ServiceItem"].Value = "";
                    grdIODetail.Rows[kount - 1].Cells["ProductItem"].Value = "";
                    grdIODetail.Rows[kount - 1].Cells["ModelNo"].Value = "";
                    grdIODetail.Rows[kount - 1].Cells["ModelName"].Value = "";
                    grdIODetail.Columns["ServiceItem"].Visible = false;
                    setDetailColumnForProduct();
                }
                else
                {
                    //DataGridViewComboBoxCell ComboColumn1 = (DataGridViewComboBoxCell)(grdIODetail.Rows[kount - 1].Cells["ServiceItem"]);
                    ////CatalogueValueDB.fillCatalogValueGridViewComboNew(ComboColumn, "Qualification");
                    //CatalogueValueDB.fillCatalogValueGridViewComboNew(ComboColumn1, "ServiceLookup");
                    grdIODetail.Rows[kount - 1].Cells["ProductItem"].Value = "";
                    grdIODetail.Rows[kount - 1].Cells["ServiceItem"].Value = "";
                    grdIODetail.Rows[kount - 1].Cells["ModelNo"].Value = "NA";
                    grdIODetail.Rows[kount - 1].Cells["ModelName"].Value = "NA";
                    setDetailColumnForService();
                }

                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdIODetail.Rows[kount - 1].Cells["OfficeID"]);
                if (docID == "IOSERVICE")
                {
                    //CatalogueValueDB.fillCatalogValueGridViewComboNew(ComboColumn2, "");
                    OfficeDB.fillOfficeIDGridViewComboNew(ComboColumn2);
                    grdIODetail.Columns["OfficeID"].Visible = true;
                }
                else
                {
                    grdIODetail.Rows[kount - 1].Cells["OfficeID"].ReadOnly = true;
                    grdIODetail.Columns["OfficeID"].Visible = false;
                }
                grdIODetail.Rows[kount - 1].Cells["Quantity"].Value = 0;
                grdIODetail.Rows[kount - 1].Cells["WarrantyDays"].Value = 0;
                ////grdIODetail.Rows[kount - 1].Cells["WorkDescription"].Value = " ";
                grdIODetail.Rows[kount - 1].Cells["Specification"].Value = " ";
                var BtnCell = (DataGridViewButtonCell)grdIODetail.Rows[kount - 1].Cells["Delete"];
               
                BtnCell.Value = "Del";
                if (AddRowClick)
                    grdIODetail.FirstDisplayedScrollingRowIndex = grdIODetail.RowCount - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddIODetailRow() : Error");
            }

            return status;
        }
        private void setDetailColumnForProduct()
        {
            grdIODetail.Columns["ServiceItem"].Visible = false;
            grdIODetail.Columns["ProductItem"].Visible = true;
            grdIODetail.Columns["Sel"].Visible = true;
            grdIODetail.Columns["ModelNo"].Visible = true;
            grdIODetail.Columns["ModelName"].Visible = true;
        }
        private void setDetailColumnForService()
        {
            grdIODetail.Columns["ServiceItem"].Visible = true;
            grdIODetail.Columns["ProductItem"].Visible = false;
            grdIODetail.Columns["Sel"].Visible = true;
            grdIODetail.Columns["ModelNo"].Visible = false;
            grdIODetail.Columns["ModelName"].Visible = false;
        }
        private Boolean verifyAndReworkIODetailGridRows()
        {
            Boolean status = true;

            try
            {
                double quantity = 0;

                if (grdIODetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in iO  details");
                    return false;
                }
                //clear tax details table
                TaxDetailsTable.Rows.Clear();
                for (int i = 0; i < grdIODetail.Rows.Count; i++)
                {

                    grdIODetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if ((grdIODetail.Rows[i].Cells["ProductItem"].Value == null && chkDocID == "IOPRODUCT") ||
                       (grdIODetail.Rows[i].Cells["ServiceItem"].Value == null && chkDocID == "IOSERVICE") ||
                       (grdIODetail.Rows[i].Cells["ProductItem"].Value.ToString().Length == 0 && chkDocID == "IOPRODUCT") ||
                       (grdIODetail.Rows[i].Cells["ServiceItem"].Value.ToString().Length == 0 && chkDocID == "IOSERVICE") ||
                       (grdIODetail.Rows[i].Cells["Specification"].Value == null) ||
                        (grdIODetail.Rows[i].Cells["Specification"].Value.ToString().Trim().Length == 0) ||
                        (grdIODetail.Rows[i].Cells["Quantity"].Value == null) ||
                        (Convert.ToDouble(grdIODetail.Rows[i].Cells["Quantity"].Value) == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                    if (docID == "IOSERVICE")
                    {
                        if ((grdIODetail.Rows[i].Cells["OfficeID"].Value == null) ||
                        (grdIODetail.Rows[i].Cells["OfficeID"].Value.ToString().Trim().Length == 0))
                        {
                            MessageBox.Show("Fill values in row " + (i + 1));
                            return false;
                        }
                    }

                    quantity = Convert.ToDouble(grdIODetail.Rows[i].Cells["Quantity"].Value);
                }
                if (!isViewMode)
                {
                    if (!validateItems())
                    {
                        MessageBox.Show("Validation failed");
                    }
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
                if (docID == "IOPRODUCT")
                {

                    for (int i = 0; i < grdIODetail.Rows.Count - 1; i++)
                    {
                        for (int j = i + 1; j < grdIODetail.Rows.Count; j++)
                        {

                            if (grdIODetail.Rows[i].Cells[1].Value.ToString() == grdIODetail.Rows[j].Cells[1].Value.ToString())
                            {
                                //duplicate item code
                                MessageBox.Show("Item code duplicated in IO details... please ensure correctness (" +
                                    grdIODetail.Rows[i].Cells[1].Value.ToString() + ")");
                            }
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

        private List<iodetail> getIODetails(ioheader ioh)
        {
            List<iodetail> IODetails = new List<iodetail>();
            try
            {
                iodetail iod = new iodetail();
                for (int i = 0; i < grdIODetail.Rows.Count; i++)
                {
                    iod = new iodetail();
                    iod.DocumentID = ioh.DocumentID;
                    iod.TemporaryNo = ioh.TemporaryNo;
                    iod.TemporaryDate = ioh.TemporaryDate;
                    if (iod.DocumentID.Equals("IOPRODUCT"))
                    {
                        iod.StockItemID = grdIODetail.Rows[i].Cells["ProductItem"].Value.ToString().Trim().Substring(0, grdIODetail.Rows[i].Cells["ProductItem"].Value.ToString().Trim().IndexOf('-'));
                        iod.ModelNo = grdIODetail.Rows[i].Cells["ModelNo"].Value.ToString();
                    }
                    else
                        iod.StockItemID = grdIODetail.Rows[i].Cells["ServiceItem"].Value.ToString().Trim().Substring(0, grdIODetail.Rows[i].Cells["ServiceItem"].Value.ToString().Trim().IndexOf('-'));
                    try
                    {
                        iod.OfficeID = grdIODetail.Rows[i].Cells["OfficeID"].Value.ToString();//.Trim().Substring(0, grdIODetail.Rows[i].Cells["OfficeID"].Value.ToString().Trim().IndexOf('-'));
                    }
                    catch (Exception ex)
                    {
                        iod.OfficeID = null;
                    }
                    iod.Quantity = Convert.ToDouble(grdIODetail.Rows[i].Cells["Quantity"].Value);
                    iod.WarrantyDays = Convert.ToInt32(grdIODetail.Rows[i].Cells["WarrantyDays"].Value);

                    iod.Specification = grdIODetail.Rows[i].Cells["Specification"].Value.ToString();

                    IODetails.Add(iod);

                }
            }
            catch (Exception)
            {
                MessageBox.Show("getIODetails() : Error getting IODB Details");
                IODetails = null;
            }
            return IODetails;
        }

        private void btnActionPending_Click_1(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredIOHeader(listOption);
        }

        private void btnApprovalPending_Click_1(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredIOHeader(listOption);
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

            ListFilteredIOHeader(listOption);
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {

                InternalOrderDB popidb = new InternalOrderDB();
                ioheader ioh = new ioheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    if (!verifyAndReworkIODetailGridRows())
                    {
                        return;
                    }
                    ioh.DocumentID = docID;
                    ioh.InternalOrderDate = dtInternalOrderDate.Value;
                    //ioh.InternalOrderNo = Convert.ToInt32(txtInternalOrderNo.Text);
                    //ioh.InternalOrderDate = dtInternalOrderDate.Value;
                    ioh.ReferenceTrackingNos = txtReferenceTrackingNo.Text;
                    ////////ioh.CustomerID = cmbCustomer.SelectedItem.ToString().Trim().Substring(0, cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-'));
                    try
                    {
                        //ioh.CustomerID = ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue;
                        string[] customer = txtcustomer.Text.Split('-');
                        ioh.CustomerID = customer[0];
                    }
                    catch (Exception ex)
                    {
                    }
                    if (docID == "IOPRODUCT")
                    {
                        ioh.SEFID = ((Structures.ComboBoxItem)cmbProductType.SelectedItem).HiddenValue;
                        InternalOrderDB ioDB = new InternalOrderDB();
                        int tCount = ioDB.getTemplateListForProductCount(ioh.SEFID);

                        if (tCount > 0)
                        {

                            if (iorequirementsfilled == null || iorequirementsfilled.Count == 0)
                            {
                                MessageBox.Show("Specification for Product type Not Filled.Fill properly before saving");
                                return;
                            }
                            List<iorequirements> ioreqCount = popidb.getTemplateListForProductType(ioh.SEFID);
                            if (ioreqCount.Count > iorequirementsfilled.Count)
                            {
                                MessageBox.Show("Specification for Product type Not Filled.Fill properly before saving");
                                return;
                            }
                        }

                    }
                    else
                        ioh.SEFID = "";
                    ioh.TargetDate = dtTargetDate.Value;
                    ioh.Remarks = txtRemarks.Text;
                    //ioh.Status = ComboFIll.getStatusCode(cmbStatus.SelectedItem.ToString());
                    ioh.Comments = docCmtrDB.DGVtoString(dgvComments).Replace("'", "''");
                    ioh.ForwarderList = previoh.ForwarderList;
                    ////////return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }

                if (!popidb.ValidateIOHeader(ioh))
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    //ioh.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    ioh.DocumentStatus = 1; //created
                    ioh.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    ioh.TemporaryNo = Convert.ToInt32(txtTemporarryNo.Text);
                    ioh.TemporaryDate = previoh.TemporaryDate;
                    ioh.DocumentStatus = previoh.DocumentStatus;
                }
                //Replacing single quotes
                ioh = verifyHeaderInputString(ioh);
                verifyDetailInputString();

                if (popidb.ValidateIOHeader(ioh))
                {
                    //--

                    //--create comment status string
                    docCmtrDB = new DocCommenterDB();
                    if (userIsACommenter)
                    {
                        //if the user is only a commenter and ticked the comment as final; then update his comment string as final
                        //and update the comment status
                        if (txtComments.Text != null && txtComments.Text.Trim().Length != 0)
                        {
                            docCmtrDB = new DocCommenterDB();
                            ioh.CommentStatus = docCmtrDB.createCommentStatusString(previoh.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            ioh.CommentStatus = previoh.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            ioh.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            ioh.CommentStatus = previoh.CommentStatus;
                        }
                    }
                    //----------
                    int tmpStatus = 1;
                    if (txtComments.Text.Trim().Length > 0)
                    {
                        ioh.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus).Replace("'", "''"); ;
                    }

                    //--
                    List<iodetail> ioList = getIODetails(ioh);
                    if (btnText.Equals("Update"))
                    {
                        if (popidb.updateIOHeaderAndDetail(ioh, previoh, ioList, iorequirementsfilled))
                        {
                            MessageBox.Show("Internal Order Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredIOHeader(listOption);
                        }
                        else
                        {
                            status = false;

                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update IO ");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (popidb.InsertIOHeaderAndDetail(ioh, ioList, iorequirementsfilled))
                        {
                            MessageBox.Show("Internal Order Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredIOHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert IO Header");
                        }
                    }
                }
                else
                {
                    MessageBox.Show(" IO Validation failed");
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("createAndUpdateIODetails() : Error");
                status = false;
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }

        private void btnAddLine_Click_1(object sender, EventArgs e)
        {
            AddRowClick = true;
            AddIODetailRow();
        }

        private void grdPOPIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdIODetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdIODetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkIODetailGridRows();
                    }
                    if (columnName.Equals("Sel"))
                    {
                        if (docID == "IOSERVICE")
                            showServiceItemTreeView();
                        else
                            showStockItemDataGridView();
                    }
                    if (columnName.Equals("selSpec"))
                    {
                        descClickRowIndex = e.RowIndex;
                        string strTest = grdIODetail.Rows[e.RowIndex].Cells["Specification"].Value.ToString().Trim();
                        showPopUpForDescription(strTest);
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
        //-----------
        private void showPopUpForDescription(string str)
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;
            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;
            frmPopup.Size = new Size(360, 170);

            Label head = new Label();
            head.AutoSize = true;
            head.Location = new System.Drawing.Point(3, 3);
            head.Name = "label2";
            head.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            head.ForeColor = Color.White;
            head.Size = new System.Drawing.Size(146, 13);
            head.Text = "Fill Description Below";
            frmPopup.Controls.Add(head);

            txtDesc = new RichTextBox();
            txtDesc.Text = str;
            txtDesc.Bounds = new Rectangle(new Point(3, 25), new Size(345, 111));
            frmPopup.Controls.Add(txtDesc);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.BackColor = Color.Tan;
            lvOK.Location = new System.Drawing.Point(210, 142);
            lvOK.Size = new System.Drawing.Size(64, 23);
            lvOK.Cursor = Cursors.Hand;
            lvOK.Click += new System.EventHandler(this.lvOK_Click5);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new System.Drawing.Point(273, 142);
            lvCancel.Size = new System.Drawing.Size(73, 23);
            lvCancel.Cursor = Cursors.Hand;
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click5);
            frmPopup.Controls.Add(lvCancel);

            frmPopup.ShowDialog();
        }
        private void lvOK_Click5(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtDesc.Text.Trim()))
                {
                    MessageBox.Show("Description is empty");
                    return;
                }
                grdIODetail.Rows[descClickRowIndex].Cells["Specification"].Value = txtDesc.Text.Trim();
                grdIODetail.FirstDisplayedScrollingRowIndex = grdIODetail.Rows[descClickRowIndex].Index;
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
                txtDesc.Text = "";
                descClickRowIndex = -1;
                frmPopup.Close();
                frmPopup.Dispose();

            }
            catch (Exception)
            {
            }
        }
        //-----------
        private Boolean checkAvailabilityOfitem()
        {
            Boolean status = true;
            if (grdIODetail.CurrentRow.Cells["ServiceItem"].Value.ToString().Length != 0)
            {
                status = false;
            }
            return status;
        }
        private void showServiceItemTreeView()
        {
            removeControlsFromForwarderPanelTV();
            if (!checkAvailabilityOfitem())
            {
                DialogResult dialog = MessageBox.Show("Selected service detail will removed?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdIODetail.CurrentRow.Cells["ServiceItem"].Value = "";

                }
                else
                    return;
            }
            tv = new TreeView();
            tv.CheckBoxes = true;
            tv.Nodes.Clear();
            tv.CheckBoxes = true;
            pnlForwarder.BorderStyle = BorderStyle.Fixed3D;
            pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(550, 340));
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Location = new Point(5, 5);
            lbl.Size = new Size(35, 13);
            lbl.Text = "Tree View For Product";
            lbl.Font = new Font("Serif", 10, FontStyle.Bold);
            lbl.ForeColor = Color.Green;
            pnlForwarder.Controls.Add(lbl);
            tv = ServiceItemsDB.getServiceItemTreeView();
            tv.Bounds = new Rectangle(new Point(20, 30), new Size(500, 260));
            pnlForwarder.Controls.Remove(tv);
            pnlForwarder.Controls.Add(tv);
            //tv.cl
            tv.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tv_AfterCheck);
            Button lvForwrdOK = new Button();
            lvForwrdOK.BackColor = Color.Tan;
            lvForwrdOK.Text = "OK";
            lvForwrdOK.Location = new Point(50, 305);
            lvForwrdOK.Click += new System.EventHandler(this.tvOK_Click);
            pnlForwarder.Controls.Add(lvForwrdOK);

            Button lvForwardCancel = new Button();
            lvForwardCancel.Text = "Cancel";
            lvForwardCancel.BackColor = Color.Tan;
            lvForwardCancel.Location = new Point(150, 305);
            lvForwardCancel.Click += new System.EventHandler(this.tvCancel_Click);
            pnlForwarder.Controls.Add(lvForwardCancel);

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
                    grdIODetail.CurrentRow.Cells["ServiceItem"].Value = s;
                    tv.CheckBoxes = true;
                    pnlForwarder.Controls.Remove(tv);
                    pnlForwarder.Visible = false;
                    //showModelListView(s);
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



        //showing gridview for stockitem
        private void showStockItemDataGridView()
        {
            try
            {
                if (grdIODetail.CurrentRow.Cells["ProductItem"].Value.ToString().Length != 0)
                {
                    DialogResult dialog = MessageBox.Show("Selected product and Model detail will removed?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        grdIODetail.CurrentRow.Cells["ProductItem"].Value = "";
                        grdIODetail.CurrentRow.Cells["ModelNo"].Value = "";
                        grdIODetail.CurrentRow.Cells["ModelName"].Value = "";
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

                frmPopup.Size = new Size(900, 370);

                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(550, 5);
                lblSearch.AutoSize = true;
                lblSearch.Text = "Search by Name";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Size = new Size(200, 18);
                txtSearch.Location = new System.Drawing.Point(680, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInStockGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                StockItemDB stkDB = new StockItemDB();
                grdStock = stkDB.getStockItemlistGrid();

                grdStock.Bounds = new Rectangle(new Point(0, 27), new Size(900, 300));
                frmPopup.Controls.Add(grdStock);
                grdStock.Columns["StockItemID"].Width = 150;
                grdStock.Columns["Name"].Width = 350;
                grdStock.Columns["Group1CodeDesc"].Width = 110;
                grdStock.Columns["Group2CodeDesc"].Width = 110;
                grdStock.Columns["Group3CodeDesc"].Width = 110;
                foreach (DataGridViewColumn cells in grdStock.Columns)
                {
                    if (cells.CellType != typeof(DataGridViewCheckBoxCell))
                        cells.ReadOnly = true;
                }
                //grdStock.ReadOnly = true;
                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grdStkOK_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 335);
                lvCancel.Click += new System.EventHandler(this.grdStkCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdStkOK_Click1(object sender, EventArgs e)
        {
            try
            {
                string iolist = "";
                var checkedRows = from DataGridViewRow r in grdStock.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("Select one Product");
                    return;
                }
                foreach (var row in checkedRows)
                {
                    iolist = row.Cells["StockItemID"].Value.ToString() + "-" + row.Cells["Name"].Value.ToString();
                }
                grdIODetail.CurrentRow.Cells["ProductItem"].Value = iolist;
                ///showModelListView(iolist);
                frmPopup.Close();
                frmPopup.Dispose();
                showModelListView(iolist);
            }
            catch (Exception)
            {
            }
        }
        private void grdStkCancel_Click1(object sender, EventArgs e)
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
        private void txtSearch_TextChangedInStockGridList(object sender, EventArgs e)
        {
            try
            {
                filterTimer1.Enabled = false;
                filterTimer1.Stop();
                filterTimer1.Tick -= new System.EventHandler(this.handlefilterTimerTimeout1);
                filterTimer1.Tick += new System.EventHandler(this.handlefilterTimerTimeout1);
                filterTimer1.Interval = 500;
                filterTimer1.Enabled = true;
                filterTimer1.Start();

            }
            catch (Exception ex)
            {

            }
        }
        private void handlefilterTimerTimeout1(object sender, EventArgs e)
        {
            filterTimer1.Enabled = false;
            filterTimer1.Stop();
            filterGridStkData();
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }

        private void filterGridStkData()
        {
            try
            {
                grdStock.CurrentCell = null;
                foreach (DataGridViewRow row in grdStock.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdStock.Rows)
                    {
                        if (!row.Cells["Name"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
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

        private void showModelListView(string stockID)
        {
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
                grdIODetail.CurrentRow.Cells["ModelNo"].Value = "NA";
                grdIODetail.CurrentRow.Cells["ModelName"].Value = "NA";
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
            lvOK.Click += new System.EventHandler(this.lvOK_Click3);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new System.Drawing.Point(130, 280);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click3);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
        }
        private void lvOK_Click3(object sender, EventArgs e)
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
                        grdIODetail.CurrentRow.Cells["ModelNo"].Value = item.SubItems[1].Text;
                        grdIODetail.CurrentRow.Cells["ModelName"].Value = item.SubItems[2].Text;
                        frmPopup.Close();
                        frmPopup.Dispose();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void lvCancel_Click3(object sender, EventArgs e)
        {
            try
            {
                //lvApprover.CheckBoxes = false;
                //lvApprover.CheckBoxes = true;
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
        //---
        private void showCLosingRequestPopUp()
        {
            try
            {
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.CadetBlue;
                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;
                frmPopup.Size = new Size(360, 170);

                Label head = new Label();
                head.AutoSize = true;
                head.Location = new System.Drawing.Point(3, 3);
                head.Name = "label2";
                head.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                head.ForeColor = Color.White;
                head.Size = new System.Drawing.Size(146, 13);
                head.Text = "Give Comment";
                frmPopup.Controls.Add(head);

                txtDesc = new RichTextBox();
                txtDesc.Text = "";
                txtDesc.Bounds = new Rectangle(new Point(3, 25), new Size(345, 111));
                frmPopup.Controls.Add(txtDesc);

                Button lvOK = new Button();
                lvOK.Text = "OK";
                lvOK.BackColor = Color.Tan;
                lvOK.Location = new System.Drawing.Point(210, 142);
                lvOK.Size = new System.Drawing.Size(64, 23);
                lvOK.Cursor = Cursors.Hand;
                lvOK.Click += new System.EventHandler(this.lvOK_ClickReqClose);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(273, 142);
                lvCancel.Size = new System.Drawing.Size(73, 23);
                lvCancel.Cursor = Cursors.Hand;
                lvCancel.Click += new System.EventHandler(this.lvCancel_ClickReqClose);
                frmPopup.Controls.Add(lvCancel);

                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {

            }
        }
        private void lvOK_ClickReqClose(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtDesc.Text.Trim()))
                {
                    MessageBox.Show("Comment is empty");
                    return;
                }
                DialogResult dialog = MessageBox.Show("Are you sure to send request for closing ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    string comment = txtDesc.Text.Trim();
                    string prevComment = previoh.Comments;
                    int tmpStatus = 1;
                    string newComments = docCmtrDB.processNewComment(dgvComments, comment, Login.userLoggedIn, Login.userLoggedInName, tmpStatus);

                    if (InternalOrderDB.RequestTOCloseIOHeader(previoh, newComments))
                    {
                        MessageBox.Show("Closing request sent");
                        grdList.CurrentRow.Cells["ClosingStatus"].Value = 1;
                        grdList.CurrentRow.DefaultCellStyle.BackColor = Color.Tan;
                    }
                    else
                    {
                        MessageBox.Show("Closing request failed");
                    }
                }
                frmPopup.Close();
                frmPopup.Dispose();
                //setButtonVisibility("btnEditPanel");
            }
            catch (Exception ex)
            {

            }
        }

        private void lvCancel_ClickReqClose(object sender, EventArgs e)
        {
            try
            {
                txtDesc.Text = "";
                frmPopup.Close();
                frmPopup.Dispose();
                //setButtonVisibility("btnEditPanel");

            }
            catch (Exception)
            {
            }
        }

        private void showpopUpForCLosing()
        {
            try
            {
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.CadetBlue;
                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;
                frmPopup.Size = new Size(360, 80);

                Label head = new Label();
                head.AutoSize = true;
                head.Location = new System.Drawing.Point(3, 3);
                head.Name = "label2";
                head.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                head.ForeColor = Color.White;
                head.Size = new System.Drawing.Size(146, 13);
                head.Text = "Request For CLosing. Are You Sure To Close !! ";
                frmPopup.Controls.Add(head);

                Button lvCLose = new Button();
                lvCLose.Text = "Close";
                lvCLose.BackColor = Color.Tan;
                lvCLose.Location = new System.Drawing.Point(20, 52);
                //lvCLose.Size = new System.Drawing.Size(64, 23);
                lvCLose.Cursor = Cursors.Hand;
                lvCLose.Click += new System.EventHandler(this.lvCLose_ClickClose);
                frmPopup.Controls.Add(lvCLose);

                Button lvReject = new Button();
                lvReject.Text = "Reject";
                lvReject.BackColor = Color.Tan;
                lvReject.Location = new System.Drawing.Point(100, 52);
                //lvReject.Size = new System.Drawing.Size(120, 23);
                lvReject.Cursor = Cursors.Hand;
                lvReject.Click += new System.EventHandler(this.lvReject_ClickClose);
                frmPopup.Controls.Add(lvReject);

                Button lvCancel = new Button();
                lvCancel.Text = "Back";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(180, 52);
                //lvCancel.Size = new System.Drawing.Size(90, 23);
                lvCancel.Cursor = Cursors.Hand;
                lvCancel.Click += new System.EventHandler(this.lvCancel_ClickClose);
                frmPopup.Controls.Add(lvCancel);

               

                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {

            }
        }
        private void lvCLose_ClickClose(object sender, EventArgs e)
        {
            try
            {
                if (InternalOrderDB.CloseIOHeader(previoh))
                {
                    MessageBox.Show("Internal Order closed.");
                    // ListFilteredIOHeader(listOption);
                    grdList.Rows.Remove(grdList.CurrentRow);
                }
                else
                {
                    MessageBox.Show("Fail to close Internal Order");
                }

                frmPopup.Close();
                frmPopup.Dispose();
                //setButtonVisibility("btnEditPanel");
            }
            catch (Exception ex)
            {

            }
        }
        private void lvReject_ClickClose(object sender, EventArgs e)
        {
            try
            {
                string comment = "Closing request rejected";
                string prevComment = previoh.Comments;
                int tmpStatus = 1;
                string newComments = docCmtrDB.processNewComment(dgvComments, comment, Login.userLoggedIn, Login.userLoggedInName, tmpStatus);

                if (InternalOrderDB.RejectClosingRequest(previoh, newComments))
                {
                    MessageBox.Show("Closing request rejected.");
                    //ListFilteredIOHeader(listOption);
                    grdList.CurrentRow.Cells["ClosingStatus"].Value = 0;
                    if ((grdList.CurrentRow.Index % 2) == 0)
                        grdList.CurrentRow.DefaultCellStyle.BackColor = Color.White;
                    else
                        grdList.CurrentRow.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
                }
                else
                {
                    MessageBox.Show("Failed to reject");
                }

                frmPopup.Close();
                frmPopup.Dispose();
                //setButtonVisibility("btnEditPanel");
            }
            catch (Exception ex)
            {

            }
        }
        private void lvCancel_ClickClose(object sender, EventArgs e)
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
        //===
        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("LoadDocument") ||
                     columnName.Equals("View") || columnName == "CloseRequest" || columnName == "Close")
                {
                    clearData();
                    setButtonVisibility(columnName);
                    //tabIOType.Enabled = false;
                    //tabIOType.Visible = false;
                    colClicked = columnName;
                    AddRowClick = false;
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    chkDocID = docID;
                    setIODetailColumns(docID);
                    tabIOHeader.Visible = true;
                    //tabPODetail.Visible = true;
                    //captureChange = false;
                    InternalOrderDB iodb = new InternalOrderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    previoh = new ioheader();
                    previoh.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmntStatus"].Value.ToString();
                    previoh.DocumentID = grdList.Rows[e.RowIndex].Cells["aa"].Value.ToString();
                    previoh.DocumentName = grdList.Rows[e.RowIndex].Cells["DName"].Value.ToString();
                    previoh.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["TempNo"].Value.ToString());
                    previoh.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["TempDate"].Value.ToString());
                    if (previoh.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    if (columnName.Equals("View"))
                    {
                        isViewMode = true;
                        tabControl1.TabPages["tabIOHeader"].Enabled = true;
                        tabControl1.TabPages["tabPODetail"].Enabled = true;
                        cmbProductType.Enabled = false;
                        //btnSpecification.Enabled = true;
                    }
                    if (columnName.Equals("Edit"))
                    {
                        isEditClick = true;
                    }

                    previoh.SEFID = grdList.Rows[e.RowIndex].Cells["SEFID"].Value.ToString();
                    chkType = previoh.SEFID;
                    InternalOrderDB ioDB = new InternalOrderDB();
                    if (docID == "IOPRODUCT" && previoh.SEFID != null && previoh.SEFID.Length != 0)
                    {
                        iorequirementsfilled = ioDB.GetTemplatesForPerticularIO(previoh.TemporaryNo, previoh.TemporaryDate);
                    }

                    previoh.Comments = InternalOrderDB.getUserComments(previoh.DocumentID, previoh.TemporaryNo, previoh.TemporaryDate);
                    previoh.InternalOrderNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["InternalOrderNo"].Value.ToString());
                    previoh.InternalOrderDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["InternalOrderDate"].Value.ToString());
                    previoh.ReferenceTrackingNos = grdList.Rows[e.RowIndex].Cells["ReferenceTrackingNo"].Value.ToString();
                    previoh.CustomerID = grdList.Rows[e.RowIndex].Cells["CustID"].Value.ToString();
                    previoh.CustomerName = grdList.Rows[e.RowIndex].Cells["CustName"].Value.ToString();
                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {

                        string hdrString = "Document Temp No:" + previoh.TemporaryNo + "\n" +
                            "Document Temp Date:" + previoh.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "Tracking No:" + previoh.DocumentID + "\n" +
                            "Tracking Date:" + previoh.InternalOrderDate.ToString("dd-MM-yyyy") + "\n" +
                            "Customer:" + previoh.CustomerName;

                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = previoh.TemporaryNo + "-" + previoh.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_1(null, null);
                        return;
                    }
                    //--------
                    previoh.TargetDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["TargetDate"].Value.ToString());
                    previoh.Remarks = grdList.Rows[e.RowIndex].Cells["Remarks"].Value.ToString();
                    previoh.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString());
                    previoh.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentStatus"].Value.ToString());
                    previoh.AcceptanceStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["AcceptanceStatus"].Value.ToString());
                    previoh.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["CreateTime"].Value.ToString());
                    previoh.CreateUser = grdList.Rows[e.RowIndex].Cells["CreateUser"].Value.ToString();
                    previoh.ApproveUser = grdList.Rows[e.RowIndex].Cells["ApproveUser"].Value.ToString();
                    previoh.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    previoh.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    previoh.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    previoh.ClosingStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["ClosingStatus"].Value.ToString());

                    previoh.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    //--comments
                    //chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    previoh.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmntStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(previoh.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(previoh.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);
                    //--
                    string prevComments = previoh.Comments;
                    if (columnName == "CloseRequest")
                    {
                        if (previoh.ClosingStatus == 1)
                        {
                            MessageBox.Show("Previous closing request is pending.");
                            btnExit.Visible = true;
                            return;
                        }
                        showCLosingRequestPopUp();
                        btnExit.Visible = true;
                        //ListFilteredIOHeader(listOption);
                        return;
                    }
                    if (columnName == "Close")
                    {
                        if (previoh.ApproveUser != Login.userLoggedIn)
                        {
                            MessageBox.Show("Only approve user can close Internal Order.");
                            btnExit.Visible = true;
                            return;
                        }
                        if(previoh.ClosingStatus == 0)
                        {
                            MessageBox.Show("No request for closing this Internal Order");
                            btnExit.Visible = true;
                            return;
                        }
                        showpopUpForCLosing();
                        btnExit.Visible = true;
                        //ListFilteredIOHeader(listOption);
                        return;
                    }
                    //---
                    txtTemporarryNo.Text = previoh.TemporaryNo.ToString();
                    dtTempDate.Value = previoh.TemporaryDate;


                    txtInternalOrderNo.Text = previoh.InternalOrderNo.ToString();
                    try
                    {
                        dtInternalOrderDate.Value = previoh.InternalOrderDate;
                    }
                    catch (Exception)
                    {
                        dtInternalOrderDate.Value = DateTime.Parse("01-01-1900");
                    }

                    ////////cmbCustomer.SelectedIndex = cmbCustomer.FindString(previoh.CustomerID);
                    if (previoh.CustomerID != "" && previoh.CustomerName != "")
                    {
                        txtcustomer.Text = previoh.CustomerID + "-" + previoh.CustomerName;
                    }

                    //cmbCustomer.SelectedIndex =
                    //    Structures.ComboFUnctions.getComboIndex(cmbCustomer, previoh.CustomerID);
                    cmbProductType.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbProductType, previoh.SEFID);
                    //cmbCustomer.Text = prevpopi.CustomerName;
                    txtReferenceTrackingNo.Text = previoh.ReferenceTrackingNos;
                    try
                    {
                        dtTargetDate.Value = previoh.TargetDate;
                    }
                    catch (Exception)
                    {

                        dtTargetDate.Value = DateTime.Parse("01-01-1900");
                    }
                    ///cmbCustomer.SelectedIndex = cmbCustomer.FindStringExact(grdList.Rows[e.RowIndex].Cells["CustID"].Value.ToString() + "-" + grdList.Rows[e.RowIndex].Cells["CustID"].Value.ToString());
                    ////////cmbCustomer.SelectedIndex = cmbCustomer.FindString(previoh.CustomerID);
                    ////////cmbCustomer.SelectedIndex =
                    ////////    Structures.ComboFUnctions.getComboIndex(cmbCustomer, previoh.CustomerID);
                    txtRemarks.Text = previoh.Remarks.ToString();
                    List<iodetail> IODetail = InternalOrderDB.getIODetail(previoh);
                    grdIODetail.Rows.Clear();
                    int i = 0;
                    foreach (iodetail iod in IODetail)
                    {
                        if (!AddIODetailRow())
                        {
                            MessageBox.Show("Error found in IO details. Please correct before updating the details");
                        }
                        else
                        {
                            try
                            {
                                if (previoh.DocumentID.Equals("IOPRODUCT"))
                                {
                                    grdIODetail.Rows[i].Cells["ProductItem"].Value = iod.StockItemID + "-" + iod.StockItemName;
                                    grdIODetail.Rows[i].Cells["ModelName"].Value = iod.ModelName;
                                    grdIODetail.Rows[i].Cells["ModelNo"].Value = iod.ModelNo;
                                }
                                else
                                {
                                    grdIODetail.Rows[i].Cells["ServiceItem"].Value = iod.StockItemID + "-" + iod.StockItemName;
                                }
                            }
                            catch (Exception)
                            {
                                grdIODetail.Rows[i].Cells["Item"].Value = null;
                            }
                            if (docID == "IOSERVICE")
                            {
                                grdIODetail.Rows[i].Cells["OfficeID"].Value = iod.OfficeID;// + "-" + iod.OfficeName;
                            }
                            else
                            {
                                grdIODetail.Rows[i].Cells["OfficeID"].Value = null;
                            }
                            ////grdIODetail.Rows[i].Cells["WorkDescription"].Value = iod.WorkDescription;
                            grdIODetail.Rows[i].Cells["Specification"].Value = iod.Specification;
                            grdIODetail.Rows[i].Cells["Quantity"].Value = iod.Quantity;
                            grdIODetail.Rows[i].Cells["WarrantyDays"].Value = iod.WarrantyDays;
                            i++;
                        }
                    }
                    if (columnName == "Edit" || columnName == "View")
                    {
                        string refIOStr = previoh.ReferenceTrackingNos;
                        getTrackingDetails(refIOStr);
                    }
                    if (!verifyAndReworkIODetailGridRows())
                    {
                        MessageBox.Show("Error found in iO details. Please correct before updating the details");
                    }
                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                    //pnlBottomActions.Visible = false;
                    tabControl1.SelectedTab = tabIOHeader;
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
                    grdIODetail.Rows.Clear();
                    MessageBox.Show("Grid items cleared.");
                    verifyAndReworkIODetailGridRows();
                }

            }
            catch (Exception)
            {
            }
        }
        private Boolean updateDashBoard(ioheader ioh, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = ioh.DocumentID;
                dsb.TemporaryNo = ioh.TemporaryNo;
                dsb.TemporaryDate = ioh.TemporaryDate;
                dsb.DocumentNo = ioh.InternalOrderNo;
                dsb.DocumentDate = ioh.InternalOrderDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = ioh.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(previoh.DocumentID);
                    foreach (documentreceiver docRec in docList)
                    {
                        dsb.ToUser = docRec.EmployeeID;    //To store UserID Form DocumentReceiver for current Document
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
                InternalOrderDB iodb = new InternalOrderDB();

                ioheader ioh = new ioheader();

                DialogResult dialog = MessageBox.Show("Are you sure to approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    previoh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(previoh.CommentStatus);
                    if (previoh.Status != 96)
                    {
                        previoh.InternalOrderNo = DocumentNumberDB.getNewNumber(docID, 2);
                    }
                    if (iodb.ApproveIOHeader(previoh))
                    {
                        MessageBox.Show("IO Document Approved");
                        if (!updateDashBoard(previoh, 2))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                        }
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredIOHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void cmbPOType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //string iostr = 
                //string iotype = cmbIOType.SelectedItem.ToString().Trim().Substring(0, cmbIOType.SelectedItem.ToString().Trim().IndexOf('-'));
                string iotype = ((Structures.ComboBoxItem)cmbIOType.SelectedItem).HiddenValue; ;
                if (iotype == "ProductIO")
                {
                    docID = "IOPRODUCT";
                    chkDocID = "IOPRODUCT";

                }
                else if (iotype == "ServiceIO")
                {
                    docID = "IOSERVICE";
                    chkDocID = "IOSERVICE";

                }
                setIODetailColumns(docID);
                cmbIOType.Enabled = false;
            }
            catch (Exception)
            {
            }
        }
        private void setIODetailColumns(string docID)
        {
            try
            {
                if (docID == "IOPRODUCT")
                {
                    label10.Visible = true;
                    cmbProductType.Visible = true;
                    poDocID = "POPRODUCTINWARD";
                    btnSpecification.Visible = false;
                }
                else if (docID == "IOSERVICE")
                {
                    poDocID = "POSERVICEINWARD";
                    label10.Visible = false;
                    cmbProductType.Visible = false;
                    btnSpecification.Visible = false;
                }
            }
            catch (Exception)
            {
            }
        }

        ////private void lstServiceItems_Click(object sender, EventArgs e)
        ////{
        ////    try
        ////    {
        ////        grdIODetail.Rows[grdIODetail.CurrentCell.RowIndex].Cells["WorkDescription"].Value = lstServiceItems.SelectedItem.ToString();
        ////        lstServiceItems.Visible = false;
        ////    }
        ////    catch (Exception)
        ////    {
        ////    }
        ////}

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //New PO
            try
            {
                if (btnSave.Text == "Save")

                {
                    if (cmbIOType.SelectedIndex == -1)
                    {
                        tabControl1.SelectedIndex = 0;
                    }
                }
                else
                {
                    if (tabControl1.SelectedIndex == 0)
                    {
                        tabControl1.SelectedIndex = 1;
                    }
                }
                string selTab = tabControl1.SelectedTab.Name;
                if (selTab == "tabPODetail" && (colClicked == "Edit" || isNewClick == true))
                {

                    if (txtReferenceTrackingNo.Text.Trim().Length != 0)
                    {
                        string refINStr = txtReferenceTrackingNo.Text.Trim();
                        getTrackingDetails(refINStr);
                    }
                    else
                        MessageBox.Show("Reference Tracking is Empty ");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSelectTrackingNo_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdIODetail.Rows.Count != 0)
                {
                    DialogResult dialog = MessageBox.Show("IO Details Will be removed ?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        grdIODetail.Rows.Clear();
                    }
                    else
                    {
                        return;
                    }
                }
                frmPopup = new Form();
                subColName1 = "";
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.CadetBlue;
                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(1000, 370);
                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(620, 5);
                lblSearch.Text = "Search Here";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(750, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInEmpGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                POPIHeaderDB popiDB = new POPIHeaderDB();
                grdRefSel = popiDB.getGridViewForGivenListOfItems(poDocID);
                grdRefSel.ColumnHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(grdRefSel_ColumnHeaderMouseDoubleClick);
                grdRefSel.Bounds = new Rectangle(new Point(0, 27), new Size(1000, 300));
                //grdRefSel.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.grdEmplList_ColumnHeaderMouseClick);
                frmPopup.Controls.Add(grdRefSel);

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grdOK_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 335);
                lvCancel.Click += new System.EventHandler(this.grdCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdOK_Click1(object sender, EventArgs e)
        {
            try
            {
                var checkedRows = from DataGridViewRow r in grdRefSel.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount == 0)
                {
                    MessageBox.Show("Select one Tracking No");
                    return;
                }
                string trlist;
                trlist = "";
                int trackNO = 0;
                foreach (var row in checkedRows)
                {
                    trlist = trlist + poDocID + ";" + row.Cells["TrackingNo"].Value.ToString() + "("
                                + Convert.ToDateTime(row.Cells["TrackingDate"].Value).ToString("yyyy-MM-dd") + ")" + Main.delimiter1 + Environment.NewLine;
                    if (trackNO < Convert.ToInt32(row.Cells["TrackingNo"].Value.ToString()))
                    {
                        txtcustomer.Text = row.Cells["CustID"].Value + " - " + row.Cells["CustName"].Value;
                        trackNO = Convert.ToInt32(row.Cells["TrackingNo"].Value.ToString());
                    }
                }
                txtReferenceTrackingNo.Text = trlist;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void grdCancel_Click1(object sender, EventArgs e)
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

        private void grdRefSel_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                subColName1 = grdRefSel.Columns[e.ColumnIndex].Name;
                foreach (DataGridViewColumn col in grdRefSel.Columns)
                {
                    col.HeaderCell.Style.BackColor = Color.LightSeaGreen;
                }
                grdRefSel.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSeaGreen;
                if (subColName1 == "Select")
                {
                    subColName1 = "";
                }
                else
                    grdRefSel.Columns[e.ColumnIndex].HeaderCell.Style.BackColor = Color.Magenta;
            }
            catch (Exception ex)
            {
            }
        }
        private void txtSearch_TextChangedInEmpGridList(object sender, EventArgs e)
        {
            try
            {
                filterTimer.Enabled = false;
                filterTimer.Stop();
                filterTimer.Tick -= new System.EventHandler(this.handlefilterTimerTimeout);
                filterTimer.Tick += new System.EventHandler(this.handlefilterTimerTimeout);
                filterTimer.Interval = 500;
                filterTimer.Enabled = true;
                filterTimer.Start();

                //filterGridData();
            }
            catch (Exception ex)
            {

            }
        }
        private void handlefilterTimerTimeout(object sender, EventArgs e)
        {
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterGridData(subColName1);
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }
        private void filterGridData(string subColName1)
        {
            try
            {
                grdRefSel.CurrentCell = null;
                foreach (DataGridViewRow row in grdRefSel.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdRefSel.Rows)
                    {
                        if (subColName1 == null || subColName1.Length == 0)
                        {
                            if (!row.Cells["ReferenceNo"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
                            {
                                row.Visible = false;
                            }
                        }
                        else
                        {

                            if (!row.Cells[subColName1].FormattedValue.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
                            {
                                row.Visible = false;
                            }
                        }
                        //if (!row.Cells["ReferenceNo"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
                        //{
                        //    row.Visible = false;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Searching");
            }
        }
        private void lvOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkLVItemChecked("Item"))
                {
                    return;
                }
                string trlist;
                trlist = "";
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        //MessageBox.Show(itemRow.SubItems[1].Text);
                        trlist = trlist + itemRow.SubItems[1].Text + "(" + itemRow.SubItems[2].Text + ");";
                        txtcustomer.Text = itemRow.SubItems[5].Text;
                    }
                }
                txtReferenceTrackingNo.Text = trlist;
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
                //btnSelectTrackingNo.Enabled = true;
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

        private void btnIOReferenceOption_Click(object sender, EventArgs e)
        {
            if (grdIODetail.Rows.Count != 0)
            {
                DialogResult dialog = MessageBox.Show("IO Details Will be removed ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdIODetail.Rows.Clear();
                }
                else
                {
                    return;
                }
            }
            string IORefOpt = "IOReferenceOptions";
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            lv = CatalogueValueDB.getCatalogValueListView(IORefOpt);
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
        }
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                if (!checkLVItemChecked("Item"))
                {
                    return;
                }
                //btnIOReferenceOption.Enabled = true;

                ////ArrayList lviItemsArrayList = new ArrayList();
                string trlist;
                trlist = "";
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        //MessageBox.Show(itemRow.SubItems[1].Text);
                        trlist = trlist + itemRow.SubItems[1].Text + ";";
                        //cmbCustomer.SelectedIndex = -1;
                        txtcustomer.Text = "";
                    }
                }
                txtReferenceTrackingNo.Text = trlist;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_Click1(object sender, EventArgs e)
        {
            try
            {
                //btnIOReferenceOption.Enabled = true;
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

        private void tabIOType_Click(object sender, EventArgs e)
        {

        }


        //--
        //-----
        //comment handling procedurs and functions
        //-----
        private void btnGetComments_Click(object sender, EventArgs e)
        {
            //removeControlsFromCommenterPanel();
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
            lvOK.Click += new System.EventHandler(this.lvOK_Click2);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
            frmPopup.Controls.Add(lvCancel);
            ////lvCancel.Visible = true;
            frmPopup.ShowDialog();
            //pnlCmtr.BringToFront();
            //pnlCmtr.Visible = true;
            //pnlComments.Controls.Add(pnlCmtr);
            //pnlComments.BringToFront();
            //pnlCmtr.BringToFront();
        }
        private void lvOK_Click2(object sender, EventArgs e)
        {
            try
            {
                commentStatus = "";
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
                                "0" + Main.delimiter2;
                        }
                    }
                }
                else
                {
                    //if the existing commenter are removed
                    commentStatus = "Cleared";
                }
                frmPopup.Close();
                //frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                frmPopup.Close();
                //frmPopup.Dispose();
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
                            InternalOrderDB iodb = new InternalOrderDB();
                            previoh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(previoh.CommentStatus);
                            previoh.ForwardUser = approverUID;
                            previoh.ForwarderList = previoh.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (iodb.forwardIOHeader(previoh))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(previoh, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredIOHeader(listOption);
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
        private void btnReverse_Click_1(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Are you sure to Reverse the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    string reverseStr = getReverseString(previoh.ForwarderList);
                    //do forward activities
                    previoh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(previoh.CommentStatus);
                    InternalOrderDB iodb = new InternalOrderDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        previoh.ForwarderList = reverseStr.Substring(0, ind);
                        previoh.ForwardUser = reverseStr.Substring(ind + 3);
                        previoh.DocumentStatus = previoh.DocumentStatus - 1;
                    }
                    else
                    {
                        previoh.ForwarderList = "";
                        previoh.ForwardUser = "";
                        previoh.DocumentStatus = 1;
                    }
                    if (iodb.reverseIO(previoh))
                    {
                        MessageBox.Show("Internal Order Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredIOHeader(listOption);
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

        private void btnReverse_Click(object sender, EventArgs e)
        {

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
        private void btnPDFClose_Click_1(object sender, EventArgs e)
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
        private void removeControlsFromTrackNoReferencePanel()
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
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, previoh.TemporaryNo + "-" + previoh.TemporaryDate.ToString("yyyyMMddhhmmss"));
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
                    string subDir = previoh.TemporaryNo + "-" + previoh.TemporaryDate.ToString("yyyyMMddhhmmss");
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
                ///chkCommentStatus.Visible = false;
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
                    //chkCommentStatus.Visible = true;
                    txtComments.Visible = true;

                    tabControl1.SelectedTab = tabIOHeader;
                    tabIOHeader.Visible = true;
                    tabIOType.Visible = false;
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
                    tabControl1.SelectedTab = tabIOType;
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
                    //chkCommentStatus.Visible = true;
                    txtComments.Visible = true;
                    tabControl1.SelectedTab = tabIOHeader;

                    tabIOHeader.Visible = true;
                    tabIOType.Visible = false;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                    disableTabPages();
                    tabControl1.SelectedTab = tabIOHeader;

                    tabIOHeader.Visible = true;
                    tabIOType.Visible = false;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabIOHeader;

                    tabIOHeader.Visible = true;
                    tabIOType.Visible = false;
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
                //Main.itemPriv[0]: View
                //Main.itemPriv[1]: Add
                //Main.itemPriv[2]: Edit
                //Main.itemPriv[3]: Delete
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
                /////chkCommentStatus.Checked = false;
                txtComments.Text = "";
                grdIODetail.Rows.Clear();
            }
            catch (Exception ex)
            {
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
        //-- Validating Header and Detail String For Single Quotes

        private ioheader verifyHeaderInputString(ioheader ioh)
        {
            try
            {
                ioh.Remarks = Utilities.replaceSingleQuoteWithDoubleSingleQuote(ioh.Remarks);
                ioh.CustomerID = Utilities.replaceSingleQuoteWithDoubleSingleQuote(ioh.CustomerID);
            }
            catch (Exception ex)
            {
            }
            return ioh;
        }
        private void verifyDetailInputString()
        {
            try
            {
                for (int i = 0; i < grdIODetail.Rows.Count; i++)
                {
                    if (docID == "IOPRODUCT")
                    {
                        grdIODetail.Rows[i].Cells["ProductItem"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdIODetail.Rows[i].Cells["ProductItem"].Value.ToString());
                        grdIODetail.Rows[i].Cells["ModelNo"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdIODetail.Rows[i].Cells["ModelNo"].Value.ToString());
                    }
                    else
                    {
                        grdIODetail.Rows[i].Cells["ServiceItem"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdIODetail.Rows[i].Cells["ServiceItem"].Value.ToString());
                    }


                    grdIODetail.Rows[i].Cells["Specification"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdIODetail.Rows[i].Cells["Specification"].Value.ToString());

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSecification_Click(object sender, EventArgs e)
        {
            if (cmbProductType.SelectedIndex == -1)
            {
                MessageBox.Show("select product type.");
                return;
            }
            string sefid = ((Structures.ComboBoxItem)cmbProductType.SelectedItem).HiddenValue;
            InternalOrderDB ioDB = new InternalOrderDB();
            List<iorequirements> ioreqList = new List<iorequirements>();
            if (isEditClick && iorequirementsfilled.Count != 0 && isChangedSpecDet == false)
                ioreqList = ioDB.GetTemplatesForPerticularIO(previoh.TemporaryNo, previoh.TemporaryDate);
            else if (isEditClick && iorequirementsfilled.Count != 0 && isChangedSpecDet == true)
                ioreqList = iorequirementsfilled;
            else if (!isEditClick && iorequirementsfilled.Count != 0)
                ioreqList = iorequirementsfilled;
            else
                ioreqList = ioDB.getTemplateListForProductType(sefid);

            //ioreqList = ioDB.getTemplateListForProductType(sefid); //uncomment this line if one internal order is saved without filling specification

            FileManager.IORequirement showSpec = new FileManager.IORequirement(ioreqList, sefid);
            showSpec.ShowDialog();
            if (showSpec.DialogResult == DialogResult.OK)
            {
                isChangedSpecDet = true;
                iorequirementsfilled = showSpec.ReturnValue1;
            }
            else if (showSpec.DialogResult == DialogResult.Cancel && isEditClick)
            {

            }
            this.RemoveOwnedForm(showSpec);
        }
        //string chkType = "";
        private void cmbProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (isEditClick)
                {
                    string str = ((Structures.ComboBoxItem)cmbProductType.SelectedItem).HiddenValue; //Trows exception in case of serviceIO. Remaining process Continue.
                    if (chkType != str)
                    {
                        DialogResult dialog = MessageBox.Show("Product Type with all Specification Details will be Removed.", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            //InternalOrderDB.deleteSpecificationOfIO(previoh.TemporaryNo, previoh.TemporaryDate);
                            iorequirementsfilled = new List<iorequirements>();
                        }
                        else
                        {
                            cmbProductType.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbProductType, chkType);
                            return;
                        }
                    }
                }
                else if (chkType.Length != 0 && chkType != ((Structures.ComboBoxItem)cmbProductType.SelectedItem).HiddenValue)
                {
                    iorequirementsfilled = new List<iorequirements>();
                }
                btnSpecification.Visible = true;
                if (cmbProductType.SelectedIndex != -1)
                    chkType = ((Structures.ComboBoxItem)cmbProductType.SelectedItem).HiddenValue;
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSelItemDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtReferenceTrackingNo.Text.Trim().Length == 0)
                {
                    MessageBox.Show("select ref Tracking No.");
                    return;
                }
                showItemDetailsForSelectedTrack();

            }
            catch (Exception ex)
            {
            }
        }
        private void showItemDetailsForSelectedTrack()
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(1000, 300);
            lv = getLVForAllSelectedPO(txtReferenceTrackingNo.Text.Trim());
            lv.Columns[1].Width = 0;
            lv.Columns[2].Width = 0;
            lv.Columns[3].Width = 0;
            lv.Columns[8].Width = 0;
            lv.Columns[9].Width = 0;
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(1000, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_ClickItem);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_ClickItem);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
        }
        private void lvOK_ClickItem(object sender, EventArgs e)
        {
            try
            {
                if (!checkLVItemChecked("Item"))
                {
                    return;
                }
                string trlist;
                trlist = "";
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        AddGridRowsForSelectedItem(itemRow);
                        break;
                    }
                }
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_ClickItem(object sender, EventArgs e)
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
        //lv.Columns.Add("Select", -2, HorizontalAlignment.Left); 0
        //lv.Columns.Add("RefNo", -2, HorizontalAlignment.Left); 1
        //lv.Columns.Add("TempNo", -2, HorizontalAlignment.Left); 2
        //lv.Columns.Add("TempDate", -2, HorizontalAlignment.Left); 3
        //lv.Columns.Add("TrackingNo", -2, HorizontalAlignment.Left); 4
        //lv.Columns.Add("TrackingDate", -2, HorizontalAlignment.Left); 5
        //lv.Columns.Add("StockItemID", -2, HorizontalAlignment.Left); 6 
        //lv.Columns.Add("stockItemName", -2, HorizontalAlignment.Left); 7
        //lv.Columns.Add("ModelNo", -2, HorizontalAlignment.Left); 8 
        //lv.Columns.Add("ModelName", -2, HorizontalAlignment.Left); 9 
        //lv.Columns.Add("Location", -2, HorizontalAlignment.Left); 10
        //lv.Columns.Add("Cust.ItemDescription", -2, HorizontalAlignment.Left); 11
        //lv.Columns.Add("Quantity", -2, HorizontalAlignment.Left); 12
        //lv.Columns.Add("Price", -2, HorizontalAlignment.Left); 13
        //lv.Columns.Add("WarrantyDays", -2, HorizontalAlignment.Left); 14
        //lv.Columns.Add("TaxCode", -2, HorizontalAlignment.Left); 15
        //grdInvoiceOutDetail.Rows.Add();
        private void AddGridRowsForSelectedItem(ListViewItem item)
        {
            AddRowClick = true;
            if (!AddIODetailRow())
            {
                MessageBox.Show("Adding Rows Failed");
                return;
            }
            else
            {
                try
                {
                    if (docID.Equals("IOPRODUCT"))
                    {
                        grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells["ProductItem"].Value = item.SubItems[6].Text + "-" + item.SubItems[7].Text;
                        grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells["ModelName"].Value = item.SubItems[8].Text;
                        grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells["ModelNo"].Value = item.SubItems[9].Text;
                    }
                    else
                    {
                        grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells["ServiceItem"].Value = item.SubItems[6].Text + "-" + item.SubItems[7].Text;
                    }
                }
                catch (Exception)
                {
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells["Item"].Value = null;
                }
                if (docID == "IOSERVICE")
                {
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells["OfficeID"].Value = "";// + "-" + iod.OfficeName;
                }
                else
                {
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells["OfficeID"].Value = "";
                }
                ////grdIODetail.Rows[i].Cells["WorkDescription"].Value = iod.WorkDescription;
                grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells["Specification"].Value = item.SubItems[11].Text;
                grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells["Quantity"].Value = item.SubItems[12].Text;
                grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells["WarrantyDays"].Value = item.SubItems[14].Text;
            }
        }

        private void btnPAF_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdIODetail.Rows.Count != 0)
                {
                    DialogResult dialog = MessageBox.Show("IO Details Will be removed ?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        grdIODetail.Rows.Clear();
                    }
                    else
                    {
                        return;
                    }
                }
                frmPopup = new Form();
                subColName1 = "";
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.CadetBlue;

                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(1000, 370);

                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(620, 5);
                lblSearch.Text = "Search Here";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(750, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInEmpGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                POPIHeaderDB popiDB = new POPIHeaderDB();
                string PAFDocid = getPAFPODocID(docID);
                grdRefSel = popiDB.getGridViewForGivenListOfItems(PAFDocid);
                grdRefSel.ColumnHeaderMouseDoubleClick += new DataGridViewCellMouseEventHandler(grdRefSel_ColumnHeaderMouseDoubleClick);
                grdRefSel.Bounds = new Rectangle(new Point(0, 27), new Size(1000, 300));
                //grdRefSel.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(this.grdEmplList_ColumnHeaderMouseClick);
                frmPopup.Controls.Add(grdRefSel);

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grdOK_PAFClick1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 335);
                lvCancel.Click += new System.EventHandler(this.grdCancel_PAFClick1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private string getPAFPODocID(string IODocID)
        {
            string id = "";
            if (IODocID == "IOPRODUCT")
                id = "PAFPRODUCTINWARD";
            else if (IODocID == "IOSERVICE")
                id = "PAFSERVICEINWARD";
            return id;
        }
        private void grdOK_PAFClick1(object sender, EventArgs e)
        {
            try
            {
                var checkedRows = from DataGridViewRow r in grdRefSel.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount == 0)
                {
                    MessageBox.Show("Select one PO");
                    return;
                }
                string trlist;
                trlist = "";
                string PAFDocid = getPAFPODocID(docID);
                int trackNO = 0;
                foreach (var row in checkedRows)
                {
                    trlist = trlist + PAFDocid + ";" + row.Cells["TrackingNo"].Value.ToString() + "("
                                + Convert.ToDateTime(row.Cells["TrackingDate"].Value).ToString("yyyy-MM-dd") + ")" + Main.delimiter1 + Environment.NewLine;
                    if (trackNO < Convert.ToInt32(row.Cells["TrackingNo"].Value.ToString()))
                    {
                        txtcustomer.Text = row.Cells["CustID"].Value + " - " + row.Cells["CustName"].Value;
                        trackNO = Convert.ToInt32(row.Cells["TrackingNo"].Value.ToString());
                    }
                }
                txtReferenceTrackingNo.Text = trlist;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        private void grdCancel_PAFClick1(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.CadetBlue;

                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(510, 370);

                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(150, 5);
                lblSearch.Text = "Search by Name";
                lblSearch.AutoSize = true;
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(280, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtCustSearch_TextChangedInEmpGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                CustomerDB custDB = new CustomerDB();
                grdCustList = custDB.getGridViewForCustomerList("Customer");
                grdCustList.Bounds = new Rectangle(new Point(0, 27), new Size(510, 300));
                frmPopup.Controls.Add(grdCustList);

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grdCustOK_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 335);
                lvCancel.Click += new System.EventHandler(this.grdCustCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdCustOK_Click1(object sender, EventArgs e)
        {
            try
            {
                var checkedRows = from DataGridViewRow r in grdCustList.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("Select one item");
                    return;
                }
                string trlist;
                trlist = "";
                foreach (var row in checkedRows)
                {
                    trlist = row.Cells["CustomerID"].Value + " - " + row.Cells["CustomerName"].Value;
                }
                txtcustomer.Text = trlist;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdCustCancel_Click1(object sender, EventArgs e)
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
        private void txtCustSearch_TextChangedInEmpGridList(object sender, EventArgs e)
        {
            try
            {
                filterCustGridData();
            }
            catch (Exception ex)
            {

            }
        }
        private void filterCustGridData()
        {
            try
            {
                grdCustList.CurrentCell = null;
                foreach (DataGridViewRow row in grdCustList.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdCustList.Rows)
                    {
                        if (!row.Cells["CustomerName"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
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

        private ListView getLVForAllSelectedPO(string fullRefTrackStr)
        {
            ListView lvTot = new ListView();
            try
            {
                lvTot.View = System.Windows.Forms.View.Details;
                lvTot.LabelEdit = true;
                lvTot.AllowColumnReorder = true;
                lvTot.CheckBoxes = true;
                lvTot.FullRowSelect = true;
                lvTot.GridLines = true;
                lvTot.Sorting = System.Windows.Forms.SortOrder.Ascending;
                lvTot.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("RefNo", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TempNo", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TempDate", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TrackingNo", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TrackingDate", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("StockItemID", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("stockItemName", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("ModelNo", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("ModelName", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Location", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Cust.ItemDescription", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Quantity", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("Price", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("WarrantyDays", -2, HorizontalAlignment.Left);
                lvTot.Columns.Add("TaxCode", -2, HorizontalAlignment.Left);
                string[] mainStr = fullRefTrackStr.Trim().Split(Main.delimiter1);
                foreach (string str in mainStr)
                {
                    if (str.Length != 0)
                    {
                        string[] strRef = str.Trim().Split(';');
                        string DocIDStr = strRef[0]; //DocID
                        int trackNo1 = Convert.ToInt32(strRef[1].Substring(0, strRef[1].IndexOf('('))); //TrackNo
                        int findex = strRef[1].IndexOf('(');
                        int sindex = strRef[1].IndexOf(')');
                        string tstr = strRef[1].Substring(findex + 1, (sindex - findex) - 1);
                        DateTime trackDate1 = Convert.ToDateTime(tstr); //TrackDate

                        ListView lv =
                          POPIHeaderDB.getPONoWiseStockListView(trackNo1, trackDate1, DocIDStr);
                        lvTot.Items.AddRange((from ListViewItem item in lv.Items
                                              select (ListViewItem)item.Clone()).ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception in showing PODetail Listview.Check Reference Tracking No. ");
            }
            return lvTot;
        }

        private void getTrackingDetails(string refTrackStr)
        {
            try
            {
                grdTrackingDetail.Rows.Clear();
                grdItemWiseTotalQuant.Rows.Clear();
                tabControl1.TabPages["tabPODetail"].Visible = true;
                tabControl2.SelectedTab = tabControl2.TabPages["tabDetail"];
                grdTrackingDetail.Visible = true;
                grdItemWiseTotalQuant.Visible = true;
                dictItemWiseTOt = new Dictionary<string, string[]>();
                string[] mainStr = refTrackStr.Trim().Split(Main.delimiter1);
                foreach (string str in mainStr)
                {
                    if (str.Length != 0)
                    {
                        string[] strRef = str.Trim().Split(';');
                        string DocIDStr = strRef[0]; //DocID
                        int trackNo1 = Convert.ToInt32(strRef[1].Substring(0, strRef[1].IndexOf('('))); //TrackNo
                        int findex = strRef[1].IndexOf('(');
                        int sindex = strRef[1].IndexOf(')');
                        string tstr = strRef[1].Substring(findex + 1, (sindex - findex) - 1);
                        DateTime trackDate1 = Convert.ToDateTime(tstr); //TrackDate
                        popiheader popih = new popiheader();
                        popih.DocumentID = DocIDStr;
                        popih.TrackingNo = trackNo1;
                        popih.TrackingDate = trackDate1;
                        List<popidetail> POPIList = POPIHeaderDB.getPOPIDetailForIO(popih);
                        addGridTrackingDetail(POPIList, trackNo1, trackDate1);
                    }
                }
                int j = 0;
                foreach (KeyValuePair<string, string[]> kvp in dictItemWiseTOt)
                {
                    grdItemWiseTotalQuant.Rows.Add();
                    grdItemWiseTotalQuant.Rows[j].Cells["ItemID"].Value = kvp.Key;
                    grdItemWiseTotalQuant.Rows[j].Cells["StockItemName"].Value = kvp.Value[0];
                    grdItemWiseTotalQuant.Rows[j].Cells["TotalQuantity"].Value = kvp.Value[1];
                    j++;
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void addGridTrackingDetail(List<popidetail> POPIList, int TrackNo, DateTime Trackdate)
        {
            try
            {
                int i = grdTrackingDetail.Rows.Count;
                foreach (popidetail ind in POPIList)
                {
                    grdTrackingDetail.Rows.Add();
                    grdTrackingDetail.Rows[i].Cells["gDocID"].Value = ind.DocumentID;
                    grdTrackingDetail.Rows[i].Cells["gTrackingNo"].Value = TrackNo.ToString();
                    grdTrackingDetail.Rows[i].Cells["gTrackingDate"].Value = Trackdate;
                    grdTrackingDetail.Rows[i].Cells["gItemID"].Value = ind.StockItemID;
                    grdTrackingDetail.Rows[i].Cells["gItemName"].Value = ind.StockItemName;
                    grdTrackingDetail.Rows[i].Cells["gDescription"].Value = ind.CustomerItemDescription;
                    grdTrackingDetail.Rows[i].Cells["gQuantity"].Value = ind.Quantity;
                    grdTrackingDetail.Rows[i].Cells["gWarranty"].Value = ind.WarrantyDays;
                    grdTrackingDetail.Rows[i].Cells["gApproverName"].Value = ind.WorkDescription;   //Approver Name
                    if (dictItemWiseTOt.ContainsKey(ind.StockItemID))
                    {
                        double quant = Convert.ToDouble(dictItemWiseTOt[ind.StockItemID][1]);
                        double tot = quant + ind.Quantity;
                        dictItemWiseTOt[ind.StockItemID] = new string[] { ind.StockItemName, tot.ToString() };
                    }
                    else
                    {
                        dictItemWiseTOt.Add(ind.StockItemID, new string[] { ind.StockItemName, ind.Quantity.ToString() });
                    }
                    i++;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void InternalOrder_Enter(object sender, EventArgs e)
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

