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

        private void ListView_Initialized(object sender, EventArgs e)
        {
            manager.LoadProducts().ForEach(v => { productsListView.Items.Add($"{v.Name} {v.Price}"); });
        }

        private void ProductsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            manager.synthesisManager.StartSpeaking(e.AddedItems[0].ToString());
        }

        public void LogDialog(string Text)
        {
            dialogTextBlock.Text += Text + Environment.NewLine;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            //TODO find something better
            Thread.Sleep(1000);
            manager.StartShopping();
        }
    }
}
