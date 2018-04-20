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
    public partial class StatusCatalogue : System.Windows.Forms.Form
    {

        employee emp = new employee();
        Form frmPopup = new Form();
        DataGridView grdEmpList = new DataGridView();
        TextBox txtSearch = new TextBox();
        int rowid = 0;
        bool btn1click = false;
        bool btn2click = false;
        public StatusCatalogue()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void DocEmpMapping_Load(object sender, EventArgs e)
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
            ListStatCatalogues();
            applyPrivilege();
        }
        ////private void Form1_Load(object sender, EventArgs e)
        ////{
        ////    try
        ////    {
        ////        ListDocEmpMapping();
        ////    }
        ////    catch (Exception)
        ////    {

        ////    }

        ////}
        private void ListStatCatalogues()
        {
            try
            {
                grdList.Rows.Clear();
                StatusCatalogueDB dbrecord = new StatusCatalogueDB();
                List<statuscatalogue> StatusCatalogue = dbrecord.getStatusCatalogue();
                foreach (statuscatalogue stat in StatusCatalogue)
                {
                    grdList.Rows.Add(stat.Rowid, stat.DocumentID, stat.DocumentName,
                        stat.ID, stat.Description, setstatus(stat.Status));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            enableBottomButtons();
            pnlDocumentList.Visible = true;
        }

        private void initVariables()
        {
            try
            {

                fillDocumentStatusCombo(cmbStatus);
                //EmployeeDB.fillEmpListCombo(cmbEmployee);
                //EmployeeDB.fillEmpListCombo(cmbSeniorEmployee);
                //MenuItemDB.fillMenuItemComboNew(cmbDocument);
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
                pnlDocumentInner.Visible = false;
                pnlDocumentOuter.Visible = false;
                pnlDocumentList.Visible = false;
            }
            catch (Exception)
            {

            }
        }
        public string setstatus(int stat)
        {
            string docstat = "";
            if (stat == 1)
            {
                docstat = "Active";
            }
            else if (stat == 0)
            {
                docstat = "Deactive";
            }
            return docstat;
        }


        public int getstatus(string stat)
        {
            int docstat = 0;
            if (stat == "Active")
            {
                docstat = 1;
            }
            else if (stat == "Deactive")
            {
                docstat = 0;
            }
            return docstat;
        }

        private void fillDocumentStatusCombo(System.Windows.Forms.ComboBox cmb)
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
                clearDocumentData();
                enableBottomButtons();
                pnlDocumentList.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        public void clearDocumentData()
        {
            try
            {
                cmbStatus.SelectedIndex = 0;
                rowid = 0;
                txtDocument.Text = "";
                txtID.Text = "";
                txtDescription.Text = "";
                //prevdoc = new docempmapping();
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
                clearDocumentData();
                btnSave.Text = "Save";
                pnlDocumentOuter.Visible = true;
                pnlDocumentInner.Visible = true;
                btnDocument.Enabled = true;
                disableBottomButtons();
            }
            catch (Exception)
            {

            }
        }

        private void showDocumentDataGridView()
        {
            try
            {
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.CadetBlue;

                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(550, 370);

                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(120, 5);
                lblSearch.AutoSize = true;
                lblSearch.Text = "Search by Name";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(250, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInDocGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                DocEmpMappingDB empDB = new DocEmpMappingDB();
                grdEmpList = empDB.getDocumentlistGrid();

                grdEmpList.Bounds = new Rectangle(new Point(0, 27), new Size(550, 300));
                frmPopup.Controls.Add(grdEmpList);
                grdEmpList.Columns["TableName"].Visible = false;
                grdEmpList.Columns["IsReversible"].Visible = false;
                grdEmpList.Columns["DocumentStatus"].Visible = false;
                grdEmpList.Columns["userCreateUser"].Visible = false;
                grdEmpList.Columns["userCreateime"].Visible = false;
                grdEmpList.Columns["DocumentID"].Width = 200;
                grdEmpList.Columns["DocumentName"].Width = 250;
                foreach (DataGridViewColumn column in grdEmpList.Columns)
                    column.SortMode = DataGridViewColumnSortMode.Automatic;

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grddocOK_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 335);
                lvCancel.Click += new System.EventHandler(this.grdCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        private void grddocOK_Click1(object sender, EventArgs e)
        {
            string users = "";
            try
            {
                var checkedRows = from DataGridViewRow r in grdEmpList.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("Select one Document");
                    return;
                }

                foreach (var row in checkedRows)
                {
                    txtDocument.Text = row.Cells["DocumentName"].Value.ToString() + '-' + row.Cells["DocumentID"].Value.ToString();
                }

                frmPopup.Close();
                frmPopup.Dispose();
                btn1click = false;
                btn2click = false;
            }
            catch (Exception)
            {
            }
        }
        private void txtSearch_TextChangedInDocGridList(object sender, EventArgs e)
        {
            try
            {
                //grdForStockList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                filterGridDocData();
                ///grdForStockList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {

            }
        }

        private void filterGridDocData()
        {
            try
            {
                grdEmpList.CurrentCell = null;
                foreach (DataGridViewRow row in grdEmpList.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdEmpList.Rows)
                    {
                        if (!row.Cells["DocumentName"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
                        {
                            row.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Searching");
            }
        }

        private void grdCancel_Click1(object sender, EventArgs e)
        {
            try
            {
                frmPopup.Close();
                frmPopup.Dispose();
                btn1click = false;
                btn2click = false;
            }
            catch (Exception)
            {
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                statuscatalogue sc = new statuscatalogue();
                StatusCatalogueDB SCDB = new StatusCatalogueDB();

                if(txtID.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Fill Status ID.");
                }
                try
                {
                    string[] docmts = txtDocument.Text.Split('-');
                    sc.Rowid = rowid;
                    sc.DocumentID = docmts[1].Trim();
                    sc.ID = Convert.ToInt32(txtID.Text.Trim());
                    sc.Description = txtDescription.Text.Trim().Replace("'", "''");
                }
                catch (Exception)
                {
                    sc.DocumentID = "";
                    sc.ID = 0;
                    sc.Description = "";
                    sc.Rowid = 0;
                }

                sc.Status = getstatus(cmbStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (btnText.Equals("Update"))
                {
                    if (SCDB.updateStatusCatalogue(sc))
                    {
                        MessageBox.Show("Status Catalogue Status updated");
                        closeAllPanels();
                        ListStatCatalogues();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update Status Catalogue");
                    }
                }
                else if (btnText.Equals("Save"))
                {
                    if (SCDB.validateDocument(sc))
                    {
                        if (SCDB.insertStatusCatalogue(sc))
                        {
                            MessageBox.Show("Status Catalogue data Added");
                            closeAllPanels();
                            ListStatCatalogues();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Status Catalogue");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Data Validation failed");
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing User Data");
            }

        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            try
            {
                string colName = grdList.Columns[e.ColumnIndex].Name;
                if (colName == "Edit")
                {
                    clearDocumentData();
                    btnDocument.Enabled = false;
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlDocumentInner.Visible = true;
                    pnlDocumentOuter.Visible = true;
                    pnlDocumentList.Visible = false;

                    rowid = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gRowID"].Value);
                    txtDocument.Text = grdList.Rows[e.RowIndex].Cells["DocumentName"].Value.ToString() + '-' + grdList.Rows[e.RowIndex].Cells["empID"].Value.ToString();
                    txtID.Text = grdList.Rows[e.RowIndex].Cells["gID"].Value.ToString();
                    txtDescription.Text = grdList.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString());
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

        private void btnDocument_Click(object sender, EventArgs e)
        {
            showDocumentDataGridView();
        }

        private void pnlDocumentInner_Paint(object sender, PaintEventArgs e)
        {

        }

        private void StatusCatalogue_Enter(object sender, EventArgs e)
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

