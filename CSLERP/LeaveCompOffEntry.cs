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
    public partial class LeaveCompOffEntry : System.Windows.Forms.Form
    {
        Panel pnllv = new Panel(); 
        ListView lv = new ListView();
        TextBox txtSearch = new TextBox();
        ListView lvCopy = new ListView();
        public static string[,] documentStatusValues;
        string docdata = "";
        public LeaveCompOffEntry()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void DocumentUC_Load(object sender, EventArgs e)
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
            //ListDocument();
           
        }
        private void ListDocument()
        {
            try
            {
                grdList.Rows.Clear();
                LeaveCompOffEntryDB lcdb = new LeaveCompOffEntryDB();
                List<leavecompoff> lco = lcdb.getLeaveOBDetails(Convert.ToInt32(txtEmpID.Text));
                foreach (leavecompoff lcf in lco)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["RowID"].Value = lcf.RowID;
                    grdList.Rows[grdList.RowCount - 1].Cells["Date"].Value = lcf.cdate.Date.ToString("dd-MM-yyyy");
                    grdList.Rows[grdList.RowCount - 1].Cells["EmpID"].Value = lcf.EmployeeID;
                    grdList.Rows[grdList.RowCount - 1].Cells["empStatus"].Value = statusvalue( lcf.Status);
                }
                if(grdList.Rows.Count>=lcdb.getMaxAccuralForLeaveType())
                {
                    btnNew.Visible = false;
                }
                else
                {
                    applyPrivilege();
                    btnNew.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            
            enableBottomButtons();
            pnlMainList.Visible = true;
        }

        private void initVariables()
        {
            try
            {
                dtpcDate.Format = DateTimePickerFormat.Custom;
                dtpcDate.CustomFormat = "dd-MM-yyyy";
                dtpcDate.Enabled = true;
                grdList.Visible = false ;
                btnNew.Visible = false;
                //documentStatusValues = new string[2, 2]
                //        {
                //    {"1","Active" },
                //    {"0","Dective" }
                //        };
                //fillDocumentStatusCombo(cmbUserStatus);

            }
            catch (Exception)
            {

            }

        }
        private string getStatusString(int stat)
        {
            string str = "Unkown";
            try
            {
                if (stat == 0)
                {
                    str = "Deactive";
                }
                else
                    str = "Active";
            }
            catch (Exception)
            {
                return str;
            }
            return str;
        }
        private int getstatuscode(string str)
        {
            int i = 0;
            try
            {
                if (str == "Active")
                {
                    i = 1;
                }
                else
                {
                    i = 0;
                }
            }
            catch ( Exception ex)
            {
                return i;
            }
            return i;
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
                //if (Main.itemPriv[2])
                //{
                //    grdList.Columns["Delete"].Visible = true;
                //}
                //else
                //{
                //    grdList.Columns["Delete"].Visible = false;
                //}
            }
            catch (Exception)
            {
            }
        }

        private void closeAllPanels()
        {
            try
            {
                pnlDocumentInner.Visible = false;
                pnlDocumentOuter.Visible = false;
                pnlMainList.Visible = false;
            }
            catch (Exception)
            {

            }
        }


        private void fillDocumentStatusCombo(System.Windows.Forms.ComboBox cmb)
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
        private void fillIsReversibleCombo(System.Windows.Forms.ComboBox cmb)
        {
            try
            {
                cmb.Items.Clear();
                for (int i = 0; i < Main.YesNo.GetLength(0); i++)
                {
                    cmb.Items.Add(Main.YesNo[i, 1]);
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
                //closeAllPanels();
                clearDocumentData();
                enableBottomButtons();
                btnNew.Enabled = true;
                pnlDocumentOuter.Visible = false;
                pnlDocumentInner.Visible = false;
                //pnlMainList.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        public void clearDocumentData()
        {
            try
            {
                //txtEmpID.Text = "";
                dtpcDate.Value = DateTime.Now;
                removeControlsFromlvPanel();
                //cmbUserStatus.SelectedIndex = 0;
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
                //closeAllPanels();
                clearDocumentData();
                btnSave.Text = "Save";
                pnlDocumentOuter.Visible = true;
                pnlDocumentInner.Visible = true;
                btnsel.Visible = true;
                btnNew.Enabled = false;
                //txtEmpID.Enabled = true;
                //disableBottomButtons();
                //cmbUserStatus.SelectedIndex = 0;               
            }
            catch (Exception)
            {

            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                leavecompoff lco = new leavecompoff();
                LeaveCompOffEntryDB LcoDB = new LeaveCompOffEntryDB();
                lco.EmployeeID = txtEmpID.Text;
                lco.cdate = dtpcDate.Value.Date;                         
                //doc.Status = getstatuscode(cmbUserStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
               
                string btnText = btn.Text;
                {
                     if (btnText.Equals("Save"))
                    {
                        for (int i = 0; i < grdList.Rows.Count; i++)
                        {
                            if(lco.cdate.Date.ToString("dd-MM-yyyy")== grdList.Rows[i].Cells["Date"].Value.ToString())
                            {
                                MessageBox.Show("Date Already Exists!!!");
                                return;
                            }
                        }
                        if (lco.cdate.Year == DateTime.Now.Year || lco.cdate.Year == DateTime.Now.Year-1)
                        {
                            if (lco.cdate < DateTime.Now.Date)
                            {
                                if (LcoDB.insertLeaveCompOff(lco))
                                {
                                    MessageBox.Show("CompOff data Added");
                                    closeAllPanels();
                                    ListDocument();

                                }
                                else
                                {
                                    MessageBox.Show("Failed to Insert CompOff");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Please Check the date!!!!");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please Check the date!!!!");
                        }
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing User Data");
            }
            btnNew.Enabled = true;
        }
        private void disableBottomButtons()
        {
            btnNew.Visible = false;
            btnExit.Visible = false;
        }
        private void enableBottomButtons()
        {
            //btnNew.Visible = true;
            btnExit.Visible = true;
        }

        private void btnsel_Click(object sender, EventArgs e)
        {
            //btnsel.Enabled = false;
            removeControlsFromlvPanel();
            pnlDocumentInner.Visible = false;
            pnlDocumentOuter.Visible = false;
            clearDocumentData();
            btnNew.Visible = false;
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;
            pnllv.Location = new Point(34, 68);
            pnllv.Size = new Size(466, 245);

            lv = EmployeePostingDB.getEmployeeListView();
            lv.Sorting = System.Windows.Forms.SortOrder.None;
            lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            lv.Location = new Point(13, 9);
            lv.Size = new Size(440, 199);
            //lv.ListViewItemSorter = new ListViewItemComparer();
            pnllv.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Location = new Point(20, 215);
            lvOK.Click += new System.EventHandler(this.lvOK_Click1);
            pnllv.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Location = new Point(100, 215);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
            pnllv.Controls.Add(lvCancel);

            Label lblSearch = new Label();
            lblSearch.Text = "Find";
            lblSearch.Location = new Point(285, 218);
            lblSearch.Size = new Size(35, 15);
            pnllv.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Location = new Point(320, 215);
            txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            pnllv.Controls.Add(txtSearch);

            pnlUI.Controls.Add(pnllv);
            pnllv.BringToFront();
            pnllv.Visible = true;
        }
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                //btnsel.Enabled = true;
                //pnllv.Visible = false;
                if (lv.Visible == true)
                {
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                            count++;
                    }
                    if (count != 1)
                    {
                        MessageBox.Show("Select one Employee");
                        return;
                    }
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            txtEmpID.Text = itemRow.SubItems[1].Text;
                            lblEmpName.Text = itemRow.SubItems[2].Text;
                            //docdata = getemp(txtDocumentID.Text);           
                        }
                    }
                }
                else
                {
                    foreach (ListViewItem itemRow in lvCopy.Items)
                    {
                        if (itemRow.Checked)
                            count++;
                    }
                    if (count != 1)
                    {
                        MessageBox.Show("Select one Employee");
                        return;
                    }
                    foreach (ListViewItem itemRow in lvCopy.Items)
                    {
                        if (itemRow.Checked)
                        {
                            txtEmpID.Text = itemRow.SubItems[1].Text;
                            lblEmpName.Text = itemRow.SubItems[2].Text;
                            //docdata = getemp(txtDocumentID.Text);           
                        }
                    }
                }

                ListDocument();
                pnllv.Visible = false;
                btnNew.Enabled = true;
                grdList.Visible = true;
              
            }
            catch (Exception ex)
            {
            }
        }

        public string statusvalue(int st)
        {
            string stat = "";
            if( st==1)
            {
                stat = "Created";
            }
            if(st == 2)
            {
                stat = "Availed";
            }
            if(st==4)
            {
                stat = "Deleted";
            }
            return stat;
        }

        private void lvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                //btnsel.Enabled = true;
                pnllv.Visible = false;
                if(lblEmpName.Text.Trim().Length!=0 && grdList.Rows.Count<3)
                {
                    btnNew.Visible = true;
                    btnNew.Enabled = true;
                }
            }
            catch (Exception)
            {
            }
        }
        //private void LvColumnClick(object o, ColumnClickEventArgs e)
        //{
        //    try
        //    {
        //        string first = lv.Items[0].SubItems[e.Column].Text;
        //        string last = lv.Items[lv.Items.Count - 1].SubItems[e.Column].Text;
        //        System.Windows.Forms.SortOrder sort_order1 = SortingListView.getSortedOrder(first, last);
        //        this.lv.ListViewItemSorter = new SortingListView(e.Column, sort_order1);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Sorting error");
        //    }
        //}

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            pnllv.Controls.Remove(lvCopy);
            addItems();
        }
        private void addItems()
        {
            try
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
                lvCopy.Columns.Add("Office Id", -2, HorizontalAlignment.Left);
                lvCopy.Columns.Add("Office Name", -2, HorizontalAlignment.Left);
                lvCopy.Columns[3].Width = 0;
                lvCopy.Columns[4].Width = 0;
                //lvCopy.Columns.Add("Select", -2, HorizontalAlignment.Left);
                //lvCopy.Columns.Add("BankAccountCode", -2, HorizontalAlignment.Left);
                //lvCopy.Columns.Add("BankAccountName", -2, HorizontalAlignment.Left);
                lvCopy.Sorting = System.Windows.Forms.SortOrder.None;
                lvCopy.ColumnClick += new ColumnClickEventHandler(LvColumnClick); ;
                //this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
                lvCopy.Bounds = new Rectangle(new Point(13, 9), new Size(440, 199));
                lvCopy.Items.Clear();
                foreach (ListViewItem row in lv.Items)
                {
                    string x = row.SubItems[0].Text;
                    string no = row.SubItems[1].Text;
                    string ch = row.SubItems[2].Text;

                    if (ch.ToLower().StartsWith(txtSearch.Text))
                    {
                        ListViewItem item = new ListViewItem();
                        item.SubItems.Add(no);
                        item.SubItems.Add(ch);
                        item.Checked = false;
                        lvCopy.Items.Add(item);
                    }
                }
                lv.Visible = false;
                lvCopy.Visible = true;
                pnllv.Controls.Add(lvCopy);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
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
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        //    }
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
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
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
        public string getuserId(string empID)
        {
            string empid = DocumentUCDB.getUserID(empID);
            return empid;
        }

        private void pnlDocumentList_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlDocumentOuter_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbDocumentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Delete"))
                {
                    if(grdList.Rows[e.RowIndex].Cells["empStatus"].Value.ToString() != "Availed")
                    {
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the CompOff Entered ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            LeaveCompOffEntryDB lcdb = new LeaveCompOffEntryDB();
                            int rowid = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["RowID"].Value);
                            if (lcdb.DeleteLeaveCompOff(rowid))
                            {
                                MessageBox.Show("CompOff Deleted Successfully");
                                ListDocument();
                            }
                            else
                            {
                                MessageBox.Show("Error Deleting CompOff!!!");
                            }
                            //disableBottomButtons();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Leave has Been Availed!!! ");
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void pnlDocumentInner_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LeaveCompOffEntry_Enter(object sender, EventArgs e)
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


