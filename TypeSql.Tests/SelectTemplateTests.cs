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
        public void SimpleQuery_TypeArguments_NoConditions()
        {
            var results = new ExampleSelectTemplate().Execute();
            Assert.IsNotNull(results);
            Assert.IsTrue(results.Any());
        }

        [TestMethod]
        public void SimpleQuery_TypeArguments_NoConditions_Async()
        {
            var executeAsync = new ExampleSelectTemplate().ExecuteAsync();
            var assertResults = executeAsync.ContinueWith((results, state) =>
            {
                IEnumerable<ExampleSelectTemplate.Result> records = results.Result;
                Assert.IsNotNull(records);
                Assert.IsTrue(records.Any());
            }, true);
            Task.WaitAll(executeAsync, assertResults);
        }

        [TestMethod]
        public async Task SimpleQuery_TypeArguments_NoConditions_AsyncAwait()
        {
            var asyncResults = await new ExampleSelectTemplate().ExecuteAsync();
            Assert.IsNotNull(asyncResults);
            Assert.IsTrue(asyncResults.Any());
        }

        [TestMethod]
        public void SimpleQuery_DynamicArguments_NoConditions() {}

        [TestMethod]
        public void SimpleQuery_TypedAndDynamicArguments_NoConditions() { }


    }
}
