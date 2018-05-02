namespace PaperRulez.Loader.Service
{
    using System.Collections.Generic;
    using System.Linq;

    public class LoaderFactory: ILoaderFactory
    {
        private readonly IEnumerable<ILoader> _candidates;

        public LoaderFactory(IEnumerable<ILoader> candidates)
        {
            _candidates = candidates;
        }

        public ILoader Select(string extension)
        {
            return (from c in _candidates
                let ext = c.Extension
                where ext == extension
                select c).FirstOrDefault();
        }
    }
}