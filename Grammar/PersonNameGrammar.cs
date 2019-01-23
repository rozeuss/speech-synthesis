using Microsoft.Speech.Recognition;
using Microsoft.Speech.Recognition.SrgsGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recognition
{
    public class PersonNameGrammar
    {
        public Grammar grammar { get; }
        System.Globalization.CultureInfo pRecognitionLanguage;

        public PersonNameGrammar(System.Globalization.CultureInfo pRecognitionLanguage)
        {
            this.pRecognitionLanguage = pRecognitionLanguage;
            grammar = init();
        }
        private Grammar init()
        {
            SrgsRule ruleImie = new SrgsRule("Imię");
            SrgsOneOf imie = new SrgsOneOf(new SrgsItem[]
            {
                new SrgsItem(1, 1, "Damian"),
                new SrgsItem(1, 1, "Jan"),
                new SrgsItem(1, 1, "Katarzyna"),
                new SrgsItem(1, 1, "Paweł"),
                new SrgsItem(1, 1, "Adam"),
                new SrgsItem(1, 1, "Maciej"),
                new SrgsItem(1, 1, "Maja"),
                new SrgsItem(1, 1, "Grzegorz")
            });
            ruleImie.Add(imie);

            SrgsRule ruleNazwisko = new SrgsRule("Nazwisko");
            SrgsOneOf nazwisko = new SrgsOneOf(new SrgsItem[]
            {
                new SrgsItem(1, 1, "Redkiewicz"),
                new SrgsItem(1, 1, "Piechota"),
                new SrgsItem(1, 1, "Klatka"),
                new SrgsItem(1, 1, "Kowalski"),
                new SrgsItem(1, 1, "Nowak"),
                new SrgsItem(1, 1, "Brzęczyszczykiewicz")
            });
            ruleNazwisko.Add(nazwisko);

            SrgsRule rootRule = new SrgsRule("rootBiletomat");
            rootRule.Scope = SrgsRuleScope.Public;
            rootRule.Add(new SrgsRuleRef(ruleImie, "IMIE"));
            rootRule.Add(new SrgsRuleRef(ruleNazwisko, "NAZWISKO"));

            SrgsDocument docBiletomat = new SrgsDocument();
            docBiletomat.Culture = pRecognitionLanguage;
            docBiletomat.Rules.Add(new SrgsRule[]
                { rootRule, ruleImie, ruleNazwisko}
             );     
            docBiletomat.Root = rootRule;
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create("srgsDocument.xml");
            docBiletomat.WriteSrgs(writer);
            writer.Close();
            Grammar gramatyka = new Grammar(docBiletomat, "rootBiletomat");
            return gramatyka;
        }
    }
}
