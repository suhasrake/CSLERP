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
    class proformainvoice
    {
        public string DocumentID { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string customername { get; set; }
        public double InvoiceAmt { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
    }

    class ProformaInvoiceDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<proformainvoice> getProformaDocuments()
        {
            proformainvoice documentrec;
            List<proformainvoice> Documents = new List<proformainvoice>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "Select DocumentID,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate from ProformaDocuments order by TemporaryNo";
                    
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    documentrec = new proformainvoice();
                    documentrec.DocumentID = reader.GetString(0);
                    documentrec.TemporaryNo = reader.GetInt32(1);
                    documentrec.TemporaryDate = reader.GetDateTime(2);
                    documentrec.DocumentNo = reader.GetInt32(3);
                    documentrec.DocumentDate = reader.GetDateTime(4);
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

        public static List<proformainvoice> getProformaInfo(int TempNo,string DOCID)
        {
            proformainvoice documentrec;
            List<proformainvoice> Documents = new List<proformainvoice>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "Select b.Name,a.InvoiceAmount,a.InvoiceNo,a.InvoiceDate,a.TemporaryDate from InvoiceOutHeader a,Customer b where b.CustomerID=a.Consignee and a.DocumentID='" + DOCID+"' and a.TemporaryNo='"+TempNo+"'";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    documentrec = new proformainvoice();
                    documentrec.customername = reader.GetString(0);
                    documentrec.InvoiceAmt = reader.GetDouble(1);
                    documentrec.DocumentNo = reader.GetInt32(2);
                    documentrec.DocumentDate = reader.GetDateTime(3);
                    documentrec.TemporaryDate = reader.GetDateTime(4);
                    Documents.Add(documentrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show( System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
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
        public Boolean insertDocument(proformainvoice doc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into ProformaDocuments (DocumentID,TemporaryNo,TemporaryDate,DocumentNo,DocumentDate,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + doc.DocumentID + "'," +
                    "'" + doc.TemporaryNo + "'," +
                    "'" + doc.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                     doc.DocumentNo + "," +
                   "'"+ doc.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "ProformaDocuments", "", updateSQL) +
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
        public Boolean validateDocument(proformainvoice doc)
        {
            Boolean status = true;
            try
            {
                if ( doc.DocumentNo == 0)
                {
                    return false;
                }
                if (doc.DocumentDate==DateTime.MinValue)
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

