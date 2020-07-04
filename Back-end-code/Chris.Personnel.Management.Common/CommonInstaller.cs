using Autofac;

namespace Chris.Personnel.Management.Common
{
    public static class CommonInstaller
    {
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<TimeSource>().As<ITimeSource>().SingleInstance();
        }
    }
}
