using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kikkare
{
    /// <summary>
    /// A random text generator utilizing Markov chains
    /// </summary>
    class TextGenerator
    {   
        private Random rand;
        private Dictionary<string, IList<string>> chain;

        private static readonly string StartKey = "___start___";
        private static readonly string EndKey = "___end___";

        /// <summary>
        /// Constructs an instance of the text generator
        /// </summary>
        public TextGenerator()
        {
            rand = new Random();
            chain = new Dictionary<string, IList<string>>();
            chain.Add("___start___", new List<string>());
            chain.Add("___end___", new List<string>());
        }


        /// <summary>
        /// Generates the Markov model from the given input text
        /// </summary>
        /// <param name="corpus">The input data as a list of sentences</param>
        public void ReadInput(IEnumerable<string> corpus)
        {
            foreach (string sentence in corpus)
            {
                ProcessSentence(sentence);
            }
        }

        /// <summary>
        /// Generates a sentence from the model.
        /// </summary>
        /// <param name="length">The maximum length of the sentence to generate</param>
        /// <returns>A random sentence generated from the underlying model</returns>
        public String GenerateSentence(int length)
        {
            if (length < 2)
                throw new ApplicationException("Invalid sentence length: length must be more than 1");

            var sentence = new List<string>();
            string word = chain[StartKey].PickRandom(rand);
            sentence.Add(word);
            
            for (;;)
            {
                if (chain[EndKey].Contains(word))
                    break;
                word = chain[word].PickRandom(rand);
                sentence.Add(word);
            }

            return String.Join(" ", sentence);

        }

        private void ProcessSentence(string sentence)
        {
            if (String.IsNullOrEmpty(sentence))
                throw new ApplicationException("Not a proper sentence: " + sentence);

            // Normalize is an extension method to lower all caps words
            String[] words = sentence.Split(' ').Normalize();
            // disregard non sentences 
            if (words.Count() < 2)
                return;

            chain[StartKey].Add(words[0]);
            chain[EndKey].Add(words[words.Length - 1]);

            for (int i = 0; i < words.Length - 1; i++) 
            {
                String word = words[i];
                IList<string> successors;
                if (!chain.TryGetValue(words[i], out successors))
                    successors = new List<string>();

                successors.Add(words[i + 1]);
                chain[word] = successors;
            }
        }
    }
}
