using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer.Model
{
    public class RandomizerEngine
    {
        private Random rnd = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);

        public T GetElement<T>(IQueryable<T> collection) where T : IRandomElement
        {
            var randomNumber = rnd.Next(0, collection.Count() - 1);

            return collection.ElementAt(randomNumber);
        }

        public IEnumerable<T> GetElements<T>(IQueryable<T> collection, int numberOfElements) where T : IRandomElement
        {
            var returnElements = new List<T>();
            var collectionList = collection.ToList();

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
        }

        public IEnumerable<T> Shuffle<T>(IQueryable<T> collection) where T : IRandomElement
        {
            var shuffledElements = new List<T>();
            var collectionList = collection.ToList();

            while(collectionList.Count() != 0)
            {
                var randomNumber = rnd.Next(0, collectionList.Count() - 1);
                shuffledElements.Add(collectionList.ElementAt(randomNumber));
                collectionList.RemoveAt(randomNumber);
            }

            return shuffledElements.AsEnumerable<T>();
        }

        public List<string> GetIpsList(string ipFrom, string ipTo)
        {
            //"10.10.0.10"//from
            //    "10.10.0.250"//to

            //    //"10.10.1.250"//to

            var ipFromArray = ipFrom.Split('.');
            var ipToArray = ipTo.Split('.');

            var listFrom = new List<int>();
            var listTo = new List<int>();
            var ipsList = new List<string>();

            for (int i = 0; i < ipFrom.Count(); i++)
            {
                listFrom[i] = Convert.ToInt32(ipToArray[i]);
                listTo[i] = Convert.ToInt32(ipToArray[i]);
            }

            int maxNumber = 255;

            //for first number
            if (listTo[0] == listFrom[0])
            {
                if (listTo[1] >= listFrom[1])
                {
                    if (listTo[2] < listFrom[2] && listFrom[1] == listTo[1])
                    {
                        throw new FormatException();
                    }
                    else
                    {
                        for (int i = listFrom[3]; i <= maxNumber; i++)
                        {
                            string ip = ipFromArray[0] + "." + ipFromArray[1] + "." + ipFromArray[2] + "." + i.ToString();
                            ipsList.Add(ip);
                        }

                        for (int i = listFrom[2]; i < maxNumber; i++)
                        {
                            for (int j = 0; j < maxNumber; j++)
                            {
                                string ip = ipFromArray[0] + "." + ipFromArray[1] + "." + i.ToString() + "." + j.ToString();
                                ipsList.Add(ip);
                            }

                        }

                        for (int i = listFrom[1] + 1; i < listTo[1]; i++)
                        {
                            for (int j = 0; j < maxNumber; j++)
                            {
                                for (int k = 0; k < maxNumber; k++)
                                {
                                    string ip = ipFromArray[0] + "." + i.ToString() + "." + j.ToString() + "." + k.ToString();
                                    ipsList.Add(ip);
                                }

                            }
                        }

                        for (int i = 0; i < listTo[2]; i++)
                        {
                            for (int j = 0; j < maxNumber; j++)
                            {
                                string ip = ipFromArray[0] + "." + ipToArray[1] + "." + i.ToString() + "." + j.ToString();
                                ipsList.Add(ip);
                            }

                        }

                        for (int i = 0; i <= listTo[3]; i++)
                        {
                            string ip = ipFromArray[0] + "." + ipToArray[1] + "." + ipToArray[2] + "." + i.ToString();
                            ipsList.Add(ip);
                        }
                    }

                }
                else if (listTo[1] < listFrom[1])
                {
                    throw new FormatException();
                }

            }

            else if (listTo[0] > listFrom[0])
            {

            }
            else throw new FormatException();

            return ipsList;
        }
    }
}
