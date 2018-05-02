namespace PaperRulez.Finalizer.Service
{
    using System.Text;
    using Consumers;
    using Loader.Service;
    using Models;
    using Newtonsoft.Json;
    using RabbitMQ.Client;

    public class RabbitMqManager: IRabbitMqManager
    {
        private readonly IBlobStorage _blobStorage;
        private readonly IModel _channel;

        public RabbitMqManager(IConnectionFactory connectionFactory, IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
            var connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
            connection.AutoClose = true;
        }

        public void ListenForProcessEndEvent()
        {
            _channel.QueueDeclare(queue: RabbitMqConstants.ProcessEndQueue,
                durable: false, exclusive: false,
                autoDelete: false, arguments: null);

            _channel.BasicQos(prefetchCount: 1, prefetchSize: 0, global: false);

            var consumer = new ProcessEndConsumer(this, _blobStorage);
            _channel.BasicConsume(queue: RabbitMqConstants.ProcessEndQueue,
                autoAck: false,
                consumer: consumer);
        }

        public void SendAck(ulong deliveryTag)
        {
            _channel.BasicAck(deliveryTag: deliveryTag, multiple: false);
        }

        public void SendFileRemovedEvent(IFileRemovedEvent @event)
        {
            _channel.ExchangeDeclare(exchange: RabbitMqConstants.FileRemovedExchange,
                type: ExchangeType.Fanout);

            _channel.QueueDeclare(queue: RabbitMqConstants.FileRemovedQueue, durable: false, exclusive: false,
                autoDelete: false, arguments: null);
            _channel.QueueBind(queue: RabbitMqConstants.FileRemovedQueue, exchange: RabbitMqConstants.FileRemovedExchange, routingKey: "");

            var serializedCommand = JsonConvert.SerializeObject(@event);

            var properties = _channel.CreateBasicProperties();
            properties.ContentType = RabbitMqConstants.JsonMimeType;

            _channel.BasicPublish(exchange: RabbitMqConstants.FileRemovedExchange, routingKey: "",
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