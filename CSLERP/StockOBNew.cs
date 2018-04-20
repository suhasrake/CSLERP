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
using CSLERP.FileManager;

namespace CSLERP
{
    public partial class StockOBNew : System.Windows.Forms.Form
    {
        string docID = "STOCKOB";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        stockObNewHeader prevsboh;
        Panel pnlForwarder = new Panel();
        Panel pnlModel = new System.Windows.Forms.Panel();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        System.Windows.Forms.TextBox txtSearch;
        ListView lvCopy = new ListView();
        ListView exlv = new ListView();
        Form frmPopup = new Form();
        public StockOBNew()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void StockOBNew_Load(object sender, EventArgs e)
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
            FilteredStockOBList(listOption);
            //applyPrivilege();
        }
        private void FilteredStockOBList(int opt)
        {
            try
            {
                grdList.Rows.Clear();
                StockOBNewDB AccDB = new StockOBNewDB();
                List<stockObNewHeader> SBList = StockOBNewDB.getStockOblist(opt);
                if (opt == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                //else if (opt == 2)
                //    lblActionHeader.Text = "List of Approved Documents";
                else if (opt == 2 || opt == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (stockObNewHeader sobh in SBList)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentID"].Value = sobh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentNo"].Value = sobh.DocumentNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentName"].Value = sobh.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentDate"].Value = sobh.DocumentDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["FYID"].Value = sobh.FYID;
                    grdList.Rows[grdList.RowCount - 1].Cells["StoreLocation"].Value = sobh.StoreLocation;
                    grdList.Rows[grdList.RowCount - 1].Cells["Value"].Value = sobh.Value;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = sobh.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatus"].Value = sobh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = sobh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = sobh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = sobh.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Remarks"].Value = sobh.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["TransferStatus"].Value = sobh.Transferstatus;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in StockOB Listing");
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
                listOption = 6;
            }
            docID = Main.currentDocument;
            dtDocDate.Format = DateTimePickerFormat.Custom;
            dtDocDate.CustomFormat = "dd-MM-yyyy";
            dtDocDate.Enabled = false;
            FinancialYearDB.fillFYIDCombo(cmbFYID);
            StoreEmpMappingDB.fillLocationComboNew(cmbStoreLocation);
            ////dtSMRNHeaderDate.Enabled = true;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            setButtonVisibility("init");
        }
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
                removeControlsFromModelPanel();
                removeControlsFromPnlLvPanel();
                //pnlAddEdit.Controls.Remove(pnllv);
                txtDocNo.Text = "";
                txtRemarks.Text = "";
                txtcredittotal.Text = "";
                lbltotal.Visible = false;
                dtDocDate.Value = DateTime.Parse("01-01-1900");
                cmbFYID.SelectedIndex = -1;
                cmbStoreLocation.SelectedIndex = -1;
                grdStockOBDetail.Rows.Clear();
                prevsboh = new stockObNewHeader();
              
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
                //closeAllPanels();
                clearData();
                btnSave.Text = "Save";
                pnlAddEdit.Visible = true;
                closeAllPanels();
                lbltotal.Visible = false;
                txtcredittotal.Visible = false;
                pnlAddEdit.Visible = true;
                cmbFYID.SelectedValue = -1;
                setButtonVisibility("btnNew");
            }
            catch (Exception)
            {

            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }
        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Finalize") || columnName.Equals("Edit") || columnName.Equals("View"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevsboh = new stockObNewHeader();
                    prevsboh.DocumentID = grdList.Rows[e.RowIndex].Cells["DocumentID"].Value.ToString();
                    prevsboh.DocumentNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentNo"].Value.ToString());
                    prevsboh.DocumentDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["DocumentDate"].Value.ToString());
                    prevsboh.FYID = grdList.Rows[e.RowIndex].Cells["FYID"].Value.ToString();
                    prevsboh.StoreLocation = grdList.Rows[e.RowIndex].Cells["StoreLocation"].Value.ToString();

                    prevsboh.Value = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["Value"].Value.ToString());
                    prevsboh.Remarks = grdList.Rows[e.RowIndex].Cells["Remarks"].Value.ToString();
                    txtDocNo.Text = prevsboh.DocumentNo.ToString();
                    dtDocDate.Value = prevsboh.DocumentDate;
                    cmbFYID.SelectedIndex = cmbFYID.FindString(prevsboh.FYID);

                    cmbStoreLocation.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbStoreLocation, prevsboh.StoreLocation);
                    //cmbStoreLocation.SelectedIndex = cmbStoreLocation.FindString(prevsboh.StoreLocation);
                    txtRemarks.Text = prevsboh.Remarks;
                    dtDocDate.Value = prevsboh.DocumentDate;
                    //txtcredittotal.Text = prevsboh.Value.ToString();
                    List<stockObNewDetail> sobdetail = StockOBNewDB.getstockObNewDetail(prevsboh);
                    grdStockOBDetail.Rows.Clear();
                    int i = 0;
                    double dd = 0;
                    foreach (stockObNewDetail sobd in sobdetail)
                    {
                        if (!AddStockOBDetailRow())
                        {
                            MessageBox.Show("Error found in stock Detail. Please correct before updating the details");
                        }
                        else
                        {
                            grdStockOBDetail.Rows[i].Cells["LineNo"].Value = i + 1;
                            grdStockOBDetail.Rows[i].Cells["item"].Value = sobd.StockItemID;
                            grdStockOBDetail.Rows[i].Cells["Description"].Value = sobd.StockItemName;
                            grdStockOBDetail.Rows[i].Cells["ModelNo"].Value = sobd.ModelNo;
                            grdStockOBDetail.Rows[i].Cells["ModelName"].Value = sobd.ModelName;
                            grdStockOBDetail.Rows[i].Cells["Quantity"].Value = sobd.Quantity;
                            grdStockOBDetail.Rows[i].Cells["PurchasePrice"].Value = sobd.Price;
                            grdStockOBDetail.Rows[i].Cells["ItemValue"].Value = sobd.Quantity * sobd.Price;

                            dd = dd + Convert.ToDouble(grdStockOBDetail.Rows[i].Cells["ItemValue"].Value);
                            i++;
                        }

                    }
                    txtcredittotal.Text = dd.ToString(); ;
                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    txtcredittotal.Visible = true;
                    lbltotal.Visible = true;
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
        private void tabStockOB_Click(object sender, EventArgs e)
        {

        }
        private void setButtonVisibility(string btnName)
        {
            try
            {
                btnActionPending.Visible = true;
                btnApproved.Visible = true;
                btnNew.Visible = false;
                btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                btnFinalize.Visible = false;
                disableTabPages();
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
                    tabControl1.SelectedTab = tabStockOB;
                }
                else if (btnName == "btnEditPanel") //called from cancel,save,forward,approve and reverse button events
                {
                    ////btnNew.Visible = true;
                    btnExit.Visible = true;
                }
                else if (btnName == "Finalize")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnFinalize.Visible = true;
                    //disableTabPages();
                    tabControl1.SelectedTab = tabStockOB;
                }
                else if (btnName == "View")
                {
                    disableTabPages();
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                }
                else if (btnName == "Edit")
                {
                    //cmbFYID.Enabled = false;
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    enableTabPages();
                }
                pnlEditButtons.Refresh();
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
        void handletab()
        {
            //cmbFYID.Enabled = true;
            btnAddLine.Enabled = true;
            txtcredittotal.Enabled = true;
            //txtdebittotal.Enabled = true;
            grdStockOBDetail.Enabled = true;
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

        private void btnAddLine_Click_1(object sender, EventArgs e)
        {

            try
            {
                AddStockOBDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private Boolean AddStockOBDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdStockOBDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkAccGridRows())
                    {
                        return false;
                    }
                }
                grdStockOBDetail.Rows.Add();
                int kount = grdStockOBDetail.RowCount;
                grdStockOBDetail.Rows[kount - 1].Cells["LineNo"].Value = kount;
                grdStockOBDetail.Rows[kount - 1].Cells["Item"].Value = "";
                grdStockOBDetail.Rows[kount - 1].Cells["Description"].Value = "";
                grdStockOBDetail.Rows[kount - 1].Cells["ModelNo"].Value = "";
                grdStockOBDetail.Rows[kount - 1].Cells["Quantity"].Value = 0;
                grdStockOBDetail.Rows[kount - 1].Cells["PurchasePrice"].Value = 0;
                grdStockOBDetail.Rows[kount - 1].Cells["ItemValue"].Value = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddStockDetailRow() : Error");
            }

            return status;
        }
        private Boolean verifyAndReworkAccGridRows()
        {
            double Total = 0;
            Boolean status = true;
            try
            {
                //int ir = grdStockOBDetail.Rows.Count;
                if (grdStockOBDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Account details");
                    lbltotal.Visible = false;
                    txtcredittotal.Visible = false;
                    return false;
                }

                for (int i = 0; i < grdStockOBDetail.Rows.Count; i++)
                {
                    //if (((Convert.ToDecimal(grdStockOBDetail.Rows[i].Cells["AccBalanceDebit"].Value) != 0) &&
                    //    (Convert.ToDecimal(grdStockOBDetail.Rows[i].Cells["AccBalanceCredit"].Value) != 0)) ||
                    //        ((Convert.ToDecimal(grdStockOBDetail.Rows[i].Cells["AccBalanceDebit"].Value) == 0) &&
                    //    (Convert.ToDecimal(grdStockOBDetail.Rows[i].Cells["AccBalanceCredit"].Value) == 0)))

                    //{
                    //    MessageBox.Show("Enter either Debrit or Credit Row:" + (i + 1));
                    //    return false;
                    //}
                    grdStockOBDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if ((grdStockOBDetail.Rows[i].Cells["Item"].Value.ToString().Trim().Length == 0) ||
                        (grdStockOBDetail.Rows[i].Cells["Description"].Value.ToString().Trim().Length == 0) ||
                        (grdStockOBDetail.Rows[i].Cells["ModelNo"].Value.ToString().Trim().Length == 0) ||
                        (grdStockOBDetail.Rows[i].Cells["Quantity"].Value.ToString().Trim().Length == 0) ||
                        (Convert.ToDouble(grdStockOBDetail.Rows[i].Cells["Quantity"].Value) == 0) ||
                        (grdStockOBDetail.Rows[i].Cells["PurchasePrice"].Value.ToString().Trim().Length == 0) ||
                             (Convert.ToDouble(grdStockOBDetail.Rows[i].Cells["PurchasePrice"].Value) == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                    double dd = Convert.ToDouble(grdStockOBDetail.Rows[i].Cells["Quantity"].Value);
                    double dd1 = Convert.ToDouble(grdStockOBDetail.Rows[i].Cells["PurchasePrice"].Value);
                    grdStockOBDetail.Rows[i].Cells["ItemValue"].Value = dd * dd1;
                    Total = Total + (Convert.ToDouble(grdStockOBDetail.Rows[i].Cells["ItemValue"].Value));
                }
                txtcredittotal.Text = Total.ToString();
                lbltotal.Visible = true;
                txtcredittotal.Visible = true;

            }

            catch (Exception ex)
            {
                return false;
            }
            return status;
        }
        private List<stockObNewDetail> getStockOBDetails(stockObNewHeader sobh)
        {
            List<stockObNewDetail> SNDetails = new List<stockObNewDetail>();
            try
            {
                stockObNewDetail sondb = new stockObNewDetail();
                for (int i = 0; i < grdStockOBDetail.Rows.Count; i++)
                {
                    sondb = new stockObNewDetail();
                    sondb.DocumentID = sobh.DocumentID;
                    sondb.DocumentNo = sobh.DocumentNo;
                    sondb.DocumentDate = sobh.DocumentDate;
                    sondb.StockItemID = grdStockOBDetail.Rows[i].Cells["Item"].Value.ToString();
                    sondb.ModelNo = grdStockOBDetail.Rows[i].Cells["ModelNo"].Value.ToString();
                    sondb.Quantity = Convert.ToInt32(grdStockOBDetail.Rows[i].Cells["Quantity"].Value.ToString());
                    sondb.Price = Convert.ToDouble(grdStockOBDetail.Rows[i].Cells["PurchasePrice"].Value.ToString());
                    SNDetails.Add(sondb);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("getStockOBDetails() : Error getting StockOb Details");
                SNDetails = null;
            }
            return SNDetails;
        }

        private void grdStockOBDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdStockOBDetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("AccDelete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdStockOBDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkAccGridRows();
                    }
                    if (columnName.Equals("Select"))
                    {
                        ShowStockListView();
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
        private void removeControlsFromPnlLvPanel()
        {
            try
            {
                //foreach (Control p in pnllv.Controls)
                //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(System.Windows.Forms.Button))
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
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            frmPopup.Controls.Remove(lvCopy);
            addItems();
        }
        //private void CopylistView2_ItemChecked(object sender, ItemCheckedEventArgs e)
        //{
        //    try
        //    {
        //        if (lvCopy.CheckedIndices.Count > 1)
        //        {
        //            MessageBox.Show("Cannot select more than one item");
        //            e.Item.Checked = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        private void addItems()
        {
            lvCopy = new ListView();
            lvCopy.View = System.Windows.Forms.View.Details;
            lvCopy.LabelEdit = true;
            lvCopy.AllowColumnReorder = true;
            lvCopy.CheckBoxes = true;
            lvCopy.FullRowSelect = true;
            lvCopy.GridLines = true;
            lvCopy.Sorting = System.Windows.Forms.SortOrder.Ascending;
            lvCopy.Columns.Add("Select", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Id", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Name", -2, HorizontalAlignment.Left);
            lvCopy.Sorting = System.Windows.Forms.SortOrder.None;
            lvCopy.ColumnClick += new ColumnClickEventHandler(LvColumnClick); ;
            //this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
            lvCopy.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new Size(450, 250));
            lvCopy.Items.Clear();
            foreach (ListViewItem row in lv.Items)
            {
                string x = row.SubItems[0].Text;
                string no = row.SubItems[1].Text;
                string ch = row.SubItems[2].Text;
                if (ch.ToLower().StartsWith(txtSearch.Text))
                {
                    ListViewItem item = new ListViewItem();
                    item.SubItems.Add(no);
                    item.SubItems.Add(ch);
                    item.Checked = false;
                    lvCopy.Items.Add(item);
                }
            }
            lv.Visible = false;
            lvCopy.Visible = true;
            frmPopup.Controls.Add(lvCopy);
        }

        private void ShowStockListView()
        {
            //removeControlsFromPnlLvPanel();
            //pnllv = new Panel();
            //pnllv.BorderStyle = BorderStyle.FixedSingle;
            //pnllv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(100, 100), new Size(600, 300));
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            lv = StockItemDB.getStockItemListView();
            lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView3_ItemChecked);
            lv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 0), new Size(450, 250));
            frmPopup.Controls.Add(lv);

            System.Windows.Forms.Button lvOK = new System.Windows.Forms.Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(44, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Click2);
            frmPopup.Controls.Add(lvOK);

            System.Windows.Forms.Button lvCancel = new System.Windows.Forms.Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new Point(141, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
            frmPopup.Controls.Add(lvCancel);

            System.Windows.Forms.Label lblSearch = new System.Windows.Forms.Label();
            lblSearch.Text = "Search";
            lblSearch.Location = new Point(250, 267);
            lblSearch.Size = new Size(45, 15);
            frmPopup.Controls.Add(lblSearch);

            txtSearch = new System.Windows.Forms.TextBox();
            txtSearch.Location = new Point(300, 265);
            txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            frmPopup.Controls.Add(txtSearch);

            frmPopup.ShowDialog();
            //pnlAddEdit.Controls.Add(pnllv);
            //pnllv.BringToFront();
            //pnllv.Visible = true;
        }
        private void lvOK_Click2(object sender, EventArgs e)
        {
            try
            {

                //btnSelect.Enabled = true;
                //pnlEditButtons.Enabled = true;
                if (lv.Visible == true)
                {
                    if (!checkLVItemChecked("Item"))
                    {
                        return;
                    }
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            grdStockOBDetail.CurrentRow.Cells["Item"].Value = itemRow.SubItems[1].Text;
                            grdStockOBDetail.CurrentRow.Cells["Description"].Value = itemRow.SubItems[2].Text;
                            frmPopup.Close();
                            frmPopup.Dispose();
                            showModelListView(itemRow.SubItems[1].Text);
                        }
                    }
                }
                else
                {
                    if (!checkLVCopyItemChecked("Item"))
                    {
                        return;
                    }
                    foreach (ListViewItem itemRow in lvCopy.Items)
                    {
                        if (itemRow.Checked)
                        {
                            grdStockOBDetail.CurrentRow.Cells["Item"].Value = itemRow.SubItems[1].Text;
                            grdStockOBDetail.CurrentRow.Cells["Description"].Value = itemRow.SubItems[2].Text;
                            frmPopup.Close();
                            frmPopup.Dispose();
                            showModelListView(itemRow.SubItems[1].Text);
                        }
                    }

                }
                //frmPopup.Close();
                //frmPopup.Dispose();
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
        private void LvColumnClick(object o, ColumnClickEventArgs e)
        {
            try
            {
                if (lv.Visible == true)
                {
                    string first = lv.Items[0].SubItems[e.Column].Text;
                    string last = lv.Items[lv.Items.Count - 1].SubItems[e.Column].Text;
                    System.Windows.Forms.SortOrder sort_order1 = SortingListView.getSortedOrder(first, last);
                    this.lv.ListViewItemSorter = new SortingListView(e.Column, sort_order1);
                }
                else
                {
                    string first = lvCopy.Items[0].SubItems[e.Column].Text;
                    string last = lvCopy.Items[lvCopy.Items.Count - 1].SubItems[e.Column].Text;
                    System.Windows.Forms.SortOrder sort_order1 = SortingListView.getSortedOrder(first, last);
                    this.lvCopy.ListViewItemSorter = new SortingListView(e.Column, sort_order1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorting error");
            }
        }
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
            lv = ProductModelsDB.getModelsForProductListView(stockID);
            if (lv.Items.Count == 0)
            {
                grdStockOBDetail.CurrentRow.Cells["ModelNo"].Value = "NA";
                grdStockOBDetail.CurrentRow.Cells["ModelName"].Value = "NA";
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
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click1);
            frmPopup.Controls.Add(lvCancel);

            frmPopup.ShowDialog();
            ////lvForwardCancel.Visible = false;
            //tv.CheckBoxes = true;
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
                        grdStockOBDetail.CurrentRow.Cells["ModelNo"].Value = item.SubItems[1].Text;
                        grdStockOBDetail.CurrentRow.Cells["ModelName"].Value = item.SubItems[2].Text;
                        frmPopup.Close();
                        frmPopup.Dispose();
                    }
                }
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
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {
                StockOBNewDB sobDB = new StockOBNewDB();
                stockObNewHeader sobh = new stockObNewHeader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;
                if (!verifyAndReworkAccGridRows())
                {
                    MessageBox.Show("Error found in StockOB details. Please correct before updating the details");
                    return;
                }
                try
                {
                    sobh.DocumentID = docID;
                    sobh.FYID = cmbFYID.SelectedItem.ToString().Trim().Substring(0, cmbFYID.SelectedItem.ToString().Trim().IndexOf(':')).Trim();
                    //sobh.StoreLocation = cmbStoreLocation.SelectedItem.ToString().Substring(0, cmbStoreLocation.SelectedItem.ToString().IndexOf('-'));
                    sobh.StoreLocation = ((Structures.ComboBoxItem)cmbStoreLocation.SelectedItem).HiddenValue;
                    sobh.Remarks = txtRemarks.Text;
                    sobh.Value = Convert.ToDouble(txtcredittotal.Text);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    status = false;
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    // sobh.DocumentNo = DocumentNumberDB.getNewNumber(docID, 1);
                    sobh.DocumentStatus = 1;
                    sobh.DocumentDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    sobh.DocumentNo = Convert.ToInt32(txtDocNo.Text);
                    sobh.DocumentStatus = prevsboh.DocumentStatus;
                    sobh.DocumentDate = prevsboh.DocumentDate;
                }
                if (!sobDB.validateStockOB(sobh))
                {
                    MessageBox.Show("Validation Failed");
                    status = false;
                    return;
                }

                if (btnText.Equals("Save"))
                {
                    if (!sobDB.verifyFYLoc(sobh.FYID, sobh.StoreLocation))
                    {
                        MessageBox.Show("For this Financial Year and Location Document already Available.Reenter not allow.");
                        return;
                    }
                    List<stockObNewDetail> StockList = getStockOBDetails(sobh);
                    if (sobDB.InsertStockHeaderAndDetail(sobh, StockList))
                    {
                        MessageBox.Show("Details Added");
                        closeAllPanels();
                        listOption = 1;
                        FilteredStockOBList(listOption);
                        pnlAddEdit.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert StockOB Detail");
                        status = false;
                    }
                    if (!status)
                    {
                        MessageBox.Show("Failed to Insert");
                        status = false;
                    }
                }
                else if (btnText.Equals("Update"))
                {
                    List<stockObNewDetail> StockList = getStockOBDetails(sobh);
                    if (sobDB.updateStockHeaderAndDetail(sobh, StockList))
                    {
                        MessageBox.Show(" details updated");
                        closeAllPanels();
                        listOption = 1;
                        FilteredStockOBList(listOption);
                    }

                    else
                    {
                        status = false;

                    }
                    if (!status)
                    {
                        MessageBox.Show("Failed to update");
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
                MessageBox.Show("Validation failed");
                status = false;
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }

        private void btnFinalize_Click(object sender, EventArgs e)
        {
            try
            {
                StockOBNewDB sobdb = new StockOBNewDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Finalize the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (sobdb.FinalizeStockOB(prevsboh))
                    {
                        MessageBox.Show(" Document Finalized");
                        closeAllPanels();
                        listOption = 1;
                        FilteredStockOBList(listOption);
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

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            FilteredStockOBList(listOption);
        }

        private void btnApproved_Click(object sender, EventArgs e)
        {
            listOption = 2;
            FilteredStockOBList(listOption);
        }

        private void btnClearEntries_Click(object sender, EventArgs e)
        {
            grdStockOBDetail.Rows.Clear();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            verifyAndReworkAccGridRows();
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
        private Boolean checkLVCopyItemChecked(string str)
        {
            Boolean status = true;
            try
            {
                if (lvCopy.CheckedIndices.Count > 1)
                {
                    MessageBox.Show("Only one " + str + " allowed");
                    return false;
                }
                if (lvCopy.CheckedItems.Count == 0)
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

        private void StockOBNew_Enter(object sender, EventArgs e)
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

