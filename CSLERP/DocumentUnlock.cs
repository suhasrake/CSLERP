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
    public partial class DocumentUnlock : System.Windows.Forms.Form
    {
        string selectedDocName = "";
        string selectedDocID = "";
        System.Windows.Forms.Button prevbtn = null;
        Dictionary<string, string> dict = new Dictionary<string, string>();
        DocUnlock prevdc = new DocUnlock();
        string unlockCommiteelist = "";
        public DocumentUnlock()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }

        private void DocumentUnlock_Load(object sender, EventArgs e)
        {
            //////this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;
            dict = new Dictionary<string, string>();
            String a = this.Size.ToString();
            grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdList.EnableHeadersVisualStyles = false;
            unlockCommiteelist = DocumentUnlockDB.getUnlockCommiteeListString();
            CreateButtons();
        }
        private void CreateButtons()
        {
            try
            {
                DocumentUnlockDB dbrecord = new DocumentUnlockDB();
                List<DocUnlock> docs = dbrecord.getDocUnlockValues();
                int intex = 0;

                foreach (DocUnlock val in docs)
                {
                    dict.Add(val.DocumentID, val.TableName);
                    addButton(val.DocumentID,val.DocumentName, intex);
                    intex++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in request processing");
            }
        }
        private void addButton(string idButton, string txtButton, int index)
        {
            try
            {
                System.Windows.Forms.Button button;
                button = new System.Windows.Forms.Button();
                button.Name = idButton;
                button.Text = txtButton;
                button.Height = 24;
                button.Width = 250;
                button.BackColor = Color.FromArgb(40, 40, 40);
                button.BackColor = Color.White;
                button.ForeColor = Color.Black;
                button.Font = new Font("Lucida Console", 10);
                button.Location = new Point(5, 4 + (index * 25));
                button.Click += new EventHandler(this.MyButtonHandler);
                button.MouseHover += Button_MouseHover;
                pnlDocUnlockButtons.Controls.Add(button);
            }
            catch (Exception)
            {
                MessageBox.Show("Error in request processing");
            }
        }

        private void Button_MouseHover(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            ToolTip tip = new ToolTip();
            tip.SetToolTip(btn, btn.Text);
        }

        void MyButtonHandler(object sender, EventArgs e)
        {
            try
            {
                if (prevbtn != null)
                {
                    prevbtn.BackColor = Color.White;
                }
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                selectedDocID = btn.Name;
                selectedDocName = btn.Text;
                btn.BackColor = Color.SkyBlue;
                //btn.Size = new Size(100, 20);
                prevbtn = btn;
                ListDocValues(dict[selectedDocID]);
                applyPrivilege();
            }
            catch (Exception)
            {
                MessageBox.Show("Error in request processing");
            }
        }

        private void ListDocValues(string tablename)
        {
            try
            {
                grdList.Rows.Clear();
                DocumentUnlockDB dbrecord = new DocumentUnlockDB();
                List<DocUnlock> docValues = dbrecord.getDocumentInfo(tablename,selectedDocID);
                foreach (DocUnlock val in docValues)
                {
                    //get the employee code of user approved the document
                    ERPUserDB erpuserdb = new ERPUserDB();
                    string ecode = erpuserdb.getEmpCode(val.ApprovedUser.Trim());
                    string edet = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter2;
                    string approverList = DocEmpMappingDB.getApproverList(selectedDocID, ecode);
                    string tstr = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
                    ////users who can unlock a document
                    ////user should be found in forwarder list and logged in user is the approver and
                    ////user shall be a member of the approverlist of the document

                    if (!(val.ApprovedUser == Login.userLoggedIn || val.forwarderlist.Contains(tstr)) && 
                        approverList.Contains(edet))
                    {
                        grdList.Rows.Add(val.TemporaryNo, val.TemporaryDate, val.ApprovedUser, val.TableName, val.forwarderlist);
                    }
                    else if (unlockCommiteelist != null && unlockCommiteelist.Contains(";"+Login.userLoggedIn+";"))
                    {
                        grdList.Rows.Add(val.TemporaryNo, val.TemporaryDate, val.ApprovedUser, val.TableName, val.forwarderlist);
                    }
                    ////if (!val.ApprovedUser.Equals(Login.empLoggedIn) || !val.forwarderlist.Contains(Login.empLoggedIn))
                    ////{
                    ////    grdList.Rows.Add(val.TemporaryNo, val.TemporaryDate, val.ApprovedUser, val.TableName, val.forwarderlist);
                    ////}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
            pnlDocUnlockList.Visible = true;
        }
        private void initVariables()
        {
        }
        private void applyPrivilege()
        {
            try
            {
                if (Main.itemPriv[1])
                {
                }
                else
                {
                }
                if (Main.itemPriv[2])
                {
                    grdList.Columns["Unlock"].Visible = true;
                }
                else
                {
                    grdList.Columns["Unlock"].Visible = false;
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
                pnlDocUnlockList.Visible = false;
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                pnlDocUnlockList.Visible = true;
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
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
        }
        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                DocumentUnlockDB dudb = new DocumentUnlockDB();
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Unlock"))
                {
                    prevdc.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["TempNo"].Value);
                    prevdc.TemporaryDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["TempDate"].Value);
                    prevdc.TableName = grdList.Rows[e.RowIndex].Cells["TableName"].Value.ToString();    
                                   
                    string frwdList = grdList.Rows[e.RowIndex].Cells["ForwarderList"].Value.ToString();
                    if (frwdList == null)
                    {
                        MessageBox.Show("Failed to retrive Forwardlist");
                        return;
                    }
                    DialogResult dialog = MessageBox.Show("Are you sure to Unlock the document ?\n\nDocument Type\t:  "+
                        selectedDocName + "\nTemporary No\t:  " + prevdc.TemporaryNo+ "\nTemprary Date\t:  " + prevdc.TemporaryDate.ToString("yyyy-MM-dd")+"\n"
                        , "Confirmation", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.No)
                    {
                        return;
                    }
                    string[] strArr = frwdList.Split(Main.delimiter2);
                    int DocStat = strArr.Count() ;
                    ////if document is MRN, do not reverse if any issue from the MRN.
                    ////if Invoice out, reverse all quantity before reversing the document
                    if (selectedDocID == "MRN")
                    {
                        if (MRNHeaderDB.checkForIssuesFromMRN(prevdc.TemporaryNo, prevdc.TemporaryDate))
                        {
                            //no issues (purchase return and Invoice) from MRN

                            MessageBox.Show("Stock has been issued from MRN. Reversal request denied");
                            return;
                        }
                        else
                        {
                            if (dudb.updateDocForUnlockingMRN(prevdc, DocStat))
                            {
                                MessageBox.Show("Document Unlocked with Updating Stock sucessfully.");
                                grdList.Rows.RemoveAt(e.RowIndex);
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Document Failed to Unlocked");
                                return;
                            }
                        }
                    }
                    else if (selectedDocID == "POPRODUCTINWARD" || selectedDocID == "POSERVICEINWARD")
                    {
                        string result = POPIHeaderDB.checkForInvoiceIssuesForPOPI(prevdc.TemporaryNo, prevdc.TemporaryDate, selectedDocID);
                        if (result == "error" || result.Length != 0)
                        {
                            if (result == "error")
                            {
                                return;
                            }
                            //Inivoice Prepared For this PO
                            string InvPrepared = "";
                            string[] invArr = result.Split(Main.delimiter1);
                            foreach (string str in invArr)
                            {
                                if (str.Length != 0)
                                {
                                    string[] invNoDate = str.Split(';');
                                    InvPrepared = InvPrepared + "\nTemporary No: " + invNoDate[0] + "    Temporary Date: " + Convert.ToDateTime(invNoDate[1]).ToString("yyyy-MM-dd");
                                }
                            }
                            MessageBox.Show("Inivoice Prepared For this PO. Reversal request denied.\n" + InvPrepared, "Invoice Prepared Details");
                            return;
                        }
                        else
                        {
                            if (dudb.updateDocForUnlocking(prevdc, DocStat, selectedDocID))
                            {
                                MessageBox.Show("PO Document Unlocked");
                                grdList.Rows.RemoveAt(e.RowIndex);
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Failed to unlock Document");
                                return;
                            }
                        }
                    }
                    else if (selectedDocID == "PRODUCTINVOICEOUT" || selectedDocID == "PRODUCTEXPORTINVOICEOUT" 
                                        || selectedDocID == "SERVICEINVOICEOUT" || selectedDocID == "SERVICEEXPORTINVOICEOUT")
                    {
                        invoiceoutheader iohTemp = new invoiceoutheader();
                        iohTemp.TemporaryNo = prevdc.TemporaryNo;
                        iohTemp.TemporaryDate = prevdc.TemporaryDate;
                        iohTemp.DocumentID = selectedDocID;
                        if (InvoiceOutHeaderDB.isInvoiceOutReceiptPreparedForInvOut(iohTemp))
                        {
                            MessageBox.Show("Receipt Adjusted against this Invoice . Not allowed to Unlock.");
                            return;
                        }
                        //return;
                        if (dudb.updateDocForUnlockingInvoiceOUT(prevdc, DocStat, selectedDocID))
                        {
                            if(selectedDocID == "PRODUCTINVOICEOUT" || selectedDocID == "PRODUCTEXPORTINVOICEOUT")
                                MessageBox.Show("Document Unlocked with Updating Stock sucessfully.");
                            else
                                MessageBox.Show("Document Unlocked sucessfully.");
                            grdList.Rows.RemoveAt(e.RowIndex);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Document Failed to Unlocked");
                            return;
                        }                   
                    }
                  
                    else if (selectedDocID == "GTN")
                    {
                        if (GTNDB.checkForIssuesOfGTNInStock(prevdc.TemporaryNo, prevdc.TemporaryDate))
                        {
                            //no issues (purchase return and Invoice) from MRN

                            MessageBox.Show("Stock has been issued from GTN. Reversal request denied");
                            return;
                        }
                        else
                        {
                            if (dudb.updateDocForUnlockingGTN(prevdc, DocStat))
                            {
                                MessageBox.Show("Document Unlocked with Updating Stock sucessfully.");
                                grdList.Rows.RemoveAt(e.RowIndex);
                                return;
                            }
                            else
                            {
                                MessageBox.Show("Document Failed to Unlocked");
                                return;
                            }
                        }
                    }
                    else if (selectedDocID == "POINVOICEIN" || selectedDocID == "WOINVOICEIN" || selectedDocID == "POGENERALINVOICEIN")
                    {
                        invoiceinheader iihTemp = new invoiceinheader();
                        iihTemp.TemporaryNo = prevdc.TemporaryNo;
                        iihTemp.TemporaryDate = prevdc.TemporaryDate;
                        iihTemp.DocumentID = selectedDocID;
                        if (InvoiceInHeaderDB.isInvoiceInPaymentPreparedForInvIN(iihTemp))
                        {
                            MessageBox.Show("Payment Adjusted against this Invoice . Not allowed to Unlock.");
                            return;
                        }
                        if (dudb.updateDocForUnlockingInvoiceIN(prevdc, DocStat, selectedDocID))
                        {
                            MessageBox.Show("Document Unlocked");
                            grdList.Rows.RemoveAt(e.RowIndex);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Document Failed to Unlocked");
                            return;
                        }
                    }
                    else if (selectedDocID == "WORKORDER")
                    {
                        workorderheader wohtemp = new workorderheader();
                        wohtemp.TemporaryNo = prevdc.TemporaryNo;
                        wohtemp.TemporaryDate = prevdc.TemporaryDate;
                        wohtemp.DocumentID = selectedDocID;
                        workorderheader woh = WorkOrderDB.getWONOAndDateOFWO(wohtemp);
                        if (WorkOrderDB.isInvoicePreparedForWO(woh.WONo, woh.WODate))
                        {
                            MessageBox.Show("invoice received against this work order . Not allowed to Unlock.");
                            return;
                        }
                        if (dudb.updateDocForUnlocking(prevdc, DocStat, selectedDocID))
                        {
                            MessageBox.Show("Document Unlocked");
                            grdList.Rows.RemoveAt(e.RowIndex);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Document Failed to Unlocked");
                            return;
                        }
                    }
                    else if (selectedDocID == "PURCHASEORDER")
                    {
                        poheader pohtemp = new poheader();
                        pohtemp.TemporaryNo = prevdc.TemporaryNo;
                        pohtemp.TemporaryDate = prevdc.TemporaryDate;
                        pohtemp.DocumentID = selectedDocID;
                        poheader poh = PurchaseOrderDB.getPONOAndDateOFPOOut(pohtemp);
                        if (PurchaseOrderDB.isMRNPreparedForPO(poh.PONo, poh.PODate))
                        {
                            MessageBox.Show("MRN received against this Purchase order . Not allowed to Unlock.");
                            return;
                        }
                        if (dudb.updateDocForUnlocking(prevdc, DocStat, selectedDocID))
                        {
                            MessageBox.Show("Document Unlocked");
                            grdList.Rows.RemoveAt(e.RowIndex);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Document Failed to Unlocked");
                            return;
                        }
                    }
                    else if (selectedDocID == "POGENERAL")
                    {
                        pogeneralheader pohtemp = new pogeneralheader();
                        pohtemp.TemporaryNo = prevdc.TemporaryNo;
                        pohtemp.TemporaryDate = prevdc.TemporaryDate;
                        pohtemp.DocumentID = selectedDocID;
                        pogeneralheader poh = PurchaseOrderGeneralDB.getPONOAndDateOFPOGen(pohtemp);
                        if (PurchaseOrderGeneralDB.isInvoicePreparedForPOGeneral(poh.PONo, poh.PODate))
                        {
                            MessageBox.Show("invoice received against this Purchase order . Not allowed to Unlock.");
                            return;
                        }
                        if (dudb.updateDocForUnlocking(prevdc, DocStat, selectedDocID))
                        {
                            MessageBox.Show("Document Unlocked");
                            grdList.Rows.RemoveAt(e.RowIndex);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Document Failed to Unlocked");
                            return;
                        }
                    }
                    else if (dudb.updateDocForUnlocking(prevdc, DocStat, selectedDocID))
                    {
                        MessageBox.Show("Document Unlocked");
                        grdList.Rows.RemoveAt(e.RowIndex);
                    }
                    else
                        MessageBox.Show("Failed to unlock Document");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception : Failed to unlock Document");
            }
        }
        private void pnlCataloueButtons_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlUI_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DocumentUnlock_Enter(object sender, EventArgs e)
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

