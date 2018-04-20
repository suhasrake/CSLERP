using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{

    class tcmapping
    {
        public int RowID { get; set; }
        public string DocumentID { get; set; }
        public Int32 ReferenceTC { get; set; }
    }
    class TCmappingDB
    {
        public List<tcmapping> getTCMappingList(string docID)
        {
            tcmapping tcmapping;
            List<tcmapping> TCMList = new List<tcmapping>();
            try
            {
                string query = "select RowID, DocumentID, ReferenceTC " +
                    " from TCMapping where DocumentID = '" + docID + "'";
                SqlConnection conn = new SqlConnection(Login.connString);
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tcmapping = new tcmapping();
                    tcmapping.RowID = reader.GetInt32(0);
                    tcmapping.DocumentID = reader.GetString(1);
                    tcmapping.ReferenceTC = reader.GetInt32(2);

                    TCMList.Add(tcmapping);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Terms and Conditions Mapping Details");
            }
            return TCMList;
        }
        public Boolean UpdateTCMapping(List<tcmapping> TCMList, tcmapping tc)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Delete from TCMapping where DocumentID = '" + tc.DocumentID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "TCMapping", "", updateSQL) +
                    Main.QueryDelimiter;
                foreach (tcmapping tcm in TCMList)
                {
                    updateSQL = "insert into TCMapping " +
                    "(DocumentID,ReferenceTC) " +
                    "values ('" + tcm.DocumentID + "'," + "" + tcm.ReferenceTC + ")";

                    utString = utString + updateSQL + Main.QueryDelimiter;
                    utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("insert", "TCMapping", "", updateSQL) +
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
        public static Boolean DeleteTCMapping(string ReferenceTC)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "Delete from TCMapping where ReferenceTC = '" + ReferenceTC + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "TCMapping", "", updateSQL) +
                    Main.QueryDelimiter;
                //foreach (tcmapping tcm in TCMList)
                //{
                //    updateSQL = "insert into TCMapping " +
                //    "(DocumentID,ReferenceTC) " +
                //    "values ('" + tcm.DocumentID + "'," + "'" + tcm.ReferenceTC + "')";

                //    utString = utString + updateSQL + Main.QueryDelimiter;
                //    utString = utString +
                //    ActivityLogDB.PrepareActivityLogQquerString("insert", "TCMapping", "", updateSQL) +
                //    Main.QueryDelimiter;
                //}
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
        public static string fillTCTextBox(string documentID)
        {
            //trackingdetails=2(dd-MM-yyyy)
            string RefTCList = "";

            try
            {
                TCmappingDB tcdb = new TCmappingDB();
                List<tcmapping> TCMList = tcdb.getTCMappingList(documentID);
                foreach(tcmapping tcmp in TCMList)
                {
                    RefTCList = RefTCList + tcmp.ReferenceTC + ";";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in retriving TCMapping List(Text Box)");
            }
            return RefTCList;
        }
    }
}
