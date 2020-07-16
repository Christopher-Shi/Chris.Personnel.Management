using System;

namespace Chris.Personnel.Management.Common.Exceptions
{
    public class LogicServiceException : Exception
    {
        public LogicServiceException()
        {
            
        }

        public LogicServiceException(string message) : base(message)
        {
        }
    }
}
