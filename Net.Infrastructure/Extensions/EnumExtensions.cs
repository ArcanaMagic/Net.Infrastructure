using System;
using System.ComponentModel;
using System.Globalization;

// ReSharper disable UnusedMember.Global
namespace Net.Infrastructure.Extensions
{
    public static class EnumExtensions
    {

        /// <summary>
        /// Extension for get description from Enums
        /// </summary>
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (false == (e is Enum))
            {
                return null;
            }
            string description = null;
            var type = e.GetType();
                
            var values = Enum.GetValues(type);

            foreach (int val in values)
            {
                if (val != e.ToInt32(CultureInfo.InvariantCulture))
                {
                    continue;
                }
                var memInfo = type.GetMember(type.GetEnumName(val));
                var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (descriptionAttributes.Length > 0)
                {
                    description = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                }

                break;
            }

            return description;
        }
        /// <summary>
        /// Convert to enum from description.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="description"></param>
        /// <returns></returns>
        public static T GetValueFromDescription<T>(this string description)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                {
                    if (attribute.Description == description)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == description)
                        return (T)field.GetValue(null);
                }
            }
            throw new ArgumentException("Not found.", nameof(description));
        }
    }
}
