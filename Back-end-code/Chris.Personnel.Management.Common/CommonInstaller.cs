using Autofac;
using Microsoft.AspNetCore.Http;

namespace Chris.Personnel.Management.Common
{
    public static class CommonInstaller
    {
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<TimeSource>().As<ITimeSource>().SingleInstance();
            builder.RegisterType<UserAuthenticationManager>().As<IUserAuthenticationManager>().InstancePerLifetimeScope();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
        }
    }
}
