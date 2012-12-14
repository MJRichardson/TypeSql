using System.Data;
using System.IO;
using TypeSql.Antlr.Runtime;
using TypeSql.Antlr.Runtime.Tree;
using TypeSql.Antlr3.ST;
using TypeSql.Antlr3.ST.Language;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;
using TypeSql.Parsing;

namespace TypeSql.DbTests.Roslyn
{
    public abstract class DapperTemplateViaRoslyn
    {
         protected DapperTemplateViaRoslyn(string name, string sql )
         {

            var lexer = new TypeSqlLexer(new ANTLRStringStream(sql));
             //var lexer2 = new TypeSqlLexer(new ANTLRStringStream(sql));

             //var commonTokenStream = new CommonTokenStream(lexer2);
             var rewriteTokenStream = new TokenRewriteStream(lexer);

            var parser = new TypeSqlParser(rewriteTokenStream);
            var parseResult = parser.sql();
            var ast = (CommonTree) parseResult.Tree;
             var nodeStream = new CommonTreeNodeStream(ast);
             var rawSqlOutput = new RawSqlTransform(nodeStream);
             nodeStream.TokenStream = rewriteTokenStream;
             rawSqlOutput.sql();
             string rawSql = rewriteTokenStream.ToString();
             
             //parser = new TypeSqlParser(commonTokenStream);
             //parseResult = parser.sql();
             //ast = (CommonTree)parseResult.Tree;
             //nodeStream = new CommonTreeNodeStream(ast) {TokenStream = rewriteTokenStream};
             lexer.Reset();
             rewriteTokenStream.Reset();
             parser.Reset();
             nodeStream.Reset();
             var daoTransform = new DaoTransform(nodeStream){TemplateGroup = new StringTemplateGroup(
                new StreamReader(new FileStream(@"..\..\..\TypeSql\Parsing\DapperDao.stg", FileMode.Open)),
                typeof (TemplateLexer))};
             var template = daoTransform.sql(name, rawSql).Template;
             string src = template.ToString();
;

             //var parseResult = Parser.Parse(sql);

             //var template = new DapperDaoTemplate(name, parseResult);

             //var transformedTemplate = template.TransformText();


             var engine = new ScriptEngine();
             Session = engine.CreateSession();
             Session.AddReference("System.Core");
             Session.AddReference(typeof(DapperDao<>).Assembly);
             Session.AddReference(typeof(IDbConnection).Assembly);
             Session.Execute("using System.Linq;");

             Session.Execute(src);
         }

         protected Session Session { get;  set; }
    }
}