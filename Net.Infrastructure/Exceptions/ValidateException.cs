using System;

namespace Net.Infrastructure.Exceptions
{
    /// <summary>
    /// Исключение для ошибок валидации, где message это список этих ошибок с разделителем строк '^'
    /// </summary>
    public class ValidateException : Exception
    {
        public ValidateException(string message) : base(message)
        {
        }
    }
}
