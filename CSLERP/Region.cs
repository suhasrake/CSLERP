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
    public partial class Region : System.Windows.Forms.Form
    {

        public Region()
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
            ListRegion();
            applyPrivilege();
        }
        private void ListRegion()
        {
            try
            {
                grdList.Rows.Clear();
                RegionDB dbrecord = new RegionDB();
                List<region> Regions = dbrecord.getRegions();
                foreach (region reg in Regions)
                {
                    grdList.Rows.Add(reg.regionID, reg.name,
                         ComboFIll.getStatusString(reg.status));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in Region listing");
            }
            enableBottomButtons();
            pnlRegionList.Visible = true;
        }

        private void initVariables()
        {
            
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
                pnlRegionInner.Visible = false;
                pnlRegionOuter.Visible = false;
                pnlRegionList.Visible = false;
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
                clearUserData();
                enableBottomButtons();
                pnlRegionList.Visible = true;
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
                txtName.Text = "";
                cmbStatus.SelectedIndex = 0;
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
                pnlRegionOuter.Visible = true;
                pnlRegionInner.Visible = true;
                txtID.Enabled = true;
                txtName.Enabled = true;
                cmbStatus.Enabled = true;
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
                region reg = new region();
                RegionDB regDB = new RegionDB();

                reg.regionID = txtID.Text;
                reg.name = txtName.Text;
                reg.status = ComboFIll.getStatusCode(cmbStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (regDB.validateRegion(reg))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (regDB.updateRegion(reg))
                        {
                            MessageBox.Show("Region updated");
                            closeAllPanels();
                            ListRegion();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update Region");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {

                        if (regDB.insertRegion(reg))
                        {
                            MessageBox.Show("Region Added");
                            closeAllPanels();
                            ListRegion();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Region");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Region Data Validation failed");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing Region");
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
                    pnlRegionInner.Visible = true;
                    pnlRegionOuter.Visible = true;
                    pnlRegionList.Visible = false;
                    txtID.Enabled = false;
                    txtName.Enabled = true;
                    cmbStatus.Enabled = true;
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[2].Value.ToString());
                    DataGridViewRow row = grdList.Rows[rowID];
                    txtID.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtName.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();
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

        private void Region_Enter(object sender, EventArgs e)
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

