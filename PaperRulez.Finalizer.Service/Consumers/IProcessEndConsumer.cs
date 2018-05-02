namespace PaperRulez.Finalizer.Service.Consumers
{
    using Models;

    public interface IProcessEndConsumer
    {
        void Consume(IProcessEndEvent command);
    }
}