using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace CSLERP.DBData
{
    //public class ledger
    //{
    //    public string AccountCode { get; set; }
    //    public string AccountName { get; set; }
    //    public string SLName { get; set; }
    //    public string SLCode { get; set; }
    //    public string TransactionAc { get; set; }
    //    public string TransactionACName { get; set; }
    //    public string TransactionSLCode { get; set; }
    //    public string TransactionSLName { get; set; }
    //    public string DocumentID { get; set; }
    //    public decimal DebitAmnt { get; set; }
    //    public decimal CreditAmnt { get; set; }
    //    public int VoucherNo { get; set; }
    //    public DateTime VoucherDate { get; set; }
    //    public string Narration { get; set; }
    //}
    class TrialBalanceDB
    {
        public List<ledger> getFilteredledger(DateTime toDate,string FYID)
        {
            ledger ldg;
            List<ledger> ledger = new List<ledger>();
            try
            {
                string query = "select accode,acname,sum(dramtinr) Debit,sum(cramtinr) Credit,SUM(dramtinr-cramtinr) Balance" +
               " from viewledger where VoucherDate >= (select StartDate from FinancialYear where FYID = '" + FYID + "')" +
               " and VoucherDate <='" + toDate.ToString("yyyy-MM-dd") + "' group by accode,acname";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        ldg = new ledger();
                        ldg.AccountCode = reader.GetString(0);
                        ldg.AccountName = reader.GetString(1);
                        ldg.DebitAmnt = reader.GetDecimal(4); // for storing balance
                        ledger.Add(ldg);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Ledger Details");
            }
            return ledger;
        }
    }
}
