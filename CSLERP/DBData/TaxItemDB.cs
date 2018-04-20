using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{


    class taxitem
    {
        public string TaxItemID { get; set; }
        public string Description { get; set; }
        public string AccountCodeIN { get; set; }
        public string AccountNameIN { get; set; }
        public string AccountCodeOUT { get; set; }
        public string AccountNameOUT { get; set; }
        public int status { get; set; }
        public DateTime Createime { get; set; }
        public string CreateUser { get; set; }
    }

    class TaxItemDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<taxitem> getTaxItems()
        {
            taxitem ti;
            List<taxitem> TaxItems = new List<taxitem>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.TaxItemID, a.Description,a.AccountCodeIN,a.AccountCodeOUT,a.status " +
                    "from TaxItem a order by a.TaxItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ti = new taxitem();
                    ti.TaxItemID = reader.GetString(0);
                    ti.Description = reader.GetString(1);
                    ti.AccountCodeIN = reader.GetString(2);
                    ti.AccountCodeOUT = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    ti.status = reader.GetInt32(4);
                    TaxItems.Add(ti);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Tax Item");

            }
            return TaxItems;

        }

        public Boolean updateTaxItem(taxitem ti)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update TaxItem set description='" + ti.Description +
                    "', AccountCodeIN='" + ti.AccountCodeIN +
                    "', AccountCodeOUT='" + ti.AccountCodeOUT +
                    "',Status=" + ti.status +
                    " where TaxItemID='" + ti.TaxItemID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "TaxItem", "", updateSQL) +
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
        public Boolean insertTaxItem(taxitem ti)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into TaxItem (TaxItemID,Description,AccountCodeIN,AccountCodeOUT,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + ti.TaxItemID + "'," +
                    "'" + ti.Description + "'," +
                    "'" + ti.AccountCodeIN + "'," +
                    "'" + ti.AccountCodeOUT + "'," +
                    ti.status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "TaxItem", "", updateSQL) +
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
        public Boolean validateTaxItem(taxitem ti)
        {
            Boolean status = true;
            try
            {
                if (ti.TaxItemID.Trim().Length == 0 || ti.TaxItemID.Trim() == null)
                {
                    return false;
                }
                if (ti.Description.Trim().Trim().Length == 0 || ti.Description == null)
                {
                    return false;
                }
                if (ti.AccountCodeIN.Trim().Length == 0 || ti.AccountCodeIN == null)
                {
                    MessageBox.Show("Please Select the Account code IN");
                    return false;
                }
                if (ti.AccountCodeOUT.Trim().Length == 0 || ti.AccountCodeOUT == null)
                {
                    MessageBox.Show("Please Select the Account code OUT");
                    return false;
                }
            }
            catch (Exception)
            {
            }
            return status;
        }

        public static void fillTaxItemCombo(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                TaxItemDB taxitemdb = new TaxItemDB();
                List<taxitem> TaxItems = taxitemdb.getTaxItems();
                foreach (taxitem ti in TaxItems)
                {
                    if (ti.status == 1)
                    {
                        cmb.Items.Add(ti.TaxItemID + "-" + ti.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
        }
        public static void fillTaxItemGridViewCombo(DataGridViewComboBoxCell cmb)
        {
            cmb.Items.Clear();
            try
            {
                TaxItemDB taxitemdb = new TaxItemDB();
                List<taxitem> TaxItems = taxitemdb.getTaxItems();
                foreach (taxitem ti in TaxItems)
                {
                    if (ti.status == 1)
                    {
                        cmb.Items.Add(ti.TaxItemID + "-" + ti.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
        }
        public string getaccountname(string accCode)
        {
            string accountname = "";
            try
            {
                SqlConnection con = new SqlConnection(Login.connString);
                string query = "select Name from AccountCode where AccountCode='"+accCode+"' ";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                     accountname = reader.GetString(0);
                }
            }
            catch(Exception ex)
            {
                
            }
            return accountname;
        }
    }
}
