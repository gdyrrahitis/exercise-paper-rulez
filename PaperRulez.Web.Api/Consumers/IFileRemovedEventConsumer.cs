namespace PaperRulez.Web.Api.Consumers
{
    using PaperRulez.Models;

    public interface IFileRemovedEventConsumer
    {
        void Consume(IFileRemovedEvent command);
    }
}