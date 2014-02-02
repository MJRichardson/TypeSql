using Roslyn.Scripting;
using Roslyn.Scripting.CSharp;

namespace TypeSql.DbTests.Roslyn.CSharp
{
    public abstract class CSharpRoslynFixture : RoslynFixture
    {
        protected CSharpRoslynFixture(string name, string sql) : base(name, sql)
        {
             Session.Execute("using System.Linq;");
             Session.Execute("using System.Data.SqlClient;");
             Session.Execute("using System.Data.Common;");
             Session.Execute("using System.Configuration;");
        }

        protected override TargetLanguage TargetLanguage
        {
            get { return TargetLanguage.CSharp;}
        }

        protected override Session CreateSession()
        {
             var engine = new ScriptEngine();
             return engine.CreateSession();
        }
    }
}