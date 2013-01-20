using TypeSql.Parsing;
using Xunit;

namespace TypeSql.UnitTests.Parsing.ANTLR
{
    public class UsingNamespaceWithNoDots: AntlrParserTest
    {
        private const string TypeSql = @"
            using MyNamespace
            SELECT Id:Class1 FROM Users";

        public UsingNamespaceWithNoDots() : base(TypeSql)
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