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
    public partial class ReportAttendanceList : System.Windows.Forms.Form
    {
        Form frmPopup = new Form();
        ListView exlv = new ListView();// list view for choice / selection list
        string officeid = "";
        Timer filterTimer = new Timer();
        string colName = "";
        public ReportAttendanceList()
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
            ListFilteredAttendanceList(dtMonth.Value, officeid);
            if (grdList.RowCount >= 1)
            {
                lblSearch.Visible = true;
                txtSearch.Visible = true;
                btnExportToExcel.Visible = true;
            }
            applyPrivilege();
        }

        private void ListFilteredAttendanceList(DateTime month, string officeID)
        {
            try
            {
                grdList.Rows.Clear();
                grdList.Columns.Clear();
                AttendanceDB Adb = new AttendanceDB();               
                List<DateTime> dates = GetDates(dtMonth.Value.Month, dtMonth.Value.Year); // get days for selected month and year
                List<attendance> AttStat = Adb.getEmployeeAttendanceList(month, officeID);
                if (AttStat.Count <= 0)
                {
                    MessageBox.Show("No data to show");
                    return;
                }
                grdList.Columns.Add("Slno", "Slno");
                grdList.Columns.Add("EmployeeID", "ID");
                grdList.Columns.Add("EmployeeName", "Employee Name");
                grdList.Columns.Add("Office", "Office");
                grdList.Columns["Slno"].Frozen = true;
                grdList.Columns["EmployeeID"].Frozen = true;
                grdList.Columns["EmployeeName"].Frozen = true;
                grdList.Columns["Office"].Frozen = true;
                grdList.Columns["Slno"].Width = 30;
                grdList.Columns["EmployeeID"].Width = 50;
                foreach (DateTime dt in dates)
                {
                    //if (dt.Date < DateTime.Now)
                    //{
                        grdList.Columns.Add(dt.Date.ToString("yyyy-MM-dd"), dt.Date.ToString("dd"));
                        grdList.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        grdList.Columns[dt.Date.ToString("yyyy-MM-dd")].Width = 25;
                    //}
                }
                var emplst = AttStat.GroupBy(G => G.EmployeeID).Select(A => new
                {
                    EmployeeID = A.Key,
                    EmployeeName = A.First().EmployeeName,
                    officeName = A.First().officeName
                });
                foreach (var att in emplst)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["Slno"].Value = grdList.RowCount;
                    grdList.Rows[grdList.RowCount - 1].Cells["EmployeeID"].Value =Convert.ToInt32( att.EmployeeID);
                    grdList.Rows[grdList.RowCount - 1].Cells["EmployeeName"].Value = att.EmployeeName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Office"].Value = att.officeName;                 

                    foreach (DataGridViewColumn dgvc in grdList.Columns)
                    {
                        List<attendance> lst = AttStat.Where(x => x.EmployeeID == att.EmployeeID).ToList();
                        foreach (attendance lt in lst)
                        {
                            if (dgvc.Name == lt.StatusDate.ToString("yyyy-MM-dd"))
                            {
                                grdList.Rows[grdList.RowCount - 1].Cells[dgvc.Name].Value = getAtstatus(lt.AttendenceStatus);
                            }
                        }
                    }
                }

                    if (grdList.Rows.Count != 0 && Main.itemPriv[1] == true && (Main.itemPriv[2] == true || Main.itemPriv[3] == true))
                    btnExportToExcel.Visible = true;
                else
                    btnExportToExcel.Visible = false;
                grdList.Visible = true;
                pnlgrdList.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error 3 : ListFilteredAttendanceListLeave() - " + ex.ToString());
            }
        }

        private void initVariables()
        {
            closeAllPanels();
            dtMonth.Value.ToString("MM-yyyy");
            cmbfilterOffice.Visible = true;
            lblOffice.Visible = true;
            btnView.Visible = true;
            OfficeDB.fillOfficeComboNew(cmbfilterOffice);
            cmbfilterOffice.Items.Add("All");
            cmbfilterOffice.SelectedItem = "All";
            if (cmbfilterOffice.SelectedItem.ToString() == "All")
            {
                officeid = "All";
            }
            else
            {
                officeid = ((Structures.ComboBoxItem)cmbfilterOffice.SelectedItem).HiddenValue;
            }
        }

        public List<DateTime> GetDates(int month, int year)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month)).Select(day => new DateTime(year, month, day)).ToList();
        }

        public string getAtstatus(string atstat)
        {
            string ret = "";
            if (atstat == "Present")
            {
                ret = "P";
            }
            else if (atstat == "Absent")
            {
                ret = "A";
            }
            else if (atstat == "Leave")
            {
                ret = "L";
            }
            else if (atstat == "LocalTravel")
            {
                ret = "P";
            }
            else if (atstat == "Tour" || atstat == "DomesticTravel" || atstat == "IntlTravel")
            {
                ret = "T";
            }
            else if(atstat=="WeeklyOff")
            {
                ret = "W";
            }
            else if(atstat=="CompOff")
            {
                ret = "C";
            }
            else
            {
                ret = "";
            }
            return ret;
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
                lblOffice.Visible = false;
                btnView.Visible = false;
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
        }
       
        private void cmbcmpnysrch_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
            grdList.Rows.Clear();
            pnlgrdList.Visible = false;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
            OfficeDB.fillOfficeComboNew(cmbfilterOffice);
            cmbfilterOffice.Items.Add("All");
            cmbfilterOffice.SelectedItem = "All";
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
            closeAllPanels();
            cmbfilterOffice.Visible = true;
            lblOffice.Visible = true;
            btnView.Visible = true;
            txtSearch.Text = "";
            pnlgrdList.Visible = true;
            grdList.Visible = true;

            if (cmbfilterOffice.SelectedItem.ToString() == "All")
            {
                officeid = "All";
            }
            else
            {
                officeid = ((Structures.ComboBoxItem)cmbfilterOffice.SelectedItem).HiddenValue;
            }
            ListFilteredAttendanceList(dtMonth.Value, officeid);
            if (grdList.RowCount >= 1)
            {
                lblSearch.Visible = true;
                txtSearch.Visible = true;
                btnExportToExcel.Visible = true;
            }
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

                string heading1 = "Attendance List";
                string heading2 = dtMonth.Value.ToString("MMM-yyyy");
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
            lblOffice.Visible = true;
            btnView.Visible = true;
            if (grdList.RowCount >= 1)
            {
                lblSearch.Visible = true;
                txtSearch.Visible = true;
                btnExportToExcel.Visible = true;
            }
        }

        private void dtMonth_ValueChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
            grdList.Rows.Clear();
            pnlgrdList.Visible = false;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
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

        private void ReportAttendanceList_Enter(object sender, EventArgs e)
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

