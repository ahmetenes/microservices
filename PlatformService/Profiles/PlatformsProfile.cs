using PlatformService.DTOs;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatformsProfile : AutoMapper.Profile
    {
        public PlatformsProfile()
        {
            CreateMap<Platform,PlatformRead>();
            CreateMap<PlatformRead,PlatformPublish>();
            CreateMap<PlatformCreate,Platform>();
        }
    }
}