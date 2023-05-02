using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TT.Deliveries.Core.Enums;
using TT.Deliveries.Services.Responses;

namespace TT.Deliveries.Services.Contracts
{
    public interface IStateService
    {
        Task<Response> UpdateAsync(Guid Id, DeliveryState state);
        Task<Response<List<Guid>>> ExpireDeliveriesAsync();
    }
}