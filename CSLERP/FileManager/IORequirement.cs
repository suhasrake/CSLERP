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
using CSLERP.FileManager;

namespace CSLERP.FileManager
{
    public partial class IORequirement : System.Windows.Forms.Form
    {
        Form frmPopup = new Form();
        int descClickRowIndex = -1;
        RichTextBox txtDesc = new RichTextBox();
        string sefid = "";
        int option = 0;
        Boolean isAllFilled = false;
        List<iorequirements> reqList = new List<iorequirements>();

        public List<iorequirements> ReturnValue1 { get; set; }

        public IORequirement()
        {
            try
            {
                InitializeComponent();
               // this.StartPosition = FormStartPosition.CenterScreen;
            }
            catch (Exception)
            {

            }
        }
        public IORequirement(List<iorequirements> ioreqList,string sefID)
        {
            InitializeComponent();
            reqList = ioreqList;
            sefid = sefID;
        }
        private void TapalSummary_Load(object sender, EventArgs e)
        {
            initVariables();
            grdMainList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdMainList.EnableHeadersVisualStyles = false;
            listTemplates();
        }
        private void listTemplates()
        {
            try
            {
                grdMainList.Rows.Clear();
                InternalOrderDB IODb = new InternalOrderDB();
                List<iorequirements> sefTempList = IODb.getTemplateListForProductType(sefid);

                int i = 1;
                List<string> strList = new List<string>();

                foreach (iorequirements req in sefTempList)
                {
                    grdMainList.Rows.Add();
                    grdMainList.Rows[grdMainList.RowCount - 1].Cells["SiNo"].Value = i;
                    grdMainList.Rows[grdMainList.RowCount - 1].Cells["SEFID1"].Value = req.SEFID;
                    grdMainList.Rows[grdMainList.RowCount - 1].Cells["SEFRefNo"].Value = req.SEFReferenceNo;
                    grdMainList.Rows[grdMainList.RowCount - 1].Cells["SequenceNo"].Value = req.SequenceNo;
                    grdMainList.Rows[grdMainList.RowCount - 1].Cells["Description"].Value = req.Description;

                    iorequirements ioreqTemp = reqList.FirstOrDefault(listItem => listItem.SEFReferenceNo == req.SEFReferenceNo);

                    if (ioreqTemp != null)
                    {
                        if(ioreqTemp.RequiredValue != null)
                            grdMainList.Rows[grdMainList.RowCount - 1].Cells["RequiredValue"].Value = ioreqTemp.RequiredValue;
                        else
                            grdMainList.Rows[grdMainList.RowCount - 1].Cells["RequiredValue"].Value = "";
                    }
                    else
                        grdMainList.Rows[grdMainList.RowCount - 1].Cells["RequiredValue"].Value = "";
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Template  Listing");
            }
            try
            {
                pnlList.Visible = true;
            }
            catch (Exception ex)
            {
            }
        }

        private void initVariables()
        {
            grdMainList.Visible = true;
            grdMainList.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        private void grdMainList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;
                string columnName = grdMainList.Columns[e.ColumnIndex].Name;
                try
                {
                    if (columnName.Equals("Select"))
                    {
                        descClickRowIndex = e.RowIndex;
                        string strTest = grdMainList.Rows[e.RowIndex].Cells["RequiredValue"].Value.ToString().Trim();
                        showPopUpForDescription(strTest);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {

            }
        }
        //-----------
        private void showPopUpForDescription(string str)
        {
            frmPopup = new Form();
            frmPopup.StartPosition = FormStartPosition.CenterScreen;
            frmPopup.BackColor = Color.CadetBlue;
            frmPopup.MaximizeBox = false;
            frmPopup.MinimizeBox = false;
            frmPopup.ControlBox = false;
            frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;
            frmPopup.Size = new Size(360, 170);

            Label head = new Label();
            head.AutoSize = true;
            head.Location = new System.Drawing.Point(3, 3);
            head.Name = "label2";
            head.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            head.ForeColor = Color.White;
            head.Size = new System.Drawing.Size(146, 13);
            head.Text = "Fill Require Value Below";
            frmPopup.Controls.Add(head);

            txtDesc = new RichTextBox();
            txtDesc.Text = str;
            txtDesc.Bounds = new Rectangle(new Point(3, 25), new Size(345, 111));
            frmPopup.Controls.Add(txtDesc);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.BackColor = Color.Tan;
            lvOK.Location = new System.Drawing.Point(210, 142);
            lvOK.Size = new System.Drawing.Size(64, 23);
            lvOK.Cursor = Cursors.Hand;
            lvOK.Click += new System.EventHandler(this.lvOK_Click5);
            frmPopup.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "CANCEL";
            lvCancel.BackColor = Color.Tan;
            lvCancel.Location = new System.Drawing.Point(273, 142);
            lvCancel.Size = new System.Drawing.Size(73, 23);
            lvCancel.Cursor = Cursors.Hand;
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click5);
            frmPopup.Controls.Add(lvCancel);

            frmPopup.ShowDialog();
        }
        private void lvOK_Click5(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtDesc.Text.Trim()))
                {
                    MessageBox.Show("Description is empty");
                    return;
                }
                grdMainList.Rows[descClickRowIndex].Cells["RequiredValue"].Value = txtDesc.Text.Trim().Replace("'", "''");
                grdMainList.FirstDisplayedScrollingRowIndex = grdMainList.Rows[descClickRowIndex].Index;
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        private void lvCancel_Click5(object sender, EventArgs e)
        {
            try
            {
                txtDesc.Text = "";
                descClickRowIndex = -1;
                frmPopup.Close();
                frmPopup.Dispose();

            }
            catch (Exception)
            {
            }
        }

        private void btnSaveSpecification_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in grdMainList.Rows)
            {
                if(row.Cells["RequiredValue"].Value.ToString().Trim().Length == 0)
                {
                    MessageBox.Show("Fill Value at row:" + (row.Index + 1));
                    return;
                }
            }
            this.ReturnValue1 = getIOReqDetails();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        //-----------
        private List<iorequirements> getIOReqDetails()
        {
            List<iorequirements> IOReqList = new List<iorequirements>();
            try
            {
                iorequirements ioReq = new iorequirements();
                for (int i = 0; i < grdMainList.Rows.Count; i++)
                {
                    ioReq = new iorequirements();
                    ioReq.SEFID = grdMainList.Rows[i].Cells["SEFID1"].Value.ToString();
                    ioReq.SEFReferenceNo = Convert.ToInt32(grdMainList.Rows[i].Cells["SEFRefNo"].Value);
                    ioReq.RequiredValue = grdMainList.Rows[i].Cells["RequiredValue"].Value.ToString();
                    ioReq.SequenceNo = Convert.ToInt32(grdMainList.Rows[i].Cells["SequenceNo"].Value);
                    ioReq.Description = grdMainList.Rows[i].Cells["Description"].Value.ToString();
                    IOReqList.Add(ioReq);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("getIOReqDetails() : Error getting IOReq Details");
                IOReqList = null;
            }
            return IOReqList;
        }



    }
}


