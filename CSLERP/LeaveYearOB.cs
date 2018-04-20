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
using System.Collections;

namespace CSLERP
{
    public partial class LeaveYearOB : System.Windows.Forms.Form
    {
        int opt = 0;
        Boolean isEdit = false;
        string officeid = "";
        DateTime dt = UpdateTable.getSQLDateTime();
        Dictionary<string, string> LeaveDict = new Dictionary<string, string>();
        Panel pnlShowOBList = new System.Windows.Forms.Panel();
        DataGridView grdPnlShowOB = new DataGridView();
        TextBox txtsearch = new TextBox();
        Label lblsearch = new Label();
        Button btnOBListClose = new Button();
        Label lblYearSel = new Label();
        ComboBox cmbListYear = new ComboBox();
        Button btnExportToExcel = new Button();
        ListView exlv = new ListView();
        Form frmPopup = new Form();
        public LeaveYearOB()
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
            //grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            //grdList.EnableHeadersVisualStyles = false;
            //grdEmpWiseLeaveOB.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            //grdEmpWiseLeaveOB.EnableHeadersVisualStyles = false;
            applyPrivilege();
        }
      
        private void initVariables()
        {
            //StockGroupDB.fillGroupValueCombo(cmbGroup1, 1);
            //CatalogueValueDB.fillCatalogValueComboNew(cmbUnit, "StockUnit");
            fillYearCombo(cmbLeaveYear);
            cmbLeaveYear.SelectedIndex = -1;
            LeaveDict = getLeaveIDList();
            pnlShowLeaveOB.Visible = false;
            //lblsearch.Visible = false;
            //txtSearchGrd.Visible = false;
        }
        private Dictionary<string, string> getLeaveIDList()
        {
            Dictionary<string, string> idict = new Dictionary<string, string>();
            LeaveSettingsdb lvDB = new LeaveSettingsdb();
            List<Leave> lvlist = lvDB.getLeaveTypeList();
            foreach (Leave lv in lvlist)
                idict.Add(lv.leaveID, lv.description);
            return idict;
        }
        private void applyPrivilege()
        {
            try
            {
                if (Main.itemPriv[1])
                {
                    //btnSaveLeaveOB.Enabled = true;
                }
                else
                {
                    //btnSaveLeaveOB.Enabled = false;
                }
                if (Main.itemPriv[2])
                {
                    //grdEmpWiseLeaveOB.Columns["LeaveCount"].ReadOnly = false;
                }
                else
                {
                    //grdEmpWiseLeaveOB.Columns["LeaveCount"].ReadOnly = true;
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
                pnlShowLeaveOB.Visible = false;
                pnlList.Visible = false;
            }
            catch (Exception)
            {

            }
        }


        private void fillYearCombo(System.Windows.Forms.ComboBox cmb)
        {
            int currentYear = dt.Year;
            for (int i = currentYear ; i <= currentYear+1; i++)
            {
                cmb.Items.Add(i.ToString());
            }
        }

        public void clearUserData()
        {
            try
            {
                isEdit = false;
                //cmbLeaveYear.SelectedIndex = cmbLeaveYear.FindString(dt.Year.ToString());
                //grdEmpWiseLeaveOB.Rows.Clear();
                //lblEmployeename.Text = "";
                //lblEmployeeiD.Text = "";
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
        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
            }
            catch (Exception ex)
            {

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                clearUserData();
                ////enableBottomButtons();
                pnlList.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        private void txtSearchGrd_TextChanged(object sender, EventArgs e)
        {
            filterGridData();
        }
        private void filterGridData()
        {
            try
            {
                foreach (DataGridViewRow row in grdPnlShowOB.Rows)
                {
                    row.Visible = true;
                }
                if (txtsearch.Text.Length != 0)
                {

                    foreach (DataGridViewRow row in grdPnlShowOB.Rows)
                    {
                        if (!row.Cells["EmployeeName"].Value.ToString().Trim().ToLower().Contains(txtsearch.Text.Trim().ToLower()))
                        {
                            row.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSaveLeaveOB_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {
                MessageBox.Show("Failed Adding / Editing Leave Year OB Detail");
                return;
            }
        }

        private void btnCancelLeaveOB_Click(object sender, EventArgs e)
        {
            clearUserData();
            pnlShowLeaveOB.Visible = false;
        }

        private void cmbLeaveYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            int year = Convert.ToInt32(cmbLeaveYear.SelectedItem);
            if( year > DateTime.Now.Year)
            { 
                if (LeaveOBDB.checkfryear(year))
                {
                    showgrid();
                    pnlShowLeaveOB.Visible = false;
                }
                else
                {
                    DialogResult dialog = MessageBox.Show("OB Already exists for this year, Are you sure you want to replace?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        if (LeaveOBDB.Delfryear(year))
                        {
                            showgrid();
                            pnlShowLeaveOB.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("ERROR!! Deleting LeaveOB Row");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please select another year!!!");
                        //cmbLeaveYear.SelectedIndex = -1;
                        return;
                       
                    }
                }
            }
            else if(cmbLeaveYear.SelectedIndex != -1)
            {
                MessageBox.Show("Leave OB cannot be Altered for this Year!!!");
                cmbLeaveYear.SelectedIndex = -1;
                return;
            }
        }
        private void fillGriddetailRow()
        {
            try
            {
                //int bal = 0;
                //int leavetaken = 0;
                //int total = 0;
                //if (lblEmployeeiD.Text.Trim().Length == 0)
                //{
                //    MessageBox.Show("Error : Employee ID is Empty");
                //    return;
                //}
                //if (cmbLeaveYear.SelectedIndex == -1)
                //{
                //    MessageBox.Show("select Leave Year");
                //    return;
                //}
                //grdEmpWiseLeaveOB.Rows.Clear();
                //LeaveOBDB lobDB = new LeaveOBDB();
                //LeaveApproveDB lvAppDB = new LeaveApproveDB();
                //List<leaveob> lobList = lobDB.getLeaveOBDetails(lblEmployeeiD.Text, Convert.ToInt32(cmbLeaveYear.SelectedItem));
                //List<leaveob> officemax = lobDB.getOfficeMax(officeid);

                //int prevYear = Convert.ToInt32(cmbLeaveYear.SelectedItem) - 1;
                //List<leaveob> lobListPrev = lobDB.getLeaveOBDetails(lblEmployeeiD.Text, prevYear);

                ////Gives Carry Forward LEave Types
                //LeaveSettingsdb lvDB = new LeaveSettingsdb();
                //List<Leave> carryAvail = lvDB.getLeaveTypeList().Where(w => w.CarryForward == 1).ToList();

                //foreach (var item in LeaveDict)
                //{

                    //string leaveGender = LeaveOBDB.getGenderForLeaveType(item.Key);
                    //string empGender = LeaveOBDB.getGenderForEmployee(lblEmployeeiD.Text);
                    //if (leaveGender.Equals("All") || leaveGender.Equals(empGender))
                    //{
                       // grdEmpWiseLeaveOB.Rows.Add();
                       // //leaveob lob = lobList.FirstOrDefault(leave => leave.LeaveID == item.Key);
                       // grdEmpWiseLeaveOB.Rows[grdEmpWiseLeaveOB.RowCount - 1].Cells["LeaveID"].Value = item.Key;
                       // grdEmpWiseLeaveOB.Rows[grdEmpWiseLeaveOB.RowCount - 1].Cells["Description"].Value = item.Value;
                       //grdEmpWiseLeaveOB.Rows[grdEmpWiseLeaveOB.RowCount - 1].Cells["LeaveCount"].Value = 0;

                    //LEave Tabken by Employees
                    //List<leaveapprove> lvremain = lobDB.getLeaveRemainYearWise(lblEmployeeiD.Text, item.Key,prevYear);
                    //foreach (leaveapprove lv in lvremain)
                    //{
                    //    leavetaken += lv.leavepending;
                    //    leavetaken += 1;
                    //}
                    //int prevLVAvail = 0;
                    //leaveob lobPrev = lobListPrev.FirstOrDefault(leave => leave.LeaveID == item.Key);
                    //if (lobPrev != null)
                    //{
                    //    prevLVAvail = lobPrev.LeaveCount;
                    //}
                    //bal = prevLVAvail - leavetaken;

                    //leaveob offmax = officemax.FirstOrDefault(leave => leave.LeaveID == item.Key);
                    //if (offmax != null)
                    //{
                    //    total = bal + offmax.maxdays;
                    //}
                    //else
                    //{
                    //    total = bal;
                    //}

                    //if (lob != null)
                    //{
                    //Leave lvCarry = carryAvail.FirstOrDefault(lv => lv.leaveID == item.Key);

                    //if (lvCarry != null) // carry forward 1
                    //{
                    //    int val= lob.LeaveCount + total;
                    //    if (lvCarry.MaxAccrual > val)
                    //    {
                    //        grdEmpWiseLeaveOB.Rows[grdEmpWiseLeaveOB.RowCount - 1].Cells["LeaveCount"].Value = val;
                    //    }
                    //    else
                    //    {
                    //        grdEmpWiseLeaveOB.Rows[grdEmpWiseLeaveOB.RowCount - 1].Cells["LeaveCount"].Value = lvCarry.MaxAccrual;
                    //    }
                    //}
                    //else
                    //{
                    //grdEmpWiseLeaveOB.Rows[grdEmpWiseLeaveOB.RowCount - 1].Cells["LeaveCount"].Value = lob.LeaveCount;
                    //}

                    //}
                    //else
                    //{
                    //    Leave lvCarry = carryAvail.FirstOrDefault(lv => lv.leaveID == item.Key);

                    //    if (lvCarry != null) // carry forward 1
                    //    {
                    //        if (lvCarry.MaxAccrual > total)
                    //        {
                    //            grdEmpWiseLeaveOB.Rows[grdEmpWiseLeaveOB.RowCount - 1].Cells["LeaveCount"].Value = total;
                    //        }
                    //        else
                    //        {
                    //            grdEmpWiseLeaveOB.Rows[grdEmpWiseLeaveOB.RowCount - 1].Cells["LeaveCount"].Value = lvCarry.MaxAccrual;
                    //        }
                    //    }
                    //    else
                    //    {
                    //                grdEmpWiseLeaveOB.Rows[grdEmpWiseLeaveOB.RowCount - 1].Cells["LeaveCount"].Value = 0;
                    //            }

                    //        }
                    //    }
                    //grdEmpWiseLeaveOB.Visible = true;
                    //btnSaveLeaveOB.Visible = true;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Filling Leave OB Detail GridView");
            }
        }

        private void pnlShowLeaveOB_Paint(object sender, PaintEventArgs e)
        {

        }
        private void cmbListYear_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            grdPnlShowOB.Rows.Clear();
            grdPnlShowOB = getDynamicGridRowsnw(grdPnlShowOB);
            grdPnlShowOB.Refresh();
          

        }
        private void cmbListYear_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (LeaveOBDB.checkfryear(Convert.ToInt32(cmbListYear.SelectedItem)))
            {
                MessageBox.Show("No OB created for this year!!!");
                return;
            }
            grdPnlShowOB.Rows.Clear();        
            grdPnlShowOB = getDynamicGridRowsnw(grdPnlShowOB);
            grdPnlShowOB.Refresh();


        }
        private void btnShowOBList_Click(object sender, EventArgs e)
        {
            showgridnw();
        }
        public void showgrid()
        {
            try
            {
                
                btnExit.Visible = false;
                
                pnlShowOBList.Controls.Clear();
                pnlShowOBList = new Panel();
                pnlShowOBList.Location = new System.Drawing.Point(8, 17);
                pnlShowOBList.Name = "pnlShowOBList";
                pnlShowOBList.Size = new System.Drawing.Size(1100, 490);

                lblYearSel = new Label();
                lblYearSel.AutoSize = true;
                lblYearSel.Location = new System.Drawing.Point(25, 21);
                lblYearSel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblYearSel.Name = "lblYearSel";
                lblYearSel.Size = new System.Drawing.Size(62, 13);
                lblYearSel.Text = "Select Year";
                pnlShowOBList.Controls.Add(lblYearSel);

                lblsearch = new Label();
                lblsearch.AutoSize = true;
                lblsearch.Location = new System.Drawing.Point(839, 11);
                lblsearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblsearch.Name = "lblsearch";
                lblsearch.Size = new System.Drawing.Size(46, 15);
                lblsearch.Text = "Search";
                pnlShowOBList.Controls.Add(lblsearch);

                txtsearch = new TextBox();
                txtsearch.AutoSize = true;
                txtsearch.Location = new System.Drawing.Point(890, 10);
                txtsearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                txtsearch.Name = "lblsearch";
                txtsearch.Size = new System.Drawing.Size(176, 21);
                txtsearch.TextChanged += new System.EventHandler(this.txtSearchGrd_TextChanged);
                pnlShowOBList.Controls.Add(txtsearch);


                cmbListYear = new ComboBox();
                cmbListYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                cmbListYear.FormattingEnabled = true;
                cmbListYear.Location = new System.Drawing.Point(100, 18);
                cmbListYear.Name = "cmbYear";
                cmbListYear.Size = new System.Drawing.Size(121, 21);
                fillYearCombo(cmbListYear);
                cmbListYear.SelectedIndex = cmbListYear.FindString(dt.Year.ToString());
                cmbListYear.SelectedIndexChanged += new System.EventHandler(this.cmbListYear_SelectedIndexChanged);
                pnlShowOBList.Controls.Add(cmbListYear);

                grdPnlShowOB = new DataGridView();
                grdPnlShowOB.AllowUserToAddRows = false;
                grdPnlShowOB.AllowUserToDeleteRows = false;
                grdPnlShowOB.AllowUserToOrderColumns = true;
                grdPnlShowOB.RowHeadersVisible = false;
                grdPnlShowOB.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdPnlShowOB.EnableHeadersVisualStyles = false;
                grdPnlShowOB.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
                grdPnlShowOB.GridColor = System.Drawing.SystemColors.ActiveCaption;
                grdPnlShowOB.Location = new System.Drawing.Point(25, 48);
                grdPnlShowOB.Name = "grdPnlShowOB";
                grdPnlShowOB.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdPnlShowOB.BorderStyle = System.Windows.Forms.BorderStyle.None;
                //grdPnlShowOB.ColumnHeadersHeight = 25;
                grdPnlShowOB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                grdPnlShowOB.Size = new System.Drawing.Size(1020, 373);
                List<DataGridViewColumn> col = getListOFColumn();
                grdPnlShowOB.Columns.AddRange(col.ToArray());
                grdPnlShowOB = getDynamicGridRows(grdPnlShowOB);
                pnlShowOBList.Controls.Add(grdPnlShowOB);

                btnOBListClose = new Button();
                btnOBListClose.Location = new System.Drawing.Point(25, 430);
                btnOBListClose.Name = "btnOBListClose";
                btnOBListClose.Size = new System.Drawing.Size(75, 23);
                btnOBListClose.Text = "Close";
                btnOBListClose.UseVisualStyleBackColor = true;
                btnOBListClose.Click += new System.EventHandler(this.btnOBListClose_Click);

                btnExportToExcel = new Button();
                btnExportToExcel.Location = new System.Drawing.Point(112, 430);
                btnExportToExcel.Name = "btnExportToExcel";
                btnExportToExcel.Size = new System.Drawing.Size(95, 23);
                btnExportToExcel.Text = "Export To Excel";
                btnExportToExcel.UseVisualStyleBackColor = true;
                btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);

                pnlShowOBList.Controls.Add(btnOBListClose);
                pnlShowOBList.Controls.Add(btnExportToExcel);
                pnlUI.Controls.Add(pnlShowOBList);
                pnlShowOBList.Visible = true;
                pnlShowOBList.BringToFront();
            }
            catch (Exception ex)
            {
            }
        }

        public void showgridnw()
        {
            try
            {
                btnExit.Visible = false;

                pnlShowOBList.Controls.Clear();
                pnlShowOBList = new Panel();
                pnlShowOBList.Location = new System.Drawing.Point(8, 17);
                pnlShowOBList.Name = "pnlShowOBList";
                pnlShowOBList.Size = new System.Drawing.Size(1100, 490);

                lblYearSel = new Label();
                lblYearSel.AutoSize = true;
                lblYearSel.Location = new System.Drawing.Point(25, 21);
                lblYearSel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblYearSel.Name = "lblYearSel";
                lblYearSel.Size = new System.Drawing.Size(62, 13);
                lblYearSel.Text = "Select Year";
                pnlShowOBList.Controls.Add(lblYearSel);

                lblsearch = new Label();
                lblsearch.AutoSize = true;
                lblsearch.Location = new System.Drawing.Point(839, 11);
                lblsearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblsearch.Name = "lblsearch";
                lblsearch.Size = new System.Drawing.Size(46, 15);
                lblsearch.Text = "Search";
                pnlShowOBList.Controls.Add(lblsearch);

                txtsearch = new TextBox();
                txtsearch.AutoSize = true;
                txtsearch.Location = new System.Drawing.Point(890, 10);
                txtsearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                txtsearch.Name = "lblsearch";
                txtsearch.Size = new System.Drawing.Size(176, 21);
                txtsearch.TextChanged += new System.EventHandler(this.txtSearchGrd_TextChanged);
                pnlShowOBList.Controls.Add(txtsearch);


                cmbListYear = new ComboBox();
                cmbListYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
                cmbListYear.FormattingEnabled = true;
                cmbListYear.Location = new System.Drawing.Point(100, 18);
                cmbListYear.Name = "cmbYear";
                cmbListYear.Size = new System.Drawing.Size(121, 21);

                cmbListYear.Items.AddRange(LeaveOBDB.getyear().ToArray());
                
                cmbListYear.SelectedIndex = cmbListYear.FindString(dt.Year.ToString());
                cmbListYear.SelectedIndexChanged += new System.EventHandler(this.cmbListYear_SelectedIndexChanged1);
                pnlShowOBList.Controls.Add(cmbListYear);

                grdPnlShowOB = new DataGridView();
                grdPnlShowOB.AllowUserToAddRows = false;
                grdPnlShowOB.AllowUserToDeleteRows = false;
                grdPnlShowOB.AllowUserToOrderColumns = true;
                grdPnlShowOB.RowHeadersVisible = false;
                grdPnlShowOB.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdPnlShowOB.EnableHeadersVisualStyles = false;
                grdPnlShowOB.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
                grdPnlShowOB.GridColor = System.Drawing.SystemColors.ActiveCaption;
                grdPnlShowOB.Location = new System.Drawing.Point(25, 48);
                grdPnlShowOB.Name = "grdPnlShowOB";
                grdPnlShowOB.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdPnlShowOB.BorderStyle = System.Windows.Forms.BorderStyle.None;
                //grdPnlShowOB.ColumnHeadersHeight = 25;
                grdPnlShowOB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                grdPnlShowOB.Size = new System.Drawing.Size(1020, 373);
                List<DataGridViewColumn> col = getListOFColumn();
                grdPnlShowOB.Columns.AddRange(col.ToArray());
                grdPnlShowOB = getDynamicGridRowsnw(grdPnlShowOB);
                pnlShowOBList.Controls.Add(grdPnlShowOB);

                btnOBListClose = new Button();
                btnOBListClose.Location = new System.Drawing.Point(25, 430);
                btnOBListClose.Name = "btnOBListClose";
                btnOBListClose.Size = new System.Drawing.Size(75, 23);
                btnOBListClose.Text = "Close";
                btnOBListClose.UseVisualStyleBackColor = true;
                btnOBListClose.Click += new System.EventHandler(this.btnOBListClose_Click);

                btnExportToExcel = new Button();
                btnExportToExcel.Location = new System.Drawing.Point(112, 430);
                btnExportToExcel.Name = "btnExportToExcel";
                btnExportToExcel.Size = new System.Drawing.Size(95, 23);
                btnExportToExcel.Text = "Export To Excel";
                btnExportToExcel.UseVisualStyleBackColor = true;
                btnExportToExcel.Click += new System.EventHandler(this.btnExportToExcel_Click);

                pnlShowOBList.Controls.Add(btnOBListClose);
                pnlShowOBList.Controls.Add(btnExportToExcel);
                pnlUI.Controls.Add(pnlShowOBList);
                pnlShowOBList.Visible = true;
                pnlShowOBList.BringToFront();
            }
            catch (Exception ex)
            {
            }
        }

        private void btnOBListClose_Click(object sender, EventArgs e)
        {
            btnExit.Visible = true;
            pnlShowOBList.Visible = false;
            cmbLeaveYear.SelectedIndex = -1;
            pnlShowLeaveOB.Visible = false;
            pnlUI.Controls.Remove(pnlShowOBList);
        }
        private void btnExportToExcel_Click(object sender, EventArgs e)
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

                frmPopup.Size = new Size(450, 310);
                frmPopup.Location = new Point(0, 0);
                System.Windows.Forms.Label pnlHeading = new System.Windows.Forms.Label();
                pnlHeading.Size = new Size(300, 20);
                pnlHeading.Location = new System.Drawing.Point(5, 5);
                pnlHeading.Text = "Select Gridview Colums to Export";
                pnlHeading.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                pnlHeading.ForeColor = Color.Black;
                frmPopup.Controls.Add(pnlHeading);

                exlv = Utilities.GridColumnSelectionView(grdPnlShowOB);
                exlv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 25), new Size(450, 250));
                frmPopup.Controls.Add(exlv);

                System.Windows.Forms.Button exlvOK = new System.Windows.Forms.Button();
                exlvOK.Text = "OK";
                exlvOK.BackColor = Color.Tan;
                exlvOK.Location = new System.Drawing.Point(40, 280);
                exlvOK.Click += new System.EventHandler(this.exlvOK_Click1);
                frmPopup.Controls.Add(exlvOK);

                System.Windows.Forms.Button exlvCancel = new System.Windows.Forms.Button();
                exlvCancel.Text = "Cancel";
                exlvCancel.BackColor = Color.Tan;
                exlvCancel.Location = new System.Drawing.Point(130, 280);
                exlvCancel.Click += new System.EventHandler(this.exlvCancel_Click2);
                frmPopup.Controls.Add(exlvCancel);

                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void exlvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        private void exlvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                // pnllv.Visible = false;
                string heading1 = "Leave OB List";
                string heading2 = "";
                Utilities.export2Excel(heading1, heading2, "", grdPnlShowOB, exlv);
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        private List<DataGridViewColumn> getListOFColumn()
        {
            List<DataGridViewColumn> dgvColList = new List<DataGridViewColumn>();
            try
            {
                string[] strColArr =
                   {
                "Employee ID",
                "Employee Name",
                "Office"
                };
                DataGridViewTextBoxColumn col = new DataGridViewTextBoxColumn();
                col.Width = 100;
                foreach (string str in strColArr)
                {
                    col = new DataGridViewTextBoxColumn();
                    col.Name = str.Replace(" ", "");
                    col.HeaderText = str;
                    dgvColList.Add(col);
                }
                foreach (var item in LeaveDict)
                {
                    col = new DataGridViewTextBoxColumn();
                    col.Name = item.Key.Replace(" ", "");
                    col.HeaderText = item.Value;
                    dgvColList.Add(col);
                }
            }
            catch (Exception ex)
            {
            }
            return dgvColList;
        }

        private DataGridView getDynamicGridRowsnw(DataGridView grdlistShowDemo)
        {
            try
            {
                List<employeeposting> EMPList = LeaveOBDB.getEmployeePostingListForLeaveOB();
                foreach (employeeposting empPost in EMPList)
                {
                    List<string> strList = new List<string>();
                    strList.Add(empPost.empID.ToString());
                    strList.Add(empPost.empName);
                    strList.Add(empPost.officeName);
                    LeaveOBDB lobDB = new LeaveOBDB();
                    List<leaveob> lobList = lobDB.getLeaveOBDetails(empPost.empID.ToString(), Convert.ToInt32(cmbListYear.SelectedItem));
                    grdlistShowDemo.Rows.Add();
                    grdlistShowDemo.Rows[grdlistShowDemo.RowCount - 1].Cells["EmployeeID"].Value = empPost.empID.ToString();
                    grdlistShowDemo.Rows[grdlistShowDemo.RowCount - 1].Cells["EmployeeName"].Value = empPost.empName;
                    grdlistShowDemo.Rows[grdlistShowDemo.RowCount - 1].Cells["Office"].Value = empPost.officeName;
                    DataGridViewColumnCollection collection = grdlistShowDemo.Columns;
                    foreach (DataGridViewColumn col in collection)
                    {
                        string leaveColID = (LeaveDict.FirstOrDefault(pair => col.Name == pair.Key.Replace(" ", ""))).Key;
                        if (leaveColID != null)
                        {
                            leaveob lob = lobList.FirstOrDefault(listItem => listItem.LeaveID == leaveColID);
                            if (lob != null)
                            {
                                grdlistShowDemo.Rows[grdlistShowDemo.RowCount - 1].Cells[col.Name].Value = lob.LeaveCount;
                            }
                            else
                            {
                                grdlistShowDemo.Rows[grdlistShowDemo.RowCount - 1].Cells[col.Name].Value = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return grdlistShowDemo;
        }



        private DataGridView getDynamicGridRows(DataGridView grdlistShowDemo)
        {
            try
            {
                List<leaveob> lvaob = new List<leaveob>();
                leaveob lva = new leaveob();
                cmbListYear.SelectedItem = cmbLeaveYear.SelectedItem;
                List<employeeposting> EMPList = LeaveOBDB.getEmployeePostingListForLeaveOB();
                grdlistShowDemo.Rows.Clear();
                foreach (employeeposting empPost in EMPList)
                {
                    if (empPost.empID==2)
                    {
                        int a = 0;
                    }
                    int bal = 0;
                    int leavetaken = 0;
                    int total = 0;
                    int val = 0;
                    List<string> strList = new List<string>();
                    strList.Add(empPost.empID.ToString());
                    strList.Add(empPost.empName);
                    strList.Add(empPost.officeName);
                    LeaveApproveDB lvAppDB = new LeaveApproveDB();
                    LeaveOBDB lobDB = new LeaveOBDB();
                    List<leaveob> lobList = lobDB.getLeaveOBDetails(empPost.empID.ToString(), Convert.ToInt32(cmbListYear.SelectedItem));
                    grdlistShowDemo.Rows.Add();
                    grdlistShowDemo.Rows[grdlistShowDemo.RowCount - 1].Cells["EmployeeID"].Value = empPost.empID.ToString();
                    grdlistShowDemo.Rows[grdlistShowDemo.RowCount - 1].Cells["EmployeeName"].Value = empPost.empName;
                    grdlistShowDemo.Rows[grdlistShowDemo.RowCount - 1].Cells["Office"].Value = empPost.officeName;
                    DataGridViewColumnCollection collection = grdlistShowDemo.Columns;
                    List<leaveob> officemax = lobDB.getOfficeMax(empPost.officeID);
                    int prevYear = Convert.ToInt32(cmbListYear.SelectedItem);
                    List<leaveob> lobListPrev = lobDB.getLeaveOBDetailsnew(empPost.empID.ToString(), prevYear);

                    //Gives Carry Forward LEave Types
                    LeaveSettingsdb lvDB = new LeaveSettingsdb();
                    List<Leave> carryAvail = lvDB.getLeaveTypeList().Where(w => w.CarryForward == 1).ToList();

                    foreach (DataGridViewColumn col in collection)
                    {
                        leavetaken = 0;
                        string leaveColID = (LeaveDict.FirstOrDefault(pair => col.Name == pair.Key.Replace(" ", ""))).Key;
                        if (leaveColID != null)
                        {
                            leaveob lob = lobList.FirstOrDefault(listItem => listItem.LeaveID == leaveColID);
                            List<leaveapprove> lvremain = lobDB.getLeaveRemainYearWise(empPost.empID.ToString(), leaveColID, prevYear);
                            foreach (leaveapprove lv in lvremain)
                            {
                                leavetaken += lv.leavepending;
                                leavetaken += 1;
                            }
                            int prevLVAvail = 0;
                            leaveob lobPrev = lobListPrev.FirstOrDefault(leave => leave.LeaveID == leaveColID);
                            if (lobPrev != null)
                            {
                                prevLVAvail = lobPrev.LeaveCount;
                            }
                            bal = prevLVAvail - leavetaken;

                            leaveob offmax = officemax.FirstOrDefault(leave => leave.LeaveID == leaveColID);
                            if (offmax != null)
                            {
                                total = bal + offmax.maxdays;
                            }
                            else
                            {
                                total = bal;
                            }
                            if (lob != null)
                            {
                                Leave lvCarry = carryAvail.FirstOrDefault(lv => lv.leaveID == leaveColID);

                                if (lvCarry != null) // carry forward 1
                                {
                                    //int finaltotal = total + lob.LeaveCount;
                                    if (lvCarry.MaxAccrual > total)
                                    {
                                        grdlistShowDemo.Rows[grdlistShowDemo.RowCount - 1].Cells[col.Name].Value = total;
                                        val = total;
                                    }
                                    else 
                                    {
                                        grdlistShowDemo.Rows[grdlistShowDemo.RowCount - 1].Cells[col.Name].Value = lvCarry.MaxAccrual;
                                        val = lvCarry.MaxAccrual;
                                    }
                                }
                                else
                                {
                                    if (offmax != null)
                                    {
                                        grdlistShowDemo.Rows[grdlistShowDemo.RowCount - 1].Cells[col.Name].Value = offmax.maxdays;
                                        val =  offmax.maxdays;
                                    }
                                    else
                                    {
                                        grdlistShowDemo.Rows[grdlistShowDemo.RowCount - 1].Cells[col.Name].Value = lob.LeaveCount;
                                        val = lob.LeaveCount;
                                    }
                                        
                                }

                            }
                            else
                            {
                                Leave lvCarry = carryAvail.FirstOrDefault(lv => lv.leaveID == leaveColID);

                                if (lvCarry != null) // carry forward 1
                                {
                                    if (lvCarry.MaxAccrual > total)
                                    {
                                        grdlistShowDemo.Rows[grdlistShowDemo.RowCount - 1].Cells[col.Name].Value = total;
                                        val = total;
                                    }
                                    else
                                    {
                                        grdlistShowDemo.Rows[grdlistShowDemo.RowCount - 1].Cells[col.Name].Value = lvCarry.MaxAccrual;
                                        val = lvCarry.MaxAccrual;
                                    }
                                }
                                else
                                {
                                    if(offmax != null)
                                    {
                                        grdlistShowDemo.Rows[grdlistShowDemo.RowCount - 1].Cells[col.Name].Value = offmax.maxdays;
                                        val = offmax.maxdays;
                                    }
                                    else
                                    {
                                        grdlistShowDemo.Rows[grdlistShowDemo.RowCount - 1].Cells[col.Name].Value = 0;
                                        val = 0;
                                    }
                                  
                                }
                            }
                            lva = new leaveob();
                            lva.EmployeeID = empPost.empID.ToString();
                            lva.LeaveID = leaveColID;
                            lva.LeaveCount = val;
                            lvaob.Add(lva);
                        }  
                       
                    }
                    
                }
                insert(lvaob);
            }
            catch (Exception ex)
            {
            }
            return grdlistShowDemo;
        }
        void insert(List<leaveob> lva)
        {
            try
            {
                int year = Convert.ToInt32(cmbLeaveYear.SelectedItem);
                LeaveOBDB LODB = new LeaveOBDB();
                if (LODB.insertLeaveyearOBDetail(lva, year))
                {
                    MessageBox.Show("Leave Year OB Detail Updated Sucessfully");
                    cmbLeaveYear.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("Fail to update Leave OB Detail");
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pnlShowLeaveOB.Visible = true;
            btnCancelLeaveOB.Visible = true;

        }

        private void LeaveYearOB_Enter(object sender, EventArgs e)
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




        //public void check(string emplid, string leaveid)
        //{
        //    try
        //    {
        //        double bal = 0;
        //        double daysPending = 0;
        //        double final = 0;
        //        Dictionary<string, double> pair = new Dictionary<string, double>();
        //        LeaveOBDB lvOBDB = new LeaveOBDB();
        //        LeaveApproveDB ladb = new LeaveApproveDB();
        //        LeaveSettingsdb lvDB = new LeaveSettingsdb();

        //        List<leaveapprove> lvapp = ladb.getLeaveLimit(emplid);

        //        double totaldays = 0;
        //        foreach (leaveapprove lv in lvapp)
        //        {
        //            totaldays += lv.maxdays;
        //            daysPending = 0;
        //            List<leaveapprove> lvremain = ladb.getLeaveRemain(emplid, lv.leaveid);
        //            foreach (leaveapprove lv1 in lvremain)
        //            {
        //                daysPending += lv1.leavepending;
        //                daysPending += 1;
        //            }
        //            bal = totaldays - daysPending;
        //            pair.Add(lv.leaveid, bal);
        //        }
        //        List<Leave> carryAvail = lvDB.getLeaveTypeList().Where(w => w.CarryForward == 1).ToList(); // Gives 

        //        List<leaveob> lvList = lvOBDB.getLeaveOBDetails(emplid, Convert.ToInt32(cmbLeaveYear.SelectedItem));

        //        foreach (Leave i in carryAvail)
        //        {
        //            foreach (leaveob l in lvList)
        //            {
        //                if (i.leaveID == l.LeaveID)
        //                {
        //                    foreach (var p in pair)
        //                    {
        //                        if (p.Key == l.LeaveID)
        //                        {
        //                            final = bal + l.maxdays;
        //                        }
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
    }
}

