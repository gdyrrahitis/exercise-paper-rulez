namespace PaperRulez.Processing.Service.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Handler: IHandler
    {
        private readonly IProcessorFactory _processingFactory;

        public Handler(IProcessorFactory processingFactory)
        {
            _processingFactory = processingFactory;
        }

        public IEnumerable<string> HandleDocumentProcessing(byte[] bytes)
        {
            var content = Encoding.UTF8.GetString(bytes);
            var processingType = content.Split(new[] { '\n' }, StringSplitOptions.None)
                .First().Split('|').First();

            return _processingFactory.Select(processingType).Process(bytes);
        }
    }
}