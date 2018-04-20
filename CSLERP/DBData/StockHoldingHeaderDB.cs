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
    class stockholdingheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public string StoreLocationID { get; set; }
        public string StoreLocationName { get; set; }
        public string Remarks { get; set; }
        public string Comments { get; set; }
        public string CommentStatus { get; set; }
        public string CreateUser { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreateTime { get; set; }
        public string ForwarderName { get; set; }
        public string ApproverName { get; set; }
        public string ForwarderList { get; set; }
        public int Status { get; set; }
        public int DocumentStatus { get; set; }
        public stockholdingheader()
        {
            //Reference = "";
            Comments = "";
        }
    }
    class stockholdingdetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string StockItemID { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string StockItemName { get; set; }
        public double Quantity { get; set; }
        public string InwardDocumentID { get; set; }
        public string InwardDocumentNo { get; set; }
        public DateTime InwardDocumentDate { get; set; }
        public int StockReferenceNo { get; set; }

    }
    class StockHoldingHeaderDB
    {
        public List<stockholdingheader> getFilteredStockHoldingHeader(string userList, int opt, string userCommentStatusString)
        {
            stockholdingheader shh;
            List<stockholdingheader> StockHoldingHeaderList = new List<stockholdingheader>();
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
                    " StoreLocationID,StoreLocationName,Remarks,Comments," +
                    " CommentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,Status,DocumentStatus " +
                    " from ViewStockHoldingHeader" +
                    " where ((ForwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (CreateUser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (CommentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) order by DocumentDate desc,DocumentID asc,DocumentNo desc";

                string query2 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate," +
                    " StoreLocationID,StoreLocationName,Remarks,Comments," +
                    " CommentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,Status,DocumentStatus " +
                    " from ViewStockHoldingHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query3 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate," +
                    " StoreLocationID,StoreLocationName,Remarks,Comments," +
                    " CommentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,Status,DocumentStatus " +
                    " from ViewStockHoldingHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99)  order by DocumentDate desc,DocumentID asc,DocumentNo desc";

                string query6 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate," +
                    " StoreLocationID,StoreLocationName,Remarks,Comments," +
                    " CommentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,ForwarderList,Status,DocumentStatus " +
                    " from ViewStockHoldingHeader" +
                    " where  DocumentStatus = 99  order by DocumentDate desc,DocumentID asc,DocumentNo desc";

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
                        shh = new stockholdingheader();
                        shh.RowID = reader.GetInt32(0);
                        shh.DocumentID = reader.GetString(1);
                        shh.DocumentName = reader.GetString(2);
                        shh.TemporaryNo = reader.GetInt32(3);
                        shh.TemporaryDate = reader.GetDateTime(4);
                        shh.DocumentNo = reader.GetInt32(5);
                        shh.DocumentDate = reader.GetDateTime(6);
                        shh.StoreLocationID = reader.GetString(7);
                        shh.StoreLocationName = reader.GetString(8);
                        shh.Remarks = reader.GetString(9);
                        shh.Comments = reader.GetString(10);
                        if (!reader.IsDBNull(11))
                        {
                            shh.CommentStatus = reader.GetString(11);
                        }
                        else
                        {
                            shh.CommentStatus = "";
                        }
                        shh.CreateUser = reader.GetString(12);
                        shh.ForwardUser = reader.GetString(13);
                        shh.ApproveUser = reader.GetString(14);
                        shh.CreatorName = reader.GetString(15);
                        shh.CreateTime = reader.GetDateTime(16);
                        shh.ForwarderName = reader.GetString(17);
                        shh.ApproverName = reader.GetString(18);

                        if (!reader.IsDBNull(19))
                        {
                            shh.ForwarderList = reader.GetString(19);
                        }
                        else
                        {
                            shh.ForwarderList = "";
                        }
                        shh.Status = reader.GetInt32(20);
                        shh.DocumentStatus = reader.GetInt32(21);
 
                        StockHoldingHeaderList.Add(shh);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Header Details");
            }
            return StockHoldingHeaderList;
        }



        public static List<stockholdingdetail> getStockHoldingHeaderDetail(stockholdingheader shh)
        {
            stockholdingdetail shd;
            List<stockholdingdetail> StockHoldingHeaderDetailList = new List<stockholdingdetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,DocumentName,TemporaryNo,TemporaryDate,StockItemID,StockItemName,ModelNo,ModelName,Quantity, " +
                   "InwardDocumentID,InwardDocumentNo,InwardDocumentDate,StockReferenceNo " + 
                  "from ViewStockHoldingDetail " +
                  " where DocumentID='" + shh.DocumentID + "'" +
                  " and TemporaryNo=" + shh.TemporaryNo +
                  " and TemporaryDate='" + shh.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                  " order by StockItemID";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    shd = new stockholdingdetail();
                    shd.RowID = reader.GetInt32(0);
                    shd.DocumentID = reader.GetString(1);
                    shd.DocumentName = reader.GetString(2);
                    shd.TemporaryNo = reader.GetInt32(3);
                    shd.TemporaryDate = reader.GetDateTime(4).Date;
                    shd.StockItemID = reader.GetString(5);
                    shd.StockItemName = reader.GetString(6);
                    shd.ModelNo = reader.IsDBNull(7)?"NA":reader.GetString(7);
                    shd.ModelName = reader.IsDBNull(8) ? "NA" : reader.GetString(8);
                    shd.Quantity = reader.GetDouble(9);
                    shd.InwardDocumentID = reader.GetString(10);
                    shd.InwardDocumentNo = reader.GetString(11);
                    shd.InwardDocumentDate = reader.GetDateTime(12);
                    shd.StockReferenceNo = reader.GetInt32(13);
                    StockHoldingHeaderDetailList.Add(shd);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Stock Header Details");
            }
            return StockHoldingHeaderDetailList;
        }
        public Boolean validateStockHoldingHeader(stockholdingheader shh)
        {
            Boolean status = true;
            try
            {
                if (shh.StoreLocationID.Trim().Length == 0 || shh.StoreLocationID == null)
                {
                    return false;
                }

                if (shh.Remarks.Trim().Length == 0 || shh.Remarks == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public Boolean forwardStockHoldingHeader(stockholdingheader shh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockHoldingHeader set DocumentStatus=" + (shh.DocumentStatus + 1) +
                    ", forwardUser='" + shh.ForwardUser + "'" +
                    ", commentStatus='" + shh.CommentStatus + "'" +
                    ", ForwarderList='" + shh.ForwarderList + "'" +
                   // ", QCStatus= " + imh.QCStatus +
                    " where DocumentID='" + shh.DocumentID + "'" +
                    " and TemporaryNo=" + shh.TemporaryNo +
                    " and TemporaryDate='" + shh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "StockHoldingHeader", "", updateSQL) +
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

        public Boolean reverseStockHoldingHeader(stockholdingheader shh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockHoldingHeader set DocumentStatus=" + shh.DocumentStatus +
                   // ",QCStatus=" + mrnh.QCStatus +
                    ", forwardUser='" + shh.ForwardUser + "'" +
                    ", commentStatus='" + shh.CommentStatus + "'" +
                    ", ForwarderList='" + shh.ForwarderList + "'" +
                    " where DocumentID='" + shh.DocumentID + "'" +
                    " and TemporaryNo=" + shh.TemporaryNo +
                    " and TemporaryDate='" + shh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "StockHoldingHeader", "", updateSQL) +
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

        public Boolean ApproveStockHoldingHeader(stockholdingheader shh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockHoldingHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + shh.CommentStatus + "'" +
                    ", DocumentNo=" + shh.DocumentNo +
                    ", DocumentDate=convert(date, getdate())" +
                    " where DocumentID='" + shh.DocumentID + "'" +
                    " and TemporaryNo=" + shh.TemporaryNo +
                    " and TemporaryDate='" + shh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "StockHoldingHeader", "", updateSQL) +
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
                string query = "select comments from StockHoldingHeader where DocumentID='" + docid + "'" +
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
        //public static mrnheader getMRNNoAndDate(int tempNo, DateTime tempDate)
        //{
        //    mrnheader mrnh = new mrnheader();
        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "select MRNNo, MRNDate from MRNHeader " +
        //                " where TemporaryNo=" + tempNo +
        //                " and TemporaryDate='" + tempDate.ToString("yyyy-MM-dd") + "'";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            mrnh.MRNNo = reader.GetInt32(0);
        //            mrnh.MRNDate = reader.GetDateTime(1);
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return mrnh;
        //}
        public void updateRefNoWiseSHDetailInStock(double Quant, int refNo)
        {
            //Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update Stock set  " +
                    " PresentStock=" + "( (select PresentStock from Stock where RowID = " + refNo + ")-" + Quant + ")" +
                    ", StockOnHold=" + "( (select StockONHold from Stock where RowID = " + refNo + ")+" + Quant + ")" +
                    " where RowID=" + refNo;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
                Main.QueryDelimiter;

                if (!UpdateTable.UT(utString))
                {
                    //status = false;
                    MessageBox.Show("failed to Update In Reference Number Wise SHDetail in stock");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("failed to Update In Reference Number Wise SHDetail in stock");
                return;
            }
            //return status;
        }
        public Boolean updatePRInStock(List<stockholdingdetail> SHDetails)
        {
            Boolean status = true;
            // string utString = "";
            try
            {
                foreach (stockholdingdetail shd in SHDetails)
                {
                    double quant = shd.Quantity;
                    int RefNo = shd.StockReferenceNo;
                    updateRefNoWiseSHDetailInStock(quant, RefNo);
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        public Boolean updateSHHeaderAndDetail(stockholdingheader shh, stockholdingheader prevshh,List<stockholdingdetail> StockHoldingHeaderDetails)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockHoldingHeader set TemporaryNo='" + shh.TemporaryNo +
                    "',TemporaryDate='" + shh.TemporaryDate.ToString("yyyy-MM-dd") +
                     "',DocumentNo='" + shh.DocumentNo +
                     "',DocumentDate='" + shh.DocumentDate.ToString("yyyy-MM-dd") +
                    "', StoreLocationID='" + shh.StoreLocationID +
                      "', Remarks='" + shh.Remarks +
                    "', Comments='" + shh.Comments +
                    "', CommentStatus='" + shh.CommentStatus +
                     "', ForwarderList='" + shh.ForwarderList + "'" +
                    " where DocumentID='" + prevshh.DocumentID + "'" +
                    " and TemporaryNo=" + prevshh.TemporaryNo +
                    " and TemporaryDate='" + prevshh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "StockHoldingHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from StockHoldingDetail where DocumentID='" + prevshh.DocumentID + "'" +
                     " and TemporaryNo=" + prevshh.TemporaryNo +
                     " and TemporaryDate='" + prevshh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "StockHoldingDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (stockholdingdetail shd in StockHoldingHeaderDetails)
                {
                    updateSQL = "insert into StockHoldingDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,Quantity,InwardDocumentID,InwardDocumentNo,InwardDocumentdate,StockReferenceNo) " +
                    "values ('" + shd.DocumentID + "'," +
                    shd.TemporaryNo + "," +
                    "'" + shd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + shd.StockItemID + "'," +
                       "'" + shd.ModelNo + "'," +
                   shd.Quantity + "," +
                  "'" + shd.InwardDocumentID + "'," +
                   shd.InwardDocumentNo + "," +
                    "'" + shd.InwardDocumentDate.ToString("yyyy-MM-dd") + "',"
                    + shd.StockReferenceNo + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "StockHoldingDetail", "", updateSQL) +
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
        public Boolean InsertSHHeaderAndDetail(stockholdingheader shh, List<stockholdingdetail> StockHoldingHeaderDetails)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                shh.TemporaryNo = DocumentNumberDB.getNumber(shh.DocumentID, 1);
                if (shh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + shh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + shh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into StockHoldingHeader " +
                      "(DocumentID,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate," +
                      "StoreLocationID,Remarks,Comments,CommentStatus,CreateUser," +
                      "CreateTime,ForwarderList,Status,DocumentStatus)" +
                      " values (" +
                      "'" + shh.DocumentID + "'," +
                      shh.TemporaryNo + "," +
                      "'" + shh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     shh.DocumentNo + "," +
                      "'" + shh.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                      "'" + shh.StoreLocationID + "'," +
                       "'" + shh.Remarks + "'," +
                      "'" + shh.Comments + "'," +
                       "'" + shh.CommentStatus + "'," +
                         "'" + Login.userLoggedIn + "'," +
                        "GETDATE()" + "," +
                      "'" + shh.ForwarderList + "'," +
                       +shh.Status + "," +
                    +shh.DocumentStatus + ")";


                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "StockHoldingHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from StockHoldingDetail where DocumentID='" + shh.DocumentID + "'" +
                      " and TemporaryNo=" + shh.TemporaryNo +
                      " and TemporaryDate='" + shh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "StockHoldingDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (stockholdingdetail shd in StockHoldingHeaderDetails)
                {
                    updateSQL = "insert into StockHoldingDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,Quantity,InwardDocumentID,InwardDocumentNo,InwardDocumentdate,StockReferenceNo) " +
                    "values ('" + shd.DocumentID + "'," +
                    shh.TemporaryNo + "," +
                    "'" + shd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + shd.StockItemID + "'," +
                       "'" + shd.ModelNo + "'," +
                   shd.Quantity + "," +
                  "'" + shd.InwardDocumentID + "'," +
                   shd.InwardDocumentNo + "," +
                    "'" + shd.InwardDocumentDate.ToString("yyyy-MM-dd") + "',"
                    + shd.StockReferenceNo + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "StockHoldingDetail", "", updateSQL) +
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
