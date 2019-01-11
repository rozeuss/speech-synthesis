using Microsoft.Speech.AudioFormat;
using Microsoft.Speech.Synthesis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synthesis
{
    public class SynthesisManager
    {
        // Obiekt reprezentujący silnik syntezy mowy
        public SpeechSynthesizer TTS;
        public SynthesisManager()
        {
            // Wywołanie konstruktra obiektu reprezentujęcego silnik syntezy mowy
            TTS = new SpeechSynthesizer();
            TTS.SelectVoice("Microsoft Server Speech Text to Speech Voice (pl-PL, Paulina)");
            TTS.SetOutputToDefaultAudioDevice();
            TTS.StateChanged += TTS_StateChanged;
            TTS.SpeakStarted += TTS_SpeakStarted;
            TTS.SpeakProgress += TTS_SpeakProgress;
            TTS.SpeakCompleted += TTS_SpeakCompleted;
        }
        private void TTS_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
        }

        private void TTS_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
        }

        private void TTS_SpeakStarted(object sender, SpeakStartedEventArgs e)
        {
        }

        public void StopSpeaking()
        {
            TTS.SetOutputToNull(); // TODO WAZNE
        }
    
        public void StartSpeaking(string var0)
        {
            TTS.SpeakAsync(var0); //do gadania
        }

        private void TTS_StateChanged(object sender, StateChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Stan syntezatora: {0}", e.State));
        }
    }
}
