namespace PaperRulez.Processing.Service.Processors
{
    using System.Collections.Generic;

    public interface IProcessor
    {
        string Type { get; }
        IEnumerable<string> Process(byte[] content);
    }
}