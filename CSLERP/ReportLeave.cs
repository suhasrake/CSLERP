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
    public partial class ReportLeave : System.Windows.Forms.Form
    {
        Form frmPopup = new Form();
        ListView exlv = new ListView();// list view for choice / selection list
        int val = 0;
        string regional = "";
        string officeid = "";
        public ReportLeave()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void CompanyData_Load(object sender, EventArgs e)
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
            ListFilteredLeave(getFilterNo(val), officeid);
            if (grdList.RowCount >= 1)
            {
                lblSearch.Visible = true;
                txtSearch.Visible = true;
                btnExportToExcel.Visible = true;
            }
            applyPrivilege();
        }
        private void ListFilteredLeave(int opt, string officeID)
        {
            try
            {
                DateTime dttemp = DateTime.Now;
                grdList.Rows.Clear();
                grdList.Columns.Clear();
                LeaveReportDB sdb = new LeaveReportDB();
                List<leaveapprove> leavetype = new List<leaveapprove>();
                leavetype = sdb.getLeaveLimit();    
                grdList.Columns.Add("EmployeeID", "EmployeeID");
                grdList.Columns.Add("EmployeeName", "Employee Name");
                grdList.Columns.Add("Office", "Office");
                grdList.Columns["EmployeeID"].Frozen = true;
                grdList.Columns["EmployeeName"].Frozen = true;
                grdList.Columns["Office"].Frozen = true;                
                foreach (leaveapprove leave in leavetype)
                {
                    grdList.Columns.Add(leave.leaveid, leave.leaveid);
                    grdList.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;                   
                }
                grdList.Columns.Add("CO", "CO");
                List<leavereport> lvremain2 = sdb.getLeaveRemain2( opt, regional, officeID);
                var lvrmn = lvremain2.Where(W => W.status != 98 && W.status != 2).GroupBy(x => new { x.EmployeeID, x.leaveid })
                    .Select(    g => new
                    {
                      GroupName = g.Key,
                      leavepending = g.Where(a => a.documentStatus == 99 && a.sanctionedTo.Year== dttemp.Year).Sum(s => s.leavepending + 1) ,
                      EmployeeName = g.First().EmployeeName,
                      officename = g.First().officename,
                      EmployeeID = g.First().EmployeeID,
                      leaveid = g.First().leaveid
                     });
                var lvrmn2 = lvrmn.GroupBy(G => G.EmployeeID).ToList();
                    int i=0, j = 0;
                try
                {
                    foreach (var GroupName in lvrmn2)
                    {
                        i = 1;
                        j = 1;
                        string tstr = "";
                        try
                        {
                            foreach (var itm in GroupName)
                            {
                                if (i == j)
                                {
                                    tstr = itm.ToString();
                                    j++;
                                    grdList.Rows.Add();
                                    grdList.Rows[grdList.Rows.Count - 1].Cells["EmployeeID"].Value = itm.EmployeeID;
                                    grdList.Rows[grdList.Rows.Count - 1].Cells["EmployeeName"].Value = itm.EmployeeName;
                                    grdList.Rows[grdList.Rows.Count - 1].Cells["Office"].Value = itm.officename;
                                    grdList.Rows[grdList.Rows.Count - 1].Cells[itm.leaveid.Replace(" ", "CL")].Value = Convert.ToInt32(itm.leavepending);
                                }
                                else
                                {
                                    grdList.Rows[grdList.Rows.Count - 1].Cells[itm.leaveid].Value = itm.leavepending;
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show("Error 1 : ListFilteredLeave() - " + ex.ToString()+"\n"+tstr);
                        }
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error 2 : ListFilteredLeave() - " + ex.ToString());
                }

                DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                grdList.Columns.Add(btn);
                btn.HeaderText = "Details";
                btn.Text = "Details";
                btn.Name = "Details";
                btn.UseColumnTextForButtonValue = true;
                if (grdList.Rows.Count != 0 && Main.itemPriv[1] == true && (Main.itemPriv[2] == true || Main.itemPriv[3] == true))
                    btnExportToExcel.Visible = true;
                else
                    btnExportToExcel.Visible = false;
                grdList.Visible = true;
                pnlgrdList.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error 3 : ListFilteredLeave() - "+ex.ToString());
            }
        }

        private void initVariables()
        {
            closeAllPanels();
            RegionDB.fillRegionComboNew(cmbFilterRegion);
            cmbFilterRegion.Items.Add("All");
            cmbFilterRegion.SelectedItem = "All";
            cmbfilterOffice.Visible = true;
            cmbFilterRegion.Visible = true;
            lblOffice.Visible = true;
            lblRegion.Visible = true;
            btnView.Visible = true;
            if (cmbfilterOffice.SelectedItem.ToString() == "All")
            {
                officeid = "All";
            }
            else
            {
                officeid = ((Structures.ComboBoxItem)cmbfilterOffice.SelectedItem).HiddenValue;
            }


        }
        private void applyPrivilege()
        {
            try
            {
                if (Main.itemPriv[1])
                {
                    btnExportToExcel.Visible = true;
                }
                else
                {
                    btnExportToExcel.Visible = false;
                }
                if (Main.itemPriv[2])
                {
                    btnExportToExcel.Visible = true;
                }
                else
                {
                    btnExportToExcel.Visible = false;
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
                cmbfilterOffice.Visible = false;
                cmbFilterRegion.Visible = false;
                lblOffice.Visible = false;
                lblRegion.Visible = false;
                btnView.Visible = false;
                pnlLeaveDetailOuter.Visible = false;
                pnlLeaveDetailInner.Visible = false;
                lblSearch.Visible = false;
                txtSearch.Visible = false;
                btnExportToExcel.Visible = false;
                pnlgrdList.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        public void clearUserData()
        {
            try
            {
                cmbFilterRegion.SelectedIndex = 1;
                cmbfilterOffice.SelectedIndex = cmbfilterOffice.Items.Count - 1;
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
            if (e.RowIndex < 0)
                return;
            string columnName = grdList.Columns[e.ColumnIndex].Name;
            if (columnName.Equals("Details"))
            {
                closeAllPanels();
                txtEmpid.Text = grdList.Rows[e.RowIndex].Cells["EmployeeID"].Value.ToString();
                txtEmpName.Text = grdList.Rows[e.RowIndex].Cells["EmployeeName"].Value.ToString();
                setleavedetailgrid(txtEmpid.Text);
                pnlLeaveDetailOuter.Visible = true;
                pnlLeaveDetailInner.Visible = true;
            }
        }

        public void setleavedetailgrid(string empid)
        {
            try
            {
                double daysPending = 0;
                dgvLeaveDetails.Columns.Clear();
                LeaveApproveDB ladb = new LeaveApproveDB();
                LeaveReportDB lvdb = new LeaveReportDB();
                List<leaveapprove> lvob = lvdb.getLeaveLimit();
                List<leaveapprove> lvapp = ladb.getLeaveLimit(empid);
                dgvLeaveDetails.Columns.Add("LeaveType", "LeaveType");
                dgvLeaveDetails.Columns["LeaveType"].Frozen = true;
                foreach (leaveapprove lv in lvapp)
                {
                    int row = dgvLeaveDetails.Rows.Count - 1;
                    dgvLeaveDetails.Columns.Add(lv.leaveid, lv.leaveid);
                    dgvLeaveDetails.Columns[lv.leaveid].SortMode = DataGridViewColumnSortMode.NotSortable;
                    dgvLeaveDetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                }
                dgvLeaveDetails.Rows.Add();
                dgvLeaveDetails.Rows[dgvLeaveDetails.Rows.Count - 1].Cells["LeaveType"].Value = "Leave OB";
                    for (int j = 1; j <= dgvLeaveDetails.ColumnCount - 1; j++)
                    {
                        dgvLeaveDetails.Rows[dgvLeaveDetails.Rows.Count - 1].Cells[j].Value = 0;
                    }
                    foreach (leaveapprove lv in lvapp)
                    {                        
                            dgvLeaveDetails.Rows[dgvLeaveDetails.Rows.Count - 1].Cells[lv.leaveid].Value = lv.maxdays;
                    }
                
                dgvLeaveDetails.Rows.Add();
                dgvLeaveDetails.Rows[dgvLeaveDetails.Rows.Count - 1].Cells["LeaveType"].Value = "Leave Taken";
                    for (int j = 1; j <= dgvLeaveDetails.ColumnCount - 1; j++)
                    {
                        dgvLeaveDetails.Rows[dgvLeaveDetails.Rows.Count - 1].Cells[j].Value = 0;
                    }
                    foreach (leaveapprove lv1 in lvapp)
                    {
                        daysPending = 0;

                        List<leaveapprove> lvremain = ladb.getLeaveRemain(empid, lv1.leaveid);
                        foreach (leaveapprove lv in lvremain)
                        {
                            daysPending += lv.leavepending;
                            daysPending += 1;
                        }
                        dgvLeaveDetails.Rows[dgvLeaveDetails.Rows.Count - 1].Cells[lv1.leaveid].Value = daysPending;
                    }
                dgvLeaveDetails.Rows.Add();
                dgvLeaveDetails.Rows[dgvLeaveDetails.Rows.Count - 1].Cells["LeaveType"].Value = "Current Balance";
                for (int j = 1; j <= dgvLeaveDetails.ColumnCount - 1; j++)
                {
                    for (int i = 0; i <= dgvLeaveDetails.RowCount - 1; i++)
                    {
                        if (i == 2)
                        {
                            if (dgvLeaveDetails.Columns[j].HeaderText != "CO")
                            {
                                dgvLeaveDetails.Rows[i].Cells[j].Value = Convert.ToDouble(dgvLeaveDetails.Rows[i - 2].Cells[j].Value) - Convert.ToDouble(dgvLeaveDetails.Rows[i - 1].Cells[j].Value);
                            }
                            else
                            {
                                dgvLeaveDetails.Rows[i].Cells[j].Value = Convert.ToDouble(dgvLeaveDetails.Rows[i - 2].Cells[j].Value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }


        private void cmbcmpnysrch_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
            grdList.Rows.Clear();
            pnlgrdList.Visible = false;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
            if (cmbFilterRegion.SelectedItem.ToString() == "All")
            {
                OfficeDB.fillOfficeComboNew(cmbfilterOffice);
                val = 1;
            }
            else
            {
                LeaveReportDB.fillRegionWiseOfficeCombo(cmbfilterOffice, ((Structures.ComboBoxItem)cmbFilterRegion.SelectedItem).HiddenValue);
                regional = ((Structures.ComboBoxItem)cmbFilterRegion.SelectedItem).HiddenValue;
                 val = 2;
            }
            cmbfilterOffice.Items.Add("All");
            cmbfilterOffice.SelectedItem = "All";
        }

        private void filterGridData()
        {
            try
            {
                foreach (DataGridViewRow row in grdList.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in grdList.Rows)
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

            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            closeAllPanels();
            cmbfilterOffice.Visible = true;
            cmbFilterRegion.Visible = true;
            lblOffice.Visible = true;
            lblRegion.Visible = true;
            btnView.Visible = true;
            txtSearch.Text = "";
            pnlgrdList.Visible = true;
            grdList.Visible = true;
            if(cmbfilterOffice.SelectedItem.ToString()=="All")
            {
                officeid = "All";
            }
            else
            {
                officeid = ((Structures.ComboBoxItem)cmbfilterOffice.SelectedItem).HiddenValue;
            }
            ListFilteredLeave(getFilterNo(val), officeid);
            if (grdList.RowCount >= 1)
            {
                lblSearch.Visible = true;
                txtSearch.Visible = true;
                btnExportToExcel.Visible = true;
            }
        }

        private int getFilterNo(int val)
        {
            int no = 0;
            if (val==1 && cmbfilterOffice.SelectedItem.ToString() == "All" )
            {
                no = 1;
            }
            else if(val==1 && cmbfilterOffice.SelectedItem.ToString() != "All" ||
                    val == 2 && cmbfilterOffice.SelectedItem.ToString() != "All")
            {
                no = 2;
            }
            else if(val == 2 && cmbfilterOffice.SelectedItem.ToString() == "All")
            {
                no = 3;
            }


                return no;
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
                exlv = Utilities.GridColumnSelectionView(grdList);

                exlv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 25), new System.Drawing.Size(450, 250));
                frmPopup.Controls.Add(exlv);

                System.Windows.Forms.Label pnlHeading = new System.Windows.Forms.Label();
                pnlHeading.Size = new Size(300, 20);
                pnlHeading.Location = new System.Drawing.Point(5, 5);
                pnlHeading.Text = "Select Gridview Colums to Export";
                pnlHeading.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                pnlHeading.ForeColor = Color.Black;
                frmPopup.Controls.Add(pnlHeading);

                System.Windows.Forms.Button exlvOK = new System.Windows.Forms.Button();
                exlvOK.Text = "OK";
                exlvOK.BackColor = Color.Tan;
                exlvOK.Location = new System.Drawing.Point(40, 280);
                exlvOK.Click += new System.EventHandler(this.exlvOK_Click1);
                frmPopup.Controls.Add(exlvOK);

                System.Windows.Forms.Button exlvCancel = new System.Windows.Forms.Button();
                exlvCancel.Text = "CANCEL";
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

                string heading1 = "Leave Report";
                string heading2 = "";
                Utilities.export2Excel(heading1, heading2, "", grdList, exlv);
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export to Excell error");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            filterGridData();
        }

        private void cmbStoreLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
            grdList.Rows.Clear();
            pnlgrdList.Visible = false;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            closeAllPanels();
            pnlgrdList.Visible = true;
            grdList.Visible = true;
            cmbfilterOffice.Visible = true;
            cmbFilterRegion.Visible = true;
            lblOffice.Visible = true;
            lblRegion.Visible = true;
            btnView.Visible = true;
            if (grdList.RowCount >= 1)
            {
                lblSearch.Visible = true;
                txtSearch.Visible = true;
                btnExportToExcel.Visible = true;
            }
        }

        private void ReportLeave_Enter(object sender, EventArgs e)
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

