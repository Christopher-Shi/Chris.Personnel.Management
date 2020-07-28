using Autofac;

namespace Chris.Personnel.Management.Common.EntityModel
{
    public static class DependencyResolverInitializer
    {
        public static void Initialize(ILifetimeScope lifetimeScope)
        {
            Dependency.Container = lifetimeScope;
        }
    }
}
