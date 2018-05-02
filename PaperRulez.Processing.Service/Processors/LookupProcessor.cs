namespace PaperRulez.Processing.Service.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class LookupProcessor : IProcessor
    {
        public string Type { get; } = "lookup";

        public IEnumerable<string> Process(byte[] bytes)
        {
            var content = Encoding.UTF8.GetString(bytes);
            var firstLine = content.Split(new[] { '\n' }, StringSplitOptions.None).First();
            var parameters = firstLine.Replace("\\n\r", "")
                .Split('|')[1]
                .Split(',');

            var pattern = @"\b(" + string.Join("|", parameters) + @")\b";
            var matches = Regex.Matches(content, pattern);
            var uniqueKeywords = new HashSet<string>();

            foreach (Match match in matches)
            {
                uniqueKeywords.Add(match.Value);
            }

            return uniqueKeywords.ToList();
        }
    }
}