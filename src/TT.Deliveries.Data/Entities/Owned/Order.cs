using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TT.Deliveries.Data.Entities
{
    [Owned]
    public class Order
    {
        public string OrderNumber { get; set; }
        public string Sender { get; set; }
    }
}
