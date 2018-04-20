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
    public partial class BankBranch : System.Windows.Forms.Form
    {
        private int selectedRowID = 0;
        public BankBranch()
        {
           
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {
            }
        }
        private void BankBranch_Load(object sender, EventArgs e)
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
            ListBankBranch();
            applyPrivilege();
        }
        private void ListBankBranch()
        {
            try
            {
                grdList.Rows.Clear();
                BankBranchDB bankbranchdb = new BankBranchDB();
                List<bankbranch> BankBranches = bankbranchdb.getBankBranches();
                foreach (bankbranch branch in BankBranches)
                {
                    grdList.Rows.Add(branch.BankID, branch.BranchName, branch.Address1 , branch.Address2,
                        branch.Address3, branch.IFSCCode, branch.SWIFTCode ,branch.MICRCode, branch.BSRCode,
                         ComboFIll.getStatusString(branch.status), branch.BranchID);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in Office listing");
            }
            enableBottomButtons();
            pnlList.Visible = true;
        }

        private void initVariables()
        {
            try
            {
                
                fillStatusCombo(cmbStatus);
               // CatalogueValueDB.fillCatalogValueCombo(cmbBank,"Bank");
                CatalogueValueDB.fillCatalogValueComboNew(cmbBank, "Bank");
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
                pnlInner.Visible = false;
                pnlOuter.Visible = false;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                clearData();
                enableBottomButtons();
                pnlList.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        public void clearData()
        {
            try
            {
                
                txtBranchName.Text = "";
                txtaddress1.Text = "";
                txtaddress2.Text = "";
                txtaddress3.Text = "";
                txtIFSCCode.Text = "";
                txtMICRCode.Text = "";
                txtBSRCode.Text = "";
                txtSWIFTCode.Text = "";
                cmbStatus.SelectedIndex = 0;
                cmbBank.SelectedIndex = 0;
            }
            catch (Exception)
            {

            }
        }

        private void btnUserListExit_Click(object sender, EventArgs e)
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

        private void btnNewUser_Click_1(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                clearData();
                btnSave.Text = "Save";
                pnlOuter.Visible = true;
                pnlInner.Visible = true;
                txtBranchName.Enabled = true;
                cmbStatus.Enabled = true;
                cmbBank.Enabled = true;
                disableBottomButtons();
            }
            catch (Exception)
            {

            }
        }

        private void btnRegionSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                bankbranch branch = new bankbranch();
                BankBranchDB bankbranchdb = new BankBranchDB();
                //off.OfficeID = txtID.Text;
               
                try
                {
                    branch.BankID = ((Structures.ComboBoxItem)cmbBank.SelectedItem).HiddenValue;
                    branch.BankName = ((Structures.ComboBoxItem)cmbBank.SelectedItem).ToString();
                    //branch.BankID = cmbBank.SelectedItem.ToString().Trim().Substring(0, cmbBank.SelectedItem.ToString().Trim().IndexOf('-'));
                    //branch.BankName = cmbBank.SelectedItem.ToString().Trim().Substring(cmbBank.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                }
                catch (Exception)
                {
                    branch.BankID = "";
                    branch.BankName = "";
                }
                branch.BranchName = txtBranchName.Text;
                branch.Address1 = txtaddress1.Text;
                branch.Address2 = txtaddress2.Text;
                branch.Address3 = txtaddress3.Text;
                branch.IFSCCode = txtIFSCCode.Text;
                branch.SWIFTCode = txtSWIFTCode.Text;
                branch.MICRCode = txtMICRCode.Text;
                branch.BSRCode = txtBSRCode.Text;
                branch.status = ComboFIll.getStatusCode(cmbStatus.SelectedItem.ToString());
                branch.BranchID = selectedRowID;
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (bankbranchdb.validateBankBranch(branch))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (bankbranchdb.updateBankBranch(branch))
                        {
                            MessageBox.Show("Bank Branch updated");
                            closeAllPanels();
                            ListBankBranch();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update Bank Branch");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {

                        if (bankbranchdb.insertBankBranch(branch))
                        {
                            MessageBox.Show("Bank Branch Added");
                            closeAllPanels();
                            ListBankBranch();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Bank Branch");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Bank Branch Data Validation failed");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing Bank Branch");
            }

        }

        private void grdRegionList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                
                if (e.ColumnIndex == 11)
                {
                    
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlInner.Visible = true;
                    pnlOuter.Visible = true;
                    pnlList.Visible = false;
                    
                    txtBranchName.Enabled = true;
                    cmbStatus.Enabled = true;
                    cmbBank.Enabled = false;
                    //txtID.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    //cmbBank.SelectedIndex = cmbBank.FindStringExact(grdList.Rows[e.RowIndex].Cells[0].Value.ToString());
                    string bank = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    cmbBank.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbBank, bank);
                    txtBranchName.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    txtaddress1.Text = grdList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    txtaddress2.Text = grdList.Rows[e.RowIndex].Cells[3].Value.ToString();
                    txtaddress3.Text = grdList.Rows[e.RowIndex].Cells[4].Value.ToString();
                    txtIFSCCode.Text = grdList.Rows[e.RowIndex].Cells[5].Value.ToString();
                    txtSWIFTCode.Text= grdList.Rows[e.RowIndex].Cells[6].Value.ToString();
                    txtMICRCode.Text = grdList.Rows[e.RowIndex].Cells[7].Value.ToString();
                    txtBSRCode.Text = grdList.Rows[e.RowIndex].Cells[8].Value.ToString();
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[9].Value.ToString());
                    selectedRowID= Convert.ToInt32(grdList.Rows[e.RowIndex].Cells[10].Value.ToString());
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

        private void pnlBottomButtons_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlList_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BankBranch_Enter(object sender, EventArgs e)
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

