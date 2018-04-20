using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    //public strin "Data Source = PRAKASHPC; Initial Catalog = newERP; Integrated Security = True"
    class user
    {
        public string userID { get; set; }
        public string userPassword { get; set; }
        public string userEmpID { get; set; }
        public string userEmpName { get; set; }
        public int userType { get; set; }
        public int userStatus { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
    }

    class ERPUserDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<user> getUsers()
        {
            user userrec;
            List<user> Users = new List<user>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.userid, a.password,a.employeeid,b.name,a.type,a.status " +
                    "from ERPUser a, Employee b where a.EmployeeID=b.EmployeeID and b.status = 1 order by b.name asc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    userrec = new user();
                    userrec.userID = reader.GetString(0);
                    userrec.userPassword = reader.GetString(1);
                    userrec.userEmpID = reader.GetString(2);
                    userrec.userEmpName = reader.GetString(3);
                    userrec.userType = reader.GetInt32(4);
                    userrec.userStatus = reader.GetInt32(5);
                    Users.Add(userrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return Users;

        }
        public int getUserCount()
        {
            int userCount = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select count(*) from ERPUser  where  status = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    userCount = reader.GetInt32(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                userCount = -1;
                ////MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            return userCount;
        }
        public string getUserStatusString(int userStatus)
        {
            string userStatusString = "Unknown";
            try
            {
                for (int i = 0; i < Main.statusValues.GetLength(0); i++)
                {
                    if (Convert.ToInt32(Main.statusValues[i, 0]) == userStatus)
                    {
                        userStatusString = Main.statusValues[i, 1];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                userStatusString = "Unknown";
            }
            return userStatusString;
        }



        public int getUserStatusCode(string userStatusString)
        {
            int userStatusCode = 0;
            try
            {
                for (int i = 0; i < Main.statusValues.GetLength(0); i++)
                {
                    if (Main.statusValues[i, 1].Equals(userStatusString))
                    {
                        userStatusCode = Convert.ToInt32(Main.statusValues[i, 0]);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                userStatusCode = 0;
            }
            return userStatusCode;
        }

        public Boolean updateUser(user user)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ERPUser set Status=" + user.userStatus +
                    " where UserID='" + user.userID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("update", "ERPUser", "", updateSQL) +
                  Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean updateUserPassword(string userID, string Password)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ERPUser set Password='" + Password + "'" +
                    " where UserID='" + userID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("update", "ERPUser", "", updateSQL) +
                  Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean insertUser(user usr)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into ERPUser (UserID,Password,EmployeeID,Type,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + usr.userID + "'," +
                    "'" + usr.userPassword + "'," +
                     usr.userEmpID + "," +
                    usr.userType + "," +
                    usr.userStatus + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("insert", "ERPUser", "", updateSQL) +
                  Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean validateUser(user usr)
        {
            Boolean status = true;
            try
            {
                if (usr.userID.Trim().Length == 0 || usr.userID == null)
                {
                    return false;
                }
                if (usr.userPassword.Trim().Length == 0 || usr.userPassword == null)
                {
                    return false;
                }
                if (usr.userEmpID.Trim().Length == 0 || usr.userEmpID == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return status;
        }
        public string getUserDetail(string userid, int opt)
        {
            string userString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.userid, a.password,a.employeeid,b.name,a.type,a.status " +
                    "from ERPUser a, Employee b where a.EmployeeID=b.EmployeeID and b.status = 1 and a.UserID = '" +
                    userid + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (opt == 0)
                    {
                        userString = userString + reader.GetString(0) + ";" + reader.GetString(1) + ";" +
                        reader.GetString(2) + ";" + reader.GetString(3);
                    }
                    else if (opt == 1)
                    {
                        userString = reader.GetString(3);
                    }

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                userString = "";
            }
            return userString;
        }
        public string getUserPassWord(string userid)
        {
            string password = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select userid, password " +
                    "from ERPUser where status = 1 and UserID = '" + userid + "'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    password = reader.GetString(1);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                password = "";
            }
            return password;
        }

        public static string getUserEmployeeID(string userid)
        {
            string employeeID = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select EmployeeID " +
                    "from viewUserEmployeeList where UserID = '" + userid + "'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    employeeID = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                employeeID = "";
            }
            return employeeID;
        }

        public string getUserNames(string userids)
        {
            string usernames = "";
            try
            {
                string[] lst1 = userids.Split(Main.delimiter1);
                for (int i = 0; i < lst1.Length - 1; i++)
                {
                    usernames = usernames + getUserDetail(lst1[i], 1) + ",";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return usernames;
        }
        public string getEmpCode(string uid)
        {
            string eid = "";
            try
            {
                eid = getUserDetail(uid, 0);
                eid = eid.Split(';')[2];
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return eid;
        }
        public static ListView getEmployeeUserlv()
        {
            ListView lv = new ListView();
            try
            {

                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Descending;
                ERPUserDB edb = new ERPUserDB();
                List<user> EMPList = edb.getUsers();
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Emp Id", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Emp Name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("UserID", -2, HorizontalAlignment.Left);
                lv.Columns[3].Width = 0;
                foreach (user user in EMPList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(user.userEmpID.ToString());
                    item1.SubItems.Add(user.userEmpName.ToString());
                    item1.SubItems.Add(user.userID.ToString());
                    ////lv.Items.Add(item1);
                    lv.Items.Insert(0, item1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lv;
        }
        public static string getemailIDs(List<user> list, string menuID)
        {
            //if opt==1, then check the email privilege for the user for the menu item
            string emailIDs = "";

            try
            {
                if (list.Count != 0)
                {
                    foreach (user us in list)
                    {
                        string[] userOptionArray;
                        MenuPrivilegeDB mpdb = new MenuPrivilegeDB();
                        String menuPrivString = mpdb.getUserMenuPrivilege(us.userID);
                        userOptionArray = menuPrivString.Split(';');
                        for (int i = 0; i < userOptionArray.Length; i++)
                        {
                            if (userOptionArray[i].StartsWith(menuID))
                            {
                                string[] privileges = userOptionArray[i].Split(',');
                                if (privileges[5] == "M")
                                {
                                    //find email id and add in to address
                                    string eid = EmployeeDB.getEmployeeEmailID(us.userEmpID);
                                    if (eid.Trim().Length > 0)
                                    {
                                        emailIDs = emailIDs + EmployeeDB.getEmployeeEmailID(us.userEmpID) + ";";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return emailIDs;
        }
        public static Boolean validateUserCredentials(string val)
        {
            Boolean stat = true;

            try
            {
                for (int i = 0; i < Login.sqlInjectionStrings.Length; i++)
                {
                    if (val.ToUpper().Contains(Login.sqlInjectionStrings[i].ToUpper()))
                    {
                        stat = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                stat = true;
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return stat;
        }
    }
}
