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
using CSLERP.FileManager;

namespace CSLERP
{
    public partial class LeaveSettings : System.Windows.Forms.Form
    {
        string selectedCatalogueID = "";
        int option = 0;
        int rowid = 0;
        string prevdesig = "";
        string prevoff = "";
        string prevleaveid = "";
        System.Windows.Forms.Button prevbtn = null;
        Form frmPopup = new Form();
        ListView lv = new ListView();
        ListView lvCopy = new ListView();

        List<office> alloffice;
        public LeaveSettings()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void LeaveSettings_Load(object sender, EventArgs e)
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
            //CreateCataloueButtons();
        }

        private void initVariables()
        {

            //249, 29
            pnlLeaveSanctionLimitOuter.Location = new Point(249, 20);
            pnlLeaveTypeOuter.Location = new Point(249, 20);
            pnlLeaveOfficeMapOuter.Location = new Point(249, 20);

            pnlLeaveSanctionLimitOuter.Parent = pnlUI;
            pnlLeaveTypeOuter.Parent = pnlUI;
            pnlLeaveOfficeMapOuter.Parent = pnlUI;

            pnlLeaveSanctionLimitInner.Parent = pnlLeaveSanctionLimitOuter;
            pnlLeaveTypeInner.Parent = pnlLeaveTypeOuter;
            pnlLeaveOfficeMapInner.Parent = pnlLeaveOfficeMapOuter;

            dtHWDate.Format = DateTimePickerFormat.Custom;
            dtHWDate.CustomFormat = "dd-MM-yyyy";
            //dtHWDate.Enabled = false;
            OfficeDB ofdb = new OfficeDB();
            alloffice = ofdb.getOffices();
            OfficeDB.fillOfficeComboNew(cmbLomOffice);
            CatalogueValueDB.fillCatalogValueComboNew(cmbLslDesignation, "Designation");
            CatalogueValueDB.fillCatalogValueComboNew(cmbGender, "Gender");
            LeaveSettingsdb.fillLeaveComboNew(cmbLomLeaveID);
            LeaveSettingsdb.fillLeaveComboNew(cmbLslLeaveID);
            cmbGender.Items.Add("All");
            btnNew.Visible = false;

            pnlLeaveOfficeMapOuter.Visible = false;
            pnlLeaveSanctionLimitOuter.Visible = false;
            pnlLeaveTypeOuter.Visible = false;
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

        public void init(int opt)
        {
            pnlLeaveList.Visible = false;
            pnlOfficeHW.Visible = false;
            if (opt == 1)
            {
                grdList.Visible = false;
                pnlLeaveTypeOuter.Visible = true;
                pnlLeaveTypeInner.Visible = true;

                pnlLeaveSanctionLimitOuter.Visible = false;
                pnlLeaveSanctionLimitInner.Visible = false;

                pnlLeaveOfficeMapOuter.Visible = false;
                pnlLeaveOfficeMapInner.Visible = false;

                pnlHWOuter.Visible = false;
                pnlHWInner.Visible = false;

                pnlWOInner.Visible = false;
                pnlWOOuter.Visible = false;

                txtLeaveID.Enabled = true;
                txtlDescription.Enabled = true;
                txtMaxSanctionLimit.Enabled = true;
                cmbSanctionType.Enabled = true;
                cmbCarryForward.SelectedIndex = 0;
            }
            else if (opt == 2)
            {
                grdList.Visible = false;
                pnlLeaveTypeOuter.Visible = false;
                pnlLeaveTypeInner.Visible = false;

                pnlLeaveSanctionLimitOuter.Visible = true;
                pnlLeaveSanctionLimitInner.Visible = true;

                pnlLeaveOfficeMapOuter.Visible = false;
                pnlLeaveOfficeMapInner.Visible = false;

                pnlHWOuter.Visible = false;
                pnlHWInner.Visible = false;

                pnlWOInner.Visible = false;
                pnlWOOuter.Visible = false;

                cmbLslLeaveID.Enabled = true;
                txtLslMaxSanctionLimit.Enabled = true;
                cmbLslDesignation.Enabled = true;

            }
            else if (opt == 3)
            {
                grdList.Visible = false;
                pnlLeaveTypeOuter.Visible = false;
                pnlLeaveTypeInner.Visible = false;

                pnlLeaveSanctionLimitOuter.Visible = false;
                pnlLeaveSanctionLimitInner.Visible = false;

                pnlLeaveOfficeMapOuter.Visible = true;
                pnlLeaveOfficeMapInner.Visible = true;

                pnlHWOuter.Visible = false;
                pnlHWInner.Visible = false;

                pnlWOInner.Visible = false;
                pnlWOOuter.Visible = false;

                cmbLomLeaveID.Enabled = true;
                txtLomMaxDays.Enabled = true;
                cmbLomOffice.Enabled = true;
            }
            else if (opt == 4 || opt == 6)
            {
                dgvWO.Visible = false;
                grdOfficeHW.Visible = false;
                pnlLeaveTypeOuter.Visible = false;
                pnlLeaveTypeInner.Visible = false;

                pnlLeaveSanctionLimitOuter.Visible = false;
                pnlLeaveSanctionLimitInner.Visible = false;

                pnlLeaveOfficeMapOuter.Visible = false;
                pnlLeaveOfficeMapInner.Visible = false;
                pnlWOInner.Visible = false;
                pnlWOOuter.Visible = false;

                pnlHWOuter.Visible = true;
                pnlHWInner.Visible = true;
                btnofficeSelect.Enabled = true;
                cmbType.Enabled = true;
                if (opt == 6)
                {
                    cmbType.Visible = false;
                    label4.Visible = false;
                    dtHWDate.Enabled = true;
                }
            }
            else if (opt == 5)
            {
                dgvWO.Visible = false;
                grdOfficeHW.Visible = false;
                pnlLeaveTypeOuter.Visible = false;
                pnlLeaveTypeInner.Visible = false;

                pnlLeaveSanctionLimitOuter.Visible = false;
                pnlLeaveSanctionLimitInner.Visible = false;

                pnlLeaveOfficeMapOuter.Visible = false;
                pnlLeaveOfficeMapInner.Visible = false;

                pnlHWOuter.Visible = false;
                pnlHWInner.Visible = false;

                pnlWOInner.Visible = true;
                pnlWOOuter.Visible = true;

                btnOfficeWoSelect.Enabled = true;
                //cmbType.Enabled = true;
            }
        }

        private void closeAllPanels()
        {
            try
            {
                pnlLeaveTypeOuter.Visible = false;
                pnlLeaveSanctionLimitOuter.Visible = false;
                pnlLeaveOfficeMapOuter.Visible = false;
                pnlLeaveTypeInner.Visible = false;
                pnlLeaveSanctionLimitInner.Visible = false;
                pnlLeaveOfficeMapInner.Visible = false;
                pnlHWOuter.Visible = false;
                pnlHWInner.Visible = false;
                pnlWOInner.Visible = false;
                pnlWOOuter.Visible = false;
                pnlOfficeHW.Visible = false;


            }
            catch (Exception)
            {

            }
        }


        private void fillStatusCombo(System.Windows.Forms.ComboBox cmb)
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
                clearData();
                grdList.Visible = true;
                pnlLeaveList.Visible = true;
                btnNew.Visible = true;
                enableBottomButtons();
            }
            catch (Exception)
            {

            }
        }

        public void clearData()
        {
            try
            {
                txtLeaveID.Text = "";
                txtlDescription.Text = "";
                txtLomMaxDays.Text = "";
                txtLslMaxSanctionLimit.Text = "";
                txtMaxSanctionLimit.Text = "";
                rtbOffice.Text = "";
                txtOfficeID.Text = "";
                rtbDescription.Text = "";
                txtOfficeWO.Text = "";
                dtHWDate.Value = DateTime.Now;
                cmbType.SelectedIndex = 0;
                cmbLslLeaveID.SelectedIndex = 0;
                cmbLomLeaveID.SelectedIndex = 0;
                cmbLomOffice.SelectedIndex = 0;
                cmbLslDesignation.SelectedIndex = 0;
                cmbSanctionType.SelectedIndex = 0;
                cmbGender.SelectedIndex = 0;
                foreach (int i in chbWO.CheckedIndices)
                {
                    chbWO.SetItemCheckState(i, CheckState.Unchecked);
                }
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
                clearData();
                btnSave.Text = "Save";
                btnLslSave.Text = "Save";
                btnLomSave.Text = "Save";
                button2.Text = "Save";
                btnWOSave.Text = "Save";
                init(option);
                disableBottomButtons();
            }
            catch (Exception)
            {

            }
        }

        public static int status(string StatusString)
        {
            int StatusCode = 0;
            try
            {
                if (StatusString == "HR")
                {
                    StatusCode = 2;
                }
                if (StatusString == "Department Head")
                {
                    StatusCode = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");

            }
            return StatusCode;
        }

        public static string strstatus(int StatusString)
        {
            string StatusCode = "";
            try
            {
                if (StatusString == 2)
                {
                    StatusCode = "HR";
                }
                if (StatusString == 1)
                {
                    StatusCode = "Department Head";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");

            }
            return StatusCode;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Leave leave = new Leave();
                LeaveSettingsdb leaveDB = new LeaveSettingsdb();
                if (option == 1)
                {
                    leave.leaveID = txtLeaveID.Text;
                    leave.description = txtlDescription.Text;
                    leave.MaxAccrual = Convert.ToInt32(txtMaxSanctionLimit.Text);
                    leave.SanctionType = status(cmbSanctionType.SelectedItem.ToString());
                    leave.ahead = Convert.ToInt32(txtDaysAhead.Text);
                    leave.Delay = Convert.ToInt32(txtDaysDelayed.Text);
                    leave.CarryForward = getcarryforwardint(cmbCarryForward.SelectedItem.ToString());
                    if (cmbGender.SelectedItem.ToString() == "All")
                    {
                        leave.Gender = cmbGender.SelectedItem.ToString();
                    }
                    else
                    {
                        leave.Gender = ((Structures.ComboBoxItem)cmbGender.SelectedItem).HiddenValue;
                    }
                    if (leaveDB.ValidateLeaveType(leave))
                    {
                        if (btnSave.Text.Equals("Update"))
                        {
                            if (leaveDB.UpdateLeaveType(leave))
                            {
                                MessageBox.Show("LeaveType updated");
                                closeAllPanels();
                                leavetype();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update LeaveType");
                            }
                        }
                        else if (btnSave.Text.Equals("Save"))
                        {
                            if (leaveDB.validateLeaveType(leave))
                            {
                                if (leaveDB.InsertLeaveType(leave))
                                {
                                    MessageBox.Show("LeaveType Value Added");
                                    closeAllPanels();
                                    leavetype();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to Insert LeaveType");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Leave Type Already exists!!!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("LeaveType Data Validation failed");
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
            try
            {
                if (e.RowIndex < 0)
                    return;
                string col = grdList.Columns[e.ColumnIndex].HeaderText;
                if (col == "Edit")
                {
                    rowid = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["lRowID"].Value.ToString());

                    if (option == 1)
                    {
                        btnSave.Text = "Update";
                        pnlLeaveTypeInner.Visible = true;
                        pnlLeaveTypeOuter.Visible = true;
                        pnlLeaveList.Visible = false;
                        txtLeaveID.Enabled = false;
                        txtlDescription.Enabled = true;
                        cmbSanctionType.Enabled = true;
                        cmbSanctionType.SelectedIndex = cmbSanctionType.FindStringExact(grdList.Rows[e.RowIndex].Cells["lSanctionType"].Value.ToString());
                        cmbCarryForward.SelectedIndex = cmbCarryForward.FindStringExact(grdList.Rows[e.RowIndex].Cells["Carryforward"].Value.ToString());
                        if (grdList.Rows[e.RowIndex].Cells["lGender"].Value.ToString() == "All")
                        {
                            cmbGender.SelectedIndex = cmbGender.FindString(grdList.Rows[e.RowIndex].Cells["lGender"].Value.ToString());
                        }
                        else
                        {
                            cmbGender.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbGender, grdList.Rows[e.RowIndex].Cells["lGender"].Value.ToString());
                        }
                        txtLeaveID.Text = grdList.Rows[e.RowIndex].Cells["LeaveID"].Value.ToString();
                        txtlDescription.Text = grdList.Rows[e.RowIndex].Cells["lDescription"].Value.ToString();
                        txtMaxSanctionLimit.Text = grdList.Rows[e.RowIndex].Cells["lMaxSanctionLimit"].Value.ToString();
                        txtDaysAhead.Text = grdList.Rows[e.RowIndex].Cells["DaysAhead"].Value.ToString();
                        txtDaysDelayed.Text = grdList.Rows[e.RowIndex].Cells["DaysDelayed"].Value.ToString();
                        prevleaveid = grdList.Rows[e.RowIndex].Cells["LeaveID"].Value.ToString();
                        disableBottomButtons();
                    }
                    if (option == 2)
                    {
                        btnLslSave.Text = "Update";
                        pnlLeaveSanctionLimitInner.Visible = true;
                        pnlLeaveSanctionLimitOuter.Visible = true;
                        pnlLeaveList.Visible = false;
                        cmbLslLeaveID.Enabled = false;
                        cmbLslDesignation.Enabled = true;
                        txtLslMaxSanctionLimit.Enabled = true;
                        cmbLslLeaveID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbLslLeaveID, grdList.Rows[e.RowIndex].Cells["LeaveID"].Value.ToString());
                        cmbLslDesignation.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbLslDesignation, grdList.Rows[e.RowIndex].Cells["lDesignation"].Value.ToString());
                        txtLslMaxSanctionLimit.Text = grdList.Rows[e.RowIndex].Cells["lMaxSanctionLimit"].Value.ToString();
                        prevdesig = cmbLslDesignation.SelectedItem.ToString();
                        disableBottomButtons();
                    }
                    if (option == 3)
                    {
                        btnLomSave.Text = "Update";
                        pnlLeaveOfficeMapInner.Visible = true;
                        pnlLeaveOfficeMapOuter.Visible = true;
                        pnlLeaveList.Visible = false;
                        cmbLomLeaveID.Enabled = false;
                        cmbLomOffice.Enabled = true;
                        txtLomMaxDays.Enabled = true;
                        cmbLomLeaveID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbLomLeaveID, grdList.Rows[e.RowIndex].Cells["LeaveID"].Value.ToString());
                        cmbLomOffice.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbLomOffice, grdList.Rows[e.RowIndex].Cells["lOfficeID"].Value.ToString());
                        txtLomMaxDays.Text = grdList.Rows[e.RowIndex].Cells["lMaxDays"].Value.ToString();
                        prevoff = cmbLomOffice.SelectedItem.ToString();
                        disableBottomButtons();

                    }
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

        private void btnLeaveType_Click(object sender, EventArgs e)
        {
            OfficeDB.fillOfficeComboNew(cmbLomOffice);
            CatalogueValueDB.fillCatalogValueComboNew(cmbLslDesignation, "Designation");
            CatalogueValueDB.fillCatalogValueComboNew(cmbGender, "Gender");
            LeaveSettingsdb.fillLeaveComboNew(cmbLomLeaveID);
            LeaveSettingsdb.fillLeaveComboNew(cmbLslLeaveID);
            cmbGender.Items.Add("All");
            leavetype();
        }
        void leavetype()
        {
            option = 1;
            try
            {
                grdList.Rows.Clear();
                LeaveSettingsdb record = new LeaveSettingsdb();
                List<Leave> leavetype = record.getLeaveTypeList();
                foreach (Leave leave in leavetype)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["lSerialNo"].Value = grdList.Rows.Count;
                    grdList.Rows[grdList.RowCount - 1].Cells["LeaveID"].Value = leave.leaveID;
                    grdList.Rows[grdList.RowCount - 1].Cells["lDescription"].Value = leave.description;
                    grdList.Rows[grdList.RowCount - 1].Cells["lMaxSanctionLimit"].Value = leave.MaxAccrual;
                    grdList.Rows[grdList.RowCount - 1].Cells["lSanctionType"].Value = strstatus(leave.SanctionType);
                    grdList.Rows[grdList.RowCount - 1].Cells["lGender"].Value = leave.Gender;
                    grdList.Rows[grdList.RowCount - 1].Cells["lRowID"].Value = leave.rowid;
                    grdList.Rows[grdList.RowCount - 1].Cells["DaysAhead"].Value = leave.ahead;
                    grdList.Rows[grdList.RowCount - 1].Cells["DaysDelayed"].Value = leave.Delay;
                    grdList.Rows[grdList.RowCount - 1].Cells["Carryforward"].Value = getcarryforwardstr(leave.CarryForward);
                }
                setvisiblity(option);
                closeAllPanels();
                pnlLeaveList.Visible = true;
                enableBottomButtons();

                pnlBottomButtons.Visible = true;
            }
            catch (Exception ex)
            {

            }
        }

        public string getcarryforwardstr(int val)
        {
            string info = "";
            if (val == 0)
            {
                info = "No";
            }
            else if (val == 1)
            {
                info = "Yes";
            }
            return info;
        }

        public int getcarryforwardint(string val)
        {
            int info = 2;
            if (val == "No")
            {
                info = 0;
            }
            else if (val == "Yes")
            {
                info = 1;
            }
            return info;
        }


        private void btnLeaveSanctionLimit_Click(object sender, EventArgs e)
        {
            OfficeDB.fillOfficeComboNew(cmbLomOffice);
            CatalogueValueDB.fillCatalogValueComboNew(cmbLslDesignation, "Designation");
            CatalogueValueDB.fillCatalogValueComboNew(cmbGender, "Gender");
            LeaveSettingsdb.fillLeaveComboNew(cmbLomLeaveID);
            LeaveSettingsdb.fillLeaveComboNew(cmbLslLeaveID);
            cmbGender.Items.Add("All");
            leavesanctionlimit();
        }
        void leavesanctionlimit()
        {
            option = 2;
            try
            {
                grdList.Rows.Clear();
                LeaveSettingsdb record = new LeaveSettingsdb();
                List<Leave> leavetype = record.getSanctionLimitList();
                foreach (Leave leave in leavetype)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["lSerialNo"].Value = grdList.Rows.Count;
                    grdList.Rows[grdList.RowCount - 1].Cells["LeaveID"].Value = leave.leaveID;
                    grdList.Rows[grdList.RowCount - 1].Cells["lDesignation"].Value = leave.designation;
                    grdList.Rows[grdList.RowCount - 1].Cells["lMaxSanctionLimit"].Value = leave.MaxAccrual;
                    grdList.Rows[grdList.RowCount - 1].Cells["lRowID"].Value = leave.rowid;
                }
                setvisiblity(option);
                closeAllPanels();
                pnlLeaveList.Visible = true;
                enableBottomButtons();
                pnlBottomButtons.Visible = true;
            }
            catch (Exception ex)
            {

            }
        }

        private void btnLeaveOfficeMapping_Click(object sender, EventArgs e)
        {
            OfficeDB.fillOfficeComboNew(cmbLomOffice);
            CatalogueValueDB.fillCatalogValueComboNew(cmbLslDesignation, "Designation");
            CatalogueValueDB.fillCatalogValueComboNew(cmbGender, "Gender");
            LeaveSettingsdb.fillLeaveComboNew(cmbLomLeaveID);
            cmbGender.Items.Add("All");
            LeaveSettingsdb.fillLeaveComboNew(cmbLslLeaveID);
            leaveofficemapping();
        }
        void leaveofficemapping()
        {
            option = 3;
            try
            {
                grdList.Rows.Clear();
                LeaveSettingsdb record = new LeaveSettingsdb();
                List<Leave> leavetype = record.getleaveofficemappingList();

                foreach (Leave leave in leavetype)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["lSerialNo"].Value = grdList.Rows.Count;
                    grdList.Rows[grdList.RowCount - 1].Cells["LeaveID"].Value = leave.leaveID;
                    grdList.Rows[grdList.RowCount - 1].Cells["lOfficeID"].Value = leave.officeID;
                    grdList.Rows[grdList.RowCount - 1].Cells["OfficeName"].Value = leave.officeName;
                    grdList.Rows[grdList.RowCount - 1].Cells["lMaxDays"].Value = leave.MaxDays;
                    grdList.Rows[grdList.RowCount - 1].Cells["lRowID"].Value = leave.rowid;
                }
                setvisiblity(option);
                closeAllPanels();
                pnlLeaveList.Visible = true;
                enableBottomButtons();
                pnlBottomButtons.Visible = true;
            }
            catch (Exception ex)
            {

            }
        }

        void ListOfficeHW()
        {
            option = 4;
            try
            {
                grdOfficeHW.Rows.Clear();
                LeaveSettingsdb record = new LeaveSettingsdb();
                List<HolidayList> OfficeHWlst = record.getOfficeHW();

                foreach (HolidayList Ohw in OfficeHWlst)
                {
                    grdOfficeHW.Rows.Add();
                    grdOfficeHW.Rows[grdOfficeHW.RowCount - 1].Cells["Slno"].Value = grdOfficeHW.Rows.Count;
                    grdOfficeHW.Rows[grdOfficeHW.RowCount - 1].Cells["gRowID"].Value = Ohw.RowID;
                    grdOfficeHW.Rows[grdOfficeHW.RowCount - 1].Cells["Type"].Value = Ohw.Type;
                    grdOfficeHW.Rows[grdOfficeHW.RowCount - 1].Cells["Date"].Value = Ohw.date.ToString("dd-MM-yyyy");
                    grdOfficeHW.Rows[grdOfficeHW.RowCount - 1].Cells["OfficeIDs"].Value = Ohw.officeID;
                    grdOfficeHW.Rows[grdOfficeHW.RowCount - 1].Cells["gOfficeName"].Value = getoffcename(Ohw.officeID, option);
                    grdOfficeHW.Rows[grdOfficeHW.RowCount - 1].Cells["Description"].Value = Ohw.Description;
                }
                setvisiblity(option);

                closeAllPanels();
                pnlLeaveList.Visible = false;
                pnlOfficeHW.Visible = true;
                enableBottomButtons();
                pnlBottomButtons.Visible = true;
            }
            catch (Exception ex)
            {

            }
        }

        void ListOfficeWO()
        {
            option = 5;
            try
            {
                dgvWO.Rows.Clear();
                LeaveSettingsdb record = new LeaveSettingsdb();
                List<HolidayList> OfficeHWlst = record.getOfficeWO();

                foreach (HolidayList Ohw in OfficeHWlst)
                {
                    dgvWO.Rows.Add();
                    dgvWO.Rows[dgvWO.RowCount - 1].Cells["gSlno"].Value = dgvWO.Rows.Count;
                    dgvWO.Rows[dgvWO.RowCount - 1].Cells["wRowID"].Value = Ohw.RowID;
                    dgvWO.Rows[dgvWO.RowCount - 1].Cells["wOfficeID"].Value = Ohw.officeID;
                    dgvWO.Rows[dgvWO.RowCount - 1].Cells["wOfficeName"].Value = alloffice.Where(x=>x.OfficeID==Ohw.officeID).Select(x=>x.name).FirstOrDefault() ;
                    if(Ohw.Weekoffs != "")
                    {
                        dgvWO.Rows[dgvWO.RowCount - 1].Cells["WeekOff"].Value = Ohw.Weekoffs.Substring(0, Ohw.Weekoffs.Length - 1).Replace(Main.delimiter1, ',');
                    }
                    
                }
                setvisiblity(option);

                closeAllPanels();
                pnlLeaveList.Visible = false;
                pnlOfficeHW.Visible = true;
                enableBottomButtons();
                pnlBottomButtons.Visible = true;
            }
            catch (Exception ex)
            {

            }
        }

        void ListOfficeWD()
        {
            option = 6;
            try
            {
                grdOfficeHW.Rows.Clear();
                LeaveSettingsdb record = new LeaveSettingsdb();
                List<HolidayList> OfficeWDlst = record.getOfficeWD();

                foreach (HolidayList Owd in OfficeWDlst)
                {
                    grdOfficeHW.Rows.Add();
                    grdOfficeHW.Rows[grdOfficeHW.RowCount - 1].Cells["Slno"].Value = grdOfficeHW.Rows.Count;
                    grdOfficeHW.Rows[grdOfficeHW.RowCount - 1].Cells["gRowID"].Value = Owd.RowID;
                    grdOfficeHW.Rows[grdOfficeHW.RowCount - 1].Cells["Type"].Value = Owd.Type;
                    grdOfficeHW.Rows[grdOfficeHW.RowCount - 1].Cells["Date"].Value = Owd.date.ToString("dd-MM-yyyy");
                    grdOfficeHW.Rows[grdOfficeHW.RowCount - 1].Cells["OfficeIDs"].Value = Owd.officeID;
                    grdOfficeHW.Rows[grdOfficeHW.RowCount - 1].Cells["gOfficeName"].Value = getoffcename(Owd.officeID, option);
                    grdOfficeHW.Rows[grdOfficeHW.RowCount - 1].Cells["Description"].Value = Owd.Description;
                }
                setvisiblity(option);

                closeAllPanels();
                pnlLeaveList.Visible = false;
                pnlOfficeHW.Visible = true;
                enableBottomButtons();
                pnlBottomButtons.Visible = true;
            }
            catch (Exception ex)
            {

            }
        }

        public string getoffcename(string oid, int opt)
        {
            string nme = "";
            string[] officeidlist = oid.Split(Main.delimiter1);
            int count = 1;
            foreach (string id in officeidlist)
            {
                if (opt == 4 || opt == 6)
                {
                    if (id != "")
                    {
                        office ofid = alloffice.Where(x => x.OfficeID == id).FirstOrDefault();
                        if (count == 1)
                        {
                            nme = count + "." + ofid.name;
                            count++;
                        }
                        else
                        {
                            nme = nme + "," + count + "." + ofid.name;
                            count++;
                        }
                    }
                }
                else
                {

                    if (id != "")
                    {
                        office ofid = alloffice.Where(x => x.OfficeID == id).FirstOrDefault();
                        if (nme == "")
                        {
                            nme = ofid.name;
                        }
                        else
                        {
                            nme = nme + "\n" + ofid.name;
                            count++;
                        }
                    }
                }


            }

            return nme;
        }

        public void setvisiblity(int opt)
        {

            if (opt == 1)
            {
                grdList.Visible = true;
                grdList.Columns["lSanctionType"].Visible = true;
                grdList.Columns["lMaxSanctionLimit"].Visible = true;
                grdList.Columns["lDescription"].Visible = true;
                grdList.Columns["lGender"].Visible = true;
                grdList.Columns["lOfficeID"].Visible = false;
                grdList.Columns["OfficeName"].Visible = false;
                grdList.Columns["lMaxDays"].Visible = false;
                grdList.Columns["lDesignation"].Visible = false;
                grdList.Columns["Carryforward"].Visible = true;
                grdList.Columns["DaysAhead"].Visible = true;
                grdList.Columns["DaysDelayed"].Visible = true;
                grdOfficeHW.Visible = false;

            }
            else if (opt == 2)
            {
                grdList.Visible = true;
                grdList.Columns["lDesignation"].Visible = true;
                grdList.Columns["lMaxSanctionLimit"].Visible = true;
                grdList.Columns["lOfficeID"].Visible = false;
                grdList.Columns["OfficeName"].Visible = false;
                grdList.Columns["lMaxDays"].Visible = false;
                grdList.Columns["lDescription"].Visible = false;
                grdList.Columns["lSanctionType"].Visible = false;
                grdList.Columns["lGender"].Visible = false;
                grdList.Columns["lGender"].Visible = false;
                grdList.Columns["DaysAhead"].Visible = false;
                grdList.Columns["DaysDelayed"].Visible = false;
                grdList.Columns["Carryforward"].Visible = false;
                grdOfficeHW.Visible = false;

            }
            else if (opt == 3)
            {
                grdList.Visible = true;
                grdList.Columns["lMaxSanctionLimit"].Visible = false;
                grdList.Columns["lDescription"].Visible = false;
                grdList.Columns["lSanctionType"].Visible = false;
                grdList.Columns["lDesignation"].Visible = false;
                grdList.Columns["lOfficeID"].Visible = false;
                grdList.Columns["OfficeName"].Visible = true;
                grdList.Columns["lMaxDays"].Visible = true;
                grdList.Columns["lGender"].Visible = false;
                grdList.Columns["DaysAhead"].Visible = false;
                grdList.Columns["DaysDelayed"].Visible = false;
                grdList.Columns["Carryforward"].Visible = false;
                grdOfficeHW.Visible = false;
            }
            else if (opt == 4)
            {
                grdList.Visible = false;
                grdOfficeHW.Visible = true;
                dgvWO.Visible = false;
                cmbType.Visible = true;
                label4.Visible = true;
                dtHWDate.Enabled = true;
            }
            else if (opt == 5)
            {
                grdList.Visible = false;
                grdOfficeHW.Visible = false;
                dgvWO.Visible = true;
            }
            else if (opt == 6)
            {
                grdList.Visible = false;
                grdOfficeHW.Visible = true;
                dgvWO.Visible = false;
                cmbType.Visible = false;
                label4.Visible = false;
            }
        }

        private void btnLomCancel_Click(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                clearData();
                grdList.Visible = true;
                pnlLeaveList.Visible = true;
                btnNew.Visible = true;
                enableBottomButtons();
            }
            catch (Exception)
            {

            }
        }

        private void btnLslCancel_Click(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                clearData();
                pnlLeaveList.Visible = true;
                btnNew.Visible = true;
                grdList.Visible = true;
                enableBottomButtons();
            }
            catch (Exception)
            {

            }
        }

        private void btnLslSave_Click(object sender, EventArgs e)
        {
            try
            {
                Leave leave = new Leave();
                LeaveSettingsdb leaveDB = new LeaveSettingsdb();
                if (option == 2)
                {

                    leave.leaveID = ((Structures.ComboBoxItem)cmbLslLeaveID.SelectedItem).HiddenValue;
                    leave.designation = ((Structures.ComboBoxItem)cmbLslDesignation.SelectedItem).HiddenValue;
                    leave.MaxAccrual = Convert.ToInt32(txtLslMaxSanctionLimit.Text.ToString());
                    if (leaveDB.ValidateLeaveSanctionLimit(leave))
                    {
                        if (btnLslSave.Text.Equals("Update"))
                        {
                            if (prevdesig != cmbLslDesignation.SelectedItem.ToString())
                            {
                                if (leaveDB.validateSanctionLimitList(leave))
                                {
                                    leave.rowid = rowid;
                                    if (leaveDB.UpdateLeaveSanctionLimit(leave))
                                    {
                                        MessageBox.Show("LeaveSanctionLimit updated");
                                        closeAllPanels();
                                        leavesanctionlimit();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to update LeaveSanctionLimit");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Sanction Limit for this data Already Exists!!!");
                                }
                            }
                            else
                            {
                                leave.rowid = rowid;
                                if (leaveDB.UpdateLeaveSanctionLimit(leave))
                                {
                                    MessageBox.Show("LeaveSanctionLimit updated");
                                    closeAllPanels();
                                    leavesanctionlimit();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update LeaveSanctionLimit");
                                }
                            }

                        }
                        else if (btnLslSave.Text.Equals("Save"))
                        {
                            if (leaveDB.validateSanctionLimitList(leave))
                            {
                                if (leaveDB.InsertLeaveSanctionLimit(leave))
                                {
                                    MessageBox.Show("LeaveSanctionLimit Value Added");
                                    closeAllPanels();
                                    leavesanctionlimit();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to Insert LeaveSanctionLimit");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Sanction Limit for this data Already Exists!!!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("LeaveSanctionLimit Data Validation failed");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Failed Adding / Editing User Data");
            }
        }

        private void btnLomSave_Click(object sender, EventArgs e)
        {
            try
            {
                Leave leave = new Leave();
                LeaveSettingsdb leaveDB = new LeaveSettingsdb();
                if (option == 3)
                {
                    leave.leaveID = ((Structures.ComboBoxItem)cmbLomLeaveID.SelectedItem).HiddenValue;
                    leave.officeID = ((Structures.ComboBoxItem)cmbLomOffice.SelectedItem).HiddenValue;
                    leave.MaxDays = Convert.ToInt32(txtLomMaxDays.Text);
                    if (leaveDB.ValidateLeaveOfficeMapping(leave))
                    {
                        if (btnLomSave.Text.Equals("Update"))
                        {
                            if (prevoff != cmbLomOffice.SelectedItem.ToString())
                            {
                                if (leaveDB.Validatemapping(leave))
                                {
                                    leave.rowid = rowid;
                                    if (leaveDB.UpdateLeaveOfficeMapping(leave))
                                    {
                                        MessageBox.Show("LeaveOfficeMapping updated");
                                        closeAllPanels();
                                        leaveofficemapping();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to update LeaveOfficeMapping");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Mapping Already Exists");
                                }
                            }
                            else
                            {
                                leave.rowid = rowid;
                                if (leaveDB.UpdateLeaveOfficeMapping(leave))
                                {
                                    MessageBox.Show("LeaveOfficeMapping updated");
                                    closeAllPanels();
                                    leaveofficemapping();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update LeaveOfficeMapping");
                                }
                            }
                        }
                        else if (btnLomSave.Text.Equals("Save"))
                        {
                            if (leaveDB.Validatemapping(leave))
                            {
                                if (leaveDB.InsertLeaveofficeMapping(leave))
                                {
                                    MessageBox.Show("LeaveOfficeMapping Value Added");
                                    closeAllPanels();
                                    leaveofficemapping();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to Insert LeaveOfficeMapping");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Mapping already Exists!!!!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("LeaveOfficeMapping Data Validation failed");
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing User Data");
            }
        }

        private void cmbCarryForward_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void LeaveSettings_Enter(object sender, EventArgs e)
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

        private void button1_Click(object sender, EventArgs e)
        {
            //grdList.Visible = false;
            try
            {
                CatalogueValueDB.fillCatalogValueComboNew(cmbType, "HolidayType");
                cmbType.SelectedIndex = 0;
                ListOfficeHW();
            }
            catch (Exception ex)
            {

            }
           
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                HolidayList OHW = new HolidayList();
                LeaveSettingsdb leaveDB = new LeaveSettingsdb();
                if (option == 4)
                {
                    OHW.Type = ((Structures.ComboBoxItem)cmbType.SelectedItem).HiddenValue;
                    OHW.date = dtHWDate.Value;
                    OHW.officeID = txtOfficeID.Text.ToString();
                    OHW.Description = rtbDescription.Text.ToString();

                    if (leaveDB.ValidateOfficeHW(OHW))
                    {
                        if (button2.Text.Equals("Update"))
                        {
                            OHW.RowID = rowid;
                            if (leaveDB.UpdateOfficeHW(OHW))
                            {
                                MessageBox.Show("OfficeHW updated");
                                closeAllPanels();
                                ListOfficeHW();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update OfficeHW");
                            }
                        }
                        else if (button2.Text.Equals("Save"))
                        {
                            if (leaveDB.validateOfficeHw(OHW))
                            {
                                if (leaveDB.InsertLeaveOfficeHW(OHW))
                                {
                                    MessageBox.Show("OfficeHW Value Added");
                                    closeAllPanels();
                                    ListOfficeHW();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to Insert OfficeHW");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Type Already exists for the day!!!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("LeaveType Data Validation failed");
                    }
                }
                else if (option == 6)
                {

                    OHW.Type = "WD";
                    OHW.date = dtHWDate.Value;
                    OHW.officeID = txtOfficeID.Text.ToString();
                    OHW.Description = rtbDescription.Text.ToString();
                    List<string> officeHOlst = leaveDB.validateOfficeWDforHW(OHW);
                    List<string> officeWOlst = leaveDB.validateOfficeWDforWO(OHW);
                    List<string> commonItems = officeHOlst.Intersect(officeWOlst).ToList();
                    if(commonItems.Count > 0)
                    {
                        StringBuilder ofc = new StringBuilder();
                        foreach (string offc in commonItems)
                        {
                            if (ofc.Length == 0)
                            {
                                ofc.Append(alloffice.Where(x => x.OfficeID == offc).Select(x => x.name).FirstOrDefault());
                            }
                            else
                            {
                                ofc.Append("," + alloffice.Where(x => x.OfficeID == offc).Select(x => x.name).FirstOrDefault());
                            }
                        }
                        MessageBox.Show( ofc + " does not have a Holiday/WeeklyOff On this day");
                        return;
                    }
                    
                    if (leaveDB.ValidateOfficeHW(OHW))
                    {
                        if (button2.Text.Equals("Update"))
                        {
                            OHW.RowID = rowid;
                            if (leaveDB.UpdateOfficeWD(OHW))
                            {
                                MessageBox.Show("OfficeWD updated");
                                closeAllPanels();
                                ListOfficeWD();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update OfficeWD");
                            }
                        }
                        else if (button2.Text.Equals("Save"))
                        {
                            if (leaveDB.validateOfficeWD(OHW))
                            {
                                if (leaveDB.InsertLeaveOfficeWD(OHW))
                                {
                                    MessageBox.Show("OfficeWD Value Added");
                                    closeAllPanels();
                                    ListOfficeWD();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to Insert OfficeWD");
                                }
                            }
                            else
                            {
                                MessageBox.Show("This Date is Already \n Declared as working!!!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("LeaveType Data Validation failed");
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing User Data");
            }
        }






        private void btnofficeSelect_Click(object sender, EventArgs e)
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            Button lvsel = new Button();
            lvsel.BackColor = Color.Tan;
            lvsel.Text = "Select All";
            lvsel.Location = new Point(2, 2);
            lvsel.Click += new System.EventHandler(this.lvSel_Click4);
            frmPopup.Controls.Add(lvsel);


            frmPopup.Size = new Size(300, 300);
            lv = LeaveSettingsdb.getOfficeListView();
            lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked2);
            lv.Bounds = new Rectangle(new Point(0, 25), new Size(300, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(4, 276);
            lvOK.Click += new System.EventHandler(this.lvOK_Click4);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(94, 276);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click4);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
        }

        private void lvOK_Click4(object sender, EventArgs e)
        {
            try
            {
                txtOfficeID.Text = "";
                rtbOffice.Text = "";
                int count = 1;
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        if (count == 1)
                        {
                            txtOfficeID.Text = itemRow.SubItems[1].Text + Main.delimiter1;
                            rtbOffice.Text = count + "." + itemRow.SubItems[2].Text + "\n";
                            count++;
                        }
                        else
                        {
                            txtOfficeID.Text = txtOfficeID.Text + itemRow.SubItems[1].Text + Main.delimiter1;
                            rtbOffice.Text = rtbOffice.Text + count + "." + itemRow.SubItems[2].Text + "\n";
                            count++;
                        }

                        itemRow.Checked = false;
                        frmPopup.Close();
                        frmPopup.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void lvSel_Click4(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem itemRow in lv.Items)
                {
                    itemRow.Checked = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void lvCancel_Click4(object sender, EventArgs e)
        {
            try
            {
                // btnInvoiceSelect.Enabled = true;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void LvColumnClick(object o, ColumnClickEventArgs e)
        {
            try
            {
                if (lv.Visible == true)
                {
                    string first = lv.Items[0].SubItems[e.Column].Text;
                    string last = lv.Items[lv.Items.Count - 1].SubItems[e.Column].Text;
                    System.Windows.Forms.SortOrder sort_order1 = SortingListView.getSortedOrder(first, last);
                    this.lv.ListViewItemSorter = new SortingListView(e.Column, sort_order1);
                }
                else
                {
                    string first = lvCopy.Items[0].SubItems[e.Column].Text;
                    string last = lvCopy.Items[lvCopy.Items.Count - 1].SubItems[e.Column].Text;
                    System.Windows.Forms.SortOrder sort_order1 = SortingListView.getSortedOrder(first, last);
                    this.lvCopy.ListViewItemSorter = new SortingListView(e.Column, sort_order1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorting error");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            closeAllPanels();
            clearData();
            grdOfficeHW.Visible = true;
            pnlOfficeHW.Visible = true;
            btnNew.Visible = true;
            enableBottomButtons();
        }

        private void grdOfficeHW_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string col = grdOfficeHW.Columns[e.ColumnIndex].HeaderText;
                if (col == "Edit")
                {
                    if (option == 4)
                    {
                        rowid = Convert.ToInt32(grdOfficeHW.Rows[e.RowIndex].Cells["gRowID"].Value.ToString());
                        button2.Text = "Update";
                        pnlHWOuter.Visible = true;
                        pnlHWInner.Visible = true;
                        pnlOfficeHW.Visible = false;
                        cmbType.Enabled = false;
                        cmbType.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbType, grdOfficeHW.Rows[e.RowIndex].Cells["Type"].Value.ToString());
                        dtHWDate.Value = Convert.ToDateTime(grdOfficeHW.Rows[e.RowIndex].Cells["Date"].Value);
                        txtOfficeID.Text = grdOfficeHW.Rows[e.RowIndex].Cells["OfficeIDs"].Value.ToString();
                        rtbDescription.Text = grdOfficeHW.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                        rtbOffice.Text = grdOfficeHW.Rows[e.RowIndex].Cells["gOfficeName"].Value.ToString().Replace(',', '\n');
                        //string[] officeidlist = txtOfficeID.Text.Split(Main.delimiter1);
                        //List<office> offid = new List<office>();
                        //int count = 1;
                        //foreach (string id in officeidlist)
                        //{

                        //  office ofid   = alloffice.Where(x => x.OfficeID == id).FirstOrDefault();
                        //    if(count==1)
                        //    {
                        //        rtbOffice.Text = count + "." + ofid.name + "\n";
                        //        count++;
                        //    }
                        //    else
                        //    {
                        //        rtbOffice.Text = rtbOffice.Text + count + "." + ofid.name + "\n";
                        //        count++;
                        //    }

                        //}

                        disableBottomButtons();
                    }
                    else if (option == 6)
                    {
                        rowid = Convert.ToInt32(grdOfficeHW.Rows[e.RowIndex].Cells["gRowID"].Value.ToString());
                        button2.Text = "Update";
                        pnlHWOuter.Visible = true;
                        pnlHWInner.Visible = true;
                        pnlOfficeHW.Visible = false;
                        dtHWDate.Enabled = false;
                        dtHWDate.Value = Convert.ToDateTime(grdOfficeHW.Rows[e.RowIndex].Cells["Date"].Value);
                        txtOfficeID.Text = grdOfficeHW.Rows[e.RowIndex].Cells["OfficeIDs"].Value.ToString();
                        rtbDescription.Text = grdOfficeHW.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                        rtbOffice.Text = grdOfficeHW.Rows[e.RowIndex].Cells["gOfficeName"].Value.ToString().Replace(',', '\n');
                        //string[] officeidlist = txtOfficeID.Text.Split(Main.delimiter1);
                        //List<office> offid = new List<office>();
                        //int count = 1;
                        //foreach (string id in officeidlist)
                        //{

                        //  office ofid   = alloffice.Where(x => x.OfficeID == id).FirstOrDefault();
                        //    if(count==1)
                        //    {
                        //        rtbOffice.Text = count + "." + ofid.name + "\n";
                        //        count++;
                        //    }
                        //    else
                        //    {
                        //        rtbOffice.Text = rtbOffice.Text + count + "." + ofid.name + "\n";
                        //        count++;
                        //    }

                        //}

                        disableBottomButtons();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnOfficeWoSelect_Click(object sender, EventArgs e)
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(300, 300);
            lv = LeaveSettingsdb.getOfficeListView();
            lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked2);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(300, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_ClickWo);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click4);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();



            //

        }

        private void lvOK_ClickWo(object sender, EventArgs e)
        {
            try
            {
                if (!checkLVItemChecked("Item"))
                {
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        txtOfficeWO.Text = itemRow.SubItems[1].Text + "-" + itemRow.SubItems[2].Text;
                        itemRow.Checked = false;
                        frmPopup.Close();
                        frmPopup.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private Boolean checkLVItemChecked(string str)
        {
            Boolean status = true;
            try
            {
                if (lv.CheckedIndices.Count > 1)
                {
                    MessageBox.Show("Only one " + str + " allowed");
                    return false;
                }
                if (lv.CheckedItems.Count == 0)
                {
                    MessageBox.Show("select one " + str);
                    return false;
                }
            }
            catch (Exception)
            {
            }
            return status;
        }

        private void btnOfficeWO_Click(object sender, EventArgs e)
        {
            try
            {
                //CatalogueValueDB.fillCatalogValueComboNew(cmbType, "HolidayType");
                //cmbType.SelectedIndex = 0;
                ListOfficeWO();
            }
            catch (Exception ex)
            {

            }
        }

        private void btnWOCancel_Click(object sender, EventArgs e)
        {
            closeAllPanels();
            clearData();
            dgvWO.Visible = true;
            pnlOfficeHW.Visible = true;
            btnNew.Visible = true;
            enableBottomButtons();
        }

        private void btnWOSave_Click(object sender, EventArgs e)
        {
            try
            {
                HolidayList OHW = new HolidayList();
                LeaveSettingsdb leaveDB = new LeaveSettingsdb();
                if (option == 5)
                {
                    string[] offid = txtOfficeWO.Text.Split('-');
                    string wkoffs = "";
                    foreach (string a in chbWO.CheckedItems)
                    {
                        if (wkoffs.Length == 0)
                        {
                            wkoffs = a + Main.delimiter1;
                        }
                        else
                        {
                            wkoffs = wkoffs + a + Main.delimiter1;
                        }
                    }
                    OHW.Weekoffs = wkoffs;
                    OHW.officeID = offid[0];
                    OHW.Description = rtbDescription.Text.ToString();

                    if (leaveDB.ValidateOfficeWO(OHW))
                    {

                        if (btnWOSave.Text.Equals("Update"))
                        {
                            OHW.RowID = rowid;
                            if (leaveDB.UpdateOfficeWO(OHW))
                            {
                                MessageBox.Show("OfficeWO updated");
                                closeAllPanels();
                                ListOfficeWO();
                            }
                            else
                            {
                                MessageBox.Show("Failed to update OfficeWO");
                            }
                        }
                        else if (btnWOSave.Text.Equals("Save"))
                        {
                            if (leaveDB.validateOfficeWO(OHW))
                            {
                                if (leaveDB.InsertLeaveOfficeWO(OHW))
                                {
                                    MessageBox.Show("OfficeHW Value Added");
                                    closeAllPanels();
                                    ListOfficeWO();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to Insert OfficeHW");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Type Already exists for the day!!!");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("LeaveType Data Validation failed");
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing User Data");
            }
        }

        private void dgvWO_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string col = dgvWO.Columns[e.ColumnIndex].HeaderText;
                if (col == "Edit")
                {
                    rowid = Convert.ToInt32(dgvWO.Rows[e.RowIndex].Cells["wRowID"].Value.ToString());
                    btnWOSave.Text = "Update";
                    pnlWOInner.Visible = true;
                    pnlWOOuter.Visible = true;
                    pnlOfficeHW.Visible = false;
                    btnOfficeWoSelect.Enabled = false;
                    txtOfficeWO.Text = dgvWO.Rows[e.RowIndex].Cells["wOfficeID"].Value.ToString() + "-" +
                                      dgvWO.Rows[e.RowIndex].Cells["wOfficeName"].Value.ToString();
                    string[] weekends = dgvWO.Rows[e.RowIndex].Cells["WeekOff"].Value.ToString().Split(',');
                    foreach (string woffs in weekends)
                    {
                        chbWO.SetItemChecked(chbWO.FindString(woffs), true);
                    }
                    //int count = 1;
                    //foreach (string id in officeidlist)
                    //{

                    //  office ofid   = alloffice.Where(x => x.OfficeID == id).FirstOrDefault();
                    //    if(count==1)
                    //    {
                    //        rtbOffice.Text = count + "." + ofid.name + "\n";
                    //        count++;
                    //    }
                    //    else
                    //    {
                    //        rtbOffice.Text = rtbOffice.Text + count + "." + ofid.name + "\n";
                    //        count++;
                    //    }

                    //}

                    disableBottomButtons();
                }
            }
            catch (Exception)
            {

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                //grdList.Visible = false;
                CatalogueValueDB.fillCatalogValueComboNew(cmbType, "HolidayType");
                cmbType.SelectedIndex = 0;
                ListOfficeWD();
            }
            catch(Exception ex)
            {

            }
            
        }
    }
}


