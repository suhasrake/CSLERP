using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSLERP.PrintForms
{
    class NumberToString
    {
        //accept currency id also
        //if not INR no paisa part
        public static string convert(string str)
        {
            long no = 0;
            int points = 0;
            try
            {
                int decimalPlace = str.IndexOf(".");
                if (decimalPlace > 0)
                {
                    string s = str.Substring(decimalPlace + 1);
                    if (s.Length != 2)
                    {
                        if (s.Length == 1)
                            s = s + "0";
                        else
                            s = s.Substring(0, 2);
                    }
                    str = str.Substring(0, decimalPlace + 2);
                    no = Convert.ToInt64(str.Substring(0, decimalPlace));
                    points = Convert.ToInt32(s);
                }
                else
                    no = Convert.ToInt64(str);
            }
            catch (Exception ex)
            {
            }
            NumberToString p = new NumberToString();
            String rupee = p.ConvertNumbertoWords(no);
            string paise = p.ConvertNumbertoWords(points);
            string convertion = "";
            if (paise.Equals("Zero"))
                convertion = "    INR " + rupee + " Only";
            else
                convertion = "    INR " + rupee + " And " + paise + " Paise only";
            return convertion;

        }
        public static string convertFC(string str, string currencyID)
        {
            long no = 0;
            int points = 0;
            try
            {
                int decimalPlace = str.IndexOf(".");
                if (decimalPlace > 0)
                {
                    string s = str.Substring(decimalPlace + 1);
                    if (s.Length != 2)
                    {
                        if (s.Length == 1)
                            s = s + "0";
                        else
                            s = s.Substring(0, 2);
                    }
                    str = str.Substring(0, decimalPlace + 2);
                    no = Convert.ToInt64(str.Substring(0, decimalPlace));
                    points = Convert.ToInt32(s);
                }
                else
                    no = Convert.ToInt64(str);
            }
            catch (Exception ex)
            {
            }
            NumberToString p = new NumberToString();
            String rupee = p.ConvertNumbertoWordsMillions(no);
            //string paise = p.ConvertNumbertoWords(points);
            string convertion = "";
            
            convertion = currencyID+ " " + rupee + " Only";
           
            return convertion;

        }
        public string ConvertNumbertoWords(long number)
        {
            string words = "";
            try
            {
                if (number == 0) return "Zero";
                if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
                if ((number / 10000000) > 0)
                {
                    words += ConvertNumbertoWords(number / 10000000) + " Crores ";
                    number %= 10000000;
                }
                if ((number / 100000) > 0)
                {
                    words += ConvertNumbertoWords(number / 100000) + " Lakh ";
                    number %= 100000;
                }
                if ((number / 1000) > 0)
                {
                    words += ConvertNumbertoWords(number / 1000) + " Thousand ";
                    number %= 1000;
                }
                if ((number / 100) > 0)
                {
                    words += ConvertNumbertoWords(number / 100) + " Hundred ";
                    number %= 100;
                }
                if (number > 0)
                {
                    //if (words != "")
                    //    words += "And ";
                    var unitsMap = new[]
                    {
                        "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
                        "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
                    };
                    var tensMap = new[]
                    {
                        "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
                    };
                    if (number < 20)
                        words += unitsMap[number];
                    else
                    {
                        words += tensMap[number / 10];
                        if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return words;
        }
        public string ConvertNumbertoWordsMillions(long number)
        {
            string words = "";
            try
            {
                if (number == 0) return "Zero";
                if (number < 0) return "minus " + ConvertNumbertoWordsMillions(Math.Abs(number));
                if ((number / 1000000000) > 0)
                {
                    words += ConvertNumbertoWordsMillions(number / 1000000000) + " Billion ";
                    number %= 1000000000;
                }
                if ((number / 1000000) > 0)
                {
                    words += ConvertNumbertoWordsMillions(number / 1000000) + " Million ";
                    number %= 1000000;
                }
                if ((number / 1000) > 0)
                {
                    words += ConvertNumbertoWordsMillions(number / 1000) + " Thousand ";
                    number %= 1000;
                }
                if ((number / 100) > 0)
                {
                    words += ConvertNumbertoWordsMillions(number / 100) + " Hundred ";
                    number %= 100;
                }
                if (number > 0)
                {
                    //if (words != "")
                    //    words += "And ";
                    var unitsMap = new[]
                    {
                        "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven",
                        "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen"
                    };
                    var tensMap = new[]
                    {
                        "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety"
                    };
                    if (number < 20)
                        words += unitsMap[number];
                    else
                    {
                        words += tensMap[number / 10];
                        if ((number % 10) > 0) words += " " + unitsMap[number % 10];
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return words;
        }
    }
}
