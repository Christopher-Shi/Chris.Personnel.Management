using System;

namespace Chris.Personnel.Management.Common
{
    public class LogicServiceExceptiony : Exception
    {
        public LogicServiceExceptiony(string message) : base(message)
        {
        }
    }
}
