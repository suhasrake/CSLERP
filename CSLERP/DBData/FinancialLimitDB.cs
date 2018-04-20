using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class financiallimit
    {
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public double FinancialLimit { get; set; }
        public int DocumentStatus { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
    }

    class FinancialLimitDB
    {
        //ActivityLogDB alDB = new ActivityLogDB();
        public List<financiallimit> getEmpFinancialLimit()
        {
            financiallimit flim;
            List<financiallimit> fLimList = new List<financiallimit>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.DocumentID, b.DocumentName,a.EmployeeID,c.Name,a.FinancialLimit," +
                    "a.status from EmployeeFinancialLimit a, Document b, " +
                    "Employee c where a.DocumentID=b.DocumentID and " +
                    "a.EmployeeID=c.EmployeeID " +
                    "order by a.DocumentID,c.Name";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    flim = new financiallimit();
                    flim.DocumentID = reader.GetString(0);
                    flim.DocumentName = reader.GetString(1);
                    flim.EmployeeID = reader.GetString(2);
                    flim.EmployeeName = reader.GetString(3);
                    flim.FinancialLimit = reader.GetDouble(4);
                    flim.DocumentStatus = reader.GetInt32(5);
                    fLimList.Add(flim);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return fLimList;

        }

        public Boolean updateFinancialLimit(financiallimit flim, financiallimit prevflim)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update EmployeeFinancialLimit set FinancialLimit = " + flim.FinancialLimit + "," +
                    " Status=" + flim.DocumentStatus +
                    " where DocumentID='" + prevflim.DocumentID + "'" +
                    " and EmployeeID='" + prevflim.EmployeeID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "EmployeeFinancialLimit", "", updateSQL) +
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
        public Boolean checkEmployeeFinancialLimit(string docID, string empID, double ProductValue, double TaxAmount)
        {
            Boolean status = false;
            double amt = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                 string query = "select FinancialLimit " +
                    "from EmployeeFinancialLimit  where DocumentID='" + docID + "' and EmployeeID='" + empID + "' and Status=1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    amt = reader.GetDouble(0);
                }
                conn.Close();
                if (amt >= ProductValue)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean insertFinancialLimit(financiallimit flim)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into EmployeeFinancialLimit (DocumentID,EmployeeID,FinancialLimit,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + flim.DocumentID + "'," +
                    "'" + flim.EmployeeID + "'," +
                    flim.FinancialLimit + "," +
                    flim.DocumentStatus + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "EmployeeFinancialLimit", "", updateSQL) +
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
        public Boolean validateFinancialLimit(financiallimit flim)
        {
            Boolean status = true;
            try
            {
                if (flim.DocumentID.Trim().Length == 0 || flim.DocumentID == null)
                {
                    return false;
                }
                if (flim.EmployeeID.Trim().Length == 0 || flim.EmployeeID == null)
                {
                    return false;
                }
                if (flim.FinancialLimit == 0)
                {
                    return false;
                }
                if (flim.DocumentStatus == 0)
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
        

    }
}

