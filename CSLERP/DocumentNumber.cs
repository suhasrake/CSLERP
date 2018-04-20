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
    public partial class DocumentNumber : System.Windows.Forms.Form
    {
        Form frmPopup = new Form();
        TextBox txtSearch = new TextBox();
        DataGridView grdEmpList = new DataGridView();
        int DRowID = 0;
        public DocumentNumber()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {
            }
        }
        private void Office_Load(object sender, EventArgs e)
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
            ListOffice();
            applyPrivilege();
        }

        private void ListOffice()
        {
            try
            {
                grdList.Rows.Clear();
                DocumentNumberDB dbrecord = new DocumentNumberDB();
                List<documentnumber> docnum = dbrecord.getDocumentNo();
                foreach (documentnumber doc in docnum)
                {
                    grdList.Rows.Add(doc.rowid, doc.fyID, doc.DocumentID,
                        doc.DocumentName, doc.TemporaryNo, doc.DocumentNo);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in Document Number listing");
            }
            enableBottomButtons();
            pnlOfficeList.Visible = true;
        }

        private void initVariables()
        {
            try
            {
                FinancialYearDB.fillFYIDCombo(cmbFY);
                cmbFY.SelectedIndex = cmbFY.FindString(Main.currentFY);
                //RegionDB.fillRegionComboNew(cmbFY);
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
                pnlOfficeInner.Visible = false;
                pnlOfficeOuter.Visible = false;
                pnlOfficeList.Visible = false;
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
                pnlOfficeList.Visible = true;
                DRowID = 0;
            }
            catch (Exception)
            {

            }
        }

        public void clearData()
        {
            try
            {
                txtTempno.Text = "";
                txtDocument.Text = "";
                txtDocumentNo.Text = "";
                cmbFY.SelectedIndex = 0;
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
                clearData();
                btnSave.Text = "Save";
                pnlOfficeOuter.Visible = true;
                pnlOfficeInner.Visible = true;
                cmbFY.Enabled = true;
                btnDocument.Enabled = true;
                txtTempno.Enabled = true;
                txtDocument.Enabled = true;
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
                documentnumber dn = new documentnumber();
                DocumentNumberDB DnDB = new DocumentNumberDB();
                string[] docmts = txtDocument.Text.Split('-');
                dn.rowid = DRowID;
                dn.fyID = cmbFY.SelectedItem.ToString().Trim().Substring(0, cmbFY.SelectedItem.ToString().Trim().IndexOf(':')).Trim();
                dn.DocumentID = docmts[1];
                dn.DocumentName = docmts[0]; 
                dn.TemporaryNo = Convert.ToInt32(txtTempno.Text);
                dn.DocumentNo = Convert.ToInt32(txtDocumentNo.Text);
               
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (DnDB.validateDocumentNo(dn))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (DnDB.updateDocumentnumber(dn))
                        {
                            MessageBox.Show("Document Number updated");
                            closeAllPanels();
                            ListOffice();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update Document Number");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {

                        if (DnDB.insertDocument(dn))
                        {
                            MessageBox.Show("Document Number Added");
                            closeAllPanels();
                            ListOffice();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Document Number");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Document Number Data Validation failed");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing Document Number");
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
                    pnlOfficeInner.Visible = true;
                    pnlOfficeOuter.Visible = true;
                    pnlOfficeList.Visible = false;
                    cmbFY.Enabled = false;
                    txtDocument.Enabled = false;
                    btnDocument.Enabled = false;
                    txtTempno.Enabled = true;
                    txtDocumentNo.Enabled = true;
                    DRowID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells[0].Value);
                    cmbFY.SelectedIndex = cmbFY.FindString(grdList.Rows[e.RowIndex].Cells[1].Value.ToString());
                    txtDocument.Text = grdList.Rows[e.RowIndex].Cells[3].Value.ToString()+"-"+ grdList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    txtTempno.Text = grdList.Rows[e.RowIndex].Cells[4].Value.ToString();
                    txtDocumentNo.Text = grdList.Rows[e.RowIndex].Cells[5].Value.ToString();
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

        private void Office_Enter(object sender, EventArgs e)
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

        private void btnDocument_Click(object sender, EventArgs e)
        {
            showDocumentDataGridView();
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

                frmPopup.Size = new Size(500, 370);

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

                grdEmpList.Bounds = new Rectangle(new Point(0, 27), new Size(500, 300));
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
            }
            catch (Exception)
            {
            }
        }
        private void grdCancel_Click1(object sender, EventArgs e)
        {
            try
            {
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

    }
}

