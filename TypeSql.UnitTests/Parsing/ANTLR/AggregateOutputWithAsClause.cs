using System.Linq;
using TypeSql.Parsing;
using Xunit;

namespace TypeSql.UnitTests.Parsing.ANTLR
{
    public class AggregateOutputWithAsClause : AntlrParserTest
    {
         
            const string Sql = 
                @"SELECT COUNT(*) AS Count:int FROM Users";

        public AggregateOutputWithAsClause() : base(Sql)
        {
        }

        [Fact]
        public void OutputTokenIsParsed()
        {
           Assert.Equal(1, GetSubTrees(TypeSqlParser.OUTPUT_TOKEN).Count); 
            var outputTokenTree = GetSubTrees(TypeSqlParser.OUTPUT_TOKEN).First();
            Assert.Equal("Count", outputTokenTree.GetChild(0).Text);
            Assert.Equal("int", outputTokenTree.GetChild(1).Text);
        }
    }
}