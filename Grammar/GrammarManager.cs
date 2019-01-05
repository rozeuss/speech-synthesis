using Microsoft.Speech.Recognition;
using Microsoft.Speech.Recognition.SrgsGrammar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recognition
{
    public class GrammarManager
    {
        private SpeechRecognitionEngine SRE;
        public System.Globalization.CultureInfo pRecognitionLanguage = new System.Globalization.CultureInfo("pl-PL");

        public GrammarManager()
        {
            SRE = new SpeechRecognitionEngine(pRecognitionLanguage);
            SRE.SpeechRecognized += SRE_SpeechRecognized;
            SRE.SetInputToDefaultAudioDevice();
            Grammar grammar = CreateGrammar();
            SRE.LoadGrammar(grammar);


        }

        public void StartRecognizing()
        {
            SRE.RecognizeAsync(RecognizeMode.Multiple);
        }

        public void StopRecognizing()
        {
            SRE.RecognizeAsyncStop();
        }


        private string GetValue(SemanticValue Semantics, string keyName)
        {
            string result = "";
            if (Semantics.ContainsKey(keyName))
                result = Semantics[keyName].Value.ToString();
            return result;
        }

        private string GetConfidence(SemanticValue Semantics, string keyName)
        {
            string result = "";
            if (Semantics.ContainsKey(keyName))
                result = Semantics[keyName].Confidence.ToString("0.0000");
            return result;
        }

        public event EventHandler SpeechRecognized;
        protected virtual void OnSpeechRecognized(GrammarManagerEventArgs e)
        {
            EventHandler handler = SpeechRecognized;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private void SRE_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result != null)
            {
                StopRecognizing();
                OnSpeechRecognized(new GrammarManagerEventArgs() { RecognizedText = e.Result.Text});
            }
            //{

            //    txtConfidence.Text = e.Result.Confidence.ToString("0.0000");
            //    if (e.Result.Semantics != null && e.Result.Semantics.Count != 0)
            //    {

            //        txtValue0.Text = GetValue(e.Result.Semantics, "ORZECZENIE");
            //        txtValue.Text = txtValue0.Text;
            //        txtConfidence0.Text = GetConfidence(e.Result.Semantics, "ORZECZENIE");

            //        txtValue1.Text = GetValue(e.Result.Semantics, "BEZOKOLICZNIK");
            //        txtValue.Text += " " + txtValue1.Text;
            //        txtConfidence1.Text = GetConfidence(e.Result.Semantics, "BEZOKOLICZNIK"); ;

            //        txtValue2.Text = GetValue(e.Result.Semantics, "LICZBA");
            //        txtValue.Text += " " + txtValue2.Text;
            //        txtConfidence2.Text = GetConfidence(e.Result.Semantics, "LICZBA");

            //        txtValue3.Text = GetValue(e.Result.Semantics, "DOPELNIENIE");
            //        txtValue.Text += " " + txtValue3.Text;
            //        txtConfidence3.Text = GetConfidence(e.Result.Semantics, "LICZBA");

            //        txtValue4.Text = GetValue(e.Result.Semantics, "PRZYDAWKA");
            //        txtValue.Text += " " + txtValue4.Text;
            //        txtConfidence4.Text = GetConfidence(e.Result.Semantics, "PRZYDAWKA");
            //    }
            //}
        }


        private Grammar CreateGrammar()
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
            SrgsOneOf dopelnienie = new SrgsOneOf(new SrgsItem[]
            {
                new SrgsItem(0, 1, "gitare"),
                new SrgsItem(0, 1, "perkusje"),
                new SrgsItem(0, 1, "skrzypce")

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
    public class GrammarManagerEventArgs : EventArgs
    {
        public string RecognizedText { get; set; }
    }
}
