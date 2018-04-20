using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSLERP.DBData;
using System.IO;
using System.Drawing.Drawing2D;

namespace CSLERP
{
    public partial class FinancialYear : System.Windows.Forms.Form
    {
        ////public static string connString;
        public static string[,] empStatusValues;
        public FinancialYear()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void FinancialYear_Load(object sender, EventArgs e)
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
            ListFinancialYear();
            applyPrivilege();
        }
        private void ListFinancialYear()
        {
            try
            {
                grdList.Rows.Clear();
                //FinancialYearDB fyDB = new FinancialYearDB();
                List<financialyear> FYears = FinancialYearDB.getFinancialYear();
                foreach (financialyear fyear in FYears)
                {
                    grdList.Rows.Add(fyear.fyID, fyear.startDate,
                        fyear.endDate,
                        ComboFIll.getStatusString(fyear.status), getCurrentFYStatusString(fyear.IsCurrentFY),fyear.documentStatus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            enableBottomButtons();
            pnlList.Visible = true;
        }

        private string getCurrentFYStatusString(int stat)
        {
            string status = "";
            status = stat == 0? status = "No" : status = "yes";
            return status;
        }
        private int getCurrentFYStatusCode(string stat)
        {
            int code;
            code = stat.Equals("Yes") ? code = 1 : code = 0;
            return code;
        }
        private void initVariables()
        {
            try
            {
                
                dtStart.Format = DateTimePickerFormat.Custom;
                dtStart.CustomFormat = "dd-MM-yyyy";
                dtEnd.Format = DateTimePickerFormat.Custom;
                dtEnd.CustomFormat = "dd-MM-yyyy";
                fillStatusCombo(cmbStatus);
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


        public void clearFYData()
        {
            try
            {
                txtFYID.Text = "";
                dtStart.Value = DateTime.Parse("1990-01-01");
                dtEnd.Value = DateTime.Parse("1990-01-01");
                cmbStatus.SelectedIndex = 1;
                cmbIsCurrentFY.SelectedIndex = -1;
                lblCurrentFy.Visible = false;
                cmbIsCurrentFY.Visible = false;
            }
            catch (Exception)
            {

            }

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                clearFYData();
                btnSave.Text = "Save";
                txtFYID.Enabled = true;
                dtStart.Enabled = true;
                dtEnd.Enabled = true;
                pnlOuter.Visible = true;
                pnlInner.Visible = true;
                btnLock.Visible = false;
                lblCurrentFy.Visible = false;
                cmbIsCurrentFY.Visible = false;
                disableBottomButtons();
            }
            catch (Exception)
            {

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                clearFYData();
                closeAllPanels();
                enableBottomButtons();
                pnlList.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                financialyear fyear = new financialyear();
                FinancialYearDB fyDB = new FinancialYearDB();
                fyear.fyID = txtFYID.Text;
                fyear.startDate = dtStart.Value;
                fyear.endDate = dtEnd.Value;
                fyear.status = ComboFIll.getStatusCode(cmbStatus.SelectedItem.ToString());
                fyear.documentStatus = 99;

                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;

                if (fyDB.validateFinancialYear(fyear))
                {
                    if (btnText.Equals("Update"))
                    {
                        fyear.IsCurrentFY = getCurrentFYStatusCode(cmbIsCurrentFY.SelectedItem.ToString());
                        if (fyear.IsCurrentFY == 1)
                        {
                            if (!FinancialYearDB.verifyCurrentFYStatus(fyear.fyID))
                            {
                                MessageBox.Show("Check Current Financial Year status. \nAlready Current Financial Year is there. \nNot allowed more Year.");
                                return;
                            }
                        }
                        if (fyear.IsCurrentFY == 1 && fyear.status == 2)
                        {
                            fyear.IsCurrentFY = 0;
                        }
                        if (fyDB.updateFinancialYear(fyear))
                        {
                            MessageBox.Show("Financial Year data updated");
                            closeAllPanels();
                            ListFinancialYear();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update Financial Year Data");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (fyDB.insertFinancialYear(fyear))
                        {
                            MessageBox.Show("Financial Year data Added");
                            closeAllPanels();
                            ListFinancialYear();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Financial Year Data");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Financial Year Data Validation Failed");
                }
            }
            catch (Exception)
            {

            }
        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == 6)
                {
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlInner.Visible = true;
                    pnlOuter.Visible = true;
                    pnlList.Visible = false;
                    clearFYData();
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[3].Value.ToString());
                    cmbIsCurrentFY.SelectedIndex = cmbIsCurrentFY.FindString(grdList.Rows[e.RowIndex].Cells["CYStatus"].Value.ToString());
                    DataGridViewRow row = grdList.Rows[rowID];
                    txtFYID.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    //tempDate = grdEmployeeList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    dtStart.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells[1].Value.ToString());
                    dtEnd.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells[2].Value.ToString());
                    btnLock.Visible = true;
                    int documentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells[5].Value.ToString());
                    if (documentStatus == 99)
                    {
                        txtFYID.Enabled = false;
                        dtStart.Enabled = false;
                        dtEnd.Enabled = false;
                        btnLock.Visible = false;
                    }
                    else
                    {
                        txtFYID.Enabled = true;
                        dtStart.Enabled = true;
                        dtEnd.Enabled = true;
                        btnLock.Visible = true;
                    }
                    if (grdList.Rows[e.RowIndex].Cells[3].Value.ToString().Equals("Active"))
                    {
                        lblCurrentFy.Visible = true;
                        cmbIsCurrentFY.Visible = true;
                    }
                    disableBottomButtons();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("You cannot modify the details after Locking. Are you sure to Lock the row ?", "Yes", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                FinancialYearDB fyDB = new FinancialYearDB();
                if (fyDB.LockFinancialYear(txtFYID.Text))
                {
                    MessageBox.Show("Financial Year Locked");
                    closeAllPanels();
                    ListFinancialYear();
                }
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

        private void btnExport_Click(object sender, EventArgs e)
        {

        }

        private void FinancialYear_Enter(object sender, EventArgs e)
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

