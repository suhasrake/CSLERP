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
    public partial class StockReport : System.Windows.Forms.Form
    {
        Form frmPopup = new Form();
        ListView exlv = new ListView();// list view for choice / selection list
        Timer filterTimer = new Timer();
        public StockReport()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void CompanyData_Load(object sender, EventArgs e)
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
            //Listdata();
            applyPrivilege();
        }
        private void ListFilteredStock(int opt,string locID)
        {
            try
            {
                grdList.Rows.Clear();
                StockDB sdb = new StockDB();
                List<stock> stockList = StockDB.getStockDetailForReport(opt, locID);
                double total = 0;
                foreach (stock st in stockList)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["StockItemID"].Value = st.StockItemID;
                    grdList.Rows[grdList.RowCount - 1].Cells["StockItemName"].Value = st.StockItemName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ModelNo"].Value = st.ModelNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["ModelName"].Value = st.ModelName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Unit"].Value = st.StockItemUnit;
                    grdList.Rows[grdList.RowCount - 1].Cells["InwardDocID"].Value = st.InwardDocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["InwardDocNo"].Value = st.InwardDocumentNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["InwardDocDate"].Value = st.InwardDocumentDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["StoreLocation"].Value = st.SupplierName; // For Store LOCation name
                    grdList.Rows[grdList.RowCount - 1].Cells["Quantity"].Value = st.InwardQuantity;
                    grdList.Rows[grdList.RowCount - 1].Cells["PresentStock"].Value = st.PresentStock;
                    grdList.Rows[grdList.RowCount - 1].Cells["StockOnHold"].Value = st.StockOnHold;
                    grdList.Rows[grdList.RowCount - 1].Cells["PurchasePrice"].Value = st.PurchasePrice;
                    grdList.Rows[grdList.RowCount - 1].Cells["Value"].Value = st.PresentStock * st.PurchasePrice;

                    grdList.Rows[grdList.RowCount - 1].Cells["G1Code"].Value = st.Level1GCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["G2Code"].Value = st.Level2GCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["G3Code"].Value = st.Level3GCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["G1Name"].Value = st.Level1GDescription;
                    grdList.Rows[grdList.RowCount - 1].Cells["G2Name"].Value = st.Level2GDescription;
                    grdList.Rows[grdList.RowCount - 1].Cells["G3Name"].Value = st.Level3GDescription;

                    total = total + (st.PresentStock * st.PurchasePrice);
                }
                btnTotalValue.Text = total.ToString();
                if (grdList.Rows.Count != 0 && Main.itemPriv[1] == true && (Main.itemPriv[2] == true || Main.itemPriv[3] == true ))
                    btnExportToExcel.Visible = true;
                else
                    btnExportToExcel.Visible = false;
                grdList.Visible = true;
                pnlgrdList.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Stock listing");
            }
        }

        private void initVariables()
        {
            pnlgrdList.Visible = false;
            grdList.Visible = false;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
            cmbFilterStock.SelectedIndex = 1;
            CatalogueValueDB.fillCatalogValueComboNew(cmbStoreLocation, "StoreLocation");
            Structures.ComboBoxItem cbitem =
                           new Structures.ComboBoxItem("All", "All");
            cmbStoreLocation.Items.Add(cbitem);
            cmbStoreLocation.SelectedIndex = cmbStoreLocation.Items.Count - 1;
            
        }
        private void applyPrivilege()
        {
            try
            {
                if (Main.itemPriv[1])
                {
                    btnExportToExcel.Visible = true;
                }
                else
                {
                    btnExportToExcel.Visible = false;
                }
                if (Main.itemPriv[2])
                {
                    btnExportToExcel.Visible = true;
                }
                else
                {
                    btnExportToExcel.Visible = false;
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
                pnlgrdList.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        public void clearUserData()
        {
            try
            {
                cmbFilterStock.SelectedIndex = 1;
                cmbStoreLocation.SelectedIndex = cmbStoreLocation.Items.Count - 1;
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
        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void cmbcmpnysrch_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
            grdList.Rows.Clear();
            pnlgrdList.Visible = false;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
        }

        private void filterGridData()
        {
            try
            {
                foreach (DataGridViewRow row in grdList.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {

                    foreach (DataGridViewRow row in grdList.Rows)
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

            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {

            if (cmbFilterStock.SelectedIndex == -1)
            {
                MessageBox.Show("Select Stock Filtering");
                return;
            }
            string storeID = ((Structures.ComboBoxItem)cmbStoreLocation.SelectedItem).HiddenValue;
            ListFilteredStock(getFilterNo(), storeID);
            lblSearch.Visible = true;
            txtSearch.Visible = true;
        }
        private int getFilterNo()
        {
            int no = 0;
            string storeID = ((Structures.ComboBoxItem)cmbStoreLocation.SelectedItem).HiddenValue;
            if (cmbFilterStock.SelectedItem.ToString().Trim().Equals("All") && storeID.Trim().Equals("All"))    //Filter1 : All, Filter2(Store) : All
                no = 1;
            else if (cmbFilterStock.SelectedItem.ToString().Trim().Equals("All") && !storeID.Trim().Equals("All"))  //Filter1 : All, Filter2(Store) : Selected
                no = 2;
            else if (!cmbFilterStock.SelectedItem.ToString().Trim().Equals("All") && storeID.Trim().Equals("All"))   //Filter1 : selected, Filter2(Store) : All
                no = 3;
            else
                no = 4;             // Filter1 : selected, Filter2(Store) : selected
            return no;
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
                ////////foreach(ListViewItem item in exlv.Items)
                ////////{
                ////////    if (item.SubItems[1].Text == "ModelNo")
                ////////    {
                ////////        exlv.Items.Remove(item);
                ////////    }
                ////////    if (item.SubItems[1].Text == "ModelName")
                ////////    {
                ////////        exlv.Items.Remove(item);
                ////////    }
                ////////}
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
              
                string heading1 = "Stock report ";
                string heading2 = "";
                Utilities.export2Excel(heading1, heading2, "", grdList, exlv);
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export to Excell error");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ////MessageBox.Show("txtSearch_TextChanged() : started");
            ////filterTimer = new Timer();
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterTimer.Tick -= new System.EventHandler(this.handlefilterTimerTimeout);
            filterTimer.Tick += new System.EventHandler(this.handlefilterTimerTimeout);
            filterTimer.Interval = 500;
            filterTimer.Enabled = true;
            filterTimer.Start();
        }
        private void handlefilterTimerTimeout(object sender, EventArgs e)
        {
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterGridData();
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }
        private void cmbStoreLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
            grdList.Rows.Clear();
            pnlgrdList.Visible = false;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
        }

        private void StockReport_Enter(object sender, EventArgs e)
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

