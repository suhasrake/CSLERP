using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;

namespace CSLERP.DBData
{
    //class tapalDistribution
    //{
    //    public int RowID { get; set; }
    //    public int TapalReference { get; set; }
    //    public string DocumentID { get; set; }
    //    public string ReceivedFrom { get; set; }
    //    public string InwardDocumentType { get; set; }
    //    public string FileName { get; set; }
    //    public DateTime CreateDate { get; set; }
    //    public DateTime ForwardDate { get; set; }
    //    public string ReceiveUser { get; set; }
    //    public string Receiver { get; set; }
    //    public int ProtectionLevel { get; set; }
    //    public string FileType { get; set; }

    //    public int CreateStatus { get; set; }
    //    public int ForwardStatus { get; set; }
       
    //    //public string DocumentContent { get; set; }
    //   // public int Status { get; set; }
    //    public DateTime CreateTime { get; set; }
    //    public DateTime ForwrdTime { get; set; }
    //    public string CreateUser { get; set; }
    //    public string ForwardUser { get; set; }
    //    //public string Creator { get; set; }
    //}
    class tapalDistribution
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public int TapalReference { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
        public string ReceivedFrom { get; set; }
        public string InwardDocumentType { get; set; }
        public string FileName { get; set; }
        public string DocumentContent { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public string DistributeUser { get; set; }
        public string Distributor { get; set; }
        public string CreateUser { get; set; }
        public string Creator { get; set; }
        public DateTime DistibuteDate { get; set; }   // used in only summary
        public int ActionStatus { get; set; }
        public int ActionReferenceID { get; set; }
        public string Description { get; set; }
    }
    class tapal
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string FileName { get; set; }
        public string InwardDocumentType { get; set; }
        public string ReceivedFrom { get; set; }
        public string DocumentContent { get; set; }
        public int ProtectionLevel { get; set; }
        public string FileType { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }
    class TapalPickDB
    {
        public List<tapal> getTapalPicFileDetails()
        {
            tapal tap;
            List<tapal> List = new List<tapal>();
            try
            {
                string query = "select RowID,DocumentID, Date, FileName, InwardDocumentType,ReceivedFrom,"+
                    " ProtectionLevel, FileType, Status , CreateUser, CreateTime,Description " +
                    " from TapalStorage where Status = 0 and CreateUser = '" + Login.userLoggedIn + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tap = new tapal();
                    tap.RowID = reader.GetInt32(0);
                    tap.DocumentID = reader.GetString(1);
                    tap.Date = reader.GetDateTime(2);
                    tap.FileName = reader.GetString(3);
                    tap.InwardDocumentType = reader.GetString(4);
                    tap.ReceivedFrom = reader.GetString(5);
                    tap.ProtectionLevel = reader.GetInt32(6);
                    tap.FileType = reader.GetString(7);
                    tap.Status = reader.GetInt32(8);
                    tap.CreateUser = reader.GetString(9);
                    tap.CreateTime = reader.GetDateTime(10);
                    tap.Description = reader.IsDBNull(11)?"":reader.GetString(11);
                    List.Add(tap);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return List;
        }
        public Boolean inserttapal(tapal tap)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                 string updateSQL = "insert into TapalStorage " +
                    "(DocumentID, Date, FileName, InwardDocumentType,ReceivedFrom,Description, DocumentContent,ProtectionLevel,"+
                    " FileType, Status ,CreateTime,CreateUser) " +
                    "values ('" + tap.DocumentID + "','" 
                    + tap.Date.ToString("yyyy-MM-dd") + "','" +
                    tap.FileName + "','" +
                     tap.InwardDocumentType + "','" +
                      tap.ReceivedFrom.Replace("'","''") + "','" +
                      tap.Description.Replace("'", "''") + "','" +
                    tap.DocumentContent + "'," +
                     tap.ProtectionLevel + ",'" +
                    tap.FileType + "'," +
                     tap.Status + "," +
                    "GETDATE()" + ",'" +
                    Login.userLoggedIn + "')";
                tap.DocumentContent = "";

                if (!UpdateTable.UTSingleQuery(updateSQL))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        public Boolean validateTapal(tapal tap)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                if (tap.FileName.Trim().Length == 0 || tap.FileName == null)
                {
                    return false;
                }
                if (tap.InwardDocumentType.Trim().Length == 0 || tap.InwardDocumentType == null)
                {
                    return false;
                }
                if (tap.ReceivedFrom.Trim().Length == 0 || tap.ReceivedFrom == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {

            }
            return status;
        }
        public static Boolean deleteTapal(int rowid)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "delete TapalStorage where RowID=" + rowid;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("delete", "TapalStorage", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        public static string getFileFromDB(int rowID, string fileName)
        {
            string filepath = "";
            try
            {
                filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Downloads";
                if (!System.IO.Directory.Exists(filepath))
                {
                    System.IO.Directory.CreateDirectory(filepath);
                }
                filepath = filepath + "\\" + fileName;
                try
                {
                    SqlConnection conn = new SqlConnection(Login.connString);
                    string query = "select DocumentContent " +
                        " from TapalStorage " +
                        "where RowID=" + rowID ;
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        byte[] strByte = Convert.FromBase64String(reader.GetString(0));
                        File.WriteAllBytes(filepath, strByte);
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving Document");
                }
            }
            catch (Exception)
            {
            }
            return filepath;
        }
        public Boolean updateTapalStatus(tapal tap)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update TapalStorage set Status = 1"+
                   " where RowID=" + tap.RowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "TapalStorage", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                status = false;
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return status;
        }
        public Boolean ChangeTapalStatus(int rowID, int tapalStatus)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                //if tapalstatus=2 and existing status is not 3, then change status to 4
                if (tapalStatus==2)
                {
                    //for deletion
                    if (getPresenetStatus(rowID) != 3)
                    {
                        //deletion without viewing
                        tapalStatus = 4;
                    }
                }
                string updateSQL = "update TapalDistribution set Status = " + tapalStatus +
                   " where RowID=" + rowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "TapalDistribution", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                status = false;
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return status;
        }
        public Boolean insertTapalDistribution(List<user> UserList, tapal tap)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "";
                //string updateSQL = "Delete from PODetail where DocumentID='" + poh.DocumentID + "'" +
                //    " and TemporaryNo=" + poh.TemporaryNo +
                //    " and TemporaryDate='" + poh.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                //utString = utString + updateSQL + Main.QueryDelimiter;
                //utString = utString +
                //    ActivityLogDB.PrepareActivityLogQquerString("delete", "PODetail", "", updateSQL) +
                //    Main.QueryDelimiter;
                foreach (user us in UserList)
                {
                    updateSQL = "insert into TapalDistribution " +
                   "(DocumentID, TapalReference, UserID, Date, Status,CreateTime,CreateUser) " +
                   "values ('" + tap.DocumentID + "',"
                   + tap.RowID + ",'" +
                   us.userID + "','" +
                  tap.Date.ToString("yyyy-MM-dd") + "'," + 
                    tap.Status + "," +
                   "GETDATE()" + ",'" +
                   Login.userLoggedIn + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "TapalDistribution", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        public List<tapalDistribution> getTapalListInDashBoard(string logInUserID)
        {
            tapalDistribution tapd;
            List<tapalDistribution> List = new List<tapalDistribution>();
            try
            {
                string query = "select a.RowID,a.DocumentID,a.Date,d.FileName,a.UserID,d.ReceivedFrom,d.InwardDocumentType,a.CreateUser,c.name ,a.TapalReference,d.Description " +
                    " from TapalDistribution a , ERPUser b, Employee c, TapalStorage d " +
                    " where a.CreateUser = b.UserID and b.EmployeeID = c.EmployeeID and a.TapalReference = d.RowID " +
                    "and a.UserID = '" + logInUserID + "' and (a.Status = 0 or a.Status=3) order by a.Date desc";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tapd = new tapalDistribution();
                    tapd.RowID = reader.GetInt32(0);
                    tapd.DocumentID = reader.GetString(1);
                    tapd.Date = reader.GetDateTime(2);
                    tapd.FileName = reader.GetString(3);
                    tapd.UserID = reader.GetString(4);
                    tapd.ReceivedFrom = reader.GetString(5);
                    tapd.InwardDocumentType = reader.GetString(6);
                    tapd.CreateUser = reader.GetString(7);
                    tapd.Creator = reader.GetString(8);
                    tapd.TapalReference = reader.GetInt32(9);
                    tapd.Description = reader.IsDBNull(10)?"":reader.GetString(10);
                    List.Add(tapd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return List;
        }
        public Boolean MoveTapal(string userID, tapalDistribution tap)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update TapalDistribution set Status = 1" +
                   " where RowID=" + tap.RowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "TapalDistribution", "", updateSQL) +
                Main.QueryDelimiter;
                updateSQL = updateSQL = "insert into TapalDistribution " +
                   "(DocumentID, TapalReference, UserID, Date, Status,CreateTime,CreateUser) " +
                   "values ('" + tap.DocumentID + "',"
                   + tap.TapalReference + ",'" +
                   userID + "','" +
                  tap.Date.ToString("yyyy-MM-dd") + "'," +
                    tap.Status + "," +
                   "GETDATE()" + ",'" +
                   Login.userLoggedIn + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "TapalDistribution", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                status = false;
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return status;
        }


        // Tapal Summary

        public List<tapalDistribution> getTapalListSummaryForADate(DateTime dt)
        {
            tapalDistribution tapd;
            List<tapalDistribution> List = new List<tapalDistribution>();
            try
            {
                string query = "select a.RowID,a.Date,b.Date, a.FileName, a.InwardDocumentType,a.ReceivedFrom,a.Status ,a.CreateUser,f.Name,"+
                    "b.CreateUser,h.Name,b.UserID ,d.Name,b.CreateTime,b.Status,b.RowID,a.Description" +
                    " from TapalStorage  AS a LEFT OUTER JOIN  " +
                    " TapalDistribution AS b ON a.RowID = b.TapalReference  LEFT OUTER JOIN " +
                    " ERPUser AS c on b.UserID = c.UserID LEFT OUTER JOIN "+
                    " Employee AS d on c.EmployeeID = d.EmployeeID LEFT OUTER JOIN "+
                    " ERPUser AS e on a.CreateUser = e.UserID LEFT OUTER JOIN "+
                    " Employee AS f on e.EmployeeID = f.EmployeeID LEFT OUTER JOIN" +
                    "  ERPUser AS g on b.CreateUser = g.UserID LEFT OUTER JOIN "+
                    "Employee AS h on g.EmployeeID = h.EmployeeID"  +
                    " where a.Date = '" + dt.ToString("yyyy-MM-dd") + "' order by a.RowID";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tapd = new tapalDistribution();
                    tapd.RowID = reader.GetInt32(0);
                    tapd.Date =  reader.GetDateTime(1);
                    tapd.DistibuteDate = reader.IsDBNull(2)?DateTime.Parse("01-01-1900") : reader.GetDateTime(2);
                    tapd.FileName = reader.GetString(3);
                    tapd.InwardDocumentType = reader.GetString(4);
                    tapd.ReceivedFrom = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    tapd.Status = reader.GetInt32(6);
                    tapd.CreateUser = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    tapd.Creator = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    tapd.DistributeUser = reader.IsDBNull(9) ? "" : reader.GetString(9);
                    tapd.Distributor = reader.IsDBNull(10) ? "" : reader.GetString(10);
                    tapd.UserID = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    tapd.UserName = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    tapd.CreateTime = reader.IsDBNull(13) ? DateTime.Parse("01-01-1900") : reader.GetDateTime(13);
                    tapd.ActionStatus = reader.IsDBNull(14) ? -1 : reader.GetInt32(14);
                    tapd.ActionReferenceID = reader.IsDBNull(15) ? -1 : reader.GetInt32(15);
                    tapd.Description = reader.IsDBNull(16) ? "" : reader.GetString(16);
                    List.Add(tapd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return List;
        }
        public List<tapalDistribution> getTapalListDateWiseSummary()
        {
            tapalDistribution taps;
            List<tapalDistribution> tpsList = new List<tapalDistribution>();
            try
            {
                string query = "select Date,Forwarded,Pending " +
                    " from ViewTapalSummary order by Date desc " ;
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    taps = new tapalDistribution();
                    taps.Date = reader.GetDateTime(0);
                    taps.RowID = reader.IsDBNull(1)? 0 : reader.GetInt32(1);
                    taps.TapalReference = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                    tpsList.Add(taps);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                 MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return tpsList;
        }
        public static String getMoveReceipentName(int rowid, int referenceno)
        {
            tapalDistribution taps;
            List<tapalDistribution> tpsList = new List<tapalDistribution>();
            try
            {

                string query = "select top 1 a.userid,b.name from TapalDistribution a, ViewUserEmployeeList b " +
                    " where TapalReference = " + referenceno +" and rowid > " + rowid + " and a.userid = b.UserID ";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    return reader.GetString(1);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("getMoveReceipentName()" + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return "";
        }

        public static int getPresenetStatus(int rowid)
        {
            tapalDistribution taps;
            List<tapalDistribution> tpsList = new List<tapalDistribution>();
            try
            {

                string query = " select status from TapalDistribution   where  rowid = " + rowid;
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    return reader.GetInt32(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("getMoveReceipentName()" + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return -1;
        }

        public static void fillInwardDocCombo(System.Windows.Forms.ComboBox cmb, string catalogvalue)
        {
            cmb.Items.Clear();
            try
            {
                CatalogueValueDB dbrecord = new CatalogueValueDB();
                List<cataloguevalue> CatalogueValues = dbrecord.getCatalogueValues();
                foreach (cataloguevalue catval in CatalogueValues)
                {
                    if (catval.catalogueID.Equals(catalogvalue) && catval.status == 1)
                    {
                        cmb.Items.Add(catval.catalogueValueID);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("fillCatalogValueCombo() : Error");
            }

        }
        public Boolean updateTapalDetails(tapal tap, int rowid)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update TapalStorage set" +
                    " InwardDocumentType='" + tap.InwardDocumentType + "', " +
                    "ReceivedFrom='" + tap.ReceivedFrom + "'," +
                    " Description='" + tap.Description + "' " +
                    "where RowID='" + rowid + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "AccoutCode", "", updateSQL) +
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
