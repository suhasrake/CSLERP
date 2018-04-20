using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{

    class termsandconditions
    {
        public int RowID { get; set; }
        public string documentID { get; set; }
        public int ParagraphID { get; set; }
        public string ParagraphHeading { get; set; }
        public string Details { get; set; }
    }
    class TermsAndConditionsDB
    {
        public List<termsandconditions> getTermsAndConditions()
        {
            termsandconditions tcond;
            List<termsandconditions> TConditions = new List<termsandconditions>();
            try
            {
                string query = "select RowID,ParagraphID, ParagraphHeading, Detail " +
                    " from TermsAndCondition ";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tcond = new termsandconditions();
                    tcond.RowID = reader.GetInt32(0);
                    tcond.ParagraphID = reader.GetInt32(1);
                    tcond.ParagraphHeading = reader.GetString(2);
                    tcond.Details = reader.GetString(3);

                    TConditions.Add(tcond);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Terms and Conditions Details");
            }
            return TConditions;
        }
        public Boolean UpdateTermsAndConditions(List<termsandconditions> TConditions, List<int> rows)
        {
            Boolean status = true;
            string utString = "";
            string updateSQL = "";
            try
            {
                foreach (termsandconditions tco in TConditions)
                {
                    if (rows.Contains(tco.RowID))
                    {

                        updateSQL = "update TermsAndCondition set ParagraphID='" + tco.ParagraphID + "', DocumentID='" + tco.documentID + "'," +
                           " ParagraphHeading='" + tco.ParagraphHeading + "', Detail='" + tco.Details + "' " +
                           " where RowID='" + tco.RowID + "'";
                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                            ActivityLogDB.PrepareActivityLogQquerString("update", "TermsAndCondition", "", updateSQL) +
                            Main.QueryDelimiter;
                    }
                    else
                    {
                        updateSQL = "insert into TermsAndCondition " +
                                       "(ParagraphID,DocumentID,ParagraphHeading,Detail) " +
                                "values (" + tco.ParagraphID + ",'" + tco.documentID + "','" + tco.ParagraphHeading + "','" + tco.Details + "')";

                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                        ActivityLogDB.PrepareActivityLogQquerString("insert", "TermsAndConditions", "", updateSQL) +
                        Main.QueryDelimiter;
                    }
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

        public Boolean DeleteTermsAndConditionsrow(int rowid)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Delete from TermsAndCondition where RowID=" + rowid;
                        utString = utString + updateSQL + Main.QueryDelimiter;
                        utString = utString +
                            ActivityLogDB.PrepareActivityLogQquerString("delete", "TermsAndConditions", "", updateSQL) +
                            Main.QueryDelimiter;
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









        public static ListView ReferenceTermsAndConditionSelectionView()
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
                TermsAndConditionsDB tcdb = new TermsAndConditionsDB();
                List<termsandconditions> TCList = tcdb.getTermsAndConditions();
                ////int index = 0;
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("TC ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("TC Header", -2, HorizontalAlignment.Left);
                lv.Columns.Add("TC Detail", -2, HorizontalAlignment.Left);
                //lv.Columns.Add("Prod Value", -2, HorizontalAlignment.Center);
                //lv.Columns.Add("Tax Amt", -2, HorizontalAlignment.Center);

                foreach (termsandconditions tc in TCList)
                {

                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = false;
                    item1.SubItems.Add(tc.ParagraphID.ToString());
                    item1.SubItems.Add(tc.ParagraphHeading);
                    item1.SubItems.Add(tc.Details);
                    //item1.SubItems.Add(ioh.CustomerName);
                    //item1.SubItems.Add(ioh.TargetDate.ToString("dd-MM-yyyy"));
                    //item1.SubItems.Add(ioh.ProductValue.ToString());
                    //item1.SubItems.Add(ioh.TaxAmount.ToString());
                    lv.Items.Add(item1);
                    ////index++;
                }
            }
            catch (Exception)
            {

            }
            return lv;
        }
        public static string getTCDetails(int pid)
        {
            string TCDetails = "";
            try
            {
                string query = "select RowID,ParagraphID, ParagraphHeading, Detail " +
                    " from TermsAndCondition where ParagraphID = " + pid;
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    TCDetails = reader.GetString(2) + "+" + reader.GetString(3) + ";";
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Terms and Conditions Details");
            }
            return TCDetails;
        }
        public static string getTCDetailsNew(int pid, string documentID)
        {
            string TCDetails = "";
            try
            {
                string query = "select RowID,ParagraphID, ParagraphHeading, Detail " +
                    " from TermsAndCondition where ParagraphID = " + pid +
                    " and DocumentID = '"+ documentID+"'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //TCDetails = reader.GetString(2) + "+" + reader.GetString(3) + ";";
                    TCDetails = reader.GetString(2) + Main.delimiter1 + reader.GetString(3) + Main.delimiter2;
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Terms and Conditions Details");
            }
            return TCDetails;
        }

        //04-04-2018
        public List<termsandconditions> getTermsAndConditionsfordoc(string docid)
        {
            termsandconditions tcond;
            List<termsandconditions> TConditions = new List<termsandconditions>();
            try
            {
                string query = "select RowID,ParagraphID, ParagraphHeading, Detail " +
                    " from TermsAndCondition where DocumentID='"+ docid + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tcond = new termsandconditions();
                    tcond.RowID = reader.GetInt32(0);
                    tcond.ParagraphID = reader.GetInt32(1);
                    tcond.ParagraphHeading = reader.GetString(2);
                    tcond.Details = reader.GetString(3);

                    TConditions.Add(tcond);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Terms and Conditions Details");
            }
            return TConditions;
        }

    }
}
