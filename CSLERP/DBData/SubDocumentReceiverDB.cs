using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    public class subdocreceiver
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string SubDocID { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }

    class SubDocumentReceiverDB
    {
        public List<subdocreceiver> getsubdocreceiverList()
        {
            subdocreceiver drrec;
            List<subdocreceiver> subdocreceivers = new List<subdocreceiver>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.DocumentID, b.DocumentName,a.EmployeeID,c.Name,"+
                    "a.status,a.RowId,a.SubDocumentID from SubDocumentReceivers a, Document b, " +
                    "Employee c where a.DocumentID=b.DocumentID and "+
                    "a.EmployeeID=c.EmployeeID "+
                    "order by c.Name";
                    
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    drrec = new subdocreceiver();
                    drrec.DocumentID = reader.GetString(0);
                    drrec.DocumentName = reader.GetString(1);
                    drrec.EmployeeID = reader.GetString(2);
                    drrec.EmployeeName = reader.GetString(3);
                    drrec.Status = reader.GetInt32(4);
                    drrec.RowID = reader.GetInt32(5);
                    drrec.SubDocID = reader.GetString(6);
                    subdocreceivers.Add(drrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return subdocreceivers;

        }
      
        public Boolean updateSubdocReceiver(subdocreceiver doc, subdocreceiver prevdoc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update SubDocumentReceivers set EmployeeID='" + doc.EmployeeID + "',"+
                    " SubDocumentID= '" + doc.SubDocID + "',"+
                    " Status=" + doc.Status +
                    " where RowID=" + prevdoc.RowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "SubDocumentReceivers", "", updateSQL) +
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
        public Boolean insertSubDocumentReceivers(subdocreceiver doc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into SubDocumentReceivers (DocumentID,SubDocumentID,EmployeeID,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + doc.DocumentID + "'," +
                    "'" + doc.SubDocID + "'," +
                    "'" + doc.EmployeeID + "'," +
                    doc.Status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "SubDocumentReceivers", "", updateSQL) +
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
        public Boolean validateDocument(subdocreceiver doc)
        {
            Boolean status = true;
            try
            {
                if (doc.DocumentID.Trim().Length == 0 || doc.DocumentID == null)
                {
                    return false;
                }
                if (doc.SubDocID.Trim().Length == 0 || doc.SubDocID == null)
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

        //public string getTrackers(string empID)
        //{
        //    string docList = "";
        //    string officeList = "";
        //    string trackerList = "";
        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "select a.EmployeeID, b.userID,a.DocumentID,a.OfficeID from subdocreceiver a, ERPUser b  " +
        //            "where a.EmployeeID = b.EmployeeID " + 
        //            " and a.EMployeeID='" + empID + "' and a.Status = 1";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            if (docList.Length > 0)
        //            {
        //                docList = docList + ",";
        //            }
        //            docList = docList + "'" + reader.GetString(2) + "'";

        //            if (officeList.Length > 0)
        //            {
        //                officeList = officeList + ",";
        //            }
        //            officeList = officeList + "'" + reader.GetString(3) + "'";

        //            trackerList = trackerList + "'" + reader.GetString(1) + "'";
        //        }
        //        conn.Close();
        //        trackerList = docList + ";" + officeList;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        //        trackerList = "";
        //    }
        //    return trackerList;
        //}
        //public static List<subdocreceiver> getDocumentWiseReceiver(string docID)
        //{
        //    subdocreceiver drrec;
        //    List<subdocreceiver> subdocreceivers = new List<subdocreceiver>();
        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "select DocumentID, DocumentName,UserID," +
        //            "status from Viewsubdocreceiver " +
        //            "where DocumentID='" + docID + "' and Status = 1";

        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            drrec = new subdocreceiver();
        //            drrec.DocumentID = reader.GetString(0);
        //            drrec.DocumentName = reader.GetString(1);
        //            drrec.EmployeeID = reader.GetString(2); // for retriving userID
        //            drrec.Status = reader.GetInt32(3);
        //            subdocreceivers.Add(drrec);
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        //    }
        //    return subdocreceivers;

        //}
        public static List<string> getReceiverWiseSubDocumnets(string employeeId, string docID)
        {
            List<string> subDocList = new List<string>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select SubDocumentID from SubDocumentReceivers " +
                    "where DocumentID='" + docID + "' and EmployeeID = '" + employeeId + "' and Status = 1";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    subDocList.Add(reader.GetString(0));
                }
                if (subDocList.Count == 0)
                {
                    subDocList = new List<string>();
                    SqlConnection connemp = new SqlConnection(Login.connString);
                    string queryempty = " select cataloguevalueid from cataloguevalue  where CatalogueID='SEFType' and status=1";

                    SqlCommand cmdemp = new SqlCommand(queryempty, connemp);
                    connemp.Open();
                    SqlDataReader readeremp = cmdemp.ExecuteReader();
                    while (readeremp.Read())
                    {
                        subDocList.Add(readeremp.GetString(0));
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return subDocList;

        }
        public static List<string> getEmpWiseSubDocList(string employeeId, string docID)
        {
            List<string> subDocList = new List<string>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select SubDocumentID from SubDocumentReceivers " +
                    "where DocumentID='" + docID + "' and EmployeeID = '" + employeeId + "' and Status = 1";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    subDocList.Add(reader.GetString(0));
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return subDocList;

        }

        
    }
}

