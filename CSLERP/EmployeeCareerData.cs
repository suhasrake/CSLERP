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
    public partial class EmployeeCareerData : System.Windows.Forms.Form
    {
        string docID = "EMPCAREERDATA";
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        TextBox txtSearch = new TextBox();
        ListView lvCopy = new ListView();
        public EmployeeCareerData()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void EmployeeCareerData_Load(object sender, EventArgs e)
        {
            //////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;
            String a = this.Size.ToString();
            grdEmpDesig.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdEmpCTC.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdEmpPosting.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdEmpDesig.EnableHeadersVisualStyles = false;
            grdEmpCTC.EnableHeadersVisualStyles = false;
            grdEmpPosting.EnableHeadersVisualStyles = false;
            grdEmpDesig.Columns.Cast<DataGridViewColumn>().ToList().ForEach(f => f.SortMode = DataGridViewColumnSortMode.NotSortable);
            grdEmpCTC.Columns.Cast<DataGridViewColumn>().ToList().ForEach(f => f.SortMode = DataGridViewColumnSortMode.NotSortable);
            grdEmpPosting.Columns.Cast<DataGridViewColumn>().ToList().ForEach(f => f.SortMode = DataGridViewColumnSortMode.NotSortable);
            ShowControl();
        }
        private void ShowControl()
        {
            pnlList.Visible = true;
            pnlShowgraph.Visible = false;
        }
        private void initVariables()
        {

            docID = Main.currentDocument;
            dtDOB.Format = DateTimePickerFormat.Custom;
            dtDOB.CustomFormat = "dd-MM-yyyy";
            dtDOJ.Format = DateTimePickerFormat.Custom;
            dtDOJ.CustomFormat = "dd-MM-yyyy";
            pnlUI.Controls.Add(pnlList);
            enableBottomButtons();
            //EmployeeDB.fillEmpListCombo(cmbEmployee);
            grdEmpDesig.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grdEmpCTC.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grdEmpPosting.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            rdbTable.Checked = true;
            btnSelect.Visible = true;
            btnSelect.Focus();
            btnSelect.BringToFront();
        }
        public void clearData()
        {
            try
            {
                grdEmpCTC.Rows.Clear();
                grdEmpPosting.Rows.Clear();
                grdEmpDesig.Rows.Clear();
                setTableVisibility();
                //cmbEmployee.SelectedIndex = -1;
                dtDOB.Value = DateTime.Parse("1990-01-01");
                dtDOJ.Value = DateTime.Parse("1990-01-01");
                empPicture.Image = null;
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
            btnExit.Visible = true;
            pnlBottomButtons.Visible = true;
        }
        private void ShowEmployeeInfo()
        {
            try
            {
                if (txtEMpID.Text.Length != 0)
                {
                    ////////byte[] empPic;
                    grdEmpDesig.Rows.Clear();
                    grdEmpCTC.Rows.Clear();
                    grdEmpPosting.Rows.Clear();
                    pnlShowgraph.Visible = false;

                    pnlShowgraph.Visible = false;
                    rdbTable.Checked = true;

                    EmployeeDB empDB = new EmployeeDB();
                    List<employee> EMPList = empDB.getEmployees();
                    int empID = Convert.ToInt32(txtEMpID.Text);
                    txtEMpID.Visible = true;
                    foreach (employee emp in EMPList)
                    {
                        if (emp.empID == empID && emp.empStatus == 1)
                        {
                            dtDOB.Value = emp.empDOB;
                            dtDOJ.Value = emp.empDOJ;

                            try
                            {
                                //byte[] data = (byte[])grdList.Rows[e.RowIndex].Cells["EmpImage"].Value;
                                byte[] data = EmployeeDB.getPictureOfEmployee(txtEMpID.Text);
                                MemoryStream ms = new MemoryStream(data);
                                empPicture.Image = Image.FromStream(ms);
                            }
                            catch (Exception)
                            {
                                empPicture.Image = null;
                            }

                        }
                    }
                    showDataTables();
                }
            }
            catch (Exception edx)
            {
            }
        }
        private void showDataTables()
        {
            try
            {
                string str = txtEMpID.Text;
                List<employeeposting> empPosting = EmployeePostingDB.getEmployeePosting(str);
                foreach (employeeposting epost in empPosting)
                {
                    if (epost.Status == 1)
                    {
                        grdEmpPosting.Rows.Add();
                        grdEmpPosting.Rows[grdEmpPosting.RowCount - 1].Cells["MontYearPost"].Value = epost.postingDate.ToString("MMM-yyyy");
                        grdEmpPosting.Rows[grdEmpPosting.RowCount - 1].Cells["Office"].Value = epost.officeName;
                        grdEmpPosting.Rows[grdEmpPosting.RowCount - 1].Cells["Department"].Value = epost.departmentName;
                    }
                }
                List<employeeDesignation> EmployeeDesig = EmployeeDB.getEmployeeDesignation(str);
                foreach (employeeDesignation emp1 in EmployeeDesig)
                {
                    if (emp1.status == 1)
                    {
                        grdEmpDesig.Rows.Add();
                        grdEmpDesig.Rows[grdEmpDesig.RowCount - 1].Cells["DMonthYear"].Value = emp1.Descdate.ToString("MMM-yyyy");
                        grdEmpDesig.Rows[grdEmpDesig.RowCount - 1].Cells["Designation"].Value = emp1.designationDescription;
                    }
                }
                List<EmployeePayRevision> EmpPayList = EmployeeDB.getEmployeePayRevDetail(str);
                foreach (EmployeePayRevision emprev in EmpPayList)
                {
                    if (emprev.status == 1)
                    {
                        grdEmpCTC.Rows.Add();
                        grdEmpCTC.Rows[grdEmpCTC.RowCount - 1].Cells["MonthYearCTC"].Value = emprev.RevisionDate.ToString("MMM-yyyy");
                        grdEmpCTC.Rows[grdEmpCTC.RowCount - 1].Cells["FixPay"].Value = emprev.FixPay;
                        grdEmpCTC.Rows[grdEmpCTC.RowCount - 1].Cells["VariablePay"].Value = emprev.VariablePay;
                        grdEmpCTC.Rows[grdEmpCTC.RowCount - 1].Cells["ctcDesignation"].Value = emprev.Designation;
                        grdEmpCTC.Rows[grdEmpCTC.RowCount - 1].Cells["ctcOffice"].Value = emprev.Office;
                        if (emprev.VariablePay > 0)
                        {
                            grdEmpCTC.Rows[grdEmpCTC.RowCount - 1].Cells["VariablePayPercent"].Value = emprev.VPPercentage;
                        }
                        else
                        {
                            grdEmpCTC.Rows[grdEmpCTC.RowCount - 1].Cells["VariablePayPercent"].Value = 0;
                        }
                        grdEmpCTC.Rows[grdEmpCTC.RowCount - 1].Cells["CTCInLakh"].Value = Math.Round( (emprev.FixPay + (emprev.VariablePay * emprev.VPPercentage / 100)) / 100000,2);
                        if (grdEmpCTC.RowCount >= 2)
                        {
                            decimal dd;
                            try
                            {
                                dd = Convert.ToDecimal(grdEmpCTC.Rows[grdEmpCTC.RowCount - 2].Cells["CTCInLakh"].Value) * 100000;
                            }
                            catch (Exception)
                            {
                                dd = 0;
                            }
                            decimal d1 = (emprev.FixPay + (emprev.VariablePay * emprev.VPPercentage / 100));
                            decimal dec = d1 - dd;
                            decimal d2;
                            if (dd != 0)
                                d2 = dec / dd * 100;
                            else
                                d2 = 100;
                            grdEmpCTC.Rows[grdEmpCTC.RowCount - 1].Cells["IncreasePercentage"].Value = Math.Round(d2) + " % ";
                        }
                        setTableVisibility();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void setTableVisibility()
        {
            if (grdEmpPosting.Rows.Count > 0 || grdEmpDesig.Rows.Count > 0 || grdEmpCTC.Rows.Count > 0)
            {
                grdEmpPosting.Visible = true;
                grdEmpDesig.Visible = true;
                grdEmpCTC.Visible = true;
                lblOffice.Visible = true;
                lblDesig.Visible = true;
                lblCTC.Visible = true;
                
            }
            else
            {
                grdEmpPosting.Visible = false;
                grdEmpDesig.Visible = false;
                grdEmpCTC.Visible = false;
                lblOffice.Visible = false;
                lblDesig.Visible = false;
                lblCTC.Visible = false;
                
            }
        }
        private void rdbTable_Click(object sender, EventArgs e)
        {
            if (grdEmpCTC.Rows.Count == 0 && grdEmpDesig.Rows.Count == 0 && grdEmpPosting.Rows.Count == 0)
            {
                MessageBox.Show("Data not available");
                return;
            }
            setTableVisibility();
            pnlShowgraph.Visible = false;

        }

        private void rdbGraph_Click(object sender, EventArgs e)
        {
            if (grdEmpCTC.Rows.Count == 0)
            {
                MessageBox.Show("CTC data not available");
                return;
            }
            setTableVisibility();
            pnlShowgraph.Visible = true;
            ShowChartInPanel();
        }

        private void btnCloseGraph_Click(object sender, EventArgs e)
        {
            setTableVisibility();
            pnlShowgraph.Visible = false;
            rdbTable.Checked = true;
            // btnCloseGraph,visi

        }
        private void ShowChartInPanel()
        {
            try
            {
                chrtCTC.Series["CTC"].Points.Clear();
                chrtCTC.ChartAreas[0].AxisY.Title = "CTC In Lakhs";
                chrtCTC.ChartAreas[0].AxisX.Interval = 1;
                chrtCTC.Visible = true;
                chkGraphGrid.Checked = true;
                uncheckRadios(pnlShowgraph);
                rdbColumn.Checked = true;
                showChart(5);
            }
            catch (Exception ex)
            {
            }

        }
        private void rbdPie_Click(object sender, EventArgs e)
        {
            showChart(1);
        }

        private void rbdList_Click(object sender, EventArgs e)
        {
            showChart(2);
        }

        private void rdbBar_Click(object sender, EventArgs e)
        {
            showChart(4);
        }

        private void rdbColumn_Click(object sender, EventArgs e)
        {
            showChart(5);
        }

        private void rdbArea_Click(object sender, EventArgs e)
        {
            showChart(6);
        }

        private void btndefault_Click(object sender, EventArgs e)
        {
            uncheckRadios(pnlShowgraph);
            rdbColumn.Checked = true;
            showChart(5);
        }

        private void btncls_Click(object sender, EventArgs e)
        {

        }

        private void chkGraphGrid_CheckedChanged(object sender, EventArgs e)
        {
            if (chkGraphGrid.Checked == true)
            {
                chrtCTC.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
                chrtCTC.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            }
            else
            {
                chrtCTC.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                chrtCTC.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            }
        }
        private void changeChartType(int type)
        {
            chrtCTC.Series["CTC"].Points.Clear();
            switch (type)
            {
                case 1:
                    chrtCTC.Series["CTC"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
                    break;
                case 2:
                    chrtCTC.Series["CTC"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                    break;
                case 3:
                    chrtCTC.Series["CTC"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                    break;
                case 4:
                    chrtCTC.Series["CTC"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Bar;
                    break;
                case 5:
                    chrtCTC.Series["CTC"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    break;
                case 6:
                    chrtCTC.Series["CTC"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
                    break;
            }
            if (type == 1)
            {
                chrtCTC.Series["CTC"].IsVisibleInLegend = true;
                chrtCTC.Series["CTC"].LegendText = "#AXISLABEL";
            }
            else
            {
                //chrtCTC.Series["CTC"].LegendText = "#SERIESNAME";
                chrtCTC.Series["CTC"].IsVisibleInLegend = false;
            }
        }
        private void showChart(int type)
        {
            pnlShowgraph.Location = new Point(5, 25);
            pnlShowgraph.Size = new Size(1030, 441);
            pnlShowgraph.BringToFront();
            pnlShowgraph.Focus();
            chrtCTC.Size = new Size(910, 391);
            rbdPie.Location = new Point(945, 105);
            rbdList.Location = new Point(945, 128);
            rdbBar.Location = new Point(945, 151);
            rdbColumn.Location = new Point(945, 175);
            rdbArea.Location = new Point(945, 199);
            btndefault.Location = new Point(945, 222);
            chkGraphGrid.Location = new Point(945, 251);
            btnCloseGraph.Location = new Point(945, 274);


            try
            {
                chrtCTC.ChartAreas[0].AxisX.Title = "Month-Year";

                changeChartType(type);
                for (int i = 0; i < grdEmpCTC.Rows.Count; i++)
                {
                    chrtCTC.Series["CTC"].Points.
                          AddXY(grdEmpCTC.Rows[i].Cells["MonthYearCTC"].Value.ToString(),
                                      Convert.ToDouble(grdEmpCTC.Rows[i].Cells["CTCInLakh"].Value).ToString("F2"));
                }
            }
            catch (Exception ex)
            {
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
        private void removeControlsFromLVPanel()
        {
            try
            {
                ////////foreach (Control p in pnllv.Controls)
                ////////    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
                ////////    {
                ////////        p.Dispose();
                ////////    }

                pnllv.Controls.Clear();
                Control nc = pnllv.Parent;
                nc.Controls.Remove(pnllv);
            }
            catch (Exception ex)
            {
            }
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            removeControlsFromLVPanel();
            pnlList.Controls.Remove(pnllv);
            btnSelect.Enabled = false;
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;
            pnllv.BackColor = Color.DarkSeaGreen;
            pnllv.Bounds = new Rectangle(new Point(24, 32), new Size(477, 282));
            lv = EmployeePostingDB.getEmployeeListView();
            lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView1_ItemChecked);
            lv.Bounds = new Rectangle(new Point(17, 14), new Size(443, 212));
            pnllv.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Location = new Point(44, 246);
            lvOK.Click += new System.EventHandler(this.lvOK_Click1);
            pnllv.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Location = new Point(141, 246);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click1);
            pnllv.Controls.Add(lvCancel);

            Label lblSearch = new Label();
            lblSearch.Text = "Find";
            lblSearch.Location = new Point(260, 250);
            lblSearch.Size = new Size(37, 15);
            pnllv.Controls.Add(lblSearch);

            txtSearch = new TextBox();
            txtSearch.Location = new Point(303, 246);
            txtSearch.TextChanged += new EventHandler(txtSearch_TextChangedEmp);
            pnllv.Controls.Add(txtSearch);

            pnlList.Controls.Add(pnllv);
            pnllv.BringToFront();
            pnllv.Visible = true;
            txtSearch.Focus();

        }
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
        //private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
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
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                btnSelect.Enabled = true;
                if (lv.Visible == true)
                {
                    if (!checkLVItemChecked("Employee"))
                    {
                        return;
                    }
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            txtEMpID.Text = itemRow.SubItems[1].Text;
                            txtEmpName.Text = itemRow.SubItems[2].Text;
                            txtEmpName.Visible = true;
                            ShowEmployeeInfo();
                        }
                    }
                }
                else
                {
                    if (!checkLVCopyItemChecked("Employee"))
                    {
                        return;
                    }
                    foreach (ListViewItem itemRow in lvCopy.Items)
                    {
                        if (itemRow.Checked)
                        {
                            txtEMpID.Text = itemRow.SubItems[1].Text;
                            txtEmpName.Text = itemRow.SubItems[2].Text;
                            ShowEmployeeInfo();
                        }
                    }
                }
                setTableVisibility();
                pnllv.Visible = false;

                dtDOB.Visible = true;
                dtDOJ.Visible = true;
                lblDOB.Visible = true;
                lblDOJ.Visible = true;

            }
            catch (Exception ex)
            {
            }
        }

        private void lvCancel_Click1(object sender, EventArgs e)
        {
            try
            {
                btnSelect.Enabled = true;
                pnllv.Visible = false;
            }
            catch (Exception)
            {
            }
        }
        private void txtSearch_TextChangedEmp(object sender, EventArgs e)
        {
            pnllv.Controls.Remove(lvCopy);
            addItemsEmp();
        }
        private void addItemsEmp()
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
            //this.lvCopy.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.CopylistView2_ItemChecked);
            //lvCopy.Location = new Point(13, 9);
            //lvCopy.Size = new Size(440, 199);
            lvCopy.Bounds = new Rectangle(new Point(17, 14), new Size(443, 212));
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
            pnllv.Controls.Add(lvCopy);
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

        private void chrtCTC_GetToolTipText(object sender, System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs e)
        {
            ////// Check selected chart element and set tooltip text for it
            ////switch (e.HitTestResult.ChartElementType)
            ////{
            ////    case System.Windows.Forms.DataVisualization.Charting.ChartElementType.DataPoint:
            ////        var dataPoint = e.HitTestResult.Series.Points[e.HitTestResult.PointIndex];
            ////        e.Text = string.Format("X:\t{0}\nY:\t{1}", dataPoint.XValue, dataPoint.YValues[0]);
            ////        break;
            ////}

            // Check selevted chart element and set tooltip text
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                string xLabel = e.HitTestResult.Series.AxisLabel;
                string xLabel1 = e.HitTestResult.Object.ToString();
                System.Windows.Forms.DataVisualization.Charting.DataPoint dp = e.HitTestResult.Series.Points[i];
                //------------------
                string edsg = "";
                string eoff = "";
                double graphval = dp.YValues[0];
                double gridval = 0.0;
                for (int j = 0; j< grdEmpCTC.Rows.Count; j++)
                {
                    gridval = Convert.ToDouble(grdEmpCTC.Rows[j].Cells["CTCInLakh"].Value);
                    if (graphval == gridval)
                    {
                        edsg = grdEmpCTC.Rows[j].Cells["ctcDesignation"].Value.ToString();
                        eoff = grdEmpCTC.Rows[j].Cells["ctcOffice"].Value.ToString();
                    }
                }
                //------------------
                ////////e.Text = string.Format("{0:F1}, {1:F1}", dp.XValue, dp.YValues[0]);
                e.Text = string.Format("{0:F1}, {1:F1}", edsg, eoff);
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

        private void chrtCTC_MouseMove(object sender, MouseEventArgs e)
        {
            ////////try
            ////////{
            ////////    int cursorX = Convert.ToInt32(chData.ChartAreas[0].AxisX.PixelPositionToValue(e.Location.X));

            ////////    tipInfo = "Bat 1: " + ch1Array[cursorX].ToString("0.00") + Environment.NewLine + "Bat 2: " + ch2Array[cursorX].ToString("0.00") + Environment.NewLine;

            ////////    tooltip.SetToolTip(chData, tipInfo);

            ////////}
            ////////catch { }


            ////Point? prevPosition = null;
            ////ToolTip tooltip = new ToolTip();
            ////var pos = e.Location;
            ////if (prevPosition.HasValue && pos == prevPosition.Value)
            ////    return;
            ////tooltip.RemoveAll();
            ////prevPosition = pos;
            ////var results = chrtCTC.HitTest(pos.X, pos.Y, false, ChartElementType.PlottingArea);
            ////foreach (var result in results)
            ////{
            ////    if (result.ChartElementType == ChartElementType.PlottingArea)
            ////    {
            ////        chrtCTC.Series[0].ToolTip = "X=#VALX, Y=#VALY";
            ////    }
            ////}
        }

        private void EmployeeCareerData_Enter(object sender, EventArgs e)
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


