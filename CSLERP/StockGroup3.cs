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

namespace CSLERP
{
    public partial class StockGroup3 : System.Windows.Forms.Form
    {
        string docID = "STOCKGROUP";
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        stockgroup3 prevsg = new stockgroup3();
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        static int lvl = 0;
        int no;
        public StockGroup3()
        {
            try
            {
                InitializeComponent();
                this.FormBorderStyle = FormBorderStyle.None;
                Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
                initVariables();
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                this.Width -= 100;
                this.Height -= 100;
                this.FormBorderStyle = FormBorderStyle.Fixed3D;
                String a = this.Size.ToString();
                grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
                grdList.EnableHeadersVisualStyles = false;
                ShowPannel();
                applyPrivilege();
                btnNew.Visible = false;
            }
            catch (Exception)
            {

            }
        }
        private void ShowPannel()
        {
            pnlList.Visible = true;
            lvlSelect.Visible = true;
            cmbSelectLevel.Visible = true;
        }
        private void listStockGroup(int lvl)
        {
            try
            {
                grdList.Rows.Clear();
                StockGroupDB3 sgdb = new StockGroupDB3();
                List<stockgroup3> sgroup = sgdb.getStockGroupDetails(lvl);
                int i = 1;
                foreach (stockgroup3 sg in sgroup)
                {
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["LineNo"].Value = i;
                    grdList.Rows[grdList.RowCount - 1].Cells["GroupCode"].Value = sg.GroupCode;
                    grdList.Rows[grdList.RowCount - 1].Cells["GroupDescription"].Value = sg.GroupDescription;
                    grdList.Rows[grdList.RowCount - 1].Cells["CreateTime"].Value = sg.CreateTime;
                    grdList.Rows[grdList.RowCount - 1].Cells["Creator"].Value = sg.CreateUser;
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Stock Group  Listing");
            }
            try
            {
                enableBottomButtons();
                pnlList.Visible = true;
                btnNew.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void initVariables()
        {
            
            docID = Main.currentDocument;

            pnlUI.Controls.Add(pnlList);
            cmbSelectLevel.Items.Add("1");
            cmbSelectLevel.Items.Add("2");
            cmbSelectLevel.Items.Add("3");
            enableBottomButtons();
            btnNew.Visible = false;
            btnAddNew.Visible = false;
            pnlAddNew.Visible = false;
            grdList.Visible = true;
            grdList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }
        private void cmbSelectLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lvl = Convert.ToInt32(cmbSelectLevel.SelectedItem.ToString());
                if (lvl != 0)
                {
                    no = lvl;
                    grdList.Visible = true;
                    listStockGroup(lvl);
                }
                btnAddNew.Visible = true;
                clearData();
                if (getuserPrivilegeStatus() == 1)
                {
                    btnAddNew.Visible = false;
                }
                else
                    btnAddNew.Visible = true;
                pnlAddNew.Visible = false;
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
        private void button1_Click(object sender, EventArgs e)
        {
            //clearData();
            closeAllPanels();
            pnlList.Visible = true;
            listStockGroup(lvl);
            //pnlBottomActions.Visible = true;
        }
        private void closeAllPanels()
        {
            try
            {
                pnlList.Visible = false;
                //pnlAddEdit.Visible = false;

            }
            catch (Exception)
            {

            }
        }

        public void clearData()
        {
            try
            {
                txtGroupCode.Text = "";
                txtGroupDescription.Text = "";
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
        private void enableBottomButtons()
        {
            btnNew.Visible = false;
            btnExit.Visible = true;
            pnlBottomButtons.Visible = true;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
        }
        public string getGroupCode()
        {
            string gc = "";

            StockGroupDB3 sdb = new StockGroupDB3();
            List<stockgroup3> LSGroup = sdb.getStockGroupDetails(lvl);
            SortedSet<string> set = new SortedSet<string>();
            try
            {
                foreach (stockgroup3 sg in LSGroup)
                {
                    set.Add(sg.GroupCode);
                }
                gc = set.Max;
            }
            catch (Exception ex)
            {
            }
            if (Convert.ToInt32(gc) == 0)
            {
                gc = "10";// group coe start with 10
            }
            return (Convert.ToInt32(gc) + 1).ToString();
        }
        private void btnAddNewLine_Click_1(object sender, EventArgs e)
        {
            try
            {
                clearData();
                pnlAddNew.Visible = true;
                string gc = new StockGroup3().getGroupCode();
                if (Convert.ToInt32(gc) > 99)
                {
                    MessageBox.Show("Group code cross the limit (GroupCode > 99)");
                    return;
                }
                else
                    txtGroupCode.Text = gc;
                btnSave.Text = "Save";
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {

        }


        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            clearData();
            pnlAddNew.Visible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Boolean status = true;
            try
            {

                StockGroupDB3 sgdb = new StockGroupDB3();
                stockgroup3 sg = new stockgroup3();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;

                try
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtGroupCode.Text, @"^[0-9]+$"))
                    {
                        MessageBox.Show("This GroupCode accepts only numeric characters");
                        return;
                    }
                    else
                        sg.GroupCode = txtGroupCode.Text;

                    if (!System.Text.RegularExpressions.Regex.IsMatch(txtGroupDescription.Text, @"^[\sa-zA-Z0-9]+$"))
                    {
                        MessageBox.Show("This GroupDescription accepts only alphanumeric characters");
                        return;
                    }
                    else
                        sg.GroupDescription = txtGroupDescription.Text;

                    sg.GroupLevel = Convert.ToInt32(cmbSelectLevel.SelectedItem.ToString().Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Validation failed");
                    return;
                }

                if (btnText.Equals("Save"))
                {
                    if (sgdb.validateStockGroup(sg))
                    {

                        if (sgdb.insertStockGroup(sg))
                        {
                            MessageBox.Show("Stock Code Added");
                            closeAllPanels();
                            listStockGroup(lvl);
                            pnlAddNew.Visible = false;
                            pnlBottomButtons.Visible = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Validation failed");
                    }
                    if (!status)
                    {
                        MessageBox.Show("Failed to Insert Stock Code");
                    }
                }
                else if (btnText.Equals("update"))
                {
                    if (sgdb.validateStockGroup(sg))
                    {
                        if (sgdb.updateStockGroup(sg))
                        {
                            MessageBox.Show("Stock Code Added");
                            closeAllPanels();
                            listStockGroup(lvl);
                            pnlAddNew.Visible = false;
                            pnlBottomButtons.Visible = true;
                        }
                        else
                        {
                            status = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Validation failed");
                    }
                    if (!status)
                    {
                        MessageBox.Show("Failed to Insert Stock Code");
                    }
                }
                else
                {
                    MessageBox.Show("btnSave error.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errorr in saving");
            }
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {

        }
        private void pnlUI_Paint(object sender, PaintEventArgs e)
        {

        }
        private void grdList_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Edit"))
                    {
                        //string btnText = btnSave.Text;
                        btnSave.Text = "update";
                        prevsg = new stockgroup3();
                        prevsg.GroupCode = grdList.Rows[e.RowIndex].Cells["GroupCode"].Value.ToString();
                        prevsg.GroupDescription = grdList.Rows[e.RowIndex].Cells["GroupDescription"].Value.ToString();
                        prevsg.GroupLevel = Convert.ToInt32(cmbSelectLevel.SelectedItem.ToString().Trim());
                        txtGroupCode.Text = prevsg.GroupCode;
                        txtGroupDescription.Text = prevsg.GroupDescription;
                        pnlAddNew.Visible = true;
                    }
                }
                catch (Exception)
                {

                }
            }
            catch (Exception ex)
            {

            }
        }
        private void QIHeader_Load(object sender, EventArgs e)
        {

        }

        private void txtCreditPeriods_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtValidityDays_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void Status_Click(object sender, EventArgs e)
        {

        }

        private void pnlList_Paint(object sender, PaintEventArgs e)
        {

        }

        private void StockGroup3_Enter(object sender, EventArgs e)
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


