namespace PaperRulez.Processing.Service.Consumers
{
    using System;
    using System.Text;
    using Messages;
    using Models;
    using Newtonsoft.Json;
    using Processors;
    using RabbitMQ.Client;

    public class FileLoadRequestCommandConsumer: DefaultBasicConsumer, IFileLoadRequestCommandConsumer
    {
        private readonly IRabbitMqManager _manager;
        private readonly IHandler _handler;

        public FileLoadRequestCommandConsumer(IRabbitMqManager manager, IHandler handler)
        {
            _manager = manager;
            _handler = handler;
        }

        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
            IBasicProperties properties, byte[] body)
        {
            if (properties.ContentType != RabbitMqConstants.JsonMimeType)
            {
                throw new ArgumentException($"Can't handle content type of {properties.ContentType}");
            }

            var message = Encoding.UTF8.GetString(body);
            var command = JsonConvert.DeserializeObject<FileLoadRequestCommand>(message);
            Consume(command);
            _manager.SendAck(deliveryTag);
        }

        public void Consume(IFileLoadRequestCommand command)
        {
            var keywords = _handler.HandleDocumentProcessing(command.Content);
            var processingSuccessEvent = new ProcessingSucessEvent
            {
                Client = command.Client,
                Id = command.FileName.Substring(0, command.FileName.IndexOf("_")),
                Keywords = keywords,
                DocumentName = command.FileName
            };
            _manager.SendProcessingSucceededEvent(processingSuccessEvent);
        }
    }
}