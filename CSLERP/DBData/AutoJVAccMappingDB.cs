using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class jvaccmapping
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string JVName { get; set; }
        public string DocumentName { get; set; }
        public string AccountCodeDebit { get; set; }
        public string AccountNameDebit { get; set; }
        public string AccountCodeCredit { get; set; }
        public string AccountNameCredit { get; set; }
        public int Status { get; set; }
        //public DateTime userCreateime { get; set; }
        //public string userCreateUser { get; set; }
    }

    class AutoJVAccMappingDB
    {
        public List<jvaccmapping> getjvaccmappingList()
        {
            jvaccmapping jvaccmappingrec;
            List<jvaccmapping> jvaccmappingsList = new List<jvaccmapping>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.RowID,a.JVName,a.DocumentID,d.DocumentName,a.AccountCodeDebit,b.Name,a.AccountCodeCredit,c.Name,a.Status " +
                    " from AutoJVAccountCodes a, AccountCode b,AccountCode c,Document d " +
                    " where a.AccountCodeDebit = b.AccountCode and a.AccountCodeCredit = c.AccountCode and a.DocumentID = d.DocumentID" +
                    " order by a.DocumentID";
                    
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    jvaccmappingrec = new jvaccmapping();
                    jvaccmappingrec.RowID = reader.GetInt32(0);
                    jvaccmappingrec.JVName = reader.GetString(1);
                    jvaccmappingrec.DocumentID = reader.GetString(2);
                    jvaccmappingrec.DocumentName = reader.GetString(3);
                    jvaccmappingrec.AccountCodeDebit = reader.GetString(4);
                    jvaccmappingrec.AccountNameDebit = reader.GetString(5);
                    jvaccmappingrec.AccountCodeCredit = reader.GetString(6);
                    jvaccmappingrec.AccountNameCredit = reader.GetString(7);
                    jvaccmappingrec.Status = reader.GetInt32(8);
                    jvaccmappingsList.Add(jvaccmappingrec);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return jvaccmappingsList;

        }
      
        public Boolean updatejvaccmapping(jvaccmapping doc, jvaccmapping prevdoc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update AutoJVAccountCodes set AccountCodeDebit='" + doc.AccountCodeDebit + "',"+
                    "AccountCodeCredit='"+doc.AccountCodeCredit+"',"+
                    " Status=" + doc.Status +
                    " where RowID=" + prevdoc.RowID ;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "AutoJVAccountCodes", "", updateSQL) +
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
        public Boolean insertAutoJVAccountCodes(jvaccmapping doc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into AutoJVAccountCodes (JVName,DocumentID,AccountCodeDebit,AccountCodeCredit,Status)" +
                    " values (" +
                     "'" + doc.JVName + "'," +
                    "'" + doc.DocumentID + "'," +
                    "'" + doc.AccountCodeDebit + "'," +
                    "'" + doc.AccountCodeCredit + "'," +
                    doc.Status +")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "AutoJVAccountCodes", "", updateSQL) +
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
        public Boolean validateDocument(jvaccmapping doc)
        {
            Boolean status = true;
            try
            {
                if (doc.JVName == null || doc.JVName.Trim().Length == 0)
                {
                    return false;
                }
                if (doc.DocumentID == null || doc.DocumentID.Trim().Length == 0 )
                {
                    return false;
                }
                if (doc.AccountCodeDebit == null || doc.AccountCodeDebit.Trim().Length == 0)
                {
                    return false;
                }
                if (doc.AccountCodeCredit == null || doc.AccountCodeCredit.Trim().Length == 0 ||
                    doc.AccountCodeCredit.Trim() == doc.AccountCodeDebit.Trim())
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

        public static jvaccmapping getjvaccmappingPerDocument(string jvname, string docid)
        {
            jvaccmapping jvaccmappingrec = new jvaccmapping();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,JVName,DocumentID,AccountCodeDebit,AccountCodeCredit,Status " +
                    " from AutoJVAccountCodes " +
                    " where JVName = '" + jvname + "' and DocumentID = '" + docid + "' and Status=1";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    jvaccmappingrec = new jvaccmapping();
                    jvaccmappingrec.RowID = reader.GetInt32(0);
                    jvaccmappingrec.JVName = reader.GetString(1);
                    jvaccmappingrec.DocumentID = reader.GetString(2);
                    jvaccmappingrec.AccountCodeDebit = reader.GetString(3);
                    jvaccmappingrec.AccountCodeCredit = reader.GetString(4);
                    jvaccmappingrec.Status = reader.GetInt32(5);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return jvaccmappingrec;

        }
        //
        //get all users who can forward the document to the user who logged in
        //
        //public string getForwarders(string docID, string empID)
        //{
        //    string forwarderList = "";
        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "select a.EmployeeID, b.userID from jvaccmapping a, ERPUser b  " +
        //            "where a.EmployeeID = b.EmployeeID and a.DocumentID='" + docID + 
        //            "' and a.SeniorEMployeeID='" + empID + "' and a.status=1";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            if (forwarderList.Length > 0)
        //            {
        //                forwarderList = forwarderList + ",";
        //            }
        //            forwarderList = forwarderList + "'" + reader.GetString(1) + "'";
        //        }
        //        conn.Close();
        //        if (forwarderList.Length == 0)
        //        {
        //            forwarderList = "''";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        //        forwarderList = "''";
        //    }
        //    return forwarderList;
        //}
        //public string getApprovers(string docID, string empID)
        //{
        //    string approverList = "";
        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "select a.EmployeeID, b.userID from jvaccmapping a, ERPUser b  " +
        //            "where a.SeniorEmployeeID = b.EmployeeID and a.DocumentID='" + docID + 
        //            "' and a.EMployeeID='" + empID + "' and a.Status = 1";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            if (approverList.Length > 0)
        //            {
        //                approverList = approverList + ",";
        //            }
        //            approverList = approverList + "'" + reader.GetString(1) + "'";
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        //        approverList = "";
        //    }
        //    return approverList;
        //}
        //public static string getApproverList(string docID, string empID)
        //{
        //    string approverList = "";
        //    try
        //    {
        //        SqlConnection conn = new SqlConnection(Login.connString);
        //        string query = "select a.EmployeeID, b.userID,b.Name from jvaccmapping a, ViewUserEmployeeList b  " +
        //            "where a.SeniorEmployeeID = b.EmployeeID and a.DocumentID='" + docID +
        //            "' and a.EMployeeID='" + empID + "' and a.Status = 1";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            approverList = approverList + reader.GetString(2) + Main.delimiter1+ reader.GetString(1)+Main.delimiter2;
        //        }
        //        conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        //        approverList = "";
        //    }
        //    return approverList;
        //}

        //public DataGridView getDocumentlistGrid()
        //{
        //    DataGridView empGgrid = new DataGridView();
        //    try
        //    {
        //        DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
        //        colChk.Width = 50;
        //        colChk.Name = "Select";
        //        colChk.HeaderText = "Select";
        //        colChk.ReadOnly = false;

        //        DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
        //        dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        //        dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
        //        dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //        dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
        //        dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        //        dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        //        dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

        //        empGgrid.EnableHeadersVisualStyles = false;

        //        empGgrid.AllowUserToAddRows = false;
        //        empGgrid.AllowUserToDeleteRows = false;
        //        empGgrid.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
        //        empGgrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
        //        empGgrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
        //        empGgrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
        //        empGgrid.ColumnHeadersHeight = 27;
        //        empGgrid.RowHeadersVisible = false;
        //        empGgrid.Columns.Add(colChk);

        //        DocumentDB dbrecord = new DocumentDB();
        //        List<document> DocItems = dbrecord.getDocuments();
        //        empGgrid.DataSource = DocItems;
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return empGgrid;
        //}

    }
}

