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
    public partial class EmployeeReport : System.Windows.Forms.Form
    {
        ListView lvCopy = new ListView();
        Panel pnlForwarder = new Panel();
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        TextBox txtSearch = new TextBox();
        Panel pnlModel = new Panel();
        ListView exlv = new ListView();
        public EmployeeReport()
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

            String a = this.Size.ToString();
            grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdList.EnableHeadersVisualStyles = false;
        }

        private void ListEmployeeReport()
        {
            try
            {
                grdList.Rows.Clear();
                EmployeeDB empdb = new EmployeeDB();
                List<employee> emplist = empdb.getEmployees();

                foreach (employee mh in emplist)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["mSerialNo"].Value = grdList.Rows.Count;
                    grdList.Rows[grdList.RowCount - 1].Cells["mEmployeeID"].Value = mh.empID;
                    grdList.Rows[grdList.RowCount - 1].Cells["mEmpoyeeName"].Value = mh.empName;
                    grdList.Rows[grdList.RowCount - 1].Cells["mOffice"].Value = mh.office;
                    grdList.Rows[grdList.RowCount - 1].Cells["mReportingOfficer"].Value = mh.reportingofficer;
                    grdList.Rows[grdList.RowCount - 1].Cells["mDesignation"].Value = mh.designation;
                    grdList.Rows[grdList.RowCount - 1].Cells["mDepartment"].Value = mh.department;
                }
                if (grdList.Rows.Count != 0)
                    btnExportToExcell.Visible = true;
                else
                    btnExportToExcell.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in PR Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;
        }
        private void initVariables()
        {
            pnlUI.Controls.Add(pnlList);
            grdList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grdList.Columns[0].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdList.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            setButtonVisibility("init");
        }
        public void clearData()
        {
            try
            {
                grdList.Rows.Clear();
                pnllv.Visible = true;
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
        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void pnlList_Paint(object sender, PaintEventArgs e)
        {
        }


        private void setButtonVisibility(string btnName)
        {
            try
            {
                ////grdList.Columns[1].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                grdList.Visible = false;
                btnExit.Visible = true;
                pnlBottomButtons.Visible = true;
                pnlMenu.Visible = true;
                if (btnName == "init")
                {
                   
                    btnExit.Visible = true;
                    pnlMenu.Visible = true;
                }
                else if (btnName == "btnCheck")
                {
                    grdList.Visible = true;
                    pnlMenu.Visible = true;
                }
            }
            catch (Exception ex)
            {
            }
        }
        //int getuserPrivilegeStatus()
        //{
        //    try
        //    {
        //        if (Main.itemPriv[0] && !Main.itemPriv[1] && !Main.itemPriv[2]) //only view
        //            return 1;
        //        else if (Main.itemPriv[0] && (Main.itemPriv[1] || Main.itemPriv[2])) //view add and edit
        //            return 2;
        //        else if (!Main.itemPriv[0] && (Main.itemPriv[1] || Main.itemPriv[2])) //view add and edit
        //            return 3;
        //        else if (!Main.itemPriv[0] && !Main.itemPriv[1] || !Main.itemPriv[2]) //no privilege
        //            return 0;
        //        else
        //            return -1;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return 0;
        //}
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

        private void removeControlsFromlvPanel()
        {
            try
            {
                //foreach (Control p in pnllv.Controls)
                //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
                //    {
                //        p.Dispose();
                //    }
                pnllv.Controls.Clear();
                Control nc = pnllv.Parent;
                nc.Controls.Remove(pnllv);
            }
            catch (Exception ex)
            {
            }
        }


        private void btnViewHistory_Click_1(object sender, EventArgs e)
        {
            removeControlsFromlvPanel();
            btnExportToExcell.Visible = true;
            ListEmployeeReport();
            grdList.Visible = true;
        }
        private void btnSelect_Click_1(object sender, EventArgs e)
        {
            try
            {
                //showEmployeeLIstView();
            }
            catch (Exception ex)
            {
            }
        }

        private void btnExportToExcell_Click(object sender, EventArgs e)
        {
            try
            {
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
            }
        }
        private void removeControlsFromPnlLvPanel()
        {
            try
            {
                //foreach (Control p in pnllv.Controls)
                //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(System.Windows.Forms.Button))
                //    {
                //        p.Dispose();
                //    }
                pnllv.Controls.Clear();
                Control nc = pnllv.Parent;
                nc.Controls.Remove(pnllv);
            }
            catch (Exception ex)
            {
            }
        }
        private void exlvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                
                pnllv.Visible = false;
                string heading1 = "Employee Report";
                //heading2 = heading2 + " To Date:" + dtToDate.Value.ToString("dd-MM-yyyy");
                Utilities.export2Excel(heading1, "", "", grdList, exlv);
                removeControlsFromPnlLvPanel();
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
            grdList.Rows.Clear();
            //txtEmpID.Text = "";
            //cmbPurpose.SelectedIndex = -1;
            //cmbDocStatus.SelectedIndex = -1;
            btnExportToExcell.Visible = false;
            grdList.Visible = false;
            //btnSelect.Enabled = true;
            removeControlsFromlvPanel();
            pnllv.Visible = false;
        }

        private void cmbDocStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            removeControlsFromlvPanel();
            grdList.Rows.Clear();
            btnExportToExcell.Visible = false;
            grdList.Visible = false;
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
        private Boolean checkLVCopyItemChecked(string str)
        {
            Boolean status = true;
            try
            {
                if (lvCopy.CheckedIndices.Count > 1)
                {
                    MessageBox.Show("Only one " + str + " allowed");
                    return false;
                }
                if (lvCopy.CheckedItems.Count == 0)
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

        private void cmbPurpose_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Rows.Clear();
            removeControlsFromlvPanel();
            btnExportToExcell.Visible = false;
            grdList.Visible = false;
        }

        private void EmployeeReport_Enter(object sender, EventArgs e)
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





