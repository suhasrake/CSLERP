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
    class cataloguevalue
    {
        public string catalogueValueID { get; set; }
        public string description { get; set; }
        public string catalogueID { get; set; }
        public string cataloguedescription { get; set; }
        public int status { get; set; }
        public DateTime userCreateime { get; set; }
        public string userCreateUser { get; set; }
    }

    class CatalogueValueDB
    {
        ActivityLogDB alDB = new ActivityLogDB();
        public List<cataloguevalue> getCatalogueValues()
        {
            cataloguevalue catval;
            List<cataloguevalue> CatalogueValues = new List<cataloguevalue>();
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.CatalogueValueID, a.Description,a.CatalogueID,b.Description,a.status " +
                    "from CatalogueValue a, Catalogue b where a.CatalogueID=b.CatalogueID order by a.CatalogueID,a.Description";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    catval = new cataloguevalue();
                    catval.catalogueValueID = reader.GetString(0);
                    catval.description = reader.GetString(1);
                    catval.catalogueID = reader.GetString(2);
                    catval.cataloguedescription = reader.GetString(3);
                    catval.status = reader.GetInt32(4);
                    CatalogueValues.Add(catval);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return CatalogueValues;

        }

        public Boolean updateCatalogueValue(cataloguevalue catval)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "update CatalogueValue set Description='" + catval.description +
                    "',CatalogueID='" + catval.catalogueID +
                    "',Status=" + catval.status +
                    " where CatalogueValueID='" + catval.catalogueValueID + "'";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("update", "CatalogueValue", "", updateSQL) +
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
        public Boolean insertCatalogueValue(cataloguevalue catval)
        {
            Boolean status = true;
            string utString = "";
            try
            {
                string updateSQL = "insert into CatalogueValue (catalogueValueID,Description,CatalogueID,Status,CreateTime,CreateUser)" +
                    "values (" +
                    "'" + catval.catalogueValueID + "'," +
                     "'" + catval.description + "'," +
                    "'" + catval.catalogueID + "'," +
                    catval.status + "," +
                    "GETDATE()" + "," +
                    "'" + Login.userLoggedIn + "'" + ")";
                utString = utString + updateSQL + Main.QueryDelimiter;
                utString = utString +
                 ActivityLogDB.PrepareActivityLogQquerString("insert", "CatalogueValue", "", updateSQL) +
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
        public Boolean validateCatalogue(cataloguevalue catval)
        {
            Boolean status = true;
            try
            {
                if (catval.catalogueValueID.Trim().Length == 0 || catval.catalogueValueID == null ||
                    catval.catalogueValueID.IndexOf('-') >= 0)
                {
                    return false;
                }
                if (catval.description.Trim().Length == 0 || catval.description == null)
                {
                    return false;
                }
                if (catval.catalogueID.Trim().Length == 0 || catval.catalogueID == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + "-" + System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                return false;
            }
            return status;
        }
        public static void fillCustomerComboNew(System.Windows.Forms.ComboBox cmb, string catalogvalue)
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
        public static string fillCatalogValueGridViewCombo(DataGridViewComboBoxCell cmb, string catalogvalue)
        {
            string firstValue = "";
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
                        if (firstValue.Length == 0)
                        {
                            firstValue = catval.catalogueValueID + "-" + catval.description;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return firstValue;
        }
        public static void  fillCatalogValueListBox(ListBox lb, string catalogvalue)
        {
            try
            {
                
                CatalogueValueDB dbrecord = new CatalogueValueDB();
                List<cataloguevalue> CatalogueValues = dbrecord.getCatalogueValues();
                foreach (cataloguevalue catval in CatalogueValues)
                {
                    if (catval.catalogueID.Equals(catalogvalue) && catval.status == 1)
                    {
                        lb.Items.Add(catval.description);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        public static ListView getCatalogValueListView(string catalogueID)
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
                CatalogueValueDB dbrecord = new CatalogueValueDB();
                List<cataloguevalue> CatalogueValues = dbrecord.getCatalogueValues();

                lv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                lv.Columns.Add("Catalouge ID", -2, HorizontalAlignment.Left);
                lv.Columns.Add("CV Description", -2, HorizontalAlignment.Left);
                foreach (cataloguevalue catval in CatalogueValues)
                {
                    if (catval.catalogueID.Equals(catalogueID) && catval.status == 1)
                    {
                        ListViewItem item1 = new ListViewItem();
                        item1.Checked = false;
                        item1.SubItems.Add(catval.catalogueValueID.ToString());
                        item1.SubItems.Add(catval.description);
                        lv.Items.Add(item1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return lv;
        }
        public static string getParamValue(string catalogID, string catalogValueID)
        {
            string catval = "";
            try
            {
                SqlConnection conn = new SqlConnection(Login.connString);
                string query = "select a.Description " +
                    "from CatalogueValue a where a.CatalogueID='"+catalogID+ "' and a.CatalogueValueID='"+catalogValueID+"'";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    catval = reader.GetString(0);
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
            return catval;
        }
       
        public static void fillToLocationCombo(ComboBox cmb,string Location)
        {
            cmb.Items.Clear();
            try
            {
                CatalogueValueDB dbrecord = new CatalogueValueDB();
                List<cataloguevalue> CatalogueValues = dbrecord.getCatalogueValues();
                foreach (cataloguevalue catval in CatalogueValues)
                {
                    if (catval.catalogueID.Equals("StoreLocation") && catval.status == 1)
                    {
                        if (catval.catalogueValueID.ToString() != Location)
                        {
                            cmb.Items.Add(catval.catalogueValueID + "-" + catval.description);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }

        public static void fillCatalogValueComboNew(System.Windows.Forms.ComboBox cmb, string catalogvalue)
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
                        Structures.ComboBoxItem cbitem = 
                            new Structures.ComboBoxItem(catval.description, catval.catalogueValueID);
                        cmb.Items.Add(cbitem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }

        }
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
        //Grid View Combobox column 
        public static void fillCatalogValueGridViewComboNew(System.Windows.Forms.DataGridViewComboBoxCell cmb, string catalogvalue)
        {
            
            try
            {
                cmb.Items.Clear();
                CatalogueValueDB dbrecord = new CatalogueValueDB();
                List<cataloguevalue> CatalogueValues = dbrecord.getCatalogueValues();
                List<Structures.GridViewComboBoxItem> ItemList =
                    new List<Structures.GridViewComboBoxItem>();
                foreach (cataloguevalue catval in CatalogueValues)
                {
                    if (catval.catalogueID.Equals(catalogvalue) && catval.status == 1)
                    {
                        Structures.GridViewComboBoxItem ch = 
                            new Structures.GridViewComboBoxItem(catval.description, catval.catalogueValueID);
                        cmb.Items.Add(ch);
                    }
                }
                cmb.DisplayMember = "Name";  // Name Property will show(Editing)
                cmb.ValueMember = "Value";  // Value Property will save(Saving)
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }

        }
        public static void fillCatalogValueComboNew2(System.Windows.Forms.ComboBox cmb, System.Windows.Forms.ComboBox cmb2, System.Windows.Forms.Label Label)
        {
            cmb.Items.Clear();
            cmb2.Items.Clear();
            try
            {
                CatalogueValueDB dbrecord = new CatalogueValueDB();
                List<cataloguevalue> CatalogueValues = dbrecord.getCatalogueValues().Where(W => W.catalogueID == "POType").ToList();
                List<documentreceiver> ListL2 = new List<documentreceiver>();
                List<documentreceiver> ListL3 = new List<documentreceiver>();
                List<documentreceiver> ListL = Main.DocumentReceivers.Where(W => W.Status == 1).ToList(); //.Select(y => y.FirstOrDefault())
                string DocName = "";
                foreach (var itm in ListL)
                {
                    documentreceiver obj = new documentreceiver();
                    DocName = itm.DocumentID;
                    DocName = DocName.Replace("POSERVICEINWARD", "Service PO");
                    DocName = DocName.Replace("PAFSERVICEINWARD", "ServicePAF");
                    DocName = DocName.Replace("POPRODUCTINWARD", "Product PO");
                    DocName = DocName.Replace("PAFPRODUCTINWARD", "ProductPAF");
                    obj.DocumentName = DocName;
                    obj.OfficeID = itm.OfficeID;
                    obj.OfficeName = itm.OfficeName;
                    obj.DocumentID = itm.DocumentID;
                    ListL2.Add(obj);
                }
                string[] Str = new string[4];
                int i = 0;
                foreach (var itm in CatalogueValues)
                {
                    documentreceiver obj = new documentreceiver();
                    DocName = itm.description;
                    DocName = DocName.Replace("Service PO", "POSERVICEINWARD");
                    DocName = DocName.Replace("ServicePAF", "PAFSERVICEINWARD");
                    DocName = DocName.Replace("Product PO", "POPRODUCTINWARD");
                    DocName = DocName.Replace("ProductPAF", "PAFPRODUCTINWARD");
                    obj.DocumentID = DocName;
                    obj.DocumentName = itm.description;
                    ListL3.Add(obj);
                }
                var CatlnameId = Str;

                var abcsd = from Ca in CatalogueValues
                            join Ll in ListL2 on Ca.description equals Ll.DocumentName
                            select new
                            {
                                DocumentName = Ca.description,
                                DocumentID = Ll.DocumentID,
                                Ofice = Ll.OfficeID,
                                OficeName = Ll.OfficeName
                            };


                // cmb.Items.AddRange(ListL.ToArray());
                foreach (var itm in abcsd.GroupBy(x => x.DocumentName).Select(y => y.FirstOrDefault()).ToList())
                {
                    Structures.ComboBoxItem cbitem =
                            new Structures.ComboBoxItem(itm.DocumentName, itm.DocumentID);
                    cmb.Items.Add(cbitem);
                }
                foreach (var itm in abcsd.GroupBy(x => x.Ofice).Select(y => y.FirstOrDefault()).ToList())
                {
                    Structures.ComboBoxItem cbitem2 =
                            new Structures.ComboBoxItem(itm.OficeName, itm.Ofice);
                    cmb2.Items.Add(cbitem2);
                }
                if (ListL.Count <= 0)
                {
                    Label.Text = "1";
                    OfficeDB obj2 = new OfficeDB();
                    DocumentDB obj3 = new DocumentDB();
                    var Ofices = obj2.getOffices().Where(W => W.status == 1).ToList();
                    var Documents = obj3.getDocuments().Where(W => W.DocumentStatus == 1).ToList();
                    var results = from x in Documents
                                  join Ll in ListL3 on x.DocumentID equals Ll.DocumentID
                                  select new
                                  {
                                      DocumntId = x.DocumentID,
                                      documntname = Ll.DocumentName
                                  };
                    foreach (var itm in results)
                    {
                        Structures.ComboBoxItem cbitem =
                                new Structures.ComboBoxItem(itm.documntname, itm.DocumntId);
                        cmb.Items.Add(cbitem);
                    }
                    foreach (var itm in Ofices)
                    {
                        Structures.ComboBoxItem cbitem =
                                new Structures.ComboBoxItem(itm.name, itm.OfficeID);
                        cmb2.Items.Add(cbitem);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }

        }
        public static void fillCatalogValueComboNew2(System.Windows.Forms.ComboBox cmb, System.Windows.Forms.ComboBox cmb2, System.Windows.Forms.ComboBox cmb3, System.Windows.Forms.Label Label)
        {
            cmb.Items.Clear();
            cmb2.Items.Clear();
            cmb3.Items.Clear();
            try
            {
                CatalogueValueDB dbrecord = new CatalogueValueDB();
                List<cataloguevalue> CatalogueValues = dbrecord.getCatalogueValues().Where(W => W.catalogueID == "POType").ToList();
                List<documentreceiver> ListL2 = new List<documentreceiver>();
                List<documentreceiver> ListL3 = new List<documentreceiver>();
                List<documentreceiver> ListL = Main.DocumentReceivers.Where(W => W.Status == 1).ToList();
                OfficeDB obj2 = new OfficeDB();
                DocumentDB obj3 = new DocumentDB();
                var Ofices = obj2.getOffices().Where(W => W.status == 1).ToList();
                string DocName = "";
                foreach (var itm in ListL)
                {
                    documentreceiver obj = new documentreceiver();
                    DocName = itm.DocumentID;
                    DocName = DocName.Replace("POSERVICEINWARD", "Service PO");
                    DocName = DocName.Replace("PAFSERVICEINWARD", "ServicePAF");
                    DocName = DocName.Replace("POPRODUCTINWARD", "Product PO");
                    DocName = DocName.Replace("PAFPRODUCTINWARD", "ProductPAF");
                    obj.DocumentName = DocName;
                    obj.OfficeID = itm.OfficeID;
                    obj.OfficeName = itm.OfficeName;
                    obj.DocumentID = itm.DocumentID;
                    ListL2.Add(obj);
                }
                string[] Str = new string[4];
                ////int i = 0;
                foreach (var itm in CatalogueValues)
                {
                    documentreceiver obj = new documentreceiver();
                    DocName = itm.description;
                    DocName = DocName.Replace("Service PO", "POSERVICEINWARD");
                    DocName = DocName.Replace("ServicePAF", "PAFSERVICEINWARD");
                    DocName = DocName.Replace("Product PO", "POPRODUCTINWARD");
                    DocName = DocName.Replace("ProductPAF", "PAFPRODUCTINWARD");
                    obj.DocumentID = DocName;
                    obj.DocumentName = itm.description;
                    ListL3.Add(obj);
                }
                var abcsd = from Ca in CatalogueValues
                            join Ll in ListL2 on Ca.description equals Ll.DocumentName
                            select new
                            {
                                DocumentName = Ca.description,
                                DocumentID = Ll.DocumentID,
                                Ofice = Ll.OfficeID,
                                OficeName = Ll.OfficeName
                            };
                var Off = from Ca in Ofices
                          join Ll in abcsd on Ca.OfficeID equals Ll.Ofice
                          select Ca;


                // cmb.Items.AddRange(ListL.ToArray());
                foreach (var itm in abcsd.GroupBy(x => x.DocumentName).Select(y => y.FirstOrDefault()).ToList())
                {
                    Structures.ComboBoxItem cbitem =
                            new Structures.ComboBoxItem(itm.DocumentName, itm.DocumentID);
                    cmb.Items.Add(cbitem);
                }
                var Abcd2 = abcsd.GroupBy(x => x.Ofice).Select(y => y.FirstOrDefault()).ToList();
                foreach (var itm in Abcd2)
                {
                    Structures.ComboBoxItem cbitem2 =
                            new Structures.ComboBoxItem(itm.OficeName, itm.Ofice);
                    cmb2.Items.Add(cbitem2);
                }
                foreach (var itm in Ofices.GroupBy(G => G.RegionID))
                {
                    Structures.ComboBoxItem cbitem =
                            new Structures.ComboBoxItem(itm.Key, itm.Select(s => s.RegionName).FirstOrDefault());
                    cmb3.Items.Add(cbitem);
                }

                if (ListL.Count <= 0)
                {
                    Label.Text = "1";
                    var Documents = obj3.getDocuments().Where(W => W.DocumentStatus == 1).ToList();
                    var results = from x in Documents
                                  join Ll in ListL3 on x.DocumentID equals Ll.DocumentID
                                  select new
                                  {
                                      DocumntId = x.DocumentID,
                                      documntname = Ll.DocumentName
                                  };
                    foreach (var itm in results)
                    {
                        Structures.ComboBoxItem cbitem =
                                new Structures.ComboBoxItem(itm.documntname, itm.DocumntId);
                        cmb.Items.Add(cbitem);
                    }
                    foreach (var itm in Ofices)
                    {
                        Structures.ComboBoxItem cbitem =
                                new Structures.ComboBoxItem(itm.name, itm.OfficeID);
                        cmb2.Items.Add(cbitem);
                    }
                    // offli = Ofices.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }

        }
    }
}
