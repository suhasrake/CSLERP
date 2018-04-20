﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLERP.FileManager
{
    class ShortingListView : IComparer
    {
        private int ColumnNumber;
        private System.Windows.Forms.SortOrder SortOrder;

        public ShortingListView(int column_number,
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
    }
}