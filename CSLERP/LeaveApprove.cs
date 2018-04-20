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
using System.Collections;
using CSLERP.DBData;
using System.Threading;

namespace CSLERP
{
    public partial class LeaveApprove : System.Windows.Forms.Form
    {
        string docID = "LEAVEAPPROVE";
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        int rowid;
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Hashtable ht = new Hashtable();
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        leaveapprove prevleave;
        double sanctiondays = 0;
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        ListView lvApprover = new ListView();

        Form frmPopup = new Form();
        RichTextBox txt;
        string id;
        public LeaveApprove()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void AccountCode_Load(object sender, EventArgs e)
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

            ListAccountDetail(listOption);


            //applyPrivilege();
        }
        private void ListAccountDetail(int option)
        {
            try
            {


                // pnlActionButtons.Visible = true;
                lblActionHeader.Visible = true;
                grdList.Rows.Clear();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                if (option == 1)
                    lblActionHeader.Text = "Inbox";
                else if (option == 2)
                    lblActionHeader.Text = "Outbox";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "Finalised";
                LeaveApproveDB ACDB = new LeaveApproveDB();
                List<leaveapprove> ACItems = ACDB.getFilteredLeaveApproval(userString, option);
                leaveapprove la = ACItems.FirstOrDefault(rpoio => (rpoio.leavestatus == 99));
                foreach (leaveapprove leave in ACItems)
                {
                    if (option == 1)
                    {
                        if (leave.documentStatus == 99 && leave.leavestatus == 1)
                            continue;
                    }
                    else if (option == 2)
                    {
                        if (leave.status == 98 && leave.leavestatus == 5)
                        {
                            continue;
                        }
                        if (leave.documentStatus == 99 && leave.leavestatus == 1)
                        {
                            continue;
                        }
                    }
                    else if (option == 3)
                    {
                        if (leave.status == 1 && leave.leavestatus == 5)
                        {
                            continue;
                        }
                        if (leave.status == 1 && leave.leavestatus == 1 && leave.documentStatus != 99)
                        {
                            continue;
                        }
                        if (leave.status == 1 && leave.leavestatus == 2)
                        {
                            continue;
                        }
                    }
                    else
                    {

                    }
                    double days = (leave.todate - leave.fromdate).TotalDays;
                    days += 1;
                    double sanctioneddays = (leave.sanctionedTo - leave.sanctionedFrom).TotalDays;
                    sanctioneddays += 1;
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gRowID"].Value = leave.rowid;
                    grdList.Rows[grdList.RowCount - 1].Cells["EmployeeID"].Value = leave.EmployeeID;
                    grdList.Rows[grdList.RowCount - 1].Cells["EmployeeName"].Value = leave.EmployeeName;
                    grdList.Rows[grdList.RowCount - 1].Cells["LeaveID"].Value = leave.leaveid;
                    grdList.Rows[grdList.RowCount - 1].Cells["LeaveType"].Value = leave.Leavetype;
                    grdList.Rows[grdList.RowCount - 1].Cells["FromDate"].Value = leave.fromdate;
                    grdList.Rows[grdList.RowCount - 1].Cells["ToDate"].Value = leave.todate;
                    grdList.Rows[grdList.RowCount - 1].Cells["NoofDays"].Value = days;
                    grdList.Rows[grdList.RowCount - 1].Cells["SanctionedDays"].Value = sanctioneddays;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = leave.status;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatus"].Value = leave.documentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["LeaveStatus"].Value = leave.leavestatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Remarks"].Value = leave.remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["SanctionedToDate"].Value = leave.sanctionedTo;
                    grdList.Rows[grdList.RowCount - 1].Cells["SanctionedFromDate"].Value = leave.sanctionedFrom;
                    grdList.Rows[grdList.RowCount - 1].Cells["SanctionType"].Value = leave.sanctiontype;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = leave.ForwardUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = leave.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwarderLists"].Value = leave.ForwarderList;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = leave.CreateTime;
                    if (leave.status == 1 && leave.documentStatus >= 2 && leave.documentStatus <= 98 && leave.leavestatus == 1)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["SanctionedToDate"].Value = "";
                        grdList.Rows[grdList.RowCount - 1].Cells["SanctionedFromDate"].Value = "";
                        grdList.Rows[grdList.RowCount - 1].Cells["SanctionedDays"].Value = "";
                        grdList.Rows[grdList.RowCount - 1].Cells["Report"].Value = "Approval Pending";
                    }
                    if (leave.status == 1 && leave.documentStatus >= 2 && leave.documentStatus <= 98 && leave.leavestatus == 2)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["SanctionedToDate"].Value = "";
                        grdList.Rows[grdList.RowCount - 1].Cells["SanctionedFromDate"].Value = "";
                        grdList.Rows[grdList.RowCount - 1].Cells["SanctionedDays"].Value = "";
                        grdList.Rows[grdList.RowCount - 1].Cells["Report"].Value = "Request For Reversal";
                    }
                    if (leave.status == 1 && leave.documentStatus == 99 && leave.leavestatus == 4)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["Report"].Value = "Request For Modification";
                    }
                    if (leave.status == 1 && leave.documentStatus == 99 && leave.leavestatus == 5)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["Report"].Value = "Request For Cancel";
                    }
                    if (leave.status == 1 && leave.documentStatus == 99 && leave.leavestatus == 8)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["Report"].Value = "Modified request Pending";
                    }
                    if (leave.status == 1 && leave.documentStatus >= 2 && leave.documentStatus <= 98 && leave.leavestatus == 99)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["SanctionedToDate"].Value = "";
                        grdList.Rows[grdList.RowCount - 1].Cells["SanctionedFromDate"].Value = "";
                        grdList.Rows[grdList.RowCount - 1].Cells["SanctionedDays"].Value = "";
                        grdList.Rows[grdList.RowCount - 1].Cells["Report"].Value = "Rejected";
                    }
                    if (leave.status == 1 && leave.documentStatus == 99 && leave.leavestatus == 1)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["Report"].Value = "Approved";
                    }
                    if (leave.status == 1 && leave.documentStatus == 1 && leave.leavestatus == 3)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["Report"].Value = "Reversal Accepted";
                    }
                    if (leave.status == 98 && leave.documentStatus == 99 && leave.leavestatus == 5)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["Report"].Value = "Cancel Request Approved";
                    }
                    if (leave.status == 1 && leave.documentStatus == 99 && leave.leavestatus == 6)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["Report"].Value = "Modification Request Approved";
                    }
                    if (leave.status == 1 && leave.documentStatus == 99 && leave.leavestatus == 7)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["Report"].Value = "Modification Request Cancelled";
                    }
                    if (leave.status == 1 && leave.documentStatus == 99 && leave.leavestatus == 9)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["Report"].Value = "Modified Approved";
                    }
                    if (leave.status == 1 && leave.documentStatus == 99 && leave.leavestatus == 10)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["Report"].Value = "Modified Rejected";
                    }
                    if (leave.status == 1 && leave.documentStatus == 99 && leave.leavestatus == 11)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["Report"].Value = "Cancel Request Rejected";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Leave Approve listing");
            }
            setGridProperties(option);
            setButtonVisibility("init");
            pnlList.Visible = true;
            pnlBottomButtons.Visible = true;
        }

        private void initVariables()
        {
            try
            {
                if (getuserPrivilegeStatus() == 1)
                {
                    //user is only a viewer
                    listOption = 3;
                }
                userString = Main.delimiter1 + Login.userLoggedIn;
                setButtonVisibility("init");
                dtpreqFromDate.Format = DateTimePickerFormat.Custom;
                dtpreqFromDate.CustomFormat = "dd-MM-yyyy";
                dtpreqFromDate.Enabled = false;
                dtpreqToDate.Format = DateTimePickerFormat.Custom;
                dtpreqToDate.CustomFormat = "dd-MM-yyyy";
                dtpreqToDate.Enabled = false;
                dtpApprFromDate.Format = DateTimePickerFormat.Custom;
                dtpApprFromDate.CustomFormat = "dd-MM-yyyy";
                dtpApprFromDate.Enabled = true;
                dtpApprToDate.Format = DateTimePickerFormat.Custom;
                dtpApprToDate.CustomFormat = "dd-MM-yyyy";
                dtpApprToDate.Enabled = true;
                //dgvLeaveDetails.Enabled = false;
                //dgvLeaveDetails.ReadOnly = true;

            }
            catch (Exception)
            {

            }
        }


        private void closeAllPanels()
        {
            try
            {
                pnlLeaveDetailInner.Visible = false;
                pnlLeaveDetailOuter.Visible = false;
                pnlList.Visible = false;
                //pnlPDFViewer.Visible = false;
            }
            catch (Exception)
            {

            }
        }
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            try
            {
                lblActionHeader.Visible = true;
                closeAllPanels();
                clearData();
                pnlList.Visible = true;
                grdList.Visible = true;

                setButtonVisibility("btnEditPanel");
            }
            catch (Exception)
            {

            }
        }

        public void clearData()
        {
            try
            {
                ////dtDate.Value = DateTime.Now;
                pnlForwarder.Visible = false;
                prevleave = new leaveapprove();
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
                lblActionHeader.Visible = false;
                clearData();
                //btnReject.Text = "Save";
                pnlLeaveDetailOuter.Visible = true;
                pnlLeaveDetailInner.Visible = true;
                setButtonVisibility("btnNew");
            }
            catch (Exception)
            {

            }
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                leaveapprove lv = new leaveapprove();
                lv.remarks = rtbRemarks.Text.Replace("'", "''") + Login.userLoggedInName +
                            " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Leave Rejected" + "\n";
                lv.EmployeeID = txtEmployeeID.Text;
                lv.rowid = rowid;
                DialogResult dialog = MessageBox.Show("Are you sure to Reject the Leave ?", "Confirm", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    LeaveApproveDB accDB = new LeaveApproveDB();
                    if (accDB.RejectLeave(lv))
                    {
                        MessageBox.Show("Leave Rejected");
                        closeAllPanels();
                        listOption = 1;
                        ListAccountDetail(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        //private void btnSave_Click(object sender, EventArgs e)
        //{


        //}

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // pnlActionButtons.Visible = false;

                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit") || columnName.Equals("View"))
                {
                    lblActionHeader.Visible = false;
                    clearData();
                    prevleave = new leaveapprove();
                    int rowID = e.RowIndex;
                    try
                    {
                        prevleave.EmployeeID = grdList.Rows[e.RowIndex].Cells["EmployeeID"].Value.ToString();
                        prevleave.EmployeeName = grdList.Rows[e.RowIndex].Cells["EmployeeName"].Value.ToString();
                        prevleave.ForwarderList = grdList.Rows[e.RowIndex].Cells["ForwarderLists"].Value.ToString();
                        prevleave.sanctiontype = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["SanctionType"].Value);
                    }
                    catch (Exception ez)
                    {

                    }
                    string empid = grdList.Rows[e.RowIndex].Cells["EmployeeID"].Value.ToString();
                    string lvid = grdList.Rows[e.RowIndex].Cells["LeaveID"].Value.ToString();
                    setleavedetailgrid(empid, lvid);
                    rowid = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gRowID"].Value);
                    txtEmployeeID.Text = grdList.Rows[e.RowIndex].Cells["EmployeeID"].Value.ToString();
                    txtEmployeeName.Text = grdList.Rows[e.RowIndex].Cells["EmployeeName"].Value.ToString();
                    txtLeaveType.Text = grdList.Rows[e.RowIndex].Cells["LeaveID"].Value.ToString();
                    //DateTime dt = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["FromDate"].Value);
                    dtpreqFromDate.Value = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["FromDate"].Value);
                    dtpreqToDate.Value = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["ToDate"].Value);
                    if (grdList.Rows[e.RowIndex].Cells["Report"].Value.ToString() == "Approval Pending" ||
                        grdList.Rows[e.RowIndex].Cells["Report"].Value.ToString() == "Request For Reversal" ||
                        grdList.Rows[e.RowIndex].Cells["Report"].Value.ToString() == "Rejected")
                    {
                        dtpApprFromDate.Value = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["FromDate"].Value);
                        dtpApprToDate.Value = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["ToDate"].Value);
                        txtAppTotal.Text = grdList.Rows[e.RowIndex].Cells["NoofDays"].Value.ToString();
                    }
                    else
                    {
                        dtpApprFromDate.Value = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["SanctionedFromDate"].Value);
                        dtpApprToDate.Value = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["SanctionedToDate"].Value);
                        txtAppTotal.Text = grdList.Rows[e.RowIndex].Cells["SanctionedDays"].Value.ToString();
                    }
                    txtreqTotal.Text = grdList.Rows[e.RowIndex].Cells["NoofDays"].Value.ToString();
                    rtbRemarks.Text = grdList.Rows[e.RowIndex].Cells["Remarks"].Value.ToString();
                    if (listOption == 3)
                    {
                        if (grdList.Rows[e.RowIndex].Cells["Report"].Value.ToString() != "Rejected")
                        {
                            dtpApprFromDate.Value = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["SanctionedFromDate"].Value);
                            dtpApprToDate.Value = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["SanctionedToDate"].Value);
                        }
                    }
                    if (grdList.Rows[e.RowIndex].Cells["Report"].Value.ToString() == "Modified Pending")
                    {
                        dtpApprToDate.Value = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["SanctionedToDate"].Value);
                    }
                    pnlLeaveDetailInner.Visible = true;
                    pnlLeaveDetailOuter.Visible = true;
                    pnlList.Visible = false;
                    if (listOption == 1)
                    {
                        if (grdList.Rows[e.RowIndex].Cells["Report"].Value.ToString() == "Approval Pending")
                        {
                            setButtonVisibility("Pending");
                        }
                        else if (grdList.Rows[e.RowIndex].Cells["Report"].Value.ToString() == "Request For Reversal")
                        {
                            setButtonVisibility("Request For Reversal");
                        }
                        else if (grdList.Rows[e.RowIndex].Cells["Report"].Value.ToString() == "Request For Modification")
                        {
                            setButtonVisibility("Request For Modification");
                        }
                        else if (grdList.Rows[e.RowIndex].Cells["Report"].Value.ToString() == "Request For Cancel")
                        {
                            setButtonVisibility("Request For Cancel");
                        }
                        else if (grdList.Rows[e.RowIndex].Cells["Report"].Value.ToString() == "Modified request Pending")
                        {
                            setButtonVisibility("Modified Pending");
                        }
                    }
                    else
                    {
                        setButtonVisibility(columnName);
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void setleavedetailgrid(string empid, string lvid)
        {
            double daysPending = 0;
            dgvLeaveDetails.Columns.Clear();
            LeaveApproveDB ladb = new LeaveApproveDB();
            List<leaveapprove> lvapp = ladb.getLeaveLimit(empid);
            dgvLeaveDetails.Columns.Add("LeaveType", "LeaveType");
            foreach (leaveapprove lv in lvapp)
            {
                int row = dgvLeaveDetails.Rows.Count - 1;
                dgvLeaveDetails.Columns.Add(lv.leaveid, lv.leaveid);
                dgvLeaveDetails.Columns[lv.leaveid].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvLeaveDetails.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            dgvLeaveDetails.Rows.Add();
            dgvLeaveDetails.Rows[dgvLeaveDetails.Rows.Count - 1].Cells["LeaveType"].Value = "Total";
            double totaldays = 0;
            foreach (leaveapprove lv in lvapp)
            {
                dgvLeaveDetails.Rows[dgvLeaveDetails.Rows.Count - 1].Cells[lv.leaveid].Value = lv.maxdays;
                totaldays += lv.maxdays;
            }
            dgvLeaveDetails.Rows.Add();
            dgvLeaveDetails.Rows[dgvLeaveDetails.Rows.Count - 1].Cells["LeaveType"].Value = "Leave Availed";
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
            dgvLeaveDetails.Rows[dgvLeaveDetails.Rows.Count - 1].Cells["LeaveType"].Value = "Balance leave";
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


        private void btnForward_Click_1(object sender, EventArgs e)
        {
            leaveapprove frdusr = new leaveapprove();
            LeaveApproveDB leaveDB = new LeaveApproveDB();
            if (prevleave.sanctiontype == 1)
            {
                frdusr = leaveDB.forwarder(Login.empLoggedIn);
            }
            else
            {
                string empid = EmployeeDB.getEmployeesOfRole("LeaveAuthority");
                //collect user id and employee name
                frdusr = leaveDB.forwarderAuthority(empid);
            }
            string snr = frdusr.username;
            DialogResult dialog = MessageBox.Show("Are you sure to forward the Leave to " + snr + " ?", "Confirm", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                //do forward activities
                if (frdusr.ForwardUser != "" && frdusr.ForwardUser != null)
                {
                    prevleave.ForwardUser = frdusr.ForwardUser;
                    prevleave.rowid = rowid;
                    prevleave.remarks = rtbRemarks.Text.Replace("'", "''") + Login.userLoggedInName +
                     " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Forwarded to " + snr + "" + "\n";
                    prevleave.ForwarderList = prevleave.ForwarderList + Main.delimiter1 + prevleave.ForwardUser;
                    if (leaveDB.forwardleave(prevleave))
                    {
                        MessageBox.Show("Leave Approval Forwarded to " + snr + "");
                        frmPopup.Close();
                        frmPopup.Dispose();
                        closeAllPanels();
                        listOption = 1;
                        ListAccountDetail(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
                else
                {
                    MessageBox.Show("No Heigher authority Found!!!");
                }
            }
        }

        private void setGridProperties(int opt)
        {
            if (opt == 1)
            {
                grdList.Columns["EmployeeName"].Width = 300;
                grdList.Columns["FromDate"].Width = 100;
                grdList.Columns["ToDate"].Width = 100;
                grdList.Columns["FromDate"].Visible = true;
                grdList.Columns["ToDate"].Visible = true;
                grdList.Columns["SanctionedFromDate"].Visible = false;
                grdList.Columns["SanctionedToDate"].Visible = false;
                ////grdList.Columns["Forwarder"].Visible = true;
                grdList.Columns["Approver"].Visible = false;
            }
            else if (opt == 2)
            {
                grdList.Columns["EmployeeName"].Width = 300;
                grdList.Columns["FromDate"].Width = 150;
                grdList.Columns["ToDate"].Width = 150;
                grdList.Columns["FromDate"].Visible = true;
                grdList.Columns["ToDate"].Visible = true;
                grdList.Columns["SanctionedFromDate"].Visible = false;
                grdList.Columns["SanctionedToDate"].Visible = false;
                grdList.Columns["Forwarder"].Visible = false;
                grdList.Columns["Approver"].Visible = false;
            }
            else if (opt == 3)
            {
                grdList.Columns["EmployeeName"].Width = 170;
                grdList.Columns["FromDate"].Width = 100;
                grdList.Columns["ToDate"].Width = 100;
                grdList.Columns["FromDate"].Visible = true;
                grdList.Columns["ToDate"].Visible = true;
                grdList.Columns["SanctionedFromDate"].Visible = true;
                grdList.Columns["SanctionedToDate"].Visible = true;
                grdList.Columns["Forwarder"].Visible = false;
                grdList.Columns["Approver"].Visible = true;
            }
        }
        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListAccountDetail(listOption);
            
        }
        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListAccountDetail(listOption);
            

        }

        private void btnApproved_Click(object sender, EventArgs e)
        {
            listOption = 3;
            ListAccountDetail(listOption);
           

        }

        private void btnApprove_Click_1(object sender, EventArgs e)
        {
            try
            {
                LeaveApproveDB accDB = new LeaveApproveDB();
                leaveapprove lv = new leaveapprove();
                lv.EmployeeID = txtEmployeeID.Text;
                lv.sanctionedFrom = dtpApprFromDate.Value;
                lv.sanctionedTo = dtpApprToDate.Value;
                lv.leaveid = txtLeaveType.Text;
                lv.remarks = rtbRemarks.Text.Replace("'", "''") + Login.userLoggedInName +
                             " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Leave Approved" + "\n";
                //lv.remarks.Replace("'","''");
                lv.rowid = rowid;
                if (dtpApprToDate.Value.Date < dtpApprFromDate.Value.Date || dtpApprToDate.Value.Date > dtpreqToDate.Value.Date)
                {
                    MessageBox.Show("Check Dates!!!");
                    return;
                }
                double totaldays = Convert.ToDouble(txtAppTotal.Text);

                DialogResult dialog = MessageBox.Show("Are you sure to Approve the Leave ?", "Confirm", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (accDB.ApproveCheck(lv, totaldays))
                    {
                        if (Convert.ToDouble(dgvLeaveDetails.Rows[2].Cells[txtLeaveType.Text].Value) >= totaldays)
                        {
                            if (txtLeaveType.Text == "CO")
                            {
                                int tp = Convert.ToInt32(txtAppTotal.Text);
                                if (accDB.ApproveLeaveRequestCOmpoff(lv, tp))
                                {
                                    MessageBox.Show("Leave Approved");
                                    closeAllPanels();
                                    listOption = 1;
                                    ListAccountDetail(listOption);
                                    setButtonVisibility("btnEditPanel");
                                }
                            }
                            else
                            {
                                if (accDB.ApproveLeaveRequest(lv))
                                {
                                    MessageBox.Show("Leave Approved");
                                    closeAllPanels();
                                    listOption = 1;
                                    ListAccountDetail(listOption);
                                    setButtonVisibility("btnEditPanel");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Leave in credit is not sufficient!!!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Not authorised !!! Please Forward");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnReverse_Click_1(object sender, EventArgs e)
        {
            try
            {
                leaveapprove lv = new leaveapprove();
                lv.remarks = rtbRemarks.Text.Replace("'", "''") + Login.userLoggedInName +
                            " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Leave Reversed" + "\n";
                lv.EmployeeID = txtEmployeeID.Text;
                lv.rowid = rowid;
                DialogResult dialog = MessageBox.Show("Are you sure to Reverse Leave ?", "Confirm", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    LeaveApproveDB accDB = new LeaveApproveDB();
                    if (accDB.reverseLeave(lv))
                    {
                        MessageBox.Show("Leave Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListAccountDetail(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        //private void btnReverse_Click(object sender, EventArgs e)
        //{

        //}
        //return the previous forward list and forwarder 
        private string getReverseString(string forwarderList)
        {
            string reverseString = "";
            try
            {
                string prevUser = "";
                string[] lst1 = forwarderList.Split(Main.delimiter2);
                for (int i = 0; i < lst1.Length - 1; i++)
                {
                    if (lst1[i].Trim().Length > 1)
                    {
                        string[] lst2 = lst1[i].Split(Main.delimiter1);

                        if (i == (lst1.Length - 2))
                        {
                            if (reverseString.Trim().Length > 0)
                            {
                                reverseString = reverseString + "!@#" + prevUser;
                            }
                        }
                        else
                        {
                            reverseString = reverseString + lst2[0] + Main.delimiter1 + lst2[1] + Main.delimiter1 + Main.delimiter2;
                            prevUser = lst2[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return reverseString;
        }
        private void setButtonVisibility(string btnName)
        {
            try
            {
                pnlTopButtons.Visible = true;
                lblActionHeader.Visible = true;
                pnlLeaveDetailInner.Visible = true;
                btnActionPending.Visible = true;
                btnApprovalPending.Visible = true;
                btnApproved.Visible = true;
                //btnNew.Visible = false;
                btnExit.Visible = false;
                btnBack.Visible = false;
                btnReject.Visible = false;
                btnForward.Visible = false;
                btnApprove.Visible = false;
                btnReverse.Visible = false;
                btnModifyAccept.Visible = false;
                btnModifyCancel.Visible = false;
                btnModifyAccept.Visible = false;
                btnModifiedApprove.Visible = false;
                btnModifiedReject.Visible = false;
                handleNewButton();
                handleGrdViewButton();
                handleGrdEditButton();
                pnlBottomButtons.Visible = true;
                //----

                if (btnName == "init")
                {
                    btnExit.Visible = true;
                    if (listOption == 1)
                    {
                        ////grdList.Columns["SanctionedFromDate"].Visible = true;
                        ////grdList.Columns["SanctionedToDate"].Visible = true;
                        ////grdList.Columns["Forwarder"].Visible = false;
                        grdList.Columns["SanctionedDays"].Visible = true;
                        grdList.Columns["NoofDays"].Visible = true;
                        grdList.Columns["Edit"].Visible = true;
                       
                    }
                    else if (listOption == 2)
                    {
                        grdList.Columns["View"].Visible = true;
                        grdList.Columns["NoofDays"].Visible = true;
                        ////grdList.Columns["Forwarder"].Visible = false;

                    }
                    else if (listOption == 3)
                    {
                       
                        grdList.Columns["SanctionedDays"].Visible = true;
                        grdList.Columns["NoofDays"].Visible = false;
                        //////grdList.Columns["Forwarder"].Visible = true;
                        //////grdList.Columns["FromDate"].Visible = false;
                        //////grdList.Columns["ToDate"].Visible = false;
                    }
                }

                //gridview buttons
                else if (btnName == "btnEditPanel")
                {
                    btnExit.Visible = true;
                    //24/11/2016
                    //btnCancel.Visible = true; 
                    //btnForward.Visible = true;
                    //btnApprove.Visible = true;
                    //btnReverse.Visible = true;
                    //btnReject.Visible = true;
                }
                else if (btnName == "Pending")
                {
                    if (listOption == 1)
                    {
                        btnApprove.Visible = true;
                        btnReject.Visible = true;
                        btnForward.Visible = true;
                        btnBack.Visible = true;
                        btnReverse.Visible = false;
                        btnCancel.Visible = false;
                        lblActionHeader.Visible = false;
                        pnlTopButtons.Visible = false;
                        pnlBottomButtons.Visible = false;
                        btnModifiedApprove.Visible = false;
                        btnModifiedReject.Visible = false;
                        btnCancel.Visible = false;
                        btnCancelRequest.Visible = false;
                        btnModifiedApprove.Visible = false;
                        btnModifiedReject.Visible = false;
                        dtpApprFromDate.Enabled = false;
                        //cmbAppFromSessn.Enabled = false;

                        dtpApprFromDate.Enabled = false;
                        btnAddRemarks.Enabled = true;
                        dtpApprToDate.Enabled = true;
                        pnlApproved.Enabled = true;
                        //cmbAppToSessn.Enabled = true;
                    }

                }
                else if (btnName == "Request For Reversal")
                {
                    if (listOption == 1)
                    {
                        btnReverse.Visible = true;
                        btnBack.Visible = true;
                        lblActionHeader.Visible = false;
                        pnlTopButtons.Visible = false;
                        pnlBottomButtons.Visible = false;
                        btnModifiedApprove.Visible = false;
                        btnModifiedReject.Visible = false;
                        btnApprove.Visible = false;
                        btnReject.Visible = false;
                        btnForward.Visible = false;
                        btnCancel.Visible = false;
                        pnlApproved.Enabled = false;
                        btnCancelRequest.Visible = false;
                        btnModifiedApprove.Visible = false;
                        btnModifiedReject.Visible = false;
                        btnAddRemarks.Enabled = true;
                    }
                }
                else if (btnName == "Request For Modification")
                {
                    if (listOption == 1)
                    {
                        btnBack.Visible = true;
                        btnModifyAccept.Visible = true;
                        btnModifyCancel.Visible = true;
                        btnModifiedApprove.Visible = false;
                        btnModifiedReject.Visible = false;
                        lblActionHeader.Visible = false;
                        pnlTopButtons.Visible = false;
                        pnlBottomButtons.Visible = false;
                        btnApprove.Visible = false;
                        btnReject.Visible = false;
                        btnForward.Visible = false;
                        btnReverse.Visible = false;
                        btnCancel.Visible = false;
                        dtpApprFromDate.Enabled = false;
                        //cmbAppFromSessn.Enabled = false;
                        dtpApprToDate.Enabled = false;
                        //cmbAppToSessn.Enabled = false;
                        btnCancelRequest.Visible = false;
                        pnlApproved.Enabled = false;
                        btnAddRemarks.Enabled = true;
                    }
                }

                else if (btnName == "Modified Pending")
                {
                    if (listOption == 1)
                    {
                        btnBack.Visible = true;
                        btnModifyAccept.Visible = false;
                        btnModifyCancel.Visible = false;
                        btnModifiedApprove.Visible = true;
                        btnModifiedReject.Visible = true;
                        lblActionHeader.Visible = false;
                        pnlTopButtons.Visible = false;
                        pnlBottomButtons.Visible = false;
                        btnApprove.Visible = false;
                        btnReject.Visible = false;
                        btnForward.Visible = false;
                        btnReverse.Visible = false;
                        btnCancel.Visible = false;
                        pnlApproved.Enabled = true;
                        dtpApprFromDate.Enabled = false;
                        //cmbAppFromSessn.Enabled = false;
                        dtpApprToDate.Enabled = true;
                        btnAddRemarks.Enabled = true;
                        //cmbAppToSessn.Enabled = true;
                        btnCancelRequest.Visible = false;
                        btnAddRemarks.Enabled = true;

                    }
                }
                else if (btnName == "Request For Cancel")
                {
                    if (listOption == 1)
                    {
                        btnBack.Visible = true;
                        lblActionHeader.Visible = false;
                        pnlTopButtons.Visible = false;
                        pnlBottomButtons.Visible = false;
                        btnModifiedApprove.Visible = false;
                        btnModifiedReject.Visible = false;
                        btnCancel.Visible = true;
                        btnCancelRequest.Visible = true;
                        btnApprove.Visible = false;
                        btnReject.Visible = false;
                        btnForward.Visible = false;
                        btnReverse.Visible = false;
                        pnlApproved.Enabled = false;
                        btnModifiedApprove.Visible = false;
                        btnModifiedReject.Visible = false;
                        btnAddRemarks.Enabled = true;
                    }
                }

                else if (btnName == "View")
                {
                    lblActionHeader.Visible = false;
                    pnlTopButtons.Visible = false;
                    pnlBottomButtons.Visible = false; //24/11/2016
                    pnlLeaveDetailInner.Enabled = true;
                    dtpApprFromDate.Enabled = false;
                    dtpApprToDate.Enabled = false;
                    //cmbAppToSessn.Enabled = false;
                    //cmbAppFromSessn.Enabled = false;
                    //pnlApproved.Enabled = false;
                    btnBack.Visible = true;
                    btnModifiedApprove.Visible = false;
                    btnModifiedReject.Visible = false;
                    btnCancel.Visible = false;
                    btnCancelRequest.Visible = false;
                    btnApprove.Visible = false;
                    btnReject.Visible = false;
                    btnForward.Visible = false;
                    btnReverse.Visible = false;
                    //pnlApproved.Enabled = false;
                    btnModifiedApprove.Visible = false;
                    btnModifiedReject.Visible = false;
                    rtbRemarks.Enabled = true;
                    rtbRemarks.ReadOnly = true;
                    btnAddRemarks.Enabled = false;
                }
                else if (btnName == "LoadDocument")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                }
                else if (btnName == "ViewDocument")
                {
                    lblActionHeader.Visible = false;
                    pnlTopButtons.Visible = false;
                    btnBack.Visible = true;
                    //btnNew.Visible = false;
                    btnActionPending.Visible = false;
                    btnApprovalPending.Visible = false;
                    btnApproved.Visible = false;
                    pnlLeaveDetailInner.Visible = false;
                    pnlLeaveDetailOuter.Visible = false;
                    grdList.Visible = false;
                    //pnlPDFViewer.Visible = true;
                }

                pnlEditButtons.Refresh();
                //if the user privilege is only view, show only the Approved documents button
                int ups = getuserPrivilegeStatus();
                if (ups == 1 || ups == 0)
                {
                    grdList.Columns["Edit"].Visible = false;
                    //grdList.Columns["Approve"].Visible = false;
                    btnActionPending.Visible = false;
                    btnApprovalPending.Visible = false;
                    btnApproved.Visible = false;
                    if (ups == 1)
                    {
                        grdList.Columns["View"].Visible = true;
                    }
                    else
                    {
                        grdList.Columns["View"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        //private void disableControlsInnerPanel()
        //{
        //    try
        //    {
        //        foreach (Control p in pnlCVINRInner.Controls)
        //            if (p.GetType() == typeof(Label) || p.GetType() == typeof(TextBox) || p.GetType() == typeof(ComboBox))
        //            {
        //                p.Enabled = false;
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //private void enableControlsInnerPanel()
        //{
        //    try
        //    {
        //        foreach (Control p in pnlCVINRInner.Controls)
        //            if (p.GetType() == typeof(Label) || p.GetType() == typeof(TextBox) || p.GetType() == typeof(ComboBox))
        //            {
        //                if (!(p.Name == "txtStockItemID"))
        //                    p.Enabled = true;
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        void handleNewButton()
        {
            //btnNew.Visible = false;
            if (Main.itemPriv[1])
            {
                //btnNew.Visible = true;
            }
        }
        void handleGrdEditButton()
        {
            grdList.Columns["Edit"].Visible = false;
            //grdList.Columns["Approve"].Visible = false;
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                if (listOption == 1)
                {
                    grdList.Columns["Edit"].Visible = true;
                    //grdList.Columns["Approve"].Visible = true;
                }
            }
        }

        void handleGrdViewButton()
        {
            grdList.Columns["View"].Visible = false;
            //if any one of view,add and edit
            if (Main.itemPriv[0] || Main.itemPriv[1] || Main.itemPriv[2])
            {
                //list option 1 should not have view buttuon visible (edit is avialable)
                if (listOption != 1)
                {
                    grdList.Columns["View"].Visible = true;
                }
            }
        }
        int getuserPrivilegeStatus()
        {
            try
            {
                if (Main.itemPriv[0] && !Main.itemPriv[1] && !Main.itemPriv[2]) //only view
                    return 1;
                else if (Main.itemPriv[0] && (Main.itemPriv[1] || Main.itemPriv[2])) //view add and edit
                    return 2;
                else if (!Main.itemPriv[0] && (Main.itemPriv[1] || Main.itemPriv[2])) //view add and edit
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
        private void btnListDocument_Click_1(object sender, EventArgs e)
        {
            try
            {
                //removePDFFileGridView();
                //removePDFControls();
                //DataGridView dgvDocumentList = new DataGridView();
                //pnlPDFViewer.Controls.Remove(dgvDocumentList);
                //dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevaccode.AccountCode + "-" + prevaccode.Name);
                //pnlPDFViewer.Controls.Add(dgvDocumentList);
                //dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
            }
            catch (Exception ex)
            {
            }
        }
        private void removePDFFileGridView()
        {
            try
            {
                //    foreach (Control p in pnlPDFViewer.Controls)
                //        if (p.GetType() == typeof(DataGridView))
                //        {
                //            p.Dispose();
                //        }
            }
            catch (Exception ex)
            {
            }
        }
        private void removePDFControls()
        {
            try
            {
                //foreach (Control p in pnlPDFViewer.Controls)
                //    if (p.GetType() == typeof(AxAcroPDFLib.AxAcroPDF))
                //    {
                //        AxAcroPDFLib.AxAcroPDF c = (AxAcroPDFLib.AxAcroPDF)p;
                //        c.Visible = false;
                //        pnlPDFViewer.Controls.Remove(c);
                //        c.Dispose();
                //    }
            }
            catch (Exception ex)
            {
            }
        }
        private void btnCloseDocument_Click_1(object sender, EventArgs e)
        {
            removePDFControls();
            showPDFFileGrid();
        }
        private void showPDFFileGrid()
        {
            try
            {
                //foreach (Control p in pnlPDFViewer.Controls)
                //    if (p.GetType() == typeof(DataGridView))
                //    {
                //        p.Visible = true;
                //    }
            }
            catch (Exception ex)
            {
            }
        }
        private void dgvDocumentList_CellContentClick(Object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //DataGridView dgv = sender as DataGridView;
                //string fileName = "";
                //if (e.RowIndex < 0)
                //    return;
                //if (e.ColumnIndex == 0)
                //{
                //    removePDFControls();
                //    fileName = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                //    ////string docDir = Main.documentDirectory + "\\" + docID;
                //    string subDir = prevaccode.AccountCode + "-" + prevaccode.Name;
                //    dgv.Enabled = false;
                //    fileName = DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                //    System.Diagnostics.Process.Start(fileName);
                //    dgv.Enabled = true;
                //}

            }
            catch (Exception ex)
            {
            }
        }
        private void showPDFFile(string fname)
        {
            try
            {
                removePDFControls();
                AxAcroPDFLib.AxAcroPDF pdf = new AxAcroPDFLib.AxAcroPDF();
                pdf.Dock = System.Windows.Forms.DockStyle.Fill;
                pdf.Enabled = true;
                pdf.Location = new System.Drawing.Point(0, 0);
                pdf.Name = "pdfReader";
                pdf.OcxState = pdf.OcxState;
                ////pdf.OcxState = ((System.Windows.Forms.AxHost.State)(new System.ComponentModel.ComponentResourceManager(typeof(ViewerWindow)).GetObject("pdf.OcxState")));
                pdf.TabIndex = 1;
                //pnlPDFViewer.Controls.Add(pdf);

                pdf.setShowToolbar(false);
                pdf.LoadFile(fname);
                pdf.setView("Fit");
                pdf.Visible = true;
                pdf.setZoom(100);
                pdf.setPageMode("None");
            }
            catch (Exception ex)
            {
            }
        }
        private void btnClose_Click_1(object sender, EventArgs e)
        {
            lblActionHeader.Visible = true;
            pnlTopButtons.Visible = true;
            removePDFFileGridView();
            removePDFControls();
            //pnlPDFViewer.Visible = false;
            pnlList.Visible = true;
            grdList.Visible = true;
            //btnNew.Visible = true;
            btnExit.Visible = true;
            btnActionPending.Visible = true;
            btnApprovalPending.Visible = true;
            btnApproved.Visible = true;
        }

        private void pnlApproved_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAddRemarks_Click(object sender, EventArgs e)
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            txt = new RichTextBox();
            txt.Bounds = new Rectangle(new Point(25, 25), new Size(350, 200));
            frmPopup.Controls.Add(txt);

            Label lblHeader = new Label();
            lblHeader.Size = new Size(300, 20);
            lblHeader.Text = "Type your Remarks";
            lblHeader.Location = new Point(50, 10);
            frmPopup.Controls.Add(lblHeader);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Location = new Point(100, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Click5);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Location = new Point(200, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click5);
            frmPopup.Controls.Add(lvCancel);

            txt.Focus();
            frmPopup.ShowDialog();
        }
        private void lvOK_Click5(object sender, EventArgs e)
        {
            try
            {
                //pnllv.Visible = true;
                if (txt.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please Add Comment");
                    return;
                }
                rtbRemarks.Text = rtbRemarks.Text + Login.userLoggedInName +
                            " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + txt.Text + "\n";
                rtbRemarks.ReadOnly = true;
                ContextMenu blankContextMenu = new ContextMenu();
                rtbRemarks.ContextMenu = blankContextMenu;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        private void lvCancel_Click5(object sender, EventArgs e)
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

        private void btnModifiedApprove_Click(object sender, EventArgs e)
        {
            try
            {
                LeaveApproveDB accDB = new LeaveApproveDB();
                leaveapprove lv = new leaveapprove();
                lv.EmployeeID = txtEmployeeID.Text;
                lv.leaveid = txtLeaveType.Text;
                lv.sanctionedTo = dtpApprToDate.Value;
                lv.remarks = rtbRemarks.Text.Replace("'", "''") + Login.userLoggedInName +
                            " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Approved Modified Leave" + "\n";
                lv.rowid = rowid;
                double totaldays = Convert.ToDouble(txtAppTotal.Text);
                if (dtpApprToDate.Value.Date < dtpApprFromDate.Value.Date || dtpApprToDate.Value.Date > dtpreqToDate.Value.Date)
                {
                    MessageBox.Show("Check Date!!!");
                    return;
                }
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the Modified Leave ?", "Confirm", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (accDB.ApproveCheck(lv, totaldays))
                    {
                        if (Convert.ToDouble(dgvLeaveDetails.Rows[2].Cells[txtLeaveType.Text].Value) >= totaldays)
                        {
                            if (lv.leaveid == "CO")
                            {
                                if (accDB.ApproveLeaveRequestModifiedCO(lv, totaldays))
                                {
                                    MessageBox.Show("Modified Leave Approved");
                                    closeAllPanels();
                                    listOption = 1;
                                    ListAccountDetail(listOption);
                                    setButtonVisibility("btnEditPanel");
                                }
                            }
                            else
                            {
                                if (accDB.ApproveLeaveRequestModified(lv))
                                {
                                    MessageBox.Show("Leave Approved");
                                    closeAllPanels();
                                    listOption = 1;
                                    ListAccountDetail(listOption);
                                    setButtonVisibility("btnEditPanel");
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("Employee has No Balance Of So many Leaves!!!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("NO Authorisation!!! Please Forward");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnModifiedReject_Click(object sender, EventArgs e)
        {
            try
            {
                LeaveApproveDB accDB = new LeaveApproveDB();
                leaveapprove lv = new leaveapprove();
                lv.EmployeeID = txtEmployeeID.Text;
                lv.leaveid = txtLeaveType.Text;
                lv.remarks = rtbRemarks.Text.Replace("'", "''") + Login.userLoggedInName +
                         " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Rejected Modified Leave" + "\n";
                lv.rowid = rowid;
                double totaldays = Convert.ToDouble(txtAppTotal.Text);
                DialogResult dialog = MessageBox.Show("Are you sure to Reject the Modified Leave ?", "Confirm", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (accDB.CancelledLeaveRequestModified(lv))
                    {
                        MessageBox.Show("Modified Leave Rejected");
                        closeAllPanels();
                        listOption = 1;
                        ListAccountDetail(listOption);
                        setButtonVisibility("btnEditPanel");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                LeaveApproveDB accDB = new LeaveApproveDB();
                leaveapprove lv = new leaveapprove();
                lv.EmployeeID = txtEmployeeID.Text;
                lv.leaveid = txtLeaveType.Text;
                lv.remarks = rtbRemarks.Text.Replace("'", "''") + Login.userLoggedInName +
                            " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Cancel Approved" + "\n";
                lv.rowid = rowid;
                double totaldays = Convert.ToDouble(txtAppTotal.Text);
                DialogResult dialog = MessageBox.Show("Are you sure to Approve Leave Cancel Request ?", "Confirm", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (lv.leaveid == "CO")
                    {
                        int val = accDB.CheckCancel(lv.EmployeeID);
                        if ((val + Convert.ToInt32(txtAppTotal.Text)) <= 3)
                        {
                            if (accDB.ApproveCancelRequestCO(lv, 1))
                            {
                                MessageBox.Show("Leave Cancel Approved");
                                closeAllPanels();
                                listOption = 1;
                                ListAccountDetail(listOption);
                                setButtonVisibility("btnEditPanel");
                            }
                            else
                            {
                                MessageBox.Show("Leave was not canncelled");
                            }
                        }
                        else
                        {
                            int tp = 3 - val;
                            if (accDB.ApproveCancelRequestCO(lv, tp))
                            {
                                MessageBox.Show("Leave Cancel Approved");
                                closeAllPanels();
                                listOption = 1;
                                ListAccountDetail(listOption);
                                setButtonVisibility("btnEditPanel");
                            }
                            else
                            {
                                MessageBox.Show("Leave was Not canncelled");
                            }
                        }
                    }
                    else
                    {
                        if (accDB.ApproveCancelRequest(lv))
                        {
                            MessageBox.Show("Leave Cancel Approved");
                            closeAllPanels();
                            listOption = 1;
                            ListAccountDetail(listOption);
                            setButtonVisibility("btnEditPanel");
                        }
                        else
                        {
                            MessageBox.Show("Leave was Not canncelled");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dtpApprToDate_ValueChanged(object sender, EventArgs e)
        {
            double dt = 0;
            dt = (dtpApprToDate.Value - dtpApprFromDate.Value).TotalDays;
            dt += 1;
            txtAppTotal.Text = dt.ToString();
        }

        private void dtpApprFromDate_ValueChanged(object sender, EventArgs e)
        {
            double dt = 0;
            dt = (dtpApprToDate.Value - dtpApprFromDate.Value).TotalDays;
            dt += 1;
            txtAppTotal.Text = dt.ToString();
        }

        private void cmbAppFromSessn_SelectedIndexChanged(object sender, EventArgs e)
        {
            double dt = 0;
            dt = (dtpApprToDate.Value - dtpApprFromDate.Value).TotalDays;
            dt += 1;
            txtAppTotal.Text = dt.ToString();
        }

        private void cmbAppToSessn_SelectedIndexChanged(object sender, EventArgs e)
        {
            double dt = 0;
            dt = (dtpApprToDate.Value - dtpApprFromDate.Value).TotalDays;
            dt += 1;
            txtAppTotal.Text = dt.ToString();
        }

        private void txtAppTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                LeaveApproveDB accDB = new LeaveApproveDB();
                leaveapprove lv = new leaveapprove();
                lv.EmployeeID = txtEmployeeID.Text;
                lv.leaveid = txtLeaveType.Text;
                lv.remarks = rtbRemarks.Text.Replace("'", "''") + Login.userLoggedInName +
                            " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Request Cancelled " + "\n";
                lv.rowid = rowid;
                double totaldays = Convert.ToDouble(txtAppTotal.Text);
                DialogResult dialog = MessageBox.Show("Are you sure to Reject Cancel request?", "Confirm", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (accDB.CancelCancelRequest(lv))
                    {
                        MessageBox.Show("Leave Cancel Rejected");
                        closeAllPanels();
                        listOption = 1;
                        ListAccountDetail(listOption);
                        setButtonVisibility("btnEditPanel");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        DateTime prevtodate = DateTime.Now;
        private void btnModifyAccept_Click(object sender, EventArgs e)
        {
            try
            {
                LeaveApproveDB accDB = new LeaveApproveDB();
                leaveapprove lv = new leaveapprove();
                lv.EmployeeID = txtEmployeeID.Text;
                lv.leaveid = txtLeaveType.Text;
                lv.remarks = rtbRemarks.Text.Replace("'", "''") + Login.userLoggedInName +
                             " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Accepted Modification" + "\n";
                lv.rowid = rowid;
                prevtodate = dtpApprFromDate.Value;
                double totaldays = Convert.ToDouble(txtAppTotal.Text);
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the Modify Leave ?", "Confirm", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (accDB.ApproveCheck(lv, totaldays))
                    {
                        if (accDB.ApproveLeaveRequestModify(lv))
                        {
                            MessageBox.Show("Modify Leave Approved");
                            closeAllPanels();
                            listOption = 1;
                            ListAccountDetail(listOption);
                            setButtonVisibility("btnEditPanel");
                        }
                    }
                    else
                    {
                        MessageBox.Show("NO Authorisation!!! Please Forward");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnModifyCancel_Click(object sender, EventArgs e)
        {
            try
            {
                LeaveApproveDB accDB = new LeaveApproveDB();
                leaveapprove lv = new leaveapprove();
                lv.EmployeeID = txtEmployeeID.Text;
                lv.leaveid = txtLeaveType.Text;
                lv.remarks = rtbRemarks.Text.Replace("'", "''") + Login.userLoggedInName +
                            " : " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss") + " : " + "Cancelled Modification" + "\n";
                lv.rowid = rowid;
                double totaldays = Convert.ToDouble(txtAppTotal.Text);
                DialogResult dialog = MessageBox.Show("Are you sure to Cancel the Modify Leave ?", "Confirm", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (accDB.CancelledLeaveRequestModify(lv))
                    {
                        MessageBox.Show("Leave Modify Cancelled");
                        closeAllPanels();
                        listOption = 1;
                        ListAccountDetail(listOption);
                        setButtonVisibility("btnEditPanel");
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dgvLeaveDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvLeaveDetails_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void LeaveApprove_Enter(object sender, EventArgs e)
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

