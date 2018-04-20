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
    public partial class ReportBankReconciliation : System.Windows.Forms.Form
    {
        Form frmPopup = new Form();
        ListView exlv = new ListView();// list view for choice / selection list
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
        Timer filterTimer = new Timer();
        ListView lv = new ListView();
        DateTime FYStartTIme = new DateTime();
        public ReportBankReconciliation()
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
            btnClose.Visible = false;
            grdVoucherList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightCoral;
            grdVoucherList.EnableHeadersVisualStyles = false;
            //applyPrivilege();
            //ListFilteredStock();

        }
       

        private void initVariables()
        {
            dtRecDate.Format = DateTimePickerFormat.Custom;
            dtRecDate.CustomFormat = "dd-MM-yyyy";
            dtRecDate.Value = UpdateTable.getSQLDateTime();
            pnlList.Visible = true;
            lblDate.Visible = true;
            dtRecDate.Visible = true;
            financialyear fyyr = FinancialYearDB.getFinancialYear().FirstOrDefault(fy => fy.fyID == Main.currentFY);
            if (fyyr != null)
            {
                FYStartTIme = fyyr.startDate;
            }
            //dtpDate.Format = DateTimePickerFormat.Custom;
            //dtpDate.CustomFormat = "dd-MM-yyyy";
            //dtpDate.Enabled = true;
            //cmbFilterStock.SelectedIndex = 1;
            ///setdgvStyle();
        }
        //private void applyPrivilege()
        //{
        //    try
        //    {
        //        if (Main.itemPriv[1])
        //        {
        //            btnExportToExcel.Visible = true;
        //        }
        //        else
        //        {
        //            btnExportToExcel.Visible = false;
        //        }
        //        if (Main.itemPriv[2])
        //        {
        //            btnExportToExcel.Visible = true;
        //        }
        //        else
        //        {
        //            btnExportToExcel.Visible = false;
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

               // btnExportToExcel.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        public void clearUserData()
        {
            try
            {
                txtbankName.Text = "";
                txtBankCode.Text = "";
                dtRecDate.Value = UpdateTable.getSQLDateTime();
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
        //private void setdgvStyle()
        //{

        //    dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        //    dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Highlight;
        //    dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //    dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
        //    dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        //    dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        //    dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

        //    this.grdList.AllowUserToAddRows = false;
        //    this.grdList.AllowUserToDeleteRows = false;
        //    this.grdList.AllowUserToOrderColumns = true;
        //    //dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        //    //this.grdList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        //    this.grdList.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
        //    this.grdList.BorderStyle = System.Windows.Forms.BorderStyle.None;
        //    dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
        //    dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Highlight;
        //    dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //    dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
        //    dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        //    dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        //    dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;

        //}
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
                DataGridViewTextBoxColumn dgvtx2 = new DataGridViewTextBoxColumn();
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
                grdDetail.Rows[grdDetail.RowCount - 1].Cells["DocumentNo"].Value = "Total";
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
                lblItemCode.Location = new Point(40, 20);
                frmPopup.Controls.Add(lblItemCode);

                Label lblItemName = new Label();
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
                if (columnName.Equals("Details"))
                {
                    int rowIndex = e.RowIndex;
                    if(rowIndex == 1)
                    {

                        List<ReceiptVoucherHeader> recptList = ReceiptVoucherDB.getAllNonDepositedReceiptsForReportBR(FYStartTIme, dtRecDate.Value, txtBankCode.Text);
                        grdVoucherList.Rows.Clear();
                        grdVoucherList.Visible = false;
                        btnClose.Visible = false;
                        foreach (ReceiptVoucherHeader rvh in recptList)
                        {
                            if (rvh.VoucherAmount != 0) //If Debit amount is not equal to zero
                            {
                                grdVoucherList.Rows.Add();
                                grdVoucherList.Rows[grdVoucherList.Rows.Count - 1].Cells["DocumentID"].Value = rvh.DocumentID;
                                grdVoucherList.Rows[grdVoucherList.Rows.Count - 1].Cells["VoucherNo"].Value = rvh.VoucherNo;
                                grdVoucherList.Rows[grdVoucherList.Rows.Count - 1].Cells["VoucherDate"].Value = rvh.VoucherDate;
                                grdVoucherList.Rows[grdVoucherList.Rows.Count - 1].Cells["PartyName"].Value = rvh.SLName;
                                grdVoucherList.Rows[grdVoucherList.Rows.Count - 1].Cells["DebitAmount"].Value = rvh.VoucherAmount; // For Debit AMount INR
                                grdVoucherList.Rows[grdVoucherList.Rows.Count - 1].Cells["CreditAmount"].Value = rvh.VoucherAmountINR; // For Credit AMount INR
                            }
                        }
                        if(grdVoucherList.Rows.Count == 0)
                        {
                            MessageBox.Show("Voucher Not found");
                        }
                        else
                        {
                            grdVoucherList.Visible = true;
                            btnClose.Visible = true;
                        }
                       
                    }
                    else if(rowIndex == 2)
                    {
                        List<paymentvoucher> PayList = PaymentVoucherDB.getAllNonDepositedPaymentsForReportBR(FYStartTIme, dtRecDate.Value, txtBankCode.Text);
                        grdVoucherList.Rows.Clear();
                        grdVoucherList.Visible = false;
                        btnClose.Visible = false;
                        foreach (paymentvoucher pvh in PayList)
                        {
                            if (pvh.VoucherAmountINR != 0) //If Credit amount is not equal to zero
                            {
                                grdVoucherList.Rows.Add();
                                grdVoucherList.Rows[grdVoucherList.Rows.Count - 1].Cells["DocumentID"].Value = pvh.DocumentID;
                                grdVoucherList.Rows[grdVoucherList.Rows.Count - 1].Cells["VoucherNo"].Value = pvh.VoucherNo;
                                grdVoucherList.Rows[grdVoucherList.Rows.Count - 1].Cells["VoucherDate"].Value = pvh.VoucherDate;
                                grdVoucherList.Rows[grdVoucherList.Rows.Count - 1].Cells["PartyName"].Value = pvh.SLName;
                                grdVoucherList.Rows[grdVoucherList.Rows.Count - 1].Cells["DebitAmount"].Value = pvh.VoucherAmount; // For Debit AMount INR
                                grdVoucherList.Rows[grdVoucherList.Rows.Count - 1].Cells["CreditAmount"].Value = pvh.VoucherAmountINR; // For Credit AMount INR
                            }
                        }
                        if (grdVoucherList.Rows.Count == 0)
                        {
                            MessageBox.Show("Voucher Not found");
                        }
                        else
                        {
                            grdVoucherList.Visible = true;
                            btnClose.Visible = true;
                        }
                    }
                }
                else
                {
                    return;
                }

            }
            catch (Exception ex)
            {

            }
        }
      
        //private void btnExportToExcel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        frmPopup = new Form();
        //        frmPopup.StartPosition = FormStartPosition.CenterScreen;
        //        frmPopup.BackColor = Color.CadetBlue;

        //        frmPopup.MaximizeBox = false;
        //        frmPopup.MinimizeBox = false;
        //        frmPopup.ControlBox = false;
        //        frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

        //        frmPopup.Size = new Size(450, 310);
        //        exlv = Utilities.GridColumnSelectionView(grdList);
        //        foreach (ListViewItem item in exlv.Items)
        //        {
        //            if (item.SubItems[1].Text == "ModelNo")
        //            {
        //                exlv.Items.Remove(item);
        //            }
        //            if (item.SubItems[1].Text == "ModelName")
        //            {
        //                exlv.Items.Remove(item);
        //            }
        //        }
        //        exlv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 25), new System.Drawing.Size(450, 250));
        //        frmPopup.Controls.Add(exlv);

        //        System.Windows.Forms.Label pnlHeading = new System.Windows.Forms.Label();
        //        pnlHeading.Size = new Size(300, 20);
        //        pnlHeading.Location = new System.Drawing.Point(5, 5);
        //        pnlHeading.Text = "Select Gridview Colums to Export";
        //        pnlHeading.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
        //        pnlHeading.ForeColor = Color.Black;
        //        frmPopup.Controls.Add(pnlHeading);

        //        System.Windows.Forms.Button exlvOK = new System.Windows.Forms.Button();
        //        exlvOK.Text = "OK";
        //        exlvOK.BackColor = Color.Tan;
        //        exlvOK.Location = new System.Drawing.Point(40, 280);
        //        exlvOK.Click += new System.EventHandler(this.exlvOK_Click1);
        //        frmPopup.Controls.Add(exlvOK);

        //        System.Windows.Forms.Button exlvCancel = new System.Windows.Forms.Button();
        //        exlvCancel.Text = "CANCEL";
        //        exlvCancel.BackColor = Color.Tan;
        //        exlvCancel.Location = new System.Drawing.Point(130, 280);
        //        exlvCancel.Click += new System.EventHandler(this.exlvCancel_Click2);
        //        frmPopup.Controls.Add(exlvCancel);

        //        frmPopup.ShowDialog();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //private void exlvCancel_Click2(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        frmPopup.Close();
        //        frmPopup.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private void exlvOK_Click1(object sender, EventArgs e)
        //{
        //    try
        //    {

        //        string heading1 = "Stock report ";
        //        string heading2 = lbldatedisp.Text;
        //        Utilities.export2Excel(heading1, heading2, "", grdList, exlv);
        //        frmPopup.Close();
        //        frmPopup.Dispose();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Export to Excell error");
        //    }
        //}
        private void btnSelbank_Click(object sender, EventArgs e)
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(450, 300);
            lv = BREntryDB.getBankAccountCodeListView();
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(450, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Click3);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click3);
            frmPopup.Controls.Add(lvCancel);
            frmPopup.ShowDialog();
        }
        private void lvOK_Click3(object sender, EventArgs e)
        {
            try
            {
                if (lv.CheckedIndices.Count == 0)
                {
                    MessageBox.Show("Select one Bank.");
                    return;
                }
                if (lv.CheckedIndices.Count > 1)
                {
                    MessageBox.Show("Not allowed to select more than one Bank.");
                    return;
                }
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        txtBankCode.Text = itemRow.SubItems[1].Text;
                        txtbankName.Text = itemRow.SubItems[2].Text;
                        itemRow.Checked = false;
                        grdVoucherList.Visible = false;
                        grdVoucherList.Rows.Clear();
                        grdList.Visible = false;
                        grdList.Rows.Clear();
                        btnClose.Visible = false;
                        frmPopup.Close();
                        frmPopup.Dispose();
                        
                       
                        break;
                    } 
                }
            }
            catch (Exception)
            {
            }
        }
        private void lvCancel_Click3(object sender, EventArgs e)
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

        private void grdVoucherList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string docid = grdVoucherList.Rows[e.RowIndex].Cells["DocumentID"].Value.ToString();
                int vocuherNo = Convert.ToInt32(grdVoucherList.Rows[e.RowIndex].Cells["VoucherNo"].Value);
                DateTime voucherDate = Convert.ToDateTime(grdVoucherList.Rows[e.RowIndex].Cells["VoucherDate"].Value);
                if (docid.Length == 0 || docid == "OB")
                {
                    return;
                }
                FileManager.ShowVoucherDetails showVoucher = new FileManager.ShowVoucherDetails(docid, vocuherNo, voucherDate);
                showVoucher.ShowDialog();
                this.RemoveOwnedForm(showVoucher);
            }
            catch (Exception ex)
            {
            }
        }

        private void dtRecDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                grdVoucherList.Visible = false;
                grdVoucherList.Rows.Clear();
                grdList.Visible = false;
                grdList.Rows.Clear();
                btnClose.Visible = false;
            }
            catch(Exception ex)
            {

            }
        }
        private void AddGrdListRows()
        {

            try
            {
                //pnlList.Visible = true;
                //grdList.Visible = true;
                grdList.Rows.Clear();
                string[] headings = { "Book balance", "Deposite not cleared", "Payments not cleared", "Bank balance" };
                for (int i = 0; i < 4; i++)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["Item"].Value = headings[i];
                    grdList.Rows[grdList.RowCount - 1].Cells["Debit"].Value = Convert.ToDecimal(0);
                    grdList.Rows[grdList.RowCount - 1].Cells["Credit"].Value = Convert.ToDecimal(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in adding rows to grid");
            }
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                AddGrdListRows();
                DateTime dt = UpdateTable.getSQLDateTime();
                if (txtBankCode.Text.Trim().Length == 0 || dtRecDate.Value > dt || dtRecDate.Value < FYStartTIme )
                {
                    MessageBox.Show("Fill Bank Ac code or Date Properly");
                    return;
                }
                decimal bookbal = LedgerDB.getTotalBookBalanceOfBankAc(FYStartTIme, dtRecDate.Value, txtBankCode.Text);
                decimal TotDr = 0;
                decimal TotCr = 0;

                if (bookbal < 0)
                {
                    grdList.Rows[0].Cells["Credit"].Value = bookbal * (-1);
                    TotCr = bookbal * (-1);
                }
                else
                {
                    grdList.Rows[0].Cells["Debit"].Value = bookbal;
                    TotDr = bookbal;
                }

                grdList.Rows[1].Cells["Debit"].Value = ReceiptVoucherDB.getTotalNotClearedDepositeOfBankAcForBRReport(FYStartTIme, dtRecDate.Value, txtBankCode.Text);
                TotDr = TotDr + Convert.ToDecimal(grdList.Rows[1].Cells["Debit"].Value);

                grdList.Rows[2].Cells["Credit"].Value = PaymentVoucherDB.getTotalNotClearedPaymentOfBankAcForBRReport(FYStartTIme, dtRecDate.Value, txtBankCode.Text);
                TotCr = TotCr + Convert.ToDecimal(grdList.Rows[2].Cells["Credit"].Value);

                //string str = grdList.Rows[3].Cells["Credit"].Value.ToString();
                decimal tot = TotDr - TotCr;
                if (tot < 0)
                    grdList.Rows[3].Cells["Credit"].Value = tot;
                else
                    grdList.Rows[3].Cells["Debit"].Value = tot;

                grdList.Visible = true;
                grdVoucherList.Rows.Clear();
                grdVoucherList.Visible = false;
                btnClose.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            grdVoucherList.Visible = false;
            grdVoucherList.Rows.Clear();
            btnClose.Visible = false;
        }

        private void ReportBankReconciliation_Enter(object sender, EventArgs e)
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

