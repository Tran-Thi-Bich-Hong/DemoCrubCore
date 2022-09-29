using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoCrubCore.Data;

namespace DemoCrubCore.Models
{
    public class Cart
    {
        private saledbContext dt = new saledbContext();
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public decimal Amount { get { return Price * Quantity; } }
        
        public Cart(int ProductId)
        {
            this.ProductId = ProductId;
            Product p = dt.Products.Single(n => n.Id == ProductId);
            ProductName = p.Name;
            Price = (decimal)p.Price;
            Quantity = 1;
        }
    }
}
