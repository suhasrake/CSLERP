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
    class brentry
    {
        public int rowid { get; set; }
        public string DocumentID { get; set; }
        public int VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public DateTime BankDate { get; set; }

        public string SLcode { get; set; } //slcode
        public string SLName { get; set; }
        public string SLType { get; set; } //slcode
        public string Bankrefno { get; set; }
        public DateTime bankrefdate { get; set; }
        public decimal AmountDebit { get; set; }
        public decimal AmountCredit { get; set; }

    }

    class BREntryDB
    {
        public List<brentry> getFilteredBREntry(int opt, DateTime from, DateTime to, string bankcode)
        {
            brentry br;
            List<brentry> BREntry = new List<brentry>();
            try
            {
                string query1 = " select RowID,DocumentID,VoucherNo,VoucherDate,AccountCode, "+
                                " AccountName,SLCode,SLName,SLType,AmountDebit,AmountCredit,BankReferenceNo,"+
                                " bankReferenceDate ,BankDate from ViewBR" +                           
                               " where VoucherDate >= '"+from.ToString("yyyy-MM-dd")+"' and VoucherDate <= '"+to.ToString("yyyy-MM-dd")+"' "+
                               " and AccountCode = '"+bankcode+"' order by bankReferenceDate,BankReferenceNo  ";

                string query2 = " select RowID,DocumentID,VoucherNo,VoucherDate,AccountCode,AccountName,"+
                                " SLCode,SLName,SLType,AmountDebit,AmountCredit,BankReferenceNo, "+
                                " bankReferenceDate,BankDate from ViewBR "+
                                " where BankDate is NULL and bankReferenceDate!= '' and bankReferenceDate != '' and "+
                                " VoucherDate >= '" + from.ToString("yyyy-MM-dd") + "' and VoucherDate <= '" + to.ToString("yyyy-MM-dd") + "' and " +
                                " AccountCode = '"+bankcode+"' order by bankReferenceDate,BankReferenceNo";

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
                    default:
                        query = "";
                        break;
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    br = new brentry();
                    br.rowid = reader.GetInt32(0);
                    br.DocumentID = reader.GetString(1);
                    br.VoucherNo = reader.GetInt32(2);
                    br.VoucherDate = reader.GetDateTime(3);
                    br.AccountCode = reader.GetString(4);
                    br.AccountName = reader.GetString(5);
                    br.SLcode = reader.GetString(6);
                    br.SLName = reader.GetString(7);
                    br.SLType = reader.GetString(8);
                    br.AmountDebit = reader.GetDecimal(9);
                    br.AmountCredit = reader.GetDecimal(10);
                    br.Bankrefno = reader.GetString(11);
                    br.bankrefdate = reader.GetDateTime(12);
                    if (!reader.IsDBNull(13))
                    {
                        br.BankDate = reader.GetDateTime(13);
                    }
                    else
                    {
                        br.BankDate = DateTime.Parse("01-01-1900");
                    }
                    BREntry.Add(br);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return BREntry;
        }



        public static List<paymentvoucherdetail> getVoucherDetail(paymentvoucher vh)
        {
            paymentvoucherdetail vd;
            List<paymentvoucherdetail> VDetail = new List<paymentvoucherdetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,TemporaryNo, TemporaryDate,AccountCodeDebit,AccountNameDebit,AmountDebit,BillNo,BillDate, " +
                   "ChequeNo,ChequeDate" +
                  " from ViewPaymentVoucher " +
                  "where DocumentID='" + vh.DocumentID + "'" +
                  " and TemporaryNo=" + vh.TemporaryNo +
                  " and TemporaryDate='" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vd = new paymentvoucherdetail();
                    vd.RowID = reader.GetInt32(0);
                    vd.DocumentID = reader.GetString(1);
                    vd.TemporaryNo = reader.GetInt32(2);
                    vd.TemporaryDate = reader.GetDateTime(3).Date;
                    vd.AccountCodeDebit = reader.GetString(4);
                    vd.AccountNameDebit = reader.GetString(5);
                    vd.AmountDebit = reader.GetDecimal(6);
                    vd.BillNo = reader.GetString(7);
                    vd.BillDate = reader.GetDateTime(8);
                    vd.ChequeNo = reader.GetString(9);
                    vd.ChequeDate = reader.GetDateTime(10);
                    VDetail.Add(vd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return VDetail;
        }


        public Boolean updateVoucherForBR(brentry br, int opt)
        {
            Boolean status = true;
            string utString = "";
            string date = "";
            try
            {
                if(br.BankDate==DateTime.MinValue)
                {
                    date = "null";
                }
                else
                {
                    date = "'"+ br.BankDate.ToString("yyyy-MM-dd")+"'";
                }
                if (opt == 1)
                {
                    string updateSQL = "update PaymentVoucherDetail set BankDate=" + date + "" +
                       " where DocumentID='" + br.DocumentID + "'" +
                       " and RowID='" + br.rowid + "'";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "PaymentVoucherDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                    if (!UpdateTable.UT(utString))
                    {
                        status = false;
                    }
                }
                else
                {
                    string updateSQL = "update ReceiptVoucherDetail set BankDate=" + date + "" +
                        " where DocumentID='" + br.DocumentID + "'" +
                          " and RowID='" + br.rowid + "'";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "ReceiptVoucherDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                    if (!UpdateTable.UT(utString))
                    {
                        status = false;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean updateReceiptVoucher(List<brentry> brlist)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                foreach (brentry br in brlist)
                {
                    string updateSQL = "update ReceiptVoucherDetail set BankDate='" + br.BankDate.ToString("yyyy-MM-dd") + "'" +
                        " where DocumentID='" + br.DocumentID + "'" +
                          " and RowID='" + br.rowid + "'";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "ReceiptVoucherDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                }
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

        public Boolean updateVoucherDetail(List<paymentvoucherdetail> VDetails, paymentvoucher vh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Delete from PaymentVoucherDetail where DocumentID='" + vh.DocumentID + "'" +
                    " and TemporaryNo=" + vh.TemporaryNo +
                    " and TemporaryDate='" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "PaymentVoucherDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (paymentvoucherdetail vd in VDetails)
                {
                    updateSQL = "insert into PaymentVoucherDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,AccountCodeDebit,AmountDebit,BillNo,BillDate,ChequeNo,ChequeDate) " +
                    "values ('" + vh.DocumentID + "'," +
                    vh.TemporaryNo + "," +
                    "'" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + vd.AccountCodeDebit + "'," +
                   vd.AmountDebit + "," +
                    "'" + vd.BillNo + "'," +
                    "'" + vd.BillDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + vd.ChequeNo + "'," +
                    "'" + vd.ChequeDate.ToString("yyyy-MM-dd") + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "PaymentVoucherDetail", "", updateSQL) +
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

        public Boolean insertPaymentVoucherHeader(paymentvoucher vh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into PaymentVoucherHeader " +
                    "(DocumentID,CreationMode,ProjectID,OfficeID,TemporaryNo,TemporaryDate,VoucherNo,VoucherDate,VoucherType,BookType," +
                    "AccountCodeCredit,SLType,SLCode,BankTransactionMode,CurrencyID,ExchangeRate," +
                    "VoucherAmount,VoucherAmountINR,Narration," +
                    "Comments,CommentStatus,CreateUser,CreateTime,ForwarderList,DocumentStatus,Status)" +
                    " values (" +
                    "'" + vh.DocumentID + "'," +
                    "1," +
                    "'" + vh.ProjectID + "'," +
                    "'" + vh.OfficeID + "'," +
                    vh.TemporaryNo + "," +
                    "'" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     vh.VoucherNo + "," +
                    "'" + vh.VoucherDate.ToString("yyyy-MM-dd") + "'," +
                      "'" + vh.VoucherType + "'," +
                    "'" + vh.BookType + "'," +
                   "'" + vh.AccountCodeCredit + "'," +
                    "'" + vh.SLType + "'," +
                    "'" + vh.SLCode + "'," +
                    "'" + vh.BankTransactionMode + "'," +
                    "'" + vh.CurrencyID + "'," +
                    vh.ExchangeRate + "," +
                    vh.VoucherAmount + "," +
                    vh.VoucherAmountINR + "," +
                    "'" + vh.Narration + "'," +
                     "'" + vh.Comments + "'," +
                       "'" + vh.CommentStatus + "'," +
                    "'" + Login.userLoggedIn + "'," +
                    "GETDATE()" + "," +
                    "'" + vh.ForwarderList + "'," +
                    vh.DocumentStatus + "," +
                         vh.status + ")";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "PaymentVoucherHeader", "", updateSQL) +
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
        public Boolean deletePaymentVoucherHeader(paymentvoucher vh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "delete PaymentVoucherHeader where DocumentID='" + vh.DocumentID + "'" +
                    " and TemporaryNo=" + vh.TemporaryNo +
                    " and TemporaryDate='" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("delete", "PaymentVoucherHeader", "", updateSQL) +
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

        public Boolean validatePaymentVoucherHeader(paymentvoucher vh)
        {
            Boolean status = true;
            try
            {
                if (vh.DocumentID.Trim().Length == 0 || vh.DocumentID == null)
                {
                    return false;
                }
                if (vh.OfficeID.Trim().Length == 0 || vh.OfficeID == null)
                {
                    return false;
                }
                if (vh.VoucherType.Trim().Length == 0 || vh.VoucherType == null)
                {
                    return false;
                }
                if (vh.BookType.Trim().Length == 0 || vh.BookType == null)
                {
                    return false;
                }
                if (vh.AccountCodeCredit.Trim().Length == 0 || vh.AccountCodeCredit == null)
                {
                    return false;
                }
                if (vh.SLType.Trim().Length == 0 || vh.SLType == null)
                {
                    return false;
                }
                if (vh.SLCode.Trim().Length == 0 || vh.SLCode == null)
                {
                    return false;
                }
                if (vh.CurrencyID.Trim().Length == 0 || vh.CurrencyID == null)
                {
                    return false;
                }
                if (vh.Narration.Trim().Length == 0 || vh.Narration == null)
                {
                    return false;
                }
                if (vh.ExchangeRate == 0)
                {
                    return false;
                }
                if (vh.VoucherAmount == 0)
                {
                    return false;
                }
                if (vh.VoucherAmountINR == 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                return false;
            }
            return status;
        }
        public Boolean forwardPaymentVoucherHeader(paymentvoucher vh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update PaymentVoucherHeader set DocumentStatus=" + (vh.DocumentStatus + 1) +
                    ", forwardUser='" + vh.ForwardUser + "'" +
                    ", commentStatus='" + vh.CommentStatus + "'" +
                    ", ForwarderList='" + vh.ForwarderList + "'" +
                    " where DocumentID='" + vh.DocumentID + "'" +
                    " and TemporaryNo=" + vh.TemporaryNo +
                    " and TemporaryDate='" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "PaymentVoucherHeader", "", updateSQL) +
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

        public Boolean reversePaymentVoucherHeader(paymentvoucher vh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update PaymentVoucherHeader set DocumentStatus=" + vh.DocumentStatus +
                    ", forwardUser='" + vh.ForwardUser + "'" +
                    ", commentStatus='" + vh.CommentStatus + "'" +
                    ", ForwarderList='" + vh.ForwarderList + "'" +
                    " where DocumentID='" + vh.DocumentID + "'" +
                    " and TemporaryNo=" + vh.TemporaryNo +
                    " and TemporaryDate='" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "PaymentVoucherHeader", "", updateSQL) +
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

        public Boolean ApprovePaymentVoucherHeader(paymentvoucher vh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update PaymentVoucherHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + vh.CommentStatus + "'" +
                    ", VoucherNo=" + vh.VoucherNo +
                    ", VoucherDate=convert(date, getdate())" +
                    " where DocumentID='" + vh.DocumentID + "'" +
                    " and TemporaryNo=" + vh.TemporaryNo +
                    " and TemporaryDate='" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "PaymentVoucherHeader", "", updateSQL) +
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
        //public void updateRefNoWisePRDetailInStock(double Quant, int refNo)
        //{
        //    //Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "update Stock set  " +
        //            " PresentStock=" + "( (select PresentStock from Stock where RowID = " + refNo + ")-" + Quant + ")"+
        //            ", PurchaseReturnQuantity=" + "( (select PurchaseReturnQuantity from Stock where RowID = " + refNo + ")+" + Quant + ")" +
        //            " where RowID=" + refNo;
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
        //        Main.QueryDelimiter;

        //        if (!UpdateTable.UT(utString))
        //        {
        //            //status = false;
        //            MessageBox.Show("failed to Update In Reference Number Wise PRDetail in stock");
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("failed to Update In Reference Number Wise PRDetail in stock");
        //        return;
        //    }
        //    //return status;
        //}
        //public Boolean updatePRInStock(List<purchasereturndetail> PRDetails)
        //{
        //    Boolean status = true;
        //   // string utString = "";
        //    try
        //    {
        //        foreach(purchasereturndetail prd in PRDetails)
        //        {
        //            double quant = prd.Quantity;
        //            int RefNo = prd.StockReferenceNo;
        //            updateRefNoWisePRDetailInStock(quant,RefNo);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = false;
        //    }
        //    return status;
        //}
        public static string getUserComments(string docid, int tempno, DateTime tempdate)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from PaymentVoucherHeader where DocumentID='" + docid + "'" +
                        " and TemporaryNo=" + tempno +
                        " and TemporaryDate='" + tempdate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    cmtString = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                cmtString = "";
            }
            return cmtString;
        }
        public List<brentry> getbanklist()
        {
            brentry br;
            List<brentry> BREntry = new List<brentry>();
            try
            {
                SqlConnection con = new SqlConnection(Login.connString);
                string query = "select a.AccountCode,a.Name from AccountCode a,AccountDayBookCode b where a.AccountCode=b.AccountCode and b.BookType='BANKBOOK'";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    br = new brentry();
                    br.AccountCode = reader.GetString(0);
                    br.AccountName = reader.GetString(1);
                    BREntry.Add(br);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return BREntry;
        }
        public static ListView getBankAccountCodeListView()
        {
            ListView lv = new ListView();
            try
            {

                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                BREntryDB ACDb = new BREntryDB();
                List<brentry> acList = ACDb.getbanklist();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("BankAccountcode", -2, HorizontalAlignment.Left);
                lv.Columns.Add("BankAccount Name", -2, HorizontalAlignment.Left);
                foreach (brentry ac in acList)
                {

                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(ac.AccountCode);
                    item1.SubItems.Add(ac.AccountName);
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lv;
        }

    }
}
