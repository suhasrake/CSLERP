using CSLERP.DBData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Deployment.Application;
using System.Reflection;

namespace CSLERP
{
    public partial class Main : Form
    {

        public static DateTime userLoggedInTime = DateTime.Now;
        public static string[,] statusValues, userTypeValues, CurrencyConversionTypeValues, QualityValues, AcceptanceStatus, YesNo;
        public static string[] userOptionArray;
        public string menuPrivString = "";
        public static Boolean[] itemPriv = new Boolean[4];
        public static string QueryDelimiter = "##UPSTRING##";
        public static string currentFY = "";
        public static string currentFYStartDate = "";
        public static string currentFYEndDate = "";
        public static string currentDocument = "";
        public static string currentMenuID = "";
        public static string currentFormDescription = "";
        System.Windows.Forms.Button prevbtn = null;
        public static List<menuitem> menuitems;
        public static List<menuitem> menuhdritems;
        public static char delimiter1 = (char)222; //minor delimiter
        public static char delimiter2 = (char)223; //major delimiter
        public static string documentDirectory = "";
        public static int CompanyID = 1;
        public static string MainStore = "NVPSTORE";
        public static string FactoryStore = "FACTORYSTORE";
        public static string CountryCode = "India";
        public static string StateCode = "29";
        Dictionary<string, int> dictMainHeader = new Dictionary<string, int>();
        List<string> prev = new List<string>();
        int intex = 0;
        int val = 0;
        public static List<documentreceiver> DocumentReceivers;
        public static List<systemparam> SystemParameters;
        public Main()
        {
            try
            {
                InitializeComponent();
                initVariables();
                ////Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, 1150, 635, 20, 20));
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                ////this.FormBorderStyle = FormBorderStyle.Fixed3D;
                adjustForScreenResolution();
                //addButton("BtnAccount", 0, "AccountGroup");
                //addButton("BtnFinance", 1, "FinanceGroup");
                //addButton("BtnReports", 2, "ReportGroup");
                createMainHeaderButtons();
                //createMainOptionButtons();
                pnlMainOptionButtons.AutoScroll = true;
                lblVersion.Text = CurrentVersion;
                txtVersionUpdate.ForeColor = Color.Red;
                string VerLoc = UpdateTable.getLatestVersionOfERP();
                string[] verStr = VerLoc.Split(Main.delimiter1);
                if (!CurrentVersion.Trim().Equals(verStr[0].Trim()))
                {
                    txtVersionUpdate.Text = "Latest ERP version is " + verStr[0].Trim() + ". Please update to latest version";
                }
                else
                {
                    txtVersionUpdate.Text = "";
                }

            }
            catch (Exception)
            {
                MessageBox.Show("Main() : Error 1");
            }
        }
        private void Main_Load(object sender, EventArgs e)
        {
            ////pnlMainHeader.Focus();
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            Button dashBtn = new Button();
            dashBtn.Name = "btnDASHBOARD";
            dashBtn.Click += new EventHandler(this.MyButtonHandler);
            dashBtn.PerformClick();
            DocumentReceiverDB drdb = new DocumentReceiverDB();
            DocumentReceivers = drdb.getDocumentReceiver().Where(recv => recv.EmployeeID == Login.empLoggedIn && recv.Status == 1).ToList();
            SystemParametersDB spdb = new SystemParametersDB();
            SystemParameters = spdb.getSystemparameters();
        }
        private void initVariables()
        {
            try
            {
                ComboFIll.fillStaticCombos();
                ////connString = Login.connString;
                ////userLoggedIn = Login.userLoggedIn;
                ////userLoggedInName = Login.userLoggedInName;
                ////empLoggedIn = Login.empLoggedIn;

                try
                {
                    string lst1 = FinancialYearDB.getCurrentFinancialYear();
                    string[] lst2 = lst1.Split(Main.delimiter2);
                    currentFY = lst2[0];
                    currentFYStartDate = lst2[1];
                    currentFYEndDate = lst2[2];
                }
                catch (Exception ex)
                {

                }

                documentDirectory = CatalogueValueDB.getParamValue("SysParam", "DocumentDirectory");
                showTime();
                lblEmployeeName.Text = Login.userLoggedInName;
                MenuPrivilegeDB mpDB = new MenuPrivilegeDB();
                MenuItemDB dbrecord = new MenuItemDB();
                menuhdritems = MenuItemDB.getMenuItemsHeader();
                foreach(menuitem menu in menuhdritems)
                {
                    dictMainHeader.Add(menu.menugrp, 0);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Main() : Error 2");
            }
        }

        private void createMainOptionButtons(string btnname)
        {
            try
            {
                //removpnlcontrols();
                ////name = new List<string>();
                //MenuPrivilegeDB mpDB = new MenuPrivilegeDB();
                //MenuItemDB dbrecord = new MenuItemDB();
                //menuitems = dbrecord.getMenuItems();

                //MenuPrivilegeDB mpdb = new MenuPrivilegeDB();
                //menuPrivString = mpdb.getUserMenuPrivilege(Login.userLoggedIn);
                //userOptionArray = menuPrivString.Split(';');
                List<menuitem> mnm = menuitems.Where(x => x.menugrp == btnname).ToList();
                foreach (menuitem menu in mnm)
                {
                    if (Utilities.checkMenuPrivilege(menu.menuItemID, userOptionArray) >= 0)
                    {
                        if (menu.menuitemStatus == 1)
                        {
                            addButton(menu.menuItemID, intex, menu.shortDescription);
                            intex++;
                            //name.Add("btn" + menu.menuItemID);
                        }
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Main() : Error 3");
            }
        }

        void removpnlcontrols()
        {
            pnlMainOptionButtons.Controls.Clear();
        }



        private void createmainheaderbuttons_onclick(string buttonclicked)
        {
            removpnlcontrols();
            intex = 0;
            foreach (menuitem menu in menuhdritems)
            {
                int count = 0;
                 List <menuitem> mnm = menuitems.Where(x => x.menugrp == menu.menugrp).ToList();
                foreach (menuitem menuin in mnm)
                {
                    if (Utilities.checkMenuPrivilege(menuin.menuItemID, userOptionArray) >= 0)
                    {
                        if (menuin.menuitemStatus == 1)
                        {
                            count++;
                        }
                    }
                }
                if (count > 0)
                {
                    if (menu.menugrp == buttonclicked || dictMainHeader[menu.menugrp] == 1)
                    {

                        if (dictMainHeader[menu.menugrp] == 0)
                        {
                            val = 0;
                            addButtonheader(menu.menugrp, intex, menu.menugrp);
                            intex++;
                        }
                        else
                        {
                            val = 1; //Once opened
                            addButtonheader(menu.menugrp, intex, menu.menugrp);
                            intex++;
                            createMainOptionButtons(menu.menugrp);
                        }

                    }
                    else
                    {
                        val = 0;
                        addButtonheader(menu.menugrp, intex, menu.menugrp);
                        intex++;
                    }
                }
            }
        
        }


        private void createMainHeaderButtons()
        {
            try
            {
                MenuPrivilegeDB mpDB = new MenuPrivilegeDB();
                MenuItemDB dbrecord = new MenuItemDB();
                menuitems = dbrecord.getMenuItems();
                MenuPrivilegeDB mpdb = new MenuPrivilegeDB();
                menuPrivString = mpdb.getUserMenuPrivilege(Login.userLoggedIn);
                userOptionArray = menuPrivString.Split(';');
                intex = 0;
                removpnlcontrols();
                foreach (menuitem menu in menuhdritems)
                {
                    int count = 0;
                    List<menuitem> mnm = menuitems.Where(x => x.menugrp == menu.menugrp).ToList();
                    foreach (menuitem menuin in mnm)
                    {
                        if (Utilities.checkMenuPrivilege(menuin.menuItemID, userOptionArray) >= 0)
                        {
                            if (menuin.menuitemStatus == 1)
                            {
                                count++;
                            }
                        }
                    }
                    if (count > 0)
                    {
                        val = 0;
                        addButtonheader(menu.menugrp, intex, menu.menugrp);
                        intex++;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Main() : Error 3");
            }
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox Sender = (PictureBox)sender;
            Point ptLowerLeft = new Point(0, Sender.Height);
            ptLowerLeft = Sender.PointToScreen(ptLowerLeft);
            MSuserAction.Show(ptLowerLeft);
        }

        private void menu3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void menu1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FormCollection fc = Application.OpenForms;
                foreach (Form frm in fc)
                {
                    if (frm.Name.Equals("UserAction"))
                    {
                        frm.BringToFront();
                        frm.Focus();
                        return;
                    }
                }
            }
            catch (Exception)
            {

            }
            Type CAType = Type.GetType("CSLERP." + "UserAction");
            var myObj = Activator.CreateInstance(CAType);
            Form myForm = (Form)myObj;
            //-------
            myForm.ControlBox = false;
            myForm.MinimizeBox = false;
            myForm.MaximizeBox = false;
            //-------
            this.IsMdiContainer = true;
            myForm.TopLevel = false;
            myForm.Text = "UserAction";
            myForm.Size = new Size(1150, 635);
            myForm.Controls["pnlUI"].Size = new Size(1100, 540);

            //////myForm.Controls["pnlBottomActions"].Size = new Size(1100, 42);
            pnlMainContent.Controls.Add(myForm);
            myForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            ////myForm.Dock = DockStyle.Fill;
            myForm.Show();
            myForm.BringToFront();
            myForm.Focus();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            string closeReason = "Logged out : ";
            try
            {
                switch (e.CloseReason)
                {
                    case CloseReason.ApplicationExitCall:
                        closeReason = closeReason + "ApplicationExitCall";
                        break;
                    case CloseReason.FormOwnerClosing:
                        closeReason = closeReason + "FormOwnerClosing";
                        break;
                    case CloseReason.MdiFormClosing:
                        closeReason = closeReason + "MdiFormClosing";
                        break;
                    case CloseReason.None:
                        break;
                    case CloseReason.TaskManagerClosing:
                        closeReason = closeReason + "TaskManagerClosing";
                        break;
                    case CloseReason.UserClosing:
                        closeReason = closeReason + "UserClosing";
                        break;
                    case CloseReason.WindowsShutDown:
                        closeReason = closeReason + "WindowsShutDown";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            ActivityLogDB.updateActivity(closeReason);
        }



        private void addButton(string txtButton, int index, string btnCaption)
        {
            try
            {
                System.Windows.Forms.Button button;
                button = new System.Windows.Forms.Button();

                ////System.Windows.Forms.LinkLabel button;
                ////button = new System.Windows.Forms.LinkLabel();


                button.Name = "btn" + txtButton;
                button.Text = btnCaption;
                button.Height = 24;
                button.Width = 180;
                button.BackColor = Color.FromArgb(40, 40, 40);
                button.BackColor = Color.White;
                button.ForeColor = Color.Black;
                button.Font = new Font("Lucida Console", 8);
                button.Location = new Point(5, 4 + (index * 25));
                button.Click += new EventHandler(this.MyButtonHandler);
                pnlMainOptionButtons.Controls.Add(button);
            }
            catch (Exception)
            {
                MessageBox.Show("Main() : addButton()");
            }
        }

        private void addButtonheader(string txtButton, int index, string btnCaption)
        {
            try
            {
                System.Windows.Forms.Button button;
                button = new System.Windows.Forms.Button();

                ////System.Windows.Forms.LinkLabel button;
                ////button = new System.Windows.Forms.LinkLabel();


                button.Name = txtButton;
                button.Text = btnCaption;
                button.Height = 24;
                button.Width = 180;
                button.BackColor = Color.FromArgb(40, 40, 40);
                button.BackColor = Color.DarkTurquoise;
                button.ForeColor = Color.White;
                button.Font = new Font("Lucida Console", 8);
                button.Location = new Point(5, 4 + (index * 25));
                if (val == 0)
                {
                    ////button.Image = Image.FromFile(@"..\Resources\side.png");
                    button.Image = Properties.Resources.add_1;
                }
                else if (val == 1)
                {
                    ////button.Image = Image.FromFile(@"..\Resources\down.png");
                    button.Image = Properties.Resources.minus_2;

                }
                button.ImageAlign = ContentAlignment.MiddleRight;
                button.TextAlign = ContentAlignment.MiddleLeft;
                button.FlatStyle = FlatStyle.Flat;

                button.Click += (s, e) =>
                {
                    if(prev.Contains(button.Name))
                    {
                        prev.Remove(button.Name);
                        dictMainHeader[button.Name] = 0;
                        createmainheaderbuttons_onclick(button.Name);
                    }
                    else
                    {
                        prev.Add(button.Name);
                        dictMainHeader[button.Name] = 1;
                        createmainheaderbuttons_onclick(button.Name);
                    }                    
                };
                pnlMainOptionButtons.Controls.Add(button);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Main() : addButtonheader()");
            }
        }



        private void tmrMain_Tick(object sender, EventArgs e)
        {
            DateTime dt = UpdateTable.getSQLDateTime();
            if ((60 - dt.Second) > 55)
            {
                tmrMain.Interval = 60 * 1000;
            }
            else
            {
                tmrMain.Interval = (60 - dt.Second) * 1000;
            }

            lblTime.Text = UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm");
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }


        void MyButtonHandler(object sender, EventArgs e)
        {
            try
            {
                if (prevbtn != null)
                {
                    prevbtn.BackColor = Color.White;
                }
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                //selectedCatalogueID = btn.Text;
                fillItemPrivileges(btn.Name.Substring(3));
                btn.BackColor = Color.SkyBlue;
                prevbtn = btn;
                string btnText = btn.Name;
                string frmName = "";
                String formText = "";
                if (Utilities.checkStringInArray(btn.Name.Substring(3), userOptionArray) >= 0)
                {
                    foreach (menuitem menu in menuitems)
                    {
                        if (btn.Name.Substring(3).Equals(menu.menuItemID))
                        {
                            if (menu.versionrequired.Trim().Length > 0)
                            {
                                if (CurrentVersion.CompareTo(menu.versionrequired) < 0)
                                {
                                    MessageBox.Show("This options requires version " + menu.versionrequired + " or higher");
                                    return;
                                }
                            }
                            frmName = menu.pageLink;
                            formText = menu.shortDescription;
                            currentDocument = menu.documentID;
                            currentMenuID = menu.menuItemID;
                            break;
                        }
                    }
                    Type CAType = Type.GetType("CSLERP." + frmName);
                    currentFormDescription = formText;
                    try
                    {
                        FormCollection fc = Application.OpenForms;
                        foreach (Form frm in fc)
                        {
                            if (frm.Name.Equals(frmName))
                            {
                                frm.BringToFront();
                                frm.Focus();
                                return;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                    var myObj = Activator.CreateInstance(CAType);
                    Form myForm = (Form)myObj;
                    //-------
                    myForm.ControlBox = false;
                    myForm.MinimizeBox = false;
                    myForm.MaximizeBox = false;
                    ////myForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    myForm.FormBorderStyle = FormBorderStyle.Fixed3D;
                    myForm.Dock = DockStyle.Fill;
                    ////this.FormBorderStyle = FormBorderStyle.None;
                    //-------
                    Label lblFormText = new Label();
                    Label lblMenuItemID = new Label();
                    lblFormText.Text = Main.currentFormDescription;
                    lblFormText.Size = new Size(300, 20);
                    lblFormText.Font = new Font("Arial", 10, FontStyle.Italic);
                    lblFormText.ForeColor = Color.Blue;
                    lblFormText.Location = new Point(750, 520);
                    lblFormText.TextAlign = ContentAlignment.MiddleRight;
                    myForm.Controls["pnlUI"].Controls.Add(lblFormText);
                    myForm.Controls["pnlUI"].Controls.Add(lblMenuItemID);
                    lblFormText.Name = "FormName";
                    lblFormText.Text = formText.Trim();
                    lblMenuItemID.Name = "MenuItemID";
                    lblMenuItemID.Text = btn.Name.Substring(3).Trim();
                    lblMenuItemID.Visible = false;
                    //-------
                    Panel prPanel = Utilities.createProcessPanel();
                    try
                    {
                        myForm.Controls["pnlUI"].Controls.Add(prPanel);
                    }
                    catch (Exception ex)
                    {
                    }
                    //-------
                    this.IsMdiContainer = true;
                    myForm.TopLevel = false;
                    myForm.Text = btn.Text;
                    myForm.Size = new Size(1150, 635);
                    myForm.Controls["pnlUI"].Size = new Size(1100, 540);
                    //////myForm.Controls["pnlBottomActions"].Size = new Size(1100, 42);
                    pnlMainContent.Controls.Add(myForm);
                    myForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    ////myForm.Dock = DockStyle.Fill;
                    myForm.Show();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Main() : Error 5");
            }
        }

        public void showDashBoard()
        {
            try
            {

                //this.IsMdiContainer = true;
                //myForm.TopLevel = false;
                //pnlMainContent.Controls.Add(myForm);
                //myForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                //myForm.Dock = DockStyle.Fill;
                //myForm.Show();

                Form myForm = new DashBoard();
                this.IsMdiContainer = true;
                myForm.TopLevel = false;
                //myForm.Text = btn.Text;
                myForm.Size = new Size(1150, 635);
                myForm.Controls["pnlUI"].Size = new Size(1100, 540);
                //////myForm.Controls["pnlBottomActions"].Size = new Size(1100, 42);
                pnlMainContent.Controls.Add(myForm);
                myForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                myForm.Dock = DockStyle.Fill;
                myForm.Show();

            }
            catch (Exception)
            {
                MessageBox.Show("Main() : Error 6");
            }
        }

        private void pnlMainOptionButtons_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fillItemPrivileges(string menuID)
        {
            //file action privileges for the menu item selected
            try
            {
                int intex = Utilities.checkMenuPrivilege(menuID, userOptionArray);
                if (intex >= 0)
                {
                    string[] prvArr = userOptionArray[intex].Split(',');
                    if (prvArr[1].Equals("V"))
                    {
                        itemPriv[0] = true;
                    }
                    else
                    {
                        itemPriv[0] = false;
                    }
                    if (prvArr[2].Equals("A"))
                    {
                        itemPriv[1] = true;
                    }
                    else
                    {
                        itemPriv[1] = false;
                    }
                    if (prvArr[3].Equals("E"))
                    {
                        itemPriv[2] = true;
                    }
                    else
                    {
                        itemPriv[2] = false;
                    }
                    if (prvArr[4].Equals("D"))
                    {
                        itemPriv[3] = true;
                    }
                    else
                    {
                        itemPriv[3] = false;
                    }
                }
            }
            catch (Exception)
            {
                itemPriv[0] = itemPriv[0] = itemPriv[0] = itemPriv[0] = false;
                MessageBox.Show("Main() : Error 7");
            }
        }
        private void ChangeButtonColor(string btnText)
        {
            try
            {
                foreach (Control p in pnlMainOptionButtons.Controls)
                    if (p.GetType() == typeof(Button))
                    {
                        if (p.Text == btnText)
                        {
                            p.BackColor = Color.Red; //Color.SkyBlue;
                        }
                    }
            }
            catch (Exception ex)
            {
            }
        }
        private void showTime()
        {
            try
            {
                lblTime.Text = UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm");
            }
            catch (Exception ex)
            {
            }
        }
        private void adjustForScreenResolution()
        {
            try
            {
                int maxscrennWidth = 1366;
                int maxscreenHeight = 768;

                int screenWidth = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Width.ToString());
                int screenHeight = Convert.ToInt32(Screen.PrimaryScreen.Bounds.Height.ToString());

                int diffWidth = maxscrennWidth - screenWidth;
                int diffHeight = maxscreenHeight - screenHeight;

                this.Size = new System.Drawing.Size(1366 - diffWidth, 750);
                pnlMainHeader.Size = new Size(1342 - diffWidth, 72);
                pnlMainContent.Size = new Size(1135 - diffWidth, 600 - diffHeight);
                ////pnlMainContent.Location = new Point(156, 82);
                pictureBox1.Location = new Point(1287 - diffWidth, 3);
                lblEmployeeName.Location = new Point(1166 - diffWidth, 17);
                lblTime.Location = new Point(1166 - diffWidth, 40);
                pnlMainOptionButtons.Size = new Size(200, 600 - diffHeight);
                pnlMainHeader.AutoScroll = false;
                pnlMainContent.AutoScroll = true;
            }
            catch (Exception ex)
            {
            }

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
        //07-03-2018
        public static string getSystemparametersforID(string id)
        {
            string value = "";
            try
            {
                value = SystemParameters.Where(x => x.ID == id).Select(x => x.Value).FirstOrDefault().ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return value;
        }

    }
}
