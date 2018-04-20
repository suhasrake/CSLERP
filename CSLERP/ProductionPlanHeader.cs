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
    public partial class ProductionPlanHeader : System.Windows.Forms.Form
    {
        //Boolean track = false;
        //int no = 0;
        string IODocID = "";
        string docID = "PRODUCTIONPLAN";
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Boolean userIsACommenter = false;
        double Temp;
        string InternalOrderDocumentID = "IOPRODUCT";
        string FloorManagerUserID = "";
        string TeamMemberUserIDs = "";
        Form dtpForm = new Form();
        // Hashtable ht = new Hashtable();
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        productionplanheader prevprodution = new productionplanheader();
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
        Boolean AddRowClick = false;
        //string custid = "";
        //DateTimePicker dtp;
        public ProductionPlanHeader()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }
        private void ProductionPlanHeader_Load(object sender, EventArgs e)
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
            grdOngoingPlns.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdOngoingPlns.EnableHeadersVisualStyles = false;
            ListFilteredProductionPlanHeader(listOption);
        }
        private void ListFilteredProductionPlanHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                ProductionPlanHeaderDB pphdb = new ProductionPlanHeaderDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);

                List<productionplanheader> ProductionPlanHeaderList = pphdb.getFilteredProductionPlanHeader(userString, option, userCommentStatusString);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (productionplanheader pph in ProductionPlanHeaderList)
                {
                    if (option == 1)
                    {
                        if (pph.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = pph.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = pph.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = pph.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = pph.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProductionPlanNo"].Value = pph.ProductionPlanNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProductionPlanDate"].Value = pph.ProductionPlanDate;
                    //Newly added
                    grdList.Rows[grdList.RowCount - 1].Cells["InternalOrderNos"].Value = pph.InternalOrderNos;
                    grdList.Rows[grdList.RowCount - 1].Cells["InternalOrderDates"].Value = pph.InternalOrderDates;
                    grdList.Rows[grdList.RowCount - 1].Cells["Customers"].Value = ProductionPlanHeaderDB.getCustomerListFromReference(pph.Reference).Trim();

                    grdList.Rows[grdList.RowCount - 1].Cells["gReference"].Value = pph.Reference;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStockItemID"].Value = pph.StockItemID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStockItemName"].Value = pph.StockItemName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ModelNo"].Value = pph.ModelNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["ModelName"].Value = pph.ModelName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gQuantity"].Value = pph.Quantity;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPlannedStartTime"].Value = pph.PlannedStartTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPlannedEndTime"].Value = pph.PlannedEndTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gActualStartTime"].Value = pph.ActualStartTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gActualEndTime"].Value = pph.ActualEndTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gFloorManager"].Value = pph.FloorManager;
                    grdList.Rows[grdList.RowCount - 1].Cells["gRemarks"].Value = pph.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = pph.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = pph.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProductionStatus"].Value = pph.ProductionStatus;
                    //grdList.Rows[grdList.RowCount - 1].Cells["ProductionStatusString"].Value = getProdStatString(pph.ProductionStatus.ToString());
                    grdList.Rows[grdList.RowCount - 1].Cells["ProductionStatusString"].Value = pph.ProductionStatusString;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = pph.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["CmtStatus"].Value = pph.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = pph.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwardUser"].Value = pph.ForwardUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ApproveUser"].Value = pph.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = pph.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = pph.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = pph.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = pph.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = pph.ForwarderList;

                }

                setButtonVisibility("init");
                pnlList.Visible = true;

                if (listOption == 1 || listOption == 2)
                {
                    cmbProdStatFilter.Visible = false;
                }
                else
                {
                    cmbProdStatFilter.Visible = true;
                    cmbProdStatFilter.SelectedIndex = -1;
                    cmbProdStatFilter.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbProdStatFilter, "All");
                }

                //grdList.Columns["Creator"].Visible = true;
                //grdList.Columns["Forwarder"].Visible = true;
                //grdList.Columns["Approver"].Visible = true;
                if (listOption == 1 || listOption == 2)
                {
                    grdList.Columns["gTemporaryNo"].Visible = true;
                    grdList.Columns["gTemporaryDate"].Visible = true;
                    grdList.Columns["ProductionStatusString"].Visible = false;
                }
                else
                {
                    grdList.Columns["gTemporaryNo"].Visible = false;
                    grdList.Columns["gTemporaryDate"].Visible = false;
                    grdList.Columns["ProductionStatusString"].Visible = true;
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Production Plan Listing");
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
            cmbProdStatFilter.Visible = false;
            StatusCatalogueDB.fillStatusCatalogueCombo(cmbProdStatFilter, "PRODUCTIONPLAN");
            cmbProdStatFilter.Items.Add(new Structures.ComboBoxItem("All", "All"));
            cmbProdStatFilter.Visible = false;
            //statuscata
            docID = Main.currentDocument;
            dtTempDate.Format = DateTimePickerFormat.Custom;
            dtTempDate.CustomFormat = "dd-MM-yyyy";
            dtTempDate.Enabled = false;
            //dtInternalOrderDate.Format = DateTimePickerFormat.Custom;
            //dtInternalOrderDate.CustomFormat = "dd-MM-yyyy";
            //dtInternalOrderDate.Enabled = false;


            dtPlannedStartTime.Format = DateTimePickerFormat.Custom;
            dtPlannedStartTime.CustomFormat = "dd-MM-yyyy HH:mm:ss";

            dtPlannnedEndTime.Format = DateTimePickerFormat.Custom;
            dtPlannnedEndTime.CustomFormat = "dd-MM-yyyy HH:mm:ss";

            dtActualStartTime.Format = DateTimePickerFormat.Custom;
            dtActualStartTime.CustomFormat = "dd-MM-yyyy HH:mm:ss";

            dtActualEndTime.Format = DateTimePickerFormat.Custom;
            dtActualEndTime.CustomFormat = "dd-MM-yyyy HH:mm:ss";

            dtProductionPlanDate.Format = DateTimePickerFormat.Custom;
            dtProductionPlanDate.CustomFormat = "dd-MM-yyyy";
            dtProductionPlanDate.Enabled = false;


            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();

            grdProductionPlanDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //---
            //create tax details table for tax breakup display
            {

            }
            txtComments.Text = "";

            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
            setTabIndex();
        }
        private void setTabIndex()
        {
            txtTemporarryNo.TabIndex = 0;
            dtTempDate.TabIndex = 1;
            txtProductionPlanNo.TabIndex = 2;
            dtProductionPlanDate.TabIndex = 3;
            txtInternalOrderNos.TabIndex = 4;
            btnSelectInternalOrderNo.TabIndex = 5;
            txtInternalOrderDates.TabIndex = 6;
            //dtInternalOrderDate.TabIndex = 6;
            txtReference.TabIndex = 7;
            txtStockItem.TabIndex = 8;
            btnSelectStockItem.TabIndex = 9;
            txtModelNo.TabIndex = 10;
            txtModelName.TabIndex = 11;
            txtQuantity.TabIndex = 12;
            txtfloormanager.TabIndex = 13;
            btnfloormanager.TabIndex = 14;
            txtRemarks.TabIndex = 15;
            dtPlannedStartTime.TabIndex = 16;
            dtPlannnedEndTime.TabIndex = 17;
            dtActualStartTime.TabIndex = 18;
            dtActualEndTime.TabIndex = 19;

            btnForward.TabIndex = 0;
            btnApprove.TabIndex = 1;
            btnCancel.TabIndex = 2;
            btnSave.TabIndex = 3;
            btnReverse.TabIndex = 4;
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

                grdProductionPlanDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();


                //grdProductionPlanDetail.ReadOnly = false;
                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                pnllv.Visible = false;
                txtTemporarryNo.Text = "";
                dtTempDate.Value = DateTime.Parse("1900-01-01");
                txtProductionPlanNo.Text = "";
                dtProductionPlanDate.Value = DateTime.Parse("1900-01-01");
                txtInternalOrderNos.Text = "";
                txtInternalOrderDates.Text = "";
                txtReference.Text = "";
                dtPlannedStartTime.Value = DateTime.Today;
                txtStockItem.Text = "";
                txtModelName.Text = "";
                txtModelNo.Text = "";
                dtPlannnedEndTime.Value = DateTime.Today;
                dtActualStartTime.Value = DateTime.Parse("1900-01-01");

                dtActualEndTime.Value = DateTime.Parse("1900-01-01");
                txtQuantity.Text = "";
                txtReference.Text = "";
                txtfloormanager.Text = "";
                txtRemarks.Text = "";
                commentStatus = "";
                grdProductionPlanDetail.Columns["gProcessStatus"].Visible = false;
                grdProductionPlanDetail.Columns["Remarks"].Visible = false;
                grdProductionPlanDetail.Columns["ActualStartTime"].Visible = false;
                grdProductionPlanDetail.Columns["ActualEndTime"].Visible = false;
                lblCust.Visible = false;
                txtCustomers.Visible = false;
                txtCustomers.Text = "";
                AddRowClick = false;
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
                //track = true;
                clearData();
                //yy = 1;
                btnSave.Text = "Save";
                pnlAddEdit.Visible = true;
                closeAllPanels();
                pnlAddEdit.Visible = true;
                //txtReference.Enabled = true;
                setButtonVisibility("btnNew");
                AddRowClick = false;
            }
            catch (Exception)
            {

            }
        }


        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                AddProductionPlanDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddProductionPlanDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdProductionPlanDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkProductionPlanDetailGridRows())
                    {
                        return false;
                    }
                }
                grdProductionPlanDetail.Rows.Add();
                int kount = grdProductionPlanDetail.RowCount;
                grdProductionPlanDetail.Rows[kount - 1].Cells["LineNo"].Value = kount;

                grdProductionPlanDetail.Rows[grdProductionPlanDetail.RowCount - 1].Cells["gProcessID"].Value = "";
                grdProductionPlanDetail.Rows[grdProductionPlanDetail.RowCount - 1].Cells["gProcessDescription"].Value = "";
                grdProductionPlanDetail.Rows[grdProductionPlanDetail.RowCount - 1].Cells["gTeamMembers"].Value = "";
                grdProductionPlanDetail.Rows[grdProductionPlanDetail.RowCount - 1].Cells["gStartTime"].Value = DateTime.Today;
                grdProductionPlanDetail.Rows[grdProductionPlanDetail.RowCount - 1].Cells["gEndTime"].Value = DateTime.Today;
                grdProductionPlanDetail.Rows[grdProductionPlanDetail.RowCount - 1].Cells["ActualStartTime"].Value = DateTime.Today;
                grdProductionPlanDetail.Rows[grdProductionPlanDetail.RowCount - 1].Cells["ActualEndTime"].Value = DateTime.Today;
                grdProductionPlanDetail.Rows[grdProductionPlanDetail.RowCount - 1].Cells["gTeamMemberIDs"].Value = "";
                var BtnCell = (DataGridViewButtonCell)grdProductionPlanDetail.Rows[kount - 1].Cells["Delete"];
                BtnCell.Value = "Del";
                if (AddRowClick)
                    grdProductionPlanDetail.FirstDisplayedScrollingRowIndex = grdProductionPlanDetail.RowCount - 1;
            }

            catch (Exception ex)
            {
                MessageBox.Show("AddProductionDetailRow() : Error");
            }

            return status;
        }

        private Boolean verifyAndReworkProductionPlanDetailGridRows()
        {
            Boolean status = true;

            try
            {
                if (grdProductionPlanDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Production details");
                    return false;
                }
                for (int i = 0; i < grdProductionPlanDetail.Rows.Count; i++)
                {

                    grdProductionPlanDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if ((grdProductionPlanDetail.Rows[i].Cells["gProcessID"].Value == null) ||
                        (grdProductionPlanDetail.Rows[i].Cells["gProcessID"].Value.ToString().Length == 0) ||
                        (grdProductionPlanDetail.Rows[i].Cells["gStartTime"].Value == null) ||
                        (grdProductionPlanDetail.Rows[i].Cells["gEndTime"].Value == null) ||
                         (grdProductionPlanDetail.Rows[i].Cells["ActualStartTime"].Value == null) ||
                        (grdProductionPlanDetail.Rows[i].Cells["ActualEndTime"].Value == null) ||
                         (Convert.ToDateTime(grdProductionPlanDetail.Rows[i].Cells["gStartTime"].Value) >
                            Convert.ToDateTime(grdProductionPlanDetail.Rows[i].Cells["gEndTime"].Value)) ||
                        (Convert.ToDateTime(grdProductionPlanDetail.Rows[i].Cells["ActualStartTime"].Value) >
                            Convert.ToDateTime(grdProductionPlanDetail.Rows[i].Cells["ActualEndTime"].Value) &&
                            grdProductionPlanDetail.Rows[i].Cells["gProcessStatus"].Value.ToString().Equals("Closed")))
                    {
                        MessageBox.Show("Check values in plan detail Row No:" + (i + 1));
                        return false;
                    }

                }
                if (!validateItems())
                {
                    //MessageBox.Show("");
                    return false;
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
                for (int i = 0; i < grdProductionPlanDetail.Rows.Count - 1; i++)
                {
                    for (int j = i + 1; j < grdProductionPlanDetail.Rows.Count; j++)
                    {

                        if (grdProductionPlanDetail.Rows[i].Cells["gProcessID"].Value.ToString() == grdProductionPlanDetail.Rows[j].Cells["gProcessID"].Value.ToString())
                        {
                            //duplicate item code
                            MessageBox.Show("Error: Process duplicated in ............... (" +
                                grdProductionPlanDetail.Rows[i].Cells["gProcessID"].Value.ToString() + ").");
                            status = false;
                            break;
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
        private void grdProductionPlanDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                string columnName = grdProductionPlanDetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdProductionPlanDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkProductionPlanDetailGridRows();
                    }

                    if (columnName.Equals("SelectPID"))
                    {
                        //removeControlsFromPnllvPanel();
                        //pnllv = new Panel();
                        //pnllv.BorderStyle = BorderStyle.FixedSingle;
                        //btnfloormanager.Enabled = false;
                        //pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
                        frmPopup = new Form();
                        frmPopup.StartPosition = FormStartPosition.CenterScreen;
                        frmPopup.BackColor = Color.CadetBlue;

                        frmPopup.MaximizeBox = false;
                        frmPopup.MinimizeBox = false;
                        frmPopup.ControlBox = false;
                        frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                        frmPopup.Size = new Size(450, 300);
                        lv = CatalogueValueDB.getCatalogValueListView("ProductionStage");
                        // this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked1);
                        lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
                        frmPopup.Controls.Add(lv);

                        Button lvOK = new Button();
                        lvOK.BackColor = Color.Tan;
                        lvOK.Text = "OK";
                        lvOK.Location = new Point(40, 265);
                        lvOK.Click += new System.EventHandler(this.lvOK_Click4);
                        frmPopup.Controls.Add(lvOK);

                        Button lvCancel = new Button();
                        lvCancel.BackColor = Color.Tan;
                        lvCancel.Text = "CANCEL";
                        lvCancel.Location = new Point(130, 265);
                        lvCancel.Click += new System.EventHandler(this.lvCancel_Click4);
                        frmPopup.Controls.Add(lvCancel);
                        frmPopup.ShowDialog();
                        //pnlAddEdit.Controls.Add(pnllv);
                        //pnllv.BringToFront();
                        //pnllv.Visible = true;

                    }
                    if (columnName.Equals("Select"))
                    {
                        //removeControlsFromPnllvPanel();
                        //pnllv = new Panel();
                        //pnllv.BorderStyle = BorderStyle.FixedSingle;
                        ////btnfloormanager.Enabled = false;
                        //pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
                        string DeptList = "Production" + Main.delimiter1 + "QC";
                        frmPopup = new Form();
                        frmPopup.StartPosition = FormStartPosition.CenterScreen;
                        frmPopup.BackColor = Color.CadetBlue;

                        frmPopup.MaximizeBox = false;
                        frmPopup.MinimizeBox = false;
                        frmPopup.ControlBox = false;
                        frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                        frmPopup.Size = new Size(450, 300);
                        lv = EmployeePostingDB.EmpListViewForQC(DeptList, "");
                        ////foreach (ListViewItem item in lv.Items)
                        ////{
                        ////    if (item.SubItems[3].Text == txtfloormanager.Text)
                        ////        lv.Items.Remove(item);
                        ////}
                        //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked1);
                        lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
                        frmPopup.Controls.Add(lv);

                        Button lvOK = new Button();
                        lvOK.BackColor = Color.Tan;
                        lvOK.Text = "OK";
                        lvOK.Location = new Point(40, 265);
                        lvOK.Click += new System.EventHandler(this.lvOK_Click3);
                        frmPopup.Controls.Add(lvOK);

                        Button lvCancel = new Button();
                        lvCancel.BackColor = Color.Tan;
                        lvCancel.Text = "CANCEL";
                        lvCancel.Location = new Point(130, 265);
                        lvCancel.Click += new System.EventHandler(this.lvCancel_Click3);
                        frmPopup.Controls.Add(lvCancel);
                        frmPopup.ShowDialog();
                        //pnlAddEdit.Controls.Add(pnllv);
                        //pnllv.BringToFront();
                        //pnllv.Visible = true;

                    }
                    if (columnName.Equals("dtButton1") || columnName.Equals("dtButton2"))
                    {
                        DateTime dt = DateTime.Today;
                        if (columnName.Equals("dtButton1"))
                        {
                            dt = Convert.ToDateTime(grdProductionPlanDetail.Rows[e.RowIndex].Cells["gStartTime"].Value);
                        }
                        else
                        {
                            dt = Convert.ToDateTime(grdProductionPlanDetail.Rows[e.RowIndex].Cells["gEndTime"].Value);
                        }
                        //showDtPicker(e.ColumnIndex,e.RowIndex);
                        Rectangle tempRect = grdProductionPlanDetail.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                        ////dt.Location = tempRect.Location;
                        ////showDtPickerForm(tempRect.Left,tempRect.Top,tempRect.Location);
                        showDtPickerForm(Cursor.Position.X, Cursor.Position.Y, tempRect.Location, dt);
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
        private void showDtPicker(int colindex, int rowindex)
        {
            DateTimePicker dt = new DateTimePicker();
            dt.ValueChanged += new EventHandler(cellDateTimePickerValueChanged);
            dt.Visible = false;
            grdProductionPlanDetail.Controls.Add(dt);
            {
                Rectangle tempRect = grdProductionPlanDetail.GetCellDisplayRectangle(colindex, rowindex, false);
                dt.Location = tempRect.Location;
                dt.Width = tempRect.Width;
                dt.Visible = true;
                dt.Show();
            }
            //Point point = dt.EditBase.ButtonRectangle.Location;
            //dt.ShowDropDown(point);
            //dt.GotFocus += new EventHandler(dateTimePicker1_GotFocus);
            //dt.Focus();

            ////dt.Select();
            ////SendKeys.Send("%{DOWN}");
        }
        void dateTimePicker1_GotFocus(object sender, EventArgs e)
        {
            System.Windows.Forms.SendKeys.Send("%{DOWN}");
        }
        void cellDateTimePickerValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            ////grdProductionPlanDetail.Rows[grdProductionPlanDetail.RowCount - 1].Cells["gProcessID"]
            grdProductionPlanDetail.Rows[grdProductionPlanDetail.CurrentCell.RowIndex].Cells[grdProductionPlanDetail.CurrentCell.ColumnIndex - 1].Value = dtp.Value;
            //dataGridView1.CurrentCell.Value = cellDateTimePicker.Value.ToString();//convert the date as per your format
            //cellDateTimePicker.Visible = false;
            ////dtp.Dispose();
            ////dtpForm.Dispose();

        }
        private void showDtPickerForm(int left, int top, Point location, DateTime dtvalue)
        {
            if (left > Screen.PrimaryScreen.Bounds.Width - 250)
            {
                left = Screen.PrimaryScreen.Bounds.Width - 250;
            }
            dtpForm = new Form();
            dtpForm.StartPosition = FormStartPosition.Manual;
            dtpForm.Size = new Size(200, 100);
            dtpForm.Location = new Point(left, top);
            //dtpForm.Location = location;
            ////dtpForm.StartPosition = FormStartPosition.CenterScreen;
            DateTimePicker dt = new DateTimePicker();
            dt.Format = DateTimePickerFormat.Custom;
            dt.CustomFormat = "dd-MM-yyyy HH:mm:ss";
            dt.ValueChanged += new EventHandler(cellDateTimePickerValueChanged);
            dt.Value = dtvalue;
            dtpForm.Controls.Add(dt);
            {
                ////dt.Location = new Point(10,10);
                dt.Width = 150;
                dt.Height = 100;
                dt.Visible = true;
                dt.ShowUpDown = true;
                ////dt.Show();
                System.Windows.Forms.SendKeys.Send("%{DOWN}");
            }
            dtpForm.ShowDialog();
            ////DateTimePicker dtp = new DateTimePicker();
            ////frm.Controls.Add(dtp);
            ////dtp.Width = 200;
            ////dtp.Height = 200;
            ////dtp.Visible = true;
            ////dtp.Show();

            //////frm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            ////DataGridView dgv = new DataGridView();
            ////List<employeeposting> EmployeePosting = new List<employeeposting>();
            //////EmployeePosting = EmployeePostingDB.getEmployeePosting(txtPostingEmpID.Text);
            ////dgv.DataSource = EmployeePosting;
            ////dgv.RowHeadersVisible = false;
            ////dgv.Size = new Size(850, 250);
            ////dgv.ReadOnly = true;
            ////dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            ////frm.Controls.Add(dgv);
            //////frm.Text = "Posting details of " + txtPostingEmpName.Text;
            ////frm.MaximizeBox = false;
            ////frm.MinimizeBox = false;
            ////try
            ////{
            ////    dgv.Columns[1].Name = "Name1";
            ////    string tmp = dgv.Columns[1].Name;
            ////    dgv.Columns["Status"].Visible = false;
            ////    //dgv.Columns.Remove(9);
            ////    //dgv.Columns.Remove(10);
            ////    //dgv.Columns.Remove("createUserName");
            ////}
            ////catch (Exception ex)
            ////{
            ////}

        }
        private List<productionplandetail> getProductionPlanDetails(productionplanheader pph)
        {
            List<productionplandetail> ProductionPlanDetailList = new List<productionplandetail>();
            try
            {
                productionplandetail ppd = new productionplandetail();
                DateTime dt = Convert.ToDateTime(grdProductionPlanDetail.Rows[0].Cells["gStartTime"].Value);
                DateTime dt1 = Convert.ToDateTime(grdProductionPlanDetail.Rows[0].Cells["gEndTime"].Value);

                for (int i = 0; i < grdProductionPlanDetail.Rows.Count; i++)
                {
                    ppd = new productionplandetail();
                    ppd.DocumentID = pph.DocumentID;
                    ppd.TemporaryNo = pph.TemporaryNo;
                    ppd.TemporaryDate = pph.TemporaryDate;
                    // ppd.ProcessID = grdProductionPlanDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Substring(0, grdProductionPlanDetail.Rows[i].Cells["Item"].Value.ToString().Trim().IndexOf('-'));
                    ppd.SlNo = Convert.ToInt32(grdProductionPlanDetail.Rows[i].Cells["LineNo"].Value.ToString());
                    ppd.ProcessID = grdProductionPlanDetail.Rows[i].Cells["gProcessID"].Value.ToString();
                    ppd.TeamMembers = grdProductionPlanDetail.Rows[i].Cells["gTeamMemberIDs"].Value.ToString();
                    ppd.StartTime = Convert.ToDateTime(grdProductionPlanDetail.Rows[i].Cells["gStartTime"].Value);
                    ppd.EndTime = Convert.ToDateTime(grdProductionPlanDetail.Rows[i].Cells["gEndTime"].Value);

                    if (dt > ppd.StartTime)
                    {
                        dt = ppd.StartTime;
                    }
                    if (dt1 < ppd.EndTime)
                    {
                        dt1 = ppd.EndTime;
                    }
                    ProductionPlanDetailList.Add(ppd);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("createAndUpdateProductionDetails() : Error updating Production Details");
                ProductionPlanDetailList = null;
            }
            return ProductionPlanDetailList;
        }



        private void btnActionPending_Click(object sender, EventArgs e)
        {
            //cmbProdStatFilter.Visible = false;
            listOption = 1;
            ListFilteredProductionPlanHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            //cmbProdStatFilter.Visible = false;
            listOption = 2;
            ListFilteredProductionPlanHeader(listOption);
        }

        private void btnApproved_Click(object sender, EventArgs e)
        {
            //cmbProdStatFilter.Visible = true;
            //cmbProdStatFilter.SelectedIndex = 0;
            if (getuserPrivilegeStatus() == 2)
            {
                listOption = 6; //viewer
            }
            else
            {
                listOption = 3;
            }

            ListFilteredProductionPlanHeader(listOption);
        }
        private Boolean UpdateProductionPlanTime()
        {
            Boolean status = true;
            try
            {
                ProductionPlanHeaderDB pphdb = new ProductionPlanHeaderDB();
                productionplandetail ppd = new productionplandetail();
                DateTime dt = Convert.ToDateTime(grdProductionPlanDetail.Rows[0].Cells["gStartTime"].Value);
                DateTime dt1 = Convert.ToDateTime(grdProductionPlanDetail.Rows[0].Cells["gEndTime"].Value);
                List<productionplandetail> ProductionPlanDetailList = new List<productionplandetail>();
                for (int i = 0; i < grdProductionPlanDetail.Rows.Count; i++)
                {
                    try
                    {
                        ppd = new productionplandetail();
                        ppd.StartTime = Convert.ToDateTime(grdProductionPlanDetail.Rows[i].Cells["gStartTime"].Value);
                        ppd.EndTime = Convert.ToDateTime(grdProductionPlanDetail.Rows[i].Cells["gEndTime"].Value);

                        if (dt > ppd.StartTime)
                        {
                            dt = ppd.StartTime;
                        }
                        if (dt1 < ppd.EndTime)
                        {
                            dt1 = ppd.EndTime;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("UpdateProductionPlanTime() : Error creating Production Details");
                        status = false;
                    }
                }
                // MessageBox.Show("value:" + dt);
                dtPlannedStartTime.Value = dt;
                dtPlannnedEndTime.Value = dt1;


            }

            catch (Exception ex)
            {
                MessageBox.Show("UpdateProductionPlanTime() : Error creating Production Details");
                status = false;
            }

            return status;
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {

                ProductionPlanHeaderDB pphdb = new ProductionPlanHeaderDB();
                productionplanheader pph = new productionplanheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    if (!verifyAndReworkProductionPlanDetailGridRows())
                    {
                        return;
                    }
                    if (!UpdateProductionPlanTime())
                    {
                        return;
                        //MessageBox.Show("failed");
                    }
                    pph.DocumentID = docID;
                    pph.ProductionPlanDate = dtProductionPlanDate.Value;

                    pph.InternalOrderNos = txtInternalOrderNos.Text.Trim();
                    pph.InternalOrderDates = txtInternalOrderDates.Text.Trim();
                    if (txtReference.Text.Length != 0)
                        pph.Reference = txtReference.Text;
                    else
                        pph.Reference = "";

                    pph.PlannedStartTime = dtPlannedStartTime.Value;
                    pph.PlannedEndTime = dtPlannnedEndTime.Value;

                    pph.ActualStartTime = dtActualStartTime.Value;
                    pph.ActualEndTime = dtActualEndTime.Value;

                    pph.FloorManager = FloorManagerUserID;
                    pph.Remarks = txtRemarks.Text;
                    pph.Quantity = Convert.ToDouble(txtQuantity.Text);
                    pph.StockItemID = txtStockItem.Text.Substring(0, txtStockItem.Text.IndexOf('-'));
                    pph.ModelNo = txtModelNo.Text;
                    pph.ModelName = txtModelName.Text;
                    pph.Comments = docCmtrDB.DGVtoString(dgvComments);
                    pph.ForwarderList = prevprodution.ForwarderList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed in Production paln Header.");
                    return;
                }
                //Replacing single quotes
                pph = verifyHeaderInputString(pph);
                verifyDetailInputString();

                if (!pphdb.validateProductionPlanHeader(pph))
                {
                    MessageBox.Show("Validation failed in Production paln Header.");
                    return;
                }
                if (!validateItems())
                {
                    MessageBox.Show("Production plan Detail Validation failed");
                    return;
                }
                //if (btnText.Equals("Update"))
                //{
                //    Temp = InternalOrderDB.getQuantityOfIO(pph.InternalOrderNo, pph.InternalOrderDate, pph.StockItemID);
                //}
                //if (Temp < Convert.ToDouble(txtQuantity.Text))
                //{
                //    MessageBox.Show("Quantity Not Available.\n Available Quant:" + Temp);
                //    return;
                //}


                if (btnText.Equals("Save"))
                {
                    //pph.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    pph.DocumentStatus = 1; //created
                    pph.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    pph.TemporaryNo = Convert.ToInt32(txtTemporarryNo.Text);
                    pph.TemporaryDate = prevprodution.TemporaryDate;
                    pph.DocumentStatus = prevprodution.DocumentStatus;
                    // inh.QCStatus = prevmrn.QCStatus;
                }


                if (pphdb.validateProductionPlanHeader(pph))
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
                            pph.CommentStatus = docCmtrDB.createCommentStatusString(prevprodution.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            pph.CommentStatus = prevprodution.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            pph.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            pph.CommentStatus = prevprodution.CommentStatus;
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
                        pph.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus);
                    }
                    List<productionplandetail> ProductionPlanDetailList = getProductionPlanDetails(pph);
                    if (btnText.Equals("Update"))
                    {
                        if (pphdb.updatePPlanHeaderAndDetail(pph, prevprodution, ProductionPlanDetailList))
                        {
                            MessageBox.Show("Production Header Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredProductionPlanHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update Product Header");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (pphdb.InsertPPlanHeaderAndDetail(pph, ProductionPlanDetailList))
                        {
                            MessageBox.Show("Production Header Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredProductionPlanHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert Production  Header Details");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Production Details Validation failed");
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
            AddRowClick = true;
            AddProductionPlanDetailRow();
        }
        private void grdPOPIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {


        }
        private void lvOK_Click3(object sender, EventArgs e)
        {
            try
            {

                //btnfloormanager.Enabled = true;
                int kount = 0;
                string s2 = "";
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        s2 = s2 + itemRow.SubItems[3].Text + Main.delimiter1;
                    }
                }
                TeamMemberUserIDs = s2;
                ERPUserDB erpuserdb = new ERPUserDB();
                grdProductionPlanDetail.CurrentRow.Cells["gTeamMembers"].Value = erpuserdb.getUserNames(s2);
                grdProductionPlanDetail.CurrentRow.Cells["gTeamMemberIDs"].Value = s2;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }


        private void lvCancel_Click3(object sender, EventArgs e)
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
        private void lvOK_Click4(object sender, EventArgs e)
        {
            try
            {


                //btnfloormanager.Enabled = true;
                int kount = 0;
                string s3 = "";
                string s4 = "";
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        kount++;
                    }
                }
                if (kount == 0)
                {
                    MessageBox.Show("Select one PROCESS ID");
                    return;
                }
                if (kount > 1)
                {
                    MessageBox.Show("Select only one PROCESS ID");
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        s3 = s3 + itemRow.SubItems[1].Text;
                        s4 = s4 + itemRow.SubItems[2].Text;
                    }
                }
                grdProductionPlanDetail.CurrentRow.Cells["gProcessID"].Value = s3;
                grdProductionPlanDetail.CurrentRow.Cells["gProcessDescription"].Value = s4;

                frmPopup.Close();
                frmPopup.Dispose();

            }
            catch (Exception ex)
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

        private void btnCalculateax_Click_1(object sender, EventArgs e)
        {
            verifyAndReworkProductionPlanDetailGridRows();
        }

        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
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
                    //track = true;
                    setButtonVisibility(columnName);
                    AddRowClick = false;
                    if (columnName.Equals("View"))
                    {
                        //grdProductionPlanDetai
                        tabControl1.TabPages["tabProductionPlanDetail"].Enabled = true;
                        grdProductionPlanDetail.Columns["gProcessStatus"].Visible = true;
                        grdProductionPlanDetail.Columns["Remarks"].Visible = true;
                        grdProductionPlanDetail.Columns["ActualStartTime"].Visible = true;
                        grdProductionPlanDetail.Columns["ActualEndTime"].Visible = true;
                        //grdProductionPlanDetail.Enabled = true;
                        //grdProductionPlanDetail.ReadOnly = true;
                    }
                    prevprodution = new productionplanheader();
                    lblCust.Visible = true;
                    txtCustomers.Visible = true;
                    prevprodution.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();
                    prevprodution.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    prevprodution.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    //previnvoice.Reference = grdList.Rows[e.RowIndex].Cells["Reference"].Value.ToString();
                    prevprodution.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    prevprodution.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    prevprodution.ProductionPlanNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gProductionPlanNo"].Value.ToString());
                    prevprodution.ProductionPlanDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gProductionPlanDate"].Value.ToString());
                    if (prevprodution.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    prevprodution.Comments = ProductionPlanHeaderDB.getUserComments(prevprodution.DocumentID, prevprodution.TemporaryNo, prevprodution.TemporaryDate);
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    //setPODetailColumns(docID);

                    ProductionPlanHeaderDB pphdb = new ProductionPlanHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevprodution.InternalOrderNos = grdList.Rows[e.RowIndex].Cells["InternalOrderNos"].Value.ToString();
                    prevprodution.InternalOrderDates = grdList.Rows[e.RowIndex].Cells["InternalOrderDates"].Value.ToString();
                    txtInternalOrderNos.Text = prevprodution.InternalOrderNos.ToString();
                    txtInternalOrderDates.Text = prevprodution.InternalOrderDates;

                    prevprodution.StockItemID = grdList.Rows[e.RowIndex].Cells["gStockItemID"].Value.ToString();
                    prevprodution.StockItemName = grdList.Rows[e.RowIndex].Cells["gStockItemName"].Value.ToString();
                    prevprodution.ModelNo = grdList.Rows[e.RowIndex].Cells["Modelno"].Value.ToString();
                    prevprodution.ModelName = grdList.Rows[e.RowIndex].Cells["ModelName"].Value.ToString();
                    prevprodution.Quantity = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gQuantity"].Value.ToString());
                    //ht.Add(prevprodution.StockItemID, prevprodution.Quantity);
                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Document Temp No:" + prevprodution.TemporaryNo + "\n" +
                            "Document Temp Date:" + prevprodution.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "Production Plan No:" + prevprodution.ProductionPlanNo + "\n" +
                            "Production Plan Date:" + prevprodution.ProductionPlanDate.ToString("dd-MM-yyyy");
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevprodution.TemporaryNo + "-" + prevprodution.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_1(null, null);
                        return;
                    }
                    //--------
                    prevprodution.Reference = grdList.Rows[e.RowIndex].Cells["gReference"].Value.ToString();
                    txtCustomers.Text = grdList.Rows[e.RowIndex].Cells["Customers"].Value.ToString();
                    prevprodution.PlannedStartTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gPlannedStartTime"].Value.ToString());
                    prevprodution.PlannedEndTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gPlannedEndTime"].Value.ToString());
                    prevprodution.ActualStartTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gActualStartTime"].Value.ToString());
                    prevprodution.ActualEndTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gActualEndTime"].Value.ToString());

                    FloorManagerUserID = grdList.Rows[e.RowIndex].Cells["gFloorManager"].Value.ToString();

                    ////prevprodution.FloorManager = grdList.Rows[e.RowIndex].Cells["gFloorManager"].Value.ToString();
                    ERPUserDB erpuserdb = new ERPUserDB();
                    prevprodution.FloorManager = erpuserdb.getUserDetail(FloorManagerUserID, 1);


                    prevprodution.Remarks = grdList.Rows[e.RowIndex].Cells["gRemarks"].Value.ToString();
                    //previnvoice.Comments = grdList.Rows[e.RowIndex].Cells["Comments"].Value.ToString();
                    prevprodution.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();
                    prevprodution.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    prevprodution.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    prevprodution.ProductionStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gProductionStatus"].Value.ToString());
                    prevprodution.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    prevprodution.ForwardUser = grdList.Rows[e.RowIndex].Cells["ForwardUser"].Value.ToString();
                    prevprodution.ApproveUser = grdList.Rows[e.RowIndex].Cells["ApproveUser"].Value.ToString();
                    prevprodution.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    prevprodution.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    prevprodution.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();

                    prevprodution.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevprodution.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    prevprodution.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    //--comments
                    chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    prevprodution.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(prevprodution.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(prevprodution.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---


                    txtReference.Text = prevprodution.Reference.ToString();
                    txtTemporarryNo.Text = prevprodution.TemporaryNo.ToString();
                    dtTempDate.Value = prevprodution.TemporaryDate;
                    //dtTempDate.Value = previnvoice.TemporaryDate;



                    //AddStockItems("IOPRODUCT");
                    txtfloormanager.Text = prevprodution.FloorManager;
                    txtProductionPlanNo.Text = prevprodution.ProductionPlanNo.ToString();
                    dtProductionPlanDate.Value = prevprodution.ProductionPlanDate;
                    txtReference.Text = prevprodution.Reference.ToString();
                    dtPlannedStartTime.Value = prevprodution.PlannedStartTime;
                    dtPlannnedEndTime.Value = prevprodution.PlannedEndTime;
                    dtActualStartTime.Value = prevprodution.ActualStartTime;

                    dtActualEndTime.Value = prevprodution.ActualEndTime;
                    txtStockItem.Text = prevprodution.StockItemID + "-" + prevprodution.StockItemName; ;
                    txtModelNo.Text = prevprodution.ModelNo;
                    txtModelName.Text = prevprodution.ModelName;
                    txtQuantity.Text = prevprodution.Quantity.ToString();
                    txtRemarks.Text = prevprodution.Remarks.ToString();
                    // .SelectedIndex = cmbStockItemID.FindString(prevprodution.StockItemID);
                    List<productionplandetail> productionplandetailList = ProductionPlanHeaderDB.getProductionPlanDetail(prevprodution);
                    grdProductionPlanDetail.Rows.Clear();
                    int i = 0;
                    try
                    {
                        foreach (productionplandetail ppd in productionplandetailList)
                        {
                            AddProductionPlanDetailRow();

                            grdProductionPlanDetail.Rows[i].Cells["gProcessID"].Value = ppd.ProcessID;
                            grdProductionPlanDetail.Rows[i].Cells["LineNo"].Value = ppd.SlNo;
                            grdProductionPlanDetail.Rows[i].Cells["gProcessDescription"].Value = ppd.ProcessDescription;
                            grdProductionPlanDetail.Rows[i].Cells["gTeamMemberIDs"].Value = ppd.TeamMembers;
                            grdProductionPlanDetail.Rows[i].Cells["gTeamMembers"].Value = ppd.TeamMembers;
                            grdProductionPlanDetail.Rows[i].Cells["gStartTime"].Value = ppd.StartTime;
                            grdProductionPlanDetail.Rows[i].Cells["gEndTime"].Value = ppd.EndTime;
                            grdProductionPlanDetail.Rows[i].Cells["ActualStartTime"].Value = ppd.ActualStartTime;
                            grdProductionPlanDetail.Rows[i].Cells["ActualEndTime"].Value = ppd.ActualEndTime;
                            grdProductionPlanDetail.Rows[i].Cells["gTeamMembers"].Value = erpuserdb.getUserNames(ppd.TeamMembers);
                            grdProductionPlanDetail.Rows[i].Cells["gProcessStatus"].Value = getProcessStatString(ppd.ProcessStatus);
                            grdProductionPlanDetail.Rows[i].Cells["Remarks"].Value = ppd.Remarks;
                            i++;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    if (!verifyAndReworkProductionPlanDetailGridRows())
                    {
                        MessageBox.Show("Error found in Production details. Please correct before updating the details");
                    }
                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                    tabControl1.SelectedTab = tabProductionPlanHeader;
                    tabControl1.Visible = true;
                }
                else
                {
                    return;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("error");
            }
        }
        private string getProcessStatString(int stat)
        {
            string str = "";
            try
            {
                if (stat == 1)
                    str = "Started";
                else if (stat == 99)
                    str = "Closed";
                else
                    str = "";
            }
            catch (Exception ex)
            {
            }
            return str;
        }
        private void btnClearEntries_Click_1(object sender, EventArgs e)
        {
            try
            {

                DialogResult dialog = MessageBox.Show("Are you sure to clear all entries in grid detail ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdProductionPlanDetail.Rows.Clear();
                    MessageBox.Show("Grid items cleared.");
                    verifyAndReworkProductionPlanDetailGridRows();
                }

            }
            catch (Exception)
            {
            }

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
                ProductionPlanHeaderDB pphdb = new ProductionPlanHeaderDB();
                if (prevprodution.PlannedStartTime < DateTime.Now)
                {
                    MessageBox.Show("Not allow to approve.\n(Planned Start Time is less than Todays Date)");
                    return;
                }
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    //if (previnvoice.QCStatus == 99)
                    //{
                    prevprodution.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevprodution.CommentStatus);
                    prevprodution.ProductionPlanNo = DocumentNumberDB.getNewNumber(docID, 2);
                    prevprodution.ProductionStatus = 1;//for Production Status

                    if (pphdb.ApproveProductionPlanHeader(prevprodution))
                    {

                        MessageBox.Show("Production Document Approved");
                        if (!updateDashBoard(prevprodution, 2))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                        }
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredProductionPlanHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse

                    }
                    else
                        MessageBox.Show("Unable to approve");
                    //}
                    //else
                    //    MessageBox.Show("Approval fails , MRN is not approved by QC");
                }
            }
            catch (Exception)
            {
            }

        }

        //private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    // if (tabControl1.SelectedTab == tabInvoiceInDetail)
        //    //  validateItems();
        //}
        //private void pnlList_Paint(object sender, PaintEventArgs e)
        //{

        //}
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
                //frmPopup.Dispose();
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
                            ProductionPlanHeaderDB pphdb = new ProductionPlanHeaderDB();
                            // prevprodution.ProductionStatus = 1;
                            prevprodution.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevprodution.CommentStatus);
                            prevprodution.ForwardUser = approverUID;
                            prevprodution.ForwarderList = prevprodution.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (pphdb.forwardProductionPlanHeader(prevprodution))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(prevprodution, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredProductionPlanHeader(listOption);
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
        private Boolean updateDashBoard(productionplanheader pph, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = pph.DocumentID;
                dsb.TemporaryNo = pph.TemporaryNo;
                dsb.TemporaryDate = pph.TemporaryDate;
                dsb.DocumentNo = pph.ProductionPlanNo;
                dsb.DocumentDate = pph.ProductionPlanDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = pph.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(prevprodution.DocumentID);
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
                tp.Enabled = false;
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
                    string s = prevprodution.ForwarderList;
                    string reverseStr = getReverseString(prevprodution.ForwarderList);
                    //do forward activities
                    prevprodution.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevprodution.CommentStatus);
                    ProductionPlanHeaderDB pphdb = new ProductionPlanHeaderDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevprodution.ForwarderList = reverseStr.Substring(0, ind);
                        prevprodution.ForwardUser = reverseStr.Substring(ind + 3);
                        prevprodution.DocumentStatus = prevprodution.DocumentStatus - 1;
                    }
                    else
                    {
                        prevprodution.ForwarderList = "";
                        prevprodution.ForwardUser = "";
                        prevprodution.DocumentStatus = 1;
                        //prevprodution.ProductionStatus = 1;
                    }
                    if (pphdb.reverseProductionPlanHeader(prevprodution))
                    {
                        MessageBox.Show("Production Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredProductionPlanHeader(listOption);
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
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevprodution.TemporaryNo + "-" + prevprodution.TemporaryDate.ToString("yyyyMMddhhmmss"));
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
                    string subDir = prevprodution.TemporaryNo + "-" + prevprodution.TemporaryDate.ToString("yyyyMMddhhmmss");
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
                // btnQC.Visible = false;
                // btnQCCompleted.Visible = false;
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
                    tabControl1.SelectedTab = tabProductionPlanHeader;
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
                    tabControl1.SelectedTab = tabProductionPlanHeader;
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

                    tabControl1.SelectedTab = tabProductionPlanHeader;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabProductionPlanHeader;
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
                grdProductionPlanDetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSelectPO_Click(object sender, EventArgs e)
        {
            //removeControlsFromPnllvPanel();
            //pnllv = new Panel();
            ////btnSelectInternalOrderNo.Enabled = false;
            //pnllv.BorderStyle = BorderStyle.FixedSingle;
            if (txtInternalOrderNos.Text.Length != 0)
            {
                DialogResult dialog = MessageBox.Show("Internal Order No/Date and Product Details will be removed ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    txtInternalOrderNos.Text = "";
                    //dtInternalOrderDate.Value = DateTime.Now;
                    txtInternalOrderDates.Text = "";
                    txtStockItem.Text = "";
                    txtModelNo.Text = "";
                    txtModelName.Text = "";
                    txtQuantity.Text = "";
                    txtReference.Text = "";
                    txtfloormanager.Text = "";
                    grdProductionPlanDetail.Rows.Clear();
                }
                else
                {
                    //btnSelectInternalOrderNo.Enabled = true;
                    return;
                }
            }
            //pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(800, 300);
            lv = InternalOrderDB.SEFIDWiseIOSelectionView("IOPRODUCT");
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(800, 250));
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
                //btnSelectPO.Enabled = true;

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
                    MessageBox.Show("Select one Internal Order");
                    return;
                }
                else
                {
                    string ionos = ";";
                    string iodates = ";";
                    string refString = "";
                    string custStr = "";
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            //btnSelectInternalOrderNo.Enabled = true;
                            ionos = ionos + itemRow.SubItems[2].Text + ";";
                            iodates = iodates + itemRow.SubItems[3].Text + ";";
                            refString = refString + itemRow.SubItems[4].Text + Main.delimiter2;
                            custStr = custStr + itemRow.SubItems[5].Text + Environment.NewLine;
                            //txtInternalOrderNo.Text = itemRow.SubItems[2].Text;
                            //dtInternalOrderDate.Value = Convert.ToDateTime(itemRow.SubItems[3].Text);
                            //txtReference.Text = itemRow.SubItems[4].Text;
                            IODocID = itemRow.SubItems[1].Text;
                        }
                    }
                    txtInternalOrderNos.Text = ionos;
                    txtInternalOrderDates.Text = iodates;
                    txtReference.Text = refString;
                    txtCustomers.Text = custStr;
                }
                //btnSelectInternalOrderNo.Enabled = true;
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
                // btnSelectInternalOrderNo.Enabled = true;
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

        private void txtPONo_TextChanged(object sender, EventArgs e)
        {
            txtReference.Enabled = false;
            txtReference.Text = "";
        }
        private void txtDCNo_TextChanged(object sender, EventArgs e)
        {
        }
        private void txtInvoiceNo_TextChanged(object sender, EventArgs e)
        {
        }
        //-----
        //private void listView1_ItemChecked1(object sender, ItemCheckedEventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception)
        //    {
        //    }
        //}



        private void btnCalculate_Click(object sender, EventArgs e)
        {
            verifyAndReworkProductionPlanDetailGridRows();
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
        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void cmbfloormanager_Click(object sender, EventArgs e)
        {
            //removeControlsFromPnllvPanel();
            //pnllv = new Panel();
            //pnllv.BorderStyle = BorderStyle.FixedSingle;
            ////btnfloormanager.Enabled = false;
            //pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));

            string DeptList = "Production" + Main.delimiter1 + "QC";
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            lv = EmployeePostingDB.EmpListViewForQC(DeptList, "");
            // this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked1);
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
                    MessageBox.Show("Select one Manager");
                    return;
                }
                if (kount > 1)
                {
                    MessageBox.Show("Select only one Manager");
                    return;
                }
                else
                {
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            txtfloormanager.Text = itemRow.SubItems[2].Text;
                            FloorManagerUserID = itemRow.SubItems[3].Text;
                            break;
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

        //private void cmbStockItemID_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (cmbStockItemID.SelectedIndex != -1)
        //        {

        //            //string str = cmbStockItemID.SelectedItem.ToString();
        //            string str = cmbStockItemID.SelectedItem.ToString().Trim().Substring(0, cmbStockItemID.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
        //            if ((txtInternalOrderNo.Text.Length != 0) && (cmbStockItemID.SelectedIndex != -1))
        //            {
        //                ////ListView lvv = InternalOrderDB.ReferenceIOSelectionView(docID);
        //                ////foreach (ListViewItem itemRow in lvv.Items)
        //                ////{
        //                ////    if ((txtInternalOrderNo.Text == itemRow.SubItems[2].Text) && (dtInternalOrderDate.Value == Convert.ToDateTime(itemRow.SubItems[3].Text)))
        //                ////    {
        //                ////        InternalOrderDocumentID = itemRow.SubItems[1].Text;
        //                ////    }
        //                ////}
        //                List<iodetail> IOList = InternalOrderDB.IODetailList(Convert.ToInt32(txtInternalOrderNo.Text), dtInternalOrderDate.Value, InternalOrderDocumentID);
        //                string stID = cmbStockItemID.SelectedItem.ToString().Trim().Substring(0, cmbStockItemID.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
        //                foreach (iodetail iod in IOList)
        //                {
        //                    if (iod.StockItemID == stID)
        //                    {
        //                        txtQuantity.Text = iod.Quantity.ToString();
        //                        Temp = Convert.ToDouble(iod.Quantity.ToString());
        //                    }

        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {


        }

        private void txtReference_TextChanged(object sender, EventArgs e)
        {

        }



        private void grdProductionPlanDetail_EditingControlShowing_1(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

        }

        //private void cmbStockItemID_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    if (grdProductionPlanDetail.Rows.Count != 0)
        //    {
        //        {
        //            DialogResult dialog = MessageBox.Show("Warning:\n Stock Holding Detail will be Removed", "", MessageBoxButtons.OKCancel);
        //            if (dialog == DialogResult.OK)
        //            {
        //                grdProductionPlanDetail.Rows.Clear();
        //            }
        //            else
        //                cmbStockItemID.SelectedIndex = cmbStockItemID.FindString(prevprodution.StockItemID);
        //        }
        //    }
        //}

        private void txtfloormanager_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtfloormanager_ControlAdded(object sender, ControlEventArgs e)
        {

        }

        private void dtPlannedStartDate_ValueChanged(object sender, EventArgs e)
        {
            //string dtPlannedStartDate = DateTimePicker.Value.ToString("yyyy-MM-dd")
        }
        private void removeControlsFromPnllvPanel()
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
        private void btnSelectStockItem_Click(object sender, EventArgs e)
        {
            string inos = "";
            string idates = "";
            if (txtInternalOrderNos.Text.Trim().Length == 0)
            {
                MessageBox.Show("Fill Internal ORder NO ");
                return;
            }
            inos = txtInternalOrderNos.Text; //;no1;no2;no3;
            idates = txtInternalOrderDates.Text; //;date1;date2;date3;
            string[] ioNoArr = inos.Split(';');
            string[] ioDateArr = idates.Split(';');

            List<iodetail> IOLIstmain = new List<iodetail>();
            for (int i = 0; i < ioNoArr.Length; i++)
            {
                if (ioNoArr[i].Length != 0)
                {
                    DateTime iodate = Convert.ToDateTime(ioDateArr[i]);
                    int iono = Convert.ToInt32(ioNoArr[i]);
                    List<iodetail> IOListTemp = InternalOrderDB.IODetailList(iono, iodate).OrderByDescending(iod => iod.StockItemName).ToList();
                    IOLIstmain = IOLIstmain.Concat(IOListTemp).ToList();
                }

            }

            //List<iodetail> IOList = IOListTemp.OrderByDescending(iod => iod.StockItemName).ToList();
            showproductListView(IOLIstmain);
        }
        private void showproductListView(List<iodetail> IODetail)
        {
            //removeControlsFromPnllvPanel();
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

            frmPopup.Size = new Size(650, 300);
            lv = getListView(IODetail);
            // this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView5_ItemChecked);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(650, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Click5);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click5);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
            //pnlAddEdit.Controls.Add(pnllv);
            //pnllv.BringToFront();
            //pnllv.Visible = true;
        }
        private ListView getListView(List<iodetail> IODetail)
        {
            ListView lv = new ListView();
            try
            {

                //btnSelectStockItem.Enabled = false;
                lv.View = System.Windows.Forms.View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                //PurchaseOrderDB podb = new PurchaseOrderDB();
                //List<poheader> POHeaders = podb.getFilteredPurchaseOrderHeader("", 6, "");
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("IONo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("IODate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Product", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Model No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Model Name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Quantity", -2, HorizontalAlignment.Left);
                foreach (iodetail iod in IODetail)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(iod.TemporaryNo.ToString());
                    item1.SubItems.Add(iod.TemporaryDate.ToString("yyyy-MM-dd"));
                    item1.SubItems.Add(iod.StockItemID + "-" + iod.StockItemName);
                    item1.SubItems.Add(iod.ModelNo);
                    item1.SubItems.Add(iod.ModelName);
                    item1.SubItems.Add(iod.Quantity.ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        private void lvOK_Click5(object sender, EventArgs e)
        {
            try
            {

                int kount = 0;
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        kount++;
                    }
                }
                if (kount != 1)
                {
                    MessageBox.Show("Select one Product");
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        txtStockItem.Text = itemRow.SubItems[3].Text;
                        txtModelNo.Text = itemRow.SubItems[4].Text;
                        txtModelName.Text = itemRow.SubItems[5].Text;
                        txtQuantity.Text = itemRow.SubItems[6].Text;
                        Temp = Convert.ToDouble(txtQuantity.Text);
                        break;
                    }
                }
                frmPopup.Close();
                frmPopup.Dispose();
                //btnSelectStockItem.Enabled = true;

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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProductionPlanHeaderDB Pdb = new ProductionPlanHeaderDB();
            ERPUserDB Udb = new ERPUserDB();
            string result = "", ss = "";
            string[] abcds = new string[10000];
            List<productionplanheader> Prodlist = Pdb.getFilteredOngoingProductionPlan();
            List<user> UserLIst = Udb.getUsers();

            var results = from p in Prodlist
                          group p by p.ProductionPlanNo into g
                          join Sy in UserLIst on g.FirstOrDefault().FloorManager equals Sy.userID
                          select new
                          {
                              ProductionPlanNo = g.Key,
                              productiondate = g.FirstOrDefault().ProductionPlanDate,
                              productionstrttime = g.FirstOrDefault().PlannedStartTime,
                              productionendtime = g.FirstOrDefault().PlannedEndTime,
                              ActulStartTime = g.FirstOrDefault().PlannedStartTime,
                              ActulEndTime = g.FirstOrDefault().PlannedEndTime,
                              FloorManager = Sy.userEmpName,
                              team = g.Select(i => i.ForwarderList.ToString().Replace("" + Main.delimiter1 + "", ",")).ToArray(),
                          };

            int j = 0;

            grdOngoingPlns.Rows.Clear();
            foreach (var abc in results)
            {
                j++;
                result = string.Join(",", abc.team);
                abcds = result.Split(',');
                var rr = from x in UserLIst
                         where abcds.Contains(x.userID)
                         select new
                         {
                             Team = x.userEmpName
                         };
                ss = string.Join(", ", rr.Select(i => i.Team.ToString()).ToArray());

                // grid fill
                grdOngoingPlns.Rows.Add();
                grdOngoingPlns.Rows[grdOngoingPlns.RowCount - 1].Cells["SRno"].Value = j.ToString();
                grdOngoingPlns.Rows[grdOngoingPlns.RowCount - 1].Cells["ProductionPlanNO"].Value = abc.ProductionPlanNo;
                grdOngoingPlns.Rows[grdOngoingPlns.RowCount - 1].Cells["ProductioPlanDate"].Value = abc.productiondate;
                grdOngoingPlns.Rows[grdOngoingPlns.RowCount - 1].Cells["StartTime"].Value = abc.productionstrttime;
                grdOngoingPlns.Rows[grdOngoingPlns.RowCount - 1].Cells["EndTime"].Value = abc.productionendtime;
                grdOngoingPlns.Rows[grdOngoingPlns.RowCount - 1].Cells["ActulStartTime"].Value = abc.productionstrttime;
                grdOngoingPlns.Rows[grdOngoingPlns.RowCount - 1].Cells["ActulEndTime"].Value = abc.productionendtime;
                grdOngoingPlns.Rows[grdOngoingPlns.RowCount - 1].Cells["FloorManager"].Value = abc.FloorManager;
                grdOngoingPlns.Rows[grdOngoingPlns.RowCount - 1].Cells["team"].Value = ss;

            }

        }
        //-----
        //-- Validating Header and Detail String For Single Quotes

        private productionplanheader verifyHeaderInputString(productionplanheader pph)
        {
            try
            {
                pph.Reference = Utilities.replaceSingleQuoteWithDoubleSingleQuote(pph.Reference);
                pph.StockItemID = Utilities.replaceSingleQuoteWithDoubleSingleQuote(pph.StockItemID);
                pph.ModelNo = Utilities.replaceSingleQuoteWithDoubleSingleQuote(pph.ModelNo);
                pph.ModelName = Utilities.replaceSingleQuoteWithDoubleSingleQuote(pph.ModelName);
                pph.Remarks = Utilities.replaceSingleQuoteWithDoubleSingleQuote(pph.Remarks);
            }
            catch (Exception ex)
            {
            }
            return pph;
        }
        private void verifyDetailInputString()
        {
            try
            {
                for (int i = 0; i < grdProductionPlanDetail.Rows.Count; i++)
                {
                    grdProductionPlanDetail.Rows[i].Cells["gProcessID"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdProductionPlanDetail.Rows[i].Cells["gProcessID"].Value.ToString());
                    grdProductionPlanDetail.Rows[i].Cells["gProcessDescription"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdProductionPlanDetail.Rows[i].Cells["gProcessDescription"].Value.ToString());
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

        private void grdProductionPlanDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    grdProductionPlanDetail.Rows[e.RowIndex].Selected = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                string selectedValue = ((Structures.ComboBoxItem)cmbProdStatFilter.SelectedItem).HiddenValue;
                if (selectedValue == null || selectedValue.Length == 0)
                {
                    return;
                }
                if (selectedValue == "All")
                {
                    foreach (DataGridViewRow row in grdList.Rows)
                    {
                        row.Visible = true;
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in grdList.Rows)
                    {
                        if (row.Cells["gProductionStatus"].Value.ToString() == selectedValue)
                            row.Visible = true;
                        else
                            row.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            //try
            //{
            //    foreach (DataGridViewRow row in grdList.Rows)
            //    {
            //        row.Visible = true;
                    
            //    }
            //    if (cmbProdStatFilter.SelectedItem.ToString().Length != 0)
            //    {
            //        if (cmbProdStatFilter.SelectedItem.ToString() == "Halted")
            //            foreach (DataGridViewRow row in grdList.Rows)
            //            {
            //                if (row.Cells["gProductionStatus"].Value.ToString() != "2")
            //                {
            //                    row.Visible = false;
            //                }
            //            }
            //        else if (cmbProdStatFilter.SelectedItem.ToString() == "Canceled")
            //            foreach (DataGridViewRow row in grdList.Rows)
            //            {
            //                if (row.Cells["gProductionStatus"].Value.ToString() != "3")
            //                {
            //                    row.Visible = false;
            //                }
            //            }
            //        else if (cmbProdStatFilter.SelectedItem.ToString() == "Completed")
            //            foreach (DataGridViewRow row in grdList.Rows)
            //            {
            //                if (row.Cells["gProductionStatus"].Value.ToString() != "99")
            //                {
            //                    row.Visible = false;
            //                }
            //            }
            //        else if (cmbProdStatFilter.SelectedItem.ToString() == "Ongoing")
            //            foreach (DataGridViewRow row in grdList.Rows)
            //            {
            //                if (row.Cells["gProductionStatus"].Value.ToString() != "1")
            //                {
            //                    row.Visible = false;
            //                }
            //            }
            //        else if (cmbProdStatFilter.SelectedItem.ToString() == "Not started")
            //            foreach (DataGridViewRow row in grdList.Rows)
            //            {
            //                if (row.Cells["gProductionStatus"].Value.ToString() != "0")
            //                {
            //                    row.Visible = false;
            //                }
            //            }
            //        else
            //        {

            //        }
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void ProductionPlanHeader_Enter(object sender, EventArgs e)
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
        //private string getProdStatString(string stat)
        //{
        //    string str = "";
        //    if (stat == "2")
        //        str = "Halted";
        //    else if (stat == "3")
        //        str = "Canceled";
        //    else if (stat == "99")
        //        str = "Completed";
        //    else if (stat == "1")
        //        str = "Ongoing";
        //    else if (stat == "0")
        //        str = "Not Started";
        //    return str;
        //}
        //private void grdProductionPlanDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    grdProductionPlanDetail.Rows[e.RowIndex].Selected = true;
        //}

        //private void grdList_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    grdList.Rows[e.RowIndex].Selected = true;
        //}
    }
}




