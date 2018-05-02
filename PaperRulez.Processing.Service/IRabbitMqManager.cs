namespace PaperRulez.Processing.Service
{
    using System;
    using Messages;

    public interface IRabbitMqManager: IDisposable
    {
        void ListenForFileLoadRequestCommand();
        void SendAck(ulong deliveryTag);
        void SendProcessingSucceededEvent(ProcessingSucessEvent processingSuccessEvent);
    }
}