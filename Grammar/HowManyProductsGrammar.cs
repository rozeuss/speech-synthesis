using Microsoft.Speech.Recognition;
using Microsoft.Speech.Recognition.SrgsGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recognition
{
    public class HowManyProductsGrammar
    {
        public Grammar grammar { get; }
        System.Globalization.CultureInfo pRecognitionLanguage;
        public Dictionary<int, string> QuantityDict { get; } = new Dictionary<int, string>(); 

        public HowManyProductsGrammar(System.Globalization.CultureInfo pRecognitionLanguage)
        {
            this.pRecognitionLanguage = pRecognitionLanguage;
            grammar = init();
        }
        private Grammar init()
        {
            string[] strWords = new string[] { "jedną", "dwie", "trzy", "cztery" };
            int i = 1;
            Array.ForEach(strWords, word =>
            {
                QuantityDict.Add(i++, word);
            });

            Choices words = new Choices(strWords);
            GrammarBuilder gramBuild = new GrammarBuilder();
            gramBuild.Append(words);
            return new Grammar(gramBuild);
        }
    }
}
