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
    public partial class MenuItem : System.Windows.Forms.Form
    {
        public static string[,] menuitemTypeValues;
        public MenuItem()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void MenuItem_Load(object sender, EventArgs e)
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
            ListMenuItems();
            applyPrivilege();
        }
        ////private void Form1_Load(object sender, EventArgs e)
        ////{
        ////    ListMenuItems();

        ////}
        private void ListMenuItems()
        {
            try
            {
                grdList.Rows.Clear();
                MenuItemDB dbrecord = new MenuItemDB();
                List<menuitem> menuitems = dbrecord.getMenuItems();
                foreach (menuitem menu in menuitems)
                {
                    grdList.Rows.Add(menu.menuItemID, menu.description, menu.shortDescription,
                         dbrecord.getMenuItemTypeString(menu.menuitemType),
                         menu.documentID + "-" + menu.documentName,
                         menu.pageLink,menu.versionrequired,
                         menu.menugrp,
                         ComboFIll.getStatusString(menu.menuitemStatus));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            enableBottomButtons();
            pnlMenuList.Visible = true;
        }

        private void initVariables()
        {
            try
            {
                
                menuitemTypeValues = new string[1, 2]
                      {
                    {"1","Normal" }
                      };
                fillMenuItemStatusCombo(cmbMenuItemStatus);
                fillMenuItemTypeCombo(cmbMenuItemType);
                MenuItemDB.fillMenuItemComboNew(cmbDocument);
                MenuItemDB.fillMenuGroupCombo(cmbMenuGroup);
                txtDocumentName.Visible = false;
                cmbDocument.Visible = false;
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
                pnlMenuItemInner.Visible = false;
                pnlMenuItemOuter.Visible = false;
                pnlMenuList.Visible = false;
            }
            catch (Exception)
            {

            }
        }


        private void fillMenuItemStatusCombo(System.Windows.Forms.ComboBox cmb)
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
        private void fillMenuItemTypeCombo(System.Windows.Forms.ComboBox cmb)
        {
            try
            {
                cmb.Items.Clear();
                for (int i = 0; i < menuitemTypeValues.GetLength(0); i++)
                {
                    cmb.Items.Add(menuitemTypeValues[i, 1]);
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
                enableBottomButtons();
                pnlMenuList.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        public void clearMenuItemData()
        {
            try
            {
                txtMenuItemID.Text = "";
                txtMenuItemDescription.Text = "";
                txtMenuItemShortDescription.Text = "";
                cmbMenuItemStatus.SelectedIndex = 0;
                cmbMenuItemType.SelectedIndex = 0;
                txtMenuItemUIName.Text = "";
                cmbDocument.SelectedIndex = 0;
            }
            catch (Exception)
            {

            }
        }

        private void btnMenuItemListExit_Click(object sender, EventArgs e)
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
                clearMenuItemData();
                btnMenuItemSave.Text = "Save";
                pnlMenuItemOuter.Visible = true;
                pnlMenuItemInner.Visible = true;
                txtDocumentName.Visible = false;
                txtMenuItemID.Enabled = true;
                cmbDocument.Visible = true;
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
                menuitem menu = new menuitem();
                MenuItemDB menuDB = new MenuItemDB();
                string docName = "";
                string docID = "";
                try
                {
                    ////////docID = cmbDocument.SelectedItem.ToString().Trim().Substring(0, cmbDocument.SelectedItem.ToString().Trim().IndexOf('-'));
                    docID = ((Structures.ComboBoxItem)cmbDocument.SelectedItem).HiddenValue;
                    ////////docName = cmbDocument.SelectedItem.ToString().Trim().Substring(cmbDocument.SelectedItem.ToString().Trim().IndexOf('-') + 1);
                    docName = ((Structures.ComboBoxItem)cmbDocument.SelectedItem).ToString();
                }
                catch (Exception)
                {

                    docName = "";
                    docID = "";
                }
                menu.menuItemID = txtMenuItemID.Text;
                menu.description = txtMenuItemDescription.Text;
                menu.shortDescription = txtMenuItemShortDescription.Text;
                menu.menuitemType = menuDB.getMenuItemTypeCode(cmbMenuItemType.SelectedItem.ToString());
                menu.documentID = docID;
                menu.documentName = docName;
                menu.pageLink = txtMenuItemUIName.Text;
                menu.menuitemStatus = ComboFIll.getStatusCode(cmbMenuItemStatus.SelectedItem.ToString());
                menu.menugrp = cmbMenuGroup.SelectedItem.ToString();
                menu.versionrequired = txtVersionRequired.Text.Trim();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                {
                    if (btnText.Equals("Update"))
                    {
                        if (menuDB.updateMenuItem(menu))
                        {
                            MessageBox.Show("Menu Item updated");
                            closeAllPanels();
                            ListMenuItems();
                        }
                        else
                        {
                            MessageBox.Show("Failed to update Mnu Item");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (menuDB.validateMenuItem(menu))
                        {
                            if (menuDB.insertMenuItem(menu))
                            {
                                MessageBox.Show("Menu Item data Added");
                                closeAllPanels();
                                ListMenuItems();
                            }
                            else
                            {
                                MessageBox.Show("Failed to Insert Menu Item Data");
                            }
                        }
                        else
                        {
                            MessageBox.Show("enu Item Data Validation failed");
                        }
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Failed Adding / Editing Menu Item Data");
            }

        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == 9)
                {
                    int rowID = e.RowIndex;
                    btnMenuItemSave.Text = "Update";
                    pnlMenuItemInner.Visible = true;
                    pnlMenuItemOuter.Visible = true;
                    pnlMenuList.Visible = false;
                    txtMenuItemID.Enabled = false;
                    txtDocumentName.Enabled = false;
                    cmbDocument.Visible = true;
                    txtMenuItemID.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtMenuItemDescription.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    txtMenuItemShortDescription.Text = grdList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    cmbMenuItemType.SelectedIndex = cmbMenuItemType.FindStringExact(grdList.Rows[e.RowIndex].Cells[3].Value.ToString());
                    txtDocumentName.Text = grdList.Rows[e.RowIndex].Cells[4].Value.ToString();
                    ////////cmbDocument.SelectedIndex = cmbDocument.FindStringExact(grdList.Rows[e.RowIndex].Cells[4].Value.ToString());
                    cmbDocument.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbDocument, 
                        grdList.Rows[e.RowIndex].Cells[4].Value.ToString().Substring
                        (0,grdList.Rows[e.RowIndex].Cells[4].Value.ToString().IndexOf('-')));
                    txtMenuItemUIName.Text = grdList.Rows[e.RowIndex].Cells[5].Value.ToString();
                    txtVersionRequired.Text = grdList.Rows[e.RowIndex].Cells[6].Value.ToString();
                    cmbMenuItemStatus.SelectedIndex = cmbMenuItemStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[8].Value.ToString());
                    cmbMenuGroup.SelectedIndex = cmbMenuGroup.FindStringExact(grdList.Rows[e.RowIndex].Cells[7].Value.ToString());
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


    }
}

