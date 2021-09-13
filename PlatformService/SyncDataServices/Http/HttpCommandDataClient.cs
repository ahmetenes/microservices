using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PlatformService.DTOs;

namespace PlatformService.SyncDataServices.Http
{
    class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient client,IConfiguration configuration)
        {
           
            _client=client;
            _configuration=configuration;
        }
        public async Task SendPlatformToCommand(PlatformRead pr)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(pr),
                Encoding.UTF8,
                "application/json");
            Console.WriteLine($"{_configuration["CommandService"]}");
            
            
            var response = await _client.PostAsync($"{_configuration["CommandService"]}",httpContent);
            Console.WriteLine($"{httpContent.ToString()}");
            
            if (response.IsSuccessStatusCode){
                Console.WriteLine("Sync Post to CommandService OK");
            }
            else
            {
             Console.WriteLine($"Sync Post to CommandService FAILED: {response}");       
            }
            
        }
    }
}