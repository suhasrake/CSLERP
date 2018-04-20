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
    public partial class CompanyAddress : System.Windows.Forms.Form
    {
        Form frmPopup = new Form();
        companyaddress prevca;
        public CompanyAddress()
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
            ListCompanyAddress();
            applyPrivilege();
        }
        private void ListCompanyAddress()
        {
            try
            {
                grdList.Rows.Clear();
                CompanyAddressDB cadb = new CompanyAddressDB();
                List<companyaddress> caList = cadb.getCompAddList();
                //BindingSource src = new BindingSource();
                //src.DataSource = caList;
                //grdList.DataSource = caList;
                foreach (companyaddress ca in caList)
                {
                    //grdList.Rows.Add(ca.CompanyID, ca.CompanyName, ca.AddressType, ca.Address,
                    //     getStatusString(ca.Status), ca.CreateTime, ca.CreateUser);

                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["RowID"].Value = ca.RowID;
                    grdList.Rows[grdList.RowCount - 1].Cells["CompanyID"].Value = ca.CompanyID;
                    grdList.Rows[grdList.RowCount - 1].Cells["CompanyName"].Value = ca.CompanyName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Address"].Value = ca.Address;
                    grdList.Rows[grdList.RowCount - 1].Cells["AddressType"].Value = getAddressTypeString(ca.AddressType);
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = getStatusString(ca.Status);
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = ca.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = ca.CreateUser;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in Company Address listing");
            }
            enableBottomButtons();
            pnlStateList.Visible = true;
        }

        private void initVariables()
        {
            CompanyDetailDB.fillCompanyIDComboNew(cmbCompanyId);
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
                cmbCompanyId.SelectedIndex = -1;
                cmbAddressType.SelectedIndex = -1;
                txtAddress.Text = "";
                cmbStatus.SelectedIndex = 0;
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
                cmbCompanyId.Enabled = true;
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
        private int getAddressTypeCode(string stat)
        {
            int code = 0;
            if (stat.Equals("Delivery Address"))
                code = 1;
            else if(stat.Equals("Billing Address"))
                code = 2;
            else if (stat.Equals("Registered Address"))
                code = 3;
            return code;
        }
        private string getAddressTypeString(int stat)
        {
            string str = "";
            if (stat == 1)
                str = "Delivery Address";
            else if (stat == 2)
                str = "Billing Address";
            else if (stat == 3)
                str = "Registered Address";
            return str;
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                companyaddress ca = new companyaddress();
                CompanyAddressDB cadb = new CompanyAddressDB();

                ca.CompanyID = Convert.ToInt32(((Structures.ComboBoxItem)cmbCompanyId.SelectedItem).HiddenValue);
                ca.AddressType = getAddressTypeCode(cmbAddressType.SelectedItem.ToString());
                ca.Address = txtAddress.Text.Trim();
                ca.Status = getStatusCode(cmbStatus.SelectedItem.ToString());
                ca.RowID = prevca.RowID;
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (cadb.validateCompanyAdd(ca))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (cadb.updateCompAddress(ca))
                        {
                            MessageBox.Show("Company Address updated");
                            closeAllPanels();
                            ListCompanyAddress();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update Company Address");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {

                        if (cadb.insertCompAddress(ca))
                        {
                            MessageBox.Show("Company Address Added");
                            closeAllPanels();
                            ListCompanyAddress();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Company Address");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Company Address Data Validation failed");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing Company Address");
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
                    cmbCompanyId.Enabled = false;
                    pnlStateInner.Visible = true;
                    pnlStateOuter.Visible = true;
                    pnlStateList.Visible = false;
                    prevca.RowID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["RowID"].Value.ToString());
                    prevca.CompanyID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["CompanyID"].Value.ToString());
                    prevca.AddressType = getAddressTypeCode(grdList.Rows[e.RowIndex].Cells["AddressType"].Value.ToString());
                    prevca.Address = grdList.Rows[e.RowIndex].Cells["Address"].Value.ToString();
                    string code = grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString();
                    cmbStatus.SelectedIndex = cmbStatus.FindString(code);
                    DataGridViewRow row = grdList.Rows[rowID];
                    //txtStateID.Text = grdList.Rows[e.RowIndex].Cells["StateID"].Value.ToString();
                    txtAddress.Text = prevca.Address;
                    string cID = grdList.Rows[e.RowIndex].Cells["CompanyID"].Value.ToString();
                    cmbCompanyId.SelectedIndex =  Structures.ComboFUnctions.getComboIndex(cmbCompanyId, cID);
                    cmbAddressType.SelectedIndex = cmbAddressType.FindString(getAddressTypeString(prevca.AddressType));


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

        private void CompanyAddress_Enter(object sender, EventArgs e)
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

