namespace PaperRulez.Loader.Service.BlobStorages
{
    using System.IO;
    using System.Threading.Tasks;

    public class FileSystemBlobStorage: BlobStorageBase, IBlobStorage
    {
        private readonly string _root;

        public FileSystemBlobStorage(string root, ILoaderFactory loaderFactory) : base(loaderFactory)
        {
            _root = root;
        }

        public async Task<byte[]> LoadDocumentFromSystemAsync(string client, string documentName)
        {
            var path = Path.Combine(_root, client, documentName);
            using (var source = File.OpenRead(path))
            {
                using (var destination = new MemoryStream())
                {
                    await source.CopyToAsync(destination);
                    var extension = Path.GetExtension(documentName)?.Replace(".", "");
                    var loader = LoaderFactory.Select(extension);
                    return await loader.LoadFileAsync(destination);
                }
            }
        }

        public Task<bool> RemoveDocumentAsync(string client, string documentName)
        {
            var path = Path.Combine(_root, client, documentName);
            if (File.Exists(path))
            {
                File.Delete(path);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}