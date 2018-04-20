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
    public partial class PAFConversion : System.Windows.Forms.Form
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
        public PAFConversion()
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
            grdPOList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdPOList.EnableHeadersVisualStyles = false;
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
            //grdList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grdList.Columns["Change"].ReadOnly = true;
            grdPOList.Visible = false;
            btnClose.Visible = false;
            btnCancel.Visible = false;
            setButtonVisibility("init");
            dtCustomerPODate.Format = DateTimePickerFormat.Custom;
            dtCustomerPODate.CustomFormat = "dd-MM-yyyy";
        }
        public void clearData()
        {
            try
            {
                grdList.Rows.Clear();
                pnllv.Visible = true;
                cmbProdType.SelectedIndex = -1;
                cmbCustomer.SelectedIndex = -1;
                grdPOList.Visible = false;
                btnClose.Visible = false;
                btnCancel.Visible = false;
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
                if (colname == "Change")
                {
                    PAFString = "";
                    string type = cmbProdType.SelectedItem.ToString();
                    string pafDocID = getPAFDocID(type);

                    PAFString = pafDocID + ";" + grdList.Rows[e.RowIndex].Cells["gTrackingNo"].Value.ToString() + "("
                                   + Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["gTrackingDate"].Value).ToString("yyyy-MM-dd") + ")" + Main.delimiter1;

                    Selectedpopih = new popiheader();
                    Selectedpopih.DocumentID = grdList.Rows[e.RowIndex].Cells["gDocumentID"].Value.ToString();
                    Selectedpopih.TrackingNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTrackingNo"].Value.ToString());
                    Selectedpopih.TrackingDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["gTrackingDate"].Value);
                    Selectedpopih.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    Selectedpopih.TemporaryDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value);

                    AddRowsInPOListGridView(PAFString);
                    //showPOPIGridview();
                    pnlUserInput.Visible = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void AddRowsInPOListGridView(string psfrefStr)
        {
            try
            {
                grdPOList.Rows.Clear();
                string type = cmbProdType.SelectedItem.ToString();
                //string POPIDocID = getPOPIDocID(type);
                //string customerID = ((Structures.ComboBoxItem)cmbCustomer.SelectedItem).HiddenValue;
                //POPIHeaderDB popihdb = new POPIHeaderDB();
                //List<popiheader> POPIHeaders = new List<popiheader>();
                //POPIHeaders = popihdb.getFilteredPAFTypeProductInward(POPIDocID, customerID);
                if (type == "Product")
                {
                    List<ioheader> IOList = InternalOrderDB.IODetailListWRTRefPONos(PAFString);
                    foreach (ioheader ioh in IOList)
                    {
                        int c = grdPOList.Rows.Count;
                        grdPOList.Rows.Add();
                        grdPOList.Rows[grdPOList.RowCount - 1].Cells["pDocumentID"].Value = ioh.DocumentID;
                        grdPOList.Rows[grdPOList.RowCount - 1].Cells["pTemporaryNo"].Value = ioh.TemporaryNo;
                        grdPOList.Rows[grdPOList.RowCount - 1].Cells["pTemporaryDate"].Value = ioh.TemporaryDate;
                        grdPOList.Rows[grdPOList.RowCount - 1].Cells["pCustomerID"].Value = ioh.CustomerID;
                        grdPOList.Rows[grdPOList.RowCount - 1].Cells["pCustomerName"].Value = ioh.CustomerName;
                        grdPOList.Rows[grdPOList.RowCount - 1].Cells["pReferenceNO"].Value = ioh.ReferenceTrackingNos.Replace(Environment.NewLine, string.Empty);
                        grdPOList.Rows[grdPOList.RowCount - 1].Cells["RefPoOriginal"].Value = ioh.ReferenceTrackingNos;
                    }
                }
                else
                {
                    List<indentserviceheader> INdList = IndentServiceDB.IndentDetailListWRTRefPONos(PAFString);
                    foreach (indentserviceheader ish in INdList)
                    {
                        int c = grdPOList.Rows.Count;
                        grdPOList.Rows.Add();
                        grdPOList.Rows[grdPOList.RowCount - 1].Cells["pDocumentID"].Value = ish.DocumentID;
                        grdPOList.Rows[grdPOList.RowCount - 1].Cells["pTemporaryNo"].Value = ish.TemporaryNo;
                        grdPOList.Rows[grdPOList.RowCount - 1].Cells["pTemporaryDate"].Value = ish.TemporaryDate;
                        grdPOList.Rows[grdPOList.RowCount - 1].Cells["pCustomerID"].Value = ish.CustomerID;
                        grdPOList.Rows[grdPOList.RowCount - 1].Cells["pCustomerName"].Value = ish.CustomerName;
                        grdPOList.Rows[grdPOList.RowCount - 1].Cells["pReferenceNO"].Value = ish.ReferenceInternalOrder.Replace("\n", "");  //For Refenernce PO Nos
                        grdPOList.Rows[grdPOList.RowCount - 1].Cells["RefPoOriginal"].Value = ish.ReferenceInternalOrder;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in POPI Listing");
            }
            grdPOList.Visible = true;
            btnClose.Visible = true;
            btnCancel.Visible = true;
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
                if (cmbProdType.SelectedIndex == -1 || cmbCustomer.SelectedIndex == -1)
                {
                    MessageBox.Show("select product type or customer");
                    return;
                }
                ListFilteredApprovePAFTracks();
                grdList.Visible = true;
                grdPOList.Visible = false;
                btnClose.Visible = false;
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
                grdPOList.Visible = false;
                btnClose.Visible = false;
                btnCancel.Visible = false;
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
                grdPOList.Visible = false;
                btnClose.Visible = false;
                btnCancel.Visible = false;
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
            grdPOList.Visible = false;
            btnClose.Visible = false;
            btnCancel.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCustomerPoNo.Text.Trim().Length == 0 || txtReferenceNo.Text.Trim().Length == 0)
                {
                    MessageBox.Show("New PO / reference not filled");
                    return;
                }

                DialogResult dialog = MessageBox.Show("Are you sure to Convert PAF document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.No)
                {
                    return;
                }

                string type = cmbProdType.SelectedItem.ToString();
                string docid = getPOPIDocID(type);
                if (type == "Product")
                {
                    List<ioheader> iohList = new List<ioheader>();
                    ioheader ioh = new ioheader();
                    foreach (DataGridViewRow row in grdPOList.Rows)
                    {
                        ioh = new ioheader();
                        ioh.DocumentID = row.Cells["pDocumentID"].Value.ToString();
                        ioh.TemporaryNo = Convert.ToInt32(row.Cells["pTemporaryNo"].Value);
                        ioh.TemporaryDate = Convert.ToDateTime(row.Cells["pTemporaryDate"].Value);
                        ioh.ReferenceTrackingNos = row.Cells["RefPoOriginal"].Value.ToString();
                        string mainrefStr = getUpdatedRefString(ioh.ReferenceTrackingNos, type);
                        ioh.ReferenceTrackingNos = mainrefStr;
                        iohList.Add(ioh);
                    }
                    if (POPIHeaderDB.CLosingPAFPOProductInward(Selectedpopih, iohList, docid,
                        txtCustomerPoNo.Text.Trim(),
                        dtCustomerPODate.Value,txtReferenceNo.Text.Trim()))
                    {
                        MessageBox.Show("PAF Product Inward Document Converted Sucessfully");
                        ListFilteredApprovePAFTracks();
                        grdList.Visible = true;
                        grdPOList.Visible = false;
                        btnClose.Visible = false;
                        btnCancel.Visible = false;
                        pnlUserInput.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Failed to Convert PAF Product Inward Document");
                        ListFilteredApprovePAFTracks();
                        grdList.Visible = true;
                        grdPOList.Visible = false;
                        btnClose.Visible = false;
                        btnCancel.Visible = false;
                    }
                }
                else
                {
                    List<indentserviceheader> inhList = new List<indentserviceheader>();
                    indentserviceheader inh = new indentserviceheader();
                    foreach (DataGridViewRow row in grdPOList.Rows)
                    {
                        inh = new indentserviceheader();
                        inh.DocumentID = row.Cells["pDocumentID"].Value.ToString();
                        inh.TemporaryNo = Convert.ToInt32(row.Cells["pTemporaryNo"].Value);
                        inh.TemporaryDate = Convert.ToDateTime(row.Cells["pTemporaryDate"].Value);
                        inh.ReferenceInternalOrder = row.Cells["RefPoOriginal"].Value.ToString();
                        string mainrefStr = getUpdatedRefString(inh.ReferenceInternalOrder, type);
                        inh.ReferenceInternalOrder = mainrefStr;
                        inhList.Add(inh);
                    }
                    if (POPIHeaderDB.CLosingPAFPOServiceInward(Selectedpopih, inhList, docid,
                        txtCustomerPoNo.Text.Trim(),
                        dtCustomerPODate.Value, txtReferenceNo.Text.Trim()))
                    {
                        MessageBox.Show("PAF Service Inward Document Converted Sucessfully");
                        ListFilteredApprovePAFTracks();
                        grdList.Visible = true;
                        grdPOList.Visible = false;
                        btnClose.Visible = false;
                        btnCancel.Visible = false;
                    }
                    else
                    {
                        MessageBox.Show("Failed to Convert PAF Service Inward Document");
                        ListFilteredApprovePAFTracks();
                        grdList.Visible = true;
                        grdPOList.Visible = false;
                        btnClose.Visible = false;
                        btnCancel.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private string getUpdatedRefString(string refTrackNos, string type)
        {
            string updatedstr = "";

            //get PO DOCID
            string POPIDocID = getPOPIDocID(type);

            //get PAF DOCID
            string PAFDocID = getPAFDocID(type);

            //spliting reference string with delimiter1 (with triming each element)
            string[] refArr = refTrackNos.Split(Main.delimiter1).Select(str => str.Trim()).ToArray();

            //finding index of selected pafstring(with replacing delimiter1 to blank) in array
            int index = Array.IndexOf(refArr, PAFString.Replace(Main.delimiter1.ToString(), ""));

            string findStr = refArr[index]; //POPRODUCTINWARD;13(2017-11-30)
            string finalSubStr = findStr.Replace(PAFDocID, POPIDocID);
            refArr[index] = finalSubStr;
            updatedstr = string.Join(Main.delimiter1 + Environment.NewLine, refArr);
            return updatedstr;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            grdPOList.Rows.Clear();
            grdPOList.Visible = false;
            btnClose.Visible = false;
            btnCancel.Visible = false;
            pnlUserInput.Visible = false;
        }

        private void grdPOList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string colname = grdPOList.Columns[e.ColumnIndex].Name;
                if (colname == "pSelect")
                {
                    foreach (DataGridViewRow row in grdPOList.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["pSelect"].Value) == true && row.Index != e.RowIndex)
                        {
                            row.Cells["pSelect"].Value = false;
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        private void PAFConversion_Enter(object sender, EventArgs e)
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





