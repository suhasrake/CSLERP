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
    public partial class CompanyData : System.Windows.Forms.Form
    { 
        public CompanyData()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void CompanyData_Load(object sender, EventArgs e)
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
            //Listdata();
            applyPrivilege();
        }
        private void Listdata()
        {
            try
            {
                grdList.Rows.Clear();
                CompanyDataDB dbrecord = new CompanyDataDB();
                int index = 0;
                CatalogueValueDB ACDb = new CatalogueValueDB();
                List<cataloguevalue> acList = ACDb.getCatalogueValues();
                List<cmpnydata> data = dbrecord.getData(cmbcmpnysrch.SelectedItem.ToString().Trim().Substring(0, cmbcmpnysrch.SelectedItem.ToString().Trim().IndexOf('-')).Trim());
                foreach (cataloguevalue cv in acList)
                {
                    index = 0;
                    if (cv.catalogueID == "CompanyData")
                    {
                        foreach (cmpnydata dat in data)
                        {

                            if (cv.catalogueValueID.Equals(dat.DataID))
                            {
                                grdList.Rows.Add(dat.DataID, dat.DataValue,
                                                    ComboFIll.getStatusString(dat.status));
                                index = 1;
                            }
                        }
                        if(index==0)
                            grdList.Rows.Add(cv.catalogueValueID, "", "");
                    }
                }
                grdList.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in CompanyData listing");
            }
            enableBottomButtons();
            pnlRegionList.Visible = true;
        }

        private void initVariables()
        {
            fillStatusCombo(cmbStatus);
            CompanyDataDB.fillCompanyIDCombo(cmbCmpnyID);
            CompanyDataDB.fillCompanyIDCombo(cmbcmpnysrch);
            //CatalogueValueDB.fillCatalogValueCombo(cmbDataID, "CompanyData");
            CatalogueValueDB.fillCatalogValueComboNew(cmbDataID, "CompanyData");
            grdList.Visible = false;
        }
        private void applyPrivilege()
        {
            try
            {
                if (Main.itemPriv[1])
                {
                   // btnNew.Visible = true;
                }
                else
                {
                    //btnNew.Visible = false;
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
                enabletop();
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
                cmbCmpnyID.SelectedIndex = 0;
                cmbDataID.SelectedIndex = 0;
                txtDataValue.Text = "";
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
                cmbCmpnyID.Enabled = true;
                cmbDataID.Enabled = true;
                txtDataValue.Enabled = true;
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
                cmpnydata dat = new cmpnydata();
                CompanyDataDB dataDB = new CompanyDataDB();
                string prevdaval = dat.DataValue;
                dat.CompanyID = Convert.ToInt32(cmbCmpnyID.SelectedItem.ToString().Trim().Substring(0, cmbCmpnyID.SelectedItem.ToString().Trim().IndexOf('-')).Trim());
                dat.DataID = ((Structures.ComboBoxItem)cmbDataID.SelectedItem).HiddenValue;
                //dat.DataID = cmbDataID.SelectedItem.ToString().Trim().Substring(0, cmbDataID.SelectedItem.ToString().Trim().IndexOf('-'));
                dat.DataValue = txtDataValue.Text;
                dat.status = ComboFIll.getStatusCode(cmbStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (dataDB.validateData(dat))
                {           
                        if (dataDB.insertData(dat))
                        {
                            MessageBox.Show("CompanyData Added");
                            closeAllPanels();
                            enabletop();
                            Listdata();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert CompanyData");
                        }
                    }
                else
                {
                    MessageBox.Show("CompanyData Validation failed");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed Adding / Editing CompanyData");
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
                    cmbCmpnyID.Enabled = false;
                    cmbDataID.Enabled = false;
                    txtDataValue.Enabled = true;
                    cmbStatus.Enabled = true;
                    cmbStatus.SelectedIndex = cmbStatus.FindString(grdList.Rows[e.RowIndex].Cells["CompanyStatus"].Value.ToString());
                    cmbCmpnyID.SelectedItem = cmbcmpnysrch.SelectedItem;
                    string dataID = grdList.Rows[e.RowIndex].Cells["DataID"].Value.ToString();
                    cmbDataID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbDataID, dataID);
                    txtDataValue.Text = grdList.Rows[e.RowIndex].Cells["DataValue"].Value.ToString();
                    disableBottomButtons();
                    disabletop();
                }
            }
            catch (Exception)
            {

            }
        }
        private void disabletop()
        {
            cmbcmpnysrch.Visible = false;
            btnSearch.Visible = false;
        }
        private void enabletop()
        {
            cmbcmpnysrch.Visible = true;
            btnSearch.Visible = true;
        }
        private void disableBottomButtons()
        {
          //  btnNew.Visible = false;
            btnExit.Visible = false;
        }
        private void enableBottomButtons()
        {
           // btnNew.Visible = true;
            btnExit.Visible = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(cmbcmpnysrch.SelectedIndex == -1)
            {
                MessageBox.Show("Select Company");
                return;
            }
            Listdata();
        }

        private void cmbcmpnysrch_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
        }

        private void CompanyData_Enter(object sender, EventArgs e)
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

