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
    public partial class CompanyDetail : System.Windows.Forms.Form
    {

        public CompanyDetail()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void CompanyDetail_Load(object sender, EventArgs e)
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
            Listdet();
            applyPrivilege();
        }
        private void Listdet()
        {
            try
            {
                grdList.Rows.Clear();
                CompanyDetailDB dbrecord = new CompanyDetailDB();
                List<cmpnydetails> details = dbrecord.getdetails();
                foreach (cmpnydetails det in details)
                {
                    grdList.Rows.Add(det.companyID, det.companyname,det.companyAddress,
                         ComboFIll.getStatusString(det.status));
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
                txtcmpnyID.Text = "";
                txtcmpnyName.Text = "";
                txtcmpnyAddrs.Text = "";
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
                txtcmpnyID.Enabled = true;
                txtcmpnyName.Enabled = true;
                txtcmpnyAddrs.Enabled = true;
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
                cmpnydetails det = new cmpnydetails();
                CompanyDetailDB detDB = new CompanyDetailDB();
                int j = 0;
                det.companyID = Convert.ToInt32(txtcmpnyID.Text);
                det.companyname = txtcmpnyName.Text;
                det.companyAddress = txtcmpnyAddrs.Text;
                det.status = ComboFIll.getStatusCode(cmbStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                for (int i = 0; i < txtcmpnyAddrs.Lines.Length; i++)
                {
                    if (txtcmpnyAddrs.Lines[i].Length == 0)
                    {
                        j++;
                    }
                    else if(txtcmpnyAddrs.Lines[i].Length > 0 && txtcmpnyAddrs.Lines[i].Trim().Length == 0)
                    {
                        j++;
                    }
                }
                
                if (detDB.validatedetails(det))
                {
                    if(txtcmpnyAddrs.Lines.Length<=4+j)
                    {
                        if (btnText.Equals("Update"))
                        {
                            if (detDB.updatedetails(det))
                            {
                                MessageBox.Show("Details updated");
                                closeAllPanels();
                                Listdet();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update Details");
                            }
                        }
                        else if (btnText.Equals("Save"))
                        {

                            if (detDB.insertdetails(det))
                            {
                                MessageBox.Show("Details Added");
                                closeAllPanels();
                                Listdet();
                            }
                            else
                            {
                                MessageBox.Show("Failed to Insert Details");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Address lenth is more");
                    }
                }
                else
                {
                    MessageBox.Show("Details Data Validation failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed Adding / Editing Details");
            }
        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == 4)
                {
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlRegionInner.Visible = true;
                    pnlRegionOuter.Visible = true;
                    pnlRegionList.Visible = false;
                    txtcmpnyID.Enabled = false;
                    txtcmpnyName.Enabled = false;
                    txtcmpnyAddrs.Enabled = true;
                    cmbStatus.Enabled = true;
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[3].Value.ToString());
                    DataGridViewRow row = grdList.Rows[rowID];
                    txtcmpnyID.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtcmpnyName.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    txtcmpnyAddrs.Text = grdList.Rows[e.RowIndex].Cells[2].Value.ToString();
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

        private void CompanyDetail_Enter(object sender, EventArgs e)
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

