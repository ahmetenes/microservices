using Microsoft.AspNetCore.Mvc;
using CommandService.Data;
using CommandService.DTOs;
using AutoMapper;
using System.Collections.Generic;
using System;
using CommandService.Models;

namespace CommandService.Controllers
{


    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public CommandsController(ICommandRepo repo,IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CommandRead>> GetCommandsByPlatform(int platformId)
        {   
                Console.WriteLine($"Tries to get commands of platform {platformId}");
            if(_repo.PlatformExists(platformId))
            {   
                var commandItems = _repo.GetCommandsByPlatform(platformId);
                return Ok(_mapper.Map<IEnumerable<CommandRead>>(commandItems));
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet("{commandId}",Name="GetCommandsOfPlatform")]
        public ActionResult<CommandRead> GetCommandsOfPlatform(int platformId,int commandId)
        {   
                Console.WriteLine($"Tries to get command {commandId} of platform {platformId}");
            if(_repo.PlatformExists(platformId))
            {   
                var commandItem = _repo.GetCommandById(platformId,commandId);
                if(commandItem!=null){
                    return Ok(_mapper.Map<CommandRead>(commandItem));
                }
            }
                return NotFound();
            
        }
        [HttpPost]
        public ActionResult<CommandRead> CreateCommand(int platformId,CommandWrite command)
        {
            Console.WriteLine($"Tries to create command of platform {platformId}");
            if(_repo.PlatformExists(platformId))
            {   

                var commandItem = _mapper.Map<Command>(command);
                _repo.CreateCommand(platformId,commandItem);
                _repo.SaveChanges();
                var returnItem = _mapper.Map<CommandRead>(commandItem);
                return CreatedAtRoute( nameof(GetCommandsOfPlatform) ,new {platformId=platformId,commandId=returnItem.Id},returnItem);
            }
            return NotFound();
            
        }
    }
}