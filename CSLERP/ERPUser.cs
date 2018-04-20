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
    public partial class ERPUser : System.Windows.Forms.Form
    {

        public ERPUser()
        {
            try
            {
                InitializeComponent();
              
            }
            catch (Exception)
            {

            }
        }

        private void ERPUser_Load(object sender, EventArgs e)
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
            ListUser();
            applyPrivilege();
        }
        private void ListUser()
        {
            try
            {
                grdList.Rows.Clear();
                txtSearch.Text = "";
                ERPUserDB dbrecord = new ERPUserDB();
                List<user> Users = dbrecord.getUsers();
                if (Users.Count == 0)
                {
                    //
                }
                foreach (user usr in Users)
                {
                    grdList.Rows.Add(usr.userID, usr.userEmpName,
                         ComboFIll.getUserTypeString(usr.userType),
                         ComboFIll.getStatusString(usr.userStatus));
                    //dbrecord.getEmpStatusString(emp.empStatus), emp.empPhoto);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            enableBottomButtons();
            pnlUserList.Visible = true;
        }

        private void initVariables()
        {
            try
            {

                fillUserStatusCombo(cmbUserStatus);
                fillUserTypeCombo(cmbUserType);
                EmployeeDB.fillEmpListCombo(cmbEmpName);
                txtEmpName.Visible = false;
                cmbEmpName.Visible = false;
                btnResetPassword.Visible = false;
                if (Main.itemPriv[1])
                {
                    btnNew.Visible = true;
                }
            }
            catch (Exception)
            {

            }
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
                pnlUserInner.Visible = false;
                pnlUserOuter.Visible = false;
                pnlUserList.Visible = false;
            }
            catch (Exception)
            {

            }
        }
        private void openAllPanels()
        {
            try
            {
                pnlUserInner.Visible = true;
                pnlUserOuter.Visible = true;
                pnlUserList.Visible = true;
              
            }
            catch (Exception)
            {

            }
        }


        private void fillUserStatusCombo(System.Windows.Forms.ComboBox cmb)
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
        private void fillUserTypeCombo(System.Windows.Forms.ComboBox cmb)
        {
            try
            {
                cmb.Items.Clear();
                for (int i = 0; i < Main.userTypeValues.GetLength(0); i++)
                {
                    cmb.Items.Add(Main.userTypeValues[i, 1]);
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
                clearUserData();
                enableBottomButtons();
                pnlUserList.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        public void clearUserData()
        {
            try
            {
                txtUserID.Text = "";
                txtPassword.Text = "";
                txtEmpName.Text = "";
                lblMessage.Text = "";
                cmbUserStatus.SelectedIndex = 0;
                cmbUserType.SelectedIndex = 0;
                txtSearch.Text = "";
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
                pnlUserOuter.Visible = true;
                pnlUserInner.Visible = true;
                txtEmpName.Visible = false;
                cmbEmpName.Visible = true;
                txtUserID.Enabled = true;
                txtPassword.Enabled = true;
                cmbUserType.Enabled = true;
                cmbUserStatus.Enabled = true;
                disableBottomButtons();
            }
            catch (Exception)
            {

            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!(ERPUserDB.validateUserCredentials(txtUserID.Text) && ERPUserDB.validateUserCredentials(txtPassword.Text)))
                {
                    MessageBox.Show("Malicious user credentials");
                    return;
                }
                user usr = new user();
                ERPUserDB userDB = new ERPUserDB();
                string empName = "";
                string empID = "";
                try
                {
                    empName = cmbEmpName.SelectedItem.ToString().Trim().Substring(0, cmbEmpName.SelectedItem.ToString().Trim().IndexOf('-'));
                    empID = cmbEmpName.SelectedItem.ToString().Trim().Substring(cmbEmpName.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                }
                catch (Exception)
                {
                    empName = "";
                    empID = "";
                }
                usr.userID = txtUserID.Text;
                usr.userPassword = txtPassword.Text;
                usr.userEmpID = empID;
                usr.userEmpName = empName;
                usr.userType = ComboFIll.getUserTypeCode(cmbUserType.SelectedItem.ToString());
                usr.userStatus = userDB.getUserStatusCode(cmbUserStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                string pw = usr.userPassword;
                if (btnText.Equals("Update"))
                {
                    if (userDB.updateUser(usr))
                    {
                        MessageBox.Show("User Status updated");
                        closeAllPanels();
                        ListUser();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update User Status");
                    }
                }
                else if (btnText.Equals("Save"))
                {
                    if (userDB.validateUser(usr))
                    {
                        if (Utilities.VarifyUserName(usr.userID) && Utilities.VarifyPassword(usr.userPassword))
                        {
                            usr.userPassword = Utilities.GenerateSHA256String(pw);
                            if (userDB.insertUser(usr))
                            {
                                MessageBox.Show("User data Added");
                                closeAllPanels();
                                ListUser();
                            }
                            else
                            {
                                MessageBox.Show("Failed to Insert User Data");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Enter UserId or Password Correctly");
                        }
                    }
                    else
                        MessageBox.Show("User Data Validation failed");
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
                if (e.ColumnIndex == 4)
                {

                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlUserInner.Visible = true;
                    pnlUserOuter.Visible = true;
                    pnlUserList.Visible = false;
                    //lblPasswordRestriction.Visible = false;
                    //lblUserIdRestrictions.Visible = false;
                    txtUserID.Enabled = false;
                    txtPassword.Enabled = false;
                    txtEmpName.Enabled = false;
                    cmbUserType.Enabled = false;
                    cmbUserStatus.Enabled = true;
                    cmbEmpName.Visible = false;
                    txtEmpName.Visible = true;
                    cmbUserType.SelectedIndex = cmbUserType.FindStringExact(grdList.Rows[e.RowIndex].Cells[2].Value.ToString());
                    cmbUserStatus.SelectedIndex = cmbUserStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[3].Value.ToString());
                    DataGridViewRow row = grdList.Rows[rowID];
                    txtUserID.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtEmpName.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();
                   
                    disableBottomButtons();
                    btnResetPassword.Visible = true;
                    lblMessage.Visible = false;
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
            btnResetPassword.Visible = false;
        }
        private void enableBottomButtons()
        {
            btnNew.Visible = true;
            btnExit.Visible = true;
        }

        ////private void txtUserID_TextChanged(object sender, EventArgs e)
        ////{
        ////    lblMessage.Text = "User ID length sholud be between 5 and 10";
        ////    lblMessage.Visible = true;
        ////}

        private void pnlUserInner_Paint(object sender, PaintEventArgs e)
        {

        }

        ////private void txtPassword_TextChanged(object sender, EventArgs e)
        ////{
        ////    lblMessage.Text = "Password sholud contains atleast 1 letter and 1 digit, length between 5 and 10";
        ////    lblMessage.Visible = true;
        ////}

        private void cmbUserID_SelectedIndexChanged(object sender, EventArgs e)
        {
            //PassWordResetDB pwrstDB = new PassWordResetDB();
            //List<PWreset> prst = pwrstDB.getname(txtUserID.Text);
            //foreach (PWreset pw in prst)
            //{
            //    txtEmpID.Text = pw.EmpID;
            //    txtEmpRName.Text = pw.Empname;
            //}
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            //pnlResetOuter.Visible = true;
            //pnlResetInner.Visible = true;
            //txtEmpID.Enabled = false;
            //txtEmpRName.Enabled = false;
            //cmbUserID.Enabled = false;
            //closeAllPanels();
            //PassWordResetDB.fillUserIDCombo(cmbUserID);
            //cmbUserID.SelectedItem = txtUserID.Text;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            
        }
        public string showrandom()
        {
            string password = "";
            try
            {
                password = RandomString();
                ////if (MessageBox.Show(" New Password of " + txtEmpName.Text + " is : \n" + password,
                ////    "New Password", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ////    == DialogResult.OK)
                {
                    ERPUserDB udb = new ERPUserDB();
                    string pw = Utilities.GenerateSHA256String(password);
                    if (udb.updateUserPassword(txtUserID.Text, pw))
                    {
                        MessageBox.Show("Password of " + txtEmpName.Text + " has been Changed.\nNew password emailed to "+txtEmpName.Text);
                        //clearUserData();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return password;
        }
        private string RandomString()
        {
            string Password = "";
            try
            {
                string input = "abcdefghijklmnopqrstuvwxyz0123456789ABCDEFGHIJKLMNOPQRST";
                Random random = new Random();
                Random rnd = new Random();
                StringBuilder builder = new StringBuilder();
                char ch;
                int num = rnd.Next(4, 8);
                for (int i = 0; i < num; i++)
                {
                    ch = input[random.Next(0, input.Length)];
                    builder.Append(ch);
                }
                Password = "CS4" + builder.ToString();
                if (Password.Length < 5)
                {
                    Password = Password + "12345";
                    Password = Password.Substring(0, 5);
                }
                else
                {
                    if (Password.Length > 10)
                    {
                        Password = Password.Substring(0, 10);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return Password;
        }

        private void btnReset_Click_1(object sender, EventArgs e)
        {
            try
            {

                if (txtUserID.Text == "" || txtEmpName.Text == "")
                {
                    MessageBox.Show("Please Select the UserID");
                }
                else
                {
                    if (MessageBox.Show("Are you sure to reset Password of " + txtEmpName.Text + "?",
                           "Password Reset", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                            == DialogResult.OK)
                    {
                        string newPassword = showrandom();

                        //send email to user
                        string menuID = getMenuID();
                        try
                        {
                            string toAddress = "";
                            string empID = ERPUserDB.getUserEmployeeID(txtUserID.Text);
                            toAddress = EmployeeDB.getEmployeeEmailID(empID);
                            //create emaildata
                            if (toAddress.Trim().Length > 0)
                            {
                                EmailDataDB emdataDB = new EmailDataDB();
                                emaildata emdata = new emaildata();
                                emdata.ToAddress = toAddress;
                                emdata.status = 0;
                                emdata.EmailData = "Your new ERP password is " + newPassword +"\n"+ Login.userLoggedInName;
                                emdataDB.insertEmailData(emdata);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        closeAllPanels();
                        clearUserData();
                        enableBottomButtons();
                        pnlUserList.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtUserID_Leave(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            lblMessage.Visible = false;
        }

        private void txtUserID_Enter(object sender, EventArgs e)
        {
            lblMessage.Text = "User ID length sholud be between 5 and 10";
            lblMessage.Visible = true;
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            lblMessage.Text = "Password sholud contains atleast 1 letter and 1 digit, length between 5 and 10";
            lblMessage.Visible = true;
        }
        private string getMenuID()
        {
            string menuID = "";
            try
            {
                foreach (Control p in Controls["pnlUI"].Controls)
                {
                    if (p.GetType() == typeof(Label))
                    {
                        Label c = (Label)p;
                        if (c.Name == "MenuItemID")
                        {
                            menuID = p.Text;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return menuID;
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

        private void ERPUser_Enter(object sender, EventArgs e)
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

