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
    public partial class SRMReport : System.Windows.Forms.Form
    {
        Form frmPopup = new Form();
        ListView exlv = new ListView();// list view for choice / selection list
        int val = 0;
        string regional = "";
        string Report = "";
        public SRMReport()
        {
            try
            {
                InitializeComponent();

            }
            catch (Exception)
            {

            }
        }
        private void CompanyData_Load(object sender, EventArgs e)
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
            //Listdata();
            ListSRMData();
            applyPrivilege();

        }
        private void ListSRMData()
        {
            try
            {

                List<SRM> SRMdata = new List<SRM>();
                SRMDB srmdb = new SRMDB();
                SRMdata = srmdb.getSRMdata();
                List<SRM> SRMdata2 = new List<SRM>();
                try
                {
                    foreach (var itm in SRMdata)
                    {
                        SRM srm = new SRM();
                        srm.StockItmID = itm.StockItmID;
                        srm.StockReferenceNo = itm.StockReferenceNo;
                        srm.DOCUMENTID = itm.DOCUMENTID;
                        srm.DOCUMENTNO = itm.DOCUMENTNO;
                        srm.DOCUMENTDATE = itm.DOCUMENTDATE;
                        srm.OB = itm.OB;
                        srm.MRN = itm.MRN;
                        srm.GTNIN = itm.GTNIN;
                        srm.INVOICE = itm.INVOICE;
                        srm.GTNOUT = itm.GTNOUT;
                        srm.STOREID = itm.STOREID;
                        srm.FYID = itm.FYID;
                        srm.PresentStock = itm.PresentStock;
                        srm.issuequantity = itm.issuequantity;
                        srm.Name = itm.Name;
                        ////if (itm.DOCUMENTID != "STOCKOB" && itm.DOCUMENTID != "OB")
                        {
                            if (itm.DOCUMENTID == "MRN")
                            {
                                srm.MRN = itm.OB;
                                srm.OB = 0;
                            }
                            else if (itm.DOCUMENTID == "GTN")
                            {
                                srm.GTNIN = itm.OB;
                                srm.OB = 0;
                            }
                        }
                        SRMdata2.Add(srm);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("ListFilteredLeave() : Error 1");
                }
                try
                {
                    List<SRM> SRMMRNData = new List<SRM>();
                    SRMMRNData = srmdb.getSRMMRNdata();


                    var SRMdatachk1 = SRMdata2.Where(W => W.DOCUMENTID == "MRN").ToList();
                    var SRMMRNdatadif2 = SRMdatachk1.Where(s => !SRMMRNData.Any(W => W.DOCUMENTID == s.DOCUMENTID && W.DOCUMENTNO2.ToString() == s.DOCUMENTNO && W.DOCUMENTDATE == s.DOCUMENTDATE && W.MRN == s.MRN));
                    var SRMMRNdatadif1 = SRMMRNData.Where(s => !SRMdata2.Any(W => W.DOCUMENTID == s.DOCUMENTID && W.DOCUMENTNO == s.DOCUMENTNO2.ToString() && W.DOCUMENTDATE == s.DOCUMENTDATE && W.MRN == s.MRN));

                    foreach (var itm in SRMMRNdatadif2)
                    {
                        Report += "StockItmID:" + itm.StockItmID + ", StockReferenceNo:" + itm.StockReferenceNo + ", DOCUMENTID:" + itm.DOCUMENTID + ", DOCUMENTNO:" + itm.DOCUMENTNO + ", DOCUMENTDATE:" + itm.DOCUMENTDATE + ", OB:" + itm.OB + ", MRN:" + itm.MRN + "GTNIN:" + itm.GTNIN + ", INVOICE:" + itm.INVOICE + ", GTNOUT:" + itm.GTNOUT + ", PresentStock" + itm.PresentStock + ", issuequantity" + itm.issuequantity + ", STOREID:" + itm.STOREID + ", FYID:" + itm.FYID + " does not contain in SRMMRN \n\n";
                    }
                    foreach (var itm in SRMMRNdatadif1)
                    {
                        Report += "StockItmID:" + itm.StockItmID + ", StockReferenceNo:" + itm.StockReferenceNo + ", DOCUMENTID:" + itm.DOCUMENTID + ", DOCUMENTNO:" + itm.DOCUMENTNO + ", DOCUMENTDATE:" + itm.DOCUMENTDATE + ", QUANTITYRECEIPT:" + itm.GTNIN + ", QUANTITYISSUE:" + itm.GTNOUT + ", STOREID:" + itm.STOREID + ", FYID:" + itm.FYID + " does not contain in SRM Grid compare to SRMMRN\n \n";
                    }
                    List<SRM> SRMGTNINdata = new List<SRM>();
                    SRMGTNINdata = srmdb.getSRMGTNINdata();
                    var SRMdatachk2 = SRMdata2.Where(W => W.DOCUMENTID == "GTN").ToList();
                    var SRMGTNINdatadif1 = SRMGTNINdata.Where(s => !SRMdata2.Any(W => W.DOCUMENTID == s.DOCUMENTID && W.DOCUMENTNO == s.DOCUMENTNO2.ToString() && W.DOCUMENTDATE == s.DOCUMENTDATE && W.GTNIN == s.GTNIN));
                    var SRMGTNINdatadif2 = SRMdatachk2.Where(s => !SRMGTNINdata.Any(W => W.DOCUMENTID == s.DOCUMENTID && W.DOCUMENTNO2.ToString() == s.DOCUMENTNO && W.DOCUMENTDATE == s.DOCUMENTDATE && W.GTNIN == s.GTNIN));

                    foreach (var itm in SRMGTNINdatadif1)
                    {
                        Report += "StockItmID:" + itm.StockItmID + ", StockReferenceNo:" + itm.StockReferenceNo + ", DOCUMENTID:" + itm.DOCUMENTID + ", DOCUMENTNO:" + itm.DOCUMENTNO + ", DOCUMENTDATE:" + itm.DOCUMENTDATE + ", QUANTITYRECEIPT:" + itm.GTNIN + ", QUANTITYISSUE:" + itm.GTNOUT + ", STOREID:" + itm.STOREID + ", FYID:" + itm.FYID + " does not contain in SRM Grid compare to SRMGTNIN \n\n";
                    }
                    foreach (var itm in SRMGTNINdatadif2)
                    {
                        Report += "StockItmID:" + itm.StockItmID + ", StockReferenceNo:" + itm.StockReferenceNo + ", DOCUMENTID:" + itm.DOCUMENTID + ", DOCUMENTNO:" + itm.DOCUMENTNO + ", DOCUMENTDATE:" + itm.DOCUMENTDATE + ", OB:" + itm.OB + ", MRN:" + itm.MRN + ", GTNIN:" + itm.GTNIN + ", INVOICE:" + itm.INVOICE + ", GTNOUT:" + itm.GTNOUT + ", PresentStock" + itm.PresentStock + ", issuequantity" + itm.issuequantity + ", STOREID:" + itm.STOREID + ", FYID:" + itm.FYID + " does not contain in SRMGTNIN \n\n";
                    }

                }
                catch (Exception)
                {
                    MessageBox.Show("ListFilteredLeave() : Error 1");
                }
                List<SRM> SRMinvdata = new List<SRM>();
                SRMinvdata = srmdb.getSRMinvdata();

                var abc = SRMinvdata.GroupBy(x => x.StockReferenceNo)
                   .Select(g => new
                   {
                       GroupName = g.Key,
                       QUANTITYISSUE = g.Sum(s => s.QUANTITYISSUE),
                       StockItemID = g.First().StockItmID,
                       StockReferenceNo = g.Key,
                       DocumentID = g.First().DOCUMENTID,
                       DOCUMENTNO2 = g.First().DOCUMENTNO2,
                       DOCUMENTDATE = g.First().DOCUMENTDATE,
                       QUANTITYRECEIPT = g.First().GTNIN,
                       STOREID = g.First().STOREID,
                       FYID = g.First().FYID
                   });
                var abc3 = from s in SRMdata2
                           join a in abc on s.StockReferenceNo equals a.GroupName into ps
                           from a in ps.DefaultIfEmpty()
                           select new
                           {
                               s.StockItmID,
                               s.StockReferenceNo,
                               s.DOCUMENTID,
                               s.DOCUMENTNO,
                               s.DOCUMENTDATE,
                               s.OB,
                               s.MRN,
                               s.GTNIN,
                               INVOICE = s.INVOICE,
                               QUANTITYISSUE = a == null ? String.Empty : a.QUANTITYISSUE.ToString(),
                               s.GTNOUT,
                               s.STOREID,
                               s.FYID,
                               s.PresentStock,
                               s.issuequantity,
                               s.Name
                           };

                var SRMinvdiff = abc.Where(s => !SRMdata2.Any(r => s.GroupName == r.StockReferenceNo));
                foreach (var itm in SRMinvdiff)
                {
                    Report += "StockItmID:" + itm.StockItemID + ", StockReferenceNo:" + itm.StockReferenceNo + ", DOCUMENTID:" + itm.DocumentID + ", DOCUMENTNO:" + itm.DOCUMENTNO2 + ", DOCUMENTDATE:" + itm.DOCUMENTDATE + ", QUANTITYRECEIPT:" + itm.QUANTITYRECEIPT + ", QUANTITYISSUE:" + itm.QUANTITYISSUE + ", STOREID:" + itm.STOREID + ", FYID:" + itm.FYID + " does not contain in SRM Grid compare to SRMinv \n\n";
                }

                List<SRM> SRMGTNOUTdata = new List<SRM>();
                SRMGTNOUTdata = srmdb.getSRMGTNOUTdata();

                var Aabc = SRMGTNOUTdata.GroupBy(x => x.StockReferenceNo)
                   .Select(g => new
                   {
                       GroupName = g.Key,
                       QUANTITYISSUE = g.Sum(s => s.QUANTITYISSUE),
                       StockItemID = g.First().StockItmID,
                       StockReferenceNo = g.Key,
                       DocumentID = g.First().DOCUMENTID,
                       DOCUMENTNO2 = g.First().DOCUMENTNO2,
                       DOCUMENTDATE = g.First().DOCUMENTDATE,
                       QUANTITYRECEIPT = g.First().GTNIN,
                       STOREID = g.First().STOREID,
                       FYID = g.First().FYID,

                   });
                List<SRM> tempFinalList = new List<SRM>();
                try
                {
                    var Aabc3 = from s in abc3
                                join a in Aabc on s.StockReferenceNo equals a.GroupName into ps
                                from a in ps.DefaultIfEmpty()
                                select new
                                {
                                    s.StockItmID,
                                    s.StockReferenceNo,
                                    s.DOCUMENTID,
                                    s.DOCUMENTNO,
                                    s.DOCUMENTDATE,
                                    s.OB,
                                    s.MRN,
                                    s.GTNIN,
                                    s.INVOICE,
                                    s.GTNOUT,
                                    s.QUANTITYISSUE,
                                    QUANTITYISSUE2 = a == null ? String.Empty : a.QUANTITYISSUE.ToString(),
                                    s.STOREID,
                                    s.FYID,
                                    s.PresentStock,
                                    s.issuequantity,
                                    s.Name
                                };
                    ////////MessageBox.Show("1");
                    var SRMGTNOUTdiff = abc.Where(s => !SRMdata2.Any(r => s.GroupName == r.StockReferenceNo));
                    foreach (var itm in SRMGTNOUTdiff)
                    {
                        Report += "StockItmID:" + itm.StockItemID + ", StockReferenceNo:" + itm.StockReferenceNo + ", DOCUMENTID:" + itm.DocumentID + ", DOCUMENTNO:" + itm.DOCUMENTNO2 + ", DOCUMENTDATE:" + itm.DOCUMENTDATE + ", QUANTITYRECEIPT:" + itm.QUANTITYRECEIPT + ", QUANTITYISSUE:" + itm.QUANTITYISSUE + ", STOREID:" + itm.STOREID + ", FYID:" + itm.FYID + " does not contain in SRM Grid compare to SRMGTNOUT \n\n";
                    }
                    //////MessageBox.Show("2");
                    foreach (var itm in Aabc3)
                    {
                        SRM srm = new SRM();
                        srm.StockItmID = itm.StockItmID;
                        srm.StockReferenceNo = itm.StockReferenceNo;
                        srm.DOCUMENTID = itm.DOCUMENTID;
                        srm.DOCUMENTNO = itm.DOCUMENTNO;
                        srm.DOCUMENTDATE = itm.DOCUMENTDATE;
                        srm.OB = itm.OB;
                        srm.MRN = itm.MRN;
                        srm.GTNIN = itm.GTNIN;
                        srm.INVOICE = Convert.ToDouble(itm.INVOICE) + Convert.ToDouble(itm.QUANTITYISSUE.ToString().Length == 0 ? "0" : itm.QUANTITYISSUE.ToString());
                        srm.GTNOUT = Convert.ToDouble(itm.GTNOUT) + Convert.ToDouble(itm.QUANTITYISSUE2.ToString().Length == 0 ? "0" : itm.QUANTITYISSUE2.ToString());
                        srm.STOREID = itm.STOREID;
                        srm.FYID = itm.FYID;
                        srm.PresentStock = itm.PresentStock;
                        srm.issuequantity = itm.issuequantity;
                        srm.Name = itm.Name;
                        srm.CalcIssueQuantity = (srm.INVOICE + srm.GTNOUT);
                        srm.CalcStock = (srm.OB + srm.MRN + srm.GTNIN) - (srm.INVOICE + srm.GTNOUT);
                        tempFinalList.Add(srm);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("ListFilteredLeave() : Error 2");
                }
                try
                {
                    grdList.Rows.Clear();
                    grdList.Columns.Clear();
                    grdList.Columns.Add("Sl.no", "Sl.no");
                    grdList.Columns.Add("StockItmID", "Stock ID");
                    grdList.Columns.Add("Name", "Stock Name");
                    grdList.Columns.Add("OB", "OB");
                    grdList.Columns.Add("MRN", "MRN");
                    grdList.Columns.Add("GTNIN", "GTNIN");
                    grdList.Columns.Add("INVOICE", "INVOICE");
                    grdList.Columns.Add("GTNOUT", "GTNOUT");
                    grdList.Columns.Add("PresentStock", "PresentStock");
                    grdList.Columns.Add("CalcStock", "CalcStock");
                    grdList.Columns.Add("issuequantity", "IssueQuantity");
                    grdList.Columns.Add("CalcIssueQuantity", "CalcIssueQuantity");
                    grdList.Columns.Add("StockReferenceNo", "StockReferenceNo");
                    grdList.Columns.Add("DOCUMENTID", "DOCUMENTID");
                    grdList.Columns.Add("DOCUMENTNO", "DOCUMENTNO");
                    grdList.Columns.Add("DOCUMENTDATE", "DOCUMENTDATE");
                    grdList.Columns.Add("STOREID", "STOREID");
                    grdList.Columns.Add("FYID", "FYID");

                    grdList.Columns["DOCUMENTDATE"].DefaultCellStyle.Format = "dd-MM-yyyy";
                    grdList.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                    for (int i = 0; i <= 10; i++)
                    {
                        grdList.Columns[i].Width = 70;
                    }
                    grdList.Columns["StockItmID"].Width = 120;
                    grdList.Columns["Name"].Width = 200;
                    ////double Clstk = 0.0, calisuQnty = 0.0;
                }
                catch (Exception)
                {
                    MessageBox.Show("ListFilteredLeave() : Error 3");
                }
                try
                {
                    foreach (var itm in tempFinalList)
                    {
                        ////////if (itm.StockItmID == "3114340017")
                        ////////{
                        ////////    MessageBox.Show("3114340017");
                        ////////}
                        grdList.Rows.Add();
                        grdList.Rows[grdList.Rows.Count - 1].Cells["Sl.no"].Value = grdList.Rows.Count;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["StockItmID"].Value = itm.StockItmID;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["Name"].Value = itm.Name;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["StockReferenceNo"].Value = itm.StockReferenceNo;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["DOCUMENTID"].Value = itm.DOCUMENTID;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["DOCUMENTNO"].Value = itm.DOCUMENTNO;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["DOCUMENTDATE"].Value = itm.DOCUMENTDATE;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["OB"].Value = itm.OB;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["MRN"].Value = itm.MRN;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["GTNIN"].Value = itm.GTNIN;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["INVOICE"].Value = itm.INVOICE;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["GTNOUT"].Value = itm.GTNOUT;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["STOREID"].Value = itm.STOREID;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["FYID"].Value = itm.FYID;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["PresentStock"].Value = itm.PresentStock;

                        ////Clstk = (itm.OB + itm.MRN + itm.GTNIN) - (itm.INVOICE + itm.GTNOUT);
                        ////calisuQnty = itm.INVOICE + itm.GTNOUT;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["CalcStock"].Value = itm.CalcStock;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["issuequantity"].Value = itm.issuequantity;
                        grdList.Rows[grdList.Rows.Count - 1].Cells["CalcIssueQuantity"].Value = itm.CalcIssueQuantity;
                        if (itm.issuequantity != itm.CalcIssueQuantity && itm.PresentStock != itm.CalcStock)
                        {
                            grdList.Rows[grdList.Rows.Count - 1].DefaultCellStyle.BackColor = Color.DeepPink;
                            grdList.Rows[grdList.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.White;
                        }
                        else if (itm.issuequantity != itm.CalcIssueQuantity)
                        {
                            grdList.Rows[grdList.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Yellow;

                        }
                        else if (itm.PresentStock != itm.CalcStock)
                        {
                            grdList.Rows[grdList.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Red;
                            grdList.Rows[grdList.Rows.Count - 1].DefaultCellStyle.ForeColor = Color.White;
                        }
                        else
                        {

                        }
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("ListFilteredLeave() : Error 4");
                }
                pnlgrdList.Visible = true;
                grdList.Visible = true;
                if (grdList.RowCount >= 1)
                {
                    lblSearch.Visible = true;
                    txtSearch.Visible = true;
                    btnExportToExcel.Visible = true;
                }
                grdList.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in Stock listing");
            }
        }

        private void initVariables()
        {
            closeAllPanels();
            btnView.Visible = true;

        }
        private void applyPrivilege()
        {
            try
            {
                if (Main.itemPriv[1])
                {
                    btnExportToExcel.Visible = true;
                }
                else
                {
                    btnExportToExcel.Visible = false;
                }
                if (Main.itemPriv[2])
                {
                    btnExportToExcel.Visible = true;
                }
                else
                {
                    btnExportToExcel.Visible = false;
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

                btnView.Visible = false;
                pnlLeaveDetailOuter.Visible = false;
                pnlLeaveDetailInner.Visible = false;
                lblSearch.Visible = false;
                txtSearch.Visible = false;
                btnExportToExcel.Visible = false;
                pnlgrdList.Visible = false;
            }
            catch (Exception)
            {

            }
        }

        public void clearUserData()
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
        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }




        private void cmbcmpnysrch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void filterGridData()
        {
            try
            {
                foreach (DataGridViewRow row in grdList.Rows)
                {
                    row.Visible = true;
                }
                if (txtSearch.Text.Length != 0)
                {

                    foreach (DataGridViewRow row in grdList.Rows)
                    {
                        if (!row.Cells["Name"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
                        {
                            row.Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            //closeAllPanels();

            pnlLeaveDetailOuter.Visible = true;
            pnlLeaveDetailInner.Visible = true;
            pnlgrdList.SendToBack();
            btnExportToExcel.Enabled = false;
            txtSearch.Enabled = false;
            grdList.Enabled = false;
            rtbreport.Text = Report;
        }

        private void btnExportToExcel_Click(object sender, EventArgs e)
        {
            try
            {
                frmPopup = new Form();
                frmPopup.StartPosition = FormStartPosition.CenterScreen;
                frmPopup.BackColor = Color.CadetBlue;

                frmPopup.MaximizeBox = false;
                frmPopup.MinimizeBox = false;
                frmPopup.ControlBox = false;
                frmPopup.FormBorderStyle = FormBorderStyle.FixedSingle;

                frmPopup.Size = new Size(450, 310);
                exlv = Utilities.GridColumnSelectionView(grdList);

                exlv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(0, 25), new System.Drawing.Size(450, 250));
                frmPopup.Controls.Add(exlv);

                System.Windows.Forms.Label pnlHeading = new System.Windows.Forms.Label();
                pnlHeading.Size = new Size(300, 20);
                pnlHeading.Location = new System.Drawing.Point(5, 5);
                pnlHeading.Text = "Select Gridview Colums to Export";
                pnlHeading.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                pnlHeading.ForeColor = Color.Black;
                frmPopup.Controls.Add(pnlHeading);

                System.Windows.Forms.Button exlvOK = new System.Windows.Forms.Button();
                exlvOK.Text = "OK";
                exlvOK.BackColor = Color.Tan;
                exlvOK.Location = new System.Drawing.Point(40, 280);
                exlvOK.Click += new System.EventHandler(this.exlvOK_Click1);
                frmPopup.Controls.Add(exlvOK);

                System.Windows.Forms.Button exlvCancel = new System.Windows.Forms.Button();
                exlvCancel.Text = "CANCEL";
                exlvCancel.BackColor = Color.Tan;
                exlvCancel.Location = new System.Drawing.Point(130, 280);
                exlvCancel.Click += new System.EventHandler(this.exlvCancel_Click2);
                frmPopup.Controls.Add(exlvCancel);

                frmPopup.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void exlvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
            }
        }

        private void exlvOK_Click1(object sender, EventArgs e)
        {
            try
            {

                string heading1 = "SRM Report";
                string heading2 = "";
                Utilities.export2Excel(heading1, heading2, "", grdList, exlv);
                frmPopup.Close();
                frmPopup.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export to Excell error");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            filterGridData();
        }

        private void cmbStoreLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdList.Visible = false;
            grdList.Rows.Clear();
            pnlgrdList.Visible = false;
            lblSearch.Visible = false;
            txtSearch.Visible = false;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            pnlgrdList.Visible = true;
            grdList.Visible = true;
            btnView.Visible = true;
            btnExportToExcel.Enabled = true;
            txtSearch.Enabled = true;
            grdList.Enabled = true;
            pnlLeaveDetailOuter.Visible = false;
            pnlLeaveDetailInner.Visible = false;
        }

        private void grdList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    grdList.Rows[e.RowIndex].Selected = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void SRMReport_Enter(object sender, EventArgs e)
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

