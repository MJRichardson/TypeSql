
using TypeSql.Parsing;

namespace TypeSql.Templating.Dapper
{
    internal partial class DapperDaoTemplate
    {
        private readonly string _name;
        private readonly ParseResult _parseResult;

        public DapperDaoTemplate(string name, ParseResult parseResult)
        {
            _name = name;
            _parseResult = parseResult;
        }
    }
}