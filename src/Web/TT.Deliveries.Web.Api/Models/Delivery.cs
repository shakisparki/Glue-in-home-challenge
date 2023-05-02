using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TT.Deliveries.Core.Enums;

namespace TT.Deliveries.Web.Api.Models
{

    public class Delivery
    {
        /// <summary>
        /// Delivery State
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Delivery Valid Window
        /// </summary>
        public AccessWindow AccessWindow { get; set; }

        /// <summary>
        /// Delivery Recipient
        /// </summary>
        public Recipient Recipient { get; set; }

        /// <summary>
        /// Delivery Order
        /// </summary>
        public Order Order { get; set; }
    }


}
