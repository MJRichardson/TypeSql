using TypeSql.Parser;

namespace TypeSql.Templating
{
    internal partial class CSharpTemplate
    {
        private readonly string _name;
        private readonly ParseResult _parseResult;

        public CSharpTemplate(string name, ParseResult parseResult)
        {
            _name = name;
            _parseResult = parseResult;
        }
    }
}