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
    class gtnheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int GTNNo { get; set; }
        public DateTime GTNDate { get; set; }
        public string FromLocation { get; set; }
        public string FromLocationName { get; set; }
        public string ToLocation { get; set; }
        public string ToLocationName { get; set; }
        public string Remarks { get; set; }
        public string Comments { get; set; }
        public string CommentStatus { get; set; }
        public DateTime AcceptDate { get; set; }
        public string AcceptUser { get; set; }
        public int AcceptStatus { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string ForwarderList { get; set; }
        public int Status { get; set; }
        public int DocumentStatus { get; set; }
        public int ReceiveStatus { get; set; }
        public decimal MaterialValue { get; set; }
        public gtnheader()
        {
           
        }
    }
    class gtndetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string StockitemID { get; set; }
        public string StockItemName { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public double Quantity { get; set; }
        public int MRNNo { get; set; }
        public DateTime MRNDate { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double PurchaseQuantity { get; set; }
        public double PurchasePrice { get; set; }
        public double PurchaseTax { get; set; }
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string SerialNo { get; set; }
        public int refNo { get; set; }
        public gtndetail()
        {

        }
    }
    class GTNDB
    {
        public List<gtnheader> getFilteredGTNHeader(string userList, int opt, string userCommentStatusString)
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
            gtnheader gheader;
            List<gtnheader> gheaderlist = new List<gtnheader>();
            try
            {
                string query1 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " GTNNo,GTNDate,FromLocation,FromLocationName,ToLocation,ToLocationName,Remarks,Comments,Commentstatus,AcceptanceDate,AcceptedUser,AcceptanceStatus," +
                    "CreateUser,CreateTime,ForwardUser,ApproveUser,ForwarderList,Status,DocumentStatus,ReceiveStatus, MaterialValue " +
                    " from ViewGTNHeader" +
                   " where ((forwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (commentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98))  and Status not in (7,98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query2 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " GTNNo,GTNDate,FromLocation,FromLocationName,ToLocation,ToLocationName,Remarks,Comments,Commentstatus,AcceptanceDate,AcceptedUser,AcceptanceStatus," +
                    "CreateUser,CreateTime,ForwardUser,ApproveUser,ForwarderList,Status,DocumentStatus,ReceiveStatus, MaterialValue " +
                    " from ViewGTNHeader" +
                   " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98))  and Status not in (7,98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query3 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " GTNNo,GTNDate,FromLocation,FromLocationName,ToLocation,ToLocationName,Remarks,Comments,Commentstatus,AcceptanceDate,AcceptedUser,AcceptanceStatus," +
                    "CreateUser,CreateTime,ForwardUser,ApproveUser,ForwarderList,Status,DocumentStatus,ReceiveStatus, MaterialValue " +
                    " from ViewGTNHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99) and Status = 1  order by GTNDate desc,DocumentID asc,GTNNo desc";
                //string query4 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                //    " GTNNo,GTNDate,FromLocation,ToLocation,Remarks,Comments,Commentstatus,AcceptanceDate,AcceptedUser,AcceptanceStatus," +
                //    "CreateUser,CreateTime,ForwardUser,ApproveUser,ForwarderList,Status,DocumentStatus " +
                //    " from ViewGTNHeader" +
                //   " where DocumentID = " + "'INDENT'" + "and IndentNo > 0 and DocumentStatus = 99  ";
                string query6 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " GTNNo,GTNDate,FromLocation,FromLocationName,ToLocation,ToLocationName,Remarks,Comments,Commentstatus,AcceptanceDate,AcceptedUser,AcceptanceStatus," +
                    "CreateUser,CreateTime,ForwardUser,ApproveUser,ForwarderList,Status,DocumentStatus,ReceiveStatus, MaterialValue " +
                    " from ViewGTNHeader" +
                    " where  DocumentStatus = 99 and Status = 1  order by GTNDate desc,DocumentID asc,GTNNo desc";
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
                    //case 4:
                    //    query = query4;
                    //    break;
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
                    gheader = new gtnheader();
                    gheader.RowID = reader.GetInt32(0);
                    gheader.DocumentID = reader.GetString(1);
                    gheader.DocumentName = reader.GetString(2);
                    gheader.TemporaryNo = reader.GetInt32(3);
                    gheader.TemporaryDate = reader.GetDateTime(4);
                    gheader.GTNNo = reader.GetInt32(5);
                    gheader.GTNDate = reader.GetDateTime(6);
                    gheader.FromLocation = reader.GetString(7);
                    gheader.FromLocationName = reader.GetString(8);
                    gheader.ToLocation = reader.GetString(9);
                    gheader.ToLocationName = reader.GetString(10);
                    gheader.Remarks = reader.GetString(11);
                    gheader.Comments = reader.GetString(12);
                    gheader.CommentStatus = reader.GetString(13);
                    gheader.AcceptDate = reader.GetDateTime(14);
                    gheader.AcceptUser = reader.IsDBNull(15)?"":reader.GetString(15);
                    gheader.AcceptStatus = reader.GetInt32(16);
                    gheader.CreateUser = reader.GetString(17);
                    gheader.CreateTime = reader.GetDateTime(18);
                    if (!reader.IsDBNull(19))
                    {
                        gheader.ForwardUser = reader.GetString(19);
                    }
                    else
                    {
                        gheader.ForwardUser = "";
                    }
                    if (!reader.IsDBNull(20))
                    {
                        gheader.ApproveUser = reader.GetString(20);
                    }
                    else
                    {
                        gheader.ApproveUser = "";
                    }
                    //gheader.ApproveUser = reader.GetString(18);
                    gheader.ForwarderList = reader.GetString(21);
                    gheader.Status = reader.GetInt32(22);
                    gheader.DocumentStatus = reader.GetInt32(23);
                    gheader.ReceiveStatus = reader.GetInt32(24);
                    gheader.MaterialValue = reader.GetDecimal(25);
                    gheaderlist.Add(gheader);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying GTN Header Data");
            }
            return gheaderlist;
        }
        public Boolean updateGTNInStock(List<gtndetail> gtndList, string store)
        {
            Boolean status = true;
            try
            {
                foreach (gtndetail gtnd in gtndList)
                {
                    if (!CheckStockAvailability(gtnd.refNo, gtnd.Quantity))
                    {
                        status = false;
                        break;
                    }
                }
                if (status)
                {
                    //stock available. select rows from stock table. update stcok table. insert GTN records in stock table

                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        public Boolean CheckStockAvailability(int stockRefNo, double Qunt)
        {
            Boolean status = false;

            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select PresentStock" +
                   " from Stock where RowID =" + stockRefNo;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Double aqty = reader.GetDouble(0);
                    if (aqty >= Qunt)
                        status = true;
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying stock quantity");
                status = false;
            }
            return status;
        }
        public List<stock> GetItemStock(string stockitem, string store)
        {
            List<stock> ItemStock = new List<stock>();
            try
            {
                stock stk = new stock();
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,StockItemID,ModelNo,ModelName,PresentStock,MRNNo,MRNDate,BatchNo,ExpiryDate,Unit,PurchaseQuantity," +
                    "PurchasePrice,PurchaseTax,SupplierID,StoreLocation from ViewStock where StockItemID='" + stockitem + "' and StoreLocation='" + store + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stk = new stock();
                    stk.RowID = reader.GetInt32(0);
                    stk.StockItemID = reader.GetString(1);
                    stk.ModelNo = reader.IsDBNull(2)?"NA":reader.GetString(2);
                    stk.ModelName = reader.IsDBNull(3) ? "NA" : reader.GetString(3);
                    stk.PresentStock = reader.GetDouble(4);
                    stk.MRNNo = reader.GetInt32(5);
                    stk.MRNDate = reader.GetDateTime(6);
                    stk.BatchNo = reader.GetString(7);
                    stk.ExpiryDate = reader.GetDateTime(8);
                    if (!reader.IsDBNull(9))
                    {
                        stk.StockItemUnit = reader.GetString(9);
                    }
                    else
                    {
                        stk.StockItemUnit = "";
                    }
                   
                    stk.PurchaseQuantity = reader.GetDouble(10);
                    stk.PurchasePrice = reader.GetDouble(11);
                    stk.PurchaseTax = reader.GetDouble(12);
                    stk.SupplierID = reader.GetString(13);
                    stk.StoreLocation = reader.GetString(14);
                    ItemStock.Add(stk);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                ItemStock = new List<stock>();
            }

            return ItemStock;
        }

        //---

        public static List<gtndetail> getGTNDetail(gtnheader gtnh)
        {
            gtndetail gtnd;
            List<gtndetail> GTNDetail = new List<gtndetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,TemporaryNo, TemporaryDate,StockItemID,StockItemName,ModelNo, ModelName,Quantity, " +
                    "MRNNo,MRNDate,BatchNo,SerialNo,ExpiryDate,PurchaseQuantity,PurchasePrice,PurchaseTax,SupplierID,SupplierName,StockReferenceNo " +
                   "from ViewGTNDetail " +
                   "where DocumentID='" + gtnh.DocumentID + "'" +
                   " and TemporaryNo=" + gtnh.TemporaryNo +
                   " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                   " order by StockItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    gtnd = new gtndetail();
                    gtnd.RowID = reader.GetInt32(0);
                    gtnd.DocumentID = reader.GetString(1);
                    gtnd.TemporaryNo = reader.GetInt32(2);
                    gtnd.TemporaryDate = reader.GetDateTime(3).Date;
                    gtnd.StockitemID = reader.GetString(4);
                    gtnd.StockItemName = reader.GetString(5);
                    gtnd.ModelNo = reader.IsDBNull(6)?"NA":reader.GetString(6);
                    gtnd.ModelName = reader.IsDBNull(7) ? "NA":reader.GetString(7);
                    gtnd.Quantity = reader.GetDouble(8);
                    gtnd.MRNNo = reader.IsDBNull(9)? 0:reader.GetInt32(9);
                    if(reader.IsDBNull(10))
                    {
                        gtnd.MRNDate = DateTime.Parse("01-01-1900");
                    }
                    else
                    {
                        gtnd.MRNDate = reader.GetDateTime(10).Date;
                    }
                    gtnd.BatchNo = reader.IsDBNull(11) ? "NA" : reader.GetString(11);
                    gtnd.SerialNo = reader.IsDBNull(12) ? "NA" : reader.GetString(12);
                    if (reader.IsDBNull(13))
                    {
                        gtnd.ExpiryDate = DateTime.Parse("01-01-1900");
                    }
                    else
                    {
                        gtnd.ExpiryDate = reader.GetDateTime(13);
                    }
                    gtnd.PurchaseQuantity = reader.GetDouble(14);
                    gtnd.PurchasePrice = reader.GetDouble(15);
                    gtnd.PurchaseTax = reader.GetDouble(16);
                    gtnd.SupplierID = reader.IsDBNull(17)? "NA" : reader.GetString(17);
                    gtnd.SupplierName = reader.IsDBNull(18)?"NA" : reader.GetString(17);
                    if (reader.IsDBNull(13))
                    {
                        gtnd.refNo = 0;
                    }
                    else
                    {
                        gtnd.refNo = reader.GetInt32(19);
                    }
                    GTNDetail.Add(gtnd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying GTN Details");
            }
            return GTNDetail;
        }
        
       
        public Boolean validateGTNHeader(gtnheader gtnh)
        {
            Boolean status = true;
            if(gtnh.FromLocation == gtnh.ToLocation)
            {
                MessageBox.Show("From Location and To Location Should not be same");
                return false;
            }
            if(gtnh.FromLocation == null)
            {
                return false;
            }
            if(gtnh.ToLocation == null)
            {
                return false;
            }
            ////////if (gtnh.MaterialValue == 0)
            ////////{
            ////////    return false;
            ////////}
            if (gtnh.Remarks.Trim().Length == 0)
            {
                return false;
            }
            return status;
        }
      
        public Boolean forwardGTN(gtnheader gtnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update GTNHeader set DocumentStatus=" + (gtnh.DocumentStatus + 1) +
                    ", ReceiveStatus = " + gtnh.ReceiveStatus +
                    ", forwardUser='" + gtnh.ForwardUser + "'" +
                    ", commentStatus='" + gtnh.CommentStatus + "'" +
                    ", ForwarderList='" + gtnh.ForwarderList + "'" +
                    " where DocumentID='" + gtnh.DocumentID + "'" +
                    " and TemporaryNo=" + gtnh.TemporaryNo +
                    " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "GTNHeader", "", updateSQL) +
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
        public Boolean reverseGTN(gtnheader gtnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update GTNHeader set DocumentStatus=" + gtnh.DocumentStatus +
                    ", forwardUser='" + gtnh.ForwardUser + "'" +
                    ", commentStatus='" + gtnh.CommentStatus + "'" +
                    ", ForwarderList='" + gtnh.ForwarderList + "'" +
                     ", ReceiveStatus=" + gtnh.ReceiveStatus +
                    " where DocumentID='" + gtnh.DocumentID + "'" +
                    " and TemporaryNo=" + gtnh.TemporaryNo +
                    " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "GTNHeader", "", updateSQL) +
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
        public Boolean AcceptGTN(gtnheader gtnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update GTNHeader set DocumentStatus=99, status=1 " +
                    ", AcceptedUser='" + Login.userLoggedIn + "'" +
                     ", ApproveUser='" + Login.userLoggedIn + "'" +
                     ", AcceptanceDate=convert(date, getdate())" +
                      ", AcceptanceStatus= 1" +
                    ", commentStatus='" + gtnh.CommentStatus + "'" +
                    ", GTNNo=" + gtnh.GTNNo +
                    ", GTNDate=convert(date, getdate())" +
                     ", ReceiveStatus = 99" +
                    " where DocumentID='" + gtnh.DocumentID + "'" +
                    " and TemporaryNo=" + gtnh.TemporaryNo +
                    " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "GTNHeader", "", updateSQL) +
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
        //----
        public Boolean updateGTNInStockDetail(gtnheader gtnh,List<gtndetail> GTNDetails)
        {
            Boolean status = true;
            // string utString = "";
            try
            {
                foreach (gtndetail gtnd in GTNDetails)
                {
                    double quant = gtnd.Quantity;
                    int RefNo = gtnd.refNo;
                    updateRefNoWiseGTNDetailInStock(quant, RefNo,gtnd,gtnh);
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        public void updateRefNoWiseGTNDetailInStock(double Quant, int refNo, gtndetail gtnd, gtnheader gtnh)
        {
            //Boolean status = true;
            string updateSQL = "";
            string utString = "";
            try
            {
                updateSQL = "update Stock set  " +
                    " PresentStock=" + "( (select PresentStock from Stock where RowID = " + refNo + ")-" + Quant + ")" +
                    " where RowID=" + refNo;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "update Stock set  " +
                   " IssueQuantity=" + "( (select isnull(IssueQuantity,0) from Stock where RowID = " + refNo + ")+" + Quant + ")" +
                   " where RowID=" + refNo;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
                Main.QueryDelimiter;


                updateSQL = "insert into Stock " +
                            "(FYID,StockItemID,ModelNo,InwardDocumentID,InwardDocumentNo,InwardDocumentDate,InwardQuantity,PresentStock,MRNNo,MRNDate," +
                            "BatchNo,ExpiryDate,SerialNo,PurchaseQuantity,PurchasePrice,PurchaseTax,SupplierID,StoreLocation," +
                            "CreateTime,CreateUser)" +
                            " values (" +
                            "'" + Main.currentFY + "'," +
                            "'" + gtnd.StockitemID + "'," +
                            "'" + gtnd.ModelNo + "'," +
                            "'" + "GTN" + "'," +
                            gtnh.GTNNo + "," +
                            "'" + gtnh.GTNDate.ToString("yyyy-MM-dd") + "'," +
                            gtnd.Quantity + "," +
                            gtnd.Quantity + "," +
                            gtnd.MRNNo + "," +
                            "'" + gtnd.MRNDate.ToString("yyyy-MM-dd") + "'," +
                            "'" + gtnd.BatchNo + "'," +
                            "'" + gtnd.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                             "'" + gtnd.SerialNo + "'," +
                            gtnd.PurchaseQuantity + "," +
                            gtnd.PurchasePrice + "," +
                            gtnd.PurchaseTax + "," +
                            "'" + gtnd.SupplierID + "'," +
                            "'" + gtnh.ToLocation + "'," +
                            "GETDATE()" + "," +
                            "'" + Login.userLoggedIn + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    //status = false;
                    MessageBox.Show("failed to Update In Reference Number Wise GTNDetail in stock");
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
        //---
        //---
        public Boolean updateGTNInStock(gtnheader gtnh, List<gtndetail> gtndList)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            DateTime dt = UpdateTable.getSQLDateTime();
            try
            {
                foreach (gtndetail gtnd in gtndList)
                {
                    List<stock> ItemStock = GetItemStock(gtnd.StockitemID, gtnh.FromLocation);
                    double tqty = 0;
                    //check quantity availability
                    foreach (stock stk in ItemStock)
                    {
                        //quantity check
                        tqty = tqty + stk.PresentStock;
                    }
                    if (tqty < gtnd.Quantity)
                    {
                        //not enough quantity for issue
                        status = false;
                        break;
                    }
                    //update stock in list
                    double gtnqty = 0;
                    double balqty = gtnd.Quantity;
                    foreach (stock stk in ItemStock)
                    {
                        if (balqty <= stk.PresentStock)
                        {
                            gtnqty = balqty;
                            stk.PresentStock = stk.PresentStock - balqty;
                            balqty = 0;

                        }
                        else
                        {
                            //stock not sufficient 
                            gtnqty = stk.PresentStock;
                            balqty = balqty - stk.PresentStock;
                            stk.PresentStock = 0;
                        }
                        //Delete GTNdetail
                        updateSQL = "Delete from GTNDetail where DocumentID='" + gtnh.DocumentID + "'" +
                        " and TemporaryNo=" + gtnh.TemporaryNo +
                        " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                        " and StockItemID='" + stk.StockItemID + "'";
                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                            ActivityLogDB.PrepareActivityLogQquerString("delete", "GTNDetail", "", updateSQL) +
                            Main.QueryDelimiter;
                        //create GTNDetail insert string
                        updateSQL = CreateGTNDetailString(stk, gtnh);
                        if (updateSQL.Trim().Length > 0)
                        {
                            utString = utString + updateSQL + Main.QueryDelimiter;
                            utString = utString +
                            ActivityLogDB.PrepareActivityLogQquerString("insert", "GTNDetail", "", updateSQL) +
                            Main.QueryDelimiter;
                        }
                        else
                        {
                            status = false;
                            break;
                        }
                        //create stock update string
                        updateSQL = "Update stock set PresentStock=" + stk.PresentStock + " where RowID=" + stk.RowID;
                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                        ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
                        Main.QueryDelimiter;
                        //create stock insert string
                        updateSQL = createGTNStockInsertString(gtnqty, stk, gtnh, gtnh.ToLocation);
                        if (updateSQL.Trim().Length > 0)
                        {
                            utString = utString + updateSQL + Main.QueryDelimiter;
                            utString = utString +
                            ActivityLogDB.PrepareActivityLogQquerString("insert", "Stock", "", updateSQL) +
                            Main.QueryDelimiter;
                        }
                        else
                        {
                            status = false;
                            break;
                        }
                    }
                    if (!status)
                        break;
                }
                if (status)
                {
                    //update tables
                    if (!UpdateTable.UT(utString))
                    {
                        status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public string CreateGTNDetailString(stock stk, gtnheader gtnh)
        {
            string updateSQL = "";
            try
            {
                updateSQL = "insert into GTNDetail " +
                           "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,Quantity,MRNNo,MRNDate," +
                           "BatchNo,ExpiryDate,PurchaseQuantity,PurchasePrice,PurchaseTax) values (" +
                           "'" + gtnh.DocumentID + "'," +
                           gtnh.TemporaryNo + "," +
                           "'" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                           "'" + stk.StockItemID + "'," +
                              "'" + stk.ModelNo + "'," +
                           stk.PresentStock + "," +
                           stk.MRNNo + "," +
                           "'" + stk.MRNDate.ToString("yyyy-MM-dd") + "'," +
                           "'" + stk.BatchNo + "'," +
                           "'" + stk.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                           stk.PurchaseQuantity + "," +
                           stk.PurchasePrice + "," +
                           stk.PurchaseTax + ")";
            }
            catch (Exception ex)
            {
                updateSQL = "";
            }
            return updateSQL;
        }
        public string createGTNStockInsertString(double gtnqty, stock stk, gtnheader gtnh, string store)
        {
            string updateSQL = "";
            try
            {
                updateSQL = "insert into Stock " +
                           "(FYID,StockItemID,ModelNo,InwardDocumentID,InwardDocumentNo,InwardDocumentDate,InwardQuantity,PresentStock,MRNNo,MRNDate," +
                           "BatchNo,ExpiryDate,PurchaseQuantity,PurchasePrice,PurchaseTax,SupplierID,StoreLocation," +
                           "CreateTime,CreateUser)" +
                           " values (" +
                           "'" + Main.currentFY + "'," +
                           "'" + stk.StockItemID + "'," +
                           "'" + stk.ModelNo + "'," +
                           "'" + "GTN" + "'," +
                           gtnh.GTNNo + "," +
                           "'" + gtnh.GTNDate.ToString("yyyy-MM-dd") + "'," +
                           gtnqty + "," +
                           gtnqty + "," +
                           stk.MRNNo + "," +
                           "'" + stk.MRNDate.ToString("yyyy-MM-dd") + "'," +
                           "'" + stk.BatchNo + "'," +
                           "'" + stk.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                           stk.PurchaseQuantity + "," +
                           stk.PurchasePrice + "," +
                           stk.PurchaseTax + "," +
                           "'" + stk.SupplierID + "'," +
                           "'" + store + "'," +
                           "GETDATE()" + "," +
                           "'" + Login.userLoggedIn + "')";
            }
            catch (Exception ex)
            {
                updateSQL = "";
            }
            return updateSQL;
        }
        public static string getUserComments(string docid, int tempno, DateTime tempdate)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from GTNHeader where DocumentID='" + docid + "'" +
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

        public Boolean updateGTNHeaderAndDetail(gtnheader gtnh, gtnheader prevgtnh, List<gtndetail> GTNDetails)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update GTNHeader set FromLocation='" + gtnh.FromLocation +
                    "',ToLocation='" + gtnh.ToLocation +
                    "',MaterialValue=" + gtnh.MaterialValue + "," +
                    " Remarks='" + gtnh.Remarks +
                    "', CommentStatus='" + gtnh.CommentStatus +
                    "', Comments='" + gtnh.Comments +
                    "', ForwarderList='" + gtnh.ForwarderList +
                    "' where DocumentID='" + gtnh.DocumentID + "'" +
                    " and TemporaryNo=" + gtnh.TemporaryNo +
                    " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "GTNHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from GTNDetail where DocumentID='" + gtnh.DocumentID + "'" +
                     " and TemporaryNo=" + gtnh.TemporaryNo +
                     " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "GTNDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (gtndetail gtnd in GTNDetails)
                {
                    updateSQL = "insert into GTNDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,Quantity,MRNNo,MRNDate,BatchNo,ExpiryDate,SerialNo,PurchaseQuantity, " +
                   "PurchasePrice,PurchaseTax,SupplierID,StockReferenceNo) " +
                    "values ('" + gtnd.DocumentID + "'," +
                    gtnd.TemporaryNo + "," +
                    "'" + gtnd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + gtnd.StockitemID + "'," +
                     "'" + gtnd.ModelNo + "'," +
                    gtnd.Quantity + "," + gtnd.MRNNo + "," +
                     "'" + gtnd.MRNDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + gtnd.BatchNo + "'," +
                   "'" + gtnd.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                   "'" + gtnd.SerialNo + "'," +
                   gtnd.PurchaseQuantity + "," +
                   gtnd.PurchasePrice + "," +
                   gtnd.PurchaseTax + "," +
                     "'" + gtnd.SupplierID + "'," +
                     +gtnd.refNo + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "GTNDetail", "", updateSQL) +
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
        public Boolean InsertGTNHeaderAndDetail(gtnheader gtnh, List<gtndetail> GTNDetails)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                gtnh.TemporaryNo = DocumentNumberDB.getNumber(gtnh.DocumentID, 1);
                if (gtnh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + gtnh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + gtnh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into GTNHeader " +
                     "(DocumentID,TemporaryNo,TemporaryDate,GTNNo,GTNDate,FromLocation,ToLocation,MaterialValue," +
                     "Remarks,Comments,CommentStatus,AcceptanceDate,AcceptedUser,AcceptanceStatus," +
                     "ForwarderList,Status,DocumentStatus,CreateTime,CreateUser)" +
                     " values (" +
                     "'" + gtnh.DocumentID + "'," +
                     gtnh.TemporaryNo + "," +
                     "'" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     gtnh.GTNNo + "," +
                     "'" + gtnh.GTNDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + gtnh.FromLocation + "'," +
                     "'" + gtnh.ToLocation + "'," +
                       gtnh.MaterialValue + "," +
                     "'" + gtnh.Remarks + "'," +
                      "'" + gtnh.Comments + "'," +
                     "'" + gtnh.CommentStatus + "'," +
                     "'" + gtnh.AcceptDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + gtnh.AcceptUser + "'," +
                     +gtnh.AcceptStatus + "," +
                     "'" + gtnh.ForwarderList + "'," +
                     gtnh.Status + "," +
                     gtnh.DocumentStatus + "," +
                      "GETDATE()" + "," +
                     "'" + Login.userLoggedIn + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "GTNHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from GTNDetail where DocumentID='" + gtnh.DocumentID + "'" +
                     " and TemporaryNo=" + gtnh.TemporaryNo +
                     " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "GTNDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (gtndetail gtnd in GTNDetails)
                {
                    updateSQL = "insert into GTNDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,Quantity,MRNNo,MRNDate,BatchNo,ExpiryDate,SerialNo,PurchaseQuantity, " +
                   "PurchasePrice,PurchaseTax,SupplierID,StockReferenceNo) " +
                    "values ('" + gtnd.DocumentID + "'," +
                    gtnh.TemporaryNo + "," +
                    "'" + gtnd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + gtnd.StockitemID + "'," +
                     "'" + gtnd.ModelNo + "'," +
                    gtnd.Quantity + "," + gtnd.MRNNo + "," +
                     "'" + gtnd.MRNDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + gtnd.BatchNo + "'," +
                   "'" + gtnd.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                   "'" + gtnd.SerialNo + "'," +
                   gtnd.PurchaseQuantity + "," +
                   gtnd.PurchasePrice + "," +
                   gtnd.PurchaseTax + "," +
                     "'" + gtnd.SupplierID + "'," +
                     +gtnd.refNo + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "GTNDetail", "", updateSQL) +
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

        public static Boolean checkForIssuesOfGTNInStock(int tno, DateTime tdt)
        {
            Boolean status = false;   // Not issued
            int count = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select COUNT(*) from Stock where InwardDocumentID = 'GTN' and " +
                        " InwardDocumentNo = (select GTNNo from GTNHeader where TemporaryNo = " + tno + " and TemporaryDate = '" + tdt.ToString("yyyy-MM-dd") + "') and" +
                        " InwardDocumentDate = (select GTNDate from GTNHeader where TemporaryNo = " + tno + " and TemporaryDate = '" + tdt.ToString("yyyy-MM-dd") + "') " +
                        " and InwardQuantity <> PresentStock";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    count = reader.GetInt32(0);
                }
                if (count != 0)
                    status = true; // Stock ALready Issued
                conn.Close();
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
    }
}
