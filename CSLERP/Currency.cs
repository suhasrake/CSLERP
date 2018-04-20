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
    public partial class Currency : System.Windows.Forms.Form
    {

        public Currency()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void Currency_Load(object sender, EventArgs e)
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
            ListCurrency();
            applyPrivilege();
        }
        private void ListCurrency()
        {
            try
            {
                grdList.Rows.Clear();
                CurrencyDB dbrecord = new CurrencyDB();
                List<currency> Currencies = dbrecord.getCurrencies();
                foreach (currency curr in Currencies)
                {
                    grdList.Rows.Add(curr.CurrencyID, curr.name, curr.symbol,
                         ComboFIll.getStatusString(curr.status));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in Currency listing");
            }
            enableBottomButtons();
            pnlCurrencyList.Visible = true;
        }

        private void initVariables()
        {
            try
            {
                
                fillStatusCombo(cmbStatus);
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
                pnlCurrencyInner.Visible = false;
                pnlCurrencyOuter.Visible = false;
                pnlCurrencyList.Visible = false;
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
                pnlCurrencyList.Visible = true;
                enableBottomButtons();
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
                txtSymbol.Text = "";
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
                pnlCurrencyOuter.Visible = true;
                pnlCurrencyInner.Visible = true;
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
                currency curr = new currency();
                CurrencyDB currDB = new CurrencyDB();

                curr.CurrencyID = txtID.Text;
                curr.name = txtName.Text;
                curr.symbol = txtSymbol.Text;
                curr.status = ComboFIll.getStatusCode(cmbStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;


                if (currDB.validateCurrency(curr))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (currDB.updateCurrency(curr))
                        {
                            MessageBox.Show("Currency updated");
                            closeAllPanels();
                            ListCurrency();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update Currency");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {

                        if (currDB.insertCurrency(curr))
                        {
                            MessageBox.Show("Currency Added");
                            closeAllPanels();
                            ListCurrency();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Currency");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Currency Data Validation failed");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed Adding / Editing Currency");
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
                    pnlCurrencyInner.Visible = true;
                    pnlCurrencyOuter.Visible = true;
                    pnlCurrencyList.Visible = false;
                    txtID.Enabled = false;
                    txtName.Enabled = true;
                    txtSymbol.Enabled = true;
                    cmbStatus.Enabled = true;
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[3].Value.ToString());
                    DataGridViewRow row = grdList.Rows[rowID];
                    txtID.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtName.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    txtSymbol.Text = grdList.Rows[e.RowIndex].Cells[2].Value.ToString();
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

        private void Currency_Enter(object sender, EventArgs e)
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

