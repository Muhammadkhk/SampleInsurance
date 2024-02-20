using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Framework.Utils
{
    public static class EnumMapper
    {
        public static T MapStringToEnum<T>(string stringValue) where T : Enum
        {
            if (string.IsNullOrEmpty(stringValue))
            {
                // Assuming your enum has a 'None' value.
                return (T)Enum.Parse(typeof(T), "None");
            }
            else if (Enum.TryParse(typeof(T), stringValue, true, out var enumValue))
            {
                return (T)enumValue;
            }
            else
            {
                // Handle invalid values as needed
                throw new ArgumentException($"Invalid string value for {typeof(T).Name}");
            }
        }

        public static string GetEnumDisplayName(this Enum enumType)
        {
            return enumType.GetType().GetMember(enumType.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                .Name;
        }
    }
}
