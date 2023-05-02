using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TT.Deliveries.Core.Enums;

namespace TT.Deliveries.Web.Api.Models
{

    public class BulkDelivery : BaseDelivery
    {
        public Guid Id { get; set; }
    }
}
