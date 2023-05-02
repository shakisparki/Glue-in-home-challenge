using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TT.Deliveries.Web.Api.Models
{
    public class AccessWindow
    {
        /// <summary>
        /// Delivery Start Time in UTC
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Delivery End Time in UTC
        /// </summary>
        public DateTime EndTime { get; set; }
    }
}
