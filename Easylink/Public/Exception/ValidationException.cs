using System;

namespace Easylink
{
    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, params object[] args)
            : base(string.Format(message, args))
        {
        }
    }
}