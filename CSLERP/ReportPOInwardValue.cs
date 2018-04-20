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
using CSLERP.FileManager;
using System.Windows.Forms.DataVisualization.Charting;

namespace CSLERP
{
    public partial class ReportPOInwardValue : System.Windows.Forms.Form
    {
        string docID = "";
        List<ReportPO> PODetailList = new List<ReportPO>();
        List<ReportPO> InvoiceDetailList = new List<ReportPO>();
        int party = 0;
        int region = 0;
        List<int> matchedIOIndexList = new List<int>();
        public ReportPOInwardValue()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void ReportPOAnalysis_Load(object sender, EventArgs e)
        {
            //////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;

            String a = this.Size.ToString();
            grdDetailList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdDetailList.EnableHeadersVisualStyles = false;
            ShowControl();
        }
        private void ShowControl()
        {
            pnlList.Visible = true;
            grdDetailList.Visible = false;
            btnClose.Visible = false;
            //btnShowChart.Visible = false;
            txtTotalPOValue.Visible = false;
            lblPOTotal.Visible = false;
            txtTotalInvoiceValue.Visible = false;
            lblInvoiceTotal.Visible = false;
        }

        private void initVariables()
        {
            pnlShowChart.Visible = false;
            docID = Main.currentDocument;
            dtFromDate.Format = DateTimePickerFormat.Custom;
            dtFromDate.CustomFormat = "dd-MM-yyyy";
            dtToDate.Format = DateTimePickerFormat.Custom;
            dtToDate.CustomFormat = "dd-MM-yyyy";
            dtFromDate.Value = DateTime.Today;
            
            dtToDate.Value = DateTime.Today;
            pnlUI.Controls.Add(pnlList);
            enableBottomButtons();
            grdDetailList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //grdDetailList.Columns["Value"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            setTabIndex();
        }

        private void setTabIndex()
        {
            dtFromDate.TabIndex = 0;
            dtToDate.TabIndex = 1;
            chkProductPO.TabIndex = 2;
            chkservicePO.TabIndex = 3;
            chkPartWise.TabIndex = 4;
            chkRegionWise.TabIndex = 5;
            btnView.TabIndex = 6;
            rdbNormal.TabIndex = 7;
            rdbThousands.TabIndex = 8;
            rdbLakhs.TabIndex = 9;
            grdDetailList.TabIndex = 10;
        }
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
                dtFromDate.Value = DateTime.Today;
                dtToDate.Value = DateTime.Today;
                uncheckCheckBoxes();
                grdDetailList.Rows.Clear();
            }
            catch (Exception ex)
            {

            }
        }
        private void uncheckCheckBoxes()
        {
            chkPartWise.Checked = false;
            chkProductPO.Checked = false;
            chkservicePO.Checked = false;
            chkRegionWise.Checked = false;
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
            ///btnNew.Visible = false;
            btnExit.Visible = true;
            pnlBottomButtons.Visible = true;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            clearData();
            grdDetailList.Visible = false;
            btnClose.Visible = false;
            //btnShowChart.Visible = false;
            txtTotalPOValue.Visible = false;
            lblPOTotal.Visible = false;
            txtTotalInvoiceValue.Visible = false;
            lblInvoiceTotal.Visible = false;
            uncheckRadios(pnlList);
        }
        private void grdDetailList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void showGridColumns()
        {
            foreach (DataGridViewColumn col in grdDetailList.Columns)
            {
                col.Visible = true;
            }
        }
        private void uncheckRadios(Panel pnl)
        {
            try
            {
                foreach (RadioButton c in pnl.Controls.OfType<RadioButton>())
                {
                    c.Checked = false;
                }
            }
            catch (Exception ex)
            {
            }
        }
        private List<ReportPO> getInvoiceDetailListGroupByFilter(List<ReportPO> InvoiceDetailList,int opt)
        {
            List<ReportPO> InvoiceDetailListTemp = new List<ReportPO>();
            foreach (ReportPO rpo in InvoiceDetailList)
            {
                if (InvoiceDetailListTemp.Count != 0)
                {
                    //ReportPO rpoCheck = new ReportPO();
                    int index = -1;
                    if(opt == 1)
                        index = InvoiceDetailListTemp.FindIndex(rpoio => (rpoio.PartyID == rpo.PartyID) && (rpoio.DocumentType == rpo.DocumentType));
                    else if(opt == 2)
                        index = InvoiceDetailListTemp.FindIndex(rpoio => (rpoio.RegionID == rpo.RegionID) && (rpoio.DocumentType == rpo.DocumentType));
                    else if(opt == 3)
                        index = InvoiceDetailListTemp.FindIndex(rpoio => (rpoio.DocumentType == rpo.DocumentType));
                    if(index != -1)
                    {
                        InvoiceDetailListTemp[index].Value = InvoiceDetailListTemp[index].Value + rpo.Value;
                    }
                    else
                        InvoiceDetailListTemp.Add(rpo);
                    //foreach (ReportPO rpo1 in InvoiceDetailListTemp)
                    //{
                    //    if (rpo1.PartyID == rpo.PartyID && rpo1.DocumentType == rpo.DocumentType && opt == 1)
                    //    {
                    //        rpo1.Value = rpo1.Value + rpo.Value;
                    //    }
                    //    else if (rpo1.RegionID == rpo.RegionID && rpo1.DocumentType == rpo.DocumentType && opt == 2)
                    //    {
                    //        rpo1.Value = rpo1.Value + rpo.Value;
                    //    }
                    //    else if (rpo1.DocumentType == rpo.DocumentType && opt == 3)
                    //    {
                    //        rpo1.Value = rpo1.Value + rpo.Value;
                    //    }
                    //    else
                    //        InvoiceDetailListTemp.Add(rpo);
                    //}
                }
                else
                    InvoiceDetailListTemp.Add(rpo);
            }
            return InvoiceDetailListTemp;
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                
                txtTotalPOValue.Text = "";
                txtTotalInvoiceValue.Text = "";
                showGridColumns();
                if (rdbNormal.Checked != true && rdbLakhs.Checked != true && rdbThousands.Checked != true)
                    rdbNormal.Checked = true;
                POPIHeaderDB RPODb = new POPIHeaderDB();
                InvoiceOutHeaderDB IODb = new InvoiceOutHeaderDB();
                DateTime fromDate = dtFromDate.Value;
                DateTime toDate = dtToDate.Value;
                int opt1 = 0; int opt2 = 0; int opt3 = 0; int opt4 = 0;
                // opt1 : PO Wise , opt2 : Sevice Wise  , opt3 : Party Wise ,  opt4 : Region WIse
                party = 0;
                region = 0;
                if (chkProductPO.Checked == true)
                    opt1 = 1;
                if (chkservicePO.Checked == true)
                    opt2 = 1;
                if (chkPartWise.Checked == true)
                {
                    opt3 = 1;
                    party = 1;
                }
                if (chkRegionWise.Checked == true)
                {
                    opt4 = 1;
                    region = 1;
                }
                if (opt1 != 1 && opt2 != 1)
                {
                    MessageBox.Show("select PO Type");
                    return;
                }
                if (opt3 == 1 && opt4 == 1)
                {
                    MessageBox.Show("Either select party or region.");
                    return;
                }
                if (opt3 == 1 || opt4 == 1)
                {
                    if (opt3 == 1)
                    {
                        grdDetailList.Columns["gRegion"].Visible = false;
                        PODetailList = RPODb.getPODetailForpartWise(opt1, opt2, fromDate, toDate);
                        List<ReportPO> IODetailList1 = IODb.getIODetailForpartWise(opt1, opt2, fromDate, toDate);
                        InvoiceDetailList = getInvoiceDetailListGroupByFilter(IODetailList1,1);
                    }
                    else
                    {
                        grdDetailList.Columns["gParty"].Visible = false;
                        PODetailList = RPODb.getDetailForRegionWise(opt1, opt2, fromDate, toDate);
                        List<ReportPO> IODetailList1 = IODb.getIODetailForRegionWise(opt1, opt2, fromDate, toDate);
                        InvoiceDetailList = getInvoiceDetailListGroupByFilter(IODetailList1,2);
                    }
                }
                else if (opt1 == 1 || opt2 == 1)
                {
                    grdDetailList.Columns["gRegion"].Visible = false;
                    grdDetailList.Columns["gParty"].Visible = false;
                    PODetailList = RPODb.getDetailForDocumentWise(opt1, opt2, fromDate, toDate);
                   
                    List<ReportPO> IODetailList1 = IODb.getIODetailForDocumentWise(opt1, opt2, fromDate, toDate);
                    InvoiceDetailList = getInvoiceDetailListGroupByFilter(IODetailList1,3);
                }
                else
                {
                    MessageBox.Show("Select one ");
                    return;
                }
                
                
                //foreach (ReportPO rpo in InvoiceDetailList)
                //{
                //    var sel = 
                //}
                txtTotalPOValue.Visible = true;
                lblPOTotal.Visible = true;
                txtTotalInvoiceValue.Visible = true;
                lblInvoiceTotal.Visible = true;
                grdDetailList.Visible = true;
                btnClose.Visible = true;
                //btnShowChart.Visible = true;
                addItemsInGridDetail(PODetailList, InvoiceDetailList);
            }
            catch (Exception ex)
            {
            }
        }
        private string getPOType(string Docid)
        {
            string potype = "";
            if (Docid.Equals("POPRODUCTINWARD"))
                potype = "Product PO";
            else
                potype = "Service PO";
            return potype;
        }
        private void chkProductPO_Click(object sender, EventArgs e)
        {
            grdDetailList.Rows.Clear();
            txtTotalPOValue.Text = "";
            txtTotalInvoiceValue.Text = "";
        }

        private void chkservicePO_Click(object sender, EventArgs e)
        {
            grdDetailList.Rows.Clear();
            txtTotalPOValue.Text = "";
            txtTotalInvoiceValue.Text = "";
        }

        private void chkPartWise_Click(object sender, EventArgs e)
        {
            grdDetailList.Rows.Clear();
            txtTotalPOValue.Text = "";
            txtTotalInvoiceValue.Text = "";
        }

        private void chkRegionWise_Click(object sender, EventArgs e)
        {
            grdDetailList.Rows.Clear();
            txtTotalPOValue.Text = "";
            txtTotalInvoiceValue.Text = "";
        }
        int type = 5;  // for Chart Type selection
        int opt = 0;   // for checkbox checked options
        private void btnShowChart_Click(object sender, EventArgs e)
        {
            try
            {
                type = 5;
                opt = 0;
                if (grdDetailList.Rows.Count == 0)
                {
                    MessageBox.Show("No Items in Grid");
                    return;
                }
                pnlShowChart.Visible = true;
                chrtProductPO.Series["Product PO"].Points.Clear();
                chrtServicePO.Series["Service PO"].Points.Clear();
                chrtProductPO.ChartAreas[0].AxisY.Title = "Value";
                chrtServicePO.ChartAreas[0].AxisY.Title = "Value";
                chrtProductPO.ChartAreas[0].AxisX.Interval = 1;
                chrtServicePO.ChartAreas[0].AxisX.Interval = 1;
                chrtServicePO.Visible = true;
                chrtProductPO.Visible = true;
                chrtProductPO.Bounds = new Rectangle(new Point(11, 27), new Size(480, 348));
                chrtServicePO.Bounds = new Rectangle(new Point(497, 27), new Size(480, 348));
                chkGraphGrid.Checked = true;
                uncheckRadios(pnlShowChart);
                rdbColumn.Checked = true;
                if (chkProductPO.Checked == true && chkservicePO.Checked == true &&
                    (chkPartWise.Checked == true || chkRegionWise.Checked == true))
                {
                    chrtServicePO.Visible = true;
                    chrtProductPO.Visible = true;
                    opt = 4;
                }
                else if (chkProductPO.Checked == true && chkservicePO.Checked == true)
                {
                    //chrtProductPO.Bounds = new Rectangle(new Point(90, 11), new Size(886, 384));
                    chrtProductPO.Visible = false;
                    opt = 3;

                }
                else if ((chkProductPO.Checked == true || chkservicePO.Checked == true) &&
                    (chkPartWise.Checked == true || chkRegionWise.Checked == true))
                {
                    if (chkProductPO.Checked == true)
                    {
                        chrtServicePO.Visible = false;
                        chrtProductPO.Visible = true;
                        chrtProductPO.Bounds = new Rectangle(new Point(90, 11), new Size(886, 384));
                        opt = 1;
                    }
                    else
                    {
                        chrtServicePO.Visible = true;
                        chrtProductPO.Visible = false;
                        chrtServicePO.Bounds = new Rectangle(new Point(90, 11), new Size(886, 384));
                        opt = 2;
                    }
                }
                else
                {
                    chrtProductPO.Visible = false;
                    opt = 3;
                }
                showChart(opt, type);
            }
            catch (Exception ex)
            {
            }

        }

        private void showChart(int opt, int type)
        {
            try
            {
                int check = 0;
                if (chkPartWise.Checked == true)
                {
                    check = 1;
                    chrtProductPO.ChartAreas[0].AxisX.Title = "Party";
                    chrtServicePO.ChartAreas[0].AxisX.Title = "Party";
                }
                else if (chkRegionWise.Checked == true)
                {
                    check = 2;
                    chrtProductPO.ChartAreas[0].AxisX.Title = "Region";
                    chrtServicePO.ChartAreas[0].AxisX.Title = "Region";
                }
                else
                {
                    chrtServicePO.ChartAreas[0].AxisX.Title = "PO Type";
                }
                changeChartType(type);
                for (int i = 0; i < grdDetailList.Rows.Count; i++)
                {
                    if (opt == 4)
                    {
                        if (grdDetailList.Rows[i].Cells["POType"].Value.ToString().Equals("Product PO"))
                        {
                            if (check == 1)
                                chrtProductPO.Series["Product PO"].Points.
                                    AddXY(grdDetailList.Rows[i].Cells["Party"].Value.ToString(),
                                                        Convert.ToDouble(grdDetailList.Rows[i].Cells["Value"].Value).ToString("F2"));
                            else
                                chrtProductPO.Series["Product PO"].Points
                                    .AddXY(grdDetailList.Rows[i].Cells["gRegion"].Value.ToString(),
                                                        Convert.ToDouble(grdDetailList.Rows[i].Cells["Value"].Value).ToString("F2"));
                        }
                        if (grdDetailList.Rows[i].Cells["POType"].Value.ToString().Equals("Service PO"))
                        {
                            if (check == 1)
                                chrtServicePO.Series["Service PO"].Points.
                                    AddXY(grdDetailList.Rows[i].Cells["Party"].Value.ToString(),
                                                        Convert.ToDouble(grdDetailList.Rows[i].Cells["Value"].Value).ToString("F2"));
                            else
                                chrtServicePO.Series["Service PO"].Points.
                                    AddXY(grdDetailList.Rows[i].Cells["gRegion"].Value.ToString(),
                                                        Convert.ToDouble(grdDetailList.Rows[i].Cells["Value"].Value).ToString("F2"));
                        }
                    }
                    else if (opt == 1)
                    {
                        if (check == 1)
                            chrtProductPO.Series["Product PO"].Points.
                                AddXY(grdDetailList.Rows[i].Cells["Party"].Value.ToString(),
                                                         Convert.ToDouble(grdDetailList.Rows[i].Cells["Value"].Value).ToString("F2"));
                        else
                            chrtProductPO.Series["Product PO"].Points
                                .AddXY(grdDetailList.Rows[i].Cells["gRegion"].Value.ToString(),
                                                         Convert.ToDouble(grdDetailList.Rows[i].Cells["Value"].Value).ToString("F2"));
                    }
                    else if (opt == 2)
                    {
                        if (check == 1)
                            chrtServicePO.Series["Service PO"].Points.
                                AddXY(grdDetailList.Rows[i].Cells["Party"].Value.ToString(),
                                                         Convert.ToDouble(grdDetailList.Rows[i].Cells["Value"].Value).ToString("F2"));
                        else
                            chrtServicePO.Series["Service PO"].Points.
                                AddXY(grdDetailList.Rows[i].Cells["gRegion"].Value.ToString(),
                                                         Convert.ToDouble(grdDetailList.Rows[i].Cells["Value"].Value).ToString("F2"));
                    }
                    else
                    {
                        chrtServicePO.Series["Service PO"].Points.
                               AddXY(grdDetailList.Rows[i].Cells["POType"].Value.ToString(),
                                                         Convert.ToDouble(grdDetailList.Rows[i].Cells["Value"].Value).ToString("F2"));
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btncls_Click(object sender, EventArgs e)
        {
            pnlShowChart.Visible = false;
        }

        private void chkGraphGrid_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGraphGrid.Checked == true)
            {
                chrtProductPO.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                chrtProductPO.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
                chrtServicePO.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                chrtServicePO.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            }
            else
            {
                chrtProductPO.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chrtProductPO.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                chrtServicePO.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chrtServicePO.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            }
        }
        private void changeChartType(int type)
        {
            chrtProductPO.Series["Product PO"].Points.Clear();
            chrtServicePO.Series["Service PO"].Points.Clear();
            switch (type)
            {
                case 1:
                    chrtProductPO.Series["Product PO"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
                    chrtServicePO.Series["Service PO"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

                    break;
                case 2:
                    chrtProductPO.Series["Product PO"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    chrtServicePO.Series["Service PO"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    break;
                case 3:
                    chrtProductPO.Series["Product PO"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                    chrtServicePO.Series["Service PO"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                    break;
                case 4:
                    chrtProductPO.Series["Product PO"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                    chrtServicePO.Series["Service PO"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                    break;
                case 5:
                    chrtProductPO.Series["Product PO"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    chrtServicePO.Series["Service PO"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    break;
                case 6:
                    chrtProductPO.Series["Product PO"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
                    chrtServicePO.Series["Service PO"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
                    break;
            }
            if (type == 1)
            {
                chrtProductPO.Series["Product PO"].LegendText = "#AXISLABEL";
                chrtServicePO.Series["Service PO"].LegendText = "#AXISLABEL";
            }
            else
            {
                chrtProductPO.Series["Product PO"].LegendText = "#SERIESNAME";
                chrtServicePO.Series["Service PO"].LegendText = "#SERIESNAME";
            }
        }
        private void rbdPie_Click(object sender, EventArgs e)
        {
            showChart(opt, 1);
        }

        private void rbdList_Click(object sender, EventArgs e)
        {
            showChart(opt, 2);
        }

        private void rdbBar_Click(object sender, EventArgs e)
        {
            showChart(opt, 4);
        }

        private void rdbColumn_Click(object sender, EventArgs e)
        {
            showChart(opt, 5);
        }

        private void rdbArea_Click(object sender, EventArgs e)
        {
            showChart(opt, 6);
        }

        private void btndefault_Click(object sender, EventArgs e)
        {
            uncheckRadios(pnlShowChart);
            showChart(opt, 5);
        }

        private void rdbNormal_Click(object sender, EventArgs e)
        {
            addItemsInGridDetail(PODetailList, InvoiceDetailList);
        }

        private void rdbThousands_Click(object sender, EventArgs e)
        {
            addItemsInGridDetail(PODetailList, InvoiceDetailList);
        }

        private void rdbLakhs_Click(object sender, EventArgs e)
        {
            addItemsInGridDetail(PODetailList, InvoiceDetailList);
        }
        private ReportPO getMatchedInvoiceDetails(List<ReportPO> InvoiceDetailList, ReportPO POrpo)
        {
            ReportPO matchedRIO = new ReportPO();
            if (party == 1)
            {
                matchedRIO = InvoiceDetailList.FirstOrDefault(rpoio => (rpoio.PartyID == POrpo.PartyID) && (rpoio.DocumentType == POrpo.DocumentType));
                if (matchedRIO != null)
                {
                    int index = InvoiceDetailList.FindIndex(rpoio => (rpoio.PartyID == POrpo.PartyID) && (rpoio.DocumentType == POrpo.DocumentType));
                    matchedIOIndexList.Add(index);
                }
            }
            else if (region == 1)
            {
                matchedRIO = InvoiceDetailList.FirstOrDefault(rpoio => (rpoio.RegionID == POrpo.RegionID) && (rpoio.DocumentType == POrpo.DocumentType));
                if (matchedRIO != null)
                {
                    int index = InvoiceDetailList.FindIndex(rpoio => (rpoio.RegionID == POrpo.RegionID) && (rpoio.DocumentType == POrpo.DocumentType));
                    matchedIOIndexList.Add(index);
                }
            }
            else
            {
                matchedRIO = InvoiceDetailList.FirstOrDefault(rpoio => (rpoio.DocumentType == POrpo.DocumentType));
                if (matchedRIO != null)
                {
                    int index = InvoiceDetailList.FindIndex(rpoio => (rpoio.DocumentType == POrpo.DocumentType));
                    matchedIOIndexList.Add(index);
                }
            }


            return matchedRIO;
        }
        private void addItemsInGridDetail(List<ReportPO> PODetailList, List<ReportPO> InvoiceDetailList)
        {
            try
            {
                grdDetailList.Rows.Clear();
                matchedIOIndexList.Clear();
                int i = 1;
                double totalPOValue = 0;
                double totalIOValue = 0;
                foreach (ReportPO rpo in PODetailList)
                {
                    double InvValue = 0;
                    ReportPO matchedIO = getMatchedInvoiceDetails(InvoiceDetailList, rpo);
                    if (matchedIO != null)
                        InvValue = matchedIO.Value;
                    else
                        InvValue = 0;
                    grdDetailList.Rows.Add();
                    //grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["LineNo"].Value = i;
                    grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["POType"].Value = rpo.DocumentType;
                    if (party == 1)
                        grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["gParty"].Value = rpo.Name; // For party name
                    else if (region == 1)
                        grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["gRegion"].Value = rpo.Name;  // For region Name
                                                                                                           //else
                                                                                                           //    grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["gRegion"].Value = rpo.Name;  // For region Name
                    if (rdbNormal.Checked == true)
                    {
                        grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["POValue"].Value = rpo.Value;
                        grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["InvoiceValue"].Value = InvValue;
                    }
                    else if (rdbThousands.Checked == true)
                    {
                        grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["POValue"].Value = rpo.Value / 1000;
                        grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["InvoiceValue"].Value = InvValue / 1000;
                    }
                    else if (rdbLakhs.Checked == true)
                    {
                        grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["POValue"].Value = rpo.Value / 100000;
                        grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["InvoiceValue"].Value = InvValue / 100000;
                    }
                    else
                    {
                        grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["POValue"].Value = rpo.Value;
                        grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["InvoiceValue"].Value = InvValue;
                    }
                    totalPOValue = totalPOValue + Convert.ToDouble(grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["POValue"].Value);
                    totalIOValue = totalIOValue + Convert.ToDouble(grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["InvoiceValue"].Value);
                    i++;
                }
                if (InvoiceDetailList.Count != matchedIOIndexList.Count)
                {
                    int track = 0;
                    for (track = 0; track < InvoiceDetailList.Count; track++)
                    {
                        if (!matchedIOIndexList.Contains(track))
                        {
                            ReportPO matchedIO = InvoiceDetailList[track];
                            grdDetailList.Rows.Add();
                            //grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["LineNo"].Value = i;
                            grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["POType"].Value = matchedIO.DocumentType;
                            if (party == 1)
                                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["gParty"].Value = matchedIO.Name; // For party name
                            else if (region == 1)
                                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["gRegion"].Value = matchedIO.Name;  // For region Name

                            if (rdbNormal.Checked == true)
                            {
                                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["POValue"].Value = 0;
                                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["InvoiceValue"].Value = matchedIO.Value;
                            }
                            else if (rdbThousands.Checked == true)
                            {
                                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["POValue"].Value = 0;
                                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["InvoiceValue"].Value = matchedIO.Value / 1000;
                            }
                            else if (rdbLakhs.Checked == true)
                            {
                                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["POValue"].Value = 0;
                                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["InvoiceValue"].Value = matchedIO.Value / 100000;
                            }
                            else
                            {
                                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["POValue"].Value = 0;
                                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["InvoiceValue"].Value = matchedIO.Value;
                            }
                            totalPOValue = totalPOValue + Convert.ToDouble(grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["POValue"].Value);
                            totalIOValue = totalIOValue + Convert.ToDouble(grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["InvoiceValue"].Value);
                        }
                    }
                }
                if (rdbLakhs.Checked == true)
                {
                    txtTotalPOValue.Text = totalPOValue.ToString("F2");
                    txtTotalInvoiceValue.Text = totalIOValue.ToString("F2");
                    grdDetailList.Columns["POValue"].DefaultCellStyle.Format = "N2";
                    grdDetailList.Columns["InvoiceValue"].DefaultCellStyle.Format = "N2";
                }
                else
                {
                    txtTotalPOValue.Text = totalPOValue.ToString("F0");
                    txtTotalInvoiceValue.Text = totalIOValue.ToString("F0");
                    grdDetailList.Columns["POValue"].DefaultCellStyle.Format = "N0";
                    grdDetailList.Columns["InvoiceValue"].DefaultCellStyle.Format = "N0";
                }
                if (chkRegionWise.Checked == false && chkPartWise.Checked == false)
                {
                    grdDetailList.Columns["POType"].Width = 400;
                }
                if (chkRegionWise.Checked == true || chkPartWise.Checked == true)
                {
                    grdDetailList.Columns["gParty"].Width = 300;
                    grdDetailList.Columns["gRegion"].Width = 300;
                    grdDetailList.Columns["POType"].Width = 100;
                }
                if (chkRegionWise.Checked == true)
                    grdDetailList.Sort(grdDetailList.Columns["gRegion"], System.ComponentModel.ListSortDirection.Ascending);
                else if (chkPartWise.Checked == true)
                    grdDetailList.Sort(grdDetailList.Columns["gParty"], System.ComponentModel.ListSortDirection.Ascending);
                foreach (DataGridViewRow row in grdDetailList.Rows)
                {
                    row.Cells["LineNo"].Value = row.Index + 1;
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void grdDetailList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //int col = e.ColumnIndex;
            //int row = e.RowIndex;
            //int localX =
        }

        private void ReportPOInwardValue_Enter(object sender, EventArgs e)
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
    public class ReportPO
    {
        public string DocumentType { get; set; }
        public string DocumentID { get; set; }
        public string PartyID { get; set; }
        public string RegionID { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
    }
}



