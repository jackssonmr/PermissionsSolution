using AutoMapper;
using Permissions.API.DTOs;
using Permissions.Infrastructure.Models;

namespace Permissions.API.Mappings
{
    public class PermissionMappingProfile : Profile
    {
        public PermissionMappingProfile()
        {
            CreateMap<Permission, PermissionDTO>()
                .ForMember(dest => dest.EmployeeForename,
                    opt => opt.MapFrom(src => src.EmployeeForename))
                .ForMember(dest => dest.EmployeeSurname,
                    opt => opt.MapFrom(src => src.EmployeeForename));
            
            CreateMap<PermissionDTO, Permission>()
                .ForMember(dest => dest.EmployeeForename,
                    opt => opt.MapFrom(src => src.EmployeeForename))
                .ForMember(dest => dest.EmployeeSurname,
                    opt => opt.MapFrom(src => src.EmployeeForename));
        }
    }
}
