using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace CommandService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;

        }
        [HttpGet]
        public ActionResult<IEnumerable<PlatformRead>> GetAllPlatforms(string parameter)
        {
            var platformItems = _repo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformRead>>(platformItems));
        }
        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # Command Service");

            return Ok("Inbound test of from Platforms Controler");
        }
        
    }
}