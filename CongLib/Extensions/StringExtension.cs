using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CongLib.Extensions
{
    public static class StringExtensions
    {
        public static string FormatStringPerAmountChar(this string input, int perAmountOfChar, string value)
        {
            string result = input;
            int inputLength = input.Length;
            int maxLength = inputLength / 2 + inputLength;

            //insert a string for every amount of characters, example 55-55-55-55

            int incrNum = perAmountOfChar + value.Length;
            for (int i = perAmountOfChar; i < maxLength; i += incrNum)
            {
                result = result.Insert(i, value);
            }

            char[] tempCharArray = value.ToCharArray();

            return result.TrimEnd(tempCharArray);
        }

        #region IsSqlDateTime
        public static bool IsSqlDateTime(this string inputStr)
        {
            bool result = false;
            DateTime tempResult;
            bool success = DateTime.TryParseExact(inputStr, "MMddyyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out tempResult);

            //if parse success, then check against max and min

            if (success)
            {
                DateTime sqlMinDateTime = (DateTime)SqlDateTime.MinValue;
                DateTime sqlMaxDateTime = (DateTime)SqlDateTime.MaxValue;
                if (tempResult >= sqlMinDateTime && tempResult <= sqlMaxDateTime)
                {
                    result = true;
                }
            }
            return result;
        }
        #endregion
    }
}
