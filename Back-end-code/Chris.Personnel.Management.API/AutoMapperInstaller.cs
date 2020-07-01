using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Chris.Personnel.Management.QueryService.AutoMapper;

namespace Chris.Personnel.Management.API
{
    public class AutoMapperInstaller
    {
        public static void ConfigureContainer(ContainerBuilder builder)
        {
            //services.AddSingleton<IMap, Mapper>();
            //Register all the AutoMapper profiles
            var typeAutoMapperProfile = typeof(AutoMapper.Profile);
            var allViewModelProfileTypes = new List<Type>();

            allViewModelProfileTypes.AddRange(
                typeof(UserViewModelAutoMapper).Assembly.GetTypes().Where(
                    x => x != typeAutoMapperProfile
                         && typeAutoMapperProfile.IsAssignableFrom(x)));
            //var allDomainServiceProfileTypes = new List<Type>();

            //allDomainServiceProfileTypes.AddRange(typeof(AreaAutoMapper).Assembly.GetTypes().Where(
            //    x =>
            //        x != typeAutoMapperProfile
            //        &&
            //        typeAutoMapperProfile.IsAssignableFrom(x)
            //));


            //AutoMapper.Mapper.Initialize(cfg =>
            //{
            //    foreach (var typeProfile in allViewModelProfileTypes)
            //    {
            //        var profile = Activator.CreateInstance(typeProfile) as AutoMapper.Profile;
            //        cfg.AddProfile(profile);
            //    }

            //    foreach (var typeProfile in allDomainServiceProfileTypes)
            //    {
            //        var profile = Activator.CreateInstance(typeProfile) as AutoMapper.Profile;
            //        cfg.AddProfile(profile);
            //    }
            //});
        }
    }
}
