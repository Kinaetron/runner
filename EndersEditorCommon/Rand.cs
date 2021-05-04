using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EndersEditorCommon
{
    /// <summary>
    /// Helper class for supplying random variables
    /// </summary>
    public class Rand
    {
        /// <summary>
        /// Get a random string
        /// </summary>
        /// <param name="size">Size of string</param>
        /// <param name="lowerCase">Lowercase only?</param>
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();

            char ch;

            for (int i = 0; i < size; i++)
            {
                ch = (Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65))));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();

            return builder.ToString();
        }
    }
}
