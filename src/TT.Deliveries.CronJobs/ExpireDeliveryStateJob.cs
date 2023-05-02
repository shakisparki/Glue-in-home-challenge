using Microsoft.Extensions.Logging;
using TT.Deliveries.CronJobs.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using TT.Deliveries.CronJobs.Options;
using TT.Deliveries.Services.Contracts;

namespace TT.Deliveries.CronJobs
{
    public class ExpireDeliveryStateJob : CronJobService
    {
        private readonly ILogger<ExpireDeliveryStateJob> _logger;
        private readonly IStateService _stateService;

        public ExpireDeliveryStateJob(
            IScheduleConfig<ExpireDeliveryStateJob> config,
            ILogger<ExpireDeliveryStateJob> logger,
            IStateService stateService)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _stateService = stateService;

        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("DeliveryStateJob Starts.");
            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            var expired = await _stateService.ExpireDeliveriesAsync();

            if (expired.Error == Core.Enums.Errors.Fail)
            {
                _logger.LogInformation($"{DateTime.Now:T} Something went wrong.");
            }
            if (expired.Error == Core.Enums.Errors.NoContent)
            {
                _logger.LogInformation($"{DateTime.Now:T} No Deliveries Expired");
            }
            else if (expired.Error == Core.Enums.Errors.Pass)
            {
                _logger.LogInformation($"{DateTime.Now:T} Expired Deliveries : {String.Join(',', expired.Value)}");
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("DeliveryStateJob is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
