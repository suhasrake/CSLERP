using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace CSLERP.DBData
{
    class ptdefinition
    {
        public int RowID { get; set; }
        public int SequenceNo { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public int status { get; set; }
        public DateTime Createime { get; set; }
        public string CreateUser { get; set; }
    }

    class PTDefinitionDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public static DataGridView dgvpt = new DataGridView();
        public static Panel pnldgv = new Panel();
        public  List<ptdefinition> getPTDefinitions()
        {
            ptdefinition ptd;
            List<ptdefinition> PTDefinitions = new List<ptdefinition>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select Rowid,SequenceNo,Description,ShortDescription, status " +
                    "from PTDefinition order by SequenceNo";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ptd = new ptdefinition();
                    ptd.RowID = reader.GetInt32(0);
                    ptd.SequenceNo = reader.GetInt32(1);
                    ptd.Description = reader.GetString(2);
                    ptd.ShortDescription = reader.GetString(3);
                    ptd.status = reader.GetInt32(4);
                    PTDefinitions.Add(ptd);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            
            }
            return PTDefinitions;
            
        }

        public static DataGridView AcceptPaymentTerms(string ptstr)
        {
            try
            {
                dgvpt.ColumnCount = 5;
                dgvpt.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
                dgvpt.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgvpt.ColumnHeadersDefaultCellStyle.Font = new Font(dgvpt.Font, FontStyle.Bold);
                dgvpt.Name = "PaymentTerms";
                dgvpt.Location = new Point(50, 8);
                dgvpt.Size = new Size(500, 250);
                dgvpt.GridColor = Color.Black;
                dgvpt.RowHeadersVisible = false;
                dgvpt.EditMode = DataGridViewEditMode.EditOnEnter;
                foreach (DataGridViewColumn column in dgvpt.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dgvpt.Columns[0].Name = "Code";
                dgvpt.Columns[1].Name = "ID";
                dgvpt.Columns[2].Name = "Description";
                dgvpt.Columns[3].Name = "Percentage";
                dgvpt.Columns[4].Name = "CreditPeriod";
                dgvpt.Columns[2].Width = 200;
                dgvpt.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvpt.MultiSelect = false;
                dgvpt.AllowUserToAddRows = false;
                dgvpt.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvpt.RowHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvpt.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgvpt.Rows.Clear();
                int i = 0;
                PTDefinitionDB ptddb = new PTDefinitionDB();
                List<ptdefinition> PTDefinitions = ptddb.getPTDefinitions();
                foreach (ptdefinition ptd in PTDefinitions)
                {
                    dgvpt.Rows.Add();
                    dgvpt.Rows[i].Cells["Code"].Value = ptd.RowID.ToString();
                    dgvpt.Rows[i].Cells["ID"].Value = ptd.SequenceNo.ToString();
                    dgvpt.Rows[i].Cells["Description"].Value = ptd.ShortDescription.ToString();
                    dgvpt.Rows[i].Cells["Percentage"].Value = "0";
                    dgvpt.Rows[i].Cells["CreditPeriod"].Value = "0";
                    if (!ptd.ShortDescription.ToString().Equals("Credit"))
                    {
                        dgvpt.Rows[i].Cells["CreditPeriod"].ReadOnly = true;
                    }
                    i++;
                }
                //--split string and post in grid
                Boolean creditFlag = false;
                string[] strArry1 = ptstr.Split(new string[] { ";" }, StringSplitOptions.None);
                for (i = 0; i < strArry1.Length; i++)
                {
                    if (strArry1[i].Trim().Length > 0)
                    {
                        string[] strArry2 = strArry1[i].Split(new string[] { "-" }, StringSplitOptions.None);
                        for (int j = 0; j < dgvpt.Rows.Count; j++)
                        {
                            if (strArry2[0].Trim().Equals(dgvpt.Rows[j].Cells["ID"].Value.ToString()))
                            {
                                //code found
                                if (dgvpt.Rows[j].Cells["Description"].Value.ToString().Equals("Credit"))
                                {
                                    if (creditFlag)
                                    {
                                        dgvpt.Rows.Add();
                                        dgvpt.Rows[dgvpt.Rows.Count - 1].Cells["Code"].Value = dgvpt.Rows[dgvpt.Rows.Count - 2].Cells["Code"].Value;
                                        dgvpt.Rows[dgvpt.Rows.Count - 1].Cells["ID"].Value = dgvpt.Rows[dgvpt.Rows.Count - 2].Cells["ID"].Value;
                                        dgvpt.Rows[dgvpt.Rows.Count - 1].Cells["Description"].Value = dgvpt.Rows[dgvpt.Rows.Count - 2].Cells["Description"].Value;
                                        dgvpt.Rows[dgvpt.Rows.Count - 1].Cells["Percentage"].Value = strArry2[1];
                                        dgvpt.Rows[dgvpt.Rows.Count - 1].Cells["CreditPeriod"].Value = strArry2[2];
                                        break; ;
                                    }
                                    else
                                    {
                                        creditFlag = true;
                                    }
                                }
                                else
                                {
                                    dgvpt.Rows[j].Cells["CreditPeriod"].ReadOnly = true;
                                    strArry2[2]="0";
                                }
                                dgvpt.Rows[j].Cells["Percentage"].Value = strArry2[1];
                                dgvpt.Rows[j].Cells["CreditPeriod"].Value = strArry2[2];
                            }
                        }
                    }
                }
                dgvpt.Columns["Code"].ReadOnly = true;
                dgvpt.Columns["Code"].Visible = false;
                dgvpt.Columns["ID"].ReadOnly = true;
                dgvpt.Columns["Description"].ReadOnly = true;
                pnldgv.Controls.Add(dgvpt);
                dgvpt.Visible = true;
                pnldgv.Visible = true;

            }
            catch (Exception ex)
            {
            }
            return dgvpt;
        }
        public static string getPaymentTermString(string ptStr)
        {
            string pt = "";
            string[] indTerms = ptStr.Split(';');
            for(int i = 0; i<indTerms.Length - 1; i++)
            {
                string[] inrArr = indTerms[i].Split('-');
                if(i != 0)
                    pt = pt + ",\n"+innerPTString(inrArr);
                else
                    pt = innerPTString(inrArr);
            }
            return pt;
        }
        private static string innerPTString(string[] strArr)
        {
            string term = "";
            switch (strArr[0])
            {
                case "1":
                    term = strArr[1] +"% On Advance";
                    break;
                case "2":
                    term = strArr[1] + "% Against PI";
                    break;
                case "3":
                    term = strArr[1] + "% Against Delivery";
                    break;
                case "4":
                    term = strArr[1] + "% After " + strArr[2] +" days of Delivery";
                    break;
            }
            return term;
        }
    }
}
