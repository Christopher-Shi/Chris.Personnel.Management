using System;

namespace Chris.Personnel.Management.Common.CommonService
{
    public interface ITimeSource
    {
        DateTime GetCurrentTime();
    }
}
