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
    public partial class Catalogue : System.Windows.Forms.Form
    {

        public Catalogue()
        {
          
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void Catalogue_Load(object sender, EventArgs e)
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
            ListCatalogue();
            applyPrivilege();
        }
        private void ListCatalogue()
        {
            try
            {
                grdList.Rows.Clear();
                CatalogueDB dbrecord = new CatalogueDB();
                List<catalogue> Catalogues = dbrecord.getCatalogues();
                foreach (catalogue cat in Catalogues)
                {
                    grdList.Rows.Add(cat.catalogueID, cat.description,
                         ComboFIll.getStatusString(cat.status));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            enableBottomButtons();
            pnlCatalogueList.Visible = true;
        }

        private void initVariables()
        {
            try
            {
                
                fillUserStatusCombo(cmbCatalogueStatus);
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
                pnlUserInner.Visible = false;
                pnlUserOuter.Visible = false;
                pnlCatalogueList.Visible = false;
            }
            catch (Exception)
            {

            }
        }


        private void fillUserStatusCombo(System.Windows.Forms.ComboBox cmb)
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
                clearUserData();
                pnlCatalogueList.Visible = true;
                enableBottomButtons();
            }
            catch (Exception)
            {

            }
        }

        public void clearUserData()
        {
            try
            {
                txtID.Text = "";
                txtDescription.Text = "";
                cmbCatalogueStatus.SelectedIndex = 0;
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
                pnlUserOuter.Visible = true;
                pnlUserInner.Visible = true;
                txtID.Enabled = true;
                txtDescription.Enabled = true;
                cmbCatalogueStatus.Enabled = true;
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
                catalogue cat = new catalogue();
                CatalogueDB catDB = new CatalogueDB();

                cat.catalogueID = txtID.Text;
                cat.description = txtDescription.Text;
                cat.status = ComboFIll.getStatusCode(cmbCatalogueStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                btnText = btnSave.Text;

                if (btnText.Equals("Update"))
                {
                    if (catDB.updateCatalogue(cat))
                    {
                        MessageBox.Show("Catalogue updated");
                        closeAllPanels();
                        ListCatalogue();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update Catalogue Status");
                    }
                }
                else if (btnText.Equals("Save"))
                {
                    if (catDB.validateCatalogue(cat))
                    {
                        if (catDB.insertCatalogue(cat))
                        {
                            MessageBox.Show("Catalogue Added");
                            closeAllPanels();
                            ListCatalogue();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Catalogue");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Catalogue Data Validation failed");
                    }
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing User Data");
            }

        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == 3)
                {
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlUserInner.Visible = true;
                    pnlUserOuter.Visible = true;
                    pnlCatalogueList.Visible = false;
                    txtID.Enabled = false;
                    txtDescription.Enabled = true;
                    cmbCatalogueStatus.Enabled = true;
                    cmbCatalogueStatus.SelectedIndex = cmbCatalogueStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[2].Value.ToString());
                    DataGridViewRow row = grdList.Rows[rowID];
                    txtID.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtDescription.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();
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

        private void Catalogue_Enter(object sender, EventArgs e)
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

