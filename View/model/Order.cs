﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechSynthesis.model
{
    public class Order
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public string Person { get; set; }
        public string Address { get; set; }
        public virtual List<ProductsOrders> ProductsOrders { get; set; }
        public double Total { get; set; }
    }
}
