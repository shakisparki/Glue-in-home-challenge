using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TT.Deliveries.Core.Enums;

namespace TT.Deliveries.Web.Api.Models
{

    public class BaseDelivery
    {
        public AccessWindow AccessWindow { get; set; }
        public Recipient Recipient { get; set; }
        public Order Order { get; set; }
    }


}
