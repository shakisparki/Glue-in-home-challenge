using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TT.Deliveries.CronJobs.Extensions;
using TT.Deliveries.CronJobs.Options;
using TT.Deliveries.Data;
using TT.Deliveries.Services;
using TT.Deliveries.Services.Contracts;

namespace TT.Deliveries.CronJobs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Write console banner
            WriteBanner();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            new HostBuilder().ConfigureAppConfiguration((ctx, builder) =>
            {
                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                builder.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
                builder.AddEnvironmentVariables();
                builder.AddCommandLine(args);
            })
            .ConfigureServices((ctx, services) =>
            {
                services.AddDbContext<DeliveryDbContext>(options =>
                    options.UseSqlite(Environment.ExpandEnvironmentVariables(ctx.Configuration.GetConnectionString("DeliveryDatabase")))
                );
                services.AddTransient<IStateService, StateService>();
                var cronOptions = ctx.Configuration.GetSection("CronJob").Get<ScheduleConfig<ExpireDeliveryStateJob>>();
                services.AddCronJob(cronOptions);
                
            })
            .ConfigureLogging((ctx, logging) =>
            {
                logging.AddConfiguration(ctx.Configuration.GetSection("Logging"));
                logging.AddConsole();
                logging.AddDebug();
                logging.AddEventSourceLogger();
            });

        private static void WriteBanner()
        {
            string banner = @"   ______                             __          __
  / ____/___  ____  ____ __________ _/ /______   / /
 / /   / __ \/ __ \/ __ `/ ___/ __ `/ __/ ___/  / / 
/ /___/ /_/ / / / / /_/ / /  / /_/ / /_(__  )  /_/  
\__________/_/ /_/\__, /_/ __\__,_/\__/____/  (_)   
   / __ \(_)___  ////_/   / /   (_)_______  ____    
  / / / / / __ \/ __ `/  / /   / / ___/ _ \/ __ \   
 / /_/ / / / / / /_/ /  / /___/ / /  /  __/ / / /   
/_____/_/_/ /_/\__, /  /_____/_/_/   \___/_/ /_/    
              /____/                                ";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(banner);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nBy Shakirudeen Lasisi - For Glue Home");
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
