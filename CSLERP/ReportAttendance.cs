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
using System.Globalization;

namespace CSLERP
{
    public partial class ReportAttendance : System.Windows.Forms.Form
    {
        Form frmPopup = new Form();
        ListView exlv = new ListView();// list view for choice / selection list
        int val = 0;
        string regional = "";
        string officeid = "";
        string monthyr = "";
        public ReportAttendance()
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

            ListFilteredLeave(monthyr);
            if (grdList.RowCount >= 1)
            {
                lblSearch.Visible = true;
                txtSearch.Visible = true;
            }
            getuploaddate();
            applyPrivilege();
        }

        private void dtMonth_ValueChanged(object sender, EventArgs e)
        {
            if (dtMonth.Value.Date <= DateTime.Now.Date)
            {
                monthyr = dtMonth.Value.ToString("MM-yyyy");
                ListFilteredLeave(monthyr);
            }
            else
            {
                MessageBox.Show("Report not generated for this Date!!!");
                grdList.Visible = false;
            }
        }

        public List<DateTime> GetDates(int month, int year)
        {
            return Enumerable.Range(1, DateTime.DaysInMonth(year, month)).Select(day => new DateTime(year, month, day)).ToList();
        }

        public void getuploaddate()
        {
            DateTime updt = ReportAttendanceDB.getUpdatedateTime();
            lblUploadTime.Text = updt.ToString("dd-MM-yyyy  HH:mm:ss");
        }

        private void ListFilteredLeave(string mnthyr)
        {
            try
            {
                DateTime dttemp = DateTime.Now;
                grdList.Rows.Clear();
                grdList.Columns.Clear();
                int status = 0;
                ReportAttendanceDB sdb = new ReportAttendanceDB();
                grdList.Columns.Add("EmployeeID", "EmployeeID");
                grdList.Columns.Add("EmployeeName", "Employee Name");
                grdList.Columns["EmployeeID"].Frozen = true;
                grdList.Columns["EmployeeName"].Frozen = true;
                List<reportattendence> timings = sdb.getcompanydata();
                reportattendence st = timings.FirstOrDefault(W => W.DataID == "OfficeOpenTime");
                reportattendence et = timings.FirstOrDefault(W => W.DataID == "OfficeCloseTime");
                DateTime EntrydateTime = Convert.ToDateTime(st.DataValue);
                DateTime ExitdateTime = Convert.ToDateTime(et.DataValue);


                List<DateTime> dates = GetDates(dtMonth.Value.Month, dtMonth.Value.Year); // get days for selected month and year
                foreach (DateTime dt in dates)
                {
                    if (dt.Date < DateTime.Now)
                    {
                        grdList.Columns.Add(dt.Date.ToString("yyyy-MM-dd"), dt.Date.ToString("dd"));
                        grdList.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        grdList.Columns[dt.Date.ToString("yyyy-MM-dd")].Width = 25;
                    }
                }

                List<reportattendence> empllist = sdb.getEmployeeList(); // get list of employee in NVP
                foreach (reportattendence emp in empllist)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.Rows.Count - 1].Cells["EmployeeID"].Value =Convert.ToInt32( emp.EmployeeID);
                    grdList.Rows[grdList.Rows.Count - 1].Cells["EmployeeName"].Value = emp.EmployeeName;
                }

                int i = 0;
                string[] yrnmth = monthyr.Split('-');

                List<reportattendence> presentlist = sdb.getEmployeePresentListnew(yrnmth[0], yrnmth[1]); // gets list of employee in biometric

                List<reportattendence> leavelist = sdb.getEmployeeLeaveList(yrnmth[0], yrnmth[1]); //gets list of leave for that particular month and year

                List<reportattendence> MRlist = sdb.getMrList(yrnmth[0], yrnmth[1]);//gets list of employee movement register

                foreach (DataGridViewColumn dgvc in grdList.Columns)
                {
                    int j = 0;      //no of rows
                    if (i > 1)     //after 2 columns
                    {
                        while (j < grdList.RowCount)    //for employees
                        {
                            int stat = 0;
                            int Instat = 0;
                            DateTime date = Convert.ToDateTime(dgvc.Name);
                            string empid = grdList.Rows[j].Cells["EmployeeID"].Value.ToString();
                            reportattendence leavels = leavelist.Where(W => W.EmployeeID == empid).FirstOrDefault
                                (dt => date >= dt.fromdate && date <= dt.todate);                             ///check leave for employee for that particular date
                            reportattendence mr = MRlist.Where
                             (W => W.EmployeeID == empid && W.outime.ToString("yyyy-MM-dd") == dgvc.Name).FirstOrDefault(dt => dt.outime.ToString("yyyy-MM-dd") == dgvc.Name);

                            if (leavels != null)
                            {
                                stat = 1;
                                status = 1;
                            }

                            List<reportattendence> rpList = presentlist.Where
                                (w => w.EmployeeID == empid && w.Etime.Date.ToString("yyyy-MM-dd") == dgvc.Name).ToList();// check if employee is present
                            if (rpList.Count != 0)
                            {
                                status = 2;
                                if (stat == 1)
                                {
                                    status = 3;
                                }
                                reportattendence reprtlate = rpList.OrderBy(W => W.Etime.TimeOfDay).FirstOrDefault();  // check if employee entered Late
                                if (reprtlate.Etime.TimeOfDay > EntrydateTime.TimeOfDay)
                                {
                                    status = 4;
                                    Instat = 1;
                                    if (mr != null)
                                    {
                                        if (mr.PlannedIntime.TimeOfDay > EntrydateTime.TimeOfDay && mr.PlannedIntime.TimeOfDay < ExitdateTime.TimeOfDay)
                                        {
                                            if (mr.Intime != null && (mr.Intime.TimeOfDay < ExitdateTime.TimeOfDay && mr.Intime.TimeOfDay > EntrydateTime.TimeOfDay))
                                            {
                                                Instat = 0;
                                                status = 13;
                                            }
                                        }
                                    }
                                    if (stat == 1)
                                    {
                                        status = 5;
                                        if (mr != null)
                                        {
                                            if (mr.PlannedIntime.TimeOfDay > EntrydateTime.TimeOfDay && mr.PlannedIntime.TimeOfDay < ExitdateTime.TimeOfDay)
                                            {
                                                if (mr.Intime != null && (mr.Intime.TimeOfDay < ExitdateTime.TimeOfDay && mr.Intime.TimeOfDay > EntrydateTime.TimeOfDay))
                                                {
                                                    Instat = 0;
                                                    status = 14;
                                                }
                                            }
                                        }
                                    }
                                }
                                    reportattendence replastExit = rpList.OrderByDescending(W => W.Etime.TimeOfDay).FirstOrDefault(); // check if employee left early
                                        if (replastExit.Etime.TimeOfDay < ExitdateTime.TimeOfDay)
                                        {
                                            status = 6;
                                            if (mr != null)
                                            {
                                                if (mr.outime.TimeOfDay < ExitdateTime.TimeOfDay)
                                                {
                                                    if (mr.Intime == DateTime.MinValue || mr.Intime.TimeOfDay > ExitdateTime.TimeOfDay)
                                                    {
                                                        status = 15;
                                                    }
                                                }
                                            }
                                            if (stat == 1 && Instat == 1)
                                            {
                                                status = 8;
                                                if (mr != null)
                                                {
                                                    if (mr.outime.TimeOfDay < ExitdateTime.TimeOfDay)
                                                    {
                                                        if (mr.Intime == DateTime.MinValue || mr.Intime.TimeOfDay > ExitdateTime.TimeOfDay)
                                                        {
                                                            status = 17;// MR:if employee left early and in MR and was late to office
                                                        }
                                                    }
                                                }
                                            }
                                            else if (stat == 1)
                                            {
                                                status = 7;
                                                if (mr != null)
                                                {
                                                    if (mr.outime.TimeOfDay < ExitdateTime.TimeOfDay)
                                                    {
                                                        if (mr.Intime == DateTime.MinValue || mr.Intime.TimeOfDay > ExitdateTime.TimeOfDay)
                                                        {
                                                            status = 16;
                                                        }
                                                    }
                                                }
                                            }
                                            if (Instat == 1)
                                            {
                                                status = 9;
                                                if (mr != null)
                                                {
                                                    if (mr.outime.TimeOfDay < ExitdateTime.TimeOfDay)
                                                    {
                                                        if (mr.Intime == DateTime.MinValue || mr.Intime > ExitdateTime)
                                                        {
                                                            status = 18;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                if (replastExit.Etime.TimeOfDay == reprtlate.Etime.TimeOfDay) // check if employee has single punch
                                {
                                    status = 10;

                                    if (mr != null)
                                    {
                                        if ((mr.outime.TimeOfDay > EntrydateTime.TimeOfDay && mr.PlannedIntime.TimeOfDay > ExitdateTime.TimeOfDay) ||
                                             (mr.outime.TimeOfDay > EntrydateTime.TimeOfDay && mr.Intime.TimeOfDay > ExitdateTime.TimeOfDay))
                                        {
                                            status = 19;// for single punch on MR
                                        }
                                    }
                                    if (stat == 1)
                                    {
                                        status = 11;
                                        if (mr != null)
                                        {
                                            if ((mr.outime.TimeOfDay > EntrydateTime.TimeOfDay && mr.PlannedIntime.TimeOfDay > ExitdateTime.TimeOfDay) ||
                                                (mr.Intime.TimeOfDay > EntrydateTime.TimeOfDay && mr.Intime.TimeOfDay > ExitdateTime.TimeOfDay))
                                            {
                                                status = 20;
                                            }
                                        }
                                    }
                                }
                            }
                                else if (stat != 1)  // if employee absent
                                {
                                    status = 12;
                                    if (mr != null)
                                    {
                                        status = 21;// no punch but has MR
                                    }
                                }
                            setcolor(status, j, dgvc.Name);
                            j++;
                        }
                    }
                    i++;
                }
                grdList.DefaultCellStyle.NullValue = "";
                grdList.Visible = true;
                grdList.Visible = true;
                pnlgrdList.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Attendence listing");
            }
        }

        void setcolor(int status, int row, string col)
        {
            switch (status)
            {
                case 1:
                    grdList.Rows[row].Cells[col].Style.BackColor = Color.Yellow;
                    break;
                case 2:
                case 13:
                case 15:
                case 18:
                case 19:
                    grdList.Rows[row].Cells[col].Style.BackColor = Color.Green;
                    break;
                case 3:
                case 5:
                case 7:
                case 8:
                case 14:
                case 16:
                case 17:
                case 20:
                    grdList.Rows[row].Cells[col].Style.BackColor = Color.Blue;
                    break;
                case 4:
                    grdList.Rows[row].Cells[col].Style.BackColor = Color.Brown;
                    break;
                case 6:
                    grdList.Rows[row].Cells[col].Style.BackColor = Color.Orange;
                    break;
                case 9:
                    grdList.Rows[row].Cells[col].Style.BackColor = Color.Gray;
                    break;
                case 10:
                    grdList.Rows[row].Cells[col].Style.BackColor = Color.Violet;
                    break;
                case 11:
                    grdList.Rows[row].Cells[col].Style.BackColor = Color.Violet;
                    break;
                case 12:
                    grdList.Rows[row].Cells[col].Style.BackColor = Color.White;
                    break;
                case 21:
                    grdList.Rows[row].Cells[col].Style.BackColor = Color.BlueViolet;
                    break;
                default:
                    break;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void initVariables()
        {
            closeAllPanels();
            pnlcolors.Visible = false;
            monthyr = dtMonth.Value.ToString("MM-yyyy");
        }
        private void applyPrivilege()
        {
            try
            {

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
            //cmbfilterOffice.Visible = true;
            //cmbFilterRegion.Visible = true;
            //lblOffice.Visible = true;
            //btnView.Visible = true;
            txtSearch.Text = "";
            pnlgrdList.Visible = true;
            grdList.Visible = true;
            //if(cmbfilterOffice.SelectedItem.ToString()=="All")
            //{
            //    officeid = "All";
            //}
            //else
            //{
            //    officeid = ((Structures.ComboBoxItem)cmbfilterOffice.SelectedItem).HiddenValue;
            //}

            //ListFilteredLeave(getFilterNo(val), officeid);
            if (grdList.RowCount >= 1)
            {
                lblSearch.Visible = true;
                txtSearch.Visible = true;
                //btnExportToExcel.Visible = true;
            }

        }

        private int getFilterNo(int val)
        {
            int no = 0;
            //if (val==1 && cmbfilterOffice.SelectedItem.ToString() == "All" )
            //{
            //    no = 1;
            //}
            //else if(val==1 && cmbfilterOffice.SelectedItem.ToString() != "All" ||
            //        val == 2 && cmbfilterOffice.SelectedItem.ToString() != "All")
            //{
            //    no = 2;
            //}
            //else if(val == 2 && cmbfilterOffice.SelectedItem.ToString() == "All")
            //{
            //    no = 3;
            //}


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
            if (grdList.RowCount >= 1)
            {
                lblSearch.Visible = true;
                txtSearch.Visible = true;
            }
        }

        private void grdList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 2)
                    return;
                ReportAttendanceDB sdb = new ReportAttendanceDB();
                reportattendence empllist = new reportattendence();
                string grdrow = grdList.Rows[e.RowIndex].Cells["EmployeeID"].Value.ToString();
                string empname = grdList.Rows[e.RowIndex].Cells["EmployeeName"].Value.ToString();
                Color color = grdList.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;
                string date = grdList.Columns[e.ColumnIndex].Name;
                empllist = sdb.getEmployeeLatenEarlyList(date, grdrow);
                if (empllist.EmployeeID != null)
                {
                    if (color != Color.Violet && color != Color.BlueViolet)
                    {
                        if(empllist.EntryTime.TimeOfDay==empllist.ExitTime.TimeOfDay)
                        {
                            string empdetail = "Employee ID        :       " + empllist.EmployeeID + "\n" +
                                     "Employee Name :    " + empname + "\n" +
                                     "Date                      :    " + empllist.EntryTime.Date.ToString("dd-MM-yyyy") + "\n" +
                                     "Entry Time           :    " + empllist.EntryTime.TimeOfDay + "";
                            DialogResult dialog = MessageBox.Show(empdetail, "Attendence Details", MessageBoxButtons.OK);
                            if (dialog == DialogResult.OK)
                            {
                                this.DialogResult = DialogResult.Cancel;
                            }
                        }
                        else
                        {
                            string empdetail = "Employee ID        :       " + empllist.EmployeeID + "\n" +
                                      "Employee Name :    " + empname + "\n" +
                                      "Date                      :    " + empllist.EntryTime.Date.ToString("dd-MM-yyyy") + "\n" +
                                      "Entry Time           :    " + empllist.EntryTime.TimeOfDay + "\n" +
                                      "Exit Time              :    " + empllist.ExitTime.TimeOfDay + "";
                            DialogResult dialog = MessageBox.Show(empdetail, "Attendence Details", MessageBoxButtons.OK);
                            if (dialog == DialogResult.OK)
                            {
                                this.DialogResult = DialogResult.Cancel;
                            }
                        }
                        
                    }
                    else
                    {
                        string empdetail = "Employee ID        :       " + empllist.EmployeeID + "\n" +
                                      "Employee Name :    " + empname + "\n" +
                                      "Date                      :    " + empllist.EntryTime.Date.ToString("dd-MM-yyyy") + "\n" +
                                      "Entry Time           :    " + empllist.EntryTime.TimeOfDay + "";
                        DialogResult dialog = MessageBox.Show(empdetail, "Attendence Details", MessageBoxButtons.OK);
                        if (dialog == DialogResult.OK)
                        {
                            this.DialogResult = DialogResult.Cancel;
                        }
                    }
                }
                else
                {
                    string empdetail = "Employee ID        :       " + grdrow + "\n" +
                                      "Employee Name :    " + empname + "\n" +
                                      "Date                      :    " + date + "\n" +
                                      "Entry Time           :    \n" +
                                      "Exit Time              :    ";
                    DialogResult dialog = MessageBox.Show(empdetail, "Attendence Details", MessageBoxButtons.OK);
                    if (dialog == DialogResult.OK)
                    {
                        this.DialogResult = DialogResult.Cancel;
                    }
                }

                //if(empllist.EntryTime.Date.TimeOfDay==empllist.ExitTime.Date.TimeOfDay)
                //{
                //    string empdetail = "Employee ID        :       " + empllist.EmployeeID + "\n" +
                //                     "Employee Name :    " + empname + "\n" +
                //                     "Date                      :    " + empllist.EntryTime.Date.ToString("dd-MM-yyyy") + "\n" +
                //                     "Entry Time           :    " + empllist.EntryTime.TimeOfDay + "";
                //    DialogResult dialog = MessageBox.Show(empdetail, "Attendence Details", MessageBoxButtons.OK);
                //    if (dialog == DialogResult.OK)
                //    {
                //        this.DialogResult = DialogResult.Cancel;
                //    }
                //}
                grdList.ClearSelection();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pnlcolors.Visible = false;
            grdList.Enabled = true;
            txtSearch.Enabled = true;
            dtMonth.Enabled = true;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            pnlcolors.Visible = true;
            grdList.Enabled = false;
            txtSearch.Enabled = false;
            dtMonth.Enabled = false;
        }

        private void ReportAttendance_Enter(object sender, EventArgs e)
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

