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
    class purchasereturnheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int PRNo { get; set; }
        public DateTime PRDate { get; set; }     
        public int MRNNo { get; set; }
        public DateTime MRNDate { get; set; }
      
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public double ProductValue { get; set; }
        public double TaxAmount { get; set; }
        public double PRValue { get; set; }
        public string Remarks { get; set; }
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
        public purchasereturnheader()
        {
            //Reference = "";
            Comments = "";
        }
    }
    class purchasereturndetail
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
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Tax { get; set; }
        public string TaxDetails { get; set; }
        public string BatchNo { get; set; }
        public string SerialNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string StoreLocationID { get; set; }
        public string StoreLocationName { get; set; }
        public int StockReferenceNo { get; set; }
        public string TaxCode { get; set; }
    }
    class PurchaseReturnHeaderDB
    {
        public List<purchasereturnheader> getFilteredPRHeader(string userList, int opt, string userCommentStatusString)
        {
            purchasereturnheader prh;
            List<purchasereturnheader> PRHeaders = new List<purchasereturnheader>();
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
                string query1 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,PurchaseReturnNo,PurchaseReturnDate," +
                    " MRNNo,MRNDate,CustomerID,CustomerName,ProductValue,TaxAmount,PurchaseReturnValue," +
                    " Remarks , CommentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName,"+
                    "ForwarderList,status,DocumentStatus " +
                    " from ViewPurchaseReturnHeader" +
                    " where ((ForwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (CreateUser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (CommentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) order by PurchaseReturnDate desc,DocumentID asc,PurchaseReturnNo desc";

                string query2 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,PurchaseReturnNo,PurchaseReturnDate," +
                    " MRNNo,MRNDate,CustomerID,CustomerName,ProductValue,TaxAmount,PurchaseReturnValue," +
                    " Remarks , CommentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName," +
                    "ForwarderList,status,DocumentStatus " +
                    " from ViewPurchaseReturnHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '"+Login.userLoggedIn+"')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc" ;

                string query3 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,PurchaseReturnNo,PurchaseReturnDate," +
                    " MRNNo,MRNDate,CustomerID,CustomerName,ProductValue,TaxAmount,PurchaseReturnValue," +
                    " Remarks , CommentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName," +
                    "ForwarderList,status,DocumentStatus " +
                    " from ViewPurchaseReturnHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'"+
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99)  order by PurchaseReturnDate desc,DocumentID asc,PurchaseReturnNo desc";

                string query6 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate,PurchaseReturnNo,PurchaseReturnDate," +
                    " MRNNo,MRNDate,CustomerID,CustomerName,ProductValue,TaxAmount,PurchaseReturnValue," +
                    " Remarks , CommentStatus,CreateUser,ForwardUser,ApproveUser,CreatorName,CreateTime,ForwarderName,ApproverName," +
                    "ForwarderList,status,DocumentStatus " +
                    " from ViewPurchaseReturnHeader" +
                    " where  DocumentStatus = 99  order by PurchaseReturnDate desc,DocumentID asc,PurchaseReturnNo desc";

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
                        prh = new purchasereturnheader();
                        prh.RowID = reader.GetInt32(0);
                        prh.DocumentID = reader.GetString(1);
                        prh.DocumentName = reader.GetString(2);
                        prh.TemporaryNo = reader.GetInt32(3);
                        prh.TemporaryDate = reader.GetDateTime(4);
                        prh.PRNo = reader.GetInt32(5);
                        prh.PRDate = reader.GetDateTime(6);
                        prh.MRNNo = reader.GetInt32(7);
                        prh.MRNDate = reader.GetDateTime(8);
                        prh.CustomerID = reader.GetString(9);
                        prh.CustomerName = reader.GetString(10);
                        prh.ProductValue = reader.GetDouble(11);
                        prh.TaxAmount = reader.GetDouble(12);
                        prh.PRValue = reader.GetDouble(13);
                        prh.Remarks = reader.GetString(14);
                        if (!reader.IsDBNull(15))
                        {
                            prh.CommentStatus = reader.GetString(15);
                        }
                        else
                        {
                            prh.CommentStatus = "";
                        }
                        prh.CreateUser = reader.GetString(16);
                        prh.ForwardUser = reader.GetString(17);
                        prh.ApproveUser = reader.GetString(18);
                        prh.CreatorName = reader.GetString(19);
                        prh.CreateTime = reader.GetDateTime(20);
                        prh.ForwarderName = reader.GetString(21);
                        prh.ApproverName = reader.GetString(22);

                        if (!reader.IsDBNull(23))
                        {
                            prh.ForwarderList = reader.GetString(23);
                        }
                        else
                        {
                            prh.ForwarderList = "";
                        }
                        prh.status = reader.GetInt32(24);
                        prh.DocumentStatus = reader.GetInt32(25);
                        PRHeaders.Add(prh);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Purchase return Header Details");
            }
            return PRHeaders;
        }



        public static List<purchasereturndetail> getPRDetail(purchasereturnheader prh)
        {
            purchasereturndetail prd;
            List<purchasereturndetail> PRDetail = new List<purchasereturndetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                 query = "select RowID,DocumentID,DocumentName,TemporaryNo, TemporaryDate,StockItemID,StockItemName,ModelNo,ModelName,Quantity, " +
                    "Price,Tax,TaxDetails,"+
                   "BatchNo,SerialNo,ExpiryDate,StoreLocationID,StoreLocationName,StockReferenceNo,TaxCode " +
                   "from ViewPurchaseReturnDetail " +
                   "where DocumentID='" + prh.DocumentID + "'" +
                   " and TemporaryNo=" + prh.TemporaryNo +
                   " and TemporaryDate='" + prh.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                   " order by StockItemID";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    prd = new purchasereturndetail();
                    prd.RowID = reader.GetInt32(0);
                    prd.DocumentID = reader.GetString(1);
                    prd.DocumentName = reader.GetString(2);
                    prd.TemporaryNo = reader.GetInt32(3);
                    prd.TemporaryDate = reader.GetDateTime(4).Date;
                    prd.StockItemID = reader.GetString(5);
                    prd.StockItemName = reader.GetString(6);
                    prd.ModelNo = reader.IsDBNull(7)?"NA":reader.GetString(7);
                    prd.ModelName = reader.IsDBNull(8) ? "NA" : reader.GetString(8);
                    prd.Quantity = reader.GetDouble(9);
                    prd.Price = reader.GetDouble(10);
                    prd.Tax = reader.GetDouble(11);
                    prd.TaxDetails = reader.GetString(12);
                    prd.BatchNo = reader.GetString(13);
                    prd.SerialNo = reader.GetString(14);
                    prd.ExpiryDate = reader.GetDateTime(15);
                    prd.StoreLocationID = reader.IsDBNull(16) ? "" : reader.GetString(16);
                    prd.StoreLocationName = reader.IsDBNull(17) ? "" : reader.GetString(17);
                    prd.StockReferenceNo = reader.GetInt32(18);
                    prd.TaxCode = reader.IsDBNull(19)?"":reader.GetString(19);
                    PRDetail.Add(prd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Purchase return Details");
            }
            return PRDetail ;
        }


        public Boolean updatePRHeader(purchasereturnheader prh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update PurchaseReturnHeader set CustomerID='" + prh.CustomerID +
                    "',MRNNo='" + prh.MRNNo +
                     "',MRNDate='" + prh.MRNDate.ToString("yyyy-MM-dd") +
                
                    "', ProductValue='" + prh.ProductValue +
                    "', TaxAmount='" + prh.TaxAmount +
                     "', PurchaseReturnValue='" + prh.PRValue +
                    "', Remarks='" + prh.Remarks +
                    "', Comments='" + prh.Comments +
                     "', CommentStatus='" + prh.CommentStatus +
                    "', ForwarderList='" + prh.ForwarderList + "'" +
                    " where DocumentID='" + prh.DocumentID + "'" +
                    " and TemporaryNo=" + prh.TemporaryNo +
                    " and TemporaryDate='" + prh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "PurchaseReturnHeader", "", updateSQL) +
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

        public Boolean updatePRDetail(List<purchasereturndetail> PRDetails, purchasereturnheader prh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Delete from PurchaseReturnDetail where DocumentID='" + prh.DocumentID + "'" +
                    " and TemporaryNo=" + prh.TemporaryNo +
                    " and TemporaryDate='" + prh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "PurchaseReturnDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (purchasereturndetail prd in PRDetails)
                {
                    updateSQL = "insert into PurchaseReturnDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,TaxCode,Quantity,Price,Tax,TaxDetails,BatchNo,SerialNo,ExpiryDate,StoreLocationID,StockReferenceNo) " +
                    "values ('" + prh.DocumentID + "'," +
                    prh.TemporaryNo + "," +
                    "'" + prh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + prd.StockItemID + "'," +
                   "'" + prd.ModelNo + "'," +
                    "'" + prd.TaxCode + "'," +
                   prd.Quantity + "," +
                   prd.Price + "," +
                   prd.Tax + "," +
                    "'" + prd.TaxDetails + "'," +
                    "'" + prd.BatchNo + "'," +
                   "'"+ prd.SerialNo + "'," +
                    "'" + prd.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + prd.StoreLocationID + "',"
                     +prd.StockReferenceNo + ")"; 
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "PurchaseReturnDetail", "", updateSQL) +
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

        public Boolean insertPRHeader(purchasereturnheader prh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into PurchaseReturnHeader " +
                    "(DocumentID,TemporaryNo,TemporaryDate,PurchaseReturnNo,PurchaseReturnDate,MRNNO,MRNDate," +
                    "CustomerID,ProductValue,TaxAmount,PurchaseReturnValue,Remarks," +
                    "Comments,CommentStatus,CreateUser,CreateTime,ForwarderList,DocumentStatus,Status)" +
                    " values (" +
                    "'" + prh.DocumentID + "'," +
                    prh.TemporaryNo + "," +
                    "'" + prh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     prh.PRNo + "," +
                    "'" + prh.PRDate.ToString("yyyy-MM-dd") + "'," +
                    prh.MRNNo + "," +
                    "'" + prh.MRNDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + prh.CustomerID + "'," +
                     prh.ProductValue + "," +
                      prh.TaxAmount + "," +
                       prh.PRValue + "," +
                    "'" + prh.Remarks + "'," +
                     "'" + prh.Comments + "'," +
                       "'" + prh.CommentStatus + "'," +
                    "'" + Login.userLoggedIn + "'," +
                    "GETDATE()" + "," +
                    "'" + prh.ForwarderList + "'," +
                    prh.DocumentStatus + "," +
                         prh.status  + ")";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "PurchaseReturnHeader", "", updateSQL) +
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
        public Boolean deletePRHeader(purchasereturnheader prh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "delete PurchaseReturnHeader where DocumentID='" + prh.DocumentID + "'" +
                    " and TemporaryNo=" + prh.TemporaryNo +
                    " and TemporaryDate='" + prh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("delete", "PurchaseReturnHeader", "", updateSQL) +
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

        public Boolean validatePRHeader(purchasereturnheader prh)
        {
            Boolean status = true;
            try
            {
                if (prh.DocumentID.Trim().Length == 0 || prh.DocumentID == null)
                {
                    return false;
                }

                if (prh.CustomerID == null || prh.CustomerID.Trim().Length == 0)
                {
                    return false;
                }
                if (prh.MRNNo == 0)
                {
                    return false;
                }
                if (prh.MRNDate == null)
                {
                    return false;
                }
                if (prh.ProductValue == 0)
                {
                    return false;
                }
                if (prh.PRValue == 0)
                {
                    return false;
                }
                if (prh.Remarks == null || prh.Remarks.Trim().Length == 0)
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
        public Boolean forwardPR(purchasereturnheader prh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update PurchaseReturnHeader set DocumentStatus=" + (prh.DocumentStatus + 1) +
                    ", forwardUser='" + prh.ForwardUser + "'" +
                    ", commentStatus='" + prh.CommentStatus + "'" +
                    ", ForwarderList='" + prh.ForwarderList + "'" +
                    " where DocumentID='" + prh.DocumentID + "'" +
                    " and TemporaryNo=" + prh.TemporaryNo +
                    " and TemporaryDate='" + prh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "PurchaseReturnHeader", "", updateSQL) +
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

        public Boolean reversePR(purchasereturnheader prh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update PurchaseReturnHeader set DocumentStatus=" + prh.DocumentStatus+
                    ", forwardUser='" + prh.ForwardUser + "'" +
                    ", commentStatus='" + prh.CommentStatus + "'" +
                    ", ForwarderList='" + prh.ForwarderList + "'" +
                    " where DocumentID='" + prh.DocumentID + "'" +
                    " and TemporaryNo=" + prh.TemporaryNo +
                    " and TemporaryDate='" + prh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "PurchaseReturnHeader", "", updateSQL) +
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

        public Boolean ApprovePR(purchasereturnheader prh, List<purchasereturndetail> PRDetails)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "";
               updateSQL = "update PurchaseReturnHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + prh.CommentStatus + "'" +
                    ", PurchaseReturnNo=" + prh.MRNNo +
                    ", PurchaseReturnDate=convert(date, getdate())" +
                    " where DocumentID='" + prh.DocumentID + "'" +
                    " and TemporaryNo=" + prh.TemporaryNo +
                    " and TemporaryDate='" + prh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "PurchaseReturnHeader", "", updateSQL) +
                Main.QueryDelimiter;

                if (prh.DocumentID == "PURCHASERETURNQA")
                {
                    foreach (purchasereturndetail prd in PRDetails)
                    {
                        double Quant = prd.Quantity;
                        int refNo = prd.StockReferenceNo;
                        updateSQL = "update Stock set  " +
                       " PresentStock=" + "( (select PresentStock from Stock where RowID = " + refNo + ")-" + Quant + ")" +
                       ", PurchaseReturnQuantity=" + "( (select PurchaseReturnQuantity from Stock where RowID = " + refNo + ")+" + Quant + ")" +
                       " where RowID=" + refNo;
                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                        ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
                        Main.QueryDelimiter;
                    }
                }
                else
                {
                    foreach (purchasereturndetail prd in PRDetails)
                    {
                        double Quant = prd.Quantity;
                        int refNo = prd.StockReferenceNo;
                        updateSQL = "update MRNDetail set  " +
                                " QuantityReturned=" + "( (select QuantityReturned from MRNDetail where RowID = " + refNo + ")+" + Quant + ")" +
                                " where RowID=" + refNo;
                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                        ActivityLogDB.PrepareActivityLogQquerString("update", "MRNDetail", "", updateSQL) +
                        Main.QueryDelimiter;
                    }
                }
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
        //public void updateRefNoWisePRDetailInStock(double Quant, int refNo)
        //{
        //    //Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "update Stock set  " +
        //            " PresentStock=" + "( (select PresentStock from Stock where RowID = " + refNo + ")-" + Quant + ")"+
        //            ", PurchaseReturnQuantity=" + "( (select PurchaseReturnQuantity from Stock where RowID = " + refNo + ")+" + Quant + ")" +
        //            " where RowID=" + refNo;
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
        //        Main.QueryDelimiter;

        //        if (!UpdateTable.UT(utString))
        //        {
        //            //status = false;
        //            MessageBox.Show("failed to Update In Reference Number Wise PRDetail in stock");
        //            return;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("failed to Update In Reference Number Wise PRDetail in stock");
        //        return;
        //    }
        //    //return status;
        //}
        //public Boolean updatePRInStock(List<purchasereturndetail> PRDetails)
        //{
        //    Boolean status = true;
        //   // string utString = "";
        //    try
        //    {
        //        foreach(purchasereturndetail prd in PRDetails)
        //        {
        //            double quant = prd.Quantity;
        //            int RefNo = prd.StockReferenceNo;
        //            updateRefNoWisePRDetailInStock(quant,RefNo);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = false;
        //    }
        //    return status;
        //}
        public static string getUserComments(string docid, int tempno, DateTime tempdate)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from PurchaseReturnHeader where DocumentID='" + docid + "'" +
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
        public Boolean updatePRHeaderAndDetail(purchasereturnheader prh, purchasereturnheader prevprh,List<purchasereturndetail> PRDetails)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update PurchaseReturnHeader set CustomerID='" + prh.CustomerID +
                     "',MRNNo='" + prh.MRNNo +
                      "',MRNDate='" + prh.MRNDate.ToString("yyyy-MM-dd") +

                     "', ProductValue='" + prh.ProductValue +
                     "', TaxAmount='" + prh.TaxAmount +
                      "', PurchaseReturnValue='" + prh.PRValue +
                     "', Remarks='" + prh.Remarks +
                     "', Comments='" + prh.Comments +
                      "', CommentStatus='" + prh.CommentStatus +
                     "', ForwarderList='" + prh.ForwarderList + "'" +
                     " where DocumentID='" + prevprh.DocumentID + "'" +
                     " and TemporaryNo=" + prevprh.TemporaryNo +
                     " and TemporaryDate='" + prevprh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "PurchaseReturnHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from PurchaseReturnDetail where DocumentID='" + prevprh.DocumentID + "'" +
                     " and TemporaryNo=" + prevprh.TemporaryNo +
                     " and TemporaryDate='" + prevprh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "PurchaseReturnDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (purchasereturndetail prd in PRDetails)
                {
                    updateSQL = "insert into PurchaseReturnDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,TaxCode,Quantity,Price,Tax,TaxDetails,BatchNo,SerialNo,ExpiryDate,StoreLocationID,StockReferenceNo) " +
                    "values ('" + prh.DocumentID + "'," +
                    prh.TemporaryNo + "," +
                    "'" + prh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + prd.StockItemID + "'," +
                   "'" + prd.ModelNo + "'," +
                    "'" + prd.TaxCode + "'," +
                   prd.Quantity + "," +
                   prd.Price + "," +
                   prd.Tax + "," +
                    "'" + prd.TaxDetails + "'," +
                    "'" + prd.BatchNo + "'," +
                   "'" + prd.SerialNo + "'," +
                    "'" + prd.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + prd.StoreLocationID + "',"
                     + prd.StockReferenceNo + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "PurchaseReturnDetail", "", updateSQL) +
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
        public Boolean InsertPRHeaderAndDetail(purchasereturnheader prh, List<purchasereturndetail> PRDetails)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                prh.TemporaryNo = DocumentNumberDB.getNumber(prh.DocumentID, 1);
                if (prh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + prh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + prh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into PurchaseReturnHeader " +
                     "(DocumentID,TemporaryNo,TemporaryDate,PurchaseReturnNo,PurchaseReturnDate,MRNNO,MRNDate," +
                     "CustomerID,ProductValue,TaxAmount,PurchaseReturnValue,Remarks," +
                     "Comments,CommentStatus,CreateUser,CreateTime,ForwarderList,DocumentStatus,Status)" +
                     " values (" +
                     "'" + prh.DocumentID + "'," +
                     prh.TemporaryNo + "," +
                     "'" + prh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                      prh.PRNo + "," +
                     "'" + prh.PRDate.ToString("yyyy-MM-dd") + "'," +
                     prh.MRNNo + "," +
                     "'" + prh.MRNDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + prh.CustomerID + "'," +
                      prh.ProductValue + "," +
                       prh.TaxAmount + "," +
                        prh.PRValue + "," +
                     "'" + prh.Remarks + "'," +
                      "'" + prh.Comments + "'," +
                        "'" + prh.CommentStatus + "'," +
                     "'" + Login.userLoggedIn + "'," +
                     "GETDATE()" + "," +
                     "'" + prh.ForwarderList + "'," +
                     prh.DocumentStatus + "," +
                          prh.status + ")";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "PurchaseReturnHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from PurchaseReturnDetail where DocumentID='" + prh.DocumentID + "'" +
                     " and TemporaryNo=" + prh.TemporaryNo +
                     " and TemporaryDate='" + prh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "PurchaseReturnDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (purchasereturndetail prd in PRDetails)
                {
                    updateSQL = "insert into PurchaseReturnDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,TaxCode,Quantity,Price,Tax,TaxDetails,BatchNo,SerialNo,ExpiryDate,StoreLocationID,StockReferenceNo) " +
                    "values ('" + prh.DocumentID + "'," +
                    prh.TemporaryNo + "," +
                    "'" + prh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + prd.StockItemID + "'," +
                   "'" + prd.ModelNo + "'," +
                    "'" + prd.TaxCode + "'," +
                   prd.Quantity + "," +
                   prd.Price + "," +
                   prd.Tax + "," +
                    "'" + prd.TaxDetails + "'," +
                    "'" + prd.BatchNo + "'," +
                   "'" + prd.SerialNo + "'," +
                    "'" + prd.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + prd.StoreLocationID + "',"
                     + prd.StockReferenceNo + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "PurchaseReturnDetail", "", updateSQL) +
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
