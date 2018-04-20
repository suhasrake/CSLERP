using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using CSLERP.DBData;

namespace CSLERP.DBData
{
    //public strin "Data Source = PRAKASHPC; Initial Catalog = newERP; Integrated Security = True"
    class employee
    {
        public int empID { get; set; }
        public string empName { get; set; }
        public string Gender { get; set; }
        public DateTime empDOB { get; set; }
        public DateTime empDOJ { get; set; }
        public DateTime empDOR { get; set; }
        public string empPhoneNo { get; set; }
        public int empUserID { get; set; }
        public int empStatus { get; set; }
        public string empEmailID{ get; set; }
        ////////public string empPhoto { get; set; }
        public string empPicture { get; set; }
        public string designation { get; set; }
        public string department { get; set; }
        public string reportingofficer { get; set; }
        public string office { get; set; }
        public string region { get; set; }
        public string remarks { get; set; }
        public int status { get; set; }
        public DateTime Descdate { get; set; }
        public DateTime createtime { get; set; }
        public string createuser { get; set; }
      
    }
    class employeeDesignation
    {
        public int rowid { get; set; }
        public int empID { get; set; } 
        public string designation { get; set; }
        public string designationDescription { get; set; }
        public string remarks { get; set; }
        public int status { get; set; }
        public DateTime Descdate { get; set; }
        public DateTime createtime { get; set; }
        public string createuser { get; set; }
    }
    class EmployeePayRevision
    {
        public int rowId { get; set; }
        public string empID { get; set; }
        public string empName { get; set; }
        public DateTime RevisionDate { get; set; }
        public decimal FixPay { get; set; }
        public decimal VariablePay { get; set; }
        public int VPPercentage { get; set; }
        public int status { get; set; }
        public DateTime createtime { get; set; }
        public string createuser { get; set; }
        public string Designation { get; set; }
        public string Office { get; set; }
    }
    class EmployeeRole
    {
        public int rowId { get; set; }
        public string empID { get; set; }
        public string userID { get; set; }
        public string empName { get; set; }
        public string EmpRoles { get; set; }
        //public int status { get; set; }
        public DateTime createtime { get; set; }
        public string createuser { get; set; }
    }
    class EmployeeQualification
    {
        public int rowId { get; set; }
        public string empID { get; set; }
        public string QualificationID { get; set; }
        public string QualificationName { get; set; }
        public Int32 Year { get; set; }
        public string Remarks { get; set; }
       
    }
    class EmployeeExperience
    {
        public int rowId { get; set; }
        public string empID { get; set; }
        public Nullable<DateTime> startdate { get; set; }
        public Nullable<DateTime> enddate { get; set; }
        public string employer { get; set; }
        public string postheld { get; set; }
        public string Remarks { get; set; }
    }
    class EmployeeSelection
    {
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string UserID { get; set; }
    }
    class EmployeeDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        ///DataGridView empGgrid = new DataGridView();
        public List<employee> getEmployees()
        {

            employee emprec;
            List<employee> Employees = new List<employee>();
            try
            {
               
                //string connString = "Data Source = PRAKASHPC; Initial Catalog = newERP; Integrated Security = True";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "Select a.EmployeeID, a.Name, a.DOB, a.DOJ, a.Designation, a.DepartmentName, a.RegionName,"+
                    " a.ReportingOfficerName, a.OfficeName, a.DOR, a.PhoneNo, a.Status, a.EmailID,a.Gender from " +
                    " ViewEmployeeDetails a order by cast(a.EmployeeID as integer)";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
               
                while (reader.Read())
                {
                    emprec = new employee();
                    string s = reader.GetString(1);
                    emprec.empID = Convert.ToInt32(reader.GetString(0));
                    emprec.empName = reader.GetString(1);
                    emprec.empDOB = reader.GetDateTime(2);
                    emprec.empDOJ = reader.GetDateTime(3);
                    emprec.designation = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    emprec.department = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    emprec.region = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    emprec.reportingofficer = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    emprec.office = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    if (!reader.IsDBNull(9))
                    {
                        emprec.empDOR = reader.GetDateTime(9);
                    }
                    emprec.empPhoneNo = reader.IsDBNull(10) ? "" : reader.GetString(10);
                    emprec.empStatus = reader.GetInt32(11);
                  
                    //emprec.empPicture = reader.IsDBNull(12) ? null : reader.GetString(12);
                    emprec.empEmailID = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    emprec.Gender = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    Employees.Add(emprec);

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");

            }
            return Employees;

        }
        public List<employee> getEmployeeReport()
        {
            employee empdescrec;
            List<employee> EmployeeDesig = new List<employee>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.EmployeeID,a.Name,a.DepartmentName,a.ReportingOfficerName,a.OfficeName,a.DesignationName from " +
                                "ViewEmployeeDetails a,(select MAX(PostingDate) as posting from ViewEmployeeDetails group by PostingDate )b, " +
                                 "(select MAX(DesignationDate) as Designation from ViewEmployeeDetails group by DesignationDate ) c " +
                                  "where a.PostingDate = b.Posting and a.DesignationDate = c.designation";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    empdescrec = new employee();
                    empdescrec.empID = Convert.ToInt32(reader.GetString(0));
                    empdescrec.empName = reader.GetString(1);
                    empdescrec.department = reader.GetString(2);
                    empdescrec.reportingofficer = reader.IsDBNull(3) ? " " : reader.GetString(3);
                    empdescrec.designation = reader.GetString(5);
                    empdescrec.office = reader.GetString(4);
                    EmployeeDesig.Add(empdescrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");

            }
            return EmployeeDesig;
        }
        public static List<employeeDesignation> getEmployeeDesignation(string empID)
        {
            employeeDesignation empdescrec;
            List<employeeDesignation> EmployeeDesig = new List<employeeDesignation>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "Select  a.EmployeeID, a.Date , a.Designation," +
                    "a.Remarks, a.Status, a.CreateTime, a.CreateUser,a.ROwID, b.Description from EmployeeDesignation a, CatalogueValue b " +
                    "where a.Designation=b.CatalogueValueID and a.EmployeeID = '" + empID + "' order by Date";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    empdescrec = new employeeDesignation();
                    //string s = reader.GetString(1);
                    empdescrec.empID = Convert.ToInt32(reader.GetString(0));
                   // empdescrec.empName = reader.GetString(1);
                    empdescrec.Descdate = reader.GetDateTime(1);
                    empdescrec.designation = reader.GetString(2);
                    empdescrec.remarks = reader.GetString(3);
                    empdescrec.status = reader.GetInt32(4);
                    empdescrec.createtime = reader.GetDateTime(5);
                    empdescrec.createuser = reader.GetString(6);
                    empdescrec.rowid = reader.GetInt32(7);
                    empdescrec.designationDescription = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    EmployeeDesig.Add(empdescrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");

            }
            return EmployeeDesig;

        }
        public static List<EmployeeQualification> getEmployeeQualification(string empID)
        {
            EmployeeQualification empqualrec;
            List<EmployeeQualification> EmployeeQualification = new List<EmployeeQualification>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "Select  a.EmployeeID, a.YearOfPassing, a.Qualification , a.Remarks," +
                    " b.Description from EmployeeQualification a, CatalogueValue b " +
                    "where a.Qualification=b.CatalogueValueID and a.EmployeeID = '" + empID + "' order by a.YearOfPassing";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    empqualrec = new EmployeeQualification();
                    //string s = reader.GetString(1);
                    empqualrec.empID = (reader.GetString(0));
                    // empdescrec.empName = reader.GetString(1);
                    empqualrec.Year = reader.IsDBNull(1)?0: reader.GetInt32(1);
                    empqualrec.QualificationID = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    empqualrec.Remarks = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    empqualrec.QualificationName = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    EmployeeQualification.Add(empqualrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return EmployeeQualification;
        }

        public static List<EmployeeExperience> getEmployeeExperience(string empID)
        {
            EmployeeExperience empexprec;
            List<EmployeeExperience> employeeexperience = new List<EmployeeExperience>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "Select  a.EmployeeID, a.ExpStartDate, a.ExpEndDate,a.Employer,a.Post , a.Remarks " +
                    "from EmployeeExperience a " +
                    "where  a.EmployeeID = '" + empID + "' order by a.ExpStartDate";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        empexprec = new EmployeeExperience();
                        //string s = reader.GetString(1);
                        empexprec.empID = (reader.GetString(0));
                        empexprec.startdate = reader.GetDateTime(1);
                        empexprec.enddate = reader.GetDateTime(2);
                        empexprec.employer = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        empexprec.postheld = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        empexprec.Remarks = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        employeeexperience.Add(empexprec);
                    }
                    catch (Exception)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return employeeexperience;
        }

        public static Boolean updateEmpDesignation(string rowid, employee emp)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                SqlConnection con = new SqlConnection(Login.connString);
                string query = "update EmployeeDesignation set"+
                    " Date='"+(emp.Descdate.ToString("yyyyMMdd"))+"'," +
                    " Designation='" + emp.designation+"'," +
                   " Remarks='" + emp.remarks+"'," +
                   " Status=" + emp.status + " where RowID=" + rowid + "";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                //cmd.Parameters.Add("@emp.empPicture", emp.empPicture);
                utString = utString + query + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "EmployeeDesignation", "", query) +
                   Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
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
                SqlConnection conn = new SqlConnection(Login.connString);
               
                string updateSQL = "update Employee set name='" + emp.empName +
                    "',Gender = '"+emp.Gender+
                    "',DOB='" +
                    (emp.empDOB.ToString("yyyyMMdd HH:mm:ss")) +
                    "',DOJ='" + (emp.empDOJ.ToString("yyyyMMdd HH:mm:ss")) +
                    "',phoneno='" + emp.empPhoneNo + "',Status=" + emp.empStatus +
                    ////////",Photo='" + emp.empPhoto + "'"+
                    ",Picture='" + emp.empPicture+"'"+
                     ",EmailID='" + emp.empEmailID + "'" +
                    " where EmployeeID='" + emp.empID + "'";
                SqlCommand cmd = new SqlCommand(updateSQL, conn);
                conn.Open();
                //cmd.Parameters.Add("@emp.empPicture", emp.empPicture);
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
        public Boolean insertEmployee(employee emp)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into Employee (EmployeeID,name,Gender,EmailID,dob,doj,phoneno,Picture,status,CreateTime,CreateUser) values (" +
                    "'" + emp.empID + "'," +
                    "'" + emp.empName + "'," +
                    "'" + emp.Gender + "'," +
                      "'" + emp.empEmailID + "'," +
                    "'" + (emp.empDOB.ToString("yyyyMMdd HH:mm:ss")) + "'," +
                     "'" + (emp.empDOJ.ToString("yyyyMMdd HH:mm:ss")) + "'," +
                      "'" + emp.empPhoneNo + "','"+emp.empPicture+"'," +
                    emp.empStatus + "," +
                     "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("insert", "Employee", "", updateSQL) +
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
        public Boolean validateEmployee(employee emp)
        {
            Boolean status = true;
            try
            {
                if (emp.empID == 0)
                {
                    return false;
                }
                if (emp.empName.Trim().Length == 0)
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
        public Boolean validateEmployeeDesc(employee emp)
        {
            Boolean status = true;
            try
            {
                if (emp.empID == 0)
                {
                    return false;
                }
                if (emp.Descdate == null)
                {
                    return false;
                }
                if (emp.designation.Trim().Length == 0 || emp.designation == null)
                {
                    return false;
                }
                if (emp.remarks.Trim().Length == 0 || emp.remarks == null)
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
                //////Employees.Sort();
                //////Employees.Sort(delegate (employee thisItem, employee otherItem) {
                //////    return thisItem.Text.CompareTo(otherItem.Text);
                //////});
                foreach (employee emp in Employees)
                {
                    if (emp.empStatus==1)
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
        public static Boolean insertDefaultData()
        {
            Boolean status = true;
            string utString = "";
            try
            {
                //Employee
                string updateSQL = "insert into Employee (EmployeeID,name,Gender,dob,doj,phoneno,Picture,status,CreateTime,CreateUser) values (" +
                    "'" + 99999 + "'," +
                    "'" + "Developer" + "'," +
                    "Male"+
                    "'" + "2016-01-01 00:00:00" + "'," +
                     "'" + "2016-01-01 00:00:00" + "'," +
                      "'" + "" + "','"+"'," +
                    1 + "," +
                     "GETDATE()" + "," +
                    "'" +"Developer" + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;

                updateSQL = "insert into ERPUser (UserID,Password,EmployeeID,Type,Status,CreateTime,CreateUser)" +
                   "values (" +
                   "'" + "Developer" + "'," +
                   "'" + "Developer1" + "'," +
                    99999 + "," +
                   1 + "," +
                   1 + "," +
                   "GETDATE()" + "," +
                   "'" + "Developer" + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;

                updateSQL = "insert into MenuPrivilege (UserID,MenuItemString,CreateTime,CreateUser)" +
                    " values (" +
                    "'" + "Developer" + "'," +
                    "'" + "Employee,V,A,E,D;Users,V,A,E,D,M;MenuPrivilege,V,A,E,D,M;" + "'," +
                    "GETDATE()" + "," +
                    "'" + "Developer" + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;

                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        //public Boolean validateEmployeeDesignation(employee emp)
        //{
        //    Boolean status = true;
        //    try
        //    {
        //        if (emp.empID == 0)
        //        {
        //            return false;
        //        }
        //        if (emp.designation == null || emp.designation.Trim().Length == 0)
        //        {
        //            return false;
        //        }
        //        if (emp.Descdate == Convert.ToDateTime("01-01-1900"))
        //        {
        //            return false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        //        return false;
        //    }

        //    return status;
        //}
        public Boolean insertEmployeeDesignation(employee emp)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into EmployeeDesignation (EmployeeID,Date,Designation,Remarks,Status,CreateTime,CreateUser) values (" +
                    "'" + emp.empID + "'," +
                    "'" + (emp.Descdate.ToString("yyyyMMdd")) + "'," +
                    "'" + emp.designation + "'," +
                    "'" + emp.remarks + "'," +           
                    emp.status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("insert", "EmployeeDesignation", "", updateSQL) +
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
        public static List<EmployeePayRevision> getEmployeePayRevDetail(string empID)
        {
            EmployeePayRevision empPay;
            List<EmployeePayRevision> EmpPayList = new List<EmployeePayRevision>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select  a.RowID, a.EmployeeID,b.Name, a.RevisionDate ,a.FixPay, a.VariablePay," +
                    " a.VPPercentage,a.Status,a.CreateTime, a.CreateUser, "+
                    " (select top 1 designation from EmployeeDesignation where  Date <= a.RevisionDate "+
                    " and EmployeeID = '" + empID + "' order by date desc) Designation, " +
                    " (select top 1 officeid from EmployeePosting where PostingDate <= a.RevisionDate "+
                    " and EmployeeID = '" + empID + "' order by PostingDate desc) Office " +
                    " from EmployeePayRevision a, Employee b " +
                    " where a.EmployeeID = b.EmployeeID and  a.EmployeeID = '" + empID + "' order by RevisionDate";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    empPay = new EmployeePayRevision();
                    empPay.rowId = reader.GetInt32(0);
                    empPay.empID = reader.GetString(1);
                    empPay.empName = reader.GetString(2);
                    empPay.RevisionDate = reader.GetDateTime(3);
                    empPay.FixPay = reader.GetDecimal(4);
                    empPay.VariablePay = reader.GetDecimal(5);
                    empPay.VPPercentage = reader.GetInt32(6);
                    empPay.status = reader.GetInt32(7);
                    empPay.createtime = reader.GetDateTime(8);
                    empPay.createuser = reader.GetString(9);
                    ////empPay.Designation = reader.GetString(10);
                    empPay.Designation = reader.IsDBNull(10) ? "" : reader.GetString(10);
                    ////empPay.Office = reader.GetString(11);
                    empPay.Office = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    EmpPayList.Add(empPay);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return EmpPayList;

        }
        public Boolean insertEmployeePayRev(EmployeePayRevision empRev)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into EmployeePayRevision (EmployeeID,RevisionDate,FixPay,VariablePay,"+
                    "VPPercentage,status,CreateTime,CreateUser) values (" +
                    "'" + empRev.empID + "'," +
                    "'" + empRev.RevisionDate.ToString("yyyy-MM-dd") + "'," +
                     + empRev.FixPay + "," +
                    empRev.VariablePay + "," +
                     empRev.VPPercentage + "," +
                    empRev.status + "," +
                     "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("insert", "EmployeePayRevision", "", updateSQL) +
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
        public Boolean updateEmployeePayRev(EmployeePayRevision empRev)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);

                string updateSQL = "update EmployeePayRevision set"+
                     " RevisionDate='" + empRev.RevisionDate.ToString("yyyy-MM-dd") + "',"+
                      " FixPay=" + empRev.FixPay +
                       ", VariablePay=" + empRev.VariablePay +
                        ", VPPercentage=" + empRev.VPPercentage +
                    " ,Status=" + empRev.status +
                    " where RowID = " + empRev.rowId;
                SqlCommand cmd = new SqlCommand(updateSQL, conn);
                conn.Open();
                //cmd.Parameters.Add("@emp.empPicture", emp.empPicture);
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "EmployeePayRevision", "", updateSQL) +
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
        public Boolean validateEmpPayRev(EmployeePayRevision emp)
        {
            Boolean status = true;
            try
            {
                if (emp.empID.Trim().Length == 0 || emp.empID == null)
                {
                    return false;
                }
                if (emp.RevisionDate == null)
                {
                    return false;
                }
                if (emp.FixPay == 0)
                {
                    return false;
                }
                if (emp.VariablePay == 0 && emp.FixPay == 0)
                {
                    return false;
                }
                //if (emp.VPPercentage == 0)
                //{
                //    return false;
                //}
                if (emp.status == 0)
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
        public static DateTime getlastEmpPayRevDate(string empID)
        {
            DateTime lastrevdate = new DateTime();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select max(RevisionDate) from EmployeePayRevision where status = 1 and EmployeeID = '" + empID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        lastrevdate = reader.GetDateTime(0);
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lastrevdate;
        }
        public static string getEmployeeEmailID(string empID)
        {
            string emailID = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select EmailID from Employee where status = 1 and EmployeeID = '" + empID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        emailID = reader.GetString(0);
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return emailID;
        }


        public Boolean insertEmployeeRoles(EmployeeRole role)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                updateSQL = "Delete from EmployeeRoleMapping where EmployeeID='" + role.empID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "EmployeeRoleMapping", "", updateSQL) +
                    Main.QueryDelimiter;
                updateSQL = "insert into EmployeeRoleMapping (EmployeeID,EmployeeRoles,CreateTime,CreateUser) values (" +
                    "'" + role.empID + "'," +
                    "'" + role.EmpRoles + "'," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("insert", "EmployeeRoleMapping", "", updateSQL) +
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
        public static String getEmployeeRolesList(string empID)
        {
            String empRole = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select  EmployeeRoles" +
                    " from EmployeeRoleMapping " +
                    "where EmployeeID = '" + empID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    empRole = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return empRole;
        }
        public static String getEmployeesOfRole(string rolename)
        {
            String empid = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select  employeeid" +
                    " from EmployeeRoleMapping " +
                    "where  EmployeeRoles like '%" + rolename + "%'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    empid = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return empid;
        }

        public static List<employeeposting> getEmpDetailsOfRole(string rolename)
        {
            List<employeeposting> empPostingList = new List<employeeposting>();
            String empid = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select  a.employeeid,b.name,c.UserID  from EmployeeRoleMapping a " + 
                    " left outer join employee as b on a.EmployeeID=b.EmployeeID" +
                    " left outer join ERPUser as c on a.EmployeeID = c.EmployeeID" +
                    " where  EmployeeRoles like '%" + rolename + "%'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employeeposting ep = new employeeposting();
                    ep.empID = Convert.ToInt32(reader.GetString(0));
                    ep.empName = reader.GetString(1);
                    ep.UserID = reader.GetString(2);
                    empPostingList.Add(ep);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return empPostingList;
        }
        public static Boolean checkEmployeeRole(string empID, string role)
        {
            //MovementApproval
            Boolean empRoleFound = false;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select  EmployeeRoles" +
                    " from EmployeeRoleMapping " +
                    "where EmployeeID = '" + empID + "' and EmployeeRoles like '%" + role+"%'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    empRoleFound = true;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return empRoleFound;
        }
        public static List<EmployeeRole> getEmployeeRoleList()
        {
            EmployeeRole role;
            List<EmployeeRole> empRoleList = new List<EmployeeRole>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.EmployeeID,b.Name,c.UserID, a.EmployeeRoles, a.CreateTime, a.CreateUser " +
                    "from EmployeeRoleMapping a , Employee b , ERPUser c where a.EmployeeID = b.EmployeeID and a.EmployeeID = c.EmployeeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    role = new EmployeeRole();
                    role.empID = reader.GetString(0);
                    role.empName = reader.GetString(1);
                    role.userID = reader.GetString(2);
                    role.EmpRoles = reader.GetString(3);
                    role.createtime = reader.GetDateTime(4);
                    role.createuser = reader.GetString(5);
                    empRoleList.Add(role);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return empRoleList;
        }
        public static ListView MovementRegApproverListView(string roleString)
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

                List<EmployeeRole> empList = EmployeeDB.getEmployeeRoleList();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Emp ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Emp name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("User ID", -2, HorizontalAlignment.Left);
                //lv.Columns[3].Width = 0;
                foreach (EmployeeRole role in empList)
                {
                    if (role.EmpRoles.Contains(roleString))
                    {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        item1.SubItems.Add(role.empID.ToString());
                        item1.SubItems.Add(role.empName);
                        //item1.SubItems.Add(role.EmpRoles);
                        item1.SubItems.Add(role.userID);
                        lv.Items.Add(item1);
                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lv;
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
                EmployeeDB edb = new EmployeeDB();
                List<employee> EMPList = edb.getEmployees();
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Emp Id", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Emp Name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Department Name", -2, HorizontalAlignment.Left);
                foreach (employee emp in EMPList)
                {
                    if (emp.empStatus == 1)
                    {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        item1.SubItems.Add(emp.empID.ToString());
                        item1.SubItems.Add(emp.empName.ToString());
                        item1.SubItems.Add(emp.department.ToString());
                        lv.Items.Add(item1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lv;
        }

        //------------------
        public static byte[] getPictureOfEmployee(string empID)
        {
            byte[] imgByte = new byte[0];
            string pic = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select Picture from Employee where EmployeeID = '" + empID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    pic = reader.IsDBNull(0) ? null : reader.GetString(0);
                }
                conn.Close();
                try
                {
                    imgByte = Convert.FromBase64String(pic);
                }
                catch (Exception ex)
                {
                    imgByte = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return imgByte;
        }
        public static Boolean updateEmployeeQualification(string empID, List<EmployeeQualification> employeequalification)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Delete from EmployeeQualification where EmployeeID='" + empID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("delete", "EmployeeQualification", "", updateSQL) +
                Main.QueryDelimiter;
                foreach (EmployeeQualification empqrec in employeequalification)
                {
                    updateSQL = "insert into EmployeeQualification (EmployeeID,Qualification,YearOfPassing,Remarks,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + empqrec.empID + "'," +
                    "'" + empqrec.QualificationID + "'," +
                    empqrec.Year + "," +
                    "'" + empqrec.Remarks + "'," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "EmployeeQualification", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }

        public static Boolean updateEmployeeExperience(string empID, List<EmployeeExperience> employeeexperience)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Delete from EmployeeExperience where EmployeeID='" + empID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("delete", "EmployeeExperience", "", updateSQL) +
                Main.QueryDelimiter;
                foreach (EmployeeExperience empexprec in employeeexperience)
                {
                    updateSQL = "insert into EmployeeExperience (EmployeeID,ExpStartDate,ExpEndDate,Employer,Post,Remarks,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + empexprec.empID + "'," +
                    "'" + Convert.ToDateTime(empexprec.startdate).ToString("yyyy-MM-dd") + "'," +
                    "'" + Convert.ToDateTime(empexprec.enddate).ToString("yyyy-MM-dd") + "'," +
                    "'" + empexprec.employer + "'," +
                    "'" + empexprec.postheld + "'," +
                    "'" + empexprec.Remarks + "'," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";

                    //////// updateSQL = "insert into EmployeeExperience (EmployeeID,ExpStartDate,ExpEndDate,Employer,Post,Remarks,CreateTime,CreateUser)" +
                    ////////"values (" +
                    ////////"'" + empexprec.empID + "'," +
                    ////////"'" + empexprec.startdate+ "'," +
                    ////////"'" + empexprec.enddate+ "'," +
                    ////////"'" + empexprec.employer + "'," +
                    ////////"'" + empexprec.postheld + "'," +
                    ////////"'" + empexprec.Remarks + "'," +
                    ////////"GETDATE()" + "," +
                    ////////"'" + Login.userLoggedIn + "'" + ")";

                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "EmployeeExperience", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        //get Employee DOJ
        public static DateTime getEmployeeDOJ(string empID)
        {
            DateTime dob = DateTime.Parse("01-01-1900");
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select  DOJ" +
                    " from Employee " +
                    "where EmployeeID = '" + empID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    dob = reader.IsDBNull(0) ? DateTime.Parse("01-01-1900") : reader.GetDateTime(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return dob;
        }
        public static string getEmployeeOffice(string empID)
        {
            string oid="";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select  OfficeID" +
                    " from ViewEmployeeLocation " +
                    "where EmployeeID = '" + empID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    oid = reader.IsDBNull(0) ? "" : reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return oid;
        }
        //Employee Selection In GridView
        ////////public List<EmployeeSelection> getEmployeeSelectionList()
        ////////{
        ////////    EmployeeSelection empSel;
        ////////    List<EmployeeSelection> EmployeeList = new List<EmployeeSelection>();
        ////////    try
        ////////    {
        ////////        SqlConnection conn = new SqlConnection(Login.connString);
        ////////        string query = "select a.EmployeeID,a.Name,a.OfficeID,a.OfficeName,b.UserID From ViewEmployeeDetails a, ERPUser b where " +
        ////////            " a.EmployeeID = b.EmployeeID and a.Status = 1 order by a.Name";
        ////////        SqlCommand cmd = new SqlCommand(query, conn);
        ////////        conn.Open();
        ////////        SqlDataReader reader = cmd.ExecuteReader();
        ////////        while (reader.Read())
        ////////        {
        ////////            empSel = new EmployeeSelection();
        ////////            empSel.EmployeeID = reader.GetString(0);
        ////////            empSel.EmployeeName = reader.GetString(1);
        ////////            empSel.OfficeId = reader.IsDBNull(2)?"":reader.GetString(2);
        ////////            empSel.OfficeName = reader.IsDBNull(3) ? "" : reader.GetString(3);
        ////////            empSel.UserID = reader.IsDBNull(4) ? "" : reader.GetString(4);
        ////////            EmployeeList.Add(empSel);
        ////////        }
        ////////        conn.Close();
        ////////    }
        ////////    catch (Exception ex)
        ////////    {
        ////////        MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");

        ////////    }
        ////////    return EmployeeList;
        ////////}

        //Employee Selection In GridView
        public List<EmployeeSelection> getEmployeeSelectionList(string officeid)
        {
            EmployeeSelection empSel;
            List<EmployeeSelection> EmployeeList = new List<EmployeeSelection>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "";
                if (officeid.Equals(string.Empty))
                {
                    query = "select a.EmployeeID,a.Name,a.OfficeID,a.OfficeName,b.UserID From ViewEmployeeDetails a, ERPUser b where " +
                  " a.EmployeeID = b.EmployeeID and a.Status = 1 order by a.Name";
                }
                else
                {
                    query = "select a.EmployeeID,a.Name,a.OfficeID,a.OfficeName,b.UserID From ViewEmployeeDetails a, ERPUser b where " +
                  " a.EmployeeID = b.EmployeeID and a.Status = 1 and a.OfficeID='" + officeid + "' order by a.Name";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    empSel = new EmployeeSelection();
                    empSel.EmployeeID = reader.GetString(0);
                    empSel.EmployeeName = reader.GetString(1);
                    empSel.OfficeId = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    empSel.OfficeName = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    empSel.UserID = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    EmployeeList.Add(empSel);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");

            }
            return EmployeeList;
        }

        public DataGridView getEmployeelistGrid(string officeID)
        {
            DataGridView empGgrid = new DataGridView();
            try
            {
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

                empGgrid.EnableHeadersVisualStyles = false;
                
                empGgrid.AllowUserToAddRows = false;
                empGgrid.AllowUserToDeleteRows = false;
                empGgrid.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                empGgrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
                empGgrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                empGgrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                empGgrid.ColumnHeadersHeight = 27;
                empGgrid.RowHeadersVisible = false;
                empGgrid.Columns.Add(colChk);

                EmployeeDB empDB = new EmployeeDB();
                List<EmployeeSelection> empSElList = empDB.getEmployeeSelectionList(officeID);
               
                empGgrid.DataSource = empSElList;
            }
            catch (Exception ex)
            {
            }

            return empGgrid;
        }
       
    }
}
