using System;
using System.Collections.Generic;
using System.Collections;
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
using System.Collections.ObjectModel;
using CSLERP.FileManager;

namespace CSLERP
{
    public partial class ProductGroupMapping : System.Windows.Forms.Form
    {
        string docID = "STOCKGROUP";
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        stockgroup prevsg;
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        static int lvl = 0;
        int no;
        Form frmPopup = new Form();
        ListView lv = new ListView();
        string grpCode = "";
        public ProductGroupMapping()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void StockGroup_Load(object sender, EventArgs e)
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
            ShowPannel();
            applyPrivilege();
            btnNew.Visible = false;
        }
        private void ShowPannel()
        {
            pnlList.Visible = true;
            lvlSelect.Visible = true;
            cmbSelectLevel.Visible = true;
        }
        private void listStockGroup(int lvl)
        {
            try
            {
                grdList.Rows.Clear();
                StockGroupDB sgdb = new StockGroupDB();
                List<stockgroup> sgroup = sgdb.getStockGroupDetails(lvl);
                int i = 1;
                foreach (stockgroup sg in sgroup)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["LineNo"].Value = i;
                    grdList.Rows[grdList.RowCount - 1].Cells["GroupCode"].Value = sg.GroupCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["GroupDescription"].Value = sg.GroupDescription;
                    //grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = sg.CreateTime;
                    ///grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = sg.CreateUser;
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Stock Group  Listing");
            }
            try
            {
                enableBottomButtons();
                pnlList.Visible = true;
                //btnNew.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void initVariables()
        {

            docID = Main.currentDocument;

            pnlUI.Controls.Add(pnlList);
            cmbSelectLevel.Items.Add("1");
            cmbSelectLevel.Items.Add("2");
            cmbSelectLevel.Items.Add("3");
            cmbSelectLevel.Items.Add("4");
            cmbSelectLevel.Items.Add("5");
            //cmbSelectLevel.Items.Add("6");

            enableBottomButtons();
            btnNew.Visible = false;
            //btnAddNew.Visible = false;
            //pnlAddNew.Visible = false;
            grdList.Visible = true;
            grdList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
        private void cmbSelectLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                clearData();
                lvl = Convert.ToInt32(cmbSelectLevel.SelectedItem.ToString());
                if (lvl != 0)
                {
                    no = lvl;
                    grdList.Visible = true;
                    listStockGroup(lvl);
                }
                ///btnAddNew.Visible = true;
                
                //if (getuserPrivilegeStatus() == 1)
                //{
                //    btnAddNew.Visible = false;
                //}
                //else
                //{
                //    btnAddNew.Visible = true;
                //}
                ////lblLevel.Text = "Level " + lvl;
                ////pnlAddNew.Visible = false;
                //btnAddNew.Text = "Add New Group in Level " + lvl;
            }
            catch (Exception ex)
            {
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
        private void button1_Click(object sender, EventArgs e)
        {
            //clearData();
            closeAllPanels();
            pnlList.Visible = true;
            listStockGroup(lvl);
            //pnlBottomActions.Visible = true;
        }
        private void closeAllPanels()
        {
            try
            {
                pnlList.Visible = false;
                //pnlAddEdit.Visible = false;

            }
            catch (Exception)
            {

            }
        }

        public void clearData()
        {
            try
            {
                //txtGroupCode.Text = "";
                //txtGroupDescription.Text = "";
                grdList.Rows.Clear();
                prevsg = new stockgroup();
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
        private void enableBottomButtons()
        {
            btnNew.Visible = false;
            btnExit.Visible = true;
            pnlBottomButtons.Visible = true;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
        }
        public string getGroupCode()
        {
            string gc = "";

            StockGroupDB sdb = new StockGroupDB();
            List<stockgroup> LSGroup = sdb.getStockGroupDetails(lvl);
            SortedSet<string> set = new SortedSet<string>();
            try
            {
                foreach (stockgroup sg in LSGroup)
                {
                    set.Add(sg.GroupCode);
                }
                gc = set.Max;
            }
            catch (Exception ex)
            {
            }
            if (Convert.ToInt32(gc) == 0)
            {
                gc = "10";// group coe start with 10
            }
            return (Convert.ToInt32(gc) + 1).ToString();
        }
        private void pnlUI_Paint(object sender, PaintEventArgs e)
        {

        }
        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Edit"))
                    {
                        grpCode = "";
                        string gcode = grdList.Rows[e.RowIndex].Cells["GroupCode"].Value.ToString();
                        grpCode = gcode;
                        showGroupListView(gcode);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorting error");
            }
        }
        private void showGroupListView(string gcode)
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
                //frmPopup.Location = new Point(579, 78);
                int level = Convert.ToInt32(cmbSelectLevel.SelectedItem);
                Label pnlHeading = new Label();
                pnlHeading.Size = new Size(300, 20);
                pnlHeading.Location = new System.Drawing.Point(5, 5);
                pnlHeading.Text = "Group Codes Of Level " + (level + 1);
                pnlHeading.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                pnlHeading.ForeColor = Color.Black;
                frmPopup.Controls.Add(pnlHeading);

                lv = StockGroupDB.getListViewForStockGroup(level + 1);
                lv.Bounds = new Rectangle(new Point(0, 25), new Size(450, 250));
                lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
                string children = StockGroupDB.getChildrenOfAGroup(gcode, level);
                string[] childs = children.Split(Main.delimiter1);
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (Array.IndexOf(childs, itemRow.SubItems[1].Text) != -1)
                    {
                        itemRow.Checked = true;
                    }
                }
                frmPopup.Controls.Add(lv);

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new Point(40, 280);
                lvOK.Click += new System.EventHandler(this.lvOK_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.BackColor = Color.Tan;
                lvCancel.Text = "CANCEL";
                lvCancel.Location = new Point(130, 280);
                lvCancel.Click += new System.EventHandler(this.lvCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ListView Error");
            }
        }
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                //if(lv.CheckedIndices.Count == 0)
                //{
                //    MessageBox.Show("Select One Group Code");
                //    return;
                //}
                string children = "";
                StockGroupDB sgdb = new StockGroupDB();
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        children += itemRow.SubItems[1].Text + Main.delimiter1;
                    }
                }
                int level = Convert.ToInt32(cmbSelectLevel.SelectedItem);
                if (sgdb.updateProductGroupMapping(children,level,grpCode))
                {
                    MessageBox.Show("Product Group mapped Sucessfully");
                }
                else
                {
                    MessageBox.Show("Product Group mapping Failed");
                    return;
                }
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        private void lvCancel_Click1(object sender, EventArgs e)
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

        private void ProductGroupMapping_Enter(object sender, EventArgs e)
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


