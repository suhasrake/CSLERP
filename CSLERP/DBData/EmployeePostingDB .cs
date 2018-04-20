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
    class employeeposting
    {
        public int RowID { get; set; }
        public int empID { get; set; }
        public string empName { get; set; }
        public DateTime postingDate { get; set; }
        public string officeID { get; set; }
        public string officeName { get; set; }
        public string departmentID { get; set; }
        public string departmentName { get; set; }
        public string remarks { get; set; }
        public string ReportingOfficer { get; set; }
        public string ReportingOfficername { get; set; }
        public int Status { get; set; }
        public DateTime createTime { get; set; }
        public string createUser { get; set; }
        public string createUserName { get; set; }
        public string UserID { get; set; }
    }

    class EmployeePostingDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public static List<employeeposting> getEmployeePosting(string empID)
        {
            employeeposting emppostingrec;
            List<employeeposting> EmployeePosting = new List<employeeposting>();
            try
            {
                //string connString = "Data Source = PRAKASHPC; Initial Catalog = newERP; Integrated Security = True";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "Select EmployeeID, Name, PostingDate, OfficeID, OfficeName, Department, DepartmentName, " +
                    "Remarks,Status,CreateTime,CreateUser,CreatorName,ReportingOfficerID,ReportingOfficerName,RowID from ViewEmployeePosting " +
                    "where  EmployeeID = '" + empID + "' order by PostingDate asc, CreateTime asc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    emppostingrec = new employeeposting();
                    string s = reader.GetString(1);
                    emppostingrec.empID = Convert.ToInt32(reader.GetString(0));
                    emppostingrec.empName = reader.GetString(1);
                    emppostingrec.postingDate = reader.GetDateTime(2);
                    emppostingrec.officeID = reader.GetString(3);
                    emppostingrec.officeName = reader.GetString(4);
                    emppostingrec.departmentID = reader.GetString(5);
                    emppostingrec.departmentName = reader.GetString(6);
                    emppostingrec.remarks = reader.GetString(7);
                    emppostingrec.Status = reader.GetInt32(8);
                    emppostingrec.createTime = reader.GetDateTime(9);
                    emppostingrec.createUser = reader.GetString(10);
                    emppostingrec.createUserName = reader.GetString(11);
                    emppostingrec.ReportingOfficer = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    emppostingrec.ReportingOfficername = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    emppostingrec.RowID = reader.GetInt32(14);
                    EmployeePosting.Add(emppostingrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show( System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");

            }
            return EmployeePosting;

        }
        public static Boolean updateEmpPosting(string rowid, employeeposting empPost)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                SqlConnection con = new SqlConnection(Login.connString);
                string query = "update EmployeePosting set"+
                    " PostingDate='" + (empPost.postingDate.ToString("yyyyMMdd")) + "',"+
                    " OfficeID='" + empPost.officeID + "'," +
                    " Department='" + empPost.departmentID + "'," +
                    " Remarks='" + empPost.remarks + "'," +
                    " Status=" + empPost.Status +
                    " , ReportingOfficerID = '" + empPost.ReportingOfficer + "'"+
                    " where RowID=" + rowid + "";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                //cmd.Parameters.Add("@emp.empPicture", emp.empPicture);
                utString = utString + query + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "EmployeePosting", "", query) +
                   Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }

        public static DateTime getLastPostingDate(string empID)
        {
            DateTime lastpostingdate = new DateTime();
            ////lastpostingdate = DateTime.Now;
            try
            {
                //string connString = "Data Source = PRAKASHPC; Initial Catalog = newERP; Integrated Security = True";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select max(postingdate) from ViewEmployeePosting where status = 1 and EmployeeID = '" + empID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        lastpostingdate = reader.GetDateTime(0);
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show( System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");

            }
            return lastpostingdate;
        }

        public string getEmpStatusString(int empStatus)
        {
            string empStatusString = "Unknown";
            try
            {
                for (int i = 0; i < Employee.empStatusValues.GetLength(0); i++)
                {
                    if (Convert.ToInt32(Employee.empStatusValues[i, 0]) == empStatus)
                    {
                        empStatusString = Employee.empStatusValues[i, 1];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                empStatusString = "Unknown";
            }
            return empStatusString;
        }
        public int getEmpStatusCode(string empStatusString)
        {
            int empStatusCode = 0;
            try
            {
                for (int i = 0; i < Employee.empStatusValues.GetLength(0); i++)
                {
                    if (Employee.empStatusValues[i, 1].Equals(empStatusString))
                    {
                        empStatusCode = Convert.ToInt32(Employee.empStatusValues[i, 0]);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                empStatusCode = 0;
            }
            return empStatusCode;
        }
        public Boolean updateEmployee(employee emp)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update Employee set name='" + emp.empName + "',DOB='" +
                    (emp.empDOB.ToString("yyyyMMdd HH:mm:ss")) +
                    "',DOJ='" + (emp.empDOJ.ToString("yyyyMMdd HH:mm:ss")) +
                    "',phoneno='" + emp.empPhoneNo + "',Status=" + emp.empStatus +
                    ////",Photo='" + emp.empPhoto + "'" +
                    " where EmployeeID='" + emp.empID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "Employee", "", updateSQL) +
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
        public Boolean insertEmployeePosting(employeeposting empposting)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into EmployeePosting (EmployeeID,PostingDate,OfficeID,Department,ReportingOfficerID,Remarks,Status,CreateTime,CreateUser) values (" +
                    "'" + empposting.empID + "'," +
                    "'" + (empposting.postingDate.ToString("yyyyMMdd")) + "'," +
                    "'" + empposting.officeID + "'," +
                    "'" + empposting.departmentID + "'," +
                    "'" + empposting.ReportingOfficer + "'," +
                    "'" + empposting.remarks + "'," +
                    empposting.Status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("insert", "EmployeePosting", "", updateSQL) +
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
        public Boolean validateEmployeePosting(employeeposting empposting)
        {
            Boolean status = true;
            try
            {
                if (empposting.empID == 0)
                {
                    return false;
                }
                if (empposting.officeID == null || empposting.officeID.Trim().Length == 0)
                {
                    return false;
                }
                if (empposting.departmentID == null || empposting.departmentID.Trim().Length == 0)
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                return false;
            }

            return status;
        }
        public static void fillEmpListCombo(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                EmployeeDB dbrecord = new EmployeeDB();
                List<employee> Employees = dbrecord.getEmployees();
                foreach (employee emp in Employees)
                {
                    if (emp.empStatus == 1)
                    {
                        cmb.Items.Add(emp.empName + "-" + emp.empID);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }

        }
        //public static List<employeeposting> getEmployeeListForQC()
        //{
        //    employeeposting empPosting;
        //    List<employeeposting> empPostingList = new List<employeeposting>();
        //    try
        //    {
        //        //string connString = "Data Source = PRAKASHPC; Initial Catalog = newERP; Integrated Security = True";
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "Select EmployeeID, Name,UserID, OfficeID, Department " +
        //            " from ViewEmployeeLocation " +
        //            "where status = 1 and isnull(UserID,' ') != ' ' and officeID = 'CSLB' and( Department = 'Production' or  Department = 'QC') order by PostingDate desc, CreateTime desc";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            empPosting = new employeeposting();
        //            empPosting.empID = Convert.ToInt32(reader.GetString(0));
        //            empPosting.empName = reader.GetString(1);
        //            empPosting.UserID = reader.GetString(2);
        //            empPosting.officeID = reader.GetString(3);
        //            empPosting.departmentID = reader.GetString(4);
        //            empPostingList.Add(empPosting);
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
        //    }
        //    return empPostingList;
        //}
        public static List<employeeposting> getEmployeeList(string departments)
        {
            employeeposting empPosting;
            List<employeeposting> empPostingList = new List<employeeposting>();
            try
            {
                string inStr = "";
                string[] lst1 = departments.Split(Main.delimiter1);
                for (int i = 0; i < lst1.Length; i++)
                {
                    if (lst1[i].Trim().Length > 0)
                    {
                        if (inStr.Trim().Length > 0)
                        {
                            inStr = inStr + ",";
                        }
                        inStr = inStr + "'" + lst1[i] + "'";
                    }
                }
                //string connString = "Data Source = PRAKASHPC; Initial Catalog = newERP; Integrated Security = True";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "Select EmployeeID, Name,UserID, OfficeID, Department,ReportingOfficerID " +
                    " from ViewEmployeeLocation " +
                    " where status = 1 and isnull(UserID,' ') != ' ' and officeID in ( 'BLR','NVP') " +
                    " and Department in (" + inStr + ") " +
                    " order by PostingDate desc, CreateTime desc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    empPosting = new employeeposting();
                    empPosting.empID = Convert.ToInt32(reader.GetString(0));
                    empPosting.empName = reader.GetString(1);
                    empPosting.UserID = reader.GetString(2);
                    empPosting.officeID = reader.GetString(3);
                    empPosting.departmentID = reader.GetString(4);
                    empPosting.ReportingOfficer = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    empPostingList.Add(empPosting);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            return empPostingList;
        }
        public static ListView EmpListViewForQC(string DepartmentList,String opt)
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
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                List<employeeposting> empList = new List<employeeposting>();
                if (opt == "MRN")
                {
                    empList = EmployeeDB.getEmpDetailsOfRole ("MRNQC");
                }
                else
                {
                    empList = EmployeePostingDB.getEmployeeList(DepartmentList);
                }
                
                ////List<employeeposting> empList = EmployeeDB.getEmpDetailsOfRole ("MRNQC");
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Emp ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Emp Name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Cust ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("OfficeID", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Dept ID", -2, HorizontalAlignment.Center);

                lv.Columns[0].Width = 50;
                lv.Columns[1].Width = 100;
                lv.Columns[2].Width = 200;
                lv.Columns[3].Width = 0;
                lv.Columns[4].Width = 0;
                lv.Columns[5].Width = 0;
                foreach (employeeposting emppost in empList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(emppost.empID.ToString());
                    item1.SubItems.Add(emppost.empName);
                    item1.SubItems.Add(emppost.UserID);
                    item1.SubItems.Add(emppost.officeID);
                    item1.SubItems.Add(emppost.departmentID);
                    lv.Items.Add(item1);

                }
            }
            catch (Exception)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lv;
        }
        public static List<employeeposting> getEmployeePostingList()
        {
            employeeposting emppostingrec;
            List<employeeposting> EmployeePosting = new List<employeeposting>();
            try
            {
                //string connString = "Data Source = PRAKASHPC; Initial Catalog = newERP; Integrated Security = True";
                SqlConnection conn = new SqlConnection(Login.connString);
                ////string query = "Select distinct a.EmployeeID, a.Name, a.OfficeID, a.OfficeName from ViewEmployeePosting a" +
                ////    " where a.status = 1 and a.postingdate= (select MAX(b.postingdate) "+
                ////    " from ViewEmployeePosting b where b.EmployeeID=a.EmployeeID) order by a.EmployeeID";
                string query = "Select distinct a.EmployeeID, a.Name, a.OfficeID, a.OfficeName from ViewEmployeeDetails a" +
                    " where a.status = 1  order by a.EmployeeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                {
                    emppostingrec = new employeeposting();
                    emppostingrec.empID = 99999;
                    emppostingrec.empName = "Developer";
                    emppostingrec.officeID = "";
                    emppostingrec.officeName = "";
                    EmployeePosting.Add(emppostingrec);
                }
                while (reader.Read())
                {
                    emppostingrec = new employeeposting();
                    emppostingrec.empID = Convert.ToInt32(reader.GetString(0));
                    emppostingrec.empName = reader.GetString(1);

                    emppostingrec.officeID = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    emppostingrec.officeName = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    EmployeePosting.Add(emppostingrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");

            }
            return EmployeePosting;

        }
        public static ListView getEmployeeListView()
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
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                EmployeePostingDB edb = new EmployeePostingDB();
                List<employeeposting> EMPList = EmployeePostingDB.getEmployeePostingList();
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Emp Id", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Emp Name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Office Id", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Office Name", -2, HorizontalAlignment.Left);
                lv.Columns[3].Width = 0;
                lv.Columns[4].Width = 0;
                foreach (employeeposting emp in EMPList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(emp.empID.ToString());
                    item1.SubItems.Add(emp.empName.ToString());
                    item1.SubItems.Add(emp.officeID.ToString());
                    item1.SubItems.Add(emp.officeName.ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lv;
        }
        public static string getCurrentOffice(string empID)
        {
            string office = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select OfficeID from EmployeePosting where status = 1 and EmployeeID = '" + empID + "' order by PostingDate desc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    office = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");

            }
            return office;
        }
        public static List<user> getOfficeWiseEmployeeList(string officeid)
        {
            user emp;
            List<user> empList = new List<user>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.EmployeeID,a.Name,b.UserID from ViewEmployeeDetails a,ViewUserEmployeeList b" +
                    " where a.EmployeeID = b.EmployeeID and a.OfficeID='" + officeid + "' and a.status=1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    emp = new user();
                    emp.userEmpID = reader.GetString(0);
                    emp.userEmpName = reader.GetString(1);
                    emp.userID = reader.GetString(2);
                    empList.Add(emp);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Employee List");
            }
            return empList;
        }
    }
}
