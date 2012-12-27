using Xunit;

namespace TypeSql.UnitTests.Compiler
{
    public class OneOutput : RoslynBasedCompilerTest
    {

        private const string Sql = @"SELECT UserId:int FROM Users";
        private const string FileName = "UserIds";
        
        public OneOutput() : base(FileName, Sql)
        {
        }

        [Fact]
        public void GeneratedClassImplementsMarkerInterface()
        {
            Assert.True(Session.Execute<bool>("new UserIds(\"connectionString\") is ITypeSqlStatement"));
        }

        
    }
}