using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Chris.Personnel.Management.Common.AOP;
using Chris.Personnel.Management.Common.Exceptions;
using Chris.Personnel.Management.Common.Extensions;
using Chris.Personnel.Management.Common.Helper;

namespace Chris.Personnel.Management.QueryService
{
    public static class QueryServiceInstaller
    {
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;
            var servicesDllFile = Path.Combine(basePath, "Chris.Personnel.Management.QueryService.dll");

            if (!File.Exists(servicesDllFile))
            {
                var msg = "Chris.Personnel.Management.QueryService.dll 丢失";
                NLogHelper.Default.Error(msg);
                throw new NotFoundException(msg);
            }

            var aopType = new List<Type>();
            if (AppSettings.Apply("AOP", "LogAOP", "Enabled").ToBool())
            {
                builder.RegisterType<LogAOP>();
                aopType.Add(typeof(LogAOP));
            }
            if (AppSettings.Apply("AOP", "RedisCachingAOP", "Enabled").ToBool())
            {
                builder.RegisterType<RedisCacheAOP>();
                aopType.Add(typeof(RedisCacheAOP));
            }
            if (AppSettings.Apply("AOP", "MemoryCachingAOP", "Enabled").ToBool())
            {
                builder.RegisterType<MemoryCacheAOP>();
                aopType.Add(typeof(MemoryCacheAOP));
            }

            var assemblyServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblyServices)
                .AsImplementedInterfaces()
                .InstancePerDependency()
                .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                .InterceptedBy(aopType.ToArray());//允许将拦截器服务的列表分配给注册。
        }
    }
}
