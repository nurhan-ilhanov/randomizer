using Microsoft.VisualStudio.TestTools.UnitTesting;
using Randomizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.Model.Tests
{
    [TestClass()]
    public class RandomizerEngineTests
    {

        [TestMethod()]
        public void GetElementsTest()
        {

            Assert.Fail();
        }

        [TestMethod()]
        public void GetElementTest()
        {
            //Arrange
            var testCollection = new List<IRandomElement>();

            for (int i = 1; i <= 100; i++)
            {
                var element = new RandomElement(i);
                testCollection.Add(element);
            }

            //Act
            var randomElement = RandomizerEngine.GetElement(testCollection.AsQueryable());

            Assert.IsTrue(testCollection.Any(e => e.ID == randomElement.ID));
        }

        [TestMethod()]
        public void ShuffleTest()
        {
            //Arrange
            var testCollection = new List<IRandomElement>();

            for (int i = 1; i <= 10; i++)
            {
                var element = new RandomElement(i);
                testCollection.Add(element);
            }

            //Act
            var shuffledElements = RandomizerEngine.Shuffle(testCollection.AsQueryable());

            shuffledElements.OrderBy(e => e.ID);

            Assert.IsTrue(testCollection.Equals(shuffledElements));
        }
    }
}