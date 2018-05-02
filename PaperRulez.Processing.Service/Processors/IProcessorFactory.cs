namespace PaperRulez.Processing.Service.Processors
{
    public interface IProcessorFactory
    {
        IProcessor Select(string type);
    }
}