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
    class DebitNoteHeader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int DebitNoteNo { get; set; }
        public DateTime DebitNoteDate { get; set; }
        public string AccountDebit { get; set; }
        public string AccountDebitName { get; set; }
        public string SLType { get; set; }
        public string SLCode { get; set; }
        public string SLName { get; set; }
        public string ReferenceDocID { get; set; }
        public string ReferenceNo { get; set; }
        public DateTime ReferenceDate { get; set; }
        public decimal AmountDebit { get; set; }
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
        public DebitNoteHeader()
        {
            //Reference = "";
            Comments = "";
        }
    }
    class DebitNoteDetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; } 
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string AccountCredit { get; set; }
        public string AccountCreditName { get; set; }
        public decimal AmountCredit { get; set; }
    }
    class DebitNoteDB
    {
        public List<DebitNoteHeader> getFilteredDebitNoteHeader(string userList, int opt, string userCommentStatusString)
        {
            DebitNoteHeader dnh;
            List<DebitNoteHeader> dnheaders = new List<DebitNoteHeader>();
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
                string query1 = "select distinct DocumentID,TemporaryNo,TemporaryDate,DebitNoteNo,DebitNoteDate," +
                    " AccountDebit,AccountDebitName, AmountDebit,SLCode,SLName, RefNo, RefDate,Narration,CreateUser,"+
                    "ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,status,DocumentStatus,CommentStatus,SLType,RefDocumentID " +
                    " from ViewDebitNote" +
                    " where ((ForwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (CreateUser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (CommentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) order by TemporaryNo asc,TemporaryDate asc ";

                string query2 = "select distinct DocumentID,TemporaryNo,TemporaryDate,DebitNoteNo,DebitNoteDate," +
                    " AccountDebit,AccountDebitName, AmountDebit,SLCode,SLName, RefNo, RefDate,Narration,CreateUser," +
                    "ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,status,DocumentStatus,CommentStatus,SLType,RefDocumentID " +
                    " from ViewDebitNote" +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '"+Login.userLoggedIn+"')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) order by TemporaryNo asc,TemporaryDate asc ";

                string query3 = "select distinct DocumentID,TemporaryNo,TemporaryDate,DebitNoteNo,DebitNoteDate," +
                    " AccountDebit,AccountDebitName, AmountDebit,SLCode,SLName, RefNo, RefDate,Narration,CreateUser," +
                    "ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,status,DocumentStatus,CommentStatus,SLType,RefDocumentID " +
                    " from ViewDebitNote" +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'"+
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99)   order by DebitNoteDate asc,DebitNoteNo asc";

                string query6 = "select distinct DocumentID,TemporaryNo,TemporaryDate,DebitNoteNo,DebitNoteDate," +
                    " AccountDebit,AccountDebitName, AmountDebit,SLCode,SLName, RefNo, RefDate,Narration,CreateUser," +
                    "ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,status,DocumentStatus,CommentStatus,SLType,RefDocumentID " +
                    " from ViewDebitNote" +
                    " where  DocumentStatus = 99  order by DebitNoteDate asc,DebitNoteNo asc";

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
                        dnh = new DebitNoteHeader();
                        dnh.DocumentID = reader.GetString(0);
                        dnh.TemporaryNo = reader.GetInt32(1);
                        dnh.TemporaryDate = reader.GetDateTime(2);
                        dnh.DebitNoteNo = reader.GetInt32(3);
                        dnh.DebitNoteDate = reader.GetDateTime(4);
                        dnh.AccountDebit = reader.GetString(5);
                        dnh.AccountDebitName = reader.GetString(6);
                        dnh.AmountDebit = reader.GetDecimal(7);
                        dnh.SLCode = reader.GetString(8);
                        dnh.SLName = reader.GetString(9);
                        dnh.ReferenceNo = reader.GetString(10);
                        dnh.ReferenceDate = reader.GetDateTime(11);
                        dnh.Narration = reader.GetString(12);
                        dnh.CreateUser = reader.GetString(13);
                        dnh.ForwardUser = reader.GetString(14);
                        dnh.ApproveUser = reader.GetString(15);
                        dnh.CreatorName = reader.GetString(16);
                        dnh.CreateTime = reader.GetDateTime(17);
                        dnh.ForwarderName = reader.GetString(18);
                        dnh.ApproverName = reader.GetString(19);
                        dnh.ForwarderList = reader.IsDBNull(20) ? "" : reader.GetString(20);
                        dnh.status = reader.GetInt32(21);
                        dnh.DocumentStatus = reader.GetInt32(22);
                        dnh.CommentStatus = reader.IsDBNull(23)?"":reader.GetString(23);
                        dnh.SLType = reader.IsDBNull(24) ? "" : reader.GetString(24);
                        dnh.ReferenceDocID = reader.IsDBNull(25) ? "" : reader.GetString(25);
                        dnheaders.Add(dnh);
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
            return dnheaders;
        }
        public static List<DebitNoteDetail> getDebitNoteDetail(DebitNoteHeader dnh)
        {
            DebitNoteDetail dnd;
            List<DebitNoteDetail> dndetail = new List<DebitNoteDetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                 query = "select RowID,DocumentID,TemporaryNo, TemporaryDate,AccountCredit,AccountCreditName,AmountCredit " +
                   " from ViewDebitNote " +
                   "where DocumentID='" + dnh.DocumentID + "'" +
                   " and TemporaryNo=" + dnh.TemporaryNo +
                   " and TemporaryDate='" + dnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dnd = new DebitNoteDetail();
                    dnd.RowID = reader.GetInt32(0);
                    dnd.DocumentID = reader.GetString(1);
                    dnd.TemporaryNo = reader.GetInt32(2);
                    dnd.TemporaryDate = reader.GetDateTime(3).Date;
                    dnd.AccountCredit = reader.GetString(4);
                    dnd.AccountCreditName = reader.GetString(5);
                    dnd.AmountCredit = reader.GetDecimal(6);
                    dndetail.Add(dnd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return dndetail;
        }
        public Boolean validateDebitNoteHeader(DebitNoteHeader dnh)
        {
            Boolean status = true;
            try
            {
                if (dnh.DocumentID.Trim().Length == 0 || dnh.DocumentID == null)
                {
                    return false;
                }
                if (dnh.AccountDebit.Trim().Length == 0 || dnh.AccountDebit == null)
                {
                    return false;
                }
                if (dnh.SLType.Trim().Length == 0 || dnh.SLType == null)
                {
                    return false;
                }
                if (dnh.SLCode.Trim().Length == 0 || dnh.SLCode == null)
                {
                    return false;
                }
                //if (dnh.ReferenceNo.Trim().Length == 0 || dnh.ReferenceNo == null)
                //{
                //    return false;
                //}
                //if (dnh.ReferenceDate == null)
                //{
                //    return false;
                //}
                if (dnh.AmountDebit == 0)
                {
                    return false;
                }
                if (dnh.Narration.Trim().Length == 0 || dnh.Narration == null)
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
        public Boolean forwardDebitNoteHeader(DebitNoteHeader dnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update DebitNoteHeader set DocumentStatus=" + (dnh.DocumentStatus + 1) +
                    ", forwardUser='" + dnh.ForwardUser + "'" +
                    ", commentStatus='" + dnh.CommentStatus + "'" +
                    ", ForwarderList='" + dnh.ForwarderList + "'" +
                    " where DocumentID='" + dnh.DocumentID + "'" +
                    " and TemporaryNo=" + dnh.TemporaryNo +
                    " and TemporaryDate='" + dnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "DebitNoteHeader", "", updateSQL) +
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

        public Boolean reverseDebitNoteHeader(DebitNoteHeader dnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update DebitNoteHeader set DocumentStatus=" + dnh.DocumentStatus+
                    ", forwardUser='" + dnh.ForwardUser + "'" +
                    ", commentStatus='" + dnh.CommentStatus + "'" +
                    ", ForwarderList='" + dnh.ForwarderList + "'" +
                    " where DocumentID='" + dnh.DocumentID + "'" +
                    " and TemporaryNo=" + dnh.TemporaryNo +
                    " and TemporaryDate='" + dnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "DebitNoteHeader", "", updateSQL) +
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

        public Boolean ApproveDebitNoteHeader(DebitNoteHeader dnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update DebitNoteHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + dnh.CommentStatus + "'" +
                    ", DebitNoteNo =" + dnh.DebitNoteNo +
                    ", DebitNoteDate=convert(date, getdate())" +
                    " where DocumentID='" + dnh.DocumentID + "'" +
                    " and TemporaryNo=" + dnh.TemporaryNo +
                    " and TemporaryDate='" + dnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "DebitNoteHeader", "", updateSQL) +
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
                string query = "select comments from DebitNoteHeader where DocumentID='" + docid + "'" +
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
        public Boolean updateDebitHeaderAndDetail( DebitNoteHeader dnh, DebitNoteHeader prevdnh, List<DebitNoteDetail> dndetails)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update DebitNoteHeader set AccountDebit='" + dnh.AccountDebit +
                    "', SLType='" + dnh.SLType +
                    "', SLCode='" + dnh.SLCode +
                     "', RefDocumentID='" + dnh.ReferenceDocID +
                     "', RefNo='" + dnh.ReferenceNo +
                   "', RefDate='" + dnh.ReferenceDate.ToString("yyyy-MM-dd") +
                   "', AmountDebit=" + dnh.AmountDebit +
                    ", Narration='" + dnh.Narration +
                   "', Comments='" + dnh.Comments +
                    "', CommentStatus='" + dnh.CommentStatus +
                   "', ForwarderList='" + dnh.ForwarderList + "'" +
                   " where DocumentID='" + prevdnh.DocumentID + "'" +
                   " and TemporaryNo=" + prevdnh.TemporaryNo +
                   " and TemporaryDate='" + prevdnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "DebitNoteHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from DebitNoteDetail where DocumentID='" + prevdnh.DocumentID + "'" +
                    " and TemporaryNo=" + prevdnh.TemporaryNo +
                    " and TemporaryDate='" + prevdnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "DebitNoteDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (DebitNoteDetail dnd in dndetails)
                {
                    updateSQL = "insert into DebitNoteDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,AccountCredit,AmountCredit) " +
                    "values ('" + dnd.DocumentID + "'," +
                    dnd.TemporaryNo + "," +
                    "'" + dnd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + dnd.AccountCredit + "'," +
                   dnd.AmountCredit + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "DebitNoteDetail", "", updateSQL) +
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
        public Boolean InsertDebitHeaderAndDetail(DebitNoteHeader dnh, List<DebitNoteDetail> dndetails)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                dnh.TemporaryNo = DocumentNumberDB.getNumber(dnh.DocumentID, 1);
                if (dnh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + dnh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + dnh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into DebitNoteHeader " +
                    "(DocumentID,TemporaryNo,TemporaryDate,DebitNoteNo,DebitNoteDate,AccountDebit,AmountDebit,SLType,SLCode,RefDocumentID, RefNo, RefDate,Narration," +
                    "Comments,CommentStatus,CreateUser,CreateTime,ForwarderList,DocumentStatus,Status)" +
                    " values (" +
                    "'" + dnh.DocumentID + "'," +
                    dnh.TemporaryNo + "," +
                    "'" + dnh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     dnh.DebitNoteNo + "," +
                    "'" + dnh.DebitNoteDate.ToString("yyyy-MM-dd") + "'," +
                      "'" + dnh.AccountDebit + "'," +
                      dnh.AmountDebit + "," +
                        "'" + dnh.SLType + "'," +
                       "'" + dnh.SLCode + "'," +
                       "'" + dnh.ReferenceDocID + "'," +
                     "'" + dnh.ReferenceNo + "'," +
                       "'" + dnh.ReferenceDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + dnh.Narration + "'," +
                     "'" + dnh.Comments + "'," +
                       "'" + dnh.CommentStatus + "'," +
                    "'" + Login.userLoggedIn + "'," +
                    "GETDATE()" + "," +
                    "'" + dnh.ForwarderList + "'," +
                    dnh.DocumentStatus + "," +
                         dnh.status + ")";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "DebitNoteHeader", "", updateSQL) +
                Main.QueryDelimiter;
                
                updateSQL = "Delete from DebitNoteDetail where DocumentID='" + dnh.DocumentID + "'" +
                    " and TemporaryNo=" + dnh.TemporaryNo +
                    " and TemporaryDate='" + dnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "DebitNoteDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (DebitNoteDetail dnd in dndetails)
                {
                    updateSQL = "insert into DebitNoteDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,AccountCredit,AmountCredit) " +
                    "values ('" + dnd.DocumentID + "'," +
                    dnh.TemporaryNo + "," +
                    "'" + dnd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + dnd.AccountCredit + "'," +
                   dnd.AmountCredit + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "DebitNoteDetail", "", updateSQL) +
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
