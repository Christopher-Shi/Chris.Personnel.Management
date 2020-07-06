using AutoMapper;
using Chris.Personnel.Management.Common;
using Chris.Personnel.Management.Entity;
using Chris.Personnel.Management.QueryService.Enums;
using Chris.Personnel.Management.ViewModel;

namespace Chris.Personnel.Management.QueryService.AutoMapper
{
    public class UserViewModelAutoMapper : Profile
    {
        public UserViewModelAutoMapper()
        {
            CreateMap<User, UserFormViewModel>()
                .ForMember(dest => dest.Id,
                    opt =>
                        opt.MapFrom(src => src.Id.ToUpperString())).ReverseMap();

            CreateMap<User, UserListViewModel>()
                .ForMember(dest => dest.Id,
                    opt =>
                        opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Gender,
                    opt =>
                        opt.MapFrom(src => src.IsEnabled.GetEnabledDescription()))
                .ForMember(dest => dest.LastModifiedTime,
                    opt =>
                        opt.MapFrom(src => src.LastModifiedTime.ToDateTimeString()));
        }
    }
}
