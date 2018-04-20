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
    public partial class StockOB : System.Windows.Forms.Form
    {
        private string[] ArithmetcOperators = { "Add", "Subtract", "Multiply", "Divide", "Percentage" };
        double totalcost = 0.0;
        Boolean captureChange = false;
        public StockOB()
        {
            try
            {
                InitializeComponent();
                this.FormBorderStyle = FormBorderStyle.None;
                Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
                initVariables();
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                this.Width -= 100;
                this.Height -= 100;
                this.FormBorderStyle = FormBorderStyle.Fixed3D;
                String a = this.Size.ToString();
                grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdList.EnableHeadersVisualStyles = false;
                ListBOMHeader();
                applyPrivilege();
            }
            catch (Exception)
            {

            }
        }

        private void ListBOMHeader()
        {
            try
            {
                grdList.Rows.Clear();
                BOMDB bomdb = new BOMDB();
                List<bomheader> BOMHeaders = bomdb.getBOMHeader();
                foreach (bomheader bh in BOMHeaders)
                {
                    grdList.Rows.Add(bh.ProductID, bh.Name, bh.Details, bh.Cost,
                         Main.getStatusString(bh.status));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in BOM Header listing");
            }
            pnlList.Visible = true;
        }

        private void initVariables()
        {
            fillStatusCombo(cmbStatus);
            StockItemDB.fillStockItemCombo(cmbProduct, "Products");
        }
        private void applyPrivilege()
        {
            try
            {
                if (Main.itemPriv[1])
                {
                    btnNew.Visible = true;
                }
                else
                {
                    btnNew.Visible = false;
                }
                if (Main.itemPriv[2])
                {
                    grdList.Columns["Edit"].Visible = true;
                }
                else
                {
                    grdList.Columns["Edit"].Visible = false;
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
                fillStatusCombo(cmbStatus);
                pnlList.Visible = false;
                pnlOuter.Visible = false;
                pnlBOMDetails.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        private void fillStatusCombo(System.Windows.Forms.ComboBox cmb)
        {
            try
            {
                cmb.Items.Clear();
                for (int i = 0; i < Main.statusValues.GetLength(0); i++)
                {
                    cmb.Items.Add(Main.statusValues[i, 1]);
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnUserCancel_Click(object sender, EventArgs e)
        {

        }

        public void clearData()
        {
            try
            {
                cmbProduct.SelectedIndex=-1;
                cmbStatus.SelectedIndex = 0;
                grdBOMDetail.Rows.Clear();
            }
            catch (Exception)
            {

            }
        }

        private void btnListExit_Click(object sender, EventArgs e)
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
                //closeAllPanels();
                clearData();
                btnSave.Text = "Save";
                cmbProduct.Enabled = true;
                pnlBottomActions.Visible = false;
                grdBOMDetail.Rows.Clear();
                pnlBOMDetails.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {
                BOMDB bomdb = new BOMDB();
                bomheader bh = new bomheader();

                bh = new bomheader();
                string iid = cmbProduct.SelectedItem.ToString();
                iid = iid.Substring(0,iid.IndexOf('-'));
                bh.ProductID = iid;
                bh.Details = txtDetails.Text;
                bh.Cost = totalcost;
                bh.status = Main.getStatusCode(cmbStatus.SelectedItem.ToString());

                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;
                if (!validateItems(bh.ProductID))
                {
                    return;
                }
                if (bomdb.validateBOMHeader(bh))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (bomdb.updateBOMHeader(bh))
                        {
                            if (createAndUpdateBOMDetails(iid))
                            {
                                MessageBox.Show("BOM Details updated");
                                closeAllPanels();
                                ListBOMHeader();
                                pnlBottomActions.Visible = true;
                            }
                            else
                            {
                                status = false;
                            }
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
                        if (bomdb.insertBOMHeader(bh))
                        {
                            if (createAndUpdateBOMDetails(iid))
                            {
                                MessageBox.Show("TBOM Details Added");
                                closeAllPanels();
                                ListBOMHeader();
                                pnlBottomActions.Visible = true;
                            }
                            else
                            {
                                status = false;
                                bomdb.deleteBOMHeader(bh);
                            }
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
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing BOM");
            }

        }
        private Boolean createAndUpdateBOMDetails(string productID)
        {
            Boolean status = true;
            try
            {
                BOMDB bomdb = new BOMDB();
                bomdetail bd = new bomdetail();

                List<bomdetail> BOMDetail = new List<bomdetail>();
                for (int i = 0; i < grdBOMDetail.Rows.Count; i++)
                {
                    try
                    {
                        string iid = grdBOMDetail.Rows[i].Cells[1].Value.ToString();
                        iid = iid.Substring(0,iid.IndexOf('-'));
                        bd = new bomdetail();
                        bd.ProductID = productID;
                        bd.StockItemID = iid;
                        bd.Quantity= Convert.ToDouble(grdBOMDetail.Rows[i].Cells[2].Value.ToString());
                        bd.PurchasePrice = Convert.ToDouble(grdBOMDetail.Rows[i].Cells[3].Value.ToString());
                        bd.CustomPrice = Convert.ToDouble(grdBOMDetail.Rows[i].Cells[4].Value.ToString());
                        BOMDetail.Add(bd);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("createAndUpdateBOMDetails() : Error creating BOM Details");
                        status = false;
                    }
                }
                /////*TaxCodeWorkingDB*/ customerbankdetailsdb = new CustomerBankDetailsDB();
                if (!bomdb.updateBOMDetail(productID, BOMDetail))
                {
                    MessageBox.Show("createAndUpdateBOMDetails() : Failed to update BOM Details. Please check the values");
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("createAndUpdateBOMDetails() : Error updating BOM Details");
                status = false;
            }
            return status;
        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == 5)
                {
                    captureChange = false;
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    cmbProduct.SelectedIndex= cmbProduct.FindStringExact(grdList.Rows[e.RowIndex].Cells[0].Value.ToString());
                    txtDetails.Text = grdList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    txtCost.Text = grdList.Rows[e.RowIndex].Cells[3].Value.ToString();
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[4].Value.ToString());
                    DataGridViewRow row = grdList.Rows[rowID];
                    //get customer bank details
                    BOMDB bomdb = new BOMDB();
                    List<bomdetail> bomdetails = bomdb.getBOMDetail(grdList.Rows[e.RowIndex].Cells[0].Value.ToString());
                    grdBOMDetail.Rows.Clear();
                    int i = 0;
                    foreach (bomdetail bd in bomdetails)
                    {

                        AddBOMDetailRow();
                        grdBOMDetail.Rows[i].Cells[0].Value = i+1;
                        //DataGridViewComboBoxCell ComboColumn1 = new DataGridViewComboBoxCell();
                        //StockItemDB.fillTaxItemGridViewCombo(ComboColumn1,"");
                        //grdBOMDetail.Rows[i].Cells[1] = ComboColumn1;
                        //grdBOMDetail.Rows[i].Cells[1].Value = bd.Name;

                        DataGridViewComboBoxCell ComboColumn1 = new DataGridViewComboBoxCell();
                        StockItemDB.fillStockItemGridViewCombo(ComboColumn1, "");
                        grdBOMDetail.Rows[i].Cells[1].Value = bd.StockItemID+"-"+bd.Name;

                        grdBOMDetail.Rows[i].Cells[2].Value = bd.Quantity;
                        grdBOMDetail.Rows[i].Cells[3].Value = bd.PurchasePrice;
                        grdBOMDetail.Rows[i].Cells[4].Value = bd.CustomPrice;

                        i++;
                    }
                    cmbProduct.SelectedIndex = cmbProduct.FindString(grdList.Rows[e.RowIndex].Cells[0].Value.ToString());
                    cmbProduct.Enabled = false;
                    verifyAndReworkBOMDetailGridRows();
                    btnSave.Text = "Update";
                    pnlBOMDetails.Visible = true;
                    pnlBottomActions.Visible = false;
                    captureChange = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                AddBOMDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private Boolean AddBOMDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdBOMDetail.Rows.Count > 0)
                {
                    verifyAndReworkBOMDetailGridRows();
                }
                grdBOMDetail.Rows.Add();
                int kount = grdBOMDetail.RowCount;
                grdBOMDetail.Rows[kount - 1].Cells[0].Value = kount;
                DataGridViewComboBoxCell ComboColumn1 = (DataGridViewComboBoxCell)(grdBOMDetail.Rows[kount - 1].Cells[1]);
                StockItemDB.fillStockItemGridViewCombo(ComboColumn1,"");
                ComboColumn1.DropDownWidth = 300;
                
                grdBOMDetail.Rows[kount - 1].Cells[2].Value = 0;
                grdBOMDetail.Rows[kount - 1].Cells[3].Value = 0;
                grdBOMDetail.Rows[kount - 1].Cells[4].Value = 0;
                grdBOMDetail.Rows[kount - 1].Cells[5].Value = 0;
                grdBOMDetail.Rows[kount - 1].Cells[7].Value = 0;
                var BtnCell = (DataGridViewButtonCell)grdBOMDetail.Rows[kount - 1].Cells[6];
                BtnCell.Value = "Del";
                grdBOMDetail.Rows[kount - 1].Cells[0].ReadOnly = true;
                grdBOMDetail.Rows[kount - 1].Cells[3].ReadOnly = true;
                grdBOMDetail.Rows[kount - 1].Cells[5].ReadOnly = true;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddBOMDetailRow() : Error");
            }

            return status;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ////pnlTaxCodeWorking.Visible = false;
            closeAllPanels();
            btnNew.Visible = true;
            btnExit.Visible = true;
            pnlList.Visible = true;
            pnlBottomActions.Visible = true;
        }

        private void grdBOMDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == 6)
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
            }
            catch (Exception ex)
            {

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
                    if ((grdBOMDetail.Rows[i].Cells[2].Value == null) ||
                        ((grdBOMDetail.Rows[i].Cells[3].Value == null) &&
                        (grdBOMDetail.Rows[i].Cells[4].Value == null)) )
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                    if (Convert.ToInt32(grdBOMDetail.Rows[i].Cells[7].Value.ToString()) == 1)
                    {
                        //load new purchase price  or BOM cost
                        grdBOMDetail.Rows[i].Cells[3].Value = 0.0;
                        grdBOMDetail.Rows[i].Cells[7].Value = 0;
                        string iid = grdBOMDetail.Rows[i].Cells[1].Value.ToString();
                        iid = iid.Substring(0, iid.IndexOf('-'));
                        double BOMCost = bomdb.getBOMCost(iid);
                        if (BOMCost==0)
                        {
                            //get purchase price from stock table
                        }
                        else
                        {
                            grdBOMDetail.Rows[i].Cells[3].Value = BOMCost;
                        }
                        //MessageBox.Show("Item changed in this row");
                    }
                    quantity = Convert.ToDouble(grdBOMDetail.Rows[i].Cells[2].Value);
                    purchaseprice = Convert.ToDouble(grdBOMDetail.Rows[i].Cells[3].Value);
                    customprice = Convert.ToDouble(grdBOMDetail.Rows[i].Cells[4].Value);
                    if (purchaseprice != 0)
                    {
                        cost = Math.Round(quantity * purchaseprice, 2);
                    }
                    else if (customprice !=0)
                    {
                        cost = Math.Round(quantity * customprice, 2);
                    }
                    else
                    {
                        cost = 0;
                    }
                    grdBOMDetail.Rows[i].Cells[5].Value = cost;
                    totalcost = totalcost + cost;
                }
                txtCost.Text = totalcost.ToString();
                btnCost.Text = txtCost.Text;
            }
            catch (Exception)
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (verifyAndReworkBOMDetailGridRows())
            {
                pnlOuter.Visible = true;
                pnlOuter.BringToFront();
            }
        }

        private void btnIgnore_Click(object sender, EventArgs e)
        {
            pnlOuter.Visible = false;
        }

        private Boolean validateItems(string ProductID)
        {
            Boolean status = true;
            for (int i = 0; i < grdBOMDetail.Rows.Count-1; i++)
            {
                string tstr = grdBOMDetail.Rows[i].Cells[1].Value.ToString().Substring(0, grdBOMDetail.Rows[i].Cells[1].Value.ToString().IndexOf('-'));
                if (tstr == ProductID)
                {
                    //same product in BOM
                    MessageBox.Show("BOM product in item list... please correct before saving");
                    return false;
                }
                for (int j = i+1; j < grdBOMDetail.Rows.Count; j++)
                {
                    if (grdBOMDetail.Rows[i].Cells[1].Value.ToString() == grdBOMDetail.Rows[j].Cells[1].Value.ToString())
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
            try
            {
                int rowIndex = e.RowIndex;
                int colIndex = e.ColumnIndex;
                if (colIndex == 1 && captureChange)
                {
                    //item changed
                    grdBOMDetail.Rows[rowIndex].Cells[7].Value = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

