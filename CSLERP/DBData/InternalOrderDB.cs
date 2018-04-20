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
    class ioheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int InternalOrderNo { get; set; }
        public DateTime InternalOrderDate { get; set; }
        public string ReferenceTrackingNos { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime TargetDate { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public int ClosingStatus { get; set; }
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
        public string SEFID { get; set; }
        public Boolean isPlanPrepared { get; set; }
        public ioheader()
        {
            Comments = "";
        }
    }
    class iodetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string WorkDescription { get; set; }
        public string Specification { get; set; }
        public string OfficeID { set; get; }
        public string OfficeName { set; get; }
        public double Quantity { get; set; }
        public int WarrantyDays { get; set; }
    }
    public class iorequirements
    {
        public int RowID { get; set; }
        public int IOTemporaryNo { get; set; }
        public DateTime IOTemporaryDate { get; set; }
        public string SEFID { get; set; }
        public int SequenceNo { get; set; }
        public string Description { get; set; }
        public int SEFReferenceNo { get; set; }
        public string RequiredValue { get; set; }
        public string Actualvalue { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTIme { get; set; }
    }
    class InternalOrderDB
    {
        public List<ioheader> getFilteredIOHeader(string userList, int opt, string userCommentStatusString)
        {
            ioheader ioh;
            List<ioheader> IOHeaders = new List<ioheader>();
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
                string qrySUbDoc = "";
               string[] subDocList = SubDocumentReceiverDB.getReceiverWiseSubDocumnets(Login.empLoggedIn,"IOPRODUCT").ToArray();
               qrySUbDoc = string.Join("','", subDocList);


                //string s = userList.Replace("'", "");
                string query1 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " InternalOrderNO,InternalOrderDate,ReferenceTrackingNos,CustomerID,CustomerName," +
                    " TargetDate,Remarks, " +
                    " Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,ForwarderList,CommentStatus,SEFID,ClosingStatus " +
                    " from ViewInternalOrder" +
                    " where ((forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                     " or (CreateUser='" + Login.userLoggedIn + "' and DocumentStatus=1)" +
                     " or (commentStatus like '%" + userCommentStatusString + "%' and DocumentStatus between 1 and 98)) and Status not in (7,98) "+
                     " and (((DocumentID = 'IOPRODUCT') and SEFID in ('" + qrySUbDoc + "')) or ((DocumentID = 'IOSERVICE') and SEFID in (''))) order by TemporaryDate desc,TemporaryNo desc";

                string query2 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " InternalOrderNO,InternalOrderDate,ReferenceTrackingNos,CustomerID,CustomerName," +
                    " TargetDate,Remarks, " +
                    " Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,ForwarderList,CommentStatus,SEFID,ClosingStatus " +
                    " from ViewInternalOrder" +
                    " where ((CreateUser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98) " +
                     " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (commentStatus like '%" + acStr + "%' and DocumentStatus between 1 and 98) " +
                    ")  and Status not in (7,98) and (((DocumentID = 'IOPRODUCT') and SEFID in ('" + qrySUbDoc + "')) or ((DocumentID = 'IOSERVICE') and SEFID in (''))) order by TemporaryDate desc,TemporaryNo desc";

                string query3 = "select a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate, a.InternalOrderNO,a.InternalOrderDate,a.ReferenceTrackingNos," +
                    " a.CustomerID,a.CustomerName, a.TargetDate,a.Remarks,  a.Status,a.DocumentStatus,a.CreateTime,a.CreateUser,a.ForwardUser," +
                    " a.ApproveUser,a.CreatorName," +
                    " a.ForwarderName,a.ApproverName,a.ForwarderList,a.CommentStatus,a.SEFID,a.ClosingStatus,b.PPCount" +
                    " from ViewInternalOrder a left outer join (select DocumentID, InternalOrderNo,InternalOrderDate,COUNT(ProductionPlanNo) as PPCount from ViewIOVsProductionPlan group by " +
                    " DocumentID, InternalOrderNo,InternalOrderDate) b on " +
                    " a.InternalOrderNo = b.InternalOrderNo and a.InternalOrderDate = b.InternalOrderDate and a.DocumentID = b.DocumentID " +
                   " where ((a.CreateUser='" + Login.userLoggedIn + "'" +
                  " or a.ForwarderList like '%" + userList + "%'" +
                    " or a.commentStatus like '%" + acStr + "%'" +
                    " or a.approveUser='" + Login.userLoggedIn + "')" +
                    " and a.DocumentStatus = 99) and a.status = 1 and (((a.DocumentID = 'IOPRODUCT') and a.SEFID in ('" + qrySUbDoc + "')) or ((a.DocumentID = 'IOSERVICE') and a.SEFID in (''))) order by a.TemporaryDate desc,a.TemporaryNo desc";


                string query4 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " InternalOrderNO,InternalOrderDate,ReferenceTrackingNos,CustomerID,CustomerName," +
                    " TargetDate,Remarks, " +
                    " Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,ForwarderList,CommentStatus,SEFID,ClosingStatus " +
                   " from ViewInternalOrder" +
                   " where DocumentID = '" + userList + "' and InternalOrderNO > 0 and DocumentStatus = 99 and status = 1  and SEFID in ('" + qrySUbDoc + "') order by InternalOrderDate desc,InternalOrderNO desc ";
                string query6 = "select a.RowID, a.DocumentID, a.DocumentName,a.TemporaryNo,a.TemporaryDate, a.InternalOrderNO,a.InternalOrderDate,a.ReferenceTrackingNos," +
                    " a.CustomerID,a.CustomerName, a.TargetDate,a.Remarks,  a.Status,a.DocumentStatus,a.CreateTime,a.CreateUser,a.ForwardUser," +
                    " a.ApproveUser,a.CreatorName," +
                    " a.ForwarderName,a.ApproverName,a.ForwarderList,a.CommentStatus,a.SEFID,a.ClosingStatus,b.PPCount" +
                    " from ViewInternalOrder a left outer join (select DocumentID, InternalOrderNo,InternalOrderDate,COUNT(ProductionPlanNo) as PPCount from ViewIOVsProductionPlan group by "+
                    " DocumentID, InternalOrderNo,InternalOrderDate) b on " +
                    " a.InternalOrderNo = b.InternalOrderNo and a.InternalOrderDate = b.InternalOrderDate and a.DocumentID = b.DocumentID " +
                    " where  a.DocumentStatus = 99 and a.status = 1  and (((a.DocumentID = 'IOPRODUCT') and a.SEFID in ('" + qrySUbDoc + "')) or ((a.DocumentID = 'IOSERVICE') and a.SEFID in (''))) order by a.TemporaryDate desc,a.TemporaryNo desc";
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
                    ioh = new ioheader();
                    ioh.RowID = reader.GetInt32(0);
                    ioh.DocumentID = reader.GetString(1);
                    ioh.DocumentName = reader.GetString(2);
                    ioh.TemporaryNo = reader.GetInt32(3);
                    ioh.TemporaryDate = reader.GetDateTime(4);
                    ioh.InternalOrderNo = reader.GetInt32(5);
                    ioh.InternalOrderDate = reader.GetDateTime(6);
                    ioh.ReferenceTrackingNos = reader.GetString(7);
                    ioh.CustomerID = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    ioh.CustomerName = reader.IsDBNull(9) ? "" : reader.GetString(9);
                    //ioh.ReferenceTrackingNos = reader.GetString(10);
                    ioh.TargetDate = reader.GetDateTime(10);
                    ioh.Remarks = reader.GetString(11);
                    ioh.Status = reader.GetInt32(12);
                    ioh.DocumentStatus = reader.GetInt32(13);
                    ioh.CreateTime = reader.GetDateTime(14);
                    ioh.CreateUser = reader.GetString(15);
                    ioh.ForwardUser = reader.GetString(16);
                    ioh.ApproveUser = reader.GetString(17);
                    ioh.CreatorName = reader.GetString(18);
                    ioh.ForwarderName = reader.GetString(19);
                    ioh.ApproverName = reader.GetString(20);
                    if (!reader.IsDBNull(21))
                    {
                        ioh.ForwarderList = reader.GetString(21);
                    }
                    else
                    {
                        ioh.ForwarderList = "";
                    }
                    if (!reader.IsDBNull(22))
                    {
                        ioh.CommentStatus = reader.GetString(22);
                    }
                    else
                    {
                        ioh.CommentStatus = "";
                    }
                    ioh.SEFID = reader.IsDBNull(23)?"":reader.GetString(23);
                    ioh.ClosingStatus = reader.IsDBNull(24) ? 0 : reader.GetInt32(24);
                    if(opt == 6 || opt == 3)
                    {
                        if ((reader.IsDBNull(25) ? 0 : reader.GetInt32(25)) == 0)
                            ioh.isPlanPrepared = false;
                        else
                            ioh.isPlanPrepared = true;
                    }
                    IOHeaders.Add(ioh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying IO Header Details");
            }
            return IOHeaders;
        }

        public static List<iodetail> getIODetail(ioheader ioh)
        {
            iodetail iod;
            List<iodetail> IODetail = new List<iodetail>();
            try
            {
                //string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,DocumentID,TemporaryNo, TemporaryDate,StockItemID,StockItemName,ModelNo, ModelName,WorkDescription, Specification,OfficeID,  " +
                   "Quantity,WarrantyDays,OfficeName " +
                   "from ViewInternalOrderDetail " +
                   " where DocumentID='" + ioh.DocumentID + "'" +
                   " and TemporaryNo=" + ioh.TemporaryNo +
                   " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                   " order by StockItemID";
                //string query2 =
                //   "select a.RowID,a.DocumentID,a.TemporaryNo, a.TemporaryDate,a.StockItemID,  " +
                //   "isnull(b.Description, ' ') Name, a.WorkDescription, ISNULL(a.Specification, ' ') Specification,a.OfficeID, " +
                //   "a.Quantity, a.WarrantyDays ,o.Name " +
                //   "from Office o, " +
                //   "InternalOrderDetail as a left join " +
                //   "ViewServiceLookup as b on a.StockItemID = b.ServiceID " +
                //   " where  a.OfficeID = o.OfficeID and a.DocumentID='" + ioh.DocumentID + "'" +
                //   " and a.TemporaryNo=" + ioh.TemporaryNo +
                //   " and a.TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'" +
                //   " order by a. StockItemID";


                //if (ioh.DocumentID == "IOPRODUCT")
                //{
                //    query = query1;
                //}
                //else if (ioh.DocumentID == "IOSERVICE")
                //{
                //    query = query2;
                //}

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    iod = new iodetail();
                    iod.RowID = reader.GetInt32(0);
                    iod.DocumentID = reader.GetString(1);
                    iod.TemporaryNo = reader.GetInt32(2);
                    iod.TemporaryDate = reader.GetDateTime(3).Date;
                    iod.StockItemID = reader.GetString(4);
                    iod.StockItemName = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                    {
                        iod.ModelNo = reader.GetString(6);
                    }
                    else
                    {
                        iod.ModelNo = "NA";
                    }

                    if (!reader.IsDBNull(7))
                    {
                        iod.ModelName = reader.GetString(7);
                    }
                    else
                    {
                        iod.ModelName = "NA";
                    }

                    iod.WorkDescription = reader.GetString(8);
                    iod.Specification = reader.GetString(9);
                    iod.OfficeID = reader.GetString(10);

                    iod.Quantity = reader.GetDouble(11);
                    iod.WarrantyDays = reader.GetInt32(12);
                    if (!reader.IsDBNull(13))
                    {
                        iod.OfficeName = reader.GetString(13);
                    }
                    else
                    {
                        iod.OfficeName = "";
                    }

                    IODetail.Add(iod);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Internal Order Details");
            }
            return IODetail;
        }
        public Boolean ValidateIOHeader(ioheader ioh)
        {
            Boolean status = true;
            try
            {
                if (ioh.DocumentID.Trim().Length == 0 || ioh.DocumentID == null)
                {
                    return false;
                }

                ////if (ioh.CustomerID.Trim().Length == 0 || ioh.CustomerID == null)
                ////{
                ////    return false;
                ////}
                //if (ioh.InternalOrderNo == 0)
                //{
                //    return false;
                //}
                if (ioh.InternalOrderDate == null)
                {
                    return false;
                }
                if (ioh.ReferenceTrackingNos.Trim().Length == 0 || ioh.ReferenceTrackingNos == null)
                {
                    return false;
                }
                if (ioh.DocumentID == "IOPRODUCT")
                {
                    if (ioh.SEFID.Trim().Length == 0 || ioh.SEFID == null)
                    {
                        return false;
                    }
                }
                if (ioh.TargetDate.Date < DateTime.Now.Date || ioh.TargetDate == null)
                {
                    return false;
                }
                if (ioh.Remarks.Trim().Length == 0 || ioh.Remarks == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public Boolean forwardIOHeader(ioheader ioh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InternalOrderHeader set DocumentStatus=" + (ioh.DocumentStatus + 1) +
                    ", forwardUser='" + ioh.ForwardUser + "'" +
                     ", commentStatus='" + ioh.CommentStatus + "'" +
                    ", ForwarderList='" + ioh.ForwarderList + "'" +
                    " where DocumentID='" + ioh.DocumentID + "'" +
                    " and TemporaryNo=" + ioh.TemporaryNo +
                    " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InternalOrderHeader", "", updateSQL) +
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
        public Boolean reverseIO(ioheader ioh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InternalOrderHeader set DocumentStatus=" + ioh.DocumentStatus +
                    ", forwardUser='" + ioh.ForwardUser + "'" +
                    ", commentStatus='" + ioh.CommentStatus + "'" +
                    ", ForwarderList='" + ioh.ForwarderList + "'" +
                    " where DocumentID='" + ioh.DocumentID + "'" +
                    " and TemporaryNo=" + ioh.TemporaryNo +
                    " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InternalOrderHeader", "", updateSQL) +
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
        public Boolean ApproveIOHeader(ioheader ioh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InternalOrderHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + ioh.CommentStatus + "'" +
                    ", InternalOrderNo=" + ioh.InternalOrderNo +
                    ", InternalOrderDate=convert(date, getdate())" +
                    " where DocumentID='" + ioh.DocumentID + "'" +
                    " and TemporaryNo=" + ioh.TemporaryNo +
                    " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InternalOrderHeader", "", updateSQL) +
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

        public List<ioheader> getApprovedIOForSelection(string DocID)
        {
            List<ioheader> ioList = new List<ioheader>();
            ioheader ioh = new ioheader();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                      " InternalOrderNO,InternalOrderDate,ReferenceTrackingNos,CustomerID,CustomerName," +
                      " TargetDate,Remarks " +
                      " from ViewInternalOrder" +
                      " where  DocumentStatus = 99 and status = 1 and DocumentID = '" + DocID +
                      "' order by InternalOrderDate desc,DocumentID asc,InternalOrderNO desc ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ioh = new ioheader();
                    ioh.RowID = reader.GetInt32(0);
                    ioh.DocumentID = reader.GetString(1);
                    ioh.DocumentName = reader.GetString(2);
                    ioh.TemporaryNo = reader.GetInt32(3);
                    ioh.TemporaryDate = reader.GetDateTime(4);
                    ioh.InternalOrderNo = reader.GetInt32(5);
                    ioh.InternalOrderDate = reader.GetDateTime(6);
                    ioh.ReferenceTrackingNos = reader.GetString(7);
                    ioh.CustomerID = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    ioh.CustomerName = reader.IsDBNull(9) ? "" : reader.GetString(9);
                    ioh.TargetDate = reader.GetDateTime(10);
                    ioh.Remarks = reader.GetString(11);
                    ioList.Add(ioh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return ioList;
        }
        public List<ioheader> getSEFIDWiseApprovedIOForSelection(string DocID, List<string> subDocListStr)
        {
            List<ioheader> ioList = new List<ioheader>();
            ioheader ioh = new ioheader();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string subDocStr = string.Join("','", subDocListStr.ToArray());
                string query = "select  DocumentID," +
                      " InternalOrderNO,InternalOrderDate,ReferenceTrackingNos,Name," +
                      " TargetDate,SEFID,ProductionPlanNo " +
                      " from ViewIOVsProductionPlan" +
                      " where  DocumentStatus = 99 and status = 1 and DocumentID = '" + DocID +
                      "' and SEFID in ('" + subDocStr + "')";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ioh = new ioheader();
                    ioh.DocumentID = reader.GetString(0);
                    ioh.InternalOrderNo = reader.GetInt32(1);
                    ioh.InternalOrderDate = reader.GetDateTime(2);
                    ioh.ReferenceTrackingNos = reader.GetString(3);
                    ioh.CustomerName = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    ioh.TargetDate = reader.GetDateTime(5);
                    ioh.SEFID = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    if (reader.IsDBNull(7))
                    {
                        ioh.isPlanPrepared = false;
                    }
                    else
                    {
                        ioh.isPlanPrepared = true;
                    }
                    ioList.Add(ioh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return ioList;
        }
        public static ListView ReferenceIOSelectionView(string DocumentID)
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
                InternalOrderDB iodb = new InternalOrderDB();
                List<ioheader> IOHeadersTemp = new List<ioheader>();
                List<ioheader> IOHeaders = new List<ioheader>();
                IOHeadersTemp = iodb.getApprovedIOForSelection(DocumentID);
                IOHeaders = IOHeadersTemp.OrderBy(ioh => ioh.InternalOrderDate).ToList();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Doc Id", -2, HorizontalAlignment.Left);
                lv.Columns.Add("IO No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("IO Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Ref.TNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Cust Name", -2, HorizontalAlignment.Center);
                lv.Columns.Add("T Date", -2, HorizontalAlignment.Center);
               
                //lv.Columns.Add("Tax Amt", -2, HorizontalAlignment.Center);

                foreach (ioheader ioh in IOHeaders)
                {

                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(ioh.DocumentID);
                    item1.SubItems.Add(ioh.InternalOrderNo.ToString());
                    item1.SubItems.Add(ioh.InternalOrderDate.ToShortDateString());
                    item1.SubItems.Add(ioh.ReferenceTrackingNos);
                    item1.SubItems.Add(ioh.CustomerName);
                    item1.SubItems.Add(ioh.TargetDate.ToShortDateString());
                    
                    //item1.SubItems.Add(ioh.TaxAmount.ToString());
                    lv.Items.Add(item1);
                    ////index++;
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static ListView SEFIDWiseIOSelectionView(string DocumentID)
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
                //lv.OwnerDraw = true;
                InternalOrderDB iodb = new InternalOrderDB();
                List<ioheader> IOHeaders = new List<ioheader>();
                List<string> subDocLIst = SubDocumentReceiverDB.getReceiverWiseSubDocumnets(Login.empLoggedIn, DocumentID);
                IOHeaders = iodb.getSEFIDWiseApprovedIOForSelection(DocumentID, subDocLIst).OrderBy(ioh => ioh.InternalOrderDate).ToList();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Doc Id", -2, HorizontalAlignment.Left);
                lv.Columns.Add("IO No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("IO Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Ref.TNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Cust Name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("T Date", -2, HorizontalAlignment.Center);
                lv.Columns.Add("SEFID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Plan Prepared", -2, HorizontalAlignment.Center);
                lv.Columns[4].Width = 220;
                lv.Columns[5].Width = 220;
                lv.Columns[8].Width = 0;
                //lv.Columns.Add("Tax Amt", -2, HorizontalAlignment.Center);

                foreach (ioheader ioh in IOHeaders)
                {

                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(ioh.DocumentID);
                    item1.SubItems.Add(ioh.InternalOrderNo.ToString());
                    item1.SubItems.Add(ioh.InternalOrderDate.ToString("yyyy-MM-dd"));
                    item1.SubItems.Add(ioh.ReferenceTrackingNos);
                    item1.SubItems.Add(ioh.CustomerName);
                    item1.SubItems.Add(ioh.TargetDate.ToShortDateString());
                    item1.SubItems.Add(ioh.SEFID.ToString());
                    item1.SubItems.Add(ioh.isPlanPrepared.ToString());
                    if(ioh.isPlanPrepared)
                        item1.BackColor = System.Drawing.Color.Magenta;
                    lv.Items.Add(item1);
                    ////index++;
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }

        public static string GetIONumbers(string trackingDetails, string documentID)
        {
            //trackingdetails=2(dd-MM-yyyy)
            string IODetails = "";

            try
            {
                string query = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                       " InternalOrderNO,InternalOrderDate,ReferenceTrackingNos,CustomerID,CustomerName " +
                      " from ViewInternalOrder" +
                      " where DocumentStatus = 99 and Status=1 and DocumentID = '" + 
                      documentID + "' and ReferenceTrackingNos like '%" + trackingDetails + "%'";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    IODetails = IODetails + reader.GetInt32(5) + " : " + reader.GetDateTime(6).ToString("dd-MM-yyyy") + " ; ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetIONumbers() : Erroer");
            }
            return IODetails;
        }
        public static string getUserComments(string docid, int tempno, DateTime tempdate)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from InternalOrderHeader where DocumentID='" + docid + "'" +
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

        public static List<iodetail> IODetailList(int ion, DateTime IODate)
        {
            iodetail iod;
            List<iodetail> IODetail = new List<iodetail>();
            ioheader ioh = new ioheader();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,DocumentID,StockItemID,StockItemName,ModelNo, ModelName," +
                "Quantity " +
                " from ViewInternalOrderDetail " +
                " where DocumentID = 'IOPRODUCT'" +
                " and InternalOrderNo=" + ion +
                " and InternalOrderDate='" + IODate.ToString("yyyy-MM-dd") + "'" +
                " order by StockItemName ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    iod = new iodetail();
                    iod.RowID = reader.GetInt32(0);
                    iod.DocumentID = reader.GetString(1);
                    iod.TemporaryNo = ion;  //Intenal order no
                    iod.TemporaryDate = IODate; // internal order date
                    iod.StockItemID = reader.GetString(2);
                    iod.StockItemName = reader.GetString(3);
                    iod.ModelNo = reader.IsDBNull(4) ? "NA" : reader.GetString(4);
                    iod.ModelName = reader.IsDBNull(5) ? "NA" : reader.GetString(5);
                    iod.Quantity = reader.GetDouble(6);
                    IODetail.Add(iod);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return IODetail;
        }
        public static string getApproverName(int ion, DateTime IODate)
        {
            string approverName = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select ApproverName from ViewInternalOrder where DocumentID='IOPRODUCT'" +
                        " and InternalOrderNo=" + ion +
                        " and InternalOrderDate='" + IODate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    approverName = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return approverName;
        }
        public static Double getQuantityOfIO(int IONo, DateTime IODate, string StockID)
        {
            Double Quant = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select Quantity from InternalOrderDetail where DocumentID='IOPRODUCT' and " +
                        " TemporaryNo= (select TemporaryNo from InternalOrderHeader where InternalOrderNo = " + IONo +
                            " and InternalOrderDate='" + IODate.ToString("yyyy-MM-dd") + "' and DocumentID='IOPRODUCT' and StockItemID = '"+ StockID + "')"+
                        " and TemporaryDate= (select TemporaryDate from InternalOrderHeader where InternalOrderNo = " + IONo +
                            " and InternalOrderDate='" + IODate.ToString("yyyy-MM-dd") + "' and DocumentID='IOPRODUCT' and StockItemID = '" + StockID + "')";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Quant = reader.GetDouble(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in retriving Quantity of selected Item");
            }
            return Quant;
        }
        public Boolean updateIOHeaderAndDetail(ioheader ioh, ioheader previoh, List<iodetail> IODetails, List<iorequirements> ioreqList)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InternalOrderHeader set " +
                      "ReferenceTrackingNos='" + ioh.ReferenceTrackingNos +
                     "', CustomerID = '" + ioh.CustomerID +
                     "', TargetDate = '" + ioh.TargetDate.ToString("yyyy-MM-dd") +
                     "', Remarks = '" + ioh.Remarks +
                      "', SEFID = '" + ioh.SEFID +
                     "', Comments='" + ioh.Comments +
                     "', ForwarderList='" + ioh.ForwarderList +
                      "', CommentStatus='" + ioh.CommentStatus +
                     "' where DocumentID='" + ioh.DocumentID + "'" +
                     " and TemporaryNo=" + ioh.TemporaryNo +
                     " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InternalOrderHeader", "", updateSQL) +
                Main.QueryDelimiter;

                //Query for updating IORequrements
                if(ioreqList.Count != 0)
                {
                    updateSQL = "Delete from IORequirement where " +
                     "IOTemporaryNo=" + ioh.TemporaryNo +
                     " and IOTemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                        ActivityLogDB.PrepareActivityLogQquerString("delete", "IORequirement", "", updateSQL) +
                        Main.QueryDelimiter;
                    foreach (iorequirements ioreq in ioreqList)
                    {
                        updateSQL = "insert into IORequirement " +
                        "(IOTemporaryNo,IOTemporaryDate,SEFID,SEFReferenceNo,RequiredValue,CreateUser,CreateTime) " +
                        "values (" +
                        ioh.TemporaryNo + "," +
                        "'" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                        "'" + ioreq.SEFID + "'," +
                         + ioreq.SEFReferenceNo + "," +
                        "'" + ioreq.RequiredValue + "'," +
                        "'" + Login.userLoggedIn + "',GETDATE())";

                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                        ActivityLogDB.PrepareActivityLogQquerString("insert", "InternalOrderDetail", "", updateSQL) +
                        Main.QueryDelimiter;
                    }
                }

                updateSQL = "Delete from InternalOrderDetail where DocumentID='" + ioh.DocumentID + "'" +
                      " and TemporaryNo=" + ioh.TemporaryNo +
                      " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InternalOrderDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (iodetail iod in IODetails)
                {
                    updateSQL = "insert into InternalOrderDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,WorkDescription,Specification,OfficeID,Quantity,WarrantyDays) " +
                    "values ('" + ioh.DocumentID + "'," +
                    ioh.TemporaryNo + "," +
                    "'" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + iod.StockItemID + "'," +
                     "'" + iod.ModelNo + "'," +
                    "'" + iod.WorkDescription + "'," +
                    "'" + iod.Specification + "'," +
                    "'" + iod.OfficeID + "'," +
                    iod.Quantity + "," +
                    iod.WarrantyDays + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InternalOrderDetail", "", updateSQL) +
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
        public Boolean InsertIOHeaderAndDetail(ioheader ioh, List<iodetail> IODetails, List<iorequirements> ioreqList)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                ioh.TemporaryNo = DocumentNumberDB.getNumber(ioh.DocumentID, 1);
                if (ioh.TemporaryNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + ioh.TemporaryNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + ioh.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into InternalOrderHeader " +
                     "(DocumentID,TemporaryNo,TemporaryDate,InternalOrderNo,InternalOrderDate,ReferenceTrackingNos,CustomerID," +
                     "TargetDate,Remarks,SEFID,Status,DocumentStatus,AcceptanceStatus,CreateUser,CreateTime,Comments,ForwarderList,CommentStatus)" +
                     " values (" +
                     "'" + ioh.DocumentID + "'," +
                     ioh.TemporaryNo + "," +
                     "'" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     ioh.InternalOrderNo + "," +
                     "'" + ioh.InternalOrderDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + ioh.ReferenceTrackingNos + "'," +
                     "'" + ioh.CustomerID + "'," +

                     "'" + ioh.TargetDate.ToString("yyyy-MM-dd") + "'," +
                     "'" + ioh.Remarks + "'," +
                     "'" + ioh.SEFID + "'," +
                     ioh.Status + "," +
                     ioh.DocumentStatus + "," +
                     ioh.AcceptanceStatus + "," +
                     "'" + Login.userLoggedIn + "'," +
                      "GETDATE()" +
                      ",'" + ioh.Comments + "'," +
                      "'" + ioh.ForwarderList + "'," +
                      "'" + ioh.CommentStatus + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "InternalOrderHeader", "", updateSQL) +
                Main.QueryDelimiter;

                //Query for updating IORequrements
                if (ioreqList.Count != 0)
                {
                    updateSQL = "Delete from IORequirement where " +
                     "IOTemporaryNo=" + ioh.TemporaryNo +
                     " and IOTemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                        ActivityLogDB.PrepareActivityLogQquerString("delete", "IORequirement", "", updateSQL) +
                        Main.QueryDelimiter;
                    foreach (iorequirements ioreq in ioreqList)
                    {
                        updateSQL = "insert into IORequirement " +
                        "(IOTemporaryNo,IOTemporaryDate,SEFID,SEFReferenceNo,RequiredValue,CreateUser,CreateTime) " +
                        "values (" +
                        ioh.TemporaryNo + "," +
                        "'" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                        "'" + ioreq.SEFID + "'," +
                         +ioreq.SEFReferenceNo + "," +
                        "'" + ioreq.RequiredValue + "'," +
                        "'" + Login.userLoggedIn + "',GETDATE())";

                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                        ActivityLogDB.PrepareActivityLogQquerString("insert", "InternalOrderDetail", "", updateSQL) +
                        Main.QueryDelimiter;
                    }
                }

                updateSQL = "Delete from InternalOrderDetail where DocumentID='" + ioh.DocumentID + "'" +
                     " and TemporaryNo=" + ioh.TemporaryNo +
                     " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "InternalOrderDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (iodetail iod in IODetails)
                {
                    updateSQL = "insert into InternalOrderDetail " +
                    "(DocumentID,TemporaryNo,TemporaryDate,StockItemID,ModelNo,WorkDescription,Specification,OfficeID,Quantity,WarrantyDays) " +
                    "values ('" + ioh.DocumentID + "'," +
                    ioh.TemporaryNo + "," +
                    "'" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + iod.StockItemID + "'," +
                     "'" + iod.ModelNo + "'," +
                    "'" + iod.WorkDescription + "'," +
                    "'" + iod.Specification + "'," +
                    "'" + iod.OfficeID + "'," +
                    iod.Quantity + "," +
                    iod.WarrantyDays + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "InternalOrderDetail", "", updateSQL) +
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

        //IORequirements....
        public static Boolean deleteSpecificationOfIO(int tempNo, DateTime tempDate)
        {
            Boolean status = true;
            int count = 0;
            int count1 = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "Delete from IORequirement where " +
                    "IOTemporaryNo=" + tempNo +
                    " and IOTemporaryDate='" + tempDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in retriving Quantity of selected Item");
                status = false;
            }
            return status;
        }
        public List<iorequirements> getTemplateListForProductType(string sefid)
        {
            List<iorequirements> ioreqList = new List<iorequirements>();
            iorequirements ioreq = new iorequirements();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,SEFID,SequenceNo,Description from SEFCheckList " +
                   " where SEFID = '" + sefid + "' and Status = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ioreq = new iorequirements();
                    ioreq.SEFReferenceNo = reader.GetInt32(0); //Row ID of Template
                    ioreq.SEFID = reader.GetString(1);
                    ioreq.SequenceNo = reader.GetInt32(2); 
                    ioreq.Description = reader.GetString(3);

                    ioreqList.Add(ioreq);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in retriving template details");
            }
            return ioreqList;
        }
        public List<iorequirements> GetTemplatesForPerticularIO(int tempno, DateTime tempDate)
        {
            List<iorequirements> ioreqList = new List<iorequirements>();
            iorequirements ioreq = new iorequirements();
            try
            {
                string query = "select a.RowID,a.IOTemporaryNo,a.IOtemporaryDate,a.SEFID,a.SEFReferenceNo,b.Description,a.RequiredValue,a.ActualValue,b.SequenceNo " +
                    "from IORequirement a , SEFCheckList b where a.SEFReferenceNo = b.RowID and a.IOTemporaryNo = " + tempno+
                    " and a.IOtemporaryDate = '" + tempDate.ToString("yyyy-MM-dd") + "' and b.Status = 1";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ioreq = new iorequirements();
                    ioreq.RowID = reader.GetInt32(0);
                    ioreq.IOTemporaryNo = reader.GetInt32(1);
                    ioreq.IOTemporaryDate = reader.GetDateTime(2);
                    ioreq.SEFID = reader.GetString(3);
                    ioreq.SEFReferenceNo = reader.GetInt32(4);
                    ioreq.Description = reader.GetString(5);
                    ioreq.RequiredValue = reader.GetString(6);
                    ioreq.Actualvalue = reader.IsDBNull(7)?"":reader.GetString(7);
                    ioreq.SequenceNo = reader.GetInt32(8);
                    ioreqList.Add(ioreq);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("GetTemplatesForPerticularIO() : Erroer");
            }
            return ioreqList;
        }
        //get IoDetail on the basis of ReferencePOnos
        public static List<ioheader> IODetailListWRTRefPONos(string refPONos)
        {
            List<ioheader> ioList = new List<ioheader>();
            ioheader ioh = new ioheader();

            //string[] strRef = refPONos.Trim().Split(';');
            //string DocIDStr = strRef[0];
            //int trackNo1 = Convert.ToInt32(strRef[1].Substring(0, strRef[1].IndexOf('(')));
            //int findex = strRef[1].IndexOf('(');
            //int sindex = strRef[1].IndexOf(')');
            //string tstr = strRef[1].Substring(findex + 1, (sindex - findex) - 1);
            //DateTime trackDate1 = Convert.ToDateTime(tstr);

            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.DocumentID,a.TemporaryNo,a.TemporaryDate,a.ReferenceTrackingNos,a.CustomerID,b.Name from InternalOrderHeader a , Customer b " +
                        " where a.CustomerID = b.CustomerID and ReferenceTrackingNos like '%" + refPONos + "%'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ioh = new ioheader();
                    ioh.DocumentID = reader.GetString(0);
                    ioh.TemporaryNo = reader.GetInt32(1);
                    ioh.TemporaryDate = reader.GetDateTime(2);
                    ioh.ReferenceTrackingNos = reader.IsDBNull(3)? "" : reader.GetString(3);
                    ioh.CustomerID = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    ioh.CustomerName = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    ioList.Add(ioh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return ioList;
        }

        public static List<iodetail> getIODetailForIndent(int ioNo, DateTime ioDate)
        {
            iodetail iod;
            List<iodetail> IODetail = new List<iodetail>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID,StockItemID,StockItemName,WorkDescription, Specification," +
                   "Quantity,ApproverName,WarrantyDays " +
                   "from ViewInternalOrderDetail " +
                   " where DocumentID='IOPRODUCT'" +
                   " and InternalOrderNo=" + ioNo + //For Internal OrderNo
                   " and InternalOrderDate='" + ioDate.ToString("yyyy-MM-dd") + "'" + //For Internal Order Date
                   " order by StockItemID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    iod = new iodetail();
                    iod.DocumentID = reader.GetString(0);
                    iod.StockItemID = reader.GetString(1);
                    iod.StockItemName = reader.GetString(2);
                    iod.WorkDescription = reader.GetString(3);
                    iod.Specification = reader.GetString(4);
                    iod.Quantity = reader.GetDouble(5);
                    iod.OfficeName = reader.IsDBNull(6)?"":reader.GetString(6); // For Approver name
                    iod.WarrantyDays = reader.IsDBNull(7) ? 0: reader.GetInt32(7);
                    IODetail.Add(iod);
                }
                conn.Close();
            }
            catch (Exception xe)
            {
                MessageBox.Show("Error querying Internal Order Details");
            }
            return IODetail;
        }

        public List<ioheader> getFilteredIOForReportIO()
        {
            ioheader ioh;
            List<ioheader> IOHeaders = new List<ioheader>();
            try
            {

                string query = "select a.InternalOrderNO,a.InternalOrderDate,a.SEFID ,a.ReferenceTrackingNos,a.CustomerID,b.Name," +
                    " a.TargetDate,a.Remarks,a.TemporaryNo,a.TemporaryDate "+
                   " from InternalOrderHeader a left outer join Customer b on a.CustomerID = b.CustomerID where " +
                   " a.DocumentID = 'IOPRODUCT' and a.SEFID in ('M2M','ACTIVE') and a.DocumentStatus = 99 and a.Status = 1 order by a.InternalOrderDate desc,a.InternalOrderNO desc ";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ioh = new ioheader();
                    ioh.InternalOrderNo = reader.GetInt32(0);
                    ioh.InternalOrderDate = reader.GetDateTime(1);
                    ioh.SEFID = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    ioh.ReferenceTrackingNos = reader.GetString(3);
                    ioh.CustomerID = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    ioh.CustomerName = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    ioh.TargetDate = reader.GetDateTime(6);
                    ioh.Remarks = reader.GetString(7);
                    ioh.TemporaryNo = reader.GetInt32(8);
                    ioh.TemporaryDate = reader.GetDateTime(9);
                    IOHeaders.Add(ioh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying IO Header Details");
            }
            return IOHeaders;
        }
        //Internal Order List In GridViewNew
        public DataGridView getGridViewOfIODetail(ioheader ioh)
        {
            DataGridView grdIODetail = new DataGridView();
            try
            {
                string[] strColArr = { "StockItemID", "StockItemName", "Quantity", "WorkDesc","Specification","WarrantyDays" };
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                };

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdIODetail.EnableHeadersVisualStyles = false;
                grdIODetail.AllowUserToAddRows = false;
                grdIODetail.AllowUserToDeleteRows = false;
                grdIODetail.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdIODetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdIODetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdIODetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdIODetail.ColumnHeadersHeight = 27;
                grdIODetail.RowHeadersVisible = false;
                grdIODetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str;
                    colArr[index].HeaderText = str;
                    colArr[index].ReadOnly = true;
                    if (index == 1)
                        colArr[index].Width = 300;
                    else if(index == 4)
                        colArr[index].Width = 300;
                    else if (index == 0)
                        colArr[index].Width = 140;
                    else
                        colArr[index].Width = 100;


                    //if (index == 2)
                    //    colArr[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    grdIODetail.Columns.Add(colArr[index]);
                }

                List< iodetail > iioList = InternalOrderDB.getIODetail(ioh);
                foreach (iodetail iod in iioList)
                {
                    grdIODetail.Rows.Add();
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells[strColArr[0]].Value = iod.StockItemID;
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells[strColArr[1]].Value = iod.StockItemName;
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells[strColArr[2]].Value = iod.Quantity;
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells[strColArr[3]].Value = iod.WorkDescription;
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells[strColArr[4]].Value = iod.Specification;
                    grdIODetail.Rows[grdIODetail.Rows.Count - 1].Cells[strColArr[5]].Value = iod.WarrantyDays;
                }
            }
            catch (Exception ex)
            {
            }

            return grdIODetail;
        }

        //CLosing Internal Order
        public static Boolean RequestTOCloseIOHeader(ioheader ioh,string comments)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InternalOrderHeader set ClosingStatus=1 " +
                     ", Comments='" + comments +
                    "' where DocumentID='" + ioh.DocumentID + "'" +
                    " and TemporaryNo=" + ioh.TemporaryNo +
                    " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InternalOrderHeader", "", updateSQL) +
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
        public static Boolean CloseIOHeader(ioheader ioh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InternalOrderHeader set ClosingStatus=2 , Status = 7 " +
                    " where DocumentID='" + ioh.DocumentID + "'" +
                    " and TemporaryNo=" + ioh.TemporaryNo +
                    " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InternalOrderHeader", "", updateSQL) +
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
        public static Boolean RejectClosingRequest(ioheader ioh,string comments)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update InternalOrderHeader set ClosingStatus= 0 " +
                     ", Comments= '" + comments +
                    "' where DocumentID='" + ioh.DocumentID + "'" +
                    " and TemporaryNo=" + ioh.TemporaryNo +
                    " and TemporaryDate='" + ioh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "InternalOrderHeader", "", updateSQL) +
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
        public int getTemplateListForProductCount(string sefid)
        {
            int tcount = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select count(*) from SEFCheckList " +
                   " where SEFID = '" + sefid + "' and Status = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    tcount = reader.GetInt32(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in retriving template details");
            }
            return tcount;
        }
        //get Reference tracking no string
        public static string getReferenceTrackingDetStr(string docid, int iono, DateTime iodate)
        {
            string refStr = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select ReferenceTrackingNos from InternalOrderHeader where DocumentID='" + docid + "'" +
                        " and InternalOrderNo=" + iono +
                        " and InternalOrderDate='" + iodate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    refStr = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return refStr;
        }
    }
}
