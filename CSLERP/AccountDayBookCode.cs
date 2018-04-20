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
    public partial class AccountDayBookCode : System.Windows.Forms.Form
    {
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        ListView lvCopy = new ListView();
        TextBox txtSearch = new TextBox();
        Form frmPopup = new Form();
        public AccountDayBookCode()
        {

            try
            {
                InitializeComponent();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void AccountDayBookCode_Load(object sender, EventArgs e)
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
            ListAccountDayBookCode();
            //applyPrivilege();
        }
        private void ListAccountDayBookCode()
        {
            try
            {
                grdList.Rows.Clear();
                AccountDayBookCodeDB ADBdb = new AccountDayBookCodeDB();
                List<accountdaybook> ADBList = ADBdb.getAccountDayBookDetail();
                foreach (accountdaybook adb in ADBList)
                {
                    grdList.Rows.Add(adb.AccountCode, adb.Name,adb.BookType, adb.currencyID,
                         getStatusString(adb.status),adb.CreateTime,adb.CreateUser);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            //enableBottomButtons();
            setButtonVisibility("init");
            pnlAccDayBookCode.Visible = true;
        }
        private string getStatusString(int stat)
        {
            string status = "";
            if (stat == 1)
                status = "active";
            else
                status = "Deactive";
            return status;
        }
        private int getStatusCode(string status)
        {
            int code = 0;
            if (status.Equals("Active"))
                code = 1;
            else
                code = 0;
            return code;
        }
        private void initVariables()
        {
            try
            {
                //CatalogueValueDB.fillCatalogValueCombo(cmbBookType, "DayBookType");
                CatalogueValueDB.fillCatalogValueComboNew(cmbBookType, "DayBookType");
                CurrencyDB.fillCurrencyComboNew(cmbCurrency);
                cmbCurrency.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCurrency, "INR");
                setButtonVisibility("init");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void closeAllPanels()
        {
            try
            {
                pnlUserInner.Visible = false;
                pnlUserOuter.Visible = false;
                pnlAccDayBookCode.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                clearUserData();
                closeAllPanels();
                btnSelectAccountCode.Enabled = true;
                pnlAccDayBookCode.Visible = true;
                setButtonVisibility("btnEditPanel");
                removeControlsFrompnllvPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        public void clearUserData()
        {
            try
            {
                txtAccCode.Text = "";
                cmbBookType.SelectedIndex = -1;
                cmbBookStatus.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                pnlUI.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                clearUserData();
                closeAllPanels();
                btnSave.Text = "Save";
                pnlUserOuter.Visible = true;
                pnlUserInner.Visible = true;
                btnSelectAccountCode.Enabled = true; ;
                setButtonVisibility("btnNew");
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {
                accountdaybook adb = new accountdaybook();
                AccountDayBookCodeDB adbDB = new AccountDayBookCodeDB();
                //adb.BookType = cmbBookType.SelectedItem.ToString().Substring(0, cmbBookType.SelectedItem.ToString().IndexOf("-"));
                adb.BookType = ((Structures.ComboBoxItem)cmbBookType.SelectedItem).HiddenValue;
                adb.AccountCode = txtAccCode.Text.Substring(0,txtAccCode.Text.IndexOf('-'));
                adb.status = getStatusCode(cmbBookStatus.SelectedItem.ToString());
                adb.currencyID= ((Structures.ComboBoxItem)cmbCurrency.SelectedItem).HiddenValue;
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                btnText = btnSave.Text;

                if (btnText.Equals("Update"))
                {
                    if (adbDB.validateACcountDayBookDetail(adb))
                    {
                        if (adbDB.updateAccountDayBookDetail(adb))
                        {
                            MessageBox.Show("Book Type updated");
                            closeAllPanels();
                            ListAccountDayBookCode();
                        }
                        else
                        {
                            status = false;
                            MessageBox.Show("Failed to update Book Type");
                        }
                    }
                    else
                    {
                        status = false;
                        MessageBox.Show("Validation Failed");
                    }
                }
                else if (btnText.Equals("Save"))
                {
                    if (adbDB.validateACcountDayBookDetail(adb))
                    {
                        if (adbDB.insertAccountDayBookDetail(adb))
                        {
                            MessageBox.Show("Book Type added");
                            closeAllPanels();
                            ListAccountDayBookCode();
                        }
                        else
                        {
                            status = false;
                            MessageBox.Show("Failed to insert Book Type");
                            //return;
                        }
                    }
                    else
                    {
                        status = false;
                        MessageBox.Show(" Validation failed");
                    }
                }

            }
            catch (Exception ex)
            {
                status = false;
                MessageBox.Show("Failed Adding / Editing User Data");
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == 7)
                {
                    btnSelectAccountCode.Enabled = false;
                    setButtonVisibility("Edit");
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlUserInner.Visible = true;
                    pnlUserOuter.Visible = true;
                    pnlAccDayBookCode.Visible = false;
                    string bookType = grdList.Rows[e.RowIndex].Cells["BookType"].Value.ToString();
                    string currency= grdList.Rows[e.RowIndex].Cells["CurrencyID"].Value.ToString();
                    cmbBookType.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbBookType, bookType);
                    cmbCurrency.SelectedIndex= Structures.ComboFUnctions.getComboIndex(cmbCurrency, currency);
                    cmbBookStatus.SelectedIndex = cmbBookStatus.FindString(grdList.Rows[e.RowIndex].Cells["empStatus"].Value.ToString());
                    DataGridViewRow row = grdList.Rows[rowID];
                    txtAccCode.Text = grdList.Rows[e.RowIndex].Cells["AccountCode"].Value.ToString() + "-" +
                                        grdList.Rows[e.RowIndex].Cells["AccountName"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void setButtonVisibility(string btnName)
        {
            try
            {
                btnNew.Visible = false;
                btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                //----24/11/2016
                handleNewButton();
                handleGrdViewButton();
                handleGrdEditButton();
                pnlBottomButtons.Visible = true;
                //----
                if (btnName == "init")
                {
                    ////btnNew.Visible = true; 24/11/2016
                    btnExit.Visible = true;
                }
                else if (btnName == "btnNew")
                {
                    btnNew.Visible = false; //added 24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (btnName == "btnEditPanel") //called from cancel,save,forward,approve and reverse button events
                {
                    ////btnNew.Visible = true;
                    btnExit.Visible = true;
                }
                //gridview buttons
                else if (btnName == "Edit")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                //if the user privilege is only view, show only the Approved documents button
                int ups = getuserPrivilegeStatus();
                if (ups == 1 || ups == 0)
                {
                    grdList.Columns["Edit"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        void handleNewButton()
        {
            btnNew.Visible = false;
            if (Main.itemPriv[1])
            {
                btnNew.Visible = true;
            }
        }
        void handleGrdEditButton()
        {
            grdList.Columns["Edit"].Visible = false;
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                grdList.Columns["Edit"].Visible = true;
            }
        }

        void handleGrdViewButton()
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
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return 0;
        }
        private void removeControlsFrompnllvPanel()
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
        private void btnSelectAccountCode_Click(object sender, EventArgs e)
        {
            try
            {
                //pnllv = new Panel();
                lv = new ListView();
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.CadetBlue;
                //frmPopup.ShowIcon = false;
                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(450, 300);
                lv = AccountCodeDB.getAccountCodeListView();
                lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
                lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
                frmPopup.Controls.Add(lv);

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new Point(44, 270);
                lvOK.Click += new System.EventHandler(this.lvOK_Clicked);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.BackColor = Color.Tan;
                lvCancel.Text = "CANCEL";
                lvCancel.Location = new Point(141, 270);
                lvCancel.Click += new System.EventHandler(this.lvCancel_Clicked);
                frmPopup.Controls.Add(lvCancel);

                Label lblSearch = new Label();
                lblSearch.Text = "Find";
                lblSearch.Location = new Point(250, 272);
                lblSearch.Size = new Size(45, 15);
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Location = new Point(300, 270);
                txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
                frmPopup.Controls.Add(txtSearch);

                frmPopup.ShowDialog();
                //pnllv.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void lvOK_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (lv.Visible == true)
                {
                    if(lv.CheckedIndices.Count == 0)
                    {
                        MessageBox.Show("Select one Account.");
                        return;
                    }
                    if(lv.CheckedIndices.Count > 1)
                    {
                        MessageBox.Show("Not allowed to select more than one Account.");
                        return;
                    }
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            txtAccCode.Text = itemRow.SubItems[1].Text +"-"+ itemRow.SubItems[2].Text;
                        }
                    }
                }
                else
                {
                    if (lvCopy.CheckedIndices.Count == 0)
                    {
                        MessageBox.Show("Select one Account.");
                        return;
                    }
                    if (lvCopy.CheckedIndices.Count > 1)
                    {
                        MessageBox.Show("Not allowed to select more than one Account.");
                        return;
                    }
                    foreach (ListViewItem itemRow in lvCopy.Items)
                    {
                        if (itemRow.Checked)
                        {
                            txtAccCode.Text = itemRow.SubItems[1].Text + "-" + itemRow.SubItems[2].Text;
                        }
                    }
                }
                //pnllv.Visible = false;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private void lvCancel_Clicked(object sender, EventArgs e)
        {
            try
            {
                //btnSelectAccountCode.Enabled = true;
                //pnllv.Visible = false;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
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
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                frmPopup.Controls.Remove(lvCopy);
                addItems();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void addItems()
        {
            lvCopy = new ListView();
            lvCopy.View = View.Details;
            lvCopy.LabelEdit = true;
            lvCopy.AllowColumnReorder = true;
            lvCopy.CheckBoxes = true;
            lvCopy.FullRowSelect = true;
            lvCopy.GridLines = true;
            lvCopy.Sorting = System.Windows.Forms.SortOrder.Ascending;
            lvCopy.Columns.Add("Select", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Account Code", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Account Name", -2, HorizontalAlignment.Left);
            lvCopy.Sorting = System.Windows.Forms.SortOrder.None;
            lvCopy.ColumnClick += new ColumnClickEventHandler(LvColumnClick); ;
            //this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
            //lvCopy.Location = new Point(13, 9);
            //lvCopy.Size = new Size(440, 199);
            //lvCopy.Bounds = new Rectangle(new Point(17, 14), new Size(443, 212));
            lvCopy.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
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
            frmPopup.Controls.Add(lvCopy);
        }

        private void AccountDayBookCode_Enter(object sender, EventArgs e)
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

