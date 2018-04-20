using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class ListViewFIll
    {
        public static ListView getSLListView()
        {
            ListView lv = new ListView();
            try
            {

                lv.View = View.Details;
                lv.LabelEdit = true;
                lv.AllowColumnReorder = true;
                lv.CheckBoxes = true;
                lv.FullRowSelect = true;
                lv.GridLines = true;
                lv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                EmployeePostingDB edb = new EmployeePostingDB();
                List<employeeposting> EMPList = EmployeePostingDB.getEmployeePostingList();
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Id", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Name", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Type", -2, HorizontalAlignment.Left);
                ////lv.Columns.Add("Office Name", -2, HorizontalAlignment.Left);
                ////lv.Columns[3].Width = 0;
                ////lv.Columns[4].Width = 0;
                foreach (employeeposting emp in EMPList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(emp.empID.ToString());
                    item1.SubItems.Add(emp.empName.ToString());
                    item1.SubItems.Add("Employee");
                    ////item1.SubItems.Add(emp.officeName.ToString());
                    lv.Items.Add(item1);
                }
                CustomerDB cdb = new CustomerDB();
                List<customer> CustList = cdb.getCustomers("", 6);
                foreach (customer cust in CustList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(cust.CustomerID.ToString());
                    item1.SubItems.Add(cust.name.ToString());
                    item1.SubItems.Add("Party");
                    lv.Items.Add(item1);
                }
                BankBranchDB bankDB = new BankBranchDB();
                List<bankbranch> BankList = bankDB.getBankBranches();
                foreach (bankbranch bank in BankList)
                {
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(bank.BranchID.ToString());
                    item1.SubItems.Add(bank.BranchName.ToString());
                    item1.SubItems.Add("Bank");
                    lv.Items.Add(item1);
                }
            }
            catch (Exception ex)
            {

            }
            return lv;
        }
        public static DataGridView getGridViewForPayeeDetails()
        {

            DataGridView grdPOPI = new DataGridView();
            try
            {
                string[] strColArr = { "ID", "Name","StateCode","Type"};
                DataGridViewTextBoxColumn[] colArr = {
                    new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn(),new DataGridViewTextBoxColumn()
                };

                DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
                dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
                dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSeaGreen;
                dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
                dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
                dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
                dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
                grdPOPI.EnableHeadersVisualStyles = false;
                grdPOPI.AllowUserToAddRows = false;
                grdPOPI.AllowUserToDeleteRows = false;
                grdPOPI.BackgroundColor = System.Drawing.SystemColors.GradientActiveCaption;
                grdPOPI.BorderStyle = System.Windows.Forms.BorderStyle.None;
                grdPOPI.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
                grdPOPI.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
                grdPOPI.ColumnHeadersHeight = 27;
                grdPOPI.RowHeadersVisible = false;
                grdPOPI.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                DataGridViewCheckBoxColumn colChk = new DataGridViewCheckBoxColumn();
                colChk.Width = 50;
                colChk.Name = "Select";
                colChk.HeaderText = "Select";
                colChk.ReadOnly = false;
                grdPOPI.Columns.Add(colChk);
                foreach (string str in strColArr)
                {
                    int index = Array.IndexOf(strColArr, str);
                    colArr[index].Name = str;
                    colArr[index].HeaderText = str;
                    colArr[index].ReadOnly = true;

                    //if (index == 1 || index == 3)
                    //    colArr[index].DefaultCellStyle.Format = "dd-MM-yyyy";
                    //if (index == 6)
                    //    colArr[index].Width = 350;
                    //else if (index == 2)
                    //    colArr[index].Width = 100;
                    //else
                    //    colArr[index].Width = 80;
                    //if (index == 5 || index == 11)
                    //    colArr[index].Visible = false;
                    //else
                    //    colArr[index].Visible = true;
                    grdPOPI.Columns.Add(colArr[index]);
                }
                EmployeePostingDB edb = new EmployeePostingDB();
                List<employeeposting> EMPList = EmployeePostingDB.getEmployeePostingList();
                foreach (employeeposting emp in EMPList)
                {
                    grdPOPI.Rows.Add();
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[0]].Value = emp.empID.ToString();
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[1]].Value = emp.empName;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[2]].Value = "";
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[3]].Value = "Employee";
                }

                CustomerDB cdb = new CustomerDB();
                //List<customer> CustList = cdb.getCustomers("", 6);
                List<customer> CustList = cdb.getCustomerList();
                foreach (customer cust in CustList)
                {
                    grdPOPI.Rows.Add();
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[0]].Value = cust.CustomerID;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[1]].Value = cust.name;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[2]].Value = cust.StateName;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[3]].Value = "Party";
                }

                OfficeDB offDb = new OfficeDB();
                List<office> offList = offDb.getOffices().Where(off => off.status == 1).ToList();
                foreach (office off in offList)
                {
                    grdPOPI.Rows.Add();
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[0]].Value = off.OfficeID;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[1]].Value = off.name;
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[2]].Value = "";
                    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[3]].Value = "Office";
                }
                //BankBranchDB bankDB = new BankBranchDB();
                //List<bankbranch> BankList = bankDB.getBankBranches();
                //foreach (bankbranch bank in BankList)
                //{
                //    grdPOPI.Rows.Add();
                //    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[0]].Value = bank.BranchID;
                //    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[1]].Value = bank.BranchName;
                //    grdPOPI.Rows[grdPOPI.Rows.Count - 1].Cells[strColArr[2]].Value = "bank";
                //}
            }
            catch (Exception ex)
            {
            }

            return grdPOPI;
        }
    }
}
