using System;
using TT.Deliveries.Core.Enums;

namespace TT.Deliveries.Data.Dto
{
    public class DeliveryDto
    {
        public Guid Id { get; set; }
        public DeliveryState State { get; set; }
        public AccessWindowDto AccessWindow { get; set; }
        public RecipientDto Recipient { get; set; }
        public OrderDto Order { get; set; }
    }
}
