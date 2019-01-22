using Microsoft.Speech.Recognition;
using Microsoft.Speech.Recognition.SrgsGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recognition
{
    public class AddressGrammar
    {
        public Grammar grammar { get; }
        System.Globalization.CultureInfo pRecognitionLanguage;

        public AddressGrammar(System.Globalization.CultureInfo pRecognitionLanguage)
        {
            this.pRecognitionLanguage = pRecognitionLanguage;
            grammar = init();
        }
        private Grammar init()
        {
            SrgsRule ruleCity = new SrgsRule("Miasto");
            SrgsOneOf city = new SrgsOneOf(new SrgsItem[]
            {
                new SrgsItem(0, 1, "Warszawa"),
                new SrgsItem(0, 1, "Radom"),
                new SrgsItem(0, 1, "Gdańsk"),
                new SrgsItem(0, 1, "Kielce"),
                new SrgsItem(0, 1, "Poznań")
            });
            ruleCity.Add(city);

            SrgsRule ruleStreet = new SrgsRule("Ulica");
            SrgsOneOf street = new SrgsOneOf(new SrgsItem[]
            {
                new SrgsItem(0, 1, "ulica Biała"),
                new SrgsItem(0, 1, "ulica Jasna"),
                new SrgsItem(0, 1, "ulica Ciemna"),
                new SrgsItem(0, 1, "ulica Czarna"),
                new SrgsItem(0, 1, "ulica Szybka"),
                new SrgsItem(0, 1, "ulica Wolna")
            });
            ruleStreet.Add(street);

            SrgsRule ruleLiczba = new SrgsRule("Liczebnik");
            SrgsOneOf liczebnik = new SrgsOneOf(new SrgsItem[]
            {
                new SrgsItem("1"),
                new SrgsItem("2"),
                new SrgsItem("3"),
                new SrgsItem("4"),
                new SrgsItem("5"),
                new SrgsItem("6"),
                new SrgsItem("7"),
                new SrgsItem("8"),
                new SrgsItem("9")
            });
            ruleLiczba.Add(liczebnik);

            SrgsRule rootRule = new SrgsRule("rootBiletomat");
            rootRule.Scope = SrgsRuleScope.Public;

            rootRule.Add(new SrgsRuleRef(ruleCity, "IMIE"));
            rootRule.Add(new SrgsRuleRef(ruleStreet, "NAZWISKO"));
            rootRule.Add(new SrgsRuleRef(ruleLiczba, "LICZBA"));


            SrgsDocument docBiletomat = new SrgsDocument();
            docBiletomat.Culture = pRecognitionLanguage;
            docBiletomat.Rules.Add(new SrgsRule[]
                { rootRule, ruleCity, ruleStreet, ruleLiczba}
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
