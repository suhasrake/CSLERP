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
    class CreditNoteHeader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int CreditNoteNo { get; set; }
        public DateTime CreditNoteDate { get; set; }
        public string AccountCredit { get; set; }
        public string AccountCreditName { get; set; }
        public string SLType { get; set; }
        public string SLCode { get; set; }
        public string SLName { get; set; }
        public string ReferenceDocID { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime ReferenceDate { get; set; }
        public decimal AmountCredit { get; set; }
        public string Narration { get; set; }
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
        public CreditNoteHeader()
        {
            //Reference = "";
            Comments = "";
        }
    }
    class CreditNoteDetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; } 
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string AccountDebit { get; set; }
        public string AccountDebitName { get; set; }
        public decimal AmountDebit { get; set; }
    }
    class CreditNoteDB
    {
        public List<CreditNoteHeader> getFilteredCreditNoteHeader(string userList, int opt, string userCommentStatusString)
        {
            CreditNoteHeader cnh;
            List<CreditNoteHeader> CNHeaders = new List<CreditNoteHeader>();
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
                string query1 = "select distinct DocumentID,TemporaryNo,TemporaryDate,CreditNoteNo,CreditNoteDate," +
                    " AccountCredit,AccountCreditName, AmountCredit,SLCode,SLName, RefNo, RefDate,Narration,CreateUser,ForwardUser,"+
                    "ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,status,DocumentStatus,CommentStatus,SLType,RefDocumentID " +
                    " from ViewCreditNote" +
                    " where ((ForwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (CreateUser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (CommentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98))";

                string query2 = "select distinct DocumentID,TemporaryNo,TemporaryDate,CreditNoteNo,CreditNoteDate," +
                    " AccountCredit,AccountCreditName, AmountCredit,SLCode,SLName, RefNo, RefDate,Narration,CreateUser,ForwardUser," +
                    "ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,status,DocumentStatus,CommentStatus,SLType,RefDocumentID " +
                    " from ViewCreditNote" +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '"+Login.userLoggedIn+"')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98))" ;

                string query3 = "select distinct DocumentID,TemporaryNo,TemporaryDate,CreditNoteNo,CreditNoteDate," +
                    " AccountCredit,AccountCreditName, AmountCredit,SLCode,SLName, RefNo, RefDate,Narration,CreateUser,ForwardUser," +
                    "ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,status,DocumentStatus,CommentStatus,SLType,RefDocumentID " +
                    " from ViewCreditNote" +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'"+
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99)   order by CreditNoteDate desc,DocumentID asc,CreditNoteNo desc";

                string query6 = "select distinct DocumentID,TemporaryNo,TemporaryDate,CreditNoteNo,CreditNoteDate," +
                    " AccountCredit,AccountCreditName, AmountCredit,SLCode,SLName, RefNo, RefDate,Narration,CreateUser,ForwardUser," +
                    "ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,status,DocumentStatus,CommentStatus,SLType,RefDocumentID " +
                    " from ViewCreditNote" +
                    " where  DocumentStatus = 99  order by CreditNoteDate desc,DocumentID asc,CreditNoteNo desc";

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
                        cnh = new CreditNoteHeader();
                        cnh.DocumentID = reader.GetString(0);
                        cnh.TemporaryNo = reader.GetInt32(1);
                        cnh.TemporaryDate = reader.GetDateTime(2);
                        cnh.CreditNoteNo = reader.GetInt32(3);
                        cnh.CreditNoteDate = reader.GetDateTime(4);
                        cnh.AccountCredit = reader.GetString(5);
                        cnh.AccountCreditName = reader.GetString(6);
                        cnh.AmountCredit = reader.GetDecimal(7);
                        cnh.SLCode = reader.GetString(8);
                        cnh.SLName = reader.GetString(9);
                        cnh.ReferenceNo = reader.GetString(10);
                        cnh.ReferenceDate = reader.GetDateTime(11);
                        cnh.Narration = reader.GetString(12);
                        cnh.CreateUser = reader.GetString(13);
                        cnh.ForwardUser = reader.GetString(14);
                        cnh.ApproveUser = reader.GetString(15);
                        cnh.CreatorName = reader.GetString(16);
                        cnh.CreateTime = reader.GetDateTime(17);
                        cnh.ForwarderName = reader.GetString(18);
                        cnh.ApproverName = reader.GetString(19);
                        cnh.ForwarderList = reader.IsDBNull(20) ? "" : reader.GetString(20);
                        cnh.status = reader.GetInt32(21);
                        cnh.DocumentStatus = reader.GetInt32(22);
                        cnh.CommentStatus = reader.IsDBNull(23)?"":reader.GetString(23);
                        cnh.SLType = reader.IsDBNull(24) ? "" : reader.GetString(24);
                        cnh.ReferenceDocID = reader.IsDBNull(25) ? "" : reader.GetString(25);
                        CNHeaders.Add(cnh);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return CNHeaders;
        }
        public static List<CreditNoteDetail> getCreditNoteDetail(CreditNoteHeader cnh)
        {
            CreditNoteDetail cnd;
            List<CreditNoteDetail> CNDetail = new List<CreditNoteDetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                 query = "select RowID,DocumentID,TemporaryNo, TemporaryDate,AccountDebit,AccountDebitName,AmountDebit " +
                   " from ViewCreditNote " +
                   "where DocumentID='" + cnh.DocumentID + "'" +
                   " and TemporaryNo=" + cnh.TemporaryNo +
                   " and TemporaryDate='" + cnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cnd = new CreditNoteDetail();
                    cnd.RowID = reader.GetInt32(0);
                    cnd.DocumentID = reader.GetString(1);
                    cnd.TemporaryNo = reader.GetInt32(2);
                    cnd.TemporaryDate = reader.GetDateTime(3).Date;
                    cnd.AccountDebit = reader.GetString(4);
                    cnd.AccountDebitName = reader.GetString(5);
                    cnd.AmountDebit = reader.GetDecimal(6);
                    CNDetail.Add(cnd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return CNDetail;
        }
        public Boolean validateCreditNoteHeader(CreditNoteHeader cnh)
        {
            Boolean status = true;
            try
            {
                if (cnh.DocumentID.Trim().Length == 0 || cnh.DocumentID == null)
                {
                    return false;
                }
                if (cnh.AccountCredit.Trim().Length == 0 || cnh.AccountCredit == null)
                {
                    return false;
                }
                if (cnh.SLCode.Trim().Length == 0 || cnh.SLCode == null)
                {
                    return false;
                }
                if (cnh.AmountCredit == 0)
                {
                    return false;
                }
                if (cnh.Narration.Trim().Length == 0 || cnh.Narration == null)
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
        public Boolean forwardCreditNoteHeader(CreditNoteHeader cnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update CreditNoteHeader set DocumentStatus=" + (cnh.DocumentStatus + 1) +
                    ", forwardUser='" + cnh.ForwardUser + "'" +
                    ", commentStatus='" + cnh.CommentStatus + "'" +
                    ", ForwarderList='" + cnh.ForwarderList + "'" +
                    " where DocumentID='" + cnh.DocumentID + "'" +
                    " and TemporaryNo=" + cnh.TemporaryNo +
                    " and TemporaryDate='" + cnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "CreditNoteHeader", "", updateSQL) +
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

        public Boolean reverseCreditNoteHeader(CreditNoteHeader cnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update CreditNoteHeader set DocumentStatus=" + cnh.DocumentStatus+
                    ", forwardUser='" + cnh.ForwardUser + "'" +
                    ", commentStatus='" + cnh.CommentStatus + "'" +
                    ", ForwarderList='" + cnh.ForwarderList + "'" +
                    " where DocumentID='" + cnh.DocumentID + "'" +
                    " and TemporaryNo=" + cnh.TemporaryNo +
                    " and TemporaryDate='" + cnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "CreditNoteHeader", "", updateSQL) +
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

        public Boolean ApproveCreditNoteHeader(CreditNoteHeader cnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update CreditNoteHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + cnh.CommentStatus + "'" +
                    ", CreditNoteNo =" + cnh.CreditNoteNo +
                    ", CreditNoteDate=convert(date, getdate())" +
                    " where DocumentID='" + cnh.DocumentID + "'" +
                    " and TemporaryNo=" + cnh.TemporaryNo +
                    " and TemporaryDate='" + cnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "CreditNoteHeader", "", updateSQL) +
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
        public static string getUserComments(string docid, int tempno, DateTime tempdate)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from CreditNoteHeader where DocumentID='" + docid + "'" +
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
            }
            return cmtString;
        }
        public Boolean updateCreditHeaderAndDetail(CreditNoteHeader cnh, CreditNoteHeader prevcnh, List<CreditNoteDetail> CNDetails)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update CreditNoteHeader set AccountCredit='" + cnh.AccountCredit +
                    "', SLCode='" + cnh.SLCode +
                     "', SLType='" + cnh.SLType +
                      "', RefDocumentID='" + cnh.ReferenceDocID +
                     "', RefNo='" + cnh.ReferenceNo +
                   "', RefDate='" + cnh.ReferenceDate.ToString("yyyy-MM-dd") +
                   "', AmountCredit=" + cnh.AmountCredit +
                    ", Narration='" + cnh.Narration +
                   "', Comments='" + cnh.Comments +
                    "', CommentStatus='" + cnh.CommentStatus +
                   "', ForwarderList='" + cnh.ForwarderList + "'" +
                   " where DocumentID='" + prevcnh.DocumentID + "'" +
                   " and TemporaryNo=" + prevcnh.TemporaryNo +
                   " and TemporaryDate='" + prevcnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "CreditNoteHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from CreditNoteDetail where DocumentID='" + cnh.DocumentID + "'" +
                    " and TemporaryNo=" + cnh.TemporaryNo +
                    " and TemporaryDate='" + cnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "CreditNoteDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (CreditNoteDetail cnd in CNDetails)
                {
                    updateSQL = "insert into CreditNoteDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,AccountDebit,AmountDebit) " +
                    "values ('" + cnd.DocumentID + "'," +
                    cnd.TemporaryNo + "," +
                    "'" + cnd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + cnd.AccountDebit + "'," +
                   cnd.AmountDebit + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "CreditNoteDetail", "", updateSQL) +
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
        public Boolean InsertCreditHeaderAndDetail(CreditNoteHeader cnh, List<CreditNoteDetail> CNDetails)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                cnh.TemporaryNo = DocumentNumberDB.getNumber(cnh.DocumentID, 1);
                if (cnh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + cnh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + cnh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into CreditNoteHeader " +
                    "(DocumentID,TemporaryNo,TemporaryDate,CreditNoteNo,CreditNoteDate,AccountCredit,AmountCredit,SLCode,SLType,RefDocumentID, RefNo, RefDate,Narration," +
                    "Comments,CommentStatus,CreateUser,CreateTime,ForwarderList,DocumentStatus,Status)" +
                    " values (" +
                    "'" + cnh.DocumentID + "'," +
                    cnh.TemporaryNo + "," +
                    "'" + cnh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     cnh.CreditNoteNo + "," +
                    "'" + cnh.CreditNoteDate.ToString("yyyy-MM-dd") + "'," +
                      "'" + cnh.AccountCredit + "'," +
                      cnh.AmountCredit + "," +
                       "'" + cnh.SLCode + "'," +
                        "'" + cnh.SLType + "'," +
                         "'" + cnh.ReferenceDocID + "'," +
                     "'" + cnh.ReferenceNo + "'," +
                       "'" + cnh.ReferenceDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + cnh.Narration + "'," +
                     "'" + cnh.Comments + "'," +
                       "'" + cnh.CommentStatus + "'," +
                    "'" + Login.userLoggedIn + "'," +
                    "GETDATE()" + "," +
                    "'" + cnh.ForwarderList + "'," +
                    cnh.DocumentStatus + "," +
                         cnh.status + ")";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "CreditNoteHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from CreditNoteDetail where DocumentID='" + cnh.DocumentID + "'" +
                     " and TemporaryNo=" + cnh.TemporaryNo +
                     " and TemporaryDate='" + cnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "CreditNoteDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (CreditNoteDetail cnd in CNDetails)
                {
                    updateSQL = "insert into CreditNoteDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,AccountDebit,AmountDebit) " +
                    "values ('" + cnd.DocumentID + "'," +
                    cnh.TemporaryNo + "," +
                    "'" + cnd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + cnd.AccountDebit + "'," +
                   cnd.AmountDebit + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "CreditNoteDetail", "", updateSQL) +
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
    }
}
