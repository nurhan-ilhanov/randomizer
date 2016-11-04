using Microsoft.VisualStudio.TestTools.UnitTesting;
using Randomizer.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Randomizer.Core.Tests
{
    [TestClass()]
    public class RandomizerEngineTests
    {
        [TestMethod()]
        public async Task GetElementsTest()
        {
            //Arrange
            var testCollection = new List<IRandomElement>();

            for (int i = 1; i <= 100; i++)
            {
                var element = new RandomElement(i);
                testCollection.Add(element);
            }

            //Act
            var randomizerEngine = new RandomizerEngine();
            var randomElements = await randomizerEngine.GetElements(testCollection.AsQueryable(), 3);

            Assert.IsTrue(!randomElements.Except(testCollection).Any());
        }

        [TestMethod()]
        public async Task GetElementTest()
        {
            //Arrange
            var testCollection = new List<IRandomElement>();

            for (int i = 1; i <= 100; i++)
            {
                var element = new RandomElement(i);
                testCollection.Add(element);
            }

            //Act
            var randomizerEngine = new RandomizerEngine();
            var randomElement = await randomizerEngine.GetElement(testCollection.AsQueryable());

            Assert.IsTrue(testCollection.Any(e => e.ID == randomElement.ID));
        }

        [TestMethod()]
        public async Task ShuffleTest()
        {
            //Arrange
            var testCollection = new List<IRandomElement>();

            for (int i = 1; i <= 10; i++)
            {
                var element = new RandomElement(i);
                testCollection.Add(element);
            }

            //Act
            var randomizerEngine = new RandomizerEngine();
            var shuffledElements = await randomizerEngine.Shuffle(testCollection.AsQueryable());

            Assert.IsTrue(testCollection.SequenceEqual(shuffledElements.OrderBy(e => e.ID).ToList()));
        }
    }
}