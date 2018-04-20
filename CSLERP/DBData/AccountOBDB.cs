using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    public class AccountOBheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string FYID { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public int Status { get; set; }
        public string CreateUser { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreateTime { get; set; }
        public decimal balancedebit { get; set; }
        public decimal balancecredit { get; set; }
        public int serialno { get; set; }
        public string accountcode { get; set; }
        public string accountname { get; set; }

    }
    public class AccountOBdetail
    {
        public int RowID { get; set; }
        public string FYID { get; set; }
        public string DocumentID { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public int SerialNo { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal BalanceDebit { get; set; }
        public decimal BalanceCredit { get; set; }
        public decimal OBValue { get; set; }
    }
    class AccountOBDB
    {
        public static List<AccountOBheader> getAccountOblist(int opt)
        {
            AccountOBheader Alist;
            List<AccountOBheader> AccList = new List<AccountOBheader>();
            try
            {
                string query1 = "select distinct DocumentID,DocumentNo,DocumentDate,FYID, " +
                    "Status, Createuser,CreatorName, CreateTime " +
                    "from ViewAccountOB where (Createuser='" + Login.userLoggedIn + "' and Status = 0 )";
                string query2 = "select distinct DocumentID,DocumentNo,DocumentDate,FYID, " +
                    "Status, Createuser,CreatorName, CreateTime " +
                    "from ViewAccountOB where Status = 1";
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
                    case 6:
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
                    Alist = new AccountOBheader();
                    Alist.DocumentID = reader.GetString(0);
                    Alist.DocumentNo = reader.GetInt32(1);
                    Alist.DocumentDate = reader.GetDateTime(2);
                    Alist.FYID = reader.GetString(3);
                    Alist.Status = reader.GetInt32(4);
                    Alist.CreateUser = reader.GetString(5);
                    Alist.CreatorName = reader.GetString(6);
                    Alist.CreateTime = reader.GetDateTime(7);
                    AccList.Add(Alist);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return AccList;
        }
        public static List<AccountOBdetail> getAccountOBDetail(AccountOBheader Acchr)
        {
            AccountOBdetail Accd;
            List<AccountOBdetail> AccDetail = new List<AccountOBdetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,DocumentNo,DocumentDate,SerialNo, " +
                    "AccountCode,AccountName,BalanceDebit,BalanceCredit " +
                   "from ViewAccountOB " +
                    " where DocumentID='" + Acchr.DocumentID + "'" +
                    " and DocumentNo=" + Acchr.DocumentNo +
                    " and DocumentDate='" + Acchr.DocumentDate.ToString("yyyy-MM-dd") + "'" + " order by SerialNo";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Accd = new AccountOBdetail();
                    Accd.RowID = reader.GetInt32(0);
                    Accd.DocumentID = reader.GetString(1);                  
                    Accd.DocumentNo = reader.GetInt32(2);
                    Accd.DocumentDate = reader.GetDateTime(3).Date;
                    if (!reader.IsDBNull(4))
                    {
                        Accd.SerialNo = reader.GetInt32(4);
                    }
                    Accd.AccountCode = reader.GetString(5);
                    Accd.AccountName = reader.GetString(6);
                    Accd.BalanceDebit = reader.GetDecimal(7);
                    Accd.BalanceCredit = reader.GetDecimal(8);
                    AccDetail.Add(Accd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return AccDetail;
        }

        //public Boolean updateAccHeader(AccountOBheader newacc, AccountOBheader oldacc)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "update AccountOBHeader set  FYID='" + newacc.FYID + "'" +
        //             " where DocumentID='" + oldacc.DocumentID +
        //            "' and DocumentNo=" + oldacc.DocumentNo +
        //            " and DocumentDate='" + oldacc.DocumentDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("update", "ProductTestTemplateHeader", "", updateSQL) +
        //        Main.QueryDelimiter;
        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        //        status = false;
        //    }
        //    return status;
        //}
        public Boolean checkFYID(string acchr)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                SqlConnection con = new SqlConnection(Login.connString);
                string query = "select FYID from AccountOBHeader";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                   if( (acchr == reader.GetString(0)))
                    {
                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status =false;
            }
            return status;

        }
        //public Boolean insertAccountOBHeader(AccountOBheader Acchr)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "insert into AccountOBHeader " +
        //            " (DocumentID,FYID,DocumentDate,DocumentNo," +
        //            "CreateUser,CreateTime)" +
        //            "values (" +
        //            "'" + Acchr.DocumentID + "'," +
        //           Acchr.FYID + "," +
        //           "'" + Acchr.DocumentDate.ToString("yyyy-MM-dd") + "'," +
        //           "'" + Acchr.DocumentNo + "'," +
        //            "'" + Login.userLoggedIn + "'," +
        //            "GETDATE())";
        //        //"'" + pheader.ForwardUser + "'," +
        //        //"'" + pheader.ApproveUser + "'," +
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("insert", "AccountOBHeader", "", updateSQL) +
        //        Main.QueryDelimiter;
        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        //        status = false;
        //    }
        //    return status;
        //}

        //public Boolean UpdateAccountDetail(List<AccountOBdetail> AccDetail, AccountOBheader Acchdr)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "Delete from AccountOBDetail where DocumentID='" + Acchdr.DocumentID + "'" +
        //            " and DocumentNo=" + Acchdr.DocumentNo +
        //            " and DocumentDate='" + Acchdr.DocumentDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //            ActivityLogDB.PrepareActivityLogQquerString("delete", "AccountOBDetail", "", updateSQL) +
        //            Main.QueryDelimiter;
        //        foreach (AccountOBdetail acc in AccDetail)
        //        {
        //            updateSQL = "insert into AccountOBDetail " +
        //            "(DocumentID,DocumentNo,DocumentDate,SerialNo,AccountCode,BalanceDebit,BalanceCredit) " +
        //            "values ('" + acc.DocumentID + "'," +
        //            acc.DocumentNo + "," +
        //            "'" + acc.DocumentDate.ToString("yyyy-MM-dd") + "'," +
        //            "'" + acc.SerialNo + "'," +
        //             acc.AccountCode + "," +
        //            "'" + acc.BalanceDebit + "'," +
        //            "'" + acc.BalanceCredit + "')";
        //            utString = utString + updateSQL + Main.QueryDelimiter;
        //            utString = utString +
        //            ActivityLogDB.PrepareActivityLogQquerString("insert", "AccountOBDetail", "", updateSQL) +
        //            Main.QueryDelimiter;
        //        }
        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        //        status = false;
        //    }
        //    return status;
        //}
        public Boolean validateAccOB(AccountOBheader acchdr)
        {
            Boolean status = true;
            try
            {
                if (acchdr.DocumentID.Trim().Length == 0 || acchdr.DocumentID == null)
                {
                    return false;
                }
                if (acchdr.FYID.Trim().Length == 0 || acchdr.FYID == null)
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
        public Boolean FinalizeAccountOB(AccountOBheader Acchr)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update AccountOBheader set Status = 1" +
                    " where DocumentNo=" + Acchr.DocumentNo +
                    " and DocumentDate='" + Acchr.DocumentDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductTestTemplateHeader", "", updateSQL) +
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
        public Boolean updateAccHeaderAndDetail(AccountOBheader newacc, AccountOBheader oldacc, List<AccountOBdetail> AccDetail)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update AccountOBHeader set  FYID='" + newacc.FYID + "'" +
                 " where DocumentID='" + oldacc.DocumentID +
                "' and DocumentNo=" + oldacc.DocumentNo +
                " and DocumentDate='" + oldacc.DocumentDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "AccountOBHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from AccountOBDetail where DocumentID='" + oldacc.DocumentID + "'" +
                " and DocumentNo=" + oldacc.DocumentNo +
                " and DocumentDate='" + oldacc.DocumentDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "AccountOBDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (AccountOBdetail acc in AccDetail)
                {
                    updateSQL = "insert into AccountOBDetail " +
                    "(DocumentID,DocumentNo,DocumentDate,SerialNo,AccountCode,BalanceDebit,BalanceCredit,FYID) " +
                    "values ('" + acc.DocumentID + "'," +
                    acc.DocumentNo + "," +
                    "'" + acc.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + acc.SerialNo + "'," +
                     acc.AccountCode + "," +
                    "'" + acc.BalanceDebit + "'," +
                    "'" + acc.BalanceCredit +"','"+ acc.FYID + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "AccountOBDetail", "", updateSQL) +
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
        public Boolean InsertAccHeaderAndDetail(AccountOBheader newacc, List<AccountOBdetail> AccDetail)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                newacc.DocumentNo = DocumentNumberDB.getNumber(newacc.DocumentID, 1);
                if (newacc.DocumentNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + newacc.DocumentNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + newacc.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into AccountOBHeader " +
                      " (DocumentID,FYID,DocumentDate,DocumentNo," +
                      "CreateUser,CreateTime)" +
                      "values (" +
                      "'" + newacc.DocumentID + "','" +
                     newacc.FYID + "'," +
                     "'" + newacc.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + newacc.DocumentNo + "'," +
                      "'" + Login.userLoggedIn + "'," +
                      "GETDATE())";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "AccountOBHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from AccountOBDetail where DocumentID='" + newacc.DocumentID + "'" +
                " and DocumentNo=" + newacc.DocumentNo +
                " and DocumentDate='" + newacc.DocumentDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "AccountOBDetail", "", updateSQL) +
                    Main.QueryDelimiter;

                foreach (AccountOBdetail acc in AccDetail)
                {
                    updateSQL = "insert into AccountOBDetail " +
                    "(DocumentID,DocumentNo,DocumentDate,SerialNo,AccountCode,BalanceDebit,BalanceCredit,FYID) " +
                    "values ('" + acc.DocumentID + "'," +
                    newacc.DocumentNo + "," +
                    "'" + acc.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + acc.SerialNo + "'," +
                     acc.AccountCode + "," +
                    "'" + acc.BalanceDebit + "'," +
                    "'" + acc.BalanceCredit +"','"+ acc.FYID + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "AccountOBDetail", "", updateSQL) +
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
        public static List<AccountOBdetail> getAccountOBListForPericualrFY(string fy)
        {
            AccountOBdetail Accd;
            List<AccountOBdetail> AccDetail = new List<AccountOBdetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,DocumentNo,DocumentDate,SerialNo, " +
                    "AccountCode,AccountName,BalanceDebit,BalanceCredit,(BalanceDebit - BalanceCredit) as OB " + 
                   "from ViewAccountOB " +
                    " where FYID='" + fy.Trim() + "' and AcStatus=1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Accd = new AccountOBdetail();
                    Accd.RowID = reader.GetInt32(0);
                    Accd.DocumentID = reader.GetString(1);
                    Accd.DocumentNo = reader.GetInt32(2);
                    Accd.DocumentDate = reader.GetDateTime(3).Date;
                    if (!reader.IsDBNull(4))
                    {
                        Accd.SerialNo = reader.GetInt32(4);
                    }
                    Accd.AccountCode = reader.GetString(5);
                    Accd.AccountName = reader.GetString(6);
                    Accd.BalanceDebit = reader.GetDecimal(7);
                    Accd.BalanceCredit = reader.GetDecimal(8);
                    Accd.OBValue = reader.GetDecimal(9);
                    AccDetail.Add(Accd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return AccDetail;
        }
    }
}
