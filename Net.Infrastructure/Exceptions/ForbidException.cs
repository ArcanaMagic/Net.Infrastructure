using System;

namespace Net.Infrastructure.Exceptions
{
    public class ForbidException : Exception
    {
        public ForbidException() : base()
        {
        }
        public ForbidException(string message) : base(message)
        {
        }
    }
}
