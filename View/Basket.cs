using SpeechSynthesis.model;
using System.Collections.Generic;

namespace SpeechSynthesis
{
    public class Basket
    {
        public Dictionary<Product, int> ProductQuantityDict { get; set; }
        public double Total { get; set; }
    }
}
