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
                new JobSchedule(typeof(HelloWorldJob), "0/5 * * * * ?")
            );
        }
    }
}
