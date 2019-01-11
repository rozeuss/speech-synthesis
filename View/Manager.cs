using Microsoft.Speech.Recognition;
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
        public Recognition.GrammarManager grammarManager { get; }
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
            grammarManager.SpeechRecognized += GrammarManager_SpeechRecognized;
            this.mainWindow = mainWindow;
        }

        private void GrammarManager_SpeechRecognized(object sender, EventArgs e)
        {
            //jesli rozpoznalo to sprawdz czy zawiera produkt do kupienia, jesli nei to powtorz,
            // jesli tak idz dalej
            var args = e as SpeechRecognizedEventArgs;
            if (args != null)
            {
                mainWindow.LogDialog($"Odczytano: {args.Result.Text} {args.Result.Confidence}");
                mainWindow.LogDialog2($"Odczytano: {args.Result.Text} {args.Result.Confidence}");
                if (args.Result.Confidence < 0.75)
                {
                    String text = "Proszę powtórzyć";
                    synthesisManager.StartSpeaking(text);
                    mainWindow.LogDialog($"System: {text}");
                    mainWindow.LogDialog2($"System: {text}");

                    grammarManager.StartRecognizing();
                } else
                {
                    ChangeGrammar();
                }
            }
        }

        private void ChangeGrammar()
        {
            mainWindow.LogDialog($"ZMIENIAMY GRAMATYKE ELLO");
            PrzelaczeniGramatyki();
            //throw new NotImplementedException();
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
            mainWindow.LogDialog2($"System: {text}");

            grammarManager.StartRecognizing();
        }

        private void PrzelaczeniGramatyki()
        {
            grammarManager.SRE.RecognizeAsyncCancel();
            grammarManager.SRE.UnloadAllGrammars();
            //if (pIdGrammar == 0)
            //pIdGrammar = 1;
            //else
            //pIdGrammar = 0;
            grammarManager.SRE.LoadGrammar(grammarManager.GetSecondGrammar());
            grammarManager.SRE.RecognizeAsync(RecognizeMode.Multiple);
        }
    }
}
