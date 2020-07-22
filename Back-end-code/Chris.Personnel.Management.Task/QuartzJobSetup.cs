using Chris.Personnel.Management.Work.Jobs;
using Chris.Personnel.Management.Work.Quartz;
using Microsoft.Extensions.DependencyInjection;

namespace Chris.Personnel.Management.Work
{
    public static class QuartzJobSetup
    {
        public static void AddQuartzJobSetup(this IServiceCollection services)
        {
            services.AddSingleton<HelloWorldJob>();
            services.AddSingleton(
                new JobSchedule(jobType: typeof(HelloWorldJob), cronExpression: "0/5 * * * * ?")
            );
        }
    }
}
