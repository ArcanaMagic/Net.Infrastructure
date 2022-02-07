using System;

namespace Net.Infrastructure.Exceptions
{
    public class LockedException : Exception
    {
        public LockedException() : base()
        {
        }
        public LockedException(string message) : base(message)
        {
        }
    }
}
