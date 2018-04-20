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
    class documentuc
    {
        public string UserID { get; set; }
        public int Status { get; set; }
        public string empid { get;set; }
        public string empname { get; set; }
        public DateTime Createime { get; set; }
        public string CreateUser { get; set; }
    }

    class DocumentUCDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<documentuc> getDocuments()
        {
            documentuc docrec;
            List<documentuc> DocUC = new List<documentuc>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.UserID, a.Status,b.EmployeeID,b.Name from DocumentUC a Inner Join ViewUserEmployeeList b on a.UserID=b.UserID";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    docrec = new documentuc();
                    docrec.UserID = reader.GetString(0);
                    docrec.Status = reader.GetInt32(1);
                    docrec.empid = reader.GetString(2);
                    docrec.empname = reader.GetString(3);
                    DocUC.Add(docrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            return DocUC;

        }
      
        public string getYesNo(int YesNoValue)
        {
            string YesNoString = "Unknown";
            try
            {
                for (int i = 0; i < DocumentUC.documentStatusValues.GetLength(0); i++)
                {
                    if (Convert.ToInt32(DocumentUC.documentStatusValues[i, 0]) == YesNoValue)
                    {
                        YesNoString = DocumentUC.documentStatusValues[i, 1];
                        break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                YesNoString = "Unknown";
            }
            return YesNoString;
        }  
                              
        public int getYesNoCode(string YesNoString)
        {
            int YesNoCode = 0;
            try
            {
                for (int i = 0; i < Main.YesNo.GetLength(0); i++)
                {
                    if (Main.YesNo[i, 1].Equals(YesNoString))
                    {
                        YesNoCode = Convert.ToInt32(Main.YesNo[i, 0]);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                YesNoCode = 0;
            }
            return YesNoCode;
        }
        public Boolean updateDocument(documentuc doc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update DocumentUC set Status=" + doc.Status + " where UserID='" + doc.UserID + "'";

                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentUC", "", updateSQL) +
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
        public Boolean insertDocument(documentuc doc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DocumentDB dc = new DocumentDB();

                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into DocumentUC (UserID,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + doc.UserID + "'," +
                    doc.Status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "DocumentUC", "", updateSQL) +
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
        public Boolean validateDocument(documentuc doc)
        {
            Boolean status = true;
            try
            {
                if (doc.UserID.Trim().Length == 0 || doc.UserID == null)
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
        public static void fillDocumentIDCumbo(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                DocumentUCDB docdb = new DocumentUCDB();
                List<documentuc> DocList = docdb.getDocuments();
                foreach (documentuc doc in DocList)
                {
                    if (doc.Status == 1)
                    {
                        cmb.Items.Add(doc.UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }

        }
        public static string getUserID(string empid)
        {
            DocumentUC doc1 = new DocumentUC();
            string str = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select UserID from ViewUserEmployeeList where EmployeeID='" + empid + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    str = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                str = "";
            }
            return str;
        }
    }
}

