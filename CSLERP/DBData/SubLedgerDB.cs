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
    public class sledger
    {
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string SLName { get; set; }
        public string SLCode { get; set; }
        public string TransactionAc { get; set; }
        public string TransactionACName { get; set; }
        public string TransactionSLCode { get; set; }
        public string TransactionSLName { get; set; }
        public string DocumentID { get; set; }
        public decimal DebitAmnt { get; set; }
        public decimal CreditAmnt { get; set; }
        public int VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public string Narration { get; set; }
    }
    class SubLedgerDB
    {
        public List<sledger> getFilteredsledger(DateTime fromDate, DateTime toDate, string ACCode, string SLCode, string SLType)
        {
            sledger ldg;
            List<sledger> ledger = new List<sledger>();
            try
            {
                string query = "select DocumentID,VoucherNo,VoucherDate,ACName,Narration,DrAmtINR,CrAmtINR from ViewLedgerCustomer" +
                                 " where SLType='" + SLType + "' and SLCode='" + SLCode + "' and VoucherDate >= '" + fromDate.ToString("yyyy-MM-dd") + "' and" +
                                 " VoucherDate <='" + toDate.ToString("yyyy-MM-dd") + "' and " +
                                 "AcCode='" + ACCode + "' order by VoucherDate,DocumentID, VoucherNo ";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        ldg = new sledger();
                        ldg.DocumentID = reader.GetString(0);
                        ldg.VoucherNo = reader.GetInt32(1);
                        ldg.VoucherDate = reader.GetDateTime(2);
                        ldg.TransactionACName = reader.GetString(3);
                        ldg.Narration = reader.GetString(4);
                        ldg.DebitAmnt = reader.GetDecimal(5);
                        ldg.CreditAmnt = reader.GetDecimal(6);
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

        public List<sledger> getsledger(DateTime FYStartDate, DateTime fromDate, DateTime toDate, string SLCode, string SLType)
        {
            decimal ob = 0;
            sledger ldg;
            List<sledger> ledger = new List<sledger>();
            try
            {
                string query = "select sum(DrAmtINR-CrAmtINR) Balance from ViewLedgerCustomer" +
                                 " where SLType='" + SLType + "' and SLCode='" + SLCode + "' and  VoucherDate >= '" + FYStartDate.ToString("yyyy-MM-dd") + "'" +
               " and VoucherDate <'" + fromDate.ToString("yyyy-MM-dd") + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ob = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                    ldg = new sledger();
                    ldg.DocumentID = "OB";
                    ldg.VoucherNo = 0;
                    ldg.VoucherDate = fromDate;
                    ldg.TransactionACName = "Opening Balance";
                    ldg.Narration = "";
                    if (ob >= 0)
                    {
                        ldg.DebitAmnt = ob;
                        ldg.CreditAmnt = 0;
                    }
                    else
                    {
                        ldg.CreditAmnt = ob * -1;
                        ldg.DebitAmnt = 0;
                    }

                    ledger.Add(ldg);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                ////MessageBox.Show("Error querying Ledger Details");
            }

            try
            {
                string query = "select DocumentID,VoucherNo,VoucherDate,ACName,Narration,DrAmtINR,CrAmtINR from ViewLedgerCustomer" +
                                 " where SLType='" + SLType + "' and SLCode='" + SLCode + "' and VoucherDate >= '" + fromDate.ToString("yyyy-MM-dd") + "' and" +
                                 " VoucherDate <='" + toDate.ToString("yyyy-MM-dd") + "'" +
                                 " order by VoucherDate,DocumentID, VoucherNo ";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        ldg = new sledger();
                        ldg.DocumentID = reader.GetString(0);
                        ldg.VoucherNo = reader.GetInt32(1);
                        ldg.VoucherDate = reader.GetDateTime(2);
                        ldg.TransactionACName = reader.GetString(3);
                        ldg.Narration = reader.GetString(4);
                        ldg.DebitAmnt = reader.GetDecimal(5);
                        ldg.CreditAmnt = reader.GetDecimal(6);
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


        /////New Modification
        public static List<sledger> getPartyWiseDrCrTotTillFromDate(DateTime FYStartDate, DateTime fromDate)
        {
            sledger ldg;
            List<sledger> ledger = new List<sledger>();
            try
            {
                string query = "select SLCode,SLName,sum(DrAmtINR),sum(CrAmtINR) from ViewLedgerCustomer " +
                                 " where  VoucherDate >= '" + FYStartDate.ToString("yyyy-MM-dd") + "' and" +
                                 " VoucherDate < '" + fromDate.ToString("yyyy-MM-dd") + "'  and DocumentID <> 'OB' group by SLCode, SLName";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        ldg = new sledger();
                        ldg.SLCode = reader.GetString(0);
                        ldg.SLName = reader.GetString(1);
                        ldg.DebitAmnt = reader.GetDecimal(2); //Debit total
                        ldg.CreditAmnt = reader.GetDecimal(3); //Credit total
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
        public static List<sledger> getPartyWiseDrCrTotWithinPeriod(DateTime fromDate, DateTime toDate)
        {
            sledger ldg;
            List<sledger> ledger = new List<sledger>();
            try
            {
                string query = "select SLCode,SLName,sum(DrAmtINR),sum(CrAmtINR) from ViewLedgerCustomer " +
                                 " where  VoucherDate >= '" + fromDate.ToString("yyyy-MM-dd") + "' and" +
                                 " VoucherDate <= '" + toDate.ToString("yyyy-MM-dd") + "' and DocumentID <> 'OB' group by SLCode, SLName";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        ldg = new sledger();
                        ldg.SLCode = reader.GetString(0);
                        ldg.SLName = reader.GetString(1);
                        ldg.DebitAmnt = reader.GetDecimal(2); //Debit total
                        ldg.CreditAmnt = reader.GetDecimal(3); //Credit total
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
        public static List<sledger> GetLedgerDetailsPerParty(string SlCode, DateTime fromDate, DateTime toDate)
        {
            sledger ldg;
            List<sledger> ledger = new List<sledger>();
            try
            {
                string query = "select DocumentID,VoucherNo,VoucherDate,ACName,Narration,DrAmtINR,CrAmtINR from ViewLedgerCustomer" +
                                 " where  VoucherDate >= '" + fromDate.ToString("yyyy-MM-dd") + "' and" +
                                 " VoucherDate <='" + toDate.ToString("yyyy-MM-dd") + "' and " +
                                 "SLCode='" + SlCode + "' and DocumentID <> 'OB' order by VoucherDate ";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        ldg = new sledger();
                        ldg.DocumentID = reader.GetString(0);
                        ldg.VoucherNo = reader.GetInt32(1);
                        ldg.VoucherDate = reader.GetDateTime(2);
                        ldg.TransactionACName = reader.GetString(3);
                        ldg.Narration = reader.GetString(4);
                        ldg.DebitAmnt = reader.GetDecimal(5);
                        ldg.CreditAmnt = reader.GetDecimal(6);
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
