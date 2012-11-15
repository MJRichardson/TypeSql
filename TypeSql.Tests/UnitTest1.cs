using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeSql.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var records = new ExampleSelectTemplate().Records;
        }
    }
}
