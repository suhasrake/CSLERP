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
using System.Globalization;
using CSLERP.FileManager;
using CSLERP.PrintForms;
using System.Collections;

namespace CSLERP
{
    public partial class MaterialDeliveryDetail : System.Windows.Forms.Form
    {
        string docID = "MATERIALDELIVERYDETAIL";
        string[] docList = { "PRODUCTINVOICEOUT", "PRODUCTEXPORTINVOICEOUT" };
        string userString = "";
        string userCommentStatusString = "";
        double productvalue = 0.0;
        double taxvalue = 0.0;
        string IODocid = "";
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        string commentStatus = "";
        int listOption = 2; //1-Pending, 2-In Process, 3-Approved
        int status = 0;
        ListView lv = new ListView();
        ListView lvCopy = new ListView();
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        materialdelivery MatDel = new materialdelivery();
        TreeView tv = new TreeView();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        Panel pnlModel = new Panel();
        Panel pnlv = new Panel();
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Form frmPopup = new Form();
        Panel pnllv = new Panel();
        RichTextBox txt = new RichTextBox();
        List<documentreceiver> DocREcvList = new List<documentreceiver>();
        string DocRecvListstr = "";
        public MaterialDeliveryDetail()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void MaterialDeliveryDetail_Load(object sender, EventArgs e)
        {
            //////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;

            String a = this.Size.ToString();
            DocREcvList = Main.DocumentReceivers.Where(rec => docList.Contains(rec.DocumentID)).ToList();
            DocRecvListstr = getRecvDocString(DocREcvList);
            grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdList.EnableHeadersVisualStyles = false;
            ListFilteredMaterialDeliveryDetail(listOption);
        }

        private string getRecvDocString(List<documentreceiver> rcvlist)
        {
            string strList = "";
            if (rcvlist.Count == 0)
            {
                strList = "";
            }
            else
            {
                ArrayList arrStr = new ArrayList();
                foreach (documentreceiver rcvr in rcvlist)
                {
                    string qry = "( DocumentID = '" + rcvr.DocumentID + "' and OfficeID = '" + rcvr.OfficeID + "')";
                    if (docList.Contains(rcvr.DocumentID) && !arrStr.Contains(qry))
                    {
                        arrStr.Add("( DocumentID = '" + rcvr.DocumentID + "' and OfficeID = '" + rcvr.OfficeID + "')");
                    }

                }
                strList = string.Join(" or ", arrStr.ToArray());
            }
            return strList;
        }


        private void ListFilteredMaterialDeliveryDetail(int option)
        {
            try
            {
                grdList.Rows.Clear();
                MaterialDeliveryDetailDB MaterialDetDB = new MaterialDeliveryDetailDB();
                List<materialdelivery> materialdel = MaterialDetDB.getFilteredMaterialdetail(option, DocRecvListstr);
                if (option == 1)
                    lblActionHeader.Text = "List of New Documents";
                else if (option == 2)
                    lblActionHeader.Text = "Status";
                foreach (materialdelivery mdd in materialdel)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["RowID"].Value = mdd.RowID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = mdd.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentType"].Value = mdd.DocumentType;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentNo"].Value = mdd.DocumentNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentDate"].Value = mdd.DocumentDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gConsignee"].Value = mdd.consignee;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCoustemerName"].Value = mdd.customername;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCourierID"].Value = mdd.courierID;
                    grdList.Rows[grdList.RowCount - 1].Cells["Couriername"].Value = mdd.Couriername;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTransportaionMode"].Value = mdd.transportationMode;
                    grdList.Rows[grdList.RowCount - 1].Cells["gLRNo"].Value = mdd.LRNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gLRDate"].Value = mdd.LRDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gRemarks"].Value = mdd.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = mdd.status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = mdd.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = mdd.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDeliveryDate"].Value = mdd.DeliveryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["CustomerApprovalNo"].Value = mdd.CustomerApprovalNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDeliveryStatus"].Value = setStatus(mdd.DeliveryStatus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Maerial Delivery Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;
        }
        public string setStatus(int status)
        {
            string stat = "";
            if (status == 1)
            {
                stat = "InTransit";
            }
            else if (status == 2)
            {
                stat = "Delivered";
            }
            return stat;
        }
        public int setStatusString(string status)
        {
            int stat = 0;
            if (status == "InTransit")
            {
                stat = 1;
            }
            else if (status == "Delivered")
            {
                stat = 2;
            }
            return stat;
        }

        private void initVariables()
        {
            if (getuserPrivilegeStatus() == 1)
            {
                listOption = 2;
            }

            CustomerDB.fillCustomerComboNew(cmbConsignee);
            //CustomerDB.fillCustomerComboNew(cmbCourierID);
            CatalogueValueDB.fillCatalogValueComboNew(cmbTransportationMode, "TransportationMode");
            CustomerDB.fillLedgerTypeComboNew(cmbCourierID, "Transporter");
            //CatalogueValueDB.fillCatalogValueCombo(cmbTransportationMode, "TransportationMode");
            txtDocNo.Enabled = false;
            dtDocDate.Enabled = false;
            dtDocDate.Format = DateTimePickerFormat.Custom;
            dtDocDate.CustomFormat = "dd-MM-yyyy";
            dtDocDate.Enabled = false;
            dtDocDate.Format = DateTimePickerFormat.Custom;
            dtDocDate.CustomFormat = "dd-MM-yyyy";
            dtDeliveryDate.Enabled = false;
            dtDeliveryDate.Format = DateTimePickerFormat.Custom;
            dtDeliveryDate.CustomFormat = "dd-MM-yyyy";
            dtDocDate.Enabled = false;
            dtLRDate.Format = DateTimePickerFormat.Custom;
            dtLRDate.CustomFormat = "dd-MM-yyyy";
            pnlUI.Controls.Add(pnlList);
            btnFinalize.Visible = false;
            {
                TaxDetailsTable.Columns.Add("TaxItem", typeof(string));
                TaxDetailsTable.Columns.Add("TaxAmount", typeof(double));
            }
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            rdbInTransit.Visible = true;
            rdbDelivered.Visible = true;
            rdbInTransit.Checked = true;
            setButtonVisibility("init");

        }

        private void closeAllPanels()
        {
            try
            {
                pnlList.Visible = false;
                pnlAddEdit.Visible = false;
            }
            catch (Exception)
            {
            }
        }

        public void clearData()
        {
            try
            {
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                //----------clear temperory panels

                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                //----------
                cmbConsignee.SelectedIndex = -1;
                cmbCourierID.SelectedIndex = -1;
                cmbTransportationMode.SelectedIndex = -1;
                cmbDocType.SelectedItem = null;
                dtDocDate.Value = DateTime.Parse("01-01-1900");
                dtDocDate.Value = DateTime.Parse("01-01-1900");
                txtLRNo.Text = "";
                txtDocNo.Text = "";
                IODocid = "";
                txtCustmrApprvlNo.Text = "";
                dtLRDate.Value = DateTime.Today.Date;
                //removeControlsFromLVPanel();
            }
            catch (Exception ex)
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
                clearData();
                btnSave.Text = "Save";
                pnlAddEdit.Visible = true;
                closeAllPanels();
                pnlAddEdit.Visible = true;
                cmbDocType.Enabled = true;

                setButtonVisibility("btnNew");
            }
            catch (Exception)
            {

            }
        }



        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            rdbInTransit.Visible = false;
            rdbDelivered.Visible = false;
            rdbInTransit.Checked = false;
            rdbDelivered.Checked = false;
            ListFilteredMaterialDeliveryDetail(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            rdbInTransit.Visible = true;
            rdbDelivered.Visible = true;
            rdbInTransit.Checked = true;
            ListFilteredMaterialDeliveryDetail(listOption);
            btnNew.Visible = false;
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

            ListFilteredMaterialDeliveryDetail(listOption);
        }
        public Boolean verifyAndReworkMaterialDetail()
        {
            bool status = false;
            try
            {
                if (txtDocNo.Text == "" || txtLRNo.Text == "" ||
                    cmbConsignee.SelectedIndex == -1 || cmbCourierID.SelectedIndex == -1)
                {
                    status = false;
                }
                else
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            if (listOption == 1)
            {
                try
                {

                    MaterialDeliveryDetailDB Materialdeliverydb = new MaterialDeliveryDetailDB();
                    materialdelivery materialdel = new materialdelivery();
                    System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                    string btnText = btnSave.Text;

                    try
                    {

                        if (!verifyAndReworkMaterialDetail())
                        {
                            MessageBox.Show("Fill all the Details");
                            return;
                        }
                        materialdel.RowID = MatDel.RowID;
                        materialdel.DocumentID = IODocid;
                        materialdel.DocumentNo = Convert.ToInt32(txtDocNo.Text);
                        materialdel.DocumentDate = dtDocDate.Value;
                        materialdel.CustomerApprovalNo = txtCustmrApprvlNo.Text.ToString();
                        ////////materialdel.consignee = cmbConsignee.SelectedItem.ToString().Trim().Substring(0, cmbConsignee.SelectedItem.ToString().Trim().IndexOf('-'));
                        materialdel.consignee = ((Structures.ComboBoxItem)cmbConsignee.SelectedItem).HiddenValue;
                        materialdel.LRNo = txtLRNo.Text;
                        materialdel.LRDate = dtLRDate.Value;
                        materialdel.DocumentType = cmbDocType.SelectedItem.ToString().Trim();
                        ////////materialdel.courierID = cmbCourierID.SelectedItem.ToString().Trim().Substring(0, cmbCourierID.SelectedItem.ToString().Trim().IndexOf('-')).Trim();
                        materialdel.courierID = ((Structures.ComboBoxItem)cmbCourierID.SelectedItem).HiddenValue;
                        materialdel.transportationMode = ((Structures.ComboBoxItem)cmbTransportationMode.SelectedItem).HiddenValue;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Validation failed");
                        return;
                    }
                    if (btnText.Equals("Save"))
                    {
                        materialdel.DeliveryStatus = 1; //created
                    }
                    else
                    {

                        materialdel.DeliveryStatus = MatDel.DeliveryStatus;
                    }
                    if (btnText.Equals("Update"))
                    {
                        if (Materialdeliverydb.updateMaterialDelivery(materialdel))
                        {
                            MessageBox.Show(" Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredMaterialDeliveryDetail(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update ");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (Materialdeliverydb.insertMaterialDetail(materialdel))
                        {
                            MessageBox.Show(" Details Added");
                            closeAllPanels();
                            listOption = 1;
                            IODocid = "";
                            ListFilteredMaterialDeliveryDetail(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert ");
                        }
                    }
                    else
                    {
                        MessageBox.Show(" Validation failed");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("btnSave_Click_1() : Error");
                    return;
                }
                if (status)
                {
                    setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
                }
            }
            else if (listOption == 2)
            {
                try
                {
                    materialdelivery matdel = new materialdelivery();
                    MaterialDeliveryDetailDB MATDB = new MaterialDeliveryDetailDB();
                    matdel.RowID = MatDel.RowID;
                    matdel.DocumentID = MatDel.DocumentID;
                    matdel.DocumentDate = dtDocDate.Value;
                    matdel.DocumentNo = Convert.ToInt32(txtDocNo.Text);
                    matdel.DeliveryDate = dtDeliveryDate.Value;
                    matdel.DeliveryStatus = setStatusString(cmbDeliveryStatus.SelectedItem.ToString().Trim());
                    matdel.Remarks = txtRemarks.Text;
                    System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                    string btnText = btn.Text;
                    {
                        if (btnText.Equals("Update"))
                        {
                            if (cmbDeliveryStatus.SelectedItem.ToString() == "InTransit")
                            {

                                if (MATDB.updateMaterialDeliveryStatus(matdel))
                                {
                                    MessageBox.Show("Document Remarks updated");
                                    closeAllPanels();
                                    listOption = 2;
                                    ListFilteredMaterialDeliveryDetail(listOption);
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update Document Remarks");
                                }
                            }
                            else if (cmbDeliveryStatus.SelectedItem.ToString() == "Delivered")
                            {
                                if (validatedelivery())
                                {
                                    if (MATDB.updateDelivery(matdel))
                                    {
                                        MessageBox.Show("Document updated");
                                        closeAllPanels();
                                        closeAllPanels();
                                        listOption = 2;
                                        ListFilteredMaterialDeliveryDetail(listOption);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to update Document Remarks");
                                    }

                                }
                                else
                                {
                                    MessageBox.Show("vALIDATION FAILED!!!Please Check Delivery Date and Remarks");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Failed Adding / Editing User Data");
                }
            }
        }

        public Boolean validatedelivery()
        {
            bool st = true;
            {
                if (cmbDeliveryStatus.SelectedItem.ToString() == "Delivered")
                {
                    if (dtDeliveryDate.Value.ToString() == "01-01-0001" || txtRemarks.Text == "")
                    {
                        st = false;
                    }
                }
            }
            return st;
        }


        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }

        private void btnAddLine_Click_1(object sender, EventArgs e)
        {

        }

        private void grdPOPIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void showStockItemTreeView()
        {
        }
        private void tvOK_Click(object sender, EventArgs e)
        {
        }
        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnForward_Click_1(object sender, EventArgs e)
        {
            removeControlsFromForwarderPanel();
            lvApprover = new ListView();
            lvApprover.Clear();
            pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
            pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
            lvApprover = DocEmpMappingDB.ApproverLV(docID, Login.empLoggedIn);
            lvApprover.Bounds = new Rectangle(new Point(50, 50), new Size(500, 200));
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

            pnlForwarder.Visible = true;
            pnlAddEdit.Controls.Add(pnlForwarder);
            pnlAddEdit.BringToFront();
            pnlForwarder.BringToFront();
            pnlForwarder.Focus();

        }

        private void btnApprove_Click_1(object sender, EventArgs e)
        {
            try
            {
                MaterialDeliveryDetailDB MDDB = new MaterialDeliveryDetailDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Finalize the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (status == 1)
                    {
                        if (MDDB.FinalizeDetails(MatDel))
                        {
                            MessageBox.Show(" Document Finalized");
                            //if (!updateDashBoard(prevpopi, 2))
                            //{
                            //    MessageBox.Show("DashBoard Fail to update");
                            //}
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredMaterialDeliveryDetail(listOption);
                            setButtonVisibility("btnEditPanel");
                        }
                    }
                    else
                    {
                        MessageBox.Show("DeliveryStatus is null");
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private Boolean updateDashBoard(popiheader popih, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = popih.DocumentID;
                dsb.TemporaryNo = popih.TemporaryNo;
                dsb.TemporaryDate = popih.TemporaryDate;
                dsb.DocumentNo = popih.TrackingNo;
                dsb.DocumentDate = popih.TrackingDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = popih.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(MatDel.DocumentID);
                    foreach (documentreceiver docRec in docList)
                    {
                        dsb.ToUser = docRec.EmployeeID;
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

        private void cmbPOType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string Doctype = cmbDocType.SelectedItem.ToString().Trim();
                setDocType(Doctype);
                cmbDocType.Enabled = false;
            }
            catch (Exception ex)
            {
            }
        }
        private void setDocType(string Doctype)
        {
            try
            {
                txtCustmrApprvlNo.Visible = true;
                Cst.Visible = true;
                if (Doctype == "Invoice")
                {
                    btnInvoiceSelect.Visible = true;
                    txtDocNo.Enabled = false;
                    dtDocDate.Enabled = false;
                    txtLRNo.ReadOnly = false;
                    cmbConsignee.Enabled = false;
                    cmbCourierID.Enabled = true;
                    ////cmbTransportationMode.Enabled = false;
                    
                    if (MatDel.status == 0)
                    {
                        lblTransportationMode.Visible = true;
                        cmbTransportationMode.Visible = true;
                    }
                    else
                    {
                        lblTransportationMode.Visible = false;
                        cmbTransportationMode.Visible = false;
                    }

                    lblLRNo.Visible = true;
                    txtLRNo.Visible = true;
                    lblLRDate.Visible = true;
                    dtLRDate.Visible = true;
                    lblDocNo.Visible = true;
                    lblDocDate.Visible = true;
                    txtDocNo.Visible = true;
                    dtDocDate.Visible = true;
                    btnInvoiceSelect.Visible = true;
                    dtDocDate.Visible = true;
                    lblDocDate.Visible = true;
                    lblCourierID.Visible = true;
                    cmbCourierID.Visible = true;
                    lblConsignee.Visible = true;
                    cmbConsignee.Visible = true;
                }
                else if (Doctype == "Others")
                {
                    btnInvoiceSelect.Visible = false;
                    txtDocNo.Visible = true;
                    dtDocDate.Visible = true;
                    txtDocNo.Enabled = true;
                    dtDocDate.Enabled = true;
                    txtLRNo.ReadOnly = false;
                    cmbConsignee.Enabled = true;
                    cmbCourierID.Enabled = true;
                    lblTransportationMode.Visible = true;
                    cmbTransportationMode.Visible = true;
                    lblLRNo.Visible = true;
                    txtLRNo.Visible = true;
                    lblLRDate.Visible = true;
                    dtLRDate.Visible = true;
                    lblDocNo.Visible = true;
                    lblDocDate.Visible = true;
                    dtDocDate.Visible = true;
                    lblDocDate.Visible = true;
                    lblCourierID.Visible = true;
                    cmbCourierID.Visible = true;
                    lblConsignee.Visible = true;
                    cmbConsignee.Visible = true;
                }
            }
            catch (Exception)
            {
            }
        }

        private void pnlList_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lvOK_Click(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show("Update the document for sending the comment requests");
                if (lvCmtr.CheckedItems.Count > 0)
                {
                    foreach (ListViewItem itemRow in lvCmtr.Items)
                    {
                        if (itemRow.Checked)
                        {
                            commentStatus = commentStatus + itemRow.SubItems[1].Text + Main.delimiter1 +
                                itemRow.SubItems[2].Text + Main.delimiter1 +
                                "0" + Main.delimiter1 + Main.delimiter2;
                        }
                    }
                }
                else
                {
                    //if the existing commenter are removed
                    commentStatus = "Cleared";
                }
                pnlCmtr.Visible = false;
                pnlCmtr.Controls.Remove(lvCmtr);
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_Click(object sender, EventArgs e)
        {
            try
            {
                lvCmtr.CheckBoxes = false;
                lvCmtr.CheckBoxes = true;
                pnlCmtr.Visible = false;
            }
            catch (Exception)
            {
            }
        }
        private void lvForwardOK_Click(object sender, EventArgs e)
        {

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
        private void disablePages()
        {
            cmbConsignee.Enabled = false;
            cmbCourierID.Enabled = false;
            cmbTransportationMode.Enabled = false;
            txtLRNo.ReadOnly = true;
            dtLRDate.Enabled = false;
            txtDocNo.Enabled = false;
            dtDocDate.Enabled = false;
            btnInvoiceSelect.Enabled = false;
            dtDocDate.Enabled = false;
            cmbDeliveryStatus.Enabled = false;
            dtDeliveryDate.Enabled = false;
            txtRemarks.Enabled = false;
            btnAddRemarks.Enabled = false;
            txtCustmrApprvlNo.ReadOnly = true;
        }
        private void enablePages()
        {
            if (listOption != 2)
            {
                cmbConsignee.Enabled = true;
                cmbCourierID.Enabled = true;
                cmbTransportationMode.Enabled = true;
                txtLRNo.ReadOnly = false;
                dtLRDate.Enabled = true;
                txtCustmrApprvlNo.ReadOnly = false;
            }
            else if (listOption == 2)
            {
                cmbDeliveryStatus.Enabled = true;
                dtDeliveryDate.Enabled = true;
                //txtRemarks.Enabled = true;
                btnAddRemarks.Enabled = true;
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
                //    if (p.GetType() == typeof(TreeView) || p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
                //    {
                //        p.Dispose();
                //}
                pnlForwarder.Controls.Clear();
                Control nc = pnlForwarder.Parent;
                nc.Controls.Remove(pnlForwarder);
            }
            catch (Exception ex)
            {
            }
        }
        //private void removeControlsFromPayTermPanel()
        //{
        //    try
        //    {
        //        foreach (Control p in pnldgv.Controls)
        //            if (p.GetType() == typeof(Button))
        //            {
        //                p.Dispose();
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}



        private void setButtonVisibility(string btnName)
        {
            try
            {
                btnActionPending.Visible = true;
                btnFinalized.Visible = true;
                //btnApproved.Visible = true;
                btnNew.Visible = false;
                btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                btnFinalize.Visible = false;
                cmbDeliveryStatus.Visible = false;
                lblDeliveryDate.Visible = false;
                lblDeliverystatus.Visible = false;
                dtDeliveryDate.Visible = false;
                lblRemarks.Visible = false;
                txtRemarks.Visible = false;
                btnAddRemarks.Visible = false;
                txtCustmrApprvlNo.Visible = false;
                Cst.Visible = false;

                disablePages();
                clearTabPageControls();
                //----24/11/2016
                handleNewButton();
                handleGrdViewButton();
                handleGrdEditButton();
                pnlBottomButtons.Visible = true;
                //----
                if (btnName == "init")
                {
                    //btnNew.Visible = true; 
                    btnExit.Visible = true;
                    lblConsignee.Visible = false;
                    cmbConsignee.Visible = false;
                    lblCourierID.Visible = false;
                    cmbCourierID.Visible = false;
                    lblTransportationMode.Visible = false;
                    cmbTransportationMode.Visible = false;
                    lblLRNo.Visible = false;
                    txtLRNo.Visible = false;
                    lblLRDate.Visible = false;
                    dtLRDate.Visible = false;
                    lblDocNo.Visible = false;
                    lblDocDate.Visible = true;
                    txtDocNo.Visible = false;
                    dtDocDate.Visible = false;
                    btnInvoiceSelect.Visible = false;
                    dtDocDate.Visible = false;
                    lblDocDate.Visible = false;

                }

                else if (btnName == "btnNew")
                {
                    btnNew.Visible = false; //added 24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    btnInvoiceSelect.Enabled = true;
                    enablePages();
                    //tabControl1.SelectedTab = tabDocType;
                }
                else if (btnName == "btnEditPanel") //called from cancel,save,forward,approve and reverse button events
                {
                    ////btnNew.Visible = true;
                    btnExit.Visible = true;
                    lblConsignee.Visible = false;
                    cmbConsignee.Visible = false;
                    lblCourierID.Visible = false;
                    cmbCourierID.Visible = false;
                    lblTransportationMode.Visible = false;
                    cmbTransportationMode.Visible = false;
                    lblLRNo.Visible = false;
                    txtLRNo.Visible = false;
                    lblLRDate.Visible = false;
                    dtLRDate.Visible = false;
                    lblDocNo.Visible = false;
                    lblDocDate.Visible = true;
                    txtDocNo.Visible = false;
                    dtDocDate.Visible = false;
                    btnInvoiceSelect.Visible = false;
                    dtDocDate.Visible = false;
                    lblDocDate.Visible = false;
                }
                //gridview buttons
                else if (btnName == "gEdit")
                {
                    if (listOption == 2)
                    {
                        cmbDeliveryStatus.Visible = true;
                        //lblDeliveryDate.Visible = false;
                        lblDeliverystatus.Visible = true;
                        //dtDeliveryDate.Visible = false;
                        lblRemarks.Visible = true;
                        txtRemarks.Visible = true;
                        btnAddRemarks.Visible = true;
                    }
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;

                    pnlAddEdit.Visible = true;
                    pnlEditButtons.Visible = true;
                    pnlList.Visible = false;
                    enablePages();
                }
                else if (btnName == "gFinalize")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnFinalize.Visible = true;
                    btnFinalize.Visible = true;
                    pnlAddEdit.Visible = true;
                    pnlList.Visible = false;
                    pnlEditButtons.Visible = true;

                    btnSave.Visible = false;
                    disablePages();
                }
                else if (btnName == "gView")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnFinalize.Visible = false;
                    btnFinalize.Visible = false;
                    pnlAddEdit.Visible = true;
                    pnlList.Visible = false;
                    pnlEditButtons.Visible = true;
                    if (listOption == 2)
                    {
                        lblDeliveryDate.Visible = false;
                        dtDeliveryDate.Visible = false;
                    }
                    else
                    {
                        lblDeliveryDate.Visible = true;
                        dtDeliveryDate.Visible = true;
                    }
                    cmbDeliveryStatus.Visible = true;
                    lblDeliverystatus.Visible = true;
                    lblRemarks.Visible = true;
                    txtRemarks.Visible = true;
                    btnAddRemarks.Visible = true;
                    disablePages();
                }
                else if (btnName == "LoadDocument")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                }
                else if (btnName == "Print")
                {
                    pnlBottomButtons.Visible = true; //24/11/2016
                    btnExit.Visible = true;
                }

                pnlEditButtons.Refresh();
                //if the user privilege is only view, show only the Approved documents button
                int ups = getuserPrivilegeStatus();
                if (ups == 1 || ups == 0)
                {
                    grdList.Columns["gEdit"].Visible = false;
                    grdList.Columns["gFinalize"].Visible = false;
                    grdList.Columns["gView"].Visible = false;
                    btnActionPending.Visible = false;
                    btnFinalized.Visible = false;
                    btnNew.Visible = false;
                    if (ups == 1)
                    {
                        grdList.Columns["gView"].Visible = true;

                    }
                    else
                    {
                        grdList.Columns["gView"].Visible = false;
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
                if (listOption == 1)
                {
                    btnNew.Visible = true;
                }
            }
        }
        void handleGrdEditButton()
        {
            grdList.Columns["gEdit"].Visible = false;
            grdList.Columns["gFinalize"].Visible = false;
            grdList.Columns["Delete"].Visible = false;
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                if (listOption == 2)
                {
                    grdList.Columns["gEdit"].Visible = true;
                }
                else if (listOption == 1)
                {
                    grdList.Columns["gEdit"].Visible = true;
                    grdList.Columns["gFinalize"].Visible = true;
                    grdList.Columns["Delete"].Visible = true;
                }
            }
        }

        void handleGrdViewButton()
        {
            grdList.Columns["gView"].Visible = false;
            if (Main.itemPriv[0] || Main.itemPriv[1] || Main.itemPriv[2])
            {
                //list option 1 should not have view buttuon visible (edit is avialable)
                if (listOption != 1)
                {
                    grdList.Columns["gView"].Visible = true;
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

                removeControlsFromCommenterPanel();
                removeControlsFromForwarderPanel();
                dgvComments.Rows.Clear();

            }
            catch (Exception ex)
            {
            }
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void btnInvoiceSelection_Click(object sender, EventArgs e)
        {
        }

        private void lvOK_Click4(object sender, EventArgs e)
        {
            try
            {
                if (!checkLVItemChecked("Item"))
                {
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        IODocid = "";
                        // btnInvoiceSelect.Enabled = true;
                        IODocid = itemRow.SubItems[1].Text;
                        txtDocNo.Text = itemRow.SubItems[3].Text;
                        dtDocDate.Value = DateTime.Parse(itemRow.SubItems[4].Text);
                        itemRow.Checked = false;
                        frmPopup.Close();
                        frmPopup.Dispose();
                        FillDetailDelivery();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void FillDetailDelivery()
        {
            MaterialDeliveryDetailDB MaterialDetDB = new MaterialDeliveryDetailDB();
            List<materialdelivery> materialdel = MaterialDetDB.getInvoiceDetails(txtDocNo.Text, dtDocDate.Value);
            try
            {
                foreach (materialdelivery mdd in materialdel)
                {
                    ////////cmbConsignee.SelectedIndex = cmbConsignee.FindString(mdd.consignee);
                    cmbConsignee.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbConsignee, mdd.consignee);
                    ////////cmbCourierID.SelectedIndex = cmbCourierID.FindString(mdd.courierID);
                    cmbCourierID.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCourierID, mdd.courierID);
                    cmbTransportationMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbTransportationMode, mdd.transportationMode);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void lvCancel_Click4(object sender, EventArgs e)
        {
            try
            {
                // btnInvoiceSelect.Enabled = true;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }
        private void removeControlsFromLVPanel()
        {
            try
            {
                //foreach (Control p in pnlv.Controls)
                //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
                //    {
                //        p.Dispose();
                //    }
                pnlv.Controls.Clear();
                Control nc = pnlv.Parent;
                nc.Controls.Remove(pnlv);
            }
            catch (Exception ex)
            {
            }
        }

        //private void listView1_ItemChecked2(object sender, ItemCheckedEventArgs e)
        //{
        //    try
        //    {
        //        if (lv.CheckedIndices.Count > 1)
        //        {
        //            MessageBox.Show("Cannot select more than one item");
        //            e.Item.Checked = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        private void LvColumnClick(object o, ColumnClickEventArgs e)
        {
            try
            {
                if (lv.Visible == true)
                {
                    string first = lv.Items[0].SubItems[e.Column].Text;
                    string last = lv.Items[lv.Items.Count - 1].SubItems[e.Column].Text;
                    System.Windows.Forms.SortOrder sort_order1 = SortingListView.getSortedOrder(first, last);
                    this.lv.ListViewItemSorter = new SortingListView(e.Column, sort_order1);
                }
                else
                {
                    string first = lvCopy.Items[0].SubItems[e.Column].Text;
                    string last = lvCopy.Items[lvCopy.Items.Count - 1].SubItems[e.Column].Text;
                    System.Windows.Forms.SortOrder sort_order1 = SortingListView.getSortedOrder(first, last);
                    this.lvCopy.ListViewItemSorter = new SortingListView(e.Column, sort_order1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorting error");
            }
        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("gEdit") || columnName.Equals("gFinalize") || columnName.Equals("gView") || columnName.Equals("Delete") || columnName.Equals("Print"))
                {
                    clearData();
                    btnSave.Text = "Update";
                    cmbDocType.SelectedIndex = cmbDocType.FindString(grdList.Rows[e.RowIndex].Cells["gDocumentType"].Value.ToString());
                    setDocType(grdList.Rows[e.RowIndex].Cells["gDocumentType"].Value.ToString());
                    setButtonVisibility(columnName);
                    MatDel = new materialdelivery();
                    MatDel.RowID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["RowID"].Value.ToString());
                    MatDel.DocumentNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentNo"].Value.ToString());
                    MatDel.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    MatDel.DocumentDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gDocumentDate"].Value.ToString());
                    MatDel.DeliveryStatus = setStatusString(grdList.Rows[e.RowIndex].Cells["gDeliveryStatus"].Value.ToString());
                    MatDel.CustomerApprovalNo = grdList.Rows[e.RowIndex].Cells["CustomerApprovalNo"].Value.ToString();
                    status = setStatusString(grdList.Rows[e.RowIndex].Cells["gDeliveryStatus"].Value.ToString());
                    //chkDocID = MatDel.DocumentID;
                    txtDocNo.Text = grdList.Rows[e.RowIndex].Cells["gDocumentNo"].Value.ToString();
                    dtDocDate.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gDocumentDate"].Value.ToString());
                    ////////cmbConsignee.SelectedIndex = cmbConsignee.FindString(grdList.Rows[e.RowIndex].Cells["gConsignee"].Value.ToString());
                    cmbConsignee.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbConsignee, grdList.Rows[e.RowIndex].Cells["gConsignee"].Value.ToString());
                    cmbCourierID.SelectedIndex = cmbCourierID.FindString(grdList.Rows[e.RowIndex].Cells["gCourierID"].Value.ToString());
                    cmbCourierID.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCourierID, grdList.Rows[e.RowIndex].Cells["gCourierID"].Value.ToString());
                    txtCustmrApprvlNo.Text = grdList.Rows[e.RowIndex].Cells["CustomerApprovalNo"].Value.ToString();
                    txtLRNo.Text = grdList.Rows[e.RowIndex].Cells["gLRNo"].Value.ToString();
                    dtLRDate.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gLRDate"].Value.ToString());
                    if (listOption == 3)
                    {
                        dtDeliveryDate.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gDeliveryDate"].Value.ToString());
                    }
                    string ds = grdList.Rows[e.RowIndex].Cells["gDeliveryStatus"].Value.ToString();
                    cmbDeliveryStatus.SelectedIndex = cmbDeliveryStatus.FindString(grdList.Rows[e.RowIndex].Cells["gDeliveryStatus"].Value.ToString());
                    //cmbTransportationMode.SelectedIndex = cmbTransportationMode.FindString();
                    string transMode = grdList.Rows[e.RowIndex].Cells["gTransportaionMode"].Value.ToString();
                    cmbTransportationMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbTransportationMode, transMode);
                    txtRemarks.Text = grdList.Rows[e.RowIndex].Cells["gRemarks"].Value.ToString();
                    setDocType(docID);
                    if (columnName.Equals("Delete"))
                    {
                        MaterialDeliveryDetailDB mdb = new MaterialDeliveryDetailDB();
                        if (mdb.DeleteDelivery(MatDel))
                        {
                            grdList.Rows.RemoveAt(e.RowIndex);
                            MessageBox.Show("Delivery detail row deleted..");
                            listOption = 1;
                            ListFilteredMaterialDeliveryDetail(listOption);
                        }
                        else
                        {
                            MessageBox.Show("ERROR!! Deleting Delivery detail row");
                        }
                    }
                    if (columnName.Equals("Print"))
                    {
                        if (grdList.Rows[e.RowIndex].Cells["gDocumentType"].Value.ToString() == "Others")
                        {
                            MessageBox.Show("Print not available");
                            return;
                        }

                        PrintInvoiceOut pio = new PrintInvoiceOut();
                        invoiceoutheader ioh = InvoiceOutHeaderDB.getInvOutHeaderFromInvNoDate(MatDel.DocumentID, MatDel.DocumentNo, MatDel.DocumentDate);
                        List<invoiceoutdetail> IODetail = InvoiceOutHeaderDB.getInvoiceOutDetail(ioh);
                        foreach (invoiceoutdetail iod in IODetail)
                        {
                            if (iod.HSNCode.Trim().Length <= 0)
                            {
                                MessageBox.Show("HSN Code not available for " + iod.StockItemName + ". Print aborted....");
                                return;
                            }
                        }
                        getTaxDetails(IODetail);
                        string taxstr = getTasDetailStr();
                        pio.PrintIO(ioh, IODetail, taxstr, getPODocIDForSelectedIO(MatDel.DocumentID));
                        //MaterialDeliveryDetailDB mdb = new MaterialDeliveryDetailDB();
                        //if (mdb.DeleteDelivery(MatDel))
                        //{
                        //    grdList.Rows.RemoveAt(e.RowIndex);
                        //    MessageBox.Show("Delivery detail row deleted..");
                        //    listOption = 1;
                        //    ListFilteredMaterialDeliveryDetail(listOption);
                        //}
                        //else
                        //{
                        //    MessageBox.Show("ERROR!! Deleting Delivery detail row");
                        //}
                    }
                    //tabControl1.SelectedTab = tabDocHeader;
                    //tabControl1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private string getPODocIDForSelectedIO(string iodocid)
        {
            string podocid = "";
            switch (iodocid)
            {
                case "PRODUCTEXPORTINVOICEOUT":
                    podocid = "POPRODUCTINWARD";
                    break;
                case "SERVICEEXPORTINVOICEOUT":
                    podocid = "POSERVICEINWARD";
                    break;
                case "PRODUCTINVOICEOUT":
                    podocid = "POPRODUCTINWARD";
                    break;
                case "SERVICEINVOICEOUT":
                    podocid = "POSERVICEINWARD";
                    break;
                case "SERVICERCINVOICEOUT":
                    podocid = "POSERVICERCINWARD";
                    break;
                default:
                    podocid = "";
                    break;
            }
            return podocid;
        }
        private void getTaxDetails(List<invoiceoutdetail> iodList)
        {
            try
            {
                double quantity = 0;
                double price = 0;
                double cost = 0.0;
                productvalue = 0.0;
                taxvalue = 0.0;
                string strtaxCode = "";
                //clear tax details table
                TaxDetailsTable.Rows.Clear();
                foreach (invoiceoutdetail iod in iodList)
                {
                    quantity = iod.Quantity;
                    price = iod.Price;
                    if (price != 0 && quantity != 0)
                    {
                        cost = Math.Round(quantity * price, 2);
                    }

                    try
                    {
                        strtaxCode = iod.TaxCode;
                    }
                    catch (Exception)
                    {
                        strtaxCode = "";
                    }
                    System.Data.DataTable TaxData = TaxCodeWorkingDB.calculateTax(strtaxCode, cost);
                    double ttax1 = 0.0;
                    double ttax2 = 0.0;
                    string strTax = "";
                    for (int j = 0; j < TaxData.Rows.Count; j++)
                    {
                        string tstr = "";
                        try
                        {
                            tstr = TaxData.Rows[j][7].ToString().Trim().Substring(0, TaxData.Rows[j][7].ToString().Trim().IndexOf('-'));
                            if (!(tstr.Length == 0 && tstr.Equals("Dummy")))
                            {
                                ttax1 = Convert.ToDouble(TaxData.Rows[j][6]);
                                string a = Convert.ToString(TaxData.Rows[j][1]);
                                string b = Convert.ToString(TaxData.Rows[j][6]);
                                string c = Convert.ToString(TaxData.Rows[j][7]);
                                strTax = strTax + tstr + "-" +
                                    Convert.ToString(TaxData.Rows[j][6]) + "\n";
                                int taxcodefound = 0;
                                for (int k = 0; k < (TaxDetailsTable.Rows.Count); k++)
                                {
                                    if (TaxDetailsTable.Rows[k][0].ToString().Trim().Equals(tstr))
                                    {
                                        TaxDetailsTable.Rows[k][1] = Convert.ToDouble(TaxDetailsTable.Rows[k][1]) +
                                            Convert.ToDouble(TaxData.Rows[j][6]);
                                        taxcodefound = 1;
                                    }
                                }
                                if (taxcodefound == 0)
                                {
                                    TaxDetailsTable.Rows.Add();
                                    TaxDetailsTable.Rows[TaxDetailsTable.Rows.Count - 1][0] = tstr;
                                    TaxDetailsTable.Rows[TaxDetailsTable.Rows.Count - 1][1] =
                                       Convert.ToDouble(TaxData.Rows[j][6]);
                                }
                            }
                            else
                            {
                                ttax1 = 0.0;
                            }
                        }
                        catch (Exception ex)
                        {
                            ttax1 = 0.0;
                        }
                        ttax2 = ttax2 + ttax1;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private string getTasDetailStr()
        {
            string strTax = "";
            try
            {
                for (int i = 0; i < (TaxDetailsTable.Rows.Count); i++)
                {
                    strTax = strTax + Convert.ToString(TaxDetailsTable.Rows[i][0]) + Main.delimiter1 +
                    Convert.ToString(TaxDetailsTable.Rows[i][1]) + Main.delimiter2;
                }
                //DialogResult dialog = MessageBox.Show(strTax, "Tax Details", MessageBoxButtons.OK);
            }
            catch (Exception)
            {
                MessageBox.Show("Error showing tax details");
            }
            return strTax;
        }
        private void pnlAddEdit_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pnlUI_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlTobButtons_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblActionHeader_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlEditButtons_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlBottomButtons_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label21_Click_1(object sender, EventArgs e)
        {

        }

        private void cmbDocType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string Doctype = cmbDocType.SelectedItem.ToString().Trim();
                setDocType(Doctype);
                cmbDocType.Enabled = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            /// btnInvoiceSelect.Enabled = false;
            //removeControlsFromLVPanel();
            //pnlAddEdit.Controls.Remove(pnlv);
            //pnlv = new Panel();
            //pnlv.BorderStyle = BorderStyle.FixedSingle;

            //pnlv.Bounds = new Rectangle(new Point(311, 46), new Size(566, 281));
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(900, 300);
            lv = MaterialDeliveryDetailDB.getInvoiceOutListView();
            lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked2);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(900, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Click4);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click4);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
            //pnlAddEdit.Controls.Add(pnlv);
            //pnlv.BringToFront();
            //pnlv.Visible = true;
        }

        private Boolean checkLVItemChecked(string str)
        {
            Boolean status = true;
            try
            {
                if (lv.CheckedIndices.Count > 1)
                {
                    MessageBox.Show("Only one " + str + " allowed");
                    return false;
                }
                if (lv.CheckedItems.Count == 0)
                {
                    MessageBox.Show("select one " + str);
                    return false;
                }
            }
            catch (Exception)
            {
            }
            return status;
        }

        private void MaterialDeliveryDetail_Enter(object sender, EventArgs e)
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

        private void rdbInTransit_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rdbDelivered_CheckedChanged(object sender, EventArgs e)
        {
            //    listOption = 3;
            //    ListFilteredMaterialDeliveryDetail(listOption);
        }

        private void rdbInTransit_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredMaterialDeliveryDetail(listOption);
        }

        private void rdbDelivered_Click(object sender, EventArgs e)
        {
            listOption = 3;
            ListFilteredMaterialDeliveryDetail(listOption);
        }

        private void cmbDeliveryStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDeliveryStatus.SelectedItem.ToString() == "Delivered" && rdbInTransit.Checked == true)
            {
                dtDeliveryDate.Enabled = true;
                dtDeliveryDate.Visible = true;
                lblDeliveryDate.Visible = true;
            }
            else if (rdbDelivered.Checked == true)
            {
                dtDeliveryDate.Enabled = false;
                dtDeliveryDate.Visible = true;
                lblDeliveryDate.Visible = true;
            }
            else
            {
                dtDeliveryDate.Enabled = false;
                dtDeliveryDate.Visible = false;
                lblDeliveryDate.Visible = false;
            }
        }

        private void removeControlsFrompnllvPanel()
        {
            try
            {
                //foreach (Control p in pnllv.Controls)
                //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
                //    {
                //        p.Dispose();
                //    }
                pnllv.Controls.Clear();
                Control nc = pnllv.Parent;
                nc.Controls.Remove(pnllv);
            }
            catch (Exception ex)
            {
            }
        }


        private void btnAddRemarks_Click(object sender, EventArgs e)
        {
            removeControlsFrompnllvPanel();

            txt = new RichTextBox();
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;

            pnllv.Bounds = new Rectangle(new Point(200, 30), new Size(400, 300));
            txt.Bounds = new Rectangle(new Point(25, 25), new Size(350, 200));
            pnllv.Controls.Add(txt);

            Label lblHeader = new Label();
            lblHeader.Size = new Size(300, 20);
            lblHeader.Text = "Type your Comments             ";
            lblHeader.Location = new Point(50, 10);
            pnllv.Controls.Add(lblHeader);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Location = new Point(200, 240);
            lvOK.Click += new System.EventHandler(this.lvOK_Click5);
            pnllv.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Location = new Point(100, 240);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click5);
            pnllv.Controls.Add(lvCancel);

            pnlDetails.Controls.Add(pnllv);
            pnllv.BringToFront();
            pnllv.Visible = true;
        }

        private void lvOK_Click5(object sender, EventArgs e)
        {
            try
            {
                pnllv.Visible = true;
                if (txt.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please Add Remarks");
                    return;
                }
                txtRemarks.Text = txtRemarks.Text + Environment.NewLine + Login.userLoggedInName + " : " + DateTime.Now.ToString() + " : " + txt.Text;
                //btnAddRemarks.Enabled = true;
                pnllv.Visible = false;
                txtRemarks.ReadOnly = true;
                ContextMenu blankContextMenu = new ContextMenu();
                txtRemarks.ContextMenu = blankContextMenu;
            }
            catch (Exception ex)
            {

            }
        }

        private void lvCancel_Click5(object sender, EventArgs e)
        {
            try
            {
                //btnAddRemarks.Enabled = true;
                pnllv.Visible = false;
            }
            catch (Exception)
            {
            }
        }
    }
}

