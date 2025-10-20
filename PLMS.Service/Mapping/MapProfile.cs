using AutoMapper;
using PLMS.Core.DTOs;
using PLMS.Core.Entities;

namespace PLMS.Service.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<AuthIdentityUser, AuthIdentityUserLoginDto>().ReverseMap();
            CreateMap<AuthIdentityUser, AuthIdentityUserRegisterDto>().ReverseMap();
            CreateMap<AuthIdentityUser, AuthIdentityUserDto>().ReverseMap();
            CreateMap<AuthIdentityUser, AuthIdentityUserForAdminDto>().ReverseMap();
            CreateMap<AuthIdentityRole, AuthIdentityRoleDto>().ReverseMap();
        }
    }
}
