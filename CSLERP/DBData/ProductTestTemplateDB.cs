using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    public class producttesttemplateheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemplateNo { get; set; }
        public DateTime TemplateDate { get; set; }
        public string StockItemID { get; set; }
        public string StockItemName { get; set; }
        public string ModelNo { get; set; }
        public string ModelName { get; set; }
        public string TestDescriptionID { get; set; }
        public string TestDescription { get; set; }
        public string ExpectedResult { get; set; }
        public string Remark { get; set; }
        public int Status { get; set; }
        public string CreateUser { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreateTime { get; set; }

    }
    public class producttesttemplatedetail
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public int TemplateNo { get; set; }
        public DateTime TemplateDate { get; set; }
        public int SlNo { get; set; }
        public string TestDescriptionID { get; set; }
        public string TestDescription { get; set; }
        public string ExpectedResult { get; set; }
        public string Remark { get; set; }
    }
    class ProductTestTemplateDB
    {
        public static List<producttesttemplateheader> getFilteredProdTemp(int opt)
        {
            producttesttemplateheader PTTemp;
            List<producttesttemplateheader> PTTempList = new List<producttesttemplateheader>();
            try
            {
                string query1 = "select distinct DocumentID,TemplateNo,TemplateDate,StockItemID, " +
                     " StockitemName,ModelNo,ModelName," +
                    "Status, Createuser,CreatorName, CreateTime " +
                    "from ViewProductTestTemplate where (Createuser='" + Login.userLoggedIn + "' and Status = 0 )";
                string query2 = "select distinct DocumentID,TemplateNo,TemplateDate,StockItemID, " +
                     " StockitemName,ModelNo,ModelName," +
                    "Status, Createuser,CreatorName, CreateTime " +
                     "from ViewProductTestTemplate where Status = 1";
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
                    default:
                        query = "";
                        break;
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PTTemp = new producttesttemplateheader();
                    PTTemp.DocumentID = reader.GetString(0);
                    PTTemp.TemplateNo = reader.GetInt32(1);
                    PTTemp.TemplateDate = reader.GetDateTime(2);
                    PTTemp.StockItemID = reader.GetString(3);
                    PTTemp.StockItemName = reader.GetString(4);
                    PTTemp.ModelNo = reader.IsDBNull(5)?"NA":reader.GetString(5);
                    PTTemp.ModelName = reader.IsDBNull(6) ? "NA" : reader.GetString(6);
                    PTTemp.Status = reader.GetInt32(7);
                    PTTemp.CreateUser = reader.GetString(8);
                    PTTemp.CreatorName = reader.GetString(9);
                    PTTemp.CreateTime = reader.GetDateTime(10);

                    PTTempList.Add(PTTemp);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Serviced List Details");
            }
            return PTTempList;
        }
        public static List<producttesttemplatedetail> getProductTestTempDetail(producttesttemplateheader pttemph)
        {
            producttesttemplatedetail pttempd;
            List<producttesttemplatedetail> PTestDetail = new List<producttesttemplatedetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,TemplateNo,templateDate,TestDescriptionID,TestDescription,SlNo,ExpectedResult, Remarks " +
                   "from ViewProductTestTemplate " +
                    " where DocumentID='" + pttemph.DocumentID + "'" +
                    " and TemplateNo=" + pttemph.TemplateNo +
                    " and TemplateDate='" + pttemph.TemplateDate.ToString("yyyy-MM-dd") + "'" + " order by SLNo";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pttempd = new producttesttemplatedetail();
                    pttempd.RowID = reader.GetInt32(0);
                    pttempd.DocumentID = reader.GetString(1);
                    pttempd.TemplateNo = reader.GetInt32(2);
                    pttempd.TemplateDate = reader.GetDateTime(3).Date;
                    pttempd.TestDescriptionID = reader.GetString(4);
                    pttempd.TestDescription = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                    {
                        pttempd.SlNo = reader.GetInt32(6);
                    }

                    pttempd.ExpectedResult = reader.GetString(7);
                    pttempd.Remark = reader.GetString(8);
                    PTestDetail.Add(pttempd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PRoduct Test Tesmplate Details");
            }
            return PTestDetail;
        }
        //public Boolean updateProductTestTempHeader(producttesttemplateheader pttemp, producttesttemplateheader prevpttemp)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "update ProductTestTemplateHeader set   StockItemId='" + pttemp.StockItemID + "'" +
        //              ", ModelNo='" + pttemp.ModelNo + "'"+
        //             " where DocumentID='" + prevpttemp.DocumentID +
        //            "' and TemplateNo=" + prevpttemp.TemplateNo +
        //            " and TemplateDate='" + prevpttemp.TemplateDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("update", "ProductTestTemplateHeader", "", updateSQL) +
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
        //public Boolean insertProductTestTempHeader(producttesttemplateheader pttemp)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "insert into ProductTestTemplateHeader " +
        //            " (DocumentID,TemplateNo,TemplateDate,StockItemID,ModelNo," +
        //            "CreateUser,CreateTime)" +
        //            "values (" +
        //            "'" + pttemp.DocumentID + "'," +
        //           pttemp.TemplateNo + "," +
        //           "'" + pttemp.TemplateDate.ToString("yyyy-MM-dd") + "'," +
        //           "'" + pttemp.StockItemID + "'," +
        //           "'" + pttemp.ModelNo + "'," +
        //            "'" + Login.userLoggedIn + "'," +
        //            "GETDATE())";
        //        //"'" + pheader.ForwardUser + "'," +
        //        //"'" + pheader.ApproveUser + "'," +
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductTestTemplateHeader", "", updateSQL) +
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

        //public Boolean UpdateProductTestTempDetail(List<producttesttemplatedetail> PTTestDetail, producttesttemplateheader pttemp)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "Delete from ProductTestTemplateDetail where DocumentID='" + pttemp.DocumentID + "'" +
        //            " and TemplateNo=" + pttemp.TemplateNo +
        //            " and TemplateDate='" + pttemp.TemplateDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //            ActivityLogDB.PrepareActivityLogQquerString("delete", "ProductTestTemplateDetail", "", updateSQL) +
        //            Main.QueryDelimiter;
        //        foreach (producttesttemplatedetail PTTemp in PTTestDetail)
        //        {
        //            updateSQL = "insert into ProductTestTemplateDetail " +
        //            "(DocumentID,TemplateNo,TemplateDate,TestDescriptionID,SlNo,ExpectedResult,Remarks) " +
        //            "values ('" + PTTemp.DocumentID + "'," +
        //            PTTemp.TemplateNo + "," +
        //            "'" + PTTemp.TemplateDate.ToString("yyyy-MM-dd") + "'," +
        //            "'" + PTTemp.TestDescriptionID + "'," +
        //             PTTemp.SlNo + "," +
        //            "'" + PTTemp.ExpectedResult + "'," +
        //            "'" + PTTemp.Remark + "')";
        //            utString = utString + updateSQL + Main.QueryDelimiter;
        //            utString = utString +
        //            ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductTestTemplateDetail", "", updateSQL) +
        //            Main.QueryDelimiter;
        //        }
        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        status = false;
        //    }
        //    return status;
        //}
        //public Boolean deleteProductTestTempDetail(producttesttemplateheader pttemp)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "delete ProductTestTemplateDetail " +
        //            " where DocumentID='" + pttemp.DocumentID + "'" +
        //            " and TemplateNo=" + pttemp.TemplateNo +
        //            " and TemplateDate='" + pttemp.TemplateDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("delete", "ProductTestTemplateDetail", "", updateSQL) +
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

        public Boolean validateProductTestTemp(producttesttemplateheader pttemp)
        {
            Boolean status = true;
            try
            {
                if (pttemp.DocumentID.Trim().Length == 0 || pttemp.DocumentID == null)
                {
                    return false;
                }
                if (pttemp.StockItemID.Trim().Length == 0 || pttemp.StockItemID == null)
                {
                    return false;
                }
                if (pttemp.ModelNo.Trim().Length == 0 || pttemp.ModelNo == null)
                {
                    return false;
                }
                if (pttemp.ModelName.Trim().Length == 0 || pttemp.ModelName == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }
        public Boolean FinalizeProductTestTemp(producttesttemplateheader pttemp)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductTestTemplateHeader set Status = 1" +
                    " where TemplateNo=" + pttemp.TemplateNo +
                    " and TemplateDate='" + pttemp.TemplateDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductTestTemplateHeader", "", updateSQL) +
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
        public static Boolean checkProductAvailability(string StockItemID, string ModelNo)
        {
            Boolean status = true;
            producttesttemplateheader PTTemp;
            List<producttesttemplateheader> PTTempList = new List<producttesttemplateheader>();
            try
            {
                string query = "select StockItemID,ModelNo " +
                    "from ProductTestTemplateHeader";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    PTTemp = new producttesttemplateheader();
                    PTTemp.StockItemID = reader.GetString(0);
                    PTTemp.ModelNo = reader.IsDBNull(1)?"NA":reader.GetString(1);
                    PTTempList.Add(PTTemp);
                }
                conn.Close();
                foreach (producttesttemplateheader PTheader in PTTempList)
                {
                    if (PTheader.StockItemID.Equals(StockItemID) && PTheader.ModelNo.Equals(ModelNo))
                    {
                        MessageBox.Show("Product already Available. select another one.");
                        status = false;
                    }
                }
            }
            catch (Exception ex)
            {
                status = false;
                MessageBox.Show("Error querying Details");
            }
            return status;
        }
        public static Boolean checkProductAvailableForPrint(string StockItemID,string ModelNo)
        {
            Boolean status = true;
            int n = 0;
            try
            {
                string query = "select count(*) " +
                    "from ProductTestTemplateHeader where StockItemID='" + StockItemID + "' and ModelNo='"+ModelNo+"'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    n = reader.GetInt32(0);
                }
                conn.Close();
                if (n == 0)
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                status = false;
                MessageBox.Show("Error querying Details");
            }
            return status;
        }
        public static List<producttesttemplatedetail> getTemplateDetailForReport(string ID, string name,string ModelNo)
        {
            producttesttemplatedetail pttempd;
            List<producttesttemplatedetail> PTestDetail = new List<producttesttemplatedetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select TestDescriptionID,TestDescription,SlNo,ExpectedResult, Remarks " +
                   "from ViewProductTestTemplate " +
                    " where DocumentID='PRODUCTTESTTEMPLATE'" +
                    " and StockItemID='" + ID + "'" +
                       " and ModelNo='" + ModelNo + "'" +
                    " and StockItemName='" + name + "'" + " order by SLNo";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pttempd = new producttesttemplatedetail();
                    pttempd.TestDescriptionID = reader.GetString(0);
                    pttempd.TestDescription = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                    {
                        pttempd.SlNo = reader.GetInt32(2);
                    }

                    pttempd.ExpectedResult = reader.GetString(3);
                    pttempd.Remark = reader.GetString(4);
                    PTestDetail.Add(pttempd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying PRoduct Test Tesmplate Details");
            }
            return PTestDetail;
        }
        public Boolean updatePTHeaderAndDetail(producttesttemplateheader pttemp, List<producttesttemplatedetail> PTTestDetail)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductTestTemplateHeader set   StockItemId='" + pttemp.StockItemID + "'" +
                      ", ModelNo='" + pttemp.ModelNo + "'" +
                     " where DocumentID='" + pttemp.DocumentID +
                    "' and TemplateNo=" + pttemp.TemplateNo +
                    " and TemplateDate='" + pttemp.TemplateDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProductTestTemplateHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from ProductTestTemplateDetail where DocumentID='" + pttemp.DocumentID + "'" +
                    " and TemplateNo=" + pttemp.TemplateNo +
                    " and TemplateDate='" + pttemp.TemplateDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "ProductTestTemplateDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (producttesttemplatedetail PTTemp in PTTestDetail)
                {
                    updateSQL = "insert into ProductTestTemplateDetail " +
                    "(DocumentID,TemplateNo,TemplateDate,TestDescriptionID,SlNo,ExpectedResult,Remarks) " +
                    "values ('" + PTTemp.DocumentID + "'," +
                    PTTemp.TemplateNo + "," +
                    "'" + PTTemp.TemplateDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + PTTemp.TestDescriptionID + "'," +
                     PTTemp.SlNo + "," +
                    "'" + PTTemp.ExpectedResult + "'," +
                    "'" + PTTemp.Remark + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductTestTemplateDetail", "", updateSQL) +
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
        public Boolean InsertPTHeaderAndDetail(producttesttemplateheader pttemp, List<producttesttemplatedetail> PTTestDetail)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                pttemp.TemplateNo = DocumentNumberDB.getNumber(pttemp.DocumentID, 1);
                if (pttemp.TemplateNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + pttemp.TemplateNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + pttemp.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into ProductTestTemplateHeader " +
                    " (DocumentID,TemplateNo,TemplateDate,StockItemID,ModelNo," +
                    "CreateUser,CreateTime)" +
                    "values (" +
                    "'" + pttemp.DocumentID + "'," +
                   pttemp.TemplateNo + "," +
                   "'" + pttemp.TemplateDate.ToString("yyyy-MM-dd") + "'," +
                   "'" + pttemp.StockItemID + "'," +
                   "'" + pttemp.ModelNo + "'," +
                    "'" + Login.userLoggedIn + "'," +
                    "GETDATE())";
                //"'" + pheader.ForwardUser + "'," +
                //"'" + pheader.ApproveUser + "'," +
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductTestTemplateHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from ProductTestTemplateDetail where DocumentID='" + pttemp.DocumentID + "'" +
                     " and TemplateNo=" + pttemp.TemplateNo +
                     " and TemplateDate='" + pttemp.TemplateDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "ProductTestTemplateDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (producttesttemplatedetail PTTemp in PTTestDetail)
                {
                    updateSQL = "insert into ProductTestTemplateDetail " +
                    "(DocumentID,TemplateNo,TemplateDate,TestDescriptionID,SlNo,ExpectedResult,Remarks) " +
                    "values ('" + PTTemp.DocumentID + "'," +
                   pttemp.TemplateNo + "," +
                    "'" + PTTemp.TemplateDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + PTTemp.TestDescriptionID + "'," +
                     PTTemp.SlNo + "," +
                    "'" + PTTemp.ExpectedResult + "'," +
                    "'" + PTTemp.Remark + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductTestTemplateDetail", "", updateSQL) +
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
