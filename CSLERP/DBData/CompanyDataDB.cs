using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    //public strin "Data Source = PRAKASHPC; Initial Catalog = newERP; Integrated Security = True"
    class cmpnydata
    {
        public int CompanyID { get; set; }
        public string DataID { get; set; }
        public string DataValue { get; set; }
        public int status { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
    }

    class CompanyDataDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<cmpnydata> getData(string cmpnyID)
        {
            cmpnydata dat;
            List<cmpnydata> Data = new List<cmpnydata>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select DataID,DataValue,Status from CompanyData where " +
                    "CompanyID='" + cmpnyID + "' order by DataID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dat = new cmpnydata();
                    dat.DataID = reader.GetString(0);
                    dat.DataValue = reader.GetString(1);
                    dat.status = reader.GetInt32(2);
                    Data.Add(dat);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return Data;
        }

        public Boolean updateData(cmpnydata dat)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update CompanyData set DataValue='" + dat.DataValue +
                    "',Status=" + dat.status +
                    " where CompanyID='" + dat.CompanyID + "' and DataID='" + dat.DataID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("update", "CompanyData", "", updateSQL) +
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
        public Boolean insertData(cmpnydata dat)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updatesql = "Delete from CompanyData Where CompanyID='" + dat.CompanyID + "'" +
                    " and DataID='" + dat.DataID + "' ";
                utString = utString + updatesql + Main.QueryDelimiter;
                utString = utString +
                    ActivityLogDB.PrepareActivityLogQquerString("delete", "CompanyData", "", updatesql) +
                    Main.QueryDelimiter;
                updatesql = "insert into CompanyData (CompanyID,DataID,DataValue,Status) " +
                    "values (" +
                    "'" + dat.CompanyID + "'," +
                     "'" + dat.DataID + "'," +
                     "'" + dat.DataValue + "'," +
                    dat.status + ")";
                utString = utString + updatesql + Main.QueryDelimiter;
                utString = utString +
                ActivityLogDB.PrepareActivityLogQquerString("insert", "CompanyData", "", updatesql) +
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
        public Boolean validateData(cmpnydata dat)
        {
            Boolean status = true;
            try
            {
                if (dat.DataValue.Trim().Length == 0 || dat.DataValue == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                return false;
            }
            return status;
        }
        public static void fillRegionCombo(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                RegionDB dbrecord = new RegionDB();
                List<region> Regions = dbrecord.getRegions();
                foreach (region reg in Regions)
                {
                    if (reg.status == 1)
                    {
                        cmb.Items.Add(reg.regionID + "-" + reg.name);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }

        }
        public static void fillCompanyIDCombo(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                CompanyDetailDB dbrecord = new CompanyDetailDB();
                List<cmpnydetails> details = dbrecord.getdetails();
                foreach (cmpnydetails det in details)
                {
                    if (det.status == 1)
                    {
                        cmb.Items.Add(det.companyID + "-" + det.companyname);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        public static List<cmpnydata> companyidcombo()
        {
            cmpnydata dat;
            List<cmpnydata> data = new List<cmpnydata>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select CompanyID " +
                    "from CompanyDetail order by CompanyID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    dat = new cmpnydata();
                    dat.CompanyID = reader.GetInt32(0);
                    data.Add(dat);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return data;
        }
    }
}
