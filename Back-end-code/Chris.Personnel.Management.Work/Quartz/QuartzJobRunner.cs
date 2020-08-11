using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Chris.Personnel.Management.Work.Quartz
{
    /// <summary>
    /// QuartzJobRunner处理正在执行的IJob的整个生命周期：它从容器中获取，执行并释放它（在释放范围时）
    /// </summary>
    public class QuartzJobRunner : IJob
    {
        private readonly IServiceProvider _serviceProvider;
        public QuartzJobRunner(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceProvider.CreateScope();
            var jobType = context.JobDetail.JobType;
            if (scope.ServiceProvider.GetRequiredService(jobType) is IJob job)
            {
                await job.Execute(context);
            }
        }
    }
}
