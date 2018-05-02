namespace PaperRulez.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILookupStore
    {
        /// <summary>
        /// Records set of keywords identified in the given document for a given client
        /// </summary>
        /// <param name="client">Client identifier</param>
        /// <param name="documentId">Document identifier</param>
        /// <param name="keywords">Enumeration of unique keywords found in the document, in any order</param>
        void Record(string client, string documentId, IEnumerable<string> keywords);
    }
}
