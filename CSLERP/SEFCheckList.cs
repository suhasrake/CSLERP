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
    public partial class SEFCheckList : System.Windows.Forms.Form
    {
        sefcheck sef;
        public SEFCheckList()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void FinancialLimit_Load(object sender, EventArgs e)
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
        private void ListEmpFinLimit(string sefid)
        {
            try
            {
                grdList.Rows.Clear();
                SEFCheckListDB fdb = new SEFCheckListDB();
                List<sefcheck> finList = fdb.getSEFCheckList(sefid);
                foreach (sefcheck flim in finList)
                {
                    grdList.Rows.Add(flim.rowid, flim.SEFID,
                        flim.Sequenceno, flim.description,
                         ComboFIll.getStatusString(flim.Status));
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
                pnlDocumentList.Visible = false;
                CatalogueValueDB.fillCatalogValueComboNew(cmbSEFType, "SEFType");
                cmbSEFType.SelectedIndex = 0;
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
                cmbSEFType.Enabled = true;
            }
            catch (Exception)
            {

            }
        }

        public void clearDocumentData()
        {
            try
            {
                cmbSEFstatus.SelectedIndex = -1;
                txtDescription.Text = "";
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
                cmbSEFType.Enabled = false;
                txtSequenceNo.Enabled = true;
                txtDescription.Text = "";
                txtSequenceNo.Text = "";
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
                sefcheck flist = new sefcheck();
                SEFCheckListDB fdb = new SEFCheckListDB();
                flist.SEFID = ((Structures.ComboBoxItem)cmbSEFType.SelectedItem).HiddenValue;
                flist.description = txtDescription.Text;
                try
                {
                    flist.Sequenceno = Convert.ToInt32(txtSequenceNo.Text);
                }
                catch(Exception)
                {
                    flist.Sequenceno = 0;
                }
                flist.Status = ComboFIll.getStatusCode(cmbSEFstatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                {
                    if (btnText.Equals("Update"))
                    {
                        flist.rowid = sef.rowid;
                        if (fdb.updateSEFCheckList(flist))
                        {
                            MessageBox.Show("SEFCheckList updated");
                            closeAllPanels();
                            ListEmpFinLimit(flist.SEFID);
                            cmbSEFType.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("Failed to update SEFCheckList");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (fdb.validateSEFChecklist(flist))
                        {
                            if (fdb.insertSefChecklist(flist))
                            {
                                MessageBox.Show("SEFCheckList data Added");
                                closeAllPanels();
                                ListEmpFinLimit(flist.SEFID);
                                cmbSEFType.Enabled = true;
                            }
                            else
                            {
                                MessageBox.Show("Failed to Insert SEFCheckList");
                            }
                        }
                        else
                        {
                            MessageBox.Show("SEFCheckList Data Validation failed");
                        }
                    }
                }
            }
            catch (Exception ex)
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
                if (e.ColumnIndex == 5)
                {
                    clearDocumentData();
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlDocumentInner.Visible = true;
                    pnlDocumentOuter.Visible = true;
                    pnlDocumentList.Visible = false;
                    sef = new sefcheck();
                    sef.rowid =Convert.ToInt32( grdList.Rows[e.RowIndex].Cells["RowID"].Value.ToString());
                    sef.SEFID = grdList.Rows[e.RowIndex].Cells["SEFID"].Value.ToString();
                    txtSequenceNo.Text = grdList.Rows[e.RowIndex].Cells["Seqno"].Value.ToString();
                    txtDescription.Text = grdList.Rows[e.RowIndex].Cells["Description"].Value.ToString();
                    cmbSEFstatus.SelectedIndex = cmbSEFstatus.FindStringExact(grdList.Rows[e.RowIndex].Cells["empStatus"].Value.ToString());
                    cmbSEFType.Enabled = false;
                    disableBottomButtons();
                }
            }
            catch (Exception ex)
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

        private void pnlDocumentInner_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbSEFType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sefid=((Structures.ComboBoxItem)cmbSEFType.SelectedItem).HiddenValue;
            ListEmpFinLimit(sefid);
            pnlDocumentList.Visible = true;
        }

        private void SEFCheckList_Enter(object sender, EventArgs e)
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

