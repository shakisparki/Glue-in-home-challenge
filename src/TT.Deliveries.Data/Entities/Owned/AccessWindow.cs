using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TT.Deliveries.Data.Entities
{
    [Owned]
    public class AccessWindow
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
