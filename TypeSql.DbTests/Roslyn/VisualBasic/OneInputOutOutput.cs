using Xunit;

namespace TypeSql.DbTests.Roslyn.VisualBasic
{
    public class OneInputOutOutput : VBRoslynFixture
    {
        public const string Name = @"CustomerLastNameByIdQuery";
        public const string Sql = @"SELECT LastName:String FROM SalesLT.Customer WHERE CustomerID = @id:Integer";

        public OneInputOutOutput() : base(Name, Sql)
        {
            Session.Execute("Dim connectionStringName As String = \"AdventureWorks\"");
            Session.Execute("Dim query As New CustomerLastNameByIdQuery(connectionStringName)") ;
            var results = Session.Execute("Dim result = query.Execute(11).Single()");
        }

        [Fact(Skip = "Haven't got VB Roslyn tests working yet")]
        public void ReturnsResult()
        {
            Assert.Equal("Harding",
                Session.Execute<string>("result.LastName"));
        }
    }
}