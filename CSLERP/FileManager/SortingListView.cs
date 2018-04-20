using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLERP.FileManager
{
    class SortingListView : IComparer
    {
        private int ColumnNumber;
        private System.Windows.Forms.SortOrder SortOrder;

        public SortingListView(int column_number,
            System.Windows.Forms.SortOrder sort_order)
        {
            ColumnNumber = column_number;
            SortOrder = sort_order;
        }
        // Compare two ListViewItems.
        public int Compare(object object_x, object object_y)
        {
            // Get the objects as ListViewItems.
            ListViewItem item_x = object_x as ListViewItem;
            ListViewItem item_y = object_y as ListViewItem;

            // Get the corresponding sub-item values.
            string string_x;
            if (item_x.SubItems.Count <= ColumnNumber)
            {
                string_x = "";
            }
            else
            {
                string_x = item_x.SubItems[ColumnNumber].Text;
            }

            string string_y;
            if (item_y.SubItems.Count <= ColumnNumber)
            {
                string_y = "";
            }
            else
            {
                string_y = item_y.SubItems[ColumnNumber].Text;
            }
            int result;
            double double_x, double_y;
            if (double.TryParse(string_x, out double_x) &&
                double.TryParse(string_y, out double_y))
            {
                // For number.
                result = double_x.CompareTo(double_y);
            }
            else
            {
                DateTime date_x, date_y;
                if (DateTime.TryParse(string_x, out date_x) &&
                    DateTime.TryParse(string_y, out date_y))
                {
                    // For date.
                    result = date_x.CompareTo(date_y);
                }
                else
                {
                    // For string.
                    result = string_x.CompareTo(string_y);
                }
            }
            if (SortOrder == System.Windows.Forms.SortOrder.Ascending)
            {
                return result;
            }
            else
            {
                return -result;
            }
        }
        public static System.Windows.Forms.SortOrder getSortedOrder(string first, string last)
        {
            System.Windows.Forms.SortOrder sort_order2 = System.Windows.Forms.SortOrder.None;
            try
            {
                double d1, d2;
                int result;
                if (double.TryParse(first, out d1) && double.TryParse(last, out d2))
                {
                    result = d1.CompareTo(d2);
                }
                else
                {
                    DateTime dt1, dt2;
                    if (DateTime.TryParse(first, out dt1) && DateTime.TryParse(last, out dt2))
                    {
                        result = dt1.CompareTo(dt2);
                    }
                    else
                        result = first.CompareTo(last);
                }
                if (result < 0)
                    sort_order2 = System.Windows.Forms.SortOrder.Descending;
                else
                    sort_order2 = System.Windows.Forms.SortOrder.Ascending;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Sorting Error");
            }
            return sort_order2;
        }
    }
}
