using Microsoft.Speech.Recognition;
using Microsoft.Speech.Synthesis;
using SpeechSynthesis.model;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace View
{

    public partial class MainWindow : Window
    {
        SpeechSynthesis.Manager manager;
        private ObservableCollection<Product> basketItems;
        public Label[] stageLabels { get; } = new Label[4];

        public MainWindow()
        {

            manager = new SpeechSynthesis.Manager(this);
            InitializeComponent();
        }

        private void ProductsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = e.AddedItems[0] as Product;
            manager.synthesisManager.StartSpeaking(a.Name.ToString() + ", cena" + a.Price.ToString());
        }

        public void LogDialogSystem(string text)
        {
            LogDialog($"System: " + text);
        }

        public void LogDialogUser(string text, float confidence)
        {
            LogDialog($"Użytkownik: { text } (confidence: {confidence})");
        }

        private void LogDialog(string Text)
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
            productsListView.ItemsSource = manager.products;

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

        private void MainWindow1_Initialized(object sender, EventArgs e)
        {
            stageLabels[0] = etap1Label;
            stageLabels[1] = etap2Label;
            stageLabels[2] = etap3Label;
            stageLabels[3] = etap4Label;
            Array.ForEach(stageLabels, label => label.Opacity = 0.2);
            etap1Label.Opacity = 1.0;
        }
    }
}
