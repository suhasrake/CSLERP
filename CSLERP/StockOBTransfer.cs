using System;
using System.Collections.Generic;
using System.Collections;
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
using System.Collections.ObjectModel;

namespace CSLERP
{
    public partial class StockOBTransfer : System.Windows.Forms.Form
    {
        string docID = "";
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        customergroup prevsg = new customergroup();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        static int lvl = 0;
        int no;
        public StockOBTransfer()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void StockOBTransfer_Load(object sender, EventArgs e)
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
            ShowPannel();
        }
        private void ShowPannel()
        {
            pnlList.Visible = true;
            lvlSelect.Visible = true;
            cmbFY.Visible = true;
            grdList.Visible = false;
            btnCancel.Visible = false;
            btnTransfer.Visible = false;
        }
        private void listStockOBList(string FYID)
        {
            try
            {
                grdList.Rows.Clear();
                StockOBNewDB AccDB = new StockOBNewDB();
                List<stockObNewHeader> SBList = StockOBNewDB.getStockOblist(3);
                int i = 1;
                foreach (stockObNewHeader sobh in SBList)
                {
                    if (sobh.FYID.Equals(FYID))
                    {
                        grdList.Rows.Add();
                        grdList.Rows[grdList.RowCount - 1].Cells["LineNo"].Value = i;
                        grdList.Rows[grdList.RowCount - 1].Cells["FYID"].Value = sobh.FYID;
                        grdList.Rows[grdList.RowCount - 1].Cells["DocumentNo"].Value = sobh.DocumentNo;
                        grdList.Rows[grdList.RowCount - 1].Cells["DocumentDate"].Value = sobh.DocumentDate;//.ToString("dd-MM-yyyy");
                        grdList.Rows[grdList.RowCount - 1].Cells["StoreLocation"].Value = sobh.StoreLocation;
                        grdList.Rows[grdList.RowCount - 1].Cells["StoreLocationName"].Value = sobh.StoreLocationName;
                        grdList.Rows[grdList.RowCount - 1].Cells["Value"].Value = sobh.Value;
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Stock OB Listing");
            }
            try
            {
                enableBottomButtons();
                grdList.Visible = true;
                pnlList.Visible = true;
                //btnNew.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void initVariables()
        {
            
            docID = Main.currentDocument;

            pnlUI.Controls.Add(pnlList);
            FinancialYearDB.fillFYIDCombo(cmbFY);
            enableBottomButtons();
            showControlsForAccessPrevelidge();
            grdList.Visible = true;
            grdList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
        private void cmbSelectLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                grdList.Rows.Clear();
                string FYID = cmbFY.SelectedItem.ToString().Trim().Substring(0, cmbFY.SelectedItem.ToString().Trim().IndexOf(':')).Trim();
                listStockOBList(FYID);
                btnCancel.Visible = true;
                btnTransfer.Visible = true;
            }
            catch (Exception ex)
            {
            }
        }
     
        private void closeAllPanels()
        {
            try
            {
                pnlList.Visible = false;
                //pnlAddEdit.Visible = false;

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
                cmbFY.SelectedIndex = -1;
                btnTransfer.Enabled = true;
                btnCancel.Enabled = true;
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
        private void enableBottomButtons()
        {
            //btnNew.Visible = false;
            btnExit.Visible = true;
            pnlBottomButtons.Visible = true;
        }
        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        private void btnTransfer_Click(object sender, EventArgs e)
        {
            string FYID = cmbFY.SelectedItem.ToString().Trim().Substring(0, cmbFY.SelectedItem.ToString().Trim().IndexOf(':')).Trim();
            if (!StockOBNewDB.checkAvailabilityOfFY(FYID))
            {
                DialogResult dialog = MessageBox.Show("Stock For this year is available , Are you sure to Transfer ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    List<stockObNewHeader> SBList = StockOBNewDB.getStockOblistForTransfer(FYID);
                    if (StockOBNewDB.TransferStockOB(SBList, FYID))
                    {
                        if (StockOBNewDB.changeTransferStatus(FYID))
                        {
                            MessageBox.Show("stock Updated");
                            listStockOBList(FYID);
                        }
                        else
                            MessageBox.Show("Failed to Update Transfer Status");
                    }
                    else
                        MessageBox.Show("Failed to Update Stock");
                }
            }
            else
            {
                List<stockObNewHeader> SBList = StockOBNewDB.getStockOblistForTransfer(FYID);
                if (StockOBNewDB.TransferStockOB(SBList, FYID))
                {
                    if (StockOBNewDB.changeTransferStatus(FYID))
                    {
                        MessageBox.Show("stock Updated");
                        listStockOBList(FYID);
                    }
                    else
                        MessageBox.Show("Failed to Update Transfer Status");
                }
                else
                    MessageBox.Show("Failed to Update Stock");
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            grdList.Rows.Clear();
            grdList.Visible = false;
            cmbFY.SelectedIndex = -1;
            btnTransfer.Visible = false;
            btnCancel.Visible = false;
        }
        private void showControlsForAccessPrevelidge()
        {
            if(getuserPrivilegeStatus() == 1)
            {
                btnTransfer.Enabled = false;
                btnCancel.Enabled = false;
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

        private void StockOBTransfer_Enter(object sender, EventArgs e)
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


