using System;
using System.ComponentModel;

namespace Chris.Personnel.Management.Work.Quartz
{
    public class JobSchedule
    {
        public JobSchedule(Type jobType, string cronExpression)
        {
            JobType = jobType ?? throw new ArgumentNullException(nameof(jobType));
            CronExpression = cronExpression ?? throw new ArgumentNullException(nameof(cronExpression));
        }

        /// <summary>
        /// Job类型
        /// </summary>
        public Type JobType { get; }
        
        /// <summary>
        /// Cron表达式
        /// </summary>
        public string CronExpression { get; }
        
        /// <summary>
        /// Job状态
        /// </summary>
        public JobStatus JobStatus { get; set; } = JobStatus.Init;
    }

    /// <summary>
    /// Job运行状态
    /// </summary>
    public enum JobStatus : byte
    {
        [Description("初始化")]
        Init = 0,

        [Description("运行中")]
        Running = 1,

        [Description("调度中")]
        Scheduling = 2,

        [Description("已停止")]
        Stopped = 3,

    }
}
