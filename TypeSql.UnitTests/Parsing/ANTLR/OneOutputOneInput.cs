using System.Linq;
using TypeSql.Parsing;
using Xunit;

namespace TypeSql.UnitTests.Parsing.ANTLR
{
    public class OneOutputOneInput : AntlrParserTest
    {
            const string Sql = 
                @"SELECT Name:string FROM Users WHERE Id= @id:int";

        public OneOutputOneInput() : base(Sql)
        {
        }

        [Fact]
        public void FindsOneInputTokenAndOneOutputToken()
        {
           Assert.Equal(1, GetSubTrees(SqlNode, TypeSqlParser.OUTPUT_TOKEN).Count ); 
        }

        [Fact]
        public void FindsInputToken()
        {
           Assert.Equal(1, GetSubTrees(SqlNode, TypeSqlParser.INPUT_TOKEN).Count );
            var intputTokenTree = GetSubTrees(SqlNode, TypeSqlParser.INPUT_TOKEN).First();
            Assert.Equal("id", intputTokenTree.GetChild(0).Text);
            Assert.Equal("int", intputTokenTree.GetChild(1).GetChild(0).Text);
        }

        [Fact]
        public void FindsOutputToken()
        {
           Assert.Equal(1, GetSubTrees(SqlNode, TypeSqlParser.OUTPUT_TOKEN).Count );
            var intputTokenTree = GetSubTrees(SqlNode, TypeSqlParser.OUTPUT_TOKEN).First();
            Assert.Equal("Name", intputTokenTree.GetChild(0).Text);
            Assert.Equal("string", intputTokenTree.GetChild(1).GetChild(0).Text);
        }
    }
}