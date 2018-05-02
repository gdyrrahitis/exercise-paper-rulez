namespace PaperRulez.Models
{
    using System.Collections.Generic;

    public interface IProcessingSuccessEvent
    {
        string Client { get; set; } 
        string Id { get; set; } 
        string DocumentName { get; set; }
        IEnumerable<string> Keywords { get; set; } 
    }
}