using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;
using PlatformService.AsyncDataService;

namespace PlatformService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {

        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        private readonly ICommandDataClient _client;
        private readonly IMessageBusClient _messageBusClient;

        public PlatformsController(IPlatformRepo repo, IMapper mapper,ICommandDataClient client,IMessageBusClient messageBusClient)
        {
            _repo = repo;
            _mapper = mapper;
            _client = client;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformRead>> GetPlatforms()
        {
            Console.WriteLine("Getting Platforms");
            var platformItem = _repo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformRead>>(platformItem));
        }
        [HttpGet("{id}",Name="GetPlatformById")]
         public ActionResult<PlatformRead> GetPlatformById(int id)
        {
            Console.WriteLine("Getting platform with specific id");
            var platformItem = _repo.GetPlatformById(id);
            if (platformItem!=null)
            {
                return Ok(_mapper.Map<PlatformRead>(platformItem));
            }else{
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<ActionResult<PlatformRead>> CreatePlatform(PlatformCreate pc)
        {
            var platformModel = _mapper.Map<Platform>(pc);
            _repo.CreatePlatform(platformModel);
            _repo.SaveChanges();
            var returnModel = _mapper.Map<PlatformRead>(platformModel);
            try
            {
                await _client.SendPlatformToCommand(returnModel);
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"Could not send asyncronously:{ex.ToString()}");
            }
            try
            {

                var platformToPublish = _mapper.Map<PlatformPublish>(returnModel);
                platformToPublish.Event = "Platform Published";
                _messageBusClient.PublishNewPlatform(platformToPublish);

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Send asyncronously : {ex.ToString()}");                

            }
            return CreatedAtRoute(nameof(GetPlatformById),new { Id=returnModel.Id },returnModel); 
        }
    }
}