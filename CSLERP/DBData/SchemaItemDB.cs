using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    public class TableDetail
    {

        public string dbName { get; set; }
        public string tableName { get; set; }
        public string tableType { get; set; }

    }
    public class ColumnDetail
    {
        public string tableName { get; set; }
        public string columnName { get; set; }
        public string columnType { get; set; }
    }
    class SchemaItemsDB
    {
        public static List<TableDetail> getTableDetail()
        {
            TableDetail td;
            List<TableDetail> tdlist = new List<TableDetail>();
            try
            {
                string query = "select TABLE_CATALOG, TABLE_NAME,TABLE_TYPE " +
                    "from INFORMATION_SCHEMA.TABLES " +
                    "where TABLE_CATALOG='newERP' and TABLE_TYPE = 'BASE TABLE' order by TABLE_NAME";

                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    td = new TableDetail();
                    td.dbName = reader.GetString(0);
                    td.tableName = reader.GetString(1);
                    td.tableType = reader.GetString(2);

                    tdlist.Add(td);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return tdlist;
        }

        public static List<ColumnDetail> getColumnDetail()
        {
            ColumnDetail cd;
            List<ColumnDetail> cdlist = new List<ColumnDetail>();
            try
            {
                ////string query = "select TABLE_NAME,COLUMN_NAME,DATA_TYPE  from INFORMATION_SCHEMA.COLUMNS order by TABLE_NAME";
                string query = "select a.TABLE_NAME,a.COLUMN_NAME,a.DATA_TYPE,b.TABLE_TYPE from " +
                " INFORMATION_SCHEMA.COLUMNS as a   join " +
                " INFORMATION_SCHEMA.TABLES as b on(b.TABLE_NAME = a.TABLE_NAME " +
                " and b.TABLE_TYPE = 'BASE TABLE') order by a.TABLE_NAME ";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cd = new ColumnDetail();
                    cd.tableName = reader.GetString(0);
                    cd.columnName = reader.GetString(1);
                    cd.columnType = reader.GetString(2);

                    cdlist.Add(cd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return cdlist;
        }

        public static string getColumnValues(string tablename, string columnname, string serachstring)
        {
            string resultString = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "SELECT STUFF( (SELECT '(!@#' + CONVERT(varchar(10),RowID) + '(!@#' + " +
                       columnname +
                       " + '$%^*'  FROM " + tablename +
                       " where " + columnname +
                       " COLLATE Latin1_General_CS_AS like '%" + serachstring + "%' FOR XML PATH('')),1, 1, '')";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        resultString = reader.GetString(0);
                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                ////MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return resultString;
        }
        public static Boolean updateValueinDB(string tableName, string columnName, string valueText)
        {
            Boolean result = true;
            string updateSQL = "";
            string utString = "";
            string oldconnString = Login.connString;
            Login.connString = "Data Source=tcp:localhost;Database=newERP;User id=sa;Password=sasa;";
            try
            {

                updateSQL = "ALTER TABLE " + tableName + " NOCHECK CONSTRAINT ALL";
                utString = utString + updateSQL + Main.QueryDelimiter;

                string[] arr1 = valueText.Split(new string[] { "$%^*" }, StringSplitOptions.None);
                for (int i = 0; i < arr1.Length; i++)
                {
                    string[] arr2 = arr1[i].Split(new string[] { "(!@#", "!@#" }, StringSplitOptions.None);
                    if (arr2.Length == 3)
                    {
                        if ((arr2[1].Trim().Length > 0 && arr2[2].Trim().Length > 0))
                        {
                            updateSQL = "Update " + tableName + " set " + columnName + "= '" + arr2[2] + "' where RowID=" + arr2[1];
                            utString = utString + updateSQL + Main.QueryDelimiter;
                        }
                    }
                }
                updateSQL = "ALTER TABLE " + tableName + " CHECK CONSTRAINT ALL";
                utString = utString + updateSQL + Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    MessageBox.Show("Updation failed for table " + tableName + " Column " + columnName);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("updateValueinDB() : Updation query string creation failed for table " + tableName + " Column " + columnName);
                result = false;
            }
            Login.connString = oldconnString;
            return result;

        }
    }
}
