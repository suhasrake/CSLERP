using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class docempmapping
    {
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string SeniorEmployeeID { get; set; }
        public string SeniorEmployeeName { get; set; }
        public int DocumentStatus { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
    }

    class DocEmpMappingDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<docempmapping> getDocEmpMapping()
        {
            docempmapping docempmappingrec;
            List<docempmapping> DocEmpMappings = new List<docempmapping>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.DocumentID, b.DocumentName,a.EmployeeID,c.Name, a.SeniorEmployeeID,"+
                    "d.Name, a.status from DocEmpMapping a, Document b, " +
                    "Employee c, Employee d where a.DocumentID=b.DocumentID and "+
                    "a.EmployeeID=c.EmployeeID and a.SeniorEmployeeID=d.EmployeeID "+
                    "order by a.DocumentID,c.Name";
                    
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    docempmappingrec = new docempmapping();
                    docempmappingrec.DocumentID = reader.GetString(0);
                    docempmappingrec.DocumentName = reader.GetString(1);
                    docempmappingrec.EmployeeID = reader.GetString(2);
                    docempmappingrec.EmployeeName = reader.GetString(3);
                    docempmappingrec.SeniorEmployeeID = reader.GetString(4);
                    docempmappingrec.SeniorEmployeeName = reader.GetString(5);
                    docempmappingrec.DocumentStatus = reader.GetInt32(6);
                    DocEmpMappings.Add(docempmappingrec);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return DocEmpMappings;

        }
      
        public Boolean updateDocEmpMapping(docempmapping doc, docempmapping prevdoc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update DocEmpMapping set EmployeeID='" + doc.EmployeeID + "',"+
                    "SeniorEmployeeID='"+doc.SeniorEmployeeID+"',"+
                    " Status=" + doc.DocumentStatus +
                    " where DocumentID='" + prevdoc.DocumentID + "'"+
                    " and EmployeeID='"+prevdoc.EmployeeID+"'"+
                    " and SeniorEmployeeID='"+prevdoc.SeniorEmployeeID+"'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "DocEmpMapping", "", updateSQL) +
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
        public Boolean insertDocEmpMapping(docempmapping doc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into DocEmpMapping (DocumentID,EmployeeID,SeniorEmployeeID,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + doc.DocumentID + "'," +
                    "'" + doc.EmployeeID + "'," +
                    "'" + doc.SeniorEmployeeID + "'," +
                    doc.DocumentStatus + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "DocEmpMapping", "", updateSQL) +
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
        public Boolean validateDocument(docempmapping doc)
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
                if (doc.SeniorEmployeeID.Trim().Length == 0 || doc.SeniorEmployeeID == null ||
                    doc.EmployeeID.Trim() == doc.SeniorEmployeeID.Trim())
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return status;
        }
        //
        //get all users who can forward the document to the user who logged in
        //
        public string getForwarders(string docID, string empID)
        {
            string forwarderList = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.EmployeeID, b.userID from DocEmpMapping a, ERPUser b  " +
                    "where a.EmployeeID = b.EmployeeID and a.DocumentID='" + docID + 
                    "' and a.SeniorEMployeeID='" + empID + "' and a.status=1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (forwarderList.Length > 0)
                    {
                        forwarderList = forwarderList + ",";
                    }
                    forwarderList = forwarderList + "'" + reader.GetString(1) + "'";
                }
                conn.Close();
                if (forwarderList.Length == 0)
                {
                    forwarderList = "''";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                forwarderList = "''";
            }
            return forwarderList;
        }
        public string getApprovers(string docID, string empID)
        {
            string approverList = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.EmployeeID, b.userID from DocEmpMapping a, ERPUser b  " +
                    "where a.SeniorEmployeeID = b.EmployeeID and a.DocumentID='" + docID + 
                    "' and a.EMployeeID='" + empID + "' and a.Status = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (approverList.Length > 0)
                    {
                        approverList = approverList + ",";
                    }
                    approverList = approverList + "'" + reader.GetString(1) + "'";
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                approverList = "";
            }
            return approverList;
        }
        public static string getApproverList(string docID, string empID)
        {
            string approverList = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.EmployeeID, b.userID,b.Name from DocEmpMapping a, ViewUserEmployeeList b  " +
                    "where a.SeniorEmployeeID = b.EmployeeID and a.DocumentID='" + docID +
                    "' and a.EMployeeID='" + empID + "' and a.Status = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    approverList = approverList + reader.GetString(2) + Main.delimiter1+ reader.GetString(1)+Main.delimiter2;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                approverList = "";
            }
            return approverList;
        }
        public static ListView ApproverLV(string docID,string empID)
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
                ////DocCommenterDB doccmdb = new DocCommenterDB();
                ////List<doccommenter> DocCommenters = doccmdb.getDocCommList();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Approver", -2, HorizontalAlignment.Left);
                lv.Columns.Add("User", -2, HorizontalAlignment.Left);
                lv.Columns[1].Width = 150;

                string approverList = getApproverList(docID, empID);
                string[] al1 = approverList.Split(Main.delimiter2);
                for (int i = 0; i < al1.Length; i++)
                {
                    try
                    {
                        if (al1[i].Trim().Length > 0)
                        {
                            string[] al2 = al1[i].Split(Main.delimiter1);

                            ListViewItem item1 = new ListViewItem();
                            item1.Checked = false;
                            item1.SubItems.Add(al2[0]);
                            item1.SubItems.Add(al2[1]);
                            lv.Items.Add(item1);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lv;
        }

        public DataGridView getDocumentlistGrid()
        {
            DataGridView empGgrid = new DataGridView();
            try
            {
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

                empGgrid.EnableHeadersVisualStyles = false;

                empGgrid.AllowUserToAddRows = false;
                empGgrid.AllowUserToDeleteRows = false;
                empGgrid.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                empGgrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
                empGgrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                empGgrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                empGgrid.ColumnHeadersHeight = 27;
                empGgrid.RowHeadersVisible = false;
                empGgrid.Columns.Add(colChk);

                DocumentDB dbrecord = new DocumentDB();
                List<document> DocItems = dbrecord.getDocuments();
                empGgrid.DataSource = DocItems;
            }
            catch (Exception ex)
            {
            }

            return empGgrid;
        }

    }
}

