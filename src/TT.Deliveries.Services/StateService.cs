using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TT.Deliveries.Core.Enums;
using TT.Deliveries.Data;
using TT.Deliveries.Data.Entities;
using TT.Deliveries.Services.Contracts;
using TT.Deliveries.Services.Responses;

namespace TT.Deliveries.Services
{
    public class StateService : IStateService
    {
        private readonly DeliveryDbContext _dbContext;
        private readonly ILogger<StateService> _logger;
        public StateService(DeliveryDbContext dbContext, ILogger<StateService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Response> UpdateAsync(Guid Id, DeliveryState state)
        {
            //Individual/Separate service methods for each state operation e.g. Approve, Cancelled etc.
            //can be created if logic is bound to change or become more complex.

            try
            {
                var stored = await _dbContext.Delivery.FindAsync(Id);
                if (stored is null)
                {
                    return new Response<Delivery>(Errors.NotFound);
                }

                if (!ValidateState(stored.State, state))
                {
                    return new Response(Errors.BadState);
                }

                if (!ValidateAccess(stored.AccessWindow, state))
                {
                    return new Response(Errors.NoAccess);
                }

                stored.State = state;
                _dbContext.Delivery.Update(stored);
                await _dbContext.SaveChangesAsync();
                return new Response();
            }
            catch (DbUpdateException e)
            {
                _logger.LogError($"Delivery state update failed: {e}");
                return new Response(Errors.Fail);
            }
        }
        public async Task<Response<List<Guid>>> ExpireDeliveriesAsync()
        {
            try
            {
                var stored = await _dbContext.Delivery
                    .Where(x => x.AccessWindow.EndTime < DateTime.UtcNow
                        && x.State != DeliveryState.Expired)
                    .ToListAsync();
                if (stored is null || !stored.Any())
                {
                    return new Response<List<Guid>>(Errors.NoContent);
                }
  
                stored.ForEach(x=> x.State = DeliveryState.Expired);
                _dbContext.Delivery.UpdateRange(stored);
                await _dbContext.SaveChangesAsync();
                return new Response<List<Guid>>(stored.Select(x=>x.Id).ToList());
            }
            catch (DbUpdateException e)
            {
                _logger.LogError($"Delivery expiration failed: {e}");
                return new Response<List<Guid>>(Errors.Fail);
            }
        }

        #region Validation
        private static bool ValidateAccess(AccessWindow accessWindow, DeliveryState desired) => desired switch
        {
            DeliveryState.Approved or
            DeliveryState.Cancelled => accessWindow.EndTime > DateTime.UtcNow,
            DeliveryState.Expired => accessWindow.EndTime < DateTime.UtcNow,
            DeliveryState.Completed => accessWindow.StartTime < DateTime.UtcNow && accessWindow.EndTime > DateTime.UtcNow,
            _ => false
        };

        private static bool ValidateState(DeliveryState present, DeliveryState desired) => desired switch
        {
            DeliveryState.Approved =>
                present == DeliveryState.Created || present == DeliveryState.Approved,
            DeliveryState.Completed =>
                present == DeliveryState.Approved || present == DeliveryState.Completed,
            DeliveryState.Cancelled =>
                present == DeliveryState.Created || present == DeliveryState.Approved
                || present == DeliveryState.Cancelled,
            DeliveryState.Expired => true,
            _ => false
        };
        #endregion
    }
}
