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
using CSLERP.FileManager;
using CSLERP.PrintForms;

namespace CSLERP
{
    public partial class PAFClose : System.Windows.Forms.Form
    {
        ListView lvCopy = new ListView();
        Panel pnlForwarder = new Panel();
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        TextBox txtSearch = new TextBox();
        Panel pnlModel = new Panel();
        ListView exlv = new ListView();
        Form frmPopup = new Form();
        DataGridView grdRefSel = new DataGridView();
        string PAFString = ""; //For updateing internal order referenceNos(new)
        popiheader Selectedpopih = new popiheader();
        public PAFClose()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }
        private void BREntry_Load(object sender, EventArgs e)
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
        }
        private string getPAFDocID(string type)
        {
            string id = "";
            if (type == "Product")
                id = "PAFPRODUCTINWARD";
            else if (type == "Service")
                id = "PAFSERVICEINWARD";
            return id;
        }
        private string getPOPIDocID(string type)
        {
            string id = "";
            if (type == "Product")
                id = "POPRODUCTINWARD";
            else if (type == "Service")
                id = "POSERVICEINWARD";
            return id;
        }
        private void ListFilteredApprovePAFTracks()
        {
            try
            {
                grdList.Rows.Clear();
                grdList.FirstDisplayedScrollingColumnIndex = 0;
                POPIHeaderDB popidb = new POPIHeaderDB();
                List<popiheader> popiList = new List<popiheader>();
                string type = cmbProdType.SelectedItem.ToString();
                string pafDocID = getPAFDocID(type);
                string customerID = ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue;
                popiList = popidb.getFilteredPAFTypeProductInward(pafDocID, customerID);
                foreach (popiheader popi in popiList)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentID"].Value = popi.DocumentID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = popi.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = popi.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTrackingNo"].Value = popi.TrackingNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTrackingDate"].Value = popi.TrackingDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerID"].Value = popi.CustomerID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerName"].Value = popi.CustomerName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gReferenceNo"].Value = popi.ReferenceNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerPONO"].Value = popi.CustomerPONO;
                    grdList.Rows[grdList.RowCount - 1].Cells["gCustomerPODate"].Value = popi.CustomerPODate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gValue"].Value = popi.POValueINR;
                }
                if (getuserPrivilegeStatus() == 1)
                    grdList.Columns["Change"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in POPI Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;
        }
        private void initVariables()
        {
            CustomerDB.fillLedgerTypeComboNew(cmbCustomer, "Customer");
            pnlUI.Controls.Add(pnlList);
            setButtonVisibility("init");
        }
        public void clearData()
        {
            try
            {
                grdList.Rows.Clear();
                pnllv.Visible = true;
                cmbProdType.SelectedIndex = -1;
                cmbCustomer.SelectedIndex = -1;
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
        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string colname = grdList.Columns[e.ColumnIndex].Name;
                if (colname == "Select")
                {
                    foreach(DataGridViewRow row in grdList.Rows)
                    {
                        if(Convert.ToBoolean(row.Cells["Select"].Value) == true && row.Index != e.RowIndex)
                        {
                            row.Cells["Select"].Value = false;
                        }
                    }
                }
                if (colname == "Change")
                {
                    try
                    {
                        if (Convert.ToBoolean(grdList.Rows[e.RowIndex].Cells["Select"].Value) != true)
                        {
                            MessageBox.Show("Current Row Not Selected.\nPlease check before Closing PO");
                            return;
                        }
                        DialogResult dialog = MessageBox.Show("Are you sure to CLose PAF document ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.No)
                        {
                            return;
                        }
                        var checkedRows = from DataGridViewRow r in grdList.Rows
                                          where Convert.ToBoolean(r.Cells["Select"].Value) == true
                                          select r;
                        int selectedRowCount = checkedRows.Count();
                        if (selectedRowCount != 1)
                        {
                            MessageBox.Show("Select one PO");
                            return;
                        }
                        Selectedpopih = new popiheader();
                        Selectedpopih.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                        Selectedpopih.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                        Selectedpopih.TemporaryDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value);
                        string type = cmbProdType.SelectedItem.ToString();
                        string POPIDocID = getPOPIDocID(type);
                        POPIHeaderDB popidb = new POPIHeaderDB();
                        if (popidb.updatePAFPOInwardCLosingNew(Selectedpopih, POPIDocID))
                        {
                            MessageBox.Show("PAF ProductInward Document Closed Sucessfully");
                            ListFilteredApprovePAFTracks();
                            grdList.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("Failed to close PAF Product Inward Document");
                            ListFilteredApprovePAFTracks();
                            grdList.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
      
        private void pnlList_Paint(object sender, PaintEventArgs e)
        {
        }
        private void setButtonVisibility(string btnName)
        {
            try
            {
                grdList.Visible = false;
                btnExit.Visible = true;
                pnlBottomButtons.Visible = true;
                pnlMenu.Visible = true;
                if (btnName == "init")
                {
                    btnExit.Visible = true;
                    pnlMenu.Visible = true;
                }
                else if (btnName == "btnCheck")
                {
                    grdList.Visible = true;
                    pnlMenu.Visible = true;
                }
            }
            catch (Exception ex)
            {
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
        private void btnViewHistory_Click_1(object sender, EventArgs e)
        {
            try
            {
                ListFilteredApprovePAFTracks();
                grdList.Visible = true;
            }
            catch (Exception ex)
            {
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                grdList.Rows.Clear();
                cmbCustomer.SelectedIndex = -1;
                cmbProdType.SelectedIndex = -1;
                PAFString = "";
                Selectedpopih = new popiheader();
                grdList.Visible = false;
                pnllv.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void cmbDocStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                grdList.Rows.Clear();
                grdList.Visible = false;
                PAFString = "";
                Selectedpopih = new popiheader();
            }
            catch (Exception ex)
            {
            }
        }
        private void cmbPurpose_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Rows.Clear();
            grdList.Visible = false;
            PAFString = "";
            Selectedpopih = new popiheader();
        }

        private void PAFClose_Enter(object sender, EventArgs e)
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





