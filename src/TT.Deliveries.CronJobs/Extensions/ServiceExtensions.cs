using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Deliveries.CronJobs.Options;
using TT.Deliveries.CronJobs.Services;

namespace TT.Deliveries.CronJobs.Extensions
{
    public static class ScheduledServiceExtensions
    {
        public static IServiceCollection AddCronJob<T>(
            this IServiceCollection services,
            IScheduleConfig<T> options) where T : CronJobService
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configurations.");
            }

            if (string.IsNullOrWhiteSpace(options.CronExpression))
            {
                throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronExpression), @"Empty Cron Expression is not allowed.");
            }

            services.AddSingleton<IScheduleConfig<T>>(options);
            services.AddHostedService<T>();
            return services;
        }
    }
}
