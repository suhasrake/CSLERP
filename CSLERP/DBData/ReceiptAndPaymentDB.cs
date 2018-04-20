using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{

    //class stockgroup
    //{
    //    public int RowID { get; set; }
    //    public int GroupLevel { get; set; }
    //    public string GroupCode { get; set; }
    //    public string GroupDescription { get; set; }
    //    public DateTime CreateTime { get; set; }
    //    public string CreateUser { get; set; }
    //}
    class ReceiptAndPaymentDB
    {
        public static string getTotalreceiptAndTotalValue(DateTime Fromdate, DateTime TODate)
        {
            decimal receiptTotal = 0;
            decimal paymentTotal = 0;
            try
            {
                string query = "select DrAmtINR,CrAmtINR " +
                    " from ViewReceiptsAndPayments where VoucherDate >= '" + Fromdate.ToString("yyyy-MM-dd") +
                    "' and VoucherDate <= '" +TODate.ToString("yyyy-MM-dd") + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    paymentTotal = paymentTotal + reader.GetDecimal(0);
                    receiptTotal = receiptTotal + reader.GetDecimal(1); 
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Voucher and payment Details");
            }
            return paymentTotal.ToString() + ":" + receiptTotal.ToString() ;
        }
        public static List<ledger> getFilteredPaymentAndReceiptDetails(int opt, DateTime FromDate, DateTime ToDate)
        {
            ledger led;
            List<ledger> LedgerList = new List<ledger>();
            try
            {
                string query1 = "select AcCode,AcName,SUM(DrAmtINR) Debit,SUM(CrAmtINR) Credit " +
                    "from ViewReceiptsAndPayments where VoucherDate >= '" + FromDate.ToString("yyyy-MM-dd") +
                    "' and VoucherDate <= '" + ToDate.ToString("yyyy-MM-dd") + "'  group by AcCode,AcName ";
                string query2 = "select SLCode,SLName,SUM(DrAmtINR) Debit,SUM(CrAmtINR) Credit " +
                    "from ViewReceiptsAndPayments where VoucherDate >= '" + FromDate.ToString("yyyy-MM-dd") +
                    "' and VoucherDate <= '" + ToDate.ToString("yyyy-MM-dd") + "'  group by SLCode,SLName";
                string query3 = "select VoucherDate,SUM(DrAmtINR) Debit,SUM(CrAmtINR) Credit " +
                  "from ViewReceiptsAndPayments where VoucherDate >= '" + FromDate.ToString("yyyy-MM-dd") +
                  "' and VoucherDate <= '" + ToDate.ToString("yyyy-MM-dd") + "'  group by VoucherDate";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "";
                switch (opt)
                {
                    case 1:
                        query = query1;
                        break;
                    case 2:
                        query = query2;
                        break;
                    case 3:
                        query = query3;
                        break;
                    default:
                        query = "";
                        break;
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    led = new ledger();

                    if (opt != 3)
                    {
                        led.AccountCode = reader.IsDBNull(0) ? "--" : reader.GetString(0);
                        led.AccountName = reader.IsDBNull(1) ? "--" : reader.GetString(1);
                        led.DebitAmnt = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2);
                        led.CreditAmnt = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3);
                    }
                    else
                    {
                        led.VoucherDate = reader.GetDateTime(0);
                        led.DebitAmnt = reader.GetDecimal(1);
                        led.CreditAmnt = reader.GetDecimal(2);
                    }
                   
                    LedgerList.Add(led);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("getFilteredPaymentAndReceiptDetails() : Error querying List Details - "+ex.ToString());
            }
            return LedgerList;
        }
        public static List<ledger> getDocumentWiseDetail(ledger led, int opt,DateTime FromDate,DateTime ToDate)
        {
            ledger ldgr = new ledger();
            List<ledger> LedgerList = new List<ledger>();
            try
            {
                string query1 = "select VoucherNo,VoucherDate,DocumentID,DrAmtINR,CrAmtINR " +
                    "from ViewReceiptsAndPayments where AcCode = '" + led.AccountCode +
                    "' and AcName = '" + led.AccountName + "'"+ " and VoucherDate >= '" + FromDate.ToString("yyyy-MM-dd") +
                    "' and VoucherDate <= '" + ToDate.ToString("yyyy-MM-dd") + "'" ;
                string query2 = "select VoucherNo,VoucherDate,DocumentID,sum(DrAmtINR) DrAmtINR,sum(CrAmtINR) CrAmINRt " +
                    "from ViewReceiptsAndPayments where SLCode = '" + led.SLCode +
                    "' and SLName = '" + led.SLName + "'" + " and VoucherDate >= '" + FromDate.ToString("yyyy-MM-dd") +
                    "' and VoucherDate <= '" + ToDate.ToString("yyyy-MM-dd") + "' group by VoucherNo,VoucherDate,DocumentID";
                string query3 = "select VoucherNo,VoucherDate,DocumentID,sum(DrAmtINR) DrAmtINR,sum(CrAmtINR) CrAmtINR " +
                  "from ViewReceiptsAndPayments where VoucherDate = '" + led.VoucherDate.ToString("yyyy-MM-dd") + "'"
                  + " and VoucherDate >= '" + FromDate.ToString("yyyy-MM-dd") +
                    "' and VoucherDate <= '" + ToDate.ToString("yyyy-MM-dd") + "' group by VoucherNo,VoucherDate,DocumentID";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "";
                switch (opt)
                {
                    case 1:
                        query = query1;
                        break;
                    case 2:
                        query = query2;
                        break;
                    case 3:
                        query = query3;
                        break;
                    default:
                        query = "";
                        break;
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ldgr = new ledger();
                    ldgr.VoucherNo = reader.GetInt32(0);
                    ldgr.VoucherDate = reader.GetDateTime(1);
                    ldgr.DocumentID = reader.GetString(2); 
                    ldgr.DebitAmnt = reader.GetDecimal(3);
                    ldgr.CreditAmnt = reader.GetDecimal(4);
                    LedgerList.Add(ldgr);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("getDocumentWiseDetail() : Error querying List Details - " + ex.ToString());
            }
            return LedgerList;
        }
    }
}
