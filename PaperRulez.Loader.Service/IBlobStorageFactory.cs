namespace PaperRulez.Loader.Service
{
    public interface IBlobStorageFactory
    {
        IBlobStorage Select(BlobStorageType type);
    }
}