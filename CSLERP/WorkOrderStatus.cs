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
using CSLERP.PrintForms;

namespace CSLERP
{
    public partial class WorkOrderStatus : System.Windows.Forms.Form
    {
        string docID = "WORKORDERSTATUS";
        double productvalue = 0.0;
        double taxvalue = 0.0;
        workorderheader prevwoh;
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        Panel pnllv = new Panel();
        ListView lv = new ListView();

        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Boolean userIsACommenter = false;
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        int check = 0;
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        public WorkOrderStatus()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void WorkOrderStatus_Load(object sender, EventArgs e)
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
            ListFilteredWOHeader();
            //applyPrivilege();
        }
        private string getWOStatusString(int code)
        {
            string statStr = "";
            switch (code)
            {
                case 1:
                    statStr = "Order Approved";
                    break;
                case 2:
                    statStr = "Work Started";
                    break;
                case 3:
                    statStr = "Work In Progress";
                    break;
                case 4:
                    statStr = "Temporary Halt";
                    break;
                case 5:
                    statStr = "Order Cancelled";
                    break;
                case 6:
                    statStr = "Completed";
                    break;
                default:
                    statStr = "";
                    break;
            }
            return statStr;
        }
        //private int getWOStatusCode(string statStr)
        //{
        //    int statCode = 0;


        //    switch (statStr)
        //    {
        //        case "Order Approved":
        //            statCode = 1;
        //            break;
        //        case "Work Started":
        //            statCode = 2;
        //            break;
        //        case "Work In Progress":
        //            statCode = 3;
        //            break;
        //        case "Temporary Halt":
        //            statCode = 4;
        //            break;
        //        case "Order Cancelled":
        //            statCode = 5;
        //            break;
        //        case "Completed":
        //            statCode = 6;
        //            break;
        //        default:
        //            statStr = "";
        //            break;
        //    }
        //    return statCode;
        //}
        private void ListFilteredWOHeader()
        {
            try
            {
                grdList.Rows.Clear();
                WorkOrderStatusDB wodb = new WorkOrderStatusDB();
                List<workorderheader> WOHeaders = wodb.getFilteredWorkOrderStatus();
                foreach (workorderheader woh in WOHeaders)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = woh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = woh.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = woh.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = woh.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gWONo"].Value = woh.WONo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gWODate"].Value = woh.WODate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gWORequestNo"].Value = woh.WORequestNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gWORequestDate"].Value = woh.WORequestDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gReferenceInternalOrder"].Value = woh.ReferenceInternalOrder;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProjectID"].Value = woh.ProjectID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gOfficeID"].Value = woh.OfficeID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerID"].Value = woh.CustomerID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerName"].Value = woh.CustomerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyID"].Value = woh.CurrencyID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCurrencyName"].Value = woh.CurrencyName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStartDate"].Value = woh.StartDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTargetDate"].Value = woh.TargetDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPaymentTerms"].Value = woh.PaymentTerms;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPaymentMode"].Value = woh.PaymentMode;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPOAddress"].Value = woh.POAddress;
                    grdList.Rows[grdList.RowCount - 1].Cells["gServiceValue"].Value = woh.ServiceValue;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTaxAmount"].Value = woh.TaxAmount;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTotalAmount"].Value = woh.TotalAmount;
                    grdList.Rows[grdList.RowCount - 1].Cells["TermsAndCondition"].Value = woh.TermsAndCond;
                    grdList.Rows[grdList.RowCount - 1].Cells["gRemarks"].Value = woh.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = woh.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = woh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = woh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreator"].Value = woh.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gForwarder"].Value = woh.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gApprover"].Value = woh.ApproverName;
                    //grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatusNo"].Value = ComboFIll.getDocumentStatusString(woh.DocumentStatus);
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = woh.CreateUser;
                    //grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatusNo"].Value = woh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["ComntStatus"].Value = woh.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = woh.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["ForwarderLists"].Value = woh.ForwarderList;
                    grdList.Rows[grdList.RowCount - 1].Cells["gWorkOrderStatus"].Value = woh.WorkOrderStatus; 
                    grdList.Rows[grdList.RowCount - 1].Cells["WOStatusName"].Value = getWOStatusString(woh.WorkOrderStatus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in PO Product Inward Listing");
            }

            setButtonVisibility("init");
            pnlList.Visible = true;

            ////////grdList.Columns["gCreator"].Visible = true;
            ////////grdList.Columns["gForwarder"].Visible = true;
            ////////grdList.Columns["gApprover"].Visible = true;
        }
        private void initVariables()
        {

            if (getuserPrivilegeStatus() == 1)
            {
                //user is only a viewer
                listOption = 6;
            }
            docID = Main.currentDocument;

            CustomerDB.fillCustomerComboNew(cmbCustomer);
            //TaxCodeDB.fillTaxCodeCombo(cmbTaxCode);

            dtTemporaryDate.Format = DateTimePickerFormat.Custom;
            dtTemporaryDate.CustomFormat = "dd-MM-yyyy";
            dtTemporaryDate.Enabled = false;
            dtWORequestDate.Format = DateTimePickerFormat.Custom;
            dtWORequestDate.CustomFormat = "dd-MM-yyyy";
            dtWORequestDate.Enabled = false;
            dtStartDate.Format = DateTimePickerFormat.Custom;
            dtStartDate.CustomFormat = "dd-MM-yyyy";
            dtStartDate.Enabled = true;
            dtTargetDate.Format = DateTimePickerFormat.Custom;
            dtTargetDate.CustomFormat = "dd-MM-yyyy";
            dtTargetDate.Enabled = true;
            StatusCatalogueDB.fillStatusCatalogueCombo(cmbWOStatus,"WORKORDER");
            //txtPaymentTerms.Enabled = false;
            txtTemporaryNo.Enabled = false;
            txtWORequestNo.Enabled = false;
            //txtCreditPeriods.Enabled = true;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            cmbCustomer.TabIndex = 0;
            grdWODetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //create tax details table for tax breakup display
            {
                TaxDetailsTable.Columns.Add("TaxItem", typeof(string));
                TaxDetailsTable.Columns.Add("TaxAmount", typeof(double));
            }
            txtComments.Text = "";

            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
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
                //clear all grid views
                grdWODetail.Rows.Clear();
                dgvComments.Rows.Clear();
                //dgvpt.Rows.Clear();
                //----------clear temperory panels

                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                //----------
                cmbCustomer.SelectedIndex = -1;
                dtTemporaryDate.Value = DateTime.Parse("1900-01-01");
                dtWORequestDate.Value = DateTime.Parse("1900-01-01");
                dtStartDate.Value = DateTime.Today.Date;
                dtTargetDate.Value = DateTime.Today.Date;

                grdWODetail.Rows.Clear();
                txtTemporaryNo.Text = "";
                txtWORequestNo.Text = "";
                btnProductValue.Text = "0";
                btnTaxAmount.Text = "0";
                btnTotalAmount.Text = "0";
                prevwoh = new workorderheader();
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
                tabControl1.SelectedTab = tabWOHeader;
                tabWOHeader.Enabled = true;
                setButtonVisibility("btnNew");
            }
            catch (Exception)
            {

            }
        }


        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                AddWODetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
        }

        private Boolean AddWODetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdWODetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkWODetailGridRows())
                    {
                        return false;
                    }
                }
                grdWODetail.Rows.Add();
                int kount = grdWODetail.RowCount;
                grdWODetail.Rows[kount - 1].Cells[0].Value = kount;
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdWODetail.Rows[kount - 1].Cells["gTCode"]);
                TaxCodeDB.fillTaxCodeGridViewCombo(ComboColumn2, "");
                grdWODetail.Rows[kount - 1].Cells["gWorkDescription"].Value = " ";
                grdWODetail.Rows[kount - 1].Cells["gWorkLocation"].Value = "";
                grdWODetail.Rows[kount - 1].Cells["gQuantity"].Value = Convert.ToDouble(0);
                grdWODetail.Rows[kount - 1].Cells["gPrice"].Value = Convert.ToDouble(0);
                grdWODetail.Rows[kount - 1].Cells["gValue"].Value = Convert.ToDouble(0);
                grdWODetail.Rows[kount - 1].Cells["gTax"].Value = Convert.ToDouble(0);
                grdWODetail.Rows[kount - 1].Cells["gWarrantyDays"].Value = Convert.ToDouble(0);
                grdWODetail.Rows[kount - 1].Cells["gTaxDetails"].Value = " ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddWODetailRow() : Error");
            }

            return status;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            closeAllPanels();
            // btnNew.Visible = true;
            btnExit.Visible = true;
            pnlList.Visible = true;
            //enableBottomButtons();
            //pnlBottomActions.Visible = true;
        }

        private Boolean verifyAndReworkWODetailGridRows()
        {
            Boolean status = true;

            try
            {
                double quantity = 0;
                double price = 0;
                double cost = 0.0;
                productvalue = 0.0;
                taxvalue = 0.0;
                string strtaxCode = "";

                if (grdWODetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Work Order Grid details");
                    btnProductValue.Text = productvalue.ToString();
                    btnTaxAmount.Text = taxvalue.ToString(); //fill this later
                    btnTotalAmount.Text = (productvalue + taxvalue).ToString();
                    //btnProductValue.Text = txtServiceValue.Text;
                    return false;
                }
                //clear tax details table
                TaxDetailsTable.Rows.Clear();
                for (int i = 0; i < grdWODetail.Rows.Count; i++)
                {

                    grdWODetail.Rows[i].Cells["LNo"].Value = (i + 1);
                    if (((grdWODetail.Rows[i].Cells["gWorkDescription"].Value == null) ||
                        (grdWODetail.Rows[i].Cells["gWorkLocation"].Value == null)) ||
                         (grdWODetail.Rows[i].Cells["gWorkDescription"].Value.ToString().Trim().Length == 0) ||
                        (grdWODetail.Rows[i].Cells["gWorkLocation"].Value.ToString().Trim().Length == 0) ||
                        (grdWODetail.Rows[i].Cells["gQuantity"].Value == null) ||
                        (grdWODetail.Rows[i].Cells["gPrice"].Value == null) ||
                         (grdWODetail.Rows[i].Cells["gValue"].Value == null) ||
                        (Convert.ToDouble(grdWODetail.Rows[i].Cells["gQuantity"].Value) == 0) ||
                        (Convert.ToDouble(grdWODetail.Rows[i].Cells["gPrice"].Value) == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }

                    quantity = Convert.ToDouble(grdWODetail.Rows[i].Cells["gQuantity"].Value);
                    price = Convert.ToDouble(grdWODetail.Rows[i].Cells["gPrice"].Value);
                    if (price != 0 && quantity != 0)
                    {
                        cost = Math.Round(quantity * price, 2);
                    }

                    try
                    {
                        strtaxCode = grdWODetail.Rows[i].Cells["gTCode"].Value.ToString();
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
                        catch (Exception)
                        {
                            ttax1 = 0.0;
                        }
                        ttax2 = ttax2 + ttax1;
                    }
                    grdWODetail.Rows[i].Cells["gValue"].Value = Convert.ToDouble(cost.ToString());
                    grdWODetail.Rows[i].Cells["gTax"].Value = Convert.ToDouble(ttax2.ToString());
                    grdWODetail.Rows[i].Cells["gTaxDetails"].Value = strTax;
                    productvalue = productvalue + cost;
                    taxvalue = taxvalue + ttax2;

                    //- rewok tax value
                }
                btnProductValue.Text = productvalue.ToString();
                btnTaxAmount.Text = taxvalue.ToString(); //fill this later
                btnTotalAmount.Text = (productvalue + taxvalue).ToString();
            }
            catch (Exception ex)
            {
                return false;
            }
            return status;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            Boolean status = true;
            try
            {

                WorkOrderStatusDB wodb = new WorkOrderStatusDB();
                workorderheader woh = new workorderheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    if (!verifyAndReworkWODetailGridRows())
                    {
                        return;
                    }
                    woh.DocumentID = docID;
                    ////////woh.CustomerID = cmbCustomer.SelectedItem.ToString().Trim().Substring(0, cmbCustomer.SelectedItem.ToString().Trim().IndexOf('-'));
                    woh.CustomerID = ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue;
                    woh.WORequestDate = dtWORequestDate.Value;

                    woh.StartDate = dtStartDate.Value;
                    woh.TargetDate = dtTargetDate.Value;

                    woh.ServiceValue = Convert.ToDouble(btnProductValue.Text);
                    //woh.TaxCode = cmbTaxCode.SelectedItem.ToString();
                    woh.TaxAmount = Convert.ToDouble(btnTaxAmount.Text);
                    woh.TotalAmount = Convert.ToDouble(btnTotalAmount.Text);
                   
                    woh.WorkOrderStatus = Convert.ToInt32(((Structures.ComboBoxItem)cmbWOStatus.SelectedItem).HiddenValue);
                    woh.Comments = docCmtrDB.DGVtoString(dgvComments);
                    woh.ForwarderList = prevwoh.ForwarderList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    woh.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    woh.DocumentStatus = 1; //created
                    woh.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    woh.TemporaryNo = Convert.ToInt32(txtTemporaryNo.Text);
                    woh.TemporaryDate = prevwoh.TemporaryDate;
                    woh.DocumentStatus = prevwoh.DocumentStatus;
                }

                if (wodb.validateWOStatusHeader(woh))
                {
                    //--create comment status string
                    docCmtrDB = new DocCommenterDB();
                    if (userIsACommenter)
                    {
                        //if the user is only a commenter and ticked the comment as final; then update his comment string as final
                        //and update the comment status
                        if (txtComments.Text.Length != 0)
                        {
                            docCmtrDB = new DocCommenterDB();
                            woh.CommentStatus = docCmtrDB.createCommentStatusString(prevwoh.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            woh.CommentStatus = prevwoh.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            woh.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            woh.CommentStatus = prevwoh.CommentStatus;
                        }
                    }
                    //----------
                    int tmpStatus = 0;
                    if (txtComments.Text.Trim().Length == 0 && prevwoh.WorkOrderStatus != woh.WorkOrderStatus)
                    {
                        MessageBox.Show("Give the Comment.Failed to save.");
                        return;
                    }
                    if (txtComments.Text.Trim().Length > 0)
                    {
                        tmpStatus = 1;
                        string cmnt = cmbWOStatus.SelectedItem.ToString() + " : " + txtComments.Text.Trim();
                        woh.Comments = docCmtrDB.processNewComment(dgvComments, cmnt, Login.userLoggedIn, Login.userLoggedInName, tmpStatus);
                        
                    }
                    if (btnText.Equals("Update"))
                    {
                        if (wodb.UpdateWORequestHeader(woh))
                        {
                            MessageBox.Show("Work Order Status updated");
                            closeAllPanels();
                            ListFilteredWOHeader();
                        }
                        else
                        {
                            status = false;

                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update Work Order Status");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Work Order Header Validation failed");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("createAndUpdateWODetails() : Error");
                return;
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
                if (columnName.Equals("Edit") || columnName.Equals("View") || columnName.Equals("Print"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    WorkOrderRequestDB wodb = new WorkOrderRequestDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevwoh = new workorderheader();
                    prevwoh.CommentStatus = grdList.Rows[e.RowIndex].Cells["ComntStatus"].Value.ToString();
                    prevwoh.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    prevwoh.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    prevwoh.WONo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gWONo"].Value.ToString());
                    prevwoh.WODate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gWODate"].Value.ToString());
                    prevwoh.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    prevwoh.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    if (prevwoh.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    prevwoh.Comments = WorkOrderDB.getUserComments(prevwoh.DocumentID, prevwoh.TemporaryNo, prevwoh.TemporaryDate);

                    prevwoh.WORequestNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gWORequestNo"].Value.ToString());
                    prevwoh.WORequestDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gWORequestDate"].Value.ToString());
                    prevwoh.ReferenceInternalOrder = grdList.Rows[e.RowIndex].Cells["gReferenceInternalOrder"].Value.ToString();
                    prevwoh.ProjectID = grdList.Rows[e.RowIndex].Cells["gProjectID"].Value.ToString();
                    prevwoh.OfficeID = grdList.Rows[e.RowIndex].Cells["gOfficeID"].Value.ToString();
                    prevwoh.CustomerID = grdList.Rows[e.RowIndex].Cells["gCustomerID"].Value.ToString();
                    prevwoh.CustomerName = grdList.Rows[e.RowIndex].Cells["gCustomerName"].Value.ToString();

                    prevwoh.CurrencyID = grdList.Rows[e.RowIndex].Cells["gCurrencyID"].Value.ToString();
                    prevwoh.CurrencyName = grdList.Rows[e.RowIndex].Cells["gCurrencyName"].Value.ToString();
                    prevwoh.StartDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gStartDate"].Value.ToString());
                    prevwoh.TargetDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTargetDate"].Value.ToString());
                    prevwoh.PaymentTerms = grdList.Rows[e.RowIndex].Cells["gPaymentTerms"].Value.ToString();
                    prevwoh.PaymentMode = grdList.Rows[e.RowIndex].Cells["gPaymentMode"].Value.ToString();
                    //prevwoh.TaxCode = grdList.Rows[e.RowIndex].Cells["gTaxCode"].Value.ToString();
                    prevwoh.POAddress = grdList.Rows[e.RowIndex].Cells["gPOAddress"].Value.ToString();
                    prevwoh.ServiceValue = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gServiceValue"].Value.ToString());
                    prevwoh.TotalAmount = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gTotalAmount"].Value.ToString());
                    prevwoh.TaxAmount = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["gTaxAmount"].Value.ToString());
                    prevwoh.TermsAndCond = grdList.Rows[e.RowIndex].Cells["TermsAndCondition"].Value.ToString();
                    prevwoh.Remarks = grdList.Rows[e.RowIndex].Cells["gRemarks"].Value.ToString();
                    prevwoh.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    prevwoh.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    prevwoh.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    prevwoh.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    prevwoh.CreatorName = grdList.Rows[e.RowIndex].Cells["gCreator"].Value.ToString();
                    prevwoh.ForwarderName = grdList.Rows[e.RowIndex].Cells["gForwarder"].Value.ToString();
                    prevwoh.ApproverName = grdList.Rows[e.RowIndex].Cells["gApprover"].Value.ToString();
                    prevwoh.ForwarderList = grdList.Rows[e.RowIndex].Cells["ForwarderLists"].Value.ToString();
                    prevwoh.WorkOrderStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gWorkOrderStatus"].Value.ToString());
                    //--comments
                    //chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    prevwoh.CommentStatus = grdList.Rows[e.RowIndex].Cells["ComntStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(prevwoh.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(prevwoh.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---
                    ////////cmbCustomer.SelectedIndex = cmbCustomer.FindString(prevwoh.CustomerID);
                    cmbCustomer.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbCustomer, prevwoh.CustomerID);
                    //cmbWOStatus.SelectedIndex = cmbWOStatus.FindString(prevwoh.WorkOrderStatus.ToString());
                    cmbWOStatus.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbWOStatus, prevwoh.WorkOrderStatus.ToString());
                    //cmbTaxCode.SelectedIndex = cmbTaxCode.FindString(prevwoh.TaxCode);

                    txtTemporaryNo.Text = prevwoh.TemporaryNo.ToString();
                    try
                    {
                        dtTemporaryDate.Value = prevwoh.TemporaryDate;
                    }
                    catch (Exception)
                    {

                        dtTemporaryDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtWORequestNo.Text = prevwoh.WORequestNo.ToString();
                    try
                    {
                        dtWORequestDate.Value = prevwoh.WORequestDate;
                    }
                    catch (Exception)
                    {
                        dtWORequestDate.Value = DateTime.Parse("01-01-1900");
                    }
                    dtStartDate.Value = prevwoh.StartDate;
                    dtTargetDate.Value = prevwoh.TargetDate;

                    btnProductValue.Text = prevwoh.ServiceValue.ToString();
                    btnTaxAmount.Text = prevwoh.TaxAmount.ToString();
                    btnTotalAmount.Text = prevwoh.TotalAmount.ToString();

                    List<workorderdetail> WODetail = WorkOrderStatusDB.getWorkOrderDetails(prevwoh);
                    grdWODetail.Rows.Clear();
                    int i = 0;
                    foreach (workorderdetail wod in WODetail)
                    {
                        if (!AddWODetailRow())
                        {
                            MessageBox.Show("Error found in WOrk Order Detail. Please correct before updating the details");
                        }
                        else
                        {
                            grdWODetail.Rows[i].Cells["gItem"].Value = wod.StockItemID + "-" + wod.Description;
                            grdWODetail.Rows[i].Cells["gTCode"].Value = wod.TaxCode;
                            grdWODetail.Rows[i].Cells["gWorkDescription"].Value = wod.WorkDescription;
                            grdWODetail.Rows[i].Cells["gWorkLocation"].Value = wod.WorkLocation;
                            grdWODetail.Rows[i].Cells["gQuantity"].Value = wod.Quantity;
                            grdWODetail.Rows[i].Cells["gPrice"].Value = wod.Price;
                            grdWODetail.Rows[i].Cells["gValue"].Value = wod.Quantity * wod.Price;
                            grdWODetail.Rows[i].Cells["gTax"].Value = wod.Tax;
                            grdWODetail.Rows[i].Cells["gWarrantyDays"].Value = wod.WarrantyDays;
                            grdWODetail.Rows[i].Cells["gTaxDetails"].Value = wod.TaxDetails;
                            i++;
                            productvalue = productvalue + wod.Quantity * wod.Price;
                            taxvalue = taxvalue + wod.Tax;
                        }

                    }
                    if (!verifyAndReworkWODetailGridRows())
                    {
                        MessageBox.Show("Error found in Work Order details. Please correct before updating the details");
                    }
                    if (columnName.Equals("Print"))
                    {
                        //PrintPurchaseOrder ppo = new PrintPurchaseOrder();
                        pnlAddEdit.Visible = false;
                        pnlList.Visible = true;
                        grdList.Visible = true;
                        btnExit.Visible = true;
                        //CSLERP.PrintForms.PrintPurchaseOrder ppo = new CSLERP.PrintForms.PrintPurchaseOrder();
                        string TotalTaxDetailstr = "";
                        for (int n = 0; n < (TaxDetailsTable.Rows.Count); n++)
                        {
                            TotalTaxDetailstr = TotalTaxDetailstr + Convert.ToString(TaxDetailsTable.Rows[n][0]) + "-" +
                            Convert.ToString(TaxDetailsTable.Rows[n][1]) + "\n";
                        }
                        PrintWorkOrder pwo = new PrintWorkOrder();
                        List<workorderdetail> WODetails = WorkOrderDB.getWorkOrderDetails(prevwoh);
                        pwo.PrintWO(prevwoh, WODetails, TotalTaxDetailstr);
                        btnExit.Visible = true;
                        return;
                    }



                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                    tabControl1.SelectedTab = tabWOHeader;
                    foreach (Control c in tabWOHeader.Controls)
                    {
                        int cc = tabWOHeader.Controls.Count;
                        string nm = c.Name;
                        if (c.Name.Equals("lblWOS") || c.Name.Equals("cmbWOStatus") || c.Name.Equals("btnTaxDetail"))
                        {

                        }
                        else
                            c.Enabled = false;
                    }
                    tabControl1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
                setButtonVisibility("init");
            }
        }
        private void pnlUI_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnCancel_Click_2(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }

        private void btnAddNewLine_Click(object sender, EventArgs e)
        {
            AddWODetailRow();
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            verifyAndReworkWODetailGridRows();
        }

        private void ClearEntries_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = grdWODetail.Rows.Count - 1; i >= 0; i--)
                {
                    grdWODetail.Rows.RemoveAt(i);
                }
            }
            catch (Exception)
            {
            }
            MessageBox.Show("item cleared.");
        }


        //-----
        //comment handling procedurs and functions
        //-----
        private void btnSelectCommenters_Click(object sender, EventArgs e)
        {
            removeControlsFromCommenterPanel();
            docCmtrDB = new DocCommenterDB();
            lvCmtr = new ListView();
            lvCmtr.Clear();
            pnlCmtr.BorderStyle = BorderStyle.FixedSingle;
            pnlCmtr.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
            lvCmtr = docCmtrDB.commenterLV(docID);
            docCmtrDB.verifyCommenterList(lvCmtr, dtCmtStatus);
            lvCmtr.Bounds = new Rectangle(new Point(50, 50), new Size(500, 200));
            pnlCmtr.Controls.Add(lvCmtr);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Size = new Size(150, 20);
            lvOK.Location = new Point(50, 270);
            lvOK.Click += new System.EventHandler(this.lvOK_Click);
            pnlCmtr.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Size = new Size(150, 20);
            lvCancel.Location = new Point(250, 270);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click);
            pnlCmtr.Controls.Add(lvCancel);
            ////lvCancel.Visible = true;

            pnlCmtr.BringToFront();
            pnlCmtr.Visible = true;
            pnlComments.Controls.Add(pnlCmtr);
            pnlComments.BringToFront();
            pnlCmtr.BringToFront();

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
                            //MessageBox.Show(itemRow.SubItems[1].Text);
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
        private void dgvComments_CellDoubleClick(Object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                ////string columnName = grdList.Columns[e.ColumnIndex].Name;
                PrintForms.SimpleReportViewer.ShowDialog(dgvComments.Rows[e.RowIndex].Cells[3].Value.ToString(), "My Message", this);
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
        private void btnViewDocument_Click(object sender, EventArgs e)
        {

        }

        private void btnCloseDocument_Click(object sender, EventArgs e)
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

        private void setButtonVisibility(string btnName)
        {
            try
            {

                //btnNew.Visible = false;
                btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;

                //btnGetComments.Visible = false;
                // chkCommentStatus.Visible = false;
                txtComments.Visible = false;
                disableTabPages();
                clearTabPageControls();
                handleGrdEditButton();
                handleGrdViewButton();
                //----24/11/2016
                pnlBottomButtons.Visible = true;
                //----
                if (btnName == "init")
                {
                    ////btnNew.Visible = true; 24/11/2016
                    btnExit.Visible = true;
                }
                else if (btnName == "Commenter")
                {
                    btnCancel.Visible = true;
                    btnSave.Visible = true;

                    tabComments.Enabled = true;

                    //btnGetComments.Visible = false; //earlier Edit enabled this button
                    //chkCommentStatus.Visible = true;
                    txtComments.Visible = true;
                }
                else if (btnName == "btnNew")
                {
                    //btnNew.Visible = false; //added 24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    enableTabPages();

                    tabComments.Enabled = false;

                    tabControl1.SelectedTab = tabWOHeader;
                }
                else if (btnName == "btnEditPanel") //called from cancel,save,forward,approve and reverse button events
                {
                    ////btnNew.Visible = true;
                    btnExit.Visible = true;
                }
                //gridview buttons
                else if (btnName == "Edit")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    //btnGetComments.Visible = true;
                    enableTabPages();

                    //chkCommentStatus.Visible = true;
                    txtComments.Visible = true;
                    tabControl1.SelectedTab = tabWOHeader;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;

                    disableTabPages();
                    tabControl1.SelectedTab = tabWOHeader;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;

                    tabControl1.SelectedTab = tabWOHeader;
                }
                else if (btnName == "LoadDocument")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
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
        void handleGrdEditButton()
        {
            grdList.Columns["Edit"].Visible = false;
            //grdList.Columns["Approve"].Visible = false;
            //Boolean m = Main.itemPriv[2];
            //Boolean mm = Main.itemPriv[1];
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {

                grdList.Columns["Edit"].Visible = true;
                //grdList.Columns["Approve"].Visible = true;
            }
        }

        void handleGrdViewButton()
        {
            grdList.Columns["View"].Visible = false;
            //if any one of view,add and edit
            //Boolean m = Main.itemPriv[0];
            if (Main.itemPriv[0])
            {
                //list option 1 should not have view buttuon visible (edit is avialable)
                grdList.Columns["View"].Visible = true;
            }
        }
        //call this form when new or edit buttons are clicked
        private void clearTabPageControls()
        {
            try
            {
                dgvComments.Rows.Clear();
                //chkCommentStatus.Checked = false;
                txtComments.Text = "";
                grdWODetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }
        private void removeControlsFromPnllvPanel()
        {
            try
            {
                foreach (Control p in pnllv.Controls)
                    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
                    {
                        p.Dispose();
                    }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnTaxDetail_Click_1(object sender, EventArgs e)
        {
            try
            {
                string strTax = "";
                for (int i = 0; i < (TaxDetailsTable.Rows.Count); i++)
                {
                    strTax = strTax + Convert.ToString(TaxDetailsTable.Rows[i][0]) + "-" +
                    Convert.ToString(TaxDetailsTable.Rows[i][1]) + "\n";
                }
                DialogResult dialog = MessageBox.Show(strTax, "Tax Details", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error showing tax details");
            }
        }

        private void grdWODetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void WorkOrderStatus_Enter(object sender, EventArgs e)
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

