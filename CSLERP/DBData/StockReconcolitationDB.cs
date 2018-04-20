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
    class stockreconcolitation
    {
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public double Reciept { get; set; }
        public double Issue { get; set; }
        public double PresentStock { get; set; }
        public string DocumentID { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }  
    }
   
    class StockReconcolitationDB
    {
        
        public static List<stockreconcolitation> GetStockReconcolitationList()
        {
            stockreconcolitation stk;
            List<stockreconcolitation> StockList = new List<stockreconcolitation>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select a.STOCKITEMID,b.Name,a.QUANTITYRECEIPT,a.QUANTITYISSUE,a.PRESENTSTOCK "+
                         " from ViewStockReconciliationTotal a,StockItem b where a.STOCKITEMID=b.StockItemID "+
                         //////" and a.StoreLocation='" +Main.MainStore+"'"+
                         " order by a.STOCKITEMID";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stk = new stockreconcolitation();
                    stk.StockItemID = reader.GetString(0);
                    stk.StockItemName = reader.GetString(1);
                    stk.Reciept = reader.IsDBNull(2) ? 0 : reader.GetDouble(2);
                    stk.Issue = reader.IsDBNull(3) ? 0 : reader.GetDouble(3);
                    stk.PresentStock = reader.IsDBNull(4) ? 0 : reader.GetDouble(4);
                    StockList.Add(stk);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Reconcolitation Main");
            }
            return StockList;
        }


        public static List<stockreconcolitation> GetStockReconcolitationDetailList(string stockid)
        {
            stockreconcolitation stk;
            List<stockreconcolitation> StockList = new List<stockreconcolitation>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select b.DOCUMENTDATE,b.DOCUMENTID,b.DOCUMENTNO,b.QUANTITYRECEIPT,b.QUANTITYISSUE "+
                         " from ViewPresentTotalStock a, ViewStockReconciliation b where "+
                         " a.STOCKITEMID = b.StockItemID and b.STOCKITEMID = '"+stockid+"' "+
                         " and a.StoreLocation='"+Main.MainStore+"'"+
                         " order by b.DocumentDate,b.DocumentID";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stk = new stockreconcolitation();
                    stk.DocumentDate = reader.GetDateTime(0);
                    stk.DocumentID = reader.GetString(1);
                    stk.DocumentNo = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                    stk.Reciept = reader.IsDBNull(3) ? 0 : reader.GetDouble(3);
                    stk.Issue = reader.IsDBNull(4) ? 0 : reader.GetDouble(4);
                    StockList.Add(stk);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Reconcolitation Details");
            }
            return StockList;
        }
    }
}
