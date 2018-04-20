using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class WorkOrderStatusDB
    {
        public List<workorderheader> getFilteredWorkOrderStatus()
        {
            workorderheader woh;
            List<workorderheader> WOHeaders = new List<workorderheader>();
            try
            {
                string query = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                   " WORequestNo,WORequestDate,ReferenceInternalOrder,ProjectID,OfficeID,CustomerID,CustomerName,CurrencyID,CurrencyName,StartDate,TargetDate,PaymentTerms,PaymentMode," +
                   " POAddress,ServiceValue,TaxAmount,TotalAmount,TermsAndCondition,Remarks, " +
                   " Status,DocumentStatus,CreateTime,CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,"+
                   "ApproverName,CommentStatus,ForwarderList,WorkOrderStatus,WONo, WODate  " +
                   " from ViewWorkOrder" +
                   " where status = 1 and DocumentStatus = 99 order by WORequestDate desc,DocumentID asc,WORequestNo desc";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    woh = new workorderheader();
                    woh.RowID = reader.GetInt32(0);
                    woh.DocumentID = reader.GetString(1);
                    woh.DocumentName = reader.GetString(2);
                    woh.TemporaryNo = reader.GetInt32(3);
                    woh.TemporaryDate = reader.GetDateTime(4);
                    woh.WORequestNo = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                    {
                        woh.WORequestDate = reader.GetDateTime(6);
                    }
                    if (!reader.IsDBNull(6))
                    {
                        woh.ReferenceInternalOrder = reader.GetString(7);
                    }
                    woh.ProjectID = reader.GetString(8);
                    woh.OfficeID = reader.GetString(9);
                    woh.CustomerID = reader.GetString(10);
                    woh.CustomerName = reader.GetString(11);
                    woh.CurrencyID = reader.GetString(12);
                    woh.CurrencyName = reader.GetString(13);
                    woh.StartDate = reader.GetDateTime(14);
                    woh.TargetDate = reader.GetDateTime(15);
                    woh.PaymentTerms = reader.GetString(16);
                    woh.PaymentMode = reader.GetString(17);
                    //woh.TaxCode = reader.GetString(18);
                    woh.POAddress = reader.GetString(18);
                    woh.ServiceValue = reader.GetDouble(19);
                    woh.TaxAmount = reader.GetDouble(20);
                    woh.TotalAmount = reader.GetDouble(21);
                    woh.TermsAndCond = reader.IsDBNull(22) ? " ":reader.GetString(22);
                    woh.Remarks = reader.GetString(23);
                    woh.Status = reader.GetInt32(24);
                    woh.DocumentStatus = reader.GetInt32(25);
                    woh.CreateTime = reader.GetDateTime(26);
                    woh.CreateUser = reader.GetString(27);
                    woh.ForwardUser = reader.GetString(28);
                    woh.ApproveUser = reader.GetString(29);
                    woh.CreatorName = reader.GetString(30);
                    woh.ForwarderName = reader.GetString(31);
                    woh.ApproverName = reader.GetString(32);
                    if (!reader.IsDBNull(33))
                    {
                        woh.CommentStatus = reader.GetString(33);
                    }
                    else
                    {
                        woh.CommentStatus = "";
                    }   
                    if (!reader.IsDBNull(34))
                    {
                        woh.ForwarderList = reader.GetString(34);
                    }
                    else
                    {
                        woh.ForwarderList = "";
                    }
                    woh.WorkOrderStatus = reader.GetInt32(35);
                    woh.WONo = reader.GetInt32(36);
                    woh.WODate = reader.GetDateTime(37);

                    WOHeaders.Add(woh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Work Order Header Details");
            }
            return WOHeaders;
        }

        public static List<workorderdetail> getWorkOrderDetails(workorderheader woh)
        {
            workorderdetail wod;
            List<workorderdetail> WODetail = new List<workorderdetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select a.RowID,a.DocumentID,a.TemporaryNo, a.TemporaryDate,a.StockItemID,b.Name as Description,a.WorkDescription,a.WorkLocation, " +
                   "a.Quantity,a.Price,a.Tax,a.WarrantyDays,a.TaxDetails,a.TaxCode " +
                   "from WODetail a , ServiceItem b " +
                   "where a.StockItemID = b.ServiceItemID and a.DocumentID='" + woh.DocumentID + "'" +
                   " and a.TemporaryNo=" + woh.TemporaryNo +
                   " and a.TemporaryDate='" + woh.TemporaryDate.ToString("yyyy-MM-dd")+ "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    wod = new workorderdetail();
                    wod.RowID = reader.GetInt32(0);
                    wod.DocumentID = reader.GetString(1);
                    wod.TemporaryNo = reader.GetInt32(2);
                    wod.TemporaryDate = reader.GetDateTime(3).Date;
                    wod.StockItemID = reader.IsDBNull(4)? "":reader.GetString(4);
                    wod.Description = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    wod.WorkDescription = reader.GetString(6);
                    wod.WorkLocation = reader.GetString(7);
                    wod.Quantity = reader.GetDouble(8);
                    wod.Price = reader.GetDouble(9);
                    wod.Tax = reader.GetDouble(10);
                    wod.WarrantyDays = reader.GetInt32(11);
                    wod.TaxDetails = reader.GetString(12);
                    wod.TaxCode = reader.IsDBNull(13) ? "" : reader.GetString(13);
                    WODetail.Add(wod);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Work Order Details");
            }
            return WODetail;
        }

        public Boolean UpdateWORequestHeader(workorderheader woh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update WOHeader set WorkOrderStatus = " + woh.WorkOrderStatus +
                    ", CommentStatus='" + woh.CommentStatus +
                    "', Comments='" + woh.Comments + "' "+
                   " where DocumentID='" + woh.DocumentID + "'" +
                   " and TemporaryNo=" + woh.TemporaryNo +
                   " and TemporaryDate='" + woh.TemporaryDate.ToString("yyyy-MM-dd") +"'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "WOHeader", "", updateSQL) +
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
        public Boolean validateWOStatusHeader(workorderheader woh)
        {
            Boolean status = true;
            try
            {
                if (woh.WorkOrderStatus == 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public static string getUserComments(string docid, int tempno, DateTime tempdate)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from WOHeader where DocumentID='" + docid + "'" +
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
    }
}
