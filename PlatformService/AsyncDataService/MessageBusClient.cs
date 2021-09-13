using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PlatformService.DTOs;
using RabbitMQ.Client;

namespace PlatformService.AsyncDataService
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _config;
        private readonly IConnection _conn;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration config)
        {
            _config = config;
            var factory = new ConnectionFactory(){
                HostName = _config["RabbitMqHost"],
                Port = int.Parse(_config["RabbitMqPort"])
            };
            try
            {
                _conn = factory.CreateConnection();
                _channel=_conn.CreateModel();
                _channel.ExchangeDeclare(exchange: "trigger",type:ExchangeType.Fanout);
                _conn.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"RabbitMq connection failed {ex.ToString()}");
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("RabbitMQ_ConnectionShutdown event popped");
            
        }

        public void PublishNewPlatform(PlatformPublish pb)
        {
            var message = JsonSerializer.Serialize(pb);
            if(_conn.IsOpen){
                Console.WriteLine($"RabbitMq is Open ");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine($"RabbitMq is Open ");
            }
        }
        private void SendMessage(string msg)
        {
            var body = Encoding.UTF8.GetBytes(msg);
            _channel.BasicPublish(exchange: "trigger",routingKey:"",basicProperties:null, body:body);
            Console.WriteLine($"We send the {msg}");
            
        }
        
        public void Dispose()
        {
            Console.WriteLine($"Channel Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _conn.Close();
            }
            
        }

    }
}