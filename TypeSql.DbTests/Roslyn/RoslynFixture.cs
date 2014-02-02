using System.Data;
using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;

namespace TypeSql.DbTests.Roslyn
{
    public abstract class RoslynFixture
    {
         protected RoslynFixture(string name, string sql )
         {
             var compileResult = TypeSqlCompiler.Compile(sql, name, TargetLanguage);

             Session = CreateSession();
             Session.AddReference("System");
             Session.AddReference("System.Core");
             Session.AddReference(typeof(DapperDao<>).Assembly);
             Session.AddReference(typeof(IDbConnection).Assembly);
             Session.AddReference(this.GetType().Assembly);
             Session.AddReference("System.Configuration");
             Session.AddReference("System.Xml");
             Session.AddReference("System.Xml.Linq");

             Session.Execute(compileResult.Dao);
         }

         protected abstract TargetLanguage TargetLanguage { get; }

        protected abstract Session CreateSession();

         protected Session Session { get;  set; }
    }
}