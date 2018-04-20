using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class bankbranch
    {
        public int BranchID { get; set; }
        public string BankID { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string IFSCCode { get; set; }
        public string SWIFTCode { get; set; }
        public string MICRCode { get; set; }
        public string BSRCode { get; set; }
        public int status { get; set; }
        public DateTime Createime { get; set; }
        public string CreateUser { get; set; }
    }

    class BankBranchDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<bankbranch> getBankBranches()
        {
            bankbranch branch;
            List<bankbranch> BankBranches = new List<bankbranch>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.RowID, a.BankID, b.Description BankName,a.BranchName,a.Address1,a.Address2,a.Address3,a.IFSCCOde,a.SWIFTCode,a.MICRCode,a.BSRCode,a.status " +
                    " from BankBranch a, CatalogueValue b" +
                    " where a.BankID=b.CatalogueValueID and b.CatalogueID='Bank' " +
                    " order by a.BankID,a.BranchName";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    branch = new bankbranch();
                    branch.BranchID = reader.GetInt32(0);
                    branch.BankID = reader.GetString(1);
                    branch.BankName = reader.GetString(2);
                    branch.BranchName = reader.GetString(3);
                    branch.Address1 = reader.GetString(4);
                    branch.Address2 = reader.GetString(5);
                    branch.Address3 = reader.GetString(6);
                    branch.IFSCCode = reader.GetString(7);
                    branch.SWIFTCode =reader.IsDBNull(8)?"": reader.GetString(8);
                    branch.MICRCode = reader.GetString(9);
                    branch.BSRCode = reader.GetString(10);
                    branch.status = reader.GetInt32(11);
                    BankBranches.Add(branch);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return BankBranches;
        }

        public Boolean updateBankBranch(bankbranch branch)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update BankBranch set BankID='" + branch.BankID +
                    "', BranchName='" + branch.BranchName +
                    "', Address1='" + branch.Address1 +
                    "', Address2='" + branch.Address2 +
                    "', Address3='" + branch.Address3 +
                    "', IFSCCode='" + branch.IFSCCode +
                     "', SWIFTCode='" + branch.SWIFTCode +
                    "', MICRCode='" + branch.MICRCode +
                    "', BSRCode='" + branch.BSRCode +
                    "',Status=" + branch.status +
                    " where rowID=" + branch.BranchID ;
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("update", "BankBranch", "", updateSQL) +
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
        public Boolean insertBankBranch(bankbranch branch)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into BankBranch (BankID,BranchName,Address1,Address2,Address3,IFSCCode,SWIFTCode,MICRCOde,BSRCode,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + branch.BankID + "'," +
                    "'" + branch.BranchName + "'," +
                    "'" + branch.Address1 + "'," +
                    "'" + branch.Address2 + "'," +
                    "'" + branch.Address3 + "'," +
                    "'" + branch.IFSCCode + "'," +
                     "'" + branch.SWIFTCode + "'," +
                    "'" + branch.MICRCode + "'," +
                    "'" + branch.BSRCode + "'," +
                    branch.status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                   ActivityLogDB.PrepareActivityLogQquerString("insert", "BankBranch", "", updateSQL) +
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
        public Boolean validateBankBranch(bankbranch branch)
        {
            Boolean status = true;
            try
            {
                if (branch.BankID.Trim().Length == 0 || branch.BankID == null)
                {
                    return false;
                }
                if (branch.BranchName.Trim().Length == 0 || branch.BranchName == null)
                {
                    return false;
                }
                //////if (branch.IFSCCode.Trim().Length >11)
                //////{
                //////    return false;
                //////}
                //////if (branch.MICRCode.Trim().Length > 11)
                //////{
                //////    return false;
                //////}
                //////if (branch.BSRCode.Trim().Length > 7)
                //////{
                //////    return false;
                //////}
                //////if(branch.SWIFTCode.Trim().Length <8 || branch.SWIFTCode.Trim().Length>11)
                //////{
                //////    return false;
                //////}
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                return false;
            }
            return status;
        }
        public static string fillBankBranchCombo(DataGridViewComboBoxCell cmb,string bank)
        {
            string firstValue = "";
            ////cmb.Items.Clear();
            try
            {
                BankBranchDB bankbranchdb = new BankBranchDB();
                List<bankbranch> BankBranches = bankbranchdb.getBankBranches();
                foreach (bankbranch branch in BankBranches)
                {
                    if (branch.BankID.Equals(bank) && branch.status == 1)
                    {

                        cmb.Items.Add(branch.BranchID + "-" + branch.BranchName);
                        if (firstValue.Length == 0)
                        {
                            firstValue = branch.BranchID + "-" + branch.BranchName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            return firstValue;
        }
        public static string fillBankBranchGridViewComboNew(DataGridViewComboBoxCell cmb, string bank)
        {
            string firstValue = "";
            ////cmb.Items.Clear();
            try
            {
                cmb.Items.Clear();
                BankBranchDB bankbranchdb = new BankBranchDB();
                List<bankbranch> BankBranches = bankbranchdb.getBankBranches();
                List<Structures.GridViewComboBoxItem> ItemList =
                    new List<Structures.GridViewComboBoxItem>();
                foreach (bankbranch branch in BankBranches)
                {
                    if (branch.BankID.Equals(bank) && branch.status == 1)
                    {
                        Structures.GridViewComboBoxItem ch =
                           new Structures.GridViewComboBoxItem(branch.BranchName, branch.BranchID.ToString());
                        cmb.Items.Add(ch);
                        //cmb.Items.Add(branch.BankID + "-" + branch.BranchName);
                        if (firstValue.Length == 0)
                        {
                            firstValue = branch.BranchID.ToString();// + "-" + branch.BranchName;
                        }
                    }
                }
                cmb.DisplayMember = "Name";  // Name Property will show(Editing)
                cmb.ValueMember = "Value";  // Value Property will save(Saving)
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return firstValue;
        }
        public bankbranch getBankBrancheDetails(string bankID,string branchName)
        {
            bankbranch branch = new bankbranch();
            
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.RowID, a.BankID, b.Description BankName,a.BranchName,a.Address1,a.Address2,a.Address3,a.IFSCCOde,a.MICRCode,a.BSRCode,a.status " +
                    " from BankBranch a, CatalogueValue b" +
                    " where a.BankID=b.CatalogueValueID and b.CatalogueID='Bank' " +
                    " and a.BankID='" + bankID + "' and a.BranchName='" + branchName + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    branch = new bankbranch();
                    branch.BranchID = reader.GetInt32(0);
                    branch.BankID = reader.GetString(1);
                    branch.BankName = reader.GetString(2);
                    branch.BranchName = reader.GetString(3);
                    branch.Address1 = reader.GetString(4);
                    branch.Address2 = reader.GetString(5);
                    branch.Address3 = reader.GetString(6);
                    branch.IFSCCode = reader.GetString(7);
                    branch.MICRCode = reader.GetString(8);
                    branch.BSRCode = reader.GetString(9);
                    branch.status = reader.GetInt32(10);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return branch;
        }
        public static ListView getBankDetailListView()
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
                BankBranchDB bankDB = new BankBranchDB();
                List<bankbranch> BankList = bankDB.getBankBranches();
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Branch Id", -2, HorizontalAlignment.Left); 
                lv.Columns.Add("Branch Name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Bank Id", -2, HorizontalAlignment.Left);
                foreach (bankbranch bank in BankList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(bank.BranchID.ToString());
                    item1.SubItems.Add(bank.BranchName.ToString());
                    item1.SubItems.Add(bank.BankID.ToString());
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
