namespace PaperRulez.Processing.Service.Processors
{
    using System.Collections.Generic;

    public interface IHandler
    {
        IEnumerable<string> HandleDocumentProcessing(byte[] bytes);
    }
}