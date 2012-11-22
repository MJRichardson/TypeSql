using System.Collections.Generic;
using System.Linq;

namespace TypeSql.Parsing
{
    internal class ParseResult
    {
        public string UnadornedSql { get; private set; }
        public IList<InputToken> InputTokens { get; private set; }
        public IList<OutputToken> OutputTokens { get; private set; }



        public ParseResult(IEnumerable<OutputToken> outputTokens, IEnumerable<InputToken> inputTokens, string unadornedSql )
        {
            UnadornedSql = unadornedSql;
            InputTokens = inputTokens.ToList().AsReadOnly();
            OutputTokens = outputTokens.ToList().AsReadOnly();
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

    internal class InputToken
    {
        public string Id { get; private set; }
        public string Type { get; private set; }

        public InputToken(string id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}