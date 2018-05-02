namespace PaperRulez.Finalizer.Service.Messages
{
    using Models;
    public class FileRemovedEvent: IFileRemovedEvent
    {
        public string Client { get; set; }
        public string DocumentName { get; set; }
    }
}