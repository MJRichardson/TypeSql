using System.Collections.Generic;
using System.Linq;

namespace TypeSql.Parser
{
    internal class ParseResult
    {
        public IList<OutputToken> OutputTokens { get; private set; }

        public ParseResult(IEnumerable<OutputToken> outputTokens)
        {
            OutputTokens = outputTokens.ToList();
        }
    }

    internal class OutputToken
    {
        public string Id { get; private set; }
        public string Type { get; private set; }

        public OutputToken(string id, string type )
        {
            Id = id;
            Type = type;
        }
    }
}