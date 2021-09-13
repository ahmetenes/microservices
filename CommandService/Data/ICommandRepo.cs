using System.Collections.Generic;
using CommandService.Models;

namespace CommandService.Data
{
    public interface ICommandRepo
    {
        

        
        bool SaveChanges();

        IEnumerable<Platform> GetAllPlatforms();

        void CreatePlatform(Platform platform);

        bool PlatformExists(int platformId);

        bool ExternalPlatformExists(int externalId);
        
        IEnumerable<Command> GetCommandsByPlatform(int platformId);

        Command GetCommandById(int platformId,int commandId);

        void CreateCommand(int platformId,Command command);

    }
}