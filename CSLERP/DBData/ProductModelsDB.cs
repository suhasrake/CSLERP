using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class productmodels
    {
        public int rowID { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
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
        public string ForwarderList { get; set; }
    }

    class ProductModelsDB
    {
        public List<productmodels> getFilteredModelName(string userList, int opt, string stockid)
        {
            productmodels pmodel;
            List<productmodels> pmodelList = new List<productmodels>();
            try
            {
                string query1 = "select RowID,ModelNo,ModelName," +
                    " StockItemID,StockItemName,Remarks," +
                    " Status,DocumentStatus,CreateTime, " +
                    "CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName , ForwarderList" +
                    " from ViewProductModel" +
                   " where ((ForwardUser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98 and StockItemID = '" + stockid + "') " +
                    " or (CreateUser='" + Login.userLoggedIn + "' and DocumentStatus=1 and StockItemID = '" + stockid + "'))";

                string query2 = "select RowID,ModelNo,ModelName," +
                    " StockItemID,StockItemName,Remarks," +
                    " Status,DocumentStatus,CreateTime, " +
                    "CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName , ForwarderList" +
                    " from ViewProductModel" +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 and StockItemID = '" + stockid + "') " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "' and StockItemID = '" + stockid + "'))";

                string query3 = "select RowID,ModelNo,ModelName," +
                    " StockItemID,StockItemName,Remarks," +
                    " Status,DocumentStatus,CreateTime, " +
                    "CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName , ForwarderList" +
                    " from ViewProductModel" +
                     " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99 and StockItemID = '" + stockid + "')";
                string query6 = "select RowID,ModelNo,ModelName," +
                    " StockItemID,StockItemName,Remarks," +
                    " Status,DocumentStatus,CreateTime, " +
                    "CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName , ForwarderList" +
                    " from ViewProductModel" +
                   " where  DocumentStatus = 99  and Status = 1 and StockItemID = '" + stockid + "'";

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
                    pmodel = new productmodels();
                    pmodel.rowID = reader.GetInt32(0);
                    pmodel.ModelNo = reader.GetString(1);
                    pmodel.ModelName = reader.GetString(2);
                    pmodel.StockItemID = reader.GetString(3);
                    pmodel.StockItemName = reader.GetString(4);
                    pmodel.Remarks = reader.GetString(5);
                    pmodel.Status = reader.GetInt32(6);
                    pmodel.DocumentStatus = reader.GetInt32(7);
                    pmodel.CreateTime = reader.GetDateTime(8);
                    pmodel.CreateUser = reader.GetString(9);
                    pmodel.ForwardUser = reader.GetString(10);
                    pmodel.ApproveUser = reader.GetString(11);
                    pmodel.CreatorName = reader.GetString(12);
                    pmodel.ForwarderName = reader.GetString(13);
                    pmodel.ApproverName = reader.GetString(14);
                    if (!reader.IsDBNull(15))
                    {
                        pmodel.ForwarderList = reader.GetString(15);
                    }
                    else
                    {
                        pmodel.ForwarderList = "";
                    }
                    pmodelList.Add(pmodel);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Product Model Data");
            }
            return pmodelList;

        }
        public Boolean updateProductModels(productmodels pmodel, productmodels prevpmodel)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductModel set   ModelDetails='" + pmodel.ModelName + "'" +
                     ", StockItemID='" + pmodel.StockItemID +
                    "', Remarks ='" + pmodel.Remarks +
                    "', ForwarderList='" + pmodel.ForwarderList +
                     "' where RowID=" + prevpmodel.rowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductModel", "", updateSQL) +
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
        public Boolean insertProductModel(productmodels pmodel)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into ProductModel " +
                    " (ModelNo,ModelName,StockItemID,Remarks," +
                    "DocumentStatus,Status,CreateUser,CreateTime,ForwarderList)" +
                    "values (" +
                    "'0'," +
                    "'" + pmodel.ModelName + "'," +
                    "'" + pmodel.StockItemID + "'," +
                    "'" + pmodel.Remarks + "'," +
                    pmodel.DocumentStatus + "," +
                    pmodel.Status + "," +
                    "'" + Login.userLoggedIn + "'," +
                    "GETDATE()" +
                    ",'" + pmodel.ForwarderList + "')";
                //"'" + pheader.ForwardUser + "'," +
                //"'" + pheader.ApproveUser + "'," +
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductModel", "", updateSQL) +
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
        public Boolean validateProductModel(productmodels pmodel)
        {
            Boolean status = true;
            try
            {
                if (pmodel.StockItemID.Trim().Length == 0 || pmodel.StockItemID == null)
                {
                    return false;
                }
                if (pmodel.Remarks.Trim().Length == 0 || pmodel.Remarks == null)
                {
                    return false;
                }
                if (pmodel.ModelName.Trim().Length == 0 || pmodel.ModelName == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to insert product model.");
            }

            return status;
        }
        public Boolean forwardProductModel(productmodels prevmodel)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductModel set DocumentStatus=" + (prevmodel.DocumentStatus + 1) +
                     ", ForwardUser='" + prevmodel.ForwardUser + "'" +
                    ", ForwarderList='" + prevmodel.ForwarderList + "'" +
                     " where StockItemID='" + prevmodel.StockItemID + "'" +
                      " and RowID = " + prevmodel.rowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("forward", "ProductModel", "", updateSQL) +
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
        public Boolean reverseProductModel(productmodels prevpmodel)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductModel set DocumentStatus=" + prevpmodel.DocumentStatus +
                    ", forwardUser='" + prevpmodel.ForwardUser + "'" +
                    ", ForwarderList='" + prevpmodel.ForwarderList + "'" +
                     " where StockItemID='" + prevpmodel.StockItemID + "'" +
                      " and RowID = " + prevpmodel.rowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductModel", "", updateSQL) +
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
        public Boolean ApproveProductModel(productmodels pmodel, string modelNo)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductModel set DocumentStatus=99" +
                    ", Status=1" +
                    ", ModelNo='" + modelNo + "'" +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                     " where StockItemID='" + pmodel.StockItemID + "'" +
                      " and RowID = " + pmodel.rowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "ProductModel", "", updateSQL) +
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
        public string getLastModelNo(string stockID)
        {
            string no = "";
            try
            {
                string query = "select Max(ModelNo)" +
                    " from ProductModel" +
                   " where StockItemID='" + stockID + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        no = reader.GetString(0);
                    }
                    else
                    {
                        no = "0";
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Last Product Model No");
            }
            return no;
        }
        public static ListView getModelsForProductListView(string stockID)
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
                ProductModelsDB pmdb = new ProductModelsDB();
                List<productmodels> PModels = pmdb.getFilteredModelName("", 6, stockID);
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Model No", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Model Details", -2, HorizontalAlignment.Left);
                foreach (productmodels pm in PModels)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(pm.ModelNo.ToString());
                    item1.SubItems.Add(pm.ModelName.ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
    }
}
