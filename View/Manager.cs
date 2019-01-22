using Microsoft.Speech.Recognition;
using SpeechSynthesis.model;
using System;
using System.Collections.Generic;
using System.Linq;
using View;

namespace SpeechSynthesis
{
    public class Manager
    {
        public Synthesis.SynthesisManager synthesisManager { get; }
        public Recognition.GrammarManager grammarManager { get; }
        private MainWindow mainWindow;
        private int currentGrammar = 0;
        public Dictionary<Product, string> ProductGrammarDict { get; }
        public List<Product> products;

        public Manager(MainWindow mainWindow)
        {
            using (var db = new DatabaseModel())
            {
                if (db.Products.Count() == 0)
                    InitalizeDb();
            }
            loadProducts();


            synthesisManager = new Synthesis.SynthesisManager();
            grammarManager = new Recognition.GrammarManager();
            grammarManager.SpeechRecognized += GrammarManager_SpeechRecognized;
            ProductGrammarDict = new Dictionary<Product, string>();

            foreach (KeyValuePair<int, string> entry in grammarManager.FirstGrammar.productIdGrammarDictionary)
            {
                foreach (Product p in products)
                {
                    if (p.ProductId == entry.Key)
                    {
                        ProductGrammarDict.Add(p, entry.Value);
                    }
                }
            };
            this.mainWindow = mainWindow;
        }

        private void GrammarManager_SpeechRecognized(object sender, EventArgs e)
        {
            //jesli rozpoznalo to sprawdz czy zawiera produkt do kupienia, jesli nei to powtorz,
            // jesli tak idz dalej
            var args = e as SpeechRecognizedEventArgs;
            if (args != null)
            {

                LogDialogUser(args.Result.Text, args.Result.Confidence);
                if (args.Result.Confidence < 0.70)
                {
                    String text = "Proszę powtórzyć";
                    LogDialogSystem(text);
                    grammarManager.StartRecognizing();
                }
                else
                {
                    var appendText = "";
                    foreach (var p in ProductGrammarDict)
                    {
                        var test = p.Value;

                        args.Result.Words.ToList().ForEach(w =>
                        {
                            if (w.Text == test)
                            {
                                Console.WriteLine(w.Text);
                                appendText = w.Text;
                                mainWindow.basketListView.Items.Add(p.Key);
                                
                            }
                        });


                    }


                    SwitchGrammar();
                    if (!String.IsNullOrEmpty(appendText)) {
                        LogDialogSystem($"Czyli chcesz {appendText}.");
                    };
                    String text = "Okej ostro lecimy widzę, ile sztuk?";
                    LogDialogSystem(text);
                }
            }
        }

        private void SwitchGrammar()
        {
            grammarManager.SRE.RecognizeAsyncCancel();
            grammarManager.SRE.UnloadAllGrammars();
            int nextGrammar = ++currentGrammar;
            grammarManager.SRE.LoadGrammar(grammarManager.pGrammars[nextGrammar >= grammarManager.pGrammars.Length ? 0 : nextGrammar]);
            grammarManager.SRE.RecognizeAsync(RecognizeMode.Multiple);
            double value = ((double)currentGrammar / (double)grammarManager.pGrammars.Length) * 100;
            mainWindow.progressBar.Value = value < 100.0 ? value : 100.0;
        }

        private List<Product> loadProducts()
        {
            products = new List<Product>();
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
                Product product1 = new Product() { Name = "Gitara", Price = 250 };
                Product product2 = new Product() { Name = "Perkusja", Price = 350 };
                Product product3 = new Product() { Name = "Skrzypce", Price = 400 };
                Product product4 = new Product() { Name = "Dzwoneczki", Price = 35 };
                Product product5 = new Product() { Name = "Bas", Price = 300 };
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


        public void StartShopping()
        {
            var text = "Siema?";
            LogDialogSystem(text);
            grammarManager.StartRecognizing();
        }

        private void LogDialogSystem(string text)
        {
            synthesisManager.StartSpeaking(text);
            mainWindow.LogDialogSystem(text);
        }

        private void LogDialogUser(string text, float confidence)
        {
            mainWindow.LogDialogUser(text, confidence);
        }

    }
}
