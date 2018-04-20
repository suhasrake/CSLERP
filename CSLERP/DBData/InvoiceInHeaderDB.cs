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
    class invoiceinheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public int MRNNo { get; set; }
        public DateTime MRNDate { get; set; }
        public string PONos { get; set; }
        public string PODates { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string SupplierInvoiceNo { get; set; }
        public DateTime SupplierInvoiceDate { get; set; }
        public string CurrencyID { get; set; }
        public decimal ExchangeRate { get; set; }

        public double FreightCharge { get; set; }
        public double ProductValue { get; set; }
        public double ProductValueINR { get; set; }
        public double ProductTax { get; set; }
        public double ProductTaxINR { get; set; }
        public double InvoiceValue { get; set; }
        public double InvoiceValueINR { get; set; }
        public string AdvancePaymentVouchers { get; set; }
        public string Remarks { get; set; }
        public string Comments { get; set; }
        public string CommentStatus { get; set; }
        public int status { get; set; }
        public int DocumentStatus { get; set; }
        public string CreateUser { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal TDSPaid { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreateTime { get; set; }
        public string ForwarderName { get; set; }
        public string ApproverName { get; set; }
        public string ForwarderList { get; set; }

        //SJV Ref In INvoice
        public int PJVTNo { get; set; }
        public DateTime PJVTDate { get; set; }
        public int PJVNo { get; set; }
        public DateTime PJVDate { get; set; }

        public invoiceinheader()
        {
            //Reference = "";
            Comments = "";
        }
    }
    class invoiceindetail
    {
        public int RowID { get; set; }
        public int ItemReferenceNo { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        //public string Unit { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Tax { get; set; }
        public string TaxDetails { get; set; }
        public string TaxCode { get; set; }
    }
    class invoiceinpayments
    {
        public int RowID { get; set; }
        public string CustomerID { get; set; }
        public string InvoiceDocumentID { get; set; }
        public string InvoiceDocumentName { get; set; }
        public int InvoiceInNo { get; set; }
        public DateTime InvoiceInDate { get; set; }
        public int InvoiceInTemporaryNo { get; set; }
        public DateTime InvoiceInTemporaryDate { get; set; }

        public string PVDocumentID { get; set; }
        public int PVTemporaryNo { get; set; }
        public DateTime PVTemporaryDate { get; set; }
        public int PVNo { get; set; }
        public DateTime PVDate { get; set; }
        public decimal Amount { get; set; }
        public decimal TDSAmount { get; set; }
    }
    class invoiceinexpense
    {
        public int RowID { get; set; }
        public int ExpenseID { get; set; }
        public string ExpenseDescription { get; set; }
        public string DocID { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public decimal Amount { get; set; }
    }
    class InvoiceInHeaderDB
    {
        public List<invoiceinheader> getFilteredInvoiceInHeader(string userList, int opt, string userCommentStatusString)
        {
            invoiceinheader inh;
            List<invoiceinheader> InvoiceInHeaderList = new List<invoiceinheader>();
            try
            {
                //approved user comment status string
                string acStr = "";
                try
                {
                    acStr = userCommentStatusString.Substring(0, userCommentStatusString.Length - 2) + "1" + Main.delimiter2;
                }
                catch (Exception ex)
                {
                    acStr = "";
                }
                //-----
                string query1 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate," +
                    " MRNNo,MRNDate,PONOs,PODates,CustomerID,CustomerName,SupplierInvoiceNo,SupplierInvoiceDate,CurrencyID,FreightCharge," +
                    " ProductValue,ProductTax,InvoiceValue,InvoiceValueINR,AdvancePaymentVouchers,Remarks ," +
                    " CommentStatus,Status,DocumentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList " +
                    ", ExchangeRate, ProductValueINR, ProductTaxINR,PJVTNo, PJVTDate, PJVNo, PJVDate from ViewInvoiceInHeader" +
                    " where ((ForwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (CreateUser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (CommentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98))  and Status not in (7,98) order by MRNDate desc,DocumentID asc,MRNNO desc";

                string query2 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate," +
                    " MRNNo,MRNDate,PONOs,PODates,CustomerID,CustomerName,SupplierInvoiceNo,SupplierInvoiceDate,CurrencyID,FreightCharge," +
                    " ProductValue,ProductTax,InvoiceValue,InvoiceValueINR,AdvancePaymentVouchers,Remarks ," +
                    " CommentStatus,Status,DocumentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList " +
                     " ,ExchangeRate, ProductValueINR, ProductTaxINR,PJVTNo, PJVTDate, PJVNo, PJVDate from ViewInvoiceInHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) and Status not in (7,98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query3 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate," +
                    " MRNNo,MRNDate,PONOs,PODates,CustomerID,CustomerName,SupplierInvoiceNo,SupplierInvoiceDate,CurrencyID,FreightCharge," +
                    " ProductValue,ProductTax,InvoiceValue,InvoiceValueINR,AdvancePaymentVouchers,Remarks ," +
                    " CommentStatus,Status,DocumentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList " +
                      " ,ExchangeRate, ProductValueINR, ProductTaxINR,PJVTNo, PJVTDate, PJVNo, PJVDate from ViewInvoiceInHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99)  and Status = 1 order by MRNDate desc,DocumentID asc,MRNNo desc";

                string query6 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate," +
                    " MRNNo,MRNDate,PONOs,PODates,CustomerID,CustomerName,SupplierInvoiceNo,SupplierInvoiceDate,CurrencyID,FreightCharge," +
                    " ProductValue,ProductTax,InvoiceValue,InvoiceValueINR,AdvancePaymentVouchers,Remarks ," +
                    " CommentStatus,Status,DocumentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList " +
                     ", ExchangeRate, ProductValueINR, ProductTaxINR,PJVTNo, PJVTDate, PJVNo, PJVDate from ViewInvoiceInHeader" +
                    " where  DocumentStatus = 99  and Status = 1 order by MRNDate desc,DocumentID asc,MRNNo desc";

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
                        inh = new invoiceinheader();
                        inh.RowID = reader.GetInt32(0);
                        inh.DocumentID = reader.GetString(1);
                        inh.DocumentName = reader.GetString(2);

                        inh.TemporaryNo = reader.GetInt32(3);
                        inh.TemporaryDate = reader.GetDateTime(4);
                        inh.DocumentNo = reader.GetInt32(5);
                        inh.DocumentDate = reader.GetDateTime(6);
                        inh.MRNNo = reader.GetInt32(7);
                        inh.MRNDate = reader.GetDateTime(8);
                        inh.PONos = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        inh.PODates = reader.IsDBNull(10) ? "" : reader.GetString(10);
                        inh.CustomerID = reader.IsDBNull(11) ? "" : reader.GetString(11);
                        inh.CustomerName = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        inh.SupplierInvoiceNo = reader.GetString(13);
                        inh.SupplierInvoiceDate = reader.GetDateTime(14);
                        inh.CurrencyID = reader.GetString(15);
                        //inh.TaxCode = reader.GetString(16);
                        inh.FreightCharge = reader.GetDouble(16);
                        inh.ProductValue = reader.GetDouble(17);
                        inh.ProductTax = reader.GetDouble(18);
                        inh.InvoiceValue = reader.GetDouble(19);
                        inh.InvoiceValueINR = reader.GetDouble(20);
                        inh.AdvancePaymentVouchers = reader.GetString(21);
                        inh.Remarks = reader.GetString(22);
                        if (!reader.IsDBNull(23))
                        {
                            inh.CommentStatus = reader.GetString(23);
                        }
                        else
                        {
                            inh.CommentStatus = "";
                        }
                        inh.status = reader.GetInt32(24);
                        inh.DocumentStatus = reader.GetInt32(25);
                        inh.CreateUser = reader.GetString(26);
                        inh.ForwardUser = reader.GetString(27);

                        inh.ApproveUser = reader.GetString(28);
                        inh.CreatorName = reader.GetString(29);
                        inh.CreateTime = reader.GetDateTime(30);
                        inh.ForwarderName = reader.GetString(31);
                        inh.ApproverName = reader.GetString(32);
                        if (!reader.IsDBNull(33))
                        {
                            inh.ForwarderList = reader.GetString(33);
                        }
                        else
                        {
                            inh.ForwarderList = "";
                        }
                        inh.ExchangeRate = reader.GetDecimal(34);
                        inh.ProductValueINR = reader.GetDouble(35);
                        inh.ProductTaxINR = reader.GetDouble(36);
                        inh.PJVTNo = reader.IsDBNull(37) ? 0 : reader.GetInt32(37);
                        inh.PJVTDate = reader.IsDBNull(38) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(38);
                        inh.PJVNo = reader.IsDBNull(39) ? 0 : reader.GetInt32(39);
                        inh.PJVDate = reader.IsDBNull(40) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(40);
                        InvoiceInHeaderList.Add(inh);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice Header Details");
            }
            return InvoiceInHeaderList;
        }



        public static List<invoiceindetail> getInvoiceDetail(invoiceinheader inh)
        {
            invoiceindetail ind;
            List<invoiceindetail> invoiceDetailList = new List<invoiceindetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,DocumentName,TemporaryNo,TemporaryDate,StockItemID,StockItemName,ModelNo,ModelName,Quantity, " +
                   "Price,Tax,TaxDetails,TaxCode,ReferenceNo " +
                  "from ViewInvoiceInDetail " +
                  " where DocumentID='" + inh.DocumentID + "'" +
                  " and TemporaryNo=" + inh.TemporaryNo +
                  " and TemporaryDate='" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                  " order by StockItemID";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ind = new invoiceindetail();
                    ind.RowID = reader.GetInt32(0);
                    ind.DocumentID = reader.GetString(1);
                    ind.DocumentName = reader.GetString(2);
                    ind.TemporaryNo = reader.GetInt32(3);
                    ind.TemporaryDate = reader.GetDateTime(4).Date;
                    ind.StockItemID = reader.GetString(5);
                    ind.StockItemName = reader.GetString(6);
                    ind.ModelNo = reader.IsDBNull(7) ? "NA" : reader.GetString(7);
                    ind.ModelName = reader.IsDBNull(8) ? "NA" : reader.GetString(8);
                    ind.Quantity = reader.GetDouble(9);
                    ind.Price = reader.GetDouble(10);
                    ind.Tax = reader.GetDouble(11);
                    ind.TaxDetails = reader.GetString(12);
                    ind.TaxCode = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    ind.ItemReferenceNo = reader.IsDBNull(14) ? 0 : reader.GetInt32(14);
                    invoiceDetailList.Add(ind);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Invoice Details");
            }
            return invoiceDetailList;
        }
        public Boolean validateInvoiceInHeader(invoiceinheader inh)
        {
            Boolean status = true;
            try
            {
                if (inh.DocumentID.Trim().Length == 0 || inh.DocumentID == null)
                {
                    return false;
                }
                if (inh.MRNNo == 0)
                {
                    return false;
                }
                if (inh.MRNDate == null || inh.MRNDate > DateTime.Now)
                {
                    return false;
                }
                if (inh.SupplierInvoiceNo.Trim().Length == 0 || inh.SupplierInvoiceNo == null)
                {

                    return false;
                }
                if (inh.SupplierInvoiceDate == null || inh.SupplierInvoiceDate > DateTime.Now)
                {

                    return false;
                }

                if (inh.CurrencyID == null || inh.CurrencyID.Trim().Length == 0)
                {
                    return false;
                }
                //if (inh.TaxCode == null || inh.TaxCode.Trim().Length == 0)
                //{
                //    return false;
                //}
                ////if (inh.FreightCharge == 0 )
                ////{
                ////    return false;
                ////}
                if (inh.ProductValue == 0)
                {
                    return false;
                }
                if (inh.InvoiceValue == 0)
                {
                    return false;
                }
                if (inh.ProductValueINR == 0)
                {
                    return false;
                }
                if (inh.ExchangeRate == 0)
                {
                    return false;
                }
                if (inh.InvoiceValueINR == 0)
                {
                    return false;
                }
                //if (inh.AdvancePaymentVouchers.Trim().Length == 0 || inh.AdvancePaymentVouchers == null)
                //{
                //    return false;
                //}
                if (inh.Remarks.Trim().Length == 0 || inh.Remarks == null)
                {
                    return false;
                }

                //if (mrnh.TaxAmount == 0)
                //{
                //    return false;
                //}
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public Boolean forwardInvoiceInHeader(invoiceinheader inh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InvoiceInHeader set DocumentStatus=" + (inh.DocumentStatus + 1) +
                    ", forwardUser='" + inh.ForwardUser + "'" +
                    ", commentStatus='" + inh.CommentStatus + "'" +
                    ", ForwarderList='" + inh.ForwarderList + "'" +
                    // ", QCStatus= " + imh.QCStatus +
                    " where DocumentID='" + inh.DocumentID + "'" +
                    " and TemporaryNo=" + inh.TemporaryNo +
                    " and TemporaryDate='" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InvoiceInHeader", "", updateSQL) +
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

        public Boolean reverseInvoiceInHeader(invoiceinheader inh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InvoiceInHeader set DocumentStatus=" + inh.DocumentStatus +
                    // ",QCStatus=" + mrnh.QCStatus +
                    ", forwardUser='" + inh.ForwardUser + "'" +
                    ", commentStatus='" + inh.CommentStatus + "'" +
                    ", ForwarderList='" + inh.ForwarderList + "'" +
                    " where DocumentID='" + inh.DocumentID + "'" +
                    " and TemporaryNo=" + inh.TemporaryNo +
                    " and TemporaryDate='" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InvoiceInHeader", "", updateSQL) +
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

        public Boolean ApproveInvoiceInHeader(invoiceinheader inh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InvoiceInHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + inh.CommentStatus + "'" +
                    ", DocumentNo=" + inh.DocumentNo +
                    ", DocumentDate=convert(date, getdate())" +
                    " where DocumentID='" + inh.DocumentID + "'" +
                    " and TemporaryNo=" + inh.TemporaryNo +
                    " and TemporaryDate='" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InvoiceInHeader", "", updateSQL) +
                Main.QueryDelimiter;

                string narration = "Puchase against Invoice No " + inh.DocumentNo + "," +
               "Dated " + UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy") + "," +
               "Party:" + inh.CustomerName;

                int PJVNo = 0; //Journal No
                DateTime PJVDate = DateTime.Parse("1900-01-01"); //Journal Date
                //int SJVTempNo = 0; //Temporary No
                //DateTime SJVTempDate = DateTime.Parse("1900-01-01"); //Temporary Date

                if (inh.PJVNo == 0 && inh.PJVTNo > 0) // JV Available but not approved
                {
                    PJVNo = DocumentNumberDB.getNewNumber("PJV", 2);
                    PJVDate = UpdateTable.getSQLDateTime();
                }
                else //JV Available and approved // JV Not available
                {
                    PJVNo = inh.PJVNo;
                    PJVDate = inh.PJVDate;
                }

                updateSQL = "update PJVHeader set DocumentStatus=99, status=1 ,InvReferenceNo = " + inh.RowID +
                  ", ApproveUser='" + Login.userLoggedIn + "'" +
                  ", JournalNo=" + PJVNo +
                  ", JournalDate= '" + PJVDate.ToString("yyyy-MM-dd") +
                   "', Narration='" + narration + "'" +
                  " where InvDocumentID='" + inh.DocumentID + "'" +
                  " and InvTempNo=" + inh.TemporaryNo +
                  " and InvTempDate='" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "PJVHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "update InvoiceInHeader set PJVNo='" + PJVNo + "'" +
                  ", PJVDate='" + PJVDate.ToString("yyyy-MM-dd") + "'" +
                 " where DocumentID='" + inh.DocumentID + "'" +
                 " and TemporaryNo=" + inh.TemporaryNo +
                 " and TemporaryDate='" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InvoiceInHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "update InvoiceInPayments set " +
                    " InvoiceInNo=" + inh.DocumentNo +
                    ", InvoiceInDate=convert(date, getdate())" +
                    "  where InvoiceDocumentID='" + inh.DocumentID + "'" +
                    " and InvoiceInTemporaryNo=" + inh.TemporaryNo +
                    " and InvoiceInTemporaryDate='" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
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
        public static string getUserComments(string docid, int tempno, DateTime tempdate)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from InvoiceInHeader where DocumentID='" + docid + "'" +
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
        public List<invoiceinheader> getInvoiceInDetailList(string CustCode)
        {
            invoiceinheader inh;
            List<invoiceinheader> InvoiceInHeaderList = new List<invoiceinheader>();
            try
            {
                string query = "select a.DocumentID,a.DocumentNo,a.DocumentDate," +
                    " a.SupplierInvoiceNo,a.SupplierInvoiceDate,c.Name, a.InvoiceValue,a.Remarks,isnull(b.AmountPaid,0) as AmountPaid,isnull(b.TDSPaid,0) as TDSPaid,a.TemporaryNo,a.TemporaryDate " +
                    " from InvoiceInHeader a left outer join ViewInvoiceInPaymentSummary as b  " +
                     " on a.DocumentNo = b.InvoiceInNo and a.DocumentDate=b.InvoiceInDate and a.DocumentID = b.InvoiceDocumentID " +
                    " left outer join Currency c on a.CurrencyID = c.CurrencyID where  a.DocumentStatus = 99  and a.Status = 1 and a.CustomerID = '" + CustCode + "'" +
                    " order by DocumentDate desc";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        inh = new invoiceinheader();
                        inh.DocumentID = reader.GetString(0);
                        inh.DocumentNo = reader.GetInt32(1);
                        inh.DocumentDate = reader.GetDateTime(2);
                        inh.SupplierInvoiceNo = reader.GetString(3);
                        inh.SupplierInvoiceDate = reader.GetDateTime(4);
                        inh.CurrencyID = reader.GetString(5); //currency name
                        inh.InvoiceValue = reader.GetDouble(6); 
                        inh.Remarks = reader.IsDBNull(7) ? "" : reader.GetString(7);
                        inh.AmountPaid = reader.GetDecimal(8); //Total Amount Paid
                        inh.TDSPaid = reader.GetDecimal(9); //Total TDS Paid
                        inh.TemporaryNo = reader.GetInt32(10);
                        inh.TemporaryDate = reader.GetDateTime(11);
                        InvoiceInHeaderList.Add(inh);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice Header Details");
            }
            return InvoiceInHeaderList;
        }
        public static DataGridView getGridViewForInvoiceIN(string CustCode)
        {

            DataGridView grdInv = new DataGridView();
            try
            {
                string[] strColArr = { "DocID", "DocNO","DocDate","SupplierInvNo", "SupplierInvDate","Currency",
                    "InvoiceValue","AmountPaid","AmountToPay","TDSPaid","TDSToPay","Remarks","TempNo","TempDate"};
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                };

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdInv.EnableHeadersVisualStyles = false;
                grdInv.AllowUserToAddRows = false;
                grdInv.AllowUserToDeleteRows = false;
                grdInv.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdInv.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdInv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdInv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdInv.ColumnHeadersHeight = 27;
                grdInv.RowHeadersVisible = false;
                grdInv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;
                grdInv.Columns.Add(colChk);
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str.Replace(" ", string.Empty);
                    colArr[index].HeaderText = str;
                    if (index == 8 || index == 10)
                        colArr[index].ReadOnly = false;
                    else
                        colArr[index].ReadOnly = true;
                    if (index == 2 || index == 4)
                        colArr[index].DefaultCellStyle.Format = "dd-MM-yyyy";
                    if (index == 1 || index == 2)
                        colArr[index].Width = 65;
                    else
                        colArr[index].Width = 92;
                    if (index == 12 || index == 13)
                        colArr[index].Visible = false;
                    else
                        colArr[index].Visible = true;
                    grdInv.Columns.Add(colArr[index]);
                }
                InvoiceInHeaderDB inhDb = new InvoiceInHeaderDB();
                List<invoiceinheader> IHList = inhDb.getInvoiceInDetailList(CustCode); //All INvoice List
                foreach (invoiceinheader iih in IHList)
                {
                    grdInv.Rows.Add();
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[0]].Value = iih.DocumentID;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[1]].Value = iih.DocumentNo;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[2]].Value = iih.DocumentDate;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[3]].Value = iih.SupplierInvoiceNo;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[4]].Value = iih.SupplierInvoiceDate;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[5]].Value = iih.CurrencyID;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[6]].Value = iih.InvoiceValue;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[7]].Value = iih.AmountPaid;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[8]].Value = Convert.ToDecimal(0);
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[9]].Value = iih.TDSPaid;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[10]].Value = Convert.ToDecimal(0);
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[11]].Value = iih.Remarks;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[12]].Value = iih.TemporaryNo;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[13]].Value = iih.TemporaryDate;
                }
            }
            catch (Exception ex)
            {
            }

            return grdInv;
        }


        public static ListView getINvoiceInDetailListView(string CustCode)
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
                lv.Columns.Add("Sel", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Doc ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Doc NO", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Doc Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Supplier Inv No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Supplier Inv Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("InvoiceValue INR", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Amount Paid", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Amount To Pay", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Remarks", -2, HorizontalAlignment.Left);
                InvoiceInHeaderDB inhDb = new InvoiceInHeaderDB();

                List<invoiceinheader> IHList = inhDb.getInvoiceInDetailList(CustCode);
                foreach (invoiceinheader iih in IHList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = false;
                    item.SubItems.Add(iih.DocumentID.ToString());
                    item.SubItems.Add(iih.DocumentNo.ToString());
                    item.SubItems.Add(iih.DocumentDate.ToString("yyyy-MM-dd"));
                    item.SubItems.Add(iih.SupplierInvoiceNo.ToString());
                    item.SubItems.Add(iih.SupplierInvoiceDate.ToString("yyyy-MM-dd"));
                    item.SubItems.Add(iih.InvoiceValueINR.ToString());
                    item.SubItems.Add(iih.AmountPaid.ToString());
                    item.SubItems.Add(iih.Remarks.ToString());
                    lv.Items.Add(item);
                }
            }
            catch (Exception ex)
            {

            }
            return lv;
        }
        public Boolean updateInvoiceINHeaderAndDetail(invoiceinheader inh, invoiceinheader previnh,
                                                            List<invoiceindetail> indList, List<invoiceinpayments> payList,List<invoiceinexpense> expList)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InvoiceInHeader set MRNNo='" + inh.MRNNo +
                    "',MRNDate='" + inh.MRNDate.ToString("yyyy-MM-dd") +
                     "',SupplierInvoiceNo='" + inh.SupplierInvoiceNo +
                     "',SupplierInvoiceDate='" + inh.SupplierInvoiceDate.ToString("yyyy-MM-dd") +
                    "', CurrencyID='" + inh.CurrencyID +
                     "', ExchangeRate=" + inh.ExchangeRate +
                    ", CustomerID='" + inh.CustomerID +
                    "', FreightCharge=" + inh.FreightCharge +
                    ", ProductValue=" + inh.ProductValue +
                     ", ProductValueINR=" + inh.ProductValueINR +
                    ", ProductTax=" + inh.ProductTax +
                     ", ProductTaxINR=" + inh.ProductTaxINR +
                     ", InvoiceValue =" + inh.InvoiceValue +
                      ", InvoicevalueINR =" + inh.InvoiceValueINR +
                       ", AdvancePaymentvouchers ='" + inh.AdvancePaymentVouchers +
                   "', Remarks='" + inh.Remarks +
                    "', Comments='" + inh.Comments +
                    "', CommentStatus='" + inh.CommentStatus +
                     "', ForwarderList='" + inh.ForwarderList + "'" +

                    " where DocumentID='" + inh.DocumentID + "'" +
                    " and TemporaryNo=" + inh.TemporaryNo +
                    " and TemporaryDate='" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InvoiceInheader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from InvoiceInDetail where DocumentID='" + inh.DocumentID + "'" +
                    " and TemporaryNo=" + inh.TemporaryNo +
                    " and TemporaryDate='" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InvoiceInDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (invoiceindetail ind in indList)
                {
                    updateSQL = "insert into InvoiceInDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,TaxCode,Quantity,Price,Tax,TaxDetails,ReferenceNo) " +
                    "values ('" + ind.DocumentID + "'," +
                    ind.TemporaryNo + "," +
                    "'" + ind.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + ind.StockItemID + "'," +
                     "'" + ind.ModelNo + "'," +
                     "'" + ind.TaxCode + "'," +
                   ind.Quantity + "," +
                   ind.Price + "," +
                   ind.Tax + "," +
                    "'" + ind.TaxDetails + "'," + ind.ItemReferenceNo + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InvoiceInDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                }

                updateSQL = "Delete from InvoiceInPayments where InvoiceDocumentID = '"+inh.DocumentID+"'"+
                    " and InvoiceInTemporaryNo='" + inh.TemporaryNo + "'" +
                    " and InvoiceInTemporaryDate='" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InvoiceInPayments", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (invoiceinpayments pay in payList)
                {
                    updateSQL = "insert into InvoiceInPayments " +
                    "(InvoiceDocumentID,CustomerID,InvoiceInNo,InvoiceInDate,InvoiceInTemporaryNo,InvoiceInTemporaryDate,PVDocumentID,PVTemporaryNo,PVTemporaryDate,PVNo,PVDate," +
                    "Amount,TDSAmount) " +
                    "values ('" + inh.DocumentID + "'," +
                     "'" + pay.CustomerID + "',0," +
                    "'1900-01-01'," +
                     +inh.TemporaryNo + "," +
                    "'" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + pay.PVDocumentID + "'," +
                        +pay.PVTemporaryNo + "," +
                    "'" + pay.PVTemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       +pay.PVNo + "," +
                    "'" + pay.PVDate.ToString("yyyy-MM-dd") + "'," +
                   pay.Amount + "," + pay.TDSAmount + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InvoiceInPayments", "", updateSQL) +
                    Main.QueryDelimiter;
                }

                updateSQL = "Delete from InvoiceExpense where DocumentID = '" + inh.DocumentID + "'" +
                   " and TemporaryNo='" + inh.TemporaryNo + "'" +
                   " and TemporaryDate='" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InvoiceExpense", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (invoiceinexpense exp in expList)
                {
                    updateSQL = "insert into InvoiceExpense " +
                    "(DocumentID,TemporaryNo,TemporaryDate,ExpenseID,Amount) " +
                    "values ('" + inh.DocumentID + "'," +
                     +inh.TemporaryNo + "," +
                    "'" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                        +exp.ExpenseID + "," +
                       +exp.Amount + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InvoiceExpense", "", updateSQL) +
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
        public Boolean InsertInvoiceINHeaderAndDetail(invoiceinheader inh, List<invoiceindetail> indList, 
                                                                out int Tno, List<invoiceinpayments> payList, List<invoiceinexpense> expList)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            Tno = 0;
            try
            {
                inh.TemporaryNo = DocumentNumberDB.getNumber(inh.DocumentID, 1);
                Tno = inh.TemporaryNo;
                if (inh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + inh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + inh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into InvoiceInHeader " +
                    "(DocumentID,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate,MRNNO,MRNDate,SupplierInvoiceNo,SupplierInvoiceDate,CustomerID," +
                    "CurrencyID,ExchangeRate,FreightCharge,ProductValue,ProductValueINR,ProductTax,ProductTaxINR,InvoiceValue,InvoiceValueINR,AdvancePaymentVouchers,Remarks,Comments,CommentStatus," +
                    "CreateUser,CreateTime,ForwarderList,DocumentStatus,Status)" +
                    " values (" +
                    "'" + inh.DocumentID + "'," +
                    inh.TemporaryNo + "," +
                    "'" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                   inh.DocumentNo + "," +
                    "'" + inh.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                    inh.MRNNo + "," +
                    "'" + inh.MRNDate.ToString("yyyy-MM-dd") + "','" +
                    inh.SupplierInvoiceNo + "'," +
                    "'" + inh.SupplierInvoiceDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + inh.CustomerID + "'," + //customeriD
                    "'" + inh.CurrencyID + "'," +
                     +inh.ExchangeRate + "," +
                     +inh.FreightCharge + "," +
                      +inh.ProductValue + "," +
                       +inh.ProductValueINR + "," +
                       +inh.ProductTax + "," +
                        +inh.ProductTaxINR + "," +
                        +inh.InvoiceValue + "," +
                         +inh.InvoiceValueINR + "," +
                    "'" + inh.AdvancePaymentVouchers + "'," +
                    "'" + inh.Remarks + "'," +
                    "'" + inh.Comments + "'," +
                     "'" + inh.CommentStatus + "'," +
                      "'" + Login.userLoggedIn + "'," +
                      "GETDATE()" + "," +
                    "'" + inh.ForwarderList + "'," +
                    inh.DocumentStatus + "," +
                         inh.status + ")";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "InvoiceInHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from InvoiceInDetail where DocumentID='" + inh.DocumentID + "'" +
                    " and TemporaryNo=" + inh.TemporaryNo +
                    " and TemporaryDate='" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InvoiceInDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (invoiceindetail ind in indList)
                {
                    updateSQL = "insert into InvoiceInDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,TaxCode,Quantity,Price,Tax,TaxDetails,ReferenceNo) " +
                    "values ('" + ind.DocumentID + "'," +
                    inh.TemporaryNo + "," +
                    "'" + ind.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + ind.StockItemID + "'," +
                     "'" + ind.ModelNo + "'," +
                      "'" + ind.TaxCode + "'," +
                   ind.Quantity + "," +
                   ind.Price + "," +
                   ind.Tax + "," +
                    "'" + ind.TaxDetails + "'," + ind.ItemReferenceNo + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InvoiceInDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                }

                updateSQL = "Delete from InvoiceInPayments where InvoiceDocumentID = '" + inh.DocumentID + "'" +
                  " and InvoiceInTemporaryNo='" + inh.TemporaryNo + "'" +
                  " and InvoiceInTemporaryDate='" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InvoiceInPayments", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (invoiceinpayments pay in payList)
                {
                    updateSQL = "insert into InvoiceInPayments " +
                    "(InvoiceDocumentID,CustomerID,InvoiceInNo,InvoiceInDate,InvoiceInTemporaryNo,InvoiceInTemporaryDate,PVDocumentID,PVTemporaryNo,PVTemporaryDate,PVNo,PVDate," +
                    "Amount,TDSAmount) " +
                    "values ('" + inh.DocumentID + "'," +
                     "'" + pay.CustomerID + "',0," +
                    "'1900-01-01'," +
                     +inh.TemporaryNo + "," +
                    "'" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'"+ pay.PVDocumentID + "'," +
                        +pay.PVTemporaryNo + "," +
                    "'" + pay.PVTemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       +pay.PVNo + "," +
                    "'" + pay.PVDate.ToString("yyyy-MM-dd") + "'," +
                   pay.Amount +","+ pay.TDSAmount + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InvoiceInPayments", "", updateSQL) +
                    Main.QueryDelimiter;
                }

                updateSQL = "Delete from InvoiceExpense where DocumentID = '" + inh.DocumentID + "'" +
                  " and TemporaryNo='" + inh.TemporaryNo + "'" +
                  " and TemporaryDate='" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InvoiceExpense", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (invoiceinexpense exp in expList)
                {
                    updateSQL = "insert into InvoiceExpense " +
                    "(DocumentID,TemporaryNo,TemporaryDate,ExpenseID,Amount) " +
                    "values ('" + inh.DocumentID + "'," +
                     +inh.TemporaryNo + "," +
                    "'" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                        +exp.ExpenseID + "," +
                       +exp.Amount + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InvoiceExpense", "", updateSQL) +
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
        //get all invoice prepared again one workorder
        public static List<invoiceinheader> getInvoiceListAgainstOneWO(int woNo, DateTime wodate)
        {
            invoiceinheader inh;
            List<invoiceinheader> InvoiceInHeaderList = new List<invoiceinheader>();
            try
            {
                string query = "select a.DocumentID,a.TemporaryNo, a.TemporaryDate,a.DocumentNo,a.DocumentDate,a.MRNNo, " +
                     "a.MRNDate, b.StockItemID, c.Name,b.Quantity,b.ReferenceNo from InvoiceInHeader a, InvoiceInDetail b, ServiceItem c" +
                    " where a.DocumentID = b.DocumentID and a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate " +
                    "  and b.StockItemID = c.ServiceItemID and a.DocumentID = 'WOINVOICEIN'  and a.MRNNo = " + woNo +
                    " and a.MRNDate = '" + wodate.ToString("yyyy-MM-dd") + "'" +
                    " and a.status = 1 and a.DocumentStatus = 99 order by a.DocumentNo desc";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        inh = new invoiceinheader();
                        inh.DocumentID = reader.GetString(0);
                        inh.TemporaryNo = reader.GetInt32(1);
                        inh.TemporaryDate = reader.GetDateTime(2);
                        inh.DocumentNo = reader.GetInt32(3);
                        inh.DocumentDate = reader.GetDateTime(4);
                        inh.MRNNo = reader.GetInt32(5);
                        inh.MRNDate = reader.GetDateTime(6);

                        inh.CreateUser = reader.GetString(7); //For StockItemID
                        inh.CreatorName = reader.GetString(8); //For StockItemName
                        inh.InvoiceValue = reader.GetDouble(9); //For WO Quant
                        inh.RowID = reader.GetInt32(10); //For WO RowId (Ref NO)
                        InvoiceInHeaderList.Add(inh);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice Header Details");
            }
            return InvoiceInHeaderList;
        }
        public static double getItemWiseTotalQuantOFWOIssuedInvoiceIn(int refNo)
        {
            double TotQuant = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select SUM(a.Quantity) from InvoiceInDetail a , InvoiceInHeader b where a.DocumentID = b.DocumentID and " +
                    " a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate and a.DocumentID = 'WOINVOICEIN' and a.ReferenceNo =" + refNo +
                    " and b.Status = 1 and b.DocumentStatus = 99";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    TotQuant = reader.IsDBNull(0) ? 0 : reader.GetDouble(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return TotQuant;
        }
        public List<invoiceinheader> getInvoicedata()
        {
            invoiceinheader popid;
            List<invoiceinheader> POPIDetail = new List<invoiceinheader>();
            try
            {
                //string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID,DocumentNo,DocumentDate, MRNNo,MRNDate,InvoiceValueINR,ProductValueINR from ViewInvoiceInHeader  where Status =1 and DocumentStatus = 99 ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popid = new invoiceinheader();
                    popid.DocumentID = reader.GetString(0);
                    popid.DocumentNo = reader.GetInt32(1);
                    popid.DocumentDate = reader.GetDateTime(2);
                    popid.MRNNo = reader.GetInt32(3);
                    popid.MRNDate = reader.GetDateTime(4);
                    popid.InvoiceValueINR = reader.GetDouble(5);
                    popid.ProductValueINR = reader.GetDouble(6);
                    POPIDetail.Add(popid);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying PO Product Inward Details");
            }
            return POPIDetail;
        }
        public static string getCustIDOfINvoiceIN(invoiceinheader inh)
        {
            string custID = "";
            try
            {
                string query = "select distinct CustomerID" +
                     " from ViewInvoiceInHeader" +
                   " where DocumentID='" + inh.DocumentID + "'" +
                  " and TemporaryNo=" + inh.TemporaryNo +
                  " and TemporaryDate='" + inh.TemporaryDate.ToString("yyyy-MM-dd") + "'";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    custID = reader.IsDBNull(0) ? "" : reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice Header CustoemrID");
            }
            return custID;
        }

        public static List<invoiceinpayments> getInvoiceInPaymentDetails(int PVTempNo, DateTime PVTempDate,string PVDocID)
        {
            List<invoiceinpayments> payLIst = new List<invoiceinpayments>();
            try
            {
                invoiceinpayments pay = new invoiceinpayments();
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,InvoiceDocumentID,InvoiceInNo,InvoiceInDate, PVTemporaryNo," +
                    "PVTemporaryDate,PVNo,PVDate,Amount,CustomerID, InvoiceInTemporaryNo,InvoiceInTemporaryDate,PVDocumentID,TDSAmount from InvoiceInPayments  where PVTemporaryNo = " + PVTempNo +
                    " and PVTemporaryDate = '" + PVTempDate.ToString("yyyy-MM-dd") + "' and PVDocumentID = '" + PVDocID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pay = new invoiceinpayments();
                    pay.RowID = reader.GetInt32(0);
                    pay.InvoiceDocumentID = reader.GetString(1);
                    pay.InvoiceInNo = reader.GetInt32(2);
                    pay.InvoiceInDate = reader.GetDateTime(3);
                    pay.PVTemporaryNo = reader.GetInt32(4);
                    pay.PVTemporaryDate = reader.GetDateTime(5);
                    pay.PVNo = reader.GetInt32(6);
                    pay.PVDate = reader.GetDateTime(7);
                    pay.Amount = reader.GetDecimal(8);
                    pay.CustomerID = reader.IsDBNull(9)?"":reader.GetString(9);
                    pay.InvoiceInTemporaryNo = reader.IsDBNull(10)?0:reader.GetInt32(10);
                    pay.InvoiceInTemporaryDate = reader.IsDBNull(11) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(11);
                    pay.PVDocumentID = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    pay.TDSAmount = reader.IsDBNull(13) ? Convert.ToDecimal(13) : reader.GetDecimal(13);
                    payLIst.Add(pay);
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice in Payent details");
            }
            return payLIst;
        }
        public static List<paymentvoucher> getPVlistOFCustomerForInvoiceIN(string custid)
        {
            List<paymentvoucher> pvLIst = new List<paymentvoucher>();
            try
            {
                paymentvoucher pay = new paymentvoucher();
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.VoucherNo, a.VoucherDate,a.TemporaryNo,a.TemporaryDate, a.VoucherAmount, isnull(b.TotAdjusted,0),a.DocumentID,c.Name,isnull(b.TotTDSAdjusted,0) from PaymentVoucherHeader a left outer join " +
                    "(select PVDocumentID,PVTemporaryNo, PVTemporaryDate, SUM(Amount) TotAdjusted, SUM(TDSAmount) TotTDSAdjusted from InvoiceInPayments group by PVDocumentID,PVTemporaryNo, PVTemporaryDate) b" +
                    " on a.DocumentID = b.PVDocumentID and a.TemporaryNo = b.PVTemporaryNo and a.TemporaryDate = b.PVTemporaryDate  " +
                    "  left outer join Currency c on a.CurrencyID = c.CurrencyID where a.DocumentStatus = 99 and a.Status = 1 and a.SLCode = '" + custid + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pay = new paymentvoucher();
                    pay.VoucherNo = reader.GetInt32(0);
                    pay.VoucherDate = reader.GetDateTime(1);
                    pay.TemporaryNo = reader.GetInt32(2);
                    pay.TemporaryDate = reader.GetDateTime(3);
                    pay.VoucherAmount = reader.GetDecimal(4);
                    pay.TotalAdjusted = reader.GetDecimal(5);
                    pay.DocumentID = reader.IsDBNull(6)? "":reader.GetString(6);
                    pay.CurrencyID = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    pay.TotalTDSAdjusted = reader.IsDBNull(8) ? 0 : reader.GetDecimal(8);
                    pvLIst.Add(pay);
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Payment voucehr details");
            }
            return pvLIst;
        }

        public static DataGridView getGridViewOFPVForAdvAdjustment(string CustCode)
        {

            DataGridView grdInv = new DataGridView();
            try
            {
                string[] strColArr = { "VoucherID","VoucherNO", "VoucherDate","VoucherTempNo","VoucherTempDate",
                                    "Currency","VoucherAmt", "AmountAdjusted", "AmountToAdjust","TDSAdjusted","TDSToAdjust" };
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                };

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdInv.EnableHeadersVisualStyles = false;
                grdInv.AllowUserToAddRows = false;
                grdInv.AllowUserToDeleteRows = false;
                grdInv.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdInv.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdInv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdInv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdInv.ColumnHeadersHeight = 27;
                grdInv.RowHeadersVisible = false;
                grdInv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;
                grdInv.Columns.Add(colChk);
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str.Replace(" ", string.Empty);
                    colArr[index].HeaderText = str;
                    if (index == 8 || index == 10)
                        colArr[index].ReadOnly = false;
                    else
                        colArr[index].ReadOnly = true;
                    if (index == 2 || index == 4)
                        colArr[index].DefaultCellStyle.Format = "dd-MM-yyyy";
                    if (index == 1)
                        colArr[index].Width = 80;
                    else if (index == 0)
                        colArr[index].Width = 150;
                    else
                        colArr[index].Width = 110;
                    if (index == 3 || index == 4)
                        colArr[index].Visible = false;
                    else
                        colArr[index].Visible = true;
                    grdInv.Columns.Add(colArr[index]);
                }
                List<paymentvoucher> PVList = getPVlistOFCustomerForInvoiceIN(CustCode); //All voucher List

                foreach (paymentvoucher pvh in PVList)
                {
                    grdInv.Rows.Add();
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[0]].Value = pvh.DocumentID;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[1]].Value = pvh.VoucherNo;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[2]].Value = pvh.VoucherDate;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[3]].Value = pvh.TemporaryNo;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[4]].Value = pvh.TemporaryDate;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[5]].Value = pvh.CurrencyID;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[6]].Value = pvh.VoucherAmount;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[7]].Value = pvh.TotalAdjusted;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[8]].Value = Convert.ToDecimal(0);
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[9]].Value = pvh.TotalTDSAdjusted;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[10]].Value = Convert.ToDecimal(0);
                }
            }
            catch (Exception ex)
            {
            }

            return grdInv;
        }

        public static List<invoiceinpayments> getInvoiceInAdvPaymentDetails(int InvTempNo, DateTime InvTempDate, string INvDocID)
        {
            List<invoiceinpayments> payLIst = new List<invoiceinpayments>();
            try
            {
                invoiceinpayments pay = new invoiceinpayments();
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,InvoiceDocumentID,InvoiceInNo,InvoiceInDate,InvoiceInTemporaryNo,InvoiceInTemporaryDate, PVTemporaryNo," +
                    "PVTemporaryDate,PVNo,PVDate,Amount,CustomerID,PVDocumentID,TDSAmount from InvoiceInPayments   where InvoiceDocumentID='" + INvDocID + "'" +
                    " and InvoiceInTemporaryNo=" + InvTempNo +
                    " and InvoiceInTemporaryDate='" + InvTempDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pay = new invoiceinpayments();
                    pay.RowID = reader.GetInt32(0);
                    pay.InvoiceDocumentID = reader.GetString(1);
                    pay.InvoiceInNo = reader.GetInt32(2);
                    pay.InvoiceInDate = reader.GetDateTime(3);
                    pay.InvoiceInTemporaryNo = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                    pay.InvoiceInTemporaryDate = reader.IsDBNull(5) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(5);
                    pay.PVTemporaryNo = reader.GetInt32(6);
                    pay.PVTemporaryDate = reader.GetDateTime(7);
                    pay.PVNo = reader.GetInt32(8);
                    pay.PVDate = reader.GetDateTime(9);
                    pay.Amount = reader.GetDecimal(10);
                    pay.CustomerID = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    pay.PVDocumentID = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    pay.TDSAmount = reader.GetDecimal(13);
                    payLIst.Add(pay);
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice in Payent details");
            }
            return payLIst;
        }

        public static DataGridView getGridViewOFInvoiceExpense()
        {
            DataGridView grdInv = new DataGridView();
            try
            {
                string[] strColArr = { "ExpenseID", "ExpenseDesc", "Amount"};
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),
                };
                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdInv.EnableHeadersVisualStyles = false;
                grdInv.AllowUserToAddRows = false;
                grdInv.AllowUserToDeleteRows = false;
                grdInv.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdInv.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdInv.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdInv.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdInv.ColumnHeadersHeight = 27;
                grdInv.RowHeadersVisible = false;
                //DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                //colChk.Width = 50;
                //colChk.Name = "Select";
                //colChk.HeaderText = "Select";
                //colChk.ReadOnly = false;
                //grdInv.Columns.Add(colChk);
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str.Replace(" ", string.Empty);
                    colArr[index].HeaderText = str;
                    if (index == 2)
                        colArr[index].ReadOnly = false;
                    else
                        colArr[index].ReadOnly = true;
                    if (index == 1)
                        colArr[index].Width = 200;
                    else if(index == 2)
                        colArr[index].Width = 120;
                    grdInv.Columns.Add(colArr[index]);
                }
                List<invoiceinexpense> ExpList = getExpenseDetialForInvoiceINGridList(); //All Expense List

                foreach (invoiceinexpense exp in ExpList)
                {
                    grdInv.Rows.Add();
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[0]].Value = exp.ExpenseID;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[1]].Value = exp.ExpenseDescription;
                    grdInv.Rows[grdInv.Rows.Count - 1].Cells[strColArr[2]].Value = Convert.ToDecimal(0);
                }
            }
            catch (Exception ex)
            {
            }

            return grdInv;
        }

        public static List<invoiceinexpense> getExpenseDetialForInvoiceINGridList()
        {
            List<invoiceinexpense> ExpLIst = new List<invoiceinexpense>();
            try
            {
                invoiceinexpense exp = new invoiceinexpense();
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,ExpenseDescription,Status From InvoiceExpenseCatalogue where Status = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    exp = new invoiceinexpense();
                    exp.RowID = reader.GetInt32(0);
                    exp.ExpenseID = reader.GetInt32(0); 
                    exp.ExpenseDescription = reader.GetString(1);

                    ExpLIst.Add(exp);
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Expense details");
            }
            return ExpLIst;
        }

        public static List<invoiceinexpense> getExpenseDetialForInvoiceIN(invoiceinheader iih)
        {
            List<invoiceinexpense> ExpLIst = new List<invoiceinexpense>();
            try
            {
                invoiceinexpense exp = new invoiceinexpense();
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.RowID,a.ExpenseID,b.ExpenseDescription,a.Amount From InvoiceExpense a, InvoiceExpenseCatalogue b " +
                    " where a.ExpenseID = b.RowID and a.DocumentID = '" + iih.DocumentID + "'"+
                    " and a.TemporaryNo = " + iih.TemporaryNo + " and a.TemporaryDate = '" + iih.TemporaryDate.ToString("yyyy-MM-dd")+ "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    exp = new invoiceinexpense();
                    exp.RowID = reader.GetInt32(0);
                    exp.ExpenseID = reader.GetInt32(1); 
                    exp.ExpenseDescription = reader.GetString(2);
                    exp.Amount = reader.IsDBNull(3)? 0 : reader.GetDecimal(3);
                    
                    ExpLIst.Add(exp);
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Expense details");
            }
            return ExpLIst;
        }
        public List<invoiceinheader> getFilteredInvoiceInHeaderList(string Docid, int tempno, DateTime tempdate)
        {
            invoiceinheader inh;
            List<invoiceinheader> InvoiceInHeaderList = new List<invoiceinheader>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate, MRNNo,MRNDate,PONOs,PODates,CustomerID," +
                               " CustomerName,SupplierInvoiceNo,SupplierInvoiceDate,CurrencyID,FreightCharge, ProductValue,ProductTax,InvoiceValue,InvoiceValueINR," +
                               " AdvancePaymentVouchers,Remarks , CommentStatus,Status,DocumentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime," +
                               " ForwarderName,ApproverName,ForwarderList , ExchangeRate, ProductValueINR, ProductTaxINR,PJVTNo, PJVTDate, PJVNo, PJVDate" +
                               " from ViewInvoiceInHeader where DocumentID = '" + Docid + "' and TemporaryNo = " + tempno + " and" +
                               " TemporaryDate = '" + tempdate.ToString("yyyy-MM-dd") + "' and DocumentStatus = 99  and Status = 1 order by MRNDate desc, DocumentID asc,MRNNo desc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        inh = new invoiceinheader();
                        inh.RowID = reader.GetInt32(0);
                        inh.DocumentID = reader.GetString(1);
                        inh.DocumentName = reader.GetString(2);

                        inh.TemporaryNo = reader.GetInt32(3);
                        inh.TemporaryDate = reader.GetDateTime(4);
                        inh.DocumentNo = reader.GetInt32(5);
                        inh.DocumentDate = reader.GetDateTime(6);
                        inh.MRNNo = reader.GetInt32(7);
                        inh.MRNDate = reader.GetDateTime(8);
                        inh.PONos = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        inh.PODates = reader.IsDBNull(10) ? "" : reader.GetString(10);
                        inh.CustomerID = reader.IsDBNull(11) ? "" : reader.GetString(11);
                        inh.CustomerName = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        inh.SupplierInvoiceNo = reader.GetString(13);
                        inh.SupplierInvoiceDate = reader.GetDateTime(14);
                        inh.CurrencyID = reader.GetString(15);
                        //inh.TaxCode = reader.GetString(16);
                        inh.FreightCharge = reader.GetDouble(16);
                        inh.ProductValue = reader.GetDouble(17);
                        inh.ProductTax = reader.GetDouble(18);
                        inh.InvoiceValue = reader.GetDouble(19);
                        inh.InvoiceValueINR = reader.GetDouble(20);
                        inh.AdvancePaymentVouchers = reader.GetString(21);
                        inh.Remarks = reader.GetString(22);
                        if (!reader.IsDBNull(23))
                        {
                            inh.CommentStatus = reader.GetString(23);
                        }
                        else
                        {
                            inh.CommentStatus = "";
                        }
                        inh.status = reader.GetInt32(24);
                        inh.DocumentStatus = reader.GetInt32(25);
                        inh.CreateUser = reader.GetString(26);
                        inh.ForwardUser = reader.GetString(27);

                        inh.ApproveUser = reader.GetString(28);
                        inh.CreatorName = reader.GetString(29);
                        inh.CreateTime = reader.GetDateTime(30);
                        inh.ForwarderName = reader.GetString(31);
                        inh.ApproverName = reader.GetString(32);
                        if (!reader.IsDBNull(33))
                        {
                            inh.ForwarderList = reader.GetString(33);
                        }
                        else
                        {
                            inh.ForwarderList = "";
                        }
                        inh.ExchangeRate = reader.GetDecimal(34);
                        inh.ProductValueINR = reader.GetDouble(35);
                        inh.ProductTaxINR = reader.GetDouble(36);
                        inh.PJVTNo = reader.IsDBNull(37) ? 0 : reader.GetInt32(37);
                        inh.PJVTDate = reader.IsDBNull(38) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(38);
                        inh.PJVNo = reader.IsDBNull(39) ? 0 : reader.GetInt32(39);
                        inh.PJVDate = reader.IsDBNull(40) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(40);
                        InvoiceInHeaderList.Add(inh);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice Header Details");
            }
            return InvoiceInHeaderList;
        }


        //CHeck InvoicInPayment prepared against invoice in for unlocking
        public static Boolean isInvoiceInPaymentPreparedForInvIN(invoiceinheader iih)
        {
            Boolean isAvail = false;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select count(*) from InvoiceInPayments where InvoiceDocumentID = '" + iih.DocumentID + "'" +
                        " and InvoiceInTemporaryNo=" + iih.TemporaryNo +
                        " and InvoiceInTemporaryDate='" + iih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader.GetInt32(0) > 0)
                    {
                        isAvail = true;
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return isAvail;
        }
        public static Boolean IsSupplnvoiceNoFoundInPrevInvoiceIn(invoiceinheader iih)
        {
            Boolean stat = false;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "";
                string query1 = "select count(*) from InvoiceInHeader " +
                        " where SupplierInvoiceNo='" + iih.SupplierInvoiceNo +
                        "' and concat(TemporaryNo,TemporaryDate,DocumentID) <> concat(" + iih.TemporaryNo +
                        ",'" + iih.TemporaryDate.ToString("yyyy-MM-dd") + "','" + iih.DocumentID + "')";
                string query2 = "select count(*) from InvoiceInHeader " +
                        " where SupplierInvoiceNo='" + iih.SupplierInvoiceNo + "'";
                if (iih.TemporaryNo == 0) // New Invoice (Save)
                    query = query2;
                else
                    query = query1; // old invoice(update)
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader.GetInt32(0) > 0)
                    {
                        return true;
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }

            return stat;
        }
        public static ListView getINvoiceInDetailLVForDebitNote(string CustCode)
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
                lv.Columns.Add("Sel", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Doc ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Inv NO", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Inv Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Product Value", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Tax Amount", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Invoice Value", -2, HorizontalAlignment.Left);

                InvoiceInHeaderDB inhDb = new InvoiceInHeaderDB();

                List<invoiceinheader> IHList = inhDb.getInvoiceInListForDebitNote(CustCode);
                foreach (invoiceinheader iih in IHList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = false;
                    item.SubItems.Add(iih.DocumentID.ToString());
                    item.SubItems.Add(iih.DocumentNo.ToString());
                    item.SubItems.Add(iih.DocumentDate.ToString("yyyy-MM-dd"));
                    item.SubItems.Add(iih.PONos);   //PO No
                    item.SubItems.Add(iih.PODates);  //PO Date
                    item.SubItems.Add(iih.ProductValueINR.ToString());
                    item.SubItems.Add(iih.ProductTaxINR.ToString());
                    item.SubItems.Add(iih.InvoiceValueINR.ToString());
                    lv.Items.Add(item);
                }
            }
            catch (Exception ex)
            {

            }
            return lv;
        }

        public List<invoiceinheader> getInvoiceInListForDebitNote(string CustCode)
        {
            invoiceinheader inh;
            List<invoiceinheader> InvoiceInHeaderList = new List<invoiceinheader>();
            try
            {
                string query = "select DocumentID,DocumentNo,DocumentDate,MRNNo,MRNDate,PONos,PODates,ProductValueINR,ProductTaxINR,InvoiceValueINR" +
                    " from ViewInvoiceInHeader " +
                    " where  DocumentStatus = 99  and Status = 1 and CustomerID = '" + CustCode + "'" +
                    " order by DocumentDate asc";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        inh = new invoiceinheader();
                        inh.DocumentID = reader.GetString(0);
                        inh.DocumentNo = reader.GetInt32(1);
                        inh.DocumentDate = reader.GetDateTime(2);
                        inh.MRNNo = reader.GetInt32(3);
                        inh.MRNDate = reader.GetDateTime(4);
                        if (inh.DocumentID == "POINVOICEIN")
                        {
                            inh.PONos = reader.GetString(5);
                            inh.PODates = reader.GetString(6);
                        }
                        else
                        {
                            inh.PONos = inh.MRNNo.ToString();
                            inh.PODates = inh.MRNDate.ToString("dd-MM-yyyy");
                        }
                        inh.ProductValueINR = reader.GetDouble(7);
                        inh.ProductTaxINR = reader.GetDouble(8);
                        inh.InvoiceValueINR = reader.GetDouble(9);

                        InvoiceInHeaderList.Add(inh);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Invoice Header Details");
            }
            return InvoiceInHeaderList;
        }
    }
}
