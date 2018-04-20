using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
  
    class emaildata
    {
        public int RowID { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
        public string EmailData { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public DateTime ProcessTime { get; set; }
        public int status { get; set; }
    }

    class EmailDataDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        
        public Boolean insertEmailData(emaildata edata)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string dtString = UpdateTable.getSQLDateTime().ToString("dd-MM-yyyy HH:mm:ss");
                edata.EmailData = edata.EmailData + ".\nTime:"+dtString;
                string updateSQL = "insert into EmailData (EmailData,ToAddress,Subject,Status,CreateTime,CreateUser)"+
                    "values (" +
                    "'" + edata.EmailData + "'," +
                    "'" + edata.ToAddress + "'," +
                    "'" + edata.Subject + "'," +
                    edata.status+","+
                    "GETDATE()"+","+
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("insert", "EmailData", "", updateSQL) +
                  Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
      
    }
}
