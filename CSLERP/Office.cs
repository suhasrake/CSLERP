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
    public partial class Office : System.Windows.Forms.Form
    {

        public Office()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {
            }
        }
        private void Office_Load(object sender, EventArgs e)
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
            ListOffice();
            applyPrivilege();
        }
        private void ListOffice()
        {
            try
            {
                grdList.Rows.Clear();
                OfficeDB dbrecord = new OfficeDB();
                List<office> Offices = dbrecord.getOffices();
                foreach (office off in Offices)
                {
                    grdList.Rows.Add(off.OfficeID, off.name, off.RegionID + "-" + off.RegionName,
                        off.Address1, off.Address2, off.Address3, off.Address4,
                         ComboFIll.getStatusString(off.status));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in Office listing");
            }
            enableBottomButtons();
            pnlOfficeList.Visible = true;
        }

        private void initVariables()
        {
            try
            {
                
                fillStatusCombo(cmbStatus);
                RegionDB.fillRegionComboNew(cmbRegion);
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
                pnlOfficeInner.Visible = false;
                pnlOfficeOuter.Visible = false;
                pnlOfficeList.Visible = false;
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
                pnlOfficeList.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        public void clearData()
        {
            try
            {
                txtID.Text = "";
                txtName.Text = "";
                txtaddress1.Text = "";
                txtaddress2.Text = "";
                txtaddress3.Text = "";
                txtaddress4.Text = "";
                cmbStatus.SelectedIndex = 0;
                cmbRegion.SelectedIndex = 0;
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
                pnlOfficeOuter.Visible = true;
                pnlOfficeInner.Visible = true;
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
                office off = new office();
                OfficeDB offDB = new OfficeDB();
                off.OfficeID = txtID.Text;
                off.name = txtName.Text;
                try
                {
                    //////off.RegionID = cmbRegion.SelectedItem.ToString().Trim().Substring(0, cmbRegion.SelectedItem.ToString().Trim().IndexOf('-'));
                    off.RegionID = ((Structures.ComboBoxItem)cmbRegion.SelectedItem).HiddenValue;

                    //////off.RegionName = cmbRegion.SelectedItem.ToString().Trim().Substring(cmbRegion.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                    off.RegionName = ((Structures.ComboBoxItem)cmbRegion.SelectedItem).ToString();

                }
                catch (Exception)
                {
                    off.RegionID = "";
                    off.RegionName = "";
                }
                off.Address1 = txtaddress1.Text;
                off.Address2 = txtaddress2.Text;
                off.Address3 = txtaddress3.Text;
                off.Address4 = txtaddress4.Text;
                off.status = ComboFIll.getStatusCode(cmbStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (offDB.validateOffice(off))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (offDB.updateOffice(off))
                        {
                            MessageBox.Show("Office updated");
                            closeAllPanels();
                            ListOffice();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update Office");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {

                        if (offDB.insertOffice(off))
                        {
                            MessageBox.Show("Office Added");
                            closeAllPanels();
                            ListOffice();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Office");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Office Data Validation failed");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing Office");
            }

        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == 8)
                {
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlOfficeInner.Visible = true;
                    pnlOfficeOuter.Visible = true;
                    pnlOfficeList.Visible = false;
                    txtID.Enabled = false;
                    txtName.Enabled = true;
                    cmbStatus.Enabled = true;
                    txtID.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtName.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    ////cmbRegion.SelectedIndex = cmbRegion.FindStringExact(grdList.Rows[e.RowIndex].Cells[2].Value.ToString());
                    string tstr = grdList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    cmbRegion.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbRegion, tstr.Substring(0,tstr.IndexOf('-')));
                    txtaddress1.Text = grdList.Rows[e.RowIndex].Cells[3].Value.ToString();
                    txtaddress2.Text = grdList.Rows[e.RowIndex].Cells[4].Value.ToString();
                    txtaddress3.Text = grdList.Rows[e.RowIndex].Cells[5].Value.ToString();
                    txtaddress4.Text = grdList.Rows[e.RowIndex].Cells[6].Value.ToString();
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[7].Value.ToString());
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

        private void Office_Enter(object sender, EventArgs e)
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

