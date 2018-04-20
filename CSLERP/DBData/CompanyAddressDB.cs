using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    //public strin "Data Source = PRAKASHPC; Initial Catalog = newERP; Integrated Security = True"
    class companyaddress
    {
        public int RowID { get; set; }
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public int AddressType { get; set; }
        public string Address { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }

    class CompanyAddressDB
    {
        public  List<companyaddress> getCompAddList()
        {
            companyaddress ca;
            List<companyaddress> AddList = new List<companyaddress>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.RowID,a.CompanyID, b.CompanyName,a.AddressType,a.Address,a.Status,a.CreateTime,a.CreateUser "+
                    "from CompanyAddress a,CompanyDetail b where a.CompanyID = b.CompanyID order by a.CompanyID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ca = new companyaddress();
                    ca.RowID = reader.GetInt32(0);
                    ca.CompanyID = reader.GetInt32(1);
                    ca.CompanyName = reader.GetString(2);
                    ca.AddressType = reader.GetInt32(3);
                    ca.Address = reader.GetString(4);
                    ca.Status = reader.GetInt32(5);
                    ca.CreateTime = reader.GetDateTime(6);
                    ca.CreateUser = reader.GetString(7);

                    AddList.Add(ca);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Company Address");
            
            }
            return AddList;
            
        }
 
        public Boolean updateCompAddress(companyaddress ca )
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update CompanyAddress set AddressType = " + ca.AddressType +
                    " ,Address = '" +ca.Address.Replace("'","''") +"',Status="+ ca.Status+
                    " where RowID=" + ca.RowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("update", "CompanyAddress", "", updateSQL) +
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
        public Boolean insertCompAddress(companyaddress ca)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into CompanyAddress (CompanyID,AddressType,Address,"+
                    "Status,CreateTime,CreateUser)" +
                    " values (" +
                     ca.CompanyID + "," +
                     ca.AddressType + "," +
                     "'" + ca.Address + "'," +
                    ca.Status+","+
                    "GETDATE()"+","+
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("insert", "CompanyAddress", "", updateSQL) +
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
        public Boolean validateCompanyAdd(companyaddress ca)
        {
            Boolean status = true;
            try
            {
                if (ca.CompanyID == 0)
                {
                    return false;
                }
                if (ca.AddressType == 0)
                {
                    return false;
                }
                if (ca.Address.Trim().Length == 0 || ca.Address == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {

            }
            return status;
        }
        //public static void fillStateComboNew(System.Windows.Forms.ComboBox cmb)
        //{
        //    cmb.Items.Clear();
        //    try
        //    {
        //        StateDB sdb = new StateDB();
        //        List<state> stList = sdb.getStateList();
        //        foreach (state stat in stList)
        //        {
        //            if (stat.Status == 1)
        //            {
        //                Structures.ComboBoxItem cbitem =
        //                    new Structures.ComboBoxItem(stat.StateName, stat.StateCode);
        //                cmb.Items.Add(cbitem);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
        //    }
        //}
        public static ListView getCustomerAddListView(int AddType)
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
                CompanyAddressDB CADb = new CompanyAddressDB();
                List<companyaddress> AddList = CADb.getCompAddList();
                string col = "";
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Company", -2, HorizontalAlignment.Left);
                if (AddType == 1)
                    col = "Delivery";
                else if (AddType == 2)
                    col = "Billing";
                lv.Columns.Add(col+" Address", -2, HorizontalAlignment.Left);
                foreach (companyaddress ca in AddList)
                {
                    if (ca.AddressType == AddType && ca.Status == 1)
                    {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        item1.SubItems.Add(ca.CompanyName);
                        item1.SubItems.Add(ca.Address);
                        lv.Items.Add(item1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lv;
        }

        public static string[] getCompTopBillingAdd(int compID)
        {
            string[] add = new string[2];
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select b.CompanyName,a.Address " +
                    "from CompanyAddress a , CompanyDetail b "+
                    "where a.CompanyID = b.CompanyID and a.CompanyID = "+ compID + " and a.AddressType = 2 and a.Status = 1 order by a.CreateTime asc";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    add[0] = reader.IsDBNull(0) ? "" : reader.GetString(0);
                    add[1] = reader.IsDBNull(1) ? "" : reader.GetString(1);
                    break;
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Company Address");

            }
            return add;

        }
    }
}
