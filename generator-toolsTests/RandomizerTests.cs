using Microsoft.VisualStudio.TestTools.UnitTesting;
using generator_tools;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace generator_tools.Tests
{
    [TestClass()]
    public class RandomizerTests
    {
        [TestMethod()]
        public void GenerateDateTest()
        {
            var samples = Enumerable.Range(0, 10).Select(e => Randomizer.GenerateDate());

            Assert.IsTrue(samples.Distinct().Count() > 1);
        }

        [TestMethod()]
        public void GenerateIntTest()
        {
            var samples = Enumerable.Range(0, 10).Select(e => new { qty = Randomizer.GenerateInt() });

            Assert.IsTrue(samples.Distinct().Count() > 1);
        }

        [TestMethod()]
        public void GenerateDecimalTest()
        {
            var samples = Enumerable.Range(0, 10).Select(e => Randomizer.GenerateDecimal());

            Assert.IsTrue(samples.Distinct().Count() > 1);
        }

        [TestMethod()]
        public void GenerateBoolTest()
        {
            var samples = Enumerable.Range(0, 10).Select(e => Randomizer.GenerateBool());

            Assert.IsTrue(samples.Distinct().Count() > 1);
        }
    }
}
