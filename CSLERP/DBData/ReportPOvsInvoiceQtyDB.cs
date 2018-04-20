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
    class reportpovsinvoiceqty
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
    }

    class ReportPOvsInvoiceQtyDB
    {

        public static List<reportpovsinvoiceqty> QtyList()
        {
            reportpovsinvoiceqty povsinv;
            List<reportpovsinvoiceqty> QtyList = new List<reportpovsinvoiceqty>();
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
                    "from ViewPOvsInvoiceQtyTotal " +
                    "group by documentid, stockitemid, StockItemName " +
                    "order by documentid, stockitemid, StockItemName";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    povsinv = new reportpovsinvoiceqty();
                    povsinv.DocumentID = reader.GetString(0);
                    povsinv.StockItemID = reader.GetString(1); 
                    povsinv.StockItemName = reader.IsDBNull(2) ? "" : reader.GetString(2);
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


        public static List<reportpovsinvoiceqty> GetQtyDetail(string stockid)
        {
            reportpovsinvoiceqty qty;
            List<reportpovsinvoiceqty> QtyList = new List<reportpovsinvoiceqty>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);

                query = "select DocumentID, trackingno, trackingdate, ReferenceNo, customerpono, customerpodate, " +
                        "isnull(quantity,0),isnull(invqty, 0),CustomerName " +
                        "from ViewPOvsInvoiceQtyTotal where stockitemid='"+stockid+"' order by trackingno";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    qty = new reportpovsinvoiceqty();
                    
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
    }
}
