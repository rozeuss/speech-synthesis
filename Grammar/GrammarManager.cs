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
        public SpeechRecognitionEngine SRE { get; }
        public System.Globalization.CultureInfo pRecognitionLanguage = new System.Globalization.CultureInfo("pl-PL");
        public Grammar[] pGrammars { get; }
        public FirstGrammar FirstGrammar { get; }

        public GrammarManager()
        {
            SRE = new SpeechRecognitionEngine(pRecognitionLanguage);
            SRE.SpeechRecognized += SRE_SpeechRecognized;
            SRE.SetInputToDefaultAudioDevice();
            pGrammars = new Grammar[3];     
            FirstGrammar = new FirstGrammar(pRecognitionLanguage);
            pGrammars[0] = FirstGrammar.grammar;
            pGrammars[1] = new SecondGrammar(pRecognitionLanguage).grammar;
            pGrammars[2] = new ThirdGrammar(pRecognitionLanguage).grammar;
            SRE.LoadGrammar(pGrammars[0]);
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
        protected virtual void OnSpeechRecognized(SpeechRecognizedEventArgs e)
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
                OnSpeechRecognized(e);
            }
        }



    }
    public class GrammarManagerEventArgs : EventArgs
    {
        public string RecognizedText { get; set; }
    }
}
