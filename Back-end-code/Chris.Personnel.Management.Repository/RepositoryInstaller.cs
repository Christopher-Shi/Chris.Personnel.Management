using Autofac;
using Chris.Personnel.Management.EF.Storage;
using Chris.Personnel.Management.Repository.Implements;

namespace Chris.Personnel.Management.Repository
{
    public static class RepositoryInstaller
    {
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<SqliteDbContextProvider>().As<IDbContextProvider>().InstancePerLifetimeScope();
            //builder.RegisterType<SqliteDbContextProvider>().As<IDbContextProvider>().InstancePerLifetimeScope();
            builder.RegisterType<SqliteContext>();
            SharedWiring(builder);
        }

        public static void ConfigureContainerForTest(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryDbContextProvider>().As<IDbContextProvider>().InstancePerLifetimeScope();

            SharedWiring(builder);
        }

        private static void SharedWiring(ContainerBuilder builder)
        {
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
        }
    }
}
