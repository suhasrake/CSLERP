using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLERP.Structures
{

    public class ComboBoxItem
    {
        string displayValue;
        string hiddenValue;

        //Constructor
        public ComboBoxItem(string d, string h)
        {
            displayValue = d;
            hiddenValue = h;
        }

        //Accessor
        public string HiddenValue
        {
            get
            {
                return hiddenValue;
            }
        }

        //Override ToString method
        public override string ToString()
        {
            return displayValue;
        }
    }
    public class ComboFUnctions
    {
        public static int getComboIndex(System.Windows.Forms.ComboBox cmb, string val)
        {
            int intex = 0;
            try
            {
                foreach (var item in cmb.Items)
                {
                    Structures.ComboBoxItem cbitem = (Structures.ComboBoxItem)item;
                    if (cbitem.HiddenValue == val)
                    {
                        return intex;
                    }
                    intex++;
                }
            }
            catch (Exception ex)
            {
            }
            return -1;
        }
    }
    public class GridViewComboBoxItem
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        public GridViewComboBoxItem(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
