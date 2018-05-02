namespace PaperRulez.Web.Api
{
    using System;
    using System.Text;
    using Consumers;
    using Newtonsoft.Json;
    using PaperRulez.Models;
    using RabbitMQ.Client;

    public class RabbitMqManager: IRabbitMqManager
    {
        private readonly IModel _channel;

        public RabbitMqManager(IConnectionFactory connectionFactory)
        {
            var connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
            connection.AutoClose = true;
        }

        public void ListenForFileRemovedEvent()
        {
            _channel.QueueDeclare(queue: RabbitMqConstants.FileRemovedQueue,
                durable: false, exclusive: false,
                autoDelete: false, arguments: null);

            _channel.BasicQos(prefetchCount: 1, prefetchSize: 0, global: false);

            var consumer = new FileRemovedEventConsumer(this);
            _channel.BasicConsume(queue: RabbitMqConstants.FileRemovedQueue,
                autoAck: false,
                consumer: consumer);
        }

        public void SendAck(ulong deliveryTag)
        {
            _channel.BasicAck(deliveryTag: deliveryTag, multiple: false);
        }

        public void SendFileLoadRequestCommand(IFileLoadRequestCommand command)
        {
            _channel.ExchangeDeclare(RabbitMqConstants.FileLoadExchange, ExchangeType.Direct);
            _channel.QueueDeclare(RabbitMqConstants.FileLoadQueue, durable: false, exclusive: false, 
                autoDelete: false, arguments: null);
            _channel.QueueBind(RabbitMqConstants.FileLoadQueue, exchange: RabbitMqConstants.FileLoadExchange, routingKey: "");

            var serializedCommand = JsonConvert.SerializeObject(command);
            var properties = _channel.CreateBasicProperties();
            properties.ContentType = RabbitMqConstants.JsonMimeType;

            _channel.BasicPublish(RabbitMqConstants.FileLoadExchange, routingKey: "", 
                basicProperties: properties, body: Encoding.UTF8.GetBytes(serializedCommand));
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