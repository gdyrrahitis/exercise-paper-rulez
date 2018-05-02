using System.Web.Mvc;

namespace PaperRulez.Web.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Newtonsoft.Json;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var vm = new ClientsViewModel
            {
                Clients = new List<ClientViewModel>
                {
                    new ClientViewModel
                    {
                        Name = "Facebook",
                        Documents = new List<DocumentViewModel>
                        {
                            new DocumentViewModel
                            {
                                Id = "fb",
                                Name = "fb1_lorem.txt"
                            },
                            new DocumentViewModel
                            {
                                Id = "fb",
                                Name = "fb2_sales.txt"
                            }
                        }
                    },
                    new ClientViewModel
                    {
                        Name = "Microsoft",
                        Documents = new List<DocumentViewModel>
                        {
                            new DocumentViewModel
                            {
                                Id = "ms",
                                Name = "ms_lorem.txt"
                            },
                            new DocumentViewModel
                            {
                                Id = "ms",
                                Name = "ms_sales.txt"
                            }
                        }
                    }
                }
            };
            return View(vm);
        }

        [System.Web.Mvc.HttpPost]
        public async Task Process([FromBody] FileLoadViewModel model)
        {
            using (var http = new HttpClient())
            {
                http.BaseAddress = new Uri("http://localhost:55233/");
                await http.PostAsync("/api/file", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
            }
        }
    }
}
