using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class indentgeneralheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime TargetDate { get; set; }
        public string PurchaseSource { get; set; }
        public string CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public decimal ExchangeRate { get; set; }
        public double ProductValue { get; set; }
        public double ProductValueINR { get; set; }

        public int Status { get; set; }
        public int DocumentStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string CreatorName { get; set; }
        public string ForwarderName { get; set; }
        public string ApproverName { get; set; }
        public string CommentStatus { get; set; }
        public string Comments { get; set; }
        public string ForwarderList { get; set; }
        public int pono { get; set; }

    }
    class indentgeneraldetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string ItemDetail { get; set; }
        public string Approver { get; set; }
        public double Quantity { get; set; }
        public double ExpectedPurchasePrice { get; set; }
        public int WarrantyDays { get; set; }
    }
    class IndentGeneralDB
    {
        public List<indentgeneralheader> getFilteredIndnetGeneralHeaders(string userList, int opt, string userCommentStatusString)
        {
            indentgeneralheader igh;
            List<indentgeneralheader> IGHeaders = new List<indentgeneralheader>();
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
                string query1 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " DocumentNo,DocumentDate,ReferenceNo,CurrencyID,CurrencyName,ExchangeRate,TargetDate,PurchaseSource," +
                    " Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,CommentStatus,ForwarderList,ProductValue,ProductValueINR " +
                    " from ViewIndentGeneralHeader" +
                   " where ((forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (commentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) and Status not in (7,98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query2 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " DocumentNo,DocumentDate,ReferenceNo,CurrencyID,CurrencyName,ExchangeRate,TargetDate,PurchaseSource," +
                    " Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,CommentStatus,ForwarderList,ProductValue,ProductValueINR " +
                    " from ViewIndentGeneralHeader" +
                   " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) and Status not in (7,98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";
                //for those who have add/edit/delete permission
                string query3 = "select a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate, " +
                                " a.DocumentNo,a.DocumentDate,a.ReferenceNo,a.CurrencyID,a.CurrencyName,a.ExchangeRate,a.TargetDate,a.PurchaseSource, " +
                                " a.Status,a.DocumentStatus,a.CreateTime,a.CreateUser,a.ForwardUser,a.ApproveUser,a.CreatorName,a.ForwarderName,"+
                                "a.ApproverName,a.CommentStatus,a.ForwarderList,a.ProductValue,a.ProductValueINR, COUNT(b.PONo)po " +
                                " from ViewIndentGeneralHeader a left outer join POHeader b " +
                                " on ReferenceIndent like CONCAT('%', a.DocumentID, '(', a.DocumentNo, CHAR(222), CONVERT(varchar, a.DocumentDate), ');%') " +
                                " and b.Status = 1 and b.DocumentStatus = 99 " +
                                " where ((a.createuser='" + Login.userLoggedIn + "'" +
                                " or a.ForwarderList like '%" + userList + "%'" +
                                " or a.commentStatus like '%" + acStr + "%'" +
                                " or a.approveUser='" + Login.userLoggedIn + "')" +
                                " and a.DocumentStatus = 99) and a.status = 1 " +
                                " group by a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate, " +
                                " a.DocumentNo,a.DocumentDate,a.ReferenceNo,a.CurrencyID,a.CurrencyName,a.ExchangeRate,a.TargetDate,a.PurchaseSource, " +
                                " a.Status,a.DocumentStatus,a.CreateTime,a.CreateUser,a.ForwardUser,a.ApproveUser,a.CreatorName,a.ForwarderName,"+
                                "a.ApproverName,a.CommentStatus,a.ForwarderList,a.ProductValue,a.ProductValueINR order by a.DocumentDate desc,a.DocumentID asc,a.DocumentNo desc";
                string query6 = "select a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate, " +
                                " a.DocumentNo,a.DocumentDate,a.ReferenceNo,a.CurrencyID,a.CurrencyName,a.ExchangeRate,a.TargetDate,a.PurchaseSource, " +
                                " a.Status,a.DocumentStatus,a.CreateTime,a.CreateUser,a.ForwardUser,a.ApproveUser,a.CreatorName,a.ForwarderName,"+
                                "a.ApproverName,a.CommentStatus,a.ForwarderList,a.ProductValue,a.ProductValueINR, COUNT(b.PONo)po " +
                                " from ViewIndentGeneralHeader a left outer join POHeader b " +
                                " on ReferenceIndent like CONCAT('%', a.DocumentID, '(', a.DocumentNo, CHAR(222), CONVERT(varchar, a.DocumentDate), ');%') " +
                                " and b.Status = 1 and b.DocumentStatus = 99 " +
                                " where a.DocumentStatus = 99 and a.status = 1" +
                                " group by a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate, " +
                                " a.DocumentNo,a.DocumentDate,a.ReferenceNo,a.CurrencyID,a.CurrencyName,a.ExchangeRate,a.TargetDate,a.PurchaseSource, " +
                                " a.Status,a.DocumentStatus,a.CreateTime,a.CreateUser,a.ForwardUser,a.ApproveUser,a.CreatorName,a.ForwarderName,"+
                                "a.ApproverName,a.CommentStatus,a.ForwarderList,a.ProductValue,a.ProductValueINR order by a.DocumentDate desc,a.DocumentID asc,a.DocumentNo desc";
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
                    igh = new indentgeneralheader();
                    igh.RowID = reader.GetInt32(0);
                    igh.DocumentID = reader.GetString(1);
                    igh.DocumentName = reader.GetString(2);
                    igh.TemporaryNo = reader.GetInt32(3);
                    igh.TemporaryDate = reader.GetDateTime(4);
                    igh.DocumentNo = reader.GetInt32(5);
                    igh.DocumentDate = reader.IsDBNull(6) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(6);
                    igh.ReferenceNo = reader.GetString(7);
                    igh.CurrencyID = reader.GetString(8);
                    igh.CurrencyName = reader.GetString(9);
                    igh.ExchangeRate = reader.GetDecimal(10);
                    igh.TargetDate = reader.GetDateTime(11);
                    igh.PurchaseSource = reader.GetString(12);

                    igh.Status = reader.GetInt32(13);
                    igh.DocumentStatus = reader.GetInt32(14);
                    igh.CreateTime = reader.GetDateTime(15);
                    igh.CreateUser = reader.GetString(16);
                    igh.ForwardUser = reader.IsDBNull(17) ? "" : reader.GetString(17);
                    igh.ApproveUser = reader.IsDBNull(18) ? "" : reader.GetString(18);
                    igh.CreatorName = reader.GetString(19);
                    igh.ForwarderName = reader.IsDBNull(20) ? "" : reader.GetString(20);
                    igh.ApproverName = reader.IsDBNull(21) ? "" : reader.GetString(21);
                    if (!reader.IsDBNull(22))
                    {
                        igh.CommentStatus = reader.GetString(22);
                    }
                    else
                    {
                        igh.CommentStatus = "";
                    }
                    if (!reader.IsDBNull(23))
                    {
                        igh.ForwarderList = reader.GetString(23);
                    }
                    else
                    {
                        igh.ForwarderList = "";
                    }
                    igh.ProductValue = reader.IsDBNull(24) ? 0 : reader.GetDouble(24);
                    igh.ProductValueINR = reader.IsDBNull(25) ? 0 : reader.GetDouble(25);
                    if(opt==3 || opt == 6)
                    {
                        igh.pono= reader.IsDBNull(26) ? 0 : reader.GetInt32(26);
                    }
                    IGHeaders.Add(igh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Indent General Header Details");
            }
            return IGHeaders;
        }

        public static List<indentgeneraldetail> getIndentGeneralDetails(indentgeneralheader igh)
        {
            indentgeneraldetail igd;
            List<indentgeneraldetail> IGDetail = new List<indentgeneraldetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,TemporaryNo, TemporaryDate,ItemDetail," +
                   "Quantity,ExpectedPurchasePrice,WarrantyDays " +
                   "from IndentGeneralDetail" +
                   " where DocumentID='" + igh.DocumentID + "'" +
                   " and TemporaryNo=" + igh.TemporaryNo +
                   " and TemporaryDate='" + igh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    igd = new indentgeneraldetail();
                    igd.RowID = reader.GetInt32(0);
                    igd.DocumentID = reader.GetString(1);
                    igd.TemporaryNo = reader.GetInt32(2);
                    igd.TemporaryDate = reader.GetDateTime(3).Date;
                    igd.ItemDetail = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    igd.Quantity = reader.GetDouble(5);
                    igd.ExpectedPurchasePrice = reader.GetDouble(6);
                    igd.WarrantyDays = reader.GetInt32(7);
                    IGDetail.Add(igd);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Inidentgeneral Details");
            }
            return IGDetail;
        }
        public Boolean validateIndentGeneralHeader(indentgeneralheader igh)
        {
            Boolean status = true;
            try
            {
                if (igh.DocumentID.Trim().Length == 0 || igh.DocumentID == null)
                {
                    return false;
                }
                //if (igh.PurchaseSource == null || igh.PurchaseSource.Trim().Length == 0 )
                //{
                //    return false;
                //}
                if (igh.CurrencyID.Trim().Length == 0 || igh.CurrencyID == null)
                {
                    return false;
                }
                if (igh.ReferenceNo == null || igh.ReferenceNo.Trim().Length == 0)
                {
                    return false;
                }
                if (igh.TargetDate == null || igh.TargetDate < DateTime.Now.Date)
                {
                    return false;
                }
                if (igh.ExchangeRate == 0)
                {
                    return false;
                }
                if (igh.ProductValue == 0)
                {
                    return false;
                }
                if (igh.ProductValueINR == 0)
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
        public Boolean forwardIndentGeneral(indentgeneralheader igh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update IndentGeneralHeader set DocumentStatus=" + (igh.DocumentStatus + 1) +
                     ", forwardUser='" + igh.ForwardUser + "'" +
                    ", commentStatus='" + igh.CommentStatus + "'" +
                    ", ForwarderList='" + igh.ForwarderList + "'" +
                    " where DocumentID='" + igh.DocumentID + "'" +
                    " and TemporaryNo=" + igh.TemporaryNo +
                    " and TemporaryDate='" + igh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "IndentGeneralHeader", "", updateSQL) +
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
        public Boolean reverseIndentGeneral(indentgeneralheader igh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update IndentGeneralHeader set DocumentStatus=" + igh.DocumentStatus +
                    ", forwardUser='" + igh.ForwardUser + "'" +
                    ", commentStatus='" + igh.CommentStatus + "'" +
                    ", ForwarderList='" + igh.ForwarderList + "'" +
                    " where DocumentID='" + igh.DocumentID + "'" +
                    " and TemporaryNo=" + igh.TemporaryNo +
                    " and TemporaryDate='" + igh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "IndentGeneralHeader", "", updateSQL) +
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
        public Boolean ApproveIndentGeneral(indentgeneralheader igh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update IndentGeneralHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + igh.CommentStatus + "'" +
                    ", DocumentNo=" + igh.DocumentNo +
                    ", DocumentDate=convert(date, getdate())" +
                    " where DocumentID='" + igh.DocumentID + "'" +
                    " and TemporaryNo=" + igh.TemporaryNo +
                    " and TemporaryDate='" + igh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "IndentGeneralHeader", "", updateSQL) +
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
                string query = "select comments from IndentGeneralHeader where DocumentID='" + docid + "'" +
                        " and TemporaryNo=" + tempno +
                        " and TemporaryDate='" + tempdate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    cmtString = reader.GetString(0);
                }
                conn.Open();
            }
            catch (Exception ex)
            {
            }
            return cmtString;
        }

        public Boolean updateIndentGeneralHeaderAndDetail(indentgeneralheader igh, indentgeneralheader previgh, List<indentgeneraldetail> IGDetail)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update IndentGeneralHeader set TemporaryNo = " + igh.TemporaryNo +
                   ", TemporaryDate='" + igh.TemporaryDate.ToString("yyyy-MM-dd") +
                   //"', DocumentNo=" + igh.DocumentNo +
                   //", DocumentDate='" + igh.DocumentDate.ToString("yyyy-MM-dd") +
                   "', ReferenceNo='" + igh.ReferenceNo +
                   "', TargetDate='" + igh.TargetDate.ToString("yyyy-MM-dd") +
                   "', CurrencyID='" + igh.CurrencyID +
                   "', Comments='" + igh.Comments +
                    "', CommentStatus='" + igh.CommentStatus +
                   "', ExchangeRate=" + igh.ExchangeRate +
                     ", ProductValue=" + igh.ProductValue +
                       ", ProductValueINR=" + igh.ProductValueINR +
                   ", PurchaseSource='" + igh.PurchaseSource + "'" +
                  " where DocumentID='" + previgh.DocumentID + "'" +
                  " and TemporaryNo=" + previgh.TemporaryNo +
                  " and TemporaryDate='" + previgh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "IndentGeneralHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from IndentGeneralDetail where DocumentID='" + previgh.DocumentID + "'" +
                     " and TemporaryNo=" + previgh.TemporaryNo +
                     " and TemporaryDate='" + previgh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "IndentGeneralDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (indentgeneraldetail igd in IGDetail)
                {
                    updateSQL = "insert into IndentGeneralDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,ItemDetail,Quantity,ExpectedPurchasePrice,WarrantyDays) " +
                    "values ('" + igd.DocumentID + "'," +
                    igd.TemporaryNo + "," +
                    "'" + igd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       "'" + igd.ItemDetail + "'," +
                    igd.Quantity + "," +
                    igd.ExpectedPurchasePrice + " ," +
                    igd.WarrantyDays + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "IndentGeneralDetail", "", updateSQL) +
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
        public Boolean InsertIndentGeneralHeaderAndDetail(indentgeneralheader igh, List<indentgeneraldetail> IGDetail)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                igh.TemporaryNo = DocumentNumberDB.getNumber(igh.DocumentID, 1);
                if (igh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + igh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + igh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into IndentGeneralHeader " +
                    "(DocumentID,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate,ReferenceNo,CurrencyID," +
                    "ExchangeRate,ProductValue,ProductValueINR,TargetDate,PurchaseSource," +
                    "Status,DocumentStatus,CreateTime,CreateUser, CommentStatus,Comments,ForwarderList)" +
                    " values (" +
                    "'" + igh.DocumentID + "'," +
                    igh.TemporaryNo + "," +
                    "'" + igh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    igh.DocumentNo + "," +
                    "'" + igh.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + igh.ReferenceNo + "'," +
                    "'" + igh.CurrencyID + "'," +
                    igh.ExchangeRate + "," +
                      igh.ProductValue + "," +
                        igh.ProductValueINR + "," +
                    "'" + igh.TargetDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + igh.PurchaseSource + "'," +
                    igh.Status + "," +
                    igh.DocumentStatus + "," +
                     "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'," +
                    "'" + igh.CommentStatus + "'," +
                    "'" + igh.Comments + "'," +
                    "'" + igh.ForwarderList + "')";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "IndentGeneralHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from IndentGeneralDetail where DocumentID='" + igh.DocumentID + "'" +
                    " and TemporaryNo=" + igh.TemporaryNo +
                    " and TemporaryDate='" + igh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "IndentGeneralDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (indentgeneraldetail igd in IGDetail)
                {
                    updateSQL = "insert into IndentGeneralDetail " +
                  "(DocumentID,TemporaryNo,TemporaryDate,ItemDetail,Quantity,ExpectedPurchasePrice,WarrantyDays) " +
                  "values ('" + igd.DocumentID + "'," +
                  igh.TemporaryNo + "," +
                  "'" + igd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + igd.ItemDetail + "'," +
                  igd.Quantity + "," +
                  igd.ExpectedPurchasePrice + " ," +
                  igd.WarrantyDays + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "IndentGeneralDetail", "", updateSQL) +
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

        public static List<indentgeneraldetail> getIndentGeneralDetailsForPO(string docid, int docNo, DateTime docDate)
        {
            indentgeneraldetail igd;
            List<indentgeneraldetail> IGDetail = new List<indentgeneraldetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select a.RowID,a.DocumentID,d.DocumentName,a.TemporaryNo, a.TemporaryDate,a.ItemDetail,a.Quantity,a.ExpectedPurchasePrice,a.WarrantyDays,c.Name as Approver" +
                   " from IndentGeneralDetail a, IndentGeneralHeader b, ViewUserEmployeeList c, Document d where a.DocumentID = b.DocumentID and" +
                   "  a.DocumentID = d.DocumentID and a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate and b.ApproveUser = c.UserID and " +
                   " b.DocumentID='" + docid + "'" +
                   " and b.DocumentNo= " + docNo +
                   " and b.DocumentDate= '" + docDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    igd = new indentgeneraldetail();
                    igd.RowID = reader.GetInt32(0);
                    igd.DocumentID = reader.GetString(1);
                    igd.DocumentName = reader.GetString(2);
                    igd.TemporaryNo = reader.GetInt32(3);
                    igd.TemporaryDate = reader.GetDateTime(4).Date;
                    igd.ItemDetail = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    igd.Quantity = reader.GetDouble(6);
                    igd.ExpectedPurchasePrice = reader.GetDouble(7);
                    igd.WarrantyDays = reader.GetInt32(8);
                    igd.Approver = reader.IsDBNull(9) ? "" : reader.GetString(9);
                    IGDetail.Add(igd);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Inidentgeneral Details");
            }
            return IGDetail;
        }
        ////new

        public static List<poheader> getPurchaseOrderHeader(indentgeneralheader iigh)
        {
            poheader poh;
            List<poheader> PODetail = new List<poheader>();
            try
            {
                string query = "";
                string refindent = iigh.DocumentID + "(" + iigh.DocumentNo + "" + Main.delimiter1 + "" + iigh.DocumentDate.ToString("yyyy-MM-dd") + ");";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                   " PONo,PODate,ReferenceIndent,ReferenceQuotation,CustomerID,CustomerName,CurrencyID,DeliveryPeriod,ValidityPeriod,TaxTerms,ModeOfPayment,PaymentTerms," +
                   " FreightTerms,FreightCharge,DeliveryAddress,ProductValue,TaxAmount,POValue,Remarks, " +
                   " TermsAndCondition,Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName ,CommentStatus,ForwarderList" +
                   " ,ExchangeRate, ProductValueINR, POValueINR, TaxAmountINR,TransportationMode,SpecialNote,PartialShipment,Transhipment,PackingSpec,PriceBasis,DeliveryAt,CountryID " +
                       "from ViewPOHeader where Status=1 and DocumentStatus=99 and  ReferenceIndent like '%" + refindent + "%'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    poh = new poheader();
                    poh.RowID = reader.GetInt32(0);
                    poh.DocumentID = reader.GetString(1);
                    poh.DocumentName = reader.GetString(2);
                    poh.TemporaryNo = reader.GetInt32(3);
                    poh.TemporaryDate = reader.GetDateTime(4);
                    poh.PONo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        poh.PODate = reader.GetDateTime(6);
                    }
                    poh.ReferenceIndent = reader.GetString(7);
                    poh.ReferenceQuotation = reader.GetString(8);
                    poh.CustomerID = reader.GetString(9);
                    poh.CustomerName = reader.GetString(10);
                    poh.CurrencyID = reader.GetString(11);
                    poh.DeliveryPeriod = reader.GetInt32(12);
                    poh.validityPeriod = reader.GetInt32(13);
                    poh.TaxTerms = reader.GetString(14);
                    poh.ModeOfPayment = reader.GetString(15);
                    poh.PaymentTerms = reader.GetString(16);
                    //poh.CreditPeriod = reader.GetInt32(17);
                    poh.FreightTerms = reader.GetString(17);
                    poh.FreightCharge = reader.GetDouble(18);
                    poh.DeliveryAddress = reader.GetString(19);
                    poh.ProductValue = reader.GetDouble(20);
                    poh.TaxAmount = reader.GetDouble(21);
                    poh.POValue = reader.GetDouble(22);
                    poh.Remarks = reader.GetString(23);
                    poh.TermsAndCondition = reader.GetString(24);
                    poh.Status = reader.GetInt32(25);
                    poh.DocumentStatus = reader.GetInt32(26);
                    poh.CreateTime = reader.GetDateTime(27);
                    poh.CreateUser = reader.GetString(28);
                    poh.ForwardUser = reader.GetString(29);
                    poh.ApproveUser = reader.GetString(30);
                    poh.CreatorName = reader.GetString(31);
                    poh.ForwarderName = reader.GetString(32);
                    poh.ApproverName = reader.GetString(33);
                    if (!reader.IsDBNull(34))
                    {
                        poh.CommentStatus = reader.GetString(34);
                    }
                    else
                    {
                        poh.CommentStatus = "";
                    }
                    if (!reader.IsDBNull(35))
                    {
                        poh.ForwarderList = reader.GetString(35);
                    }
                    else
                    {
                        poh.ForwarderList = "";
                    }
                    poh.ExchangeRate = reader.GetDecimal(36);
                    poh.ProductValueINR = reader.GetDouble(37);
                    poh.POValueINR = reader.GetDouble(38);
                    poh.TaxAmountINR = reader.GetDouble(39);
                    poh.TransportationMode = reader.IsDBNull(40) ? "" : reader.GetString(40);
                    poh.SpecialNote = reader.IsDBNull(41) ? "" : reader.GetString(41);
                    poh.PartialShipment = reader.IsDBNull(42) ? "" : reader.GetString(42);
                    poh.Transhipment = reader.IsDBNull(43) ? "" : reader.GetString(43);
                    poh.PackingSpec = reader.IsDBNull(44) ? "" : reader.GetString(44);
                    poh.PriceBasis = reader.IsDBNull(45) ? "" : reader.GetString(45);
                    poh.DeliveryAt = reader.IsDBNull(46) ? "" : reader.GetString(46);
                    poh.CountryID = reader.IsDBNull(47) ? "" : reader.GetString(47);
                    PODetail.Add(poh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Work Order Details");
            }
            return PODetail;
        }        
    }
}
