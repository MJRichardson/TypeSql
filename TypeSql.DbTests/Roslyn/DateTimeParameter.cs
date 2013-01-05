using System;
using Xunit;

namespace TypeSql.DbTests.Roslyn
{
    public class DateTimeParameter : DapperTemplateViaRoslyn
    {
        public const string Name = @"SalesOrderDueDate";
        public const string Sql = @"SELECT DueDate:DateTime FROM SalesLT.SalesOrderHeader WHERE SalesOrderID = @id:int";

        public DateTimeParameter() : base(Name, Sql)
        {
            Session.Execute("var query = new SalesOrderDueDate(\"AdventureWorks\");") ;
            var results = Session.Execute("var result = query.Execute(71935).Single();");
        }

        [Fact]
        public void ReturnsResult()
        {
            Assert.Equal(new DateTime(2004, 6, 13),
            Session.Execute<DateTime>("result.DueDate"));
        }
    }
}