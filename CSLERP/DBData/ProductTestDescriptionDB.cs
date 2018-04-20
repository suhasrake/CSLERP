using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    //public strin "Data Source = PRAKASHPC; Initial Catalog = newERP; Integrated Security = True"
    class productTestDesc
    {
        public string TestDescriptionID { get; set; }
        public string TestDescription { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }

    class ProductTestDescriptionDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<productTestDesc> getProductTestDescriptionList()
        {
            productTestDesc ptdesc;
            List<productTestDesc> PTDescList = new List<productTestDesc>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select TestDescriptionID, TestDescription,Status,CreateTime,CreateUser from ProductTestDescription order by TestDescriptionID";
                    
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ptdesc = new productTestDesc();
                    ptdesc.TestDescriptionID = reader.GetString(0);
                    ptdesc.TestDescription = reader.GetString(1);
                    ptdesc.Status = reader.GetInt32(2);
                    ptdesc.CreateTime = reader.GetDateTime(3);
                    ptdesc.CreateUser = reader.GetString(4);
                    PTDescList.Add(ptdesc);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
               
            }
            return PTDescList;

        }
        //public string getDocumentStatusString(int userStatus)
        //{
        //    string documentStatusString = "Unknown";
        //    try
        //    {
        //        for (int i = 0; i < Document.documentStatusValues.GetLength(0); i++)
        //        {
        //            if (Convert.ToInt32(Document.documentStatusValues[i, 0]) == userStatus)
        //            {
        //                documentStatusString = Document.documentStatusValues[i, 1];
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        documentStatusString = "Unknown";
        //    }
        //    return documentStatusString;
        //}



        //public int getDocumentStatusCode(string documentStatusString)
        //{
        //    int documentStatusCode = 0;
        //    try
        //    {
        //        for (int i = 0; i < Document.documentStatusValues.GetLength(0); i++)
        //        {
        //            if (Document.documentStatusValues[i, 1].Equals(documentStatusString))
        //            {
        //                documentStatusCode = Convert.ToInt32(Document.documentStatusValues[i, 0]);
        //                break;
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        documentStatusCode = 0;
        //    }
        //    return documentStatusCode;
        //}

        public Boolean updateProductTestDescription(productTestDesc ptdesc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProductTestDescription set Status=" + ptdesc.Status + ","+
                    "TestDescription='"+ ptdesc.TestDescription+"'"+
                    " where TestDescriptionID='" + ptdesc.TestDescriptionID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "ProductTestDescription", "", updateSQL) +
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
        public Boolean insertProductTestDescription(productTestDesc ptdesc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into ProductTestDescription (TestDescriptionID,TestDescription,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + ptdesc.TestDescriptionID + "'," +
                    "'" + ptdesc.TestDescription + "'," +
                    ptdesc.Status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "ProductTestDescription", "", updateSQL) +
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
        public Boolean validateProductTestDescription(productTestDesc ptdesc)
        {
            Boolean status = true;
            try
            {
                if (ptdesc.TestDescriptionID.Trim().Length == 0 || ptdesc.TestDescriptionID == null)
                {
                    return false;
                }
                if (ptdesc.TestDescription.Trim().Length == 0 || ptdesc.TestDescription == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

            }
           
            return status;
        }
        //public static void fillDocumentIDCumbo(System.Windows.Forms.ComboBox cmb)
        //{
        //    cmb.Items.Clear();
        //    try
        //    {
        //        DocumentDB docdb = new DocumentDB();
        //        List<document> DocList = docdb.getDocuments();
        //        foreach (document doc in DocList)
        //        {
        //            if (doc.DocumentStatus == 1)
        //            {
        //                cmb.Items.Add(doc.DocumentID);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
        //    }

        //}
        public static ListView getTestDescriptionListView()
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
                ProductTestDescriptionDB ptdb = new ProductTestDescriptionDB();
                List<productTestDesc> PTDList = ptdb.getProductTestDescriptionList();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Desc ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Test Desc", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Status", -2, HorizontalAlignment.Left);
                foreach (productTestDesc ptd in PTDList)
                {
                    {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        item1.SubItems.Add(ptd.TestDescriptionID.ToString());
                        item1.SubItems.Add(ptd.TestDescription);
                        item1.SubItems.Add(ptd.Status.ToString());
                        lv.Items.Add(item1);
                    }
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
    }
}

