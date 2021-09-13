using System;
using System.Text.Json;
using AutoMapper;
using CommandService.Data;
using CommandService.DTOs;
using CommandService.Models;
using Microsoft.Extensions.DependencyInjection;
namespace CommandService.EventProcess
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory factory, IMapper mapper)
        {
            _scopeFactory = factory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch(eventType)
            {
                case EventType.PlatformPublished:
                    addPlatform(message);
                    break;
                default:
                    Console.WriteLine($"{eventType.ToString()}");
                    break;
                    
            }
        }

        private EventType DetermineEvent(string message)
        {
            Console.WriteLine($"{message}");
            var genericEvent = JsonSerializer.Deserialize<GenericEvent>(message);
            switch(genericEvent.Event)
            {
                case "Platform Published":
                    Console.WriteLine($"Platform published event popped");
                    return EventType.PlatformPublished;
                default:
                    return EventType.NonSpecified;
            }
        }
        private void addPlatform(string message)
        {
            using(var scope=_scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPublished = JsonSerializer.Deserialize<PlatformPublish>(message);
                try
                {
                    var platform = _mapper.Map<Platform>(platformPublished);
                    if(!repo.ExternalPlatformExists(platform.ExternalId))
                    {
                        repo.CreatePlatform(platform);
                        repo.SaveChanges();
                    }
                    else
                    {
                        Console.WriteLine($"Platform already exists");
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex}");
                    
                }
            }
        }

        private int ICommandRepo()
        {
            throw new NotImplementedException();
        }
    }
    enum EventType
    {
        PlatformPublished,
        NonSpecified
    }
}