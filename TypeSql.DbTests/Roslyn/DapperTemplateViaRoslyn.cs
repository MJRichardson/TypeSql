using System.Data;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;

namespace TypeSql.DbTests.Roslyn
{
    public abstract class DapperTemplateViaRoslyn
    {
         protected DapperTemplateViaRoslyn(string name, string sql )
         {

            //var lexer = new TypeSqlLexer(new ANTLRStringStream(sql));
            // //var lexer2 = new TypeSqlLexer(new ANTLRStringStream(sql));

            // //var commonTokenStream = new CommonTokenStream(lexer2);
            // var rewriteTokenStream = new TokenRewriteStream(lexer);

            //var parser = new TypeSqlParser(rewriteTokenStream);
            //var parseResult = parser.sql();
            //var ast = (CommonTree) parseResult.Tree;
            // var nodeStream = new CommonTreeNodeStream(ast);
            // var rawSqlOutput = new RawSqlTransform(nodeStream);
            // nodeStream.TokenStream = rewriteTokenStream;
            // rawSqlOutput.sql();
            // string rawSql = rewriteTokenStream.ToString();
             
            // //parser = new TypeSqlParser(commonTokenStream);
            // //parseResult = parser.sql();
            // //ast = (CommonTree)parseResult.Tree;
            // //nodeStream = new CommonTreeNodeStream(ast) {TokenStream = rewriteTokenStream};
            // lexer.Reset();
            // rewriteTokenStream.Reset();
            // parser.Reset();
            // nodeStream.Reset();
            // var daoTransform = new DaoTransform(nodeStream){TemplateGroup = new StringTemplateGroup(
            //    new StreamReader(new FileStream(@"..\..\..\TypeSql\Parsing\DapperDao.stg", FileMode.Open)),
            //    typeof (TemplateLexer))};
            // var template = daoTransform.sql(name, rawSql).Template;
            // string src = template.ToString();


             //var parseResult = Parser.Parse(sql);

             //var template = new DapperDaoTemplate(name, parseResult);

             //var transformedTemplate = template.TransformText();

             var compileResult = TypeSqlCompiler.Compile(sql, name);


             var engine = new ScriptEngine();
             Session = engine.CreateSession();
             Session.AddReference("System");
             Session.AddReference("System.Core");
             Session.AddReference(typeof(DapperDao<>).Assembly);
             Session.AddReference(typeof(IDbConnection).Assembly);
             Session.AddReference("System.Configuration");
             Session.Execute("using System.Linq;");
             Session.Execute("using System.Data.SqlClient;");
             Session.Execute("using System.Data.Common;");
             Session.Execute("using System.Configuration;");

             Session.Execute(compileResult.Dao);
         }

         protected Session Session { get;  set; }
    }
}