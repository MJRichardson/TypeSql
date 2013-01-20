using Xunit;

namespace TypeSql.DbTests.Roslyn
{
    public class NullableOutput :DapperTemplateViaRoslyn
    {
        public const string Name = @"ParentProductCategoryQuery";
        public const string Sql = @"SELECT ParentProductCategoryID:int? FROM SalesLT.ProductCategory WHERE ProductCategoryID = @ProductCategoryId:int";

        public NullableOutput() : base(Name, Sql)
        {
            Session.Execute("var query = new ParentProductCategoryQuery(\"AdventureWorks\");") ;
            Session.Execute("var resultWithoutValue = query.Execute(1).Single();");
            Session.Execute("var resultWithValue = query.Execute(5).Single();");
        }

        [Fact]
        public void NullDbValuesDoNotHaveValue()
        {
           Assert.Equal(false, 
               Session.Execute("resultWithoutValue.ParentProductCategoryID.HasValue")); 
        }

        [Fact]
        public void NonNullDbValuesHaveValue()
        {
           Assert.Equal(true, 
               Session.Execute("resultWithValue.ParentProductCategoryID.HasValue")); 

           Assert.Equal(1, 
               Session.Execute("resultWithValue.ParentProductCategoryID.Value")); 
        }
    }
}