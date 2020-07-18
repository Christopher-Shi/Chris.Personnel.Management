using System;
using Chris.Personnel.Management.Common.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Chris.Personnel.Management.Work.Quartz
{
    /// <summary>
    /// 用于创建作业实例
    /// </summary>
    public class SingletonJobFactory : IJobFactory
    {
        //提供一个自定义的IJobFactory挂钩到ASP.NET Core依赖项注入容器
        private readonly IServiceProvider _serviceProvider;

        public SingletonJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                var serviceScope = _serviceProvider.CreateScope();
                var job = serviceScope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
                return job;

            }
            catch (Exception exception)
            {
                throw new TaskException("创建作业失败", exception.InnerException);
            }
        }

        /// <summary>
        /// 该ReturnJob方法是调度程序尝试返回（即销毁）工厂创建的作业的地方
        /// 不幸的是，使用内置的IServiceProvider没有这样做的机制
        /// 我们无法创建适合Quartz API所需的新的IScopeService，因此我们只能创建单例作业
        /// 这个很重要: 使用上述实现，仅对创建单例（或瞬态）的IJob实现是安全的
        /// </summary>
        /// <param name="job"></param>
        public void ReturnJob(IJob job)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}