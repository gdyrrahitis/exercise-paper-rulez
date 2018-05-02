namespace PaperRulez.Web.Api.Controllers
{
    using System;
    using System.Web.Http;
    using System.IO;
    using System.Threading.Tasks;
    using Loader.Service;
    using Models;

    [RoutePrefix("api/file")]
    public class FileLoadController : ApiController
    {
        private readonly IRabbitMqManager _manager;
        private readonly IBlobStorage _blobStorage;

        public FileLoadController(IRabbitMqManager manager, IBlobStorage blobStorage)
        {
            _manager = manager;
            _blobStorage = blobStorage;
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Post(FileLoadViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (string.IsNullOrWhiteSpace(model.DocumentName))
                {
                    return BadRequest("Document full name is not in correct format");
                }

                var extension = Path.GetExtension(model.DocumentName).Replace(".", "");

                if (string.IsNullOrWhiteSpace(extension))
                {
                    return BadRequest("Extension is not in correct format");
                }

                var fileBytes = await _blobStorage.LoadDocumentFromSystemAsync(model.Client, model.DocumentName);
                var command = new FileLoadRequestCommand
                {
                    Client = model.Client,
                    FileName = model.DocumentName,
                    Content = fileBytes
                };
                _manager.SendFileLoadRequestCommand(command);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
