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
    public partial class AutoJVAccMapping : System.Windows.Forms.Form
    {
        jvaccmapping prevdoc;
        employee emp = new employee();
        Form frmPopup = new Form();
        Timer filterTimer = new Timer();
        DataGridView AccCodeGrd = new DataGridView();
        DataGridView grdEmpList = new DataGridView();
        TextBox txtSearch = new TextBox();
        public AutoJVAccMapping()
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
            Listjvaccmapping();
            applyPrivilege();
        }
        ////private void Form1_Load(object sender, EventArgs e)
        ////{
        ////    try
        ////    {
        ////        Listjvaccmapping();
        ////    }
        ////    catch (Exception)
        ////    {

        ////    }

        ////}
        private void Listjvaccmapping()
        {
            try
            {
                grdList.Rows.Clear();
                AutoJVAccMappingDB dbrecord = new AutoJVAccMappingDB();
                List<jvaccmapping> jvaccmappings = dbrecord.getjvaccmappingList();
                foreach (jvaccmapping doc in jvaccmappings)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["RowID"].Value = doc.RowID;
                    grdList.Rows[grdList.RowCount - 1].Cells["JVName"].Value = doc.JVName;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentID"].Value = doc.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentName"].Value = doc.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["AccountCodeDebit"].Value = doc.AccountCodeDebit;
                    grdList.Rows[grdList.RowCount - 1].Cells["AccountNameDebit"].Value = doc.AccountNameDebit;
                    grdList.Rows[grdList.RowCount - 1].Cells["AccountCodeCredit"].Value = doc.AccountCodeCredit;
                    grdList.Rows[grdList.RowCount - 1].Cells["AccountNameCredit"].Value = doc.AccountNameCredit;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = doc.Status;
                    if(doc.Status == 0)
                        grdList.Rows[grdList.RowCount - 1].Cells["StatusString"].Value = "Deactive";
                    else
                        grdList.Rows[grdList.RowCount - 1].Cells["StatusString"].Value = "Active";
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

        private void fillDocumentStatusCombo(System.Windows.Forms.ComboBox cmb)
        {
            try
            {
                Structures.ComboBoxItem item = new Structures.ComboBoxItem("Active", "1");
                Structures.ComboBoxItem item1 = new Structures.ComboBoxItem("Deactive", "0");
                cmbStatus.Items.Clear();
                cmbStatus.Items.Add(item);
                cmbStatus.Items.Add(item1);
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
                txtJVName.Text = "";
                txtDocumentID.Text = "";
                txtAccountNameCredit.Text = "";
                txtAccountNameDebit.Text = "";
                txtDocName.Text = "";
                txtAccountCodeDebit.Text = "";
                txtAccountCodeCredit.Text = "";
                prevdoc = new jvaccmapping();
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
                btnDocument.Visible = true;
                txtJVName.ReadOnly = false;
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
                    txtDocumentID.Text = row.Cells["DocumentID"].Value.ToString();
                    txtDocName.Text = row.Cells["DocumentName"].Value.ToString();
                }

                frmPopup.Close();
                frmPopup.Dispose();
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
            }
            catch (Exception)
            {
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                jvaccmapping doc = new jvaccmapping();
                AutoJVAccMappingDB docDB = new AutoJVAccMappingDB();
                try
                {
                    doc.JVName = txtJVName.Text.Trim();
                    doc.DocumentID = txtDocumentID.Text;
                    doc.DocumentName = txtDocName.Text;
                    doc.AccountCodeDebit =txtAccountCodeDebit.Text;
                    doc.AccountCodeCredit = txtAccountCodeCredit.Text;
                    doc.AccountNameDebit = txtAccountNameDebit.Text;
                    doc.AccountNameCredit = txtAccountNameCredit.Text;
                }
                catch (Exception)
                {
                    doc.JVName = "";
                    doc.DocumentID = "";
                    doc.DocumentName = "";
                    doc.AccountCodeDebit = "";
                    doc.AccountCodeCredit = "";
                    doc.AccountNameDebit = "";
                    doc.AccountNameCredit = "";
                }

                doc.Status = Convert.ToInt32(((Structures.ComboBoxItem)cmbStatus.SelectedItem).HiddenValue);
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                {
                    if (btnText.Equals("Update"))
                    {
                        if (docDB.updatejvaccmapping(doc, prevdoc))
                        {
                            MessageBox.Show("JV Account mapping updated");
                            closeAllPanels();
                            Listjvaccmapping();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update JV Account mapping ");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (docDB.validateDocument(doc))
                        {
                            if (docDB.insertAutoJVAccountCodes(doc))
                            {
                                MessageBox.Show("JV Account mapping Added");
                                closeAllPanels();
                                Listjvaccmapping();
                            }
                            else
                            {
                                MessageBox.Show("Failed to Insert JV Account mapping ");
                            }
                        }
                        else
                        {
                            MessageBox.Show("JV Account mapping  Data Validation failed");
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
                string colName = grdList.Columns[e.ColumnIndex].Name;
                if (colName == "Edit")
                {
                    clearDocumentData();
                    btnDocument.Visible = false;
                    txtJVName.ReadOnly = true;
                    prevdoc = new jvaccmapping();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlDocumentInner.Visible = true;
                    pnlDocumentOuter.Visible = true;
                    pnlDocumentList.Visible = false;
                    prevdoc.RowID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["RowID"].Value.ToString());
                    prevdoc.DocumentID = grdList.Rows[e.RowIndex].Cells["DocumentID"].Value.ToString();
                    prevdoc.JVName = grdList.Rows[e.RowIndex].Cells["JVName"].Value.ToString();
                    prevdoc.DocumentName = grdList.Rows[e.RowIndex].Cells["DocumentName"].Value.ToString();
                    prevdoc.AccountCodeDebit = grdList.Rows[e.RowIndex].Cells["AccountCodeDebit"].Value.ToString();
                    prevdoc.AccountNameDebit = grdList.Rows[e.RowIndex].Cells["AccountNameDebit"].Value.ToString();
                    prevdoc.AccountCodeCredit = grdList.Rows[e.RowIndex].Cells["AccountCodeCredit"].Value.ToString();
                    prevdoc.AccountNameCredit = grdList.Rows[e.RowIndex].Cells["AccountNameCredit"].Value.ToString();

                    prevdoc.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString());
                    txtJVName.Text = grdList.Rows[e.RowIndex].Cells["JVName"].Value.ToString();
                    txtDocumentID.Text = grdList.Rows[e.RowIndex].Cells["DocumentID"].Value.ToString() ;
                    txtDocName.Text = grdList.Rows[e.RowIndex].Cells["DocumentName"].Value.ToString();
                    txtAccountCodeDebit.Text = grdList.Rows[e.RowIndex].Cells["AccountCodeDebit"].Value.ToString();
                    txtAccountNameDebit.Text = grdList.Rows[e.RowIndex].Cells["AccountNameDebit"].Value.ToString();
                    txtAccountCodeCredit.Text = grdList.Rows[e.RowIndex].Cells["AccountCodeCredit"].Value.ToString();
                    txtAccountNameCredit.Text = grdList.Rows[e.RowIndex].Cells["AccountNameCredit"].Value.ToString();
                    cmbStatus.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbStatus, prevdoc.Status.ToString());
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

        private void btnAccDebit_Click(object sender, EventArgs e)
        {
            showDebitAccountCodeDataGridView();
        }

        private void showDebitAccountCodeDataGridView()
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
                lblSearch.Location = new System.Drawing.Point(200, 5);
                lblSearch.AutoSize = true;
                lblSearch.Text = "Search by Name";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(320, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInAccCodeGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                AccCodeGrd = AccountCodeDB.getGridViewForAccountCode();
                AccCodeGrd.Bounds = new Rectangle(new Point(0, 27), new Size(550, 300));
                frmPopup.Controls.Add(AccCodeGrd);
                AccCodeGrd.Columns["AccountName"].Width = 380;

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grdAccCOdeOK_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 335);
                lvCancel.Click += new System.EventHandler(this.grdAccCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdAccCOdeOK_Click1(object sender, EventArgs e)
        {
            string users = "";
            try
            {
                var checkedRows = from DataGridViewRow r in AccCodeGrd.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("Select one AccCode");
                    return;
                }

                foreach (var row in checkedRows)
                {
                    txtAccountCodeDebit.Text = row.Cells["AccountCode"].Value.ToString();
                    txtAccountNameDebit.Text = row.Cells["AccountName"].Value.ToString();
                }

                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void txtSearch_TextChangedInAccCodeGridList(object sender, EventArgs e)
        {
            try
            {
                filterTimer.Enabled = false;
                filterTimer.Stop();
                filterTimer.Tick -= new System.EventHandler(this.handlefilterAccTimerTimeout);
                filterTimer.Tick += new System.EventHandler(this.handlefilterAccTimerTimeout);
                filterTimer.Interval = 500;
                filterTimer.Enabled = true;
                filterTimer.Start();
            }
            catch (Exception ex)
            {

            }
        }
        private void handlefilterAccTimerTimeout(object sender, EventArgs e)
        {
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterAccCodeGridData();
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }
        private void filterAccCodeGridData()
        {
            try
            {
                AccCodeGrd.CurrentCell = null;
                foreach (DataGridViewRow row in AccCodeGrd.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in AccCodeGrd.Rows)
                    {
                        if (!row.Cells["AccountName"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
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
        private void grdAccCancel_Click1(object sender, EventArgs e)
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
        private void btnAccCredit_Click(object sender, EventArgs e)
        {
            showCreditAccountCodeDataGridView();
        }
        private void showCreditAccountCodeDataGridView()
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
                lblSearch.Location = new System.Drawing.Point(200, 5);
                lblSearch.AutoSize = true;
                lblSearch.Text = "Search by Name";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(320, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInAccCodeGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                AccCodeGrd = AccountCodeDB.getGridViewForAccountCode();
                AccCodeGrd.Bounds = new Rectangle(new Point(0, 27), new Size(550, 300));
                frmPopup.Controls.Add(AccCodeGrd);
                AccCodeGrd.Columns["AccountName"].Width = 380;

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grdAccCOdeOKCredit_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 335);
                lvCancel.Click += new System.EventHandler(this.grdAccCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdAccCOdeOKCredit_Click1(object sender, EventArgs e)
        {
            string users = "";
            try
            {
                var checkedRows = from DataGridViewRow r in AccCodeGrd.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("Select one AccCode");
                    return;
                }

                foreach (var row in checkedRows)
                {
                    txtAccountCodeCredit.Text = row.Cells["AccountCode"].Value.ToString();
                    txtAccountNameCredit.Text = row.Cells["AccountName"].Value.ToString();
                }

                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void AutoJVAccMapping_Enter(object sender, EventArgs e)
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

