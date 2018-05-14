using System;

namespace Easylink
{
    public class EasylinkException : Exception
    {
        public EasylinkException(string message)
            : base(message)
        {
        }

        public EasylinkException(Exception ex, string message)
            : base(message, ex)
        {
        }


        public EasylinkException(string message, params object[] args) : base(string.Format(message, args))
        {
        }

        public EasylinkException(Exception ex, string message, params object[] args)
            : base(string.Format(message, args), ex)
        {
        }
    }
}