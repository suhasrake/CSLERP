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
    class documentnumber
    {
        public string fyID { get; set; }
        public string DocumentID { get; set; }
        public int TemporaryNo { get; set; }
        public int DocumentNo { get; set; }
        public string DocumentName { get; set; }
        public int rowid { get; set; }
    }

    class DocumentNumberDB
    {
        public static int getNewNumber(string documentID, int opt)
        {
            int newNumber = 0;
            string utString = "";
            try
            {
                //increment number
                string updateSQL = "";
                string columnName = "";
                if (opt == 1)
                {
                    columnName = "TempNo";
                }
                else if (opt == 2)
                {
                    columnName = "DocumentNo";
                }
                string querySQL = "select " + columnName + " from DocumentNumber " +
                       " where FYID='" + Main.currentFY + "' and DocumentID='" + documentID + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(querySQL, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    updateSQL = "insert into DocumentNumber (FYID, DocumentID,TempNo,DocumentNo,CreateTime,CreateUser)" +
                         " values (" +
                         "'"+Main.currentFY+"',"+
                         "'" + documentID + "'," +
                         "0,0,"+
                         "GETDATE()" + "," +
                         "'" + Login.userLoggedIn + "'" + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                       ActivityLogDB.PrepareActivityLogQquerString("insert", "DocumentNumber", "", updateSQL) +
                       Main.QueryDelimiter;
                }
                updateSQL = "update DocumentNumber set " + columnName + "=" + columnName + "+1" +
                        " where FYID='" + Main.currentFY + "' and DocumentID='" + documentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    return 0;
                }
                //query new number
                querySQL = "select " + columnName + " from DocumentNumber " +
                        " where FYID='" + Main.currentFY + "' and DocumentID='" + documentID + "'";
                conn = new SqlConnection(Login.connString);
                cmd = new SqlCommand(querySQL, conn);
                conn.Open();
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    newNumber = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                newNumber = 0;
            }
            return newNumber;
        }
        public static int getNumber(string documentID, int opt)
        {
            int no = 0;
            string columnName = "";
            string updateSQL = "";
            string utString = "";
            try
            {

                if (opt == 1)
                {
                    columnName = "TempNo";
                }
                else if (opt == 2)
                {
                    columnName = "DocumentNo";
                }
                string querySQL = "select " + columnName + " from DocumentNumber " +
                              " where FYID='" + Main.currentFY + "' and DocumentID='" + documentID + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(querySQL, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (!reader.Read())
                {
                    updateSQL = "insert into DocumentNumber (FYID, DocumentID,TempNo,DocumentNo,CreateTime,CreateUser)" +
                         " values (" +
                         "'" + Main.currentFY + "'," +
                         "'" + documentID + "'," +
                         "0,0," +
                         "GETDATE()" + "," +
                         "'" + Login.userLoggedIn + "'" + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                       ActivityLogDB.PrepareActivityLogQquerString("insert", "DocumentNumber", "", updateSQL) +
                       Main.QueryDelimiter;
                    if (!UpdateTable.UT(utString))
                    {
                        conn.Close();
                        conn.Dispose();
                        return -1;
                    }
                    else
                    {
                        conn.Close();
                        conn.Dispose();
                        no = 0;
                    }
                }
                else
                    no = reader.GetInt32(0);
            }
            catch (Exception ex)
            {
                return -1;
            }
            return no + 1;
        }

        //01-03-2018
        public List<documentnumber> getDocumentNo()
        {
            documentnumber documentrec;
            List<documentnumber> Documents = new List<documentnumber>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.RowID,a.FYID,a.DocumentID, b.DocumentName,a.TempNo,a.DocumentNo " +
                               " from DocumentNumber a ,Document b " +
                               " where a.DocumentID = b.DocumentID order by b.DocumentName";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    documentrec = new documentnumber();
                    documentrec.rowid = reader.GetInt32(0);
                    documentrec.fyID = reader.GetString(1);
                    documentrec.DocumentID = reader.GetString(2);
                    documentrec.DocumentName = reader.GetString(3);
                    documentrec.TemporaryNo = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
                    documentrec.DocumentNo = reader.GetInt32(5);
                    Documents.Add(documentrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return Documents;

        }

        public Boolean validateDocumentNo(documentnumber doc)
        {
            Boolean status = true;
            try
            {
                if (doc.DocumentID.Trim().Length == 0 || doc.DocumentID == null)
                {
                    return false;
                }
                if (doc.fyID.Trim().Length == 0 || doc.fyID == null)
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

        public Boolean insertDocument(documentnumber doc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into DocumentNumber (DocumentID,FYID,TempNo,DocumentNo,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + doc.DocumentID + "'," +
                    "'" + doc.fyID + "'," +
                    "'" + doc.TemporaryNo + "'," +
                     doc.DocumentNo + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "DocumentNumber", "", updateSQL) +
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

        public Boolean updateDocumentnumber(documentnumber doc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update DocumentNumber set TempNo=" + doc.TemporaryNo + "," +
                    " DocumentNo = " + doc.DocumentNo +
                    " where RowID='" + doc.rowid + "'  and DocumentID='" + doc.DocumentID + "' ";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
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
    }
}
