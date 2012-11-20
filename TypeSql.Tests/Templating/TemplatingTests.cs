using System.Collections.Generic;
using System.Data;
using TypeSql.Parser;
using TypeSql.Templating;
using TypeSql.Templating.Dapper;
using Xunit;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;
using System.Linq;

namespace TypeSql.Tests.Templating
{
    public class TemplatingTests
    {
        //todo: this is just a dummy test, to quickly trial the templating. Remove me later.
        [Fact]
        public void CreateCSharpClassFromParseResult()
        {
           //arrange 
            var parseResult = new ParseResult(
                new List<OutputToken> { new OutputToken("Name", "string") },
                new List<InputToken> { new InputToken("Id", "int") }, "");

            const string sqlName = "UserNameById";

            var template = new CSharpTemplate(sqlName, parseResult);

            //act
            string result = template.TransformText();

        }

        [Fact]
        public void DapperTemplateTest()
        {
            //arrange 
            var parseResult = new ParseResult(
                new List<OutputToken> { new OutputToken("FirstName", "string") },
                new List<InputToken>(), "SELECT FirstName FROM SalesLT.Customer");

            const string sqlName = "CustomerFirstNameQuery";

            var template = new DapperDaoTemplate(sqlName, parseResult);

            //act
            string result = template.TransformText();

            var engine = new ScriptEngine();
            var session = engine.CreateSession();
            session.AddReference("System.Core");
            session.AddReference(typeof (DapperDaoTemplate).Assembly);
            session.AddReference(typeof(IDbConnection).Assembly);
            session.Execute("using System.Linq;");
            session.Execute(result);
            session.Execute("var query = new CustomerFirstNameQuery(\"AdventureWorks\");") ;
            var queryResult = session.Execute("query.Execute().ToList();");

        }
    }
}