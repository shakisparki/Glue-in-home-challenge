using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TT.Deliveries.Web.Api.Models
{
    public class Order
    {
        /// <summary>
        /// Order Number
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// Sending Partner Name
        /// </summary>
        public string Sender { get; set; }
    }
}
