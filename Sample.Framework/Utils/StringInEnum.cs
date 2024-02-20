using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Framework.Utils
{
    public static class StringInEnum
    {
        public static bool Exists<TEnum>(string input) where TEnum : struct, Enum
        {
            if (Enum.TryParse<TEnum>(input, out _))
            {
                return true; // Match found
            }
            else
            {
                return false; // No match found
            }
        }
    }
}
