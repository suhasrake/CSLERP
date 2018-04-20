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
using CSLERP.PrintForms;

namespace CSLERP
{
    public partial class WorkplaceCReport : System.Windows.Forms.Form
    {
        string docID = "WorkplaceCReport";
        int opt = 1; //1-Pending, 2-In Process, 3-Approved
        ListView exlv = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        public WorkplaceCReport()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }
        private void BREntry_Load(object sender, EventArgs e)
        {
            //////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;

            //String a = this.Size.ToString();
            grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdList.EnableHeadersVisualStyles = false;
            pnlList.Visible = true;
            pnlHeader.Visible = true;
            btnExportExcel.Visible = false;
        }
        private void ListFilteredComplaints(int opt)
        {
            try
            {
                grdList.Rows.Clear();
                WorkplaceCRDB wcdb = new WorkplaceCRDB();
                string ComplaintType = "";
                if (opt == 1)
                {
                    ComplaintType = ((Structures.ComboBoxItem)cmbComplaintType.SelectedItem).HiddenValue;
                }
                //ComplaintType = ((Structures.ComboBoxItem)cmbComplaintType.SelectedItem).HiddenValue;
                List<workplacecr> CompList = wcdb.getFilteredComplaintsForReport(opt, dtFromDate.Value, dtToDate.Value, ComplaintType);
                foreach (workplacecr br in CompList)
                {

                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["cDate"].Value = br.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["Complainer"].Value = br.EmployeeName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ComplaintType"].Value = br.ComplaintType;
                    grdList.Rows[grdList.RowCount - 1].Cells["ComplaintDescription"].Value = br.ComplaintDescription;
                    grdList.Rows[grdList.RowCount - 1].Cells["Remark"].Value = br.Remarks;
                    if (br.AcceptTime != DateTime.MinValue)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["AcceptTime"].Value = br.AcceptTime;
                    }
                    if (br.AcceptTime != DateTime.MinValue)
                    {
                        grdList.Rows[grdList.RowCount - 1].Cells["ClosedTime"].Value = br.CloseTime;
                    }
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = valuetostring(br.Status, br.DocumentStatus);
                    //grdList.Rows[grdList.RowCount - 1].Cells["DocStatus"].Value = br.DocumentStatus;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Complaint Listing");
            }
            setButtonVisibility("init");
            if (grdList.Rows.Count > 0)
                btnExportExcel.Visible = true;
            else
                btnExportExcel.Visible = false;
            pnlList.Visible = true;
        }
        private void initVariables()
        {
            dtFromDate.Enabled = true;
            dtToDate.Enabled = true;
            CatalogueValueDB.fillCatalogValueComboNew(cmbComplaintType, "WorkplaceComplaints");
            docID = Main.currentDocument;
            dtFromDate.Format = DateTimePickerFormat.Custom;
            dtFromDate.CustomFormat = "dd-MM-yyyy";
            dtToDate.Format = DateTimePickerFormat.Custom;
            dtToDate.CustomFormat = "dd-MM-yyyy";
            pnlUI.Controls.Add(pnlList);
            closeAllPanels();
            grdList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            setButtonVisibility("init");
            clearData();
        }
        
        private void closeAllPanels()
        {
            try
            {
                pnlList.Visible = false;
            }
            catch (Exception)
            {
            }
        }
        public void clearData()
        {
            try
            {
                grdList.Rows.Clear();
                removeControlsFromPnlLvPanel();
                dtFromDate.Value = DateTime.Now.AddMonths(-1);
                dtToDate.Value = DateTime.Now;
                cmbComplaintType.SelectedIndex = -1;
            }
            catch (Exception ex)
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
                clearData();
                closeAllPanels();
                setButtonVisibility("btnNew");
            }
            catch (Exception)
            {
            }
        }
        private void pnlList_Paint(object sender, PaintEventArgs e)
        {
        }

        private void setButtonVisibility(string btnName)
        {
            try
            {
                //btnExportToExcell.Visible = false;
                grdList.Visible = false;
                btnExit.Visible = true;
                pnlBottomButtons.Visible = true;
                if (btnName == "init")
                {
                    btnExit.Visible = true;
                }
                else if (btnName == "btnCheck")
                {
                    grdList.Visible = true;
                    //btnUpdate.Visible = true;
                    //btnExportToExcell.Visible = true;
                }
                int ups = getuserPrivilegeStatus();
                if (ups == 1 || ups == 0)
                {
                }
            }
            catch (Exception ex)
            {
            }
        }
        void handleNewButton()
        {
        }
        void handleGrdEditButton()
        {
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
        private void btnCheck_Click(object sender, EventArgs e)
        {
            grdList.Rows.Clear();
            removeControlsFromPnlLvPanel();
            btnExportExcel.Visible = false;
            DateTime dtNow = UpdateTable.getSQLDateTime();
            if (dtFromDate.Value.Date > dtToDate.Value.Date ||
                dtFromDate.Value.Date > dtNow.Date || dtToDate.Value.Date > dtNow.Date)
            {
                MessageBox.Show("Check FromDate And Todate.");
                grdList.Rows.Clear();
                grdList.Visible = false;
                return;
            }
            if (cmbComplaintType.SelectedIndex == -1)
            {
                opt = 2;
            }
            else
            {
                opt = 1;
            }
            ListFilteredComplaints(opt);
            grdList.Visible = true;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Rows.Clear();
            removeControlsFromPnlLvPanel();
            btnExportExcel.Visible = false;
            grdList.Visible = false;
        }
        private void dtFromDate_ValueChanged(object sender, EventArgs e)
        {
            grdList.Rows.Clear();
            removeControlsFromPnlLvPanel();
            btnExportExcel.Visible = false;
            grdList.Visible = false;
        }
        private void dtToDate_ValueChanged(object sender, EventArgs e)
        {
            grdList.Rows.Clear();
            removeControlsFromPnlLvPanel();
            btnExportExcel.Visible = false;
            grdList.Visible = false;
        }
        public string valuetostring(int stat, int docstat)
        {
            string status = "";
            if (stat == 1 && docstat == 1)
            {
                status = "Created";
            }
            else if (stat == 2 && (docstat == 1 || docstat == 3))
            {
                status = "Accepted";
            }
            else if (stat == 3 && docstat == 2)
            {
                status = "Rejected";
            }
            else if (stat == 4 && docstat == 1)
            {
                status = "Cancelled";
            }
            else if (stat == 5 && docstat == 2)
            {
                status = "Completed";
            }
            else if (stat == 6 && (docstat == 1 || docstat == 3))
            {
                status = "Closed By MM";
            }
            else if (stat == 1 && docstat == 3)
            {
                status = "Reversed";
            }
            return status;
        }
        private void removeControlsFromPnlLvPanel()
        {
            try
            {
                pnllv.Controls.Clear();
                Control nc = pnllv.Parent;
                nc.Controls.Remove(pnllv);
            }
            catch (Exception ex)
            {
            }
        }
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdList.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in grid");
                    return;
                }
                removeControlsFromPnlLvPanel();
                pnllv = new Panel();
                pnllv.BorderStyle = BorderStyle.FixedSingle;

                pnllv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(100, 100), new Size(600, 300));
                exlv = Utilities.GridColumnSelectionView(grdList);
                exlv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(50, 50), new Size(500, 200));
                pnllv.Controls.Add(exlv);

                System.Windows.Forms.Label pnlHeading = new System.Windows.Forms.Label();
                pnlHeading.Size = new Size(300, 20);
                pnlHeading.Location = new System.Drawing.Point(50, 20);
                pnlHeading.Text = "Select Gridview Colums to Export";
                pnlHeading.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                pnlHeading.ForeColor = Color.Black;
                pnllv.Controls.Add(pnlHeading);

                System.Windows.Forms.Button exlvOK = new System.Windows.Forms.Button();
                exlvOK.Text = "OK";
                exlvOK.Location = new System.Drawing.Point(50, 270);
                exlvOK.Click += new System.EventHandler(this.exlvOK_Click1);
                pnllv.Controls.Add(exlvOK);

                System.Windows.Forms.Button exlvCancel = new System.Windows.Forms.Button();
                exlvCancel.Text = "Cancel";
                exlvCancel.Location = new System.Drawing.Point(150, 270);
                exlvCancel.Click += new System.EventHandler(this.exlvCancel_Click2);
                pnllv.Controls.Add(exlvCancel);
                pnlList.Controls.Add(pnllv);
                pnllv.BringToFront();
                pnllv.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in grid data. Export failed");
            }
        }
        private void exlvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                pnllv.Visible = false;
                string ctype = "";
                if (cmbComplaintType.SelectedIndex == -1)
                    ctype = "All";
                else
                    ctype = ((Structures.ComboBoxItem)cmbComplaintType.SelectedItem).HiddenValue;
                string heading1 = "Complaint Report";
                string heading2 = "";
                string heading3 = "Complaint Type" + Main.delimiter1 + ctype + Main.delimiter2 +
                    "Form Date" + Main.delimiter1 + dtFromDate.Value.ToString("dd/MM/yyyy") + Main.delimiter2 +
                    "To Date" + Main.delimiter1 + dtToDate.Value.ToString("dd/MM/yyyy");
                Utilities.export2Excel(heading1, heading2, heading3, grdList, exlv);
                removeControlsFromPnlLvPanel();
                pnllv.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }
        private void exlvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                removeControlsFromPnlLvPanel();
                pnllv.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearData();
            grdList.Visible = false;
            btnExportExcel.Visible = false;
          
        }

        private void WorkplaceCReport_Enter(object sender, EventArgs e)
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





