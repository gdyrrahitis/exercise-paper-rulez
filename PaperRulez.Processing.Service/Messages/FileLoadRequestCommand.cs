namespace PaperRulez.Processing.Service.Messages
{
    using Models;

    public class FileLoadRequestCommand : IFileLoadRequestCommand
    {
        public string Client { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}