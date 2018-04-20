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
    public partial class SubDocumentReceiver : System.Windows.Forms.Form
    {
        subdocreceiver prevdoc;
        Form frmPopup = new Form();
        DataGridView grdEmpList = new DataGridView(); 
        DataGridView grdgrdSubList = new DataGridView();
        TextBox txtSearch = new TextBox();
        public SubDocumentReceiver()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void DocumentReceiver_Load(object sender, EventArgs e)
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
            ListSubDocumentReceiver();
            applyPrivilege();
        }
        private void ListSubDocumentReceiver()
        {
            try
            {
                grdList.Rows.Clear();
                SubDocumentReceiverDB drdb = new SubDocumentReceiverDB();
                List<subdocreceiver> DocumentReceivers = drdb.getsubdocreceiverList();
                foreach (subdocreceiver drrec in DocumentReceivers)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.Rows.Count - 1].Cells["RowID"].Value = drrec.RowID;
                    grdList.Rows[grdList.Rows.Count - 1].Cells["DocumentID"].Value = drrec.DocumentID;
                    grdList.Rows[grdList.Rows.Count - 1].Cells["DocumentName"].Value = drrec.DocumentName;
                    grdList.Rows[grdList.Rows.Count - 1].Cells["EmployeeID"].Value = drrec.EmployeeID;
                    grdList.Rows[grdList.Rows.Count - 1].Cells["Employee"].Value = drrec.EmployeeName;
                    grdList.Rows[grdList.Rows.Count - 1].Cells["SubDocumentID"].Value = drrec.SubDocID;
                    grdList.Rows[grdList.Rows.Count - 1].Cells["Status"].Value = getDocStatusString(drrec.Status);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            enableBottomButtons();
            pnlDocumentList.Visible = true;
        }
        private string getDocStatusString(int code)
        {
            string status = "";
            if (code == 1)
                status = "Active";
            else
                status = "Deactive";
            return status;
        }
        private void initVariables()
        {
            try
            {
                fillDocumentStatusCombo(cmbStatus);
                CatalogueValueDB.fillCatalogValueComboNew(cmbSubDOc, "SEFType");
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
                txtDocument.Text = "";
                txtEmployee.Text = "";
                cmbSubDOc.SelectedIndex = -1;
                prevdoc = new subdocreceiver();
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
                txtDocument.Text = "Internal Order for Product-IOPRODUCT";
                pnlDocumentOuter.Visible = true;
                pnlDocumentInner.Visible = true;
                txtDocument.Enabled = true;
                btnSelDoc.Enabled = true;
                disableBottomButtons();
            }
            catch (Exception)
            {

            }
        }
        private int getStatusCode(string codeStr)
        {
            int status = 0;
            if (codeStr == "Active")
            {
                status = 1;
            }
            else if(codeStr == "Deactive")
            {
                status = 0;
            }
            return status;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                subdocreceiver drrec = new subdocreceiver();
                SubDocumentReceiverDB drDB = new SubDocumentReceiverDB();
                try
                {
                    string[] docmts = txtDocument.Text.Split('-');
                    string[] emply = txtEmployee.Text.Split('-');
                    drrec.DocumentID = docmts[1];
                    drrec.EmployeeID = emply[1];
                    drrec.SubDocID = ((Structures.ComboBoxItem)cmbSubDOc.SelectedItem).HiddenValue;
                }
                catch (Exception)
                {
                    drrec.DocumentID = "";
                    drrec.DocumentName = "";
                    drrec.EmployeeName = "";
                    drrec.EmployeeID = "";
                    drrec.SubDocID = "";
                }

                drrec.Status = getStatusCode(cmbStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;

                {
                    if (btnText.Equals("Update"))
                    {
                        if (drDB.updateSubdocReceiver(drrec, prevdoc))
                        {
                            MessageBox.Show("Sub Document Receiver Status updated");
                            closeAllPanels();
                            ListSubDocumentReceiver(); 
                        }
                        else
                        {
                            MessageBox.Show("Failed to update Document Receiver Status");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (drDB.validateDocument(drrec))
                        {
                            if (drDB.insertSubDocumentReceivers(drrec))
                            {
                                MessageBox.Show("Sub Document Receiver Added");
                                closeAllPanels();
                                ListSubDocumentReceiver();
                            }
                            else
                            {
                                MessageBox.Show("Failed to Insert Sub Document Receiver");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Sub Document Receiver Validation failed");
                        }
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing Sub Document Receiver");
            }

        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            try
            {
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit"))
                {
                    clearDocumentData();
                    prevdoc = new subdocreceiver();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlDocumentInner.Visible = true;
                    pnlDocumentOuter.Visible = true;
                    pnlDocumentList.Visible = false;
                    prevdoc.RowID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["RowID"].Value);
                    prevdoc.DocumentID = grdList.Rows[e.RowIndex].Cells["DocumentID"].Value.ToString();
                    prevdoc.EmployeeID = grdList.Rows[e.RowIndex].Cells["EmployeeID"].Value.ToString();
                    prevdoc.SubDocID = grdList.Rows[e.RowIndex].Cells["SubDocumentID"].Value.ToString();

                    cmbSubDOc.SelectedIndex =
                                           Structures.ComboFUnctions.getComboIndex(cmbSubDOc, prevdoc.SubDocID);
                    txtDocument.Text = grdList.Rows[e.RowIndex].Cells["DocumentName"].Value.ToString() + "-" +
                                            grdList.Rows[e.RowIndex].Cells["DocumentID"].Value.ToString();
                    txtEmployee.Text = grdList.Rows[e.RowIndex].Cells["Employee"].Value.ToString() + "-" +
                                          grdList.Rows[e.RowIndex].Cells["EmployeeID"].Value.ToString();
                    cmbStatus.SelectedIndex = cmbStatus.FindString(grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString());
                    txtDocument.Enabled = false;
                    btnSelDoc.Enabled = false;
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

        private void btnSelDoc_Click(object sender, EventArgs e)
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

                frmPopup.Size = new Size(520, 370);

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

                grdEmpList.Bounds = new Rectangle(new Point(0, 27), new Size(520, 300));
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
                lvOK.Click += new System.EventHandler(this.grdEmpOK_Click1);
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
            }
            catch (Exception)
            {
            }
        }



        private void grdEmpOK_Click1(object sender, EventArgs e)
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

        private void btSelEmp_Click(object sender, EventArgs e)
        {
            showEmployeeDataGridView("");
        }
        private void txtSearch_TextChangedInDocGridList(object sender, EventArgs e)
        {
            try
            {
                filterGridDocData();
            }
            catch (Exception ex)
            {

            }
        }

        private void txtSearch_TextChangedInEmpGridList(object sender, EventArgs e)
        {
            try
            {
                filterGridData();
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
            }
            catch (Exception)
            {
            }
        }

        private void btnSelSubDoc_Click(object sender, EventArgs e)
        {
        }

        private void SubDocumentReceiver_Enter(object sender, EventArgs e)
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

