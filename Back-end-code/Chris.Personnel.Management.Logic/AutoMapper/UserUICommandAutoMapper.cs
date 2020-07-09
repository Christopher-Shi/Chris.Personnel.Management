using AutoMapper;
using Chris.Personnel.Management.Entity;
using Chris.Personnel.Management.UICommand;

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
                //.ForMember(dest => dest.Password,
                //    opt =>
                //        opt.Ignore())
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
                        opt.MapFrom(src => src.User.IsEnabled))
                //.ForMember(dest => dest.CreatedUserId,
                //    opt =>
                //        opt.Ignore())
                //.ForMember(dest => dest.CreatedTime,
                //    opt =>
                //        opt.Ignore())
                //.ForMember(dest => dest.LastModifiedUserId,
                //    opt =>
                //        opt.Ignore())
                //.ForMember(dest => dest.LastModifiedTime,
                //    opt =>
                //        opt.Ignore())
                //.ForMember(dest => dest.Id,
                //    opt =>
                //        opt.Ignore())
                ;
        }
    }
}
