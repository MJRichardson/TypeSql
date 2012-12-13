using System.Linq;
using TypeSql.Parsing;
using Xunit;

namespace TypeSql.UnitTests.Parsing.ANTLR
{
    public class BracketedOutputId : AntlrParserTest
    {
         
            const string Sql = 
                @"SELECT [Name]:string FROM Users";

        public BracketedOutputId() : base(Sql)
        {
        }

        [Fact]
        public void FindsOutputToken()
        {
            
           Assert.Equal(1, GetSubTrees(TypeSqlParser.OUTPUT_TOKEN).Count );
            var outputTokenTree = GetSubTrees(TypeSqlParser.OUTPUT_TOKEN).First();
            Assert.Equal("Name", outputTokenTree.GetChild(0).Text);
            Assert.Equal("string", outputTokenTree.GetChild(1).Text);
        }
    }
}