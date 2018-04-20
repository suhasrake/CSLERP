using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class taxcodeworking
    {
        public string TaxCode { get; set; }
        public int LineNo { get; set; }
        public string Description { get; set; }
        public string Operator { get; set; }
        public int OperandLine1 { get; set; }
        public int OperandLine2 { get; set; }
        public double OperatorValue { get; set; }
        public double Amount { get; set; }
        public string TaxItemName { get; set; }
        public int status { get; set; }
        public DateTime Createime { get; set; }
        public string CreateUser { get; set; }
    }


    class TaxCodeWorkingDB
    {
        ActivityLogDB alDB = new ActivityLogDB();

        public List<taxcodeworking> getTaxCodeWorkings(string TaxCode)
        {
            taxcodeworking tcw;
            List<taxcodeworking> TaxCodeWorkings = new List<taxcodeworking>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select TaxCode, [LineNo],Description,Operator,OperandLine1,OperandLine2,OperatorValue," +
                    "Amount, TaxItemName,status " +
                    "from TaxcodeWorking  where Taxcode='" + TaxCode + "' order by Taxcode";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tcw = new taxcodeworking();
                    tcw.TaxCode = reader.GetString(0);
                    tcw.LineNo = reader.GetInt32(1);
                    tcw.Description = reader.GetString(2);
                    tcw.Operator = reader.GetString(3);
                    tcw.OperandLine1 = reader.GetInt32(4);
                    tcw.OperandLine2 = reader.GetInt32(5);
                    tcw.OperatorValue = reader.GetDouble(6);
                    tcw.Amount = reader.GetDouble(7);
                    tcw.TaxItemName = reader.GetString(8);
                    tcw.status = reader.GetInt32(9);
                    TaxCodeWorkings.Add(tcw);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error querying Tax Code Workings");
            }
            return TaxCodeWorkings;
        }

        public List<taxcodeworking> getTaxCodeDetails()
        {
            taxcodeworking tcw;
            List<taxcodeworking> TaxCodeWorkings = new List<taxcodeworking>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select taxcode,operator,operatorValue,TaxItemName,SUBSTRING(TaxItemName,0,CHARINDEX('-',TaxItemName)) TaxItem" +
                    " from TaxCodeWorking " +
                    " where operator='Percentage' order by Taxcode";
                    ////" where Taxcode='" + TaxCode + "' and operator='Percentage' order by Taxcode";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tcw = new taxcodeworking();
                    tcw.TaxCode = reader.GetString(0);
                    tcw.Operator = reader.GetString(1);
                    tcw.OperatorValue = reader.GetDouble(2);
                    tcw.TaxItemName = reader.GetString(4);
                    TaxCodeWorkings.Add(tcw);
                }
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error Getting tax code details");
            }
            return TaxCodeWorkings;
        }

        public Boolean updateTaxCodeWorkings(string TaxCode, List<taxcodeworking> tcwList)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Delete from TaxCodeWorking where TaxCode='" + TaxCode + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString + 
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "TaxCodeWorking", "", updateSQL)+
                    Main.QueryDelimiter;
                foreach (taxcodeworking tcw in tcwList)
                {
                    updateSQL = "insert into TaxCodeWorking " +
                    "(TaxCode,[LineNo],Description,Operator,OperandLine1,OperandLine2,OperatorValue,Amount,TaxItemName," +
                    "Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + tcw.TaxCode + "'," +
                    "" + tcw.LineNo + "," +
                    "'" + tcw.Description + "'," +
                    "'" + tcw.Operator + "'," +
                    "" + tcw.OperandLine1 + "," +
                    "" + tcw.OperandLine2 + "," +
                    "" + tcw.OperatorValue + "," +
                    "" + tcw.Amount + "," +
                    "'" + tcw.TaxItemName + "'," +
                    tcw.status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "TaxCodeWorking", "", updateSQL)+
                    Main.QueryDelimiter;
                }
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }

        public static System.Data.DataTable calculateTax(string TaxCode, double Amount)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            {
                dt.Columns.Add("LineNo", typeof(Int32));
                dt.Columns.Add("Description", typeof(string));
                dt.Columns.Add("Operator", typeof(string));
                dt.Columns.Add("OperandLine1", typeof(Int32));
                dt.Columns.Add("OperandLine2", typeof(Int32));
                dt.Columns.Add("OperatorValue", typeof(double));
                dt.Columns.Add("Amount", typeof(double));
                dt.Columns.Add("TaxItemName", typeof(string));
            }

            TaxCodeWorkingDB tcwdb = new TaxCodeWorkingDB();
            List<taxcodeworking> tcwDetails = tcwdb.getTaxCodeWorkings(TaxCode);
            //load data in table
            foreach (taxcodeworking tcw in tcwDetails)
            {
                dt.Rows.Add();
                dt.Rows[dt.Rows.Count - 1][0] = tcw.LineNo;
                dt.Rows[dt.Rows.Count - 1][1] = tcw.Description;
                dt.Rows[dt.Rows.Count - 1][2] = tcw.Operator;
                dt.Rows[dt.Rows.Count - 1][3] = tcw.OperandLine1;
                dt.Rows[dt.Rows.Count - 1][4] = tcw.OperandLine2;
                dt.Rows[dt.Rows.Count - 1][5] = tcw.OperatorValue;
                dt.Rows[dt.Rows.Count - 1][6] = tcw.Amount;
                dt.Rows[dt.Rows.Count - 1][7] = tcw.TaxItemName;
                //process data using new amount
                dt.Rows[0][6] = Amount;
            }

            for (int i = 1; i < (dt.Rows.Count); i++)
            {
                double operand1 = 0, operand2 = 0;
                string oprtr = dt.Rows[i][2].ToString();
                int operandline1 = Convert.ToInt32(dt.Rows[i][3].ToString());
                int operandline2 = Convert.ToInt32(dt.Rows[i][4].ToString());
                double oprtrValue = Convert.ToDouble(dt.Rows[i][5].ToString());
                if (operandline1 > 0 && operandline1 < dt.Rows.Count)
                {
                    operand1 = Convert.ToDouble(dt.Rows[operandline1-1][6].ToString());
                }
                if (oprtrValue != 0)
                {
                    operand2 = oprtrValue;
                }
                else if (operandline2 > 0 && operandline2 < dt.Rows.Count)
                {
                    operand2 = Convert.ToDouble(dt.Rows[operandline2-1][6].ToString());
                }

                if (oprtr == "Add")
                {
                    dt.Rows[i][6] = Math.Round(operand1 + operand2, 2);
                }
                if (oprtr == "Subtract")
                {
                    dt.Rows[i][6] = Math.Round(operand1 - operand2, 2);
                }
                if (oprtr == "Multiply")
                {
                    dt.Rows[i][6] = Math.Round(operand1 * operand2, 2);
                }
                if (oprtr == "Divide")
                {
                    dt.Rows[i][6] = Math.Round(operand1 / operand2, 2);
                }
                if (oprtr == "Percentage")
                {
                    dt.Rows[i][6] = Math.Round((operand1 * operand2) / 100, 2);
                }
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string tst = dt.Rows[i][2].ToString()+"-"+ dt.Rows[i][6].ToString() +"-"+ dt.Rows[i][7].ToString();
            }
            return dt;
        }
    }
}
