using Randomizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Randomizer.Core.Tests
{
    public class RandomizerEngineTests
    {
        [Fact]
        public async Task GetElementsTest()
        {
            //Arrange
            var testCollection = new List<IRandomElement>();

            for (int i = 1; i <= 100; i++)
            {
                var element = new TestElement(i);
                testCollection.Add(element);
            }

            //Act
            var randomizerEngine = new RandomizerEngine();
            var randomElements = await randomizerEngine.GetElements(testCollection.AsQueryable(), 3);

            Assert.True(!randomElements.Except(testCollection).Any());
        }

        [Fact]
        public async Task GetElementTest()
        {
            //Arrange
            var testCollection = new List<IRandomElement>();

            for (int i = 1; i <= 100; i++)
            {
                var element = new TestElement(i);
                testCollection.Add(element);
            }

            //Act
            var randomizerEngine = new RandomizerEngine();
            var randomElement = await randomizerEngine.GetElement(testCollection.AsQueryable());

            Assert.True(testCollection.Any(e => e.ID == randomElement.ID));
        }

        [Fact]
        public async Task ShuffleTest()
        {
            //Arrange
            var testCollection = new List<IRandomElement>();

            for (int i = 1; i <= 10; i++)
            {
                var element = new TestElement(i);
                testCollection.Add(element);
            }

            //Act
            var randomizerEngine = new RandomizerEngine();
            var shuffledElements = await randomizerEngine.Shuffle(testCollection.AsQueryable());

            Assert.True(testCollection.SequenceEqual(shuffledElements.OrderBy(e => e.ID).ToList()));
        }
    }
}
