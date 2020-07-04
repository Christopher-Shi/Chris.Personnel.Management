﻿using Autofac;
using Chris.Personnel.Management.EF.Storage;
using Chris.Personnel.Management.Repository.Implements;
using Chris.Personnel.Management.Repository.UnitOfWork;

namespace Chris.Personnel.Management.Repository
{
    public static class RepositoryInstaller
    {
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType<SqliteDbContextProvider>().As<IDbContextProvider>().InstancePerLifetimeScope();
            builder.RegisterType<SqliteContext>();
            SharedWiring(builder);
        }

        public static void ConfigureContainerForTest(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryDbContextProvider>().As<IDbContextProvider>().InstancePerLifetimeScope();
            builder.RegisterType<InMemoryContext>();
            SharedWiring(builder);
        }

        private static void SharedWiring(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWorkFactory>().As<IUnitOfWorkFactory>().InstancePerLifetimeScope();
            builder.RegisterType<EntityFrameworkUnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
        }
    }
}
