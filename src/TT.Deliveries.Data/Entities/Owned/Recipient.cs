using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TT.Deliveries.Data.Entities
{
    [Owned]
    public class Recipient
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
