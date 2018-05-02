namespace PaperRulez.Processing.Service
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Consumers;
    using Messages;
    using Models;
    using Newtonsoft.Json;
    using Processors;
    using RabbitMQ.Client;

    public class RabbitMqManager : IRabbitMqManager
    {
        private readonly IModel _channel;

        public RabbitMqManager(IConnectionFactory connectionFactory)
        {
            var connection = connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
            connection.AutoClose = true;
        }

        public void ListenForFileLoadRequestCommand()
        {
            _channel.QueueDeclare(queue: RabbitMqConstants.FileLoadQueue,
                durable: false, exclusive: false,
                autoDelete: false, arguments: null);

            _channel.BasicQos(prefetchCount: 1, prefetchSize: 0, global: false);

            var consumer = new FileLoadRequestCommandConsumer(this, new Handler(new ProcessorFactory(new List<IProcessor>
            {
                new LookupProcessor()
            })));
            _channel.BasicConsume(queue: RabbitMqConstants.FileLoadQueue,
                autoAck: false,
                consumer: consumer);
        }

        public void SendAck(ulong deliveryTag)
        {
            _channel.BasicAck(deliveryTag: deliveryTag, multiple: false);
        }

        public void SendProcessingSucceededEvent(ProcessingSucessEvent @event)
        {
            _channel.ExchangeDeclare(exchange: RabbitMqConstants.ProcessSuccessExchange,
                type: ExchangeType.Fanout);

            _channel.QueueDeclare(queue: RabbitMqConstants.ProcessSuccessQueue, durable: false, exclusive: false,
                autoDelete: false, arguments: null);
            _channel.QueueBind(queue: RabbitMqConstants.ProcessSuccessQueue, exchange: RabbitMqConstants.ProcessSuccessExchange, routingKey: "");

            var serializedCommand = JsonConvert.SerializeObject(@event);

            var properties = _channel.CreateBasicProperties();
            properties.ContentType = RabbitMqConstants.JsonMimeType;

            _channel.BasicPublish(exchange: RabbitMqConstants.ProcessSuccessExchange, routingKey: "",
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