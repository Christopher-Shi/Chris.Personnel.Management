using System;

namespace Chris.Personnel.Management.Common.CommonService
{
    public class TimeSource : ITimeSource
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}
