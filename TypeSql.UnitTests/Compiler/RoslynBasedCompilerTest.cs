using System.Data;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;

namespace TypeSql.UnitTests.Compiler
{
    public class RoslynBasedCompilerTest
    {
        public RoslynBasedCompilerTest(string name, string sql)
        {
             var compileResult = TypeSqlCompiler.Compile(sql, name);

             var engine = new ScriptEngine();
             Session = engine.CreateSession();
             Session.AddReference("System.Core");
             Session.AddReference(typeof(DapperDao<>).Assembly);
             Session.AddReference(typeof(IDbConnection).Assembly);
             Session.Execute("using System.Linq;");

             Session.Execute(compileResult.Dao);
        }

         protected Session Session { get;  set; }
    }
}