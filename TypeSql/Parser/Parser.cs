using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TypeSql.Parser
{
    internal class Parser
    {

        static Parser()
        {
            IdAndTypeRegex = new Regex(IdAndType, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public static ParseResult Parse(string sql)
        {
            var matches = IdAndTypeRegex.Matches(sql);

            var outputTokens = matches.Cast<Match>().Select(match => new OutputToken(match.Groups[IdGroupName].Value, match.Groups[TypeGroupName].Value));

            return new ParseResult(outputTokens);
        }

        private const string IdGroupName = "Id";
        private const string TypeGroupName = "Type";
        private const string LettersAndDigits = @"[A-Za-z0-9]*";
        private const string Id = @"(?<" + IdGroupName + ">" + LettersAndDigits + ")";
        private const string Type = @"(?<" + TypeGroupName + ">" + LettersAndDigits + ")";
        private const string IdAndType = Id + ":" + Type;

        private readonly static Regex IdAndTypeRegex;
    }
}