using AutoMapper;
using Chris.Personnel.Management.Common.Extensions;
using Chris.Personnel.Management.Entity;
using Chris.Personnel.Management.ViewModel;
using Chris.Personnel.Management.ViewModel.DropDownListItems;

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

            CreateMap<User, UserPageViewModel>()
                .ForMember(dest => dest.Id,
                    opt =>
                        opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Gender,
                    opt =>
                        opt.MapFrom(src => src.IsEnabled.GetDescription()))
                .ForMember(dest => dest.LastModifiedTime,
                    opt =>
                        opt.MapFrom(src => src.LastModifiedTime.ToDateTimeString()));

            CreateMap<User, UserDropDownListViewModel>()
                .ForMember(dest => dest.Key,
                    opt =>
                        opt.MapFrom(src => src.Id.ToUpperString()))
                .ForMember(dest => dest.Value,
                    opt =>
                        opt.MapFrom(src => src.Name));
        }
    }
}
