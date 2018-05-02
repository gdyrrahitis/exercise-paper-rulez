namespace PaperRulez.Processing.Service.Messages
{
    using System.Collections.Generic;
    using Models;

    public class ProcessingSucessEvent: IProcessingSuccessEvent
    {
        public string Client { get; set; }
        public string Id { get; set; }
        public string DocumentName { get; set; }
        public IEnumerable<string> Keywords { get; set; }
    }
}