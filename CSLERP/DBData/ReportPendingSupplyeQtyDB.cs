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
    class reportpendingsupplyqty
    {
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public double POQty { get; set; }
        public double BilledQty { get; set; }
        public double BalanceQty { get; set; }
        public string DocumentID { get; set; }
        public int TrackingNo { get; set; }
        public DateTime TrackingDate { get; set; }
        public string ReferenceNo { get; set; }
        public string CustomerPoNo { get; set; }
        public DateTime CustomerPODate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerID { get; set; }
    }

    class ReportPendingSupplyDB
    {

        public static List<reportpendingsupplyqty> QtyList()
        {
            reportpendingsupplyqty povsinv;
            List<reportpendingsupplyqty> QtyList = new List<reportpendingsupplyqty>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                ////////query = "select a.STOCKITEMID,b.Name,a.QUANTITYRECEIPT,a.QUANTITYISSUE,a.PRESENTSTOCK " +
                ////////         " from ViewPresentTotalStock a,StockItem b where a.STOCKITEMID=b.StockItemID " +
                ////////         " order by a.STOCKITEMID";
                query = "select documentid, stockitemid, StockItemName, " +
                    "sum(isnull(quantity, 0)) POqty, sum(isnull(invqty, 0)) InvQty," +
                    "sum(isnull(quantity, 0)) - sum(isnull(invqty, 0)) InvQty " +
                    "from ViewPOvsInvoiceQtyTotal where documentid='POPRODUCTINWARD'" +
                    "group by documentid, stockitemid, StockItemName " +
                    "order by documentid, stockitemid, StockItemName";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    povsinv = new reportpendingsupplyqty();
                    povsinv.DocumentID = reader.GetString(0);
                    povsinv.StockItemID = reader.GetString(1); 
                    povsinv.StockItemName = reader.IsDBNull(2) ? "": reader.GetString(2);
                    povsinv.POQty = reader.IsDBNull(3) ? 0 : reader.GetDouble(3);
                    povsinv.BilledQty = reader.IsDBNull(4) ? 0 : reader.GetDouble(4);
                    povsinv.BalanceQty = reader.IsDBNull(5) ? 0 : reader.GetDouble(5);
                    QtyList.Add(povsinv);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Reconcolitation Main");
            }
            return QtyList;
        }


        public static List<reportpendingsupplyqty> GetQtyDetail(string stockid)
        {
            reportpendingsupplyqty qty;
            List<reportpendingsupplyqty> QtyList = new List<reportpendingsupplyqty>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);

                query = "select DocumentID, trackingno, trackingdate, ReferenceNo, customerpono, customerpodate, " +
                        "isnull(quantity,0),isnull(invqty, 0),CustomerName " +
                        "from ViewPOvsInvoiceQtyTotal where stockitemid='"+stockid+"'"+
                        " and documentid='POPRODUCTINWARD' order by trackingno";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    qty = new reportpendingsupplyqty();
                    
                    qty.DocumentID = reader.GetString(0);
                    qty.TrackingNo = reader.GetInt32(1); 
                    qty.TrackingDate = reader.GetDateTime(2);
                    qty.ReferenceNo = reader.GetString(3);
                    qty.CustomerPoNo = reader.GetString(4);
                    qty.CustomerPODate = reader.GetDateTime(5);
                    
                    qty.POQty = reader.IsDBNull(6) ? 0 : reader.GetDouble(6);
                    qty.BilledQty = reader.IsDBNull(7) ? 0 : reader.GetDouble(7);
                    qty.CustomerName = reader.GetString(8);
                    QtyList.Add(qty);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Quantity Details");
            }
            return QtyList;
        }

        public static List<reportpendingsupplyqty> getPendingSupplyList()
        {
            reportpendingsupplyqty pendingSupQty;
            List<reportpendingsupplyqty> pendingSupQtyList = new List<reportpendingsupplyqty>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);

                query = "select a.POno,a.POdate,a.stockitemid,a.qtyordered,isnull(b.qtyreceived, 0) qtyreceived, " +
                    "(isnull(qtyOrdered, 0) - isnull(qtyReceived, 0)) pendingsupply " +
                    "from viewpoqtyordered as a left join  ViewPOQtyReceived as b " +
                    "on(a.pono = b.pono and a.podate = b.podate and a.stockitemid = b.stockitemid)";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pendingSupQty = new reportpendingsupplyqty();
                    pendingSupQty.TrackingNo = reader.GetInt32(0); //For PONO
                    pendingSupQty.TrackingDate = reader.GetDateTime(1); //For PODate
                    pendingSupQty.StockItemID = reader.IsDBNull(2) ? "" : reader.GetString(2); //For StockITemID
                    pendingSupQty.POQty = reader.IsDBNull(3) ? 0 : reader.GetDouble(3);  //Quanity Ordered
                    pendingSupQty.BilledQty = reader.IsDBNull(4) ? 0 : reader.GetDouble(4);  //Quantity Received
                    pendingSupQty.BalanceQty = reader.IsDBNull(5) ? 0 : reader.GetDouble(5);  //Pending Supply
                    pendingSupQtyList.Add(pendingSupQty);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Reconcolitation");
            }
            return pendingSupQtyList;
        }

        public static List<reportpendingsupplyqty> GetInTransitQtyDetail(string stockid)
        {
            reportpendingsupplyqty qty;
            List<reportpendingsupplyqty> QtyList = new List<reportpendingsupplyqty>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);

                query = "select a.pono,a.podate,c.DeliveryPeriod,c.CustomerID,d.Name customername, a.stockitemid,a.qtyordered,isnull(b.qtyReceived, 0) qtyreceived," +
                        "(a.qtyordered - isnull(b.qtyReceived, 0)) pendingsupply " +
                        "from ViewPOQtyOrdered as a left join viewpoqtyreceived as b " +
                        "on (a.pono = b.pono and a.podate = b.podate and a.stockitemid = b.stockitemid) " +
                        "left join POHeader as c on (a.PONo = c.pono and a.podate = c.podate) " +
                        "left join customer as d on c.CustomerID = d.CustomerID where a.stockitemid = '" + stockid + "'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    qty = new reportpendingsupplyqty();
                    qty.TrackingNo = reader.GetInt32(0);  //For PONO
                    qty.TrackingDate = reader.GetDateTime(1);  // For PODate
                    qty.CustomerPODate = reader.GetDateTime(1).AddDays(reader.GetInt32(2)); //Target Date
                    qty.CustomerID = reader.GetString(3);
                    qty.CustomerName = reader.GetString(4);
                    qty.StockItemID = reader.GetString(5);

                    qty.POQty = reader.IsDBNull(6) ? 0 : reader.GetDouble(6); //Ordered Quantity
                    qty.BilledQty = reader.IsDBNull(7) ? 0 : reader.GetDouble(7); //Received Quantity
                    qty.BalanceQty = reader.IsDBNull(8) ? 0 : reader.GetDouble(8); // Supply Pendding Quantity
                  
                    QtyList.Add(qty);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Quantity Details");
            }
            return QtyList;
        }
    }
}
