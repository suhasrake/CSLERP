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
    public partial class DocCommenter : System.Windows.Forms.Form
    {
        doccommenter prevdoc ;
        public DocCommenter()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void DocCommenter_Load(object sender, EventArgs e)
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
            ListDocCommentor();
            applyPrivilege();
        }
        private void ListDocCommentor()
        {
            try
            {
                grdList.Rows.Clear();
                DocCommenterDB dcdb = new DocCommenterDB();
                List<doccommenter> docCommList = dcdb.getDocCommList();
                foreach (doccommenter doc in docCommList)
                {
                    grdList.Rows.Add(doc.DocumentID, doc.DocumentName,
                        doc.EmployeeName + "-" + doc.EmployeeID,
                         ComboFIll.getStatusString(doc.DocumentStatus));
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
                
                fillDocumentStatusCombo(cmbDocumentStatus);
                EmployeeDB.fillEmpListCombo(cmbEmployee);
                MenuItemDB.fillMenuItemComboNew(cmbDocument);
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
                cmbDocumentStatus.SelectedIndex = -1;
                cmbEmployee.SelectedIndex = -1;
                prevdoc = new doccommenter();
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
                cmbDocument.Enabled = true;
                cmbDocument.SelectedIndex = -1;
                cmbEmployee.SelectedIndex = -1;
                cmbDocument.Enabled = true;
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
                doccommenter doc = new doccommenter();
                DocCommenterDB docDB = new DocCommenterDB();


                try
                {
                    ////////doc.DocumentID = cmbDocument.SelectedItem.ToString().Trim().Substring(0, cmbDocument.SelectedItem.ToString().Trim().IndexOf('-'));
                    doc.DocumentID = ((Structures.ComboBoxItem)cmbDocument.SelectedItem).HiddenValue;
                    ////doc.DocumentName = cmbDocument.SelectedItem.ToString().Trim().Substring(cmbDocument.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                    doc.DocumentName = ((Structures.ComboBoxItem)cmbDocument.SelectedItem).ToString();
                    doc.EmployeeName = cmbEmployee.SelectedItem.ToString().Trim().Substring(0, cmbEmployee.SelectedItem.ToString().Trim().IndexOf('-'));
                    doc.EmployeeID = cmbEmployee.SelectedItem.ToString().Trim().Substring(cmbEmployee.SelectedItem.ToString().Trim().IndexOf('-') + 1);                  
                }
                catch (Exception)
                {
                    doc.DocumentID = "";
                    doc.DocumentName = "";
                    doc.EmployeeName = "";
                    doc.EmployeeID = "";
                }

                doc.DocumentStatus = ComboFIll.getStatusCode(cmbDocumentStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;

                {
                    if (btnText.Equals("Update"))
                    {
                        if (docDB.updateDocCommList(doc, prevdoc))
                        {
                            MessageBox.Show("DocEmpMapping Status updated");
                            closeAllPanels();
                            ListDocCommentor();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update DocEmpMapping Status");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (docDB.validateDocument(doc))
                        {
                            if (docDB.insertDocCommList(doc))
                            {
                                MessageBox.Show("DocEmpMapping data Added");
                                closeAllPanels();
                                ListDocCommentor();
                            }
                            else
                            {
                                MessageBox.Show("Failed to Insert DocEmpMapping");
                            }
                        }
                        else
                        {
                            MessageBox.Show("DocEmpMapping Data Validation failed");
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
            if (e.RowIndex < 0)
                return;
            try
            {
                if (e.ColumnIndex == 4)
                {
                    clearDocumentData();
                    prevdoc = new doccommenter();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlDocumentInner.Visible = true;
                    pnlDocumentOuter.Visible = true;
                    pnlDocumentList.Visible = false;
                    prevdoc.DocumentID = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    prevdoc.EmployeeID = grdList.Rows[e.RowIndex].Cells[2].Value.ToString().Substring(grdList.Rows[e.RowIndex].Cells[2].Value.ToString().Trim().IndexOf('-') + 1);
                    ////cmbDocument.SelectedIndex = cmbDocument.FindStringExact(grdList.Rows[e.RowIndex].Cells[0].Value.ToString() + "-" + grdList.Rows[e.RowIndex].Cells[1].Value.ToString());
                    cmbDocument.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbDocument, grdList.Rows[e.RowIndex].Cells[0].Value.ToString());
                    cmbEmployee.SelectedIndex = cmbEmployee.FindStringExact(grdList.Rows[e.RowIndex].Cells[2].Value.ToString());
                   
                    cmbDocumentStatus.SelectedIndex = cmbDocumentStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[3].Value.ToString());
                    cmbDocument.Enabled = false;
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

        private void DocCommenter_Enter(object sender, EventArgs e)
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

