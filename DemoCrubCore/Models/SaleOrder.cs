using System;
using System.Collections.Generic;

#nullable disable

namespace DemoCrubCore.Models
{
    public partial class SaleOrder
    {
        public SaleOrder()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public decimal? Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
