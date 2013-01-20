using TypeSql.Parsing;
using Xunit;

namespace TypeSql.UnitTests.Parsing.ANTLR
{
    public class OneOutput : AntlrParserTest
    {
            //arrange
            const string Sql = 
                @"SELECT UserId:int FROM Users";

        public OneOutput() : base(Sql)
        {
        }

        [Fact]
        public void CreatesTypeSqlAst()
        {
           Assert.Equal(TypeSqlParser.TYPESQL,AST.Type);

        }

        [Fact]
        public void AstContainsOutputToken()
        {
            var outputToken = SqlNode.GetChild(1);
            Assert.Equal(TypeSqlParser.OUTPUT_TOKEN, outputToken.Type );
        }

        [Fact]
        public void OutputTokenContainsIdAndType()
        {
            var outputToken = SqlNode.GetChild(1);
            var idNode = outputToken.GetChild(0);
            var typeNode = outputToken.GetChild(1);

            Assert.Equal(TypeSqlParser.ID, idNode.Type);
            Assert.Equal("UserId", idNode.Text);

            Assert.Equal(TypeSqlParser.TYPE, typeNode.Type);
            Assert.Equal("int", typeNode.GetChild(0).Text);
        }
    }
}