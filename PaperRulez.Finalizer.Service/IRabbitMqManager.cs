namespace PaperRulez.Finalizer.Service
{
    using System;
    using Models;

    public interface IRabbitMqManager : IDisposable
    {
        void ListenForProcessEndEvent();
        void SendAck(ulong deliveryTag);
        void SendFileRemovedEvent(IFileRemovedEvent @event);
    }
}