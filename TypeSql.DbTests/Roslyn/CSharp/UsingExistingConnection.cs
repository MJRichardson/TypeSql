using Xunit;

namespace TypeSql.DbTests.Roslyn.CSharp
{
    public class UsingExistingConnection : CSharpRoslynFixture
    {
        public const string Name = @"CustomerLastNameByIdQuery";
        public const string Sql = @"SELECT LastName:string FROM SalesLT.Customer WHERE CustomerID = @id:int";

        public UsingExistingConnection() : base(Name, Sql)
        {
            Session.Execute("var connectionString = ConfigurationManager.ConnectionStrings[\"AdventureWorks\"].ConnectionString;");
            Session.Execute("var connection = new SqlConnection(connectionString);");
            Session.Execute("var query = new CustomerLastNameByIdQuery(connection);") ;
            var results = Session.Execute("var result = query.Execute(11).Single();");
            Session.Execute("connection.Dispose();");
        }

        [Fact]
        public void ReturnsResult()
        {
            Assert.Equal("Harding",
                Session.Execute<string>("result.LastName"));
        }
    }
}