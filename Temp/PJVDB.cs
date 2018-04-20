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
    class PJVHeader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int JournalNo { get; set; }
        public DateTime JournalDate { get; set; }
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
        public PJVHeader()
        {
            //Reference = "";
            Comments = "";
        }
    }
    class PJVDetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; } 
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal AmountDebit { get; set; }
        public decimal AmountCredit { get; set; }
        public string SLType { get; set; }
        public string SLCode { get; set; }
        public string SLName { get; set; }
    }
    class PJVDB
    {
        public List<PJVHeader> getFilteredPJVHeader(string userList, int opt, string userCommentStatusString)
        {
            PJVHeader jvh;
            List<PJVHeader> JVHeaders = new List<PJVHeader>();
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
                string query1 = "select distinct DocumentID,TemporaryNo,TemporaryDate,JournalNo,JournalDate," +
                    " Narration,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,status,DocumentStatus,CommentStatus " +
                    " from ViewJournalVoucher" +
                    " where ((ForwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (CreateUser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (CommentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query2 = "select distinct DocumentID,TemporaryNo,TemporaryDate,JournalNo,JournalDate," +
                    " Narration,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,status,DocumentStatus,CommentStatus " +
                    " from ViewJournalVoucher" +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '"+Login.userLoggedIn+"')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc" ;

                string query3 = "select distinct DocumentID,TemporaryNo,TemporaryDate,JournalNo,JournalDate," +
                    " Narration,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,status,DocumentStatus,CommentStatus " +
                    " from ViewJournalVoucher" +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'"+
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99)   order by JournalDate desc,DocumentID asc,JournalNo desc";

                string query6 = "select distinct DocumentID,TemporaryNo,TemporaryDate,JournalNo,JournalDate," +
                    " Narration,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,status,DocumentStatus,CommentStatus " +
                    " from ViewJournalVoucher" +
                    " where  DocumentStatus = 99  order by JournalDate desc,DocumentID asc,JournalNo desc";

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
                        jvh = new PJVHeader();
                        jvh.DocumentID = reader.GetString(0);
                        jvh.TemporaryNo = reader.GetInt32(1);
                        jvh.TemporaryDate = reader.GetDateTime(2);
                        jvh.JournalNo = reader.GetInt32(3);
                        jvh.JournalDate = reader.GetDateTime(4);
                        jvh.Narration = reader.GetString(5);
                        jvh.CreateUser = reader.GetString(6);
                        jvh.ForwardUser = reader.GetString(7);
                        jvh.ApproveUser = reader.GetString(8);
                        jvh.CreatorName = reader.GetString(9);
                        jvh.CreateTime = reader.GetDateTime(10);
                        jvh.ForwarderName = reader.GetString(11);
                        jvh.ApproverName = reader.GetString(12);

                        if (!reader.IsDBNull(13))
                        {
                            jvh.ForwarderList = reader.GetString(13);
                        }
                        else
                        {
                            jvh.ForwarderList = "";
                        }
                        jvh.status = reader.GetInt32(14);
                        jvh.DocumentStatus = reader.GetInt32(15);
                        if (!reader.IsDBNull(16))
                        {
                            jvh.CommentStatus = reader.GetString(16);
                        }
                        else
                        {
                            jvh.CommentStatus = "";
                        }
                        JVHeaders.Add(jvh);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Journal Header Details");
            }
            return JVHeaders;
        }
        public static List<PJVDetail> getPJVDetail(PJVHeader jvh)
        {
            PJVDetail jvd;
            List<PJVDetail> VDetail = new List<PJVDetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                 query = "select RowID,DocumentID,TemporaryNo, TemporaryDate,AccountCode,AccountName,AmountDebit,AmountCredit,SLCode, SLName, SLType " +
                   " from ViewJournalVoucher " +
                   "where DocumentID='" + jvh.DocumentID + "'" +
                   " and TemporaryNo=" + jvh.TemporaryNo +
                   " and TemporaryDate='" + jvh.TemporaryDate.ToString("yyyy-MM-dd") + "'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    jvd = new PJVDetail();
                    jvd.RowID = reader.GetInt32(0);
                    jvd.DocumentID = reader.GetString(1);
                    jvd.TemporaryNo = reader.GetInt32(2);
                    jvd.TemporaryDate = reader.GetDateTime(3).Date;
                    jvd.AccountCode = reader.GetString(4);
                    jvd.AccountName = reader.GetString(5);
                    jvd.AmountDebit = reader.GetDecimal(6);
                    jvd.AmountCredit = reader.GetDecimal(7);
                    jvd.SLCode = reader.IsDBNull(8)?"":reader.GetString(8);
                    jvd.SLName = reader.IsDBNull(9) ? "" : reader.GetString(9);
                    jvd.SLType = reader.IsDBNull(10) ? "" : reader.GetString(10);
                    VDetail.Add(jvd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Journal Details");
            }
            return VDetail;
        }
        public Boolean reversePJVHeader(PJVHeader jvh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update JournalVoucherHeader set DocumentStatus=" + jvh.DocumentStatus+
                    ", forwardUser='" + jvh.ForwardUser + "'" +
                    ", commentStatus='" + jvh.CommentStatus + "'" +
                    ", ForwarderList='" + jvh.ForwarderList + "'" +
                    " where DocumentID='" + jvh.DocumentID + "'" +
                    " and TemporaryNo=" + jvh.TemporaryNo +
                    " and TemporaryDate='" + jvh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "JournalVoucherHeader", "", updateSQL) +
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


        public Boolean updatePJVHeaderAndDetail( PJVHeader jvh, PJVHeader prevjvh, List<PJVDetail> JVDetails)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update JournalVoucherHeader set Narration='" + jvh.Narration +
                   "', Comments='" + jvh.Comments +
                   "', JournalDate='" + jvh.JournalDate.ToString("yyyy-MM-dd") +
                    "', CommentStatus='" + jvh.CommentStatus +
                   "', ForwarderList='" + jvh.ForwarderList + "'" +
                   " where DocumentID='" + prevjvh.DocumentID + "'" +
                   " and TemporaryNo=" + prevjvh.TemporaryNo +
                   " and TemporaryDate='" + prevjvh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "JournalVoucherHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from JournalVoucherDetail where DocumentID='" + prevjvh.DocumentID + "'" +
                     " and TemporaryNo=" + prevjvh.TemporaryNo +
                     " and TemporaryDate='" + prevjvh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "JournalVoucherDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (PJVDetail jvd in JVDetails)
                {
                    updateSQL = "insert into JournalVoucherDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,AccountCode,AmountDebit,AmountCredit,SLCode,SLType) " +
                    "values ('" + jvd.DocumentID + "'," +
                    jvd.TemporaryNo + "," +
                    "'" + jvd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + jvd.AccountCode + "'," +
                   jvd.AmountDebit + "," +
                     jvd.AmountCredit + ",'" + jvd.SLCode + "','" + jvd.SLType + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "JournalVoucherDetail", "", updateSQL) +
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
        public Boolean InsertPJVHeaderAndDetail(PJVHeader jvh, List<PJVDetail> JVDetails)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                jvh.TemporaryNo = DocumentNumberDB.getNumber(jvh.DocumentID, 1);
                if (jvh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + jvh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + jvh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into JournalVoucherHeader " +
                    "(DocumentID,TemporaryNo,TemporaryDate,JournalNo,JournalDate,Narration," +
                    "Comments,CommentStatus,CreateUser,CreateTime,ForwarderList,DocumentStatus,Status)" +
                    " values (" +
                    "'" + jvh.DocumentID + "'," +
                    jvh.TemporaryNo + "," +
                    "'" + jvh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     jvh.JournalNo + "," +
                    "'" + jvh.JournalDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + jvh.Narration + "'," +
                     "'" + jvh.Comments + "'," +
                       "'" + jvh.CommentStatus + "'," +
                    "'" + Login.userLoggedIn + "'," +
                    "GETDATE()" + "," +
                    "'" + jvh.ForwarderList + "'," +
                    jvh.DocumentStatus + "," +
                         jvh.status + ")";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "JournalVoucherHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from JournalVoucherDetail where DocumentID='" + jvh.DocumentID + "'" +
                     " and TemporaryNo=" + jvh.TemporaryNo +
                     " and TemporaryDate='" + jvh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "JournalVoucherDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (PJVDetail jvd in JVDetails)
                {
                    updateSQL = "insert into JournalVoucherDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,AccountCode,AmountDebit,AmountCredit,SLCode,SLType) " +
                    "values ('" + jvd.DocumentID + "'," +
                    jvh.TemporaryNo + "," +
                    "'" + jvd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + jvd.AccountCode + "'," +
                   jvd.AmountDebit + "," +
                     jvd.AmountCredit + ",'" + jvd.SLCode + "','" + jvd.SLType + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "JournalVoucherDetail", "", updateSQL) +
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
        public static PJVHeader getPJVHeaderForTrialBalance(PJVHeader jvhTemp)
        {
            PJVHeader jvh = new PJVHeader();
            try
            {
                string query = "select distinct DocumentID,TemporaryNo,TemporaryDate,JournalNo,JournalDate," +
                    " Narration,status,DocumentStatus " +
                    " from JournalVoucherHeader" +
                    " where DocumentID = '" + jvhTemp.DocumentID + "' and JournalNo = " + jvhTemp.JournalNo +
                    " and JournalDate = '" + jvhTemp.JournalDate.ToString("yyyy-MM-dd") + "'";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    try
                    {
                        jvh.DocumentID = reader.GetString(0);
                        jvh.TemporaryNo = reader.GetInt32(1);
                        jvh.TemporaryDate = reader.GetDateTime(2);
                        jvh.JournalNo = reader.GetInt32(3);
                        jvh.JournalDate = reader.GetDateTime(4);
                        jvh.Narration = reader.GetString(5);
                        jvh.status = reader.GetInt32(6);
                        jvh.DocumentStatus = reader.GetInt32(7);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Journal Header Details");
            }
            return jvh;
        }
    }
}
