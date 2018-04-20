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
    public partial class DocTCMapping : System.Windows.Forms.Form
    {
        doctcmapping prevdoc;
        employee emp = new employee();
        Form frmPopup = new Form();
        DataGridView grdEmpList = new DataGridView();
        DataGridView tclist = new DataGridView();
        TextBox txtSearch = new TextBox();

        public DocTCMapping()
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
                DocTcMappingDB dbrecord = new DocTcMappingDB();
                List<doctcmapping> TCMappings = dbrecord.getTCMapping();

                foreach (doctcmapping doc in TCMappings)
                {
                    grdList.Rows.Add(doc.DocumentID, doc.DocumentName);
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
                //cmbDocumentStatus.SelectedIndex = 0;
                txtDocument.Text = "";
                //txtTc.Text = "";
                prevdoc = new doctcmapping();
                tclist.Rows.Clear();
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
                tclist.Visible = false;
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

        private void showTCDataGridView()
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

                frmPopup.Size = new Size(685, 370);

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
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInTCGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                DocTcMappingDB TcDB = new DocTcMappingDB();
                grdEmpList = TcDB.getTclistGrid(txtDocument.Text);

                if (grdEmpList.Rows.Count > 0)
                {
                    foreach (DataGridViewRow rows in tclist.Rows)
                    {
                        foreach (DataGridViewRow rw in grdEmpList.Rows)
                        {
                            if (rw.Cells["Heading"].Value.ToString() == rows.Cells["Header"].Value.ToString())
                            {
                                rw.Cells["Select"].Value = true;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("This Document has no Terms and conditions!!!");
                    return;
                }




                grdEmpList.Bounds = new Rectangle(new Point(0, 27), new Size(680, 300));
                frmPopup.Controls.Add(grdEmpList);
                grdEmpList.Columns["DocumentID"].Visible = false;
                grdEmpList.Columns["DocumentName"].Visible = false;
                grdEmpList.Columns["ReferenceTC"].Visible = false;
                grdEmpList.Columns["RowID"].Visible = false;
                grdEmpList.Columns["ParagraphID"].Visible = false;
                grdEmpList.Columns["Heading"].Width = 130;
                grdEmpList.Columns["Detail"].Width = 499;
                grdEmpList.RowTemplate.Height = 80;


                foreach (DataGridViewColumn column in grdEmpList.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.Automatic;
                    column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                }
                grdEmpList.CurrentCell.Selected = false;
                grdEmpList.Columns["Heading"].ReadOnly = true;
                grdEmpList.Columns["Detail"].ReadOnly = true;
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
                    txtDocument.Text = row.Cells["DocumentID"].Value.ToString();
                }


                creategrid();
                if (tclist.RowCount >= 0)
                {
                    tclist.Visible = true;
                }


                frmPopup.Close();
                frmPopup.Dispose();
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
                if (selectedRowCount == 0)
                {
                    MessageBox.Show("Select one item");
                    return;
                }
                if (tclist.ColumnCount <= 0)
                {
                    tclist.Columns.Clear();
                    tclist.Columns.Add("RowID", "RowID");
                    tclist.Columns.Add("Header", "Header");
                    tclist.Columns.Add("Detail", "Detail");
                }

                tclist.Rows.Clear();
                foreach (var row in checkedRows)
                {
                    tclist.Rows.Add();
                    tclist.Rows[tclist.RowCount - 1].Cells["RowID"].Value = row.Cells["RowID"].Value.ToString();
                    tclist.Rows[tclist.RowCount - 1].Cells["Header"].Value = row.Cells["Heading"].Value.ToString();
                    tclist.Rows[tclist.RowCount - 1].Cells["Detail"].Value = row.Cells["Detail"].Value.ToString();
                    tclist.Rows[tclist.RowCount - 1].Height = 60;
                    tclist.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                }
                //tclist.CurrentCell.Selected = false;
                //txtTc.Text = trlist;
                frmPopup.Close();
                tclist.Visible = true;
                frmPopup.Dispose();

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

        private void txtSearch_TextChangedInTCGridList(object sender, EventArgs e)
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
                        if (!row.Cells["Heading"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
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

        private Boolean createAndUpdateTermsAndConditions(DocTcMappingDB tcm)
        {
            Boolean status = true;
            try
            {
                DocTcMappingDB tcdb = new DocTcMappingDB();
                doctcmapping tc = new doctcmapping();
                int count = 0;
                List<doctcmapping> TCMDetails = new List<doctcmapping>();
                foreach (DataGridViewRow row in tclist.Rows)
                {
                    try
                    {
                        tc = new doctcmapping();
                        tc.DocumentID = txtDocument.Text;
                        tc.ReferenceTC = row.Cells["RowID"].Value.ToString();
                        TCMDetails.Add(tc);
                        count++;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("createAndUpdateTermsAndConditions() : Error creating Terms and Conditions mapping");
                        status = false;
                    }
                }
                if (count == 0)
                {
                    MessageBox.Show("No item selected");
                    status = false;
                }
                else
                {
                    if (!tcdb.UpdateTCMapping(TCMDetails, tc))
                    {
                        MessageBox.Show("createAndUpdateTermsAndConditions() : Failed to update Terms and Conditions mapping. Please check the values");
                        status = false;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("createAndUpdateTermsAndConditions() : Error updating Terms and Conditions.");
                status = false;
            }
            return status;
        }





        private void btnSave_Click(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {
                DocTcMappingDB tc = new DocTcMappingDB();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;
                if (btnText.Equals("Save") || btnText.Equals("Update"))
                {
                    if (createAndUpdateTermsAndConditions(tc))
                    {
                        MessageBox.Show("Terms and Conditions updated");
                        closeAllPanels();
                        ListDocEmpMapping();
                    }
                    else
                    {
                        status = false;
                    }

                }
                if (!status)
                {
                    MessageBox.Show("Failed to update Terms and condition mappiing");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("createAndUpdateTermsAndConditions() : Error");
            }
        }




        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            try
            {
                if (e.ColumnIndex == 3)
                {
                    clearDocumentData();
                    btnDocument.Enabled = false;
                    prevdoc = new doctcmapping();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlDocumentInner.Visible = true;
                    pnlDocumentOuter.Visible = true;
                    pnlDocumentList.Visible = false;
                    prevdoc.DocumentID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtDocument.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    //string[] termncond = grdList.Rows[e.RowIndex].Cells[2].Value.ToString().Split('-');
                    creategrid();
                    //txtTc.Text = grdList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    disableBottomButtons();
                }
            }
            catch (Exception)
            {

            }
        }


        public void creategrid()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            tclist.EnableHeadersVisualStyles = false;

            tclist.AllowUserToAddRows = false;
            tclist.AllowUserToDeleteRows = false;
            tclist.BackgroundColor = System.Drawing.Color.White;
            tclist.BorderStyle = System.Windows.Forms.BorderStyle.None;
            tclist.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            tclist.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            tclist.ColumnHeadersHeight = 27;
            tclist.RowHeadersVisible = false;
            tclist.Columns.Clear();
            tclist.Columns.Add("RowID", "RowID");
            tclist.Columns.Add("Header", "Header");
            tclist.Columns.Add("Detail", "Detail");
            tclist.Bounds = new Rectangle(new Point(140, 60), new Size(820, 300));

            pnlDocumentInner.Controls.Add(tclist);
            tclist.Columns["RowID"].Visible = false;
            tclist.Columns["Header"].Width = 200;
            tclist.Columns["Detail"].Width = 600;

            DocTcMappingDB dbrecord = new DocTcMappingDB();
            List<doctcmapping> TCdocMappings = dbrecord.getdocTCList(txtDocument.Text);
            if (TCdocMappings.Count != 0)
            {
                foreach (doctcmapping dm in TCdocMappings)
                {
                    tclist.Rows.Add();
                    tclist.Rows[tclist.RowCount - 1].Cells["RowID"].Value = dm.RowID;
                    tclist.Rows[tclist.RowCount - 1].Cells["Header"].Value = dm.Heading;
                    tclist.Rows[tclist.RowCount - 1].Cells["Detail"].Value = dm.Detail;
                    tclist.Rows[tclist.RowCount - 1].Height = 60;
                    tclist.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                }
                tclist.CurrentCell.Selected = false;
                foreach (DataGridViewColumn column in tclist.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.Automatic;

                }

                tclist.ReadOnly = true;

                tclist.Visible = true;
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
            tclist.Rows.Clear();
            tclist.Visible = false;
            showDocumentDataGridView();
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            if (txtDocument.Text.Trim().Length != 0)
            {
                showTCDataGridView();
            }
            else
            {
                MessageBox.Show("Please select the document");
            }

        }

        private void btnSnrEmployee_Click(object sender, EventArgs e)
        {
        }

        private void pnlDocumentInner_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DocTCMapping_Enter(object sender, EventArgs e)
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

        private void txtDocument_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

