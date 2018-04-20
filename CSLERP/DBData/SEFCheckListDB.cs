using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class sefcheck
    {
        public int rowid { get; set; }
        public string SEFID { get; set; }
        public int Sequenceno { get; set; }
        public string description { get; set; }
        public int Status { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
    }

    class SEFCheckListDB
    {
        //ActivityLogDB alDB = new ActivityLogDB();
        public List<sefcheck> getSEFCheckList(string sefid)
        {
            sefcheck flim;
            List<sefcheck> fLimList = new List<sefcheck>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID,SEFID,SequenceNo,Description,Status,CreateUser"+
                    ",CreateTime from SEFCheckList where SEFID='"+sefid+ "' order by SequenceNo";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    flim = new sefcheck();
                    flim.rowid = reader.GetInt32(0);
                    flim.SEFID = reader.GetString(1);
                    flim.Sequenceno = reader.GetInt32(2);
                    flim.description = reader.GetString(3);
                    flim.Status = reader.GetInt32(4);
                    flim.userCreateUser = reader.GetString(5);
                    flim.userCreateime = reader.GetDateTime(6);
                    fLimList.Add(flim);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return fLimList;

        }

        public Boolean updateSEFCheckList(sefcheck flim)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update SEFCheckList set SequenceNo='"+flim.Sequenceno+"', Description = '" + flim.description + "'," +
                    " Status='" + flim.Status +"'"+
                    " where RowID='" + flim.rowid + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "SEFCheckList", "", updateSQL) +
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
        ////////public Boolean checkEmployeeFinancialLimit(string docID, string empID, double ProductValue, double TaxAmount)
        ////////{
        ////////    Boolean status = false;
        ////////    double amt = 0;
        ////////    try
        ////////    {
        ////////        SqlConnection conn = new SqlConnection(Login.connString);
        ////////         string query = "select FinancialLimit " +
        ////////            "from EmployeeFinancialLimit  where DocumentID='" + docID + "' and EmployeeID='" + empID + "' and Status=1";
        ////////        SqlCommand cmd = new SqlCommand(query, conn);
        ////////        conn.Open();
        ////////        SqlDataReader reader = cmd.ExecuteReader();
        ////////        if (reader.Read())
        ////////        {
        ////////            amt = reader.GetDouble(0);
        ////////        }
        ////////        conn.Close();
        ////////        if (amt >= ProductValue)
        ////////        {
        ////////            status = true;
        ////////        }
        ////////    }
        ////////    catch (Exception ex)
        ////////    {
        ////////        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        ////////        status = false;
        ////////    }
        ////////    return status;
        ////////}
        public Boolean insertSefChecklist(sefcheck flim)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into SEFCheckList (SEFID,SequenceNo,Description,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + flim.SEFID + "'," +
                    "'" + flim.Sequenceno + "','" +
                    flim.description + "','" +
                    flim.Status + "'," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "SEFCheckList", "", updateSQL) +
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
        public Boolean validateSEFChecklist(sefcheck flim)
        {
            Boolean status = true;
            try
            {
                if (flim.description.Trim().Length == 0 || flim.description == null)
                {
                    return false;
                }
                if (flim.Status == 0)
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
    }
}

