using TypeSql.Parsing;
using Xunit;

namespace TypeSql.UnitTests.Parsing.ANTLR
{
    public class UsingStatement : AntlrParserTest
    {
        private const string TypeSql = @"
            using Namespace1.Namespace2
            SELECT Id:int FROM Users";

        public UsingStatement() : base(TypeSql)
        {
        }

        [Fact]
        public void ParsesUsingStatement()
        {
            var usingStatementNode = AST.Children[0];
            Assert.NotNull(usingStatementNode);
            Assert.Equal(TypeSqlParser.USING, usingStatementNode.Type);
        }
    }
}