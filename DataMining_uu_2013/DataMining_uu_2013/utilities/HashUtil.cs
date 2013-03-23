using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMining_uu_2012.utilities
{
    public class HashUtil
    {
        private static Random Ran = new Random();
        // I want to build h number of hash functions that will make the 
        // domain of n to a number in k
        // ie, if n is 26, and k is 10, I need to randomize a subset of n to fit into k
        // I need h many of these...

        private void HashFunctions(int h, int k, int n)
        {
            for (var i = 0; i < h; i++)
            {
                // we need to force n into k now
                // there may be a better way to do it...
                var mappingDictionary = new Dictionary<int, int>();
                var filledDictionary = new Dictionary<int, int>();
                for (var j = 0; j < n; j++)
                {
                    filledDictionary[j] = 0;
                }

                while (mappingDictionary.Keys.Count < n)
                {
                    var indexToChoose = Ran.Next(0, filledDictionary.Keys.Count);
                    var result = filledDictionary.Keys.ElementAt(indexToChoose);
                    //mappingDictionary[result]
                }
            }
        }
    }
}
