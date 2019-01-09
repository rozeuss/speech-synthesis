using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
            dialogListBox.ScrollIntoView(dialogListBox.SelectedItem);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            //manager.StartShopping();



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
    }
}
