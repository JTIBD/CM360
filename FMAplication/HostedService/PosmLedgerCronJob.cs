using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FMAplication.Domain;
using FMAplication.Domain.Sales;
using FMAplication.Domain.SPWisePOSMLedgers;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using FMAplication.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FMAplication.HostedService
{
    public class PosmLedgerCronJob : CronJobService
    {
        private readonly ILogger<PosmLedgerCronJob> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public PosmLedgerCronJob(IScheduleConfig<PosmLedgerCronJob> config,  
            ILogger<PosmLedgerCronJob> logger,
            IServiceScopeFactory serviceScopeFactory, IWebHostEnvironment environment)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _environment = environment;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 starts.");
            Debug.WriteLine($"CronJob 1 starts.");

            return base.StartAsync(cancellationToken);
        }

        public override async Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} CronJob 1 is working.");
            Debug.WriteLine($"{DateTime.Now:dd:MM:yyyy:hh:mm:ss} CronJob 1 is working.");

            await AddOrUpdateForCurrentDate();
            await Task.CompletedTask;
        }

        public async Task AddOrUpdateForCurrentDate()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _context = scope.ServiceProvider.GetService<ApplicationDbContext>();

                var todaysDate = DateTime.UtcNow.BangladeshDateInUtc();
                var previousDay = todaysDate.AddDays(-1);
                var previousDayledgers = _context.SpWisePosmLedgers.Where(x => x.Date == previousDay).ToList();

                if (!previousDayledgers.Any())
                {
                    var spWiseLedgerService = scope.ServiceProvider.GetService<ISPWisePosmLedgerService>();
                    await spWiseLedgerService.InsertMissingLedgers(previousDay);
                    previousDayledgers = _context.SpWisePosmLedgers.Where(x => x.Date == previousDay).ToList();
                }

                var todayData = _context.SpWisePosmLedgers.Any(x => x.Date == todaysDate);

                if (previousDayledgers.Any() && !todayData)
                {
                    var list = previousDayledgers.Select(x => new SPWisePOSMLedger
                    {
                        SalesPointId = x.SalesPointId,
                        PosmProductId = x.PosmProductId,
                        OpeningStock = x.ClosingStock,
                        ClosingStock = x.ClosingStock,
                        ExecutedStock = 0,
                        ReceivedStock = 0,
                        Date = todaysDate
                    });
                    _context.SpWisePosmLedgers.AddRange(list);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred in AddOrUpdateForCurrentDate. Message: {ex.Message}");
                _logger.LogError($"AddOrUpdateForCurrentDate: errormessage: {ex.Message}");
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("CronJob 1 is stopping.");
            Debug.WriteLine($"CronJob 1 is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
