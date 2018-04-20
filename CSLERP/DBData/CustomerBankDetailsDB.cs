using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class customerbankdetails
    {
        public int rowID { get; set; }
        public int CustomerRowID { get; set; }
        public string CustomerName { get; set; }
        public string BankID { get; set; }
        public string BankName { get; set; }
        public int BankBranch { get; set; }
        public string BankBranchName { get; set; }
        public string AccountNo { get; set; }
        public int status { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
    }

    class CustomerBankDetailsDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<customerbankdetails> getCustomerBankDetails(int customerid)
        {
            customerbankdetails cbd;
            List<customerbankdetails> cbDetails = new List<customerbankdetails>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.rowid,a.CustomerRowID,b.name,a.bankbranch,c.BranchName,c.bankid,c.bankname, " +
                    " a.AccountNo,a.Status" +
                    " from CustomerBankDetail a,Customer b,ViewBankBranchDetails c " +
                    " where a.CustomerRowID=b.RowID and a.BankBranch=c.branchid and a.CustomerRowID='" + customerid+"'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cbd = new customerbankdetails();
                    cbd.rowID = reader.GetInt32(0);
                    cbd.CustomerRowID = reader.GetInt32(1);
                    cbd.CustomerName = reader.GetString(2);
                    cbd.BankBranch = reader.GetInt32(3);
                    cbd.BankBranchName = reader.GetString(4);
                    cbd.BankID = reader.GetString(5);
                    cbd.BankName = reader.GetString(6);
                    
                    cbd.AccountNo = reader.GetString(7);
                    cbd.status = reader.GetInt32(8);
                    cbDetails.Add(cbd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return cbDetails;
        }

        public Boolean updateCustomerBankDetails(string customeRowid, List<customerbankdetails> cbdList)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Delete from CustomerBankDetail where CustomerRowID='" + customeRowid + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("delete", "CustomerBankDetail", "", updateSQL) +
                Main.QueryDelimiter;
                foreach (customerbankdetails cbd in cbdList)
                {
                    updateSQL = "insert into CustomerBankDetail (CustomerRowID,BankBranch,AccountNo,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + cbd.CustomerRowID + "'," +
                    "" + cbd.BankBranch + "," +
                    "'" + cbd.AccountNo + "'," +
                    cbd.status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "CustomerBankDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                }
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
    }
}
