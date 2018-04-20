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
    class financialyear
    {
        public string fyID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int status { get; set; }
        public int documentStatus { get; set; }
        public int IsCurrentFY { get; set; }
    }

    class FinancialYearDB
    {
        
        public static List<financialyear> getFinancialYear()
        {
            financialyear fyrec;
            List<financialyear> FYears = new List<financialyear>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "Select fyid,startdate,enddate,status,documentstatus,createuser,createtime,IsCurrentFY from Financialyear order by fyid";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    fyrec = new financialyear();
                    fyrec.fyID = reader.GetString(0);
                    fyrec.startDate = reader.GetDateTime(1);
                    fyrec.endDate = reader.GetDateTime(2);
                    fyrec.status = reader.GetInt32(3);
                    fyrec.documentStatus = reader.GetInt32(4);
                    fyrec.IsCurrentFY = reader.GetInt32(7);
                    FYears.Add(fyrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return FYears;

        }
     
        public Boolean updateFinancialYear(financialyear fyrec)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update FinancialYear set startDate='" +
                    (fyrec.startDate.ToString("yyyyMMdd HH:mm:ss")) +
                    "',EndDate='" + (fyrec.endDate.ToString("yyyyMMdd HH:mm:ss")) +
                    "',Status=" + fyrec.status + ", IsCurrentFY = " + fyrec.IsCurrentFY +
                    " where FYID='" + fyrec.fyID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "FinancialYear", "", updateSQL) +
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
        public Boolean insertFinancialYear(financialyear fyrec)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into FinancialYear (FYID,startdate,enddate,status,documentstatus,CreateTime,CreateUser) values (" +
                    "'" + fyrec.fyID + "'," +
                    "'" + (fyrec.startDate.ToString("yyyyMMdd HH:mm:ss")) + "'," +
                     "'" + (fyrec.endDate.ToString("yyyyMMdd HH:mm:ss")) + "'," +
                    fyrec.status + "," + fyrec.documentStatus + "," +
                     "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("insert", "FinancialYear", "", updateSQL) +
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

        public Boolean LockFinancialYear(string fyID)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update FinancialYear set documentstatus = 99";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("insert", "FinancialYear", "", updateSQL) +
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

        public Boolean validateFinancialYear(financialyear fyrec)
        {
            Boolean status = true;
            try
            {
                if (fyrec.fyID.Trim().Length == 0 || fyrec.fyID == null)
                {
                    return false;
                }
                if (fyrec.startDate >= fyrec.endDate || 
                    fyrec.startDate.Day != 1 || 
                    fyrec.endDate.Day != 31 ||
                    fyrec.startDate.Month != 4 ||
                    fyrec.endDate.Month != 3 ||
                    (fyrec.endDate.Year - fyrec.startDate.Year) != 1)
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
        public Boolean verifyFinancialYear(string fyear)
        {
            Boolean status = true;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "Select fyid,startdate,enddate,status,documentstatus,createuser,createtime "+
                    " from Financialyear where FYID = '"+fyear+ "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int FYstatus = reader.GetInt32(3);
                    int documentStatus = reader.GetInt32(4);
                    if (!(FYstatus == 1 && documentStatus == 99))
                    {
                        status = false;
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return status;
        }

        public static string getFinancialYearDates(string fyear)
        {
            string fDates = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "Select fyid,startdate,enddate,status,documentstatus,createuser,createtime " +
                    " from Financialyear where FYID = '" + fyear + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    fDates = reader.GetDateTime(1).ToString("yyyy-MM-dd") + Main.delimiter1 +
                         reader.GetDateTime(2).ToString("yyyy-MM-dd");
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("getFinancialYearDates() : " + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return fDates;
        }

        public static String getCurrentFinancialYear()
        {
            string currentFY = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "Select fyid,startdate,enddate,status,documentstatus " +
                    " from Financialyear where startdate <= convert(date, getdate())" +
                    " and enddate >= convert(date, getdate()) and IsCurrentFY=1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int FYstatus = reader.GetInt32(3);
                    int documentStatus = reader.GetInt32(4);
                    if (!(FYstatus == 1 && documentStatus == 99))
                    {
                        currentFY="";
                    }
                    else
                    {
                        currentFY = reader.GetString(0)+ Main.delimiter2 + 
                            reader.GetDateTime(1).ToString("yyyy-MM-dd")+ Main.delimiter2 + 
                            reader.GetDateTime(2).ToString("yyyy-MM-dd");
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return currentFY;
        }
        public static void fillFYIDCombo(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                List<financialyear> FYIDs = getFinancialYear();
                foreach (financialyear fy in FYIDs)
                {
                    if (fy.status == 1)
                    {
                        cmb.Items.Add(fy.fyID + " : " + fy.startDate.ToShortDateString()+" : "+
                            fy.endDate.ToShortDateString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
        }
        public static void fillFYIDComboNew(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                List<financialyear> FYIDs = getFinancialYear();
                foreach (financialyear fy in FYIDs)
                {
                    if (fy.status == 1)
                    {
                        cmb.Items.Add(fy.fyID + " : " + fy.startDate.ToString("dd-MM-yyyy") + " : " +
                            fy.endDate.ToString("dd-MM-yyyy"));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        public static Boolean verifyCurrentFYStatus(string fyid)
        {
            Boolean status = true;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "Select count(*) " +
                    " from FinancialYear where IsCurrentFY = 1 and FYID != '" + fyid + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int count = reader.GetInt32(0);
                    if (count > 0)
                    {
                        status = false;
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
    }
}
