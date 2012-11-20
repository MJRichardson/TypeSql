using System.Collections.Generic;
using TypeSql.Parser;
using TypeSql.Templating;
using Xunit;

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

        //[Fact]
        //public void DapperTemplateTest()
        //{
        //    //arrange 
        //    var parseResult = new ParseResult(
        //        new List<OutputToken> { new OutputToken("Name", "string") },
        //        new List<InputToken>(), "SELECT Name FROM Users");

        //    const string sqlName = "UserNameById";

        //    var template = new CSharpTemplate(sqlName, parseResult);

        //    //act
        //    string result = template.TransformText();
        //}
    }
}