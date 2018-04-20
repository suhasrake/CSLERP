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
    public class menuitem
    {
        public string menuItemID { get; set; }
        public string description { get; set; }
        public string shortDescription { get; set; }
        public string userEmpName { get; set; }
        public int menuitemType { get; set; }
        public string documentID { get; set; }
        public string documentName { get; set; }
        public string pageLink { get; set; }
        public int menuitemStatus { get; set; }
        public string versionrequired { get; set; }
        public string menugrp { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
    }

    class MenuItemDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<menuitem> getMenuItems()
        {
            menuitem menuitemrec;
            List<menuitem> menuitems = new List<menuitem>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.menuitemid,a.description,a.shortdescription,a.TYPE, " +
                    "isnull(a.documentid,' '),isnull(b.documentName,' '), a.Pagelink,a.Status,a.CreateTime,a.CreateUser,isnull(a.VersionRequired,' ') VersionRequired ,a.MenuGroup " +
                    "from menuitem a LEFT OUTER JOIN Document b on a.DocumentID = b.DocumentID  order by a.shortdescription";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    menuitemrec = new menuitem();
                    menuitemrec.menuItemID = reader.GetString(0);
                    menuitemrec.description = reader.GetString(1);
                    menuitemrec.shortDescription = reader.GetString(2);
                    menuitemrec.menuitemType = reader.GetInt32(3);
                    menuitemrec.documentID = reader.GetString(4);
                    menuitemrec.documentName = reader.GetString(5);
                    menuitemrec.pageLink = reader.GetString(6);
                    menuitemrec.menuitemStatus = reader.GetInt32(7);
                    menuitemrec.versionrequired = reader.GetString(10);
                    menuitemrec.menugrp =reader.IsDBNull(11)?"": reader.GetString(11);
                    menuitems.Add(menuitemrec);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("getMenuItems() : Error");

            }
            return menuitems;

        }


        public string getMenuItemTypeString(int menuItemType)
        {
            string menuItemTypeString = "Unknown";
            try
            {
                for (int i = 0; i < MenuItem.menuitemTypeValues.GetLength(0); i++)
                {
                    if (Convert.ToInt32(MenuItem.menuitemTypeValues[i, 0]) == menuItemType)
                    {
                        menuItemTypeString = MenuItem.menuitemTypeValues[i, 1];
                        break;
                    }
                }
            }
            catch (Exception)
            {
                menuItemTypeString = "Unknown";
            }
            return menuItemTypeString;
        }


        public int getMenuItemTypeCode(string menuItemTypeString)
        {
            int menuItemTypeCode = 0;
            try
            {
                for (int i = 0; i < MenuItem.menuitemTypeValues.GetLength(0); i++)
                {
                    if (MenuItem.menuitemTypeValues[i, 1].Equals(menuItemTypeString))
                    {
                        menuItemTypeCode = Convert.ToInt32(MenuItem.menuitemTypeValues[i, 0]);
                        break;
                    }
                }
            }
            catch (Exception)
            {
                menuItemTypeCode = 0;
            }
            return menuItemTypeCode;
        }
        public Boolean updateMenuItem(menuitem menu)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update MenuItem set Description='" + menu.description + "'," +
                    "ShortDescription='" + menu.shortDescription + "'," +
                    "Type=" + menu.menuitemType + "," +
                    "DocumentID='" + menu.documentID + "'," +
                    "PageLink='" + menu.pageLink + "'," +
                    "VersionRequired='" + menu.versionrequired + "'," +
                      "MenuGroup='" + menu.menugrp + "'," +
                    "Status=" + menu.menuitemStatus + 
                    " where MenuItemID='" + menu.menuItemID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("update", "MenuItem", "", updateSQL) +
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
        public Boolean insertMenuItem(menuitem menu)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                DateTime cdt = DateTime.Now;
                string updateSQL = "insert into MenuItem (MenuItemID,Description,ShortDescription,Type,DocumentID,PageLink,VersionRequired,MenuGroup,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + menu.menuItemID + "'," +
                    "'" + menu.description + "'," +
                    "'" + menu.shortDescription + "'," +
                    menu.menuitemType + "," +
                    "'" + menu.documentID + "'," +
                    "'" + menu.pageLink + "'," +
                    "'" + menu.versionrequired + "'," +
                    "'" + menu.menugrp + "'," +
                    menu.menuitemStatus + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                  ActivityLogDB.PrepareActivityLogQquerString("insert", "MenuItem", "", updateSQL) +
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
        public Boolean validateMenuItem(menuitem menu)
        {
            Boolean status = true;
            try
            {
                if (menu.menuItemID.Trim().Length == 0 || menu.menuItemID == null)
                {
                    return false;
                }
                if (menu.description.Trim().Length == 0 || menu.description == null)
                {
                    return false;
                }
                if (menu.shortDescription.Trim().Length == 0 || menu.shortDescription == null)
                {
                    return false;
                }
                if (menu.menugrp.Trim().Length == 0 || menu.menugrp == null)
                {
                    return false;
                }
            }
            catch (Exception)
            {

            }
           
            return status;
        }
        public static void fillMenuItemCombo(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                DocumentDB dbrecord = new DocumentDB();
                List<document> DocItems = dbrecord.getDocuments();
                foreach (document doc in DocItems)
                {
                    cmb.Items.Add(doc.DocumentID + "-" + doc.DocumentName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("fillMenuItemCombo() : Error");
            }

        }
        public static void fillMenuItemComboNew(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                DocumentDB dbrecord = new DocumentDB();
                List<document> DocItems = dbrecord.getDocuments();
                foreach (document doc in DocItems)
                {
                    Structures.ComboBoxItem cbitem =
                        new Structures.ComboBoxItem(doc.DocumentName, doc.DocumentID);
                    cmb.Items.Add(cbitem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("fillMenuItemComboNew() : Error");
            }

        }

        //27-02-2018
        public static void fillMenuGroupCombo(System.Windows.Forms.ComboBox cmb)
        {
            cmb.Items.Clear();
            try
            {
                List<menuitem> DocItems = getMenuItemsHeader();
                foreach (menuitem doc in DocItems)
                {
                    Structures.ComboBoxItem cbitem =
                        new Structures.ComboBoxItem(doc.menugrp, doc.menugrp);
                    cmb.Items.Add(cbitem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("fillMenuItemComboNew() : Error");
            }

        }


        //2018-02-20
        public static string getMenuItemID(string pagelink)
        {
            string menuID = "";
            try
            {
                string query = "";
                SqlConnection conn = new SqlConnection(Login.connString);
                query = "select MenuItemID from MenuItem where PageLink='" + pagelink + "'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    menuID = reader.IsDBNull(0) ? "" : reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error querying Work Order Details");
            }
            return menuID;
        }


        //2018-02-23
        public static List<menuitem> getMenuItemsHeader()
        {
            menuitem menuitemhdr;
            List<menuitem> menuitems = new List<menuitem>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select distinct MenuGroup from MenuGroup where Status='1'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    menuitemhdr = new menuitem();
                    menuitemhdr.menugrp = reader.GetString(0);
                    menuitems.Add(menuitemhdr);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("getMenuItemsHeader() : Error");
            }
            return menuitems;
        }

    }
}
