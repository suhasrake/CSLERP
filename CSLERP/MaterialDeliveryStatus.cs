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

namespace CSLERP
{
    public partial class MaterialDeliveryStatus : System.Windows.Forms.Form
    {
        Panel pnllv = new Panel(); 
        ListView lv = new ListView();
        public static string[,] documentStatusValues;
        string docdata = "";
        DateTime dt = new DateTime();
        RichTextBox txt = new RichTextBox();
        public MaterialDeliveryStatus()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void MaterialDeliveryStatus_Load(object sender, EventArgs e)
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
            applyPrivilege();
        }
        private void ListDocument( int opt)
        {
            try
            {
                grdList.Rows.Clear();
                MaterialDeliveryDetailDB dbrecord = new MaterialDeliveryDetailDB();
                ////List<materialdelivery> Documents = dbrecord.getFilteredMaterialdetail(opt);
                ////foreach (materialdelivery doc in Documents)
                ////{

                ////    grdList.Rows.Add();
                ////    grdList.Rows[grdList.RowCount - 1].Cells["gDeliveryDocumentType"].Value = doc.DocumentType;
                ////    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentNo"].Value = doc.DocumentNo;
                ////    grdList.Rows[grdList.RowCount - 1].Cells["gDocumentDate"].Value = doc.DocumentDate;
                ////    grdList.Rows[grdList.RowCount - 1].Cells["gConsignee"].Value = doc.consignee;
                ////    grdList.Rows[grdList.RowCount - 1].Cells["gCourierID"].Value = doc.courierID;
                ////    grdList.Rows[grdList.RowCount - 1].Cells["gTransportationMode"].Value = doc.transportationMode;
                ////    grdList.Rows[grdList.RowCount - 1].Cells["gLRNo"].Value = doc.LRNo;
                ////    grdList.Rows[grdList.RowCount - 1].Cells["gLRDate"].Value = doc.LRDate;
                ////    grdList.Rows[grdList.RowCount - 1].Cells["gDeliveryDate"].Value = doc.DeliveryDate;
                ////    grdList.Rows[grdList.RowCount - 1].Cells["gRemarks"].Value = doc.Remarks;
                ////    grdList.Rows[grdList.RowCount - 1].Cells["gStatus"].Value = doc.status;
                ////    grdList.Rows[grdList.RowCount - 1].Cells["gCreateTime"].Value = doc.CreateTime;
                ////    grdList.Rows[grdList.RowCount - 1].Cells["gCreateUser"].Value = doc.CreateUser;
                ////    grdList.Rows[grdList.RowCount - 1].Cells["gDeliveryStatus"].Value = setStatus(doc.DeliveryStatus);
                ////}
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            enableBottomButtons();
            pnlDocumentList.Visible = true;
        }

        public string setStatus(int status)
        {
            string stat = "";
            if (status == 1)
            {
                stat = "In Transit";
            }
            else if (status == 2)
            {
                stat = "Delivered";
            }
            return stat;
        }
        public int setStatusstring(string status)
        {
            int stat = 0;
            if (status == "In Transit")
            {
                stat = 1;
            }
            else if (status == "Delivered")
            {
                stat = 2;
            }
            return stat;
        }

        private void initVariables()
        {
            try
            {
                CustomerDB.fillCustomerComboNew(cmbConsignee);
                CatalogueValueDB.fillCatalogValueComboNew(cmbTransportationmode, "TransportationMode");
                //CatalogueValueDB.fillCustomerComboNew(cmbTransportationmode, "TransportationMode");
                dtDocumentDate.Format = DateTimePickerFormat.Custom;
                dtDocumentDate.CustomFormat = "dd-MM-yyyy";
                dtDocumentDate.Enabled = false;
                dtLRdate.Format = DateTimePickerFormat.Custom;
                dtLRdate.CustomFormat = "dd-MM-yyyy";
                dtDeliveryDate.Format = DateTimePickerFormat.Custom;
                dtDeliveryDate.CustomFormat = "dd-MM-yyyy";
                pnlUI.Controls.Add(pnlDocumentList);
                closeAllPanels();
                setbuttonVisibility("init");

            }
            catch (Exception)
            {

            }

        }
        private string getStatusString(int stat)
        {
            string str = "Unkown";
            try
            {
                if (stat == 0)
                {
                    str = "Deactive";
                }
                else
                    str = "Active";
            }
            catch (Exception)
            {
                return str;
            }
            return str;
        }
        private int getstatuscode(string str)
        {
            int i = 0;
            try
            {
                if (str == "Active")
                {
                    i = 1;
                }
                else
                {
                    i = 0;
                }
            }
            catch ( Exception ex)
            {
                return i;
            }
            return i;
        }

        int getuserPrivilegeStatus()
        {
            try
            {
                if (Main.itemPriv[0] && !Main.itemPriv[1] && !Main.itemPriv[2]) 
                    return 1;
                else if (Main.itemPriv[0] && (Main.itemPriv[1] || Main.itemPriv[2])) 
                    return 2;
                else if (!Main.itemPriv[0] && (Main.itemPriv[1] || Main.itemPriv[2])) 
                    return 3;
                else if (!Main.itemPriv[0] && !Main.itemPriv[1] || !Main.itemPriv[2]) 
                    return 0;
                else
                    return -1;
            }
            catch (Exception ex)
            {
            }
            return 0;
        }
        private void applyPrivilege()
        {
            try
            {
                if (Main.itemPriv[1])
                {
                    
                }
                else
                {
                    
                }
                if (Main.itemPriv[2])
                {
                    grdList.Columns["gView"].Visible = true;
                    grdList.Columns["Edit"].Visible = true;
                }
                else
                {
                    grdList.Columns["gView"].Visible = true;
                    grdList.Columns["gEdit"].Visible = false;
                }
            }
            catch (Exception)
            {
            }
        }

        private void closeAllPanels()
        {
            try
            {
                pnlDocumentInner.Visible = false;
                pnlDocumentOuter.Visible = false;
                pnlDocumentList.Visible = false;
                pnlSelection.Visible = true;
            }
            catch (Exception)
            {

            }
        }


        private void fillDocumentStatusCombo(System.Windows.Forms.ComboBox cmb)
        {
            try
            {
                cmb.Items.Clear();
                for (int i = 0; i < Main.statusValues.GetLength(0); i++)
                {
                    cmb.Items.Add(Main.statusValues[i, 1]);
                }
            }
            catch (Exception)
            {

            }
        }
        private void fillIsReversibleCombo(System.Windows.Forms.ComboBox cmb)
        {
            try
            {
                cmb.Items.Clear();
                for (int i = 0; i < Main.YesNo.GetLength(0); i++)
                {
                    cmb.Items.Add(Main.YesNo[i, 1]);
                }
            }
            catch (Exception)
            {

            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if(rdbDelivered.Checked==true)
                {
                    ListDocument(3);
                }
                else
                {
                    ListDocument(2);
                }

                closeAllPanels();
                clearDocumentData();
                enableBottomButtons();
                pnlBottomButtons.Visible = true;
                pnlSelection.Visible = true;
                pnlDocumentList.Visible = true;
                btnExit.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        public void clearDocumentData()
        {
            try
            {
                txtDocno.Text = "";
                txtLRno.Text = "";
                txtRemarks.Text = "";
                dtDeliveryDate.Value = DateTime.Parse("01-01-1900");
                cmbConsignee.SelectedIndex = -1;
                ///cmbDeliveryStatus.SelectedIndex = -1;
                cmbTransportationmode.SelectedIndex = -1;
                dtDocumentDate.Value = DateTime.Parse("01-01-1900");
                dtLRdate.Value = DateTime.Parse("01-01-1900");
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
               
            }
            catch (Exception)
            {

            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                materialdelivery matdel = new materialdelivery();
                MaterialDeliveryDetailDB MATDB = new MaterialDeliveryDetailDB();
                matdel.DocumentDate = dtDocumentDate.Value; 
                matdel.DocumentNo = Convert.ToInt32(txtDocno.Text);
                matdel.DeliveryDate = dtDeliveryDate.Value;
                matdel.DeliveryStatus = setStatusstring(cmbDeliveryStatus.SelectedItem.ToString().Trim());
                matdel.Remarks = txtRemarks.Text;
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                {
                    if (btnText.Equals("Update"))
                    {
                        if (cmbDeliveryStatus.SelectedItem.ToString() == "In Transit")
                        {

                            if (MATDB.updateMaterialDeliveryStatus(matdel))
                            {
                                MessageBox.Show("Document Remarks updated");
                                closeAllPanels();
                                enableBottomButtons();
                                ListDocument(2);
                                pnlSelection.Visible = true;
                                pnlDocumentList.Visible = true;
                            }
                            else
                            {
                                MessageBox.Show("Failed to update Document Remarks");
                            }
                        }
                        else if (cmbDeliveryStatus.SelectedItem.ToString() == "Delivered")
                        {
                            if (validatedelivery())
                            {
                                if (MATDB.updateDelivery(matdel))
                                {
                                    MessageBox.Show("Document Remarks updated");
                                    closeAllPanels();
                                    enableBottomButtons();
                                    ListDocument(2);
                                    pnlSelection.Visible = true;
                                    pnlDocumentList.Visible = true;
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update Document Remarks");
                                }

                            }
                            else
                            {
                                MessageBox.Show("vALIDATION FAILED");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Failed Adding / Editing User Data");
            }
        }
            public string addremark(string remark)
        {
            string rmrk = "";
            try
            {
                if(remark.Contains("-"))
                {
                    string[] str = remark.Split('-');
                    rmrk = Login.userLoggedInName + "-" + UpdateTable.getSQLDateTime() + "-" + str[2];
                }
                else
                {
                    rmrk = Login.userLoggedInName + "-" + UpdateTable.getSQLDateTime() + "-" + txtRemarks.Text;
                }
            }
            catch(Exception ex)
            {

            }
            return rmrk;
        }

        private void disableBottomButtons()
        {
            btnExit.Visible = false;
        }
        private void enableBottomButtons()
        {
            btnExit.Visible = true;
        }
        public string getuserId(string empID)
        {
            string empid = DocumentUCDB.getUserID(empID);
            return empid;
        }

        private void pnlDocumentList_Paint(object sender, PaintEventArgs e)
        {

        }
        private void pnlDocumentOuter_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbDocumentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public Boolean validatedelivery()
        {
            bool st = true;
            {
                if (cmbDeliveryStatus.SelectedItem.ToString() == "Delivered" )
                {
                    if(dtDeliveryDate.Value.ToString()=="01-01-0001")
                    {
                        st = false;
                    }
                }
            }
            return st;
        }
        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("gEdit") || columnName.Equals("gView"))
                {                    
                    int rowID = e.RowIndex;
                    clearDocumentData();
                    ////////cmbConsignee.SelectedIndex = cmbConsignee.FindString(grdList.Rows[e.RowIndex].Cells["gConsignee"].Value.ToString());
                    cmbConsignee.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbConsignee, grdList.Rows[e.RowIndex].Cells["gConsignee"].Value.ToString());
                    cmbTransportationmode.SelectedIndex = 
                        Structures.ComboFUnctions.getComboIndex(cmbTransportationmode,grdList.Rows[e.RowIndex].Cells["gTransportationMode"].Value.ToString());
                    txtDocno.Text = grdList.Rows[e.RowIndex].Cells["gDocumentNo"].Value.ToString();
                    txtLRno.Text = grdList.Rows[e.RowIndex].Cells["gLRNo"].Value.ToString();
                    cmbDeliveryStatus.SelectedIndex = cmbDeliveryStatus.FindString(grdList.Rows[e.RowIndex].Cells["gDeliveryStatus"].Value.ToString());
                    dtDocumentDate.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gDocumentDate"].Value.ToString());
                    if(rdbDelivered.Checked==true)
                    {
                        dtDeliveryDate.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gDeliveryDate"].Value.ToString());
                    }
                    dtLRdate.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["gLRDate"].Value.ToString());
                    txtRemarks.Text = grdList.Rows[e.RowIndex].Cells["gRemarks"].Value.ToString();
                    setbuttonVisibility(columnName);
                    disableBottomButtons(); 
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void setbuttonVisibility(string btnname)
        {
            pnlDocumentList.Visible = false;
            pnlDocumentOuter.Visible = false;
            pnlDocumentInner.Visible = false;
            pnlSelection.Visible = false;
            txtRemarks.Enabled = false;
            btnAddRemarks.Visible = false;
           
            
            if(btnname == "gEdit")
            {
                btnSave.Text = "Update";
                pnlDocumentList.Visible = false;
                pnlSelection.Visible = false;
                pnlDocumentOuter.Visible = true;
                pnlDocumentInner.Visible = true;
                txtDocno.Enabled = false;
                dtDeliveryDate.Enabled = false;
                cmbDeliveryStatus.Enabled = true;
                txtRemarks.Enabled = false;
                txtLRno.Enabled = false;
                dtDocumentDate.Enabled = false;
                dtLRdate.Enabled = false;
                cmbConsignee.Enabled = false;
                cmbTransportationmode.Enabled = false;
                btnSave.Visible = true;
                btnAddRemarks.Visible = true;
            }
            if(btnname=="init")
            {
                pnlSelection.Visible = true;
                pnlBottomButtons.Visible = true;
            }
            if(btnname=="gView")
            {
                pnlDocumentOuter.Visible = true;
                pnlDocumentInner.Visible = true;
                pnlDocumentList.Visible = false;
                pnlSelection.Visible = false;
                pnlBottomButtons.Visible = false;
                btnCancel.Visible = true;
                txtDocno.Enabled = false;
                dtDeliveryDate.Enabled = false;
                cmbDeliveryStatus.Enabled = false;
                txtLRno.Enabled = false;
                dtDocumentDate.Enabled = false;
                dtLRdate.Enabled = false;
                cmbConsignee.Enabled = false;
                cmbTransportationmode.Enabled = false;
                txtRemarks.Enabled = false;
                btnSave.Visible = false;
                btnAddRemarks.Visible = false;
            }
            if(btnname=="In Transit")
            {
                grdList.Columns["gEdit"].Visible = true;
                grdList.Columns["gView"].Visible = true;
                grdList.Columns["gDeliveryDate"].Visible = false;
                pnlDocumentOuter.Visible = false;
                pnlDocumentInner.Visible = false;
                pnlDocumentList.Visible = true;
                pnlSelection.Visible = true;
                btnCancel.Visible = true;
                btnAddRemarks.Visible = true;
            }
            if(btnname == "Delivered")
            {
                grdList.Columns["gEdit"].Visible = false;
                grdList.Columns["gView"].Visible = true;
                grdList.Columns["gDeliveryDate"].Visible = true;
                pnlDocumentOuter.Visible = false;
                pnlDocumentInner.Visible = false;
                pnlDocumentList.Visible = true;
                pnlSelection.Visible = true;
                btnCancel.Visible = true;
                btnAddRemarks.Visible = false;
            }
        }

        private void rdbInTransit_CheckedChanged(object sender, EventArgs e)
        {
            setbuttonVisibility(rdbInTransit.Text);
            ListDocument(2);
            applyPrivilege();
        }

        private void rdbDelivered_CheckedChanged(object sender, EventArgs e)
        {
            setbuttonVisibility(rdbDelivered.Text);
            ListDocument(3);
            applyPrivilege();
        }

        private void cmbDeliveryStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDeliveryStatus.SelectedItem.ToString() == "Delivered" && rdbInTransit.Checked == true)
            {
                dtDeliveryDate.Enabled = true;
                dtDeliveryDate.Visible = true;
                lblDeliveryDate.Visible = true;
            }
            else if (rdbDelivered.Checked == true)
            {
                dtDeliveryDate.Enabled = false;
                dtDeliveryDate.Visible = true;
                lblDeliveryDate.Visible = true;
            }
            else
            {
                dtDeliveryDate.Enabled = false;
                dtDeliveryDate.Visible = false;
                lblDeliveryDate.Visible = false;
            }
           
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
        
        private void btnAddRemarks_Click(object sender, EventArgs e)
        {
            removeControlsFrompnllvPanel();

            txt = new RichTextBox();
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;

            pnllv.Bounds = new Rectangle(new Point(200, 30), new Size(400, 300));
            txt.Bounds = new Rectangle(new Point(25, 25), new Size(350, 200));
            pnllv.Controls.Add(txt);

            Label lblHeader = new Label();
            lblHeader.Size = new Size(300, 20);
            lblHeader.Text = "Type your Comments             ";
            lblHeader.Location = new Point(50, 10);
            pnllv.Controls.Add(lblHeader);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Location = new Point(200, 240);
            lvOK.Click += new System.EventHandler(this.lvOK_Click5);
            pnllv.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Location = new Point(100, 240);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click5);
            pnllv.Controls.Add(lvCancel);

            pnlDocumentInner.Controls.Add(pnllv);
            pnllv.BringToFront();
            pnllv.Visible = true;
        }
        
        private void lvOK_Click5(object sender, EventArgs e)
        {
            try
            {
                pnllv.Visible = true;
                if (txt.Text.Trim().Length == 0)
                {
                    MessageBox.Show("Please Add Remarks");
                    return;
                }
                txtRemarks.Text = txtRemarks.Text + Environment.NewLine + Login.userLoggedInName + " : " + DateTime.Now.ToString() + " : " + txt.Text ;
                //btnAddRemarks.Enabled = true;
                pnllv.Visible = false;
                txtRemarks.ReadOnly = true;
                ContextMenu blankContextMenu = new ContextMenu();
                txtRemarks.ContextMenu = blankContextMenu;
            }
            catch (Exception ex)
            {

            }
        }

        private void lvCancel_Click5(object sender, EventArgs e)
        {
            try
            {
                //btnAddRemarks.Enabled = true;
                pnllv.Visible = false;
            }
            catch (Exception)
            {
            }
        }

        private void MaterialDeliveryStatus_Enter(object sender, EventArgs e)
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


