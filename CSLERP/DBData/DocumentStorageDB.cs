using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace CSLERP.DBData
{

    class documentStorage
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string DocumentSubID { get; set; }
        public string Directory { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string DocumentContent { get; set; }
        public int ProtectionLevel { get; set; }
        public string FileType { get; set; }
        public string LastUploadedUser { get; set; }
        public DateTime LastUploadedTime { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
    }
    class DocumentStorageDB
    {
        public Boolean InsertDocumentDetails(documentStorage ds)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into DocumentStorage " +
                    "(DocumentID,DocumentSubID,Directory,FileName,Description,DocumentContent,ProtectionLevel,FileType,LastUploadedUser,LastUploadedTime,CreateUser,CreateTime)" +
                    " values (" +
                    "'" + ds.DocumentID + "'," +
                    "'" + ds.DocumentSubID + "'," +
                    "'" + ds.Directory + "'," +
                    "'" + ds.FileName + "'," +
                    "'" + ds.Description + "'," +
                    "'" + ds.DocumentContent + "'," +
                    ds.ProtectionLevel + "," +
                    "'" + ds.FileType + "'," +
                    "'" + Login.userLoggedIn + "'," + "GETDATE()," +
                    "'" + Login.userLoggedIn + "'," + "GETDATE()" + ")";
                ds.DocumentContent = "";
                //utString = utString + updateSQL + Main.QueryDelimiter;
                //updateSQL = "";
                ////utString = utString +
                ////ActivityLogDB.PrepareActivityLogQquerString("insert", "DocumentStorage", "", updateSQL) +
                ////Main.QueryDelimiter;
                if (!UpdateTable.UTSingleQuery(updateSQL))
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
        public Boolean UpdateDocumentDetails(documentStorage ds)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update DocumentStorage set Description='" + ds.Description +
                    "', DocumentContent='" + ds.DocumentContent + "'" +
                    ", LastUpLoadedUser='" + Login.userLoggedIn + "'" +
                    ", LastUpLoadedTime=" + "GETDATE()" +
                    " where DocumentID='" + ds.DocumentID + "' and DocumentSubID='" + ds.DocumentSubID + "' and FileName='" + ds.FileName + "'";
                ////utString = utString + updateSQL + Main.QueryDelimiter;
                ds.DocumentContent = "";
                ////utString = utString +
                //// ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentStorage", "", updateSQL) +
                //// Main.QueryDelimiter;
                if (!UpdateTable.UTSingleQuery(updateSQL))
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
        public Boolean UpdateDocumentDescription(documentStorage ds)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update DocumentStorage set Description='" + ds.Description +
                    "', LastUpLoadedUser='" + Login.userLoggedIn + "'" +
                    ", LastUpLoadedTime=" + "GETDATE()" +
                    " where DocumentID='" + ds.DocumentID + "' and DocumentSubID='" + ds.DocumentSubID + "' and FileName='" + ds.FileName + "'";
                ////utString = utString + updateSQL + Main.QueryDelimiter;
                ds.DocumentContent = "";
                ////utString = utString +
                //// ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentStorage", "", updateSQL) +
                //// Main.QueryDelimiter;
                if (!UpdateTable.UTSingleQuery(updateSQL))
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
        public Boolean iskDocumentDuplication(documentStorage ds)
        {
            Boolean status = false;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID,DocumentSubId,FileName " +
                    "from DocumentStorage a where DocumentID='" + ds.DocumentID + "' and DocumentSubID='" + ds.DocumentSubID + "' and FileName='" + ds.FileName + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    status = true;
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = true;
            }
            return status;
        }

        //--
        public List<documentStorage> getDetailsFromDB(string DocId, string dir)
        {
            documentStorage ds;
            List<documentStorage> docList = new List<documentStorage>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DocumentID,DocumentSUbID,FileName,Description ,ProtectionLevel,FileType,CreateUserName,CreateTime,LastUploadedUserName,LastUploadedTime,RowID " +
                    " from ViewDocumentStorage " +
                    "where DocumentID='" + DocId + "' and DocumentSubID='" + dir + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ds = new documentStorage();
                    ds.DocumentID = reader.GetString(0);
                    ds.DocumentSubID = reader.GetString(1);
                    ds.FileName = reader.GetString(2);
                    ds.Description = reader.GetString(3);
                    ds.ProtectionLevel = reader.GetInt32(4);
                    ds.FileType = reader.GetString(5);
                    ds.CreateUser = reader.GetString(6);
                    ds.CreateTime = reader.GetDateTime(7);
                    ds.LastUploadedUser = reader.IsDBNull(8) ? "" : reader.GetString(8);

                    try
                    {
                        ds.LastUploadedTime = reader.GetDateTime(9);
                    }
                    catch (Exception)
                    {

                    }
                    ds.RowID = reader.GetInt32(10);
                    docList.Add(ds);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return docList;
        }
        public static DataGridView getDocumentDetails(string docId, string dir)
        {
            DataGridView dgv = new DataGridView();
            try
            {
                DocumentStorageDB ds = new DocumentStorageDB();
                List<documentStorage> dsList = ds.getDetailsFromDB(docId, dir);
                dgv.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font(dgv.Font, FontStyle.Bold);
                dgv.Location = new Point(50, 8);
                dgv.Size = new Size(610, 250);
                dgv.RowHeadersVisible = false;
                //0
                DataGridViewLinkColumn dgvlc = new DataGridViewLinkColumn();
                dgvlc.SortMode = DataGridViewColumnSortMode.Automatic;
                dgv.Columns.Add(dgvlc);
                //1
                DataGridViewTextBoxColumn dgvtx = new DataGridViewTextBoxColumn();
                dgv.Columns.Add(dgvtx);
                //2
                DataGridViewTextBoxColumn dgvtx1 = new DataGridViewTextBoxColumn();
                dgv.Columns.Add(dgvtx1);
                //3
                DataGridViewTextBoxColumn dgvtx2 = new DataGridViewTextBoxColumn();
                dgv.Columns.Add(dgvtx2);
                //4
                DataGridViewButtonColumn dgEditvbtn = new DataGridViewButtonColumn();
                dgEditvbtn.UseColumnTextForButtonValue = true;
                dgEditvbtn.Text = "Edit";
                dgv.Columns.Add(dgEditvbtn);
                //5
                DataGridViewButtonColumn dgvDelbtn = new DataGridViewButtonColumn();
                dgvDelbtn.UseColumnTextForButtonValue = true;
                dgvDelbtn.Text = "Del";
                dgv.Columns.Add(dgvDelbtn);
                //6
                DataGridViewTextBoxColumn dgvtx3 = new DataGridViewTextBoxColumn();
                dgv.Columns.Add(dgvtx3);
                dgvtx3.Visible = false;
                //7
                DataGridViewTextBoxColumn dgvtx4 = new DataGridViewTextBoxColumn();
                dgv.Columns.Add(dgvtx4);
                dgvtx4.Visible = false;
                //8
                DataGridViewTextBoxColumn dgvColRowID = new DataGridViewTextBoxColumn();
                dgv.Columns.Add(dgvColRowID);
                dgvColRowID.Visible = false;

                dgv.Columns[0].Width = 160;
                dgv.Columns[1].Width = 280;
                dgv.Columns[2].Width = 130;
                dgv.Columns[3].Width = 140;
                dgv.Columns[4].Width = 70;
                dgv.Columns[5].Width = 70;

                dgv.Columns[0].Name = "FileName";
                dgv.Columns[1].Name = "Description";
                dgv.Columns[2].Name = "LastUpdatedUser";
                dgv.Columns[3].Name = "LastUpdatedTime";
                dgv.Columns[4].Name = "Edit";
                dgv.Columns[5].Name = "Delete";
                dgv.Columns[6].Name = "docID";
                dgv.Columns[7].Name = "docsubID";

                dgv.Columns[8].Name = "RowID";

                dgv.Columns[0].HeaderText = "File Name";
                dgv.Columns[1].HeaderText = "Description";
                dgv.Columns[2].HeaderText = "Last Updated by";
                dgv.Columns[3].HeaderText = "Last Updation Time";
                dgv.Columns[4].HeaderText = "Edit";
                dgv.Columns[5].HeaderText = "Delete";
                dgv.ReadOnly = true;

                dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                
                dgv.Rows.Clear();
                int i = 0;
                foreach (documentStorage doc in dsList)
                {
                    try
                    {
                        dgv.Rows.Add();
                        i++;
                        dgv.Rows[i-1].Cells["FileName"].Value = doc.FileName.ToString();
                        dgv.Rows[i-1].Cells["Description"].Value = doc.Description.ToString();
                        dgv.Rows[i-1].Cells["LastUpdatedUser"].Value = doc.LastUploadedUser.ToString();
                        dgv.Rows[i-1].Cells["LastUpdatedTime"].Value = doc.LastUploadedTime.ToString("dd-MM-yyyy HH:mm:ss");
                        dgv.Rows[i - 1].Cells["docID"].Value = doc.DocumentID.ToString();
                        dgv.Rows[i - 1].Cells["docsubID"].Value = doc.DocumentSubID.ToString();
                        dgv.Rows[i - 1].Cells["RowID"].Value = doc.RowID;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show( System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                    }
                }
                dgv.AllowUserToAddRows = false;
                dgv.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return dgv;
        }
        //--

        public Boolean validateDocumentDetails(documentStorage ds)
        {
            Boolean status = true;
            try
            {
                if (ds.FileName.Trim().Length == 0 || ds.FileName == null)
                {
                    return false;
                }
                if (ds.Description.Trim().Length == 0 || ds.Description == null)
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
        public static string createFileFromDB(string docID, string docSubID, string fileName)
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
                        " from DocumentStorage " +
                        "where DocumentID='" + docID + "' and DocumentSubID='" + docSubID + "' and FileName='" + fileName + "'";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        byte[] strByte = Convert.FromBase64String(reader.GetString(0));
                        try
                        {
                            File.Delete(filepath);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                        File.WriteAllBytes(filepath, strByte);
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                }
            }
            catch (Exception exx)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return filepath;
        }

        public static Boolean deleteDocument(int rowid)
        {
            Boolean stat = true;
            string utString = "";
            try
            {
                string updateSQL =  "Delete from DocumentStorage where RowID= " + rowid ;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "DocumentStorage", "", updateSQL) +
                    Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    stat = false;
                    MessageBox.Show("Failed to delete.");
                }
            }
            catch(Exception ex)
            {
                stat = false;
            }
            return stat;
        }
        public static Boolean UpdateDocumentDesc(int rowid, string decs)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update DocumentStorage set Description='" + decs +
                    "', LastUpLoadedUser='" + Login.userLoggedIn + "'" +
                    ", LastUpLoadedTime=" + "GETDATE()" +
                   " where RowID = " + rowid;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentStorage", "", updateSQL) +
                    Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                    MessageBox.Show("Failed to update.");
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
    }
}
