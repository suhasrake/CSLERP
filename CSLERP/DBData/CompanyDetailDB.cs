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
    class cmpnydetails
    {
        public int companyID { get; set; }
        public string companyname { get; set; }
        public string companyAddress { get; set; }
        public int status { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
    }

    class CompanyDetailDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public  List<cmpnydetails> getdetails()
        {
            cmpnydetails det;
            List<cmpnydetails> Details = new List<cmpnydetails>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select CompanyID, CompanyName,Address,Status "+
                    "from CompanyDetail order by CompanyID";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    det = new cmpnydetails();
                    det.companyID = reader.GetInt32(0);
                    det.companyname = reader.GetString(1);
                    det.companyAddress = reader.GetString(2);
                    det.status = reader.GetInt32(3);
                    Details.Add(det);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");

            }
            return Details;
            
        }
 
        public Boolean updatedetails(cmpnydetails det )
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update CompanyDetail set Address='" + det.companyAddress + 
                    "',Status="+det.status+
                    " where CompanyID='" + det.companyID+"'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("update", "CompanyDetail", "", updateSQL) +
                 Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean insertdetails(cmpnydetails det)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into CompanyDetail (CompanyID,CompanyName,Address,Status)" +
                    "values (" +
                    "'" + det.companyID + "'," +
                     "'" + det.companyname + "'," +
                     "'"+det.companyAddress+"',"+
                 "'"+   det.status+"'"+ ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("insert", "CompanyDetail", "", updateSQL) +
                 Main.QueryDelimiter;
                if (!UpdateTable.UT(utString))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                status = false;
            }
            return status;
        }
        public Boolean validatedetails(cmpnydetails det)
        {
            Boolean status = true;
            try
            {
                if (/*bool hasAllwhitespace =*/ det.companyAddress.Trim().Length == 0 || det.companyAddress == null)
                {
                    return false;
                }
                if (det.companyname.Trim().Length == 0 || det.companyname == null)
                {
                    return false;
                }
                if(det.companyID.ToString().Length == 0)
                {
                    return false;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return status;
        }
        //public static void fillRegionCombo(System.Windows.Forms.ComboBox cmb)
        //{
        //    cmb.Items.Clear();
        //    try
        //    {
        //        RegionDB dbrecord = new RegionDB();
        //        List<region> Regions = dbrecord.getRegions();
        //        foreach (region reg in Regions)
        //        {
        //            if (reg.status == 1)
        //            {
        //                cmb.Items.Add(reg.regionID + "-" + reg.name);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
        //    }

        //}
        public static void fillCatalogValueCombo(System.Windows.Forms.ComboBox cmb, string catalogvalue)
        {
            cmb.Items.Clear();
            try
            {
                CatalogueValueDB dbrecord = new CatalogueValueDB();
                List<cataloguevalue> CatalogueValues = dbrecord.getCatalogueValues();
                foreach (cataloguevalue catval in CatalogueValues)
                {
                    if (catval.catalogueID.Equals(catalogvalue) && catval.status == 1)
                    {
                        cmb.Items.Add(catval.catalogueValueID + "-" + catval.description);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }

        }
        public static void fillCompanyIDComboNew(System.Windows.Forms.ComboBox cmb)
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
                        //cmb.Items.Add(det.companyID + "-" + det.companyname);
                        Structures.ComboBoxItem cbitem =
                           new Structures.ComboBoxItem(det.companyname, det.companyID.ToString());
                        cmb.Items.Add(cbitem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

       
        }
    }

