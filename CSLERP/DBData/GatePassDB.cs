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
    public class gatepassheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int GatePassNo { get; set; }
        public DateTime GatePassDate { get; set; }
        public string FromOffice { get; set; }
        public string FromOfficeName { get; set; }
        public string ToOffice { get; set; }
        public string ToOfficeName { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string AcceptedUser { get; set; }
        public int AcceptanceStatus { get; set; }
        public DateTime AcceptanceDate { get; set; }
        public string Comments { get; set; }
        public string CommentStatus { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string ForwarderList { get; set; }
        public int Status { get; set; }
        public int DocumentStatus { get; set; }
        public int ReturnStatus { get; set; }
        public string Remarks { get; set; }
        public string SpecialNotes { get; set; }
    }
    public class gatepassdetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string StockitemID { get; set; }
        public string StockItemName { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public double Value { get; set; }
        public double Quantity { get; set; }
        public double ReturningQuantity { get; set; }
        public double ReturnedQuantity { get; set; }
        public int MRNNo { get; set; }
        public DateTime MRNDate { get; set; }
        public string BatchNo { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double PurchaseQuantity { get; set; }
        public double PurchasePrice { get; set; }
        public double PurchaseTax { get; set; }
        public string SupplierID { get; set; }
        public string SupplierName { get; set; }
        public string SerialNo { get; set; }
        public int refNo { get; set; }
    }
    class GatePassDB
    {
        public List<gatepassheader> getFilteredGPHeader(string userList, int opt, string userCommentStatusString)
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
            gatepassheader gheader;
            List<gatepassheader> gheaderlist = new List<gatepassheader>();
            try
            {
                string query1 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " GatePassNO,GatePassDate,FromOffice,FromOfficeName,ToOffice,ToOfficeName,CustomerID,CustomerName,Comments,Commentstatus,AcceptanceDate,AcceptedUser,AcceptanceStatus," +
                    "CreateUser,CreateTime,ForwardUser,ApproveUser,ForwarderList,Status,DocumentStatus,ReturnStatus,Remarks,SpecialNotes" +
                    " from ViewInventoryGatepassHeader" +
                   " where ((forwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                    " or (commentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98))  and Status not in (7,98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";

                //string query2 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                //    " GetPassNO,GetPassDate,FromOffice,FromOfficeName,ToOffice,ToOfficeName,CustomerID,CustomerName,Comments,Commentstatus,AcceptanceDate,AcceptedUser,AcceptanceStatus," +
                //    "CreateUser,CreateTime,ForwardUser,ApproveUser,ForwarderList,Status,DocumentStatus" +
                //    " from ViewInventoryGatepassHeader" +
                //   " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                //    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                //    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98))  and Status not in (7,98) order by TemporaryDate desc,DocumentID asc,TemporaryNo desc";
                string query6 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " GatePassNO,GatePassDate,FromOffice,FromOfficeName,ToOffice,ToOfficeName,CustomerID,CustomerName,Comments,Commentstatus,AcceptanceDate,AcceptedUser,AcceptanceStatus," +
                    "CreateUser,CreateTime,ForwardUser,ApproveUser,ForwarderList,Status,DocumentStatus,ReturnStatus,Remarks,SpecialNotes" +
                    " from ViewInventoryGatepassHeader" +
                    " where  DocumentStatus = 99 and Status = 1  order by GatePassDate desc,DocumentID asc,GatePassNO desc";

                string query4 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                  " GatePassNO,GatePassDate,FromOffice,FromOfficeName,ToOffice,ToOfficeName,CustomerID,CustomerName,Comments,Commentstatus,AcceptanceDate,AcceptedUser,AcceptanceStatus," +
                  "CreateUser,CreateTime,ForwardUser,ApproveUser,ForwarderList,Status,DocumentStatus,ReturnStatus,Remarks,SpecialNotes" +
                  " from ViewInventoryGatepassHeader" +
                  " where  DocumentStatus = 99 and Status = 1  and ForwarderList like '%" + userList + "%' order by GatePassDate desc,DocumentID asc,GatePassNO desc";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "";
                switch (opt)
                {
                    case 1:
                        query = query1;
                        break;
                    case 6:
                        query = query6;
                        break;
                    case 4:
                        query = query4;
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
                    gheader = new gatepassheader();
                    gheader.RowID = reader.GetInt32(0);
                    gheader.DocumentID = reader.GetString(1);
                    gheader.DocumentName = reader.GetString(2);
                    gheader.TemporaryNo = reader.GetInt32(3);
                    gheader.TemporaryDate = reader.GetDateTime(4);
                    gheader.GatePassNo = reader.GetInt32(5);
                    gheader.GatePassDate = reader.GetDateTime(6);
                    gheader.FromOffice = reader.GetString(7);
                    gheader.FromOfficeName = reader.GetString(8);
                    gheader.ToOffice = reader.GetString(9);
                    gheader.ToOfficeName = reader.GetString(10);
                    gheader.CustomerID = reader.IsDBNull(11)? "" : reader.GetString(11);
                    gheader.CustomerName = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    gheader.Comments = reader.GetString(13);
                    gheader.CommentStatus = reader.GetString(14);
                    gheader.AcceptanceDate = reader.GetDateTime(15);
                    gheader.AcceptedUser = reader.IsDBNull(16)?"":reader.GetString(16);
                    gheader.AcceptanceStatus = reader.GetInt32(17);
                    gheader.CreateUser = reader.GetString(18);
                    gheader.CreateTime = reader.GetDateTime(19);
                    gheader.ForwardUser = reader.IsDBNull(20) ? "" : reader.GetString(20);
                    gheader.ApproveUser = reader.IsDBNull(21) ? "" : reader.GetString(21);
                    gheader.ForwarderList = reader.GetString(22);
                    gheader.Status = reader.GetInt32(23);
                    gheader.DocumentStatus = reader.GetInt32(24);
                    gheader.ReturnStatus = reader.IsDBNull(25) ? 0 : reader.GetInt32(25);
                    gheader.Remarks = reader.IsDBNull(26) ? "" : reader.GetString(26);
                    gheader.SpecialNotes = reader.IsDBNull(27) ? "" : reader.GetString(27);
                    gheaderlist.Add(gheader);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying GTN Header Data");
            }
            return gheaderlist;
        }
        //public Boolean updateGTNInStock(List<getpassdetail> gtndList, string store)
        //{
        //    Boolean status = true;
        //    try
        //    {
        //        foreach (getpassdetail gtnd in gtndList)
        //        {
        //            if (!CheckStockAvailability(gtnd.refNo, gtnd.Quantity))
        //            {
        //                status = false;
        //                break;
        //            }
        //        }
        //        if (status)
        //        {
        //            //stock available. select rows from stock table. update stcok table. insert GTN records in stock table

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = false;
        //    }
        //    return status;
        //}
        //public Boolean CheckStockAvailability(int stockRefNo, double Qunt)
        //{
        //    Boolean status = false;

        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "select PresentStock" +
        //           " from Stock where RowID =" + stockRefNo;
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader.Read())
        //        {
        //            Double aqty = reader.GetDouble(0);
        //            if (aqty >= Qunt)
        //                status = true;
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("Error querying stock quantity");
        //        status = false;
        //    }
        //    return status;
        //}

        public static List<gatepassdetail> getGatePassdetail(gatepassheader gtnh)
        {
            gatepassdetail gtnd;
            List<gatepassdetail> getpassdetail = new List<gatepassdetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,TemporaryNo, TemporaryDate,StockItemID,StockItemName,ModelNo, ModelName,Quantity,QuantityReturning, " +
                    "QuantityReturned,MRNNo,MRNDate,BatchNo,SerialNo,ExpiryDate,PurchaseQuantity,PurchasePrice,PurchaseTax,SupplierID,SupplierName,StockReferenceNo,Value " +
                   "from ViewInventoryGatepassDetail " +
                   "where DocumentID='" + gtnh.DocumentID + "'" +
                   " and TemporaryNo=" + gtnh.TemporaryNo +
                   " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                   " order by StockItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    gtnd = new gatepassdetail();
                    gtnd.RowID = reader.GetInt32(0);
                    gtnd.DocumentID = reader.GetString(1);
                    gtnd.TemporaryNo = reader.GetInt32(2);
                    gtnd.TemporaryDate = reader.GetDateTime(3).Date;
                    gtnd.StockitemID = reader.GetString(4);
                    gtnd.StockItemName = reader.GetString(5);
                    gtnd.ModelNo = reader.IsDBNull(6)?"NA":reader.GetString(6);
                    gtnd.ModelName = reader.IsDBNull(7) ? "NA":reader.GetString(7);
                    gtnd.Quantity = reader.GetDouble(8);
                   
                    gtnd.ReturningQuantity = reader.GetDouble(9);
                    gtnd.ReturnedQuantity = reader.GetDouble(10);

                    gtnd.MRNNo = reader.IsDBNull(11)? 0:reader.GetInt32(11);
                    if(reader.IsDBNull(12))
                    {
                        gtnd.MRNDate = DateTime.Parse("1900-01-01");
                    }
                    else
                    {
                        gtnd.MRNDate = reader.GetDateTime(12).Date;
                    }
                    gtnd.BatchNo = reader.IsDBNull(13) ? "NA" : reader.GetString(13);
                    gtnd.SerialNo = reader.IsDBNull(14) ? "NA" : reader.GetString(14);
                    if (reader.IsDBNull(15))
                    {
                        gtnd.ExpiryDate = DateTime.Parse("1900-01-01");
                    }
                    else
                    {
                        gtnd.ExpiryDate = reader.GetDateTime(15);
                    }
                    gtnd.PurchaseQuantity = reader.GetDouble(16);
                    gtnd.PurchasePrice = reader.GetDouble(17);
                    gtnd.PurchaseTax = reader.GetDouble(18);
                    gtnd.SupplierID = reader.IsDBNull(19)? "NA" : reader.GetString(19);
                    gtnd.SupplierName = reader.IsDBNull(20)?"NA" : reader.GetString(20);
                    if (reader.IsDBNull(21))
                    {
                        gtnd.refNo = 0;
                    }
                    else
                    {
                        gtnd.refNo = reader.GetInt32(21);
                    }
                    gtnd.Value = reader.IsDBNull(22) ? 0 : reader.GetDouble(22);
                    getpassdetail.Add(gtnd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Gate Pass Details");
            }
            return getpassdetail;
        }
        
       
        public Boolean validategetpassheader(gatepassheader gtnh)
        {
            Boolean status = true;
            if(gtnh.FromOffice == gtnh.ToOffice)
            {
                MessageBox.Show("From Location and To Location Should not be same");
                return false;
            }
            if(gtnh.FromOffice == null)
            {
                return false;
            }
            if(gtnh.ToOffice == null)
            {
                return false;
            }
            return status;
        }

        //public Boolean forwardGTN(getpassheader gtnh)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "update getpassheader set DocumentStatus=" + (gtnh.DocumentStatus + 1) +
        //            ", ReceiveStatus = " + gtnh.ReceiveStatus +
        //            ", forwardUser='" + gtnh.ForwardUser + "'" +
        //            ", commentStatus='" + gtnh.CommentStatus + "'" +
        //            ", ForwarderList='" + gtnh.ForwarderList + "'" +
        //            " where DocumentID='" + gtnh.DocumentID + "'" +
        //            " and TemporaryNo=" + gtnh.TemporaryNo +
        //            " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("update", "getpassheader", "", updateSQL) +
        //        Main.QueryDelimiter;
        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        status = false;
        //    }
        //    return status;
        //}
        //public Boolean reverseGTN(getpassheader gtnh)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "update getpassheader set DocumentStatus=" + gtnh.DocumentStatus +
        //            ", forwardUser='" + gtnh.ForwardUser + "'" +
        //            ", commentStatus='" + gtnh.CommentStatus + "'" +
        //            ", ForwarderList='" + gtnh.ForwarderList + "'" +
        //             ", ReceiveStatus=" + gtnh.ReceiveStatus +
        //            " where DocumentID='" + gtnh.DocumentID + "'" +
        //            " and TemporaryNo=" + gtnh.TemporaryNo +
        //            " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("update", "getpassheader", "", updateSQL) +
        //        Main.QueryDelimiter;
        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        status = false;
        //    }
        //    return status;
        //}
        public Boolean ApproveGatePass(gatepassheader gtnh, List<gatepassdetail> gplist)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InventoryGatepassHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser ='" + Login.userLoggedIn + "'" +
                     ", ForwarderList ='" + gtnh.ForwarderList + "'" +
                    ", commentStatus='" + gtnh.CommentStatus + "'" +
                    ", GatePassNo =" + gtnh.GatePassNo +
                    ", GatePassDate =convert(date, getdate())" +
                    " where DocumentID='" + gtnh.DocumentID + "'" +
                    " and TemporaryNo=" + gtnh.TemporaryNo +
                    " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InventoryGatepassHeader", "", updateSQL) +
                Main.QueryDelimiter;
                foreach (gatepassdetail gtnd in gplist)
                {
                    double quant = gtnd.Quantity;
                    int RefNo = gtnd.refNo;
                    updateSQL = "update Stock set  " +
                        " PresentStock=" + "( (select PresentStock from Stock where RowID = " + RefNo + ")-" + quant + ")" +
                        " where RowID=" + RefNo;

                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
                        Main.QueryDelimiter;

                    updateSQL = "update Stock set  " +
                    "IssueQuantity=" + "( (select isnull(IssueQuantity,0) from Stock where RowID = " + RefNo + ")+" + quant + ")" +
                    " where RowID=" + RefNo;

                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
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
        public Boolean AcceptGatePass(gatepassheader gtnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InventoryGatepassHeader set " +
                    "AcceptedUser='" + Login.userLoggedIn + "'" +
                     ", AcceptanceDate=convert(date, getdate())" +
                      ", AcceptanceStatus= 1" +
                    ", commentStatus='" + gtnh.CommentStatus + "'" +
                    " where DocumentID='" + gtnh.DocumentID + "'" +
                    " and TemporaryNo=" + gtnh.TemporaryNo +
                    " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InventoryGatepassHeader", "", updateSQL) +
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
        public Boolean ReturnGatePass(gatepassheader gtnh, List<gatepassdetail> gplist)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InventoryGatepassHeader set ReturnStatus=" + (gtnh.ReturnStatus + 1) +
                    " where DocumentID='" + gtnh.DocumentID + "'" +
                    " and TemporaryNo=" + gtnh.TemporaryNo +
                    " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InventoryGatepassHeader", "", updateSQL) +
                Main.QueryDelimiter;

                foreach (gatepassdetail gtnd in gplist)
                {
                    updateSQL = "update InventoryGatepassDetail " +
                    "set QuantityReturning = " + gtnd.ReturningQuantity +
                    " where RowID = " + gtnd.RowID ;
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InventoryGatepassDetail", "", updateSQL) +
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
        public Boolean AcceptReturningQnatGatePass(gatepassheader gtnh, List<gatepassdetail> gplist)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InventoryGatepassHeader set ReturnStatus= 0 " +
                    " where DocumentID='" + gtnh.DocumentID + "'" +
                    " and TemporaryNo=" + gtnh.TemporaryNo +
                    " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InventoryGatepassHeader", "", updateSQL) +
                Main.QueryDelimiter;

                foreach (gatepassdetail gtnd in gplist)
                {
                    updateSQL = "update InventoryGatepassDetail " +
                    "set QuantityReturned = " + (gtnd.ReturningQuantity + gtnd.ReturnedQuantity) +
                    " , QuantityReturning = 0" + 
                    " where RowID = " + gtnd.RowID;
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InventoryGatepassDetail", "", updateSQL) +
                    Main.QueryDelimiter;

                    double quant = gtnd.ReturningQuantity;
                    int RefNo = gtnd.refNo;
                    updateSQL = "update Stock set  " +
                            " PresentStock=" + "( (select isnull(PresentStock,0) from Stock where RowID = " + RefNo + ")+" + quant + ")" +
                            " where RowID=" + RefNo;

                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
                        Main.QueryDelimiter;

                    updateSQL = "update Stock set  " +
                           " IssueQuantity=" + "( (select isnull(IssueQuantity,0) from Stock where RowID = " + RefNo + ")-" + quant + ")" +
                           " where RowID=" + RefNo;

                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
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
        //----
        public Boolean updateInventoryForApproveStockDetail(gatepassheader gtnh,List<gatepassdetail> getPDet)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                foreach (gatepassdetail gtnd in getPDet)
                {
                    double quant = gtnd.Quantity;
                    int RefNo = gtnd.refNo;
                    updateSQL = "update Stock set  " +
                        " PresentStock=" + "( (select PresentStock from Stock where RowID = " + RefNo + ")-" + quant + ")" +
                        " where RowID=" + RefNo;

                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
                        Main.QueryDelimiter;
                   
                }
                if (!UpdateTable.UT(utString))
                {
                    MessageBox.Show("failed to Update In Reference Number Wise getpassdetail in stock");
                    status = false;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        //public Boolean updateInventoryForAcceptStockDetail(gatepassheader gtnh, List<gatepassdetail> getPDet)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    string updateSQL = "";
        //    try
        //    {
        //        foreach (gatepassdetail gtnd in getPDet)
        //        {
        //            double quant = gtnd.ReturningQuantity;
        //            int RefNo = gtnd.refNo;
        //            updateSQL = "update Stock set  " +
        //                    " PresentStock=" + "( (select isnull(PresentStock,0) from Stock where RowID = " + RefNo + ")+" + quant + ")" +
        //                    " where RowID=" + RefNo;

        //            utString = utString + updateSQL + Main.QueryDelimiter;
        //            utString = utString +
        //            ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
        //                Main.QueryDelimiter;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = false;
        //    }
        //    return status;
        //}
        public static string getUserComments(string docid, int tempno, DateTime tempdate)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from InventoryGatepassHeader where DocumentID='" + docid + "'" +
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

        public Boolean updateInvGatePassHEaderAndDetail(gatepassheader gtnh, gatepassheader prevgtnh, List<gatepassdetail> getpassdetails)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InventoryGatepassHeader set FromOffice='" + gtnh.FromOffice +
                    "',ToOffice='" + gtnh.ToOffice +
                    "', CustomerID ='" + gtnh.CustomerID +
                     "', Remarks ='" + gtnh.Remarks +
                      "', SpecialNotes ='" + gtnh.SpecialNotes +
                    "', CommentStatus='" + gtnh.CommentStatus +
                    "', Comments='" + gtnh.Comments +
                    "', ForwarderList='" + gtnh.ForwarderList +
                    "' where DocumentID='" + gtnh.DocumentID + "'" +
                    " and TemporaryNo=" + gtnh.TemporaryNo +
                    " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InventoryGatepassHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from InventoryGatepassDetail where DocumentID='" + gtnh.DocumentID + "'" +
                     " and TemporaryNo=" + gtnh.TemporaryNo +
                     " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InventoryGatepassDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (gatepassdetail gtnd in getpassdetails)
                {
                    updateSQL = "insert into InventoryGatepassDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,Quantity,Value,QuantityReturning,QuantityReturned,MRNNo,MRNDate,BatchNo,ExpiryDate,SerialNo,PurchaseQuantity, " +
                   "PurchasePrice,PurchaseTax,SupplierID,StockReferenceNo) " +
                    "values ('" + gtnd.DocumentID + "'," +
                    gtnd.TemporaryNo + "," +
                    "'" + gtnd.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + gtnd.StockitemID + "'," +
                     "'" + gtnd.ModelNo + "'," +
                    gtnd.Quantity + "," + gtnd.Value + "," + gtnd.ReturningQuantity + "," + gtnd.ReturnedQuantity + "," + gtnd.MRNNo + "," +
                     "'" + gtnd.MRNDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + gtnd.BatchNo + "'," +
                   "'" + gtnd.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                   "'" + gtnd.SerialNo + "'," +
                   gtnd.PurchaseQuantity + "," +
                   gtnd.PurchasePrice + "," +
                   gtnd.PurchaseTax + "," +
                     "'" + gtnd.SupplierID + "'," +
                     +gtnd.refNo + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InventoryGatepassDetail", "", updateSQL) +
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
        public Boolean insertInvGatePassHEaderAndDetail(gatepassheader gtnh, List<gatepassdetail> getpassdetails)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                gtnh.TemporaryNo = DocumentNumberDB.getNumber(gtnh.DocumentID, 1);
                if (gtnh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + gtnh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + gtnh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into InventoryGatepassHeader " +
                     "(DocumentID,TemporaryNo,TemporaryDate,GatePassNo,GatePassDate,FromOffice,ToOffice,CustomerID,Remarks,SpecialNotes," +
                     "Comments,CommentStatus,AcceptanceDate,AcceptedUser,AcceptanceStatus," +
                     "ForwarderList,Status,DocumentStatus,CreateTime,CreateUser)" +
                     " values (" +
                     "'" + gtnh.DocumentID + "'," +
                     gtnh.TemporaryNo + "," +
                     "'" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     gtnh.GatePassNo + "," +
                     "'" + gtnh.GatePassDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + gtnh.FromOffice + "'," +
                     "'" + gtnh.ToOffice + "'," +
                     "'" + gtnh.CustomerID + "'," +
                        "'" + gtnh.Remarks + "'," +
                           "'" + gtnh.SpecialNotes + "'," +
                      "'" + gtnh.Comments + "'," +
                     "'" + gtnh.CommentStatus + "'," +
                     "'" + DateTime.Parse("1900-01-01").ToString("yyyy-MM-dd") + "'," +
                     "'" + gtnh.AcceptedUser + "'," +
                     +gtnh.AcceptanceStatus + "," +
                     "'" + gtnh.ForwarderList + "'," +
                     gtnh.Status + "," +
                     gtnh.DocumentStatus + "," +
                      "GETDATE()" + "," +
                     "'" + Login.userLoggedIn + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "InventoryGatepassHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from InventoryGatepassDetail where DocumentID='" + gtnh.DocumentID + "'" +
                    " and TemporaryNo=" + gtnh.TemporaryNo +
                    " and TemporaryDate='" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InventoryGatepassDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (gatepassdetail gtnd in getpassdetails)
                {
                    updateSQL = "insert into InventoryGatepassDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,Quantity,Value,QuantityReturning,QuantityReturned,MRNNo,MRNDate,BatchNo,ExpiryDate,SerialNo,PurchaseQuantity, " +
                   "PurchasePrice,PurchaseTax,SupplierID,StockReferenceNo) " +
                    "values ('" + gtnh.DocumentID + "'," +
                    gtnh.TemporaryNo + "," +
                    "'" + gtnh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + gtnd.StockitemID + "'," +
                     "'" + gtnd.ModelNo + "'," +
                    gtnd.Quantity + "," + gtnd.Value + "," + gtnd.ReturningQuantity + "," + gtnd.ReturnedQuantity + "," + gtnd.MRNNo + "," +
                     "'" + gtnd.MRNDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + gtnd.BatchNo + "'," +
                   "'" + gtnd.ExpiryDate.ToString("yyyy-MM-dd") + "'," +
                   "'" + gtnd.SerialNo + "'," +
                   gtnd.PurchaseQuantity + "," +
                   gtnd.PurchasePrice + "," +
                   gtnd.PurchaseTax + "," +
                     "'" + gtnd.SupplierID + "'," +
                     +gtnd.refNo + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InventoryGatepassDetail", "", updateSQL) +
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
