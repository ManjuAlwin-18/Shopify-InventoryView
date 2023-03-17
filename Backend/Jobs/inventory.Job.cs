using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Example_FrontEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;


namespace Example_FrontEnd.Backend.Job
{

    public class TimedHostedService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private Timer _timer;
        private readonly IInventoryService _inventoryService;

        public TimedHostedService(
            ILogger<TimedHostedService> logger,
            IInventoryService inventoryService)
        {
            _logger = logger;
            _inventoryService = inventoryService;
        }


        /// <summary>
        /// Starts cronjob
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");


            _timer = new Timer(DoWork, null, TimeSpan.Zero,
               TimeSpan.FromMinutes(1));
            return Task.CompletedTask;
        }


        /// <summary>
        /// Will be called, when timespan is full.
        /// 
        /// Will call SetInventory from InventoryService
        /// </summary>
        /// <param name="state"></param>
        private void DoWork(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            Console.WriteLine("Timed Hosted Service running.");

            _inventoryService.SetInventory();

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
        }


        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }

}
