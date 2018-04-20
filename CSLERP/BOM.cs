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
using System.Diagnostics;

namespace CSLERP
{
    public partial class BOM : System.Windows.Forms.Form
    {
        string docID = "BOM";
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Boolean userIsACommenter = false;
        private string[] ArithmetcOperators = { "Add", "Subtract", "Multiply", "Divide", "Percentage" };
        double totalcost = 0.0;
        Boolean captureChange = false;
        Boolean AddRowClick = false;
        TextBox txtSearch = new TextBox();
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        bomheader prevbom ;
        Timer filterTimer = new Timer();
        DataGridView grdStock = new DataGridView();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        int rowIndex = 0;
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Form frmPopup = new Form();
        public BOM()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void BOM_Load(object sender, EventArgs e)
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
            ListBOMHeader(listOption);
            //applyPrivilege();
        }
        private void ListBOMHeader(int option)
        {
            try
            {

                grdList.Rows.Clear();
                BOMDB bomdb = new BOMDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                List<bomheader> BOMHeaders = bomdb.getBOMHeader(userString, option, userCommentStatusString);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (bomheader bh in BOMHeaders)
                {
                    if (option == 1)
                    {
                        if (bh.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["ProductID"].Value = bh.ProductID;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProductName"].Value = bh.Name;
                    grdList.Rows[grdList.RowCount - 1].Cells["Details"].Value = bh.Details;
                    grdList.Rows[grdList.RowCount - 1].Cells["Cost"].Value = bh.Cost;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = bh.status;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatus"].Value = bh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = bh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = bh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = bh.ForwardUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = bh.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["CommStatus"].Value = bh.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = bh.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = bh.ForwarderList;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in BOM Header listing");
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
            closeAllPanels();

            txtComments.Text = "";

            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
            setTabIndex();
            ///closeAllPanels();
        }
        private void setTabIndex()
        {
            txtProduct.TabIndex = 0;
            btnSelProduct.TabIndex = 1;
            txtDetails.TabIndex = 2;
            txtCost.TabIndex = 3;
            cmbStatus.TabIndex = 4;

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
        public void clearData()
        {
            try
            {
                //clear all grid views
                grdBOMDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                //----------clear temperory panels

                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                //----------
                txtProduct.Text = "";
                txtDetails.Text = "";
                AddRowClick = false;
                //grdBOMDetail.Rows.Clear();
                txtCost.Text = "";
                btnProductValue.Text = "0";
                prevbom = new bomheader();
                commentStatus = "";
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

        private void btnNew_Click_1(object sender, EventArgs e)
        {
            try
            {
                clearData();
                btnSave.Text = "Save";
                closeAllPanels();
                tabControl1.SelectedTab = tabBOMHeader;
                setButtonVisibility("btnNew");
                AddRowClick = false;
            }
            catch (Exception)
            {

            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {


        }
        private List<bomdetail> getBOMDetails(string productID)
        {
            List<bomdetail> BOMDetail = new List<bomdetail>();
            try
            {
                bomdetail bd = new bomdetail();
                for (int i = 0; i < grdBOMDetail.Rows.Count; i++)
                {
                    try
                    {
                        string iid = "";
                        iid = grdBOMDetail.Rows[i].Cells["Item"].Value.ToString().Substring(0, grdBOMDetail.Rows[i].Cells["Item"].Value.ToString().IndexOf('-'));
                        bd = new bomdetail();
                        bd.ProductID = productID;
                        bd.StockItemID = iid;
                        bd.Quantity = Convert.ToDouble(grdBOMDetail.Rows[i].Cells["Quantity"].Value.ToString());
                        bd.PurchasePrice = Convert.ToDouble(grdBOMDetail.Rows[i].Cells["PurchasePrice"].Value.ToString());
                        bd.CustomPrice = Convert.ToDouble(grdBOMDetail.Rows[i].Cells["CustomPrice"].Value.ToString());
                        BOMDetail.Add(bd);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("createAndUpdateBOMDetails() : Error creating BOM Details");
                        BOMDetail = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("createAndUpdateBOMDetails() : Error updating BOM Details");
                BOMDetail = null;
            }
            return BOMDetail;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Boolean status = true;

            try
            {
                if (!verifyAndReworkBOMDetailGridRows())
                {
                    return;
                }
                string btnText = btnSave.Text;
                BOMDB bomdb = new BOMDB();
                bomheader bh = new bomheader();
                bh = new bomheader();
                string iid = txtProduct.Text;
                iid = iid.Substring(0, iid.IndexOf('-'));
                bh.ProductID = iid;
                bh.Details = txtDetails.Text;
                bh.Cost = totalcost;
                bh.Comments = docCmtrDB.DGVtoString(dgvComments);
                bh.ForwarderList = prevbom.ForwarderList;
                if (btnText.Equals("Save"))
                {
                    bh.DocumentStatus = 1; //created
                }
                else
                {
                    bh.DocumentStatus = prevbom.DocumentStatus;
                }
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;

                if (!validateItems(bh.ProductID))
                {
                    return;
                }
                if (bomdb.validateBOMHeader(bh))
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
                            bh.CommentStatus = docCmtrDB.createCommentStatusString(prevbom.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            bh.CommentStatus = prevbom.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            bh.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            bh.CommentStatus = prevbom.CommentStatus;
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
                        bh.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus);
                    }
                    //Replacing single quotes
                    bh = verifyHeaderInputString(bh);
                    List<bomdetail> BOMDetail = getBOMDetails(bh.ProductID);
                    if (btnText.Equals("Update"))
                    {
                        if (bomdb.updateBOMHeaderAndDetail(bh, prevbom, BOMDetail))
                        {
                            MessageBox.Show("BOM Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListBOMHeader(listOption);
                        }
                        else
                        {
                            status = false;

                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update BOM Header");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (bomdb.InsertBOMHeaderAndDetail(bh, BOMDetail))
                        {
                            MessageBox.Show("BOM Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListBOMHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert BOM Header");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("BOM  Validation failed");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed Adding / Editing BOM");
                return;
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }
        //-- Validating Header and Detail String For Single Quotes

        private bomheader verifyHeaderInputString(bomheader bomh)
        {
            try
            {
                bomh.Details = Utilities.replaceSingleQuoteWithDoubleSingleQuote(bomh.Details);
            }
            catch (Exception ex)
            {
            }
            return bomh;
        }
        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListBOMHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListBOMHeader(listOption);
        }

        private void Approved_Click(object sender, EventArgs e)
        {
            if (getuserPrivilegeStatus() == 2)
            {
                listOption = 6; //viewer
            }
            else
            {
                listOption = 3;
            }

            ListBOMHeader(listOption);
        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            string columnName = grdList.Columns[e.ColumnIndex].Name;
            try
            {

                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("LoadDocument") ||
                    columnName.Equals("View"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    AddRowClick = false;
                    captureChange = false;
                    int rowID = e.RowIndex;

                    btnSave.Text = "Update";
                    prevbom = new bomheader();


                    prevbom.CommentStatus = grdList.Rows[e.RowIndex].Cells["CommStatus"].Value.ToString();
                    prevbom.ProductID = grdList.Rows[e.RowIndex].Cells["ProductID"].Value.ToString();
                    prevbom.Details = grdList.Rows[e.RowIndex].Cells["Details"].Value.ToString();
                    if (prevbom.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    prevbom.Comments = BOMDB.getUserComments(prevbom.ProductID);



                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Product ID: " + prevbom.ProductID + "\n" +
                            "Details: " + prevbom.Details;
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevbom.ProductID;
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click(null, null);
                        return;
                    }
                    //--------
                    prevbom.status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString());
                    prevbom.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentStatus"].Value.ToString());
                    prevbom.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["CreateTime"].Value.ToString());
                    // prevbom.CreateUser = grdList.Rows[e.RowIndex].Cells["CreateUser"].Value.ToString();
                    prevbom.CreateUser = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    prevbom.ForwardUser = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevbom.ApproveUser = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    prevbom.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    //--comments
                    chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    prevbom.CommentStatus = grdList.Rows[e.RowIndex].Cells["CommStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(prevbom.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(prevbom.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---
                    //--------Load Documents
                    if (columnName.Equals("View"))
                    {
                        tabControl1.TabPages["tabBOMDetail"].Enabled = true;
                    }
                    //====
                    txtProduct.Text = grdList.Rows[e.RowIndex].Cells["ProductID"].Value.ToString() + "-" + grdList.Rows[e.RowIndex].Cells["ProductName"].Value.ToString();
                    txtDetails.Text = grdList.Rows[e.RowIndex].Cells["Details"].Value.ToString();
                    txtCost.Text = grdList.Rows[e.RowIndex].Cells["Cost"].Value.ToString();
                    //cmbStatus.SelectedIndex = cmbStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[4].Value.ToString());
                    DataGridViewRow row = grdList.Rows[rowID];
                    //get customer bank details
                    BOMDB bomdb = new BOMDB();
                    List<bomdetail> bomdetails = bomdb.getBOMDetail(grdList.Rows[e.RowIndex].Cells["ProductID"].Value.ToString());
                    grdBOMDetail.Rows.Clear();
                    int i = 0;
                    foreach (bomdetail bd in bomdetails)
                    {

                        AddBOMDetailRow();
                        grdBOMDetail.Rows[i].Cells["LineNo"].Value = i + 1;
                        grdBOMDetail.Rows[i].Cells["Item"].Value = bd.StockItemID + "-" + bd.Name;

                        grdBOMDetail.Rows[i].Cells["Quantity"].Value = bd.Quantity;
                        grdBOMDetail.Rows[i].Cells["PurchasePrice"].Value = bd.PurchasePrice;
                        grdBOMDetail.Rows[i].Cells["CustomPrice"].Value = bd.CustomPrice;

                        i++;
                    }
                    //txtCost.Enabled = false;
                    verifyAndReworkBOMDetailGridRows();
                    btnSave.Text = "Update";
                    pnlList.Visible = true;


                    //tabControl1.SelectedTab = tabBOMHeader;
                    //tabControl1.Visible = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                AddRowClick = true;
                AddBOMDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private Boolean AddBOMDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdBOMDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkBOMDetailGridRows())
                    {
                        return false;
                    }
                }
                grdBOMDetail.Rows.Add();
                int kount = grdBOMDetail.RowCount;
                grdBOMDetail.Rows[kount - 1].Cells["LineNo"].Value = kount;
                grdBOMDetail.Rows[kount - 1].Cells["Item"].Value = "";

                grdBOMDetail.Rows[kount - 1].Cells["Quantity"].Value = Convert.ToDouble(0);
                grdBOMDetail.Rows[kount - 1].Cells["PurchasePrice"].Value = Convert.ToDouble(0);
                grdBOMDetail.Rows[kount - 1].Cells["CustomPrice"].Value = Convert.ToDouble(0);
                grdBOMDetail.Rows[kount - 1].Cells["Value"].Value = Convert.ToDouble(0);
                if (AddRowClick)
                    grdBOMDetail.FirstDisplayedScrollingRowIndex = grdBOMDetail.RowCount - 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddBOMDetailRow() : Error");
            }

            return status;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }

        private void grdBOMDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string colName = grdBOMDetail.Columns[e.ColumnIndex].Name;
                if (colName == "Del")
                {
                    try
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdBOMDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkBOMDetailGridRows();
                    }
                    catch (Exception)
                    {

                    }
                }
                if (colName == "Sel")
                {
                    try
                    {
                        if (txtProduct.Text == null && txtProduct.Text.Trim().Length == 0)
                        {
                            MessageBox.Show("Select Product In BOM Header");
                            return;
                        }
                        if (grdBOMDetail.CurrentRow.Cells["Item"].Value.ToString().Length != 0)
                        {
                            DialogResult dialog = MessageBox.Show(" Current BOM Details will be removed ?", "Yes", MessageBoxButtons.YesNo);
                            if (dialog == DialogResult.Yes)
                            {
                                grdBOMDetail.Rows[e.RowIndex].Cells["Item"].Value = "" ;
                                grdBOMDetail.Rows[e.RowIndex].Cells["Quantity"].Value = Convert.ToDouble(0);
                                grdBOMDetail.Rows[e.RowIndex].Cells["PurchasePrice"].Value = Convert.ToDouble(0);
                                grdBOMDetail.Rows[e.RowIndex].Cells["CustomPrice"].Value = Convert.ToDouble(0);
                                grdBOMDetail.Rows[e.RowIndex].Cells["Value"].Value = Convert.ToDouble(0);
                            }
                            else
                                return;

                        }
                        rowIndex = e.RowIndex;
                        ShowItemListView();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void ShowItemListView()
        {

            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(750, 400);

            Label lblSearch = new Label();
            lblSearch.Location = new System.Drawing.Point(330, 5);
            lblSearch.AutoSize = true;
            lblSearch.Text = "Search by Name";
            lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
            lblSearch.ForeColor = Color.Black;
            frmPopup.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Size = new Size(220, 18);
            txtSearch.Location = new System.Drawing.Point(450, 3);
            txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
            txtSearch.ForeColor = Color.Black;
            txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInDocGridList);
            txtSearch.TabIndex = 0;
            txtSearch.Focus();
            frmPopup.Controls.Add(txtSearch);

            StockItemDB sidb = new StockItemDB();
            grdStock = sidb.getStockItemlistGrid();
            grdStock.Bounds = new Rectangle(new Point(0, 30), new Size(750, 320));
            
            frmPopup.Controls.Add(grdStock);
            grdStock.Columns["Name"].Width = 250;
            grdStock.Columns["StockItemID"].Width = 120;

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 365);
            lvOK.Click += new System.EventHandler(this.lvOK_Click2);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 365);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
            frmPopup.Controls.Add(lvCancel);

            frmPopup.ShowDialog();
        }
        private void lvOK_Click2(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    var checkedRows = from DataGridViewRow r in grdStock.Rows
                                      where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                      select r;
                    int selectedRowCount = checkedRows.Count();
                    if (selectedRowCount != 1)
                    {
                        MessageBox.Show("Select Any Product");
                        return;
                    }
                    
                    foreach (var row in checkedRows)
                    {
                        grdBOMDetail.CurrentRow.Cells["Item"].Value = row.Cells["StockItemID"].Value.ToString() + "-" + row.Cells["Name"].Value.ToString();
                        string id = row.Cells["StockItemID"].Value.ToString();
                        grdBOMDetail.CurrentRow.Cells["PurchasePrice"].Value = StockDB.getLastPurchasePriceForBOM(id);
                        grdBOMDetail.CurrentRow.Cells["CustomPrice"].Value = Convert.ToDouble(grdBOMDetail.CurrentRow.Cells["PurchasePrice"].Value);
                    }
                    frmPopup.Close();
                    frmPopup.Dispose();
                }
                catch (Exception)
                {
                }

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
        private void txtSearch_TextChangedInDocGridList(object sender, EventArgs e)
        {
            try
            {
                //grdForStockList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                filterGridDocData();
                ///grdForStockList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {

            }
        }
        private void filterGridDocData()
        {
            try
            {
                grdStock.CurrentCell = null;
                foreach (DataGridViewRow row in grdStock.Rows)
                {
                    row.Visible = true;
                    row.Cells["Select"].Value = false;
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
        private Boolean verifyAndReworkBOMDetailGridRows()
        {
            Boolean status = true;

            try
            {
                double quantity = 0;
                double purchaseprice = 0;
                double customprice = 0.0;
                double cost = 0.0;
                totalcost = 0.0;
                BOMDB bomdb = new BOMDB();
                if (grdBOMDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in BOM table");
                    return false;
                }
                for (int i = 0; i < grdBOMDetail.Rows.Count; i++)
                {
                    grdBOMDetail.Rows[i].Cells[0].Value = (i + 1);
                    if ((grdBOMDetail.Rows[i].Cells["Item"].Value == null) ||
                        (Convert.ToDouble(grdBOMDetail.Rows[i].Cells["Quantity"].Value) == 0) ||
                        ((Convert.ToDouble(grdBOMDetail.Rows[i].Cells["PurchasePrice"].Value) == 0) &&
                        (Convert.ToDouble(grdBOMDetail.Rows[i].Cells["CustomPrice"].Value) == 0)))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                    //if (Convert.ToInt32(grdBOMDetail.Rows[i].Cells[7].Value.ToString()) == 1)
                    //{
                    //    //load new purchase price  or BOM cost
                    //    grdBOMDetail.Rows[i].Cells[3].Value = 0.0;
                    //    grdBOMDetail.Rows[i].Cells[7].Value = 0;
                    //    string iid = grdBOMDetail.Rows[i].Cells[1].Value.ToString();
                    //    //iid = iid.Substring(0, iid.IndexOf('-'));
                    //    double BOMCost = bomdb.getBOMCost(iid);
                    //    if (BOMCost == 0)
                    //    {
                    //        //get purchase price from stock table
                    //    }
                    //    else
                    //    {
                    //        grdBOMDetail.Rows[i].Cells[3].Value = BOMCost;
                    //    }
                    //    //MessageBox.Show("Item changed in this row");
                    //}
                    quantity = Convert.ToDouble(grdBOMDetail.Rows[i].Cells["Quantity"].Value);
                    purchaseprice = Convert.ToDouble(grdBOMDetail.Rows[i].Cells["PurchasePrice"].Value);
                    customprice = Convert.ToDouble(grdBOMDetail.Rows[i].Cells["CustomPrice"].Value);
                    if (customprice != 0)
                    {
                        cost = Math.Round(quantity * customprice, 2);
                    }
                    else
                    {
                        cost = 0;
                    }
                    grdBOMDetail.Rows[i].Cells["Value"].Value = cost;
                    totalcost = totalcost + cost;
                }
                txtCost.Text = totalcost.ToString();
                btnProductValue.Text = txtCost.Text;
            }
            catch (Exception ex)
            {
                return false;
            }
            return status;
        }

        private void btnCalculateax_Click(object sender, EventArgs e)
        {
            verifyAndReworkBOMDetailGridRows();
        }

        private void btnClearEntries_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = grdBOMDetail.Rows.Count - 1; i >= 0; i--)
                {
                    grdBOMDetail.Rows.RemoveAt(i);
                }
            }
            catch (Exception)
            {
            }
        }

        private Boolean validateItems(string ProductID)
        {
            Boolean status = true;
            for (int i = 0; i < grdBOMDetail.Rows.Count; i++)
            {
                string tstr = grdBOMDetail.Rows[i].Cells["Item"].Value.ToString().Substring(0, grdBOMDetail.Rows[i].Cells["Item"].Value.ToString().IndexOf('-'));
                if (tstr == ProductID)
                {
                    //same product in BOM
                    MessageBox.Show("BOM product in item list... please correct before saving");
                    return false;
                }
                for (int j = i + 1; j < grdBOMDetail.Rows.Count; j++)
                {
                    if (grdBOMDetail.Rows[i].Cells["Item"].Value.ToString() == grdBOMDetail.Rows[j].Cells["Item"].Value.ToString())
                    {
                        //duplicate item code
                        MessageBox.Show("Item code duplicated in BOM... please correct before saving");
                        return false;
                    }
                }
            }
            return status;
        }



        private void grdBOMDetail_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    int rowIndex = e.RowIndex;
            //    int colIndex = e.ColumnIndex;
            //    if (colIndex == 1 && captureChange)
            //    {
            //        //item changed
            //        grdBOMDetail.Rows[rowIndex].Cells[7].Value = 1;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            //}
        }
        private void btnForward_Click(object sender, EventArgs e)
        {
            //removeControlsFromForwarderPanel();

            //lvApprover.Clear();
            //pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
            //pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
            lvApprover = new ListView();
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

        private void pnlList_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                BOMDB bmdb = new BOMDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevbom.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevbom.CommentStatus);
                    if (bmdb.ApproveBOM(prevbom))
                    {
                        MessageBox.Show("BOM Document Approved");
                        closeAllPanels();
                        listOption = 1;
                        ListBOMHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void btnSelectCommenters_Click(object sender, EventArgs e)
        {
            //removeControlsFromCommenterPanel();


            //lvCmtr.Clear();
            //pnlCmtr.BorderStyle = BorderStyle.FixedSingle;
            //pnlCmtr.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
            lvCmtr = new ListView();
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

            frmPopup.ShowDialog();
            ////lvCancel.Visible = true;

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
                if(lvCmtr.CheckedIndices.Count == 0)
                {
                    MessageBox.Show("Select One commenter");
                    return;
                }
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
                            BOMDB bmdb = new BOMDB();
                            prevbom.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevbom.CommentStatus);
                            prevbom.ForwardUser = approverUID;
                            prevbom.ForwarderList = prevbom.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (bmdb.forwardBOM(prevbom))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                closeAllPanels();
                                listOption = 1;
                                ListBOMHeader(listOption);
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
                    string reverseStr = getReverseString(prevbom.ForwarderList);
                    //do forward activities
                    prevbom.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevbom.CommentStatus);
                    BOMDB bmdb = new BOMDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevbom.ForwarderList = reverseStr.Substring(0, ind);
                        prevbom.ForwardUser = reverseStr.Substring(ind + 3);
                        prevbom.DocumentStatus = prevbom.DocumentStatus - 1;
                    }
                    else
                    {
                        prevbom.ForwarderList = "";
                        prevbom.ForwardUser = "";
                        prevbom.DocumentStatus = 1;
                    }
                    if (bmdb.reverseBOM(prevbom))
                    {
                        MessageBox.Show("BOM Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListBOMHeader(listOption);
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

        private void btnListDocument_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevbom.ProductID);
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
                    string subDir = prevbom.ProductID;
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
        private void btnCloseDocument_Click(object sender, EventArgs e)
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
        private void setButtonVisibility(string btnName)
        {
            try
            {
                pnlAddEdit.Visible = false;
                pnlTopButtons.Visible = true;
                lblActionHeader.Visible = true;
                pnlList.Visible = true;
                grdList.Visible = true;
                lblActionHeader.Visible = true;
                pnlTopButtons.Visible = true;
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
                btnSelectCommenters.Visible = false;
                chkCommentStatus.Visible = false;
                txtComments.Visible = false;
                disableTabPages();
                clearTabPageControls();
                //----24/11/2016
                handleNewButton();
                handleGrdViewButton();
                handleGrdEditButton();
                pnlButtomButtons.Visible = true;
                //----
                if (btnName == "init")
                {
                    ////btnNew.Visible = true; 24/11/2016
                    btnExit.Visible = true;
                    pnlAddEdit.Visible = false;
                }
                else if (btnName == "Commenter")
                {
                    pnlTopButtons.Visible = false;
                    pnlAddEdit.Visible = true;
                    lblActionHeader.Visible = false;
                    grdList.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    pnlPDFViewer.Visible = true;
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    btnSelectCommenters.Visible = false; //earlier Edit enabled this button
                    chkCommentStatus.Visible = true;
                    txtComments.Visible = true;
                    tabControl1.SelectedTab = tabBOMHeader;
                }
                else if (btnName == "btnNew")
                {
                    pnlList.Visible = true;
                    pnlTopButtons.Visible = false;
                    pnlAddEdit.Visible = true;
                    lblActionHeader.Visible = false;
                    grdList.Visible = false;
                    btnNew.Visible = false; //added 24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = false;
                    tabComments.Enabled = false;
                    tabPDFViewer.Enabled = false;
                    tabControl1.SelectedTab = tabBOMHeader;
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
                    pnlAddEdit.Visible = true;
                    lblActionHeader.Visible = false;
                    grdList.Visible = false;
                    pnlButtomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    btnSelectCommenters.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = true;
                    chkCommentStatus.Visible = true;
                    txtComments.Visible = true;
                    tabControl1.SelectedTab = tabBOMHeader;
                }
                else if (btnName == "Approve")
                {
                    pnlTopButtons.Visible = false;
                    pnlAddEdit.Visible = true;
                    lblActionHeader.Visible = false;
                    grdList.Visible = false;
                    pnlButtomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                    disableTabPages();
                    tabControl1.SelectedTab = tabBOMHeader;
                }
                else if (btnName == "View")
                {
                    pnlTopButtons.Visible = false;
                    pnlAddEdit.Visible = true;
                    lblActionHeader.Visible = false;
                    grdList.Visible = false;
                    pnlButtomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabBOMHeader;
                }
                else if (btnName == "LoadDocument")
                {
                    pnlButtomButtons.Visible = false; //24/11/2016
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
                grdBOMDetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }

        private void pnlAddEdit_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            removeControlsFromCommenterPanel();
            removeControlsFromForwarderPanel();
            
        }

        private void btnSelProduct_Click(object sender, EventArgs e)
        {
            showStockItemDataGridView();
        }
        private void showStockItemDataGridView()
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
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInEmpGridList);
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
                string iolist = "";
                var checkedRows = from DataGridViewRow r in grdStock.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("Select Any one Product");
                    return;
                }
                foreach (var row in checkedRows)
                {
                    iolist = row.Cells["StockItemID"].Value.ToString() + "-" + row.Cells["Name"].Value.ToString();
                }
                txtProduct.Text = iolist;
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
            }
            catch (Exception ex)
            {

            }
        }
        private void handlefilterTimerTimeout(object sender, EventArgs e)
        {
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterGridData();
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }
        private void filterGridData()
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

        private void BOM_Enter(object sender, EventArgs e)
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

