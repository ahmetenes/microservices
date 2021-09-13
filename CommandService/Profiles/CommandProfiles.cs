using AutoMapper;
using CommandService.DTOs;
using CommandService.Models;

namespace CommandService.Profiles
{
    public class CommandProfiles: Profile
    {
        public CommandProfiles()
        {
            CreateMap<Platform,PlatformRead>();
            CreateMap<Command,CommandRead>();
            CreateMap<CommandWrite,Command>();
            CreateMap<PlatformPublish,Platform>()
                .ForMember(destinationMember=>destinationMember.ExternalId,opt=>opt.MapFrom(src=>src.Id));
        }
    }
}