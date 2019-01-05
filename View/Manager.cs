using Recognition;
using SpeechSynthesis.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using View;

namespace SpeechSynthesis
{
    public class Manager
    {
        public Synthesis.SynthesisManager synthesisManager { get; }
        private Recognition.GrammarManager grammarManager;
        private MainWindow mainWindow;


        public Manager(MainWindow mainWindow)
        {
            using (var db = new DatabaseModel())
            {
                if (db.Products.Count() == 0)
                    InitalizeDb();
            }

            synthesisManager = new Synthesis.SynthesisManager();
            grammarManager = new Recognition.GrammarManager();
            grammarManager.SpeechRecognized += GrammarManager_SpeechRecognized1;
            this.mainWindow = mainWindow;
        }

        private void GrammarManager_SpeechRecognized1(object sender, EventArgs e)
        {
            var args = e as GrammarManagerEventArgs;
            if (args != null)
            {
                mainWindow.LogDialog($"Odczytano: {args.RecognizedText}");
            }
        }
        public List<Product> LoadProducts()
        {
            List<Product> products = new List<Product>();

            using (var db = new DatabaseModel())
            {
                products = db.Products.ToList();
            }
            return products;
        }
        private void InitalizeDb()
        {
            using (var db = new DatabaseModel())
            {
                Product product1 = new Product() { Name = "Gitara", Price = 2.5 };
                Product product2 = new Product() { Name = "Perkusja", Price = 3.5 };
                Product product3 = new Product() { Name = "Skrzypce", Price = 3.5 };
                Product product4 = new Product() { Name = "Dzwoneczki", Price = 3.5 };
                Product product5 = new Product() { Name = "Bas", Price = 3.5 };
                var products = new List<Product>();
                products.Add(product1);
                products.Add(product2);
                products.Add(product3);
                var orderProducts = new List<Product>(products);
                products.Add(product4);
                products.Add(product5);
                Order order1 = new Order() { Address = "Radom", Person = "Palys", Products = orderProducts };
                Order order2 = new Order() { Address = "Radom", Person = "Palys", Products = orderProducts };
                db.Orders.Add(order1);
                db.Orders.Add(order2);  
                db.Products.AddRange(products);
                db.SaveChanges();
            }
        }

        //public void Printing(string str)
        //{
        //    Console.WriteLine(str);
        //}

        public void StartShopping()
        {
            var text = "WiTAJ W SKLEPIE ZIOMEK, CZEGO POTRZEBA?";
            synthesisManager.StartSpeaking(text);
            mainWindow.LogDialog($"System: {text}");
            grammarManager.StartRecognizing();
        }
    }
}
