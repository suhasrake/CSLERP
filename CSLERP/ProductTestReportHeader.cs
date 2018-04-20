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
    public partial class ProductTestReportHeader : System.Windows.Forms.Form
    {
        string docID = "PRODUCTTESTREPORT";
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        string forwarderList = "";
        string approverList = "";
        producttestreportheader prevptrh;
        Panel pnllv = new Panel();
        ListView lv = new ListView();
        string userString = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        string planDetail = "";
        string modNo = "";
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        string prodName = "";
        double quant = 0;
        Form frmPopup = new Form();
        public ProductTestReportHeader()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void ProductTestReportHeader_Load(object sender, EventArgs e)
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
            pnlShowPlanDetail.Visible = true;
            disableControlsInPnlList();
            //ListFilteredPTRHeader(listOption);
        }
        private void disableControlsInPnlList()
        {
            pnlList.Visible = true;
            pnlBottomButtons.Visible = false;
            pnlTobButtons.Visible = false;
            grdList.Visible = false;
            pnlShowPlanDetail.Visible = true;
        }
        private void enableControlsInPnlList()
        {
            pnlBottomButtons.Visible = true;
            pnlTobButtons.Visible = true;
            grdList.Visible = true;
            pnlList.Visible = true;
        }
        private void ListFilteredPTRHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                ProductTestReportHeaderDB ptrDB = new ProductTestReportHeaderDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                List<producttestreportheader> PTRHeaders = ptrDB.getFilteredProductTestReportHeader(userString, option, planDetail);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (producttestreportheader ptrh in PTRHeaders)
                {
                    if (option == 1)
                    {
                        if (ptrh.DocumentStatus == 99)
                            continue;
                    }

                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["Did"].Value = ptrh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["TempNo"].Value = ptrh.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["TempDate"].Value = ptrh.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["ReportNo"].Value = ptrh.ReportNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["Reportdate"].Value = ptrh.ReportDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProductSerialNo"].Value = ptrh.ProductSerialNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProductionPlanNo"].Value = ptrh.ProductionPalnNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["ProductionPlanDate"].Value = ptrh.ProductionPlanDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["StockItemID"].Value = ptrh.StockItemID;
                    grdList.Rows[grdList.RowCount - 1].Cells["StockItemName"].Value = ptrh.StockItemName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Status"].Value = ptrh.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["DocumentStatus"].Value = ptrh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = ptrh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = ptrh.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = ptrh.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = ptrh.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateUser"].Value = ptrh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = ptrh.ForwarderList;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Product Test Report Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;

            grdList.Columns["Creator"].Visible = true;
            grdList.Columns["Forwarder"].Visible = true;
            grdList.Columns["Approver"].Visible = true;


        }

        private void initVariables()
        {

            if (getuserPrivilegeStatus() == 1)
            {
                //user is only a viewer
                listOption = 6;
            }
            docID = Main.currentDocument;
            dtTemporaryDate.Format = DateTimePickerFormat.Custom;
            dtTemporaryDate.CustomFormat = "dd-MM-yyyy";
            dtTemporaryDate.Enabled = false;
            dtProdPlanDate.Format = DateTimePickerFormat.Custom;
            dtProdPlanDate.CustomFormat = "dd-MM-yyyy";
            dtProdPlanDate.Enabled = false;
            dtReportDate.Format = DateTimePickerFormat.Custom;
            dtReportDate.CustomFormat = "dd-MM-yyyy";
            dtReportDate.Enabled = false;

            dtSelectedPlanDate.Format = DateTimePickerFormat.Custom;
            dtSelectedPlanDate.CustomFormat = "dd-MM-yyyy";
            dtSelectedPlanDate.Enabled = false;
            //txtCreditPeriods.Enabled = true;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            grdPTRDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            //userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
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
                grdPTRDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                //dgvpt.Rows.Clear();
                //----------clear temperory panels

                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                //----------
                dtTemporaryDate.Value = DateTime.Parse("01-01-1900");
                dtReportDate.Value = DateTime.Parse("01-01-1900");
                dtProdPlanDate.Value = DateTime.Parse("01-01-1900");
                grdPTRDetail.Rows.Clear();
                txtPlannedQuantity.Text = "";
                txtTemporaryNo.Text = "";
                txtReportNo.Text = "";

                txtProdPlanNo.Text = "";
                txtPlannedQuantity.Text = "";
                txtPlannedSerailNo.Text = "";
                prevptrh = new producttestreportheader();
                removeControlsFromForwarderPanel();
                removeControlsFromPnlLvPanel();
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
                int planNo = Convert.ToInt32(planDetail.Substring(0, planDetail.IndexOf('-')));
                if (!ProductTestReportHeaderDB.checkForApprove(planNo, quant))
                {
                    return;
                }
                clearData();
                btnSave.Text = "Save";
                pnlAddEdit.Visible = true;
                closeAllPanels();
                pnlAddEdit.Visible = true;
                tabPTRHeader.Enabled = true;
                setButtonVisibility("btnNew");
                addNewTestReport(planDetail, prodName);
            }
            catch (Exception)
            {

            }
        }
        private void addNewTestReport(string planDetail, string prodName)
        {
            int planNo = Convert.ToInt32(planDetail.Substring(0, planDetail.IndexOf('-')));
            DateTime planDate = Convert.ToDateTime(planDetail.Substring(planDetail.IndexOf('-') + 1));
            txtProdPlanNo.Text = planNo.ToString(); ;
            dtProdPlanDate.Value = Convert.ToDateTime(planDate);
            txtPlannedQuantity.Text = quant.ToString(); ;
            txtProdName.Text = prodName;
            AddPTRGridDetail(prodName);
        }
        private void btnAddLine_Click(object sender, EventArgs e)
        {

        }

        private Boolean AddPTRDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdPTRDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkPTRDetailGridRows())
                    {
                        return false;
                    }
                }
                grdPTRDetail.Rows.Add();
                int kount = grdPTRDetail.RowCount;
                grdPTRDetail.Rows[kount - 1].Cells[0].Value = kount;
                grdPTRDetail.Rows[kount - 1].Cells["SLNo"].Value = kount;
                grdPTRDetail.Rows[kount - 1].Cells["TestDescriptionID"].Value = "";
                grdPTRDetail.Rows[kount - 1].Cells["TestDescription"].Value = "";
                grdPTRDetail.Rows[kount - 1].Cells["ExpectedResult"].Value = "";
                grdPTRDetail.Rows[kount - 1].Cells["ActualResult"].Value = "";
                grdPTRDetail.Rows[kount - 1].Cells["TestStatus"].Value = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddPTRDetailRow() : Error");
            }

            return status;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            closeAllPanels();
            btnNew.Visible = true;
            btnExit.Visible = true;
            pnlList.Visible = true;
        }

        private Boolean verifyAndReworkPTRDetailGridRows()
        {
            Boolean status = true;

            try
            {
                if (grdPTRDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Purchase Order details");
                    return false;
                }
                for (int i = 0; i < grdPTRDetail.Rows.Count; i++)
                {
                    grdPTRDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if ((Convert.ToInt32(grdPTRDetail.Rows[i].Cells["SLNo"].Value) == 0) ||
                        (grdPTRDetail.Rows[i].Cells["TestDescriptionID"].Value.ToString().Trim().Length == 0) ||
                        (grdPTRDetail.Rows[i].Cells["ExpectedResult"].Value.ToString().Trim().Length == 0) ||
                        (grdPTRDetail.Rows[i].Cells["ActualResult"].Value.ToString().Trim().Length == 0) ||
                        (grdPTRDetail.Rows[i].Cells["TestStatus"].Value.ToString().Trim().Length == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Grid Detail Validation Failed.");
                return false;
            }
            return status;
        }
        private List<producttestreportdetail> getPTRDetails(producttestreportheader ptrh)
        {
            List<producttestreportdetail> PTRDetails = new List<producttestreportdetail>();
            try
            {
                producttestreportdetail ptrd = new producttestreportdetail();
                for (int i = 0; i < grdPTRDetail.Rows.Count; i++)
                {
                    ptrd = new producttestreportdetail();
                    ptrd.DocumentID = ptrh.DocumentID;
                    ptrd.TemporaryNo = ptrh.TemporaryNo;
                    ptrd.TemporaryDate = ptrh.TemporaryDate;
                    ptrd.TestDescriptionID = grdPTRDetail.Rows[i].Cells["TestDescriptionID"].Value.ToString();
                    ptrd.ExpectedResult = grdPTRDetail.Rows[i].Cells["ExpectedResult"].Value.ToString().Trim();
                    ptrd.ActualResult = grdPTRDetail.Rows[i].Cells["ActualResult"].Value.ToString();
                    ptrd.TestStatus = getStatusValue(grdPTRDetail.Rows[i].Cells["TestStatus"].Value.ToString());
                    ptrd.SLNo = Convert.ToInt32(grdPTRDetail.Rows[i].Cells["SLNo"].Value.ToString());
                    PTRDetails.Add(ptrd);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("createAndUpdatePTRDetails() : Error updating Product Test Report Details");
                PTRDetails = null;
            }
            return PTRDetails;
        }
        private int getStatusValue(string code)
        {
            int n = 0;
            if (code.Equals("Pass"))
                n = 1;
            else if (code.Equals("Fail"))
                n = 0;
            return n;
        }
        private string getStatusCode(int status)
        {
            string str = "";
            if (status == 1)
                str = "Pass";
            else if (status == 0)
                str = "Fail";
            return str;
        }
        private void grdPTRDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            string columnName = grdPTRDetail.Columns[e.ColumnIndex].Name;
            try
            {
                if (columnName.Equals("Delete"))
                {
                    DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        grdPTRDetail.Rows.RemoveAt(e.RowIndex);
                    }
                    verifyAndReworkPTRDetailGridRows();
                }
            }
            catch (Exception)
            {

            }
        }
        private void removeControlsFromPnlLvPanel()
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
        private Boolean validateItems(string item)
        {
            Boolean status = true;
            try
            {
                foreach (DataGridViewRow row in grdPTRDetail.Rows)
                {
                    if (grdPTRDetail.Rows[row.Index].Cells["TestDescriptionID"].Value.ToString() == item)
                    {
                        MessageBox.Show("Test Duplication Not allowed");
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {

            Boolean status = true;
            try
            {

                ProductTestReportHeaderDB ptrDB = new ProductTestReportHeaderDB();
                producttestreportheader ptrh = new producttestreportheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    if (!verifyAndReworkPTRDetailGridRows())
                    {
                        return;
                    }
                    ptrh.DocumentID = docID;
                    ptrh.ReportDate = dtReportDate.Value;
                    ptrh.ProductionPalnNo = Convert.ToInt32(txtProdPlanNo.Text.ToString());
                    ptrh.ProductionPlanDate = dtProdPlanDate.Value;
                    ptrh.ProductSerialNo = txtPlannedSerailNo.Text.ToString();
                    //qih.DocumentNo = Convert.ToInt32(txtDocumentNo.Text);
                    ptrh.ForwarderList = prevptrh.ForwarderList;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (!ptrDB.validateProductTestReportHeader(ptrh))
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    //ptrh.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    ptrh.DocumentStatus = 1; //created
                    ptrh.TemporaryDate = UpdateTable.getSQLDateTime();
                }

                else
                {
                    ptrh.TemporaryNo = Convert.ToInt32(txtTemporaryNo.Text);
                    ptrh.TemporaryDate = prevptrh.TemporaryDate;
                    ptrh.DocumentStatus = prevptrh.DocumentStatus;
                }
                List<producttestreportdetail> PTRDetails = getPTRDetails(ptrh); ;
                if (ptrDB.validateProductTestReportHeader(ptrh))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (ptrDB.updatePTRHeaderAndDetail(ptrh, prevptrh, PTRDetails))
                        {
                            MessageBox.Show("ProductTest report details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredPTRHeader(listOption);
                        }
                        else
                        {
                            status = false;

                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update ProductTest report Header");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (ptrDB.InsertPTRHeaderAndDetail(ptrh, PTRDetails))
                        {
                            MessageBox.Show("PProductTest report Details Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredPTRHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert Product Test Report Header");
                        }
                    }
                }
                else
                {
                    status = false;
                    MessageBox.Show(" Product Test Report Validation failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("createAndUpdatePTRDetails() : Error");
                status = false;
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
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("gEdit") || columnName.Equals("gApprove") ||
                    columnName.Equals("gView"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    ProductTestReportHeaderDB ptrDB = new ProductTestReportHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevptrh = new producttestreportheader();
                    prevptrh.DocumentID = grdList.Rows[e.RowIndex].Cells["Did"].Value.ToString();
                    prevptrh.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["TempNo"].Value.ToString());
                    prevptrh.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["TempDate"].Value.ToString());
                    prevptrh.ReportNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["ReportNo"].Value.ToString());
                    prevptrh.ReportDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["ReportDate"].Value.ToString());
                    prevptrh.ProductionPalnNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["ProductionPlanNo"].Value.ToString());
                    prevptrh.ProductionPlanDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["ProductionPlanDate"].Value.ToString());
                    prevptrh.ProductSerialNo = grdList.Rows[e.RowIndex].Cells["ProductSerialNo"].Value.ToString();
                    prevptrh.StockItemID = grdList.Rows[e.RowIndex].Cells["StockItemID"].Value.ToString();
                    prevptrh.StockItemName = grdList.Rows[e.RowIndex].Cells["StockItemName"].Value.ToString();
                    prevptrh.Status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["Status"].Value.ToString());
                    prevptrh.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["DocumentStatus"].Value.ToString());
                    prevptrh.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["CreateTime"].Value.ToString());
                    prevptrh.CreateUser = grdList.Rows[e.RowIndex].Cells["CreateUser"].Value.ToString();
                    prevptrh.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    prevptrh.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevptrh.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    prevptrh.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    txtReportNo.Text = prevptrh.ReportNo.ToString();
                    txtTemporaryNo.Text = prevptrh.TemporaryNo.ToString();
                    try
                    {
                        dtTemporaryDate.Value = prevptrh.TemporaryDate;
                    }
                    catch (Exception)
                    {

                        dtTemporaryDate.Value = DateTime.Parse("01-01-1900");
                    }
                    try
                    {
                        dtReportDate.Value = prevptrh.ReportDate;
                    }
                    catch (Exception)
                    {
                        dtReportDate.Value = DateTime.Parse("01-01-1900");
                    }
                    try
                    {
                        dtProdPlanDate.Value = prevptrh.ProductionPlanDate;
                    }
                    catch (Exception)
                    {
                        dtProdPlanDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtProdPlanNo.Text = prevptrh.ProductionPalnNo.ToString();
                    txtPlannedSerailNo.Text = prevptrh.ProductSerialNo.ToString();
                    List<producttestreportdetail> PTRDetail = ProductTestReportHeaderDB.getProductTestReportDetails(prevptrh);
                    grdPTRDetail.Rows.Clear();
                    int i = 0;
                    foreach (producttestreportdetail ptrd in PTRDetail)
                    {
                        if (!AddPTRDetailRow())
                        {
                            MessageBox.Show("Error found in Detail. Please correct before updating the details");
                        }
                        else
                        {
                            grdPTRDetail.Rows[i].Cells["SLNo"].Value = ptrd.SLNo;
                            grdPTRDetail.Rows[i].Cells["TestDescriptionID"].Value = ptrd.TestDescriptionID;
                            grdPTRDetail.Rows[i].Cells["TestDescription"].Value = ptrd.TestDescription;
                            grdPTRDetail.Rows[i].Cells["ExpectedResult"].Value = ptrd.ExpectedResult;
                            grdPTRDetail.Rows[i].Cells["ActualResult"].Value = ptrd.ActualResult;
                            grdPTRDetail.Rows[i].Cells["TestStatus"].Value = getStatusCode(ptrd.TestStatus);
                            i++;

                        }

                    }
                    if (!verifyAndReworkPTRDetailGridRows())
                    {
                        MessageBox.Show("Error found in Product Test Report details. Please correct before updating the details");
                    }

                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
                    tabControl1.SelectedTab = tabPTRHeader;
                    tabControl1.Visible = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
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
            AddPTRDetailRow();
        }

        private void Calculate_Click(object sender, EventArgs e)
        {
            verifyAndReworkPTRDetailGridRows();
        }

        private void ClearEntries_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = grdPTRDetail.Rows.Count - 1; i >= 0; i--)
                {
                    grdPTRDetail.Rows.RemoveAt(i);
                }
            }
            catch (Exception)
            {
            }
            MessageBox.Show("item cleared.");
        }
        private void pnlAddEdit_Paint(object sender, PaintEventArgs e)
        {

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

            ListFilteredPTRHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredPTRHeader(listOption);
        }

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredPTRHeader(listOption);
        }
        private void btnSelect_Click_1(object sender, EventArgs e)
        {
            //removeControlsFromForwarderPanel();
            if (grdPTRDetail.Rows.Count != 0 || txtProdPlanNo.Text.Length != 0)
            {
                DialogResult dialog = MessageBox.Show("Grid Detail will be removed.", "OK", MessageBoxButtons.OKCancel);

                if (dialog == DialogResult.OK)
                {
                    txtProdPlanNo.Text = "";
                    dtProdPlanDate.Value = DateTime.Parse("01-01-1900"); ;
                    grdPTRDetail.Rows.Clear();
                }
                else
                    return;
            }
            //btnSelect.Enabled = false;
            //lvApprover = new ListView();
            //l/vApprover.Clear();
            //pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
            //pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            lvApprover = ProductionPlanHeaderDB.getPlanNoListView();
            lvApprover.Bounds = new Rectangle(new Point(0,0), new Size(450, 250));
            //pnlForwarder.Controls.Remove(lvApprover);
            frmPopup.Controls.Add(lvApprover);

            Button lvForwrdOK = new Button();
            lvForwrdOK.BackColor = Color.Tan;
            lvForwrdOK.Text = "OK";
            lvForwrdOK.Location = new Point(40, 265);
            lvForwrdOK.Click += new System.EventHandler(this.lvOK_Click1);
            frmPopup.Controls.Add(lvForwrdOK);

            Button lvForwardCancel = new Button();
            lvForwardCancel.BackColor = Color.Tan;
            lvForwardCancel.Text = "CANCEL";
            lvForwardCancel.Location = new Point(130, 265);
            lvForwardCancel.Click += new System.EventHandler(this.lvCancel_Click1);
            frmPopup.Controls.Add(lvForwardCancel);
            ////lvForwardCancel.Visible = false;
            frmPopup.ShowDialog();
            //pnlForwarder.Visible = true;
            //pnlList.Controls.Add(pnlForwarder);
            //pnlList.BringToFront();
            //pnlForwarder.BringToFront();
            //pnlForwarder.Focus();
        }
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                foreach (ListViewItem itemRow in lvApprover.Items)
                {
                    if (itemRow.Checked)
                    {
                        count++;
                    }
                }
                if (count != 1)
                {
                    MessageBox.Show("Select one item.");
                    return;
                }
                foreach (ListViewItem itemRow in lvApprover.Items)
                {
                    if (itemRow.Checked)
                    {
                        string id = itemRow.SubItems[3].Text.Substring(0, itemRow.SubItems[3].Text.IndexOf('-'));
                        string ModelNo = itemRow.SubItems[4].Text;
                        if (!ProductTestTemplateDB.checkProductAvailableForPrint(id, ModelNo))
                        {
                            MessageBox.Show("Template not prepared for this Product");
                            return;
                        }
                        modNo = itemRow.SubItems[4].Text;
                        planDetail = itemRow.SubItems[1].Text + "-" + itemRow.SubItems[2].Text;
                        txtSelectedPlanNo.Text = itemRow.SubItems[1].Text;
                        dtSelectedPlanDate.Value = Convert.ToDateTime(itemRow.SubItems[2].Text);
                        quant = Convert.ToDouble(itemRow.SubItems[6].Text);
                        prodName = itemRow.SubItems[3].Text;
                        //pnlForwarder.Controls.Remove(lvApprover);
                        //pnlForwarder.Visible = false;
                        frmPopup.Close();
                        frmPopup.Dispose();
                        enableControlsInPnlList();
                        //btnSelect.Enabled = true;
                        if (getuserPrivilegeStatus() == 1)
                        {
                            listOption = 6;
                            ListFilteredPTRHeader(listOption);
                        }
                        else
                        {
                            listOption = 1;
                            ListFilteredPTRHeader(listOption);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void lvCancel_Click1(object sender, EventArgs e)
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
        private void AddPTRGridDetail(string nm)
        {
            string id = nm.Substring(0, nm.IndexOf('-'));
            string name = nm.Substring(nm.IndexOf('-') + 1);
            List<producttesttemplatedetail> ptdetail = ProductTestTemplateDB.getTemplateDetailForReport(id, name, modNo);
            int i = 0;
            foreach (producttesttemplatedetail pt in ptdetail)
            {
                grdPTRDetail.Rows.Add();
                grdPTRDetail.Rows[i].Cells["LineNo"].Value = i + 1;
                grdPTRDetail.Rows[i].Cells["SLNo"].Value = pt.SlNo;
                grdPTRDetail.Rows[i].Cells["TestDescriptionID"].Value = pt.TestDescriptionID;
                grdPTRDetail.Rows[i].Cells["TestDescription"].Value = pt.TestDescription;
                grdPTRDetail.Rows[i].Cells["ExpectedResult"].Value = pt.ExpectedResult;
                grdPTRDetail.Rows[i].Cells["ActualResult"].Value = "";
                i++;
            }
        }
        private Boolean updateDashBoard(producttestreportheader ptrh, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = docID;
                dsb.TemporaryNo = ptrh.TemporaryNo;
                dsb.TemporaryDate = ptrh.TemporaryDate;
                dsb.DocumentNo = ptrh.ReportNo;
                dsb.DocumentDate = ptrh.ReportDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = ptrh.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(docID);
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
        private void btnForward_Click(object sender, EventArgs e)
        {
            //removeControlsFromForwarderPanel();
            //lvApprover = new ListView();
            //lvApprover.Clear();
            //pnlForwarder.BorderStyle = BorderStyle.FixedSingle;
            //pnlForwarder.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));
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
            ////lvForwardCancel.Visible = false;
            frmPopup.ShowDialog();
            //pnlForwarder.Visible = true;
            //pnlAddEdit.Controls.Add(pnlForwarder);
            //pnlAddEdit.BringToFront();
            //pnlForwarder.BringToFront();
            //pnlForwarder.Focus();
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                ProductTestReportHeaderDB ptrDB = new ProductTestReportHeaderDB();
                DialogResult dialog = MessageBox.Show("Are you sure to forward the document for Approval ?", "Yes", MessageBoxButtons.YesNo);

                if (dialog == DialogResult.Yes)
                {
                    prevptrh.ReportNo = DocumentNumberDB.getNewNumber(docID, 2);

                    if (ptrDB.ApproveProductTestReportHeader(prevptrh))
                    {
                        MessageBox.Show("Product Test Document Approved");
                        if (!updateDashBoard(prevptrh, 2))
                        {
                            MessageBox.Show("DashBoard Fail to update");
                        }
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredPTRHeader(listOption);
                    }
                }
            }
            catch (Exception)
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
                            ProductTestReportHeaderDB ptrDB = new ProductTestReportHeaderDB();
                            prevptrh.ForwardUser = approverUID;
                            prevptrh.ForwarderList = prevptrh.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (ptrDB.ForwardProductTestReportHeader(prevptrh))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(prevptrh, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredPTRHeader(listOption);
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
        //-----
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
                    string reverseStr = getReverseString(prevptrh.ForwarderList);
                    //do forward activities
                    ProductTestReportHeaderDB ptrDB = new ProductTestReportHeaderDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevptrh.ForwarderList = reverseStr.Substring(0, ind);
                        prevptrh.ForwardUser = reverseStr.Substring(ind + 3);
                        prevptrh.DocumentStatus = prevptrh.DocumentStatus - 1;
                    }
                    else
                    {
                        prevptrh.ForwarderList = "";
                        prevptrh.ForwardUser = "";
                        prevptrh.DocumentStatus = 1;
                    }
                    if (ptrDB.reverseProductTestReportHeader(prevptrh))
                    {
                        MessageBox.Show("Product Test report Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredPTRHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnAddNewLine_Click_1(object sender, EventArgs e)
        {
            try
            {
                AddPTRDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private void btnClearEntries_Click(object sender, EventArgs e)
        {
            grdPTRDetail.Rows.Clear();
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
        //private void removeControlsFromPnllvPanel()
        //{
        //    try
        //    {
        //        foreach (Control p in pnllv.Controls)
        //            if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
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
                //----24/11/2016
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
                else if (btnName == "Commenter")
                {
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                }
                else if (btnName == "btnNew")
                {
                    btnNew.Visible = false; //added 24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    enableTabPages();
                    tabControl1.SelectedTab = tabPTRHeader;
                }
                else if (btnName == "btnEditPanel") //called from cancel,save,forward,approve and reverse button events
                {
                    ////btnNew.Visible = true;
                    btnExit.Visible = true;
                }
                //gridview buttons
                else if (btnName == "gEdit")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    enableTabPages();
                    tabControl1.SelectedTab = tabPTRDetail;
                }
                else if (btnName == "gApprove")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                    disableTabPages();
                    tabControl1.SelectedTab = tabPTRDetail;
                }
                else if (btnName == "gView")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabControl1.SelectedTab = tabPTRDetail;
                }
                else if (btnName == "gLoadDocument")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                }
                pnlEditButtons.Refresh();
                //if the user privilege is only view, show only the Approved documents button
                int ups = getuserPrivilegeStatus();
                if (ups == 1 || ups == 0)
                {
                    grdList.Columns["gEdit"].Visible = false;
                    grdList.Columns["gApprove"].Visible = false;
                    btnActionPending.Visible = false;
                    btnApprovalPending.Visible = false;
                    btnApproved.Visible = false;
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
                btnNew.Visible = true;
            }
        }
        void handleGrdEditButton()
        {
            grdList.Columns["gEdit"].Visible = false;
            grdList.Columns["gApprove"].Visible = false;
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                if (listOption == 1)
                {
                    grdList.Columns["gEdit"].Visible = true;
                    grdList.Columns["gApprove"].Visible = true;
                }
            }
        }
        void handleGrdViewButton()
        {
            grdList.Columns["gView"].Visible = false;
            //if any one of view,add and edit
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
                removeControlsFromForwarderPanel();
                dgvComments.Rows.Clear();
                grdPTRDetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            removeControlsFromForwarderPanel();
            removeControlsFromPnlLvPanel();
        }

        private void ProductTestReportHeader_Enter(object sender, EventArgs e)
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

