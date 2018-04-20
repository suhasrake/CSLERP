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
    public partial class StoreEmpMapping : System.Windows.Forms.Form
    {
        storeempmapping prevdoc;
        public StoreEmpMapping()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }

        private void StoreEmpMapping_Load(object sender, EventArgs e)
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
            ListStoreEmpMapping();
            applyPrivilege();
        }
        private void ListStoreEmpMapping()
        {
            try
            {
                grdList.Rows.Clear();
                StoreEmpMappingDB semDB = new StoreEmpMappingDB();
                List<storeempmapping> SEMs = semDB.getStockEmpMapping();
                foreach (storeempmapping sem in SEMs)
                {
                    grdList.Rows.Add(sem.StoreLocationID, sem.Description,
                        sem.EmployeeID,sem.EmployeeName,
                         ComboFIll.getStatusString(sem.Status));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            enableBottomButtons();
            pnlDocumentList.Visible = true;
        }

        private void initVariables()
        {
            try
            {
                
                fillStatusCombo(cmbDocumentStatus);
                EmployeeDB.fillEmpListCombo(cmbEmployee);
                CatalogueValueDB.fillCatalogValueComboNew(cmbStoreLocation,"StoreLocation");
            }
            catch (Exception)
            {

            }
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
                pnlDocumentInner.Visible = false;
                pnlDocumentOuter.Visible = false;
                pnlDocumentList.Visible = false;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                clearData();
                enableBottomButtons();
                pnlDocumentList.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        public void clearData()
        {
            try
            {
                cmbDocumentStatus.SelectedIndex = 0;
                cmbEmployee.SelectedIndex = 0;
                cmbStoreLocation.SelectedIndex = 0;
                //prevdoc = new storeempmapping();
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
                clearData();
                btnSave.Text = "Save";
                pnlDocumentOuter.Visible = true;
                pnlDocumentInner.Visible = true;
                cmbStoreLocation.Enabled = true;
                cmbStoreLocation.SelectedIndex = -1;
                cmbEmployee.SelectedIndex = -1;
                cmbStoreLocation.Enabled = true;
                cmbEmployee.Enabled = true;
                disableBottomButtons();
            }
            catch (Exception)
            {

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                storeempmapping sem = new storeempmapping();
                StoreEmpMappingDB semDB = new StoreEmpMappingDB();

                try
                {
                    sem.StoreLocationID = ((Structures.ComboBoxItem)cmbStoreLocation.SelectedItem).HiddenValue;
                    sem.Description = ((Structures.ComboBoxItem)cmbStoreLocation.SelectedItem).ToString();

                    //sem.StoreLocationID = cmbStoreLocation.SelectedItem.ToString().Trim().Substring(0, cmbStoreLocation.SelectedItem.ToString().Trim().IndexOf('-'));
                    //sem.Description = cmbStoreLocation.SelectedItem.ToString().Trim().Substring(cmbStoreLocation.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                    sem.EmployeeName = cmbEmployee.SelectedItem.ToString().Trim().Substring(0, cmbEmployee.SelectedItem.ToString().Trim().IndexOf('-'));
                    sem.EmployeeID = cmbEmployee.SelectedItem.ToString().Trim().Substring(cmbEmployee.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                }
                catch (Exception)
                {
                    sem.StoreLocationID = "";
                    sem.Description = "";
                    sem.EmployeeName = "";
                    sem.EmployeeID = "";
                }

                sem.Status = ComboFIll.getStatusCode(cmbDocumentStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;

                {
                    if (btnText.Equals("Update"))
                    {
                        if (semDB.updateStockEmpMapping(sem))
                        {
                            MessageBox.Show("StoreEmpMapping Status updated");
                            closeAllPanels();
                            ListStoreEmpMapping();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update StoreEmpMapping Status");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (semDB.validateDocument(sem))
                        {
                            if (semDB.insertStockEmpMapping(sem))
                            {
                                MessageBox.Show("StoreEmpMapping data Added");
                                closeAllPanels();
                                ListStoreEmpMapping();
                            }
                            else
                            {
                                MessageBox.Show("Failed to Insert StoreEmpMapping");
                            }
                        }
                        else
                        {
                            MessageBox.Show("StoreEmpMapping Data Validation failed");
                        }
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing StoreEmpMapping");
            }

        }

        private void grdDocumentList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            try
            {
                if (e.ColumnIndex == 5)
                {
                    clearData();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlDocumentInner.Visible = true;
                    pnlDocumentOuter.Visible = true;
                    pnlDocumentList.Visible = false;
                    cmbStoreLocation.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbStoreLocation, grdList.Rows[e.RowIndex].Cells[0].Value.ToString());
                    //cmbStoreLocation.SelectedIndex = cmbStoreLocation.FindStringExact(grdList.Rows[e.RowIndex].Cells[0].Value.ToString() + "-" + grdList.Rows[e.RowIndex].Cells[1].Value.ToString());
                    cmbEmployee.SelectedIndex = cmbEmployee.FindStringExact(grdList.Rows[e.RowIndex].Cells[3].Value.ToString() + "-" + grdList.Rows[e.RowIndex].Cells[2].Value.ToString());
                    cmbDocumentStatus.SelectedIndex = cmbDocumentStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[4].Value.ToString());
                    cmbStoreLocation.Enabled = false;
                    cmbEmployee.Enabled = false;
                    disableBottomButtons();
                }
            }
            catch (Exception)
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

        private void StoreEmpMapping_Enter(object sender, EventArgs e)
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

