using AutoMapper;
using Chris.Personnel.Management.Entity;
using Chris.Personnel.Management.UICommand;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chris.Personnel.Management.LogicService.AutoMapper
{
    public class UserUICommandAutoMapper : Profile
    {
        public UserUICommandAutoMapper()
        {
            CreateMap<UserAddUICommand, User>()
                .ForMember(dest => dest.Name,
                    opt =>
                        opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Password,
                    opt =>
                        opt.MapFrom(src => "admin123"))
                .ForMember(dest => dest.TrueName,
                    opt =>
                        opt.MapFrom(src => src.User.TrueName))
                .ForMember(dest => dest.Gender,
                    opt =>
                        opt.MapFrom(src => src.User.Gender))
                .ForMember(dest => dest.CardId,
                    opt =>
                        opt.MapFrom(src => src.User.CardId))
                .ForMember(dest => dest.Phone,
                    opt =>
                        opt.MapFrom(src => src.User.Phone))
                .ForMember(dest => dest.IsEnabled,
                    opt =>
                        opt.MapFrom(src => src.User.IsEnabled));
        }
    }
}
