namespace PaperRulez.Store.Service
{
    using System;
    using Messages;

    public interface IRabbitMqManager : IDisposable
    {
        void ListenForProcessingSucceededEvent();
        void SendAck(ulong deliveryTag);
        void SendProcessEndEvent(ProcessEndEvent processEndEvent);
    }
}