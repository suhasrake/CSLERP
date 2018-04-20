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
    class materialdelivery
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentType { get; set; }
        public int DocumentNo { get; set; }
        public string consignee { get; set; }
        public DateTime DocumentDate { get; set; }
        public string LRNo { get; set; }
        public DateTime LRDate { get; set; }
        public string courierID { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string transportationMode { get; set; }
        public string Remarks { get; set; }
        public int status { get; set; }
        public int DeliveryStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string customername { get; set; }
        public string Couriername { get; set; }
        
        public string CustomerApprovalNo { get; set; }
    }

    class MaterialDeliveryDetailDB
    {
        public List<materialdelivery> getFilteredMaterialdetail(int opt, string docrecvStr)
        {
            materialdelivery matdelivery;
            List<materialdelivery> Materialdetaildelivery = new List<materialdelivery>();
            try
            {
                //approved user comment status string
                //-----'
                string query1 = "select a.RowID , a.DeliveryDocumentType, a.DocumentNo, a.DocumentDate , a.Consignee, " + 
                               " a.CourierID, a.TransportationMode , a.LRNo, a.LRDate, a.CreateUser, a.CreateTime, "+
                               " a.Status, a.Remarks, a.DeliveryStatus, a.DeliveryDate,b.Name,c.Name,a.CustomerApprovalNo,a.DocumentID from MaterialDeliveryDetail a, Customer b, Customer c " +
                               " where a.Consignee = b.CustomerID and a.CourierID=c.CustomerID and a.Status = 0 ";

                string query2 = "select a.RowID , a.DeliveryDocumentType, a.DocumentNo, a.DocumentDate , a.Consignee, " +
                               " a.CourierID, a.TransportationMode , a.LRNo, a.LRDate, a.CreateUser, a.CreateTime, " +
                               " a.Status, a.Remarks, a.DeliveryStatus, a.DeliveryDate,b.Name,c.Name,a.CustomerApprovalNo,a.DocumentID from MaterialDeliveryDetail a, Customer b, Customer c " +
                               " where a.Consignee = b.CustomerID and a.CourierID=c.CustomerID and a.Status = 1 and a.DeliveryStatus=1 ";

                string query3 = "select a.RowID , a.DeliveryDocumentType, a.DocumentNo, a.DocumentDate , a.Consignee, " +
                               " a.CourierID, a.TransportationMode , a.LRNo, a.LRDate, a.CreateUser, a.CreateTime, " +
                               " a.Status, a.Remarks, a.DeliveryStatus, a.DeliveryDate,b.Name,c.Name,a.CustomerApprovalNo,a.DocumentID from MaterialDeliveryDetail a, Customer b, Customer c " +
                               " where a.Consignee = b.CustomerID and a.CourierID=c.CustomerID and a.Status = 1  and a.DeliveryStatus=2";

                //string docRcvQry = " and (" + docrecvStr + ")" ;
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
                    default:
                        query = "";
                        break;
                }

                //if (docrecvStr.Length != 0)
                //{
                //    query = query + docRcvQry;
                //}
                   

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    matdelivery = new materialdelivery();
                    matdelivery.RowID = reader.GetInt32(0);
                    matdelivery.DocumentType = reader.GetString(1);
                    matdelivery.DocumentNo = reader.GetInt32(2);
                    if (!reader.IsDBNull(3))
                    {
                        matdelivery.DocumentDate = reader.GetDateTime(3);
                    }
                    matdelivery.consignee = reader.GetString(4);
                    matdelivery.courierID = reader.GetString(5);
                    matdelivery.transportationMode = reader.GetString(6);
                    matdelivery.LRNo = reader.GetString(7);
                    if (!reader.IsDBNull(8))
                    {
                        matdelivery.LRDate = reader.GetDateTime(8);
                    }
                    matdelivery.CreateUser = reader.GetString(9);
                    matdelivery.CreateTime = reader.GetDateTime(10);
                    matdelivery.status = reader.GetInt32(11);
                    matdelivery.Remarks = reader.GetString(12);
                    matdelivery.DeliveryStatus = reader.GetInt32(13);
                    if (!reader.IsDBNull(14))
                    {
                        matdelivery.DeliveryDate = reader.GetDateTime(14);
                    }
                    matdelivery.customername = reader.GetString(15);
                    matdelivery.Couriername = reader.GetString(16);
                    matdelivery.CustomerApprovalNo = reader.IsDBNull(17)?"":reader.GetString(17);
                    matdelivery.DocumentID = reader.IsDBNull(18) ? "" : reader.GetString(18);
                    Materialdetaildelivery.Add(matdelivery);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying MAterial Delivery Details");
            }
            return Materialdetaildelivery;
        }


        public static ListView getInvoiceOutListView()
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
                InvoiceOutHeaderDB IODB = new InvoiceOutHeaderDB();
                List<invoiceoutheader> IOList = IODB.getinvoiceforproduct();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("DocumentID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Document Name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Invoice No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Invoice Date", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Consignee", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Invoice Amount", -2, HorizontalAlignment.Left);
                foreach (invoiceoutheader ioh in IOList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(ioh.DocumentID.ToString());
                    item1.SubItems.Add(ioh.DocumentName.ToString());
                    item1.SubItems.Add(ioh.InvoiceNo.ToString());
                    item1.SubItems.Add(ioh.InvoiceDate.ToString());
                    item1.SubItems.Add(ioh.ConsigneeID + "-" + ioh.ConsigneeName);
                    item1.SubItems.Add(ioh.InvoiceAmount.ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }

        public Boolean updateMaterialDelivery(materialdelivery mdd)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update MaterialDeliveryDetail set DeliveryDocumentType='" + mdd.DocumentType +
                    "', LRDate='" + mdd.LRDate.ToString("yyyy-MM-dd") +
                      "', DeliveryDate='" + mdd.DeliveryDate.ToString("yyyy-MM-dd") +
                    "', Consignee='" + mdd.consignee +
                    "',CourierID='" + mdd.courierID +
                    "', TransportationMode ='" + mdd.transportationMode +
                    "', LRNo=" + mdd.LRNo +
                    ", Remarks='" + mdd.Remarks +
                    "', DeliveryStatus='" + mdd.DeliveryStatus +
                    "', CustomerApprovalNo='" + mdd.CustomerApprovalNo +
                    "' where DocumentNo='" + mdd.DocumentNo + "'" +
                     " and DocumentDate = '" + mdd.DocumentDate.ToString("yyyy-MM-dd") + "'"+
                     " and RowID='"+mdd.RowID+"'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "MaterialDeliveryDetail", "", updateSQL) +
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
        public Boolean updateMaterialDeliveryStatus(materialdelivery mdd)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update MaterialDeliveryDetail set " +
                    " Remarks='" + mdd.Remarks +
                    "' where DocumentNo='" + mdd.DocumentNo + "'" +
                     " and DocumentDate = '" + mdd.DocumentDate.ToString("yyyy-MM-dd") + "' and RowID='" + mdd.RowID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "MaterialDeliveryDetail", "", updateSQL) +
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

        public Boolean insertMaterialDetail(materialdelivery mdd)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into MaterialDeliveryDetail " +
                    "(DeliveryDocumentType, DocumentID,DocumentNo,DocumentDate,Consignee,CourierID,TransportationMode,LRNo,LRDate," +
                    "DeliveryDate,DeliveryStatus,Remarks,Status,CustomerApprovalNo,CreateTime,CreateUser)" +
                    " values (" +
                    "'" + mdd.DocumentType + "'," +
                     "'" + mdd.DocumentID + "'," +
                    "'" + mdd.DocumentNo + "'," +
                    "'" + mdd.DocumentDate.ToString("yyyy-MM-dd") + "','" +
                    mdd.consignee + "'," +
                    "'" + mdd.courierID + "'," +
                    "'" + mdd.transportationMode + "'," +
                         "'" + mdd.LRNo + "'," +
                           "'" + mdd.LRDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + mdd.DeliveryDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + mdd.DeliveryStatus + "'," +
                    "'" + mdd.Remarks + "'," +
                    mdd.status + "," +
                    "'" + mdd.CustomerApprovalNo + "'," +
                   
                     "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "MaterialDeliveryDetail", "", updateSQL) +
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

        public Boolean updateDelivery(materialdelivery mdd)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update MaterialDeliveryDetail set " +
                    " Remarks='" + mdd.Remarks + "',DeliveryStatus='"+ mdd.DeliveryStatus + "' , DeliveryDate ='"+mdd.DeliveryDate.ToString("yyyy-MM-dd")+ 
                    "' where DocumentNo='" + mdd.DocumentNo + "'" +
                     " and DocumentDate = '" + mdd.DocumentDate.ToString("yyyy-MM-dd") + "' and RowID='" + mdd.RowID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "MaterialDeliveryDetail", "", updateSQL) +
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


        public Boolean DeleteDelivery(materialdelivery mdd)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "delete from MaterialDeliveryDetail where DocumentNo='"+mdd.DocumentNo+"' "+
                                   "and DocumentDate='"+mdd.DocumentDate.ToString("yyyy-MM-dd")+ "' and RowID='" + mdd.RowID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("delete", "MaterialDeliveryDetail", "", updateSQL) +
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


        public Boolean FinalizeDetails(materialdelivery mdd)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update MaterialDeliveryDetail set  status=1" +
                    " where DocumentNo='" + mdd.DocumentNo + "'" +
                    " and DocumentDate='" + mdd.DocumentDate.ToString("yyyy-MM-dd") + "' and RowID='" + mdd.RowID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "MaterialDeliveryDetail", "", updateSQL) +
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

        public List<materialdelivery> getInvoiceDetails(string InvoiceNo, DateTime InvoiceDate)
        {
            materialdelivery matdelivery;
            List<materialdelivery> Materialdetaildelivery = new List<materialdelivery>();
            try
            {

                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID,Consignee,TransportationMode,Remarks,Transporter" +
                    " from InvoiceOutHeader where InvoiceNo = '" + InvoiceNo + "' and" +
                    " InvoiceDate = '" + InvoiceDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    matdelivery = new materialdelivery();
                    matdelivery.consignee = reader.GetString(1);
                    matdelivery.courierID = reader.GetString(4);
                    matdelivery.transportationMode = reader.GetString(2);
                    Materialdetaildelivery.Add(matdelivery);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return Materialdetaildelivery;
        }

    }
}
