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

    public partial class ShowPurchaseOrder : Form
    {
        string docID;
        int tempno;
        DateTime tempdate;
        string docList = "";
        string officeList = "";
        string trackerList = "";
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        public ShowPurchaseOrder(string docid, int tno, DateTime tdate)
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
                //ProjectHeaderDB.fillprojectCombo(cmbProjectID);
                CustomerDB.fillCustomerComboNew(cmbCustomer);
                CurrencyDB.fillCurrencyComboNew(cmbCurrency);
                CatalogueValueDB.fillCatalogValueComboNew(cmbPaymentMode, "PaymentMode");
                CatalogueValueDB.fillCatalogValueComboNew(cmbFreightTerms, "Freight");
                CatalogueValueDB.fillCatalogValueComboNew(cmbTaxterms, "TaxStatus");
                CatalogueValueDB.fillCatalogValueComboNew(cmbTransportationMode, "TransportationMode");
                //dtDeliveryDate.Format = DateTimePickerFormat.Custom;
                //dtDeliveryDate.CustomFormat = "dd-MM-yyyy";
                dtPODate.Format = DateTimePickerFormat.Custom;
                dtPODate.CustomFormat = "dd-MM-yyyy";
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
                PurchaseOrderDB pdb = new PurchaseOrderDB();
                poheader poh = pdb.getFilteredPurchaseOrderHeaderlist(docID, tempno, tempdate).FirstOrDefault();
                if (poh != null)
                {
                    txtDocID.Text = poh.DocumentID;
                    //txtTrackingNo.Text = poh.TrackingNo.ToString();
                    //dtTrackingDate.Value = poh.TrackingDate;
                    txtDevileryPeriod.Text = poh.DeliveryPeriod.ToString();
                    cmbCustomer.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCustomer, poh.CustomerID);
                    txtPONo.Text = poh.PONo.ToString();
                    dtPODate.Value = poh.PODate;
                    cmbCurrency.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCurrency, poh.CurrencyID);
                    cmbPaymentMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbPaymentMode, poh.ModeOfPayment);
                    cmbFreightTerms.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbFreightTerms, poh.FreightTerms);
                    cmbTaxterms.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbTaxterms, poh.TaxTerms);
                    cmbTransportationMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbTransportationMode, poh.TransportationMode);
                    txtValidityperiod.Text = poh.validityPeriod.ToString();
                    txtFreightCharge.Text = poh.FreightCharge.ToString();
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
            catch (Exception ex)
            {
                MessageBox.Show("Error in POOut");
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
