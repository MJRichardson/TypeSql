using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeSql.Tests
{
    [TestClass]
    public class SelectTemplateTests
    {
        [TestMethod]
        public void SimpleQuery_TypeArguments_NoConditions()
        {
            var records = new ExampleSelectTemplate().Records;
            Assert.IsNotNull(records);
            Assert.IsTrue(records.Any());
        }

        [TestMethod]
        public void SimpleQuery_DynamicArguments_NoConditions() {}

        [TestMethod]
        public void SimpleQuery_TypedAndDynamicArguments_NoConditions() { }


    }
}
