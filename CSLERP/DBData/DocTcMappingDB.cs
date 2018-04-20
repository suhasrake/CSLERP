using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class doctcmapping
    {
        public string DocumentID { get; set; }
        public string DocumentName { get; set; }
        public int RowID { get; set; }
        public int ParagraphID { get; set; }
        public string Heading { get; set; }
        public string ReferenceTC { get; set; }
        public string Detail { get; set; }
       
    }

    class DocTcMappingDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<doctcmapping> getTCMapping()
        {
            doctcmapping doctcmappingrec;
            List<doctcmapping> DoctcMappings = new List<doctcmapping>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select distinct a.DocumentID,b.DocumentName from TCMapping a,Document b where a.DocumentID=b.DocumentID";
                    
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    doctcmappingrec = new doctcmapping();
                    doctcmappingrec.DocumentID = reader.GetString(0);
                    doctcmappingrec.DocumentName = reader.GetString(1);
                    //doctcmappingrec.ReferenceTC = reader.GetString(2);
                    //doctcmappingrec.RowID = reader.GetInt32(2);
                    //doctcmappingrec.ParagraphID = reader.GetInt32(3);
                    //doctcmappingrec.Heading = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    //doctcmappingrec.Detail = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    DoctcMappings.Add(doctcmappingrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return DoctcMappings;

        }

        public List<doctcmapping> getdocTCList(string docid)
        {
            doctcmapping TcSel;
            List<doctcmapping> TCList = new List<doctcmapping>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select c.RowID,c.ParagraphID,c.ParagraphHeading,c.Detail from TermsAndCondition c,TCMapping d where "+
                              "  c.RowID = d.ReferenceTC and d.DocumentID = '"+docid+ "' order by c.ParagraphID asc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TcSel = new doctcmapping();
                    TcSel.RowID = reader.GetInt32(0);
                    TcSel.ParagraphID = reader.GetInt32(1);
                    TcSel.Heading = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    TcSel.Detail = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    TCList.Add(TcSel);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return TCList;
        }


        public List<doctcmapping> getTCList(string docid)
        {
            doctcmapping TcSel;
            List<doctcmapping> TCList = new List<doctcmapping>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,ParagraphID,ParagraphHeading,detail from TermsAndCondition where DocumentID='"+docid+"' ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TcSel = new doctcmapping();
                    TcSel.RowID = reader.GetInt32(0);
                    TcSel.ParagraphID = reader.GetInt32(1);
                    TcSel.Heading = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    TcSel.Detail = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    TCList.Add(TcSel);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return TCList;
        }

        public DataGridView getTclistGrid(string docid)
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

                DocTcMappingDB DoctcDB = new DocTcMappingDB();
                List<doctcmapping> empSElList = DoctcDB.getTCList(docid);
                doctcmapping obj = new doctcmapping();
                var prop = obj.GetType().GetProperties();
                foreach (var v in prop)
                {
                    empGgrid.Columns.Add(v.Name, v.Name);
                }

                //empGgrid.DataSource = empSElList;
                foreach (doctcmapping tcsel in empSElList)
                {
                    empGgrid.Rows.Add();
                    empGgrid.Rows[empGgrid.Rows.Count - 1].Cells["RowID"].Value = tcsel.RowID;
                    empGgrid.Rows[empGgrid.Rows.Count - 1].Cells["ParagraphID"].Value = tcsel.ParagraphID;
                    empGgrid.Rows[empGgrid.Rows.Count - 1].Cells["Heading"].Value = tcsel.Heading;
                    empGgrid.Rows[empGgrid.Rows.Count - 1].Cells["Detail"].Value = tcsel.Detail;
                    empGgrid.Rows[empGgrid.Rows.Count - 1].Height = 60;
                }


            }
            catch (Exception ex)
            {
            }

            return empGgrid;
        }

        public Boolean UpdateTCMapping(List<doctcmapping> TCMList, doctcmapping tc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Delete from TCMapping where DocumentID = '" + tc.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "TCMapping", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (doctcmapping tcm in TCMList)
                {
                    updateSQL = "insert into TCMapping " +
                    "(DocumentID,ReferenceTC) " +
                    "values ('" + tcm.DocumentID + "'," + "'" + tcm.ReferenceTC + "')";

                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "TCMapping", "", updateSQL) +
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







        public Boolean updateDocTCMapping(string rowid, doctcmapping prevdoc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update TCMapping set ReferenceTC='" + rowid + "'"+
                    " where DocumentID='" + prevdoc.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "TCMapping", "", updateSQL) +
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
        public Boolean insertDocTCMapping(doctcmapping doc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into TCMapping (DocumentID,ReferenceTC)" +
                    "values (" +
                    "'" + doc.DocumentID + "'," +
                    "'" + doc.RowID + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "TCMapping", "", updateSQL) +
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
        public Boolean validateDocument(doctcmapping doc)
        {
            Boolean status = true;
            try
            {
                if (doc.DocumentID.Trim().Length == 0 || doc.DocumentID == null)
                {
                    return false;
                }
                //if (doc.RowID == 0)
                //{
                //    return false;
                //}
                //if (doc.SeniorEmployeeID.Trim().Length == 0 || doc.SeniorEmployeeID == null ||
                //    doc.EmployeeID.Trim() == doc.SeniorEmployeeID.Trim())
                //{
                //    return false;
                //}
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
        public static ListView getTCListViewForPerticularDoc(string docid)
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
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Paragraph ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Paragraph Heading", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Detail", -2, HorizontalAlignment.Left);
                DocTcMappingDB tcmapdb = new DocTcMappingDB();
                List<doctcmapping> TCList = tcmapdb.getdocTCList(docid).OrderByDescending(tc => tc.ParagraphID).ToList();
                foreach (doctcmapping tcmap in TCList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = false;
                    item.SubItems.Add(tcmap.ParagraphID.ToString());
                    item.SubItems.Add(tcmap.Heading);
                    item.SubItems.Add(tcmap.Detail);
                    lv.Items.Add(item);
                }

            }
            catch (Exception ex)
            {

            }
            return lv;
        }
        public static DataGridView getGridViewForTCMapping(string docID, List<doctcmapping> TCListTemp)
        {

            DataGridView grdTC = new DataGridView();
            try
            {
                string[] strColArr = { "ParagraphID", "ParagraphHeading","Detail"};
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                };

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                //dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdTC.EnableHeadersVisualStyles = false;
                grdTC.AllowUserToAddRows = false;
                grdTC.AllowUserToDeleteRows = false;
                grdTC.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdTC.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdTC.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdTC.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdTC.ColumnHeadersHeight = 27;
                grdTC.RowHeadersVisible = false;
                grdTC.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;
                grdTC.Columns.Add(colChk);
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str;
                    //colArr[index].ToolTipText = "Double click to see detail";
                    colArr[index].HeaderText = str;
                    colArr[index].ReadOnly = true;
                    if (index == 2)
                        colArr[index].Width = 440;
                    //else if (index == 2)
                    //    colArr[index].Width = 100;
                    else
                        colArr[index].Width = 100;
                    grdTC.Columns.Add(colArr[index]);
                }

                DocTcMappingDB tcmapdb = new DocTcMappingDB();
                if (TCListTemp.Count == 0)
                {
                    TCListTemp = tcmapdb.getdocTCList(docID);
                }
                foreach (doctcmapping tcmap in TCListTemp)
                {
                    grdTC.Rows.Add();
                    grdTC.Rows[grdTC.Rows.Count - 1].Cells[strColArr[0]].Value = tcmap.ParagraphID;
                    grdTC.Rows[grdTC.Rows.Count - 1].Cells[strColArr[1]].Value = tcmap.Heading;
                    grdTC.Rows[grdTC.Rows.Count - 1].Cells[strColArr[2]].Value = tcmap.Detail;
                }
            }
            catch (Exception ex)
            {
            }

            return grdTC;
        }

    }
}

