using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class stockObNewHeader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string FYID { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public string StoreLocation { get; set; }
        public string StoreLocationName { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Value { get; set; }
        public int Status { get; set; }
        public int DocumentStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string CreatorName { get; set; }
        public string Remarks { get; set; }
        public int Transferstatus { get; set; }
    }
    class stockObNewDetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
    }
    class StockOBNewDB
    {
        public static List<stockObNewHeader> getStockOblist(int opt)
        {
            stockObNewHeader sbh;
            List<stockObNewHeader> SbList = new List<stockObNewHeader>();
            try
            {
                string query1 = "select distinct DocumentID,DocumentNo,DocumentDate,FYID,StoreLocation,Value, " +
                    "Status, Createuser,CreatorName, CreateTime,Remarks, TransferStatus,DocumentStatus,DocumentName,StoreLocationName " +
                    "from ViewStockOB where (Createuser='" + Login.userLoggedIn + "' and Status = 0 and TransferStatus = 0)";
                string query2 = "select distinct DocumentID,DocumentNo,DocumentDate,FYID,StoreLocation,Value, " +
                    "Status, Createuser,CreatorName, CreateTime,Remarks, TransferStatus,DocumentStatus,DocumentName,StoreLocationName " +
                    "from ViewStockOB where Status = 1 and DocumentStatus = 99";
                string query3 = "select distinct DocumentID,DocumentNo,DocumentDate,FYID,StoreLocation,Value, " +
                    "Status, Createuser,CreatorName, CreateTime,Remarks, TransferStatus,DocumentStatus,DocumentName,StoreLocationName " +
                    "from ViewStockOB where Status = 1 and DocumentStatus = 99 and TransferStatus = 0";
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
                        query = query3; // For showing Details in StockTransfer Form
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
                    sbh = new stockObNewHeader();
                    sbh.DocumentID = reader.GetString(0);
                    sbh.DocumentNo = reader.GetInt32(1);
                    sbh.DocumentDate = reader.GetDateTime(2);
                    sbh.FYID = reader.GetString(3);
                    sbh.StoreLocation = reader.GetString(4);
                    sbh.Value = reader.GetDouble(5);
                    sbh.Status = reader.GetInt32(6);
                    sbh.CreateUser = reader.GetString(7);
                    sbh.CreatorName = reader.GetString(8);
                    sbh.CreateTime = reader.GetDateTime(9);
                    sbh.Remarks = reader.IsDBNull(10)?"":reader.GetString(10);
                    sbh.Transferstatus = reader.GetInt32(11);
                    sbh.DocumentStatus = reader.GetInt32(12);
                    sbh.DocumentName = reader.GetString(13);
                    sbh.StoreLocationName = reader.GetString(14);
                    SbList.Add(sbh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Serviced List Details");
            }
            return SbList;
        }
        public static List<stockObNewDetail> getstockObNewDetail(stockObNewHeader Acchr)
        {
            stockObNewDetail sobDetail;
            List<stockObNewDetail> SobDetail = new List<stockObNewDetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,DocumentNo,DocumentDate,StockItemID,StockItemName, ModelNo, ModelName, Quantity , Price "+
                   "from ViewStockOB " +
                    " where DocumentID='" + Acchr.DocumentID + "'" +
                    " and DocumentNo=" + Acchr.DocumentNo +
                    " and DocumentDate='" + Acchr.DocumentDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sobDetail = new stockObNewDetail();
                    sobDetail.RowID = reader.GetInt32(0);
                    sobDetail.DocumentID = reader.GetString(1);
                    sobDetail.DocumentNo = reader.GetInt32(2);
                    sobDetail.DocumentDate = reader.GetDateTime(3).Date;
                    sobDetail.StockItemID = reader.GetString(4);
                    sobDetail.StockItemName = reader.GetString(5);
                    sobDetail.ModelNo = reader.IsDBNull(6)? "NA" : reader.GetString(6);
                    sobDetail.ModelName = reader.IsDBNull(7) ? "NA" : reader.GetString(7);
                    sobDetail.Quantity = reader.GetDouble(8);
                    sobDetail.Price = reader.GetDouble(9);
                    SobDetail.Add(sobDetail);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Details");
            }
            return SobDetail;
        }
        //public Boolean updateStockOBHeader(stockObNewHeader sobh, stockObNewHeader prevSobh)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "update StockOBHeader set  FYID='" + sobh.FYID + "', StoreLocation = '" + sobh.StoreLocation + "'" +
        //            ", Value = " + sobh.Value + ", Remarks = '"+ sobh.Remarks +"'" + 
        //             " where DocumentID='" + prevSobh.DocumentID +
        //            "' and DocumentNo=" + prevSobh.DocumentNo +
        //            " and DocumentDate='" + prevSobh.DocumentDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("update", "StockOBHeader", "", updateSQL) +
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
        public Boolean verifyFYLoc(string FYID, string Loc)
        {
            Boolean status = true;
            try
            {
                SqlConnection con = new SqlConnection(Login.connString);
                string query = "select FYID, StoreLocation from StockOBHeader";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if ((FYID.Equals(reader.GetString(0)) && Loc.Equals(reader.GetString(1))))
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;

        }
        public static Boolean checkAvailabilityOfFY(string FYID)
        {
            Boolean status = true;
            int count = 0;
            try
            {
                SqlConnection con = new SqlConnection(Login.connString);
                string query = "select count(*) from Stock where FYID = '" + FYID + "'";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    count = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            if(count > 0)
            {
                status = false;
            }
            return status;

        }
        //public Boolean insertstockObNewHeader(stockObNewHeader sobh)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "insert into StockOBHeader " +
        //            " (DocumentID,FYID,DocumentDate,DocumentNo,StoreLocation, Value, Status, DocumentStatus,Remarks," +
        //            "CreateUser,CreateTime)" +
        //            "values (" +
        //            "'" + sobh.DocumentID + "'," +
        //           sobh.FYID + "," +
        //           "'" + sobh.DocumentDate.ToString("yyyy-MM-dd") + "'," +
        //             + sobh.DocumentNo + "," +
        //             "'" + sobh.StoreLocation + "'," +
        //           sobh.Value + "," +
        //           sobh.Status + "," +
        //           sobh.DocumentStatus + "," +
        //          "'" + sobh.Remarks + "'," +
        //            "'" + Login.userLoggedIn + "'," +
        //            "GETDATE())";
        //        //"'" + pheader.ForwardUser + "'," +
        //        //"'" + pheader.ApproveUser + "'," +
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("insert", "StockOBHeader", "", updateSQL) +
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

        //public Boolean UpdateStockOBDetail(List<stockObNewDetail> SobDetail, stockObNewHeader sobh)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "Delete from StockOBDetail where DocumentID='" + sobh.DocumentID + "'" +
        //            " and DocumentNo=" + sobh.DocumentNo +
        //            " and DocumentDate='" + sobh.DocumentDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //            ActivityLogDB.PrepareActivityLogQquerString("delete", "StockOBDetail", "", updateSQL) +
        //            Main.QueryDelimiter;
        //        foreach (stockObNewDetail sobd in SobDetail)
        //        {
        //            updateSQL = "insert into StockOBDetail " +
        //            "(DocumentID,DocumentNo,DocumentDate,StockItemID,ModelNo,Quantity,Price) " +
        //            "values ('" + sobd.DocumentID + "'," +
        //            sobd.DocumentNo + "," +
        //            "'" + sobd.DocumentDate.ToString("yyyy-MM-dd") + "'," +
        //            "'" + sobd.StockItemID + "'," +
        //            "'" + sobd.ModelNo + "'," +
        //             sobd.Quantity + "," +
        //             sobd.Price + ")";
        //            utString = utString + updateSQL + Main.QueryDelimiter;
        //            utString = utString +
        //            ActivityLogDB.PrepareActivityLogQquerString("insert", "StockOBDetail", "", updateSQL) +
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
        //public Boolean deleteProductTestTempDetail(stockObNewDetail sdbd)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "delete StockOBDetail " +
        //            " where DocumentID='" + sdbd.DocumentID + "'" +
        //            " and DocumentNo=" + sdbd.DocumentNo +
        //            " and DocumentDate='" + sdbd.DocumentDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("delete", "StockOBDetail", "", updateSQL) +
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

        public Boolean validateStockOB(stockObNewHeader sodh)
        {
            Boolean status = true;
            try
            {
                if (sodh.DocumentID.Trim().Length == 0 || sodh.DocumentID == null)
                {
                    return false;
                }
                //if (sodh.DocumentNo == 0)
                //{
                //    return false;
                //}
                if (sodh.DocumentDate == null)
                {
                    return false;
                }
                if (sodh.FYID.Trim().Length == 0 || sodh.FYID == null)
                {
                    return false;
                }
                if (sodh.StoreLocation.Trim().Length == 0 || sodh.StoreLocation == null)
                {
                    return false;
                }
                if (sodh.Remarks.Trim().Length == 0 || sodh.Remarks == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public Boolean FinalizeStockOB(stockObNewHeader Acchr)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockOBHeader set Status = 1, DocumentStatus = 99" +
                    " where DocumentNo=" + Acchr.DocumentNo +
                    " and DocumentDate='" + Acchr.DocumentDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "StockOBHeader", "", updateSQL) +
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
        public static Boolean changeTransferStatus(string FYID)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockOBHeader set TransferStatus = 1 " +
                    " where FYID = '"+FYID + "' and DocumentStatus = 99 and Status = 1" ;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "StockOBHeader", "", updateSQL) +
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
        public static List<stockObNewHeader> getStockOblistForTransfer(string FYID)
        {
            stockObNewHeader sbh;
            List<stockObNewHeader> SbList = new List<stockObNewHeader>();
            try
            {
                string query = "select DocumentID,DocumentName,FYID,DocumentNo,DocumentDate,StoreLocation,StoreLocationName,Value,Remarks,StockItemID,StockItemName " +
                    ", ModelNo, ModelName,Quantity, Price,Status,DocumentStatus, Createuser,CreatorName, CreateTime,TransferStatus " +
                    "from ViewStockOB where (FYID='" + FYID + "' and Status = 1)";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sbh = new stockObNewHeader();
                    sbh.DocumentID = reader.GetString(0);
                    sbh.DocumentName = reader.GetString(1);
                    sbh.FYID = reader.GetString(2);
                    sbh.DocumentNo = reader.GetInt32(3);
                    sbh.DocumentDate = reader.GetDateTime(4);
                    sbh.StoreLocation = reader.GetString(5);
                    sbh.StoreLocationName = reader.GetString(6);
                    sbh.Value = reader.GetDouble(7);
                    sbh.Remarks = reader.GetString(8);
                    sbh.StockItemID = reader.GetString(9);
                    sbh.StockItemName = reader.GetString(10);
                    sbh.ModelNo = reader.IsDBNull(11) ? "NA" : reader.GetString(11);
                    sbh.ModelName = reader.IsDBNull(12)?"NA":reader.GetString(12);
                    sbh.Quantity = reader.GetDouble(13);
                    sbh.Price = reader.GetDouble(14);
                    sbh.Status = reader.GetInt32(15);
                    sbh.DocumentStatus = reader.GetInt32(16);
                    sbh.CreateUser = reader.GetString(17);
                    sbh.CreatorName = reader.GetString(18);
                    sbh.CreateTime = reader.GetDateTime(19);
                    sbh.Transferstatus = reader.GetInt32(20);
                    SbList.Add(sbh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying StockOB List Details");
            }
            return SbList;
        }
        public static Boolean TransferStockOB(List<stockObNewHeader> SBList, string FYID)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Delete from Stock where FYID='" + FYID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "Stock", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (stockObNewHeader sobh in SBList)
                {
                    updateSQL = "insert into Stock" +
                    "(FYID,StockItemID,ModelNo,InwardDocumentID,InwardDocumentNo,InwardDocumentDate,InwardQuantity,PurchaseQuantity,PresentStock,PurchasePrice," +
                    "StoreLocation, CreateUser, CreateTime) " +
                    "values ('" + sobh.FYID + "'," +
                     "'" + sobh.StockItemID + "'," +
                      "'" + sobh.ModelNo + "'," +
                    "'" + sobh.DocumentID + "'," +
                    sobh.DocumentNo + "," +
                    "'" + sobh.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                    +sobh.Quantity + "," +
                    +sobh.Quantity + "," +
                    +sobh.Quantity + "," +
                     sobh.Price + "," +
                     "'" + sobh.StoreLocation + "'," +
                       "'" + Login.userLoggedIn + "', GETDATE())";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "Stock", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch(Exception ex)
            {

            }
            return status;
        }
        public Boolean updateStockHeaderAndDetail(stockObNewHeader sobh, List<stockObNewDetail> StockDetail)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockOBHeader set  FYID='" + sobh.FYID + "', StoreLocation = '" + sobh.StoreLocation + "'" +
                    ", Value = " + sobh.Value + ", Remarks = '" + sobh.Remarks + "'" +
                     " where DocumentID='" + sobh.DocumentID +
                    "' and DocumentNo=" + sobh.DocumentNo +
                    " and DocumentDate='" + sobh.DocumentDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "StockOBHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from StockOBDetail where DocumentID='" + sobh.DocumentID + "'" +
                     " and DocumentNo=" + sobh.DocumentNo +
                     " and DocumentDate='" + sobh.DocumentDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "StockOBDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (stockObNewDetail sobd in StockDetail)
                {
                    updateSQL = "insert into StockOBDetail " +
                    "(DocumentID,DocumentNo,DocumentDate,StockItemID,ModelNo,Quantity,Price) " +
                    "values ('" + sobd.DocumentID + "'," +
                    sobd.DocumentNo + "," +
                    "'" + sobd.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + sobd.StockItemID + "'," +
                    "'" + sobd.ModelNo + "'," +
                     sobd.Quantity + "," +
                     sobd.Price + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "StockOBDetail", "", updateSQL) +
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
        public Boolean InsertStockHeaderAndDetail(stockObNewHeader sobh, List<stockObNewDetail> StockDetail)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                sobh.DocumentNo = DocumentNumberDB.getNumber(sobh.DocumentID, 1);
                if (sobh.DocumentNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + sobh.DocumentNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + sobh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into StockOBHeader " +
                     " (DocumentID,FYID,DocumentDate,DocumentNo,StoreLocation, Value, Status, DocumentStatus,Remarks," +
                     "CreateUser,CreateTime)" +
                     "values (" +
                     "'" + sobh.DocumentID + "','" +
                    sobh.FYID + "'," +
                    "'" + sobh.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                      +sobh.DocumentNo + "," +
                      "'" + sobh.StoreLocation + "'," +
                    sobh.Value + "," +
                    sobh.Status + "," +
                    sobh.DocumentStatus + "," +
                   "'" + sobh.Remarks + "'," +
                     "'" + Login.userLoggedIn + "'," +
                     "GETDATE())";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "StockOBHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from StockOBDetail where DocumentID='" + sobh.DocumentID + "'" +
                    " and DocumentNo=" + sobh.DocumentNo +
                    " and DocumentDate='" + sobh.DocumentDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "StockOBDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (stockObNewDetail sobd in StockDetail)
                {
                    updateSQL = "insert into StockOBDetail " +
                    "(DocumentID,DocumentNo,DocumentDate,StockItemID,ModelNo,Quantity,Price) " +
                    "values ('" + sobd.DocumentID + "'," +
                    sobh.DocumentNo + "," +
                    "'" + sobd.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + sobd.StockItemID + "'," +
                    "'" + sobd.ModelNo + "'," +
                     sobd.Quantity + "," +
                     sobd.Price + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "StockOBDetail", "", updateSQL) +
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
