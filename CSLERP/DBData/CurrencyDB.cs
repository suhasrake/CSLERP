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
    class currency
    {
        public string CurrencyID { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public int status { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
    }

    class CurrencyDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public  List<currency> getCurrencies()
        {
            currency curr;
            List<currency> Currencies = new List<currency>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select CurrencyID, Name,Symbol,status "+
                    "from Currency order by CurrencyID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    curr = new currency();
                    curr.CurrencyID = reader.GetString(0);
                    curr.name = reader.GetString(1);
                    curr.symbol = reader.GetString(2);
                    curr.status = reader.GetInt32(3);
                    Currencies.Add(curr);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return Currencies;
            
        }
 
        public Boolean updateCurrency(currency curr )
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update Currency set name='"+curr.name +
                    "',Symbol='" + curr.symbol +
                    "',Status=" +curr.status+
                    " where CurrencyID='" + curr.CurrencyID+"'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "Currency", "", updateSQL) +
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
        public Boolean insertCurrency(currency curr)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into Currency (CurrencyID,Name,Symbol,Status,CreateTime,CreateUser)"+
                    "values (" +
                    "'" + curr.CurrencyID + "'," +
                     "'" + curr.name + "'," +
                     "'" + curr.symbol + "'," +
                    curr.status+","+
                    "GETDATE()"+","+
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "Currency", "", updateSQL) +
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
        public Boolean validateCurrency(currency curr)
        {
            Boolean status = true;
            try
            {
                if (curr.CurrencyID.Trim().Length == 0 || curr.CurrencyID == null)
                {
                    return false;
                }
                if (curr.name.Trim().Length == 0 || curr.name == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return status;
        }
        public static void fillCurrencyCombo(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                CurrencyDB dbrecord = new CurrencyDB();
                List<currency> Currencies = dbrecord.getCurrencies();
                foreach (currency curr in Currencies)
                {
                    cmb.Items.Add(curr.CurrencyID + "-" + curr.name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }

        }
        public static void fillCurrencyComboNew(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                CurrencyDB dbrecord = new CurrencyDB();
                List<currency> Currencies = dbrecord.getCurrencies();
                foreach (currency curr in Currencies)
                {
                    ////////cmb.Items.Add(curr.CurrencyID + "-" + curr.name);
                    Structures.ComboBoxItem cbitem =
                            new Structures.ComboBoxItem(curr.name, curr.CurrencyID);
                    cmb.Items.Add(cbitem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }

        }
    }
}
