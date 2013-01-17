using TypeSql.DbTests.Data;
using Xunit;

namespace TypeSql.DbTests.Roslyn
{
    public class EnumOutput : DapperTemplateViaRoslyn
    {
         
        public const string Name = @"ProductCategoryQuery";
        public const string Sql = @"
USING TypeSql.DbTests.Data
SELECT [Name]:ProductCategory FROM SalesLT.ProductCategory WHERE ProductCategoryID = @Id:int ";


        public EnumOutput() : base(Name, Sql)
        {
            Session.Execute("var query = new ProductCategoryQuery(\"AdventureWorks\");") ;
            var result = Session.Execute("var result = query.Execute(2).Single();");
        }

        [Fact]
        public void ReturnTypeContainsEnum()
        {
           Assert.Equal(ProductCategory.Components, 
               Session.Execute("result.Name")); 
        }
    }
}