using Microsoft.Speech.Recognition;
using Microsoft.Speech.Recognition.SrgsGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recognition
{
    public class SecondGrammar
    {
        public Grammar grammar { get; }
        System.Globalization.CultureInfo pRecognitionLanguage;

        public SecondGrammar(System.Globalization.CultureInfo pRecognitionLanguage)
        {
            this.pRecognitionLanguage = pRecognitionLanguage;
            grammar = init();
        }
        private Grammar init()
        {
            string[] strWords = new string[] { "jedną", "dwie", "trzy", "cztery" };
            Choices words = new Choices(strWords);
            GrammarBuilder gramBuild = new GrammarBuilder();
            gramBuild.Append(words);
            return new Grammar(gramBuild);
        }
    }
}
