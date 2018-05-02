namespace PaperRulez.Loader.Service
{
    public interface ILoaderFactory
    {
        ILoader Select(string extension);
    }
}