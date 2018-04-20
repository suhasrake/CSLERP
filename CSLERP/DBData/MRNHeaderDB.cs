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
    class mrnheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int MRNNo { get; set; }
        public DateTime MRNDate { get; set; }
        public int PONO { get; set; }
        public DateTime PODate { get; set; }
        public string PONOs { get; set; }
        public string PODates { get; set; }
        public string Reference { get; set; }
        public string DCNo { get; set; }
        public DateTime DCDate { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string TransportationMode { get; set; }
        public string TransporterType { get; set; }
        public string TransporterName { get; set; }

        public double ProductValue { get; set; }
        public double TaxAmount { get; set; }
        public double MRNValue { get; set; }

        public double ProductValueINR { get; set; }
        public double TaxAmountINR { get; set; }
        public double MRNValueINR { get; set; }

        public string Remarks { get; set; }
        public int status { get; set; }
        public int DocumentStatus { get; set; }
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
        public int QCStatus { get; set; }
        public mrnheader()
        {
            Reference = "";
            Comments = "";
        }

        public string CurrencyID { get; set; }
        public decimal ExchangeRate { get; set; }
    }
    class mrndetail
    {
        public int RowID { get; set; }
        public int PORefNo { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int PONO { get; set; }
        public DateTime PODate { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string Unit { get; set; }
        public double Quantity { get; set; }
        public string BatchNo { get; set; }
        public string SerialNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string StoreLocationID { get; set; }
        public string Description { get; set; }
        public Double QuantityAccepted { get; set; }
        public Double QuantityReturned { get; set; }
        public Double QuantityRejected { get; set; }
        public double Price { get; set; }
        public double Tax { get; set; }
        public double PriceINR { get; set; }
        public double TaxINR { get; set; }
        public string TaxDetails { get; set; }
        public string TaxCode { get; set; }
    }
    class MRNHeaderDB
    {
        public List<mrnheader> getFilteredMRNHeader(string userList, int opt, string userCommentStatusString)
        {
            mrnheader mrnh;
            List<mrnheader> MrnHeaders = new List<mrnheader>();
            try
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
                string query1 = "select RowID, DocumentID, DocumentName,CustomerID,CustomerName,TemporaryNo,TemporaryDate," +
                    " MRNNo,MRNDate,PONOs,PODates,Reference,DCNo,DCDate,InvoiceNo,InvoiceDate," +
                    " TransportationMode,TransporterType,TransporterName,ProductValue,TaxAmount,MRNValue,Remarks ," +
                    " status,DocumentStatus,QCStatus, CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,CommentStatus,ForwarderList,CurrencyID,ExchangeRate,ProductValueINR,TaxAmountINR,MRNValueINR " +
                    " from ViewMRNHeader" +
                    " where ((ForwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (CreateUser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (CommentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) and Status not in (7,98) order by MRNDate desc,DocumentID asc,MRNNO desc";

                string query2 = "select RowID, DocumentID, DocumentName,CustomerID,CustomerName,TemporaryNo,TemporaryDate," +
                    " MRNNo,MRNDate,PONOs,PODates,Reference,DCNo,DCDate,InvoiceNo,InvoiceDate," +
                    " TransportationMode,TransporterType,TransporterName,ProductValue,TaxAmount,MRNValue,Remarks ," +
                    " status,DocumentStatus,QCStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,CommentStatus,ForwarderList,CurrencyID,ExchangeRate,ProductValueINR,TaxAmountINR,MRNValueINR  " +
                    " from ViewMRNHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98)) and Status not in (7,98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                string query3 = "select RowID, DocumentID, DocumentName,CustomerID,CustomerName,TemporaryNo,TemporaryDate," +
                    " MRNNo,MRNDate,PONOs,PODates,Reference,DCNo,DCDate,InvoiceNo,InvoiceDate," +
                    "TransportationMode,TransporterType,TransporterName,ProductValue,TaxAmount,MRNValue,Remarks ," +
                    " status,DocumentStatus,QCStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,CommentStatus,ForwarderList ,CurrencyID,ExchangeRate,ProductValueINR,TaxAmountINR,MRNValueINR  " +
                    " from ViewMRNHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or commentStatus like '%" + acStr + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99) and Status = 1  order by MRNDate desc,DocumentID asc,MRNNo desc";

                string query6 = "select RowID, DocumentID, DocumentName,CustomerID,CustomerName,TemporaryNo,TemporaryDate," +
                    " MRNNo,MRNDate,PONOs,PODates,Reference,DCNo,DCDate,InvoiceNo,InvoiceDate," +
                    " TransportationMode,TransporterType,TransporterName,ProductValue,TaxAmount,MRNValue,Remarks ," +
                    " status,DocumentStatus,QCStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,CommentStatus,ForwarderList,CurrencyID,ExchangeRate,ProductValueINR,TaxAmountINR,MRNValueINR  " +
                    " from ViewMRNHeader" +
                    " where  DocumentStatus = 99  and Status = 1 order by MRNDate desc,DocumentID asc,MRNNo desc";

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
                    try
                    {
                        mrnh = new mrnheader();
                        mrnh.RowID = reader.GetInt32(0);
                        mrnh.DocumentID = reader.GetString(1);
                        mrnh.DocumentName = reader.GetString(2);
                        mrnh.CustomerID = reader.GetString(3);
                        mrnh.CustomerName = reader.GetString(4);
                        mrnh.TemporaryNo = reader.GetInt32(5);
                        mrnh.TemporaryDate = reader.GetDateTime(6);
                        mrnh.MRNNo = reader.GetInt32(7);
                        mrnh.MRNDate = reader.GetDateTime(8);

                        mrnh.PONOs = reader.IsDBNull(9) ? "" : reader.GetString(9);
                        mrnh.PODates = reader.IsDBNull(10) ? "" : reader.GetString(10);
                        mrnh.Reference = reader.GetString(11);
                        mrnh.DCNo = reader.GetString(12);
                        mrnh.DCDate = reader.GetDateTime(13);
                        mrnh.InvoiceNo = reader.GetString(14);
                        mrnh.InvoiceDate = reader.GetDateTime(15);
                        mrnh.TransportationMode = reader.GetString(16);
                        mrnh.TransporterType = reader.GetString(17);
                        mrnh.TransporterName = reader.GetString(18);
                        // mrnh.TaxCode = reader.GetString(19);
                        mrnh.ProductValue = reader.GetDouble(19);
                        mrnh.TaxAmount = reader.GetDouble(20);
                        mrnh.MRNValue = reader.GetDouble(21);
                        mrnh.Remarks = reader.GetString(22);
                        mrnh.status = reader.GetInt32(23);
                        mrnh.DocumentStatus = reader.GetInt32(24);
                        mrnh.QCStatus = reader.GetInt32(25);
                        mrnh.CreateTime = reader.GetDateTime(26);
                        mrnh.CreateUser = reader.GetString(27);
                        mrnh.ForwardUser = reader.GetString(28);
                        mrnh.ApproveUser = reader.GetString(29);
                        mrnh.CreatorName = reader.GetString(30);
                        mrnh.ForwarderName = reader.GetString(31);
                        mrnh.ApproverName = reader.GetString(32);

                        if (!reader.IsDBNull(33))
                        {
                            mrnh.CommentStatus = reader.GetString(33);
                        }
                        else
                        {
                            mrnh.CommentStatus = "";
                        }
                        if (!reader.IsDBNull(34))
                        {
                            mrnh.ForwarderList = reader.GetString(34);
                        }
                        else
                        {
                            mrnh.ForwarderList = "";
                        }
                        mrnh.CurrencyID = reader.IsDBNull(35) ? "" : reader.GetString(35);
                        mrnh.ExchangeRate = reader.IsDBNull(36) ? 0 : reader.GetDecimal(36);

                        mrnh.ProductValueINR = reader.GetDouble(37);
                        mrnh.TaxAmountINR = reader.GetDouble(38);
                        mrnh.MRNValueINR = reader.GetDouble(39);

                        MrnHeaders.Add(mrnh);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error querying MRN Header Details");
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying MRN Header Details");
            }
            return MrnHeaders;
        }



        public static List<mrndetail> getMRNDetail(mrnheader mrnh)
        {
            mrndetail mrnd;
            List<mrndetail> MrnDetail = new List<mrndetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,DocumentName,TemporaryNo, TemporaryDate,StockItemID,Name, Unit,Quantity, " +
                   "Price,Tax,TaxDetails," +
                  "BatchNo,SerialNo,ExpiryDate,StoreLocationID,Description,QuantityAccepted," +
                  "QuantityRejected, ModelNo,ModelName,TaxCode,PONo,PODate,POItemReferenceNo,PriceINR,TaxINR " +
                  "from ViewMRNDetail" +
                  " where DocumentID='" + mrnh.DocumentID + "'" +
                  " and TemporaryNo=" + mrnh.TemporaryNo +
                  " and TemporaryDate='" + mrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                  " order by StockItemID";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mrnd = new mrndetail();
                    mrnd.RowID = reader.GetInt32(0);
                    mrnd.DocumentID = reader.GetString(1);
                    mrnd.DocumentName = reader.GetString(2);
                    mrnd.TemporaryNo = reader.GetInt32(3);
                    mrnd.TemporaryDate = reader.GetDateTime(4).Date;
                    mrnd.StockItemID = reader.GetString(5);
                    mrnd.StockItemName = reader.GetString(6);

                    mrnd.Unit = reader.GetString(7);
                    mrnd.Quantity = reader.GetDouble(8);
                    mrnd.Price = reader.GetDouble(9);
                    mrnd.Tax = reader.GetDouble(10);
                    mrnd.TaxDetails = reader.GetString(11);
                    mrnd.BatchNo = reader.GetString(12);
                    mrnd.SerialNo = reader.GetString(13);
                    mrnd.ExpiryDate = reader.GetDateTime(14);
                    mrnd.StoreLocationID = reader.GetString(15);
                    mrnd.Description = reader.GetString(16);
                    mrnd.QuantityAccepted = reader.GetDouble(17);
                    mrnd.QuantityRejected = reader.GetDouble(18);
                    mrnd.ModelNo = reader.IsDBNull(19) ? "NA" : reader.GetString(19);
                    mrnd.ModelName = reader.IsDBNull(20) ? "NA" : reader.GetString(20);
                    mrnd.TaxCode = reader.IsDBNull(21) ? "" : reader.GetString(21);
                    mrnd.PONO = reader.IsDBNull(22) ? 0 : reader.GetInt32(22);
                    mrnd.PODate = reader.IsDBNull(23) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(23);
                    mrnd.PORefNo = reader.IsDBNull(24) ? 0 : reader.GetInt32(24);

                    mrnd.PriceINR = reader.GetDouble(25);
                    mrnd.TaxINR = reader.GetDouble(26);

                    MrnDetail.Add(mrnd);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying MRN Details");
            }
            return MrnDetail;
        }
        public Boolean validateMRNHeader(mrnheader mrnh, int opt)
        {
            Boolean status = true;
            try
            {
                if (mrnh.DocumentID.Trim().Length == 0 || mrnh.DocumentID == null)
                {
                    return false;
                }

                if (mrnh.CustomerID.Trim().Length == 0 || mrnh.CustomerID == null)
                {
                    return false;
                }
                if (mrnh.PONOs.Trim().Length == 0 || mrnh.PONOs == null)
                {
                    return false;
                }
                if (mrnh.PODates.Trim().Length == 0 || mrnh.PODates == null)
                {
                    return false;
                }
                if (mrnh.Reference.Trim().Length == 0 || mrnh.Reference == null)
                {
                    return false;
                }
                if (mrnh.DCNo.Trim().Length == 0 || mrnh.DCNo == null || mrnh.DCDate == null || mrnh.DCDate > UpdateTable.getSQLDateTime())
                {
                    if (mrnh.InvoiceNo.Trim().Length == 0 || mrnh.InvoiceNo == null || mrnh.InvoiceDate == null || mrnh.DCDate > UpdateTable.getSQLDateTime()
                        || mrnh.InvoiceDate > UpdateTable.getSQLDateTime())
                    {
                        return false;
                    }
                }
                if (mrnh.TransportationMode == null || mrnh.TransportationMode.Trim().Length == 0)
                {
                    return false;
                }
                if (mrnh.TransporterName == null || mrnh.TransporterName.Trim().Length == 0)
                {
                    return false;
                }
                if (mrnh.TransporterType.Trim().Length == 0 || mrnh.TransporterType == null)
                {
                    return false;
                }
                if (mrnh.Remarks.Trim().Length == 0 || mrnh.Remarks == null)
                {
                    return false;
                }
                if (mrnh.CurrencyID.Trim().Length == 0 || mrnh.CurrencyID == null)
                {
                    return false;
                }
                if (opt == 0)
                {
                    if (mrnh.ProductValue == 0)
                    {
                        return false;
                    }
                    if (mrnh.MRNValue == 0)
                    {
                        return false;
                    }
                }
                if (mrnh.ExchangeRate == 0)
                {
                    return false;
                }
                //if (mrnh.TaxAmount == 0)
                //{
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                return false;
            }
            return status;
        }
        public Boolean forwardMRN(mrnheader mrnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update MRNHeader set DocumentStatus=" + (mrnh.DocumentStatus + 1) +
                    ", forwardUser='" + mrnh.ForwardUser + "'" +
                    ", commentStatus='" + mrnh.CommentStatus + "'" +
                    ", ForwarderList='" + mrnh.ForwarderList + "'" +
                    ", QCStatus= " + mrnh.QCStatus +
                    " where DocumentID='" + mrnh.DocumentID + "'" +
                    " and TemporaryNo=" + mrnh.TemporaryNo +
                    " and TemporaryDate='" + mrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "MRNHeader", "", updateSQL) +
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

        public Boolean reverseMRN(mrnheader mrnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update MRNHeader set DocumentStatus=" + mrnh.DocumentStatus +
                    ",QCStatus=" + mrnh.QCStatus +
                    ", forwardUser='" + mrnh.ForwardUser + "'" +
                    ", commentStatus='" + mrnh.CommentStatus + "'" +
                    ", ForwarderList='" + mrnh.ForwarderList + "'" +
                    " where DocumentID='" + mrnh.DocumentID + "'" +
                    " and TemporaryNo=" + mrnh.TemporaryNo +
                    " and TemporaryDate='" + mrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "MRNHeader", "", updateSQL) +
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

        public static Boolean checkForIssuesFromMRN(int tno, DateTime tdt)
        {
            Boolean status = false;   // Not issued
            int count = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select COUNT(*) from Stock where InwardDocumentID = 'MRN' and " +
                        " InwardDocumentNo = (select MRNNo from MRNHeader where TemporaryNo = " + tno + " and TemporaryDate = '" + tdt.ToString("yyyy-MM-dd") + "') and" +
                        " InwardDocumentDate = (select MRNDate from MRNHeader where TemporaryNo = " + tno + " and TemporaryDate = '" + tdt.ToString("yyyy-MM-dd") + "') " +
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

        public Boolean ApproveMRN(mrnheader mrnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update MRNHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + mrnh.CommentStatus + "'" +
                    ", MRNNo=" + mrnh.MRNNo +
                    ", MRNDate=convert(date, getdate())" +
                    " where DocumentID='" + mrnh.DocumentID + "'" +
                    " and TemporaryNo=" + mrnh.TemporaryNo +
                    " and TemporaryDate='" + mrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "MRNHeader", "", updateSQL) +
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
        public static string getUserComments(string docid, int tempno, DateTime tempdate)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from MRNHeader where DocumentID='" + docid + "'" +
                        " and TemporaryNo=" + tempno +
                        " and TemporaryDate='" + tempdate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    cmtString = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return cmtString;
        }
        public static mrnheader getMRNNoAndDate(int tempNo, DateTime tempDate)
        {
            mrnheader mrnh = new mrnheader();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select MRNNo, MRNDate from MRNHeader " +
                        " where TemporaryNo=" + tempNo +
                        " and TemporaryDate='" + tempDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    mrnh.MRNNo = reader.GetInt32(0);
                    mrnh.MRNDate = reader.GetDateTime(1);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }

            return mrnh;
        }
        public List<mrnheader> getMRNHeaderDetailForListView()
        {
            mrnheader mrnh;
            List<mrnheader> MrnHeaders = new List<mrnheader>();
            try
            {

                string query =
                    " select a.MRNNo,a.MRNDate,a.PONOs,a.PODates,a.InvoiceNo,a.InvoiceDate,b.Name as CustomerName,a.CustomerID,c.NoOfFound from MRNHeader a" +
                    " left outer join Customer b on a.CustomerID = b.CustomerID left outer join " +
                    " (select MRNNo,MRNDate,COUNT(*) as NoOfFound  from InvoiceInHeader where DocumentID = 'POINVOICEIN' group by MRNNo,MRNDate) c on a.MRNNo = c.MRNNo and a.MRNDate = c.MRNDate " +
                    " where a.DocumentStatus = 99 and a.status=1 order by MRNNo asc";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        mrnh = new mrnheader();
                        mrnh.MRNNo = reader.GetInt32(0);
                        mrnh.MRNDate = reader.GetDateTime(1);
                        mrnh.PONOs = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        mrnh.PODates = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        mrnh.InvoiceNo = reader.GetString(4);
                        mrnh.InvoiceDate = reader.GetDateTime(5);
                        mrnh.CustomerName = reader.GetString(6);
                        mrnh.CustomerID = reader.GetString(7);
                        mrnh.QCStatus = reader.IsDBNull(8) ? 0 : reader.GetInt32(8); //For Temp Var storing Count of INvoice prepeared 
                        MrnHeaders.Add(mrnh);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying MRN Header Details");
            }
            return MrnHeaders;
        }

        public static ListView getMRNHeaderListView()
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
                MRNHeaderDB mrnhdb = new MRNHeaderDB();
                List<mrnheader> MRNHeaders = mrnhdb.getMRNHeaderDetailForListView();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("MRN No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("MRN Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO Nos", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO Dates", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Invoice No", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Invoice Date", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Cust ID", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Cust Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("IsInvPrepared", -2, HorizontalAlignment.Center);
                lv.Columns[9].Width = 0;
                foreach (mrnheader mrnh in MRNHeaders)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(mrnh.MRNNo.ToString());
                    item1.SubItems.Add(mrnh.MRNDate.ToShortDateString());
                    item1.SubItems.Add(mrnh.PONOs.ToString());
                    item1.SubItems.Add(mrnh.PODates);
                    item1.SubItems.Add(mrnh.InvoiceNo);
                    item1.SubItems.Add(mrnh.InvoiceDate.ToShortDateString());
                    item1.SubItems.Add(mrnh.CustomerID);
                    item1.SubItems.Add(mrnh.CustomerName);
                    if (mrnh.QCStatus != 0)
                        item1.BackColor = Color.Tan;
                    item1.SubItems.Add(mrnh.QCStatus.ToString()); //For Temp Var storing Count of INvoice prepeared 
                    lv.Items.Add(item1);
                    ////index++;
                }


            }
            catch (Exception)
            {

            }
            return lv;
        }

        public Boolean updateMRNHeaderAndDetail(mrnheader mrnh, mrnheader prevmrnh, List<mrndetail> MRNDetails)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update MRNHeader set CustomerID='" + mrnh.CustomerID +
                    "',PONos='" + mrnh.PONOs +
                     "',PODates='" + mrnh.PODates +
                    "', Reference='" + mrnh.Reference +
                    "', DCNo='" + mrnh.DCNo +
                    "', DCDate='" + mrnh.DCDate.ToString("yyyy-MM-dd") +
                    "', InvoiceNo ='" + mrnh.InvoiceNo +
                    "', InvoiceDate='" + mrnh.InvoiceDate.ToString("yyyy-MM-dd") +
                    "', TransportationMode='" + mrnh.TransportationMode +
                    "', TransporterType='" + mrnh.TransporterType +
                    "', TransporterName='" + mrnh.TransporterName +
                      "', CurrencyID='" + mrnh.CurrencyID +
                    "', ExchangeRate='" + mrnh.ExchangeRate +
                    "', ProductValue='" + mrnh.ProductValue +
                    "', TaxAmount='" + mrnh.TaxAmount +
                     "', MRNValue='" + mrnh.MRNValue +
                        "', ProductValueINR='" + mrnh.ProductValueINR +
                    "', TaxAmountINR='" + mrnh.TaxAmountINR +
                     "', MRNValueINR='" + mrnh.MRNValueINR +
                    "', Remarks='" + mrnh.Remarks +
                    "', Comments='" + mrnh.Comments +
                    "', ForwarderList='" + mrnh.ForwarderList +
                    "', CommentStatus='" + mrnh.CommentStatus +
                    "', QCStatus=" + mrnh.QCStatus +
                    " where DocumentID='" + prevmrnh.DocumentID + "'" +
                    " and TemporaryNo=" + prevmrnh.TemporaryNo +
                    " and TemporaryDate='" + prevmrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "MRNHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from MRNDetail where DocumentID='" + prevmrnh.DocumentID + "'" +
                    " and TemporaryNo=" + prevmrnh.TemporaryNo +
                    " and TemporaryDate='" + prevmrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "MRNDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (mrndetail mrnd in MRNDetails)
                {
                    updateSQL = "insert into MRNDetail " +
                    "(DocumentID,POItemReferenceNo,TemporaryNo,TemporaryDate,PONo,PODate,StockItemID,ModelNo,TaxCode,Quantity,Price," +
                    "Tax,PriceINR,TaxINR,TaxDetails,BatchNo,SerialNo,ExpiryDate,StoreLocationID,QuantityAccepted,QuantityRejected) " +
                    "values ('" + mrnh.DocumentID + "'," +
                     mrnd.PORefNo + "," +
                    mrnh.TemporaryNo + "," +
                    "'" + mrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    mrnd.PONO + "," +
                    "'" + mrnd.PODate.ToString("yyyy-MM-dd") + "'," +
                    "'" + mrnd.StockItemID + "'," +
                     "'" + mrnd.ModelNo + "'," +
                     "'" + mrnd.TaxCode + "'," +
                   mrnd.Quantity + "," +
                   mrnd.Price + "," +
                   mrnd.Tax + "," +
                      mrnd.PriceINR + "," +
                   mrnd.TaxINR + "," +
                    "'" + mrnd.TaxDetails + "'," +
                    "'" + mrnd.BatchNo + "'," +
                   "'" + mrnd.SerialNo + "'," +
                    "'" + mrnd.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + mrnd.StoreLocationID + "'," +
                    mrnd.QuantityAccepted + "," +
                    +mrnd.QuantityRejected + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "MRNDetail", "", updateSQL) +
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
        public Boolean InsertMRNHeaderAndDetail(mrnheader mrnh, List<mrndetail> MRNDetails)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                mrnh.TemporaryNo = DocumentNumberDB.getNumber(mrnh.DocumentID, 1);
                if (mrnh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + mrnh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + mrnh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into MRNHeader " +
                    "(DocumentID,TemporaryNo,TemporaryDate,MRNNO,MRNDate,PONos,PODates,Reference," +
                    "DCNo,DCDate,InvoiceNo,InvoiceDate,CustomerID,TransportationMode,TransporterType,TransporterName,Remarks,CurrencyID,ExchangeRate," +
                    "ProductValue,TaxAmount,MRNValue,ProductValueINR,TaxAmountINR,MRNValueINR,Comments,CreateTime,CreateUser,ForwarderList,CommentStatus,DocumentStatus,Status)" +
                    " values (" +
                    "'" + mrnh.DocumentID + "'," +
                    mrnh.TemporaryNo + "," +
                    "'" + mrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    mrnh.MRNNo + "," +
                    "'" + mrnh.MRNDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + mrnh.PONOs + "'," +
                    "'" + mrnh.PODates + "'," +
                    "'" + mrnh.Reference + "'," +
                     "'" + mrnh.DCNo + "'," +
                    "'" + mrnh.DCDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + mrnh.InvoiceNo + "'," +
                    "'" + mrnh.InvoiceDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + mrnh.CustomerID + "'," +

                    "'" + mrnh.TransportationMode + "'," +
                    "'" + mrnh.TransporterType + "'," +
                    "'" + mrnh.TransporterName + "'," +
                    "'" + mrnh.Remarks + "'," +
                     "'" + mrnh.CurrencyID + "'," +
                     mrnh.ExchangeRate + "," +
                     mrnh.ProductValue + "," +
                      mrnh.TaxAmount + "," +
                       mrnh.MRNValue + "," +
                            mrnh.ProductValueINR + "," +
                      mrnh.TaxAmountINR + "," +
                       mrnh.MRNValueINR + "," +
                     "'" + mrnh.Comments + "'," +
                      "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'," +
                    "'" + mrnh.ForwarderList + "'," +
                     "'" + mrnh.CommentStatus + "'," +
                    mrnh.DocumentStatus + "," +
                         mrnh.status + ")";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "MRNHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from MRNDetail where DocumentID='" + mrnh.DocumentID + "'" +
                    " and TemporaryNo=" + mrnh.TemporaryNo +
                    " and TemporaryDate='" + mrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "MRNDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (mrndetail mrnd in MRNDetails)
                {
                    updateSQL = "insert into MRNDetail " +
                    "(DocumentID,POItemReferenceNo,TemporaryNo,TemporaryDate,PONo,PODate,StockItemID,ModelNo,TaxCode,Quantity,Price,Tax,PriceINR,TaxINR,TaxDetails,BatchNo,SerialNo,ExpiryDate,StoreLocationID,QuantityAccepted,QuantityRejected) " +
                    "values ('" + mrnh.DocumentID + "'," +
                     mrnd.PORefNo + "," +
                    mrnh.TemporaryNo + "," +
                    "'" + mrnh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     mrnd.PONO + "," +
                    "'" + mrnd.PODate.ToString("yyyy-MM-dd") + "'," +
                    "'" + mrnd.StockItemID + "'," +
                     "'" + mrnd.ModelNo + "'," +
                      "'" + mrnd.TaxCode + "'," +
                   mrnd.Quantity + "," +
                   mrnd.Price + "," +
                   mrnd.Tax + "," +
                       mrnd.PriceINR + "," +
                   mrnd.TaxINR + "," +
                    "'" + mrnd.TaxDetails + "'," +
                    "'" + mrnd.BatchNo + "'," +
                   "'" + mrnd.SerialNo + "'," +
                    "'" + mrnd.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + mrnd.StoreLocationID + "'," +
                    mrnd.QuantityAccepted + "," +
                    +mrnd.QuantityRejected + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "MRNDetail", "", updateSQL) +
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
        public List<mrndetail> getStockDetailsForOnePO(int poNo, DateTime poDate)
        {
            mrndetail mrnd;
            List<mrndetail> detList = new List<mrndetail>();
            try
            {

                string query =
                    "select b.MRNNo,b.MRNDate,a.StockItemID,a.Name,a.ModelNo,a.ModelName,a.Quantity,a.QuantityAccepted,a.QuantityRejected,a.PONo,a.PODate " +
                    " from ViewMRNDetail a , MRNHeader b where a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate" +
                    " and a.TemporaryNo in (select TemporaryNo from MRNHeader where PONos like '%;" + poNo + ";%' and PODates like '%;" + poDate.ToString("yyyy-MM-dd") + ";%')" +
                    " and a.TemporaryDate in (select TemporaryDate from MRNHeader  where PONos like '%;" + poNo + ";%' and PODates like '%;" + poDate.ToString("yyyy-MM-dd") + ";%')" +
                    " and  b.DocumentStatus = 99 and b.status=1 order by b.MRNDate desc,b.MRNNo desc";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        mrnd = new mrndetail();
                        mrnd.TemporaryNo = reader.GetInt32(0);      // For MRNNO
                        mrnd.TemporaryDate = reader.GetDateTime(1); //for MRNDate
                        //mrnd.StoreLocationID = reader.GetString(2);            //For PONos
                        //mrnd.Description = reader.GetString(3);    // For PODates
                        mrnd.StockItemID = reader.GetString(2);
                        mrnd.StockItemName = reader.GetString(3);
                        mrnd.ModelNo = reader.IsDBNull(4) ? "" : reader.GetString(4);
                        mrnd.ModelName = reader.IsDBNull(5) ? "" : reader.GetString(5);
                        mrnd.Quantity = reader.GetDouble(6);
                        mrnd.QuantityAccepted = reader.GetDouble(7);
                        mrnd.QuantityRejected = reader.GetDouble(8);
                        mrnd.PONO = reader.IsDBNull(9) ? 0 : reader.GetInt32(9);
                        mrnd.PODate = reader.IsDBNull(10) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(10);

                        detList.Add(mrnd);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying MRN Details");
            }
            return detList;
        }
        public static ListView getStockDetailsForOnePOListView(int poNo, DateTime poDate)
        {
            ListView lv = new ListView();
            try
            {

                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                //lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                MRNHeaderDB mrnhdb = new MRNHeaderDB();
                List<mrndetail> MRNdetails = mrnhdb.getStockDetailsForOnePO(poNo, poDate);
                ////int index = 0;
                lv.Columns.Add("MRN No", -2, HorizontalAlignment.Center);
                lv.Columns.Add("MRN Date", -2, HorizontalAlignment.Center);
                lv.Columns.Add("PO No", -2, HorizontalAlignment.Center);
                lv.Columns.Add("PO Date", -2, HorizontalAlignment.Center);
                lv.Columns.Add("StockItem ID", -2, HorizontalAlignment.Center);
                lv.Columns.Add("StockItem Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Model No", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Model Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Quant", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Acpt Quan", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Rej Quan", -2, HorizontalAlignment.Center);
                //lv.Columns[3].Width = 0;
                lv.Columns[6].Width = 0;
                //lv.Columns[2].Width = 0;
                lv.Columns[5].Width = 250;
                lv.Columns[7].Width = 100;
                //lv.Columns["Model No"].Width = 0;
                foreach (mrndetail mrnd in MRNdetails)
                {
                    ListViewItem item1 = new ListViewItem(mrnd.TemporaryNo.ToString()); //For MRNNo
                    item1.SubItems.Add(mrnd.TemporaryDate.ToShortDateString());                  //For MRNDate
                    item1.SubItems.Add(poNo.ToString());                          //For PONo
                    item1.SubItems.Add(poDate.ToShortDateString());                     //For PODate
                    item1.SubItems.Add(mrnd.StockItemID);
                    item1.SubItems.Add(mrnd.StockItemName);
                    item1.SubItems.Add(mrnd.ModelNo);
                    item1.SubItems.Add(mrnd.ModelName);
                    item1.SubItems.Add(mrnd.Quantity.ToString());
                    item1.SubItems.Add(mrnd.QuantityAccepted.ToString());
                    item1.SubItems.Add(mrnd.QuantityRejected.ToString());
                    lv.Items.Add(item1);
                }


            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static mrndetail getItemWiseTotalQuantForPerticularPOInMRN(int poRefNo)
        {
            mrndetail mrnd = new mrndetail();
            //List<mrnheader> MrnHeaders = new List<mrnheader>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                //string query = "select b.PONo,b.PODate, b.StockItemID,b.Name,b.ModelNo,b.ModelName,sum(b.QuantityAccepted) as TotQuant " +
                //        " from  ViewMRNDetail b,MRNHeader a " +
                //        "where a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate and a.DocumentStatus = 99"+
                //        " and b.PONo = "+ mrndet.PONO+" and b.PODate = '"+ mrndet.PODate.ToString("yyyy-MM-dd") + "'"+
                //        " and b.StockItemID = '"+ mrndet.StockItemID+"' and b.ModelNo = '" + mrndet.ModelNo+
                //        "' group by b.PONo,b.PODate,b.StockItemID,b.Name,b.ModelNo,b.ModelName";

                string query = "select b.POItemReferenceNo,sum(b.QuantityAccepted) as TotQuant " +
                        " from ViewMRNDetail b,MRNHeader a  " +
                        "where a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate and a.DocumentStatus = 99" +
                        " and b.POItemReferenceNo = " + poRefNo +
                        " group by b.POItemReferenceNo";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    mrnd.PORefNo = reader.GetInt32(0);
                    //mrnd.PODate = reader.GetDateTime(1);
                    //mrnd.StockItemID = reader.GetString(2);
                    //mrnd.StockItemName = reader.GetString(3);
                    //mrnd.ModelNo = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    //mrnd.ModelName = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    mrnd.Quantity = reader.GetDouble(1); // Total Quantity
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return mrnd;
        }

        public List<mrndetail> getItemwiseTotalMRNDetail(int MRNNo, DateTime MRNDate)
        {
            mrndetail mrnd;
            List<mrndetail> MrnDetails = new List<mrndetail>();
            try
            {
                string query =
                    "select StockItemID,Name,ModelNo,ModelName,sum(Quantity) as TotalQuantity from ViewMRNDetail " +
                    " where TemporaryNo = (select TemporaryNo from MRNHeader where MRNNo = '" + MRNNo + "' and MRNDate = '" + MRNDate.ToString("yyyy-MM-dd") + "')" +
                    " and TemporaryDate = (select TemporaryDate from MRNHeader where MRNNo = '" + MRNNo + "' and MRNDate = '" + MRNDate.ToString("yyyy-MM-dd") + "')" +
                    " group by StockItemID,Name,ModelNo,ModelName ";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        mrnd = new mrndetail();
                        mrnd.StockItemID = reader.GetString(0);
                        mrnd.StockItemName = reader.GetString(1);
                        mrnd.ModelNo = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        mrnd.ModelName = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        mrnd.Quantity = reader.GetDouble(4);
                        MrnDetails.Add(mrnd);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying MRN Details");
            }
            return MrnDetails;
        }
        public List<mrndetail> getMRNDetailFromMrnNoAndDate(int MRNNo, DateTime MRNDate, string DociD)
        {
            mrndetail mrnd;
            List<mrndetail> MrnDetails = new List<mrndetail>();
            try
            {
                string query =
                    "select PONo,PODate,StockItemID,Name,ModelNo,ModelName,Quantity,Price,TaxCode,RowID from ViewMRNDetail " +
                    " where TemporaryNo = (select TemporaryNo from MRNHeader where MRNNo = '" + MRNNo + "' and MRNDate = '" + MRNDate.ToString("yyyy-MM-dd") + "' and DocumentID = '" + DociD + "')" +
                    " and TemporaryDate = (select TemporaryDate from MRNHeader where MRNNo = '" + MRNNo + "' and MRNDate = '" + MRNDate.ToString("yyyy-MM-dd") + "' and DocumentID = '" + DociD + "')" +
                    " order by Name";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        mrnd = new mrndetail();
                        mrnd.PONO = reader.GetInt32(0);
                        mrnd.PODate = reader.GetDateTime(1);
                        mrnd.StockItemID = reader.GetString(2);
                        mrnd.StockItemName = reader.GetString(3);
                        mrnd.ModelNo = reader.IsDBNull(4) ? "NA" : reader.GetString(4);
                        mrnd.ModelName = reader.IsDBNull(5) ? "NA" : reader.GetString(5);
                        mrnd.Quantity = reader.GetDouble(6);
                        mrnd.Price = reader.GetDouble(7);
                        mrnd.TaxCode = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        mrnd.RowID = reader.GetInt32(9);
                        MrnDetails.Add(mrnd);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying MRN Details");
            }
            return MrnDetails;
        }
        public static ListView getMRNDetailListView(int MRNNo, DateTime MRNDate, string DociD)
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
                MRNHeaderDB mrnhdb = new MRNHeaderDB();
                List<mrndetail> MRNDetails = mrnhdb.getMRNDetailFromMrnNoAndDate(MRNNo, MRNDate, DociD);
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("PO Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItem ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItem Name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Model No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Model Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("Quantity", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Price", -2, HorizontalAlignment.Center);
                lv.Columns.Add("TaxCode", -2, HorizontalAlignment.Center);
                lv.Columns.Add("RowID", -2, HorizontalAlignment.Center);
                lv.Columns[10].Width = 0;
                foreach (mrndetail mrnd in MRNDetails)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(mrnd.PONO.ToString());
                    item1.SubItems.Add(mrnd.PODate.ToShortDateString());
                    item1.SubItems.Add(mrnd.StockItemID.ToString());
                    item1.SubItems.Add(mrnd.StockItemName.ToString());
                    item1.SubItems.Add(mrnd.ModelNo.ToString());
                    item1.SubItems.Add(mrnd.ModelName);
                    item1.SubItems.Add(mrnd.Quantity.ToString());
                    item1.SubItems.Add(mrnd.Price.ToString());
                    item1.SubItems.Add(mrnd.TaxCode);
                    item1.SubItems.Add(mrnd.RowID.ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static Boolean IsInvoiceNoFoundInPrevMRN(mrnheader mrnh)
        {
            Boolean stat = false;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "";
                string query1 = "select count(*) from MRNHeader " +
                        " where CustomerID='" + mrnh.CustomerID + "' " +
                        " and InvoiceNo='" + mrnh.InvoiceNo + "' and  concat(TemporaryNo,TemporaryDate,DocumentID) <> concat(" + mrnh.TemporaryNo +
                        ",'" + mrnh.TemporaryDate.ToString("yyyy-MM-dd") + "','" + mrnh.DocumentID + "') and Status <> 98";
                string query2 = "select count(*) from MRNHeader " +
                        " where CustomerID='" + mrnh.CustomerID + "' and InvoiceNo ='" + mrnh.InvoiceNo + "' and Status <> 98";
                if (mrnh.TemporaryNo == 0)
                {
                    query = query2;
                }
                else
                {
                    query = query1;
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (reader.GetInt32(0) > 0)
                    {
                        return true;
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }

            return stat;
        }


        public static List<stock> getMRNNoFromStock(string docid)
        {
            stock stk;
            List<stock> StockList = new List<stock>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                if (docid == "PURCHASERETURNQR")
                {
                    query = "select distinct a.MRNNo, a.MRNDate, a.CustomerID, c.Name from MRNHeader a left outer join MRNDetail b " +
                            " on a.DocumentID = b.DocumentID and a.TemporaryNo = b.TemporaryNo and a.TemporaryDate = b.TemporaryDate " +
                            " left outer join Customer c on a.CustomerID = c.CustomerID " +
                            " where a.Status = 1 and a.DocumentStatus = 99 and b.QuantityRejected > b.QuantityReturned ";
                }
                else
                {
                    query = "select distinct a.MRNNo, a.MRNDate, a.CustomerID, c.Name from MRNHeader a left outer join Stock b " +
                            " on a.DocumentID = b.InwardDocumentID and a.MRNNo = b.MRNNo and a.MRNDate = b.MRNDate " +
                            " left outer join Customer c on a.CustomerID = c.CustomerID " +
                            " where a.Status = 1 and a.DocumentStatus = 99 and b.PresentStock > 0";
                }


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
        public static ListView getMRNNoWithStockDetail(string docid)
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
                lv.Columns.Add("MRN No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("MRN Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SupplierID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SupplierName", -2, HorizontalAlignment.Left);

                List<stock> StockList = MRNHeaderDB.getMRNNoFromStock(docid);
                foreach (stock stk in StockList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = false;
                    item.SubItems.Add(stk.MRNNo.ToString());
                    item.SubItems.Add(stk.MRNDate.ToShortDateString());
                    item.SubItems.Add(stk.SupplierID);
                    item.SubItems.Add(stk.SupplierName);
                    lv.Items.Add(item);
                }

            }
            catch (Exception ex)
            {

            }
            return lv;
        }
        public List<mrndetail> getMRNDetailWithRejectedQuantity(int MRNNo, DateTime MRNDate, string DociD)
        {
            mrndetail mrnd;
            List<mrndetail> MrnDetails = new List<mrndetail>();
            try
            {
                string query =
                    "select PONo,PODate,StockItemID,Name,ModelNo,ModelName,QuantityRejected,Price,TaxCode,RowID,QuantityReturned,StoreLocationID,Description from ViewMRNDetail " +
                    " where TemporaryNo = (select TemporaryNo from MRNHeader where MRNNo = '" + MRNNo + "' and MRNDate = '" + MRNDate.ToString("yyyy-MM-dd") + "' and DocumentID = '" + DociD + "')" +
                    " and TemporaryDate = (select TemporaryDate from MRNHeader where MRNNo = '" + MRNNo + "' and MRNDate = '" + MRNDate.ToString("yyyy-MM-dd") + "' and DocumentID = '" + DociD + "')" +
                    " and QuantityRejected > QuantityReturned order by Name";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    try
                    {
                        mrnd = new mrndetail();
                        mrnd.PONO = reader.GetInt32(0);
                        mrnd.PODate = reader.GetDateTime(1);
                        mrnd.StockItemID = reader.GetString(2);
                        mrnd.StockItemName = reader.GetString(3);
                        mrnd.ModelNo = reader.IsDBNull(4) ? "NA" : reader.GetString(4);
                        mrnd.ModelName = reader.IsDBNull(5) ? "NA" : reader.GetString(5);
                        mrnd.QuantityRejected = reader.GetDouble(6);
                        mrnd.Price = reader.GetDouble(7);
                        mrnd.TaxCode = reader.IsDBNull(8) ? "" : reader.GetString(8);
                        mrnd.RowID = reader.GetInt32(9);
                        mrnd.QuantityReturned = reader.IsDBNull(10) ? 0 : reader.GetDouble(10);
                        mrnd.StoreLocationID = reader.IsDBNull(11) ? "" : reader.GetString(11);
                        mrnd.Description = reader.IsDBNull(12) ? "" : reader.GetString(12);
                        MrnDetails.Add(mrnd);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying MRN Details");
            }
            return MrnDetails;
        }
        public static ListView getMRNNoWiseMRNDetailLV(int MRNNo, DateTime MRNDate)
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
                lv.Columns.Add("MRNRefNO", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItemID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StockItemName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ModelNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("ModelName", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Rejected Quant", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Returned Quant", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Price", -2, HorizontalAlignment.Left);
                lv.Columns.Add("TaxCode", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StoreLocID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("StoreLocName", -2, HorizontalAlignment.Left);

                MRNHeaderDB mrndb = new MRNHeaderDB();
                List<mrndetail> MRNDetList = mrndb.getMRNDetailWithRejectedQuantity(MRNNo, MRNDate, "MRN");
                foreach (mrndetail mrnd in MRNDetList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = false;
                    item.SubItems.Add(mrnd.RowID.ToString());
                    item.SubItems.Add(mrnd.StockItemID.ToString());
                    item.SubItems.Add(mrnd.StockItemName.ToString());
                    item.SubItems.Add(mrnd.ModelNo.ToString());
                    item.SubItems.Add(mrnd.ModelName.ToString());
                    item.SubItems.Add(mrnd.QuantityRejected.ToString());
                    item.SubItems.Add(mrnd.QuantityReturned.ToString());
                    item.SubItems.Add(mrnd.Price.ToString());
                    item.SubItems.Add(mrnd.TaxCode.ToString());
                    item.SubItems.Add(mrnd.StoreLocationID.ToString());
                    item.SubItems.Add(mrnd.Description.ToString());
                    lv.Items.Add(item);
                }

            }
            catch (Exception ex)
            {

            }
            return lv;
        }
        public static Boolean verifyRejectedStockAvailability(int itemRefNo, double Qunt)
        {
            Boolean status = true;
            double RejStock = 0;
            try
            {
                double RetQuant = 0;
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select QuantityRejected,QuantityReturned" +
                   " from MRNDetail where RowID =" + itemRefNo;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                //SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    RejStock = reader.GetDouble(0);
                    RetQuant = reader.IsDBNull(1) ? 0 : reader.GetDouble(1);
                }
                conn.Close();
                if ((Qunt + RetQuant) > RejStock)
                    return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Stock Details");
                return false;
            }
            return status;
        }
    }
}
