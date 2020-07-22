using System;
using System.Threading.Tasks;
using Chris.Personnel.Management.Common.Extensions;
using Chris.Personnel.Management.LogicService;
using Chris.Personnel.Management.UICommand;
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
        private readonly IUserLogicService _userLogicService;

        public HelloWorldJob(
            ILogger<HelloWorldJob> logger,
            IServiceProvider serviceProvider,
            IUserLogicService userLogicService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _userLogicService = userLogicService ?? throw new ArgumentNullException(nameof(userLogicService));
        }

        public Task Execute(IJobExecutionContext context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var result = ExecuteJob(context, () =>
                  {
                      _userLogicService.StopUsing(new UserDeleteUICommand
                      {
                          Id = new Guid("32EC1E37-FE6D-4606-902E-6705BEB0AFC0")
                      });
                      return Task.CompletedTask;
                  });

                _logger.LogInformation($"{result} at {DateTime.Now.ToDateTimeString()}!");
            }
            return Task.CompletedTask;
        }
    }
}
