using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    public class documentreceiver
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string OfficeID { get; set; }
        public string OfficeName { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }

    class DocumentReceiverDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<documentreceiver> getDocumentReceiver()
        {
            documentreceiver drrec;
            List<documentreceiver> DocumentReceivers = new List<documentreceiver>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.DocumentID, b.DocumentName,a.EmployeeID,c.Name, a.officeid,"+
                    "d.Name, a.status,a.RowId from DocumentReceiver a, Document b, " +
                    "Employee c, Office d where a.DocumentID=b.DocumentID and "+
                    "a.EmployeeID=c.EmployeeID and a.OfficeID=d.OfficeID "+
                    "order by a.DocumentID,c.Name";
                    
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    drrec = new documentreceiver();
                    drrec.DocumentID = reader.GetString(0);
                    drrec.DocumentName = reader.GetString(1);
                    drrec.EmployeeID = reader.GetString(2);
                    drrec.EmployeeName = reader.GetString(3);
                    drrec.OfficeID = reader.GetString(4);
                    drrec.OfficeName = reader.GetString(5);
                    drrec.Status = reader.GetInt32(6);
                    drrec.RowID = reader.GetInt32(7);
                    DocumentReceivers.Add(drrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return DocumentReceivers;

        }
      
        public Boolean updateDocumentReceiver(documentreceiver doc, documentreceiver prevdoc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update DocumentReceiver set EmployeeID='" + doc.EmployeeID + "',"+
                    "OfficeID='"+doc.OfficeID+"',"+
                    " Status=" + doc.Status +
                    " where RowID=" + prevdoc.RowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentReceiver", "", updateSQL) +
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
        public Boolean insertDocumentReceiver(documentreceiver doc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into DocumentReceiver (DocumentID,EmployeeID,OfficeID,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + doc.DocumentID + "'," +
                    "'" + doc.EmployeeID + "'," +
                    "'" + doc.OfficeID + "'," +
                    doc.Status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "DocumentReceiver", "", updateSQL) +
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
        public Boolean validateDocument(documentreceiver doc)
        {
            Boolean status = true;
            try
            {
                if (doc.DocumentID.Trim().Length == 0 || doc.DocumentID == null)
                {
                    return false;
                }
                if (doc.EmployeeID.Trim().Length == 0 || doc.EmployeeID == null)
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

        public string getTrackers(string empID)
        {
            string docList = "";
            string officeList = "";
            string trackerList = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.EmployeeID, b.userID,a.DocumentID,a.OfficeID from DocumentReceiver a, ERPUser b  " +
                    "where a.EmployeeID = b.EmployeeID " + 
                    " and a.EMployeeID='" + empID + "' and a.Status = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (docList.Length > 0)
                    {
                        docList = docList + ",";
                    }
                    docList = docList + "'" + reader.GetString(2) + "'";

                    if (officeList.Length > 0)
                    {
                        officeList = officeList + ",";
                    }
                    officeList = officeList + "'" + reader.GetString(3) + "'";

                    trackerList = trackerList + "'" + reader.GetString(1) + "'";
                }
                conn.Close();
                trackerList = docList + ";" + officeList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                trackerList = "";
            }
            return trackerList;
        }
        public static List<documentreceiver> getDocumentWiseReceiver(string docID)
        {
            documentreceiver drrec;
            List<documentreceiver> DocumentReceivers = new List<documentreceiver>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID, DocumentName,UserID," +
                    "status from ViewDocumentReceiver " +
                    "where DocumentID='" + docID + "' and Status = 1";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    drrec = new documentreceiver();
                    drrec.DocumentID = reader.GetString(0);
                    drrec.DocumentName = reader.GetString(1);
                    drrec.EmployeeID = reader.GetString(2); // for retriving userID
                    drrec.Status = reader.GetInt32(3);
                    DocumentReceivers.Add(drrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return DocumentReceivers;

        }

       
    }
}

