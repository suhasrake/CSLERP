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
    public partial class MenuPrivilege : System.Windows.Forms.Form
    {
        private string userIDSelected = "";
        private string userMenuString = "";
        int val = 0;
        private Boolean userPrivilegeExists = false;
        Form frmPopup = new Form();
        ListView lv = new ListView();
        DataGridView grdMpList = new DataGridView();
        public MenuPrivilege()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }

        private void MenuPrivilege_Load(object sender, EventArgs e)
        {
            //////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            //initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;

            String a = this.Size.ToString();
            grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdList.EnableHeadersVisualStyles = false;
            btnShowChecked.Visible = false;
            ListUser();
            ////applyPrivilege();
        }
        private void ListUser()
        {
            try
            {
                grdList.Rows.Clear();
                ERPUserDB dbrecord = new ERPUserDB();
                List<user> Users = dbrecord.getUsers();
                foreach (user usr in Users)
                {
                    if (usr.userStatus == 1)
                    {
                        grdList.Rows.Add(usr.userID, usr.userEmpName,
                        ComboFIll.getUserTypeString(usr.userType));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            enableBottomButtons();
            pnlMenuPrivilegeList.Visible = true;
        }
        private void ListMenuItems()
        {
            try
            {

                grdUserPrivileges.Rows.Clear();
                MenuItemDB dbrecord = new MenuItemDB();
                List<menuitem> menuitems = dbrecord.getMenuItems();
                foreach (menuitem menu in menuitems)
                {
                    if (menu.menuitemStatus == 1)
                    {
                        grdUserPrivileges.Rows.Add(menu.menuItemID, menu.description);
                    }
                }
                fillUserMenuPrivileges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            pnlMenuPrivilegeList.Visible = true;
        }
        ////private void applyPrivilege()
        ////{
        ////    try
        ////    {

        ////    }
        ////    catch (Exception)
        ////    {
        ////    }
        ////}
        private void fillUserMenuPrivileges()
        {
            try
            {
                MenuPrivilegeDB mpdb = new MenuPrivilegeDB();
                string mpString = mpdb.getUserMenuPrivilege(userIDSelected);
                if (mpString.Length > 0)
                {
                    userPrivilegeExists = true;
                }
                string[] strArr = mpString.Split(';');
                int intex = 0;
                foreach (DataGridViewRow row in grdUserPrivileges.Rows)
                {
                    try
                    {
                        string docID = row.Cells[0].Value.ToString();
                        intex = Utilities.checkMenuPrivilege(docID, strArr);
                        if (intex >= 0)
                        {
                            string[] prvArr = strArr[intex].Split(',');
                            if (prvArr[1].Equals("V"))
                            {
                                row.Cells[2].Value = "true";
                            }
                            if (prvArr[2].Equals("A"))
                            {
                                row.Cells[3].Value = "true";
                            }
                            if (prvArr[3].Equals("E"))
                            {
                                row.Cells[4].Value = "true";
                            }
                            if (prvArr[4].Equals("D"))
                            {
                                row.Cells[5].Value = "true";
                            }
                            try
                            {
                                if (prvArr[5].Equals("M"))
                                {
                                    row.Cells[6].Value = "true";
                                }
                            }
                            catch (Exception ex)
                            {
                                row.Cells[6].Value = "false";
                            }
                            //DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[2];
                            //chk.Selected = true;
                        }
                        else
                        {
                            row.Cells[2].Value = "false";
                            row.Cells[3].Value = "false";
                            row.Cells[4].Value = "false";
                            row.Cells[5].Value = "false";
                            row.Cells[6].Value = "false";
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception)
            {
            }

        }


        private void closeAllPanels()
        {
            pnlMenuPrivilegeList.Visible = false;
        }

        public void clearUserData()
        {

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
            }
            catch (Exception)
            {

            }
        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == 3)
                {
                    grdList.Rows[e.RowIndex].Selected = true;
                    userPrivilegeExists = false;
                    userIDSelected = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    ListMenuItems();
                    val = 0;
                    btnShowChecked.Text = "Show Checked Rows";
                    grdUserPrivileges.Visible = true;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    grdUserPrivileges.Visible = true;
                    btnShowChecked.Visible = true;
                    disableBottomButtons();
                }
            }
            catch (Exception)
            {
            }
        }

        private void brnSave_Click(object sender, EventArgs e)
        {
            Boolean status = false;
            userMenuString = "";
            try
            {
                MenuPrivilegeDB mpDB = new MenuPrivilegeDB();
                usermenuprivilege ump = new usermenuprivilege();
                ump.userID = userIDSelected;
                foreach (DataGridViewRow row in grdUserPrivileges.Rows)
                {
                    ////if (Convert.ToBoolean(row.Cells[2].Value) == true)
                    {
                        userMenuString = userMenuString + row.Cells[0].Value.ToString();
                        if (Convert.ToBoolean(row.Cells[2].Value) == true)
                        {
                            userMenuString = userMenuString + ",V";
                        }
                        else
                        {
                            userMenuString = userMenuString + ",";
                        }

                        if (Convert.ToBoolean(row.Cells[3].Value) == true)
                        {
                            userMenuString = userMenuString + ",A";
                        }
                        else
                        {
                            userMenuString = userMenuString + ",";
                        }
                        if (Convert.ToBoolean(row.Cells[4].Value) == true)
                        {
                            userMenuString = userMenuString + ",E";
                        }
                        else
                        {
                            userMenuString = userMenuString + ",";
                        }
                        if (Convert.ToBoolean(row.Cells[5].Value) == true)
                        {
                            userMenuString = userMenuString + ",D";
                        }
                        else
                        {
                            userMenuString = userMenuString + ",";
                        }
                        if (Convert.ToBoolean(row.Cells[6].Value) == true)
                        {
                            userMenuString = userMenuString + ",M";
                        }
                        else
                        {
                            userMenuString = userMenuString + ",";
                        }
                        userMenuString = userMenuString + ";";
                    }
                }
                ump.menuItemString = userMenuString;
                if (userPrivilegeExists)
                {
                    //update
                    if (mpDB.updateUserMenuPrivilege(ump))
                    {
                        MessageBox.Show("User Menu Privileges updated");
                        status = true;
                    }
                    else
                    {
                        MessageBox.Show("Failed to update User Menu Privileges");
                        status = false;
                    }
                }
                else
                {
                    //add
                    mpDB.deleteUserMenuPrivilege(userIDSelected);
                   
                    if (mpDB.insertUserMenuPrivilege(ump))
                    {
                        MessageBox.Show("User Menu Privileges Added");
                        status = true;
                    }
                    else
                    {
                        MessageBox.Show("Failed to Add User Menu Privileges");
                        status = false;
                    }
                }
                if (status)
                {
                    closegrdUserPrivileges();
                }
                enableBottomButtons();
            }
            catch (Exception)
            {

                MessageBox.Show("Error updating User Menu Privileges");
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            closegrdUserPrivileges();
            enableBottomButtons();
        }
        private void closegrdUserPrivileges()
        {
            try
            {
                btnSave.Visible = false;
                btnCancel.Visible = false;
                grdUserPrivileges.Visible = false;
                btnShowChecked.Visible = false;
                enableBottomButtons();
            }
            catch (Exception)
            {
            }
        }
        private void disableBottomButtons()
        {
            btnExit.Visible = false;
        }
        private void enableBottomButtons()
        {
            btnExit.Visible = true;
        }
        private void filterGridData()
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
                        if (!row.Cells["empName"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
                        {
                            row.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            filterGridData();
        }

        private void MenuPrivilege_Enter(object sender, EventArgs e)
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

        private void btnShowChecked_Click(object sender, EventArgs e)
        {

            try
            {
                if(val==0)
                {
                    val = 1;
                    btnShowChecked.Text = "Show All rows ";
                    foreach (DataGridViewRow row in grdUserPrivileges.Rows)
                    {
                        row.Visible = false;
                        foreach (DataGridViewColumn col in grdUserPrivileges.Columns)
                        {
                            if (col.Name != "MenuID")
                            {
                                if (col.Name != "MenuDescription")
                                {
                                    bool isSelected = Convert.ToBoolean(row.Cells[col.Name].Value);
                                    if (isSelected)
                                    {
                                        row.Visible = true;
                                        break;
                                    }
                                }

                            }

                        }

                    }
                }
               else if(val==1)
                {
                    val = 0;
                    btnShowChecked.Text = "Show checked Rows";
                    foreach (DataGridViewRow row in grdUserPrivileges.Rows)
                    {
                        row.Visible = true;
                    }
                }
               
            }
            catch (Exception ex)
            {
            }
        }

        private void grdUserPrivileges_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==0)
            {
                try
                {
                    string docid = grdUserPrivileges.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string docname= grdUserPrivileges.Rows[e.RowIndex].Cells[1].Value.ToString();
                    frmPopup = new Form();
                    frmPopup.StartPosition = FormStartPosition.CenterScreen;
                    frmPopup.BackColor = Color.CadetBlue;
                    frmPopup.MaximizeBox = false;
                    frmPopup.MinimizeBox = false;
                    frmPopup.ControlBox = false;
                    frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;
                    frmPopup.Size = new Size(550, 320);

                    Label lblDoc = new Label();
                    lblDoc.BackColor = Color.CadetBlue;
                    lblDoc.Text = docname;
                    lblDoc.Location = new Point(5, 5);
                    frmPopup.Controls.Add(lblDoc);

                    grdMpList = MenuPrivilegeDB.PrivilageListView(docid);
                    grdMpList.Bounds = new Rectangle(new Point(0, 30), new Size(550, 250));
                    grdMpList.Columns["EmployeeName"].Width = 180;
                    grdMpList.Columns["EmployeeID"].Width = 100;
                    grdMpList.ReadOnly = true;
                    foreach (DataGridViewRow row in grdMpList.Rows)
                    {
                        row.Visible = false;
                        foreach (DataGridViewColumn col in grdMpList.Columns)
                        {
                            if (col.Name != "EmployeeID")
                            {
                                if (col.Name != "EmployeeName")
                                {
                                    bool isSelected = Convert.ToBoolean(row.Cells[col.Name].Value);
                                    if (isSelected)
                                    {
                                        row.Visible = true;
                                        break;
                                    }
                                }

                            }

                        }

                    }


                    frmPopup.Controls.Add(grdMpList);



                    Button lvCancel = new Button();
                    lvCancel.BackColor = Color.Tan;
                    lvCancel.Text = "CANCEL";
                    lvCancel.Location = new Point(5, 290);
                    lvCancel.Click += new System.EventHandler(this.lvCancel_Clicked);
                    frmPopup.Controls.Add(lvCancel);
                    frmPopup.ShowDialog();


                    

                }
                catch(Exception ex)
                {

                }
            }
        }

        private void lvCancel_Clicked(object sender, EventArgs e)
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

    }
}

