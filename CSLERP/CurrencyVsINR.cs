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

namespace CSLERP
{
    public partial class CurrencyVsINR : System.Windows.Forms.Form
    {
        string docID = "CVINR";
        string forwarderList = "";
        string approverList = "";
        Panel pnlForwarder = new Panel();
        Panel pnlCmtr = new Panel();
        string userString = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        cvinr prevcvi = new cvinr();
        ListView lvApprover = new ListView();
        Form frmPopup = new Form();
        public CurrencyVsINR()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void CurrencyVsINR_Load(object sender, EventArgs e)
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
            ListFilteredCVINR(listOption);
            //applyPrivilege();
        }

        private void ListFilteredCVINR(int option)
        {
            try
            {
                grdList.Rows.Clear();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                CurrencyVsINRDB dbrecord = new CurrencyVsINRDB();
                List<cvinr> CVINRs = dbrecord.getFilteredCVINR(userString, option);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (cvinr cvi in CVINRs)
                {
                    if (option == 1)
                    {
                        if (cvi.documentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }

                    grdList.Rows.Add(cvi.ConversionDate,
                        cvi.CurrencyID + "-" + cvi.CurrencyName,
                        ComboFIll.getCurrencyConversionTypeString(cvi.type),
                        cvi.INRValue,
                        // ComboFIll.getDocumentStatusString(cvi.documentStatus),
                        cvi.status,
                        cvi.documentStatus,
                        cvi.CreateUser,
                        cvi.ForwardUser,
                        cvi.ApproveUser, cvi.ForwarderList);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in CVINR listing");
            }
            setButtonVisibility("init");
            pnlCVINRList.Visible = true;

            grdList.Columns["Creator"].Visible = true;
            grdList.Columns["Forwarder"].Visible = true;
            grdList.Columns["Approver"].Visible = true;
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
                dtDate.Format = DateTimePickerFormat.Custom;
                dtDate.CustomFormat = "dd-MM-yyyy";
                CurrencyDB.fillCurrencyCombo(cmbCurrency);
                fillTypeCombo(cmbType);
                setButtonVisibility("init");
                closeAllPanels();
                userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            }
            catch (Exception)
            {

            }
        }

        private void fillTypeCombo(System.Windows.Forms.ComboBox cmb)
        {
            try
            {
                cmb.Items.Clear();
                for (int i = 0; i < Main.CurrencyConversionTypeValues.GetLength(0); i++)
                {
                    cmb.Items.Add(Main.CurrencyConversionTypeValues[i, 1]);
                }
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
                pnlCVINRList.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                clearData();
                pnlForwarder.Visible = false;
                pnlCVINRList.Visible = true;
               
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
                dtDate.Value = DateTime.Now;
                cmbType.SelectedIndex = -1;
                cmbCurrency.SelectedIndex = -1;
                txtINRValue.Text = "";
                removeControlsFromForwarderPanel();
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
                clearData();
                btnSave.Text = "Save";
                pnlCVINROuter.Visible = true;
                pnlCVINRInner.Visible = true;
                dtDate.Enabled = true;
                cmbCurrency.Enabled = true;
                cmbType.Enabled = true;
                setButtonVisibility("btnNew");
            }
            catch (Exception)
            {

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {
                cvinr cvi = new cvinr();
                CurrencyVsINRDB cvinrDB = new CurrencyVsINRDB();
                cvi.ConversionDate = dtDate.Value;

                try
                {
                    cvi.CurrencyID = cmbCurrency.SelectedItem.ToString().Trim().Substring(0, cmbCurrency.SelectedItem.ToString().Trim().IndexOf('-'));
                    cvi.CurrencyName = cmbCurrency.SelectedItem.ToString().Trim().Substring(cmbCurrency.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                    cvi.INRValue = float.Parse(txtINRValue.Text);
                    cvi.ForwarderList = prevcvi.ForwarderList;
                    cvi.type = ComboFIll.getCurrencyConversionTypeCode(cmbType.SelectedItem.ToString());
                }
                catch (Exception ex)
                {
                    status = false;
                    MessageBox.Show("CVINR Data Validation failed");
                    return;
                }

                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;


                if (cvinrDB.validateCVINR(cvi))
                {
                    //cvi.documentStatus = prevcvi.documentStatus;
                    if (btnText.Equals("Update"))
                    {
                        if (cvinrDB.updateCVINR(cvi, prevcvi))
                        {
                            MessageBox.Show("CVINR updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredCVINR(listOption);
                        }
                        else
                        {
                            status = false;
                            MessageBox.Show("Failed to update CVINR");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        cvi.documentStatus = 1;//created
                        if (cvinrDB.insertCVINR(cvi))
                        {
                            MessageBox.Show("CVINR Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredCVINR(listOption);
                        }
                        else
                        {
                            status = false;
                            MessageBox.Show("Failed to Insert CVINR");
                        }
                    }
                }
                else
                {
                    status = false;
                    MessageBox.Show("CVINR Data Validation failed");
                    return;
                }
            }
            catch (Exception ex)
            {
                status = false;
                MessageBox.Show("Failed Adding / Editing CVINR");
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }

        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("View"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    prevcvi = new cvinr();
                    int rowID = e.RowIndex;
                    prevcvi.ConversionDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells[0].Value.ToString());
                    try
                    {
                        prevcvi.CurrencyID = grdList.Rows[e.RowIndex].Cells[1].Value.ToString().Trim().Substring(0, grdList.Rows[e.RowIndex].Cells[1].Value.ToString().Trim().IndexOf('-'));
                        prevcvi.CurrencyName = grdList.Rows[e.RowIndex].Cells[1].Value.ToString().Trim().Substring(grdList.Rows[e.RowIndex].Cells[1].Value.ToString().IndexOf('-') + 1);
                        prevcvi.type = ComboFIll.getCurrencyConversionTypeCode(grdList.Rows[e.RowIndex].Cells[2].Value.ToString());
                        prevcvi.documentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells[5].Value.ToString());
                        prevcvi.CreateUser = grdList.Rows[e.RowIndex].Cells[6].Value.ToString();
                        prevcvi.ForwarderList = grdList.Rows[e.RowIndex].Cells["ForwarderLists"].Value.ToString();
                    }
                    catch (Exception)
                    {
                        prevcvi.CurrencyID = "";
                        prevcvi.CurrencyName = "";
                    }
                    btnSave.Text = "Update";

                    dtDate.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells[0].Value.ToString());
                    cmbCurrency.SelectedIndex = cmbCurrency.FindStringExact(grdList.Rows[e.RowIndex].Cells[1].Value.ToString());
                    cmbType.SelectedIndex = cmbType.FindStringExact(grdList.Rows[e.RowIndex].Cells[2].Value.ToString());
                    txtINRValue.Text = grdList.Rows[e.RowIndex].Cells[3].Value.ToString();
                    pnlCVINRInner.Visible = true;
                    pnlCVINROuter.Visible = true;
                    pnlCVINRList.Visible = false;

                }
            }
            catch (Exception)
            {

            }
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            //removeControlsFromForwarderPanel();
            //lvApprover = new ListView();
            //lvApprover.Clear();
            //pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
            //pnlForwarder.BackColor = Color.DarkSeaGreen;
            //pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(500, 300));
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
            //pnlForwarder.Controls.Add(lvApprover);
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
            //pnlCVINRInner.Controls.Add(pnlForwarder);
            //pnlCVINRInner.BringToFront();
            //pnlForwarder.BringToFront();
            //pnlForwarder.Focus();

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
        private void lvForwardOK_Click(object sender, EventArgs e)
        {
            try
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
                        CurrencyVsINRDB cvinrDB = new CurrencyVsINRDB();
                        prevcvi.ForwardUser = approverUID;
                        string s = prevcvi.ForwarderList;
                        prevcvi.ForwarderList = prevcvi.ForwarderList + approverUName + Main.delimiter1 +
                            approverUID + Main.delimiter1 + Main.delimiter2;

                        if (cvinrDB.forwardCVINR(prevcvi))
                        {
                            frmPopup.Close();
                            frmPopup.Dispose();
                            MessageBox.Show("CVINR Forwarded");
                            if (!updateDashBoard(prevcvi, 1))
                            {
                                MessageBox.Show("DashBoard Fail to update");
                            }
                            closeAllPanels();
                            pnlForwarder.Controls.Remove(lvApprover);
                            pnlForwarder.Visible = false;
                            listOption = 1;
                            ListFilteredCVINR(listOption);
                            setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
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
            ListFilteredCVINR(listOption);
        }
        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredCVINR(listOption);
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

            ListFilteredCVINR(listOption);
        }
        private Boolean updateDashBoard(cvinr cvi, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = "CVINR";
                //dsb.TemporaryNo = cvi.TemporaryNo;
                //dsb.TemporaryDate = cvi.TemporaryDate;
                //dsb.DocumentNo = cvi.InternalOrderNo;
                //dsb.DocumentDate = cvi.InternalOrderDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = cvi.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver("CVINR");
                    foreach (documentreceiver docRec in docList)
                    {
                        dsb.ToUser = docRec.EmployeeID;   //To store UserID Form DocumentReceiver for current Document
                        dsb.DocumentDate = UpdateTable.getSQLDateTime();
                        if (!ddsDB.insertDashboardAlarm(dsb))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                            status = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                CurrencyVsINRDB cvinrDB = new CurrencyVsINRDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (cvinrDB.ApproveCVINR(prevcvi))
                    {
                        MessageBox.Show("CVINR Approved");
                        if (!updateDashBoard(prevcvi, 2))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                        }
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredCVINR(listOption);
                        setButtonVisibility("btnEditPanel");
                    }
                }
            }
            catch (Exception)
            {
            }
        }


        private void btnReverse_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Are you sure to Reverse the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    string reverseStr = getReverseString(prevcvi.ForwarderList);
                    //do forward activities
                    //prevpopi.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevpopi.CommentStatus);
                    CurrencyVsINRDB cvinrDB = new CurrencyVsINRDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevcvi.ForwarderList = reverseStr.Substring(0, ind);
                        prevcvi.ForwardUser = reverseStr.Substring(ind + 3);
                        prevcvi.documentStatus = prevcvi.documentStatus - 1;
                    }
                    else
                    {
                        prevcvi.ForwarderList = "";
                        prevcvi.ForwardUser = "";
                        prevcvi.documentStatus = 1;
                    }
                    if (cvinrDB.reverseCVINR(prevcvi))
                    {
                        MessageBox.Show("CVINR Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredCVINR(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception ex)
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
        private void setButtonVisibility(string btnName)
        {
            try
            {
                lblActionHeader.Visible = true;
                pnlTopButtons.Visible = true;
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
                enableControlsInnerPanel();
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
                    pnlTopButtons.Visible = false;
                    lblActionHeader.Visible = false;
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
                    pnlTopButtons.Visible = false;
                    lblActionHeader.Visible = false;
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (btnName == "Approve")
                {
                    pnlTopButtons.Visible = false;
                    lblActionHeader.Visible = false;
                    disableControlsInnerPanel();
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                }
                else if (btnName == "View")
                {
                    pnlTopButtons.Visible = false;
                    lblActionHeader.Visible = false;
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableControlsInnerPanel();

                }

                pnlEditbuttons.Refresh();
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
        private void disableControlsInnerPanel()
        {
            try
            {
                foreach (Control p in pnlCVINRInner.Controls)
                    if (p.GetType() == typeof(Label) || p.GetType() == typeof(TextBox) || p.GetType() == typeof(ComboBox) || p.GetType() == typeof(DateTimePicker))
                    {
                        p.Enabled = false;
                    }
            }
            catch (Exception ex)
            {
            }
        }
        private void enableControlsInnerPanel()
        {
            try
            {
                foreach (Control p in pnlCVINRInner.Controls)
                    if (p.GetType() == typeof(Label) || p.GetType() == typeof(TextBox) || p.GetType() == typeof(ComboBox) || p.GetType() == typeof(DateTimePicker))
                    {
                        if (!(p.Name == "txtStockItemID"))
                            p.Enabled = true;
                    }
            }
            catch (Exception ex)
            {
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

        private void CurrencyVsINR_Enter(object sender, EventArgs e)
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

