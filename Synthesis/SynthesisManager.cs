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
        private SpeechSynthesizer TTS;
        public SynthesisManager()
        {
            // Wywołanie konstruktra obiektu reprezentujęcego silnik syntezy mowy
            TTS = new SpeechSynthesizer();
            GetInstaledVoices();
            TTS.SelectVoice("Microsoft Server Speech Text to Speech Voice (pl-PL, Paulina)");
            TTS.SetOutputToDefaultAudioDevice();
            TTS.StateChanged += TTS_StateChanged;
            TTS.SpeakStarted += TTS_SpeakStarted;
            TTS.SpeakProgress += TTS_SpeakProgress;
            TTS.SpeakCompleted += TTS_SpeakCompleted;
        }

        private void GetInstaledVoices()
        {
            foreach (InstalledVoice voice in TTS.GetInstalledVoices())
            {
                VoiceInfo info = voice.VoiceInfo;
                string AudioFormats = "";
                foreach (SpeechAudioFormatInfo fmt in info.SupportedAudioFormats)
                {
                    AudioFormats += String.Format("{0}",
                    fmt.EncodingFormat.ToString());
                }

                System.Diagnostics.Debug.WriteLine(" Name:       " + info.Name);
                System.Diagnostics.Debug.WriteLine(" Język:       " + info.Culture);
                System.Diagnostics.Debug.WriteLine(" Wiek:        " + info.Age);
                System.Diagnostics.Debug.WriteLine(" Płeć:        " + info.Gender);
                System.Diagnostics.Debug.WriteLine(" Opis:        " + info.Description);
                System.Diagnostics.Debug.WriteLine(" ID:          " + info.Id);
                System.Diagnostics.Debug.WriteLine(" Odblokowany: " + voice.Enabled);
                if (info.SupportedAudioFormats.Count != 0)
                {
                    System.Diagnostics.Debug.WriteLine(" Format audio: " + AudioFormats);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(" Nie wspiera żadnego formatu");
                }

                string AdditionalInfo = "";
                foreach (string key in info.AdditionalInfo.Keys)
                {
                    AdditionalInfo += String.Format("  {0}: {1}\n", key, info.AdditionalInfo[key]);
                }

                System.Diagnostics.Debug.WriteLine(" Dodatkowe inforamcje" + AdditionalInfo);
                System.Diagnostics.Debug.WriteLine("-------------------------------------------------------");
            }
        }

        private void TTS_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("SpeakCompleted");
            System.Diagnostics.Debug.WriteLine("-------------------------------------------------------");
            System.Diagnostics.Debug.WriteLine("\n\n\n\n");
        }

        private void TTS_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("SpeakProgress");
            System.Diagnostics.Debug.WriteLine("\te.AudioPosition: {0}", e.AudioPosition);
            System.Diagnostics.Debug.WriteLine("-------------------------------------------------------");
        }

        private void TTS_SpeakStarted(object sender, SpeakStartedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("SpeakStarted");
            System.Diagnostics.Debug.WriteLine("-------------------------------------------------------");
        }

        public void StopSpeaking()
        {
            TTS.SetOutputToNull(); // TODO WAZNE
        }
    
        public void Speak(string var0)
        {
            TTS.SpeakAsync(var0); //do gadania
        }

        private void TTS_StateChanged(object sender, StateChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Stan syntezatora: {0}", e.State));
        }


    }
}
