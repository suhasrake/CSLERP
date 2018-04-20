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
    class menugroup
    {
        public int rowid { get; set; }
        public string MenuGroup { get; set; }
        public int status { get; set; }
    }

    class MenuGroupDB
    {
        public List<menugroup> getMenuGroup()
        {
            menugroup br;
            List<menugroup> MenuGroup = new List<menugroup>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select rowid,MenuGroup,status from MenuGroup ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    br = new menugroup();
                    br.rowid = reader.GetInt32(0);
                    br.MenuGroup = reader.GetString(1);
                    br.status = reader.GetInt32(2);
                    MenuGroup.Add(br);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return MenuGroup;
        }





        public Boolean updateMenuGroup(menugroup mg)
        {
            Boolean status = true;
            string utString = "";
            string date = "";
            try
            {
                string updateSQL = "update MenuGroup set MenuGroup='" + mg.MenuGroup + "', " +
                    " Status='" + mg.status + "' where " +
                      " RowID='" + mg.rowid + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "MenuGroup", "", updateSQL) +
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




        public Boolean insertMenuGroup(menugroup mg)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into MenuGroup " +
                    "(MenuGroup,Status)" +
                    " values (" +
                    "'" + mg.MenuGroup + "'," +
                    "'" + mg.status + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "MenuGroup", "", updateSQL) +
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

        public Boolean validateMenuGroup(menugroup mg)
        {
            Boolean status = true;
            try
            {
                if (mg.MenuGroup.Trim().Length == 0 || mg.MenuGroup == null)
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
    }
}
