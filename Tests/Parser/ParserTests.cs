using Xunit;

namespace Tests.Parser
{
    public class ParserTests
    {
        [Fact] 
        public void SelectWithOneFieldNoWhereClause()
        {
            //arrange
            const string sql = @"SELECT UserId:int 
                FROM Users";

            var result = TypeSql.Parser.Parser.Parse(sql);

            Assert.NotNull(result);
        }
    }
}