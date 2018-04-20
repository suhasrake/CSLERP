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
    public class paymentvoucher
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
        public string AccountCodeCredit { get; set; }
        public string AccountCodeDebit { get; set; }
        public string AccountNameCredit { get; set; }
        public string AccountNameDebit { get; set; }
        public decimal AmountDebit { get; set; }
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public string Narration { get; set; }
        public string BillDetails { get; set; }
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
        public decimal TotalTDSAdjusted{ get; set; }
        public paymentvoucher()
        {
            //Reference = "";
            Comments = "";
        }
    }
    public class paymentvoucherdetail
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
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }

        //TO Be Remove later.
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public string AccountCodeDebit { get; set; }
        public string AccountNameDebit { get; set; }
    }
    class PaymentVoucherDB
    {
        public List<paymentvoucher> getFilteredPaymentVoucherHeader(string userList, int opt, string userCommentStatusString)
        {
            paymentvoucher vh;
            List<paymentvoucher> VHeaders = new List<paymentvoucher>();
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
                    " VoucherType,VoucherAmount,VoucherAmountINR," +
                    " CreationMode,BillDetails,BookType,SLType,SLCode,SLName,Narration,ProjectID,OfficeID," +
                    " BankTransactionMode,CurrencyID,ExchangeRate,OfficeName,status,DocumentStatus," +
                    " CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,CommentStatus " +
                    " from ViewPaymentVoucher" +
                    " where ((ForwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (CreateUser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (CommentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) "+
                    " and Status not in (7, 98) order by VoucherDate desc,DocumentID asc,VoucherNo desc";

                string query2 = "select distinct DocumentID,TemporaryNo,TemporaryDate,VoucherNo,VoucherDate," +
                    " VoucherType,VoucherAmount,VoucherAmountINR," +
                    " CreationMode,BillDetails,BookType,SLType,SLCode,SLName,Narration,ProjectID,OfficeID," +
                    " BankTransactionMode,CurrencyID,ExchangeRate,OfficeName,status,DocumentStatus," +
                    " CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,CommentStatus " +
                    " from ViewPaymentVoucher" +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '"+Login.userLoggedIn+"')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) "+
                    " and Status not in (7, 98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query3 = "select distinct DocumentID,TemporaryNo,TemporaryDate,VoucherNo,VoucherDate," +
                    " VoucherType,VoucherAmount,VoucherAmountINR, " +
                    " CreationMode,BillDetails,BookType,SLType,SLCode,SLName,Narration,ProjectID,OfficeID," +
                    " BankTransactionMode,CurrencyID,ExchangeRate,OfficeName,status,DocumentStatus," +
                    " CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,CommentStatus " +
                    " from ViewPaymentVoucher" +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'"+
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99 and Status = 1)   order by VoucherDate desc,DocumentID asc,VoucherNo desc";

                string query4 = "select distinct DocumentID,TemporaryNo,TemporaryDate,VoucherNo,VoucherDate," +
                   " VoucherType,VoucherAmount,VoucherAmountINR, " +
                   " CreationMode,BillDetails,BookType,SLType,SLCode,SLName,Narration,ProjectID,OfficeID," +
                   " BankTransactionMode,CurrencyID,ExchangeRate,OfficeName,status,DocumentStatus," +
                   " CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,CommentStatus " +
                   " from ViewPaymentVoucher" +
                   " where ((createuser='" + Login.userLoggedIn + "'" +
                   " or ForwarderList like '%" + userList + "%'" +
                   " or commentStatus like '%" + acStr + "%'" +
                   " or approveUser='" + Login.userLoggedIn + "')" +
                   " and VoucherNo > 0 and Status = 98)   order by VoucherDate desc,DocumentID asc,VoucherNo desc";

                string query6 = "select distinct DocumentID,TemporaryNo,TemporaryDate,VoucherNo,VoucherDate," +
                    " VoucherType,VoucherAmount,VoucherAmountINR, " +
                    " CreationMode,BillDetails,BookType,SLType,SLCode,SLName,Narration,ProjectID,OfficeID," +
                    " BankTransactionMode,CurrencyID,ExchangeRate,OfficeName,status,DocumentStatus," +
                    " CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,CommentStatus " +
                    " from ViewPaymentVoucher" +
                    " where  DocumentStatus = 99 and Status = 1  order by VoucherDate desc,DocumentID asc,VoucherNo desc";

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
                    case 4:
                        query = query4;
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
                        vh = new paymentvoucher();
                        vh.DocumentID = reader.GetString(0);
                        vh.TemporaryNo = reader.GetInt32(1);
                        vh.TemporaryDate = reader.GetDateTime(2);
                        vh.VoucherNo = reader.GetInt32(3);
                        vh.VoucherDate = reader.GetDateTime(4);
                        vh.VoucherType = reader.GetString(5);
                        vh.VoucherAmount = reader.GetDecimal(6);
                        vh.VoucherAmountINR = reader.GetDecimal(7);
                        //vh.AccountCodeCredit = reader.GetString(8);
                        //vh.AccountNameCredit = reader.GetString(9);
                        vh.CreationMode = reader.GetInt32(8);
                        vh.BillDetails = reader.IsDBNull(9)?"":reader.GetString(9);
                        vh.BookType = reader.GetString(10);
                        vh.SLType = reader.GetString(11);
                        vh.SLCode = reader.GetString(12);
                        vh.SLName = reader.GetString(13);
                        vh.Narration = reader.GetString(14);          
                        vh.ProjectID = reader.IsDBNull(15) ? "":reader.GetString(15);
                        vh.OfficeID = reader.GetString(16);
                        vh.BankTransactionMode = reader.IsDBNull(17) ? "" : reader.GetString(17);
                        vh.CurrencyID = reader.GetString(18);
                        vh.ExchangeRate = reader.GetDecimal(19);
                        vh.OfficeName = reader.IsDBNull(20) ? "" : reader.GetString(20);
                        vh.status = reader.GetInt32(21);
                        vh.DocumentStatus = reader.GetInt32(22);

                        vh.CreateUser = reader.GetString(23);
                        vh.ForwardUser = reader.GetString(24);
                        vh.ApproveUser = reader.GetString(25);
                        vh.CreatorName = reader.GetString(26);
                        vh.CreateTime = reader.GetDateTime(27);
                        vh.ForwarderName = reader.GetString(28);
                        vh.ApproverName = reader.GetString(29);

                        if (!reader.IsDBNull(30))
                        {
                            vh.ForwarderList = reader.GetString(30);
                        }
                        else
                        {
                            vh.ForwarderList = "";
                        }
                     
                        if (!reader.IsDBNull(31))
                        {
                            vh.CommentStatus = reader.GetString(31);
                        }
                        else
                        {
                            vh.CommentStatus = "";
                        }
                       
                        
                        VHeaders.Add(vh);
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
            return VHeaders;
        }



        public static List<paymentvoucherdetail> getVoucherDetail(paymentvoucher vh)
        {
            paymentvoucherdetail vd;
            List<paymentvoucherdetail> VDetail = new List<paymentvoucherdetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                 query = "select RowID,DocumentID,TemporaryNo, TemporaryDate,AccountCode,AccountName,AmountDebit,AmountDebitINR,AmountCredit,AmountCreditINR, " +
                    "ChequeNo,ChequeDate"+
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
                    vd.TemporaryDate = reader.GetDateTime(3);
                    vd.AccountCode = reader.GetString(4);
                    vd.AccountName = reader.IsDBNull(5)?"":reader.GetString(5);
                    vd.AmountDebit = reader.GetDecimal(6);
                    vd.AmountDebitINR = reader.GetDecimal(7);
                    vd.AmountCredit = reader.GetDecimal(8);
                    vd.AmountCreditINR = reader.GetDecimal(9);  
                    vd.ChequeNo = reader.GetString(10);
                    vd.ChequeDate = reader.IsDBNull(11) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(11);
                    VDetail.Add(vd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying MRN Details");
            }
            return VDetail;
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
                if(vh.DocumentID == "BANKPAYMENTVOUCHER")
                {
                    if(vh.BankTransactionMode == null || vh.BankTransactionMode.Trim().Length == 0)
                    {
                        return false;
                    }
                }

                //Temporary Validation
                ////if (vh.VoucherDate == null || vh.VoucherDate == DateTime.Parse("1900-01-01"))
                ////{
                ////    return false;
                ////}

                //if (vh.AccountCodeCredit.Trim().Length == 0 || vh.AccountCodeCredit == null)
                //{
                //    return false;
                //}
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
            catch (Exception)
            {
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
                string updateSQL = "update PaymentVoucherHeader set DocumentStatus=" + vh.DocumentStatus+
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
            catch (Exception)
            {
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
                //string updateSQL = "update PaymentVoucherHeader set DocumentStatus=99, status=1 " +
                //    ", ApproveUser='" + Login.userLoggedIn + "'" +
                //    ", commentStatus='" + vh.CommentStatus + "'" +
                //    ", VoucherNo=" + vh.VoucherNo +
                //    ", VoucherDate=convert(date, getdate())" +
                //    " where DocumentID='" + vh.DocumentID + "'" +
                //    " and TemporaryNo=" + vh.TemporaryNo +
                //    " and TemporaryDate='" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "'";

                //Temporary Query String
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

                updateSQL = "update InvoiceInPayments set " +
                 " PVNo=" + vh.VoucherNo +
                 ", PVDate=convert(date, getdate())" +
                 " where PVTemporaryNo =" + vh.TemporaryNo +
                 " and PVTemporaryDate ='" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                 " and PVDocumentID = '" + vh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InvoiceInPayments", "", updateSQL) +
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
            }
            return cmtString;
        }
        public static string getPVHDtlsForProjectTrans(string projectID, int opt)
        {
            //opt 1: Material Payment
            //opt 2: WO Payment
            //opt 3: Other Payment
            string str = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "";
                if(opt == 1)
                    query = "select count(*),sum(AmountDebit) from ViewPaymentVoucher " +
                        "where BillNo in (select SupplierInvoiceNo from InvoiceInHeader where DocumentID = 'POINVOICEIN') " +
                        "and BillDate in (select SupplierInvoiceDate from InvoiceInHeader where DocumentID = 'POINVOICEIN') "+
                        " and ProjectID = '" + projectID + "' and DocumentStatus = 99 and status = 1 group by ProjectID";
                else if(opt == 2)
                    query = "select count(*),sum(AmountDebit) from ViewPaymentVoucher " +
                       "where BillNo in (select SupplierInvoiceNo from InvoiceInHeader where DocumentID = 'WOINVOICEIN') " +
                       "and BillDate in (select SupplierInvoiceDate from InvoiceInHeader where DocumentID = 'WOINVOICEIN') " +
                       " and ProjectID = '" + projectID + "' and DocumentStatus = 99 and status = 1 group by ProjectID";
                else
                    query = "select count(*),sum(AmountDebit) from ViewPaymentVoucher " +
                      "where BillNo Not in (select SupplierInvoiceNo from InvoiceInHeader where DocumentID in ('WOINVOICEIN','POINVOICEIN')) " +
                      "and BillDate Not in (select SupplierInvoiceDate from InvoiceInHeader where DocumentID in ('WOINVOICEIN','POINVOICEIN')) " +
                      " and ProjectID = '" + projectID + "' and DocumentStatus = 99 and status = 1 group by ProjectID";
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
        public static List<paymentvoucher> getRVINFOForProjectTrans(string projectID, int opt)
        {
            //opt 1: Material Payment
            //opt 2: WO Payment
            //opt 3: Other Payment
            paymentvoucher vh;
            List<paymentvoucher> VHeaders = new List<paymentvoucher>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "";
                if (opt == 1)
                    query = "select VoucherNo,VoucherDate,SLType,SLName,AmountDebit, BillNo,BillDate,ProjectID from ViewPaymentVoucher " +
                        "where BillNo in (select SupplierInvoiceNo from InvoiceInHeader where DocumentID = 'POINVOICEIN') " +
                        "and BillDate in (select SupplierInvoiceDate from InvoiceInHeader where DocumentID = 'POINVOICEIN') " +
                        " and ProjectID = '" + projectID + "' and DocumentStatus = 99 and status = 1 ";
                else if(opt == 2)
                    query = "select VoucherNo,VoucherDate,SLType,SLName,AmountDebit, BillNo,BillDate,ProjectID from ViewPaymentVoucher " +
                       "where BillNo in (select SupplierInvoiceNo from InvoiceInHeader where DocumentID = 'WOINVOICEIN') " +
                       "and BillDate in (select SupplierInvoiceDate from InvoiceInHeader where DocumentID = 'WOINVOICEIN') " +
                       " and ProjectID = '" + projectID + "' and DocumentStatus = 99 and status = 1 ";
                else
                    query = "select VoucherNo,VoucherDate,SLType,SLName,AmountDebit, BillNo,BillDate,ProjectID from ViewPaymentVoucher " +
                      "where BillNo Not in (select SupplierInvoiceNo from InvoiceInHeader where DocumentID in ('WOINVOICEIN','POINVOICEIN')) " +
                      "and BillDate Not in (select SupplierInvoiceDate from InvoiceInHeader where DocumentID in ('WOINVOICEIN','POINVOICEIN')) " +
                      " and ProjectID = '" + projectID + "' and DocumentStatus = 99 and status = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    vh = new paymentvoucher();
                    vh.VoucherNo = reader.GetInt32(0);
                    vh.VoucherDate = reader.GetDateTime(1);
                    vh.SLType = reader.GetString(2);
                    vh.SLName = reader.GetString(3);
                    vh.AmountDebit = reader.GetDecimal(4);
                    vh.BillNo = reader.GetString(5);
                    vh.BillDate = reader.GetDateTime(6);
                    vh.ProjectID = reader.GetString(7);
                    VHeaders.Add(vh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return VHeaders;
        }


        public Boolean updatePVHeaderAndDetail(paymentvoucher vh, paymentvoucher prevvh, List<paymentvoucherdetail> VDetails,List<invoiceinpayments> payList)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                //string updateSQL = "update PaymentVoucherHeader set CreationMode='" + vh.CreationMode +
                //    "',ProjectID='" + vh.ProjectID +
                //     "',OfficeID='" + vh.OfficeID +
                //      "', VoucherType='" + vh.VoucherType +
                //    "', BookType='" + vh.BookType +
                //     "', SLType='" + vh.SLType +
                //     "', SLCode='" + vh.SLCode +
                //      "', BankTransactionMode='" + vh.BankTransactionMode +
                //    "', CurrencyID='" + vh.CurrencyID +
                //     "', ExchangeRate=" + vh.ExchangeRate +
                //      ", VoucherAmount=" + vh.VoucherAmount +
                //       ", VoucherAmountINR=" + vh.VoucherAmountINR +
                //        ", Narration='" + vh.Narration +
                //          "', BillDetails='" + vh.BillDetails +
                //    "', Comments='" + vh.Comments +
                //     "', CommentStatus='" + vh.CommentStatus +
                //    "', ForwarderList='" + vh.ForwarderList + "'" +
                //    " where DocumentID='" + prevvh.DocumentID + "'" +
                //    " and TemporaryNo=" + prevvh.TemporaryNo +
                //    " and TemporaryDate='" + prevvh.TemporaryDate.ToString("yyyy-MM-dd") + "'";

                //Temporary Query Sstring

                string updateSQL = "update PaymentVoucherHeader set CreationMode='" + vh.CreationMode +
                   "',ProjectID='" + vh.ProjectID +
                    "',OfficeID='" + vh.OfficeID +
                     "',VoucherDate='" + vh.VoucherDate.ToString("yyyy-MM-dd") +
                     "', VoucherType='" + vh.VoucherType +
                   "', BookType='" + vh.BookType +
                    "', SLType='" + vh.SLType +
                    "', SLCode='" + vh.SLCode +
                     "', BankTransactionMode='" + vh.BankTransactionMode +
                   "', CurrencyID='" + vh.CurrencyID +
                    "', ExchangeRate=" + vh.ExchangeRate +
                     ", VoucherAmount=" + vh.VoucherAmount +
                      ", VoucherAmountINR=" + vh.VoucherAmountINR +
                       ", Narration='" + vh.Narration +
                         "', BillDetails='" + vh.BillDetails +
                   "', Comments='" + vh.Comments +
                    "', CommentStatus='" + vh.CommentStatus +
                   "', ForwarderList='" + vh.ForwarderList + "'" +
                   " where DocumentID='" + prevvh.DocumentID + "'" +
                   " and TemporaryNo=" + prevvh.TemporaryNo +
                   " and TemporaryDate='" + prevvh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "PaymentVoucherHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from PaymentVoucherDetail where DocumentID='" + prevvh.DocumentID + "'" +
                   " and TemporaryNo=" + prevvh.TemporaryNo +
                   " and TemporaryDate='" + prevvh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "PaymentVoucherDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (paymentvoucherdetail vd in VDetails)
                {
                    updateSQL = "insert into PaymentVoucherDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,AccountCode,AmountDebit,AmountDebitINR,AmountCredit,"+
                    "AmountCreditINR,ChequeNo,ChequeDate) " +
                    "values ('" + vh.DocumentID + "'," +
                    vh.TemporaryNo + "," +
                    "'" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + vd.AccountCode + "'," +
                   vd.AmountDebit + "," +
                    vd.AmountDebitINR + "," +
                     vd.AmountCredit + "," +
                      vd.AmountCreditINR + "," +
                    "'" + vd.ChequeNo + "'," +
                    "'" + vd.ChequeDate.ToString("yyyy-MM-dd") + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "PaymentVoucherDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                updateSQL = "Delete from InvoiceInPayments where PVTemporaryNo='" + vh.TemporaryNo + "'" +
                  " and PVTemporaryDate='" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "' and PVDocumentID = '" + vh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InvoiceInPayments", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (invoiceinpayments pay in payList)
                {
                    updateSQL = "insert into InvoiceInPayments " +
                    "(InvoiceDocumentID,CustomerID,InvoiceInNo,InvoiceInDate,InvoiceInTemporaryNo,InvoiceInTemporaryDate,PVDocumentID,PVTemporaryNo,PVTemporaryDate,PVNo,PVDate," +
                    "Amount,TDSAmount) " +
                    "values ('" + pay.InvoiceDocumentID + "'," +
                     "'" + pay.CustomerID + "'," +
                    pay.InvoiceInNo + "," +
                    "'" + pay.InvoiceInDate.ToString("yyyy-MM-dd") + "'," +
                       pay.InvoiceInTemporaryNo + "," +
                    "'" + pay.InvoiceInTemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + vh.DocumentID + "'," +
                     +vh.TemporaryNo + "," +
                    "'" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       "0," +
                    "'1900-01-01'," +
                   pay.Amount+ "," + pay.TDSAmount+ ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InvoiceInPayments", "", updateSQL) +
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
        public Boolean InsertPVHeaderAndDetail(paymentvoucher vh,List<paymentvoucherdetail> VDetails, List<invoiceinpayments> payList)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                vh.TemporaryNo = DocumentNumberDB.getNumber(vh.DocumentID, 1);
                if (vh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + vh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + vh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into PaymentVoucherHeader " +
                    "(DocumentID,CreationMode,ProjectID,OfficeID,TemporaryNo,TemporaryDate,VoucherNo,VoucherDate,VoucherType,BookType," +
                    "SLType,SLCode,BankTransactionMode,CurrencyID,ExchangeRate," +
                    "VoucherAmount,VoucherAmountINR,Narration,BillDetails," +
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
                    "'" + vh.SLType + "'," +
                    "'" + vh.SLCode + "'," +
                    "'" + vh.BankTransactionMode + "'," +
                    "'" + vh.CurrencyID + "'," +
                    vh.ExchangeRate + "," +
                    vh.VoucherAmount + "," +
                    vh.VoucherAmountINR + "," +
                    "'" + vh.Narration + "'," +
                     "'" + vh.BillDetails + "'," +
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

                updateSQL = "Delete from PaymentVoucherDetail where DocumentID='" + vh.DocumentID + "'" +
                   " and TemporaryNo=" + vh.TemporaryNo +
                   " and TemporaryDate='" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "PaymentVoucherDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (paymentvoucherdetail vd in VDetails)
                {
                    updateSQL = "insert into PaymentVoucherDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,AccountCode,AmountDebit,AmountDebitINR,AmountCredit," +
                    "AmountCreditINR,ChequeNo,ChequeDate) " +
                    "values ('" + vh.DocumentID + "'," +
                    vh.TemporaryNo + "," +
                    "'" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + vd.AccountCode + "'," +
                   vd.AmountDebit + "," +
                    vd.AmountDebitINR + "," +
                     vd.AmountCredit + "," +
                      vd.AmountCreditINR + "," +
                    "'" + vd.ChequeNo + "'," +
                    "'" + vd.ChequeDate.ToString("yyyy-MM-dd") + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "PaymentVoucherDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                updateSQL = "Delete from InvoiceInPayments where PVTemporaryNo='" + vh.TemporaryNo + "'" +
                  " and PVTemporaryDate='" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "' and PVDocumentID = '" + vh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InvoiceInPayments", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (invoiceinpayments pay in payList)
                {
                    updateSQL = "insert into InvoiceInPayments " +
                    "(InvoiceDocumentID,CustomerID,InvoiceInNo,InvoiceInDate,InvoiceInTemporaryNo,InvoiceInTemporaryDate,PVDocumentID,PVTemporaryNo,PVTemporaryDate,PVNo,PVDate," +
                    "Amount,TDSAmount) " +
                    "values ('" + pay.InvoiceDocumentID + "'," +
                     "'" + pay.CustomerID + "'," +
                    pay.InvoiceInNo + "," +
                    "'" + pay.InvoiceInDate.ToString("yyyy-MM-dd") + "'," +
                       pay.InvoiceInTemporaryNo + "," +
                    "'" + pay.InvoiceInTemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + vh.DocumentID + "'," +
                     +vh.TemporaryNo + "," +
                    "'" + vh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       "0," +
                    "'1900-01-01'," +
                   pay.Amount + "," + pay.TDSAmount + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InvoiceInPayments", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                //////return false;
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
        public List<paymentvoucher> getPaymentdata()
        {
            paymentvoucher popid;
            List<paymentvoucher> POPIDetail = new List<paymentvoucher>();
            try
            {
                //string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select BillDetails,AmountDebitINR from ViewPaymentVoucher";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popid = new paymentvoucher();
                    popid.BillDetails = reader.GetString(0);
                    popid.VoucherAmountINR = reader.GetDecimal(1);
                    POPIDetail.Add(popid);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO Product Inward Details");
            }
            return POPIDetail;
        }
        public static paymentvoucher getVoucherHeaderForTrialBalance(paymentvoucher payTemp)
        {
            paymentvoucher vh = new paymentvoucher();
            try
            {
                string query = "select distinct DocumentID,TemporaryNo,TemporaryDate,VoucherNo,VoucherDate," +
                    " VoucherType,VoucherAmount,VoucherAmountINR, " +
                    " CreationMode,BillDetails,BookType,SLType,SLCode,SLName,Narration,ProjectID,OfficeID," +
                    " BankTransactionMode,CurrencyID,ExchangeRate,OfficeName,status,DocumentStatus " +
                    " from ViewPaymentVoucher" +
                    " where  DocumentID = '" + payTemp.DocumentID + "'" +
                    " and VoucherNo = " + payTemp.VoucherNo +
                    " and VoucherDate = '" + payTemp.VoucherDate.ToString("yyyy-MM-dd") + "'";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    try
                    {
                        vh.DocumentID = reader.GetString(0);
                        vh.TemporaryNo = reader.GetInt32(1);
                        vh.TemporaryDate = reader.GetDateTime(2);
                        vh.VoucherNo = reader.GetInt32(3);
                        vh.VoucherDate = reader.GetDateTime(4);
                        vh.VoucherType = reader.GetString(5);
                        vh.VoucherAmount = reader.GetDecimal(6);
                        vh.VoucherAmountINR = reader.GetDecimal(7);
                        vh.CreationMode = reader.GetInt32(8);
                        vh.BillDetails = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        vh.BookType = reader.GetString(10);
                        vh.SLType = reader.GetString(11);
                        vh.SLCode = reader.GetString(12);
                        vh.SLName = reader.GetString(13);
                        vh.Narration = reader.GetString(14);
                        vh.ProjectID = reader.IsDBNull(15) ? "" : reader.GetString(15);
                        vh.OfficeID = reader.GetString(16);
                        vh.BankTransactionMode = reader.IsDBNull(17) ? "" : reader.GetString(17);
                        vh.CurrencyID = reader.GetString(18);
                        vh.ExchangeRate = reader.GetDecimal(19);
                        vh.OfficeName = reader.IsDBNull(20) ? "" : reader.GetString(20);
                        vh.status = reader.GetInt32(21);
                        vh.DocumentStatus = reader.GetInt32(22);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Payment Voucehr Header Details");
            }
            return vh;
        }

        //Report BR
        public static List<paymentvoucher> getAllNonDepositedPaymentsForReportBR(DateTime FYStartDate, DateTime todate, string acCode)
        {
            paymentvoucher pvh;
            List<paymentvoucher> PVHList = new List<paymentvoucher>();
            string str = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID, VoucherNo, VoucherDate,SLName,AmountDebitINR,AmountCreditINR from ViewPaymentVoucher where AccountCode = '" + acCode + "'" +
                                 " and VoucherDate >= '" + FYStartDate.ToString("yyyy-MM-dd") + "' and " +
                                 " VoucherDate <= '" + todate.ToString("yyyy-MM-dd") + "' and BankDate is NULL " +
                                 " Union " +
                                "select DocumentID, VoucherNo, VoucherDate,SLName,AmountDebitINR,AmountCreditINR from ViewReceiptVoucher where AccountCode = '" + acCode + "'" +
                                 " and VoucherDate >= '" + FYStartDate.ToString("yyyy-MM-dd") + "' and " +
                                 " VoucherDate <= '" + todate.ToString("yyyy-MM-dd") + "' and BankDate is NULL order by VoucherDate";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pvh = new paymentvoucher();
                    pvh.DocumentID = reader.GetString(0);
                    pvh.VoucherNo = reader.GetInt32(1);
                    pvh.VoucherDate = reader.GetDateTime(2);
                    pvh.SLName = reader.GetString(3);
                    pvh.VoucherAmount = reader.GetDecimal(4); //For AmountDebitINR
                    pvh.VoucherAmountINR = reader.GetDecimal(5);//For AmountCreditINR
                    PVHList.Add(pvh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return PVHList;
        }
        public static decimal getTotalNotClearedPaymentOfBankAcForBRReport(DateTime fystartdate, DateTime todate, string bankAcCode)
        {
            decimal total = 0;
            try
            {
                string query = "select AccountCode,AccountName,SUM(AmountCreditINR) as sub from ViewPaymentVoucher where AccountCode = '" + bankAcCode + "'" +
                                 " and VoucherDate >= '" + fystartdate.ToString("yyyy-MM-dd") + "' and" +
                                 " VoucherDate <= '" + todate.ToString("yyyy-MM-dd") + "' and BankDate is Null group by AccountCode, AccountName" +
                                 " union " +
                                 "select AccountCode,AccountName,SUM(AmountCreditINR) as sub from ViewReceiptVoucher where AccountCode = '" + bankAcCode + "'" +
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
