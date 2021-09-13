using System.Collections.Generic;
using System.Linq;
using CommandService.Models;

namespace CommandService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;

        public CommandRepo(AppDbContext context)
        {
            _context = context;
        }
        public void CreateCommand(int platformId, Command command)
        {
            if(command!=null){
                command.PlatformId=platformId;
                _context.Commands.Add(command);
                
            }else{
                throw new System.ArgumentNullException();
            }
        }

        public void CreatePlatform(Platform platform)
        {
            if(platform!=null)
            {

             _context.Platforms.Add(platform);
            }
            else
            {
                throw new System.ArgumentNullException();
            }
        }

        public bool ExternalPlatformExists(int externalId)
        {
            return _context.Platforms.Any(p=>p.ExternalId==externalId);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList<Platform>();
        }

        public Command GetCommandById(int platformId, int commandId)
        {
            return _context.Commands.Where<Command>(c=>c.PlatformId==platformId&&c.Id==commandId).FirstOrDefault<Command>();
        }

        public IEnumerable<Command> GetCommandsByPlatform(int platformId)
        {
            return _context.Commands.Where<Command>(c=>c.PlatformId==platformId);
        }

        public bool PlatformExists(int platformId)
        {
            return _context.Platforms.Any(p=>p.Id==platformId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges()>=0);
        }
    }
}