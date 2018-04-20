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

    public partial class ShowPOGeneral : Form
    {
        string docID;
        int tempno;
        DateTime tempdate;
        string docList = "";
        string officeList = "";
        string trackerList = "";
        DocumentReceiverDB drDB = new DocumentReceiverDB();
        public ShowPOGeneral(string docid, int tno, DateTime tdate)
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
                //CatalogueValueDB.fillCatalogValueComboNew(cmbFreightTerms, "Freight");
                dtDeliveryDate.Format = DateTimePickerFormat.Custom;
                dtDeliveryDate.CustomFormat = "dd-MM-yyyy";               
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

        private void ShowAllDetails()
        {
                try
                {
                    PurchaseOrderGeneralDB POGdb = new PurchaseOrderGeneralDB();
                    pogeneralheader vd = POGdb.getFilteredpogeneralheadersList(docID,tempno, tempdate).FirstOrDefault();
                    if (vd != null)
                    {
                        txtDocID.Text = vd.DocumentID;
                        txtTrackingNo.Text = vd.PONo.ToString();
                        dtTrackingDate.Value = vd.PODate;
                        txtrefno.Text = vd.Reference;
                        cmbCustomer.SelectedIndex =Structures.ComboFUnctions.getComboIndex(cmbCustomer, vd.CustomerID);
                        //txtPONo.Text = vd.WORequestNo.ToString();
                        //dtPODate.Value = vd.WORequestDate;
                        dtDeliveryDate.Value = vd.StartDate;
                        cmbOfficeID.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbOfficeID, vd.OfficeID);
                        cmbProjectID.SelectedIndex = cmbProjectID.FindString(vd.ProjectID);
                        cmbPaymentMode.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbPaymentMode, vd.PaymentMode);
                        cmbCurrency.SelectedIndex = Structures.ComboFUnctions.getComboIndex(cmbCurrency, vd.CurrencyID);
                        dtValidateDate.Value = vd.TargetDate;
                        txtpaymentTerms.Text = getPaymentTermsExplained( vd.PaymentTerms);
                        txtproductValue.Text = vd.ServiceValueINR.ToString();
                        txtTaxvalue.Text = vd.TaxAmountINR.ToString();
                        txtPOvalue.Text = vd.TotalAmountINR.ToString();
                    }
                pogeneralheader popih = new pogeneralheader();
                    popih.DocumentID = docID;
                    popih.TemporaryNo = tempno;
                    popih.TemporaryDate = tempdate;
                    List<pogeneraldetail> POPIDetail = PurchaseOrderGeneralDB.getpogeneraldetails(popih);
                    grdPRDetail.Rows.Clear();
                    int i = 0;
                    double count = 0;
                    foreach (pogeneraldetail pop in POPIDetail)
                    {
                        grdPRDetail.Rows.Add();
                        grdPRDetail.Rows[i].Cells["LineNo"].Value = grdPRDetail.Rows.Count;
                        grdPRDetail.Rows[i].Cells["Item"].Value = pop.ServiceItemID;
                        grdPRDetail.Rows[i].Cells["ItemDesc"].Value = pop.Description;
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
