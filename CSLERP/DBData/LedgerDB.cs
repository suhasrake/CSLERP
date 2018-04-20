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
    public class ledger
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
    class LedgerDB
    {
        public List<ledger> getFilteredledger(DateTime FYStartDate, DateTime fromDate,DateTime toDate,string ACCode)
        {
            //--------------- find balance as on from date
            decimal ob = 0;
            ledger ldg;
            List<ledger> ledger = new List<ledger>();
            try
            {
                string query = "select SUM(dramtinr-cramtinr) Balance" +
               " from viewledger where VoucherDate = '" + FYStartDate.ToString("yyyy-MM-dd") + "'" +
               " and AcCode='" + ACCode +"' and DocumentID= 'OB'";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ob = reader.IsDBNull(0)?0:reader.GetDecimal(0);
                    ldg = new ledger();
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
                        ldg.CreditAmnt = ob*-1;
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
            //---------------
          
            try
            {
                string query = "select DocumentID,VoucherNo,VoucherDate,TransACName,Narration,DrAmtINR,CrAmtINR from ViewLedger"+
                                 " where  VoucherDate >= '"+ fromDate.ToString("yyyy-MM-dd" )+ "' and"+
                                 " VoucherDate <='"+ toDate.ToString("yyyy-MM-dd")+ "' and "+
                                 "AcCode='" + ACCode + "' and documentID <> 'OB' order by VoucherDate,DocumentID, VoucherNo ";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        ldg = new ledger();
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

        public static List<ledger> getAccountWiseDrCrTotTillFromDate(DateTime FYStartDate, DateTime fromDate)
        {
            ledger ldg;
            List<ledger> ledger = new List<ledger>();
            try
            {
                string query = "select AcCode,AcName,sum(DrAmtINR),sum(CrAmtINR) from ViewLedger " +
                                 " where  VoucherDate >= '" + FYStartDate.ToString("yyyy-MM-dd") + "' and" +
                                 " VoucherDate < '" + fromDate.ToString("yyyy-MM-dd") + "'  and DocumentID <> 'OB' group by AcCode, AcName";
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
        public static List<ledger> getAccountWiseDrCrTotWithinPeriod(DateTime fromDate, DateTime toDate)
        {
            ledger ldg;
            List<ledger> ledger = new List<ledger>();
            try
            {
                string query = "select AcCode,AcName,sum(DrAmtINR),sum(CrAmtINR) from ViewLedger " +
                                 " where  VoucherDate >= '" + fromDate.ToString("yyyy-MM-dd") + "' and" +
                                 " VoucherDate <= '" + toDate.ToString("yyyy-MM-dd") + "' and DocumentID <> 'OB' group by AcCode, AcName";
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

        public static List<ledger> GetLedgerDetailsPerCustomer(string ACCode, DateTime fromDate, DateTime toDate)
        {
            ledger ldg;
            List<ledger> ledger = new List<ledger>();
            try
            {
                string query = "select DocumentID,VoucherNo,VoucherDate,TransACName,Narration,DrAmtINR,CrAmtINR from ViewLedger" +
                                 " where  VoucherDate >= '" + fromDate.ToString("yyyy-MM-dd") + "' and" +
                                 " VoucherDate <='" + toDate.ToString("yyyy-MM-dd") + "' and " +
                                 "AcCode='" + ACCode + "' and DocumentID <> 'OB' order by VoucherDate ";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        ldg = new ledger();
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
        //    DataGridView grdLdg = new DataGridView();
        //    try
        //    {
        //        string[] strColArr = { "DocumentID", "VoucherNo","VoucherDate","TransAcName", "Narration",
        //            "DrAmountINR","CrAmountINR"};
        //        DataGridViewTextBoxColumn[] colArr = {
        //            new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
        //            new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
        //        };

        //        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        //        dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        //        dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
        //        dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //        dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
        //        dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        //        dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        //        dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
        //        grdLdg.EnableHeadersVisualStyles = false;
        //        grdLdg.AllowUserToAddRows = false;
        //        grdLdg.AllowUserToDeleteRows = false;
        //        grdLdg.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
        //        grdLdg.BorderStyle = System.Windows.Forms.BorderStyle.None;
        //        grdLdg.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        //        grdLdg.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
        //        grdLdg.ColumnHeadersHeight = 27;
        //        grdLdg.RowHeadersVisible = false;
        //        grdLdg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        //        foreach (string str in strColArr)
        //        {
        //            int index = Array.IndexOf(strColArr, str);
        //            colArr[index].Name = str;
        //            colArr[index].HeaderText = str;
        //            colArr[index].ReadOnly = true;
        //            if (index == 2)
        //                colArr[index].DefaultCellStyle.Format = "dd-MM-yyyy";
        //            if (index == 0)
        //                colArr[index].Width = 150;
        //            else if (index == 3)
        //                colArr[index].Width = 250;
        //            else if (index == 4)
        //                colArr[index].Width = 200;
        //            else
        //                colArr[index].Width = 100;
        //            grdLdg.Columns.Add(colArr[index]);
        //        }

        //        foreach (ledger ldg in ldgList)
        //        {
        //            grdLdg.Rows.Add();
        //            grdLdg.Rows[grdLdg.Rows.Count - 1].Cells[strColArr[0]].Value = ldg.DocumentID;
        //            grdLdg.Rows[grdLdg.Rows.Count - 1].Cells[strColArr[1]].Value = ldg.VoucherNo;
        //            grdLdg.Rows[grdLdg.Rows.Count - 1].Cells[strColArr[2]].Value = ldg.VoucherDate;
        //            grdLdg.Rows[grdLdg.Rows.Count - 1].Cells[strColArr[3]].Value = ldg.TransactionACName;
        //            grdLdg.Rows[grdLdg.Rows.Count - 1].Cells[strColArr[4]].Value = ldg.Narration;
        //            grdLdg.Rows[grdLdg.Rows.Count - 1].Cells[strColArr[5]].Value = ldg.DebitAmnt;
        //            grdLdg.Rows[grdLdg.Rows.Count - 1].Cells[strColArr[6]].Value = ldg.CreditAmnt;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return grdLdg;
        //}

        public static decimal getTotalBookBalanceOfBankAc(DateTime fystartdate, DateTime todate, string bankAcCode)
        {
            decimal total = 0;
            try
            {
                string query = "select AcCode,AcName,SUM(DrAmtINR)-SUM(CrAmtINR) as sub from ViewLedger where AcCode = '" + bankAcCode + "'" +
                                 " and VoucherDate >= '" + fystartdate.ToString("yyyy-MM-dd") + "' and" +
                                 " VoucherDate <= '" + todate.ToString("yyyy-MM-dd") + "' group by AcCode, AcName";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    total = reader.GetDecimal(2);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Ledger book balance");
            }
            return total;
        }
    }
}
