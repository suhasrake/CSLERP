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

namespace CSLERP
{
    public partial class ProductionProcessStatus : System.Windows.Forms.Form
    {
        //Boolean track = false;
        //int no = 0;
        RichTextBox txt;
        string docID = "PRODUCTIONPLAN";
        string forwarderList = "";
        string approverList = "";
        string TempNo;
        DateTime TempDate;
        RichTextBox txtDesc = new RichTextBox();
        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        Boolean userIsACommenter = false;
        string Temp;
        string FloorManagerUserID = "";
        string TeamMemberUserIDs = "";
        Form dtpForm = new Form();
        // Hashtable ht = new Hashtable();
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        productionplanheader prevprodution;
        productionplandetail prevProdDetail;
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        Form frmPopup = new Form();
        Form frmPopupCmt = new Form();
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        //string custid = "";
        //DateTimePicker dtp;
        public ProductionProcessStatus()
        {
            try
            {

                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }

        private void ProductionProcessStatus_Load(object sender, EventArgs e)
        {
            //////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;
            grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdList.EnableHeadersVisualStyles = false;
            grdProcessDetail.ColumnHeadersDefaultCellStyle.BackColor = Color.LightCoral;
            grdProcessDetail.EnableHeadersVisualStyles = false;
            grdProcessDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            String a = this.Size.ToString();
        }

        //called only in the beginning
        private void initVariables()
        {

            //pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            txtComments.Text = "";
            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
            ListFilteredProductionPlanHeader();
        }

        private void ListFilteredProductionPlanHeader()
        {
            try
            {
                grdList.Rows.Clear();
                ProductionPlanHeaderDB pphdb = new ProductionPlanHeaderDB();
                forwarderList = demDB.getForwarders(docID, Login.empLoggedIn);
                approverList = demDB.getApprovers(docID, Login.empLoggedIn);

                List<productionplanheader> ProductionPlanHeaderList = pphdb.getOnGoingProductionPlansForProcessStatus(1);
                foreach (productionplanheader pph in ProductionPlanHeaderList)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = pph.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = pph.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = pph.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = pph.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProductionPlanNo"].Value = pph.ProductionPlanNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProductionPlanDate"].Value = pph.ProductionPlanDate;
                    //Newly added
                    grdList.Rows[grdList.RowCount - 1].Cells["InternalOrderNos"].Value = pph.InternalOrderNos;
                    grdList.Rows[grdList.RowCount - 1].Cells["InternalOrderDates"].Value = pph.InternalOrderDates;
                    grdList.Rows[grdList.RowCount - 1].Cells["Customers"].Value = ProductionPlanHeaderDB.getCustomerListFromReference(pph.Reference).Trim();

                    grdList.Rows[grdList.RowCount - 1].Cells["gReference"].Value = pph.Reference;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStockItemID"].Value = pph.StockItemID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStockItemName"].Value = pph.StockItemName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ModelNo"].Value = pph.ModelNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["ModelName"].Value = pph.ModelName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gQuantity"].Value = pph.Quantity;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPlannedStartTime"].Value = pph.PlannedStartTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gPlannedEndTime"].Value = pph.PlannedEndTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gActualStartTime"].Value = pph.ActualStartTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gActualEndTime"].Value = pph.ActualEndTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["gFloorManager"].Value = pph.FloorManager;
                    grdList.Rows[grdList.RowCount - 1].Cells["gRemarks"].Value = pph.Remarks;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = pph.Status;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentStatus"].Value = pph.DocumentStatus;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProductionStatus"].Value = pph.ProductionStatus;
                    //grdList.Rows[grdList.RowCount - 1].Cells["ProductionStatusString"].Value = getProdStatString(pph.ProductionStatus.ToString());
                    grdList.Rows[grdList.RowCount - 1].Cells["ProductionStatusString"].Value = pph.ProductionStatusString;
                }

                setButtonVisibility("init");
                pnlList.Visible = true;
                grdList.Visible = true;
                grdProcessDetail.Visible = false;
                btnClose.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Production Plan Listing");
            }
        }
        //private string getProdStatString(string stat)
        //{
        //    string str = "";
        //    if (stat == "2")
        //        str = "Halted";
        //    else if (stat == "3")
        //        str = "Canceled";
        //    else if (stat == "99")
        //        str = "Completed";
        //    else if (stat == "1")
        //        str = "Ongoing";
        //    else if (stat == "0")
        //        str = "Not Started";
        //    return str;
        //}
        private void closeAllPanels()
        {
            try
            {
                pnlList.Visible = false;
                //pnlAddEdit.Visible = false;
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

                //grdProductionPlanDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                prevprodution = new productionplanheader();

                removeControlsFrompnllvPanel();

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
                //label1.Visible = false;
                //track = true;
                clearData();
                //pnlAddEdit.Visible = true;
                closeAllPanels();
                //pnlAddEdit.Visible = true;
                //txtReference.Enabled = true;
                setButtonVisibility("btnNew");
            }
            catch (Exception)
            {

            }
        }
        //private int UpdateComboValue()
        //{
        //    //Boolean status = true;
        //    int x = 0;
        //    try
        //    {

        //        if (cmbProcessStatus.Text == "Closed")
        //        {
        //            return 1;
        //        }
        //        else if (cmbProcessStatus.Text == "Open")
        //        {
        //            return 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return x;
        //}

        private Boolean validateProcessStatus(productionplandetail ppd)
        {
            Boolean status = true;
            try
            {

                if (ppd.Remarks.Trim().Length == 0 || ppd.Remarks == null)
                {
                    return false;
                }
                if (ppd.ActualStartTime < ppd.ProductionPlanDate)
                {
                    return false;
                }
                if (ppd.ProcessStatus == 1)
                {
                    //check for actual start and end dates
                    if (ppd.ActualStartTime >= ppd.ActualEndTime)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return status;
        }




        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            //track = true;
            clearData();
            // StockItemDB.fillStockItemCombo(cmbProductName, "");
            closeAllPanels();
            ///pnlAddEdit.Visible = false;
            pnlList.Visible = true;
            setButtonVisibility("btnEditPanel");
            frmPopup.Dispose();
            frmPopup.Close();
        }
        private void pnlList_Paint(object sender, PaintEventArgs e)
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

        private void showPDFFile(string fname)
        {
            try
            {

                AxAcroPDFLib.AxAcroPDF pdf = new AxAcroPDFLib.AxAcroPDF();
                pdf.Dock = System.Windows.Forms.DockStyle.Fill;
                pdf.Enabled = true;
                pdf.Location = new System.Drawing.Point(0, 0);
                pdf.Name = "pdfReader";
                pdf.OcxState = pdf.OcxState;
                ////pdf.OcxState = ((System.Windows.Forms.AxHost.State)(new System.ComponentModel.ComponentResourceManager(typeof(ViewerWindow)).GetObject("pdf.OcxState")));
                pdf.TabIndex = 1;
                // pnlPDFViewer.Controls.Add(pdf);

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

        }

        private void removeControlsFromCommenterPanel()
        {
            try
            {
                foreach (Control p in pnlCmtr.Controls)
                    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
                    {
                        p.Dispose();
                    }
            }
            catch (Exception ex)
            {
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
        private void btnListDocuments_Click(object sender, EventArgs e)
        {

        }
        private void setButtonVisibility(string btnName)
        {
            try
            {

                //btnNew.Visible = false;
                btnExit.Visible = false;

                btnGetComments.Visible = false;
                chkCommentStatus.Visible = false;
                txtComments.Visible = false;
                //disableTabPages();
                clearTabPageControls();
                //----24/11/2016
                handleNewButton();
                ////handleGrdEditButton();
                pnlBottomButtons.Visible = true;
                //----
                if (btnName == "init")
                {

                    btnExit.Visible = true;

                }
                else if (btnName == "Commenter")
                {

                    btnGetComments.Visible = false; //earlier Edit enabled this button
                    chkCommentStatus.Visible = true;
                    txtComments.Visible = true;
                }
                else if (btnName == "btnNew")
                {
                    //btnNew.Visible = false; //added 24/11/2016

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
                    btnGetComments.Visible = true;
                    //enableTabPages();
                    //pnlPDFViewer.Visible = true;
                    chkCommentStatus.Visible = true;
                    txtComments.Visible = true;
                    //tabControl1.SelectedTab = tabProductionPlanHeader;
                }
                else if (btnName == "Approve")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016

                }
                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016

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
        void handleNewButton()
        {
            //btnNew.Visible = false;
            if (Main.itemPriv[1])
            {
                // btnNew.Visible = true;
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
                //removePDFControls();
                //removePDFFileGridView();
                dgvComments.Rows.Clear();
                chkCommentStatus.Checked = false;
                txtComments.Text = "";
                // grdProductionPlanDetail.Rows.Clear();
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

        //-----

        private void grdList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string docid = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                int TempNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value);
                DateTime TmepDatechk = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value);
                productionplanheader pph = new productionplanheader();
                pph.DocumentID = docid;
                pph.TemporaryNo = TempNo;
                pph.TemporaryDate = TmepDatechk;
                showProcessDetails(pph);
            }
            catch (Exception ex)
            {
            }
        }
        private string getProcessStatString(int stat)
        {
            string str = "";
            if (stat == 1)
                str = "Started";
            else if (stat == 99)
                str = "Closed";
            else
                str = "";
            return str;
        }
        private int getProcessStatCode(string statStr)
        {
            int str = 0;
            if (statStr == "Started")
                str = 1;
            else if (statStr == "Closed")
                str = 99;
            else
                str = 0;
            return str;
        }
        private Boolean AddProductionPlanDetailRow()
        {
            Boolean status = true;
            try
            {
                grdProcessDetail.Rows.Add();
                int kount = grdProcessDetail.RowCount;
                grdProcessDetail.Rows[kount - 1].Cells["SiNo"].Value = kount;
                grdProcessDetail.Rows[grdProcessDetail.RowCount - 1].Cells["cTempNo"].Value = "";
                grdProcessDetail.Rows[grdProcessDetail.RowCount - 1].Cells["cTempDate"].Value = "";
                grdProcessDetail.Rows[grdProcessDetail.RowCount - 1].Cells["ProcessStatus"].Value = "";
                grdProcessDetail.Rows[grdProcessDetail.RowCount - 1].Cells["ProcessID"].Value = "";
                grdProcessDetail.Rows[grdProcessDetail.RowCount - 1].Cells["ProcessDescription"].Value = "";
                grdProcessDetail.Rows[grdProcessDetail.RowCount - 1].Cells["TeamMembers"].Value = "";
                grdProcessDetail.Rows[grdProcessDetail.RowCount - 1].Cells["StartTime"].Value = DateTime.Today;
                grdProcessDetail.Rows[grdProcessDetail.RowCount - 1].Cells["EndTime"].Value = DateTime.Today;
                grdProcessDetail.Rows[grdProcessDetail.RowCount - 1].Cells["cActualaStartTime"].Value = DateTime.Today;
                grdProcessDetail.Rows[grdProcessDetail.RowCount - 1].Cells["cActualEndTime"].Value = DateTime.Today;
                grdProcessDetail.Rows[grdProcessDetail.RowCount - 1].Cells["TeamMembersID"].Value = "";
            }

            catch (Exception ex)
            {
                MessageBox.Show("AddProductionDetailRow() : Error");
            }

            return status;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            grdProcessDetail.Visible = false;
            grdProcessDetail.Rows.Clear();
            btnClose.Visible = false;
        }

        private void grdProcessDetail_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Det"))
                {
                    string docid = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    int TempNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value);
                    DateTime TmepDatechk = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value);
                    productionplanheader pph = new productionplanheader();
                    pph.DocumentID = docid;
                    pph.TemporaryNo = TempNo;
                    pph.TemporaryDate = TmepDatechk;
                    pph.ProductionStatus = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gProductionStatus"].Value);
                    showProcessDetails(pph);
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void showProcessDetails(productionplanheader pph)
        {
            try
            {
                List<productionplandetail> processdetial = ProductionPlanHeaderDB.getProductionPlanDetail(pph);
                ERPUserDB erpuser = new ERPUserDB();
                grdProcessDetail.Visible = true;
                btnClose.Visible = true;
                //grdProcessDetail.Refresh();
                grdProcessDetail.Rows.Clear();
                int i = 0;
                try
                {
                    foreach (productionplandetail ppd in processdetial)
                    {
                        AddProductionPlanDetailRow();
                        grdProcessDetail.Rows[i].Cells["ProdStat"].Value = pph.ProductionStatus;
                        grdProcessDetail.Rows[i].Cells["cTempNo"].Value = pph.TemporaryNo;
                        grdProcessDetail.Rows[i].Cells["cTempDate"].Value = pph.TemporaryDate;
                        grdProcessDetail.Rows[i].Cells["ProcessID"].Value = ppd.ProcessID;
                        grdProcessDetail.Rows[i].Cells["SiNo"].Value = ppd.SlNo;
                        grdProcessDetail.Rows[i].Cells["ProcessDescription"].Value = ppd.ProcessDescription;
                        grdProcessDetail.Rows[i].Cells["TeamMembers"].Value = ppd.TeamMembers;
                        grdProcessDetail.Rows[i].Cells["StartTime"].Value = ppd.StartTime;
                        grdProcessDetail.Rows[i].Cells["EndTime"].Value = ppd.EndTime;
                        grdProcessDetail.Rows[i].Cells["cActualaStartTime"].Value = ppd.ActualStartTime;
                        grdProcessDetail.Rows[i].Cells["cActualEndTime"].Value = ppd.ActualEndTime;
                        grdProcessDetail.Rows[i].Cells["TeamMembers"].Value = erpuser.getUserNames(ppd.TeamMembers);
                        grdProcessDetail.Rows[i].Cells["ProcessStatus"].Value = getProcessStatString(ppd.ProcessStatus);
                        if (ppd.ProcessStatus == 1)
                        {
                            grdProcessDetail.Rows[i].DefaultCellStyle.BackColor = Color.LightGreen;
                        }
                        else if (ppd.ProcessStatus == 99)
                        {
                            grdProcessDetail.Rows[i].DefaultCellStyle.BackColor = Color.DarkTurquoise;
                        }
                        else
                        {
                            grdProcessDetail.Rows[i].DefaultCellStyle.BackColor = Color.CadetBlue;
                        }
                        grdProcessDetail.Rows[i].Cells["cRemarks"].Value = ppd.Remarks;
                        i++;
                    }
                    int n = getuserPrivilegeStatus();
                    if (n == 1)
                    {
                        grdProcessDetail.Columns["Start"].Visible = false;
                        grdProcessDetail.Columns["End"].Visible = false;
                    }
                    else
                    {
                        grdProcessDetail.Columns["Start"].Visible = true;
                        grdProcessDetail.Columns["End"].Visible = true;
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

        private void grdProcessDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdProcessDetail.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Start") || columnName.Equals("End") || columnName.Equals("Remark"))
                {
                    prevProdDetail = new productionplandetail();
                    prevProdDetail.DocumentID = "PRODUCTIONPLAN";
                    prevProdDetail.TemporaryNo = Convert.ToInt32(grdProcessDetail.Rows[e.RowIndex].Cells["cTempNo"].Value);
                    prevProdDetail.TemporaryDate = Convert.ToDateTime(grdProcessDetail.Rows[e.RowIndex].Cells["cTempDate"].Value);
                    prevProdDetail.SlNo = Convert.ToInt32(grdProcessDetail.Rows[e.RowIndex].Cells["SiNo"].Value);
                    prevProdDetail.ProcessStatus = getProcessStatCode(grdProcessDetail.Rows[e.RowIndex].Cells["ProcessStatus"].Value.ToString());
                    prevProdDetail.Remarks = grdProcessDetail.Rows[e.RowIndex].Cells["cRemarks"].Value.ToString();
                    int prodstat = Convert.ToInt32(grdProcessDetail.Rows[e.RowIndex].Cells["ProdStat"].Value);
                    if (columnName == "Start")
                    {
                        //if (!ProductionPlanHeaderDB.checkProductionProcessStatusAgainstRawmaterialIssue(prevProdDetail, prevProdDetail.TemporaryNo.ToString(), prevProdDetail.TemporaryDate))
                        //{
                        //    MessageBox.Show("Raw Material Usage Not Finalized For this Production Plan.\nPlease update Process After finalizing RawMaterial Usage.");
                        //    return;
                        //}
                        Boolean isUpdateProdStat = false;

                        if (prodstat == 1) // prodstat == 1 : Not started  prodstat != 1 : started
                        {
                            isUpdateProdStat = true;
                        }
                        else
                        {
                            isUpdateProdStat = false;
                        }
                        if (prevProdDetail.ProcessStatus == 1)
                        {
                            MessageBox.Show("Production already Started.");
                            return;
                        }
                        if (prevProdDetail.ProcessStatus == 99)
                        {
                            MessageBox.Show("Production already Closed.");
                            return;
                        }
                        prevProdDetail.ActualStartTime = UpdateTable.getSQLDateTime();
                        string cmnt = Login.userLoggedInName + " : " + UpdateTable.getSQLDateTime().ToString("yyyy-MM-dd HH:mm:ss") + " : " + "Process started" + Environment.NewLine;
                        prevProdDetail.Remarks = prevProdDetail.Remarks + cmnt;
                        DialogResult dialog = MessageBox.Show("Are you sure to Start the process ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            if (ProductionPlanHeaderDB.updateProductionProcessStatus(prevProdDetail, 1, isUpdateProdStat))
                            {
                                MessageBox.Show("Production Started");
                                if (isUpdateProdStat)
                                    prodstat = 2;
                            }
                            else
                            {
                                MessageBox.Show("Failed to production start");
                            }
                        }
                    }
                    if (columnName == "End")
                    {

                        if (prevProdDetail.ProcessStatus == 0)
                        {
                            MessageBox.Show("Production not started.");
                            return;
                        }
                        if (prevProdDetail.ProcessStatus == 99)
                        {
                            MessageBox.Show("Production already closed.");
                            return;
                        }
                        if (!ProductionPlanHeaderDB.checkProductionProcessStatusAgainstRawmaterialIssue(prevProdDetail, prevProdDetail.TemporaryNo.ToString(), prevProdDetail.TemporaryDate))
                        {
                            MessageBox.Show("Raw Material Usage Not Finalized For this Production Plan.\nPlease update Process After finalizing RawMaterial Usage.");
                            return;
                        }
                        string cmnt = Login.userLoggedInName + " : " + UpdateTable.getSQLDateTime().ToString("yyyy-MM-dd HH:mm:ss") + " : " + "Process closed" + Environment.NewLine;
                        prevProdDetail.Remarks = prevProdDetail.Remarks + cmnt;
                        prevProdDetail.ActualEndTime = UpdateTable.getSQLDateTime();
                        DialogResult dialog = MessageBox.Show("Are you sure to Close the process ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            if (ProductionPlanHeaderDB.updateProductionProcessStatus(prevProdDetail, 2, false))
                            {
                                MessageBox.Show("Production closed");
                            }
                            else
                            {
                                MessageBox.Show("Failed to production close");
                            }
                        }
                    }
                    if (columnName == "Remark")
                    {

                        showPopUpForDescription();
                    }
                    productionplanheader pph = new productionplanheader();
                    pph.DocumentID = "PRODUCTIONPLAN";
                    pph.TemporaryNo = prevProdDetail.TemporaryNo;
                    pph.TemporaryDate = prevProdDetail.TemporaryDate;
                    pph.ProductionStatus = prodstat;
                    showProcessDetails(pph);
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void showPopUpForDescription()
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;
            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;
            frmPopup.Size = new Size(700, 200);

            Label head = new Label();
            head.AutoSize = true;
            head.Location = new System.Drawing.Point(340, 3);
            head.Name = "label2";
            head.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            head.ForeColor = Color.White;
            head.Size = new System.Drawing.Size(146, 13);
            head.Text = "Fill Remark Below";
            frmPopup.Controls.Add(head);

            RichTextBox txtOld = new RichTextBox();
            //string remarksPrev = ProductionPlanHeaderDB.
            //              getUserCommentsForProcess(prevProdDetail.DocumentID, prevProdDetail.TemporaryNo, prevProdDetail.TemporaryDate, prevProdDetail.SlNo);
            txtOld.ReadOnly = true;
            txtOld.BackColor = Color.LemonChiffon;
            txtOld.Text = prevProdDetail.Remarks;
            txtOld.Bounds = new Rectangle(new Point(5, 6), new Size(330, 180));
            frmPopup.Controls.Add(txtOld);

            txtDesc = new RichTextBox();
            txtDesc.Bounds = new Rectangle(new Point(340, 25), new Size(350, 111));
            frmPopup.Controls.Add(txtDesc);


            Button lvOK = new Button();
            lvOK.Text = "Save";
            lvOK.BackColor = Color.Tan;
            lvOK.Location = new System.Drawing.Point(550, 142);
            lvOK.Size = new System.Drawing.Size(64, 23);
            lvOK.Cursor = Cursors.Hand;
            lvOK.Click += new System.EventHandler(this.lvOK_Click5);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new System.Drawing.Point(620, 142);
            lvCancel.Size = new System.Drawing.Size(73, 23);
            lvCancel.Cursor = Cursors.Hand;
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click5);
            frmPopup.Controls.Add(lvCancel);
            if (prevProdDetail.ProcessStatus == 99)
            {
                lvOK.Visible = false;
            }
            int n = getuserPrivilegeStatus();
            if (n == 1)
            {
                lvOK.Visible = false;
            }
            frmPopup.ShowDialog();
        }
        private void lvOK_Click5(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtDesc.Text.Trim()))
                {
                    MessageBox.Show("Remarks is empty");
                    return;
                }
                string remarksPrev = prevProdDetail.Remarks;
                string cmnt = Login.userLoggedInName + " : " + UpdateTable.getSQLDateTime().ToString("yyyy-MM-dd HH:mm:ss") + " : " + txtDesc.Text + Environment.NewLine;
                string remarksMain = remarksPrev + cmnt;
                if (ProductionPlanHeaderDB.updateProductionProcessRemarks(prevProdDetail, remarksMain))
                {
                    MessageBox.Show("Remarks updated");
                }
                else
                {
                    MessageBox.Show("Failed to updatae remark");
                }
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        private void lvCancel_Click5(object sender, EventArgs e)
        {
            try
            {
                txtDesc.Text = "";
                frmPopup.Close();
                frmPopup.Dispose();

            }
            catch (Exception)
            {
            }
        }

        private void ProductionProcessStatus_Enter(object sender, EventArgs e)
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















