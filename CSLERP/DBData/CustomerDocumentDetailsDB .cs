using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class customerdocumentdetails
    {
        public int rowID { get; set; }
        public int CustomerRowID { get; set; }
        public string CustomerName { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string DocumentValue { get; set; }
        public int status { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
    }

    class CustomerDocumentDetailsDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<customerdocumentdetails> getCustomerDocumentDetails(int CustomerRowID)
        {
            customerdocumentdetails cdd;
            List<customerdocumentdetails> cdDetails = new List<customerdocumentdetails>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.rowid,a.CustomerRowID,b.name,a.documentID,c.Description,a.documentValue,  a.Status " +
                    " from CustomerStatutoryDetail a,Customer b, CatalogueValue c   " +
                    " where a.CustomerRowID=b.RowID and a.documentID=c.CatalogueValueID and a.CustomerRowID='" + CustomerRowID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cdd = new customerdocumentdetails();
                    cdd.rowID = reader.GetInt32(0);
                    cdd.CustomerRowID = reader.GetInt32(1);
                    cdd.CustomerName = reader.GetString(2);
                    cdd.DocumentID = reader.GetString(3);
                    cdd.DocumentName = reader.GetString(4);
                    cdd.DocumentValue = reader.GetString(5);
                    cdd.status = reader.GetInt32(6);
                    cdDetails.Add(cdd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return cdDetails;
        }

        public Boolean updateCustomerDocumentDetails(string customerRowid, List<customerdocumentdetails> cddList)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Delete from CustomerStatutoryDetail where CustomerRowID='" + customerRowid + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("delete", "CustomerStatutoryDetail", "", updateSQL) +
                Main.QueryDelimiter;
                foreach (customerdocumentdetails cdd in cddList)
                {
                    updateSQL = "insert into CustomerStatutoryDetail (CustomerRowID,DocumentID,DocumentValue,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + cdd.CustomerRowID + "'," +
                    "'" + cdd.DocumentID + "'," +
                    "'" + cdd.DocumentValue + "'," +
                    cdd.status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "CustomerStatutoryDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
    }
}
