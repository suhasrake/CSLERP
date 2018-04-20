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
    public partial class ProductionStatusChange : System.Windows.Forms.Form
    {
        string userString = "";
        string userCommentStatusString = "";
        Form dtpForm = new Form();
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        productionplanheader prevprodution;
        Form frmPopup = new Form();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        //ComboBox cmbStore = new ComboBox();
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        //string selStore = "";
        //ComboBox cmbStore = new ComboBox();
        public ProductionStatusChange()
        {
            try
            {

                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }

        private void ProductionStatusChange_Load(object sender, EventArgs e)
        {
            //////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;
            grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdList.EnableHeadersVisualStyles = false;
            String a = this.Size.ToString();
        }

        //called only in the beginning
        private void initVariables()
        {

            ////docID = Main.currentDocument;
            dtProductionPlanDate.Format = DateTimePickerFormat.Custom;
            dtProductionPlanDate.CustomFormat = "dd-MM-yyyy";
            dtProductionPlanDate.Enabled = false;
            dtPlanedEndTime.Format = DateTimePickerFormat.Custom;
            dtPlanedEndTime.CustomFormat = "dd-MM-yyyy HH:mm:ss";
            dtPlanedEndTime.Enabled = false;
            dtPlannedStartTime.Format = DateTimePickerFormat.Custom;
            dtPlannedStartTime.CustomFormat = "dd-MM-yyyy HH:mm:ss";
            dtPlannedStartTime.Enabled = false;

            dtActualStartTime.Format = DateTimePickerFormat.Custom;
            dtActualStartTime.CustomFormat = "dd-MM-yyyy HH:mm:ss";
            dtActualStartTime.Enabled = false;
            dtActualEndTime.Format = DateTimePickerFormat.Custom;
            dtActualEndTime.CustomFormat = "dd-MM-yyyy HH:mm:ss";
            dtActualEndTime.Enabled = false;
            
            //StoreEmpMappingDB.fillLocationCombo(cmbStore);
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            txtComments.Text = "";

            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
            grdList.Visible = true;
            pnlAddEdit.Visible = false;
            setTabIndex();
            ListFilteredProductionPlanHeader();
        }
        private void ListFilteredProductionPlanHeader()
        {
            try
            {
                grdList.Rows.Clear();
                ProductionPlanHeaderDB pphdb = new ProductionPlanHeaderDB();

                List<productionplanheader> ProductionPlanHeaderList = pphdb.getOnGoingProductionPlansForProcessStatus(2);
                foreach (productionplanheader pph in ProductionPlanHeaderList)
                {
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
                }

                setButtonVisibility("init");
                pnlList.Visible = true;
                grdList.Visible = true;
                pnlAddEdit.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Production Plan Listing");
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
        private void setTabIndex()
        {
            txtProductionPlanNo.TabIndex = 0;
            dtProductionPlanDate.TabIndex = 1;
            txtProductName.TabIndex = 3;
            txtModel.TabIndex = 4;
            txtQuantity.TabIndex = 5;
            dtPlannedStartTime.TabIndex = 6;
            dtPlanedEndTime.TabIndex = 7;
            cmbProductionStatus.TabIndex = 8;

            btnCancel.TabIndex = 0;
            btnSave.TabIndex = 1;
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
                cmbProductionStatus.Enabled = false;
                pnlAddEdit.Visible = false;
                cmbProductionStatus.Text = "";
                cmbProductionStatus.Items.Clear();
                txtProductionPlanNo.Text = "";
                dtProductionPlanDate.Value = DateTime.Parse("01-01-1900");
                txtProductName.Text = "";
                txtModel.Text = "";
                txtQuantity.Text = "";
                prevprodution = new productionplanheader();
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
        //private void showStoreLocationFillPanel()
        //{
        //    frmPopup = new Form();
        //    frmPopup.StartPosition = FormStartPosition.CenterScreen;
        //    frmPopup.BackColor = Color.CadetBlue;
        //    frmPopup.MaximizeBox = false;
        //    frmPopup.MinimizeBox = false;
        //    frmPopup.ControlBox = false;
        //    frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;
        //    frmPopup.Size = new Size(330, 140);

        //    Label head = new Label();
        //    head.AutoSize = true;
        //    head.Location = new System.Drawing.Point(13, 13);
        //    head.Name = "label2";
        //    head.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
        //    head.ForeColor = Color.White;
        //    head.Size = new System.Drawing.Size(146, 13);
        //    head.Text = "Select Store For This Product";
        //    frmPopup.Controls.Add(head);

        //    Label lblStore = new Label();
        //    lblStore.AutoSize = true;
        //    lblStore.Location = new System.Drawing.Point(45, 52);
        //    lblStore.Name = "lblStore";
        //    lblStore.Size = new System.Drawing.Size(76, 13);
        //    lblStore.Text = "Store Location";
        //    frmPopup.Controls.Add(lblStore);

        //    cmbStore.SelectedIndex = -1;
        //    cmbStore.FormattingEnabled = true;
        //    cmbStore.DropDownStyle = ComboBoxStyle.DropDownList;
        //    cmbStore.Location = new System.Drawing.Point(127, 50);
        //    cmbStore.Name = "cmbStoreLoc";
        //    cmbStore.Size = new System.Drawing.Size(133, 21);
        //    frmPopup.Controls.Add(cmbStore);

        //    Button lvOK = new Button();
        //    lvOK.Text = "OK";
        //    lvOK.BackColor = Color.Tan;
        //    lvOK.Location = new System.Drawing.Point(165, 83);
        //    lvOK.Click += new System.EventHandler(this.lvOK_Click5);
        //    frmPopup.Controls.Add(lvOK);

        //    Button lvCancel = new Button();
        //    lvCancel.Text = "CANCEL";
        //    lvCancel.BackColor = Color.Tan;
        //    lvCancel.Location = new System.Drawing.Point(72, 83);
        //    lvCancel.Click += new System.EventHandler(this.lvCancel_Click5);
        //    frmPopup.Controls.Add(lvCancel);

        //    frmPopup.ShowDialog();
        //}
        //private void lvOK_Click5(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if(cmbStore.SelectedIndex == -1)
        //        {
        //            MessageBox.Show("select Store Location");
        //            return;
        //        }
        //        selStore = cmbStore.SelectedItem.ToString();
        //        frmPopup.Close();
        //        //frmPopup.Dispose();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //private void lvCancel_Click5(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        selStore = "";
        //        frmPopup.Close();
        //        //frmPopup.Dispose();

        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        private void pnlList_Paint(object sender, PaintEventArgs e)
        {

        }
        private void btnPDFClose_Click(object sender, EventArgs e)
        {

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
        private void btnListDocuments_Click(object sender, EventArgs e)
        {

        }

        private void dgvDocumentList_CellContentClick(Object sender, DataGridViewCellEventArgs e)
        {
        }
        private void setButtonVisibility(string btnName)
        {
            try
            {
                btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;

                btnGetComments.Visible = false;
                chkCommentStatus.Visible = false;
                txtComments.Visible = false;
                //disableTabPages();
                clearTabPageControls();
                //----24/11/2016
                ////handleGrdEditButton();
                pnlBottomButtons.Visible = true;
                //----
                if (btnName == "init")
                {

                    btnExit.Visible = true;

                }
                else if (btnName == "Commenter")
                {
                    btnCancel.Visible = true;
                    btnSave.Visible = true;

                    btnGetComments.Visible = false; //earlier Edit enabled this button
                    chkCommentStatus.Visible = true;
                    txtComments.Visible = true;
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
                    //enableTabPages();
                    //pnlPDFViewer.Visible = true;
                    chkCommentStatus.Visible = true;
                    txtComments.Visible = true;
                    //tabControl1.SelectedTab = tabProductionPlanHeader;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;

                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;

                }
                else if (btnName == "LoadDocument")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                }


                ///pnlEditButtons.Refresh();

            }
            catch (Exception ex)
            {
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
                dgvComments.Rows.Clear();
                chkCommentStatus.Checked = false;
                txtComments.Text = "";
            }
            catch (Exception ex)
            {
            }
        }
        //private void btnSelectProductionPlanNo_Click(object sender, EventArgs e)
        //{
        //    if (txtProductionPlanNo.Text.Length != 0)
        //    {
        //        DialogResult dialog = MessageBox.Show("Plan No and details will be removed?", "Yes", MessageBoxButtons.YesNo);
        //        if (dialog == DialogResult.Yes)
        //        {
        //            txtProductionPlanNo.Text = "";
        //            txtModel.Text = "";
        //            txtProductName.Text = "";
        //            dtProductionPlanDate.Value = DateTime.Parse("01-01-1900");
        //            cmbProductionStatus.SelectedIndex = -1;
        //            cmbProductionStatus.Items.Clear();
        //        }
        //        else
        //            return;
        //    }

        //    frmPopup = new Form();
        //    frmPopup.StartPosition = FormStartPosition.CenterScreen;
        //    frmPopup.BackColor = Color.CadetBlue;

        //    frmPopup.MaximizeBox = false;
        //    frmPopup.MinimizeBox = false;
        //    frmPopup.ControlBox = false;
        //    frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

        //    frmPopup.Size = new Size(700, 300);
        //    lv = ProductionPlanHeaderDB.getPlanNoListViewForStatusChange();
        //    lv.Bounds = new Rectangle(new Point(0, 0), new Size(700, 250));
        //    frmPopup.Controls.Add(lv);

        //    Button lvOK = new Button();
        //    lvOK.BackColor = Color.Tan;
        //    lvOK.Text = "OK";
        //    lvOK.Location = new Point(40, 265);
        //    lvOK.Click += new System.EventHandler(this.lvOK_Click4);
        //    frmPopup.Controls.Add(lvOK);

        //    Button lvCancel = new Button();
        //    lvCancel.BackColor = Color.Tan;
        //    lvCancel.Text = "CANCEL";
        //    lvCancel.Location = new Point(130, 265);
        //    lvCancel.Click += new System.EventHandler(this.lvCancel_Click4);
        //    frmPopup.Controls.Add(lvCancel);

        //    frmPopup.ShowDialog();
        //}
        //string item = "";
        //private void lvOK_Click4(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int kount = 0;
        //        foreach (ListViewItem itemRow in lv.Items)
        //        {
        //            if (itemRow.Checked)
        //            {
        //                kount++;
        //            }
        //        }
        //        if (kount == 0)
        //        {
        //            MessageBox.Show("Select one Production Plan No");
        //            return;
        //        }
        //        if (kount > 1)
        //        {
        //            MessageBox.Show("Select only one Production Plan No");
        //            return;
        //        }
        //        else
        //        {
        //            foreach (ListViewItem itemRow in lv.Items)
        //            {
        //                if (itemRow.Checked)
        //                {
                           
        //                    break;
        //                }
        //            }
        //        }
        //        frmPopup.Close();
        //        frmPopup.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
       
        //private void lvCancel_Click4(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        frmPopup.Close();
        //        frmPopup.Dispose();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //-----
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

        //private void cmbProductionStatus_DropDown(object sender, EventArgs e)
        //{
        //    cmbProductionStatus.Items.Remove(item);
        //    cmbProductionStatus.Text = "";
        //    // MessageBox.Show("Done");
        //}

        //private void cmbProductionStatus_DropDownClosed(object sender, EventArgs e)
        //{

        //}
        ToolTip tt1 = new ToolTip();
        ToolTip tt2 = new ToolTip();
        private void txtProductName_MouseHover(object sender, EventArgs e)
        {
            tt1.SetToolTip(txtProductName, txtProductName.Text);
        }

        private void txtModel_MouseHover(object sender, EventArgs e)
        {
            tt2.SetToolTip(txtModel, txtModel.Text);
        }

        private void txtModel_MouseLeave(object sender, EventArgs e)
        {
            tt1.Hide(txtProductName);
        }

        private void txtProductName_MouseLeave(object sender, EventArgs e)
        {
            tt2.Hide(txtModel);
        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Det"))
                {
                    string docid = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    int TempNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value);
                    DateTime TmepDatechk = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value);
                    productionplanheader pph = new productionplanheader();
                    pph.DocumentID = docid;
                    pph.TemporaryNo = TempNo;
                    pph.TemporaryDate = TmepDatechk;
                    pph.ProductionPlanNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gProductionPlanNo"].Value);
                    pph.ProductionPlanDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["gProductionPlanDate"].Value);
                    pph.StockItemName = grdList.Rows[e.RowIndex].Cells["gStockItemName"].Value.ToString();
                    pph.Quantity = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gQuantity"].Value);
                    pph.PlannedStartTime = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["gPlannedStartTime"].Value);
                    pph.PlannedEndTime = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["gPlannedEndTime"].Value);
                    pph.ActualStartTime = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["gActualStartTime"].Value);
                    pph.ActualEndTime = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["gActualEndTime"].Value);
                    pph.ProductionStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gProductionStatus"].Value);


                    txtProductionPlanNo.Text = pph.ProductionPlanNo.ToString();
                    dtProductionPlanDate.Value = pph.ProductionPlanDate;
                    //txtModel.Text = pph.ModelName;
                    txtProductName.Text = pph.StockItemName;
                    dtPlannedStartTime.Value = pph.PlannedStartTime;
                    dtPlanedEndTime.Value = pph.PlannedEndTime;
                    txtQuantity.Text = pph.Quantity.ToString();
                    fillProductionStatusCumbo(pph.ProductionStatus);

                    //Get Actual Start And End Time
                    DateTime[] dtArr = ProductionPlanHeaderDB.
                        getActualStartAndEndTimeForAPlan(pph.ProductionPlanNo, pph.ProductionPlanDate);
                    dtActualStartTime.Value = dtArr[0];
                    dtActualEndTime.Value = dtArr[1];
                    cmbProductionStatus.Enabled = true;
                    pnlList.Visible = true;
                    pnlAddEdit.Visible = true;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    pnlAddEdit.BringToFront();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void fillProductionStatusCumbo(int status)
        {
            try
            {
                StatusCatalogueDB.fillStatusCatalogueCombo(cmbProductionStatus, "PRODUCTIONPLAN"); // adding all status availble

                Dictionary<string, Structures.ComboBoxItem> CmbDict = new Dictionary<string, Structures.ComboBoxItem>();
                foreach (var item in cmbProductionStatus.Items)
                {
                    Structures.ComboBoxItem val = ((Structures.ComboBoxItem)item);
                    string id = ((Structures.ComboBoxItem)val).HiddenValue;
                    CmbDict.Add(id,val);
                }
                if (status == 1) // approved
                {
                    cmbProductionStatus.Items.Clear();
                    //cmbProductionStatus.Items.Add(CmbDict["2"]); //For start
                    cmbProductionStatus.Items.Add(CmbDict["3"]); //For Cancel
                    cmbProductionStatus.Items.Add(CmbDict["4"]); //For halt
                }
                else if(status == 2) // started
                {
                    cmbProductionStatus.Items.Clear();
                    cmbProductionStatus.Items.Add(CmbDict["99"]); //For complete
                    cmbProductionStatus.Items.Add(CmbDict["3"]); //For cancel
                    cmbProductionStatus.Items.Add(CmbDict["4"]); //For halt
                }
                else if (status ==4) //Halted
                {
                    cmbProductionStatus.Items.Clear();
                    cmbProductionStatus.Items.Add(CmbDict["5"]); //For Resume
                    cmbProductionStatus.Items.Add(CmbDict["4"]); //For Cancel
                }
                else if (status == 5)  // Resumed
                {
                    cmbProductionStatus.Items.Clear();
                    //cmbProductionStatus.Items.Add(CmbDict["2"]); //For start
                    cmbProductionStatus.Items.Add(CmbDict["3"]); //For Halt
                    cmbProductionStatus.Items.Add(CmbDict["4"]); //For Cancel
                    cmbProductionStatus.Items.Add(CmbDict["99"]); //For complete
                }
            }
            catch(Exception ex)
            {

            }
            if (status.Equals("1"))
            {
                cmbProductionStatus.Items.Add("2-Halted");
                cmbProductionStatus.Items.Add("3-Canceled");
                cmbProductionStatus.Items.Add("99-Completed");
            }
            else if (status.Equals("2"))
            {
                cmbProductionStatus.Items.Add("1-Production Stage");
            }
        }

        private void showProdcutionDetails()
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {
                ProductionPlanHeaderDB phDB = new ProductionPlanHeaderDB();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;
                if (cmbProductionStatus.SelectedIndex == -1)
                {
                    MessageBox.Show("Select Production Status");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    int stat = Convert.ToInt32(((Structures.ComboBoxItem)cmbProductionStatus.SelectedItem).HiddenValue);
                    if ((dtActualEndTime.Value <= dtActualStartTime.Value) && stat != 3 && stat != 4 && stat != 5)
                    {
                        MessageBox.Show("Actual End Time should be more than the actual start time.");
                        return;
                    }
                   
                    int planNo = Convert.ToInt32(txtProductionPlanNo.Text);
                    string loc = "FACTORYSTORE";
                    DateTime planDate = dtProductionPlanDate.Value;
                    productionplanheader pph = new productionplanheader();
                    if (!ProductionPlanHeaderDB.checkproductionClosedStatus(planNo, planDate) && stat == 99)
                    {
                        MessageBox.Show("CLose all production process for this production plan.");
                        MessageBox.Show("Failed to update Status");
                        return;
                    }
                    DialogResult dialog = MessageBox.Show("Are you sure to Finalize the production?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        ///return;
                        pph = phDB.getProductionPlanHeaderDetail(planNo, planDate);

                        //Storing Actual StartTIme And Actual End time
                        pph.ActualStartTime = dtActualStartTime.Value;
                        pph.ActualEndTime = dtActualEndTime.Value;
                        if (phDB.updateProductionStatus(stat, pph, loc))
                        {
                            MessageBox.Show("Status updated");
                            if (stat == 99)
                                MessageBox.Show("Stock Added TO Factory Store.");
                            pnlAddEdit.Visible = false;
                            ListFilteredProductionPlanHeader();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update Status");
                            pnlAddEdit.Visible = false;
                            status = false;
                        }
                    }
                    else
                        status = false;
                }
                else
                    status = false;

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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlAddEdit.Visible = false;
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }

        private void ProductionStatusChange_Enter(object sender, EventArgs e)
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















