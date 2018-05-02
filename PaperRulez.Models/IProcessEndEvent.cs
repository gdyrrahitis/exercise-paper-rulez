namespace PaperRulez.Models
{
    public interface IProcessEndEvent
    {
        string Client { get; set; } 
        string DocumentName { get; set; } 
    }
}