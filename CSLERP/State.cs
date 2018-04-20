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
    public partial class State : System.Windows.Forms.Form
    {

        public State()
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
            ListState();
            applyPrivilege();
        }
        private void ListState()
        {
            try
            {
                grdList.Rows.Clear();
                StateDB sdb = new StateDB();
                List<state> sList = sdb.getStateList();
                foreach (state stat in sList)
                {
                    grdList.Rows.Add(stat.StateCode, stat.StateName,
                         getStatusString(stat.Status),stat.CreateTime,stat.CreateUser);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in State listing");
            }
            enableBottomButtons();
            pnlStateList.Visible = true;
        }

        private void initVariables()
        {

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
                txtStateID.Text = "";
                txtStateName.Text = "";
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
                pnlStateOuter.Visible = true;
                pnlStateInner.Visible = true;
                txtStateID.Enabled = true;
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
                state stat = new state();
                StateDB sdb = new StateDB();

                stat.StateCode = txtStateID.Text.Trim() ;
                stat.StateName = txtStateName.Text.Trim();
                stat.Status = getStatusCode(cmbStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (sdb.validateState(stat))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (sdb.updateState(stat))
                        {
                            MessageBox.Show("State updated");
                            closeAllPanels();
                            ListState();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update State");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {

                        if (sdb.insertState(stat))
                        {
                            MessageBox.Show("State Added");
                            closeAllPanels();
                            ListState();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert state");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("state Data Validation failed");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing state");
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
                    btnSave.Text = "Update";
                    pnlStateInner.Visible = true;
                    pnlStateOuter.Visible = true;
                    pnlStateList.Visible = false;
                    txtStateID.Enabled = false;
                    string code = grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString();
                    cmbStatus.SelectedIndex = cmbStatus.FindString(code);
                    DataGridViewRow row = grdList.Rows[rowID];
                    txtStateID.Text = grdList.Rows[e.RowIndex].Cells["StateID"].Value.ToString();
                    txtStateName.Text = grdList.Rows[e.RowIndex].Cells["StateName"].Value.ToString();
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

        private void State_Enter(object sender, EventArgs e)
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

