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

namespace CSLERP
{
    public partial class TapalSummary : System.Windows.Forms.Form
    {
        string fileDir = "";
        string docID = "TAPALSUMMARY";
        List<user> list = new List<user>();
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        TextBox txtSearch = new TextBox();
        tapal prevag;
        Panel pnllv = new Panel();
        ListView lv = new ListView();
        ListView lvCopy = new ListView();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        static int lvl = 0;
        int no;
        int refr = 0;
        DateTime dt = DateTime.Now;
        public TapalSummary()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }

        private void TapalSummary_Load(object sender, EventArgs e)
        {
            //////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;

            String a = this.Size.ToString();
            grdMainList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdMainList.EnableHeadersVisualStyles = false;
            ShowControl();
            listtapal();
            applyPrivilege();
        }

        private void ShowControl()
        {
            pnlList.Visible = true;
            grdMainList.Visible = true;
            grdDetailList.Visible = false;
            btnCancel.Visible = false;
        }

        private void listtapal()
        {
            try
            {
                grdMainList.Rows.Clear();
                clearData();
                TapalPickDB TapDB = new TapalPickDB();
                List<tapalDistribution> list = TapDB.getTapalListDateWiseSummary();
                int i = 1;
                foreach (tapalDistribution ts in list)
                {
                    grdMainList.Rows.Add();
                    grdMainList.Rows[grdMainList.RowCount - 1].Cells["SiNo"].Value = i;
                    grdMainList.Rows[grdMainList.RowCount - 1].Cells["dtDate"].Value = ts.Date;
                    grdMainList.Rows[grdMainList.RowCount - 1].Cells["DocAdded"].Value = ts.RowID;
                    grdMainList.Rows[grdMainList.RowCount - 1].Cells["DocDistributed"].Value = ts.TapalReference;
                    i++;
                }
                grdMainList.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Account Group  Listing");
            }
            try
            {
                enableBottomButtons();
                pnlList.Visible = true;
            }
            catch (Exception ex)
            {
            }
        }

        private void initVariables()
        {

            docID = Main.currentDocument;

            pnlUI.Controls.Add(pnlList);
            enableBottomButtons();
            TapalPickDB.fillInwardDocCombo(cmbInwardDocType, "TapalType");
            grdMainList.Visible = true;
            grdMainList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            pnlUpdate.Visible = false;
        }
        private void closeAllPanels()
        {
            try
            {
                pnlUpdate.Visible = false;
                grdDetailList.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        public void clearData()
        {
            try
            {
                grdDetailList.Rows.Clear();
                dt = DateTime.Now;
                prevag = new tapal();
            }
            catch (Exception ex)
            {

            }
        }

        private void applyPrivilege()
        {
            try
            {
                if (Main.itemPriv[2])
                {
                    grdDetailList.Columns["Edit"].Visible = true;
                }
                else
                {
                    grdDetailList.Columns["Edit"].Visible = false;
                }
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
        private void enableBottomButtons()
        {
            ///btnNew.Visible = false;
            btnExit.Visible = true;
            pnlBottomButtons.Visible = true;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            clearData();
            grdMainList.Visible = true;
            grdDetailList.Visible = false;
            btnCancel.Visible = false;
        }
        private void grdMainList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdMainList.Columns[e.ColumnIndex].Name;
                clearData();
                try
                {
                    if (columnName.Equals("gView"))
                    {
                        dt = Convert.ToDateTime(grdMainList.Rows[e.RowIndex].Cells["dtDate"].Value.ToString());
                        if (columnName.Equals("gView"))
                        {
                            grdDetailList.Visible = true;
                            btnCancel.Visible = true;
                            detaillist();
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {

            }
        }


        public void detaillist()
        {
            prevag = new tapal();
            TapalPickDB TapDB = new TapalPickDB();
            List<tapalDistribution> list = TapDB.getTapalListSummaryForADate(dt);
            int i = 1;
            foreach (tapalDistribution ts in list)
            {
                grdDetailList.Rows.Add();
                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["LineNo"].Value = i;
                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["RowID"].Value = ts.RowID;
                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["Date"].Value = ts.Date;
                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["FileName"].Value = ts.FileName;
                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["InwardDocumentType"].Value = ts.InwardDocumentType;
                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["ReceivedFrom"].Value = ts.ReceivedFrom;
                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["Description"].Value = ts.Description;
                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["Status"].Value = showStatusString(ts.ActionReferenceID, ts.RowID, ts.Status, ts.ActionStatus);
                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["CreateUser"].Value = ts.CreateUser;
                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["Creator"].Value = ts.Creator;
                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["DistributeUser"].Value = ts.DistributeUser;
                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["Distributor"].Value = ts.Distributor;
                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["ReceiveUser"].Value = ts.UserID;
                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["Receiver"].Value = ts.UserName;
                grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["CreateTime"].Value = ts.CreateTime;
                if (grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["Status"].Value.ToString() == "Delivered")
                {
                    //orange
                    grdDetailList.Rows[grdDetailList.RowCount - 1].DefaultCellStyle.BackColor = Color.Orange;
                }
                else if (grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["Status"].Value.ToString() == "Deleted Before Viewing")
                {
                    grdDetailList.Rows[grdDetailList.RowCount - 1].DefaultCellStyle.BackColor = Color.Red;
                }
                else if (grdDetailList.Rows[grdDetailList.RowCount - 1].Cells["Status"].Value.ToString().Contains("Moved to"))
                {
                    grdDetailList.Rows[grdDetailList.RowCount - 1].DefaultCellStyle.BackColor = Color.Yellow;
                }
                i++;
            }
            grdDetailList.ClearSelection();
        }

        private string showStatusString(int rowid, int referenceNo, int stat, int ActionStatus)
        {
            string str = "";

            try
            {
                if (stat == 0)
                    str = "Delivered";
                else if (ActionStatus == 0)
                    str = "Delivered";
                else if (ActionStatus == 1)
                {
                    str = TapalPickDB.getMoveReceipentName(rowid, referenceNo);
                    str = "Moved to " + str;
                }
                else if (ActionStatus == 2) //deleted
                    str = "Viewed";
                else if (ActionStatus == 3)
                    str = "Viewed";
                else if (ActionStatus == 4)
                    str = "Deleted Before Viewing";
            }
            catch (Exception ex)
            {
            }

            return str;
        }
        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
        
        private void grdDetailList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ////////grdDetailList.Enabled = false;
            if (e.RowIndex < 0)
                return;
            string columnName = grdDetailList.Columns[e.ColumnIndex].Name;
            if (columnName.Equals("View"))
            {
                grdDetailList.Enabled = false;
                ////grdDetailList.Columns["View"].ReadOnly = true;
                //btnCancel.Enabled = false;
                int rowId = Convert.ToInt32(grdDetailList.CurrentRow.Cells["RowID"].Value);
                string FileName = grdDetailList.CurrentRow.Cells["FileName"].Value.ToString();
                string fileName = TapalPickDB.getFileFromDB(rowId, FileName);
                fileDir = fileName;
                System.Diagnostics.Process process = System.Diagnostics.Process.Start(fileName);
                grdDetailList.Enabled = true;
                process.EnableRaisingEvents = true;
                process.Exited += new EventHandler(myProcess_Exited);
                process.WaitForExit();
            }
            if (columnName.Equals("Edit"))
            {
                refr = Convert.ToInt32(grdDetailList.Rows[e.RowIndex].Cells["RowID"].Value);
                try
                {
                    cmbInwardDocType.SelectedIndex = cmbInwardDocType.FindString(grdDetailList.Rows[e.RowIndex].Cells["InwardDocumentType"].Value.ToString());
                    txtDescription.Text = grdDetailList.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                    txtReceivedFrom.Text = grdDetailList.Rows[e.RowIndex].Cells["ReceivedFrom"].Value.ToString();
                    pnlUpdate.Visible = true;
                    grdDetailList.Enabled = false;
                    grdMainList.Enabled = false;
                    btnCancel.Enabled = false;
                }
                catch(Exception ex)
                {

                }
              
            }
        }
        private void myProcess_Exited(object sender, System.EventArgs e)
        {
            if (File.Exists(fileDir))
            {
                try
                {
                    File.Delete(fileDir);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            tapal tap = new tapal();
            TapalPickDB tapdb = new TapalPickDB();
            tap.ReceivedFrom = txtReceivedFrom.Text.Trim();
            tap.Description = txtDescription.Text.Trim();
            tap.InwardDocumentType = cmbInwardDocType.SelectedItem.ToString();
            if (cmbInwardDocType.SelectedIndex == -1 || string.IsNullOrWhiteSpace(tap.ReceivedFrom))
            {
                MessageBox.Show("Please check the Data Entered!!!");
                return;
            }
            if (tapdb.updateTapalDetails(tap, refr))
            {
                MessageBox.Show("Tapal Summary updated");
                grdDetailList.Rows.Clear();
                detaillist();
                pnlUpdate.Visible = false;
                grdDetailList.Enabled = true;
                grdMainList.Enabled = true;
                btnCancel.Enabled = true;
            }
            else
            {
                MessageBox.Show("Failed to Update Tapal Summary");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            refr = 0;
            pnlUpdate.Visible = false;
            grdDetailList.Enabled = true;
            grdMainList.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void TapalSummary_Enter(object sender, EventArgs e)
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


