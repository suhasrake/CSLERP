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
    public partial class DocEmpMapping : System.Windows.Forms.Form
    {
        docempmapping prevdoc;
        employee emp = new employee();
        Form frmPopup = new Form();
        DataGridView grdEmpList = new DataGridView();
        TextBox txtSearch = new TextBox();
        bool btn1click = false;
        bool btn2click = false;
        public DocEmpMapping()
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
            ListDocEmpMapping();
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
        private void ListDocEmpMapping()
        {
            try
            {
                grdList.Rows.Clear();
                DocEmpMappingDB dbrecord = new DocEmpMappingDB();
                List<docempmapping> DocEmpMappings = dbrecord.getDocEmpMapping();
                foreach (docempmapping doc in DocEmpMappings)
                {
                    grdList.Rows.Add(doc.DocumentID, doc.DocumentName,
                        doc.EmployeeName + "-" + doc.EmployeeID,
                        doc.SeniorEmployeeName + "-" + doc.SeniorEmployeeID,
                         ComboFIll.getStatusString(doc.DocumentStatus));
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

                fillDocumentStatusCombo(cmbDocumentStatus);
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
                cmbDocumentStatus.SelectedIndex = 0;
                txtDocument.Text = "";
                txtEmployee.Text = "";
                txtSnrEmployee.Text = "";
                prevdoc = new docempmapping();
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
                    if(btn1click)
                    {
                        txtEmployee.Text = row.Cells["EmployeeName"].Value.ToString() + '-' + row.Cells["EmployeeID"].Value.ToString();
                    }
                    else if(btn2click)
                    {
                        txtSnrEmployee.Text = row.Cells["EmployeeName"].Value.ToString() + '-' + row.Cells["EmployeeID"].Value.ToString();
                    }    
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
                docempmapping doc = new docempmapping();
                DocEmpMappingDB docDB = new DocEmpMappingDB();


                try
                {
                    string[] docmts = txtDocument.Text.Split('-');
                    string[] emply = txtEmployee.Text.Split('-');
                    string[] snremply = txtSnrEmployee.Text.Split('-');
                    doc.DocumentID = docmts[1];
                    doc.DocumentName = docmts[0];
                    doc.EmployeeID = emply[1];
                    doc.EmployeeName = emply[0];
                    doc.SeniorEmployeeID = snremply[1];
                    doc.SeniorEmployeeName = snremply[0];
                    //doc.DocumentID = ((Structures.ComboBoxItem)cmbDocument.SelectedItem).HiddenValue;
                    //////////doc.DocumentName = cmbDocument.SelectedItem.ToString().Trim().Substring(cmbDocument.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                    //doc.DocumentName = ((Structures.ComboBoxItem)cmbDocument.SelectedItem).ToString();
                    //doc.EmployeeName = cmbEmployee.SelectedItem.ToString().Trim().Substring(0, cmbEmployee.SelectedItem.ToString().Trim().IndexOf('-'));
                    //doc.EmployeeID = cmbEmployee.SelectedItem.ToString().Trim().Substring(cmbEmployee.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                    //doc.SeniorEmployeeName = cmbSeniorEmployee.SelectedItem.ToString().Trim().Substring(0, cmbSeniorEmployee.SelectedItem.ToString().Trim().IndexOf('-'));
                    //doc.SeniorEmployeeID = cmbSeniorEmployee.SelectedItem.ToString().Trim().Substring(cmbSeniorEmployee.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                }
                catch (Exception)
                {
                    doc.DocumentID = "";
                    doc.DocumentName = "";
                    doc.EmployeeName = "";
                    doc.EmployeeID = "";
                    doc.SeniorEmployeeName = "";
                    doc.SeniorEmployeeID = "";
                }

                doc.DocumentStatus = ComboFIll.getStatusCode(cmbDocumentStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;

                {
                    if (btnText.Equals("Update"))
                    {
                        if (docDB.updateDocEmpMapping(doc, prevdoc))
                        {
                            MessageBox.Show("DocEmpMapping Status updated");
                            closeAllPanels();
                            ListDocEmpMapping();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update DocEmpMapping Status");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (docDB.validateDocument(doc))
                        {
                            if (docDB.insertDocEmpMapping(doc))
                            {
                                MessageBox.Show("DocEmpMapping data Added");
                                closeAllPanels();
                                ListDocEmpMapping();
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
                    btnDocument.Enabled = false;
                    prevdoc = new docempmapping();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlDocumentInner.Visible = true;
                    pnlDocumentOuter.Visible = true;
                    pnlDocumentList.Visible = false;
                    prevdoc.DocumentID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    prevdoc.EmployeeID = grdList.Rows[e.RowIndex].Cells[2].Value.ToString().Substring(grdList.Rows[e.RowIndex].Cells[2].Value.ToString().Trim().IndexOf('-') + 1);
                    prevdoc.SeniorEmployeeID = grdList.Rows[e.RowIndex].Cells[3].Value.ToString().Substring(grdList.Rows[e.RowIndex].Cells[3].Value.ToString().Trim().IndexOf('-') + 1);
                    txtDocument.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString() + '-' + grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtEmployee.Text = grdList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    txtSnrEmployee.Text = grdList.Rows[e.RowIndex].Cells[3].Value.ToString();
                    ////////cmbDocument.SelectedIndex = cmbDocument.FindStringExact(grdList.Rows[e.RowIndex].Cells[0].Value.ToString() + "-" + grdList.Rows[e.RowIndex].Cells[1].Value.ToString());
                    //cmbDocument.SelectedIndex =
                    //    Structures.ComboFUnctions.getComboIndex(cmbDocument, grdList.Rows[e.RowIndex].Cells[0].Value.ToString());
                    //cmbEmployee.SelectedIndex = cmbEmployee.FindStringExact(grdList.Rows[e.RowIndex].Cells[2].Value.ToString());
                    //cmbSeniorEmployee.SelectedIndex = cmbSeniorEmployee.FindStringExact(grdList.Rows[e.RowIndex].Cells[3].Value.ToString());
                    //cmbDocumentStatus.SelectedIndex = cmbDocumentStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[4].Value.ToString());
                    //cmbDocument.Enabled = false;
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

        private void btnDocument_Click(object sender, EventArgs e)
        {
            showDocumentDataGridView();
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            btn1click = true;
            btn2click = false;
            showEmployeeDataGridView("");
        }

        private void btnSnrEmployee_Click(object sender, EventArgs e)
        {
            btn1click = false;
            btn2click = true;
            showEmployeeDataGridView("");
        }

        private void DocEmpMapping_Enter(object sender, EventArgs e)
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

