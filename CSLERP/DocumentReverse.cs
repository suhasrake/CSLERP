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
    public partial class DocumentReverse : System.Windows.Forms.Form
    {
        string selectedDocName = "";
        string selectedDocID = "";
        System.Windows.Forms.Button prevbtn = null;
        Dictionary<string, string> dict = new Dictionary<string, string>();
        DocReverse prevdc = new DocReverse();
        string unlockCommiteelist = "";
        public DocumentReverse()
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
            unlockCommiteelist = DocumentReverseDB.getReverseCommiteeListString();
            CreateButtons();
        }
        private void CreateButtons()
        {
            try
            {
                DocumentReverseDB dbrecord = new DocumentReverseDB();
                List<DocReverse> docs = dbrecord.getDocReverseValues();
                int intex = 0;

                foreach (DocReverse val in docs)
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
                DocumentReverseDB dbrecord = new DocumentReverseDB();
                List<DocReverse> docValues = dbrecord.getDocumentInfo(tablename,selectedDocID);
                foreach (DocReverse val in docValues)
                {
                    //get the employee code of user approved the document
                    ERPUserDB erpuserdb = new ERPUserDB();
                    string ecode = erpuserdb.getEmpCode(val.ForwardUser.Trim());
                    string edet = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter2;
                    string approverList = DocEmpMappingDB.getApproverList(selectedDocID, ecode);
                    string tstr = Login.userLoggedInName + Main.delimiter1 + Login.userLoggedIn + Main.delimiter1 + Main.delimiter2;
                    ////users who can unlock a document
                    ////user should be found in forwarder list and logged in user is the approver and
                    ////user shall be a member of the approverlist of the document

                    if (!(val.ForwardUser == Login.userLoggedIn || val.forwarderlist.Contains(tstr)) && 
                        approverList.Contains(edet))
                    {
                        grdList.Rows.Add(val.TemporaryNo, val.TemporaryDate, val.ForwardUser, val.TableName, val.forwarderlist,val.RowID);
                    }
                    else if (unlockCommiteelist != null && unlockCommiteelist.Contains(";"+Login.userLoggedIn+";"))
                    {
                        grdList.Rows.Add(val.TemporaryNo, val.TemporaryDate, val.ForwardUser, val.TableName, val.forwarderlist,val.RowID);
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
                    grdList.Columns["Reverse"].Visible = true;
                }
                else
                {
                    grdList.Columns["Reverse"].Visible = false;
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
        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                DocumentReverseDB dudb = new DocumentReverseDB();
        
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                if (columnName.Equals("Reverse"))
                {
                    prevdc.RowID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["RowID"].Value);
                    prevdc.TemporaryNo = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["TempNo"].Value);
                    prevdc.TemporaryDate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells["TempDate"].Value);
                    prevdc.TableName = grdList.Rows[e.RowIndex].Cells["TableName"].Value.ToString();
                    string frwdList = grdList.Rows[e.RowIndex].Cells["ForwarderList"].Value.ToString();
                    if (frwdList == null)
                    {
                        MessageBox.Show("Failed to retrive Forwardlist");
                        return;
                    }
                    DialogResult dialog = MessageBox.Show("Are you sure to Reverse the document ?\n\nDocument Type\t:  " +
                        selectedDocName + "\nTemporary No\t:  " + prevdc.TemporaryNo + "\nTemprary Date\t:  " + prevdc.TemporaryDate.ToString("yyyy-MM-dd") + "\n"
                        , "Confirmation", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.No)
                    {
                        return;
                    }

                    string Forwader = "";
                    string[] strArr = frwdList.Split(Main.delimiter2);
                    strArr = strArr.Take(strArr.Count() - 1).ToArray();                  
                    strArr = strArr.Take(strArr.Count() - 1).ToArray();                    
                    int DocStat = strArr.Count();                    
                    if (DocStat > 0)
                    {
                        string[] str = strArr[DocStat - 1].Split(Main.delimiter1);
                        Forwader = str[1];
                    }
                    string FWDList = "";
                foreach(var itm in strArr)
                    {
                        FWDList += itm + Main.delimiter2;
                    }
                    prevdc.ForwardUser = Forwader;
                    prevdc.forwarderlist = FWDList;
                    if (dudb.updateDocForReverse(prevdc))
                    {
                        MessageBox.Show("Document Reversed sucessfully.");
                        grdList.Rows.RemoveAt(e.RowIndex);
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Document Failed to Reverse");
                        return;
                    }

                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception : Failed to Reverse Document");
            }
        }
        private void pnlCataloueButtons_Paint(object sender, PaintEventArgs e)
        {

        }
        private void pnlUI_Paint(object sender, PaintEventArgs e)
        {

        }

        private void DocumentReverse_Enter(object sender, EventArgs e)
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

