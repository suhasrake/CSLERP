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

    public partial class ShowInvoiceOut : Form
    {
        string docID;
        int tempno;
        DateTime tempdate;
        string docList = "";
        string officeList = "";
        string trackerList = "";
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        public ShowInvoiceOut(string docid, int tno, DateTime tdate)
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
                //OfficeDB.fillOfficeComboNew(cmbOfficeID);
                ProjectHeaderDB.fillprojectCombo(cmbProject);
                CustomerDB.fillCustomerComboNew(cmbCustomer);
                CurrencyDB.fillCurrencyComboNew(cmbCurrency);
                CatalogueValueDB.fillCatalogValueComboNew(cmbTransportationMode, "TransportationMode");
                CustomerDB.fillLedgerTypeComboNew(cmbTransporter, "Transporter");
                //CatalogueValueDB.fillCatalogValueCombo(cmbPaymentTerms, "TermsOfPayment");
                CatalogueValueDB.fillCatalogValueComboNew(cmbOriginCountryID, "Country");
                // CatalogueValueDB.fillCatalogValueCombo(cmbInvoiceType, "POType");
                CatalogueValueDB.fillCatalogValueComboNew(cmbFinalDestinationCountryID, "Country");
                CatalogueValueDB.fillCatalogValueComboNew(cmbPreCarriageTransMode, "TransportationMode");
                // CatalogueValueDB.fillCatalogValueCombo(cmbFinalDestinationCountryID, "Country");
                //CatalogueValueDB.fillCatalogValueCombo(cmbPreCarriageTransMode, "TransportationMode");
                CatalogueValueDB.fillCatalogValueComboNew(cmbADCode, "ADCODES");
                //dtDeliveryDate.Format = DateTimePickerFormat.Custom;
                //dtDeliveryDate.CustomFormat = "dd-MM-yyyy";
                dtInvoiceDate.Format = DateTimePickerFormat.Custom;
                dtInvoiceDate.CustomFormat = "dd-MM-yyyy";
                //dtTrackingDate.Format = DateTimePickerFormat.Custom;
                //dtTrackingDate.CustomFormat = "dd-MM-yyyy";
                //dtValidateDate.Format = DateTimePickerFormat.Custom;
                //dtValidateDate.CustomFormat = "dd-MM-yyyy";
                ShowAllDetails();
                btnClose.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("initVariable() : Error");
            }
        }
        private void ShowAllDetails()
        {

            try
            {
                InvoiceOutHeaderDB pdb = new InvoiceOutHeaderDB();
                invoiceoutheader poh = pdb.getFilteredInvoiceOutHeaderList(docID, tempno, tempdate).FirstOrDefault();
                if (poh != null)
                {
                    hide();
                    txtDocID.Text = poh.DocumentID;
                    txtPOTrackingNo.Text = poh.TrackingNos.ToString();
                    txtPOTrackingdate.Text = poh.TrackingDates;
                    cmbCustomer.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCustomer, poh.ConsigneeID);
                    txtInvoiceNo.Text = poh.InvoiceNo.ToString();
                    dtInvoiceDate.Value = poh.InvoiceDate;
                    cmbCurrency.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCurrency, poh.CurrencyID);
                    cmbTransporter.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbTransporter, poh.Transporter);
                    cmbTransportationMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbTransportationMode, poh.TransportationMode);
                    cmbProject.SelectedIndex = cmbProject.FindString(poh.ProjectID);
                    txtFreightCharge.Text = poh.FreightCharge.ToString();
                    txtpaymentTerms.Text = poh.TermsOfPayment;
                    txtproductValue.Text = poh.ProductValueINR.ToString();
                    txtTaxvalue.Text = poh.TaxAmountINR.ToString();
                    txtFreightForwarding.Text = (poh.FreightCharge * poh.INRConversionRate).ToString();
                    txtInvoiceAmt.Text = poh.InvoiceAmount.ToString();
                    txtBankAccount.Text = poh.BankAcReference.ToString();
                    if ((docID == "PRODUCTEXPORTINVOICEOUT") || (docID == "SERVICEEXPORTINVOICEOUT"))
                    {
                        show();
                        cmbOriginCountryID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbOriginCountryID, poh.OriginCountryID);
                        cmbFinalDestinationCountryID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbFinalDestinationCountryID, poh.FinalDestinatoinCountryID);
                        //cmbFinalDestinationCountryID.SelectedIndex = cmbFinalDestinationCountryID.FindString(previoh.FinalDestinatoinCountryID);
                        txtFinalDestinationPlace.Text = poh.FinalDestinationPlace;
                        //cmbPreCarriageTransMode.SelectedIndex = cmbPreCarriageTransMode.FindString(previoh.PreCarriageTransportationMode);
                        cmbPreCarriageTransMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbPreCarriageTransMode, poh.PreCarriageTransportationMode);
                        txtPrecarrierReceivedPlace.Text = poh.PreCarrierReceiptPlace;
                        txtTermsOfDelivery.Text = poh.TermsOfDelivery;
                        txtEntryPort.Text = poh.EntryPort;
                        txtExitPort.Text = poh.ExitPort;
                        cmbADCode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbADCode, poh.ADCode);
                        //cmbADCode.SelectedIndex = cmbADCode.FindString(previoh.ADCode);
                    }


                }
                invoiceoutheader popd = new invoiceoutheader();
                popd.DocumentID = docID;
                popd.TemporaryNo = tempno;
                popd.TemporaryDate = tempdate;
                List<invoiceoutdetail> pod = InvoiceOutHeaderDB.getInvoiceOutDetail(popd);
                grdPRDetail.Rows.Clear();
                int i = 0;
                double count = 0;
                foreach (invoiceoutdetail po in pod)
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
            catch (Exception ex)
            {
                MessageBox.Show("Error in POOut");
            }
        }

        public void hide()
        {
            cmbOriginCountryID.Visible = false;            
            cmbFinalDestinationCountryID.Visible = false;
            cmbPreCarriageTransMode.Visible = false;
            cmbADCode.Visible = false;
            lbladcode.Visible = false;
            lblFinalDestinationCountryID.Visible = false;
            lblFinalDestinationPlace.Visible = false;
            lblOriginCountryID.Visible = false;
            lblPreCarriageTransportationMode.Visible = false;
            lblTermsOfDelivery.Visible = false;
            txtTermsOfDelivery.Visible = false;
            txtFinalDestinationPlace.Visible = false;
            lblPrecarrierReceiptPlace.Visible = false;
            txtPrecarrierReceivedPlace.Visible = false;
            lblEntryport.Visible = false;
            lblExitport.Visible = false;
            txtEntryPort.Visible = false;
            txtExitPort.Visible = false; 
        }
        public void show()
        {
            cmbOriginCountryID.Visible = true;
            cmbFinalDestinationCountryID.Visible = true;
            cmbPreCarriageTransMode.Visible = true;
            cmbADCode.Visible = true;
            lbladcode.Visible = true;
            lblFinalDestinationCountryID.Visible = true;
            lblFinalDestinationPlace.Visible = true;
            lblOriginCountryID.Visible = true;
            lblPreCarriageTransportationMode.Visible = true;
            lblTermsOfDelivery.Visible = true;
            txtTermsOfDelivery.Visible = true;
            txtFinalDestinationPlace.Visible = true;
            lblPrecarrierReceiptPlace.Visible = true;
            txtPrecarrierReceivedPlace.Visible = true;
            lblEntryport.Visible = true;
            lblExitport.Visible = true;
            txtEntryPort.Visible = true;
            txtExitPort.Visible = true;

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
            if (e.ColumnIndex == 0)
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
