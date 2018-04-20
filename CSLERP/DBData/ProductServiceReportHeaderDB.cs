using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    public class productservicereportheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int ReportType { get; set; }
        public int ReportNo { get; set; }
        public DateTime ReportDate { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int SMRNNo { get; set; }
        public DateTime SMRNDate { get; set; }
        public int SMRNHeaderTempNo { get; set; }
        public DateTime SMRNHeaderTempDate { get; set; }
        public int Status { get; set; }
        public string CreateUser { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreateTime { get; set; }
        public int jobIDNo { get; set; }
    }
    public class productservicereportdetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int ReportType { get; set; }
        public int ReportNo { get; set; }
        public DateTime ReportDate { get; set; }
        public int SMRNNo { get; set; }
        public DateTime SMRNDate { get; set; }
        public int SMRNHeaderNo { get; set; }
        public DateTime SMRNHeaderDate { get; set; }
        public int SMRNHeaderTempNo { get; set; }
        public DateTime SMRNHeaderTempDate { get; set; }
        public string CreateUser { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreateTime { get; set; }
        public int jobIDNo { get; set; }
        public string TestDescriptionID { get; set; }
        public string TestResult { get; set; }
        public string TestRemarks { get; set; }
    }
    class ProductServiceReportHeaderDB
    {
        public List<productservicereportheader> getFilteredPSRHeader(string userList, int opt, string userCommentStatusString)
        {
            productservicereportheader psrheader;
            List<productservicereportheader> PSRHeaders = new List<productservicereportheader>();
            string acStr = "";
            //try
            //{
            //    acStr = userCommentStatusString.Substring(0, userCommentStatusString.Length - 2) + "1" + Main.delimiter2;
            //}
            //catch (Exception ex)
            //{
            //    acStr = "";
            //}
            try
            {
                string query1= "select distinct RowID, DocumentID, DocumentName," +
                    " ReportType,ReportNo,ReportDate,TemporaryNo, TemporaryDate, SMRNNo,SMRNDate,SMRNHeaderTempNo,SMRNHeaderTempDate,JobIDNo," +
                    " CreateUser,CreatorName,CreateTime,Status " +
                    " from ViewProductServiceReportHeader" +
                    " where (Createuser='" + Login.userLoggedIn + "' and Status = 0)" +
                    " order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";
                string query2 = "select distinct RowID, DocumentID, DocumentName," +
                    " ReportType,ReportNo,ReportDate,TemporaryNo, TemporaryDate, SMRNNo,SMRNDate,SMRNHeaderTempNo,SMRNHeaderTempDate,JobIDNo," +
                    " CreateUser,CreatorName,CreateTime,Status " +
                    " from ViewProductServiceReportHeader" +
                    " where  Status = 1 and ReportNo != 0  order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";
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
                    psrheader = new productservicereportheader();
                    psrheader.RowID = reader.GetInt32(0);
                    psrheader.DocumentID = reader.GetString(1);
                    psrheader.DocumentName = reader.GetString(2);
                    psrheader.ReportType = reader.GetInt32(3);
                    psrheader.ReportNo = reader.GetInt32(4);
                    psrheader.ReportDate = reader.GetDateTime(5);
                    psrheader.TemporaryNo = reader.GetInt32(6);
                    if (!reader.IsDBNull(7))
                    {
                        psrheader.TemporaryDate = reader.GetDateTime(7);
                    }

                    psrheader.SMRNNo = reader.GetInt32(8);
                    psrheader.SMRNDate = reader.GetDateTime(9);
                    psrheader.SMRNHeaderTempNo = reader.GetInt32(10);
                    psrheader.SMRNHeaderTempDate = reader.GetDateTime(11);
                    psrheader.jobIDNo = reader.GetInt32(12);
                    psrheader.CreateUser = reader.GetString(13);
                    psrheader.CreatorName = reader.GetString(14);
                    psrheader.CreateTime = reader.GetDateTime(15);
                    psrheader.Status = reader.GetInt32(16);
                   
                    PSRHeaders.Add(psrheader);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Quotation Inward Details");
            }
            return PSRHeaders;
        }

    

        public static List<productservicereportdetail> getPSRDetail(productservicereportheader psrheader)
        {
            productservicereportdetail psrdetail;
            List<productservicereportdetail> PSRDetail = new List<productservicereportdetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,TemporaryNo,TemporaryDate,TestDescriptionID," +
                   "TestResult,TestRemarks " +
                   "from ProductServiceReportDetail " +
                    " where DocumentID='" + psrheader.DocumentID + "'" +
                    " and TemporaryNo=" + psrheader.TemporaryNo +
                    " and TemporaryDate='" + psrheader.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    psrdetail = new productservicereportdetail();
                    psrdetail.RowID = reader.GetInt32(0);
                    psrdetail.DocumentID = reader.GetString(1);
                    psrdetail.TemporaryNo = reader.GetInt32(2);
                    psrdetail.TemporaryDate = reader.GetDateTime(3).Date;
                    psrdetail.TestDescriptionID = reader.GetString(4);
                    psrdetail.TestResult = reader.GetString(5);
                    psrdetail.TestRemarks = reader.GetString(6);
                    PSRDetail.Add(psrdetail);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Quotation Inward Details");
            }
            return PSRDetail;
        }
        public Boolean validatePSRHeader(productservicereportheader psrh)
        {
            Boolean status = true;
            try
            {
                if (psrh.DocumentID.Trim().Length == 0 || psrh.DocumentID == null)
                {
                    return false;
                }
                if (psrh.ReportType == 0 )
                {
                    return false;
                }
                if (psrh.TemporaryDate == null)
                {
                    return false;
                }
                if (psrh.SMRNNo == 0)
                {
                    return false;
                }
                if (psrh.SMRNDate == null)
                {
                    return false;
                }
                if (psrh.SMRNHeaderTempNo == 0)
                {
                    return false;
                }
                if (psrh.jobIDNo == 0)
                {
                    return false;
                }
                if (psrh.SMRNHeaderTempDate == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public Boolean FinalizePSR(productservicereportheader psrh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductServiceReportHeader set status=1 " +
                    ", ReportNo=" + psrh.ReportNo +
                    ", ReportDate = convert(date, getdate())" +
                     " where DocumentID='" + psrh.DocumentID + "'" +
                    " and TemporaryNo=" + psrh.TemporaryNo +
                    " and TemporaryDate='" + psrh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductServiceReportHeader", "", updateSQL) +
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
        public Boolean updateInspectionStatusOfItem(productservicereportheader psrh)
        {
            Boolean status = true;
            string updateSQL = "";
            string utString = "";
            try
            {
                if (psrh.ReportType == 1)
                {
                    updateSQL = "update SMRNDetail set InspectionStatus=1 " +
                       " where JobIDNo= " + psrh.jobIDNo +
                      " and TemporaryNo=" + psrh.SMRNHeaderTempNo +
                      " and TemporaryDate='" + psrh.SMRNHeaderTempDate.ToString("yyyy-MM-dd") + "'";
                }
                else if(psrh.ReportType == 2)
                {
                    updateSQL = "update SMRNDetail set ServiceStatus=1 " +
                       " where JobIDNo= " + psrh.jobIDNo +
                      " and TemporaryNo=" + psrh.SMRNHeaderTempNo +
                      " and TemporaryDate='" + psrh.SMRNHeaderTempDate.ToString("yyyy-MM-dd") + "'";
                }
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "SMRNDetail", "", updateSQL) +
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


        public Boolean updatePSRHeaderAndDetail(productservicereportheader psrh, productservicereportheader prevpsrh,List<productservicereportdetail> PSRDetail)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductServiceReportHeader set ReportType=" + psrh.ReportType +
                    ",ReportNo=" + psrh.ReportNo +
                     ",ReportDate='" + psrh.ReportDate.ToString("yyyy-MM-dd") +
                    "', TemporaryNo=" + psrh.TemporaryNo +
                    ",TemporaryDate='" + psrh.TemporaryDate.ToString("yyyy-MM-dd") +
                    "', SMRNNo=" + psrh.SMRNNo +
                    ", SMRNDate='" + psrh.SMRNDate.ToString("yyyy-MM-dd") + "'" +
                    ",SMRNHeaderTempNo=" + psrh.SMRNHeaderTempNo +
                    ",SMRNHeaderTempDate='" + psrh.SMRNHeaderTempDate.ToString("yyyy-MM-dd") + "'," +
                      "JobIDNo=" + psrh.jobIDNo +
                   " where DocumentID='" + prevpsrh.DocumentID + "'" +
                    " and TemporaryNo=" + prevpsrh.TemporaryNo +
                    " and TemporaryDate='" + prevpsrh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductServiceReportHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from ProductServiceReportDetail where DocumentID='" + prevpsrh.DocumentID + "'" +
                    " and TemporaryNo=" + prevpsrh.TemporaryNo +
                    " and TemporaryDate='" + prevpsrh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "ProductServiceReportDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (productservicereportdetail psrd in PSRDetail)
                {
                    updateSQL = "insert into ProductServiceReportDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,TestDescriptionID,TestResult,TestRemarks) " +
                    "values ('" + psrh.DocumentID + "'," +
                    psrh.TemporaryNo + "," +
                    "'" + psrh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + psrd.TestDescriptionID + "' ," +
                     "'" + psrd.TestResult + "' ," +
                     "'" + psrd.TestRemarks + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductServiceReportDetail", "", updateSQL) +
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
        public Boolean InsertPSRHeaderAndDetail(productservicereportheader psrh, List<productservicereportdetail> PSRDetail)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                psrh.TemporaryNo = DocumentNumberDB.getNumber(psrh.DocumentID, 1);
                if (psrh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + psrh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + psrh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into ProductServiceReportHeader " +
                    "(DocumentID,ReportType,ReportNo,ReportDate,TemporaryNo, TemporaryDate," +
                    "SMRNHeaderTempNo,SMRNHeaderTempDate,SMRNNo,SMRNDate,JobIDNo,CreateUser,CreateTime,Status)" +
                    " values (" +
                    "'" + psrh.DocumentID + "'," +
                    "'" + psrh.ReportType + "'," +
                    psrh.ReportNo + "," +
                    "'" + psrh.ReportDate.ToString("yyyy-MM-dd") + "'," +
                    psrh.TemporaryNo + "," +
                    "'" + psrh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     psrh.SMRNHeaderTempNo + "," +
                    "'" + psrh.SMRNHeaderTempDate.ToString("yyyy-MM-dd") + "'," +
                    +psrh.SMRNNo + "," +
                    "'" + psrh.SMRNDate.ToString("yyyy-MM-dd") + "'," +
                    psrh.jobIDNo + "," +
                    "'" + Login.userLoggedIn + "'," +
                    "GETDATE()" + "," +
                     psrh.Status + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductServiceReportHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from ProductServiceReportDetail where DocumentID='" + psrh.DocumentID + "'" +
                     " and TemporaryNo=" + psrh.TemporaryNo +
                     " and TemporaryDate='" + psrh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "ProductServiceReportDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (productservicereportdetail psrd in PSRDetail)
                {
                    updateSQL = "insert into ProductServiceReportDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,TestDescriptionID,TestResult,TestRemarks) " +
                    "values ('" + psrh.DocumentID + "'," +
                    psrh.TemporaryNo + "," +
                    "'" + psrh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + psrd.TestDescriptionID + "' ," +
                     "'" + psrd.TestResult + "' ," +
                     "'" + psrd.TestRemarks + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductServiceReportDetail", "", updateSQL) +
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
