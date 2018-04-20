using System;
using System.Collections.Generic;
using System.Collections;
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
using System.Collections.ObjectModel;

namespace CSLERP
{
    public partial class ReceiptsAndPayments : System.Windows.Forms.Form
    {
        string docID = "";
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        //stockgroup prevsg = new stockgroup();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        ListView lvCopy = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        TextBox txtSearch = new TextBox();
        string selFYear = "";
        int rowIndex;
        Color c;
        public ReceiptsAndPayments()
        {
            try
            {
                InitializeComponent();

               
            }
            catch (Exception)
            {

            }
        }
        private void ReceiptsAndPayments_Load(object sender, EventArgs e)
        {
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;

            String a = this.Size.ToString();
        }
     
        private void initVariables()
        {  
            docID = Main.currentDocument;
            pnlUI.Controls.Add(pnlList);
            dtFromDate.Format = DateTimePickerFormat.Custom;
            dtFromDate.CustomFormat = "dd-MM-yyyy";
            dtToDate.Format = DateTimePickerFormat.Custom;
            dtToDate.CustomFormat = "dd-MM-yyyy";
            dtFromDate.Value = DateTime.Now.AddMonths(-1);
            dtToDate.Value = DateTime.Now;
            enableBottomButtons();
            btnDocWiseExportToExcel.Visible = false;
            btnLevel2ExportToexcel.Visible = false;
            pnlShowTotal.Visible = false;
            FinancialYearDB.fillFYIDCombo(cmbFYID);
            cmbFYID.SelectedIndex = cmbFYID.FindString(Main.currentFY);
            grdDocumentWiseList.Visible = false;
            grdRadioSelectWiseList.Visible = false;
            btnClose1StGrid.Visible = false;
            btnClose2ndGrid.Visible = false;
            grdDocumentWiseList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grdRadioSelectWiseList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            //---------
            string[] str = cmbFYID.SelectedItem.ToString().Split(':');
            string s = str[0];
            string ss = str[1];
            string sss = str[2];
            DateTime FYstartDate = Convert.ToDateTime(ss.Trim());
            DateTime FYEndDate = Convert.ToDateTime(sss.Trim());


            if (dtFromDate.Value.Date < FYstartDate.Date)
            {
                dtFromDate.Value = FYstartDate.Date;
            }

        }
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
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    //clearData();
        //    closeAllPanels();
        //    pnlList.Visible = true;
        //    listStockGroup(lvl);
        //    //pnlBottomActions.Visible = true;
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

        public void clearData()
        {
            try
            {
                txtTotalPayment.Text = "";
                txtTotalReceipt.Text = "";
                grdRadioSelectWiseList.Rows.Clear();
                grdDocumentWiseList.Rows.Clear();
                rdbAccountWise.Checked = false;
                rdbDateWise.Checked = false;
                rdbPartyWise.Checked = false;
                removeControlsFromlvPanel();
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
        private void enableBottomButtons()
        {
            //btnNew.Visible = false;
            btnExit.Visible = true;
            pnlBottomButtons.Visible = true;
        }
 
        private void removeControlsFromlvPanel()
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
        private List<ledger> getLedgerDetails()
        {
            ledger ldg = new ledger();
            List<ledger> VDetails = new List<ledger>();
            try
            {
                for (int i = 0; i < grdRadioSelectWiseList.Rows.Count - 2; i++)
                {
                    ldg = new ledger();
                    ldg.DocumentID = grdRadioSelectWiseList.Rows[i].Cells["SelectedType"].Value.ToString();  // For Storing Selected Type of radio button
                    ldg.DebitAmnt = Convert.ToDecimal(grdRadioSelectWiseList.Rows[i].Cells["Payment"].Value.ToString());
                    ldg.CreditAmnt = Convert.ToDecimal(grdRadioSelectWiseList.Rows[i].Cells["Receipt"].Value);
                    VDetails.Add(ldg);
                }
            }
            catch (Exception EX)
            {

            }
            return VDetails;
        }
        private void btnLevel2ExportToexcel_Click(object sender, EventArgs e)
        {
            List<ledger> LedgerList = getLedgerDetails();
            removeControlsFromlvPanel();
            pnlList.Controls.Remove(pnllv);
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;

            pnllv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(100, 100), new Size(600, 300));
            lv = Utilities.GridColumnSelectionView(grdRadioSelectWiseList);

            lv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(50, 50), new Size(500, 200));
            pnllv.Controls.Add(lv);

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
        private void exlvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                pnllv.Visible = false;
                string heading3 = "From Date" + Main.delimiter1 +
                     dtFromDate.Value + Main.delimiter2 + "To Date" + Main.delimiter1 + dtToDate.Value;
                string heading1 = "Receipt And Payment";
                string heading2 = "";
                Utilities.export2Excel(heading1, heading2, heading3, grdRadioSelectWiseList, lv);
            }
            catch (Exception ex)
            { 
            }
        }
        private void exlvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                removeControlsFromlvPanel();
                pnllv.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }
        private List<ledger> getDocumetnWiseLedgerDetails()
        {
            ledger ldg = new ledger();
            List<ledger> VDetails = new List<ledger>();
            try
            {
                for (int i = 0; i < grdDocumentWiseList.Rows.Count - 2; i++)
                {
                    ldg = new ledger();
                    ldg.VoucherNo = Convert.ToInt32(grdDocumentWiseList.Rows[i].Cells["VoucherNo"].Value.ToString());
                    ldg.VoucherDate = Convert.ToDateTime(grdDocumentWiseList.Rows[i].Cells["VoucherDate"].Value.ToString());
                    ldg.Narration = grdDocumentWiseList.Rows[i].Cells["VoucherType"].Value.ToString();  // For Storing Voucher Type
                    ldg.DebitAmnt = Convert.ToDecimal(grdDocumentWiseList.Rows[i].Cells["gPayment"].Value.ToString());
                    ldg.CreditAmnt = Convert.ToDecimal(grdDocumentWiseList.Rows[i].Cells["gReceipt"].Value);
                    VDetails.Add(ldg);
                }
            }
            catch (Exception EX)
            {

            }
            return VDetails;
        }
        private void btnDocWiseExportToExcel_Click(object sender, EventArgs e)
        {
            List<ledger> LedgerList = getDocumetnWiseLedgerDetails();
            removeControlsFromlvPanel();
            pnlList.Controls.Remove(pnllv);
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;

            pnllv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(100, 100), new Size(600, 300));
            lv = Utilities.GridColumnSelectionView(grdDocumentWiseList);

            lv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(50, 50), new Size(500, 200));
            pnllv.Controls.Add(lv);

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
            exlvOK.Click += new System.EventHandler(this.exlvOK_Click2);
            pnllv.Controls.Add(exlvOK);

            System.Windows.Forms.Button exlvCancel = new System.Windows.Forms.Button();
            exlvCancel.Text = "Cancel";
            exlvCancel.Location = new System.Drawing.Point(150, 270);
            exlvCancel.Click += new System.EventHandler(this.exlvCancel_Click3);
            pnllv.Controls.Add(exlvCancel);
            pnlList.Controls.Add(pnllv);
            pnllv.BringToFront();
            pnllv.Visible = true;
        }
        private void exlvOK_Click2(object sender, EventArgs e)
        {
            try
            {
                pnllv.Visible = false;
                string heading3 = "From Date" + Main.delimiter1 +
                     dtFromDate.Value + Main.delimiter2 + "To Date" + Main.delimiter1 + dtToDate.Value;
                string heading1 = "Receipt And Payment";
                string heading2 = "";
                Utilities.export2Excel(heading1, heading2, heading3, grdDocumentWiseList, lv);
            }
            catch (Exception ex)
            {
            }
        }
        private void exlvCancel_Click3(object sender, EventArgs e)
        {
            try
            {
                removeControlsFromlvPanel();
                pnllv.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            clearData();
            if(dtFromDate.Value > dtToDate.Value)
            {
                MessageBox.Show("From Date is Greater than to Date. Check First.");
                return;
            }
            string totalString = "";
            DateTime ToDate = dtToDate.Value;
            DateTime FromDate = dtFromDate.Value;
            if (Utilities.verifyFinancialYear(cmbFYID.SelectedItem.ToString(), FromDate)
                        && Utilities.verifyFinancialYear(cmbFYID.SelectedItem.ToString(), ToDate))
            {
                totalString = ReceiptAndPaymentDB.getTotalreceiptAndTotalValue(FromDate, ToDate);
            }
            else
            {
                MessageBox.Show("Error:\n From Date or To Date is wrong.");
                return;
            }
            txtTotalPayment.Text = totalString.Substring(0, totalString.IndexOf(':'));
            txtTotalReceipt.Text = totalString.Substring(totalString.IndexOf(':') + 1);
            txtDifference.Text = (Convert.ToDecimal(txtTotalReceipt.Text) - Convert.ToDecimal(txtTotalPayment.Text)).ToString();
            pnlShowTotal.Visible = true;
            grdDocumentWiseList.Visible = false;
            grdRadioSelectWiseList.Visible = false;
            btnDocWiseExportToExcel.Visible = false;
            btnLevel2ExportToexcel.Visible = false;
            btnClose1StGrid.Visible = false;
            btnClose2ndGrid.Visible = false;
        }
      
        private void grdRadioSelectWiseList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            string columnName = grdRadioSelectWiseList.Columns[e.ColumnIndex].Name;
            try
            {
                if (columnName.Equals("Details") && e.RowIndex != grdRadioSelectWiseList.Rows.Count-1)
                {
                    grdRadioSelectWiseList.Rows[rowIndex].DefaultCellStyle.BackColor = c;
                    c = grdRadioSelectWiseList.Rows[e.RowIndex].DefaultCellStyle.BackColor;
                    grdRadioSelectWiseList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.CornflowerBlue;
                    rowIndex = e.RowIndex;
                    btnClose2ndGrid.Visible = true;
                    grdDocumentWiseList.Visible = true;
                    int opt;
                    ledger ldg = new ledger();
                    string HText = grdRadioSelectWiseList.Columns["SelectedType"].HeaderText;
                    if (HText.Equals("Account Code"))
                    {
                        opt = 1;
                        //string str = grdRadioSelectWiseList.Rows[e.RowIndex].Cells["SelectedType"].Value.ToString();
                        ldg.AccountCode = grdRadioSelectWiseList.Rows[e.RowIndex].Cells["SelectedType"].Value.ToString();
                        ldg.AccountName = grdRadioSelectWiseList.Rows[e.RowIndex].Cells["SelectedName"].Value.ToString();

                    }
                    else if (HText.Equals("Party Code"))
                    {
                        opt = 2;
                        //string str = grdRadioSelectWiseList.Rows[e.RowIndex].Cells["SelectedType"].Value.ToString();
                        ldg.SLCode = grdRadioSelectWiseList.Rows[e.RowIndex].Cells["SelectedType"].Value.ToString();
                        ldg.SLName = grdRadioSelectWiseList.Rows[e.RowIndex].Cells["SelectedName"].Value.ToString();
                    }
                    else
                    {
                        opt = 3;
                        string str = grdRadioSelectWiseList.Rows[e.RowIndex].Cells["SelectedType"].Value.ToString();
                        str = Utilities.convertDateStringToAnsi(str);
                        ldg.VoucherDate = Convert.ToDateTime(str);
                        ////DateTime tDate = Convert.ToDateTime("30-01-2018",);
                        ////DateTime tDate = DateTime.ParseExact("2018-01-30 00:00:00", "yyyy-MM-dd HH:mm", null);
                        //ldg.VoucherDate = DateTime.Parse(str);
                    }
                    List<ledger> ledgList = ReceiptAndPaymentDB.getDocumentWiseDetail(ldg, opt, dtFromDate.Value, dtToDate.Value);
                    btnDocWiseExportToExcel.Visible = true;
                    showDocumentWiseDetailGrid(ledgList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("grdRadioSelectWiseList_CellContentClick() : Error - "+ex.ToString());
            }
        }
        private void showDocumentWiseDetailGrid(List<ledger> LedgerList)
        {
            try
            {
                decimal receiptTOtal = 0;
                decimal paymentTotal = 0;
                grdDocumentWiseList.Rows.Clear();
                foreach (ledger led in LedgerList)
                {
                    grdDocumentWiseList.Rows.Add();
                    grdDocumentWiseList.Rows[grdDocumentWiseList.RowCount - 1].Cells["VoucherNo"].Value = led.VoucherNo;
                    grdDocumentWiseList.Rows[grdDocumentWiseList.RowCount - 1].Cells["VoucherType"].Value = led.DocumentID; 
                    grdDocumentWiseList.Rows[grdDocumentWiseList.RowCount - 1].Cells["VoucherDate"].Value = led.VoucherDate;
                    grdDocumentWiseList.Rows[grdDocumentWiseList.RowCount - 1].Cells["gReceipt"].Value = led.CreditAmnt;
                    grdDocumentWiseList.Rows[grdDocumentWiseList.RowCount - 1].Cells["gPayment"].Value = led.DebitAmnt;
                    grdDocumentWiseList.Rows[grdDocumentWiseList.RowCount - 1].Cells["gDocumentID"].Value = led.DocumentID;
                    receiptTOtal = receiptTOtal + led.CreditAmnt;
                    paymentTotal = paymentTotal + led.DebitAmnt;
                }
                grdDocumentWiseList.Rows.Add();
                grdDocumentWiseList.Rows[grdDocumentWiseList.RowCount - 1].Cells["VoucherDate"].Value = "Total";
                grdDocumentWiseList.Rows[grdDocumentWiseList.RowCount - 1].Cells["gReceipt"].Value = receiptTOtal;
                grdDocumentWiseList.Rows[grdDocumentWiseList.RowCount - 1].Cells["gPayment"].Value = paymentTotal;
            }
            catch (Exception ex)
            {

            }
        }
        private Boolean checkTotal()
        {
            Boolean stat = true;
            try
            {
                if (Convert.ToDecimal(txtTotalPayment.Text) == 0 && Convert.ToDecimal(txtTotalReceipt.Text) == 0)
                {
                    MessageBox.Show("Total Amount is ZERO");
;                    stat = false;
                }
            }
            catch (Exception ex)
            {
                stat = false;
            }
            return stat;
        }
        private void setRadioClickVisiblity()
        {
            btnClose1StGrid.Visible = true;
            btnClose2ndGrid.Visible = false;
            grdRadioSelectWiseList.Visible = true;
            btnLevel2ExportToexcel.Visible = true;
            grdDocumentWiseList.Visible = false;
            grdDocumentWiseList.Rows.Clear();
            btnDocWiseExportToExcel.Visible = false;
        }
        private void rdbAccountWise_Click(object sender, EventArgs e)
        {
            if (checkTotal())
            {
                setRadioClickVisiblity();
                ShowRadioSelectWiseGridView(1);
            }
        }

        private void rdbPartyWise_Click(object sender, EventArgs e)
        {
            if (checkTotal())
            {
                setRadioClickVisiblity();
                ShowRadioSelectWiseGridView(2);
            }

        }

        private void rdbDateWise_Click_1(object sender, EventArgs e)
        {
            if (checkTotal())
            {
                setRadioClickVisiblity();
                ShowRadioSelectWiseGridView(3);
            }
        }

        private void ShowRadioSelectWiseGridView(int opt)
        {
            try
            {
                if (opt==3)
                {
                    grdRadioSelectWiseList.Columns["SelectedType"].Width = 120;
                    grdRadioSelectWiseList.Columns["Receipt"].Width = 150;
                    grdRadioSelectWiseList.Columns["Payment"].Width = 150;
                    grdRadioSelectWiseList.Columns["Details"].Width = 90;
                }
                else
                {
                    grdRadioSelectWiseList.Columns["SelectedType"].Width = 70;
                    grdRadioSelectWiseList.Columns["Receipt"].Width = 100;
                    grdRadioSelectWiseList.Columns["Payment"].Width = 100;
                    grdRadioSelectWiseList.Columns["Details"].Width = 70;
                }
                if (opt == 1)
                {
                    grdRadioSelectWiseList.Columns["SelectedType"].HeaderText = "Account Code";
                    grdRadioSelectWiseList.Columns["SelectedName"].HeaderText = "Account Name";
                    grdRadioSelectWiseList.Columns["SelectedName"].Visible = true;
                    List<ledger> LedgerList = ReceiptAndPaymentDB.getFilteredPaymentAndReceiptDetails(opt, dtFromDate.Value, dtToDate.Value);
                    addDetailsToGrid(LedgerList, opt);
                }
                else if (opt == 2)
                {
                    grdRadioSelectWiseList.Columns["SelectedType"].HeaderText = "Party Code";
                    grdRadioSelectWiseList.Columns["SelectedName"].HeaderText = "Party Name";
                    grdRadioSelectWiseList.Columns["SelectedName"].Visible = true;
                    List<ledger> LedgerList = ReceiptAndPaymentDB.getFilteredPaymentAndReceiptDetails(opt, dtFromDate.Value, dtToDate.Value);
                    addDetailsToGrid(LedgerList, opt);
                }
                else if (opt == 3)
                {
                    grdRadioSelectWiseList.Columns["SelectedType"].HeaderText = "Date";
                    grdRadioSelectWiseList.Columns["SelectedName"].Visible = false;
                    List <ledger> LedgerList = ReceiptAndPaymentDB.getFilteredPaymentAndReceiptDetails(opt, dtFromDate.Value, dtToDate.Value);
                    addDetailsToGrid(LedgerList, opt);
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void addDetailsToGrid(List<ledger> LedgerList, int opt)
        {
            try
            {
                grdRadioSelectWiseList.Rows.Clear();
                decimal receiptTOtal = 0;
                decimal paymentTotal = 0;
                foreach (ledger led in LedgerList)
                {
                    grdRadioSelectWiseList.Rows.Add();
                    if (opt != 3)
                    {
                        grdRadioSelectWiseList.Rows[grdRadioSelectWiseList.RowCount - 1].Cells["SelectedType"].Value = led.AccountCode;
                        grdRadioSelectWiseList.Rows[grdRadioSelectWiseList.RowCount - 1].Cells["SelectedName"].Value = led.AccountName;
                    }
                    else
                    {
                        grdRadioSelectWiseList.Rows[grdRadioSelectWiseList.RowCount - 1].Cells["SelectedType"].Value = led.VoucherDate.ToString("dd-MM-yyyy");
                    }
                    grdRadioSelectWiseList.Rows[grdRadioSelectWiseList.RowCount - 1].Cells["Receipt"].Value = led.CreditAmnt;
                    grdRadioSelectWiseList.Rows[grdRadioSelectWiseList.RowCount - 1].Cells["Payment"].Value = led.DebitAmnt;
                    receiptTOtal = receiptTOtal + led.CreditAmnt;
                    paymentTotal = paymentTotal + led.DebitAmnt;
                }
                grdRadioSelectWiseList.Rows.Add();
                grdRadioSelectWiseList.Rows[grdRadioSelectWiseList.RowCount - 1].Cells["SelectedType"].Value = "Total";
                grdRadioSelectWiseList.Rows[grdRadioSelectWiseList.RowCount - 1].Cells["Receipt"].Value = receiptTOtal;
                grdRadioSelectWiseList.Rows[grdRadioSelectWiseList.RowCount - 1].Cells["Payment"].Value = paymentTotal;
            }
            catch (Exception ex)
            {

            }
        }

        private void grdRadioSelectWiseList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }
        DataGridViewRow dgRowTotalCount;
        private void grdRadioSelectWiseList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                dgRowTotalCount = (DataGridViewRow)grdRadioSelectWiseList.Rows[((DataGridView)sender).Rows.Count - 1].Clone();
                for (Int32 index = 0; index < ((DataGridView)sender).Rows[((DataGridView)sender).Rows.Count - 1].Cells.Count; index++)
                {
                    dgRowTotalCount.Cells[index].Value = ((DataGridView)sender).Rows[((DataGridView)sender).Rows.Count - 1].Cells[index].Value;
                }
                ((DataGridView)sender).Rows.RemoveAt(((DataGridView)sender).Rows.Count - 1);
            }
        }

        private void grdRadioSelectWiseList_Sorted(object sender, EventArgs e)
        {
            grdRadioSelectWiseList.Rows.Insert(((DataGridView)sender).Rows.Count, dgRowTotalCount);
        }
        DataGridViewRow dgRowTotalCount2;
        private void grdDocumentWiseList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                dgRowTotalCount2 = (DataGridViewRow)grdDocumentWiseList.Rows[((DataGridView)sender).Rows.Count - 1].Clone();
                for (Int32 index = 0; index < ((DataGridView)sender).Rows[((DataGridView)sender).Rows.Count - 1].Cells.Count; index++)
                {
                    dgRowTotalCount2.Cells[index].Value = ((DataGridView)sender).Rows[((DataGridView)sender).Rows.Count - 1].Cells[index].Value;
                }
                ((DataGridView)sender).Rows.RemoveAt(((DataGridView)sender).Rows.Count - 1);
            }
        }

        private void grdDocumentWiseList_Sorted(object sender, EventArgs e)
        {
            grdDocumentWiseList.Rows.Insert(((DataGridView)sender).Rows.Count, dgRowTotalCount2);
        }
        private void disableControls()
        {
            pnlShowTotal.Visible = false;
            grdDocumentWiseList.Visible = false;
            grdRadioSelectWiseList.Visible = false;
            btnDocWiseExportToExcel.Visible = false;
            btnLevel2ExportToexcel.Visible = false;
            btnClose1StGrid.Visible = false;
            btnClose2ndGrid.Visible = false;
        }
        private void dtToDate_ValueChanged(object sender, EventArgs e)
        {
            disableControls();
        }

        private void dtFromDate_ValueChanged(object sender, EventArgs e)
        {
            disableControls();
        }

        private void cmbFYID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pnlShowTotal.Visible == true)
            {
                string str = cmbFYID.SelectedItem.ToString();
                if (selFYear != null && selFYear != str)
                {
                        dtFromDate.Value = DateTime.Now.AddMonths(-1);
                        dtToDate.Value = DateTime.Now;
                        disableControls();
                }
            }
            selFYear = cmbFYID.SelectedItem.ToString();
        }

        private void btnClose1StGrid_Click(object sender, EventArgs e)
        {
            rdbAccountWise.Checked = false;
            rdbDateWise.Checked = false;
            rdbPartyWise.Checked = false;
            btnClose1StGrid.Visible = false;
            btnClose2ndGrid.Visible = false;
            btnDocWiseExportToExcel.Visible = false;
            grdDocumentWiseList.Visible = false;
            grdDocumentWiseList.Rows.Clear();
            btnLevel2ExportToexcel.Visible = false;
            grdRadioSelectWiseList.Visible = false;
            grdRadioSelectWiseList.Rows.Clear();
        }

        private void btnClose2ndGrid_Click(object sender, EventArgs e)
        {
            grdRadioSelectWiseList.Rows[rowIndex].DefaultCellStyle.BackColor = c;
            //changeColor(grdRadioSelectWiseList);
            btnClose2ndGrid.Visible = false;
            btnDocWiseExportToExcel.Visible = false;
            grdDocumentWiseList.Visible = false;
            grdDocumentWiseList.Rows.Clear();
        }

        private void grdDocumentWiseList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (grdDocumentWiseList.Rows[e.RowIndex].Cells["gDocumentID"].Value == null)
                {
                    return;
                }
                string docid = grdDocumentWiseList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                int vocuherNo = Convert.ToInt32(grdDocumentWiseList.Rows[e.RowIndex].Cells["VoucherNo"].Value);
                DateTime voucherDate = Convert.ToDateTime(grdDocumentWiseList.Rows[e.RowIndex].Cells["VoucherDate"].Value);
               
                FileManager.ShowVoucherDetails showVoucher = new FileManager.ShowVoucherDetails(docid, vocuherNo, voucherDate);
                showVoucher.ShowDialog();
                this.RemoveOwnedForm(showVoucher);
            }
            catch (Exception ex)
            {
            }
        }

        private void ReceiptsAndPayments_Enter(object sender, EventArgs e)
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


