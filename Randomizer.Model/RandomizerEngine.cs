using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.Model
{
    public static class RandomizerEngine
    {
        private static Random rnd = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);

        public static T GetElement<T>(IQueryable<T> collection) where T : IRandomElement
        {
            var randomNumber = rnd.Next(0, collection.Count() - 1);

            return collection.ElementAt(randomNumber);
        }

        public static IEnumerable<T> GetElements<T>(IQueryable<T> collection, int numberOfElements) where T : IRandomElement
        {
            var returnElements = new List<T>();

            for (int i = 0; i < numberOfElements; i++)
            {
                var randomNumber = rnd.Next(0, collection.Count() - 1);
                returnElements.Add(collection.ElementAt(randomNumber));
            }

            return returnElements.AsEnumerable<T>();

        }

        public static IEnumerable<T> Shuffle<T>(IQueryable<T> collection) where T : IRandomElement
        {
            var shuffledElements = new List<T>();
            T randomElement;

            for (int i = 0; i < collection.Count(); i++)
            {
                var tempElement = collection.ElementAt(i);
                randomElement = collection.ElementAt(rnd.Next(i, collection.Count() - 1));
                
                shuffledElements.Add(randomElement);
            }

            return shuffledElements.AsEnumerable<T>();
        }

        public static List<string> GetIpsList(string ipFrom, string ipTo)
        {
            //"10.10.0.10"//from
            //    "10.10.0.250"//to

            //    //"10.10.1.250"//to

            var ipFromArray = ipFrom.Split('.');
            var ipToArray = ipTo.Split('.');
            int counter = 0;
            List<string> helpArray = new List<string>();
            var ipsList = new List<string>();

            for (int i = ipFromArray.Count() - 1; i >= 0; i++)
            {
                if (String.Compare(ipFromArray[i],ipToArray[i]) < 0)
                {
                    counter++;

                    int from = Convert.ToInt32(ipFromArray[i]);
                    int to = Convert.ToInt32(ipToArray[i]);

                    for (int j = 0; j < to - from; j++)
                    {
                        var ip = ipFromArray;
                        ip[ip.Count() - counter] = to.ToString();
                        ipsList.Add(ip.ToString());

                        to++;
                    }
                }
            }

            return ipsList;
        }
    }
}
