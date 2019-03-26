using Microsoft.AspNetCore.SignalR;
using Oppari.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oppari.Hubs
{
    public class WatchDogHub : Hub
    {
        public async Task UpdateWatchDogErrors()
        {
            //TODO - päivitä kanta
            Random rng = new Random();
            int number = rng.Next(1, 10);
            await Clients.All.SendAsync("ReceiveWatchDogErrorsUpdate", number);
        }
    }
}
