using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class projectheader
    {
        public int rowID { get; set; }
        public String DocumentID { get; set; }
        public String DocumentName { get; set; }
        public int TemporaryNo { get; set; }
        public DateTime TemporaryDate { get; set; }
        public int TrackingNo { get; set; }
        public DateTime TrackingDate { get; set; }
        public string ProjectID { get; set; }
        public string ProjectManager { get; set; }
        public string ProjectManagerName { get; set; }
        public string ShortDescription { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string OfficeID { get; set; }
        public string OfficeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TargetDate { get; set; }
        public int Status { get; set; }
        public int DocumentStatus { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string CreatorName { get; set; }
        public string ForwarderName { get; set; }
        public string ApproverName { get; set; }
        public string ForwarderList { get; set; }
        public int ProjectStatus { get; set; }
    }
    class customerforproject
    {
        public string code { get; set; }
        public string name { get; set; }
    }
    class ProjectHeaderDB
    {
        ActivityLogDB alDB = new ActivityLogDB();


        public List<projectheader> getFilteredProjectHeader(string userList, int opt)
        {
            projectheader pheader;
            List<projectheader> pheaderlist = new List<projectheader>();
            try
            {
                string query1 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " TrackingNo,TrackingDate,ProjectID,ProjectManager,ProjectManagerName,ShortDescription,CustomerID,CustomerName,StartDate," +
                    " TargetDate,Status,DocumentStatus,CreateTime, " +
                    "CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName , ForwarderList,OfficeID,OfficeName" +
                    " from ViewProjectHeader" +
                   " where ((forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1))" +
                     " order by TemporaryNo";

                string query2 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " TrackingNo,TrackingDate,ProjectID,ProjectManager,ProjectManagerName,ShortDescription,CustomerID,CustomerName,StartDate," +
                    " TargetDate,Status,DocumentStatus,CreateTime, " +
                    "CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,ForwarderList,OfficeID,OfficeName" +
                    " from ViewProjectHeader" +
                    " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "'))" +
                    " order by TemporaryNo";

                string query3 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " TrackingNo,TrackingDate,ProjectID,ProjectManager,ProjectManagerName,ShortDescription,CustomerID,CustomerName,StartDate," +
                    " TargetDate,Status,DocumentStatus,CreateTime, " +
                    "CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,ForwarderList,OfficeID,OfficeName" +
                    " from ViewProjectHeader" +
                     " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99)   order by TemporaryNo";
                string query6 = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " TrackingNo,TrackingDate,ProjectID,ProjectManager,ProjectManagerName,ShortDescription,CustomerID,CustomerName,StartDate," +
                    " TargetDate,Status,DocumentStatus,CreateTime, " +
                    "CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,ForwarderList,OfficeID,OfficeName" +
                    " from ViewProjectHeader" +
                   " where  DocumentStatus = 99  and Status = 1 order by ProjectID";

                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "";
                switch (opt)
                {
                    case 1:
                        query = query1;
                        break;
                    case 2:
                        query = query2;
                        break;
                    case 3:
                        query = query3;
                        break;
                    case 6:
                        query = query6;
                        break;
                    default:
                        query = "";
                        break;
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pheader = new projectheader();
                    pheader.rowID = reader.GetInt32(0);
                    pheader.DocumentID = reader.GetString(1);
                    pheader.DocumentName = reader.GetString(2);
                    pheader.TemporaryNo = reader.GetInt32(3);
                    pheader.TemporaryDate = reader.GetDateTime(4);
                    pheader.TrackingNo = reader.GetInt32(5);
                    pheader.TrackingDate = reader.GetDateTime(6);
                    pheader.ProjectID = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    pheader.ProjectManager = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    pheader.ProjectManagerName = reader.IsDBNull(9) ? "" : reader.GetString(9);
                    pheader.ShortDescription = reader.IsDBNull(10) ? "" : reader.GetString(10);
                    pheader.CustomerID = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    pheader.CustomerName = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    pheader.StartDate = reader.GetDateTime(13);
                    pheader.TargetDate = reader.GetDateTime(14);
                    pheader.Status = reader.GetInt32(15);
                    pheader.DocumentStatus = reader.GetInt32(16);
                    pheader.CreateTime = reader.GetDateTime(17);
                    pheader.CreateUser = reader.GetString(18);
                    pheader.ForwardUser = reader.GetString(19);
                    pheader.ApproveUser = reader.GetString(20);
                    pheader.CreatorName = reader.GetString(21);
                    pheader.ForwarderName = reader.GetString(22);
                    pheader.ApproverName = reader.GetString(23);
                    if (!reader.IsDBNull(24))
                    {
                        pheader.ForwarderList = reader.GetString(24);
                    }
                    else
                    {
                        pheader.ForwarderList = "";
                    }
                    pheader.OfficeID = reader.IsDBNull(25) ? "" : reader.GetString(25);
                    pheader.OfficeName = reader.IsDBNull(26) ? "" : reader.GetString(26);
                    pheaderlist.Add(pheader);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Project Header Data");
            }
            return pheaderlist;

        }

        public Boolean updateProjectHeader(projectheader pheader, projectheader prevpheader)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProjectHeader set   CustomerID='" + pheader.CustomerID + "'" +
                     ", Status=" + pheader.Status +
                    ", ProjectID ='" + pheader.ProjectID +
                    "', OfficeID ='" + pheader.OfficeID +
                    "', ProjectManager='" + pheader.ProjectManager +
                    "', StartDate='" + pheader.StartDate.ToString("yyyy-MM-dd") +
                    "', TargetDate='" + pheader.TargetDate.ToString("yyyy-MM-dd") +
                    "', ShortDescription='" + pheader.ShortDescription +
                    "', ForwarderList='" + pheader.ForwarderList +
                     "' where DocumentID='" + prevpheader.DocumentID +
                    "' and TemporaryNo=" + prevpheader.TemporaryNo +
                    " and TemporaryDate='" + prevpheader.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProjectHeader", "", updateSQL) +
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
        public Boolean insertProjectHeader(projectheader pheader)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into ProjectHeader " +
                    " (DocumentID,TemporaryNo,TemporaryDate,TrackingNo,TrackingDate," +
                    "ProjectID,OfficeID,ProjectManager,ShortDescription,CustomerID,StartDate,TargetDate,DocumentStatus," +
                    "Status,CreateUser,CreateTime,ForwarderList)" +
                    "values (" +
                    "'" + pheader.DocumentID + "'," +
                   pheader.TemporaryNo + "," +
                   "'" + pheader.TemporaryDate.ToString("yyyy-MM-dd") + "'," +
                   pheader.TrackingNo + "," +
                    "'" + pheader.TrackingDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + pheader.ProjectID + "'," +
                    "'" + pheader.OfficeID + "'," +
                    "'" + pheader.ProjectManager + "'," +
                    "'" + pheader.ShortDescription + "'," +
                    "'" + pheader.CustomerID + "'," +
                    "'" + pheader.StartDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + pheader.TargetDate.ToString("yyyy-MM-dd") + "'," +
                    pheader.DocumentStatus + "," +
                    pheader.Status + "," +
                    "'" + Login.userLoggedIn +"',"+ 
                    "GETDATE()" +
                    ",'" + pheader.ForwarderList + "')" ;
                //"'" + pheader.ForwardUser + "'," +
                //"'" + pheader.ApproveUser + "'," +
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "ProjectHeader", "", updateSQL) +
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
        public Boolean validateProjectHeader(projectheader pheader)
        {
            Boolean status = true;
            try
            {
                if (pheader.DocumentID.Trim().Length == 0 || pheader.DocumentID == null)
                {
                    return false;
                }

                if (pheader.ProjectID.Trim().Length == 0 || pheader.ProjectID == null)
                {
                    return false;
                }
                if (pheader.OfficeID.Trim().Length == 0 || pheader.OfficeID == null)
                {
                    return false;
                }
                //if (pheader.Status <= 0)
                //{
                //    return false;
                //}

                if (pheader.ProjectManager.Trim().Length == 0 || pheader.ProjectManager == null)
                {
                    return false;
                }

                if (pheader.ShortDescription.Trim().Length == 0 || pheader.ShortDescription == null)
                {
                    return false;
                }

                if (pheader.CustomerID.Trim().Length == 0 || pheader.CustomerID == null)
                {
                    return false;
                }

                if ( pheader.StartDate == null)
                {
                    return false;
                }
                if (pheader.TargetDate < DateTime.Now)
                {
                    return false;
                }
                if (pheader.TargetDate < DateTime.Now || pheader.TargetDate < pheader.StartDate || pheader.TargetDate == null)
                {
                    return false;
                }
    
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to insert Project Header.");
            }
            
            return status;
        }
        public Boolean forwardProjectHeader(projectheader prevpheader)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProjectHeader set DocumentStatus=" + (prevpheader.DocumentStatus + 1) +
                     ", forwardUser='" + prevpheader.ForwardUser + "'" +
                    ", ForwarderList='" + prevpheader.ForwarderList + "'" +
                    " where DocumentID='" + prevpheader.DocumentID +
                    "' and TemporaryNo=" + prevpheader.TemporaryNo +
                    " and TemporaryDate='" + prevpheader.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("forward", "ProjectHeader", "", updateSQL) +
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
        public Boolean reverseProjectHeader(projectheader prevpheader)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProjectHeader set DocumentStatus=" + prevpheader.DocumentStatus +
                    ", forwardUser='" + prevpheader.ForwardUser + "'" +
                    ", ForwarderList='" + prevpheader.ForwarderList + "'" +
                    " where DocumentID='" + prevpheader.DocumentID + "'" +
                    " and TemporaryNo=" + prevpheader.TemporaryNo +
                    " and TemporaryDate='" + prevpheader.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProjectHeader", "", updateSQL) +
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
        public Boolean ApproveProjectHeader(projectheader prevpheader)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update ProjectHeader set DocumentStatus=99" +
                    ", Status=1" +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    ", TrackingNo=" + prevpheader.TrackingNo +
                    ", TrackingDate=convert(date, getdate())" +
                    " where DocumentID='" + prevpheader.DocumentID +
                    "' and TemporaryNo=" + prevpheader.TemporaryNo +
                    " and TemporaryDate='" + prevpheader.TemporaryDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "ProjectHeader", "", updateSQL) +
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
        public static void fillprojectCombo(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                ProjectHeaderDB projectdb = new ProjectHeaderDB();
                List<projectheader> project = projectdb.getFilteredProjectHeader("",6);
                foreach (projectheader poh in project)
                {
                    if (poh.Status == 1)
                    {
                        cmb.Items.Add(poh.ProjectID);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
        }

        public static void fillCustomerForProjectComboNew(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                List<customerforproject> cfps = getCustomerForProject();
                foreach (customerforproject cfp in cfps)
                {
                    Structures.ComboBoxItem cbitem =
                            new Structures.ComboBoxItem(cfp.name, cfp.code);
                    cmb.Items.Add(cbitem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }

        }
        static List<customerforproject> getCustomerForProject()
        {
            customerforproject cfp;
            List<customerforproject> cfps = new List<customerforproject>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);

                string query = "select ReferenceID,ReferenceName from viewcustomerforproject order by ReferenceName";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cfp = new customerforproject();
                    cfp.code = reader.GetString(0);
                    cfp.name = reader.GetString(1);
                    cfps.Add(cfp);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return cfps;
        }

        //Project List In GridView
        public DataGridView getGridViewForProjectList()
        {
            DataGridView grdCust = new DataGridView();
            try
            {
                string[] strColArr = { "ProjectID", "Description", "ProjectManager" };
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
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdCust.EnableHeadersVisualStyles = false;
                grdCust.AllowUserToAddRows = false;
                grdCust.AllowUserToDeleteRows = false;
                grdCust.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdCust.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdCust.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdCust.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdCust.ColumnHeadersHeight = 27;
                grdCust.RowHeadersVisible = false;
                grdCust.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;
                grdCust.Columns.Add(colChk);
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str;
                    colArr[index].HeaderText = str;
                    colArr[index].ReadOnly = true;
                    if (index == 1)
                        colArr[index].Width = 300;
                    else
                        colArr[index].Width = 125;
                    //if (index == 2)
                    //    colArr[index].Visible = false;
                    grdCust.Columns.Add(colArr[index]);
                }

                ProjectHeaderDB pdb = new ProjectHeaderDB();
                List<projectheader> ProjList = pdb.getFilteredProjectHeader("", 6).OrderBy(proj => proj.ProjectID).ToList();

                foreach (projectheader projh in ProjList)
                {
                    grdCust.Rows.Add();
                    grdCust.Rows[grdCust.Rows.Count - 1].Cells[strColArr[0]].Value = projh.ProjectID;
                    grdCust.Rows[grdCust.Rows.Count - 1].Cells[strColArr[1]].Value = projh.ShortDescription;
                    grdCust.Rows[grdCust.Rows.Count - 1].Cells[strColArr[2]].Value = projh.ProjectManagerName;
                }
            }
            catch (Exception ex)
            {
            }
            return grdCust;
        }
        //gridadded
        public DataGridView getProjectlistGrid(int stat)
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

                ProjectHeaderDB phdb = new ProjectHeaderDB();
                List<projectheader> PHList = new List<projectheader>();
                if (stat == 0)
                {
                    PHList = phdb.getFilteredProject();
                }
                else
                {
                    PHList = phdb.getFilteredProject().Where(x => x.ProjectStatus == stat).ToList();
                }
                empGgrid.DataSource = PHList;
            }
            catch (Exception ex)
            {
            }

            return empGgrid;
        }

        public List<projectheader> getFilteredProject()
        {
            projectheader pheader;
            List<projectheader> pheaderlist = new List<projectheader>();
            try
            {

                string query = "select RowID, DocumentID, DocumentName,TemporaryNo,TemporaryDate," +
                    " TrackingNo,TrackingDate,ProjectID,ProjectManager,ProjectManagerName,ShortDescription,CustomerID,CustomerName,StartDate," +
                    " TargetDate,Status,DocumentStatus,CreateTime, " +
                    "CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,ForwarderList,OfficeID,OfficeName,ProjectStatus" +
                    " from ViewProjectHeader" +
                   " where  DocumentStatus = 99  and Status = 1 order by ProjectID";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    pheader = new projectheader();
                    pheader.rowID = reader.GetInt32(0);
                    pheader.DocumentID = reader.GetString(1);
                    pheader.DocumentName = reader.GetString(2);
                    pheader.TemporaryNo = reader.GetInt32(3);
                    pheader.TemporaryDate = reader.GetDateTime(4);
                    pheader.TrackingNo = reader.GetInt32(5);
                    pheader.TrackingDate = reader.GetDateTime(6);
                    pheader.ProjectID = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    pheader.ProjectManager = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    pheader.ProjectManagerName = reader.IsDBNull(9) ? "" : reader.GetString(9);
                    pheader.ShortDescription = reader.IsDBNull(10) ? "" : reader.GetString(10);
                    pheader.CustomerID = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    pheader.CustomerName = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    pheader.StartDate = reader.GetDateTime(13);
                    pheader.TargetDate = reader.GetDateTime(14);
                    pheader.Status = reader.GetInt32(15);
                    pheader.DocumentStatus = reader.GetInt32(16);
                    pheader.CreateTime = reader.GetDateTime(17);
                    pheader.CreateUser = reader.GetString(18);
                    pheader.ForwardUser = reader.GetString(19);
                    pheader.ApproveUser = reader.GetString(20);
                    pheader.CreatorName = reader.GetString(21);
                    pheader.ForwarderName = reader.GetString(22);
                    pheader.ApproverName = reader.GetString(23);
                    if (!reader.IsDBNull(24))
                    {
                        pheader.ForwarderList = reader.GetString(24);
                    }
                    else
                    {
                        pheader.ForwarderList = "";
                    }
                    pheader.OfficeID = reader.IsDBNull(25) ? "" : reader.GetString(25);
                    pheader.OfficeName = reader.IsDBNull(26) ? "" : reader.GetString(26);
                    pheader.ProjectStatus = reader.GetInt32(27);
                    pheaderlist.Add(pheader);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Project  Data");
            }
            return pheaderlist;

        }
    }
}
