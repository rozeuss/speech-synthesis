using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechSynthesis.model
{
    public class ProductsOrders
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductsOrdersId { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
    }
}
