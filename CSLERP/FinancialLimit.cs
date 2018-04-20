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
    public partial class FinancialLimit : System.Windows.Forms.Form
    {
        financiallimit prevflim;
        Form frmPopup = new Form();
        TextBox txtSearch = new TextBox();
        DataGridView grdEmpList = new DataGridView();

        public FinancialLimit()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void FinancialLimit_Load(object sender, EventArgs e)
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
            ListEmpFinLimit();
            applyPrivilege();
        }
        private void ListEmpFinLimit()
        {
            try
            {
                grdList.Rows.Clear();
                FinancialLimitDB fdb = new FinancialLimitDB();
                List<financiallimit> finList = fdb.getEmpFinancialLimit();
                foreach (financiallimit flim in finList)
                {
                    grdList.Rows.Add(flim.DocumentID, flim.DocumentName,
                        flim.EmployeeName + "-" + flim.EmployeeID, flim.FinancialLimit,
                         ComboFIll.getStatusString(flim.DocumentStatus));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            enableBottomButtons();
            pnlDocumentList.Visible = true;
        }

        private void initVariables()
        {
            try
            {
                
                fillDocumentStatusCombo(cmbDocumentStatus);
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
                cmbDocumentStatus.SelectedIndex = -1;
                txtEmployee.Text = "";
                txtDocument.Text = "";
                txtFinancialLimit.Text = "";
                prevflim = new financiallimit();
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
                txtEmployee.Enabled = true;
                txtDocument.Enabled = true;
                btnEmployee.Enabled = true;
                btnDocument.Enabled = true;
                txtFinancialLimit.Text = "";
                txtEmployee.Text = "";
                txtDocument.Text = "";
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
                financiallimit flist = new financiallimit();
                FinancialLimitDB fdb = new FinancialLimitDB();


                try
                {
                    string[] docmts = txtDocument.Text.Trim().Split('-');
                    string[] emply = txtEmployee.Text.Trim().Split('-');
                    ////////flist.DocumentID = cmbDocument.SelectedItem.ToString().Trim().Substring(0, cmbDocument.SelectedItem.ToString().Trim().IndexOf('-'));
                    flist.DocumentID = docmts[1];
                    ////////flist.DocumentName = cmbDocument.SelectedItem.ToString().Trim().Substring(cmbDocument.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                    flist.DocumentName = docmts[0];
                    //flist.EmployeeName = txtEmployee.Text.Trim().Substring(0, txtEmployee.Text.Trim().IndexOf('-'));
                    //flist.EmployeeID = txtEmployee.Text.Trim().Substring(txtEmployee.Text.Trim().IndexOf('-') + 1);
                    flist.EmployeeName = emply[0];
                    flist.EmployeeID = emply[1];
                    flist.FinancialLimit = Convert.ToDouble(txtFinancialLimit.Text);
                }
                catch (Exception ex)
                {
                    flist.DocumentID = "";
                    flist.DocumentName = "";
                    flist.EmployeeName = "";
                    flist.EmployeeID = "";
                }

                flist.DocumentStatus = ComboFIll.getStatusCode(cmbDocumentStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;

                {
                    if (btnText.Equals("Update"))
                    {
                        if (fdb.updateFinancialLimit(flist, prevflim))
                        {
                            MessageBox.Show("DocEmpMapping Status updated");
                            closeAllPanels();
                            ListEmpFinLimit();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update DocEmpMapping Status");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (fdb.validateFinancialLimit(flist))
                        {
                            if (fdb.insertFinancialLimit(flist))
                            {
                                MessageBox.Show("EMP Financial list data Added");
                                closeAllPanels();
                                ListEmpFinLimit();
                            }
                            else
                            {
                                MessageBox.Show("Failed to Insert DocEmpMapping");
                            }
                        }
                        else
                        {
                            MessageBox.Show("DocEmpMapping Data Validation failed");
                        }
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
                if (e.ColumnIndex == 5)
                {
                    clearDocumentData();
                    prevflim = new financiallimit();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlDocumentInner.Visible = true;
                    pnlDocumentOuter.Visible = true;
                    pnlDocumentList.Visible = false;
                    prevflim.DocumentID = grdList.Rows[e.RowIndex].Cells["DocID"].Value.ToString();
                    prevflim.EmployeeID = grdList.Rows[e.RowIndex].Cells["EmpName"].Value.ToString().Substring(grdList.Rows[e.RowIndex].Cells["EmpName"].Value.ToString().Trim().IndexOf('-') + 1);
                    prevflim.FinancialLimit = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["FinLimit"].Value.ToString());
                    ////////cmbDocument.SelectedIndex = cmbDocument.FindStringExact(grdList.Rows[e.RowIndex].Cells["DocID"].Value.ToString() + "-" + grdList.Rows[e.RowIndex].Cells["DocName"].Value.ToString());
                    txtDocument.Text= grdList.Rows[e.RowIndex].Cells["DocName"].Value.ToString()+"-"+grdList.Rows[e.RowIndex].Cells["DocID"].Value.ToString();

                    txtEmployee.Text = grdList.Rows[e.RowIndex].Cells["EmpName"].Value.ToString();
                    txtFinancialLimit.Text = grdList.Rows[e.RowIndex].Cells["FinLimit"].Value.ToString();
                    cmbDocumentStatus.SelectedIndex = cmbDocumentStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells["empStatus"].Value.ToString());
                    txtEmployee.Enabled = false;
                    txtDocument.Enabled = false;
                    btnEmployee.Enabled = false;
                    btnDocument.Enabled = false;
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

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            showEmployeeDataGridView("");
        }

        private void showEmployeeDataGridView(string Office)
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
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInEmpGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                EmployeeDB empDB = new EmployeeDB();
                grdEmpList = empDB.getEmployeelistGrid(Office);

                grdEmpList.Bounds = new Rectangle(new Point(0, 27), new Size(500, 300));
                frmPopup.Controls.Add(grdEmpList);
                grdEmpList.Columns["OfficeID"].Visible = false;
                grdEmpList.Columns["UserID"].Visible = false;
                grdEmpList.Columns["EmployeeName"].Width = 180;
                grdEmpList.Columns["OfficeName"].Width = 150;
                foreach (DataGridViewColumn column in grdEmpList.Columns)
                    column.SortMode = DataGridViewColumnSortMode.Automatic;

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grdOK_Click1);
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
        private void filterGridData()
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
                        if (!row.Cells["EmployeeName"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
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

        private void txtSearch_TextChangedInEmpGridList(object sender, EventArgs e)
        {
            try
            {
                //grdForStockList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                filterGridData();
                ///grdForStockList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (Exception ex)
            {

            }
        }

        private void grdOK_Click1(object sender, EventArgs e)
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
                    MessageBox.Show("Select one Employee");
                    return;
                }

                foreach (var row in checkedRows)
                {
                        txtEmployee.Text = row.Cells["EmployeeName"].Value.ToString() + '-' + row.Cells["EmployeeID"].Value.ToString();
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

        private void btnDocument_Click(object sender, EventArgs e)
        {
            showDocumentDataGridView();
        }

        private void FinancialLimit_Enter(object sender, EventArgs e)
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

