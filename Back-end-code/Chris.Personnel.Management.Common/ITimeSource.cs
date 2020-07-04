using System;

namespace Chris.Personnel.Management.Common
{
    public interface ITimeSource
    {
        DateTime GetCurrentTime();
    }
}
