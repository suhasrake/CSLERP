using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    public class smrnservicedlist
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int ListNo { get; set; }
        public DateTime ListDate { get; set; }
        public int SMRNHeaderNo { get; set; }
        public DateTime SMRNHeaderDate { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporayDate { get; set; }
        public int TrackingNo { get; set; }
        public DateTime TrackingDate { get; set; }
        public string CustomerPONo { get; set; }
        public DateTime CustomerPODate { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int JobIDNo { get; set; }
        public int Status { get; set; }
        public int DocumentStatus { get; set; }
        public string CreateUser { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreateTime { get; set; }

    }
    class SMRNServicedListDB
    {
        public List<smrnservicedlist> getFilteredServicedList(string userList, int opt, string userCommentStatusString)
        {
            smrnservicedlist ServiceList;
            List<smrnservicedlist> SMRNServiceList = new List<smrnservicedlist>();
            //string acStr = "";
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
                string query1 = "select distinct DocumentID,ListNo,ListDate," +
                    " TrackingNo, TrackingDate,CustomerPONo, CustomerPODate,CustomerID " +
                    "from viewSMRNServicedList where (Createuser='" + Login.userLoggedIn + "' and Status = 1 and DocumentStatus = 1)";
                string query2 = "select distinct DocumentID,ListNo,ListDate," +
                    " TrackingNo, TrackingDate,CustomerPONo, CustomerPODate,CustomerID " +
                     "from viewSMRNServicedList where  Createuser='" + Login.userLoggedIn + "' and Status = 1 and DocumentStatus = 99";
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
                    ServiceList = new smrnservicedlist();
                    ServiceList.DocumentID = reader.GetString(0);
                    ServiceList.ListNo = reader.GetInt32(1);
                    ServiceList.ListDate = reader.GetDateTime(2);
                    ServiceList.TrackingNo = reader.GetInt32(3);
                    ServiceList.TrackingDate = reader.GetDateTime(4);
                    ServiceList.CustomerPONo = reader.GetString(5);
                    ServiceList.CustomerPODate = reader.GetDateTime(6);
                    ServiceList.CustomerID = reader.GetString(7);
                    SMRNServiceList.Add(ServiceList);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Serviced List Details");
            }
            return SMRNServiceList;
        }
        //public static List<smrnservicedlist> getFilteredServicedListDetail( smrnservicedlist list, int opt)
        //{
        //    smrnservicedlist ServiceList;
        //    List<smrnservicedlist> SMRNServiceList = new List<smrnservicedlist>();
        //    try
        //    {
        //        string query1 = "select SMRNHeaderNo,SMRNHeaderDate," +
        //           " TemporaryNo, TemporaryDate,CustomerID, JObIDNo, Status, " +
        //           "DocumentStatus,CreateUser,CreateTime from ViewSMRNServicedList" +
        //           " Createuser='" + Login.userLoggedIn + "' and Status = 1 and DocumentStatus = 1"+
        //           " and ListNo = " + list.ListNo+
        //           "and ListDate = " + list.ListDate;
        //        string query2 = "select RowID, DocumentID, ListNo,ListDate,SMRNHeaderNo,SMRNHeaderDate," +
        //           " TemporaryNo, TemporaryDate,TrackingNo, TrackingDate,CustomerPONo, CustomerPODate,CustomerID, JObIDNo, Status, " +
        //           "DocumentStatus,CreateUser,CreateTime from ViewSMRNServicedList" +
        //           " and Createuser='" + Login.userLoggedIn + "' and Status = 1 and DocumentStatus = 99)"+
        //           " and ListNo = " + list.ListNo +
        //           "and ListDate = " + list.ListDate;
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "";
        //        switch (opt)
        //        {
        //            case 1:
        //                query = query1;
        //                break;
        //            case 2:
        //                query = query2;
        //                break;
        //            default:
        //                query = "";
        //                break;
        //        }
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            ServiceList = new smrnservicedlist();
        //            ServiceList.SMRNHeaderNo = reader.GetInt32(0);
        //            ServiceList.SMRNHeaderDate = reader.GetDateTime(1);
        //            ServiceList.TemporaryNo = reader.GetInt32(2);
        //            ServiceList.TemporayDate = reader.GetDateTime(3);
        //            ServiceList.CustomerID = reader.GetString(4);
        //            ServiceList.JobIDNo = reader.GetInt32(5);
        //            ServiceList.Status = reader.GetInt32(6);
        //            ServiceList.DocumentStatus = reader.GetInt32(7);
        //            ServiceList.CreateUser = reader.GetString(8);
        //            ServiceList.CreateTime = reader.GetDateTime(9);
        //            SMRNServiceList.Add(ServiceList);
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error querying Serviced List Details");
        //    }
        //    return SMRNServiceList;
        //}
        //public static Boolean updateBililngRequestStatus(smrnservicedlist servList, string tempNo, DateTime tempDate, int opt)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL1 = "update SMRNDetail set BillingRequestStatus= 1 " +
        //            " where TemporaryNo=" + tempNo +
        //            " and TemporaryDate='" + tempDate.ToString("yyyy-MM-dd") + "'"+
        //            " and JobIDNo = " + servList.JobIDNo;
        //        string updateSQL2 = "update SMRNDetail set BillingRequestStatus= 0 " +
        //            " where TemporaryNo=" + tempNo +
        //            " and TemporaryDate='" + tempDate.ToString("yyyy-MM-dd") + "'" +
        //            " and JobIDNo = " + servList.JobIDNo;
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string updateSQL = "";
        //        switch (opt)
        //        {
        //            case 1:
        //                updateSQL = updateSQL1;
        //                break;
        //            case 2:
        //                updateSQL = updateSQL2;
        //                break;
        //            default:
        //                updateSQL = "";
        //                break;
        //        }
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("update", "SMRNDetail", "", updateSQL) +
        //        Main.QueryDelimiter;
        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        status = false;
        //    }
        //    return status;
        //}


        //public static List<productservicereportdetail> getPSRDetail(productservicereportheader psrheader)
        //{
        //    productservicereportdetail psrdetail;
        //    List<productservicereportdetail> PSRDetail = new List<productservicereportdetail>();
        //    try
        //    {
        //        string query = "";
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        query = "select RowID,DocumentID,TemporaryNo,TemporaryDate,TestDescriptionID," +
        //           "TestResult,TestRemarks " +
        //           "from ProductServiceReportDetail " +
        //            " where DocumentID='" + psrheader.DocumentID + "'" +
        //            " and TemporaryNo=" + psrheader.TemporaryNo +
        //            " and TemporaryDate='" + psrheader.TemporaryDate.ToString("yyyy-MM-dd") + "'";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            psrdetail = new productservicereportdetail();
        //            psrdetail.RowID = reader.GetInt32(0);
        //            psrdetail.DocumentID = reader.GetString(1);
        //            psrdetail.TemporaryNo = reader.GetInt32(2);
        //            psrdetail.TemporaryDate = reader.GetDateTime(3).Date;
        //            psrdetail.TestDescriptionID = reader.GetString(4);
        //            psrdetail.TestResult = reader.GetString(5);
        //            psrdetail.TestRemarks = reader.GetString(6);
        //            PSRDetail.Add(psrdetail);
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error querying Quotation Inward Details");
        //    }
        //    return PSRDetail;
        //}

        //public Boolean updateSMRNServicedList(smrnservicedlist servList)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "update SMRNServicedList set SMRNHeaderNo=" + servList.SMRNHeaderNo +
        //            ",SMRNHeaderDate=" + servList.SMRNHeaderDate.ToString("yyyy-MM-dd") +
        //              "JobIDNo=" + servList.JobIDNo +
        //           " where DocumentID='" + servList.DocumentID + "'" +
        //            " and TemporaryNo=" + servList.TemporaryNo +
        //            " and TemporaryDate='" + servList.TemporaryDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("update", "ProductServiceReportHeader", "", updateSQL) +
        //        Main.QueryDelimiter;
        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        status = false;
        //    }
        //    return status;
        //}

        //public Boolean UpdatePSRDetail(List<productservicereportdetail> PSRDetail, productservicereportheader psrh)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "Delete from ProductServiceReportDetail where DocumentID='" + psrh.DocumentID + "'" +
        //            " and TemporaryNo=" + psrh.TemporaryNo +
        //            " and TemporaryDate='" + psrh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //            ActivityLogDB.PrepareActivityLogQquerString("delete", "ProductServiceReportDetail", "", updateSQL) +
        //            Main.QueryDelimiter;
        //        foreach (productservicereportdetail psrd in PSRDetail)
        //        {
        //            updateSQL = "insert into ProductServiceReportDetail " +
        //            "(DocumentID,TemporaryNo,TemporaryDate,TestDescriptionID,TestResult,TestRemarks) " +
        //            "values ('" + psrh.DocumentID + "'," +
        //            psrh.TemporaryNo + "," +
        //            "'" + psrh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
        //             "'" + psrd.TestDescriptionID + "' ,"+
        //             "'" + psrd.TestResult + "' ," +
        //             "'" + psrd.TestRemarks+ "')";
        //            utString = utString + updateSQL + Main.QueryDelimiter;
        //            utString = utString +
        //            ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductServiceReportDetail", "", updateSQL) +
        //            Main.QueryDelimiter;
        //        }
        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = false;
        //    }
        //    return status;
        //}
        public static List<smrndetail> getSMRNDetailForServiceList(smrnservicedlist list, int opt)
        {
            smrndetail smrnd;
            List<smrndetail> SMRNDetail = new List<smrndetail>();
            try
              {

                SqlConnection conn = new SqlConnection(Login.connString);
                string query1 = "select a.SMRNHeaderNo,a.SMRNHeaderDate,a.JobIDNo,b.TemporaryNo,b.TemporaryDate," +
                   "b.StockItemID,b.StockItemName,b.SerialNo,b.ItemDetails,b.WarrantyStatus,b.ProductServiceStatus " +
                   "from SMRNServicedList as a left outer join" +
                    " ViewSMRNDetail as b on a.SMRNHeaderNo = b.SMRNHeaderNo and a.SMRNHeaderDate=b.SMRNHeaderDate and a.JobIDNo=b.JobIDNo" +
                    " where a.ListNo ="+list.ListNo+
                    " and a.ListDate = '"+list.ListDate.ToString("yyyy-MM-dd") + "'";

                string query2 = "select SMRNHeaderNo, SMRNHeaderDate,JobIDNo,TemporaryNo, TemporaryDate, StockItemID,StockItemName, SerialNo, " +
                   "ItemDetails,WarrantyStatus, ProductServiceStatus " +
                   "from ViewSMRNDetail where " +
                    " DocumentID='SMRNHEADER'" +
                    " and TrackingNo=" + list.TrackingNo +
                    " and TrackingDate='" + list.TrackingDate.ToString("yyyy-MM-dd") + "'"+
                    " and BillingRequestStatus = 0";
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
                    smrnd = new smrndetail();
                    smrnd.InspectionStatus = reader.GetInt32(0); // for storing SMRNHEaderNo
                    smrnd.TemporaryDate = reader.GetDateTime(1); // for storing SMRNHEaderDate
                    smrnd.JobIDNo = reader.GetInt32(2);
                    smrnd.TemporaryNo = reader.GetInt32(3);
                    smrnd.TemporaryDate = reader.GetDateTime(4);
                    smrnd.StockItemID = reader.GetString(5);
                    smrnd.StockItemName = reader.GetString(6);
                    smrnd.SerialNo = reader.GetString(7);
                    smrnd.ItemDetails = reader.GetString(8);
                    smrnd.WarrantyStatus = reader.GetInt32(9);
                    smrnd.ProductServiceStatus = reader.GetString(10);
                   
                    SMRNDetail.Add(smrnd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying SMRN Details");
            }
            return SMRNDetail;
        }
        public Boolean updateSMRNServicedList(smrnservicedlist servList, List<smrnservicedlist> list, int opt)
        {
            string updateSQL = "";
            Boolean status = true;
            string utString = "";
            try
            {
                if (opt == 1)
                {
                    foreach (smrnservicedlist serv in list)
                    {
                        updateSQL = "insert into SMRNServicedList " +
                         "(DocumentID,ListNo,ListDate,SMRNHeaderNo, SMRNHeaderDate,Status,DocumentStatus," +
                         "JobIDNo,CreateUser,CreateTime)" +
                         " values (" +
                         "'" + servList.DocumentID + "'," +
                         servList.ListNo + "," +
                         "'" + servList.ListDate.ToString("yyyy-MM-dd") + "'," +
                         serv.SMRNHeaderNo + "," +
                         "'" + serv.SMRNHeaderDate.ToString("yyyy-MM-dd") + "'," +
                          servList.Status + "," +
                           servList.DocumentStatus + "," +
                         serv.JobIDNo + "," +
                         "'" + Login.userLoggedIn + "'," +
                         "GETDATE()" + ")";
                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                        ActivityLogDB.PrepareActivityLogQquerString("insert", "SMRNServicedList", "", updateSQL) +
                        Main.QueryDelimiter;

                        updateSQL = "update SMRNDetail set BillingRequestStatus= 1 " +
                        " where TemporaryNo=" + serv.ListNo +   // for Temporary no
                        " and TemporaryDate='" + serv.ListDate.ToString("yyyy-MM-dd") + "'" +   // for temporary date
                        " and JobIDNo = " + serv.JobIDNo;
                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                        ActivityLogDB.PrepareActivityLogQquerString("update", "SMRNDetail", "", updateSQL) +
                        Main.QueryDelimiter;
                    }
                    if (!UpdateTable.UT(utString))
                    {
                        status = false;
                    }
                }
                else if(opt == 2)
                {
                    foreach (smrnservicedlist serv in list)
                    {
                        updateSQL = "delete from SMRNServicedList where ListNo = "+ servList.ListNo+
                            " and ListDate = '" + servList.ListDate.ToString("yyyy-MM-dd")+ "'"+
                            "and SMRNHeaderNo = " + serv.SMRNHeaderNo + 
                        "and SMRNHeaderDate = '" + serv.SMRNHeaderDate.ToString("yyyy-MM-dd") + "'" ;
                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                        ActivityLogDB.PrepareActivityLogQquerString("delete", "SMRNServicedList", "", updateSQL) +
                        Main.QueryDelimiter;

                        updateSQL = "update SMRNDetail set BillingRequestStatus= 0 " +
                    " where TemporaryNo=" + serv.ListNo +    // for temporary No
                    " and TemporaryDate='" + serv.ListDate.ToString("yyyy-MM-dd") + "'" +   // for Temporay Date
                    " and JobIDNo = " + serv.JobIDNo;
                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                        ActivityLogDB.PrepareActivityLogQquerString("update", "SMRNDetail", "", updateSQL) +
                        Main.QueryDelimiter;
                    }
                    if (!UpdateTable.UT(utString))
                    {
                        status = false;
                    }
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        public Boolean deleteSMRNServicedList(smrnservicedlist servList)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "delete SMRNServicedList " +
                    " where DocumentID='" + servList.DocumentID + "'" +
                    " and ListNo=" + servList.ListNo +
                    " and ListDate='" + servList.ListDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("delete", "SMRNServicedList", "", updateSQL) +
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

        public Boolean validateSMRNServicedList(smrnservicedlist servList)
        {
            Boolean status = true;
            try
            {
                if (servList.DocumentID.Trim().Length == 0 || servList.DocumentID == null)
                {
                    return false;
                }
                if (servList.ListNo == 0)
                {
                    return false;
                }
                if (servList.Status == 0)
                {
                    return false;
                }
                if (servList.ListDate == null)
                {
                    return false;
                }
                if (servList.SMRNHeaderNo == 0)
                {
                    return false;
                }
                if (servList.SMRNHeaderDate == null)
                {
                    return false;
                }
                if (servList.JobIDNo == 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public Boolean FinalizeSMRNServicedList(smrnservicedlist servList)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update SMRNServicedList set DocumentStatus = 99" +
                    " where ListNo=" + servList.ListNo +
                    " and ListDate='" + servList.ListDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "SMRNServicedList", "", updateSQL) +
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
        //public Boolean updateInspectionStatusOfItem(productservicereportheader psrh)
        //{
        //    Boolean status = true;
        //    string updateSQL = "";
        //    string utString = "";
        //    try
        //    {
        //        if (psrh.ReportType == 1)
        //        {
        //            updateSQL = "update SMRNDetail set InspectionStatus=1 " +
        //               " where JobIDNo= " + psrh.jobIDNo +
        //              " and TemporaryNo=" + psrh.SMRNHeaderTempNo +
        //              " and TemporaryDate='" + psrh.SMRNHeaderTempDate.ToString("yyyy-MM-dd") + "'";
        //        }
        //        else if(psrh.ReportType == 2)
        //        {
        //            updateSQL = "update SMRNDetail set ServiceStatus=1 " +
        //               " where JobIDNo= " + psrh.jobIDNo +
        //              " and TemporaryNo=" + psrh.SMRNHeaderTempNo +
        //              " and TemporaryDate='" + psrh.SMRNHeaderTempDate.ToString("yyyy-MM-dd") + "'";
        //        }
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("update", "SMRNDetail", "", updateSQL) +
        //        Main.QueryDelimiter;

        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        status = false;
        //    }
        //    return status;
        //}
    }
}
