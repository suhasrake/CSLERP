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
using System.Collections;
using CSLERP.DBData;
using System.Threading;

namespace CSLERP
{
    public partial class AccountCode : System.Windows.Forms.Form
    {
        string docID = "ACCOUNTCODE";
        string forwarderList = "";
        System.Windows.Forms.Timer filterTimer = new System.Windows.Forms.Timer();
        string colName = "";
        string approverList = "";
        string userString = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Hashtable ht = new Hashtable();
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        accountcode prevaccode;
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        ListView lvApprover = new ListView();
        Form frmPopup = new Form();
        string id;
        Boolean isTempEditCLick = false;
        public AccountCode()
        {
            try
            {
                InitializeComponent();
               
            }
            catch (Exception)
            {

            }
        }
        private void AccountCode_Load(object sender, EventArgs e)
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
            ListAccountDetail(listOption);
            //applyPrivilege();
        }
        private void ListAccountDetail(int option)
        {
            try
            {
               // pnlActionButtons.Visible = true;
                lblActionHeader.Visible = true;
                grdList.Rows.Clear();
                txtSearch.Text = "";
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                AccountCodeDB ACDB = new AccountCodeDB();
                List<accountcode> ACItems = ACDB.getFilteredAccountDetails(userString, option);
                foreach (accountcode acc in ACItems)
                {
                    if (option == 1)
                    {
                        if (acc.documentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["AccCode"].Value = acc.AccountCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["AccName"].Value = acc.Name;
                    grdList.Rows[grdList.RowCount - 1].Cells["GroupLevel1"].Value = acc.GroupLevel1;
                    grdList.Rows[grdList.RowCount - 1].Cells["GroupLevel1Desc"].Value = acc.GroupLevel1Description;
                    grdList.Rows[grdList.RowCount - 1].Cells["GroupLevel2"].Value = acc.GroupLevel2;
                    grdList.Rows[grdList.RowCount - 1].Cells["GroupLevel2Desc"].Value = acc.GroupLevel2eDescription;
                    grdList.Rows[grdList.RowCount - 1].Cells["GroupLevel3"].Value = acc.GroupLevel3;
                    grdList.Rows[grdList.RowCount - 1].Cells["GroupLevel3Desc"].Value = acc.GroupLevel3Description;
                    grdList.Rows[grdList.RowCount - 1].Cells["GroupLevel4"].Value = acc.GroupLevel4;
                    grdList.Rows[grdList.RowCount - 1].Cells["GroupLevel4Desc"].Value = acc.GroupLevel4Description;
                    grdList.Rows[grdList.RowCount - 1].Cells["GroupLevel5"].Value = acc.GroupLevel5;
                    grdList.Rows[grdList.RowCount - 1].Cells["GroupLevel5Desc"].Value = acc.GroupLevel5Description;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = acc.status;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatus"].Value = acc.documentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = acc.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = acc.CreateUserName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = acc.ForwardUserName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = acc.ApproveUserName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwarderLists"].Value = acc.ForwarderList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Account Code listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;

            grdList.Columns["Creator"].Visible = true;
            grdList.Columns["Forwarder"].Visible = true;
            grdList.Columns["Approver"].Visible = true;
            if(listOption == 3 || listOption == 6)
                grdList.Columns["TempEdit"].Visible = true;
            else
                grdList.Columns["TempEdit"].Visible = false;
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
                AccountGroupDB.fillAccountGroupValueCombo(cmbGroup1Select, 1);
                AccountGroupDB.fillAccountGroupValueCombo(cmbGroup2Select, 2);
                AccountGroupDB.fillAccountGroupValueCombo(cmbGroup3Select, 3);
                AccountGroupDB.fillAccountGroupValueCombo(cmbGroup4Select, 4);
                AccountGroupDB.fillAccountGroupValueCombo(cmbGroup5Select, 5);
                userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;          
                setButtonVisibility("init");
            }
            catch (Exception)
            {

            }
        }
       
        private void closeAllPanels()
        {
            try
            {
                pnlCVINRInner.Visible = false;
                pnlCVINROuter.Visible = false;
                pnlList.Visible = false;
                pnlPDFViewer.Visible = false;
            }
            catch (Exception)
            {

            }
        }
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            try
            {
                lblActionHeader.Visible = true;
                closeAllPanels();
                clearData();
                pnlList.Visible = true;
                grdList.Visible = true;
                setButtonVisibility("btnEditPanel");
            }
            catch (Exception)
            {

            }
        }
        
        public void clearData()
        {
            try
            {
                ////dtDate.Value = DateTime.Now;
              
                pnlForwarder.Visible = false;
                cmbGroup1Select.SelectedIndex = -1;
                cmbGroup2Select.SelectedIndex = -1;
                cmbGroup3Select.SelectedIndex = -1;
                cmbGroup4Select.SelectedIndex = -1;
                cmbGroup5Select.SelectedIndex = -1;
                txtAccCode.Text = "";
                txtName.Text = "";
                isTempEditCLick = false;
                //foreach (ComboBox cmb in pnlCVINRInner.Controls.OfType<ComboBox>())
                //{
                //    cmb.Enabled = true;
                //}
                //txtName.Enabled = true;
                cmbStatus.Visible = false;
                lblStatus.Visible = false;
                prevaccode = new accountcode();
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
                closeAllPanels();
                lblActionHeader.Visible = false;
                clearData();
                btnSave.Text = "Save";
                pnlCVINROuter.Visible = true;
                pnlCVINRInner.Visible = true;
                cmbStatus.Visible = false;
                lblStatus.Visible = false;
                setButtonVisibility("btnNew");
                pnlCVINRInner.Enabled = true;
                isTempEditCLick = false;
            }
            catch (Exception)
            {

            }
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {
                accountcode acc = new accountcode();
                AccountCodeDB accDB = new AccountCodeDB();
                //cvi.ConversionDate = dtDate.Value;

                try
                {
                    acc.GroupLevel1 = cmbGroup1Select.SelectedItem.ToString().Trim().Substring(0,cmbGroup1Select.SelectedItem.ToString().Trim().IndexOf('-'));
                    acc.GroupLevel1Description = cmbGroup1Select.SelectedItem.ToString().Trim().Substring(cmbGroup1Select.SelectedItem.ToString().Trim().IndexOf('-') +1);
                }
                catch (Exception)
                {
                    acc.GroupLevel1 = "";
                    acc.GroupLevel1Description = "";
                }
                try
                {
                    acc.GroupLevel2 = cmbGroup2Select.SelectedItem.ToString().Trim().Substring(0, cmbGroup2Select.SelectedItem.ToString().Trim().IndexOf('-'));
                    acc.GroupLevel2eDescription = cmbGroup2Select.SelectedItem.ToString().Trim().Substring(cmbGroup2Select.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                }
                catch (Exception)
                {
                    acc.GroupLevel2 = "";
                    acc.GroupLevel2eDescription = "";
                }
                try
                {
                    acc.GroupLevel3 = cmbGroup3Select.SelectedItem.ToString().Trim().Substring(0, cmbGroup3Select.SelectedItem.ToString().Trim().IndexOf('-'));
                    acc.GroupLevel3Description = cmbGroup3Select.SelectedItem.ToString().Trim().Substring(cmbGroup3Select.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                }
                catch (Exception)
                {
                    acc.GroupLevel3 = "";
                    acc.GroupLevel3Description = "";
                }
                try
                {
                    acc.GroupLevel4 = cmbGroup4Select.SelectedItem.ToString().Trim().Substring(0, cmbGroup4Select.SelectedItem.ToString().Trim().IndexOf('-'));
                    acc.GroupLevel4Description = cmbGroup4Select.SelectedItem.ToString().Trim().Substring(cmbGroup4Select.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                }
                catch (Exception)
                {
                    acc.GroupLevel4 = "";
                    acc.GroupLevel4Description = "";
                }
                try
                {
                    acc.GroupLevel5 = cmbGroup5Select.SelectedItem.ToString().Trim().Substring(0, cmbGroup5Select.SelectedItem.ToString().Trim().IndexOf('-'));
                    acc.GroupLevel5Description = cmbGroup5Select.SelectedItem.ToString().Trim().Substring(cmbGroup5Select.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                }
                catch (Exception ex)
                {
                    acc.GroupLevel5 = "";
                    acc.GroupLevel5Description = "";
                }
                acc.Name = txtName.Text.Replace("'", "''");
                 
                acc.ForwarderList = prevaccode.ForwarderList;
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (accDB.validateAccountCodeDetail(acc))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (isTempEditCLick)
                        {
                            acc.status = getstatuscode(cmbStatus.SelectedItem.ToString().Trim());
                        }
                        else
                            acc.status = 0;
                        if (accDB.updateAccountCodeDetails(acc, prevaccode))
                        {
                            MessageBox.Show("Account Code updated");
                            closeAllPanels();
                            listOption = 1;
                            ListAccountDetail(listOption);
                        }
                        else
                        {
                            status = false;
                            MessageBox.Show("Failed to Update Account Code");
                        }

                    }
                    else if (btnText.Equals("Save"))
                    {
                        acc.documentStatus = 1;//created
                        if (accDB.insertAccountCodeDetails(acc))
                        {
                            MessageBox.Show("Account Code Added");
                            closeAllPanels();
                            listOption = 1;
                            ListAccountDetail(listOption);
                        }
                        else
                        {
                            status = false;
                            MessageBox.Show("Failed to Insert Account Code");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Account Code Data Validation failed");
                    return;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Failed Adding / Editing Account Code");
                return;
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }
        private int getstatuscode(string str)
        {
            int code = 0;
            if (str == "Active")
                code = 1;
            else if (str == "Deactive")
                code = 0;
            return code;
        }
        private string getstatusString(int code)
        {
            string str = "";
            if (code == 1)
                str = "Active";
            else 
                str = "Deactive";
            return str;
        }
        //private void btnSave_Click(object sender, EventArgs e)
        //{


        //}

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
               // pnlActionButtons.Visible = false;
                lblActionHeader.Visible = false;
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit") || columnName.Equals("TempEdit") || columnName.Equals("Approve") || columnName.Equals("LoadDocument") ||
                    columnName.Equals("View") || columnName.Equals("ViewDocument"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    prevaccode = new accountcode();
                    int rowID = e.RowIndex;
                   
                    try
                    {
                        prevaccode.AccountCode = grdList.Rows[e.RowIndex].Cells["AccCode"].Value.ToString();
                        prevaccode.Name = grdList.Rows[e.RowIndex].Cells["AccName"].Value.ToString();
                        prevaccode.documentStatus =Convert.ToInt32( grdList.Rows[e.RowIndex].Cells["DocumentStatus"].Value.ToString());
                        prevaccode.CreateUser = grdList.Rows[e.RowIndex].Cells["CreateUser"].Value.ToString();
                        prevaccode.ForwarderList = grdList.Rows[e.RowIndex].Cells["ForwarderLists"].Value.ToString();
                    }
                    catch (Exception ez)
                    {
                        
                    }
                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Account NO" + prevaccode.AccountCode + "\n" +
                            "Account name:" + prevaccode.Name;
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevaccode.AccountCode + "-" + prevaccode.Name;
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_1(null, null);
                        return;
                    }
                    //--------
                    if (columnName.Equals("ViewDocument"))
                    {
                        return;
                    }

                    if (columnName.Equals("TempEdit"))
                    {
                        cmbStatus.Visible = true;
                        lblStatus.Visible = true;                       
                        isTempEditCLick = true;
                    }
                    else
                    {
                        isTempEditCLick = false;
                        cmbStatus.Visible = false;
                        lblStatus.Visible = false;
                    }
                    btnSave.Text = "Update";
                    
                    //dtDate.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells[0].Value.ToString());
                    txtAccCode.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtName.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();

                    string grp1 = grdList.Rows[e.RowIndex].Cells["GroupLevel1"].Value.ToString();
                    cmbGroup1Select.SelectedIndex = grp1.Length == 0 ? -1 : cmbGroup1Select.FindString(grp1);

                    string grp2 = grdList.Rows[e.RowIndex].Cells["GroupLevel2"].Value.ToString();
                    cmbGroup2Select.SelectedIndex = grp2.Length == 0 ? -1 : cmbGroup2Select.FindString(grp2);

                    string grp3 = grdList.Rows[e.RowIndex].Cells["GroupLevel3"].Value.ToString();
                    cmbGroup3Select.SelectedIndex = grp3.Length == 0 ? -1 : cmbGroup3Select.FindString(grp3);

                    string grp4 = grdList.Rows[e.RowIndex].Cells["GroupLevel4"].Value.ToString();
                    cmbGroup4Select.SelectedIndex = grp4.Length == 0 ? -1 : cmbGroup4Select.FindString(grp4);

                    string grp5 = grdList.Rows[e.RowIndex].Cells["GroupLevel5"].Value.ToString();
                    cmbGroup5Select.SelectedIndex = grp5.Length == 0 ? -1 : cmbGroup5Select.FindString(grp5);

                    cmbStatus.SelectedIndex =
                        cmbStatus.FindString(getstatusString(Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["Status"].Value)));
                    if (columnName.Equals("Approve"))
                    {
                        string gd1 = cmbGroup1Select.SelectedItem.ToString().Trim().Substring(0,cmbGroup1Select.SelectedItem.ToString().Trim().IndexOf('-'));
                        string gd2 = cmbGroup2Select.SelectedItem.ToString().Trim().Substring(0,cmbGroup2Select.SelectedItem.ToString().Trim().IndexOf('-'));
                        string gd3 = cmbGroup3Select.SelectedItem.ToString().Trim().Substring(0,cmbGroup3Select.SelectedItem.ToString().Trim().IndexOf('-'));
                        string gd4 = cmbGroup4Select.SelectedItem.ToString().Trim().Substring(0, cmbGroup4Select.SelectedItem.ToString().Trim().IndexOf('-'));
                        string gd5 = cmbGroup5Select.SelectedItem.ToString().Trim().Substring(0, cmbGroup5Select.SelectedItem.ToString().Trim().IndexOf('-'));
                        string itc = gd1 + gd2 + gd3 + gd4 + gd5;
                        id = CreateAccountID(itc);
                    }
                    pnlCVINRInner.Visible = true;
                    pnlCVINROuter.Visible = true;
                    pnlList.Visible = false;
                }
                else
                {
                    return;
                }
                ////pnlCVINRInner.Visible = true;
                ////pnlCVINROuter.Visible = true;
                ////pnlList.Visible = false;
            }
            catch (Exception ex)
            {
                setButtonVisibility("init");
            }
        }
        private static Hashtable AccountIdTable()
        {
            Hashtable ht = new Hashtable();
            //accountcode acc = new accountcode();
            AccountCodeDB accDB = new AccountCodeDB();
            List<accountcode> AccList = accDB.getAccoutCodesList();
            foreach (accountcode accode in AccList)
            {
                try
                {

                    string id = accode.AccountCode;
                    if(id.Length == 10)
                    {
                        string key = id.Substring(0, 6);
                        string value = id.Substring(6, 4);
                        if (ht.ContainsKey(key))
                            ht[key] = value;
                        else
                            ht.Add(key, value);
                    }
                    else
                    {
                        string key = id.Substring(0, 10);
                        string value = id.Substring(10, 4);
                        if (ht.ContainsKey(key))
                            ht[key] = value;
                        else
                            ht.Add(key, value);
                    }
                  
                }
                catch (Exception ex)
                {
                }

            }
            return ht;
        }
        public string CreateAccountID(string itc)
        {
            string id = "";
            int val = 1;
            Hashtable ht = AccountCode.AccountIdTable();
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
        private void btnForward_Click_1(object sender, EventArgs e)
        {
            //removeControlsFromForwarderPanel();
            //lvApprover = new ListView();
            //lvApprover.Clear();
            //pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
            //pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(500, 300));
            lvApprover = new ListView();
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
                            AccountCodeDB accDB = new AccountCodeDB();
                            prevaccode.ForwardUser = approverUID;
                            string s = prevaccode.ForwarderList;
                            prevaccode.ForwarderList = prevaccode.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (accDB.forwardAccountCode(prevaccode))
                            {
                                MessageBox.Show("Account Code Document Forwarded");

                                closeAllPanels();
                                listOption = 1;
                                ListAccountDetail(listOption);
                                setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                                frmPopup.Close();
                                frmPopup.Dispose();
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
        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListAccountDetail(listOption);
        }
        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListAccountDetail(listOption);
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

            ListAccountDetail(listOption);
        }
       

        private void btnApprove_Click_1(object sender, EventArgs e)
        {
            try
            {
                AccountCodeDB accDB = new AccountCodeDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (accDB.ApproveAccountCode(prevaccode, id))
                    {
                        MessageBox.Show("Account Code Approved");
                        closeAllPanels();
                        listOption = 1;
                        ListAccountDetail(listOption);
                        setButtonVisibility("btnEditPanel");
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void btnReverse_Click_1(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Are you sure to Reverse the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    string reverseStr = getReverseString(prevaccode.ForwarderList);
                    //do forward activities
                    //prevpopi.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevpopi.CommentStatus);
                    AccountCodeDB accDB = new AccountCodeDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevaccode.ForwarderList = reverseStr.Substring(0, ind);
                        prevaccode.ForwardUser = reverseStr.Substring(ind + 3);
                        prevaccode.documentStatus = prevaccode.documentStatus - 1;
                    }
                    else
                    {
                        prevaccode.ForwarderList = "";
                        prevaccode.ForwardUser = "";
                        prevaccode.documentStatus = 1;
                    }
                    if (accDB.reverseAccountCode(prevaccode))
                    {
                        MessageBox.Show("Account Code Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListAccountDetail(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        //private void btnReverse_Click(object sender, EventArgs e)
        //{
           
        //}
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
        private void setButtonVisibility(string btnName)
        {
            try
            {
                pnlTopButtons.Visible = true;
                lblActionHeader.Visible = true; 
                pnlCVINRInner.Enabled = true;
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
                handleNewButton();
                handleGrdViewButton();
                handleGrdEditButton();
                pnlBottomButtons.Visible = true;
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
                //}
                else if (btnName == "btnNew")
                {
                    lblActionHeader.Visible = false;
                    pnlTopButtons.Visible = false;
                    btnNew.Visible = false; //added 24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (btnName == "btnEditPanel") //called from cancel,save,forward,approve and reverse button events
                {
                    ////btnNew.Visible = true;
                    btnExit.Visible = true;
                }
                //gridview buttons
                else if (btnName == "Edit")
                {
                    lblActionHeader.Visible = false;
                    pnlTopButtons.Visible = false;
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (btnName == "TempEdit")
                {
                    lblActionHeader.Visible = false;
                    pnlTopButtons.Visible = false;
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (btnName == "Approve")
                {
                    lblActionHeader.Visible = false;
                    pnlTopButtons.Visible = false;
                    pnlCVINRInner.Enabled = false;
                    //lvApprover.Enabled = true;
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                }
                else if (btnName == "View")
                {
                    lblActionHeader.Visible = false;
                    pnlTopButtons.Visible = false;
                    pnlBottomButtons.Visible = false; //24/11/2016
                    pnlCVINRInner.Enabled = false;
                    btnCancel.Visible = true;
                }
                else if (btnName == "LoadDocument")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                }
                else if (btnName == "ViewDocument")
                {
                    lblActionHeader.Visible = false;
                    pnlTopButtons.Visible = false;
                    btnCancel.Visible = true;
                    btnNew.Visible = false;
                    btnActionPending.Visible = false;
                    btnApprovalPending.Visible = false;
                    btnApproved.Visible = false;
                    pnlCVINRInner.Visible = false;
                    pnlCVINROuter.Visible = false;
                    grdList.Visible = false;
                    pnlPDFViewer.Visible = true;
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
        //private void disableControlsInnerPanel()
        //{
        //    try
        //    {
        //        foreach (Control p in pnlCVINRInner.Controls)
        //            if (p.GetType() == typeof(Label) || p.GetType() == typeof(TextBox) || p.GetType() == typeof(ComboBox))
        //            {
        //                p.Enabled = false;
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //private void enableControlsInnerPanel()
        //{
        //    try
        //    {
        //        foreach (Control p in pnlCVINRInner.Controls)
        //            if (p.GetType() == typeof(Label) || p.GetType() == typeof(TextBox) || p.GetType() == typeof(ComboBox))
        //            {
        //                if (!(p.Name == "txtStockItemID"))
        //                    p.Enabled = true;
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
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
        private void btnListDocument_Click_1(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevaccode.AccountCode + "-" + prevaccode.Name);
                pnlPDFViewer.Controls.Add(dgvDocumentList);
                dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
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
        private void btnCloseDocument_Click_1(object sender, EventArgs e)
        {
            removePDFControls();
            showPDFFileGrid();
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
                    ////string docDir = Main.documentDirectory + "\\" + docID;
                    string subDir = prevaccode.AccountCode + "-" + prevaccode.Name;
                    dgv.Enabled = false;
                    fileName = DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    System.Diagnostics.Process.Start(fileName);
                    dgv.Enabled = true;
                }

            }
            catch (Exception ex)
            {
            }
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
        private void btnClose_Click_1(object sender, EventArgs e)
        {
            lblActionHeader.Visible = true;
            pnlTopButtons.Visible = true;
            removePDFFileGridView();
            removePDFControls();
            pnlPDFViewer.Visible = false;
            pnlList.Visible = true;
            grdList.Visible = true;
            btnNew.Visible = true;
            btnExit.Visible = true;
            btnActionPending.Visible = true;
            btnApprovalPending.Visible = true;
            btnApproved.Visible = true;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterTimer.Tick -= new System.EventHandler(this.handlefilterTimerTimeout);
            filterTimer.Tick += new System.EventHandler(this.handlefilterTimerTimeout);
            filterTimer.Interval = 500;
            filterTimer.Enabled = true;
            filterTimer.Start();
        }
        private void handlefilterTimerTimeout(object sender, EventArgs e)
        {
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterGridData(colName);
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }
        private void filterGridData(string clm)
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
                        if (clm == null || clm.Length == 0)
                        {
                            if (!row.Cells["AccName"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
                            {
                                row.Visible = false;
                            }
                        }
                        else
                        {

                            if (!row.Cells[clm].FormattedValue.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
                            {
                                row.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void grdList_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                colName = grdList.Columns[e.ColumnIndex].Name;
                foreach (DataGridViewColumn col in grdList.Columns)
                {
                    col.HeaderCell.Style.BackColor = Color.LightBlue;
                }
                grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdList.Columns[e.ColumnIndex].HeaderCell.Style.BackColor = Color.Magenta;
            }
            catch (Exception ex)
            {
            }
        }

        private void AccountCode_Enter(object sender, EventArgs e)
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
    }
}

