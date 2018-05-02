namespace PaperRulez.Store.Service.Consumers
{
    using Models;

    public interface IProcessingSucessEventConsumer
    {
        void Consume(IProcessingSuccessEvent command);
    }
}