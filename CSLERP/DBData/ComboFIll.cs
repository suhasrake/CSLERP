using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLERP.DBData
{
    class ComboFIll
    {
        public static void fillStaticCombos()
        {
            try
            {
                Main.statusValues = new string[2, 2]
                        {
                    {"1","Active" },
                    {"2","Deactive" }
                        };
                Main.userTypeValues = new string[1, 2]
                 {
                    {"1","CSL Employee" }

                 };
                Main.CurrencyConversionTypeValues = new string[2, 2]
                   {
                    {"1","Import" },
                    {"2","Export" }
                   };
                Main.QualityValues = new string[2, 2]
                  {
                    {"1","Good" },
                    {"2","Bad" }
                  };
                Main.AcceptanceStatus = new string[2, 2]
                  {
                    {"1","Accepted" },
                    {"2","Rejected" }
                  };
                Main.YesNo = new string[2, 2]
                  {
                    {"1","Yes" },
                    {"2","No" }
                  };
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        public static string getStatusString(int Status)
        {
            string StatusString = "Unknown";
            try
            {
                for (int i = 0; i < Main.statusValues.GetLength(0); i++)
                {
                    if (Convert.ToInt32(Main.statusValues[i, 0]) == Status)
                    {
                        StatusString = Main.statusValues[i, 1];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                StatusString = "Unknown";
            }
            return StatusString;
        }
        public static string getCurrencyConversionTypeString(int Type)
        {
            string CurrencyConversionTypeString = "Unknown";
            try
            {
                for (int i = 0; i < Main.CurrencyConversionTypeValues.GetLength(0); i++)
                {
                    if (Convert.ToInt32(Main.CurrencyConversionTypeValues[i, 0]) == Type)
                    {
                        CurrencyConversionTypeString = Main.CurrencyConversionTypeValues[i, 1];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                CurrencyConversionTypeString = "Unknown";
            }
            return CurrencyConversionTypeString;
        }
        public static int getStatusCode(string StatusString)
        {
            int StatusCode = 0;
            try
            {
                for (int i = 0; i < Main.statusValues.GetLength(0); i++)
                {
                    if (Main.statusValues[i, 1].Equals(StatusString))
                    {
                        StatusCode = Convert.ToInt32(Main.statusValues[i, 0]);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                StatusCode = 0;
            }
            return StatusCode;
        }
        public static int getCurrencyConversionTypeCode(string TypeString)
        {
            int TypeCode = 0;
            try
            {
                for (int i = 0; i < Main.CurrencyConversionTypeValues.GetLength(0); i++)
                {
                    if (Main.CurrencyConversionTypeValues[i, 1].Equals(TypeString))
                    {
                        TypeCode = Convert.ToInt32(Main.CurrencyConversionTypeValues[i, 0]);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                TypeCode = 0;
            }
            return TypeCode;
        }
        public static string getUserTypeString(int userType)
        {
            string userTypeString = "Unknown";
            try
            {
                for (int i = 0; i < Main.userTypeValues.GetLength(0); i++)
                {
                    if (Convert.ToInt32(Main.userTypeValues[i, 0]) == userType)
                    {
                        userTypeString = Main.userTypeValues[i, 1];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                userTypeString = "Unknown";
            }
            return userTypeString;
        }
        public static int getUserTypeCode(string userTypeString)
        {
            int userTypeCode = 0;
            try
            {
                for (int i = 0; i < Main.userTypeValues.GetLength(0); i++)
                {
                    if (Main.userTypeValues[i, 1].Equals(userTypeString))
                    {
                        userTypeCode = Convert.ToInt32(Main.userTypeValues[i, 0]);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                userTypeCode = 0;
            }
            return userTypeCode;
        }
        public static string getDocumentStatusString(int docStatus)
        {
            string userTypeString = "Unknown";
            try
            {
                if (docStatus == 1)
                    userTypeString = "Created";
                else if (docStatus > 1 && docStatus < 99)
                    userTypeString = "Forwarded";
                else if (docStatus == 99)
                    userTypeString = "Approved";
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                userTypeString = "Unknown";
            }
            return userTypeString;
        }
        public static void fillStatusCombo(System.Windows.Forms.ComboBox cmb)
        {
            try
            {
                cmb.Items.Clear();
                for (int i = 0; i < Main.statusValues.GetLength(0); i++)
                {
                    cmb.Items.Add(Main.statusValues[i, 1]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        public static void fillQualityGridViewCombo(System.Windows.Forms.DataGridViewComboBoxCell cmb)
        {
            try
            {
                cmb.Items.Clear();
                for (int i = 0; i < Main.QualityValues.GetLength(0); i++)
                {
                    cmb.Items.Add(Main.QualityValues[i, 1]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name+"() : Error");
            }
        }
        public static void fillQualitysCombo(System.Windows.Forms.ComboBox cmb)
        {
            try
            {
                cmb.Items.Clear();
                for (int i = 0; i < Main.QualityValues.GetLength(0); i++)
                {
                    cmb.Items.Add(Main.QualityValues[i, 1]);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        public static void fillAcceptanceStatusCombo(System.Windows.Forms.ComboBox cmb)
        {
            try
            {
                cmb.Items.Clear();
                for (int i = 0; i < Main.AcceptanceStatus.GetLength(0); i++)
                {
                    cmb.Items.Add(Main.AcceptanceStatus[i, 1]);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        public static void fillAcceptanceStatusGridViewCombo(System.Windows.Forms.DataGridViewComboBoxCell cmb)
        {
            try
            {
                cmb.Items.Clear();
                for (int i = 0; i < Main.AcceptanceStatus.GetLength(0); i++)
                {
                    cmb.Items.Add(Main.AcceptanceStatus[i, 1]);
                }
            }
            catch (Exception)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
            }
        }
        public static int getAcceptanceStatusCode(string StatusString)
        {
            int StatusCode = 0;
            try
            {
                for (int i = 0; i < Main.AcceptanceStatus.GetLength(0); i++)
                {
                    if (Main.AcceptanceStatus[i, 1].Equals(StatusString))
                    {
                        StatusCode = Convert.ToInt32(Main.AcceptanceStatus[i, 0]);
                        break;
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                StatusCode = 0;
            }
            return StatusCode;
        }

        public static int getQualityID(string QualityString)
        {
            int QualityID = 0;
            try
            {
                for (int i = 0; i < Main.QualityValues.GetLength(0); i++)
                {
                    if (Main.QualityValues[i, 1].Equals(QualityString))
                    {
                        QualityID = Convert.ToInt32(Main.QualityValues[i, 0]);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                QualityID = 0;
            }
            return QualityID;
        }


        public static string getQualityString(int QualityID)
        {
            string QualityString = "";
            try
            {
                for (int i = 0; i < Main.QualityValues.GetLength(0); i++)
                {
                    if (Convert.ToInt32(Main.QualityValues[i, 0]) == QualityID)
                    {
                        QualityString = Main.QualityValues[i, 1];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                QualityString = "";
            }
            return QualityString;
        }

        public static string getAcceptanceStatusString(int StatusCode)
        {
            string StatusString = "";
            try
            {
                for (int i = 0; i < Main.AcceptanceStatus.GetLength(0); i++)
                {
                    if (Convert.ToInt32( Main.AcceptanceStatus[i, 0]) == StatusCode)
                    {
                        StatusString = Main.AcceptanceStatus[i, 1];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(System.Reflection.MethodBase.GetCurrentMethod().Name + "() : Error");
                StatusString = "";
            }
            return StatusString;
        }

    }
}
