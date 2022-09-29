using System;
using System.Collections.Generic;

#nullable disable

namespace DemoCrubCore.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal? UnitPrice { get; set; }
        public string Num { get; set; }

        public virtual SaleOrder Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
