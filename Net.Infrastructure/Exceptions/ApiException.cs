using System;

namespace Net.Infrastructure.Exceptions
{
    public class ApiException : Exception
    {
        public ApiException()
        { }
        public ApiException(string text) : base(text)
        { }
        public ApiException(string text, Exception innerException) : base(text, innerException)
        { }
    }
}
