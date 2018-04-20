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
using CSLERP.PrintForms;

namespace CSLERP
{
    public partial class PrintProductTestReport : System.Windows.Forms.Form
    {

        public static string[,] documentStatusValues;
        Panel panel = new Panel();
        int planNo = 0;
        string selectType = "";
        DateTime planDate = DateTime.Parse("01-01-1900");
        ListView lvProdList = new ListView();
        Panel pnlProdList = new Panel();
        public PrintProductTestReport()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }

        private void PrintProductTestReport_Load(object sender, EventArgs e)
        {
            //////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;

            String a = this.Size.ToString();
            grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdList.EnableHeadersVisualStyles = false;
            pnlPDFViewer.Visible = false;
            ListProductTestDesc();
        }
        private void ListProductTestDesc()
        {
            try
            {
                grdList.Rows.Clear();
                ProductionPlanHeaderDB  pphDB = new ProductionPlanHeaderDB();
                List<productionplanheader> pphList = pphDB.getFilteredProductionPlanHeader("",6,"");
                foreach (productionplanheader pph in pphList)
                {

                    grdList.Rows.Add(pph.ProductionPlanNo, pph.ProductionPlanDate,
                         pph.StockItemID, pph.StockItemName, pph.ModelNo, pph.ModelName);
                }
            }
            catch (Exception ex)
            {
               
            }
            pnlProductionList.Visible = true;
        }
        private void closeAllPanels()
        {
            try
            {
                pnlProductionList.Visible = false;
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
        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                planNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["ProductionPlanNo"].Value.ToString());
                planDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["ProductionPlanDate"].Value);
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Print"))
                {
                    foreach (Control c in panel.Controls)
                    {
                        panel.Controls.Remove(c);
                    }
                    removeControlsFromChoicePanel();
                    panel.BorderStyle = BorderStyle.FixedSingle;
                    panel.Location = new Point(224, 110);
                    panel.Size = new Size(292, 145);
                    CheckBox chk1 = new CheckBox();
                    chk1.Text = "Total";
                    chk1.Location = new Point(93, 23);
                    CheckBox chk2 = new CheckBox();
                    chk2.Text = "Selective";
                    chk2.Location = new Point(93, 47);
                    panel.Controls.Add(chk1);
                    panel.Controls.Add(chk2);
                    Button lvSelectOK = new Button();
                    lvSelectOK.Text = "OK";
                    //lvSelectOK.Size = new Size(150, 20);
                    lvSelectOK.Location = new Point(57, 107);
                    lvSelectOK.Click += new System.EventHandler(this.lvSelectOK_Click);
                    panel.Controls.Add(lvSelectOK);

                    Button lvSelectCancel = new Button();
                    lvSelectCancel.Text = "Cancel";
                    //lvSelectCancel.Size = new Size(150, 20);
                    lvSelectCancel.Location = new Point(178, 107);
                    lvSelectCancel.Click += new System.EventHandler(this.lvSelctCancel_Click);
                    panel.Controls.Add(lvSelectCancel);

                    panel.Visible = true;

                    pnlProductionList.Controls.Add(panel);
                    pnlProductionList.BringToFront();
                    panel.BringToFront();
                    panel.Focus();
                    pnlProductionList.Visible = true;
                }
                else if (columnName.Equals("View"))
                {
                    removePDFFileGridView();
                    removePDFControls();
                    removeControlsFromChoicePanel();
                    removeControlsFromForwarderPanel();
                    pnlPDFViewer.Visible = true;
                    pnlProductionList.Visible = false;
                    grdList.Visible = false;
                }
            }
            catch (Exception)
            {

            }
        }
        private void lvSelectOK_Click(object sender, EventArgs e)
        {
            try
            {
                string result = "";
                int count = 0;
                foreach (CheckBox cb in panel.Controls.OfType<CheckBox>())
                {
                    if (cb.Checked == true)
                    {
                        result = cb.Text;
                        count++;
                    }
                }
                if (count != 1)
                {
                    MessageBox.Show("select one item");
                    return;
                }
                if (result.Equals("Selective"))
                {
                    selectType = "Selective";
                    ShowProductTestList(planNo, planDate, 1);
                }
                else
                {
                    selectType = "Total";
                    ShowProductTestList(planNo, planDate, 2);
                }
                panel.Visible = false;
                pnlProdList.Visible = true;
            }
            catch (Exception ex)
            {
            }
        }

        private void lvSelctCancel_Click(object sender, EventArgs e)
        {
            try
            {
                panel.Visible = false;
            }
            catch (Exception)
            {
            }
        }
        private void ShowProductTestList(int planNo, DateTime planDate, int opt)
        {
            removeControlsFromForwarderPanel();
            lvProdList = new ListView();
            lvProdList.Enabled = true;
            lvProdList.Clear();
            lvProdList = ProductTestReportHeaderDB.getProdTestReportListView(planNo, planDate);
            if (lvProdList.Items.Count == 0)
            {
                MessageBox.Show("Report not prepared");
            }

            pnlProdList.BorderStyle = BorderStyle.FixedSingle;
            pnlProdList.Size = new Size(559, 252);
        
            lvProdList.Size = new Size(529, 188);
            lvProdList.Location = new Point(15, 21);
            if (opt == 2)
            {
                foreach(ListViewItem item in lvProdList.Items)
                {
                    item.Checked = true;
                }
                lvProdList.Enabled = false;
            }
       
            pnlProdList.Controls.Remove(lvProdList);
            pnlProdList.Controls.Add(lvProdList);

            Button lvSelectOK = new Button();
            lvSelectOK.Text = "OK";
            lvSelectOK.Location = new Point(108, 216);
            lvSelectOK.Click += new System.EventHandler(this.lvOK_Click1);
            pnlProdList.Controls.Add(lvSelectOK);

            Button lvSelectCancel = new Button();
            lvSelectCancel.Text = "Cancel";
            lvSelectCancel.Location = new Point(276, 215);
            lvSelectCancel.Click += new System.EventHandler(this.lvCancel_Click1);
            pnlProdList.Controls.Add(lvSelectCancel);

            pnlProductionList.Visible = true;
            pnlProductionList.Controls.Add(pnlProdList);
            pnlProductionList.BringToFront();
            pnlProdList.BringToFront();
            pnlProdList.Focus();
        }
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                if(lvProdList.Items.Count == 0)
                {
                    MessageBox.Show("No item in list view");
                    return;
                }
                string prodInfo ="";
                Dictionary<Int32, DateTime> ReprotDict = new Dictionary<int, DateTime>();
                foreach(ListViewItem item in lvProdList.Items)
                {
                    if (item.Checked == true)
                    {
                        ReprotDict.Add(Convert.ToInt32(item.SubItems[1].Text), Convert.ToDateTime(item.SubItems[2].Text));
                        prodInfo = item.SubItems[4].Text + "-" + item.SubItems[5].Text+";"+ item.SubItems[6].Text + "-" + item.SubItems[7].Text;
                    }
                }
                
                PrintProductTestReportList print = new PrintProductTestReportList();
                if (!print.PrintReport(ReprotDict, planNo + "-" + planDate.ToString("yyyyMMddhhmmss"), selectType, prodInfo))
                {
                    MessageBox.Show("Failed to print");
                    return;
                }
                pnlProdList.Controls.Remove(lvProdList);
                pnlProdList.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void lvCancel_Click1(object sender, EventArgs e)
        {
            try
            {
                pnlProdList.Controls.Remove(lvProdList);
                pnlProdList.Visible = false;
            }
            catch (Exception)
            {
            }
        }
        private void removeControlsFromForwarderPanel()
        {
            try
            {
                //foreach (Control p in pnlProdList.Controls)
                //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
                //    {
                //        p.Dispose();
                //    }
                pnlProdList.Controls.Clear();
                Control nc = pnlProdList.Parent;
                nc.Controls.Remove(pnlProdList);
            }
            catch (Exception ex)
            {
            }
        }
        private void removeControlsFromChoicePanel()
        {
            try
            {
                //foreach (Control p in panel.Controls)
                //    if (p.GetType() == typeof(CheckBox) || p.GetType() == typeof(Button))
                //    {
                //        p.Dispose();
                //    }
                panel.Controls.Clear();
                Control nc = panel.Parent;
                nc.Controls.Remove(panel);
            }
            catch (Exception ex)
            {
            }
        }
        private void removePDFFileGridView()
        {
            try
            {
                foreach (Control p in pnlPDFViewer.Controls)
                    if (p.GetType() == typeof(DataGridView))
                    {
                        p.Dispose();
                    }
            }
            catch (Exception ex)
            {
            }
        }
        private void removePDFControls()
        {
            try
            {
                foreach (Control p in pnlPDFViewer.Controls)
                    if (p.GetType() == typeof(AxAcroPDFLib.AxAcroPDF))
                    {
                        AxAcroPDFLib.AxAcroPDF c = (AxAcroPDFLib.AxAcroPDF)p;
                        c.Visible = false;
                        pnlPDFViewer.Controls.Remove(c);
                        c.Dispose();
                    }
            }
            catch (Exception ex)
            {
            }
        }
        private void btnListDocument_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(docID, planNo + "-" + planDate.ToString("yyyyMMddhhmmss"));
                pnlPDFViewer.Controls.Add(dgvDocumentList);
                dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);
            }
            catch (Exception ex)
            {
            }
        }

        private void btnCloseDocument_Click(object sender, EventArgs e)
        {
            removePDFControls();
            showPDFFileGrid();
        }
        private void showPDFFileGrid()
        {
            try
            {
                foreach (Control p in pnlPDFViewer.Controls)
                    if (p.GetType() == typeof(DataGridView))
                    {
                        p.Visible = true;
                    }
            }
            catch (Exception ex)
            {
            }
        }
        string docID = "PRODUCTTESTREPORT";
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
                    string subDir = planNo + "-" + planDate.ToString("yyyyMMddhhmmss");
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
                pnlPDFViewer.Controls.Add(pdf);

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                removePDFFileGridView();
                removePDFControls();
                removeControlsFromChoicePanel();
                removeControlsFromForwarderPanel();
                pnlPDFViewer.Visible = false;
                pnlProductionList.Visible = true;
                grdList.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("test");
            }
        }

        private void PrintProductTestReport_Enter(object sender, EventArgs e)
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

