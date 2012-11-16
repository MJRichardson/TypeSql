using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeSql.Tests.Templates;

namespace TypeSql.Tests
{
    [TestClass]
    public class SelectTemplateTests
    {
        [TestMethod]
        public void SelectTemplate_TypeArguments_NoConditions()
        {
            var results = new SelectTemplate().Execute();
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void SelectTemplate_TypeArguments_NoConditions_Async()
        {
            var executeAsync = new SelectTemplate().ExecuteAsync();
            var assertResults = executeAsync.ContinueWith((results, state) =>
            {
                IEnumerable<SelectTemplate.Result> records = results.Result;
                Assert.IsNotNull(records);
                Assert.IsTrue(records.Any());
            }, true);
            Task.WaitAll(executeAsync, assertResults);
        }

        [TestMethod]
        public async Task SelectTemplate_TypeArguments_NoConditions_AsyncAwait()
        {
            var asyncResults = await new SelectTemplate().ExecuteAsync();
            Assert.IsNotNull(asyncResults);
            Assert.IsTrue(asyncResults.Any());
            Assert.IsTrue(asyncResults.Count() > 0);
            Assert.IsTrue(asyncResults.Any(c => c.Name.StartsWith("A")));    
            
        }

    }
}
