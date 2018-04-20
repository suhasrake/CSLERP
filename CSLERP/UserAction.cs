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
    public partial class UserAction : System.Windows.Forms.Form
    {
        public UserAction()
        {
            InitializeComponent();
            //////this.FormBorderStyle = FormBorderStyle.None;
           
            //
          
        }
        private void UserAction_Load(object sender, EventArgs e)
        {
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }
        private void lnkClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void lnkLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Application.Exit();
        }

        private void lnkChangePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pnlChangePassword.Visible = true;
            
        }
        private void btnCancel_Click(object sender, EventArgs e)
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
        private void txtOldPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtOldPassword.Text.Length != 0)
            {
                ERPUserDB euDB = new ERPUserDB();
                string userDetails = euDB.getUserDetail(Login.userLoggedIn,0);
                string[] strArr = userDetails.Trim().Split(';');
                string OldPWHash = Utilities.GenerateSHA256String(txtOldPassword.Text.Trim());
                if (OldPWHash != strArr[1])
                {
                    MessageBox.Show("Old Password is not correct.");
                    txtOldPassword.Text = "";
                    txtOldPassword.Focus();
                }
            }
        }

        private void txtNewPassword1_Click(object sender, EventArgs e)
        {
            //lblPasswordCheck.Text = "Password should contain atleast one letter and one number.\n Length between 5 and 10";
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                if (!( ERPUserDB.validateUserCredentials(txtNewPassword1.Text) && ERPUserDB.validateUserCredentials(txtNewPassword2.Text)))
                {
                    MessageBox.Show("Malicious user credentials");
                    return;
                }
                lblMessage.Text = "";
                if (!Utilities.VarifyPassword(txtNewPassword1.Text))
                {
                    MessageBox.Show("New Password rejected. Please check the password rule");
                    txtNewPassword1.Text = "";
                    txtNewPassword2.Text = "";
                    return;
                }
                if (!ValidateChangePWDetails())
                {
                    MessageBox.Show("Password Validation Failed. ");
                    return;
                }
                if (txtNewPassword1.Text.Trim() == txtNewPassword2.Text.Trim())
                {
                    string newPWHash = Utilities.GenerateSHA256String(txtNewPassword1.Text.Trim());
                    ERPUserDB erpuserdb = new ERPUserDB();
                    if (erpuserdb.updateUserPassword(Login.userLoggedIn, newPWHash))
                    {
                        MessageBox.Show("Password Changed Successfully.\nYou will will be logged out now.\nPlease log in again.");
                        Application.Exit();
                    }
                    else
                        MessageBox.Show("Failed to Change Password");
                }
                else
                {
                    MessageBox.Show("New password confirmation failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to Change Password");
            }
        }
        private Boolean ValidateChangePWDetails()
        {
            Boolean status = true;
            try
            {
                
                if (txtNewPassword1.Text.Length == 0)
                {
                    return false;
                }
                if (txtOldPassword.Text.Length == 0)
                {
                    return false;
                }
                if (txtNewPassword2.Text.Length == 0)
                {
                    return false;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Validation Failed. ");
            }
            return status;
        }
        ////private Boolean updateNewPassword(string newPw)
        ////{
        ////    Boolean status = true;
        ////    string utString = "";
        ////    try
        ////    {
        ////        string updateSQL = "update ERPUser set Password= '" + newPw + "'" +
        ////            " where UserID='" + Login.userLoggedIn + "'";
        ////        utString = utString + updateSQL + Main.QueryDelimiter;
        ////        utString = utString +
        ////        ActivityLogDB.PrepareActivityLogQquerString("update", "ERPUser", "", updateSQL) +
        ////        Main.QueryDelimiter;
        ////        if (!UpdateTable.UT(utString))
        ////        {
        ////            status = false;
        ////        }
        ////    }
        ////    catch (Exception)
        ////    {
        ////        status = false;
        ////    }
        ////    return status;
        ////}
        ToolTip passwordToolTip = null;
        private void txtNewPassword1_MouseHover(object sender, EventArgs e)
        {

            try
            {
                if (passwordToolTip == null)
                    passwordToolTip = new ToolTip();
                passwordToolTip.SetToolTip(this.txtNewPassword1, "Password should contain atleast one letter and one number.\n Length between 5 and 10");
            }
            catch (Exception ex)
            {
            }
        }

        private void txtOldPassword_Enter(object sender, EventArgs e)
        {
            lblMessage.Text = "";
        }

        private void txtNewPassword1_Enter(object sender, EventArgs e)
        {
            lblMessage.Text = "Password sholud contains atleast 1 letter and 1 digit, length between 5 and 10";
        }

        private void txtNewPassword2_Enter(object sender, EventArgs e)
        {
            lblMessage.Text = "Password sholud contains atleast 1 letter and 1 digit, length between 5 and 10";
        }

        private void UserAction_Enter(object sender, EventArgs e)
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

