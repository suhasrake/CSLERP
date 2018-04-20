using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Collections;

namespace CSLERP.DBData
{
    class doccommenter
    {
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string UserID { get; set; }
        public int DocumentStatus { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }


    }

    class DocCommenterDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<doccommenter> getDocCommList()
        {
            doccommenter docComm;
            List<doccommenter> DocCommList = new List<doccommenter>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.DocumentID, b.DocumentName,a.EmployeeID,c.Name," +
                    "a.status,d.UserID from DocumentCommenter a, Document b, " +
                    "Employee c,ERPUser d where a.DocumentID=b.DocumentID and " +
                    "a.EmployeeID=c.EmployeeID and " +
                    "a.EmployeeID=d.EmployeeID " +
                    "order by a.DocumentID,c.Name";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    docComm = new doccommenter();
                    docComm.DocumentID = reader.GetString(0);
                    docComm.DocumentName = reader.GetString(1);
                    docComm.EmployeeID = reader.GetString(2);
                    docComm.EmployeeName = reader.GetString(3);
                    docComm.DocumentStatus = reader.GetInt32(4);
                    docComm.UserID = reader.GetString(5);
                    DocCommList.Add(docComm);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return DocCommList;

        }

        public Boolean updateDocCommList(doccommenter doc, doccommenter prevdoc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update DocumentCommenter set EmployeeID='" + doc.EmployeeID + "'," +
                    " Status=" + doc.DocumentStatus +
                    " where DocumentID='" + prevdoc.DocumentID + "'" +
                    " and EmployeeID='" + prevdoc.EmployeeID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentCommenter", "", updateSQL) +
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
        public Boolean insertDocCommList(doccommenter doc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into DocumentCommenter (DocumentID,EmployeeID,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + doc.DocumentID + "'," +
                    "'" + doc.EmployeeID + "'," +
                    doc.DocumentStatus + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "DocumentCommenter", "", updateSQL) +
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
        public Boolean validateDocument(doccommenter doc)
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
        //create list view of existing commenters
        public ListView commenterLV(string docID)
        {
            ListView lv = new ListView();
            try
            {
                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                DocCommenterDB doccmdb = new DocCommenterDB();
                List<doccommenter> DocCommenters = doccmdb.getDocCommList();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Commenter", -2, HorizontalAlignment.Left);
                lv.Columns.Add("User", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Remarks", -2, HorizontalAlignment.Left);
                lv.Columns[1].Width = 150;

                foreach (doccommenter doccmtr in DocCommenters)
                {
                    if (doccmtr.DocumentID.Equals(docID))
                    {
                        if (doccmtr.DocumentStatus == 1)
                        {
                            ListViewItem item1 = new ListViewItem();
                            item1.Checked = false;
                            item1.SubItems.Add(doccmtr.EmployeeName.ToString());
                            item1.SubItems.Add(doccmtr.UserID.ToString());
                            item1.SubItems.Add("NA");
                            lv.Items.Add(item1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lv;
        }
        //create a data table from existing comment status string
        public System.Data.DataTable splitCommentStatus(string commentStatus)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                {
                    dt.Columns.Add("EmployeeName", typeof(string));
                    dt.Columns.Add("UserID", typeof(string));
                    dt.Columns.Add("Status", typeof(Int32));
                }
                string[] cmtstr = commentStatus.Split(Main.delimiter2);
                for (int i = 0; i < cmtstr.Length; i++)
                {
                    try
                    {
                        if (cmtstr[i].Trim().Length > 0)
                        {
                            string[] cmtValuesr = cmtstr[i].Split(Main.delimiter1);
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1][0] = cmtValuesr[0];
                            dt.Rows[dt.Rows.Count - 1][1] = cmtValuesr[1];
                            dt.Rows[dt.Rows.Count - 1][2] = cmtValuesr[2];
                        }
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return dt;
        }
        //update comment status string, if the user is a commenter and comment if final
        public string createCommentStatusString(string commentStatus, string uid)
        {
            string cmtStatusString = "";
            try
            {
                string[] cmtstr = commentStatus.Split(Main.delimiter2);
                for (int i = 0; i < cmtstr.Length; i++)
                {
                    try
                    {
                        if (cmtstr[i].Trim().Length > 0)
                        {
                            string[] cmtValuesr = cmtstr[i].Split(Main.delimiter1);
                            if (cmtValuesr[1] == uid)
                            {
                                cmtStatusString = cmtStatusString + cmtValuesr[0] + Main.delimiter1 + cmtValuesr[1] +
                                Main.delimiter1 + "1" + Main.delimiter2;
                            }
                            else
                            {
                                cmtStatusString = cmtStatusString + cmtValuesr[0] + Main.delimiter1 + cmtValuesr[1] +
                                Main.delimiter1 + cmtValuesr[2] + Main.delimiter2;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                        cmtStatusString = "";
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                cmtStatusString = "";
            }
            return cmtStatusString;
        }
        //function called by forward,approve and reverseprocess
        public static string removeUnapprovedCommentStatus(string commentStatus)
        {
            string cmtStatusString = "";
            try
            {
                string[] cmtstr = commentStatus.Split(Main.delimiter2);
                for (int i = 0; i < cmtstr.Length; i++)
                {
                    try
                    {
                        if (cmtstr[i].Trim().Length > 0)
                        {
                            string[] cmtValuesr = cmtstr[i].Split(Main.delimiter1);
                            if (cmtValuesr[2] == "1")
                            {
                                cmtStatusString = cmtStatusString + cmtValuesr[0] + Main.delimiter1 + cmtValuesr[1] +
                                Main.delimiter1 + cmtValuesr[2] + Main.delimiter2;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                        cmtStatusString = "";
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                cmtStatusString = "";
            }
            return cmtStatusString;
        }
        //create listview of commenters and mark those who are already in the comment loop
        public ListView verifyCommenterList(ListView lvCmtr, System.Data.DataTable dt)
        {
            try
            {
                foreach (ListViewItem itemRow in lvCmtr.Items)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i][1].ToString().Trim().Equals(itemRow.SubItems[2].Text.Trim()))
                        {
                            if (dt.Rows[i][2].ToString() == "0")
                            {
                                itemRow.Checked = true;
                                itemRow.SubItems[3].Text = "Comment Pending";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lvCmtr;
        }
        //create comenter list using old and new strings
        public string createCommenterList(ListView lvCmtr, System.Data.DataTable dt)
        {
            string commentStatus = "";
            try
            {
                foreach (ListViewItem itemRow in lvCmtr.Items)
                {
                    if (itemRow.Checked)
                    {
                        commentStatus = commentStatus + itemRow.SubItems[1].Text.Trim() + Main.delimiter1 +
                             itemRow.SubItems[2].Text.Trim() + Main.delimiter1 +
                             "0" + Main.delimiter2;
                    }
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][2].ToString() == "1")
                    {
                        commentStatus = commentStatus + dt.Rows[i][0].ToString() + Main.delimiter1 +
                             dt.Rows[i][1].ToString() + Main.delimiter1 +
                             "1" + Main.delimiter2;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return commentStatus;
        }

        //---Create gridview from comment string
        public DataGridView createCommentGridview(string comment)
        {
            DataGridView dgv = new DataGridView();
            dgv.Rows.Clear();
            try
            {
                dgv.Size = new Size(1000, 200);
                dgv.ColumnCount = 6;
                dgv.Columns[0].Name = "SlNo";
                dgv.Columns[0].Width = 50;
                dgv.Columns[1].Name = "Date";
                dgv.Columns[1].Width = 140;
                dgv.Columns[2].Name = "Commenter";
                dgv.Columns[2].Width = 100;
                dgv.Columns[3].Name = "Comment";
                dgv.Columns[3].Width = 650;
                dgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgv.Columns[4].Name = "Status";
                dgv.Columns[4].Width = 50;
                dgv.Columns[5].Name = "UserID";
                dgv.Columns[5].Width = 50;
                dgv.Columns[5].Visible = false;

                dgv.AllowUserToAddRows = false;
                dgv.ReadOnly = true;
                dgv.RowHeadersVisible = false;
                ////dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                ////dgv.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                dgv.AllowUserToAddRows = false;
                ////dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                string[] userstr = comment.Split(Main.delimiter2);
                for (int i = 0; i < userstr.Length; i++)
                {
                    try
                    {
                        string[] detailstr = userstr[i].Split(Main.delimiter1);
                        if (detailstr.Length == 6)
                        {
                            dgv.Rows.Add();
                            dgv.Rows[i].Cells["SlNo"].Value = i + 1;
                            dgv.Rows[i].Cells["Date"].Value = detailstr[2];
                            dgv.Rows[i].Cells["Commenter"].Value = detailstr[1];
                            dgv.Rows[i].Cells["Comment"].Value = detailstr[5];
                            dgv.Rows[i].Cells["Status"].Value = detailstr[3];
                            dgv.Rows[i].Cells["UserID"].Value = detailstr[0];
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            dgv.RefreshEdit();
            dgv.Refresh();
            return dgv;
        }

        public DataGridView setCommentsDGVroperties(DataGridView dgv)
        {
            try
            {
                dgv.Dock = DockStyle.Fill;
                dgv.AllowUserToResizeColumns = false;
                dgv.AllowUserToResizeColumns = false;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dgv.AllowUserToResizeRows = false;
                dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                dgv.DefaultCellStyle.Font = new Font("Calibri", 10);
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", 10);
                dgv.RowTemplate.Resizable = DataGridViewTriState.True;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgv.RowTemplate.MinimumHeight = 30;
                dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.RowHeadersVisible = false;
                dgv.ReadOnly = true;

                dgv.Bounds = new Rectangle(new Point(20, 20), new Size(750, 350));
                dgv.Columns[5].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return dgv;
        }
        //get last unauthorised comment of the user
        public string getLastUnauthorizedComment(DataGridView gv, string uid)
        {
            Dictionary<DateTime, string> CommentDict = new Dictionary<DateTime, string>();
            string cmntDetails = "";
            int rowNo = -1;
            int k = 0;
            try
            {
                for (int i = 0; i < gv.Rows.Count; i++)
                {

                    if ((gv.Rows[i].Cells["UserID"].Value.ToString() == uid) &&
                               (Convert.ToInt32(gv.Rows[i].Cells["Status"].Value) == 0))
                    {
                        k = 1;
                        rowNo = i;
                        string cmnt = gv.Rows[i].Cells["Comment"].Value.ToString();
                        DateTime dtt = Convert.ToDateTime(gv.Rows[i].Cells["Date"].Value.ToString());
                        CommentDict.Add(dtt, cmnt);
                    }
                    //dict2.Add(Convert.ToDateTime(gv.Rows[i].Cells["Date"].Value), i);
                }
                foreach (var item in CommentDict.OrderByDescending(key => key.Value))
                {
                    cmntDetails = item.Value;
                    break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                cmntDetails = "";
            }
            return cmntDetails;
        }
        public Int32 getLastCommentRow(DataGridView gv, string uid)
        {
            Dictionary<DateTime, Int32> CommentDict = new Dictionary<DateTime, Int32>();
            int rowNo = -1;
            try
            {
                for (int i = 0; i < gv.Rows.Count; i++)
                {

                    if ((gv.Rows[i].Cells["UserID"].Value.ToString() == uid) &&
                               (Convert.ToInt32(gv.Rows[i].Cells["Status"].Value) == 0))
                    {
                        DateTime dtt = Convert.ToDateTime(gv.Rows[i].Cells["Date"].Value.ToString());
                        CommentDict.Add(dtt, i);
                    }
                }
                foreach (var item in CommentDict.OrderByDescending(key => key.Value))
                {
                    rowNo = item.Value;
                    break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                rowNo = -1;
            }
            return rowNo;
        }

        //create comment string using comment gridview and edited string of the user
        public string processNewComment(DataGridView dgv, string cmntStr, string uid, string eName, int Status)
        {
            try
            {
                int cnt = dgv.Rows.Count;
                int row = getLastCommentRow(dgv, uid);
                if (row >= 0)
                {
                    dgv.Rows[row].Cells["Date"].Value = UpdateTable.getSQLDateTime().ToString("yyyy-MM-dd");
                    dgv.Rows[row].Cells["Commenter"].Value = eName;
                    dgv.Rows[row].Cells["Comment"].Value = cmntStr;
                    dgv.Rows[row].Cells["Status"].Value = Status;
                    dgv.Rows[row].Cells["UserId"].Value = uid;
                }
                else
                {
                    dgv.Rows.Add();
                    dgv.Rows[dgv.Rows.Count - 1].Cells["SlNo"].Value = 1;
                    dgv.Rows[dgv.Rows.Count - 1].Cells["Date"].Value = UpdateTable.getSQLDateTime().ToString("yyyy-MM-dd");
                    dgv.Rows[dgv.Rows.Count - 1].Cells["Commenter"].Value = eName;
                    dgv.Rows[dgv.Rows.Count - 1].Cells["Comment"].Value = cmntStr;
                    dgv.Rows[dgv.Rows.Count - 1].Cells["Status"].Value = Status;
                    dgv.Rows[dgv.Rows.Count - 1].Cells["UserID"].Value = uid;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            string finalStr = DGVtoString(dgv);
            ////MessageBox.Show("DETAILS:\n" + finalStr);
            return finalStr;
        }

        public string DGVtoString(DataGridView dgv)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    sb.Append(dgv.Rows[i].Cells["UserId"].Value.ToString() + Main.delimiter1);
                    sb.Append(dgv.Rows[i].Cells["Commenter"].Value.ToString() + Main.delimiter1);
                    sb.Append(dgv.Rows[i].Cells["Date"].Value.ToString() + Main.delimiter1);
                    sb.Append(dgv.Rows[i].Cells["Status"].Value.ToString() + Main.delimiter1);
                    sb.Append((dgv.Rows[i].Cells["Comment"].Value.ToString()).Length.ToString() + Main.delimiter1);
                    sb.Append(dgv.Rows[i].Cells["Comment"].Value.ToString());
                    if (i != dgv.Rows.Count - 1)
                        sb.Append(Main.delimiter2); // Removes the last delimiter 
                }
                string s = sb.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return sb.ToString();
        }
    }
}

