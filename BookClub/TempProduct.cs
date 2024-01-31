using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookClub
{
    public class TempProduct
    {
        public Product Product { get; set; }
        public int amount { get; set; }

        public TempProduct(Product product, int amount) 
        {
            Product = product;
            this.amount = amount;
        }
    }
}
