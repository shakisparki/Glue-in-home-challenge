

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TT.Deliveries.Data.Dto;
using TT.Deliveries.Data.Entities;
using TT.Deliveries.Services.Responses;

namespace TT.Deliveries.Services.Contracts
{
    public interface IDeliveryService
    {
        Task<Response<DeliveryDto>> CreateAsync(CreateDeliveryDto delivery);
        Task<Response<DeliveryDto>> ReadAsync(Guid Id);
        Task<Response<List<DeliveryDto>>> ReadAllAsync();
        Task<Response<DeliveryDto>> UpdateAsync(UpdateDeliveryDto delivery);
        Task<Response<List<Response>>> BulkUpdateAsync(List<UpdateDeliveryDto> deliveries);
        Task<Response> DeleteAsync(Guid Id);
        Task<Response> BulkDeleteAsync();
    }
}