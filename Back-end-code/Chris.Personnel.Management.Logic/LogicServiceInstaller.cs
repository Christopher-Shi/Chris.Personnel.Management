using Autofac;
using Chris.Personnel.Management.LogicService.Implements;

namespace Chris.Personnel.Management.LogicService
{
    public static class LogicServiceInstaller
    {
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<UserLogicService>().As<IUserLogicService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleLogicService>().As<IRoleLogicService>().InstancePerLifetimeScope();
        }
    }
}
