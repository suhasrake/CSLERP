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
    public partial class ReportLeaveDetail : System.Windows.Forms.Form
    {
        Form frmPopup = new Form();
        ListView exlv = new ListView();// list view for choice / selection list
        Timer filterTimer = new Timer();
        string colName = "";
        public ReportLeaveDetail()
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
           
            if (grdList.RowCount >= 1)
            {
                lblSearch.Visible = true;
                txtSearch.Visible = true;
                btnExportToExcel.Visible = true;
            }
            applyPrivilege();
        }
        private void ListFilteredLeave(int opt)
        {
            try
            {

                DateTime dtfrom = dtFrom.Value;
                DateTime dtto = dtTo.Value;
                grdList.Rows.Clear();
                grdList.Columns.Clear();
                grdList.Columns.Add("EmployeeID", "EmployeeID");
                grdList.Columns.Add("EmployeeName", "Employee Name");
                grdList.Columns.Add("ApplicationDate", "Application Date");
                grdList.Columns.Add("LeaveType", "Leave Type");               
                grdList.Columns.Add("NoofDays", "No Of Days");
                grdList.Columns["ApplicationDate"].Visible = false;
                grdList.Columns["ApplicationDate"].DefaultCellStyle.Format = "dd-MM-yyyy";
                grdList.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                LeaveApproveDB lvdb = new LeaveApproveDB();
                if (opt==1 || opt==3 || opt==4)
                {
                    grdList.Columns.Add("FromDate", "From");
                    grdList.Columns.Add("ToDate", "To");
                    grdList.Columns.Add("PresentStatus", "Present Status");
                    if(opt==1)
                    {
                        grdList.Columns.Add("ForwardedTo", "Forwarded To");
                    }
                    else if(opt==3)
                    {
                        grdList.Columns.Add("ForwardedTo", "Cancelled By");
                    }
                    else if (opt == 4)
                    {
                        grdList.Columns.Add("ForwardedTo", "Rejected By");
                    }
                    grdList.Columns["FromDate"].DefaultCellStyle.Format = "dd-MM-yyyy";
                    grdList.Columns["ToDate"].DefaultCellStyle.Format = "dd-MM-yyyy";
                    grdList.Columns["PresentStatus"].Width = 200;
                    grdList.Columns["ForwardedTo"].Width = 120;
                    grdList.Columns["EmployeeName"].Width = 160;
                    List<leaveapprove> leaveactnpending = lvdb.getActionPending(dtfrom.Date,dtto.Date,opt).OrderBy(x=> Convert.ToInt32(x.EmployeeID)).ToList();
                    foreach(leaveapprove ap in leaveactnpending )
                    {
                        grdList.Rows.Add();
                        grdList.Rows[grdList.Rows.Count - 1].Cells["EmployeeID"].Value = Convert.ToInt32(ap.EmployeeID);
                        grdList.Rows[grdList.Rows.Count - 1].Cells["EmployeeName"].Value = ap.EmployeeName;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["ApplicationDate"].Value = ap.CreateTime;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["LeaveType"].Value = ap.leaveid;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["NoofDays"].Value = ap.noofdays;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["FromDate"].Value = ap.fromdate;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["ToDate"].Value = ap.todate;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["PresentStatus"].Value = presentstatus(ap.status,ap.documentStatus,ap.leavestatus);
                        grdList.Rows[grdList.Rows.Count - 1].Cells["ForwardedTo"].Value = ap.ForwardUser;
                    }
                }
                else if(opt==2)
                {
                    grdList.Columns.Add("SanctionedFromDate", "Sanctioned From");
                    grdList.Columns.Add("SanctionedToDate", "Sanctioned To");
                    grdList.Columns.Add("Approver", "Approver");
                    grdList.Columns["SanctionedFromDate"].DefaultCellStyle.Format = "dd-MM-yyyy";
                    grdList.Columns["SanctionedToDate"].DefaultCellStyle.Format = "dd-MM-yyyy";
                    grdList.Columns["Approver"].Width = 170;
                    grdList.Columns["EmployeeName"].Width = 200;
                    grdList.Columns["SanctionedFromDate"].Width = 120;
                    grdList.Columns["SanctionedToDate"].Width = 120;
                    List<leaveapprove> leaveApprve = lvdb.getApprovedLeave(dtfrom.Date, dtto.Date).OrderBy(x => Convert.ToInt32(x.EmployeeID)).ToList();
                    foreach (leaveapprove ap in leaveApprve)
                    {
                        grdList.Rows.Add();
                        grdList.Rows[grdList.Rows.Count - 1].Cells["EmployeeID"].Value = Convert.ToInt32(ap.EmployeeID);
                        grdList.Rows[grdList.Rows.Count - 1].Cells["EmployeeName"].Value = ap.EmployeeName;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["ApplicationDate"].Value = ap.CreateTime;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["LeaveType"].Value = ap.leaveid;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["NoofDays"].Value = ap.noofdays;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["SanctionedFromDate"].Value = ap.sanctionedFrom;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["SanctionedToDate"].Value = ap.sanctionedTo;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["Approver"].Value = ap.ApproveUser;
                        if(ap.sanctionedTo.Date>=DateTime.Now.Date && ap.sanctionedFrom.Date<=DateTime.Now)
                        {
                            grdList.Rows[grdList.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Yellow;
                        }
                    }
                }
                if(grdList.RowCount>=1)
                {
                    grdList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    grdList.CurrentCell.Selected = false;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Report Leave Detail listing");
            }
        }


        public string presentstatus(int status, int documentstatus, int leavestatus)
        {
            string stat = "";
            try
            {
                if (status == 1)
                {
                    if (documentstatus>=2 && documentstatus<=98)
                    {
                        if(leavestatus==1)
                        {
                            stat = "Leave Forwarded";
                        }
                        if (leavestatus == 99)
                        {
                            stat = "Leave Rejected";
                        }
                        if(leavestatus==2)
                        {                          
                            stat = "Reversal Request";
                        }                        
                    }
                    if(documentstatus==99)
                    {
                        if (leavestatus == 1)
                        {
                            stat = "Approved";
                        }
                        if (leavestatus == 5)
                        {
                            stat = "Cancel Request";
                        }
                        if (leavestatus == 4)
                        {
                            stat = "Modification Requested";
                        }
                        if (leavestatus == 6)
                        {
                            stat = "Modification Request Accepted";
                        }
                        if (leavestatus == 7)
                        {
                            stat = "Modification Request Cancelled";
                        }
                        if (leavestatus == 8)
                        {
                            stat = "Modified Request Forwarded ";
                        }
                        if (leavestatus == 9)
                        {
                            stat = "Approved Modified";
                        }
                        if (leavestatus == 10)
                        {
                            stat = "Modified Cancelled";
                        }
                        if (leavestatus == 11)
                        {
                            stat = "Cancel request Rejected";
                        }
                    }
                    if(documentstatus == 1)
                    {
                        if (leavestatus == 1)
                        {
                            stat = "Created";
                        }
                        if (leavestatus == 3)
                        {
                            stat = "Reversal Accepted";
                        }
                    }
                }
                else if (status == 98)
                {
                    stat = "Cancel Request Approved";
                }
                else if (status == 2)
                {
                    stat = "Leave Request Deleted";
                }
            }
            catch (Exception ex)
            {

            }
            return stat;
        }




        private void initVariables()
        {
            closeAllPanels();
            dtFrom.Format = DateTimePickerFormat.Custom;
            dtFrom.CustomFormat = "dd-MM-yyyy";
            dtFrom.Enabled = true;
            dtFrom.Value = DateTime.Now.AddMonths(-1);
            dtTo.Format = DateTimePickerFormat.Custom;
            dtTo.CustomFormat = "dd-MM-yyyy";
            dtTo.Enabled = true;
            btnView.Visible = true;
            
            cmbfilterStatus.SelectedIndex = -1;


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
                lblSearch.Visible = false;
                txtSearch.Visible = false;
                btnExportToExcel.Visible = false;
                pnlgrdList.Visible = false;
                btnView.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        public void clearUserData()
        {
            try
            {
                cmbfilterStatus.SelectedIndex = -1;                
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

            }
        }

        private void cmbcmpnysrch_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
            grdList.Rows.Clear();
            pnlgrdList.Visible = false;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
            cmbfilterStatus.Items.Add("All");
            cmbfilterStatus.SelectedItem = "All";
        }

        private void filterGridData(string clm)
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
                        if (clm == null || clm.Length == 0)
                        {
                            if (!row.Cells["EmployeeName"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
                            {
                                row.Visible = false;
                            }
                        }
                        else
                        {

                            if (!row.Cells[clm].FormattedValue.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
                            {
                                row.Visible = false;
                            }
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
            try
            {
                closeAllPanels();
                cmbfilterStatus.Visible = true;
                //cmbFilterRegion.Visible = true;
                lblOffice.Visible = true;
                //lblRegion.Visible = true;
                btnView.Visible = true;
                txtSearch.Text = "";
                colName = "";
                pnlgrdList.Visible = true;
                grdList.Visible = true;
                if(dtFrom.Value>dtTo.Value)
                {
                    MessageBox.Show("Please Check the Dates!!!");
                    return;
                }
                if(cmbfilterStatus.SelectedIndex !=-1)
                {
                    if (cmbfilterStatus.SelectedItem.ToString() == "Action pending")
                    {
                        ListFilteredLeave(1);
                    }
                    else if (cmbfilterStatus.SelectedItem.ToString() == "Approved")
                    {
                        ListFilteredLeave(2);
                    }
                    else if (cmbfilterStatus.SelectedItem.ToString() == "Cancelled")
                    {
                        ListFilteredLeave(3);
                    }
                    else if (cmbfilterStatus.SelectedItem.ToString() == "Rejected")
                    {
                        ListFilteredLeave(4);
                    }
                }
                 else
                {
                    MessageBox.Show("Please Select the Status!!!");
                    return;
                }

                if (grdList.RowCount >= 1)
                {
                    lblSearch.Visible = true;
                    txtSearch.Visible = true;
                    btnExportToExcel.Visible = true;
                }
            }
           catch(Exception ex)
            {

            }
        }

        private int getFilterNo(int val)
        {
            int no = 0;
            if (val==1 && cmbfilterStatus.SelectedItem.ToString() == "All" )
            {
                no = 1;
            }
            else if(val==1 && cmbfilterStatus.SelectedItem.ToString() != "All" ||
                    val == 2 && cmbfilterStatus.SelectedItem.ToString() != "All")
            {
                no = 2;
            }
            else if(val == 2 && cmbfilterStatus.SelectedItem.ToString() == "All")
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

                string heading1 = "Leave Report Detail";
                string heading2 = "From:"+dtFrom.Value.ToString("dd-MM-yyyy")+" To:"+ dtTo.Value.ToString("dd-MM-yyyy") + " Status:"+cmbfilterStatus.SelectedItem.ToString();
                //string heading3 = dtTo.Value.ToString("dd-MM-yyyy");
                Utilities.export2Excel(heading1, heading2,"", grdList, exlv);
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
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterTimer.Tick -= new System.EventHandler(this.handlefilterTimerTimeout);
            filterTimer.Tick += new System.EventHandler(this.handlefilterTimerTimeout);
            filterTimer.Interval = 500;
            filterTimer.Enabled = true;
            filterTimer.Start();
        }

        private void handlefilterTimerTimeout(object sender, EventArgs e)
        {
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterGridData(colName);
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
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
            cmbfilterStatus.Visible = true;
            //cmbFilterRegion.Visible = true;
            lblOffice.Visible = true;
            //lblRegion.Visible = true;
            btnView.Visible = true;
            if (grdList.RowCount >= 1)
            {
                lblSearch.Visible = true;
                txtSearch.Visible = true;
                btnExportToExcel.Visible = true;
            }
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            closeAllPanels();
            clearUserData();
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            closeAllPanels();
            clearUserData();
        }

        private void grdList_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                colName = grdList.Columns[e.ColumnIndex].Name;
                foreach (DataGridViewColumn col in grdList.Columns)
                {
                    col.HeaderCell.Style.BackColor = Color.LightBlue;
                }
                grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdList.Columns[e.ColumnIndex].HeaderCell.Style.BackColor = Color.Magenta;
            }
            catch (Exception ex)
            {
            }
        }

        private void grdList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
           
        }

        private void ReportLeaveDetail_Enter(object sender, EventArgs e)
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
    


