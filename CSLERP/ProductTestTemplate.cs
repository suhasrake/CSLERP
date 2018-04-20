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
    public partial class ProductTestTemplate : System.Windows.Forms.Form
    {
        string docID = "PRODUCTTESTTEMPLATE";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        producttesttemplateheader prevsptheader;
        Panel pnlForwarder = new Panel();
        Panel pnlModel = new System.Windows.Forms.Panel();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        TreeView tv = new TreeView();
        Form frmPopup = new Form();
        public ProductTestTemplate()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }

        private void ProductTestTemplate_Load(object sender, EventArgs e)
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
            ListFilteredProdTestTempList(listOption);
            //applyPrivilege();
        }
        private void ListFilteredProdTestTempList(int opt)
        {
            try
            {
                grdList.Rows.Clear();
                SMRNServicedListDB ServListDB = new SMRNServicedListDB();
                List<producttesttemplateheader> PTempList = ProductTestTemplateDB.getFilteredProdTemp(opt);
                if (opt == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (opt == 2)
                    lblActionHeader.Text = "List of Approved Documents";
                else if (opt == 3 || opt == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (producttesttemplateheader ptemp in PTempList)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = ptemp.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["TemplateNo"].Value = ptemp.TemplateNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["TemplateDate"].Value = ptemp.TemplateDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["StockItemID"].Value = ptemp.StockItemID;
                    grdList.Rows[grdList.RowCount - 1].Cells["StockItemName"].Value = ptemp.StockItemName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ModelNo"].Value = ptemp.ModelNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["ModelName"].Value = ptemp.ModelName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = ptemp.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = ptemp.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = ptemp.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = ptemp.CreatorName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Product Test Template Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;

            try
            {
                grdList.Columns["gCreateUser"].Visible = true;
                grdList.Columns["Creator"].Visible = true;
            }
            catch (Exception ex)
            {
            }
        }

        private void initVariables()
        {

            if (getuserPrivilegeStatus() == 1)
            {
                //user is only a viewer
                listOption = 2;
            }
            docID = Main.currentDocument;
            dtTemplateDate.Format = DateTimePickerFormat.Custom;
            dtTemplateDate.CustomFormat = "dd-MM-yyyy";
            dtTemplateDate.Enabled = false;
            ////dtSMRNHeaderDate.Enabled = true;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            ///userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            ///userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
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
                grdProductTestDetail.Rows.Clear();
                //----------clear temperory panels
                clearTabPageControls();
                pnlForwarder.Visible = false;
                dtTemplateDate.Value = DateTime.Parse("01-01-1900");
                txtTemplateNo.Text = "";
                txtStockItemId.Text = "";
                txtModelNo.Text = "";
                txtModelName.Text = "";
                btnSelectStockItemID.Enabled = true;
                prevsptheader = new producttesttemplateheader();
                removeControlsFromForwarderPanelTV();
                removeControlsFromModelPanel();
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
                //btnSelectStockItemID.Enabled = true;
                //grdProductTestDetail.ReadOnly = false;
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
        private void btnClearEntries_Click(object sender, EventArgs e)
        {
            grdProductTestDetail.Rows.Clear();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }
        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                AddProductTestTemDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private Boolean AddProductTestTemDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdProductTestDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkProdDetailGridRows())
                    {
                        return false;
                    }
                }
                grdProductTestDetail.Rows.Add();
                int kount = grdProductTestDetail.RowCount;
                grdProductTestDetail.Rows[kount - 1].Cells["LineNo"].Value = kount;
                grdProductTestDetail.Rows[kount - 1].Cells["SINo"].Value = kount;
                grdProductTestDetail.Rows[kount - 1].Cells["TestDescriptionID"].Value = "";
                grdProductTestDetail.Rows[kount - 1].Cells["ExpectedResult"].Value = "";
                grdProductTestDetail.Rows[kount - 1].Cells["Remark"].Value = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddProductTestTemDetailRow() : Error");
            }

            return status;
        }

        private Boolean verifyAndReworkProdDetailGridRows()
        {
            Boolean status = true;

            try
            {
                if (grdProductTestDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Product test template details");
                    return false;
                }
                for (int i = 0; i < grdProductTestDetail.Rows.Count; i++)
                {

                    grdProductTestDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if ((grdProductTestDetail.Rows[i].Cells["TestDescriptionID"].ToString().Trim().Length == 0) ||
                        (grdProductTestDetail.Rows[i].Cells["ExpectedResult"].ToString().Trim().Length == 0) ||
                        (grdProductTestDetail.Rows[i].Cells["Remark"].Value.ToString().Trim().Length == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return status;
        }
        private List<producttesttemplatedetail> getProdTempDetails(producttesttemplateheader pttemph)
        {
            List<producttesttemplatedetail> PTTempDetails = new List<producttesttemplatedetail>();
            try
            {
                producttesttemplatedetail pttempd = new producttesttemplatedetail();
                for (int i = 0; i < grdProductTestDetail.Rows.Count; i++)
                {
                    pttempd = new producttesttemplatedetail();
                    pttempd.DocumentID = pttemph.DocumentID;
                    pttempd.TemplateNo = pttemph.TemplateNo;
                    pttempd.TemplateDate = pttemph.TemplateDate;
                    pttempd.TestDescriptionID = grdProductTestDetail.Rows[i].Cells["TestDescriptionID"].Value.ToString();
                    pttempd.ExpectedResult = grdProductTestDetail.Rows[i].Cells["ExpectedResult"].Value.ToString();
                    pttempd.SlNo = Convert.ToInt32(grdProductTestDetail.Rows[i].Cells["SINo"].Value.ToString());
                    pttempd.Remark = grdProductTestDetail.Rows[i].Cells["Remark"].Value.ToString();
                    PTTempDetails.Add(pttempd);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("getProdTempDetails() : Error getting ProductTemp Details");
                PTTempDetails = null;
            }
            return PTTempDetails;
        }
        private void grdProductTestDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdProductTestDetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdProductTestDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkProdDetailGridRows();
                    }
                    if (columnName.Equals("Select"))
                    {
                        ShowTestDescriptionIDListView();
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
        private void ShowTestDescriptionIDListView()
        {
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
            lv = ProductTestDescriptionDB.getTestDescriptionListView();
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView3_ItemChecked);
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
                        if (validateItems(itemRow.SubItems[1].Text))
                        {
                            grdProductTestDetail.CurrentRow.Cells["TestDescriptionID"].Value = itemRow.SubItems[1].Text;
                            grdProductTestDetail.CurrentRow.Cells["TestDescription"].Value = itemRow.SubItems[2].Text;
                        }
                    }
                }

                frmPopup.Close();
                frmPopup.Dispose();
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
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        //-----
        //private void listView3_ItemChecked(object sender, ItemCheckedEventArgs e)
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
        private Boolean validateItems(string item)
        {
            Boolean status = true;
            try
            {
                foreach (DataGridViewRow row in grdProductTestDetail.Rows)
                {
                    if (grdProductTestDetail.Rows[row.Index].Cells["TestDescriptionID"].Value.ToString() == item)
                    {
                        MessageBox.Show("Test Duplication Not allowed");
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            Boolean status = true;
            try
            {
                ProductTestTemplateDB PTTempDB = new ProductTestTemplateDB();
                producttesttemplateheader ptlist = new producttesttemplateheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;
                try
                {
                    ptlist.DocumentID = docID;
                    ptlist.StockItemID = txtStockItemId.Text.Substring(0, txtStockItemId.Text.IndexOf('-'));
                    ptlist.ModelNo = txtModelNo.Text;
                    ptlist.ModelName = txtModelName.Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    // ptlist.TemplateNo = DocumentNumberDB.getNewNumber(docID, 1);
                    ptlist.TemplateDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    ptlist.TemplateNo = Convert.ToInt32(txtTemplateNo.Text);
                    ptlist.TemplateDate = prevsptheader.TemplateDate;
                }
                if (!PTTempDB.validateProductTestTemp(ptlist))
                {
                    MessageBox.Show("Validation Failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    List<producttesttemplatedetail> PTTempDetails = getProdTempDetails(ptlist);
                    if (PTTempDB.InsertPTHeaderAndDetail(ptlist, PTTempDetails))
                    {
                        MessageBox.Show("Product Test Template Details Added");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredProdTestTempList(listOption);
                    }
                    else
                    {
                        MessageBox.Show("Failed to Insert Product Test Template Details");
                        status = false;
                    }

                }
                else if (btnText.Equals("Update"))
                {
                    List<producttesttemplatedetail> PTTempDetails = getProdTempDetails(ptlist);
                    if (PTTempDB.updatePTHeaderAndDetail(ptlist, PTTempDetails))
                    {
                        MessageBox.Show("Product Test Template updated");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredProdTestTempList(listOption);
                    }
                    else
                    {
                        status = false;
                        MessageBox.Show("Failed to update Product Test Template Details");
                    }
                }
                else
                {
                    MessageBox.Show("Validation failed");
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(" Validation failed");
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
                if (columnName.Equals("Finalize") || columnName.Equals("Edit") || columnName.Equals("View") || columnName.Equals("LoadDocument"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevsptheader = new producttesttemplateheader();
                    prevsptheader.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    prevsptheader.TemplateNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["TemplateNo"].Value.ToString());
                    prevsptheader.TemplateDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["TemplateDate"].Value.ToString());
                    prevsptheader.StockItemID = grdList.Rows[e.RowIndex].Cells["StockItemID"].Value.ToString();
                    prevsptheader.StockItemName = grdList.Rows[e.RowIndex].Cells["StockItemName"].Value.ToString();
                    prevsptheader.ModelNo = grdList.Rows[e.RowIndex].Cells["ModelNo"].Value.ToString();
                    prevsptheader.ModelName = grdList.Rows[e.RowIndex].Cells["ModelName"].Value.ToString();
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Document Template No:" + prevsptheader.TemplateNo + "\n" +
                            "Template Date:" + prevsptheader.TemplateDate.ToString("dd-MM-yyyy");
                        //poheader po =PurchaseOrderDB.getPODetailForMR(prevpoh.PONo, prevpoh.PODate);
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevsptheader.TemplateNo + "-" + prevsptheader.TemplateDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_2(null, null);
                        return;
                    }
                    txtTemplateNo.Text = prevsptheader.TemplateNo.ToString();
                    txtStockItemId.Text = prevsptheader.StockItemID + "-" + prevsptheader.StockItemName;
                    txtModelName.Text = prevsptheader.ModelName;
                    txtModelNo.Text = prevsptheader.ModelNo;
                    //+ "-" +prevsptheader.StockItemName;
                    dtTemplateDate.Value = prevsptheader.TemplateDate;
                    List<producttesttemplatedetail> pttempd = ProductTestTemplateDB.getProductTestTempDetail(prevsptheader);
                    grdProductTestDetail.Rows.Clear();
                    int i = 0;
                    foreach (producttesttemplatedetail pttempdetail in pttempd)
                    {
                        if (!AddProductTestTemDetailRow())
                        {
                            MessageBox.Show("Error found in Prod Temp Detail. Please correct before updating the details");
                        }
                        else
                        {
                            grdProductTestDetail.Rows[i].Cells["SINo"].Value = pttempdetail.SlNo;
                            grdProductTestDetail.Rows[i].Cells["TestDescriptionID"].Value = pttempdetail.TestDescriptionID;
                            grdProductTestDetail.Rows[i].Cells["TestDescription"].Value = pttempdetail.TestDescription;
                            grdProductTestDetail.Rows[i].Cells["ExpectedResult"].Value = pttempdetail.ExpectedResult;
                            grdProductTestDetail.Rows[i].Cells["Remark"].Value = pttempdetail.Remark;
                            i++;
                        }
                    }
                    if (!verifyAndReworkProdDetailGridRows())
                    {
                        MessageBox.Show("Error found in Product Templete details. Please correct before updating the details");
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
                ProductTestTemplateDB ProdTempDB = new ProductTestTemplateDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Finalize the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (ProdTempDB.FinalizeProductTestTemp(prevsptheader))
                    {
                        MessageBox.Show("ProductTest Template List Finalized");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredProdTestTempList(listOption);
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
                //btnDelete.Visible = false;
                btnApproved.Visible = true;
                btnNew.Visible = false;
                btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                btnFinalize.Visible = false;
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
                else if (btnName == "btnNew")
                {
                    btnNew.Visible = false; //added 24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = false; ;
                    tabPDFViewer.Enabled = false;
                    tabControl1.SelectedTab = tabProductTestHeader;
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
                    disableTabPages();
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabProductTestHeader;
                }
                else if (btnName == "LoadDocument")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                }
                else if (btnName == "Edit")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabProductTestHeader;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabProductTestHeader;
                }
                pnlEditButtons.Refresh();
                //if the user privilege is only view, show only the Approved documents button
                int ups = getuserPrivilegeStatus();
                if (ups == 1 || ups == 0)
                {
                    grdList.Columns["Finalize"].Visible = false;
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
        void handleNewButton()
        {
            btnNew.Visible = false;
            if (Main.itemPriv[1])
            {
                btnNew.Visible = true;
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
        void handleGrdEditButton()
        {
            grdList.Columns["Edit"].Visible = false;
            grdList.Columns["Finalize"].Visible = false;
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                if (listOption == 1)
                {
                    grdList.Columns["Edit"].Visible = true;
                    grdList.Columns["Finalize"].Visible = true;
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
                    string subDir = prevsptheader.TemplateNo + "-" + prevsptheader.TemplateDate.ToString("yyyyMMddhhmmss");
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
            ListFilteredProdTestTempList(listOption);
        }

        private void btnApproved_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredProdTestTempList(listOption);
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
        private void btnSelectSMRNHeaderNo_Click(object sender, EventArgs e)
        {
            if (grdProductTestDetail.Rows.Count != 0)
            {
                DialogResult dialog = MessageBox.Show("Grid Detail will be removed", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    grdProductTestDetail.Rows.Clear();
                }
                else
                    return;
            }
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

                    txtStockItemId.Text = s;
                    tv.CheckBoxes = true;
                    pnlForwarder.Controls.Remove(lvApprover);
                    pnlForwarder.Visible = false;
                    showModelListView(s);
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
        private void showModelListView(string stockID)
        {
            //removeControlsFromModelPanel();
            //lv = new ListView();
            //lv.CheckBoxes = true;
            //lv.Items.Clear();
            //pnlModel.BorderStyle = BorderStyle.FixedSingle;
            //pnlModel.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
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
            lbl.ForeColor = Color.Green;
            frmPopup.Controls.Add(lbl);
            lv = ProductModelsDB.getModelsForProductListView(stockID.Substring(0, stockID.IndexOf('-')));
            if (lv.Items.Count == 0)
            {
                txtModelNo.Text = "NA";
                txtModelName.Text = "NA";
                pnlModel.Visible = false;
                return;
            }
            lv.Bounds = new Rectangle(new Point(0, 25), new Size(450, 250));
            //pnlModel.Controls.Remove(lv);
            frmPopup.Controls.Add(lv);
            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.BackColor = Color.Tan;
            lvOK.Location = new Point(40, 280);
            lvOK.Click += new System.EventHandler(this.lvOK_Click3);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new Point(130, 280);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click3);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
            //pnlModel.Visible = true;
            //pnlAddEdit.Controls.Add(pnlModel);
            //pnlAddEdit.BringToFront();
            //pnlModel.BringToFront();
            //pnlModel.Focus();
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
                        string id = txtStockItemId.Text.Substring(0, txtStockItemId.Text.IndexOf('-'));
                        if (ProductTestTemplateDB.checkProductAvailability(id, item.SubItems[1].Text))
                        {
                            txtModelNo.Text = item.SubItems[1].Text;
                            txtModelName.Text = item.SubItems[2].Text;
                            frmPopup.Close();
                            frmPopup.Dispose();
                        }
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
        private void btnListDocuments_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevsptheader.TemplateNo + "-" + prevsptheader.TemplateDate.ToString("yyyyMMddhhmmss"));
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
        private void tabPSReport_Click(object sender, EventArgs e)
        {

        }

        private void tabProductTestDetail_Click(object sender, EventArgs e)
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
            removeControlsFromForwarderPanelTV();
            removeControlsFromModelPanel();
            //removeControlsFromPnlLvPanel();
        }

        private void ProductTestTemplate_Enter(object sender, EventArgs e)
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

