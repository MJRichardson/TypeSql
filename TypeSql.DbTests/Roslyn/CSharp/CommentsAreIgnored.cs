using Xunit;

namespace TypeSql.DbTests.Roslyn.CSharp
{
    public class CommentsAreIgnored_InLineComment : CSharpRoslynFixture
    {
        public const string Name = @"CustomerFirstNameQuery";
        public const string Sql = @"SELECT FirstName:string FROM SalesLT.Customer -- Test Comment One";

        public CommentsAreIgnored_InLineComment() : base(Name, Sql)
        {
            Session.Execute("var query = new CustomerFirstNameQuery(\"AdventureWorks\");") ;
            var results = Session.Execute("var results = query.Execute().ToList();");
        }

        [Fact]
        public void ReturnsCorrectCount()
        {
            Assert.Equal(847, Session.Execute<int>("results.Count();"));
        }
    }

    public class CommentsAreIgnored_BlockComment : CSharpRoslynFixture
    {
        public const string Name = @"CustomerFirstNameQuery";
        public const string Sql = @"SELECT FirstName:string FROM SalesLT.Customer /* Test Comment One */";

        public CommentsAreIgnored_BlockComment() : base(Name, Sql)
        {
            Session.Execute("var query = new CustomerFirstNameQuery(\"AdventureWorks\");") ;
            var results = Session.Execute("var results = query.Execute().ToList();");
        }

        [Fact]
        public void ReturnsCorrectCount()
        {
            Assert.Equal(847, Session.Execute<int>("results.Count();"));
        }

    }

    public class CommentsAreIgnored_BothBlockAndInlineComment : CSharpRoslynFixture
    {
        public const string Name = @"CustomerFirstNameQuery";
        public const string Sql = @"/* Test Comment One */ SELECT FirstName:string FROM SalesLT.Customer -- Test Comment Two";

        public CommentsAreIgnored_BothBlockAndInlineComment() : base(Name, Sql)
        {
            Session.Execute("var query = new CustomerFirstNameQuery(\"AdventureWorks\");") ;
            var results = Session.Execute("var results = query.Execute().ToList();");
        }

        [Fact]
        public void ReturnsCorrectCount()
        {
            Assert.Equal(847, Session.Execute<int>("results.Count();"));
        }

    }
}