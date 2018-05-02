namespace PaperRulez.Processing.Service.Consumers
{
    using Models;

    public interface IFileLoadRequestCommandConsumer
    {
        void Consume(IFileLoadRequestCommand command);
    }
}