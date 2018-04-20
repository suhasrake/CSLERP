using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class stockobheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string FYID { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public string StoreLocation { get; set; }
        public string StoreLocationName { get; set; }
        public double Value { get; set; }
        public int status { get; set; }
        public int Documentstatus { get; set; }
        public DateTime Createime { get; set; }
        public string CreateUser { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string CreatorName { get; set; }
        public string ForwarderName { get; set; }
        public string ApproverName { get; set; }
        public string Remarks { get; set; }
        public int Transferstatus { get; set; }
    }
    class stockobdetail
    {
        public int HeaderNo { get; set; }
        public string DocumentID { get; set; }
        public string FYID { get; set; }
        public int TemporaryNo { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public double Quantity { get; set; }
        public double PurchasePrice { get; set; }
        public int QualityID { get; set; }
        public int AcceptanceStatus { get; set; }
    }
    class StockOBDB
    {
        public List<stockobheader> getFilteredStockOBHeader(string userList,int opt)
        {
            stockobheader sobh;

            string query1 = "select RowID, DocumentID, DocumentName,FYID,TemporaryNo,TemporaryDate," +
                    " DocumentNo,DocumentDate,StoreLocation,StoreLocationName,Value,Status,DocumentStatus," +
                    " CreateUser,ForwardUser,approveUser,CreatorName,ForwarderName,ApproverName,Remarks,transferStatus " +
                    " from ViewStockOBHeader" +
                    " where ((forwardUser in (" + userList + ") and DocumentStatus between 2 and 98) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1))";
            string query2 = "select RowID, DocumentID, DocumentName,FYID,TemporaryNo,TemporaryDate," +
                    " DocumentNo,DocumentDate,StoreLocation,StoreLocationName,Value,Status,DocumentStatus," +
                    " CreateUser,ForwardUser,approveUser,CreatorName,ForwarderName,ApproverName,Remarks,transferStatus " +
                    " from ViewStockOBHeader" +
                   " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98) " +
                     " or (forwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98))";
            string query3 = "select RowID, DocumentID, DocumentName,FYID,TemporaryNo,TemporaryDate," +
                    " DocumentNo,DocumentDate,StoreLocation,StoreLocationName,Value,Status,DocumentStatus," +
                    " CreateUser,ForwardUser,approveUser,CreatorName,ForwarderName,ApproverName,Remarks,transferStatus " +
                    " from ViewStockOBHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or forwardUser='" + Login.userLoggedIn + "'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99) ";

            List<stockobheader> SOBHeaders = new List<stockobheader>();
            try
            {
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
                    default:
                        query = "";
                        break;
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sobh = new stockobheader();
                    sobh.RowID = reader.GetInt32(0);
                    sobh.DocumentID = reader.GetString(1);
                    sobh.DocumentName = reader.GetString(2);
                    sobh.FYID = reader.GetString(3);
                    sobh.TemporaryNo = reader.GetInt32(4);
                    sobh.TemporaryDate = reader.GetDateTime(5);
                    sobh.DocumentNo = reader.GetInt32(6);
                    if (!reader.IsDBNull(7))
                    {
                        sobh.DocumentDate = reader.GetDateTime(7);
                    }

                    sobh.StoreLocation = reader.GetString(8);
                    sobh.StoreLocationName = reader.GetString(9);
                    sobh.Value = reader.GetDouble(10);
                    sobh.status = reader.GetInt32(11);
                    sobh.Documentstatus = reader.GetInt32(12);
                    sobh.CreateUser = reader.GetString(13);
                    sobh.ForwardUser = reader.GetString(14);
                    sobh.ApproveUser = reader.GetString(15);
                    sobh.CreatorName = reader.GetString(16);
                    sobh.ForwarderName = reader.GetString(17);
                    sobh.ApproverName = reader.GetString(18);
                    sobh.Remarks = reader.GetString(19);
                    sobh.Transferstatus = reader.GetInt32(20);
                    SOBHeaders.Add(sobh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying STOCK-OB Header Details");
            }
            return SOBHeaders;
        }

        public List<stockobheader> getStockOBHeader()
        {
            stockobheader sobh;
            List<stockobheader> SOBHeaders = new List<stockobheader>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID, DocumentID, DocumentName,FYID,TemporaryNo,TemporaryDate," +
                    " DocumentNo,DocumentDate,StoreLocation,StoreLocationName,Value,Status,DocumentStatus," +
                    " CreateUser,ForwardUser,approveUser,CreatorName,ForwarderName,ApproverName,Remarks,TransferStatus " +
                    "from ViewStockOBHeader";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sobh = new stockobheader();
                    sobh.RowID = reader.GetInt32(0);
                    sobh.DocumentID = reader.GetString(1);
                    sobh.DocumentName = reader.GetString(2);
                    sobh.FYID = reader.GetString(3);
                    sobh.TemporaryNo = reader.GetInt32(4);
                    sobh.TemporaryDate = reader.GetDateTime(5);
                    sobh.DocumentNo = reader.GetInt32(6);
                    if (!reader.IsDBNull(7))
                    {
                        sobh.DocumentDate = reader.GetDateTime(7);
                    }

                    sobh.StoreLocation = reader.GetString(8);
                    sobh.StoreLocationName = reader.GetString(9);
                    sobh.Value = reader.GetDouble(10);
                    sobh.status = reader.GetInt32(11);
                    sobh.Documentstatus = reader.GetInt32(12);
                    sobh.CreateUser = reader.GetString(13);
                    sobh.ForwardUser = reader.GetString(14);
                    sobh.ApproveUser = reader.GetString(15);
                    sobh.CreatorName = reader.GetString(16);
                    sobh.ForwarderName = reader.GetString(17);
                    sobh.ApproverName = reader.GetString(18);
                    sobh.Remarks = reader.GetString(19);
                    sobh.Transferstatus = reader.GetInt32(20);
                    SOBHeaders.Add(sobh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying STOCK-OB Header Details");
            }
            return SOBHeaders;
        }



        public static List<stockobdetail> getStockOBDetail(stockobheader sobh)
        {
            stockobdetail sobd;
            List<stockobdetail> SOBDetail = new List<stockobdetail>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.DocumentID,a.TemporaryNo,a.FYID, a.StockItemID,b.Name, a.Quantity,a.Price,a.QualityID,a.AcceptanceStatus " +
                    "from StockOBDetail a, StockItem b  where a.StockItemID= b.StockItemID and a.DocumentID='" + sobh.DocumentID + "'" +
                    " and a.TemporaryNo=" + sobh.TemporaryNo + " and FYID='" + sobh.FYID + "'" +
                    " order by a. StockItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sobd = new stockobdetail();
                    sobd.DocumentID = reader.GetString(0);
                    sobd.TemporaryNo = reader.GetInt32(1);
                    sobd.FYID = reader.GetString(2);
                    sobd.StockItemID = reader.GetString(3);
                    sobd.StockItemName = reader.GetString(4);
                    sobd.Quantity = reader.GetDouble(5);
                    sobd.PurchasePrice = reader.GetDouble(6);
                    sobd.QualityID = reader.GetInt32(7);
                    sobd.AcceptanceStatus = reader.GetInt32(8);
                    SOBDetail.Add(sobd);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Stock-OB Details");
            }
            return SOBDetail;
        }

        public static Boolean TransferOBToStock(DataGridView grdList, string FYID, String DocumentID)
        {
            string utString = "";
            Boolean status = true;
            try
            {
                stockobheader sobh = new stockobheader();
                List<stockobdetail> SOBDetail = new List<stockobdetail>();
                string updateSQL = "Delete from Stock where InwardDocumentID='" + DocumentID + "'" +
                        " and FYID='" + FYID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "Stock", "", updateSQL) +
                    Main.QueryDelimiter;

                for (int i = 0; i < grdList.Rows.Count; i++)
                {
                    sobh = new stockobheader();
                    sobh.DocumentID = grdList.Rows[i].Cells["DocumentID"].Value.ToString();
                    sobh.FYID = grdList.Rows[i].Cells["FYID"].Value.ToString();
                    sobh.TemporaryNo = Convert.ToInt32(grdList.Rows[i].Cells["TemporaryNo"].Value);
                    sobh.DocumentNo = Convert.ToInt32(grdList.Rows[i].Cells["DocumentNo"].Value);
                    sobh.DocumentDate = DateTime.Parse(grdList.Rows[i].Cells["DocumentDate"].Value.ToString());
                    sobh.StoreLocation = grdList.Rows[i].Cells["StoreLocationID"].Value.ToString();
                    SOBDetail = getStockOBDetail(sobh);
                    foreach (stockobdetail sobd in SOBDetail)
                    {
                        updateSQL = "insert into stock (FYID,InwardDocumentID,InwardDocumentNo,InwardDocumentDate," +
                            "StockItemID,InwardQuantity,PresentStock,PurchasePrice,StoreLocation,CreateTime,CreateUser) values (" +
                            "'" + sobd.FYID + "'," +
                            "'" + sobd.DocumentID + "'," +
                            "" + sobh.DocumentNo + "," +
                            "'" + sobh.DocumentDate + "'," +
                            "'" + sobd.StockItemID + "'," +
                            "" + sobd.Quantity + "," +
                            "" + sobd.Quantity + "," +
                            "" + sobd.PurchasePrice + "," +
                            "'" + sobh.StoreLocation + "'," +
                            "GETDATE()" + "," +
                            "'" + Login.userLoggedIn + "')";
                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                            ActivityLogDB.PrepareActivityLogQquerString("insert", "Stock", "", updateSQL) +
                            Main.QueryDelimiter;
                    }
                    updateSQL = "update StockOBHeader set  TransferStatus=1" + 
                    " where DocumentID='" + sobh.DocumentID + "'" +
                    " and TemporaryNo=" + sobh.TemporaryNo +
                    " and FYID='" + sobh.FYID + "'";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "StockOBHeader", "", updateSQL) +
                    Main.QueryDelimiter;
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

        public Boolean updateStockOBHeader(stockobheader sobh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockOBHeader set StoreLocation='" + sobh.StoreLocation +
                    "',Remarks='" + sobh.Remarks +
                     "',Value=" + sobh.Value +
                    ", Status=" + sobh.status +
                    ", DocumentStatus=" + sobh.Documentstatus +
                    " where DocumentID='" + sobh.DocumentID + "'" +
                    " and TemporaryNo=" + sobh.TemporaryNo +
                    " and FYID='" + sobh.FYID + "'";
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

        public Boolean updateStockOBDetail(List<stockobdetail> SOBDetails, stockobheader sobh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Delete from StockOBDetail where DocumentID='" + sobh.DocumentID + "'" +
                    " and TemporaryNo=" + sobh.TemporaryNo +
                    " and FYID='" + sobh.FYID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "StockOBDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (stockobdetail sobd in SOBDetails)
                {
                    updateSQL = "insert into StockOBDetail " +
                    "(DocumentID,TemporaryNo,FYID,StockItemID,Quantity,Price,QualityID,AcceptanceStatus) " +
                    "values ('" + sobd.DocumentID + "'," +
                    sobd.TemporaryNo + "," +
                    "'" + sobd.FYID + "'," +
                    "'" + sobd.StockItemID + "'," +
                    sobd.Quantity + "," +
                    sobd.PurchasePrice + "," +
                    sobd.QualityID + "," +
                    sobd.AcceptanceStatus + ")";
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
            }
            return status;
        }

        public Boolean insertStockOBHeader(stockobheader sobh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into StockOBHeader " +
                    "(DocumentID,FYID,TemporaryNo,Value,TemporaryDate,DocumentNo,DocumentDate,StoreLocation,Status,DocumentStatus," +
                    "CreateTime,CreateUser,ForwardUser,ApproveUser,Remarks)" +
                    "values (" +
                    "'" + sobh.DocumentID + "'," +
                    "'" + sobh.FYID + "'," +
                    sobh.TemporaryNo + "," +
                    sobh.Value + "," +
                    "GETDATE()" + "," +
                    "0,null," +
                    "'" + sobh.StoreLocation + "'," +
                    sobh.status + "," +
                    sobh.Documentstatus + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'," +
                    "'',''," +
                    "'" + sobh.Remarks + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "StockOBHeader", "", updateSQL) +
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
        public Boolean deleteStockOBHeader(stockobheader sobh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "delete StockOBHeader where DocumentID='" + sobh.DocumentID + "'" +
                    " and TemporaryNo=" + sobh.TemporaryNo +
                    " and FYID='" + sobh.FYID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("delete", "StockOBHeader", "", updateSQL) +
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

        public Boolean validateStockOBHeader(stockobheader sobh)
        {
            Boolean status = true;
            try
            {
                if (sobh.TemporaryNo == 0)
                {
                    return false;
                }
                if (sobh.FYID.Trim().Length == 0 || sobh.FYID == null)
                {
                    return false;
                }

                if (sobh.StoreLocation.Trim().Length == 0 || sobh.StoreLocation == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {
            }
            return status;
        }
        public Boolean forwardStockOB(stockobheader sobh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockOBHeader set DocumentStatus=" + (sobh.Documentstatus + 1) +
                    ", forwardUser='" + Login.userLoggedIn + "'" +
                    " where DocumentID='" + sobh.DocumentID + "'" +
                    " and TemporaryNo=" + sobh.TemporaryNo +
                    " and FYID='" + sobh.FYID + "'";
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
        public Boolean ApproveStockOB(stockobheader sobh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StockOBHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", DocumentNo=" + sobh.DocumentNo +
                    ", DocumentDate=GETDATE()" +
                    " where DocumentID='" + sobh.DocumentID + "'" +
                    " and TemporaryNo=" + sobh.TemporaryNo +
                    " and FYID='" + sobh.FYID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "StockOBHeader", "", updateSQL) +
                Main.QueryDelimiter;
                updateSQL = "update StockOBDetail set DocumentStatus=99 " +
                    " where DocumentID='" + sobh.DocumentID + "'" +
                    " and TemporaryNo=" + sobh.TemporaryNo +
                    " and FYID='" + sobh.FYID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;

                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "StockOBDetail", "", updateSQL) +
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
    }
}
