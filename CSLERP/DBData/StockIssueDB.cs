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
    class stockissueheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public int IssueType { get; set; }
        public int ReferenceNo { get; set; }
        public DateTime ReferenceDate { get; set; }
        public string ToLocation { get; set; }
        public string ToLocationName { get; set; }
        public string Remarks { get; set; }
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
    }
    class stockissuedetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public double IssueQuantity { get; set; }
        public double UsedQuantity { get; set; }
        public double DamagedQuantity { get; set; }
        public double ReturnedQuantity { get; set; }
        public int MRNNo { get; set; }
        public DateTime MRNDate { get; set; }
        public double PurchaseQuantity { get; set; }
        public double PurchasePrice { get; set; }
        public double PurchaseTax { get; set; }
        public string SupplierID { get; set; }
        public string BatchNo { get; set; }
        public string SerialNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string SupplierName { get; set; }
        public int StockReferenceNo { get; set; }
    }
    class StockIssueDB
    {
        public List<stockissueheader> getFilteredSIHeader(string userList, int opt)
        {
            stockissueheader sih;
            List<stockissueheader> SIHeaders = new List<stockissueheader>();
            try
            {
                ////approved user comment status string
                //string acStr="";
                //try
                //{
                //    acStr = userCommentStatusString.Substring(0, userCommentStatusString.Length - 2) + "1" + Main.delimiter2;
                //}
                //catch (Exception ex)
                //{
                //    acStr = "";
                //}
                ////-----
                string query1 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate," +
                    " IssueType,ReferenceNo,ReferenceDate,ToLocation,ToLocationName," +
                    " Remarks ,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,"+
                    "ForwarderList,status,DocumentStatus " +
                    " from ViewStockIssueHeader" +
                    " where ((ForwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (CreateUser='" + Login.userLoggedIn + "' and DocumentStatus=1)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc ";

                string query2 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate," +
                    " IssueType,ReferenceNo,ReferenceDate,ToLocation,ToLocationName," +
                    " Remarks ,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName," +
                    "ForwarderList,status,DocumentStatus " +
                    " from ViewStockIssueHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '"+Login.userLoggedIn+"'))"+
                    " order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query3 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate," +
                    " IssueType,ReferenceNo,ReferenceDate,ToLocation,ToLocationName," +
                    " Remarks ,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName," +
                    "ForwarderList,status,DocumentStatus " +
                    " from ViewStockIssueHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99 and status  = 1) order by DocumentDate desc,DocumentID asc,DocumentNo desc";

                string query6 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate," +
                    " IssueType,ReferenceNo,ReferenceDate,ToLocation,ToLocationName," +
                    " Remarks ,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName," +
                    "ForwarderList,status,DocumentStatus " +
                     " from ViewStockIssueHeader" +
                    " where  DocumentStatus = 99 and status  = 1 order by DocumentDate desc,DocumentID asc,DocumentNo desc";

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
                        sih = new stockissueheader();
                        sih.RowID = reader.GetInt32(0);
                        sih.DocumentID = reader.GetString(1);
                        sih.DocumentName = reader.GetString(2);
                        sih.TemporaryNo = reader.GetInt32(3);
                        sih.TemporaryDate = reader.GetDateTime(4);
                        sih.DocumentNo = reader.GetInt32(5);
                        sih.DocumentDate = reader.GetDateTime(6);
                        sih.IssueType = reader.GetInt32(7);
                        sih.ReferenceNo = reader.GetInt32(8);
                        sih.ReferenceDate = reader.GetDateTime(9);
                        sih.ToLocation = reader.GetString(10);
                        sih.ToLocationName = reader.GetString(11);
                        sih.Remarks = reader.GetString(12);
                        sih.CreateUser = reader.GetString(13);
                        sih.ForwardUser = reader.GetString(14);
                        sih.ApproveUser = reader.GetString(15);
                        sih.CreatorName = reader.GetString(16);
                        sih.CreateTime = reader.GetDateTime(17);
                        sih.ForwarderName = reader.GetString(18);
                        sih.ApproverName = reader.GetString(19);

                        if (!reader.IsDBNull(20))
                        {
                            sih.ForwarderList = reader.GetString(20);
                        }
                        else
                        {
                            sih.ForwarderList = "";
                        }
                        sih.status = reader.GetInt32(21);
                        sih.DocumentStatus = reader.GetInt32(22);
                        SIHeaders.Add(sih);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying StockIssue Header Details");
            }
            return SIHeaders;
        }



        public static List<stockissuedetail> getPRDetail(stockissueheader sih)
        {
            stockissuedetail sid;
            List<stockissuedetail> SIDetail = new List<stockissuedetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                 query = "select RowID,DocumentID,DocumentName,TemporaryNo, TemporaryDate,StockItemID,StockItemName,ModelNo,ModelName,IssueQuantity, " +
                    "UsedQuantity,DamagedQuantity,ReturnedQuantity,"+
                   "MRNNo,MRNDate,BatchNo,SerialNo,ExpiryDate,PurchaseQuantity,PurchasePrice,PurchaseTax,SupplierID,SupplierName,StockReferenceNo " +
                   "from ViewStockIssueDetail " +
                   "where DocumentID='" + sih.DocumentID + "'" +
                   " and TemporaryNo=" + sih.TemporaryNo +
                   " and TemporaryDate='" + sih.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                   " order by StockItemID";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sid = new stockissuedetail();
                    sid.RowID = reader.GetInt32(0);
                    sid.DocumentID = reader.GetString(1);
                    sid.DocumentName = reader.GetString(2);
                    sid.TemporaryNo = reader.GetInt32(3);
                    sid.TemporaryDate = reader.GetDateTime(4).Date;
                    sid.StockItemID = reader.GetString(5);
                    sid.StockItemName = reader.GetString(6);
                    sid.ModelNo = reader.IsDBNull(7)?"NA":reader.GetString(7);
                    sid.ModelName = reader.IsDBNull(8) ? "NA" : reader.GetString(8);
                    sid.IssueQuantity = reader.GetDouble(9);
                    sid.UsedQuantity = reader.GetDouble(10);
                    sid.DamagedQuantity = reader.GetDouble(11);
                    sid.ReturnedQuantity = reader.GetDouble(12);
                    sid.MRNNo = reader.GetInt32(13);
                    sid.MRNDate = reader.GetDateTime(14).Date;
                    sid.BatchNo = reader.IsDBNull(15) ? "" : reader.GetString(15);
                    sid.SerialNo = reader.IsDBNull(16) ? "" : reader.GetString(16);
                    sid.ExpiryDate = reader.IsDBNull(17) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(17);
                    sid.PurchaseQuantity = reader.GetDouble(18);
                    sid.PurchasePrice = reader.GetDouble(19);
                    sid.PurchaseTax = reader.GetDouble(20);
                    sid.SupplierID = reader.IsDBNull(21) ? "" : reader.GetString(21);
                    sid.SupplierName = reader.IsDBNull(22) ? "" : reader.GetString(22);
                    sid.StockReferenceNo = reader.GetInt32(23);
                    SIDetail.Add(sid);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying MRN Details");
            }
            return SIDetail ;
        }
        public Boolean validateSIHeader(stockissueheader sih)
        {
            Boolean status = true;
            try
            {
                if (sih.DocumentID.Trim().Length == 0 || sih.DocumentID == null)
                {
                    return false;
                }
                if (sih.IssueType == 0)
                {
                    return false;
                }
                if (sih.ReferenceNo == 0)
                {
                    return false;
                }
                if (sih.ReferenceDate == null)
                {
                    return false;
                }
                if (sih.ToLocation.Trim().Length == 0 || sih.ToLocation == null)
                { 
                    return false;
                }
                if (sih.Remarks.Trim().Length == 0 || sih.Remarks == null)
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
        public Boolean forwardPR(stockissueheader sih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockIssueHeader set DocumentStatus=" + (sih.DocumentStatus + 1) +
                    ", forwardUser='" + sih.ForwardUser + "'" +
                    ", ForwarderList='" + sih.ForwarderList + "'" +
                    " where DocumentID='" + sih.DocumentID + "'" +
                    " and TemporaryNo=" + sih.TemporaryNo +
                    " and TemporaryDate='" + sih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "StockIssueHeader", "", updateSQL) +
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

        public Boolean reversePR(stockissueheader sih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockIssueHeader set DocumentStatus=" + sih.DocumentStatus+
                    ", forwardUser='" + sih.ForwardUser + "'" +
                    ", ForwarderList='" + sih.ForwarderList + "'" +
                    " where DocumentID='" + sih.DocumentID + "'" +
                    " and TemporaryNo=" + sih.TemporaryNo +
                    " and TemporaryDate='" + sih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "StockIssueHeader", "", updateSQL) +
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

        public Boolean ApprovePR(stockissueheader sih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockIssueHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", DocumentNo=" + sih.DocumentNo +
                    ", DocumentDate=convert(date, getdate())" +
                    " where DocumentID='" + sih.DocumentID + "'" +
                    " and TemporaryNo=" + sih.TemporaryNo +
                    " and TemporaryDate='" + sih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "StockIssueHeader", "", updateSQL) +
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
        public void updateRefNoWiseSIDetailInStock(double Quant, int refNo)
        {
            //Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update Stock set  " +
                    " PresentStock=" + "( (select PresentStock from Stock where RowID = " + refNo + ")-" + Quant + ")"+
                    " where RowID=" + refNo;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
                Main.QueryDelimiter;

                if (!UpdateTable.UT(utString))
                {
                    //status = false;
                    MessageBox.Show("failed to Update In Reference Number Wise SIDetail in stock");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed to Update In Reference Number Wise SIDetail in stock");
                return;
            }
            //return status;
        }
        public Boolean updateSIInStock(List<stockissuedetail> SIDetails)
        {
            Boolean status = true;
           // string utString = "";
            try
            {
                foreach(stockissuedetail sid in SIDetails)
                {
                    double quant = sid.IssueQuantity;
                    int RefNo = sid.StockReferenceNo;
                    updateRefNoWiseSIDetailInStock(quant,RefNo);
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        public Boolean updateStockIssueUsage(List<stockissuedetail> sidDetails)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "";
                foreach (stockissuedetail sid in sidDetails)
                {
                    updateSQL = "update StockIssueDetail set " +
                   " UsedQuantity=" + sid.UsedQuantity +
                   ", DamagedQuantity=" + sid.DamagedQuantity +
                    ", ReturnedQuantity=" + sid.ReturnedQuantity +
                    " where RowID=" + sid.RowID +
                   " and TemporaryNo=" + sid.TemporaryNo +
                   " and TemporaryDate='" + sid.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "StockIssueDetail", "", updateSQL) +
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
            }
            return status;
        }
        public Boolean updateSIHeaderAndDetail(stockissueheader sih, stockissueheader prevsih,List<stockissuedetail> SIDetails)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockIssueHeader set IssueType='" + sih.IssueType +
                    "',ReferenceNo='" + sih.ReferenceNo +
                     "',ReferenceDate='" + sih.ReferenceDate.ToString("yyyy-MM-dd") +
                      "', ToLocation='" + sih.ToLocation +
                    "', Remarks='" + sih.Remarks +
                    "', ForwarderList='" + sih.ForwarderList + "'" +
                    " where DocumentID='" + prevsih.DocumentID + "'" +
                    " and TemporaryNo=" + prevsih.TemporaryNo +
                    " and TemporaryDate='" + prevsih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "StockIssueHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from StockIssueDetail where DocumentID='" + prevsih.DocumentID + "'" +
                      " and TemporaryNo=" + prevsih.TemporaryNo +
                      " and TemporaryDate='" + prevsih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "StockIssueDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (stockissuedetail sid in SIDetails)
                {
                    updateSQL = "insert into StockIssueDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,IssueQuantity,MRNNo,MRNDate,BatchNo,ExpiryDate,SerialNo,PurchaseQuantity," +
                    "PurchasePrice,PurchaseTax,SupplierID,StockReferenceNo) " +
                    "values ('" + sih.DocumentID + "'," +
                    sih.TemporaryNo + "," +
                    "'" + sih.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + sid.StockItemID + "'," +
                   "'" + sid.ModelNo + "'," +
                   sid.IssueQuantity + "," +
                   sid.MRNNo + "," +
                     "'" + sid.MRNDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + sid.BatchNo + "'," +
                   "'" + sid.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                   "'" + sid.SerialNo + "'," +
                   sid.PurchaseQuantity + "," +
                   sid.PurchasePrice + "," +
                   sid.PurchaseTax + "," +
                     "'" + sid.SupplierID + "'," +
                     +sid.StockReferenceNo + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "StockIssueDetail", "", updateSQL) +
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
        public Boolean InsertSIHeaderAndDetail(stockissueheader sih, List<stockissuedetail> SIDetails)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                sih.TemporaryNo = DocumentNumberDB.getNumber(sih.DocumentID, 1);
                if (sih.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + sih.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + sih.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into StockIssueHeader " +
                   "(DocumentID,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate,IssueType,ReferenceNo," +
                   "ReferenceDate,ToLocation,Remarks" +
                   ",CreateUser,CreateTime,ForwarderList,DocumentStatus,Status)" +
                   " values (" +
                   "'" + sih.DocumentID + "'," +
                   sih.TemporaryNo + "," +
                   "'" + sih.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    sih.DocumentNo + "," +
                   "'" + sih.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                   sih.IssueType + "," +
                    sih.ReferenceNo + "," +
                   "'" + sih.ReferenceDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + sih.ToLocation + "'," +
                   "'" + sih.Remarks + "'," +
                   "'" + Login.userLoggedIn + "'," +
                   "GETDATE()" + "," +
                   "'" + sih.ForwarderList + "'," +
                   sih.DocumentStatus + "," +
                        sih.status + ")";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "StockIssueHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from StockIssueDetail where DocumentID='" + sih.DocumentID + "'" +
                                    " and TemporaryNo=" + sih.TemporaryNo +
                                    " and TemporaryDate='" + sih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "StockIssueDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (stockissuedetail sid in SIDetails)
                {
                    updateSQL = "insert into StockIssueDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,IssueQuantity,MRNNo,MRNDate,BatchNo,ExpiryDate,SerialNo,PurchaseQuantity," +
                    "PurchasePrice,PurchaseTax,SupplierID,StockReferenceNo) " +
                    "values ('" + sih.DocumentID + "'," +
                    sih.TemporaryNo + "," +
                    "'" + sih.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + sid.StockItemID + "'," +
                   "'" + sid.ModelNo + "'," +
                   sid.IssueQuantity + "," +
                   sid.MRNNo + "," +
                     "'" + sid.MRNDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + sid.BatchNo + "'," +
                   "'" + sid.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                   "'" + sid.SerialNo + "'," +
                   sid.PurchaseQuantity + "," +
                   sid.PurchasePrice + "," +
                   sid.PurchaseTax + "," +
                     "'" + sid.SupplierID + "'," +
                     +sid.StockReferenceNo + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "StockIssueDetail", "", updateSQL) +
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
        public List<stockissueheader> getFilteredSIHeaderForRawMaterial(int prodPlanNo, DateTime planDate)
        {
            stockissueheader sih;
            List<stockissueheader> SIHeaders = new List<stockissueheader>();
            try
            {
                string query = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate," +
                    " IssueType,ReferenceNo,ReferenceDate,ToLocation,ToLocationName," +
                    " Remarks ,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName," +
                    "ForwarderList,status,DocumentStatus " +
                     " from ViewStockIssueHeader" +
                    " where ReferenceNo = " +prodPlanNo +
                    " and ReferenceDate = '" + planDate.ToString("yyyy-MM-dd") +"' and IssueType = 1 and"+
                    " DocumentStatus = 99 and status  = 1";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        sih = new stockissueheader();
                        sih.RowID = reader.GetInt32(0);
                        sih.DocumentID = reader.GetString(1);
                        sih.DocumentName = reader.GetString(2);
                        sih.TemporaryNo = reader.GetInt32(3);
                        sih.TemporaryDate = reader.GetDateTime(4);
                        sih.DocumentNo = reader.GetInt32(5);
                        sih.DocumentDate = reader.GetDateTime(6);
                        sih.IssueType = reader.GetInt32(7);
                        sih.ReferenceNo = reader.GetInt32(8);
                        sih.ReferenceDate = reader.GetDateTime(9);
                        sih.ToLocation = reader.GetString(10);
                        sih.ToLocationName = reader.GetString(11);
                        sih.Remarks = reader.GetString(12);
                        sih.CreateUser = reader.GetString(13);
                        sih.ForwardUser = reader.GetString(14);
                        sih.ApproveUser = reader.GetString(15);
                        sih.CreatorName = reader.GetString(16);
                        sih.CreateTime = reader.GetDateTime(17);
                        sih.ForwarderName = reader.GetString(18);
                        sih.ApproverName = reader.GetString(19);

                        if (!reader.IsDBNull(20))
                        {
                            sih.ForwarderList = reader.GetString(20);
                        }
                        else
                        {
                            sih.ForwarderList = "";
                        }
                        sih.status = reader.GetInt32(21);
                        sih.DocumentStatus = reader.GetInt32(22);
                        SIHeaders.Add(sih);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying StockIssue Header Details");
            }
            return SIHeaders;
        }
    }
}
