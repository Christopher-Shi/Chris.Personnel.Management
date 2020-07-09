using System;

namespace Chris.Personnel.Management.Common
{
    public class LogicServiceException : Exception
    {
        public LogicServiceException(string message) : base(message)
        {
        }
    }
}
