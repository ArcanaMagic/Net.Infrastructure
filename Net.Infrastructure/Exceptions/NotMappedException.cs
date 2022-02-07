using System;

namespace Net.Infrastructure.Exceptions
{
    public class NotMappedException : Exception
    {
        private static string _baseMessage = "Not mapped";
        /// <summary>
        /// Prefix: Not mapped.
        /// </summary>
        public NotMappedException() : base(_baseMessage)
        { }
        /// <summary>
        /// Prefix: Not mapped.
        /// Sample: Not mapped objectName with id = 1 (where text = 'objectName with id = 1')
        /// </summary>
        /// <param name="text"></param>
        public NotMappedException(string text) : base($"{_baseMessage} {text}")
        { }
        /// <summary>
        /// Prefix: Not mapped.
        /// Sample: Not mapped objectName with id = 1 (where text = 'objectName with id = 1')
        /// </summary>
        /// <param name="text"></param>
        /// <param name="innerException"></param>
        public NotMappedException(string text, Exception innerException) : base($"{_baseMessage} {text}", innerException)
        { }
    }
}
