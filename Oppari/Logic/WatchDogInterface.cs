using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Oppari.Logic
{
    public interface IWatchDog
    {
        Task UpdateWatchDogErrors(string methodName, int errorCount);
    }
}
