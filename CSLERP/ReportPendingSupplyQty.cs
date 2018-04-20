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
    public partial class ReportPendingSupplyQty : System.Windows.Forms.Form
    {
        Form frmPopup = new Form();
        ListView exlv = new ListView();// list view for choice / selection list
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
        Timer filterTimer = new Timer();
        public ReportPendingSupplyQty()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void Report_Load(object sender, EventArgs e)
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
            applyPrivilege();
            closeAllPanels();
            ListQtyDetails();
            
        }
        private void ListQtyDetails()
        {
            
            try
            {
                pnlList.Visible = true;
                grdList.Visible = true;
                grdList.Rows.Clear();
                ReportPOvsInvoiceQtyDB sdb = new ReportPOvsInvoiceQtyDB();
                List<reportpendingsupplyqty> qtylist = ReportPendingSupplyDB.QtyList();
                List<TotalStock> tStock = StockDB.TotalStock(Main.MainStore);
                List<reportpendingsupplyqty> PendingSupList = ReportPendingSupplyDB.getPendingSupplyList();
                foreach (reportpendingsupplyqty poqty in qtylist)
                {
                    if (poqty.BalanceQty > 0)
                    {
                        try
                        {
                            grdList.Rows.Add();
                            grdList.Rows[grdList.RowCount - 1].Cells["StockItemID"].Value = poqty.StockItemID;
                            grdList.Rows[grdList.RowCount - 1].Cells["StockItemName"].Value = poqty.StockItemName;
                            grdList.Rows[grdList.RowCount - 1].Cells["POQty"].Value = poqty.POQty;
                            grdList.Rows[grdList.RowCount - 1].Cells["BilledQty"].Value = poqty.BilledQty;
                            grdList.Rows[grdList.RowCount - 1].Cells["BalanceQty"].Value = poqty.BalanceQty;
                            
                            List<reportpendingsupplyqty> pList = PendingSupList.Where(x => x.StockItemID == poqty.StockItemID).ToList();
                            double total = pList.Sum(item => item.BalanceQty);
                            if (total != 0)
                            {
                                grdList.Rows[grdList.RowCount - 1].Cells["PendingPOQty"].Value = total; //For Pending Supply
                            }
                            else
                            {
                                grdList.Rows[grdList.RowCount - 1].Cells["PendingPOQty"].Value = Convert.ToDouble(0);
                            }
                            try
                            {
                                TotalStock tstk = tStock.Single(s => s.StockItemID == poqty.StockItemID);
                                if (tstk != null)
                                {
                                    grdList.Rows[grdList.RowCount - 1].Cells["PresentStock"].Value = tstk.Stock;
                                }
                                if (poqty.BalanceQty <= tstk.Stock)
                                {
                                    grdList.Rows[grdList.RowCount - 1].Cells["PresentStock"].Style.BackColor = Color.Yellow;
                                }
                            }
                            catch (Exception ex)
                            {
                                grdList.Rows[grdList.RowCount - 1].Cells["PresentStock"].Value = Convert.ToDouble(0);
                            }
                            double qtytobeproceured = 0.0;
                            try
                            {
                                qtytobeproceured =
                                Convert.ToDouble(grdList.Rows[grdList.RowCount - 1].Cells["BalanceQty"].Value) -
                                (Convert.ToDouble(grdList.Rows[grdList.RowCount - 1].Cells["PresentStock"].Value) +
                                Convert.ToDouble(grdList.Rows[grdList.RowCount - 1].Cells["PendingPOQty"].Value));
                            }
                            catch (Exception ex)
                            {
                                qtytobeproceured = 0.0;
                            }
                            if (qtytobeproceured > 0)
                            {
                                grdList.Rows[grdList.RowCount - 1].Cells["QtyToBeProcured"].Value = qtytobeproceured;
                            }
                            
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                if(grdList.RowCount>0)
                {
                    txtSearch.Visible = true;
                    lblSearch.Visible = true;
                    btnExportToExcel.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Stock listing");
            }
        }

        private void initVariables()
        {
            pnlList.Visible = false;
            grdList.Visible = false;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
            btnExportToExcel.Visible = false;
            lbldatedisp.Visible = true;
            lbldatedisp.Text = DateTime.Now.Date.ToString("dd-MM-yyyy");
            //dtpDate.Format = DateTimePickerFormat.Custom;
            //dtpDate.CustomFormat = "dd-MM-yyyy";
            //dtpDate.Enabled = true;
            //cmbFilterStock.SelectedIndex = 1;
            setdgvStyle();
        }
        private void applyPrivilege()
        {
            try
            {
                if (Main.itemPriv[1])
                {
                    btnExportToExcel.Visible = true;
                }
                else
                {
                    btnExportToExcel.Visible = false;
                }
                if (Main.itemPriv[2])
                {
                    btnExportToExcel.Visible = true;
                }
                else
                {
                    btnExportToExcel.Visible = false;
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
                pnlList.Visible = false;
              
                btnExportToExcel.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        public void clearUserData()
        {
            try
            {
                //txtSearch.Text = "";
               
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

        //----------------
        private void setdgvStyle()
        {

            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

            this.grdList.AllowUserToAddRows = false;
            this.grdList.AllowUserToDeleteRows = false;
            this.grdList.AllowUserToOrderColumns = true;
            //dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            //this.grdList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.grdList.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.grdList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            
        }
        private DataGridView fillgrddetail(string stockitemid)
        {
            DataGridView grdDetail = new DataGridView();
            try
            {
               
                grdDetail.RowHeadersVisible = false;
                ////DataGridViewLinkColumn dgvlc = new DataGridViewLinkColumn();
                ////dgvlc.SortMode = DataGridViewColumnSortMode.Automatic;
                ////grdDetail.Columns.Add(dgvlc);



                DataGridViewTextBoxColumn dgvtx3 = new DataGridViewTextBoxColumn();
                dgvtx3.Name = "ReferenceNo";
                dgvtx3.Width = 80;
                grdDetail.Columns.Add(dgvtx3);

                DataGridViewTextBoxColumn dgvtx4 = new DataGridViewTextBoxColumn();
                dgvtx4.Name = "CustomerPONo";
                dgvtx4.Width = 80;
                grdDetail.Columns.Add(dgvtx4);

                DataGridViewTextBoxColumn dgvtx5 = new DataGridViewTextBoxColumn();
                dgvtx5.Name = "CustomerPODate";
                dgvtx5.Width = 90;
                grdDetail.Columns.Add(dgvtx5);

                DataGridViewTextBoxColumn dgvtx6 = new DataGridViewTextBoxColumn();
                dgvtx6.Width = 200;
                dgvtx6.Name = "CustomerName";
                grdDetail.Columns.Add(dgvtx6);

                DataGridViewTextBoxColumn dgvtx7 = new DataGridViewTextBoxColumn();
                dgvtx7.Name = "POQty";
                grdDetail.Columns.Add(dgvtx7);

                DataGridViewTextBoxColumn dgvtx8 = new DataGridViewTextBoxColumn();
                dgvtx8.Name = "BilledQty";
                grdDetail.Columns.Add(dgvtx8);

                DataGridViewTextBoxColumn dgvtx9 = new DataGridViewTextBoxColumn();
                dgvtx9.Name = "BalanceQty";
                grdDetail.Columns.Add(dgvtx9);

                DataGridViewTextBoxColumn dgvtx0 = new DataGridViewTextBoxColumn();
                dgvtx0.Name = "DocumentID";
                grdDetail.Columns.Add(dgvtx0);

                DataGridViewTextBoxColumn dgvtx1 = new DataGridViewTextBoxColumn();
                dgvtx1.Name = "TrackingNo";
                dgvtx1.Width = 70;
                grdDetail.Columns.Add(dgvtx1);

                DataGridViewTextBoxColumn dgvtx2 = new DataGridViewTextBoxColumn();
                dgvtx2.Name = "TrackingDate";
                dgvtx2.Width = 80;
                grdDetail.Columns.Add(dgvtx2);
                ////////StockReconcolitationDB sdb = new StockReconcolitationDB();
                List<reportpendingsupplyqty> qtyList = ReportPendingSupplyDB.GetQtyDetail(stockitemid);
                int i = 0;
                foreach (reportpendingsupplyqty qty in qtyList)
                {
                    if (qty.POQty - qty.BilledQty > 0)
                    {
                        grdDetail.Rows.Add();
                        grdDetail.Rows[i].Cells["DocumentID"].Value = qty.DocumentID;
                        grdDetail.Rows[i].Cells["TrackingNo"].Value = qty.TrackingNo;
                        grdDetail.Rows[i].Cells["TrackingDate"].Value = qty.TrackingDate.Date.ToString("dd-MM-yyyy");
                        grdDetail.Rows[i].Cells["ReferenceNo"].Value = qty.ReferenceNo;
                        grdDetail.Rows[i].Cells["CustomerPoNo"].Value = qty.CustomerPoNo;
                        grdDetail.Rows[i].Cells["CustomerPODate"].Value = qty.CustomerPODate.Date.ToString("dd-MM-yyyy");
                        grdDetail.Rows[i].Cells["POQty"].Value = qty.POQty;
                        grdDetail.Rows[i].Cells["BilledQty"].Value = qty.BilledQty;
                        grdDetail.Rows[i].Cells["CustomerName"].Value = qty.CustomerName;
                        i++;
                    }
                }
               
                //btnExit.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Quantity listing");
            }
            return grdDetail;
        }
        private void showQtyDetails(string stockitemid, string stockitemname)
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

                frmPopup.Size = new Size(1010, 400);


                //lv = POPIHeaderDB.getPONoWiseStockListView(Convert.ToInt32(txtPOTrackNo.Text), dtPOTrackDate.Value);
                DataGridView grdDetail = new DataGridView();
                grdDetail = fillgrddetail(stockitemid);

                //--------------
                grdDetail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdDetail.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdDetail.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdDetail.RowHeadersVisible = false;
                grdDetail.AllowUserToAddRows = false;
                //---

                double potot = 0;
                double billedtot = 0;
                for (int i = 0; i < grdDetail.RowCount; i++)
                {
                    potot += Convert.ToDouble(grdDetail.Rows[i].Cells["POQty"].Value);
                    billedtot += Convert.ToDouble(grdDetail.Rows[i].Cells["BilledQty"].Value);
                    grdDetail.Rows[i].Cells["BalanceQty"].Value = 
                        Convert.ToDouble(grdDetail.Rows[i].Cells["POQty"].Value) -
                        Convert.ToDouble(grdDetail.Rows[i].Cells["BilledQty"].Value);
                }
                grdDetail.Rows.Add();
                grdDetail.Rows[grdDetail.RowCount-1].Cells["CustomerName"].Value = "Total";
                ////////grdDetail.Rows[grdDetail.RowCount - 1].Cells["POqty"].Value = potot;
                ////////grdDetail.Rows[grdDetail.RowCount - 1].Cells["BilledQty"].Value = billedtot;
                grdDetail.Rows[grdDetail.RowCount - 1].Cells["BalanceQty"].Value = potot - billedtot;
                
                //ListView 
                // this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked1);
                grdDetail.Bounds = new Rectangle(new Point(0, 60), new Size(1010, 300));
                frmPopup.Controls.Add(grdDetail);

                Label lblItemCode = new Label();
                lblItemCode.BackColor = Color.White;
                lblItemCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblItemCode.AutoSize = true;
                lblItemCode.Text = "Item Code:" + stockitemid;
                lblItemCode.Location= new Point(40, 20);
                frmPopup.Controls.Add(lblItemCode);

                Label lblItemName= new Label();
                lblItemName.BackColor = Color.White;
                lblItemName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblItemName.AutoSize = true;
                lblItemName.Text = "Item Name:" + stockitemname;
                lblItemName.Location = new Point(240, 20);
                frmPopup.Controls.Add(lblItemName);

                ////Button lvOK = new Button();
                ////lvOK.BackColor = Color.Tan;
                ////lvOK.Text = "OK";
                ////lvOK.Location = new Point(40, 365);
                ////lvOK.Click += new System.EventHandler(this.grddetailclickOK);
                ////frmPopup.Controls.Add(lvOK);

                Button lvClose = new Button();
                lvClose.BackColor = Color.Tan;
                lvClose.Text = "CLOSE";
                lvClose.Location = new Point(40, 365);
                lvClose.Click += new System.EventHandler(this.grddetailclickClose);
                frmPopup.Controls.Add(lvClose);
                frmPopup.ShowDialog();

                //pnlAddEdit.Controls.Add(pnllv);
                //pnllv.BringToFront();
                //pnllv.Visible = true;
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
        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
               
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if(columnName.Equals("Details"))
                {
                    clearUserData();
                    ////closeAllPanels();
                    string itemid = grdList.Rows[e.RowIndex].Cells["StockItemID"].Value.ToString();
                    string itemname = grdList.Rows[e.RowIndex].Cells["StockItemName"].Value.ToString();
                    showQtyDetails(itemid, itemname);
                }
                else if (columnName.Equals("DetailsInTransit"))
                {
                    if (Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["PendingPOQty"].Value.ToString()) > 0)
                    {
                        clearUserData();
                        string itemid = grdList.Rows[e.RowIndex].Cells["StockItemID"].Value.ToString();
                        string itemname = grdList.Rows[e.RowIndex].Cells["StockItemName"].Value.ToString();
                        showQtyInTransitDetails(itemid, itemname);
                    }
                }
                else
                {
                    return;
                }
               
            }
            catch(Exception ex)
            {

            }
            }
        private void showQtyInTransitDetails(string stockitemid, string stockitemname)
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

                frmPopup.Size = new Size(900, 400);

                
                DataGridView grdDetail = new DataGridView();
                grdDetail = fillgrddetailForIntransit(stockitemid);

                //--------------
                grdDetail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdDetail.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdDetail.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdDetail.RowHeadersVisible = false;
                grdDetail.AllowUserToAddRows = false;
                //---

                double intranstot = 0;
                for (int i = 0; i < grdDetail.RowCount; i++)
                {
                    intranstot += Convert.ToDouble(grdDetail.Rows[i].Cells["InTransitQty"].Value);
                }
                grdDetail.Rows.Add();
                grdDetail.Rows[grdDetail.RowCount - 1].Cells["CustomerName"].Value = "Total";
                grdDetail.Rows[grdDetail.RowCount - 1].Cells["InTransitQty"].Value = intranstot;

                grdDetail.Bounds = new Rectangle(new Point(0, 60), new Size(900, 300));
                frmPopup.Controls.Add(grdDetail);

                Label lblItemCode = new Label();
                lblItemCode.BackColor = Color.White;
                lblItemCode.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblItemCode.AutoSize = true;
                lblItemCode.Text = "Item Code:" + stockitemid;
                lblItemCode.Location = new Point(40, 20);
                frmPopup.Controls.Add(lblItemCode);

                Label lblItemName = new Label();
                lblItemName.BackColor = Color.White;
                lblItemName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                lblItemName.AutoSize = true;
                lblItemName.Text = "Item Name:" + stockitemname;
                lblItemName.Location = new Point(240, 20);
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
        private DataGridView fillgrddetailForIntransit(string stockitemid)
        {
            DataGridView grdDetail = new DataGridView();
            try
            {

                grdDetail.RowHeadersVisible = false;

                DataGridViewTextBoxColumn dgvtx4 = new DataGridViewTextBoxColumn();
                dgvtx4.Name = "PONo";
                dgvtx4.HeaderText = "PO No";
                dgvtx4.Width = 100;
                grdDetail.Columns.Add(dgvtx4);

                DataGridViewTextBoxColumn dgvtx5 = new DataGridViewTextBoxColumn();
                dgvtx5.Name = "PODate";
                dgvtx5.HeaderText = "PO Date";
                dgvtx5.Width = 100;
                grdDetail.Columns.Add(dgvtx5);

                DataGridViewTextBoxColumn dgvtx10 = new DataGridViewTextBoxColumn();
                dgvtx10.Name = "TargetDate";
                dgvtx10.HeaderText = "Target Date";
                dgvtx10.Width = 100;
                grdDetail.Columns.Add(dgvtx10);

                DataGridViewTextBoxColumn dgvtx6 = new DataGridViewTextBoxColumn();
                dgvtx6.Width = 290;
                dgvtx6.Name = "CustomerName";
                dgvtx6.HeaderText = "Customer";
                grdDetail.Columns.Add(dgvtx6);

                DataGridViewTextBoxColumn dgvtx7 = new DataGridViewTextBoxColumn();
                dgvtx7.Name = "OrderedQty";
                dgvtx7.HeaderText = "Ordered Qty";
                grdDetail.Columns.Add(dgvtx7);

                DataGridViewTextBoxColumn dgvtx8 = new DataGridViewTextBoxColumn();
                dgvtx8.Name = "ReceivedQty";
                dgvtx8.HeaderText = "Received Qty";
                grdDetail.Columns.Add(dgvtx8);

                DataGridViewTextBoxColumn dgvtx9 = new DataGridViewTextBoxColumn();
                dgvtx9.Name = "InTransitQty";
                dgvtx9.HeaderText = "In-Transit Qty";
                grdDetail.Columns.Add(dgvtx9);

                List<reportpendingsupplyqty> qtyList = ReportPendingSupplyDB.GetInTransitQtyDetail(stockitemid);
                int i = 0;
                foreach (reportpendingsupplyqty qty in qtyList)
                {
                    if (qty.POQty - qty.BilledQty > 0) // (Ordered Qunatity - ReceivedQunatity) > 0
                    {
                        grdDetail.Rows.Add();
                        grdDetail.Rows[i].Cells["PONo"].Value = qty.TrackingNo;
                        grdDetail.Rows[i].Cells["PODate"].Value = qty.TrackingDate.Date.ToString("dd-MM-yyyy");
                        grdDetail.Rows[i].Cells["CustomerName"].Value = qty.CustomerName;
                        grdDetail.Rows[i].Cells["TargetDate"].Value = qty.CustomerPODate.Date.ToString("dd-MM-yyyy");

                        grdDetail.Rows[i].Cells["OrderedQty"].Value = qty.POQty; //Ordered Quant
                        grdDetail.Rows[i].Cells["ReceivedQty"].Value = qty.BilledQty; //Received Quant
                        grdDetail.Rows[i].Cells["InTransitQty"].Value = qty.BalanceQty; //Supply Pending Quant

                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Quantity listing");
            }
            return grdDetail;
        }
        private void cmbcmpnysrch_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
            grdList.Rows.Clear();
            pnlList.Visible = false;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
            txtSearch.Text = "";
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
                        if (!row.Cells["StockItemName"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
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

        private void btnView_Click(object sender, EventArgs e)
        {

            //if (cmbFilterStock.SelectedIndex == -1)
            //{
            //    MessageBox.Show("Select Stock Filtering");
            //    return;
            //}

            //ListFilteredStock(getFilterNo());
            //lblSearch.Visible = true;
            //txtSearch.Visible = true;
        }
        //private int getFilterNo()
        //{
        //    //int no = 0;
        //    //if (cmbFilterStock.SelectedItem.ToString().Trim().Equals("All"))
        //    //    no = 1;
        //    //else
        //    //    no = 2;
        //    //return no;
        //}
        private void btnExportToExcel_Click(object sender, EventArgs e)
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

                frmPopup.Size = new Size(450, 310);
                exlv = Utilities.GridColumnSelectionView(grdList);
                foreach(ListViewItem item in exlv.Items)
                {
                    if (item.SubItems[1].Text == "ModelNo")
                    {
                        exlv.Items.Remove(item);
                    }
                    if (item.SubItems[1].Text == "ModelName")
                    {
                        exlv.Items.Remove(item);
                    }
                }
                exlv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 25), new System.Drawing.Size(450, 250));
                frmPopup.Controls.Add(exlv);

                System.Windows.Forms.Label pnlHeading = new System.Windows.Forms.Label();
                pnlHeading.Size = new Size(300, 20);
                pnlHeading.Location = new System.Drawing.Point(5, 5);
                pnlHeading.Text = "Select Gridview Colums to Export";
                pnlHeading.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                pnlHeading.ForeColor = Color.Black;
                frmPopup.Controls.Add(pnlHeading);

                System.Windows.Forms.Button exlvOK = new System.Windows.Forms.Button();
                exlvOK.Text = "OK";
                exlvOK.BackColor = Color.Tan;
                exlvOK.Location = new System.Drawing.Point(40, 280);
                exlvOK.Click += new System.EventHandler(this.exlvOK_Click1);
                frmPopup.Controls.Add(exlvOK);

                System.Windows.Forms.Button exlvCancel = new System.Windows.Forms.Button();
                exlvCancel.Text = "CANCEL";
                exlvCancel.BackColor = Color.Tan;
                exlvCancel.Location = new System.Drawing.Point(130, 280);
                exlvCancel.Click += new System.EventHandler(this.exlvCancel_Click2);
                frmPopup.Controls.Add(exlvCancel);

                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void exlvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        private void exlvOK_Click1(object sender, EventArgs e)
        {
            try
            {
              
                string heading1 = "Supply Pending ";
                string heading2 = lbldatedisp.Text;
                Utilities.export2Excel(heading1, heading2, "", grdList, exlv);
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export to Excell error");
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pnlList.Visible = true;
            btnExit.Visible = true;
            btnExportToExcel.Visible = true;
        }

        private void ReportPendingSupplyQty_Enter(object sender, EventArgs e)
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

