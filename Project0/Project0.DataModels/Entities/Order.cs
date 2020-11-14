using System;
using System.Collections.Generic;

#nullable disable

namespace Project0.DataModels.Entities
{
    public partial class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int LocationId { get; set; }
        public DateTime Date { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Location Location { get; set; }
    }
}
