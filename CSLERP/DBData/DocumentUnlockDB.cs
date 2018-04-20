using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class DocUnlock
    {
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public string ApprovedUser { get; set; }
        public string TableName { get; set; }
        public DateTime TemporaryDate { get; set; }
        public string forwarderlist { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
    }

    class DocumentUnlockDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<DocUnlock> getDocUnlockValues()
        {
            DocUnlock Docval;
            List<DocUnlock> DocValues = new List<DocUnlock>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string empLogedin = Login.userLoggedIn;
                string query = "select distinct a.DocumentID,a.DocumentName,a.TableName  from Document a , DocEmpMapping b ,DocumentUC c" +
                     " where a.DocumentID = b.DocumentID and" +
                      " a.IsReversible = 1 and b.SeniorEmployeeID = '" + Login.empLoggedIn + "' or "+
                      "c.UserID = (select distinct UserID from ViewUserEmployeeList a where a.EmployeeID = '" + Login.empLoggedIn + "' )"+
                      "and c.Status = 1 and a.TableName != 'NULL'and a.IsReversible = 1 order by a.DocumentName";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Docval = new DocUnlock();
                    Docval.DocumentID = reader.GetString(0);
                    Docval.DocumentName = reader.GetString(1);
                    Docval.TableName = reader.GetString(2);
                    DocValues.Add(Docval);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");

            }
            return DocValues;
        }
        public Boolean updateDocForUnlocking(DocUnlock du,int docStat,string docID)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update " + du.TableName + 
                    " set Status=96,ApproveUser=NULL,DocumentStatus= "+ docStat +
                     " where TemporaryNo = " + du.TemporaryNo +
                    " and TemporaryDate = '" + du.TemporaryDate.ToString("yyyy-MM-dd") + "' and DocumentID = '" + docID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("update", du.TableName, "", updateSQL) +
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
        public List<DocUnlock> getDocumentInfo(string tablename, string documentID)
        {
            DocUnlock tb;
            DocumentUnlockDB dbrecord = new DocumentUnlockDB();
            List<DocUnlock> tbdata = new List<DocUnlock>();
            try
            {

                SqlConnection conn = new SqlConnection(Login.connString);
                string empLogedin = Login.userLoggedIn;
                string query = "select DocumentID,TemporaryNo,TemporaryDate,ApproveUser,ForwarderList from " + tablename +
                    " where DocumentID = '" + documentID + "' and DocumentStatus = 99 and Status=1 order by TemporaryNo desc, TemporaryDate desc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tb = new DocUnlock();
                    tb.DocumentID = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    tb.TemporaryNo = reader.GetInt32(1);
                    tb.TemporaryDate = reader.GetDateTime(2);
                    tb.TableName = tablename;
                    tb.ApprovedUser = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    tb.forwarderlist = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    tbdata.Add(tb);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return tbdata;

        }
        public string getForwardlist(DocUnlock du)
        {
            string list = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string empLogedin = Login.userLoggedIn;
                string query = "select ForwarderList from " + du.TableName +
                    " where TemporaryNo = " + du.TemporaryNo +
                    " and TemporaryDate = '" + du.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    list = reader.GetString(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                list = null;
            }
            return list;
        }
        public Boolean updateDocForUnlockingMRN(DocUnlock du, int docStat)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update " + du.TableName +
                    " set Status = 96,ApproveUser=NULL,QCStatus = 0,DocumentStatus= " + docStat +
                     " where TemporaryNo = " + du.TemporaryNo +
                    " and TemporaryDate = '" + du.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("update", du.TableName, "", updateSQL) +
                 Main.QueryDelimiter;

                updateSQL = "Delete from Stock where InwardDocumentID ='MRN' and " +
                        " InwardDocumentNo = (select MRNNo from MRNHeader where TemporaryNo = " + du.TemporaryNo + " and TemporaryDate = '" + du.TemporaryDate.ToString("yyyy-MM-dd") + "') and" +
                        " InwardDocumentDate = (select MRNDate from MRNHeader where TemporaryNo = " + du.TemporaryNo + " and TemporaryDate = '" + du.TemporaryDate.ToString("yyyy-MM-dd") + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "Stock", "", updateSQL) +
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
        public Boolean updateDocForUnlockingGTN(DocUnlock du, int docStat)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update " + du.TableName +
                    " set Status = 96, AcceptedUser=NULL,ApproveUser = NULL,AcceptanceDate = '" + DateTime.Parse("1900-01-01").ToString("yyyy-MM-dd") +"'"+
                    ",AcceptanceStatus= 0, ReceiveStatus = 1,DocumentStatus= " + docStat +
                     " where TemporaryNo = " + du.TemporaryNo +
                    " and TemporaryDate = '" + du.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("update", du.TableName, "", updateSQL) +
                 Main.QueryDelimiter;

                //Get All Stock Details Prepared In GTN
                gtnheader gtnh = new gtnheader();
                gtnh.DocumentID = "GTN";
                gtnh.TemporaryNo = du.TemporaryNo;
                gtnh.TemporaryDate = du.TemporaryDate;
                List<gtndetail> gtnDet = GTNDB.getGTNDetail(gtnh);

                foreach(gtndetail gtnd in gtnDet)
                {
                    updateSQL = "update Stock set  " +
                         " PresentStock=" + "( (select PresentStock from Stock where RowID = " + gtnd.refNo + ")+" + gtnd.Quantity + ")" +
                        " where RowID=" + gtnd.refNo;
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
                    Main.QueryDelimiter;

                    updateSQL = "update Stock set  " +
                       " IssueQuantity=" + "( (select isnull(IssueQuantity,0) from Stock where RowID = " + gtnd.refNo + ")-" + gtnd.Quantity + ")" +
                       " where RowID=" + gtnd.refNo;
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
                    Main.QueryDelimiter;
                }

                updateSQL = "Delete from Stock where InwardDocumentID ='GTN' and " +
                        " InwardDocumentNo = (select GTNNo from GTNHeader where TemporaryNo = " + du.TemporaryNo + " and TemporaryDate = '" + du.TemporaryDate.ToString("yyyy-MM-dd") + "') and" +
                        " InwardDocumentDate = (select GTNDate from GTNHeader where TemporaryNo = " + du.TemporaryNo + " and TemporaryDate = '" + du.TemporaryDate.ToString("yyyy-MM-dd") + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "Stock", "", updateSQL) +
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
        public Boolean updateDocForUnlockingInvoiceOUT(DocUnlock du, int docStat,string docID)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update " + du.TableName +
                    " set Status = 96,ApproveUser=NULL,DocumentStatus= " + docStat +
                     " where TemporaryNo = " + du.TemporaryNo +
                    " and TemporaryDate = '" + du.TemporaryDate.ToString("yyyy-MM-dd") + "' and DocumentID = '" + docID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("update", du.TableName, "", updateSQL) +
                 Main.QueryDelimiter;
                InvoiceOutHeaderDB ionDB = new InvoiceOutHeaderDB();
                if (docID == "PRODUCTINVOICEOUT" || docID == "PRODUCTEXPORTINVOICEOUT")
                {
                    invoiceoutheader ioh = new invoiceoutheader();
                    ioh.DocumentID = docID;
                    ioh.TemporaryNo = du.TemporaryNo;
                    ioh.TemporaryDate = du.TemporaryDate;
                    List<invoiceoutdetail> IOList = InvoiceOutHeaderDB.getInvoiceOutDetail(ioh);
                    foreach (invoiceoutdetail iod in IOList)
                    {
                        updateSQL = "update stock set PresentStock = ((select PresentStock from Stock where RowID = " + iod.StockReferenceNo + ")+" + iod.Quantity + ")," +
                           " IssueQuantity =  ((select IssueQuantity from Stock where RowID = " + iod.StockReferenceNo + ")-" + iod.Quantity + ")" +
                           " where RowID = " + iod.StockReferenceNo;
                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                            ActivityLogDB.PrepareActivityLogQquerString("update", "Stock", "", updateSQL) +
                            Main.QueryDelimiter;
                    }
                }
                //Updating SJVHeader
                updateSQL = "update SJVHeader " +
                    " set Status = 96,ApproveUser=NULL,DocumentStatus= 1" + 
                     " where InvTempNo = " + du.TemporaryNo +
                    " and InvTempDate = '" + du.TemporaryDate.ToString("yyyy-MM-dd") + "' and InvDocumentID = '" + docID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("update", "SJVHeader", "", updateSQL) +
                 Main.QueryDelimiter;
                //return false;
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

        public static string getUnlockCommiteeListString()
        {
            string list = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string empLogedin = Login.userLoggedIn;
                string query = "select UserID from DocumentUC where Status = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list = list +";"+ reader.GetString(0) + ";";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                list = null;
            }
            return list;
        }

        public Boolean updateDocForUnlockingInvoiceIN(DocUnlock du, int docStat, string docID)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update " + du.TableName +
                    " set Status = 96,ApproveUser=NULL,DocumentStatus= " + docStat +
                     " where TemporaryNo = " + du.TemporaryNo +
                    " and TemporaryDate = '" + du.TemporaryDate.ToString("yyyy-MM-dd") + "' and DocumentID = '" + docID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("update", du.TableName, "", updateSQL) +
                 Main.QueryDelimiter;
               
                //Updating PJVHeader
                updateSQL = "update PJVHeader " +
                    " set Status = 96,ApproveUser=NULL,DocumentStatus= 1" +
                     " where InvTempNo = " + du.TemporaryNo +
                    " and InvTempDate = '" + du.TemporaryDate.ToString("yyyy-MM-dd") + "' and InvDocumentID = '" + docID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("update", "PJVHeader", "", updateSQL) +
                 Main.QueryDelimiter;
                //return false;
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


