using CSLERP.DBData;
using CSLERP.PrintForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLERP.FileManager
{

    public partial class ShowPOInward : Form
    {
        string docID;
        int tempno;
        DateTime tempdate;
        string docList = "";
        string officeList = "";
        string trackerList = "";
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        public ShowPOInward(string docid, int tno, DateTime tdate)
        {
            InitializeComponent();
            docID = docid;
            tempno = tno;
            tempdate = tdate;
            initVariable();
        }
        private void initVariable()
        {
            try
            {
                OfficeDB.fillOfficeComboNew(cmbOfficeID);
                ProjectHeaderDB.fillprojectCombo(cmbProjectID);
                CustomerDB.fillCustomerComboNew(cmbCustomer);
                CurrencyDB.fillCurrencyComboNew(cmbCurrency);
                CatalogueValueDB.fillCatalogValueComboNew(cmbPaymentMode, "PaymentMode");
                CatalogueValueDB.fillCatalogValueComboNew(cmbFreightTerms, "Freight");
                dtDeliveryDate.Format = DateTimePickerFormat.Custom;
                dtDeliveryDate.CustomFormat = "dd-MM-yyyy";
                dtPODate.Format = DateTimePickerFormat.Custom;
                dtPODate.CustomFormat = "dd-MM-yyyy";
                dtTrackingDate.Format = DateTimePickerFormat.Custom;
                dtTrackingDate.CustomFormat = "dd-MM-yyyy";
                dtValidateDate.Format = DateTimePickerFormat.Custom;
                dtValidateDate.CustomFormat = "dd-MM-yyyy";
                ShowAllDetails();
                btnClose.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("initVariable() : Error");
            }
        }
        private Boolean AddPRDetailRow()
        {
            Boolean status = true;
            try
            {
                grdPRDetail.Rows.Add();
                int kount = grdPRDetail.RowCount;
                grdPRDetail.Rows[kount - 1].Cells["LineNo"].Value = kount;
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["AccountCode"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["AccountName"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["AmountDebit"].Value = Convert.ToDecimal(0);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["AmountCredit"].Value = Convert.ToDecimal(0);
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["ChequeNo"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["ChequeDate"].Value = DateTime.Parse("1900-01-01");
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["PartyCode"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["PartyName"].Value = "";
                grdPRDetail.Rows[grdPRDetail.RowCount - 1].Cells["SLType"].Value = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("AddPRDetailRow() : Error");
            }

            return status;
        }
        private void ShowAllDetails()
        {
            if(docID== "POPRODUCTINWARD" || docID == "POPRODUCTINWARD")
            {
                try
                {
                    POPIHeaderDB popihdb = new POPIHeaderDB();
                    popiheader vd = popihdb.getPOPIHeader(tempno, tempdate, docID).FirstOrDefault();
                    if (vd != null)
                    {
                        txtDocID.Text = vd.DocumentID;
                        txtTrackingNo.Text = vd.TrackingNo.ToString();
                        dtTrackingDate.Value = vd.TrackingDate;
                        txtrefno.Text = vd.ReferenceNo;
                        cmbCustomer.SelectedIndex =Structures.ComboFUnctions.getComboIndex(cmbCustomer, vd.CustomerID);
                        txtPONo.Text = vd.CustomerPONO;
                        dtPODate.Value = vd.CustomerPODate;
                        dtDeliveryDate.Value = vd.DeliveryDate;
                        cmbOfficeID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbOfficeID, vd.OfficeID);
                        cmbProjectID.SelectedIndex = cmbProjectID.FindString(vd.ProjectID);
                        cmbPaymentMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbPaymentMode, vd.PaymentMode);
                        cmbFreightTerms.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbFreightTerms, vd.FreightTerms);
                        cmbCurrency.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCurrency, vd.CurrencyID);
                        txtFreightCharge.Text = vd.FreightCharge.ToString();
                        dtValidateDate.Value = vd.ValidityDate;
                        txtpaymentTerms.Text = getPaymentTermsExplained( vd.PaymentTerms);
                        txtproductValue.Text = vd.ProductValueINR.ToString();
                        txtTaxvalue.Text = vd.TaxAmountINR.ToString();
                        txtPOvalue.Text = vd.POValueINR.ToString();
                    }
                    popiheader popih = new popiheader();
                    popih.DocumentID = docID;
                    popih.TemporaryNo = tempno;
                    popih.TemporaryDate = tempdate;
                    List<popidetail> POPIDetail = POPIHeaderDB.getPOPIDetail(popih);
                    grdPRDetail.Rows.Clear();
                    int i = 0;
                    double count = 0;
                    foreach (popidetail pop in POPIDetail)
                    {
                        grdPRDetail.Rows.Add();
                        grdPRDetail.Rows[i].Cells["LineNo"].Value = grdPRDetail.Rows.Count;
                        grdPRDetail.Rows[i].Cells["Item"].Value = pop.StockItemID;
                        grdPRDetail.Rows[i].Cells["ItemDesc"].Value = pop.StockItemName;
                        grdPRDetail.Rows[i].Cells["TaxCode"].Value = pop.TaxCode;
                        grdPRDetail.Rows[i].Cells["Quantity"].Value = pop.Quantity;
                        grdPRDetail.Rows[i].Cells["Price"].Value = pop.Price;
                        grdPRDetail.Rows[i].Cells["Tax"].Value = pop.Tax;
                        double valu = pop.Price * pop.Quantity + pop.Tax;
                        grdPRDetail.Rows[i].Cells["Value"].Value = valu;
                        count += valu;
                        i++;
                    }
                    txtTotalValue.Text = count.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in POInward");
                }
            }
            else if(docID== "PURCHASEORDER")
            {
                try
                {

                    PurchaseOrderDB pdb = new PurchaseOrderDB();
                    poheader poh = pdb.getFilteredPurchaseOrderHeaderlist(docID,tempno,tempdate).FirstOrDefault();
                    if(poh != null)
                    {
                        txtDocID.Text = poh.DocumentID;
                        //txtTrackingNo.Text = poh.TrackingNo.ToString();
                        //dtTrackingDate.Value = poh.TrackingDate;
                        //txtrefno.Text = poh.ReferenceNo;
                        cmbCustomer.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCustomer, poh.CustomerID);
                        //txtPONo.Text = poh.CustomerPONO;
                        //dtPODate.Value = poh.CustomerPODate;
                        //dtDeliveryDate.Value = poh.DeliveryDate;
                        //cmbOfficeID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbOfficeID, poh.OfficeID);
                        //cmbProjectID.SelectedIndex = cmbProjectID.FindString(poh.ProjectID);
                        //dtValidateDate.Value = poh.ValidityDate;
                        txtpaymentTerms.Text = getPaymentTermsExplained(poh.PaymentTerms);
                        txtproductValue.Text = poh.ProductValueINR.ToString();
                        txtTaxvalue.Text = poh.TaxAmountINR.ToString();
                        txtPOvalue.Text = poh.POValueINR.ToString();
                    }
                    poheader popd = new poheader();
                    popd.DocumentID = docID;
                    popd.TemporaryNo = tempno;
                    popd.TemporaryDate = tempdate;
                    List<podetail> pod = PurchaseOrderDB.getPurchaseOrderDetails(popd);
                    grdPRDetail.Rows.Clear();
                    int i = 0;
                    double count = 0;
                    foreach (podetail po in pod)
                    {
                        grdPRDetail.Rows.Add();
                        grdPRDetail.Rows[i].Cells["LineNo"].Value = grdPRDetail.Rows.Count;
                        grdPRDetail.Rows[i].Cells["Item"].Value = po.StockItemID;
                        grdPRDetail.Rows[i].Cells["ItemDesc"].Value = po.StockItemName;
                        grdPRDetail.Rows[i].Cells["TaxCode"].Value = po.TaxCode;
                        grdPRDetail.Rows[i].Cells["Quantity"].Value = po.Quantity;
                        grdPRDetail.Rows[i].Cells["Price"].Value = po.Price;
                        grdPRDetail.Rows[i].Cells["Tax"].Value = po.Tax;
                        double valu = po.Price * po.Quantity + po.Tax;
                        grdPRDetail.Rows[i].Cells["Value"].Value = valu;
                        count += valu;
                        i++;
                    }
                    txtTotalValue.Text = count.ToString();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error in POOut");
                }
            }
            else if(docID == "WORKORDER")
            {
                try
                {

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in WorkOrder");
                }
            }
            else if (docID == "POGENERAL")
            {
                try
                {

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in POgeneral");
                }
            }
            else if(docID == "POINVOICEIN" || docID == "WOINVOICEIN" || docID == "POGENERALINVOICEIN")
            {
                try
                {

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in InvoiceIn");
                }
            }
            else if(docID == "PRODUCTINVOICEOUT" || docID == "SERVICEINVOICEOUT")
            {
                try
                {

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error in InvoiceOut");
                }
            }

        }
        private void addGridDetail()
        {
            try
            {
            }
            catch (Exception ex)
            {
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdlist_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

           

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void grdPRDetail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void grdPRDetail_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==0)
            {
                grdPRDetail.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            else
            {
                grdPRDetail.SelectionMode = DataGridViewSelectionMode.CellSelect;
            }
        }


        public static string getPaymentTermsExplained(string ptstr)
        {
            string ptexplainrd = "";
            PTDefinitionDB ptddb = new PTDefinitionDB();
            List<ptdefinition> PTDefinitions = ptddb.getPTDefinitions();
            string[] strArry1 = ptstr.Split(new string[] { ";" }, StringSplitOptions.None);
            for (int i = 0; i < strArry1.Length; i++)
            {
                if (strArry1[i].Trim().Length > 0)
                {
                    string[] strArry2 = strArry1[i].Split(new string[] { "-" }, StringSplitOptions.None);
                    foreach (ptdefinition ptd in PTDefinitions)
                    {
                        if (ptd.SequenceNo == Convert.ToInt32(strArry2[0]))
                        {
                            ptexplainrd = ptexplainrd + ptd.ShortDescription + " : " + strArry2[1] + "%";
                            if (Convert.ToInt32(strArry2[2]) != 0)
                            {
                                ptexplainrd = ptexplainrd + " in " + strArry2[2] + " days";
                            }
                            ptexplainrd = ptexplainrd + "\n";
                            break;
                        }
                    }
                }
            }
            return ptexplainrd;
        }
    }
}
