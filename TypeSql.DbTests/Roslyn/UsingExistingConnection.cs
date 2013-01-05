using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using Xunit;

namespace TypeSql.DbTests.Roslyn
{
    public class UsingExistingConnection : DapperTemplateViaRoslyn
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