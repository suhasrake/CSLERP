using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class storeempmapping
    {
        public string StoreLocationID { get; set; }
        public string Description { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public int Status { get; set; }
        public DateTime Createime { get; set; }
        public string CreateUser { get; set; }
    }

    class StoreEmpMappingDB
    {
        //ActivityLogDB alDB = new ActivityLogDB();
        public List<storeempmapping> getStockEmpMapping()
        {
            storeempmapping sem;
            List<storeempmapping> SEMs = new List<storeempmapping>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.StoreLocationID, b.Description,a.EmployeeID,c.Name,a.status " +
                    "from StoreEmpMapping as a  " +
                    "LEFT OUTER JOIN CatalogueValue as b on a.StoreLocationID = b.CatalogueValueID " +
                    "LEFT OUTER JOIN Employee as c on a.EmployeeID=c.EmployeeID " +
                    "order by a.StoreLocationID,c.Name";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sem = new storeempmapping();
                    sem.StoreLocationID = reader.GetString(0);
                    sem.Description = reader.GetString(1);
                    sem.EmployeeID = reader.GetString(2);
                    sem.EmployeeName = reader.GetString(3);
                    sem.Status = reader.GetInt32(4);
                    SEMs.Add(sem);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error Loading StockEmpMappings");
            }
            return SEMs;
        }

        public Boolean updateStockEmpMapping(storeempmapping sem)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update StoreEmpMapping set Status = " + sem.Status +
                    " where StoreLocationID='" + sem.StoreLocationID + "'" +
                    " and EmployeeID='" + sem.EmployeeID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "StoreEmpMapping", "", updateSQL) +
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
        public Boolean insertStockEmpMapping(storeempmapping sem)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into StoreEmpMapping (StoreLocationID,EmployeeID,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + sem.StoreLocationID + "'," +
                    "'" + sem.EmployeeID + "'," +
                    sem.Status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "StoreEmpMapping", "", updateSQL) +
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
        public Boolean validateDocument(storeempmapping sem)
        {
            Boolean status = true;
            try
            {
                if (sem.StoreLocationID.Trim().Length == 0 || sem.StoreLocationID == null)
                {
                    return false;
                }
                if (sem.EmployeeID.Trim().Length == 0 || sem.EmployeeID == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {

            }
            return status;
        }
        public static void fillLocationCombo(ComboBox cmb)
        {
            StoreEmpMappingDB smpdb = new StoreEmpMappingDB();
            List<storeempmapping> LocList = smpdb.getStockEmpMapping();
            foreach (storeempmapping smp in LocList)
            {
                Boolean status = true;
                foreach (string itm in cmb.Items)
                {
                    if (itm == smp.StoreLocationID + "-" + smp.Description)
                    {
                        status = false;
                    }     
                }
                if(status)
                    cmb.Items.Add(smp.StoreLocationID + "-" + smp.Description);
            }
        }
        public static void fillLocationComboNew(ComboBox cmb)
        {
            StoreEmpMappingDB smpdb = new StoreEmpMappingDB();
            List<storeempmapping> LocList = smpdb.getStockEmpMapping();
            foreach (storeempmapping smp in LocList)
            {
                Boolean status = true;
                foreach (Structures.ComboBoxItem itm in cmb.Items)
                {
                    if ((itm.HiddenValue+"-"+itm.ToString()).Equals( smp.StoreLocationID + "-" + smp.Description))
                    {
                        status = false;
                    }
                }
                if (status)
                {
                    Structures.ComboBoxItem cbitem =
                           new Structures.ComboBoxItem(smp.Description, smp.StoreLocationID);
                    cmb.Items.Add(cbitem);
                    //cmb.Items.Add(smp.StoreLocationID + "-" + smp.Description);
                }
            }
        }

        public static Boolean varifyLocationEmpMapping(string loc, string EmpID)
        {
            Boolean status = false;
            StoreEmpMappingDB smpdb = new StoreEmpMappingDB();
            List<storeempmapping> LocList = smpdb.getStockEmpMapping();
            foreach (storeempmapping smp in LocList)
            {
                if (smp.StoreLocationID == loc)
                {
                    if (smp.EmployeeID == EmpID)
                    {
                        return true;
                    }
                }
            }
            return status;
        }
        //public static void fillFromLocationCombo(ComboBox cmb, string empID)
        //{
        //    StoreEmpMappingDB smpdb = new StoreEmpMappingDB();
        //    List<storeempmapping> LocList = smpdb.getStockEmpMapping();
        //    foreach (storeempmapping smp in LocList)
        //    {
        //        if (smp.EmployeeID == empID)
        //        {
        //            cmb.Items.Add(smp.StoreLocationID + "-" + smp.Description);
        //        }
        //    }
        //}
        public static ListView ListStoreMappingEmp(string toLoc)
        {
            ListView lv = new ListView();
            try
            {
                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                ////DocCommenterDB doccmdb = new DocCommenterDB();
                ////List<doccommenter> DocCommenters = doccmdb.getDocCommList();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("EmployeeID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("userID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Name", -2, HorizontalAlignment.Left);
                lv.Columns[1].Width = 150;
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.EmployeeID, b.userID,b.Name from StoreEmpMapping a, ViewUserEmployeeList b" +
                    " where a.EmployeeID = b.EmployeeID and a.StoreLocationID = '" + toLoc + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(reader.GetString(0));
                    item1.SubItems.Add(reader.GetString(1));
                    item1.SubItems.Add(reader.GetString(2));
                    lv.Items.Add(item1);
                }
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return lv;
        }
        public static Boolean CheckAcceptUser(string empID,string toLoc)
        {
            Boolean status = false;
            ListView lv = ListStoreMappingEmp(toLoc);
            foreach (ListViewItem itemRow in lv.Items)
            {
                if (empID == itemRow.SubItems[1].Text)
                    return true;
            }
            return status;
        }
    }
}

