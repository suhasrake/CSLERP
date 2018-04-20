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
    public partial class StockOBProcess : System.Windows.Forms.Form
    {
        string docID = "PURCHASEORDER";
        Form frmPopup = new Form();
        string mainTabName = "";
        Form dtpForm = new Form();
        DataGridView grdCustList = new DataGridView();
        CheckedListBox chkBoxCustomer = new CheckedListBox();
        ListView exlv = new ListView();
        ListView lv = new ListView();
        TextBox txtSearchStock = new TextBox();
        TextBox txtSearchStocksubgrd = new TextBox();
        Timer filterTimer1 = new Timer();
        Timer filterTimer = new Timer();
        DataGridView grdStock = new DataGridView();
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Dictionary<string, string[]> dictItemWiseTOt = new Dictionary<string, string[]>();
        List<int> TrackModifiedList = new List<int>();
        List<stock> StockobList = new List<stock>();
        TextBox txtSearch = new TextBox();
        public StockOBProcess()
        {
            try
            {
                InitializeComponent();
                Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
                initVariables();
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                this.Width -= 100;
                this.Height -= 100;
                String a = this.Size.ToString();
                grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdList.EnableHeadersVisualStyles = false;
                grdSummary.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdSummary.EnableHeadersVisualStyles = false;
                grdList.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        private void ListFilteredPOHeader()
        {
            try
            {
                grdList.Rows.Clear();
                StockDB stockdb = new StockDB();
                string tablename = "";
                string TofyID = cmbToFYID.SelectedItem.ToString().Substring(0, cmbToFYID.SelectedItem.ToString().IndexOf(':')).Trim();
                tablename = "StockOB" + TofyID.Replace('-', '_');
                mainTabName = tablename;
                StockobList.Clear();
                StockobList = StockDB.getStockDetailFromStockOBTable(tablename);
                foreach (stock stk in StockobList)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["RowID"].Value = stk.RowID;
                    grdList.Rows[grdList.RowCount - 1].Cells["StockItemID"].Value = stk.StockItemID;
                    grdList.Rows[grdList.RowCount - 1].Cells["StockItemName"].Value = stk.StockItemName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ModelNo"].Value = stk.ModelNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["ModelName"].Value = stk.ModelName;
                    grdList.Rows[grdList.RowCount - 1].Cells["InwardDocID"].Value = stk.InwardDocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["InwardDocNo"].Value = stk.InwardDocumentNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["InwardDocDate"].Value = stk.InwardDocumentDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["InwardQuantity"].Value = stk.InwardQuantity;
                    grdList.Rows[grdList.RowCount - 1].Cells["PresentStock"].Value = stk.PresentStock;
                    grdList.Rows[grdList.RowCount - 1].Cells["StockOnHold"].Value = stk.StockOnHold;
                    grdList.Rows[grdList.RowCount - 1].Cells["PurchaseReturnQuantity"].Value = stk.PurchaseReturnQuantity;
                    grdList.Rows[grdList.RowCount - 1].Cells["MRNNo"].Value = stk.MRNNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["MRNDate"].Value = stk.MRNDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["BatchNo"].Value = stk.BatchNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["SerialNo"].Value = stk.SerielNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["ExpiryDate"].Value = stk.ExpiryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["Unit"].Value = stk.StockItemUnit;
                    grdList.Rows[grdList.RowCount - 1].Cells["PurchaseQuantity"].Value = stk.PurchaseQuantity;
                    grdList.Rows[grdList.RowCount - 1].Cells["PurchasePrice"].Value = stk.PurchasePrice;
                    grdList.Rows[grdList.RowCount - 1].Cells["PurchaseTax"].Value = stk.PurchaseTax;
                    grdList.Rows[grdList.RowCount - 1].Cells["SupplierID"].Value = stk.SupplierID;
                    grdList.Rows[grdList.RowCount - 1].Cells["StoreLocation"].Value = stk.StoreLocation;
                    grdList.Rows[grdList.RowCount - 1].Cells["IssueQuantity"].Value = stk.IssueQuantity;
                    grdList.Rows[grdList.RowCount - 1].Cells["CBValue"].Value = stk.PresentStock*stk.PurchasePrice;
                    grdList.Rows[grdList.RowCount - 1].Cells["OBValue"].Value = stk.InwardQuantity * stk.PurchasePrice;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Stock OB Listing");
            }
        }
        private void setTabIndex()
        {
            cmbFromFYID.TabIndex = 0;
            cmbToFYID.TabIndex = 1;

        }
        private void initVariables()
        {

            if (getuserPrivilegeStatus() == 1)
            {
                //user is only a viewer
                listOption = 6;
            }
            docID = Main.currentDocument;
            FinancialYearDB.fillFYIDComboNew(cmbFromFYID);
            FinancialYearDB.fillFYIDComboNew(cmbToFYID);
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("init");
            TrackModifiedList.Clear();
            setTabIndex();
        }
        private void closeAllPanels()
        {
            try
            {
                pnlCreateOB.Visible = false;
                pnlCreateAddStock.Visible = false;
                pnlReport.Visible = false;
                pnlUpdateOB.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        public void clearData()
        {
            try
            {
                grdList.Rows.Clear();
                TrackModifiedList.Clear();
                cmbFromFYID.SelectedIndex = -1;
                cmbToFYID.SelectedIndex = -1;
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
        private void pnlAddEdit_Paint(object sender, PaintEventArgs e)
        {

        }


        private void btnApproved_Click(object sender, EventArgs e)
        {
            try
            {
                if (!validateFinancialYear())
                {
                    MessageBox.Show("Check Finanacial year");
                    return;
                }
                StockDB stkOB = new StockDB();
                string TofyID = cmbToFYID.SelectedItem.ToString().Substring(0, cmbToFYID.SelectedItem.ToString().IndexOf(':')).Trim();
                if (!stkOB.CheckStockOBTableAvail(TofyID))
                {
                    MessageBox.Show("OB Not prepared for selected financial year.");
                    return;
                }

                listOption = 3;
                setButtonVisibility("Report");
               
                ListFilteredPOHeader();
                ListFilteredSummaryreprot();
            }
            catch (Exception ex)
            {
            }
        }
        private void ListFilteredSummaryreprot()
        {
            try
            {
                grdSummary.Rows.Clear();
                string tablename = "";
                string TofyID = cmbToFYID.SelectedItem.ToString().Substring(0, cmbToFYID.SelectedItem.ToString().IndexOf(':')).Trim();
                tablename = "StockOB" + TofyID.Replace('-', '_');
                mainTabName = tablename;
                List<ClosingStock> StockobList = StockDB.getStockDetailgroupbyitem(tablename);
                foreach (ClosingStock stk in StockobList)
                {
                    grdSummary.Rows.Add();
                    grdSummary.Rows[grdSummary.RowCount - 1].Cells["LineNo"].Value = grdSummary.RowCount;
                    grdSummary.Rows[grdSummary.RowCount - 1].Cells["gStockItemID"].Value = stk.StockItemID;
                    grdSummary.Rows[grdSummary.RowCount - 1].Cells["gStockItemName"].Value = stk.StockItemName;
                    grdSummary.Rows[grdSummary.RowCount - 1].Cells["gOBQuantity"].Value = stk.OBQty;
                    grdSummary.Rows[grdSummary.RowCount - 1].Cells["gOBValue"].Value = stk.OBValue;
                    grdSummary.Rows[grdSummary.RowCount - 1].Cells["gCBQuantity"].Value = stk.CBQty;
                    grdSummary.Rows[grdSummary.RowCount - 1].Cells["gCBValue"].Value = stk.CBValue;
                }
                filterSummary();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Stock OB Listing");
            }
        }

        private void filterSummary()
        {
            double obval = 0, cbval = 0;
            for (int i=0; i<grdSummary.RowCount;i++)
            {
                if (rbOnlyOB.Checked)
                {
                    if (Convert.ToDouble(grdSummary.Rows[i].Cells["gOBQuantity"].Value) <= 0)
                    {
                        grdSummary.Rows[i].Visible = false;
                    }
                    else
                    {
                        obval = obval + Convert.ToDouble(grdSummary.Rows[i].Cells["gOBValue"].Value);
                        cbval = cbval + Convert.ToDouble(grdSummary.Rows[i].Cells["gCBValue"].Value);
                    }
                }
                else
                {
                    grdSummary.Rows[i].Visible = true;
                    obval = obval + Convert.ToDouble(grdSummary.Rows[i].Cells["gOBValue"].Value);
                    cbval = cbval + Convert.ToDouble(grdSummary.Rows[i].Cells["gCBValue"].Value);
                }
            }
            lblOBValue.Text = "OB Value : " + obval.ToString("N2");
            lblCBValue.Text = "CB Value : " + cbval.ToString("N2");
            ////grdSummary.Rows[grdSummary.Rows.Count-1]
        }
        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            try
            {
                if (!validateFinancialYear())
                {
                    MessageBox.Show("Check Finanacial year");
                    return;
                }
                StockDB stkOB = new StockDB();
                string tablename = "";
                string TofyID = cmbToFYID.SelectedItem.ToString().Substring(0, cmbToFYID.SelectedItem.ToString().IndexOf(':')).Trim();
                if (!stkOB.CheckStockOBTableAvail(TofyID))
                {
                    MessageBox.Show("OB Not prepared for selected financial year.");
                    return;
                }
                tablename = "StockOB" + TofyID.Replace('-', '_');
                mainTabName = tablename;

                listOption = 2;
                setButtonVisibility("Add/Modify");
               
              

                
                StockobList.Clear();
                grdModifyStockOB.Rows.Clear();
                txtStockItemID.Text = "";
                txtStockItemName.Text = "";
                StockobList = StockDB.getStockDetailFromStockOBTable(tablename);
                //ListFilteredPOHeader();
                //grdList.Columns["InwardQuantity"].ReadOnly = false;
                //grdList.Columns["sel"].Visible = true;
            }
            catch (Exception ex)
            {
            }
        }
        private Boolean validateFinancialYear()
        {
            Boolean status = true;
            try
            {
                string FromfyID = cmbFromFYID.SelectedItem.ToString().Substring(0, cmbFromFYID.SelectedItem.ToString().IndexOf(':')).Trim();
                string TofyID = cmbToFYID.SelectedItem.ToString().Substring(0, cmbToFYID.SelectedItem.ToString().IndexOf(':')).Trim();

                StockDB stkOB = new StockDB();

                string strt = FromfyID.Substring(0, FromfyID.IndexOf('-'));
                string end = TofyID.Substring(0, TofyID.IndexOf('-'));
                if ((Convert.ToInt32(strt) > Convert.ToInt32(end)) || (Convert.ToInt32(end) - Convert.ToInt32(strt) != 1))
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return status;
        }
        private void btnActionPending_Click(object sender, EventArgs e)
        {
            try
            {
                listOption = 1;
                TrackModifiedList.Clear();
                setButtonVisibility("CreateOB");
            }
            catch (Exception ex)
            {
            }

        }


        private void cmbTaxCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            //verifyAndReworkPODetailGridRows();
        }


        //-------

        private void setButtonVisibility(string btnName)
        {
            try
            {
                btnCreateOB.Visible = true;
                btnCreateOrAddStock.Visible = true;
                btnReport.Visible = true;
                btnUpdateOB.Visible = true;
                btnExit.Visible = true;
                closeAllPanels();
                grdList.Visible = false;
                pnlBottomButtons.Visible = true;
                grdList.Rows.Clear();
                pnlList.Visible = true;
                TrackModifiedList.Clear();
                if (btnName == "init")
                {
                    pnlCreateOB.Visible = true;

                }
                else if (btnName == "CreateOB")
                {
                    pnlCreateOB.Visible = true;
                }
                else if (btnName == "Add/Modify")
                {
                    pnlCreateAddStock.Visible = true;
                    pnlCreateAddStock.Location = pnlCreateOB.Location;
                }
                else if (btnName == "Report")
                {
                    pnlReport.Visible = true;
                    pnlReport.Location = pnlCreateOB.Location;
                    //grdList.Location = new Point(30, 77);
                    grdList.Visible = true;
                    tabReport.SelectedIndex = 0;
                }
                else if (btnName == "UpdateOB")
                {
                    pnlUpdateOB.Visible = true;
                    pnlUpdateOB.Location = pnlCreateOB.Location;

                    //grdList.Location = new Point(30, 77);
                    //grdList.Visible = true;
                    //grdList.BringToFront();
                }
                changeListOptColor();
            }
            catch (Exception ex)
            {
            }
        }
        void changeListOptColor()
        {
            try
            {
                btnCreateOB.UseVisualStyleBackColor = true;
                btnReport.UseVisualStyleBackColor = true;
                btnCreateOrAddStock.UseVisualStyleBackColor = true;
                btnUpdateOB.UseVisualStyleBackColor = true;
                switch (listOption)
                {
                    case 1:
                        btnCreateOB.BackColor = Color.MediumAquamarine;
                        break;
                    case 2:
                        btnCreateOrAddStock.BackColor = Color.MediumAquamarine;
                        break;
                    case 3:
                        btnReport.BackColor = Color.MediumAquamarine;
                        break;
                    case 4:
                        btnUpdateOB.BackColor = Color.MediumAquamarine;
                        break;
                    default:
                        break;

                }
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

        private void PurchaseOrder_Enter(object sender, EventArgs e)
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

        private void btnUpdateOB_Click(object sender, EventArgs e)
        {
            try
            {
                if (!validateFinancialYear())
                {
                    MessageBox.Show("Check Finanacial year");
                    return;
                }
                StockDB stkOB = new StockDB();
                string TofyID = cmbToFYID.SelectedItem.ToString().Substring(0, cmbToFYID.SelectedItem.ToString().IndexOf(':')).Trim();

                lblUpdateOBInStock.Text = "Update OB in stock for Year : " + TofyID;
                if (!stkOB.CheckStockOBTableAvail(TofyID))
                {
                    MessageBox.Show("OB Not prepared for selected financial year.");
                    return;
                }

                listOption = 4;
                setButtonVisibility("UpdateOB");
               
                ///ListFilteredPOHeader();
                //grdList.Columns["InwardQuantity"].ReadOnly = true;
                ///grdList.Columns["sel"].Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string FromfyID = cmbFromFYID.SelectedItem.ToString().Substring(0, cmbFromFYID.SelectedItem.ToString().IndexOf(':')).Trim();
                string TofyID = cmbToFYID.SelectedItem.ToString().Substring(0, cmbToFYID.SelectedItem.ToString().IndexOf(':')).Trim();

                StockDB stkOB = new StockDB();

                string strt = FromfyID.Substring(0, FromfyID.IndexOf('-'));
                string end = TofyID.Substring(0, TofyID.IndexOf('-'));
                if ((Convert.ToInt32(strt) > Convert.ToInt32(end)) || (Convert.ToInt32(end) - Convert.ToInt32(strt) != 1))
                {
                    MessageBox.Show("Check Financial Year");
                    return;
                }
                int opt = 1;
                if (stkOB.CheckStockOBTableAvail(TofyID))
                {
                    DialogResult dialog = MessageBox.Show("Stock OB for selected Financial year is available. \nAre you sure to recreate?", "Alert", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.No)
                    {
                        return;
                    }
                    else
                    {
                        opt = 2;
                    }
                }
                //opt 1: Not created, opt 2: already created
                if (stkOB.createStockOB(FromfyID, TofyID, opt))
                {
                    MessageBox.Show("Stock OB created sucessfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exeption while creating Stock OB");
            }
        }
        private Boolean verifyAndReworkPOPIDetailGridRows()
        {
            Boolean status = true;
            try
            {
                for (int i = 0; i < grdModifyStockOB.Rows.Count; i++)
                {
                    if (grdModifyStockOB.Rows[i].Cells["mStockItemID"].Value == null ||
                        grdModifyStockOB.Rows[i].Cells["mStockItemID"].Value.ToString().Trim().Length == 0)
                    {
                        MessageBox.Show("Fill StockItem ID in row " + (i + 1));
                        return false;
                    }
                    if(Convert.ToInt32(grdModifyStockOB.Rows[i].Cells["mRowID"].Value) == 0)
                    {
                        if (grdModifyStockOB.Rows[i].Cells["mInwardQuantity"].Value == null || 
                             grdModifyStockOB.Rows[i].Cells["mInwardQuantity"].Value.ToString().Trim().Length == 0 ||
                            Convert.ToDouble(grdModifyStockOB.Rows[i].Cells["mInwardQuantity"].Value) == 0)
                        {
                            MessageBox.Show("Fill Quantity in row " + (i + 1));
                            return false;
                        }
                        if (grdModifyStockOB.Rows[i].Cells["mPurchasePrice"].Value == null || 
                            grdModifyStockOB.Rows[i].Cells["mPurchasePrice"].Value.ToString().Trim().Length == 0 ||
                           Convert.ToDouble(grdModifyStockOB.Rows[i].Cells["mPurchasePrice"].Value) == 0)
                        {
                            MessageBox.Show("Fill Purchase Price in row " + (i + 1));
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return status;
        }
        private void btnAddNewLine_Click(object sender, EventArgs e)
        {
            try
            {
                addModifiedgrdiLine(2);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddNewLine() : Error");
            }
        }
        private void addModifiedgrdiLine(int opt)
        {
            try
            {
                if (grdModifyStockOB.Rows.Count > 0)
                {
                    if (!verifyAndReworkPOPIDetailGridRows())
                    {
                        return;
                    }
                }
                grdModifyStockOB.Rows.Add();
                grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mRowID"].Value = Convert.ToInt32(0);
                grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mStockItemID"].Value = txtStockItemID.Text;
                grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mStockItemName"].Value = txtStockItemName.Text;
                grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mModelNo"].Value = "NA";
                grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mModelName"].Value = "NA";
                grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mInwardQuantity"].Value = Convert.ToDouble(0);

                grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mPurchasePrice"].Value = Convert.ToDouble(0);
                grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mBatchNo"].Value = "";
                grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mSerialNo"].Value = "";
                grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mExpiryDate"].Value = DateTime.Parse("1900-01-01");
                grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mSupplierID"].Value = "";
                //grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mStoreLocation"].Value = "";
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mStoreLocation"]);
                CatalogueValueDB.fillCatalogValueGridViewComboNew(ComboColumn2, "StoreLocation");
                grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mStoreLocation"].Value = Main.MainStore;

                grdModifyStockOB.FirstDisplayedScrollingRowIndex = grdModifyStockOB.RowCount - 1;
                grdModifyStockOB.CurrentCell = grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells[1];
                if(opt == 2)
                {
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mPurchasePrice"].ReadOnly = false;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mBatchNo"].ReadOnly = false;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mSerialNo"].ReadOnly = false;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mStoreLocation"].ReadOnly = false;
                }
                else
                {
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mPurchasePrice"].ReadOnly = true;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mBatchNo"].ReadOnly = true;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mSerialNo"].ReadOnly = true;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mStoreLocation"].ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddNewLine() : Error");
            }
        }
        private void btnExportToExcel_Click(object sender, EventArgs e)
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

                frmPopup.Size = new Size(450, 310);
                exlv = Utilities.GridColumnSelectionView(grdList);
                exlv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 25), new System.Drawing.Size(450, 250));
                frmPopup.Controls.Add(exlv);

                System.Windows.Forms.Label pnlHeading = new System.Windows.Forms.Label();
                pnlHeading.Size = new Size(300, 20);
                pnlHeading.Location = new System.Drawing.Point(5, 5);
                pnlHeading.Text = "Select Gridview Colums to Export";
                pnlHeading.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                pnlHeading.ForeColor = Color.Black;
                frmPopup.Controls.Add(pnlHeading);

                System.Windows.Forms.Button exlvOK = new System.Windows.Forms.Button();
                exlvOK.Text = "OK";
                exlvOK.BackColor = Color.Tan;
                exlvOK.Location = new System.Drawing.Point(40, 280);
                exlvOK.Click += new System.EventHandler(this.exlvOK_Click1);
                frmPopup.Controls.Add(exlvOK);

                System.Windows.Forms.Button exlvCancel = new System.Windows.Forms.Button();
                exlvCancel.Text = "CANCEL";
                exlvCancel.BackColor = Color.Tan;
                exlvCancel.Location = new System.Drawing.Point(130, 280);
                exlvCancel.Click += new System.EventHandler(this.exlvCancel_Click2);
                frmPopup.Controls.Add(exlvCancel);

                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void exlvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        private void exlvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                string heading1 = "Stock OB ";
                string heading2 = "Financial Year - " + cmbToFYID.SelectedItem.ToString().Trim().Substring(0, cmbToFYID.SelectedItem.ToString().Trim().IndexOf(':')).Trim();
                Utilities.export2Excel(heading1, heading2, "", grdList, exlv);
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export to Excell error");
            }
        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    if (e.RowIndex < 0)
            //        return;
            //    string columnName = grdList.Columns[e.ColumnIndex].Name;
            //    if (columnName.Equals("Sel"))
            //    {
            //        showStockDataGridView();
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
        }
        private void showStockDataGridView()
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

                txtSearchStock = new TextBox();
                txtSearchStock.Size = new Size(200, 18);
                txtSearchStock.Location = new System.Drawing.Point(680, 3);
                txtSearchStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearchStock.ForeColor = Color.Black;
                txtSearchStock.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInEmpGridList);
                txtSearchStock.TabIndex = 0;
                txtSearchStock.Focus();
                frmPopup.Controls.Add(txtSearchStock);

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
                    txtStockItemID.Text = row.Cells["StockItemID"].Value.ToString();
                    txtStockItemName.Text = row.Cells["Name"].Value.ToString();
                }
                frmPopup.Close();
                frmPopup.Dispose();
                AddItemDetailTOModifyGrid(txtStockItemID.Text.Trim());
                //showModelListView(iolist);
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
        private void txtSearch_TextChangedInEmpGridList(object sender, EventArgs e)
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
            filterGridData1();
        }

        private void filterGridData1()
        {
            try
            {
                grdStock.CurrentCell = null;
                foreach (DataGridViewRow row in grdStock.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearchStock.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdStock.Rows)
                    {
                        if (!row.Cells["Name"].Value.ToString().Trim().ToLower().Contains(txtSearchStock.Text.Trim().ToLower()))
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
        private void AddItemDetailTOModifyGrid(string itemid)
        {
            try
            {
                grdModifyStockOB.Rows.Clear();
                List<stock> filterdOBList = StockobList.Where(stk => stk.StockItemID == itemid ).ToList();
                filterdOBList = filterdOBList.OrderBy(stk => stk.RowID).ToList();

                foreach (stock stk in filterdOBList)
                {
                    addModifiedgrdiLine(1);
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mRowID"].Value = stk.RowID;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mStockItemID"].Value = stk.StockItemID;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mStockItemName"].Value = stk.StockItemName;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mModelNo"].Value = stk.ModelNo;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mModelName"].Value = stk.ModelName;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mInwardQuantity"].Value = stk.InwardQuantity;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mcbquantity"].Value = stk.PresentStock;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mPurchasePrice"].Value = stk.PurchasePrice;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mBatchNo"].Value = stk.BatchNo;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mSerialNo"].Value = stk.SerielNo;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mMRNNo"].Value = stk.MRNNo;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mMRNDate"].Value = stk.MRNDate.ToString("dd-MM-yyyy");
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mExpiryDate"].Value = stk.ExpiryDate;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mSupplierID"].Value = stk.SupplierID;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["msuppliername"].Value = stk.SupplierName;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["mStoreLocation"].Value = stk.StoreLocation;
                    grdModifyStockOB.Rows[grdModifyStockOB.RowCount - 1].Cells["msuppliername"].Value = stk.SupplierName;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void showModelListView(string stockID)
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
                    grdModifyStockOB.CurrentRow.Cells["mModelNo"].Value = "NA";
                    grdModifyStockOB.CurrentRow.Cells["mModelName"].Value = "NA";
                    return;
                }
                lv.Bounds = new Rectangle(new Point(0, 25), new Size(450, 250));
                //pnlModel.Controls.Remove(lv);
                frmPopup.Controls.Add(lv);
                Button lvOK = new Button();
                lvOK.Text = "OK";
                lvOK.BackColor = Color.Tan;
                lvOK.Location = new Point(40, 280);
                lvOK.Click += new System.EventHandler(this.lvOK_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new Point(130, 280);
                lvCancel.Click += new System.EventHandler(this.lvCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void lvOK_Click1(object sender, EventArgs e)
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
                        grdModifyStockOB.CurrentRow.Cells["mModelNo"].Value = item.SubItems[1].Text;
                        grdModifyStockOB.CurrentRow.Cells["mModelName"].Value = item.SubItems[2].Text;
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

        private void UpdateNewAddedStk_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdModifyStockOB.Rows.Count == 0)
                {
                    MessageBox.Show("No Data Found");
                    return;
                }
                if (!verifyAndReworkPOPIDetailGridRows())
                {
                    return;
                }
                if (txtStockItemID.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Select stock item id.");
                    return;
                }
                List<stock> stockList = new List<stock>();
                stock stk = new stock();
                foreach (DataGridViewRow row in grdModifyStockOB.Rows)
                {
                    stk = new stock();
                    int rowid = Convert.ToInt32(row.Cells["mRowiD"].Value);
                    if (rowid == 0 || TrackModifiedList.Contains(rowid)) //rowid 0 : New stock added, rowid found in modified list
                    {
                        stk.FYID = cmbToFYID.SelectedItem.ToString().Substring(0, cmbToFYID.SelectedItem.ToString().IndexOf(':')).Trim();
                        stk.RowID = Convert.ToInt32(row.Cells["mRowiD"].Value);
                        stk.StockItemID = row.Cells["mStockItemID"].Value.ToString();
                        stk.ModelNo = row.Cells["mModelNo"].Value.ToString();
                        stk.InwardDocumentID = "";
                        stk.InwardDocumentNo = "";
                        stk.InwardDocumentDate = DateTime.Parse("1900-01-01");
                        stk.MRNNo = 0;
                        stk.PresentStock = 0;
                        stk.InwardQuantity = Convert.ToDouble(row.Cells["mInwardQuantity"].Value.ToString());
                        stk.MRNDate = DateTime.Parse("1900-01-01");
                        stk.ExpiryDate = Convert.ToDateTime(row.Cells["mExpiryDate"].Value);
                        stk.BatchNo = row.Cells["mBatchNo"].Value.ToString();
                        stk.SerielNo = row.Cells["mSerialNo"].Value.ToString();
                        stk.PurchaseQuantity = Convert.ToDouble(0);
                        stk.PurchasePrice = Convert.ToDouble(row.Cells["mPurchasePrice"].Value.ToString());
                        stk.PurchaseTax = Convert.ToDouble(0);
                        stk.SupplierID = row.Cells["mSupplierID"].Value.ToString();
                        try
                        {
                            stk.StoreLocation = row.Cells["mStoreLocation"].Value.ToString();
                        }
                        catch (Exception ex)
                        {
                            stk.StoreLocation = "";
                        }
                        stockList.Add(stk);
                    }
                }
                if (stockList.Count == 0)
                {
                    MessageBox.Show("No Data modified.");
                    return;
                }
                if (StockDB.updateStockOB(stockList, mainTabName))
                {
                    MessageBox.Show("Stock OB updated sucessfully");
                    grdModifyStockOB.Rows.Clear();
                    txtStockItemID.Text = "";
                    txtStockItemName.Text = "";
                    StockobList = StockDB.getStockDetailFromStockOBTable(mainTabName);
                }
                else
                {
                    MessageBox.Show("Failed to update Stock OB");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update Stock OB");
            }
        }

        private void btnDiscardAddedStk_Click(object sender, EventArgs e)
        {
            try
            {
                grdModifyStockOB.Rows.Clear();
                txtStockItemID.Text = "";
                txtStockItemName.Text = "";
                StockobList = StockDB.getStockDetailFromStockOBTable(mainTabName);
            }
            catch (Exception ex)
            {

            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!validateFinancialYear())
                {
                    MessageBox.Show("Check Finanacial year");
                    return;
                }
                
                string[] str = cmbToFYID.SelectedItem.ToString().Split(':');
                string s = str[0];
                string ss = str[1];
                string sss = str[2];
                if (StockDB.CheckStockOBAvailability(s.Trim()))
                {
                    MessageBox.Show("OB already available in stock for this financial year.");
                    return;
                }
                DateTime FYstartDate = Convert.ToDateTime(Utilities.convertDateStringToAnsi( ss.Trim()));
                DateTime FYEndDate = Convert.ToDateTime(Utilities.convertDateStringToAnsi(sss.Trim()));
                List<stock> stockList = new List<stock>();
                string tablename = "";
                string TofyID = cmbToFYID.SelectedItem.ToString().Substring(0, cmbToFYID.SelectedItem.ToString().IndexOf(':')).Trim();
                StockDB stkOB = new StockDB();
                if (!stkOB.CheckStockOBTableAvail(TofyID))
                {
                    MessageBox.Show("OB Not prepared for selected financial year.");
                    return;
                }
                tablename = "StockOB" + TofyID.Replace('-', '_');
                mainTabName = tablename;
                StockobList.Clear();
                stockList = StockDB.getStockDetailFromStockOBTable(tablename).Where(stk => stk.InwardQuantity != 0).ToList();
                if (stockList.Count == 0)
                {
                    MessageBox.Show("OB Not prepared for this financial year.");
                    return;
                }
                DialogResult dialog = MessageBox.Show("Are you sure to Update OB In stock ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.No)
                {
                    return;
                }
                if (StockDB.insertStockFromStockOB(stockList, FYstartDate))
                {
                    MessageBox.Show("Stock updated sucessfully");
                }
                else
                {
                    MessageBox.Show("Failed to update Stock");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update Stock");
            }
        }

        private void btnSelectStockItem_Click(object sender, EventArgs e)
        {
            try
            {
                TrackModifiedList.Clear();
                showStockDataGridView();
            }
            catch (Exception ex)
            {
            }
        }

        private void grdModifyStockOB_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdModifyStockOB.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Sel"))
                {
                    if (Convert.ToInt32(grdModifyStockOB.Rows[e.RowIndex].Cells["mRowID"].Value) == 0)
                    {
                        showStockDataGridViewForGrid();
                    }
                }
                if (columnName.Equals("SelSup"))
                {
                    if (Convert.ToInt32(grdModifyStockOB.Rows[e.RowIndex].Cells["mRowID"].Value) == 0)
                    {
                        showSupplierGridView();
                    }
                }
                if (columnName.Equals("D1"))
                {
                    if (Convert.ToInt32(grdModifyStockOB.Rows[e.RowIndex].Cells["mRowID"].Value) == 0)
                    {
                        DateTime dt = DateTime.Today;
                        dt = Convert.ToDateTime(grdModifyStockOB.Rows[e.RowIndex].Cells["mExpiryDate"].Value);
                        Rectangle tempRect = grdModifyStockOB.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                        showDtPickerForm(Cursor.Position.X, Cursor.Position.Y, tempRect.Location, dt);
                    }
                    else
                    {
                        MessageBox.Show("Cannot change the supplier of an existing row.");
                    }
                }
                if (columnName.Equals("Del"))
                {
                    if (Convert.ToInt32(grdModifyStockOB.Rows[e.RowIndex].Cells["mRowID"].Value) == 0)
                    {
                        grdModifyStockOB.Rows.RemoveAt(e.RowIndex);
                    }
                    else
                    {
                        MessageBox.Show("Cannot delete an existing row. Instead, change the quantity to zero");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void showSupplierGridView()
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

                txtSearch = new TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(460, 36);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtCustSearch_TextChangedInEmpGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                CustomerDB custDB = new CustomerDB();
                grdCustList = custDB.getGridViewForCustomerListNew("Customer");

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
                foreach (var row in checkedRows)
                {
                    grdModifyStockOB.CurrentRow.Cells["msuppliername"].Value = row.Cells["Name"].Value.ToString();
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
        private void chkBoxCustomer_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                string checkedStr = "";
                txtSearch.Text = "";
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
            dt.CustomFormat = "dd-MM-yyyy";
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
        }
        void cellDateTimePickerValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            grdModifyStockOB.Rows[grdModifyStockOB.CurrentCell.RowIndex].Cells[grdModifyStockOB.CurrentCell.ColumnIndex - 1].Value = dtp.Value;
        }
        private void showStockDataGridViewForGrid()
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

                txtSearchStocksubgrd = new TextBox();
                txtSearchStocksubgrd.Size = new Size(200, 18);
                txtSearchStocksubgrd.Location = new System.Drawing.Point(680, 3);
                txtSearchStocksubgrd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearchStocksubgrd.ForeColor = Color.Black;
                txtSearchStocksubgrd.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInEmpGridListsubgrd);
                txtSearchStocksubgrd.TabIndex = 0;
                txtSearchStocksubgrd.Focus();
                frmPopup.Controls.Add(txtSearchStocksubgrd);

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
                lvOK.Click += new System.EventHandler(this.grdStkOK_Click1grd);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 335);
                lvCancel.Click += new System.EventHandler(this.grdStkCancel_Click1grd);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdStkOK_Click1grd(object sender, EventArgs e)
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
                    grdModifyStockOB.CurrentRow.Cells["mStockItemID"].Value = row.Cells["StockItemID"].Value.ToString();
                    grdModifyStockOB.CurrentRow.Cells["mStockItemName"].Value = row.Cells["Name"].Value.ToString();
                }
                frmPopup.Close();
                frmPopup.Dispose();
                //AddItemDetailTOModifyGrid(txtStockItemID.Text.Trim);
                showModelListView(iolist);
            }
            catch (Exception)
            {
            }
        }
        private void grdStkCancel_Click1grd(object sender, EventArgs e)
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
        private void txtSearch_TextChangedInEmpGridListsubgrd(object sender, EventArgs e)
        {
            try
            {
                filterTimer.Enabled = false;
                filterTimer.Stop();
                filterTimer.Tick -= new System.EventHandler(this.handlefilterTimerTimeoutSubGrd);
                filterTimer.Tick += new System.EventHandler(this.handlefilterTimerTimeoutSubGrd);
                filterTimer.Interval = 500;
                filterTimer.Enabled = true;
                filterTimer.Start();

            }
            catch (Exception ex)
            {

            }
        }
        private void handlefilterTimerTimeoutSubGrd(object sender, EventArgs e)
        {
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterGridDataSubGrd();
        }

        private void filterGridDataSubGrd()
        {
            try
            {
                grdStock.CurrentCell = null;
                foreach (DataGridViewRow row in grdStock.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearchStocksubgrd.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdStock.Rows)
                    {
                        if (!row.Cells["Name"].Value.ToString().Trim().ToLower().Contains(txtSearchStocksubgrd.Text.Trim().ToLower()))
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

        private void grdModifyStockOB_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (grdModifyStockOB.IsCurrentCellDirty)
                {
                    if (grdModifyStockOB.Columns[e.ColumnIndex].Name == "mInwardQuantity" || grdModifyStockOB.Columns[e.ColumnIndex].Name == "mStockItemID")
                    {
                        int rowid = Convert.ToInt32(grdModifyStockOB.CurrentRow.Cells["mRowID"].Value);
                        if (!TrackModifiedList.Contains(rowid))
                            TrackModifiedList.Add(rowid);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnExportToExcelSum_Click(object sender, EventArgs e)
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

                frmPopup.Size = new Size(450, 310);
                exlv = Utilities.GridColumnSelectionView(grdSummary);
                exlv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 25), new System.Drawing.Size(450, 250));
                frmPopup.Controls.Add(exlv);

                System.Windows.Forms.Label pnlHeading = new System.Windows.Forms.Label();
                pnlHeading.Size = new Size(300, 20);
                pnlHeading.Location = new System.Drawing.Point(5, 5);
                pnlHeading.Text = "Select Gridview Colums to Export";
                pnlHeading.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                pnlHeading.ForeColor = Color.Black;
                frmPopup.Controls.Add(pnlHeading);

                System.Windows.Forms.Button exlvOK = new System.Windows.Forms.Button();
                exlvOK.Text = "OK";
                exlvOK.BackColor = Color.Tan;
                exlvOK.Location = new System.Drawing.Point(40, 280);
                exlvOK.Click += new System.EventHandler(this.exlvOK_Clicksum);
                frmPopup.Controls.Add(exlvOK);

                System.Windows.Forms.Button exlvCancel = new System.Windows.Forms.Button();
                exlvCancel.Text = "CANCEL";
                exlvCancel.BackColor = Color.Tan;
                exlvCancel.Location = new System.Drawing.Point(130, 280);
                exlvCancel.Click += new System.EventHandler(this.exlvCancel_Clicksum);
                frmPopup.Controls.Add(exlvCancel);

                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void exlvCancel_Clicksum(object sender, EventArgs e)
        {
            try
            {
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        private void exlvOK_Clicksum(object sender, EventArgs e)
        {
            try
            {
                string heading1 = "Stock OB (Itemwise Detail)";
                string heading2 = "Financial Year - " + cmbToFYID.SelectedItem.ToString().Trim().Substring(0, cmbToFYID.SelectedItem.ToString().Trim().IndexOf(':')).Trim();

                Utilities.export2Excel(heading1, heading2, "", grdSummary, exlv);
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export to Excell error");
            }
        }

        private void rbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAll.Checked)
                rbOnlyOB.Checked = false;
            else
                rbOnlyOB.Checked = true;
            filterSummary();
        }

        private void rbOnlyOB_CheckedChanged(object sender, EventArgs e)
        {
            if (rbOnlyOB.Checked)
                rbAll.Checked = false;
            else
                rbAll.Checked = true;
            filterSummary();
        }
    }

}

