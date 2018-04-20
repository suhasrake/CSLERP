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
    public partial class GatePassHeader : System.Windows.Forms.Form
    {
        string docID = "GATEPASS";
        string forwarderList = "";
        string approverList = "";
        Timer filterTimer1 = new Timer();
        TextBox txtSearchCust = new TextBox();
        string userString = "";
        DataGridView grdCustList = new DataGridView();
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Boolean IsEditMode = false;
        Boolean IsViewMode = false;
        Boolean userIsACommenter = false;
        CheckedListBox chkBoxCustomer = new CheckedListBox();
        Boolean track = false;
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        gatepassheader prevgtn;
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
        string office = "";
        Boolean isnewClick = false;
        public GatePassHeader()
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
                if (office != "NVP")
                {
                    option = 4;
                    listOption = 4;
                }
                grdList.Rows.Clear();
                GatePassDB gdb = new GatePassDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);

                List<gatepassheader> GPHeaders = gdb.getFilteredGPHeader(userString, option, userCommentStatusString);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (gatepassheader gtnh in GPHeaders)
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
                    grdList.Rows[grdList.RowCount - 1].Cells["GatePassNo"].Value = gtnh.GatePassNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["GatePassDate"].Value = gtnh.GatePassDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["FromOffice"].Value = gtnh.FromOffice;
                    grdList.Rows[grdList.RowCount - 1].Cells["FromOfficeName"].Value = gtnh.FromOfficeName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ToOffice"].Value = gtnh.ToOffice;
                    grdList.Rows[grdList.RowCount - 1].Cells["ToOfficeName"].Value = gtnh.ToOfficeName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerID"].Value = gtnh.CustomerID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerName"].Value = gtnh.CustomerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = gtnh.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["CmtStatus"].Value = gtnh.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["AcceptanceDate"].Value = gtnh.AcceptanceDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["AcceptanceStatus"].Value = gtnh.AcceptanceStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["AccpetanceString"].Value = gtnh.AcceptanceStatus == 1 ? "Accepted" : "";
                    grdList.Rows[grdList.RowCount - 1].Cells["AcceptedUser"].Value = gtnh.AcceptedUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = gtnh.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = gtnh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = gtnh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = gtnh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = gtnh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = gtnh.ForwardUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = gtnh.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = gtnh.ForwarderList;
                    grdList.Rows[grdList.RowCount - 1].Cells["ReturnStatus"].Value = gtnh.ReturnStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Remarks"].Value = gtnh.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["SpecialNotes"].Value = gtnh.SpecialNotes;
                    if (gtnh.ReturnStatus > 0)
                    {
                        grdList.Rows[grdList.RowCount - 1].DefaultCellStyle.BackColor = Color.Tan;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in GatePass Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;
            grdList.Columns["Accept"].Visible = false;
            grdList.Columns["Return"].Visible = false;
            grdList.Columns["AcceptReturned"].Visible = false;
            if (listOption == 4)
            {
                btnActionPending.Visible = false;
                btnApproved.Visible = false;
                btnNew.Visible = false;

            }
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                if (listOption == 4)
                {
                    grdList.Columns["Accept"].Visible = true;
                    grdList.Columns["Return"].Visible = true;
                }
                else if (listOption == 6)
                {
                    grdList.Columns["AcceptReturned"].Visible = true;
                }
            }
        }

        //called only in the beginning
        private void initVariables()
        {

            if (getuserPrivilegeStatus() == 1)
            {
                //user is only a viewer
                listOption = 6;
            }
            OfficeDB.fillOfficeComboNew(cmbFromOffice);
            OfficeDB.fillOfficeComboNew(cmbTOOffice);
            cmbFromOffice.Enabled = true;
            cmbTOOffice.Enabled = true;
            cmbFromOffice.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbFromOffice, "NVP");
            cmbTOOffice.Width = 250;
            cmbFromOffice.Width = 250;
            dtTempDate.Format = DateTimePickerFormat.Custom;
            dtTempDate.CustomFormat = "dd-MM-yyyy";
            dtTempDate.Enabled = false;
            dtGetPassDate.Format = DateTimePickerFormat.Custom;
            dtGetPassDate.CustomFormat = "dd-MM-yyyy";

            dtGetPassDate.Enabled = false;
            txtTemporarryNo.Enabled = false;
            dtTempDate.Enabled = false;
            txtGetPassNo.Enabled = false;
            dtGetPassDate.Enabled = false;

            //btnApprove.Visible = false;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            grdGPDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //---
            txtComments.Text = "";

            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
            setTabIndex();
            office = EmployeePostingDB.getCurrentOffice(Login.empLoggedIn);
        }
        private void setTabIndex()
        {
            txtTemporarryNo.TabIndex = 0;
            dtTempDate.TabIndex = 1;
            txtGetPassNo.TabIndex = 2;
            dtGetPassDate.TabIndex = 3;
            cmbFromOffice.TabIndex = 4;
            cmbTOOffice.TabIndex = 5;
            txtCustomerID.TabIndex = 6;
            txtCustomerName.TabIndex = 7;
            txtRemarks.TabIndex = 8;
            txtSpecialNote.TabIndex = 9;

            btnCancel.TabIndex = 1;
            btnSave.TabIndex = 2;
            btnAcceptReturn.TabIndex = 3;
            btnReturn.TabIndex = 4;
            btnAccept.TabIndex = 5;
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
                grdGPDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                isnewClick = false;
                cmbFromOffice.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbFromOffice, "NVP");
                //----------clear temperory panels
                ///cmbTOLocation.Enabled = false;
                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                //----------
                cmbTOOffice.SelectedIndex = -1;
                txtTemporarryNo.Text = "";
                txtCustomerName.Text = "";
                txtRemarks.Text = "";
                txtSpecialNote.Text = "";
                dtTempDate.Value = DateTime.Parse("1900-01-01");
                txtGetPassNo.Text = "";
                dtGetPassDate.Value = DateTime.Parse("1900-01-01");
                txtCustomerID.Text = "";
                track = false;
                prevgtn = new gatepassheader();
                AddRowClick = false;
                grdGPDetail.Columns["Quantity"].ReadOnly = false;
                grdGPDetail.Columns["Value"].ReadOnly = false;
                grdGPDetail.Columns["ReturningQuantity"].ReadOnly = true;
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
                isnewClick = true;
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
                AddGPDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddGPDetailRow()
        {
            Boolean status = true;
            //AddRowClick = true;
            try
            {
                if (grdGPDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkGPDetailGridRows())
                    {
                        return false;
                    }
                }
                grdGPDetail.Rows.Add();
                int kount = grdGPDetail.RowCount;
                grdGPDetail.Rows[kount - 1].Cells["LineNo"].Value = kount;
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["RowID"].Value = Convert.ToInt32(0);
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["Item"].Value = "";
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["AvailableQuantity"].Value = Convert.ToDouble(0);
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["Quantity"].Value = Convert.ToDouble(0);
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["Value"].Value = Convert.ToDouble(0);
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["ModelNo"].Value = "";
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["ModelName"].Value = "";
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["ReturnedQuantity"].Value = Convert.ToDouble(0);
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["ReturningQuantity"].Value = Convert.ToDouble(0);
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["StockRefNo"].Value = Convert.ToInt32(0);
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["MRNNo"].Value = Convert.ToInt32(0);
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["MRNDate"].Value = DateTime.Parse("1900-01-01");
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["BatchNo"].Value = "";
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["SerialNo"].Value = "";
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["ExpiryDate"].Value = DateTime.Parse("1900-01-01");
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["PurchaseQuantity"].Value = Convert.ToDouble(0);
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["PurchasePrice"].Value = Convert.ToDouble(0);
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["PurchaseTax"].Value = Convert.ToDouble(0);
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["SupplierID"].Value = "";
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["SupplierName"].Value = "";
                if (AddRowClick)
                {
                    grdGPDetail.FirstDisplayedScrollingRowIndex = grdGPDetail.RowCount - 1;
                    grdGPDetail.CurrentCell = grdGPDetail.Rows[kount - 1].Cells[0];
                }
                grdGPDetail.FirstDisplayedScrollingColumnIndex = 0;
                if (isnewClick)
                {
                    grdGPDetail.Columns["ReturnedQuantity"].Visible = false;
                    grdGPDetail.Columns["ReturningQuantity"].Visible = false;
                    grdGPDetail.Columns["Item"].Width = 500;
                }
                else
                {
                    grdGPDetail.Columns["ReturnedQuantity"].Visible = true;
                    grdGPDetail.Columns["ReturningQuantity"].Visible = true;
                    grdGPDetail.Columns["Item"].Width = 330;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddGPDetailRow() : Error");
            }

            return status;
        }

        private Boolean verifyAndReworkGPDetailGridRows()
        {
            Boolean status = true;

            try
            {

                if (grdGPDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in GP details");
                    return false;
                }
                for (int i = 0; i < grdGPDetail.Rows.Count; i++)
                {

                    grdGPDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if ((grdGPDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Length == 0) ||
                        (grdGPDetail.Rows[i].Cells["ModelNo"].Value.ToString().Trim().Length == 0) ||
                        (grdGPDetail.Rows[i].Cells["ModelName"].Value.ToString().Trim().Length == 0) ||
                        (grdGPDetail.Rows[i].Cells["Quantity"].Value == null) ||
                        (grdGPDetail.Rows[i].Cells["Quantity"].Value.ToString().Length == 0) ||
                        Convert.ToDouble(grdGPDetail.Rows[i].Cells["Quantity"].Value) == 0)
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }

                    if (!IsViewMode)
                    {
                        GTNDB gtndb = new GTNDB();
                        double AvailQuant = Convert.ToDouble(grdGPDetail.Rows[i].Cells["AvailableQuantity"].Value.ToString());
                        double Quant = Convert.ToDouble(grdGPDetail.Rows[i].Cells["Quantity"].Value);
                        if (Quant > AvailQuant)
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
                for (int i = 0; i < grdGPDetail.Rows.Count - 1; i++)
                {
                    for (int j = i + 1; j < grdGPDetail.Rows.Count; j++)
                    {

                        if (grdGPDetail.Rows[i].Cells[1].Value.ToString() == grdGPDetail.Rows[j].Cells["Item"].Value.ToString() &&
                            grdGPDetail.Rows[i].Cells["ModelNo"].Value.ToString() == grdGPDetail.Rows[j].Cells["ModelNo"].Value.ToString())
                        {
                            //duplicate item code
                            MessageBox.Show("Item code duplicated in GTN details... please ensure correctness (" +
                                grdGPDetail.Rows[i].Cells["Item"].Value.ToString() + ")");
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
        private List<gatepassdetail> getGPDetailList(gatepassheader gtnh)
        {
            List<gatepassdetail> GPDetails = new List<gatepassdetail>();
            gatepassdetail gtnd = new gatepassdetail();
            for (int i = 0; i < grdGPDetail.Rows.Count; i++)
            {
                try
                {
                    gtnd = new gatepassdetail();
                    gtnd.DocumentID = gtnh.DocumentID;
                    gtnd.TemporaryNo = gtnh.TemporaryNo;
                    gtnd.TemporaryDate = gtnh.TemporaryDate;
                    gtnd.StockitemID = grdGPDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Substring(0, grdGPDetail.Rows[i].Cells["Item"].Value.ToString().Trim().IndexOf('-'));
                    gtnd.Quantity = Convert.ToDouble(grdGPDetail.Rows[i].Cells["Quantity"].Value);
                    gtnd.Value = Convert.ToDouble(grdGPDetail.Rows[i].Cells["Value"].Value);
                    gtnd.RowID = Convert.ToInt32(grdGPDetail.Rows[i].Cells["RowID"].Value);
                    gtnd.ReturnedQuantity = Convert.ToDouble(grdGPDetail.Rows[i].Cells["ReturnedQuantity"].Value);
                    gtnd.ReturningQuantity = Convert.ToDouble(grdGPDetail.Rows[i].Cells["ReturningQuantity"].Value);
                    gtnd.ModelNo = grdGPDetail.Rows[i].Cells["ModelNo"].Value.ToString();
                    gtnd.MRNNo = Convert.ToInt32(grdGPDetail.Rows[i].Cells["MRNNo"].Value);
                    gtnd.MRNDate = Convert.ToDateTime(grdGPDetail.Rows[i].Cells["MRNDate"].Value);
                    gtnd.BatchNo = grdGPDetail.Rows[i].Cells["BatchNo"].Value.ToString();
                    gtnd.SerialNo = grdGPDetail.Rows[i].Cells["SerialNo"].Value.ToString();
                    gtnd.ExpiryDate = Convert.ToDateTime(grdGPDetail.Rows[i].Cells["ExpiryDate"].Value);
                    gtnd.PurchaseQuantity = Convert.ToDouble(grdGPDetail.Rows[i].Cells["PurchaseQuantity"].Value);
                    gtnd.PurchasePrice = Convert.ToDouble(grdGPDetail.Rows[i].Cells["PurchasePrice"].Value);
                    gtnd.PurchaseTax = Convert.ToDouble(grdGPDetail.Rows[i].Cells["PurchaseTax"].Value);
                    gtnd.SupplierID = grdGPDetail.Rows[i].Cells["SupplierID"].Value.ToString().Trim();
                    gtnd.refNo = Convert.ToInt32(grdGPDetail.Rows[i].Cells["StockRefNo"].Value);
                    GPDetails.Add(gtnd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("getGPDetailList() : Error creating GP Details");
                }
            }
            return GPDetails;
        }

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredGTNHeader(listOption);
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
            listOption = 6;
            ListFilteredGTNHeader(listOption);
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {

                GatePassDB gpdb = new GatePassDB();
                gatepassheader gtnh = new gatepassheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    if (!verifyAndReworkGPDetailGridRows())
                    {
                        return;
                    }
                    gtnh.DocumentID = docID;
                    gtnh.GatePassDate = dtGetPassDate.Value;
                    gtnh.CustomerID = txtCustomerID.Text;
                    gtnh.FromOffice = ((Structures.ComboBoxItem)cmbFromOffice.SelectedItem).HiddenValue;
                    gtnh.ToOffice = ((Structures.ComboBoxItem)cmbTOOffice.SelectedItem).HiddenValue;
                    gtnh.Comments = docCmtrDB.DGVtoString(dgvComments).Replace("'", "''");
                    gtnh.ForwarderList = prevgtn.ForwarderList;
                    gtnh.Remarks = txtRemarks.Text.Trim().Replace("'", "''");
                    gtnh.SpecialNotes = txtSpecialNote.Text.Trim().Replace("'", "''");
                    //prevgtn.FromLocation = gtnh.FromLocation;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }

                if (!gpdb.validategetpassheader(gtnh))
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
                if (gpdb.validategetpassheader(gtnh))
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
                    List<gatepassdetail> GPDetails = getGPDetailList(gtnh);
                    if (btnText.Equals("Update"))
                    {
                        if (gpdb.updateInvGatePassHEaderAndDetail(gtnh, prevgtn, GPDetails))
                        {
                            MessageBox.Show("Gate pass details updated");
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
                        if (gpdb.insertInvGatePassHEaderAndDetail(gtnh, GPDetails))
                        {
                            MessageBox.Show("Gate pass details Added");
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
                            MessageBox.Show("Failed to Insert  Gate pass Header");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Validation failed");
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
            AddRowClick = true;
            AddGPDetailRow();

        }

        private void grdPOPIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdGPDetail.Columns[e.ColumnIndex].Name;
                ///grdGTNDetail.Rows[e.RowIndex].Selected = true;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdGPDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkGPDetailGridRows();
                    }
                    if (columnName.Equals("Select"))
                    {
                        if (cmbTOOffice.SelectedIndex != -1)
                            ShowStoreWiseStockListView();
                        else
                            MessageBox.Show("Select To Office");
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
                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("LoadDocument") || columnName.Equals("Print") ||
                    (columnName.Equals("View")) || columnName.Equals("Return") || columnName.Equals("Accept") || columnName.Equals("AcceptReturned"))
                {
                    IsEditMode = true;
                    IsViewMode = false;
                    AddRowClick = false;

                    clearData();
                    ///grdList.Rows[e.RowIndex].Selected = true;
                    setButtonVisibility(columnName);
                    if ((columnName.Equals("View")) || columnName.Equals("Return") || columnName.Equals("Accept") || columnName.Equals("AcceptReturned"))
                    {
                        IsViewMode = true;
                        tabControl1.TabPages["tabGTNDetail"].Enabled = true;
                    }
                    prevgtn = new gatepassheader();
                    prevgtn.ToOffice = grdList.Rows[e.RowIndex].Cells["ToOffice"].Value.ToString();
                    //prevgtn.MaterialValue = Convert.ToDecimal(grdList.Rows[e.RowIndex].Cells["MaterialValue"].Value.ToString());
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
                    GatePassDB gpdb = new GatePassDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevgtn.GatePassNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["GatePassNo"].Value.ToString());
                    prevgtn.GatePassDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["GatePassDate"].Value.ToString());

                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Document Temp No:" + prevgtn.TemporaryNo + "\n" +
                            "Document Temp Date:" + prevgtn.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "Gate Pass No:" + prevgtn.GatePassNo + "\n" +
                            "Gate Pass Date:" + prevgtn.GatePassDate.ToString("dd-MM-yyyy");
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevgtn.TemporaryNo + "-" + prevgtn.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_1(null, null);
                        return;
                    }
                    //--------
                    prevgtn.SpecialNotes = grdList.Rows[e.RowIndex].Cells["SpecialNotes"].Value.ToString();
                    prevgtn.Remarks = grdList.Rows[e.RowIndex].Cells["Remarks"].Value.ToString();
                    prevgtn.CustomerID = grdList.Rows[e.RowIndex].Cells["gCustomerID"].Value.ToString();
                    prevgtn.CustomerName = grdList.Rows[e.RowIndex].Cells["gCustomerName"].Value.ToString();
                    prevgtn.AcceptedUser = grdList.Rows[e.RowIndex].Cells["AcceptedUser"].Value.ToString();
                    prevgtn.AcceptanceStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["AcceptanceStatus"].Value);
                    prevgtn.AcceptanceDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["AcceptanceDate"].Value.ToString());
                    prevgtn.FromOffice = grdList.Rows[e.RowIndex].Cells["FromOffice"].Value.ToString();
                    //prevgtn.FromLocation = grdList.Rows[e.RowIndex].Cells["FromLocation"].Value.ToString();
                    prevgtn.FromOfficeName = grdList.Rows[e.RowIndex].Cells["FromOfficeName"].Value.ToString();
                    prevgtn.ToOfficeName = grdList.Rows[e.RowIndex].Cells["ToOfficeName"].Value.ToString();
                    prevgtn.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    prevgtn.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    prevgtn.ReturnStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["ReturnStatus"].Value.ToString());
                    prevgtn.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    prevgtn.CreateUser = grdList.Rows[e.RowIndex].Cells["CreateUser"].Value.ToString();
                    if (columnName.Equals("Accept"))
                    {
                        if (prevgtn.AcceptanceStatus != 0)
                        {
                            MessageBox.Show("Already Accepted.");
                            pnlAddEdit.Visible = false;
                            grdList.Visible = true;
                            btnExit.Visible = true;
                            return;
                        }
                    }
                    if (columnName.Equals("AcceptReturned"))
                    {
                        if (prevgtn.ReturnStatus == 0)
                        {
                            MessageBox.Show("Return request not found.");
                            pnlAddEdit.Visible = false;
                            grdList.Visible = true;
                            btnExit.Visible = true;
                            btnNew.Visible = true;
                            return;
                        }
                    }
                    if (columnName.Equals("Return"))
                    {
                        if (prevgtn.AcceptanceStatus == 0)
                        {
                            MessageBox.Show("Not accepted yet.");
                            pnlAddEdit.Visible = false;
                            btnAddLine.Enabled = false;
                            btnClearEntries.Enabled = false;
                            grdList.Visible = true;
                            btnExit.Visible = true;
                            return;
                        }
                    }
                    prevgtn.ForwardUser = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevgtn.ApproveUser = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    prevgtn.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    if (columnName.Equals("Print"))
                    {
                        //MessageBox.Show("Not accepted yet.");
                        //pnlAddEdit.Visible = false;
                        //btnAddLine.Enabled = false;
                        //btnClearEntries.Enabled = false;
                        PrintForms.PrintGatePass pgp = new PrintForms.PrintGatePass();
                        List<gatepassdetail> GPDetailPrint = GatePassDB.getGatePassdetail(prevgtn);
                        pgp.PrintGatePassDetail(prevgtn, GPDetailPrint);
                        grdList.Visible = true;
                        btnExit.Visible = true;
                        return;
                    }
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
                    txtCustomerID.Text = prevgtn.CustomerID.ToString();
                    txtCustomerName.Text = prevgtn.CustomerName.ToString();
                    txtGetPassNo.Text = prevgtn.GatePassNo.ToString();
                    txtSpecialNote.Text = prevgtn.SpecialNotes.ToString();
                    txtRemarks.Text = prevgtn.Remarks.ToString();
                    try
                    {
                        dtGetPassDate.Value = prevgtn.GatePassDate;
                    }
                    catch (Exception)
                    {
                        dtGetPassDate.Value = DateTime.Parse("1900-01-01");
                    }
                    cmbFromOffice.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbFromOffice, prevgtn.FromOffice);
                    cmbTOOffice.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbTOOffice, prevgtn.ToOffice);
                    List<gatepassdetail> GPDetail = GatePassDB.getGatePassdetail(prevgtn);
                    grdGPDetail.Rows.Clear();
                    int i = 0;
                    foreach (gatepassdetail gtnd in GPDetail)
                    {
                        if (!AddGPDetailRow())
                        {
                            MessageBox.Show("Error found in Gate pass details. Please correct before updating the details");
                        }
                        else
                        {
                            try
                            {
                                grdGPDetail.Rows[i].Cells["Item"].Value = gtnd.StockitemID + "-" + gtnd.StockItemName;
                            }
                            catch (Exception)
                            {
                                grdGPDetail.Rows[i].Cells["Item"].Value = null;
                            }
                            grdGPDetail.Rows[i].Cells["RowID"].Value = gtnd.RowID;
                            grdGPDetail.Rows[i].Cells["Quantity"].Value = gtnd.Quantity;
                            grdGPDetail.Rows[i].Cells["Value"].Value = gtnd.Value;
                            grdGPDetail.Rows[i].Cells["ReturnedQuantity"].Value = gtnd.ReturnedQuantity;
                            grdGPDetail.Rows[i].Cells["ReturningQuantity"].Value = gtnd.ReturningQuantity;
                            grdGPDetail.Rows[i].Cells["ModelNo"].Value = gtnd.ModelNo;
                            grdGPDetail.Rows[i].Cells["ModelName"].Value = gtnd.ModelName;
                            grdGPDetail.Rows[i].Cells["MRNNo"].Value = gtnd.MRNNo;
                            grdGPDetail.Rows[i].Cells["MRNDate"].Value = gtnd.MRNDate;
                            grdGPDetail.Rows[i].Cells["PurchaseQuantity"].Value = gtnd.PurchaseQuantity;
                            grdGPDetail.Rows[i].Cells["PurchasePrice"].Value = gtnd.PurchasePrice;
                            grdGPDetail.Rows[i].Cells["PurchaseTax"].Value = gtnd.PurchaseTax;
                            grdGPDetail.Rows[i].Cells["BatchNo"].Value = gtnd.BatchNo;
                            grdGPDetail.Rows[i].Cells["SerialNo"].Value = gtnd.SerialNo;
                            grdGPDetail.Rows[i].Cells["ExpiryDate"].Value = gtnd.ExpiryDate;
                            grdGPDetail.Rows[i].Cells["SupplierID"].Value = gtnd.SupplierID;
                            grdGPDetail.Rows[i].Cells["SupplierName"].Value = gtnd.SupplierName;
                            grdGPDetail.Rows[i].Cells["StockRefNo"].Value = gtnd.refNo;
                            grdGPDetail.Rows[i].Cells["AvailableQuantity"].Value = StockDB.getAvailiableStockQuantity(gtnd.refNo);
                            i++;
                        }

                    }
                    if (columnName.Equals("Return") || columnName.Equals("Accept") || columnName.Equals("AcceptReturned"))
                    {
                        IsViewMode = true;
                        grdGPDetail.Columns["Quantity"].ReadOnly = true;
                        grdGPDetail.Columns["Value"].ReadOnly = true;
                        if (columnName.Equals("Return"))
                        {
                            grdGPDetail.Columns["ReturningQuantity"].ReadOnly = false;
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
                setButtonVisibility("btnEditPanel");
            }
        }

        private void btnClearEntries_Click_1(object sender, EventArgs e)
        {
            try
            {

                DialogResult dialog = MessageBox.Show("Are you sure to clear all entries in grid detail ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdGPDetail.Rows.Clear();
                    MessageBox.Show("Grid items cleared.");
                    verifyAndReworkGPDetailGridRows();
                }
            }
            catch (Exception)
            {
            }
        }
        private Boolean checkAvailabilityOfitem()
        {
            Boolean status = true;
            if (grdGPDetail.CurrentRow.Cells["Item"].Value.ToString().Length != 0 ||
                grdGPDetail.CurrentRow.Cells["ModelNo"].Value.ToString().Length != 0 ||
                grdGPDetail.CurrentRow.Cells["ModelName"].Value.ToString().Length != 0)
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
                    for (int i = 0; i < grdGPDetail.ColumnCount; i++)
                    {
                        if (grdGPDetail.CurrentRow.Cells[i].GetType() != typeof(Button))
                        {
                            grdGPDetail.CurrentRow.Cells[i].Value = "";
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

            string location = Main.MainStore;
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
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("select Only One Item");
                    return;
                }
                DataGridViewRow selectedRow = new DataGridViewRow();
                foreach (var row in checkedRows)
                {
                    selectedRow = row;
                }
                for (int i = 0; i < grdGPDetail.Rows.Count - 1; i++)
                {
                    if ((grdGPDetail.Rows[i].Cells["StockRefNo"].Value.ToString() == selectedRow.Cells["StockRefNo"].Value.ToString()))
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
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["LineNo"].Value = grdGPDetail.RowCount;
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["StockRefNo"].Value = grdRow.Cells["StockRefNo"].Value;
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["Item"].Value = grdRow.Cells["StockItemID"].Value + "-" + grdRow.Cells["StockItemName"].Value;
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["ModelNo"].Value = grdRow.Cells["ModelNo"].Value;
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["ModelName"].Value = grdRow.Cells["ModelName"].Value;
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["AvailableQuantity"].Value = grdRow.Cells["PresentStock"].Value;
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["Quantity"].Value = Convert.ToDouble(0);
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["MRNNo"].Value = grdRow.Cells["MRNNo"].Value;
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["MRNDate"].Value = grdRow.Cells["MRNDate"].Value;
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["BatchNo"].Value = grdRow.Cells["BAtchNo"].Value;
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["SerialNo"].Value = grdRow.Cells["SerialNO"].Value;
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["ExpiryDate"].Value = grdRow.Cells["ExpiryDate"].Value;
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["PurchaseQuantity"].Value = grdRow.Cells["PurchaseQuantity"].Value;
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["PurchasePrice"].Value = grdRow.Cells["PurchasePrice"].Value;
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["PurchaseTax"].Value = grdRow.Cells["PurchaseTax"].Value;
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["SupplierID"].Value = grdRow.Cells["SupplierId"].Value;
                grdGPDetail.Rows[grdGPDetail.RowCount - 1].Cells["SupplierName"].Value = grdRow.Cells["SupplierName"].Value;
            }
            catch (Exception ex)
            {
            }
            return status;
        }

        //-----
        //comment handling procedurs and functions
        //-----
        private void btnGetComments_Click(object sender, EventArgs e)
        {
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
                btnAccept.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                btnAcceptReturn.Visible = false;
                btnApprove.Visible = false;
                btnReturn.Visible = false;

                btnNew.Visible = false;
                btnExit.Visible = false;

                btnGetComments.Visible = false;
                txtComments.Visible = false;
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
                else if (btnName == "Accept")
                {
                    btnExit.Visible = false;
                    btnNew.Visible = false;
                    btnCancel.Visible = true;
                    btnAccept.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabGTNDetail;
                }
                else if (btnName == "AcceptReturned")
                {
                    btnExit.Visible = false;
                    btnNew.Visible = false;
                    btnCancel.Visible = true;
                    btnAcceptReturn.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabGTNDetail;
                }
                else if (btnName == "Return")
                {
                    btnExit.Visible = false;
                    btnNew.Visible = false;
                    btnCancel.Visible = true;
                    btnReturn.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabGTNDetail;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnApprove.Visible = true;
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
                switch (listOption)
                {
                    case 1:
                        btnActionPending.BackColor = Color.MediumAquamarine;
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
            if (Main.itemPriv[1] && office == "NVP")
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
        void handleGrdPrintButton()
        {
            grdList.Columns["Print"].Visible = false;
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                if (listOption == 6)
                {
                    grdList.Columns["Print"].Visible = true;
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
                grdGPDetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }
        private void cmbFromLocation_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (!verifyAndReworkGPDetailGridRows())
                {
                    return;
                }
                GatePassDB gdb = new GatePassDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Accept the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {

                    if (gdb.AcceptGatePass(prevgtn))
                    {
                        MessageBox.Show("Gate Pass Accepted");
                        closeAllPanels();
                        if (office != "NVP")
                        {
                            listOption = 4;
                        }
                        else
                        {
                            listOption = 1;
                        }
                        ListFilteredGTNHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private Boolean updateDashBoard(gatepassheader gtnh, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = gtnh.DocumentID;
                dsb.TemporaryNo = gtnh.TemporaryNo;
                dsb.TemporaryDate = gtnh.TemporaryDate;
                dsb.DocumentNo = gtnh.GatePassNo;
                dsb.DocumentDate = gtnh.GatePassDate;
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
        private void grdGTNDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    grdGPDetail.Rows[e.RowIndex].Selected = true;
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

        private void btnSelCustomer_Click(object sender, EventArgs e)
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

                frmPopup.Size = new Size(800, 400);

                chkBoxCustomer = new CheckedListBox();
                chkBoxCustomer.BackColor = System.Drawing.SystemColors.InactiveCaption;
                chkBoxCustomer.ColumnWidth = 80;
                chkBoxCustomer.FormattingEnabled = true;
                chkBoxCustomer.Items.AddRange(new object[] { "Customer", "Supplier", "Contractor", "Transporter", "Others" });
                chkBoxCustomer.Location = new System.Drawing.Point(69, 22);
                chkBoxCustomer.MultiColumn = true;
                chkBoxCustomer.SetItemChecked(0, true);
                chkBoxCustomer.Name = "chkBoxCustomer";
                chkBoxCustomer.Size = new System.Drawing.Size(446, 19);
                chkBoxCustomer.Location = new Point(10, 10);
                chkBoxCustomer.CheckOnClick = true;
                chkBoxCustomer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.chkBoxCustomer_MouseUp);
                frmPopup.Controls.Add(chkBoxCustomer);

                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(340, 38);
                lblSearch.Text = "Search by Name";
                lblSearch.AutoSize = true;
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearchCust = new TextBox();
                txtSearchCust.Size = new Size(220, 18);
                txtSearchCust.Location = new System.Drawing.Point(460, 36);
                txtSearchCust.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearchCust.ForeColor = Color.Black;
                txtSearchCust.TextChanged += new System.EventHandler(this.txtCustSearch_TextChangedInEmpGridList);
                txtSearchCust.TabIndex = 0;
                txtSearchCust.Focus();
                frmPopup.Controls.Add(txtSearchCust);

                CustomerDB custDB = new CustomerDB();
                grdCustList = custDB.getGridViewForCustomerListNew("Supplier");

                grdCustList.Bounds = new Rectangle(new Point(0, 60), new Size(800, 300));
                frmPopup.Controls.Add(grdCustList);

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 370);
                lvOK.Click += new System.EventHandler(this.grdCustOK_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 370);
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
                    MessageBox.Show("Select one Customer");
                    return;
                }
                string trlist;
                trlist = "";
                foreach (var row in checkedRows)
                {
                    customer cust = new customer();
                    txtCustomerID.Text = row.Cells["ID"].Value.ToString();
                    txtCustomerName.Text = row.Cells["Name"].Value.ToString();
                }

                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
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
            filterCustGridData();
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
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
                if (txtSearchCust.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdCustList.Rows)
                    {
                        if (!row.Cells["Name"].Value.ToString().Trim().ToLower().Contains(txtSearchCust.Text.Trim().ToLower()))
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
        private void chkBoxCustomer_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                string checkedStr = "";
                txtSearchCust.Text = "";
                int n = 1;
                foreach (var item in chkBoxCustomer.CheckedItems)
                {
                    if (chkBoxCustomer.CheckedItems.Count == n)
                        checkedStr = checkedStr + item.ToString();
                    else
                        checkedStr = checkedStr + item.ToString() + Main.delimiter1;
                    n++;
                }
                frmPopup.Controls.Remove(grdCustList);
                CustomerDB custDB = new CustomerDB();
                grdCustList = custDB.getGridViewForCustomerListNew(checkedStr);

                grdCustList.Bounds = new Rectangle(new Point(0, 60), new Size(800, 300));
                frmPopup.Controls.Add(grdCustList);
            }
            catch (Exception ex)
            {
            }
        }
        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                GatePassDB gdb = new GatePassDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevgtn.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevgtn.CommentStatus);
                    prevgtn.ForwarderList = getForwaradersList();
                    if (prevgtn.Status != 96)
                    {
                        prevgtn.GatePassNo = DocumentNumberDB.getNewNumber(docID, 2);
                    }

                    if (!verifyAndReworkGPDetailGridRows())
                    {
                        return;
                    }
                    List<gatepassdetail> Details = getGPDetailList(prevgtn);
                    if (gdb.ApproveGatePass(prevgtn, Details))
                    {
                        MessageBox.Show("Gate Pass Accepted with updated in stock");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredGTNHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private string getForwaradersList()
        {
            string list = "";
            try
            {
                string officeID = ((Structures.ComboBoxItem)cmbTOOffice.SelectedItem).HiddenValue;
                List<user> userList = EmployeePostingDB.getOfficeWiseEmployeeList(officeID);
                foreach (user u in userList)
                {
                    list = list + u.userEmpName + Main.delimiter1 +
                               u.userID + Main.delimiter1 + Main.delimiter2;
                }
                //string ForwarderList = previgh.ForwarderList + approverUName + Main.delimiter1 +
                //               approverUID + Main.delimiter1 + Main.delimiter2;
            }
            catch (Exception ex)
            {

            }
            return list;
        }
        private void cmbTOOffice_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {

                if (!verifyAndReworkGPDetailGridRows())
                {
                    return;
                }
                double returnSum = 0;
                foreach (DataGridViewRow row in grdGPDetail.Rows)
                {
                    double returning = Convert.ToDouble(row.Cells["ReturningQuantity"].Value);
                    double returned = Convert.ToDouble(row.Cells["ReturnedQuantity"].Value);
                    double Quantity = Convert.ToDouble(row.Cells["Quantity"].Value);
                    if ((returning + returned) > Quantity)
                    {
                        MessageBox.Show("Returning quantity in row " + (row.Index + 1) + " is not correct.");
                        return;
                    }
                    returnSum = returnSum + returning;
                }
                if (returnSum == 0)
                {
                    MessageBox.Show("Fill Quantity to be return in Detail.");
                    return;
                }
                List<gatepassdetail> GPDetails = getGPDetailList(prevgtn);

                GatePassDB gdb = new GatePassDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Return Quantity ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {

                    if (gdb.ReturnGatePass(prevgtn, GPDetails))
                    {
                        MessageBox.Show("Quantity Returned");
                        closeAllPanels();
                        if (office != "NVP")
                        {
                            listOption = 4;
                        }
                        else
                        {
                            listOption = 1;
                        }
                        ListFilteredGTNHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnAcceptReturn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!verifyAndReworkGPDetailGridRows())
                {
                    return;
                }
                GatePassDB gdb = new GatePassDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Accept Return Quantity ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    List<gatepassdetail> GPDetails = getGPDetailList(prevgtn);
                    if (gdb.AcceptReturningQnatGatePass(prevgtn, GPDetails))
                    {
                        MessageBox.Show("Quantity Returning accepted with updated in stock");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredGTNHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}

