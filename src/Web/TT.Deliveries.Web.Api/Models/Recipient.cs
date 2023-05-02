using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TT.Deliveries.Web.Api.Models
{
    public class Recipient
    {
        /// <summary>
        /// Recipient Full Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Recipient's Address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Recipient's Email Address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Recipients phone number
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}
