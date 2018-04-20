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
    class usermenuprivilege
    {
        public string userID { get; set; }
        public string menuItemString { get; set; }
        public string EmpID { get; set; }
        public string EmpName { get; set; }

    }
    class itemprivilege
    {
        public Boolean privView { get; set; }
        public Boolean privAdd { get; set; }
        public Boolean privEdit { get; set; }
        public Boolean privDelete { get; set; }
    }
    class MenuPrivilegeDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public string getUserMenuPrivilege(string userID)
        {
            string umpString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select MenuItemString " +
                    "from MenuPrivilege where UserID='" + userID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    umpString = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                umpString = "";
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            return umpString;
        }
        public Boolean deleteUserMenuPrivilege(string userID)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string updateSQL = "delete MenuPrivilege " +
                    "where UserID='" + userID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "MenuPrivilege", "", updateSQL) +
                    Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        public Boolean updateUserMenuPrivilege(usermenuprivilege mp)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update MenuPrivilege set MenuItemString='" + mp.menuItemString +
                    "' where UserID='" + mp.userID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("update", "MenuPrivilege", "", updateSQL) +
                  Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {

                status = false;
            }
            return status;
        }
        public Boolean insertUserMenuPrivilege(usermenuprivilege mp)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into MenuPrivilege (UserID,MenuItemString,CreateTime,CreateUser)" +
                    " values (" +
                    "'" + mp.userID + "'," +
                    "'" + mp.menuItemString + "'," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("insert", "MenuPrivilege", "", updateSQL) +
                  Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {

                status = false;
            }
            return status;
        }
        public Boolean validateUser(usermenuprivilege mp)
        {
            Boolean status = true;
            try
            {
                if (mp.userID.Trim().Length == 0 || mp.userID == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {

            }
            return status;
        }

        //04-04-2018
        public static DataGridView PrivilageListView(string docid)
        {
            DataGridView LV = new DataGridView();

            try
            {

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                LV.EnableHeadersVisualStyles = false;
                LV.AllowUserToAddRows = false;
                LV.AllowUserToDeleteRows = false;
                LV.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                LV.BorderStyle = System.Windows.Forms.BorderStyle.None;
                LV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                LV.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                LV.ColumnHeadersHeight = 27;
                LV.RowHeadersVisible = false;
                LV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                DataGridViewTextBoxColumn colempid = new DataGridViewTextBoxColumn();
                colempid.Width = 50;
                colempid.Name = "EmployeeID";
                colempid.HeaderText = "Employee ID";
                colempid.ReadOnly = false;
                LV.Columns.Add(colempid);

                DataGridViewTextBoxColumn colempname = new DataGridViewTextBoxColumn();
                colempname.Width = 50;
                colempname.Name = "EmployeeName";
                colempname.HeaderText = "Employee Name";
                colempname.ReadOnly = false;
                LV.Columns.Add(colempname);

                DataGridViewCheckBoxColumn colview = new DataGridViewCheckBoxColumn();
                colview.Width = 50;
                colview.Name = "View";
                colview.HeaderText = "View";
                colview.ReadOnly = false;
                LV.Columns.Add(colview);

                DataGridViewCheckBoxColumn colAdd = new DataGridViewCheckBoxColumn();
                colAdd.Width = 50;
                colAdd.Name = "Add";
                colAdd.HeaderText = "Add";
                colAdd.ReadOnly = false;
                LV.Columns.Add(colAdd);

                DataGridViewCheckBoxColumn colEdit = new DataGridViewCheckBoxColumn();
                colEdit.Width = 50;
                colEdit.Name = "Edit";
                colEdit.HeaderText = "Edit";
                colEdit.ReadOnly = false;
                LV.Columns.Add(colEdit);

                DataGridViewCheckBoxColumn colDel = new DataGridViewCheckBoxColumn();
                colDel.Width = 50;
                colDel.Name = "Delete";
                colDel.HeaderText = "Delete";
                colDel.ReadOnly = false;
                LV.Columns.Add(colDel);

                DataGridViewCheckBoxColumn colMail = new DataGridViewCheckBoxColumn();
                colMail.Width = 50;
                colMail.Name = "Mail";
                colMail.HeaderText = "E-Mail";
                colMail.ReadOnly = false;
                LV.Columns.Add(colMail);

                MenuPrivilegeDB MPDB = new MenuPrivilegeDB();
                List<usermenuprivilege> Allpriv = MPDB.getUserMenuPrivilegeforall();
                int i = 0;
                foreach (usermenuprivilege fordoc in Allpriv)
                {

                    LV.Rows.Add();
                    LV.Rows[i].Cells[0].Value = fordoc.EmpID.ToString();
                    LV.Rows[i].Cells[1].Value = fordoc.EmpName.ToString();
                    string mpString = fordoc.menuItemString;

                    string[] strArr = mpString.Split(';');
                    int intex = 0;
                    intex = Utilities.checkMenuPrivilege(docid, strArr);
                    if (intex >= 0)
                    {
                        string[] prvArr = strArr[intex].Split(',');
                        if (prvArr[1].Equals("V"))
                        {
                            LV.Rows[i].Cells[2].Value = true;
                        }
                        if (prvArr[2].Equals("A"))
                        {
                            LV.Rows[i].Cells[3].Value = true;
                        }
                        if (prvArr[3].Equals("E"))
                        {
                            LV.Rows[i].Cells[4].Value = true;
                        }
                        if (prvArr[4].Equals("D"))
                        {
                            LV.Rows[i].Cells[5].Value = true;
                        }
                        try
                        {
                            if (prvArr[5].Equals("M"))
                            {
                                LV.Rows[i].Cells[6].Value = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            LV.Rows[i].Cells[6].Value = false;
                        }
                    }
                    else
                    {
                        LV.Rows[i].Cells[2].Value = false;
                        LV.Rows[i].Cells[3].Value = false;
                        LV.Rows[i].Cells[4].Value = false;
                        LV.Rows[i].Cells[5].Value = false;
                        LV.Rows[i].Cells[6].Value = false;
                    }
                    i++;
                }



            }
            catch (Exception)
            {

            }
            return LV;
        }

        public List<usermenuprivilege> getUserMenuPrivilegeforall()
        {
            usermenuprivilege usr;
            List<usermenuprivilege> usrlist = new List<usermenuprivilege>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select b.EmployeeID,b.Name,a.MenuItemString from " +
                             " MenuPrivilege a,ViewUserEmployeeList b where a.UserID=b.UserID ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    usr = new usermenuprivilege();
                    usr.EmpID = reader.GetString(0);
                    usr.EmpName = reader.GetString(1);
                    usr.menuItemString = reader.GetString(2);
                    usrlist.Add(usr);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return usrlist;
        }
    }
}
