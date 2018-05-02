namespace PaperRulez.Store.Service.Messages
{
    using Models;
    public class ProcessEndEvent: IProcessEndEvent
    {
        public string Client { get; set; }
        public string DocumentName { get; set; }
    }
}