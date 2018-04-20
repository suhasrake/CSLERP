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
using System.Collections;

namespace CSLERP
{
    public partial class RawMaterialUsage : System.Windows.Forms.Form
    {
        string userString = "";
        string userCommentStatusString = "";
        string commentStatus = "";
        string Temp;
        Form dtpForm = new Form();
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        DocCommenterDB docCmtrDB = new DocCommenterDB();
        productionplanheader prevprodution;

        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        System.Data.DataTable dtCmtStatus = new DataTable();
        Panel pnldgv = new Panel(); // panel for gridview
        Panel pnlCmtr = new Panel();
        Panel pnlForwarder = new Panel();
        Form frmPopup = new Form();
        DataGridView dgvpt = new DataGridView(); // grid view for Payment Terms
        DataGridView dgvComments = new DataGridView();
        ListView lvCmtr = new ListView(); // list view for choice / selection list
        ListView lvApprover = new ListView();
        Panel pnllv = new Panel(); // panel for listview
        ListView lv = new ListView(); // list view for choice / selection list
        public RawMaterialUsage()
        {
            try
            {

                InitializeComponent();

            }
            catch (Exception ex)
            {

            }
        }

        private void RawMaterialUsage_Load(object sender, EventArgs e)
        {
            //////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;

            String a = this.Size.ToString();
        }

        //called only in the beginning
        private void initVariables()
        {

            ////docID = Main.currentDocument;
            dtProductionPlanDate.Format = DateTimePickerFormat.Custom;
            dtProductionPlanDate.CustomFormat = "dd-MM-yyyy";
            dtProductionPlanDate.Enabled = false;

            pnlUI.Controls.Add(pnlAddEdit);
            closeAllPanels();
            txtComments.Text = "";

            userString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
            userCommentStatusString = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + "0" + Main.delimiter2;
            setButtonVisibility("init");
            ListFilteredProductionPlan();
        }

        private void ListFilteredProductionPlan()
        {
            try
            {
                grdList.Rows.Clear();
                ProductionPlanHeaderDB pphdb = new ProductionPlanHeaderDB();
                List<productionplanheader> ProductionPlanHeaderList = pphdb.getProductionPlanForRawMaterialMainGrid();
                foreach (productionplanheader pph in ProductionPlanHeaderList)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryNo"].Value = pph.TemporaryNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gTemporaryDate"].Value = pph.TemporaryDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProductionPlanNo"].Value = pph.ProductionPlanNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gProductionPlanDate"].Value = pph.ProductionPlanDate;
                    grdList.Rows[grdList.RowCount - 1].Cells["gInternalOrderNo"].Value = pph.InternalOrderNos;
                    grdList.Rows[grdList.RowCount - 1].Cells["gInternalOrderDate"].Value = pph.InternalOrderDates;
                    grdList.Rows[grdList.RowCount - 1].Cells["gReference"].Value = pph.Reference;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStockItemID"].Value = pph.StockItemID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gStockItemName"].Value = pph.StockItemName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gModelNo"].Value = pph.ModelNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["gModelName"].Value = pph.ModelName;
                    grdList.Rows[grdList.RowCount - 1].Cells["gQuantity"].Value = pph.Quantity;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Production Listing");
            }
            setButtonVisibility("init");
            pnlList.Visible = true;
            grdList.Visible = true;
            grdList.BringToFront();
            grdList.Focus();
        }

        private void closeAllPanels()
        {
            try
            {
                pnlList.Visible = false;
                pnlAddEdit.Visible = false;
            }
            catch (Exception)
            {
            }
        }

        //called when new,cancel buttons are clicked.
        //called at the end of event processing for forward, approve,reverse and save
        public void clearData()
        {
            try
            {

                //grdProductionPlanDetail.Rows.Clear();
                dgvComments.Rows.Clear();
                dgvpt.Rows.Clear();
                grdSIDetail.Rows.Clear();

                prevprodution = new productionplanheader();
                clearTabPageControls();
                pnlCmtr.Visible = false;
                pnlForwarder.Visible = false;
                pnllv.Visible = false;
                txtProductionPlanNo.Text = "";
                dtProductionPlanDate.Value = DateTime.Parse("01-01-1900");
                txtProductName.Text = "";
                txtModel.Text = "";
                removeControlsFrompnllvPanel();
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
        private Boolean validateMaterialDetail()
        {
            Boolean status = true;
            try
            {
                for (int i = 0; i < grdSIDetail.Rows.Count; i++)
                {
                    if ((Convert.ToDouble(grdSIDetail.Rows[i].Cells["UsedQuantity"].Value)) +
                        (Convert.ToDouble(grdSIDetail.Rows[i].Cells["ReturnedQuantity"].Value)) +
                        (Convert.ToDouble(grdSIDetail.Rows[i].Cells["DamagedQuantity"].Value)) !=
                        (Convert.ToDouble(grdSIDetail.Rows[i].Cells["IssueQuantity"].Value)))
                    {
                        MessageBox.Show("IssueQuantity Should be equal to toatal(used/return/damaged) quantity.\nCheck RowNo:" + (i + 1));
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {

                status = false;
            }
            return status;
        }
        private List<stockissuedetail> getStockIssueDetails()
        {
            List<stockissuedetail> SIDetails = new List<stockissuedetail>();
            try
            {
                if (!validateMaterialDetail())
                {
                    MessageBox.Show("Check material Detail");
                    return null;
                }
                stockissuedetail sid = new stockissuedetail();
                for (int i = 0; i < grdSIDetail.Rows.Count; i++)
                {
                    sid = new stockissuedetail();
                    sid.TemporaryNo = Convert.ToInt32(grdSIDetail.Rows[i].Cells["TempNo"].Value);
                    sid.TemporaryDate = Convert.ToDateTime(grdSIDetail.Rows[i].Cells["TempDate"].Value);
                    sid.StockItemID = grdSIDetail.Rows[i].Cells["StockItemID"].Value.ToString().Trim();
                    sid.ModelNo = grdSIDetail.Rows[i].Cells["ModelNo"].Value.ToString().Trim();
                    sid.IssueQuantity = Convert.ToDouble(grdSIDetail.Rows[i].Cells["IssueQuantity"].Value);
                    sid.UsedQuantity = Convert.ToDouble(grdSIDetail.Rows[i].Cells["UsedQuantity"].Value);
                    sid.ReturnedQuantity = Convert.ToDouble(grdSIDetail.Rows[i].Cells["ReturnedQuantity"].Value);
                    sid.DamagedQuantity = Convert.ToDouble(grdSIDetail.Rows[i].Cells["DamagedQuantity"].Value);
                    sid.MRNNo = Convert.ToInt32(grdSIDetail.Rows[i].Cells["MRNNo"].Value);
                    sid.RowID = Convert.ToInt32(grdSIDetail.Rows[i].Cells["ReferenceNo"].Value);
                    sid.StockReferenceNo = Convert.ToInt32(grdSIDetail.Rows[i].Cells["StockRefNo"].Value);
                    SIDetails.Add(sid);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Validation failed");
                return null;
            }
            return SIDetails;
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {

                StockIssueDB siDB = new StockIssueDB();
                
                List<stockissuedetail> SIDetails = new List<stockissuedetail>();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                int planNo = Convert.ToInt32(txtProductionPlanNo.Text);
                DateTime planDate = dtProductionPlanDate.Value;
                string btnText = btnSave.Text;
                SIDetails = getStockIssueDetails();
                if(SIDetails == null)
                {
                    MessageBox.Show("Check Issue Detail Grid");
                    return;
                }
                if (btnText.Equals("Save"))
                {
                    DialogResult dialog = MessageBox.Show("Are you sure to Save Entries?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        if (siDB.updateStockIssueUsage(SIDetails))
                        {
                            MessageBox.Show("STOCK Issue Details updated");
                            pnlAddEdit.Visible = false;
                            grdSIDetail.Rows.Clear();
                            ListFilteredProductionPlan();
                        }
                        else
                        {
                            MessageBox.Show("STOCK Issue Details updation Failed.");
                            status = false;
                        }
                    }
                    else
                    {
                        status = false;
                    }
                }
                else
                {
                    MessageBox.Show("Production Details Validation failed");
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("btnSave_Click_1() : Error");
                status = false;
            }
            if (status)
            {
                setButtonVisibility("btnEditPanel"); //activites are same for cancel, forward,approve, reverse and save
            }
        }

        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            clearData();
            closeAllPanels();
            pnlAddEdit.Visible = false;
            pnlList.Visible = true;
            grdList.Visible = true;
            setButtonVisibility("btnEditPanel");
        }
        private void pnlList_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lvForwardCancel_Click(object sender, EventArgs e)
        {
            try
            {
                lvApprover.CheckBoxes = false;
                lvApprover.CheckBoxes = true;
                pnlForwarder.Controls.Remove(lvApprover);
                pnlForwarder.Visible = false;
            }
            catch (Exception)
            {
            }
        }

        private void btnReverse_Click(object sender, EventArgs e)
        {
        }
        private void btnPDFClose_Click(object sender, EventArgs e)
        {

        }
        private void removeControlsFrompnllvPanel()
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
        private void btnListDocuments_Click(object sender, EventArgs e)
        {

        }

        private void dgvDocumentList_CellContentClick(Object sender, DataGridViewCellEventArgs e)
        {
        }
        private void setButtonVisibility(string btnName)
        {
            try
            {
                //btnExit.Visible = false;
                btnCancel.Visible = false;
                btnSave.Visible = false;

                btnGetComments.Visible = false;
                chkCommentStatus.Visible = false;
                txtComments.Visible = false;
                //disableTabPages();
                clearTabPageControls();
                //----24/11/2016
                ////handleGrdEditButton();
                pnlBottomButtons.Visible = true;
                //----
                if (btnName == "init")
                {
                    btnExit.Visible = true;
                }
                else if (btnName == "btnEditPanel") //called from cancel,save,forward,approve and reverse button events
                {
                    ////btnNew.Visible = true;
                    btnExit.Visible = true;
                }
                //gridview buttons
                else if (btnName == "Edit")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = true;
                    btnFinalize.Visible = false;
                }
                else if (btnName == "Finalize")
                {
                    pnlBottomButtons.Visible = false; //24/11/2016
                    btnCancel.Visible = true;
                    btnSave.Visible = false;
                    btnFinalize.Visible = true;
                }

                pnlEditButtons.Refresh();

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
        //call this form when new or edit buttons are clicked
        private void clearTabPageControls()
        {
            try
            {
                dgvComments.Rows.Clear();
                chkCommentStatus.Checked = false;
                txtComments.Text = "";
            }
            catch (Exception ex)
            {
            }
        }
        private void btnSelectProductionPlanNo_Click(object sender, EventArgs e)
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;

            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

            frmPopup.Size = new Size(600, 300);
            lv = ProductionPlanHeaderDB.getPlanNoListViewForRawMaterialUsage();
            //this.lv.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listView2_ItemChecked);
            lv.Bounds = new Rectangle(new Point(0, 0), new Size(600, 250));
            frmPopup.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.BackColor = Color.Tan;
            lvOK.Text = "OK";
            lvOK.Location = new Point(40, 265);
            lvOK.Click += new System.EventHandler(this.lvOK_Click4);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.BackColor = Color.Tan;
            lvCancel.Text = "CANCEL";
            lvCancel.Location = new Point(130, 265);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click4);
            frmPopup.Controls.Add(lvCancel);

            frmPopup.ShowDialog();

        }
        private void lvOK_Click4(object sender, EventArgs e)
        {
            try
            {
                //btnSelectProductionPlanNo.Enabled = true;
                int kount = 0;

                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        kount++;
                    }
                }
                if (kount == 0)
                {
                    MessageBox.Show("Select one Production Plan No");
                    return;
                }
                if (kount > 1)
                {
                    MessageBox.Show("Select only one Production Plan No");
                    return;
                }
                else
                {
                    foreach (ListViewItem itemRow in lv.Items)
                    {
                        if (itemRow.Checked)
                        {
                            txtProductionPlanNo.Text = itemRow.SubItems[1].Text;
                            dtProductionPlanDate.Value = Convert.ToDateTime(itemRow.SubItems[2].Text);
                            txtModel.Text = itemRow.SubItems[4].Text + "-" + itemRow.SubItems[5].Text;
                            txtProductName.Text = itemRow.SubItems[3].Text;
                            pnllv.Visible = false;
                            if (!AddGridDetailRowForMU(Convert.ToInt32(txtProductionPlanNo.Text), dtProductionPlanDate.Value))
                            {
                                MessageBox.Show("Failed to show Issue Dtails");
                            }
                            break;
                        }
                    }
                }
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }
        private Boolean AddGridSIDetailRow()
        {
            Boolean status = true;
            try
            {
                grdSIDetail.Rows.Add();
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["LineNum"].Value = grdSIDetail.RowCount;
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["ReferenceNo"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["TempNo"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["TempDate"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["DocumentNo"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["DocumentDate"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["IssueType"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["StockItemID"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["StockItemName"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["ModelNo"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["ModelName"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["IssueQuantity"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["UsedQuantity"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["DamagedQuantity"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["ReturnedQuantity"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["MRNNo"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["MRNDate"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["BatchNo"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["SerielNo"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["ExpiryDate"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["PurchaseQuantity"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["PurchasePrice"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["PurchaseTax"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["SupplierID"].Value = "";
                grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["SupplierName"].Value = "";
            }
            catch (Exception ex)
            {
                status = false;
            }

            return status;
        }
        private Boolean AddGridDetailRowForMU(int planNo, DateTime planDate)
        {
            grdSIDetail.Rows.Clear();
            Boolean status = true;
            int count = 0;
            StockIssueDB siDB = new StockIssueDB();
            List<stockissueheader> SIheader = siDB.getFilteredSIHeaderForRawMaterial(planNo, planDate);
            foreach (stockissueheader sih in SIheader)
            {
                count++;
                if (!AddDetailgridForDetail(sih))
                {
                    MessageBox.Show("Unable to show Grid Detail");
                    return false;
                }
            }
            if (count == 0)
            {
                MessageBox.Show("Stock Issue not prepared For selected Production plan");
                status = false;
            }
            return status;
        }
        private Boolean AddDetailgridForDetail(stockissueheader sih)
        {
            Boolean status = true;
            try
            {
                List<stockissuedetail> SIDetail = StockIssueDB.getPRDetail(sih);

                foreach (stockissuedetail sid in SIDetail)
                {
                    if (!AddGridSIDetailRow())
                    {
                        MessageBox.Show("failed to Add Rows.");
                        return false;
                    }
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["TempNo"].Value = sih.TemporaryNo;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["TempDate"].Value = sih.TemporaryDate;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["DocumentNo"].Value = sih.DocumentNo;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["DocumentDate"].Value = sih.DocumentDate;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["IssueType"].Value = sih.IssueType;
                    //----- 
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["StockRefNo"].Value = sid.StockReferenceNo;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["ReferenceNo"].Value = sid.RowID;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["StockItemID"].Value = sid.StockItemID;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["StockItemName"].Value = sid.StockItemName;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["ModelNo"].Value = sid.ModelNo;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["ModelName"].Value = sid.ModelName;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["IssueQuantity"].Value = sid.IssueQuantity;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["MRNNo"].Value = sid.MRNNo;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["MRNDate"].Value = sid.MRNDate;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["BatchNo"].Value = sid.BatchNo;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["SerielNo"].Value = sid.SerialNo;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["ExpiryDate"].Value = sid.ExpiryDate;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["PurchaseQuantity"].Value = sid.PurchaseQuantity;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["PurchasePrice"].Value = sid.PurchasePrice;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["PurchaseTax"].Value = sid.PurchaseTax;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["SupplierID"].Value = sid.SupplierID;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["SupplierName"].Value = sid.SupplierName;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["UsedQuantity"].Value = sid.UsedQuantity;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["ReturnedQuantity"].Value = sid.ReturnedQuantity;
                    grdSIDetail.Rows[grdSIDetail.RowCount - 1].Cells["DamagedQuantity"].Value = sid.DamagedQuantity;
                }

            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
        private void lvCancel_Click4(object sender, EventArgs e)
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
        //-----
        //private void listView2_ItemChecked(object sender, ItemCheckedEventArgs e)
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

        private void grdSIDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdSIDetail.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("del"))
                {
                    DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        grdSIDetail.Rows.RemoveAt(e.RowIndex);
                    }
                }
            }
            catch (Exception ex)
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
                if (columnName.Equals("Finalize") || columnName.Equals("Edit"))
                {
                    setButtonVisibility(columnName);
                    //if (columnName.Equals("Finalize"))
                    //{
                    //    btnSave.Visible = false;
                    //    btnFinalize.Visible = true;
                    //}
                    //else
                    //{
                    //    btnSave.Visible = true;
                    //    btnFinalize.Visible = false;
                    //}
                    prevprodution = new productionplanheader();
                    prevprodution.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gTemporaryNo"].Value.ToString());
                    prevprodution.TemporaryDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gTemporaryDate"].Value.ToString());
                    prevprodution.ProductionPlanNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gProductionPlanNo"].Value.ToString());
                    prevprodution.ProductionPlanDate = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gProductionPlanDate"].Value.ToString());

                    prevprodution.InternalOrderNos = grdList.Rows[e.RowIndex].Cells["gInternalOrderNo"].Value.ToString();
                    prevprodution.InternalOrderDates = grdList.Rows[e.RowIndex].Cells["gInternalOrderDate"].Value.ToString();
                    prevprodution.StockItemID = grdList.Rows[e.RowIndex].Cells["gStockItemID"].Value.ToString();
                    prevprodution.StockItemName = grdList.Rows[e.RowIndex].Cells["gStockItemName"].Value.ToString();
                    prevprodution.ModelNo = grdList.Rows[e.RowIndex].Cells["gModelno"].Value.ToString();
                    prevprodution.ModelName = grdList.Rows[e.RowIndex].Cells["gModelName"].Value.ToString();
                    prevprodution.Quantity = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gQuantity"].Value.ToString());

                    txtProductionPlanNo.Text = prevprodution.ProductionPlanNo.ToString();
                    dtProductionPlanDate.Value = prevprodution.ProductionPlanDate;
                    txtModel.Text = prevprodution.ModelNo + "-" + prevprodution.ModelName;
                    txtProductName.Text = prevprodution.StockItemName;
                    if (!AddGridDetailRowForMU(Convert.ToInt32(txtProductionPlanNo.Text), dtProductionPlanDate.Value))
                    {
                        MessageBox.Show("Failed to show Issue Dtails");
                        pnlBottomButtons.Visible = true;
                        return;
                    }
                    grdList.Visible = false;
                    pnlBottomButtons.Visible = false;
                    pnlAddEdit.Visible = true;
                    pnlAddEdit.BringToFront();
                    pnlAddEdit.Focus();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnFinalize_Click(object sender, EventArgs e)
        {
            try
            {
                List<stockissuedetail> SIDetails = new List<stockissuedetail>();
                SIDetails = getStockIssueDetails();
                if (SIDetails == null)
                {
                    MessageBox.Show("Check Issue Detail Grid");
                    return;
                }
                ProductionPlanHeaderDB pphdb = new ProductionPlanHeaderDB();
                DialogResult dialog = MessageBox.Show("Are you sure to Finalized the document ?", "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    if (ProductionPlanHeaderDB.FinalizeRawmaterialusageForPlan(Convert.ToInt32(txtProductionPlanNo.Text), dtProductionPlanDate.Value, SIDetails))
                    {
                        
                        MessageBox.Show("Raw Material Finalized");
                        pnlAddEdit.Visible = false;
                        grdSIDetail.Rows.Clear();
                        ListFilteredProductionPlan();
                    }
                    else
                        MessageBox.Show("Unable to Finalized");
                }
            }
            catch (Exception)
            {
            }
        }

        private void grdSIDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    grdSIDetail.Rows[e.RowIndex].Selected = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void grdList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    grdList.Rows[e.RowIndex].Selected = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void RawMaterialUsage_Enter(object sender, EventArgs e)
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















