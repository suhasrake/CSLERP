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
    public partial class ProductServiceReportHeader : System.Windows.Forms.Form
    {
        string docID = "";
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        productservicereportheader prevpsrh;
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        //popiheader prevpopi = new popiheader();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        Boolean userIsACommenter = false;
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Form frmPopup = new Form();
        public ProductServiceReportHeader()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void ProductServiceReportHeader_Load(object sender, EventArgs e)
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
            ListFilteredPSRHeader(listOption);
            //applyPrivilege();
        }
        private void ListFilteredPSRHeader(int opt)
        {
            try
            {
                grdList.Rows.Clear();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);
                ProductServiceReportHeaderDB psrDB = new ProductServiceReportHeaderDB();
                List<productservicereportheader> PSRHeaders = psrDB.getFilteredPSRHeader(userString, opt, userCommentStatusString);
                if (opt == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (opt == 2)
                    lblActionHeader.Text = "List of Approved Documents";
                else if (opt == 3 || opt == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (productservicereportheader psrh in PSRHeaders)
                {
                    //if (opt == 1)
                    //{
                    //    if (psrh.DocumentStatus == 99)
                    //        continue;
                    //}
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = psrh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = psrh.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ReportType"].Value = psrh.ReportType;
                    grdList.Rows[grdList.RowCount - 1].Cells["ReportNo"].Value = psrh.ReportNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["ReportDate"].Value = psrh.ReportDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = psrh.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = psrh.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["SMRNNo"].Value = psrh.SMRNNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["SMRNDate"].Value = psrh.SMRNDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["SMRNHeaderNo"].Value = psrh.SMRNHeaderTempNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["SMRNHeaderDate"].Value = psrh.SMRNHeaderTempDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["JobIDNo"].Value = psrh.jobIDNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = psrh.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = psrh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = psrh.CreateUser;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Product ServiceReport Detail Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;

            try
            {
                grdList.Columns["Creator"].Visible = true;
                // grdList.Columns["Forwarder"].Visible = true;
                // grdList.Columns["Approver"].Visible = true;
            }
            catch (Exception ex)
            {
            }
        }

        private void initVariables()
        {
            if (getuserPrivilegeStatus() == 1)
            {
                //user is only a viewer
                listOption = 2;
            }
            docID = Main.currentDocument;
            dtreportDate.Format = DateTimePickerFormat.Custom;
            dtreportDate.CustomFormat = "dd-MM-yyyy";
            dtreportDate.Enabled = false;
            dtTemporaryDate.Format = DateTimePickerFormat.Custom;
            dtTemporaryDate.CustomFormat = "dd-MM-yyyy";
            dtTemporaryDate.Enabled = false;
            dtSMRNDate.Format = DateTimePickerFormat.Custom;
            dtSMRNDate.CustomFormat = "dd-MM-yyyy";
            dtSMRNDate.Enabled = false;
            dtSMRNHeaderDate.Format = DateTimePickerFormat.Custom;
            dtSMRNHeaderDate.CustomFormat = "dd-MM-yyyy";
            ////dtSMRNHeaderDate.Enabled = true;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            grdPSRDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
        }
        //private void fillStatusCombo(System.Windows.Forms.ComboBox cmb)
        //{
        //    try
        //    {
        //        cmb.Items.Clear();
        //        for (int i = 0; i < Main.statusValues.GetLength(0); i++)
        //        {
        //            cmb.Items.Add(Main.statusValues[i, 1]);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}
        //private void applyPrivilege()
        //{
        //    try
        //    {
        //        if (Main.itemPriv[1])
        //        {
        //            btnNew.Visible = true;
        //        }
        //        else
        //        {
        //            btnNew.Visible = false;
        //        }
        //        if (Main.itemPriv[2])
        //        {
        //            grdList.Columns["Edit"].Visible = true;
        //        }
        //        else
        //        {
        //            grdList.Columns["Edit"].Visible = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

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
                grdPSRDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                //----------clear temperory panels
                int n = cmbReportType.Items.Count;
                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                txtJobIDNo.Text = "";
                txtReportNo.Text = "";
                dtreportDate.Value = DateTime.Parse("01-01-1900");
                cmbReportType.SelectedIndex = 0;
                dtreportDate.Value = DateTime.Parse("01-01-1900");
                dtSMRNDate.Value = DateTime.Parse("01-01-1900");
                dtSMRNHeaderDate.Value = DateTime.Parse("01-01-1900");
                dtTemporaryDate.Value = DateTime.Parse("01-01-1900");
                txtTemporaryNo.Text = "";
                grdPSRDetail.Rows.Clear();
                txtSMRNNo.Text = "";
                txtSMRNHeaderNo.Text = "";
                prevpsrh = new productservicereportheader();
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
                clearData();
                btnSave.Text = "Save";
                pnlAddEdit.Visible = true;
                closeAllPanels();
                pnlAddEdit.Visible = true;
                setButtonVisibility("btnNew");
            }
            catch (Exception)
            {

            }
        }


        private void btnAddLine_Click(object sender, EventArgs e)
        {

        }
        private void btnAddNewLine_Click_1(object sender, EventArgs e)
        {
            try
            {
                AddPSRDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private Boolean AddPSRDetailRow()
        {
            Boolean status = true;
            try
            {
                if (grdPSRDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkPSRDetailGridRows())
                    {
                        return false;
                    }
                }
                grdPSRDetail.Rows.Add();
                int kount = grdPSRDetail.RowCount;
                grdPSRDetail.Rows[kount - 1].Cells[0].Value = kount;
                grdPSRDetail.Rows[kount - 1].Cells["TestDescriptionID"].Value = "";
                grdPSRDetail.Rows[kount - 1].Cells["TestResult"].Value = "";
                grdPSRDetail.Rows[kount - 1].Cells["TestRemarks"].Value = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddQIDetailRow() : Error");
            }

            return status;
        }
        //public static void fillTaxCodeGridViewCombo(DataGridViewComboBoxCell cmb, string CategoryName)
        //{
        //    cmb.Items.Clear();
        //    try
        //    {
        //        TaxCodeDB tcdb = new TaxCodeDB();
        //        List<taxcode> TaxCodes = tcdb.getTaxCode();
        //        foreach (taxcode tc in TaxCodes)
        //        {
        //            cmb.Items.Add(tc.TaxCode);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
        //    }

        //}
        private void btnCancel_Click(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }

        private Boolean verifyAndReworkPSRDetailGridRows()
        {
            Boolean status = true;

            try
            {

                if (grdPSRDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Quotation Inward details");
                    return false;
                }
                //clear tax details table
                TaxDetailsTable.Rows.Clear();
                for (int i = 0; i < grdPSRDetail.Rows.Count; i++)
                {
                    //int i = 0;
                    grdPSRDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if (((grdPSRDetail.Rows[i].Cells["TestDescriptionID"].Value.ToString().Length == 0)) ||
                        (grdPSRDetail.Rows[i].Cells["TestResult"].Value.ToString().Length == 0) ||
                        (grdPSRDetail.Rows[i].Cells["TestRemarks"].Value.ToString().Length == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                }
                //if (!validateItems())
                //{
                //    MessageBox.Show("Validation failed");
                //}
            }
            catch (Exception ex)
            {
                return false;
            }
            return status;
        }
        private Boolean validateItems(string item)
        {
            Boolean status = true;
            try
            {
                foreach (DataGridViewRow row in grdPSRDetail.Rows)
                {
                    if (grdPSRDetail.Rows[row.Index].Cells["TestDescriptionID"].Value.ToString() == item)
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

        private List<productservicereportdetail> getPSRDetails(productservicereportheader psrh)
        {
            List<productservicereportdetail> PSRDetails = new List<productservicereportdetail>();
            try
            {
                productservicereportdetail psrd = new productservicereportdetail();
                for (int i = 0; i < grdPSRDetail.Rows.Count; i++)
                {
                    psrd = new productservicereportdetail();
                    psrd.TestDescriptionID = grdPSRDetail.Rows[i].Cells["TestDescriptionID"].Value.ToString().Trim();
                    psrd.TestRemarks = grdPSRDetail.Rows[i].Cells["TestRemarks"].Value.ToString().Trim();
                    psrd.TestResult = grdPSRDetail.Rows[i].Cells["TestResult"].Value.ToString();
                    PSRDetails.Add(psrd);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("getPSRDetails() : Error updating PSR Details");
                PSRDetails = null;
            }
            return PSRDetails;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdPSRDetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdPSRDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkPSRDetailGridRows();
                    }
                    if (columnName.Equals("Select"))
                    {
                        //grdPSRDetail.
                        ShowTestDescriptionIDListView();
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {

            }
        }
        private void cmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int doc = Convert.ToInt32(cmbReportType.SelectedItem.ToString().Trim().Substring(0, cmbReportType.SelectedItem.ToString().Trim().IndexOf('-')));
            if (doc == 1)
                docID = "PSRPRELIMINARY";
            else if (doc == 2)
                docID = "PSRFINAL";

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {
                ProductServiceReportHeaderDB prshDB = new ProductServiceReportHeaderDB();
                productservicereportheader prsh = new productservicereportheader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;
                try
                {
                    if (!verifyAndReworkPSRDetailGridRows())
                    {
                        return;
                    }
                    prsh.DocumentID = docID;
                    prsh.ReportType = Convert.ToInt32(cmbReportType.SelectedItem.ToString().Trim().Substring(0, cmbReportType.SelectedItem.ToString().Trim().IndexOf('-')));
                    prsh.ReportDate = dtreportDate.Value;
                    prsh.SMRNNo = Convert.ToInt32(txtSMRNNo.Text);
                    prsh.SMRNDate = dtSMRNDate.Value;
                    prsh.SMRNHeaderTempNo = Convert.ToInt32(txtSMRNHeaderNo.Text);
                    prsh.SMRNHeaderTempDate = dtSMRNHeaderDate.Value;
                    prsh.jobIDNo = Convert.ToInt32(txtJobIDNo.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    //prsh.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    prsh.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    prsh.TemporaryNo = Convert.ToInt32(txtTemporaryNo.Text);
                    prsh.TemporaryDate = prevpsrh.TemporaryDate;
                }
                List<productservicereportdetail> PSRDetails = getPSRDetails(prsh);
                if (prshDB.validatePSRHeader(prsh))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (prshDB.updatePSRHeaderAndDetail(prsh, prevpsrh, PSRDetails))
                        {
                            MessageBox.Show("Product ServiceReport Detail Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredPSRHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update Product ServiceReport");
                            status = false;
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (prshDB.InsertPSRHeaderAndDetail(prsh, PSRDetails))
                        {
                            MessageBox.Show("Product ServiceReport Detail Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredPSRHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert Product ServiceReport Detail");
                            status = false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Product ServiceReport Detail Validation failed");
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Product ServiceReport Detail : Error");
                status = false;
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
                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("View") || columnName.Equals("LoadDocument"))
                {
                    clearData();
                    setButtonVisibility(columnName);
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    QIHeaderDB qidb = new QIHeaderDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];
                    prevpsrh = new productservicereportheader();
                    prevpsrh.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    prevpsrh.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    prevpsrh.ReportType = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["ReportType"].Value.ToString());
                    prevpsrh.ReportNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["ReportNo"].Value.ToString());
                    prevpsrh.ReportDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    prevpsrh.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    prevpsrh.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    prevpsrh.SMRNNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["SMRNNo"].Value.ToString());
                    prevpsrh.SMRNDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["SMRNDate"].Value.ToString());
                    prevpsrh.SMRNHeaderTempNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["SMRNHeaderNo"].Value.ToString());
                    prevpsrh.SMRNHeaderTempDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["SMRNHeaderDate"].Value.ToString());
                    prevpsrh.jobIDNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["JobIDNo"].Value.ToString());
                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Temporary No:" + prevpsrh.TemporaryNo + "\n" +
                            "Temporary Date:" + prevpsrh.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "Report No:" + prevpsrh.ReportNo + "\n" +
                            "Report Date:" + prevpsrh.ReportDate.ToString("dd-MM-yyyy");
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevpsrh.TemporaryNo + "-" + prevpsrh.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_2(null, null);
                        return;
                    }
                    cmbReportType.SelectedIndex = cmbReportType.FindString(prevpsrh.ReportType.ToString());
                    txtReportNo.Text = prevpsrh.ReportNo.ToString();
                    txtJobIDNo.Text = prevpsrh.jobIDNo.ToString();
                    txtSMRNNo.Text = prevpsrh.SMRNNo.ToString();
                    dtSMRNDate.Value = prevpsrh.SMRNDate;
                    txtTemporaryNo.Text = prevpsrh.TemporaryNo.ToString();
                    dtTemporaryDate.Value = prevpsrh.TemporaryDate;
                    try
                    {
                        dtreportDate.Value = prevpsrh.ReportDate;
                    }
                    catch (Exception)
                    {

                        dtreportDate.Value = DateTime.Parse("01-01-1900");
                    }
                    txtSMRNHeaderNo.Text = prevpsrh.SMRNHeaderTempNo.ToString();
                    try
                    {
                        dtSMRNHeaderDate.Value = prevpsrh.SMRNHeaderTempDate;
                    }
                    catch (Exception)
                    {
                        dtSMRNHeaderDate.Value = DateTime.Parse("01-01-1900");
                    }
                    List<productservicereportdetail> PSRDetail = ProductServiceReportHeaderDB.getPSRDetail(prevpsrh);
                    grdPSRDetail.Rows.Clear();
                    int i = 0;
                    foreach (productservicereportdetail qid in PSRDetail)
                    {
                        if (!AddPSRDetailRow())
                        {
                            MessageBox.Show("Error found in QI details. Please correct before updating the details");
                        }
                        else
                        {
                            grdPSRDetail.Rows[i].Cells["TestDescriptionID"].Value = qid.TestDescriptionID;
                            grdPSRDetail.Rows[i].Cells["TestResult"].Value = qid.TestResult;
                            grdPSRDetail.Rows[i].Cells["TestRemarks"].Value = qid.TestRemarks;
                            i++;
                        }

                    }
                    if (!verifyAndReworkPSRDetailGridRows())
                    {
                        MessageBox.Show("Error found in Product ServiceReport details. Please correct before updating the details");
                    }
                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;

                    btnSave.Text = "Update";
                    pnlList.Visible = false;
                    pnlAddEdit.Visible = true;
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
            AddPSRDetailRow();
        }
        private void ClearEntries_Click(object sender, EventArgs e)
        {

        }
        private void ClearEntries_Click_1(object sender, EventArgs e)
        {
            try
            {
                for (int i = grdPSRDetail.Rows.Count - 1; i >= 0; i--)
                {
                    grdPSRDetail.Rows.RemoveAt(i);
                }
            }
            catch (Exception)
            {
            }
            MessageBox.Show("item cleared.");
        }
        private void grdQIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ShowTestDescriptionIDListView()
        {
            //btnSelectJobID.Enabled = false;

            //removeControlsFromPnlLvPanel();
            //pnllv = new Panel();
            //pnllv.BorderStyle = BorderStyle.FixedSingle;

            //pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
            //int tempNo = Convert.ToInt32(txtSMRNHeaderNo.Text);
            //DateTime tempDate = dtSMRNHeaderDate.Value;
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            lv = ProductTestDescriptionDB.getTestDescriptionListView();
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView3_ItemChecked);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Click2);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
            //pnlAddEdit.Controls.Add(pnllv);
            //pnllv.BringToFront();
            //pnllv.Visible = true;
        }
        private void lvOK_Click2(object sender, EventArgs e)
        {
            try
            {
                //btnSelectJobID.Enabled = true;
                if (!checkLVItemChecked("item"))
                {
                    return;
                }
                
                string trlist;
                if (lv.CheckedItems.Count > 0)
                {
                    trlist = "";
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            if (validateItems(itemRow.SubItems[1].Text))
                            {
                                grdPSRDetail.CurrentRow.Cells["TestDescriptionID"].Value = itemRow.SubItems[2].Text;
                            }
                        }
                    }
                }
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_Click2(object sender, EventArgs e)
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
        //private void listView3_ItemChecked(object sender, ItemCheckedEventArgs e)
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
        private void pnlAddEdit_Paint(object sender, PaintEventArgs e)
        {

        }
        private void btnFinalize_Click(object sender, EventArgs e)
        {
            try
            {
                ProductServiceReportHeaderDB psrDB = new ProductServiceReportHeaderDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Finalize the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevpsrh.ReportNo = DocumentNumberDB.getNewNumber(docID, 2);
                    if (psrDB.FinalizePSR(prevpsrh))
                    {
                        if (psrDB.updateInspectionStatusOfItem(prevpsrh))
                        {
                            MessageBox.Show("Report Finalized");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredPSRHeader(listOption);
                            setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                        }
                        else
                            MessageBox.Show("Failed to update Inspection Status in SMRNDetail");
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void setButtonVisibility(string btnName)
        {
            try
            {
                btnActionPending.Visible = true;
                //btnApprovalPending.Visible = true;
                btnApproved.Visible = true;
                btnNew.Visible = false;
                btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;
                btnFinalize.Visible = false;
                //btnApprove.Visible = false;
                //btnReverse.Visible = false;
                //btnGetComments.Visible = false;
                //chkCommentStatus.Visible = false;
                //txtComments.Visible = false;
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
                    pnlPDFViewer.Visible = true;
                    //tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    //btnGetComments.Visible = false; //earlier Edit enabled this button
                    //chkCommentStatus.Visible = true;
                    //txtComments.Visible = true;
                }
                else if (btnName == "btnNew")
                {
                    btnNew.Visible = false; //added 24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = false;
                    //tabComments.Enabled = false;
                    tabPDFViewer.Enabled = false;
                    tabControl1.SelectedTab = tabPSReport;
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
                    pnlPDFViewer.Visible = true;
                    //chkCommentStatus.Visible = true;
                    //txtComments.Visible = true;
                    tabControl1.SelectedTab = tabPSReport;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnFinalize.Visible = true;
                    //btnApprove.Visible = true;
                    //btnReverse.Visible = true;
                    disableTabPages();
                    tabControl1.SelectedTab = tabPSReport;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    //tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabPSReport;
                }
                else if (btnName == "LoadDocument")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                }


                pnlEditButtons.Refresh();
                //if the user privilege is only view, show only the Approved documents button
                int ups = getuserPrivilegeStatus();
                if (ups == 1 || ups == 0)
                {
                    grdList.Columns["Edit"].Visible = false;
                    grdList.Columns["Approve"].Visible = false;
                    btnActionPending.Visible = false;
                    //btnApprovalPending.Visible = false;
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
        private void clearTabPageControls()
        {
            try
            {
                removePDFControls();
                removePDFFileGridView();
                grdPSRDetail.Rows.Clear();
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
        private void btnListDocuments_Click_1(object sender, EventArgs e)
        {

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
        private void btnPDFClose_Click(object sender, EventArgs e)
        {
            removePDFControls();
            showPDFFileGrid();
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
                    string subDir = prevpsrh.TemporaryNo + "-" + prevpsrh.TemporaryDate.ToString("yyyyMMddhhmmss");
                    dgv.Enabled = false;
                    ////DocumentStorageDB.createFileFromDB(docID, subDir, fileName);
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

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredPSRHeader(listOption);
        }

        private void btnApproved_Click(object sender, EventArgs e)
        {
            //if (getuserPrivilegeStatus() == 2)
            //{
            //    listOption = 6; //viewer
            //}
            //else
            //{
            //    listOption = 3;
            //}
            //ListFilteredPSRHeader(listOption);
            listOption = 2;
            ListFilteredPSRHeader(listOption);
        }
        private void removeControlsFromPnlLvPanel()
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
        private void btnSelectSMRNHeaderNo_Click(object sender, EventArgs e)
        {
            //btnSelectSMRNHeaderNo.Enabled = false;
            //removeControlsFromPnlLvPanel();
            int RType = 0;
            try
            {
                RType = Convert.ToInt32(cmbReportType.SelectedItem.ToString().Trim().Substring(0, cmbReportType.SelectedItem.ToString().Trim().IndexOf('-')));
            }
            catch (Exception)
            {
                MessageBox.Show("Select Report Type");
                btnSelectSMRNHeaderNo.Enabled = true;
                return;
            }
            //pnllv = new Panel();
            //pnllv.BorderStyle = BorderStyle.FixedSingle;

            //pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);

            lv = SMRNHeaderDB.getSMRNHeaderNoListView(RType);
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Click);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
            //pnlAddEdit.Controls.Add(pnllv);
            //pnllv.BringToFront();
            //pnllv.Visible = true;
        }
        private void lvOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (!checkLVItemChecked("Item"))
                {
                    return;
                }
                
                string trlist;
                if (lv.CheckedItems.Count > 0)
                {
                    trlist = "";
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            txtSMRNHeaderNo.Text = itemRow.SubItems[1].Text;
                            dtSMRNHeaderDate.Value = Convert.ToDateTime(itemRow.SubItems[2].Text);
                            txtSMRNNo.Text = itemRow.SubItems[3].Text;
                            dtSMRNDate.Value = Convert.ToDateTime(itemRow.SubItems[4].Text);
                            txtJobIDNo.Text = "";
                        }
                    }
                }
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_Click(object sender, EventArgs e)
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
        //private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
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

        private void btnSelectJobID_Click(object sender, EventArgs e)
        {

            //btnSelectJobID.Enabled = false;
            if (txtSMRNHeaderNo.Text.Length == 0)
            {
                MessageBox.Show("Select SMRN Temporary No");
                //btnSelectJobID.Enabled = true;
                return;
            }
            if (cmbReportType.SelectedIndex == -1)
            {
                MessageBox.Show("Select Report Type");
                //btnSelectJobID.Enabled = true;
                return;
            }
            //removeControlsFromPnlLvPanel();
            //pnllv = new Panel();
            //pnllv.BorderStyle = BorderStyle.FixedSingle;

            //pnllv.Bounds = new Rectangle(new Point(100, 100), new Size(600, 300));
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            int tempNo = Convert.ToInt32(txtSMRNHeaderNo.Text);
            DateTime tempDate = dtSMRNHeaderDate.Value;
            int ReportType = Convert.ToInt32(cmbReportType.SelectedItem.ToString().Trim().Substring(0, cmbReportType.SelectedItem.ToString().Trim().IndexOf('-')));
            lv = SMRNHeaderDB.getSMRNJOBIDNoListView(tempNo, tempDate, ReportType);
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView2_ItemChecked);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Click1);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click1);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
            //pnlAddEdit.Controls.Add(pnllv);
            //pnllv.BringToFront();
            //pnllv.Visible = true;
        }
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                if (!checkLVItemChecked("Item"))
                {
                    return;
                }
                
                string trlist;
                if (lv.CheckedItems.Count > 0)
                {
                    trlist = "";
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            //btnSelectJobID.Enabled = true;
                            txtJobIDNo.Text = itemRow.SubItems[1].Text;
                        }
                    }
                }
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
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
        //-----
        //private void listView2_ItemChecked(object sender, ItemCheckedEventArgs e)
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

        private void btnListDocuments_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevpsrh.TemporaryNo + "-" + prevpsrh.TemporaryDate.ToString("yyyyMMddhhmmss"));
                pnlPDFViewer.Controls.Add(dgvDocumentList);
                dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            removeControlsFromPnlLvPanel();
        }

        private void ProductServiceReportHeader_Enter(object sender, EventArgs e)
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

