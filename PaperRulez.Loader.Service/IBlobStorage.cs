namespace PaperRulez.Loader.Service
{
    using System.Threading.Tasks;

    public interface IBlobStorage
    {
        Task<byte[]> LoadDocumentFromSystemAsync(string client, string documentName);
        Task<bool> RemoveDocumentAsync(string client, string documentName);
    }
}