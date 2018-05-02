namespace PaperRulez.Processing.Service.Processors
{
    using System.Collections.Generic;
    using System.Linq;

    public class ProcessorFactory: IProcessorFactory
    {
        private readonly IEnumerable<IProcessor> _candidates;

        public ProcessorFactory(IEnumerable<IProcessor> candidates)
        {
            _candidates = candidates;
        }

        public IProcessor Select(string type)
        {
            return (from c in _candidates
                let t = c.Type
                where t == type
                select c).FirstOrDefault();
        }
    }
}