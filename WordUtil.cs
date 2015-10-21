using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kikkare
{
    /// <summary>
    /// A static class containing extension methods to help dealing with stupid word related issues
    /// </summary>
    static class WordUtil
    {
        public static string[] Normalize(this string[] words)
        {
            if (words != null)
            {
                for (int i = 0; i < words.Length; i++)
                {
                    // If the word is all caps ie. "SKANDAALI" or some other bullshit, convert to lower case
                    if (i == 0)
                        words[i] = (words[i].Equals(words[i].ToUpper())) ? (words[i][0] + words[i].Substring(1).ToLowerInvariant()) : words[i];
                    else
                        words[i] = (words[i].Equals(words[i].ToUpper())) ? words[i].ToLowerInvariant() : words[i];
                }
            }
            return words;
        }

        public static string PickRandom(this IEnumerable<string> input, Random rand)
        {
            if (input == null || rand == null || input.Count() == 0)
                throw new ArgumentNullException("Input must be a nonempty collection and an initialized random number generator");

            int index = rand.Next(input.Count());
            return input.ElementAt(index);
        }

        public static int GetAverageLength(this IEnumerable<string> input)
        {
            if (input == null || input.Count() == 0)
                return 0;

            int sentenceCount = input.Count();
            int wordSum = 0;
            foreach (string sentence in input)
            {
                wordSum += sentence.Split(' ').Count();
            }

            return (wordSum == 0) ? wordSum : wordSum / sentenceCount;
        }
    }
}
