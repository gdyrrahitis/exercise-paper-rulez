namespace PaperRulez.Web.Api.Consumers
{
    using System;
    using System.Text;
    using Hubs;
    using Microsoft.AspNet.SignalR;
    using Models;
    using Newtonsoft.Json;
    using PaperRulez.Models;
    using RabbitMQ.Client;

    public class FileRemovedEventConsumer: DefaultBasicConsumer, IFileRemovedEventConsumer
    {
        private readonly IRabbitMqManager _manager;

        public FileRemovedEventConsumer(IRabbitMqManager manager)
        {
            _manager = manager;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
            IBasicProperties properties, byte[] body)
        {
            if (properties.ContentType != RabbitMqConstants.JsonMimeType)
            {
                throw new ArgumentException($"Can't handle content type of {properties.ContentType}");
            }

            var message = Encoding.UTF8.GetString(body);
            var command = JsonConvert.DeserializeObject<FileRemovedEvent>(message);
            Consume(command);
            _manager.SendAck(deliveryTag);
        }

        public void Consume(IFileRemovedEvent command)
        {
            GlobalHost.ConnectionManager.GetHubContext<FileHub>().Clients.All
                .sendFileProcessed(command.Client, command.DocumentName);
        }
    }
}