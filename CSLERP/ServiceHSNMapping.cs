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
using CSLERP.FileManager;

namespace CSLERP
{
    public partial class ServiceHSNMapping : System.Windows.Forms.Form
    {
        Form frmPopup = new Form();
        ListView lv = new ListView();
        ListView lvCopy = new ListView();
        TextBox txtSearch = new TextBox();
        serviceHSNMapping prevmap;
        public ServiceHSNMapping()
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
            ListHSNMApping();
            applyPrivilege();
        }
        private void ListHSNMApping()
        {
            try
            {
                grdList.Rows.Clear();
                ServiceHSNMappingDB mapDB = new ServiceHSNMappingDB();
                List<serviceHSNMapping> mapList = mapDB.getServiceHSNMappingList();
                foreach (serviceHSNMapping map in mapList)
                {
                    //grdList.Rows.Add(map.StockItemID, map.StockItemName,map.ModelNo,map.ModelName, map.HSNCode,
                    //     getStatusString(map.Status), map.CreateTime, map.CreateUser);
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["RowID"].Value = map.RowID;
                    grdList.Rows[grdList.RowCount - 1].Cells["ServiceItemID"].Value = map.ServiceItemID;
                    grdList.Rows[grdList.RowCount - 1].Cells["ServiceItemName"].Value = map.ServiceItemName;
                    grdList.Rows[grdList.RowCount - 1].Cells["HSNCode"].Value = map.HSNCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = getStatusString(map.Status);
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = map.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = map.CreateUser;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in HSNMapping listing");
            }
            enableBottomButtons();
            pnlHSNList.Visible = true;
        }

        private void initVariables()
        {

            cmbStatus.SelectedIndex = 0;
            CatalogueValueDB.fillCatalogValueComboNew(cmbServiceItemID, "ServiceLookup");
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
                pnlHSNInner.Visible = false;
                pnlHSNOuter.Visible = false;
                pnlHSNList.Visible = false;
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
                pnlHSNList.Visible = true;
            }
            catch (Exception)
            {

            }
        }
        public void clearUserData()
        {
            try
            {
                cmbServiceItemID.SelectedIndex = -1;
                txtHSNCode.Text = "";
                cmbStatus.SelectedIndex = 0;
                prevmap = new serviceHSNMapping();
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
                pnlHSNOuter.Visible = true;
                pnlHSNInner.Visible = true;
                //btnSelectStock.Enabled = true;
                cmbServiceItemID.Enabled = true;
                //txtStateName.Enabled = true;
                //cmbStatus.Enabled = true;
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
                serviceHSNMapping map = new serviceHSNMapping();
                ServiceHSNMappingDB mapDB = new ServiceHSNMappingDB();
                if(txtHSNCode.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Fill HSN COde");
                    return;
                }
                map.ServiceItemID = ((Structures.ComboBoxItem)cmbServiceItemID.SelectedItem).HiddenValue;
                map.HSNCode = txtHSNCode.Text.Trim();
                map.Status = getStatusCode(cmbStatus.SelectedItem.ToString());
                map.RowID = prevmap.RowID;
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (mapDB.validateHSNMapping(map))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (mapDB.updateHSNCode(map))
                        {
                            MessageBox.Show("Service HSNCode updated");
                            closeAllPanels();
                            ListHSNMApping();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update HSNCode");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {

                        if (mapDB.insertHSNCOde(map))
                        {
                            MessageBox.Show("HSNCode Added");
                            closeAllPanels();
                            ListHSNMApping();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert HSNCode");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("HSNCode Data Validation failed");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing HSNCode");
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
                    int rowID = e.RowIndex;
                    clearUserData();
                    btnSave.Text = "Update";
                    pnlHSNInner.Visible = true;
                    pnlHSNOuter.Visible = true;
                    pnlHSNList.Visible = false;
                    //btnSelectStock.Enabled = false;
                    cmbServiceItemID.Enabled = false;
                    string code = grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString();
                    cmbStatus.SelectedIndex = cmbStatus.FindString(code);
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevmap.RowID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["RowID"].Value);
                    prevmap.ServiceItemID = grdList.Rows[e.RowIndex].Cells["ServiceItemID"].Value.ToString();
                    prevmap.ServiceItemName = grdList.Rows[e.RowIndex].Cells["ServiceItemName"].Value.ToString();
                    prevmap.HSNCode = grdList.Rows[e.RowIndex].Cells["HSNCode"].Value.ToString();

                    cmbServiceItemID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbServiceItemID, prevmap.ServiceItemID);
                    txtHSNCode.Text = grdList.Rows[e.RowIndex].Cells["HSNCode"].Value.ToString();

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

        private void ServiceHSNMapping_Enter(object sender, EventArgs e)
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

