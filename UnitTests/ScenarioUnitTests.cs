using System;
using System.Linq;
using Analysis;
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

        private Scenario<TestScenario> NewScenario => new TestScenario
        {
            Condition1 = "Test",
            Backing2 = 5,
            Backing3 = DateTime.Now,
            Backing4 = true
        }.AsScenario();

        [TestMethod]
        public void TestScenarioSerialization()
        {
            var serializedResults = NewScenario.Get();

            Assert.IsTrue(serializedResults.All(pair => pair.Key.Contains("Condition")));
            Assert.IsTrue(serializedResults.All(pair => !pair.Key.Contains("Backing")));
            Assert.IsTrue(serializedResults.All(pair => pair.Value != ""));
        }

        [TestMethod]
        public void TestScenarioDeSerialization()
        {
            var scenario = NewScenario;
            var serializedResults = scenario.Get();

            var reversed = new TestScenario();
            Scenario.Set(reversed, serializedResults);

            Assert.AreEqual(scenario.Core.Condition1, reversed.Condition1);
            Assert.AreEqual(scenario.Core.Condition2, reversed.Condition2);
            Assert.AreEqual(scenario.Core.Condition3, reversed.Condition3);
            Assert.AreEqual(scenario.Core.Condition4, reversed.Condition4);
        }
    }
}
