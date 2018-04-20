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
    class companybank
    {
        public int RowID { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string AccountType { get; set; }
        public string AccountCode { get; set; }
        public string BankID { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public int branchID { get; set; }
        public int Status { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
    }

    class CompanyBankDB
    {
        public List<companybank> getCompBankList()
        {
            companybank ca;
            List<companybank> AddList = new List<companybank>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select  a.CompanyID,e.CompanyName,b.BankID,b.BranchName, a.AccountType , " +
                                "a.AccountCode,a.CreateUser,a.CreateTime,a.RowID,a.BranchID,a.Status from CompanyBank a," +
                                " BankBranch b, CompanyDetail e ,CatalogueValue d  where a.BranchID = b.RowID and " +
                                 " a.AccountType = d.CatalogueValueID and a.CompanyID = e.CompanyID order by a.CompanyID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ca = new companybank();
                    ca.CompanyID = Convert.ToString(reader.GetInt32(0));
                    ca.CompanyName = reader.GetString(1);
                    ca.BankID = reader.GetString(2);
                    ca.BranchName = reader.GetString(3);
                    ca.AccountType = reader.GetString(4);
                    ca.AccountCode = reader.GetString(5);
                    ca.CreateTime = reader.GetDateTime(7);
                    ca.CreateUser = reader.GetString(6);
                    ca.RowID = reader.GetInt32(8);
                    ca.branchID = reader.GetInt32(9);
                    ca.Status = reader.GetInt32(10);
                    AddList.Add(ca);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Company Bank");
            }
            return AddList;
        }
        public List<companybank> getBranch(string bank)
        {
            companybank det;
            List<companybank> Details = new List<companybank>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select RowID, BranchName " +
                    "from BankBranch where BankID='" + bank + "' and Status = 1 order by RowID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    det = new companybank();
                    det.RowID = reader.GetInt32(0);
                    det.BranchName = reader.GetString(1);
                    Details.Add(det);
                }
                conn.Close();
            }
            catch (Exception ex)
            {

            }
            return Details;
        }

        public Boolean updateCompBAnk(companybank ca)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update CompanyBank set BranchID = " + ca.BranchName +
                    " ,AccountType = '" + ca.AccountType + "',AccountCode='" + ca.AccountCode +
                    "',Status=" + ca.Status +
                    " where RowID=" + ca.RowID;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("update", "CompanyBank", "", updateSQL) +
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
        public Boolean insertCompBank(companybank ca)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into CompanyBank (CompanyID,BranchID,AccountType," +
                    "AccountCode,Status,CreateTime,CreateUser)" +
                    " values (" +
                     ca.CompanyID + "," +
                     ca.BranchName + "," +
                     "'" + ca.AccountType + "','" +
                    ca.AccountCode + "'," +
                    ca.Status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("insert", "CompanyBank", "", updateSQL) +
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
        public static void fillBranchCombo(System.Windows.Forms.ComboBox cmb, string bank)
        {
            cmb.Items.Clear();
            try
            {
                CompanyBankDB dbrecord = new CompanyBankDB();
                List<companybank> details = dbrecord.getBranch(bank);
                foreach (companybank det in details)
                {
                    //cmb.Items.Add(det.companyID + "-" + det.companyname);
                    Structures.ComboBoxItem cbitem =
                       new Structures.ComboBoxItem(det.BranchName, det.RowID.ToString());
                    cmb.Items.Add(cbitem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        public Boolean validateCompanyBank(companybank ca)
        {
            Boolean status = true;
            try
            {
                if (ca.CompanyID.Trim().Length == 0 || ca.CompanyID == null)
                {
                    return false;
                }
                if (ca.BranchName.Trim().Length == 0 || ca.BranchName == null)
                {
                    return false;
                }
                if (ca.AccountType.Trim().Length == 0 || ca.AccountType == null)
                {
                    return false;
                }
                if (ca.AccountCode.Trim().Length == 0 || ca.AccountCode == null)
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
                lv.Columns.Add(col + " Address", -2, HorizontalAlignment.Left);
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
                    "from CompanyAddress a , CompanyDetail b " +
                    "where a.CompanyID = b.CompanyID and a.CompanyID = " + compID + " and a.AddressType = 2 and a.Status = 1 order by a.CreateTime asc";
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

        public static companybank getCompBankDetailForIOPrint(int CompbankRowID)
        {
            companybank ca = new companybank();

            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select b.CompanyName,c.BankID,e.Description,c.BranchName,d.Description,a.AccountCode ,c.IFSCCode,c.SWIFTCode " +
                                "from CompanyBank a, CompanyDetail b , BankBranch c , CatalogueValue d, CatalogueValue e " +
                                "where a.CompanyID = b.CompanyID and a.BranchID = c.RowID and " +
                                " a.AccountType = d.CatalogueValueID and c.BankID = e.CatalogueValueID and a.RowID = " + CompbankRowID;
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ca.CompanyName = reader.GetString(0);
                    ca.BankID = reader.GetString(1);
                    ca.BankName = reader.GetString(2);
                    ca.BranchName = reader.GetString(3);
                    ca.AccountType = reader.GetString(4);
                    ca.AccountCode = reader.GetString(5);
                    ca.CreateUser = reader.IsDBNull(6) ? "":reader.GetString(6); // For IFSC Code
                    ca.CompanyID = reader.IsDBNull(7) ? "" : reader.GetString(7);  // for SWIFT Code
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Company Bank");
            }
            return ca;
        }
        public List<companybank> getCompBankListForIO(int compID)
        {
            companybank ca;
            List<companybank> AddList = new List<companybank>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select  a.CompanyID,e.CompanyName,b.BankID,b.BranchName, a.AccountType , " +
                                "a.AccountCode,a.CreateUser,a.CreateTime,a.RowID,a.BranchID,a.Status from CompanyBank a," +
                                " BankBranch b, CompanyDetail e ,CatalogueValue d  where a.BranchID = b.RowID and " +
                                 " a.AccountType = d.CatalogueValueID and a.CompanyID = e.CompanyID and a.CompanyID = " + compID +
                                 " and b.Status = 1 and a.AccountType = 'CurrentA/c' and a.Status = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ca = new companybank();
                    ca.CompanyID = Convert.ToString(reader.GetInt32(0));
                    ca.CompanyName = reader.GetString(1);
                    ca.BankID = reader.GetString(2);
                    ca.BranchName = reader.GetString(3);
                    ca.AccountType = reader.GetString(4);
                    ca.AccountCode = reader.GetString(5);
                    ca.CreateTime = reader.GetDateTime(7);
                    ca.CreateUser = reader.GetString(6);
                    ca.RowID = reader.GetInt32(8);
                    ca.branchID = reader.GetInt32(9);
                    ca.Status = reader.GetInt32(10);
                    AddList.Add(ca);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Company Bank");
            }
            return AddList;
        }
        public static ListView getBankListView(int CompID)
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
                CompanyBankDB cadb = new CompanyBankDB();
                List<companybank> caList = cadb.getCompBankListForIO(CompID);
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Row Id", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Bank Id", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Branch Name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Account Type", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Account Code", -2, HorizontalAlignment.Left);
                //lv.Columns[1].Width = 0;
                foreach (companybank ca in caList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(ca.RowID.ToString());
                    item1.SubItems.Add(ca.BankID.ToString());
                    item1.SubItems.Add(ca.BranchName.ToString());
                    item1.SubItems.Add(ca.AccountType.ToString());
                    item1.SubItems.Add(ca.AccountCode.ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lv;
        }
    }
}
