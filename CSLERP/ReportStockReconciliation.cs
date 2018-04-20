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
    public partial class ReportStockReconciliation : System.Windows.Forms.Form
    {
        Form frmPopup = new Form();
        ListView exlv = new ListView();// list view for choice / selection list
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
        Timer filterTimer = new Timer();
        public ReportStockReconciliation()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void CompanyData_Load(object sender, EventArgs e)
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
            ListFilteredStock();
            
        }
        private void ListFilteredStock()
        {
            
            try
            {
                pnlList.Visible = true;
                grdList.Visible = true;
                grdList.Rows.Clear();
                StockReconcolitationDB sdb = new StockReconcolitationDB();
                List<stockreconcolitation> stockList = StockReconcolitationDB.GetStockReconcolitationList();
                foreach (stockreconcolitation st in stockList)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["StockItemID"].Value = st.StockItemID;
                    grdList.Rows[grdList.RowCount - 1].Cells["StockItemName"].Value = st.StockItemName;
                    grdList.Rows[grdList.RowCount - 1].Cells["Reciept"].Value = st.Reciept;
                    grdList.Rows[grdList.RowCount - 1].Cells["Issue"].Value = st.Issue;
                    grdList.Rows[grdList.RowCount - 1].Cells["Balance"].Value = st.PresentStock;

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
                txtSearch.Text = "";
               
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
                DataGridViewTextBoxColumn dgvtx1 = new DataGridViewTextBoxColumn();
                dgvtx1.Name = "DocumentDate";
                grdDetail.Columns.Add(dgvtx1);
                DataGridViewTextBoxColumn dgvtx2= new DataGridViewTextBoxColumn();
                dgvtx2.Name = "DocumentID";
                dgvtx2.Width = 150;
                grdDetail.Columns.Add(dgvtx2);

                DataGridViewTextBoxColumn dgvtx3 = new DataGridViewTextBoxColumn();
                dgvtx3.Name = "DocumentNo";
                grdDetail.Columns.Add(dgvtx3);

                DataGridViewTextBoxColumn dgvtx4 = new DataGridViewTextBoxColumn();
                dgvtx4.Name = "Receipt";
                grdDetail.Columns.Add(dgvtx4);

                DataGridViewTextBoxColumn dgvtx5 = new DataGridViewTextBoxColumn();
                dgvtx5.Name = "Issue";
                grdDetail.Columns.Add(dgvtx5);

                DataGridViewTextBoxColumn dgvtx6 = new DataGridViewTextBoxColumn();
                dgvtx6.Name = "Balance";
                grdDetail.Columns.Add(dgvtx6);

                StockReconcolitationDB sdb = new StockReconcolitationDB();
                List<stockreconcolitation> stockList = StockReconcolitationDB.GetStockReconcolitationDetailList(stockitemid);
                int i = 0;
                foreach (stockreconcolitation st in stockList)
                {
                    grdDetail.Rows.Add();
                    grdDetail.Rows[i].Cells["DocumentDate"].Value = st.DocumentDate.Date.ToString("dd-MM-yyyy");
                    grdDetail.Rows[i].Cells["DocumentID"].Value = st.DocumentID;
                    grdDetail.Rows[i].Cells["DocumentNo"].Value = st.DocumentNo;
                    grdDetail.Rows[i].Cells["Receipt"].Value = st.Reciept;
                    grdDetail.Rows[i].Cells["Issue"].Value = st.Issue;
                   
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Stock listing");
            }
            return grdDetail;
        }
        private void showStockLedger(string stockitemid, string stockitemname)
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

                frmPopup.Size = new Size(700, 400);


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

                double recptot = 0;
                double issuetot = 0;
                for (int i = 0; i < grdDetail.RowCount; i++)
                {
                    recptot += Convert.ToDouble(grdDetail.Rows[i].Cells["Receipt"].Value);
                    issuetot += Convert.ToDouble(grdDetail.Rows[i].Cells["Issue"].Value);
                    grdDetail.Rows[i].Cells["Balance"].Value = recptot - issuetot;

                }
                grdDetail.Rows.Add();
                grdDetail.Rows[grdDetail.RowCount-1].Cells["DocumentNo"].Value = "Total";
                grdDetail.Rows[grdDetail.RowCount - 1].Cells["Receipt"].Value = recptot;
                grdDetail.Rows[grdDetail.RowCount - 1].Cells["Issue"].Value = issuetot;
                grdDetail.Rows[grdDetail.RowCount - 1].Cells["Balance"].Value = recptot - issuetot;
                
                //ListView 
                // this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked1);
                grdDetail.Bounds = new Rectangle(new Point(20, 60), new Size(650, 300));
                frmPopup.Controls.Add(grdDetail);

                Label lblItemCode = new Label();
                lblItemCode.Width = 150;
                lblItemCode.Text = "Item Code:" + stockitemid;
                lblItemCode.Location= new Point(40, 20);
                frmPopup.Controls.Add(lblItemCode);

                Label lblItemName= new Label();
                lblItemName.Width = 400;
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
                    ////clearUserData();
                    ////closeAllPanels();
                    string itemid = grdList.Rows[e.RowIndex].Cells["StockItemID"].Value.ToString();
                    string itemname = grdList.Rows[e.RowIndex].Cells["StockItemName"].Value.ToString();
                    showStockLedger(itemid, itemname);
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
              
                string heading1 = "Stock report ";
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

        private void ReportStockReconciliation_Enter(object sender, EventArgs e)
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

