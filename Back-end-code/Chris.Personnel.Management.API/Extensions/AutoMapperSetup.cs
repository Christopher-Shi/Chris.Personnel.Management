using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Chris.Personnel.Management.QueryService.AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Chris.Personnel.Management.API.Extensions
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

            foreach (var typeProfile in allViewModelProfileTypes)
            {
                services.AddAutoMapper(typeProfile);
            }
        }
    }
}
