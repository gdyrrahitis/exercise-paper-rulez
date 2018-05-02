namespace PaperRulez.Loader.Service.BlobStorages
{
    using System.Collections.Generic;

    public abstract class BlobStorageBase
    {
        protected readonly ILoaderFactory LoaderFactory;

        protected BlobStorageBase(ILoaderFactory loaderFactory)
        {
            LoaderFactory = loaderFactory;
        }
    }
}