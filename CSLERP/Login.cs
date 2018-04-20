using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using CSLERP.DBData;
using CSLERP.Encription;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Deployment.Application;
using System.Reflection;
using System.Management;
namespace CSLERP
{
    public partial class Login : Form
    {

        public static string userLoggedIn = "";
        public static string userLoggedInName = "";
        public static string empLoggedIn = "";
        public static string connString = "";
        public static int companyID = 1;
        public static string[] sqlInjectionStrings = new string[] { "SELECT", "INSERT", "UPDATE", "DELETE", "REPLACE", "DROP", "'1'='1'", "1=1", " OR ", " AND " };
        public Login()
        {
            try
            {
                ////Boolean interNetStatus = false;
                InitializeComponent();
                ////interNetStatus = InternetAvailability.IsInternetConnected();
                ////interNetStatus = InternetAvailability.IsInternetAvailable();
                //IsProcessOpen("Login");
                if (isERPRunning())
                {
                    MessageBox.Show("ERP already running");
                    this.Close();
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initialising ERP");
                this.Close();
                Application.Exit();
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            login();
        }


        public bool IsProcessOpen(string name)
        {
            try
            {
                foreach (Process clsProcess in Process.GetProcesses())
                {
                    if (clsProcess.ProcessName.Contains(name))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }
        static Boolean isERPRunning()
        {
            try
            {
                String thisprocessname = Process.GetCurrentProcess().ProcessName;

                if (Process.GetProcesses().Count(p => p.ProcessName == thisprocessname) > 1)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
            }
            return true;
        }

        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        public static string getCfg()
        {
            string str = "";
            AESEncription enc = new AESEncription();
            try
            {
                string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                using (System.IO.StreamReader sr = new System.IO.StreamReader(path + "\\ERP.txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        str = line;
                        str = enc.Decrypt256(str.Trim());
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return str;
        }

        private void txtUserID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtUserPassword.Text = "";
            }
            catch (Exception ex)
            {
            }
        }

        private void txtUserPassword_Enter(object sender, EventArgs e)
        {

        }
        public void login()
        {
            try
            {
                if (!(ERPUserDB.validateUserCredentials(txtUserID.Text) && ERPUserDB.validateUserCredentials(txtUserPassword.Text)))
                {
                    MessageBox.Show("Malicious user credentials");
                    return;
                }
                if (!(chkLocal.Checked) ^ (chkRemote.Checked))
                {
                    MessageBox.Show("Please Select Login mode (Local or Remote)");
                    return;
                }
                if (chkLocal.Checked)
                {
                    //if no LAN, return
                    if (!IsLanAvailable(0))
                    {
                        if (!InternetAvailability.IsInternetConnected())
                        {
                            ////MessageBox.Show("LAN not found");
                            ////return;
                        }
                    }
                }
                if (chkRemote.Checked)
                {
                    //if no internet, return
                    if (!InternetAvailability.IsInternetConnected())
                    {
                        MessageBox.Show("Internet not available");
                        return;
                    }
                    
                }
                if (txtUserID.Text.Trim().Length == 0 || txtUserPassword.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Login Failed");
                    return;
                }
                btnSubmit.Enabled = false;
                string cfgData = getCfg();
                string[] result = cfgData.Split(new string[] { "\r", "\n", "\r\n", "\n\r" }, StringSplitOptions.RemoveEmptyEntries);
                if (chkLocal.Checked)
                {
                    ////string[] cfgArr1 = result[0].Split(';');
                    ////connString = "Data Source=tcp:" + cfgArr1[0] + ";Database=" + cfgArr1[1] + ";User id=" + cfgArr1[2] + ";Password=" + cfgArr1[3] + ";";
                    connString = result[0];
                }
                else if (chkRemote.Checked)
                {
                    ////string[] cfgArr2 = result[1].Split(';');
                    ////connString = "Data Source=tcp:" + cfgArr2[0] + ";Database=" + cfgArr2[1] + ";User id=" + cfgArr2[2] + ";Password=" + cfgArr2[3] + ";";
                    connString = result[1];
                }
                ////connString = "Data Source=tcp:45.124.158.119,501;Database=newERP;User id=sa;Password=sasa;";
                //check user count. if no user add admin/admin123
                ERPUserDB userDB = new ERPUserDB();
                int userCount = userDB.getUserCount();
                if (userCount == 0)
                {
                    EmployeeDB.insertDefaultData();
                    userLoggedIn = "Developer";
                    empLoggedIn = "Developer";
                    this.Hide();
                    Main frmMain = new Main();
                    frmMain.ShowDialog();
                }
                else if (userCount == -1)
                {
                    MessageBox.Show("Cannont find db");
                    this.Close();
                    Application.Exit();
                }

                string hashPw = txtUserPassword.Text.Trim();
                ERPUserDB euDB = new ERPUserDB();
                string userDetails = euDB.getUserDetail(txtUserID.Text.Trim(), 0);
                string[] strArr = userDetails.Trim().Split(';');

                hashPw = Utilities.GenerateSHA256String(txtUserPassword.Text.Trim());
                if (hashPw == strArr[1])
                {
                    userLoggedIn = strArr[0];
                    empLoggedIn = strArr[2];
                    userLoggedInName = strArr[3];
                     
                    ActivityLogDB.updateActivity("Logged in. Version:" + CurrentVersion);
                    this.Hide();
                    Main frmMain = new Main();
                    frmMain.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Login Failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login Failed");
            }
            btnSubmit.Enabled = true;
        }

        private void txtUserPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void txtUserPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }

        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void chkRemote_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkLocal_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkLocal_Click(object sender, EventArgs e)
        {
            if ((chkRemote.Checked == true))
            {
                chkRemote.Checked = false;
            }
        }

        private void chkRemote_Click(object sender, EventArgs e)
        {
            if ((chkLocal.Checked == true))
            {
                chkLocal.Checked = false;
            }
        }

        public static bool IsLanAvailable(long minimumSpeed)
        {
            try
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                    return false;

                foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
                {
                    // discard because of standard reasons
                    if ((ni.OperationalStatus != OperationalStatus.Up) ||
                        (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback) ||
                        (ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel))
                        continue;

                    // this allow to filter modems, serial, etc.
                    // I use 10000000 as a minimum speed for most cases
                    if (ni.Speed < minimumSpeed)
                        continue;

                    // discard virtual cards (virtual box, virtual pc, etc.)
                    if ((ni.Description.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0) ||
                        (ni.Name.IndexOf("virtual", StringComparison.OrdinalIgnoreCase) >= 0))
                        continue;

                    // discard "Microsoft Loopback Adapter", it will not show as NetworkInterfaceType.Loopback but as Ethernet Card.
                    if (ni.Description.Equals("Microsoft Loopback Adapter", StringComparison.OrdinalIgnoreCase))
                        continue;

                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }
        private string CurrentVersion
        {
            get
            {
                return ApplicationDeployment.IsNetworkDeployed
                       ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
                       : Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
    }
    public class InternetAvailability
    {
        private static int ERROR_SUCCESS = 0;
        public static bool IsInternetConnected()
        {
            try
            {
                long dwConnectionFlags = 0;
                if (!InternetGetConnectedState(dwConnectionFlags, 0))
                    return false;

                if (InternetAttemptConnect(0) != ERROR_SUCCESS)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        [DllImport("wininet.dll", SetLastError = true)]
        public static extern int InternetAttemptConnect(uint res);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetConnectedState(long flags, long reserved);

      
    }
  
}
