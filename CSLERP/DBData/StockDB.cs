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
    public class stock
    {
        public int RowID { get; set; }
        public string FYID { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string StockItemUnit { get; set; }
        public string InwardDocumentID { get; set; }
        public string InwardDocumentNo { get; set; }
        public DateTime InwardDocumentDate { get; set; }
        public double InwardQuantity { get; set; }
        public double PresentStock { get; set; }
        public double QuantityBooked { get; set; }
        public double StockOnHold { get; set; }
        public int MRNNo { get; set; }
        public DateTime MRNDate { get; set; }
        public string BatchNo { get; set; }
        public string SerielNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double PurchaseQuantity { get; set; }
        public double PurchasePrice { get; set; }
        public double PurchaseTax { get; set; }
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string StoreLocation { get; set; }
        public string Level1GCode { get; set; }
        public string Level2GCode { get; set; }
        public string Level3GCode { get; set; }
        public string Level1GDescription { get; set; }
        public string Level2GDescription { get; set; }
        public string Level3GDescription { get; set; }
        public DateTime CreateTime { get; set; }

        public double IssueQuantity { get; set; }
        public double PurchaseReturnQuantity { get; set; }
        public stock()
        {

        }
    }
    class TotalStock
    {
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public double Stock { get; set; }
    }
    class ClosingStock
    {
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public double OBQty { get; set; }
        public double OBValue { get; set; }
        public double CBQty { get; set; }
        public double CBValue { get; set; }
    }
    class StockDB
    {
        public Boolean insertStockFromMRN(List<mrndetail> MRNDetails, mrnheader mrnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                foreach (mrndetail mrnd in MRNDetails)
                {
                    string updateSQL = "insert into Stock " +
                    "(FYID,StockItemID,ModelNo,InwardDocumentID,InwardDocumentNo,InwardDocumentDate,InwardQuantity,PresentStock,MRNNo,MRNDate," +
                    "BatchNo,SerialNo,ExpiryDate,PurchaseQuantity,PurchasePrice,PurchaseTax,SupplierID,StoreLocation," +
                    "CreateTime,CreateUser,IssueQuantity)" +
                    " values (" +
                    "'" + Main.currentFY + "'," +
                    "'" + mrnd.StockItemID + "'," +
                      "'" + mrnd.ModelNo + "'," +
                    "'" + "MRN" + "'," +
                    mrnh.MRNNo + "," +
                    "'" + mrnh.MRNDate.ToString("yyyy-MM-dd") + "'," +
                    mrnd.QuantityAccepted + "," +
                    mrnd.QuantityAccepted + "," +
                     mrnh.MRNNo + "," +
                    "'" + mrnh.MRNDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + mrnd.BatchNo + "'," +
                   "'" + mrnd.SerialNo + "'," +
                    "'" + mrnd.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                    mrnd.QuantityAccepted + "," +
                    mrnd.PriceINR + "," +
                    mrnd.TaxINR + "," +
                    "'" + mrnh.CustomerID + "'," +
                    "'" + mrnd.StoreLocationID + "'," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "',0)";
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
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        public static List<stock> getStoreWiseStockDetail(string Location)
        {
            stock stk;
            List<stock> StockList = new List<stock>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,StockItemID,StockItemName,ModelNo, ModelName,MRNNo,MRnDate,BatchNo,SerialNo,ExpiryDate," +
                    "PurchaseQuantity,PurchasePrice,PurchaseTax,SupplierId,SupplierName, StoreLocation,PresentStock,InwardDocumentID,InwardDocumentDate " +
                  "from ViewStock where FYID = '" + Main.currentFY + "' and StoreLocation = '" + Location + " and PresentStock > 0' order by StockItemName asc, RowID asc";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stk = new stock();
                    stk.RowID = reader.GetInt32(0);
                    stk.StockItemID = reader.GetString(1);
                    stk.StockItemName = reader.GetString(2);
                    stk.ModelNo = reader.IsDBNull(3) ? "NA" : reader.GetString(3);
                    stk.ModelName = reader.IsDBNull(4) ? "NA" : reader.GetString(4);
                    stk.MRNNo = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                    stk.MRNDate = reader.IsDBNull(6) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(6);
                    if (reader.IsDBNull(7))
                    {
                        stk.BatchNo = "";
                    }
                    else
                    {
                        stk.BatchNo = reader.GetString(7);
                    }
                    if (reader.IsDBNull(8))
                    {
                        stk.SerielNo = "";
                    }
                    else
                    {
                        stk.SerielNo = reader.IsDBNull(8) ? "NA" : reader.GetString(8);
                    }
                    if (reader.IsDBNull(8))
                    {
                        stk.ExpiryDate = DateTime.Parse("1900-01-01");
                    }
                    else
                    {
                        stk.ExpiryDate = reader.GetDateTime(9);
                    }
                    stk.PurchaseQuantity = reader.GetDouble(10);
                    stk.PurchasePrice = reader.GetDouble(11);
                    stk.PurchaseTax = reader.GetDouble(12);
                    stk.SupplierID = reader.IsDBNull(13) ? "NA" : reader.GetString(13);
                    stk.SupplierName = reader.IsDBNull(14) ? "NA" : reader.GetString(14);
                    stk.StoreLocation = reader.GetString(15);
                    stk.PresentStock = reader.GetDouble(16);
                    stk.InwardDocumentID = reader.GetString(17);
                    stk.InwardDocumentDate = reader.IsDBNull(18) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(18);
                    StockList.Add(stk);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying MRN Details");
            }
            return StockList;
        }
        public static ListView getStoreWiseStockItemName(string location)
        {
            ListView lv = new ListView();
            try
            {
                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockReferenceNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItemID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItemName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ModelNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ModelName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("MRNNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("MRNDate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PresentStock", -2, HorizontalAlignment.Left);
                lv.Columns.Add("BAtchNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SerialNO", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ExpiryDate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PurchaseQuantity", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PurchasePrice", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PurchaseTax", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SupplierId", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SupplierName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Document", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Document Date", -2, HorizontalAlignment.Left);

                lv.Columns[1].Width = 0;
                lv.Columns[6].Width = 0;
                lv.Columns[7].Width = 0;

                List<stock> StockList = StockDB.getStoreWiseStockDetail(location);
                foreach (stock stk in StockList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = false;
                    item.SubItems.Add(stk.RowID.ToString());
                    item.SubItems.Add(stk.StockItemID);
                    item.SubItems.Add(stk.StockItemName);
                    item.SubItems.Add(stk.ModelNo);
                    item.SubItems.Add(stk.ModelName);
                    item.SubItems.Add(stk.MRNNo.ToString());
                    item.SubItems.Add(stk.MRNDate.ToString("dd-MM-yyyy"));
                    item.SubItems.Add(stk.PresentStock.ToString());
                    item.SubItems.Add(stk.BatchNo.ToString());
                    item.SubItems.Add(stk.SerielNo.ToString());
                    item.SubItems.Add(stk.ExpiryDate.ToString("dd-MM-yyyy"));
                    item.SubItems.Add(stk.PurchaseQuantity.ToString());
                    item.SubItems.Add(stk.PurchasePrice.ToString());
                    item.SubItems.Add(stk.PurchaseTax.ToString());
                    item.SubItems.Add(stk.SupplierID.ToString());
                    item.SubItems.Add(stk.SupplierName.ToString());
                    item.SubItems.Add(stk.InwardDocumentID);
                    item.SubItems.Add(stk.InwardDocumentDate.ToString("dd-MM-yyyy"));
                    lv.Items.Add(item);
                }

            }
            catch (Exception ex)
            {

            }
            return lv;
        }
        public static List<stock> getMRNNoFromStock()
        {
            stock stk;
            List<stock> StockList = new List<stock>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select distinct a.MRNNo,a.MRNDate,a.SupplierID,b.name " +
                   " from Stock as a left join Customer as b on a.SupplierID = b.CustomerID " +
                   " where FYID = '" + Main.currentFY + "' and a.presentStock > 0 and a.InwardDocumentID = 'MRN'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stk = new stock();
                    stk.MRNNo = reader.GetInt32(0);
                    stk.MRNDate = reader.GetDateTime(1);
                    stk.SupplierID = reader.GetString(2);
                    stk.SupplierName = reader.IsDBNull(3) ? "NA" : reader.GetString(3);
                    //stk.BatchNo = reader.IsDBNull(4) ? "NA" : reader.GetString(4); // BatchNo holds TaxCode value
                    StockList.Add(stk);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Details");
            }
            return StockList;
        }
        public static ListView getMRNNoWithStockDetail()
        {
            ListView lv = new ListView();
            try
            {
                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                ////DocCommenterDB doccmdb = new DocCommenterDB();
                ////List<doccommenter> DocCommenters = doccmdb.getDocCommList();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("MRN No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("MRN Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SupplierID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SupplierName", -2, HorizontalAlignment.Left);
                //lv.Columns.Add("TaxCode", -2, HorizontalAlignment.Left);
                //lv.Columns.Add("StockOnHold", -2, HorizontalAlignment.Left);
                //lv.Columns[1].Width = 150;

                List<stock> StockList = StockDB.getMRNNoFromStock();
                foreach (stock stk in StockList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = false;
                    item.SubItems.Add(stk.MRNNo.ToString());
                    item.SubItems.Add(stk.MRNDate.ToShortDateString());
                    item.SubItems.Add(stk.SupplierID);
                    item.SubItems.Add(stk.SupplierName);
                    //item.SubItems.Add(stk.BatchNo);  // BatchNo holds TaxCode value
                    lv.Items.Add(item);
                }

            }
            catch (Exception ex)
            {

            }
            return lv;
        }
        public static List<stock> getMRNNoWiseStockDetails(int MRNNo, DateTime MRNDate)
        {
            stock stk;
            List<stock> StockList = new List<stock>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select a.StockItemID, b.name,a.ModelNo, d.ModelName, a.PresentStock, a.PurchasePrice, a.BatchNo, a.SerialNo, a.ExpiryDate," +
                    " a.StoreLocation, c.Description, a.RowID,e.TaxCode,a.PurchaseQuantity " +
                   " from Stock as a left join StockItem as b on a.StockItemID = b.StockItemID" +
                   " left join CatalogueValue as c on a.StoreLocation = c.CatalogueValueID " +
                   " left join ProductModel as d on a.StockItemID = d.StockItemID and a.ModelNo = d.ModelNo " +
                   " left join MRNDetail as e on a.StockItemID = e.StockItemID and (a.ModelNo = e.ModelNo or a.ModelNo = null) and " +
                   " e.TemporaryNo = (select TemporaryNo from MRNHeader where MRNNo =" + MRNNo + " and MRNDate = '" + MRNDate.ToString("yyyy-MM-dd") + "') and" +
                   " e.TemporaryDate = (select TemporaryDate from MRNHeader where MRNNo =" + MRNNo + " and MRNDate = '" + MRNDate.ToString("yyyy-MM-dd") + "')" +
                   " where a.MRNNo =" + MRNNo +
                   " and a.MRNDate = '" + MRNDate.ToString("yyyy-MM-dd") + "'" + 
                   " and a.PresentStock > 0 and FYID = '" + Main.currentFY + "'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stk = new stock();
                    stk.StockItemID = reader.GetString(0);
                    stk.StockItemName = reader.GetString(1);
                    stk.ModelNo = reader.IsDBNull(2) ? "NA" : reader.GetString(2);
                    stk.ModelName = reader.IsDBNull(3) ? "NA" : reader.GetString(3);
                    stk.PresentStock = reader.GetDouble(4);
                    stk.PurchasePrice = reader.GetDouble(5);
                    stk.BatchNo = reader.GetString(6);
                    if (!reader.IsDBNull(7))
                    {
                        stk.SerielNo = reader.GetString(7);
                    }
                    else
                        stk.SerielNo = "";
                    stk.ExpiryDate = reader.GetDateTime(8);
                    stk.StoreLocation = reader.GetString(9);
                    stk.SupplierName = reader.GetString(10); //// SupplierName used for StoreLOcationName
                    stk.RowID = reader.GetInt32(11);
                    stk.InwardDocumentID = reader.IsDBNull(12) ? "" : reader.GetString(12);//// InwaradDocumentID used for TaxCode
                    stk.PurchaseQuantity = reader.GetDouble(13);
                    StockList.Add(stk);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Details");
            }
            return StockList;
        }
        public static ListView getMRNNoWiseStockListView(int MRNNo, DateTime MRNDate)
        {
            ListView lv = new ListView();
            try
            {
                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                ////DocCommenterDB doccmdb = new DocCommenterDB();
                ////List<doccommenter> DocCommenters = doccmdb.getDocCommList();
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StkRefNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ItemID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ItemName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ModelNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ModelName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PresentStock", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PurchaseQuan", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PurchasePrice", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Batch No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Seriel No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Expiary Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StoreLocation", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StoreLocName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("TaxCode", -2, HorizontalAlignment.Left);
                //lv.Columns[1].Width = 150;

                List<stock> StockList = StockDB.getMRNNoWiseStockDetails(MRNNo, MRNDate);
                foreach (stock stk in StockList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = false;
                    item.SubItems.Add(stk.RowID.ToString());
                    item.SubItems.Add(stk.StockItemID.ToString());
                    item.SubItems.Add(stk.StockItemName.ToString());
                    item.SubItems.Add(stk.ModelNo.ToString());
                    item.SubItems.Add(stk.ModelName.ToString());
                    item.SubItems.Add(stk.PresentStock.ToString());
                    item.SubItems.Add(stk.PurchaseQuantity.ToString());
                    item.SubItems.Add(stk.PurchasePrice.ToString());
                    item.SubItems.Add(stk.BatchNo.ToString());
                    item.SubItems.Add(stk.SerielNo.ToString());
                    item.SubItems.Add(stk.ExpiryDate.ToShortDateString());
                    item.SubItems.Add(stk.StoreLocation.ToString());
                    item.SubItems.Add(stk.SupplierName.ToString()); // SupplierName used for StoreLOcationName
                    item.SubItems.Add(stk.InwardDocumentID.ToString());//// InwaradDocumentID used for TaxCode
                    lv.Items.Add(item);
                }

            }
            catch (Exception ex)
            {

            }
            return lv;
        }
        public static Boolean verifyPresentStockAvailability(int stockRefNo, double Qunt)
        {
            Boolean status = true;
            double PresentStock = 0;
            try
            {
                string query = "";
                int text = 0;
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select PresentStock" +
                   " from Stock where RowID =" + stockRefNo;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                //SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    PresentStock = reader.GetDouble(0);
                    text = 1;
                }
                conn.Close();
                if (PresentStock != 0)
                {
                    if (Qunt > PresentStock)
                        return false;
                }
                else if (text == 1 && PresentStock == 0)
                {
                    MessageBox.Show("Available Quantity For this stock is ZERO.");
                    return false;
                }
                else if (text == 0 && PresentStock == 0)
                {
                    MessageBox.Show("stock NOt found");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Details");
            }
            return status;
        }

        public static ListView getStoreWiseItemDetailForStockHolding(string location)
        {
            ListView lv = new ListView();
            try
            {
                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                ////DocCommenterDB doccmdb = new DocCommenterDB();
                ////List<doccommenter> DocCommenters = doccmdb.getDocCommList();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockReferenceNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItemID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItemName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ModelNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ModelName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Inward Doc ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Inward Doc No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Inward Doc Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Inward Quantity", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PresentStock", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockOnHold", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Store Location", -2, HorizontalAlignment.Left);
                //lv.Columns[1].Width = 150;

                List<stock> StockList = StockDB.getStoreWiseStockDetailForStockHolding(location);
                foreach (stock stk in StockList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = false;
                    item.SubItems.Add(stk.RowID.ToString());
                    item.SubItems.Add(stk.StockItemID);
                    item.SubItems.Add(stk.StockItemName);
                    item.SubItems.Add(stk.ModelNo);
                    item.SubItems.Add(stk.ModelName);
                    item.SubItems.Add(stk.InwardDocumentID.ToString());
                    item.SubItems.Add(stk.InwardDocumentNo.ToString());
                    item.SubItems.Add(stk.InwardDocumentDate.ToString());
                    item.SubItems.Add(stk.InwardQuantity.ToString());
                    item.SubItems.Add(stk.PresentStock.ToString());
                    item.SubItems.Add(stk.StockOnHold.ToString());
                    item.SubItems.Add(stk.StoreLocation);
                    lv.Items.Add(item);
                }

            }
            catch (Exception ex)
            {

            }
            return lv;
        }
        public static List<stock> getStoreWiseStockDetailForStockHolding(string Location)
        {
            stock stk;
            List<stock> StockList = new List<stock>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,StockItemID,StockItemName,ModelNo, ModelName,InwardDocumentID,InwardDocumentNo,InwardDocumentDate,InwardQuantity," +
                    "PresentStock,StockOnHold,StoreLocation " +
                  "from ViewStock where StoreLocation = '" + Location + "' and PresentStock > 0 and FYID = '" 
                  + Main.currentFY + "'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stk = new stock();
                    stk.RowID = reader.GetInt32(0);
                    stk.StockItemID = reader.GetString(1);
                    stk.StockItemName = reader.GetString(2);
                    stk.ModelNo = reader.IsDBNull(3) ? "NA" : reader.GetString(3);
                    stk.ModelName = reader.IsDBNull(4) ? "NA" : reader.GetString(4);
                    stk.InwardDocumentID = reader.GetString(5);
                    stk.InwardDocumentNo = reader.GetString(6);
                    stk.InwardDocumentDate = reader.GetDateTime(7);
                    stk.InwardQuantity = reader.GetDouble(8);
                    stk.PresentStock = reader.GetDouble(9);
                    stk.StockOnHold = reader.GetDouble(10);
                    stk.StoreLocation = reader.GetString(11);

                    StockList.Add(stk);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying stock Details");
            }
            return StockList;
        }
        public static double getLastPurchasePriceForMRN(string id, string ModNo)
        {
            double price = 0;
            try
            {
                string query = "";
                string itemID = id.Substring(0, id.IndexOf('-'));
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select PurchasePrice" +
                   " from Stock where StockItemId ='" + itemID + "'" +
                   " and ModelNo = '" + ModNo + "' and InwardDocumentID = 'MRN' " +
                   " and StoreLocation in ('" + Main.MainStore + "','" + Main.FactoryStore + "') order by InwardDocumentDate desc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                //SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    price = reader.GetDouble(0);
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Details");
            }
            return price;
        }

        public static double getTotalStockForIndent(string id, string ModNo)
        {
            double stock = 0;
            try
            {
                string query = "";
                string itemID = id.Substring(0, id.IndexOf('-'));
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select SUM(PresentStock)" +
                   " from Stock where StockItemId ='" + itemID + "'" +
                   " and ModelNo = '" + ModNo + "'" +
                   " and StoreLocation in ('" + Main.MainStore + "','" + Main.FactoryStore + "')"+
                   " and FYID = '" + Main.currentFY + "' group by StockItemID,ModelNo";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                //SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    stock = reader.GetDouble(0);
                conn.Close();
                //if (stock == 0)
                //    MessageBox.Show("Stock Item Not Found in Existing Stocks");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Details");
            }
            return stock;
        }
        public static ListView getFactoryWiseStock()
        {
            ListView lv = new ListView();
            try
            {
                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockRefNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItemID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItemName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ModelNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ModelName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("MRNNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("MRNDate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PresentStock", -2, HorizontalAlignment.Left);
                lv.Columns.Add("BAtchNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SerialNO", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ExpiryDate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PurchaseQuantity", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PurchasePrice", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PurchaseTax", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SupplierId", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SupplierName", -2, HorizontalAlignment.Left);
                lv.Columns[2].Width = 0;
                lv.Columns[4].Width = 0;
                //lv.Columns[2].Width = 0;
                //lv.Columns[2].Width = 0;

                List<stock> StockList = StockDB.getFactoryStockDetails();
                foreach (stock stk in StockList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = false;
                    item.SubItems.Add(stk.RowID.ToString());
                    item.SubItems.Add(stk.StockItemID);
                    item.SubItems.Add(stk.StockItemName);
                    item.SubItems.Add(stk.ModelNo);
                    item.SubItems.Add(stk.ModelName);
                    item.SubItems.Add(stk.MRNNo.ToString());
                    //item.SubItems.Add(stk.MRNDate.ToString());
                    item.SubItems.Add(stk.MRNDate.ToShortDateString());
                    item.SubItems.Add(stk.PresentStock.ToString());
                    item.SubItems.Add(stk.BatchNo.ToString());
                    item.SubItems.Add(stk.SerielNo.ToString());
                    //item.SubItems.Add(stk.ExpiryDate.ToString());
                    item.SubItems.Add(stk.ExpiryDate.ToShortDateString());
                    item.SubItems.Add(stk.PurchaseQuantity.ToString());
                    item.SubItems.Add(stk.PurchasePrice.ToString());
                    item.SubItems.Add(stk.PurchaseTax.ToString());
                    item.SubItems.Add(stk.SupplierID.ToString());
                    item.SubItems.Add(stk.SupplierName.ToString());
                    lv.Items.Add(item);
                }

            }
            catch (Exception ex)
            {

            }
            return lv;
        }
        public static List<stock> getFactoryStockDetails()
        {
            stock stk;
            List<stock> StockList = new List<stock>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,StockItemID,StockItemName,ModelNo, ModelName,MRNNo,MRNDate,BatchNo,SerialNo,ExpiryDate," +
                    "PurchaseQuantity,PurchasePrice,PurchaseTax,SupplierId,SupplierName, StoreLocation,PresentStock " +
                  "from ViewStock where StoreLocation = 'FACTORYSTORE' and FYID = '" + 
                  Main.currentFY + "' order by StockItemName";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stk = new stock();
                    stk.RowID = reader.GetInt32(0);
                    stk.StockItemID = reader.GetString(1);
                    stk.StockItemName = reader.GetString(2);
                    stk.ModelNo = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    stk.ModelName = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    stk.MRNNo = reader.GetInt32(5);
                    stk.MRNDate = reader.GetDateTime(6);
                    stk.BatchNo = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    stk.SerielNo = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    stk.ExpiryDate = reader.IsDBNull(9) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(9);
                    stk.PurchaseQuantity = reader.GetDouble(10);
                    stk.PurchasePrice = reader.GetDouble(11);
                    stk.PurchaseTax = reader.GetDouble(12);
                    stk.SupplierID = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    stk.SupplierName = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    stk.StoreLocation = reader.GetString(15);
                    stk.PresentStock = reader.GetDouble(16);
                    StockList.Add(stk);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying stock Details");
            }
            return StockList;
        }
        ////-----list stock from main store only
        public static List<stock> getProductWiseStockDetails(string pID, string ModelNo)
        {
            stock stk;
            List<stock> StockList = new List<stock>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                if (ModelNo == "NA" || ModelNo == null)
                {
                    query = "select RowID,StockItemID,StockItemName,ModelNo, ModelName,MRNNo,MRNDate,BatchNo,SerialNo,ExpiryDate," +
                    " PurchaseQuantity,PurchasePrice,PurchaseTax,SupplierId,SupplierName, StoreLocation,PresentStock,storeLocation,QuantityBooked " +
                    " from ViewStock where FYID = '" + Main.currentFY + "' and StockItemID = '" + pID + "'" +
                    " and storelocation='" + Main.MainStore + "'" +
                    " and (ModelNo = 'NA' or ModelNo is null)";
                }
                else
                {
                    query = "select RowID,StockItemID,StockItemName,ModelNo, ModelName,MRNNo,MRNDate,BatchNo,SerialNo,ExpiryDate," +
                   "PurchaseQuantity,PurchasePrice,PurchaseTax,SupplierId,SupplierName, StoreLocation,PresentStock,storeLocation,QuantityBooked " +
                 "from ViewStock where FYID = '" + Main.currentFY + "' and StockItemID = '" + pID + "' and ModelNo = '" + ModelNo + "'";
                }
                //query = "select RowID,StockItemID,StockItemName,ModelNo, ModelName,MRNNo,MRNDate,BatchNo,SerialNo,ExpiryDate," +
                //   "PurchaseQuantity,PurchasePrice,PurchaseTax,SupplierId,SupplierName, StoreLocation,PresentStock " +
                // "from ViewStock where StockItemID = '" + pID + "' and ModelNo = '" + ModelNo +"'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stk = new stock();
                    stk.RowID = reader.GetInt32(0);
                    stk.StockItemID = reader.GetString(1);
                    stk.StockItemName = reader.GetString(2);
                    stk.ModelNo = reader.IsDBNull(3) ? "NA" : reader.GetString(3);
                    stk.ModelName = reader.IsDBNull(4) ? "NA" : reader.GetString(4);
                    stk.MRNNo = reader.GetInt32(5);
                    stk.MRNDate = reader.GetDateTime(6);
                    stk.BatchNo = reader.IsDBNull(7) ? "NA" : reader.GetString(7);
                    stk.SerielNo = reader.IsDBNull(8) ? "NA" : reader.GetString(8);
                    stk.ExpiryDate = reader.IsDBNull(9) ? Convert.ToDateTime("1900-01-01") : reader.GetDateTime(9);
                    stk.PurchaseQuantity = reader.GetDouble(10);
                    stk.PurchasePrice = reader.GetDouble(11);
                    stk.PurchaseTax = reader.GetDouble(12);
                    stk.SupplierID = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    stk.SupplierName = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    stk.StoreLocation = reader.GetString(15);
                    stk.PresentStock = reader.GetDouble(16);
                    stk.StoreLocation = reader.GetString(17);
                    stk.QuantityBooked = reader.IsDBNull(18)?0:reader.GetDouble(18);
                    StockList.Add(stk);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying stock Details");
            }
            return StockList;
        }
        public static ListView getProductWiseStockListView(string pID, string ModNo)
        {
            ListView lv = new ListView();
            try
            {
                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                lv.Columns.Add("Sel", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Stk RefNo", 60, HorizontalAlignment.Left);
                lv.Columns.Add("StockItemID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItemName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ModelNo", 0, HorizontalAlignment.Left);
                lv.Columns.Add("ModelName", 0, HorizontalAlignment.Left);
                lv.Columns.Add("MRNNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("MRNDate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PresentStock", -2, HorizontalAlignment.Left);
                //----
                lv.Columns.Add("Booked Quant", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Avail Quant", -2, HorizontalAlignment.Left);
                //----
                lv.Columns.Add("BAtchNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SerialNO", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ExpiryDate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PurchaseQuant", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PurchasePrice", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PurchaseTax", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SupplierId", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SupplierName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Store", -2, HorizontalAlignment.Left);
                

                List<stock> StockList = StockDB.getProductWiseStockDetails(pID, ModNo);
                foreach (stock stk in StockList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = false;
                    item.SubItems.Add(stk.RowID.ToString());
                    item.SubItems.Add(stk.StockItemID);
                    item.SubItems.Add(stk.StockItemName);
                    item.SubItems.Add(stk.ModelNo);
                    item.SubItems.Add(stk.ModelName);
                    item.SubItems.Add(stk.MRNNo.ToString());
                    item.SubItems.Add(stk.MRNDate.ToShortDateString());
                    item.SubItems.Add(stk.PresentStock.ToString());
                    //--
                    item.SubItems.Add(stk.QuantityBooked.ToString());
                   
                    item.SubItems.Add((stk.PresentStock - stk.QuantityBooked).ToString());
                    item.SubItems[10].BackColor = Color.MediumSpringGreen;
                    item.UseItemStyleForSubItems = false;
                    //---
                    item.SubItems.Add(stk.BatchNo.ToString());
                    item.SubItems.Add(stk.SerielNo.ToString());
                    item.SubItems.Add(stk.ExpiryDate.ToShortDateString());
                    item.SubItems.Add(stk.PurchaseQuantity.ToString());
                    item.SubItems.Add(stk.PurchasePrice.ToString());
                    item.SubItems.Add(stk.PurchaseTax.ToString());
                    item.SubItems.Add(stk.SupplierID.ToString());
                    item.SubItems.Add(stk.SupplierName.ToString());
                    item.SubItems.Add(stk.StoreLocation.ToString());
                    lv.Items.Add(item);
                }

            }
            catch (Exception ex)
            {

            }
            return lv;
        }

        public static List<stock> getStockDetailForReport(int opt, string locID)
        {
            stock stk;
            List<stock> StockList = new List<stock>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query1 = "SELECT a.RowID, a.FYID, a.StockItemID, b.Name AS StockItemName, a.ModelNo, d.ModelName, b.Unit AS StockItemUnit," +
                            " a.InwardDocumentID, a.InwardDocumentNo, a.InwardDocumentDate, a.InwardQuantity, a.PresentStock, a.StockOnHold, " +
                            " a.PurchasePrice, a.StoreLocation,e.Description," +
                            " b.Level1GCode,b.Level2GCode,b.Level3GCode, " +
                            " b.Level1GDescription,b.Level2GDescription,b.Level3GDescription" +
                            " FROM dbo.Stock AS a LEFT OUTER JOIN " +
                            " dbo.ViewStockItem AS b ON a.StockItemID = b.StockItemID LEFT OUTER JOIN " +
                            " dbo.ProductModel AS d ON a.StockItemID = d.StockItemID AND a.ModelNo = d.ModelNo LEFT OUTER JOIN " +
                            " dbo.CatalogueValue as e on a.StoreLocation = e.CatalogueValueID "+
                            " and e.CatalogueID = 'StoreLocation' where FYID = '" + Main.currentFY + "' order by b.Name";

                string query2 = "SELECT a.RowID, a.FYID, a.StockItemID, b.Name AS StockItemName, a.ModelNo, d.ModelName, b.Unit AS StockItemUnit," +
                            " a.InwardDocumentID, a.InwardDocumentNo, a.InwardDocumentDate, a.InwardQuantity, a.PresentStock, a.StockOnHold, " +
                            " a.PurchasePrice, a.StoreLocation,e.Description," +
                            " b.Level1GCode,b.Level2GCode,b.Level3GCode, " +
                            " b.Level1GDescription,b.Level2GDescription,b.Level3GDescription" +
                            " FROM dbo.Stock AS a LEFT OUTER JOIN " +
                            " dbo.ViewStockItem AS b ON a.StockItemID = b.StockItemID LEFT OUTER JOIN " +
                            " dbo.ProductModel AS d ON a.StockItemID = d.StockItemID AND a.ModelNo = d.ModelNo LEFT OUTER JOIN " +
                            " dbo.CatalogueValue as e on a.StoreLocation = e.CatalogueValueID " +
                            " and e.CatalogueID = 'StoreLocation'" +
                            " where a.StoreLocation = '" + locID + "' and FYID = '" + Main.currentFY + "' order by b.Name";

                string query3 = "SELECT a.RowID, a.FYID, a.StockItemID, b.Name AS StockItemName, a.ModelNo, d.ModelName, b.Unit AS StockItemUnit," +
                            " a.InwardDocumentID, a.InwardDocumentNo, a.InwardDocumentDate, a.InwardQuantity, a.PresentStock, a.StockOnHold, " +
                            " a.PurchasePrice, a.StoreLocation,e.Description," +
                            " b.Level1GCode,b.Level2GCode,b.Level3GCode, " +
                            " b.Level1GDescription,b.Level2GDescription,b.Level3GDescription" +
                            " FROM dbo.Stock AS a LEFT OUTER JOIN" +
                            " dbo.ViewStockItem AS b ON a.StockItemID = b.StockItemID LEFT OUTER JOIN" +
                            " dbo.ProductModel AS d ON a.StockItemID = d.StockItemID AND a.ModelNo = d.ModelNo LEFT OUTER JOIN " +
                            " dbo.CatalogueValue as e on a.StoreLocation = e.CatalogueValueID "+
                            " and e.CatalogueID = 'StoreLocation'" +
                            " where   (a.PresentStock + a.StockOnHold > 0) and FYID = '" + Main.currentFY + 
                            "' order by b.Name";

                string query4 = "SELECT a.RowID, a.FYID, a.StockItemID, b.Name AS StockItemName, a.ModelNo, d.ModelName, b.Unit AS StockItemUnit," +
                            " a.InwardDocumentID, a.InwardDocumentNo, a.InwardDocumentDate, a.InwardQuantity, a.PresentStock, a.StockOnHold, " +
                            " a.PurchasePrice, a.StoreLocation,e.Description," +
                            " b.Level1GCode,b.Level2GCode,b.Level3GCode, " +
                            " b.Level1GDescription,b.Level2GDescription,b.Level3GDescription" +
                            " FROM dbo.Stock AS a LEFT OUTER JOIN" +
                            " dbo.ViewStockItem AS b ON a.StockItemID = b.StockItemID LEFT OUTER JOIN" +
                            " dbo.ProductModel AS d ON a.StockItemID = d.StockItemID AND a.ModelNo = d.ModelNo LEFT OUTER JOIN " +
                            " dbo.CatalogueValue as e on a.StoreLocation = e.CatalogueValueID " +
                            " and e.CatalogueID = 'StoreLocation'" +
                            " where   (a.PresentStock + a.StockOnHold > 0) and a.StoreLocation = '" + locID + 
                            "' and FYID = '" + Main.currentFY + "' order by b.Name";

                switch (opt)
                {
                    case 1:
                        query = query1;     //Filter1 : All, Filter2(Store) : All
                        break;
                    case 2:
                        query = query2;     //Filter1 : All, Filter2(Store) : Selected
                        break;
                    case 3:
                        query = query3;     //Filter1 : selected, Filter2(Store) : All
                        break;
                    case 4:
                        query = query4;     // Filter1 : selected, Filter2(Store) : selected
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
                    stk = new stock();
                    stk.RowID = reader.GetInt32(0);
                    stk.FYID = reader.GetString(1);
                    stk.StockItemID = reader.GetString(2);
                    stk.StockItemName = reader.GetString(3);
                    stk.ModelNo = reader.IsDBNull(4) ? "NA" : reader.GetString(4);
                    stk.ModelName = reader.IsDBNull(5) ? "NA" : reader.GetString(5);
                    stk.StockItemUnit = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    stk.InwardDocumentID = reader.GetString(7);
                    stk.InwardDocumentNo = reader.GetString(8);
                    stk.InwardDocumentDate = reader.GetDateTime(9);
                    stk.InwardQuantity = reader.IsDBNull(10) ? 0 : reader.GetDouble(10);
                    stk.PresentStock = reader.IsDBNull(11) ? 0 : reader.GetDouble(11);
                    stk.StockOnHold = reader.IsDBNull(12) ? 0 : reader.GetDouble(12);
                    stk.PurchasePrice = reader.IsDBNull(13) ? 0 : reader.GetDouble(13);
                    stk.StoreLocation = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    // For Store Location Name
                    stk.SupplierName = reader.IsDBNull(15) ? "" : reader.GetString(15); // For Store Location Name
                    stk.Level1GCode = reader.IsDBNull(16) ? "" : reader.GetString(16);
                    stk.Level2GCode = reader.IsDBNull(17) ? "" : reader.GetString(17);
                    stk.Level3GCode = reader.IsDBNull(18) ? "" : reader.GetString(18);
                    stk.Level1GDescription = reader.IsDBNull(19) ? "" : reader.GetString(19);
                    stk.Level2GDescription = reader.IsDBNull(20) ? "" : reader.GetString(20);
                    stk.Level3GDescription = reader.IsDBNull(21) ? "" : reader.GetString(21);

                    StockList.Add(stk);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying MRN Details");
            }
            return StockList;
        }

        public static double getAvailiableStockQuantity(int stockRefNo)
        {
            Boolean status = true;
            double PresentStock = 0;
            try
            {
                string query = "";
                int text = 0;
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select PresentStock" +
                   " from Stock where RowID =" + stockRefNo;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    PresentStock = reader.GetDouble(0);

                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Details");
            }
            return PresentStock;
        }
        public static List<TotalStock> TotalStock(string Location)
        {
            TotalStock tstk;
            List<TotalStock> tStockList = new List<TotalStock>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select stockitemid,presentstock from ViewPresentTotalStock where FYID = '" + 
                    Main.currentFY + "' and storelocation='" + Location+"'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tstk = new TotalStock();
                   
                    tstk.StockItemID = reader.GetString(0);
                    tstk.Stock = reader.GetDouble(1);

                    tStockList.Add(tstk);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Details");
            }
            return tStockList;
        }
        //Data Grid View FOr Factory Wise Stock Details
        public static DataGridView getGridViewOfFactoryWiseStock()
        {

            DataGridView grdStock = new DataGridView();
            try
            {
                string[] strColArr = { "StockRefNo", "StockItemID","StockItemName","ModelNo", "ModelName",
                    "MRNNo","MRNDate","PresentStock","BatchNo","SerielNo","ExpiryDate","PurchaseQuanity","PurchasePrice","PurchaseTax","SupplierID","SupplierName"};
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                     new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                };

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdStock.EnableHeadersVisualStyles = false;
                grdStock.AllowUserToAddRows = false;
                grdStock.AllowUserToDeleteRows = false;
                grdStock.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdStock.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdStock.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdStock.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdStock.ColumnHeadersHeight = 27;
                grdStock.RowHeadersVisible = false;
                grdStock.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;
                grdStock.Columns.Add(colChk);
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str;
                    colArr[index].HeaderText = str;
                    colArr[index].ReadOnly = true;
                    if (index == 6 || index == 10)
                        colArr[index].DefaultCellStyle.Format = "dd-MM-yyyy";
                    if (index == 2)
                        colArr[index].Width = 300;
                    else if (index == 15)
                        colArr[index].Width = 150;
                    else if (index == 11 || index == 12 || index == 13)
                        colArr[index].Width = 100;
                    else
                        colArr[index].Width = 80;

                    if (index == 1 || index == 3 || index == 4)
                        colArr[index].Visible = false;
                    else
                        colArr[index].Visible = true;
                    grdStock.Columns.Add(colArr[index]);
                }

                List<stock> StockList = StockDB.getFactoryStockDetails();
                foreach (stock stk in StockList)
                {
                    grdStock.Rows.Add();
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[0]].Value = stk.RowID;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[1]].Value = stk.StockItemID;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[2]].Value = stk.StockItemName;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[3]].Value = stk.ModelNo;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[4]].Value = stk.ModelName;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[5]].Value = stk.MRNNo;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[6]].Value = stk.MRNDate;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[7]].Value = stk.PresentStock;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[8]].Value = stk.BatchNo;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[9]].Value = stk.SerielNo;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[10]].Value = stk.ExpiryDate;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[11]].Value = stk.PurchaseQuantity;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[12]].Value = stk.PurchasePrice;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[13]].Value = stk.PurchaseTax;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[14]].Value = stk.SupplierID;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[15]].Value = stk.SupplierName;
                }
            }
            catch (Exception ex)
            {
            }

            return grdStock;
        }
        public static double getLastPurchasePriceForBOM(string id)
        {
            double price = 0;
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select top 1 PurchasePrice" +
                   " from Stock where StockItemId ='" + id + "'" +
                   " and InwardDocumentID = 'MRN'" +
                   " order by CreateTime desc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                //SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    price = reader.GetDouble(0);
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Details");
            }
            return price;
        }
        public static double getTotalItemWiseStock(string id, string modNo)
        {
            double stock = 0;
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                if (modNo != null)
                {
                    query = "select SUM(PresentStock)" +
                     " from Stock where StockItemId ='" + id + "'" + " and ModelNo = '" + modNo + "'" +
                     " and FYID = '" + Main.currentFY + 
                     "' and StoreLocation in ('" + Main.FactoryStore + "') group by StockItemID,ModelNo";
                }
                else
                {
                    query = "select SUM(PresentStock)" +
                    " from Stock where StockItemId ='" + id + "'" +
                    " and FYID = '" + Main.currentFY + 
                    "' and StoreLocation in ('" + Main.FactoryStore + "') group by StockItemID,ModelNo";
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                //SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                    stock = reader.GetDouble(0);
                conn.Close();
                //if (stock == 0)
                //    MessageBox.Show("Stock Item Not Found in Existing Stocks");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Details");
            }
            return stock;
        }
        public static List<stock> getStockDetailForStockIssue(string prodID, String ModelNo, double orderedQuant)
        {
            stock stk;
            double totordQuant = 0;
            List<stock> StockList = new List<stock>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                if (ModelNo == "NA" || ModelNo == null)
                {
                    query = "select RowID,StockItemID,StockItemName,ModelNo, ModelName,MRNNo,MRNDate,BatchNo,SerialNo,ExpiryDate," +
                    " PurchaseQuantity,PurchasePrice,PurchaseTax,SupplierId,SupplierName, StoreLocation,PresentStock,storeLocation,createtime " +
                    " from ViewStock where StockItemID = '" + prodID + "'" +
                    " and storelocation='" + Main.FactoryStore + "'" +
                    " and FYID = '" + Main.currentFY + 
                    "' and (ModelNo = 'NA' or ModelNo is null) order by createtime asc";
                }
                else
                {
                    query = "select RowID,StockItemID,StockItemName,ModelNo, ModelName,MRNNo,MRNDate,BatchNo,SerialNo,ExpiryDate," +
                   "PurchaseQuantity,PurchasePrice,PurchaseTax,SupplierId,SupplierName, StoreLocation,PresentStock,storeLocation,createtime " +
                 " from ViewStock where StockItemID = '" + prodID + "'" + " and storelocation = '" + Main.FactoryStore + "'" +
                 " and FYID = '" + Main.currentFY + "'"+
                 " and ModelNo = '" + ModelNo + "'  order by createtime asc";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stk = new stock();
                    stk.RowID = reader.GetInt32(0);
                    stk.StockItemID = reader.GetString(1);
                    stk.StockItemName = reader.GetString(2);
                    stk.ModelNo = reader.IsDBNull(3) ? "NA" : reader.GetString(3);
                    stk.ModelName = reader.IsDBNull(4) ? "NA" : reader.GetString(4);
                    stk.MRNNo = reader.GetInt32(5);
                    stk.MRNDate = reader.GetDateTime(6);
                    stk.BatchNo = reader.IsDBNull(7) ? "NA" : reader.GetString(7);
                    stk.SerielNo = reader.IsDBNull(8) ? "NA" : reader.GetString(8);
                    stk.ExpiryDate = reader.IsDBNull(9) ? Convert.ToDateTime("1900-01-01") : reader.GetDateTime(9);
                    stk.PurchaseQuantity = reader.GetDouble(10);
                    stk.PurchasePrice = reader.GetDouble(11);
                    stk.PurchaseTax = reader.GetDouble(12);
                    stk.SupplierID = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    stk.SupplierName = reader.IsDBNull(14) ? "" : reader.GetString(14);
                    stk.StoreLocation = reader.GetString(15);
                    stk.PresentStock = reader.GetDouble(16);
                    stk.StoreLocation = reader.GetString(17);
                    stk.CreateTime = reader.GetDateTime(18);

                    totordQuant = totordQuant + stk.PresentStock;
                    if (totordQuant >= orderedQuant) //Check Temp Order Total Quant  Exceed Issued Quantity
                    {
                        double dd = totordQuant - orderedQuant; //Quant Exceed
                        stk.StockOnHold = stk.PresentStock - dd; //replace prasent stock with quantity to be issue From currrent stock
                        StockList.Add(stk);
                        break;
                    }
                    else
                    {
                        stk.StockOnHold = stk.PresentStock; // Quanity of stock Allwed TO Order (Full)
                    }
                    StockList.Add(stk);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying stock Details");
            }
            return StockList;
        }

        //Stock OB Process Methods

        public Boolean createStockOB(string From, string to, int opt)
        {
            Boolean status = true;
            string utString = "";
            string tablename = "StockOB" + to.Replace('-', '_');
            try
            {
                string updateSQL = "";
                if (opt == 2)
                {
                    updateSQL = "IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + tablename + "')" +
                     " BEGIN drop table " + tablename + " END ";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                     ActivityLogDB.PrepareActivityLogQquerString("delete", tablename, "", updateSQL) +
                     Main.QueryDelimiter;
                }
                updateSQL = "Select * into " + tablename +
                    " from Stock where FYID = '" + From + "' and PresentStock > 0";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("insert", tablename, "", updateSQL) +
                 Main.QueryDelimiter;
                string docDate = "1900-01-01";
                try
                {
                    string fDates = FinancialYearDB.getFinancialYearDates(to);
                    string[] lst1 = fDates.Split(Main.delimiter1);
                    docDate = lst1[0];
                }
                catch (Exception)
                {
                }

                updateSQL = "update a " +
                   " set a.InwardDocumentID = 'OB' , a.InwardDocumentNo = '1' , a.InwardDocumentDate = '"+docDate+"' , a.InwardQuantity = 0 ,"+
                   " a.StockOnHold = 0 ,a.IssueQuantity = 0 , a.FYID = '" + to + "' from " + tablename + 
                   " a inner join " + tablename + " b on a.RowID = b.RowID";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("insert", tablename, "", updateSQL) +
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
        public Boolean CheckStockOBTableAvail(string to)
        {
            Boolean status = false;
            string query = "";
            SqlConnection conn = new SqlConnection(Login.connString);
            string tablename = "StockOB" + to.Replace('-', '_');
            try
            {
                query = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '" + tablename + "'" ;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    status = true;
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        public static List<stock> getStockDetailFromStockOBTable(string tablename)
        {
            stock stk;
            List<stock> StockList = new List<stock>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select a.RowID,a.StockItemID,b.Name as StockItemName,a.ModelNo, c.ModelName,a.MRNNo,a.MRnDate,a.BatchNo,a.SerialNo,a.ExpiryDate," +
                    "a.PurchaseQuantity,a.PurchasePrice,a.PurchaseTax,a.SupplierId, a.StoreLocation,a.PresentStock,a.InwardDocumentID,a.InwardDocumentNo,a.InwardDocumentDate, " +
                    "a.IssueQuantity , a.PurchaseReturnQuantity,a.Unit,a.StockOnHold,a.InwardQuantity,a.FYID, d.Name SupplierName from " + tablename + " a left outer join " +
                    " StockItem b on a.StockItemID = b.StockItemID left outer join ProductModel c on a.ModelNo = c.ModelNo" +
                    " left outer join Customer d on a.SupplierID = d.CustomerID order by StockItemId desc, InwardDocumentID desc";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stk = new stock();
                    stk.RowID = reader.GetInt32(0);
                    stk.StockItemID = reader.GetString(1);
                    stk.StockItemName = reader.GetString(2);
                    stk.ModelNo = reader.IsDBNull(3) ? "NA" : reader.GetString(3);
                    stk.ModelName = reader.IsDBNull(4) ? "NA" : reader.GetString(4);
                    stk.MRNNo = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);
                    stk.MRNDate = reader.IsDBNull(6) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(6);
                    stk.BatchNo = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    stk.SerielNo = reader.IsDBNull(8) ? "NA" : reader.GetString(8);
                    stk.ExpiryDate = reader.IsDBNull(9) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(9);
                    stk.PurchaseQuantity = reader.GetDouble(10);
                    stk.PurchasePrice = reader.GetDouble(11);
                    stk.PurchaseTax = reader.GetDouble(12);
                    stk.SupplierID = reader.IsDBNull(13) ? "NA" : reader.GetString(13);
                    //stk.SupplierName = reader.IsDBNull(14) ? "NA" : reader.GetString(14);
                    stk.StoreLocation = reader.GetString(14);
                    stk.PresentStock = reader.GetDouble(15);
                    stk.InwardDocumentID = reader.GetString(16);
                    stk.InwardDocumentNo = reader.IsDBNull(17) ? "" : reader.GetString(17);
                    stk.InwardDocumentDate = reader.IsDBNull(18) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(18);
                    stk.IssueQuantity = reader.IsDBNull(19) ? 0 : reader.GetDouble(19);
                    stk.PurchaseReturnQuantity = reader.IsDBNull(20) ? 0 : reader.GetDouble(20);
                    stk.StockItemUnit = reader.IsDBNull(21) ? "" : reader.GetString(21);
                    stk.StockOnHold = reader.IsDBNull(22) ? 0 : reader.GetDouble(22);
                    stk.InwardQuantity = reader.IsDBNull(23) ? 0 : reader.GetDouble(23);
                    stk.FYID = reader.GetString(24);
                    stk.SupplierName = reader.IsDBNull(25) ? "" : reader.GetString(25);
                    StockList.Add(stk);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock OB Details");
            }
            return StockList;
        }
        public static Boolean updateStockOB(List<stock> StockObList,string tablename)
        {
            Boolean status = true;
            string utString = "";
            try
            {

                string updateSQL = "";
                foreach (stock stk in StockObList)
                {
                    if (stk.RowID != 0)
                    {
                        updateSQL = "update " + tablename + " set StockItemID='" + stk.StockItemID +
                            "',InwardQuantity='" + stk.InwardQuantity +
                            "' where RowID=" + stk.RowID;
                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                        ActivityLogDB.PrepareActivityLogQquerString("update", tablename, "", updateSQL) +
                        Main.QueryDelimiter;
                    }
                    else
                    {
                        updateSQL = "insert into " + tablename + 
                                "(FYID,StockItemID,ModelNo,InwardDocumentID,InwardDocumentNo,InwardDocumentDate,InwardQuantity,PresentStock,MRNNo,MRNDate," +
                            "BatchNo,SerialNo,ExpiryDate,PurchaseQuantity,PurchasePrice,PurchaseTax,SupplierID,StoreLocation," +
                                "CreateTime,CreateUser,IssueQuantity,StockOnHold,PurchaseReturnQuantity)" +
                                " values (" +
                                "'" + stk.FYID + "'," +
                                "'" + stk.StockItemID + "'," +
                                "'" + stk.ModelNo + "'," +
                                "'" + stk.InwardDocumentID + "','" +
                                stk.InwardDocumentNo + "'," +
                                "'" + stk.InwardDocumentDate.ToString("yyyy-MM-dd") + "'," +
                                stk.InwardQuantity + "," +
                                '0' + ",'" +
                                stk.InwardDocumentNo + "'," +
                                "'" + stk.InwardDocumentDate.ToString("yyyy-MM-dd") + "'," +
                                "'" + stk.BatchNo + "'," +
                                "'" + stk.SerielNo + "'," +
                                "'" + stk.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                                stk.InwardQuantity + "," +
                                stk.PurchasePrice + "," +
                                stk.PurchaseTax + "," +
                                "'" + stk.SupplierID + "'," +
                                "'" + stk.StoreLocation + "'," +
                                "GETDATE()" + "," +
                                "'" + Login.userLoggedIn + "',0,0,0)";
                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                        ActivityLogDB.PrepareActivityLogQquerString("insert", tablename, "", updateSQL) +
                        Main.QueryDelimiter;
                    }
                }
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                    MessageBox.Show("Failed to update Stock OB");
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public static Boolean insertStockFromStockOB(List<stock> StockObList,DateTime startdate)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                foreach (stock stk in StockObList)
                {
                    updateSQL = "insert into Stock" + 
                               "(FYID,StockItemID,ModelNo,InwardDocumentID,InwardDocumentNo,InwardDocumentDate,InwardQuantity,PresentStock,MRNNo,MRNDate," +
                           "BatchNo,SerialNo,ExpiryDate,PurchaseQuantity,PurchasePrice,PurchaseTax,SupplierID,StoreLocation," +
                               "CreateTime,CreateUser,IssueQuantity,StockOnHold,PurchaseReturnQuantity)" +
                               " values (" +
                               "'" + stk.FYID + "'," +
                               "'" + stk.StockItemID + "'," +
                               "'" + stk.ModelNo + "'," +
                               "'OB','1'," +
                               "'" + startdate.ToString("yyyy-MM-dd") + "'," +
                               stk.InwardQuantity + "," +
                               stk.InwardQuantity + ",'" +
                               stk.MRNNo + "'," +
                               "'" + stk.MRNDate.ToString("yyyy-MM-dd") + "'," +
                               "'" + stk.BatchNo + "'," +
                               "'" + stk.SerielNo + "'," +
                               "'" + stk.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                               stk.PurchaseQuantity + "," +
                               stk.PurchasePrice + "," +
                               stk.PurchaseTax + "," +
                               "'" + stk.SupplierID + "'," +
                               "'" + stk.StoreLocation + "'," +
                               "GETDATE()" + "," +
                               "'" + Login.userLoggedIn + "',0,0,0)";
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
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        public static Boolean CheckStockOBAvailability(string FYID)
        {
            Boolean status = false;
            string query = "";
            SqlConnection conn = new SqlConnection(Login.connString);
            try
            {
                query = "SELECT Count(*) FROM Stock WHERE FYID = '" + FYID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int no = reader.GetInt32(0);
                    if(no > 0)
                    {
                        status = true;
                    }
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        public static List<ClosingStock> getStockDetailgroupbyitem(string tablename)
        {
            ClosingStock stk;
            List<ClosingStock> StockList = new List<ClosingStock>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select a.StockItemID,b.Name as StockItemName,SUM(a.InwardQuantity) obqty, SUM(a.InwardQuantity * a.PurchasePrice) obvalue," +
                    " SUM(a.PresentStock) cbqty, SUM(a.PresentStock * a.PurchasePrice) cbvalue" +
                    " from " + tablename + " a , StockItem b where a.StockItemID = b.StockItemID " +
                    "group by a.StockItemID,b.Name order by a.StockItemID desc ";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stk = new ClosingStock();
                    stk.StockItemID = reader.GetString(0);
                    stk.StockItemName = reader.GetString(1);
                    stk.OBQty = reader.IsDBNull(2) ? 0 : reader.GetDouble(2);
                    stk.OBValue = reader.IsDBNull(3) ? 0 : reader.GetDouble(3);
                    stk.CBQty = reader.IsDBNull(4) ? 0 : reader.GetDouble(4);
                    stk.CBValue = reader.IsDBNull(5) ? 0 : reader.GetDouble(5);
                    StockList.Add(stk);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock OB Details");
            }
            return StockList;
        }
    }
}
