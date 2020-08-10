using System;
using System.Threading.Tasks;
using Chris.Personnel.Management.Common;
using Chris.Personnel.Management.Common.Helper;
using Chris.Personnel.Management.QueryService;
using Chris.Personnel.Management.Work.Quartz;
using Microsoft.Extensions.Logging;
using Quartz;
using StackExchange.Profiling.Internal;

namespace Chris.Personnel.Management.Work.Jobs
{
    public class HelloWorldJob : JobBase, IJob
    {
        private readonly ILogger<HelloWorldJob> _logger;
        private readonly IServiceProvider _serviceProvider;

        public HelloWorldJob(
            ILogger<HelloWorldJob> logger,
            IServiceProvider serviceProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public Task Execute(IJobExecutionContext context)
        {
            //_logger.LogInformation($"{result} at {DateTime.Now.ToDateTimeString()}!"); 
            //var service = AutofacExtension.Resolve<IUserQueryService>();
            //_logger.LogInformation($"施晓勇测试：{service.GetAll().ToJson()}");

            var service = AutofacExtension.Resolve<ITimeSource>();
            //_logger.LogInformation($"施晓勇测试：{service.GetCurrentTime()}");
            NLogHelper.Default.Info($"施晓勇测试：{service.GetCurrentTime()}");
            return Task.CompletedTask;
        }
    }
}
