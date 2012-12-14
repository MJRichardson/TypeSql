using System.Collections.Generic;
using System.Linq;
using TypeSql.Antlr.Runtime;
using TypeSql.Antlr.Runtime.Tree;
using TypeSql.Parsing;

namespace TypeSql.UnitTests.Parsing.ANTLR
{
    public abstract class AntlrParserTest
    {
        protected AntlrParserTest(string sql)
        {
            //act
            var lexer = new TypeSqlLexer(new ANTLRStringStream(sql));
            var parser = new TypeSqlParser(new CommonTokenStream(lexer));
            var parseResult = parser.sql();
            AST = (CommonTree) parseResult.Tree;
        }

        protected CommonTree AST { get; private set; }

        protected IList<CommonTree> GetSubTrees(int type)
        {
            return AST.Children.Where(child => child.Type == type).Cast<CommonTree>().ToList();
        }
    }
}