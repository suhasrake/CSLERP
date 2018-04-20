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
    public class ReceiptVoucherHeader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public int CreationMode { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public string VoucherType { get; set; }
        public string BankTransactionMode { get; set; }
        public string CurrencyID { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal VoucherAmountINR { get; set; }
        public string BookType { get; set; }
        public decimal VoucherAmount { get; set; }
        public string AccountCodeDebit { get; set; }
        public string AccountNameDebit { get; set; }
        public string AccountCode { get; set; } ///For detail
        public string AccountName { get; set; }///For detail
        public decimal AmountDebit { get; set; }
        public string BillDetails { get; set; }
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public string Narration { get; set; }
        public string ProjectID { get; set; }
        public string OfficeID { get; set; }
        public string OfficeName { get; set; }
        public string SLType { get; set; }
        public string SLCode { get; set; }
        public string SLName { get; set; }
        public string CommentStatus { get; set; }
        public string Comments { get; set; }
        public string CreateUser { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreateTime { get; set; }
        public string ForwarderName { get; set; }
        public string ApproverName { get; set; }
        public string ForwarderList { get; set; }
        public int status { get; set; }
        public int DocumentStatus { get; set; }
        public decimal TotalAdjusted { get; set; }
        public ReceiptVoucherHeader()
        {
            //Reference = "";
            Comments = "";
        }
    }
    public class ReceiptVoucherDetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; } 
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }

        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal AmountDebit { get; set; }
        public decimal AmountDebitINR { get; set; }
        public decimal AmountCreditINR { get; set; }
        public decimal AmountCredit { get; set; }
        public DateTime BankDate { get; set; }

        //public decimal AmountCredit { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        //To be remove later
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public string AccountCodeCredit { get; set; }
        public string AccountNameCredit { get; set; }

    }
    class ReceiptVoucherDB
    {
        public List<ReceiptVoucherHeader> getFilteredReceiptVoucherHeader(string userList, int opt, string userCommentStatusString)
        {
            ReceiptVoucherHeader rvh;
            List<ReceiptVoucherHeader> RVHList = new List<ReceiptVoucherHeader>();
            try
            {
                //approved user comment status string
                string acStr="";
                try
                {
                    acStr = userCommentStatusString.Substring(0, userCommentStatusString.Length - 2) + "1" + Main.delimiter2;
                }
                catch (Exception ex)
                {
                    acStr = "";
                }
                //-----
                string query1 = "select distinct DocumentID,TemporaryNo,TemporaryDate,VoucherNo,VoucherDate," +
                    " CreationMode,ProjectID,OfficeID,VoucherType,BookType," +
                    " SLType,SLCode,BankTransactionMode,CurrencyID,ExchangeRate,VoucherAmount,VoucherAmountINR,Narration," +
                    " CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,status,DocumentStatus,CommentStatus,SLName,BillDetails " +
                    " from ViewReceiptVoucher" +
                    " where ((ForwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (CreateUser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (CommentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) order by VoucherDate desc,DocumentID asc,VoucherNo desc";

                string query2 = "select distinct DocumentID,TemporaryNo,TemporaryDate,VoucherNo,VoucherDate," +
                    " CreationMode,ProjectID,OfficeID,VoucherType,BookType," +
                    " SLType,SLCode,BankTransactionMode,CurrencyID,ExchangeRate,VoucherAmount,VoucherAmountINR,Narration," +
                    " CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,status,DocumentStatus,CommentStatus,SLName,BillDetails " +
                    " from ViewReceiptVoucher" +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '"+Login.userLoggedIn+"')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc" ;

                string query3 = "select distinct DocumentID,TemporaryNo,TemporaryDate,VoucherNo,VoucherDate," +
                    " CreationMode,ProjectID,OfficeID,VoucherType,BookType," +
                    " SLType,SLCode,BankTransactionMode,CurrencyID,ExchangeRate,VoucherAmount,VoucherAmountINR,Narration," +
                    " CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,status,DocumentStatus,CommentStatus,SLName,BillDetails " +
                    " from ViewReceiptVoucher" +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'"+
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99 and status = 1 )   order by VoucherDate desc,DocumentID asc,VoucherNo desc";

                string query6 = "select distinct DocumentID,TemporaryNo,TemporaryDate,VoucherNo,VoucherDate," +
                    " CreationMode,ProjectID,OfficeID,VoucherType,BookType," +
                    " SLType,SLCode,BankTransactionMode,CurrencyID,ExchangeRate,VoucherAmount,VoucherAmountINR,Narration," +
                    " CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,status,DocumentStatus,CommentStatus,SLName,BillDetails " +
                    " from ViewReceiptVoucher" +
                    " where  DocumentStatus = 99 and status = 1 order by VoucherDate desc,DocumentID asc,VoucherNo desc";

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
                    case 6:
                        query = query6;
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
                    try
                    {
                        rvh = new ReceiptVoucherHeader();
                        rvh.DocumentID = reader.GetString(0);
                        rvh.TemporaryNo = reader.GetInt32(1);
                        rvh.TemporaryDate = reader.GetDateTime(2);
                        rvh.VoucherNo = reader.GetInt32(3);
                        rvh.VoucherDate = reader.GetDateTime(4);
                        rvh.CreationMode = reader.GetInt32(5);
                        rvh.ProjectID = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        rvh.OfficeID = reader.GetString(7);
                        rvh.VoucherType = reader.GetString(8);
                        rvh.BookType = reader.GetString(9);
                        //rvh.AccountCodeDebit = reader.GetString(10);
                        //rvh.AccountNameDebit = reader.GetString(11);
                        rvh.SLType = reader.GetString(10);
                        rvh.SLCode = reader.GetString(11);
                        rvh.BankTransactionMode = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        rvh.CurrencyID = reader.GetString(13);
                        rvh.ExchangeRate = reader.GetDecimal(14);
                        rvh.VoucherAmount = reader.GetDecimal(15);
                        rvh.VoucherAmountINR = reader.GetDecimal(16);
                        rvh.Narration = reader.GetString(17);
                        rvh.CreateUser = reader.GetString(18);
                        rvh.ForwardUser = reader.GetString(19);
                        rvh.ApproveUser = reader.GetString(20);
                        rvh.CreatorName = reader.GetString(21);
                        rvh.CreateTime = reader.GetDateTime(22);
                        rvh.ForwarderName = reader.GetString(23);
                        rvh.ApproverName = reader.GetString(24);

                        if (!reader.IsDBNull(25))
                        {
                            rvh.ForwarderList = reader.GetString(25);
                        }
                        else
                        {
                            rvh.ForwarderList = "";
                        }
                        rvh.status = reader.GetInt32(26);
                        rvh.DocumentStatus = reader.GetInt32(27);
                        if (!reader.IsDBNull(28))
                        {
                            rvh.CommentStatus = reader.GetString(28);
                        }
                        else
                        {
                            rvh.CommentStatus = "";
                        }
                        rvh.SLName = reader.GetString(29);
                        rvh.BillDetails = reader.IsDBNull(30) ? "":reader.GetString(30);
                        RVHList.Add(rvh);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying MRN Header Details");
            }
            return RVHList;
        }



        public static List<ReceiptVoucherDetail> getVoucherDetail(ReceiptVoucherHeader rvh)
        {
            ReceiptVoucherDetail rvd;
            List<ReceiptVoucherDetail> RVDetail = new List<ReceiptVoucherDetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,TemporaryNo, TemporaryDate,AccountCode,AccountName,AmountDebit,AmountDebitINR,AmountCredit,AmountCreditINR," +
                   "ChequeNo,ChequeDate" +
                  " from ViewReceiptVoucher " +
                  "where DocumentID='" + rvh.DocumentID + "'" +
                  " and TemporaryNo=" + rvh.TemporaryNo +
                  " and TemporaryDate='" + rvh.TemporaryDate.ToString("yyyy-MM-dd") + "'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rvd = new ReceiptVoucherDetail();
                    rvd.RowID = reader.GetInt32(0);
                    rvd.DocumentID = reader.GetString(1);
                    rvd.TemporaryNo = reader.GetInt32(2);
                    rvd.TemporaryDate = reader.GetDateTime(3);
                    rvd.AccountCode = reader.GetString(4);
                    rvd.AccountName = reader.GetString(5);
                    rvd.AmountDebit = reader.GetDecimal(6);
                    rvd.AmountDebitINR = reader.GetDecimal(7);
                    rvd.AmountCredit = reader.GetDecimal(8);
                    rvd.AmountCreditINR = reader.GetDecimal(9);
                    rvd.ChequeNo = reader.GetString(10);
                    rvd.ChequeDate = reader.GetDateTime(11);
                    RVDetail.Add(rvd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying MRN Details");
            }
            return RVDetail;
        }

        public Boolean validateReceiptVoucherHeader(ReceiptVoucherHeader rvh)
        {
            Boolean status = true;
            try
            {
                if (rvh.DocumentID.Trim().Length == 0 || rvh.DocumentID == null)
                {
                    return false;
                }
                //if (rvh.ProjectID.Trim().Length == 0 || rvh.ProjectID == null)
                //{
                //    return false;
                //}

                //Temporary Validation
                ////if (rvh.VoucherDate == null || rvh.VoucherDate == DateTime.Parse("1900-01-01"))
                ////{
                ////    return false;
                ////}

                if (rvh.VoucherType.Trim().Length == 0 || rvh.VoucherType == null)
                {
                    return false;
                }
                if (rvh.BookType.Trim().Length == 0 || rvh.BookType == null)
                {
                    return false;
                }
                //if (rvh.AccountCodeDebit.Trim().Length == 0 || rvh.AccountCodeDebit == null)
                //{
                //    return false;
                //}
                if (rvh.SLType.Trim().Length == 0 || rvh.SLType == null)
                {
                    return false;
                }
                if (rvh.SLCode.Trim().Length == 0 || rvh.SLCode == null)
                {
                    return false;
                }
                if (rvh.DocumentID == "BANKRECEIPTVOUCHER")
                {
                    if (rvh.BankTransactionMode == null || rvh.BankTransactionMode.Trim().Length == 0)
                    {
                        return false;
                    }
                }

                if (rvh.CurrencyID.Trim().Length == 0 || rvh.CurrencyID == null)
                {
                    return false;
                }
                if (rvh.Narration.Trim().Length == 0 || rvh.Narration == null)
                {
                    return false;
                }
                if (rvh.ExchangeRate == 0)
                {
                    return false;
                }
                if (rvh.VoucherAmount == 0)
                {
                    return false;
                }
                if (rvh.VoucherAmountINR == 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return status;
        }
        public Boolean forwardReceiptVoucherHeader(ReceiptVoucherHeader rvh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ReceiptVoucherHeader set DocumentStatus=" + (rvh.DocumentStatus + 1) +
                    ", forwardUser='" + rvh.ForwardUser + "'" +
                    ", commentStatus='" + rvh.CommentStatus + "'" +
                    ", ForwarderList='" + rvh.ForwarderList + "'" +
                    " where DocumentID='" + rvh.DocumentID + "'" +
                    " and TemporaryNo=" + rvh.TemporaryNo +
                    " and TemporaryDate='" + rvh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ReceiptVoucherHeader", "", updateSQL) +
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

        public Boolean reverseReceiptVoucherHeader(ReceiptVoucherHeader rvh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ReceiptVoucherHeader set DocumentStatus=" + rvh.DocumentStatus+
                    ", forwardUser='" + rvh.ForwardUser + "'" +
                    ", commentStatus='" + rvh.CommentStatus + "'" +
                    ", ForwarderList='" + rvh.ForwarderList + "'" +
                    " where DocumentID='" + rvh.DocumentID + "'" +
                    " and TemporaryNo=" + rvh.TemporaryNo +
                    " and TemporaryDate='" + rvh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ReceiptVoucherHeader", "", updateSQL) +
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

        public Boolean ApproveReceiptVoucherHeader(ReceiptVoucherHeader rvh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ReceiptVoucherHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + rvh.CommentStatus + "'" +
                    ", VoucherNo=" + rvh.VoucherNo +
                    ", VoucherDate=convert(date, getdate())" +
                    " where DocumentID='" + rvh.DocumentID + "'" +
                    " and TemporaryNo=" + rvh.TemporaryNo +
                    " and TemporaryDate='" + rvh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ReceiptVoucherHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "update InvoiceOutReceipts set " +
                    " RVNo=" + rvh.VoucherNo +
                    ", RVDate=convert(date, getdate())" +
                     " where RVTemporaryNo =" + rvh.TemporaryNo +
                     " and RVTemporaryDate ='" + rvh.TemporaryDate.ToString("yyyy-MM-dd") + "' and RVDocumentID = '" + rvh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InvoiceOutReceipts", "", updateSQL) +
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
                string query = "select comments from ReceiptVoucherHeader where DocumentID='" + docid + "'" +
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
            }
            return cmtString;
        }
        public static string getRVDtlsForProjectTrans(string projectID)
        {
            string str = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select COUNT(*), SUM(VoucherAmount) from ReceiptVoucherHeader where ProjectID = '" + projectID + "' and DocumentStatus = 99";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    decimal dd = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);
                    str = reader.GetInt32(0) + "-" + dd;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return str;
        }
        public static List<ReceiptVoucherHeader> getRVINFOForProjectTrans(string projectID)
        {
            ReceiptVoucherHeader rvh;
            List<ReceiptVoucherHeader> RVHList = new List<ReceiptVoucherHeader>();
            string str = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select distinct VoucherNo,VoucherDate,SLType,SLName,ProjectID,VoucherAmount from ViewReceiptVoucher where ProjectID = '" + projectID + "' and DocumentStatus = 99";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rvh = new ReceiptVoucherHeader();
                    rvh.VoucherNo = reader.GetInt32(0);
                    rvh.VoucherDate = reader.GetDateTime(1);
                    rvh.SLType = reader.GetString(2);
                    rvh.SLName = reader.GetString(3);
                    rvh.ProjectID = reader.GetString(4);
                    rvh.VoucherAmount = reader.GetDecimal(5);
                    RVHList.Add(rvh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return RVHList;
        }


        public Boolean updateRVHeaderAndDetail(ReceiptVoucherHeader rvh, ReceiptVoucherHeader prevrvh,
                                                List<ReceiptVoucherDetail> RVDetails, List<invoiceoutreceipts> receiveList)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ReceiptVoucherHeader set CreationMode='" + rvh.CreationMode +
                   "',ProjectID='" + rvh.ProjectID +
                    "',VoucherDate='" + rvh.VoucherDate.ToString("yyyy-MM-dd") +
                    "',OfficeID='" + rvh.OfficeID +
                     "', VoucherType='" + rvh.VoucherType +
                   "', BookType='" + rvh.BookType +
                    "', SLType='" + rvh.SLType +
                    "', SLCode='" + rvh.SLCode +
                     "', BankTransactionMode='" + rvh.BankTransactionMode +
                   "', CurrencyID='" + rvh.CurrencyID +
                    "', ExchangeRate=" + rvh.ExchangeRate +
                     ", VoucherAmount=" + rvh.VoucherAmount +
                      ", VoucherAmountINR=" + rvh.VoucherAmountINR +
                      ", BillDetails='" + rvh.BillDetails +
                       "', Narration='" + rvh.Narration +
                   "', Comments='" + rvh.Comments +
                    "', CommentStatus='" + rvh.CommentStatus +
                   "', ForwarderList='" + rvh.ForwarderList + "'" +
                   " where DocumentID='" + prevrvh.DocumentID + "'" +
                   " and TemporaryNo=" + prevrvh.TemporaryNo +
                   " and TemporaryDate='" + prevrvh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ReceiptVoucherHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from ReceiptVoucherDetail where DocumentID='" + prevrvh.DocumentID + "'" +
                     " and TemporaryNo=" + prevrvh.TemporaryNo +
                     " and TemporaryDate='" + prevrvh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "ReceiptVoucherDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (ReceiptVoucherDetail rvd in RVDetails)
                {
                    updateSQL = "insert into ReceiptVoucherDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,AccountCode,AmountDebit,AmountDebitINR,AmountCredit," +
                    "AmountCreditINR,ChequeNo,ChequeDate) " +
                    "values ('" + rvh.DocumentID + "'," +
                    rvh.TemporaryNo + "," +
                    "'" + rvh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + rvd.AccountCode + "'," +
                   rvd.AmountDebit + "," +
                    rvd.AmountDebitINR + "," +
                     rvd.AmountCredit + "," +
                      rvd.AmountCreditINR + "," +
                    "'" + rvd.ChequeNo + "'," +
                    "'" + rvd.ChequeDate.ToString("yyyy-MM-dd") + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "ReceiptVoucherDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                }

                updateSQL = "Delete from InvoiceOutReceipts where RVTemporaryNo='" + rvh.TemporaryNo + "'" +
                " and RVTemporaryDate='" + rvh.TemporaryDate.ToString("yyyy-MM-dd") + "' and RVDocumentID = '" + rvh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InvoiceOutReceipts", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (invoiceoutreceipts rec in receiveList)
                {
                    updateSQL = "insert into InvoiceOutReceipts " +
                    "(InvoiceDocumentID,CustomerID,InvoiceOutNo,InvoiceOutDate,InvoiceOutTemporaryNo,InvoiceOutTemporaryDate,RVDocumentID,RVTemporaryNo,RVTemporaryDate,RVNo,RVDate,Amount,TDSAmount) " +
                    "values ('" + rec.InvoiceDocumentID + "'," +
                    "'" + rec.CustomerID + "'," +
                    rec.InvoiceOutNo + "," +
                    "'" + rec.InvoiceOutDate.ToString("yyyy-MM-dd") + "'," +
                      rec.InvoiceOutTemporaryNo + "," +
                    "'" + rec.InvoiceOutTemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'"+ rvh.DocumentID + "'," +
                     +rvh.TemporaryNo + "," +
                    "'" + rvh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       "0," +
                    "'" + Convert.ToDateTime("1900-01-01").ToString("yyyy-MM-dd") + "'," +
                    rec.Amount + "," + rec.TDSAmount +  ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InvoiceOutReceipts", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                    MessageBox.Show("Transaction Exception Occured");
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        public Boolean InsertRVHeaderAndDetail(ReceiptVoucherHeader rvh, List<ReceiptVoucherDetail> RVDetails,
                                                                                        List<invoiceoutreceipts> receiveList)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                rvh.TemporaryNo = DocumentNumberDB.getNumber(rvh.DocumentID, 1);
                if (rvh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + rvh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + rvh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into ReceiptVoucherHeader " +
                    "(DocumentID,CreationMode,ProjectID,OfficeID,TemporaryNo,TemporaryDate,VoucherNo,VoucherDate,VoucherType,BookType," +
                    "BillDetails,SLType,SLCode,BankTransactionMode,CurrencyID,ExchangeRate," +
                    "VoucherAmount,VoucherAmountINR,Narration," +
                    "Comments,CommentStatus,CreateUser,CreateTime,ForwarderList,DocumentStatus,Status)" +
                    " values (" +
                    "'" + rvh.DocumentID + "'," +
                    "1," +
                    "'" + rvh.ProjectID + "'," +
                    "'" + rvh.OfficeID + "'," +
                    rvh.TemporaryNo + "," +
                    "'" + rvh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     rvh.VoucherNo + "," +
                    "'" + rvh.VoucherDate.ToString("yyyy-MM-dd") + "'," +
                      "'" + rvh.VoucherType + "'," +
                    "'" + rvh.BookType + "'," +
                   "'" + rvh.BillDetails + "'," +
                    "'" + rvh.SLType + "'," +
                    "'" + rvh.SLCode + "'," +
                    "'" + rvh.BankTransactionMode + "'," +
                    "'" + rvh.CurrencyID + "'," +
                    rvh.ExchangeRate + "," +
                    rvh.VoucherAmount + "," +
                    rvh.VoucherAmountINR + "," +
                    "'" + rvh.Narration + "'," +
                     "'" + rvh.Comments + "'," +
                       "'" + rvh.CommentStatus + "'," +
                    "'" + Login.userLoggedIn + "'," +
                    "GETDATE()" + "," +
                    "'" + rvh.ForwarderList + "'," +
                    rvh.DocumentStatus + "," +
                         rvh.status + ")";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "ReceiptVoucherHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from ReceiptVoucherDetail where DocumentID='" + rvh.DocumentID + "'" +
                     " and TemporaryNo=" + rvh.TemporaryNo +
                     " and TemporaryDate='" + rvh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "ReceiptVoucherDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (ReceiptVoucherDetail rvd in RVDetails)
                {
                    updateSQL = "insert into ReceiptVoucherDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,AccountCode,AmountDebit,AmountDebitINR,AmountCredit," +
                    "AmountCreditINR,ChequeNo,ChequeDate) " +
                    "values ('" + rvh.DocumentID + "'," +
                    rvh.TemporaryNo + "," +
                    "'" + rvh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + rvd.AccountCode + "'," +
                   rvd.AmountDebit + "," +
                    rvd.AmountDebitINR + "," +
                     rvd.AmountCredit + "," +
                      rvd.AmountCreditINR + "," +
                    "'" + rvd.ChequeNo + "'," +
                    "'" + rvd.ChequeDate.ToString("yyyy-MM-dd") + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "ReceiptVoucherDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                }

                updateSQL = "Delete from InvoiceOutReceipts where RVTemporaryNo='" + rvh.TemporaryNo + "'" +
                " and RVTemporaryDate='" + rvh.TemporaryDate.ToString("yyyy-MM-dd") + "' and RVDocumentID = '" + rvh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InvoiceOutReceipts", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (invoiceoutreceipts rec in receiveList)
                {
                    updateSQL = "insert into InvoiceOutReceipts " +
                    "(InvoiceDocumentID,CustomerID,InvoiceOutNo,InvoiceOutDate,InvoiceOutTemporaryNo,InvoiceOutTemporaryDate,RVDocumentID,RVTemporaryNo,RVTemporaryDate,RVNo,RVDate,Amount,TDSAmount) " +
                    "values ('" + rec.InvoiceDocumentID + "'," +
                    "'" + rec.CustomerID + "'," +
                    rec.InvoiceOutNo + "," +
                    "'" + rec.InvoiceOutDate.ToString("yyyy-MM-dd") + "'," +
                      rec.InvoiceOutTemporaryNo + "," +
                    "'" + rec.InvoiceOutTemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + rvh.DocumentID + "'," +
                     +rvh.TemporaryNo + "," +
                    "'" + rvh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       "0," +
                    "'" + Convert.ToDateTime("1900-01-01").ToString("yyyy-MM-dd") + "'," +
                    rec.Amount + "," + rec.TDSAmount + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InvoiceOutReceipts", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                status = false;
                MessageBox.Show("Transaction Exception Occured");
            }
            return status;
        }
        public static ReceiptVoucherHeader getReceiptVoucherHeaderForTrailbalance(ReceiptVoucherHeader rvhTemp)
        {
            ReceiptVoucherHeader rvh = new ReceiptVoucherHeader();
            try
            {
                string query = "select distinct DocumentID,TemporaryNo,TemporaryDate,VoucherNo,VoucherDate," +
                    " CreationMode,ProjectID,OfficeID,VoucherType,BookType," +
                    " SLType,SLCode,BankTransactionMode,CurrencyID,ExchangeRate,VoucherAmount,VoucherAmountINR,Narration," +
                    " status,DocumentStatus,SLName,BillDetails " +
                    " from ViewReceiptVoucher" +
                    " where  VoucherNo = " + rvhTemp.VoucherNo + " and VoucherDate = '" + rvhTemp.VoucherDate.ToString("yyyy-MM-dd") +
                     "' and DocumentID = '" + rvhTemp.DocumentID + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    try
                    {
                        rvh.DocumentID = reader.GetString(0);
                        rvh.TemporaryNo = reader.GetInt32(1);
                        rvh.TemporaryDate = reader.GetDateTime(2);
                        rvh.VoucherNo = reader.GetInt32(3);
                        rvh.VoucherDate = reader.GetDateTime(4);
                        rvh.CreationMode = reader.GetInt32(5);
                        rvh.ProjectID = reader.IsDBNull(6) ? "" : reader.GetString(6);
                        rvh.OfficeID = reader.GetString(7);
                        rvh.VoucherType = reader.GetString(8);
                        rvh.BookType = reader.GetString(9);
                        rvh.SLType = reader.GetString(10);
                        rvh.SLCode = reader.GetString(11);
                        rvh.BankTransactionMode = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        rvh.CurrencyID = reader.GetString(13);
                        rvh.ExchangeRate = reader.GetDecimal(14);
                        rvh.VoucherAmount = reader.GetDecimal(15);
                        rvh.VoucherAmountINR = reader.GetDecimal(16);
                        rvh.Narration = reader.GetString(17);
                        rvh.status = reader.GetInt32(18);
                        rvh.DocumentStatus = reader.GetInt32(19);
                        rvh.SLName = reader.GetString(20);
                        rvh.BillDetails = reader.IsDBNull(21) ? "" : reader.GetString(21);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying MRN Header Details");
            }
            return rvh;
        }
        //Report BR
        public static List<ReceiptVoucherHeader> getAllNonDepositedReceiptsForReportBR(DateTime FYStartDate, DateTime todate, string acCode)
        {
            ReceiptVoucherHeader rvh;
            List<ReceiptVoucherHeader> RVHList = new List<ReceiptVoucherHeader>();
            string str = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID, VoucherNo, VoucherDate,SLName,AmountDebitINR,AmountCreditINR from ViewReceiptVoucher where AccountCode = '" + acCode + "'" +
                                 " and VoucherDate >= '" + FYStartDate.ToString("yyyy-MM-dd") + "' and " +
                                 " VoucherDate <= '" + todate.ToString("yyyy-MM-dd") + "' and BankDate is NULL " +
                                 " union " +
                                  "select DocumentID, VoucherNo, VoucherDate,SLName,AmountDebitINR,AmountCreditINR from ViewPaymentVoucher where AccountCode = '" + acCode + "'" +
                                 " and VoucherDate >= '" + FYStartDate.ToString("yyyy-MM-dd") + "' and " +
                                 " VoucherDate <= '" + todate.ToString("yyyy-MM-dd") + "' and BankDate is NULL order by VoucherDate";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    rvh = new ReceiptVoucherHeader();
                    rvh.DocumentID = reader.GetString(0);
                    rvh.VoucherNo = reader.GetInt32(1);
                    rvh.VoucherDate = reader.GetDateTime(2);
                    rvh.SLName = reader.GetString(3);
                    rvh.VoucherAmount = reader.GetDecimal(4); //For AmountDebitINR
                    rvh.VoucherAmountINR = reader.GetDecimal(5);//For AmountCreditINR
                    RVHList.Add(rvh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return RVHList;
        }
        public static decimal getTotalNotClearedDepositeOfBankAcForBRReport(DateTime fystartdate, DateTime todate, string bankAcCode)
        {
            decimal total = 0;
            try
            {
                string query = "select AccountCode,AccountName,SUM(AmountDebitINR) as sub from ViewReceiptVoucher where AccountCode = '" + bankAcCode + "'" +
                                 " and VoucherDate >= '" + fystartdate.ToString("yyyy-MM-dd") + "' and" +
                                 " VoucherDate <= '" + todate.ToString("yyyy-MM-dd") + "' and BankDate is Null group by AccountCode, AccountName" +
                                 " Union " +
                                 "select AccountCode,AccountName,SUM(AmountDebitINR) as sub from ViewPaymentVoucher where AccountCode = '" + bankAcCode + "'" +
                                 " and VoucherDate >= '" + fystartdate.ToString("yyyy-MM-dd") + "' and" +
                                 " VoucherDate <= '" + todate.ToString("yyyy-MM-dd") + "' and BankDate is Null group by AccountCode, AccountName";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    total = total + reader.GetDecimal(2);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Not cleared deposite Receipt Voucher total");
            }
            return total;
        }


    }
}
