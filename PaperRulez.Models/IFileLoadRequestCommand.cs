namespace PaperRulez.Models
{
    public interface IFileLoadRequestCommand
    {
        string Client { get; set; }
        string FileName { get; set; }
        byte[] Content { get; set; }
    }
}