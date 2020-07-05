﻿using AutoMapper;
using Chris.Personnel.Management.Common;
using Chris.Personnel.Management.Entity;
using Chris.Personnel.Management.ViewModel;

namespace Chris.Personnel.Management.QueryService.AutoMapper
{
    public class UserViewModelAutoMapper : Profile
    {
        public UserViewModelAutoMapper()
        {
            CreateMap<User, UserViewModel>()
                .ForMember(dest => dest.Id,
                    opt =>
                        opt.MapFrom(src => src.Id.ToUpperString())).ReverseMap();
        }
    }
}