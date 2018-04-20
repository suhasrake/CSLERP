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
    public partial class TCMapping : System.Windows.Forms.Form
    {
        string docID = "TCMapping";
        DocEmpMappingDB demDB = new DocEmpMappingDB();
        string did = "";
        public TCMapping()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void TCMapping_Load(object sender, EventArgs e)
        {
            //////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;

            String a = this.Size.ToString();
            //applyPrivilege();
            btnNew.Visible = false;
        }
        private void ListTermsAndConditions(string did)
        {
            try
            {
                ListViewTC.Items.Clear();
                TCmappingDB tcmdb = new TCmappingDB();
                List<tcmapping> TCMList = tcmdb.getTCMappingList(did);

                string[] str = new string[TCMList.Count];
                int i = 0;
                TermsAndConditionsDB tcdb = new TermsAndConditionsDB();
                List<termsandconditions> TCList = tcdb.getTermsAndConditions();
                foreach (tcmapping tcm in TCMList)
                {
                    str[i] = Convert.ToString( tcm.ReferenceTC);
                    string s = str[i];
                    i++;
                }
                foreach (termsandconditions tcond in TCList)
                {
                    ListViewItem item1 = new ListViewItem();
                    for (int j = 0; j < str.Length; j++)
                    {
                        if (str[j].Equals(tcond.ParagraphID.ToString()))
                        {
                            item1.Checked = true;
                            break;
                        }
                        else
                            item1.Checked = false;
                    }

                    item1.SubItems.Add(tcond.ParagraphID.ToString());
                    item1.SubItems.Add(tcond.ParagraphHeading);
                    item1.SubItems.Add(tcond.Details);
                    ListViewTC.Items.Add(item1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Terms and Condition mapping Listing");
            }
            try
            {
                enableButtons();
                pnlList.Visible = true;
                if (getuserPrivilegeStatus() == 1)
                {
                    btnSave.Visible = false;
                }
                else
                    btnSave.Visible = true;
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
        private void initVariables()
        {
            
            clearData();
            docID = Main.currentDocument;
            pnlUI.Controls.Add(pnlList);
            DocumentDB.fillDocumentIDCumbo(cmbSelectDocument);
            btnNew.Visible = false;
            disableButtons();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            clearData();
            pnlList.Visible = true;

            enableButtons();
            disableButtons();
        }
        private void disableButtons()
        {
            ListViewTC.Visible = false;
            btnSave.Visible = false;
            btnCancel.Visible = false;
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
                ListViewTC.Items.Clear();
                cmbSelectDocument.SelectedIndex = -1;
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
        private void enableButtons()
        {
            btnExit.Visible = true;
            pnlBottomButtons.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            ListViewTC.Visible = true;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
        }
        private void cmbSelectDocument_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            did = cmbSelectDocument.SelectedItem.ToString();
            ListTermsAndConditions(did);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            closeAllPanels();
            pnlList.Visible = true;
            enableButtons();
            ListViewTC.Visible = false;
            pnlBottomButtons.Visible = true;
        }
        private Boolean createAndUpdateTermsAndConditions(tcmapping tcm)
        {
            Boolean status = true;
            try
            {
                TCmappingDB tcdb = new TCmappingDB();
                tcmapping tc = new tcmapping();
                int count = 0;
                List<tcmapping> TCMDetails = new List<tcmapping>();
                foreach (ListViewItem itemRow in ListViewTC.Items)
                {
                    try
                    {   
                        if (itemRow.Checked)
                        {
                            tc = new tcmapping();
                            tc.DocumentID = cmbSelectDocument.SelectedItem.ToString();
                            tc.ReferenceTC = Convert.ToInt32( itemRow.SubItems[1].Text);
                            TCMDetails.Add(tc);
                            count++;
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("createAndUpdateTermsAndConditions() : Error creating Terms and Conditions mapping");
                        status = false;
                    }
                }
                if (count == 0)
                {
                    MessageBox.Show("No item selected");
                    status = false;
                }
                else
                {
                    if (!tcdb.UpdateTCMapping(TCMDetails, tc))
                    {
                        MessageBox.Show("createAndUpdateTermsAndConditions() : Failed to update Terms and Conditions mapping. Please check the values");
                        status = false;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("createAndUpdateTermsAndConditions() : Error updating Terms and Conditions.");
                status = false;
            }
            return status;
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            
            Boolean status = true;
            try
            {
                tcmapping tc = new tcmapping();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btnSave.Text;
                if (btnText.Equals("Save"))
                {
                    if (createAndUpdateTermsAndConditions(tc))
                    {
                        MessageBox.Show("Terms and Conditions updated");
                        closeAllPanels();
                        pnlList.Visible = true;
                        
                        clearData();
                        disableButtons();
                        //ListTermsAndConditions(did);
                    }
                    else
                    {
                        status = false;
                    }

                }
                if (!status)
                {
                    MessageBox.Show("Failed to update Terms and condition mappiing");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("createAndUpdateTermsAndConditions() : Error");
            }
        }

        private void TCMapping_Enter(object sender, EventArgs e)
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

