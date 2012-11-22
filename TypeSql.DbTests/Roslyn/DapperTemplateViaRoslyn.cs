using System.Data;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;
using TypeSql.Parsing;
using TypeSql.Templating.Dapper;

namespace TypeSql.DbTests.Roslyn
{
    public abstract class DapperTemplateViaRoslyn
    {
         protected DapperTemplateViaRoslyn(string name, string sql )
         {
             var parseResult = Parser.Parse(sql);

             var template = new DapperDaoTemplate(name, parseResult);

             var transformedTemplate = template.TransformText();


             var engine = new ScriptEngine();
             Session = engine.CreateSession();
             Session.AddReference("System.Core");
             Session.AddReference(typeof(DapperDaoTemplate).Assembly);
             Session.AddReference(typeof(IDbConnection).Assembly);
             Session.Execute("using System.Linq;");

             Session.Execute(transformedTemplate);
         }

         protected Session Session { get;  set; }
    }
}