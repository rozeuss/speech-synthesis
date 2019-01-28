﻿using Microsoft.Speech.Recognition;
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
        private BasketItem basketItem;

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

            SetupProductGrammarDict();
            this.mainWindow = mainWindow;
        }

        private void SetupProductGrammarDict()
        {
            foreach (KeyValuePair<int, string> entry in grammarManager.WhichProductGrammar.productIdGrammarDictionary)
            {
                foreach (Product p in products)
                {
                    if (p.ProductId == entry.Key)
                    {
                        ProductGrammarDict.Add(p, entry.Value);
                    }
                }
            };
        }

        private void GrammarManager_SpeechRecognized(object sender, EventArgs e)
        {
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
                    switch (currentGrammar)
                    {
                        case 0:
                            ManageWhichProductGrammar(args);
                            SwitchGrammar();
                            break;
                        case 1:
                            if (args.Result.Text.Equals("nie"))
                            {
                                LogDialogSystem("Podaj imię i nazwisko.");
                                SwitchGrammar();
                            }
                            else if (args.Result.Text.Equals("tak"))
                            {
                                currentGrammar = -1;
                                LogDialogSystem("To co do tego?");
                                SwitchGrammar();
                            }
                            else
                            {
                                ManageHowManyProductsGrammar(args);
                            }
                            break;
                        case 2:
                            ManagePersonNameGrammar(args);
                            SwitchGrammar();
                            break;
                        case 3:
                            ManageAddressGrammar(args);
                            SwitchGrammar();
                            break;
                        case 4:
                            grammarManager.SRE.RecognizeAsyncStop();
                            grammarManager.SRE.UnloadAllGrammars();
                            //TODO zakonczenie programu, czyli np wyswietlenie
                            //wyniku po pobraniu z bazy i posprzatanie SRE i TTS
                            break;
                    }
                }
            }
        }

        private void ManageAddressGrammar(SpeechRecognizedEventArgs args)
        {
            var value = args.Result.Text;
            mainWindow.addressTextBox.Text = value;
            saveOrder();
            LogDialogSystem("Dziękuję. Zamówienie zostało złożone!");
        }

        private void saveOrder()
        {
            using (var db = new DatabaseModel())
            {
                var productsOrdersList = new List<ProductsOrders>();
                foreach (var basketItem in mainWindow.Basket)
                {
                    var product = db.Products.Where(p => p.ProductId == basketItem.Product.ProductId).FirstOrDefault();
                    productsOrdersList.Add(new ProductsOrders()
                    {
                        Product = product,
                        Quantity = basketItem.Quantity
                    });
                }
                var order = new Order()
                {
                    Person = mainWindow.personTextBox.Text,
                    Address = mainWindow.addressTextBox.Text,
                    Total = Convert.ToDouble(mainWindow.totalTextBox.Text),
                    ProductsOrders = productsOrdersList
                };
                db.Orders.Add(order);
                db.SaveChanges();
            }
        }

        private void ManagePersonNameGrammar(SpeechRecognizedEventArgs args)
        {
            var value = args.Result.Text;
            LogDialogSystem($"Witaj {value}.");
            mainWindow.personTextBox.Text = value;
            LogDialogSystem("Podaj adres wysyłki.");
        }

        private void ManageHowManyProductsGrammar(SpeechRecognizedEventArgs args)
        {
            var value = args.Result.Text;
            var key = grammarManager.HowManyProductsGrammar.QuantityDict
                .FirstOrDefault(x => x.Value == value).Key;
            //var item = mainWindow.basketListView.Items[mainWindow.basketListView.Items.Count - 1];
            //(item as BasketItem).Quantity = key;
            var lastItem = mainWindow.Basket[mainWindow.Basket.Count - 1];
            lastItem.Quantity = key;
            var total = Convert.ToDouble(mainWindow.totalTextBox.Text);
            total = total + lastItem.Price * lastItem.Quantity;
            mainWindow.totalTextBox.Text = total.ToString();
            //TODO dlaczego nie musi być ta linijka pomimo bindingu?
            mainWindow.basketListView.Items.Refresh();
            LogDialogSystem($"Czyli chcesz {value} sztuk.");
            LogDialogSystem("Czy chcesz coś jeszcze kupić?");
            grammarManager.SRE.RecognizeAsyncCancel();
            grammarManager.SRE.UnloadAllGrammars();
            grammarManager.SRE.LoadGrammar(grammarManager.YesNoGrammar.grammar);
            grammarManager.SRE.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void ManageWhichProductGrammar(SpeechRecognizedEventArgs args)
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
                        basketItem = new BasketItem() { Product = p.Key };
                        //mainWindow.basketListView.Items.Add(basketItem);
                        mainWindow.Basket.Add(basketItem);
                    }
                });
            }
            if (!String.IsNullOrEmpty(appendText))
            {
                LogDialogSystem($"Czyli chcesz {appendText}.");
            };
            String text = "Ile sztuk?";
            LogDialogSystem(text);
        }

        private void SwitchGrammar()
        {
            int nextGrammar = ++currentGrammar;
            grammarManager.SRE.RecognizeAsyncCancel();
            grammarManager.SRE.UnloadAllGrammars();
            grammarManager.SRE.LoadGrammar(grammarManager.pGrammars[nextGrammar >= grammarManager.pGrammars.Length ? 0 : nextGrammar]);
            grammarManager.SRE.RecognizeAsync(RecognizeMode.Multiple);
            double value = ((double)currentGrammar / (double)grammarManager.pGrammars.Length) * 100;
            mainWindow.progressBar.Value = value < 100.0 ? value : 100.0;
            Array.ForEach(mainWindow.stageLabels, label => label.Opacity = 0.2);
            if (currentGrammar < mainWindow.stageLabels.Length)
                mainWindow.stageLabels[currentGrammar].Opacity = 1.0;
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
                var products = new List<Product>
                {
                    product1,
                    product2,
                    product3
                };
                var orderProducts = new List<Product>(products);
                products.Add(product4);
                products.Add(product5);
                db.Products.AddRange(products);
                db.SaveChanges();
            }
        }

        public void StartShopping()
        {
            var text = "W czym mogę pomóc?";
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
