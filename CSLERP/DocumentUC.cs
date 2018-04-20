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
    public partial class DocumentUC : System.Windows.Forms.Form
    {
        Panel pnllv = new Panel(); 
        ListView lv = new ListView();
        public static string[,] documentStatusValues;
        string docdata = "";
        public DocumentUC()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void DocumentUC_Load(object sender, EventArgs e)
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
            ListDocument();
            applyPrivilege();
        }
        private void ListDocument()
        {
            try
            {
                grdList.Rows.Clear();
                DocumentUCDB dbrecord = new DocumentUCDB();
                List<documentuc> Documents = dbrecord.getDocuments();
                foreach (documentuc doc in Documents)
                {

                    grdList.Rows.Add(doc.UserID, doc.empid, doc.empname,
                         getStatusString(doc.Status));
                    //dbrecord.getEmpStatusString(emp.empStatus), emp.empPhoto);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            enableBottomButtons();
            pnlDocumentList.Visible = true;
        }

        private void initVariables()
        {
            try
            {
                
                //documentStatusValues = new string[2, 2]
                //        {
                //    {"1","Active" },
                //    {"0","Dective" }
                //        };
                fillDocumentStatusCombo(cmbUserStatus);
                
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
        private void applyPrivilege()
        {
            try
            {
                if (Main.itemPriv[1])
                {
                    btnNew.Visible = true;
                }
                else
                {
                    btnNew.Visible = false;
                }
                if (Main.itemPriv[2])
                {
                    grdList.Columns["Edit"].Visible = true;
                }
                else
                {
                    grdList.Columns["Edit"].Visible = false;
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
                closeAllPanels();
                clearDocumentData();
                enableBottomButtons();
                pnlDocumentList.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        public void clearDocumentData()
        {
            try
            {
                txtUserID.Text = "";
                removeControlsFromlvPanel();
                cmbUserStatus.SelectedIndex = 0;
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

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                clearDocumentData();
                btnSave.Text = "Save";
                pnlDocumentOuter.Visible = true;
                pnlDocumentInner.Visible = true;
                btnsel.Visible = true;
                txtUserID.Enabled = true;
                disableBottomButtons();
                cmbUserStatus.SelectedIndex = 0;               
            }
            catch (Exception)
            {

            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                documentuc doc = new documentuc();
                DocumentUCDB docDB = new DocumentUCDB();
                doc.UserID = getuserId(txtUserID.Text);                             
                doc.Status = getstatuscode(cmbUserStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                {
                    if (btnText.Equals("Update"))
                    {
                        
                        if (docDB.updateDocument(doc))
                        {
                            MessageBox.Show("Document Status updated");
                            closeAllPanels();
                            ListDocument();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update Document Status");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (docDB.validateDocument(doc))
                        {
                            if (docDB.insertDocument(doc))
                            {
                                MessageBox.Show("Document data Added");
                                closeAllPanels();
                                ListDocument();
                            }
                            else
                            {
                                MessageBox.Show("Failed to Insert Document");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Employee UserId not found");
                        }
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing User Data");
            }

        }
        private void disableBottomButtons()
        {
            btnNew.Visible = false;
            btnExit.Visible = false;
        }
        private void enableBottomButtons()
        {
            btnNew.Visible = true;
            btnExit.Visible = true;
        }

        private void btnsel_Click(object sender, EventArgs e)
        {
            //btnsel.Enabled = false;
            removeControlsFromlvPanel();
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;
            pnllv.Location = new Point(34, 68);
            pnllv.Size = new Size(466, 245);

            lv = EmployeePostingDB.getEmployeeListView();
            lv.Sorting = System.Windows.Forms.SortOrder.None;
            lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            lv.Location = new Point(13, 9);
            lv.Size = new Size(440, 199);
            //lv.ListViewItemSorter = new ListViewItemComparer();
            pnllv.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Location = new Point(38, 215);
            lvOK.Click += new System.EventHandler(this.lvOK_Click1);
            pnllv.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Location = new Point(155, 215);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
            pnllv.Controls.Add(lvCancel);

            pnlDocumentInner.Controls.Add(pnllv);
            pnllv.BringToFront();
            pnllv.Visible = true;
        }
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                        count++;
                }
                if (count != 1)
                {
                    MessageBox.Show("Select one item");
                    return;
                }
                //btnsel.Enabled = true;
                pnllv.Visible = false;
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        txtUserID.Text = itemRow.SubItems[1].Text;
                        txtEmpName.Text = itemRow.SubItems[2].Text;
                        //docdata = getemp(txtDocumentID.Text);           
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                //btnsel.Enabled = true;
                pnllv.Visible = false;
            }
            catch (Exception)
            {
            }
        }
        private void LvColumnClick(object o, ColumnClickEventArgs e)
        {
            try
            {
                string first = lv.Items[0].SubItems[e.Column].Text;
                string last = lv.Items[lv.Items.Count - 1].SubItems[e.Column].Text;
                System.Windows.Forms.SortOrder sort_order1 = SortingListView.getSortedOrder(first, last);
                this.lv.ListViewItemSorter = new SortingListView(e.Column, sort_order1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorting error");
            }
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

        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit"))
                {                    
                    btnsel.Visible = false;
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlDocumentInner.Visible = true;
                    pnlDocumentOuter.Visible = true;
                    pnlDocumentList.Visible = false;
                    txtUserID.Enabled = false;
                    cmbUserStatus.SelectedIndex = 0;
                    cmbUserStatus.SelectedIndex = cmbUserStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[2].Value.ToString());
                    DataGridViewRow row = grdList.Rows[rowID];
                    txtUserID.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    ERPUserDB erpuserdb = new ERPUserDB();
                    txtEmpName.Text = grdList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    disableBottomButtons();
                }
            }
            catch (Exception)
            {

            }
        }

        private void DocumentUC_Enter(object sender, EventArgs e)
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


