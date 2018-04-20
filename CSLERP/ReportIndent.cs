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

namespace CSLERP
{
    public partial class ReportIndent : System.Windows.Forms.Form
    {
        string docID = "";
        ////string chkDocID = "";
        ////string forwarderList = "";
        ////string approverList = "";
        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        int listOption = 1; //1-Pending, 2-In Process, 3-Approved
        double productvalue = 0.0;
        double taxvalue = 0.0;
        ////Boolean userIsACommenter = false;
        ListView exlv = new ListView();
        popiheader prevpopi;
        TreeView tv = new TreeView();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        Panel pnllv = new Panel();
        Panel pnlModel = new Panel();
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Form frmPopup = new Form();
        List<documentreceiver> ListL2 = new List<documentreceiver>();
        public ReportIndent()
        {
            try
            {
                var ListL = Main.DocumentReceivers.Where(W => W.DocumentID == "POSERVICEINWARD" || W.DocumentID == "PAFSERVICEINWARD" || W.DocumentID == "POPRODUCTINWARD" || W.DocumentID == "PAFPRODUCTINWARD" && W.Status == 1).ToList();


                string DocName = "";
                foreach (var itm in ListL)
                {
                    documentreceiver obj = new documentreceiver();
                    DocName = itm.DocumentID;
                    DocName = DocName.Replace("POSERVICEINWARD", "Service PO");
                    DocName = DocName.Replace("PAFSERVICEINWARD", "ServicePAF");
                    DocName = DocName.Replace("POPRODUCTINWARD", "Product PO");
                    DocName = DocName.Replace("PAFPRODUCTINWARD", "ProductPAF");
                    obj.DocumentName = DocName;
                    obj.OfficeID = itm.OfficeID;
                    obj.OfficeName = itm.OfficeName;
                    obj.DocumentID = itm.DocumentID;
                    ListL2.Add(obj);
                }
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void POPIHeader_Load(object sender, EventArgs e)
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
            setButtonVisibility("init");
            pnlList.Visible = true;
            pnlFilter.Visible = true;
        }

        private void initVariables()
        {
            if (getuserPrivilegeStatus() == 1)
            {
                listOption = 6;
            }

            //Structures.ComboBoxItem cbitem =   new Structures.ComboBoxItem("All", "All");

            cmbStatus.SelectedIndex = 0;
            docID = Main.currentDocument;
            //CustomerDB.fillLedgerTypeComboNew(cmbCustomer, "Supplier");
            //cmbCustomer.Items.Add("All");

            //cmbCustomer.SelectedItem = "All";          
            //cmbTrack.SelectedIndex = 0;



            dtFrom.Format = DateTimePickerFormat.Custom;
            dtFrom.CustomFormat = "dd-MM-yyyy";
            dtFrom.Enabled = true;
            dtFrom.Value = DateTime.Now.AddMonths(-2);
            dtTo.Format = DateTimePickerFormat.Custom;
            dtTo.CustomFormat = "dd-MM-yyyy";
            dtTo.Enabled = true;
            ////pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            {
                TaxDetailsTable.Columns.Add("TaxItem", typeof(string));
                TaxDetailsTable.Columns.Add("TaxAmount", typeof(double));
            }
            //cmbProduct.TabIndex = 0;
            ////grdPOPIDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
        }


        private void closeAllPanels()
        {
            try
            {
                pnlList.Visible = false;
                ////pnlAddEdit.Visible = false;
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

                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                prevpopi = new popiheader();
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




        private void btnActionPending_Click(object sender, EventArgs e)
        {
            listOption = 1;
            // ListFilteredStockOBHeader(listOption);
        }

        private void btnApprovalPending_Click(object sender, EventArgs e)
        {
            listOption = 2;
            //  ListFilteredStockOBHeader(listOption);
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

            // ListFilteredStockOBHeader(listOption);
        }


        public List<string> GetCheckedNodes(TreeNodeCollection nodes)
        {
            List<string> nodeList = new List<string>();
            try
            {

                if (nodes == null)
                {
                    return nodeList;
                }

                foreach (TreeNode childNode in nodes)
                {
                    if (childNode.Checked)
                    {
                        nodeList.Add(childNode.Text);
                    }
                    nodeList.AddRange(GetCheckedNodes(childNode.Nodes));
                }

            }
            catch (Exception ex)
            {
            }
            return nodeList;
        }
        private void tvCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //lvApprover.CheckBoxes = false;
                //lvApprover.CheckBoxes = true;
                tv.CheckBoxes = true;
                pnlForwarder.Controls.Remove(tv);
                pnlForwarder.Visible = false;
            }
            catch (Exception)
            {
            }
        }
        private void tv_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Checked == true)
            {
                if (e.Node.Nodes.Count != 0)
                {
                    MessageBox.Show("you are not allowed to select group");
                    e.Node.Checked = false;
                }
            }
        }
        private void removeControlsFromForwarderPanelTV()
        {
            try
            {
                //foreach (Control p in pnlForwarder.Controls)
                //    if (p.GetType() == typeof(TreeView) || p.GetType() == typeof(Button) || p.GetType() == typeof(ListView))
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

        private void removeControlsFromModelPanel()
        {
            try
            {
                pnlModel.Controls.Clear();
                Control nc = pnlModel.Parent;
                nc.Controls.Remove(pnlModel);
            }
            catch (Exception ex)
            {
            }
        }
        //-----
        private void btnCalculateax_Click_1(object sender, EventArgs e)
        {
        }

        private string getPODocIDForSelectedIO(string iodocid)
        {
            string podocid = "";
            switch (docID)
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
                default:
                    podocid = "";
                    break;
            }
            return podocid;
        }

        private void showStockPOQuantity(indentheader poph)
        {

            try
            {
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.AntiqueWhite;

                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(1100, 400);
                //frmPopup.Location = new Point(300, 200);

                //lv = POPIHeaderDB.getPONoWiseStockListView(Convert.ToInt32(txtPOTrackNo.Text), dtPOTrackDate.Value);
                DataGridView grdDetail = new DataGridView();
                grdDetail.AllowUserToAddRows = false;
                grdDetail = fillgrddetail(poph);

                //--------------
                grdDetail.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdDetail.EnableHeadersVisualStyles = false;
                grdDetail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdDetail.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdDetail.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grdDetail.RowHeadersVisible = false;
                grdDetail.AllowUserToAddRows = false;
                grdDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                grdDetail.Bounds = new Rectangle(new Point(20, 60), new Size(1070, 300));
                frmPopup.Controls.Add(grdDetail);

                Label lblItemCode = new Label();
                lblItemCode.Width = 500;
                lblItemCode.Text = "Indent No  :" + poph.IndentNo;
                lblItemCode.Location = new Point(40, 10);
                frmPopup.Controls.Add(lblItemCode);

                Label lblItemName = new Label();
                lblItemName.Width = 500;
                lblItemName.Text = "Indent Date :" + poph.IndentDate.ToString("dd-MM-yyyy");
                lblItemName.Location = new Point(40, 35);
                frmPopup.Controls.Add(lblItemName);


                Button lvClose = new Button();
                lvClose.BackColor = Color.LightGray;
                lvClose.Text = "CLOSE";
                lvClose.Location = new Point(40, 365);
                lvClose.Click += new System.EventHandler(this.grddetailclickClose);
                frmPopup.Controls.Add(lvClose);
                frmPopup.ShowDialog();

            }
            catch (Exception ex)
            {

            }
        }

        private void showitemDetail(indentheader poph)
        {

            try
            {
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.Manual;
                frmPopup.BackColor = Color.AntiqueWhite;

                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(900, 400);
                frmPopup.Location = new Point(300, 200);

                //lv = POPIHeaderDB.getPONoWiseStockListView(Convert.ToInt32(txtPOTrackNo.Text), dtPOTrackDate.Value);
                DataGridView grdDetail = new DataGridView();
                grdDetail.AllowUserToAddRows = false;
                grdDetail = fillgrddetail2(poph);

                //--------------
                grdDetail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdDetail.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdDetail.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grdDetail.RowHeadersVisible = false;
                grdDetail.AllowUserToAddRows = false;
                grdDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grdDetail.Bounds = new Rectangle(new Point(20, 60), new Size(850, 300));
                frmPopup.Controls.Add(grdDetail);

                Label lblItemCode = new Label();
                lblItemCode.Width = 500;
                lblItemCode.Text = "Indent No  : " + poph.IndentNo;
                lblItemCode.Location = new Point(40, 12);
                frmPopup.Controls.Add(lblItemCode);

                Label lblItemName = new Label();
                lblItemName.Width = 500;
                lblItemName.Text = "IndentDate : " + poph.IndentDate.ToString("dd-MM-yyyy");
                lblItemName.Location = new Point(40, 35);
                frmPopup.Controls.Add(lblItemName);


                Button lvClose = new Button();
                lvClose.BackColor = Color.LightGray;
                lvClose.Text = "CLOSE";
                lvClose.Location = new Point(40, 365);
                lvClose.Click += new System.EventHandler(this.grddetailclickClose);
                frmPopup.Controls.Add(lvClose);
                frmPopup.ShowDialog();

            }
            catch (Exception ex)
            {

            }
        }
        private void grddetailclickOK(object sender, EventArgs e)
        {
            try
            {


            }
            catch (Exception ex)
            {
            }
        }
        private void grddetailclickClose(object sender, EventArgs e)
        {
            try
            {
                //btnSelectItem.Enabled = true;
                frmPopup.Close();
                frmPopup.Dispose();

            }
            catch (Exception)
            {
            }
        }
        private DataGridView fillgrddetail(indentheader ih)
        {
            DataGridView grdDetail = new DataGridView();
            try
            {

                grdDetail.RowHeadersVisible = false;

                DataGridViewTextBoxColumn dgvtx0 = new DataGridViewTextBoxColumn();
                dgvtx0.Width = 50;
                dgvtx0.ReadOnly = true;
                dgvtx0.Name = "SlNo";
                dgvtx0.Visible = true;
                dgvtx0.Frozen = true;
                grdDetail.Columns.Add(dgvtx0);

                DataGridViewTextBoxColumn dgvtx2 = new DataGridViewTextBoxColumn();
                dgvtx2.Width = 60;
                dgvtx2.ReadOnly = true;
                dgvtx2.Name = "PONo";
                dgvtx2.Frozen = true;
                grdDetail.Columns.Add(dgvtx2);

                DataGridViewTextBoxColumn dgvtx3 = new DataGridViewTextBoxColumn();
                dgvtx3.Width = 100;
                dgvtx3.ReadOnly = true;
                dgvtx3.DefaultCellStyle.Format = "dd-MM-yyyy";
                dgvtx3.Name = "PODate";
                dgvtx3.Frozen = true;
                grdDetail.Columns.Add(dgvtx3);

                DataGridViewTextBoxColumn dgvtx4 = new DataGridViewTextBoxColumn();
                dgvtx4.Width = 200;
                dgvtx4.ReadOnly = true;
                dgvtx4.Name = "Supplier";
                grdDetail.Columns.Add(dgvtx4);

                DataGridViewTextBoxColumn dgvtx5 = new DataGridViewTextBoxColumn();
                dgvtx5.Width = 120;
                dgvtx5.ReadOnly = true;
                dgvtx5.Name = "ItemCode";
                grdDetail.Columns.Add(dgvtx5);

                DataGridViewTextBoxColumn dgvtx7 = new DataGridViewTextBoxColumn();
                dgvtx7.Width = 200;
                dgvtx7.ReadOnly = true;
                dgvtx7.DefaultCellStyle.Format = "N0";
                dgvtx7.Name = "ItemName";
                grdDetail.Columns.Add(dgvtx7);

                DataGridViewTextBoxColumn dgvtx8 = new DataGridViewTextBoxColumn();
                dgvtx8.Width = 70;
                dgvtx8.ReadOnly = true;
                dgvtx8.DefaultCellStyle.Format = "N2";
                dgvtx8.Name = "Quantity";
                grdDetail.Columns.Add(dgvtx8);

                DataGridViewTextBoxColumn dgvtx9 = new DataGridViewTextBoxColumn();
                dgvtx9.DefaultCellStyle.Format = "N2";
                dgvtx9.Name = "Rate";
                dgvtx9.ReadOnly = true;
                grdDetail.Columns.Add(dgvtx9);

                DataGridViewTextBoxColumn dgvtx10 = new DataGridViewTextBoxColumn();
                dgvtx10.Width = 100;
                dgvtx10.ReadOnly = true;
                dgvtx10.DefaultCellStyle.Format = "N0";
                dgvtx10.Name = "ProductValue";
                dgvtx10.HeaderText = "Product Value";
                grdDetail.Columns.Add(dgvtx10);

                DataGridViewTextBoxColumn dgvtx11 = new DataGridViewTextBoxColumn();
                dgvtx11.Width = 70;
                dgvtx11.ReadOnly = true;
                dgvtx11.DefaultCellStyle.Format = "N0";
                dgvtx11.Name = "Tax";
                dgvtx11.HeaderText = "Tax";
                grdDetail.Columns.Add(dgvtx11);

                {
                    string indentno = Convert.ToString(ih.IndentNo);
                    string indentdate = ih.IndentDate.ToString("yyyy-MM-dd");
                    string docid = ih.DocumentID;
                    if (docid != "INDENTSERVICE")
                    {
                        string refIndStr = docid + "(" + indentno + Main.delimiter1 + indentdate + ")";
                        indentheader inh = new indentheader();
                        inh.IndentNo = ih.IndentNo;
                        //List<indentdetail> inddet = new List<indentdetail>();
                        if (ih.DocumentID == "INDENT" || ih.DocumentID == "INDENTSTATIONERY")
                        {
                            //inddet = IndentHeaderDB.getIndentDetailForReport(ih);   //Indent Details List
                            List<podetail> PODetail = IndentHeaderDB.GetDetailPO(refIndStr); // Po Details
                            grdDetail.Rows.Clear();
                            int i = 0;
                            foreach (podetail pod in PODetail)
                            {
                                try
                                {
                                    //indentdetail ind = inddet.FirstOrDefault(x => x.StockItemID == pod.StockItemID);
                                    //if (ind != null)
                                    //{
                                    grdDetail.Rows.Add();
                                    grdDetail.Rows[i].Cells["SLNo"].Value = grdDetail.RowCount - 1;
                                    grdDetail.Rows[i].Cells["PONo"].Value = pod.TemporaryNo;
                                    grdDetail.Rows[i].Cells["PODate"].Value = pod.TemporaryDate;
                                    grdDetail.Rows[i].Cells["Supplier"].Value = pod.ModelName;
                                    grdDetail.Rows[i].Cells["ItemCode"].Value = pod.StockItemID;
                                    grdDetail.Rows[i].Cells["ItemName"].Value = pod.StockItemName;
                                    grdDetail.Rows[i].Cells["Quantity"].Value = pod.Quantity;
                                    grdDetail.Rows[i].Cells["Rate"].Value = pod.Price;
                                    grdDetail.Rows[i].Cells["ProductValue"].Value = pod.Quantity * pod.Price;
                                    grdDetail.Rows[i].Cells["Tax"].Value = pod.Tax;
                                    i++;
                                    //}
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                        else // for indent general
                        {
                            //inddet = IndentHeaderDB.getIndentGeneralDetailForReport(ih);
                            List<podetail> PODetail = IndentHeaderDB.GetDetailPO(refIndStr);
                            grdDetail.Rows.Clear();
                            int i = 0;
                            foreach (podetail pod in PODetail)
                            {
                                try
                                {
                                    grdDetail.Rows.Add();
                                    grdDetail.Rows[i].Cells["SLNo"].Value = grdDetail.RowCount - 1;
                                    grdDetail.Rows[i].Cells["PONo"].Value = pod.TemporaryNo;
                                    grdDetail.Rows[i].Cells["PODate"].Value = pod.TemporaryDate;
                                    grdDetail.Rows[i].Cells["Supplier"].Value = pod.ModelName;
                                    grdDetail.Rows[i].Cells["ItemCode"].Value = pod.StockItemID;
                                    grdDetail.Rows[i].Cells["ItemName"].Value = pod.StockItemName;
                                    grdDetail.Rows[i].Cells["Quantity"].Value = pod.Quantity;
                                    grdDetail.Rows[i].Cells["Rate"].Value = pod.Price;
                                    grdDetail.Rows[i].Cells["ProductValue"].Value = pod.Quantity * pod.Price;
                                    grdDetail.Rows[i].Cells["Tax"].Value = pod.Tax;
                                    i++;
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    }
                    else    //For indent service
                    {
                        string refIndStr = indentno + "(" + indentdate + ")";
                        indentheader inh = new indentheader();
                        inh.IndentNo = ih.IndentNo;
                        //List<indentdetail> inddet = IndentHeaderDB.getIndentServiceDetailForReport(ih);
                        List<podetail> PODetail = IndentHeaderDB.GetDetailWOForIndentService(refIndStr);
                        grdDetail.Rows.Clear();
                        int i = 0;
                        foreach (podetail pod in PODetail)
                        {
                            try
                            {
                                //indentdetail ind = inddet.FirstOrDefault(x => x.StockItemID == pod.StockItemID);
                                //if (ind != null)
                                //{
                                    grdDetail.Rows.Add();
                                    grdDetail.Rows[i].Cells["SLNo"].Value = grdDetail.RowCount - 1;
                                    grdDetail.Rows[i].Cells["PONo"].Value = pod.TemporaryNo;
                                    grdDetail.Rows[i].Cells["PODate"].Value = pod.TemporaryDate;
                                    grdDetail.Rows[i].Cells["Supplier"].Value = pod.ModelName;
                                    grdDetail.Rows[i].Cells["ItemCode"].Value = pod.StockItemID;
                                    grdDetail.Rows[i].Cells["ItemName"].Value = pod.StockItemName;
                                    grdDetail.Rows[i].Cells["Quantity"].Value = pod.Quantity;
                                    grdDetail.Rows[i].Cells["Rate"].Value = pod.Price;
                                    grdDetail.Rows[i].Cells["ProductValue"].Value = pod.Quantity * pod.Price;
                                    grdDetail.Rows[i].Cells["Tax"].Value = pod.Tax;
                                    i++;
                                //}
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in PO Detail filling");
            }
            return grdDetail;
        }
        private DataGridView fillgrddetail2(indentheader ih)
        {
            DataGridView grdDetail = new DataGridView();
            try
            {
                grdDetail.RowHeadersVisible = false;
                grdDetail.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdDetail.EnableHeadersVisualStyles = false;
                DataGridViewTextBoxColumn dgvtx0 = new DataGridViewTextBoxColumn();
                dgvtx0.ReadOnly = true;
                dgvtx0.Width = 80;
                dgvtx0.Name = "SlNo";
                dgvtx0.Visible = true;
                grdDetail.Columns.Add(dgvtx0);

                DataGridViewTextBoxColumn dgvtx1 = new DataGridViewTextBoxColumn();
                dgvtx1.Width = 215;
                dgvtx1.ReadOnly = true;
                dgvtx1.Name = "Name";
                dgvtx1.Visible = true;
                grdDetail.Columns.Add(dgvtx1);

                DataGridViewTextBoxColumn dgvtx4 = new DataGridViewTextBoxColumn();
                dgvtx4.Width = 212;
                dgvtx4.ReadOnly = true;
                dgvtx4.Name = "Description";
                grdDetail.Columns.Add(dgvtx4);

                DataGridViewTextBoxColumn dgvtx2 = new DataGridViewTextBoxColumn();
                dgvtx2.Width = 170;
                dgvtx2.ReadOnly = true;
                dgvtx2.Name = "LastPurchasePrice";
                grdDetail.Columns.Add(dgvtx2);

                DataGridViewTextBoxColumn dgvtx3 = new DataGridViewTextBoxColumn();
                dgvtx3.Width = 170;
                dgvtx3.ReadOnly = true;
                dgvtx3.Name = "ExpectedPurchasePrice";
                grdDetail.Columns.Add(dgvtx3);
                if (ih.DocumentID == "INDENT" || ih.DocumentID == "INDENTSTATIONERY")
                {
                    List<indentdetail> indDet = IndentHeaderDB.getIndentDetailForReport(ih);
                    grdDetail.Rows.Clear();
                    int i = 0;
                    foreach (var popid in indDet)
                    {
                        try
                        {
                            grdDetail.Rows.Add();
                            grdDetail.Rows[i].Cells["SLNo"].Value = grdDetail.RowCount - 1;
                            grdDetail.Rows[i].Cells["Name"].Value = popid.StockItemName;
                            grdDetail.Rows[i].Cells["LastPurchasePrice"].Value = popid.LastPurchasedPrice;
                            grdDetail.Rows[i].Cells["ExpectedPurchasePrice"].Value = popid.ExpectedPurchasePrice;
                            grdDetail.Rows[i].Cells["Description"].Value = popid.ModelDetails;
                            i++;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                else if (ih.DocumentID == "INDENTGENERAL")
                {
                    List<indentdetail> indDet = IndentHeaderDB.getIndentGeneralDetailForReport(ih);
                    grdDetail.Rows.Clear();
                    int i = 0;
                    foreach (var popid in indDet)
                    {
                        try
                        {
                            grdDetail.Rows.Add();
                            grdDetail.Rows[i].Cells["SLNo"].Value = grdDetail.RowCount - 1;
                            grdDetail.Rows[i].Cells["Name"].Value = popid.StockItemName; //Item Detail
                            grdDetail.Rows[i].Cells["LastPurchasePrice"].Value = "";
                            grdDetail.Rows[i].Cells["ExpectedPurchasePrice"].Value = popid.ExpectedPurchasePrice;
                            grdDetail.Rows[i].Cells["Description"].Value = "";
                            i++;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                else if (ih.DocumentID == "INDENTSERVICE")
                {
                    List<indentdetail> indDet = IndentHeaderDB.getIndentServiceDetailForReport(ih);
                    grdDetail.Rows.Clear();
                    int i = 0;
                    foreach (var popid in indDet)
                    {
                        try
                        {
                            grdDetail.Rows.Add();
                            grdDetail.Rows[i].Cells["SLNo"].Value = grdDetail.RowCount - 1;
                            grdDetail.Rows[i].Cells["Name"].Value = popid.StockItemName; //Item Name
                            grdDetail.Rows[i].Cells["LastPurchasePrice"].Value = "";
                            grdDetail.Rows[i].Cells["ExpectedPurchasePrice"].Value = popid.ExpectedPurchasePrice; // Main Price of service
                            grdDetail.Rows[i].Cells["Description"].Value = popid.ModelDetails; // Work description
                            i++;
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                //List<indentdetail> indDet = IndentHeaderDB.getIndentDetailqty2(ih, opt);
                //grdDetail.Rows.Clear();
                //int i = 0;
                //foreach (var popid in indDet)
                //{
                //    try
                //    {
                //        grdDetail.Rows.Add();
                //        grdDetail.Rows[i].Cells["SLNo"].Value = grdDetail.RowCount - 1;
                //        grdDetail.Rows[i].Cells["Name"].Value = popid.StockItemName;
                //        grdDetail.Rows[i].Cells["LastPurchasePrice"].Value = popid.LastPurchasedPrice;
                //        grdDetail.Rows[i].Cells["ExpectedPurchasePrice"].Value = popid.ExpectedPurchasePrice;
                //        grdDetail.Rows[i].Cells["Description"].Value = popid.ModelDetails;
                //        i++;

                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in PO Quantity filling");
            }
            return grdDetail;
        }
        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                grdList.Rows[e.RowIndex].Selected = true;
                ////grdList.CurrentRow.DefaultCellStyle.BackColor = Color.LightSeaGreen;
                string columnName = grdList.Columns[e.ColumnIndex].Name;

                if (columnName.Equals("PODetails") || columnName.Equals("Close") || columnName.Equals("ItemDetail"))
                {
                    indentheader poph = new indentheader();
                    //int totalvalue = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["TotalValueBilled"].Value);                
                    poph.IndentNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["IndentNo"].Value);
                    poph.IndentDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["IndentDate"].Value);
                    poph.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    //poph.PODates = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["PODate"].Value);
                    //    poph.ProductValue = grdList.Rows[e.RowIndex].Cells["gCustomerName"].Value.ToString();
                    poph.DocumentName = grdList.Rows[e.RowIndex].Cells["gDocumentName"].Value.ToString();
                    if (columnName.Equals("PODetails"))
                    {
                        showStockPOQuantity(poph);
                    }
                    if (columnName.Equals("ItemDetail"))
                    {
                        showitemDetail(poph);
                    }
                    if (columnName.Equals("Close"))
                    {
                        IndentHeaderDB popihdb = new IndentHeaderDB();

                        string dstr = "Document Type : " + poph.DocumentName + "\n" +
                            "Indent No : " + poph.IndentNo + "\n" +
                            "Indent Date : " + poph.IndentDate.ToString("dd-MM-yyyy") + "\n" +
                            "Are you sure to Close the Indent ?";//;+ poph.CustomerName + "\n" +
                        DialogResult dialog = MessageBox.Show(dstr, "Close Indent", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {

                            if (popihdb.ClosIndent2(poph))
                            {
                                MessageBox.Show("Indent Closed");
                                btnView.PerformClick();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private void cmbPOType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }
        private void setPODetailColumns(string docID)
        {
            try
            {
                if (docID == "POPRODUCTINWARD")
                {

                }
                else if (docID == "POSERVICEINWARD")
                {

                }
            }
            catch (Exception)
            {
            }
        }

        private void cmbFreightTerms_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
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

        private void enableTabPages()
        {
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
                //pnlPDFViewer.Controls.Add(pdf);

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

        }
        private void showPDFFileGrid()
        {
        }
        private void removePDFFileGridView()
        {

        }

        private void removeControlsFromCommenterPanel()
        {
            try
            {

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
                //    }
                pnlForwarder.Controls.Clear();
                Control nc = pnlForwarder.Parent;
                nc.Controls.Remove(pnlForwarder);
            }
            catch (Exception ex)
            {
            }
        }

        private void btnListDocuments_Click(object sender, EventArgs e)
        {

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
                    string subDir = prevpopi.TemporaryNo + "-" + prevpopi.TemporaryDate.ToString("yyyyMMddhhmmss");
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

                btnExit.Visible = false;

                pnlBottomButtons.Visible = true;
                //----
                if (btnName == "init")
                {
                    btnExit.Visible = true;
                    pnlFilter.Visible = true;
                }
                if (btnName == "Edit")
                {
                    btnExit.Visible = true;
                    //btnExportToExcel.Visible = true;

                    if (cmbStatus.SelectedItem.ToString() == "Closed")
                    {
                        grdList.Columns["Close"].Visible = false;
                    }
                    else
                    {
                        int prv = getuserPrivilegeStatus();
                        if (prv == 2)
                        {
                            grdList.Columns["Close"].Visible = true;
                        }
                        else
                        {
                            grdList.Columns["Close"].Visible = false;
                        }
                    }
                }
                else if (btnName == "QtyDetails")
                {
                    pnlFilter.Enabled = false;
                    pnlBottomButtons.Enabled = false; //24/11/2016

                    grdList.Enabled = false;
                    grdList.Visible = true;
                    pnlFilter.Visible = true;

                    pnlList.Enabled = false;
                    btnExit.Visible = true;

                }


                else if (btnName == "View")
                {
                    pnlBottomButtons.Visible = true;
                    pnlFilter.Visible = true;
                    grdList.Visible = true;
                    grdList.Visible = true;
                    btnExit.Visible = true;
                    grdList.Enabled = true;
                    pnlList.Enabled = true;
                    pnlFilter.Enabled = true;
                    btnExit.Enabled = true;
                    pnlBottomButtons.Enabled = true;
                    //btnExportToExcel.Visible = true;

                }


            }
            catch (Exception ex)
            {
            }
        }
        void handleNewButton()
        {

        }
        void handleGrdEditButton()
        {
            grdList.Columns["Edit"].Visible = false;
            if (Main.itemPriv[1] || Main.itemPriv[2])
            {
                if (listOption == 1)
                {
                    grdList.Columns["Edit"].Visible = true;
                }
            }
        }

        void handleGrdViewButton()
        {
            grdList.Columns["Close"].Visible = false;
            int priv = getuserPrivilegeStatus();
            //if any one of view,add and edit
            if (priv == 3)
            {
                grdList.Columns["Close"].Visible = true;
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
                removeControlsFromCommenterPanel();
                removeControlsFromForwarderPanel();
                dgvComments.Rows.Clear();

            }
            catch (Exception ex)
            {
            }
        }

        private void txtExchangeRate_TextChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            grdList.Visible = true;
            grdList.Rows.Clear();
            handleGrdViewButton();
            setButtonVisibility("Edit");
            try
            {

                StringBuilder query = new StringBuilder();
                if (cmbStatus.SelectedIndex == -1)
                {
                    MessageBox.Show("Fill Status combo");
                    return;
                }
                string type = cmbStatus.SelectedItem.ToString();

                IndentHeaderDB inhdb = new IndentHeaderDB();
                List<indentheader> inhList = inhdb.getIndentHeaderdata(type, dtFrom.Value, dtTo.Value);

                foreach (indentheader inh in inhList) //1110
                {

                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = inh.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentName"].Value = inh.DocumentName;
                    grdList.Rows[grdList.RowCount - 1].Cells["IndentNo"].Value = inh.IndentNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["IndentDate"].Value = inh.IndentDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = inh.CreatorName;
                    if (inh.DocumentID == "INDENTSERVICE")
                    {
                        if (IndentServiceDB.isWOPreparedForIS(inh.IndentNo, inh.IndentDate))
                        {
                            grdList.Rows[grdList.RowCount - 1].DefaultCellStyle.BackColor = Color.Tan;
                        }
                    }
                    else
                    {
                        if (IndentHeaderDB.isPOPreapredForIndent(inh.IndentNo, inh.IndentDate, inh.DocumentID))
                        {
                            grdList.Rows[grdList.RowCount - 1].DefaultCellStyle.BackColor = Color.Tan;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Indent Report Listing");
                grdList.Rows.Clear();
            }
            if (grdList.Rows.Count <= 0)
            {
                //btnExportToExcel.Visible = false;
                grdList.Visible = false;
                //pnlTotal.Visible = false;
                MessageBox.Show("No Data to Show");
            }
            else
            {
                //lblPOValue.Text = Math.Round(totalPOValue/100000,2).ToString() +" Lakhs";
                //lblBilledValue.Text = Math.Round(totalInvoiceValue/100000,2).ToString() + " Lakhs";
                //lblBalanceValue.Text = Math.Round((totalPOValue - totalInvoiceValue)/100000,2).ToString() + " Lakhs";
                //lblPOCount.Text = " PO Count:" + grdList.Rows.Count;
                //pnlTotal.Visible = true;
            }
        }

        private void grdPOPIDetail_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            grdList.Enabled = true;
            pnlList.Enabled = true;
            pnlFilter.Enabled = true;
            pnlBottomButtons.Enabled = true;
        }

        private void removeControlsFromPnlLvPanel()
        {
            try
            {
                pnllv.Controls.Clear();
                Control nc = pnllv.Parent;
                nc.Controls.Remove(pnllv);
            }
            catch (Exception ex)
            {
            }
        }


        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdList.Rows.Count <= 0)
                {
                    MessageBox.Show("No entries in grid");
                    return;
                }
                removeControlsFromPnlLvPanel();
                pnllv = new Panel();
                pnllv.BorderStyle = BorderStyle.FixedSingle;

                pnllv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(100, 100), new Size(600, 300));
                exlv = Utilities.GridColumnSelectionView(grdList);
                exlv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(50, 50), new Size(500, 200));
                pnllv.Controls.Add(exlv);

                System.Windows.Forms.Label pnlHeading = new System.Windows.Forms.Label();
                pnlHeading.Size = new Size(300, 20);
                pnlHeading.Location = new System.Drawing.Point(50, 20);
                pnlHeading.Text = "Select Gridview Colums to Export";
                pnlHeading.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                pnlHeading.ForeColor = Color.Black;
                pnllv.Controls.Add(pnlHeading);

                System.Windows.Forms.Button exlvOK = new System.Windows.Forms.Button();
                exlvOK.Text = "OK";
                exlvOK.Location = new System.Drawing.Point(50, 270);
                exlvOK.Click += new System.EventHandler(this.exlvOK_Click1);
                pnllv.Controls.Add(exlvOK);

                System.Windows.Forms.Button exlvCancel = new System.Windows.Forms.Button();
                exlvCancel.Text = "Cancel";
                exlvCancel.Location = new System.Drawing.Point(150, 270);
                exlvCancel.Click += new System.EventHandler(this.exlvCancel_Click2);
                pnllv.Controls.Add(exlvCancel);
                pnlList.Controls.Add(pnllv);
                pnllv.BringToFront();
                pnllv.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in grid data. Export failed");
            }
        }
        private void exlvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                pnllv.Visible = false;
                string heading1 = "POPI Report";
                string heading2 = "";
                string heading3 = "Form Date" + Main.delimiter1 + dtFrom.Value.ToString("dd/MM/yyyy") + Main.delimiter2 +
                    "To Date" + Main.delimiter1 + dtTo.Value.ToString("dd/MM/yyyy");
                Utilities.export2Excel(heading1, heading2, heading3, grdList, exlv);
                removeControlsFromPnlLvPanel();
                pnllv.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }
        private void exlvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                removeControlsFromPnlLvPanel();
                pnllv.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
            //pnlTotal.Visible = false;
            //btnExportToExcel.Visible = false;
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
            //pnlTotal.Visible = false;
            //btnExportToExcel.Visible = false;
        }

        private void cmbCustomer_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            grdList.Visible = false;
            //pnlTotal.Visible = false;
            //btnExportToExcel.Visible = false;
        }

        private void cmbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
            //pnlTotal.Visible = false;
            //btnExportToExcel.Visible = false;
        }

        private void cmbTrack_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
            //pnlTotal.Visible = false;
            //btnExportToExcel.Visible = false;
        }

        private void ReportIndent_Enter(object sender, EventArgs e)
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

        //private void cmbPOType_SelectedIndexChanged_1(object sender, EventArgs e)
        //{
        //    grdList.Visible = false;
        //    pnlTotal.Visible = false;
        //    btnExportToExcel.Visible = false;
        //    if (RPOUsrAuthorty.Text != "1")
        //    {
        //        cmbOfficetyp.Items.Clear();
        //       string DocID = ((Structures.ComboBoxItem)cmbPOType.SelectedItem).HiddenValue;

        //        var ListL = ListL2.GroupBy(G => G.OfficeID).Select(y => y.FirstOrDefault()).ToList();
        //        if (DocID != "All")
        //        {                  
        //            foreach (var itm in ListL.Where(W => W.DocumentID == DocID))
        //            {
        //                Structures.ComboBoxItem cbitem = new Structures.ComboBoxItem(itm.OfficeName, itm.OfficeID);
        //                cmbOfficetyp.Items.Add(cbitem);
        //            }
        //        }
        //        else
        //        {
        //            foreach (var itm in ListL)
        //            {
        //                Structures.ComboBoxItem cbitem = new Structures.ComboBoxItem(itm.OfficeName, itm.OfficeID);
        //                cmbOfficetyp.Items.Add(cbitem);
        //            }
        //        }
        //        Structures.ComboBoxItem cbitem2 = new Structures.ComboBoxItem("All", "All");               
        //        cmbOfficetyp.Items.Add(cbitem2);
        //        cmbOfficetyp.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbOfficetyp, "All");
        //    }

        //}
    }
}


