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
    public partial class StockItemNew : System.Windows.Forms.Form
    {
        int opt = 0;
        public StockItemNew()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void Region_Load(object sender, EventArgs e)
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
            ListStockItem();
            applyPrivilege();
        }
        private string getStatusString(int code)
        {
            string str = "";
            if (code == 1)
                str = "Active";
            else
                str = "Deactive";
            return str;
        }
        private void ListStockItem()
        {
            try
            {
                grdList.Rows.Clear();
                StockItemNewDB SiDb = new StockItemNewDB();
                List<stockitemnew> SIlist = SiDb.getStockItems();
                foreach (stockitemnew item in SIlist)
                {
                    grdList.Rows.Add(item.StockItemID, item.Name,item.Group1Code,item.Group1CodeDescription,item.Unit,
                         getStatusString(item.Status),item.DocumentStatus);
                }
                filterGridData();
            }
            catch (Exception)
            {
                MessageBox.Show("Error in Stock listing");
            }
            enableBottomButtons();
            pnlList.Visible = true;
        }

        private void initVariables()
        {
            StockGroupDB.fillGroupValueCombo(cmbGroup1, 1);
            CatalogueValueDB.fillCatalogValueComboNew(cmbUnit, "StockUnit");
            fillStatusCombo(cmbStatus);
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
                pnlStockInner.Visible = false;
                pnlStockOuter.Visible = false;
                pnlList.Visible = false;
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
        public void clearUserData()
        {
            try
            {
                txtStockItemID.Text = "";
                txtDescription.Text = "";
                cmbStatus.SelectedIndex = 0;
                cmbGroup1.SelectedIndex = -1;
                cmbUnit.SelectedIndex = -1;
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
                closeAllPanels();
                clearUserData();
                btnSave.Text = "Save";
                pnlStockOuter.Visible = true;
                pnlStockInner.Visible = true;
                cmbGroup1.Enabled = true ;
                disableBottomButtons();
            }
            catch (Exception)
            {

            }
        }
        private int getStatusCode(string str)
        {
            int code = 0;
            if (str.Equals("Active"))
                code = 1;
            else
                code = 0;
            return code;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                stockitemnew si = new stockitemnew();
                StockItemNewDB siDB = new StockItemNewDB();
                if (cmbStatus.SelectedIndex == -1)
                {
                    MessageBox.Show("Fill Status Combo");
                    return;
                }

                si.Name = txtDescription.Text;
                si.Group1Code = cmbGroup1.SelectedItem.ToString().Trim().
                    Substring(0, cmbGroup1.SelectedItem.ToString().Trim().IndexOf('-'));
                si.Unit = ((Structures.ComboBoxItem)cmbUnit.SelectedItem).HiddenValue;
                //si.Status = ComboFIll.getStatusCode(cmbStatus.SelectedItem.ToString());
                si.Status = getStatusCode(cmbStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (siDB.validateRegion(si))
                {
                    if (btnText.Equals("Update"))
                    {
                        si.StockItemID = txtStockItemID.Text;
                        if (siDB.updateStockItem(si))
                        {
                            MessageBox.Show("StockItem updated");
                            closeAllPanels();
                            //txtSearchGrd.Text = "";
                            ListStockItem();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update StockItem");
                            return;
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        si.StockItemID = CreateStockItemID(si.Group1Code + "0000000000");
                        si.DocumentStatus = 99;
                        if (siDB.insertStockItem(si))
                        {
                            MessageBox.Show("StockItem Added");
                            closeAllPanels();
                            txtSearchGrd.Text = "";
                            ListStockItem();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert StockItem");
                            return;
                        }

                    }
                }
                else
                {
                    MessageBox.Show("StockItem Data Validation failed");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed Adding / Editing StockItem");
                return;
            }
        }
        public string CreateStockItemID(string baseCode)
        {
            string id = "";
            int val = 1;
            Hashtable ht = StockItemNew.getStockIDTable();
            try
            {
                if (ht.ContainsKey(baseCode))
                {
                    val = Convert.ToInt32(ht[baseCode].ToString());
                    val = val + 1;
                    //ht[itc] = val;
                }
                else
                {
                    ht.Add(baseCode, val.ToString());
                    val = 1;
                }
                id = baseCode + val.ToString("0000");
            }
            catch (Exception ex)
            {
            }
            return id;
        }
        private static Hashtable getStockIDTable()
        {
            Hashtable ht = new Hashtable();
            StockItemNewDB sidb = new StockItemNewDB();
            List<stockitemnew> Silist = sidb.getStockItems();
            foreach(stockitemnew si in Silist)
            {
                try
                {
                    string id = si.StockItemID;
                    string key = id.Substring(0, 12);
                    string value = id.Substring(12, 4);
                    if (ht.ContainsKey(key))
                        ht[key] = value;
                    else
                        ht.Add(key, value);
                }
                catch (Exception ex)
                {
                }
            }
            return ht;
        }
        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit"))
                {
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlStockInner.Visible = true;
                    pnlStockOuter.Visible = true;
                    cmbGroup1.Enabled = false;
                    pnlList.Visible = false;
                    cmbGroup1.SelectedIndex = cmbGroup1.FindString(grdList.Rows[e.RowIndex].Cells["GroupCode1"].Value.ToString());
                    cmbStatus.SelectedIndex = cmbStatus.FindString(grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString());
                    txtStockItemID.Text = grdList.Rows[e.RowIndex].Cells["StockItemID"].Value.ToString();
                    txtDescription.Text = grdList.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                    cmbUnit.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbUnit, grdList.Rows[e.RowIndex].Cells["Unit"].Value.ToString());
                    disableBottomButtons();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void disableBottomButtons()
        {
            btnNew.Visible = false;
            btnExit.Visible = false;
        }
        private void enableBottomButtons()
        {
            btnNew.Visible = true;
            btnExit.Visible = true;
        }

        private void cmbGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                clearUserData();
                enableBottomButtons();
                pnlList.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        private void txtSearchGrd_TextChanged(object sender, EventArgs e)
        {
            filterGridData();
        }
        private void filterGridData()
        {
            try
            {
                foreach (DataGridViewRow row in grdList.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearchGrd.Text.Length != 0)
                {

                    foreach (DataGridViewRow row in grdList.Rows)
                    {
                        if (!row.Cells["Description"].Value.ToString().Trim().ToLower().Contains(txtSearchGrd.Text.Trim().ToLower()))
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

        private void StockItemNew_Enter(object sender, EventArgs e)
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

