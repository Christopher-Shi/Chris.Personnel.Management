using System;

namespace Chris.Personnel.Management.Common
{
    public class TimeSource : ITimeSource
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}
