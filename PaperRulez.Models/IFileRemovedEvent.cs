namespace PaperRulez.Models
{
    public interface IFileRemovedEvent
    {
        string Client { get; set; }
        string DocumentName { get; set; }
    }
}