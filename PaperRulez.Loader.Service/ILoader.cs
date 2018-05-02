namespace PaperRulez.Loader.Service
{
    using System.IO;
    using System.Threading.Tasks;

    public interface ILoader
    {
        string Extension { get; }
        Task<byte[]> LoadFileAsync(Stream stream);
    }
}