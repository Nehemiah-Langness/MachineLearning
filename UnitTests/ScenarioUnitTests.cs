using System;
using System.Linq;
using Analysis.Attributes;
using Analysis.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class ScenarioUnitTests
    {
        private class TestScenario
        {
            public int Backing2 { get; set; }
            public DateTime Backing3 { get; set; }
            public bool Backing4 { get; set; }

            [Condition]
            public string Condition1 { get; set; }

            [Condition]
            public string Condition2 { get => Backing2.ToString(); set => Backing2 = Convert.ToInt32(value); }

            [Condition]
            public string Condition3 { get => Backing3.ToString("O"); set => Backing3 = Convert.ToDateTime(value); }

            [Condition]
            public string Condition4 { get => Backing4.ToString(); set => Backing4 = Convert.ToBoolean(value); }
        }


        [TestMethod]
        public void TestScenarioSerialization()
        {
            var serializedResults = new TestScenario().GetScenario();

            Assert.IsTrue(serializedResults.All(pair => pair.Key.Contains("Condition")));
            Assert.IsTrue(serializedResults.All(pair => !pair.Key.Contains("Backing")));
            Assert.IsTrue(serializedResults.All(pair => pair.Value != ""));
        }

        [TestMethod]
        public void TestScenarioDeSerialization()
        {
            var scenario = new TestScenario();
            var serializedResults = scenario.GetScenario();

            var reversed = new TestScenario();
            reversed.SetScenario(serializedResults);

            Assert.AreEqual(scenario.Condition1, reversed.Condition1);
            Assert.AreEqual(scenario.Condition2, reversed.Condition2);
            Assert.AreEqual(scenario.Condition3, reversed.Condition3);
            Assert.AreEqual(scenario.Condition4, reversed.Condition4);
        }
    }
}
