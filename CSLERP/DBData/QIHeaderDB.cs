using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class qiheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public string QuotationNo { get; set; }
        public DateTime QuotationDate { get; set; }
        public int ValidityDays { get; set; }
        public string CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public String PaymentTerms { get; set; }
        public String PaymentMode { get; set; }
        public int CreditPeriod { get; set; }
        public int Status { get; set; }
        public string CreateUser { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int DocumentStatus { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string ForwarderName { get; set; }
        public string ApproverName { get; set; }
        public string OfficeID { get; set; }
        public string CommentStatus { get; set; }
        public string Comments { get; set; }
        public string ForwarderList { get; set; }

    }
    class qidetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public string ModelName { get; set; }
        public string ModelNo { get; set; }
        public string ModelDetails { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public string TaxCode { get; set; }
        public double Tax { get; set; }
        public int WarrantyDays { get; set; }
        public string TaxDetails { get; set; }

    }
    class qipricedetail
    {
        public string DocumentID { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string QuotationNo { get; set; }
        public DateTime QuotationDate { get; set; }
        public string CustomerName { get; set; }
        public string StockItemID { get; set; }
        public double Quantity { get; set; }
        public double PricePerUnit { get; set; }
        public double TaxePerUnit { get; set; }
        public double TotalPricePerUnit { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int WarrantyDays { get; set; }
    }
    class QIHeaderDB
    {
        public List<qiheader> getFilteredQIHeader(string userList, int opt, string userCommentStatusString)
        {
            qiheader qih;
            List<qiheader> QIHeaders = new List<qiheader>();
            string acStr = "";
            try
            {
                acStr = userCommentStatusString.Substring(0, userCommentStatusString.Length - 2) + "1" + Main.delimiter2;
            }
            catch (Exception ex)
            {
                acStr = "";
            }
            try
            {
                string query1= "select RowID, DocumentID, DocumentName," +
                    " CustomerID,CustomerName,TemporaryNo, TemporaryDate, DocumentNo,DocumentDate,QuotationNo,QuotationDate,ValidityDays," +
                    " PaymentTerms,ISNULL(PaymentMode,' ') as PaymentMode,CreditPeriod,CurrencyID,CurrencyName,Status,CreateUser,CreatorName,CreateTime, " +
                    "DocumentStatus,ForwardUser, ApproveUser, ForwarderName, ApproverName, CommentStatus, ForwarderList"+
                    " from ViewQIHeader" +
                     " where ((ForwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (Createuser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (CommentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";
                string query2 = "select RowID, DocumentID, DocumentName," +
                    " CustomerID,CustomerName,TemporaryNo, TemporaryDate, DocumentNo,DocumentDate,QuotationNo,QuotationDate,ValidityDays," +
                    " PaymentTerms,ISNULL(PaymentMode,' ') as PaymentMode,CreditPeriod,CurrencyID,CurrencyName,Status,CreateUser,CreatorName,CreateTime, " +
                    "DocumentStatus,ForwardUser, ApproveUser, ForwarderName, ApproverName, CommentStatus, ForwarderList" +
                   " from ViewQIHeader" +
                    " where ((Createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (CommentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";
                string query3 = "select RowID, DocumentID, DocumentName," +
                    " CustomerID,CustomerName,TemporaryNo, TemporaryDate, DocumentNo,DocumentDate,QuotationNo,QuotationDate,ValidityDays," +
                    " PaymentTerms,ISNULL(PaymentMode,' ') as PaymentMode,CreditPeriod,CurrencyID,CurrencyName,Status,CreateUser,CreatorName,CreateTime, " +
                    "DocumentStatus,ForwardUser, ApproveUser, ForwarderName, ApproverName, CommentStatus, ForwarderList" +
                   " from ViewQIHeader" +
                    " where ((CreateUser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or CommentStatus like '%" + acStr + "%'" +
                    " or ApproveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99)  order by DocumentDate desc,DocumentID asc,DocumentNo desc";
                string query6 = "select RowID, DocumentID, DocumentName," +
                    " CustomerID,CustomerName,TemporaryNo, TemporaryDate, DocumentNo,DocumentDate,QuotationNo,QuotationDate,ValidityDays," +
                    " PaymentTerms,ISNULL(PaymentMode,' ') as PaymentMode,CreditPeriod,CurrencyID,CurrencyName,Status,CreateUser,CreatorName,CreateTime, " +
                    "DocumentStatus, ForwardUser, ApproveUser, ForwarderName, ApproverName, CommentStatus, ForwarderList" +
                   " from ViewQIHeader" +
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
                    qih = new qiheader();
                    qih.RowID = reader.GetInt32(0);
                    qih.DocumentID = reader.GetString(1);
                    qih.DocumentName = reader.GetString(2);
                    qih.CustomerID = reader.GetString(3);
                    qih.CustomerName = reader.GetString(4);
                    qih.TemporaryNo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        qih.TemporaryDate = reader.GetDateTime(6);
                    }
                    
                    qih.DocumentNo = reader.GetInt32(7);
                    qih.DocumentDate = reader.GetDateTime(8);
                    qih.QuotationNo = reader.GetString(9);
                    qih.QuotationDate = reader.GetDateTime(10);
                    qih.ValidityDays = reader.GetInt32(11);                  
                    qih.PaymentTerms = reader.GetString(12);
                    qih.PaymentMode = reader.GetString(13);
                    qih.CreditPeriod = reader.GetInt32(14);
                    qih.CurrencyID = reader.GetString(15);
                    qih.CurrencyName = reader.GetString(16);
                    qih.Status = reader.GetInt32(17);
                    qih.CreateUser = reader.GetString(18);
                    qih.CreatorName = reader.GetString(19);
                    qih.CreateTime = reader.GetDateTime(20);
                    qih.DocumentStatus = reader.GetInt32(21);
                    qih.ForwardUser = reader.GetString(22);
                    qih.ApproveUser = reader.GetString(23);
                    qih.ForwarderName = reader.GetString(24);
                    qih.ApproverName = reader.GetString(25);
                    qih.CommentStatus = reader.GetString(26);
                    qih.ForwarderList = reader.GetString(27);
                    QIHeaders.Add(qih);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Quotation Inward Details");
            }
            return QIHeaders;
        }

    

        public static List<qidetail> getQIDetail(qiheader qih)
        {
            qidetail qid;
            List<qidetail> QIDetail = new List<qidetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,TemporaryNo,TemporaryDate,StockItemID,StockItemName,ModelNo,ModelName,ModelDetails, " +
                   "Quantity,Price,TaxCode,Tax,WarrantyDays,TaxDetails " +
                   "from ViewQIDetail " +
                    " where DocumentID='" + qih.DocumentID + "'" +
                    " and TemporaryNo=" + qih.TemporaryNo +
                    " and TemporaryDate='" + qih.TemporaryDate.ToString("yyyy-MM-dd") + "'"+
                " order by StockItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    qid = new qidetail();
                    qid.RowID = reader.GetInt32(0);
                    qid.DocumentID = reader.GetString(1);
                    qid.TemporaryNo = reader.GetInt32(2);
                    qid.TemporaryDate = reader.GetDateTime(3).Date;
                    qid.StockItemID = reader.GetString(4);
                    qid.StockItemName = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                    {
                        qid.ModelNo = reader.GetString(6);
                    }
                    
                    if (!reader.IsDBNull(7))
                    {
                        qid.ModelName = reader.GetString(7);
                    }
                    else
                        qid.ModelName = "NA";

                    qid.ModelDetails = reader.GetString(8);
                    qid.Quantity = reader.GetDouble(9);
                    qid.Price = reader.GetDouble(10);
                    qid.TaxCode = reader.GetString(11);
                    qid.Tax = reader.GetDouble(12);
                    qid.WarrantyDays = reader.GetInt32(13);
                    qid.TaxDetails = reader.GetString(14);
                    QIDetail.Add(qid);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Quotation Inward Details");
            }
            return QIDetail;
        }

        public static List<qipricedetail> getQIPriceDetail(string stockid)
        {
            qipricedetail qipd;
            List<qipricedetail> QIPriceDetail = new List<qipricedetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select a.DocumentID,a.CustomerName,a.TemporaryNo,a.TemporaryDate,a.QuotationNo,a.QuotationDate,"+
                   " a.StockItemID,a.Quantity, a.PricePerUnit, a.TaxPerUnit,a.TotalPricePerUnit," +
                   " a.ExpiryDate,a.WarrantyDays " +
                   " from viewQIPriceDetail a where " +
                   " a.expiryDate >= convert(date, getdate()) " +
                   " and a.StockItemID='" + stockid + "'" +
                   " order by a. TotalPricePerUnit";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    qipd = new qipricedetail();
                    qipd.DocumentID = reader.GetString(0);
                    qipd.CustomerName = reader.GetString(1);
                    qipd.TemporaryNo = reader.GetInt32(2);
                    qipd.TemporaryDate = reader.GetDateTime(3).Date;
                    qipd.QuotationNo = reader.GetString(4);
                    qipd.QuotationDate = reader.GetDateTime(5).Date;
                    qipd.StockItemID = reader.GetString(6);
                    qipd.Quantity = reader.GetDouble(7);
                    qipd.PricePerUnit = reader.GetDouble(8);
                    qipd.TaxePerUnit = reader.GetDouble(9);
                    qipd.TotalPricePerUnit = reader.GetDouble(10);
                    qipd.ExpiryDate = reader.GetDateTime(11).Date;
                    qipd.WarrantyDays = reader.GetInt32(12);
                    QIPriceDetail.Add(qipd);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Quotation Inward Price Details");
            }
            return QIPriceDetail;
        }
        //--

        public static List<qiheader> getQIHeaderSelctionView(string CustomerID)
        {
            qiheader qid;
            List<qiheader> QIList = new List<qiheader>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select a.CustomerID,a.DocumentDate,a.QuotationNo,a.QuotationDate, " + 
                   " DATEADD(d, a.ValidityDays, a.QuotationDate) AS ExpiryDate " +
                    " from QIHeader a where a.CustomerID = '" + CustomerID +
                    "' and DATEADD(d, a.ValidityDays, a.QuotationDate) >= convert(date, getdate())";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    qid = new qiheader();
                    qid.CustomerID = reader.GetString(0);
                    qid.DocumentDate = reader.GetDateTime(1).Date;
                    qid.QuotationNo = reader.GetString(2);
                    qid.QuotationDate = reader.GetDateTime(3).Date;
                    qid.ExpiryDate = reader.GetDateTime(4).Date;
                    QIList.Add(qid);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Quotation Inward header Details");
            }
            return QIList;
        }

        public Boolean validateQIHeader(qiheader qih)
        {
            Boolean status = true;
            try
            {
                if (qih.DocumentID.Trim().Length == 0 || qih.DocumentID == null)
                {
                    return false;
                }
               
                if (qih.CustomerID.Trim().Length == 0 || qih.CustomerID == null)
                {
                    return false;
                }
                if (qih.QuotationNo.Trim().Length == 0 || qih.QuotationNo == null)
                {
                    return false;
                }
                if (qih.QuotationDate == null || qih.QuotationDate.Date > DateTime.Now.Date || 
                    (DateTime.Now.Date - qih.QuotationDate.Date).TotalDays > 90 )
                {
                    return false;
                }
                if (qih.ValidityDays == 0 || qih.ValidityDays < 0)
                {
                    return false;
                }
                if(qih.CurrencyID == null)
                {
                    return false;
                }
                if (qih.PaymentTerms == null)
                {
                    return false;
                }
                if (qih.PaymentMode == null)
                {
                    return false;
                }
                if (qih.PaymentTerms == "Credit")
                {
                    if (qih.CreditPeriod == 0)
                    {
                        return false;
                    }
                }
                //if (qih.Status == 0)
                //{
                   
                //}
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public static ListView QIPriceSelectionView(string stockid)
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
               
                List<qipricedetail> QIPriceDetail = getQIPriceDetail(stockid);
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Customer", -2, HorizontalAlignment.Left);
                lv.Columns.Add("TempNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("TempDate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Price", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Tax", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Total", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Expiry", -2, HorizontalAlignment.Center);

                foreach (qipricedetail qipd in QIPriceDetail)
                {
                    {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        item1.SubItems.Add(qipd.CustomerName.ToString());
                        item1.SubItems.Add(qipd.QuotationNo.ToString());
                        item1.SubItems.Add(qipd.QuotationDate.ToString("dd-MM-yyyy"));
                        item1.SubItems.Add(qipd.PricePerUnit.ToString());
                        item1.SubItems.Add(qipd.TaxePerUnit.ToString());
                        item1.SubItems.Add(qipd.TotalPricePerUnit.ToString());
                        item1.SubItems.Add(qipd.ExpiryDate.ToString("dd-MM-yyyy"));
                        lv.Items.Add(item1);
                    }
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static ListView ReferenceQuotationSelctionView(string CustomerID)
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

                List<qiheader> QIDetail = getQIHeaderSelctionView(CustomerID);
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                //lv.Columns.Add("CustomerID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Doc Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Quot No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Quot Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Expiry", -2, HorizontalAlignment.Center);

                foreach (qiheader qid in QIDetail)
                {
                    {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        //item1.SubItems.Add(qid.CustomerID.ToString());
                        item1.SubItems.Add(qid.DocumentDate.ToString("dd-MM-yyyy"));
                        item1.SubItems.Add(qid.QuotationNo.ToString());
                        item1.SubItems.Add(qid.QuotationDate.ToString("dd-MM-yyyy"));
                        item1.SubItems.Add(qid.ExpiryDate.ToString("dd-MM-yyyy"));
                        lv.Items.Add(item1);
                    }
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static string getUserComments(string docid, int dno, DateTime ddate)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from QIHeader where DocumentID='" + docid + "'" +
                       " and TemporaryNo=" + dno +
                    " and TemporaryDate='" + ddate.ToString("yyyy-MM-dd") + "'";
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
        public Boolean forwardQI(qiheader qih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update QIHeader set DocumentStatus=" + (qih.DocumentStatus + 1) +
                    ", forwardUser='" + qih.ForwardUser + "'" +
                    ", commentStatus='" + qih.CommentStatus + "'" +
                    ", ForwarderList='" + qih.ForwarderList + "'" +
                   " where DocumentID='" + qih.DocumentID + "'" +
                    " and TemporaryNo=" + qih.TemporaryNo +
                    " and TemporaryDate='" + qih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "QIHeader", "", updateSQL) +
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

        public Boolean reverseQI(qiheader qih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update QIHeader set DocumentStatus=" + qih.DocumentStatus +
                    ", forwardUser='" + qih.ForwardUser + "'" +
                    ", commentStatus='" + qih.CommentStatus + "'" +
                    ", ForwarderList='" + qih.ForwarderList + "'" +
                    " where DocumentID='" + qih.DocumentID + "'" +
                    " and TemporaryNo=" + qih.TemporaryNo +
                    " and TemporaryDate='" + qih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "QIHeader", "", updateSQL) +
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

        public Boolean ApproveQI(qiheader qih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update QIHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + qih.CommentStatus + "'" +
                    ", DocumentNo=" + qih.DocumentNo +
                    ", DocumentDate = convert(date, getdate())" +
                     " where DocumentID='" + qih.DocumentID + "'" +
                    " and TemporaryNo=" + qih.TemporaryNo +
                    " and TemporaryDate='" + qih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "QIHeader", "", updateSQL) +
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
        public Boolean updateQIHeaderAndDetail(qiheader qih, qiheader prevqih, List<qidetail> QIDetail)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update QIHeader set CustomerID='" + qih.CustomerID +
                    "',DocumentNo='" + qih.DocumentNo +
                     "',DocumentDate='" + qih.DocumentDate.ToString("yyyy-MM-dd") +
                    "', QuotationNo='" + qih.QuotationNo +
                    "',QuotationDate='" + qih.QuotationDate.ToString("yyyy-MM-dd") +
                    "', ValidityDays=" + qih.ValidityDays +
                    ", CurrencyID='" + qih.CurrencyID + "'" +
                    ",PaymentTerms='" + qih.PaymentTerms + "'" +
                    ",PaymentMode='" + qih.PaymentMode + "'," +
                    "CreditPeriod= " + qih.CreditPeriod +
                    ", Status =" + qih.Status +
                    ", CommentStatus='" + qih.CommentStatus +
                    "', Comments='" + qih.Comments +
                    "', ForwarderList='" + qih.ForwarderList + "'" +
                   " where DocumentID='" + prevqih.DocumentID + "'" +
                    " and TemporaryNo=" + prevqih.TemporaryNo +
                    " and TemporaryDate='" + prevqih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "QIHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from QIDetail where DocumentID='" + prevqih.DocumentID + "'" +
                     " and TemporaryNo=" + prevqih.TemporaryNo +
                     " and TemporaryDate='" + prevqih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "QIDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (qidetail qid in QIDetail)
                {
                    updateSQL = "insert into QIDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,ModelDetails,Quantity,Price,TaxCode,Tax,WarrantyDays,TaxDetails) " +
                    "values ('" + qid.DocumentID + "'," +
                    qid.TemporaryNo + "," +
                    "'" + qid.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + qid.StockItemID + "'," +
                     "'" + qid.ModelNo + "'," +
                    "'" + qid.ModelDetails + "'," +
                    qid.Quantity + "," +
                    qid.Price + " ," +
                    "'" + qid.TaxCode + "'," +
                    qid.Tax + "," +
                    qid.WarrantyDays + "," +
                    "'" + qid.TaxDetails + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "QIDetail", "", updateSQL) +
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
        public Boolean InsertQIHeaderAndDetail(qiheader qih, List<qidetail> QIDetail)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                qih.TemporaryNo = DocumentNumberDB.getNumber(qih.DocumentID, 1);
                if (qih.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + qih.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + qih.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into QIHeader " +
                     "(DocumentID,CustomerID,TemporaryNo, TemporaryDate,DocumentNo,DocumentDate," +
                     "QuotationNo,QuotationDate,ValidityDays,CurrencyID,PaymentTerms,PaymentMode,CreditPeriod,Status,DocumentStatus,CreateUser,CreateTime," +
                     "Comments,ForwarderList,CommentStatus)" +

                     " values (" +
                     "'" + qih.DocumentID + "'," +
                     "'" + qih.CustomerID + "'," +
                     qih.TemporaryNo + "," +
                     "'" + qih.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     qih.DocumentNo + "," +
                     "'" + qih.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + qih.QuotationNo + "'," +
                     "'" + qih.QuotationDate.ToString("yyyy-MM-dd") + "'," +
                     qih.ValidityDays + "," +
                     "'" + qih.CurrencyID + "'," +
                     "'" + qih.PaymentTerms + "'," +
                     "'" + qih.PaymentMode + "'," +
                     qih.CreditPeriod + "," +
                     qih.Status + "," +
                     qih.DocumentStatus + "," +
                     "'" + Login.userLoggedIn + "'," +
                     "GETDATE()" +
                     ",'" + qih.Comments + "'," +
                     "'" + qih.ForwarderList + "'," +
                     "'" + qih.CommentStatus + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "QIHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from QIDetail where DocumentID='" + qih.DocumentID + "'" +
                   " and TemporaryNo=" + qih.TemporaryNo +
                   " and TemporaryDate='" + qih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "QIDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (qidetail qid in QIDetail)
                {
                    updateSQL = "insert into QIDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,ModelDetails,Quantity,Price,TaxCode,Tax,WarrantyDays,TaxDetails) " +
                    "values ('" + qid.DocumentID + "'," +
                    qih.TemporaryNo + "," +
                    "'" + qid.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + qid.StockItemID + "'," +
                     "'" + qid.ModelNo + "'," +
                    "'" + qid.ModelDetails + "'," +
                    qid.Quantity + "," +
                    qid.Price + " ," +
                    "'" + qid.TaxCode + "'," +
                    qid.Tax + "," +
                    qid.WarrantyDays + "," +
                    "'" + qid.TaxDetails + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "QIDetail", "", updateSQL) +
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
