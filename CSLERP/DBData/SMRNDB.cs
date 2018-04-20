using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class smrn
    {
        public int rowID { get; set; }
        public String DocumentID { get; set; }
        public String DocumentName { get; set; }
        public int SMRNNo { get; set; }
        public DateTime SMRNDate { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerDocumentNo { get; set; }
        public DateTime CustomerDocumentDate { get; set; }
        public string CourierID { get; set; }
        public string CourierName { get; set; }
        public int NoOfPackets { get; set; }
        public string Remarks { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public string ForwardUser { get; set; }
        public string ApproveUser { get; set; }
        public string CreatorName { get; set; }
        public string ForwarderName { get; set; }
        public string ApproverName { get; set; }
        public string ForwarderList { get; set; }
        public int Status { get; set; }
        public int DocumentStatus { get; set; }
    }

    class SMRNDB
    {
        ActivityLogDB alDB = new ActivityLogDB();


        public List<smrn> getFilteredSMRNHeader(string userList, int opt)
        {
            smrn smrnh;
            List<smrn> smrnhlist = new List<smrn>();
            try
            {
                string query1 = "select RowID, DocumentID, DocumentName,SMRNNo,SMRNDate," +
                    " CustomerID,CustomerName,CustomerDocumentNo,CustomerDocumentDate,CourierID,CourierName,NoOfPackets,Remarks," +
                    " CreateTime, CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,status,DocumentStatus,ForwarderList" +
                    " from viewSMRN" +
                   " where ((forwarduser='" + Login.userLoggedIn + "' and DocumentStatus between 2 and 98) " +
                    " or (createuser='" + Login.userLoggedIn + "' and DocumentStatus=1))" +
                     " order by SMRNNo";
                string query2 = "select RowID, DocumentID, DocumentName,SMRNNo,SMRNDate," +
                    " CustomerID,CustomerName,CustomerDocumentNo,CustomerDocumentDate,CourierID,CourierName,NoOfPackets,Remarks," +
                    " CreateTime, CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,status,DocumentStatus,ForwarderList" +
                    " from viewSMRN" +
                   " where ((createuser='" + Login.userLoggedIn + "'  and DocumentStatus between 2 and 98 ) " +
                    " or (ForwarderList like '%" + userList + "%' and DocumentStatus between 2 and 98 and ForwardUser <> '" + Login.userLoggedIn + "'))" +
                    " order by SMRNNo";
                string query3 = "select RowID, DocumentID, DocumentName,SMRNNo,SMRNDate," +
                    " CustomerID,CustomerName,CustomerDocumentNo,CustomerDocumentDate,CourierID,CourierName,NoOfPackets,Remarks," +
                    " CreateTime, CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,status,DocumentStatus,ForwarderList" +
                    " from viewSMRN" +
                     " where ((createuser='" + Login.userLoggedIn + "'" +
                    " or ForwarderList like '%" + userList + "%'" +
                    " or approveUser='" + Login.userLoggedIn + "')" +
                    " and DocumentStatus = 99)   order by SMRNNo";
                string query6 = "select RowID, DocumentID, DocumentName,SMRNNo,SMRNDate," +
                    " CustomerID,CustomerName,CustomerDocumentNo,CustomerDocumentDate,CourierID,CourierName,NoOfPackets,Remarks," +
                    " CreateTime, CreateUser,ForwardUser,ApproveUser,CreatorName,ForwarderName,ApproverName,status,DocumentStatus,ForwarderList" +
                    " from viewSMRN" +
                   " where  DocumentStatus = 99  and Status = 1 order by SMRNNo";

                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "";
                switch (opt)
                {
                    case 1:
                        query = query1;
                        break;
                    case 2:
                        query = query2;
                        break;
                    case 3:
                        query = query3;
                        break;
                    case 6:
                        query = query6;
                        break;
                    default:
                        query = "";
                        break;
                }
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    smrnh = new smrn();
                    smrnh.rowID = reader.GetInt32(0);
                    smrnh.DocumentID = reader.GetString(1);
                    smrnh.DocumentName = reader.GetString(2);
                    smrnh.SMRNNo = reader.GetInt32(3);
                    smrnh.SMRNDate = reader.GetDateTime(4);
                    smrnh.CustomerID = reader.GetString(5);
                    smrnh.CustomerName = reader.GetString(6);
                    smrnh.CustomerDocumentNo = reader.GetString(7);
                    smrnh.CustomerDocumentDate = reader.GetDateTime(8);
                    smrnh.CourierID = reader.GetString(9);
                    smrnh.CourierName = reader.GetString(10);
                    smrnh.NoOfPackets = reader.GetInt32(11);
                    smrnh.Remarks = reader.GetString(12);
                    smrnh.CreateTime = reader.GetDateTime(13);
                    smrnh.CreateUser = reader.GetString(14);
                    smrnh.ForwardUser = reader.GetString(15);
                    smrnh.ApproveUser = reader.GetString(16);
                    smrnh.CreatorName = reader.GetString(17);
                    smrnh.ForwarderName = reader.GetString(18);
                    smrnh.ApproverName = reader.GetString(19);
                    smrnh.Status = reader.GetInt32(20);
                    smrnh.DocumentStatus = reader.GetInt32(21);
                    if (!reader.IsDBNull(22))
                    {
                        smrnh.ForwarderList = reader.GetString(22);
                    }
                    else
                    {
                        smrnh.ForwarderList = "";
                    }
                    //smrnh.ForwarderList = reader.GetString(19);
                    smrnhlist.Add(smrnh);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying SMRN  Data");
            }
            return smrnhlist;

        }
        public Boolean updateSMRN(smrn smrnh, smrn prevsmrnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update SMRN set   CustomerID='" + smrnh.CustomerID + "'" +
                     ", SMRNNo =" + smrnh.SMRNNo +
                    ", SMRNDate='" + smrnh.SMRNDate.ToString("yyyy-MM-dd") +
                     "', CustomerDocumentNo ='" + smrnh.CustomerDocumentNo +
                    "', CustomerDocumentDate='" + smrnh.CustomerDocumentDate.ToString("yyyy-MM-dd") +
                    "', CourierID='" + smrnh.CourierID +
                    "', NoOfPackets=" + smrnh.NoOfPackets +
                   ", Remarks='" + smrnh.Remarks +
                    "', ForwarderList='" + smrnh.ForwarderList +
                     "' where DocumentID='" + prevsmrnh.DocumentID +
                    "' and SMRNNo=" + prevsmrnh.SMRNNo +
                    " and SMRNDate='" + prevsmrnh.SMRNDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "SMRN", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        public Boolean insertSMRN(smrn smrnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into SMRN " +
                    " (DocumentID,SMRNNo,SMRNDate,CustomerID,CustomerDocumentNo," +
                    "CustomerDocumentDate,CourierID,NoOfPackets,Remarks,DocumentStatus," +
                    "Status,CreateUser,CreateTime,ForwarderList)" +
                    "values (" +
                    "'" + smrnh.DocumentID + "'," +
                   smrnh.SMRNNo + "," +
                   "'" + smrnh.SMRNDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + smrnh.CustomerID + "'," +
                    "'" + smrnh.CustomerDocumentNo + "',"+
                    "'" + smrnh.CustomerDocumentDate.ToString("yyyy-MM-dd") + "'," +
                    "'" + smrnh.CourierID + "'," +
                    smrnh.NoOfPackets + "," +
                    "'" + smrnh.Remarks + "'," +
                    smrnh.DocumentStatus + "," +
                    smrnh.Status + "," +
                    "'" + Login.userLoggedIn +"',"+
                    "GETDATE()" +
                    ",'" + smrnh.ForwarderList + "')";
                //"'" + pheader.ForwardUser + "'," +
                //"'" + pheader.ApproveUser + "'," +
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "SMRN", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {

                status = false;
            }
            return status;
        }
        public Boolean validateSMRN(smrn smrnh)
        {
            Boolean status = true;
            try
            {
                if (smrnh.DocumentID.Trim().Length == 0 || smrnh.DocumentID == null)
                {
                    return false;
                }

                if (smrnh.SMRNNo== 0)
                {
                    return false;
                }
                if (smrnh.SMRNDate == null)
                {
                    return false;
                }
                if (smrnh.CustomerID.Trim().Length == 0 || smrnh.CustomerID == null)
                {
                    return false;
                }
                if (smrnh.CustomerDocumentNo.Trim().Length == 0 || smrnh.CustomerDocumentNo == null)
                {
                    return false;
                }
                if (smrnh.CustomerDocumentDate == null || smrnh.CustomerDocumentDate > DateTime.Today)
                {
                    return false;
                }
                if (smrnh.CourierID.Trim().Length == 0 || smrnh.CourierID == null)
                {
                    return false;
                }
                if (smrnh.NoOfPackets == 0)
                {
                    return false;
                }
                if (smrnh.Remarks.Trim().Length == 0 || smrnh.Remarks == null)
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Validation Failed.");
            }
            
            return status;
        }
        public Boolean forwardSMRN(smrn prevsmrnh)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update SMRN set DocumentStatus=" + (prevsmrnh.DocumentStatus + 1) +
                     ", forwardUser='" + prevsmrnh.ForwardUser + "'" +
                       ", ForwarderList='" + prevsmrnh.ForwarderList + "'" +
                    " where DocumentID='" + prevsmrnh.DocumentID +
                    "' and SMRNNo=" + prevsmrnh.SMRNNo +
                    " and SMRNDate='" + prevsmrnh.SMRNDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("forward", "SMRN", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        public Boolean reverseSMRN(smrn prevsmrn)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update SMRN set DocumentStatus=" + prevsmrn.DocumentStatus +
                    ", forwardUser='" + prevsmrn.ForwardUser + "'" +
                    ", ForwarderList='" + prevsmrn.ForwarderList + "'" +
                    " where DocumentID='" + prevsmrn.DocumentID + "'" +
                    " and SMRNNo=" + prevsmrn.SMRNNo +
                    " and SMRNDate='" + prevsmrn.SMRNDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "ProjectHeader", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        public Boolean ApproveSMRN(smrn prevsmrn)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update SMRN set DocumentStatus=99" +
                    ", Status=1" +
                    ", ApproveUser='" + Login.userLoggedIn + "'" +
                    " where DocumentID='" + prevsmrn.DocumentID +
                   "' and SMRNNo=" + prevsmrn.SMRNNo +
                    " and SMRNDate='" + prevsmrn.SMRNDate.ToString("yyyy-MM-dd") + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("approve", "SMRN", "", updateSQL) +
                Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }
        public static List<smrn> getAcceptedSMRNNO()
        {
            smrn sm;
            List<smrn> SMRNList = new List<smrn>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select SMRNNo, SMRNDate, CustomerID from SMRN where DocumentStatus = 99 and Status = 1";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    sm = new smrn();
                    sm.SMRNNo = reader.GetInt32(0);
                    sm.SMRNDate = reader.GetDateTime(1);
                    sm.CustomerID = reader.GetString(2);
                    SMRNList.Add(sm);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
            }
            return SMRNList;
        }
        public static ListView getSMRNListView()
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
                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SMRNNo", -2, HorizontalAlignment.Left);
                lv.Columns.Add("SMRNDate", -2, HorizontalAlignment.Left);
                lv.Columns.Add("CustID", -2, HorizontalAlignment.Left);
                List<smrn> SMRNList = SMRNDB.getAcceptedSMRNNO();
                foreach (smrn sm in SMRNList)
                {
                    ListViewItem item = new ListViewItem();
                    item.Checked = false;
                    item.SubItems.Add(sm.SMRNNo.ToString());
                    item.SubItems.Add(sm.SMRNDate.ToString());
                    item.SubItems.Add(sm.CustomerID.ToString());
                    lv.Items.Add(item);
                }

            }
            catch (Exception ex)
            {

            }
            return lv;
        }
        //public static void fillprojectCombo(System.Windows.Forms.ComboBox cmb)
        //{
        //    cmb.Items.Clear();
        //    try
        //    {
        //        ProjectHeaderDB projectdb = new ProjectHeaderDB();
        //        List<projectheader> project = projectdb.getFilteredProjectHeader("",6);
        //        foreach (projectheader poh in project)
        //        {
        //            if (poh.Status == 1)
        //            {
        //                cmb.Items.Add(poh.ProjectID);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(this.ToString() + "-"+ System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
        //    }
        //}
    }
}
