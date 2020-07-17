namespace Chris.Personnel.Management.Work
{
    public interface ISchedulerCenter
    {
        /// <summary>
        /// 开启任务调度
        /// </summary>
        /// <returns></returns>
        System.Threading.Tasks.Task StartScheduleAsync();

        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        System.Threading.Tasks.Task StopScheduleAsync();
    }
}