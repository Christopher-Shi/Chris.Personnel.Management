using System;
using System.Threading.Tasks;
using Chris.Personnel.Management.Common.Extensions;
using Chris.Personnel.Management.Work.Quartz;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;

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
            using (var scope = _serviceProvider.CreateScope())
            {
                var result = ExecuteJob(context, () =>
                  {
                      //var service = scope.ServiceProvider.GetService<IScopedService>();
                      Console.WriteLine();

                      return Task.CompletedTask;
                  });

                _logger.LogInformation($"{result} at {DateTime.Now.ToDateTimeString()}!");
            }
            return Task.CompletedTask;
        }
    }
}
