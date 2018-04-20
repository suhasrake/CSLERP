using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using CSLERP.DBData;
using System.Collections;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CSLERP
{
    public partial class CustomerSPL : System.Windows.Forms.Form
    {
        string docID = "CUSTOMER";
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        List<customer> Allcustomername = new List<customer>();
        ErrorProvider err = new ErrorProvider();
        ListView lv = new ListView();
        //Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        customer prevcust = new customer();
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        Form frmPopup = new Form();
        ListView lvApprover = new ListView();
        Regex gst = new Regex(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$");
        Regex PAN = new Regex(@"^([a-zA-Z]{5})(\d{4})([a-zA-Z]{1})$");
        //private int selectedRowID = 0;
        int tempEdit = 0;
        int val = 0;
        int Aval = 0;
        public CustomerSPL()
        {

            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {
            }
        }
        private void Customer_Load(object sender, EventArgs e)
        {
            //////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;

            String a = this.Size.ToString();
            grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdList.EnableHeadersVisualStyles = false;

            lv.Visible = false;
            ListFilteredCustomer(listOption);
            //applyPrivilege();
            grdCustomerBank.CellValueChanged +=
         new DataGridViewCellEventHandler(grdCustomerBank_CellValueChanged);
        }
        private void ListFilteredCustomer(int option)
        {
            try
            {
                grdList.Rows.Clear();
                //txtSearch.Text = "";
                if (tempEdit == 1)
                {
                    txtSearch.Text = "";
                }
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                CustomerDB customerdb = new CustomerDB();

                List<customer> Customers = customerdb.getCustomers(userString, option);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (customer cust in Customers)
                {

                    if (option == 1)
                    {
                        if (cust.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["rowID"].Value = cust.rowID;
                    grdList.Rows[grdList.RowCount - 1].Cells["CustomerID"].Value = cust.CustomerID;
                    grdList.Rows[grdList.RowCount - 1].Cells["CustomerName"].Value = cust.name;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = getstatusstring(cust.status);
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = cust.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwardUser"].Value = cust.ForwardUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ApproveUser"].Value = cust.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = cust.Creator;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = cust.Forwarder;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = cust.Approver;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwarderLists"].Value = cust.ForwarderList;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatus"].Value = cust.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["LedgerType"].Value = cust.LedgerType;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in Customer listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;
            filterGridData();
            grdList.Columns["Creator"].Visible = true;
            grdList.Columns["Forwarder"].Visible = true;
            grdList.Columns["Approver"].Visible = true;
            if ((listOption == 6 || listOption == 3) && getuserPrivilegeStatus() != 1)
            {
                grdList.Columns["TempEdit"].Visible = true;
            }
            else
            {
                grdList.Columns["TempEdit"].Visible = false;
            }
        }

        private void initVariables()
        {
            try
            {

                if (getuserPrivilegeStatus() == 1)
                {
                    //user is only a viewer
                    listOption = 6;
                }
                //fillStatusCombo(cmbStatus);
                ////CatalogueValueDB.fillCatalogValueCombo(cmbCustomerType, "CustomerType");
                ////CatalogueValueDB.fillCatalogValueCombo(cmbCustomerGroup, "CustomerGroup");
                ////CatalogueValueDB.fillCatalogValueCombo(cmbCustomerCategory, "CustomerCategory");

                CustomerGroupDB.fillCustomerGroupValueCombo(cmbCustomerGroup1, 1);
                CustomerGroupDB.fillCustomerGroupValueCombo(cmbCustomerGroup2, 2);
                CustomerGroupDB.fillCustomerGroupValueCombo(cmbCustomerGroup3, 3);
                CatalogueValueDB.fillCatalogValueComboNew(cmbCountry, "Country");
                StateDB.fillStateComboNew(cmbState);
                //CatalogueValueDB.fillCatalogValueCombo(cmbCountry, "Country");
                OfficeDB.fillOfficeComboNew(cmbOfficeAttached);
                cmbStatus.SelectedIndex = 0;
                setButtonVisibility("init");
                closeAllPanels();
                userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
                setTabIndex();
                txtCustomerName.Focus();
            }
            catch (Exception)
            {
            }
        }

        private void setTabIndex()
        {
            txtCustomerID.TabIndex = 0;
            txtCustomerName.TabIndex = 1;
            cmbCustomerGroup1.TabIndex = 2;
            cmbCustomerGroup2.TabIndex = 3;
            cmbCustomerGroup3.TabIndex = 4;
            cmbCountry.TabIndex = 5;
            cmbState.TabIndex = 6;
            cmbOfficeAttached.TabIndex = 7;
            txtPinCode.TabIndex = 8;
            cmbStatus.TabIndex = 9;
            clbCustomerType.TabIndex = 10;

            txtAddress1.TabIndex = 0;
            txtAddress2.TabIndex = 1;
            txtAddress3.TabIndex = 2;
            txtAddress4.TabIndex = 3;
            txtPhone.TabIndex = 4;
            txtFax.TabIndex = 5;
            txtEmailID.TabIndex = 6;
            txtWebSite.TabIndex = 7;
            txtContactList.TabIndex = 8;
            txtClientList.TabIndex = 9;

            btnNewBank.TabIndex = 0;
            grdCustomerBank.TabIndex = 1;

            btnNewDocument.TabIndex = 0;
            grdCustomerDocuments.TabIndex = 1;

            btnForward.TabIndex = 1;
            btnApprove.TabIndex = 0;
            btnCancel.TabIndex = 2;
            btnSave.TabIndex = 3;
            btnReverse.TabIndex = 4;
        }
        private void closeAllPanels()
        {
            try
            {
                pnlInner.Visible = false;
                pnlOuter.Visible = false;
                pnlList.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        private void clearData()
        {
            try
            {
                //clear all grid views
                // grdPOPIDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                //----------clear temperory panels

                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                //----------
                txtCustomerID.Text = "";
                txtCustomerName.Text = "";

                cmbCustomerGroup1.SelectedIndex = -1;
                cmbCustomerGroup3.SelectedIndex = -1;
                cmbCustomerGroup2.SelectedIndex = -1;
                cmbCountry.SelectedIndex = -1;
                cmbOfficeAttached.SelectedIndex = -1;
                cmbState.SelectedIndex = -1;

                txtAddress1.Text = "";
                txtAddress2.Text = "";
                txtAddress3.Text = "";
                txtAddress4.Text = "";
                txtPinCode.Text = "";
                txtPhone.Text = "";
                txtFax.Text = "";
                txtEmailID.Text = "";
                txtWebSite.Text = "";
                txtContactList.Text = "";
                txtClientList.Text = "";
                txtBillingAddress.Text = "";
                txtDeliveryAddress.Text = "";
                txtProductList.Text = "";
                // cmbStatus.SelectedIndex = 0;
                grdCustomerBank.Rows.Clear();
                grdCustomerDocuments.Rows.Clear();
                foreach (int i in clbCustomerType.CheckedIndices)
                {
                    clbCustomerType.SetItemCheckState(i, CheckState.Unchecked);
                }
                clbCustomerType.SelectedIndex = clbCustomerType.Items.Count - 1;
                cmbCustomerGroup1.Enabled = true;
                cmbCustomerGroup2.Enabled = true;
                cmbCustomerGroup3.Enabled = true;
                //removeControlsFromCommenterPanel();
                //removeControlsFromForwarderPanel();
                Aval = 0;
                tempEdit = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("clearData() : Error-" + ex.ToString());
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                clearData();
                pnlList.Visible = true;
                setButtonVisibility("btnEditPanel");
                lv.Visible = false;
                err.Clear();
            }
            catch (Exception)
            {

            }
        }



        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                pnlUI.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                Allcustomername = new List<customer>();
                Allcustomername = CustomerDB.getCustomernamelist();
                closeAllPanels();
                clearData();
                btnSave.Text = "Save";
                pnlOuter.Visible = true;
                pnlInner.Visible = true;
                ////txtCustomerID.Enabled = true;
                setButtonVisibility("btnNew");
                tabControl1.SelectedTab = tabBasicDetails;
                txtCustomerName.Focus();
            }
            catch (Exception)
            {

            }
        }
        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            txtSearch.Text = "";
            ListFilteredCustomer(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            txtSearch.Text = "";
            ListFilteredCustomer(listOption);

        }

        private void btnApproved_Click(object sender, EventArgs e)
        {
            if (getuserPrivilegeStatus() == 2)
            {
                listOption = 6; //viewer
            }
            else
            {
                listOption = 3;
            }
            txtSearch.Text = "";
            ListFilteredCustomer(listOption);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {
                if (!verifyBankDetails())
                {
                    MessageBox.Show("Error in Bank Detail");
                    return;
                }
                if (!verifyDocumentDetails())
                {
                    MessageBox.Show("Error in Statutory Detail");
                    return;
                }
                customer cust = new customer();
                CustomerDB customerdb = new CustomerDB();
                cust.CustomerID = txtCustomerID.Text;
                cust.name = txtCustomerName.Text;
                try
                {
                    cust.GroupID = ((Structures.ComboBoxItem)cmbCustomerGroup1.SelectedItem).HiddenValue;
                    //cust.GroupName = cmbCustomerGroup1.SelectedItem.ToString().Trim().Substring(cmbCustomerGroup1.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                    cust.CategoryID = ((Structures.ComboBoxItem)cmbCustomerGroup2.SelectedItem).HiddenValue;
                    //cust.CategoryName = cmbCustomerGroup2.SelectedItem.ToString().Trim().Substring(cmbCustomerGroup2.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                    cust.TypeID = ((Structures.ComboBoxItem)cmbCustomerGroup3.SelectedItem).HiddenValue;

                    cust.CountryID = ((Structures.ComboBoxItem)cmbCountry.SelectedItem).HiddenValue;
                    cust.CountryName = ((Structures.ComboBoxItem)cmbCountry.SelectedItem).ToString();

                    try
                    {
                        cust.StateCode = ((Structures.ComboBoxItem)cmbState.SelectedItem).HiddenValue;
                    }
                    catch (Exception ex)
                    {
                    }
                    cust.OfficeID = ((Structures.ComboBoxItem)cmbOfficeAttached.SelectedItem).HiddenValue;
                    cust.OfficeName = ((Structures.ComboBoxItem)cmbOfficeAttached.SelectedItem).ToString();
                    cust.PinCode = txtPinCode.Text.Trim();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Customer Data Validation failed");
                    return;
                }
                if (cust.CountryID == "India" && txtPinCode.Text.Trim().Length != 0)
                {
                    if (txtPinCode.Text.Trim().Length != 6)
                    {
                        MessageBox.Show("Pin code is not correct.");
                        return;
                    }
                }
                cust.Address1 = txtAddress1.Text;
                cust.Address2 = txtAddress2.Text;
                cust.Address3 = txtAddress3.Text;
                cust.Address4 = txtAddress4.Text;
                cust.Phone = txtPhone.Text;
                cust.Fax = txtFax.Text;
                cust.EmailID = txtEmailID.Text;
                cust.WebSite = txtWebSite.Text;
                cust.ContactPerson = txtContactList.Text;
                cust.ClientList = txtClientList.Text;
                cust.BillingAddress = txtBillingAddress.Text;
                cust.DeliveryAddress = txtDeliveryAddress.Text;
                cust.ProductList = txtProductList.Text;
                cust.ForwarderList = prevcust.ForwarderList;
                //----
                foreach (var item in clbCustomerType.CheckedItems)
                {
                    cust.LedgerType = cust.LedgerType + item.ToString() + Main.delimiter1;
                }
                //----
                cust.status = getstatuscode(cmbStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;

                //Replacing single quotes
                cust = verifyHeaderInputString(cust);
                verifyDetailInputString();
                if (customerdb.validateCustomer(cust))
                {
                    if (btnText.Equals("Update"))
                    {
                        cust.DocumentStatus = prevcust.DocumentStatus;
                        if (customerdb.updateCustomer(cust))
                        {
                            createAndUpdateCustomerBankDetails(prevcust.rowID);
                            createAndUpdateCustomerDocumentDetails(prevcust.rowID);
                            MessageBox.Show("Customer updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredCustomer(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update Customet Details");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        cust.DocumentStatus = 1;
                        if (customerdb.insertCustomer(cust))
                        {
                            int cistID = CustomerDB.getMaxRowID();
                            createAndUpdateCustomerBankDetails(cistID);
                            createAndUpdateCustomerDocumentDetails(cistID);
                            MessageBox.Show("Customer Added");
                            closeAllPanels();
                            txtSearch.Text = "";
                            listOption = 1;
                            ListFilteredCustomer(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Save Customer Details");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Customer Data Validation failed");
                    status = false;
                }
            }
            catch (Exception ex)
            {
                status = false;
                MessageBox.Show("Failed Adding / Editing Customer");
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }

        public int getstatuscode(string stat)
        {
            int status = 2;
            if (stat == "Active")
            {
                status = 1;
            }
            else if (stat == "Deactive")
            {
                status = 0;
            }
            return status;
        }

        public string getstatusstring(int stat)
        {
            string status = "";
            if (stat == 1)
            {
                status = "Active";
            }
            else if (stat == 0)
            {
                status = "Deactive";
            }
            return status;
        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("LoadDocument") ||
                    columnName.Equals("View") || columnName.Equals("TempEdit"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    prevcust.rowID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["rowID"].Value.ToString());
                    prevcust.CustomerID = grdList.Rows[e.RowIndex].Cells["CustomerID"].Value.ToString();
                    prevcust.name = grdList.Rows[e.RowIndex].Cells["CustomerName"].Value.ToString();
                    prevcust.CreateUser = grdList.Rows[e.RowIndex].Cells["CreateUser"].Value.ToString();
                    prevcust.ForwardUser = grdList.Rows[e.RowIndex].Cells["ForwardUser"].Value.ToString();
                    prevcust.ApproveUser = grdList.Rows[e.RowIndex].Cells["ApproveUser"].Value.ToString();
                    prevcust.Creator = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    prevcust.Forwarder = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevcust.Approver = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    prevcust.ForwarderList = grdList.Rows[e.RowIndex].Cells["ForwarderLists"].Value.ToString();
                    prevcust.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentStatus"].Value.ToString());
                    prevcust.LedgerType = grdList.Rows[e.RowIndex].Cells["LedgerType"].Value.ToString();
                    //-------
                    try
                    {
                        string[] lst1 = prevcust.LedgerType.Split(Main.delimiter1);
                        for (int j = 0; j < lst1.Length - 1; j++)
                        {
                            clbCustomerType.SetItemChecked(clbCustomerType.Items.IndexOf(lst1[j]), true);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    //-------
                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Customer Name:" + prevcust.name + "\n" +
                            "Customer ID:" + prevcust.CustomerID;
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevcust.CustomerID + "-" + prevcust.name;
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click(null, null);
                        return;
                    }
                    //--------
                    if (columnName.Equals("TempEdit"))
                    {
                        tempEdit = 1;
                    }
                    txtCustomerID.Text = grdList.Rows[e.RowIndex].Cells["CustomerID"].Value.ToString();
                    CustomerDB customerdb = new CustomerDB();
                    customer cust = new customer();
                    cust = CustomerDB.getCustomerDetails(txtCustomerID.Text);
                    //load data for editing

                    txtCustomerName.Text = cust.name;
                    cmbCustomerGroup1.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCustomerGroup1, cust.GroupID);
                    cmbCustomerGroup2.SelectedIndex =
                         Structures.ComboFUnctions.getComboIndex(cmbCustomerGroup1, cust.CategoryID);
                    cmbCustomerGroup3.SelectedIndex =
                         Structures.ComboFUnctions.getComboIndex(cmbCustomerGroup1, cust.TypeID);
                    cmbCountry.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCountry, cust.CountryID.Trim());
                    //cmbCountry.FindString(cust.CountryID.Trim());
                    //cmbOfficeAttached.SelectedIndex = cmbOfficeAttached.FindString(cust.OfficeID.Trim());
                    cmbOfficeAttached.SelectedIndex =
                         Structures.ComboFUnctions.getComboIndex(cmbOfficeAttached, cust.OfficeID);
                    cmbState.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbState, cust.StateCode);
                    txtPinCode.Text = cust.PinCode;
                    txtAddress1.Text = cust.Address1;
                    txtAddress2.Text = cust.Address2;
                    txtAddress3.Text = cust.Address3;
                    txtAddress4.Text = cust.Address4;

                    txtPhone.Text = cust.Phone;
                    txtFax.Text = cust.Fax;
                    txtEmailID.Text = cust.EmailID;
                    txtWebSite.Text = cust.WebSite;
                    txtContactList.Text = cust.ContactPerson;
                    txtClientList.Text = cust.ClientList;
                    txtBillingAddress.Text = cust.BillingAddress;
                    txtDeliveryAddress.Text = cust.DeliveryAddress;
                    txtProductList.Text = cust.ProductList;
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(getstatusstring(cust.status));

                    //get customer bank details
                    CustomerBankDetailsDB cbddb = new CustomerBankDetailsDB();
                    List<customerbankdetails> cbDetails = cbddb.getCustomerBankDetails(prevcust.rowID);
                    grdCustomerBank.Rows.Clear();
                    int i = 0;
                    foreach (customerbankdetails cbd in cbDetails)
                    {
                        grdCustomerBank.Rows.Add();
                        DataGridViewComboBoxCell ComboColumn = (DataGridViewComboBoxCell)(grdCustomerBank.Rows[i].Cells["Bank"]);
                        //string firstValue = CatalogueValueDB.fillCatalogValueGridViewCombo(ComboColumn, "Bank");
                        CatalogueValueDB.fillCatalogValueGridViewComboNew(ComboColumn, "Bank");
                        grdCustomerBank.Rows[i].Cells["Bank"].Value = cbd.BankID;// + "-" + cbd.BankName;

                        DataGridViewComboBoxCell ComboColumn1 = new DataGridViewComboBoxCell();
                        string firstValue = BankBranchDB.fillBankBranchGridViewComboNew(ComboColumn1, cbd.BankID);
                        try
                        {
                            grdCustomerBank.Rows[i].Cells["Branch"] = ComboColumn1;
                            int c = ComboColumn1.Items.Count;
                            grdCustomerBank.Rows[i].Cells["Branch"].Value = cbd.BankBranch.ToString();// + "-" + cbd.BankBranchName;
                        }

                        catch (Exception ex)
                        {
                            MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                        }
                        try
                        {
                            grdCustomerBank.Rows[i].Cells["AccountNo"].Value = cbd.AccountNo;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                        }
                        i++;
                    }
                    //get customer document details
                    CustomerDocumentDetailsDB cdddb = new CustomerDocumentDetailsDB();
                    List<customerdocumentdetails> cdDetails = cdddb.getCustomerDocumentDetails(prevcust.rowID);
                    grdCustomerDocuments.Rows.Clear();
                    i = 0;
                    foreach (customerdocumentdetails cdd in cdDetails)
                    {
                        try
                        {
                            grdCustomerDocuments.Rows.Add();
                            DataGridViewComboBoxCell ComboColumn = (DataGridViewComboBoxCell)(grdCustomerDocuments.Rows[i].Cells["Document"]);
                            //string firstValue = CatalogueValueDB.fillCatalogValueGridViewCombo(ComboColumn, "Statutory");
                            CatalogueValueDB.fillCatalogValueGridViewComboNew(ComboColumn, "Statutory");
                            grdCustomerDocuments.Rows[i].Cells["Document"].Value = cdd.DocumentID;// + "-" + cdd.DocumentName;
                            grdCustomerDocuments.Rows[i].Cells["DocumentValue"].Value = cdd.DocumentValue;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                        }


                        i++;
                    }
                    tabControl1.SelectedTab = tabBasicDetails;
                    //disableBottomButtons();
                    //tabPage1.Focus();
                    if (columnName.Equals("TempEdit"))
                    {
                        cmbCustomerGroup1.Enabled = false;
                        cmbCustomerGroup2.Enabled = false;
                        cmbCustomerGroup3.Enabled = false;
                    }
                    else
                    {
                        cmbCustomerGroup1.Enabled = true;
                        cmbCustomerGroup2.Enabled = true;
                        cmbCustomerGroup3.Enabled = true;
                    }
                    pnlInner.Visible = true;
                    pnlOuter.Visible = true;
                    pnlList.Visible = false;
                    ////txtCustomerID.Enabled = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void tabPage2_Click(object sender, EventArgs e)
        {
            //fill bank details in combo
        }


        private void btnNewBank_Click(object sender, EventArgs e)
        {
            try
            {
                grdCustomerBank.Rows.Add();
                int kount = grdCustomerBank.RowCount;
                DataGridViewComboBoxCell ComboColumn = (DataGridViewComboBoxCell)(grdCustomerBank.Rows[kount - 1].Cells[0]);
                //string firstValue = CatalogueValueDB.fillCatalogValueGridViewCombo(ComboColumn, "Bank");
                CatalogueValueDB.fillCatalogValueGridViewComboNew(ComboColumn, "Bank");
                //grdCustomerBank.Rows[kount-1].Cells[0].Value = firstValue;
            }
            catch (Exception ex)
            {

                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void grdCustomerBank_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int rowIndex = e.RowIndex;
                int colIndex = e.ColumnIndex;
                if (colIndex == 0)
                {

                    string selectedBank = grdCustomerBank.Rows[rowIndex].Cells[colIndex].Value.ToString();
                    //selectedBank = selectedBank.Substring(0, selectedBank.IndexOf('-'));
                    DataGridViewComboBoxCell ComboColumn = new DataGridViewComboBoxCell();
                    string firstValue = BankBranchDB.fillBankBranchGridViewComboNew(ComboColumn, selectedBank);
                    try
                    {
                        grdCustomerBank.Rows[rowIndex].Cells["Branch"] = ComboColumn;
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private void grdCustomerBank_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("grdCustomerBank_DataError() : Error Raised");
        }

        private void grdCustomerBank_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("Column clicked is " + e.RowIndex+"-"+e.ColumnIndex);
            string columnName = grdCustomerBank.Columns[e.ColumnIndex].Name;
            if (columnName.Equals("Del"))
            {
                try
                {
                    //delete row
                    DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        grdCustomerBank.Rows.RemoveAt(e.RowIndex);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                }
            }
            if (columnName.Equals("Det"))
            {
                try
                {
                    //create popup form
                    Form popupForm = new Form();
                    popupForm.Text = "Branch Details";
                    popupForm.Icon = null;
                    popupForm.Height = 300;
                    popupForm.Width = 500;
                    popupForm.FormBorderStyle = FormBorderStyle;
                    popupForm.StartPosition = FormStartPosition.CenterParent;
                    //Font font = new System.Drawing.Font("Meiryo UI", 16.0f);
                    //popupForm.Font = font;
                    popupForm.BackColor = Color.White;
                    popupForm.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
                    popupForm.MaximizeBox = false;
                    popupForm.MinimizeBox = false;
                    popupForm.BackColor = Color.SkyBlue;
                    popupForm.Opacity = .90;
                    popupForm.ShowIcon = false;
                    popupForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    //add data
                    //string selectedBank = grdCustomerBank.Rows[e.RowIndex].Cells[0].Value.ToString();
                    //string bankID = selectedBank.Substring(0, selectedBank.IndexOf('-'));
                    string bankID = grdCustomerBank.Rows[e.RowIndex].Cells["Bank"].Value.ToString();
                    //string selectedBranch = grdCustomerBank.Rows[e.RowIndex].Cells[1].Value.ToString();
                    //string branchName = selectedBranch.Substring(selectedBranch.IndexOf('-') + 1);
                    string branchName = grdCustomerBank.Rows[e.RowIndex].Cells["Branch"].FormattedValue.ToString().Trim();
                    BankBranchDB bankbranchdb = new BankBranchDB();
                    bankbranch branchdetails = bankbranchdb.getBankBrancheDetails(bankID, branchName);
                    Label[] lbl = new Label[10];
                    for (int i = 0; i < 10; i++)
                    {
                        lbl[i] = new Label();
                        //txt[i].Text = "text " + i;
                        lbl[i].Location = new Point(40, i * 30 + 20);
                        lbl[i].Height = 25;
                        lbl[i].Width = 400;
                        popupForm.Controls.Add(lbl[i]);
                    }
                    lbl[1].Text = "Bank Details";
                    lbl[1].Font = new Font("Arial", 14);
                    lbl[3].Text = "Bank : " + branchdetails.BankName;
                    lbl[4].Text = "Branch Name : " + branchdetails.BranchName;
                    lbl[5].Text = "IFSC Code : " + branchdetails.IFSCCode;
                    lbl[6].Text = "MICR Code : " + branchdetails.MICRCode;
                    lbl[7].Text = "BSR Code : " + branchdetails.BSRCode;
                    popupForm.ShowDialog();
                }
                catch (Exception)
                {
                    MessageBox.Show("Error Showing Branch Details");
                }
            }
        }
        private Boolean verifyBankDetails()
        {
            Boolean status = true;
            try
            {
                for (int i = 0; i < grdCustomerBank.RowCount; i++)
                {
                    if (grdCustomerBank.Rows[i].Cells["Bank"].Value.ToString() == null)
                    {
                        return false;
                    }
                    if (grdCustomerBank.Rows[i].Cells["Branch"].Value.ToString() == null)
                    {
                        return false;
                    }
                    if (grdCustomerBank.Rows[i].Cells["AccountNo"].Value.ToString() == null ||
                        grdCustomerBank.Rows[i].Cells["AccountNo"].Value.ToString().Trim().Length == 0)
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("verifyBankDetails() : Error");
                status = false;
            }
            return status;
        }

        private void createAndUpdateCustomerBankDetails(int custID)
        {
            try
            {
                BankBranchDB bankbranchdb = new BankBranchDB();
                bankbranch branchdetails = new bankbranch();
                customerbankdetails cbd;
                List<customerbankdetails> CustomerBankDetails = new List<customerbankdetails>();
                for (int i = 0; i < grdCustomerBank.Rows.Count; i++)
                {
                    try
                    {
                        cbd = new customerbankdetails();
                        cbd.CustomerRowID = custID;
                        //get bank branch id
                        //string selectedBank = grdCustomerBank.Rows[i].Cells[0].Value.ToString();
                        //string bankID = selectedBank.Substring(0, selectedBank.IndexOf('-'));
                        string bankID = grdCustomerBank.Rows[i].Cells["Bank"].Value.ToString();
                        //string selectedBranch = grdCustomerBank.Rows[i].Cells[1].Value.ToString();
                        //string branchName = selectedBranch.Substring(selectedBranch.IndexOf('-') + 1);
                        string branchName = grdCustomerBank.Rows[i].Cells["Branch"].FormattedValue.ToString().Trim();
                        branchdetails = bankbranchdb.getBankBrancheDetails(bankID, branchName);
                        cbd.BankBranch = branchdetails.BranchID;
                        try
                        {
                            cbd.AccountNo = grdCustomerBank.Rows[i].Cells["AccountNo"].Value.ToString();
                        }
                        catch (Exception)
                        {
                            cbd.AccountNo = "";
                        }
                        cbd.status = 1;
                        CustomerBankDetails.Add(cbd);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("createAndUpdateCustomerBankDetails() : Error creating customer branch list");
                        return;
                    }
                }
                CustomerBankDetailsDB customerbankdetailsdb = new CustomerBankDetailsDB();
                if (!customerbankdetailsdb.updateCustomerBankDetails(custID.ToString(), CustomerBankDetails))
                {
                    MessageBox.Show("Failed to update customer branch list. Please check the values");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("createAndUpdateCustomerBankDetails() : Error updating customer branch list");
            }
        }

        private void btnNewDocument_Click(object sender, EventArgs e)
        {
            try
            {
                if (Aval == 1)
                {
                    int rowno = grdCustomerDocuments.RowCount - 1;
                    validateCustomerDocument(rowno);
                }
                grdCustomerDocuments.Rows.Add();
                int kount = grdCustomerDocuments.RowCount;
                Aval = 1;
                DataGridViewComboBoxCell ComboColumn = (DataGridViewComboBoxCell)(grdCustomerDocuments.Rows[kount - 1].Cells["Document"]);
                //string firstValue = CatalogueValueDB.fillCatalogValueGridViewCombo(ComboColumn, "Statutory");
                CatalogueValueDB.fillCatalogValueGridViewComboNew(ComboColumn, "Statutory");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating new document row");
            }
        }
        private void validateCustomerDocument(int rowno)
        {
            DataGridViewRow rw = grdCustomerDocuments.Rows[rowno];
            if (rw.Cells["Document"].Value.ToString() == "GST")
            {
                string docid = "GST";
                string gstval = rw.Cells["DocumentValue"].Value.ToString();
                if (!gst.IsMatch(gstval))
                {
                    MessageBox.Show("Please check the GST number...Entered!!!!");
                    grdCustomerDocuments.Rows.RemoveAt(rw.Index);
                }
                customer cst = CustomerDB.validateCustomerGst(gstval, docid);
                if (cst.CustomerID != null)
                {
                    MessageBox.Show("!!!This GST number already exists for \n Customer     : " + cst.name + " \n CustomerID : " + cst.CustomerID + "");
                    grdCustomerDocuments.Rows.RemoveAt(rw.Index);
                }
            }
            else if (rw.Cells["Document"].Value.ToString() == "PANNo")
            {
                string docid = "PANNo";
                string panval = rw.Cells["DocumentValue"].Value.ToString();
                if (!PAN.IsMatch(panval))
                {
                    MessageBox.Show("Please check the PAN Number...Entered!!!!");
                    grdCustomerDocuments.Rows.RemoveAt(rw.Index);
                }
                customer cst = CustomerDB.validateCustomerGst(panval, docid);

                if (cst.CustomerID != null)
                {
                    MessageBox.Show("!!!! This PAN number already exists for \n Customer     : " + cst.name + " \n CustomerID : " + cst.CustomerID + "");
                    grdCustomerDocuments.Rows.RemoveAt(rw.Index);
                }
            }
        }
        public void combocolevent(object s, EventArgs e)
        {

        }

        private void grdCustomerDocuments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string columnName = grdCustomerDocuments.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Delete"))
                {
                    try
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdCustomerDocuments.Rows.RemoveAt(e.RowIndex);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }
        private Boolean verifyDocumentDetails()
        {
            Boolean status = true;
            try
            {
                for (int i = 0; i < grdCustomerDocuments.RowCount; i++)
                {
                    if (grdCustomerDocuments.Rows[i].Cells["Document"].Value.ToString() == null)
                    {
                        return false;
                    }
                    if (grdCustomerDocuments.Rows[i].Cells["DocumentValue"].Value.ToString() == null ||
                        grdCustomerDocuments.Rows[i].Cells["DocumentValue"].Value.ToString().Trim().Length == 0)
                    {
                        return false;
                    }
                }

                if (Aval == 1)
                {
                    Aval = 0;
                    DataGridViewRow rw = grdCustomerDocuments.Rows[grdCustomerDocuments.RowCount - 1];
                    if (rw.Cells["Document"].Value.ToString() == "GST")
                    {
                        string docid = "GST";
                        string gstval = rw.Cells["DocumentValue"].Value.ToString();
                        if (!gst.IsMatch(gstval))
                        {
                            MessageBox.Show("Please check the GST...Entered!!!!");
                            grdCustomerDocuments.Rows.RemoveAt(rw.Index);
                            return false;
                        }
                        customer cst = CustomerDB.validateCustomerGst(gstval, docid);
                        if (cst.CustomerID != null)
                        {
                            MessageBox.Show(" GST number Already exists for \n Customer     : " + cst.name + " \n CustomerID : " + cst.CustomerID + "");
                            grdCustomerDocuments.Rows.RemoveAt(rw.Index);
                            return false;
                        }
                    }
                    else if (rw.Cells["Document"].Value.ToString() == "PANNo")
                    {
                        string docid = "PANNo";
                        string panval = rw.Cells["DocumentValue"].Value.ToString();
                        if (!PAN.IsMatch(panval))
                        {
                            MessageBox.Show("Please check the PAN No...Entered!!!!");
                            grdCustomerDocuments.Rows.RemoveAt(rw.Index);
                            return false;
                        }
                        customer cst = CustomerDB.validateCustomerGst(panval, docid);

                        if (cst.CustomerID != null)
                        {
                            MessageBox.Show(" This PAN number Already exists for \n Customer     : " + cst.name + " \n CustomerID : " + cst.CustomerID + "");
                            grdCustomerDocuments.Rows.RemoveAt(rw.Index);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("verifyDocumentDetails() : Error");
                status = false;
            }
            return status;
        }
        private void createAndUpdateCustomerDocumentDetails(int custID)
        {
            try
            {
                customerdocumentdetails cdd;
                List<customerdocumentdetails> CustomerDocumentDetails = new List<customerdocumentdetails>();
                for (int i = 0; i < grdCustomerDocuments.Rows.Count; i++)
                {
                    try
                    {
                        cdd = new customerdocumentdetails();
                        cdd.CustomerRowID = custID;

                        //string selectedDocument = grdCustomerDocuments.Rows[i].Cells[0].Value.ToString();
                        //string DocumentID = selectedDocument.Substring(0, selectedDocument.IndexOf('-'));
                        string DocumentID = grdCustomerDocuments.Rows[i].Cells["Document"].Value.ToString();
                        cdd.DocumentID = DocumentID;
                        try
                        {
                            cdd.DocumentValue = grdCustomerDocuments.Rows[i].Cells["DocumentValue"].Value.ToString();
                        }
                        catch (Exception)
                        {
                            cdd.DocumentValue = "";
                        }
                        cdd.status = 1;
                        CustomerDocumentDetails.Add(cdd);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("createAndUpdateCustomerDocumentDetails() : Error creating customer document list");
                        return;
                    }
                }
                CustomerDocumentDetailsDB customerdocumentdetails = new CustomerDocumentDetailsDB();
                if (!customerdocumentdetails.updateCustomerDocumentDetails(custID.ToString(), CustomerDocumentDetails))
                {
                    MessageBox.Show("Failed to update customer document list. Please check the values");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("createAndUpdateCustomerDocumentDetails() : Error updating customer document list");
            }
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                CustomerDB cdb = new CustomerDB();

                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    string custid = "";
                    {
                        try
                        {
                            //string gd1 = cmbCustomerGroup1.SelectedItem.ToString().Trim().Substring(0, cmbCustomerGroup1.SelectedItem.ToString().Trim().IndexOf('-'));
                            //string gd2 = cmbCustomerGroup2.SelectedItem.ToString().Trim().Substring(0, cmbCustomerGroup3.SelectedItem.ToString().Trim().IndexOf('-'));
                            //string gd3 = cmbCustomerGroup3.SelectedItem.ToString().Trim().Substring(0, cmbCustomerGroup2.SelectedItem.ToString().Trim().IndexOf('-'));

                            string gd1 = ((Structures.ComboBoxItem)cmbCustomerGroup1.SelectedItem).HiddenValue;
                            string gd2 = ((Structures.ComboBoxItem)cmbCustomerGroup2.SelectedItem).HiddenValue;
                            string gd3 = ((Structures.ComboBoxItem)cmbCustomerGroup3.SelectedItem).HiddenValue;
                            string itc = gd1 + gd2 + gd3;
                            custid = CreateCustomerID(itc);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    if (cdb.ApproveCustomer(prevcust, custid))
                    {
                        MessageBox.Show("Customer Document Approved");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredCustomer(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private static Hashtable CustomerTable()
        {
            Hashtable ht = new Hashtable();
            customer st = new customer();
            CustomerDB customerdb = new CustomerDB();
            List<customer> Customerss = customerdb.getCustomers("", 7);
            foreach (customer cust in Customerss)
            {
                try
                {
                    string id = cust.CustomerID;
                    string key = id.Substring(0, 6);
                    string value = id.Substring(6, 4);
                    if (ht.ContainsKey(key))
                        ht[key] = value;
                    else
                        ht.Add(key, value);
                }
                catch (Exception ex)
                {
                }

            }
            return ht;
        }
        public string CreateCustomerID(string itc)
        {
            string id = "";
            int val = 1;
            Hashtable ht = CustomerTable();
            try
            {
                if (ht.ContainsKey(itc))
                {
                    val = Convert.ToInt32(ht[itc].ToString());
                    val = val + 1;
                    //ht[itc] = val;
                }
                else
                {
                    ht.Add(itc, val.ToString());
                    val = 1;
                }
                id = itc + val.ToString("0000");
            }
            catch (Exception ex)
            {
            }
            return id;
        }
        private void btnForward_Click(object sender, EventArgs e)
        {
            //removeControlsFromForwarderPanel();
            //lvApprover = new ListView();
            //lvApprover.Clear();
            //pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
            //pnlForwarder.BackColor = Color.DarkSeaGreen;
            //pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(600, 300));
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;
            frmPopup.Size = new Size(450, 300);
            lvApprover = DocEmpMappingDB.ApproverLV(docID, Login.empLoggedIn);
            lvApprover.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            //pnlForwarder.Controls.Remove(lvApprover);
            frmPopup.Controls.Add(lvApprover);

            Button lvForwrdOK = new Button();
            lvForwrdOK.BackColor = Color.Tan;
            lvForwrdOK.Text = "OK";
            lvForwrdOK.Location = new Point(40, 265);
            lvForwrdOK.Click += new System.EventHandler(this.lvForwardOK_Click);
            frmPopup.Controls.Add(lvForwrdOK);

            Button lvForwardCancel = new Button();
            lvForwardCancel.BackColor = Color.Tan;
            lvForwardCancel.Text = "CANCEL";
            lvForwardCancel.Location = new Point(130, 265);
            lvForwardCancel.Click += new System.EventHandler(this.lvForwardCancel_Click);
            frmPopup.Controls.Add(lvForwardCancel);
            frmPopup.ShowDialog();
            ////lvForwardCancel.Visible = false;

            //pnlForwarder.Visible = true;
            //pnlInner.Controls.Add(pnlForwarder);
            //pnlInner.BringToFront();
            //pnlForwarder.BringToFront();
            //pnlForwarder.Focus();
        }
        private void lvForwardOK_Click(object sender, EventArgs e)
        {
            try
            {
                {
                    int kount = 0;
                    string approverUID = "";
                    string approverUName = "";
                    foreach (ListViewItem itemRow in lvApprover.Items)
                    {
                        if (itemRow.Checked)
                        {
                            approverUID = itemRow.SubItems[2].Text;
                            approverUName = itemRow.SubItems[1].Text;
                            kount++;
                        }
                    }
                    //string ss = prevcust.ForwarderList;
                    if (kount == 0)
                    {
                        MessageBox.Show("Select one approver");
                        return;
                    }
                    if (kount > 1)
                    {
                        MessageBox.Show("Select only one approver");
                        return;
                    }
                    else
                    {
                        DialogResult dialog = MessageBox.Show("Are you sure to forward the document ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            //do forward activities
                            CustomerDB cdb = new CustomerDB();
                            prevcust.ForwardUser = approverUID;
                            string s = prevcust.ForwarderList;
                            prevcust.ForwarderList = prevcust.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (cdb.forwardCustomer(prevcust))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredCustomer(listOption);
                                setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void lvForwardCancel_Click(object sender, EventArgs e)
        {
            try
            {
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        //return the previous forward list and forwarder 
        private string getReverseString(string forwarderList)
        {
            string reverseString = "";
            try
            {
                string prevUser = "";
                string[] lst1 = forwarderList.Split(Main.delimiter2);
                for (int i = 0; i < lst1.Length - 1; i++)
                {
                    if (lst1[i].Trim().Length > 1)
                    {
                        string[] lst2 = lst1[i].Split(Main.delimiter1);

                        if (i == (lst1.Length - 2))
                        {
                            if (reverseString.Trim().Length > 0)
                            {
                                reverseString = reverseString + "!@#" + prevUser;
                            }
                        }
                        else
                        {
                            reverseString = reverseString + lst2[0] + Main.delimiter1 + lst2[1] + Main.delimiter1 + Main.delimiter2;
                            prevUser = lst2[1];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return reverseString;
        }
        private void btnReverse_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Are you sure to Reverse the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    string reverseStr = getReverseString(prevcust.ForwarderList);
                    //do forward activities

                    CustomerDB cdb = new CustomerDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevcust.ForwarderList = reverseStr.Substring(0, ind);
                        prevcust.ForwardUser = reverseStr.Substring(ind + 3);
                        prevcust.DocumentStatus = prevcust.DocumentStatus - 1;
                    }
                    else
                    {
                        prevcust.ForwarderList = "";
                        prevcust.ForwardUser = "";
                        prevcust.DocumentStatus = 1;
                    }
                    if (cdb.reverseCustomer(prevcust))
                    {
                        MessageBox.Show("Customer Details Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredCustomer(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnListDocument_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevcust.CustomerID + "-" + prevcust.name);
                pnlPDFViewer.Controls.Add(dgvDocumentList);
                dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
            }
            catch (Exception ex)
            {
            }
        }
        private void dgvDocumentList_CellContentClick(Object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView dgv = sender as DataGridView;
                string fileName = "";
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == 0)
                {
                    removePDFControls();
                    fileName = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    string subDir = prevcust.CustomerID + "-" + prevcust.name;
                    ////DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    dgv.Enabled = false;
                    fileName = DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    ////showPDFFile(fileName);
                    ////dgv.Visible = false;
                    System.Diagnostics.Process.Start(fileName);
                    dgv.Enabled = true;
                }

            }
            catch (Exception ex)
            {
            }
        }
        private void btnCloseDocument_Click(object sender, EventArgs e)
        {
            removePDFControls();
            showPDFFileGrid();
        }


        private void showPDFFile(string fname)
        {
            try
            {
                removePDFControls();
                AxAcroPDFLib.AxAcroPDF pdf = new AxAcroPDFLib.AxAcroPDF();
                pdf.Dock = System.Windows.Forms.DockStyle.Fill;
                pdf.Enabled = true;
                pdf.Location = new System.Drawing.Point(0, 0);
                pdf.Name = "pdfReader";
                pdf.OcxState = pdf.OcxState;
                ////pdf.OcxState = ((System.Windows.Forms.AxHost.State)(new System.ComponentModel.ComponentResourceManager(typeof(ViewerWindow)).GetObject("pdf.OcxState")));
                pdf.TabIndex = 1;
                pnlPDFViewer.Controls.Add(pdf);

                pdf.setShowToolbar(false);
                pdf.LoadFile(fname);
                pdf.setView("Fit");
                pdf.Visible = true;
                pdf.setZoom(100);
                pdf.setPageMode("None");
            }
            catch (Exception ex)
            {
            }
        }
        private void removePDFControls()
        {
            try
            {
                foreach (Control p in pnlPDFViewer.Controls)
                    if (p.GetType() == typeof(AxAcroPDFLib.AxAcroPDF))
                    {
                        AxAcroPDFLib.AxAcroPDF c = (AxAcroPDFLib.AxAcroPDF)p;
                        c.Visible = false;
                        pnlPDFViewer.Controls.Remove(c);
                        c.Dispose();
                    }
            }
            catch (Exception ex)
            {
            }
        }
        private void showPDFFileGrid()
        {
            try
            {
                foreach (Control p in pnlPDFViewer.Controls)
                    if (p.GetType() == typeof(DataGridView))
                    {
                        p.Visible = true;
                    }
            }
            catch (Exception ex)
            {
            }
        }
        private void removePDFFileGridView()
        {
            try
            {
                foreach (Control p in pnlPDFViewer.Controls)
                    if (p.GetType() == typeof(DataGridView))
                    {
                        p.Dispose();
                    }
            }
            catch (Exception ex)
            {
            }
        }
        private void removeControlsFromCommenterPanel()
        {
            try
            {
                //foreach (Control p in pnlCmtr.Controls)
                //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
                //    {
                //        p.Dispose();
                //    }
                pnlCmtr.Controls.Clear();
                Control nc = pnlCmtr.Parent;
                nc.Controls.Remove(pnlCmtr);
            }
            catch (Exception ex)
            {
            }
        }
        private void removeControlsFromForwarderPanel()
        {
            try
            {
                //foreach (Control p in pnlForwarder.Controls)
                //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
                //    {
                //        p.Dispose();
                //    }
                pnlForwarder.Controls.Clear();
                Control nc = pnlForwarder.Parent;
                nc.Controls.Remove(pnlForwarder);
            }
            catch (Exception ex)
            {
            }
        }
        private void disableTabPages()
        {
            foreach (TabPage tp in tabControl1.TabPages)
            {
                tp.Enabled = false; ;
            }
        }
        private void enableTabPages()
        {
            foreach (TabPage tp in tabControl1.TabPages)
            {
                tp.Enabled = true;
            }
        }
        private void setButtonVisibility(string btnName)
        {
            try
            {
                btnActionPending.Visible = true;
                btnApprovalPending.Visible = true;
                btnApproved.Visible = true;
                btnNew.Visible = false;
                btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                btnForward.Visible = false;
                btnApprove.Visible = false;
                btnReverse.Visible = false;
                disableTabPages();
                clearTabPageControls();
                handleNewButton();
                handleGrdViewButton();
                handleGrdEditButton();
                pnlButtomButtons.Visible = true;
                //----
                if (btnName == "init")
                {
                    ////btnNew.Visible = true; 24/11/2016
                    btnExit.Visible = true;
                }
                //else if (btnName == "Commenter")
                //{
                //    btnCancel.Visible = true;
                //    btnSave.Visible = true;
                //    pnlPDFViewer.Visible = true;
                //    //tabComments.Enabled = true;
                //    tabPDFViewer.Enabled = true;
                //}
                else if (btnName == "btnNew")
                {
                    btnNew.Visible = false; //added 24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = false;
                    tabPDFViewer.Enabled = false;

                }
                else if (btnName == "btnEditPanel") //called from cancel,save,forward,approve and reverse button events
                {
                    ////btnNew.Visible = true;
                    btnExit.Visible = true;
                }
                //gridview buttons
                else if (btnName == "Edit" || btnName == "TempEdit")
                {
                    pnlButtomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    //btnGetComments.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = true;
                }
                else if (btnName == "Approve")
                {
                    pnlButtomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                    disableTabPages();
                    tabControl1.SelectedTab = tabBasicDetails;
                }
                else if (btnName == "View")
                {
                    pnlButtomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    //tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabBasicDetails;
                }
                else if (btnName == "LoadDocument")
                {
                    pnlButtomButtons.Visible = false; //24/11/2016
                }


                pnlEditButtons.Refresh();
                //if the user privilege is only view, show only the Approved documents button
                int ups = getuserPrivilegeStatus();
                if (ups == 1 || ups == 0)
                {
                    grdList.Columns["Edit"].Visible = false;
                    grdList.Columns["Approve"].Visible = false;
                    btnActionPending.Visible = false;
                    btnApprovalPending.Visible = false;
                    btnApproved.Visible = false;
                    if (ups == 1)
                    {
                        grdList.Columns["View"].Visible = true;
                    }
                    else
                    {
                        grdList.Columns["View"].Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        void handleNewButton()
        {
            btnNew.Visible = false;
            if (Main.itemPriv[1])
            {
                btnNew.Visible = true;
            }
        }
        void handleGrdEditButton()
        {
            grdList.Columns["Edit"].Visible = false;
            grdList.Columns["Approve"].Visible = false;
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                if (listOption == 1)
                {
                    grdList.Columns["Edit"].Visible = true;
                    grdList.Columns["Approve"].Visible = true;
                }
            }
        }

        void handleGrdViewButton()
        {
            grdList.Columns["View"].Visible = false;
            //if any one of view,add and edit
            if (Main.itemPriv[0] || Main.itemPriv[1] || Main.itemPriv[2])
            {
                //list option 1 should not have view buttuon visible (edit is avialable)
                if (listOption != 1)
                {
                    grdList.Columns["View"].Visible = true;
                }
            }
        }
        int getuserPrivilegeStatus()
        {
            try
            {
                if (Main.itemPriv[0] && !Main.itemPriv[1] && !Main.itemPriv[2]) //only view
                    return 1;
                else if (Main.itemPriv[0] && (Main.itemPriv[1] || Main.itemPriv[2])) //view add and edit
                    return 2;
                else if (!Main.itemPriv[0] && (Main.itemPriv[1] || Main.itemPriv[2])) //view add and edit
                    return 3;
                else if (!Main.itemPriv[0] && !Main.itemPriv[1] || !Main.itemPriv[2]) //no privilege
                    return 0;
                else
                    return -1;
            }
            catch (Exception ex)
            {
            }
            return 0;
        }
        //call this form when new or edit buttons are clicked
        private void clearTabPageControls()
        {
            try
            {
                removePDFControls();
                removePDFFileGridView();
                removeControlsFromCommenterPanel();
                removeControlsFromForwarderPanel();
                dgvComments.Rows.Clear();
                //chkCommentStatus.Checked = false;
                //txtComments.Text = "";
                //grdPOPIDetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }

        private void grdCustomerBank_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //    ComboBox cb = e.Control as ComboBox;
            //    if (cb != null && e.Control.Name.Equals("Bank"))
            //    {
            //        object selectedValue = grdCustomerBank.Rows[e.row].Cells[e.ColumnIndex].Value;
            //    }
        }

        private void grdCustomerBank_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grdCustomerBank.IsCurrentCellDirty)
            {
                grdCustomerBank.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            filterGridData();
        }
        //void cb_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    MessageBox.Show("Selected index changed");
        //}
        private void filterGridData()
        {
            try
            {
                foreach (DataGridViewRow row in grdList.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {

                    foreach (DataGridViewRow row in grdList.Rows)
                    {
                        if (!row.Cells["CustomerName"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
                        {
                            row.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        //-- Validating Header and Detail String For Single Quotes

        private customer verifyHeaderInputString(customer cust)
        {
            try
            {
                cust.name = Utilities.replaceSingleQuoteWithDoubleSingleQuote(cust.name);
                cust.Address1 = Utilities.replaceSingleQuoteWithDoubleSingleQuote(cust.Address1);
                cust.Address2 = Utilities.replaceSingleQuoteWithDoubleSingleQuote(cust.Address2);
                cust.Address3 = Utilities.replaceSingleQuoteWithDoubleSingleQuote(cust.Address3);
                cust.Address4 = Utilities.replaceSingleQuoteWithDoubleSingleQuote(cust.Address4);
                cust.Phone = Utilities.replaceSingleQuoteWithDoubleSingleQuote(cust.Phone);
                cust.Fax = Utilities.replaceSingleQuoteWithDoubleSingleQuote(cust.Fax);
                cust.EmailID = Utilities.replaceSingleQuoteWithDoubleSingleQuote(cust.EmailID);
                cust.WebSite = Utilities.replaceSingleQuoteWithDoubleSingleQuote(cust.WebSite);
                cust.ContactPerson = Utilities.replaceSingleQuoteWithDoubleSingleQuote(cust.ContactPerson);
                cust.ClientList = Utilities.replaceSingleQuoteWithDoubleSingleQuote(cust.ClientList);
                cust.BillingAddress = Utilities.replaceSingleQuoteWithDoubleSingleQuote(cust.BillingAddress);
                cust.DeliveryAddress = Utilities.replaceSingleQuoteWithDoubleSingleQuote(cust.DeliveryAddress);
                cust.ProductList = Utilities.replaceSingleQuoteWithDoubleSingleQuote(cust.ProductList);
            }
            catch (Exception ex)
            {
            }
            return cust;
        }
        private void verifyDetailInputString()
        {
            try
            {
                for (int i = 0; i < grdCustomerDocuments.Rows.Count; i++)
                {
                    grdCustomerDocuments.Rows[i].Cells["DocumentValue"].Value = Utilities.replaceSingleQuoteWithDoubleSingleQuote(grdCustomerDocuments.Rows[i].Cells["DocumentValue"].Value.ToString());
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void cmbCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbCountry.SelectedItem.ToString() == Main.CountryCode)
                {
                    cmbState.Enabled = true;
                }
                else
                {
                    cmbState.SelectedIndex = -1;
                    cmbState.Enabled = false;
                }
            }
            catch (Exception)
            {
            }
            ////MessageBox.Show("cmbCountry_SelectedIndexChanged()");
        }

        private void txtCustomerName_TextChanged(object sender, EventArgs e)
        {
            try
            {
                err.Clear();
                if (txtCustomerName.Text.Trim().Length == 0)
                {
                    lv.Visible = false;
                    return;
                }

                lv.View = System.Windows.Forms.View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                lv.Visible = true;
                lv.Items.Clear();
                lv.Columns.Clear();
                lv.Columns.Add("Exixting Customer Name");
                lv.BackColor = Color.LightGray;
                foreach (customer cst in Allcustomername)
                {
                    if (cst.name.Trim().ToLower().Contains(txtCustomerName.Text.Trim().ToLower().ToString()))
                    {
                        //ListViewItem lv1 = new ListViewItem();
                        //lv1.SubItems.Add(cst.name);
                        lv.Items.Add(cst.name);
                    }
                }
                lv.Location = new Point(550, 30);
                lv.Size = new Size(250, 250);
                lv.Columns[0].Width = 240;
                tabBasicDetails.Controls.Add(lv);
            }
            catch (Exception ex)
            {

            }
        }

        private void txtCustomerName_Leave(object sender, EventArgs e)
        {
            lv.Visible = false;

            foreach (customer cst in Allcustomername)
            {
                if (cst.name.Trim().ToLower().Equals(txtCustomerName.Text.Trim().ToLower()))
                {
                    err.SetError(txtCustomerName, "Customer Name exists!!!");
                }
            }
        }

        private void CustomerSPL_Enter(object sender, EventArgs e)
        {
            try
            {
                string frmname = this.Name;
                string menuid = Main.menuitems.Where(x => x.pageLink == frmname).Select(x => x.menuItemID).FirstOrDefault().ToString();
                Main.itemPriv = Utilities.fillItemPrivileges(Main.userOptionArray, menuid);
            }
            catch (Exception ex)
            {
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Aval == 1)
                {
                    Aval = 0;
                    DataGridViewRow rw = grdCustomerDocuments.Rows[grdCustomerDocuments.RowCount - 1];
                    if (rw.Cells["Document"].Value.ToString() == "GST")
                    {
                        string docid = "GST";
                        string gstval = rw.Cells["DocumentValue"].Value.ToString();
                        if (!gst.IsMatch(gstval))
                        {
                            MessageBox.Show("Please check the GST...Entered!!!!Row Will be Removed");
                            grdCustomerDocuments.Rows.RemoveAt(rw.Index);
                        }
                        customer cst = CustomerDB.validateCustomerGst(gstval, docid);
                        if (cst.CustomerID != null)
                        {
                            MessageBox.Show("Row Will be Removed!!!This GST number Already exists for \n Customer     : " + cst.name + " \n CustomerID : " + cst.CustomerID + "");
                            grdCustomerDocuments.Rows.RemoveAt(rw.Index);
                        }
                    }
                    else if (rw.Cells["Document"].Value.ToString() == "PANNo")
                    {
                        string docid = "PANNo";
                        string panval = rw.Cells["DocumentValue"].Value.ToString();
                        if (!PAN.IsMatch(panval))
                        {
                            MessageBox.Show("Please check the PAN No...Entered!!!! Row Will be Removed");
                            grdCustomerDocuments.Rows.RemoveAt(rw.Index);
                        }
                        customer cst = CustomerDB.validateCustomerGst(panval, docid);

                        if (cst.CustomerID != null)
                        {
                            MessageBox.Show("Row Will be Removed!!!! This PAN number Already exists for \n Customer     : " + cst.name + " \n CustomerID : " + cst.CustomerID + "");
                            grdCustomerDocuments.Rows.RemoveAt(rw.Index);
                        }
                    }
                    //if (grdCustomerDocuments.RowCount > 0)
                    //{
                    //    foreach (DataGridViewRow rw in grdCustomerDocuments.Rows)
                    //    {
                    //        if (rw.Cells["Document"].Value.ToString() == "GST")
                    //        {
                    //            string docid = "GST";
                    //            string gstval = rw.Cells["DocumentValue"].Value.ToString();
                    //            if (!gst.IsMatch(gstval))
                    //            {
                    //                MessageBox.Show("Please check the GST...Entered!!!!Row Will be Removed");
                    //                grdCustomerDocuments.Rows.RemoveAt(rw.Index);
                    //            }
                    //            customer cst = CustomerDB.validateCustomerGst(gstval, docid);
                    //            if (cst.CustomerID != null)
                    //            {
                    //                MessageBox.Show("Row Will be Removed!!!This GST number Already exists for \n Customer     : " + cst.name + " \n CustomerID : " + cst.CustomerID + "");
                    //                grdCustomerDocuments.Rows.RemoveAt(rw.Index);
                    //            }
                    //        }
                    //        else if (rw.Cells["Document"].Value.ToString() == "PANNo")
                    //        {
                    //            string docid = "PANNo";
                    //            string panval = rw.Cells["DocumentValue"].Value.ToString();
                    //            if (!PAN.IsMatch(panval))
                    //            {
                    //                MessageBox.Show("Please check the PAN No...Entered!!!! Row Will be Removed");
                    //                grdCustomerDocuments.Rows.RemoveAt(rw.Index);
                    //            }
                    //            customer cst = CustomerDB.validateCustomerGst(panval, docid);

                    //            if (cst.CustomerID != null)
                    //            {
                    //                MessageBox.Show("Row Will be Removed!!!! This PAN number Already exists for \n Customer     : " + cst.name + " \n CustomerID : " + cst.CustomerID + "");
                    //                grdCustomerDocuments.Rows.RemoveAt(rw.Index);
                    //            }
                    //        }
                    //    }
                    //}
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void grdCustomerDocuments_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {


                if (grdCustomerDocuments.IsCurrentCellDirty)
                {
                    var currentcell = grdCustomerDocuments.CurrentCellAddress;
                    if (currentcell.X == 0)
                    {
                        DataGridViewComboBoxCell cel = (DataGridViewComboBoxCell)grdCustomerDocuments.Rows[currentcell.Y].Cells[0];

                        for (int i = 0; i < grdCustomerDocuments.RowCount; i++)
                        {
                            if (i != currentcell.Y)
                            {
                                if (cel.EditedFormattedValue.ToString().Trim() == grdCustomerDocuments.Rows[i].Cells[0].EditedFormattedValue.ToString().Trim())
                                {
                                    MessageBox.Show("This Document has already been added!!! Please select a different document..");
                                    grdCustomerDocuments.Rows.RemoveAt(currentcell.Y);
                                    //int rw = currentcell.Y;
                                    //grdCustomerDocuments.Rows[i].Cells[0].Value = null;
                                    Aval = 0;
                                    return;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}


