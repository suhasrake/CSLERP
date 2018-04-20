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
    public partial class ProductTestDescription : System.Windows.Forms.Form
    {

        public static string[,] documentStatusValues;
        public ProductTestDescription()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void ProductTestDescription_Load(object sender, EventArgs e)
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
            ListProductTestDesc();
            applyPrivilege();
        }
        private void ListProductTestDesc()
        {
            try
            {
                grdList.Rows.Clear();
                ProductTestDescriptionDB  ptDescDB = new ProductTestDescriptionDB();
                List<productTestDesc> PTDescList = ptDescDB.getProductTestDescriptionList();
                foreach (productTestDesc ptdesc in PTDescList)
                {

                    grdList.Rows.Add(ptdesc.TestDescriptionID, ptdesc.TestDescription,
                         ComboFIll.getStatusString(ptdesc.Status), ptdesc.CreateUser,ptdesc.CreateTime);
                    //dbrecord.getEmpStatusString(emp.empStatus), emp.empPhoto);
                }
            }
            catch (Exception ex)
            {
               
            }
            enableBottomButtons();
            pnlDocumentList.Visible = true;
        }

        private void initVariables()
        {
            try
            {
                
                fillUserStatusCombo(cmbStatus);
            }
            catch (Exception)
            {

            }

        }
        private void fillUserStatusCombo(System.Windows.Forms.ComboBox cmb)
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


        //private void fillDocumentStatusCombo(System.Windows.Forms.ComboBox cmb)
        //{
        //    try
        //    {
        //        cmb.Items.Clear();
        //        for (int i = 0; i < documentStatusValues.GetLength(0); i++)
        //        {
        //            cmb.Items.Add(documentStatusValues[i, 1]);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

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
                txtTestDescID.Text = "";
                txtTestDesc.Text = "";
                cmbStatus.SelectedIndex = 0;
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
                txtTestDescID.Enabled = true;
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
                productTestDesc ptdesc = new productTestDesc();
                ProductTestDescriptionDB ptdescDB = new ProductTestDescriptionDB();
                ptdesc.TestDescriptionID = txtTestDescID.Text;
                ptdesc.TestDescription = txtTestDesc.Text;
                ptdesc.Status = ComboFIll.getStatusCode(cmbStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;

                {
                    if (btnText.Equals("Update"))
                    {
                        if (ptdescDB.updateProductTestDescription(ptdesc))
                        {
                            MessageBox.Show("Document Status updated");
                            closeAllPanels();
                            ListProductTestDesc();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update Document Status");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (ptdescDB.validateProductTestDescription(ptdesc))
                        {
                            if (ptdescDB.insertProductTestDescription(ptdesc))
                            {
                                MessageBox.Show("Document data Added");
                                closeAllPanels();
                                ListProductTestDesc();
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
                if (e.ColumnIndex == 5)
                {
                    int rowID = e.RowIndex;
                    ////string tempDate = "";
                    //Edit Button
                    //MessageBox.Show("You clicked edit button");
                    btnSave.Text = "Update";
                    pnlDocumentInner.Visible = true;
                    pnlDocumentOuter.Visible = true;
                    pnlDocumentList.Visible = false;
                    txtTestDescID.Enabled = false;
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[2].Value.ToString());
                    DataGridViewRow row = grdList.Rows[rowID];
                    txtTestDescID.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtTestDesc.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();
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

        private void ProductTestDescription_Enter(object sender, EventArgs e)
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

