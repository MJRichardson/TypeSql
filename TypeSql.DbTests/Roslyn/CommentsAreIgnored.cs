using System.Collections;
using Xunit;

namespace TypeSql.DbTests.Roslyn
{
    public class CommentsAreIgnored_InLineComment : DapperTemplateViaRoslyn
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

    public class CommentsAreIgnored_BlockComment : DapperTemplateViaRoslyn
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

    public class CommentsAreIgnored_BothBlockAndInlineComment : DapperTemplateViaRoslyn
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