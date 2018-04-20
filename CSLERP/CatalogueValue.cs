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
    public partial class CatalogueValue : System.Windows.Forms.Form
    {
        string selectedCatalogueID = "";
        System.Windows.Forms.Button prevbtn = null;
        public CatalogueValue()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void CatalogueValue_Load(object sender, EventArgs e)
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
            CreateCataloueButtons();
        }

        private void CreateCataloueButtons()
        {
            try
            {
                CatalogueDB dbrecord = new CatalogueDB();
                List<catalogue> Catalogues = dbrecord.getCatalogues();
                int intex = 0;
                foreach (catalogue cat in Catalogues)
                {
                    if (cat.status == 1)
                    {
                        addButton(cat.catalogueID, intex);
                        intex++;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in request processing");
            }
        }

        private void addButton(string txtButton, int index)
        {
            try
            {
                System.Windows.Forms.Button button;
                button = new System.Windows.Forms.Button();
                button.Name = "btn" + txtButton;
                button.Text = txtButton;
                button.Height = 24;
                button.Width = 180;
                button.BackColor = Color.FromArgb(40, 40, 40);
                button.BackColor = Color.White;
                button.ForeColor = Color.Black;
                button.Font = new Font("Lucida Console", 10);
                button.Location = new Point(5, 4 + (index * 25));
                button.Click += new EventHandler(this.MyButtonHandler);
                pnlCataloueButtons.Controls.Add(button);
            }
            catch (Exception)
            {
                MessageBox.Show("Error in request processing");
            }
        }


        void MyButtonHandler(object sender, EventArgs e)
        {
            try
            {
                if (prevbtn != null)
                {
                    prevbtn.BackColor = Color.White;
                }
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                selectedCatalogueID = btn.Text;
                btn.BackColor = Color.SkyBlue;
                prevbtn = btn;
                pnlCatalogueValueOuter.Visible = false;
                ListCatalogueValues(selectedCatalogueID);
                applyPrivilege();
            }
            catch (Exception)
            {
                MessageBox.Show("Error in request processing");
            }
        }

        private void ListCatalogueValues(string CatalogueID)
        {
            try
            {
                grdList.Rows.Clear();
                CatalogueValueDB dbrecord = new CatalogueValueDB();
                List<cataloguevalue> CatalogueValues = dbrecord.getCatalogueValues();
                foreach (cataloguevalue catval in CatalogueValues)
                {
                    if (catval.catalogueID.Equals(CatalogueID))
                    {
                        grdList.Rows.Add(catval.catalogueValueID, catval.description,
                        ComboFIll.getStatusString(catval.status));
                    }
                }
                lblAddEditHeader.Text = CatalogueID;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            enableBottomButtons();
            pnlCatalogueValueList.Visible = true;
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
                pnlCatalogueValueInner.Visible = false;
                pnlCatalogueValueOuter.Visible = false;
                pnlCatalogueValueList.Visible = false;
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
                pnlCatalogueValueList.Visible = true;
                enableBottomButtons();
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
                txtDescription.Text = "";
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
                if (selectedCatalogueID.Trim().Length != 0)
                {
                    closeAllPanels();
                    clearData();
                    btnSave.Text = "Save";
                    pnlCatalogueValueOuter.Visible = true;
                    pnlCatalogueValueInner.Visible = true;
                    txtID.Enabled = true;
                    txtDescription.Enabled = true;
                    cmbStatus.Enabled = true;
                    disableBottomButtons();
                }
                else
                {
                    MessageBox.Show("Select catelog before clicking on New Btton");
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                cataloguevalue catval = new cataloguevalue();
                CatalogueValueDB catvalDB = new CatalogueValueDB();

                catval.catalogueValueID = txtID.Text;
                catval.description = txtDescription.Text;
                catval.catalogueID = selectedCatalogueID;
                catval.status = ComboFIll.getStatusCode(cmbStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;


                if (catvalDB.validateCatalogue(catval))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (catvalDB.updateCatalogueValue(catval))
                        {
                            MessageBox.Show("Catalogue Value updated");
                            closeAllPanels();
                            ListCatalogueValues(selectedCatalogueID);
                        }
                        else
                        {
                            MessageBox.Show("Failed to update Catalogue Value Status");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {

                        if (catvalDB.insertCatalogueValue(catval))
                        {
                            MessageBox.Show("Catalogue Value Added");
                            closeAllPanels();
                            ListCatalogueValues(selectedCatalogueID);
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Catalogue Value");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Catalogue Value Data Validation failed");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing User Data");
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
                    pnlCatalogueValueInner.Visible = true;
                    pnlCatalogueValueOuter.Visible = true;
                    pnlCatalogueValueList.Visible = false;
                    txtID.Enabled = false;
                    txtDescription.Enabled = true;
                    cmbStatus.Enabled = true;
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[2].Value.ToString());
                    DataGridViewRow row = grdList.Rows[rowID];
                    txtID.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtDescription.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();
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

        private void CatalogueValue_Enter(object sender, EventArgs e)
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


        //////private void grdList_KeyPress(object sender, KeyPressEventArgs e)
        //////{
        //////    if (e.KeyChar == (Char)Keys.Enter)
        //////    {
        //////        MessageBox.Show("testing key press");
        //////    }
        //////}
        //////private void grdList_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        //////{
        //////    MessageBox.Show("testing EditingControlShowing");
        //////}

    }
}

