using AutoMapper;
using System.Runtime;
using winetranet_api.DTO;
using winetranet_api.Entities;

namespace winetranet_api
{
    public class AutoMapperProfile : Profile
    {
        public  AutoMapperProfile(){
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<Service, ServiceDTO>();
            CreateMap<ServiceDTO, Service>();

            CreateMap<Site, SiteDTO>();
            CreateMap<SiteDTO, Site>();
        }
    }
}
