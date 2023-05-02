using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using TT.Deliveries.Core.Enums;

namespace TT.Deliveries.Data.Entities
{
    public class Delivery
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DeliveryState State { get; set; }
        public AccessWindow AccessWindow { get; set; }
        public Recipient Recipient { get; set; }
        public Order Order { get; set; }
    }
}
