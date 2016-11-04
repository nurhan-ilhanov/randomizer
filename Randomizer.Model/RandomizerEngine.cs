using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.Model
{
    public class RandomizerEngine
    {
        private Random rnd = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);

        public async Task<T> GetElement<T>(IQueryable<T> collection) where T : IRandomElement
        {
            return await Task.Run(() =>
             {
                 var randomNumber = rnd.Next(0, collection.Count() - 1);

                 return collection.ElementAt(randomNumber);
             });
            
        }

        public async Task<IEnumerable<T>> GetElements<T>(IQueryable<T> collection, int numberOfElements) where T : IRandomElement
        {
            var returnElements = new List<T>();
            var collectionList = collection.ToList();

            return await Task.Run(() =>
            {
                if (numberOfElements > collection.Count())
                {
                    throw new ArgumentOutOfRangeException();
                }
                else
                {
                    for (int i = 0; i < numberOfElements; i++)
                    {
                        var randomNumber = rnd.Next(0, collection.Count() - 1);
                        returnElements.Add(collection.ElementAt(randomNumber));
                        collectionList.RemoveAt(randomNumber);
                    }
                }

                return returnElements.AsEnumerable<T>();
            });
        }

        public async Task<IEnumerable<T>> Shuffle<T>(IQueryable<T> collection) where T : IRandomElement
        {
            var shuffledElements = new List<T>();
            var collectionList = collection.ToList();

            return await Task.Run(() =>
            {
                while (collectionList.Count() != 0)
                {
                    var randomNumber = rnd.Next(0, collectionList.Count() - 1);
                    shuffledElements.Add(collectionList.ElementAt(randomNumber));
                    collectionList.RemoveAt(randomNumber);
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
