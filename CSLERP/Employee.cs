using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSLERP.DBData;
using System.IO;
using System.Drawing.Drawing2D;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.Collections;
using CSLERP.FileManager;
using System.Text.RegularExpressions;

namespace CSLERP
{
    public partial class Employee : System.Windows.Forms.Form
    {
        ////public static string connString;
        int EMPid = 0;
        string EMPName = "";
        string DocID = "EMPLOYEE";
        Panel pnlPDFViewer = new Panel();
        Button btnListDocuments = new Button();
        Button btnPDFClose = new Button();
        Panel pnllv = new Panel();// panel for listview

        ListView lv = new ListView();
        ListView exlv = new ListView();// list view for choice / selection list
        DataGridView dgv = new DataGridView();
        DataGridView dgv1 = new DataGridView();
        DataGridView dgvPayRev = new DataGridView();
        Form frm = new Form();
        Form frm1 = new Form();
        Form frm2 = new Form();

        string Rowid = "";
        string drowid = "";
        int payrevRowID = 0;
        Form dtpForm = new Form();
        public static string[,] empStatusValues;
        DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
        DataGridViewButtonColumn btn1 = new DataGridViewButtonColumn();
        //DateTIme global variable for preveious Edit
        DateTime prevPostDate = DateTime.MinValue;
        DateTime nextPostDate = DateTime.MinValue;
        DateTime prevPayRevDate = DateTime.MinValue;
        DateTime nextPayRevDate = DateTime.MinValue;
        public Employee()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {

            }
        }
        string imagelocation = "";

        private void Employee_Load(object sender, EventArgs e)
        {
            txtMailID.AutoSize = true;
            Region = System.Drawing.Region.FromHrgn(Utilities.CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            initVariables();
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Width -= 100;
            this.Height -= 100;
            String a = this.Size.ToString();
            grdList.ColumnHeadersDefaultCellStyle.BackColor = Color.LightBlue;
            grdList.EnableHeadersVisualStyles = false;
            ListEmployee();
            privilagepostdesig();
        }
        private void ListEmployee()
        {
            try
            {
                this.Controls["pnlUI"].Controls["processPanel"].Visible = true;
                this.Refresh();
                byte[] empPic;
                grdList.Rows.Clear();
                txtSearch.Text = "";
                EmployeeDB dbrecord = new EmployeeDB();
                List<employee> Employees = dbrecord.getEmployees();
                foreach (employee emp in Employees)
                {
                    //try
                    //{
                    //    empPic = Convert.FromBase64String(emp.empPicture);
                    //}
                    //catch (Exception)
                    //{
                    //    empPic = null;
                    //}
                    grdList.Rows.Add();
                    grdList.Rows[grdList.RowCount - 1].Cells["empSlno"].Value = grdList.RowCount;
                    grdList.Rows[grdList.RowCount - 1].Cells["gEmpID"].Value = emp.empID;
                    grdList.Rows[grdList.RowCount - 1].Cells["gEmpName"].Value = emp.empName;
                    grdList.Rows[grdList.RowCount - 1].Cells["empGender"].Value = emp.Gender;
                    //grdList.Rows[grdList.RowCount - 1].Cells["empImage"].Value = empPic;
                    grdList.Rows[grdList.RowCount - 1].Cells["empDOB"].Value = emp.empDOB;//.ToString("dd-MM-yyyy");
                    grdList.Rows[grdList.RowCount - 1].Cells["empDOJ"].Value = emp.empDOJ;//.ToString("dd-MM-yyyy");
                    grdList.Rows[grdList.RowCount - 1].Cells["empDesignation"].Value = emp.designation;
                    grdList.Rows[grdList.RowCount - 1].Cells["empDepartment"].Value = emp.department;
                    grdList.Rows[grdList.RowCount - 1].Cells["empRegion"].Value = emp.region;
                    grdList.Rows[grdList.RowCount - 1].Cells["empReportingOfficer"].Value = emp.reportingofficer;
                    grdList.Rows[grdList.RowCount - 1].Cells["empOffice"].Value = emp.office;
                    grdList.Rows[grdList.RowCount - 1].Cells["EmailID"].Value = emp.empEmailID;
                    grdList.Rows[grdList.RowCount - 1].Cells["empPhoneNo"].Value = emp.empPhoneNo;
                    grdList.Rows[grdList.RowCount - 1].Cells["empStatus"].Value = emp.empStatus;
                }
                this.Controls["pnlUI"].Controls["processPanel"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }

            pnlList.Visible = true;
            enableBottomButtons();
        }


        private void initVariables()
        {
            try
            {

                empStatusValues = new string[4, 2]
                        {
                    {"0","Approval Pending" },
                    {"1","Active" },
                    {"8","Resigned" },
                    {"9","Terminated" }
                        };
                dtEmpDOB.Format = DateTimePickerFormat.Custom;
                dtEmpDOB.CustomFormat = "dd-MM-yyyy";
                dtEmpDOJ.Format = DateTimePickerFormat.Custom;
                dtEmpDOJ.CustomFormat = "dd-MM-yyyy";
                fillEmployeeStatusCombo(cmbEmpStatus);
                dtPostingDate.Format = DateTimePickerFormat.Custom;
                dtPostingDate.CustomFormat = "dd-MM-yyyy";
                dtPostingPrevious.Format = DateTimePickerFormat.Custom;
                dtPostingPrevious.CustomFormat = "dd-MM-yyyy";
                dtDesignation.Format = DateTimePickerFormat.Custom;
                dtDesignation.CustomFormat = "dd-MM-yyyy";
                dtPostingPrevious.Value = DateTime.Parse("1990-01-01");
                dtDesignation.Value = DateTime.Parse("1990-01-01");
                txtFixedpay.Text = "0";
                txtVarPay.Text = "0";
                txtVariablePercentage.Text = "100";
                dtPrevRevDate.Format = DateTimePickerFormat.Custom;
                dtPrevRevDate.CustomFormat = "dd-MM-yyyy";
                dtrevDate.Format = DateTimePickerFormat.Custom;
                dtrevDate.CustomFormat = "dd-MM-yyyy";
                dtPrevRevDate.Value = DateTime.Parse("1990-01-01");
                ComboFIll.fillStatusCombo(cmbPostingStatus);
                ComboFIll.fillStatusCombo(cmbdStatus);
                ComboFIll.fillStatusCombo(cmbPayRevStatus);
                OfficeDB.fillOfficeComboNew(cmbPostingOffice);

                CatalogueValueDB.fillCatalogValueComboNew(cmbPostingDepartment, "Department");
                CatalogueValueDB.fillCatalogValueComboNew(cmbDesignation, "Designation");
                CatalogueValueDB.fillCatalogValueComboNew(cmbGender, "Gender");

                pnlOuter.Parent = pnlUI;
                pnlPayRevOuter.Parent = pnlUI;
                pnlOuterRole.Parent = pnlUI;
                pnlPostingOuter.Parent = pnlUI;
                pnlDesignationOuter.Parent = pnlUI;
                pnlQualOuter.Parent = pnlUI;
                pnlExpOuter.Parent = pnlUI;

                ListViewRoleSetUp();
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
        private void privilagepostdesig()
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
                    grdList.Columns["LoadDocument"].Visible = true;
                }
                else
                {
                    grdList.Columns["Edit"].Visible = false;
                    grdList.Columns["LoadDocument"].Visible = false;
                    desig();
                    Postng();
                }
            }
            catch (Exception)
            {
            }
        }
        public void desig()
        {
            dtDesignation.Visible = false;
            cmbDesignation.Visible = false;
            txtDremark.Visible = false;
            cmbdStatus.Visible = false;
            btnDescriptionSave.Visible = false;
            lblDateofDasignation.Visible = false;
            lblDesignation.Visible = false;
            lblRemarks.Visible = false;
            cmbdStatus.Visible = false;
            btnDescriptionCancel.Location = new Point(114, 159);
            pnlDesignationInner.Size = new Size(515, 189);
            pnlDesignationOuter.Size = new Size(544, 228);
        }
        public void Postng()
        {
            dtPostingDate.Visible = false;
            cmbPostingOffice.Visible = false;
            cmbPostingDepartment.Visible = false;
            txtReportingOfficer.Visible = false;
            btnselectRepOfficer.Visible = false;
            txtRemarks.Visible = false;
            cmbPostingStatus.Visible = false;
            label13.Visible = false;
            label12.Visible = false;
            label15.Visible = false;
            label16.Visible = false;
            label9.Visible = false;
            label8.Visible = false;
            btnPostingSave.Visible = false;
            btnPostingCancel.Location = new Point(142, 157);
            pnlPostingInner.Size = new Size(515, 186);
            pnlPostingOuter.Size = new Size(544, 218);
        }



        private void closeAllPanels()
        {
            try
            {
                pnlInner.Visible = false;
                pnlOuter.Visible = false;
                pnlList.Visible = false;
                pnlPostingOuter.Visible = false;
                pnlDesignationOuter.Visible = false;
                pnlDesignationInner.Visible = false;
                pnlPayRevOuter.Visible = false;
                pnlPayRevInner.Visible = false;
                pnlOuterRole.Visible = false;
                pnlInnerRole.Visible = false;
                pnlQualOuter.Visible = false;
                pnlQualInner.Visible = false;
                pnlExpOuter.Visible = false;
                pnlExpInner.Visible = false;
                pnlList.Visible = false;
                removeControlsFromPnlLvPanel();
            }
            catch (Exception)
            {

            }
        }



        private void fillEmployeeStatusCombo(System.Windows.Forms.ComboBox cmb)
        {
            try
            {
                cmb.Items.Clear();
                for (int i = 0; i < empStatusValues.GetLength(0); i++)
                {
                    cmb.Items.Add(empStatusValues[i, 1]);
                }
            }
            catch (Exception)
            {

            }
        }

        public void clearEmpData()
        {
            try
            {
                txtEmpID.Text = "";
                txtEmpName.Text = "";
                dtEmpDOB.Value = DateTime.Parse("1990-01-01");
                dtEmpDOJ.Value = DateTime.Parse("1990-01-01");
                txtEmpPhoneNo.Text = "";
                empPicture.Image = null;
                txtMailID.Text = "";
                cmbGender.SelectedIndex = -1;
                cmbEmpStatus.SelectedIndex = 1;
                removeControlsFromPnlLvPanel();
            }
            catch (Exception)
            {

            }

        }
        public void clearPostingEmpData()
        {
            try
            {
                prevPostDate = DateTime.MinValue;
                nextPostDate = DateTime.MinValue;
                txtPostingEmpID.Text = "";
                txtPostingEmpName.Text = "";
                txtReportingOfficer.Text = "";
                txtRemarks.Text = "";
                dtPostingDate.Value = DateTime.Parse("1990-01-01");
                dtPostingPrevious.Value = DateTime.Parse("1990-01-01");
                cmbPostingDepartment.SelectedIndex = -1;
                cmbPostingOffice.SelectedIndex = -1;
                cmbPostingStatus.SelectedIndex = -1;
                removeControlsFromPnlLvPanel();
            }
            catch (Exception)
            {

            }

        }
        public void clearDescEmpData()
        {
            try
            {
                txtDempID.Text = "";
                txtDempname.Text = "";
                txtDremark.Text = "";
                dtDesignation.Value = DateTime.Parse("1990-01-01");
                dtPostingPrevious.Value = DateTime.Parse("1990-01-01");
                cmbDesignation.SelectedIndex = -1;
                cmbdStatus.SelectedIndex = -1;
                removeControlsFromPnlLvPanel();
            }
            catch (Exception)
            {

            }

        }
        public void clearPayrevData()
        {
            try
            {
                //txtCTC.Text = "";
                //txtCurrentpay.Text = "";
                prevPayRevDate = DateTime.MinValue;
                nextPayRevDate = DateTime.MinValue;
                txtPayRevEmpName.Text = "";
                txtpayRevEmpID.Text = "";
                dtrevDate.Value = DateTime.Parse("1990-01-01");
                dtPrevRevDate.Value = DateTime.Parse("1990-01-01");
                cmbPayRevStatus.SelectedIndex = -1;

                txtFixedpay.Text = "0";
                txtVarPay.Text = "0";
                txtVariablePercentage.Text = "100";
                removeControlsFromPnlLvPanel();
            }
            catch (Exception)
            {

            }

        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                clearEmpData();
                btnSave.Text = "Save";
                pnlOuter.Visible = true;
                pnlInner.Visible = true;
                txtEmpID.Enabled = true;
                disableBottomButtons();
            }
            catch (Exception)
            {

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                clearEmpData();
                closeAllPanels();
                pnlList.Visible = true;
                enableBottomButtons();
            }
            catch (Exception)
            {

            }
        }
        private void lnkEmpPhoto_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "jpeg|*.jpg|bmp|*.bmp|all files|*.*";
            DialogResult res = openFileDialog1.ShowDialog();
            if (res == DialogResult.OK)
            {
                imagelocation = openFileDialog1.FileName.ToString();
                empPicture.ImageLocation = imagelocation;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                //Image img = empPicture.Image();
                //byte[] arr;
                //ImageConverter converter = new ImageConverter();
                //arr = (byte[])converter.ConvertTo(img, typeof(byte[]));
                Boolean isEmail = Regex.IsMatch(txtMailID.Text, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
                if (!isEmail)
                {
                    MessageBox.Show("Check Mail ID");
                    return;
                }
                MemoryStream ms = new MemoryStream();
                empPicture.Image.Save(ms, ImageFormat.Jpeg);
                byte[] photo = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(photo, 0, photo.Length);
                string strPicture = Convert.ToBase64String(photo);
                ////command.CommandText = "INSERT INTO ImagesTable (Image) VALUES('" + photo + "')";
                ////command.CommandType = CommandType.Text;
                ////command.ExecuteNonQuery();

                employee emp = new employee();
                EmployeeDB empDB = new EmployeeDB();

                emp.empID = Convert.ToInt32(txtEmpID.Text);
                emp.empName = txtEmpName.Text;
                emp.Gender = ((Structures.ComboBoxItem)cmbGender.SelectedItem).HiddenValue;
                emp.empDOB = dtEmpDOB.Value;
                emp.empDOJ = dtEmpDOJ.Value;
                emp.empPhoneNo = txtEmpPhoneNo.Text;
                ////////emp.empPhoto = lnkEmpPhoto.Text;
                emp.empPicture = strPicture;
                emp.empStatus = empDB.getEmpStatusCode(cmbEmpStatus.SelectedItem.ToString());
                emp.empEmailID = txtMailID.Text.Trim();
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;

                if (empDB.validateEmployee(emp))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (empDB.updateEmployee(emp))
                        {
                            MessageBox.Show("Employee data updated");
                            closeAllPanels();
                            ListEmployee();

                        }
                        else
                        {
                            MessageBox.Show("Failed to update Employee Data");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (empDB.insertEmployee(emp))
                        {
                            MessageBox.Show("Employee data Added");
                            closeAllPanels();
                            ListEmployee();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert Employee Data");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Employee Data Validation Failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Employee Data Validation Failed. Check Input Info.");
            }
        }

        private void grdList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.RowIndex < 0)
                    return;
                string columnName = grdList.Columns[e.ColumnIndex].Name;
                txtEmpID.Text = grdList.Rows[e.RowIndex].Cells["gEmpID"].Value.ToString();
                txtEmpName.Text = grdList.Rows[e.RowIndex].Cells["gEmpName"].Value.ToString();
                if (columnName.Equals("Edit"))
                {
                    int rowID = e.RowIndex;
                    btnSave.Text = "Update";
                    pnlInner.Visible = true;
                    pnlOuter.Visible = true;
                    pnlList.Visible = false;
                    cmbEmpStatus.SelectedIndex = cmbEmpStatus.FindStringExact(grdList.Rows[e.RowIndex].Cells[5].Value.ToString());
                    DataGridViewRow row = grdList.Rows[rowID];


                    //tempDate = grdEmployeeList.Rows[e.RowIndex].Cells[2].Value.ToString();
                    dtEmpDOB.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["empDOB"].Value.ToString());
                    dtEmpDOJ.Value = DateTime.Parse(grdList.Rows[e.RowIndex].Cells["empDOJ"].Value.ToString());
                    cmbGender.SelectedIndex =
                       Structures.ComboFUnctions.getComboIndex(cmbGender, grdList.Rows[e.RowIndex].Cells["empGender"].Value.ToString());
                    txtEmpPhoneNo.Text = grdList.Rows[e.RowIndex].Cells["empPhoneNo"].Value.ToString();
                    txtMailID.Text = grdList.Rows[e.RowIndex].Cells["EmailID"].Value.ToString();
                    try
                    {
                        //byte[] data = (byte[])grdList.Rows[e.RowIndex].Cells["EmpImage"].Value;
                        byte[] data = EmployeeDB.getPictureOfEmployee(txtEmpID.Text);
                        MemoryStream ms = new MemoryStream(data);
                        empPicture.Image = Image.FromStream(ms);
                    }
                    catch (Exception)
                    {
                        empPicture.Image = null;
                    }

                    empPicture.Visible = true;
                    empPicture.Refresh();
                    txtEmpID.Enabled = false;
                    disableBottomButtons();
                }
                if (columnName.Equals("LoadDocument"))
                {
                    int rowID = e.RowIndex;
                    employee ee = new employee();
                    ee.empID = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gEmpID"].Value);
                    ee.empName = grdList.Rows[e.RowIndex].Cells["gEmpName"].Value.ToString();
                    string hdrString = "Employee ID:" + ee.empID + "\n" +
                            "Employee Name:" + ee.empName;
                    string dicDir = Main.documentDirectory + "\\" + DocID;
                    string subDir = ee.empID + "-";
                    FileManager.LoadDocuments load = new FileManager.LoadDocuments(dicDir, subDir, DocID, hdrString);
                    load.ShowDialog();
                    this.RemoveOwnedForm(load);
                    //btnCancel_Click_1(null, null);
                    return;
                }
                if (columnName.Equals("ViewDocument"))
                {
                    int rowID = e.RowIndex;
                    employee ee = new employee();
                    EMPid = Convert.ToInt32(grdList.Rows[e.RowIndex].Cells["gEmpID"].Value);
                    EMPName = grdList.Rows[e.RowIndex].Cells["gEmpName"].Value.ToString();
                    ShowViewDocumentPanel();
                    grdList.Visible = false;
                    pnlBottomButtons.Visible = false;
                    pnlList.Visible = false;
                }
                if (columnName.Equals("Posting"))
                {
                    enableposting();
                    btnPostingSave.Text = "Save";
                    pnlPostingOuter.Visible = true;
                    txtPostingEmpID.Text = grdList.Rows[e.RowIndex].Cells["gEmpID"].Value.ToString();
                    txtPostingEmpName.Text = grdList.Rows[e.RowIndex].Cells["gEmpName"].Value.ToString();
                    try
                    {
                        dtPostingPrevious.Value = EmployeePostingDB.getLastPostingDate(txtPostingEmpID.Text);
                    }
                    catch (Exception)
                    {
                    }
                    pnlList.Visible = false;
                    disableBottomButtons();
                }
                if (columnName.Equals("Designation"))
                {
                    enableDesignation();
                    btnDescriptionSave.Text = "Save";
                    pnlDesignationOuter.Visible = true;
                    pnlDesignationInner.Visible = true;

                    txtDempID.Text = grdList.Rows[e.RowIndex].Cells["gEmpID"].Value.ToString();
                    txtDempname.Text = grdList.Rows[e.RowIndex].Cells["gEmpName"].Value.ToString();
                    pnlList.Visible = false;

                    disableBottomButtons();
                }
                if (columnName.Equals("PayRevision"))
                {
                    clearPayrevData();
                    dgvPayRevEnable();
                    btnPayRevInfoSave.Text = "Save";
                    pnlPayRevOuter.Visible = true;
                    pnlPayRevInner.Visible = true;
                    dtrevDate.Value = DateTime.Now;
                    EmployeePayRevision empPay = new EmployeePayRevision();
                    txtpayRevEmpID.Text = grdList.Rows[e.RowIndex].Cells["gEmpID"].Value.ToString();
                    txtPayRevEmpName.Text = grdList.Rows[e.RowIndex].Cells["gEmpName"].Value.ToString();
                    try
                    {
                        dtPrevRevDate.Value = EmployeeDB.getlastEmpPayRevDate(txtpayRevEmpID.Text);
                    }
                    catch (Exception ex)
                    {
                        dtPrevRevDate.Value = DateTime.Parse("1990-01-01");
                    }
                    pnlList.Visible = false;
                    disableBottomButtons();
                }
                if (columnName.Equals("Role"))
                {

                    try
                    {
                        closeAllPanels();
                        pnlOuterRole.Visible = true;
                        pnlInnerRole.Visible = true;
                        string[] roles = new string[0];
                        txtEmpIdRole.Text = grdList.Rows[e.RowIndex].Cells["gEmpID"].Value.ToString();
                        txtEmpNameRole.Text = grdList.Rows[e.RowIndex].Cells["gEmpName"].Value.ToString();
                        String strRole = EmployeeDB.getEmployeeRolesList(txtEmpIdRole.Text);
                        if (strRole.Length != 0)
                        {
                            roles = strRole.Split(Main.delimiter1);
                        }
                        CatalogueValueDB catvalDB = new CatalogueValueDB();
                        List<cataloguevalue> CatalogueValues = catvalDB.getCatalogueValues();

                        lvRole.Items.Clear();
                        foreach (cataloguevalue catval in CatalogueValues)
                        {
                            if (catval.catalogueID.Equals("EmployeeRole") && catval.status == 1)
                            {
                                ListViewItem item1 = new ListViewItem();

                                item1.SubItems.Add(catval.catalogueValueID.ToString());
                                item1.SubItems.Add(catval.description);
                                if (strRole.Contains(catval.catalogueValueID))
                                    item1.Checked = true;
                                else
                                    item1.Checked = false;
                                lvRole.Items.Add(item1);
                            }
                        }


                        grdList.Visible = false;
                        pnlUI.Visible = true;
                        disableBottomButtons();
                    }
                    catch (Exception ex)
                    {
                    }
                }

                if (columnName.Equals("Qualification"))
                {
                    closeAllPanels();
                    //-------------
                    //get customer bank details
                    EmployeeDB empdb = new EmployeeDB();
                    List<EmployeeQualification> EmployeeQualification =
                        EmployeeDB.getEmployeeQualification(txtEmpID.Text);
                    txtQualEmpID.Text = grdList.Rows[e.RowIndex].Cells["gEmpID"].Value.ToString();
                    txtQualEmpName.Text = grdList.Rows[e.RowIndex].Cells["gEmpName"].Value.ToString();
                    dgvQualification.Rows.Clear();
                    int i = 0;
                    foreach (EmployeeQualification empq in EmployeeQualification)
                    {
                        dgvQualification.Rows.Add();
                        var BtnCell = (DataGridViewButtonCell)dgvQualification.Rows[i].Cells[3];
                        BtnCell.Value = "Del";
                        DataGridViewComboBoxCell ComboColumn =
                            (DataGridViewComboBoxCell)(dgvQualification.Rows[i].Cells[0]);
                        //string firstValue = CatalogueValueDB.fillCatalogValueGridViewCombo(ComboColumn, "Qualification");
                        //DataGridViewComboBoxColumn col = (DataGridViewComboBoxColumn)dgvQualification.Columns[0];
                        //col.Items.clear
                        CatalogueValueDB.fillCatalogValueGridViewComboNew(ComboColumn, "Qualification");
                        //int c = col.Items.Count;
                        dgvQualification.Rows[i].Cells[0].Value = empq.QualificationID;//+ "-" + empq.QualificationName;
                        dgvQualification.Rows[i].Cells[1].Value = empq.Year;
                        dgvQualification.Rows[i].Cells[2].Value = empq.Remarks;

                        DataGridViewComboBoxCell ComboColumn1 = new DataGridViewComboBoxCell();

                        i++;
                    }
                    //-------------
                    pnlQualOuter.Visible = true;
                    pnlQualInner.Visible = true;
                }
                if (columnName.Equals("Experience"))
                {
                    closeAllPanels();

                    //-------------
                    //get customer bank details
                    EmployeeDB empdb = new EmployeeDB();
                    txtExpEmpID.Text = grdList.Rows[e.RowIndex].Cells["gEmpID"].Value.ToString();
                    txtExpEmpName.Text = grdList.Rows[e.RowIndex].Cells["gEmpName"].Value.ToString();
                    List<EmployeeExperience> employeeexperience =
                        EmployeeDB.getEmployeeExperience(txtEmpID.Text);
                    dgvExperience.Rows.Clear();

                    int i = 0;
                    //int c = dgvExperience.Rows.Count;
                    foreach (EmployeeExperience empe in employeeexperience)
                    {
                        dgvExperience.Rows.Add();
                        //var BtnCell = (DataGridViewButtonCell)dgvExperience.Rows[i].Cells[5];
                        //BtnCell.Value = "Del";
                        dgvExperience.Rows[i].Cells["StartDate"].Value = empe.startdate;
                        dgvExperience.Rows[i].Cells["EndDate"].Value = empe.enddate;
                        dgvExperience.Rows[i].Cells["Organisation"].Value = empe.employer;
                        dgvExperience.Rows[i].Cells["PostHeld"].Value = empe.postheld;
                        dgvExperience.Rows[i].Cells["expRemarks"].Value = empe.Remarks;

                        i++;
                    }
                    //-------------
                    pnlExpOuter.Visible = true;
                    pnlExpInner.Visible = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void ListViewRoleSetUp()
        {
            lvRole.View = View.Details;
            lvRole.LabelEdit = true;
            lvRole.AllowColumnReorder = true;
            lvRole.CheckBoxes = true;
            lvRole.FullRowSelect = true;
            lvRole.GridLines = true;
            lvRole.Sorting = System.Windows.Forms.SortOrder.Ascending;
            lvRole.Columns.Add("Select", -2, HorizontalAlignment.Left);
            lvRole.Columns.Add("Catalouge ID", -2, HorizontalAlignment.Left);
            lvRole.Columns.Add("Role Description", -2, HorizontalAlignment.Left);
            lvRole.Columns[0].Width = 60;
            lvRole.Columns[1].Width = 200;
            lvRole.Columns[2].Width = 200;
        }
        private void removeControlsFromPnlLvPanel()
        {
            try
            {
                //foreach (Control p in pnllv.Controls)
                //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(System.Windows.Forms.Button))
                //    {
                //        p.Dispose();
                //    }
                pnllv.Controls.Clear();
                Control nc = pnllv.Parent;
                nc.Controls.Remove(pnllv);
            }
            catch (Exception ex)
            {
            }
        }
        private void ShowViewDocumentPanel()
        {
            try
            {
                pnlPDFViewer = new Panel();
                pnlPDFViewer.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
                pnlPDFViewer.BorderStyle = BorderStyle.FixedSingle;
                pnlPDFViewer.Location = new System.Drawing.Point(80, 50);
                pnlPDFViewer.Name = "pnlPDFViewer";
                pnlPDFViewer.Size = new System.Drawing.Size(930, 450);

                Label lblEmpID = new Label();
                lblEmpID.Location = new System.Drawing.Point(40, 50);
                lblEmpID.Size = new Size(100, 30);
                lblEmpID.Text = "Employee ID";
                lblEmpID.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                pnlPDFViewer.Controls.Add(lblEmpID);

                Label lblEmpName = new Label();
                lblEmpName.Location = new System.Drawing.Point(40, 85);
                lblEmpName.Size = new Size(100, 30);
                lblEmpName.Text = "Employee Name";
                lblEmpName.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                pnlPDFViewer.Controls.Add(lblEmpName);

                TextBox empid = new TextBox();
                TextBox empname = new TextBox();
                empid.Location = new Point(150, 50);
                empid.Size = new Size(50, 30);
                empid.Enabled = false;
                empid.Text = EMPid.ToString();
                empid.BorderStyle = BorderStyle.None;
                empname.BorderStyle = BorderStyle.None;

                empname.Text = EMPName.ToString();
                empname.Location = new Point(150, 85);
                empname.Size = new Size(300, 30);
                empid.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                empname.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                empid.Enabled = false;
                pnlPDFViewer.Controls.Add(empid);
                pnlPDFViewer.Controls.Add(empname);


                Button btnPanelClose = new Button();
                btnPanelClose.Location = new System.Drawing.Point(40, 420);
                btnPanelClose.Size = new System.Drawing.Size(93, 23);
                //btnPDFClose.TabIndex = 2;
                btnPanelClose.Text = "Close Panel";
                btnPanelClose.UseVisualStyleBackColor = true;
                btnPanelClose.Click += new System.EventHandler(this.btnPanelClose_Click);
                removePDFFileGridView();
                removePDFControls();
                DataGridView dgvDocumentList = new DataGridView();
                pnlPDFViewer.Controls.Remove(dgvDocumentList);
                dgvDocumentList = DocumentStorageDB.getDocumentDetails(DocID, EMPid + "-");
                dgvDocumentList.Size = new Size(860, 270);
                if (getuserPrivilegeStatus() == 1)
                {
                    dgvDocumentList.Columns[4].Visible = false;
                    dgvDocumentList.Columns[5].Visible = false;
                }
                dgvDocumentList.Location = new System.Drawing.Point(40, 120);
                pnlPDFViewer.Controls.Add(dgvDocumentList);
                dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);

                pnlPDFViewer.Controls.Add(dgvDocumentList);
                pnlPDFViewer.Controls.Add(btnPanelClose);

                pnlPDFViewer.Visible = true;
                this.Controls.Add(pnlPDFViewer);
                pnlPDFViewer.BringToFront();
                //showfilegridview();

            }
            catch (Exception ex)
            {
            }
        }
        //private void showfilegridview()
        //{
        //    try
        //    {

        //        DataGridView dgvDocumentList = new DataGridView();
        //        pnlPDFViewer.Controls.Remove(dgvDocumentList);
        //        dgvDocumentList = DocumentStorageDB.getDocumentDetails(DocID, EMPid + "-");
        //        dgvDocumentList.Size = new Size(810, 270);
        //        dgvDocumentList.Location = new System.Drawing.Point(40, 120);
        //        pnlPDFViewer.Controls.Add(dgvDocumentList);
        //        dgvDocumentList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDocumentList_CellContentClick);

        //        pnlPDFViewer.Controls.Add(dgvDocumentList);


        //        pnlPDFViewer.Visible = true;
        //        this.Controls.Add(pnlPDFViewer);
        //        pnlPDFViewer.BringToFront();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        private void btnPanelClose_Click(object sender, EventArgs e)
        {
            pnlPDFViewer.Visible = false;
            this.Controls.Remove(pnlPDFViewer);
            grdList.Visible = true;
            pnlList.Visible = true;
            pnlBottomButtons.Visible = true;
        }
        private void removePDFFileGridView()
        {
            try
            {
                foreach (Control p in pnlPDFViewer.Controls)
                    if (p.GetType() == typeof(DataGridView))
                    {
                        p.Dispose();
                    }
            }
            catch (Exception ex)
            {
            }
        }
        private void removePDFControls()
        {
            try
            {
                foreach (Control p in pnlPDFViewer.Controls)
                    if (p.GetType() == typeof(AxAcroPDFLib.AxAcroPDF))
                    {
                        AxAcroPDFLib.AxAcroPDF c = (AxAcroPDFLib.AxAcroPDF)p;
                        c.Visible = false;
                        pnlPDFViewer.Controls.Remove(c);
                        c.Dispose();
                    }
            }
            catch (Exception ex)
            {
            }
        }
        private void showPDFFileGrid()
        {
            try
            {
                foreach (Control p in pnlPDFViewer.Controls)
                    if (p.GetType() == typeof(DataGridView))
                    {
                        p.Visible = true;
                    }
            }
            catch (Exception ex)
            {
            }
        }
        private void removeControlsFromPDFViewer()
        {
            try
            {
                pnlPDFViewer.Controls.Clear();
                Control nc = pnlPDFViewer.Parent;
                nc.Controls.Remove(pnlPDFViewer);
            }
            catch (Exception ex)
            {
            }
        }
        private void dgvDocumentList_CellContentClick(Object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ////this.Controls["pnlUI"].Controls["processPanel"].Visible = true;
                ////prPanel.Visible = true;
                this.Refresh();
                DataGridView dgv = sender as DataGridView;
                string fileName = "";
                if (e.RowIndex < 0)
                    return;
                if (e.ColumnIndex == 0)
                {
                    removePDFControls();
                    fileName = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    ////string docDir = Main.documentDirectory + "\\" + docID;
                    string subDir = EMPid + "-";
                    dgv.Enabled = false;

                    ////System.Threading.Thread.Sleep(5000);
                    fileName = DocumentStorageDB.createFileFromDB(DocID, subDir, fileName);
                    System.Diagnostics.Process.Start(fileName);

                    dgv.Enabled = true;
                }
                if (e.ColumnIndex == 4)
                {

                    Panel pnlview = new Panel();
                    pnlview.Location = new System.Drawing.Point(150, 150);
                    pnlview.Size = new Size(450, 180);
                    //pnlview.BackColor = Color.Gray;
                    pnlview.BorderStyle = BorderStyle.FixedSingle;
                    pnlPDFViewer.Controls.Add(pnlview);
                    pnlview.Visible = true;
                    dgv.Visible = false;

                    Label lbl = new Label();
                    lbl.Location = new Point(10, 20);
                    lbl.Size = new Size(100, 45);
                    lbl.Text = "Edit Description";
                    lbl.Font = new Font("Microsoft Sans Serif", 10, FontStyle.Regular);
                    pnlview.Controls.Add(lbl);

                    RichTextBox rtcview = new RichTextBox();
                    rtcview.Location = new Point(120, 20);
                    rtcview.Size = new Size(250, 80);
                    pnlview.Controls.Add(rtcview);
                    rtcview.Text = dgv.Rows[e.RowIndex].Cells["Description"].Value.ToString();

                    Button btn1 = new Button();
                    btn1.Location = new Point(120, 120);
                    btn1.Size = new Size(70, 23);
                    btn1.Text = "Cancel";
                    pnlview.Controls.Add(btn1);
                    btn1.Click += (s, t) =>
                    {
                        pnlview.Visible = false;
                        rtcview.Text = "";
                        dgv.Visible = true;
                    };

                    Button btnupdate = new Button();
                    btnupdate.Location = new Point(200, 120);
                    btnupdate.Size = new Size(70, 23);
                    btnupdate.Text = "Update";
                    pnlview.Controls.Add(btnupdate);


                    btnupdate.Click += (s, t) =>
                    {
                        try
                        {
                            documentStorage ds = new documentStorage();
                            DocumentStorageDB dsdb = new DocumentStorageDB();
                            ds.Description = rtcview.Text;
                            ds.DocumentID = dgv.Rows[e.RowIndex].Cells["docID"].Value.ToString();
                            ds.DocumentSubID = dgv.Rows[e.RowIndex].Cells["docsubID"].Value.ToString();
                            ds.FileName = dgv.Rows[e.RowIndex].Cells["FileName"].Value.ToString();
                            DialogResult dialog = MessageBox.Show("Do You Want to update the Document Description?", "Yes", MessageBoxButtons.YesNo);
                            if (dialog == DialogResult.Yes)
                            {
                                if (rtcview.Text.Trim().Length != 0)
                                {
                                    if (dsdb.UpdateDocumentDescription(ds))
                                    {
                                        MessageBox.Show("Document Updated Sucessfuly");
                                        rtcview.Text = "";
                                        dgv.Visible = true;
                                        pnlview.Visible = false;
                                        removeControlsFromPDFViewer();
                                        ShowViewDocumentPanel();
                                    }
                                    else
                                    {
                                        MessageBox.Show("Failed to Update Document");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Please Enter Some Description");
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }


                    };

                    pnlview.Visible = true;
                    pnlPDFViewer.Controls.Add(pnlview);
                    pnlview.BringToFront();
                    pnlview.Focus();
                }
                if (e.ColumnIndex == 5)
                {
                    int rowid = Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["RowID"].Value);
                    DialogResult dialog = MessageBox.Show("Are you sure to to delete the Document?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        if (DocumentStorageDB.deleteDocument(rowid))
                        {
                            MessageBox.Show("Deleted Sucessfully.");
                            dgv.Rows.RemoveAt(e.RowIndex);
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                ////dform.Close();
                ////this.Controls["pnlUI"].Controls["processPanel"].Visible = false;
            }
            catch (Exception ex)
            {
            }
            ////prPanel.Visible = false;
        }

        private void btnviewcancel_click(Object sender, DataGridViewCellEventArgs e)
        {
        }
        private void EmployeebtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                removeControlsFromPnlLvPanel();
                pnllv = new Panel();
                pnllv.BorderStyle = BorderStyle.FixedSingle;

                pnllv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(100, 100), new Size(600, 300));
                exlv = Utilities.GridColumnSelectionView(grdList);
                exlv.Bounds = new System.Drawing.Rectangle(new System.Drawing.Point(50, 50), new Size(500, 200));
                foreach (ListViewItem lvitem in exlv.Items)
                {
                    if (lvitem.SubItems[1].Text == "Photo")
                    {
                        exlv.Items.Remove(lvitem);
                    }
                    if (lvitem.SubItems[1].Text == "Image")
                    {
                        exlv.Items.Remove(lvitem);
                    }
                    pnllv.Controls.Add(exlv);
                }

                System.Windows.Forms.Label pnlHeading = new System.Windows.Forms.Label();
                pnlHeading.Size = new Size(300, 20);
                pnlHeading.Location = new System.Drawing.Point(50, 20);
                pnlHeading.Text = "Select Gridview Colums to Export";
                pnlHeading.Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                pnlHeading.ForeColor = Color.Black;
                pnllv.Controls.Add(pnlHeading);

                System.Windows.Forms.Button exlvOK = new System.Windows.Forms.Button();
                exlvOK.Text = "OK";
                exlvOK.Location = new System.Drawing.Point(50, 270);
                exlvOK.Click += new System.EventHandler(this.exlvOK_Click1);
                pnllv.Controls.Add(exlvOK);

                System.Windows.Forms.Button exlvCancel = new System.Windows.Forms.Button();
                exlvCancel.Text = "Cancel";
                exlvCancel.Location = new System.Drawing.Point(150, 270);
                exlvCancel.Click += new System.EventHandler(this.exlvCancel_Click2);
                pnllv.Controls.Add(exlvCancel);

                pnlList.Controls.Add(pnllv);
                pnllv.BringToFront();
                pnllv.Visible = true;
            }
            catch (Exception ex)
            {
            }
        }
        private void exlvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                removeControlsFromPnlLvPanel();
                pnllv.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void exlvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                pnllv.Visible = false;
                string heading1 = "Employee List";
                string heading2 = "";
                Utilities.export2Excel(heading1, heading2, "", grdList, exlv);
            }
            catch (Exception ex)
            {
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void disableBottomButtons()
        {
            btnNew.Visible = false;
            btnExit.Visible = false;
            btnExport.Visible = false;
        }
        private void enableBottomButtons()
        {
            btnNew.Visible = true;
            btnExit.Visible = true;
            btnExport.Visible = true;
        }

        private void btnPostingCancel_Click(object sender, EventArgs e)
        {
            try
            {
                clearPostingEmpData();
                closeAllPanels();
                pnlList.Visible = true;
                enableBottomButtons();
                privilagepostdesig();
            }
            catch (Exception)
            {

            }
        }
        private void btnQualCancel_Click(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                pnlList.Visible = true;
                enableBottomButtons();
            }
            catch (Exception)
            {

            }
        }
        private void btnPostingSave_Click(object sender, EventArgs e)
        {
            try
            {
                employeeposting empposting = new employeeposting();
                EmployeePostingDB empPostingDB = new EmployeePostingDB();
                empposting.empID = Convert.ToInt32(txtPostingEmpID.Text);
                empposting.postingDate = dtPostingDate.Value;
                ////////empposting.officeID = cmbPostingOffice.SelectedItem.ToString().Trim().Substring(0, cmbPostingOffice.SelectedItem.ToString().Trim().IndexOf('-'));
                empposting.officeID = ((Structures.ComboBoxItem)cmbPostingOffice.SelectedItem).HiddenValue;
                ////////empposting.departmentID = cmbPostingDepartment.SelectedItem.ToString().Trim().Substring(0, cmbPostingDepartment.SelectedItem.ToString().Trim().IndexOf('-'));
                empposting.departmentID = ((Structures.ComboBoxItem)cmbPostingDepartment.SelectedItem).HiddenValue;
                empposting.remarks = txtRemarks.Text;
                empposting.Status = ComboFIll.getStatusCode(cmbPostingStatus.SelectedItem.ToString());
                empposting.ReportingOfficer = txtReportingOfficer.Text.Substring(0, txtReportingOfficer.Text.IndexOf('-'));
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (empPostingDB.validateEmployeePosting(empposting))
                {
                    if (btnPostingSave.Text == "Update")
                    {
                        if (empposting.postingDate > prevPostDate && empposting.postingDate < nextPostDate)
                        {
                            string status = empposting.Status.ToString();
                            if (EmployeePostingDB.updateEmpPosting(Rowid, empposting))
                            {
                                MessageBox.Show("Status Updated");
                                clearPostingEmpData();
                                closeAllPanels();
                                ListEmployee();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Check Posting Date");
                        }
                    }
                    else
                    {
                        if (empposting.postingDate > dtPostingPrevious.Value)
                        {
                            if (empPostingDB.insertEmployeePosting(empposting))
                            {
                                MessageBox.Show("Employee Posting data Added");
                                clearPostingEmpData();
                                closeAllPanels();
                                ListEmployee();
                            }
                            else
                            {
                                MessageBox.Show("Failed to Insert EmployeePosting Data");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Posting Date Error");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Employee Posting Data Validation Failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Employee Posting Data Validation Failed");
            }
        }

        private void btnShowPreviousData_Click(object sender, EventArgs e)
        {
            try
            {
                removeControlsFromPnlLvPanel();
                frm = new Form();
                frm.Size = new Size(900, 300);
                dgv = new DataGridView();
                List<employeeposting> EmployeePosting = EmployeePostingDB.getEmployeePosting(txtPostingEmpID.Text);
                if (EmployeePosting.Count == 0)
                {
                    MessageBox.Show("Posting not specified for this employee");
                    return;
                }
                dgv.DataSource = EmployeePosting;

                dgv.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_DataBindingComplete);
                dgv.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellContentClick);
                dgv.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgv_ColumnAdded);
                dgv.RowHeadersVisible = false;
                dgv.Size = new Size(850, 250);
                dgv.ReadOnly = true;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                //dgv.Columns["postingDate"].DefaultCellStyle.Format = "dd-MM-yyyy";
                frm.Controls.Add(dgv);
                frm.Text = "Posting details of " + txtPostingEmpName.Text;
                frm.MaximizeBox = false;
                frm.MinimizeBox = false;
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        private void dgv_ColumnAdded(Object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column.Name.Equals("postingDate"))
            {
                e.Column.DefaultCellStyle.Format = "dd-MM-yyyy";
            }
            if (e.Column.Name.Equals("createTime"))
            {
                e.Column.DefaultCellStyle.Format = "dd-MM-yyyy HH:mm:ss";
            }
        }
        private void dgv_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (getuserPrivilegeStatus() != 1)
            {
                DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                btn.Name = "Edit";
                btn.HeaderText = "Edit";
                btn.Text = "Edit";
                btn.UseColumnTextForButtonValue = true;
                btn.DefaultCellStyle.BackColor = Color.Red;
                //btn.
                //btn.Width = 300;
                dgv.Columns.Add(btn);
            }
        }
        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0)
                return;
            string colName = dgv.Columns[e.ColumnIndex].Name;
            if (colName.Equals("Edit"))
            {
                // if()
                int rowID = e.RowIndex;
                btnPostingSave.Text = "Update";

                try
                {
                    prevPostDate = DateTime.Parse(dgv.Rows[e.RowIndex - 1].Cells["postingDate"].Value.ToString());
                }
                catch (Exception ex)
                {
                    prevPostDate = DateTime.MinValue;
                }
                try
                {
                    nextPostDate = DateTime.Parse(dgv.Rows[e.RowIndex + 1].Cells["postingDate"].Value.ToString());
                }
                catch (Exception ex)
                {
                    nextPostDate = DateTime.Now;
                }

                Rowid = dgv.Rows[e.RowIndex].Cells["RowID"].Value.ToString();
                txtPostingEmpID.Text = dgv.Rows[e.RowIndex].Cells["empID"].Value.ToString();
                txtPostingEmpName.Text = dgv.Rows[e.RowIndex].Cells["empName"].Value.ToString();
                dtPostingDate.Value = DateTime.Parse(dgv.Rows[e.RowIndex].Cells["postingDate"].Value.ToString());
                txtRemarks.Text = dgv.Rows[e.RowIndex].Cells["remarks"].Value.ToString();
                ////////cmbPostingOffice.SelectedIndex = cmbPostingOffice.FindString(dgv.Rows[e.RowIndex].Cells["officeID"].Value.ToString());
                cmbPostingOffice.SelectedIndex =
                       Structures.ComboFUnctions.getComboIndex(cmbPostingOffice, dgv.Rows[e.RowIndex].Cells["officeID"].Value.ToString());
                ////////cmbPostingDepartment.SelectedIndex = cmbPostingDepartment.FindString(dgv.Rows[e.RowIndex].Cells["departmentID"].Value.ToString());
                cmbPostingDepartment.SelectedIndex =
                       Structures.ComboFUnctions.getComboIndex(cmbPostingDepartment, dgv.Rows[e.RowIndex].Cells["departmentID"].Value.ToString());
                txtReportingOfficer.Text = dgv.Rows[e.RowIndex].Cells["ReportingOfficer"].Value.ToString() + "-" + dgv.Rows[e.RowIndex].Cells["ReportingOfficerName"].Value.ToString();
                cmbPostingStatus.SelectedIndex = cmbPostingStatus.FindString(dgv.Rows[e.RowIndex].Cells["Status"].Value.ToString());
                //dgvclose();
                frm.Close();
            }

        }
        public void enableposting()
        {
            btnselectRepOfficer.Enabled = true;
            txtRemarks.Enabled = true;
            txtReportingOfficer.Enabled = true;
            cmbPostingOffice.Enabled = true;
            cmbPostingDepartment.Enabled = true;
            dtPostingDate.Enabled = true;
        }
        public void enableDesignation()
        {
            txtDremark.Enabled = true;
            cmbDesignation.Enabled = true;
            dtDesignation.Enabled = true;
        }
        public void dgvclose()
        {
            //btnselectRepOfficer.Enabled = false;
            txtRemarks.Enabled = false;
            txtReportingOfficer.Enabled = false;
            cmbPostingOffice.Enabled = false;
            cmbPostingDepartment.Enabled = false;
            dtPostingDate.Enabled = false;
            frm.Close();
        }
        public void dgv1close()
        {
            //txtDremark.Enabled = false;
            //cmbDesignation.Enabled = false;
            //dtDesignation.Enabled = false;
            frm1.Close();
        }

        private void pnlInner_Paint(object sender, PaintEventArgs e)
        {

        }

        private void grdList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

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
        private void btnselectRepOfficer_Click(object sender, EventArgs e)
        {
            //btnselectRepOfficer.Enabled = false;
            removeControlsFromPnlLvPanel();
            pnllv = new Panel();
            pnllv.BorderStyle = BorderStyle.FixedSingle;
            pnllv.BackColor = Color.DarkSeaGreen;
            pnllv.Location = new Point(34, 68);
            pnllv.Size = new Size(466, 245);

            lv = EmployeePostingDB.getEmployeeListView();
            lv.Sorting = System.Windows.Forms.SortOrder.None;
            lv.ColumnClick += new ColumnClickEventHandler(LvColumnClick);
            lv.Location = new Point(13, 9);
            lv.Size = new Size(440, 199);
            //lv.ListViewItemSorter = new ListViewItemComparer();
            pnllv.Controls.Add(lv);

            Button lvOK = new Button();
            lvOK.Text = "OK";
            lvOK.Location = new Point(38, 215);
            lvOK.Click += new System.EventHandler(this.lvOK_Click1);
            pnllv.Controls.Add(lvOK);

            Button lvCancel = new Button();
            lvCancel.Text = "Cancel";
            lvCancel.Location = new Point(155, 215);
            lvCancel.Click += new System.EventHandler(this.lvCancel_Click2);
            pnllv.Controls.Add(lvCancel);

            pnlPostingInner.Controls.Add(pnllv);
            pnllv.BringToFront();
            pnllv.Visible = true;
        }
        private void lvOK_Click1(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                        count++;
                }
                if (count != 1)
                {
                    MessageBox.Show("Select one item");
                    return;
                }
                //btnselectRepOfficer.Enabled = true;
                pnllv.Visible = false;
                foreach (ListViewItem itemRow in lv.Items)
                {
                    if (itemRow.Checked)
                    {
                        txtReportingOfficer.Text = itemRow.SubItems[1].Text + "-" + itemRow.SubItems[2].Text;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void lvCancel_Click2(object sender, EventArgs e)
        {
            try
            {
                //btnselectRepOfficer.Enabled = true;
                pnllv.Visible = false;
            }
            catch (Exception)
            {
            }
        }
        private void LvColumnClick(object o, ColumnClickEventArgs e)
        {
            try
            {
                string first = lv.Items[0].SubItems[e.Column].Text;
                string last = lv.Items[lv.Items.Count - 1].SubItems[e.Column].Text;
                System.Windows.Forms.SortOrder sort_order1 = SortingListView.getSortedOrder(first, last);
                this.lv.ListViewItemSorter = new SortingListView(e.Column, sort_order1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorting error");
            }
        }
        //private void removeControlsFromlvPanel()
        //{
        //    try
        //    {
        //        //foreach (Control p in pnllv.Controls)
        //        //    if (p.GetType() == typeof(ListView) || p.GetType() == typeof(Button))
        //        //    {
        //        //        p.Dispose();
        //        //    }
        //        pnllv.Controls.Clear();
        //        Control nc = pnllv.Parent;
        //        nc.Controls.Remove(pnllv);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        private void btnDescriptionSave_Click(object sender, EventArgs e)
        {
            try
            {

                employee emp = new employee();
                EmployeeDB empDB = new EmployeeDB();
                emp.empID = Convert.ToInt32(txtDempID.Text);
                emp.Descdate = dtDesignation.Value;
                ////////emp.designation = cmbDesignation.SelectedItem.ToString().Trim().Substring(0, cmbDesignation.SelectedItem.ToString().Trim().IndexOf('-'));
                emp.designation = ((Structures.ComboBoxItem)cmbDesignation.SelectedItem).HiddenValue;
                emp.remarks = txtDremark.Text;
                emp.status = getDescStatusCode(cmbdStatus.SelectedItem.ToString());
                System.Windows.Forms.Button btn = sender as System.Windows.Forms.Button;
                string btnText = btn.Text;
                if (empDB.validateEmployeeDesc(emp))
                {
                    if (btnDescriptionSave.Text == "Update")
                    {
                        string stat = emp.status.ToString();
                        if (EmployeeDB.updateEmpDesignation(drowid, emp))
                        {
                            MessageBox.Show("Designation Updated");
                            clearDescEmpData();
                            closeAllPanels();
                            ListEmployee();
                        }
                        else
                        {
                            MessageBox.Show("Update failed");
                        }
                    }
                    else
                    {
                        if (empDB.insertEmployeeDesignation(emp))
                        {
                            MessageBox.Show("Employee Designation data Added");
                            clearDescEmpData();
                            closeAllPanels();
                            ListEmployee();
                        }
                        else
                        {
                            MessageBox.Show("Failed to Insert EmployeeDesignation Data");
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Employee Designation Data Validation Failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Validation failed");
            }

        }

        private void cmbDesignation_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbdStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbEmpStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            //////////int selectedIndex = cmbEmpStatus.SelectedIndex;
            //////////int selecteVal = (int)cmbEmpStatus.SelectedValue;
        }

        private void btnDescriptionCancel_Click(object sender, EventArgs e)
        {
            try
            {
                clearDescEmpData();
                closeAllPanels();
                pnlList.Visible = true;
                enableBottomButtons();
                privilagepostdesig();
            }
            catch (Exception)
            {

            }
        }

        private void dtPostingDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            frm1 = new Form();
            frm1.Size = new Size(820, 300);
            dgv1 = new DataGridView();
            List<employeeDesignation> EmployeeDesig = new List<employeeDesignation>();
            EmployeeDesig = EmployeeDB.getEmployeeDesignation(txtDempID.Text);
            if (EmployeeDesig.Count == 0)
            {
                MessageBox.Show("No Designation assigned till now");
                return;
            }
            dgv1.DataSource = EmployeeDesig;
            dgv1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv1_DataBindingComplete);
            dgv1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv1_CellContentClick);
            dgv1.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgv1_ColumnAdded);
            dgv1.RowHeadersVisible = false;
            dgv1.Size = new Size(790, 250);
            dgv1.ReadOnly = true;
            dgv1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            frm1.Controls.Add(dgv1);
            frm1.Text = "Designation details of " + txtDempname.Text;
            frm1.MaximizeBox = false;
            frm1.MinimizeBox = false;
            frm1.ShowDialog();
            //   dgv1.Columns.Remove(btn1);
        }
        private void dgv1_ColumnAdded(Object sender, DataGridViewColumnEventArgs e)
        {
            if (e.Column.Name.Equals("Descdate"))
            {
                e.Column.DefaultCellStyle.Format = "dd-MM-yyyy";
            }
            if (e.Column.Name.Equals("createtime"))
            {
                e.Column.DefaultCellStyle.Format = "dd-MM-yyyy HH:mm:ss";
            }
            if (e.Column.Name.Equals("status"))
            {
                //e.Column.
            }
        }
        private void dgv1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (getuserPrivilegeStatus() != 1)
            {
                DataGridViewButtonColumn btn1 = new DataGridViewButtonColumn();
                btn1.Name = "Edit";
                btn1.HeaderText = "Edit";
                btn1.Text = "Edit";
                btn1.UseColumnTextForButtonValue = true;
                btn1.DefaultCellStyle.BackColor = Color.Red;
                dgv1.Columns.Add(btn1);
            }
        }
        private void dgv1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0)
                return;
            string colName = dgv1.Columns[e.ColumnIndex].Name;
            if (colName.Equals("Edit"))
            {
                int rowID = e.RowIndex;
                btnDescriptionSave.Text = "Update";
                drowid = dgv1.Rows[e.RowIndex].Cells["RowID"].Value.ToString();
                txtDempID.Text = dgv1.Rows[e.RowIndex].Cells["empID"].Value.ToString();
                //txtDempname.Text = grdList.Rows[e.RowIndex].Cells["empName"].Value.ToString();
                dtDesignation.Value = DateTime.Parse(dgv1.Rows[e.RowIndex].Cells["Descdate"].Value.ToString());
                txtDremark.Text = dgv1.Rows[e.RowIndex].Cells["remarks"].Value.ToString();
                ////////cmbDesignation.SelectedIndex = cmbDesignation.FindString(dgv1.Rows[e.RowIndex].Cells["designation"].Value.ToString());
                cmbDesignation.SelectedIndex =
                        Structures.ComboFUnctions.getComboIndex(cmbDesignation, dgv1.Rows[e.RowIndex].Cells["designation"].Value.ToString());
                cmbdStatus.SelectedIndex = cmbdStatus.FindString(getDescStatusString(Convert.ToInt32(dgv1.Rows[e.RowIndex].Cells["status"].Value)));
                dgv1close();
            }

        }
        private string getDescStatusString(int stat)
        {
            string status = "";
            if (stat == 1)
                status = "Active";
            else
                status = "Deactive";
            return status;
        }
        private int getDescStatusCode(string stat)
        {
            int code = 0;
            if (stat.Equals("Active"))
                code = 1;
            else
                code = 0;
            return code;
        }
        private void pnlList_Paint(object sender, PaintEventArgs e)
        {

        }



        private void txtMailID_MouseHover(object sender, EventArgs e)
        {
            TextBox TB = (TextBox)sender;
            int VisibleTime = 1000;  //in milliseconds

            ToolTip tt = new ToolTip();
            tt.Show(txtMailID.Text, TB, 20, 20, VisibleTime);
        }

        private void pnlPayRevInner_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnPayRevInfoSave_Click(object sender, EventArgs e)
        {
            try
            {
                string btnText = btnPayRevInfoSave.Text;
                EmployeePayRevision empPay = new EmployeePayRevision();
                EmployeeDB empDB = new EmployeeDB();
                empPay.empID = txtpayRevEmpID.Text;
                empPay.RevisionDate = dtrevDate.Value;
                empPay.FixPay = Convert.ToDecimal(txtFixedpay.Text);

                try
                {
                    empPay.VariablePay = Convert.ToDecimal(txtVarPay.Text);
                }
                catch (Exception ex)
                {
                    empPay.VariablePay = 0;
                }
                try
                {
                    empPay.VPPercentage = Convert.ToInt32(txtVariablePercentage.Text);
                }
                catch (Exception ex)
                {
                    empPay.VPPercentage = 0;
                }
                empPay.status = ComboFIll.getStatusCode(cmbPayRevStatus.SelectedItem.ToString());
                if (empDB.validateEmpPayRev(empPay))
                {
                    if (btnText.Equals("Update"))
                    {
                        if (dtrevDate.Value.Date > prevPayRevDate.Date && dtrevDate.Value.Date < nextPayRevDate.Date)
                        {
                            empPay.rowId = payrevRowID;
                            if (empDB.updateEmployeePayRev(empPay))
                            {
                                MessageBox.Show("Employee PayRevision data updated");
                                closeAllPanels();
                                ListEmployee();

                            }
                            else
                            {
                                MessageBox.Show("Failed to update Employee Data");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Check Pay Revision Date");
                        }
                    }
                    else if (btnText.Equals("Save"))
                    {
                        if (dtrevDate.Value.Date > dtPrevRevDate.Value.Date)
                        {
                            if (empDB.insertEmployeePayRev(empPay))
                            {
                                MessageBox.Show("Employee PayRevision data Added");
                                closeAllPanels();
                                ListEmployee();
                            }
                            else
                            {
                                MessageBox.Show("Failed to insert Employee PayRevision Data");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Check Revision Date");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Employee PayRevision Data Validation Failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Employee PayRevision Data Validation Failed");
            }
        }

        private void btnPayRevInfoCancel_Click(object sender, EventArgs e)
        {
            clearPayrevData();
            pnlPayRevInner.Visible = false;
            pnlPayRevOuter.Visible = false;
            closeAllPanels();
            //ListEmployee();
            pnlList.Visible = true;
            grdList.Visible = true;
            enableBottomButtons();
        }

        private void btnShowPrevPayRev_Click(object sender, EventArgs e)
        {
            //clearPayrevData();
            frm2 = new Form();
            frm2.Size = new Size(820, 300);
            dgvPayRev = new DataGridView();
            List<EmployeePayRevision> empPayRevList = new List<EmployeePayRevision>();
            empPayRevList = EmployeeDB.getEmployeePayRevDetail(txtpayRevEmpID.Text);
            if (empPayRevList.Count == 0)
            {
                MessageBox.Show("No previous Pay Revision Updated");
                return;
            }
            //clearPayrevData();
            dgvPayRev.DataSource = empPayRevList;
            dgvPayRev.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvPayRev_DataBindingComplete);
            dgvPayRev.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayRev_CellContentClick);
            dgvPayRev.RowHeadersVisible = false;
            dgvPayRev.Size = new Size(800, 250);
            dgvPayRev.ReadOnly = true;
            dgvPayRev.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            frm2.Controls.Add(dgvPayRev);
            frm2.Text = "Designation details of " + txtDempname.Text;
            frm2.MaximizeBox = false;
            frm2.MinimizeBox = false;
            frm2.ShowDialog();
        }
        private void dgvPayRev_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (getuserPrivilegeStatus() != 1)
            {
                DataGridViewButtonColumn btn1 = new DataGridViewButtonColumn();
                btn1.Name = "Edit";
                btn1.HeaderText = "Edit";
                btn1.Text = "Edit";
                btn1.UseColumnTextForButtonValue = true;
                btn1.DefaultCellStyle.BackColor = Color.Red;
                dgvPayRev.Columns.Add(btn1);
            }
        }
        private void dgvPayRev_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0)
                return;
            string colName = dgvPayRev.Columns[e.ColumnIndex].Name;
            if (colName.Equals("Edit"))
            {
                clearPayrevData();
                int rowID = e.RowIndex;
                btnPayRevInfoSave.Text = "Update";
                try
                {
                    prevPayRevDate = DateTime.Parse(dgvPayRev.Rows[e.RowIndex - 1].Cells["RevisionDate"].Value.ToString());
                }
                catch (Exception ex)
                {
                    prevPayRevDate = DateTime.MinValue;
                }
                try
                {
                    nextPayRevDate = DateTime.Parse(dgvPayRev.Rows[e.RowIndex + 1].Cells["RevisionDate"].Value.ToString());
                }
                catch (Exception ex)
                {
                    nextPayRevDate = DateTime.Now;
                }
                payrevRowID = Convert.ToInt32(dgvPayRev.Rows[e.RowIndex].Cells["RowID"].Value.ToString());
                txtpayRevEmpID.Text = dgvPayRev.Rows[e.RowIndex].Cells["empID"].Value.ToString();
                txtPayRevEmpName.Text = dgvPayRev.Rows[e.RowIndex].Cells["empName"].Value.ToString();
                dtrevDate.Value = DateTime.Parse(dgvPayRev.Rows[e.RowIndex].Cells["RevisionDate"].Value.ToString());
                txtFixedpay.Text = dgvPayRev.Rows[e.RowIndex].Cells["FixPay"].Value.ToString();
                txtVarPay.Text = dgvPayRev.Rows[e.RowIndex].Cells["VariablePay"].Value.ToString();
                txtVariablePercentage.Text = dgvPayRev.Rows[e.RowIndex].Cells["VPPercentage"].Value.ToString();
                //cmbDesignation.SelectedIndex = cmbDesignation.FindString(dgv1.Rows[e.RowIndex].Cells["designation"].Value.ToString());
                cmbPayRevStatus.SelectedIndex =
                    cmbPayRevStatus.FindString(getStatusString(Convert.ToInt32(dgvPayRev.Rows[e.RowIndex].Cells["status"].Value.ToString())));
                dgvPayRevDisable();
            }

        }
        public void dgvPayRevDisable()
        {
            //txtFixedpay.Enabled = false;
            //txtVarPay.Enabled = false;
            //txtVariablePercentage.Enabled = false;
            //txtCurrentpay.Enabled = false;
            //txtCTC.Enabled = false;
            //cmbDesignation.Enabled = false;
            //dtrevDate.Enabled = false;
            frm2.Close();
        }
        public void dgvPayRevEnable()
        {
            txtFixedpay.Enabled = true;
            txtVarPay.Enabled = true;
            txtVariablePercentage.Enabled = true;
            dtrevDate.Enabled = true;
            //txtCTC.Enabled = true;
            //cmbDesignation.Enabled = false;
            //dtPrevRevDate.Enabled = false;
            //frm2.Close();
        }
        private string getStatusString(int stat)
        {
            string status = "";
            if (stat == 1)
                status = "Active";
            else
                status = "Deactive";
            return status;
        }
        private void txtVarPay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //txtCTC.Text = "0";
                if (txtVarPay.Text.Length != 0 && txtFixedpay.Text.Length != 0)
                {
                    decimal fp = Convert.ToDecimal(txtFixedpay.Text);
                    decimal vp = Convert.ToDecimal(txtVarPay.Text);
                    txtCTC.Text = Convert.ToString(fp + vp);
                    txtVariablePercentage_TextChanged(sender, e);
                }
                else if (txtVarPay.Text.Length == 0 && txtFixedpay.Text.Length == 0)
                {
                    decimal fp = 0;
                    decimal vp = 0;
                    txtCTC.Text = Convert.ToString(fp + vp);
                    txtVariablePercentage_TextChanged(sender, e);
                }
                else if (txtVarPay.Text.Length == 0)
                {
                    decimal fp = Convert.ToDecimal(txtFixedpay.Text);
                    decimal vp = 0;
                    txtCTC.Text = Convert.ToString(fp + vp);
                    txtVariablePercentage_TextChanged(sender, e);
                }
                else
                {
                    decimal fp = 0;
                    decimal vp = Convert.ToDecimal(txtVarPay.Text);
                    txtCTC.Text = Convert.ToString(fp + vp);
                    txtVariablePercentage_TextChanged(sender, e);
                }
                //txtFixedpay_TextChanged(sender, e);
            }
            catch (Exception)
            {
                MessageBox.Show("Fill Correct Value in Fixed pay or Variable pay");
            }
        }

        private void txtVariablePercentage_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //txtCurrentpay.Text = "0";
                decimal fp, vp, percnt;
                if (txtVarPay.Text.Length != 0 && txtFixedpay.Text.Length != 0 && txtVariablePercentage.Text.Length != 0)
                {
                    fp = Convert.ToDecimal(txtFixedpay.Text);
                    vp = Convert.ToDecimal(txtVarPay.Text);
                    percnt = Convert.ToInt32(txtVariablePercentage.Text);
                    txtCurrentpay.Text = Convert.ToString(fp + (vp * percnt / 100));
                }
                else
                {
                    try
                    {
                        fp = Convert.ToDecimal(txtFixedpay.Text);
                    }
                    catch (Exception)
                    {
                        fp = 0;
                    }
                    try
                    {
                        vp = Convert.ToDecimal(txtVarPay.Text);
                    }
                    catch (Exception)
                    {
                        vp = 0;
                    }
                    try
                    {
                        percnt = Convert.ToInt32(txtVariablePercentage.Text);
                    }
                    catch (Exception)
                    {
                        percnt = 0;
                    }
                    txtCurrentpay.Text = Convert.ToString(fp + (vp * percnt / 100));
                }

                // txtFixedpay_TextChanged(sender, e);
            }
            catch (Exception)
            {
                MessageBox.Show("Fill Correct Value in Fixed pay or Variable pay or Percntage");
            }
        }

        private void txtFixedpay_TextChanged(object sender, EventArgs e)
        {
            ////txtVariablePercentage_TextChanged(sender, e);
            txtVarPay_TextChanged(sender, e);
        }

        private void pnlPayRevOuter_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSaveRole_Click(object sender, EventArgs e)
        {
            EmployeeRole role = new EmployeeRole();
            EmployeeDB empDb = new EmployeeDB();

            try
            {
                role.empID = txtEmpIdRole.Text;
                role.EmpRoles = getRoleString();

                if (empDb.insertEmployeeRoles(role))
                {
                    MessageBox.Show("Role Saved Sucessfully");
                    pnlInnerRole.Visible = false;
                    pnlOuterRole.Visible = false;
                    pnlList.Visible = true;
                    grdList.Visible = true;
                    enableBottomButtons();
                }
                else
                {
                    MessageBox.Show("Failed To Save Employee Role");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed To Save Employee Role");
            }

        }
        private String getRoleString()
        {
            String str = "";
            foreach (ListViewItem item in lvRole.Items)
            {
                if (item.Checked)
                {
                    str = str + item.SubItems[1].Text + Main.delimiter1;
                }
            }
            return str;
        }
        private void btnCancelRole_Click(object sender, EventArgs e)
        {
            pnlInnerRole.Visible = false;
            pnlOuterRole.Visible = false;
            pnlList.Visible = true;
            grdList.Visible = true;
            enableBottomButtons();
        }

        private void btnAddQual_Click(object sender, EventArgs e)
        {
            dgvQualification.Rows.Add();
            int kount = dgvQualification.RowCount;

            DataGridViewComboBoxCell ComboColumn = (DataGridViewComboBoxCell)(dgvQualification.Rows[kount - 1].Cells[0]);
            ComboColumn.DropDownWidth = 250;
            CatalogueValueDB.fillCatalogValueGridViewComboNew(ComboColumn, "Qualification");
            dgvQualification.Rows[kount - 1].Cells[2].Value = "";
        }

        private void dgvQualification_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            string columnName = dgvQualification.Columns[e.ColumnIndex].Name;
            if (columnName.Equals("Delete"))
            {
                try
                {
                    //delete row
                    DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        dgvQualification.Rows.RemoveAt(e.RowIndex);
                    }
                }
                catch (Exception)
                {

                }
            }
        }
        private Boolean validateQualDetail(EmployeeQualification empqual, List<EmployeeQualification> list)
        {
            Boolean stat = true;
            foreach (EmployeeQualification qual in list)
            {
                if (qual.QualificationID.Equals(empqual.QualificationID))
                {
                    return false;
                }
            }
            return stat;
        }
        private void btnQualSave_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeeQualification empqualrec;
                List<EmployeeQualification> EmployeeQualification = new List<EmployeeQualification>();
                for (int i = 0; i < dgvQualification.Rows.Count; i++)
                {
                    try
                    {
                        empqualrec = new EmployeeQualification();
                        string qdetails = dgvQualification.Rows[i].Cells[0].Value.ToString();
                        string year = dgvQualification.Rows[i].Cells[1].Value.ToString();
                        string remarks = dgvQualification.Rows[i].Cells[2].Value.ToString();
                        empqualrec.empID = txtEmpID.Text;
                        empqualrec.QualificationID = qdetails;
                        empqualrec.Year = Convert.ToInt32(year);
                        empqualrec.Remarks = remarks;
                        if (empqualrec.QualificationID.Trim().Length > 0 && empqualrec.Year > 0
                                && validateQualDetail(empqualrec, EmployeeQualification) && empqualrec.Year < DateTime.Now.Year)
                        {
                            EmployeeQualification.Add(empqualrec);
                        }
                        else
                        {
                            MessageBox.Show("Validation Failed At row:" + (i + 1));
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        ////////MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error - Please verify data entered");
                        ////return;
                    }
                }
                //CustomerBankDetailsDB customerbankdetailsdb = new CustomerBankDetailsDB();
                if (EmployeeQualification.Count == 0)
                {
                    MessageBox.Show("validation failed");
                    return;
                }
                if (!EmployeeDB.updateEmployeeQualification(txtEmpID.Text, EmployeeQualification))
                {
                    MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                }
                else
                {
                    try
                    {
                        MessageBox.Show("Employee Qualification updated");
                        closeAllPanels();
                        pnlList.Visible = true;
                        enableBottomButtons();
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        private void btnAddExp_Click(object sender, EventArgs e)
        {

            dgvExperience.Rows.Add();
            dgvExperience.Rows[dgvExperience.RowCount - 1].Cells["StartDate"].Value = DateTime.Today;
            dgvExperience.Rows[dgvExperience.RowCount - 1].Cells["EndDate"].Value = DateTime.Today;
            int kount = dgvExperience.RowCount;
            //var BtnCell = (DataGridViewButtonCell)dgvExperience.Rows[kount - 1].Cells[5];
            //BtnCell.Value = "Del";
        }
        DateTimePicker oDateTimePicker = new DateTimePicker();
        private void dgvExperience_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            string columnName = dgvExperience.Columns[e.ColumnIndex].Name;
            if (columnName.Equals("expDelete"))
            {
                try
                {
                    //delete row
                    DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
                    if (dialog == DialogResult.Yes)
                    {
                        dgvExperience.Rows.RemoveAt(e.RowIndex);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            if (columnName.Equals("SD") || columnName.Equals("ED"))
            {
                DateTime dt = DateTime.Today;
                if (columnName.Equals("SD"))
                {
                    dt = Convert.ToDateTime(dgvExperience.Rows[e.RowIndex].Cells["StartDate"].Value);
                }
                else
                {
                    dt = Convert.ToDateTime(dgvExperience.Rows[e.RowIndex].Cells["EndDate"].Value);
                }
                Rectangle tempRect = dgvExperience.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                showDtPickerForm(Cursor.Position.X, Cursor.Position.Y, tempRect.Location, dt);
            }

        }

        private void btnExpCancel_Click(object sender, EventArgs e)
        {
            try
            {
                closeAllPanels();
                pnlList.Visible = true;
                enableBottomButtons();

            }
            catch (Exception)
            {

            }
        }
        private void dgvExperience_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //if (e.RowIndex < 0)
            //    return;
            ////removeDateTimePicker();
            ////if (dgvExperience.Controls.Contains(oDateTimePicker))
            ////{
            ////    dgvExperience.Controls.Remove(oDateTimePicker);
            ////}
            //string columnName = dgvExperience.Columns[e.ColumnIndex].Name;
            //if (columnName.Equals("expDelete"))
            //{
            //    try
            //    {
            //        //delete row
            //        DialogResult dialog = MessageBox.Show("Are you sure to Delete the row ?", "Yes", MessageBoxButtons.YesNo);
            //        if (dialog == DialogResult.Yes)
            //        {
            //            dgvExperience.Rows.RemoveAt(e.RowIndex);
            //        }
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
            //if (columnName.Equals("StartDate") || columnName.Equals("EndDate"))
            //{
            //    try
            //    {
            //        oDateTimePicker = new DateTimePicker();

            //        dgvExperience.Controls.Add(oDateTimePicker);
            //        oDateTimePicker.CustomFormat = "dd-MM-yyyy";
            //        //////oDateTimePicker.CustomFormat = "dd-MM-yyyy";
            //        oDateTimePicker.Format = DateTimePickerFormat.Custom;
            //        try
            //        {
            //            DateTime dt = DateTime.Parse(dgvExperience.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            //            oDateTimePicker.Value = dt.Date;
            //        }
            //        catch (Exception)
            //        {
            //            oDateTimePicker.Value = DateTime.Today.Date;
            //        }
            //        Rectangle oRectangle = dgvExperience.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
            //        oDateTimePicker.Size = new Size(oRectangle.Width, oRectangle.Height); 
            //        oDateTimePicker.Location = new Point(oRectangle.X, oRectangle.Y);
            //        //oDateTimePicker.MouseLeave += new EventHandler(oDateTimePicke_Leave);
            //        //oDateTimePicker.CloseUp += new EventHandler(oDateTimePicker_CloseUp);
            //        oDateTimePicker.ValueChanged += new EventHandler(dateTimePicker_OnTextChange);
            //        oDateTimePicker.Visible = true;
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}else
            //{
            //    //oDateTimePicker.Visible = false;

            //}
        }
        //private void dateTimePicker_OnTextChange(object sender, EventArgs e)
        //{
        //    // Saving the 'Selected Date on Calendar' into DataGridView current cell  
        //    try
        //    {
        //        dgvExperience.CurrentCell.Value = oDateTimePicker.Value;
        //        oDateTimePicker.Visible = false;


        //        ////oDateTimePicker.Dispose();
        //        ////dgvExperience.Controls.Remove(oDateTimePicker);
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //}

        private void btnExpSave_Click(object sender, EventArgs e)
        {
            try
            {
                EmployeeExperience empexprec;

                List<EmployeeExperience> employeeexperience = new List<EmployeeExperience>();
                for (int i = 0; i < dgvExperience.Rows.Count; i++)
                {
                    try
                    {
                        string sdatecol = dgvExperience.Rows[i].Cells["StartDate"].Value.ToString();
                        string edatecol = dgvExperience.Rows[i].Cells["EndDate"].Value.ToString();
                        empexprec = new EmployeeExperience();
                        string sdate = dgvExperience.Rows[i].Cells["StartDate"].Value.ToString();
                        string edate = dgvExperience.Rows[i].Cells["EndDate"].Value.ToString();
                        string employer = dgvExperience.Rows[i].Cells["Organisation"].Value.ToString();
                        string postheld = dgvExperience.Rows[i].Cells["PostHeld"].Value.ToString();
                        string remarks = "";
                        if (dgvExperience.Rows[i].Cells["expRemarks"].Value != null)
                        {
                            remarks = dgvExperience.Rows[i].Cells["expRemarks"].Value.ToString().Trim();
                        }
                        empexprec.empID = txtEmpID.Text;
                        empexprec.startdate = Convert.ToDateTime(sdate);
                        empexprec.enddate = Convert.ToDateTime(edate);
                        empexprec.employer = employer;
                        empexprec.postheld = postheld;
                        empexprec.Remarks = remarks;
                        if (empexprec.startdate != null && empexprec.enddate != null &&
                            empexprec.startdate < empexprec.enddate &&
                            empexprec.employer.Trim().Length > 0 && empexprec.postheld.Trim().Length > 0)
                        {
                            employeeexperience.Add(empexprec);
                        }
                        else
                        {
                            MessageBox.Show("Validation Failed");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Validation Failed");
                        ////////MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error - Please verify data entered");
                        return;
                    }
                }
                if (validateEmpExpGridDetails(txtEmpID.Text))
                {
                    if (!EmployeeDB.updateEmployeeExperience(txtEmpID.Text, employeeexperience))
                    {
                        MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                    }
                    else
                    {
                        try
                        {
                            MessageBox.Show("Employee experience updated");
                            closeAllPanels();
                            pnlList.Visible = true;
                            enableBottomButtons();
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Grid Detail Date Validation Failed");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        private Boolean validateEmpExpGridDetails(string EmpId)
        {
            Boolean stat = true;
            DateTime doj = EmployeeDB.getEmployeeDOJ(EmpId);
            int count = 0;
            foreach (DataGridViewRow row in dgvExperience.Rows)
            {
                count++;
                DateTime sdt = Convert.ToDateTime(row.Cells["StartDate"].Value);
                DateTime edt = Convert.ToDateTime(row.Cells["EndDate"].Value);
                if (sdt >= doj || edt > doj)
                {
                    return false;
                }
                if (count != 1)
                {
                    foreach (DataGridViewRow inrrow in dgvExperience.Rows)
                    {
                        if (row.Index != inrrow.Index)
                        {
                            DateTime inSdt = Convert.ToDateTime(inrrow.Cells["StartDate"].Value);
                            DateTime inEdt = Convert.ToDateTime(inrrow.Cells["EndDate"].Value);
                            if (sdt >= inSdt && sdt < inEdt)
                            {
                                return false;
                            }
                            if (edt > inSdt && edt <= inEdt)
                            {
                                return false;
                            }
                            if (inSdt > sdt && inSdt < edt && inEdt > sdt && inEdt < edt)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return stat;
        }
        private void showDtPickerForm(int left, int top, Point location, DateTime dtvalue)
        {
            if (left > Screen.PrimaryScreen.Bounds.Width - 250)
            {
                left = Screen.PrimaryScreen.Bounds.Width - 250;
            }
            dtpForm = new Form();
            dtpForm.StartPosition = FormStartPosition.Manual;
            dtpForm.Size = new Size(200, 100);
            dtpForm.Location = new Point(left, top);
            //dtpForm.Location = location;
            ////dtpForm.StartPosition = FormStartPosition.CenterScreen;
            DateTimePicker dt = new DateTimePicker();
            dt.Format = DateTimePickerFormat.Custom;
            dt.CustomFormat = "dd-MM-yyyy";
            dt.ValueChanged += new EventHandler(cellDateTimePickerValueChanged);
            dt.Value = dtvalue;
            dtpForm.Controls.Add(dt);
            {
                ////dt.Location = new Point(10,10);
                dt.Width = 150;
                dt.Height = 100;
                dt.Visible = true;
                dt.ShowUpDown = true;
                ////dt.Show();
                System.Windows.Forms.SendKeys.Send("%{DOWN}");
            }
            dtpForm.ShowDialog();
        }
        void cellDateTimePickerValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            dgvExperience.Rows[dgvExperience.CurrentCell.RowIndex].Cells[dgvExperience.CurrentCell.ColumnIndex - 1].Value = dtp.Value;
        }

        private void dgvQualification_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Error");
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
                        if (!row.Cells["gEmpName"].Value.ToString().Trim().ToLower().Contains(txtSearch.Text.Trim().ToLower()))
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
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            filterGridData();
        }

        private void Employee_Enter(object sender, EventArgs e)
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

