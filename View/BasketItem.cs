using SpeechSynthesis.model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SpeechSynthesis
{
    public class BasketItem
    {
        public Product Product { get; set; }
        public double Total {
            get => Price * Quantity;
        }
        public string Name {
            get => Product.Name;
        }
        public double Price {
            get => Product.Price;
        }
        public int Quantity { get; set; }
    }
}
