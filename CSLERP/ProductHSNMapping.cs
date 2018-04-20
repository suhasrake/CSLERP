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
    public partial class ProductHSNMapping : System.Windows.Forms.Form
    {
        Form frmPopup = new Form();
        ListView lv = new ListView();
        ListView lvCopy = new ListView();
        TextBox txtSearch = new TextBox();
        HSNMapping prevmap;
        public ProductHSNMapping()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void Region_Load(object sender, EventArgs e)
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
            ListHSNMApping();
            applyPrivilege();
        }
        private void ListHSNMApping()
        {
            try
            {
                grdList.Rows.Clear();
                ProductHSNMappingDB mapDB = new ProductHSNMappingDB();
                List<HSNMapping> mapList = mapDB.getHSNMappingList();
                foreach (HSNMapping map in mapList)
                {
                    //grdList.Rows.Add(map.StockItemID, map.StockItemName,map.ModelNo,map.ModelName, map.HSNCode,
                    //     getStatusString(map.Status), map.CreateTime, map.CreateUser);
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["RowID"].Value = map.RowID;
                    grdList.Rows[grdList.RowCount - 1].Cells["StockItemID"].Value = map.StockItemID;
                    grdList.Rows[grdList.RowCount - 1].Cells["StockItemName"].Value = map.StockItemName;
                    if(map.ModelNo.Trim().Length == 0 || map.ModelNo == null)
                        grdList.Rows[grdList.RowCount - 1].Cells["ModelNo"].Value = "NA";
                    else
                        grdList.Rows[grdList.RowCount - 1].Cells["ModelNo"].Value = map.ModelNo;
                    if (map.ModelName.Trim().Length == 0 || map.ModelName == null)
                        grdList.Rows[grdList.RowCount - 1].Cells["ModelName"].Value = "NA";
                    else
                        grdList.Rows[grdList.RowCount - 1].Cells["ModelName"].Value = map.ModelName;
                    grdList.Rows[grdList.RowCount - 1].Cells["HSNCode"].Value = map.HSNCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = getStatusString(map.Status);
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = map.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = map.CreateUser;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in HSNMapping listing");
            }
            enableBottomButtons();
            pnlHSNList.Visible = true;
        }

        private void initVariables()
        {

            cmbStatus.SelectedIndex = 0;
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

        private void closeAllPanels()
        {
            try
            {
                pnlHSNInner.Visible = false;
                pnlHSNOuter.Visible = false;
                pnlHSNList.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                clearUserData();
                enableBottomButtons();
                pnlHSNList.Visible = true;
            }
            catch (Exception)
            {

            }
        }
        public void clearUserData()
        {
            try
            {
                txtStockItemID.Text = "";
                txtStockITemName.Text = "";
                txtModelNo.Text = "";
                txtModelName.Text = "";
                txtHSNCode.Text = "";
                cmbStatus.SelectedIndex = 0;
                prevmap = new HSNMapping();
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
                clearUserData();
                btnSave.Text = "Save";
                pnlHSNOuter.Visible = true;
                pnlHSNInner.Visible = true;
                btnSelectStock.Enabled = true;

                //txtStateName.Enabled = true;
                //cmbStatus.Enabled = true;
                disableBottomButtons();
            }
            catch (Exception)
            {

            }
        }
        private int getStatusCode(string stat)
        {
            int code = 0;
            if (stat.Equals("Active"))
                code = 1;
            else
                code = 0;
            return code;
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                HSNMapping map = new HSNMapping();
                ProductHSNMappingDB mapDB = new ProductHSNMappingDB();
                if(txtHSNCode.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Fill HSN COde");
                    return;
                }
                map.StockItemID = txtStockItemID.Text.Trim() ;
                
                map.ModelNo = txtModelNo.Text.Trim();
                map.HSNCode = txtHSNCode.Text.Trim();
                map.Status = getStatusCode(cmbStatus.SelectedItem.ToString());
                map.RowID = prevmap.RowID;
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (mapDB.validateHSNMapping(map))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (mapDB.updateHSNCode(map))
                        {
                            MessageBox.Show("HSNCode updated");
                            closeAllPanels();
                            ListHSNMApping();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update HSNCode");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {

                        if (mapDB.insertHSNCOde(map))
                        {
                            MessageBox.Show("HSNCode Added");
                            closeAllPanels();
                            ListHSNMApping();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert HSNCode");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("HSNCode Data Validation failed");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing HSNCode");
            }
        }
        private string getStatusString(int code)
        {
            string str = "";
            if (code == 1)
                str = "Active";
            else
                str = "Deactive";
            return str;
        }
        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string colName = grdList.Columns[e.ColumnIndex].Name;
                if (e.RowIndex < 0)
                    return;
                if (colName.Equals("Edit"))
                {
                    int rowID = e.RowIndex;
                    clearUserData();
                    btnSave.Text = "Update";
                    pnlHSNInner.Visible = true;
                    pnlHSNOuter.Visible = true;
                    pnlHSNList.Visible = false;
                    btnSelectStock.Enabled = false;
                    string code = grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString();
                    cmbStatus.SelectedIndex = cmbStatus.FindString(code);
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevmap.RowID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["RowID"].Value);
                    prevmap.StockItemID = grdList.Rows[e.RowIndex].Cells["StockItemID"].Value.ToString();
                    prevmap.StockItemName = grdList.Rows[e.RowIndex].Cells["StockItemName"].Value.ToString();
                    prevmap.ModelNo = grdList.Rows[e.RowIndex].Cells["ModelNo"].Value.ToString();
                    prevmap.ModelName = grdList.Rows[e.RowIndex].Cells["ModelName"].Value.ToString();
                    prevmap.HSNCode = grdList.Rows[e.RowIndex].Cells["HSNCode"].Value.ToString();

                    txtStockItemID.Text = grdList.Rows[e.RowIndex].Cells["StockItemID"].Value.ToString();
                    txtStockITemName.Text = grdList.Rows[e.RowIndex].Cells["StockItemName"].Value.ToString();
                    txtModelNo.Text = grdList.Rows[e.RowIndex].Cells["ModelNo"].Value.ToString();
                    txtModelName.Text = grdList.Rows[e.RowIndex].Cells["ModelName"].Value.ToString();
                    txtHSNCode.Text = grdList.Rows[e.RowIndex].Cells["HSNCode"].Value.ToString();

                    disableBottomButtons();
                }
            }
            catch (Exception ex)
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

        private void btnSelectStock_Click(object sender, EventArgs e)
        {
            try
            {
                ShowStockListView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception on showing ListView");
            }
        }

        private void ShowStockListView()
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(550, 310);
            lv = StockItemDB.getStockItemListView();
            lv.ShowItemToolTips = true;
            lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView3_ItemChecked);
            lv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 25), new Size(550, 250));
            frmPopup.Controls.Add(lv);

            System.Windows.Forms.Label pnlHeading = new System.Windows.Forms.Label();
            pnlHeading.Size = new Size(300, 20);
            pnlHeading.Location = new System.Drawing.Point(5, 5);
            pnlHeading.Text = "Select StockItem";
            pnlHeading.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            pnlHeading.ForeColor = Color.Black;
            frmPopup.Controls.Add(pnlHeading);

            System.Windows.Forms.Button lvOK = new System.Windows.Forms.Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 280);
            lvOK.Click += new System.EventHandler(this.lvOK_Click2);
            frmPopup.Controls.Add(lvOK);

            System.Windows.Forms.Button lvCancel = new System.Windows.Forms.Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new Point(130, 280);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
            frmPopup.Controls.Add(lvCancel);

            System.Windows.Forms.Label lblSearch = new System.Windows.Forms.Label();
            lblSearch.Text = "Search";
            lblSearch.Location = new Point(260, 282);
            lblSearch.Size = new Size(45, 15);
            frmPopup.Controls.Add(lblSearch);

            txtSearch = new System.Windows.Forms.TextBox();
            txtSearch.Location = new Point(310, 280);
            txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
            frmPopup.Controls.Add(txtSearch);

            frmPopup.ShowDialog();
        }
        private void lvOK_Click2(object sender, EventArgs e)
        {
            try
            {
                if (lv.Visible == true)
                {
                    if (!checkLVItemChecked("Item"))
                    {
                        return;
                    }
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            txtStockItemID.Text = itemRow.SubItems[1].Text;
                            txtStockITemName.Text = itemRow.SubItems[2].Text;
                            frmPopup.Close();
                            frmPopup.Dispose();
                            showModelListView(itemRow.SubItems[1].Text);
                        }
                    }
                }
                else
                {
                    if (!checkLVCopyItemChecked("Item"))
                    {
                        return;
                    }
                    foreach (ListViewItem itemRow in lvCopy.Items)
                    {
                        if (itemRow.Checked)
                        {
                            txtStockItemID.Text = itemRow.SubItems[1].Text;
                            txtStockITemName.Text = itemRow.SubItems[2].Text;
                            frmPopup.Close();
                            frmPopup.Dispose();
                            showModelListView(itemRow.SubItems[1].Text);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void lvCancel_Click2(object sender, EventArgs e)
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
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            frmPopup.Controls.Remove(lvCopy);
            addItems();
        }
        private void addItems()
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
            lvCopy.Columns.Add("StockItem ID", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("StockItem Name", -2, HorizontalAlignment.Left);
            lvCopy.Sorting = System.Windows.Forms.SortOrder.None;
            lvCopy.ColumnClick += new ColumnClickEventHandler(LvColumnClick); ;
            //this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
            lvCopy.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 25), new Size(550, 250));
            lvCopy.Items.Clear();
            foreach (ListViewItem row in lv.Items)
            {
                string x = row.SubItems[0].Text;
                string no = row.SubItems[1].Text;
                string ch = row.SubItems[2].Text;
                if (ch.ToLower().StartsWith(txtSearch.Text.ToLower()))
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
            frmPopup.Controls.Add(lvCopy);
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
        private void showModelListView(string stockID)
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(550, 310);
            Label lbl = new Label();
            lbl.AutoSize = true;
            lbl.Location = new Point(5, 5);
            lbl.Size = new Size(300, 20);
            lbl.Text = "ListView For Model";
            lbl.Font = new Font("Serif", 10, FontStyle.Bold);
            lbl.ForeColor = Color.Black;
            frmPopup.Controls.Add(lbl);
            lv = ProductModelsDB.getModelsForProductListView(stockID);
            if (lv.Items.Count == 0)
            {
                txtModelNo.Text = "NA";
                txtModelName.Text = "NA";
                return;
            }
            lv.Bounds = new Rectangle(new System.Drawing.Point(0, 25), new Size(550, 250));
            frmPopup.Controls.Add(lv);
            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.BackColor = Color.Tan;
            lvOK.Location = new System.Drawing.Point(40, 280);
            lvOK.Click += new System.EventHandler(this.lvOK_Click3);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new System.Drawing.Point(130, 280);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click1);
            frmPopup.Controls.Add(lvCancel);

            frmPopup.ShowDialog();
        }
        private void lvOK_Click3(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                foreach (ListViewItem item in lv.Items)
                {
                    if (item.Checked == true)
                    {
                        count++;
                    }
                }
                if (count != 1)
                {
                    MessageBox.Show("select one item");
                    return;
                }
                foreach (ListViewItem item in lv.Items)
                {
                    if (item.Checked == true)
                    {
                        txtModelNo.Text = item.SubItems[1].Text;
                        txtModelName.Text = item.SubItems[2].Text;
                        frmPopup.Close();
                        frmPopup.Dispose();
                    }
                }
            }
            catch (Exception)
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

        private void ProductHSNMapping_Enter(object sender, EventArgs e)
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

