using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace CSLERP.DBData
{
    public class systemparam
    {
        public int rowid { get; set; }
        public string ID { get; set; }
        public string Value { get; set; }
        public string description { get; set; }
    }

    class SystemParametersDB
    {
        public List<systemparam> getSystemparameters()
        {
            systemparam br;
            List<systemparam> systemparam = new List<systemparam>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select Rowid,ID,Value,description from SystemParameters ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    br = new systemparam();
                    br.rowid = reader.GetInt32(0);
                    br.ID = reader.GetString(1);
                    br.Value = reader.GetString(2);
                    br.description = reader.GetString(3);
                    systemparam.Add(br);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return systemparam;
        }
        
        public Boolean updateSystemParam(systemparam sp)
        {
            Boolean status = true;
            string utString = "";
            string date = "";
            try
            {
                string updateSQL = "update SystemParameters set Value='" + sp.Value + "', " +
                    " Description ='" + sp.description + "' where " +
                      " RowID='" + sp.rowid + "' and ID='"+sp.ID+"'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "SystemParameters", "", updateSQL) +
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

        public Boolean insertSyatemParam(systemparam sp)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into SystemParameters " +
                    "(ID,Value,Description)" +
                    " values (" +
                    "'" + sp.ID + "'," +
                     "'" + sp.Value + "'," +
                    "'" + sp.description + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "SystemParameters", "", updateSQL) +
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

        public Boolean validateSyetemParam(systemparam sp)
        {
            Boolean status = true;
            try
            {
                if (sp.ID.Trim().Length == 0 || sp.ID == null)
                {
                    return false;
                }
                if (sp.Value.Trim().Length == 0 || sp.Value == null)
                {
                    return false;
                }
                if (sp.description.Trim().Length == 0 || sp.description == null)
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

        public string getSystemparametersforID(string id)
        {
            string value = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select Value from SystemParameters where ID='"+id+"'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    value = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return value;
        }
    }
}
