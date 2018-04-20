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
    public partial class ProductModels : System.Windows.Forms.Form
    {
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        string docID = "PRODUCTMODELS";
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        string pid = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        productmodels prevpmodel;
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnlForwarder = new Panel();
        TreeView tv = new TreeView();
        ListView lvApprover = new ListView();
        Form frmPopup = new Form();
        public ProductModels()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void ProductModels_Load(object sender, EventArgs e)
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
            disableControlsInPnlList();
            txtSelectStockItemID.Text = "";
            //ListFilteredProductModels(listOption);
            //applyPrivilege();
        }
        private void disableControlsInPnlList()
        {
            pnlList.Visible = true;
            pnlBottomButtons.Visible = false;
            pnlTobButtons.Visible = false;
            grdList.Visible = false;
            label1.Visible = true;
            txtSelectStockItemID.Visible = true;
            btnSelectProduct.Visible = true;
        }
        private void enableControlsInPnlList()
        {
            pnlBottomButtons.Visible = true;
            pnlTobButtons.Visible = true;
            grdList.Visible = true;
            pnlList.Visible = true;
        }
        private void ListFilteredProductModels(int opt, string stockid)
        {
            try
            {
                grdList.Rows.Clear();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                ProductModelsDB pmdb = new ProductModelsDB();
                List<productmodels> PMList = pmdb.getFilteredModelName(userString, opt, stockid);
                if (opt == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (opt == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (opt == 3 || opt == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (productmodels pm in PMList)
                {
                    if (opt == 1)
                    {
                        if (pm.DocumentStatus == 99)
                            continue;
                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["RowID"].Value = pm.rowID;
                    grdList.Rows[grdList.RowCount - 1].Cells["ModelNo"].Value = pm.ModelNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["ModelName"].Value = pm.ModelName;
                    grdList.Rows[grdList.RowCount - 1].Cells["StockItem"].Value = pm.StockItemID;
                    grdList.Rows[grdList.RowCount - 1].Cells["StockItemName"].Value = pm.StockItemName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Remarks"].Value = pm.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = pm.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = pm.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = pm.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwardUser"].Value = pm.ForwardUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ApproveUser"].Value = pm.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = pm.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = pm.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = pm.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatusNo"].Value = pm.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = pm.ForwarderList;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Product Model Listing");
            }
            setButtonVisibility("init");
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
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            setButtonVisibility("init");
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
                //----------clear temperory panels
                clearTabPageControls();
                pnlForwarder.Visible = false;
                txtModelName.Text = "";
                txtModelNo.Text = "";
                txtStockItemID.Text = "";
                txtRemarks.Text = "";
                prevpmodel = new productmodels();
                //removeControlsFromForwarderPanel();
                //removeControlsFromForwarderPanelTV();
                
            }
            catch (Exception ex)
            {

            }
        }
        private void btnSelectProduct_Click_1(object sender, EventArgs e)
        {
            showStockItemTreeView();
        }
        private void txtSelectStockItemID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                pid = txtSelectStockItemID.Text.Substring(0, txtSelectStockItemID.Text.IndexOf('-'));
                //txtStockItemID.Text = txtSelectStockItemID.Text;
            }
            catch (Exception ex)
            {
            }
        }
        private void showStockItemTreeView()
        {
            removeControlsFromForwarderPanelTV();
            tv = new TreeView();
            tv.CheckBoxes = true;
            tv.Nodes.Clear();
            tv.CheckBoxes = true;
            pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
            pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));

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
            pnlList.Controls.Add(pnlForwarder);
            pnlList.BringToFront();
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
                    txtSelectStockItemID.Text = s;
                    tv.CheckBoxes = true;
                    pnlForwarder.Controls.Remove(lvApprover);
                    pnlForwarder.Visible = false;
                    if (getuserPrivilegeStatus() == 1)
                    {
                        //user is only a viewer
                        listOption = 6;
                        ListFilteredProductModels(listOption, pid);
                    }
                    else
                    {
                        listOption = 1;
                        ListFilteredProductModels(listOption, pid);
                    }
                    enableControlsInPnlList();
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
                pnlForwarder.Controls.Remove(lvApprover);
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
                tabControl1.SelectedTab = tabProductModel;
                setButtonVisibility("btnNew");
                txtStockItemID.Text=txtSelectStockItemID.Text;
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
        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredProductModels(listOption,pid);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredProductModels(listOption, pid);
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

            ListFilteredProductModels(listOption, pid);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            Boolean status = true;
            try
            {

                ProductModelsDB pmdb = new ProductModelsDB();
                productmodels pm = new productmodels();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    pm.ModelName = txtModelName.Text;
                    pm.StockItemID = txtStockItemID.Text.Substring(0,txtStockItemID.Text.IndexOf('-'));
                    pm.Remarks = txtRemarks.Text;
                    pm.ForwarderList = prevpmodel.ForwarderList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (pmdb.validateProductModel(pm))
                {
                    if (btnText.Equals("Update"))
                    {
                        pm.DocumentStatus = prevpmodel.DocumentStatus;
                        if (pmdb.updateProductModels(pm, prevpmodel))
                        {
                            MessageBox.Show("Product Model Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredProductModels(listOption, pid);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update Product Model");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        pm.DocumentStatus = 1;
                        if (pmdb.insertProductModel(pm))
                        {
                            MessageBox.Show("Product Model Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredProductModels(listOption, pid);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert Product Model");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Product Model Validation failed");
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Product Model(Save()) : Error");
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
                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("LoadDocument") ||
                    columnName.Equals("View"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                   // QIHeaderDB qidb = new QIHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevpmodel = new productmodels();
                    prevpmodel.rowID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["RowID"].Value.ToString());
                    prevpmodel.StockItemID = grdList.Rows[e.RowIndex].Cells["StockItem"].Value.ToString();
                    prevpmodel.StockItemName = grdList.Rows[e.RowIndex].Cells["StockItemName"].Value.ToString();
                    prevpmodel.ModelNo = grdList.Rows[e.RowIndex].Cells["ModelNo"].Value.ToString();
                    prevpmodel.ModelName = grdList.Rows[e.RowIndex].Cells["ModelName"].Value.ToString();
                    prevpmodel.Remarks = grdList.Rows[e.RowIndex].Cells["Remarks"].Value.ToString();
                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Model No:" + prevpmodel.ModelNo + "\n" +
                            "Model Detail:" + prevpmodel.ModelName + "\n" +
                             "Product ID:" + prevpmodel.StockItemID + "\n" +
                              "Product Name:" + prevpmodel.StockItemName;
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevpmodel.StockItemID + "-" + prevpmodel.ModelNo;
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_2(null, null);
                        return;
                    }
                    //--------
                    prevpmodel.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    prevpmodel.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    prevpmodel.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    prevpmodel.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    prevpmodel.ForwardUser = grdList.Rows[e.RowIndex].Cells["ForwardUser"].Value.ToString();
                    prevpmodel.ApproveUser = grdList.Rows[e.RowIndex].Cells["ApproveUser"].Value.ToString();
                    prevpmodel.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentStatusNo"].Value.ToString());
                    prevpmodel.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevpmodel.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    prevpmodel.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    txtModelName.Text = prevpmodel.ModelName.ToString();
                    txtModelNo.Text = prevpmodel.ModelNo.ToString();
                    txtRemarks.Text = prevpmodel.Remarks;
                    txtStockItemID.Text = prevpmodel.StockItemID +"-" + prevpmodel.StockItemName;
                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;

                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                    tabControl1.SelectedTab = tabProductModel;
                    tabControl1.Visible = true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
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
        private Boolean updateDashBoard(productmodels pm, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = docID;
                //dsb.TemporaryNo = pm.TemporaryNo;
                //dsb.TemporaryDate = pm.TemporaryDate;
                //dsb.DocumentNo = pm.DocumentNo;
                //dsb.DocumentDate = pm.DocumentDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = prevpmodel.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(docID);
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
        private void btnForward_Click(object sender, EventArgs e)
        {
            //removeControlsFromForwarderPanel();
            //lvApprover = new ListView();
            //lvApprover.Clear();
            //pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
            //pnlForwarder.Bounds = new Rectangle(new Point(100, 60), new Size(600, 300));
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
                            ProductModelsDB pmdb = new ProductModelsDB();
                            prevpmodel.ForwardUser = approverUID;
                            prevpmodel.ForwarderList = prevpmodel.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (pmdb.forwardProductModel(prevpmodel))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(prevpmodel, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredProductModels(listOption, pid);
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
        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                ProductModelsDB pmdb = new ProductModelsDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    string ModelNo = (Convert.ToInt32(pmdb.getLastModelNo(prevpmodel.StockItemID)) + 1).ToString();
                    if (pmdb.ApproveProductModel(prevpmodel, ModelNo))
                    {
                        MessageBox.Show("Product Model Approved");
                        if (!updateDashBoard(prevpmodel, 2))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                        }
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredProductModels(listOption, pid);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
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
                btnApprovalPending.Visible = true;
                btnApproved.Visible = true;
                btnNew.Visible = false;
                btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                btnForward.Visible = false;
                btnApprove.Visible = false;
                btnReverse.Visible = false;
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
                    tabPDFViewer.Enabled = true;
                }
                else if (btnName == "btnNew")
                {
                    btnNew.Visible = false; //added 24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = false;
                    tabPDFViewer.Enabled = false;
                    tabControl1.SelectedTab = tabProductModel;
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
                    enableTabPages();
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabProductModel;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                    disableTabPages();
                    tabControl1.SelectedTab = tabProductModel;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabProductModel;
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
                removeControlsFromForwarderPanel();
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
                    string reverseStr = getReverseString(prevpmodel.ForwarderList);
                    ProductModelsDB pmdb = new ProductModelsDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevpmodel.ForwarderList = reverseStr.Substring(0, ind);
                        prevpmodel.ForwardUser = reverseStr.Substring(ind + 3);
                        prevpmodel.DocumentStatus = prevpmodel.DocumentStatus - 1;
                    }
                    else
                    {
                        prevpmodel.ForwarderList = "";
                        prevpmodel.ForwardUser = "";
                        prevpmodel.DocumentStatus = 1;
                    }
                    if (pmdb.reverseProductModel(prevpmodel))
                    {
                        MessageBox.Show("Product Model Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredProductModels(listOption, pid);
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

        private void btnListDocument_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevpmodel.StockItemID + "-" + prevpmodel.ModelNo);
                pnlPDFViewer.Controls.Add(dgvDocumentList);
                dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
            }
            catch (Exception ex)
            {
            }
        }

        private void btnCloseDoc_Click(object sender, EventArgs e)
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
        private void removeControlsFromForwarderPanel()
        {
            try
            {
                //foreach (Control p in pnlForwarder.Controls)
                //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(TreeView) || p.GetType() == typeof(Button))
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
        private void removeControlsFromForwarderPanelTV()
        {
            try
            {
                //foreach (Control p in pnlForwarder.Controls)
                //    if (p.GetType() == typeof(TreeView) || p.GetType() == typeof(Button))
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
                    string subDir = prevpmodel.StockItemID + "-" + prevpmodel.ModelNo;
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //removeControlsFromForwarderPanel();
            //removeControlsFromForwarderPanelTV();
        }

        private void ProductModels_Enter(object sender, EventArgs e)
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

