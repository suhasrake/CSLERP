using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class accountdaybook
    {
        public string AccountCode { get; set; }
        public string Name { get; set; }
        public string BookType { get; set; }
        public int status { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string currencyID { get; set; }
    }

    class AccountDayBookCodeDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<accountdaybook> getAccountDayBookDetail()
        {
            accountdaybook adb;
            List<accountdaybook> ADBList = new List<accountdaybook>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.AccountCode,b.Name, a.BookType,a.status,a.CreateTime,a.CreateUser,a.CurrencyID " +
                    "from AccountDayBookCode a , AccountCode b " +
                    " where a.AccountCode = b.AccountCode order by AccountCode";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    adb = new accountdaybook();
                    adb.AccountCode = reader.GetString(0);
                    adb.Name = reader.GetString(1);
                    adb.BookType = reader.GetString(2);
                    adb.status = reader.GetInt32(3);
                    adb.CreateTime = reader.GetDateTime(4);
                    adb.CreateUser = reader.GetString(5);
                    adb.currencyID = reader.GetString(6);
                    ADBList.Add(adb);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return ADBList;

        }

        public Boolean updateAccountDayBookDetail(accountdaybook adb)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update AccountDayBookCode set BookType='" + adb.BookType +
                    "',Status=" + adb.status +", CurrencyID='"+adb.currencyID+"'"+
                    " where AccountCode='" + adb.AccountCode + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("update", "AccountDayBookCode", "", updateSQL) +
                  Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean insertAccountDayBookDetail(accountdaybook adb)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into AccountDayBookCode (AccountCode,BookType,CurrencyID,Status,CreateTime,CreateUser)" +
                    " values (" +
                    "'" + adb.AccountCode + "'," +
                     "'" + adb.BookType + "'," +
                      "'" + adb.currencyID + "'," +
                    adb.status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("insert", "AccountDayBookCode", "", updateSQL) +
                  Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean validateACcountDayBookDetail(accountdaybook adb)
        {
            Boolean status = true;
            try
            {
                if (adb.AccountCode.Trim().Length == 0 || adb.AccountCode == null)
                {
                    return false;
                }
                if (adb.BookType.Trim().Length == 0 || adb.BookType == null)
                {
                    return false;
                }
                if (adb.status == 2)
                {
                    return false;
                }
                if (adb.currencyID.Trim().Length == 0 || adb.currencyID == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                return false;
            }
            return status;
        }
        public static void fillAccountCodeCombo(ComboBox cmb, string type)
        {
            cmb.Items.Clear();
            try
            {
                AccountDayBookCodeDB acDB = new AccountDayBookCodeDB();
                accountdaybook adb = new accountdaybook();
                List<accountdaybook> ACDEtail = acDB.getAccountDayBookDetail();
                foreach (accountdaybook accode in ACDEtail)
                {
                    if (accode.BookType.Equals(type) && accode.status == 1)
                    {
                        cmb.Items.Add(accode.AccountCode + "-" + accode.Name);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }

        }
        public List<accountdaybook> getAccountDayBookDetailWRTBookType(string bookType)
        {
            accountdaybook adb;
            List<accountdaybook> ADBList = new List<accountdaybook>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.AccountCode,b.Name, a.BookType,a.status,a.CreateTime,a.CreateUser " +
                    "from AccountDayBookCode a , AccountCode b " +
                    " where a.AccountCode = b.AccountCode and a.BookType = '" + bookType + "' and a.status = 1 order by a.AccountCode";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    adb = new accountdaybook();
                    adb.AccountCode = reader.GetString(0);
                    adb.Name = reader.GetString(1);
                    adb.BookType = reader.GetString(2);
                    adb.status = reader.GetInt32(3);
                    adb.CreateTime = reader.GetDateTime(4);
                    adb.CreateUser = reader.GetString(5);
                    ADBList.Add(adb);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return ADBList;

        }
        public static void fillAccountCodeComboNew(ComboBox cmb, string type)
        {
            cmb.Items.Clear();
            try
            {
                AccountDayBookCodeDB acDB = new AccountDayBookCodeDB();
                accountdaybook adb = new accountdaybook();
                List<accountdaybook> ACDEtail = acDB.getAccountDayBookDetailWRTBookType(type);
                foreach (accountdaybook accode in ACDEtail)
                {
                    Structures.ComboBoxItem cbitem =
                            new Structures.ComboBoxItem(accode.Name, accode.AccountCode);
                    cmb.Items.Add(cbitem);
                    /////cmb.Items.Add(accode.AccountCode + "-" + accode.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        public static string getAcountBookType(string AccCode)
        {
            string booktype = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select BookType " +
                    "from AccountDayBookCode" +
                    " where AccountCode ='" + AccCode + "' and status = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    booktype = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show( System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return booktype;

        }
    }
}
