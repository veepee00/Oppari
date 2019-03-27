using Microsoft.AspNetCore.SignalR;
using Oppari.Logic;
using Oppari.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oppari.Hubs
{
    public class WatchDogHub : Hub<IWatchDog>
    {
        public async Task RandomButton()
        {
            Random rng = new Random();
            int randomNumber = rng.Next(1, 10);
            await Clients.All.RandomButton(randomNumber);
        }
    }
}
