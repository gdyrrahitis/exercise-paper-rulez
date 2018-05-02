namespace PaperRulez.Web.Api.Controllers
{
    using System.Collections.Generic;

    public class ClientsViewModel
    {
        public IEnumerable<ClientViewModel> Clients { get; set; }
    }

    public class ClientViewModel
    {
        public string Name { get; set; }
        public IList<DocumentViewModel> Documents { get; set; } 
    }

    public class DocumentViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}