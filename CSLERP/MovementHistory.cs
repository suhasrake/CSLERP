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
    public partial class MovementHistory : System.Windows.Forms.Form
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
        public MovementHistory()
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
        private int getStatus(String stat)
        {
            int status = 0;
            if (stat.Equals("Approved"))
                status = 2;
            else if (stat.Equals("ApprovalPending"))
                status = 1;
            else if (stat.Equals("Rejected"))
                status = 10;
            else if (stat.Equals("Out"))
                status = 3;
            else if (stat.Equals("In"))
                status = 99;
            else if (stat.Equals("Canceled"))
                status = 98;          
            else if (stat.Equals("All"))
                status = 0;
            else if (stat.Equals("Auto-In"))
                status = 4;
            return status;
        }
        private int getOPtion()
        {
            int opt = 0;
            if (txtEmpID.Text.Trim().Length != 0 && cmbDocStatus.SelectedIndex != -1
                        && cmbPurpose.SelectedIndex != -1)
            {
                opt = 8;
            }
            else if (txtEmpID.Text.Trim().Length != 0 && cmbDocStatus.SelectedIndex != -1)
            {
                opt = 6;
            }
            else if (txtEmpID.Text.Trim().Length != 0 && cmbPurpose.SelectedIndex != -1)
            {
                opt = 5;
            }
            else if (cmbDocStatus.SelectedIndex != -1 && cmbPurpose.SelectedIndex != -1)
            {
                opt = 7;
            }
            else if (txtEmpID.Text.Trim().Length != 0)
            {
                opt = 2;
            }
            else if (cmbPurpose.SelectedIndex != -1)
            {
                opt = 3;
            }
            else if (cmbDocStatus.SelectedIndex != -1)
            {
                opt =4;
            }
            
            else if (txtEmpID.Text.Trim().Length == 0 && cmbDocStatus.SelectedIndex == -1 && cmbPurpose.SelectedIndex == -1)
            {
                opt = 1;
            }
            
            return opt;
        }
        private void ListFilteredmovementhistory()
        {
            try
            {
                grdList.Rows.Clear();
                MovementHistoryDB mhdb = new MovementHistoryDB();
                List<movementhistory> MHList = new List<movementhistory>();
                Dictionary<String, String> dict = new Dictionary<string, string>();
                string selEmp = "";
                if (txtEmpID.Text.Trim().Length != 0)
                    selEmp = txtEmpID.Text.Substring(0, txtEmpID.Text.IndexOf('-'));
                string selPurpose = "";
                if (cmbPurpose.SelectedIndex != -1)
                    selPurpose = ((Structures.ComboBoxItem)cmbPurpose.SelectedItem).HiddenValue;


                if (txtEmpID.Text.Trim().Length != 0)
                    dict.Add("EmpId", selEmp);
                else
                    dict.Add("EmpId", "");
                if (cmbPurpose.SelectedIndex != -1)
                    dict.Add("Purpose", selPurpose);
                else
                    dict.Add("Purpose", "");
                if (cmbDocStatus.SelectedIndex != -1)
                    if(cmbDocStatus.SelectedItem.ToString() =="Canceled")
                    {
                        dict.Add("DocStat", "6,1,98");
                    }
                else
                    {
                        dict.Add("DocStat", getStatus(cmbDocStatus.SelectedItem.ToString()).ToString());
                    }
                   
                else
                    dict.Add("DocStat", "");
                if (cmbDocStatus.SelectedIndex != -1)
                {
                    if (cmbDocStatus.SelectedItem.ToString().Equals("All"))
                    {
                        if (txtEmpID.Text.Length != 0 && cmbPurpose.SelectedIndex != -1)
                            MHList = mhdb.getMovementHistoryForAll(dtFromDate.Value, dtToDate.Value, selEmp, selPurpose);
                        else if(txtEmpID.Text.Length != 0)
                            MHList = mhdb.getMovementHistoryForAll(dtFromDate.Value, dtToDate.Value, selEmp, "");
                        else if (cmbPurpose.SelectedIndex != -1)
                            MHList = mhdb.getMovementHistoryForAll(dtFromDate.Value, dtToDate.Value, "", selPurpose);
                        else
                            MHList = mhdb.getMovementHistoryForAll(dtFromDate.Value, dtToDate.Value, "", "");
                    }
                    else
                    {
                        MHList = mhdb.getFilteredMovementHistory(dtFromDate.Value, dtToDate.Value, getOPtion(), dict);
                    }
                }
                else
                {
                    MHList = mhdb.getFilteredMovementHistory(dtFromDate.Value, dtToDate.Value, getOPtion(), dict);
                }

                //grdList.Columns["dt"].Visible = true;


                foreach (movementhistory mh in MHList)
                {
                    if( cmbDocStatus.SelectedIndex!=2 && cmbDocStatus.SelectedIndex != 0 &&  mh.documentstatus==1 && mh.status==1)
                    {
                        continue;
                    }
                    grdList.Rows.Add();
                    //grdList.Rows[grdList.RowCount - 1].Cells["mRowID"].Value = mh.rowid;
                    grdList.Rows[grdList.RowCount - 1].Cells["mDate"].Value = mh.date;
                    grdList.Rows[grdList.RowCount - 1].Cells["mEmployee"].Value = mh.empname;
                    grdList.Rows[grdList.RowCount - 1].Cells["mPurpose"].Value = mh.purpose;
                    grdList.Rows[grdList.RowCount - 1].Cells["mExitTime"].Value = mh.exittime;
                    grdList.Rows[grdList.RowCount - 1].Cells["mReturnTime"].Value = mh.returntime;
                    grdList.Rows[grdList.RowCount - 1].Cells["aExitTime"].Value = mh.actexittime;
                    grdList.Rows[grdList.RowCount - 1].Cells["aReturnTime"].Value = mh.actreturntime;
                    grdList.Rows[grdList.RowCount - 1].Cells["MovementStatus"].Value = MovementRegisterDB.valuetostring(mh.documentstatus,mh.status);
                }
                if (grdList.Rows.Count != 0)
                    btnExportToExcell.Visible = true;
                else
                    btnExportToExcell.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Movement History  Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;
        }
        private void initVariables()
        {
            dtFromDate.Format = DateTimePickerFormat.Custom;
            dtFromDate.CustomFormat = "dd-MM-yyyy";
            CatalogueValueDB.fillCatalogValueComboNew(cmbPurpose, "MovementPurpose");
            dtFromDate.Value = DateTime.Now.AddMonths(-1);
            dtToDate.Format = DateTimePickerFormat.Custom;
            dtToDate.CustomFormat = "dd-MM-yyyy";
            dtToDate.Value = DateTime.Now;
            pnlUI.Controls.Add(pnlList);
            grdList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            setButtonVisibility("init");
        }
        public void clearData()
        {
            try
            {
                grdList.Rows.Clear();
                pnllv.Visible = true;
                txtEmpID.Text = "";
                dtFromDate.Value = DateTime.Now.AddMonths(-1);
                dtToDate.Value = DateTime.Now;
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

        private void showEmployeeLIstView()
        {
            removeControlsFromlvPanel();
            //pnlList.Controls.Remove(pnllv);
            //btnSelect.Enabled = false;
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;
            pnllv.BackColor = Color.DarkSeaGreen;
            pnllv.Bounds = new Rectangle(new Point(350, 32), new Size(477, 282));
            lv = EmployeeDB.getEmployeeListView();
            lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            lv.Bounds = new Rectangle(new Point(17, 14), new Size(443, 212));
            pnllv.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Location = new Point(44, 246);
            lvOK.Click += new System.EventHandler(this.lvOK_Click1);
            pnllv.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Location = new Point(141, 246);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click1);
            pnllv.Controls.Add(lvCancel);

            Label lblSearch = new Label();
            lblSearch.Text = "Find";
            lblSearch.Location = new Point(260, 250);
            lblSearch.Size = new Size(37, 15);
            pnllv.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Location = new Point(303, 246);
            txtSearch.TextChanged += new EventHandler(txtSearch_TextChangedEmp);
            pnllv.Controls.Add(txtSearch);

            pnlList.Controls.Add(pnllv);
            pnllv.BringToFront();
            pnllv.Visible = true;
            txtSearch.Focus();
        }
        //private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        //{
        //    try
        //    {
        //        if (lv.CheckedIndices.Count > 1)
        //        {
        //            MessageBox.Show("Cannot select more than one item");
        //            e.Item.Checked = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                
                if (lv.Visible == true)
                {
                    if (!checkLVItemChecked("Employee"))
                    {
                        return;
                    }
                    //btnSelect.Enabled = true;
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            txtEmpID.Text = itemRow.SubItems[1].Text + "-" + itemRow.SubItems[2].Text;
                            pnllv.Controls.Remove(lv);
                            pnllv.Visible = false;
                            grdList.Rows.Clear();
                            grdList.Visible = false;
                            btnExportToExcell.Visible = false;
                        }
                    }
                }
                else
                {
                    if (!checkLVCopyItemChecked("Employee"))
                    {
                        return;
                    }
                    //btnSelect.Enabled = true;
                    foreach (ListViewItem itemRow in lvCopy.Items)
                    {
                        if (itemRow.Checked)
                        {
                            txtEmpID.Text = itemRow.SubItems[1].Text + "-" + itemRow.SubItems[2].Text;
                            pnllv.Controls.Remove(lvCopy);
                            pnllv.Visible = false;
                            grdList.Rows.Clear();
                            grdList.Visible = false;
                            btnExportToExcell.Visible = false;
                        }
                    }
                }
                pnllv.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }
        private void txtSearch_TextChangedEmp(object sender, EventArgs e)
        {
            pnllv.Controls.Remove(lvCopy);
            addItemsEmp();
        }
        private void addItemsEmp()
        {
            lvCopy = new ListView();
            lvCopy.View = System.Windows.Forms.View.Details;
            lvCopy.LabelEdit = true;
            lvCopy.AllowColumnReorder = true;
            lvCopy.CheckBoxes = true;
            lvCopy.FullRowSelect = true;
            lvCopy.GridLines = true;
            lvCopy.Sorting = System.Windows.Forms.SortOrder.Ascending;
            lvCopy.Columns.Add("Select", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Emp Id", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Emp Name", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Department", -2, HorizontalAlignment.Left);

            lvCopy.Sorting = System.Windows.Forms.SortOrder.None;
            lvCopy.ColumnClick += new ColumnClickEventHandler(LvColumnClick); ;
            //this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
            //lvCopy.Location = new Point(13, 9);
            //lvCopy.Size = new Size(440, 199);
            lvCopy.Bounds = new Rectangle(new Point(17, 14), new Size(443, 212));
            lvCopy.Items.Clear();
            foreach (ListViewItem row in lv.Items)
            {
                string x = row.SubItems[0].Text;
                string no = row.SubItems[1].Text;
                string ch = row.SubItems[2].Text;
                string name = row.SubItems[3].Text;
                if (ch.ToLower().StartsWith(txtSearch.Text))
                {
                    ListViewItem item = new ListViewItem();
                    item.SubItems.Add(no);
                    item.SubItems.Add(ch);
                    item.SubItems.Add(name);
                    item.Checked = false;
                    lvCopy.Items.Add(item);
                }
            }
            lv.Visible = false;
            lvCopy.Visible = true;
            pnllv.Controls.Add(lvCopy);
        }
        private void lvCancel_Click1(object sender, EventArgs e)
        {
            try
            {
                //btnSelect.Enabled = true;
                pnllv.Visible = false;
                lv.CheckBoxes = false;
                lv.CheckBoxes = true;
                pnllv.Controls.Clear();
                pnllv.Visible = false;
            }
            catch (Exception)
            {
            }
        }
        private void lvOK_Click3(object sender, EventArgs e)
        {
            try
            {
                if (lv.Visible == true)
                {
                    foreach (ListViewItem itemRow in lv.Items)
                    {

                        if (itemRow.Checked)
                        {
                            txtEmpID.Text = itemRow.SubItems[1].Text + "-" + itemRow.SubItems[2].Text;
                            grdList.Rows.Clear();
                            itemRow.Checked = false;
                            grdList.Visible = false;
                            btnExportToExcell.Visible = false;
                            pnllv.Controls.Remove(lv);
                            pnllv.Visible = false;
                        }
                    }
                }
                else
                {
                    foreach (ListViewItem itemRow in lvCopy.Items)
                    {

                        if (itemRow.Checked)
                        {
                            txtEmpID.Text = itemRow.SubItems[1].Text + "-" + itemRow.SubItems[2].Text;
                            grdList.Rows.Clear();
                            itemRow.Checked = false;
                            pnllv.Controls.Remove(lvCopy);
                            grdList.Visible = false;
                            btnExportToExcell.Visible = false;
                            pnllv.Visible = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void lvCancel_Click3(object sender, EventArgs e)
        {
            try
            {
                btnSelect.Enabled = true;
                lv.CheckBoxes = false;
                lv.CheckBoxes = true;
                pnllv.Controls.Remove(lv);
                pnllv.Visible = false;
            }
            catch (Exception)
            {
            }
        }
        //private void CopylistView2_ItemChecked(object sender, ItemCheckedEventArgs e)
        //{
        //    try
        //    {
        //        if (lvCopy.CheckedIndices.Count > 1)
        //        {
        //            MessageBox.Show("Cannot select more than one item");
        //            e.Item.Checked = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        private void btnViewHistory_Click_1(object sender, EventArgs e)
        {
            removeControlsFromlvPanel();
            btnExportToExcell.Visible = true;
            ListFilteredmovementhistory();
            grdList.Visible = true;
        }
        private void btnSelect_Click_1(object sender, EventArgs e)
        {
            try
            {
                showEmployeeLIstView();
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
                string heading1 = "Employee Movement";
                string heading2 = "From Date: " + dtFromDate.Value.ToString("dd-MM-yyyy");
                heading2 = heading2 + " To Date:" + dtToDate.Value.ToString("dd-MM-yyyy");
                string heading3 = "";
                Utilities.export2Excel(heading1, heading2, heading3, grdList, exlv);
                removeControlsFromPnlLvPanel();
            }
            catch (Exception ex)
            {
            }
            ////try
            ////{
            ////    heading3 = "EmployeeID" + Main.delimiter1 + txtEmpID.Text + Main.delimiter2;
            ////    if (cmbDocStatus.SelectedIndex  >= 0)
            ////    {
            ////        heading3 = heading3 + "Status" + Main.delimiter1 + cmbDocStatus.SelectedItem.ToString() + Main.delimiter2;
            ////    }
            ////    else
            ////    {
            ////        heading3 = heading3 + "Status" + Main.delimiter1 + "All" + Main.delimiter2;
            ////    }

            ////    heading3 = heading3 + "Form Date" + Main.delimiter1 + dtFromDate.Value.ToString("dd-MM-yyyy") + Main.delimiter2;
            ////    heading3 = heading3 + "To Date" + Main.delimiter1 + dtToDate.Value.ToString("dd-MM-yyyy");
            ////}
            ////catch (Exception ex)
            ////{

            ////}

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
            txtEmpID.Text = "";
            cmbPurpose.SelectedIndex = -1;
            cmbDocStatus.SelectedIndex = -1;
            btnExportToExcell.Visible = false;
            grdList.Visible = false;
            btnSelect.Enabled = true;
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

        private void MovementHistory_Enter(object sender, EventArgs e)
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





