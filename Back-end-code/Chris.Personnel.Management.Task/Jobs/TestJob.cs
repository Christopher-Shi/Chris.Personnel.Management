using System;
using Quartz;

namespace Chris.Personnel.Management.Work.Jobs
{
    public class TestJob : JobBase, IJob
    {
        private readonly ISchedulerCenter _schedulerCenter;

        public TestJob(ISchedulerCenter schedulerCenter)
        {
            _schedulerCenter = schedulerCenter;
        }

        public System.Threading.Tasks.Task Execute(IJobExecutionContext context)
        {

        }
    }
}
