using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandService.EventProcess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandService.AsyncDataServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly IEventProcessor _processor;
        private IConnection _conn;
        private IModel _channel;
        private string _queueName;

        public MessageBusSubscriber(IConfiguration config, IEventProcessor eventProcessor)
        {
            _config = config;
            _processor = eventProcessor;
            InitializeRabbitMq();
        }

        private void InitializeRabbitMq()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config["RabbitMqHost"],
                Port = int.Parse(_config["RabbitMqPort"])
            };
            _conn = factory.CreateConnection();
            _channel = _conn.CreateModel();
            _channel.ExchangeDeclare
            (
                exchange: "trigger",
                type: ExchangeType.Fanout
            );
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind
            (
                queue: _queueName,
                exchange: "trigger",
                routingKey: ""
            );
            
            Console.WriteLine($"Listening the Message Bus");
            _conn.ConnectionShutdown += RabbitMQ_ConncetionShutdown;
        }

        private void RabbitMQ_ConncetionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine($"Connection shutdown");
            
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _conn.Close();
            }
            Console.WriteLine($"Connection Disposed");
            base.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var _consumer = new EventingBasicConsumer(_channel);

            _consumer.Received += (ModuleHandle,args)=>{
                Console.WriteLine($"Event received");
                var body=args.Body;
                var notifMessage = Encoding.UTF8.GetString(body.ToArray());
                _processor.ProcessEvent(notifMessage);
            };
            _channel.BasicConsume
            (
                queue:_queueName,
                autoAck:true,
                consumer:_consumer
            );
            return Task.CompletedTask;
        }
    }
}