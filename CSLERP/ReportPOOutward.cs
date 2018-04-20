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
    public partial class ReportPOOutward : System.Windows.Forms.Form
    {
        string docID = "";
        string Documenttype = "";
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
        public ReportPOOutward()
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

            Structures.ComboBoxItem cbitem = new Structures.ComboBoxItem("All", "All");
            docID = Main.currentDocument;
            CustomerDB.fillCustomerComboNew(cmbCustomer);
            cmbCustomer.Items.Add("All");

            cmbCustomer.SelectedItem = "All";
            cmbTrack.SelectedIndex = 0;
            cmbType.SelectedItem = "All";


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
            cmbCustomer.TabIndex = 0;
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

        //private string getPODocIDForSelectedIO(string iodocid)
        //{
        //    string podocid = "";
        //    switch (docID)
        //    {

        //        case "PRODUCTEXPORTINVOICEOUT":
        //            podocid = "POPRODUCTINWARD";
        //            break;
        //        case "SERVICEEXPORTINVOICEOUT":
        //            podocid = "POSERVICEINWARD";
        //            break;
        //        case "PRODUCTINVOICEOUT":
        //            podocid = "POPRODUCTINWARD";
        //            break;
        //        case "SERVICEINVOICEOUT":
        //            podocid = "POSERVICEINWARD";
        //            break;
        //        default:
        //            podocid = "";
        //            break;
        //    }
        //    return podocid;
        //}

        private void showStockPOQuantity(poheader poph)
        {

            try
            {
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.Manual;
                frmPopup.BackColor = Color.CadetBlue;
                frmPopup.BackColor = Color.White;

                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(900, 400);
                frmPopup.Location = new Point(300, 200);

                //lv = POPIHeaderDB.getPONoWiseStockListView(Convert.ToInt32(txtPOTrackNo.Text), dtPOTrackDate.Value);
                DataGridView grdDetail = new DataGridView();
                grdDetail.AllowUserToAddRows = false;
                if(poph.DocumentID== "PURCHASEORDER")
                {
                    grdDetail = fillgrddetail(poph);
                }
               else if(poph.DocumentID== "WORKORDER")
                {
                    workorderheader woh = new workorderheader();
                    woh.DocumentID = poph.DocumentID;
                    woh.TemporaryNo = poph.TemporaryNo;
                    woh.TemporaryDate = poph.TemporaryDate;
                    grdDetail =fillgrddetailforWO(woh);
                }
                else if(poph.DocumentID== "POGENERAL")
                {
                    pogeneralheader pogh = new pogeneralheader();
                    pogh.DocumentID = poph.DocumentID;
                    pogh.TemporaryNo = poph.TemporaryNo;
                    pogh.TemporaryDate = poph.TemporaryDate;
                    grdDetail =fillgrddetailforPOGeneral(pogh);
                }
                //--------------
                grdDetail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdDetail.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdDetail.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grdDetail.RowHeadersVisible = false;
                grdDetail.AllowUserToAddRows = false;

                grdDetail.Bounds = new Rectangle(new Point(20, 60), new Size(850, 300));
                frmPopup.Controls.Add(grdDetail);

                Label lblItemCode = new Label();
                lblItemCode.Width = 500;
                lblItemCode.Text = "Tracking No:" + poph.PONo + "  Date:" + poph.PODate.ToString("dd-MM-yyyy");
                lblItemCode.Location = new Point(40, 10);
                frmPopup.Controls.Add(lblItemCode);

                Label lblItemName = new Label();
                lblItemName.Width = 500;
                lblItemName.Text = "Customer:" + poph.CustomerName;
                lblItemName.Location = new Point(40, 35);
                frmPopup.Controls.Add(lblItemName);


                Button lvClose = new Button();
                lvClose.BackColor = Color.Tan;
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
        private DataGridView fillgrddetail(poheader poph)
        {
            DataGridView grdDetail = new DataGridView();
            try
            {

                grdDetail.RowHeadersVisible = false;

                DataGridViewTextBoxColumn dgvtx0 = new DataGridViewTextBoxColumn();
                dgvtx0.Width = 50;
                dgvtx0.Name = "SlNo";
                dgvtx0.Visible = true;
                grdDetail.Columns.Add(dgvtx0);

                DataGridViewTextBoxColumn dgvtx1 = new DataGridViewTextBoxColumn();
                dgvtx1.Name = "LineNo";
                dgvtx1.Visible = false;
                grdDetail.Columns.Add(dgvtx1);

                DataGridViewTextBoxColumn dgvtx2 = new DataGridViewTextBoxColumn();
                dgvtx2.Width = 200;
                dgvtx2.Name = "Product";
                grdDetail.Columns.Add(dgvtx2);

                DataGridViewTextBoxColumn dgvtx3 = new DataGridViewTextBoxColumn();
                dgvtx3.Width = 200;
                dgvtx3.Name = "Description";
                grdDetail.Columns.Add(dgvtx3);

                DataGridViewTextBoxColumn dgvtx4 = new DataGridViewTextBoxColumn();
                dgvtx4.Width = 80;
                dgvtx4.Name = "MRNNo";
                grdDetail.Columns.Add(dgvtx4);

                DataGridViewTextBoxColumn dgvtx5 = new DataGridViewTextBoxColumn();
                dgvtx5.Width = 100;
                dgvtx5.Name = "MRNDate";
                grdDetail.Columns.Add(dgvtx5);

                DataGridViewTextBoxColumn dgvtx7 = new DataGridViewTextBoxColumn();
                dgvtx7.Width = 70;
                dgvtx7.DefaultCellStyle.Format = "N0";
                dgvtx7.Name = "Quantity";
                grdDetail.Columns.Add(dgvtx7);

                DataGridViewTextBoxColumn dgvtx8 = new DataGridViewTextBoxColumn();
                dgvtx8.Width = 70;
                dgvtx8.DefaultCellStyle.Format = "N2";
                dgvtx8.Name = "Price";
                grdDetail.Columns.Add(dgvtx8);

                DataGridViewTextBoxColumn dgvtx9 = new DataGridViewTextBoxColumn();
                dgvtx9.DefaultCellStyle.Format = "N2";
                dgvtx9.Name = "Value";
                grdDetail.Columns.Add(dgvtx9);

                DataGridViewTextBoxColumn dgvtx10 = new DataGridViewTextBoxColumn();
                dgvtx10.Width = 70;
                dgvtx10.DefaultCellStyle.Format = "N0";
                dgvtx10.Name = "BilledQty";
                dgvtx10.HeaderText = "Recived Qty";
                grdDetail.Columns.Add(dgvtx10);

                DataGridViewTextBoxColumn dgvtx11 = new DataGridViewTextBoxColumn();
                dgvtx11.Width = 70;
                dgvtx11.DefaultCellStyle.Format = "N0";
                dgvtx11.Name = "BalanceQty";
                dgvtx11.HeaderText = "Balance Qty";
                grdDetail.Columns.Add(dgvtx11);

                {
                    List<podetail> POPIDetail = PurchaseOrderDB.getPOPIDetailqty2(poph);
                    grdDetail.Rows.Clear();

                    var results = from p in POPIDetail
                                  group p by p.RowID into g
                                  select new
                                  {
                                      RowId = g.Key,
                                      StockItemName = g.FirstOrDefault().StockItemName,
                                      Description = g.FirstOrDefault().Description,
                                      Quantity = g.FirstOrDefault().Quantity,
                                      Price = g.FirstOrDefault().Price,
                                      BilledQuantity = g.Sum(S => S.BilledQuantity),
                                      MRNNo = g.Select(S => S.MRNNo).ToArray(),
                                      MRNDate = g.Select(S => S.MRNDate).ToArray()
                                  };


                    int i = 0;
                    foreach (var popid in results)
                    {
                        try
                        {
                            grdDetail.Rows.Add();
                            grdDetail.Rows[i].Cells["SLNo"].Value = grdDetail.RowCount - 1;
                            grdDetail.Rows[i].Cells["LineNo"].Value = popid.RowId;
                            grdDetail.Rows[i].Cells["Product"].Value = popid.StockItemName;
                            grdDetail.Rows[i].Cells["Description"].Value = popid.Description;
                            grdDetail.Rows[i].Cells["MRNNo"].Value = string.Join(",", popid.MRNNo);
                            grdDetail.Rows[i].Cells["MRNDate"].Value = string.Join(",", popid.MRNDate);
                            grdDetail.Rows[i].Cells["Quantity"].Value = popid.Quantity;
                            grdDetail.Rows[i].Cells["Description"].Value = popid.Description;
                            grdDetail.Rows[i].Cells["Price"].Value = popid.Price;
                            grdDetail.Rows[i].Cells["Value"].Value = popid.Quantity * popid.Price;
                            grdDetail.Rows[i].Cells["BilledQty"].Value = popid.BilledQuantity;
                            grdDetail.Rows[i].Cells["BalanceQty"].Value = Convert.ToDouble(popid.Quantity) - Convert.ToDouble(popid.BilledQuantity);
                            i++;
                            // productvalue = productvalue + popid.Quantity * popid.Price;
                            //  taxvalue = taxvalue + popid.Tax;
                        }

                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in PO Quantity filling");
            }
            return grdDetail;
        }


        private DataGridView fillgrddetailforWO(workorderheader WOH)
        {
            DataGridView grdDetail = new DataGridView();
            try
            {

                grdDetail.RowHeadersVisible = false;

                DataGridViewTextBoxColumn dgvtx0 = new DataGridViewTextBoxColumn();
                dgvtx0.Width = 50;
                dgvtx0.Name = "SlNo";
                dgvtx0.Visible = true;
                grdDetail.Columns.Add(dgvtx0);

                DataGridViewTextBoxColumn dgvtx1 = new DataGridViewTextBoxColumn();
                dgvtx1.Name = "LineNo";
                dgvtx1.Visible = false;
                grdDetail.Columns.Add(dgvtx1);

                DataGridViewTextBoxColumn dgvtx2 = new DataGridViewTextBoxColumn();
                dgvtx2.Width = 200;
                dgvtx2.Name = "Product";
                grdDetail.Columns.Add(dgvtx2);

                DataGridViewTextBoxColumn dgvtx3 = new DataGridViewTextBoxColumn();
                dgvtx3.Width = 200;
                dgvtx3.Name = "Description";
                grdDetail.Columns.Add(dgvtx3);

                DataGridViewTextBoxColumn dgvtx4 = new DataGridViewTextBoxColumn();
                dgvtx4.Width = 80;
                dgvtx4.Name = "WorkDescription";
                grdDetail.Columns.Add(dgvtx4);

                DataGridViewTextBoxColumn dgvtx5 = new DataGridViewTextBoxColumn();
                dgvtx5.Width = 100;
                dgvtx5.Name = "WorkLocation";
                grdDetail.Columns.Add(dgvtx5);

                DataGridViewTextBoxColumn dgvtx7 = new DataGridViewTextBoxColumn();
                dgvtx7.Width = 70;
                dgvtx7.DefaultCellStyle.Format = "N0";
                dgvtx7.Name = "Quantity";
                grdDetail.Columns.Add(dgvtx7);

                DataGridViewTextBoxColumn dgvtx8 = new DataGridViewTextBoxColumn();
                dgvtx8.Width = 70;
                dgvtx8.DefaultCellStyle.Format = "N2";
                dgvtx8.Name = "Price";
                grdDetail.Columns.Add(dgvtx8);

                DataGridViewTextBoxColumn dgvtx9 = new DataGridViewTextBoxColumn();
                dgvtx9.DefaultCellStyle.Format = "N2";
                dgvtx9.Name = "WarrantyDays";
                grdDetail.Columns.Add(dgvtx9);

                DataGridViewTextBoxColumn dgvtx10 = new DataGridViewTextBoxColumn();
                dgvtx10.Width = 70;
                dgvtx10.DefaultCellStyle.Format = "N0";
                dgvtx10.Name = "Tax";
                dgvtx10.HeaderText = "Tax";
                grdDetail.Columns.Add(dgvtx10);

                DataGridViewTextBoxColumn dgvtx11 = new DataGridViewTextBoxColumn();
                dgvtx11.Width = 70;
                dgvtx11.DefaultCellStyle.Format = "N0";
                dgvtx11.Name = "TotalAmount";
                dgvtx11.HeaderText = "Total Amount";
                grdDetail.Columns.Add(dgvtx11);

                {
                    List<workorderdetail> WODetail = WorkOrderDB.getWorkOrderDetails(WOH);
                    grdDetail.Rows.Clear();

                    var results = from p in WODetail
                                  group p by p.RowID into g
                                  select new
                                  {
                                      RowId = g.Key,
                                      StockItemName = g.FirstOrDefault().StockItemID,
                                      Description = g.FirstOrDefault().Description,
                                      Quantity = g.FirstOrDefault().Quantity,
                                      Price = g.FirstOrDefault().Price,
                                      Tax=g.FirstOrDefault().Tax,
                                      Warrantydays=g.FirstOrDefault().WarrantyDays,
                                      WorkLocation=g.FirstOrDefault().WorkLocation,
                                      WorkDescription=g.FirstOrDefault().WorkDescription
                                  };


                    int i = 0;
                    foreach (var WOid in results)
                    {
                        try
                        {
                            grdDetail.Rows.Add();
                            grdDetail.Rows[i].Cells["SLNo"].Value = grdDetail.RowCount - 1;
                            grdDetail.Rows[i].Cells["LineNo"].Value = WOid.RowId;
                            grdDetail.Rows[i].Cells["Product"].Value = WOid.StockItemName;
                            grdDetail.Rows[i].Cells["Description"].Value = WOid.Description;
                            grdDetail.Rows[i].Cells["WorkDescription"].Value =WOid.WorkDescription;
                            grdDetail.Rows[i].Cells["WorkLocation"].Value = WOid.WorkLocation;
                            grdDetail.Rows[i].Cells["Quantity"].Value = WOid.Quantity;
                            grdDetail.Rows[i].Cells["Price"].Value = WOid.Price;
                            grdDetail.Rows[i].Cells["Tax"].Value = WOid.Tax;
                            grdDetail.Rows[i].Cells["WarrantyDays"].Value = WOid.Warrantydays;
                            grdDetail.Rows[i].Cells["TotalAmount"].Value = Convert.ToDouble(WOid.Quantity) * Convert.ToDouble(WOid.Price) + Convert.ToDouble(WOid.Tax);
                            i++;
                            // productvalue = productvalue + popid.Quantity * popid.Price;
                            //  taxvalue = taxvalue + popid.Tax;
                        }

                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in PO Quantity filling");
            }
            return grdDetail;
        }

        private DataGridView fillgrddetailforPOGeneral(pogeneralheader poph)
        {
            DataGridView grdDetail = new DataGridView();
            try
            {

                grdDetail.RowHeadersVisible = false;

                DataGridViewTextBoxColumn dgvtx0 = new DataGridViewTextBoxColumn();
                dgvtx0.Width = 50;
                dgvtx0.Name = "SlNo";
                dgvtx0.Visible = true;
                grdDetail.Columns.Add(dgvtx0);

                DataGridViewTextBoxColumn dgvtx1 = new DataGridViewTextBoxColumn();
                dgvtx1.Name = "LineNo";
                dgvtx1.Visible = false;
                grdDetail.Columns.Add(dgvtx1);

                DataGridViewTextBoxColumn dgvtx2 = new DataGridViewTextBoxColumn();
                dgvtx2.Width = 200;
                dgvtx2.Name = "Product";
                grdDetail.Columns.Add(dgvtx2);

                DataGridViewTextBoxColumn dgvtx3 = new DataGridViewTextBoxColumn();
                dgvtx3.Width = 200;
                dgvtx3.Name = "Description";
                grdDetail.Columns.Add(dgvtx3);

                DataGridViewTextBoxColumn dgvtx4 = new DataGridViewTextBoxColumn();
                dgvtx4.Width = 80;
                dgvtx4.Name = "TaxCode";
                grdDetail.Columns.Add(dgvtx4);

                DataGridViewTextBoxColumn dgvtx5 = new DataGridViewTextBoxColumn();
                dgvtx5.Width = 100;
                dgvtx5.Name = "WorkDescription";
                grdDetail.Columns.Add(dgvtx5);

                DataGridViewTextBoxColumn dgvtx7 = new DataGridViewTextBoxColumn();
                dgvtx7.Width = 70;
                dgvtx7.DefaultCellStyle.Format = "N0";
                dgvtx7.Name = "Quantity";
                grdDetail.Columns.Add(dgvtx7);

                DataGridViewTextBoxColumn dgvtx8 = new DataGridViewTextBoxColumn();
                dgvtx8.Width = 70;
                dgvtx8.DefaultCellStyle.Format = "N2";
                dgvtx8.Name = "Price";
                grdDetail.Columns.Add(dgvtx8);

                DataGridViewTextBoxColumn dgvtx9 = new DataGridViewTextBoxColumn();
                dgvtx9.DefaultCellStyle.Format = "N2";
                dgvtx9.Name = "Tax";
                grdDetail.Columns.Add(dgvtx9);

                DataGridViewTextBoxColumn dgvtx10 = new DataGridViewTextBoxColumn();
                dgvtx10.Width = 70;
                dgvtx10.DefaultCellStyle.Format = "N0";
                dgvtx10.Name = "WarrantyDays";
                dgvtx10.HeaderText = "Warranty Days";
                grdDetail.Columns.Add(dgvtx10);

                DataGridViewTextBoxColumn dgvtx11 = new DataGridViewTextBoxColumn();
                dgvtx11.Width = 70;
                dgvtx11.DefaultCellStyle.Format = "N0";
                dgvtx11.Name = "TotalAmount";
                dgvtx11.HeaderText = "Total Amount";
                grdDetail.Columns.Add(dgvtx11);

                {
                    List<pogeneraldetail> POGDetail = PurchaseOrderGeneralDB.getpogeneraldetails(poph);
                    grdDetail.Rows.Clear();

                    var results = from p in POGDetail
                                  group p by p.RowID into g
                                  select new
                                  {
                                      RowId = g.Key,
                                      StockItemName = g.FirstOrDefault().ServiceItemID,
                                      Description = g.FirstOrDefault().Description,
                                      WorkDescription = g.FirstOrDefault().WorkDescription,
                                      TaxCode = g.FirstOrDefault().TaxCode,
                                      Quantity = g.FirstOrDefault().Quantity,
                                      Price = g.FirstOrDefault().Price,
                                      Tax = g.FirstOrDefault().Tax,
                                      WarrantyDays = g.FirstOrDefault().WarrantyDays
                                  };


                    int i = 0;
                    foreach (var popid in results)
                    {
                        try
                        {
                            grdDetail.Rows.Add();
                            grdDetail.Rows[i].Cells["SLNo"].Value = grdDetail.RowCount - 1;
                            grdDetail.Rows[i].Cells["LineNo"].Value = popid.RowId;
                            grdDetail.Rows[i].Cells["Product"].Value = popid.StockItemName;
                            grdDetail.Rows[i].Cells["Description"].Value = popid.Description;
                            grdDetail.Rows[i].Cells["TaxCode"].Value = popid.TaxCode;
                            grdDetail.Rows[i].Cells["WorkDescription"].Value = popid.WorkDescription;
                            grdDetail.Rows[i].Cells["Quantity"].Value = popid.Quantity;
                            grdDetail.Rows[i].Cells["Price"].Value = popid.Price;
                            grdDetail.Rows[i].Cells["Tax"].Value = popid.Tax;
                            grdDetail.Rows[i].Cells["WarrantyDays"].Value = popid.WarrantyDays;
                            grdDetail.Rows[i].Cells["TotalAmount"].Value = Convert.ToDouble(popid.Quantity) * Convert.ToDouble(popid.Price)+ Convert.ToDouble(popid.Tax);
                            i++;
                            // productvalue = productvalue + popid.Quantity * popid.Price;
                            //  taxvalue = taxvalue + popid.Tax;
                        }

                        catch (Exception ex)
                        {

                        }
                    }
                }
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

                if (columnName.Equals("QtyDetails") || columnName.Equals("ClosePO"))
                {
                    poheader poph = new poheader();
                    int totalvalue = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["TotalValueBilled"].Value);
                    poph.DocumentID= grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    poph.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    poph.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    poph.PODate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTrackingDate"].Value.ToString());
                    poph.PONo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTrackingNo"].Value.ToString());
                    poph.CustomerName = grdList.Rows[e.RowIndex].Cells["gCustomerName"].Value.ToString();

                    if (columnName.Equals("QtyDetails"))
                    {
                        showStockPOQuantity(poph);
                    }
                    if (columnName.Equals("ClosePO"))
                    {
                        if(poph.DocumentID== "PURCHASEORDER")
                        {
                            PurchaseOrderDB popihdb = new PurchaseOrderDB();
                            if (popihdb.ClosePOPICheck(poph))
                            {
                                MessageBox.Show("PO Closed");
                                btnView.PerformClick();
                            }
                            else
                            {
                                string dstr = "PO No:" + poph.PONo + "\n" +
                                    "PO Date:" + poph.PODate.ToString("dd-MM-yyyy") + "\n" +
                                    "Customer:" + poph.CustomerName + "\n" +
                                    "Are you sure to Close the document ?";
                                DialogResult dialog = MessageBox.Show(dstr, "Close PO", MessageBoxButtons.YesNo);
                                if (dialog == DialogResult.Yes)
                                {

                                    if (popihdb.ClosePOPI2(poph))
                                    {
                                        MessageBox.Show("PO Closed");
                                        btnView.PerformClick();
                                    }
                                }
                            }
                        }
                        else if(poph.DocumentID == "POGENERAL")
                        {
                            PurchaseOrderGeneralDB popihdb = new PurchaseOrderGeneralDB();
                            pogeneralheader pog = new pogeneralheader();
                            pog.DocumentID = poph.DocumentID;
                            pog.PONo = poph.PONo;
                            pog.PODate = poph.PODate;
                            if (popihdb.ClosePOCheck(pog))
                            {
                                MessageBox.Show("PO Closed");
                                btnView.PerformClick();
                            }
                            else
                            {
                                string dstr = "PO No:" + poph.PONo + "\n" +
                                    "PO Date:" + poph.PODate.ToString("dd-MM-yyyy") + "\n" +
                                    "Customer:" + poph.CustomerName + "\n" +
                                    "Are you sure to Close the document ?";
                                DialogResult dialog = MessageBox.Show(dstr, "Close PO", MessageBoxButtons.YesNo);
                                if (dialog == DialogResult.Yes)
                                {

                                    if (popihdb.ClosePO(pog))
                                    {
                                        MessageBox.Show("PO Closed");
                                        btnView.PerformClick();
                                    }
                                }
                            }
                        }
                        else if(poph.DocumentID == "WORKORDER")
                        {
                            WorkOrderDB popihdb = new WorkOrderDB();
                            workorderheader woh = new workorderheader();
                            woh.DocumentID = poph.DocumentID;
                            woh.WONo = poph.PONo;
                            woh.WODate = poph.PODate;
                            if (popihdb.CloseWOCheck(woh))
                            {
                                MessageBox.Show("WO Closed");
                                btnView.PerformClick();
                            }
                            else
                            {
                                string dstr = "WO No:" + poph.PONo + "\n" +
                                    "WO Date:" + poph.PODate.ToString("dd-MM-yyyy") + "\n" +
                                    "Customer:" + poph.CustomerName + "\n" +
                                    "Are you sure to Close the document ?";
                                DialogResult dialog = MessageBox.Show(dstr, "Close PO", MessageBoxButtons.YesNo);
                                if (dialog == DialogResult.Yes)
                                {
                                    if (popihdb.CloseWO(woh))
                                    {
                                        MessageBox.Show("WO Closed");
                                        btnView.PerformClick();
                                    }
                                }
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
                    btnExportToExcel.Visible = true;

                    if (cmbTrack.SelectedItem.ToString() == "Closed")
                    {
                        grdList.Columns["ClosePO"].Visible = false;
                    }
                    else
                    {
                        int prv = getuserPrivilegeStatus();
                        if (prv == 2)
                        {
                            grdList.Columns["ClosePO"].Visible = true;
                        }
                        else
                        {
                            grdList.Columns["ClosePO"].Visible = false;
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
                    btnExportToExcel.Visible = true;

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
            //------
            double totalPOValue = 0.0;
            double totalInvoiceValue = 0.0;
            double balanceAmount = 0.0, Invoiceamt = 0.0, Paymntamunt = 0.0;

            //------
            //pnlAddEdit.Visible = true;
            setButtonVisibility("Edit");
            try
            {
                StringBuilder query = new StringBuilder();
                if (cmbType.SelectedItem.ToString() != "All")
                {
                    if(cmbType.SelectedItem.ToString()=="Product")
                    {
                        query.Append("select DocumentID,TemporaryNo,TemporaryDate,PONo,PODate,CustomerName,ProductValueINR from ViewPOHeader ");
                    }
                    else if(cmbType.SelectedItem.ToString()=="General")
                    {
                        query.Append("select DocumentID,TemporaryNo,TemporaryDate,PONo,PODate,CustomerName,ProductValueINR from ViewPOGeneral ");
                    }
                    else if (cmbType.SelectedItem.ToString() == "WorkOrder")
                    {
                        query.Append("select DocumentID,TemporaryNo,TemporaryDate,WONo,WODate,CustomerName,ServiceValueINR from ViewWorkOrder ");
                    }

                     if (cmbTrack.SelectedIndex != -1)
                        {
                            string str = "";
                            if (cmbType.SelectedItem.ToString() == "WorkOrder")
                            {
                                str = "where WODate >= '" + dtFrom.Value.ToString("yyyy-MM-dd") + "' and WODate <='" + dtTo.Value.ToString("yyyy-MM-dd") + "'";
                                if (cmbTrack.SelectedItem.ToString() == "Open")
                                {
                                    str += "and Status=1 and DocumentStatus = 99";
                                }
                                else if (cmbTrack.SelectedItem.ToString() == "Closed")
                                {
                                    str += " and Status=7 and DocumentStatus = 99 ";
                                }
                                else
                                {
                                    str += "and Status in (1,7) and DocumentStatus = 99 ";
                                }

                            }
                            else
                            {
                                str = "where PODate >= '" + dtFrom.Value.ToString("yyyy-MM-dd") + "' and PODate <='" + dtTo.Value.ToString("yyyy-MM-dd") + "'";
                                if (cmbTrack.SelectedItem.ToString() == "Open")
                                {
                                    str += "and Status=1 and DocumentStatus = 99";
                                }
                                else if (cmbTrack.SelectedItem.ToString() == "Closed")
                                {
                                    str += " and Status=7 and DocumentStatus = 99";
                                }
                                else
                                {
                                    str += "and Status in (1,7) and DocumentStatus = 99 ";
                                }
                            }

                            query.Append(str);
                        }
                    if (cmbCustomer.SelectedItem.ToString() != "All" )
                    {
                        query.Append(" and CustomerID='" + ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue + "'");
                    }
                }
                else
                {
                    if(cmbTrack.SelectedItem.ToString() == "All" )
                    {
                        query.Append("select DocumentID,TemporaryNo,TemporaryDate,PONo,PODate,CustomerName,ProductValueINR from ViewPOHeader " +
                                    "where PODate >= '" + dtFrom.Value.ToString("yyyy-MM-dd") + "' and PODate <='" + dtTo.Value.ToString("yyyy-MM-dd") + "' and Status in (1,7) and DocumentStatus = 99 ");
                        if (cmbCustomer.SelectedItem.ToString() != "All")
                        {
                            query.Append(" and CustomerID='" + ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue + "'");
                        }
                        query.Append(" union select DocumentID,TemporaryNo,TemporaryDate,PONo,PODate,CustomerName,ProductValueINR from ViewPOGeneral" +
                                   " where PODate >= '" + dtFrom.Value.ToString("yyyy-MM-dd") + "' and PODate <='" + dtTo.Value.ToString("yyyy-MM-dd") + "' and Status in (1,7) and DocumentStatus = 99 ");
                                    if (cmbCustomer.SelectedItem.ToString() != "All")
                        {
                            query.Append(" and CustomerID='" + ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue + "'");
                        }
                        query.Append(" union select DocumentID, TemporaryNo, TemporaryDate, WONo, WODate, CustomerName,ServiceValueINR from ViewWorkOrder " +
                                      " where WODate >= '" + dtFrom.Value.ToString("yyyy-MM-dd") + "' and WODate <='" + dtTo.Value.ToString("yyyy-MM-dd") + "' and Status in (1,7) and DocumentStatus = 99");
                        if (cmbCustomer.SelectedItem.ToString() != "All")
                        {
                            query.Append(" and CustomerID='" + ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue + "'");
                        }

                    }
                    else
                    {
                        query.Append("select DocumentID,TemporaryNo,TemporaryDate,PONo,PODate,CustomerName,ProductValueINR from ViewPOHeader ");
                        if (cmbTrack.SelectedIndex != -1)
                        {
                            string str = "";                            
                            
                                str = "where PODate >= '" + dtFrom.Value.ToString("yyyy-MM-dd") + "' and PODate <='" + dtTo.Value.ToString("yyyy-MM-dd") + "'";
                                if (cmbTrack.SelectedItem.ToString() == "Open")
                                {
                                    str += "and Status=1 and DocumentStatus = 99";
                                }
                                else if (cmbTrack.SelectedItem.ToString() == "Closed")
                                {
                                    str += " and Status=7 and DocumentStatus = 99";
                                }
                                else
                                {
                                    str += "and Status in (1,7) and DocumentStatus = 99 ";
                                }
                            

                            query.Append(str);
                        }
                        if (cmbCustomer.SelectedItem.ToString() != "All" && cmbTrack.SelectedItem.ToString() != "All")
                        {
                            query.Append(" and CustomerID='" + ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue + "'");
                        }
                        query.Append(" union select DocumentID,TemporaryNo,TemporaryDate,PONo,PODate,CustomerName,ProductValueINR from ViewPOGeneral ");
                        if (cmbTrack.SelectedIndex != -1)
                        {
                            string str = "";
                            
                                str = "where PODate >= '" + dtFrom.Value.ToString("yyyy-MM-dd") + "' and PODate <='" + dtTo.Value.ToString("yyyy-MM-dd") + "'";
                                if (cmbTrack.SelectedItem.ToString() == "Open")
                                {
                                    str += "and Status=1 and DocumentStatus = 99";
                                }
                                else if (cmbTrack.SelectedItem.ToString() == "Closed")
                                {
                                    str += " and Status=7 and DocumentStatus = 99";
                                }
                                else
                                {
                                    str += "and Status in (1,7) and DocumentStatus = 99 ";
                                }                            

                            query.Append(str);
                        }
                        if (cmbCustomer.SelectedItem.ToString() != "All" && cmbTrack.SelectedItem.ToString() != "All")
                        {
                            query.Append(" and CustomerID='" + ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue + "'");
                        }
                        query.Append(" union select DocumentID, TemporaryNo, TemporaryDate, WONo, WODate,CustomerName, ServiceValueINR from ViewWorkOrder ");
                        if (cmbTrack.SelectedIndex != -1)
                        {
                            string str = "";
                                str = "where WODate >= '" + dtFrom.Value.ToString("yyyy-MM-dd") + "' and WODate <='" + dtTo.Value.ToString("yyyy-MM-dd") + "'";
                                if (cmbTrack.SelectedItem.ToString() == "Open")
                                {
                                    str += "and Status=1 and DocumentStatus = 99";
                                }
                                else if (cmbTrack.SelectedItem.ToString() == "Closed")
                                {
                                    str += " and Status=7 and DocumentStatus = 99 ";
                                }
                                else
                                {
                                    str += "and Status in (1,7) and DocumentStatus = 99 ";
                                }
                            query.Append(str);
                        }
                        if (cmbCustomer.SelectedItem.ToString() != "All" && cmbTrack.SelectedItem.ToString() != "All")
                        {
                            query.Append(" and CustomerID='" + ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue + "'");
                        }
                    }                   
                }
                PurchaseOrderDB popihdb = new PurchaseOrderDB();
                InvoiceInHeaderDB InvoDb = new InvoiceInHeaderDB();
                PaymentVoucherDB PaymtvDB = new PaymentVoucherDB();
                List<poheader> POPIHeaders = popihdb.ListPopiFilters2(query.ToString());
                List<MRNHeader2> pop = popihdb.getMRNHeaderdata();
                List<invoiceinheader> Invoices = InvoDb.getInvoicedata();
                List<paymentvoucher> Payments = PaymtvDB.getPaymentdata();
                foreach (poheader popih in POPIHeaders)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = popih.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = popih.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = popih.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTrackingNo"].Value = popih.PONo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTrackingDate"].Value = popih.PODate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerName"].Value = popih.CustomerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["POValueINR"].Value = popih.ProductValueINR;
                    grdList.Rows[grdList.RowCount - 1].Cells["TotalValueBilled"].Value = 0;
                    totalPOValue += popih.ProductValueINR;
                    if (popih.PONo != 0 && popih.PODate != DateTime.MinValue)
                    {
                        double iamt = 0;
                        if (popih.DocumentID == "PURCHASEORDER")
                        {
                           var  results = pop.Where(oh => oh.PONos.Contains("" + popih.PONo + "") && oh.PODates.Contains("" + popih.PODate.ToString("yyyy-MM-dd") + "")).ToList();
                            int[] MRNNos = results.Select(i => i.MRNNo).ToArray();
                            DateTime[] MrnDats = results.Select(i => i.MRNDate).ToArray();
                            var AVC = from x in Invoices
                                      where MRNNos.Contains(x.MRNNo) && MrnDats.Contains(x.MRNDate)
                                      select x;
                            string[] ABCCD = AVC.Select(i => i.DocumentID.ToString() + Main.delimiter1 + i.DocumentNo.ToString() + Main.delimiter1 + i.DocumentDate.ToString("yyyy-MM-dd") + Main.delimiter2).ToArray();
                            var AVC2 = from x in Payments
                                       where ABCCD.Contains(x.BillDetails)
                                       select x;

                             iamt = results.Sum(item => item.MRNValue);
                            Invoiceamt = AVC.Sum(item => item.InvoiceValueINR);
                            Paymntamunt = AVC2.Sum(item => Convert.ToDouble(item.VoucherAmountINR));
                        }
                        else if(popih.DocumentID== "POGENERAL" || popih.DocumentID== "WORKORDER")
                        {
                            var results = Invoices.Where(oh => oh.MRNNo==popih.PONo && oh.MRNDate.ToString("yyyy-MM-dd") == popih.PODate.ToString("yyyy-MM-dd")).ToList();                           
                            string[] ABCCD = results.Select(i => i.DocumentID.ToString() + Main.delimiter1 + i.DocumentNo.ToString() + Main.delimiter1 + i.DocumentDate.ToString("yyyy-MM-dd") + Main.delimiter2).ToArray();
                            var AVC2 = from x in Payments
                                       where ABCCD.Contains(x.BillDetails)
                                       select x;
                            iamt= results.Sum(item => item.ProductValueINR);
                            Invoiceamt = results.Sum(item => item.InvoiceValueINR);
                            Paymntamunt = AVC2.Sum(item => Convert.ToDouble(item.VoucherAmountINR));
                        }
                        
                        grdList.Rows[grdList.RowCount - 1].Cells["TotalValueBilled"].Value = iamt;
                        totalInvoiceValue += iamt;
                    }
                    balanceAmount = Convert.ToDouble(grdList.Rows[grdList.RowCount - 1].Cells["POValueINR"].Value) - Convert.ToDouble(grdList.Rows[grdList.RowCount - 1].Cells["TotalValueBilled"].Value);
                    grdList.Rows[grdList.RowCount - 1].Cells["BalanceAmount"].Value = balanceAmount;
                    grdList.Rows[grdList.RowCount - 1].Cells["InvoiceAmt"].Value = Invoiceamt;
                    grdList.Rows[grdList.RowCount - 1].Cells["Payamt"].Value = Paymntamunt;
                    grdList.Rows[grdList.RowCount - 1].Cells["AmtBalance"].Value = Invoiceamt - Paymntamunt; ;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in PO Product Inward Report Listing");
                grdList.Rows.Clear();
            }
            if (grdList.Rows.Count <= 0)
            {
                btnExportToExcel.Visible = false;
                grdList.Visible = false;
                pnlTotal.Visible = false;
                MessageBox.Show("No Data to Show");
            }
            else
            {
                lblPOValue.Text = Math.Round(totalPOValue / 100000, 2).ToString() + " Lakhs";
                lblBilledValue.Text = Math.Round(totalInvoiceValue / 100000, 2).ToString() + " Lakhs";
                lblBalanceValue.Text = Math.Round((totalPOValue - totalInvoiceValue) / 100000, 2).ToString() + " Lakhs";
                lblPOCount.Text = " PO Count:" + grdList.Rows.Count;
                pnlTotal.Visible = true;
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
            pnlTotal.Visible = false;
            btnExportToExcel.Visible = false;
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
            pnlTotal.Visible = false;
            btnExportToExcel.Visible = false;
        }

        private void cmbCustomer_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            grdList.Visible = false;
            pnlTotal.Visible = false;
            btnExportToExcel.Visible = false;
        }

        private void cmbProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
            pnlTotal.Visible = false;
            btnExportToExcel.Visible = false;
        }

        private void cmbTrack_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
            pnlTotal.Visible = false;
            btnExportToExcel.Visible = false;
        }

        private void ReportPOOutward_Enter(object sender, EventArgs e)
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

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmbType.SelectedItem.ToString() == "All")
            //{
            //    Documenttype = "All";
            //}
            //else if (cmbType.SelectedItem.ToString() == "Product")
            //{
            //    Documenttype = "POINVOICEIN";
            //}
            //else if (cmbType.SelectedItem.ToString() == "General")
            //{
            //    Documenttype = "POGENERALINVOICEIN";
            //}
            //else if (cmbType.SelectedItem.ToString() == "WorkOrder")
            //{
            //    Documenttype = "WOINVOICEIN";
            grdList.Visible = false;
            pnlTotal.Visible = false;
            btnExportToExcel.Visible = false;
            //}
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


