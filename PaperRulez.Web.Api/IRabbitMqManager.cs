namespace PaperRulez.Web.Api
{
    using System;
    using PaperRulez.Models;

    public interface IRabbitMqManager: IDisposable
    {
        void ListenForFileRemovedEvent();
        void SendAck(ulong deliveryTag);
        void SendFileLoadRequestCommand(IFileLoadRequestCommand command);
    }
}