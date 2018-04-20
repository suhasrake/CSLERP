using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class cvinr
    {
        public int rowID { get; set; }
        public DateTime ConversionDate { get; set; }
        public string CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        public int type { get; set; }
        public float INRValue { get; set; }
        public int status { get; set; }
        public int documentStatus { get; set; }
        public DateTime Createime { get; set; }
        public string CreateUser { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string ForwarderList { get; set; }
    }

    class CurrencyVsINRDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<cvinr> getFilteredCVINR(string userList, int opt)
        {
            cvinr cvi;
            List<cvinr> CVINRs = new List<cvinr>();
            try
            {
                string query1 = "select a.rowid, a.CurrencyDate, a.CurrencyID,b.Name,a.Type,a.INRValue,a.Status,a.DocumentStatus,isnull(a.CreateUser,' '),isnull(a.ForwardUser,' '),isnull(a.ApproveUser,' '),ForwarderList " +
                    " from CurrencyVsINR a, Currency b where a.CurrencyID=b.CurrencyID " +
                    " and" +

                   " ((a.forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (a.createuser='" + Login.userLoggedIn + "' and DocumentStatus=1))" +

                    " order by a.CurrencyDate,a.CurrencyID";

                string query2 = "select a.rowid, a.CurrencyDate, a.CurrencyID,b.Name,a.Type,a.INRValue,a.Status,a.DocumentStatus,isnull(a.CreateUser,' '),isnull(a.ForwardUser,' '),isnull(a.ApproveUser,' '),ForwarderList " +
                   " from CurrencyVsINR a, Currency b where a.CurrencyID=b.CurrencyID " +
                   " and" +

                   " ((a.createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (a.ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "'))" +

                   " order by a.CurrencyDate,a.CurrencyID";

                string query3 = "select a.rowid, a.CurrencyDate, a.CurrencyID,b.Name,a.Type,a.INRValue,a.Status,a.DocumentStatus,isnull(a.CreateUser,' '),isnull(a.ForwardUser,' '),isnull(a.ApproveUser,' '),ForwarderList " +
                   " from CurrencyVsINR a, Currency b where a.CurrencyID=b.CurrencyID " +
                   " and" +

                    " ((a.createuser='" + Login.userLoggedIn + "'" +
                    " or a.ForwarderList like '%" + userList + "%'" +
                    " or a.approveUser='" + Login.userLoggedIn + "')" +
                    " and a.DocumentStatus = 99) "+
                     " order by a.CurrencyDate,a.CurrencyID";
                string query6 = "select a.rowid, a.CurrencyDate, a.CurrencyID,b.Name,a.Type,a.INRValue,a.Status,a.DocumentStatus,isnull(a.CreateUser,' '),isnull(a.ForwardUser,' '),isnull(a.ApproveUser,' '),ForwarderList " +
                  " from CurrencyVsINR a, Currency b where a.CurrencyID=b.CurrencyID " +
                  " and " +

                   " a.DocumentStatus = 99  and a.Status = 1"+
                    " order by a.CurrencyDate,a.CurrencyID";
                SqlConnection conn = new SqlConnection(Login.connString);
                ////string query = "select a.rowid, a.CurrencyDate, a.CurrencyID,b.Name,a.Type,a.INRValue,a.Status,a.DocumentStatus,isnull(a.CreateUser,' '),isnull(a.ForwardUser,' '),isnull(a.ApproveUser,' ') " +
                ////    " from CurrencyVsINR a, Currency b where a.CurrencyID=b.CurrencyID " +
                ////    " and a.Status=0 " +
                ////    " and ((a.forwardUser in (" + userList + ") and documentstatus between 2 and 98) " + 
                ////    " or (a.createuser='" + Login.userLoggedIn + "' and DocumentStatus=1))" +
                ////    " order by a.CurrencyDate,a.CurrencyID";
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
                    cvi = new cvinr();
                    cvi.rowID = reader.GetInt32(0);
                    cvi.ConversionDate = reader.GetDateTime(1);
                    cvi.CurrencyID = reader.GetString(2);
                    cvi.CurrencyName = reader.GetString(3);
                    cvi.type = reader.GetInt32(4);
                    cvi.INRValue = (float)reader.GetDouble(5);
                    cvi.status = reader.GetInt32(6);
                    cvi.documentStatus = reader.GetInt32(7);
                    cvi.CreateUser = reader.GetString(8);
                    cvi.ForwardUser = reader.GetString(9);
                    cvi.ApproveUser = reader.GetString(10);
                    if (!reader.IsDBNull(11))
                    {
                        cvi.ForwarderList = reader.GetString(11);
                    }
                    else
                    {
                        cvi.ForwarderList = "";
                    }
                   
                    CVINRs.Add(cvi);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return CVINRs;

        }
        public Boolean updateCVINR(cvinr cvi, cvinr prevcvi)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update CurrencyVsINR set CurrencyDate='" + cvi.ConversionDate.ToString("yyyyMMdd") +
                    "', CurrencyID='" + cvi.CurrencyID +
                    "', Type=" + cvi.type +
                    ", INRValue=" + cvi.INRValue +
                    ", ForwarderList='" + cvi.ForwarderList +"'"+
                    " where CurrencyDate='" + prevcvi.ConversionDate.ToString("yyyyMMdd") +
                    "' and Type=" + prevcvi.type +
                    " and CurrencyID='" + prevcvi.CurrencyID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "CurrencyVsINR", "", updateSQL) +
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
        public Boolean insertCVINR(cvinr cvi)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into CurrencyVsINR " +
                    " (CurrencyDate,CurrencyID,Type,INRValue,Status,DocumentStatus,CreateTime,CreateUser,ForwarderList)" +
                    "values (" +
                    "'" + cvi.ConversionDate.ToString("yyyyMMdd") + "'," +
                    "'" + cvi.CurrencyID + "'," +
                    cvi.type + "," +
                    cvi.INRValue + "," +
                    cvi.status + "," +
                    cvi.documentStatus + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'," +
                    "'" + cvi.ForwarderList + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "CurrencyVsINR", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean validateCVINR(cvinr cvi)
        {
            Boolean status = true;
            try
            {
                if (cvi.CurrencyID.Trim().Length == 0 || cvi.CurrencyID == null)
                {
                    return false;
                }
                if (cvi.type == 0)
                {
                    return false;
                }
                if (cvi.ConversionDate > DateTime.Now || cvi.ConversionDate == null)
                {
                    return false;
                }
                if (cvi.INRValue <= 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean forwardCVINR(cvinr prevcvi)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update CurrencyVsINR set documentstatus=" + (prevcvi.documentStatus + 1) +
                    ", forwardUser='" + prevcvi.ForwardUser + "'" +
                    ", ForwarderList='" + prevcvi.ForwarderList + "'" +
                    " where CurrencyDate='" + prevcvi.ConversionDate.ToString("yyyyMMdd") +
                    "' and Type=" + prevcvi.type +
                    " and CurrencyID='" + prevcvi.CurrencyID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("forward", "CurrencyVsINR", "", updateSQL) +
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
        public Boolean ApproveCVINR(cvinr prevcvi)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update CurrencyVsINR set documentstatus=99" +
                    ", status=1" +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    " where CurrencyDate='" + prevcvi.ConversionDate.ToString("yyyyMMdd ") +
                    "' and Type=" + prevcvi.type +
                    " and CurrencyID='" + prevcvi.CurrencyID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "CurrencyVsINR", "", updateSQL) +
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
        public Boolean reverseCVINR(cvinr prevcvi)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update CurrencyVsINR set DocumentStatus=" + prevcvi.documentStatus +
                    ", forwardUser='" + prevcvi.ForwardUser + "'" +
                    ", ForwarderList='" + prevcvi.ForwarderList + "'" +
                    " where CurrencyDate='" + prevcvi.ConversionDate.ToString("yyyyMMdd") +
                    "' and Type=" + prevcvi.type +
                    " and CurrencyID='" + prevcvi.CurrencyID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "CurrencyVsINR", "", updateSQL) +
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
        public static string getINRRate(DateTime cdate, string cid )
        {
            string rstr = "";

            return rstr;
        }
    }
}
