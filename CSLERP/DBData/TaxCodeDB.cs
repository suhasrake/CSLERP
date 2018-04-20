using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class taxcode
    {
        public string TaxCode { get; set; }
        public string Description { get; set; }
        public int status { get; set; }
        public DateTime Createime { get; set; }
        public string CreateUser { get; set; }
    }


    class TaxCodeDB
    {
        ActivityLogDB alDB = new ActivityLogDB();

        public List<taxcode> getTaxCode()
        {
            taxcode tc;
            List<taxcode> TaxCodes = new List<taxcode>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select TaxCode, Description,status " +
                    "from Taxcode  order by Taxcode";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tc = new taxcode();
                    tc.TaxCode = reader.GetString(0);
                    tc.Description = reader.GetString(1);
                    tc.status = reader.GetInt32(2);
                    TaxCodes.Add(tc);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Tax Code");
            }
            return TaxCodes;
        }

   
        public Boolean updateTaxCode(taxcode tc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update TaxCode set Description='" + tc.Description +
                    "', Status=" + tc.status +
                    " where TaxCode='" + tc.TaxCode + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "TaxCode", "", updateSQL) +
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

        public Boolean insertTaxCode(taxcode tc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into TaxCode (TaxCode,Description,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + tc.TaxCode + "'," +
                    "'" + tc.Description + "'," +
                    tc.status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "TaxCode", "", updateSQL) +
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
        public Boolean validateTaxCode(taxcode tc)
        {
            Boolean status = true;
            try
            {
                if (tc.TaxCode.Trim().Length == 0 || tc.TaxCode == null)
                {
                    return false;
                }
                if (tc.Description.Trim().Length == 0 || tc.Description == null)
                {
                    return false;
                }
              
            }
            catch (Exception)
            {
            }
            return status;
        }
        public static void fillTaxCodeCombo(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                TaxCodeDB tcdb = new TaxCodeDB();
                List<taxcode> TaxCodes = tcdb.getTaxCode();
                foreach (taxcode tc in TaxCodes)
                {
                    cmb.Items.Add(tc.TaxCode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }

        }
        public static void fillTaxCodeGridViewCombo(DataGridViewComboBoxCell cmb, string CategoryName)
        {
            cmb.Items.Clear();
            try
            {
                TaxCodeDB tcdb = new TaxCodeDB();
                List<taxcode> TaxCodes = tcdb.getTaxCode();
                foreach (taxcode tc in TaxCodes)
                {
                    if (tc.status == 1)
                    {
                        Structures.GridViewComboBoxItem ch =
                            new Structures.GridViewComboBoxItem(tc.Description, tc.TaxCode);
                        cmb.Items.Add(ch);
                    }
                }
                cmb.DisplayMember = "Name";  // Name Property will show(Editing)
                cmb.ValueMember = "Value";  // Value Property will save(Saving)
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }

        }
    }
}
