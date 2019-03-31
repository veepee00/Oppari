using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Oppari.Hubs;
using Oppari.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Oppari.Logic
{
    public class WatchDogChecks
    {
        //public interface IChecker
        //{
        //    void Check();
        //}

        //public abstract class CheckerBase : IChecker
        //{
        //    protected string[] Arguments;

        //    public CheckerBase(string[] arguments)
        //    {
        //        Arguments = arguments;
        //    }


        //    public virtual void Check()
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        //public class Check1 : CheckerBase
        //{
        //    public Check1(string[] arguments) : base(arguments)
        //    {
        //    }

        //    public override void Check()
        //    {

        //    }
        //}

        //public class Check2: CheckerBase
        //{
        //    public Check2(string[] arguments) : base(arguments)
        //    {
        //    }


        //    public override void Check()
        //    {

        //    }
        //}
        IHubContext<WatchDogHub,IWatchDog> _hubContext;
        public WatchDogChecks(IHubContext<WatchDogHub, IWatchDog> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task CheckOldFilesFromDirectory(string folder, string mask, int time = -10)
        {
            //Montako tiedostoa löytyy, joihin ei ole koskettu {time} minuutin sisällä
            if (String.IsNullOrEmpty(folder) || String.IsNullOrEmpty(mask))
            {
                throw new ArgumentNullException();
            }

            List<string> files = new List<string>(Directory.GetFiles(folder));
            List<string> returnFiles = new List<string>();

            foreach (string file in files)
            {
                FileInfo fi = new FileInfo(file);
                if ((fi.LastWriteTime < DateTime.Now.AddMinutes(time)) && fi.Extension == mask)
                {
                    Console.WriteLine(fi.Extension);
                    returnFiles.Add(file);
                }
            }
            if (returnFiles.Count() > 0)
            {
                //jippii
            }
            else
            {
                WatchDogErrorModel wdError = new WatchDogErrorModel("CheckOldFilesFromDirectory", $"Method returned 0 files.", 50, DateTime.Now, folder, mask);
                WatchDogHandler watchDogHandler = new WatchDogHandler(_hubContext);
                await watchDogHandler.AddWatchDogErrorToDb(wdError);
            }
        }

        public async Task CheckSqlQueries(string query)
        {
            //Kantakysely
            if (String.IsNullOrEmpty(query))
            {
                throw new ArgumentNullException();
            }

            using (var context = new WatchDogErrorContext())
            {
                var errors = context.WatchDogErrors.FromSql(query).ToList();
                //lisää ehto
                WatchDogErrorModel wdError = new WatchDogErrorModel("CheckSqlQueries", $"Method returned 0 rows.", 50, DateTime.Now, query);
                WatchDogHandler watchDogHandler = new WatchDogHandler(_hubContext);
                await watchDogHandler.AddWatchDogErrorToDb(wdError);
            }
        }
    }
}
