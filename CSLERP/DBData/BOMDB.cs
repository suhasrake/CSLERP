using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class bomheader
    {
        public string ProductID { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public DateTime ReworkDate  { get; set; }
        public double Cost { get; set; }
        public int status { get; set; }
        public int DocumentStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string CommentStatus { get; set; }
        public string Comments { get; set; }
        public string ForwarderList { get; set; }
        public bomheader()
        {
           
            Comments = "";
        }
    }
    public class bomdetail
    {
        public string ProductID { get; set; }
        public string StockItemID { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public double PurchasePrice { get; set; }
        public double CustomPrice { get; set; }
        public int status { get; set; }
    }
    class BOMDB
    {
        ////////ActivityLogDB alDB = new ActivityLogDB();

        public List<bomheader> getBOMHeader(string userList, int opt, string userCommentStatusString)
        {
            bomheader bh;
            List<bomheader> BOMHeaders = new List<bomheader>();
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
                SqlConnection conn = new SqlConnection(Login.connString);
                string query1 = "select a.ProductID, b.Name,a.Details,a.ReworkDate,a.Cost,a.status, " +
                     "a.DocumentStatus,a.CreateUser,a.CreateTime,a.ForwardUser,a.ApproveUser,a.CommentStatus,a.ForwarderList " +
                    "from BOMHeader a, StockItem b where a.ProductID=b.StockItemID" +
                    " and ((a.forwarduser='" + Login.userLoggedIn + "' and a.DocumentStatus between 2 and 98) " +
                    " or (a.createuser='" + Login.userLoggedIn + "' and a.DocumentStatus=1)" +
                    " or (a.commentStatus like '%" + userCommentStatusString + "%' and a.DocumentStatus between 1 and 98)) order by a.ProductID";

                string query2 = "select a.ProductID, b.Name,a.Details,a.ReworkDate,a.Cost,a.status, " +
                     "a.DocumentStatus,a.CreateUser,a.CreateTime,a.ForwardUser,a.ApproveUser,a.CommentStatus,a.ForwarderList " +
                    "from BOMHeader a, StockItem b where a.ProductID=b.StockItemID" +
                    " and ((a.createuser='" + Login.userLoggedIn + "'  and a.DocumentStatus between 2 and 98 ) " +
                    " or (a.ForwarderList like '%" + userList + "%' and a.DocumentStatus between 2 and 98 and a.ForwardUser <> '" + Login.userLoggedIn + "')" +
                    " or (a.commentStatus like '%" + acStr + "%' and a.DocumentStatus between 1 and 98)) order by a.ProductID ";

                string query3 = "select a.ProductID, b.Name,a.Details,a.ReworkDate,a.Cost,a.status, " +
                     "a.DocumentStatus,a.CreateUser,a.CreateTime,a.ForwardUser,a.ApproveUser,a.CommentStatus,a.ForwarderList " +
                    "from BOMHeader a, StockItem b where a.ProductID=b.StockItemID" +
                    " and ((a.createuser='" + Login.userLoggedIn + "'" +
                    " or a.ForwarderList like '%" + userList + "%'" +
                    " or a.commentStatus like '%" + acStr + "%'" +
                    " or a.approveUser='" + Login.userLoggedIn + "')" +
                    " and a.DocumentStatus = 99)  order by a.ProductID";

              
                string query6 = "select a.ProductID, b.Name,a.Details,a.ReworkDate,a.Cost,a.status, " +
                     "a.DocumentStatus,a.CreateUser,a.CreateTime,a.ForwardUser,a.ApproveUser,a.CommentStatus,a.ForwarderList " +
                    "from BOMHeader a, StockItem b where a.ProductID=b.StockItemID" +
                    " and  a.DocumentStatus = 99 order by a.ProductID";
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
                    bh = new bomheader();
                    bh.ProductID = reader.GetString(0);
                    bh.Name = reader.GetString(1);
                    bh.Details = reader.GetString(2);
                    //bh.ReworkDate = reader.GetDateTime(3);
                    bh.Cost = reader.GetDouble(4);
                    bh.status = reader.GetInt32(5);
                    bh.DocumentStatus = reader.GetInt32(6);
                    bh.CreateUser = reader.GetString(7);
                    bh.CreateTime = reader.GetDateTime(8);
                    bh.ForwardUser = !(reader.IsDBNull(9))? reader.GetString(9): "";
                    bh.ApproveUser = !(reader.IsDBNull(10)) ? reader.GetString(10) : "";
                    bh.CommentStatus = !(reader.IsDBNull(11)) ? reader.GetString(11) : "";
                    bh.ForwarderList = !(reader.IsDBNull(12)) ? reader.GetString(12) : "";
                    BOMHeaders.Add(bh);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return BOMHeaders;
        }

        public List<bomdetail> getBOMDetail(string productID)
        {
            bomdetail bd;
            List<bomdetail> BOMDetails = new List<bomdetail>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.ProductID, a.StockItemID,b.Name, a.Quantity,a.PurchasePrice,a.CustomPrice " +
                    "from BOMDetail a, StockItem b  where a.StockItemID= b.StockItemID and a.ProductID='" + productID +
                    "' order by b.Name";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    bd = new bomdetail();
                    bd.ProductID = reader.GetString(0);
                    bd.StockItemID = reader.GetString(1);
                    bd.Name = reader.GetString(2);
                    bd.Quantity = reader.GetDouble(3);
                    bd.PurchasePrice = reader.GetDouble(4);
                    bd.CustomPrice = reader.GetDouble(5);
                    BOMDetails.Add(bd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return BOMDetails;
        }

        public double getBOMCost(string  productID)
        {
            double BOMCost = 0.0;
            try
            {
                string query = "select cost from BOMHeader "+
                    " where status = 1 and ProductID='" + productID + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    BOMCost = reader.GetDouble(0);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                BOMCost = 0.0;
            }
            return BOMCost;
        }

        public Boolean validateBOMHeader(bomheader bh)
        {
            Boolean status = true;
            try
            {
                if (bh.ProductID.Trim().Length == 0 || bh.ProductID == null)
                {
                    return false;
                }
                if (bh.Details.Trim().Length == 0 || bh.Details == null)
                {
                    return false;
                }
                if (bh.Cost <= 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                return false;
            }
            return status;
        }
        public Boolean forwardBOM(bomheader boh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update BOMHeader set DocumentStatus=" + (boh.DocumentStatus + 1) +
                    ", forwardUser='" + boh.ForwardUser + "'" +
                    ", commentStatus='" + boh.CommentStatus + "'" +
                    ", ForwarderList='" + boh.ForwarderList + "'" +
                    " where ProductID='" + boh.ProductID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "BOMHeader", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }

        public Boolean reverseBOM(bomheader bomh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update BOMHeader set DocumentStatus=" + bomh.DocumentStatus +
                    ", forwardUser='" + bomh.ForwardUser + "'" +
                    ", commentStatus='" + bomh.CommentStatus + "'" +
                    ", ForwarderList='" + bomh.ForwarderList + "'" +
                     " where ProductID='" + bomh.ProductID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "BOMHeader", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }

        public Boolean ApproveBOM(bomheader bomh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update BOMHeader set DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", commentStatus='" + bomh.CommentStatus + "'" +
                    ", ReworkDate = convert(date, getdate())" +
                   " where ProductID='" + bomh.ProductID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "BOMHeader", "", updateSQL) +
                Main.QueryDelimiter;

                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public static string getUserComments(string productID)
        {
            string cmtString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select comments from BOMHeader where ProductID='" + productID + "'";
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
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return cmtString;
        }
        public static Boolean checkBOMPrepaaredForAnItem(string productID)
        {
            int count = 0;
            Boolean stat = true;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select count(*) from BOMDetail where ProductID='" + productID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    count = reader.GetInt32(0);
                }
                if (count == 0)
                    stat = false;
                else
                    stat = true;
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                stat = false;
            }
            return stat;
        }
        public Boolean updateBOMHeaderAndDetail(bomheader bh, bomheader prevbh, List<bomdetail> BOMDEtails)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update BOMHeader set Details='" + bh.Details +
                     "',ReworkDate=GETDATE()" +
                     ", Cost=" + bh.Cost +
                      ", CommentStatus='" + bh.CommentStatus +
                     "', Comments='" + bh.Comments +
                     "', ForwarderList='" + bh.ForwarderList + "'" +
                     " where ProductID='" + prevbh.ProductID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "BOMHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from BOMDetail where ProductID='" + prevbh.ProductID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "BOMDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (bomdetail bd in BOMDEtails)
                {
                    updateSQL = "insert into BOMDetail " +
                    "(ProductID,StockItemID,Quantity,PurchasePrice,CustomPrice) " +
                    "values (" +
                    "'" + bd.ProductID + "'," +
                    "'" + bd.StockItemID + "'," +
                    bd.Quantity + "," +
                    bd.PurchasePrice + "," +
                    bd.CustomPrice + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "BOMDetail", "", updateSQL) +
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
        public Boolean InsertBOMHeaderAndDetail(bomheader bh, List<bomdetail> BOMDEtails)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                updateSQL = "insert into BOMHeader (ProductID,Details,Cost,Status," +
                    " DocumentStatus,CreateUser,CreateTime,ForwarderList,Comments,CommentStatus)" +
                     "values (" +
                     "'" + bh.ProductID + "'," +
                     "'" + bh.Details + "'," +
                     bh.Cost + "," +
                     bh.status + "," +
                     bh.DocumentStatus + "," +
                      "'" + Login.userLoggedIn + "'," +
                     "GETDATE()" + "," +
                      "'" + bh.ForwarderList + "'," +
                      "'" + bh.Comments + "'," +
                      "'" + bh.CommentStatus + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "BOMHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from BOMDetail where ProductID='" + bh.ProductID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "BOMDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (bomdetail bd in BOMDEtails)
                {
                    updateSQL = "insert into BOMDetail " +
                    "(ProductID,StockItemID,Quantity,PurchasePrice,CustomPrice) " +
                    "values (" +
                    "'" + bd.ProductID + "'," +
                    "'" + bd.StockItemID + "'," +
                    bd.Quantity + "," +
                    bd.PurchasePrice + "," +
                    bd.CustomPrice + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "BOMDetail", "", updateSQL) +
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

        public DataGridView getGridViewForBOMDetail(string ProdID, string modNo)
        {

            DataGridView grdPOPI = new DataGridView();
            try
            {
                string[] strColArr = { "Naveen", "StockItemName","Quantity","PurchasePrice", "CustomPrice"};
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                };

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
                grdPOPI.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str;
                    colArr[index].HeaderText = str;
                    colArr[index].ReadOnly = true;
                    //if (index == 1 || index == 3)
                    //    colArr[index].DefaultCellStyle.Format = "dd-MM-yyyy";
                    if (index == 0)
                        colArr[index].Width = 120;
                    else if (index == 1)
                        colArr[index].Width = 250;
                    else
                        colArr[index].Width = 100;
                    if (index == 3)
                        colArr[index].Visible = false;
                    else
                        colArr[index].Visible = true;
                    grdPOPI.Columns.Add(colArr[index]);
                }

                BOMDB bomdb = new BOMDB();
                List<bomdetail> bomDet = new List<bomdetail>();
                bomDet = bomdb.getBOMDetail(ProdID);
                foreach (bomdetail bom in bomDet)
                {
                    grdPOPI.Rows.Add();
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[0]].Value = bom.StockItemID;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[1]].Value = bom.Name;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[2]].Value = bom.Quantity;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[3]].Value = bom.PurchasePrice;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[4]].Value = bom.CustomPrice;
                }
            }
            catch (Exception ex)
            {
            }

            return grdPOPI;
        }
    }
}
