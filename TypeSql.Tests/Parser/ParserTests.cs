using System.Linq;
using Xunit;

namespace TypeSql.Tests.Parser
{
    public class ParserTests
    {
        [Fact] 
        public void OneOutputNoInputs()
        {
            //arrange
            const string sql = 
                @"SELECT UserId:int 
                FROM Users";

            //act
            var result = TypeSql.Parser.Parser.Parse(sql);

            //assert
            Assert.NotNull(result);
            Assert.Equal(1, result.OutputTokens.Count());
            Assert.Equal("UserId", result.OutputTokens.First().Id);
            Assert.Equal("int", result.OutputTokens.First().Type);
        }

        [Fact] 
        public void TwoOutputsNoInputs()
        {
            //arrange
            const string sql = 
                @"SELECT UserId:int, UserName:string 
                FROM Users";

            //act
            var result = TypeSql.Parser.Parser.Parse(sql);

            //assert
            Assert.NotNull(result);
            Assert.Equal(2, result.OutputTokens.Count());
            Assert.Equal("UserId", result.OutputTokens[0].Id);
            Assert.Equal("int", result.OutputTokens[0].Type);
            Assert.Equal("UserName", result.OutputTokens[1].Id);
            Assert.Equal("string", result.OutputTokens[1].Type);
        }

        [Fact] 
        public void OneOutputOneInput()
        {
            //arrange
            const string sql = 
                @"SELECT Name:string 
                FROM Users
                WHERE Id= @id:int";

            //act
            var result = TypeSql.Parser.Parser.Parse(sql);

            //assert
            Assert.NotNull(result);
            Assert.Equal(2, result.OutputTokens.Count());
            Assert.Equal("Name", result.OutputTokens[0].Id);
            Assert.Equal("string", result.OutputTokens[0].Type);
            Assert.Equal("id", result.InputTokens[0].Id);
            Assert.Equal("int", result.OutputTokens[1].Type);
        }
    }
}