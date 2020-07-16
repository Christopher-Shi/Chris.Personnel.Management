using System;

namespace Chris.Personnel.Management.Common.Exceptions
{
    public class LogicException : Exception
    {
        public LogicException()
        {

        }

        public LogicException(string message) : base(message)
        {
        }
    }
}
