using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace TypeSql.Parsing
{
    internal class Parser
    {
        static Parser()
        {
            OutputTokenRegex = new Regex(OutputToken, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            InputTokenRegex = new Regex(InputToken, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            OutputTokenWithBracketedIdRegex = new Regex(OutputTokenWithBracketedId, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public static ParseResult Parse(string sql)
        {
            var outputTokenMatches = OutputTokenRegex.Matches(sql);
            var outputTokenWithBracketedIdMatches = OutputTokenWithBracketedIdRegex.Matches(sql);
            var outputTokens = 
                outputTokenMatches.Cast<Match>()
                .Select(match => new OutputToken(match.Groups[IdGroupName].Value, match.Groups[TypeGroupName].Value))
                .Concat(outputTokenWithBracketedIdMatches.Cast<Match>().Select(match => new OutputToken(match.Groups[IdGroupName].Value, match.Groups[TypeGroupName].Value)));

            var inputTokenMatches = InputTokenRegex.Matches(sql);
            var inputTokens = inputTokenMatches.Cast<Match>().Select(match => new InputToken(match.Groups[IdGroupName].Value, match.Groups[TypeGroupName].Value));

            string unadornedSql =
                InputTokenRegex.Replace(
                    OutputTokenWithBracketedIdRegex.Replace(
                    OutputTokenRegex.Replace(sql, match => match.Groups[IdGroupName].Value), 
                    match => "[" + match.Groups[IdGroupName].Value + "]"),
                    match => "@" + match.Groups[IdGroupName].Value); 

            return new ParseResult(outputTokens, inputTokens, unadornedSql);
        }

        private const string IdGroupName = "Id";
        private const string TypeGroupName = "Type";
        private const string LettersAndDigits = @"[A-Za-z0-9]*";
        private const string WordBoundary = @"\b";
        private const string Id = WordBoundary + @"(?<" + IdGroupName + ">" + LettersAndDigits + ")";
        private const string Type = @"(?<" + TypeGroupName + ">" + LettersAndDigits + ")";
        private const string OutputToken =   Id + ":" + Type;
        private const string InputToken =  "@" + Id + ":" + Type;
        private const string OutputTokenWithBracketedId = @"\[" + Id + "]" + ":" + Type; 

        private readonly static Regex OutputTokenRegex;
        private static readonly Regex InputTokenRegex;
        private static readonly Regex OutputTokenWithBracketedIdRegex;
    }
}