using Xunit;

namespace TypeSql.DbTests.Roslyn
{
    public class OneInputOneOutput : DapperTemplateViaRoslyn
    {
         
        public const string Name = @"CustomerLastNameByIdQuery";
        public const string Sql = @"SELECT LastName:string FROM SalesLT.Customer WHERE CustomerID = @id:int";

        public OneInputOneOutput() : base(Name, Sql)
        {
            Session.Execute("var query = new CustomerLastNameByIdQuery(\"AdventureWorks\");") ;
            var results = Session.Execute("var result = query.Execute(11).Single();");
        }



        [Fact]
        public void ReturnsResult()
        {
            Assert.Equal("Harding",
                Session.Execute<string>("result.LastName"));
        }
    }
}