using System;
using System.IO;
using TypeSql.Antlr.Runtime;
using TypeSql.Antlr.Runtime.Tree;
using TypeSql.Antlr3.ST;
using TypeSql.Antlr3.ST.Language;
using TypeSql.Parsing;

namespace TypeSql
{
    public class TypeSqlCompiler
    {

        public static CompileResult Compile(string typeSql, string typeSqlFileName)
        {
            //create the antlr-based lexer and parser
            var lexer = new TypeSqlLexer(new ANTLRStringStream(typeSql));
             var rewriteTokenStream = new TokenRewriteStream(lexer);
            var parser = new TypeSqlParser(rewriteTokenStream);

            //parse the typeSql, producing the AST
            var ast = (CommonTree) parser.typesql().Tree;
             var nodeStream = new CommonTreeNodeStream(ast);

            //transform the AST into raw-sql
             var rawSqlOutput = new RawSqlTransform(nodeStream);
             nodeStream.TokenStream = rewriteTokenStream;
             rawSqlOutput.typeSql();
             string rawSql = rewriteTokenStream.ToString();

            //reset 
             lexer.Reset();
             rewriteTokenStream.Reset();
             nodeStream.Reset();
            //and transform the AST into DAO source code
             var daoTransform = new DaoTransform(nodeStream){TemplateGroup = new StringTemplateGroup(
                new StreamReader(typeof(TypeSqlCompiler).Assembly.GetManifestResourceStream("TypeSql.Parsing.DapperDao.stg")),
                typeof (TemplateLexer))};
             var template = daoTransform.typeSql(typeSqlFileName, rawSql).Template;
             string daoSourceCode = template.ToString();

            return new CompileResult(daoSourceCode, rawSql);
        }
        
        public class CompileResult
        {
            public string Dao { get; private set; }
            public string RawSql { get; private set; }

            internal CompileResult(string dao, string rawSql)
            {
                Dao = dao;
                RawSql = rawSql;
            }
        }
    }
}