using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class indentserviceheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int WORequestNo { get; set; }
        public DateTime WORequestDate { get; set; }
        public string ReferenceInternalOrder { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CurrencyID { get; set; }
        public decimal ExchangeRate { get; set; }
        public string CurrencyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TargetDate { get; set; }
        public String PaymentTerms { get; set; }
        public String PaymentMode { get; set; }

        public double ServiceValue { get; set; }
        public double TaxAmount { get; set; }
        public double TotalAmount { get; set; }
        public double ServiceValueINR { get; set; }
        public double TaxAmountINR { get; set; }
        public double TotalAmountINR { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
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
        public int NoOfWOFound { get; set; }

        public string ContractorReference { get; set; }
    }
    class indentservicedetail
    {
        public int RowID { get; set; }
        public int POItemRefNo { get; set; }
        public string DocumentID { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string StockItemID { get; set; }
        public string Description { get; set; }
        public string WorkDescription { get; set; }
        public string WorkLocation { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public double Rate { get; set; }
        public double Tax { get; set; }
        public int WarrantyDays { get; set; }
        public string TaxDetails { get; set; }
        public string TaxCode { get; set; }
    }
    class IndentServiceDB
    {
        public List<indentserviceheader> getFilteredIndentServiceHeaders(string userList, int opt, string userCommentStatusString)
        {
            indentserviceheader ish;
            List<indentserviceheader> ISHeaders = new List<indentserviceheader>();
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
                string query1 = "select a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate," +
                    " a.WORequestNo,a.WORequestDate,a.ReferenceInternalOrder,a.CustomerID,a.CustomerName,a.CurrencyID,a.CurrencyName,a.StartDate,a.TargetDate,a.PaymentTerms,a.PaymentMode," +
                    " a.ServiceValue,a.TaxAmount,a.TotalAmount,a.Remarks, " +
                    " a.Status,a.DocumentStatus,a.CreateTime,a.CreateUser,a.ForwardUser,a.ApproveUser,a.CreatorName,a.ForwarderName,a.ApproverName,a.CommentStatus,a.ForwarderList " +
                    " ,a.ExchangeRate,a.ServiceValueINR,a.TaxAmountINR,a.TotalAmountINR,b.NoFound,a.ContractorReference from ViewWORequestHeader a left outer join " +
                    " (select DocumentID,WORequestNo,WORequestDate,COUNT(*) as NoFound from WOHeader where Status = 1 and DocumentStatus = 99 group by DocumentID,WORequestNo,WORequestDate) b on" +
                    " a.WORequestNo = b.WORequestNo and a.WORequestDate = b.WORequestDate " +

                   " where a.DocumentID = 'INDENTSERVICE' and ((a.forwarduser='" + Login.userLoggedIn + "' and a.DocumentStatus between 2 and 98) " +
                    " or (a.createuser='" + Login.userLoggedIn + "' and a.DocumentStatus=1)" +
                    " or (a.commentStatus like '%" + userCommentStatusString + "%' and a.DocumentStatus between 1 and 98)) and a.Status not in (7,98) order by a.TemporaryDate desc,a.DocumentID asc,a.TemporaryNo desc";

                string query2 = "select a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate," +
                    " a.WORequestNo,a.WORequestDate,a.ReferenceInternalOrder,a.CustomerID,a.CustomerName,a.CurrencyID,a.CurrencyName,a.StartDate,a.TargetDate,a.PaymentTerms,a.PaymentMode," +
                    " a.ServiceValue,a.TaxAmount,a.TotalAmount,a.Remarks, " +
                    " a.Status,a.DocumentStatus,a.CreateTime,a.CreateUser,a.ForwardUser,a.ApproveUser,a.CreatorName,a.ForwarderName,a.ApproverName,a.CommentStatus,a.ForwarderList " +
                    " ,a.ExchangeRate,a.ServiceValueINR,a.TaxAmountINR,a.TotalAmountINR,b.NoFound,a.ContractorReference from ViewWORequestHeader a left outer join " +
                    " (select DocumentID,WORequestNo,WORequestDate,COUNT(*) as NoFound from WOHeader where Status = 1 and DocumentStatus = 99 group by DocumentID,WORequestNo,WORequestDate) b on" +
                    " a.WORequestNo = b.WORequestNo and a.WORequestDate = b.WORequestDate " +

                   " where a.DocumentID = 'INDENTSERVICE' and ((a.createuser='" + Login.userLoggedIn + "'  and a.DocumentStatus between 2 and 98 ) " +
                    " or (a.ForwarderList like '%" + userList + "%' and a.DocumentStatus between 2 and 98 and a.ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (a.commentStatus like '%" + acStr + "%' and a.DocumentStatus between 1 and 98)) and a.Status not in (7,98) order by a.TemporaryDate desc,a.DocumentID asc,a.TemporaryNo desc";

                string query3 = "select a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate," +
                    " a.WORequestNo,a.WORequestDate,a.ReferenceInternalOrder,a.CustomerID,a.CustomerName,a.CurrencyID,a.CurrencyName,a.StartDate,a.TargetDate,a.PaymentTerms,a.PaymentMode," +
                    " a.ServiceValue,a.TaxAmount,a.TotalAmount,a.Remarks, " +
                    " a.Status,a.DocumentStatus,a.CreateTime,a.CreateUser,a.ForwardUser,a.ApproveUser,a.CreatorName,a.ForwarderName,a.ApproverName,a.CommentStatus,a.ForwarderList " +
                    " ,a.ExchangeRate,a.ServiceValueINR,a.TaxAmountINR,a.TotalAmountINR,b.NoFound,a.ContractorReference from ViewWORequestHeader a left outer join " +
                    " (select DocumentID,WORequestNo,WORequestDate,COUNT(*) as NoFound from WOHeader where Status = 1 and DocumentStatus = 99 group by DocumentID,WORequestNo,WORequestDate) b on" +
                    " a.WORequestNo = b.WORequestNo and a.WORequestDate = b.WORequestDate " +

                   " where a.DocumentID = 'INDENTSERVICE' and ((a.createuser='" + Login.userLoggedIn + "'" +
                    " or a.ForwarderList like '%" + userList + "%'" +
                    " or a.commentStatus like '%" + acStr + "%'" +
                    " or a.approveUser='" + Login.userLoggedIn + "')" +
                    " and a.DocumentStatus = 99) and a.status = 1 order by a.WORequestDate desc,a.DocumentID asc,a.WORequestNo desc";
                string query6 = "select a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate," +
                    " a.WORequestNo,a.WORequestDate,a.ReferenceInternalOrder,a.CustomerID,a.CustomerName,a.CurrencyID,a.CurrencyName,a.StartDate,a.TargetDate,a.PaymentTerms,a.PaymentMode," +
                    " a.ServiceValue,a.TaxAmount,a.TotalAmount,a.Remarks, " +
                    " a.Status,a.DocumentStatus,a.CreateTime,a.CreateUser,a.ForwardUser,a.ApproveUser,a.CreatorName,a.ForwarderName,a.ApproverName,a.CommentStatus,a.ForwarderList " +
                    " ,a.ExchangeRate,a.ServiceValueINR,a.TaxAmountINR,a.TotalAmountINR,b.NoFound,a.ContractorReference from ViewWORequestHeader a left outer join " +
                    " (select DocumentID,WORequestNo,WORequestDate,COUNT(*) as NoFound from WOHeader where Status = 1 and DocumentStatus = 99 group by DocumentID,WORequestNo,WORequestDate) b on" +
                    " a.WORequestNo = b.WORequestNo and a.WORequestDate = b.WORequestDate " +

                   " where a.DocumentID = 'INDENTSERVICE' and  a.DocumentStatus = 99 and a.status = 1  order by a.WORequestDate desc,a.DocumentID asc,a.WORequestNo desc";
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
                    ish = new indentserviceheader();
                    ish.RowID = reader.GetInt32(0);
                    ish.DocumentID = reader.GetString(1);
                    ish.DocumentName = reader.GetString(2);
                    ish.TemporaryNo = reader.GetInt32(3);
                    ish.TemporaryDate = reader.GetDateTime(4);
                    ish.WORequestNo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        ish.WORequestDate = reader.GetDateTime(6);
                    }
                    else
                        ish.WORequestDate = DateTime.Parse("1900-01-01");
                    ish.ReferenceInternalOrder = reader.IsDBNull(7) ? " " : reader.GetString(7);
                    ish.CustomerID = reader.GetString(8);
                    ish.CustomerName = reader.GetString(9);
                    ish.CurrencyID = reader.GetString(10);
                    ish.CurrencyName = reader.GetString(11);
                    ish.StartDate = reader.GetDateTime(12);
                    ish.TargetDate = reader.GetDateTime(13);
                    ish.PaymentTerms = reader.GetString(14);
                    ish.PaymentMode = reader.GetString(15);
                    ish.ServiceValue = reader.GetDouble(16);
                    ish.TaxAmount = reader.GetDouble(17);
                    ish.TotalAmount = reader.GetDouble(18);
                    ish.Remarks = reader.GetString(19);
                    ish.Status = reader.GetInt32(20);
                    ish.DocumentStatus = reader.GetInt32(21);
                    ish.CreateTime = reader.GetDateTime(22);
                    ish.CreateUser = reader.GetString(23);
                    ish.ForwardUser = reader.GetString(24);
                    ish.ApproveUser = reader.GetString(25);
                    ish.CreatorName = reader.GetString(26);
                    ish.ForwarderName = reader.GetString(27);
                    ish.ApproverName = reader.GetString(28);
                    if (!reader.IsDBNull(29))
                    {
                        ish.CommentStatus = reader.GetString(29);
                    }
                    else
                    {
                        ish.CommentStatus = "";
                    }
                    if (!reader.IsDBNull(30))
                    {
                        ish.ForwarderList = reader.GetString(30);
                    }
                    else
                    {
                        ish.ForwarderList = "";
                    }
                    ish.ExchangeRate = reader.GetDecimal(31);
                    ish.ServiceValueINR = reader.GetDouble(32);
                    ish.TaxAmountINR = reader.GetDouble(33);
                    ish.TotalAmountINR = reader.GetDouble(34);
                    ish.NoOfWOFound = reader.IsDBNull(35)?0:reader.GetInt32(35);
                    ish.ContractorReference = reader.IsDBNull(36) ? "" : reader.GetString(36);
                    ISHeaders.Add(ish);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Indent Service Header Details");
            }
            return ISHeaders;
        }

        public static List<indentservicedetail> getIndentServiceDetails(indentserviceheader ish)
        {
            indentservicedetail isd;
            List<indentservicedetail> ISDetail = new List<indentservicedetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select a.RowID,a.DocumentID,a.TemporaryNo, a.TemporaryDate,a.StockItemID,b.Name as Description,a.WorkDescription,a.WorkLocation, " +
                   "a.Quantity,a.Price,a.Tax,a.WarrantyDays,a.TaxDetails,a.TaxCode,a.POItemReferenceNo " +
                   "from WORequestDetail a , ServiceItem b where a.StockItemID = b.ServiceItemID " +
                   " and a.DocumentID='" + ish.DocumentID + "'" +
                   " and a.TemporaryNo=" + ish.TemporaryNo +
                   " and a.TemporaryDate='" + ish.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    isd = new indentservicedetail();
                    isd.RowID = reader.GetInt32(0);
                    isd.DocumentID = reader.GetString(1);
                    isd.TemporaryNo = reader.GetInt32(2);
                    isd.TemporaryDate = reader.GetDateTime(3).Date;
                    isd.StockItemID = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    isd.Description = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    isd.WorkDescription = reader.GetString(6);
                    isd.WorkLocation = reader.GetString(7);
                    isd.Quantity = reader.GetDouble(8);
                    isd.Rate = reader.GetDouble(9);  //Price INdent
                    isd.Tax = reader.GetDouble(10);
                    isd.WarrantyDays = reader.GetInt32(11);
                    isd.TaxDetails = reader.GetString(12);
                    isd.TaxCode = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    isd.POItemRefNo = reader.IsDBNull(14) ? 0 : reader.GetInt32(14);  //PO Item Row ID
                    ISDetail.Add(isd);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Work Order Details");
            }
            return ISDetail;
        }
        public Boolean validateIndentServiceHeader(indentserviceheader ish)
        {
            Boolean status = true;
            try
            {
                if (ish.DocumentID.Trim().Length == 0 || ish.DocumentID == null)
                {
                    return false;
                }
                if (ish.ReferenceInternalOrder.Trim().Length == 0 || ish.ReferenceInternalOrder == null)
                {
                    return false;
                }

                if (ish.CustomerID.Trim().Length == 0 || ish.CustomerID == null)
                {
                    return false;
                }
                if (ish.CurrencyID.Trim().Length == 0 || ish.CurrencyID == null)
                {
                    return false;
                }
                if (ish.StartDate == null)
                {
                    return false;
                }
                if (ish.TargetDate < DateTime.Now.Date || ish.TargetDate < ish.StartDate || ish.TargetDate == null)
                {
                    return false;
                }
                if (ish.PaymentTerms == null)
                {
                    return false;
                }
                if (ish.PaymentMode == null)
                {
                    return false;
                }
                if (ish.ServiceValue == 0)
                {
                    return false;
                }
                if (ish.TotalAmount == 0)
                {
                    return false;
                }
                if (ish.ExchangeRate == 0)
                {
                    return false;
                }
                if (ish.Remarks.Trim().Length == 0 || ish.Remarks == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public Boolean forwardIndentService(indentserviceheader ish)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WORequestHeader set DocumentStatus=" + (ish.DocumentStatus + 1) +
                     ", forwardUser='" + ish.ForwardUser + "'" +
                    ", commentStatus='" + ish.CommentStatus + "'" +
                    ", ForwarderList='" + ish.ForwarderList + "'" +
                    " where DocumentID='" + ish.DocumentID + "'" +
                    " and TemporaryNo=" + ish.TemporaryNo +
                    " and TemporaryDate='" + ish.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WORequestHeader", "", updateSQL) +
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
        public Boolean reverseIndentService(indentserviceheader ish)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WORequestHeader set DocumentStatus=" + ish.DocumentStatus +
                    ", forwardUser='" + ish.ForwardUser + "'" +
                    ", commentStatus='" + ish.CommentStatus + "'" +
                    ", ForwarderList='" + ish.ForwarderList + "'" +
                    " where DocumentID='" + ish.DocumentID + "'" +
                    " and TemporaryNo=" + ish.TemporaryNo +
                    " and TemporaryDate='" + ish.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WORequestHeader", "", updateSQL) +
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
        public Boolean ApproveIndentService(indentserviceheader ish)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WORequestHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + ish.CommentStatus + "'" +
                    ", WORequestNo=" + ish.WORequestNo +
                    ", WORequestDate=convert(date, getdate())" +
                    " where DocumentID='" + ish.DocumentID + "'" +
                    " and TemporaryNo=" + ish.TemporaryNo +
                    " and TemporaryDate='" + ish.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WORequestHeader", "", updateSQL) +
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
                string query = "select comments from WORequestHeader where DocumentID='" + docid + "'" +
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
        public Boolean updateIndentServiceHeaderAndDetail(indentserviceheader ish, indentserviceheader prevish, List<indentservicedetail> ISHDetail)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WORequestHeader set TemporaryNo = " + ish.TemporaryNo +
                   ", TemporaryDate='" + ish.TemporaryDate.ToString("yyyy-MM-dd") +
                   //"', WORequestNo=" + ish.WORequestNo +
                   //", WORequestDate='" + ish.WORequestDate.ToString("yyyy-MM-dd") +
                   "', ReferenceInternalOrder='" + ish.ReferenceInternalOrder +
                   "', CustomerID='" + ish.CustomerID +
                   "', CurrencyID='" + ish.CurrencyID +
                   "', ExchangeRate=" + ish.ExchangeRate +
                    ", StartDate='" + ish.StartDate.ToString("yyyy-MM-dd") +
                    "', TargetDate='" + ish.TargetDate.ToString("yyyy-MM-dd") +
                   "', PaymentTerms='" + ish.PaymentTerms +
                   "', PaymentMode='" + ish.PaymentMode +
                   "', ServiceValue=" + ish.ServiceValue +
                   ",TaxAmount=" + ish.TaxAmount + "," +
                   "TotalAmount= " + ish.TotalAmount +
                    ", ServiceValueINR=" + ish.ServiceValueINR +
                   ",TaxAmountINR=" + ish.TaxAmountINR + "," +
                   "TotalAmountINR= " + ish.TotalAmountINR +
                   ", Remarks ='" + ish.Remarks +
                    "', ContractorReference ='" + ish.ContractorReference +
                   "', CommentStatus='" + ish.CommentStatus +
                   "', Comments='" + ish.Comments +
                   "', ForwarderList='" + ish.ForwarderList + "'" +
                  " where DocumentID='" + prevish.DocumentID + "'" +
                  " and TemporaryNo=" + prevish.TemporaryNo +
                  " and TemporaryDate='" + prevish.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WORequestHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from WORequestDetail where DocumentID='" + prevish.DocumentID + "'" +
                     " and TemporaryNo=" + prevish.TemporaryNo +
                     " and TemporaryDate='" + prevish.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "WORequestDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (indentservicedetail isd in ISHDetail)
                {
                    updateSQL = "insert into WORequestDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,WorkDescription,TaxCode,WorkLocation,Quantity,Price,Tax,POItemReferenceNo,WarrantyDays,TaxDetails) " +
                    "values ('" + isd.DocumentID + "'," +
                    isd.TemporaryNo + "," +
                    "'" + isd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       "'" + isd.StockItemID + "'," +
                    "'" + isd.WorkDescription + "'," +
                     "'" + isd.TaxCode + "'," +
                    "'" + isd.WorkLocation + "'," +
                    isd.Quantity + "," +
                    isd.Rate + " ," +   //Indent Price
                    isd.Tax + "," +
                    isd.POItemRefNo + "," +
                    isd.WarrantyDays + "," +
                    "'" + isd.TaxDetails + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "WORequestDetail", "", updateSQL) +
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
        public Boolean InsertIndentServiceHeaderAndDetail(indentserviceheader ish, List<indentservicedetail> ISDetail)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                ish.TemporaryNo = DocumentNumberDB.getNumber(ish.DocumentID, 1);
                if (ish.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + ish.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + ish.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into WORequestHeader " +
                    "(DocumentID,TemporaryNo,TemporaryDate,WORequestNo,WORequestDate,ReferenceInternalOrder,CustomerID,CurrencyID," +
                    "ExchangeRate,StartDate,TargetDate,PaymentTerms,PaymentMode,ServiceValue,TaxAmount,TotalAmount,ServiceValueINR,TaxAmountINR,TotalAmountINR," +
                    "Remarks,Status,DocumentStatus,CreateTime,CreateUser, CommentStatus,Comments,ContractorReference,ForwarderList)" +
                    " values (" +
                    "'" + ish.DocumentID + "'," +
                    ish.TemporaryNo + "," +
                    "'" + ish.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    ish.WORequestNo + "," +
                    "'" + ish.WORequestDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + ish.ReferenceInternalOrder + "'," +
                    "'" + ish.CustomerID + "'," +
                    "'" + ish.CurrencyID + "'," +
                    ish.ExchangeRate + "," +
                    "'" + ish.StartDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + ish.TargetDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + ish.PaymentTerms + "'," +
                    "'" + ish.PaymentMode + "'," +
                    ish.ServiceValue + "," +
                    ish.TaxAmount + "," +
                    ish.TotalAmount + "," +
                     ish.ServiceValueINR + "," +
                    ish.TaxAmountINR + "," +
                    ish.TotalAmountINR + "," +
                    "'" + ish.Remarks + "'," +
                    ish.Status + "," +
                    ish.DocumentStatus + "," +
                     "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'," +
                    "'" + ish.CommentStatus + "'," +
                    "'" + ish.Comments + "'," +
                    "'" + ish.ContractorReference + "'," +
                    "'" + ish.ForwarderList + "')";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "WORequestHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from WORequestDetail where DocumentID='" + ish.DocumentID + "'" +
                    " and TemporaryNo=" + ish.TemporaryNo +
                    " and TemporaryDate='" + ish.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "WORequestDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (indentservicedetail isd in ISDetail)
                {
                    updateSQL = "insert into WORequestDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,WorkDescription,TaxCode,WorkLocation,Quantity,Price,Tax,POItemReferenceNo,WarrantyDays,TaxDetails) " +
                    "values ('" + isd.DocumentID + "'," +
                    ish.TemporaryNo + "," +
                    "'" + isd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                       "'" + isd.StockItemID + "'," +
                    "'" + isd.WorkDescription + "'," +
                        "'" + isd.TaxCode + "'," +
                    "'" + isd.WorkLocation + "'," +
                    isd.Quantity + "," +
                    isd.Rate + " ," +
                    isd.Tax + "," +
                     isd.POItemRefNo + "," +
                    isd.WarrantyDays + "," +
                    "'" + isd.TaxDetails + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "WORequestDetail", "", updateSQL) +
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

        public List<indentserviceheader> getIndentServiceListView()
        {
            indentserviceheader ish;
            List<indentserviceheader> ISHeaders = new List<indentserviceheader>();
            try
            {
                string query = "select WORequestNo,WORequestDate,CustomerID,CustomerName,ContractorReference from ViewWORequestHeader" +
                   " where DocumentID = 'INDENTSERVICE' and Status = 1 and DocumentStatus = 99  order by WORequestDate asc,WORequestNo asc";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ish = new indentserviceheader();
                    ish.WORequestNo = reader.GetInt32(0);
                    ish.WORequestDate = reader.GetDateTime(1);
                    ish.CustomerID = reader.GetString(2);
                    ish.CustomerName = reader.GetString(3);
                    ish.ContractorReference = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    ISHeaders.Add(ish);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Indent Service Header Details");
            }
            return ISHeaders;
        }
        public static ListView IndentServiceSelectionListView()
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
                IndentServiceDB isdb = new IndentServiceDB();
                List<indentserviceheader> ISHeader = isdb.getIndentServiceListView();
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Indent No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Indent Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Contractor ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Contractor Name", -2, HorizontalAlignment.Left);
                //lv.Columns.Add("Contractor Ref", -2, HorizontalAlignment.Left);
                lv.Columns[3].Width = 0;
                ///lv.Columns[5].Width = 0;
                foreach (indentserviceheader ish in ISHeader)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(ish.WORequestNo.ToString());
                    item1.SubItems.Add(ish.WORequestDate.ToShortDateString());
                    item1.SubItems.Add(ish.CustomerID);
                    item1.SubItems.Add(ish.CustomerName);
                    ////item1.SubItems.Add(ish.ContractorReference);
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }

        //Detail Indent list for closoing PAF
        public static List<indentserviceheader> IndentDetailListWRTRefPONos(string refPONos)
        {
            List<indentserviceheader> IndentList = new List<indentserviceheader>();
            indentserviceheader ish = new indentserviceheader();

            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.DocumentID,a.TemporaryNo,a.TemporaryDate,a.ReferenceInternalOrder,a.CustomerID,b.Name from WORequestHeader a , Customer b " +
                        " where a.CustomerID = b.CustomerID and ReferenceInternalOrder like '%" + refPONos + "%'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ish = new indentserviceheader();
                    ish.DocumentID = reader.GetString(0);
                    ish.TemporaryNo = reader.GetInt32(1);
                    ish.TemporaryDate = reader.GetDateTime(2);
                    ish.ReferenceInternalOrder = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    ish.CustomerID = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    ish.CustomerName = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    IndentList.Add(ish);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return IndentList;
        }

        public static Boolean CloseIndentService(indentserviceheader ish)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WORequestHeader set Status = 7 " +
                    " where DocumentID='" + ish.DocumentID + "'" +
                    " and TemporaryNo=" + ish.TemporaryNo +
                    " and TemporaryDate='" + ish.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WORequestHeader", "", updateSQL) +
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
        public static Boolean isWOPreparedForIS(int indentNo, DateTime indentDate)
        {
            Boolean isAvail = false ;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select count(*) from WOHeader where " +
                        " WORequestNo=" + indentNo +
                        " and WORequestDate='" + indentDate.ToString("yyyy-MM-dd") + "' and status = 1 and DocumentStatus = 99";
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

        public indentserviceheader getInvdentServiceForWO(int No, DateTime date)
        {
            indentserviceheader ish = new indentserviceheader() ;
            try
            {
                string query = "select a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate," +
                    " a.WORequestNo,a.WORequestDate,a.ReferenceInternalOrder,a.CustomerID,a.CustomerName,a.CurrencyID,a.CurrencyName,a.StartDate,a.TargetDate,a.PaymentTerms,a.PaymentMode," +
                    " a.ServiceValue,a.TaxAmount,a.TotalAmount,a.Remarks, " +
                    " a.Status,a.DocumentStatus,a.CreateTime,a.CreateUser,a.ForwardUser,a.ApproveUser,a.CreatorName,a.ForwarderName,a.ApproverName,a.CommentStatus,a.ForwarderList " +
                    " ,a.ExchangeRate,a.ServiceValueINR,a.TaxAmountINR,a.TotalAmountINR,b.NoFound,a.ContractorReference from ViewWORequestHeader a left outer join " +
                    " (select DocumentID,WORequestNo,WORequestDate,COUNT(*) as NoFound from WOHeader where Status = 1 and DocumentStatus = 99 group by DocumentID,WORequestNo,WORequestDate) b on" +
                    " a.WORequestNo = b.WORequestNo and a.WORequestDate = b.WORequestDate " +
                   " where a.DocumentID = 'INDENTSERVICE' and  a.DocumentStatus = 99 and a.status = 1 and a.WORequestNo = " + No +
                   " and a.WORequestDate = '" + date.ToString("yyyy-MM-dd") + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    ish = new indentserviceheader();
                    ish.RowID = reader.GetInt32(0);
                    ish.DocumentID = reader.GetString(1);
                    ish.DocumentName = reader.GetString(2);
                    ish.TemporaryNo = reader.GetInt32(3);
                    ish.TemporaryDate = reader.GetDateTime(4);
                    ish.WORequestNo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        ish.WORequestDate = reader.GetDateTime(6);
                    }
                    else
                        ish.WORequestDate = DateTime.Parse("1900-01-01");
                    ish.ReferenceInternalOrder = reader.IsDBNull(7) ? " " : reader.GetString(7);
                    ish.CustomerID = reader.GetString(8);
                    ish.CustomerName = reader.GetString(9);
                    ish.CurrencyID = reader.GetString(10);
                    ish.CurrencyName = reader.GetString(11);
                    ish.StartDate = reader.GetDateTime(12);
                    ish.TargetDate = reader.GetDateTime(13);
                    ish.PaymentTerms = reader.GetString(14);
                    ish.PaymentMode = reader.GetString(15);
                    ish.ServiceValue = reader.GetDouble(16);
                    ish.TaxAmount = reader.GetDouble(17);
                    ish.TotalAmount = reader.GetDouble(18);
                    ish.Remarks = reader.GetString(19);
                    ish.Status = reader.GetInt32(20);
                    ish.DocumentStatus = reader.GetInt32(21);
                    ish.CreateTime = reader.GetDateTime(22);
                    ish.CreateUser = reader.GetString(23);
                    ish.ForwardUser = reader.GetString(24);
                    ish.ApproveUser = reader.GetString(25);
                    ish.CreatorName = reader.GetString(26);
                    ish.ForwarderName = reader.GetString(27);
                    ish.ApproverName = reader.GetString(28);
                    if (!reader.IsDBNull(29))
                    {
                        ish.CommentStatus = reader.GetString(29);
                    }
                    else
                    {
                        ish.CommentStatus = "";
                    }
                    if (!reader.IsDBNull(30))
                    {
                        ish.ForwarderList = reader.GetString(30);
                    }
                    else
                    {
                        ish.ForwarderList = "";
                    }
                    ish.ExchangeRate = reader.GetDecimal(31);
                    ish.ServiceValueINR = reader.GetDouble(32);
                    ish.TaxAmountINR = reader.GetDouble(33);
                    ish.TotalAmountINR = reader.GetDouble(34);
                    ish.NoOfWOFound = reader.IsDBNull(35) ? 0 : reader.GetInt32(35);
                    ish.ContractorReference = reader.IsDBNull(36) ? "" : reader.GetString(36);
                }
                else
                    ish = null;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Indent Service Header Details");
                ish = null;
            }
            return ish;
        }

    }
}
