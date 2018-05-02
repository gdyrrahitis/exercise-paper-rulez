namespace PaperRulez.Web.Api.Controllers
{
    using System.ComponentModel.DataAnnotations;

    public class FileLoadViewModel
    {
        [Required]
        public string Client { get; set; } 

        [Required]
        public string DocumentName { get; set; }
    }
}