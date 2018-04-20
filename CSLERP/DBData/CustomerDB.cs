using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{

    class customer
    {
        public int rowID { get; set; }
        public string CustomerID { get; set; }
        public string name { get; set; }
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string TypeID { get; set; }
        public string TypeName { get; set; }
        public string CountryID { get; set; }
        public string CountryName { get; set; }
        public string StateCode { get; set; }
        public string StateName { get; set; }
        public string OfficeID { get; set; }
        public string OfficeName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string PinCode { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string EmailID { get; set; }
        public string WebSite { get; set; }
        public string ContactPerson { get; set; }
        public string ClientList { get; set; }
        public string BillingAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public string LedgerType { get; set; }
        public string ProductList { get; set; }
        public int DocumentStatus { get; set; }
        public int status { get; set; }
        public DateTime Createime { get; set; }
        public string CreateUser { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string Creator { get; set; }
        public string Forwarder { get; set; }
        public string Approver { get; set; }
        public string ForwarderList { get; set; }
    }

    class CustomerDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<customer> getCustomers(string userList, int opt)
        {
            customer cust;
            List<customer> Customers = new List<customer>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);

                string query1 = "select a.rowID,a.customerID, a.Name,a.status,a.DocumentStatus," +
                    "ISNULL(a.CreateUser, ' ') AS CreateUser, ISNULL(a.ForwardUser, ' ') AS ForwardUser,"+
                    " ISNULL(a.ApproveUser, ' ') AS ApproveUser,a.ForwarderList,a.LedgerType, " +
                    "ISNULL(b.Name, ' ') as Creator,ISNULL(c.Name, ' ') as Forwarder,ISNULL(d.Name, ' ') as Approver "+
                    "from Customer as a LEFT OUTER JOIN "+
                    "ViewUserEmployeeList as b on a.CreateUser = b.UserID LEFT OUTER JOIN "+
                    "ViewUserEmployeeList as c on a.ForwardUser = c.UserID LEFT OUTER JOIN "+
                    "ViewUserEmployeeList as d on a.ApproveUser = d.UserID "+
                    " where ((a.ForwardUser='" + Login.userLoggedIn + "' and a.DocumentStatus between 2 and 98) " +
                    " or (a.CreateUser='" + Login.userLoggedIn + "' and a.DocumentStatus=1) ) order by a.Name";
                string query2 = "select a.rowID,a.customerID, a.Name,a.status,a.DocumentStatus," +
                    "ISNULL(a.CreateUser, ' ') AS CreateUser, ISNULL(a.ForwardUser, ' ') AS ForwardUser," +
                    " ISNULL(a.ApproveUser, ' ') AS ApproveUser,a.ForwarderList,a.LedgerType, " +
                    "ISNULL(b.Name, ' ') as Creator,ISNULL(c.Name, ' ') as Forwarder,ISNULL(d.Name, ' ') as Approver " +
                    "from Customer as a LEFT OUTER JOIN " +
                    "ViewUserEmployeeList as b on a.CreateUser = b.UserID LEFT OUTER JOIN " +
                    "ViewUserEmployeeList as c on a.ForwardUser = c.UserID LEFT OUTER JOIN " +
                    "ViewUserEmployeeList as d on a.ApproveUser = d.UserID " +
                    " where ((a.createuser='" + Login.userLoggedIn + "'  and a.DocumentStatus between 2 and 98 ) " +
                    " or (a.ForwarderList like '%" + userList + "%' and a.DocumentStatus between 2 and 98 and a.ForwardUser <> '" + Login.userLoggedIn + "')) order by a.Name";
                string query3 = "select a.rowID,a.customerID, a.Name,a.status,a.DocumentStatus," +
                    "ISNULL(a.CreateUser, ' ') AS CreateUser, ISNULL(a.ForwardUser, ' ') AS ForwardUser," +
                    " ISNULL(a.ApproveUser, ' ') AS ApproveUser,a.ForwarderList,a.LedgerType, " +
                    "ISNULL(b.Name, ' ') as Creator,ISNULL(c.Name, ' ') as Forwarder,ISNULL(d.Name, ' ') as Approver " +
                    "from Customer as a LEFT OUTER JOIN " +
                    "ViewUserEmployeeList as b on a.CreateUser = b.UserID LEFT OUTER JOIN " +
                    "ViewUserEmployeeList as c on a.ForwardUser = c.UserID LEFT OUTER JOIN " +
                    "ViewUserEmployeeList as d on a.ApproveUser = d.UserID " +
                    " where ((a.createuser='" + Login.userLoggedIn + "'" +
                    " or a.ForwarderList like '%" + userList + "%'" +
                    " or a.approveUser='" + Login.userLoggedIn + "')" +
                    " and a.DocumentStatus = 99)  order by a.Name";
                string query6 = "select a.rowID,a.customerID, a.Name,a.status,a.DocumentStatus," +
                    "ISNULL(a.CreateUser, ' ') AS CreateUser, ISNULL(a.ForwardUser, ' ') AS ForwardUser," +
                    " ISNULL(a.ApproveUser, ' ') AS ApproveUser,a.ForwarderList,a.LedgerType, " +
                    "ISNULL(b.Name, ' ') as Creator,ISNULL(c.Name, ' ') as Forwarder,ISNULL(d.Name, ' ') as Approver " +
                    "from Customer as a LEFT OUTER JOIN " +
                    "ViewUserEmployeeList as b on a.CreateUser = b.UserID LEFT OUTER JOIN " +
                    "ViewUserEmployeeList as c on a.ForwardUser = c.UserID LEFT OUTER JOIN " +
                    "ViewUserEmployeeList as d on a.ApproveUser = d.UserID " +
                    " where  a.DocumentStatus = 99  order by a.Name";
                string query7 = "select a.rowID,a.customerID, a.Name,a.status,a.DocumentStatus," +
                    "ISNULL(a.CreateUser, ' ') AS CreateUser, ISNULL(a.ForwardUser, ' ') AS ForwardUser," +
                    " ISNULL(a.ApproveUser, ' ') AS ApproveUser,a.ForwarderList,a.LedgerType, " +
                    "ISNULL(b.Name, ' ') as Creator,ISNULL(c.Name, ' ') as Forwarder,ISNULL(d.Name, ' ') as Approver " +
                    "from Customer as a LEFT OUTER JOIN " +
                    "ViewUserEmployeeList as b on a.CreateUser = b.UserID LEFT OUTER JOIN " +
                    "ViewUserEmployeeList as c on a.ForwardUser = c.UserID LEFT OUTER JOIN " +
                    "ViewUserEmployeeList as d on a.ApproveUser = d.UserID " +
                    " where  a.DocumentStatus = 99  order by a.customerID";
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
                        break;;
                    case 6:
                        query = query6;
                        break;
                    case 7:
                        query = query7;
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
                    cust = new customer();
                    cust.rowID = reader.GetInt32(0);
                    cust.CustomerID = reader.GetString(1);
                    cust.name = reader.GetString(2);
                    cust.status = reader.GetInt32(3);
                    cust.DocumentStatus = reader.GetInt32(4);
                    cust.CreateUser = reader.GetString(5);
                    cust.ForwardUser = reader.IsDBNull(6) ? "" : reader.GetString(6);
                    cust.ApproveUser = reader.IsDBNull(7) ? "" : reader.GetString(7);
                    cust.ForwarderList = reader.IsDBNull(8) ? "" : reader.GetString(8);
                    cust.LedgerType = reader.IsDBNull(9) ? "" : reader.GetString(9);
                    cust.Creator = reader.IsDBNull(10) ? "" : reader.GetString(10);
                    cust.Forwarder = reader.IsDBNull(11) ? "" : reader.GetString(11);
                    cust.Approver = reader.IsDBNull(12) ? "" : reader.GetString(12);
                    Customers.Add(cust);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return Customers;

        }
        public static customer getCustomerDetails(string customerid)
        {
            customer cust=new customer();
          
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select rowID,customerID, Name,CustomerType,CustomerCategory,CustomerGroup, " +
                    "Address1,Address2,Address3,Address4,CountryID,OfficeID,Phone,Fax,EmailId,Website,"+
                    "ContactPerson,ClientList,BillingAddress,DeliveryAddress,DocumentStatus,Status,LedgerType,StateCode,ProductList,PinCode " +
                    " from Customer  where customerID='"+customerid+"'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cust = new customer();
                    cust.rowID = reader.GetInt32(0);
                    cust.CustomerID = reader.GetString(1);
                    cust.name = reader.GetString(2);
                    cust.TypeID = reader.GetString(3);
                    cust.CategoryID = reader.GetString(4);
                    cust.GroupID = reader.GetString(5);
                    cust.Address1 = reader.GetString(6);
                    cust.Address2 = reader.GetString(7);
                    cust.Address3 = reader.GetString(8);
                    cust.Address4 = reader.GetString(9);
                    cust.CountryID = reader.GetString(10);
                    cust.OfficeID = reader.GetString(11);
                    cust.Phone = reader.GetString(12);
                    cust.Fax = reader.GetString(13);
                    cust.EmailID = reader.GetString(14);
                    cust.WebSite = reader.GetString(15);
                    cust.ContactPerson = reader.GetString(16);
                    cust.ClientList = reader.GetString(17);
                    cust.BillingAddress = reader.GetString(18);
                    cust.DeliveryAddress = reader.GetString(19);
                    cust.DocumentStatus = reader.GetInt32(20);
                    cust.status = reader.GetInt32(21);
                    cust.LedgerType = reader.IsDBNull(22) ? "" : reader.GetString(22);
                    cust.StateCode = reader.IsDBNull(23) ? "" : reader.GetString(23);
                    cust.ProductList = reader.IsDBNull(24) ? "" : reader.GetString(24);
                    cust.PinCode = reader.IsDBNull(25) ? "" : reader.GetString(25);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return cust;

        }
        public static int getMaxRowID()
        {
            int id = 0;
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select MAX(rowID) from Customer";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    id = reader.GetInt32(0);    
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return id;
        }
        public Boolean updateCustomer(customer cust)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update Customer set name='" + cust.name +
                    "', CustomerGroup='" + cust.GroupID +
                    "', CustomerType='" + cust.TypeID +
                    "', CustomerCategory='" + cust.CategoryID +
                    "', CountryID='" + cust.CountryID +
                    "', StateCode='" + cust.StateCode +
                     "', PinCode='" + cust.PinCode +
                    "', OfficeID='" + cust.OfficeID +
                    "', Address1='" + cust.Address1+
                    "', Address2='" + cust.Address2 +
                    "', Address3='" + cust.Address3 +
                    "', Address4='" + cust.Address4 +
                    "', Phone='" + cust.Phone +
                    "', Fax='" + cust.Fax +
                    "', EmailID='" + cust.EmailID +
                    "', WebSite='" + cust.WebSite +
                    "', ContactPerson='" + cust.ContactPerson +
                    "', ClientList='" + cust.ClientList +
                    "', BillingAddress='" + cust.BillingAddress +
                    "', DeliveryAddress='" + cust.DeliveryAddress +
                     "', ProductList='" + cust.ProductList +
                    "', ForwarderList = '" + cust.ForwarderList  +
                    "', LedgerType = '" + cust.LedgerType +
                    "', Status = '" + cust.status + "'" +
                    " where CustomerID='" + cust.CustomerID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "Customer", "", updateSQL) +
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
        public Boolean insertCustomer(customer cust)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into Customer (CustomerID,Name,CountryID,StateCode,OfficeID,PinCode, CustomerGroup,CustomerCategory,"+
                    "CustomerType,Address1,Address2,Address3,Address4,Phone,Fax,EmailID,"+
                    "WebSite,ContactPerson,ClientList,BillingAddress,DeliveryAddress,ProductList,DocumentStatus,Status," +
                    "CreateTime,CreateUser,ForwarderList,LedgerType)" +
                    " values (" +
                    "IDENT_CURRENT('Customer')" + "," +
                    "'" + cust.name + "'," +
                    "'" + cust.CountryID + "'," +
                     "'" + cust.StateCode + "'," +
                    "'" + cust.OfficeID + "'," +
                     "'" + cust.PinCode + "'," +
                    "'" + cust.GroupID + "'," +
                    "'" + cust.CategoryID + "'," +
                    "'" + cust.TypeID + "'," +
                    "'" + cust.Address1 + "'," +
                    "'" + cust.Address2 + "'," +
                    "'" + cust.Address3 + "'," +
                    "'" + cust.Address4 + "'," +
                    "'" + cust.Phone + "'," +
                    "'" + cust.Fax + "'," +
                    "'" + cust.EmailID + "'," +
                    "'" + cust.WebSite + "'," +
                    "'" + cust.ContactPerson + "'," +
                    "'" + cust.ClientList + "'," +
                    "'" + cust.BillingAddress + "'," +
                    "'" + cust.DeliveryAddress + "'," +
                     "'" + cust.ProductList + "'," +
                    cust.DocumentStatus + "," +
                    cust.status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn  + "'," +
                    "'" +  cust.ForwarderList + "'," +
                    "'" + cust.LedgerType + "')";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "Customer", "", updateSQL) +
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
        public Boolean validateCustomer(customer cust)
        {
            Boolean status = true;
            try
            {
                if (cust.name.Trim().Length == 0 || cust.name == null)
                {
                    return false;
                }
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return status;
        }
        public static void fillCustomerCombo(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                CustomerDB dbrecord = new CustomerDB();
                List<customer> Customers = dbrecord.getCustomers("",6);
                foreach (customer cust in Customers)
                {
                    cmb.Items.Add(cust.CustomerID + "-" + cust.name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show( System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }

        }

        public static void fillCustomerComboNew(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                CustomerDB dbrecord = new CustomerDB();
                List<customer> Customers = dbrecord.getCustomers("", 6);
                foreach (customer cust in Customers)
                {
                    Structures.ComboBoxItem cbitem =
                            new Structures.ComboBoxItem(cust.name, cust.CustomerID);
                    cmb.Items.Add(cbitem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }

        }
        public static void fillLedgerTypeComboNew(System.Windows.Forms.ComboBox cmb,String listType)
        {
            cmb.Items.Clear();

            try
            {

                CustomerDB dbrecord = new CustomerDB();
                List<customer> Customers = dbrecord.getCustomers("", 6).Where(cust => cust.LedgerType.Contains(listType)).ToList();
                foreach (customer cust in Customers)
                {
                    Structures.ComboBoxItem cbitem =
                            new Structures.ComboBoxItem(cust.name, cust.CustomerID);
                    cmb.Items.Add(cbitem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }

        }
        public Boolean forwardCustomer(customer cust)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update Customer set DocumentStatus=" + (cust.DocumentStatus + 1) +
                    ", forwardUser='" + cust.ForwardUser + "'" +
                    ", ForwarderList='" + cust.ForwarderList + "'" +
                    " where CustomerID='" + cust.CustomerID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "Customer", "", updateSQL) +
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

        public Boolean reverseCustomer(customer cust)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update Customer set DocumentStatus=" + cust.DocumentStatus +
                    ", forwardUser='" + cust.ForwardUser + "'" +
                    ", ForwarderList='" + cust.ForwarderList + "'" +
                   " where CustomerID='" + cust.CustomerID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "Customer", "", updateSQL) +
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

        public Boolean ApproveCustomer(customer cust, string custid)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update Customer set CustomerID = '"+custid+"', DocumentStatus=99, status=1 " +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    " where CustomerID='" + cust.CustomerID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "Customer", "", updateSQL) +
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
        public static ListView getCustomerListView()
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
                CustomerDB cdb = new CustomerDB();
                List<customer> CustList = cdb.getCustomers("",6);
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Customer Id", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Customer Name", -2, HorizontalAlignment.Left);
                foreach (customer cust in CustList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(cust.CustomerID.ToString());
                    item1.SubItems.Add(cust.name.ToString());
                    lv.Items.Add(item1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lv;
        }

        public List<customer> getCustomerList()
        {
            List<customer> custList = new List<customer>();
            customer cust = new customer();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.RowID,a.CustomerID, a.Name,a.LedgerType,b.StateName,a.pincode  from Customer as a left outer join State as b " +
                    " on  a.StateCode = b.StateCode where a.Status = 1 and a.DocumentStatus = 99 ";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cust = new customer();
                    cust.rowID = reader.GetInt32(0);
                    cust.CustomerID = reader.GetString(1);
                    cust.name = reader.GetString(2);
                    cust.LedgerType = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    cust.StateName = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    cust.PinCode = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    custList.Add(cust);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return custList;
        }
        public static customer getCustomerDetailForPO(string custID)
        {
            customer cust = new customer();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.RowID,a.CustomerID, a.Name, a.BillingAddress,a.StateCode, b.StateName,c.DocumentID,c.DocumentValue " +
                    " from Customer as a left outer join" +
                    " State as b on a.StateCode = b.StateCode left outer join" +
                    " CustomerStatutoryDetail c on a.RowID = c.CustomerRowID and c.DocumentID = 'GST'" +
                    " where  a.Status = 1 and a.DocumentStatus = 99 and a.CustomerID = '" + custID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cust = new customer();
                    cust.rowID = reader.GetInt32(0);
                    cust.CustomerID = reader.GetString(1);
                    cust.name = reader.GetString(2);
                    cust.BillingAddress = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    cust.StateCode = reader.IsDBNull(4) ? "" : reader.GetString(4);
                    cust.StateName = reader.IsDBNull(5) ? "" : reader.GetString(5);
                    cust.OfficeID = reader.IsDBNull(6) ? "" : reader.GetString(6); //For GST(Customer Statutory DocumentCode)
                    cust.OfficeName = reader.IsDBNull(7) ? "" : reader.GetString(7);// for GST Code Value(Customer Statutory DocumentValue)
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return cust;
        }
        public static string getCustomerPANForInvoicePrint(string custID)
        {
            string panCode = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select c.DocumentValue " +
                    " from Customer as a left outer join" +
                    " CustomerStatutoryDetail c on a.RowID = c.CustomerRowID and c.DocumentID = 'PANNo'" +
                    " where  a.Status = 1 and a.DocumentStatus = 99 and a.CustomerID = '" + custID + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    panCode = reader.IsDBNull(0) ? "" : reader.GetString(0);// for PAN Code Value(Customer Statutory DocumentValue)
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return panCode;
        }

        //Customer List In GridView
        public DataGridView getGridViewForCustomerList(string ledgerType)
        {
            DataGridView grdCust = new DataGridView();
            try
            {
                string[] strColArr = { "CustomerID", "CustomerName","LedgerType"};
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                };

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdCust.EnableHeadersVisualStyles = false;
                grdCust.AllowUserToAddRows = false;
                grdCust.AllowUserToDeleteRows = false;
                grdCust.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdCust.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdCust.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdCust.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdCust.ColumnHeadersHeight = 27;
                grdCust.RowHeadersVisible = false;
                grdCust.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;
                grdCust.Columns.Add(colChk);
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str;
                    colArr[index].HeaderText = str;
                    colArr[index].ReadOnly = true;
                    if (index == 1)
                        colArr[index].Width = 300;
                    else
                        colArr[index].Width = 150;
                    if (index == 2)
                        colArr[index].Visible = false;
                    grdCust.Columns.Add(colArr[index]);
                }

                CustomerDB cdb = new CustomerDB();
                List<customer> CustList = cdb.getCustomerList().Where(cust => cust.LedgerType.Contains(ledgerType)).OrderBy(cust => cust.name).ToList();

                foreach (customer cust in CustList)
                {
                    grdCust.Rows.Add();
                    grdCust.Rows[grdCust.Rows.Count - 1].Cells[strColArr[0]].Value = cust.CustomerID;
                    grdCust.Rows[grdCust.Rows.Count - 1].Cells[strColArr[1]].Value = cust.name;
                    grdCust.Rows[grdCust.Rows.Count - 1].Cells[strColArr[2]].Value = cust.LedgerType;
                }
            }
            catch (Exception ex)
            {
            }

            return grdCust;
        }
        //Customer List In GridViewNew
        public DataGridView getGridViewForCustomerListNew(string ledgerTypes)
        {
            DataGridView grdCust = new DataGridView();
            try
            {
                string[] ledgerTypeArr = ledgerTypes.Split(Main.delimiter1);
                string[] strColArr = { "ID", "Name","State","PinCode", "LedgerType" };
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                };

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdCust.EnableHeadersVisualStyles = false;
                grdCust.AllowUserToAddRows = false;
                grdCust.AllowUserToDeleteRows = false;
                grdCust.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdCust.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdCust.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdCust.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdCust.ColumnHeadersHeight = 27;
                grdCust.RowHeadersVisible = false;
                grdCust.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;
                grdCust.Columns.Add(colChk);
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str;
                    colArr[index].HeaderText = str;
                    colArr[index].ReadOnly = true;
                    if (index == 1)
                        colArr[index].Width = 300;
                    else if (index == 0)
                        colArr[index].Width = 80;
                    else if (index == 2 || index == 3)
                        colArr[index].Width = 120;
                    else
                        colArr[index].Width = 100;
                    if (index == 4)
                        colArr[index].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    grdCust.Columns.Add(colArr[index]);
                }

                CustomerDB cdb = new CustomerDB();
                List<customer> CustList = new List<customer>();
                if (ledgerTypes.Length != 0)
                    CustList = cdb.getCustomerList().Where(cust => ledgerTypeArr.Any(x => cust.LedgerType.Contains(x))).OrderBy(cust => cust.name).ToList();
                else
                    CustList = cdb.getCustomerList().Where(cust => cust.LedgerType.Length != 0).OrderBy(cust => cust.name).ToList();
                foreach (customer cust in CustList)
                {
                    grdCust.Rows.Add();
                    grdCust.Rows[grdCust.Rows.Count - 1].Cells[strColArr[0]].Value = cust.CustomerID;
                    grdCust.Rows[grdCust.Rows.Count - 1].Cells[strColArr[1]].Value = cust.name;
                    grdCust.Rows[grdCust.Rows.Count - 1].Cells[strColArr[2]].Value = cust.StateName;
                    grdCust.Rows[grdCust.Rows.Count - 1].Cells[strColArr[3]].Value = cust.PinCode;
                    string ldg = cust.LedgerType.Remove(cust.LedgerType.LastIndexOf(Main.delimiter1));
                    grdCust.Rows[grdCust.Rows.Count - 1].Cells[strColArr[4]].Value = ldg.Replace(Main.delimiter1, '/');
                    //grdCust.Rows[grdCust.Rows.Count - 1].Cells[strColArr[2]].Value = string.Join("/",ldg.Split(Main.delimiter1));
                }
            }
            catch (Exception ex)
            {
            }

            return grdCust;
        }
        //new customername
        public static List<customer> getCustomernamelist()
        {
            customer cust = new customer();
            List<customer> cstmr = new List<customer>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select distinct Name  from Customer";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cust = new customer();
                    cust.name = reader.GetString(0);
                    cstmr.Add(cust);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return cstmr;

        }
        public static customer validateCustomerGst(string val, string docid)
        {
            customer cst = new customer();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.CustomerID,a.Name from Customer a," +
                               " (select CustomerRowID, DocumentValue from CustomerStatutoryDetail " +
                               "where documentid = '" + docid + "' and DocumentValue = '" + val + "')b " +
                               " where a.RowID = b.CustomerRowID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cst = new customer();
                    cst.CustomerID = reader.GetString(0);
                    cst.name = reader.GetString(1);
                }
                conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return cst;
        }
    }
}
