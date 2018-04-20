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
using CSLERP.FileManager;
using CSLERP.PrintForms;

namespace CSLERP
{
    public partial class JournalVoucher : System.Windows.Forms.Form
    {
        string docID = "JOURNALVOUCHER";
        string forwarderList = "";
        string approverList = "";
        string userString = "";
        Timer filterTimer = new Timer();
        string userCommentStatusString = "";
        Form dtpForm = new Form();
        Boolean AddRowClick = false;
        DataGridView AccCodeGrd = new DataGridView();
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Boolean userIsACommenter = false;
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        JournalVoucherHeader prevjvh;
        ListView lvCopy = new ListView();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        TextBox txtSearch = new TextBox();
        Panel pnlModel = new Panel();
        Form frmPopup = new Form();
        DataGridView payeeCodeGrd = new DataGridView();
        public JournalVoucher()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }
        private void JournalVoucher_Load(object sender, EventArgs e)
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
            grdPRDetail.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdPRDetail.EnableHeadersVisualStyles = false;
            ListFilteredJournalVoucherHeader(listOption);
            //tabControl1.SelectedIndexChanged += new EventHandler(TabControl1_SelectedIndexChanged);
        }
        private void ListFilteredJournalVoucherHeader(int option)
        {
            try
            {
                grdList.Rows.Clear();
                JournalVoucherDB JvDb = new JournalVoucherDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);

                List<JournalVoucherHeader> JVHList = JvDb.getFilteredJournalHeader(userString, option, userCommentStatusString);
                if (option == 1)
                    lblActionHeader.Text = "List of Action Pending Documents";
                else if (option == 2)
                    lblActionHeader.Text = "List of In-Process Documents";
                else if (option == 3 || option == 6)
                    lblActionHeader.Text = "List of Approved Documents";
                foreach (JournalVoucherHeader jvh in JVHList)
                {
                    if (option == 1)
                    {
                        if (jvh.DocumentStatus == 99)
                            continue;
                    }
                    else
                    {

                    }
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = jvh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = jvh.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = jvh.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["JournalNo"].Value = jvh.JournalNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["JournalDate"].Value = jvh.JournalDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["Narration"].Value = jvh.Narration;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = jvh.CreateUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["FrowardUser"].Value = jvh.ForwardUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["ApproveUser"].Value = jvh.ApproveUser;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = jvh.CreatorName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarder"].Value = jvh.ForwarderName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Approver"].Value = jvh.ApproverName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = jvh.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = jvh.status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = jvh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = jvh.CreateUser;
                    //grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatusNo"].Value = prh.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["CmtStatus"].Value = jvh.CommentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["Comments"].Value = jvh.Comments;
                    grdList.Rows[grdList.RowCount - 1].Cells["Forwarders"].Value = jvh.ForwarderList;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Journal Voucher Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;

            grdList.Columns["Creator"].Visible = true;
            //grdList.Columns["Forwarder"].Visible = true;
            grdList.Columns["Approver"].Visible = true;
            if (listOption == 1 || listOption == 2)
            {
                //grdList.Columns["gTemporaryNo"].Visible = true;
                //grdList.Columns["gTemporaryDate"].Visible = true;
                grdList.Columns["Narration"].Width = 200;
            }
            else
            {
                //grdList.Columns["gTemporaryNo"].Visible = false;
                //grdList.Columns["gTemporaryDate"].Visible = false;
                grdList.Columns["Narration"].Width = 300;
            }

        }

        //called only in the beginning
        private void initVariables()
        {

            if (getuserPrivilegeStatus() == 1)
            {
                //user is only a viewer
                listOption = 6;
            }
            docID = Main.currentDocument;
            dtTempDate.Format = DateTimePickerFormat.Custom;
            dtTempDate.CustomFormat = "dd-MM-yyyy";
            dtJournalDate.Format = DateTimePickerFormat.Custom;
            dtJournalDate.CustomFormat = "dd-MM-yyyy";
            btnForward.Visible = false;
            btnApprove.Visible = false;
            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            txtJournalNo.TabIndex = 1;
            dtJournalDate.TabIndex = 2;
            //dtDCDate.TabIndex = 3;
            grdPRDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
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

        //called when new,cancel buttons are clicked.
        //called at the end of event processing for forward, approve,reverse and save
        public void clearData()
        {
            try
            {
                //clear all grid views
                grdPRDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                //----------clear temperory panels
                clearTabPageControls();
                AddRowClick = false;
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                pnllv.Visible = false;
                //SLType = "";
                txtTemporarryNo.Text = "";
                commentStatus = "";

                txtJournalNo.Text = "";
                dtTempDate.Value = DateTime.Parse("1900-01-01");
                dtJournalDate.Value = DateTime.Now;
                txtTotalAmountDebit.Text = "";
                txtnarration.Text = "";
                prevjvh = new JournalVoucherHeader();

                //removeControlsFromCommenterPanel();
                //removeControlsFromForwarderPanel();
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
                AddRowClick = false;
                pnlAddEdit.Visible = true;
                setButtonVisibility("btnNew");
            }
            catch (Exception)
            {

            }
        }
        private void btnAddLine_Click_2(object sender, EventArgs e)
        {
            try
            {
                if (grdPRDetail.Rows.Count > 0)
                {
                    if (!verifyAndReworkVoucherDetailGridRows())
                    {
                        return;
                    }
                }
                AddRowClick = true;
                AddPRDetailRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private Boolean AddPRDetailRow()
        {
            Boolean status = true;
            try
            {

                grdPRDetail.Rows.Add();
                int kount = grdPRDetail.RowCount;
                grdPRDetail.Rows[kount - 1].Cells["LineNo"].Value = kount;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["AccountCode"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["AccountName"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["PartyCode"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["PartyName"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["gSLType"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["AmountDebit"].Value = Convert.ToDecimal(0);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["AmountCredit"].Value = Convert.ToDecimal(0);
                if (AddRowClick)
                {
                    grdPRDetail.FirstDisplayedScrollingRowIndex = grdPRDetail.RowCount - 1;
                    grdPRDetail.CurrentCell = grdPRDetail.Rows[kount - 1].Cells[0];
                }
                grdPRDetail.FirstDisplayedScrollingColumnIndex = 0;
            }

            catch (Exception ex)
            {
                MessageBox.Show("AddPRDetailRow() : Error");
            }

            return status;
        }
        private Boolean verifyAndReworkVoucherDetailGridRows()
        {
            Boolean status = true;

            try
            {
                if (grdPRDetail.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in Journal details");
                    return false;
                }
                if (!validateItems())
                {
                    MessageBox.Show("Validation failed");
                    return false;
                }
                int debitCount = 0, creditCount = 0;
                Decimal amntDebit = 0;
                Decimal amntCredit = 0;
                for (int i = 0; i < grdPRDetail.Rows.Count; i++)
                {
                    if ((grdPRDetail.Rows[i].Cells["AmountDebit"].Value == null) ||
                                (grdPRDetail.Rows[i].Cells["AmountDebit"].Value.ToString().Trim().Length == 0))
                    {
                        grdPRDetail.Rows[i].Cells["AmountCredit"].Value = Convert.ToDecimal(0);
                    }
                    if ((grdPRDetail.Rows[i].Cells["AmountCredit"].Value == null) ||
                                (grdPRDetail.Rows[i].Cells["AmountCredit"].Value.ToString().Trim().Length == 0))
                    {
                        grdPRDetail.Rows[i].Cells["AmountCredit"].Value = Convert.ToDecimal(0);
                    }
                    if (((Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountDebit"].Value) != 0) &&
                       (Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountCredit"].Value) != 0)) ||
                           ((Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountDebit"].Value) == 0) &&
                       (Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountCredit"].Value) == 0)))

                    {
                        MessageBox.Show("Enter either debit or credit value.Row:" + (i + 1));
                        return false;
                    }
                    grdPRDetail.Rows[i].Cells["LineNo"].Value = (i + 1);
                    if ((grdPRDetail.Rows[i].Cells["AccountCode"].Value == null) ||
                        (grdPRDetail.Rows[i].Cells["AccountCode"].Value.ToString().Trim().Length == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }
                    if ((Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountDebit"].Value) != 0))
                        debitCount++;
                    else
                        creditCount++;
                    amntDebit = amntDebit + Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountDebit"].Value);
                    amntCredit = amntCredit + Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountCredit"].Value);
                }
                if ((grdPRDetail.Rows.Count != 1) && (debitCount > 1 && creditCount > 1))
                {
                    MessageBox.Show("Check the debit and Credit columns in detail.");
                    return false;
                }
                txtTotalAmountCredit.Text = amntCredit.ToString();
                txtTotalAmountDebit.Text = amntDebit.ToString();
            }
            catch (Exception ex)
            {

                return false;
            }
            return status;
        }
        private Boolean validateItems()
        {
            Boolean status = true;
            try
            {
                for (int i = 0; i < grdPRDetail.Rows.Count - 1; i++)
                {
                    for (int j = i + 1; j < grdPRDetail.Rows.Count; j++)
                    {

                        if (grdPRDetail.Rows[i].Cells["AccountCode"].Value.ToString() == grdPRDetail.Rows[j].Cells["AccountCode"].Value.ToString())
                        {
                            MessageBox.Show("AccountCode code duplicated.");
                            return false;
                        }
                    }

                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        private List<JournalVoucherDetail> getVoucherDetails(JournalVoucherHeader jvh)
        {
            JournalVoucherDetail jvd = new JournalVoucherDetail();

            List<JournalVoucherDetail> JVDetails = new List<JournalVoucherDetail>();
            for (int i = 0; i < grdPRDetail.Rows.Count; i++)
            {
                try
                {
                    jvd = new JournalVoucherDetail();
                    jvd.DocumentID = jvh.DocumentID;
                    jvd.TemporaryNo = jvh.TemporaryNo;
                    jvd.TemporaryDate = jvh.TemporaryDate;
                    jvd.AccountCode = grdPRDetail.Rows[i].Cells["AccountCode"].Value.ToString();
                    jvd.SLType = grdPRDetail.Rows[i].Cells["gSLType"].Value.ToString();
                    jvd.SLCode = grdPRDetail.Rows[i].Cells["PartyCode"].Value.ToString();
                    if (grdPRDetail.Rows[i].Cells["AmountDebit"].Value != null)
                        jvd.AmountDebit = Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountDebit"].Value.ToString().Trim());
                    else
                        jvd.AmountDebit = 0;
                    if (grdPRDetail.Rows[i].Cells["AmountCredit"].Value != null)
                        jvd.AmountCredit = Convert.ToDecimal(grdPRDetail.Rows[i].Cells["AmountCredit"].Value.ToString().Trim());
                    else
                        jvd.AmountCredit = 0;
                    JVDetails.Add(jvd);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("createAndUpdatePRDetails() : Error creating Journal Details");
                    //status = false;
                }
            }
            return JVDetails;
        }
        //private Boolean createAndUpdateJournalDetails(JournalVoucherHeader jvh)
        //{
        //    Boolean status = true;
        //    try
        //    {
        //        JournalVoucherDB jvDB = new JournalVoucherDB();
        //        List<JournalVoucherDetail> JVDetails = getVoucherDetails(jvh);
        //        if (!jvDB.updateJournalVoucherDetail(JVDetails, jvh))
        //        {
        //            MessageBox.Show("createAndUpdatePRDetails() : Failed to update Journal Voucher Details. Please check the values");
        //            status = false;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        MessageBox.Show("createAndUpdatePRDetails() : Error updating Journal Voucher Details");
        //        status = false;
        //    }
        //    return status;
        //}

        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            ListFilteredJournalVoucherHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            ListFilteredJournalVoucherHeader(listOption);
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

            ListFilteredJournalVoucherHeader(listOption);
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {

                JournalVoucherDB jvhDB = new JournalVoucherDB();
                JournalVoucherHeader jvh = new JournalVoucherHeader();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;
                if (!verifyAndReworkVoucherDetailGridRows())
                {
                    MessageBox.Show("Validation Failed in Journal voucher Detail");
                    return;
                }
                jvh.DocumentID = docID;
                jvh.JournalDate = dtJournalDate.Value.Date;
                jvh.Narration = txtnarration.Text.Replace("'", "''");
                jvh.Comments = docCmtrDB.DGVtoString(dgvComments).Replace("'", "''");
                jvh.ForwarderList = prevjvh.ForwarderList;

                if (!jvhDB.validateVoucherHeader(jvh))
                {
                    MessageBox.Show("Validation failed");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    //jvh.TemporaryNo = DocumentNumberDB.getNewNumber(docID, 1);
                    jvh.DocumentStatus = 1; //created
                    jvh.TemporaryDate = UpdateTable.getSQLDateTime();
                }
                else
                {
                    jvh.TemporaryNo = Convert.ToInt32(txtTemporarryNo.Text);
                    jvh.TemporaryDate = prevjvh.TemporaryDate;
                    jvh.DocumentStatus = prevjvh.DocumentStatus;

                }

                if (jvhDB.validateVoucherHeader(jvh))
                {
                    //--create comment status string
                    docCmtrDB = new DocCommenterDB();
                    if (userIsACommenter)
                    {
                        //if the user is only a commenter and ticked the comment as final; then update his comment string as final
                        //and update the comment status
                        if (txtComments.Text != null && txtComments.Text.Trim().Length != 0)
                        {
                            docCmtrDB = new DocCommenterDB();
                            jvh.CommentStatus = docCmtrDB.createCommentStatusString(prevjvh.CommentStatus, Login.userLoggedIn);
                        }
                        else
                        {
                            jvh.CommentStatus = prevjvh.CommentStatus;
                        }
                    }
                    else
                    {
                        if (commentStatus.Trim().Length > 0)
                        {
                            //clicked the Get Commenter button
                            jvh.CommentStatus = docCmtrDB.createCommenterList(lvCmtr, dtCmtStatus);
                        }
                        else
                        {
                            jvh.CommentStatus = prevjvh.CommentStatus;
                        }
                    }
                    //----------
                    int tmpStatus = 1;
                    if (txtComments.Text.Trim().Length > 0)
                    {
                        jvh.Comments = docCmtrDB.processNewComment(dgvComments, txtComments.Text, Login.userLoggedIn, Login.userLoggedInName, tmpStatus).Replace("'", "''");
                    }
                    if (Convert.ToDecimal(txtTotalAmountCredit.Text) != Convert.ToDecimal(txtTotalAmountDebit.Text))
                    {
                        MessageBox.Show("Debit and Credit total should be equal.");
                        return;
                    }
                    List<JournalVoucherDetail> JVDetails = getVoucherDetails(jvh);
                    if (btnText.Equals("Update"))
                    {
                        if (jvhDB.updateJournalHeaderAndDetail(jvh, prevjvh,JVDetails))
                        {
                            MessageBox.Show("Journal Voucher Details updated");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredJournalVoucherHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to update Journal Voucher");
                        }
                    }

                    else if (btnText.Equals("Save"))
                    {
                        if (jvhDB.InsertJournalHeaderAndDetail(jvh, JVDetails))
                        {
                            MessageBox.Show("Journal Voucher Added");
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredJournalVoucherHeader(listOption);
                        }
                        else
                        {
                            status = false;
                        }
                        if (!status)
                        {
                            MessageBox.Show("Failed to Insert Journal  Header");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Journal Details Validation failed");
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("btnSave_Click_1() : Error");
                status = false;
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }
        private void btnCalculateax_Click_1(object sender, EventArgs e)
        {
            verifyAndReworkVoucherDetailGridRows();
        }

        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit") || columnName.Equals("Approve") || columnName.Equals("LoadDocument") ||
                    columnName.Equals("View"))
                {

                    clearData();
                    setButtonVisibility(columnName);
                    prevjvh = new JournalVoucherHeader();

                    AddRowClick = false;
                    prevjvh.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();
                    prevjvh.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    prevjvh.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    prevjvh.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    if (prevjvh.CommentStatus.IndexOf(userCommentStatusString) >= 0)
                    {
                        // only for commeting and viwing documents
                        userIsACommenter = true;
                        setButtonVisibility("Commenter");
                    }
                    prevjvh.Comments = JournalVoucherDB.getUserComments(prevjvh.DocumentID, prevjvh.TemporaryNo, prevjvh.TemporaryDate);
                    docID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();

                    JournalVoucherDB jvdb = new JournalVoucherDB();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    DataGridViewRow row = grdList.Rows[rowID];

                    prevjvh.JournalNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["JournalNo"].Value.ToString());
                    prevjvh.JournalDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["JournalDate"].Value.ToString());
                    //--------Load Documents
                    if (columnName.Equals("LoadDocument"))
                    {
                        string hdrString = "Document Temp No:" + prevjvh.TemporaryNo + "\n" +
                            "Document Temp Date:" + prevjvh.TemporaryDate.ToString("dd-MM-yyyy") + "\n" +
                            "Journal No:" + prevjvh.JournalNo + "\n" +
                            "Journal Date:" + prevjvh.JournalDate.ToString("dd-MM-yyyy");
                        string dicDir = Main.documentDirectory + "\\" + docID;
                        string subDir = prevjvh.TemporaryNo + "-" + prevjvh.TemporaryDate.ToString("yyyyMMddhhmmss");
                        FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, docID, hdrString);
                        load.ShowDialog();
                        this.RemoveOwnedForm(load);
                        btnCancel_Click_1(null, null);
                        return;
                    }
                    //--------
                    prevjvh.Narration = grdList.Rows[e.RowIndex].Cells["Narration"].Value.ToString();
                    prevjvh.status = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gStatus"].Value.ToString());
                    prevjvh.DocumentStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gDocumentStatus"].Value.ToString());
                    prevjvh.CreateTime = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gCreateTime"].Value.ToString());
                    prevjvh.CreateUser = grdList.Rows[e.RowIndex].Cells["gCreateUser"].Value.ToString();
                    prevjvh.CreatorName = grdList.Rows[e.RowIndex].Cells["Creator"].Value.ToString();
                    prevjvh.ForwarderName = grdList.Rows[e.RowIndex].Cells["Forwarder"].Value.ToString();
                    prevjvh.ApproverName = grdList.Rows[e.RowIndex].Cells["Approver"].Value.ToString();
                    prevjvh.ForwarderList = grdList.Rows[e.RowIndex].Cells["Forwarders"].Value.ToString();
                    //--comments
                   /// chkCommentStatus.Checked = false;
                    docCmtrDB = new DocCommenterDB();
                    pnlComments.Controls.Remove(dgvComments);
                    prevjvh.CommentStatus = grdList.Rows[e.RowIndex].Cells["CmtStatus"].Value.ToString();

                    dtCmtStatus = docCmtrDB.splitCommentStatus(prevjvh.CommentStatus);
                    dgvComments = new DataGridView();
                    dgvComments = docCmtrDB.createCommentGridview(prevjvh.Comments);
                    pnlComments.Controls.Add(dgvComments);
                    txtComments.Text = docCmtrDB.getLastUnauthorizedComment(dgvComments, Login.userLoggedIn);
                    dgvComments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvComments_CellDoubleClick);

                    //---
                    txtTemporarryNo.Text = prevjvh.TemporaryNo.ToString();
                    dtTempDate.Value = prevjvh.TemporaryDate;
                    txtJournalNo.Text = prevjvh.JournalNo.ToString();
                    try
                    {
                        dtJournalDate.Value = prevjvh.JournalDate;
                    }
                    catch (Exception)
                    {
                        dtJournalDate.Value = DateTime.Parse("1900-01-01");
                    }
                    txtnarration.Text = prevjvh.Narration.ToString();
                    decimal totDebit = 0;
                    decimal totCredit = 0;
                    List<JournalVoucherDetail> JVDetail = JournalVoucherDB.getJournalVoucherDetail(prevjvh);
                    grdPRDetail.Rows.Clear();
                    int i = 0;
                    try
                    {
                        foreach (JournalVoucherDetail jvd in JVDetail)
                        {
                            AddPRDetailRow();
                            grdPRDetail.Rows[i].Cells["AccountCode"].Value = jvd.AccountCode;
                            grdPRDetail.Rows[i].Cells["AccountName"].Value = jvd.AccountName;
                            grdPRDetail.Rows[i].Cells["PartyCode"].Value = jvd.SLCode;
                            grdPRDetail.Rows[i].Cells["PartyName"].Value = jvd.SLName;
                            grdPRDetail.Rows[i].Cells["gSLType"].Value = jvd.SLType;
                            grdPRDetail.Rows[i].Cells["AmountDebit"].Value = jvd.AmountDebit;
                            grdPRDetail.Rows[i].Cells["AmountCredit"].Value = jvd.AmountCredit;
                            totDebit = totDebit + jvd.AmountDebit;
                            totCredit = totCredit + jvd.AmountCredit;
                            i++;
                        }
                        txtTotalAmountCredit.Text = totCredit.ToString();
                        txtTotalAmountDebit.Text = totDebit.ToString();
                    }
                    catch (Exception ex)
                    {
                    }

                }
                else
                {
                    return;
                }
                btnSave.Text = "Update";
                pnlList.Visible = false;
                pnlAddEdit.Visible = true;
                tabControl1.SelectedTab = tabVHeader;
                tabControl1.Visible = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show("error");
            }
        }
        private void btnClearEntries_Click_1(object sender, EventArgs e)
        {
            try
            {
                for (int i = grdPRDetail.Rows.Count - 1; i >= 0; i--)
                {
                    grdPRDetail.Rows.RemoveAt(i);
                }
            }
            catch (Exception)
            {
            }
            MessageBox.Show("item cleared.");
        }

        private void btnForward_Click_1(object sender, EventArgs e)
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

        private void btnApprove_Click_1(object sender, EventArgs e)
        {
            try
            {
                JournalVoucherDB jvDB = new JournalVoucherDB();
                FinancialLimitDB flDB = new FinancialLimitDB();
                if (!flDB.checkEmployeeFinancialLimit(docID, Login.empLoggedIn, Convert.ToDouble(txtTotalAmountDebit.Text), 0))
                {
                    MessageBox.Show("No financial power for approving this document");
                    return;
                }
                DialogResult dialog = MessageBox.Show("Are you sure to Approve the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    prevjvh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevjvh.CommentStatus);
                    prevjvh.JournalNo = DocumentNumberDB.getNewNumber(docID, 2);
                    if (verifyAndReworkVoucherDetailGridRows())
                    {
                        if (jvDB.ApproveJournalVoucherHeader(prevjvh))
                        {
                            MessageBox.Show("Journal Voucher Document Approved");
                            if (!updateDashBoard(prevjvh, 2))
                            {
                                MessageBox.Show("DashBoard Fail to update");
                            }
                            closeAllPanels();
                            listOption = 1;
                            ListFilteredJournalVoucherHeader(listOption);
                            setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                        }
                        else
                            MessageBox.Show("Unable to approve");
                    }
                }
            }
            catch (Exception)
            {
            }

        }
        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //removeControlsFromCommenterPanel();
                //removeControlsFromForwarderPanel();
                //removeControlsFromLVPanel();
            }
            catch (Exception es)
            {
            }
        }
        private void pnlList_Paint(object sender, PaintEventArgs e)
        {

        }
        //-----
        //comment handling procedurs and functions
        //-----
        private void btnGetComments_Click(object sender, EventArgs e)
        {
            //removeControlsFromCommenterPanel();

            //lvCmtr = new ListView();
            //lvCmtr.Clear();
            //pnlCmtr.BorderStyle = BorderStyle.FixedSingle;
            //pnlCmtr.Bounds = new Rectangle(new Point(100, 10), new Size(700, 300));

            docCmtrDB = new DocCommenterDB();
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            lvCmtr = docCmtrDB.commenterLV(docID);
            docCmtrDB.verifyCommenterList(lvCmtr, dtCmtStatus);
            lvCmtr.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            frmPopup.Controls.Add(lvCmtr);

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
            ////lvCancel.Visible = true;
            frmPopup.ShowDialog();

            //pnlCmtr.BringToFront();
            //pnlCmtr.Visible = true;
            //pnlComments.Controls.Add(pnlCmtr);
            //pnlComments.BringToFront();
            //pnlCmtr.BringToFront();

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
                frmPopup.Close();
                //rmPopup.Dispose();
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
                //frmPopup.Dispose();
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
                            JournalVoucherDB jvhDB = new JournalVoucherDB();
                            prevjvh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevjvh.CommentStatus);
                            prevjvh.ForwardUser = approverUID;
                            prevjvh.ForwarderList = prevjvh.ForwarderList + approverUName + Main.delimiter1 +
                                approverUID + Main.delimiter1 + Main.delimiter2;
                            if (jvhDB.forwardJournalVoucherHeader(prevjvh))
                            {
                                frmPopup.Close();
                                frmPopup.Dispose();
                                MessageBox.Show("Document Forwarded");
                                if (!updateDashBoard(prevjvh, 1))
                                {
                                    MessageBox.Show("DashBoard Fail to update");
                                }
                                closeAllPanels();
                                listOption = 1;
                                ListFilteredJournalVoucherHeader(listOption);
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
        private Boolean updateDashBoard(JournalVoucherHeader jvh, int stat)
        {
            Boolean status = true;
            try
            {
                dashboardalarm dsb = new dashboardalarm();
                DashboardDB ddsDB = new DashboardDB();
                dsb.DocumentID = jvh.DocumentID;
                dsb.TemporaryNo = jvh.TemporaryNo;
                dsb.TemporaryDate = jvh.TemporaryDate;
                dsb.DocumentNo = jvh.JournalNo;
                dsb.DocumentDate = jvh.JournalDate;
                dsb.FromUser = Login.userLoggedIn;
                if (stat == 1)
                {
                    dsb.ActivityType = 2;
                    dsb.ToUser = jvh.ForwardUser;
                    if (!ddsDB.insertDashboardAlarm(dsb))
                    {
                        MessageBox.Show("DashBoard Fail to update");
                        status = false;
                    }
                }
                else if (stat == 2)
                {
                    dsb.ActivityType = 3;
                    List<documentreceiver> docList = DocumentReceiverDB.getDocumentWiseReceiver(prevjvh.DocumentID);
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
                    string s = prevjvh.ForwarderList;
                    string reverseStr = getReverseString(prevjvh.ForwarderList);
                    //do forward activities
                    prevjvh.CommentStatus = DocCommenterDB.removeUnapprovedCommentStatus(prevjvh.CommentStatus);
                    JournalVoucherDB jvhDB = new JournalVoucherDB();
                    if (reverseStr.Trim().Length > 0)
                    {
                        int ind = reverseStr.IndexOf("!@#");
                        prevjvh.ForwarderList = reverseStr.Substring(0, ind);
                        prevjvh.ForwardUser = reverseStr.Substring(ind + 3);
                        prevjvh.DocumentStatus = prevjvh.DocumentStatus - 1;
                    }
                    else
                    {
                        prevjvh.ForwarderList = "";
                        prevjvh.ForwardUser = "";
                        prevjvh.DocumentStatus = 1;
                    }
                    if (jvhDB.reverseJournalVoucherHeader(prevjvh))
                    {
                        MessageBox.Show("JOurnal Voucher Document Reversed");
                        closeAllPanels();
                        listOption = 1;
                        ListFilteredJournalVoucherHeader(listOption);
                        setButtonVisibility("btnEditPanel"); //activites are same for cance, forward,approce and reverse
                    }
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
        private void removeControlsFromLVPanel()
        {
            try
            {
                //foreach (Control p in pnllv.Controls)
                //{
                //    p.Dispose();
                pnllv.Controls.Clear();
                Control nc = pnllv.Parent;
                nc.Controls.Remove(pnllv);
            }
            catch (Exception ex)
            {
            }
        }
        private void btnListDocuments_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, prevjvh.TemporaryNo + "-" + prevjvh.TemporaryDate.ToString("yyyyMMddhhmmss"));
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
                    ////string docDir = Main.documentDirectory + "\\" + docID;
                    string subDir = prevjvh.TemporaryNo + "-" + prevjvh.TemporaryDate.ToString("yyyyMMddhhmmss");
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
                btnGetComments.Visible = false;
                //chkCommentStatus.Visible = false;
                txtComments.Visible = false;
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
                    pnlBottomButtons.Visible = false;
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    pnlPDFViewer.Visible = true;
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    btnGetComments.Visible = false; //earlier Edit enabled this button
                    //chkCommentStatus.Visible = true;
                    txtComments.Visible = true;
                }
                else if (btnName == "btnNew")
                {
                    btnNew.Visible = false; //added 24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = false;
                    tabComments.Enabled = false;
                    tabPDFViewer.Enabled = false;
                    tabControl1.SelectedTab = tabVHeader;
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
                    btnGetComments.Visible = true;
                    enableTabPages();
                    pnlPDFViewer.Visible = true;
                    ///chkCommentStatus.Visible = true;
                    txtComments.Visible = true;
                    tabControl1.SelectedTab = tabVHeader;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnForward.Visible = true;
                    btnApprove.Visible = true;
                    btnReverse.Visible = true;
                    // btnQC.Visible = true;
                    disableTabPages();

                    tabControl1.SelectedTab = tabVHeader;
                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    disableTabPages();
                    tabComments.Enabled = true;
                    tabPDFViewer.Enabled = true;
                    pnlPDFViewer.Visible = true;
                    tabControl1.SelectedTab = tabVHeader;
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
                removeControlsFromLVPanel();
                removeControlsFromForwarderPanel();
                dgvComments.Rows.Clear();
                ////chkCommentStatus.Checked = false;
                txtComments.Text = "";
                grdPRDetail.Rows.Clear();
            }
            catch (Exception ex)
            {
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            frmPopup.Controls.Remove(lvCopy);
            addItems();
        }
        private void addItems()
        {
            lvCopy = new ListView();
            lvCopy.View = System.Windows.Forms.View.Details;
            lvCopy.LabelEdit = true;
            lvCopy.AllowColumnReorder = true;
            lvCopy.CheckBoxes = true;
            lvCopy.FullRowSelect = true;
            lvCopy.GridLines = true;
            lvCopy.Sorting = System.Windows.Forms.SortOrder.Ascending;
            lvCopy.Columns.Add("Select", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Id", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Name", -2, HorizontalAlignment.Left);
            lvCopy.Sorting = System.Windows.Forms.SortOrder.None;
            lvCopy.ColumnClick += new ColumnClickEventHandler(LvColumnClick); ;
            //this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
            //lvCopy.Location = new Point(13, 9);
            //lvCopy.Size = new Size(440, 199);
            lvCopy.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            lvCopy.Items.Clear();
            foreach (ListViewItem row in lv.Items)
            {
                string x = row.SubItems[0].Text;
                string no = row.SubItems[1].Text;
                string ch = row.SubItems[2].Text;
                if (ch.ToLower().StartsWith(txtSearch.Text))
                {
                    ListViewItem item = new ListViewItem();
                    item.SubItems.Add(no);
                    item.SubItems.Add(ch);
                    item.Checked = false;
                    lvCopy.Items.Add(item);
                }
            }
            lv.Visible = false;
            lvCopy.Visible = true;
            frmPopup.Controls.Add(lvCopy);
        }
        //private void CopylistView2_ItemChecked(object sender, ItemCheckedEventArgs e)
        //{
        //    try
        //    {
        //        if (lvCopy.CheckedIndices.Count > 1)
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
        private void txtPONo_TextChanged(object sender, EventArgs e)
        {
        }
        private void cmbCustomer_SelectedValueChanged(object sender, EventArgs e)
        {
        }
        private void tabPRHeader_Click(object sender, EventArgs e)
        {

        }
        private void grdPRDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdPRDetail.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Delete"))
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            grdPRDetail.Rows.RemoveAt(e.RowIndex);
                        }
                        verifyAndReworkVoucherDetailGridRows();
                    }
                    if (columnName.Equals("SelAcDebit"))
                    {
                        showAccountCodeDataGridView();
                    }
                    if (columnName.Equals("SelParty"))
                    {
                        showPayeeCodeDataGridView();
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
        private void showAccountCodeDataGridView()
        {
            try
            {
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.CadetBlue;

                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(550, 370);

                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(200, 5);
                lblSearch.AutoSize = true;
                lblSearch.Text = "Search by Name";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(320, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInAccCodeGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                AccCodeGrd = AccountCodeDB.getGridViewForAccountCode();
                AccCodeGrd.Bounds = new Rectangle(new Point(0, 27), new Size(550, 300));
                frmPopup.Controls.Add(AccCodeGrd);
                AccCodeGrd.Columns["AccountName"].Width = 380;

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grdAccCOdeOK_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 335);
                lvCancel.Click += new System.EventHandler(this.grdAccCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdAccCOdeOK_Click1(object sender, EventArgs e)
        {
            string users = "";
            try
            {
                var checkedRows = from DataGridViewRow r in AccCodeGrd.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("Select one AccCode");
                    return;
                }

                foreach (var row in checkedRows)
                {
                    grdPRDetail.CurrentRow.Cells["AccountCode"].Value = row.Cells["AccountCode"].Value.ToString();
                    grdPRDetail.CurrentRow.Cells["AccountName"].Value = row.Cells["AccountName"].Value.ToString();
                   
                }

                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void txtSearch_TextChangedInAccCodeGridList(object sender, EventArgs e)
        {
            try
            {
                filterTimer.Enabled = false;
                filterTimer.Stop();
                filterTimer.Tick -= new System.EventHandler(this.handlefilterAccTimerTimeout);
                filterTimer.Tick += new System.EventHandler(this.handlefilterAccTimerTimeout);
                filterTimer.Interval = 500;
                filterTimer.Enabled = true;
                filterTimer.Start();
            }
            catch (Exception ex)
            {

            }
        }
        private void handlefilterAccTimerTimeout(object sender, EventArgs e)
        {
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterAccCodeGridData();
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }
        private void filterAccCodeGridData()
        {
            try
            {
                AccCodeGrd.CurrentCell = null;
                foreach (DataGridViewRow row in AccCodeGrd.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in AccCodeGrd.Rows)
                    {
                        if (!row.Cells["AccountName"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
                        {
                            row.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Searching");
            }
        }
        private void grdAccCancel_Click1(object sender, EventArgs e)
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
        private void showPayeeCodeDataGridView()
        {
            try
            {
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.CadetBlue;

                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(610, 370);

                Label lblSearch = new Label();
                lblSearch.Location = new System.Drawing.Point(200, 5);
                lblSearch.AutoSize = true;
                lblSearch.Text = "Search by Name";
                lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Bold);
                lblSearch.ForeColor = Color.Black;
                frmPopup.Controls.Add(lblSearch);

                txtSearch = new TextBox();
                txtSearch.Size = new Size(220, 18);
                txtSearch.Location = new System.Drawing.Point(320, 3);
                txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9, FontStyle.Regular);
                txtSearch.ForeColor = Color.Black;
                txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChangedInEmpGridList);
                txtSearch.TabIndex = 0;
                txtSearch.Focus();
                frmPopup.Controls.Add(txtSearch);

                payeeCodeGrd = ListViewFIll.getGridViewForPayeeDetails();

                payeeCodeGrd.Bounds = new Rectangle(new Point(0, 27), new Size(610, 300));
                frmPopup.Controls.Add(payeeCodeGrd);
                payeeCodeGrd.Columns["Name"].Width = 340;

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new System.Drawing.Point(20, 335);
                lvOK.Click += new System.EventHandler(this.grdOK_Click1);
                frmPopup.Controls.Add(lvOK);

                Button lvCancel = new Button();
                lvCancel.Text = "CANCEL";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(110, 335);
                lvCancel.Click += new System.EventHandler(this.grdCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void grdOK_Click1(object sender, EventArgs e)
        {
            string users = "";
            try
            {
                var checkedRows = from DataGridViewRow r in payeeCodeGrd.Rows
                                  where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                  select r;
                int selectedRowCount = checkedRows.Count();
                if (selectedRowCount != 1)
                {
                    MessageBox.Show("Select one Payee");
                    return;
                }

                foreach (var row in checkedRows)
                {
                    grdPRDetail.CurrentRow.Cells["PartyCode"].Value = row.Cells["ID"].Value.ToString();
                    grdPRDetail.CurrentRow.Cells["PartyName"].Value = row.Cells["Name"].Value.ToString();
                    grdPRDetail.CurrentRow.Cells["gSLType"].Value = row.Cells["Type"].Value.ToString();

                }

                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception)
            {
            }
        }

        private void txtSearch_TextChangedInEmpGridList(object sender, EventArgs e)
        {
            try
            {
                filterTimer.Enabled = false;
                filterTimer.Stop();
                filterTimer.Tick -= new System.EventHandler(this.handlefilterTimerTimeout);
                filterTimer.Tick += new System.EventHandler(this.handlefilterTimerTimeout);
                filterTimer.Interval = 500;
                filterTimer.Enabled = true;
                filterTimer.Start();
            }
            catch (Exception ex)
            {

            }
        }
        private void handlefilterTimerTimeout(object sender, EventArgs e)
        {
            filterTimer.Enabled = false;
            filterTimer.Stop();
            filterGridData();
            ////MessageBox.Show("handlefilterTimerTimeout() : executed");
        }
        private void filterGridData()
        {
            try
            {
                payeeCodeGrd.CurrentCell = null;
                foreach (DataGridViewRow row in payeeCodeGrd.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {
                    foreach (DataGridViewRow row in payeeCodeGrd.Rows)
                    {
                        if (!row.Cells["Name"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
                        {
                            row.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Searching");
            }
        }
        private void grdCancel_Click1(object sender, EventArgs e)
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
        //private void showCustomerLIstView()
        //{
        //    //removeControlsFromLVPanel();
        //    //pnlAddEdit.Controls.Remove(pnllv);

        //    //pnllv = new Panel();
        //    //pnllv.BorderStyle = BorderStyle.FixedSingle;

        //    //pnllv.Bounds = new Rectangle(new Point(24, 32), new Size(477, 282));

        //    frmPopup = new Form();
        //    frmPopup.StartPosition = FormStartPosition.CenterScreen;
        //    frmPopup.BackColor = Color.CadetBlue;

        //    frmPopup.MaximizeBox = false;
        //    frmPopup.MinimizeBox = false;
        //    frmPopup.ControlBox = false;
        //    frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

        //    frmPopup.Size = new Size(450, 300);
        //    lv = ListViewFIll.getSLListView();
        //    lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
        //   // this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked3);
        //    lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
        //    frmPopup.Controls.Add(lv);

        //    Button lvOK = new Button();
        //    lvOK.BackColor = Color.Tan;
        //    lvOK.Text = "OK";
        //    lvOK.Location = new Point(44, 265);
        //    lvOK.Click += new System.EventHandler(this.lvOK_Click3);
        //    frmPopup.Controls.Add(lvOK);

        //    Button lvCancel = new Button();
        //    lvCancel.Text = "CANCEL";
        //    lvCancel.BackColor = Color.Tan;
        //    lvCancel.Location = new Point(141, 265);
        //    lvCancel.Click += new System.EventHandler(this.lvCancel_Click3);
        //    frmPopup.Controls.Add(lvCancel);

        //    Label lblSearch = new Label();
        //    lblSearch.Text = "Search";
        //    lblSearch.Location = new Point(250, 267);
        //    lblSearch.Size = new Size(45, 15);
        //    frmPopup.Controls.Add(lblSearch);

        //    txtSearch = new TextBox();
        //    txtSearch.Location = new Point(300, 265);
        //    txtSearch.TextChanged += new EventHandler(txtSearch_TextChangedparty);
        //    frmPopup.Controls.Add(txtSearch);

        //    //pnlAddEdit.Controls.Add(pnllv);
        //    //pnllv.BringToFront();
        //    //pnllv.Visible = true;
        //    txtSearch.Focus();
        //    frmPopup.ShowDialog();
        //}
        //private void lvOK_Click3(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (lv.Visible == true)
        //        {
        //            if (!checkLVItemChecked("Party"))
        //            {
        //                return;
        //            }
        //            foreach (ListViewItem itemRow in lv.Items)
        //            {

        //                if (itemRow.Checked)
        //                {
        //                    grdPRDetail.CurrentRow.Cells["PartyCode"].Value = itemRow.SubItems[1].Text;
        //                    grdPRDetail.CurrentRow.Cells["PartyName"].Value = itemRow.SubItems[2].Text;
        //                    grdPRDetail.CurrentRow.Cells["gSLType"].Value = itemRow.SubItems[3].Text;
        //                    itemRow.Checked = false;
        //                    frmPopup.Close();
        //                    frmPopup.Dispose();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (!checkLVCopyItemChecked("Party"))
        //            {
        //                return;
        //            }
        //            foreach (ListViewItem itemRow in lvCopy.Items)
        //            {

        //                if (itemRow.Checked)
        //                {
        //                    grdPRDetail.CurrentRow.Cells["PartyCode"].Value = itemRow.SubItems[1].Text;
        //                    grdPRDetail.CurrentRow.Cells["PartyName"].Value = itemRow.SubItems[2].Text;
        //                    grdPRDetail.CurrentRow.Cells["gSLType"].Value = itemRow.SubItems[3].Text;
        //                    itemRow.Checked = false;
        //                    frmPopup.Close();
        //                    frmPopup.Dispose();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //private void lvCancel_Click3(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        frmPopup.Close();
        //        frmPopup.Dispose();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //private void listView1_ItemChecked3(object sender, ItemCheckedEventArgs e)
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
        private void txtSearch_TextChangedparty(object sender, EventArgs e)
        {
            frmPopup.Controls.Remove(lvCopy);
            addItemsparty();
        }
        private void addItemsparty()
        {
            lvCopy = new ListView();
            lvCopy.View = System.Windows.Forms.View.Details;
            lvCopy.LabelEdit = true;
            lvCopy.AllowColumnReorder = true;
            lvCopy.CheckBoxes = true;
            lvCopy.FullRowSelect = true;
            lvCopy.GridLines = true;
            lvCopy.Sorting = System.Windows.Forms.SortOrder.Ascending;
            lvCopy.Columns.Add("Select", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Id", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Name", -2, HorizontalAlignment.Left);
            lvCopy.Columns.Add("Type", -2, HorizontalAlignment.Left);
            lvCopy.Sorting = System.Windows.Forms.SortOrder.None;
            lvCopy.ColumnClick += new ColumnClickEventHandler(LvColumnClick); ;
            ///this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
            //lvCopy.Location = new Point(13, 9);
            //lvCopy.Size = new Size(440, 199);
            lvCopy.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            lvCopy.Items.Clear();
            foreach (ListViewItem row in lv.Items)
            {
                string x = row.SubItems[0].Text;
                string no = row.SubItems[1].Text;
                string ch = row.SubItems[2].Text;
                string name = row.SubItems[3].Text;
                if (ch.ToLower().StartsWith(txtSearch.Text))
                {
                    ListViewItem item = new ListViewItem();
                    item.SubItems.Add(no);
                    item.SubItems.Add(ch);
                    item.SubItems.Add(name);
                    item.Checked = false;
                    lvCopy.Items.Add(item);
                }
            }
            lv.Visible = false;
            lvCopy.Visible = true;
            frmPopup.Controls.Add(lvCopy);
        }
        //private void showAccountLIstView()
        //{
        //    //removeControlsFromLVPanel();
        //    ////pnlAddEdit.Controls.Remove(pnllv);
        //    //pnllv = new Panel();
        //    //pnllv.BorderStyle = BorderStyle.FixedSingle;

        //    //pnllv.Bounds = new Rectangle(new Point(24, 32), new Size(477, 282));
        //    frmPopup = new Form();
        //    frmPopup.StartPosition = FormStartPosition.CenterScreen;
        //    frmPopup.BackColor = Color.CadetBlue;

        //    frmPopup.MaximizeBox = false;
        //    frmPopup.MinimizeBox = false;
        //    frmPopup.ControlBox = false;
        //    frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

        //    frmPopup.Size = new Size(450, 300);
        //    lv = AccountCodeDB.getAccountCodeListView();
        //    lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
        //    //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked1);
        //    lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
        //    frmPopup.Controls.Add(lv);

        //    Button lvOK = new Button();
        //    lvOK.BackColor = Color.Tan;
        //    lvOK.Text = "OK";
        //    lvOK.Location = new Point(44, 265);
        //    lvOK.Click += new System.EventHandler(this.lvOK_Click2);
        //    frmPopup.Controls.Add(lvOK);

        //    Button lvCancel = new Button();
        //    lvCancel.Text = "CANCEL";
        //    lvCancel.BackColor = Color.Tan;
        //    lvCancel.Location = new Point(141, 265);
        //    lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
        //    frmPopup.Controls.Add(lvCancel);

        //    Label lblSearch = new Label();
        //    lblSearch.Text = "Search";
        //    lblSearch.Location = new Point(250, 267);
        //    lblSearch.Size = new Size(45, 15);
        //    frmPopup.Controls.Add(lblSearch);

        //    txtSearch = new TextBox();
        //    txtSearch.Location = new Point(300, 265);
        //    txtSearch.TextChanged += new EventHandler(txtSearch_TextChanged);
        //    frmPopup.Controls.Add(txtSearch);

        //    //pnlAddEdit.Controls.Add(pnllv);
        //    //pnllv.BringToFront();
        //    //pnllv.Visible = true;
        //    txtSearch.Focus();
        //    frmPopup.ShowDialog();
        //}
        //private void lvOK_Click2(object sender, EventArgs e)
        //{
        //    try
        //    {
               
        //        if (lv.Visible == true)
        //        {
        //            if (!checkLVItemChecked("Account"))
        //            {
        //                return;
        //            }
        //            foreach (ListViewItem itemRow in lv.Items)
        //            {

        //                if (itemRow.Checked)
        //                {
        //                    grdPRDetail.CurrentRow.Cells["AccountCode"].Value = itemRow.SubItems[1].Text;
        //                    grdPRDetail.CurrentRow.Cells["AccountName"].Value = itemRow.SubItems[2].Text;
        //                    itemRow.Checked = false;
        //                    frmPopup.Close();
        //                    frmPopup.Dispose();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (!checkLVCopyItemChecked("Account"))
        //            {
        //                return;
        //            }
        //            foreach (ListViewItem itemRow in lvCopy.Items)
        //            {

        //                if (itemRow.Checked)
        //                {
        //                    grdPRDetail.CurrentRow.Cells["AccountCode"].Value = itemRow.SubItems[1].Text;
        //                    grdPRDetail.CurrentRow.Cells["AccountName"].Value = itemRow.SubItems[2].Text;
        //                    itemRow.Checked = false;
        //                    frmPopup.Close();
        //                    frmPopup.Dispose();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //private void lvCancel_Click2(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        frmPopup.Close();
        //        frmPopup.Dispose();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}
        //private void listView1_ItemChecked1(object sender, ItemCheckedEventArgs e)
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

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                verifyAndReworkVoucherDetailGridRows();
            }
            catch (Exception ex)
            {
            }
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
        private Boolean checkLVCopyItemChecked(string str)
        {
            Boolean status = true;
            try
            {
                if (lvCopy.CheckedIndices.Count > 1)
                {
                    MessageBox.Show("Only one " + str + " allowed");
                    return false;
                }
                if (lvCopy.CheckedItems.Count == 0)
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

        private void JournalVoucher_Enter(object sender, EventArgs e)
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



