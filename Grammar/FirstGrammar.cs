using Microsoft.Speech.Recognition;
using Microsoft.Speech.Recognition.SrgsGrammar;
using System.Collections.Generic;

namespace Recognition
{
    public class FirstGrammar
    {
        public Dictionary<int, string> productIdGrammarDictionary { get; }
        public Grammar grammar { get; }
        System.Globalization.CultureInfo pRecognitionLanguage;

        public FirstGrammar(System.Globalization.CultureInfo pRecognitionLanguage)
        {
            this.pRecognitionLanguage = pRecognitionLanguage;
            productIdGrammarDictionary = new Dictionary<int, string>();
            grammar = init();
        }
        private Grammar init()
        {
            SrgsRule ruleOrzeczenie = new SrgsRule("Orzeczenie");
            SrgsOneOf orzeczenie = new SrgsOneOf(new SrgsItem[]
            {
                new SrgsItem(0, 1, "chciałbym"),
                new SrgsItem(0, 1, "daj"),
                new SrgsItem(0, 1, "dawaj"),
                new SrgsItem(0, 1, "podaj"),
                new SrgsItem(0, 1, "poproszę"),
                new SrgsItem(0, 1, "proszę"),
                new SrgsItem(0, 1, "sprzedaj")
            });
            ruleOrzeczenie.Add(orzeczenie);

            SrgsRule ruleBezokolicznik = new SrgsRule("Bezokolicznik");
            SrgsOneOf bezokolicznik = new SrgsOneOf(new SrgsItem[]
            {
                new SrgsItem(0, 1, "dostać"),
                new SrgsItem(0, 1, "kupić"),
                new SrgsItem(0, 1, "podać"),
                new SrgsItem(0, 1, "wziąć")
            });
            ruleBezokolicznik.Add(bezokolicznik);

            SrgsRule ruleDopelnienie = new SrgsRule("Dopelnienie");
            string guitarItem = "gitare";
            string drumsItem = "perkusje";
            string violinItem = "skrzypce";
            productIdGrammarDictionary.Add(6, guitarItem);
            productIdGrammarDictionary.Add(7, drumsItem);
            productIdGrammarDictionary.Add(8, violinItem);

            SrgsOneOf dopelnienie = new SrgsOneOf(new SrgsItem[]
            {
               new SrgsItem(0, 1, guitarItem), new SrgsItem(0, 1, drumsItem), new SrgsItem(0, 1, violinItem)

        });
            ruleDopelnienie.Add(dopelnienie);
            // utwórz korzeń.
            SrgsRule rootRule = new SrgsRule("rootBiletomat");
            rootRule.Scope = SrgsRuleScope.Public;

            rootRule.Add(new SrgsRuleRef(ruleOrzeczenie, "ORZECZENIE"));
            rootRule.Add(new SrgsRuleRef(ruleBezokolicznik, "BEZOKOLICZNIK"));
            //rootRule.Add(new SrgsRuleRef(ruleLiczba, "LICZBA"));
            rootRule.Add(new SrgsRuleRef(ruleDopelnienie, "DOPELNIENIE"));

            SrgsDocument docBiletomat = new SrgsDocument();
            docBiletomat.Culture = pRecognitionLanguage;

            docBiletomat.Rules.Add(new SrgsRule[]
                { rootRule, ruleOrzeczenie, ruleBezokolicznik, ruleDopelnienie}
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
