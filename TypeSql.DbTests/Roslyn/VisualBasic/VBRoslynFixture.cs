using Roslyn.Scripting;
using Roslyn.Scripting.VisualBasic;

namespace TypeSql.DbTests.Roslyn.VisualBasic
{
    public class VBRoslynFixture : RoslynFixture
    {
        public VBRoslynFixture(string name, string sql) : base(name, sql)
        {
             Session.Execute("Imports System.Linq");
            Session.Execute("Imports System.Core");
            Session.Execute("Imports System.Xml");
            Session.Execute("Imports System.Xml.Linq");
             Session.Execute("Imports System.Data.SqlClient");
             Session.Execute("Imports System.Data.Common");
             Session.Execute("Imports System.Configuration");
        }

        protected override TargetLanguage TargetLanguage
        {
            get { return TargetLanguage.VisualBasic;}
        }

        protected override Session CreateSession()
        {
             var engine = new ScriptEngine();
             return engine.CreateSession();
        }
    }
}