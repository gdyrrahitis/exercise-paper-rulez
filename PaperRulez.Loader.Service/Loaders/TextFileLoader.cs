namespace PaperRulez.Loader.Service.Loaders
{
    using System.IO;
    using System.Threading.Tasks;

    public class TextFileLoader : ILoader
    {
        public string Extension { get; } = "txt";

        public async Task<byte[]> LoadFileAsync(Stream stream)
        {
            using (var destination = new MemoryStream())
            {
                using (var source = new BufferedStream(stream))
                {
                    source.Position = 0;
                    source.Seek(0, SeekOrigin.Begin);
                    var buffer = new byte[2048];
                    int bytesRead;
                    while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length)) > 0)
                    {
                        await destination.WriteAsync(buffer, 0, bytesRead);
                    }
                }

                stream.Close();
                return destination.ToArray();
            }
        }
    }
}