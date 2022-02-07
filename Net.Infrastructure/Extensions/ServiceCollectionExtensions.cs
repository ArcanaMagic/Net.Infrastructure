using System.Linq;
using System.Reflection;
using System.Text;
using Net.Infrastructure.BaseTypes.Models;

namespace Net.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {

        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance |
                                                  System.Reflection.BindingFlags.NonPublic |
                                                  System.Reflection.BindingFlags.Public;
        /// <summary>
        /// Преобразует поля реквеста в строковую форму для отправки в url 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string ToStringParams<T>(this T request) where T : IRequest
        {
            var fields = typeof(T).GetFields(BindingFlags).ToList();
            var parentFields = typeof(T).BaseType?.GetFields(BindingFlags);

            if (parentFields != null)
                fields.AddRange(parentFields);

            var sb = new StringBuilder();

            for (var i = 0; i < fields.Count; i++)
            {
                sb.Append(i == 0
                    ? $"?{fields[i].Name.FindTextBetween("<",">")}={fields[i].GetValue(request)}"
                    : $"&{fields[i].Name.FindTextBetween("<",">")}={fields[i].GetValue(request)}");
            }

            return sb.ToString();
        }
    }
}
