using Autofac;
using Chris.Personnel.Management.QueryService.Implements;

namespace Chris.Personnel.Management.QueryService
{
    public static class QueryServiceInstaller
    {
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<UserQueryService>().As<IUserQueryService>().InstancePerLifetimeScope();
            builder.RegisterType<RoleQueryService>().As<IRoleQueryService>().InstancePerLifetimeScope();
        }
    }
}
