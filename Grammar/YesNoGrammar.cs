using Microsoft.Speech.Recognition;
using Microsoft.Speech.Recognition.SrgsGrammar;
using System;
using System.Collections.Generic;

namespace Recognition
{
    public class YesNoGrammar
    {
        public Dictionary<int, string> productIdGrammarDictionary { get; }
        public Grammar grammar { get; }
        System.Globalization.CultureInfo pRecognitionLanguage;

        public YesNoGrammar(System.Globalization.CultureInfo pRecognitionLanguage)
        {
            this.pRecognitionLanguage = pRecognitionLanguage;
            productIdGrammarDictionary = new Dictionary<int, string>();
            grammar = init();
        }
        private Grammar init()
        {
            string[] strWords = new string[] { "tak", "nie" };
            Choices words = new Choices(strWords);
            GrammarBuilder gramBuild = new GrammarBuilder();
            gramBuild.Append(words);
            return new Grammar(gramBuild);
        }
    }
}
