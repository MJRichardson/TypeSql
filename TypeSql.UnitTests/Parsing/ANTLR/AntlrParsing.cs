using System.Collections.Generic;
using System.IO;
using TypeSql.Antlr.Runtime;
using TypeSql.Antlr.Runtime.Tree;
using TypeSql.Antlr3.ST;
using TypeSql.Antlr3.ST.Language;
using TypeSql.Parsing;
using Xunit;
using System.Linq;

namespace TypeSql.UnitTests.Parsing.ANTLR
{
    public class AntlrParsing
    {
        [Fact]
        public void Template()
        {
            //arrange
            const string sql = 
                @"SELECT UserId:int FROM Users";
            
            //act
            var input = new ANTLRStringStream(sql);
            var lexer = new TypeSqlLexer(input);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new TypeSqlParser(tokenStream);
            var parseResult = parser.sql();

            var templateGroup = new StringTemplateGroup(
                new StreamReader(new FileStream(@"..\..\..\TypeSql\Parsing\DapperDao.stg", FileMode.Open)),
                typeof (TemplateLexer));

            var dapperDaoTransform = new DaoTransform(new CommonTreeNodeStream(parseResult.Tree))
                {
                    TemplateGroup = templateGroup 
                };

            //var result = dapperDaoTransform.sql("UserIds").Template.ToString();



        }

        [Fact]
        public void OneOutput()
        {
            
            //arrange
            const string sql = 
                @"SELECT UserId:int FROM Users";
            
            //act
            var input = new ANTLRStringStream(sql);
            var lexer = new TypeSqlLexer(input);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new TypeSqlParser(tokenStream);

            var parseResult = parser.sql();

        }

        [Fact]
        public void TwoOutputs()
        {
            
            //arrange
            const string sql = 
                @"SELECT UserId:int, UserName:string FROM Users";
            
            //act
            var input = new ANTLRStringStream(sql);
            var lexer = new TypeSqlLexer(input);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new TypeSqlParser(tokenStream);

            var parseResult = parser.sql();

        }
        
        [Fact]
        public void OneOutputOneInput()
        {
            
            //arrange
            const string sql = 
                @"SELECT Name:string FROM Users WHERE Id= @id:int";
            
            //act
            var input = new ANTLRStringStream(sql);
            var lexer = new TypeSqlLexer(input);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new TypeSqlParser(tokenStream);

            var parseResult = parser.sql();

        }
        //private static IEnumerable<TypeSqlOutputToken> GetOutputTokens(Lexer lexer)
        //{
        //    IToken token=lexer.NextToken();
        //    while (token.Type != TypeSqlLexer.EOF)
        //    {
        //        var outputToken = token as TypeSqlOutputToken;
        //        if (outputToken != null)
        //            yield return outputToken;

        //        token = lexer.NextToken();
        //    }
        //}
    }
}