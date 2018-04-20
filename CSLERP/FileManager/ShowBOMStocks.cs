using CSLERP.DBData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLERP.FileManager
{
    public partial class ShowBOMStocks : Form
    {
        private List<bomdetail> bomList;
        string prod;
        string ModelNo;
        double prodQuant;
        Form frmPopup = new Form();
        DataGridView grdBomDetail = new DataGridView();
        double prevPrepQuant = 0;
        public List<stock> StockDetail;
        DataGridView grdStock = new DataGridView();
        int st = 0;
        public ShowBOMStocks(string product,double quant,string modNo)
        {
            InitializeComponent();
            prod = product;
            prodQuant = quant;
            ModelNo = modNo;
            addGridListRows();
        }
        private void btnShowBOM_Click(object sender, EventArgs e)
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

                frmPopup.Size = new Size(500, 340);

                BOMDB bomdb = new BOMDB();
                string prodID = lblProductCode.Text.Trim();
                grdBomDetail = bomdb.getGridViewForBOMDetail(prodID,ModelNo);
                grdBomDetail.Bounds = new Rectangle(new Point(0, 0), new Size(500, 300));
                grdBomDetail.Columns["PurchasePrice"].Visible = false;
                grdBomDetail.Columns["CustomPrice"].Visible = false;
                frmPopup.Controls.Add(grdBomDetail);

                Button lvCancel = new Button();
                lvCancel.Text = "Close";
                lvCancel.BackColor = Color.Tan;
                lvCancel.Location = new System.Drawing.Point(20, 310);
                lvCancel.Click += new System.EventHandler(this.grdCancel_Click1);
                frmPopup.Controls.Add(lvCancel);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
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
        private void addGridListRows()
        {
            try
            {
                BOMDB bomdb = new BOMDB();
                string[] ProductStrArr = prod.Split('-');
                string ProdID = ProductStrArr[0];
                string ProdName = ProductStrArr[1];
                lblProductCode.Text = ProdID;
                lblProductname.Text = ProdName;
                bomList = bomdb.getBOMDetail(ProdID);
                txtProdQuantity.Text = prodQuant.ToString();
                int QuantTOPrepare = Convert.ToInt32(txtProdQuantity.Text);
                lblModelNo.Text = ModelNo;
                foreach (bomdetail bom in bomList)
                {
                    string stockID = bom.StockItemID;
                    double totalStock = StockDB.getTotalItemWiseStock(stockID, lblModelNo.Text);
                    
                    grdList.Rows.Add();
                    grdList.Rows[grdList.Rows.Count - 1].Cells["SINO"].Value = grdList.Rows.Count;
                    grdList.Rows[grdList.Rows.Count - 1].Cells["StockItemID"].Value = bom.StockItemID;
                    grdList.Rows[grdList.Rows.Count - 1].Cells["StockItemName"].Value = bom.Name;
                    grdList.Rows[grdList.Rows.Count - 1].Cells["gModelNo"].Value = "NA";
                    grdList.Rows[grdList.Rows.Count - 1].Cells["RequiredQuantity"].Value = bom.Quantity * QuantTOPrepare;
                    grdList.Rows[grdList.Rows.Count - 1].Cells["StockQunatity"].Value = totalStock;
                    if ((bom.Quantity * QuantTOPrepare) > totalStock)
                        grdList.Rows[grdList.Rows.Count - 1].Cells["IssueQuantity"].Value = totalStock;
                    else
                        grdList.Rows[grdList.Rows.Count - 1].Cells["IssueQuantity"].Value = bom.Quantity * QuantTOPrepare;
                }
            }
            catch (Exception ex)
            {
            }
           
        }

        private void txtProdQuantity_TextChanged(object sender, EventArgs e)
        {
            string txt = txtProdQuantity.Text;
           
            try
            {
                if (txt == null || txt.Length == 0 || Convert.ToDouble(txt) == 0)
                {
                    txtProdQuantity.Text = "1";
                }
                double prepareQuant = Convert.ToDouble(txtProdQuantity.Text);
                ChangeGridReqQuant(prepareQuant);
               
                prevPrepQuant = Convert.ToDouble(txtProdQuantity.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Check product quanity string.");
            }
        }
        private void ChangeGridReqQuant(double prepareQuant)
        {
            
            foreach (DataGridViewRow row in grdList.Rows)
            {
                if(prepareQuant == 0)
                {
                    row.Cells["RequiredQuantity"].Value = Convert.ToDouble(0);
                }
                else
                {
                    
                    double bomQuant = Convert.ToDouble(row.Cells["RequiredQuantity"].Value) / prevPrepQuant;
                    row.Cells["RequiredQuantity"].Value = Convert.ToDouble(txtProdQuantity.Text) * bomQuant;
                }
               
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                st = 1;
                if(grdList.Rows.Count == 0)
                {
                    MessageBox.Show("Detail list is empty");
                    return;
                }
                if (!validateIssuedQuantity())
                {
                    return;
                }
                this.StockDetail = getStockOrderedDetail();
                DialogResult dialog = MessageBox.Show("Are you sure to add stock detail in stock issue?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.No)
                {
                    return;
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private List<stock> getStockOrderedDetail()
        {
            List<stock> stockList = new List<stock>();
            List<stock> substockList = new List<stock>();
            foreach(DataGridViewRow row in grdList.Rows)
            {
                substockList = StockDB.getStockDetailForStockIssue(row.Cells["StockItemID"].Value.ToString(),
                    row.Cells["gModelNo"].Value.ToString(), Convert.ToDouble(row.Cells["IssueQuantity"].Value.ToString()));
                stockList.AddRange(substockList);
            }
            showStockListOrderedInGridView(stockList);
            return stockList;
        }
        private Boolean validateIssuedQuantity()
        {
            Boolean stat = true;
            try
            {
                foreach (DataGridViewRow row in grdList.Rows)
                {
                    double stockQuant = Convert.ToDouble(row.Cells["StockQunatity"].Value);
                    double IssueQuant = Convert.ToDouble(row.Cells["IssueQuantity"].Value);
                    if (stockQuant < IssueQuant)
                    {
                        MessageBox.Show("Order Quantity More Than Stock Quantity.Check Row:" + (row.Index + 1));
                        stat = false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return stat;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void showStockListOrderedInGridView(List<stock> stockList)
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

                frmPopup.Size = new Size(1100, 400);

                grdStock = getGridViewOfFactoryWiseStock(stockList);
                grdStock.Bounds = new Rectangle(new Point(0, 0), new Size(1100, 350));
                frmPopup.Controls.Add(grdStock);

                Button lvOK = new Button();
                lvOK.BackColor = Color.Tan;
                lvOK.Text = "OK";
                lvOK.Location = new Point(20, 365);
                lvOK.Click += new System.EventHandler(this.lvOK_Click2);
                frmPopup.Controls.Add(lvOK);
                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void lvOK_Click2(object sender, EventArgs e)
        {
            frmPopup.Close();
            frmPopup.Dispose();
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
        static DataGridView getGridViewOfFactoryWiseStock(List<stock> StockList)
        {

            DataGridView grdStock = new DataGridView();
            try
            {
                string[] strColArr = { "StockRefNo", "StockItemID","StockItemName","ModelNo", "ModelName",
                    "MRNNo","MRNDate","PresentStock","OrderedStock","BatchNo","SerielNo","ExpiryDate","PurchaseQuanity","PurchasePrice","PurchaseTax","SupplierID","SupplierName"};
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),
                     new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                };

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdStock.EnableHeadersVisualStyles = false;
                grdStock.AllowUserToAddRows = false;
                grdStock.AllowUserToDeleteRows = false;
                grdStock.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdStock.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdStock.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdStock.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdStock.ColumnHeadersHeight = 27;
                grdStock.RowHeadersVisible = false;
                grdStock.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str;
                    colArr[index].HeaderText = str;
                    colArr[index].ReadOnly = true;
                    if (index == 6 || index == 11)
                        colArr[index].DefaultCellStyle.Format = "dd-MM-yyyy";
                    if (index == 2)
                        colArr[index].Width = 300;
                    else if (index == 16)
                        colArr[index].Width = 150;
                    else if (index == 12 || index == 13 || index == 14 || index == 8)
                        colArr[index].Width = 100;
                    else
                        colArr[index].Width = 80;

                    if (index == 1 || index == 3 || index == 4)
                        colArr[index].Visible = false;
                    else
                        colArr[index].Visible = true;
                    grdStock.Columns.Add(colArr[index]);
                }
                foreach (stock stk in StockList)
                {
                    grdStock.Rows.Add();
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[0]].Value = stk.RowID;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[1]].Value = stk.StockItemID;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[2]].Value = stk.StockItemName;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[3]].Value = stk.ModelNo;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[4]].Value = stk.ModelName;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[5]].Value = stk.MRNNo;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[6]].Value = stk.MRNDate;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[7]].Value = stk.PresentStock;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[8]].Value = stk.StockOnHold; //For Stock ORdered
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[9]].Value = stk.BatchNo;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[10]].Value = stk.SerielNo;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[11]].Value = stk.ExpiryDate;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[12]].Value = stk.PurchaseQuantity;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[13]].Value = stk.PurchasePrice;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[14]].Value = stk.PurchaseTax;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[15]].Value = stk.SupplierID;
                    grdStock.Rows[grdStock.Rows.Count - 1].Cells[strColArr[16]].Value = stk.SupplierName;
                }
            }
            catch (Exception ex)
            {
            }

            return grdStock;
        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = grdList.Columns[e.ColumnIndex].Name;
            if(colName == "ShowStockDetail")
            {
                try
                {
                    st = 0;
                    string stocktiemId = grdList.Rows[e.RowIndex].Cells["StockItemID"].Value.ToString();
                    string ModelNo = grdList.Rows[e.RowIndex].Cells["gModelNo"].Value.ToString();
                    double orderedQuant = Convert.ToDouble(grdList.Rows[e.RowIndex].Cells["IssueQuantity"].Value.ToString());
                    showStockOrderedItemDetails(stocktiemId, ModelNo, orderedQuant);
                }
                catch (Exception ex)
                {
                }
            }
            if (colName == "Delete")
            {
                try
                {
                    grdList.Rows.RemoveAt(e.RowIndex);
                }
                catch (Exception ex)
                {
                }
            }
        }
        private void showStockOrderedItemDetails(string itemid, string modno, double ordQuant)
        {
            List<stock> stockList = new List<stock>();
            stockList = StockDB.getStockDetailForStockIssue(itemid, modno, ordQuant);
            showStockListOrderedInGridView(stockList);
        }
    }
}
