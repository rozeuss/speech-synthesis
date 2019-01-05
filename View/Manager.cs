using SpeechSynthesis.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SpeechSynthesis
{
    public class Manager
    {
        private Synthesis.SynthesisManager synthesisManager;
        private Grammar.GrammarManager grammarManager;

        public Manager()
        {
            using (var db = new DatabaseModel())
            {
                if (db.Products.Count() == 0)
                    InitalizeDb();
            }

            synthesisManager = new Synthesis.SynthesisManager();
            grammarManager = new Grammar.GrammarManager();

            synthesisManager.Speak("dupa ja pierdole");
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

        public void Printing(string str)
        {
            Console.WriteLine(str);
        }
    }
}
