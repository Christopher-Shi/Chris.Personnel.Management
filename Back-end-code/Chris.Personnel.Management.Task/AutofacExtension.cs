using Autofac;

namespace Chris.Personnel.Management.Work
{
    internal static class AutofacExtension
    {
        private static IContainer _container;

        public static void InitialAutofac()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new AutofacModuleRegister());

            _container = builder.Build();
        }

        /// <summary>
        /// 从容器中获取对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
