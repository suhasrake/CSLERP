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
using iTextSharp.text;

namespace CSLERP
{
    public partial class CompanyBank : System.Windows.Forms.Form
    {
        Form frmPopup = new Form();
        companyaddress prevca;
        Int32 selectedRow;
        public CompanyBank()
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
            ListCompanyBank();
            applyPrivilege();
        }
        private void ListCompanyBank()
        {
            try
            {
                grdList.Rows.Clear();
                CompanyBankDB cadb = new CompanyBankDB();
                List<companybank> caList = cadb.getCompBankList();
                //BindingSource src = new BindingSource();
                //src.DataSource = caList;
                //grdList.DataSource = caList;
                foreach (companybank ca in caList)
                {
                    //grdList.Rows.Add(ca.CompanyID, ca.CompanyName, ca.AddressType, ca.Address,
                    //     getStatusString(ca.Status), ca.CreateTime, ca.CreateUser);

                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["ROWID"].Value = ca.RowID;  // Stores Company Bank RowID
                    grdList.Rows[grdList.RowCount - 1].Cells["BranchID"].Value = ca.branchID;  // Stores bank Branch RowID
                    grdList.Rows[grdList.RowCount - 1].Cells["CompanyID"].Value = ca.CompanyID; 
                    grdList.Rows[grdList.RowCount - 1].Cells["Company"].Value = ca.CompanyName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Bank"].Value = ca.BankID;
                    grdList.Rows[grdList.RowCount - 1].Cells["Branch"].Value = ca.BranchName;
                    grdList.Rows[grdList.RowCount - 1].Cells["AccountType"].Value = ca.AccountType;
                    grdList.Rows[grdList.RowCount - 1].Cells["AccountCode"].Value = ca.AccountCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = ca.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = ca.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = getStatusString(ca.Status);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in Company Bank listing");
            }
            enableBottomButtons();
            pnlStateList.Visible = true;
        }

        private void initVariables()
        {
            CompanyDetailDB.fillCompanyIDComboNew(cmbCompany);
            CatalogueValueDB.fillCatalogValueComboNew(cmbBank, "Bank");
            CatalogueValueDB.fillCatalogValueComboNew(cmbAccType,"AccountType");
            cmbAccType.SelectedIndex = -1;
            cmbCompany.SelectedIndex = -1;
            cmbBank.SelectedIndex = -1;
            cmbStatus.SelectedIndex = 0;
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
                pnlStateInner.Visible = false;
                pnlStateOuter.Visible = false;
                pnlStateList.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                clearUserData();
                enableBottomButtons();
                pnlStateList.Visible = true;
            }
            catch (Exception)
            {

            }
        }
        public void clearUserData()
        {
            try
            {
                cmbCompany.SelectedIndex = -1;
                cmbBank.SelectedIndex = -1;
                txtAccCode.Text = "";
                cmbAccType.SelectedIndex = -1;
                cmbBranch.SelectedIndex = -1;
                cmbStatus.SelectedIndex = 0;
                selectedRow = 0;
                prevca = new companyaddress();
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
                cmbCompany.Enabled = true;
                cmbBranch.Enabled = true;
                cmbAccType.Enabled = true;
                pnlStateOuter.Visible = true;
                pnlStateInner.Visible = true;
                disableBottomButtons();
            }
            catch (Exception)
            {

            }
        }
        private int getStatusCode(string stat)
        {
            int code = 0;
            if (stat.Equals("Active"))
                code = 1;
            else
                code = 0;
            return code;
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                companybank ca = new companybank();
                CompanyBankDB cadb = new CompanyBankDB();
                ca.RowID = selectedRow;
                ca.CompanyID = ((Structures.ComboBoxItem)cmbCompany.SelectedItem).HiddenValue;
                ca.BranchName = ((Structures.ComboBoxItem)cmbBranch.SelectedItem).HiddenValue; // For Storing BAnk Branch RowID
                ca.AccountType= ((Structures.ComboBoxItem)cmbAccType.SelectedItem).HiddenValue;
                ca.AccountCode = txtAccCode.Text.Trim();
                ca.Status = getStatusCode(cmbStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (cadb.validateCompanyBank(ca))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (cadb.updateCompBAnk(ca))
                        {
                            MessageBox.Show("Company Bank updated");
                            closeAllPanels();
                            ListCompanyBank();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update Company Bank");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (cadb.insertCompBank(ca))
                        {
                            MessageBox.Show("Company Bank Added");
                            closeAllPanels();
                            ListCompanyBank();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Company Bank");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Company Bank Data Validation failed");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed Adding / Editing Company Bank");
            }
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

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string colName = grdList.Columns[e.ColumnIndex].Name;
                if (e.RowIndex < 0)
                    return;
                if (colName.Equals("Edit"))
                {
                    clearUserData();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    cmbCompany.Enabled = false;
                    pnlStateInner.Visible = true;
                    pnlStateOuter.Visible = true;
                    pnlStateList.Visible = false;
                    selectedRow = Convert.ToInt32( grdList.Rows[e.RowIndex].Cells["RowID"].Value.ToString());
                    string branchid = grdList.Rows[e.RowIndex].Cells["BranchID"].Value.ToString();
                    cmbCompany.SelectedIndex =  Structures.ComboFUnctions.getComboIndex(cmbCompany, grdList.Rows[e.RowIndex].Cells["CompanyID"].Value.ToString());
                    cmbBank.SelectedIndex= Structures.ComboFUnctions.getComboIndex(cmbBank, grdList.Rows[e.RowIndex].Cells["Bank"].Value.ToString());
                    cmbBranch.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbBranch, branchid);
                    cmbAccType.SelectedIndex= Structures.ComboFUnctions.getComboIndex(cmbAccType, grdList.Rows[e.RowIndex].Cells["AccountType"].Value.ToString());
                    string stat = grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString();
                    cmbStatus.SelectedIndex = cmbStatus.FindString(stat);
                    txtAccCode.Text= grdList.Rows[e.RowIndex].Cells["AccountCode"].Value.ToString();
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

        private void pnlStateList_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbBank_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                CompanyBankDB.fillBranchCombo(cmbBranch, ((Structures.ComboBoxItem)cmbBank.SelectedItem).HiddenValue);
                cmbBranch.SelectedIndex = -1;
            }
            catch(Exception ex)
            {

            }
        }

        private void cmbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CompanyBank_Enter(object sender, EventArgs e)
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

