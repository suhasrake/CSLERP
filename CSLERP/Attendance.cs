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
    public partial class Attendance : System.Windows.Forms.Form
    {
        string emplOfficeID = "";
        List<attendance> empstatuslist;
        Dictionary<string, string> changedval = new Dictionary<string, string>();
        Dictionary<string, string> Initval = new Dictionary<string, string>();
        Timer filterTimer = new Timer();
        DateTime a;
        string colName = "";
        string systime = Main.SystemParameters.Where(x => x.ID == "AttendanceTime").Select(x => x.Value).FirstOrDefault().ToString();

        public Attendance()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void MenuItem_Load(object sender, EventArgs e)
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
            ListEmployee();
            txtSearch.Text = "Search here....";
            txtSearch.GotFocus += new EventHandler(onfocus);
            txtSearch.LostFocus += new EventHandler(Lostfocus);
        }

        private void ListEmployee()
        {
            try
            {
                grdList.Visible = true;
                grdList.Rows.Clear();
                AttendanceDB dbrecord = new AttendanceDB();
                List<attendance> emplist = new List<attendance>();
                List<attendance> empleavelist = new List<attendance>();
                List<attendance> empbiolist = new List<attendance>();
                List<attendance> empMRlist = new List<attendance>();
                int opt = 0;
                empstatuslist = new List<attendance>();
                if (Main.itemPriv[2])
                {
                    if (cmbfilterOffice.SelectedIndex != -1)
                    {
                        string officeid = ((Structures.ComboBoxItem)cmbfilterOffice.SelectedItem).HiddenValue;
                        emplist = dbrecord.getEmployeeListforoffice(officeid).OrderBy(x => x.EmployeeName).ToList();
                        if (dtpdate.Value.Date <= UpdateTable.getSQLDateTime().Date)
                        {
                            empstatuslist = dbrecord.getEmployeestatusListforoffice(officeid, dtpdate.Value);
                            if (empstatuslist.Count == 0)
                            {
                                empleavelist = dbrecord.getEmployeeLeaveList(dtpdate.Value);
                                empbiolist = dbrecord.getEmployeeBiometricList(dtpdate.Value);
                                empMRlist = dbrecord.getMrEmployeeList(dtpdate.Value);
                            }
                        }
                        else if (dtpdate.Value.Date > UpdateTable.getSQLDateTime().Date)
                        {
                            pnlEditButtons.Visible = false;
                            MessageBox.Show("Please select the date equal to or less than today!!!");
                            return;
                        }
                    }
                }
                else
                {
                    emplist = dbrecord.getEmployeeList().OrderBy(x => x.EmployeeName).ToList();
                    empstatuslist = dbrecord.getEmployeestatusListforoffice(emplOfficeID, UpdateTable.getSQLDateTime());
                    if (empstatuslist.Count == 0)
                    {
                        empleavelist = dbrecord.getEmployeeLeaveList(UpdateTable.getSQLDateTime());
                        empbiolist = dbrecord.getEmployeeBiometricList(UpdateTable.getSQLDateTime());
                        empMRlist = dbrecord.getMrEmployeeList(UpdateTable.getSQLDateTime());
                    }
                }

                foreach (attendance elst in emplist)
                {
                    grdList.Rows.Add();
                    int kount = grdList.RowCount;
                    grdList.Rows[kount - 1].Cells["RowID"].Value = kount;
                    grdList.Rows[kount - 1].Cells["EmployeeID"].Value = Convert.ToInt32(elst.EmployeeID);
                    grdList.Rows[kount - 1].Cells["EmployeeName"].Value = elst.EmployeeName;
                    DataGridViewComboBoxCell cmbAttendanceStatus = (DataGridViewComboBoxCell)(grdList.Rows[kount - 1].Cells["AttendanceStatus"]);
                    CatalogueValueDB.fillCatalogValueGridViewComboNew(cmbAttendanceStatus, "AttendanceStatus");
                    attendance empstatlst = empstatuslist.FirstOrDefault(x => x.EmployeeID == elst.EmployeeID);
                    if (empstatlst == null)
                    {
                        attendance emplvlst = empleavelist.FirstOrDefault(x => x.EmployeeID == elst.EmployeeID);
                        attendance empbiolst = empbiolist.FirstOrDefault(x => x.EmployeeID == elst.EmployeeID);
                        attendance empMRlst = empMRlist.FirstOrDefault(x => x.EmployeeID == elst.EmployeeID);
                        if (emplvlst != null)
                        {
                            grdList.Rows[kount - 1].Cells["AttendanceStatus"].Value = "Leave";
                            Initval.Add(emplvlst.EmployeeID, "Leave");
                            changedval.Add(emplvlst.EmployeeID, "Leave");
                        }
                        else
                        {
                            if (empbiolst != null)
                            {
                                grdList.Rows[kount - 1].Cells["AttendanceStatus"].Value = "Present";
                                Initval.Add(empbiolst.EmployeeID, "Present");
                                changedval.Add(empbiolst.EmployeeID, "Present");
                            }
                            else if (empMRlst != null)
                            {
                                grdList.Rows[kount - 1].Cells["AttendanceStatus"].Value = "LocalTravel";
                                Initval.Add(empMRlst.EmployeeID, "LocalTravel");
                                changedval.Add(empMRlst.EmployeeID, "LocalTravel");
                            }
                        }
                    }
                    else
                    {
                        grdList.Rows[kount - 1].Cells["AttendanceStatus"].Value = empstatlst.AttendenceStatus;
                        Initval.Add(empstatlst.EmployeeID, empstatlst.AttendenceStatus);
                    }
                }
                pnlEditButtons.Visible = false;
                if (!Main.itemPriv[2])
                {
                    DateTime.TryParse(systime, out a);
                    if (grdList.RowCount > 0 && UpdateTable.getSQLDateTime().TimeOfDay < a.TimeOfDay)
                    {
                        pnlEditButtons.Visible = true;
                    }
                }
                else if (grdList.RowCount > 0)
                {
                    pnlEditButtons.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            enableBottomButtons();
            pnlMenuList.Visible = true;
        }

        private void initVariables()
        {
            try
            {

                lbldatetime.Text = UpdateTable.getSQLDateTime().Date.ToString("dd-MM-yyyy");
                int count = grdList.RowCount;
                lblOffice.Visible = false;
                cmbfilterOffice.Visible = false;
                lbldatetime.Visible = false;
                lbldate.Visible = false;
                dtpdate.Visible = false;
                pnlEditButtons.Visible = false;

                if (Main.itemPriv[2])
                {
                    pnlEditButtons.Visible = true;
                    OfficeDB.fillOfficeComboNew(cmbfilterOffice);
                    lblOffice.Visible = true;
                    cmbfilterOffice.Visible = true;
                    lbldate.Visible = true;
                    dtpdate.Visible = true;
                    dtpdate.Format = DateTimePickerFormat.Custom;
                    dtpdate.CustomFormat = "dd-MM-yyyy";
                    dtpdate.Enabled = true;
                    dtpdate.Value = UpdateTable.getSQLDateTime();
                }
                else
                {
                    DateTime.TryParse(systime, out a);
                    if (grdList.RowCount > 0 && UpdateTable.getSQLDateTime().TimeOfDay < a.TimeOfDay)
                    {
                        pnlEditButtons.Visible = true;
                    }
                    lbldatetime.Visible = true;
                }
                emplOfficeID = EmployeeDB.getEmployeeOffice(Login.empLoggedIn);
            }
            catch (Exception)
            {

            }
        }

        int getuserPrivilegeStatus()
        {
            try
            {
                if (Main.itemPriv[0] && !Main.itemPriv[1] && !Main.itemPriv[2]) //only view
                    return 1;
                else if (Main.itemPriv[0] && (Main.itemPriv[1] || Main.itemPriv[2])) //view add or edit
                    return 2;
                else if (!Main.itemPriv[0] && (Main.itemPriv[1] || Main.itemPriv[2])) // no view ,add or edit
                    return 3;
                else if (!Main.itemPriv[0] && !Main.itemPriv[1] || !Main.itemPriv[2]) //no privilege
                    return 0;
                else
                    return -1;
            }
            catch (Exception ex)
            {
            }
            return 0;
        }



        private void closeAllPanels()
        {
            try
            {
                //pnlMenuItemInner.Visible = false;
                //pnlMenuItemOuter.Visible = false;
                pnlMenuList.Visible = false;
            }
            catch (Exception)
            {

            }
        }


        private void fillMenuItemStatusCombo(System.Windows.Forms.ComboBox cmb)
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


        private void btnMenuItemListExit_Click(object sender, EventArgs e)
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

        private void disableBottomButtons()
        {
            //btnNew.Visible = false;
            btnExit.Visible = false;
        }
        private void enableBottomButtons()
        {
            //btnNew.Visible = true;
            btnExit.Visible = true;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (grdList.IsCurrentCellDirty)
                {
                    var currentcell = grdList.CurrentCellAddress;
                    var sendingCB = sender as DataGridViewComboBoxEditingControl;
                    DataGridViewComboBoxCell cel = (DataGridViewComboBoxCell)grdList.Rows[currentcell.Y].Cells[3];
                    string str1 = grdList.Rows[currentcell.Y].Cells["EmployeeID"].Value.ToString();
                    string str = grdList.Rows[currentcell.Y].Cells["AttendanceStatus"].FormattedValue.ToString();
                    if (changedval.ContainsKey(grdList.Rows[currentcell.Y].Cells[1].ToString()))
                    {
                        changedval[grdList.Rows[currentcell.Y].Cells[1].Value.ToString()] = cel.EditedFormattedValue.ToString();//replace value
                    }
                    else
                    {
                        //changedval.Add(grdList.Rows[currentcell.Y].Cells[1].Value.ToString(), cel.EditedFormattedValue.ToString() /*grdList.Rows[currentcell.Y].Cells[3]..ToString()*/);// new value
                        changedval.Add(grdList.Rows[currentcell.Y].Cells[1].Value.ToString(), cel.EditedFormattedValue.ToString()/*grdList.Rows[currentcell.Y].Cells[3]..ToString()*/);// new value
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            try
            {
                var a = changedval.Count();

                foreach (DataGridViewRow rl in grdList.Rows)
                {
                    string rwl = rl.Cells[1].Value.ToString();
                    if (changedval.ContainsKey(rl.Cells[1].Value.ToString()))
                    {
                        rl.Cells[3].Value = null;
                    }
                    if (Initval.ContainsKey(rl.Cells[1].Value.ToString()))
                    {
                        rl.Cells[3].Value = Initval[rl.Cells[1].Value.ToString()];
                    }
                }
                changedval.Clear();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow cb in grdList.Rows)
                {
                    if (cb.Cells[3].Value == null)
                    {
                        MessageBox.Show("Please enter attendence for all Employees!!!");
                        return;
                    }
                }
                attendance att = new attendance();
                List<attendance> attlstnew = new List<attendance>();
                List<attendance> attlstupdt = new List<attendance>();
                AttendanceDB docDB = new AttendanceDB();
                foreach (DataGridViewRow dgvrw in grdList.Rows)
                {
                    if (changedval.ContainsKey(dgvrw.Cells[1].Value.ToString()))
                    {
                        if (Initval.ContainsKey(dgvrw.Cells[1].Value.ToString()))
                        {
                            att = new attendance();
                            att.EmployeeID = dgvrw.Cells[1].Value.ToString();
                            att.AttendenceStatus = dgvrw.Cells[3].Value.ToString();
                            attlstupdt.Add(att);
                        }
                        else
                        {
                            att = new attendance();
                            att.EmployeeID = dgvrw.Cells[1].Value.ToString();
                            att.AttendenceStatus = dgvrw.Cells[3].Value.ToString();
                            attlstnew.Add(att);
                        }
                    }
                }
                DateTime dt = UpdateTable.getSQLDateTime();
                if (Main.itemPriv[2])
                {
                    dt = dtpdate.Value;
                }
                if (docDB.UpdateAttendance(attlstupdt, attlstnew, empstatuslist, dt))
                {
                    MessageBox.Show("Attendance Status Updated");
                    changedval.Clear();
                    Initval.Clear();
                    ListEmployee();
                }
                else
                {
                    MessageBox.Show("Error Updating Attendance Status");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed Adding / Editing User Data");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text != "Search here....")
            {
                filterTimer.Enabled = false;
                filterTimer.Stop();
                filterTimer.Tick -= new System.EventHandler(this.handlefilterTimerTimeout);
                filterTimer.Tick += new System.EventHandler(this.handlefilterTimerTimeout);
                filterTimer.Interval = 500;
                filterTimer.Enabled = true;
                filterTimer.Start();
            }

        }
        private void handlefilterTimerTimeout(object sender, EventArgs e)
        {
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterGridData(colName);
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
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

        public void onfocus(object sender, EventArgs e)
        {
            txtSearch.Text = "";
        }
        public void Lostfocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
                txtSearch.Text = "Search here....";
        }

        private void cmbfilterOffice_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "Search here....";
            changedval.Clear();
            Initval.Clear();
            ListEmployee();
        }

        private void dtpdate_ValueChanged(object sender, EventArgs e)
        {
            txtSearch.Text = "Search here....";
            changedval.Clear();
            Initval.Clear();
            ListEmployee();
        }
    }

}

