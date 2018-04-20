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
    public partial class SearchText : System.Windows.Forms.Form
    {

        public static string[,] documentStatusValues;
        public SearchText()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void SearchText_Load(object sender, EventArgs e)
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
            ////ListTableColumns();
            applyPrivilege();
        }
        private void ListTableColumns()
        {
            try
            {
                string query = "";
                string cType = "";
                string[] comparisonList = { "char", "varchar" };
                grdList.Rows.Clear();
                SchemaItemsDB schemaitemsdb = new SchemaItemsDB();
                List<ColumnDetail> Columns = SchemaItemsDB.getColumnDetail();
                foreach (ColumnDetail cd in Columns)
                {

                    ////cType= cd.columnType;
                    if (!comparisonList.Contains(cd.columnType))
                    {
                        continue;
                    }
                    string cvalues = SchemaItemsDB.getColumnValues(cd.tableName, cd.columnName, txtToSearch.Text.Trim());
                    ////query = "SELECT STUFF( (SELECT ';~!@#' + CONVERT(varchar(10),RowID) + '~!@#' " +
                    ////    cd.columnName + 
                    ////    " FROM " + cd.tableName +
                    ////    " where "+ cd.columnName + 
                    ////    " like '%"+txtToSearch.Text.Trim()+"%' FOR XML PATH('')),1, 1, '')";

                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["TableName"].Value = cd.tableName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ColumnName"].Value = cd.columnName;
                    grdList.Rows[grdList.RowCount - 1].Cells["ColumnType"].Value = cd.columnType;
                    grdList.Rows[grdList.RowCount - 1].Cells["ColumnValue"].Value = cvalues;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            enableBottomButtons();
            pnlSearchResult.Visible = true;
        }

        private void initVariables()
        {
            try
            {


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

                pnlSearchResult.Visible = false;
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
                pnlSearchResult.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        public void clearDocumentData()
        {
            try
            {

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
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;

                {
                    if (btnText.Equals("Update"))
                    {
                        if (docDB.updateDocument(doc))
                        {
                            MessageBox.Show("Document Status updated");
                            closeAllPanels();
                            //ListDocument();
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
                                //ListDocument();
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
                    pnlSearchResult.Visible = false;
                    DataGridViewRow row = grdList.Rows[rowID];
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ListTableColumns();
            txtToReplace.Visible = true;
            btnReplace.Visible = true;
            btnSave.Visible = false;
        }

        private void txtToSearch_TextChanged(object sender, EventArgs e)
        {
            txtToReplace.Text = "";
            txtToReplace.Visible = false;
            btnReplace.Visible = false;
            btnSave.Visible = false;
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Are you sure to replace all occurance of " +
                        txtToSearch.Text + " with " + txtToReplace.Text, "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    for (int i = 0; i < grdList.Rows.Count; i++)
                    {
                        if (grdList.Rows[i].Cells["ColumnValue"].Value.ToString().Length > 0)
                        {
                            grdList.Rows[i].Cells["ColumnValue"].Value=
                                grdList.Rows[i].Cells["ColumnValue"].Value.ToString().Replace
                                (txtToSearch.Text, txtToReplace.Text);
                        }
                    }
                    grdList.CurrentCell = grdList.Rows[0].Cells[0];
                }
            }
            catch (Exception ex)
            {

            }
            btnSave.Visible = true;
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Are you sure to save the changes in Database ",
                         "Yes", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    for (int i = 0; i < grdList.Rows.Count; i++)
                    {
                        if (grdList.Rows[i].Cells["ColumnValue"].Value.ToString().Length > 0)
                        {
                            ////grdList.Rows[i].Cells["ColumnValue"].Value =
                            ////    grdList.Rows[i].Cells["ColumnValue"].Value.ToString().Replace
                            ////    (txtToSearch.Text, txtToReplace.Text);
                            SchemaItemsDB.updateValueinDB(grdList.Rows[i].Cells["TableName"].Value.ToString(),
                                grdList.Rows[i].Cells["ColumnName"].Value.ToString(),
                                grdList.Rows[i].Cells["ColumnValue"].Value.ToString());
                        }
                    }
                    grdList.CurrentCell = grdList.Rows[0].Cells[0];
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void SearchText_Enter(object sender, EventArgs e)
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

