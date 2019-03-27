using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Oppari.Hubs;
using Oppari.Models;

namespace Oppari.Logic
{
    public class WatchDogHandler : BackgroundService
    {
        IHubContext<WatchDogHub, IWatchDog> _hubContext;
        public bool watchDogRunning { get; set; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //WatcgDogTimer
            while (!stoppingToken.IsCancellationRequested)
            {
                ExecuteWatchDogTests();
                await Task.Delay(10000);
            }
        }

        public WatchDogHandler(IHubContext<WatchDogHub,IWatchDog> hubContext)
        {
            _hubContext = hubContext;
        }

        public void ExecuteWatchDogTests()
        {
            try
            {
                if (Startup.watchDogTests.Count == 0)
                {
                    //lisää cancellationtoken
                    throw new Exception("Suoritettavia testejä ei löytynyt!");
                }
                foreach (var method in Startup.watchDogTests)
                {
                    method.Invoke();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {

            }
        }

        public async Task AddWatchDogErrorToDb(WatchDogErrorModel wdError)
        {
            if (wdError == null)
            {
                throw new ArgumentNullException();
            }

            using (var context = new WatchDogErrorContext())
            {
                context.WatchDogErrors.Add(wdError);
                context.SaveChanges();

                await _hubContext.Clients.All.UpdateWatchDogErrors(context.WatchDogErrors.ToList());
            }
        }
    }
}
