using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TT.Deliveries.CronJobs.Options
{
    public class ScheduleConfig<T> : IScheduleConfig<T>
    {
        public string CronExpression { get; set; } = string.Empty;
        public TimeZoneInfo TimeZoneInfo { get; set; } = TimeZoneInfo.Local;
    }
}
