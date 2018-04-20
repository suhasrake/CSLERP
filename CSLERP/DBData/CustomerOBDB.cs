using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    public class CustomerOBheader
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public string FYID { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public int Status { get; set; }
        public string CreateUser { get; set; }
        public string CreatorName { get; set; }
        public DateTime CreateTime { get; set; }
        public decimal balancedebit { get; set; }
        public decimal balancecredit { get; set; }
        public int serialno { get; set; }
        public string accountcode { get; set; }
        public string accountname { get; set; }

    }
    public class CustomerOBdetail
    {
        public int RowID { get; set; }
        public string FYID { get; set; }
        public string DocumentID { get; set; }
        public int DocumentNo { get; set; }
        public DateTime DocumentDate { get; set; }
        public int SerialNo { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public decimal BalanceDebit { get; set; }
        public decimal BalanceCredit { get; set; }
        public decimal OBValue { get; set; }
    }
    class CustomerOBDB
    {
        public static List<CustomerOBheader> getCustomerOblist(int opt)
        {
            CustomerOBheader Alist;
            List<CustomerOBheader> AccList = new List<CustomerOBheader>();
            try
            {
                string query1 = "select distinct DocumentID,DocumentNo,DocumentDate,FYID, " +
                    "Status, Createuser,CreatorName, CreateTime " +
                    "from ViewCustomerOB where (Createuser='" + Login.userLoggedIn + "' and Status = 0 )";
                string query2 = "select distinct DocumentID,DocumentNo,DocumentDate,FYID, " +
                    "Status, Createuser,CreatorName, CreateTime " +
                    "from ViewCustomerOB where Status = 1";
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
                    case 6:
                        query = query2;
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
                    Alist = new CustomerOBheader();
                    Alist.DocumentID = reader.GetString(0);
                    Alist.DocumentNo = reader.GetInt32(1);
                    Alist.DocumentDate = reader.GetDateTime(2);
                    Alist.FYID = reader.GetString(3);
                    Alist.Status = reader.GetInt32(4);
                    Alist.CreateUser = reader.GetString(5);
                    Alist.CreatorName = reader.GetString(6);
                    Alist.CreateTime = reader.GetDateTime(7);
                    AccList.Add(Alist);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return AccList;
        }
        public static List<CustomerOBdetail> getCustomerOBDetail(CustomerOBheader Acchr)
        {
            CustomerOBdetail Accd;
            List<CustomerOBdetail> AccDetail = new List<CustomerOBdetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,DocumentNo,DocumentDate,SerialNo, " +
                    "CustomerID,CustomerName,BalanceDebit,BalanceCredit " +
                   "from ViewCustomerOB " +
                    " where DocumentID='" + Acchr.DocumentID + "'" +
                    " and DocumentNo=" + Acchr.DocumentNo +
                    " and DocumentDate='" + Acchr.DocumentDate.ToString("yyyy-MM-dd") + "'" + " order by SerialNo";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Accd = new CustomerOBdetail();
                    Accd.RowID = reader.GetInt32(0);
                    Accd.DocumentID = reader.GetString(1);                  
                    Accd.DocumentNo = reader.GetInt32(2);
                    Accd.DocumentDate = reader.GetDateTime(3).Date;
                    if (!reader.IsDBNull(4))
                    {
                        Accd.SerialNo = reader.GetInt32(4);
                    }
                    Accd.AccountCode = reader.GetString(5);
                    Accd.AccountName = reader.GetString(6);
                    Accd.BalanceDebit = reader.GetDecimal(7);
                    Accd.BalanceCredit = reader.GetDecimal(8);
                    AccDetail.Add(Accd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return AccDetail;
        }
        //public Boolean updateCstHeader(CustomerOBheader newacc, CustomerOBheader oldacc)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "update CustomerOBHeader set  FYID='" + newacc.FYID + "'" +
        //             " where DocumentID='" + oldacc.DocumentID +
        //            "' and DocumentNo=" + oldacc.DocumentNo +
        //            " and DocumentDate='" + oldacc.DocumentDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("update", "CustomerOBHeader", "", updateSQL) +
        //        Main.QueryDelimiter;
        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        //        status = false;
        //    }
        //    return status;
        //}
        public Boolean checkFYID(string acchr)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                SqlConnection con = new SqlConnection(Login.connString);
                string query = "select FYID from CustomerOBHeader";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                   if( (acchr == reader.GetString(0)))
                    {
                        return false;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status =false;
            }
            return status;

        }
        //public Boolean insertCustomerOBHeader(CustomerOBheader Acchr)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "insert into CustomerOBHeader " +
        //            " (DocumentID,FYID,DocumentDate,DocumentNo,Status," +
        //            "CreateUser,CreateTime)" +
        //            "values (" +
        //            "'" + Acchr.DocumentID + "'," +
        //           Acchr.FYID + "," +
        //           "'" +  Acchr.DocumentDate.ToString("yyyy-MM-dd") + "'," +
        //           "'" + Acchr.DocumentNo + "','"+Acchr.Status+"'," +
        //            "'" + Login.userLoggedIn + "'," +
        //            "GETDATE())";
        //        //"'" + pheader.ForwardUser + "'," +
        //        //"'" + pheader.ApproveUser + "'," +
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //        ActivityLogDB.PrepareActivityLogQquerString("insert", "CustomerOBHeader", "", updateSQL) +
        //        Main.QueryDelimiter;
        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        //        status = false;
        //    }
        //    return status;
        //}

        //public Boolean UpdateCustomerDetail(List<CustomerOBdetail> AccDetail, CustomerOBheader Acchdr)
        //{
        //    Boolean status = true;
        //    string utString = "";
        //    try
        //    {
        //        string updateSQL = "Delete from CustomerOBDetail where DocumentID='" + Acchdr.DocumentID + "'" +
        //            " and DocumentNo=" + Acchdr.DocumentNo +
        //            " and DocumentDate='" + Acchdr.DocumentDate.ToString("yyyy-MM-dd") + "'";
        //        utString = utString + updateSQL + Main.QueryDelimiter;
        //        utString = utString +
        //            ActivityLogDB.PrepareActivityLogQquerString("delete", "CustomerOBDetail", "", updateSQL) +
        //            Main.QueryDelimiter;
        //        foreach (CustomerOBdetail acc in AccDetail)
        //        {
        //            updateSQL = "insert into CustomerOBDetail " +
        //            "(DocumentID,DocumentNo,DocumentDate,SerialNo,CustomerID,BalanceDebit,BalanceCredit) " +
        //            "values ('" + acc.DocumentID + "'," +
        //            acc.DocumentNo + "," +
        //            "'" + acc.DocumentDate.ToString("yyyy-MM-dd") + "'," +
        //            "'" + acc.SerialNo + "','" +
        //             acc.AccountCode + "'," +
        //            "'" + acc.BalanceDebit + "'," +
        //            "'" + acc.BalanceCredit + "')";
        //            utString = utString + updateSQL + Main.QueryDelimiter;
        //            utString = utString +
        //            ActivityLogDB.PrepareActivityLogQquerString("insert", "CustomerOBDetail", "", updateSQL) +
        //            Main.QueryDelimiter;
        //        }
        //        if (!UpdateTable.UT(utString))
        //        {
        //            status = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        //        status = false;
        //    }
        //    return status;
        //}
        public Boolean validateAccOB(CustomerOBheader acchdr)
        {
            Boolean status = true;
            try
            {
                if (acchdr.DocumentID.Trim().Length == 0 || acchdr.DocumentID == null)
                {
                    return false;
                }
                //if (acchdr.DocumentNo == 0)
                //{
                //    return false;
                //}
                //if (acchdr.DocumentDate == null)
                //{
                //    return false;
                //}
                if (acchdr.FYID.Trim().Length == 0 || acchdr.FYID == null)
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
        public Boolean FinalizeCustomerOB(CustomerOBheader Acchr)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update CustomerOBheader set Status = 1" +
                    " where DocumentNo=" + Acchr.DocumentNo +
                    " and DocumentDate='" + Acchr.DocumentDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "CustomerOBheader", "", updateSQL) +
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
        public Boolean updateCustHeaderAndDetail(CustomerOBheader newcust, List<CustomerOBdetail> CustDetail)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update CustomerOBHeader set  FYID='" + newcust.FYID + "'" +
                     " where DocumentID='" + newcust.DocumentID +
                    "' and DocumentNo=" + newcust.DocumentNo +
                    " and DocumentDate='" + newcust.DocumentDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "CustomerOBHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from CustomerOBDetail where DocumentID='" + newcust.DocumentID + "'" +
                   " and DocumentNo=" + newcust.DocumentNo +
                   " and DocumentDate='" + newcust.DocumentDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "CustomerOBDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (CustomerOBdetail cob in CustDetail)
                {
                    updateSQL = "insert into CustomerOBDetail " +
                    "(DocumentID,DocumentNo,DocumentDate,SerialNo,CustomerID,BalanceDebit,BalanceCredit,FYID) " +
                    "values ('" + cob.DocumentID + "'," +
                    cob.DocumentNo + "," +
                    "'" + cob.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + cob.SerialNo + "','" +
                     cob.AccountCode + "'," +
                    "'" + cob.BalanceDebit + "'," +
                    "'" + cob.BalanceCredit +"','"+ cob.FYID + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "CustomerOBDetail", "", updateSQL) +
                    Main.QueryDelimiter;
                }
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                    MessageBox.Show("Transaction Exception Occured");
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        public Boolean InsertCustHeaderAndDetail(CustomerOBheader newcust, List<CustomerOBdetail> CustDetail)
        {

            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                newcust.DocumentNo = DocumentNumberDB.getNumber(newcust.DocumentID, 1);
                if (newcust.DocumentNo <= 0)
                {
                    MessageBox.Show("Error in Creating New Number");
                    return false;
                }
                updateSQL = "update DocumentNumber set TempNo =" + newcust.DocumentNo +
                    " where FYID='" + Main.currentFY + "' and DocumentID='" + newcust.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("update", "DocumentNumber", "", updateSQL) +
                   Main.QueryDelimiter;

                updateSQL = "insert into CustomerOBHeader " +
                     " (DocumentID,FYID,DocumentDate,DocumentNo,Status," +
                     "CreateUser,CreateTime)" +
                     "values (" +
                     "'" + newcust.DocumentID + "','" +
                    newcust.FYID + "'," +
                    "'" + newcust.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + newcust.DocumentNo + "','" + newcust.Status + "'," +
                     "'" + Login.userLoggedIn + "'," +
                     "GETDATE())";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "CustomerOBHeader", "", updateSQL) +
                Main.QueryDelimiter;

                updateSQL = "Delete from CustomerOBDetail where DocumentID='" + newcust.DocumentID + "'" +
                " and DocumentNo=" + newcust.DocumentNo +
                " and DocumentDate='" + newcust.DocumentDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "CustomerOBDetail", "", updateSQL) +
                    Main.QueryDelimiter;

                foreach (CustomerOBdetail cob in CustDetail)
                {
                    updateSQL = "insert into CustomerOBDetail " +
                    "(DocumentID,DocumentNo,DocumentDate,SerialNo,CustomerID,BalanceDebit,BalanceCredit,FYID) " +
                    "values ('" + cob.DocumentID + "'," +
                    newcust.DocumentNo + "," +
                    "'" + cob.DocumentDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + cob.SerialNo + "','" +
                     cob.AccountCode + "'," +
                    "'" + cob.BalanceDebit + "'," +
                    "'" + cob.BalanceCredit+"','" + cob.FYID + "')";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "CustomerOBDetail", "", updateSQL) +
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
                MessageBox.Show("Transaction Exception Occured");
            }
            return status;
        }
        //Customer List In GridView
        public DataGridView getGridViewForCustomerDetailsList()
        {
            DataGridView grdCust = new DataGridView();
            try
            {
                string[] strColArr = { "CustomerID", "CustomerName", "LedgerType" };
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
                        colArr[index].Width = 500;
                    else
                        colArr[index].Width = 200;
                    if (index == 2)
                        colArr[index].Visible = false;
                    grdCust.Columns.Add(colArr[index]);
                }

                CustomerDB cdb = new CustomerDB();
                List<customer> CustList = cdb.getCustomerList().OrderBy(cust => cust.name).ToList();

                foreach (customer cust in CustList)
                {
                    grdCust.Rows.Add();
                    grdCust.Rows[grdCust.Rows.Count - 1].Cells[strColArr[0]].Value = cust.CustomerID;
                    grdCust.Rows[grdCust.Rows.Count - 1].Cells[strColArr[1]].Value = cust.name;
                    grdCust.Rows[grdCust.Rows.Count - 1].Cells[strColArr[2]].Value = cust.LedgerType;
                }
            }
            catch (Exception ex)
            {
            }

            return grdCust;
        }
        /////////Subledger modification
        public static List<CustomerOBdetail> getCustomerOBListForPericualrFY(string fy)
        {
            CustomerOBdetail cust;
            List<CustomerOBdetail> CustDetail = new List<CustomerOBdetail>();
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select RowID,DocumentID,DocumentNo,DocumentDate,SerialNo, " +
                    "CustomerID,CustomerName,BalanceDebitINR,BalanceCreditINR,(BalanceDebitINR-BalanceCreditINR) " +
                   "from ViewCustomerOB " +
                    " where FYID='" + fy.Trim() + "' and status = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cust = new CustomerOBdetail();
                    cust.RowID = reader.GetInt32(0);
                    cust.DocumentID = reader.GetString(1);
                    cust.DocumentNo = reader.GetInt32(2);
                    cust.DocumentDate = reader.GetDateTime(3).Date;
                    if (!reader.IsDBNull(4))
                    {
                        cust.SerialNo = reader.GetInt32(4);
                    }
                    cust.AccountCode = reader.GetString(5);
                    cust.AccountName = reader.GetString(6);
                    cust.BalanceDebit = reader.GetDecimal(7);
                    cust.BalanceCredit = reader.GetDecimal(8);
                    cust.OBValue = reader.GetDecimal(9);
                    CustDetail.Add(cust);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return CustDetail;
        }
        //new customername
      
    }
}
