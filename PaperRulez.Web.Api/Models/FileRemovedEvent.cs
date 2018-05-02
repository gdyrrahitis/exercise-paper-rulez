namespace PaperRulez.Web.Api.Models
{
    using PaperRulez.Models;
    public class FileRemovedEvent: IFileRemovedEvent
    {
        public string Client { get; set; }
        public string DocumentName { get; set; }
    }
}