using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class statuscatalogue
    {
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int Status { get; set; }
        public int Rowid { get; set; }
        public int ID { get; set; }
        public string Description { get; set; }      
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
    }

    class StatusCatalogueDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<statuscatalogue> getStatusCatalogue()
        {
            statuscatalogue    stclg;
            List<statuscatalogue> statcatlog = new List<statuscatalogue>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "  select a.RowID,a.DocumentID, b.DocumentName, a.ID,a.Description, a.CreateUser, "+
                    " a.CreateTime,a.Status from StatusCatalogue a, Document b where a.DocumentID=b.DocumentID"+
                    " order by a.DocumentId,a.ID";                    
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    stclg = new statuscatalogue();
                    stclg.Rowid = reader.GetInt32(0);
                    stclg.DocumentID = reader.GetString(1);
                    stclg.DocumentName = reader.GetString(2);
                    stclg.ID = reader.GetInt32(3);
                    stclg.Description = reader.GetString(4);
                    stclg.userCreateUser = reader.GetString(5);
                    stclg.userCreateime = reader.GetDateTime(6);
                    stclg.Status = reader.GetInt32(7);
                    statcatlog.Add(stclg);
                }
                conn.Close();
            }
            catch (Exception )
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return statcatlog;

        }
      
        public Boolean updateStatusCatalogue(statuscatalogue doc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StatusCatalogue set ID = '" + doc.ID + "',"+
                    "Description= '" + doc.Description+
                     "', Status= " + doc.Status +
                    " where RowID= " + doc.Rowid;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "StatusCatalogue", "", updateSQL) +
                    Main.QueryDelimiter;
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
        public Boolean insertStatusCatalogue(statuscatalogue doc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into StatusCatalogue(DocumentID,ID,Description,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + doc.DocumentID + "'," +
                    "'" + doc.ID + "'," +
                    "'" + doc.Description + "'," +
                     "'" + doc.Status + "'," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "StatusCatalogue", "", updateSQL) +
                    Main.QueryDelimiter;
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
        public Boolean validateDocument(statuscatalogue doc)
        {
            Boolean status = true;
            try
            {
                if (doc.DocumentID.Trim().Length == 0 || doc.DocumentID == null)
                {
                    return false;
                }
                if (doc.Description.Trim().Length == 0 || doc.Description == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return status;
        }
        public static void fillStatusCatalogueCombo(System.Windows.Forms.ComboBox cmb, string DocID)
        {
            cmb.Items.Clear();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select ID, Description from StatusCatalogue where DocumentID='" + DocID + "'" +
                        " and Status = 1 order by ID,Description asc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string desc = reader.GetString(1);
                    Structures.ComboBoxItem cbitem =
                           new Structures.ComboBoxItem(desc, id.ToString());
                    cmb.Items.Add(cbitem);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }

        }
    }
}

