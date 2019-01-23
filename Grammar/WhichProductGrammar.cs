using Microsoft.Speech.Recognition;
using Microsoft.Speech.Recognition.SrgsGrammar;
using System.Collections.Generic;

namespace Recognition
{
    public class WhichProductGrammar
    {
        public Dictionary<int, string> productIdGrammarDictionary { get; }
        public Grammar grammar { get; }
        System.Globalization.CultureInfo pRecognitionLanguage;

        public WhichProductGrammar(System.Globalization.CultureInfo pRecognitionLanguage)
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
            string guitarItem = "gitarę";
            string drumsItem = "perkusję";
            string violinItem = "skrzypce";
            string bellsItem = "dzwoneczki";
            string bassItem = "bas";
            productIdGrammarDictionary.Add(6, guitarItem);
            productIdGrammarDictionary.Add(7, drumsItem);
            productIdGrammarDictionary.Add(8, violinItem);
            productIdGrammarDictionary.Add(9, bellsItem);
            productIdGrammarDictionary.Add(10, bassItem);

            SrgsOneOf dopelnienie = new SrgsOneOf(new SrgsItem[]
            {
               new SrgsItem(0, 1, guitarItem), new SrgsItem(0, 1, drumsItem), new SrgsItem(0, 1, violinItem)

            });
            ruleDopelnienie.Add(dopelnienie);
            SrgsRule rootRule = new SrgsRule("rootBiletomat");
            rootRule.Scope = SrgsRuleScope.Public;        
            rootRule.Add(new SrgsRuleRef(ruleOrzeczenie, "ORZECZENIE"));
            rootRule.Add(new SrgsRuleRef(ruleBezokolicznik, "BEZOKOLICZNIK"));
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
            return new Grammar(docBiletomat, "rootBiletomat");
        }
    }
}
