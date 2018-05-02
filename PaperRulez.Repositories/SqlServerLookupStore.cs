namespace PaperRulez.Repositories
{
    using System.Collections.Generic;
    using Syrx;

    public class SqlServerLookupStore : ILookupStore
    {
        private readonly ICommander<SqlServerLookupStore> _commander;

        public SqlServerLookupStore(ICommander<SqlServerLookupStore> commander)
        {
            _commander = commander;
        }

        public void Record(string client, string documentId, IEnumerable<string> keywords)
        {
            _commander.Execute(new { client, documentId, keywords = string.Join(",", keywords)});
        }
    }
}
