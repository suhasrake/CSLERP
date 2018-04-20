using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class UpdateTable
    {
        public static Boolean UT(string QueryStrings)
        {
            Boolean status = true;
            try
            {
                string updateSQL;
                string[] queries = QueryStrings.Split(new string[] { Main.QueryDelimiter }, StringSplitOptions.None);
                if (queries.Length > 0)
                {
                    SqlTransaction Trans;
                    SqlConnection conn = new SqlConnection(Login.connString);
                    conn.Open();
                    Trans = conn.BeginTransaction();
                    try
                    {
                        for (int i = 0; i < queries.Length; i++)
                        {
                            if (queries[i].Trim().Length > 0)
                            {
                                updateSQL = queries[i];
                                SqlCommand cmd = new SqlCommand(updateSQL, conn, Trans);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        Trans.Commit();
                        Trans.Dispose();
                        conn.Close();
                        conn.Dispose();
                    }
                    catch (Exception ex)
                    {
                        status = false;
                        Trans.Rollback();
                        MessageBox.Show("UT() : Failed");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("UT() : Failed");
                status = false;
            }

            return status;
        }
        public static Boolean UTSingleQuery(string updateSQL)
        {
            Boolean status = true;
            try
            {
                GC.Collect();
                SqlTransaction Trans;
                SqlConnection conn = new SqlConnection(Login.connString);
                conn.Open();
                Trans = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand(updateSQL, conn, Trans);
                    cmd.ExecuteNonQuery();
                    Trans.Commit();
                    Trans.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
                catch (Exception ex)
                {
                    status = false;
                    Trans.Rollback();
                    MessageBox.Show("UTSingleQuery() : Failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("UTSingleQuery() : Failed");
                status = false;
            }
            return status;
        }
        public static DateTime getSQLDateTime()
        {
            DateTime dtnow = DateTime.Now;
            try
            {
                SqlTransaction Trans;
                SqlConnection conn = new SqlConnection(Login.connString);
                string querySQL = "select getdate()";
                conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(querySQL, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    dtnow = reader.GetDateTime(0);
                }
                conn.Close();
            }
            catch (Exception)
            {
                
            }
            return dtnow;
        }
        public static string getLatestVersionOfERP()
        {
            string version = "";
            string location = "";
            try
            {
                SqlTransaction Trans;
                SqlConnection conn = new SqlConnection(Login.connString);
                string querySQL = "select VersionNo,Location from VersionDetail where VersionDateTime =(select MAX(VersionDateTime) from VersionDetail)";
                conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(querySQL, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    version = reader.GetString(0);
                    location = reader.GetString(1);
                }
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return version + Main.delimiter1 + location;
        }
    }
}
