using Autofac;

namespace Chris.Personnel.Management.LogicService
{
    public static class LogicServiceInstaller
    {
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<UserLogicService>().As<IUserLogicService>().InstancePerLifetimeScope();
        }
    }
}
