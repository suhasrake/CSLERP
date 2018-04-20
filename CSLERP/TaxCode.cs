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
    public partial class TaxCode : System.Windows.Forms.Form
    {
        private string[] ArithmetcOperators = { "Add", "Subtract", "Multiply", "Divide", "Percentage" };
        public TaxCode()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void TaxCode_Load(object sender, EventArgs e)
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
            ListTaxCode();
            applyPrivilege();
        }
        private void ListTaxCode()
        {
            try
            {
                grdList.Rows.Clear();
                TaxCodeDB taxcodedb = new TaxCodeDB();
                List<taxcode> TaxCodes = taxcodedb.getTaxCode();
                foreach (taxcode tc in TaxCodes)
                {
                    grdList.Rows.Add(tc.TaxCode, tc.Description,
                         ComboFIll.getStatusString(tc.status));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error in Tax Code listing");
            }
            enableBottomButtons();
            pnlList.Visible = true;
        }

        private void initVariables()
        {
            
            fillStatusCombo(cmbStatus);
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
                fillStatusCombo(cmbStatus);
                pnlList.Visible = false;
                pnlOuter.Visible = false;
                pnlTaxCodeWorking.Visible = false;
            }
            catch (Exception)
            {

            }
        }


        private void fillStatusCombo(System.Windows.Forms.ComboBox cmb)
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


        public void clearData()
        {
            try
            {
                txtID.Text = "";
                txtDescription.Text = "";
                cmbStatus.SelectedIndex = 0;
                txtID.Enabled = true;
                grdTaxCodeWorking.Rows.Clear();
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
                //closeAllPanels();
                clearData();
                btnSave.Text = "Save";
                pnlBottomButtons.Visible = false;
                grdTaxCodeWorking.Rows.Clear();
                pnlTaxCodeWorking.Visible = true;
                AddFirstTaxWorkingRow();
                disableBottomButtons();
            }
            catch (Exception)
            {

            }
        }
        private Boolean AddFirstTaxWorkingRow()
        {
            Boolean status = true;
            try
            {
                grdTaxCodeWorking.Rows.Add();
                int kount = grdTaxCodeWorking.RowCount;
                grdTaxCodeWorking.Rows[kount - 1].Cells[0].Value = kount;
                grdTaxCodeWorking.Rows[kount - 1].Cells[1].Value = "Product Value";
                grdTaxCodeWorking.Rows[kount - 1].Cells[6].Value = 100000;
                for (int i = 1; i < grdTaxCodeWorking.ColumnCount; i++)
                {
                    grdTaxCodeWorking.Rows[kount - 1].Cells[i].ReadOnly = true;
                }
            }
            catch (Exception)
            {
                status = false;
            }

            return status;
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        //private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                taxcode tc = new taxcode();
                TaxCodeDB taxcodedb = new TaxCodeDB();
                tc.TaxCode = txtID.Text;
                tc.Description = txtDescription.Text;
                tc.status = ComboFIll.getStatusCode(cmbStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;
                if (taxcodedb.validateTaxCode(tc))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (taxcodedb.updateTaxCode(tc))
                        {
                            createAndUpdateTaxCodeWorking();
                            MessageBox.Show("Tax Code updated");
                            closeAllPanels();
                            ListTaxCode();
                            pnlBottomButtons.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("Failed to update tax Code");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (taxcodedb.insertTaxCode(tc))
                        {
                            createAndUpdateTaxCodeWorking();
                            MessageBox.Show("Tax Code Added");
                            closeAllPanels();
                            ListTaxCode();
                            pnlBottomButtons.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Tax Code");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Tax Code Data Validation failed");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing Tax Code");
            }

        }
        private void createAndUpdateTaxCodeWorking()
        {
            try
            {
                TaxCodeWorkingDB tcwdb = new TaxCodeWorkingDB();
                taxcodeworking tcw = new taxcodeworking();

                List<taxcodeworking> tcwDetails = new List<taxcodeworking>();
                for (int i = 0; i < grdTaxCodeWorking.Rows.Count; i++)
                {
                    try
                    {
                        tcw = new taxcodeworking();
                        tcw.TaxCode = txtID.Text;
                        try
                        {
                            tcw.LineNo = Convert.ToInt32(grdTaxCodeWorking.Rows[i].Cells[0].Value.ToString());
                        }
                        catch (Exception)
                        {
                            tcw.LineNo = -1;
                        }
                        try
                        {
                            tcw.Description = grdTaxCodeWorking.Rows[i].Cells[1].Value.ToString();
                        }
                        catch (Exception)
                        {
                            tcw.Description = null;
                        }
                        try
                        {
                            tcw.Operator = grdTaxCodeWorking.Rows[i].Cells[2].Value.ToString();
                        }
                        catch (Exception)
                        {
                            tcw.Operator = null;
                        }
                        try
                        {
                            tcw.OperandLine1 = Convert.ToInt32(grdTaxCodeWorking.Rows[i].Cells[3].Value.ToString());
                        }
                        catch (Exception)
                        {
                            tcw.OperandLine1 = 0;
                        }
                        try
                        {
                            tcw.OperandLine2 = Convert.ToInt32(grdTaxCodeWorking.Rows[i].Cells[4].Value.ToString());
                        }
                        catch (Exception)
                        {
                            tcw.OperandLine2 = 0;
                        }
                        try
                        {
                            tcw.OperatorValue = Convert.ToDouble(grdTaxCodeWorking.Rows[i].Cells[5].Value.ToString());
                        }
                        catch (Exception)
                        {
                            tcw.OperatorValue = 0;
                        }
                        try
                        {
                            tcw.Amount = Convert.ToDouble(grdTaxCodeWorking.Rows[i].Cells[6].Value.ToString());
                        }
                        catch (Exception)
                        {
                            tcw.Amount = 0;
                        }
                        try
                        {
                            tcw.TaxItemName = grdTaxCodeWorking.Rows[i].Cells[7].Value.ToString();
                        }
                        catch (Exception)
                        {
                            tcw.TaxItemName = null;
                        }
                        tcw.status = 1;
                        tcwDetails.Add(tcw);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("createAndUpdateTaxCodeWorking() : Error creating Tax Code Working");
                        return;
                    }
                }
                /////*TaxCodeWorkingDB*/ customerbankdetailsdb = new CustomerBankDetailsDB();
                if (!tcwdb.updateTaxCodeWorkings(txtID.Text, tcwDetails))
                {
                    MessageBox.Show("createAndUpdateTaxCodeWorking() : Failed to update Tax Code Working. Please check the values");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("createAndUpdateTaxCodeWorking() : Error updating Tax Code Workingt");
            }
        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == 3)
                {
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    txtID.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtDescription.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    cmbStatus.SelectedIndex = cmbStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[2].Value.ToString());
                    txtID.Enabled = false;
                    DataGridViewRow row = grdList.Rows[rowID];
                    //get customer bank details
                    TaxCodeWorkingDB tcwdb = new TaxCodeWorkingDB();
                    List<taxcodeworking> tcwDetails = tcwdb.getTaxCodeWorkings(txtID.Text);
                    grdTaxCodeWorking.Rows.Clear();
                    int i = 0;
                    foreach (taxcodeworking tcw in tcwDetails)
                    {
                        if (i == 0)
                        {
                            AddFirstTaxWorkingRow();
                            grdTaxCodeWorking.Rows[i].Cells[6].Value = tcw.Amount;
                            //break;
                        }
                        else
                        {
                            AddTaxWorkingRow();
                            grdTaxCodeWorking.Rows[i].Cells[0].Value = tcw.LineNo;
                            grdTaxCodeWorking.Rows[i].Cells[1].Value = tcw.Description;

                            //DataGridViewComboBoxCell ComboColumn1 = new DataGridViewComboBoxCell();
                            //ComboColumn1.FindStringExact(Main.getStatusString(cust.status));

                            grdTaxCodeWorking.Rows[i].Cells[2].Value = tcw.Operator;
                            if (tcw.OperandLine1 != 0)
                            {
                                grdTaxCodeWorking.Rows[i].Cells[3].Value = tcw.OperandLine1.ToString();
                            }
                            else
                            {
                                grdTaxCodeWorking.Rows[i].Cells[3].Value = null;
                            }
                            if (tcw.OperandLine2 != 0)
                            {
                                grdTaxCodeWorking.Rows[i].Cells[4].Value = tcw.OperandLine2.ToString();
                            }
                            else
                            {
                                grdTaxCodeWorking.Rows[i].Cells[4].Value = null;
                            }
                            if (tcw.OperatorValue != 0)
                            {
                                grdTaxCodeWorking.Rows[i].Cells[5].Value = tcw.OperatorValue;
                            }
                            else
                            {
                                grdTaxCodeWorking.Rows[i].Cells[5].Value = null;
                            }
                                
                            grdTaxCodeWorking.Rows[i].Cells[6].Value = tcw.Amount;
                            grdTaxCodeWorking.Rows[i].Cells[7].Value = tcw.TaxItemName;
                        }
                        i++;
                    }
                    btnSave.Text = "Update";
                    pnlTaxCodeWorking.Visible = true;
                    pnlBottomButtons.Visible = false;
                    disableBottomButtons();
                }
                //////////TaxCodeWorkingDB tcwdb1 = new TaxCodeWorkingDB();
                //////////tcwdb1.calculateTax(txtID.Text, 25000);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
        }

        private void btnAddLine_Click(object sender, EventArgs e)
        {
            try
            {
                AddTaxWorkingRow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
        }

        private Boolean AddTaxWorkingRow()
        {
            Boolean status = true;
            try
            {
                if (grdTaxCodeWorking.Rows.Count > 1)
                {
                    verifyAndReworkTaxGridRows();
                }
                grdTaxCodeWorking.Rows.Add();
                int kount = grdTaxCodeWorking.RowCount;
                grdTaxCodeWorking.Rows[kount - 1].Cells[0].Value = kount;
                DataGridViewComboBoxCell ComboColumn1 = (DataGridViewComboBoxCell)(grdTaxCodeWorking.Rows[kount - 1].Cells[2]);
                string firstValue1 = fillArithmetcGridViewCombo(ComboColumn1);
                ComboColumn1.DropDownWidth = 200;
                DataGridViewComboBoxCell ComboColumn2 = (DataGridViewComboBoxCell)(grdTaxCodeWorking.Rows[kount - 1].Cells[7]);
                TaxItemDB.fillTaxItemGridViewCombo(ComboColumn2);
                ComboColumn2.Items.Add("Dummy");
                ComboColumn2.DropDownWidth = 200;
                DataGridViewComboBoxCell ComboColumn3 = new DataGridViewComboBoxCell();
                DataGridViewComboBoxCell ComboColumn4 = new DataGridViewComboBoxCell();

                for (int i = 1; i < kount; i++)
                {
                    ComboColumn3.Items.Add(i.ToString());
                    ComboColumn4.Items.Add(i.ToString());
                }
                grdTaxCodeWorking.Rows[kount - 1].Cells[3] = ComboColumn3;
                grdTaxCodeWorking.Rows[kount - 1].Cells[4] = ComboColumn4;
                //grdCustomerBank.Rows[kount-1].Cells[0].Value = firstValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddTaxWorkingRow() : Error");
            }

            return status;
        }
        private string fillArithmetcGridViewCombo(DataGridViewComboBoxCell cmb)
        {
            string firstValue = "";
            cmb.Items.Clear();
            try
            {

                for (int i = 0; i < ArithmetcOperators.Length; i++)
                {
                    cmb.Items.Add(ArithmetcOperators[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            return firstValue;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ////pnlTaxCodeWorking.Visible = false;
            closeAllPanels();
            btnNew.Visible = true;
            btnExit.Visible = true;
            pnlList.Visible = true;
            enableBottomButtons();
            pnlBottomButtons.Visible = true;
        }

        private void grdTaxCodeWorking_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex == 0)
                {
                    MessageBox.Show("Cannot delete this row");
                    return;
                }
                if (e.ColumnIndex == 3 || e.ColumnIndex == 4)
                {
                    //no action
                    return;
                }
                if (e.ColumnIndex == 8)
                {
                    try
                    {
                        //delete row
                        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                        if (dialog == DialogResult.Yes)
                        {
                            ////grdTaxCodeWorking.Rows.RemoveAt(e.RowIndex);
                        }
                        reArrangeGridValues();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void reArrangeGridValues()
        {
            for (int i = 1; i < grdTaxCodeWorking.Rows.Count; i++)
            {
                grdTaxCodeWorking[0, i].Value = (i + 1);
                ////DataGridViewComboBoxCell ComboColumn1 = (DataGridViewComboBoxCell)(grdTaxCodeWorking.Rows[i].Cells[2]);
                grdTaxCodeWorking.Rows[i].Cells[2].Value = null;
                grdTaxCodeWorking.Rows[i].Cells[3].Value = null;
                grdTaxCodeWorking.Rows[i].Cells[4].Value = null;
                grdTaxCodeWorking.Rows[i].Cells[7].Value = null;
            }
        }

        private Boolean verifyAndReworkTaxGridRows()
        {
            Boolean status = true;
            int no = 1;
            try
            {
                int operandLin1Value = 0;
                int operandLin2Value = 0;
                double operandLine1Amount = 0.0;
                double operandLine2Amount = 0.0;
                double operatorAmount = 0.0;
                if (grdTaxCodeWorking.Rows.Count <= 1)
                {
                    MessageBox.Show("No entries in table");
                    return false;
                }
               
                for (int i = 1; i < grdTaxCodeWorking.Rows.Count; i++)
                {
                    no = i;
                    if ((grdTaxCodeWorking.Rows[i].Cells[3].Value != null) &&
                        (grdTaxCodeWorking.Rows[i].Cells[4].Value != null) &&
                        (grdTaxCodeWorking.Rows[i].Cells[5].Value != null) &&
                        (Convert.ToDouble(grdTaxCodeWorking.Rows[i].Cells[5].Value.ToString()) != 0))
                    {
                        MessageBox.Show("Value Error in row " + (i + 1));
                        return false;
                    }
                    if ((grdTaxCodeWorking.Rows[i].Cells[2].Value == null) ||
                        (grdTaxCodeWorking.Rows[i].Cells[3].Value == null) ||
                        (grdTaxCodeWorking.Rows[i].Cells[7].Value == null))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }

                    if ((grdTaxCodeWorking.Rows[i].Cells[2].Value.ToString().Trim().Length == 0) ||
                        (grdTaxCodeWorking.Rows[i].Cells[3].Value.ToString().Trim().Length == 0) ||
                    (grdTaxCodeWorking.Rows[i].Cells[7].Value.ToString().Trim().Length == 0))
                    {
                        MessageBox.Show("Fill values in row " + (i + 1));
                        return false;
                    }

                    if (grdTaxCodeWorking.Rows[i].Cells[4].Value == null &&
                        grdTaxCodeWorking.Rows[i].Cells[5].Value == null)
                    {
                        MessageBox.Show("Value should be filled in Operand Line2 or Operator value in row " + (i + 1));
                        return false;
                    }
                    operandLin1Value = Convert.ToInt32(grdTaxCodeWorking.Rows[i].Cells[3].Value.ToString());
                    operandLine1Amount = Convert.ToDouble(grdTaxCodeWorking.Rows[operandLin1Value - 1].Cells[6].Value.ToString());

                    //if (grdTaxCodeWorking.Rows[i].Cells[2].Value.ToString().Trim() == "Add" ||
                    //    grdTaxCodeWorking.Rows[i].Cells[2].Value.ToString().Trim() == "Divide" ||
                    //    grdTaxCodeWorking.Rows[i].Cells[2].Value.ToString().Trim() == "Multiply" ||
                    //    grdTaxCodeWorking.Rows[i].Cells[2].Value.ToString().Trim() == "Subtract")
                    {
                        //line 2 or operator value should be filled

                        if (grdTaxCodeWorking.Rows[i].Cells[4].Value != null)
                        {
                            operandLin2Value = Convert.ToInt32(grdTaxCodeWorking.Rows[i].Cells[4].Value.ToString());
                            operandLine2Amount = Convert.ToDouble(grdTaxCodeWorking.Rows[operandLin2Value - 1].Cells[6].Value.ToString());
                        }
                        if (grdTaxCodeWorking.Rows[i].Cells[5].Value != null &&
                            Convert.ToDouble(grdTaxCodeWorking.Rows[i].Cells[5].Value.ToString()) != 0)
                        {
                            operandLine2Amount = Convert.ToDouble(grdTaxCodeWorking.Rows[i].Cells[5].Value.ToString());
                        }
                        if (grdTaxCodeWorking.Rows[i].Cells[2].Value.ToString().Trim() == "Add")
                        {
                            grdTaxCodeWorking.Rows[i].Cells[6].Value = Math.Round(operandLine1Amount + operandLine2Amount,2);
                        }
                        if (grdTaxCodeWorking.Rows[i].Cells[2].Value.ToString().Trim() == "Subtract")
                        {
                            grdTaxCodeWorking.Rows[i].Cells[6].Value = Math.Round(operandLine1Amount - operandLine2Amount,2);
                        }
                        if (grdTaxCodeWorking.Rows[i].Cells[2].Value.ToString().Trim() == "Multiply")
                        {
                            grdTaxCodeWorking.Rows[i].Cells[6].Value = Math.Round(operandLine1Amount * operandLine2Amount,2);
                        }
                        if (grdTaxCodeWorking.Rows[i].Cells[2].Value.ToString().Trim() == "Divide")
                        {
                            grdTaxCodeWorking.Rows[i].Cells[6].Value = Math.Round(operandLine1Amount / operandLine2Amount,2);
                        }
                        if (grdTaxCodeWorking.Rows[i].Cells[2].Value.ToString().Trim() == "Percentage")
                        {
                            grdTaxCodeWorking.Rows[i].Cells[6].Value = Math.Round((operandLine1Amount * operandLine2Amount) / 100,2);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:Check Grid Row "+(no + 1));
                return false;
            }
            return status;
        }

        private void btnCalculateax_Click(object sender, EventArgs e)
        {
            verifyAndReworkTaxGridRows();
        }

        private void btnClearEntries_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = grdTaxCodeWorking.Rows.Count - 1; i > 0; i--)
                {
                    grdTaxCodeWorking.Rows.RemoveAt(i);
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (verifyAndReworkTaxGridRows())
            {
                pnlOuter.Visible = true;
                pnlOuter.BringToFront();
            }
        }

        private void btnIgnore_Click(object sender, EventArgs e)
        {
            pnlOuter.Visible = false;
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

        private void TaxCode_Enter(object sender, EventArgs e)
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

