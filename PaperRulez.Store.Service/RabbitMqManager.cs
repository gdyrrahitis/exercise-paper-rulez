namespace PaperRulez.Store.Service
{
    using System.Text;
    using Consumers;
    using Messages;
    using Models;
    using Newtonsoft.Json;
    using RabbitMQ.Client;
    using Repositories;

    public class RabbitMqManager: IRabbitMqManager
    {
        private readonly ILookupStore _store;
        private readonly IModel _channel;

        public RabbitMqManager(IConnectionFactory connectionFactory, ILookupStore store)
        {
            _store = store;
            var connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
            connection.AutoClose = true;
        }

        public void ListenForProcessingSucceededEvent()
        {
            _channel.QueueDeclare(queue: RabbitMqConstants.ProcessSuccessQueue,
                durable: false, exclusive: false,
                autoDelete: false, arguments: null);

            _channel.BasicQos(prefetchCount: 1, prefetchSize: 0, global: false);

            var consumer = new ProcessingSucessEventConsumer(this, _store);
            _channel.BasicConsume(queue: RabbitMqConstants.ProcessSuccessQueue,
                autoAck: false,
                consumer: consumer);
        }

        public void SendAck(ulong deliveryTag)
        {
            _channel.BasicAck(deliveryTag: deliveryTag, multiple: false);
        }

        public void SendProcessEndEvent(ProcessEndEvent @event)
        {
            _channel.ExchangeDeclare(exchange: RabbitMqConstants.ProcessEndExchange,
                type: ExchangeType.Fanout);

            _channel.QueueDeclare(queue: RabbitMqConstants.ProcessEndQueue, durable: false, exclusive: false,
                autoDelete: false, arguments: null);
            _channel.QueueBind(queue: RabbitMqConstants.ProcessEndQueue, exchange: RabbitMqConstants.ProcessEndExchange, routingKey: "");

            var serializedCommand = JsonConvert.SerializeObject(@event);

            var properties = _channel.CreateBasicProperties();
            properties.ContentType = RabbitMqConstants.JsonMimeType;

            _channel.BasicPublish(exchange: RabbitMqConstants.ProcessEndExchange, routingKey: "",
                basicProperties: properties,
                body: Encoding.UTF8.GetBytes(serializedCommand));
        }

        public void Dispose()
        {
            if (!_channel.IsClosed)
            {
                _channel.Close();
            }
        }
    }
}