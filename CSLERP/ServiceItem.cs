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

namespace CSLERP
{
    public partial class ServiceItem : System.Windows.Forms.Form
    {
        string docID = "SERVICEITEM";
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Hashtable ht = new Hashtable();
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        serviceitem prevsitem = new serviceitem();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        ListView lvApprover = new ListView();
        string id;
        Timer filterTimer = new Timer();
        Boolean isTempEdit = false;
        public ServiceItem()
        {
            try
            {
                InitializeComponent();
                this.FormBorderStyle = FormBorderStyle.None;
                Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
                initVariables();
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                this.Width -= 100;
                this.Height -= 100;
                this.FormBorderStyle = FormBorderStyle.Fixed3D;
                String a = this.Size.ToString();
                grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdList.EnableHeadersVisualStyles = false;
                ListServiceItem(listOption);
                //applyPrivilege();
            }
            catch (Exception)
            {

            }
        }

        private void ListServiceItem(int option)
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
                ServiceItemsDB Serviceitemdb = new ServiceItemsDB();
                List<serviceitem> ServiceItems = Serviceitemdb.getFilteredServiceItems(userString, option);
                foreach (serviceitem sitem in ServiceItems)
                {
                    if (option == 1)
                    {
                        if (sitem.documentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["ServiceItemID"].Value = sitem.ServiceItemID;
                    grdList.Rows[grdList.RowCount - 1].Cells["Description"].Value = sitem.Name;
                    grdList.Rows[grdList.RowCount - 1].Cells["Group1"].Value = sitem.Group1CodeDescription;
                    grdList.Rows[grdList.RowCount - 1].Cells["Group2"].Value = sitem.Group2CodeDescription;
                    grdList.Rows[grdList.RowCount - 1].Cells["Group3"].Value = sitem.Group3CodeDescription;
                    grdList.Rows[grdList.RowCount - 1].Cells["Group1Code"].Value = sitem.Group1Code;
                    grdList.Rows[grdList.RowCount - 1].Cells["Group2Code"].Value = sitem.Group2Code;
                    grdList.Rows[grdList.RowCount - 1].Cells["Group3Code"].Value = sitem.Group3Code;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = sitem.status;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatus"].Value = sitem.documentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = sitem.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["HSNCode"].Value = sitem.HSNCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = sitem.CreateUserName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = sitem.ForwardUserName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = sitem.ApproveUserName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwarderLists"].Value = sitem.ForwarderList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Service Item listing");
            }
            setButtonVisibility("init");
            isTempEdit = false;
            txtSearch.Visible = true;
            lblSearch.Visible = true;

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
            pnlList.Visible = true;
            grdList.Visible = true;

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
                ServiceGroupDB.fillGroupValueCombo(cmbGroup1Select, 1);
                ServiceGroupDB.fillGroupValueCombo(cmbGroup2Select, 2);
                ServiceGroupDB.fillGroupValueCombo(cmbGroup3Select, 3);
                //fillTypeCombo(cmbGroup);
                userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
                lblStatus.Visible = false;
                cmbStatus.Visible = false;
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
                txtSearch.Visible = true;
                lblSearch.Visible = true;
                txtSearch.Text = "";
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
                cmbStatus.SelectedIndex = -1;
                txtServiceItemID.Text = "";
                txtName.Text = "";
                txtHSNCode1.Text = "";
                lblStatus.Visible = false;
                cmbStatus.Visible = false;
                isTempEdit = false;
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
                //enableControlsInnerPanel();
                closeAllPanels();
                //pnlActionButtons.Visible = false;
                lblActionHeader.Visible = false;
                clearData();
                btnSave.Text = "Save";
                pnlCVINROuter.Visible = true;
                pnlCVINRInner.Visible = true;
                setButtonVisibility("btnNew");
                isTempEdit = false;
            }
            catch (Exception)
            {

            }
        }
        private int getStatusCode(string statStr)
        {
            int code = 0;
            if (statStr == "Active")
                return 1;
            else if (statStr == "Deactive")
                return 0;
            return code;
        }
        private string getStatusString(int code)
        {
            string stat = "";
            if (code == 0)
                return "Deactive";
            else if (code == 1)
                return "Active";
            return stat;
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {
                serviceitem sitem = new serviceitem();
                ServiceItemsDB Serviceitemdb = new ServiceItemsDB();
                //cvi.ConversionDate = dtDate.Value;

                try
                {
                    sitem.Group1Code = cmbGroup1Select.SelectedItem.ToString().Trim().Substring(0,cmbGroup1Select.SelectedItem.ToString().Trim().IndexOf('-'));
                    sitem.Group1CodeDescription = cmbGroup1Select.SelectedItem.ToString().Trim().Substring(cmbGroup1Select.SelectedItem.ToString().Trim().IndexOf('-') +1);
                }
                catch (Exception)
                {
                    sitem.Group1Code = "";
                    sitem.Group1CodeDescription = "";
                }
                try
                {
                    sitem.Group2Code = cmbGroup2Select.SelectedItem.ToString().Trim().Substring(0, cmbGroup2Select.SelectedItem.ToString().Trim().IndexOf('-'));
                    sitem.Group2CodeDescription = cmbGroup2Select.SelectedItem.ToString().Trim().Substring(cmbGroup2Select.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                }
                catch (Exception)
                {
                    sitem.Group2Code = "";
                    sitem.Group2CodeDescription = "";
                }
                try
                {
                    sitem.Group3Code = cmbGroup3Select.SelectedItem.ToString().Trim().Substring(0, cmbGroup3Select.SelectedItem.ToString().Trim().IndexOf('-'));
                    sitem.Group3CodeDescription = cmbGroup3Select.SelectedItem.ToString().Trim().Substring(cmbGroup3Select.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                }
                catch (Exception)
                {
                    sitem.Group3Code = "";
                    sitem.Group3CodeDescription = "";
                }
                //sitem.ServiceItemID = CreateServiceItemID(itc);
                sitem.Name = txtName.Text;
                sitem.ForwarderList = prevsitem.ForwarderList;
                sitem.HSNCode = txtHSNCode1.Text;
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (btnText.Equals("Update"))
                {
                    if (isTempEdit)
                        sitem.status = getStatusCode(cmbStatus.SelectedItem.ToString());
                    else
                        sitem.status = 0;
                }
                if (Serviceitemdb.validateServiceItem(sitem))
                {
                    if (btnText.Equals("Update"))
                    {

                        if (Serviceitemdb.updateServiceItem(sitem, prevsitem))
                        {
                            MessageBox.Show("Service Item updated");
                            closeAllPanels();
                            listOption = 1;
                            ListServiceItem(listOption);
                        }
                        else
                        {
                            status = false;
                            MessageBox.Show("Failed to Update Service Item");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        sitem.documentStatus = 1;//created
                        if (Serviceitemdb.insertServiceItem(sitem))
                        {
                            MessageBox.Show("Service Item Added");
                            closeAllPanels();
                            listOption = 1;
                            ListServiceItem(listOption);
                        }
                        else
                        {
                            status = false;
                            MessageBox.Show("Failed to Insert Service Item");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Service Item Data Validation failed");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Failed Adding / Editing Service Item");
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }
        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // pnlActionButtons.Visible = false;
                lblActionHeader.Visible = false;
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("LoadDocument") ||
                    columnName.Equals("View") || columnName.Equals("ViewDocument") || columnName.Equals("TempEdit"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    prevsitem = new serviceitem();
                    int rowID = e.RowIndex;
                    txtSearch.Visible = false;
                    lblSearch.Visible = false;
                    try
                    {

                        prevsitem.ServiceItemID = grdList.Rows[e.RowIndex].Cells["ServiceItemID"].Value.ToString();
                        prevsitem.Name = grdList.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                        prevsitem.documentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentStatus"].Value.ToString());
                        prevsitem.CreateUser = grdList.Rows[e.RowIndex].Cells["CreateUser"].Value.ToString();
                        prevsitem.ForwarderList = grdList.Rows[e.RowIndex].Cells["ForwarderLists"].Value.ToString();
                        prevsitem.HSNCode = grdList.Rows[e.RowIndex].Cells["HSNCode"].Value.ToString();
                    }
                    catch (Exception ez)
                    {

                    }
                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "ServiceItemID" + prevsitem.ServiceItemID + "\n" +
                            "ServiceItem name:" + prevsitem.Name;
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevsitem.ServiceItemID + "-" + prevsitem.Name;
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
                    if(columnName.Equals("TempEdit"))
                    {
                        lblStatus.Visible = true;
                        cmbStatus.Visible = true;
                        isTempEdit = true;
                        cmbStatus.SelectedIndex = cmbStatus.
                            FindString(getStatusString(Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["Status"].Value)));
                    }
                    btnSave.Text = "Update";

                    //dtDate.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells[0].Value.ToString());
                    txtServiceItemID.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtName.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    txtHSNCode1.Text = grdList.Rows[e.RowIndex].Cells["HSNCode"].Value.ToString();
                    cmbGroup1Select.SelectedIndex = cmbGroup1Select.FindString(grdList.Rows[e.RowIndex].Cells["Group1Code"].Value.ToString());
                    cmbGroup2Select.SelectedIndex = cmbGroup2Select.FindString(grdList.Rows[e.RowIndex].Cells["Group2Code"].Value.ToString());
                    cmbGroup3Select.SelectedIndex = cmbGroup3Select.FindString(grdList.Rows[e.RowIndex].Cells["Group3Code"].Value.ToString());
                    if (columnName.Equals("Approve"))
                    {
                        string gd1 = cmbGroup1Select.SelectedItem.ToString().Trim().Substring(0, cmbGroup1Select.SelectedItem.ToString().Trim().IndexOf('-'));
                        string gd2 = cmbGroup2Select.SelectedItem.ToString().Trim().Substring(0, cmbGroup2Select.SelectedItem.ToString().Trim().IndexOf('-'));
                        string gd3 = cmbGroup3Select.SelectedItem.ToString().Trim().Substring(0, cmbGroup3Select.SelectedItem.ToString().Trim().IndexOf('-'));
                        string itc = gd1 + gd2 + gd3;
                        id = CreateServiceItemID(itc);
                    }
                    pnlCVINRInner.Visible = true;
                    pnlCVINROuter.Visible = true;
                    pnlList.Visible = false;
                }
              
            }
            catch (Exception ex)
            {

            }
        }
        private Hashtable ServiceItemIdTable()
        {
            Hashtable ht = new Hashtable();
            serviceitem st = new serviceitem();
            ServiceItemsDB Serviceitemdb = new ServiceItemsDB();
            List<serviceitem> ServiceItems = Serviceitemdb.getServiceItems();
            foreach (serviceitem sitem in ServiceItems)
            {
                try
                {

                    string id = sitem.ServiceItemID;
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
        public string CreateServiceItemID(string itc)
        {
            string id = "";
            int val = 1;
            Hashtable ht = ServiceItemIdTable();
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
            removeControlsFromForwarderPanel();
            lvApprover = new ListView();
            lvApprover.Clear();
            pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
            pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(500, 300));

            lvApprover = DocEmpMappingDB.ApproverLV(docID, Login.empLoggedIn);
            lvApprover.Bounds = new Rectangle(new Point(50, 50), new Size(400, 200));
            pnlForwarder.Controls.Remove(lvApprover);
            pnlForwarder.Controls.Add(lvApprover);

            Button lvForwrdOK = new Button();
            lvForwrdOK.Text = "OK";
            lvForwrdOK.Size = new Size(150, 20);
            lvForwrdOK.Location = new Point(50, 270);
            lvForwrdOK.Click += new System.EventHandler(this.lvForwardOK_Click);
            pnlForwarder.Controls.Add(lvForwrdOK);

            Button lvForwardCancel = new Button();
            lvForwardCancel.Text = "Cancel";
            lvForwardCancel.Size = new Size(150, 20);
            lvForwardCancel.Location = new Point(250, 270);
            lvForwardCancel.Click += new System.EventHandler(this.lvForwardCancel_Click);
            pnlForwarder.Controls.Add(lvForwardCancel);
            ////lvForwardCancel.Visible = false;

            pnlForwarder.Visible = true;
            pnlCVINROuter.Controls.Add(pnlForwarder);
            pnlCVINROuter.BringToFront();
            pnlForwarder.BringToFront();
            pnlForwarder.Focus();

        }
        //private void btnForward_Click(object sender, EventArgs e)
        //{
        //}
        private void removeControlsFromForwarderPanel()
        {
            try
            {
                foreach (Control p in pnlForwarder.Controls)
                    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
                    {
                        p.Dispose();
                    }
            }
            catch (Exception ex)
            {
            }
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
                            ServiceItemsDB sidb = new ServiceItemsDB();
                            prevsitem.ForwardUser = approverUID;
                            string s = prevsitem.ForwarderList;
                            prevsitem.ForwarderList = prevsitem.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (sidb.forwardServiceItem(prevsitem))
                            {
                                pnlCmtr.Visible = false;
                                pnlCmtr.Controls.Remove(lvApprover);
                                MessageBox.Show("Document Forwarded");
                                closeAllPanels();
                                listOption = 1;
                                ListServiceItem(listOption);
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
                lvApprover.CheckBoxes = false;
                lvApprover.CheckBoxes = true;
                pnlForwarder.Controls.Remove(lvApprover);
                pnlForwarder.Visible = false;
            }
            catch (Exception)
            {
            }
        }
        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            txtSearch.Text = "";
            ListServiceItem(listOption);
        }
        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            txtSearch.Text = "";
            ListServiceItem(listOption);
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
            ListServiceItem(listOption);
        }
       

        private void btnApprove_Click_1(object sender, EventArgs e)
        {
            try
            {
                ServiceItemsDB Serviceitemdb = new ServiceItemsDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (Serviceitemdb.ApproveServiceItem(prevsitem, id))
                    {
                        MessageBox.Show("Service Item Approved");
                        closeAllPanels();
                        listOption = 1;
                        ListServiceItem(listOption);
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
                    string reverseStr = getReverseString(prevsitem.ForwarderList);
                    //do forward activities
                    //prevpopi.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevpopi.CommentStatus);
                    ServiceItemsDB sidb = new ServiceItemsDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevsitem.ForwarderList = reverseStr.Substring(0, ind);
                        prevsitem.ForwardUser = reverseStr.Substring(ind + 3);
                        prevsitem.documentStatus = prevsitem.documentStatus - 1;
                    }
                    else
                    {
                        prevsitem.ForwarderList = "";
                        prevsitem.ForwardUser = "";
                        prevsitem.documentStatus = 1;
                    }
                    if (sidb.reverseServiceITem(prevsitem))
                    {
                        MessageBox.Show("Service ITem Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListServiceItem(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

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
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevsitem.ServiceItemID + "-" + prevsitem.Name);
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
                    string subDir = prevsitem.ServiceItemID + "-" + prevsitem.Name;
                    ////DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    fileName = DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
                    ////showPDFFile(fileName);
                    ////dgv.Visible = false;
                    System.Diagnostics.Process.Start(fileName);
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
            txtSearch.Visible = true;
            lblSearch.Visible = true;
        }
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
                        if (!row.Cells["Description"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            ////MessageBox.Show("txtSearch_TextChanged() : started");
            ////filterTimer = new Timer();
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
            filterGridData();
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }

        private void ServiceItem_Enter(object sender, EventArgs e)
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

