using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using BookBooking.Models;

namespace BookBooking.Attributes
{
    public static class EnumExtensions
    {
        static IDictionary<int, string> GetEnumValueName(Type type)
        {
            var names = type.GetFields(BindingFlags.Public | BindingFlags.Static)
                .Select(f => f.GetCustomAttribute<DisplayAttribute>()?.Name ?? f.Name);

            var values = Enum.GetValues(type).Cast<int>();

            var dictionary = names.Zip(values, (n, v) => new KeyValuePair<int, string>(v, n))
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            return dictionary;
        }

        public static string GetDisplayName(this Enum value)
        {
            var dictionary = GetEnumValueName(value.GetType());
            return dictionary[Convert.ToInt32(value)];
        }
    }
}

