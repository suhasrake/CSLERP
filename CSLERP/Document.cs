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

namespace CSLERP
{
    public partial class Document : System.Windows.Forms.Form
    {

        public static string[,] documentStatusValues;
        public Document()
        {
            try
            {
                InitializeComponent();
                //////this.FormBorderStyle = FormBorderStyle.None;
               
            }
            catch (Exception)
            {

            }
        }
        private void Document_Load(object sender, EventArgs e)
        {
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
                DocumentDB dbrecord = new DocumentDB();
                List<document> Documents = dbrecord.getDocuments();
                foreach (document doc in Documents)
                {

                    grdList.Rows.Add(doc.DocumentID, doc.DocumentName,doc.TableName,
                        dbrecord.getYesNo(doc.IsReversible),
                         dbrecord.getDocumentStatusString(doc.DocumentStatus));
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
                
                ////documentStatusValues = new string[2, 2]
                ////        {
                ////    {"1","Active" },
                ////    {"2","Dective" }
                ////        };
                fillDocumentStatusCombo(cmbDocumentStatus);
                fillIsReversibleCombo(cmbIsReversible);
            }
            catch (Exception)
            {

            }

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
                txtDocumentID.Text = "";
                txtDocumentName.Text = "";
                cmbDocumentStatus.SelectedIndex = 0;
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
                txtDocumentID.Enabled = true;
                disableBottomButtons();
            }
            catch (Exception)
            {

            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                document doc = new document();
                DocumentDB docDB = new DocumentDB();
                doc.DocumentID = txtDocumentID.Text;
                doc.DocumentName = txtDocumentName.Text;
                doc.TableName = txtTableName.Text;
                doc.IsReversible = docDB.getYesNoCode(cmbIsReversible.SelectedItem.ToString());
                doc.DocumentStatus = docDB.getDocumentStatusCode(cmbDocumentStatus.SelectedItem.ToString());
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
                            MessageBox.Show("Document Data Validation failed");
                        }
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing User Data");
            }

        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Edit"))              
                {
                    int rowID = e.RowIndex;
                    ////string tempDate = "";
                    //Edit Button
                    //MessageBox.Show("You clicked edit button");
                    btnSave.Text = "Update";
                    pnlDocumentInner.Visible = true;
                    pnlDocumentOuter.Visible = true;
                    pnlDocumentList.Visible = false;
                    txtDocumentID.Enabled = false;
                    cmbDocumentStatus.SelectedIndex = cmbDocumentStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[2].Value.ToString());
                    DataGridViewRow row = grdList.Rows[rowID];
                    txtDocumentID.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtDocumentName.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    txtTableName.Text = grdList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    disableBottomButtons();
                }
            }
            catch (Exception)
            {

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

        private void Document_Enter(object sender, EventArgs e)
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

