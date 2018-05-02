namespace PaperRulez.Store.Service.Consumers
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using Messages;
    using Models;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using Repositories;

    public class ProcessingSucessEventConsumer: DefaultBasicConsumer, IProcessingSucessEventConsumer
    {
        private readonly IRabbitMqManager _manager;
        private readonly ILookupStore _store;

        public ProcessingSucessEventConsumer(IRabbitMqManager manager, ILookupStore store)
        {
            _manager = manager;
            _store = store;
        }

        public override void  HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
            IBasicProperties properties, byte[] body)
        {
            if (properties.ContentType != RabbitMqConstants.JsonMimeType)
            {
                throw new ArgumentException($"Can't handle content type of {properties.ContentType}");
            }

            var message = Encoding.UTF8.GetString(body);
            var command = JsonConvert.DeserializeObject<ProcessingSucessEvent>(message);
            Consume(command);
            _manager.SendAck(deliveryTag);
        }

        public void Consume(IProcessingSuccessEvent command)
        {
            _store.Record(command.Client, command.Id, command.Keywords);
            var processEndEvent = new ProcessEndEvent
            {
                DocumentName = command.DocumentName,
                Client = command.Client
            };

            _manager.SendProcessEndEvent(processEndEvent);
        }
    }
}