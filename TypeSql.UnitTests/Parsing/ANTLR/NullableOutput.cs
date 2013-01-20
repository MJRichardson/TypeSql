using TypeSql.Parsing;
using Xunit;

namespace TypeSql.UnitTests.Parsing.ANTLR
{
    public class NullableOutput : AntlrParserTest
    {

        //arrange
        const string Sql = 
            @"SELECT ParentProductCategoryID:int? FROM SalesLT.ProductCategory";

        public NullableOutput() : base(Sql)
        {
        }

        [Fact]
        public void OutputTokenContainsIdAndType()
        {
            var outputToken = SqlNode.GetChild(1);
            var idNode = outputToken.GetChild(0);
            var typeNode = outputToken.GetChild(1);

            Assert.Equal(TypeSqlParser.ID, idNode.Type);
            Assert.Equal("ParentProductCategoryID", idNode.Text);

            Assert.Equal(TypeSqlParser.TYPE, typeNode.Type);
            Assert.Equal("int", typeNode.GetChild(0).Text);
            Assert.Equal("?", typeNode.GetChild(1).Text);
        }
    }
}