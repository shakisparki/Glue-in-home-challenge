using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TT.Deliveries.Core.Enums;
using TT.Deliveries.Data;
using TT.Deliveries.Data.Dto;
using TT.Deliveries.Data.Entities;
using TT.Deliveries.Services.Contracts;
using TT.Deliveries.Services.Responses;

namespace TT.Deliveries.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly DeliveryDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<DeliveryService> _logger;
        public DeliveryService(DeliveryDbContext dbContext, IMapper mapper, ILogger<DeliveryService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Response<DeliveryDto>> CreateAsync(CreateDeliveryDto delivery)
        {
            try
            {
                var newDelivery = _mapper.Map<Delivery>(delivery);
                newDelivery.State = DeliveryState.Created;
                _dbContext.Delivery.Add(newDelivery);
                await _dbContext.SaveChangesAsync();
                var newDeliveryDto = _mapper.Map<DeliveryDto>(newDelivery);
                return new Response<DeliveryDto>(newDeliveryDto, Errors.Created);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError($"Delivery creation failed: {e}");
                return new Response<DeliveryDto>(Errors.Fail);
            }
        }

        public async Task<Response<DeliveryDto>> ReadAsync(Guid Id)
        {
            var stored = await _dbContext.Delivery.FindAsync(Id);
            if (stored is null)
            {
                return new Response<DeliveryDto>(Errors.NotFound);
            }
            return new Response<DeliveryDto>(_mapper.Map<DeliveryDto>(stored));
        }

        public async Task<Response<List<DeliveryDto>>> ReadAllAsync()
        {
            var stored = await _dbContext.Delivery.Select(x => _mapper.Map<DeliveryDto>(x)).ToListAsync();
            var code = stored.Any() ? Errors.Pass : Errors.NoContent;
            return new Response<List<DeliveryDto>>(stored, code);
        }

        public async Task<Response<DeliveryDto>> UpdateAsync(UpdateDeliveryDto delivery)
        {
            try
            {
                var stored = await _dbContext.Delivery.FindAsync(delivery.Id);
                if (stored is null)
                {
                    return new Response<DeliveryDto>(Errors.NotFound);
                }
                _mapper.Map(delivery, stored);
                _dbContext.Delivery.Update(stored);
                await _dbContext.SaveChangesAsync();
                return new Response<DeliveryDto>(_mapper.Map<DeliveryDto>(stored));
            }
            catch (DbUpdateException e)
            {
                _logger.LogError($"Delivery update failed: {e}");
                return new Response<DeliveryDto>(Errors.Fail);
            }
            
        }

        public async Task<Response<List<Response>>> BulkUpdateAsync(List<UpdateDeliveryDto> deliveries)
        {
            var responses = new List<Response>();
            var updated = new List<Delivery>();

            try
            {
                foreach (var delivery in deliveries)
                {
                    var stored = await _dbContext.Delivery.FindAsync(delivery.Id);
                    if (stored is null)
                    {
                        responses.Add(new Response<Guid>(delivery.Id, Errors.NotFound));
                    }
                    else
                    {
                        responses.Add(new Response<Guid>(delivery.Id, Errors.Pass));
                        updated.Add(_mapper.Map(delivery, stored));
                    }
                }

                _dbContext.Delivery.UpdateRange(updated);
                await _dbContext.SaveChangesAsync();
                var status = responses.Any() ? Errors.Pass : Errors.NoContent;
                return new Response<List<Response>>(responses, status);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError($"Delivery bulk update failed: {e}");
                responses.Add(new Response(Errors.Fail));
                return new Response<List<Response>>(responses);
            }
        }

        public async Task<Response> BulkDeleteAsync()
        {
            try
            {
                var stored = await _dbContext.Delivery.ToListAsync();
                _dbContext.Delivery.RemoveRange(stored);
                await _dbContext.SaveChangesAsync();
                return new Response(Errors.NoContent);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError($"Delivery cbulk delete failed: {e}");
                return new Response(Errors.Fail);
            }
        }

        public async Task<Response> DeleteAsync(Guid Id)
        {
            try
            {
                var stored = await _dbContext.Delivery.FindAsync(Id);
                if (stored is null)
                {
                    return new Response(Errors.NotFound);
                }

                _dbContext.Delivery.Remove(stored);
                await _dbContext.SaveChangesAsync();
                return new Response(Errors.NoContent);
            }
            catch (DbUpdateException e)
            {
                _logger.LogError($"Delivery deletion failed: {e}");
                return new Response(Errors.Fail);
            }

        }
    }
}
