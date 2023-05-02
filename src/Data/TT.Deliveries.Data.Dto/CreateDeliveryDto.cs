using System;

namespace TT.Deliveries.Data.Dto
{
    public class CreateDeliveryDto
    {
        public AccessWindowDto AccessWindow { get; set; }
        public RecipientDto Recipient { get; set; }
        public OrderDto Order { get; set; }
    }
}
