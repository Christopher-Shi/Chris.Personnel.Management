using AutoMapper;
using Chris.Personnel.Management.Common.Extensions;
using Chris.Personnel.Management.Entity;
using Chris.Personnel.Management.ViewModel;
using Chris.Personnel.Management.ViewModel.DropDownListItems;

namespace Chris.Personnel.Management.QueryService.AutoMapper
{
    public class RoleViewModelAutoMapper : Profile
    {
        public RoleViewModelAutoMapper()
        {
            CreateMap<Role, RoleViewModel>()
                .ForMember(dest => dest.Id,
                    opt =>
                        opt.MapFrom(src => src.Id.ToUpperString()));

            CreateMap<Role, RolePageViewModel>()
                .ForMember(dest => dest.Id,
                    opt =>
                        opt.MapFrom(src => src.Id.ToUpperString()));

            CreateMap<Role, RoleDropDownListViewModel>()
                .ForMember(dest => dest.Key,
                    opt =>
                        opt.MapFrom(src => src.Id.ToUpperString()))
                .ForMember(dest => dest.Value,
                    opt =>
                        opt.MapFrom(src => src.Name));
        }
    }
}
