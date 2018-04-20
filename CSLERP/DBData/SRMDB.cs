using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class SRM
    {
        public String StockItmID { get; set; }
        public int StockReferenceNo { get; set; }
        public String DOCUMENTID { get; set; }
        public String DOCUMENTNO { get; set; }
        public DateTime DOCUMENTDATE { get; set; }
        public double OB { get; set; }
        public double MRN { get; set; }
        public double GTNIN { get; set; }
        public double INVOICE { get; set; }
        public double GTNOUT { get; set; }
        public String STOREID { get; set; }
        public String FYID { get; set; }
        public int DOCUMENTNO2 { get; set; }
        public double QUANTITYISSUE { get; set; }
        public double PresentStock { get; set; }
        public double CalcStock { get; set; }
        public double issuequantity { get; set; }
        public double CalcIssueQuantity { get; set; }
        public String Name { get; set; }
    }

    class SRMDB
    {
        ActivityLogDB alDB = new ActivityLogDB();


        public List<SRM> getSRMdata()
        {
            SRM srm;
            List<SRM> SRMlist = new List<SRM>();
            try
            {
                string query = "SELECT a.StockItemID, a.RowID AS StockReferenceNo, a.InwardDocumentID AS DOCUMENTID, "+
                    "a.InwardDocumentNo AS DOCUMENTNO, a.InwardDocumentDate AS DOCUMENTDATE, a.InwardQuantity AS OB,"+
                    "cast(0 as float) MRN, cast(0 as float) GTNIN, cast(0 as float) AS INVOICE,cast(0 as float) as GTNOUT, "+
                    "a.StoreLocation AS STOREID,PresentStock,issuequantity, " +
                    "b.Name,a.FYID " +
                    "FROM  dbo.Stock AS a, stockitem b, FinancialYear c " +
                    "where StoreLocation = 'NVPSTORE'and a.StockItemID=b.StockItemID and a.FYID=c.FYID and c.IsCurrentFY=1";

                SqlConnection conn = new SqlConnection(Login.connString);              
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    srm = new SRM();
                    srm.StockItmID = reader.GetString(0);
                    srm.StockReferenceNo = reader.GetInt32(1);
                    srm.DOCUMENTID = reader.GetString(2);
                    srm.DOCUMENTNO = reader.GetString(3);
                    srm.DOCUMENTDATE = reader.GetDateTime(4);
                    srm.OB = reader.GetDouble(5);
                    srm.MRN = reader.GetDouble(6);
                    srm.GTNIN = reader.GetDouble(7);
                    srm.INVOICE = reader.GetDouble(8);
                    srm.GTNOUT = reader.GetDouble(9);
                    srm.STOREID = reader.GetString(10);                  
                    srm.PresentStock = reader.GetDouble(11);                  
                    srm.issuequantity = reader.GetDouble(12);
                    srm.Name = reader.GetString(13);
                    srm.FYID = reader.GetString(14);

                    SRMlist.Add(srm);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock  Data");
            }
            return SRMlist;
        }
        public List<SRM> getSRMMRNdata()
        {
            SRM srm;
            List<SRM> SRMlist = new List<SRM>();
            try
            {
        string query = "SELECT StockItemID,0 as StockReferenceNo,DocumentID,DOCUMENTNO ,DOCUMENTDATE ,QUANTITYRECEIPT,QUANTITYISSUE ,STOREID ,FYID FROM ViewSRMMRN"; 

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    srm = new SRM();
                    srm.StockItmID = reader.GetString(0);
                    srm.StockReferenceNo = reader.GetInt32(1);
                    srm.DOCUMENTID = reader.GetString(2);
                    srm.DOCUMENTNO2 = reader.GetInt32(3);
                    srm.DOCUMENTDATE = reader.GetDateTime(4);                  
                    srm.MRN = reader.GetDouble(5);
                    srm.QUANTITYISSUE = reader.GetDouble(6);
                    srm.STOREID = reader.GetString(7);
                    srm.FYID = reader.GetString(8);
                    SRMlist.Add(srm);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying SMRN  Data");
            }
            return SRMlist;
        }

        public List<SRM> getSRMGTNINdata()
        {
            SRM srm;
            List<SRM> SRMlist = new List<SRM>();
            try
            {
                string query = "SELECT StockItemID,StockReferenceNo,DocumentID,DOCUMENTNO ,DOCUMENTDATE ,QUANTITYRECEIPT,QUANTITYISSUE ,STOREID ,FYID FROM ViewSRMGTNIN";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    srm = new SRM();
                    srm.StockItmID = reader.GetString(0);
                    srm.StockReferenceNo = reader.GetInt32(1);
                    srm.DOCUMENTID = reader.GetString(2);
                    srm.DOCUMENTNO2 = reader.GetInt32(3);
                    srm.DOCUMENTDATE = reader.GetDateTime(4);
                    srm.GTNIN = reader.GetDouble(5);
                    srm.QUANTITYISSUE = reader.GetDouble(6);
                    srm.STOREID = reader.GetString(7);
                    srm.FYID = reader.GetString(8);
                    SRMlist.Add(srm);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying SMRN  Data");
            }
            return SRMlist;
        }
        public List<SRM> getSRMinvdata()
        {
            SRM srm;
            List<SRM> SRMlist = new List<SRM>();
            try
            {
                string query = "SELECT StockItemID,StockReferenceNo,DocumentID,DOCUMENTNO ,DOCUMENTDATE ,QUANTITYRECEIPT,QUANTITYISSUE ,STOREID ,FYID FROM ViewSRMInv";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    srm = new SRM();
                    srm.StockItmID = reader.GetString(0);
                    srm.StockReferenceNo = reader.GetInt32(1);
                    srm.DOCUMENTID = reader.GetString(2);
                    srm.DOCUMENTNO2 = reader.GetInt32(3);
                    srm.DOCUMENTDATE = reader.GetDateTime(4);
                    srm.GTNIN = reader.GetDouble(5);
                    srm.QUANTITYISSUE = reader.GetDouble(6);
                    srm.STOREID = reader.GetString(7);
                    srm.FYID = reader.GetString(8);
                    SRMlist.Add(srm);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying SMRN  Data");
            }
            return SRMlist;
        }
        public List<SRM> getSRMGTNOUTdata()
        {
            SRM srm;
            List<SRM> SRMlist = new List<SRM>();
            try
            {
                string query = "SELECT StockItemID,StockReferenceNo,DocumentID,DOCUMENTNO ,DOCUMENTDATE ,QUANTITYRECEIPT,QUANTITYISSUE ,STOREID ,FYID FROM ViewSRMGTNOUT";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    srm = new SRM();
                    srm.StockItmID = reader.GetString(0);
                    srm.StockReferenceNo = reader.GetInt32(1);
                    srm.DOCUMENTID = reader.GetString(2);
                    srm.DOCUMENTNO2 = reader.GetInt32(3);
                    srm.DOCUMENTDATE = reader.GetDateTime(4);
                    srm.GTNIN = reader.GetDouble(5);
                    srm.QUANTITYISSUE = reader.GetDouble(6);
                    srm.STOREID = reader.GetString(7);
                    srm.FYID = reader.GetString(8);
                    SRMlist.Add(srm);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying SMRN  Data");
            }
            return SRMlist;
        }
    }
}
