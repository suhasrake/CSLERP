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
using CSLERP.PrintForms;

namespace CSLERP
{
    public partial class ProformaInvoice : System.Windows.Forms.Form
    {
        System.Data.DataTable TaxDetailsTable = new System.Data.DataTable();
        double productvalue = 0.0;
        double taxvalue = 0.0;
        int DocNo = 0;
        DateTime docdate = DateTime.MinValue;
        DateTime Tempdate = DateTime.MinValue;
        public ProformaInvoice()
        {
            try
            {
                InitializeComponent();
                //////this.FormBorderStyle = FormBorderStyle.None;

            }
            catch (Exception)
            {

            }
        }
        private void Document_Load(object sender, EventArgs e)
        {
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;

            String a = this.Size.ToString();
            grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdList.EnableHeadersVisualStyles = false;
            ListDocument();
            applyPrivilege();
        }
        private void ListDocument()
        {
            try
            {
                grdList.Rows.Clear();
                ProformaInvoiceDB dbrecord = new ProformaInvoiceDB();
                List<proformainvoice> Documents = dbrecord.getProformaDocuments();
                foreach (proformainvoice doc in Documents)
                {

                    grdList.Rows.Add(doc.DocumentID, doc.TemporaryNo, doc.TemporaryDate,
                        doc.DocumentNo, doc.DocumentDate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            enableBottomButtons();
            pnlDocumentList.Visible = true;
        }

        private void initVariables()
        {
            try
            {
                TaxDetailsTable.Columns.Add("TaxItem", typeof(string));
                TaxDetailsTable.Columns.Add("TaxAmount", typeof(double));
                ////documentStatusValues = new string[2, 2]
                ////        {
                ////    {"1","Active" },
                ////    {"2","Dective" }
                ////        };
                //fillDocumentStatusCombo(cmbDocumentStatus);
                //fillIsReversibleCombo(cmbInvoiceDocument);
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
                pnlDocumentList.Visible = true;
            }
            catch (Exception)
            {

            }
        }

        public void clearDocumentData()
        {
            try
            {
                txtCustomerName.Text = "";
                txtTemporaryNo.Text = "";
                Tempdate = DateTime.MinValue;
                docdate = DateTime.MinValue;
                DocNo = 0;
                //cmbDocumentStatus.SelectedIndex = 0;
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
                //txtCustomerName.Enabled = true;
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

                proformainvoice PI = new proformainvoice();
                ProformaInvoiceDB PIDB = new ProformaInvoiceDB();
                PI.DocumentID = cmbInvoiceDocument.SelectedItem.ToString();
                PI.TemporaryNo = Convert.ToInt32(txtTemporaryNo.Text);
                PI.TemporaryDate = Tempdate;
                PI.DocumentDate = docdate;
                PI.DocumentNo = DocNo;
                if (PIDB.validateDocument(PI))
                {
                    if (PIDB.insertDocument(PI))
                    {
                        MessageBox.Show("Proforma Invoice data Added");
                        Tempdate = DateTime.MinValue;
                        docdate = DateTime.MinValue;
                        DocNo = 0;
                        closeAllPanels();
                        ListDocument();
                    }
                    else
                    {
                        MessageBox.Show("Failed to Insert Proforma Invoice");
                    }
                }
                else
                {
                    MessageBox.Show("Please check the temporaryNo Entered!!!");
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
                if (columnName.Equals("Print"))
                {
                    //int rowID = e.RowIndex;
                    //////string tempDate = "";
                    ////Edit Button
                    ////MessageBox.Show("You clicked edit button");
                    //btnSave.Text = "Update";
                    //pnlDocumentInner.Visible = true;
                    //pnlDocumentOuter.Visible = true;
                    //pnlDocumentList.Visible = false;
                    //txtCustomerName.Enabled = false;
                    //cmbDocumentStatus.SelectedIndex = cmbDocumentStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[2].Value.ToString());
                    //DataGridViewRow row = grdList.Rows[rowID];
                    //txtCustomerName.Text = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    //txtTemporaryNo.Text = grdList.Rows[e.RowIndex].Cells[1].Value.ToString();
                    //txtInvoiceAmount.Text = grdList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    string Docid = grdList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    int Tempno = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells[1].Value);
                    DateTime Tempdate = Convert.ToDateTime(grdList.Rows[e.RowIndex].Cells[2].Value);
                   string PODocIdWRTIO = getPODocIDForSelectedIO(Docid);
                    //disableBottomButtons();
                    grdList.Visible = true;
                    btnNew.Visible = true;
                    btnExit.Visible = true;
                    //CSLERP.PrintForms.PrintPurchaseOrder ppo = new CSLERP.PrintForms.PrintPurchaseOrder();
                    PrintInvoiceOut pio = new PrintInvoiceOut();
                    invoiceoutheader ioh = new invoiceoutheader();
                    InvoiceOutHeaderDB iodb = new InvoiceOutHeaderDB();
                    List<invoiceoutheader> InvceOuthdr = iodb.getFilteredInvoiceOutHeaderList(Docid, Tempno, Tempdate);
                    ioh = InvceOuthdr.FirstOrDefault();
                    List<invoiceoutdetail> IODetails = InvoiceOutHeaderDB.getInvoiceOutDetail(ioh);
                    foreach (invoiceoutdetail iod in IODetails)
                    {
                        if (iod.HSNCode.Trim().Length <= 0)
                        {
                            MessageBox.Show("HSN Code not available for " + iod.StockItemName + ". Print aborted....");
                            return;
                        }
                    }
                    getTaxDetails(IODetails);
                    string taxstr = getTasDetailStr();
                    //string taxstr = "";
                    taxDetails4Print(IODetails, ioh.DocumentID);
                    if (ioh.DocumentID == "PRODUCTEXPORTINVOICEOUT" || ioh.DocumentID == "SERVICEEXPORTINVOICEOUT")
                    {
                        PrintExportInvoiceOut expPrint = new PrintExportInvoiceOut();
                        expPrint.PrintExportIO(ioh, IODetails, taxstr, PODocIdWRTIO);
                    }
                    else
                    {
                        pio.PrintIO(ioh, IODetails, taxstr, PODocIdWRTIO);
                    }
                    btnNew.Visible = true;
                    btnExit.Visible = true;
                    return;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private string getPODocIDForSelectedIO(string iodocid)
        {
            string podocid = "";
            switch (iodocid)
            {
                case "PRODUCTEXPORTINVOICEOUT":
                    podocid = "POPRODUCTINWARD";
                    break;
                case "SERVICEEXPORTINVOICEOUT":
                    podocid = "POSERVICEINWARD";
                    break;
                case "PRODUCTINVOICEOUT":
                    podocid = "POPRODUCTINWARD";
                    break;
                case "SERVICEINVOICEOUT":
                    podocid = "POSERVICEINWARD";
                    break;
                case "SERVICERCINVOICEOUT":
                    podocid = "POSERVICERCINWARD";
                    break;
                default:
                    podocid = "";
                    break;
            }
            return podocid;
        }

        private string getTasDetailStr()
        {
            string strTax = "";
            try
            {
                for (int i = 0; i < (TaxDetailsTable.Rows.Count); i++)
                {
                    strTax = strTax + Convert.ToString(TaxDetailsTable.Rows[i][0]) + Main.delimiter1 +
                    Convert.ToString(TaxDetailsTable.Rows[i][1]) + Main.delimiter2;
                }
                //DialogResult dialog = MessageBox.Show(strTax, "Tax Details", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error showing tax details");
            }
            return strTax;
        }

        private System.Data.DataTable taxDetails4Print(List<invoiceoutdetail> IODetails, string documentID)
        {
            int HSNLength = 0;
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataTable dtTax = new System.Data.DataTable();
            try
            {
                if (documentID == "PRODUCTINVOICE")
                {
                    HSNLength = 4;
                }
                else if (documentID == "SERVICEINVOICE")
                {
                    HSNLength = 6;
                }

                {
                    dt.Columns.Add("HSNCode", typeof(string));
                    dt.Columns.Add("TaxCode", typeof(string));
                    dt.Columns.Add("Amount", typeof(double));
                    dt.Columns.Add("TaxAmount", typeof(double));
                    dt.Columns.Add("TaxItem", typeof(string));
                    dt.Columns.Add("TaxItemPercentage", typeof(double));
                    dt.Columns.Add("TaxItemAmount", typeof(double));
                }
                //fill hsn code wise tax details in dt
                foreach (invoiceoutdetail iod in IODetails)
                {
                    string tstr = iod.TaxDetails;
                    string[] lst1 = tstr.Split('\n');
                    for (int j = 0; j < lst1.Length - 1; j++)
                    {
                        string[] lst2 = lst1[j].Split('-');
                        if (Convert.ToDouble(lst2[1]) > 0)
                        {
                            dt.Rows.Add();
                            dt.Rows[dt.Rows.Count - 1][0] = iod.HSNCode.Substring(0, HSNLength);
                            dt.Rows[dt.Rows.Count - 1][1] = iod.TaxCode;
                            dt.Rows[dt.Rows.Count - 1][2] = iod.Quantity * iod.Price;
                            dt.Rows[dt.Rows.Count - 1][3] = iod.Tax;
                            dt.Rows[dt.Rows.Count - 1][4] = lst2[0];
                            dt.Rows[dt.Rows.Count - 1][5] = iod.HSNCode; //need to replace with percentage
                            dt.Rows[dt.Rows.Count - 1][6] = lst2[1];
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }

            try
            {
                //fill tax rate for each tax item in dt
                TaxCodeWorkingDB tcwdb = new TaxCodeWorkingDB();
                List<taxcodeworking> tcwDetails = tcwdb.getTaxCodeDetails();
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    foreach (taxcodeworking tcwd in tcwDetails)
                    {
                        if (dt.Rows[i][1].ToString() == tcwd.TaxCode && dt.Rows[i][4].ToString() == tcwd.TaxItemName)
                        {
                            dt.Rows[i][5] = tcwd.OperatorValue;
                            break;
                        }
                    }

                }
                //prepare HSN Code wise totals in a new table
                System.Data.DataTable dttotal = new System.Data.DataTable();
                dttotal = dt.Copy();
                dttotal.Clear();

                for (int i = 0; i < (dt.Rows.Count); i++)
                {

                    Boolean fount = false;
                    string tstr1 = dt.Rows[i][0].ToString();
                    string tstr2 = dt.Rows[i][4].ToString();
                    string tstr3 = dt.Rows[i][5].ToString();
                    for (int j = 0; j < (dttotal.Rows.Count); j++)
                    {
                        string tstr4 = dttotal.Rows[j][0].ToString();
                        string tstr5 = dttotal.Rows[j][4].ToString();
                        string tstr6 = dttotal.Rows[j][5].ToString();

                        if (tstr1 == tstr4 && tstr2 == tstr5 && tstr3 == tstr6)
                        {
                            dttotal.Rows[j][2] = Convert.ToDouble(dttotal.Rows[j][2].ToString()) +
                                Convert.ToDouble(dt.Rows[i][2].ToString());
                            dttotal.Rows[j][6] = Convert.ToDouble(dttotal.Rows[j][6].ToString()) +
                                Convert.ToDouble(dt.Rows[i][6].ToString());
                            fount = true;
                        }
                    }
                    if (!fount)
                    {
                        dttotal.ImportRow(dt.Rows[i]);
                    }
                }
                string tstr = "";
                ////for (int i = 0; i < (dttotal.Rows.Count); i++)
                ////{
                ////    tstr = tstr+
                ////        dttotal.Rows[i][0].ToString() + "," +
                ////        dttotal.Rows[i][1].ToString() + "," +
                ////        dttotal.Rows[i][2].ToString() + "," +
                ////        dttotal.Rows[i][3].ToString() + "," +
                ////        dttotal.Rows[i][4].ToString() + "," +
                ////        dttotal.Rows[i][5].ToString() + "," +
                ////        dttotal.Rows[i][6].ToString() + "\n";

                ////}
                ////MessageBox.Show(tstr);
                //create print table
                tstr = "";
                //find distinct tax item in dttotal
                DataTable dtDistinct = dttotal.AsEnumerable().GroupBy(row => row.Field<string>("TaxItem")).Select(group => group.First()).CopyToDataTable();
                ////for (int i = 0; i < (dtDistinct.Rows.Count); i++)
                ////{
                ////    tstr = tstr +
                ////        dtDistinct.Rows[i][0].ToString() + "," +
                ////        dtDistinct.Rows[i][1].ToString() + "," +
                ////        dtDistinct.Rows[i][2].ToString() + "," +
                ////        dtDistinct.Rows[i][3].ToString() + "," +
                ////        dtDistinct.Rows[i][4].ToString() + "," +
                ////        dtDistinct.Rows[i][5].ToString() + "," +
                ////        dtDistinct.Rows[i][6].ToString() + "\n";

                ////}
                ////MessageBox.Show(tstr);

                //create columns in dttax table. dynamically creating the columns for each tax item
                {
                    dtTax.Columns.Add("HSNCode", typeof(string));
                    dtTax.Columns.Add("Amount", typeof(double));
                    for (int i = 0; i < dtDistinct.Rows.Count && i < 3; i++)
                    {
                        dtTax.Columns.Add(dtDistinct.Rows[i][4].ToString(), typeof(string));
                        dtTax.Columns.Add(dtDistinct.Rows[i][4].ToString() + "Amount", typeof(double));
                    }
                    dtTax.Columns.Add("Total", typeof(double));
                }
                //add data in dttax table
                for (int i = 0; i < (dttotal.Rows.Count); i++)
                {
                    Boolean hsnFount = false;
                    string tstr1 = dttotal.Rows[i][0].ToString(); //for domestic
                    int j = 0;
                    for (j = 0; j < (dtTax.Rows.Count); j++)
                    {
                        string tstr2 = dtTax.Rows[j][0].ToString(); //for domestic
                        if (tstr1 == tstr2)
                        {
                            hsnFount = true;
                            break;
                        }
                    }
                    if (!hsnFount)
                    {
                        dtTax.Rows.Add();
                        j = dtTax.Rows.Count - 1;
                        dtTax.Rows[j][0] = tstr1;
                        dtTax.Rows[j][1] = dttotal.Rows[i][2]; ;
                    }
                    string tstr3 = dttotal.Rows[i][4].ToString();
                    string tstr4 = dttotal.Rows[i][4].ToString() + "Amount";
                    try
                    {
                        dtTax.Rows[j][tstr3] = dttotal.Rows[i][5];
                        dtTax.Rows[j][tstr4] = dttotal.Rows[i][6];
                        string t1 = String.IsNullOrEmpty(dtTax.Rows[j]["Total"].ToString()) ? "0" : dtTax.Rows[j]["Total"].ToString();
                        string t2 = String.IsNullOrEmpty(dttotal.Rows[i][6].ToString()) ? "0" : dttotal.Rows[i][6].ToString();
                        double d1 = Convert.ToDouble(t1) + Convert.ToDouble(t2);
                        dtTax.Rows[j]["Total"] = d1;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error creating HSN wise tax summary");
                    }
                }
                tstr = "";
                ////double td1=0, td2=0, td3 = 0;
                ////for (int i = 0; i < (dtTax.Rows.Count); i++)
                ////{
                ////    for (int j=0; j<dtTax.Columns.Count;j++)
                ////    {
                ////        tstr = tstr + dtTax.Rows[i][j].ToString() + ",";
                ////    }
                ////    tstr = tstr+"\n";
                ////    td1 = td1 + Convert.ToDouble(dtTax.Rows[i][1].ToString());
                ////    td2 = td2 + Convert.ToDouble(dtTax.Rows[i][dtTax.Columns.Count-1].ToString());
                ////}

                ////MessageBox.Show(tstr);
                ////MessageBox.Show(td1.ToString());
                ////MessageBox.Show(td2.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("taxDetails4Print() : Error - " + ex.ToString());
            }
            return dtTax;
        }

        private void getTaxDetails(List<invoiceoutdetail> iodList)
        {
            try
            {
                double quantity = 0;
                double price = 0;
                double cost = 0.0;
                productvalue = 0.0;
                taxvalue = 0.0;
                string strtaxCode = "";
                //clear tax details table
                TaxDetailsTable.Rows.Clear();
                foreach (invoiceoutdetail iod in iodList)
                {
                    quantity = iod.Quantity;
                    price = iod.Price;
                    if (price != 0 && quantity != 0)
                    {
                        cost = Math.Round(quantity * price, 2);
                    }

                    try
                    {
                        strtaxCode = iod.TaxCode;
                    }
                    catch (Exception)
                    {
                        strtaxCode = "";
                    }
                    System.Data.DataTable TaxData = TaxCodeWorkingDB.calculateTax(strtaxCode, cost);
                    double ttax1 = 0.0;
                    double ttax2 = 0.0;
                    string strTax = "";
                    for (int j = 0; j < TaxData.Rows.Count; j++)
                    {
                        string tstr = "";
                        try
                        {
                            tstr = TaxData.Rows[j][7].ToString().Trim().Substring(0, TaxData.Rows[j][7].ToString().Trim().IndexOf('-'));
                            if (!(tstr.Length == 0 && tstr.Equals("Dummy")))
                            {
                                ttax1 = Convert.ToDouble(TaxData.Rows[j][6]);
                                string a = Convert.ToString(TaxData.Rows[j][1]);
                                string b = Convert.ToString(TaxData.Rows[j][6]);
                                string c = Convert.ToString(TaxData.Rows[j][7]);
                                strTax = strTax + tstr + "-" +
                                    Convert.ToString(TaxData.Rows[j][6]) + "\n";
                                int taxcodefound = 0;
                                for (int k = 0; k < (TaxDetailsTable.Rows.Count); k++)
                                {
                                    if (TaxDetailsTable.Rows[k][0].ToString().Trim().Equals(tstr))
                                    {
                                        TaxDetailsTable.Rows[k][1] = Convert.ToDouble(TaxDetailsTable.Rows[k][1]) +
                                            Convert.ToDouble(TaxData.Rows[j][6]);
                                        taxcodefound = 1;
                                    }
                                }
                                if (taxcodefound == 0)
                                {
                                    TaxDetailsTable.Rows.Add();
                                    TaxDetailsTable.Rows[TaxDetailsTable.Rows.Count - 1][0] = tstr;
                                    TaxDetailsTable.Rows[TaxDetailsTable.Rows.Count - 1][1] =
                                       Convert.ToDouble(TaxData.Rows[j][6]);
                                }
                            }
                            else
                            {
                                ttax1 = 0.0;
                            }
                        }
                        catch (Exception ex)
                        {
                            ttax1 = 0.0;
                        }
                        ttax2 = ttax2 + ttax1;
                    }
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

        private void Document_Enter(object sender, EventArgs e)
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

        private void txtTemporaryNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtTemporaryNo.Text.Trim().Length > 0)
                {
                    string docid = cmbInvoiceDocument.SelectedItem.ToString();
                    int tempno = Convert.ToInt32(txtTemporaryNo.Text);
                    List<proformainvoice> custval = ProformaInvoiceDB.getProformaInfo(tempno, docid);
                    if (custval != null)
                    {
                        txtCustomerName.Text = custval.Select(x => x.customername).FirstOrDefault();
                        txtInvoiceAmount.Text = custval.Select(x => x.InvoiceAmt).FirstOrDefault().ToString();
                        DocNo = custval.Select(x => x.DocumentNo).FirstOrDefault();
                        docdate = custval.Select(x => x.DocumentDate).FirstOrDefault();
                        Tempdate = custval.Select(x => x.TemporaryDate).FirstOrDefault();
                    }
                }
                else
                {
                    txtCustomerName.Text = "";
                    txtInvoiceAmount.Text ="";
                    DocNo = 0;
                    docdate = DateTime.MinValue;
                    Tempdate = DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void txtTemporaryNo_TabIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

