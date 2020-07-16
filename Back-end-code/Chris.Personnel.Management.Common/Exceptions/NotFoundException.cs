using System;

namespace Chris.Personnel.Management.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {

        }

        public NotFoundException(string message) : base(message)
        {
        }
    }
}
