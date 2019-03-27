using Oppari.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oppari.Logic
{
    public interface IWatchDog
    {
        Task UpdateWatchDogErrors(List<WatchDogErrorModel> watchDogErrorList);
        Task RandomButton(int randomNumber);
    }
}
