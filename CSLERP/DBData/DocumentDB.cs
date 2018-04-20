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
    class document
    {
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string TableName { get; set; }
        public int DocumentStatus { get; set; }
        public int IsReversible { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
    }

    class DocumentDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<document> getDocuments()
        {
            document documentrec;
            List<document> Documents = new List<document>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID, DocumentName,TableName,IsReversible,status from Document order by DocumentName";
                    
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    documentrec = new document();
                    documentrec.DocumentID = reader.GetString(0);
                    documentrec.DocumentName = reader.GetString(1);
                    documentrec.TableName = reader.IsDBNull(2)? "": reader.GetString(2);
                    documentrec.IsReversible = reader.GetInt32(3);
                    documentrec.DocumentStatus = reader.GetInt32(4);
                    Documents.Add(documentrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            return Documents;

        }
        public string getDocumentStatusString(int userStatus)
        {
            string documentStatusString = "Unknown";
            try
            {
                for (int i = 0; i < Main.statusValues.GetLength(0); i++)
                {
                    if (Convert.ToInt32(Main.statusValues[i, 0]) == userStatus)
                    {
                        documentStatusString = Main.statusValues[i, 1];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                documentStatusString = "Unknown";
            }
            return documentStatusString;
        }

        public string getYesNo(int YesNoValue)
        {
            string YesNoString = "Unknown";
            try
            {
                for (int i = 0; i < Main.YesNo.GetLength(0); i++)
                {
                    if (Convert.ToInt32(Main.YesNo[i, 0]) == YesNoValue)
                    {
                        YesNoString = Main.YesNo[i, 1];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                YesNoString = "Unknown";
            }
            return YesNoString;
        }

        public int getDocumentStatusCode(string documentStatusString)
        {
            int documentStatusCode = 0;
            try
            {
                for (int i = 0; i < Main.statusValues.GetLength(0); i++)
                {
                    if (Main.statusValues[i, 1].Equals(documentStatusString))
                    {
                        documentStatusCode = Convert.ToInt32(Main.statusValues[i, 0]);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                documentStatusCode = 0;
            }
            return documentStatusCode;
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
        public Boolean updateDocument(document doc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update Document set Status=" + doc.DocumentStatus + ","+
                    " IsReversible = " + doc.IsReversible + ","+
                    " DocumentName='" +doc.DocumentName + "'," +
                    " TableName='" + doc.TableName + "'" +
                    " where DocumentID='" + doc.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "Document", "", updateSQL) +
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
        public Boolean insertDocument(document doc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into Document (DocumentID,DocumentName,TableName,IsReversible,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + doc.DocumentID + "'," +
                    "'" + doc.DocumentName + "'," +
                    "'" + doc.TableName + "'," +
                     doc.IsReversible + "," +
                    doc.DocumentStatus + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "Document", "", updateSQL) +
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
        public Boolean validateDocument(document doc)
        {
            Boolean status = true;
            try
            {
                if (doc.DocumentID.Trim().Length == 0 || doc.DocumentID == null)
                {
                    return false;
                }
                if (doc.DocumentName.Trim().Length == 0 || doc.DocumentName == null)
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
                DocumentDB docdb = new DocumentDB();
                List<document> DocList = docdb.getDocuments();
                foreach (document doc in DocList)
                {
                    if (doc.DocumentStatus == 1)
                    {
                        cmb.Items.Add(doc.DocumentID);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }

        }
    }
}

