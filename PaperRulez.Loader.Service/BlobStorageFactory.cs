namespace PaperRulez.Loader.Service
{
    using System.Collections.Generic;
    using System.Linq;

    public class BlobStorageFactory : IBlobStorageFactory
    {
        private readonly IEnumerable<IBlobStorage> _candidates;

        public BlobStorageFactory(IEnumerable<IBlobStorage> candidates)
        {
            _candidates = candidates;
        }

        public IBlobStorage Select(BlobStorageType type)
        {
            return (from c in _candidates
                    let name = c.GetType().FullName
                    where name.StartsWith(type.ToString())
                    select c).FirstOrDefault();
        }
    }
}