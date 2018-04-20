using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace CSLERP.DBData
{
    public class indentheader
    {
        public int RowID { get; set; }
        public int ClosingStatus { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int IndentNo { get; set; }
        public DateTime IndentDate { get; set; }
        public string ReferenceInternalOrders { get; set; }
        public string ProductCode { get; set; }
        public DateTime TargetDate { get; set; }
        public string Remarks { get; set; }
        public string CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public double ExchangeRate { get; set; }
        public double ProductValue { get; set; }
        public double ProductValueINR { get; set; }
        public int Status { get; set; }
        public int DocumentStatus { get; set; }
        public int AcceptanceStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string CreatorName { get; set; }
        public string ForwarderName { get; set; }
        public string ApproverName { get; set; }
        public string CommentStatus { get; set; }
        public string Comments { get; set; }
        public string ForwarderList { get; set; }
        public string PurchaseSource { get; set; }
        public int POno { get; set; }
    }
    public class indentdetail
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
        public string ModelDetails { get; set; }
        public double LastPurchasedPrice { get; set; }
        public double QuotedPrice { set; get; }
        public double ExpectedPurchasePrice { get; set; }
        public string QuotationNo { get; set; }
        public double Quantity { get; set; }
        public double Stock { get; set; }
        //public string TaxCode { get; set; }
        public double BufferQuantity { get; set; }
        public int WarrantyDays { get; set; }


    }
    class IndentHeaderDB
    {
        public List<indentheader> getFilteredIndentHeader(string userList, int opt, string userCommentStatusString)
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
            indentheader iheader;
            List<indentheader> iheaderlist = new List<indentheader>();
            try
            {
                string query1 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " IndentNo,IndentDate,ReferenceInternalOrders,ProductCodes,TargetDate,CurrencyID,CurrencyName,Remarks," +
                    " Status,DocumentStatus,AcceptanceStatus,CreateTime, " +
                    "CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,CommentStatus,ForwarderList,PurchaseSource,ProductValue,ProductValueINR,ExchangeRate,ClosingStatus " +
                    " from ViewIndentHeader" +
                   " where ((forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (commentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98))  and Status not in (7,98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query2 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " IndentNo,IndentDate,ReferenceInternalOrders,ProductCodes,TargetDate,CurrencyID,CurrencyName,Remarks," +
                    " Status,DocumentStatus,AcceptanceStatus,CreateTime, " +
                    "CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,CommentStatus,ForwarderList,PurchaseSource,ProductValue,ProductValueINR,ExchangeRate,ClosingStatus " +
                    " from ViewIndentHeader" +
                   " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) and Status not in (7,98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";
                //query 3 is for those who have add/edit/delete permission
                string query3 = "select a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate, a.IndentNo, " +
                                               " a.IndentDate,a.ReferenceInternalOrders,a.ProductCodes, a.TargetDate,a.CurrencyID,a.CurrencyName, " +
                                               " a.Remarks, a.Status,a.DocumentStatus,a.AcceptanceStatus,a.CreateTime,a.CreateUser,a.ForwardUser, " +
                                               " a.ApproveUser,a.CreatorName,a.ForwarderName,a.ApproverName,a.CommentStatus,a.ForwarderList, " +
                                               " a.PurchaseSource,a.ProductValue,a.ProductValueINR, a.ExchangeRate,a.ClosingStatus, COUNT(b.PONo)po " +
                                               " from ViewIndentHeader a  left outer join POHeader b " +
                                               " on ReferenceIndent like CONCAT('%', a.DocumentID, '(', a.IndentNo, CHAR(222), CONVERT(varchar, a.IndentDate), ');%') " +
                                               " and b.Status = 1 and b.DocumentStatus = 99 where ((a.createuser='" + Login.userLoggedIn + "'" +
                                               " or a.ForwarderList like '%" + userList + "%'" +
                                               " or a.commentStatus like '%" + acStr + "%'" +
                                               " or a.approveUser='" + Login.userLoggedIn + "')" +
                                               " and a.DocumentStatus = 99) and a.Status=1 " +
                                               " group by a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate, a.IndentNo, " +
                                               " a.IndentDate,a.ReferenceInternalOrders,a.ProductCodes, a.TargetDate,a.CurrencyID,a.CurrencyName, " +
                                               " a.Remarks, a.Status,a.DocumentStatus,a.AcceptanceStatus,a.CreateTime,a.CreateUser,a.ForwardUser, " +
                                               " a.ApproveUser,a.CreatorName,a.ForwarderName,a.ApproverName,a.CommentStatus,a.ForwarderList, " +
                                               " a.PurchaseSource,a.ProductValue,a.ProductValueINR, a.ExchangeRate,a.ClosingStatus " +
                                               " order by IndentDate desc,DocumentID asc,IndentNo desc";
                string query4 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " IndentNo,IndentDate,ReferenceInternalOrders,ProductCodes,TargetDate,CurrencyID,CurrencyName,Remarks," +
                    " Status,DocumentStatus,AcceptanceStatus,CreateTime, " +
                    "CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,CommentStatus,ForwarderList,PurchaseSource,ProductValue,ProductValueINR,ExchangeRate,ClosingStatus " +
                   " from ViewIndentHeader" +
                   " where IndentNo > 0 and DocumentStatus = 99  and Status=1  ";
                //query 6 for those who have view permission
                string query6 = " select a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate, a.IndentNo, " +
                                " a.IndentDate,a.ReferenceInternalOrders,a.ProductCodes, a.TargetDate,a.CurrencyID,a.CurrencyName, " +
                                " a.Remarks, a.Status,a.DocumentStatus,a.AcceptanceStatus,a.CreateTime,a.CreateUser,a.ForwardUser, " +
                                " a.ApproveUser,a.CreatorName,a.ForwarderName,a.ApproverName,a.CommentStatus,a.ForwarderList, " +
                                " a.PurchaseSource,a.ProductValue,a.ProductValueINR, a.ExchangeRate,a.ClosingStatus, COUNT(b.PONo)po " +
                                " from ViewIndentHeader a  left outer join POHeader b " +
                                " on ReferenceIndent like CONCAT('%', a.DocumentID, '(', a.IndentNo, CHAR(222), CONVERT(varchar, a.IndentDate), ');%') " +
                                " and b.Status = 1 and b.DocumentStatus = 99   where a.Status = 1 and a.DocumentStatus = 99 " +
                                " group by  a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate, a.IndentNo, " +
                                " a.IndentDate,a.ReferenceInternalOrders,a.ProductCodes, a.TargetDate,a.CurrencyID,a.CurrencyName, " +
                                " a.Remarks, a.Status,a.DocumentStatus,a.AcceptanceStatus,a.CreateTime,a.CreateUser,a.ForwardUser, " +
                                " a.ApproveUser,a.CreatorName,a.ForwarderName,a.ApproverName,a.CommentStatus,a.ForwarderList, " +
                                " a.PurchaseSource,a.ProductValue,a.ProductValueINR, a.ExchangeRate,a.ClosingStatus " +
                                " order by a.DocumentID asc, a.IndentNo  ";
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
                    case 4:
                        query = query4;
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
                    iheader = new indentheader();
                    iheader.RowID = reader.GetInt32(0);
                    iheader.DocumentID = reader.GetString(1);
                    iheader.DocumentName = reader.GetString(2);
                    iheader.TemporaryNo = reader.GetInt32(3);
                    iheader.TemporaryDate = reader.GetDateTime(4);
                    iheader.IndentNo = reader.GetInt32(5);
                    iheader.IndentDate = reader.GetDateTime(6);
                    iheader.ReferenceInternalOrders = reader.GetString(7);
                    iheader.ProductCode = reader.GetString(8);
                    iheader.TargetDate = reader.GetDateTime(9);
                    iheader.CurrencyID = reader.IsDBNull(10) ? "" : reader.GetString(10);
                    iheader.CurrencyName = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    iheader.Remarks = reader.GetString(12);
                    iheader.Status = reader.GetInt32(13);
                    iheader.DocumentStatus = reader.GetInt32(14);
                    iheader.AcceptanceStatus = reader.GetInt32(15);
                    iheader.CreateTime = reader.GetDateTime(16);
                    iheader.CreateUser = reader.GetString(17);
                    iheader.ForwardUser = reader.GetString(18);
                    iheader.ApproveUser = reader.GetString(19);
                    iheader.CreatorName = reader.GetString(20);
                    iheader.ForwarderName = reader.GetString(21);
                    iheader.ApproverName = reader.GetString(22);
                    if (!reader.IsDBNull(23))
                    {
                        iheader.CommentStatus = reader.GetString(23);
                    }
                    else
                    {
                        iheader.CommentStatus = "";
                    }
                    if (!reader.IsDBNull(24))
                    {
                        iheader.ForwarderList = reader.GetString(24);
                    }
                    else
                    {
                        iheader.ForwarderList = "";
                    }
                    iheader.PurchaseSource = reader.IsDBNull(25) ? "" : reader.GetString(25);
                    iheader.ProductValue = reader.IsDBNull(26) ? 0 : reader.GetDouble(26);
                    iheader.ProductValueINR = reader.IsDBNull(27) ? 0 : reader.GetDouble(27);
                    iheader.ExchangeRate = reader.IsDBNull(28) ? 0 : reader.GetDouble(28);
                    iheader.ClosingStatus = reader.IsDBNull(29) ? 0 : reader.GetInt32(29);
                    if (opt == 6)
                    {
                        //iheader.PODocID = reader.IsDBNull(30) ? "NULL" : reader.GetString(30);
                        iheader.POno = reader.IsDBNull(30) ? 0 : reader.GetInt32(30);
                        //iheader.PODate = reader.IsDBNull(32) ?DateTime.MinValue: reader.GetDateTime(32);
                    }
                    iheaderlist.Add(iheader);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Indent Header Data");
            }
            return iheaderlist;

        }
        public static List<indentdetail> getIndentDetail(indentheader ih)
        {
            indentdetail id;
            List<indentdetail> IDetail = new List<indentdetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,TemporaryNo,TemporaryDate,StockItemID,StockItemName, ModelNo, ModelName, ModelDetails, " +
                   "LastPurchasePrice,QuotedPrice,ExpectedPurchasePrice,QuotationNo,Quantity,Stock,BufferQuantity,WarrantyDays" +
                   " from ViewIndentDetail " +
                   " where DocumentID='" + ih.DocumentID + "'" +
                   " and TemporaryNo=" + ih.TemporaryNo +
                   " and TemporaryDate='" + ih.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                   " order by StockItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id = new indentdetail();
                    id.RowID = reader.GetInt32(0);
                    id.DocumentID = reader.GetString(1);
                    id.TemporaryNo = reader.GetInt32(2);
                    id.TemporaryDate = reader.GetDateTime(3).Date;
                    id.StockItemID = reader.GetString(4);
                    id.StockItemName = reader.GetString(5);
                    id.ModelNo = reader.IsDBNull(6) ? "NA" : reader.GetString(6);
                    id.ModelName = reader.IsDBNull(7) ? "NA" : reader.GetString(7);
                    id.ModelDetails = reader.GetString(8);
                    id.LastPurchasedPrice = reader.GetDouble(9);
                    id.QuotedPrice = reader.GetDouble(10);
                    id.ExpectedPurchasePrice = reader.GetDouble(11);
                    id.QuotationNo = reader.GetString(12);
                    id.Quantity = reader.GetDouble(13);
                    id.Stock = reader.GetDouble(14);
                    id.BufferQuantity = reader.GetDouble(15);
                    id.WarrantyDays = reader.GetInt32(16);

                    IDetail.Add(id);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Indent Details");
            }
            return IDetail;
        }
        public Boolean forwardIndentHeader(indentheader ih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update IndentHeader set DocumentStatus=" + (ih.DocumentStatus + 1) +
                    ", forwardUser='" + ih.ForwardUser + "'" +
                     ", commentStatus='" + ih.CommentStatus + "'" +
                    ", ForwarderList='" + ih.ForwarderList + "'" +
                    " where DocumentID='" + ih.DocumentID + "'" +
                    " and TemporaryNo=" + ih.TemporaryNo +
                    " and TemporaryDate='" + ih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "IndentHeader", "", updateSQL) +
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
        public Boolean reverseIndentHeader(indentheader ih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update IndentHeader set DocumentStatus=" + ih.DocumentStatus +
                     ", forwardUser='" + ih.ForwardUser + "'" +
                    ", commentStatus='" + ih.CommentStatus + "'" +
                    ", ForwarderList='" + ih.ForwarderList + "'" +
                    " where DocumentID='" + ih.DocumentID + "'" +
                    " and TemporaryNo=" + ih.TemporaryNo +
                    " and TemporaryDate='" + ih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "POProductInwardHeader", "", updateSQL) +
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
        public Boolean ApproveIndentHeader(indentheader ih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update IndentHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                     ", commentStatus='" + ih.CommentStatus + "'" +
                    ", IndentNo=" + ih.IndentNo +
                    ", IndentDate=convert(date, getdate())" +
                    " where DocumentID='" + ih.DocumentID + "'" +
                    " and TemporaryNo=" + ih.TemporaryNo +
                    " and TemporaryDate='" + ih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "IndentHeader", "", updateSQL) +
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
        public Boolean validateIndentHeader(indentheader ih, int opt)
        {
            Boolean status = true;
            try
            {
                if (ih.DocumentID.Trim().Length == 0 || ih.DocumentID == null)
                {
                    return false;
                }
                if (opt == 1) // 1: For Indent Product   2: For Indent Stationery
                {
                    if (ih.ReferenceInternalOrders == null || ih.ReferenceInternalOrders.Trim().Length == 0)
                    {
                        return false;
                    }
                }
                ////if (ih.ProductCode == null || ih.ProductCode.Trim().Length == 0)
                ////{
                ////    return false;
                ////}
                if (ih.TargetDate == null || ih.TargetDate.Date < UpdateTable.getSQLDateTime().Date)
                {
                    return false;
                }
                if (ih.Remarks == null || ih.Remarks.Trim().Length == 0)
                {
                    return false;
                }
                ////if (ih.PurchaseSource == null || ih.PurchaseSource.Trim().Length == 0)
                ////{
                ////    return false;
                ////}
                if (ih.ExchangeRate == 0)
                {
                    return false;
                }
                if (ih.ProductValueINR == 0)
                {
                    return false;
                }
                if (ih.ProductValue == 0)
                {
                    return false;
                }
                if (ih.CurrencyID == null || ih.CurrencyID.Trim().Length == 0)
                {
                    return false;
                }
                //if (ih.Status == 0)
                //{
                //    return false;
                //}
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public List<indentheader> getIndentDetailListForPurchaseOrder()
        {
            indentheader iheader;
            List<indentheader> iheaderlist = new List<indentheader>();
            try
            {
                string query = "select a.DocumentID, b.DocumentName, a.IndentNo,a.IndentDate,a.ReferenceInternalOrders as Reference " +
                    "from IndentHeader a, Document b where a.DocumentID = b.DocumentID and a.DocumentStatus = 99 and a.Status=1" +
                   " union " +
                    " select a.DocumentID, b.DocumentName, a.DocumentNo,a.DocumentDate,a.ReferenceNo  as Reference " +
                    " from IndentGeneralHeader a, Document b where a.DocumentID = b.DocumentID and a.DocumentStatus = 99 and a.Status=1";
                SqlConnection conn = new SqlConnection(Login.connString);

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    iheader = new indentheader();
                    iheader.DocumentID = reader.GetString(0);
                    iheader.DocumentName = reader.GetString(1);
                    iheader.IndentNo = reader.GetInt32(2);
                    iheader.IndentDate = reader.GetDateTime(3);
                    iheader.ReferenceInternalOrders = reader.GetString(4);
                    iheaderlist.Add(iheader);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Indent Header Data");
            }
            return iheaderlist;

        }
        public static ListView ReferenceIndentHeaderSelectionView()
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
                IndentHeaderDB idb = new IndentHeaderDB();
                List<indentheader> IHeaders = idb.getIndentDetailListForPurchaseOrder();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Document ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Document Type", -2, HorizontalAlignment.Left);
                lv.Columns.Add("INDENT No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("INDENT Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Reference", -2, HorizontalAlignment.Left);
                lv.Columns[1].Width = 0;
                foreach (indentheader ih in IHeaders)
                {

                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(ih.DocumentID.ToString());
                    item1.SubItems.Add(ih.DocumentName.ToString());
                    item1.SubItems.Add(ih.IndentNo.ToString());
                    item1.SubItems.Add(ih.IndentDate.ToString("yyyy-MM-dd"));
                    item1.SubItems.Add(ih.ReferenceInternalOrders);
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static string getUserComments(string docid, int tempno, DateTime tempdate)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from IndentHeader where DocumentID='" + docid + "'" +
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


        public Boolean updateindHeaderAndDetail(indentheader ih, indentheader previh, List<indentdetail> IDetail)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update IndentHeader set ReferenceInternalOrders='" + ih.ReferenceInternalOrders +
                     "',ProductCodes='" + ih.ProductCode +
                      "',TargetDate='" + ih.TargetDate.ToString("yyyy-MM-dd") +
                     "', Remarks='" + ih.Remarks +
                      "', PurchaseSource='" + ih.PurchaseSource +
                       "', CurrencyID='" + ih.CurrencyID +
                      "', ExchangeRate=" + ih.ExchangeRate +
                      ",ProductValue=" + ih.ProductValue +
                      ",ProductValueINR=" + ih.ProductValueINR +
                     //",Status=" + ih.Status +
                     ", AcceptanceStatus=" + ih.AcceptanceStatus +
                      ", CommentStatus='" + ih.CommentStatus +
                     "', Comments='" + ih.Comments +
                     "', ForwarderList='" + ih.ForwarderList + "'" +
                     " where DocumentID='" + previh.DocumentID + "'" +
                     " and TemporaryNo=" + previh.TemporaryNo +
                     " and TemporaryDate='" + previh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "IndentHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from IndentDetails where DocumentID='" + previh.DocumentID + "'" +
                      " and TemporaryNo=" + previh.TemporaryNo +
                     " and TemporaryDate='" + previh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "IndentDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (indentdetail id in IDetail)
                {
                    updateSQL = "insert into IndentDetails " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,ModelDetails,LastPurchasePrice,QuotedPrice,ExpectedPurchasePrice,QuotationNo,Quantity,Stock,BufferQuantity,WarrantyDays) " +
                    "values ('" + id.DocumentID + "'," +
                    id.TemporaryNo + "," +
                    "'" + id.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + id.StockItemID + "'," +
                    "'" + id.ModelNo + "'," +
                    "'" + id.ModelDetails + "'," +
                    id.LastPurchasedPrice + "," +
                    id.QuotedPrice + " ," +
                    id.ExpectedPurchasePrice + " ," +
                    "'" + id.QuotationNo + "'," +
                    id.Quantity + "," +
                    id.Stock + "," +
                    id.BufferQuantity + "," +
                    id.WarrantyDays + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "IndentDetail", "", updateSQL) +
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
        public Boolean InsertIndHeaderAndDetail(indentheader ih, List<indentdetail> IDetail)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                ih.TemporaryNo = DocumentNumberDB.getNumber(ih.DocumentID, 1);
                if (ih.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + ih.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + ih.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into IndentHeader " +
                     "(DocumentID,TemporaryNo,TemporaryDate,IndentNo," +
                     "IndentDate,ReferenceInternalOrders,ProductCodes,TargetDate,Remarks,PurchaseSource,CurrencyID,ExchangeRate,ProductValue,ProductValueINR,Status," +
                     " DocumentStatus,AcceptanceStatus,CreateUser,CreateTime,Comments,ForwarderList,CommentStatus)" +
                     " values (" +
                     "'" + ih.DocumentID + "'," +
                     ih.TemporaryNo + "," +
                     "'" + ih.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     ih.IndentNo + "," +
                     "'" + ih.IndentDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + ih.ReferenceInternalOrders + "'," +
                     "'" + ih.ProductCode + "'," +
                     "'" + ih.TargetDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + ih.Remarks + "'," +
                      "'" + ih.PurchaseSource + "'," +
                       "'" + ih.CurrencyID + "'," +
                        ih.ExchangeRate + "," +
                        ih.ProductValue + "," +
                        ih.ProductValueINR + "," +
                     ih.Status + "," +
                     ih.DocumentStatus + "," +
                     ih.AcceptanceStatus + "," +
                     "'" + Login.userLoggedIn + "'," +
                     "GETDATE()" +
                      ",'" + ih.CommentStatus + "'," +
                     "'" + ih.Comments + "'," +
                     "'" + ih.ForwarderList + "')";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "IndentHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from IndentDetails where DocumentID='" + ih.DocumentID + "'" +
                     " and TemporaryNo=" + ih.TemporaryNo +
                    " and TemporaryDate='" + ih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "IndentDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (indentdetail id in IDetail)
                {
                    updateSQL = "insert into IndentDetails " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,ModelDetails,LastPurchasePrice,QuotedPrice,ExpectedPurchasePrice,QuotationNo,Quantity,Stock,BufferQuantity,WarrantyDays) " +
                    "values ('" + id.DocumentID + "'," +
                    ih.TemporaryNo + "," +
                    "'" + id.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + id.StockItemID + "'," +
                    "'" + id.ModelNo + "'," +
                    "'" + id.ModelDetails + "'," +
                    id.LastPurchasedPrice + "," +
                    id.QuotedPrice + " ," +
                    id.ExpectedPurchasePrice + " ," +
                    "'" + id.QuotationNo + "'," +
                    id.Quantity + "," +
                    id.Stock + "," +
                    id.BufferQuantity + "," +
                    id.WarrantyDays + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "IndentDetail", "", updateSQL) +
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

        public static List<indentdetail> getIndentDetailForPO(int InNo, DateTime InDate, string docid)
        {
            indentdetail id;
            List<indentdetail> IDetail = new List<indentdetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select DocumentID,DocumentName,StockItemID,StockItemName, ModelDetails, " +
                   "Quantity,ApproverName,WarrantyDays" +
                   " from ViewIndentDetail " +
                   " where DocumentID='" + docid + "'" +
                   " and IndentNo=" + InNo +
                   " and IndentDate='" + InDate.ToString("yyyy-MM-dd") + "'" +
                   " order by StockItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    id = new indentdetail();
                    id.DocumentID = reader.GetString(0);
                    id.DocumentName = reader.GetString(1);
                    id.StockItemID = reader.GetString(2);
                    id.StockItemName = reader.GetString(3);
                    id.ModelDetails = reader.GetString(4);
                    id.Quantity = reader.GetDouble(5);
                    id.ModelName = reader.GetString(6); //For ApproverName
                    id.WarrantyDays = reader.IsDBNull(7) ? 0 : reader.GetInt32(7);

                    IDetail.Add(id);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Indent Details");
            }
            return IDetail;
        }

        //--------------------
        //CLosing Internal Order
        public static Boolean RequestTOCloseIOHeader(indentheader ih, string comments)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update IndentHeader set ClosingStatus=1 " +
                     ", Comments='" + comments +
                    "' where DocumentID='" + ih.DocumentID + "'" +
                    " and TemporaryNo=" + ih.TemporaryNo +
                    " and TemporaryDate='" + ih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "IndentHeader", "", updateSQL) +
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
        public static Boolean CloseIOHeader(indentheader ih)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update IndentHeader set ClosingStatus=2 , Status = 7 " +
                    " where DocumentID='" + ih.DocumentID + "'" +
                    " and TemporaryNo=" + ih.TemporaryNo +
                    " and TemporaryDate='" + ih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "IndentHeader", "", updateSQL) +
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
        public static Boolean RejectClosingRequest(indentheader ih, string comments)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update IndentHeader set ClosingStatus= 0 " +
                     ", Comments= '" + comments +
                    "' where DocumentID='" + ih.DocumentID + "'" +
                    " and TemporaryNo=" + ih.TemporaryNo +
                    " and TemporaryDate='" + ih.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "IndentHeader", "", updateSQL) +
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
        //---------------
        ///For Report

        public List<indentheader> getIndentHeaderdata(string type, DateTime dtFrom, DateTime dtTo)
        {
            indentheader popid;
            List<indentheader> POPIDetail = new List<indentheader>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "";
                if (type == "Open")
                {
                    query = "select DocumentID, DocumentName,IndentNo,IndentDate,ProductValue,CreatorName from ViewIndentHeader where" +
                            " IndentDate >= '" + dtFrom.ToString("yyyy-MM-dd") + "'" +
                              " and IndentDate <= '" + dtTo.ToString("yyyy-MM-dd") + "'" +
                            " and Status = 1 and DocumentStatus = 99 union " +
                            " select a.DocumentID, b.DocumentName,a.DocumentNo,a.DocumentDate,a.ProductValue,c.Name from IndentGeneralHeader a," +
                            " Document b, ViewUserEmployeeList c where a.DocumentID = b.DocumentID and a.CreateUser = c.UserID " +
                              " and a.DocumentDate >= '" + dtFrom.ToString("yyyy-MM-dd") + "'" +
                              " and a.DocumentDate <= '" + dtTo.ToString("yyyy-MM-dd") + "'" +
                            " and a.Status = 1 and a.DocumentStatus = 99 union " +
                            " select DocumentID, DocumentName,WORequestNo,WORequestDate,TotalAmount,CreatorName from ViewWORequestHeader where " +
                              " WORequestDate >= '" + dtFrom.ToString("yyyy-MM-dd") + "'" +
                              " and WORequestDate <= '" + dtTo.ToString("yyyy-MM-dd") + "'" +
                            " and Status = 1 and DocumentStatus = 99";
                }
                else if (type == "Closed")
                {
                    query = "select DocumentID, DocumentName,IndentNo,IndentDate,ProductValue,CreatorName from ViewIndentHeader where" +
                            " IndentDate >= '" + dtFrom.ToString("yyyy-MM-dd") + "'" +
                              " and IndentDate <= '" + dtTo.ToString("yyyy-MM-dd") + "'" +
                            " and Status = 7 and DocumentStatus = 99 union " +
                            " select a.DocumentID, b.DocumentName,a.DocumentNo,a.DocumentDate,a.ProductValue,c.Name from IndentGeneralHeader a," +
                            " Document b, ViewUserEmployeeList c where a.DocumentID = b.DocumentID and a.CreateUser = c.UserID " +
                              " and a.DocumentDate >= '" + dtFrom.ToString("yyyy-MM-dd") + "'" +
                              " and a.DocumentDate <= '" + dtTo.ToString("yyyy-MM-dd") + "'" +
                            " and a.Status = 7 and a.DocumentStatus = 99 union " +
                            " select DocumentID, DocumentName,WORequestNo,WORequestDate,TotalAmount,CreatorName from ViewWORequestHeader where " +
                              " WORequestDate >= '" + dtFrom.ToString("yyyy-MM-dd") + "'" +
                              " and WORequestDate <= '" + dtTo.ToString("yyyy-MM-dd") + "'" +
                            " and Status = 7 and DocumentStatus = 99";
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popid = new indentheader();
                    popid.DocumentID = reader.GetString(0);
                    popid.DocumentName = reader.GetString(1);
                    popid.IndentNo = reader.GetInt32(2);
                    popid.IndentDate = reader.GetDateTime(3);
                    popid.ProductValue = reader.GetDouble(4);
                    popid.CreatorName = reader.GetString(5);
                    POPIDetail.Add(popid);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Indent Details");
            }
            return POPIDetail;
        }

        public static List<indentdetail> getIndentDetailForReport(indentheader ih)
        {
            indentdetail popid;
            List<indentdetail> POPIDetail = new List<indentdetail>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "SELECT StockItemName ,ModelDetails,LastPurchasePrice,ExpectedPurchasePrice,StockItemID  FROM ViewIndentDetail" +
                  " where IndentNo = " + ih.IndentNo +
                  " and IndentDate = '" + ih.IndentDate.ToString("yyyy-MM-dd") + "'" +
                  " and DocumentID = '" + ih.DocumentID + "'" +
                  " order by StockItemName";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popid = new indentdetail();

                    popid.StockItemName = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    popid.ModelDetails = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    popid.LastPurchasedPrice = reader.IsDBNull(2) ? 0 : reader.GetDouble(2);
                    popid.ExpectedPurchasePrice = reader.IsDBNull(3) ? 0 : reader.GetDouble(3);
                    popid.StockItemID = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    POPIDetail.Add(popid);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO Product Inward Details");
            }
            return POPIDetail;
        }
        public static List<indentdetail> getIndentGeneralDetailForReport(indentheader ih)
        {
            indentdetail popid;
            List<indentdetail> POPIDetail = new List<indentdetail>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "SELECT a.ItemDetail,a.ExpectedPurchasePrice  FROM IndentGeneralDetail a, IndentGeneralHeader b " +
                  " where a.DocumentID = b.DocumentID and a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate and b.DocumentNo = " + ih.IndentNo +
                  " and b.DocumentDate = '" + ih.IndentDate.ToString("yyyy-MM-dd") + "'" +
                  " and b.DocumentID = '" + ih.DocumentID + "'" +
                  " order by a.ItemDetail";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popid = new indentdetail();
                    popid.StockItemName = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    popid.ExpectedPurchasePrice = reader.IsDBNull(1) ? 0 : reader.GetDouble(1);
                    POPIDetail.Add(popid);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO Product Inward Details");
            }
            return POPIDetail;
        }
        public static List<indentdetail> getIndentServiceDetailForReport(indentheader ih)
        {
            indentdetail popid;
            List<indentdetail> POPIDetail = new List<indentdetail>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "SELECT b.Name ,a.WorkDescription,a.Price,a.StockItemID  FROM WORequestDetail a, ServiceItem b, WORequestHeader c" +
                  " where a.StockItemID = b.ServiceItemID and a.DocumentID = c.DocumentID and a.TemporaryNo = c.TemporaryNo and a.TemporaryDate = c.temporaryDate and " +
                  " c.WORequestNo = " + ih.IndentNo +
                  " and c.WORequestDate = '" + ih.IndentDate.ToString("yyyy-MM-dd") + "'" +
                  " and c.DocumentID = '" + ih.DocumentID + "'" +
                  " order by b.Name";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popid = new indentdetail();
                    popid.StockItemName = reader.IsDBNull(0) ? "" : reader.GetString(0); //Name
                    popid.ModelDetails = reader.IsDBNull(1) ? "" : reader.GetString(1);  //WO desc
                    popid.ExpectedPurchasePrice = reader.IsDBNull(2) ? 0 : reader.GetDouble(2); // Price
                    popid.StockItemID = reader.IsDBNull(3) ? "" : reader.GetString(3); //Item ID
                    POPIDetail.Add(popid);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO Product Inward Details");
            }
            return POPIDetail;
        }
        public Boolean ClosIndent2(indentheader popih)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                if (popih.DocumentID == "INDENT" || popih.DocumentID == "INDENTSTATIONERY")
                {
                    updateSQL = "update IndentHeader set status=7 " +
                        " where DocumentID = '" + popih.DocumentID + "'" +
                            "  and IndentNo=" + popih.IndentNo +
                            " and IndentDate='" + popih.IndentDate.ToString("yyyy-MM-dd") + "'";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "IndentHeader", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                else if (popih.DocumentID == "INDENTGENERAL")
                {
                    updateSQL = "update IndentGeneralHeader set status=7 " +
                       " where DocumentID = '" + popih.DocumentID + "'" +
                           "  and DocumentNo=" + popih.IndentNo +
                           " and DocumentDate='" + popih.IndentDate.ToString("yyyy-MM-dd") + "'";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "IndentGeneralHeader", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                else if (popih.DocumentID == "INDENTSERVICE")
                {
                    updateSQL = "update WORequestHeader set status=7 " +
                       " where DocumentID = '" + popih.DocumentID + "'" +
                           "  and WORequestNo=" + popih.IndentNo +
                           " and WORequestDate='" + popih.IndentDate.ToString("yyyy-MM-dd") + "'";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "WORequestHeader", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                if (utString.Length == 0)
                {
                    MessageBox.Show("check document type");
                    return false;
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


        public static List<podetail> GetDetailPO(string indent)
        {
            podetail popid;
            List<podetail> POPIDetail = new List<podetail>();
            try
            {
                //string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);

                string query = "select b.PONo,b.PODate,b.CustomerName,a.StockItemID,a.StockItemName,a.Quantity,a.Price,a.Tax,b.ReferenceIndent " +
                    " from ViewPODetail a,(select PONo, PODate, TemporaryNo, TemporaryDate, CustomerName, ReferenceIndent " +
                    "from ViewPOHeader where ReferenceIndent like '%" + indent + ";%' and DocumentStatus=99 and Status =1) b" +
                    " where a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string[] refind = reader.GetString(8).Replace(Environment.NewLine, string.Empty).Split(';');
                    int ind = Array.IndexOf(refind, indent);
                    if (ind != -1)
                    {
                        popid = new podetail();
                        popid.TemporaryNo = reader.GetInt32(0); //pono
                        popid.TemporaryDate = reader.IsDBNull(1) ? DateTime.MinValue : reader.GetDateTime(1);  //podate
                        popid.ModelName = reader.GetString(2); //customername
                        popid.StockItemID = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        popid.StockItemName = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        popid.Quantity = reader.GetDouble(5);
                        popid.Price = reader.GetDouble(6);
                        popid.Tax = reader.IsDBNull(7) ? 0 : reader.GetDouble(7);
                        POPIDetail.Add(popid);
                    }

                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO Details");
            }
            return POPIDetail;
        }
        public static List<podetail> GetDetailWOForIndentService(string indent)
        {
            podetail popid;
            List<podetail> POPIDetail = new List<podetail>();
            try
            {
                //string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);

                string query = "select a.WONo, a.WODate,a.CustomerName,b.StockItemID,c.Name,b.Quantity," +
                    " b.Price, b.Tax  from ViewWorkOrder a,WODetail b, ServiceItem c " +
                    " where a.DocumentID = b.DocumentID and a.TemporaryNo = b.TemporaryNo" +
                    " and a.TemporaryDate = b.TemporaryDate and b.StockItemID = c.ServiceItemID and" +
                    " a.DocumentStatus = 99 and a.Status = 1 and a.ReferenceInternalOrder like '%" + indent + ";%' order by a.WONo";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    popid = new podetail();
                    popid.TemporaryNo = reader.GetInt32(0); //pono
                    popid.TemporaryDate = reader.IsDBNull(1) ? DateTime.MinValue : reader.GetDateTime(1);  //podate
                    popid.ModelName = reader.GetString(2); //customername
                    popid.StockItemID = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    popid.StockItemName = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    popid.Quantity = reader.GetDouble(5);
                    popid.Price = reader.GetDouble(6);
                    popid.Tax = reader.IsDBNull(7) ? 0 : reader.GetDouble(7);
                    POPIDetail.Add(popid);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO Details");
            }
            return POPIDetail;
        }
        public static List<poheader> getPurchaseOrderHeader(indentheader iih)
        {
            poheader poh;
            List<poheader> PODetail = new List<poheader>();
            try
            {
                string query = "";
                string refindent = iih.DocumentID + "(" + iih.IndentNo + "" + Main.delimiter1 + "" + iih.IndentDate.ToString("yyyy-MM-dd") + ");";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                   " PONo,PODate,ReferenceIndent,ReferenceQuotation,CustomerID,CustomerName,CurrencyID,DeliveryPeriod,ValidityPeriod,TaxTerms,ModeOfPayment,PaymentTerms," +
                   " FreightTerms,FreightCharge,DeliveryAddress,ProductValue,TaxAmount,POValue,Remarks, " +
                   " TermsAndCondition,Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName ,CommentStatus,ForwarderList" +
                   " ,ExchangeRate, ProductValueINR, POValueINR, TaxAmountINR,TransportationMode,SpecialNote,PartialShipment,Transhipment,PackingSpec,PriceBasis,DeliveryAt,CountryID "+
                       "from ViewPOHeader where Status=1 and DocumentStatus=99 and  ReferenceIndent like '%" + refindent + "%'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    poh = new poheader();
                    poh.RowID = reader.GetInt32(0);
                    poh.DocumentID = reader.GetString(1);
                    poh.DocumentName = reader.GetString(2);
                    poh.TemporaryNo = reader.GetInt32(3);
                    poh.TemporaryDate = reader.GetDateTime(4);
                    poh.PONo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        poh.PODate = reader.GetDateTime(6);
                    }
                    poh.ReferenceIndent = reader.GetString(7);
                    poh.ReferenceQuotation = reader.GetString(8);
                    poh.CustomerID = reader.GetString(9);
                    poh.CustomerName = reader.GetString(10);
                    poh.CurrencyID = reader.GetString(11);
                    poh.DeliveryPeriod = reader.GetInt32(12);
                    poh.validityPeriod = reader.GetInt32(13);
                    poh.TaxTerms = reader.GetString(14);
                    poh.ModeOfPayment = reader.GetString(15);
                    poh.PaymentTerms = reader.GetString(16);
                    //poh.CreditPeriod = reader.GetInt32(17);
                    poh.FreightTerms = reader.GetString(17);
                    poh.FreightCharge = reader.GetDouble(18);
                    poh.DeliveryAddress = reader.GetString(19);
                    poh.ProductValue = reader.GetDouble(20);
                    poh.TaxAmount = reader.GetDouble(21);
                    poh.POValue = reader.GetDouble(22);
                    poh.Remarks = reader.GetString(23);
                    poh.TermsAndCondition = reader.GetString(24);
                    poh.Status = reader.GetInt32(25);
                    poh.DocumentStatus = reader.GetInt32(26);
                    poh.CreateTime = reader.GetDateTime(27);
                    poh.CreateUser = reader.GetString(28);
                    poh.ForwardUser = reader.GetString(29);
                    poh.ApproveUser = reader.GetString(30);
                    poh.CreatorName = reader.GetString(31);
                    poh.ForwarderName = reader.GetString(32);
                    poh.ApproverName = reader.GetString(33);
                    if (!reader.IsDBNull(34))
                    {
                        poh.CommentStatus = reader.GetString(34);
                    }
                    else
                    {
                        poh.CommentStatus = "";
                    }
                    if (!reader.IsDBNull(35))
                    {
                        poh.ForwarderList = reader.GetString(35);
                    }
                    else
                    {
                        poh.ForwarderList = "";
                    }
                    poh.ExchangeRate = reader.GetDecimal(36);
                    poh.ProductValueINR = reader.GetDouble(37);
                    poh.POValueINR = reader.GetDouble(38);
                    poh.TaxAmountINR = reader.GetDouble(39);
                    poh.TransportationMode = reader.IsDBNull(40) ? "" : reader.GetString(40);
                    poh.SpecialNote = reader.IsDBNull(41) ? "" : reader.GetString(41);
                    poh.PartialShipment = reader.IsDBNull(42) ? "" : reader.GetString(42);
                    poh.Transhipment = reader.IsDBNull(43) ? "" : reader.GetString(43);
                    poh.PackingSpec = reader.IsDBNull(44) ? "" : reader.GetString(44);
                    poh.PriceBasis = reader.IsDBNull(45) ? "" : reader.GetString(45);
                    poh.DeliveryAt = reader.IsDBNull(46) ? "" : reader.GetString(46);
                    poh.CountryID = reader.IsDBNull(47) ? "" : reader.GetString(47);
                    PODetail.Add(poh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Work Order Details");
            }
            return PODetail;
        }
        public static Boolean isPOPreapredForIndent(int Ino, DateTime Idate, string DocID)
        {
            Boolean isAvail = false;
            try
            {
                string format = DocID + "(" + Ino + Main.delimiter1 + Idate.ToString("yyyy-MM-dd") + ");";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select count(*) from POHeader where ReferenceIndent like '%" + format +
                    "%' and Status=1 and DocumentStatus=99";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader.GetInt32(0) > 0)
                    {
                        isAvail = true;
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return isAvail;
        }
        public DataGridView getGridViewFromIndentForPOAndIODetails(indentheader inh)
        {
            DataGridView grdPOPI = new DataGridView();
            try
            {
                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdPOPI.EnableHeadersVisualStyles = false;
                grdPOPI.AllowUserToAddRows = false;
                grdPOPI.AllowUserToDeleteRows = false;
                grdPOPI.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdPOPI.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdPOPI.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdPOPI.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdPOPI.ColumnHeadersHeight = 27;
                grdPOPI.RowHeadersVisible = false;
                ///grdPOPI.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataTable dt = getPODetailsInDatatable(inh);
                grdPOPI.DataSource = dt;
            }
            catch (Exception ex)
            {
            }

            return grdPOPI;
        }
        public static DataTable getPODetailsInDatatable(indentheader inh)
        {
            DataTable table = new DataTable();
            try
            {
                if (inh.IndentNo == 0)
                {
                    table.Columns.Add("Temporary No");
                    table.Columns.Add("Temporary Date");
                }
                else
                {
                    table.Columns.Add("Indent No");
                    table.Columns.Add("Indent Date");
                }
                table.Columns.Add("Internal Order No");
                table.Columns.Add("Internal Order Date");
                table.Columns.Add("Tracking No");
                table.Columns.Add("Tracking Date");
                table.Columns.Add("Customer");

                string[] IOArr = inh.ReferenceInternalOrders.Split(';');
                List<ioheader> IOLIst = new List<ioheader>();
                for (int i = 0; i < IOArr.Length - 1; i++)
                {
                    ioheader ioh = new ioheader();
                    string[] subStr = IOArr[i].Split('(');
                    string st = subStr[1].Substring(0, subStr[1].IndexOf(')'));
                    DateTime dt = Convert.ToDateTime(subStr[1].Substring(0, subStr[1].IndexOf(')')));

                    //Reference internal orders
                    ioh.InternalOrderNo = Convert.ToInt32(subStr[0]);
                    ioh.InternalOrderDate = dt;
                    ioh.DocumentID = "IOPRODUCT";
                    ioheader iohtemp = getReferenceTrackingNo(ioh);
                    ioh.ReferenceTrackingNos = iohtemp.ReferenceTrackingNos;
                    ioh.CustomerName = iohtemp.CustomerName;

                    //POPRODUCTINWARD;593(2018-02-07)Þ POPRODUCTINWARD; 591(2018 - 02 - 07)Þ
                    string[] mainStr = ioh.ReferenceTrackingNos.Trim().Split(Main.delimiter1);
                    for (int j = 0; j < mainStr.Length - 1; j++)
                    {
                        string[] strRef = mainStr[j].Trim().Split(';'); //POPRODUCTINWARD; 591(2018 - 02 - 07)
                        int findex = strRef[1].IndexOf('(');
                        int sindex = strRef[1].IndexOf(')');
                        string tstr = strRef[1].Substring(findex + 1, (sindex - findex) - 1);
                        string DocIDStr = strRef[0]; //DocID
                        int trackNo1 = Convert.ToInt32(strRef[1].Substring(0, strRef[1].IndexOf('('))); //TrackNo
                        DateTime trackDate1 = Convert.ToDateTime(tstr); //TrackDate
                        popiheader popih = getPOPIHeader(trackNo1, trackDate1, DocIDStr);
                        if (inh.IndentNo == 0)
                        {
                            table.Rows.Add(inh.TemporaryNo, inh.TemporaryDate.ToString("dd-MM-yyyy"), ioh.InternalOrderNo, ioh.InternalOrderDate.ToString("dd-MM-yyyy"),
                                    popih.TrackingNo, popih.TrackingDate.ToString("dd-MM-yyyy"), ioh.CustomerName);
                        }
                        else
                        {
                            table.Rows.Add(inh.IndentNo, inh.IndentDate.ToString("dd-MM-yyyy"), ioh.InternalOrderNo, ioh.InternalOrderDate.ToString("dd-MM-yyyy"),
                                   popih.TrackingNo, popih.TrackingDate.ToString("dd-MM-yyyy"), ioh.CustomerName);
                        }
                    }
                }


            }
            catch (Exception ex)
            {

            }
            return table;
        }
        public static ioheader getReferenceTrackingNo(ioheader ioh)
        {
            ioheader iohtemp = new ioheader();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.ReferenceTrackingNos,a.CustomerID,b.Name from InternalOrderHeader a"+
                        " left outer join Customer b on a.CustomerID = b.CustomerID where DocumentID='" + ioh.DocumentID + "'" +
                        " and InternalOrderNo=" + ioh.InternalOrderNo +
                        " and InternalOrderDate='" + ioh.InternalOrderDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    iohtemp.ReferenceTrackingNos = reader.GetString(0);
                    iohtemp.CustomerID = reader.IsDBNull(1)? "" :reader.GetString(1);
                    iohtemp.CustomerName = reader.IsDBNull(2) ? "" : reader.GetString(2);
                }
                conn.Open();
            }
            catch (Exception ex)
            {
            }
            return iohtemp;
        }
        public static popiheader getPOPIHeader(int trackNo, DateTime trackDate, string DocID)
        {
            popiheader popih = new popiheader();
            try
            {
                string query = "select a.RowID,a.DocumentID,a.TemporaryNo, a.TemporaryDate,a.TrackingNo,a.TrackingDate,a.ReferenceNo, " +
                    " a.CustomerID , a.CustomerPONo,a.CustomerPODate,b.Name from POProductInwardHeader a left outer join Customer b on a.CustomerID = b.CustomerID " +
                    " where a.DocumentID = '" + DocID + "' and a.TrackingNo = " + trackNo +
                    " and a.TrackingDate='" + trackDate.ToString("yyyy-MM-dd") + "'";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    popih.RowID = reader.GetInt32(0);
                    popih.DocumentID = reader.GetString(1);
                    popih.TemporaryNo = reader.GetInt32(2);
                    popih.TemporaryDate = reader.GetDateTime(3);
                    popih.TrackingNo = reader.GetInt32(4);
                    popih.TrackingDate = reader.GetDateTime(5);
                    popih.ReferenceNo = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    popih.CustomerID = reader.GetString(7);
                    popih.CustomerPONO = reader.GetString(8);
                    popih.CustomerPODate = reader.GetDateTime(9);
                    popih.CustomerName = reader.IsDBNull(10) ? "" : reader.GetString(10);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PO Product Inward Header Details");
            }
            return popih;
        }
    }
}
