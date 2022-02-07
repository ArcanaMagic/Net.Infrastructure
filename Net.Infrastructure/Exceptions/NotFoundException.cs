using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Net.Infrastructure.Exceptions
{
    public class NotFoundException : DbUpdateException
    {
        private static string _baseMessage;

        public NotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Not found {entityTypeName} with {Id} = {1}. Where pairs is Key = Id, Value = 1
        /// </summary>
        /// <param name="entityTypeName"></param>
        /// <param name="pairs"></param>
        public NotFoundException(string entityTypeName, params KeyValuePair<string, string>[] pairs) : base(_baseMessage)
        {
            var sb = new StringBuilder();
            sb.Append($"Not found {entityTypeName} with ");

            for (var i = 0; i < pairs.Length ; i++)
            {
                sb.Append($"{pairs[i].Key} = '{pairs[i].Value}'");

                if (pairs.Length != i + 1)
                    sb.Append($", ");
            }

            _baseMessage = sb.ToString();
        }
    }
}
