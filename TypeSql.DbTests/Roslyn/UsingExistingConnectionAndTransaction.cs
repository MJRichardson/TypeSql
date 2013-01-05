using System.Data.SqlClient;
using Roslyn.Scripting;
using Xunit;

namespace TypeSql.DbTests.Roslyn
{
    public class UsingExistingConnectionAndTransaction : DapperTemplateViaRoslyn
    {
         
        public const string Name = @"CustomerLastNameByIdQuery";
        public const string Sql = @"SELECT LastName:string FROM SalesLT.Customer WHERE CustomerID = @id:int";

        public UsingExistingConnectionAndTransaction() : base(Name, Sql)
        {
            Session.Execute("var connectionString = ConfigurationManager.ConnectionStrings[\"AdventureWorks\"].ConnectionString;");
            Session.Execute("var connection = new SqlConnection(connectionString);");
            Session.Execute("connection.Open();");
            Session.Execute("var tx = connection.BeginTransaction();");
            Session.Execute("var query = new CustomerLastNameByIdQuery(connection, tx);") ;
            var results = Session.Execute("var result = query.Execute(11).Single();");
            Session.Execute("tx.Commit();");
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