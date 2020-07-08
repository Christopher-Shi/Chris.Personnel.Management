using System;

namespace Chris.Personnel.Management.Common
{
    public class LogicException : Exception
    {
        public LogicException(string message) : base(message)
        {
        }
    }
}
