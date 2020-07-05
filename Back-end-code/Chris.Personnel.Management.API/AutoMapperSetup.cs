using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using AutoMapper;
using Chris.Personnel.Management.LogicService.AutoMapper;
using Chris.Personnel.Management.QueryService.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Chris.Personnel.Management.API
{
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //Register all the AutoMapper profiles
            var typeAutoMapperProfile = typeof(Profile);
            var allViewModelProfileTypes = new List<Type>();

            allViewModelProfileTypes.AddRange(
                typeof(UserViewModelAutoMapper).Assembly.GetTypes().Where(
                    x => x != typeAutoMapperProfile
                         && typeAutoMapperProfile.IsAssignableFrom(x)));

            var allUICommandProfileTypes = new List<Type>();

            allUICommandProfileTypes.AddRange(typeof(UserUICommandAutoMapper).Assembly.GetTypes().Where(
                x =>
                    x != typeAutoMapperProfile
                    &&
                    typeAutoMapperProfile.IsAssignableFrom(x)
            ));

            services.AddAutoMapper(cfg =>
            {
                foreach (var typeProfile in allViewModelProfileTypes)
                {
                    var profile = Activator.CreateInstance(typeProfile) as AutoMapper.Profile;
                    cfg.AddProfile(profile);
                }
            }, allViewModelProfileTypes);
            services.AddAutoMapper(cfg =>
            {
                foreach (var typeProfile in allUICommandProfileTypes)
                {
                    var profile = Activator.CreateInstance(typeProfile) as AutoMapper.Profile;
                    cfg.AddProfile(profile);
                }
            }, allUICommandProfileTypes);
        }
    }
}
