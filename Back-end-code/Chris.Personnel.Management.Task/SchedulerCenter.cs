using System;
using System.Threading.Tasks;
using Chris.Personnel.Management.Common.Exceptions;
using Quartz;

namespace Chris.Personnel.Management.Work
{
    public class SchedulerCenter : ISchedulerCenter
    {
        private readonly ISchedulerFactory _schedulerFactory;
        private Task<IScheduler> _scheduler;

        public SchedulerCenter(ISchedulerFactory schedulerFactory)
        {
            _schedulerFactory = schedulerFactory;
            _scheduler = GetSchedulerAsync();
        }
        private Task<IScheduler> GetSchedulerAsync()
        {
            if (_scheduler != null)
            {
                return _scheduler;
            }

            return _scheduler = _schedulerFactory.GetScheduler();
        }

        public async System.Threading.Tasks.Task StartScheduleAsync()
        {
            try
            {
                if (!_scheduler.Result.IsStarted)
                {
                    await _scheduler.Result.Start();
                    await Console.Out.WriteLineAsync("任务调度开启");

                    //return await _scheduler;
                }
                //return null;
            }
            catch (Exception exception)
            {

                throw new TaskException("任务调度开启失败", exception.InnerException);
            }
        }

        public async System.Threading.Tasks.Task StopScheduleAsync()
        {
            try
            {
                if (!_scheduler.Result.IsShutdown)
                {
                    await _scheduler.Result.Shutdown();
                    await Console.Out.WriteLineAsync("任务调度停止");
                }
                else
                {
                    await Console.Out.WriteLineAsync("任务调度已经停止");
                }
            }
            catch (Exception exception)
            {
                throw new TaskException("任务调度停止失败", exception.InnerException);
            }
        }
    }
}
