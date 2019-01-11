using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace View
{

    public partial class MainWindow : Window
    {
        SpeechSynthesis.Manager manager;

        public MainWindow()
        {
            manager = new SpeechSynthesis.Manager(this);
            InitializeComponent();
        }

        private void ProductsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            manager.synthesisManager.StartSpeaking(e.AddedItems[0].ToString());
        }

        public void LogDialog(string Text)
        {
            dialogTextBlock.Text += Text + Environment.NewLine;
        }

        public void LogDialog2(string Text)
        {
            dialogListBox.Items.Add(Text + Environment.NewLine);
            dialogListBox.SelectedIndex = dialogListBox.Items.Count - 1;
            dialogListBox.SelectedIndex = -1;
            var border = (Border)VisualTreeHelper.GetChild(dialogListBox, 0);
            var scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
            scrollViewer.ScrollToBottom();
            dialogListBox.ScrollIntoView(dialogListBox.SelectedItem);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            timer.Start();
            timer.Tick += (sender2, args) =>
            {
                timer.Stop();
                manager.StartShopping();

            };

        }

        private void ProductsListView_Loaded(object sender, RoutedEventArgs e)
        {
            //manager.LoadProducts().ForEach(v =>
            //{
            //    productsListView.Items.Add($"{v.Name} {v.Price}");
            //});
            productsListView.ItemsSource = manager.LoadProducts();
        }


        public SpeechRecognitionEngine GetSRE()
        {
            return manager.grammarManager.SRE;
        }

        public SpeechSynthesizer GetTTS()
        {
            return manager.synthesisManager.TTS;
        }

        private void MainWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GetSRE().RecognizeAsyncStop();
            GetSRE().UnloadAllGrammars();
        }



    }
}
