namespace PaperRulez.Finalizer.Service.Consumers
{
    using System;
    using System.Text;
    using Loader.Service;
    using Messages;
    using Models;
    using Newtonsoft.Json;
    using RabbitMQ.Client;

    public class ProcessEndConsumer: DefaultBasicConsumer, IProcessEndConsumer
    {
        private readonly IRabbitMqManager _manager;
        private readonly IBlobStorage _blobStorage;

        public ProcessEndConsumer(IRabbitMqManager manager, IBlobStorage blobStorage)
        {
            _manager = manager;
            _blobStorage = blobStorage;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
            IBasicProperties properties, byte[] body)
        {
            if (properties.ContentType != RabbitMqConstants.JsonMimeType)
            {
                throw new ArgumentException($"Can't handle content type of {properties.ContentType}");
            }

            var message = Encoding.UTF8.GetString(body);
            var command = JsonConvert.DeserializeObject<ProcessEndEvent>(message);
            Consume(command);
            _manager.SendAck(deliveryTag);
        }

        public void Consume(IProcessEndEvent command)
        {
            _blobStorage.RemoveDocumentAsync(command.Client, command.DocumentName);

            var fileRemovedEvent = new FileRemovedEvent { Client = command.Client, DocumentName = command.DocumentName};
            _manager.SendFileRemovedEvent(fileRemovedEvent);
        }
    }
}