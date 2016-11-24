using Randomizer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Randomizer.Core
{
    /// <summary>
    /// A class for shuffling a collection, taking random elements from a collection.
    /// </summary>
    public class RandomizerEngine
    {
        private Random rnd = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);

        private int GenerateRandomNumber(int maxValue)
        {
            return rnd.Next(0, maxValue);
        }

        /// <summary>
        /// Takes a random element from a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns>Element from the collection</returns>
        public async Task<T> GetElement<T>(IQueryable<T> collection) where T : IRandomElement
        {
            return await Task.Run(() =>
            {
                if (collection == null)
                {
                    throw new ArgumentNullException();
                }
                else if (!collection.Any())
                {
                    throw new ArgumentOutOfRangeException();
                }

                var randomNumber = this.GenerateRandomNumber(collection.Count());

                return collection.ElementAt(randomNumber);
            });

        }

        /// <summary>
        /// Takes specific number of random elements from a collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="numberOfElements">How many elements the user wants to take</param>
        /// <returns>A specific number of elements from the collection</returns>
        public async Task<IEnumerable<T>> GetElements<T>(IQueryable<T> collection, int numberOfElements) where T : IRandomElement
        {
            var returnElements = new List<T>();
            var collectionList = await this.Shuffle(collection);

            return await Task.Run(() =>
            {
                if (collection == null)
                {
                    throw new ArgumentNullException();
                }
                else if (numberOfElements > collection.Count() || !collection.Any())
                {
                    throw new ArgumentOutOfRangeException();
                }

                for (int i = 0; i < numberOfElements; i++)
                {
                    returnElements.Add(collectionList.ElementAt(i));
                }

                return returnElements.AsEnumerable<T>();
            });
        }

        /// <summary>
        /// Shuffles a collection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns>Shuffsled collection</returns>
        public async Task<IEnumerable<T>> Shuffle<T>(IQueryable<T> collection) where T : IRandomElement
        {
            var shuffledElements = collection.ToList();

            return await Task.Run(() =>
            {
                if (collection == null)
                {
                    throw new ArgumentNullException();
                }
                else if (!collection.Any())
                {
                    throw new ArgumentOutOfRangeException();
                }

                for (int counter = shuffledElements.Count() - 1; counter > 1; counter--)
                {
                    var randomNumber = this.GenerateRandomNumber(counter);
                    var tempElement = shuffledElements[randomNumber];

                    shuffledElements[randomNumber] = shuffledElements[counter];
                    shuffledElements[counter] = tempElement;
                }

                return shuffledElements.AsEnumerable<T>();
            });
        }



        private uint ConvertIpStringToUint(string ip)
        {
            return (uint)IPAddress.NetworkToHostOrder((int)System.BitConverter.ToUInt32(
               IPAddress.Parse(ip).GetAddressBytes(), 0));
        }

        public List<string> GetIpsList(string ipFrom, string ipTo)
        {
            var ipList = new List<string>();

            uint ip1 = ConvertIpStringToUint(ipFrom);
            uint ip2 = ConvertIpStringToUint(ipTo);

            for (int i = (int)ip1; i <= (int)ip2; i++)
            {
                string newIpAddress = new IPAddress((uint)IPAddress.NetworkToHostOrder(i)).ToString();
                ipList.Add(newIpAddress);
            }

            return ipList;
        }
    }
}
