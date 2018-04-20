using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{


    class office
    {
        public string OfficeID { get; set; }
        public string name { get; set; }
        public string RegionID { get; set; }
        public string RegionName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public int status { get; set; }
        public DateTime Createime { get; set; }
        public string CreateUser { get; set; }
    }

    class OfficeDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<office> getOffices()
        {
            office off;
            List<office> Offices = new List<office>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.OfficeID, a.Name,a.RegionID,b.Name,a.Address1,a.Address2,a.Address3,a.Address4,a.status " +
                    "from Office a, Region b where a.RegionID=b.RegionID order by a.Name";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    off = new office();
                    off.OfficeID = reader.GetString(0);
                    off.name = reader.GetString(1);
                    off.RegionID = reader.GetString(2);
                    off.RegionName = reader.GetString(3);
                    off.Address1 = reader.GetString(4);
                    off.Address2 = reader.GetString(5);
                    off.Address3 = reader.GetString(6);
                    off.Address4 = reader.GetString(7);
                    off.status = reader.GetInt32(8);
                    Offices.Add(off);
                    
                    
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Office Data");

            }
            return Offices;

        }

        public Boolean updateOffice(office off)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update Office set name='" + off.name +
                    "', RegionID='" + off.RegionID +
                    "', Address1='" + off.Address1 +
                    "', Address2='" + off.Address2 +
                    "', Address3='" + off.Address3 +
                    "', Address4='" + off.Address4 +
                    "',Status=" + off.status +
                    " where OfficeID='" + off.OfficeID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("update", "Office", "", updateSQL) +
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
        public Boolean insertOffice(office off)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into Office (OfficeID,Name,RegionID,Address1,Address2,Address3,Address4,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + off.OfficeID + "'," +
                    "'" + off.name + "'," +
                    "'" + off.RegionID + "'," +
                    "'" + off.Address1 + "'," +
                    "'" + off.Address2 + "'," +
                    "'" + off.Address3 + "'," +
                    "'" + off.Address4 + "'," +
                    off.status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("insert", "Office", "", updateSQL) +
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
        public Boolean validateOffice(office off)
        {
            Boolean status = true;
            try
            {
                if (off.OfficeID.Trim().Length == 0 || off.OfficeID == null)
                {
                    return false;
                }
                if (off.RegionID.Trim().Length == 0 || off.RegionID == null)
                {
                    return false;
                }
                if (off.name.Trim().Length == 0 || off.name == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {
            }
            return status;
        }
        public static void fillOfficeCombo(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                OfficeDB officedb = new OfficeDB();
                List<office> Offices = officedb.getOffices();
                foreach (office off in Offices)
                {
                    if (off.status == 1)
                    {
                        cmb.Items.Add(off.OfficeID + "-" + off.name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
        }
        public static void fillOfficeComboNew(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                OfficeDB officedb = new OfficeDB();
                List<office> Offices = officedb.getOffices();
                foreach (office off in Offices)
                {
                    if (off.status == 1)
                    {
                        ////cmb.Items.Add(off.OfficeID + "-" + off.name);
                        Structures.ComboBoxItem cbitem =
                            new Structures.ComboBoxItem(off.name, off.OfficeID);
                        cmb.Items.Add(cbitem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        public static void fillOfficeIDGridViewCombo(DataGridViewComboBoxCell cmb, string CategoryName)
        {
            cmb.Items.Clear();
            try
            {
                OfficeDB officedb = new OfficeDB();
                List<office> Offices = officedb.getOffices();
                foreach (office off in Offices)
                {
                    if (off.status == 1)
                    {
                        cmb.Items.Add(off.OfficeID + "-" + off.name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
        }
        public static void fillOfficeIDGridViewComboNew(DataGridViewComboBoxCell cmb)
        {
            cmb.Items.Clear();
            try
            {
                OfficeDB officedb = new OfficeDB();
                List<office> Offices = officedb.getOffices();
                foreach (office off in Offices)
                {
                    if (off.status == 1)
                    {
                        Structures.GridViewComboBoxItem ch =
                            new Structures.GridViewComboBoxItem(off.name, off.OfficeID);
                        cmb.Items.Add(ch);
                        //cmb.Items.Add(off.OfficeID + "-" + off.name);
                    }
                }
                cmb.DisplayMember = "Name";  // Name Property will show(Editing)
                cmb.ValueMember = "Value";  // Value Property will save(Saving)
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        public List<office> getOfficesfromregion(string region)
        {
            office off;
            List<office> Offices = new List<office>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.OfficeID, a.Name,a.RegionID,b.Name,a.status " +
                    "from Office a, Region b where a.RegionID='" + region + "' and a.RegionID=b.RegionID order by a.RegionID,a.OfficeID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    off = new office();
                    off.OfficeID = reader.GetString(0);
                    off.name = reader.GetString(1);
                    off.RegionID = reader.GetString(2);
                    off.RegionName = reader.GetString(3);
                    off.status = reader.GetInt32(4);
                    Offices.Add(off);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Office Data");
            }
            return Offices;

        }
    }
}
