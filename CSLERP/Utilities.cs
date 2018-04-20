using CSLERP.DBData;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLERP
{
    class Utilities
    {
        public static void exportToExcel(DataGridView dgv, string filename)
        {
            if (dgv.Rows.Count > 0)
            {
                try
                {
                    SaveFileDialog save = new SaveFileDialog();
                    save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    //save.Filter = "Text Files (*.txt)|*.txt";
                    save.FileName = String.Format("{0}.xls", filename + Utilities.getDateTimeString(DateTime.Now));
                    save.ShowDialog();
                    filename = save.FileName;
                    // Bind Grid Data to Datatable 

                    System.Data.DataTable dt = new System.Data.DataTable();
                    foreach (DataGridViewColumn col in dgv.Columns)
                    {
                        dt.Columns.Add(col.HeaderText, typeof(string));
                    }
                    int count = 0;
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (count < dgv.Rows.Count - 1)
                        {
                            dt.Rows.Add();
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                dt.Rows[dt.Rows.Count - 1][cell.ColumnIndex] = cell.Value.ToString();
                            }
                        }
                        count++;
                    }
                    // Bind table data to Stream Writer to export data to respective folder
                    System.IO.StreamWriter wr = new StreamWriter(filename);
                    // Write Columns to excel file
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        wr.Write(dt.Columns[i].ToString().ToUpper() + "\t");
                    }
                    wr.WriteLine();
                    //write rows to excel file
                    for (int i = 0; i < (dt.Rows.Count); i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (dt.Rows[i][j] != null)
                            {
                                wr.Write(Convert.ToString(dt.Rows[i][j]) + "\t");
                            }
                            else
                            {
                                wr.Write("\t");
                            }
                        }
                        wr.WriteLine();
                    }
                    wr.Close();
                    MessageBox.Show("Data Exported Successfully");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Export to Excel Failed");
                    throw ex;
                }
            }
        }
        public static string getDateTimeString(DateTime dt)
        {
            string dts = "";
            try
            {
                dts = dt.ToString("yyyyMMddHHmmss");
            }
            catch (Exception)
            {

                throw;
            }
            return dts;

        }

        public static string convertDTfromLocalToAnsi(DateTime dt)
        {
            string dts = "";
            try
            {
                string tStr = dt.ToString("");
                dts = tStr.Substring(6, 4) + "-" +
                    tStr.Substring(3, 2) + "-" +
                    tStr.Substring(0, 2) + " " +
                    tStr.Substring(11);
            }
            catch (Exception)
            {
                MessageBox.Show("convertDTfromLocalToAnsi() : Date conversion failed");
            }
            return dts;
        }

        public static string convertDatefromLocalToAnsi(DateTime dt)
        {
            string dts = "";
            try
            {
                string tStr = dt.ToString("");
                dts = tStr.Substring(6, 4) + "-" +
                    tStr.Substring(3, 2) + "-" +
                    tStr.Substring(0, 2);
            }
            catch (Exception)
            {
                MessageBox.Show("convertDatefromLocalToAnsi() : Date conversion failed");
            }
            return dts;
        }
        public static string convertDateStringToAnsi(string tStr)
        {
            string dts = "";
            try
            {
                dts = tStr.Substring(6, 4) + "-" +
                    tStr.Substring(3, 2) + "-" +
                    tStr.Substring(0, 2);
            }
            catch (Exception)
            {
                MessageBox.Show("convertDateStringToAnsi() : Date conversion failed");
            }
            return dts;
        }
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn
        (
         int nLeftRect, // x-coordinate of upper-left corner
         int nTopRect, // y-coordinate of upper-left corner
         int nRightRect, // x-coordinate of lower-right corner
         int nBottomRect, // y-coordinate of lower-right corner
         int nWidthEllipse, // height of ellipse
         int nHeightEllipse // width of ellipse
         );

        public static int checkStringInArray(string val, string[] arr)
        {
            int intex = -1;
            try
            {
                int count = 0;
                for (count = 0; count <= arr.Length - 1; count++)
                {
                    if (arr[count].StartsWith(val))
                    {
                        intex = count;
                        break;
                    }
                }
            }
            catch (Exception)
            {
                intex = -1;
            }
            return intex;
        }

        public static int checkMenuPrivilege(string val, string[] arr)
        {
            int intex = -1;
            try
            {
                int count = 0;
                for (count = 0; count <= arr.Length - 1; count++)
                {
                    string[] strArr = arr[count].Split(',');
                    ////if (arr[count].Trim().StartsWith(val.Trim()))
                    if (strArr[0].Equals(val.Trim()))
                    {
                        ////string[] strArr = arr[count].Split(',');
                        for (int i = 1; i < strArr.Length; i++) //lenghth-1, because Delete Privilege not implented as on 24/11/2016
                        {
                            string ss = strArr[i].Trim();
                            if (strArr[i].Trim().Length == 1)
                            {
                                intex = count;
                                break;
                            }
                        }
                        if (intex >= 0)
                            break;
                    }
                }
            }
            catch (Exception)
            {
                intex = -1;
            }
            return intex;
        }
        public static int checkMenuPrivilegeOld(string val, string[] arr)
        {
            int intex = -1;
            try
            {
                int count = 0;
                for (count = 0; count <= arr.Length - 1; count++)
                {
                    if (arr[count].StartsWith(val))
                    {
                        string[] strArr = arr[count].Split(',');
                        for (int i = 1; i < strArr.Length - 1; i++) //lenghth-1, because Delete Privilege not implented as on 24/11/2016
                        {
                            if (strArr[i].Trim().Length == 1)
                            {
                                intex = count;
                                break;
                            }
                        }
                        if (intex >= 0)
                            break;
                    }
                }
            }
            catch (Exception)
            {
                intex = -1;
            }
            return intex;
        }
        public static void showValidationErrorMessage(string errType)
        {
            try
            {
                string errString = "";
                switch (errType)
                {
                    case "Length":
                        errString = "Length Error";
                        break;
                    case "Value":
                        errString = "Value Type Error";
                        break;
                    case "Reference":
                        errString = "Value Reference Error";
                        break;
                    case "Null":
                        errString = "Null Value Error";
                        break;
                    case "Comaprison":
                        errString = "Value Comparison Error";
                        break;
                    default:
                        errString = "Validation error";
                        break;
                }
                MessageBox.Show(errString);
            }
            catch (Exception)
            {
                MessageBox.Show("showValidationErrorMessage() : Error");
            }
        }
        public static string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }
        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
        public static Boolean VarifyPassword(string str)
        {
            //IsLetterDigitOrSpecialCharacter
            //At least contain one Digit and one character
            int NoOfChar = 0;
            int NoOfInt = 0;
            Boolean status = false;
            if (Regex.IsMatch(str, "^[a-zA-Z0-9_!@#$%^&*()]+$") && (str.Length >= 5 && str.Length <= 10))
            {
                foreach (char c in str)
                {
                    if (Char.IsDigit(c))
                        NoOfInt++;
                    if (Char.IsLetter(c))
                        NoOfChar++;
                }
                if (NoOfInt > 0 && NoOfChar > 0)
                    status = true;
            }
            return status;
        }
        public static Boolean VarifyUserName(string str)
        {
            //Length between 5 and 10
            Boolean status = false;
            if (Regex.IsMatch(str, "^[a-zA-Z0-9_!@#$%^&*()]+$") && (str.Length >= 5 && str.Length <= 10))
            {
                status = true;
            }
            return status;
        }
        public static ListView GridColumnSelectionView(DataGridView grd)
        {
            ListView exlv = new ListView();
            try
            {
                exlv.View = System.Windows.Forms.View.Details;
                exlv.LabelEdit = true;
                exlv.AllowColumnReorder = true;
                exlv.CheckBoxes = true;
                exlv.FullRowSelect = true;
                exlv.GridLines = true;
                exlv.Sorting = System.Windows.Forms.SortOrder.Ascending;
                exlv.Columns.Add("Select", -2, HorizontalAlignment.Left);
                exlv.Columns.Add("Column Name", -2, HorizontalAlignment.Left);
                exlv.Columns.Add("width", -2, HorizontalAlignment.Left);
                exlv.Columns[2].Width = 0;
                for (int i = grd.Columns.Count - 1; i >= 0; i--)
                {
                    int daat = grd.Columns[i].Width / 4;
                    string str = grd.Columns[i].HeaderText;
                    ListViewItem item1 = new ListViewItem();
                    item1.Checked = true;
                    if (grd.Columns[i].CellType != typeof(System.Windows.Forms.DataGridViewButtonCell)
                        && grd.Columns[i].Visible == true)
                    {
                        item1.SubItems.Add(grd.Columns[i].HeaderText);
                        item1.SubItems.Add(daat.ToString());
                        item1.SubItems.Add(grd.Columns[i].Name);
                        exlv.Items.Add(item1);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return exlv;
        }

        public static void export2Excel(string heading1, string heading2, string heading3, DataGridView dgv, ListView lvcolumns)
        {
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            //excel.Windows.Application.ActiveWindow.DisplayGridlines = false;
            //excel.ScreenUpdating = false;
            try
            {
                worksheet = workbook.ActiveSheet;
                worksheet.Columns.ColumnWidth = 20;

                ////worksheet.Cells.HorizontalAlignment = ContentAlignment.MiddleCenter;

                worksheet.Columns.WrapText = true;
                worksheet.Cells[1, 1] = heading1;
                worksheet.Cells[2, 1] = heading2;
                worksheet.Cells.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                ////worksheet.get_Range("A1", "B2").HorizontalAlignment = ContentAlignment.MiddleLeft;
                ////worksheet.get_Range("A1", "B2").Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                worksheet.get_Range("A1", "B2").WrapText = false;
                worksheet.get_Range("A1", "B2").Font.Bold = true;
                worksheet.get_Range("A1", "B2").Font.Size = 15;
                worksheet.get_Range("A1", "B2").Font.Color = Color.SeaGreen;
                string[] detail = { };
                if (heading3.Length != 0)
                    detail = heading3.Split(Main.delimiter2);
                int k = 3;
                for (int i = 0; i < detail.Length; i++)
                {
                    try
                    {
                        DateTime temp;
                        string str1 = detail[i].Substring(0, detail[i].IndexOf(Main.delimiter1));
                        string str2 = detail[i].Substring(detail[i].IndexOf(Main.delimiter1) + 1);

                        worksheet.Cells[k, 1] = str1;
                        if (DateTime.TryParse(str2, out temp))
                            worksheet.Cells[k, 2] = (Convert.ToDateTime(str2)).Date;
                        else
                            worksheet.Cells[k, 2] = str2;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("export2Excel() : ERROR in header String format");
                    }
                    k++;
                }
                worksheet.Name = heading1;
                int col = 0;
                DateTime dt;
                //---------16/10/2017
                int[] tArray = new Int32[dgv.ColumnCount];
                int colCount = 0;
                int colNumber = 0;
                foreach (ListViewItem itemRow in lvcolumns.Items)
                {
                    if (itemRow.Checked)
                    {
                        int cNumber = dgv.Columns.IndexOf(dgv.Columns[itemRow.SubItems[3].Text]);
                        tArray[colNumber] = cNumber;
                        colNumber++;
                    }
                    colCount++;
                }
                for (int i = colNumber; i < tArray.Length; i++)
                {
                    tArray[i] = -1;
                }
                //int row = 3;
                int row = k;
                //print heading
                //Boolean flip = true;
                for (int i = 0; i < colNumber; i++)
                {
                    int cNumber = tArray[i];
                    worksheet.Cells[row, i + 1] = dgv.Columns[cNumber].HeaderText;
                    worksheet.Cells[row, i + 1].Interior.Color = Color.Orange;
                }
                row++;
                //Using Range
                int a = 0;
                object[,] data = new object[dgv.Rows.Count, dgv.Columns.Count];
                foreach (DataGridViewRow rw in dgv.Rows)
                {
                    int b = 0;
                    foreach (DataGridViewCell c in rw.Cells)
                    {
                        if (tArray.Contains(c.ColumnIndex))
                        {
                            data[a, b] = c.Value;
                            b++;
                        }
                    }
                    a++;
                }
                worksheet.Range[worksheet.Cells[row, 1], worksheet.Cells[dgv.Rows.Count + k, dgv.Columns.Count]].value = data;
                int lastrow = worksheet.UsedRange.Rows.Count;
                int lastcolumn = worksheet.UsedRange.Columns.Count;
                Microsoft.Office.Interop.Excel.Range rng = worksheet.Range[worksheet.Cells[row, 1], worksheet.Cells[lastrow, lastcolumn]];
                Microsoft.Office.Interop.Excel.FormatCondition format = rng.Rows.FormatConditions.
                    Add(Microsoft.Office.Interop.Excel.XlFormatConditionType.xlExpression,
                        Microsoft.Office.Interop.Excel.XlFormatConditionOperator.xlEqual, "=MOD(ROW(),2)=0");
                format.Interior.Color = Microsoft.Office.Interop.Excel.XlRgbColor.rgbLightGreen;
                Microsoft.Office.Interop.Excel.Range rangeData = excel.Range[worksheet.Cells[row, 1], worksheet.Cells[lastrow, lastcolumn]];
                rangeData.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                Microsoft.Office.Interop.Excel.Range rangeHead = excel.Range[worksheet.Cells[row - 1, 1], worksheet.Cells[row - 1, lastcolumn]];
                rangeHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                //-----------
                //excel.ScreenUpdating = false;
                SaveFileDialog saveDialog = new SaveFileDialog();
                ////saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveDialog.FilterIndex = 2;
                saveDialog.OverwritePrompt = false;
                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Export Successful");
                }
                workbook.Close(false, Type.Missing, Type.Missing);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export Failed");
            }
        }
        public static void export2Excel20102017(string heading1, string heading2, string heading3, DataGridView dgv, ListView lvcolumns)
        {
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            excel.Windows.Application.ActiveWindow.DisplayGridlines = false;
            try
            {
                worksheet = workbook.ActiveSheet;
                worksheet.Columns.ColumnWidth = 20;

                ////worksheet.Cells.HorizontalAlignment = ContentAlignment.MiddleCenter;

                worksheet.Columns.WrapText = true;
                worksheet.Cells[1, 1] = heading1;
                worksheet.Cells[2, 1] = heading2;
                worksheet.Cells.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                ////worksheet.get_Range("A1", "B2").HorizontalAlignment = ContentAlignment.MiddleLeft;
                ////worksheet.get_Range("A1", "B2").Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                worksheet.get_Range("A1", "B2").WrapText = false;
                worksheet.get_Range("A1", "B2").Font.Bold = true;
                worksheet.get_Range("A1", "B2").Font.Size = 15;
                worksheet.get_Range("A1", "B2").Font.Color = Color.SeaGreen;
                string[] detail = { };
                if (heading3.Length != 0)
                    detail = heading3.Split(Main.delimiter2);
                int k = 3;
                for (int i = 0; i < detail.Length; i++)
                {
                    try
                    {
                        DateTime temp;
                        string str1 = detail[i].Substring(0, detail[i].IndexOf(Main.delimiter1));
                        string str2 = detail[i].Substring(detail[i].IndexOf(Main.delimiter1) + 1);

                        worksheet.Cells[k, 1] = str1;
                        if (DateTime.TryParse(str2, out temp))
                            worksheet.Cells[k, 2] = (Convert.ToDateTime(str2)).Date;
                        else
                            worksheet.Cells[k, 2] = str2;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("export2Excel() : ERROR in header String format");
                    }
                    k++;
                }
                worksheet.Name = heading1;
                int col = 0;
                DateTime dt;
                //---------16/10/2017
                int[] tArray = new Int32[dgv.ColumnCount];
                int colCount = 0;
                int colNumber = 0;
                foreach (ListViewItem itemRow in lvcolumns.Items)
                {
                    if (itemRow.Checked)
                    {
                        int cNumber = dgv.Columns.IndexOf(dgv.Columns[itemRow.SubItems[3].Text]);
                        tArray[colNumber] = cNumber;
                        colNumber++;
                    }
                    colCount++;
                }
                for (int i = colNumber; i < tArray.Length; i++)
                {
                    tArray[i] = -1;
                }
                int row = 3;
                //print heading
                Boolean flip = true;
                for (int i = 0; i < colNumber; i++)
                {
                    int cNumber = tArray[i];
                    worksheet.Cells[row, i + 1] = dgv.Columns[cNumber].HeaderText;
                    worksheet.Cells[row, i + 1].Interior.Color = Color.Orange;
                }
                row++;

                //print details
                DateTime stime = DateTime.Now;
                for (int j = 0; j < dgv.Rows.Count; j++)
                {
                    for (int i = 0; i < colNumber; i++)
                    {
                        int cNumber = tArray[i];

                        if (dgv.Rows[j].Cells[cNumber].Value == null || dgv.Rows[j].Cells[cNumber].Value.ToString() == "")
                        {
                            worksheet.Cells[row, i + 1] = "";
                        }
                        else
                        {
                            string str1 = dgv.Rows[j].Cells[cNumber].Value.ToString();
                            worksheet.Cells[row, i + 1] = str1;
                        }

                        if (flip)
                        {
                            worksheet.Cells[row, i + 1].Interior.Color = Color.LightGreen;
                        }
                        else
                        {
                            worksheet.Cells[row, i + 1].Interior.Color = Color.LightSkyBlue;
                        }
                    }
                    if (flip)
                    {
                        flip = false;
                    }
                    else
                    {
                        flip = true;
                    }

                    row++;
                }
                DateTime etime = DateTime.Now;
                //-----------

                worksheet.get_Range("A1", "A30").Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                SaveFileDialog saveDialog = new SaveFileDialog();
                ////saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveDialog.FilterIndex = 2;
                saveDialog.OverwritePrompt = false;
                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Export Successful");
                }
                workbook.Close(false, Type.Missing, Type.Missing);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export Failed");
            }
        }

        public static void export2ExcelOld(string heading1, string heading2, string heading3, DataGridView dgv, ListView lvcolumns)
        {
            Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
            Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
            excel.Windows.Application.ActiveWindow.DisplayGridlines = false;
            try
            {
                worksheet = workbook.ActiveSheet;
                worksheet.Columns.ColumnWidth = 20;

                ////worksheet.Cells.HorizontalAlignment = ContentAlignment.MiddleCenter;

                worksheet.Columns.WrapText = true;
                worksheet.Cells[1, 1] = heading1;
                worksheet.Cells[2, 1] = heading2;
                worksheet.Cells.Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                ////worksheet.get_Range("A1", "B2").HorizontalAlignment = ContentAlignment.MiddleLeft;
                ////worksheet.get_Range("A1", "B2").Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
                worksheet.get_Range("A1", "B2").WrapText = false;
                worksheet.get_Range("A1", "B2").Font.Bold = true;
                worksheet.get_Range("A1", "B2").Font.Size = 15;
                worksheet.get_Range("A1", "B2").Font.Color = Color.SeaGreen;
                string[] detail = { };
                if (heading3.Length != 0)
                    detail = heading3.Split(Main.delimiter2);
                int k = 3;
                for (int i = 0; i < detail.Length; i++)
                {
                    try
                    {
                        DateTime temp;
                        string str1 = detail[i].Substring(0, detail[i].IndexOf(Main.delimiter1));
                        string str2 = detail[i].Substring(detail[i].IndexOf(Main.delimiter1) + 1);

                        worksheet.Cells[k, 1] = str1;
                        if (DateTime.TryParse(str2, out temp))
                            worksheet.Cells[k, 2] = (Convert.ToDateTime(str2)).Date;
                        else
                            worksheet.Cells[k, 2] = str2;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ERROR in HeaderDetail String format");
                    }
                    k++;
                }
                worksheet.Name = heading1;
                int col = 0;
                DateTime dt;
                foreach (ListViewItem itemRow in lvcolumns.Items)
                {
                    if (itemRow.Checked)
                    {
                        int row = k + 1;
                        col = col + 1;
                        string str = itemRow.SubItems[1].Text;
                        string[] format = { "dd-MM-yyyy", "dd/MM/yyyy", "dd/MM/yyyy hh:mm:ss", "dd-MM-yyyy hh:mm:ss", "dd/MM/yyyy hh:mm:ss tt", "dd-MM-yyyy hh:mm:ss tt",
                        "dd-MM-yy", "dd/MM/yy", "dd/MM/yy hh:mm:ss", "dd-MM-yy hh:mm:ss", "dd/MM/yy hh:mm:ss tt", "dd-MM-yy hh:mm:ss tt"};
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            if (dgv.Columns[i].HeaderText.Equals(itemRow.SubItems[1].Text))
                            // if column is selected for export
                            {
                                ////worksheet.Columns[col].ColumnWidth = itemRow.SubItems[2].Text;
                                worksheet.Cells[row, col] = dgv.Columns[i].HeaderText;
                                worksheet.Cells[row, col].Interior.Color = Color.Orange;
                                Boolean flip = true;
                                for (int j = 0; j <= dgv.Rows.Count - 1; j++)
                                {
                                    row = row + 1;
                                    if (dgv.Rows[j].Cells[i].Value == null || dgv.Rows[j].Cells[i].Value.ToString() == "")
                                    {
                                        worksheet.Cells[row, col] = "";
                                    }
                                    else
                                    {
                                        string str1 = dgv.Rows[j].Cells[i].Value.ToString();
                                        worksheet.Cells[row, col] = str1;
                                        //string format = "";
                                        ////if (DateTime.TryParseExact(dgv.Rows[j].Cells[i].Value.ToString(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                                        ////    worksheet.Cells[row + 1, col] = Convert.ToDateTime(dgv.Rows[j].Cells[i].Value.ToString());
                                        ////else
                                        ////    worksheet.Cells[row + 1, col] = dgv.Rows[j].Cells[i].Value.ToString();
                                    }
                                    if (flip)
                                    {
                                        worksheet.Cells[row, col].Interior.Color = Color.LightGreen;
                                        flip = false;
                                    }
                                    else
                                    {
                                        worksheet.Cells[row, col].Interior.Color = Color.LightSkyBlue;
                                        flip = true;
                                    }
                                    worksheet.Cells[row, col].Borders.Color =
                                    System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);

                                }
                            }
                        }
                    }
                }
                worksheet.get_Range("A1", "A30").Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                SaveFileDialog saveDialog = new SaveFileDialog();
                ////saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                saveDialog.FilterIndex = 2;
                saveDialog.OverwritePrompt = false;
                if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    workbook.SaveAs(saveDialog.FileName);
                    MessageBox.Show("Export Successful");
                }
                workbook.Close(false, Type.Missing, Type.Missing);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Export Failed");
            }
        }


        ////public static void export2ExcelOld(string heading1, string heading2, string heading3, DataGridView dgv, ListView lvcolumns)
        ////{
        ////    Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
        ////    Microsoft.Office.Interop.Excel._Workbook workbook = excel.Workbooks.Add(Type.Missing);
        ////    Microsoft.Office.Interop.Excel._Worksheet worksheet = null;

        ////    try
        ////    {
        ////        worksheet = workbook.ActiveSheet;
        ////        worksheet.Columns.ColumnWidth = 30;
        ////        worksheet.Cells[1, 1] = heading1;
        ////        worksheet.Cells[2, 1] = heading2;
        ////        string[] detail = { };
        ////        if (heading3.Length != 0)
        ////            detail = heading3.Split(Main.delimiter2);
        ////        int k = 3;
        ////        for (int i = 0; i < detail.Length; i++)
        ////        {
        ////            try
        ////            {
        ////                DateTime temp;
        ////                string str1 = detail[i].Substring(0, detail[i].IndexOf(Main.delimiter1));
        ////                string str2 = detail[i].Substring(detail[i].IndexOf(Main.delimiter1) + 1);

        ////                worksheet.Cells[k, 1] = str1;
        ////                if (DateTime.TryParse(str2, out temp))
        ////                    worksheet.Cells[k, 2] = (Convert.ToDateTime(str2)).Date;
        ////                else
        ////                    worksheet.Cells[k, 2] = str2;
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                MessageBox.Show("ERROR in HeaderDetail String format");
        ////            }
        ////            k++;
        ////        }
        ////        worksheet.Name = heading1;
        ////        int col = 0;
        ////        DateTime dt;
        ////        foreach (ListViewItem itemRow in lvcolumns.Items)
        ////        {
        ////            if (itemRow.Checked)
        ////            {
        ////                int row = k + 1;
        ////                col = col + 1;
        ////                string str = itemRow.SubItems[1].Text;
        ////                string[] format = { "dd-MM-yyyy", "dd/MM/yyyy", "dd/MM/yyyy hh:mm:ss", "dd-MM-yyyy hh:mm:ss", "dd/MM/yyyy hh:mm:ss tt", "dd-MM-yyyy hh:mm:ss tt",
        ////                "dd-MM-yy", "dd/MM/yy", "dd/MM/yy hh:mm:ss", "dd-MM-yy hh:mm:ss", "dd/MM/yy hh:mm:ss tt", "dd-MM-yy hh:mm:ss tt"};
        ////                for (int i = 0; i < dgv.Columns.Count; i++)
        ////                {
        ////                    if (dgv.Columns[i].HeaderText.Equals(itemRow.SubItems[1].Text))
        ////                    {
        ////                        worksheet.Columns[col].ColumnWidth = itemRow.SubItems[2].Text;
        ////                        worksheet.Cells[row, col] = dgv.Columns[i].HeaderText;
        ////                        worksheet.Cells[row, col].Interior.Color = Color.LightBlue;
        ////                        for (int j = 0; j <= dgv.Rows.Count - 1; j++)
        ////                        {
        ////                            if (dgv.Rows[j].Cells[i].Value == null || dgv.Rows[j].Cells[i].Value.ToString() == "")
        ////                            {
        ////                                worksheet.Cells[row + 1, col] = "";
        ////                            }
        ////                            else
        ////                            {
        ////                                string str1 = dgv.Rows[j].Cells[i].Value.ToString();

        ////                                //string format = "";
        ////                                if (DateTime.TryParseExact(dgv.Rows[j].Cells[i].Value.ToString(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
        ////                                    worksheet.Cells[row + 1, col] = Convert.ToDateTime(dgv.Rows[j].Cells[i].Value.ToString());
        ////                                else
        ////                                    worksheet.Cells[row + 1, col] = dgv.Rows[j].Cells[i].Value.ToString();
        ////                            }
        ////                            row = row + 1;
        ////                        }
        ////                    }
        ////                }
        ////            }
        ////        }
        ////        worksheet.get_Range("A1", "A30").Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        ////        SaveFileDialog saveDialog = new SaveFileDialog();
        ////        ////saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
        ////        saveDialog.Filter = "Excel files (*.xlsx)|*.xlsx";
        ////        saveDialog.FilterIndex = 2;
        ////        saveDialog.OverwritePrompt = false;
        ////        if (saveDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        ////        {
        ////            workbook.SaveAs(saveDialog.FileName);
        ////            MessageBox.Show("Export Successful");
        ////        }
        ////        workbook.Close();
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        MessageBox.Show("Export Failed");
        ////    }
        ////}
        public static Boolean verifyFinancialYear(string yearString, DateTime date)
        {
            Boolean stat = true;
            try
            {
                string[] str = yearString.Split(':');
                string s = str[0];
                string ss = str[1];
                string sss = str[2];
                DateTime startDate = Convert.ToDateTime(ss.Trim());
                DateTime lastDate = Convert.ToDateTime(sss.Trim());
                if (date.Date < startDate.Date || date.Date > lastDate.Date)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                stat = false;
            }
            return stat;
        }
        public static Panel createProcessPanel()
        {
            Panel prPanel = new Panel();

            try
            {
                prPanel.BackColor = Color.White;
                prPanel.Location = new System.Drawing.Point(530, 500);
                prPanel.Size = new Size(200, 36);

                System.Windows.Forms.Label prLabel = new System.Windows.Forms.Label();
                prLabel.Location = new System.Drawing.Point(10, 10);
                prLabel.Size = new Size(121, 13);
                prLabel.Location = new System.Drawing.Point(14, 12);
                PictureBox prPicture = new PictureBox();
                prPicture.Size = new Size(38, 29);
                prPicture.Location = new System.Drawing.Point(141, 2);
                prPicture.Image = CSLERP.Properties.Resources.ajax_loader__2_;
                prLabel.Text = "Processing...please wait";
                prPanel.Controls.Add(prLabel);
                prPanel.Controls.Add(prPicture);
                prPanel.Visible = false;
                prPanel.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
                prPanel.Name = "processPanel";
            }
            catch (Exception ex)
            {
            }
            return prPanel;
        }
        public static string replaceSingleQuoteWithDoubleSingleQuote(string str2Replace)
        {
            string replacedStr = "";
            try
            {
                replacedStr = str2Replace.Replace("'", "''");
            }
            catch (Exception ex)
            {
            }
            return replacedStr;
        }
        //2018-02-20
        public static Boolean[] fillItemPrivileges(string[] userOptionArray, string menuID)
        {
            Boolean[] menuPrevValue = new Boolean[4];
            try
            {
                int intex = Utilities.checkMenuPrivilege(menuID, userOptionArray);
                if (intex >= 0)
                {
                    string[] prvArr = userOptionArray[intex].Split(',');
                    if (prvArr[1].Equals("V"))
                    {
                        menuPrevValue[0] = true;
                    }
                    else
                    {
                        menuPrevValue[0] = false;
                    }
                    if (prvArr[2].Equals("A"))
                    {
                        menuPrevValue[1] = true;
                    }
                    else
                    {
                        menuPrevValue[1] = false;
                    }
                    if (prvArr[3].Equals("E"))
                    {
                        menuPrevValue[2] = true;
                    }
                    else
                    {
                        menuPrevValue[2] = false;
                    }
                    if (prvArr[4].Equals("D"))
                    {
                        menuPrevValue[3] = true;
                    }
                    else
                    {
                        menuPrevValue[3] = false;
                    }
                }
            }
            catch (Exception)
            {
                menuPrevValue[0] = menuPrevValue[1] = menuPrevValue[2] = menuPrevValue[3] = false;
                MessageBox.Show("Main() : Error 7");
            }
            return menuPrevValue;
        }
        public static string getPaymentTermsExplained(string ptstr)
        {
            string ptexplainrd = "";
            PTDefinitionDB ptddb = new PTDefinitionDB();
            List<ptdefinition> PTDefinitions = ptddb.getPTDefinitions();
            string[] strArry1 = ptstr.Split(new string[] { ";" }, StringSplitOptions.None);
            for (int i = 0; i < strArry1.Length; i++)
            {
                if (strArry1[i].Trim().Length > 0)
                {
                    string[] strArry2 = strArry1[i].Split(new string[] { "-" }, StringSplitOptions.None);
                    foreach (ptdefinition ptd in PTDefinitions)
                    {
                        if (ptd.SequenceNo == Convert.ToInt32(strArry2[0]))
                        {
                            ptexplainrd = ptexplainrd + ptd.ShortDescription + " : " + strArry2[1] + "%";
                            if (Convert.ToInt32(strArry2[2]) != 0)
                            {
                                ptexplainrd = ptexplainrd + " in " + strArry2[2] + " days";
                            }
                            ptexplainrd = ptexplainrd + "\n";
                            break;
                        }
                    }
                }
            }
            return ptexplainrd;
        }
    }
}
