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
    class catalogue
    {
        public string catalogueID { get; set; }
        public string description { get; set; }
        public int status { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
    }

    class CatalogueDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public  List<catalogue> getCatalogues()
        {
            catalogue cat;
            List<catalogue> Catalogues = new List<catalogue>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select CatalogueID, Description,status "+
                    "from Catalogue order by CatalogueID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cat = new catalogue();
                    cat.catalogueID = reader.GetString(0);
                    cat.description = reader.GetString(1);
                    cat.status = reader.GetInt32(2);
                    Catalogues.Add(cat);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            
            }
            return Catalogues;
            
        }
 
        public Boolean updateCatalogue(catalogue cat )
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update Catalogue set Description='"+cat.description + 
                    "',Status="+cat.status+
                    " where CatalogueID='" + cat.catalogueID+"'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("update", "Catalogue", "", updateSQL) +
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
        public Boolean insertCatalogue(catalogue cat)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                
                string updateSQL = "insert into Catalogue (catalogueID,Description,Status,CreateTime,CreateUser)"+
                    "values (" +
                    "'" + cat.catalogueID + "'," +
                     "'" + cat.description + "'," +
                    cat.status+","+
                    "GETDATE()"+","+
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("insert", "Catalogue", "", updateSQL) +
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
        public Boolean validateCatalogue(catalogue cat)
        {
            Boolean status = true;
            try
            {
                if (cat.catalogueID.Trim().Length == 0 || cat.catalogueID == null)
                {
                    return false;
                }
                if (cat.description.Trim().Length == 0 || cat.description == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                return false;
            }
            return status;
        }
    }
}
